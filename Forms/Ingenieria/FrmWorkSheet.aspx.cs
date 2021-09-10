using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmWorkSheet : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DST = new DataSet();
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
                    Session["C77U"] = "00000082"; //00000082|00000133
                    Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = "77NEO01";
                    Session["V$U@"] = "sa";
                    Session["P@$"] = "admindemp";
                    Session["N77U"] = Session["D[BX"];
                     Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                MlVw.ActiveViewIndex = 0;
                ModSeguridad();
                BindBDdlAK();
                ViewState["PageTit"] = "Work Sheet";
                Page.Title = ViewState["PageTit"].ToString();
                TitForm.Text = "Work Sheet";
                PerfilesGrid();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnWSNew.Visible = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
            IdiomaControles();
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdWSAbiertas.Rows)
            {
                if ((int)ViewState["VblImpMS"] == 0)
                {
                    ImageButton IbtImprSvc = Row.FindControl("IbtPrintOT") as ImageButton;
                    if (IbtImprSvc != null)
                    { IbtImprSvc.Visible = false; }

                    ImageButton IbtPrintRecu = Row.FindControl("IbtPrintRecu") as ImageButton;
                    if (IbtPrintRecu != null)
                    { IbtPrintRecu.Visible = false; }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton IbtDelete = Row.FindControl("IbtDelete") as ImageButton;
                    if (IbtDelete != null)
                    { IbtDelete.Visible = false; }
                }
            }
            foreach (GridViewRow Row in GrdOTRteWS.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton IbtEdit = Row.FindControl("IbtEdit") as ImageButton;
                    if (IbtEdit != null)
                    { IbtEdit.Visible = false; }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton IbtDelete = Row.FindControl("IbtDelete") as ImageButton;
                    if (IbtDelete != null)
                    { IbtDelete.Visible = false; }
                }
            }
            foreach (GridViewRow Row in GrdReportes.Rows)
            {
                if ((int)ViewState["VblIngMS"] == 0)
                {
                    ImageButton imgAsig = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgAsig != null)
                    { imgAsig.Visible = false; }
                }
            }
            foreach (GridViewRow Row in GrdServicios.Rows)
            {
                if ((int)ViewState["VblIngMS"] == 0)
                {
                    ImageButton imgAsig = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgAsig != null)
                    { imgAsig.Visible = false; }
                }
            }
        }
        protected void IdiomaControles()
        {
            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
            {
                string LtxtSql = "EXEC Idioma @I,@F1,@F2,@F3,@F4";
                SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                SC.Parameters.AddWithValue("@I", Session["77IDM"].ToString().Trim());
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"].ToString().Trim());
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
                    LblStsHK.Text = bO.Equals("LblStsHK") ? bT : LblStsHK.Text;
                    BtnWSNew.Text = bO.Equals("BtnWSNew") ? bT : BtnWSNew.Text;
                    BtnWSNew.ToolTip = bO.Equals("BtnWSNewTT") ? bT : LblStsHK.ToolTip;
                    BtnWSProces.Text = bO.Equals("BtnWSProces") ? bT : BtnWSProces.Text;
                    BtnWSProces.ToolTip = bO.Equals("BtnWSProcesTT") ? bT : BtnWSProces.ToolTip;
                    LblTitWSOpen.Text = bO.Equals("LblTitWSOpen") ? bT : LblTitWSOpen.Text;
                    LblTitWsBusq.Text = bO.Equals("LblTitWsBusq") ? bT : LblTitWsBusq.Text;
                    GrdWSAbiertas.Columns[1].HeaderText = bO.Equals("NroWS") ? bT : GrdWSAbiertas.Columns[1].HeaderText;
                    GrdWSAbiertas.Columns[2].HeaderText = bO.Equals("GrdGenerada") ? bT : GrdWSAbiertas.Columns[2].HeaderText;
                    GrdWSAbiertas.Columns[3].HeaderText = bO.Equals("GrdVence") ? bT : GrdWSAbiertas.Columns[3].HeaderText;
                    GrdWSAbiertas.Columns[4].HeaderText = bO.Equals("GrdAvance") ? bT : GrdWSAbiertas.Columns[4].HeaderText;
                    if (bO.Equals("placeholderBq"))
                    {
                        TxtWSBusq.Attributes.Add("placeholder", bT);
                    }
                    IbtSWConsultar.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtSWConsultar.ToolTip;
                    GrdWSBusq.Columns[1].HeaderText = bO.Equals("NroWS") ? bT : GrdWSBusq.Columns[1].HeaderText;
                    GrdWSBusq.Columns[2].HeaderText = bO.Equals("LblStsHK") ? bT : GrdWSBusq.Columns[2].HeaderText;
                    GrdWSBusq.Columns[3].HeaderText = bO.Equals("GrdEstado") ? bT : GrdWSBusq.Columns[3].HeaderText;
                    GrdWSBusq.Columns[4].HeaderText = bO.Equals("GrdGenerada") ? bT : GrdWSBusq.Columns[4].HeaderText;
                    //**************************************Asignar OT RTE**********************************************
                    LblTitAsigOTaWS.Text = bO.Equals("LblTitAsigOTaWS") ? bT : LblTitAsigOTaWS.Text;
                    LblTitAsigOTaWS.Text = bO.Equals("LblTitAsigOTaWS") ? bT : LblTitAsigOTaWS.Text;
                    LblAsigOTHK.Text = bO.Equals("LblStsHK") ? bT : LblStsHK.Text;
                    IbtCerrarAsigOT.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsigOT.ToolTip;
                    RdbAsigOT.Text = bO.Equals("RdbAsigOT") ? bT : RdbAsigOT.Text;
                    RdbAsigRte.Text = bO.Equals("RdbAsigRte") ? bT : RdbAsigRte.Text;
                    if (bO.Equals("placeholderDC"))
                    {
                        TxtAsigOT_RTE.Attributes.Add("placeholder", bT);
                    }
                    IbtAsigOTBusq.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtAsigOTBusq.ToolTip;
                    GrdServicios.Columns[0].HeaderText = bO.Equals("GrdSvcs") ? bT : GrdServicios.Columns[0].HeaderText;
                    GrdServicios.Columns[3].HeaderText = bO.Equals("RdbAsigOT") ? bT : GrdServicios.Columns[3].HeaderText;
                    GrdServicios.Columns[4].HeaderText = bO.Equals("GrdProyec") ? bT : GrdServicios.Columns[4].HeaderText;
                    GrdReportes.Columns[0].HeaderText = bO.Equals("GrdDescRte") ? bT : GrdReportes.Columns[0].HeaderText;
                    GrdReportes.Columns[2].HeaderText = bO.Equals("GrdSnHK") ? bT : GrdReportes.Columns[2].HeaderText;
                    GrdReportes.Columns[3].HeaderText = bO.Equals("RdbAsigRte") ? bT : GrdReportes.Columns[3].HeaderText;
                    GrdReportes.Columns[4].HeaderText = bO.Equals("GrdProyec") ? bT : GrdReportes.Columns[4].HeaderText;
                    lblTitOTWS.Text = bO.Equals("lblTitOTWS") ? bT : lblTitOTWS.Text;
                    GrdOTRteWS.Columns[0].HeaderText = bO.Equals("GrdPpl") ? bT : GrdOTRteWS.Columns[0].HeaderText;
                    GrdOTRteWS.Columns[1].HeaderText = bO.Equals("GrdTrab") ? bT : GrdOTRteWS.Columns[1].HeaderText;
                    GrdOTRteWS.Columns[2].HeaderText = bO.Equals("GrdFrec") ? bT : GrdOTRteWS.Columns[2].HeaderText;
                    GrdOTRteWS.Columns[3].HeaderText = bO.Equals("GrdDias") ? bT : GrdOTRteWS.Columns[3].HeaderText;
                    GrdOTRteWS.Columns[6].HeaderText = bO.Equals("GrdOTRTE") ? bT : GrdOTRteWS.Columns[6].HeaderText;
                    GrdOTRteWS.Columns[7].HeaderText = bO.Equals("GrdEstado") ? bT : GrdOTRteWS.Columns[7].HeaderText;
                    GrdOTRteWS.Columns[8].HeaderText = bO.Equals("GrdVence") ? bT : GrdOTRteWS.Columns[8].HeaderText;
                    GrdOTRteWS.Columns[9].HeaderText = bO.Equals("GrdProyec") ? bT : GrdOTRteWS.Columns[9].HeaderText;
                    LblTitImpresion.Text = bO.Equals("LblTitImpresion") ? bT : LblTitImpresion.Text;
                    IbtCerrarImpresion.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpresion.ToolTip;
                    ViewState["RteWSAK"] = bO.Equals("LblStsHK") ? bT : ViewState["RteWSAK"];
                    ViewState["RteModel"] = bO.Equals("RteModel") ? bT : ViewState["RteModel"];
                    ViewState["RteFecha"] = bO.Equals("GrdGenerada") ? bT : ViewState["RteFecha"];
                    ViewState["RteVence"] = bO.Equals("GrdVence") ? bT : ViewState["RteVence"];
                    ViewState["RteElabora"] = bO.Equals("RteElabora") ? bT : ViewState["RteElabora"];
                    ViewState["RteFirma"] = bO.Equals("RteFirma") ? bT : ViewState["RteFirma"];
                    ViewState["RtePag"] = bO.Equals("RtePag") ? bT : ViewState["RtePag"];
                    ViewState["RteDe"] = bO.Equals("RteDe") ? bT : ViewState["RteDe"];
                    ViewState["RteDocumen"] = bO.Equals("RteDocumen") ? bT : ViewState["RteDocumen"];
                    ViewState["RteDescr"] = bO.Equals("RteDescr") ? bT : ViewState["RteDescr"];
                    ViewState["RteNroDoc"] = bO.Equals("RteNroDoc") ? bT : ViewState["RteNroDoc"];
                    ViewState["RteCumpl"] = bO.Equals("RteCumpl") ? bT : ViewState["RteCumpl"];
                    ViewState["RteFecCum"] = bO.Equals("RteFecCum") ? bT : ViewState["RteFecCum"];
                    ViewState["RteTecnL"] = bO.Equals("RteTecnL") ? bT : ViewState["RteTecnL"];
                    ViewState["RteCodRef"] = bO.Equals("RteCodRef") ? bT : ViewState["RteCodRef"];
                    ViewState["RteUndMed"] = bO.Equals("RteCantSol") ? bT : ViewState["RteCantSol"];
                    ViewState["RteCantSol"] = bO.Equals("RteUndMed") ? bT : ViewState["RteUndMed"];
                    ViewState["RteUndMed"] = bO.Equals("RteUndMed") ? bT : ViewState["RteUndMed"];
                    ViewState["RteDispon"] = bO.Equals("RteDispon") ? bT : ViewState["RteDispon"];
                    ViewState["RteCantEntr"] = bO.Equals("RteCantEntr") ? bT : ViewState["RteCantEntr"];
                    ViewState["RteCantDev"] = bO.Equals("RteCantDev") ? bT : ViewState["RteCantDev"];
                    ViewState["RteFecha"] = bO.Equals("RteFecha") ? bT : ViewState["RteFecha"];
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdlAK()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Status 11,'','','WS','HK',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlWSHK.DataSource = Cnx.DSET(LtxtSql);
            DdlWSHK.DataTextField = "Matricula";
            DdlWSHK.DataValueField = "CodAeronave";
            DdlWSHK.DataBind();
        }
        protected void Traerdatos(string Prmtr)
        {

            ViewState["UltFechaProces"] = DateTime.Today.ToString("dd-MM-yyyy");
            ViewState["HrasProm"] = "0";
            ViewState["CclProm"] = "0";
            ViewState["APUsProm"] = "0";/**/

            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format(" EXEC SP_PANTALLA_Status 12,'','','','',@Prmtr,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    ViewState["UltFechaProces"] = HttpUtility.HtmlDecode(SDR["UltFechaProces"].ToString().Trim());
                    ViewState["HrasProm"] = HttpUtility.HtmlDecode(SDR["HrasProm"].ToString().Trim());
                    ViewState["CclProm"] = HttpUtility.HtmlDecode(SDR["CclProm"].ToString().Trim());
                    ViewState["APUsProm"] = HttpUtility.HtmlDecode(SDR["APUsProm"].ToString().Trim()); /**/
                }
                if (ViewState["UltFechaProces"].ToString().Equals("")) { ViewState["UltFechaProces"] = DateTime.Today.ToString("dd-MM-yyyy"); }
                if (ViewState["HrasProm"].ToString().Equals("")) { ViewState["HrasProm"] = "0"; }
                if (ViewState["CclProm"].ToString().Equals("")) { ViewState["CclProm"] = "0"; }
                if (ViewState["APUsProm"].ToString().Equals("")) { ViewState["APUsProm"] = "0"; }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void BtnWSNew_Click(object sender, EventArgs e)
        {
            if (!DdlWSHK.Text.Equals("0")) { MlVw.ActiveViewIndex = 1; DatosAsignarOT("", "0001"); BIndDSvcSinAsingar(); }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnWSProces_Click(object sender, EventArgs e)
        {
            if (!DdlWSHK.Text.Equals("0"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Traerdatos(DdlWSHK.Text);
                Page.Title = ViewState["PageTit"].ToString();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_StatusReport @HK,@Fec,'NO',@HV,@CV,@APU,@Usu, @ICC";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                SC.Parameters.AddWithValue("@Fec", ViewState["UltFechaProces"].ToString());
                                SC.Parameters.AddWithValue("@HV", ViewState["HrasProm"]);
                                SC.Parameters.AddWithValue("@CV", ViewState["CclProm"]);
                                SC.Parameters.AddWithValue("@APU", ViewState["APUsProm"]);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit(); DataRow[] Result = Idioma.Select("Objeto= 'MstrMens02'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplWS, UplWS.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Proceso finalizado correctamente');", true);
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensjWS01'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplWS, UplWS.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Error en el proceso de actualizar el status", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        //*************************************************  WORK SHEET ABIERTAS  *************************************************
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BIndDWSAOpen(string Accion)
        {
            PerfilesGrid();
            if (Accion.Equals("UPD"))
            {
                DataTable DtB = new DataTable();
                Page.Title = ViewState["PageTit"].ToString();
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_WorkSheet 15,'','','','',@Prmtr,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Prmtr", DdlWSHK.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DST = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DST);
                                DST.Tables[0].TableName = "WSAbiertas";
                                DST.Tables[1].TableName = "OTAsignadas";
                                DST.Tables[2].TableName = "SvcSinAsingr";
                                DST.Tables[3].TableName = "RtesSinAsingr";

                                ViewState["DST"] = DST;
                            }
                        }
                    }
                }
            }
            DST = (DataSet)ViewState["DST"];
            if (DST.Tables[0].Rows.Count > 0)//WS Abiertas
            { GrdWSAbiertas.DataSource = DST.Tables[0]; GrdWSAbiertas.DataBind(); }
            else
            { GrdWSAbiertas.DataSource = null; GrdWSAbiertas.DataBind(); }
        }
        protected void DdlWSHK_TextChanged(object sender, EventArgs e)
        {
            if (!DdlWSHK.Text.Equals("0")) { BIndDWSAOpen("UPD"); }//DatosAsignarOT("", "0001");
        }
        protected void GrdWSAbiertas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString();
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int rowIndex = row.RowIndex;
            string vbcod = ((Label)row.FindControl("LblWS")).Text.ToString().Trim();
            if (e.CommandName.Equals("Abrir"))
            { DatosAsignarOT(vbcod, "0001"); BIndDWSOTRTE(); BIndDSvcSinAsingar(); MlVw.ActiveViewIndex = 1; }
            if (e.CommandName.Equals("PrintWSTrab"))// imprimir servicios
            {
                MlVw.ActiveViewIndex = 2;
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[17];

                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("RteWSAK", ViewState["RteWSAK"].ToString());
                    parameters[4] = new ReportParameter("RteModel", ViewState["RteModel"].ToString());
                    parameters[5] = new ReportParameter("RteFecha", ViewState["RteFecha"].ToString());
                    parameters[6] = new ReportParameter("RteVence", ViewState["RteVence"].ToString());
                    parameters[7] = new ReportParameter("RteElabora", ViewState["RteElabora"].ToString());
                    parameters[8] = new ReportParameter("RteFirma", ViewState["RteFirma"].ToString());
                    parameters[9] = new ReportParameter("RtePag", ViewState["RtePag"].ToString());
                    parameters[10] = new ReportParameter("RteDe", ViewState["RteDe"].ToString());
                    parameters[11] = new ReportParameter("RteDocumen", ViewState["RteDocumen"].ToString());
                    parameters[12] = new ReportParameter("RteDescr", ViewState["RteDescr"].ToString());
                    parameters[13] = new ReportParameter("RteNroDoc", ViewState["RteNroDoc"].ToString());
                    parameters[14] = new ReportParameter("RteCumpl", ViewState["RteCumpl"].ToString());
                    parameters[15] = new ReportParameter("RteFecCum", ViewState["RteFecCum"].ToString());
                    parameters[16] = new ReportParameter("RteTecnL", ViewState["RteTecnL"].ToString());

                    string StSql = "EXEC SP_PANTALLA_WorkSheet 1,@NWS,'','',@Nt,0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900' ";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {
                        SC.Parameters.AddWithValue("@NWS", vbcod);
                        SC.Parameters.AddWithValue("@Nt", Session["Nit77Cia"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RpV.LocalReport.EnableExternalImages = true;
                            RpV.LocalReport.ReportPath = "Report/Ing/WSTrabajos.rdlc";
                            RpV.LocalReport.DataSources.Clear();
                            RpV.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RpV.LocalReport.SetParameters(parameters);
                            RpV.LocalReport.Refresh();
                        }
                    }
                }
            }
            if (e.CommandName.Equals("PrintWSRecur"))// imprimir recurso
            {
                MlVw.ActiveViewIndex = 2;
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[15];

                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("RteWSAK", ViewState["RteWSAK"].ToString());
                    parameters[4] = new ReportParameter("RteModel", ViewState["RteModel"].ToString());
                    parameters[5] = new ReportParameter("RteVence", ViewState["RteVence"].ToString());
                    parameters[6] = new ReportParameter("RtePag", ViewState["RtePag"].ToString());
                    parameters[7] = new ReportParameter("RteDe", ViewState["RteDe"].ToString());
                    parameters[8] = new ReportParameter("RteCodRef", ViewState["RteCodRef"].ToString());
                    parameters[9] = new ReportParameter("RteCantSol", ViewState["RteCantSol"].ToString());
                    parameters[10] = new ReportParameter("RteUndMed", ViewState["RteUndMed"].ToString());
                    parameters[11] = new ReportParameter("RteDispon", ViewState["RteDispon"].ToString());
                    parameters[12] = new ReportParameter("RteCantEntr", ViewState["RteCantEntr"].ToString());
                    parameters[13] = new ReportParameter("RteCantDev", ViewState["RteCantDev"].ToString());
                    parameters[14] = new ReportParameter("RteFecha", ViewState["RteFecha"].ToString());

                    using (SqlCommand SC = new SqlCommand("EXEC  SP_TallyDos @NWS, @Nt, @ICC", SCnx1))
                    {
                        SC.Parameters.AddWithValue("@NWS", vbcod);
                        SC.Parameters.AddWithValue("@Nt", Session["Nit77Cia"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RpV.LocalReport.EnableExternalImages = true;
                            RpV.LocalReport.ReportPath = "Report/Ing/WSRecurso.rdlc";
                            RpV.LocalReport.DataSources.Clear();
                            RpV.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RpV.LocalReport.SetParameters(parameters);
                            RpV.LocalReport.Refresh();
                        }
                    }
                }
            }
        }
        protected void GrdWSAbiertas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlWSHK.Text.Trim().Equals("0"))
                { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_PANTALLA_WorkSheet 14,@WS,@Usu,'','WEB',@HK,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string VbWS = (GrdWSAbiertas.Rows[e.RowIndex].FindControl("LblWS") as Label).Text;
                                SC.Parameters.AddWithValue("@WS", VbWS);
                                SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                string Mensj = "";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensjWS02'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplWS, UplWS.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Tiene trabajos cumplidos, no es posible eliminar el documento.
                                }
                                GrdOTRteWS.EditIndex = -1;
                                BIndDWSAOpen("UPD");

                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplWS, UplWS.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Eliminar Work Sheet", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdWSAbiertas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtAbrir'");
                ImageButton IbtAbrir = (e.Row.FindControl("IbtAbrir") as ImageButton);
                if (IbtAbrir != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtPrintOT = (e.Row.FindControl("IbtPrintOT") as ImageButton);
                if (IbtPrintOT != null)
                {
                    Result = Idioma.Select("Objeto='IbtPrintOT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtPrintOT.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtPrintRecu = (e.Row.FindControl("IbtPrintRecu") as ImageButton);
                if (IbtPrintRecu != null)
                {
                    Result = Idioma.Select("Objeto='IbtPrintRecu'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtPrintRecu.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    Result = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }

            }
        }
        //*************************************************  BUSQUEDA  *************************************************
        protected void BIndDBusqWS()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurGridWS", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "EXEC SP_PANTALLA_WorkSheet 12,@Prmtr,'','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtWSBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdWSBusq.DataSource = DtB;
                            GrdWSBusq.DataBind();
                        }
                        else
                        {
                            GrdWSBusq.DataSource = null;
                            GrdWSBusq.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtSWConsultar_Click(object sender, ImageClickEventArgs e)
        { BIndDBusqWS(); }
        protected void GrdWSBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Abrir"))
            {
                MlVw.ActiveViewIndex = 1;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                string vbcod = ((Label)row.FindControl("LblWS")).Text.ToString().Trim();
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbIdx = GrdWSBusq.DataKeys[gvr.RowIndex].Values["Estado"].ToString();
                DdlWSHK.Text = GrdWSBusq.DataKeys[gvr.RowIndex].Values["CodHKWS"].ToString();
                BIndDWSAOpen("UPD");
                DatosAsignarOT(vbcod, VbIdx);
                BIndDWSOTRTE();
                BIndDSvcSinAsingar();
            }
        }
        protected void GrdWSBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtAbrir'");
                ImageButton IbtAbrir2 = (e.Row.FindControl("IbtAbrir2") as ImageButton);
                if (IbtAbrir2 != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir2.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //*************************************************  Asignar OT a la WS  *************************************************
        protected void DatosAsignarOT(string WS, string Estado)
        { TxtAsigOTHK.Text = DdlWSHK.SelectedItem.Text.Trim(); TxtAsingOTWS.Text = WS; RdbAsigOT.Checked = true; }
        protected void BIndDSvcSinAsingar()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DST = (DataSet)ViewState["DST"];

            DataRow[] Result = Idioma.Select("Objeto= 'LblTitServiciosOT'");
            foreach (DataRow row in Result) { LblTitServicios.Text = row["Texto"].ToString(); }
            GrdServicios.Visible = true;

            DataTable DT = new DataTable();
            DT = DST.Tables[2].Clone();
            DataRow[] DR = DST.Tables[2].Select("CodHKRva = " + DdlWSHK.Text.Trim() + " AND Orden LIKE '%" + TxtAsigOT_RTE.Text.Trim() + "%'");
            if (IsIENumerableLleno(DR)) { DT = DR.CopyToDataTable(); }

            if (DT.Rows.Count > 0) { GrdServicios.DataSource = DT; GrdServicios.DataBind(); }
            else { GrdServicios.DataSource = null; GrdServicios.DataBind(); }
        }
        protected void IbtCerrarAsigOT_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; GrdServicios.Visible = false; GrdOTRteWS.Visible = false; Page.Title = ViewState["PageTit"].ToString(); }
        protected void IbtAsigOTBusq_Click(object sender, ImageClickEventArgs e)
        {
            if (RdbAsigOT.Checked == true) { BIndDSvcSinAsingar(); }
            else { BIndDRtesSinAsingar(); }
            BIndDWSOTRTE();
        }
        protected void RdbAsigOT_CheckedChanged(object sender, EventArgs e)
        { BIndDWSAOpen("SEL"); BIndDSvcSinAsingar(); GrdServicios.Visible = true; GrdReportes.Visible = false; }
        protected void GrdServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblOT = "";
            try
            {
                if (e.CommandName.Equals("Asignar"))
                {
                    if (DdlWSHK.Text.Trim().Equals("0")) { return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VBQuery = string.Format("INSERT_UPDATE_WORK_SHEET_WS @WS,@HK,@OT,0,@Usu,NULL,0,'INSERT', @ICC");

                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                                    int rowIndex = row.RowIndex;
                                    VblOT = ((Label)row.FindControl("LblOT")).Text.ToString().Trim();
                                    SC.Parameters.AddWithValue("@WS", TxtAsingOTWS.Text.Trim());
                                    SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                    SC.Parameters.AddWithValue("@OT", VblOT);
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    string Mensj = "", VbNumWS = "";
                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        VbNumWS = HttpUtility.HtmlDecode(SDR["NumWS"].ToString().Trim());
                                    }
                                    SDR.Close();
                                    Transac.Commit();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row1 in Result)
                                        { Mensj = row1["Texto"].ToString(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "');", true);
                                    }
                                    TxtAsingOTWS.Text = VbNumWS;
                                    BIndDWSAOpen("UPD"); BIndDSvcSinAsingar(); BIndDWSOTRTE();

                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asingar OT a WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtEditGrdWS'");
                ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (IbtEdit != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //********************* Rtes para asignar *********************
        protected void BIndDRtesSinAsingar()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DST = (DataSet)ViewState["DST"];
            DataRow[] Result = Idioma.Select("Objeto= 'LblTitServiciosRT'");
            foreach (DataRow row in Result)
            { LblTitServicios.Text = row["Texto"].ToString(); }

            DataTable DT = new DataTable();
            DT = DST.Tables[3].Clone();
            DataRow[] DR = DST.Tables[3].Select("CodHKRva = " + DdlWSHK.Text.Trim() + " AND Orden LIKE '%" + TxtAsigOT_RTE.Text.Trim() + "%'");
            if (IsIENumerableLleno(DR)) { DT = DR.CopyToDataTable(); }

            if (DST.Tables[3].Rows.Count > 0) { GrdReportes.DataSource = DST.Tables[3]; GrdReportes.DataBind(); }
            else { GrdReportes.DataSource = null; GrdReportes.DataBind(); }
        }
        protected void RdbAsigRte_CheckedChanged(object sender, EventArgs e)
        { BIndDWSAOpen("SEL"); BIndDRtesSinAsingar(); GrdServicios.Visible = false; GrdReportes.Visible = true; }
        protected void GrdReportes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (e.CommandName.Equals("Asignar"))
                {
                    if (DdlWSHK.Text.Trim().Equals("0"))
                    { return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VBQuery = string.Format("INSERT_UPDATE_WORK_SHEET_WS @WS,@HK,0,@Rt,@Usu,NULL,0,'INSERT', @ICC");

                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                                    int rowIndex = row.RowIndex;
                                    string VbRte = ((Label)row.FindControl("LblRte")).Text.ToString().Trim();
                                    SC.Parameters.AddWithValue("@WS", TxtAsingOTWS.Text.Trim());
                                    SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                    SC.Parameters.AddWithValue("@Rt", VbRte);
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    string Mensj = "", VbNumWS = "";
                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        VbNumWS = HttpUtility.HtmlDecode(SDR["NumWS"].ToString().Trim());
                                    }
                                    SDR.Close();
                                    Transac.Commit();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row1 in Result)
                                        { Mensj = row1["Texto"].ToString(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "');", true);
                                    }
                                    TxtAsingOTWS.Text = VbNumWS;
                                    BIndDWSAOpen("UPD"); BIndDRtesSinAsingar(); BIndDWSOTRTE();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asingar Rte a WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdReportes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtEditGrdWS'");
                ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (IbtEdit != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void BIndDWSOTRTE()
        {
            GrdOTRteWS.Visible = true;
            DST = (DataSet)ViewState["DST"];
            DataTable DT = new DataTable();
            DT = DST.Tables[1].Clone();
            DataRow[] DR = DST.Tables[1].Select("Numerado = '" + TxtAsingOTWS.Text.Trim() + "'");
            if (IsIENumerableLleno(DR))
            { DT = DR.CopyToDataTable(); }
            if (DT.Rows.Count > 0)
            {
                GrdOTRteWS.DataSource = DT;
                GrdOTRteWS.DataBind();
            }
            else
            {
                GrdOTRteWS.DataSource = null;
                GrdOTRteWS.DataBind();
            }
        }
        protected void GrdOTRteWS_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdOTRteWS.EditIndex = e.NewEditIndex; BIndDWSOTRTE(); }
        protected void GrdOTRteWS_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblOT = "0", VblRte = "0";
            try
            {
                if (DdlWSHK.Text.Trim().Equals("0"))
                { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC INSERT_UPDATE_WORK_SHEET_WS @WS,@HK,@OT,@Rt,@Usu,@FV,@Ej,'UPDATE', @ICC");
                        DateTime? FechaVence = Convert.ToDateTime("01/01/1900");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string VbFuente = GrdOTRteWS.DataKeys[e.RowIndex].Values["FuenteWS"].ToString();
                                if (!(GrdOTRteWS.Rows[e.RowIndex].FindControl("TxtFecVence") as TextBox).Text.Equals(""))
                                { FechaVence = Convert.ToDateTime((GrdOTRteWS.Rows[e.RowIndex].FindControl("TxtFecVence") as TextBox).Text); }
                                if (VbFuente.Trim().Equals("OT")) { VblOT = (GrdOTRteWS.Rows[e.RowIndex].FindControl("LblOtE") as Label).Text.Trim(); }
                                else { VblRte = (GrdOTRteWS.Rows[e.RowIndex].FindControl("LblOtE") as Label).Text.Trim(); }
                                SC.Parameters.AddWithValue("@WS", TxtAsingOTWS.Text.Trim());
                                SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                SC.Parameters.AddWithValue("@OT", VblOT);
                                SC.Parameters.AddWithValue("@Rt", VblRte);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@FV", FechaVence);
                                SC.Parameters.AddWithValue("@Ej", (GrdOTRteWS.Rows[e.RowIndex].FindControl("CkbPpl") as CheckBox).Checked == true ? "1" : "0");
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                                string Mensj = "", VbNumWS = "";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    VbNumWS = HttpUtility.HtmlDecode(SDR["NumWS"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row1 in Result)
                                    { Mensj = row1["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "');", true);

                                }
                                GrdOTRteWS.EditIndex = -1;
                                TxtAsingOTWS.Text = VbNumWS;
                                BIndDWSAOpen("UPD"); BIndDWSOTRTE();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRteWS_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdOTRteWS.EditIndex = -1; BIndDWSOTRTE(); }
        protected void GrdOTRteWS_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string VbNumDoc = "0";
                if (DdlWSHK.Text.Trim().Equals("0"))
                { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = " EXEC SP_PANTALLA_WorkSheet 5,'WEB',@WS,@Usu,@Fte,@Doc,@HK,@Ej,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string VbFuente = GrdOTRteWS.DataKeys[e.RowIndex].Values["FuenteWS"].ToString();
                                VbNumDoc = (GrdOTRteWS.Rows[e.RowIndex].FindControl("LblOTRtP") as Label).Text.Trim();

                                SC.Parameters.AddWithValue("@WS", TxtAsingOTWS.Text.Trim());
                                SC.Parameters.AddWithValue("@Doc", VbNumDoc);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Fte", VbFuente);
                                SC.Parameters.AddWithValue("@HK", DdlWSHK.Text);
                                SC.Parameters.AddWithValue("@Ej", (GrdOTRteWS.Rows[e.RowIndex].FindControl("CkbPplP") as CheckBox).Checked == true ? 1 : 0);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                string Mensj = "";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row1 in Result)
                                    { Mensj = row1["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                }
                                BIndDWSAOpen("UPD"); BIndDWSOTRTE();
                                if (VbFuente.Equals("OT")) { BIndDSvcSinAsingar(); }
                                else { BIndDRtesSinAsingar(); }
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ELIMINAR OT de WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplAsigOT, UplAsigOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ELIMINAR OT de WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRteWS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbCap = dr["FuenteWS"].ToString();
                if (VbCap.Equals("RT"))
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Bisque;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.Bisque;
                }
                DataRowView drE = e.Row.DataItem as DataRowView;
                string VbPpl = drE["EJE"].ToString();
                if (VbPpl.Equals("1")) { e.Row.Cells[0].BackColor = System.Drawing.Color.Red; }
                DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (IbtEdit != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    Result = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result) { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result) { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result) { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result) { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        protected void IbtCerrarImpresion_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; }
    }
}