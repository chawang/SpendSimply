using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Threading.Tasks;

namespace SpendSimply
{
    public class TapEventsTracker
    {
        private readonly SQLiteConnection TimeConn;

        public TapEventsTracker(string dbPath)
        {
            TimeConn = new SQLiteConnection(dbPath);
            TimeConn.CreateTable<DBofTapEvents>();
        }

        public void AddTapEvent(DBofTapEvents tapevent)
        {
            TimeConn.Insert(tapevent);
            //var test = TimeConn.Table<DBofTapEvents>().ToList();
        }

        public void RemoveLastTapEvent(int IDofItem)
        {
            TimeConn.Delete(GetTapEventsforItem(IDofItem).LastOrDefault());
        }
        public void ResetAllTapEvents(int IDofItem)
        {
            var listOfTapEvents = GetTapEventsforItem(IDofItem);

            foreach (var entry in listOfTapEvents)
            {
                TimeConn.Delete(entry);
            }
        }

        public List<DBofTapEvents> GetTapEventsforItem(int IDofItem)
        {
            var allTapsList = TimeConn.Table<DBofTapEvents>().ToList();
            var allTapsForItemList = new List<DBofTapEvents>();

            foreach (var tap in allTapsList)
            {
                if (tap.ItemId == IDofItem)
                {
                    allTapsForItemList.Add(tap);
                }
            }
            return allTapsForItemList;
        }
        public List<DBofTapEvents> GetLast30DaysOfTaps(int IDofItem)
        {
            var allTapsList = TimeConn.Table<DBofTapEvents>().ToList();
            var tapsForItemIn30DaysList = new List<DBofTapEvents>();

            foreach (var tap in allTapsList)
            {
                if (tap.ItemId == IDofItem)
                {
                    if (tap.TimeOfTap >= DateTime.UtcNow.AddDays(-30).Ticks)
                    {
                        tapsForItemIn30DaysList.Add(tap);
                    }
                }
            }
            return tapsForItemIn30DaysList;
        }
    }
}

