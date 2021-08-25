using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg
{
    public class ClsCierreContable
    {
        static public string PMensj;
        static public string PMes;
        static public string PAno;
        public string Mes { get; set; }
        public string Ano { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsCierreContable> oCC)
        {
            foreach (var Campo in oCC)
            {
                PMes = Campo.Mes;
                PAno = Campo.Ano;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    PMensj = "";
                    string VBQuery = " EXEC Consultas_General 25,@M,@Cia,@U,@A, 0,@ICC,'01-01-1','01-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        sqlCmd.Parameters.AddWithValue("@M", PMes);
                        sqlCmd.Parameters.AddWithValue("@Cia", System.Web.HttpContext.Current.Session["Nit77Cia"].ToString());
                        sqlCmd.Parameters.AddWithValue("@U", System.Web.HttpContext.Current.Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@A", PAno);
                        sqlCmd.Parameters.AddWithValue("@ICC", System.Web.HttpContext.Current.Session["!dC!@"].ToString());
                        try
                        {
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read()) { PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                            SDR.Close();
                            Transac.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmCierreContable";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsCierreContable", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            Transac.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        {
            return PMensj;
        }
    }
}