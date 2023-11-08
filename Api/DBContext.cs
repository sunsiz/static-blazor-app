using ApiIsolated;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Shared
{
    public class DBContext : DbContext
    {
        private readonly ILogger _logger;
        private readonly string _dbPath;
        SQLiteConnection conn;
        public DBContext(ILoggerFactory loggerFactory)
        {
            _dbPath = "quran.db";;
            _logger = loggerFactory.CreateLogger<DBContext>();
        }
        private void Initialize()
        {
            // Don't Create database if it exists
            if (conn != null)
                return;

            try
            {
                if (conn == null)
                {
                    // Create database and Tables
                    //if (!File.Exists(_dbPath)) await CopyFileToAppDataDirectory();//CopyDB(_dbPath);
                    conn = new SQLiteConnection(_dbPath,
                        SQLite.SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache)
                    {
                        Tracer = new Action<string>(q => Debug.WriteLine(q)),
                        Trace = true
                    };
                    Console.WriteLine("The database path: " + conn.DatabasePath);
                    var count = conn.TableMappings.Count();
                    if (count == 0)
                    {
                        Debug.WriteLine("The table counts before: " + count);
                        conn.CreateTable<Sura>();
                        conn.CreateTable<Aya>();
                        Debug.WriteLine("The table counts after: " + conn.TableMappings.Count());
                        Debug.WriteLine(conn.DatabasePath);
                    }
                    //conn.Backup(conn.DatabasePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public List<Sura> GetSuras()
        {
            Initialize();
            return conn.Table<Sura>().ToList();
        }
        public IEnumerable<Aya> GetAyaList(int suraId)
        {
            Initialize();
            return conn.Table<Aya>().Where(a => a.SuraId == suraId).ToList();
        }
    }
}
