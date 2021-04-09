using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgManto
{
    public class ClsUbicaGeograf
    {
        static public string PMensj;
        static public int PId;
        public int IdUbicaGeogr { get; set; }
        public string CodUbicaGeogr { get; set; }
        public string Nombre { get; set; }
        public string CodUbicaGeoSup { get; set; }
        public string CodTipoUbicaGeogr { get; set; }
        public string Usu { get; set; }
        public double VlorTasa { get; set; }
        public int Activa { get; set; }
        public int RutaFrecuente { get; set; }
        public int IdConfigCia { get; set; }
        public string Accion { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsUbicaGeograf> oUbicacionGeografica)
        {
            DataTable TblUbicacionGeografica = new DataTable();
            TblUbicacionGeografica.Columns.Add("IdUbicaGeogr", typeof(int));
            TblUbicacionGeografica.Columns.Add("CodUbicaGeogr", typeof(string));
            TblUbicacionGeografica.Columns.Add("Nombre", typeof(string));
            TblUbicacionGeografica.Columns.Add("CodUbicaGeoSup", typeof(string));
            TblUbicacionGeografica.Columns.Add("CodTipoUbicaGeogr", typeof(string));
            TblUbicacionGeografica.Columns.Add("Usu", typeof(string));
            TblUbicacionGeografica.Columns.Add("VlorTasa", typeof(double));
            TblUbicacionGeografica.Columns.Add("Activa", typeof(int));
            TblUbicacionGeografica.Columns.Add("RutaFrecuente", typeof(int));
            TblUbicacionGeografica.Columns.Add("IdConfigCia", typeof(int));
            TblUbicacionGeografica.Columns.Add("Accion", typeof(string));


            foreach (var Campo in oUbicacionGeografica)
            {
                TblUbicacionGeografica.Rows.Add(new object[]{
                    Campo.IdUbicaGeogr,
                    Campo.CodUbicaGeogr,
                    Campo.Nombre,
                    Campo.CodUbicaGeoSup,
                    Campo.CodTipoUbicaGeogr,
                    Campo.Usu,
                    Campo.VlorTasa,
                    Campo.Activa,
                    Campo.RutaFrecuente,
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
                    PMensj = "";
                    PId = 0;
                    string VBQuery = "CRUD_UbicacionGeografica";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurUbicaGeogrf", TblUbicacionGeografica);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PId = Convert.ToInt32(SDR["Id"].ToString());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmUbicacionGeografica";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsUbicaGeograf", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public int GetId()
        {
            return PId;
        }
    }
}