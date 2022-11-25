using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace WeKus
{
    class Qualif
    {
        private OleDbConnection Conn = null;
        private OleDbCommand oCmd = null;
        private DataTable result = null;
        private bool state = false;
        private static string oQualif = "";
        public static string ConnectionString = "Provider=VFPOLEDB.1;Data Source=C:\\IK\\Database\\Qualif\\;Password=\"\";Collating Sequence=MACHINE;";
        //public static string ConnectionString = @"DRIVER={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=C:\\IK\\Database;Exclusive=No;BackgroundFetch=No;Collate=Machine;Null=No;Deleted=No;";

        public void SetDataBasePath(string path)
        {
            ConnectionString = "Provider=VFPOLEDB.1;Data Source=" + path + "Qualif\\;Password=\"\";Collating Sequence=MACHINE;";
        }

        public Qualif()
        {
            Conn = new OleDbConnection();
            result = new DataTable();
            state = false;
        }

        public void OpenQualif(string name)
        {
            if (Conn != null)
            {
                try
                {
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    state = true;
                    oCmd = Conn.CreateCommand();
                    oQualif = name;
                    result.Reset();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    state = false;
                }
            } else { state = false; }
        }

        public void ChangeQualif(string name)
        {
            ChangeQualif(name, false);
        }

        public void ChangeQualif(string name, bool reset)
        {
            oQualif = name;
            if (reset) result.Reset();
        }

        public void CloseOpenedQualif()
        {
            if (state)
            {
                Conn.Close();
                state = false;
                result.Reset();
            }
        }

        public bool Opened
        {
            get
            {
               return state;
            }
        }

        public string GetFromQualif(string ID, string qualif)
        {
            OpenQualif(qualif);
            string res = GetFromQualifByID(ID);
            CloseOpenedQualif();
            return res;
        }

        public string GetFromQualifByID(string ID)
        {
            if (state)
            {
                if (oCmd.Parameters.Count == 0) oCmd.Parameters.Add(new OleDbParameter("id", OleDbType.WChar));         
                oCmd.Parameters["id"].Value = ID;
                oCmd.CommandText = "SELECT item, name FROM " + oQualif + " WHERE item == ?";
                result.Load(oCmd.ExecuteReader());
                if (result.Rows.Count > 0) return result.Rows[result.Rows.Count - 1][1].ToString().TrimEnd(' ');
                else return "";
            }
            else return "";
        }

        public string GetStatiaSimpleByID(string ID)
        {
            int IDcount = ID.TrimEnd(' ').Length / 2;
            string res = "ст. ";
            OpenQualif("pc8");
            for(int i = 0; i < IDcount; i++)
            {
                if (i == (IDcount - 1))
                {
                    res += GetFromQualifByID(ID.Substring(2 * i, 2)) + " УК РФ";
                }
                else
                {
                    res += GetFromQualifByID(ID.Substring(2 * i, 2)) + ", ";
                }
            }
            CloseOpenedQualif();
            return res;
        }

        public string GetNomerOtryada(string ID)
        {
            OpenQualif("pc52");
            string otrjad = GetFromQualifByID(ID);
            CloseOpenedQualif();
            if (otrjad == "") otrjad = "Карантин";
            return otrjad;
        }

        public string GetNomerOtryadaByID(string ID, int padej)
        {
            OpenQualif("pc52");
            string otrjad = GetFromQualifByID(ID);
            CloseOpenedQualif();
            if (otrjad == "") otrjad = "Карантин";
            string res = "";
            if (padej == 0)
            {
                if (otrjad == "Карантин")
                {
                    res = "отделение \"Карантин\"";
                }
                else
                {
                    res = "отряд № " + otrjad;
                }
            }
            else
            {
                if (otrjad == "Карантин")
                {
                    res = "отделения \"Карантин\"";
                }
                else
                {
                    res = "отряда № " + otrjad;
                }
            }
            return res;
        }

        public string GetOtkudaPribByID(string ID)
        {
            OpenQualif("pc5");
            string res = GetFromQualifByID(ID);
            CloseOpenedQualif();
            if (res != "")
            {
                if ((res[0] != 'Ф') && (res[0] != 'П')) { res = "ФКУ " + res; }
                return res;
            }
            else return "";
        }

        public string GetRegionAdress(string gos, string obl, string gor)
        {
            string res = "";
            OpenQualif("pc1");
            if (gos.Trim(' ') != "") res += GetFromQualifByID(gos);
            ChangeQualif("pc2", true);
            if (obl.Trim(' ') != "") res += ", " + GetFromQualifByID(obl);
            ChangeQualif("pc3", true);
            if (gor.Trim(' ') != "") res += ", " + GetFromQualifByID(gor);
            CloseOpenedQualif();
            res = Tools.ConvertRegion(res);
            return res;
        }

        public string GetGrajdanstvo(string id)
        {
            string res = "";
            OpenQualif("pc1");
            if (id.Trim(' ') != "") res += GetFromQualifByID(id);
            CloseOpenedQualif();
            if (res == "") res = "без гражданства";
            return res;
        }

        public string GetObrazovanie(string id)
        {
            string res = "";
            OpenQualif("pc22");
            if (id.Trim(' ') != "") res += GetFromQualifByID(id);
            CloseOpenedQualif();
            if (res == "") res = "без образования";
            return res;
        }

        public string GetStatiaFullByID(string ID)
        {
            int IDcount = ID.TrimEnd(' ').Length / 2;
            string res = "";
            OpenQualif("pc8");
            for (int i = 0; i < IDcount; i++)
            {
                string tmp = GetFromQualifByID(ID.Substring(2 * i, 2));
                string pu = "";
                string ch = "";
                string st = "";
                tmp = tmp.ToLower().Replace(" ", "");
                if (tmp.IndexOf("п.") > 1)
                {
                    pu = tmp.Substring(tmp.IndexOf("п.") + 2, tmp.Length - tmp.IndexOf("п.") - 2);
                    ch = tmp.Substring(tmp.IndexOf("ч.") + 2, tmp.IndexOf("п.") - tmp.IndexOf("ч.") - 2);
                    st = tmp.Substring(0, tmp.IndexOf("ч."));
                    if (pu.Length > 3) res += "пунктам "; else res += "пункту ";
                    res += pu + " части " + ch + " статьи "+ st;
                }
                else
                {
                    if (tmp.IndexOf("ч.") > 1)
                    {
                        ch = tmp.Substring(tmp.IndexOf("ч.") + 2, tmp.Length - tmp.IndexOf("ч.") - 2);
                        st = tmp.Substring(0, tmp.IndexOf("ч."));
                        res += "части " + ch + " статьи " + st;
                    }
                    else
                    {
                        res += "статьи " + tmp;
                    }
                }
                if (i == (IDcount - 1))
                {
                    res += " УК РФ";
                }
                else
                {
                    res += ", ";
                }
            }
            CloseOpenedQualif();
            return res;
        }

    }
}
