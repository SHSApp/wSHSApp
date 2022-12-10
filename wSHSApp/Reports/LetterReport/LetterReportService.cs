using SharpDocx;
using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using wSHSApp.Data;
using wSHSApp.Models;
using wSHSApp.Reports.Models;
using FSLib.Declension;
using System.Collections.Generic;

namespace wSHSApp.Reports.LetterReport;

public class LetterReportService : InfoService, IReport
{
    public string Name { set; get; } = "Справка по корреспонденции";
    public string Description { get; set; } = "Макрос формирует справку по корреспонденции на осужденного";
    public string Author { get; set; } = "© 2022 Сергей Шевченко";
    public string OutputFileName { set; get; } = "Справка по корреспонденции";

    public async Task<string> EntryPoint(string[] args)
    {
        var documentOutput = string.Empty;
        await Task.Run(async () =>
        {
            var Model = new LetterReportModel();
            AkusPrisoner currentPrisoner = AkusConnect.Names.Where(p => p.Itemperson == args[0]).First();
            Model.FullName2 = Declension1251.GetSNPDeclension(currentPrisoner.Surname, currentPrisoner.Name, currentPrisoner.Lastname, DeclensionCase.Rodit);
            Model.Birthday = currentPrisoner.Birthday;
            Model.Name0 = currentPrisoner?.GetShortName();
            Model.Name3 = Declension1251.GetSNPDeclension(currentPrisoner!.GetShortName(), Gender.MasculineGender, DeclensionCase.Datel);
            string Query = "SELECT cast(iif(empty(vdataotpr), evl(vdataotpr, NULL), ctod(substr(vdataotpr, 4, 2) + " +
            "\".\" + substr(vdataotpr, 1, 2) + \".\" + substr(vdataotpr, 7, 4))) as date) as vdataotpr1, vdataotpr, " +
            "vkomu, vhapr_spr FROM Data\\obida WHERE obida.Itemperson = ? ORDER BY vdataotpr1";
            List<LetterModel> petitions = await GetItemsAsync(args[0], Query, (reader) =>
            {
                return new LetterModel
                {
                    Date = DateTime.Parse((string)reader["vdataotpr"]),
                    Adress = (reader["vkomu"].ToString()!.Trim(' ') + " " + Tools.IsEmpty(AkusService.QueryPc((string)reader["vhapr_spr"], "pc5"))).Trim(' ')
                };
            });
            Query = "SELECT cast(iif(empty(vd), evl(vd, NULL), ctod(substr(vd, 4, 2) + " +
                "\".\" + substr(vd, 1, 2) + \".\" + substr(vd, 7, 4))) as date) as vd1, vd, " +
                "vinout, vadresat, vadres FROM Data\\pism WHERE (pism.Itemperson = ?) AND (pism.vinout = 1) ORDER BY vd1";
            List<LetterModel> letters = await GetItemsAsync(args[0], Query, (reader) =>
            {
                return new LetterModel
                {
                    Date = DateTime.Parse((string)reader["vd"]),
                    Adress = reader["vadresat"].ToString()?.Trim(' ') + ", " + reader["vadres"].ToString()?.Trim(' ')
                };
            });
            Model.OutputLetters = new List<LetterModel>();
            Model.OutputLetters.AddRange(petitions);
            Model.OutputLetters.AddRange(letters);
            Model.OutputLetters.Sort(new DateComparer());
            Query = "SELECT cast(iif(empty(vd), evl(vd, NULL), ctod(substr(vd, 4, 2) + " +
                "\".\" + substr(vd, 1, 2) + \".\" + substr(vd, 7, 4))) as date) as vd1, vd, " +
                "vinout, vadresat, vadres FROM Data\\pism WHERE (pism.Itemperson = ?) AND (pism.vinout = 2) ORDER BY vd1";
            Model.InputLetters = await GetItemsAsync(args[0], Query, (reader) =>
            {
                return new LetterModel
                {
                    Date = DateTime.Parse((string)reader["vd"]),
                    Adress = reader["vadresat"].ToString()?.Trim(' ') + ", " + reader["vadres"].ToString()?.Trim(' ')
                };
            });
            Model.CountIn = Model.InputLetters.Count;
            Model.CountOut = Model.OutputLetters.Count;
            documentOutput = GenerateReportFileName(currentPrisoner.GetShortName());
            var documentInput = DocumentFactory.Create(Path.Combine("Reports/Templates", args[1]), Model);
            documentInput.Generate(Path.Combine("Reports/Output", documentOutput), Model);
        });
        return await Task.FromResult(Path.Combine("/Documents", documentOutput));
    }

    private string GenerateReportFileName(string prisonerName)
    {
        return OutputFileName = prisonerName + " - справка по корреспонденции-" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".docx";
    }

    public Task RemoveReport()
    {
        File.Delete(Path.Combine("Reports/Output", OutputFileName));
        return Task.CompletedTask;
    }
}
