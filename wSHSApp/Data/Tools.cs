using System;
using System.Collections.Generic;
using System.Text;

namespace wSHSApp.Data;

public static class Tools
{
    public static string DataBasePath = string.Empty;        

    public static string GetTerm(decimal years, decimal months, decimal days)
    {
        string res = "";
        if (years != 0) res += years.ToString() + GetDecl(Convert.ToInt32(years), " год ", " года ", " лет ");
        if (months != 0) res += months.ToString() + GetDecl(Convert.ToInt32(months), " месяц ", " месяца ", " месяцев ");
        if (days != 0) res += days.ToString() + GetDecl(Convert.ToInt32(days), " день", " дня", " дней");
        return res;
    }
    public static string GetTermShort(decimal years, decimal months, decimal days)
    {
        string res = "";
        if (years != 0) res += years.ToString() + GetDecl(Convert.ToInt32(years), " г. ", " г. ", " л. ");
        if (months != 0) res += months.ToString() + " м. ";
        if (days != 0) res += days.ToString() + " д.";
        return res + " л/св.";
    }
    public static string GetDecl(int num, string one, string two, string five)
    {
        int n = num % 100;
        if (n >= 5 && n <= 20)
        {
            return five;
        }
        n = num % 10;
        if (n == 1)
        {
            return one;
        }
        if (n >= 2 && n <= 4)
        {
            return two;
        }
        return five;
    }

    public static string Decl(string num, string one, string two, string five)
    {
        int convertedNum = Convert.ToInt32(num);
        int n = convertedNum % 100;
        if (n >= 5 && n <= 20)
        {
            return $"{num} {five}";
        }
        n = convertedNum % 10;
        if (n == 1)
        {
            return $"{num} {one}";
        }
        if (n >= 2 && n <= 4)
        {
            return $"{num} {two}";
        }
        return $"{num} {five}";
    }

    public static string Decl(int num, string one, string two, string five) => Decl(num.ToString(), one, two, five);

    public static List<TItem> ReverseArray<TItem>(List<TItem> src)
    {
        var dst = new List<TItem>();
        for (int i = src.Count - 1; i > -1; i--)
        {
            dst.Add(src[i]);
        }
        return dst;
    }

    public static string GetDecl(int num, string one, string two) => GetDecl(num, one, two, two);

    public static string GetMaritalStatus(decimal status) => status == 1 ? "Женат" : "Не женат";

    public static string IsEmpty(string src) => src.Length < 2 ? "" : src;
    
    public static string GetDepartureType(decimal type) => Convert.ToInt32(type) switch
    {
        1 => "Район",
        2 => "Колония",
        3 => "Колония-поселение",
        _ => "Неизвестно"
    };

    public static string CombineAdress(string Country, string Region, string City, string Adress)
    {
        string res = string.IsNullOrWhiteSpace(Country) ? "" : (AkusService.QueryPc(Country, "pc1") + ", ");
        res += string.IsNullOrWhiteSpace(Region) ? "" : (AkusService.QueryPc(Region, "pc2") + ", ");
        res += string.IsNullOrWhiteSpace(City) ? "" : (AkusService.QueryPc(City, "pc3") + ", ");
        res += Adress.TrimEnd(' ');
        return res;
    }

    public static string GetPrisonerStatus(decimal  vpr_osv, decimal  vubylpost, decimal  vumer)
    {
        string status = "В наличии";            
        if (vubylpost > 0) status = "Убыл";
        if (vpr_osv > 0) status = "Освободился";
        if (vumer > 0) status = "Умер";
        return status;
    }

    private static long Cton(string str, int p, int shift)
    {
        long nresult = 0;
        str = str.Replace(((char)33).ToString(), "");
        for (int k = 0; k < str.Length; k++)
        {
            nresult = nresult * p + Asc(str[k]) - shift;
        }
        return nresult;
    }

    public static int Asc(char c)
    {
        int converted = c;
        if (converted >= 0x80)
        {
            byte[] buffer = new byte[2];
            Encoding? enc = CodePagesEncodingProvider.Instance.GetEncoding(1251);
            if (enc?.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
            {
                converted = buffer[0];
            }
            else
            {
                converted = buffer[0] << 16 | buffer[1];
            }
        }
        return converted;
    }

    public static string GetFotoPath(string id)
    {
        if (id.Trim(' ').Length > 0)
        {
            string res = Convert.ToString(Cton(id, 142, 34))[..14];
            return "/Foto/" + res[..7] + "/" + res + ".jpg";
        }
        else return "";
    }

    public static string ExPackageNote(string src, string dst)
    {
        if ((src+dst).ToLower().Contains("медиц") || (src + dst).ToLower().Contains("медец")) return "Медицинская";
        else if ((src + dst).ToLower().Contains("поощ")) return "По поощрению";
        else return "По положенности";
    }

    public static string ExPackageNote(string src) => ExPackageNote(src, "");

    public static string ExMeetingDuration(string src, decimal vid)
    {
        if (src.TrimEnd(' ').Length > 0)
        {
            int dur = Convert.ToInt32(src[0].ToString());
            string time = Convert.ToInt32(vid) == 1 ? GetDecl(dur, " день", " дня") : GetDecl(dur, " час", " часа");
            return dur + time;
        }
        else return "Не определена";
    }

    public static string GetPartUDO(decimal Part) => Convert.ToInt32(Part) switch
    {
        1 => "1/3",
        2 => "1/2",
        3 => "2/3",
        4 => "3/4",
        5 => "4/5",
        _ => "3/4"
    };

    public static string GetPartKP(decimal Part) => Convert.ToInt32(Part) switch
    {
        1 => "1/4",
        2 => "1/3",
        3 => "1/2",
        4 => "2/3",
        5 => "3/4",
        6 => "4/5",
        _ => "2/3"
    };
    
    public static string GetPartZMN(decimal Part) => Convert.ToInt32(Part) switch
    {
        1 => "1/3",
        2 => "1/2",
        3 => "2/3",
        4 => "3/4",
        5 => "4/5",
        _ => "2/3"
    };

    public static string GetPartPR(decimal Part) => Convert.ToInt32(Part) switch
    {
        1 => "1/4",
        2 => "1/3",
        3 => "1/2",
        4 => "3/4",
        5 => "4/5",
        _ => "1/2"
    };

    public static string Proper(string str) => string.IsNullOrEmpty(str) ? "" : char.ToUpper(str[0]) + str[1..];

}    
