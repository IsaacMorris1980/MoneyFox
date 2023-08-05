namespace MoneyFox.Core.Tests.Commands.Payments.UpdatePaymentById;

using Core.Common.Settings;
using Core.Features._Legacy_.Payments.CreateRecurringPayments;
using Core.Features._Legacy_.Payments.UpdatePayment;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class UpdatePaymentCommandTests : InMemoryTestBase
{
    private readonly UpdatePayment.Handler handler;

    public UpdatePaymentCommandTests()
    {
        var sender = Substitute.For<ISender>();
        var settings = Substitute.For<ISettingsFacade>();
        handler = new(appDbContext: Context, sender: sender, settings: settings);
    }

    [Fact]
    public async Task UpdatePayment_PaymentFound()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();
        payment1.UpdatePayment(date: payment1.Date, amount: 100, type: payment1.Type, chargedAccount: payment1.ChargedAccount);

        // Act
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                Type: payment1.Type,
                Note: payment1.Note!,
                IsRecurring: payment1.IsRecurring,
                CategoryId: payment1.Category?.Id ?? 0,
                ChargedAccountId: payment1.ChargedAccount.Id,
                TargetAccountId: payment1.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: false,
                Recurrence: null,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await Context.Payments.SingleAsync(p => p.Id == payment1.Id)).Amount.Should().Be(payment1.Amount);
    }

    [Fact]
    public async Task ChangeRecurringToNonRecurringPayment()
    {
        // Arrange
        var payment1 = new Payment(
            date: DateTime.Now.AddDays(-1),
            amount: 20,
            type: PaymentType.Expense,
            chargedAccount: new(name: "test", initialBalance: 80));

        payment1.AddRecurringPayment(recurrence: PaymentRecurrence.Daily);
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();

        // Trigger creation of recurring payment transactions
        await new CreateRecurringPaymentsCommand.Handler(Context).Handle(request: new(), cancellationToken: default);

        // Disable recurrence on the payment
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                Type: payment1.Type,
                Note: payment1.Note!,
                IsRecurring: false,
                CategoryId: 0,
                ChargedAccountId: payment1.ChargedAccount.Id,
                TargetAccountId: payment1.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: true,
                Recurrence: PaymentRecurrence.Daily,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        var loadedPayments = Context.Payments.ToList();
        loadedPayments.Should().HaveCount(2);
        loadedPayments.ForEach(x => x.IsRecurring.Should().BeFalse());
    }
}
