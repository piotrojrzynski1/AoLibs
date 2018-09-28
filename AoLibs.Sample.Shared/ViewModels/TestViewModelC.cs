﻿using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.ViewModels
{
    public class TestViewModelC : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        public TestViewModelC(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public RelayCommand NavigateAWithFirstOccurrence => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.PageA, NavigationBackstackOption.ClearBackstackToFirstOccurence);
        });

        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            _navigationManager.GoBack();
        });
    }
}
