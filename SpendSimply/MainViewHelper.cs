using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Foundation;

namespace SpendSimply
{
    public partial class MainView : UIViewController
    {
        public static void UpdateCostOfAllItems()
        {
            decimal costOfAllItems = AppDelegate.itemsToTrack.CostOfItemsInLast30Days();
            spent.Text = string.Format("{0:C}", costOfAllItems);
        }

        public static void LoadCollectionView()
        {
            var simpleLayout = new UICollectionViewFlowLayout()
            {
                ItemSize = new CGSize(width - 150, ((height - height / 9) - 70) / 3.3),
                SectionInset = new UIEdgeInsets(30, 20, 0, 20),
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
                //MinimumInteritemSpacing = 20,
                MinimumLineSpacing = 30,
            };

            //var fancyLayout = new LineLayout(width, height);

            collectionView = new UICollectionView((new CGRect(0, height / 9 + 10, width, height - (height / 9 + 10) - 50)), simpleLayout);

            collectionView.BackgroundColor = UIColor.Clear;
            collectionView.Source = new ItemCollectionSource(TheMainView, CellID);

            collectionView.RegisterClassForCell(typeof(ItemCollectionViewCell), CellID);
            collectionView.ShowsVerticalScrollIndicator = true;
            collectionView.ReloadData();

            view.AddSubview(collectionView);
        }
    }
}

