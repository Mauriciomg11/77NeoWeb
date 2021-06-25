using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class ClsTypAeronaveVirtualCrearElem
    {
        static public string PMensj;
        static public string borrar;
        public string TipoEvento { get; set; }
        public int CodAeronave { get; set; }
        public string CodModelo  { get; set; }
        public string NivelElemento { get; set; }
        public string Motor { get; set; }
        public string UltimoNivel { get; set; }
        public string CodMayor { get; set; }
        public string CodElemento { get; set; }
        public string Pn { get; set; }
        public string Sn { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Posicion { get; set; }
        public string Usu { get; set; }
        public string MotivoRemocion { get; set; }

        //-------------  Servicios --------------------
        public int CodIdContadorElem { get; set; }
        public string CodElementoSvc { get; set; }        
        public DateTime? FechaVence { get; set; }
        public DateTime? FechaVenceAnt { get; set; }
        public int Resetear { get; set; }
        public int CodOT { get; set; }
        public int CodIdContaSrvManto { get; set; }
        public string NumReporte { get; set; }
        public double ValorUltCump { get; set; }
        public string GeneraHist { get; set; }        

       

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypAeronaveVirtualCrearElem> AeronaveVirtual, IEnumerable<ClsTypAeronaveVirtualCrearElem> ServicioManto)
        {
            DataTable TblAeronaveVirtual = new DataTable();
            TblAeronaveVirtual.Columns.Add("TipoEvento", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodAeronave", typeof(int));
            TblAeronaveVirtual.Columns.Add("CodModelo", typeof(string));
            TblAeronaveVirtual.Columns.Add("NivelElemento", typeof(string));
            TblAeronaveVirtual.Columns.Add("Motor", typeof(string));
            TblAeronaveVirtual.Columns.Add("UltimoNivel", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodMayor", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodElemento", typeof(string));
            TblAeronaveVirtual.Columns.Add("Pn", typeof(string));
            TblAeronaveVirtual.Columns.Add("Sn", typeof(string));
            TblAeronaveVirtual.Columns.Add("FechaEvento", typeof(DateTime));
            TblAeronaveVirtual.Columns.Add("Posicion", typeof(string));
            TblAeronaveVirtual.Columns.Add("Usu", typeof(string));
            TblAeronaveVirtual.Columns.Add("MotivoRemocion", typeof(string));
            foreach (var Campo in AeronaveVirtual)
            {
                TblAeronaveVirtual.Rows.Add(new object[]{
                    Campo.TipoEvento,
                    Campo.CodAeronave,
                    Campo.CodModelo,
                    Campo.NivelElemento,
                    Campo.Motor,
                    Campo.UltimoNivel,
                    Campo.CodMayor,
                    Campo.CodElemento,
                    Campo.Pn,
                    Campo.Sn,
                    Campo.FechaEvento,
                    Campo.Posicion,
                    Campo.Usu,
                    Campo.MotivoRemocion,
                });
            }

            DataTable TblServicios = new DataTable();
            TblServicios.Columns.Add("CodIdContadorElem", typeof(int));
            TblServicios.Columns.Add("CodElementoSvc", typeof(string));
            TblServicios.Columns.Add("FechaVence", typeof(DateTime));
            TblServicios.Columns.Add("FechaVenceAnt", typeof(DateTime));
            TblServicios.Columns.Add("Resetear", typeof(int));
            TblServicios.Columns.Add("CodOT", typeof(int));
            TblServicios.Columns.Add("CodIdContaSrvManto", typeof(int));
            TblServicios.Columns.Add("NumReporte", typeof(string));
            TblServicios.Columns.Add("ValorUltCump", typeof(double));
            TblServicios.Columns.Add("GeneraHist", typeof(string));

            foreach (var CampoSvc in ServicioManto)
            {
                TblServicios.Rows.Add(new object[] {
                    CampoSvc.CodIdContadorElem,
                    CampoSvc.CodElementoSvc,
                    CampoSvc.FechaVence,
                    CampoSvc.FechaVenceAnt,
                    CampoSvc.Resetear,
                    CampoSvc.CodOT,
                    CampoSvc.CodIdContaSrvManto,
                    CampoSvc.NumReporte,
                    CampoSvc.ValorUltCump,
                    CampoSvc.GeneraHist,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_AeroVirtualCrearElem";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        PMensj = "Inconveniente en el movimiento";
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurAeroVirtual", TblAeronaveVirtual);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@CurServManto", TblServicios);
                            SqlParameter Prmtrs1 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;                          
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); 
                                string CodElemento = HttpUtility.HtmlDecode(SDR["CodElemento"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmAeronaveVirtual";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypAeronaveVirtualCrearElem", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetBorrar()
        {
            return borrar;
        }
    }
}