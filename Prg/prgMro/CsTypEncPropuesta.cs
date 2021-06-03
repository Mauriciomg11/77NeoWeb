using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.prgMro
{
    public class CsTypEncPropuesta
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PId;
        public int IdPropuesta { get; set; }
        public string CodTipoPropuesta { get; set; }
        public string NumContrato { get; set; }
        public string DocReferencia { get; set; }
        public string ObservacionRef { get; set; }
        public string CodTipoPago { get; set; }
        public int Garantia { get; set; }
        public string TiempoEntrega { get; set; }
        public string DanoOculto { get; set; }
        public string CodCliente { get; set; }
        public DateTime? FechaPropuesta { get; set; }
        public string CodTipoMoneda { get; set; }
        public DateTime? TRM { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaValidez { get; set; }
        public string CodEstadoPropuesta { get; set; }
        public string IdTipoSolicitudPropuesta { get; set; }
        public double ValorBruto { get; set; }
        public double ValorNeto { get; set; }
        public double ValorImpuesto { get; set; }
        public string Usu { get; set; }
        public string Formadepagor { get; set; }
        public string danoocultor { get; set; }
        public string garantiar { get; set; }
        public int DanoOc { get; set; }
        public double Impuesto { get; set; }
        public double VlorTotalHHEP { get; set; }
        public double VlrRepuestoEP { get; set; }
        public DateTime FechaEntregaTrabajo { get; set; }
        public double ValorTRM { get; set; }
        public double GananciaNAL { get; set; }
        public double GananciaInta { get; set; }
        public int AplicaIVA { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string CodBanco { get; set; }
        public string NroDeCta { get; set; }
        public int TipoCuenta { get; set; }
        public int EvaluarDesdeOT { get; set; }
        public int IntegradorNeoS { get; set; }
        public double Miscelaneos { get; set; }
        public double AvancePPT { get; set; }
        public int IdConfigCia { get; set; }
        public string ClienteAnt { get; set; }/**/
        public string Accion { get; set; }
        public void Alimentar(IEnumerable<CsTypEncPropuesta> TypEncPropuesta)
        {
            DataTable TblEncPropuesta = new DataTable();
            TblEncPropuesta.Columns.Add("IdPropuesta", typeof(int));
            TblEncPropuesta.Columns.Add("CodTipoPropuesta", typeof(string));
            TblEncPropuesta.Columns.Add("NumContrato", typeof(string));
            TblEncPropuesta.Columns.Add("DocReferencia", typeof(string));
            TblEncPropuesta.Columns.Add("ObservacionRef", typeof(string));
            TblEncPropuesta.Columns.Add("CodTipoPago", typeof(string));
            TblEncPropuesta.Columns.Add("Garantia", typeof(int));
            TblEncPropuesta.Columns.Add("TiempoEntrega", typeof(string));
            TblEncPropuesta.Columns.Add("DanoOculto", typeof(string));
            TblEncPropuesta.Columns.Add("CodCliente", typeof(string));
            TblEncPropuesta.Columns.Add("FechaPropuesta", typeof(DateTime));
            TblEncPropuesta.Columns.Add("CodTipoMoneda", typeof(string));
            TblEncPropuesta.Columns.Add("TRM", typeof(DateTime));
            TblEncPropuesta.Columns.Add("FechaEntrega", typeof(DateTime));
            TblEncPropuesta.Columns.Add("FechaValidez", typeof(DateTime));
            TblEncPropuesta.Columns.Add("CodEstadoPropuesta", typeof(string));
            TblEncPropuesta.Columns.Add("IdTipoSolicitudPropuesta", typeof(string));
            TblEncPropuesta.Columns.Add("ValorBruto", typeof(double));
            TblEncPropuesta.Columns.Add("ValorNeto", typeof(double));
            TblEncPropuesta.Columns.Add("ValorImpuesto", typeof(double));
            TblEncPropuesta.Columns.Add("Usu", typeof(string));
            TblEncPropuesta.Columns.Add("Formadepagor", typeof(string));
            TblEncPropuesta.Columns.Add("danoocultor", typeof(string));
            TblEncPropuesta.Columns.Add("garantiar", typeof(string));
            TblEncPropuesta.Columns.Add("DanoOc", typeof(int));
            TblEncPropuesta.Columns.Add("Impuesto", typeof(double));
            TblEncPropuesta.Columns.Add("VlorTotalHHEP", typeof(double));
            TblEncPropuesta.Columns.Add("VlrRepuestoEP", typeof(double));
            TblEncPropuesta.Columns.Add("FechaEntregaTrabajo", typeof(DateTime));
            TblEncPropuesta.Columns.Add("ValorTRM", typeof(double));
            TblEncPropuesta.Columns.Add("GananciaNAL", typeof(double));
            TblEncPropuesta.Columns.Add("GananciaInta", typeof(double));
            TblEncPropuesta.Columns.Add("AplicaIVA", typeof(int));
            TblEncPropuesta.Columns.Add("FechaAprobacion", typeof(DateTime));
            TblEncPropuesta.Columns.Add("CodBanco", typeof(string));
            TblEncPropuesta.Columns.Add("NroDeCta", typeof(string));
            TblEncPropuesta.Columns.Add("TipoCuenta", typeof(int));
            TblEncPropuesta.Columns.Add("EvaluarDesdeOT", typeof(int));
            TblEncPropuesta.Columns.Add("IntegradorNeoS", typeof(int));
            TblEncPropuesta.Columns.Add("Miscelaneos", typeof(double));
            TblEncPropuesta.Columns.Add("AvancePPT", typeof(double));
            TblEncPropuesta.Columns.Add("IdConfigCia", typeof(int));
            TblEncPropuesta.Columns.Add("ClienteAnt", typeof(string));/**/
            TblEncPropuesta.Columns.Add("Accion", typeof(string));
            foreach (var Campo in TypEncPropuesta)
            {
                TblEncPropuesta.Rows.Add(new object[]
                {
                   Campo.IdPropuesta,
                    Campo.CodTipoPropuesta,
                    Campo.NumContrato,
                    Campo.DocReferencia,
                    Campo.ObservacionRef,
                    Campo.CodTipoPago,
                    Campo.Garantia,
                    Campo.TiempoEntrega,
                    Campo.DanoOculto,
                    Campo.CodCliente,
                    Campo.FechaPropuesta,
                    Campo.CodTipoMoneda,
                    Campo.TRM,
                    Campo.FechaEntrega,
                    Campo.FechaValidez,
                    Campo.CodEstadoPropuesta,
                    Campo.IdTipoSolicitudPropuesta,
                    Campo.ValorBruto,
                    Campo.ValorNeto,
                    Campo.ValorImpuesto,
                    Campo.Usu,
                    Campo.Formadepagor,
                    Campo.danoocultor,
                    Campo.garantiar,
                    Campo.DanoOc,
                    Campo.Impuesto,
                    Campo.VlorTotalHHEP,
                    Campo.VlrRepuestoEP,
                    Campo.FechaEntregaTrabajo,
                    Campo.ValorTRM,
                    Campo.GananciaNAL,
                    Campo.GananciaInta,
                    Campo.AplicaIVA,
                    Campo.FechaAprobacion,
                    Campo.CodBanco,
                    Campo.NroDeCta,
                    Campo.TipoCuenta,
                    Campo.EvaluarDesdeOT,
                    Campo.IntegradorNeoS,
                    Campo.Miscelaneos,
                    Campo.AvancePPT,
                    Campo.IdConfigCia,
                    Campo.ClienteAnt,/**/
                    Campo.Accion,

                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = ""; PId = "";
                    string VBQuery = "INSERT_UPDATE_EncPropuesta";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurEncPropuesta", TblEncPropuesta);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PId = HttpUtility.HtmlDecode(SDR["IdNew"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmPropuesta";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypEncPropuesta", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj() { return PMensj; }
        public string GetCodPPT() { return PId; }
    }
}