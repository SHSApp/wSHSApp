using SharpDocx;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading.Tasks;
using wSHSApp.Data;
using wSHSApp.Models;
using wSHSApp.Reports.Models;

namespace wSHSApp.Reports;

public class LettersReport : InfoService, IReport
{
    public string Name { set; get; } = "Справка по корреспонденции";
    public string Description { get; set; } = "Макрос формирует справку по корреспонденции на осужденного";
    public string Author { get; set; } = "(с) 2022 Сергей Шевченко";
    public string OutputFileName { set; get; } = "Справка по корреспонденции";

    //protected NavigationManager? navManager;

    //public LettersReport(NavigationManager? navigationManager)
    //{
    //    navManager = navigationManager;
    //}

    private readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
            Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";

    public async Task<string> EntryPoint(string[] args)
    {
        var documentOutput = GenerateReportFileName();
        await Task.Run(async () =>
        {
            var Model = new LetterReportModel();
            string Query = "SELECT cast(iif(empty(vdataotpr), evl(vdataotpr, NULL), ctod(substr(vdataotpr, 4, 2) + " +
            "\".\" + substr(vdataotpr, 1, 2) + \".\" + substr(vdataotpr, 7, 4))) as date) as vdataotpr1, vdataotpr, " +
            "vvid, vkomu, vhapr_spr FROM Data\\obida WHERE obida.Itemperson = ? ORDER BY vdataotpr1";


            var documentInput = DocumentFactory.Create(Path.Combine("Reports/Templates", args[1]));
            
            documentInput.Generate(Path.Combine("Reports/Output", documentOutput), Model);
        });        
        return await Task.FromResult(Path.Combine("/Documents", documentOutput));
    }

    private string GenerateReportFileName()
    {
        return OutputFileName = "Справка по корреспонденции-" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".docx";
    }

    public Task RemoveReport()
    {
        File.Delete(Path.Combine("Reports/Output", OutputFileName));
        return Task.CompletedTask;
    }
}
