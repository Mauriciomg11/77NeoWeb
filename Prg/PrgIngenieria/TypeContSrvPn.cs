using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class TypeContSrvPn
    {
        public int CodidcodSrvPn { get; set; }
        public string CodServicioManto { get; set; }
        public int CodIdContadorPn { get; set; }
        public int Frecuencia { get; set; }
        public int NroDias { get; set; }
        public string Usu { get; set; }
        public int banUnicoCumplimiento { get; set; }
        public int Resetear { get; set; }
        public string Accion { get; set; }
        public string PN { get; set; }
        public string CodContador { get; set; }
        public void Alimentar(IEnumerable<TypeContSrvPn> TblContSrvPn)
        {
            DataTable table = new DataTable();

            table.Columns.Add("CodidcodSrvPn", typeof(int));
            table.Columns.Add("CodServicioManto", typeof(string));
            table.Columns.Add("CodIdContadorPn", typeof(int));
            table.Columns.Add("Frecuencia", typeof(int));
            table.Columns.Add("NroDias", typeof(int));
            table.Columns.Add("Usu", typeof(string));
            table.Columns.Add("banUnicoCumplimiento", typeof(int));
            table.Columns.Add("Resetear", typeof(int));
            table.Columns.Add("Accion", typeof(string));
            table.Columns.Add("PN", typeof(string));
            table.Columns.Add("CodContador", typeof(string));


            foreach (var Campos in TblContSrvPn)
            {
                table.Rows.Add(new object[]
                    {
                        Campos.CodidcodSrvPn,
                        Campos.CodServicioManto,
                        Campos.CodIdContadorPn,
                        Campos.Frecuencia,
                        Campos.NroDias,
                        Campos.Usu,
                        Campos.banUnicoCumplimiento,
                        Campos.Resetear,
                        Campos.Accion,
                        Campos.PN,
                        Campos.CodContador,
                    });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "SP_InsUpd_ContSrvPn";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurVD", table);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            sqlCmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmServicioManto";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "TypeContSrvPn", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        ClsConexion Cnx = new ClsConexion();
    }
}