namespace MoneyFox.Ui;

using System.Threading.Tasks;

public interface INavigationService
{
    Task NavigateToAsync<T>() where T : ContentPage;

    Task NavigateBackAsync();

    Task NavigateBackAsync(string parameterName, string queryParameter);

    Task OpenModalAsync<T>();

    Task GoBackFromModalAsync();
}
