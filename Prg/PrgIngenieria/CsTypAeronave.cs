using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypAeronave
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public int  PCodHK;
        public int CodAeronave { get; set; }
        public string SN { get; set; }
        public string Matricula { get; set; }
        public DateTime FechaFabricante { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string CodModelo { get; set; }
        public string CodPropietario { get; set; }
        public string CodTipoAeronave { get; set; }
        public string CodProveedor { get; set; }
        public string CodEstadoAeronave { get; set; }
        public string Activo { get; set; }
        public string Descripcion { get; set; }
        public double HoraVoladaIng { get; set; }
        public string Usu { get; set; }
        public int Bloqueada { get; set; }
        public int Propiedad { get; set; }
        public string CentroDeCosto { get; set; }
        public int PropiedadCia { get; set; }
        public int CSN { get; set; }
        public string Accion { get; set; }

        public void Alimentar(IEnumerable<CsTypAeronave> TypAeronave)//
        {
            DataTable TblAeronave = new DataTable();
            TblAeronave.Columns.Add("CodAeronave", typeof(int));
            TblAeronave.Columns.Add("SN", typeof(string));
            TblAeronave.Columns.Add("Matricula", typeof(string));
            TblAeronave.Columns.Add("FechaFabricante", typeof(DateTime));
            TblAeronave.Columns.Add("FechaIngreso", typeof(DateTime));
            TblAeronave.Columns.Add("CodModelo", typeof(string));
            TblAeronave.Columns.Add("CodPropietario", typeof(string));
            TblAeronave.Columns.Add("CodTipoAeronave", typeof(string));
            TblAeronave.Columns.Add("CodProveedor", typeof(string));
            TblAeronave.Columns.Add("CodEstadoAeronave", typeof(string));
            TblAeronave.Columns.Add("Activo", typeof(string));
            TblAeronave.Columns.Add("Descripcion", typeof(string));
            TblAeronave.Columns.Add("HoraVoladaIng", typeof(double));
            TblAeronave.Columns.Add("Usu", typeof(string));
            TblAeronave.Columns.Add("Bloqueada", typeof(int));
            TblAeronave.Columns.Add("Propiedad", typeof(int));
            TblAeronave.Columns.Add("CentroDeCosto", typeof(string));
            TblAeronave.Columns.Add("PropiedadCia", typeof(int));
            TblAeronave.Columns.Add("CSN", typeof(int));
            TblAeronave.Columns.Add("Accion", typeof(string));


            foreach (var Campos in TypAeronave)
            {
                TblAeronave.Rows.Add(new object[]
                {
                Campos.CodAeronave,
                Campos.SN,
                Campos.Matricula,
                Campos.FechaFabricante,
                Campos.FechaIngreso,
                Campos.CodModelo,
                Campos.CodPropietario,
                Campos.CodTipoAeronave,
                Campos.CodProveedor,
                Campos.CodEstadoAeronave,
                Campos.Activo,
                Campos.Descripcion,
                Campos.HoraVoladaIng,
                Campos.Usu,
                Campos.Bloqueada,
                Campos.Propiedad,
                Campos.CentroDeCosto,
                Campos.PropiedadCia,
                Campos.CSN,
                Campos.Accion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = "";
                    PCodHK =0;
                    string VBQuery = "INSERT_UPDATE_TypAeronave";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurHK", TblAeronave);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured; 
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodHK = Convert.ToInt32(SDR["CodHK"].ToString());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmAeronave";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypAeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public int GetCodHK()
        {
            return PCodHK;
        }        
    }
}