﻿using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Views.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SelectCategoryListView
    {
        public SelectCategoryListView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<SelectCategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();

            var selectCategoryListViewModel = DataContext as SelectCategoryListViewModel;
            if (selectCategoryListViewModel != null)
            {
                selectCategoryListViewModel.SearchText = string.Empty;
                selectCategoryListViewModel.Search();
            }
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ((SelectCategoryListViewModel) DataContext).DoneCommand.Execute();
            }

            base.OnKeyDown(e);
        }
    }
}