using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypSolicitudPedido
    {
        static public string PMensj, PCodPedido, PPN;
        static public int PIdPedido;
        public int IdPedido { get; set; }
        public string CodPedido { get; set; }
        public DateTime Fechapedido { get; set; }
        public string CodPrioridad { get; set; }
        public string CodResponsable { get; set; }
        public int CodReserva { get; set; }
        public string CodEstado { get; set; }
        public string Obsevacion { get; set; }
        public string CodtipoSolPedido { get; set; }
        public string Ccostos { get; set; }
        public string Usu { get; set; }
        public string CodTipoCodigo { get; set; }
        public DateTime? FechaRemocionSP { get; set; }
        public string Aplicabilidad { get; set; }
        public string Accion { get; set; }

        //-------------  TblDetPedido --------------------
        public int IdDetPedido { get; set; }
        public string CodReferencia { get; set; }
        public string PN { get; set; }
        public string CodUndMedida { get; set; }
        public double CantidadTotal { get; set; }
        public double CantidadAlmacen { get; set; }
        public double CantidadReparacion { get; set; }
        public double CantidadOrden { get; set; }
        public int Posicion { get; set; }
        public int AprobacionDetalle { get; set; }
        public string CodSeguimiento { get; set; }
        public string Descripcion { get; set; }
        public int TipoPedido { get; set; }
        public double CantidadAjustada { get; set; }
        public string Notas { get; set; }
        public int PosicionPr { get; set; }
        public int IdSrvPr { get; set; }
        public int IdReporte { get; set; }
        public int IdDetProPSrvSP { get; set; }
        public int CodIdDetalleResSP { get; set; }
        public DateTime? FechaAprob { get; set; }
        public int CodAeronaveSP { get; set; }



        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypSolicitudPedido> EncPedido, IEnumerable<ClsTypSolicitudPedido> DetPedido)
        {
            DataTable TblEncPedido = new DataTable();
            TblEncPedido.Columns.Add("IdPedido", typeof(int));
            TblEncPedido.Columns.Add("CodPedido", typeof(string));
            TblEncPedido.Columns.Add("Fechapedido", typeof(DateTime));
            TblEncPedido.Columns.Add("CodPrioridad", typeof(string));
            TblEncPedido.Columns.Add("CodResponsable", typeof(string));
            TblEncPedido.Columns.Add("CodReserva", typeof(int));
            TblEncPedido.Columns.Add("CodEstado", typeof(string));
            TblEncPedido.Columns.Add("Obsevacion", typeof(string));
            TblEncPedido.Columns.Add("CodtipoSolPedido", typeof(string));
            TblEncPedido.Columns.Add("Ccostos", typeof(string));
            TblEncPedido.Columns.Add("Usu", typeof(string));
            TblEncPedido.Columns.Add("CodTipoCodigo", typeof(string));
            TblEncPedido.Columns.Add("FechaRemocionSP", typeof(DateTime));
            TblEncPedido.Columns.Add("Aplicabilidad", typeof(string));
            TblEncPedido.Columns.Add("Accion", typeof(string));

            foreach (var Campos in EncPedido)
            {
                TblEncPedido.Rows.Add(new object[]{
                    Campos.IdPedido,
                    Campos.CodPedido,
                    Campos.Fechapedido,
                    Campos.CodPrioridad,
                    Campos.CodResponsable,
                    Campos.CodReserva,
                    Campos.CodEstado,
                    Campos.Obsevacion,
                    Campos.CodtipoSolPedido,
                    Campos.Ccostos,
                    Campos.Usu,
                    Campos.CodTipoCodigo,
                    Campos.FechaRemocionSP,
                    Campos.Aplicabilidad,
                    Campos.Accion,
                });
            }

            DataTable TblDetPedido = new DataTable();
            TblDetPedido.Columns.Add("IdDetPedido", typeof(int));
            TblDetPedido.Columns.Add("CodReferencia", typeof(string));
            TblDetPedido.Columns.Add("PN", typeof(string));
            TblDetPedido.Columns.Add("CodUndMedida", typeof(string));
            TblDetPedido.Columns.Add("CantidadTotal", typeof(double));
            TblDetPedido.Columns.Add("CantidadAlmacen", typeof(double));
            TblDetPedido.Columns.Add("CantidadReparacion", typeof(double));
            TblDetPedido.Columns.Add("CantidadOrden", typeof(double));
            TblDetPedido.Columns.Add("Posicion", typeof(int));
            TblDetPedido.Columns.Add("AprobacionDetalle", typeof(int));
            TblDetPedido.Columns.Add("CodSeguimiento", typeof(string));
            TblDetPedido.Columns.Add("Descripcion", typeof(string));
            TblDetPedido.Columns.Add("TipoPedido", typeof(int));
            TblDetPedido.Columns.Add("CantidadAjustada", typeof(double));
            TblDetPedido.Columns.Add("Notas", typeof(string));
            TblDetPedido.Columns.Add("PosicionPr", typeof(int));
            TblDetPedido.Columns.Add("IdSrvPr", typeof(int));
            TblDetPedido.Columns.Add("IdReporte", typeof(int));
            TblDetPedido.Columns.Add("IdDetProPSrvSP", typeof(int));
            TblDetPedido.Columns.Add("CodIdDetalleResSP", typeof(int));
            TblDetPedido.Columns.Add("FechaAprob", typeof(DateTime));
            TblDetPedido.Columns.Add("CodAeronaveSP", typeof(int));

            foreach (var CampoD in DetPedido)
            {
                TblDetPedido.Rows.Add(new object[] {
                    CampoD.IdDetPedido,
                    CampoD.CodReferencia,
                    CampoD.PN,
                    CampoD.CodUndMedida,
                    CampoD.CantidadTotal,
                    CampoD.CantidadAlmacen,
                    CampoD.CantidadReparacion,
                    CampoD.CantidadOrden,
                    CampoD.Posicion,
                    CampoD.AprobacionDetalle,
                    CampoD.CodSeguimiento,
                    CampoD.Descripcion,
                    CampoD.TipoPedido,
                    CampoD.CantidadAjustada,
                    CampoD.Notas,
                    CampoD.PosicionPr,
                    CampoD.IdSrvPr,
                    CampoD.IdReporte,
                    CampoD.IdDetProPSrvSP,
                    CampoD.CodIdDetalleResSP,
                    CampoD.FechaAprob,
                    CampoD.CodAeronaveSP,
                });
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INSERT_UPDATE_SolPed";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PCodPedido = "";
                            PMensj = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@EncSP", TblEncPedido);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetSP", TblDetPedido);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PIdPedido = Convert.ToInt32(SDR["IdPedido"].ToString().Trim());
                                PCodPedido = HttpUtility.HtmlDecode(SDR["CodPedido"].ToString().Trim());
                                PPN = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
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
        {
            return PMensj;
        }
        public int GetIdSolPed()
        {
            return PIdPedido;
        }
        public string GetCodPedido()
        {
            return PCodPedido;
        }
        public string GetPN()
        {
            return PPN;
        }
    }
}