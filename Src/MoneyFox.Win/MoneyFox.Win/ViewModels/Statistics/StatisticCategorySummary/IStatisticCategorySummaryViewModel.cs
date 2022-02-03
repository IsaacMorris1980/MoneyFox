﻿using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary
{
    public interface IStatisticCategorySummaryViewModel
    {
        ObservableCollection<CategoryOverviewViewModel> CategorySummary { get; }
        bool HasData { get; }
        IncomeExpenseBalanceViewModel IncomeExpenseBalance { get; set; }
        RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand { get; }
    }
}