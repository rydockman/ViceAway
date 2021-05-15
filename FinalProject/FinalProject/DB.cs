using System;
using SQLite;
using Xamarin.Essentials;

namespace FinalProject
{
    public class DB
    {
        private static string DBName = "log.db";
        private static string CravingsDB = "cravings.db";

        public static SQLiteConnection conn;
        public static SQLiteConnection conn2;

        public static void OpenConnection()
        {
            string libFolder = FileSystem.AppDataDirectory;

            string fname = System.IO.Path.Combine(libFolder, DBName);
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Withdrawl>();

            string fname2 = System.IO.Path.Combine(libFolder, CravingsDB);
            conn2 = new SQLiteConnection(fname2);
            conn2.CreateTable<CravingsClass>();
        }
        public DB()
        {
        }
    }
}
