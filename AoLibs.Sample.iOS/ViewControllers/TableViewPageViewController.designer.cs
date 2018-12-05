// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AoLibs.Sample.iOS
{
    [Register ("TableViewPageViewController")]
    partial class TableViewPageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddManyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddSingleButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RemoveManyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RemoveSingleButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddManyButton != null) {
                AddManyButton.Dispose ();
                AddManyButton = null;
            }

            if (AddSingleButton != null) {
                AddSingleButton.Dispose ();
                AddSingleButton = null;
            }

            if (RemoveManyButton != null) {
                RemoveManyButton.Dispose ();
                RemoveManyButton = null;
            }

            if (RemoveSingleButton != null) {
                RemoveSingleButton.Dispose ();
                RemoveSingleButton = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}