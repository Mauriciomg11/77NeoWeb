using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.prgMro
{
    public class CsTypDetallePropuestaSvcManto
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PId;
        public int IdDetPropSrv { get; set; }
        public int IdDetPropHk { get; set; }
        public int IdPropuesta { get; set; }
        public int AprobadoDPSM { get; set; }
        public int IdSvcManto { get; set; }
        public int IdReporte { get; set; }
        public int CodOTPrta { get; set; }
        public string Matricula { get; set; }
        public string CodModeloDPSM { get; set; }
        public string DescricionServicio { get; set; }
        public string Usu { get; set; }
        public string PN { get; set; }
        public string CodReferencia { get; set; }
        public string DescripcionPN { get; set; }
        public string CodContadorDPSM { get; set; }
        public int ReparacionExterna { get; set; }
        public string Accion { get; set; }
        public void Alimentar(IEnumerable<CsTypDetallePropuestaSvcManto> TypDetallePropuestaSvcManto)
        {
            DataTable TblDetallePropuestaSvcManto = new DataTable();
            TblDetallePropuestaSvcManto.Columns.Add("IdDetPropSrv", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("IdDetPropHk", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("IdPropuesta", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("AprobadoDPSM", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("IdSvcManto", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("IdReporte", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("CodOTPrta", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("Matricula", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("CodModeloDPSM", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("DescricionServicio", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("Usu", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("PN", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("CodReferencia", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("DescripcionPN", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("CodContadorDPSM", typeof(string));
            TblDetallePropuestaSvcManto.Columns.Add("ReparacionExterna", typeof(int));
            TblDetallePropuestaSvcManto.Columns.Add("Accion", typeof(string));

            foreach (var Campo in TypDetallePropuestaSvcManto)
            {
                TblDetallePropuestaSvcManto.Rows.Add(new object[]
                {
                    Campo.IdDetPropSrv,
                    Campo.IdDetPropHk,
                    Campo.IdPropuesta,
                    Campo.AprobadoDPSM,
                    Campo.IdSvcManto,
                    Campo.IdReporte,
                    Campo.CodOTPrta,
                    Campo.Matricula,
                    Campo.CodModeloDPSM,
                    Campo.DescricionServicio,
                    Campo.Usu,
                    Campo.PN,
                    Campo.CodReferencia,
                    Campo.DescripcionPN,
                    Campo.CodContadorDPSM,
                    Campo.ReparacionExterna,
                    Campo.Accion,
                });
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = ""; PId = "";
                    string VBQuery = "CRUD_DetallePropuestaSvcManto";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurDetPptSvcs", TblDetallePropuestaSvcManto);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());

                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PId = HttpUtility.HtmlDecode(SDR["IdNew"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmPropuesta";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypDetallePropuestaSvcManto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? 300 : Ex.StackTrace.Length, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj() { return PMensj; }
        public string GetCodPPT() { return PId; }
    }
}