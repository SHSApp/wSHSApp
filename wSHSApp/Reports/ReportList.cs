using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using wSHSApp.Data;

namespace wSHSApp.Reports;

public static class Reports
{
    public static readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
            Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";
    public static List<ReportEntity> ReportList { get; set; } = new();

    public static void Init()
    {
        ReportList.Add(new ReportEntity { 
            Id = 1,
            Name = "Справка по корреспонденции",
            Description = "Макрос, генерирующий справку по корреспонденции на осужденного.",
            Author = "Сергей Шевченко",
            EntryPoint = LettersReport.EntryPoint,
            TemplateName = "letters.docx"
        });
    }

    public static async Task<DataTable> GetData(string Itemperson, string Query, Func<DataTable, DataTable> AddColumns, Func<OleDbDataReader, DataTable, DataTable> AddRow)
    {
        DataTable res = new();        
        if (!string.IsNullOrEmpty(Itemperson))
        {
            await Task.Run(() =>
            {
                try
                {
                    OleDbConnection Conn = new(ConnectionString);
                    Conn.Open();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                    oCmd.Parameters["itemperson"].Value = Itemperson;
                    oCmd.CommandText = Query;
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    AddColumns(res);
                    while (reader.Read())
                    {
                        res = AddRow(reader, res);
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

public class ReportEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public Action<string[]> EntryPoint { get; set; }
    public string TemplateName { get; set; }
}
