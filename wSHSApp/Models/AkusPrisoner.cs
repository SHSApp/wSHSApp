using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using wSHSApp.Data;

namespace wSHSApp.Models;

public class AkusPrisoner
{
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string? Itemperson { get; set; }
    public string? GroupId { get; set; }
}



//public class InfoServiceODBC
//{
//    private static readonly string ConnectionString = "Driver={Microsoft FoxPro VFP Driver (*.dbf)};SourceType=DBF;SourceDB=" +
//        Tools.DataBasePath + ";Exclusive=No;Collate=Russian;"; //NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;"; 
//    public async Task<List<TItem>> GetItemsAsync<TItem>(string? Itemperson, string Query, Func<IDataReader, TItem> Action) where TItem : class
//    {
//        List<TItem> infoItems = new();
//        if (!string.IsNullOrEmpty(Itemperson))
//        {
//            await Task.Run(() =>
//            {
//                try
//                {
//                    using (OdbcConnection Conn = new(ConnectionString))
//                    {
//                        Conn.Open();
//                        OdbcCommand oCmd = Conn.CreateCommand();
//                        if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OdbcParameter("itemperson", OdbcType.VarChar));
//                        oCmd.Parameters["itemperson"].Value = Itemperson;
//                        oCmd.CommandText = Query;
//                        IDataReader reader = oCmd.ExecuteReader();
//                        while (reader.Read())
//                        {
//                            infoItems.Add(Action(reader));
//                        }
//                        reader.Close();
//                    }
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e.Message);
//                }
//            });
//        }
//        return await Task.FromResult(infoItems);
//    }
//}