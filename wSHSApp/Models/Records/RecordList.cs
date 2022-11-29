using System;

namespace wSHSApp.Models.Records;

public enum RecordType
{
    Medicine,

}

public class Prisoner
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string Itemperson { get; set; }
    public string? GroupId { get; set; }
}

public class Record
{
    public int Id { get; set; }
    public RecordType Type { get; set; }
    public DateTime Date { get; set; }
    public int PrisonerId { get; set; }
    public Prisoner Prisoner { get; set; }
    public string? Description { get; set; }
}
