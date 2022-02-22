using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmStatusReportAtas : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTGrl = new DataSet();
        DataTable DTMultL = new DataTable();
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
                    Session["MonLcl"] = Cnx.GetMonedLcl();// Moneda Local
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                MlVwSt.ActiveViewIndex = 0;
                ModSeguridad();
                BindBDdlAK("UPD");
                ViewState["CONSULTA"] = "N";
                ViewState["ActualizarDiaProy"] = "ACTIVAR";
                Page.Title = "Status Report";
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
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0)
            { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0)
            { }
            if (ClsP.GetImprimir() == 0)
            { ViewState["VblImpMS"] = 0; BtnStsExport.Visible = false; BtnStsImp.Visible = false; }
            if (ClsP.GetEliminar() == 0)
            { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0)//MODIF DIA PROYecc
            { ViewState["VblCE1"] = 0; BtnModifDiaProy.Visible = false; LblModifDiaProy.Visible = false; }
            if (ClsP.GetCE2() == 0)
            { }
            if (ClsP.GetCE3() == 0)//ORDEN IMPR GRUPO 
            { ViewState["VblCE3"] = 0; BtnStsOrdenar.Visible = false; }
            if (ClsP.GetCE4() == 0)//ASIG OT A PROPUESTA
            { ViewState["VblCE4"] = 0; BtnStsAsigOT.Visible = false; BtnStsliberOT.Visible = false; }
            if (ClsP.GetCE5() == 0)
            { }
            if (ClsP.GetCE6() == 0)
            { }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,0,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                SC.Parameters.AddWithValue("@F", ViewState["PFileName"]);
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    int VbCaso = Convert.ToInt32(Regs["CASO"]);
                    string VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 2 && VbAplica.Equals("S"))
                    {

                    }
                }
            }
            IdiomaControles();
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
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"].ToString().Trim());
                SC.Parameters.AddWithValue("@F2", "CurStatus");
                SC.Parameters.AddWithValue("@F3", "0");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string b1 = tbl["Objeto"].ToString();
                    string b2 = tbl["Texto"].ToString();
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                    TitForm.Text = b1.Equals("Titulo") ? b2 : TitForm.Text;
                    if (b1.Equals("Caption"))
                    { Page.Title = b2; ViewState["PageTit"] = b2; }
                    LblStsHK.Text = b1.Trim().Equals("LblStsHK") ? b2.Trim() : LblStsHK.Text;
                    LblStsModelo.Text = b1.Trim().Equals("LblStsModelo") ? b2.Trim() : LblStsModelo.Text;
                    LblStsFecCarga.Text = b1.Trim().Equals("LblStsFecCarga") ? b2.Trim() : LblStsFecCarga.Text;
                    LblStsTSN.Text = b1.Trim().Equals("LblStsTSN") ? b2.Trim() : LblStsTSN.Text;
                    LblStsCSN.Text = b1.Trim().Equals("LblStsCSN") ? b2.Trim() : LblStsCSN.Text;
                    LblStsDiaProy.Text = b1.Trim().Equals("LblStsDiaProy") ? b2.Trim() : LblStsDiaProy.Text;
                    LblModifDiaProy.Text = b1.Trim().Equals("LblModifDiaProy") ? b2.Trim() : LblModifDiaProy.Text;
                    BtnModifDiaProy.ToolTip = b1.Trim().Equals("BtnModifDiaProyTT") ? b2.Trim() : BtnModifDiaProy.ToolTip;
                    LblStsUtilDiaHr.Text = b1.Trim().Equals("LblStsUtilDiaHr") ? b2.Trim() : LblStsUtilDiaHr.Text;
                    LblStsUtilDiaCc.Text = b1.Trim().Equals("LblStsUtilDiaCc") ? b2.Trim() : LblStsUtilDiaCc.Text;
                    LblStsUtilDiaAPU.Text = b1.Trim().Equals("LblStsUtilDiaAPU") ? b2.Trim() : LblStsUtilDiaAPU.Text;
                    BtnStsImp.Text = b1.Trim().Equals("BtnStsImp") ? b2.Trim() : BtnStsImp.Text;
                    BtnStsImp.ToolTip = b1.Trim().Equals("BtnStsImpToolT") ? b2.Trim() : BtnStsImp.ToolTip;
                    BtnStsExport.Text = b1.Trim().Equals("BtnStsExport") ? b2.Trim() : BtnStsExport.Text;
                    BtnStsOrdenar.Text = b1.Trim().Equals("BtnStsOrdenar") ? b2.Trim() : BtnStsOrdenar.Text;
                    BtnStsOrdenar.ToolTip = b1.Trim().Equals("BtnStsOrdenarToolT") ? b2.Trim() : BtnStsOrdenar.ToolTip;
                    BtnStsAsigOT.Text = b1.Trim().Equals("BtnStsAsigOT") ? b2.Trim() : BtnStsAsigOT.Text;
                    BtnStsAsigOT.ToolTip = b1.Trim().Equals("BtnStsAsigOTToolT") ? b2.Trim() : BtnStsAsigOT.ToolTip;
                    BtnStsliberOT.Text = b1.Trim().Equals("BtnStsliberOT") ? b2.Trim() : BtnStsliberOT.Text;
                    BtnStsliberOT.ToolTip = b1.Trim().Equals("BtnStsliberOTToolT") ? b2.Trim() : BtnStsliberOT.ToolTip;
                    LblStsGrupo.Text = b1.Trim().Equals("LblStsGrupo") ? b2.Trim() : LblStsGrupo.Text;
                    LblStsOrder.Text = b1.Trim().Equals("LblStsOrder") ? b2.Trim() : LblStsOrder.Text;
                    RdbStsAta.Text = b1.Trim().Equals("RdbStsAta") ? b2.Trim() : RdbStsAta.Text;
                    RdbStsProy.Text = b1.Trim().Equals("RdbStsProy") ? b2.Trim() : RdbStsProy.Text;
                    RdbStsDescrip.Text = b1.Trim().Equals("RdbStsDescrip") ? b2.Trim() : RdbStsDescrip.Text;
                    BtnStsConsult.Text = b1.Trim().Equals("BtnStsConsult") ? b2.Trim() : BtnStsConsult.Text;
                    BtnStsConsult.ToolTip = b1.Trim().Equals("BtnStsConsultTT") ? b2.Trim() : BtnStsConsult.ToolTip;
                    LblTitImpresion.Text = b1.Trim().Equals("LblTitImpresion") ? b2.Trim() : LblTitImpresion.Text;
                    IbtCerrarPrint.ToolTip = b1.Trim().Equals("CerrarVentana") ? b2.Trim() : IbtCerrarPrint.ToolTip;
                    ViewState["RTEMATRIC"] = b1.Trim().Equals("RTEMATRIC") ? b2.Trim() : ViewState["RTEMATRIC"];
                    ViewState["RTEModelo"] = b1.Trim().Equals("RTEModelo") ? b2.Trim() : ViewState["RTEModelo"];
                    ViewState["RTEUDH"] = b1.Trim().Equals("RTEUDH") ? b2.Trim() : ViewState["RTEUDH"];
                    ViewState["RTEUDC"] = b1.Trim().Equals("RTEUDC") ? b2.Trim() : ViewState["RTEUDC"];
                    ViewState["RTEActualiado"] = b1.Trim().Equals("RTEActualiado") ? b2.Trim() : ViewState["RTEActualiado"];
                    ViewState["RTEFec"] = b1.Trim().Equals("RTEFec") ? b2.Trim() : ViewState["RTEFec"];
                    ViewState["RTEPAG"] = b1.Trim().Equals("RTEPAG") ? b2.Trim() : ViewState["RTEPAG"];
                    ViewState["RTEDE"] = b1.Trim().Equals("RTEDE") ? b2.Trim() : ViewState["RTEDE"];
                    ViewState["RTEDE"] = b1.Trim().Equals("RTEDE") ? b2.Trim() : ViewState["RTEDE"];
                    ViewState["RteDesc"] = b1.Trim().Equals("RdbStsDescrip") ? b2.Trim() : ViewState["RteDesc"];
                    ViewState["RteNroDoc"] = b1.Trim().Equals("C03") ? b2.Trim() : ViewState["RteNroDoc"];
                    ViewState["RteFechCum"] = b1.Trim().Equals("RteFechCum") ? b2.Trim() : ViewState["RteFechCum"];
                    ViewState["RteFechIns"] = b1.Trim().Equals("RteFechIns") ? b2.Trim() : ViewState["RteFechIns"];
                    ViewState["RteFrec"] = b1.Trim().Equals("RteFrec") ? b2.Trim() : ViewState["RteFrec"];
                    ViewState["RteUnM"] = b1.Trim().Equals("RteUnM") ? b2.Trim() : ViewState["RteUnM"];
                    ViewState["RteFrD"] = b1.Trim().Equals("RteFrD") ? b2.Trim() : ViewState["RteFrD"];
                    ViewState["RteTipS"] = b1.Trim().Equals("RteTipS") ? b2.Trim() : ViewState["RteTipS"];
                    ViewState["RteVrCu"] = b1.Trim().Equals("RteVrCu") ? b2.Trim() : ViewState["RteVrCu"];
                    ViewState["RteAcum"] = b1.Trim().Equals("RteAcum") ? b2.Trim() : ViewState["RteAcum"];
                    ViewState["RteProS"] = b1.Trim().Equals("RteProS") ? b2.Trim() : ViewState["RteProS"];
                    ViewState["RteProF"] = b1.Trim().Equals("RteProF") ? b2.Trim() : ViewState["RteProF"];
                    ViewState["RteRmn"] = b1.Trim().Equals("RteRmn") ? b2.Trim() : ViewState["RteRmn"];
                    ViewState["RteRmnD"] = b1.Trim().Equals("RteRmnD") ? b2.Trim() : ViewState["RteRmnD"];
                    ViewState["RteOT"] = b1.Trim().Equals("C18") ? b2.Trim() : ViewState["RteOT"];
                    ViewState["RteProy"] = b1.Trim().Equals("RteProy") ? b2.Trim() : ViewState["RteProy"];
                    BtnImpStsStdr.Text = b1.Trim().Equals("BtnImpStsStdr") ? b2.Trim() : BtnImpStsStdr.Text;
                    BtnImpStsCompr.Text = b1.Trim().Equals("BtnImpStsCompr") ? b2.Trim() : BtnImpStsCompr.Text;
                    BtnImpStsGrupos.Text = b1.Trim().Equals("BtnImpStsGrupos") ? b2.Trim() : BtnImpStsGrupos.Text;
                    BtnImpStsStdr.ToolTip = b1.Trim().Equals("BtnImpStsStdrTT") ? b2.Trim() : BtnImpStsStdr.ToolTip;
                    BtnImpStsCompr.ToolTip = b1.Trim().Equals("BtnImpStsComprTT") ? b2.Trim() : BtnImpStsCompr.ToolTip;
                    BtnImpStsGrupos.ToolTip = b1.Trim().Equals("BtnImpStsGruposTT") ? b2.Trim() : BtnImpStsGrupos.ToolTip;
                    LblTitOrdenarGrupImpr.Text = b1.Trim().Equals("LblTitOrdenarGrupImpr") ? b2.Trim() : LblTitOrdenarGrupImpr.Text;
                    IbtCerrarOrder.ToolTip = b1.Trim().Equals("CerrarVentana") ? b2.Trim() : IbtCerrarOrder.ToolTip;
                    GrdOrderGrup.Columns[0].HeaderText = b1.Trim().Equals("GrdOrderGrup0") ? b2.Trim() : GrdOrderGrup.Columns[0].HeaderText;
                    GrdOrderGrup.Columns[1].HeaderText = b1.Trim().Equals("GrdOrderGrup1") ? b2.Trim() : GrdOrderGrup.Columns[1].HeaderText;
                    GrdOrderGrup.Columns[2].HeaderText = b1.Trim().Equals("GrdOrderGrup2") ? b2.Trim() : GrdOrderGrup.Columns[2].HeaderText;

                    //********************* Asignar *************************
                    LblTitAsigOTPPT.Text = b1.Trim().Equals("LblTitAsigOTPPT") ? b2.Trim() : LblTitAsigOTPPT.Text;
                    IbtCerrarAsigOtPPT.ToolTip = b1.Trim().Equals("CerrarVentana") ? b2.Trim() : IbtCerrarAsigOtPPT.ToolTip;
                    if (b1.Trim().Equals("placeholderDC"))
                    { TxtOTBusq.Attributes.Add("placeholder", b2.Trim()); }
                    IbtOTConsulAsigOTPPT.ToolTip = b1.Equals("BtnConsultar") ? b2.Trim() : IbtOTConsulAsigOTPPT.ToolTip;
                    GrdAsigOTPPT.Columns[0].HeaderText = b1.Trim().Equals("GrdAsigOTPPT0") ? b2.Trim() : GrdAsigOTPPT.Columns[0].HeaderText;
                    GrdAsigOTPPT.Columns[1].HeaderText = b1.Trim().Equals("GrdAsigOTPPT1") ? b2.Trim() : GrdAsigOTPPT.Columns[1].HeaderText;
                    GrdAsigOTPPT.Columns[2].HeaderText = b1.Trim().Equals("GrdAsigOTPPT2") ? b2.Trim() : GrdAsigOTPPT.Columns[2].HeaderText;
                    GrdAsigOTPPT.Columns[3].HeaderText = b1.Trim().Equals("LblStsHK") ? b2.Trim() : GrdAsigOTPPT.Columns[3].HeaderText;
                    GrdAsigOTPPT.Columns[4].HeaderText = b1.Trim().Equals("GrdAsigOTPPT4") ? b2.Trim() : GrdAsigOTPPT.Columns[4].HeaderText;
                    GrdAsigOTPPT.Columns[5].HeaderText = b1.Trim().Equals("GrdAsigOTPPT5") ? b2.Trim() : GrdAsigOTPPT.Columns[5].HeaderText;
                    LblTitAsigOTPPTRepa.Text = b1.Trim().Equals("LblTitAsigOTPPTRepa") ? b2.Trim() : LblTitAsigOTPPTRepa.Text;
                    LblAsigOTPPTRepa.Text = b1.Trim().Equals("GrdAsigOTPPT1") ? b2.Trim() : LblAsigOTPPTRepa.Text;
                    LblAsigOTPPTHK.Text = b1.Trim().Equals("LblStsHK") ? b2.Trim() : LblAsigOTPPTHK.Text;
                    lblAsigOTPPTCliente.Text = b1.Trim().Equals("lblAsigOTPPTCliente") ? b2.Trim() : lblAsigOTPPTCliente.Text;
                    GrdOTPPTRepa.Columns[0].HeaderText = b1.Trim().Equals("GrdAsigOTPPT0") ? b2.Trim() : GrdOTPPTRepa.Columns[0].HeaderText;
                    GrdOTPPTRepa.Columns[1].HeaderText = b1.Trim().Equals("GrdAsigOTPPT2") ? b2.Trim() : GrdOTPPTRepa.Columns[1].HeaderText;
                    GrdOTPPTRepa.Columns[2].HeaderText = b1.Trim().Equals("GrdAsigOTPPT0") ? b2.Trim() : GrdOTPPTRepa.Columns[2].HeaderText;
                    BtnStatusAnt.Text = b1.Trim().Equals("BtnStatusAnt") ? b2.Trim() : BtnStatusAnt.Text;
                    BtnStatusAnt.ToolTip = b1.Trim().Equals("BtnStatusAntTT") ? b2.Trim() : BtnStatusAnt.ToolTip;
                    //********************* Liberar OT PPT A todo Costo *************************
                    LblTitLiberarOT.Text = b1.Trim().Equals("LblTitLiberarOT") ? b2.Trim() : LblTitLiberarOT.Text;
                    IbtCerrarLiberarOT.ToolTip = b1.Trim().Equals("CerrarVentana") ? b2.Trim() : IbtCerrarLiberarOT.ToolTip;
                    LblLiberarOTNum.Text = b1.Trim().Equals("GrdAsigOTPPT1") ? b2.Trim() : LblLiberarOTNum.Text;
                    BtnLiberarOTPPT.Text = b1.Trim().Equals("BtnLiberarOTPPT") ? b2.Trim() : BtnLiberarOTPPT.Text;
                    BtnLiberarOTPPT.ToolTip = b1.Trim().Equals("BtnLiberarOTPPTTT") ? b2.Trim() : BtnLiberarOTPPT.ToolTip;
                    LblBtnLiberar.Text = b1.Trim().Equals("LblBtnLiberar") ? b2.Trim() : LblBtnLiberar.Text;
                    LblLiberarPPT.Text = b1.Trim().Equals("GrdAsigOTPPT2") ? b2.Trim() : LblLiberarPPT.Text;
                    //********************* Status en fechas anteriores *************************
                    TitStsAnterior.Text = b1.Trim().Equals("TitStsAnterior") ? b2.Trim() : TitStsAnterior.Text;
                    IbtCerrarLStsAnterior.ToolTip = b1.Trim().Equals("CerrarVentana") ? b2.Trim() : IbtCerrarLStsAnterior.ToolTip;
                    LblFechaStsAnt.Text = b1.Trim().Equals("LblFechaStsAnt") ? b2.Trim() : LblFechaStsAnt.Text;
                    BtnFechaStsAntEje.Text = b1.Trim().Equals("BtnFechaStsAntEje") ? b2.Trim() : BtnFechaStsAntEje.Text;
                    BtnStsAntExportar.Text = b1.Trim().Equals("BtnStsExport") ? b2.Trim() : BtnStsAntExportar.Text;
                    BtnStsAntExportar.ToolTip = b1.Trim().Equals("BtnStsExportTT") ? b2.Trim() : BtnStsAntExportar.ToolTip;
                    TitForm.Text = b1.Trim().Equals("Caption") ? b2.Trim() : TitForm.Text;
                }
                sqlCon.Close();
                DataRow[] Result = Idioma.Select("Objeto= 'BtnLiberarOTPPTOnClick'");
                foreach (DataRow row in Result)
                { BtnLiberarOTPPT.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/

                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdlAK(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand("EXEC SP_PANTALLA_Status 17,'','','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'", sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTGrl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTGrl);
                                DSTGrl.Tables[0].TableName = "HK";
                                DSTGrl.Tables[1].TableName = "Patron";
                                DSTGrl.Tables[2].TableName = "OrdenPtrnMto";
                                DSTGrl.Tables[3].TableName = "OTAsingar";
                                DSTGrl.Tables[4].TableName = "OTLiberar";

                                ViewState["DSTGrl"] = DSTGrl;
                            }
                        }
                    }
                }
            }
            DSTGrl = (DataSet)ViewState["DSTGrl"];
            string VbCodAnt = "";


            VbCodAnt = DdlStsHK.Text.Trim();
            DdlStsHK.DataSource = DSTGrl.Tables[0];
            DdlStsHK.DataTextField = "Matricula";
            DdlStsHK.DataValueField = "CodAeronave";
            DdlStsHK.DataBind();
            DdlStsHK.Text = VbCodAnt;

            VbCodAnt = DdlStsGrupo.Text.Trim();
            DdlStsGrupo.DataSource = DSTGrl.Tables[1];
            DdlStsGrupo.DataTextField = "Descripcion";
            DdlStsGrupo.DataValueField = "CodPatronManto";
            DdlStsGrupo.DataBind();
            DdlStsGrupo.Text = VbCodAnt;
        }
        protected void ConsulStatus()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                DateTime VbDate = Convert.ToDateTime(TxtStsFecCarga.Text.Trim().Equals("") ? "01/01/1900" : TxtStsFecCarga.Text.Trim());
                CursorIdioma.Alimentar("CurStatus", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "EXEC SP_StatusReport_WEB @CodHk,@UltFech,'NO',@PromUtlH,@PromUtlC,@PromUtlAPU,@Usu,'CurStatus',@Grupo,@Order,'',@ICC";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    string VbOrden = "";
                    if (RdbStsAta.Checked == true) { VbOrden = "ATA"; }
                    if (RdbStsProy.Checked == true) { VbOrden = "PROYECCION"; }
                    if (RdbStsDescrip.Checked == true) { VbOrden = "DESCRIPCION"; }
                    SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                    SC.Parameters.AddWithValue("@UltFech", VbDate);
                    SC.Parameters.AddWithValue("@PromUtlH", TxtStsUtilDiaHr.Text);
                    SC.Parameters.AddWithValue("@PromUtlC", TxtStsUtilDiaCc.Text);
                    SC.Parameters.AddWithValue("@PromUtlAPU", TxtStsUtilDiaAPU.Text);
                    SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                    SC.Parameters.AddWithValue("@Grupo", DdlStsGrupo.Text.Trim());
                    SC.Parameters.AddWithValue("@Order", VbOrden);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC; DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0) { GrdStatusReport.DataSource = DtB; }
                        else { GrdStatusReport.DataSource = null; }
                        GrdStatusReport.DataBind();
                    }
                    ViewState["CONSULTA"] = "S";
                }
            }
        }
        protected void Traerdatos(string Prmtr)
        {
            try
            {
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
                        TxtStsSn.Text = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                        TxtStsModelo.Text = HttpUtility.HtmlDecode(SDR["NomModelo"].ToString().Trim());
                        TxtStsFecCarga.Text = Cnx.ReturnFecha(HttpUtility.HtmlDecode(SDR["UltFechaProces"].ToString().Trim()));
                        TxtStsTSN.Text = HttpUtility.HtmlDecode(SDR["TSN"].ToString().Trim());
                        TxtStsCSN.Text = HttpUtility.HtmlDecode(SDR["CSN"].ToString().Trim());
                        TxtStsDiaProy.Text = HttpUtility.HtmlDecode(SDR["NroDiaProy"].ToString().Trim());
                        TxtStsUtilDiaHr.Text = HttpUtility.HtmlDecode(SDR["HrasProm"].ToString().Trim());
                        TxtStsUtilDiaCc.Text = HttpUtility.HtmlDecode(SDR["CclProm"].ToString().Trim());
                        TxtStsUtilDiaAPU.Text = HttpUtility.HtmlDecode(SDR["APUsProm"].ToString().Trim());
                    }
                    SDR.Close();
                    Cnx2.Close();
                }
            }
            catch (Exception Ex)
            {               
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Traer datos Status Report", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void DdlStsHK_TextChanged(object sender, EventArgs e)
        {
            Traerdatos(DdlStsHK.Text);
            RdbStsAta.Checked = true;
            TblOpciones.Visible = true;
            GrdStatusReport.Visible = false;
            ViewState["CONSULTA"] = "N";
        }
        protected void BtnModifDiaProy_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlStsHK.Text.Equals("0")) { return; }
            if (ViewState["ActualizarDiaProy"].ToString().Equals("ACTIVAR"))
            {
                TxtStsDiaProy.Enabled = true;
                TxtStsDiaProy.Focus();
                ViewState["ActualizarDiaProy"] = "GUARDAR";
                DataRow[] Result = Idioma.Select("Objeto= 'BtnModifDiaProyText'");
                foreach (DataRow row in Result)
                { BtnModifDiaProy.Text = row["Texto"].ToString().Trim(); }
            }
            else
            {
                if (Convert.ToInt32(TxtStsDiaProy.Text) < 0 || Convert.ToInt32(TxtStsDiaProy.Text) > 30)
                {
                    TxtStsDiaProy.Text = "30";
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VBQuery = "UPDATE TblFConfiguracion SET NroDiasProyeccionStatus =@Vnew WHERE IdConfigCia = @ICC";

                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {

                                SC.Parameters.AddWithValue("@Vnew", TxtStsDiaProy.Text);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                            }
                        }
                    }
                    BtnModifDiaProy.Text = "";
                    BtnModifDiaProy.Enabled = false;
                    ViewState["ActualizarDiaProy"] = "ACTIVAR";
                }
            }

        }
        protected void BtnStsImp_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (ViewState["CONSULTA"].ToString().Equals("N")) { return; }
            MlVwSt.ActiveViewIndex = 1;
        }
        protected void BtnStsExport_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (ViewState["CONSULTA"].ToString().Equals("N")) { return; }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurStatus", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_StatusReport_WEB @CodHk,@UltFech,'NO',@PromUtlH,@PromUtlC,@PromUtlAPU,@Usu,'CurStatus',@Grupo,@Order,'',@ICC";
            string VbNomRpt = "Status";

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    string VbOrden = "";
                    if (RdbStsAta.Checked == true) { VbOrden = "ATA"; }
                    if (RdbStsProy.Checked == true) { VbOrden = "PROYECCION"; }
                    if (RdbStsDescrip.Checked == true) { VbOrden = "DESCRIPCION"; }
                    SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                    SC.Parameters.AddWithValue("@UltFech", Convert.ToDateTime(TxtStsFecCarga.Text.Trim()));
                    SC.Parameters.AddWithValue("@PromUtlH", TxtStsUtilDiaHr.Text);
                    SC.Parameters.AddWithValue("@PromUtlC", TxtStsUtilDiaCc.Text);
                    SC.Parameters.AddWithValue("@PromUtlAPU", TxtStsUtilDiaAPU.Text);
                    SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                    SC.Parameters.AddWithValue("@Grupo", DdlStsGrupo.Text.Trim());
                    SC.Parameters.AddWithValue("@Order", VbOrden);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SC.Connection = con;
                        sda.SelectCommand = SC;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            ds.Tables[0].TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    wb.Worksheets.Add(dt);
                                }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
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
        protected void BtnStsConsult_Click(object sender, EventArgs e)
        {
            ConsulStatus();
            TblOpciones.Visible = false;
            GrdStatusReport.Visible = true;
        }
        //**************************** IMPRIMIR *******************************
        protected void CampoMultiL()
        {
            DTMultL.Columns.Add("ID", typeof(int)); // 
            DTMultL.Columns.Add("MltlC01", typeof(string)); // 
            DTMultL.Columns.Add("MltlC02", typeof(string)); // 
            DTMultL.Columns.Add("MltlC03", typeof(string)); // 
            DTMultL.Columns.Add("MltlC04", typeof(string)); // 
            DTMultL.Columns.Add("MltlC05", typeof(string)); // 
            DTMultL.Columns.Add("MltlC06", typeof(string)); // 
            DTMultL.Columns.Add("MltlC07", typeof(string)); // 
            DTMultL.Columns.Add("MltlC08", typeof(string)); // 
            DTMultL.Columns.Add("MltlC09", typeof(string)); // 
            DTMultL.Columns.Add("MltlC10", typeof(string)); // 
            DTMultL.Columns.Add("MltlC11", typeof(string)); // 
            DTMultL.Columns.Add("MltlC12", typeof(string)); // 
            DTMultL.Columns.Add("MltlC13", typeof(string)); // 
            DTMultL.Columns.Add("MltlC14", typeof(string)); // 
            DTMultL.Columns.Add("MltlC15", typeof(string)); // 
            DTMultL.Columns.Add("MltlC16", typeof(string)); // 
            DTMultL.Columns.Add("MltlC17", typeof(string)); // 
            DTMultL.Columns.Add("MltlC18", typeof(string)); // 
            DTMultL.Columns.Add("MltlC19", typeof(string)); // 
            DTMultL.Columns.Add("MltlC20", typeof(string)); // 
            DTMultL.Columns.Add("MltlC21", typeof(string)); //
            DTMultL.Columns.Add("MltlC22", typeof(string)); //
            DTMultL.Columns.Add("MltlC23", typeof(string)); //
            DTMultL.Columns.Add("MltlC24", typeof(string)); //
            DTMultL.Columns.Add("MltlC25", typeof(string)); //
            DTMultL.Columns.Add("MltlC26", typeof(string)); //
            DTMultL.Columns.Add("MltlC27", typeof(string)); //
            DTMultL.Columns.Add("MltlC28", typeof(string)); //
            DTMultL.Columns.Add("MltlC29", typeof(string));
            DTMultL.Columns.Add("MltlC30", typeof(string));
            DTMultL.Columns.Add("MltlC31", typeof(string));
            DTMultL.Columns.Add("MltlC32", typeof(string));// formato fecha impresion fecha y hora
            DTMultL.Columns.Add("MltlC33", typeof(string));
            DTMultL.Columns.Add("MltlC34", typeof(string));
            DTMultL.Columns.Add("MltlC35", typeof(string));
            DTMultL.Columns.Add("MltlC36", typeof(string));
            DTMultL.Columns.Add("MltlC37", typeof(string));
            DTMultL.Columns.Add("MltlC38", typeof(string));
            DTMultL.Columns.Add("MltlC39", typeof(string));
            if (DTMultL.Rows.Count == 0)
            { DTMultL.Rows.Add(0, "01", "", "03", "", "", "06", "", "", "09", "", "11", "12", "13", "", "", "16", "", "", "19", ""); }

            ViewState["DTMultL"] = DTMultL;
        }
        protected void IbtCerrarPrint_Click(object sender, ImageClickEventArgs e)
        { MlVwSt.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString(); }
        protected void BtnImpStsStdr_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString();
                if (ViewState["CONSULTA"].ToString().Equals("N")) { return; }

                CampoMultiL();
                DTMultL = (DataTable)ViewState["DTMultL"];
                string StSql = "";
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[35];
                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmFechAct", TxtStsFecCarga.Text);
                    parameters[4] = new ReportParameter("HK", DdlStsHK.SelectedItem.Text);
                    parameters[5] = new ReportParameter("SN", TxtStsSn.Text.Trim(), true);
                    parameters[6] = new ReportParameter("Model", TxtStsModelo.Text);
                    parameters[7] = new ReportParameter("TSN", TxtStsTSN.Text);
                    parameters[8] = new ReportParameter("CSN", TxtStsCSN.Text.Trim(), true);
                    parameters[9] = new ReportParameter("UDH", TxtStsUtilDiaHr.Text);
                    parameters[10] = new ReportParameter("UDC", TxtStsUtilDiaCc.Text);
                    parameters[11] = new ReportParameter("RTEMATRIC", ViewState["RTEMATRIC"].ToString());
                    parameters[12] = new ReportParameter("RTEModelo", ViewState["RTEModelo"].ToString());
                    parameters[13] = new ReportParameter("RTEUDH", ViewState["RTEUDH"].ToString());
                    parameters[14] = new ReportParameter("RTEUDC", ViewState["RTEUDC"].ToString());
                    parameters[15] = new ReportParameter("RTEActualiado", ViewState["RTEActualiado"].ToString());
                    parameters[16] = new ReportParameter("RTEFec", ViewState["RTEFec"].ToString());
                    parameters[17] = new ReportParameter("RTEPAG", ViewState["RTEPAG"].ToString());
                    parameters[18] = new ReportParameter("RTEDE", ViewState["RTEDE"].ToString());
                    parameters[19] = new ReportParameter("RteDesc", ViewState["RteDesc"].ToString());
                    parameters[20] = new ReportParameter("RteNroDoc", ViewState["RteNroDoc"].ToString());
                    parameters[21] = new ReportParameter("RteFecCu", ViewState["RteFechCum"].ToString());
                    parameters[22] = new ReportParameter("RteFecIn", ViewState["RteFechIns"].ToString());
                    parameters[23] = new ReportParameter("RteFrec", ViewState["RteFrec"].ToString());
                    parameters[24] = new ReportParameter("RteUnM", ViewState["RteUnM"].ToString());
                    parameters[25] = new ReportParameter("RteFrD", ViewState["RteFrD"].ToString());
                    parameters[26] = new ReportParameter("RteTipS", ViewState["RteTipS"].ToString());
                    parameters[27] = new ReportParameter("RteVrCu", ViewState["RteVrCu"].ToString());
                    parameters[28] = new ReportParameter("RteAcum", ViewState["RteAcum"].ToString());
                    parameters[29] = new ReportParameter("RteProS", ViewState["RteProS"].ToString());
                    parameters[30] = new ReportParameter("RteProF", ViewState["RteProF"].ToString());
                    parameters[31] = new ReportParameter("RteRmn", ViewState["RteRmn"].ToString());
                    parameters[32] = new ReportParameter("RteRmnD", ViewState["RteRmnD"].ToString());
                    parameters[33] = new ReportParameter("RteOT", ViewState["RteOT"].ToString());
                    parameters[34] = new ReportParameter("RteProy", ViewState["RteProy"].ToString());

                    DataRow DR = DTMultL.AsEnumerable().Where(r => ((int)r["ID"]).Equals(0)).First();
                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC32"] = "MM/dd/yyyy HH:mm"; }
                    else { { DR["MltlC32"] = "dd/MM/yyyy HH:mm"; } }

                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC33"] = "MM/dd/yyyy"; }
                    else { { DR["MltlC33"] = "dd/MM/yyyy"; } }

                    StSql = "EXEC SP_StatusReport_WEB @CodHk,@UltFech,'NO',@PromUtlH,@PromUtlC,@PromUtlAPU,@Usu,'',@Grupo,@Order,'I', @ICC";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {
                        string VbOrden = "";
                        if (RdbStsAta.Checked == true) { VbOrden = "ATA"; }
                        if (RdbStsProy.Checked == true) { VbOrden = "PROYECCION"; }
                        if (RdbStsDescrip.Checked == true) { VbOrden = "DESCRIPCION"; }
                        SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                        SC.Parameters.AddWithValue("@UltFech", Convert.ToDateTime(TxtStsFecCarga.Text.Trim()));
                        SC.Parameters.AddWithValue("@PromUtlH", TxtStsUtilDiaHr.Text);
                        SC.Parameters.AddWithValue("@PromUtlC", TxtStsUtilDiaCc.Text);
                        SC.Parameters.AddWithValue("@PromUtlAPU", TxtStsUtilDiaAPU.Text);
                        SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Grupo", DdlStsGrupo.Text.Trim());
                        SC.Parameters.AddWithValue("@Order", VbOrden);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RvwPrint.LocalReport.EnableExternalImages = true;
                            RvwPrint.LocalReport.ReportPath = Server.MapPath("~/Report/Ing/Status_Std.rdlc");   //"Forms /Ingenieria/Informe/Status_Std.rdlc";
                            RvwPrint.LocalReport.DataSources.Clear();
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DTMultL));
                            RvwPrint.LocalReport.SetParameters(parameters);
                            RvwPrint.LocalReport.Refresh();
                        }
                    }
                }
            }
            catch (Exception Ex)
            { string b1 = Ex.ToString(); }
        }
        protected void BtnImpStsCompr_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString();
                if (ViewState["CONSULTA"].ToString().Equals("N")) { return; }
                string StSql = "";
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[31];
                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmFechAct", TxtStsFecCarga.Text);
                    parameters[4] = new ReportParameter("HK", DdlStsHK.SelectedItem.Text);
                    parameters[5] = new ReportParameter("SN", TxtStsSn.Text.Trim(), true);
                    parameters[6] = new ReportParameter("Model", TxtStsModelo.Text);
                    parameters[7] = new ReportParameter("TSN", TxtStsTSN.Text);
                    parameters[8] = new ReportParameter("CSN", TxtStsCSN.Text.Trim(), true);
                    parameters[9] = new ReportParameter("UDH", TxtStsUtilDiaHr.Text);
                    parameters[10] = new ReportParameter("UDC", TxtStsUtilDiaCc.Text);
                    parameters[11] = new ReportParameter("RTEMATRIC", ViewState["RTEMATRIC"].ToString());
                    parameters[12] = new ReportParameter("RTEModelo", ViewState["RTEModelo"].ToString());
                    parameters[13] = new ReportParameter("RTEUDH", ViewState["RTEUDH"].ToString());
                    parameters[14] = new ReportParameter("RTEUDC", ViewState["RTEUDC"].ToString());
                    parameters[15] = new ReportParameter("RTEActualiado", ViewState["RTEActualiado"].ToString());
                    parameters[16] = new ReportParameter("RTEFec", ViewState["RTEFec"].ToString());
                    parameters[17] = new ReportParameter("RTEPAG", ViewState["RTEPAG"].ToString());
                    parameters[18] = new ReportParameter("RTEDE", ViewState["RTEDE"].ToString());
                    parameters[19] = new ReportParameter("RteDesc", ViewState["RteDesc"].ToString());
                    parameters[20] = new ReportParameter("RteNroDoc", ViewState["RteNroDoc"].ToString());
                    parameters[21] = new ReportParameter("RteFecCu", ViewState["RteFechCum"].ToString());
                    parameters[22] = new ReportParameter("RteFecIn", ViewState["RteFechIns"].ToString());
                    parameters[23] = new ReportParameter("RteFrec", ViewState["RteFrec"].ToString());
                    parameters[24] = new ReportParameter("RteUnM", ViewState["RteUnM"].ToString());
                    parameters[25] = new ReportParameter("RteFrD", ViewState["RteFrD"].ToString());
                    parameters[26] = new ReportParameter("RteAcum", ViewState["RteAcum"].ToString());
                    parameters[27] = new ReportParameter("RteRmn", ViewState["RteRmn"].ToString());
                    parameters[28] = new ReportParameter("RteRmnD", ViewState["RteRmnD"].ToString());
                    parameters[29] = new ReportParameter("RteOT", ViewState["RteOT"].ToString());
                    parameters[30] = new ReportParameter("RteProy", ViewState["RteProy"].ToString());

                    CampoMultiL();
                    DTMultL = (DataTable)ViewState["DTMultL"];
                    DataRow DR = DTMultL.AsEnumerable().Where(r => ((int)r["ID"]).Equals(0)).First();
                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC32"] = "MM/dd/yyyy HH:mm"; }
                    else { { DR["MltlC32"] = "dd/MM/yyyy HH:mm"; } }

                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC33"] = "MM/dd/yyyy"; }
                    else { { DR["MltlC33"] = "dd/MM/yyyy"; } }

                    StSql = "EXEC SP_StatusReport_WEB @CodHk,@UltFech,'NO',@PromUtlH,@PromUtlC,@PromUtlAPU,@Usu,'',@Grupo,@Order,'I', @ICC";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {
                        string VbOrden = "";
                        if (RdbStsAta.Checked == true) { VbOrden = "ATA"; }
                        if (RdbStsProy.Checked == true) { VbOrden = "PROYECCION"; }
                        if (RdbStsDescrip.Checked == true) { VbOrden = "DESCRIPCION"; }
                        SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                        SC.Parameters.AddWithValue("@UltFech", Convert.ToDateTime(TxtStsFecCarga.Text.Trim()));
                        SC.Parameters.AddWithValue("@PromUtlH", TxtStsUtilDiaHr.Text);
                        SC.Parameters.AddWithValue("@PromUtlC", TxtStsUtilDiaCc.Text);
                        SC.Parameters.AddWithValue("@PromUtlAPU", TxtStsUtilDiaAPU.Text);
                        SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Grupo", DdlStsGrupo.Text.Trim());
                        SC.Parameters.AddWithValue("@Order", VbOrden);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RvwPrint.LocalReport.EnableExternalImages = true;
                            RvwPrint.LocalReport.ReportPath = Server.MapPath("~/Report/Ing/Status_Compr.rdlc");
                            RvwPrint.LocalReport.DataSources.Clear();
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DTMultL));
                            RvwPrint.LocalReport.SetParameters(parameters);
                            RvwPrint.LocalReport.Refresh();
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }
        protected void BtnImpStsGrupos_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["CONSULTA"].ToString().Equals("N")) { return; }
                string StSql = "";
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[35];
                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmFechAct", TxtStsFecCarga.Text);
                    parameters[4] = new ReportParameter("HK", DdlStsHK.SelectedItem.Text);
                    parameters[5] = new ReportParameter("SN", TxtStsSn.Text.Trim(), true);
                    parameters[6] = new ReportParameter("Model", TxtStsModelo.Text);
                    parameters[7] = new ReportParameter("TSN", TxtStsTSN.Text);
                    parameters[8] = new ReportParameter("CSN", TxtStsCSN.Text.Trim(), true);
                    parameters[9] = new ReportParameter("UDH", TxtStsUtilDiaHr.Text);
                    parameters[10] = new ReportParameter("UDC", TxtStsUtilDiaCc.Text);
                    parameters[11] = new ReportParameter("RTEMATRIC", ViewState["RTEMATRIC"].ToString());
                    parameters[12] = new ReportParameter("RTEModelo", ViewState["RTEModelo"].ToString());
                    parameters[13] = new ReportParameter("RTEUDH", ViewState["RTEUDH"].ToString());
                    parameters[14] = new ReportParameter("RTEUDC", ViewState["RTEUDC"].ToString());
                    parameters[15] = new ReportParameter("RTEActualiado", ViewState["RTEActualiado"].ToString());
                    parameters[16] = new ReportParameter("RTEFec", ViewState["RTEFec"].ToString());
                    parameters[17] = new ReportParameter("RTEPAG", ViewState["RTEPAG"].ToString());
                    parameters[18] = new ReportParameter("RTEDE", ViewState["RTEDE"].ToString());
                    parameters[19] = new ReportParameter("RteDesc", ViewState["RteDesc"].ToString());
                    parameters[20] = new ReportParameter("RteNroDoc", ViewState["RteNroDoc"].ToString());
                    parameters[21] = new ReportParameter("RteFecCu", ViewState["RteFechCum"].ToString());
                    parameters[22] = new ReportParameter("RteFecIn", ViewState["RteFechIns"].ToString());
                    parameters[23] = new ReportParameter("RteFrec", ViewState["RteFrec"].ToString());
                    parameters[24] = new ReportParameter("RteUnM", ViewState["RteUnM"].ToString());
                    parameters[25] = new ReportParameter("RteFrD", ViewState["RteFrD"].ToString());
                    parameters[26] = new ReportParameter("RteTipS", ViewState["RteTipS"].ToString());
                    parameters[27] = new ReportParameter("RteVrCu", ViewState["RteVrCu"].ToString());
                    parameters[28] = new ReportParameter("RteAcum", ViewState["RteAcum"].ToString());
                    parameters[29] = new ReportParameter("RteProS", ViewState["RteProS"].ToString());
                    parameters[30] = new ReportParameter("RteProF", ViewState["RteProF"].ToString());
                    parameters[31] = new ReportParameter("RteRmn", ViewState["RteRmn"].ToString());
                    parameters[32] = new ReportParameter("RteRmnD", ViewState["RteRmnD"].ToString());
                    parameters[33] = new ReportParameter("RteOT", ViewState["RteOT"].ToString());
                    parameters[34] = new ReportParameter("RteProy", ViewState["RteProy"].ToString());

                    CampoMultiL();
                    DTMultL = (DataTable)ViewState["DTMultL"];
                    DataRow DR = DTMultL.AsEnumerable().Where(r => ((int)r["ID"]).Equals(0)).First();
                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC32"] = "MM/dd/yyyy HH:mm"; }
                    else { { DR["MltlC32"] = "dd/MM/yyyy HH:mm"; } }

                    if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC33"] = "MM/dd/yyyy"; }
                    else { { DR["MltlC33"] = "dd/MM/yyyy"; } }

                    StSql = "EXEC SP_StatusReport_WEB @CodHk,@UltFech,'NO',@PromUtlH,@PromUtlC,@PromUtlAPU,@Usu,'',@Grupo,@Order,'I', @ICC";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {
                        string VbOrden = "GRUPOS";
                        SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                        SC.Parameters.AddWithValue("@UltFech", Convert.ToDateTime(TxtStsFecCarga.Text.Trim()));
                        SC.Parameters.AddWithValue("@PromUtlH", TxtStsUtilDiaHr.Text);
                        SC.Parameters.AddWithValue("@PromUtlC", TxtStsUtilDiaCc.Text);
                        SC.Parameters.AddWithValue("@PromUtlAPU", TxtStsUtilDiaAPU.Text);
                        SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Grupo", DdlStsGrupo.Text.Trim());
                        SC.Parameters.AddWithValue("@Order", VbOrden);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RvwPrint.LocalReport.EnableExternalImages = true;
                            RvwPrint.LocalReport.ReportPath = Server.MapPath("~/Report/Ing/Status_Grupos.rdlc");   //"Forms /Ingenieria/Informe/Status_Std.rdlc";
                            RvwPrint.LocalReport.DataSources.Clear();
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RvwPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DTMultL));
                            RvwPrint.LocalReport.SetParameters(parameters);
                            RvwPrint.LocalReport.Refresh();
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }
        //**************************** ORdernar *******************************
        protected void BindDOrdenar()
        {
            DSTGrl = (DataSet)ViewState["DSTGrl"];
            if (DSTGrl.Tables[2].Rows.Count > 0)
            { GrdOrderGrup.DataSource = DSTGrl.Tables[2]; GrdOrderGrup.DataBind(); }
            else
            { GrdOrderGrup.DataSource = null; GrdOrderGrup.DataBind(); }
        }
        protected void BtnStsOrdenar_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); BindDOrdenar(); MlVwSt.ActiveViewIndex = 2; }
        protected void IbtCerrarOrder_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); MlVwSt.ActiveViewIndex = 0; }
        protected void GrdOrderGrup_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdOrderGrup.EditIndex = e.NewEditIndex; BindDOrdenar(); }
        protected void GrdOrderGrup_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblId = GrdOrderGrup.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "UPDATE TblPatronManto SET OrdenImpresion=@Orden,UsuMod=@Usu,FechaMod=GETDATE() WHERE CodPatronManto=@I";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@I", VblId);
                            SC.Parameters.AddWithValue("@Orden", (GrdOrderGrup.Rows[e.RowIndex].FindControl("TxtPos") as TextBox).Text.Trim());
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());

                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdOrderGrup.EditIndex = -1;
                            BindBDdlAK("UPD");
                            BindDOrdenar();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplOrder, UplOrder.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Ordenar impresión grupos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdOrderGrup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdOrderGrup.EditIndex = -1; BindDOrdenar(); }
        protected void GrdOrderGrup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                    ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                    Result = Idioma.Select("Objeto= 'IbtCancel'");
                    foreach (DataRow row in Result)
                    { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                    if (IbtEdit != null)
                    {
                        foreach (DataRow RowIdioma in Result)
                        { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }
                }
            }
            catch (Exception)
            { }
        }
        //**************************** Asignar OT a PPT *******************************
        protected void BindDAsignarOTPPT()
        {
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC Consultas_General_MRO 20,@OT,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                {
                    SC.Parameters.AddWithValue("@OT", TxtOTBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SCX2.Open();
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(DT);
                        if (DT.Rows.Count > 0)
                        { GrdAsigOTPPT.DataSource = DT; GrdAsigOTPPT.DataBind(); }
                        else
                        { GrdAsigOTPPT.DataSource = null; GrdAsigOTPPT.DataBind(); }
                    }
                }
            }
        }
        protected void BtnStsAsigOT_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); BindDAsignarOTPPT(); BindDdlOTAsig(); MlVwSt.ActiveViewIndex = 3; }
        protected void IbtCerrarAsigOtPPT_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); MlVwSt.ActiveViewIndex = 0; }
        protected void IbtOTConsulAsigOTPPT_Click(object sender, ImageClickEventArgs e)
        { BindDAsignarOTPPT(); }
        protected void GrdAsigOTPPT_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdAsigOTPPT.EditIndex = e.NewEditIndex; BindDAsignarOTPPT();
            if (!DdlAsigOTPPT.Text.Equals("0")) { GrdOTPPTRepa.EditIndex = -1; BindDAsignarOTPPTRepa(); }
        }
        protected void GrdAsigOTPPT_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblIdSvcM = GrdAsigOTPPT.DataKeys[e.RowIndex].Value.ToString();
            string VbIdOT = GrdAsigOTPPT.DataKeys[e.RowIndex].Values["CodNumOrdenTrab"].ToString();
            string VbIdPPT = GrdAsigOTPPT.DataKeys[e.RowIndex].Values["IdPropuesta"].ToString();
            int VbAsignar = (GrdAsigOTPPT.Rows[e.RowIndex].FindControl("CkbSelec") as CheckBox).Checked == true ? 1 : 0;
            if (VbAsignar == 1)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Status 13,@Usu,'','','',@PPT,@OT,@IdSvc,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PPT", VbIdPPT.Equals("") ? "0" : VbIdPPT);
                                SC.Parameters.AddWithValue("@OT", VbIdOT.Equals("")?"0": VbIdOT);
                                SC.Parameters.AddWithValue("@IdSvc", VblIdSvcM);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                string VbEjecPlano = "N";

                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                sqlCon.Close();
                                if (VbEjecPlano.Trim().Equals("S"))
                                {
                                    Cnx.SelecBD();
                                    using (SqlConnection Cnx3 = new SqlConnection(Cnx.GetConex()))
                                    {
                                        Cnx3.Open();
                                        VBQuery = string.Format("EXEC SP_IntegradorNEW 15,'',@Usu1,'','','',@PPT,@OT,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, Cnx3))
                                        {
                                            try
                                            {
                                                sqlCmd.Parameters.AddWithValue("@PPT", VbIdPPT.Equals("") ? "0" : VbIdPPT);
                                                sqlCmd.Parameters.AddWithValue("@OT", VbIdOT.Equals("") ? "0" : VbIdOT);
                                                sqlCmd.Parameters.AddWithValue("@Usu1", Session["C77U"].ToString());
                                                sqlCmd.ExecuteNonQuery();
                                                Cnx3.Close();
                                            }
                                            catch (Exception ex)
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this.GrdAsigOTPPT, GrdAsigOTPPT.GetType(), "alert", "alert('Error en la generación del plano');", true);
                                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Asingar OT A PPT STATUS", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOrder, UplOrder.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Asignar OT a PPT STATUS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            GrdAsigOTPPT.EditIndex = -1;
            BindDAsignarOTPPT();
            if (!DdlAsigOTPPT.Text.Equals("0")) { BindDAsignarOTPPTRepa(); }
        }
        protected void GrdAsigOTPPT_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAsigOTPPT.EditIndex = -1; BindDAsignarOTPPT(); }
        protected void GrdAsigOTPPT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result) { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result) { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (IbtEdit != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void GrdAsigOTPPT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAsigOTPPT.PageIndex = e.NewPageIndex; BindDAsignarOTPPT(); }
        //**************************** Asignar OT a PPT REPA *******************************
        protected void BindDdlOTAsig()
        {
            DSTGrl = (DataSet)ViewState["DSTGrl"];
            DdlAsigOTPPT.DataSource = DSTGrl.Tables[3];
            DdlAsigOTPPT.DataTextField = "CodOT";
            DdlAsigOTPPT.DataValueField = "CodNumOrdenTrab";
            DdlAsigOTPPT.DataBind();
        }
        protected void BindDAsignarOTPPTRepa()
        {
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_Status 9,@CodPr,@Tipo,'','',@HK,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                {
                    SC.Parameters.AddWithValue("@CodPr", ViewState["CodPropietario"].ToString().Trim());
                    SC.Parameters.AddWithValue("@Tipo", ViewState["TipoPPT"].ToString().Trim());
                    SC.Parameters.AddWithValue("@HK", ViewState["CodAeronave"].ToString().Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SCX2.Open();
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(DT);
                        if (DT.Rows.Count > 0)
                        {
                            GrdOTPPTRepa.DataSource = DT;
                            GrdOTPPTRepa.DataBind();
                        }
                        else
                        {
                            GrdOTPPTRepa.DataSource = null;
                            GrdOTPPTRepa.DataBind();
                        }
                    }
                }
            }
        }
        protected void DdlAsigOTPPT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format("  EXEC SP_PANTALLA_Status 8,'WEB',@Prmtr,'','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                    SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                    SC.Parameters.AddWithValue("@Prmtr", DdlAsigOTPPT.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        TxtAsigOTPPTHK.Text = HttpUtility.HtmlDecode(SDR["Matricula"].ToString().Trim());
                        ViewState["AplicaOTPPTAsing"] = HttpUtility.HtmlDecode(SDR["Aplicabilidad"].ToString().Trim());
                        if (ViewState["AplicaOTPPTAsing"].ToString().Equals("SN"))
                        {
                            TxtlAsigOTPPTPN.Text = HttpUtility.HtmlDecode(SDR["PNOT"].ToString().Trim());
                            TxtlAsigOTPPTSN.Text = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                        }
                        else
                        { TxtlAsigOTPPTPN.Text = ""; TxtlAsigOTPPTSN.Text = ""; }
                        TxtAsigOTPPTCliente.Text = HttpUtility.HtmlDecode(SDR["RazonSocial"].ToString().Trim());
                        TxtAsigOTPPTSvc.Text = HttpUtility.HtmlDecode(SDR["Servicio"].ToString().Trim());
                        ViewState["TipoPPT"] = HttpUtility.HtmlDecode(SDR["TipoPPT"].ToString().Trim());
                        ViewState["CodPropietario"] = HttpUtility.HtmlDecode(SDR["CodPropietario"].ToString().Trim());
                        ViewState["CodAeronave"] = HttpUtility.HtmlDecode(SDR["CodAeronave"].ToString().Trim());
                    }
                    SDR.Close();
                    Cnx2.Close();
                }
                BindDAsignarOTPPTRepa();
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UplPpal, UplPpal.GetType(), "alert", "alert('Inconveniente con la consulta');", true);
            }
        }
        protected void GrdOTPPTRepa_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdOTPPTRepa.EditIndex = e.NewEditIndex; BindDAsignarOTPPTRepa(); GrdAsigOTPPT.EditIndex = -1; BindDAsignarOTPPT(); }
        protected void GrdOTPPTRepa_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int VbIdPpt = Convert.ToInt32(GrdOTPPTRepa.DataKeys[e.RowIndex].Value.ToString());
            int VbIdDetPropHk = Convert.ToInt32(GrdOTPPTRepa.DataKeys[e.RowIndex].Values["IdDetPropHk"].ToString());

            int VbAsignar = (GrdOTPPTRepa.Rows[e.RowIndex].FindControl("CkbSelec") as CheckBox).Checked == true ? 1 : 0;
            if (VbAsignar == 1)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Status 14,@Usu,'','','',@PPT,@OT,@VbIdDetPropHk,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                //string b1 = (GrdOTPPTRepa.Rows[e.RowIndex].FindControl("LblPPT") as Label).Text.Trim();
                                SC.Parameters.AddWithValue("@PPT", VbIdPpt);
                                SC.Parameters.AddWithValue("@OT", DdlAsigOTPPT.Text);
                                SC.Parameters.AddWithValue("@VbIdDetPropHk", VbIdDetPropHk);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                string VbEjecPlano = "N";

                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim()); }
                                SDR.Close();
                                Transac.Commit();
                                sqlCon.Close();
                                if (VbEjecPlano.Trim().Equals("S"))
                                {
                                    Cnx.SelecBD();
                                    using (SqlConnection Cnx3 = new SqlConnection(Cnx.GetConex()))
                                    {
                                        Cnx3.Open();
                                        VBQuery = string.Format("EXEC SP_IntegradorNEW 15,'',@Usu1,'','','',@PPT,@OT,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, Cnx3))
                                        {
                                            try
                                            {
                                                sqlCmd.Parameters.AddWithValue("@PPT", VbIdPpt);
                                                sqlCmd.Parameters.AddWithValue("@OT", DdlAsigOTPPT.Text);
                                                sqlCmd.Parameters.AddWithValue("@Usu1", Session["C77U"].ToString());
                                                sqlCmd.ExecuteNonQuery();
                                                Cnx3.Close();
                                            }
                                            catch (Exception ex)
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this.GrdAsigOTPPT, GrdAsigOTPPT.GetType(), "alert", "alert('Error en la generación del plano');", true);
                                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Asingar OT A PPT REPA STATUS", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.GrdAsigOTPPT, GrdAsigOTPPT.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Asignar OT a PPT STATUS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            GrdOTPPTRepa.EditIndex = -1;
            BindBDdlAK("UPD");
            BindDAsignarOTPPT();
            BindDdlOTAsig();
            TxtAsigOTPPTHK.Text = "";
            TxtlAsigOTPPTPN.Text = "";
            TxtlAsigOTPPTSN.Text = "";
            TxtAsigOTPPTCliente.Text = "";
            TxtAsigOTPPTSvc.Text = "";
            GrdOTPPTRepa.DataSource = null;
            GrdOTPPTRepa.DataBind();
        }
        protected void GrdOTPPTRepa_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdOTPPTRepa.EditIndex = -1; BindDAsignarOTPPTRepa(); }
        protected void GrdOTPPTRepa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result) { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result) { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (IbtEdit != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //**************************** Status de fechas pasadas *******************************
        protected void BtnStatusAnt_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); if (DdlStsHK.Text.Equals("0")) { return; } MlVwSt.ActiveViewIndex = 5; }
        protected void BindDStsAnt()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

                CursorIdioma.Alimentar("CurStatus", Session["77IDM"].ToString().Trim());
                string VbTxtSql = " EXEC SP_PANTALLA_Proceso_Ingenieria 12,'','','CurStatus','',@CodHk,@Order,1,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    string VbOrden = "2"; //1 ATA|2 PROYECCION|3 DESCRIPCION
                    SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                    SC.Parameters.AddWithValue("@Fech", TxtFechaStsAnt.Text.Trim());
                    SC.Parameters.AddWithValue("@Order", VbOrden);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdStsAnt.DataSource = DtB;
                            GrdStsAnt.DataBind();
                        }
                        else
                        {
                            GrdStsAnt.DataSource = null;
                            GrdStsAnt.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtCerrarLStsAnterior_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); MlVwSt.ActiveViewIndex = 0; }
        protected void BtnFechaStsAntEje_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (TxtFechaStsAnt.Text.Equals("")) { return; }
            if (TxtFechaStsAnt.Text.Length > 10) { return; }
            BindDStsAnt();
        }
        protected void BtnStsAntExportar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (TxtFechaStsAnt.Text.Equals("")) { return; }
            if (TxtFechaStsAnt.Text.Length > 10) { return; }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurStatus", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 12,'','','CurStatus','',@CodHk,@Order,1,@ICC,@Fech,'01-01-1900','01-01-1900'";
            string VbNomRpt = "Status_2";

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    string VbOrden = "2"; //1 ATA|2 PROYECCION|3 DESCRIPCION
                    SC.Parameters.AddWithValue("@CodHk", DdlStsHK.Text);
                    SC.Parameters.AddWithValue("@Fech", TxtFechaStsAnt.Text.Trim());
                    SC.Parameters.AddWithValue("@Order", VbOrden);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SC.Connection = con;
                        sda.SelectCommand = SC;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            ds.Tables[0].TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    wb.Worksheets.Add(dt);
                                }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
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
        //**************************** Liberar OT de PPT a todo costo *******************************
        protected void BtnStsliberOT_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); BindDdlLiberarOT(); MlVwSt.ActiveViewIndex = 4; }
        protected void BindDdlLiberarOT()
        {
            DSTGrl = (DataSet)ViewState["DSTGrl"];
            DdlLiberarOTNum.DataSource = DSTGrl.Tables[4];
            DdlLiberarOTNum.DataTextField = "OT";
            DdlLiberarOTNum.DataValueField = "CodOT";
            DdlLiberarOTNum.DataBind();
        }
        protected void IbtCerrarLiberarOT_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString(); MlVwSt.ActiveViewIndex = 0; }
        protected void DdlLiberarOTNum_TextChanged(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Status 15,'','','','',@Prmtr,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@Prmtr", DdlLiberarOTNum.Text);
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtLiberarPPT.Text = HttpUtility.HtmlDecode(SDR["IdPropuesta"].ToString().Trim());
                    TxtLiberarCodPPT.Text = HttpUtility.HtmlDecode(SDR["CodigoPPT"].ToString().Trim());
                }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void BtnLiberarOTPPT_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (!DdlLiberarOTNum.Text.Equals("0") && !TxtLiberarPPT.Text.Trim().Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Status 16,@Usu,'','','',@OT,@PPT,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@OT", DdlLiberarOTNum.Text);
                                SC.Parameters.AddWithValue("@PPT", TxtLiberarPPT.Text);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                sqlCon.Close();
                                BindBDdlAK("UPD");
                                BindDdlLiberarOT();
                                TxtLiberarPPT.Text = "";
                                TxtLiberarCodPPT.Text = "";
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.GrdAsigOTPPT, GrdAsigOTPPT.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "LIBERAR OT DE PPT A TODO CASTO STATUS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            GrdOTPPTRepa.EditIndex = -1;           
            BindDAsignarOTPPT();
            BindDdlOTAsig();
            TxtAsigOTPPTHK.Text = "";
            TxtlAsigOTPPTPN.Text = "";
            TxtlAsigOTPPTSN.Text = "";
            TxtAsigOTPPTCliente.Text = "";
            TxtAsigOTPPTSvc.Text = "";
            GrdOTPPTRepa.DataSource = null;
            GrdOTPPTRepa.DataBind();
        }
    }
}