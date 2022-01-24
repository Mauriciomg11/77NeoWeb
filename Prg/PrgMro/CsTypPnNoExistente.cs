using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.prgMro
{
    public class CsTypPnNoExistente
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PN;
        public int IdPnNoExistente { get; set; }
        public string PnNoExistente { get; set; }
        public string PnNuevo { get; set; }
        public string Descripcion { get; set; }
        public double CantSolicitada { get; set; }
        public int CodAeronave { get; set; }
        public int Reporte { get; set; }
        public int CodOrdenTrabajo { get; set; }
        public int CodIdDetalleRes { get; set; }
        public string Matricula { get; set; }
        public string Usu { get; set; }
        public int IdDetPedido { get; set; }
        public int IdPropuesta { get; set; }
        public int IdDetPropuesta { get; set; }
        public int IdDetPropHk { get; set; }
        public int CodIdDetElemPlanInstrumento { get; set; }
        public int IdSrvc { get; set; }
        public string CodPedido { get; set; }
        public int IdConfigCia { get; set; }

        public void Alimentar(IEnumerable<CsTypPnNoExistente> TypPnNoExistente)
        {
            DataTable TblPnNoExistente = new DataTable();
            TblPnNoExistente.Columns.Add("IdPnNoExistente", typeof(int));
            TblPnNoExistente.Columns.Add("PnNoExistente", typeof(string));
            TblPnNoExistente.Columns.Add("PnNuevo", typeof(string));
            TblPnNoExistente.Columns.Add("Descripcion", typeof(string));
            TblPnNoExistente.Columns.Add("CantSolicitada", typeof(double));
            TblPnNoExistente.Columns.Add("CodAeronave", typeof(int));
            TblPnNoExistente.Columns.Add("Reporte", typeof(int));
            TblPnNoExistente.Columns.Add("CodOrdenTrabajo", typeof(int));
            TblPnNoExistente.Columns.Add("CodIdDetalleRes", typeof(int));
            TblPnNoExistente.Columns.Add("Matricula", typeof(string));
            TblPnNoExistente.Columns.Add("Usu", typeof(string));
            TblPnNoExistente.Columns.Add("IdDetPedido", typeof(int));
            TblPnNoExistente.Columns.Add("IdPropuesta", typeof(int));
            TblPnNoExistente.Columns.Add("IdDetPropuesta", typeof(int));
            TblPnNoExistente.Columns.Add("IdDetPropHk", typeof(int));
            TblPnNoExistente.Columns.Add("CodIdDetElemPlanInstrumento", typeof(int));
            TblPnNoExistente.Columns.Add("IdSrvc", typeof(int));
            TblPnNoExistente.Columns.Add("CodPedido", typeof(string));
            TblPnNoExistente.Columns.Add("IdConfigCia", typeof(int));

            foreach (var Campo in TypPnNoExistente)
            {
                TblPnNoExistente.Rows.Add(new object[]
                {
                    Campo.IdPnNoExistente,
                    Campo.PnNoExistente,
                    Campo.PnNuevo,
                    Campo.Descripcion,
                    Campo.CantSolicitada,
                    Campo.CodAeronave,
                    Campo.Reporte,
                    Campo.CodOrdenTrabajo,
                    Campo.CodIdDetalleRes,
                    Campo.Matricula,
                    Campo.Usu,
                    Campo.IdDetPedido,
                    Campo.IdPropuesta,
                    Campo.IdDetPropuesta,
                    Campo.IdDetPropHk,
                    Campo.CodIdDetElemPlanInstrumento,
                    Campo.IdSrvc,
                    Campo.CodPedido,
                    Campo.IdConfigCia,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = ""; PN = "";
                    string VBQuery = "UPDATE_PnNoExistente";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurVD", TblPnNoExistente);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PN = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmAlertaPNNuevos";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypPnNoExistente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj() { return PMensj; }
        public string GetPN() { return PN; }
    }
}