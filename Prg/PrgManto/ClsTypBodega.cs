using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgManto
{
    public class ClsTypBodega
    {
        static public string PMensj;
        static public int PIdBase;
        public int IdBase { get; set; }
        public string CodBase { get; set; }
        public string NomBase { get; set; }
        public string CodUbicaGeogr { get; set; }
        public string Descripcion { get; set; }
        public string CodTecnico { get; set; }
        public string FrecuenciaRadio { get; set; }
        public string Fax { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Usu { get; set; }
        public int Activo { get; set; }
        public int IdConfigCia { get; set; }
        public string Accion { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypBodega> oBodega)
        {
            DataTable TblBase = new DataTable();
            TblBase.Columns.Add("CodBase", typeof(string));
            TblBase.Columns.Add("NomBase", typeof(string));
            TblBase.Columns.Add("CodUbicaGeogr", typeof(string));
            TblBase.Columns.Add("Descripcion", typeof(string));
            TblBase.Columns.Add("CodTecnico", typeof(string));
            TblBase.Columns.Add("FrecuenciaRadio", typeof(string));
            TblBase.Columns.Add("Fax", typeof(string));
            TblBase.Columns.Add("Telefono", typeof(string));
            TblBase.Columns.Add("Direccion", typeof(string));
            TblBase.Columns.Add("Usu", typeof(string));
            TblBase.Columns.Add("Activo", typeof(int));
            TblBase.Columns.Add("IdConfigCia", typeof(int));
            TblBase.Columns.Add("IdBase", typeof(int));
            TblBase.Columns.Add("Accion", typeof(string));


            foreach (var Campo in oBodega)
            {
                TblBase.Rows.Add(new object[]{
                   Campo.CodBase,
                   Campo.NomBase,
                   Campo.CodUbicaGeogr,
                   Campo.Descripcion,
                   Campo.CodTecnico,
                   Campo.FrecuenciaRadio,
                   Campo.Fax,
                   Campo.Telefono,
                   Campo.Direccion,
                   Campo.Usu,
                   Campo.Activo,
                   Campo.IdConfigCia,
                   Campo.IdBase,
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
                    PIdBase = 0;
                    string VBQuery = "CRUD_Bodega";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurBase", TblBase);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PIdBase = Convert.ToInt32(SDR["IdBase"].ToString());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmBase";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypBodega", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public int GetIdBase()
        {
            return PIdBase;
        }
    }
}