﻿using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCategorySpreadingPage
	{
		public StatisticCategorySpreadingPage ()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.StatisticCategorySpreadingVm;

            Title = Strings.CategorySpreadingTitle;

		    var filterItem = new ToolbarItem
		    {
		        Command = new Command(OpenDialog),
		        Text = Strings.SelectDateLabel,
		        Priority = 0,
		        Order = ToolbarItemOrder.Primary
		    };

		    ToolbarItems.Add(filterItem);
        }

	    private async void OpenDialog()
        {
            await Navigation.PushPopupAsync(new DateSelectionPopup
            {
                BindingContext = ViewModelLocator.SelectDateRangeDialogVm
            });
        }
    }
}