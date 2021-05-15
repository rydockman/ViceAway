using System;
using SkiaSharp;
using SQLite;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace FinalProject
{
    [Table("cravingsclass")]
    public class CravingsClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Hex { get; set; }
        public int Count { get; set; }

        public CravingsClass()
        {
        }

        public override string ToString()
        {
            return DisplayName + " " + Count;
        }
    }
}
