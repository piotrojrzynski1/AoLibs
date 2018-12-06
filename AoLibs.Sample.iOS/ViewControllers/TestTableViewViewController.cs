using System;
using System.Collections.ObjectModel;
using AoLibs.Navigation.iOS.Navigation.Attributes;
using AoLibs.Navigation.iOS.Navigation.Controllers;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;
using AoLibs.Utilities.iOS;
using AoLibs.Utilities.iOS.Extensions;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [NavigationPage((int)PageIndex.PageTestTableView, NavigationPageAttribute.PageProvider.Cached, StoryboardName = "Main",
        ViewControllerIdentifier = "TestTableViewViewController")]
    public partial class TestTableViewViewController : ViewControllerBase<TestTableViewViewModel>
    {
        public TestTableViewViewController(IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = new TestTableViewTableViewSource(ViewModel.UserResponses);
        }

        public override void InitBindings()
        {
            AddSingleButton.SetOnClickCommand(ViewModel.AddSingleCommand);
            RemoveSingleButton.SetOnClickCommand(ViewModel.RemoveSingleCommand);
            AddManyButton.SetOnClickCommand(ViewModel.AddManyCommand);
            RemoveManyButton.SetOnClickCommand(ViewModel.RemoveManyCommand);
        }
    }

    public class TestTableViewTableViewSource : ObservableCollectionTableViewSourceFlat<UserResponse>
    {
        public TestTableViewTableViewSource(ObservableCollection<UserResponse> collection) : base(collection)
        {
        }

        public override UITableViewCell GetCellForModel(UserResponse model)
        {
            var cell = new UITableViewCell();
            cell.TextLabel.Text = $"{model.FancyThing} - {model.DateTime}";
            return cell;
        }
    }
}