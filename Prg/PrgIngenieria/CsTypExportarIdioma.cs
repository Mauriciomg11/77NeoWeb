using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypExportarIdioma
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Tb77lIdioma = new DataTable();

        public void Alimentar(string Vista, string Idioma)//
        {
            Tb77lIdioma.Columns.Add("NomCursor", typeof(string));
            Tb77lIdioma.Columns.Add("C01", typeof(string));
            Tb77lIdioma.Columns.Add("C02", typeof(string));
            Tb77lIdioma.Columns.Add("C03", typeof(string));
            Tb77lIdioma.Columns.Add("C04", typeof(string));
            Tb77lIdioma.Columns.Add("C05", typeof(string));
            Tb77lIdioma.Columns.Add("C06", typeof(string));
            Tb77lIdioma.Columns.Add("C07", typeof(string));
            Tb77lIdioma.Columns.Add("C08", typeof(string));
            Tb77lIdioma.Columns.Add("C09", typeof(string));
            Tb77lIdioma.Columns.Add("C10", typeof(string));
            Tb77lIdioma.Columns.Add("C11", typeof(string));
            Tb77lIdioma.Columns.Add("C12", typeof(string));
            Tb77lIdioma.Columns.Add("C13", typeof(string));
            Tb77lIdioma.Columns.Add("C14", typeof(string));
            Tb77lIdioma.Columns.Add("C15", typeof(string));
            Tb77lIdioma.Columns.Add("C16", typeof(string));
            Tb77lIdioma.Columns.Add("C17", typeof(string));
            Tb77lIdioma.Columns.Add("C18", typeof(string));
            Tb77lIdioma.Columns.Add("C19", typeof(string));
            Tb77lIdioma.Columns.Add("C20", typeof(string));
            Tb77lIdioma.Columns.Add("C21", typeof(string));
            Tb77lIdioma.Columns.Add("C22", typeof(string));
            Tb77lIdioma.Columns.Add("C23", typeof(string));
            Tb77lIdioma.Columns.Add("C24", typeof(string));
            Tb77lIdioma.Columns.Add("C25", typeof(string));
            Tb77lIdioma.Columns.Add("C26", typeof(string));
            Tb77lIdioma.Columns.Add("C27", typeof(string));
            Tb77lIdioma.Columns.Add("C28", typeof(string));
            Tb77lIdioma.Columns.Add("C29", typeof(string));
            Tb77lIdioma.Columns.Add("C30", typeof(string));
            Tb77lIdioma.Columns.Add("C31", typeof(string));
            Tb77lIdioma.Columns.Add("C32", typeof(string));
            Tb77lIdioma.Columns.Add("C33", typeof(string));
            Tb77lIdioma.Columns.Add("C34", typeof(string));
            Tb77lIdioma.Columns.Add("C35", typeof(string));
            Tb77lIdioma.Columns.Add("C36", typeof(string));
            Tb77lIdioma.Columns.Add("C37", typeof(string));
            Tb77lIdioma.Columns.Add("C38", typeof(string));
            Tb77lIdioma.Columns.Add("C39", typeof(string));
            Tb77lIdioma.Columns.Add("C40", typeof(string));
            Tb77lIdioma.Columns.Add("C41", typeof(string));
            Tb77lIdioma.Columns.Add("C42", typeof(string));
            Tb77lIdioma.Columns.Add("C43", typeof(string));
            Tb77lIdioma.Columns.Add("C44", typeof(string));
            Tb77lIdioma.Columns.Add("C45", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
            {
                string C01="", C02 = "", C03 = "", C04 = "", C05 = "", C06 = "", C07 = "", C08 = "", C09 = "", C10 = "", C11 = "", C12 = "", C13 = "", C14 = "",C15 = "", C16 = "";
                string C17 = "", C18 = "", C19 = "", C20 = "", C21 = "", C22 = "", C23 = "", C24 = "", C25 = "", C26 = "", C27 = "", C28 = "", C29 = "", C30 = "", C31 = "", C32 = "", 
                    C33 = "", C34 = "", C35 = "", C36 = "", C37 = "", C38 = "", C39 = "";
                string LtxtSql = "EXEC Idioma @I,@F,'','',''";
                SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                SC.Parameters.AddWithValue("@I", Idioma);
                SC.Parameters.AddWithValue("@F", Vista);
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())
                {
                    C01 = tbl["Objeto"].ToString().Trim() == "C01" ? tbl["Texto"].ToString().Trim() : C01;
                    C02 = tbl["Objeto"].ToString().Trim() == "C02" ? tbl["Texto"].ToString().Trim() : C02;
                    C03 = tbl["Objeto"].ToString().Trim() == "C03" ? tbl["Texto"].ToString().Trim() : C03;
                    C04 = tbl["Objeto"].ToString().Trim() == "C04" ? tbl["Texto"].ToString().Trim() : C04;
                    C05 = tbl["Objeto"].ToString().Trim() == "C05" ? tbl["Texto"].ToString().Trim() : C05;
                    C06 = tbl["Objeto"].ToString().Trim() == "C06" ? tbl["Texto"].ToString().Trim() : C06;
                    C07 = tbl["Objeto"].ToString().Trim() == "C07" ? tbl["Texto"].ToString().Trim() : C07;
                    C08 = tbl["Objeto"].ToString().Trim() == "C08" ? tbl["Texto"].ToString().Trim() : C08;
                    C09 = tbl["Objeto"].ToString().Trim() == "C09" ? tbl["Texto"].ToString().Trim() : C09;
                    C10 = tbl["Objeto"].ToString().Trim() == "C10" ? tbl["Texto"].ToString().Trim() : C10;
                    C11 = tbl["Objeto"].ToString().Trim() == "C11" ? tbl["Texto"].ToString().Trim() : C11;
                    C12 = tbl["Objeto"].ToString().Trim() == "C12" ? tbl["Texto"].ToString().Trim() : C12;
                    C13 = tbl["Objeto"].ToString().Trim() == "C13" ? tbl["Texto"].ToString().Trim() : C13;
                    C14 = tbl["Objeto"].ToString().Trim() == "C14" ? tbl["Texto"].ToString().Trim() : C14;
                    C15 = tbl["Objeto"].ToString().Trim() == "C15" ? tbl["Texto"].ToString().Trim() : C15;
                    C16 = tbl["Objeto"].ToString().Trim() == "C16" ? tbl["Texto"].ToString().Trim() : C16;
                    C17 = tbl["Objeto"].ToString().Trim() == "C17" ? tbl["Texto"].ToString().Trim() : C17;
                    C18 = tbl["Objeto"].ToString().Trim() == "C18" ? tbl["Texto"].ToString().Trim() : C18;
                    C19 = tbl["Objeto"].ToString().Trim() == "C19" ? tbl["Texto"].ToString().Trim() : C19;
                    C20 = tbl["Objeto"].ToString().Trim() == "C20" ? tbl["Texto"].ToString().Trim() : C20;
                    C21 = tbl["Objeto"].ToString().Trim() == "C21" ? tbl["Texto"].ToString().Trim() : C21;
                    C22 = tbl["Objeto"].ToString().Trim() == "C22" ? tbl["Texto"].ToString().Trim() : C22;
                    C23 = tbl["Objeto"].ToString().Trim() == "C23" ? tbl["Texto"].ToString().Trim() : C23;
                    C24 = tbl["Objeto"].ToString().Trim() == "C24" ? tbl["Texto"].ToString().Trim() : C24;
                    C25 = tbl["Objeto"].ToString().Trim() == "C25" ? tbl["Texto"].ToString().Trim() : C25;
                    C26 = tbl["Objeto"].ToString().Trim() == "C26" ? tbl["Texto"].ToString().Trim() : C26;
                    C27 = tbl["Objeto"].ToString().Trim() == "C27" ? tbl["Texto"].ToString().Trim() : C27;
                    C28 = tbl["Objeto"].ToString().Trim() == "C28" ? tbl["Texto"].ToString().Trim() : C28;
                    C29 = tbl["Objeto"].ToString().Trim() == "C29" ? tbl["Texto"].ToString().Trim() : C29;
                    C30 = tbl["Objeto"].ToString().Trim() == "C30" ? tbl["Texto"].ToString().Trim() : C30;
                    C31 = tbl["Objeto"].ToString().Trim() == "C31" ? tbl["Texto"].ToString().Trim() : C31;
                    C32 = tbl["Objeto"].ToString().Trim() == "C32" ? tbl["Texto"].ToString().Trim() : C32;
                    C33 = tbl["Objeto"].ToString().Trim() == "C33" ? tbl["Texto"].ToString().Trim() : C33;
                    C34 = tbl["Objeto"].ToString().Trim() == "C34" ? tbl["Texto"].ToString().Trim() : C34;
                    C35 = tbl["Objeto"].ToString().Trim() == "C35" ? tbl["Texto"].ToString().Trim() : C35;
                    C36 = tbl["Objeto"].ToString().Trim() == "C36" ? tbl["Texto"].ToString().Trim() : C36;
                    C37 = tbl["Objeto"].ToString().Trim() == "C37" ? tbl["Texto"].ToString().Trim() : C37;
                    C38 = tbl["Objeto"].ToString().Trim() == "C38" ? tbl["Texto"].ToString().Trim() : C38;
                    C39 = tbl["Objeto"].ToString().Trim() == "C39" ? tbl["Texto"].ToString().Trim() : C39;
                }
                Tb77lIdioma.Rows.Add(Vista.Trim(),C01, C02, C03, C04, C05, C06, C07, C08, C09, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25,
                    C26, C27, C28, C29, C30, C31, C32, C33, C34, C35, C36, C37, C38, C39, "40", "41", "42", "43", "44", "45");

                Cnx.SelecBD();
                using (SqlConnection sqlCon2 = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon2.Open();
                    using (SqlTransaction transaction = sqlCon2.BeginTransaction())
                    {

                        string VBQuery = "ExportIdioma";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon2, transaction))
                        {
                            try
                            {
                                sqlCmd.CommandType = CommandType.StoredProcedure;
                                SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@Curidioma", Tb77lIdioma);
                                Prmtrs.SqlDbType = SqlDbType.Structured;
                                SqlDataReader SDR = sqlCmd.ExecuteReader();
                                if (SDR.Read())
                                {
                                    string PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();
                                transaction.Commit();
                                sqlCon.Close();

                            }
                            catch (Exception Ex)
                            {
                                string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                                VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                                VbPantalla = "FrmAeronave";
                                VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                                VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypAeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }          
           
        }        
    }
}