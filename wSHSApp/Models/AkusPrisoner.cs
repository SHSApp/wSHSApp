namespace wSHSApp.Models;

public class AkusPrisoner
{
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string? Itemperson { get; set; }
    public string? GroupId { get; set; }
    public string GetFullName() => Surname + " " + Name + (!string.IsNullOrWhiteSpace(Lastname) ? " " + Lastname : "");
    public string GetShortName() => Surname + " " + Name?.ToUpper()[0] + "." + (!string.IsNullOrWhiteSpace(Lastname) ? Lastname?.ToUpper()[0] + "." : "");
}
