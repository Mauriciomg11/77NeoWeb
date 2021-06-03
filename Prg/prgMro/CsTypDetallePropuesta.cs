using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.prgMro
{
    public class CsTypDetallePropuesta
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PId;
        static public double VbGananciaNalAnt;
        static public double VbGananciaIntaAnt;
        public int IdDetPropuesta { get; set; }
        public int IdPropuesta { get; set; }
        public string PN { get; set; }
        public string Descripcion { get; set; }
        public double CantidadSol { get; set; }
        public double PorcentajeUtilidad { get; set; }
        public double ValorUnd { get; set; }
        public double CostoVenta { get; set; }
        public int TiempoEntregaDias { get; set; }
        public int TiempoEntregaDiasCoti { get; set; }
        public int IdServicio { get; set; }
        public int SelectComprar { get; set; }
        public int Posicion { get; set; }
        public int Aprobado { get; set; }
        public string NomServicio { get; set; }
        public string Usu { get; set; }
        public int IdReporte { get; set; }
        public string EstadoPosicion { get; set; }
        public int CantidadEntregada { get; set; }
        public string UnidadMedida { get; set; }
        public string CodMoneda { get; set; }
        public double ValorMonedaProp { get; set; }
        public double IVA { get; set; }
        public double ValorTotal { get; set; }
        public double ValorConImpuesto { get; set; }
        public double UnidMinCompra { get; set; }
        public string CodEstado { get; set; }
        public string ObservacionesDP { get; set; }
        public string PnAlterno { get; set; }
        public string TipoCotizacion { get; set; }
        public int IdDetPropSrv { get; set; }
        public int RepaExterna { get; set; }
        public double CantRealDP { get; set; }
        public string UndCompraDPV { get; set; }
        public int IdConfigCia { get; set; }
        public string CodTipoPT { get; set; }
        public string Accion { get; set; }
        public void GananciaAnterior(double GanNal, double GanInta)
        {
            VbGananciaNalAnt = GanNal;
            VbGananciaIntaAnt = GanInta;
        }
        public void Alimentar(IEnumerable<CsTypDetallePropuesta> TypDetallePropuesta)
        {
            DataTable TblDetallePropuesta = new DataTable();
            TblDetallePropuesta.Columns.Add("IdDetPropuesta", typeof(int));
            TblDetallePropuesta.Columns.Add("IdPropuesta", typeof(int));
            TblDetallePropuesta.Columns.Add("PN", typeof(string));
            TblDetallePropuesta.Columns.Add("Descripcion", typeof(string));
            TblDetallePropuesta.Columns.Add("CantidadSol", typeof(double));
            TblDetallePropuesta.Columns.Add("PorcentajeUtilidad", typeof(double));
            TblDetallePropuesta.Columns.Add("ValorUnd", typeof(double));
            TblDetallePropuesta.Columns.Add("CostoVenta", typeof(double));
            TblDetallePropuesta.Columns.Add("TiempoEntregaDias", typeof(int));
            TblDetallePropuesta.Columns.Add("TiempoEntregaDiasCoti", typeof(int));
            TblDetallePropuesta.Columns.Add("IdServicio", typeof(int));
            TblDetallePropuesta.Columns.Add("SelectComprar", typeof(int));
            TblDetallePropuesta.Columns.Add("Posicion", typeof(int));
            TblDetallePropuesta.Columns.Add("Aprobado", typeof(int));
            TblDetallePropuesta.Columns.Add("NomServicio", typeof(string));
            TblDetallePropuesta.Columns.Add("Usu", typeof(string));
            TblDetallePropuesta.Columns.Add("IdReporte", typeof(int));
            TblDetallePropuesta.Columns.Add("EstadoPosicion", typeof(string));
            TblDetallePropuesta.Columns.Add("CantidadEntregada", typeof(int));
            TblDetallePropuesta.Columns.Add("UnidadMedida", typeof(string));
            TblDetallePropuesta.Columns.Add("CodMoneda", typeof(string));
            TblDetallePropuesta.Columns.Add("ValorMonedaProp", typeof(double));
            TblDetallePropuesta.Columns.Add("IVA", typeof(double));
            TblDetallePropuesta.Columns.Add("ValorTotal", typeof(double));
            TblDetallePropuesta.Columns.Add("ValorConImpuesto", typeof(double));
            TblDetallePropuesta.Columns.Add("UnidMinCompra", typeof(double));
            TblDetallePropuesta.Columns.Add("CodEstado", typeof(string));
            TblDetallePropuesta.Columns.Add("ObservacionesDP", typeof(string));
            TblDetallePropuesta.Columns.Add("PnAlterno", typeof(string));
            TblDetallePropuesta.Columns.Add("TipoCotizacion", typeof(string));
            TblDetallePropuesta.Columns.Add("IdDetPropSrv", typeof(int));
            TblDetallePropuesta.Columns.Add("RepaExterna", typeof(int));
            TblDetallePropuesta.Columns.Add("CantRealDP", typeof(double));
            TblDetallePropuesta.Columns.Add("UndCompraDPV", typeof(string));
            TblDetallePropuesta.Columns.Add("IdConfigCia", typeof(int));
            TblDetallePropuesta.Columns.Add("CodTipoPT", typeof(string));
            TblDetallePropuesta.Columns.Add("Accion", typeof(string));
           
            foreach (var Campo in TypDetallePropuesta)
            {
                TblDetallePropuesta.Rows.Add(new object[]
                {
                    Campo.IdDetPropuesta,
                    Campo.IdPropuesta,
                    Campo.PN,
                    Campo.Descripcion,
                    Campo.CantidadSol,
                    Campo.PorcentajeUtilidad,
                    Campo.ValorUnd,
                    Campo.CostoVenta,
                    Campo.TiempoEntregaDias,
                    Campo.TiempoEntregaDiasCoti,
                    Campo.IdServicio,
                    Campo.SelectComprar,
                    Campo.Posicion,
                    Campo.Aprobado,
                    Campo.NomServicio,
                    Campo.Usu,
                    Campo.IdReporte,
                    Campo.EstadoPosicion,
                    Campo.CantidadEntregada,
                    Campo.UnidadMedida,
                    Campo.CodMoneda,
                    Campo.ValorMonedaProp,
                    Campo.IVA,
                    Campo.ValorTotal,
                    Campo.ValorConImpuesto,
                    Campo.UnidMinCompra,
                    Campo.CodEstado,
                    Campo.ObservacionesDP,
                    Campo.PnAlterno,
                    Campo.TipoCotizacion,
                    Campo.IdDetPropSrv,
                    Campo.RepaExterna,
                    Campo.CantRealDP,
                    Campo.UndCompraDPV,
                    Campo.IdConfigCia,
                    Campo.CodTipoPT,
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
                    string VBQuery = "CRUD_DetallePropuesta";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurDetPpt", TblDetallePropuesta);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@Origen", "ASP");
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@GanancNalAnt", VbGananciaNalAnt);
                            SqlParameter Prmtrs3 = sqlCmd.Parameters.AddWithValue("@GanancIntaAnt", VbGananciaIntaAnt);
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
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypDetallePropuesta", Ex.StackTrace.Substring(Ex.StackTrace.Length>300? Ex.StackTrace.Length-300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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