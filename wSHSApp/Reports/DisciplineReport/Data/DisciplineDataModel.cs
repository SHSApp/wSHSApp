using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wSHSApp.Models;

namespace wSHSApp.Reports.DisciplineReport.Data;

//#pragma warning disable CS8618 

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}

[Table("Departments")]
public class Department : BaseEntity
{
    public required string ShortName { get; set; } 
    public string? FullName { get; set; }
}

[Table("Ranks")]
public class Rank : BaseEntity
{
    public required string VeryShortName { get; set; }
    public required string ShortName { get; set; }
    public string? FullName { get; set; }
}

[Table("Positions")]
public class Position : BaseEntity
{
    public required string ShortName { get; set; }
    public string? FullName { get; set; }
}

[Table("Personal")]
public class Employee : BaseEntity
{
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required int PositionId { get; set; }
    public Position? Position { get; set; }
    public required int RankId { get; set; }
    public Rank? Rank { get; set; }
    public required int DepartmentId { get; set; }
    public Department? Department { get; set; }
}

[Table("Places")]
public class Place : BaseEntity
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}

[Table("Material")]
public class Violation : BaseEntity
{
    public required string Name { get; set; }
    public required string PreambleText { get; set; }
    public required string ViolationText { get; set; }
    public string? ViolationAdditionalText { get; set; }
    public required string RulesText { get; set; }
    public string? VideoText { get; set; }
    public required string RejectionText { get; set; }
}

[Table("Options")]
public class Option : BaseEntity
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}

//#pragma warning restore CS8618