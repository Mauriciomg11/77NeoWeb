using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypPasosOT
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public int PCodIdPaso;
        public int IDPasos { get; set; }
        public string Paso { get; set; }
        public int OT { get; set; }
        public int Realizado { get; set; }
        public string DescripcionRealizado { get; set; }
        public string DocReferencia { get; set; }
        public string Discrepancia { get; set; }
        public DateTime FechaI { get; set; }
        public DateTime? FechaF { get; set; }
        public string Estado { get; set; }
        public double HHEst { get; set; }
        public double HHReal { get; set; }
        public string CodTecnico { get; set; }
        public string LicenciaTec { get; set; }
        public string CodInspector { get; set; }
        public string LicenciaInsp { get; set; }
        public string Notas { get; set; }
        public int Otro { get; set; }
        public string Usu { get; set; }
        public string CodLicenciaTecP { get; set; }
        public string CodLicenciaInsP { get; set; }
        public double HHRealInsp { get; set; }

        public void Alimentar(IEnumerable<CsTypPasosOT> TypPasos)//
        {
            DataTable TblPasos = new DataTable();
            TblPasos.Columns.Add("IDPasos", typeof(int));
            TblPasos.Columns.Add("Paso", typeof(string));
            TblPasos.Columns.Add("OT", typeof(int));
            TblPasos.Columns.Add("Realizado", typeof(int));
            TblPasos.Columns.Add("DescripcionRealizado", typeof(string));
            TblPasos.Columns.Add("DocReferencia", typeof(string));
            TblPasos.Columns.Add("Discrepancia", typeof(string));
            TblPasos.Columns.Add("FechaI", typeof(DateTime));
            TblPasos.Columns.Add("FechaF", typeof(DateTime));
            TblPasos.Columns.Add("Estado", typeof(string));
            TblPasos.Columns.Add("HHEst", typeof(double));
            TblPasos.Columns.Add("HHReal", typeof(double));
            TblPasos.Columns.Add("CodTecnico", typeof(string));
            TblPasos.Columns.Add("LicenciaTec", typeof(string));
            TblPasos.Columns.Add("CodInspector", typeof(string));
            TblPasos.Columns.Add("LicenciaInsp", typeof(string));
            TblPasos.Columns.Add("Notas", typeof(string));
            TblPasos.Columns.Add("Otro", typeof(int));
            TblPasos.Columns.Add("Usu", typeof(string));
            TblPasos.Columns.Add("CodLicenciaTecP", typeof(string));
            TblPasos.Columns.Add("CodLicenciaInsP", typeof(string));
            TblPasos.Columns.Add("HHRealInsp", typeof(double));

            foreach (var Campos in TypPasos)
            {
                TblPasos.Rows.Add(new object[]
                {
                Campos.IDPasos,
                Campos.Paso,
                Campos.OT,
                Campos.Realizado,
                Campos.DescripcionRealizado,
                Campos.DocReferencia,
                Campos.Discrepancia,
                Campos.FechaI,
                Campos.FechaF,
                Campos.Estado,
                Campos.HHEst,
                Campos.HHReal,
                Campos.CodTecnico,
                Campos.LicenciaTec,
                Campos.CodInspector,
                Campos.LicenciaInsp,
                Campos.Notas,
                Campos.Otro,
                Campos.Usu,
                Campos.CodLicenciaTecP,
                Campos.CodLicenciaInsP,
                Campos.HHRealInsp,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = "";
                    PCodIdPaso=0;
                    string VBQuery = "INS_UPD_PasosOT";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurPasoOT", TblPasos);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured; 
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodIdPaso = Convert.ToInt32(HttpUtility.HtmlDecode(SDR["IdPaso"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "EDIT Pasos OT";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypPasosOT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        {
            return PMensj;
        }
        public int GetIdPaso()
        {
            return PCodIdPaso;
        }
    }
}