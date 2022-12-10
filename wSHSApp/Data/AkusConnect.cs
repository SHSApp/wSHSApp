using System;
using System.Collections.Generic;
using System.Data.OleDb;
using wSHSApp.Models;

namespace wSHSApp.Data;

public static class AkusConnect
{
    public static List<AkusPrisoner> Names = new List<AkusPrisoner>();
    private static readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
        Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";

    public static void Init()
    {
        try
        {
            OleDbConnection Conn = new(ConnectionString);
            Conn.Open();
            OleDbCommand oCmd = Conn.CreateCommand();
            oCmd.CommandText = "SELECT proper(vfamily) as vsurname, vname, vlastname, vdatar, itemperson, vnomotr FROM Data\\card";
            OleDbDataReader reader = oCmd.ExecuteReader();
            while (reader.Read())
            {
                Names.Add(new AkusPrisoner
                {
                    Surname = reader["vsurname"]?.ToString()?.Trim(' '),
                    Name = reader["vname"]?.ToString()?.Trim(' '),
                    Lastname = reader["vlastname"]?.ToString()?.Trim(' '),
                    Birthday = (string)reader["vdatar"],
                    Itemperson = (string)reader["itemperson"],
                    GroupId = (string)reader["vnomotr"]
                });
            }
            reader.Close();
            Conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


}
