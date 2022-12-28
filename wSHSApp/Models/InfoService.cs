using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading.Tasks;
using System;
using wSHSApp.Data;

namespace wSHSApp.Models;

public class InfoService
{
    private static readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
            Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";
    public async Task<List<TItem>> GetItemsAsync<TItem>(string? Itemperson, string Query, Func<OleDbDataReader, TItem> Action) where TItem : class
    {
        List<TItem> infoItems = new();
        if (!string.IsNullOrEmpty(Itemperson))
        {
            await Task.Run(() =>
            {
                try
                {
                    OleDbConnection Conn = new(ConnectionString);
                    Conn.Open();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                    oCmd.Parameters["itemperson"].Value = Itemperson;
                    oCmd.CommandText = Query;
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        infoItems.Add(Action(reader));
                    }
                    reader.Close();
                    Conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
        return await Task.FromResult(infoItems);
    }

    public async Task<TItem?> GetItemAsync<TItem>(string? Itemperson, string Query, Func<OleDbDataReader, TItem?> Action)
    {
        TItem? item = default;
        if (!string.IsNullOrEmpty(Itemperson))
        {
            await Task.Run(() =>
            {
                try
                {
                    OleDbConnection Conn = new(ConnectionString);
                    Conn.Open();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                    oCmd.Parameters["itemperson"].Value = Itemperson;
                    oCmd.CommandText = Query;
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    reader.Read();
                    item = Action(reader);
                    reader.Close();
                    Conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
        return await Task.FromResult(item);
    }

    public async Task<double> GetCountAsync(string? ItemValue, OleDbType Param, string Query)
    {
        double CountItems = default;
        if (!string.IsNullOrEmpty(ItemValue) && !string.IsNullOrEmpty(ItemValue))
        {
            try
            {
                OleDbConnection Conn = new(ConnectionString);
                Conn.Open();
                OleDbCommand oCmd = Conn.CreateCommand();
                if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("item", Param));
                oCmd.Parameters["item"].Value = ItemValue;
                oCmd.CommandText = Query;
                CountItems = Convert.ToDouble((decimal?)oCmd.ExecuteScalar());
                Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return await Task.FromResult(CountItems);
    }
}