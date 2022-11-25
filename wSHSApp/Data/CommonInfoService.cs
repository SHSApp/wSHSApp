using System;
using System.Data.OleDb;
using System.Threading.Tasks;
using wSHSApp.Models;

namespace wSHSApp.Data;

public class CommonInfoService : InfoService
{
    private static readonly string ConnectionString = "Provider=VFPOLEDB.1;Data Source=" +
        Tools.DataBasePath + ";Password=\"\";Collating Sequence=MACHINE;";

    public async Task<CommonInfo> GetCommonInfoAsync(string? Itemperson, bool IsExtraData)
    {
        CommonInfo commonInfo = new();
        if (!string.IsNullOrEmpty(Itemperson))
        {
            await Task.Run(async () =>
            {
                try
                {
                    OleDbConnection Conn = new(ConnectionString);
                    Conn.Open();
                    OleDbCommand oCmd = Conn.CreateCommand();
                    if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("itemperson", OleDbType.WChar));
                    oCmd.Parameters["itemperson"].Value = Itemperson;
                    string ExtraQuery = "";
                    if (IsExtraData) ExtraQuery = ", vmestorgos, vmestorobl, vmestorgor, vmestoradr, vmestopgos, vmestopobl, vmestopgor, " +
                        "vmestopadr, vrabota, vdoljnost, vobrazov, vsemia, vpr_osv, vubylpost, vumer, vdataposdo, vosvdata, vosvorg, " +
                        "vosvosn, vosvharak, vosvgos, vosvkr, vosvgr, vosvmesto";
                    oCmd.CommandText = "SELECT proper(vfamily) AS vsurname, vname, vlastname, vdatar, vosustatia, vsroklet, " +
                        "vsrokmes, vsrokdney, vnachsroka, vkonecsrok, vnomotr, vdataprib, votkudauch, votkudapr, " +
                        "vdatavstup, vnation, vgrajdanst, vosudata, vsudname, vsrokudo, vdataudo, vsrokposel, " +
                        "vdataposel, change, data1_ch, vsrok_pr, d1_pr, vfotofas, vdataotudo, vdataotpos, data2_ch, d2_pr, " +
                        "vfotoprof, vlichndelo, vardata, vuslovia, vpr_osv, vubylpost, vumer" + ExtraQuery + " FROM Data\\card WHERE itemperson = ?";
                    OleDbDataReader reader = oCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        commonInfo.Surname = (string)reader["vsurname"];
                        commonInfo.Name = (string)reader["vname"];
                        commonInfo.Lastname = (string)reader["vlastname"];
                        commonInfo.Birthday = (string)reader["vdatar"];
                        commonInfo.Articles = AkusService.QueryPc((string)reader["vosustatia"], "pc8");
                        commonInfo.Term = Tools.GetTerm((decimal)reader["vsroklet"], (decimal)reader["vsrokmes"], (decimal)reader["vsrokdney"]);
                        commonInfo.BeginTerm = (string)reader["vnachsroka"];
                        commonInfo.EndTerm = (string)reader["vkonecsrok"];
                        commonInfo.GroupId = AkusService.Get((string)reader["vnomotr"], "pc52");
                        commonInfo.ArrivalDate = (string)reader["vdataprib"];
                        commonInfo.ArrivalRegion = AkusService.QueryPc((string)reader["votkudapr"], "pc4");
                        commonInfo.ArrivalInstitution = AkusService.QueryPc((string)reader["votkudauch"], "pc5");
                        commonInfo.VerdictDate = (string)reader["vdatavstup"];
                        commonInfo.Nation = AkusService.QueryPc((string)reader["vnation"], "pc6");
                        commonInfo.Citizenship = AkusService.QueryPc((string)reader["vgrajdanst"], "pc1");
                        commonInfo.СondemnationDate = (string)reader["vosudata"];
                        commonInfo.CourtName = AkusService.QueryPc((string)reader["vsudname"], "pc5");
                        commonInfo.PartUDO = Tools.GetPartUDO((decimal)reader["vsrokudo"]);
                        commonInfo.DateUDO = (string)reader["vdataudo"];
                        commonInfo.PartKP = Tools.GetPartKP((decimal)reader["vsrokposel"]);
                        commonInfo.DateKP = (string)reader["vdataposel"];
                        commonInfo.PartZMN = Tools.GetPartZMN((decimal)reader["change"]);
                        commonInfo.DateZMN = (string)reader["data1_ch"];
                        commonInfo.PartPR = Tools.GetPartPR((decimal)reader["vsrok_pr"]);
                        commonInfo.DatePR = (string)reader["d1_pr"];
                        commonInfo.PathToFotoFas = Tools.GetFotoPath((string)reader["vfotofas"]);
                        commonInfo.PathToFotoProf = Tools.GetFotoPath((string)reader["vfotoprof"]);
                        commonInfo.PersonalNumber = (string)reader["vlichndelo"];
                        commonInfo.DetentionDate = (string)reader["vardata"];
                        commonInfo.Conditions = Tools.Proper(AkusService.Get((string)reader["vuslovia"], "pc70"));
                        commonInfo.Status = Tools.GetPrisonerStatus((decimal)reader["vpr_osv"], (decimal)reader["vubylpost"], (decimal)reader["vumer"]);
                        commonInfo.IsNotEmpty = true;
                        if (IsExtraData)
                        {
                            commonInfo.IsExtraData = true;
                            commonInfo.PlaceOfBirth = Tools.CombineAdress((string)reader["vmestorgos"], (string)reader["vmestorobl"],
                                (string)reader["vmestorgor"], (string)reader["vmestoradr"]);
                            commonInfo.PlaceOfRegistration = Tools.CombineAdress((string)reader["vmestopgos"], (string)reader["vmestopobl"],
                                (string)reader["vmestopgor"], (string)reader["vmestopadr"]);
                            commonInfo.PlaceOfWork = Tools.Proper((string)reader["vrabota"]);
                            commonInfo.JobTitle = Tools.Proper((string)reader["vdoljnost"]);
                            commonInfo.Education = Tools.Proper(AkusService.Get((string)reader["vobrazov"], "pc22"));
                            commonInfo.MaritalStatus = Tools.GetMaritalStatus((decimal)reader["vsemia"]);                         
                            if (commonInfo.Status.Contains("Освоб"))
                            {
                                commonInfo.Release = new()
                                {
                                    ResolutionDate = (string)reader["vdataposdo"],
                                    ReleaseDate = (string)reader["vosvdata"],
                                    CourtName = AkusService.QueryPc((string)reader["vosvorg"], "pc5"),
                                    Reason = AkusService.QueryPc((string)reader["vosvosn"], "pc11"),
                                    Type = AkusService.QueryPc((string)reader["vosvharak"], "pc12"),
                                    DepartureAdress = Tools.CombineAdress((string)reader["vosvgos"], (string)reader["vosvkr"],
                                        (string)reader["vosvgr"], (string)reader["vosvmesto"])
                                };
                            }
                        }
                    }
                    reader.Close();
                    Conn.Close();
                    if (IsExtraData)
                    {
                        string Query = "SELECT cast(iif(empty(vdata), evl(vdata, NULL), ctod(substr(vdata, 4, 2) + \".\" + substr(vdata, 1, 2) + " + 
                        "\".\" + substr(vdata, 7, 4))) as date) as vdata1, vdata, vuslov FROM Data\\uslovia WHERE Itemperson = ? ORDER BY vdata1";
                        commonInfo.ConditionList = await GetItemsAsync(Itemperson, Query, (reader) =>
                        {
                            return new ConditionItem()
                            {
                                Date = (string)reader["vdata"],
                                Condition = Tools.Proper(AkusService.Get((string)reader["vuslov"], "pc70"))
                            };
                        });
                        Query = "SELECT cast(iif(empty(vdataprib), evl(vdataprib, NULL), ctod(substr(vdataprib, 4, 2) + \".\" + substr(vdataprib, 1, 2) + " +
                        "\".\" + substr(vdataprib, 7, 4))) as date) as vdataprib1, vdataprib, vnumotr FROM Data\\otrjady WHERE Itemperson = ? ORDER BY vdataprib1";
                        commonInfo.MovingList = await GetItemsAsync(Itemperson, Query, (reader) =>
                        {
                            return new MovingItem()
                            {
                                Date = (string)reader["vdataprib"],
                                GroupId = AkusService.GetGroup((string)reader["vnumotr"])
                            };
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
        else commonInfo.IsNotEmpty = false;

        return await Task.FromResult(commonInfo);
    }

    public async Task<CommonInfo> GetCommonInfoAsync(string? Itemperson)
    {
        return await GetCommonInfoAsync(Itemperson, false);
    }

}
