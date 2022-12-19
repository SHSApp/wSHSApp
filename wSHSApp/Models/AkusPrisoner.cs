namespace wSHSApp.Models;

public class AkusPrisoner
{
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string? Itemperson { get; set; }
    public string? GroupId { get; set; }
    public string ToString(bool getFullname) =>
        getFullname ? $"{Surname} {Name}{(!string.IsNullOrWhiteSpace(Lastname) ? " " + Lastname : "")}"
        : $"{Surname} {Name?.ToUpper()[0]}.{(!string.IsNullOrWhiteSpace(Lastname) ? Lastname?.ToUpper()[0] + "." : "")}";
    public override string ToString() => ToString(false);
}
