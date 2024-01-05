namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Dashboard;

internal sealed class WelcomeViewModel(ISettingsFacade facade, INavigationService navigationService) : NavigableViewModel
{
    public AsyncRelayCommand NextStepCommand => new(() => navigationService.GoTo<SetupAccountsViewModel>());

    public async Task InitAsync()
    {
        if (facade.IsSetupCompleted)
        {
            await navigationService.GoTo<DashboardViewModel>();
        }
    }
}
