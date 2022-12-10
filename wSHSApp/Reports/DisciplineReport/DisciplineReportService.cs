using SharpDocx;
using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using wSHSApp.Data;
using wSHSApp.Models;
using FSLib.Declension;
using System.Collections.Generic;
using wSHSApp.Reports.LetterReport.Model;

namespace wSHSApp.Reports.DisciplineReport;

public class DisciplineReportService : InfoService, IReport
{
    public string Name { set; get; } = "Материал на осужденного";
    public string Description { get; set; } = "Макрос формирует рапорт о нарушении осужденным ПВР, для привлечения его к дисциплинарной ответственности.";
    public string Author { get; set; } = "© 2022 Сергей Шевченко";
    public string OutputFileName { set; get; } = "Материал на осужденного";

    public async Task<string> EntryPoint(string[] args)
    {
        var documentOutput = string.Empty;
        await Task.Run(async () =>
        {
            //var Model = new LetterReportModel();
            AkusPrisoner currentPrisoner = AkusConnect.Names.Where(p => p.Itemperson == args[0]).First();
            //Model.FullName2 = Declension1251.GetSNPDeclension(currentPrisoner.Surname, currentPrisoner.Name, currentPrisoner.Lastname, DeclensionCase.Rodit);
            //Model.Birthday = currentPrisoner.Birthday;
            //Model.Name0 = currentPrisoner?.GetShortName();
            //Model.Name3 = Declension1251.GetSNPDeclension(currentPrisoner!.GetShortName(), Gender.MasculineGender, DeclensionCase.Datel);

            documentOutput = GenerateReportFileName(currentPrisoner.GetShortName());
            //var documentInput = DocumentFactory.Create(Path.Combine("Reports/Templates", args[1]), Model);
            //documentInput.Generate(Path.Combine("Reports/Output", documentOutput), Model);
        });
        return await Task.FromResult(Path.Combine("/Documents", documentOutput));
    }

    private string GenerateReportFileName(string prisonerName)
    {
        return OutputFileName = prisonerName + " - " + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".docx";
    }

    public Task RemoveReport()
    {
        File.Delete(Path.Combine("Reports/Output", OutputFileName));
        return Task.CompletedTask;
    }
}
