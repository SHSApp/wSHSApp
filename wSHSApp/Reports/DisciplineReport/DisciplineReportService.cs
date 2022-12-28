using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using wSHSApp.Data;
using wSHSApp.Models;
using wSHSApp.Reports.DisciplineReport.Model;
using SharpDocx;
using wSHSApp.Reports.DisciplineReport.Data;
using System.Drawing.Text;
using wSHSApp.Components;

namespace wSHSApp.Reports.DisciplineReport;

public class DisciplineReportService : InfoService, IReport
{
    public string Name { set; get; } = "Материал на осужденного";
    public string Description { get; set; } = "Макрос формирует рапорт о нарушении осужденным ПВР, для привлечения его к дисциплинарной ответственности.";
    public string Author { get; set; } = "© Сергей Шевченко";
    public string OutputFileName { set; get; } = "Материал на осужденного";
    private readonly DataRepo? repo;

    public DisciplineReportService(DataRepo Repo) => repo = Repo;

    public async Task<string> EntryPoint(string[] args)
    {
        var documentOutput = string.Empty;
        await Task.Run(async () =>
        {
            AkusPrisoner? currentPrisoner = AkusConnect.Names.FirstOrDefault(p => p.Itemperson == args[0]);
            string query = "SELECT vosustatia, vsroklet, vsrokmes, vsrokdney, vnachsroka, vkonecsrok FROM Data\\card WHERE itemperson = ?"; ;
            currentPrisoner!.SettingData = await GetItemAsync(args[0], query, reader =>
            {
                return $"ст. {AkusService.QueryPc((string)reader["vosustatia"], "pc8")} УК РФ, " +
                $"срок {Tools.GetTermShort((decimal)reader["vsroklet"], (decimal)reader["vsrokmes"], (decimal)reader["vsrokdney"])}, начало срока " +
                $"{(string)reader["vnachsroka"]}, конец срока {(string)reader["vkonecsrok"]}";
            });
            Employee dpnu = repo?.Personal?.First(i => i.Id == Convert.ToInt32(args[12]))!;
            Employee head = repo?.Personal?.First(i => i.Id == Convert.ToInt32(args[20]))!;
            string fixationText = Convert.ToInt32(args[5]) == 1 ? $"видеорегистратор «Дозор № {args[6]}»." 
            : $"видеокамеру № {args[7]}, монитор № {args[8]}. " +
            $"О данном факте нарушения информирован {dpnu.Position?.ShortName} {dpnu.Rank?.VeryShortName} в/с {dpnu.ShortName}";
            var Model = new DisciplineReportModel()
            {
                Prisoner = currentPrisoner,
                Date = args[2],
                Time = $"{Tools.Decl(args[3], "час", "часа", "часов")} {Tools.Decl(args[4], "минуту", "минуты", "минут")}",
                RejectTime = $"{Tools.Decl(args[15], "час", "часа", "часов")} {Tools.Decl(args[16], "минуту", "минуты", "минут")}",
                RejectPlace = repo?.Places?.First(i => i.Id == Convert.ToInt32(args[13])).Value.Replace("[Номер отряда]", AkusService.GetGroupDecl(currentPrisoner?.GroupId!), StringComparison.OrdinalIgnoreCase),
                RejectVideoReg = args[14],
                Violation = ReplaceSubstrings(currentPrisoner!, Convert.ToInt32(args[10]), Convert.ToInt32(args[17]), Convert.ToInt32(args[11]), Convert.ToInt32(args[9])),
                FixationText = fixationText,
                HeadText = head.PositionId == 23 ? "Начальнику" : "Врио начальника",
                Dpnu = dpnu,
                Emploee1 = repo?.Personal?.First(i => i.Id == Convert.ToInt32(args[17])),
                Emploee2 = repo?.Personal?.First(i => i.Id == Convert.ToInt32(args[18])),
                Emploee3 = repo?.Personal?.First(i => i.Id == Convert.ToInt32(args[19])),
                Head = head
            };

            documentOutput = GenerateReportFileName(currentPrisoner!.ToString());
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
    private Violation ReplaceSubstrings(AkusPrisoner prisoner, int violationId, int employeeId, int placeId, int cell)
    {
        Violation output = repo?.Violations?.First(i => i.Id == violationId)!;
        output.PreambleText = string.IsNullOrEmpty(output.PreambleText) ? "" : ReplaceSubstring(output.PreambleText);
        output.ViolationText = string.IsNullOrEmpty(output.ViolationText) ? "" : ReplaceSubstring(output.ViolationText);
        output.ViolationAdditionalText = string.IsNullOrEmpty(output.ViolationAdditionalText) ? "" : ReplaceSubstring(output.ViolationAdditionalText!);
        output.RulesText = string.IsNullOrEmpty(output.RulesText) ? "" : ReplaceSubstring(output.RulesText);
        output.VideoText = string.IsNullOrEmpty(output.VideoText) ? "" : ReplaceSubstring(output.VideoText!);
        output.RejectionText = string.IsNullOrEmpty(output.RejectionText) ? "" : ReplaceSubstring(output.RejectionText);
        string ReplaceSubstring(string input)
        {
            input = input.Replace("[Место нарушения]", repo?.Places?.First(i => i.Id == placeId).Value, StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО1]", prisoner.ToString(true), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО2]", prisoner.Decl(2), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО3]", prisoner.Decl(3), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО5]", prisoner.Decl(5), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[Дата рождения]", prisoner.Birthday, StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[Установочные данные]", prisoner.SettingData, StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[Номер камеры]", cell.ToString(), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО сотрудника полное1]", repo?.Personal?.First(i => i.Id == employeeId)!.Nominative(), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО сотрудника полное2]", repo?.Personal?.First(i => i.Id == employeeId)!.Decl(2), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО сотрудника полное3]", repo?.Personal?.First(i => i.Id == employeeId)!.Decl(3), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[ФИО сотрудника полное5]", repo?.Personal?.First(i => i.Id == employeeId)!.Decl(5), StringComparison.OrdinalIgnoreCase);
            input = input.Replace("[Номер отряда]", AkusService.GetGroupDecl(prisoner?.GroupId!), StringComparison.OrdinalIgnoreCase);
            foreach (var opt in repo?.Options!)
            {
                input = input.Replace($"[{opt.Name}]", opt.Value, StringComparison.OrdinalIgnoreCase);
            }            
            return input;
        }
        return output;
    }
}
