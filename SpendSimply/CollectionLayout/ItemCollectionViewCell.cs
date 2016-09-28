using System;
using CoreGraphics;
using UIKit;
using System.Linq;
using Foundation;
using System.Diagnostics;

namespace SpendSimply
{
    public class ItemCollectionViewCell : UICollectionViewCell
    {
        public MainView Owner;
        public DBofItems ItemInDB;
        public UIButton DeleteButton;
        public UIButton DisplayInfoButton;
        public UIButton HideInfoButton;
        public UILabel ItemLabel;

        [Export("initWithFrame:")]
        public ItemCollectionViewCell(CGRect frame) : base(frame)
        {
            //ContentView.Layer.BorderColor = UIColor.Brown.CGColor;
            //ContentView.Layer.BorderWidth = 10f;
            //ContentView.BackgroundColor = UIColor.Gray;
            //ContentView.Transform = CGAffineTransform.MakeScale(20f, 20f);

            ItemSetup(frame);
            var cellButton = new UIButton()
            {
                Frame = new CGRect(frame.Width / 2, frame.Height / 2, 0, 0),
                Bounds = new CGRect(0, 0, frame.Width, frame.Height),
                BackgroundColor = colorSet[0],
                AccessibilityIdentifier = "rounded corner wrapper"
            };
            cellButton.Layer.CornerRadius = 33f;
            BackgroundView = cellButton;

            //var selectedButton = new UIButton()
            //{
            //    Frame = new CGRect(frame.Width / 2, frame.Height / 2, 0, 0),
            //    Bounds = new CGRect(0, 0, frame.Width, frame.Height),
            //    BackgroundColor = UIColor.Yellow,
            //    AccessibilityIdentifier = "rounded corner wrapper"
            //};
            //selectedButton.Layer.CornerRadius = 20f;
            //SelectedBackgroundView = selectedButton;

            DisplayInfoButton.TouchUpInside += (sender, e) =>
            {
                ItemLabel.TextColor = UIColor.DarkGray;
                UpdateDetails(ItemLabel, ItemInDB);
                DisplayInfoButton.RemoveFromSuperview();
                ContentView.Add(HideInfoButton);
                ItemLabel.AccessibilityIdentifier = "item details";
                //ContentView.Add(DeleteButton);
            };
            HideInfoButton.TouchUpInside += (sender, e) =>
            {
                ItemLabel.TextColor = UIColor.Black;
                ItemLabel.Text = ItemInDB.ItemName;
                HideInfoButton.RemoveFromSuperview();
                ContentView.Add(DisplayInfoButton);
                ItemLabel.AccessibilityIdentifier = "tracked item";
                //DeleteButton.RemoveFromSuperview();
            };

            DeleteButton.TouchUpInside += (sender, e) =>
            {
                Alerts.PresentDeleteOptions(Owner, ItemInDB, DeleteButton, ItemLabel);
            };

            var touch = new UITapGestureRecognizer(() =>
            {
                AppDelegate.itemsToTrack.AddTapEventToItem(ItemInDB);
                MainView.UpdateCostOfAllItems();
                if (ItemLabel.Text != ItemInDB.ItemName)
                {
                    UpdateDetails(ItemLabel, ItemInDB);
                }
                //DeleteButton.RemoveFromSuperview();
            });
            ItemLabel.AddGestureRecognizer(touch);
            ItemLabel.UserInteractionEnabled = true;

            var longpress = new UILongPressGestureRecognizer(() =>
            {
                ContentView.Add(DeleteButton);
                ItemLabel.UserInteractionEnabled = false;
            });
            ItemLabel.AddGestureRecognizer(longpress);

            ContentView.AddSubviews(ItemLabel, DisplayInfoButton);
        }

        public static void UpdateDetails(UILabel itemLabel, DBofItems itemInDB)
        {
            itemLabel.Text = "Value: " + string.Format("{0:N}", (itemInDB.ItemPrice)) +
                "\nClicked: " + Convert.ToString(AppDelegate.itemsToTrack.NumberOfTapsInLast30DaysForItem(itemInDB)) +
                "\nTotal: $" + string.Format("{0:N}", (itemInDB.ItemPrice * AppDelegate.itemsToTrack.NumberOfTapsInLast30DaysForItem(itemInDB)));
        }

        public void ItemSetup(CGRect frame)
        {
            ItemLabel = new UILabel()
            {
                Frame = new CGRect(frame.Width / 2, frame.Height / 2, 0, 0),
                Bounds = new CGRect(0, 0, frame.Width - 10, frame.Height - 8),
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(22),
                BackgroundColor = UIColor.Clear,
                Lines = 3,
                AccessibilityIdentifier = "tracked item",
            };

            DisplayInfoButton = new UIButton()
            {
                Frame = new CGRect(frame.Width - frame.Width / 10 - 3, frame.Height - frame.Width / 10 - 3, 0, 0),
                Bounds = new CGRect(0, 0, frame.Width / 6, frame.Width / 6),
                AccessibilityIdentifier = "display info",
                Font = UIFont.SystemFontOfSize(13)
            };
            DisplayInfoButton.SetImage(UIImage.FromFile("Images/Info.png"), UIControlState.Normal);

            HideInfoButton = new UIButton()
            {
                Frame = new CGRect(frame.Width - frame.Width / 10 - 3, frame.Height - frame.Width / 10 - 3, 0, 0),
                Bounds = new CGRect(0, 0, frame.Width / 6, frame.Width / 6),
                AccessibilityIdentifier = "hide info",
                Font = UIFont.SystemFontOfSize(22)
            };
            HideInfoButton.SetImage(UIImage.FromFile("Images/Info.png"), UIControlState.Normal);

            DeleteButton = new UIButton()
            {
                Frame = new CGRect(5, 5, 0, 0),
                Bounds = new CGRect(0, 0, frame.Width / 6, frame.Width / 6),
                AccessibilityIdentifier = "delete item"
            };
            DeleteButton.SetImage(UIImage.FromFile("Images/Cancel.png"), UIControlState.Normal);
        }

        UIColor[] colorSet = new UIColor[]
        {
            UIColor.FromRGB(122, 198, 50)/*, UIColor.FromRGB(205, 214, 74),
            UIColor.FromRGB(58, 90, 144)*/
        };
    }
}

