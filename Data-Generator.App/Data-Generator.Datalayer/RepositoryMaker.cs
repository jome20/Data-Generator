using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dapper;
using System.Collections;

namespace Data_Generator.Datalayer
{
    public class RepositoryMaker
    {
        #region MAin

        public static void setdatabaseSQLite(string dbname)
        {
            _dbStatus = DBStatus.SQLite;
            connectionString = "Data Source=" + dbname + @";Version=3;Compress=True;";
            // connectionString = "SERVER='localhost';" + "DATABASE=" + dbname + ";" + "UID=root;" + "PASSWORD='';charset=utf8;";
        }
        public static void setdatabase(string dbname)
        {
            _dbStatus = DBStatus.MySql;
            connectionString = "SERVER='localhost';" + "DATABASE=" + dbname + ";" + "UID=root;" + "PASSWORD='';charset=utf8;";
        }
        public static List<string> getDATABASES()
        {
            _dbStatus = DBStatus.MySql;
            connectionString = "SERVER='localhost'; UID=root;" + "PASSWORD='';";
            string q = "show DATABASES";
            using (IDbConnection db = GetIDbConnection())
            {
                try { return db.Query<string>(q).ToList(); } catch (Exception ex) { return null; }
            }
        }
        public  static List<string> getTables(string dbname)
        {
            RepositoryMaker.setdatabase(dbname);

            return setTables();
        }

        public static List<string> setTables()
        {
            string q = "show TABLES";
            switch (_dbStatus)
            {
                case DBStatus.Sqlserver:
                    break;
                case DBStatus.MySql:
                    break;
                case DBStatus.SQLite:
                    //return new List<string>();

                    q = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
                    break;
                default:
                    break;
            }

            //q = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";

            using (IDbConnection db = GetIDbConnection())
            {
                // D:\apk\Folder apk\English Stories.apks\base\assets\databases\KidsSoftwares.db
                try
                {
                    //q = "SELECT NAME FROM REMEDIES";
                    var lst = db.Query<string>(q).ToList();
                    return lst;
                }
                catch (Exception ex) { return null; }
            }
        }

        public enum DBStatus { Sqlserver, MySql, SQLite };
        static DBStatus _dbStatus;
        private static string connectionString;
        static void Startmysql(string Database, string servername, string uid, string password)
        {
            _dbStatus = DBStatus.MySql;
            connectionString = "SERVER='" + servername + "';" + "DATABASE=" + Database + ";" + "UID=" + uid + ";" + "PASSWORD=" +
                password + ";charset=utf8;";
        }
        internal static IDbConnection GetIDbConnection()
        {
            switch (_dbStatus)
            {
                case DBStatus.Sqlserver:
                    return new System.Data.SqlClient.SqlConnection(connectionString);
                //case DBStatus.SQLite: return new Finisar.SQLite.SQLiteConnection(connectionString);
                case DBStatus.MySql:
                    return new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            }
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }
        #endregion






        /*
        public static ArrayList GetTables()
        {
            ArrayList list = new ArrayList();

            // executes query that select names of all tables in master table of the database
            String query = "SELECT name FROM sqlite_master " +
                    "WHERE type = 'table'" +
                    "ORDER BY 1";
            try
            {

                DataTable table = GetDataTable(query);

                // Return all table names in the ArrayList

                foreach (DataRow row in table.Rows)
                {
                    list.Add(row.ItemArray[0].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list;
        }

        public static DataTable GetDataTable(string sql)
        {
            try
            {

                DataTable dt = new DataTable();
                using (var c = new Finisar.SQLite.SQLiteConnection(connectionString))
                {
                    c.Open();
                    using (Finisar.SQLite.SQLiteCommand cmd = new Finisar.SQLite.SQLiteCommand(sql, c))
                    {
                        using (Finisar.SQLite.SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //*/

    }
}
