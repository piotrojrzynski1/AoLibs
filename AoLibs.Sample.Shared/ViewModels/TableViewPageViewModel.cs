using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using AoLibs.Utilities.Shared;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.ViewModels
{
    public class TableViewPageViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly Random _random = new Random();

        private SmartObservableCollection<UserResponse> _userResponses = new SmartObservableCollection<UserResponse>();

        public TableViewPageViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public SmartObservableCollection<UserResponse> UserResponses => _userResponses;

        public RelayCommand AddSingleCommand => new RelayCommand(() =>
        {
            _userResponses.Add(CreateRandomModel());
        });

        public RelayCommand RemoveSingleCommand => new RelayCommand(() =>
        {
            if(_userResponses.Count!=0)
                _userResponses.RemoveAt(0);
        });

        public RelayCommand AddManyCommand => new RelayCommand(() =>
        {
            var items = new List<UserResponse>()
            {
                CreateRandomModel(),
                CreateRandomModel(),
                CreateRandomModel(),
            };

            _userResponses.AddRange(items);
        });

        public RelayCommand RemoveManyCommand => new RelayCommand(() =>
        {
            AddManyCommand.Execute(null);
            _userResponses.Clear();
            AddManyCommand.Execute(null);
            AddManyCommand.Execute(null);
        });

        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            _navigationManager.GoBack();
        });

        private UserResponse CreateRandomModel()
        {
            return new UserResponse() 
            { 
                FancyThing = _random.Next(1000, 9999).ToString(),
                DateTime = DateTime.Now 
            };
        }
    }
}
