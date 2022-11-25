namespace wSHSApp.Models;

public class ConditionItem
{
    public string? Date { get; set; }
    public string? Condition { get; set; }
}

public class MovingItem
{
    public string? Date { get; set; }
    public string? GroupId { get; set; }
}

public class CriminalItem
{
    public string? Date { get; set; }
    public string? CourtName { get; set; }
    public string? Articles { get; set; }
    public string? Mode { get; set; }
    public string? Text { get; set; }
    public string? Term { get; set; }
    public string? Note { get; set; }
    public string? ReleaseDate { get; set; }
    public string? ReleaseReason { get; set; }
}

public class RelativeItem
{
    public string? Relation { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Birthday { get; set; }
    public string? PlaceOfRegistration { get; set; }
}

public class DepartureItem
{
    public bool CurrentStatus { get; set; }
    public string? Type { get; set; }
    public string? DepartureDate { get; set; }
    public string? DepartureInstitution { get; set; }
    public string? DepartureRegion { get; set; }
    public string? ArrivalDate { get; set; }
    public string? ArrivalInstitution { get; set; }
    public string? ArrivalRegion { get; set; }
}

public class Release
{
    public string? ResolutionDate { get; set; }
    public string? ReleaseDate { get; set; }
    public string? CourtName { get; set; }
    public string? Reason { get; set; }
    public string? Type { get; set; }
    public string? DepartureAdress { get; set; }
}

public class DocumentItem
{
    public string? Name { get; set; }
    public bool Avail { get; set; }
    public string? Series { get; set; }
    public string? Number { get; set; }
    public string? DivisionCode { get; set; }
    public string? IssuedBy { get; set; }
    public string? IssuedOrganization { get; set; }
    public string? IssuedDate { get; set; }
}
