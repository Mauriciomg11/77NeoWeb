using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypAsingarBodega
    {
        
        public int CodIdUbicacion { get; set; }
        public string CodElemento { get; set; }
        public int CodAlmacen { get; set; }
        public string CodBodega { get; set; }
        public double Cantidad { get; set; }
        public string Usu { get; set; }
        public string Accion { get; set; }


        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypAsingarBodega> ElementoBodega)
        {
            DataTable TblElementoBodega = new DataTable();
            TblElementoBodega.Columns.Add("CodIdUbicacion", typeof(int));
            TblElementoBodega.Columns.Add("CodElemento", typeof(string));
            TblElementoBodega.Columns.Add("CodAlmacen", typeof(int));
            TblElementoBodega.Columns.Add("CodBodega", typeof(string));
            TblElementoBodega.Columns.Add("Cantidad", typeof(double));
            TblElementoBodega.Columns.Add("Usu", typeof(string));
            TblElementoBodega.Columns.Add("Accion", typeof(string));           

            foreach (var Campos in ElementoBodega)
            {
                TblElementoBodega.Rows.Add(new object[]{
                    Campos.CodIdUbicacion,
                    Campos.CodElemento,
                    Campos.CodAlmacen,
                    Campos.CodBodega,
                    Campos.Cantidad,
                    Campos.Usu,                   
                    Campos.Accion,
                });
            }     
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_ElemBod";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {                          
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurElemBod", TblElementoBodega);
                            SqlParameter Prmtrs1 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SC.ExecuteNonQuery();                           
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "Insert Masivamete ElementoBodega";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypAsingarBodega", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }

        }       
    }
}