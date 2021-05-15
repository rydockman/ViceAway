using System;
using SQLite;

namespace FinalProject
{
    [Table("withdrawl")]
    public class Withdrawl
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DatePerformed { get; set; }
        public double Amount { get; set; }
        public string Reason { get; set; }

        public Withdrawl()
        {
        }

        public override string ToString()
        {
            return DatePerformed.ToShortDateString() + " - $" + Amount;
        }
    }
}
