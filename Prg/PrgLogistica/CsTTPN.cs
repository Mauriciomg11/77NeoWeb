using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class CsTTPN
    {
        ClsConexion Cnx = new ClsConexion();
        static public string VblAccion;
        static public string VbMensIPN;
        static public string VbMensj;
        public string PN { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionEsp { get; set; }
        public string CodReferencia { get; set; }
        public string CodFabricante { get; set; }
        public string CodUndMed { get; set; }
        public string CodEstadoPn { get; set; }
        public int Bloquear { get; set; }
        public string CodClaseElem { get; set; }
        public string CodTipoElem { get; set; }
        public string IdentificadorElem { get; set; }
        public string CodKit { get; set; }
        public int SubComponente { get; set; }
        public int Consumo { get; set; }
        public int Motor { get; set; }
        public int ComponenteMayor { get; set; }
        public string Codcapitulo { get; set; }
        public string Usu { get; set; }
        public int PosicionPn { get; set; }
        public string UndCompra { get; set; }
        public float Equivalencia { get; set; }
        public string NSN { get; set; }
        public int FechaVencPN { get; set; }
        public void Accion(string VbAccion)
        {
            VblAccion = VbAccion;
        }

        public void Insert(IEnumerable<CsTTPN> TTPN)
        {
            try
            {
                DataTable table = new DataTable();

                table.Columns.Add("PN", typeof(string));
                table.Columns.Add("Descripcion", typeof(string));
                table.Columns.Add("DescripcionEsp", typeof(string));
                table.Columns.Add("CodReferencia", typeof(string));
                table.Columns.Add("CodFabricante", typeof(string));
                table.Columns.Add("CodUndMed", typeof(string));
                table.Columns.Add("CodEstadoPn", typeof(string));
                table.Columns.Add("Bloquear", typeof(int));
                table.Columns.Add("CodClaseElem", typeof(string));
                table.Columns.Add("CodTipoElem", typeof(string));
                table.Columns.Add("IdentificadorElem", typeof(string));
                table.Columns.Add("CodKit", typeof(string));
                table.Columns.Add("SubComponente", typeof(int));
                table.Columns.Add("Consumo", typeof(int));
                table.Columns.Add("Motor", typeof(int));
                table.Columns.Add("ComponenteMayor", typeof(int));
                table.Columns.Add("Codcapitulo", typeof(string));
                table.Columns.Add("Usu", typeof(string));
                table.Columns.Add("PosicionPn", typeof(int));
                table.Columns.Add("UndCompra", typeof(string));
                table.Columns.Add("Equivalencia", typeof(float));
                table.Columns.Add("NSN", typeof(string));
                table.Columns.Add("FechaVencPN", typeof(int));

                foreach (var Campos in TTPN)
                {
                    table.Rows.Add(new object[]
                        {

                        Campos.PN,
                        Campos.Descripcion,
                        Campos.DescripcionEsp,
                        Campos.CodReferencia,
                        Campos.CodFabricante,
                        Campos.CodUndMed,
                        Campos.CodEstadoPn,
                        Campos.Bloquear,
                        Campos.CodClaseElem,
                        Campos.CodTipoElem,
                        Campos.IdentificadorElem,
                        Campos.CodKit,
                        Campos.SubComponente,
                        Campos.Consumo,
                        Campos.Motor,
                        Campos.ComponenteMayor,
                        Campos.Codcapitulo,
                        Campos.Usu,
                        Campos.PosicionPn,
                        Campos.UndCompra,
                        Campos.Equivalencia,
                        Campos.NSN,
                        Campos.FechaVencPN,
                        });
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction transaction = sqlCon.BeginTransaction())
                    {
                        string VBQuery;
                        if (VblAccion == "INSERT")
                        {
                            VBQuery = "SP_Insert_PN";
                        }
                        else
                        { VBQuery = "SP_UPDATE_PN"; }

                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                        {
                            try
                            {
                                VbMensIPN = "";
                                VbMensj = "";
                                sqlCmd.CommandType = CommandType.StoredProcedure;
                                SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurPN", table);
                                SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                                Prmtrs.SqlDbType = SqlDbType.Structured;

                                SqlDataReader SDR = sqlCmd.ExecuteReader();
                                if (SDR.Read())
                                {
                                    VbMensIPN = HttpUtility.HtmlDecode(SDR["Plano"].ToString().Trim());
                                    VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string mensjaes = Ex.Message;
            }
        }
        public string GetPlano()
        {
            return VbMensIPN;
        }
        public string GetMensj()
        {
            return VbMensj;
        }
    }
}