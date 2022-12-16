using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wSHSApp.Reports.DisciplineReport.Data;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}

[Table("Departments")]
public class Department : BaseEntity
{
    [Required]
    public string ShortName { get; set; }
    public string? FullName { get; set; }
}

[Table("Ranks")]
public class Rank : BaseEntity
{
    public string VeryShortName { get; set; }
    public string ShortName { get; set; }
    public string? FullName { get; set; }
}

[Table("Positions")]
public class Position : BaseEntity
{
    public string ShortName { get; set; }
    public string? FullName { get; set; }
}

[Table("Personal")]
public class Employee : BaseEntity
{
    [Required]
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; }
    public int RankId { get; set; }
    public Rank Rank { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}

[Table("Places")]
public class Place : BaseEntity
{
    public string Name { get; set; }
    public string Value { get; set; }
}

[Table("Material")]
public class Violation : BaseEntity
{
    public string Name { get; set; }
    public string PreambleText { get; set; }
    public string ViolationText { get; set; }
    public string? ViolationAdditionalText { get; set; }
    public string RulesText { get; set; }
    public string? VideoText { get; set; }
    public string RejectionText { get; set; }
}

[Table("Options")]
public class Option : BaseEntity
{
    public string Name { get; set; }
    public string Value { get; set; }
}