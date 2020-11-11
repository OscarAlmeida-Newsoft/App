using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;
using Xamarin.Forms;
using App.iOS;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace App.iOS
{
    public class SQLite_iOS : ISQLite
    {
        string sqliteFilename = "tdm.db3";

        public bool ExisteBaseDeDatos()
        {
            
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            // This is where we copy in the prepopulated database
            Console.WriteLine(path);
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region ISQLite implementation
        public SQLite.SQLiteConnection GetConnection()
        {
            
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            // This is where we copy in the prepopulated database
            Console.WriteLine(path);
            if (!File.Exists(path))
            {
                File.Copy(sqliteFilename, path);
            }

            var conn = new SQLite.SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }
        #endregion
    }
}
