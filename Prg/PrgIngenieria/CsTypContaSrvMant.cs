using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypContaSrvMant
    {       
        public int CodIdContaSrvManto { get; set; }
        public int? CodAeronave { get; set; }
        public int? CodElemento { get; set; }
        public string CodServicioManto { get; set; }
        public double Frecuencia { get; set; }
        public double Extension { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public double NroDias { get; set; }
        public double ExtensionDias { get; set; }
        public int BanOrdenTrabajo { get; set; }
        public string Usu { get; set; }
        public int banUnicoCumplimiento { get; set; }
        public int? CodOt { get; set; }
        public double Compensacion { get; set; }
        public int Resetear { get; set; }
        public double FrecuenciaInicial { get; set; }
        public int FrecuenciaInicalEjecutada { get; set; }
        public string CodContador { get; set; }
        public string CodElem { get; set; }
        public string PN { get; set; }
        public string Accion { get; set; }
        public string Aplicabilidad { get; set; }
        public string CrearHistorico { get; set; }
        public string Historico { get; set; }
        public void Alimentar(IEnumerable<CsTypContaSrvMant> TblContaSrvManto)
        {
            DataTable table = new DataTable();

            table.Columns.Add("CodIdContaSrvManto", typeof(int));
            table.Columns.Add("CodAeronave", typeof(int));
            table.Columns.Add("CodElemento", typeof(int));
            table.Columns.Add("CodServicioManto", typeof(string));
            table.Columns.Add("Frecuencia", typeof(double));
            table.Columns.Add("Extension", typeof(double));
            table.Columns.Add("FechaVencimiento", typeof(DateTime));
            table.Columns.Add("NroDias", typeof(double));
            table.Columns.Add("ExtensionDias", typeof(double));
            table.Columns.Add("BanOrdenTrabajo", typeof(int));
            table.Columns.Add("Usu", typeof(string));
            table.Columns.Add("banUnicoCumplimiento", typeof(int));
            table.Columns.Add("CodOt", typeof(int));
            table.Columns.Add("Compensacion", typeof(double));
            table.Columns.Add("Resetear", typeof(int));
            table.Columns.Add("FrecuenciaInicial", typeof(double));
            table.Columns.Add("FrecuenciaInicalEjecutada", typeof(int));
            table.Columns.Add("CodContador", typeof(string));
            table.Columns.Add("CodElem", typeof(string));
            table.Columns.Add("PN", typeof(string));
            table.Columns.Add("Accion", typeof(string));
            table.Columns.Add("Aplicabilidad", typeof(string));
            table.Columns.Add("CrearHistorico", typeof(string));
            table.Columns.Add("Historico", typeof(string));

            foreach (var Campos in TblContaSrvManto)
            {
                table.Rows.Add(new object[]
                 {
                        Campos.CodIdContaSrvManto,
                        Campos.CodAeronave,
                        Campos.CodElemento,
                        Campos.CodServicioManto,
                        Campos.Frecuencia,
                        Campos.Extension,
                        Campos.FechaVencimiento,
                        Campos.NroDias,
                        Campos.ExtensionDias,
                        Campos.BanOrdenTrabajo,
                        Campos.Usu,
                        Campos.banUnicoCumplimiento,
                        Campos.CodOt,
                        Campos.Compensacion,
                        Campos.Resetear,
                        Campos.FrecuenciaInicial,
                        Campos.FrecuenciaInicalEjecutada,
                        Campos.CodContador,
                        Campos.CodElem,
                        Campos.PN,
                        Campos.Accion,
                        Campos.Aplicabilidad,
                        Campos.CrearHistorico,
                        Campos.Historico,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "SP_InsUpd_ContaSrvManto";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurVD", table);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
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
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypContaSrvMant", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        ClsConexion Cnx = new ClsConexion();
    }
}