using System;
using SQLite;

namespace SpendSimply
{
    [Table("itemstotrack")]
    public class DBofItems
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string ItemName { get; set; }

        [MaxLength(3)]
        public decimal ItemPrice { get; set; }

        //public int NumberOfPresses { get; set; }

        //[Ignore]
        //public decimal TotalValueOfOneItem
        //{
        //    get { return ItemPrice * NumberOfPresses; }
        //}
    }
}

