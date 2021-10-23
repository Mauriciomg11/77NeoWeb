using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypCotizacion
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj, PId, PCodCotiza, VbAccion, VbPN;
        public int IdCotizacion { get; set; }
        public string CodCotizacion { get; set; }
        public string CodTipoCotizacion { get; set; }
        public string CodProveedor { get; set; }
        public DateTime? FechaSolicitudPet { get; set; }
        public DateTime? FechaMaxRespuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public DateTime? FechaVigenciaCot { get; set; }
        public string CodTipoPeticion { get; set; }
        public double ValorTotalCot { get; set; }
        public string CodMoneda { get; set; }
        public double Monto { get; set; }
        public double ValorBruto { get; set; }
        public string DiaTasa { get; set; }
        public string MesTasa { get; set; }
        public string AñoTasa { get; set; }
        public double TrmAcordado { get; set; }
        public double TrmAcordado_Ant { get; set; }
        public string CodTipoPago { get; set; }
        public double ValorIva { get; set; }
        public double TasaIva { get; set; }
        public double ValorIca { get; set; }
        public double TasaIca { get; set; }
        public double ValorRetencion { get; set; }
        public double TasaRetencion { get; set; }
        public double ValorOtrosImpuestos { get; set; }
        public string CodEstadoCot { get; set; }
        public int Aprobado { get; set; }
        public double ValorDescuento { get; set; }
        public double TasaDescuento { get; set; }
        public string Contacto { get; set; }
        public string LugarEntrega { get; set; }
        public string CodCondicionElem { get; set; }
        public string Observacion { get; set; }
        public string TipoCotiza { get; set; }
        public string CodMedioCotizacion { get; set; }
        public string CodTipoCodigo { get; set; }
        public int PeticionEC { get; set; }
        public int IdConfigCia { get; set; }
        public DateTime? FechaTRM { get; set; }        
        public DateTime? FechaTRM_Ant { get; set; }        
        public string CodProveedor_ANT { get; set; }
        public string CodTipoCotizacion_ANT { get; set; }

        //-------------  TblDetCotiza --------------------
        public int IdDetCotizacion { get; set; }
        public int IdDetPedido { get; set; }
        public int PosDC { get; set; }
        public string Pn { get; set; }
        public double ValorIVA { get; set; }
        public double TasaIVA { get; set; }
        public double ValorTotal { get; set; }
        public double Cantidad { get; set; }
        public string CodUndMed { get; set; }
        public double ValorUnidad { get; set; }
        public int Aprobacion { get; set; }
        public string CodMedioCotiza { get; set; }
        public string CodDetEstadoCotiza { get; set; }
        public int TiempoEntrega { get; set; }
        public string CodEstdo { get; set; }
        public double UndMinimaCompra { get; set; }
        public string Alterno { get; set; }
        public string ObservacionesDC { get; set; }
        public int TiempEntregaPropuesta { get; set; }
        public double PorcAlMonto { get; set; }
        public double PorcAlimpuesto { get; set; }
        public double ValorUnidadP { get; set; }
        public double ValorUnidadPExp { get; set; }
        public int GarantiaDC { get; set; }
        public int CodAeronaveCT { get; set; }
        public string SN { get; set; }
        public int IdDetPedido_Ant { get; set; }
        public string Pn_Ant { get; set; }
        public double Cantidad_Ant { get; set; }
        public string CodUndMed_Ant { get; set; }
        public double ValorUnidad_Ant { get; set; }
        public double TasaIVA_Ant { get; set; }
        public string AccionDet { get; set; }

        public void Accion(string Accion)
        { VbAccion = Accion; }
        public void Alimentar(IEnumerable<ClsTypCotizacion> TypEncCotiza, IEnumerable<ClsTypCotizacion> TypDetCotiza)
        {
            DataTable TblEncCotiza = new DataTable();
            TblEncCotiza.Columns.Add("IdCotizacion", typeof(int));
            TblEncCotiza.Columns.Add("CodCotizacion", typeof(string));
            TblEncCotiza.Columns.Add("CodTipoCotizacion", typeof(string));
            TblEncCotiza.Columns.Add("CodProveedor", typeof(string));
            TblEncCotiza.Columns.Add("FechaSolicitudPet", typeof(DateTime));
            TblEncCotiza.Columns.Add("FechaMaxRespuesta", typeof(DateTime));
            TblEncCotiza.Columns.Add("FechaRespuesta", typeof(DateTime));
            TblEncCotiza.Columns.Add("FechaVigenciaCot", typeof(DateTime));
            TblEncCotiza.Columns.Add("CodTipoPeticion", typeof(string));
            TblEncCotiza.Columns.Add("ValorTotalCot", typeof(double));
            TblEncCotiza.Columns.Add("CodMoneda", typeof(string));
            TblEncCotiza.Columns.Add("Monto", typeof(double));
            TblEncCotiza.Columns.Add("ValorBruto", typeof(double));
            TblEncCotiza.Columns.Add("DiaTasa", typeof(string));
            TblEncCotiza.Columns.Add("MesTasa", typeof(string));
            TblEncCotiza.Columns.Add("AñoTasa", typeof(string));
            TblEncCotiza.Columns.Add("TrmAcordado", typeof(double));
            TblEncCotiza.Columns.Add("TrmAcordado_Ant", typeof(double));
            TblEncCotiza.Columns.Add("CodTipoPago", typeof(string));
            TblEncCotiza.Columns.Add("ValorIva", typeof(double));
            TblEncCotiza.Columns.Add("TasaIva", typeof(double));
            TblEncCotiza.Columns.Add("ValorIca", typeof(double));
            TblEncCotiza.Columns.Add("TasaIca", typeof(double));
            TblEncCotiza.Columns.Add("ValorRetencion", typeof(double));
            TblEncCotiza.Columns.Add("TasaRetencion", typeof(double));
            TblEncCotiza.Columns.Add("ValorOtrosImpuestos", typeof(double));
            TblEncCotiza.Columns.Add("CodEstadoCot", typeof(string));
            TblEncCotiza.Columns.Add("Aprobado", typeof(int));
            TblEncCotiza.Columns.Add("ValorDescuento", typeof(double));
            TblEncCotiza.Columns.Add("TasaDescuento", typeof(double));
            TblEncCotiza.Columns.Add("Contacto", typeof(string));
            TblEncCotiza.Columns.Add("LugarEntrega", typeof(string));
            TblEncCotiza.Columns.Add("CodCondicionElem", typeof(string));
            TblEncCotiza.Columns.Add("Observacion", typeof(string));
            TblEncCotiza.Columns.Add("TipoCotiza", typeof(string));
            TblEncCotiza.Columns.Add("CodMedioCotizacion", typeof(string));
            TblEncCotiza.Columns.Add("CodTipoCodigo", typeof(string));
            TblEncCotiza.Columns.Add("PeticionEC", typeof(int));
            TblEncCotiza.Columns.Add("IdConfigCia", typeof(int));
            TblEncCotiza.Columns.Add("FechaTRM", typeof(DateTime));
            TblEncCotiza.Columns.Add("FechaTRM_Ant", typeof(DateTime));
            TblEncCotiza.Columns.Add("CodProveedor_ANT", typeof(string));
            TblEncCotiza.Columns.Add("CodTipoCotizacion_ANT", typeof(string));

            foreach (var Campo in TypEncCotiza)
            {
                TblEncCotiza.Rows.Add(new object[]{
                    Campo.IdCotizacion,
                    Campo.CodCotizacion,
                    Campo.CodTipoCotizacion,
                    Campo.CodProveedor,
                    Campo.FechaSolicitudPet,
                    Campo.FechaMaxRespuesta,
                    Campo.FechaRespuesta,
                    Campo.FechaVigenciaCot,
                    Campo.CodTipoPeticion,
                    Campo.ValorTotalCot,
                    Campo.CodMoneda,
                    Campo.Monto,
                    Campo.ValorBruto,
                    Campo.DiaTasa,
                    Campo.MesTasa,
                    Campo.AñoTasa,
                    Campo.TrmAcordado,
                    Campo.TrmAcordado_Ant,
                    Campo.CodTipoPago,
                    Campo.ValorIva,
                    Campo.TasaIva,
                    Campo.ValorIca,
                    Campo.TasaIca,
                    Campo.ValorRetencion,
                    Campo.TasaRetencion,
                    Campo.ValorOtrosImpuestos,
                    Campo.CodEstadoCot,
                    Campo.Aprobado,
                    Campo.ValorDescuento,
                    Campo.TasaDescuento,
                    Campo.Contacto,
                    Campo.LugarEntrega,
                    Campo.CodCondicionElem,
                    Campo.Observacion,
                    Campo.TipoCotiza,
                    Campo.CodMedioCotizacion,
                    Campo.CodTipoCodigo,
                    Campo.PeticionEC,
                    Campo.IdConfigCia,
                    Campo.FechaTRM,
                    Campo.FechaTRM_Ant,
                    Campo.CodProveedor_ANT,
                    Campo.CodTipoCotizacion_ANT,
                 });
            }

            DataTable TblDetCotiza = new DataTable();
            TblDetCotiza.Columns.Add("IdDetCotizacion", typeof(int));
            TblDetCotiza.Columns.Add("IdCotizacion", typeof(int));
            TblDetCotiza.Columns.Add("IdDetPedido", typeof(int));
            TblDetCotiza.Columns.Add("PosDC", typeof(int));
            TblDetCotiza.Columns.Add("Pn", typeof(string));
            TblDetCotiza.Columns.Add("Monto", typeof(double));
            TblDetCotiza.Columns.Add("ValorIVA", typeof(double));
            TblDetCotiza.Columns.Add("TasaIVA", typeof(double));
            TblDetCotiza.Columns.Add("ValorTotal", typeof(double));
            TblDetCotiza.Columns.Add("Cantidad", typeof(double));
            TblDetCotiza.Columns.Add("CodUndMed", typeof(string));
            TblDetCotiza.Columns.Add("ValorUnidad", typeof(double));
            TblDetCotiza.Columns.Add("Aprobacion", typeof(int));
            TblDetCotiza.Columns.Add("CodMedioCotiza", typeof(string));
            TblDetCotiza.Columns.Add("CodDetEstadoCotiza", typeof(string));
            TblDetCotiza.Columns.Add("TiempoEntrega", typeof(int));
            TblDetCotiza.Columns.Add("CodEstdo", typeof(string));
            TblDetCotiza.Columns.Add("UndMinimaCompra", typeof(double));
            TblDetCotiza.Columns.Add("Alterno", typeof(string));
            TblDetCotiza.Columns.Add("ObservacionesDC", typeof(string));
            TblDetCotiza.Columns.Add("TiempEntregaPropuesta", typeof(int));
            TblDetCotiza.Columns.Add("PorcAlMonto", typeof(double));
            TblDetCotiza.Columns.Add("PorcAlimpuesto", typeof(double));
            TblDetCotiza.Columns.Add("ValorUnidadP", typeof(double));
            TblDetCotiza.Columns.Add("ValorUnidadPExp", typeof(double));
            TblDetCotiza.Columns.Add("GarantiaDC", typeof(int));
            TblDetCotiza.Columns.Add("CodAeronaveCT", typeof(int));
            TblDetCotiza.Columns.Add("SN", typeof(string));
            TblDetCotiza.Columns.Add("IdConfigCia", typeof(int));
            TblDetCotiza.Columns.Add("IdDetPedido_Ant", typeof(int));
            TblDetCotiza.Columns.Add("Pn_Ant", typeof(string));
            TblDetCotiza.Columns.Add("Cantidad_Ant", typeof(double));
            TblDetCotiza.Columns.Add("CodUndMed_Ant", typeof(string));
            TblDetCotiza.Columns.Add("ValorUnidad_Ant", typeof(double));
            TblDetCotiza.Columns.Add("TasaIVA_Ant", typeof(double));
            TblDetCotiza.Columns.Add("AccionDet", typeof(string));
            foreach (var Campo in TypDetCotiza)
            {
                TblDetCotiza.Rows.Add(new object[]{
                    Campo.IdDetCotizacion,
                    Campo.IdCotizacion,
                    Campo.IdDetPedido,
                    Campo.PosDC,
                    Campo.Pn,
                    Campo.Monto,
                    Campo.ValorIVA,
                    Campo.TasaIVA,
                    Campo.ValorTotal,
                    Campo.Cantidad,
                    Campo.CodUndMed,
                    Campo.ValorUnidad,
                    Campo.Aprobacion,
                    Campo.CodMedioCotiza,
                    Campo.CodDetEstadoCotiza,
                    Campo.TiempoEntrega,
                    Campo.CodEstdo,
                    Campo.UndMinimaCompra,
                    Campo.Alterno,
                    Campo.ObservacionesDC,
                    Campo.TiempEntregaPropuesta,
                    Campo.PorcAlMonto,
                    Campo.PorcAlimpuesto,
                    Campo.ValorUnidadP,
                    Campo.ValorUnidadPExp,
                    Campo.GarantiaDC,
                    Campo.CodAeronaveCT,
                    Campo.SN,
                    Campo.IdConfigCia,
                    Campo.IdDetPedido_Ant,
                    Campo.Pn_Ant,
                    Campo.Cantidad_Ant,
                    Campo.CodUndMed_Ant,
                    Campo.ValorUnidad_Ant,
                    Campo.TasaIVA_Ant,
                    Campo.AccionDet,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_Cotiza";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PCodCotiza = "";
                            PMensj = "";
                            VbPN = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@EncCot", TblEncCotiza);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetCot", TblDetCotiza);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                            SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PId = SDR["IdCotiza"].ToString().Trim();
                                PCodCotiza = HttpUtility.HtmlDecode(SDR["CodCotiza"].ToString().Trim());
                                VbPN = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "GenerarSolicitud";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypSolicitudPedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        { return PMensj; }
        public string GetIdCotiza()
        { return PId; }
        public string GetCodCotiza()
        { return PCodCotiza; }
        public string GetPN()
        { return VbPN; }
    }
}