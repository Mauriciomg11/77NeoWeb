using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypReparacion
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj, PCodRepa, PCodCotiza, VbAccion, VbPN;

        public string CodReparacion { get; set; }
        public string CodCotizacion { get; set; }
        public string CodPedido { get; set; }
        public string CodProveedor { get; set; }
        public string CodEmpleado { get; set; }
        public string CodAutorizador { get; set; }
        public string CodTipoOrdenRepa { get; set; }
        public string CodTransportador { get; set; }
        public string CodUbicacionCia { get; set; }
        public DateTime? FechaReparacion { get; set; }
        public string CodEstadoRepa { get; set; }
        public int Garantia { get; set; }
        public string Sn { get; set; }
        public int AOG { get; set; }
        public string RazonRemocion { get; set; }
        public string Instruccion { get; set; }
        public string Observacion { get; set; }
        public string EngineerBull { get; set; }
        public string Otros { get; set; }
        public int Aprobado { get; set; }
        public int OverHaul { get; set; }
        public int CompMayor { get; set; }
        public int SalidaRepa { get; set; }
        public string obstransportador { get; set; }
        public int Asentado { get; set; }
        public string CodAsiento { get; set; }
        public int Recibido { get; set; }
        public int CodNumOrdenTrab { get; set; }
        public int TipoReparacion { get; set; }
        public double Cantidad { get; set; }
        public string ReembolsableProv { get; set; }
        public string CuentaPuc { get; set; }
        public string NumFactura { get; set; }
        public string PNRepa { get; set; }
        public int CodAeronaveRO { get; set; }
        public int PPT { get; set; }

        //-------------  TblDetCotiza --------------------
        public int IDRepaDetSolPed { get; set; }
        public int IdDetPedido { get; set; }
        public int IdPedido { get; set; }
        public int Posicion { get; set; }

        public void Accion(string Accion)
        { VbAccion = Accion; }
        public void Alimentar(IEnumerable<ClsTypReparacion> TypEncRepa, IEnumerable<ClsTypReparacion> TypDetRepa)
        {
            DataTable TblEncRepa = new DataTable();
            TblEncRepa.Columns.Add("CodReparacion", typeof(string));
            TblEncRepa.Columns.Add("CodCotizacion", typeof(string));
            TblEncRepa.Columns.Add("CodPedido", typeof(string));
            TblEncRepa.Columns.Add("CodProveedor", typeof(string));
            TblEncRepa.Columns.Add("CodEmpleado", typeof(string));
            TblEncRepa.Columns.Add("CodAutorizador", typeof(string));
            TblEncRepa.Columns.Add("CodTipoOrdenRepa", typeof(string));
            TblEncRepa.Columns.Add("CodTransportador", typeof(string));
            TblEncRepa.Columns.Add("CodUbicacionCia", typeof(string));
            TblEncRepa.Columns.Add("FechaReparacion", typeof(DateTime));
            TblEncRepa.Columns.Add("CodEstadoRepa", typeof(string));
            TblEncRepa.Columns.Add("Garantia", typeof(int));
            TblEncRepa.Columns.Add("Sn", typeof(string));
            TblEncRepa.Columns.Add("AOG", typeof(int));
            TblEncRepa.Columns.Add("RazonRemocion", typeof(string));
            TblEncRepa.Columns.Add("Instruccion", typeof(string));
            TblEncRepa.Columns.Add("Observacion", typeof(string));
            TblEncRepa.Columns.Add("EngineerBull", typeof(string));
            TblEncRepa.Columns.Add("Otros", typeof(string));
            TblEncRepa.Columns.Add("Aprobado", typeof(int));
            TblEncRepa.Columns.Add("OverHaul", typeof(int));
            TblEncRepa.Columns.Add("CompMayor", typeof(int));
            TblEncRepa.Columns.Add("SalidaRepa", typeof(int));
            TblEncRepa.Columns.Add("obstransportador", typeof(string));
            TblEncRepa.Columns.Add("Asentado", typeof(int));
            TblEncRepa.Columns.Add("CodAsiento", typeof(string));
            TblEncRepa.Columns.Add("Recibido", typeof(int));
            TblEncRepa.Columns.Add("CodNumOrdenTrab", typeof(int));
            TblEncRepa.Columns.Add("TipoReparacion", typeof(int));
            TblEncRepa.Columns.Add("Cantidad", typeof(double));
            TblEncRepa.Columns.Add("ReembolsableProv", typeof(string));
            TblEncRepa.Columns.Add("CuentaPuc", typeof(string));
            TblEncRepa.Columns.Add("NumFactura", typeof(string));
            TblEncRepa.Columns.Add("PNRepa", typeof(string));
            TblEncRepa.Columns.Add("CodAeronaveRO", typeof(int));
            TblEncRepa.Columns.Add("PPT", typeof(int));

            foreach (var Campo in TypEncRepa)
            {
                TblEncRepa.Rows.Add(new object[]{
                   Campo.CodReparacion,
                   Campo.CodCotizacion,
                   Campo.CodPedido,
                   Campo.CodProveedor,
                   Campo.CodEmpleado,
                   Campo.CodAutorizador,
                   Campo.CodTipoOrdenRepa,
                   Campo.CodTransportador,
                   Campo.CodUbicacionCia,
                   Campo.FechaReparacion,
                   Campo.CodEstadoRepa,
                   Campo.Garantia,
                   Campo.Sn,
                   Campo.AOG,
                   Campo.RazonRemocion,
                   Campo.Instruccion,
                   Campo.Observacion,
                   Campo.EngineerBull,
                   Campo.Otros,
                   Campo.Aprobado,
                   Campo.OverHaul,
                   Campo.CompMayor,
                   Campo.SalidaRepa,
                   Campo.obstransportador,
                   Campo.Asentado,
                   Campo.CodAsiento,
                   Campo.Recibido,
                   Campo.CodNumOrdenTrab,
                   Campo.TipoReparacion,
                   Campo.Cantidad,
                   Campo.ReembolsableProv,
                   Campo.CuentaPuc,
                   Campo.NumFactura,
                   Campo.PNRepa,
                   Campo.CodAeronaveRO,
                   Campo.PPT,
                 });
            }

            DataTable TblDetRepa = new DataTable();
            TblDetRepa.Columns.Add("IDRepaDetSolPed", typeof(int));
            TblDetRepa.Columns.Add("IdDetPedido", typeof(int));
            TblDetRepa.Columns.Add("IdPedido", typeof(int));
            TblDetRepa.Columns.Add("Posicion", typeof(int));
            foreach (var Campo in TypDetRepa)
            {
                TblDetRepa.Rows.Add(new object[]{
                    Campo.IDRepaDetSolPed,
                    Campo.IdDetPedido,
                    Campo.IdPedido,
                    Campo.Posicion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_Reparacion";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PCodCotiza = "";
                            PMensj = "";
                            VbPN = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@Repa", TblEncRepa);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetRepa", TblDetRepa);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                            SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodRepa = SDR["CodRepa"].ToString().Trim();
                                PCodCotiza = HttpUtility.HtmlDecode(SDR["CodCotiza"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "Generar Reparacion";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypReparacion", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        { return PMensj; }
        public string GetCodRepa()
        { return PCodRepa; }
        public string GetCodCotiza()
        { return PCodCotiza; }
    }
}