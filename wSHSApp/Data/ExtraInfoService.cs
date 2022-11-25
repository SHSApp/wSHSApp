using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wSHSApp.Models;

namespace wSHSApp.Data;

public class ExtraInfoService : InfoService
{
    public async Task<List<DisciplItem>> GetDisсiplInfoAsync(string? Itemperson, bool IsExtraData)
    {
        string ExtraQuery = "";
        if (IsExtraData) ExtraQuery = ", itemme, vdatanaru, vkem";
        string Query = "SELECT cast(iif(empty(vdata), evl(vdata, NULL), ctod(substr(vdata, 4, 2) + " +
            "\".\" + substr(vdata, 1, 2) + \".\" + substr(vdata, 7, 4))) as date) AS vdata1, vdata, vprichiny, " +
            "vvid, vsrok, vdatasnajt" + ExtraQuery + " FROM Data\\distipl WHERE distipl.Itemperson = ? ORDER BY vdata1";

        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            DisciplItem Item = new()
            {
                Date = (string)reader["vdata"],
                Violation = AkusService.Get((string)reader["vprichiny"], "pc56"),
                Penalty = AkusService.Get((string)reader["vvid"], "pc29"),
                Term = ((decimal)reader["vsrok"] == 0) ? "–" : Convert.ToString((decimal)reader["vsrok"]),
                Mark = (string)reader["vdatasnajt"]
            };
            if (IsExtraData) 
            {
                Item.IsExtraData = true;
                Item.DateOfViolation = (string)reader["vdatanaru"];
                Item.WhoMadeDecision = (string)reader["vkem"];
                Item.Note = AkusService.GetNote(Itemperson, (string)reader["itemme"], "_pr15");
            }
            return Item;
        }); 
    }

    public async Task<List<AwardItem>> GetAwardInfoAsync(string? Itemperson)
    {
        string Query = "SELECT cast(iif(empty(vdataob), evl(vdataob, NULL), ctod(substr(vdataob, 4, 2) + " +
            "\".\" + substr(vdataob, 1, 2) + \".\" + substr(vdataob, 7, 4))) as date) AS vdataob1, vdataob, " +
            "vzachto, vvidpoosh FROM Data\\pooshren WHERE pooshren.Itemperson = ? ORDER BY vdataob1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new AwardItem
            {
                Date = (string)reader["vdataob"],
                Reason = (string)reader["vzachto"],
                Gift = AkusService.Get((string)reader["vvidpoosh"], "pc37")
            };
        });
    }

    public async Task<List<PackageItem>> GetPackagesInfoAsync(string? Itemperson, bool IsExtraData)
    {
        string ExtraQuery = "";
        if (IsExtraData) ExtraQuery = ", itemme, votkogo, vdosmotr";
        string Query = "SELECT cast(iif(empty(vdata), evl(vdata, NULL), ctod(substr(vdata, 4, 2) + " +
            "\".\" + substr(vdata, 1, 2) + \".\" + substr(vdata, 7, 4))) AS date) as vdata1, vdata, " +
            "vposylka, votkogo, vdosmotr" + ExtraQuery + " FROM Data\\pered WHERE pered.Itemperson = ? ORDER BY vdata1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            PackageItem Item = new() 
            {
                Date = (string)reader["vdata"],
                Type = Convert.ToInt32((decimal)reader["vposylka"])==1?"Бандероль":"Посылка/Передача",
                Note = Tools.ExPackageNote((string)reader["votkogo"] + (string)reader["vdosmotr"])
            };
            if (IsExtraData)
            {
                Item.IsExtraData = true;
                Item.WhoPassed = (string)reader["votkogo"];
                Item.WhoTook = (string)reader["vdosmotr"];
                Item.Note2 = AkusService.GetNote(Itemperson, (string)reader["itemme"], "_pr9");
                Item.Note = Tools.ExPackageNote(Item.Note2, Item.Note);
            }
            return Item;
        });
    }

    public async Task<List<MeetingItem>> GetMeetingsInfoAsync(string? Itemperson, bool IsExtraData)
    {
        string ExtraQuery = "";
        if (IsExtraData) ExtraQuery = ", vskem, vsteprodst, vktoprovod";
        string Query = "SELECT cast(iif(empty(vdatanach), evl(vdatanach, NULL), ctod(substr(vdatanach, 4, 2) + " +
            "\".\" + substr(vdatanach, 1, 2) + \".\" + substr(vdatanach, 7, 4))) as date) as vdatanach1, vdatanach, " +
            "vvidsvid, vprodol" + ExtraQuery + " FROM Data\\svidan WHERE svidan.Itemperson = ? ORDER BY vdatanach1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            MeetingItem Item =  new()
            {
                Date = (string)reader["vdatanach"],
                Type = Convert.ToInt32((decimal)reader["vvidsvid"]) == 1 ? "Длительное" : "Краткосрочное",
                Duration = Tools.ExMeetingDuration((string)reader["vprodol"], (decimal)reader["vvidsvid"])
            };
            if (IsExtraData)
            {
                Item.IsExtraData = true;
                Item.WhoIsDate = (string)reader["vskem"];
                Item.Relation = AkusService.QueryPc((string)reader["vsteprodst"], "pc20");
                Item.WhoSpent = (string)reader["vktoprovod"];
            }
            return Item;
        });
    }

    public async Task<List<PetitionItem>> GetPetitionsInfoAsync(string? Itemperson)
    {
        string Query = "SELECT cast(iif(empty(vdataotpr), evl(vdataotpr, NULL), ctod(substr(vdataotpr, 4, 2) + " +
            "\".\" + substr(vdataotpr, 1, 2) + \".\" + substr(vdataotpr, 7, 4))) as date) as vdataotpr1, vdataotpr, " +
            "vvid, vnomish, vkomu, vhapr_spr FROM Data\\obida WHERE obida.Itemperson = ? ORDER BY vdataotpr1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new PetitionItem
            {
                Date = (string)reader["vdataotpr"],
                Type = AkusService.Get((string)reader["vvid"], "pc28"),
                ID = (string)reader["vnomish"],
                Destination = (string)reader["vkomu"] + " " + Tools.IsEmpty(AkusService.QueryPc((string)reader["vhapr_spr"], "pc5"))
            };
        });
    }

    public async Task<List<LetterItem>> GetLettersInfoAsync(string? Itemperson, bool IsExtraData)
    {
        string ExtraQuery = "";
        if (IsExtraData) ExtraQuery = ", vadresat, vadres";
        string Query = "SELECT cast(iif(empty(vd), evl(vd, NULL), ctod(substr(vd, 4, 2) + " +
            "\".\" + substr(vd, 1, 2) + \".\" + substr(vd, 7, 4))) as date) as vd1, vd, " +
            "vinout, vkol" + ExtraQuery + " FROM Data\\pism WHERE pism.Itemperson = ? ORDER BY vd1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            LetterItem Item = new ()
            {
                Date = (string)reader["vd"],
                Type = Convert.ToInt32((decimal)reader["vinout"]) == 2 ? "Входящее" : "Исходящее",
                Count = Convert.ToInt32((decimal)reader["vkol"]).ToString()
            };
            if (IsExtraData)
            {
                Item.IsExtraData = true;
                Item.ToWhom = (string)reader["vadresat"];
                Item.Where = (string)reader["vadres"];
            }
            return Item;
        });
    }

    public async Task<List<DebtItem>> GetDebtInfoAsync(string? Itemperson)
    {
        string Query = "SELECT cast(iif(empty(vdatavyd), evl(vdatavyd, NULL), ctod(substr(vdatavyd, 4, 2) + " +
            "\".\" + substr(vdatavyd, 1, 2) + \".\" + substr(vdatavyd, 7, 4))) as date) as vdatavyd1, vdatavyd, " +
            "vnumisplis, vkemvydan, vosnovan, vorgvzysk, valimenty, vuder, vuders, vosnovprek " + 
            "FROM Data\\isplist WHERE isplist.Itemperson = ? ORDER BY vdatavyd1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            return new DebtItem
            {
                Date = (string)reader["vdatavyd"],
                ID = (string)reader["vnumisplis"],
                IssuedBy = AkusService.QueryPc((string)reader["vkemvydan"], "pc5"),
                Reason = AkusService.Get((string)reader["vosnovan"], "pc34"),
                Claimant = (string)reader["vorgvzysk"],
                Total = Convert.ToString((decimal)reader["valimenty"]),
                Alimony = (decimal)reader["vuder"] > 0 ? Convert.ToString((decimal)reader["vuder"]) + "%" :
                (decimal)reader["vuders"] > 0 ? Convert.ToString((decimal)reader["vuders"]) + " руб." : "-",
                EndReason = (string)reader["vosnovprek"]
            };
        });
    }

    public async Task<List<AccountingItem>> GetAccountingInfoAsync(string? Itemperson, bool IsExtraData)
    {
        string ExtraQuery = "";
        if (IsExtraData) ExtraQuery = ", vnomuchkar, vktoprofr, vkomispost, vprotpost, vprotsnat";
        string Query = "SELECT cast(iif(empty(vdatapost), evl(vdatapost, NULL), ctod(substr(vdatapost, 4, 2) + " +
            "\".\" + substr(vdatapost, 1, 2) + \".\" + substr(vdatapost, 7, 4))) as date) AS vdatapost1, vdatapost, " +
            "vkategory, vdatasnyat" + ExtraQuery + " FROM Data\\profuch WHERE profuch.Itemperson = ? ORDER BY vdatapost1";
        return await GetItemsAsync(Itemperson, Query, (reader) =>
        {
            AccountingItem Item = new ()
            {
                StartDate = (string)reader["vdatapost"],
                Category = AkusService.Get((string)reader["vkategory"], "pc32"),
                EndDate = (string)reader["vdatasnyat"]
            };
            if (IsExtraData)
            {
                Item.IsExtraData = true;
                Item.AccountNumber = (string)reader["vnomuchkar"];
                Item.WhoIsFixed = (string)reader["vktoprofr"];
                Item.DecisionDate = (string)reader["vkomispost"];
                Item.SettingProtocolNumber = (string)reader["vprotpost"];
                Item.TakeOffProtocolNumber = (string)reader["vprotsnat"];
            }
            return Item;
        });
    }

    public async Task<List<DisciplItem>> GetDisсiplInfoAsync(string? Itemperson)
    {
        return await GetDisсiplInfoAsync(Itemperson, false);
    }

    public async Task<List<PackageItem>> GetPackagesInfoAsync(string? Itemperson)
    {
        return await GetPackagesInfoAsync(Itemperson, false);
    }

    public async Task<List<MeetingItem>> GetMeetingsInfoAsync(string? Itemperson)
    {
        return await GetMeetingsInfoAsync(Itemperson, false);
    }

    public async Task<List<LetterItem>> GetLettersInfoAsync(string? Itemperson)
    {
        return await GetLettersInfoAsync(Itemperson, false);
    }

    public async Task<List<AccountingItem>> GetAccountingInfoAsync(string? Itemperson)
    {
        return await GetAccountingInfoAsync(Itemperson, false);
    }
}
