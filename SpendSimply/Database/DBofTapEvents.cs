using System;
using SQLite;

namespace SpendSimply
{
    [Table("tapevents")]
    public class DBofTapEvents
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public int ItemId { get; set; }

        public long TimeOfTap { get; set; }

        //public int NumberOfPresses { get; set; }
    }
}

