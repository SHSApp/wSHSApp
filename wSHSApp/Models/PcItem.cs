using System;

namespace wSHSApp.Models;

public class AppData
{
    private string currentItemperson = "";
    public string CurrentItemperson
    {
        get => currentItemperson;
        set { currentItemperson = value; OnChange?.Invoke(); }
    }

    public event Action? OnChange;
}

public class PcItem
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}
public class DisciplItem
{
    public string? Date { get; set; }
    public string? Violation { get; set; }
    public string? Penalty { get; set; }
    public string? Term { get; set; }
    public string? Mark { get; set; }
    public bool IsExtraData { get; set; }
    /// <summary>
    /// Дата нарушения
    /// </summary>
    public string? DateOfViolation { get; set;}
    /// <summary>
    /// Кто вынес решение
    /// </summary>
    public string? WhoMadeDecision { get; set; }
    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
}
public class AwardItem
{
    public string? Date { get; set; }
    public string? Reason { get; set; }
    public string? Gift { get; set; }
}
public class PackageItem
{
    public string? Date { get; set; }
    public string? Type { get; set; }
    public string? Note { get; set; }
    public bool IsExtraData { get; set; }
    /// <summary>
    /// Кто передавал
    /// </summary>
    public string? WhoPassed { get; set; }
    /// <summary>
    /// Кто принимал
    /// </summary>
    public string? WhoTook { get; set;}
    /// <summary>
    /// Примечание дополнительное
    /// </summary>
    public string? Note2 { get; set; }
}

public class MeetingItem
{
    public string? Date { get; set; }
    public string? Type { get; set; }
    public string? Duration { get; set; }
    public bool IsExtraData { get; set; }
    /// <summary>
    /// С кем свидание
    /// </summary>
    public string? WhoIsDate { get; set; }
    /// <summary>
    /// Степень родства
    /// </summary>
    public string? Relation { get; set; }
    /// <summary>
    /// Кто проводил
    /// </summary>
    public string? WhoSpent { get; set; }
}

public class LetterItem
{
    public string? Date { get; set; }
    public string? Type { get; set; }
    public string? Count { get; set; }
    public bool IsExtraData { get; set;}
    /// <summary>
    /// Кому
    /// </summary>
    public string? ToWhom { get; set; }
    /// <summary>
    /// Куда
    /// </summary>
    public string? Where { get; set; }
}

public class PetitionItem
{
    public string? Date { get; set; }
    public string? Type { get; set; }
    public string? ID { get; set; }
    public string? Destination { get; set; }
}

public class DebtItem
{
    /// <summary>
    /// Дата выдачи испольнительного листа
    /// </summary>
    public string? Date { get; set; }
    /// <summary>
    /// Номер исполнительного листа
    /// </summary>
    public string? ID { get; set; }
    /// <summary>
    /// Кем выдан
    /// </summary>
    public string? IssuedBy { get; set; }
    /// <summary>
    /// Основание
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// Взыскатель
    /// </summary>
    public string? Claimant { get; set; }
    /// <summary>
    /// Сумма иска
    /// </summary>
    public string? Total { get; set; }
    /// <summary>
    /// Алименты
    /// </summary>
    public string? Alimony { get; set; }
    /// <summary>
    /// Основание для прекращения
    /// </summary>
    public string? EndReason { get; set; }
}

public class AccountingItem
{
    public string? StartDate { get; set; }
    public string? Category { get; set; }
    public string? EndDate { get; set; }
    public bool IsExtraData { get; set; }
    /// <summary>
    /// Номер учетной карточки
    /// </summary>
    public string? AccountNumber { get; set; }
    /// <summary>
    /// Кто закреплен
    /// </summary>
    public string? WhoIsFixed { get; set; }
    /// <summary>
    /// Дата вынесения решения комиссией
    /// </summary>
    public string? DecisionDate { get; set; }
    /// <summary>
    /// Номер протокола постановки
    /// </summary>
    public string? SettingProtocolNumber { get; set; }
    /// <summary>
    /// Номер протокола снятия
    /// </summary>
    public string? TakeOffProtocolNumber { get; set; }
}
