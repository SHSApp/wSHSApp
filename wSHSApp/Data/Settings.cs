using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace WeKus
{
    public static class Settings
    {
        private static OleDbConnection Conn = new OleDbConnection(ConnectionString);
        //private static OleDbConnection Conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\prj\WeKus\App_Data\settings.mdb;");
        //public static string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=~\App_data\settings.mdb;";
        public static string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\prj\wapp\wapp\App_Data\settings.mdb;";
        private static DataTable Personal = new DataTable();
        private static DataTable Options = new DataTable();

        public static void LoadTables()
        {
            //if (Conn != null)
            
                try
                {
                    Conn.Open();
                    //Personal = new DataTable();
                    //Options = new DataTable();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    oCmd.CommandText = "SELECT * FROM Personal ORDER BY ID";
                    Personal.Load(oCmd.ExecuteReader());
                    oCmd.CommandText = "SELECT * FROM Options ORDER BY ID";
                    Options.Load(oCmd.ExecuteReader());
                    Conn.Close();
                }
                catch (Exception e)
                {
                    string s = "2";
                }
            
        }

        public static string GetOption(string name)
        {
            var opt = from row in Options.AsEnumerable() where row.Field<string>("Параметр") == name select row.Field<string>("Значение");
            return opt.First();
        }

        public static void FillHeadersByID(int ID)
        {
            switch (ID)
            {
                case 0:

                    break;
                case 1:

                    break;
            }               
        }

        public static string GetPersonalDoljnostByID(int ID, bool Sklonenie)
        {
            if (!Sklonenie)
            {
                return Personal.Rows[ID].ItemArray[2].ToString();
            }
            else
            {
                return Personal.Rows[ID].ItemArray[5].ToString();
            }
        }

        public static string GetPersonalZvanieByID(int ID, bool Sklonenie)
        {
            if (!Sklonenie)
            {
                return Personal.Rows[ID].ItemArray[3].ToString();
            }
            else
            {
                return Personal.Rows[ID].ItemArray[6].ToString();
            }
        }

        public static string GetPersonalFIOByID(int ID, bool Sklonenie)
        {
            if (!Sklonenie)
            {
                return Personal.Rows[ID].ItemArray[1].ToString();
            }
            else
            {
                return Personal.Rows[ID].ItemArray[4].ToString();
            }
        }

        public static string GetPersonalFullNameByID(int ID, bool Sklonenie)
        {
            if (!Sklonenie)
            {
                return Personal.Rows[ID].ItemArray[2].ToString() + " " 
                    + Personal.Rows[ID].ItemArray[3].ToString() + " внутренней службы " + Personal.Rows[ID].ItemArray[1].ToString();
            }
            else
            {
                return Personal.Rows[ID].ItemArray[5].ToString() + " "
                    + Personal.Rows[ID].ItemArray[6].ToString() + " внутренней службы " + Personal.Rows[ID].ItemArray[4].ToString();
            }
        }



    }
}
