using System.Collections.Generic;

namespace wSHSApp.Reports.Models;

public class LetterReportModel
{
    public string? Name0 { get; set; }
    public string? FullName2 { get; set; }
    public string? Name3 { get; set; }
    public string? Birthday { get; set; }
    public int CountIn { get; set; }
    public int CountOut { get; set; }
    public List<LetterModel> InputLetters { get; set; }
    public List<LetterModel> OutputLetters { get; set; }
}

public class LetterModel
{
    public int Id { get; set; }
    public string? Whom { get; set; }
    public string? Where { get; set; }
}