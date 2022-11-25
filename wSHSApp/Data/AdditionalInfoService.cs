using System.Collections.Generic;
using System.Threading.Tasks;
using wSHSApp.Models;

namespace wSHSApp.Data;

public class AdditionalInfoService : InfoService
{
    public async Task<List<CriminalItem>> GetPreviousConvictionsInfoAsync(string? Itemperson)
    {
        string Query = "SELECT cast(iif(empty(vdataosu), evl(vdataosu, NULL), ctod(substr(vdataosu, 4, 2) + \".\" + substr(vdataosu, 1, 2) + " + 
            "\".\" + substr(vdataosu, 7, 4))) as date) AS vdataosu1, vdataosu, vkemosuj, vstatosuj, vvidnakaz, vsroklet, vsrokmes, " + 
            "vsrokdney, vprimechan, vdataosvob, vosnovosv, itemme FROM Data\\sudimost WHERE Itemperson = ? ORDER BY vdataosu1";

        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new CriminalItem
            {
                Date = (string)reader["vdataosu"],
                CourtName = AkusService.QueryPc((string)reader["vkemosuj"], "pc5"),
                Articles = AkusService.QueryPc((string)reader["vstatosuj"], "pc8"),
                Mode = AkusService.QueryPc((string)reader["vvidnakaz"], "pc24"),
                Text = AkusService.GetNote(Itemperson, (string)reader["itemme"], "_pr22"),
                Term = Tools.GetTerm((decimal)reader["vsroklet"], (decimal)reader["vsrokmes"], (decimal)reader["vsrokdney"]),
                Note = (string)reader["vprimechan"],
                ReleaseDate = (string)reader["vdataosvob"],
                ReleaseReason = AkusService.QueryPc((string)reader["vosnovosv"], "pc12")
            };
        });
    }

    public async Task<List<DepartureItem>> GetDeparturesInfoAsync(string? Itemperson)
    {
        string Query = "SELECT cast(iif(empty(vdataubyt), evl(vdataubyt, NULL), ctod(substr(vdataubyt, 4, 2) + \".\" + substr(vdataubyt, 1, 2) + " +
            "\".\" + substr(vdataubyt, 7, 4))) as date) AS vdataubyt1, vpriznak, vharakter, vdataubyt, vkudaubyl, vkudauchr, " + 
            "vdataprib, votkuda, votkudauch FROM Data\\peremesh WHERE Itemperson = ? ORDER BY vdataubyt1";

        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new DepartureItem
            {
                CurrentStatus = (decimal)reader["vpriznak"] == 1,
                Type = Tools.GetDepartureType((decimal)reader["vharakter"]),
                DepartureDate = (string)reader["vdataubyt"],
                DepartureInstitution = AkusService.QueryPc((string)reader["vkudauchr"], "pc5"),
                DepartureRegion = AkusService.QueryPc((string)reader["vkudaubyl"], "pc4"),
                ArrivalDate = (string)reader["vdataprib"],
                ArrivalInstitution = AkusService.QueryPc((string)reader["votkudauch"], "pc5"),
                ArrivalRegion = AkusService.QueryPc((string)reader["votkuda"], "pc4")
            };
        });
    }

    public async Task<List<DocumentItem>> GetDocumentsInfoAsync(string? Itemperson)
    {
        string Query = "SELECT vdokum, vnalichie, vseria, vnomer, kodprazd, vkemvydan, vkemuchr, vdatavyd FROM Data\\docum WHERE Itemperson = ?";

        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new DocumentItem
            {
                Name = AkusService.Get((string)reader["vdokum"], "pc36"),
                Avail = (decimal)reader["vnalichie"] == 1,
                Series = (string)reader["vseria"],
                Number = (string)reader["vnomer"],
                DivisionCode = (string)reader["kodprazd"],
                IssuedBy = (string)reader["vkemvydan"],
                IssuedOrganization = AkusService.QueryPc((string)reader["vkemuchr"], "pc5"),
                IssuedDate = (string)reader["vdatavyd"]
            };
        });
    }

    public async Task<List<RelativeItem>> GetRelativesInfoAsync(string? Itemperson)
    {
        string Query = "SELECT vstepen, proper(vfamily) AS vsurname, vname, vlastname, vdatar, " + 
            "vgosud, voblast, vgorodray, vadres FROM Data\\rodstv WHERE Itemperson = ?";

        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new RelativeItem
            {
                Relation = Tools.Proper(AkusService.QueryPc((string)reader["vstepen"], "pc20")),
                Surname = (string)reader["vsurname"],
                Name = (string)reader["vname"],
                Lastname = (string)reader["vlastname"],
                Birthday = (string)reader["vdatar"],
                PlaceOfRegistration = Tools.CombineAdress((string)reader["vgosud"], (string)reader["voblast"], 
                    (string)reader["vgorodray"], (string)reader["vadres"])
            };
        });
    }
}
