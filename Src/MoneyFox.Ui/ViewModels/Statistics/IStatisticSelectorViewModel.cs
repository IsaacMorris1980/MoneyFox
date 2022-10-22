﻿namespace MoneyFox.Ui.ViewModels.Statistics;

using CommunityToolkit.Mvvm.Input;
using MoneyFox.Ui.Views.Statistics.Selector;

public interface IStatisticSelectorViewModel
{
    List<StatisticSelectorTypeViewModel> StatisticItems { get; }

    RelayCommand<StatisticSelectorTypeViewModel> GoToStatisticCommand { get; }
}
