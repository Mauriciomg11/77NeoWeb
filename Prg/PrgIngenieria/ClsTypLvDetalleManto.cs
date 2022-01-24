using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class ClsTypLvDetalleManto
    {
        static public string PMensj;
        static public int PCodIdRte;
        public DateTime FechaProyectada { get; set; }
        public DateTime? FechaCumplimiento { get; set; }
        public DateTime FechaReporte { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public int CodIdLvDetManto { get; set; }
        public string CodLibroVuelo { get; set; }
        public int CodAeronave { get; set; }
        public string NumCasilla { get; set; }
        public string Reporte { get; set; }
        public string AccionCorrectiva { get; set; }
        public string CodTecnico { get; set; }
        public string CodClaseReporteManto { get; set; }
        public string CodClasifReporteManto { get; set; }
        public string CodCategoriaMel { get; set; }
        public string CodStatus { get; set; }
        public string DocumentoRef { get; set; }
        public string UbicacionTecnica { get; set; }
        public int BanderaOrdenTrabajo { get; set; }
        public string NroVuelo { get; set; }
        public string CodBase { get; set; }
        public string Usu { get; set; }
        public string ConsInterno { get; set; }
        public string Posicion { get; set; }
        public int Programado { get; set; }
        public int FallaConfirmada { get; set; }
        public string ReportadoPor { get; set; }
        public string AccionParcial { get; set; }
        public int CodOt { get; set; }
        public string CodUsuarioDiferido { get; set; }
        public int VerificadoRM { get; set; }
        public string CodInspectorVerifica { get; set; }
        public string NumLicenciaRM { get; set; }
        public int TearDown { get; set; }
        public int RII { get; set; }
        public int Notificado { get; set; }
        public string NumLicTecCierre { get; set; }
        public double TT_A_C { get; set; }
        public double HraProxCump { get; set; }
        public double Next_Due { get; set; }
        public string NumLicTecAbre { get; set; }
        public int? IdPosicionTT { get; set; }
        public string Accion { get; set; }
        //-------------  TblMROReporteOTPpal --------------------
        public int IDMroRepOT { get; set; }
        public string PasoOT { get; set; }
        public int NumReporte { get; set; }
        public string CodTaller { get; set; }
        public string ParteNumero { get; set; }
        public string SerieNumero { get; set; }
        public string ConsecutivoROTP { get; set; }
        public int SubOT { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypLvDetalleManto> LvDetalleManto, IEnumerable<ClsTypLvDetalleManto> MROReporteOTPpal)
        {
            DataTable TblLvDetalleManto = new DataTable();
            TblLvDetalleManto.Columns.Add("FechaProyectada", typeof(DateTime));
            TblLvDetalleManto.Columns.Add("FechaCumplimiento", typeof(DateTime));
            TblLvDetalleManto.Columns.Add("FechaReporte", typeof(DateTime));
            TblLvDetalleManto.Columns.Add("FechaInicio", typeof(DateTime));
            TblLvDetalleManto.Columns.Add("FechaVerificacion", typeof(DateTime));
            TblLvDetalleManto.Columns.Add("CodIdLvDetManto", typeof(int));
            TblLvDetalleManto.Columns.Add("CodLibroVuelo", typeof(string));
            TblLvDetalleManto.Columns.Add("CodAeronave", typeof(int));
            TblLvDetalleManto.Columns.Add("NumCasilla", typeof(string));
            TblLvDetalleManto.Columns.Add("Reporte", typeof(string));
            TblLvDetalleManto.Columns.Add("AccionCorrectiva", typeof(string));
            TblLvDetalleManto.Columns.Add("CodTecnico", typeof(string));
            TblLvDetalleManto.Columns.Add("CodClaseReporteManto", typeof(string));
            TblLvDetalleManto.Columns.Add("CodClasifReporteManto", typeof(string));
            TblLvDetalleManto.Columns.Add("CodCategoriaMel", typeof(string));
            TblLvDetalleManto.Columns.Add("CodStatus", typeof(string));
            TblLvDetalleManto.Columns.Add("DocumentoRef", typeof(string));
            TblLvDetalleManto.Columns.Add("UbicacionTecnica", typeof(string));
            TblLvDetalleManto.Columns.Add("BanderaOrdenTrabajo", typeof(int));
            TblLvDetalleManto.Columns.Add("NroVuelo", typeof(string));
            TblLvDetalleManto.Columns.Add("CodBase", typeof(string));
            TblLvDetalleManto.Columns.Add("Usu", typeof(string));
            TblLvDetalleManto.Columns.Add("ConsInterno", typeof(string));
            TblLvDetalleManto.Columns.Add("Posicion", typeof(string));
            TblLvDetalleManto.Columns.Add("Programado", typeof(int));
            TblLvDetalleManto.Columns.Add("FallaConfirmada", typeof(int));
            TblLvDetalleManto.Columns.Add("ReportadoPor", typeof(string));
            TblLvDetalleManto.Columns.Add("AccionParcial", typeof(string));
            TblLvDetalleManto.Columns.Add("CodOt", typeof(int));
            TblLvDetalleManto.Columns.Add("CodUsuarioDiferido", typeof(string));
            TblLvDetalleManto.Columns.Add("VerificadoRM", typeof(int));
            TblLvDetalleManto.Columns.Add("CodInspectorVerifica", typeof(string));
            TblLvDetalleManto.Columns.Add("NumLicenciaRM", typeof(string));
            TblLvDetalleManto.Columns.Add("TearDown", typeof(int));
            TblLvDetalleManto.Columns.Add("RII", typeof(int));
            TblLvDetalleManto.Columns.Add("Notificado", typeof(int));
            TblLvDetalleManto.Columns.Add("NumLicTecCierre", typeof(string));
            TblLvDetalleManto.Columns.Add("TT_A_C", typeof(double));
            TblLvDetalleManto.Columns.Add("HraProxCump", typeof(double));
            TblLvDetalleManto.Columns.Add("Next_Due", typeof(double));
            TblLvDetalleManto.Columns.Add("NumLicTecAbre", typeof(string));
            TblLvDetalleManto.Columns.Add("IdPosicionTT", typeof(int));
            TblLvDetalleManto.Columns.Add("Accion", typeof(string));

            foreach (var Campos in LvDetalleManto)
            {
                TblLvDetalleManto.Rows.Add(new object[]{
                    Campos.FechaProyectada,
                    Campos.FechaCumplimiento,
                    Campos.FechaReporte,
                    Campos.FechaInicio,
                    Campos.FechaVerificacion,
                    Campos.CodIdLvDetManto,
                    Campos.CodLibroVuelo,
                    Campos.CodAeronave,
                    Campos.NumCasilla,
                    Campos.Reporte,
                    Campos.AccionCorrectiva,
                    Campos.CodTecnico,
                    Campos.CodClaseReporteManto,
                    Campos.CodClasifReporteManto,
                    Campos.CodCategoriaMel,
                    Campos.CodStatus,
                    Campos.DocumentoRef,
                    Campos.UbicacionTecnica,
                    Campos.BanderaOrdenTrabajo,
                    Campos.NroVuelo,
                    Campos.CodBase,
                    Campos.Usu,
                    Campos.ConsInterno,
                    Campos.Posicion,
                    Campos.Programado,
                    Campos.FallaConfirmada,
                    Campos.ReportadoPor,
                    Campos.AccionParcial,
                    Campos.CodOt,
                    Campos.CodUsuarioDiferido,
                    Campos.VerificadoRM,
                    Campos.CodInspectorVerifica,
                    Campos.NumLicenciaRM,
                    Campos.TearDown,
                    Campos.RII,
                    Campos.Notificado,
                    Campos.NumLicTecCierre,
                    Campos.TT_A_C,
                    Campos.HraProxCump,
                    Campos.Next_Due,
                    Campos.NumLicTecAbre,
                    Campos.IdPosicionTT,
                    Campos.Accion,
                });
            }

            DataTable TblMROReporteOTPpal = new DataTable();

            TblMROReporteOTPpal.Columns.Add("IDMroRepOT", typeof(int));
            TblMROReporteOTPpal.Columns.Add("PasoOT", typeof(string));
            TblMROReporteOTPpal.Columns.Add("NumReporte", typeof(int));
            TblMROReporteOTPpal.Columns.Add("CodTaller", typeof(string));
            TblMROReporteOTPpal.Columns.Add("ParteNumero", typeof(string));
            TblMROReporteOTPpal.Columns.Add("SerieNumero", typeof(string));
            TblMROReporteOTPpal.Columns.Add("ConsecutivoROTP", typeof(string));
            TblMROReporteOTPpal.Columns.Add("SubOT", typeof(int));

            foreach (var CamposD in MROReporteOTPpal)
            {
                TblMROReporteOTPpal.Rows.Add(new object[] {
                    CamposD.IDMroRepOT,
                    CamposD.PasoOT,
                    CamposD.NumReporte,
                    CamposD.CodTaller,
                    CamposD.ParteNumero,
                    CamposD.SerieNumero,
                    CamposD.ConsecutivoROTP,
                    CamposD.SubOT,
                });
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "SP_InsUpd_LvDetalleManto";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurLvDetManto", TblLvDetalleManto);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@CurOTRteMRO", TblMROReporteOTPpal);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@ICC", HttpContext.Current.Session["!dC!@"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            // SC.ExecuteNonQuery();
                            //PMensj = (string)SC.ExecuteScalar();
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodIdRte = Convert.ToInt32(SDR["CodIdRte"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmLibroVuelo";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypLvDetalleManto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public int GetCodIdRte()
        {
            return PCodIdRte;
        }
    }
}