using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Foundation;
using UIKit;
using System.Linq;

namespace AoLibs.Utilities.iOS.Extensions
{
    public class ObservableCollectionTableViewSourceFlat<TModel> : UITableViewSource
    {
        private UITableView _tableView;
        private ObservableCollection<TModel> _collection;
        private ObservableCollection<TModel> _previousCollection;

        public ObservableCollectionTableViewSourceFlat(ObservableCollection<TModel> collection)
        {
            _collection = collection;
            _previousCollection = new ObservableCollection<TModel>(collection);
            _collection.CollectionChanged+= Collection_CollectionChanged;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return GetCellForModel(_collection[indexPath.Row]);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _collection.Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            _tableView = tableView;
            return 1;
        }

        public virtual UITableViewCell GetCellForModel(TModel model)
        {
            throw new NotImplementedException("Did not override GetCellForModel method from ObservableCollectionTableViewSource");
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var changes = new NSIndexPath[] { NSIndexPath.FromRowSection(e.NewStartingIndex, 0) };
            var oldChanges = new NSIndexPath[] { NSIndexPath.FromRowSection(e.OldStartingIndex, 0) };

            _tableView.BeginUpdates();

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.Count != 1)
                {
                    var items = new List<TModel>();
                    foreach (var i in e.NewItems)
                        items.Add((TModel)i);

                    changes = items.Select(p =>
                         NSIndexPath.FromRowSection(
                             e.NewStartingIndex + e.NewItems.IndexOf(p),
                             0))
                        .ToArray();
                }

                _tableView.InsertRows(changes, UITableViewRowAnimation.Automatic);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                _tableView.DeleteRows(oldChanges, UITableViewRowAnimation.Automatic);
            }
            else if(_collection.Count==0)
            {
                var allIndexes = new List<NSIndexPath>();
                for (int i = 0; i < _previousCollection.Count; i++)
                    allIndexes.Add(NSIndexPath.FromRowSection(i, 0));

                _tableView.DeleteRows(allIndexes.ToArray(), UITableViewRowAnimation.Automatic);
            }
            else
            {

            }

            _tableView.EndUpdates();
            _previousCollection = new ObservableCollection<TModel>(_collection);
        }
    }
}
