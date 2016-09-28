using System;
using System.IO;
using System.Reflection;
using CoreGraphics;
using Google.MobileAds;
using UIKit;

namespace SpendSimply
{
    public partial class MainView : UIViewController
    {
        static MainView TheMainView { get; set; }
        public const string CellID = "cell";
        public static UILabel spent;
        public static UICollectionView collectionView;
        public static nfloat width;
        public static nfloat height;
        public static UIView view;
        public static UIColor ColorTheme;

        public MainView()
        {
            ColorTheme = UIColor.FromRGB(122, 198, 50);
            TheMainView = this;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            view = View;
            width = View.Bounds.Width;
            height = View.Bounds.Height;
            View.BackgroundColor = UIColor.Black;
            AddBanner();

            LoadCollectionView();

            var header = new UILabel()
            {
                Frame = new CGRect(0, 0, width, height / 9 + 10),
                BackgroundColor = ColorTheme
            };

            spent = new UILabel()
            {
                Frame = new CGRect(30, 20, width / 2, height / 9 - 25),
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear,
                Font = UIFont.SystemFontOfSize(24),
                AccessibilityIdentifier = "total 30 day cost"
            };
            var info = new UILabel()
            {
                Frame = new CGRect(30, 20, width / 2, height / 9 + 11),
                TextAlignment = UITextAlignment.Left,
                BackgroundColor = UIColor.Clear,
                Font = UIFont.SystemFontOfSize(10)
            };
            info.Text = " Over 30 days";

            UpdateCostOfAllItems();

            UIButton addButton = new UIButton()
            {
                Frame = new CGRect(width - 60, height / 18 - 10, 40, 40),
                AccessibilityIdentifier = "add button"
            };
            addButton.SetImage(UIImage.FromFile("Images/Plus.png"), UIControlState.Normal);

            UIButton resetButton = new UIButton()
            {
                Frame = new CGRect(width - 110, height / 18 - 10, 40, 40),
                AccessibilityIdentifier = "reset button"
            }; ;
            resetButton.SetImage(UIImage.FromFile("Images/Restart.png"), UIControlState.Normal);

            View.AddSubviews(header, spent, info, addButton, resetButton);

            addButton.TouchUpInside += (sender, e) =>
            {
                Alerts.GenerateNewItemTitleAlert(this);
            };

            resetButton.TouchUpInside += (sender, e) =>
            {
                Alerts.ResetAllTrackedItems(this);
            };

            var longPressGesture = new UILongPressGestureRecognizer(gesture =>
            {
                switch (gesture.State)
                {
                    case UIGestureRecognizerState.Began:
                        var selectedIndexPath = collectionView.IndexPathForItemAtPoint(gesture.LocationInView(View));
                        if (selectedIndexPath != null)
                        {
                            collectionView.BeginInteractiveMovementForItem(selectedIndexPath);
                        }
                        //collectionView.CellForItem(selectedIndexPath).Add(ItemCollectionViewCell.DeleteButton);
                        break;
                    case UIGestureRecognizerState.Changed:
                        collectionView.UpdateInteractiveMovement(gesture.LocationInView(View));
                        break;
                    case UIGestureRecognizerState.Ended:
                        collectionView.EndInteractiveMovement();
                        break;
                    default:
                        collectionView.CancelInteractiveMovement();
                        break;
                }
            });
            collectionView.AddGestureRecognizer(longPressGesture);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        BannerView ad;
        bool viewOnScreen = false;

        public void AddBanner()
        {
            string bannderID;
            //var assembly = typeof(MainView).GetTypeInfo().Assembly;
            //var stream = assembly.GetManifestResourceStream("SpendSimply.personalID.txt");
            var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpendSimply.personalID.txt"));
            bannderID = stream.ReadLine();

            ad = new BannerView(AdSizeCons.Banner)
            {
                AdUnitID = bannderID,
                RootViewController = this
            };
            ad.Frame = new CGRect(0, height - 50, width, 50);

            ad.AdReceived += (object sender, EventArgs e) =>
            {
                if (!viewOnScreen)
                    View.AddSubview(ad);
                viewOnScreen = true;
            };
            ad.LoadRequest(Request.GetDefaultRequest());
        }
    }
}

