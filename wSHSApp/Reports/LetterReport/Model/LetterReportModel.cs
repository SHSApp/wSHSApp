using System;
using System.Collections.Generic;

namespace wSHSApp.Reports.LetterReport.Model;

public class LetterReportModel
{
    public string? Name0 { get; set; }
    public string? FullName2 { get; set; }
    public string? Name3 { get; set; }
    public string? Birthday { get; set; }
    public int CountIn { get; set; }
    public int CountOut { get; set; }
    public List<LetterModel>? InputLetters { get; set; }
    public List<LetterModel>? OutputLetters { get; set; }
}

public class LetterModel
{
    public DateTime Date { get; set; }
    public string? Adress { get; set; }
}

class DateComparer : IComparer<LetterModel>
{
    public int Compare(LetterModel? x, LetterModel? y)
    {
        if (x?.Date == null || y?.Date == null) return 0;
        return x.Date.CompareTo(y.Date);
    }
}