using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class Frm_InfIngenieria : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                MlVw.ActiveViewIndex = 0;
                Page.Title = "Reportes de ingeniería";
                TitForm.Text = "Reportes de ingeniería";
                ModSeguridad();
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1; // Procesos de ingenieria
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), "Frm_InfIngenieria.aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } //
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
            IdiomaControles();
            // PerfilesGrid();
        }
        protected void IdiomaControles()
        {
            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
            {
                string LtxtSql = "EXEC Idioma @I,@F1,@F2,@F3,@F4";
                SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                SC.Parameters.AddWithValue("@I", Session["77IDM"].ToString().Trim());
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"]);
                SC.Parameters.AddWithValue("@F2", "");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("Caption"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }

                    TitForm.Text = bO.Equals("Titulo") ? bT : TitForm.Text;
                    BtnAdvice.ToolTip = bO.Equals("BtnAdviceTT") ? bT : BtnAdvice.ToolTip;
                    BtnInsRemElem.Text = bO.Equals("BtnInsRemElem") ? bT : BtnInsRemElem.Text;
                    BtnInsRemElem.ToolTip = bO.Equals("BtnInsRemElemTT") ? bT : BtnInsRemElem.ToolTip;
                    BtnInsRemSubC.Text = bO.Equals("BtnInsRemSubC") ? bT : BtnInsRemSubC.Text;
                    BtnInsRemSubC.ToolTip = bO.Equals("BtnInsRemSubCTT") ? bT : BtnInsRemSubC.ToolTip;
                    BtnPnPlanti.Text = bO.Equals("BtnPnPlanti") ? bT : BtnPnPlanti.Text;
                    BtnPnPlanti.ToolTip = bO.Equals("BtnPnPlantiTT") ? bT : BtnPnPlanti.ToolTip;
                    BtnHistCont.Text = bO.Equals("BtnHistCont") ? bT : BtnHistCont.Text;
                    BtnHistCont.ToolTip = bO.Equals("BtnHistContTT") ? bT : BtnHistCont.ToolTip;
                    BtnProcIngeni.Text = bO.Equals("BtnProcIngeni") ? bT : BtnProcIngeni.Text;
                    BtnProcIngeni.ToolTip = bO.Equals("BtnProcIngeniTT") ? bT : BtnProcIngeni.ToolTip;
                    BtnProxCump.Text = bO.Equals("BtnProxCump") ? bT : BtnProxCump.Text;
                    BtnProxCump.ToolTip = bO.Equals("BtnProxCumpTT") ? bT : BtnProxCump.ToolTip;
                    BtnCostoOT.Text = bO.Equals("BtnCostoOT") ? bT : BtnCostoOT.Text;
                    BtnCostoOT.ToolTip = bO.Equals("BtnCostoOTTT") ? bT : BtnCostoOT.ToolTip;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BtnAdvice_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmAdvice.aspx"); }
        protected void BtnInsRemElem_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/Inf_FrmComponenteRemovidoDeAeronave.aspx"); }
        protected void BtnInsRemSubC_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmHistoricoSubComponentes.aspx"); }
        protected void BtnHistCont_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmHistoricosContadores.aspx"); }
        protected void BtnPnPlanti_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string VbNomArchivo = "";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurPLantillaMaestraExportar", Session["77IDM"].ToString().Trim());

                string query = "EXEC SP_PANTALLA_Informe_Ingenieria 3,'','','','CurPLantillaMaestraExportar',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                DataRow[] Result = Idioma.Select("Objeto= 'NomExpPM'");
                foreach (DataRow row in Result)
                { VbNomArchivo = row["Texto"].ToString().Trim(); }

                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(query, con))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SC.Connection = con;
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                ds.Tables[0].TableName = "PlMaGrl";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
                                    Response.Charset = "";
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Histórico", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnProcIngeni_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmControlContadoresGeneral.aspx"); }
        protected void BtnProxCump_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmIngProxCumplimiento.aspx"); }
        protected void BtnCostoOT_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmIngCostoOT.aspx"); }
    }
}