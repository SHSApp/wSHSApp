using FSLib.Declension;
using wSHSApp.Data;
using wSHSApp.Models;
using wSHSApp.Reports.DisciplineReport.Data;

namespace wSHSApp.Reports.DisciplineReport.Model;

public class DisciplineReportModel
{
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? RejectTime { get; set; }
    public string? RejectPlace { get; set; }
    public string? RejectVideoReg { get; set; }
    public string? FixationText { get; set; }
    public string? HeadText { get; set; }
    public string? GroupIdText { get; set; }
    public string? YearText { get; set; }
    public Violation? Violation { get; set; }
    public AkusPrisoner? Prisoner { get; set; }
    public Employee? Dpnu { get; set; }
    public Employee? Emploee1 { get; set; }
    public Employee? Emploee2 { get; set; }
    public Employee? Emploee3 { get; set; }
    public Employee? Head { get; set; }
}

public static class EmployeeExtension
{
    public static string Decl(this Employee e, int declension)
    {
        return Declension1251.GetAppointmentDeclension(e.Position!.ShortName, (DeclensionCase)declension) + " " +
            Declension1251.GetAppointmentDeclension(e.Rank!.FullName!, (DeclensionCase)declension) + " внутренней службы " +
            Declension1251.GetSNPDeclension(e.ShortName, Gender.MasculineGender, (DeclensionCase)declension);
    }
    public static string DeclName(this Employee e, int declension, bool swap)
    {
        string res = Declension1251.GetSNPDeclension(e.ShortName, Gender.MasculineGender, (DeclensionCase)declension);
        res = swap ? Tools.SwapSNL(res) : res;
        return res;
    }
    public static string DeclName(this Employee e, int declension) => DeclName(e,declension, false);
    public static string DeclRank(this Employee e, int declension) => Declension1251.GetAppointmentDeclension(e.Rank!.FullName!, (DeclensionCase)declension) + " внутренней службы ";
    public static string Nominative(this Employee e) => $"{e.Position?.ShortName} {e.Rank?.FullName} внутренней службы {e.ShortName}";
    public static string ShortRecord(this Employee e) => $"{e.Position?.ShortName} {e.Rank?.VeryShortName} в/с {e.ShortName}";
    public static string PositionProper(this Employee e) => Tools.Proper(e.Position?.ShortName!);

}

public static class AkusPrisonerExtension
{
    public static string Decl(this AkusPrisoner ap, int declension, bool getShortName)
    {
        return Declension1251.GetSNPDeclension(ap.ToString(!getShortName), Gender.MasculineGender, (DeclensionCase)declension);
    }
    public static string Decl(this AkusPrisoner ap, int declension) => Decl(ap, declension, false);
}