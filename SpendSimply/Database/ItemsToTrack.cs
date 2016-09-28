using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace SpendSimply
{
    public class ItemsToTrack
    {
        readonly SQLiteConnection ItemConn;
        public TapEventsTracker tapEventsTracker;

        public ItemsToTrack(string dbPath)
        {
            ItemConn = new SQLiteConnection(dbPath);
            ItemConn.CreateTable<DBofItems>();

            string tapEventPath = FileAccessHelper.GetLocalFilePath("tapevents.db3");
            tapEventsTracker = new TapEventsTracker(tapEventPath);
            //tapEventsTracker = AppDelegate.tapEventsTracker;

            //if (conn.Table<DBofItems>().Count() == 0)
            if (GetAllItemProperties().Any() == false)
            {
                var newItem = new DBofItems();
                //newItem.ItemName = "small coffee";
                //newItem.ItemPrice = 3.5m;
                newItem.ItemName = "Press the 'i' for details";
                newItem.ItemPrice = 12.34m;
                AddItem(newItem);
                //newItem.ItemName = "Lunch (salad)";
                //newItem.ItemPrice = 9.8m;
                newItem.ItemName = "Long press to delete or reset";
                newItem.ItemPrice = 1;
                AddItem(newItem);
            }
        }

        public void AddItem(DBofItems newitem)
        {
            ItemConn.Insert(newitem);
        }
        public void AddTapEventToItem(DBofItems item)
        {
            var newTapEvent = new DBofTapEvents();
            newTapEvent.ItemId = item.Id;
            newTapEvent.TimeOfTap = DateTime.UtcNow.Ticks;

            tapEventsTracker.AddTapEvent(newTapEvent);
        }

        public List<string> GetAllItemNames()
        {
            List<string> allItems = ItemConn.Table<DBofItems>().ToList().Select(x => x.ItemName).ToList();
            return allItems;
        }
        public List<decimal> GetAllPrices()
        {
            List<decimal> allPrices = ItemConn.Table<DBofItems>().ToList().Select(x => x.ItemPrice).ToList();
            return allPrices;
        }
        public List<DBofItems> GetAllItemProperties()
        {
            var itemList = ItemConn.Table<DBofItems>().ToList();
            return itemList;
        }

        public int NumberOfTaps(DBofItems item)
        {
            var timesTapped = tapEventsTracker.GetTapEventsforItem(item.Id).Count();
            return timesTapped;
        }
        public decimal CostOfAllItems()
        {
            var allItems = GetAllItemProperties();
            List<decimal> totalSpentPerItem = new List<decimal>();

            foreach (var item in allItems)
            {
                var numberOfTaps = NumberOfTaps(item);
                var price = item.ItemPrice * numberOfTaps;
                totalSpentPerItem.Add(price);
            }
            return totalSpentPerItem.Sum();
        }

        public int NumberOfTapsInLast30DaysForItem(DBofItems item)
        {
            var timesTapped = tapEventsTracker.GetLast30DaysOfTaps(item.Id);
            return timesTapped.Count();
        }
        public decimal CostOfItemsInLast30Days()
        {
            var allItems = GetAllItemProperties();
            List<decimal> totalSpentPerItem = new List<decimal>();

            foreach (var item in allItems)
            {
                var numberOfTaps = NumberOfTapsInLast30DaysForItem(item);
                var price = item.ItemPrice * numberOfTaps;
                totalSpentPerItem.Add(price);
            }
            return totalSpentPerItem.Sum();
        }

        public void RemoveLastTapEvent(DBofItems item)
        {
            tapEventsTracker.RemoveLastTapEvent(item.Id);
        }
        public void ResetAllTapEventsOfItem(DBofItems item)
        {
            tapEventsTracker.ResetAllTapEvents(item.Id);
        }
        public void RemoveItem(DBofItems item)
        {
            ItemConn.Delete(item);
            tapEventsTracker.ResetAllTapEvents(item.Id);
        }
    }
}

