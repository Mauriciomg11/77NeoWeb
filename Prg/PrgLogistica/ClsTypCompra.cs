using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypCompra
    {

        ClsConexion Cnx = new ClsConexion();
        static public string PMensj, PCodCompra, VbAccion;
        public string CodOrdenCompra { get; set; }
        public string CodProveedor { get; set; }
        public string CodEmpleado { get; set; }
        public string CodAutorizador { get; set; }
        public string CodMoneda { get; set; }
        public string TipoOrdenCompra { get; set; }
        public string CodTransportador { get; set; }
        public string CodTipoPago { get; set; }
        public string CodUbicaCia { get; set; }
        public DateTime? FechaOC { get; set; }
        public string CodEstadoCompra { get; set; }
        public double Monto { get; set; }
        public double TasaIva { get; set; }
        public double ValorIVA { get; set; }
        public double TasaRetencion { get; set; }
        public double ValorRetencion { get; set; }
        public double TasaIca { get; set; }
        public double ValorICA { get; set; }
        public double ValorOtrosImp { get; set; }
        public double TasaDescuento { get; set; }
        public double ValorDescuento { get; set; }
        public double ValorTotal { get; set; }
        public string Referencia { get; set; }
        public string Observacion { get; set; }
        public int Aprobado { get; set; }
        public int Asentado { get; set; }
        public int Recibido { get; set; }
        public string CuentaPuc { get; set; }
        public string CodTipoCodigo { get; set; }
        public string CodIdTipoUbicaFac { get; set; }
        public string NumFacturaOC { get; set; }
        public string CompraIntercambio { get; set; }
        //-------------  TblDetCompra --------------------
        public int IdDetOrdenCompra { get; set; }
        public DateTime? FechaRecibo { get; set; }
        public int Posicion { get; set; }
        public string PN { get; set; }
        public int IdDetCotiza { get; set; }
        public int ShippingOrder { get; set; }
        public int ElementoRecibido { get; set; }
        public int FacturaProveedor { get; set; }
        public int Anticipo { get; set; }
        public double Cant { get; set; }
        public double VlrUnd { get; set; }
        public double TasaIVA { get; set; }
        public string AccionDet { get; set; }

        public void Accion(string Accion)
        { VbAccion = Accion; }
        public void Alimentar(IEnumerable<ClsTypCompra> TypEncCompra, IEnumerable<ClsTypCompra> TypDetCompra)
        {
            DataTable TblEncCompra = new DataTable();
            TblEncCompra.Columns.Add("CodOrdenCompra", typeof(string));
            TblEncCompra.Columns.Add("CodProveedor", typeof(string));
            TblEncCompra.Columns.Add("CodEmpleado", typeof(string));
            TblEncCompra.Columns.Add("CodAutorizador", typeof(string));
            TblEncCompra.Columns.Add("CodMoneda", typeof(string));
            TblEncCompra.Columns.Add("TipoOrdenCompra", typeof(string));
            TblEncCompra.Columns.Add("CodTransportador", typeof(string));
            TblEncCompra.Columns.Add("CodTipoPago", typeof(string));
            TblEncCompra.Columns.Add("CodUbicaCia", typeof(string));
            TblEncCompra.Columns.Add("FechaOC", typeof(DateTime));
            TblEncCompra.Columns.Add("CodEstadoCompra", typeof(string));
            TblEncCompra.Columns.Add("Monto", typeof(double));
            TblEncCompra.Columns.Add("TasaIva", typeof(double));
            TblEncCompra.Columns.Add("ValorIVA", typeof(double));
            TblEncCompra.Columns.Add("TasaRetencion", typeof(double));
            TblEncCompra.Columns.Add("ValorRetencion", typeof(double));
            TblEncCompra.Columns.Add("TasaIca", typeof(double));
            TblEncCompra.Columns.Add("ValorICA", typeof(double));
            TblEncCompra.Columns.Add("ValorOtrosImp", typeof(double));
            TblEncCompra.Columns.Add("TasaDescuento", typeof(double));
            TblEncCompra.Columns.Add("ValorDescuento", typeof(double));
            TblEncCompra.Columns.Add("ValorTotal", typeof(double));
            TblEncCompra.Columns.Add("Referencia", typeof(string));
            TblEncCompra.Columns.Add("Observacion", typeof(string));
            TblEncCompra.Columns.Add("Aprobado", typeof(int));
            TblEncCompra.Columns.Add("Asentado", typeof(int));
            TblEncCompra.Columns.Add("Recibido", typeof(int));
            TblEncCompra.Columns.Add("CuentaPuc", typeof(string));
            TblEncCompra.Columns.Add("CodTipoCodigo", typeof(string));
            TblEncCompra.Columns.Add("CodIdTipoUbicaFac", typeof(string));
            TblEncCompra.Columns.Add("NumFacturaOC", typeof(string));
            TblEncCompra.Columns.Add("CompraIntercambio", typeof(string));

            foreach (var Campo in TypEncCompra)
            {
                TblEncCompra.Rows.Add(new object[]{
                Campo.CodOrdenCompra,
                Campo.CodProveedor,
                Campo.CodEmpleado,
                Campo.CodAutorizador,
                Campo.CodMoneda,
                Campo.TipoOrdenCompra,
                Campo.CodTransportador,
                Campo.CodTipoPago,
                Campo.CodUbicaCia,
                Campo.FechaOC,
                Campo.CodEstadoCompra,
                Campo.Monto,
                Campo.TasaIva,
                Campo.ValorIVA,
                Campo.TasaRetencion,
                Campo.ValorRetencion,
                Campo.TasaIca,
                Campo.ValorICA,
                Campo.ValorOtrosImp,
                Campo.TasaDescuento,
                Campo.ValorDescuento,
                Campo.ValorTotal,
                Campo.Referencia,
                Campo.Observacion,
                Campo.Aprobado,
                Campo.Asentado,
                Campo.Recibido,
                Campo.CuentaPuc,
                Campo.CodTipoCodigo,
                Campo.CodIdTipoUbicaFac,
                Campo.NumFacturaOC,
                Campo.CompraIntercambio,
                 });
            }

            DataTable TblDetCompra = new DataTable();
            TblDetCompra.Columns.Add("IdDetOrdenCompra", typeof(int));
            TblDetCompra.Columns.Add("FechaRecibo", typeof(DateTime));
            TblDetCompra.Columns.Add("Posicion", typeof(int));
            TblDetCompra.Columns.Add("PN", typeof(string));
            TblDetCompra.Columns.Add("IdDetCotiza", typeof(int));
            TblDetCompra.Columns.Add("ShippingOrder", typeof(int));
            TblDetCompra.Columns.Add("ElementoRecibido", typeof(int));
            TblDetCompra.Columns.Add("FacturaProveedor", typeof(int));
            TblDetCompra.Columns.Add("Anticipo", typeof(int));
            TblDetCompra.Columns.Add("Cant", typeof(double));
            TblDetCompra.Columns.Add("VlrUnd", typeof(double));
            TblDetCompra.Columns.Add("TasaIVA", typeof(double));
            TblDetCompra.Columns.Add("AccionDet", typeof(string));

            foreach (var Campo in TypDetCompra)
            {
                TblDetCompra.Rows.Add(new object[]{
                    Campo.IdDetOrdenCompra,
                    Campo.FechaRecibo,
                    Campo.Posicion,
                    Campo.PN,
                    Campo.IdDetCotiza,
                    Campo.ShippingOrder,
                    Campo.ElementoRecibido,
                    Campo.FacturaProveedor,
                    Campo.Anticipo,
                    Campo.Cant,
                    Campo.VlrUnd,
                    Campo.TasaIVA,
                    Campo.AccionDet,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_Compra";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PCodCompra = "";
                            PMensj = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@EncCom", TblEncCompra);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetCom", TblDetCompra);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                            SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodCompra = HttpUtility.HtmlDecode(SDR["CodCompra"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "Generar Cotizacion";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypCotizacion", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        { return PMensj; }

        public string GetCodCompra()
        { return PCodCompra; }

    }
}
