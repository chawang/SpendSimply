using System;
using UIKit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Foundation;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace SpendSimply
{
    public class ItemCollectionSource : UICollectionViewSource
    {
        MainView Owner;
        string CellID;
        List<DBofItems> itemList { get; set; } = new List<DBofItems>();
        public ItemCollectionViewCell itemCell;


        public ItemCollectionSource(MainView owner, string cellID)
        {
            Owner = owner;
            CellID = cellID;
            itemList = AppDelegate.itemsToTrack.GetAllItemProperties();
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            Contract.Ensures(Contract.Result<UICollectionViewCell>() != null);
            itemCell = (ItemCollectionViewCell)collectionView.DequeueReusableCell(CellID, indexPath);
            itemCell.ItemInDB = itemList[indexPath.Row];
            itemCell.ItemLabel.Text = itemList[indexPath.Row].ItemName;
            itemCell.Owner = Owner;

            return itemCell;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return itemList.Count();
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return false;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            // Reorder our list of items
            var item = itemList[(int)sourceIndexPath.Item];
            itemList.RemoveAt((int)sourceIndexPath.Item);
            itemList.Insert((int)destinationIndexPath.Item, item);
        }

        //public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    return true;
        //}

        //public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = UIColor.Blue;
        //}

        //public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = UIColor.Gray;
        //}
    }
}

