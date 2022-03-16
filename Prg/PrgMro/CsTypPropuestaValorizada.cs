using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.prgMro
{
    public class CsTypPropuestaValorizada
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PId;
        public int IdValorizacion { get; set; }
        public int IdPropuesta { get; set; }
        public int IdServicio { get; set; }
        public string NomServicio { get; set; }
        public string PnPropuesta { get; set; }
        public string CodReferencia { get; set; }
        public string Descripcion { get; set; }
        public double? CantidadPropuesta { get; set; }
        public double ValorCompra { get; set; }
        public string DocReferencia { get; set; }
        public DateTime? FechaUltimaCompra { get; set; }
        public int TiempoEntregaDiasCoti { get; set; }
        public string PnStock { get; set; }
        public double CantStock { get; set; }
        public int CodIdUbicacion { get; set; }
        public string Bodega { get; set; }
        public double StockMinimo { get; set; }
        public int CodTipoCotiza { get; set; }
        public int SelectBodeg { get; set; }
        public int SelectSolicitud { get; set; }
        public double CantidadSolicitud { get; set; }
        public string ObservacionValorizar { get; set; }
        public int Posicion { get; set; }
        public string NomBodega { get; set; }
        public int TiempoEntregaDias { get; set; }
        public string Usu { get; set; }
        public int Aprobado { get; set; }
        public int IdReporte { get; set; }
        public string NumPedido { get; set; }
        public string MonedaProVa { get; set; }
        public string UndMedProVa { get; set; }
        public double UnidMinCompra { get; set; }
        public string CodEstado { get; set; }
        public string PnAlternoPV { get; set; }
        public string TipoCotizacion { get; set; }
        public int IdDetPropSrv { get; set; }
        public int RepaExterna { get; set; }
        public double CantRealPV { get; set; }
        public string UndCompraPV { get; set; }
        public double? EquivalenciaPV { get; set; }
        public int OTVal { get; set; }
        public int CodAeronaveVal { get; set; }
        public string MatriculaVal { get; set; }
        public string SNElementoV { get; set; }
        public int IdConfigCia { get; set; }
        public string Accion { get; set; }
        public void Alimentar(IEnumerable<CsTypPropuestaValorizada> TypPropuestaValorizada)
        {
            DataTable TblPropuestaValorizada = new DataTable();
            TblPropuestaValorizada.Columns.Add("IdValorizacion", typeof(int));
            TblPropuestaValorizada.Columns.Add("IdPropuesta", typeof(int));
            TblPropuestaValorizada.Columns.Add("IdServicio", typeof(int));
            TblPropuestaValorizada.Columns.Add("NomServicio", typeof(string));
            TblPropuestaValorizada.Columns.Add("PnPropuesta", typeof(string));
            TblPropuestaValorizada.Columns.Add("CodReferencia", typeof(string));
            TblPropuestaValorizada.Columns.Add("Descripcion", typeof(string));
            TblPropuestaValorizada.Columns.Add("CantidadPropuesta", typeof(double));
            TblPropuestaValorizada.Columns.Add("ValorCompra", typeof(double));
            TblPropuestaValorizada.Columns.Add("DocReferencia", typeof(string));
            TblPropuestaValorizada.Columns.Add("FechaUltimaCompra", typeof(DateTime));
            TblPropuestaValorizada.Columns.Add("TiempoEntregaDiasCoti", typeof(int));
            TblPropuestaValorizada.Columns.Add("PnStock", typeof(string));
            TblPropuestaValorizada.Columns.Add("CantStock", typeof(double));
            TblPropuestaValorizada.Columns.Add("CodIdUbicacion", typeof(int));
            TblPropuestaValorizada.Columns.Add("Bodega", typeof(string));
            TblPropuestaValorizada.Columns.Add("StockMinimo", typeof(double));
            TblPropuestaValorizada.Columns.Add("CodTipoCotiza", typeof(int));
            TblPropuestaValorizada.Columns.Add("SelectBodeg", typeof(int));
            TblPropuestaValorizada.Columns.Add("SelectSolicitud", typeof(int));
            TblPropuestaValorizada.Columns.Add("CantidadSolicitud", typeof(double));
            TblPropuestaValorizada.Columns.Add("ObservacionValorizar", typeof(string));
            TblPropuestaValorizada.Columns.Add("Posicion", typeof(int));
            TblPropuestaValorizada.Columns.Add("NomBodega", typeof(string));
            TblPropuestaValorizada.Columns.Add("TiempoEntregaDias", typeof(int));
            TblPropuestaValorizada.Columns.Add("Usu", typeof(string));
            TblPropuestaValorizada.Columns.Add("Aprobado", typeof(int));
            TblPropuestaValorizada.Columns.Add("IdReporte", typeof(int));
            TblPropuestaValorizada.Columns.Add("NumPedido", typeof(string));
            TblPropuestaValorizada.Columns.Add("MonedaProVa", typeof(string));
            TblPropuestaValorizada.Columns.Add("UndMedProVa", typeof(string));
            TblPropuestaValorizada.Columns.Add("UnidMinCompra", typeof(double));
            TblPropuestaValorizada.Columns.Add("CodEstado", typeof(string));
            TblPropuestaValorizada.Columns.Add("PnAlternoPV", typeof(string));
            TblPropuestaValorizada.Columns.Add("TipoCotizacion", typeof(string));
            TblPropuestaValorizada.Columns.Add("IdDetPropSrv", typeof(int));
            TblPropuestaValorizada.Columns.Add("RepaExterna", typeof(int));
            TblPropuestaValorizada.Columns.Add("CantRealPV", typeof(double));
            TblPropuestaValorizada.Columns.Add("UndCompraPV", typeof(string));
            TblPropuestaValorizada.Columns.Add("EquivalenciaPV", typeof(double));
            TblPropuestaValorizada.Columns.Add("OTVal", typeof(int));
            TblPropuestaValorizada.Columns.Add("CodAeronaveVal", typeof(int));
            TblPropuestaValorizada.Columns.Add("MatriculaVal", typeof(string));
            TblPropuestaValorizada.Columns.Add("SNElementoV", typeof(string));
            TblPropuestaValorizada.Columns.Add("IdConfigCia", typeof(int));
            TblPropuestaValorizada.Columns.Add("Accion", typeof(string));

            foreach (var Campo in TypPropuestaValorizada)
            {
                TblPropuestaValorizada.Rows.Add(new object[]
                {
                    Campo.IdValorizacion,
                    Campo.IdPropuesta,
                    Campo.IdServicio,
                    Campo.NomServicio,
                    Campo.PnPropuesta,
                    Campo.CodReferencia,
                    Campo.Descripcion,
                    Campo.CantidadPropuesta,
                    Campo.ValorCompra,
                    Campo.DocReferencia,
                    Campo.FechaUltimaCompra,
                    Campo.TiempoEntregaDiasCoti,
                    Campo.PnStock,
                    Campo.CantStock,
                    Campo.CodIdUbicacion,
                    Campo.Bodega,
                    Campo.StockMinimo,
                    Campo.CodTipoCotiza,
                    Campo.SelectBodeg,
                    Campo.SelectSolicitud,
                    Campo.CantidadSolicitud,
                    Campo.ObservacionValorizar,
                    Campo.Posicion,
                    Campo.NomBodega,
                    Campo.TiempoEntregaDias,
                    Campo.Usu,
                    Campo.Aprobado,
                    Campo.IdReporte,
                    Campo.NumPedido,
                    Campo.MonedaProVa,
                    Campo.UndMedProVa,
                    Campo.UnidMinCompra,
                    Campo.CodEstado,
                    Campo.PnAlternoPV,
                    Campo.TipoCotizacion,
                    Campo.IdDetPropSrv,
                    Campo.RepaExterna,
                    Campo.CantRealPV,
                    Campo.UndCompraPV,
                    Campo.EquivalenciaPV,
                    Campo.OTVal,
                    Campo.CodAeronaveVal,
                    Campo.MatriculaVal,
                    Campo.SNElementoV,
                    Campo.IdConfigCia,
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
                    string VBQuery = "CRUD_ValorizacionPpt";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurValorizaPpt", TblPropuestaValorizada);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
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
                            string  VbEx = Ex.StackTrace.ToString();
                            /*string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmValorizacion";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();*/
                            //Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypPropuestaValorizada", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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