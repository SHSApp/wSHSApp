namespace wSHSApp.Models;

public class AkusPrisoner
{
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string? Itemperson { get; set; }
    public string? GroupId { get; set; }
    public string GetFullName()
    {
        string res = Surname + " " + Name;
        if (!string.IsNullOrWhiteSpace(Lastname)) res += " " + Lastname;
        return res;
    }
    public string GetShortName()
    {
        string res = Surname + " " + Name?.ToUpper()[0] + "."; 
        if (!string.IsNullOrWhiteSpace(Lastname)) res += Lastname?.ToUpper()[0] + ".";
        return res;
    }
}
