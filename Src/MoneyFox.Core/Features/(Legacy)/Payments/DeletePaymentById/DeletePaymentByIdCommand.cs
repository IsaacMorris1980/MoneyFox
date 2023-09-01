namespace MoneyFox.Core.Features._Legacy_.Payments.DeletePaymentById;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeletePaymentById
{
    public record Command(int PaymentId, bool DeleteRecurringPayment = false) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var entityToDelete = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .SingleOrDefaultAsync(predicate: x => x.Id == command.PaymentId, cancellationToken: cancellationToken);

            if (entityToDelete == null)
            {
                return;
            }

            entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
            entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);
            if (command.DeleteRecurringPayment && entityToDelete.IsRecurring)
            {
                var recurringTransaction = await appDbContext.RecurringTransactions.SingleAsync(
                    predicate: rt => rt.RecurringTransactionId == entityToDelete.RecurringTransactionId,
                    cancellationToken: cancellationToken);

                recurringTransaction.EndRecurrence();
            }

            appDbContext.Payments.Remove(entityToDelete);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
