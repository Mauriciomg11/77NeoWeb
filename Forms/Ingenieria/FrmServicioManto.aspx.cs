using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Runtime.InteropServices.WindowsRuntime;
using _77NeoWeb.Prg.PrgIngenieria;
using System.IO;
using ClosedXML.Excel;
using System.EnterpriseServices;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using System.Configuration;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmServicioManto : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDet = new DataSet();
        DataSet DSTRcso = new DataSet();
        private string Vbl3Desc, Vbl4Ruta, VBQuery, Vbl6Ext, Vbl8Type;
        private byte[] imagen;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            { Page.Title = string.Format("Servicio_Mantenimiento"); }
            else
            { Page.Title = string.Format("Reparaciones_Mayores"); }
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["VldrCntdr"] = "S";
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
                ViewState["PageTit"] = "";
                ModSeguridad();
                TipoPantalla();
                CorreccionDatos();
                ViewState["UCD"] = 0;
                ViewState["TIPO"] = "A";
                ViewState["IdCodElem"] = -1;
                ViewState["PN"] = "";
                ViewState["SN"] = "";
                ViewState["CodElem"] = "";
                BtnAK.CssClass = "btn btn-primary";
                BindDTraerdatos("0", "", "UPD");
                BindDAK();
                BindDataAll();
                GrdAeron.Visible = true;
                ViewState["TipoAccion"] = "";
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["CE1"] = 1;
            ViewState["CE3"] = 1;
            ViewState["CE4"] = 1;
            ViewState["CE5"] = 1;
            ViewState["CE6"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                IbtAdd.Visible = false; BtnConfigContdrInic.Visible = false;
                GrdAeron.ShowFooter = false;
                GrdPN.ShowFooter = false;
                GrdHKAsig.ShowFooter = false;
                GrdAdj.ShowFooter = false;
                GrdRecursoF.ShowFooter = false;
                GrdLicen.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                IbtUpdate.Visible = false;
                IbtGenerOT.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
                IbtFind.Visible = false;
            }
            if (ClsP.GetImprimir() == 0)
            {
                IbtPrint.Visible = false;
            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                IbtDelete.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {// opcion de visualizar status
                ViewState["CE1"] = 0;
                CkbVisuStat.Visible = false;
            }
            if (ClsP.GetCE2() == 0)
            {

            }
            if (ClsP.GetCE3() == 0)
            {// Asignar aeronaves
                ViewState["CE3"] = 0;
            }
            if (ClsP.GetCE4() == 0)
            {// cambiar etapa actual
                ViewState["CE4"] = 0;
            }
            if (ClsP.GetCE5() == 0)
            {
                ViewState["CE5"] = 0;
                IbtRecurso.Visible = false;
            }
            if (ClsP.GetCE6() == 0)
            {
                ViewState["CE6"] = 0;
                CkbBloqRec.Visible = false;
            }
            IdiomaControles();
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
                SC.Parameters.AddWithValue("@F1", "FrmServicioManto");
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
                    {
                        if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
                        { Page.Title = bT; ViewState["PageTit"] = bT; }

                    }
                    if (bO.Equals("CaptionRepa"))
                    {
                        if (!Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
                        { Page.Title = bT; ViewState["PageTit"] = bT; }
                    }
                    TitForm.Text = bO.Equals("LblTituloSMtoRP") ? bT : TitForm.Text;
                    IbtAdd.ToolTip = bO.Equals("IbtAdd") ? bT : IbtAdd.ToolTip;
                    IbtUpdate.ToolTip = bO.Equals("IbtUpdate") ? bT : IbtUpdate.ToolTip;
                    IbtFind.ToolTip = bO.Equals("IbtFind") ? bT : IbtFind.ToolTip;
                    IbtPrint.ToolTip = bO.Equals("IbtPrint") ? bT : IbtPrint.ToolTip;
                    IbtDelete.ToolTip = bO.Equals("IbtDelete") ? bT : IbtDelete.ToolTip;
                    IbtRecurso.ToolTip = bO.Equals("IbtRecurso") ? bT : IbtRecurso.ToolTip;
                    IbtGenerOT.ToolTip = bO.Equals("IbtGenerOT") ? bT : IbtGenerOT.ToolTip;
                    CkbVisuStat.Text = bO.Equals("CkbVisuStat") ? bT : CkbVisuStat.Text;
                    LblCod.Text = bO.Equals("LblCod") ? bT + ":" : LblCod.Text;
                    if (bO.Equals("LblDescrip"))
                    {
                        LblDescrip.Text = bT; GrdPN.Columns[1].HeaderText = bT;
                        GrdAdj.Columns[0].HeaderText = bT;
                        RdbBusqDes.Text = "&nbsp " + bT;
                        RdbBusqDesPN.Text = "&nbsp " + bT;
                        RdbBusqDesSN.Text = "&nbsp " + bT;
                        GrdRecursoF.Columns[2].HeaderText = bT;
                        GrdLicen.Columns[1].HeaderText = bT;
                        ViewState["DesInf"] = bT;
                    }
                    LblAkAsing.Text = bO.Equals("LblAkAsing") ? bT : LblAkAsing.Text;
                    GrdHKAsig.Columns[0].HeaderText = bO.Equals("BtnAK") ? bT : GrdHKAsig.Columns[0].HeaderText;
                    GrdHKAsig.Columns[1].HeaderText = bO.Equals("GrdMod") ? bT : GrdHKAsig.Columns[1].HeaderText;
                    LblHoriz.Text = bO.Equals("LblHoriz") ? bT + ":" : LblHoriz.Text;
                    LblHoriz.ToolTip = bO.Equals("LblHorizTT") ? bT : LblHoriz.ToolTip;
                    LblCumplimi.Text = bO.Equals("LblCumplimi") ? bT : LblCumplimi.Text;
                    LblGrupo.Text = bO.Equals("LblGrupo") ? bT + ":" : LblGrupo.Text;
                    //   LblActual.Text = bO.Equals("LblActual") ? bT + ":" : LblActual.Text;
                    LblDoc.Text = bO.Equals("LblDoc") ? bT + ":" : LblDoc.Text;
                    LblRefOT.Text = bO.Equals("LblRefOT") ? bT + ":" : LblRefOT.Text;
                    LblModel.Text = bO.Equals("GrdMod") ? bT + ":" : LblModel.Text;
                    LblTaller.Text = bO.Equals("LblTaller") ? bT + ":" : LblTaller.Text;
                    CkbAplSub.Text = bO.Equals("CkbAplSub") ? bT : CkbAplSub.Text;
                    LblAta.Text = bO.Equals("LblAta") ? bT + ":" : LblAta.Text;
                    LblSubAta.Text = bO.Equals("LblSubAta") ? bT + ":" : LblSubAta.Text;
                    LblConsecAta.Text = bO.Equals("LblConsecAta") ? bT + ":" : LblConsecAta.Text;
                    if (bO.Equals("LblTipo")) { LblTipo.Text = bT + ":"; GrdRecursoF.Columns[7].HeaderText = bT; ViewState["TypInf"] = bT; }
                    if (bO.Equals("placeholder02"))
                    { TxtHistorico.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("placeholder03"))
                    { TxtEstadoOT.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("BtnAK"))
                    { TxtMatric.Attributes.Add("placeholder", bT); BtnAK.Text = bT; }
                    CkbBloqRec.Text = bO.Equals("CkbBloqRec") ? bT + ":" : CkbBloqRec.Text;
                    CkbBloqRec.ToolTip = bO.Equals("CkbBloqRecTT") ? bT : CkbBloqRec.ToolTip;
                    if (bO.Equals("GrdMatr")) { GrdAeron.Columns[0].HeaderText = bT; ViewState["AkInf"] = bT; }
                    if (bO.Equals("GrdCont")) { GrdAeron.Columns[1].HeaderText = bT; GrdPN.Columns[2].HeaderText = bT; ViewState["ContInf"] = bT; }
                    GrdAeron.Columns[2].HeaderText = bO.Equals("GrdFreIni") ? bT : GrdAeron.Columns[2].HeaderText;
                    if (bO.Equals("GrdFrec")) { GrdAeron.Columns[3].HeaderText = bT; GrdPN.Columns[3].HeaderText = bT; GrdSN.Columns[4].HeaderText = bT; }

                    GrdAeron.Columns[4].HeaderText = bO.Equals("GrdExt") ? bT : GrdAeron.Columns[4].HeaderText;
                    GrdAeron.Columns[5].HeaderText = bO.Equals("GrdFrecAct") ? bT : GrdAeron.Columns[5].HeaderText;
                    GrdAeron.Columns[6].HeaderText = bO.Equals("GrdDias") ? bT : GrdAeron.Columns[6].HeaderText;
                    GrdAeron.Columns[7].HeaderText = bO.Equals("GrdExtD") ? bT : GrdAeron.Columns[7].HeaderText;
                    GrdAeron.Columns[8].HeaderText = bO.Equals("GrdFechI") ? bT : GrdAeron.Columns[8].HeaderText;
                    GrdAeron.Columns[10].HeaderText = bO.Equals("GrdHist") ? bT : GrdAeron.Columns[10].HeaderText;


                    GrdPN.Columns[4].HeaderText = bO.Equals("GrdDias") ? bT : GrdPN.Columns[4].HeaderText;
                    GrdSN.Columns[2].HeaderText = bO.Equals("GrdCont2") ? bT : GrdSN.Columns[2].HeaderText;
                    GrdSN.Columns[3].HeaderText = bO.Equals("GrdFreIni") ? bT : GrdSN.Columns[3].HeaderText;

                    GrdSN.Columns[6].HeaderText = bO.Equals("GrdFrecAct") ? bT : GrdSN.Columns[6].HeaderText;
                    GrdSN.Columns[7].HeaderText = bO.Equals("GrdDias") ? bT : GrdSN.Columns[7].HeaderText;
                    GrdSN.Columns[8].HeaderText = bO.Equals("GrdExtD") ? bT : GrdSN.Columns[8].HeaderText;
                    GrdSN.Columns[9].HeaderText = bO.Equals("GrdFechI") ? bT : GrdSN.Columns[9].HeaderText;
                    GrdAdj.Columns[1].HeaderText = bO.Equals("GrdNomArch") ? bT : GrdAdj.Columns[1].HeaderText;
                    // ************************************** Busqueda  *******************************************************       
                    LbltitBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LbltitBusq.Text;
                    LblBusq.Text = bO.Equals("Busqueda") ? bT : LblBusq.Text;
                    if (bO.Equals("placeholder")) { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtConsultar.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    // ************************************** Recurso  *******************************************************       
                    LblTitRecursoLice.Text = bO.Equals("LblTitRecursoLice") ? bT : LblTitRecursoLice.Text;
                    IbtCerrarRec.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarRec.ToolTip;
                    GrdRecursoF.Columns[0].HeaderText = bO.Equals("GrdPNum") ? bT : GrdRecursoF.Columns[0].HeaderText;
                    GrdRecursoF.Columns[1].HeaderText = bO.Equals("GrdRef") ? bT : GrdRecursoF.Columns[1].HeaderText;
                    GrdRecursoF.Columns[3].HeaderText = bO.Equals("GrdCant") ? bT : GrdRecursoF.Columns[3].HeaderText;
                    GrdRecursoF.Columns[4].HeaderText = bO.Equals("LblEtapa") ? bT : GrdRecursoF.Columns[4].HeaderText;
                    GrdRecursoF.Columns[5].HeaderText = bO.Equals("GrdCondic") ? bT : GrdRecursoF.Columns[5].HeaderText;
                    GrdRecursoF.Columns[6].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdRecursoF.Columns[6].HeaderText;
                    LblTitLicen.Text = bO.Equals("LblTitLicen") ? bT : LblTitLicen.Text;
                    GrdLicen.Columns[0].HeaderText = bO.Equals("GrdLicen") ? bT : GrdLicen.Columns[0].HeaderText;
                    GrdLicen.Columns[2].HeaderText = bO.Equals("GrdTiemEst") ? bT : GrdLicen.Columns[2].HeaderText;
                    // ************************************** Imprimir  *******************************************************  
                    IbtCerrarInf.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarInf.ToolTip;
                    BtnSvcAct.Text = bO.Equals("BtnSvcAct") ? bT : BtnSvcAct.Text;
                    BtnCumplim.Text = bO.Equals("BtnCumplim") ? bT : BtnCumplim.Text;
                    IbtExpExcelSvcAplAK.ToolTip = bO.Equals("IbtExpExcelSvcAplAK") ? bT : IbtExpExcelSvcAplAK.ToolTip;
                    IbtExpExcelSvcGnrl.ToolTip = bO.Equals("IbtExpExcelSvcGnrl") ? bT : IbtExpExcelSvcGnrl.ToolTip;
                    ViewState["TitInf"] = bO.Equals("TitInf") ? bT : ViewState["TitInf"];
                    ViewState["DocInf"] = bO.Equals("LblDoc") ? bT : ViewState["DocInf"];
                    ViewState["FrecInf"] = bO.Equals("FrecInf") ? bT : ViewState["FrecInf"];
                    ViewState["fechUCInf"] = bO.Equals("fechUCInf") ? bT : ViewState["fechUCInf"];
                    ViewState["InfOT"] = bO.Equals("InfOT") ? bT : ViewState["InfOT"];
                    ViewState["TitCumpInf"] = bO.Equals("TitCumpInf") ? bT : ViewState["TitCumpInf"];
                    ViewState["DatosEleInf"] = bO.Equals("DatosEleInf") ? bT : ViewState["DatosEleInf"];
                    ViewState["DatosHkInf"] = bO.Equals("DatosHkInf") ? bT : ViewState["DatosHkInf"];
                    ViewState["ServInf"] = bO.Equals("ServInf") ? bT : ViewState["ServInf"];
                    ViewState["GrupInf"] = bO.Equals("LblGrupo") ? bT : ViewState["GrupInf"];
                    ViewState["DiaInf"] = bO.Equals("DiaInf") ? bT : ViewState["DiaInf"];
                    ViewState["OrdenInf"] = bO.Equals("OrdenInf") ? bT : ViewState["OrdenInf"];
                    ViewState["ContInf2"] = bO.Equals("ContInf2") ? bT : ViewState["ContInf2"];
                    ViewState["VlrInf"] = bO.Equals("VlrInf") ? bT : ViewState["VlrInf"];
                    // **************** Configurar contador inicial HK  ***********************
                    BtnConfigContdrInic.Text = bO.Equals("BtnConfigContdrInic") ? bT : BtnConfigContdrInic.Text;
                    LblTitConfgIniCntd.Text = bO.Equals("BtnConfigContdrInic") ? bT : LblTitConfgIniCntd.Text;
                    IbtCloseConfIniCF.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseConfIniCF.ToolTip;
                    GrdConfInic.Columns[0].HeaderText = bO.Equals("GrdCont") ? bT : GrdConfInic.Columns[0].HeaderText;
                    GrdConfInic.Columns[1].HeaderText = bO.Equals("GrdFrec") ? bT : GrdConfInic.Columns[1].HeaderText;
                    GrdConfInic.Columns[2].HeaderText = bO.Equals("GrdDias") ? bT : GrdConfInic.Columns[2].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDeleteOnCl'");
                foreach (DataRow row in Result)
                { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            foreach (GridViewRow Row in GrdAdj.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null) { Row.Cells[2].Controls.Remove(imgE); }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[2].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdAeron.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[11].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[11].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdPN.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[6].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[6].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdSN.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[12].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[12].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdHKAsig.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[2].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[2].Controls.Remove(imgD);
                    }
                }
            }
            if ((int)ViewState["CE3"] == 0)
            {
                foreach (GridViewRow Row in GrdHKAsig.Rows)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        imgD.ToolTip = "No tiene permiso";
                    }
                }
                GrdHKAsig.ShowFooter = false;
            }
            foreach (GridViewRow Row in GrdRecursoF.Rows)
            {
                ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                if ((int)ViewState["VblModMS"] == 0)
                {
                    if (imgE != null)
                    {
                        Row.Cells[8].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
                if (CkbBloqRec.Checked == true)
                {
                    string VblText = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { VblText = row["Texto"].ToString().Trim(); }// El recurso se encuentra bloqueado
                    if (imgE != null)
                    {
                        imgE.Enabled = false; imgE.ToolTip = VblText;
                        imgD.Enabled = false; imgD.ToolTip = VblText;
                    }
                }
            }
            foreach (GridViewRow Row in GrdLicen.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[3].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
            TxtHistorico.Enabled = false;
            TxtHistorico.Text = "";
        }
        private decimal LRemanente, LRemanente1, LremanenteDia, LremanenteDia1, LCorridoDias, LCorridoDias1, LCorrido, LCorrido1;
        protected void Cumplimiento(int Id, decimal Ext, decimal ExtDia)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 24,'','','','','WEB',@Id,0,0, @ICC,'01-01-01','01-01-01','01-01-01'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Id", Id);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSTDdl = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DSTDdl);
                            DSTDdl.Tables[0].TableName = "Cumplimiento";
                            DSTDdl.Tables[1].TableName = "EstadoOT";

                            if (DSTDdl.Tables[0].Rows.Count > 0)
                            {
                                LRemanente = Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["Remanente"].ToString());
                                LRemanente1 = LRemanente + Ext;
                                LremanenteDia = Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["Remanente2"].ToString());
                                LremanenteDia1 = LremanenteDia + ExtDia;
                                LCorridoDias = Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["DiasCorridos"].ToString()); // Calcula de % actual de cumplimiento en dias
                                LCorridoDias1 = 100 - (LremanenteDia / Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["frec2"].ToString())) * 100; // Calcula de % actual de cumplimiento en dias
                                LCorrido = Math.Round(Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["Corrido"].ToString()), 2); // Calcula de % actual de cumplimiento
                                LCorrido1 = 100 - (LRemanente / Convert.ToDecimal(DSTDdl.Tables[0].Rows[0]["Frecu"].ToString())) * 100; // Calcula de % actual de cumplimiento
                                LCorrido1 = Math.Round(LCorrido1, 2);

                                if (LCorrido > LCorridoDias) // Si el porcentaje de corrido el servicio es mayor el valor que en dias
                                {
                                    if (LCorrido > 100)
                                    { LblCumplimi.Text = " Cump: 100%"; }
                                    else
                                    { LblCumplimi.Text = " Cump: " + Convert.ToString(LCorrido) + "%"; }
                                }
                                else
                                {
                                    if (LCorridoDias > 100)
                                    { LblCumplimi.Text = " Cump: 100%"; }
                                    else
                                    { LblCumplimi.Text = " Cump: " + Convert.ToString(LCorridoDias) + "%"; }
                                }
                                UpPnlCampos.Update();
                            }

                            if (DSTDdl.Tables[1].Rows.Count > 0)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + DSTDdl.Tables[1].Rows[0]["Mensj"].ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { TxtEstadoOT.Text = row["Texto"].ToString().Trim() + " " + DSTDdl.Tables[1].Rows[0]["OT"].ToString().Trim(); }
                            }
                        }
                    }
                }
            }
        }
        protected void TipoPantalla()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'LblTituloSMto'");
                    foreach (DataRow row in Result)
                    { TitForm.Text = row["Texto"].ToString().Trim(); }//Configuración Servicio de Mantenimiento
                }
                else
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'LblTituloSMtoRP'");
                    foreach (DataRow row in Result)
                    { TitForm.Text = row["Texto"].ToString().Trim(); } //Configuración Reparaciones Mayores";
                    LblCumplimi.Visible = false;
                    TxtHoriz.Visible = false;
                    TxtMatric.Visible = false;
                    TxtEtapa.Visible = false;
                    TxtActual.Visible = false;
                    CkbAD.Visible = false;
                    CkbSB.Visible = false;
                    CkbAplSub.Visible = false;
                    CkbVisuStat.Visible = false;
                    TxtSubAta.Visible = false;
                    TxtConsAta.Visible = false;
                    DdlTipo.Visible = false;
                    TxtRefOT.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "TipoPantalla", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void CorreccionDatos()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 14,'','','','','',0,0,0, @ICC,'01-01-01','01-01-01','01-01-01'");
                using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.ExecuteNonQuery();
                    }
                    catch (Exception Ex)
                    {
                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "CorreccionDatos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                    }
                }
            }
        }
        protected void BindDTraerdatos(string Prmtr, string Tipo, string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;
                string VbCatalogo = Session["PllaSrvManto"].ToString().Trim();
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string borr = Session["!dC!@"].ToString();

                        string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 5,@Catlgo,'','','','',0,0,@Idm,@ICC,'01-01-01','01-01-01','01-01-01'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Catlgo", VbCatalogo);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTDet = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTDet);
                                    DSTDet.Tables[0].TableName = "BusSvc";
                                    DSTDet.Tables[1].TableName = "Patron";
                                    DSTDet.Tables[2].TableName = "MOdelo";
                                    DSTDet.Tables[3].TableName = "Taller";
                                    DSTDet.Tables[4].TableName = "ATA";
                                    DSTDet.Tables[5].TableName = "Tipo";
                                    DSTDet.Tables[6].TableName = "DetSvcHK";
                                    DSTDet.Tables[7].TableName = "DetSvcPN";
                                    DSTDet.Tables[8].TableName = "DetSvcSN";
                                    DSTDet.Tables[9].TableName = "HKxSvc";
                                    DSTDet.Tables[10].TableName = "PNDdlGrdPN";
                                    DSTDet.Tables[11].TableName = "MaxCsctvo";
                                    DSTDet.Tables[12].TableName = "ConfIniCntdrHk";
                                    ViewState["DSTDet"] = DSTDet;
                                }
                            }
                        }
                    }
                }
                DSTDet = (DataSet)ViewState["DSTDet"];

                DataTable DTBusq = new DataTable();
                string VbCodAnt = @Prmtr;
                DTBusq = DSTDet.Tables[0].Clone();
                Result = DSTDet.Tables[0].Select("BadPlan='" + Tipo.Trim() + "' AND Catalogo='" + VbCatalogo + "'");
                foreach (DataRow SDR in Result)
                { DTBusq.ImportRow(SDR); }

                DdlBusq.DataSource = DTBusq;
                DdlBusq.DataTextField = "Servicio";
                DdlBusq.DataValueField = "IdSrvManto";
                DdlBusq.DataBind();
                DdlBusq.Text = VbCodAnt.Equals("0") ? @Prmtr : VbCodAnt;

                VbCodAnt = DdlGrupo.Text.Trim();
                DdlGrupo.DataSource = DSTDet.Tables[1];
                DdlGrupo.DataTextField = "Descripcion";
                DdlGrupo.DataValueField = "CodPatronManto";
                DdlGrupo.DataBind();
                DdlGrupo.Text = VbCodAnt;


                VbCodAnt = DdlModel.Text.Trim();
                DdlModel.DataSource = DSTDet.Tables[2];
                DdlModel.DataTextField = "NomModelo";
                DdlModel.DataValueField = "CodModelo";
                DdlModel.DataBind();
                DdlModel.Text = VbCodAnt;

                VbCodAnt = Ddltaller.Text.Trim();
                Ddltaller.DataSource = DSTDet.Tables[3];
                Ddltaller.DataTextField = "NomTaller";
                Ddltaller.DataValueField = "CodTaller";
                Ddltaller.DataBind();
                Ddltaller.Text = VbCodAnt;

                VbCodAnt = DdlAta.Text.Trim();
                DdlAta.DataSource = DSTDet.Tables[4];
                DdlAta.DataTextField = "Descripcion";
                DdlAta.DataValueField = "CodCapitulo";
                DdlAta.DataBind();
                DdlAta.Text = VbCodAnt;

                VbCodAnt = DdlTipo.Text.Trim();
                DdlTipo.DataSource = DSTDet.Tables[5];
                DdlTipo.DataTextField = "NomTipoSrv";
                DdlTipo.DataValueField = "IdTipoSrv";
                DdlTipo.DataBind();
                DdlTipo.Text = VbCodAnt;

                if (!Prmtr.Equals("0"))
                {
                    Result = DSTDet.Tables[0].Select("IdSrvManto = " + DdlBusq.Text.Trim());
                    foreach (DataRow SDR in Result)
                    {
                        CkbVisuStat.Checked = HttpUtility.HtmlDecode(SDR["VisualizarStatus"].ToString().Trim()) == "S" ? true : false;
                        TxtId.Text = SDR["IdSrvManto"].ToString();
                        TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodServicioManto"].ToString().Trim());
                        TxtDesc.Text = HttpUtility.HtmlDecode(SDR["Servicio"].ToString().Trim());
                        TxtHoriz.Text = SDR["HorizonteApertura"].ToString();
                        DdlGrupo.SelectedValue = SDR["CodPatronManto"].ToString().Trim();
                        if (DdlGrupo.SelectedValue.Trim().Equals("UCD"))
                        {
                            ViewState["UCD"] = 1;
                        }
                        else
                        {
                            ViewState["UCD"] = 0;
                        }
                        TxtEtapa.Text = SDR["NroEtapas"].ToString();
                        TxtActual.Text = SDR["EtapaActual"].ToString();
                        TxtDoc.Text = HttpUtility.HtmlDecode(SDR["Nrodocumento"].ToString().Trim());
                        TxtRefOT.Text = HttpUtility.HtmlDecode(SDR["Referencia"].ToString().Trim());
                        DdlModel.Text = HttpUtility.HtmlDecode(SDR["CodModeloSM"].ToString().Trim());
                        Ddltaller.Text = HttpUtility.HtmlDecode(SDR["CodTaller"].ToString().Trim());
                        CkbAD.Checked = HttpUtility.HtmlDecode(SDR["AD"].ToString().Trim()) == "S" ? true : false;
                        CkbSB.Checked = HttpUtility.HtmlDecode(SDR["SB"].ToString().Trim()) == "S" ? true : false;
                        CkbAplSub.Checked = HttpUtility.HtmlDecode(SDR["SubComponenteSM"].ToString().Trim()) == "S" ? true : false;
                        DdlAta.Text = HttpUtility.HtmlDecode(SDR["CodCapitulo"].ToString().Trim());
                        TxtSubAta.Text = SDR["SubAta"].ToString();
                        TxtConsAta.Text = SDR["ConsecutivoAta"].ToString();
                        DdlTipo.Text = HttpUtility.HtmlDecode(SDR["IdTipoSrv"].ToString().Trim());
                        CkbBloqRec.Checked = HttpUtility.HtmlDecode(SDR["ValidarRecurso"].ToString().Trim()) == "S" ? true : false;

                        switch (ViewState["TIPO"].ToString())
                        {
                            case "A":
                                BindDAK();
                                break;

                            default:
                                BindDPN();
                                BindDSN();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string VbMEns = ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + VbMEns + "')", true);
            }
        }
        protected void BindDataAll()
        {
            BindDHKAsig();
            BindDAdjunto();
            PerfilesGrid();
            if (ViewState["TIPO"].Equals("A")) { LblEtapa.Visible = true; TxtEtapa.Visible = true; TxtActual.Visible = true; }
            else { LblEtapa.Visible = false; TxtEtapa.Visible = false; TxtActual.Visible = false; }
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            if (!ViewState["TIPO"].ToString().Equals("S"))
            {
                IbtAdd.Enabled = In;
            }
            IbtUpdate.Enabled = Md;
            IbtDelete.Enabled = El;
            IbtFind.Enabled = Otr;
            IbtPrint.Enabled = Ip;
            IbtRecurso.Enabled = Otr;
            IbtGenerOT.Enabled = Otr;
            BtnAK.Enabled = Otr;
            BtnPN.Enabled = Otr;
            BtnSN.Enabled = Otr;
            GrdAeron.Enabled = Otr;
            GrdPN.Enabled = Otr;
            GrdSN.Enabled = Otr;
            GrdHKAsig.Enabled = Otr;
            GrdAdj.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            TxtDesc.Enabled = Edi;
            DdlGrupo.Enabled = Ing;
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            {
                CkbVisuStat.Enabled = Edi;
                TxtHoriz.Enabled = Edi;
                TxtRefOT.Enabled = Edi;
                CkbAD.Enabled = Edi;
                CkbSB.Enabled = Edi;
                if (!ViewState["TIPO"].ToString().Equals("A"))
                { CkbAplSub.Enabled = Edi; }
                else
                {
                    if (DdlGrupo.SelectedValue.Trim().Equals("SVC"))
                    {
                        TxtEtapa.Enabled = Edi;
                        TxtActual.Enabled = (int)ViewState["CE4"] == 0 ? false : Edi;
                    }
                }
                TxtSubAta.Enabled = Edi;
                TxtConsAta.Enabled = Edi;
                DdlTipo.Enabled = Edi;
            }
            TxtDoc.Enabled = Edi;
            DdlModel.Enabled = Edi;
            Ddltaller.Enabled = Edi;
            DdlAta.Enabled = Edi;
            DdlBusq.Enabled = Edi == true ? false : true;
            CkbBloqRec.Enabled = Edi;
        }
        protected void LimpiarCampos()
        {
            TxtId.Text = "";
            TxtCod.Text = "";
            TxtDesc.Text = "";
            TxtHoriz.Text = "";
            DdlGrupo.Text = "";
            TxtEtapa.Text = "";
            TxtActual.Text = "";
            TxtDoc.Text = "";
            DdlAta.Text = "";
            TxtRefOT.Text = "";
            DdlModel.Text = "";
            Ddltaller.Text = "";
            CkbAD.Checked = false;
            CkbSB.Checked = false;
            CkbAplSub.Checked = false;
            CkbVisuStat.Checked = false;
            DdlAta.Text = "";
            TxtSubAta.Text = "";
            TxtConsAta.Text = "";
            DdlTipo.Text = "0";
            TxtEstadoOT.Text = "";
            TxtMatric.Text = "";
            BtnConfigContdrInic.Visible = false;
        }
        protected void ValidarSvcManto(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                if (TxtDesc.Text.Trim().Equals(""))
                {
                    Idioma = (DataTable)ViewState["TablaIdioma"];

                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una descripción')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlGrupo.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un grupo')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarSvcManto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        {
            string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
            BindDTraerdatos(DdlBusq.Text, VbTpo, "SEL");
            UpPnlCampos.Update();
            BindDataAll();
            UpPnlPN.Update();
            PerfilesGrid();
        }
        protected void DdlGrupo_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            ViewState["UCM"] = 0;
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            {
                if (DdlGrupo.SelectedValue.Trim().Equals("SVC") && GrdAeron.Visible == true)
                { TxtEtapa.Enabled = true; TxtActual.Enabled = true; }
                else
                {
                    TxtEtapa.Enabled = false; TxtActual.Enabled = false;
                    TxtEtapa.Text = "0"; TxtActual.Text = "0";
                }
            }
        }
        protected void DdlHKPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlHKPP = (GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','','','','CON',{1},0,0,{2},'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlHKPP.SelectedValue, Session["!dC!@"]);
            DropDownList DdlContHKPP = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList);
            DdlContHKPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContHKPP.DataTextField = "CodContador";
            DdlContHKPP.DataValueField = "Cod";
            DdlContHKPP.DataBind();
            return;
        }
        protected void DdlContHKPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlContHKPP = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList);
            TextBox TxtNumDiaPP = (GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox);
            TextBox TxtExtDiaPP = (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox);
            TxtNumDiaPP.Enabled = true;
            TxtExtDiaPP.Enabled = true;

            if (DdlContHKPP.SelectedValue.Trim().Equals("CAL") || DdlContHKPP.SelectedValue.Trim().Equals("CTI"))
            {
                TxtNumDiaPP.Enabled = false;
                TxtNumDiaPP.Text = "0";
                TxtExtDiaPP.Enabled = false;
                TxtExtDiaPP.Text = "0";
            }
        }
        protected void DdlPNPP_TextChanged(object sender, EventArgs e)
        {
            DSTDet = (DataSet)ViewState["DSTDet"];
            PerfilesGrid();
            DropDownList DdlPNPP = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','CONPN',0,0,0,{2},'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlPNPP.Text.Trim(), Session["!dC!@"]);
            DropDownList DdlContPNPP = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList);
            DdlContPNPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContPNPP.DataTextField = "CodContador";
            DdlContPNPP.DataValueField = "Cod";
            DdlContPNPP.DataBind();

            DataRow[] Result = DSTDet.Tables[10].Select("PN = '" + DdlPNPP.Text.Trim() + "'");
            foreach (DataRow SDR in Result)
            { (GrdPN.FooterRow.FindControl("TxtDescPnPP") as TextBox).Text = SDR["Descripcion"].ToString(); }
        }
        protected void DdlContPNPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlContPNPP = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList);
            TextBox TxtNumDiaPNPP = (GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox);
            TxtNumDiaPNPP.Enabled = true;

            if (DdlContPNPP.SelectedValue.Trim().Equals("CAL") || DdlContPNPP.SelectedValue.Trim().Equals("CTI"))
            {
                TxtNumDiaPNPP.Enabled = false;
                TxtNumDiaPNPP.Text = "0";
            }
        }
        protected void TxtSubAta_TextChanged(object sender, EventArgs e)
        {
            DSTDet = (DataSet)ViewState["DSTDet"];
            DataRow[] Result = DSTDet.Tables[11].Select("SubAta = '" + TxtSubAta.Text.Trim() + "'");
            if (Result.Length == 0)
            { TxtConsAta.Text = "0"; }
            else
            {
                foreach (DataRow SDR in Result)
                { TxtConsAta.Text = SDR["MAXI"].ToString(); }
            }
        }
        protected void IbtAdd_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["TipoAccion"].ToString().Equals(""))
            {
                IbtAdd.ImageUrl = "~/images/SaveV2.png";
                ActivarBotones(true, false, false, false, false);
                ViewState["TipoAccion"] = "Ingresar";
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { IbtAdd.ToolTip = row["Texto"].ToString().Trim(); }
                ActivarCampos(true, true, "Ingresar");
                LimpiarCampos();
                BindDataAll();
                BindDAK();
                BindDPN();
                BindDSN();
                DdlBusq.SelectedValue = "0";
                CkbVisuStat.Checked = true;
                Result = Idioma.Select("Objeto= 'IbtAddOnCl'");
                foreach (DataRow row in Result)
                { IbtAdd.OnClientClick = row["Texto"].ToString().Trim(); };
            }
            else
            {
                try
                {
                    ValidarSvcManto("Ingresar");
                    if (ViewState["Validar"].ToString() == "N")
                    {
                        BindDataAll();
                        return;
                    }
                    List<CsTypeServicioManto> ObjTSM = new List<CsTypeServicioManto>();
                    var detail = new CsTypeServicioManto()
                    {
                        IdSrvManto = 0,
                        CodServicioManto = "77NEO",
                        CodPatronManto = DdlGrupo.Text,
                        Descripcion = TxtDesc.Text.Trim(),
                        NroDocumento = TxtDoc.Text.Trim(),
                        CodCapitulo = DdlAta.SelectedValue,
                        BadPlan = ViewState["TIPO"].ToString().Equals("A") ? "" : "P",
                        Bandera = ViewState["TIPO"].ToString().Equals("A") ? "A" : "E",
                        BanTipoSrv = 0, //este campo tiene que ver si tiene ot cerradas y el detalle banderaOT sigue con valor 1 0 2
                        Usu = Session["C77U"].ToString(),
                        NroEtapas = TxtEtapa.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtEtapa.Text),
                        EtapaActual = TxtActual.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtActual.Text),
                        SubAta = TxtSubAta.Text.Trim(),
                        ConsecutivoAta = TxtConsAta.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtConsAta.Text),
                        IdTipoSrv = Convert.ToInt32(DdlTipo.SelectedValue),
                        AD = CkbAD.Checked == true ? 1 : 0,
                        SB = CkbSB.Checked == true ? 1 : 0,
                        HorizonteApertura = TxtHoriz.Text.Trim().Equals("") ? 0 : Convert.ToDouble(TxtHoriz.Text),
                        Referencia = TxtRefOT.Text.Trim(),
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = CkbAplSub.Checked == true ? 1 : 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = Session["PllaSrvManto"].ToString(),
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = 1,//CkbVisuStat.Checked == true ? 1 : 0,
                        ServicioMayor = "",
                        Accion = "INSERT",
                        Aplicabilidad = ViewState["TIPO"].ToString(),
                    };
                    ObjTSM.Add(detail);
                    CsTypeServicioManto TblServicioManto = new CsTypeServicioManto();
                    TblServicioManto.Alimentar(ObjTSM);
                    int VblIdSvcManto = TblServicioManto.GetID();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 16,'{0}','','','','',{1},0,0,@CC,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), VblIdSvcManto);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Servicio", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                    IbtAdd.ImageUrl = "~/images/AddNew.png";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAdd'");
                    foreach (DataRow row in Result)
                    { IbtAdd.ToolTip = row["Texto"].ToString().Trim(); }
                    IbtAdd.ToolTip = "Ingresar";
                    ViewState["TipoAccion"] = "";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    IbtAdd.OnClientClick = "";
                    BindDTraerdatos(VblIdSvcManto.ToString(), ViewState["TIPO"].ToString(), "UPD");
                    switch (ViewState["TIPO"].ToString())
                    {
                        case "A":
                            BindDAK();
                            break;
                        case "P":
                        default:
                            BindDPN();
                            break;
                    }
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtUpdate_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["TipoAccion"].ToString().Equals(""))
            {
                if (!TxtCod.Text.Trim().Equals(""))
                {
                    IbtUpdate.ImageUrl = "~/images/SaveV2.png";
                    ActivarBotones(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                    ViewState["TipoAccion"] = "Modificar";
                    ActivarCampos(false, true, "Modificar");
                    Result = Idioma.Select("Objeto= 'IbtUpdateOnCl'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.OnClientClick = row["Texto"].ToString().Trim(); };
                }
            }
            else
            {
                try
                {
                    ValidarSvcManto("Modificar");
                    if (ViewState["Validar"].ToString() == "N")
                    {
                        BindDataAll();
                        return;
                    }
                    List<CsTypeServicioManto> ObjTSM = new List<CsTypeServicioManto>();
                    var detail = new CsTypeServicioManto()
                    {
                        IdSrvManto = Convert.ToInt32(TxtId.Text),
                        CodServicioManto = TxtCod.Text.Trim(),
                        CodPatronManto = DdlGrupo.Text.Trim(),
                        Descripcion = TxtDesc.Text.Trim(),
                        NroDocumento = TxtDoc.Text.Trim(),
                        CodCapitulo = DdlAta.SelectedValue,
                        BadPlan = ViewState["TIPO"].ToString().Equals("A") ? "" : "P",
                        Bandera = ViewState["TIPO"].ToString().Equals("A") ? "A" : "E",
                        BanTipoSrv = 0, //este campo tiene que ver si tiene ot cerradas y el detalle banderaOT sigue con valor 1 0 2
                        Usu = Session["C77U"].ToString(),
                        NroEtapas = TxtEtapa.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtEtapa.Text),
                        EtapaActual = TxtActual.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtActual.Text),
                        SubAta = TxtSubAta.Text.Trim(),
                        ConsecutivoAta = TxtConsAta.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtConsAta.Text),
                        IdTipoSrv = Convert.ToInt32(DdlTipo.SelectedValue),
                        AD = CkbAD.Checked == true ? 1 : 0,
                        SB = CkbSB.Checked == true ? 1 : 0,
                        HorizonteApertura = TxtHoriz.Text.Trim().Equals("") ? 0 : Convert.ToDouble(TxtHoriz.Text),
                        Referencia = TxtRefOT.Text.Trim(),
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = CkbAplSub.Checked == true ? 1 : 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = Session["PllaSrvManto"].ToString(),
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = CkbVisuStat.Checked == true ? 1 : 0,
                        ServicioMayor = "",
                        Accion = "UPDATE",
                        Aplicabilidad = ViewState["TIPO"].ToString(),
                    };
                    ObjTSM.Add(detail);
                    CsTypeServicioManto TblServicioManto = new CsTypeServicioManto();
                    TblServicioManto.Alimentar(ObjTSM);
                    //IbtUpdate.CssClass = "BtnImagenUpdate";
                    IbtUpdate.ImageUrl = "~/images/Edit.png";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                    ViewState["TipoAccion"] = "";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    IbtUpdate.OnClientClick = "";
                    BindDTraerdatos(TxtId.Text.ToString(), ViewState["TIPO"].ToString().Equals("A") ? "" : "P", "UPD");
                    switch (ViewState["TIPO"].ToString())
                    {
                        case "A":
                            BindDAK();
                            break;
                        case "P":
                        default:
                            BindDPN();
                            break;
                    }
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrMod'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtFind_Click(object sender, ImageClickEventArgs e)
        {
            PnlCampos.Visible = false;
            PnlBusq.Visible = true;

            if (ViewState["TIPO"].ToString().Equals("A"))
            {
                TblBusqHK.Visible = true;
                TblBusqPN.Visible = false;
                TblBusqSN.Visible = false;
            }
            if (ViewState["TIPO"].ToString().Equals("P"))
            {
                TblBusqHK.Visible = false;
                TblBusqPN.Visible = true;
                TblBusqSN.Visible = false;
            }
            if (ViewState["TIPO"].ToString().Equals("S"))
            {
                TblBusqHK.Visible = false;
                TblBusqPN.Visible = false;
                TblBusqSN.Visible = true;
            }
            BIndDataBusq(TxtBusqueda.Text);
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtPrint_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string Titulo_InfSvc = "", VbMensj = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitInfSvc'");
            foreach (DataRow row in Result)
            { Titulo_InfSvc = row["Texto"].ToString().Trim() + ": "; }//Informes de servicios

            Result = Idioma.Select("Objeto= 'Mens29SM'");
            foreach (DataRow row in Result)
            { VbMensj = row["Texto"].ToString().Trim(); }//Debe seleccionar un detalle

            switch (ViewState["TIPO"].ToString())
            {
                case "A":
                    if (TxtMatric.Text.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    TitInfSvc.Text = Titulo_InfSvc + TxtMatric.Text;
                    break;
                case "P":
                    if (ViewState["PN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    TitInfSvc.Text = Titulo_InfSvc + ViewState["PN"].ToString();
                    break;
                default:
                    if (ViewState["SN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    TitInfSvc.Text = Titulo_InfSvc + ViewState["PN"].ToString() + " | " + ViewState["SN"].ToString();
                    break;
            }
            PnlCampos.Visible = false;
            PnlInforme.Visible = true;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtDelete_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtId.Text.Trim().Equals(""))
                { return; }
                string VBQuery;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 6, @Cd, @Dsc, @Tp, @Pll, @Us,'','','','',@I,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim());
                                SC.Parameters.AddWithValue("@Dsc", TxtDesc.Text.Trim());
                                SC.Parameters.AddWithValue("@Tp", ViewState["TIPO"].ToString().Trim());
                                SC.Parameters.AddWithValue("@Pll", Session["PllaSrvManto"].ToString().Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString().Trim());
                                SC.Parameters.AddWithValue("@I", TxtId.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }

                                Transac.Commit();
                                string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                                BindDTraerdatos("0", VbTpo, "UPD");
                                BindDataAll();
                                LimpiarCampos();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación'
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR  SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void IbtRecurso_Click(object sender, ImageClickEventArgs e)
        {
            if (!TxtId.Text.Trim().Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                BindDRecursoF("UPD");
                BindDLicencia();
                PnlCampos.Visible = false;
                PnlRecursos.Visible = true;
                if (CkbBloqRec.Checked == true)
                {
                    GrdRecursoF.FooterRow.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { GrdRecursoF.FooterRow.ToolTip = row["Texto"].ToString().Trim(); }// "El recurso se encuentra bloqueado";
                }
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtGenerOT_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtId.Text.Trim().Equals(""))
            {
                return;
            }
            if (!ViewState["TIPO"].ToString().Equals("P"))
            {
                if (TxtMatric.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens19SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un registro del detalle para obtener la matrícula 
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 7, @Hk, @Grp,@CdSvc, @Tp, @Pll,'','','','',@IdElem,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Hk", TxtMatric.Text.Trim());
                                SC.Parameters.AddWithValue("@Grp", DdlGrupo.Text.Trim());
                                SC.Parameters.AddWithValue("@CdSvc", TxtCod.Text.Trim());
                                SC.Parameters.AddWithValue("@Tp", ViewState["TIPO"]);
                                SC.Parameters.AddWithValue("@Pll", Session["PllaSrvManto"]);
                                SC.Parameters.AddWithValue("@IdElem", ViewState["IdCodElem"]);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);


                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                            }
                            catch (Exception Ex)
                            {

                                DataRow[] Result = Idioma.Select("Objeto= 'Mens22SM'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de generación orden de trabajo')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "GENERAR OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        // ****************Detalle Aeronave ***********************
        protected void BindDAK()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDet = (DataSet)ViewState["DSTDet"];
                DataRow[] Result;
                DataTable DT = new DataTable();
                DT = DSTDet.Tables[6].Clone();
                Result = DSTDet.Tables[6].Select("CodServicioManto ='" + TxtCod.Text.Trim() + "'");
                foreach (DataRow DR in Result)
                {
                    DT.ImportRow(DR);
                }
                if (DT.Rows.Count > 0)
                {
                    DataView DV = DT.DefaultView;
                    DV.Sort = "Matricula";
                    DT = DV.ToTable();
                    GrdAeron.DataSource = DT;
                    GrdAeron.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdAeron.DataSource = DT;
                    GrdAeron.DataBind();
                    GrdAeron.Rows[0].Cells.Clear();
                    GrdAeron.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdAeron.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdAeron.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDAK", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ValidarHK(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                string VBQuery;

                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["CodHK"].ToString().Trim().Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens07SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una aeronave')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un contador')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (TxtHistorico.Enabled == true && TxtHistorico.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens09SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["FrecIni"].ToString().Trim().Equals("0") && ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 7, @Ct, @Cd,'','','HK',@Rst, @HK, @Frc,@ICC, '01-01-01','01-01-01','01-01-01'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@Ct", ViewState["Cntdr"]);
                        SC.Parameters.AddWithValue("@Cd", TxtCod.Text);
                        SC.Parameters.AddWithValue("@Rst", ViewState["Reset"]);
                        SC.Parameters.AddWithValue("@HK", ViewState["CodHK"]);
                        SC.Parameters.AddWithValue("@Frc", ViewState["Frec"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataReader DAE = SC.ExecuteReader();
                        if (DAE.Read())
                        {
                            string Mensj = DAE["Mensj"].ToString().Trim();
                            DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { Mensj = row["Texto"].ToString().Trim(); }

                            ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                            ViewState["Validar"] = "N";
                            return;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarHK", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnAK_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-primary";
            BtnPN.CssClass = "btn btn-outline-primary";
            BtnSN.CssClass = "btn btn-outline-primary";
            ViewState["TIPO"] = "A";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            BindDTraerdatos("0", "", "SEL");
            GrdAeron.Visible = true;
            GrdPN.Visible = false;
            GrdSN.Visible = false;
            LblAkAsing.Visible = false;
            GrdHKAsig.Visible = false;

            if ((int)ViewState["VblIngMS"] == 1)
            { BtnConfigContdrInic.Visible = true; }
            IbtAdd.Enabled = true;
            LimpiarCampos();
            BindDataAll();
            PerfilesGrid();
        }
        protected void GrdAeron_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.CommandName.Equals("AddNew"))
                {
                    while ((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Contains("-"))
                    {
                        (GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text = (GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Replace("-", "");
                    }
                    if ((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Ext"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["Ext"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim()) * -1;
                    }

                    while ((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Contains("-"))
                    {
                        (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text = (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Replace("-", "");
                    }
                    if ((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["ExtDia"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["ExtDia"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim()) * -1;
                    }
                    ViewState["CodHK"] = Convert.ToInt32((GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList).SelectedValue.Trim());
                    ViewState["Cntdr"] = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Reset"] = (GrdAeron.FooterRow.FindControl("CkbResetPP") as CheckBox).Checked == true ? 1 : 0;
                    if ((GrdAeron.FooterRow.FindControl("TxtFrecIniPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["FrecIni"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["FrecIni"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtFrecIniPP") as TextBox).Text.Trim());
                    }

                    if ((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Frec"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["Frec"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim());
                    }

                    if ((GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["NroDia"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["NroDia"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox).Text.Trim());
                    }

                    if (!(GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["FechaVenc"] = Convert.ToDateTime((GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim());
                        DateTime borrar = (DateTime)ViewState["FechaVenc"];
                    }
                    // validar
                    ValidarHK("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    {
                        BindDataAll();
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                        var Detail = new CsTypContaSrvMant()
                        {
                            CodIdContaSrvManto = 0,
                            CodAeronave = (int)ViewState["CodHK"],
                            CodElemento = null,
                            CodServicioManto = TxtCod.Text.Trim(),
                            Frecuencia = (double)ViewState["Frec"],
                            Extension = (double)ViewState["Ext"],
                            FechaVencimiento = (GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                            NroDias = (double)ViewState["NroDia"],
                            ExtensionDias = (double)ViewState["ExtDia"],
                            BanOrdenTrabajo = 0,
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            CodOt = null,
                            Compensacion = 0,
                            Resetear = (int)ViewState["Reset"],
                            FrecuenciaInicial = (double)ViewState["FrecIni"],
                            FrecuenciaInicalEjecutada = 0,
                            CodContador = ViewState["Cntdr"].ToString(),
                            CodElem = "",
                            PN = "",
                            Accion = "INSERT",
                            Aplicabilidad = "HK",
                            CrearHistorico = "N",
                            Historico = "",
                        };
                        ObjTypContaSM.Add(Detail);
                        CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                        ContaSrvMant.Alimentar(ObjTypContaSM);
                        string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                        BindDTraerdatos("0", "", "UPD");
                        BindDataAll();
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["IdCodElem"] = -1;
                foreach (GridViewRow Row in GrdAeron.Rows)
                {
                    if (Row.RowIndex == GrdAeron.SelectedIndex)
                    {

                        Row.Style["background-color"] = "#D4DAD3";
                        Row.Attributes["onclick"] = "";

                        Label ext = Row.FindControl("LblExt") as Label;
                        Label extDia = Row.FindControl("LblExtDia") as Label;
                        if (ext != null)
                        {
                            decimal VbExt = Convert.ToDecimal(ext.Text);
                            decimal VbExtD = Convert.ToDecimal(extDia.Text);
                            int VbID = Convert.ToInt32(GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][0].ToString());
                            TxtMatric.Text = GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][1].ToString();
                            Cumplimiento(VbID, VbExt, VbExtD);
                        }
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0)
                        {
                            Row.Style["background-color"] = "white";
                        }
                        else
                        {
                            Row.Style["background-color"] = "#cae4ff";
                        }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + Row.RowIndex);

                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SELECT DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdAeron.EditIndex = e.NewEditIndex; BindDAK(); }
        protected void GrdAeron_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());
                if ((GrdAeron.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true)
                { TxtHistorico.Enabled = true; }
                else
                { TxtHistorico.Enabled = false; TxtHistorico.Text = ""; }
                while ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text = (GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Ext"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Ext"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim()) * -1;
                }
                while ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text = (GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Equals(""))
                { ViewState["ExtDia"] = Convert.ToDouble(0); }
                else
                { ViewState["ExtDia"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim()) * -1; }
                ViewState["CodHK"] = Convert.ToInt32((GrdAeron.Rows[e.RowIndex].FindControl("DdlHK") as DropDownList).SelectedValue.Trim());
                ViewState["Cntdr"] = (GrdAeron.Rows[e.RowIndex].FindControl("DdlCont") as DropDownList).SelectedValue.Trim();
                ViewState["Reset"] = (GrdAeron.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim().Equals(""))
                { ViewState["FrecIni"] = Convert.ToDouble(0); }
                else
                { ViewState["FrecIni"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim()); }

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                { ViewState["Frec"] = Convert.ToDouble(0); }
                else
                { ViewState["Frec"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim()); }

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim().Equals(""))
                { ViewState["NroDia"] = Convert.ToDouble(0); }
                else
                { ViewState["NroDia"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim()); }

                if (!(GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals(""))
                { ViewState["FechaVenc"] = Convert.ToDateTime((GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim()); }
                // validar
                ValidarHK("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                {
                    BindDataAll();
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                    var Detail = new CsTypContaSrvMant()
                    {
                        CodIdContaSrvManto = VblId,
                        CodAeronave = (int)ViewState["CodHK"],
                        CodElemento = null,
                        CodServicioManto = TxtCod.Text.Trim(),
                        Frecuencia = (double)ViewState["Frec"],
                        Extension = (double)ViewState["Ext"],
                        FechaVencimiento = (GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                        NroDias = (double)ViewState["NroDia"],
                        ExtensionDias = (double)ViewState["ExtDia"],
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = (int)ViewState["Reset"],
                        FrecuenciaInicial = (double)ViewState["FrecIni"],
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = "",
                        PN = "",
                        Accion = "UPDATE",
                        Aplicabilidad = "HK",
                        CrearHistorico = (GrdAeron.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true ? "S" : "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdAeron.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDTraerdatos(DdlBusq.Text.Trim(), "", "UPD");
                    BindDataAll();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAeron.EditIndex = -1; BindDAK(); ; }
        protected void GrdAeron_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 8,'','','','','VALIDA',{0},0,0, @ICC,'01-01-01','01-01-01','01-01-01'", IDContaSrvManto);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 8,'','','', @Us,'DELETE',@I,0,0, @ICC,'01-01-01','01-01-01','01-01-01'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Us", Session["C77U"]);
                                sqlCmd.Parameters.AddWithValue("@I", IDContaSrvManto);
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDTraerdatos(DdlBusq.Text.Trim(), "", "UPD");
                                BindDataAll();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                string LtxtSql = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','HK',0,0,0,{2},'01-01-01','01-01-01','01-01-01'", DdlModel.SelectedValue, TxtCod.Text, Session["!dC!@"]);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlHKPP = (e.Row.FindControl("DdlHKPP") as DropDownList);
                    DdlHKPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlHKPP.DataTextField = "Matricula";
                    DdlHKPP.DataValueField = "CodAeronave";
                    DdlHKPP.DataBind();
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrecI = (e.Row.FindControl("TxtFrecIniPP") as TextBox);
                        TxtFrecI.ReadOnly = true;
                        TxtFrecI.Enabled = false;
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPP") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtExt = (e.Row.FindControl("TxtExtPP") as TextBox);
                        TxtExt.ReadOnly = true;
                        TxtExt.Enabled = false;
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPP") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        TextBox TxtED = (e.Row.FindControl("TxtExtDiaPP") as TextBox);
                        TxtED.ReadOnly = true;
                        TxtED.Enabled = false;
                        ImageButton BtnFech = (e.Row.FindControl("IbtFechaPP") as ImageButton);
                        BtnFech.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbResetPP") as CheckBox);
                        CkRest.Enabled = false;
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','HKMOD',0,0,0,{0},'01-01-01','01-01-01','01-01-01'", Session["!dC!@"]);
                    DropDownList DdlHK = (e.Row.FindControl("DdlHK") as DropDownList);
                    DdlHK.DataSource = Cnx.DSET(LtxtSql);
                    DdlHK.DataTextField = "Matricula";
                    DdlHK.DataValueField = "CodAeronave";
                    DdlHK.DataBind();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DdlHK.SelectedValue = dr["CodHK"].ToString();

                    LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','CONMOD',{0},0,0,{1},'01-01-01','01-01-01','01-01-01'", dr["CodHK"].ToString(), Session["!dC!@"]);
                    DropDownList DdlCont = (e.Row.FindControl("DdlCont") as DropDownList);
                    DdlCont.DataSource = Cnx.DSET(LtxtSql);
                    DdlCont.DataTextField = "CodContador";
                    DdlCont.DataValueField = "Cod";
                    DdlCont.DataBind();
                    DataRowView DRVC = e.Row.DataItem as DataRowView;
                    DdlCont.SelectedValue = DRVC["CodContador"].ToString();
                    if (DdlCont.SelectedValue.Trim().Equals("CAL") || DdlCont.SelectedValue.Trim().Equals("CTI"))
                    {
                        TextBox TxtNumDia = (e.Row.FindControl("TxtNumDia") as TextBox);
                        TxtNumDia.Enabled = false;
                        TxtNumDia.Text = "0";
                        TextBox TxtExtDia = (e.Row.FindControl("TxtExtDia") as TextBox);
                        TxtExtDia.Enabled = false;
                        TxtExtDia.Text = "0";
                    }
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrecI = (e.Row.FindControl("TxtFrecIni") as TextBox);
                        TxtFrecI.ReadOnly = true;
                        TxtFrecI.Enabled = false;
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrec") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtExt = (e.Row.FindControl("TxtExt") as TextBox);
                        TxtExt.ReadOnly = true;
                        TxtExt.Enabled = false;
                        TextBox TxtND = (e.Row.FindControl("TxtNumDia") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        TextBox TxtED = (e.Row.FindControl("TxtExtDia") as TextBox);
                        TxtED.ReadOnly = true;
                        TxtED.Enabled = false;
                        ImageButton BtnFech = (e.Row.FindControl("IbtFecha") as ImageButton);
                        BtnFech.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbReset") as CheckBox);
                        CkRest.Enabled = false;
                        CheckBox CkbHist = (e.Row.FindControl("CkbHist") as CheckBox);
                        CkbHist.Visible = false;
                    }
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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + e.Row.RowIndex);
                    DataRow[] Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// Seleccione el registro.

                    ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }
                }
            }
        }
        protected void GrdAeron_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAeron.PageIndex = e.NewPageIndex;
            BindDataAll();
            BindDAK();
        }
        // ****************Detalle P/N ***********************
        protected void BindDPN()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDet = (DataSet)ViewState["DSTDet"];
                DataRow[] Result;
                DataTable DT = new DataTable();
                DT = DSTDet.Tables[7].Clone();
                Result = DSTDet.Tables[7].Select("CodServicioManto ='" + TxtCod.Text.Trim() + "'");
                foreach (DataRow DR in Result)
                {
                    DT.ImportRow(DR);
                }
                if (DT.Rows.Count > 0)
                {
                    DataView DV = DT.DefaultView;
                    DV.Sort = "PN";
                    DT = DV.ToTable();
                    GrdPN.DataSource = DT;
                    GrdPN.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdPN.DataSource = DT;
                    GrdPN.DataBind();
                    GrdPN.Rows[0].Cells.Clear();
                    GrdPN.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdPN.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdPN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDAK", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ValidarDetPN(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                string VBQuery;

                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["PN"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens14SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un P/N')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un contador')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 9, @Ct,@P, @Cd,'','VALIDA',@Rst, @Frc,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@Ct", ViewState["Cntdr"]);
                        SC.Parameters.AddWithValue("@P", ViewState["PN"]);
                        SC.Parameters.AddWithValue("@Cd", TxtCod.Text);
                        SC.Parameters.AddWithValue("@Rst", ViewState["Reset"]);
                        SC.Parameters.AddWithValue("@Frc", ViewState["Frec"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataReader DAE = SC.ExecuteReader();
                        if (DAE.Read())
                        {
                            string Mensj = DAE["Mensj"].ToString().Trim();
                            DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { Mensj = row["Texto"].ToString().Trim(); }

                            ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                            ViewState["Validar"] = "N";
                            return;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarPN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnPN_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-outline-primary";
            BtnPN.CssClass = "btn btn-primary";
            BtnSN.CssClass = "btn btn-outline-primary";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            GrdAeron.Visible = false;
            GrdPN.Visible = true;
            GrdSN.Visible = false;
            LblAkAsing.Visible = true;
            GrdHKAsig.Visible = true;
            BtnConfigContdrInic.Visible = false;
            IbtAdd.Enabled = true;
            if (ViewState["TIPO"].ToString().Equals("A")) { LimpiarCampos(); BindDTraerdatos("0", "P", "SEL"); }
            else { BindDTraerdatos(DdlBusq.Text, "P", "SEL"); }
            ViewState["TIPO"] = "P";
            BindDataAll();
            PerfilesGrid();
        }
        protected void GrdPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtCod.Text.Equals(""))
                {
                    return;
                }
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    ViewState["PN"] = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Cntdr"] = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Reset"] = (GrdPN.FooterRow.FindControl("CkbResetPP") as CheckBox).Checked == true ? 1 : 0;

                    if ((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Frec"] = Convert.ToInt32(0);
                    }
                    else
                    {
                        ViewState["Frec"] = Convert.ToInt32((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim());
                    }

                    if ((GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["NroDia"] = Convert.ToInt32(0);
                    }
                    else
                    {
                        ViewState["NroDia"] = Convert.ToInt32((GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox).Text.Trim());
                    }

                    // validar
                    ValidarDetPN("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    {
                        BindDataAll();
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        List<TypeContSrvPn> ObjContSrvPn = new List<TypeContSrvPn>();
                        var Detail = new TypeContSrvPn()
                        {
                            CodidcodSrvPn = 0,
                            CodServicioManto = TxtCod.Text,
                            CodIdContadorPn = 0,
                            Frecuencia = (int)ViewState["Frec"],
                            NroDias = (int)ViewState["NroDia"],
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            Resetear = (int)ViewState["Reset"],
                            Accion = "INSERT",
                            PN = ViewState["PN"].ToString(),
                            CodContador = ViewState["Cntdr"].ToString(),
                        };
                        ObjContSrvPn.Add(Detail);
                        TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                        ContSrvPn.Alimentar(ObjContSrvPn);
                        BindDTraerdatos("0", "P", "UPD");
                        BindDataAll();
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET PN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow Row in GrdPN.Rows)
            {
                if (Row.RowIndex == GrdPN.SelectedIndex)
                {
                    Row.Style["background-color"] = "#D4DAD3";
                    Row.Attributes["onclick"] = "";
                    ViewState["PN"] = GrdPN.DataKeys[this.GrdPN.SelectedIndex][2].ToString();
                }
                else
                {
                    if (Row.RowIndex % 2 == 0)
                    {
                        Row.Style["background-color"] = "white";
                    }
                    else
                    {
                        Row.Style["background-color"] = "#cae4ff";
                    }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + Row.RowIndex);
                }
            }
        }
        protected void GrdPN_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdPN.EditIndex = e.NewEditIndex; BindDPN(); }
        protected void GrdPN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                int VblId = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());
                int VbIdContPN = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Values["CodIdContadorPn"].ToString());
                PerfilesGrid();
                ViewState["PN"] = (GrdPN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();
                ViewState["Cntdr"] = (GrdPN.Rows[e.RowIndex].FindControl("LblContPN") as Label).Text.Trim();
                ViewState["Reset"] = (GrdPN.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Frec"] = Convert.ToInt32(0);
                }
                else
                {
                    ViewState["Frec"] = Convert.ToInt32((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim());
                }

                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtNumDiaPN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["NroDia"] = Convert.ToInt32(0);
                }
                else
                {
                    ViewState["NroDia"] = Convert.ToInt32((GrdPN.Rows[e.RowIndex].FindControl("TxtNumDiaPN") as TextBox).Text.Trim());
                }

                // validar
                ValidarDetPN("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                {
                    BindDataAll();
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<TypeContSrvPn> ObjContSrvPn = new List<TypeContSrvPn>();
                    var Detail = new TypeContSrvPn()
                    {
                        CodidcodSrvPn = VblId,
                        CodServicioManto = TxtCod.Text,
                        CodIdContadorPn = VbIdContPN,
                        Frecuencia = (int)ViewState["Frec"],
                        NroDias = (int)ViewState["NroDia"],
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        Resetear = (int)ViewState["Reset"],
                        Accion = "UPDATE",
                        PN = ViewState["PN"].ToString(),
                        CodContador = ViewState["Cntdr"].ToString(),
                    };
                    ObjContSrvPn.Add(Detail);
                    TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                    ContSrvPn.Alimentar(ObjContSrvPn);
                    GrdPN.EditIndex = -1;
                    BindDTraerdatos(DdlBusq.Text.Trim(), "P", "UPD");
                    BindDataAll();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la edicion')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET PN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdPN.EditIndex = -1; BindDPN(); }
        protected void GrdPN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VBQuery;
                int CodidcodSrvPn = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','','','','VALIDA',{0},0,0,{2},'01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text, Session["!dC!@"]);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','{2}','','','DELETE',{0},0,0,@ICC,'01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text, Session["C77U"].ToString());
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDTraerdatos(DdlBusq.Text.Trim(), "P", "UPD");
                                BindDataAll();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET P/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (!TxtCod.Text.Equals(""))
            {
                DSTDet = (DataSet)ViewState["DSTDet"];
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlPNPP = (e.Row.FindControl("DdlPNPP") as DropDownList);
                    DdlPNPP.DataSource = DSTDet.Tables[10];
                    DdlPNPP.DataTextField = "PN";
                    DdlPNPP.DataValueField = "CodPN";
                    DdlPNPP.DataBind();
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPNPP") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPNPP") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbResetPP") as CheckBox);
                        CkRest.Enabled = false;
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPN") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPN") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbReset") as CheckBox);
                        CkRest.Enabled = false;
                    }
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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + e.Row.RowIndex);
                    DataRow[] Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// 

                    ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }
                }
            }
        }
        protected void GrdPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAeron.PageIndex = e.NewPageIndex; BindDataAll(); BindDPN(); }
        // ********************************* Detalle S/N *********************************
        protected void BindDSN()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDet = (DataSet)ViewState["DSTDet"];
                DataRow[] Result;
                DataTable DT = new DataTable();
                DT = DSTDet.Tables[8].Clone();
                Result = DSTDet.Tables[8].Select("CodServicioManto ='" + TxtCod.Text.Trim() + "'");
                foreach (DataRow DR in Result)
                {
                    DT.ImportRow(DR);
                }
                if (DT.Rows.Count > 0)
                {
                    DataView DV = DT.DefaultView;
                    DV.Sort = "PN,SN";
                    DT = DV.ToTable();
                    GrdSN.DataSource = DT;
                    GrdSN.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdSN.DataSource = DT;
                    GrdSN.DataBind();
                    GrdSN.Rows[0].Cells.Clear();
                    GrdSN.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdSN.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdSN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ValidaDetSN()
        {
            try
            {
                ViewState["Validar"] = "S";
                string VBQuery;
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtHistorico.Enabled == true && ViewState["Historico"].ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens09SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["FrecIni"].ToString().Trim().Equals("0") && ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 7, @Ct, @Cd, @CE,'','SN', @Rst, @Frc,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@Ct", ViewState["Cntdr"]);
                        SC.Parameters.AddWithValue("@Cd", TxtCod.Text);
                        SC.Parameters.AddWithValue("@CE", ViewState["CodElem"]);
                        SC.Parameters.AddWithValue("@Rst", ViewState["Reset"]);
                        SC.Parameters.AddWithValue("@Frc", ViewState["Frec"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataReader DAE = SC.ExecuteReader();
                        if (DAE.Read())
                        {
                            string Mensj = DAE["Mensj"].ToString().Trim();
                            DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { Mensj = row["Texto"].ToString().Trim(); }
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                            ViewState["Validar"] = "N";
                            return;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidaDetSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnSN_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-outline-primary";
            BtnPN.CssClass = "btn btn-outline-primary";
            BtnSN.CssClass = "btn btn-primary";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            GrdAeron.Visible = false;
            GrdPN.Visible = false;
            GrdSN.Visible = true;
            LblAkAsing.Visible = true;
            GrdHKAsig.Visible = true;
            BtnConfigContdrInic.Visible = false;
            IbtAdd.Enabled = false;
            if (ViewState["TIPO"].ToString().Equals("A")) { LimpiarCampos(); BindDTraerdatos("0", "P", "SEL"); }
            else { BindDTraerdatos(DdlBusq.Text, "P", "SEL"); }
            ViewState["TIPO"] = "S";
            BindDataAll();
            PerfilesGrid();
        }
        protected void GrdSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdCodElem = Convert.ToInt32(GrdSN.DataKeys[this.GrdSN.SelectedIndex][3].ToString()); //Indices 0 = pos1
                ViewState["CodElem"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][1].ToString().Trim();

                ViewState["IdCodElem"] = IdCodElem;
                foreach (GridViewRow Row in GrdSN.Rows)
                {
                    if (Row.RowIndex == GrdSN.SelectedIndex)
                    {
                        Row.Style["background-color"] = "#D4DAD3";
                        Row.Attributes["onclick"] = "";

                        ViewState["PN"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][4].ToString().Trim();
                        ViewState["SN"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][5].ToString().Trim();
                        Label ext = Row.FindControl("LblExt") as Label;
                        Label extDia = Row.FindControl("LblExtDia") as Label;
                        if (ext != null)
                        {
                            decimal VbExt = Convert.ToDecimal(ext.Text);
                            decimal VbExtD = Convert.ToDecimal(extDia.Text);
                            TxtMatric.Text = GrdSN.DataKeys[this.GrdSN.SelectedIndex][2].ToString();
                            int VbID = Convert.ToInt32(GrdSN.DataKeys[this.GrdSN.SelectedIndex][0].ToString());
                            Cumplimiento(VbID, VbExt, VbExtD);
                            //EstadoOT(VbID);
                        }
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0)
                        {
                            Row.Style["background-color"] = "white";
                        }
                        else
                        {
                            Row.Style["background-color"] = "#cae4ff";
                        }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + Row.RowIndex);

                    }
                    PerfilesGrid();
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SelectedIndexChanged DET S/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdSN.EditIndex = e.NewEditIndex; BindDSN(); }
        protected void GrdSN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());
                ViewState["PN"] = (GrdSN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();
                if ((GrdSN.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true)
                { TxtHistorico.Enabled = true; }
                else
                { TxtHistorico.Enabled = false; TxtHistorico.Text = ""; ViewState["Historico"] = ""; }

                while ((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text = (GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Ext"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Ext"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim()) * -1;
                }

                while ((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text = (GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["ExtDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["ExtDia"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim()) * -1;
                }

                ViewState["CodElem"] = GrdSN.DataKeys[e.RowIndex].Values["CodElem"].ToString();

                ViewState["Cntdr"] = (GrdSN.Rows[e.RowIndex].FindControl("LblCont") as Label).Text.Trim();
                ViewState["Reset"] = (GrdSN.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FrecIni"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["FrecIni"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim());
                }

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Frec"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Frec"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim());
                }

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["NroDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["NroDia"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim());
                }

                if (!(GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FechaVenc"] = Convert.ToDateTime((GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim());
                }
                // validar
                ValidaDetSN();
                if (ViewState["Validar"].Equals("N"))
                {
                    BindDataAll();
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                    var Detail = new CsTypContaSrvMant()
                    {
                        CodIdContaSrvManto = VblId,
                        CodAeronave = 0,
                        CodElemento = 0,
                        CodServicioManto = TxtCod.Text.Trim(),
                        Frecuencia = (double)ViewState["Frec"],
                        Extension = (double)ViewState["Ext"],
                        FechaVencimiento = (GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                        NroDias = (double)ViewState["NroDia"],
                        ExtensionDias = (double)ViewState["ExtDia"],
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = (int)ViewState["Reset"],
                        FrecuenciaInicial = (double)ViewState["FrecIni"],
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = ViewState["CodElem"].ToString(),
                        PN = ViewState["PN"].ToString(),
                        Accion = "UPDATE",
                        Aplicabilidad = "SN",
                        CrearHistorico = (GrdSN.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true ? "S" : "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdSN.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDTraerdatos(DdlBusq.Text.Trim(), "P", "UPD");
                    BindDataAll();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET SN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdSN.EditIndex = -1; BindDSN(); }
        protected void GrdSN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 12,'','','','','VALIDA',@I,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@I", IDContaSrvManto);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataReader registro = SC.ExecuteReader();
                        if (registro.Read())
                        {
                            string Mensj = registro["Mensj"].ToString().Trim();
                            DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { Mensj = row["Texto"].ToString().Trim(); }

                            ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                            PerfilesGrid();
                            return;
                        }
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 12,'','','', @Us,'DELETE', @I, @IdSv,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@I", IDContaSrvManto);
                                sqlCmd.Parameters.AddWithValue("@IdSv", TxtId.Text);
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDTraerdatos(DdlBusq.Text.Trim(), "P", "UPD");
                                BindDataAll();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET S/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + e.Row.RowIndex);
                    DataRow[] Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// 

                    ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdSN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdSN.PageIndex = e.NewPageIndex; BindDSN(); }
        // ********************************* Aeronaves Asignadas *********************************
        protected void BindDHKAsig()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDet = (DataSet)ViewState["DSTDet"];
                DataRow[] Result;
                DataTable DT = new DataTable();
                DT = DSTDet.Tables[9].Clone();
                string VbId = TxtId.Text.Equals("") ? "0" : TxtId.Text;
                Result = DSTDet.Tables[9].Select("IdSrvManto = " + VbId);
                foreach (DataRow DR in Result)
                {
                    DT.ImportRow(DR);
                }
                if (DT.Rows.Count > 0)
                {
                    DataView DV = DT.DefaultView;
                    DV.Sort = "Matricula";
                    DT = DV.ToTable();
                    GrdHKAsig.DataSource = DT;
                    GrdHKAsig.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdHKAsig.DataSource = DT;
                    GrdHKAsig.DataBind();
                    GrdHKAsig.Rows[0].Cells.Clear();
                    GrdHKAsig.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdHKAsig.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdHKAsig.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDHKAsig", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdHKAsig_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtCod.Text.Equals(""))
            {
                BindDataAll();
                return;
            }
            if (e.CommandName.Equals("AddNew"))
            {
                string VBQuery;
                int VbCodHK = Convert.ToInt32((GrdHKAsig.FooterRow.FindControl("DdlMatAsigPP") as DropDownList).Text.Trim());
                if ((GrdHKAsig.FooterRow.FindControl("DdlMatAsigPP") as DropDownList).Text.Trim().Equals("0"))
                {
                    BindDataAll();
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens07SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una aeronave
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 4, @Us,'','','','','','','HKASIG','INSERT',@ISM, @CHK,0,0,0,@ICC, '01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ISM", TxtId.Text);
                                SC.Parameters.AddWithValue("@CHK", VbCodHK);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                                BindDTraerdatos(DdlBusq.Text, VbTpo, "UPD");
                                BindDataAll();
                                UpPnlPN.Update();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }// Error en el ingreso
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Aaeronaves asignadas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdHKAsig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    int VblId = Convert.ToInt32(GrdHKAsig.DataKeys[e.RowIndex].Values["IdSrvMantoAeronave"].ToString());
                    int VbCodHK = Convert.ToInt32(GrdHKAsig.DataKeys[e.RowIndex].Values["CodAeronave"].ToString());

                    string VBQuery = "EXEC SP_TablasIngenieria 4, @Us,'','','','','','','HKASIG','DELETE', @ISM,@CHK,@I,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@ISM", TxtId.Text);
                            SC.Parameters.AddWithValue("@CHK", VbCodHK);
                            SC.Parameters.AddWithValue("@I", VblId);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                            BindDTraerdatos(DdlBusq.Text, VbTpo, "UPD");
                            BindDataAll();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//rror en el proceso de eliminación
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE Aaeronaves asignadas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdHKAsig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (IbtAddNew != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if (!TxtId.Text.Equals(""))
            {
                string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','','','','HKAS',{1},0,0,{2},'01-01-01','01-01-01','01-01-01'", DdlModel.Text.Trim(), TxtId.Text, Session["!dC!@"]);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlMatPP = (e.Row.FindControl("DdlMatAsigPP") as DropDownList);
                    DdlMatPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlMatPP.DataTextField = "Matricula";
                    DdlMatPP.DataValueField = "CodAeronave";
                    DdlMatPP.DataBind();
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }
                }
            }
        }
        protected void GrdHKAsig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdHKAsig.PageIndex = e.NewPageIndex; BindDHKAsig(); }
        // ********************************* Adjuntar *********************************
        protected void BindDAdjunto()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 28,'DOCINGENIERIA','{0}','','',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", TxtCod.Text, Session["!dC!@"]);
                sqlCon.Open();
                SqlDataAdapter SDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SDA.Fill(DT);
                if (DT.Rows.Count > 0)
                {
                    GrdAdj.DataSource = DT;
                    GrdAdj.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdAdj.DataSource = DT;
                    GrdAdj.DataBind();
                    GrdAdj.Rows[0].Cells.Clear();
                    GrdAdj.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdAdj.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                    GrdAdj.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            } /**/
        }
        protected void GrdAdj_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtCod.Text.Equals(""))
                {
                    BindDataAll();
                    return;
                }
                if (e.CommandName.Equals("Download"))
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int VblID = int.Parse(GrdAdj.DataKeys[gvr.RowIndex].Value.ToString());
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        string LtxtSql = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 2,'','','','','',{0},0,0,{1},'01-01-01','01-01-01','01-01-01'", VblID, Session["!dC!@"]);
                        SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                        SqlDataReader SDR = SC.ExecuteReader();
                        if (SDR.Read())
                        {
                            Vbl8Type = HttpUtility.HtmlDecode(SDR["TipoArchivo"].ToString().Trim());
                            imagen = (byte[])SDR["ArchivoAdj"];
                            Vbl4Ruta = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                            //Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Vbl4Ruta));
                            Response.ContentType = Vbl8Type;
                            //finalmente escribimos los bytes en la respuesta de la página web
                            Response.BinaryWrite(imagen);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens25SM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la descarga
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "Descargar adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    PerfilesGrid();
                    Vbl3Desc = (GrdAdj.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();
                    FileUpload FileUpPP = (FileUpload)GrdAdj.FooterRow.FindControl("FileUpPP");
                    if (FileUpPP != null)
                    {
                        if (FileUpPP.HasFile)
                        {
                            Vbl4Ruta = FileUpPP.FileName;
                            Vbl6Ext = Path.GetExtension(Vbl4Ruta);
                            Vbl8Type = FileUpPP.PostedFile.ContentType;
                            imagen = new byte[FileUpPP.PostedFile.InputStream.Length];
                            FileUpPP.PostedFile.InputStream.Read(imagen, 0, imagen.Length);
                        }
                        else
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens26SM'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un archivo
                            return;
                        }
                    }
                    if (Vbl3Desc.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una descripción')", true);
                        return;
                    }
                    if (Vbl4Ruta.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens26SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un archivo')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "INSERT INTO TblAdjuntos(IdProceso,CodProceso,Proceso,Descripcion,Ruta,ArchivoAdj,Extension,UsuCrea,UsuMod,FechaCrea,FechaMod,TipoArchivo,IdConfigCia)  " +
                            "VALUES(@ISM,@Cd, 'DOCINGENIERIA',@Desc, @Rta,@Image, @Ext,@Us,@Us,GETDATE(),GETDATE(),@Tp,@ICC)";
                            using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SqlCmd.Parameters.AddWithValue("@ISM", TxtId.Text);
                                    SqlCmd.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim());
                                    SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                                    SqlCmd.Parameters.AddWithValue("@Rta", Vbl4Ruta);
                                    SqlCmd.Parameters.AddWithValue("@Image", imagen);
                                    SqlCmd.Parameters.AddWithValue("@Ext", Vbl6Ext);
                                    SqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                    SqlCmd.Parameters.AddWithValue("@Tp", Vbl8Type);
                                    SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SqlCmd.ExecuteNonQuery();
                                    Transac.Commit();
                                    BindDAdjunto();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT TblAdjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAdj_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdAdj.EditIndex = e.NewEditIndex; BindDAdjunto(); }
        protected void GrdAdj_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            Vbl3Desc = (GrdAdj.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim();
            FileUpload FileUp = GrdAdj.Rows[GrdAdj.EditIndex].FindControl("FileUp") as FileUpload;
            if (FileUp != null)
            {
                if (FileUp.HasFile)
                {
                    Vbl4Ruta = FileUp.FileName;
                    Vbl6Ext = Path.GetExtension(Vbl4Ruta);
                    Vbl8Type = FileUp.PostedFile.ContentType;
                    imagen = new byte[FileUp.PostedFile.InputStream.Length];
                    FileUp.PostedFile.InputStream.Read(imagen, 0, imagen.Length);
                }
            }
            if (Vbl3Desc.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una descripción')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VblSiAdjunto = FileUp.HasFile == true ? " Ruta = @Nom,ArchivoAdj = @Image, Extension = @Ext,TipoArchivo = @TipoA," : "";
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "UPDATE TblAdjuntos SET Descripcion = @Desc ," + VblSiAdjunto + "  UsuMod= @Us, FechaMod=GETDATE()  WHERE IdAdjuntos = @I AND IdConfigCia = @ICC";
                    using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                            SqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SqlCmd.Parameters.AddWithValue("@I", GrdAdj.DataKeys[e.RowIndex].Value.ToString());
                            SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            if (FileUp.HasFile)
                            {
                                SqlCmd.Parameters.AddWithValue("@Nom", Vbl4Ruta);
                                SqlCmd.Parameters.AddWithValue("@Image", imagen);
                                SqlCmd.Parameters.AddWithValue("@Ext", Vbl6Ext);
                                SqlCmd.Parameters.AddWithValue("@TipoA", Vbl8Type);
                            }
                            SqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdAdj.EditIndex = -1;
                            BindDAdjunto();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la actualización')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPATE Adjunto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdAdj_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAdj.EditIndex = -1; BindDAdjunto(); }
        protected void GrdAdj_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    int VblId = Convert.ToInt32(GrdAdj.DataKeys[e.RowIndex].Values["IdAdjuntos"].ToString());
                    string VblRuta = GrdAdj.DataKeys[e.RowIndex].Values["Ruta"].ToString();

                    string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 13,'{0}','{1}','','','',{2},{3},0, @ICC,'01-01-01','01-01-01','01-01-01'"
                           , Session["C77U"].ToString(), VblRuta, VblId, TxtId.Text);
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAll();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE Adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdAdj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                    IbtAddNew.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                    ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdateAdj") as ImageButton);
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
                    ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    if (imgE != null)
                    {
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }
                    if (imgD != null)
                    {
                        DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }
                }
            }
            catch (Exception Ex)
            {
                string borr = Ex.ToString();
            }
        }
        protected void GrdAdj_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAdj.PageIndex = e.NewPageIndex; BindDAdjunto(); }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetFixedHeightForGridIfRowsAreLess(GrdAeron);
            SetFixedHeightForGridIfRowsAreLess(GrdPN);
            SetFixedHeightForGridIfRowsAreLess(GrdSN);
        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 25; //we set header height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 25;
                gv.Height = new Unit(height);
            }
        }
        // ****************Opciones de busqueda ***********************
        protected void BIndDataBusq(string Prmtr)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql, VbOpcion = "";
                VbTxtSql = "";
                if (RdbBusqDes.Checked == true && TblBusqHK.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqDesPN.Checked == true && TblBusqPN.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqPnPN.Checked == true && TblBusqPN.Visible == true)
                {
                    VbOpcion = "P";
                }
                if (RdbBusqDesSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqPnSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "P";
                }
                if (RdbBusqSnSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "S";
                }
                if (!VbOpcion.Equals(""))
                {
                    VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 15, @Prmt, @Tp, @Pll, @Opc,'',0,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Prmt", Prmtr.Trim());
                        SC.Parameters.AddWithValue("@Tp", ViewState["TIPO"].ToString().Trim());
                        SC.Parameters.AddWithValue("@Pll", Session["PllaSrvManto"].ToString().Trim());
                        SC.Parameters.AddWithValue("@Opc", VbOpcion);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DtB);

                            if (DtB.Rows.Count > 0)
                            {
                                GrdBusq.DataSource = DtB;
                                GrdBusq.DataBind();
                            }
                            else
                            {
                                GrdBusq.DataSource = null;
                                GrdBusq.DataBind();
                            }
                        }
                    }
                }
            }
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        {
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BIndDataBusq(TxtBusqueda.Text);
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text.Trim());
            string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
            BindDTraerdatos(vbcod, VbTpo, "SEL");
            UpPnlCampos.Update();
            BindDataAll();
            UpPnlPN.Update();
            PerfilesGrid();
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdBusq.PageIndex = e.NewPageIndex; BIndDataBusq(TxtBusqueda.Text); }
        // ****************Controles de Recurso fisico Reserva ***********************
        protected void BindDRecursoF(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Servicio_Manto 4,'','','','',@ISM,0, @Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ISM", TxtId.Text.Equals("") ? "0" : TxtId.Text);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTRcso = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTRcso);
                                DSTRcso.Tables[0].TableName = "Rsrva";
                                DSTRcso.Tables[1].TableName = "HH";
                                DSTRcso.Tables[2].TableName = "PNDdl";
                                DSTRcso.Tables[3].TableName = "LicencDdl";

                                ViewState["DSTRcso"] = DSTRcso;
                            }
                        }
                    }
                }
            }
            DSTRcso = (DataSet)ViewState["DSTRcso"];
            if (DSTRcso.Tables[0].Rows.Count > 0)
            {
                GrdRecursoF.DataSource = DSTRcso.Tables[0];
                GrdRecursoF.DataBind();
            }
            else
            {
                DSTRcso.Tables[0].Rows.Add(DSTRcso.Tables[0].NewRow());
                GrdRecursoF.DataSource = DSTRcso.Tables[0];
                GrdRecursoF.DataBind();
                GrdRecursoF.Rows[0].Cells.Clear();
                GrdRecursoF.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdRecursoF.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void DdlPNRFPP_TextChanged(object sender, EventArgs e)
        {
            DSTRcso = (DataSet)ViewState["DSTRcso"];
            PerfilesGrid();
            TextBox TxtDesRFPP = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox);
            DropDownList DdlPNRFPP = (GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList);
            TextBox TxtPNRFPP = (GrdRecursoF.FooterRow.FindControl("TxtPNRFPP") as TextBox);

            if (DdlPNRFPP.Text.Trim().Equals("- N -"))
            {
                DdlPNRFPP.Visible = false;
                TxtPNRFPP.Visible = true;
                TxtPNRFPP.Enabled = true;
                TxtDesRFPP.Text = "";
                TxtDesRFPP.Enabled = true;
                return;
            }

            DataRow[] Result = DSTRcso.Tables[2].Select("PN = '" + DdlPNRFPP.Text.Trim() + "'");
            foreach (DataRow SDR in Result)
            { TxtDesRFPP.Text = SDR["Descripcion"].ToString(); }
        }
        protected void IbtCerrarRec_Click(object sender, ImageClickEventArgs e)
        {
            PnlCampos.Visible = true;
            PnlRecursos.Visible = false;
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblPN, VBQuery, VblTxtCant, VbDesc;
                    int VblFase, VblCond;
                    double VblCant;
                    if ((GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Visible == true)
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).SelectedValue.Trim(); }
                    else
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("TxtPNRFPP") as TextBox).Text.Trim(); }
                    VblFase = (GrdRecursoF.FooterRow.FindControl("TxtFaseRFPP") as TextBox).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.FooterRow.FindControl("TxtFaseRFPP") as TextBox).Text.Trim());
                    VblTxtCant = (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim();
                    CultureInfo Culture = new CultureInfo("en-US");
                    VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    VblCond = (GrdRecursoF.FooterRow.FindControl("CkbCondicPP") as CheckBox).Checked == true ? 1 : 0;
                    VbDesc = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "EXEC SP_TablasIngenieria 5,@PN,@Us,@Desc,'','','','','','INSERT',@IdPlIns,@IdSvc,@Cnt,@Condc,@Fs,@ICC,'01-01-1','02-01-1','03-01-1'";
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {

                                    SC.Parameters.AddWithValue("@PN", VblPN);
                                    SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@Desc", VbDesc);
                                    SC.Parameters.AddWithValue("@IdPlIns", 0);
                                    SC.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                    SC.Parameters.AddWithValue("@Cnt", VblCant);
                                    SC.Parameters.AddWithValue("@Condc", VblCond);
                                    SC.Parameters.AddWithValue("@Fs", VblFase);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    var Mensj = SC.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString().Trim(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDRecursoF("UPD");
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdRecursoF.EditIndex = e.NewEditIndex; BindDRecursoF("SEL"); }
        protected void GrdRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VblPN, VBQuery, VblTxtCant;
                int VblFase, VblCond;
                double VblCant;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[e.RowIndex].Value.ToString());
                VblPN = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtPNRF") as TextBox).Text.Trim();
                VblFase = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim());
                VblTxtCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                VblCond = (GrdRecursoF.Rows[e.RowIndex].FindControl("CkbCondic") as CheckBox).Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = "EXEC SP_TablasIngenieria 5,@PN,@Us,'','','','','','','UPDATE',@IdPlIns,@IdSvc,@Cant,@Condc,@Fs,@ICC,'01-01-1','02-01-1','03-01-1'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@IdPlIns", VblId);
                                SC.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@Condc", VblCond);
                                SC.Parameters.AddWithValue("@Fs", VblFase);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                GrdRecursoF.EditIndex = -1;
                                BindDRecursoF("UPD");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdRecursoF.EditIndex = -1; BindDRecursoF("SEL"); }
        protected void GrdRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VblPN, VBQuery, VblTxtCant;
                int VblFase, VblCond;
                double VblCant;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[e.RowIndex].Value.ToString());
                VblPN = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblPn") as Label).Text.Trim();
                VblFase = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblFaseRF") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.Rows[e.RowIndex].FindControl("LblFaseRF") as Label).Text.Trim());
                VblTxtCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim().Equals("") ? "0" : (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim();
                Cnx.RetirarPuntos(VblTxtCant);
                VblTxtCant = Cnx.ValorDecimal();
                VblCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VblTxtCant);
                VblCond = (GrdRecursoF.Rows[e.RowIndex].FindControl("CkbCondicP") as CheckBox).Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 5, @PN, @Us,'','','','','','','DELETE',@I, @IdSvc,@Cant, @Condc,@Fs,@ICC,'01-01-1','02-01-1','03-01-1'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@I", VblId);
                                SC.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@Condc", VblCond);
                                SC.Parameters.AddWithValue("@Fs", VblFase);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDRecursoF("UPD");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = DSTRcso.Tables[2];
                DdlPNRFPP.DataTextField = "PN";
                DdlPNRFPP.DataValueField = "CodPN";
                DdlPNRFPP.DataBind();
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }

                if (CkbBloqRec.Checked == true)
                {
                    Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }// El recurso se encuentra bloqueado
                    IbtAddNew.Enabled = false;
                }
            }
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


                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result1 = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result1)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdRecursoF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdRecursoF.PageIndex = e.NewPageIndex; BindDRecursoF("SEL"); }
        // **************** Licencias  ***********************
        protected void BindDLicencia()
        {
            DSTRcso = (DataSet)ViewState["DSTRcso"];
            if (DSTRcso.Tables[1].Rows.Count > 0)
            {
                GrdLicen.DataSource = DSTRcso.Tables[1];
                GrdLicen.DataBind();
            }
            else
            {
                DSTRcso.Tables[1].Rows.Add(DSTRcso.Tables[1].NewRow());
                GrdLicen.DataSource = DSTRcso.Tables[1];
                GrdLicen.DataBind();
                GrdLicen.Rows[0].Cells.Clear();
                GrdLicen.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdLicen.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdLicen.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void DdlLicenRFPP_TextChanged(object sender, EventArgs e)
        {
            DSTRcso = (DataSet)ViewState["DSTRcso"];
            PerfilesGrid();
            TextBox TxtDesLiRFPP = (GrdLicen.FooterRow.FindControl("TxtDesLiRFPP") as TextBox);
            DropDownList DdlLicenRFPP = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList);

            DataRow[] Result = DSTRcso.Tables[3].Select("CodIdLicencia = " + DdlLicenRFPP.Text.Trim());
            foreach (DataRow SDR in Result)
            { TxtDesLiRFPP.Text = SDR["Descripcion"].ToString(); }
        }
        protected void GrdLicen_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                string VBQuery, VblTxtTE, VbCodIdLicencia;
                double VblTE;
                if ((GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue.Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una licencia')", true);
                    return;
                }
                VbCodIdLicencia = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue;
                CultureInfo Culture = new CultureInfo("en-US");
                VblTxtTE = (GrdLicen.FooterRow.FindControl("TxtTieEstRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdLicen.FooterRow.FindControl("TxtTieEstRFPP") as TextBox).Text.Trim();
                VblTE = VblTxtTE.Length == 0 ? 0 : Convert.ToDouble(VblTxtTE, Culture);
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 8,@Us, @Cd,'','','','','','','INSERT', @IdSvc,@IdLic,@TiempEst,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SqlCmd.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim());
                                SqlCmd.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                SqlCmd.Parameters.AddWithValue("@IdLic", VbCodIdLicencia);
                                SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                                SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SqlCmd.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDRecursoF("UPD");
                                BindDLicencia();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdLicen.EditIndex = e.NewEditIndex; BindDLicencia(); }
        protected void GrdLicen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            PerfilesGrid();
            string VBQuery, VblTxtTE;
            double VblTE;
            int IdSrvLic = Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString());
            string VbCodIdLicencia = GrdLicen.DataKeys[e.RowIndex].Values["CodIdLicencia"].ToString();
            CultureInfo Culture = new CultureInfo("en-US");
            VblTxtTE = (GrdLicen.Rows[e.RowIndex].FindControl("TxtTieEstRF") as TextBox).Text.Trim().Equals("") ? "0" : (GrdLicen.Rows[e.RowIndex].FindControl("TxtTieEstRF") as TextBox).Text.Trim();
            VblTE = VblTxtTE.Length == 0 ? 0 : Convert.ToDouble(VblTxtTE, Culture);

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','UPDATE',{2},{3},@TiempEst,{4},0,@ICC,'01-01-1','02-01-1','03-01-1'",
                     Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);
                    using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                            SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdLicen.EditIndex = -1;
                            BindDRecursoF("UPD");
                            BindDLicencia();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdLicen.EditIndex = -1; BindDLicencia(); }
        private string VblTE;
        protected void GrdLicen_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int IdSrvLic = Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString());
            string VbCodIdLicencia = GrdLicen.DataKeys[e.RowIndex].Values["CodIdLicencia"].ToString();
            foreach (GridViewRow row in GrdLicen.Rows)
            {
                if (Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString()) == Convert.ToInt32(GrdLicen.DataKeys[row.RowIndex].Value.ToString()))
                {
                    VblTE = ((Label)row.FindControl("LblTieEstRF")).Text;
                }
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','DELETE',{2},{3},@TiempEst,{4},0,@ICC,'01-01-1','02-01-1','03-01-1'",
                    Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);

                    using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SqlCmd.Parameters.AddWithValue("@TiempEst", Convert.ToDouble(VblTE));
                            SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDRecursoF("UPD");
                            BindDLicencia();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = DSTRcso.Tables[3];
                DdlLicenRFPP.DataTextField = "CodLicencia";
                DdlLicenRFPP.DataValueField = "CodIdLicencia";
                DdlLicenRFPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdLicen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdLicen.PageIndex = e.NewPageIndex; BindDLicencia(); }
        // ****************Panel informes  *********************** OK IdConfigCia
        protected void IbtCerrarInf_Click(object sender, ImageClickEventArgs e)
        {
            PnlInforme.Visible = false;
            PnlCampos.Visible = true;
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString();
        }
        private string StSql;
        protected void BtnSvcAct_Click(object sender, EventArgs e)
        {

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SC = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[12];
                switch (ViewState["TIPO"].ToString())
                {
                    case "A":
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','A',0,0,0,{1},'01-01-01','01-01-01','01-01-01'", TxtMatric.Text.Trim(), Session["!dC!@"]);
                        parameters[0] = new ReportParameter("PrmrHK", ViewState["AkInf"].ToString().Trim() + ": " + TxtMatric.Text.Trim());
                        break;
                    case "P":
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','P',0,0,0,{1},'01-01-01','01-01-01','01-01-01'", ViewState["PN"].ToString().Trim(), Session["!dC!@"]);
                        parameters[0] = new ReportParameter("PrmrHK", "P/N: " + ViewState["PN"].ToString().Trim());
                        break;
                    default:
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','S',0,0,0,{1},'01-01-01','01-01-01','01-01-01'", ViewState["CodElem"].ToString().Trim(), Session["!dC!@"]);
                        string VbMatr = TxtMatric.Text.Equals("") ? "" : "  | " + ViewState["AkInf"].ToString().Trim() + ": " + TxtMatric.Text;
                        parameters[0] = new ReportParameter("PrmrHK", "P/N  " + ViewState["PN"].ToString().Trim() + "  |  S/N  " + ViewState["SN"].ToString().Trim() + VbMatr);
                        break;
                }
                parameters[1] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[3] = new ReportParameter("PrmImg", VbLogo, true);
                parameters[4] = new ReportParameter("TitInf", ViewState["TitInf"].ToString().Trim());
                parameters[5] = new ReportParameter("DesInf", ViewState["DesInf"].ToString().Trim());
                parameters[6] = new ReportParameter("DocInf", ViewState["DocInf"].ToString().Trim());
                parameters[7] = new ReportParameter("TypInf", ViewState["TypInf"].ToString().Trim());
                parameters[8] = new ReportParameter("ContInf", ViewState["ContInf"].ToString().Trim());
                parameters[9] = new ReportParameter("fechUCInf", ViewState["fechUCInf"].ToString().Trim());
                parameters[10] = new ReportParameter("FrecInf", ViewState["FrecInf"].ToString().Trim());
                parameters[11] = new ReportParameter("InfOT", ViewState["InfOT"].ToString().Trim());



                SqlDataAdapter da = new SqlDataAdapter(StSql, SC);
                da.Fill(ds);
                RprvSvcActivos.LocalReport.EnableExternalImages = true;
                RprvSvcActivos.LocalReport.ReportPath = "Report/Ing/ServiciosActivos.rdlc";
                RprvSvcActivos.LocalReport.DataSources.Clear();
                RprvSvcActivos.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                RprvSvcActivos.LocalReport.SetParameters(parameters);
                RprvSvcActivos.LocalReport.Refresh();
            }
        }
        protected void BtnCumplim_Click(object sender, EventArgs e)
        {
            if (!ViewState["TIPO"].ToString().Equals("P"))
            {
                if (ViewState["TIPO"].ToString().Equals("A"))
                {
                    StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 19,'{0}','{1}','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtMatric.Text.Trim(), TxtCod.Text);
                }
                else
                {
                    StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 20,'{0}','{1}','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", ViewState["CodElem"], TxtCod.Text);
                }
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SC = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[16];
                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmTipo", ViewState["TIPO"].ToString());
                    parameters[4] = new ReportParameter("TitCumpInf", ViewState["TitCumpInf"].ToString());
                    parameters[5] = new ReportParameter("DatosEleInf", ViewState["DatosEleInf"].ToString());
                    parameters[6] = new ReportParameter("DatosHkInf", ViewState["DatosHkInf"].ToString());
                    parameters[7] = new ReportParameter("ServInf", ViewState["ServInf"].ToString());
                    parameters[8] = new ReportParameter("DocInf", ViewState["DocInf"].ToString().Trim());
                    parameters[9] = new ReportParameter("GrupInf", ViewState["GrupInf"].ToString().Trim());
                    parameters[10] = new ReportParameter("FrecInf", ViewState["FrecInf"].ToString().Trim());
                    parameters[11] = new ReportParameter("DiaInf", ViewState["DiaInf"].ToString().Trim());
                    parameters[12] = new ReportParameter("OrdenInf", ViewState["OrdenInf"].ToString().Trim());
                    parameters[13] = new ReportParameter("ContInf2", ViewState["ContInf2"].ToString().Trim());
                    parameters[14] = new ReportParameter("fechUCInf", ViewState["fechUCInf"].ToString().Trim());
                    parameters[15] = new ReportParameter("VlrInf", ViewState["VlrInf"].ToString().Trim()); /* */

                    SqlDataAdapter da = new SqlDataAdapter(StSql, SC);
                    da.Fill(ds);
                    RprvSvcActivos.LocalReport.EnableExternalImages = true;
                    RprvSvcActivos.LocalReport.ReportPath = "Report/Ing/CumplimientoSvc.rdlc";
                    RprvSvcActivos.LocalReport.DataSources.Clear();
                    RprvSvcActivos.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                    RprvSvcActivos.LocalReport.SetParameters(parameters);
                    RprvSvcActivos.LocalReport.Refresh();
                }
            }
        }
        protected void IbtExpExcelSvcAplAK_Click(object sender, ImageClickEventArgs e)
        { Exportar("Asignada"); }
        protected void IbtExpExcelSvcGnrl_Click(object sender, ImageClickEventArgs e)
        { Exportar(""); }
        protected void Exportar(string Condcion)
        {
            try
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportarSvcManto", Session["77IDM"].ToString().Trim());
                string StSql, VbNomRpt;
                if (Condcion.Equals("Asignada"))
                {
                    StSql = "EXEC SP_PANTALLA_Servicio_Manto 27,'','','','CurExportarSvcManto',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    VbNomRpt = "Svc_aeronave_Asignadas";
                }
                else
                {
                    StSql = "EXEC SP_PANTALLA_Servicio_Manto 22,'','','','CurExportarSvcManto',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    VbNomRpt = "Svc_Mantenimiento";
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(StSql, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "REPORTES";
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
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlInforme, UpPnlInforme.GetType(), "IdntificadorBloqueScript", "alert('error')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        // **************** Configurar contador inicial HK  ***********************
        protected void BtnConfigContdrInic_Click(object sender, EventArgs e)
        {
            if (TxtId.Text.Equals(""))
            { return; }
            MultVw.ActiveViewIndex = 1; BindDConfIniCntdrHK();
        }
        protected void BindDConfIniCntdrHK()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDet = (DataSet)ViewState["DSTDet"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            DT = DSTDet.Tables[12].Clone();
            string VbId = TxtId.Text.Equals("") ? "0" : TxtId.Text;
            Result = DSTDet.Tables[12].Select("IdSrvMantoCntdrSMHK = " + VbId);
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "CodContador";
                DT = DV.ToTable();
                GrdConfInic.DataSource = DT;
                GrdConfInic.DataBind();
            }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdConfInic.DataSource = DT;
                GrdConfInic.DataBind();
                GrdConfInic.Rows[0].Cells.Clear();
                GrdConfInic.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdConfInic.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdConfInic.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtCloseConfIniCF_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void GrdConfInic_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                if (DdlModel.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens33SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//  El servicio debe tener un modelo asignado.
                    return;
                }
                string VbContdr = (GrdConfInic.FooterRow.FindControl("TxtCodCntdrPP") as TextBox).Text.Trim();
                if (VbContdr.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens08SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe ingresar un contador.
                    return;
                }
                double VbFrec = Convert.ToDouble((GrdConfInic.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdConfInic.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim());
                int VbFrecD = Convert.ToInt32((GrdConfInic.FooterRow.FindControl("TxtFrecDPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdConfInic.FooterRow.FindControl("TxtFrecDPP") as TextBox).Text.Trim());
                if (VbFrec + VbFrecD <= 0)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe ingresar una frecuencia.
                    return;
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasIngenieria 18,@Cntd,@US, '','','','','','','INSERT', @Id,@IdSvc,@Frc,@FrcD,0,@ICC,NULL,'02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@Cntd", VbContdr.ToUpper());
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Id", 0);
                            SC.Parameters.AddWithValue("@IdSvc", TxtId.Text.Equals("") ? "0" : TxtId.Text.Trim());
                            SC.Parameters.AddWithValue("@Frc", VbFrec);
                            SC.Parameters.AddWithValue("@FrcD", VbFrecD);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                                BindDTraerdatos(TxtCod.Text.Trim(), VbTpo, "UPD");
                                BindDConfIniCntdrHK(); BindDAK();
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdConfInic_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdConfInic.EditIndex = e.NewEditIndex; BindDConfIniCntdrHK(); }
        protected void GrdConfInic_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbContdr = GrdConfInic.DataKeys[e.RowIndex].Values["CodContador"].ToString().Trim();
            double VbFrec = Convert.ToDouble((GrdConfInic.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals("") ? "0" : (GrdConfInic.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim());
            int VbFrecD = Convert.ToInt32((GrdConfInic.Rows[e.RowIndex].FindControl("TxtFrecD") as TextBox).Text.Trim().Equals("") ? "0" : (GrdConfInic.Rows[e.RowIndex].FindControl("TxtFrecD") as TextBox).Text.Trim());
            if (VbFrec + VbFrecD <= 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe ingresar una frecuencia.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {

                    string VBQuery = "EXEC SP_TablasIngenieria 18,@Cntd,@US, '','','','','','','UPDATE', @Id,@IdSvc,@Frc,@FrcD,0,@ICC,NULL,'02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Cntd", VbContdr.ToUpper());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Id", GrdConfInic.DataKeys[e.RowIndex].Values["IdConfIni"].ToString());
                        SC.Parameters.AddWithValue("@IdSvc", TxtId.Text.Equals("") ? "0" : TxtId.Text.Trim());
                        SC.Parameters.AddWithValue("@Frc", VbFrec);
                        SC.Parameters.AddWithValue("@FrcD", VbFrecD);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdConfInic.EditIndex = -1;
                            string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                            BindDTraerdatos(TxtCod.Text.Trim(), VbTpo, "UPD");
                            BindDConfIniCntdrHK(); BindDAK();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdConfInic_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdConfInic.EditIndex = -1; BindDConfIniCntdrHK(); }
        protected void GrdConfInic_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasIngenieria 18,@Cntd,@US, '','','','','','','DELETE', @Id,@IdSvc,0,0,0,@ICC,'01-01-01','02-01-1','03-01-1'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@Cntd", GrdConfInic.DataKeys[e.RowIndex].Values["CodContador"].ToString());
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Id", GrdConfInic.DataKeys[e.RowIndex].Values["IdConfIni"].ToString());
                            SC.Parameters.AddWithValue("@IdSvc", TxtId.Text.Equals("") ? "0" : TxtId.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            string VbTpo = ViewState["TIPO"].ToString().Equals("A") ? "" : "P";
                            BindDTraerdatos(TxtCod.Text.Trim(), VbTpo, "UPD");
                            BindDConfIniCntdrHK();
                        }
                    }
                }
            }
            catch (Exception)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                //  Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdConfInic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {              
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }               
            }
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


                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result1 = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result1)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
    }
}
