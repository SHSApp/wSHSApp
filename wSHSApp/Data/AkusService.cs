using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using wSHSApp.Models;

namespace wSHSApp.Data;

public static class AkusService
{
    private static readonly string[] pcNames = { "pc22", "pc28", "pc29", "pc32", "pc34", "pc36", "pc37", "pc52", "pc56", "pc70" };

    private static readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
        Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";

    private static List<List<PcItem>> pcCollections = new();

    public static string GetGroup(string Id)
    {
        if (Id.Trim(' ').Length > 0)
        {
            int index = Array.IndexOf(pcNames, "pc52");
            string result = string.Empty;
            var res = from item in pcCollections[index].AsEnumerable() where item.Id == Id select item.Name;
            result = res.First().Contains("Кар") ? "Отделение \"Карантин\"" : "Отряд № " + res.First();
            return result;
        }
        else return "Отряд не указан";
    }

    public static string Get(string Id, string Pc)
    {
        string srcId = Id.Trim(' ');
        if (srcId.Length > 0)
        {
            int index = Array.IndexOf(pcNames, Pc);
            int IDcount = srcId.Length / 2;
            string result = "";
            for (int i = 0; i < IDcount; i++)
            {
                var res = from item in pcCollections[index].AsEnumerable()
                          where item.Id == srcId.Substring(2 * i, 2)
                          select item.Name;
                if (i > 0) { result += ", "; }
                result += res.First();
            }
            return result;
        }
        else return "–";
    }

    public static void Init()
    {
        try
        {
            OleDbConnection Conn = new(ConnectionString);
            Conn.Open();
            OleDbCommand oCmd = Conn.CreateCommand();
            foreach (string pcName in pcNames)
            {
                List<PcItem> pcItems = new();
                oCmd.CommandText = "SELECT item, name FROM Qualif\\" + pcName;
                OleDbDataReader reader = oCmd.ExecuteReader();
                while (reader.Read())
                {
                    pcItems.Add(new PcItem
                    {
                        Name = (string)reader["name"],
                        Id = (string)reader["item"],
                    });
                }
                reader.Close();
                pcCollections.Add(pcItems);
            }
            Conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
    public static string QueryPc(string Id, string PcName)
    {
        string ItemId = Id.Trim(' ');
        string result = "–";
        if (!string.IsNullOrEmpty(ItemId))
        {
            try
            {
                OleDbConnection Conn = new(ConnectionString);
                Conn.Open();
                OleDbCommand oCmd = Conn.CreateCommand();
                if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("id", OleDbType.WChar));
                int IDcount = ItemId.Length / 2;
                result = string.Empty;
                for (int i = 0; i < IDcount; i++)
                {
                    oCmd.Parameters["id"].Value = ItemId.Substring(2 * i, 2);
                    oCmd.CommandText = "SELECT item, name FROM Qualif\\" + PcName + " WHERE item == ?";
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    reader.Read();
                    if ((string)reader["name"] != "")
                    {
                        if (i > 0) result += ", ";
                        result += reader["name"]?.ToString()?.TrimEnd(' ');
                    }
                    reader.Close();
                }
                Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return result;
    }

    public static string GetNote(string? Itemperson, string? Itemme, string PrName)
    {
        string result = "–";
        if (!string.IsNullOrEmpty(Itemperson) && !string.IsNullOrEmpty(Itemme))
        {
            try
            {
                OleDbConnection Conn = new(ConnectionString);
                Conn.Open();
                OleDbCommand oCmd = Conn.CreateCommand();
                oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                oCmd.Parameters.Add(new OleDbParameter("itemme", OleDbType.WChar));
                oCmd.Parameters["itemperson"].Value = Itemperson;
                oCmd.Parameters["itemme"].Value = Itemme;
                oCmd.CommandText = "SELECT itemperson, itemme, name FROM Data\\" + PrName + " WHERE (itemperson == ? AND itemme == ?)";
                OleDbDataReader reader = oCmd.ExecuteReader();
                result = string.Empty;
                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace((string)reader["name"])) result += reader["name"]?.ToString()?.TrimEnd(' ');
                }                    
                reader.Close();
                Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return result;
    }
}
