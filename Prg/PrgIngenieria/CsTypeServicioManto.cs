using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypeServicioManto
    {        
        static public int VbID;
        public int IdSrvManto { get; set; }
        public string CodServicioManto { get; set; }
        public string CodPatronManto { get; set; }
        public string Descripcion { get; set; }
        public string NroDocumento { get; set; }
        public string CodCapitulo { get; set; }
        public string BadPlan { get; set; }
        public string Bandera { get; set; }
        public int BanTipoSrv { get; set; }
        public string Usu { get; set; }
        public int NroEtapas { get; set; }
        public int EtapaActual { get; set; }
        public string SubAta { get; set; }
        public int ConsecutivoAta { get; set; }
        public int IdTipoSrv { get; set; }
        public int AD { get; set; }
        public int SB { get; set; }
        public double HorizonteApertura { get; set; }
        public string Referencia { get; set; }
        public string CodModeloSM { get; set; }
        public string PnMayor { get; set; }
        public int SubComponenteSM { get; set; }
        public string CodTaller { get; set; }
        public string CodReferenciaSrv { get; set; }
        public string Catalogo { get; set; }
        public int? ValidarRecurso { get; set; }
        public int VisualizarStatus { get; set; }
        public string ServicioMayor { get; set; }
        public string Accion { get; set; }
        public string Aplicabilidad { get; set; }       

        public void Alimentar(IEnumerable<CsTypeServicioManto> TblServicioManto)
        {
            DataTable table = new DataTable();

            table.Columns.Add("IdSrvManto", typeof(int));
            table.Columns.Add("CodServicioManto", typeof(string));
            table.Columns.Add("CodPatronManto", typeof(string));
            table.Columns.Add("Descripcion", typeof(string));
            table.Columns.Add("NroDocumento", typeof(string));
            table.Columns.Add("CodCapitulo", typeof(string));
            table.Columns.Add("BadPlan", typeof(string));
            table.Columns.Add("Bandera", typeof(string));
            table.Columns.Add("BanTipoSrv", typeof(int));
            table.Columns.Add("Usu", typeof(string));
            table.Columns.Add("NroEtapas", typeof(int));
            table.Columns.Add("EtapaActual", typeof(int));
            table.Columns.Add("SubAta", typeof(string));
            table.Columns.Add("ConsecutivoAta", typeof(int));
            table.Columns.Add("IdTipoSrv", typeof(int));
            table.Columns.Add("AD", typeof(int));
            table.Columns.Add("SB", typeof(int));
            table.Columns.Add("HorizonteApertura", typeof(double));
            table.Columns.Add("Referencia", typeof(string));
            table.Columns.Add("CodModeloSM", typeof(string));
            table.Columns.Add("PnMayor", typeof(string));
            table.Columns.Add("SubComponenteSM", typeof(int));
            table.Columns.Add("CodTaller", typeof(string));
            table.Columns.Add("CodReferenciaSrv", typeof(string));
            table.Columns.Add("Catalogo", typeof(string));
            table.Columns.Add("ValidarRecurso", typeof(int));
            table.Columns.Add("VisualizarStatus", typeof(int));
            table.Columns.Add("ServicioMayor", typeof(string));
            table.Columns.Add("Accion", typeof(string));
            table.Columns.Add("Aplicabilidad", typeof(string));

            foreach (var Campos in TblServicioManto)
            {
                table.Rows.Add(new object[]
                    {
                        Campos.IdSrvManto,
                        Campos.CodServicioManto,
                        Campos.CodPatronManto,
                        Campos.Descripcion,
                        Campos.NroDocumento,
                        Campos.CodCapitulo,
                        Campos.BadPlan,
                        Campos.Bandera,
                        Campos.BanTipoSrv,
                        Campos.Usu,
                        Campos.NroEtapas,
                        Campos.EtapaActual,
                        Campos.SubAta,
                        Campos.ConsecutivoAta,
                        Campos.IdTipoSrv,
                        Campos.AD,
                        Campos.SB,
                        Campos.HorizonteApertura,
                        Campos.Referencia,
                        Campos.CodModeloSM,
                        Campos.PnMayor,
                        Campos.SubComponenteSM,
                        Campos.CodTaller,
                        Campos.CodReferenciaSrv,
                         Campos.Catalogo,
                        Campos.ValidarRecurso,
                        Campos.VisualizarStatus,
                        Campos.ServicioMayor,
                        Campos.Accion,
                        Campos.Aplicabilidad,
                    });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "SP_InsUpd_ServicioManto";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurVD", table);
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            //sqlCmd.ExecuteNonQuery();
                            var Id = sqlCmd.ExecuteScalar();
                            if (Id!=null)
                            { VbID = Convert.ToInt32(Id.ToString()); }                            
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmServicioManto";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypeServicioManto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public int GetID()
        {
            return VbID;
        }
        ClsConexion Cnx = new ClsConexion();
    }
}