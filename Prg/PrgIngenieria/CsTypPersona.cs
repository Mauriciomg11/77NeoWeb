using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypPersona
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PCodPers;
        public string CodPersona { get; set; }
        public string CodEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Registro { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public DateTime Fechanacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string CodArea { get; set; }
        public string CodCargo { get; set; }
        public string NivelTecnico { get; set; }
        public string NumeroLicencia { get; set; }
        public int ValorHoraPer { get; set; }
        public string CodTipoContrPer { get; set; }
        public string CodBase { get; set; }
        public string CodFS { get; set; }
        public string Estado { get; set; }
        public string Pusuario1 { get; set; }
        public string Usu { get; set; }
        public string CorreoCia { get; set; }
        public string HorasTotales { get; set; }
        public string Accion { get; set; }

        public void Alimentar(IEnumerable<CsTypPersona> TypPersona)
        {
            DataTable TblPersona = new DataTable();
            TblPersona.Columns.Add("CodPersona", typeof(string));
            TblPersona.Columns.Add("CodEmpresa", typeof(string));
            TblPersona.Columns.Add("Nombre", typeof(string));
            TblPersona.Columns.Add("Apellido", typeof(string));
            TblPersona.Columns.Add("Registro", typeof(string));
            TblPersona.Columns.Add("Cedula", typeof(string));
            TblPersona.Columns.Add("Telefono", typeof(string));
            TblPersona.Columns.Add("Celular", typeof(string));
            TblPersona.Columns.Add("Correo", typeof(string));
            TblPersona.Columns.Add("Direccion", typeof(string));
            TblPersona.Columns.Add("Fechanacimiento", typeof(DateTime));
            TblPersona.Columns.Add("FechaIngreso", typeof(DateTime));
            TblPersona.Columns.Add("CodArea", typeof(string));
            TblPersona.Columns.Add("CodCargo", typeof(string));
            TblPersona.Columns.Add("NivelTecnico", typeof(string));
            TblPersona.Columns.Add("NumeroLicencia", typeof(string));
            TblPersona.Columns.Add("ValorHoraPer", typeof(int));
            TblPersona.Columns.Add("CodTipoContrPer", typeof(string));
            TblPersona.Columns.Add("CodBase", typeof(string));
            TblPersona.Columns.Add("CodFS", typeof(string));
            TblPersona.Columns.Add("Estado", typeof(string));
            TblPersona.Columns.Add("Pusuario1", typeof(string));
            TblPersona.Columns.Add("Usu", typeof(string));
            TblPersona.Columns.Add("CorreoCia", typeof(string));
            TblPersona.Columns.Add("HorasTotales", typeof(string));
            TblPersona.Columns.Add("Accion", typeof(string));

            foreach (var Campo in TypPersona)
            {
                TblPersona.Rows.Add(new object[]
                {
                    Campo.CodPersona,
                    Campo.CodEmpresa,
                    Campo.Nombre,
                    Campo.Apellido,
                    Campo.Registro,
                    Campo.Cedula,
                    Campo.Telefono,
                    Campo.Celular,
                    Campo.Correo,
                    Campo.Direccion,
                    Campo.Fechanacimiento,
                    Campo.FechaIngreso,
                    Campo.CodArea,
                    Campo.CodCargo,
                    Campo.NivelTecnico,
                    Campo.NumeroLicencia,
                    Campo.ValorHoraPer,
                    Campo.CodTipoContrPer,
                    Campo.CodBase,
                    Campo.CodFS,
                    Campo.Estado,
                    Campo.Pusuario1,
                    Campo.Usu,
                    Campo.CorreoCia,
                    Campo.HorasTotales,
                    Campo.Accion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = ""; PCodPers = "";
                    string VBQuery = "INSERT_UPDATE_Persona";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurPersona", TblPersona);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodPers = HttpUtility.HtmlDecode(SDR["CodPersn"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmPersona";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypPersona", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj() { return PMensj; }
        public string GetCodPersn() { return PCodPers; }
    }
}