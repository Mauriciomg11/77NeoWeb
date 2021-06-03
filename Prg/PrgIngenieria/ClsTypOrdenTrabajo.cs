using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class ClsTypOrdenTrabajo
    {
        static public string PMensj;
        static public string PMensjAlterno;
        //-------------  Trabajo --------------------
        public int CodNumOrdenTrab { get; set; }
        public string Descripcion { get; set; }
        public string CodEstOrdTrab1 { get; set; }
        public string CodEstOrdTrab2 { get; set; }
        public string Aplicabilidad { get; set; }
        public string CodCapitulo { get; set; }
        public string CodUbicaTecn { get; set; }
        public string CodBase { get; set; }
        public string CodTaller { get; set; }
        public string CodPlanManto { get; set; }
        public string CentroCosto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public DateTime? FechaReg { get; set; }
        public int IdentificadorCorrPrev { get; set; }
        public string CodPrioridad { get; set; }
        public int CodIdLvDetManto { get; set; }
        public int CodIdDetSrvManto { get; set; }
        public int BanCerrado { get; set; }
        public int HorasProyectadas { get; set; }
        public DateTime? FechaProyectada { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string UsuOT { get; set; }
        public string Referencia { get; set; }
        public string AccionParcial { get; set; }
        public string CodTipoCodigo { get; set; }
        public string CodInspectorCierre { get; set; }
        public string LicenciaInspCierre { get; set; }
        public string PNOT { get; set; }
        public int BloquearDetalle { get; set; }
        public string CodResponsable { get; set; }
        public double OTSN { get; set; }
        public double OTSO { get; set; }
        public double OTSR { get; set; }
        public double OCSN { get; set; }
        public double OCSO { get; set; }
        public double OCSR { get; set; }
        public int EjecPasos { get; set; }
        public int CancelOT { get; set; }
        public string WS { get;  set; }
        public int OKOT { get; set; }
        public string AccionOT { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypOrdenTrabajo> OrdenTrabajo)
        {  
            DataTable TblORdenTrabajo = new DataTable();
            TblORdenTrabajo.Columns.Add("CodNumOrdenTrab", typeof(int));
            TblORdenTrabajo.Columns.Add("Descripcion", typeof(string));
            TblORdenTrabajo.Columns.Add("CodEstOrdTrab1", typeof(string));
            TblORdenTrabajo.Columns.Add("CodEstOrdTrab2", typeof(string));
            TblORdenTrabajo.Columns.Add("Aplicabilidad", typeof(string));
            TblORdenTrabajo.Columns.Add("CodCapitulo", typeof(string));
            TblORdenTrabajo.Columns.Add("CodUbicaTecn", typeof(string));
            TblORdenTrabajo.Columns.Add("CodBase", typeof(string));
            TblORdenTrabajo.Columns.Add("CodTaller", typeof(string));
            TblORdenTrabajo.Columns.Add("CodPlanManto", typeof(string));
            TblORdenTrabajo.Columns.Add("CentroCosto", typeof(string));
            TblORdenTrabajo.Columns.Add("FechaInicio", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaFinal", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaReg", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("IdentificadorCorrPrev", typeof(int));
            TblORdenTrabajo.Columns.Add("CodPrioridad", typeof(string));
            TblORdenTrabajo.Columns.Add("CodIdLvDetManto", typeof(int));
            TblORdenTrabajo.Columns.Add("CodIdDetSrvManto", typeof(int));
            TblORdenTrabajo.Columns.Add("BanCerrado", typeof(int));
            TblORdenTrabajo.Columns.Add("HorasProyectadas", typeof(int));
            TblORdenTrabajo.Columns.Add("FechaProyectada", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaVencimiento", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("UsuOT", typeof(string));
            TblORdenTrabajo.Columns.Add("Referencia", typeof(string));
            TblORdenTrabajo.Columns.Add("AccionParcial", typeof(string));
            TblORdenTrabajo.Columns.Add("CodTipoCodigo", typeof(string));
            TblORdenTrabajo.Columns.Add("CodInspectorCierre", typeof(string));
            TblORdenTrabajo.Columns.Add("LicenciaInspCierre", typeof(string));
            TblORdenTrabajo.Columns.Add("PNOT", typeof(string));
            TblORdenTrabajo.Columns.Add("BloquearDetalle", typeof(int));
            TblORdenTrabajo.Columns.Add("CodResponsable", typeof(string));
            TblORdenTrabajo.Columns.Add("OTSN", typeof(double));
            TblORdenTrabajo.Columns.Add("OTSO", typeof(double));
            TblORdenTrabajo.Columns.Add("OTSR", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSN", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSO", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSR", typeof(double));
            TblORdenTrabajo.Columns.Add("EjecPasos", typeof(int));
            TblORdenTrabajo.Columns.Add("CancelOT", typeof(int));
            TblORdenTrabajo.Columns.Add("WS", typeof(string));
            TblORdenTrabajo.Columns.Add("OKOT", typeof(int));
            TblORdenTrabajo.Columns.Add("AccionOT", typeof(string));
            foreach (var CampoOT in OrdenTrabajo)
            {
                TblORdenTrabajo.Rows.Add(new object[] {
                    CampoOT.CodNumOrdenTrab,
                    CampoOT.Descripcion,
                    CampoOT.CodEstOrdTrab1,
                    CampoOT.CodEstOrdTrab2,
                    CampoOT.Aplicabilidad,
                    CampoOT.CodCapitulo,
                    CampoOT.CodUbicaTecn,
                    CampoOT.CodBase,
                    CampoOT.CodTaller,
                    CampoOT.CodPlanManto,
                    CampoOT.CentroCosto,
                    CampoOT.FechaInicio,
                    CampoOT.FechaFinal,
                    CampoOT.FechaReg,
                    CampoOT.IdentificadorCorrPrev,
                    CampoOT.CodPrioridad,
                    CampoOT.CodIdLvDetManto,
                    CampoOT.CodIdDetSrvManto,
                    CampoOT.BanCerrado,
                    CampoOT.HorasProyectadas,
                    CampoOT.FechaProyectada,
                    CampoOT.FechaVencimiento,
                    CampoOT.UsuOT,
                    CampoOT.Referencia,
                    CampoOT.AccionParcial,
                    CampoOT.CodTipoCodigo,
                    CampoOT.CodInspectorCierre,
                    CampoOT.LicenciaInspCierre,
                    CampoOT.PNOT,
                    CampoOT.BloquearDetalle,
                    CampoOT.CodResponsable,
                    CampoOT.OTSN,
                    CampoOT.OTSO,
                    CampoOT.OTSR,
                    CampoOT.OCSN,
                    CampoOT.OCSO,
                    CampoOT.OCSR,
                    CampoOT.EjecPasos,
                    CampoOT.CancelOT,
                    CampoOT.WS,
                    CampoOT.OKOT,
                    CampoOT.AccionOT,
                });
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "UPD_OrdenTrabajo";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        PMensj = "";
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;                              
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurOT", TblORdenTrabajo);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;                          
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PMensjAlterno = SDR["MensAlterno"].ToString().Trim(); 
                                string borrar = SDR["MensjcumplirSvc"].ToString().Trim();
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmOrdenTrabajo";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypOrdenTrabajo", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetMensjAlterno()
        {
            return PMensjAlterno;
        }
    }
}