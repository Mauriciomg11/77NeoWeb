using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgManto
{
    public class ClsTypAsignaciones
    {
        static public string PMensj;
        static public string PNumSP;
        public int CodIdUbicacion { get; set; }
        public string CodUbicaBodegaOrg { get; set; }
        public string CodUbicaBodegaDst { get; set; }
        public string CodElemento { get; set; }
        public string CodTipoElemento { get; set; }
        public string IdentificadorElem { get; set; }
        public int CodAlmacen { get; set; }
        public string CodBodegaOrg { get; set; }
        public string CodBodegaDst { get; set; }
        public double Cantidad { get; set; }
        public DateTime? FechaVence { get; set; }
        public string AplicaFV { get; set; }
        public string Usu { get; set; }
        public string SP { get; set; }
        public string Accion { get; set; }
        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypAsignaciones> Asignacion)
        {
            DataTable TblAsignacion = new DataTable();
            TblAsignacion.Columns.Add("CodIdUbicacion", typeof(int));
            TblAsignacion.Columns.Add("CodUbicaBodegaOrg", typeof(string));
            TblAsignacion.Columns.Add("CodUbicaBodegaDst", typeof(string));
            TblAsignacion.Columns.Add("CodElemento", typeof(string));
            TblAsignacion.Columns.Add("CodTipoElemento", typeof(string));
            TblAsignacion.Columns.Add("IdentificadorElem", typeof(string));
            TblAsignacion.Columns.Add("CodAlmacen", typeof(int));
            TblAsignacion.Columns.Add("CodBodegaOrg", typeof(string));
            TblAsignacion.Columns.Add("CodBodegaDst", typeof(string));
            TblAsignacion.Columns.Add("Cantidad", typeof(double));
            TblAsignacion.Columns.Add("AplicaFV", typeof(string));
            TblAsignacion.Columns.Add("FechaVence", typeof(DateTime));
            TblAsignacion.Columns.Add("Usu", typeof(string));
            TblAsignacion.Columns.Add("SP", typeof(string));
            TblAsignacion.Columns.Add("Accion", typeof(string));

            foreach (var Campos in Asignacion)
            {
                TblAsignacion.Rows.Add(new object[]{
                    Campos.CodIdUbicacion,
                    Campos.CodUbicaBodegaOrg,
                    Campos.CodUbicaBodegaDst,
                    Campos.CodElemento,
                    Campos.CodTipoElemento,
                    Campos.IdentificadorElem,
                    Campos.CodAlmacen,
                    Campos.CodBodegaOrg,
                    Campos.CodBodegaDst,
                    Campos.Cantidad,
                    Campos.AplicaFV,
                    Campos.FechaVence,
                    Campos.Usu,
                    Campos.SP,
                    Campos.Accion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "UPDATE_Asignacion";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PMensj = "";
                            PNumSP = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@Asignacion", TblAsignacion);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@NT", HttpContext.Current.Session["Nit77Cia"].ToString());
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PNumSP = HttpUtility.HtmlDecode(SDR["CodSP"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "Asignaciones-Incoming";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypAsignaciones", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetNumSP()
        {
            return PNumSP;
        }
    }
}