﻿using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgAlmacen
{
    public class CsInsertElementoAlmacen
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PPN;
        static public string PSN;
        static public string PLote;
        static public string PReferencia;
        static public string PFormOrign;

        public int IdIE { get; set; }
        public string CodElemento { get; set; }
        public string CodReferencia { get; set; }
        public string PN { get; set; }
        public string SN { get; set; }
        public string Lote { get; set; }
        public string CodTipoElem { get; set; }
        public string Identificador { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public double CantidadAnt { get; set; }
        public double Valor { get; set; }
        public string CodUndMed { get; set; }
        public int IdAlmacen { get; set; }
        public string CodBodega { get; set; }
        public string CodShippingOrder { get; set; }
        public string Posicion { get; set; }
        public int CodAeronave { get; set; }
        public string Matricula { get; set; }
        public string DiaTasa { get; set; }
        public string MesTasa { get; set; }
        public string AnoTasa { get; set; }
        public double VlorTasaDM { get; set; }
        public string CodTipoMoneda { get; set; }
        public string DocumentoNro { get; set; }
        public int PosicionDocumento { get; set; }
        public double Cant_Compra { get; set; }
        public double Valor_Compra { get; set; }
        public string UndMed_Compra { get; set; }
        public string FacturaNro { get; set; }
        public string NumSolPed { get; set; }
        public string CCosto { get; set; }
        public int AfectaInventario { get; set; }
        public double CostoImportacion { get; set; }
        public string CodTercero { get; set; }
        public int Consignacion { get; set; }
        public int CodIdUbicacion { get; set; }
        public DateTime? FechaVence { get; set; }
        public string Observacion { get; set; }
        public double ValorOT { get; set; }
        public string CodUsuarioReserva { get; set; }
        public string Proceso { get; set; }
        public int IdDetPropHk { get; set; }
        public int IdPPt { get; set; }
        public string Accion { get; set; }

        public void FormOrigen(string formulario)
        { PFormOrign = formulario; }
        public void Alimentar(IEnumerable<CsInsertElementoAlmacen> TypDetalle)
        {
            DataTable TblDetalle = new DataTable();
            TblDetalle.Columns.Add("IdIE", typeof(int));
            TblDetalle.Columns.Add("CodElemento", typeof(string));
            TblDetalle.Columns.Add("CodReferencia", typeof(string));
            TblDetalle.Columns.Add("PN", typeof(string));
            TblDetalle.Columns.Add("SN", typeof(string));
            TblDetalle.Columns.Add("Lote", typeof(string));
            TblDetalle.Columns.Add("CodTipoElem", typeof(string));
            TblDetalle.Columns.Add("Identificador", typeof(string));
            TblDetalle.Columns.Add("Descripcion", typeof(string));
            TblDetalle.Columns.Add("Cantidad", typeof(double));
            TblDetalle.Columns.Add("CantidadAnt", typeof(double));
            TblDetalle.Columns.Add("Valor", typeof(double));
            TblDetalle.Columns.Add("CodUndMed", typeof(string));
            TblDetalle.Columns.Add("IdAlmacen", typeof(int));
            TblDetalle.Columns.Add("CodBodega", typeof(string));
            TblDetalle.Columns.Add("CodShippingOrder", typeof(string));
            TblDetalle.Columns.Add("Posicion", typeof(string));
            TblDetalle.Columns.Add("CodAeronave", typeof(int));
            TblDetalle.Columns.Add("Matricula", typeof(string));
            TblDetalle.Columns.Add("DiaTasa", typeof(string));
            TblDetalle.Columns.Add("MesTasa", typeof(string));
            TblDetalle.Columns.Add("AnoTasa", typeof(string));
            TblDetalle.Columns.Add("VlorTasaDM", typeof(double));
            TblDetalle.Columns.Add("CodTipoMoneda", typeof(string));
            TblDetalle.Columns.Add("DocumentoNro", typeof(string));
            TblDetalle.Columns.Add("PosicionDocumento", typeof(int));
            TblDetalle.Columns.Add("Cant_Compra", typeof(double));
            TblDetalle.Columns.Add("Valor_Compra", typeof(double));
            TblDetalle.Columns.Add("UndMed_Compra", typeof(string));
            TblDetalle.Columns.Add("FacturaNro", typeof(string));
            TblDetalle.Columns.Add("NumSolPed", typeof(string));
            TblDetalle.Columns.Add("CCosto", typeof(string));
            TblDetalle.Columns.Add("AfectaInventario", typeof(int));
            TblDetalle.Columns.Add("CostoImportacion", typeof(double));
            TblDetalle.Columns.Add("CodTercero", typeof(string));
            TblDetalle.Columns.Add("Consignacion", typeof(int));
            TblDetalle.Columns.Add("CodIdUbicacion", typeof(int));
            TblDetalle.Columns.Add("FechaVence", typeof(DateTime));
            TblDetalle.Columns.Add("Observacion", typeof(string));
            TblDetalle.Columns.Add("ValorOT", typeof(double));
            TblDetalle.Columns.Add("CodUsuarioReserva", typeof(string));
            TblDetalle.Columns.Add("Proceso", typeof(string));
            TblDetalle.Columns.Add("IdDetPropHk", typeof(int));
            TblDetalle.Columns.Add("IdPPt", typeof(int));
            TblDetalle.Columns.Add("Accion", typeof(string));
            foreach (var Campo in TypDetalle)
            {
                TblDetalle.Rows.Add(new object[]
                  {
                    Campo.IdIE,
                    Campo.CodElemento,
                    Campo.CodReferencia,
                    Campo.PN,
                    Campo.SN,
                    Campo.Lote,
                    Campo.CodTipoElem,
                    Campo.Identificador,
                    Campo.Descripcion,
                    Campo.Cantidad,
                    Campo.CantidadAnt,
                    Campo.Valor,
                    Campo.CodUndMed,
                    Campo.IdAlmacen,
                    Campo.CodBodega,
                    Campo.CodShippingOrder,
                    Campo.Posicion,
                    Campo.CodAeronave,
                    Campo.Matricula,
                    Campo.DiaTasa,
                    Campo.MesTasa,
                    Campo.AnoTasa,
                    Campo.VlorTasaDM,
                    Campo.CodTipoMoneda,
                    Campo.DocumentoNro,
                    Campo.PosicionDocumento,
                    Campo.Cant_Compra,
                    Campo.Valor_Compra,
                    Campo.UndMed_Compra,
                    Campo.FacturaNro,
                    Campo.NumSolPed,
                    Campo.CCosto,
                    Campo.AfectaInventario,
                    Campo.CostoImportacion,
                    Campo.CodTercero,
                    Campo.Consignacion,
                    Campo.CodIdUbicacion,
                    Campo.FechaVence,
                    Campo.Observacion,
                    Campo.ValorOT,
                    Campo.CodUsuarioReserva,
                    Campo.Proceso,
                    Campo.IdDetPropHk,
                    Campo.IdPPt,
                    Campo.Accion,
                  });
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = ""; PN = "";
                    string VBQuery = "INSERT_EntradaElemento";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurInsertElementos", TblDetalle);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PReferencia = HttpUtility.HtmlDecode(SDR["CodReferencia"].ToString().Trim());
                                PPN = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
                                PSN = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                                PLote = HttpUtility.HtmlDecode(SDR["Lote"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            PMensj= Ex.Message;
                            transaction.Rollback();
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = PFormOrign;
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsInsertElementoAlmacen", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj() { return PMensj; }
        public string GetReferencia() { return PReferencia; }
        public string GetPn() { return PPN; }
        public string GetSn() { return PSN; }
        public string GetLote() { return PLote; }
    }
}