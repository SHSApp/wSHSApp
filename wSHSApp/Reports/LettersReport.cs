using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading.Tasks;
using wSHSApp.Data;

namespace wSHSApp.Reports;

public static class LettersReport
{
    public static async void EntryPoint(string[] args)
    {
        DataSet ds = new();
        string Query = "SELECT cast(iif(empty(vdataotpr), evl(vdataotpr, NULL), ctod(substr(vdataotpr, 4, 2) + " +
            "\".\" + substr(vdataotpr, 1, 2) + \".\" + substr(vdataotpr, 7, 4))) as date) as vdataotpr1, vdataotpr, " +
            "vvid, vkomu, vhapr_spr FROM Data\\obida WHERE obida.Itemperson = ? ORDER BY vdataotpr1";
        ds.Tables.Add(await GetData(args[0],Query));

        //Path.Combine("Reports/Templates", args[1])

    }

public static async Task<DataTable> GetData(string Itemperson, string Query)
    {
        DataTable res = new();
        if (!string.IsNullOrEmpty(Itemperson))
        {
            await Task.Run(() =>
            {
                try
                {
                    OleDbConnection Conn = new(Reports.ConnectionString);
                    Conn.Open();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                    oCmd.Parameters["itemperson"].Value = Itemperson;
                    oCmd.CommandText = Query;
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    res.Columns.Add("date");
                    res.Columns.Add("whom");
                    res.Columns.Add("where");
                    while (reader.Read())
                    {
                        DataRow dr = res.NewRow();
                        dr[0] = reader["vdataotpr"];
                        dr[1] = "";
                        dr[2] = reader["vkomu"]?.ToString().Trim(' ') + " " + Tools.IsEmpty(AkusService.QueryPc((string)reader["vhapr_spr"], "pc5"));
                        res.Rows.Add(dr);
                    }
                    Conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
        return await Task.FromResult(res);
    }
}
