using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg
{
    public class CsTypSubirReserva
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        public int IdRsva { get; set; }
        public int Posicion { get; set; }
        public string PN { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public string UndSolicitada { get; set; }
        public string UndSistema { get; set; }
        public string IPC { get; set; }
        public string Usu { get; set; }
        public int CodAeronave { get; set; }
        public string ProcesoOrigen { get; set; }
        public string Accion { get; set; }
        public void Alimentar(IEnumerable<CsTypSubirReserva> Rsva)
        {
            DataTable TblRsva = new DataTable();

            TblRsva.Columns.Add("IdRsva", typeof(int));
            TblRsva.Columns.Add("Posicion", typeof(int));
            TblRsva.Columns.Add("PN", typeof(string));
            TblRsva.Columns.Add("Descripcion", typeof(string));
            TblRsva.Columns.Add("Cantidad", typeof(double));
            TblRsva.Columns.Add("UndSolicitada", typeof(string));
            TblRsva.Columns.Add("UndSistema", typeof(string));
            TblRsva.Columns.Add("IPC", typeof(string));
            TblRsva.Columns.Add("Usu", typeof(string));
            TblRsva.Columns.Add("CodAeronave", typeof(int));
            TblRsva.Columns.Add("ProcesoOrigen", typeof(string));
            TblRsva.Columns.Add("Accion", typeof(string));
            foreach (var Campos in Rsva)
            {
                TblRsva.Rows.Add(new object[] {
                Campos.IdRsva,
                Campos.Posicion,
                Campos.PN,
                Campos.Descripcion,
                Campos.Cantidad,
                Campos.UndSolicitada,
                Campos.UndSistema,
                Campos.IPC,
                Campos.Usu,
                Campos.CodAeronave,
                Campos.ProcesoOrigen,
                Campos.Accion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VBQuery = "SP_SubirReservaMaxiva";
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            PMensj = "OK";
                            DataTable TbResultado = new DataTable();
                            TbResultado.Columns.Add("Posicion", typeof(int));
                            TbResultado.Columns.Add("PN", typeof(string));
                            TbResultado.Columns.Add("Descripcion", typeof(string));
                            TbResultado.Columns.Add("Cantidad", typeof(double));
                            TbResultado.Columns.Add("UndDespacho", typeof(string));
                            TbResultado.Columns.Add("UndSistema", typeof(string));
                            TbResultado.Columns.Add("IPC", typeof(string));
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurSubRvaMax", TblRsva);
                            SqlParameter Prmtrs1 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            while (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                int VbPos = Convert.ToInt32(SDR["POS"].ToString().Trim());
                                string VbPn = SDR["PN"].ToString().Trim();
                                string VbDesc = SDR["Descripcion"].ToString().Trim();
                                double VbCant = Convert.ToDouble(SDR["Cantidad"].ToString());
                                string VbUndSol = SDR["UndSolicitada"].ToString().Trim();
                                string VbUndSys = SDR["CodUndMed"].ToString().Trim();
                                string VbIPC = SDR["IPC"].ToString().Trim();
                                TbResultado.Rows.Add(VbPos, VbPn, VbDesc, VbCant, VbUndSol, VbUndSys, VbIPC);
                            }
                            System.Web.HttpContext.Current.Session["TablaRsvaResul"] = TbResultado;
                            SDR.Close();
                            Transac.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmServicioManto";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypSubirReserva", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }

        }
        public string GetMensj()
        {
            return PMensj;
        }
    }
}