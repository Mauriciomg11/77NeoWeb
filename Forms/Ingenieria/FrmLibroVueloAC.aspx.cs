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
using AjaxControlToolkit;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using System.Data.OleDb;
using _77NeoWeb.Prg;
using Microsoft.Reporting.WebForms;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using System.Configuration;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmLibroVueloAC : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        private DateTime FechaD = DateTime.Today;
        private DateTime FechaLv, FechaMax, FechaI, FechaF, FechaCompletaI, FechaCompletaF;
        private TimeSpan TtalHoras;
        private byte[] imagenLV;

        protected void Page_Load(object sender, EventArgs e)
        {
           if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }   /**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles     */
            }
            if (!IsPostBack)
            {
                CalFecha.EndDate = DateTime.Now;
                CldFecDet.EndDate = DateTime.Now;
                CldFecCump.EndDate = DateTime.Now;
                CldFecPry.EndDate = DateTime.Now.AddDays(120);
                ViewState["PageTit"] = "";
                ViewState["UltimoDestino"] = "";
                ViewState["ViewOrigen"] = "LV";
                ViewState["Procesado"] = "S";
                ViewState["Validar"] = "S";
                ViewState["SinMotor"] = "N";
                ViewState["CodLvAnt"] = "";
                ViewState["IdLibroVuelo"] = 0;
                ViewState["TotalPasSal"] = 0;
                ViewState["SNApu"] = "";
                ViewState["BtnAccion"] = "";
                ViewState["TtlRtes"] = 0;
                TitForm.Text = "Administración Libro de vuelo";
                MultVieLV.ActiveViewIndex = 0;
                ModSeguridad();
                BindDDdl();
                BindDdlRte();
                BindDMotor("", -1);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                IbtAdd.Visible = false;
                GrdTray.ShowFooter = false;
                FileUpCLV.Visible = false; cargarLV.Visible = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                IbtUpdate.Visible = false;
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
            {

            }
            if (ClsP.GetCE2() == 0)
            {

            }
            if (ClsP.GetCE3() == 0)
            {
            }
            if (ClsP.GetCE4() == 0)
            {

            }
            if (ClsP.GetCE5() == 0)
            {

            }
            if (ClsP.GetCE6() == 0)
            {
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string VblFrm = "FRMINGLIBROVUELO";//ViewState["PFileName"].ToString();
                ViewState["HabilitaVuelos"] = "N";
                ViewState["AplicaFrmlC1C2"] = "N";
                string TxQry = string.Format("EXEC SP_HabilitarCampos '{0}','{1}',1,'{1}',4,'',0,'',0,'',0,'',0,'',0,'',0,'',0",
                Session["Nit77Cia"].ToString(), VblFrm);
                SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                sqlCon.Open();
                SqlDataReader Regs = Comando.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("S"))
                    {
                        //campo vuelos se activa ingreso manual
                        ViewState["HabilitaVuelos"] = "S";
                    }
                    if (VbCaso == 4 && VbAplica.Equals("S"))
                    {
                        //Habilitar campo Evento de autorrotación y simulación 
                        LblEveAutoR.Visible = true;
                        TxtEveAutoR.Visible = true;
                        LblEveSimul.Visible = true;
                        TxtEveSimul.Visible = true;
                        ViewState["AplicaFrmlC1C2"] = "S";
                    }
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;

                string VblFrm = "FrmReporte";//ViewState["PFileName"].ToString();
                string TxQry = string.Format("EXEC SP_HabilitarCampos '{0}','{1}',14,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0",
                  Session["Nit77Cia"].ToString(), VblFrm);
                SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                sqlCon.Open();
                SqlDataReader Regs = Comando.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 14 && VbAplica.Equals("S"))
                    {
                        //Habilitar campos de tiempos aeronave en reporte de mantenimiento.
                        LblTtlAKSN.Visible = true;
                        TxtTtlAKSN.Visible = true;
                        LblHPrxCu.Visible = true;
                        TxtHPrxCu.Visible = true;
                        LblNexDue.Visible = true;
                        TxtNexDue.Visible = true;
                    }
                }
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
                SC.Parameters.AddWithValue("@F1", "FrmLibroVueloAC");
                SC.Parameters.AddWithValue("@F2", "FrmReporte");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("CaptionLV"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("TituloLV") ? bT : TitForm.Text;
                    BtnDatos.Text = bO.Equals("BtnDatos") ? bT : BtnDatos.Text;
                    BtnDatos.Text = bO.Equals("BtnDatos") ? bT : BtnDatos.Text;
                    BtnVuelos.Text = bO.Equals("BtnVuelos") ? bT : BtnVuelos.Text;
                    BtnManto.Text = bO.Equals("BtnManto") ? bT : BtnManto.Text;
                    CkbProcesado.Text = bO.Equals("CkbProcesado") ? "&nbsp " + bT : CkbProcesado.Text;
                    LblNumLVTit.Text = bO.Equals("LblNumLVTit") ? bT : LblNumLVTit.Text;
                    IbtAdd.ToolTip = bO.Equals("BotonIng") ? bT : IbtAdd.ToolTip;
                    IbtUpdate.ToolTip = bO.Equals("BotonMod") ? bT : IbtUpdate.ToolTip;
                    IbtFind.ToolTip = bO.Equals("IbtFindTT") ? bT : IbtFind.ToolTip;
                    IbtDelete.ToolTip = bO.Equals("IbtDelete") ? bT : IbtDelete.ToolTip;
                    cargarLV.Text = bO.Equals("cargarLV") ? bT : cargarLV.Text;
                    LblDatosGrales.Text = bO.Equals("BtnDatos") ? bT : LblDatosGrales.Text;
                    LblFecha.Text = bO.Equals("RdbBusqLVloFech") ? bT + ":" : LblFecha.Text;
                    LblNumLV.Text = bO.Equals("LblNumLVTit") ? bT + ":" : LblNumLV.Text;
                    LbLMatri.Text = bO.Equals("RdbBusqLVloHK") ? bT + ":" : LbLMatri.Text;
                    LblBase.Text = bO.Equals("LblBase") ? bT : LblBase.Text;
                    LblObserv.Text = bO.Equals("LblObserv") ? bT + ":" : LblObserv.Text;
                    LblHrAPU.Text = bO.Equals("LblHrAPU") ? bT + ":" : LblHrAPU.Text;
                    LblNumVuelo.Text = bO.Equals("InfLVVLS") ? bT + ":" : LblNumVuelo.Text;
                    LblLevante.Text = bO.Equals("LblLevante") ? bT + ":" : LblLevante.Text;
                    LblAterrCorr.Text = bO.Equals("LblAterrCorr") ? bT + ":" : LblLevante.Text;
                    LblEveAutoR.Text = bO.Equals("LblEveAutoR") ? bT + ":" : LblEveAutoR.Text;
                    LblEveSimul.Text = bO.Equals("LblEveSimul") ? bT + ":" : LblEveSimul.Text;
                    // ************************************** Datos Motor  *******************************************************
                    LblTitDaMtr.Text = bO.Equals("LblTitDaMtr") ? bT : LblTitDaMtr.Text;
                    GrdMotor.Columns[7].HeaderText = bO.Equals("GrdPreAcei") ? bT : GrdMotor.Columns[7].HeaderText;
                    GrdMotor.Columns[8].HeaderText = bO.Equals("GridTmpAcei") ? bT : GrdMotor.Columns[8].HeaderText;
                    GrdMotor.Columns[9].HeaderText = bO.Equals("GrdPreComb") ? bT : GrdMotor.Columns[9].HeaderText;
                    GrdMotor.Columns[10].HeaderText = bO.Equals("GrdPreHYD") ? bT : GrdMotor.Columns[10].HeaderText;
                    GrdMotor.Columns[11].HeaderText = bO.Equals("GrdNvlComb") ? bT : GrdMotor.Columns[11].HeaderText;
                    GrdMotor.EmptyDataText = bO.Equals("GrdEmptyD") ? bT : GrdMotor.EmptyDataText;
                    // ************************************** Informes LV  *******************************************************
                    IbtAuxiliar.ToolTip = bO.Equals("IbtAuxiliar") ? bT : IbtAuxiliar.ToolTip;
                    LblTitInfLV.Text = bO.Equals("IbtAuxiliar") ? bT : LblTitInfLV.Text;
                    LblAKInfLV.Text = bO.Equals("RdbBusqLVloHK") ? bT : LblAKInfLV.Text;
                    LblFechaIInfLV.Text = bO.Equals("LblFechaIInfLV") ? bT : LblFechaIInfLV.Text;
                    LblFechaFInfLV.Text = bO.Equals("LblFechaFInfLV") ? bT : LblFechaFInfLV.Text;
                    BtnInfLibroVuelos.Text = bO.Equals("BtnInfLibroVuelos") ? bT : BtnInfLibroVuelos.Text;
                    BtnInfDetLV.Text = bO.Equals("BtnVuelos") ? bT : BtnInfDetLV.Text;
                    IbtCerrarInfLV.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarInfLV.ToolTip;
                    ViewState["InfLVTit"] = bO.Equals("InfLVTit") ? bT : ViewState["InfLVTit"];
                    ViewState["InfLVHK"] = bO.Equals("InfLVHK") ? bT : ViewState["InfLVHK"];
                    ViewState["InfLVDate"] = bO.Equals("RdbBusqLVloFech") ? bT : ViewState["InfLVDate"];
                    ViewState["InfLVBase"] = bO.Equals("InfLVBase") ? bT : ViewState["InfLVBase"];
                    ViewState["InfLVTTH"] = bO.Equals("InfLVTTH") ? bT : ViewState["InfLVTTH"];
                    ViewState["InfLVVLS"] = bO.Equals("InfLVVLS") ? bT : ViewState["InfLVVLS"];
                    ViewState["InfLVRN"] = bO.Equals("InfLVRN") ? bT : ViewState["InfLVRN"];
                    ViewState["InfLVTPax"] = bO.Equals("InfLVTPax") ? bT : ViewState["InfLVTPax"];
                    ViewState["InfLVRealz"] = bO.Equals("InfLVRealz") ? bT : ViewState["InfLVRealz"];
                    ViewState["InfLVProcs"] = bO.Equals("CkbProcesado") ? bT : ViewState["InfLVProcs"];
                    ViewState["InfLVFcP"] = bO.Equals("InfLVFcP") ? bT : ViewState["InfLVFcP"];
                    ViewState["InfDLVPag"] = bO.Equals("RTEPAG") ? bT : ViewState["InfDLVPag"];
                    ViewState["InfDLVTit"] = bO.Equals("InfDLVTit") ? bT : ViewState["InfDLVTit"];
                    ViewState["InfDLVDe"] = bO.Equals("RteDe") ? bT : ViewState["InfDLVDe"];
                    ViewState["InfDLVOri"] = bO.Equals("GrdOrigen") ? bT : ViewState["InfDLVOri"];
                    ViewState["InfDLVDest"] = bO.Equals("GrdDestino") ? bT : ViewState["InfDLVDest"];
                    ViewState["InfDLVPeso"] = bO.Equals("InfDLVPeso") ? bT : ViewState["InfDLVPeso"];
                    // ************************************** BUSQUEDA  *******************************************************
                    RdbBusqLVloNum.Text = bO.Equals("LblNumLVTit") ? "&nbsp " + bT : RdbBusqLVloNum.Text;
                    RdbBusqLVloFech.Text = bO.Equals("RdbBusqLVloFech") ? "&nbsp " + bT : RdbBusqLVloFech.Text;
                    RdbBusqLVloHK.Text = bO.Equals("RdbBusqLVloHK") ? "&nbsp " + bT : RdbBusqLVloHK.Text;
                    RdbBusqLVloNroRte.Text = bO.Equals("RdbBusqLVloNroRte") ? "&nbsp " + bT : RdbBusqLVloNroRte.Text;
                    RdbBusqRteNum.Text = bO.Equals("RdbBusqRteNum") ? "&nbsp " + bT : RdbBusqRteNum.Text;
                    RdbBusqRteNum.Text = bO.Equals("RdbBusqRteNum") ? "&nbsp " + bT : RdbBusqRteNum.Text;
                    RdbBusqRteHk.Text = bO.Equals("RdbBusqRteHk") ? "&nbsp " + bT : RdbBusqRteHk.Text;
                    RdbBusqRteOT.Text = bO.Equals("RdbBusqRteOT") ? "&nbsp " + bT : RdbBusqRteOT.Text;
                    RdbBusqRteTecn.Text = bO.Equals("RdbBusqRteTecn") ? "&nbsp " + bT : RdbBusqRteTecn.Text;
                    RdbBusqRteDescRte.Text = bO.Equals("RdbBusqRteDescRte") ? "&nbsp " + bT : RdbBusqRteDescRte.Text;
                    IbtExpConsulRte.ToolTip = bO.Equals("IbtExpConsulRte") ? bT : IbtExpConsulRte.ToolTip;
                    LblOpcBusq.Text = bO.Equals("Busqueda") ? bT : LblOpcBusq.Text;
                    if (bO.Equals("placeholderDC"))
                    {
                        TxtBusqueda.Attributes.Add("placeholder", bT);
                        TxtConsulPnRecurRte.Attributes.Add("placeholder", bT);
                    }
                    IbtConsultarBusq.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtConsultarBusq.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[0].HeaderText = bO.Equals("GrdSelecc") ? bT : GrdBusq.Columns[0].HeaderText;
                    // ************************************** TRAYECTOS  *******************************************************
                    GrdTray.EmptyDataText = bO.Equals("GrdSinTray") ? bT : GrdTray.EmptyDataText;
                    GrdTray.Columns[0].HeaderText = bO.Equals("GrdOrigen") ? bT : GrdTray.Columns[0].HeaderText;
                    GrdTray.Columns[1].HeaderText = bO.Equals("GrdDestino") ? bT : GrdTray.Columns[1].HeaderText;
                    GrdTray.Columns[2].HeaderText = bO.Equals("GrdFechS") ? bT : GrdTray.Columns[2].HeaderText;
                    GrdTray.Columns[3].HeaderText = bO.Equals("GrdHM") ? bT : GrdTray.Columns[3].HeaderText;
                    GrdTray.Columns[4].HeaderText = bO.Equals("GrdFechLl") ? bT : GrdTray.Columns[4].HeaderText;
                    GrdTray.Columns[5].HeaderText = bO.Equals("GrdHM") ? bT : GrdTray.Columns[5].HeaderText;
                    GrdTray.Columns[6].HeaderText = bO.Equals("GrdTiemVlo") ? bT : GrdTray.Columns[6].HeaderText;
                    GrdTray.Columns[7].HeaderText = bO.Equals("InfLVTPax") ? bT : GrdTray.Columns[7].HeaderText;
                    //****************************************** Reporte *********************************************************
                    //IbtCerrarRte.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarRte.ToolTip;
                    LblAeroRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblAeroRte") ? tbl["Texto"].ToString().Trim() : LblAeroRte.Text;
                    LblOtSec.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblOtSec.Text;
                    LblTitRteManto.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitRteManto") ? tbl["Texto"].ToString().Trim() : LblTitRteManto.Text;
                    LblNroRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblNroRte.Text;
                    LblTipRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblTipRte") ? tbl["Texto"].ToString().Trim() : LblTipRte.Text;
                    LblFuente.Text = tbl["Objeto"].ToString().Trim().Equals("LblFuente") ? tbl["Texto"].ToString().Trim() : LblFuente.Text;
                    LblCasi.Text = tbl["Objeto"].ToString().Trim().Equals("LblCasi") ? tbl["Texto"].ToString().Trim() : LblCasi.Text;
                    LblTall.Text = tbl["Objeto"].ToString().Trim().Equals("LblTall") ? tbl["Texto"].ToString().Trim() : LblTall.Text;
                    LblEstad.Text = tbl["Objeto"].ToString().Trim().Equals("LblEstad") ? tbl["Texto"].ToString().Trim() : LblEstad.Text;
                    LblNotif.Text = tbl["Objeto"].ToString().Trim().Equals("LblNotif") ? tbl["Texto"].ToString().Trim() : LblNotif.Text;
                    LblClasf.Text = tbl["Objeto"].ToString().Trim().Equals("LblClasf") ? tbl["Texto"].ToString().Trim() : LblClasf.Text;
                    LblCatgr.Text = tbl["Objeto"].ToString().Trim().Equals("LblCatgr") ? tbl["Texto"].ToString().Trim() : LblCatgr.Text;
                    LblDocRef.Text = tbl["Objeto"].ToString().Trim().Equals("LblDocRef") ? tbl["Texto"].ToString().Trim() : LblDocRef.Text;
                    LblPosRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblPosRte") ? tbl["Texto"].ToString().Trim() : LblPosRte.Text;
                    LblAta.Text = tbl["Objeto"].ToString().Trim().Equals("LblAta") ? tbl["Texto"].ToString().Trim() : LblAta.Text;
                    Generado.Text = tbl["Objeto"].ToString().Trim().Equals("Generado") ? tbl["Texto"].ToString().Trim() : Generado.Text;
                    LblLicGene.Text = tbl["Objeto"].ToString().Trim().Equals("LblLicGene") ? tbl["Texto"].ToString().Trim() : LblLicGene.Text;
                    LblFecDet.Text = tbl["Objeto"].ToString().Trim().Equals("LblFecDet") ? tbl["Texto"].ToString().Trim() : LblFecDet.Text;
                    LblFecProy.Text = tbl["Objeto"].ToString().Trim().Equals("LblFecProy") ? tbl["Texto"].ToString().Trim() : LblFecProy.Text;
                    LblOtRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtRte") ? tbl["Texto"].ToString().Trim() : LblOtRte.Text;
                    LblBasRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblBasRte") ? tbl["Texto"].ToString().Trim() : LblBasRte.Text;
                    LblCumpl.Text = tbl["Objeto"].ToString().Trim().Equals("LblCumpl") ? tbl["Texto"].ToString().Trim() : LblCumpl.Text;
                    LblLicCump.Text = tbl["Objeto"].ToString().Trim().Equals("LblLicGene") ? tbl["Texto"].ToString().Trim() : LblLicCump.Text;
                    LblFecCump.Text = tbl["Objeto"].ToString().Trim().Equals("LblFecCump") ? tbl["Texto"].ToString().Trim() : LblFecCump.Text;
                    lblProgr.Text = tbl["Objeto"].ToString().Trim().Equals("lblProgr") ? tbl["Texto"].ToString().Trim() : lblProgr.Text;
                    LblPgSi.Text = tbl["Objeto"].ToString().Trim().Equals("LblPgSi") ? tbl["Texto"].ToString().Trim() : LblPgSi.Text;
                    LblFallC.Text = tbl["Objeto"].ToString().Trim().Equals("LblFallC") ? tbl["Texto"].ToString().Trim() : LblFallC.Text;
                    LblSi.Text = tbl["Objeto"].ToString().Trim().Equals("LblPgSi") ? tbl["Texto"].ToString().Trim() : LblPgSi.Text;
                    LblTtlAKSN.Text = tbl["Objeto"].ToString().Trim().Equals("LblTtlAKSN") ? tbl["Texto"].ToString().Trim() : LblTtlAKSN.Text;
                    LblHPrxCu.Text = tbl["Objeto"].ToString().Trim().Equals("LblHPrxCu") ? tbl["Texto"].ToString().Trim() : LblHPrxCu.Text;
                    LblNexDue.Text = tbl["Objeto"].ToString().Trim().Equals("LblNexDue") ? tbl["Texto"].ToString().Trim() : LblNexDue.Text;
                    LblDescRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblDescRte") ? tbl["Texto"].ToString().Trim() : LblDescRte.Text;
                    LblAccCorr.Text = tbl["Objeto"].ToString().Trim().Equals("LblAccCorr") ? tbl["Texto"].ToString().Trim() : LblAccCorr.Text;
                    AcciParc.Text = tbl["Objeto"].ToString().Trim().Equals("AcciParc") ? tbl["Texto"].ToString().Trim() : AcciParc.Text;
                    LblTecDif.Text = tbl["Objeto"].ToString().Trim().Equals("LblTecDif") ? tbl["Texto"].ToString().Trim() : LblTecDif.Text;
                    LblTitDatosVer.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitDatosVer") ? tbl["Texto"].ToString().Trim() : LblTitDatosVer.Text;
                    LblVerif.Text = tbl["Objeto"].ToString().Trim().Equals("LblVerif") ? tbl["Texto"].ToString().Trim() : LblVerif.Text;
                    BtnIngresar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnIngresar") ? tbl["Texto"].ToString().Trim() : BtnIngresar.Text;
                    BtnModificar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnModificar") ? tbl["Texto"].ToString().Trim() : BtnModificar.Text;
                    BtnReserva.Text = tbl["Objeto"].ToString().Trim().Equals("BtnReserva") ? tbl["Texto"].ToString().Trim() : BtnReserva.Text;
                    BtnConsultar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnConsultar") ? tbl["Texto"].ToString().Trim() : BtnConsultar.Text;
                    BtnImprimir.Text = tbl["Objeto"].ToString().Trim().Equals("BtnImprimir") ? tbl["Texto"].ToString().Trim() : BtnImprimir.Text;
                    BtnEliminar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnEliminar") ? tbl["Texto"].ToString().Trim() : BtnEliminar.Text;
                    BtnSnOnOf.Text = tbl["Objeto"].ToString().Trim().Equals("BtnSnOnOf") ? tbl["Texto"].ToString().Trim() : BtnSnOnOf.Text;
                    BtnSnOnOf.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnSnOnOf1") ? tbl["Texto"].ToString().Trim() : BtnSnOnOf.ToolTip;
                    BtnExporRte.Text = tbl["Objeto"].ToString().Trim().Equals("BtnExporRte") ? tbl["Texto"].ToString().Trim() : BtnExporRte.Text;
                    BtnExporRte.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnExporRte1") ? tbl["Texto"].ToString().Trim() : BtnExporRte.ToolTip;
                    BtnNotificar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnNotificar") ? tbl["Texto"].ToString().Trim() : BtnNotificar.Text;
                    BtnNotificar.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnNotificar1") ? tbl["Texto"].ToString().Trim() : BtnNotificar.ToolTip;
                    //****************************************  Recuso Fisico --------------------
                    LblRecFRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblRecFRte.Text;
                    LblRecFSubOt.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblRecFSubOt.Text;
                    LblPrioridadOT.Text = tbl["Objeto"].ToString().Trim().Equals("LblTtlRecursoRte") ? tbl["Texto"].ToString().Trim() : LblPrioridadOT.Text;
                    LblTtlRecursoRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblTtlRecursoRte") ? tbl["Texto"].ToString().Trim() : LblTtlRecursoRte.Text;
                    LblTitRecursFis.Text = tbl["Objeto"].ToString().Trim().Equals("BtnReserva") ? tbl["Texto"].ToString().Trim() : LblTitRecursFis.Text;
                    GrdRecursoF.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdRecursoF.Columns[2].HeaderText;
                    GrdRecursoF.Columns[3].HeaderText = bO.Equals("Cantidad") ? bT : GrdRecursoF.Columns[3].HeaderText;
                    GrdRecursoF.Columns[4].HeaderText = bO.Equals("UndMed") ? bT : GrdRecursoF.Columns[4].HeaderText;
                    GrdRecursoF.Columns[5].HeaderText = bO.Trim().Equals("CantEntreg") ? bT : GrdRecursoF.Columns[5].HeaderText;
                    LblTitLicencia.Text = bO.Equals("LblTtlRecursoRte") ? bT : LblTitLicencia.Text;
                    IbtCerrarRec.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarRec.ToolTip;
                    LblOtRecurBusq.Text = tbl["Objeto"].ToString().Trim().Equals("Busqueda") ? tbl["Texto"].ToString().Trim() : LblOtRecurBusq.Text;
                    IbtConsulPnRecurRte.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnConsultar") ? tbl["Texto"].ToString().Trim() : IbtConsulPnRecurRte.ToolTip;
                    IbtExpExcelPnRecurRte.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtRecurExpExcelPn") ? tbl["Texto"].ToString().Trim() : IbtExpExcelPnRecurRte.ToolTip;
                    BtnCargaMaxiva.Text = tbl["Objeto"].ToString().Trim().Equals("BtnCargaMasivaTT1") ? tbl["Texto"].ToString().Trim() : BtnCargaMaxiva.Text;
                    LblTitOTCargMasiv.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitCargMasiv") ? tbl["Texto"].ToString().Trim() : LblTitOTCargMasiv.Text;
                    LblCargaMasRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblCargaMasRte.Text;
                    LblCargaMasOt.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblCargaMasOt.Text;
                    IbtCerrarSubMaxivo.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarSubMaxivo.ToolTip;
                    IbtSubirCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtSubirCargaMax") ? tbl["Texto"].ToString().Trim() : IbtSubirCargaMax.ToolTip;
                    IbtGuardarCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtGuardarCargaMax") ? tbl["Texto"].ToString().Trim() : IbtGuardarCargaMax.ToolTip;
                    GrdCargaMax.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[2].HeaderText;
                    GrdCargaMax.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Cantidad") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[3].HeaderText;
                    GrdCargaMax.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndMed") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[4].HeaderText;
                    GrdCargaMax.Columns[5].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndSistem") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[5].HeaderText;
                    LblTitLicencia.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitLicencia") ? tbl["Texto"].ToString().Trim() : LblTitLicencia.Text;
                    GrdLicen.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Licencia") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[0].HeaderText;
                    GrdLicen.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[1].HeaderText;
                    GrdLicen.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("TiempoEstimado") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[2].HeaderText;
                    //****************************************************************** Impresion Reporte ************************************************************
                    LblTitImpresion.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitImpresion") ? tbl["Texto"].ToString().Trim() : LblTitImpresion.Text;
                    IbtCerrarImpresion.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarImpresion.ToolTip;
                    //****************************************************************** Sn ON OFF ************************************************************
                    LblSnONOfNumRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblSnONOfNumRte.Text;
                    IbtCerrarSnOnOff.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarSnOnOff.ToolTip;
                    LlTitSnOnOff.Text = tbl["Objeto"].ToString().Trim().Equals("LlTitSnOnOff") ? tbl["Texto"].ToString().Trim() : LlTitSnOnOff.Text;
                    GrdSnOnOff.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Fecha") ? tbl["Texto"].ToString().Trim() : GrdSnOnOff.Columns[0].HeaderText;
                    GrdSnOnOff.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("RazonRemoc") ? tbl["Texto"].ToString().Trim() : GrdSnOnOff.Columns[1].HeaderText;
                    GrdSnOnOff.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Posicion") ? tbl["Texto"].ToString().Trim() : GrdSnOnOff.Columns[2].HeaderText;
                    GrdSnOnOff.Columns[5].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdSnOnOff.Columns[5].HeaderText;
                    GrdSnOnOff.Columns[8].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Cantidad") ? tbl["Texto"].ToString().Trim() : GrdSnOnOff.Columns[8].HeaderText;
                    //****************************************************************** Herramienta ************************************************************
                    LblTitHta.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitHta") ? tbl["Texto"].ToString().Trim() : LblTitHta.Text;
                    GrdHta.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdHta.Columns[2].HeaderText;
                    GrdHta.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Fecha") ? tbl["Texto"].ToString().Trim() : GrdHta.Columns[3].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                foreach (DataRow row in Result)
                { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                
                Result = Idioma.Select("Objeto= 'GuardarCargaMaxClientClick'");
                foreach (DataRow row in Result)
                { IbtGuardarCargaMax.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                
                Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdTray.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[8].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdRecursoF.Rows)
            {
                if ((int)ViewState["VblModMSRte"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[7].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMSRte"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[7].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdLicen.Rows)
            {
                if ((int)ViewState["VblModMSRte"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[3].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMSRte"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdSnOnOff.Rows)
            {
                if ((int)ViewState["VblModMSRte"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[9].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMSRte"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[9].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdHta.Rows)
            {
                if ((int)ViewState["VblModMSRte"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[4].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMSRte"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[4].Controls.Remove(imgD);
                    }
                }
            }

        }
        protected void BtnDatos_Click(object sender, EventArgs e)
        {
            TblBusqRte.Visible = false;
            TblBusqLVlo.Visible = false;
            ViewState["BtnAccion"] = "";
            MultVieLV.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void TraerHorasTray(string NumLV, string Horas)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbTrayec = "", VblTTH = "";
            DataRow[] Result = Idioma.Select("Objeto= 'BtnVuelos'");
            foreach (DataRow row in Result)
            { VbTrayec = row["Texto"].ToString().Trim(); }
            Result = Idioma.Select("Objeto= 'InfLVTTH'");
            foreach (DataRow row in Result)
            { VblTTH = row["Texto"].ToString().Trim(); }
            LblTrayectos.Text = VbTrayec + " [" + NumLV + "  " + VblTTH + ": " + Horas + "]";
        }
        protected void BtnVuelos_Click(object sender, EventArgs e)
        {
            if (!TxtNumLv.Text.Equals(""))
            {
                TblBusqRte.Visible = false;
                TblBusqLVlo.Visible = false;
                TraerHorasTray(TxtNumLv.Text.Trim(), ViewState["HraMin"].ToString().Trim());
                MultVieLV.ActiveViewIndex = 1;
                BindDTrayectos();
                PerfilesGrid();
                ViewState["BtnAccion"] = "";
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BtnManto_Click(object sender, EventArgs e)
        {
            if (!TxtNumLv.Text.Equals(""))
            {
                ViewState["CodPrioridad"] = "NORMAL";
                TblBusqRte.Visible = false;
                TblBusqLVlo.Visible = false;
                BindBDdlBusqRte();
                ViewState["VblIngMSRte"] = 1;
                BtnIngresar.Visible = true;
                ViewState["VblModMSRte"] = 1;
                ViewState["VblEliMSRte"] = 1;
                ViewState["VblImpMSRte"] = 1;
                ViewState["VblCE4Rte"] = 1;
                ViewState["VblCE6Rte"] = 1;

                ClsPermisos ClsP = new ClsPermisos();
                ClsP.Acceder(Session["C77U"].ToString(), "FrmReporte.aspx");

                if (ClsP.GetIngresar() == 0)
                {
                    ViewState["VblIngMSRte"] = 0;
                    BtnIngresar.Visible = false;
                    GrdRecursoF.ShowFooter = false;
                    GrdLicen.ShowFooter = false;
                    GrdSnOnOff.ShowFooter = false;
                    GrdHta.ShowFooter = false;
                }
                if (ClsP.GetModificar() == 0)
                {
                    ViewState["VblModMSRte"] = 0;
                    BtnModificar.Visible = false;
                }
                if (ClsP.GetConsultar() == 0)
                {
                }
                if (ClsP.GetImprimir() == 0)
                {
                    //El reporte sólo lo puede modificar el técnico que lo creó   
                    ViewState["VblImpMSRte"] = 0;
                    BtnImprimir.Visible = false;
                }
                if (ClsP.GetEliminar() == 0)
                {
                    ViewState["VblEliMSRte"] = 0;
                    BtnEliminar.Visible = false;
                }
                if (ClsP.GetCE1() == 0)
                {
                    // este caso aplica para activar reserva pero no es funcional se debe elimianar
                }
                if (ClsP.GetCE2() == 0)
                {
                    //  este caso especial se debe borrar porque se maneja desde ejecutar codigo
                }
                if (ClsP.GetCE3() == 0)
                {
                    //El reporte sólo lo puede modificar el técnico que lo creó
                    //se debe retirar esta condiiion porque lo puede editar cualquier usuario
                }
                if (ClsP.GetCE4() == 0)
                {
                    // Notificar
                    ViewState["VblCE4Rte"] = 0;
                    BtnNotificar.Visible = false;
                }
                if (ClsP.GetCE5() == 0)
                {

                }
                if (ClsP.GetCE6() == 0)
                {
                    // Abrir Reporte, verifcar
                    ViewState["VblCE6Rte"] = 0;
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbAplica;
                    int VbCaso;
                    ViewState["UsuDefecto"] = "N";
                    ViewState["ImprFmtHta"] = "N";
                    ViewState["AlertaCazaF"] = "N";
                    ViewState["EditCampoRte"] = "N";
                    ViewState["PermiteFechaIgualDetPry"] = "N";
                    string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,2,@F,3,@F,4,@F,6,@F,7,@F,8,@F,12,@F,13,@F,14");
                    SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                    SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                    SC.Parameters.AddWithValue("@F", "FrmReporte");
                    sqlCon.Open();
                    SqlDataReader Regs = SC.ExecuteReader();
                    while (Regs.Read())
                    {
                        VbCaso = Convert.ToInt32(Regs["CASO"]);
                        VbAplica = Regs["EjecutarCodigo"].ToString();
                        if (VbCaso == 2 && VbAplica.Equals("S"))
                        {
                            //Asignar por defecto usuario logiado en abrir y cerrar reporte manto
                            ViewState["UsuDefecto"] = "S";
                        }
                        if (VbCaso == 3)
                        {
                            if (VbAplica.Equals("S"))
                            {
                                //Habilitar boton ingresar en el reporte de manto
                                /*if (Convert.ToInt32(ViewState["VblIngMSRte"]) == 1)
                                { BtnIngresar.Visible = true; }*/
                            }
                            else
                            {
                                //BtnIngresar.Visible = false;
                            }
                        }
                        if (VbCaso == 4)
                        {
                            if (VbAplica.Equals("S"))
                            {
                                //Habilitar Botón Eliminar en Reporte Manto
                                if (Convert.ToInt32(ViewState["VblEliMSRte"]) == 1)
                                { BtnEliminar.Visible = true; }
                            }
                            else
                            {
                                BtnEliminar.Visible = false;
                            }
                        }
                        if (VbCaso == 6 && VbAplica.Equals("S"))
                        {
                            //NOTIFICAR  
                            LblNotif.Visible = true;
                            CkbNotif.Visible = true;
                        }
                        if (VbCaso == 7 && VbAplica.Equals("S"))
                        {
                            //Imprimir FORMATO HERRAMIENTA  ya no aplca
                            ViewState["ImprFmtHta"] = "S";
                        }
                        if (VbCaso == 8 && VbAplica.Equals("S"))
                        {
                            //Alerta caza falla pendiente por publicar 
                            ViewState["AlertaCazaF"] = "S";
                        }
                        if (VbCaso == 12 && VbAplica.Equals("S"))
                        {
                            //Editar campo reporte cualquier usuario en pantalla modificar  
                            ViewState["EditCampoRte"] = "S";
                        }
                        if (VbCaso == 13 && VbAplica.Equals("S"))
                        {
                            //Editar campo reporte cualquier usuario en pantalla modificar  
                            ViewState["PermiteFechaIgualDetPry"] = "S";
                        }
                        if (VbCaso == 14 && VbAplica.Equals("S"))
                        {
                            //Habilitar campos de tiempos aeronave en reporte de mantenimiento. 
                            LblTtlAKSN.Visible = true;
                            TxtTtlAKSN.Visible = true;
                            LblHPrxCu.Visible = true;
                            TxtHPrxCu.Visible = true;
                            LblNexDue.Visible = true;
                            TxtNexDue.Visible = true;
                        }
                    }
                }
                PerfilesGrid();
                ViewState["BtnAccion"] = "";
                MultVieLV.ActiveViewIndex = 2;
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void BindDDdl()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','BLV',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlBusq.DataSource = Cnx.DSET(LtxtSql);
            DdlBusq.DataMember = "Datos";
            DdlBusq.DataTextField = "CodLibroVuelo";
            DdlBusq.DataValueField = "CodLV";
            DdlBusq.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','BAS',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlBase.DataSource = Cnx.DSET(LtxtSql);
            DdlBase.DataMember = "Datos";
            DdlBase.DataTextField = "NomBase";
            DdlBase.DataValueField = "CodBase";
            DdlBase.DataBind();
            DdlBasRte.DataSource = Cnx.DSET(LtxtSql);
            DdlBasRte.DataMember = "Datos";
            DdlBasRte.DataTextField = "NomBase";
            DdlBasRte.DataValueField = "CodBase";
            DdlBasRte.DataBind();

        }
        protected void BindDdlCondLV(int PrAdministrable, int PrNotAdministrable)
        {
            //PrAdministrable=1 si son | PrNotAdministrable =2 si no son administrables
            string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','MTR',{0},{1},{2},0,'01-1-2009','01-01-1900','01-01-1900'",
                PrAdministrable, PrNotAdministrable, DdlMatri.Text.Equals("") ? "0" : DdlMatri.Text);
            DdlMatri.DataSource = Cnx.DSET(LtxtSql);
            DdlMatri.DataMember = "Datos";
            DdlMatri.DataTextField = "Matricula";
            DdlMatri.DataValueField = "CodAeronave";
            DdlMatri.DataBind();
            DdlAeroRte.DataSource = Cnx.DSET(LtxtSql);
            DdlAeroRte.DataMember = "Datos";
            DdlAeroRte.DataTextField = "Matricula";
            DdlAeroRte.DataValueField = "CodAeronave";
            DdlAeroRte.DataBind();
        }
        protected void cargarLV_Click(object sender, EventArgs e)
        {
            if (FileUpCLV != null && !TxtNumLv.Text.Equals(""))
            {
                if (FileUpCLV.HasFile)
                {
                    string VblRuta = FileUpCLV.FileName;
                    string VblExt = Path.GetExtension(VblRuta);
                    string VblType = FileUpCLV.PostedFile.ContentType;


                    VblExt = VblExt.Substring(VblExt.LastIndexOf(".") + 1).ToLower();
                    string[] formatos = new string[] { "jpg", "jpeg", "bmp", "png", "gif", "pdf" };
                    if (Array.IndexOf(formatos, VblExt) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Formato de imagen inválido.')", true);
                        return;
                    }
                    imagenLV = new byte[FileUpCLV.PostedFile.InputStream.Length];
                    FileUpCLV.PostedFile.InputStream.Read(imagenLV, 0, imagenLV.Length);

                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string VBQuery = "";

                        sqlCon.Open();
                        if (LkbDescargarLV.Text.Trim().Equals(""))
                        {
                            VBQuery = string.Format("INSERT INTO TblAdjuntos(IdProceso,CodProceso,Proceso,Descripcion,Ruta,ArchivoAdj,Extension,UsuCrea,UsuMod,FechaCrea,FechaMod,TipoArchivo)  " +
                               "VALUES(@Id,'LV','LIBROVUELO',@Des,@Rt,@Image,@Ex,@Us,@Us,GETDATE(),GETDATE(),@Typ)");
                        }
                        else
                        {
                            VBQuery = string.Format("UPDATE TblAdjuntos SET Descripcion=@Des,Ruta=@Rt,ArchivoAdj=@Image,Extension=@Ex,UsuMod=@Us,FechaMod =GETDATE(),TipoArchivo= @Typ " +
                                "WHERE IdProceso=@Id AND CodProceso='LV'");
                        }
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Id", ViewState["IdLibroVuelo"]);
                                SqlCmd.Parameters.AddWithValue("@Des", TxtNumLv.Text.Trim());
                                SqlCmd.Parameters.AddWithValue("@Rt", VblRuta.Trim());
                                SqlCmd.Parameters.AddWithValue("@Image", imagenLV);
                                SqlCmd.Parameters.AddWithValue("@Ex", VblExt.Trim());
                                SqlCmd.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SqlCmd.Parameters.AddWithValue("@Typ", VblType.Trim());
                                SqlCmd.ExecuteNonQuery();

                                LkbDescargarLV.Text = VblRuta.Trim();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Adjunto LV", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un archivo')", true);
                    return;
                }
            }
        }
        protected void LkbDescargarLV_Click(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 24,'','','','',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", ViewState["IdLibroVuelo"]);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    string VblType = HttpUtility.HtmlDecode(SDR["TipoArchivo"].ToString().Trim());
                    imagenLV = (byte[])SDR["ArchivoAdj"];
                    string VblRuta = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                    //Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", VblRuta));
                    Response.ContentType = VblType;
                    //finalmente escribimos los bytes en la respuesta de la página web
                    Response.BinaryWrite(imagenLV);
                }
            }
        }
        //****************************************<Datos Generales> ******************************************
        protected void ValidarCampos(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                TxtHrAPU.Text = TxtHrAPU.Text.Trim().Equals("") ? "00:00" : TxtHrAPU.Text;
                if (TxtFecha.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens01LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }   //Debe ingresar una fecha')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtNumLv.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens02LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }   //Debe ingresar un número de libro de vuelo')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlMatri.Text.Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens03LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }   //Debe ingresar una matrícula')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarCampos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void Traerdatos(string Prmtr)
        {
            try
            {
                BindDdlCondLV(1, 2);
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbFecha;
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 21,@Prmtr,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                    SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                    SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaReporte"].ToString().Trim());
                        if (!VbFecha.Trim().Equals(""))
                        {
                            FechaD = Convert.ToDateTime(VbFecha);
                            //TxtFecha.Text = String.Format("{0:yyyy-MM-dd}", FechaD);
                            TxtFecha.Text = String.Format("{0:dd/MM/yyyy}", FechaD);
                        }
                        else
                        {
                            TxtFecha.Text = "";
                        }
                        ViewState["IdLibroVuelo"] = SDR["IdLibroVuelo"].ToString();
                        TxtNumLv.Text = SDR["CodLibroVuelo"].ToString().Trim();
                        LblNumLVTit.Text = SDR["CodLibroVuelo"].ToString().Trim();
                        ViewState["CodLvAnt"] = TxtNumLv.Text;
                        DdlMatri.SelectedValue = HttpUtility.HtmlDecode(SDR["CodAeronave"].ToString());
                        DdlBase.Text = HttpUtility.HtmlDecode(SDR["CodBase"].ToString().Trim());
                        TxtObserv.Text = HttpUtility.HtmlDecode(SDR["Comentario"].ToString().Trim());
                        TxtNumVuelo.Text = SDR["Vuelos"].ToString();
                        TxtLevante.Text = SDR["Levantes"].ToString();
                        TxtAterrCorr.Text = SDR["AterrizajeCorrido"].ToString();
                        TxtEveAutoR.Text = SDR["EventoDeAutorrotacion"].ToString();
                        TxtEveSimul.Text = SDR["EventoDeSimulacionFallaMotor"].ToString();
                        TxtHrAPU.Text = HttpUtility.HtmlDecode(SDR["HoraInicial"].ToString().Trim().Substring(0, 5));
                        TxtAlt.Text = SDR["PAlt"].ToString();
                        TxtKias.Text = SDR["Kias"].ToString();
                        TxtOat.Text = SDR["Oat"].ToString();
                        TxtGW.Text = SDR["GW"].ToString();
                        TxtTat.Text = SDR["TAT"].ToString();
                        TxtMach.Text = SDR["MACHS"].ToString();
                        ViewState["UltimoDestino"] = SDR["UltimoDestino"].ToString();
                        ViewState["Procesado"] = SDR["Procesado"].ToString();
                        CkbProcesado.Checked = ViewState["Procesado"].Equals("S") ? true : false;
                        if (CkbProcesado.Checked == false)
                        { FileUpCLV.Enabled = true; cargarLV.Enabled = true; }
                        else { FileUpCLV.Enabled = false; cargarLV.Enabled = false; }
                        ViewState["TotalPasSal"] = SDR["TotalPasSal"].ToString();
                        ViewState["SNApu"] = HttpUtility.HtmlDecode(SDR["SNAPU"].ToString().Trim());
                        ViewState["HraMin"] = HttpUtility.HtmlDecode(SDR["Hr_Mn"].ToString().Trim());
                        ViewState["TtlRtes"] = Convert.ToInt32(SDR["TtlRtes"].ToString());
                        if (SDR["Tipo"].ToString().Trim().Equals("AF"))
                        {
                            LblLevante.Visible = false;
                            TxtLevante.Visible = false;
                            LblAterrCorr.Visible = false;
                            TxtAterrCorr.Visible = false;
                        }
                        else
                        {
                            LblLevante.Visible = true;
                            TxtLevante.Visible = true;
                            LblAterrCorr.Visible = true;
                            TxtAterrCorr.Visible = true;
                        }
                        LkbDescargarLV.Text = HttpUtility.HtmlDecode(SDR["Adjunto"].ToString().Trim());
                        BindDMotor(TxtNumLv.Text.Trim(), -1);
                        UpPnlBtnPpl.Update();
                        LimpiarCamposRte();
                    }
                    SDR.Close();
                    Cnx2.Close();
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente con la consulta')", true);
            }
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            IbtAdd.Enabled = In;
            IbtUpdate.Enabled = Md;
            IbtDelete.Enabled = El;
            IbtFind.Enabled = Otr;
            IbtPrint.Enabled = Ip;
            IbtAuxiliar.Enabled = Otr;
            BtnDatos.Enabled = Otr;
            BtnVuelos.Enabled = Otr;
            BtnManto.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            DdlBusq.Enabled = Edi == true ? false : true;
            TxtNumLv.Enabled = Edi;
            DdlBase.Enabled = Edi;
            TxtObserv.Enabled = Edi;

            if (ViewState["Procesado"].ToString().Equals("N"))
            {
                IbtFecha.Enabled = Edi;
                DdlMatri.Enabled = (int)ViewState["TtlRtes"] == 0 ? Edi : false;
                DdlMatri.ToolTip = (int)ViewState["TtlRtes"] == 0 ? "" : "El libro de vuelo tiene reportes asignados";
                //TxtHrAPU.Enabled = Edi;
                TxtNumVuelo.Enabled = ViewState["HabilitaVuelos"].Equals("S") ? Edi : false;
                TxtLevante.Enabled = Edi;
                TxtAterrCorr.Enabled = Edi;
                TxtEveAutoR.Enabled = Edi;
                TxtEveSimul.Enabled = Edi;
                TxtAlt.Enabled = Edi;
                TxtKias.Enabled = Edi;
                TxtOat.Enabled = Edi;
                TxtGW.Enabled = Edi;
                TxtTat.Enabled = Edi;
                TxtMach.Enabled = Edi;
                ActivarCamGridMot(Edi);
            }
        }
        protected void LimpiarCampos()
        {
            DdlBusq.SelectedValue = "";
            TxtFecha.Text = "";
            TxtNumLv.Text = "";
            DdlMatri.Text = "0";
            DdlBase.Text = "";
            TxtObserv.Text = "";
            TxtHrAPU.Text = "00:00";
            TxtNumVuelo.Text = "0";
            TxtLevante.Text = "0";
            TxtAterrCorr.Text = "0";
            TxtEveAutoR.Text = "0";
            TxtEveSimul.Text = "0";
            TxtAlt.Text = "0";
            TxtKias.Text = "0";
            TxtOat.Text = "0";
            TxtGW.Text = "0";
            TxtTat.Text = "0";
            TxtMach.Text = "0";
            BindDMotor("-1", -1);
            LkbDescargarLV.Text = "";
        }
        protected void ActivarCamGridMot(bool Etd)
        {
            foreach (GridViewRow Row in GrdMotor.Rows)
            {
                TextBox TxtStartP = Row.FindControl("TxtStartP") as TextBox;
                TextBox TxtTQP = Row.FindControl("TxtTQP") as TextBox;
                TextBox TxtITTP = Row.FindControl("TxtITTP") as TextBox;
                TextBox TxtNIP = Row.FindControl("TxtNIP") as TextBox;
                TextBox TxtNIIP = Row.FindControl("TxtNIIP") as TextBox;
                TextBox TxtPresAcP = Row.FindControl("TxtPresAcP") as TextBox;
                TextBox TxtTempAcP = Row.FindControl("TxtTempAcP") as TextBox;
                TextBox TxtPresCombP = Row.FindControl("TxtPresCombP") as TextBox;
                TextBox TxtPresHYDP = Row.FindControl("TxtPresHYDP") as TextBox;
                TextBox TxtNivCombP = Row.FindControl("TxtNivCombP") as TextBox;
                TextBox TxtOEIP = Row.FindControl("TxtOEIP") as TextBox;
                TextBox TxtC1P = Row.FindControl("TxtC1P") as TextBox;
                TextBox TxtC2P = Row.FindControl("TxtC2P") as TextBox;
                if (TxtStartP != null)
                {
                    TxtStartP.Enabled = Etd;
                    TxtTQP.Enabled = Etd;
                    TxtITTP.Enabled = Etd;
                    TxtNIP.Enabled = Etd;
                    TxtNIIP.Enabled = Etd;
                    TxtPresAcP.Enabled = Etd;
                    TxtTempAcP.Enabled = Etd;
                    TxtPresCombP.Enabled = Etd;
                    TxtPresHYDP.Enabled = Etd;
                    TxtNivCombP.Enabled = Etd;
                    TxtOEIP.Enabled = Etd;
                    TxtC1P.Enabled = Etd;
                    TxtC2P.Enabled = Etd;
                }
            }
        }
        protected void BindDMotor(string NroLV, int CodHK)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    DataTable DTMr = new DataTable();
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 11,'{0}','','','{1}',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", NroLV, CodHK);
                    sqlCon.Open();
                    SqlDataAdapter SDAMr = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDAMr.Fill(DTMr);
                    if (DTMr.Rows.Count > 0)
                    {
                        GrdMotor.DataSource = DTMr;
                        GrdMotor.DataBind();
                        ViewState["TablaDet"] = DTMr;
                    }
                    else
                    {
                        GrdMotor.DataSource = null;
                        GrdMotor.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        {
            Traerdatos(DdlBusq.SelectedValue);
            PerfilesGrid();
        }
        protected void IbtAdd_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["BtnAccion"].ToString() == "")
            {
                BindDdlCondLV(1, 1);
                IbtAdd.ImageUrl = "~/images/SaveV2.png";
                ViewState["Procesado"] = "N";
                ActivarBotones(true, false, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { IbtAdd.ToolTip = row["Texto"].ToString().Trim(); }
                ViewState["BtnAccion"] = "NEW";
                LimpiarCampos();
                ActivarCampos(true, true, "Ingresar");
                TblBtnPpal.Enabled = false;
                Result = Idioma.Select("Objeto= 'IbtAddOnC'");
                foreach (DataRow row in Result)
                { IbtAdd.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }
            }
            else
            {
                try
                {
                    ValidarCampos("INSERT");
                    if (ViewState["Validar"].ToString() == "N")
                    { return; }
                    List<CsTypLibroVuelo> ObjEncLV = new List<CsTypLibroVuelo>();
                    var TypEncLV = new CsTypLibroVuelo()
                    {
                        IdLibroVuelo = Convert.ToInt32(ViewState["IdLibroVuelo"]),
                        CodLibroVuelo = TxtNumLv.Text.Trim(),
                        CodLvAnt = ViewState["CodLvAnt"].ToString().Trim(),
                        FechaReporte = Convert.ToDateTime(TxtFecha.Text.Trim()),
                        CodAeronave = Convert.ToInt32(DdlMatri.Text),
                        CodBase = DdlBase.Text.Trim(),
                        Comentario = TxtObserv.Text.Trim(),
                        Realizado = Session["C77U"].ToString(),
                        TotalPasSal = Convert.ToInt32(ViewState["TotalPasSal"]),
                        PAlt = Convert.ToInt32(TxtAlt.Text),
                        Kias = Convert.ToInt32(TxtKias.Text),
                        Oat = Convert.ToInt32(TxtOat.Text),
                        GW = Convert.ToInt32(TxtGW.Text),
                        TAT = Convert.ToInt32(TxtTat.Text),
                        MACHS = Convert.ToInt32(TxtMach.Text),
                        HoraInicial = TxtHrAPU.Text,
                        HoraFinal = 0,
                        Horometro = 0,
                        SnAPU = "Se actualiza en el SP",
                        NumLevante = Convert.ToInt32(TxtLevante.Text),
                        RevisionManto = "0",
                        IdentificadorH = "H",
                        Horas = 0,
                        identificadorV = "V",
                        Vuelos = Convert.ToInt32(TxtNumVuelo.Text),
                        identificadorL = "L",
                        Levantes = Convert.ToInt32(TxtLevante.Text),
                        rines = 0,
                        identificadorR = "R",
                        Acentado = 0,
                        Usu = Session["C77U"].ToString(),
                        AterrizajeCorrido = Convert.ToInt32(TxtAterrCorr.Text),
                        EventoDeAutorrotacion = Convert.ToDouble(TxtEveAutoR.Text),
                        EventoDeSimulacionFallaMotor = Convert.ToDouble(TxtEveSimul.Text),
                        Accion = "INSERT",/**/


                    };
                    ObjEncLV.Add(TypEncLV);

                    List<CsTypLibroVuelo> ObjDetMotr = new List<CsTypLibroVuelo>();
                    foreach (GridViewRow Row in GrdMotor.Rows)
                    {
                        string VbCodElem = GrdMotor.DataKeys[Row.RowIndex].Values[1].ToString(); // obtener indice
                        Label LblPosP = Row.FindControl("LblPosP") as Label;
                        Label LblSNP = Row.FindControl("LblSNP") as Label;
                        TextBox TxtStartP = Row.FindControl("TxtStartP") as TextBox;
                        TextBox TxtTQP = Row.FindControl("TxtTQP") as TextBox;
                        TextBox TxtITTP = Row.FindControl("TxtITTP") as TextBox;
                        string StrITT, StrNI, StrTempA, StrPresAc, StrCombV, StrPresC, StrPresH, StrGGC, StrPTCy;
                        double VbITT, VbNI, VbTempA, VbPresAc, VbCombV, VbPresC, VbPresH, VbGGC, VbStrPTCy;
                        CultureInfo Culture = new CultureInfo("en-US");
                        StrITT = TxtITTP.Text.Trim().Equals("") ? "0" : TxtITTP.Text.Trim();
                        VbITT = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrITT, Culture);

                        TextBox TxtNIP = Row.FindControl("TxtNIP") as TextBox;
                        StrNI = TxtNIP.Text.Trim().Equals("") ? "0" : TxtNIP.Text.Trim();
                        VbNI = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrNI, Culture);
                        TextBox TxtNIIP = Row.FindControl("TxtNIIP") as TextBox;

                        TextBox TxtPresAcP = Row.FindControl("TxtPresAcP") as TextBox;
                        StrPresAc = TxtPresAcP.Text.Trim().Equals("") ? "0" : TxtPresAcP.Text.Trim();
                        VbPresAc = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresAc, Culture);

                        TextBox TxtTempAcP = Row.FindControl("TxtTempAcP") as TextBox;
                        StrTempA = TxtTempAcP.Text.Trim().Equals("") ? "0" : TxtTempAcP.Text.Trim();
                        VbTempA = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrTempA, Culture);

                        TextBox TxtPresCombP = Row.FindControl("TxtPresCombP") as TextBox;
                        StrPresC = TxtPresCombP.Text.Trim().Equals("") ? "0" : TxtPresCombP.Text.Trim();
                        VbPresC = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresC, Culture);

                        TextBox TxtPresHYDP = Row.FindControl("TxtPresHYDP") as TextBox;
                        StrPresH = TxtPresHYDP.Text.Trim().Equals("") ? "0" : TxtPresHYDP.Text.Trim();
                        VbPresH = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresH, Culture);

                        TextBox TxtNivCombP = Row.FindControl("TxtNivCombP") as TextBox;
                        StrCombV = TxtNivCombP.Text.Trim().Equals("") ? "0" : TxtNivCombP.Text.Trim();
                        VbCombV = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrCombV, Culture);

                        TextBox TxtOEIP = Row.FindControl("TxtOEIP") as TextBox;

                        TextBox TxtC1P = Row.FindControl("TxtC1P") as TextBox;
                        StrGGC = TxtC1P.Text.Trim().Equals("") ? "0" : TxtC1P.Text.Trim();
                        VbGGC = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrGGC, Culture);

                        TextBox TxtC2P = Row.FindControl("TxtC2P") as TextBox;
                        StrPTCy = TxtC2P.Text.Trim().Equals("") ? "0" : TxtC2P.Text.Trim();
                        VbStrPTCy = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPTCy, Culture);

                        var TypDetMotr = new CsTypLibroVuelo()
                        {
                            CodIDLvDetMotor = 0,
                            SN = LblSNP.Text.Trim(),
                            NumArranque = Convert.ToInt32(TxtStartP.Text),
                            NII = Convert.ToInt32(TxtNIIP.Text),
                            ITT = VbITT,
                            NI = VbNI,
                            TempAceite = VbTempA,
                            PresionAceite = VbPresAc,
                            Torque = Convert.ToInt32(TxtTQP.Text),
                            Posicion = Convert.ToInt32(LblPosP.Text),
                            OEI = Convert.ToInt32(TxtOEIP.Text),
                            NroVuelo = "",
                            SangradoMotor = 0,
                            AceiteAgreMot = 0,
                            GenOnOff = "",
                            SnAPUDet = "",
                            AceiteAgreAPU = 0,
                            SnAYD = "",
                            AceiteAgreAYD = 0,
                            ART = 0,
                            CombVuelo = VbCombV,
                            PresComb = VbPresC,
                            PresHYD = VbPresH,
                            GasGenCycle = VbGGC,
                            PwrTurbineCycle = VbStrPTCy,
                            CodElemMotorLV = VbCodElem.Trim(),

                        };
                        ObjDetMotr.Add(TypDetMotr);
                    } /**/
                    CsTypLibroVuelo LibroVuelo = new CsTypLibroVuelo();

                    LibroVuelo.Alimentar(ObjEncLV, ObjDetMotr);// 
                    string Mensj = LibroVuelo.GetMensj();
                    if (!Mensj.Trim().Equals("OK"))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    IbtAdd.ImageUrl = "~/images/AddNew.png";
                    ViewState["BtnAccion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
                    foreach (DataRow row in Result)
                    { IbtAdd.ToolTip = row["Texto"].ToString().Trim(); }
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    IbtAdd.OnClientClick = "";
                    IbtFecha.Visible = true;
                    Traerdatos(LibroVuelo.GetNewLv());
                    TblBtnPpal.Enabled = true;
                    BindDDdl();
                }
                catch (Exception Ex)
                {
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtUpdate_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNumLv.Text.Trim().Equals(""))
            { return; }
            if (ViewState["BtnAccion"].ToString() == "")
            {
                string VblCodAkAnt = DdlMatri.Text;
                ViewState["BtnAccion"] = "UPDATE";
                BindDdlCondLV(1, 1);
                DdlMatri.Text = VblCodAkAnt;
                IbtUpdate.ImageUrl = "~/images/SaveV2.png";
                ActivarBotones(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ActivarCampos(false, true, "Modificar");
                Result = Idioma.Select("Objeto= 'IbtUpdateOnC'");
                foreach (DataRow row in Result)
                { IbtUpdate.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }
                if (!ViewState["SNApu"].Equals("") && ViewState["Procesado"].Equals("N"))
                    if (!ViewState["SNApu"].Equals("") && ViewState["Procesado"].Equals("N"))
                    {
                        TxtHrAPU.Enabled = true;
                    }
            }
            else
            {
                try
                {
                    ValidarCampos("UPDATE");
                    if (ViewState["Validar"].ToString() == "N")
                    { return; }
                    List<CsTypLibroVuelo> ObjEncLV = new List<CsTypLibroVuelo>();
                    var TypEncLV = new CsTypLibroVuelo()
                    {
                        IdLibroVuelo = Convert.ToInt32(ViewState["IdLibroVuelo"]),
                        CodLibroVuelo = TxtNumLv.Text.Trim(),
                        CodLvAnt = ViewState["CodLvAnt"].ToString().Trim(),
                        FechaReporte = Convert.ToDateTime(TxtFecha.Text.Trim()),
                        CodAeronave = Convert.ToInt32(DdlMatri.Text),
                        CodBase = DdlBase.Text.Trim(),
                        Comentario = TxtObserv.Text.Trim(),
                        TotalPasSal = 0,
                        Realizado = Session["C77U"].ToString(),
                        PAlt = Convert.ToInt32(TxtAlt.Text),
                        Kias = Convert.ToInt32(TxtKias.Text),
                        Oat = Convert.ToInt32(TxtOat.Text),
                        GW = Convert.ToInt32(TxtGW.Text),
                        TAT = Convert.ToInt32(TxtTat.Text),
                        MACHS = Convert.ToInt32(TxtMach.Text),
                        HoraInicial = TxtHrAPU.Text,
                        HoraFinal = 0,
                        Horometro = 0,
                        SnAPU = "Se actualiza en el SP",
                        NumLevante = Convert.ToInt32(TxtLevante.Text),
                        RevisionManto = "0",
                        IdentificadorH = "H",
                        Horas = 0,
                        identificadorV = "V",
                        Vuelos = Convert.ToInt32(TxtNumVuelo.Text),
                        identificadorL = "L",
                        Levantes = Convert.ToInt32(TxtLevante.Text),
                        rines = 0,
                        identificadorR = "R",
                        Acentado = ViewState["Procesado"].Equals("N") ? 0 : 10,
                        Usu = Session["C77U"].ToString(),
                        AterrizajeCorrido = Convert.ToInt32(TxtAterrCorr.Text),
                        EventoDeAutorrotacion = Convert.ToDouble(TxtEveAutoR.Text),
                        EventoDeSimulacionFallaMotor = Convert.ToDouble(TxtEveSimul.Text),
                        Accion = "UPDATE",/**/
                    };
                    ObjEncLV.Add(TypEncLV);

                    List<CsTypLibroVuelo> ObjDetMotr = new List<CsTypLibroVuelo>();
                    foreach (GridViewRow Row in GrdMotor.Rows)
                    {
                        string VbCodElem = GrdMotor.DataKeys[Row.RowIndex].Values[1].ToString(); // obtener indice
                        Label LblPosP = Row.FindControl("LblPosP") as Label;
                        Label LblSNP = Row.FindControl("LblSNP") as Label;
                        TextBox TxtStartP = Row.FindControl("TxtStartP") as TextBox;
                        TextBox TxtTQP = Row.FindControl("TxtTQP") as TextBox;
                        TextBox TxtITTP = Row.FindControl("TxtITTP") as TextBox;
                        string StrITT, StrNI, StrTempA, StrPresAc, StrCombV, StrPresC, StrPresH, StrGGC, StrPTCy;
                        double VbITT, VbNI, VbTempA, VbPresAc, VbCombV, VbPresC, VbPresH, VbGGC, VbStrPTCy;
                        StrITT = TxtITTP.Text.Trim().Equals("") ? "0" : TxtITTP.Text.Trim();
                        CultureInfo Culture = new CultureInfo("en-US");
                        VbITT = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrITT, Culture);

                        TextBox TxtNIP = Row.FindControl("TxtNIP") as TextBox;
                        StrNI = TxtNIP.Text.Trim().Equals("") ? "0" : TxtNIP.Text.Trim();
                        VbNI = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrNI, Culture);
                        TextBox TxtNIIP = Row.FindControl("TxtNIIP") as TextBox;

                        TextBox TxtPresAcP = Row.FindControl("TxtPresAcP") as TextBox;
                        StrPresAc = TxtPresAcP.Text.Trim().Equals("") ? "0" : TxtPresAcP.Text.Trim();
                        VbPresAc = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresAc, Culture);

                        TextBox TxtTempAcP = Row.FindControl("TxtTempAcP") as TextBox;
                        StrTempA = TxtTempAcP.Text.Trim().Equals("") ? "0" : TxtTempAcP.Text.Trim();
                        VbTempA = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrTempA, Culture);

                        TextBox TxtPresCombP = Row.FindControl("TxtPresCombP") as TextBox;
                        StrPresC = TxtPresCombP.Text.Trim().Equals("") ? "0" : TxtPresCombP.Text.Trim();
                        VbPresC = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresC, Culture);

                        TextBox TxtPresHYDP = Row.FindControl("TxtPresHYDP") as TextBox;
                        StrPresH = TxtPresHYDP.Text.Trim().Equals("") ? "0" : TxtPresHYDP.Text.Trim();
                        VbPresH = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPresH, Culture);

                        TextBox TxtNivCombP = Row.FindControl("TxtNivCombP") as TextBox;
                        StrCombV = TxtNivCombP.Text.Trim().Equals("") ? "0" : TxtNivCombP.Text.Trim();
                        VbCombV = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrCombV, Culture);

                        TextBox TxtOEIP = Row.FindControl("TxtOEIP") as TextBox;

                        TextBox TxtC1P = Row.FindControl("TxtC1P") as TextBox;
                        StrGGC = TxtC1P.Text.Trim().Equals("") ? "0" : TxtC1P.Text.Trim();
                        VbGGC = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrGGC, Culture);

                        TextBox TxtC2P = Row.FindControl("TxtC2P") as TextBox;
                        StrPTCy = TxtC2P.Text.Trim().Equals("") ? "0" : TxtC2P.Text.Trim();
                        VbStrPTCy = StrITT.Length == 0 ? 0 : Convert.ToDouble(StrPTCy, Culture);
                        int vbCodIDLvDetMotor = Convert.ToInt32(GrdMotor.DataKeys[Row.RowIndex].Values[0].ToString());

                        var TypDetMotr = new CsTypLibroVuelo()
                        {
                            CodIDLvDetMotor = Convert.ToInt32(GrdMotor.DataKeys[Row.RowIndex].Values[0].ToString()),
                            SN = LblSNP.Text.Trim(),
                            NumArranque = Convert.ToInt32(TxtStartP.Text),
                            NII = Convert.ToInt32(TxtNIIP.Text),
                            ITT = VbITT,
                            NI = VbNI,
                            TempAceite = VbTempA,
                            PresionAceite = VbPresAc,
                            Torque = Convert.ToInt32(TxtTQP.Text),
                            Posicion = Convert.ToInt32(LblPosP.Text),
                            OEI = Convert.ToInt32(TxtOEIP.Text),
                            NroVuelo = "",
                            SangradoMotor = 0,
                            AceiteAgreMot = 0,
                            GenOnOff = "",
                            SnAPUDet = "",
                            AceiteAgreAPU = 0,
                            SnAYD = "",
                            AceiteAgreAYD = 0,
                            ART = 0,
                            CombVuelo = VbCombV,
                            PresComb = VbPresC,
                            PresHYD = VbPresH,
                            GasGenCycle = VbGGC,
                            PwrTurbineCycle = VbStrPTCy,
                            CodElemMotorLV = VbCodElem.Trim(),

                        };
                        ObjDetMotr.Add(TypDetMotr);
                    } /**/
                    CsTypLibroVuelo LibroVuelo = new CsTypLibroVuelo();

                    LibroVuelo.Alimentar(ObjEncLV, ObjDetMotr);// 
                    string Mensj = LibroVuelo.GetMensj();
                    if (!Mensj.Trim().Substring(0, 2).Equals("OK"))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }

                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    IbtUpdate.ImageUrl = "~/images/Edit.png";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    IbtUpdate.OnClientClick = "";
                    IbtFecha.Visible = true;
                    TxtHrAPU.Enabled = false;
                    Traerdatos(Mensj.Trim().Substring(2));
                    BindDDdl();
                    ViewState["BtnAccion"] = "";
                }
                catch (Exception Ex)
                {
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtDelete_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void IbtPrint_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void IbtAuxiliar_Click(object sender, ImageClickEventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','MTR',{0},{1},{2},0,'01-1-2009','01-01-1900','01-01-1900'",
              1, 1, "0");
            DdlHkInfLV.DataSource = Cnx.DSET(LtxtSql);
            DdlHkInfLV.DataMember = "Datos";
            DdlHkInfLV.DataTextField = "Matricula";
            DdlHkInfLV.DataValueField = "CodAeronave";
            DdlHkInfLV.DataBind();
            MultVieLV.ActiveViewIndex = 8;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void ValidaDatosAeronave()
        {
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                ViewState["SinMotor"].Equals("N");
                SCnx.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 3,'','','','',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlMatri.Text);
                SqlCommand SC = new SqlCommand(LtxtSql, SCnx);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    if (Convert.ToInt32(SDR["NroMotor"]) != Convert.ToInt32(SDR["TtlMotIstalados"]))
                    {
                        ViewState["SinMotor"].Equals("S");
                        string Mensj = "La aeronave tiene pendiente motores por instalar";
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        DdlMatri.SelectedValue = "0";
                        BindDMotor("-1", -1);
                    }
                    else
                    {

                        BindDMotor("", Convert.ToInt32(DdlMatri.SelectedValue));
                        ActivarCamGridMot(true);
                        DdlBase.SelectedValue = SDR["CodBase"].ToString().Trim();
                        if (SDR["Tipo"].ToString().Trim().Equals("AF"))
                        {
                            LblLevante.Visible = false;
                            TxtLevante.Visible = false;
                            LblAterrCorr.Visible = false;
                            TxtAterrCorr.Visible = false;
                            TxtLevante.Text = "0";
                            TxtLevante.Text = "0";
                        }
                        else
                        {
                            LblLevante.Visible = true;
                            TxtLevante.Visible = true;
                            LblAterrCorr.Visible = true;
                            TxtAterrCorr.Visible = true;
                        }
                        if (SDR["APU"].ToString().Trim().Equals("S"))
                        {
                            TxtHrAPU.Enabled = true;
                        }
                        else
                        {
                            TxtHrAPU.Enabled = false;
                            TxtHrAPU.Text = "00:00";
                        }
                        FechaLv = Convert.ToDateTime(TxtFecha.Text);
                        FechaMax = Convert.ToDateTime(HttpUtility.HtmlDecode(SDR["FechaMaxima"].ToString().Trim()));
                        int Comparar = DateTime.Compare(FechaLv, FechaMax);
                        if (Comparar <= 0)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('La  fecha ingresada es anterior o igual a las procesadas, si almacena el libro de vuelo actual debe reprocesar la aeronave.')", true); }
                        if (!DdlMatri.SelectedValue.Equals("0"))
                        { IbtFecha.Visible = false; }
                        else { IbtFecha.Visible = true; }

                    }
                }
            }
        }
        protected void TxtFecha_TextChanged(object sender, EventArgs e)
        {
            if (!DdlMatri.Text.Equals("0"))
            {
                ValidaDatosAeronave();
            }
        }
        protected void DdlMatri_TextChanged(object sender, EventArgs e)
        {
            if (TxtFecha.Text.Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar la fecha del libro de  vuelo')", true);
                DdlMatri.SelectedValue = "0";
                return;
            }
            ValidaDatosAeronave();
        }
        protected void GrdMotor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ViewState["AplicaFrmlC1C2"].ToString().Equals("N"))
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                }
            }
        }
        //**************************************** <Informes del libro de vuelo> ******************************************
        protected void IbtCerrarInfLV_Click(object sender, ImageClickEventArgs e)
        {
            MultVieLV.ActiveViewIndex = 0;
        }
        protected void BtnInfLibroVuelos_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtFIInfLV.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens07LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInfLV, UplInfLV.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una fecha inicial
                    return;
                }
                if (TxtFFInfLV.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens08LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInfLV, UplInfLV.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una fecha final
                    return;
                }
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[15];

                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmLVTit", ViewState["InfLVTit"].ToString().Trim());
                    parameters[4] = new ReportParameter("PrmLVHK", ViewState["InfLVHK"].ToString().Trim());
                    parameters[5] = new ReportParameter("PrmLVFc", ViewState["InfLVDate"].ToString().Trim());
                    parameters[6] = new ReportParameter("PrmLVNum", BtnInfLibroVuelos.Text.Trim());
                    parameters[7] = new ReportParameter("PrmLVBse", ViewState["InfLVBase"].ToString().Trim());
                    parameters[8] = new ReportParameter("PrmLVTTH", ViewState["InfLVTTH"].ToString().Trim());
                    parameters[9] = new ReportParameter("PrmLVVLS", ViewState["InfLVVLS"].ToString().Trim());
                    parameters[10] = new ReportParameter("PrmLVRN", ViewState["InfLVRN"].ToString().Trim());
                    parameters[11] = new ReportParameter("PrmLVTPax", ViewState["InfLVTPax"].ToString().Trim());
                    parameters[12] = new ReportParameter("InfLVRealz", ViewState["InfLVRealz"].ToString().Trim());
                    parameters[13] = new ReportParameter("PrmLVPrcs", ViewState["InfLVProcs"].ToString().Trim());
                    parameters[14] = new ReportParameter("InfLVFcP", ViewState["InfLVFcP"].ToString().Trim());

                    string StSql = "SET DATEFORMAT DMY; EXEC SP_PANTALLA_LibroVuelo 1,@HK,'','','',2,0,0,0,@FI,@FF,'01-01-1900' ";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {
                        SC.Parameters.AddWithValue("@HK", DdlHkInfLV.Text.Equals("0") ? "" : DdlHkInfLV.SelectedItem.Text);
                        SC.Parameters.AddWithValue("@FI", TxtFIInfLV.Text);
                        SC.Parameters.AddWithValue("@FF", TxtFFInfLV.Text);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RvwInfLV.LocalReport.EnableExternalImages = true;
                            RvwInfLV.LocalReport.ReportPath = "Report/Ing/LibroVueloGnral.rdlc";
                            RvwInfLV.LocalReport.DataSources.Clear();
                            RvwInfLV.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RvwInfLV.LocalReport.SetParameters(parameters);
                            RvwInfLV.LocalReport.Refresh();
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Impresion Libro de vuelo general", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnInfDetLV_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtFIInfLV.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens07LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInfLV, UplInfLV.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una fecha inicial
                    return;
                }
                if (TxtFFInfLV.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens08LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInfLV, UplInfLV.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una fecha final
                    return;
                }
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[12];

                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmDLVTit", ViewState["InfDLVTit"].ToString().Trim());
                    parameters[4] = new ReportParameter("PrmDLVPag", ViewState["InfDLVPag"].ToString().Trim());
                    parameters[5] = new ReportParameter("PrmDLVDe", ViewState["InfDLVDe"].ToString().Trim());
                    parameters[6] = new ReportParameter("PrmDLVFc", ViewState["InfLVDate"].ToString().Trim());
                    parameters[7] = new ReportParameter("PrmDLVHK", ViewState["InfLVHK"].ToString().Trim());
                    parameters[8] = new ReportParameter("PrmDLVNum", BtnInfLibroVuelos.Text.Trim());
                    parameters[9] = new ReportParameter("PrmDLVOrig", ViewState["InfDLVOri"].ToString().Trim());
                    parameters[10] = new ReportParameter("PrmDLVDest", ViewState["InfDLVDest"].ToString().Trim());
                    parameters[11] = new ReportParameter("PrmDLVPeso", ViewState["InfDLVPeso"].ToString().Trim());
                    string StSql = "SET DATEFORMAT DMY; EXEC SP_PANTALLA_LibroVuelo 4,@HK,'','','',0,0,0,0,@FI,@FF,'01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                    {

                        SC.Parameters.AddWithValue("@HK", DdlHkInfLV.SelectedItem.Text.Trim());
                        SC.Parameters.AddWithValue("@FI", TxtFIInfLV.Text);
                        SC.Parameters.AddWithValue("@FF", TxtFFInfLV.Text);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(ds);
                            RvwInfLV.LocalReport.EnableExternalImages = true;
                            RvwInfLV.LocalReport.ReportPath = "Report/Ing/LibroVueloDetalle.rdlc";
                            RvwInfLV.LocalReport.DataSources.Clear();
                            RvwInfLV.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                            RvwInfLV.LocalReport.SetParameters(parameters);
                            RvwInfLV.LocalReport.Refresh();
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Impresion Libro de vuelo general", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        //**************************************** <Trayectos> ******************************************
        protected void BindDTrayectos()
        {
            try
            {
                DataTable DTMr = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 22,'{0}','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtNumLv.Text.Trim());
                    sqlCon.Open();
                    SqlDataAdapter SDAMr = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDAMr.Fill(DTMr);
                    if (DTMr.Rows.Count > 0)
                    {
                        GrdTray.DataSource = DTMr;
                        GrdTray.DataBind();
                    }
                    else
                    {
                        DTMr.Rows.Add(DTMr.NewRow());
                        GrdTray.DataSource = DTMr;
                        GrdTray.DataBind();
                        /*GrdOTDetTec.Rows[0].Cells.Clear();
                         GrdOTDetTec.Rows[0].Cells.Add(new TableCell());
                         GrdOTDetTec.Rows[0].Cells[0].Text = "Sin técnicos asignados!";
                         GrdOTDetTec.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;*/
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void CalculoHoras(DateTime FI, DateTime FF, string HS, string HLL, string Tipo)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (!HS.Equals("") && !HLL.Equals(""))
                {
                    ViewState["Validar"] = "S";
                    string HI = HS.Substring(0, 2);
                    string HF = HLL.Substring(0, 2);
                    string MI = HS.Substring(3);
                    string MF = HLL.Substring(3);
                    FechaCompletaI = FI.Add(new TimeSpan(Convert.ToInt32(HI), Convert.ToInt32(MI), 0));
                    FechaCompletaF = FF.Add(new TimeSpan(Convert.ToInt32(HF), Convert.ToInt32(MF), 0));
                    int Comparar = DateTime.Compare(FechaCompletaF, FechaCompletaI);
                    if (Comparar < 0)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens18LV'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La fecha y hora de salida no puede ser menores a la de llegada
                    }
                    TtalHoras = FechaCompletaF.Subtract(FechaCompletaI);
                    if (Tipo.Equals("INSERT"))
                    {
                        TextBox TxtTimeVPP = (GrdTray.FooterRow.FindControl("TxtTimeVPP") as TextBox);
                        TxtTimeVPP.Text = TtalHoras.ToString().Substring(0, 5);
                    }
                    else
                    {
                        TextBox TxtTimeV = GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtTimeV") as TextBox;
                        TxtTimeV.Text = TtalHoras.ToString().Substring(0, 5);
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void TxtHMSPP_TextChanged(object sender, EventArgs e)
        {
            FechaI = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecSalPP") as TextBox).Text);
            FechaF = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecLlePP") as TextBox).Text);
            string VbHS = (GrdTray.FooterRow.FindControl("TxtHMSPP") as TextBox).Text;
            string VbHLl = (GrdTray.FooterRow.FindControl("TxtHMLPP") as TextBox).Text;
            CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "INSERT");
            TextBox TxtHMLPP = (GrdTray.FooterRow.FindControl("TxtHMLPP") as TextBox);
            TxtHMLPP.Focus();
            PerfilesGrid();
        }
        protected void TxtHMS_TextChanged(object sender, EventArgs e)
        {
            string borrar = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecSal") as TextBox).Text;
            FechaI = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecSal") as TextBox).Text); // El indice se toma en el evento RowEditing
            FechaF = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecLle") as TextBox).Text);
            string VbHS = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHMS") as TextBox).Text;
            string VbHLl = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHML") as TextBox).Text;
            CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "UPDATE");
            TextBox TxtHML = GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHML") as TextBox;
            TxtHML.Focus();
            PerfilesGrid();
        }
        protected void TxtHMLPP_TextChanged(object sender, EventArgs e)
        {
            FechaI = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecSalPP") as TextBox).Text);
            FechaF = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecLlePP") as TextBox).Text);
            string VbHS = (GrdTray.FooterRow.FindControl("TxtHMSPP") as TextBox).Text;
            string VbHLl = (GrdTray.FooterRow.FindControl("TxtHMLPP") as TextBox).Text;
            CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "INSERT");

            TextBox TxtNumPasPP = (GrdTray.FooterRow.FindControl("TxtNumPasPP") as TextBox);
            TxtNumPasPP.Focus();
            PerfilesGrid();
        }
        protected void TxtHML_TextChanged(object sender, EventArgs e)
        {
            FechaI = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecSal") as TextBox).Text); // El indice se toma en el evento RowEditing
            FechaF = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecLle") as TextBox).Text);
            string VbHS = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHMS") as TextBox).Text;
            string VbHLl = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHML") as TextBox).Text;
            CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "UPDATE");

            TextBox TxtNumPas = GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtNumPas") as TextBox;
            TxtNumPas.Focus();
            PerfilesGrid();
        }
        protected void GrdTray_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.CommandName.Equals("AddNew"))
                {
                    PerfilesGrid();
                    string VbOri = (GrdTray.FooterRow.FindControl("DdlOrigPP") as DropDownList).SelectedValue;
                    if (VbOri.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens09LV'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe ingresar un origen')", true); return;
                    }
                    string VbDest = (GrdTray.FooterRow.FindControl("DdlDestPP") as DropDownList).SelectedValue;
                    if (VbDest.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens10LV'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un destino
                    }
                    string VbHS = (GrdTray.FooterRow.FindControl("TxtHMSPP") as TextBox).Text;
                    if (VbHS.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens11LV'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una hora de salida
                    }
                    string VbHLl = (GrdTray.FooterRow.FindControl("TxtHMLPP") as TextBox).Text;
                    if (VbHLl.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens12LV'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una hora de llegada')", true); return;
                    }

                    FechaI = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecSalPP") as TextBox).Text);
                    FechaF = Convert.ToDateTime((GrdTray.FooterRow.FindControl("TxtFecLlePP") as TextBox).Text);
                    CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    int VbNumPass = Convert.ToInt32((GrdTray.FooterRow.FindControl("TxtNumPasPP") as TextBox).Text);
                    VbNumPass = VbNumPass < 0 ? 0 : VbNumPass;
                    double VbHorasTray = Convert.ToDouble(TtalHoras.ToString().Substring(0, 2)) + (Convert.ToDouble(TtalHoras.ToString().Substring(3, 2)) / 60);
                    List<CsTypDetalleLibroVuelo> ObjDetLV = new List<CsTypDetalleLibroVuelo>();
                    var TypDetLV = new CsTypDetalleLibroVuelo()
                    {
                        CodIdDetLibroVuelo = 0,
                        CodLibroVuelo = TxtNumLv.Text.Trim(),
                        CodOrigen = VbOri,
                        HoraSalida = FechaCompletaI,
                        CodDestino = VbDest,
                        HoraLlegada = FechaCompletaF,
                        CodTipoVuelo = "0001",
                        NumPersTransp = VbNumPass,
                        NumHoraCiclo = VbHorasTray,
                        Generado = 0,
                        NroVuelo = "",
                        HoraDespegue = FechaCompletaI,
                        HoraAterrizaje = FechaCompletaF,
                        TiempoVuelo = TtalHoras.ToString().Substring(0, 2) + TtalHoras.ToString().Substring(3, 2),
                        Usu = Session["C77U"].ToString(),
                        HoraAPU = "00:00",
                        Accion = "INSERT",
                    };
                    ObjDetLV.Add(TypDetLV);
                    CsTypDetalleLibroVuelo DetLibroVuelo = new CsTypDetalleLibroVuelo();
                    DetLibroVuelo.Alimentar(ObjDetLV);
                    string Mensj = DetLibroVuelo.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    string HrMn = DetLibroVuelo.GetTHrMn();
                    // string TtlHrasVoldas = DetLibroVuelo.GetTtlHorasLV();
                    if (ViewState["HabilitaVuelos"].Equals("N"))
                    { TxtNumVuelo.Text = DetLibroVuelo.GetTtlVuelos().ToString(); }
                    TraerHorasTray(TxtNumLv.Text.Trim(), HrMn);
                    ViewState["UltimoDestino"] = VbDest;
                    BindDTrayectos();
                    PerfilesGrid();
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdTray_RowCommand", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdTray_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GrdTray.EditIndex = e.NewEditIndex;
                ViewState["Index"] = e.NewEditIndex;
                BindDTrayectos();
                PerfilesGrid();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdTray_RowEditing", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdTray_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VbOri = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("DdlOrig") as DropDownList).SelectedValue;
                if (VbOri.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens09LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un origen')", true); return; 
                }
                string VbDest = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("DdlDest") as DropDownList).SelectedValue;
                if (VbDest.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un destino')", true); return; 
                }
                string VbHS = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHMS") as TextBox).Text;
                if (VbHS.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens11LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una hora de salida')", true); return; 
                }
                string VbHLl = (GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtHML") as TextBox).Text;
                if (VbHLl.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens12LV'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una hora de llegada')", true); return; 
                }
                FechaI = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecSal") as TextBox).Text);
                FechaF = Convert.ToDateTime((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtFecLle") as TextBox).Text);
                CalculoHoras(FechaI, FechaF, VbHS, VbHLl, "UPDATE");
                if (ViewState["Validar"].Equals("N"))
                { return; }
                int VbNumPass = Convert.ToInt32((GrdTray.Rows[(int)ViewState["Index"]].FindControl("TxtNumPas") as TextBox).Text);
                VbNumPass = VbNumPass < 0 ? 0 : VbNumPass;
                double VbHorasTray = Convert.ToDouble(TtalHoras.ToString().Substring(0, 2)) + (Convert.ToDouble(TtalHoras.ToString().Substring(3, 2)) / 60);
                List<CsTypDetalleLibroVuelo> ObjDetLV = new List<CsTypDetalleLibroVuelo>();
                var TypDetLV = new CsTypDetalleLibroVuelo()
                {
                    CodIdDetLibroVuelo = Convert.ToInt32(GrdTray.DataKeys[e.RowIndex].Value.ToString()),
                    CodLibroVuelo = TxtNumLv.Text.Trim(),
                    CodOrigen = VbOri,
                    HoraSalida = FechaCompletaI,
                    CodDestino = VbDest,
                    HoraLlegada = FechaCompletaF,
                    CodTipoVuelo = "0001",
                    NumPersTransp = VbNumPass,
                    NumHoraCiclo = VbHorasTray,
                    Generado = 0,
                    NroVuelo = "",
                    HoraDespegue = FechaCompletaI,
                    HoraAterrizaje = FechaCompletaF,
                    TiempoVuelo = TtalHoras.ToString().Substring(0, 2) + TtalHoras.ToString().Substring(3, 2),
                    Usu = Session["C77U"].ToString(),
                    HoraAPU = "00:00",
                    Accion = "UPDATE",
                };
                ObjDetLV.Add(TypDetLV);
                CsTypDetalleLibroVuelo DetLibroVuelo = new CsTypDetalleLibroVuelo();
                DetLibroVuelo.Alimentar(ObjDetLV);
                string Mensj = DetLibroVuelo.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString(); }
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlTray, UpPnlTray.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
                string HrMn = DetLibroVuelo.GetTHrMn();
                TraerHorasTray(TxtNumLv.Text.Trim(), HrMn);
                ViewState["UltimoDestino"] = VbDest;
                if (ViewState["HabilitaVuelos"].Equals("N"))
                { TxtNumVuelo.Text = DetLibroVuelo.GetTtlVuelos().ToString(); }
                GrdTray.EditIndex = -1;
                BindDTrayectos();
                PerfilesGrid();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdTray_RowCommand", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdTray_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdTray.EditIndex = -1;
            BindDTrayectos();
        }
        protected void GrdTray_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                PerfilesGrid();
                string VBQuery;
                int IDDetLibroVuelo = Convert.ToInt32(GrdTray.DataKeys[e.RowIndex].Value.ToString());
                string VbOri = (GrdTray.Rows[e.RowIndex].FindControl("LblOrigP") as Label).Text;
                string VbDest = (GrdTray.Rows[e.RowIndex].FindControl("LblDestP") as Label).Text;
                string VbFI = (GrdTray.Rows[e.RowIndex].FindControl("LblFecSal") as Label).Text;
                string VbFF = (GrdTray.Rows[e.RowIndex].FindControl("LblFecLle") as Label).Text;
                string VbHS = (GrdTray.Rows[e.RowIndex].FindControl("LblHMS") as Label).Text;
                string VbHL = (GrdTray.Rows[e.RowIndex].FindControl("LblHML") as Label).Text;
                string VbTiempo = (GrdTray.Rows[e.RowIndex].FindControl("LblTimeV") as Label).Text;
                string VbPass = (GrdTray.Rows[e.RowIndex].FindControl("LblNumPas") as Label).Text;
                string Org_Des = VbOri.Trim() + " | H. Salida: " + VbFI + " " + VbHS + " | " + VbDest + " | H. Llegada: " + VbFF + " " + VbHL;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        /* VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 21,'{0}','{1}','{2}','{3}','DELETE',{4},{5},0,0,'01-01-01','01-01-01','01-01-01'",
                         TxtNumLv.Text.Trim(), VbTiempo, Org_Des, Session["C77U"].ToString(), IDDetLibroVuelo, VbPass);*/
                        VBQuery = "EXEC SP_PANTALLA__Servicio_Manto2 21, @LV, @Tim, @OD, @US,'DELETE', @IdDLv, @Px,0,0,'01-01-01','01-01-01','01-01-01'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@LV", TxtNumLv.Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Tim", VbTiempo);
                                sqlCmd.Parameters.AddWithValue("@OD", Org_Des);
                                sqlCmd.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@IdDLv", IDDetLibroVuelo);
                                sqlCmd.Parameters.AddWithValue("@Px", VbPass);
                                string HrMn = (string)sqlCmd.ExecuteScalar();
                                Transac.Commit();
                                BindDTrayectos();
                                TraerHorasTray(TxtNumLv.Text.Trim(), HrMn);
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DETLLE LIBRO VUELO", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdTray_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string LtxtSql = "";
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','OriDes',1,1,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                    DropDownList DdlOrigPP = (e.Row.FindControl("DdlOrigPP") as DropDownList);
                    DdlOrigPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlOrigPP.DataTextField = "Nombre";
                    DdlOrigPP.DataValueField = "CodUbicaGeogr";
                    DdlOrigPP.DataBind();
                    DdlOrigPP.SelectedValue = ViewState["UltimoDestino"].ToString().Trim();

                    DropDownList DdlDestPP = (e.Row.FindControl("DdlDestPP") as DropDownList);
                    DdlDestPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlDestPP.DataTextField = "Nombre";
                    DdlDestPP.DataValueField = "CodUbicaGeogr";
                    DdlDestPP.DataBind();


                    TextBox TxtFecSalPP = (e.Row.FindControl("TxtFecSalPP") as TextBox);
                    TxtFecSalPP.Text = TxtFecha.Text;
                    CalendarExtender CalFechSPP = (e.Row.FindControl("CalFechSPP") as CalendarExtender);
                    DateTime DiaI = Convert.ToDateTime(TxtFecha.Text);
                    CalFechSPP.StartDate = Convert.ToDateTime(TxtFecha.Text);
                    CalFechSPP.EndDate = DiaI.AddDays(1);

                    TextBox TxtFecLlePP = (e.Row.FindControl("TxtFecLlePP") as TextBox);
                    TxtFecLlePP.Text = TxtFecha.Text;
                    CalendarExtender CalFechLPP = (e.Row.FindControl("CalFechLPP") as CalendarExtender);
                    DiaI = Convert.ToDateTime(TxtFecha.Text);
                    CalFechLPP.StartDate = Convert.ToDateTime(TxtFecha.Text);
                    CalFechLPP.EndDate = DiaI.AddDays(1);
                    ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                    if (ViewState["Procesado"].Equals("S"))
                    {
                        if (IbtAddNew != null)
                        {
                            IbtAddNew.Enabled = false;
                            DataRow[] Result = Idioma.Select("Objeto= 'CkbProcesado'");
                            foreach (DataRow row in Result)
                            { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                        }
                    }
                    else
                    {
                        if (IbtAddNew != null)
                        {
                            IbtAddNew.Enabled = true;
                            DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                            foreach (DataRow row in Result)
                            { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }

                        }
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','OriDes',1,1,0,0,'01-1-2009','01-01-1900','01-01-1900'", dr["CodOrigen"].ToString().Trim());
                    DropDownList DdlOrig = (e.Row.FindControl("DdlOrig") as DropDownList);
                    DdlOrig.DataSource = Cnx.DSET(LtxtSql);
                    DdlOrig.DataTextField = "Nombre";
                    DdlOrig.DataValueField = "CodUbicaGeogr";
                    DdlOrig.DataBind();
                    DdlOrig.SelectedValue = dr["CodOrigen"].ToString().Trim();

                    DataRowView DrD = e.Row.DataItem as DataRowView;
                    LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','OriDes',1,1,0,0,'01-1-2009','01-01-1900','01-01-1900'", DrD["CodDestino"].ToString().Trim());
                    DropDownList DdlDest = (e.Row.FindControl("DdlDest") as DropDownList);
                    DdlDest.DataSource = Cnx.DSET(LtxtSql);
                    DdlDest.DataTextField = "Nombre";
                    DdlDest.DataValueField = "CodUbicaGeogr";
                    DdlDest.DataBind();
                    DdlDest.SelectedValue = DrD["CodDestino"].ToString().Trim();

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
                    if (ViewState["Procesado"].Equals("S"))
                    {
                        if (imgE != null)
                        {
                            imgE.Enabled = false;
                            DataRow[] Result = Idioma.Select("Objeto= 'CkbProcesado'");
                            foreach (DataRow row in Result)
                            { imgE.ToolTip = row["Texto"].ToString().Trim(); }
                        }

                        if (imgD != null)
                        {
                            imgD.Enabled = false;
                            DataRow[] Result = Idioma.Select("Objeto= 'CkbProcesado'");
                            foreach (DataRow row in Result)
                            { imgD.ToolTip = row["Texto"].ToString().Trim(); }
                        }
                    }
                    else
                    {
                        if (imgE != null)
                        {
                            imgE.Enabled = true;
                            DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                            foreach (DataRow RowIdioma in Result)
                            { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                        }

                        if (imgD != null)
                        {
                            imgD.Enabled = true;
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdTray_RowDataBound", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdTray_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTray.PageIndex = e.NewPageIndex;
            BindDTrayectos();
            PerfilesGrid();
        }
        //******************************************  Reporte de mantenimiento *********************************************************
        protected void BindBDdlBusqRte()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','BSQDRLV',0,0,0,0,'01-01-1','02-01-1','03-01-1'", TxtNumLv.Text.Trim());
            DdlBusqRte.DataSource = Cnx.DSET(LtxtSql);
            DdlBusqRte.DataMember = "Datos";
            DdlBusqRte.DataTextField = "NumRte";
            DdlBusqRte.DataValueField = "Codigo";
            DdlBusqRte.DataBind();
        }
        protected void BindDdlRte()
        {
            string LtxtSql = "";

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','FTE',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            DdlFuente.DataSource = Cnx.DSET(LtxtSql);
            DdlFuente.DataMember = "Datos";
            DdlFuente.DataTextField = "Descripcion";
            DdlFuente.DataValueField = "Codigo";
            DdlFuente.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','STD',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            DdlEstad.DataSource = Cnx.DSET(LtxtSql);
            DdlEstad.DataMember = "Datos";
            DdlEstad.DataTextField = "Descripcion";
            DdlEstad.DataValueField = "CodStatus";
            DdlEstad.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','ATA',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            DdlAtaRte.DataSource = Cnx.DSET(LtxtSql);
            DdlAtaRte.DataMember = "Datos";
            DdlAtaRte.DataTextField = "Descripcion";
            DdlAtaRte.DataValueField = "CodCapitulo";
            DdlAtaRte.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','PNRTE',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            DdlPnRte.DataSource = Cnx.DSET(LtxtSql);
            DdlPnRte.DataMember = "Datos";
            DdlPnRte.DataTextField = "PN";
            DdlPnRte.DataValueField = "Codigo";
            DdlPnRte.DataBind();
        }
        protected void BindDdlRteCondicional(int Act, int Inact, string Categ, string LicGen, string LicCump, string LicVer, string CodTall, string CodClasf,
            string CodPos, string UsuGen, string UsuCump, string UsuDif, string UsuVer)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','TLLR',0,0,0,0,'01-01-1','02-01-1','03-01-1'", CodTall);
            DdlTall.DataSource = Cnx.DSET(LtxtSql);
            DdlTall.DataMember = "Datos";
            DdlTall.DataTextField = "NomTaller";
            DdlTall.DataValueField = "CodTaller";
            DdlTall.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','CSF',0,0,0,0,'01-01-1','02-01-1','03-01-1'", CodClasf);
            DdlClasf.DataSource = Cnx.DSET(LtxtSql);
            DdlClasf.DataMember = "Datos";
            DdlClasf.DataTextField = "Descripcion";
            DdlClasf.DataValueField = "Codigo";
            DdlClasf.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{2}','','','CatM',{1},0,0,0,'01-01-1','02-01-1','03-01-1'",
               DdlClasf.Text, DdlClasf.SelectedValue.Equals("") ? "0" : DdlMatri.Text, Categ);
            DdlCatgr.DataSource = Cnx.DSET(LtxtSql);
            DdlCatgr.DataMember = "Datos";
            DdlCatgr.DataTextField = "CodCategoriaMel";
            DdlCatgr.DataValueField = "IdCategoria";
            DdlCatgr.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PosR',0,0,0,0,'01-01-1','02-01-1','03-01-1'", CodPos);
            DdlPosRte.DataSource = Cnx.DSET(LtxtSql);
            DdlPosRte.DataMember = "Datos";
            DdlPosRte.DataTextField = "Descripcion";
            DdlPosRte.DataValueField = "Codigo";
            DdlPosRte.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','TECA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuGen);
            DdlGenerado.DataSource = Cnx.DSET(LtxtSql);
            DdlGenerado.DataMember = "Datos";
            DdlGenerado.DataTextField = "Tecnico";
            DdlGenerado.DataValueField = "CodPersona";
            DdlGenerado.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','TECA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuCump);
            DdlCumpl.DataSource = Cnx.DSET(LtxtSql);
            DdlCumpl.DataMember = "Datos";
            DdlCumpl.DataTextField = "Tecnico";
            DdlCumpl.DataValueField = "CodPersona";
            DdlCumpl.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','TECA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuDif);
            DdlTecDif.DataSource = Cnx.DSET(LtxtSql);
            DdlTecDif.DataMember = "Datos";
            DdlTecDif.DataTextField = "Tecnico";
            DdlTecDif.DataValueField = "CodPersona";
            DdlTecDif.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','TECA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuVer);
            DdlVerif.DataSource = Cnx.DSET(LtxtSql);
            DdlVerif.DataMember = "Datos";
            DdlVerif.DataTextField = "Tecnico";
            DdlVerif.DataValueField = "CodPersona";
            DdlVerif.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuGen, LicGen);
            DdlLicGene.DataSource = Cnx.DSET(LtxtSql);
            DdlLicGene.DataMember = "Datos";
            DdlLicGene.DataTextField = "Licencia";
            DdlLicGene.DataValueField = "Codigo";
            DdlLicGene.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuCump, LicCump);
            DdlLicCump.DataSource = Cnx.DSET(LtxtSql);
            DdlLicCump.DataMember = "Datos";
            DdlLicCump.DataTextField = "Licencia";
            DdlLicCump.DataValueField = "Codigo";
            DdlLicCump.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", UsuVer, LicVer);
            DdlLicVer.DataSource = Cnx.DSET(LtxtSql);
            DdlLicVer.DataMember = "Datos";
            DdlLicVer.DataTextField = "Licencia";
            DdlLicVer.DataValueField = "Codigo";
            DdlLicVer.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','OTPP',{0},{1},0,0,'01-01-1','02-01-1','03-01-1'", DdlMatri.Text, DdlOtRte.Text.Equals("") ? "0" : DdlOtRte.Text);
            DdlOtRte.DataSource = Cnx.DSET(LtxtSql);
            DdlOtRte.DataMember = "Datos";
            DdlOtRte.DataTextField = "OT";
            DdlOtRte.DataValueField = "CodNumOrdenTrab";
            DdlOtRte.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','TpRte',{0},{1},{2},0,'01-01-1','02-01-1','03-01-1'", Act, Inact, DdlTipRte.Text.Equals("") ? "0" : DdlTipRte.Text);
            DdlTipRte.DataSource = Cnx.DSET(LtxtSql);
            DdlTipRte.DataMember = "Datos";
            DdlTipRte.DataTextField = "TipoReporte";
            DdlTipRte.DataValueField = "CodReporte";
            DdlTipRte.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PRIO',0,0,0,0,'01-01-1','02-01-1','03-01-1'", ViewState["CodPrioridad"].ToString());
            DdlPrioridadOT.DataSource = Cnx.DSET(LtxtSql);
            DdlPrioridadOT.DataMember = "Datos";
            DdlPrioridadOT.DataTextField = "Descripcion";
            DdlPrioridadOT.DataValueField = "CodPrioridadSolicitudMat";
            DdlPrioridadOT.DataBind();
        }
        protected void TraerDatosRtes(int NumRte)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbFecha;
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 2,'','','','','',{0},0,0,0,'01-01-1','02-01-1','03-01-1'", NumRte);
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader SDR = SqlC.ExecuteReader();
                    if (SDR.Read())
                    {
                        string VbCodCat = SDR["CodCategoriaMel"].ToString().Trim();
                        string VbLicGen = SDR["NumLicTecAbre"].ToString().Trim();
                        string VbLicCump = SDR["NumLicTecCierre"].ToString().Trim();
                        string VbLicVer = SDR["NumLicenciaRM"].ToString().Trim();
                        string VbCodTall = SDR["CodTaller"].ToString().Trim();
                        string VbCodClasf = SDR["CodClasifReporteManto"].ToString().Trim();
                        string VbCodPos = SDR["Posicion"].ToString().Trim();
                        string UsuGen = SDR["ReportadoPor"].ToString().Trim();
                        string UsuCump = SDR["CodTecnico"].ToString().Trim();
                        string UsuDif = SDR["CodUsuarioDiferido"].ToString().Trim();
                        string UsuVer = SDR["CodInspectorVerifica"].ToString().Trim();
                        ViewState["ESTAPPT"] = SDR["EstaPPT"].ToString().Trim();
                        ViewState["CodPrioridad"] = HttpUtility.HtmlDecode(SDR["CodPrioridad"].ToString().Trim());
                        BindDdlRteCondicional(0, 1, VbCodCat, VbLicGen, VbLicCump, VbLicVer, VbCodTall, VbCodClasf, VbCodPos, UsuGen, UsuCump, UsuDif, UsuVer);
                        DdlAeroRte.Text = SDR["CodAeronave"].ToString();
                        TxtNroRte.Text = SDR["NumReporte"].ToString();
                        TxtConsTall.Text = SDR["ConsecutivoROTP"].ToString().Trim();
                        DdlTipRte.SelectedValue = SDR["TipoReporte"].ToString();
                        DdlFuente.SelectedValue = SDR["Fuente"].ToString().Trim();
                        TxtCas.Text = SDR["NumCasilla"].ToString();
                        DdlTall.Text = VbCodTall;
                        DdlEstad.SelectedValue = SDR["Estado"].ToString().Trim();
                        CkbNotif.Checked = Convert.ToBoolean(SDR["Notificado"].ToString());
                        BtnNotificar.Enabled = CkbNotif.Checked == true ? false : true;
                        DdlClasf.SelectedValue = VbCodClasf;
                        DdlCatgr.SelectedValue = VbCodCat;
                        TxtDocRef.Text = SDR["DocumentoRef"].ToString().Trim();
                        DdlPosRte.SelectedValue = VbCodPos;
                        DdlAtaRte.SelectedValue = SDR["UbicacionTecnica"].ToString().Trim();
                        DdlGenerado.SelectedValue = UsuGen;
                        DdlLicGene.SelectedValue = VbLicGen;
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaReporte"].ToString().Trim());
                        TxtFecDet.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaProyectada"].ToString().Trim());
                        TxtFecPry.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
                        DdlOtRte.SelectedValue = SDR["OtPrincipal"].ToString().Trim();
                        DdlBasRte.SelectedValue = SDR["CodBase"].ToString().Trim();
                        DdlCumpl.SelectedValue = UsuCump;
                        DdlLicCump.SelectedValue = VbLicCump;
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaCumplimiento"].ToString().Trim());
                        TxtFecCump.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
                        RdbPgSi.Checked = Convert.ToBoolean(SDR["ProgramadoSi"].ToString());
                        RdbPgNo.Checked = Convert.ToBoolean(SDR["ProgramadoNo"].ToString());
                        RdbFlCSi.Checked = Convert.ToBoolean(SDR["FallaConfirmadaSi"].ToString());
                        RdbFlCNo.Checked = Convert.ToBoolean(SDR["FallaConfirmadaNo"].ToString());
                        CkbRII.Checked = Convert.ToBoolean(SDR["RII"].ToString());
                        DdlPnRte.Text = SDR["ParteNumero"].ToString().Trim();
                        TxtSnRte.Text = HttpUtility.HtmlDecode(SDR["SerieNumero"].ToString().Trim());
                        TxtTtlAKSN.Text = SDR["TT_A_C"].ToString().Trim();
                        TxtHPrxCu.Text = SDR["HraProxCump"].ToString().Trim();
                        TxtNexDue.Text = SDR["Next_Due"].ToString().Trim();
                        TxtDescRte.Text = HttpUtility.HtmlDecode(SDR["Reporte"].ToString().Trim());
                        txtAccCrr.Text = HttpUtility.HtmlDecode(SDR["AccionCorrectiva"].ToString().Trim());
                        TxtAcciParc.Text = HttpUtility.HtmlDecode(SDR["AccionParcial"].ToString().Trim());
                        DdlTecDif.SelectedValue = UsuDif;
                        DdlVerif.SelectedValue = UsuVer;
                        DdlLicVer.SelectedValue = VbLicVer;
                        CkbTearDown.Checked = Convert.ToBoolean(SDR["TearDown"].ToString());
                        ViewState["PasoOT"] = HttpUtility.HtmlDecode(SDR["PasoOT"].ToString().Trim());
                        TxtOtSec.Text = SDR["OtSec"].ToString().Trim();
                        int borrar = Convert.ToInt32(SDR["IDMroRepOT"].ToString());
                        ViewState["IDMroRepOT"] = Convert.ToInt32(SDR["IDMroRepOT"].ToString());
                        ViewState["BloquearDetalle"] = Convert.ToInt32(SDR["BloquearDetalle"].ToString());
                        ViewState["TtlRegDet"] = Convert.ToInt32(SDR["TtlRegDet"].ToString());
                        ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(SDR["CargaMasiva"].ToString().Trim());
                    }
                    SDR.Close();
                    Cnx2.Close();
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente con la consulta')", true);
            }
        }
        protected void ActivarBtnRpt(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnReserva.Enabled = Otr;
            BtnConsultar.Enabled = Otr;
            BtnImprimir.Enabled = Ip;
            BtnEliminar.Enabled = El;
            BtnSnOnOf.Enabled = Otr;
            BtnExporRte.Enabled = Otr;
            BtnDatos.Enabled = Otr;
            BtnVuelos.Enabled = Otr;
            BtnManto.Enabled = Otr;
            UpPnlBtnPpl.Update();
        }
        protected void ActivarCampRte(bool Ing, bool Edi, string accion)
        {
            if (DdlEstad.SelectedValue.Equals("C") && DdlTipRte.Enabled == false)
            {
                if (Convert.ToInt32(ViewState["VblCE6Rte"]) == 1)
                {
                    DdlEstad.Enabled = Edi;
                    if (DdlVerif.Text.Equals(""))
                    {
                        // DdlVerif.Enabled = Edi;
                        DdlVerif.Text = Session["C77U"].ToString().Trim();
                        string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DdlVerif.Text, "");
                        DdlLicVer.DataSource = Cnx.DSET(LtxtSql);
                        DdlLicVer.DataMember = "Datos";
                        DdlLicVer.DataTextField = "Licencia";
                        DdlLicVer.DataValueField = "Codigo";
                        DdlLicVer.DataBind();
                    }
                    DdlLicVer.Enabled = Edi;
                    CkbTearDown.Enabled = Edi;
                }
            }
            else
            {
                DdlOtRte.Enabled = false;
                DdlEstad.Enabled = Edi;
                DdlTipRte.Enabled = Edi;
                DdlFuente.Enabled = Edi;
                DdlTall.Enabled = Edi;
                DdlClasf.Enabled = Edi;
                DdlCatgr.Enabled = Edi;
                TxtDocRef.Enabled = Edi;
                DdlPosRte.Enabled = Edi;
                DdlAtaRte.Enabled = Edi;
                DdlGenerado.Enabled = ViewState["UsuDefecto"].Equals("S") ? false : Edi;
                DdlLicGene.Enabled = Edi;
                IbtFecDet.Enabled = Edi;
                IbtFecPry.Enabled = Edi;
                if (DdlOtRte.Text.Equals("0") && !LblNumLVTit.Text.Trim().Equals(""))
                { DdlOtRte.Enabled = Edi; }
                DdlBasRte.Enabled = Edi;
                DdlCumpl.Enabled = ViewState["UsuDefecto"].Equals("S") ? false : Edi;
                DdlLicCump.Enabled = Edi;
                IbtFecCump.Enabled = Edi;
                RdbPgSi.Enabled = Edi;
                RdbPgNo.Enabled = Edi;
                RdbFlCSi.Enabled = Edi;
                RdbFlCNo.Enabled = Edi;
                CkbRII.Enabled = Edi;
                if (accion.Equals("UPDATE"))
                {
                    DdlPnRte.Enabled = ViewState["ESTAPPT"].ToString().Equals("S") ? false : Edi;
                    TxtSnRte.Enabled = ViewState["ESTAPPT"].ToString().Equals("S") ? false : Edi;
                }
                else
                { DdlPnRte.Enabled = Edi; TxtSnRte.Enabled = Edi; }
                DdlPnRte.ToolTip = "";
                TxtSnRte.ToolTip = "";
                if (accion.Equals("UPDATE"))
                {
                    if (DdlPnRte.Enabled == false)
                    { DdlPnRte.ToolTip = "El reporte se encuentra en una propuesta"; TxtSnRte.ToolTip = "El reporte se encuentra en una propuesta"; }
                    if (DdlPnRte.Text.Trim().Equals("") && !DdlOtRte.Text.Trim().Equals("0") && LblNumLVTit.Text.Trim().Equals("") && ViewState["ESTAPPT"].ToString().Equals("N"))
                    { DdlAeroRte.Enabled = DdlAeroRte.Text.Equals("0") ? Edi : false; }
                }
                else
                { DdlAeroRte.Enabled = DdlAeroRte.Text.Equals("0") ? Edi : false; }
                TxtTtlAKSN.Enabled = Edi;
                TxtHPrxCu.Enabled = Edi;
                txtAccCrr.Enabled = Edi;
                TxtAcciParc.Enabled = Edi;
                DdlTecDif.Enabled = Edi;
                if (accion.Equals("UPDATE"))
                {
                    if (ViewState["EditCampoRte"].Equals("S") && Convert.ToInt32(ViewState["VblCE6Rte"].ToString()) == 1)
                    { TxtDescRte.Enabled = Edi; }
                    if (ViewState["EditCampoRte"].Equals("S"))
                    {
                        TxtDescRte.Enabled = Edi;
                    }
                    else
                    {
                        if (Convert.ToInt32(ViewState["VblCE6Rte"]) == 1)
                        { TxtDescRte.Enabled = Edi; }
                    }
                }
                else { TxtDescRte.Enabled = Edi; TxtDescRte.Enabled = Edi; }
            }
        }
        protected void LimpiarCamposRte()
        {
            TxtOtSec.Text = "0";
            TxtNroRte.Text = "0";
            TxtConsTall.Text = "";
            DdlTipRte.Text = "7777";
            DdlFuente.Text = "";
            TxtCas.Text = "";
            DdlTall.Text = "";
            DdlEstad.Text = "A";
            CkbNotif.Checked = false;
            DdlClasf.Text = "";
            DdlCatgr.Text = "";
            TxtDocRef.Text = "";
            DdlPosRte.Text = "";
            DdlAtaRte.Text = "";
            DdlGenerado.Text = "";
            DdlLicGene.Text = "";
            TxtFecDet.Text = "";
            TxtFecPry.Text = "";
            DdlOtRte.Text = "0";
            DdlBasRte.Text = "";
            DdlCumpl.SelectedValue = "";
            DdlLicCump.Text = "";
            TxtFecCump.Text = "";
            RdbPgSi.Checked = false;
            RdbPgNo.Checked = true;
            RdbFlCSi.Checked = false;
            RdbFlCNo.Checked = true;
            CkbRII.Checked = false;
            DdlPnRte.Text = "";
            TxtSnRte.Text = "";
            TxtTtlAKSN.Text = "0";
            TxtHPrxCu.Text = "0";
            TxtNexDue.Text = "0";
            TxtDescRte.Text = "";
            txtAccCrr.Text = "";
            TxtAcciParc.Text = "";
            DdlTecDif.Text = "";
            DdlVerif.Text = "";
            DdlLicVer.Text = "";
            CkbTearDown.Checked = false;
        }
        protected void ValidarRpte(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                if (DdlAeroRte.Text.Equals("0") && DdlPnRte.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens01'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una aeronave o P/N')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlAeroRte.Text.Equals("0") && DdlAeroRte.Enabled == true && DdlPnRte.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens02'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una aeronave')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlTipRte.Text.Trim().Equals("7777"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens03'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un tipo reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlFuente.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens04'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fuente')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlClasf.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens05'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una clasificación')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlCatgr.Text.Trim().Equals("") && (DdlClasf.Text.Trim().Equals("CARRY OVER") || DdlClasf.Text.Trim().Equals("CARRY FORWARD")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens06'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una categoría')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtDocRef.Text.Trim().Equals("") && DdlClasf.Text.Trim().Equals("CARRY OVER"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens07'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un documento referencia')", true);
                    ViewState["Validar"] = "N";
                    TxtDocRef.Focus();
                    return;
                }
                if (DdlAtaRte.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens08'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una ATA')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlGenerado.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens09'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el usuario que genera el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlLicGene.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens10'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la licencia  del usuario que genera el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtFecDet.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha')", true);
                    ViewState["Validar"] = "N";
                    TxtFecDet.Focus();
                    return;
                }
                if (TxtFecPry.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens12'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha de proyección')", true);
                    ViewState["Validar"] = "N";
                    TxtFecPry.Focus();
                    return;
                }
                if (DdlBasRte.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens13'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una base')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlCumpl.Text.Trim().Equals("") && DdlEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens14'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el usuario que cierra el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlLicCump.Text.Trim().Equals("") && DdlEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens15'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la licencia del usuario que cierra el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtFecCump.Text.Trim().Equals("") && DdlEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens16'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha de cumplimiento')", true);
                    ViewState["Validar"] = "N";
                    TxtFecCump.Focus();
                    return;
                }
                if (DdlPnRte.Text.Trim().Equals("") && !TxtSnRte.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens17'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un P/N si el campo S/N se encuentra con información')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlEstad.Text.Equals("A") && !txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens18'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe cerrar el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlEstad.Text.Equals("C") && txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la acción correctiva')", true);
                    ViewState["Validar"] = "N";
                    txtAccCrr.Focus();
                    return;
                }
                if (DdlEstad.Text.Equals("A") && !TxtFecCump.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens20'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe cerrar el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtDescRte.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens21'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la descripción del reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtAcciParc.Text.Equals("") && (DdlClasf.Text.Trim().Equals("CARRY OVER") || DdlClasf.Text.Trim().Equals("CARRY FORWARD")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens22'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una acción parcial si el reporte está clasificado como diferido')", true);
                    ViewState["Validar"] = "N";
                    TxtAcciParc.Focus();
                    return;
                }
                if (!TxtAcciParc.Text.Equals("") && DdlTecDif.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens23'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el técnico que difiere el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["PermiteFechaIgualDetPry"].Equals("N") && TxtFecDet.Text == TxtFecPry.Text && DdlClasf.Text.Trim().Equals("CARRY FORWARD"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens24'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //La fecha de detección y la fecha de proyección no pueden ser iguales cuando es un reporte C/F.')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if ((DdlVerif.Text.Equals("") && !DdlLicVer.Text.Equals("")) || (!DdlVerif.Text.Equals("") && DdlLicVer.Text.Equals("")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens25'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la persona que verifica y licencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarRpte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void CalcularFechaPry()
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 3,'','','','','',@Cat,0,0,0,'01-01-1','02-01-1','03-01-1'");
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                string borrar = DdlCatgr.Text.Equals("") ? "0" : DdlCatgr.Text;
                SC.Parameters.AddWithValue("@Cat", DdlCatgr.Text.Equals("") ? "0" : DdlCatgr.Text);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    int VbCritDias = Convert.ToInt32(SDR["CriterioDias"].ToString());
                    DateTime VbProy = Convert.ToDateTime(TxtFecDet.Text).AddDays(VbCritDias);
                    TxtFecPry.Text = String.Format("{0:dd/MM/yyyy}", VbProy);
                }
            }
        }
        protected void CalcularNexDue(string TT, string Prox)
        {
            string StrTT, StrProx;
            double VbTT, VbProx;
            CultureInfo Culture = new CultureInfo("en-US");
            StrTT = TT.Trim().Equals("") ? "0" : TT.Trim();
            VbTT = StrTT.Length == 0 ? 0 : Convert.ToDouble(StrTT, Culture);

            StrProx = Prox.Trim().Equals("") ? "0" : Prox.Trim();
            VbProx = StrProx.Length == 0 ? 0 : Convert.ToDouble(StrProx, Culture);

            TxtNexDue.Text = Convert.ToString(VbTT + VbProx);
        }
        protected void DdlBusqRte_TextChanged(object sender, EventArgs e)
        {
            TraerDatosRtes(Convert.ToInt32(DdlBusqRte.SelectedValue));
            PerfilesGrid();
        }
        protected void DdlEstad_TextChanged(object sender, EventArgs e)
        {
            if (DdlTipRte.Enabled == true)
            {
                string LtxtSql;
                if (DdlEstad.SelectedValue.Equals("C"))
                {

                    DdlCumpl.SelectedValue = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlCumpl.SelectedValue;
                    DdlLicCump.Text = "";
                    LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DdlCumpl.SelectedValue);
                    DdlLicCump.DataSource = Cnx.DSET(LtxtSql);
                    DdlLicCump.DataMember = "Datos";
                    DdlLicCump.DataTextField = "Licencia";
                    DdlLicCump.DataValueField = "Codigo";
                    DdlLicCump.DataBind();
                }
                else
                {
                    if (BtnIngresar.Text.Equals("Aceptar"))
                    {
                        DdlGenerado.SelectedValue = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlGenerado.SelectedValue;
                        DdlLicGene.Text = "";
                    }
                    LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','LICTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DdlGenerado.SelectedValue);
                    DdlLicGene.DataSource = Cnx.DSET(LtxtSql);
                    DdlLicGene.DataMember = "Datos";
                    DdlLicGene.DataTextField = "Licencia";
                    DdlLicGene.DataValueField = "Codigo";
                    DdlLicGene.DataBind();
                }
            }
            else
            {
                if (DdlEstad.SelectedValue.Equals("A"))
                {
                    DdlVerif.Text = "";
                    DdlLicVer.Text = "";
                }
            }
        }
        protected void DdlClasf_TextChanged(object sender, EventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','CatM',{1},0,0,0,'01-01-1','02-01-1','03-01-1'", DdlClasf.Text, DdlMatri.Text);
            DdlCatgr.DataSource = Cnx.DSET(LtxtSql);
            DdlCatgr.DataMember = "Datos";
            DdlCatgr.DataTextField = "CodCategoriaMel";
            DdlCatgr.DataValueField = "IdCategoria";
            DdlCatgr.DataBind();
            DdlCatgr.Text = "";
            if (DdlClasf.Text.Equals("CARRY OVER"))
            { IbtFecPry.Enabled = false; }
            else
            { IbtFecPry.Enabled = true; }
        }
        protected void DdlCatgr_TextChanged(object sender, EventArgs e)
        {
            if (!DdlCatgr.Text.Equals(""))
            { CalcularFechaPry(); }
        }
        protected void TxtFecDet_TextChanged(object sender, EventArgs e)
        {
            CldFecPry.StartDate = Convert.ToDateTime(TxtFecDet.Text);
            CldFecCump.StartDate = Convert.ToDateTime(TxtFecDet.Text);
            if (DdlCatgr.Text.Equals(""))
            { TxtFecPry.Text = TxtFecDet.Text; }
            else { CalcularFechaPry(); }
            TxtFecCump.Text = "";
        }
        protected void TxtTtlAKSN_TextChanged(object sender, EventArgs e)
        {
            CalcularNexDue(TxtTtlAKSN.Text, TxtHPrxCu.Text);
            TxtHPrxCu.Focus();
        }
        protected void TxtHPrxCu_TextChanged(object sender, EventArgs e)
        {
            CalcularNexDue(TxtTtlAKSN.Text, TxtHPrxCu.Text);
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["BtnAccion"].ToString() == "")
                {
                    ViewState["BtnAccion"] = "Nuevo";
                    ActivarBtnRpt(true, false, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    LimpiarCamposRte();
                    DdlAeroRte.Text = DdlMatri.Text;
                    TxtFecDet.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    TxtFecPry.Text = TxtFecDet.Text;
                    CldFecPry.StartDate = Convert.ToDateTime(TxtFecDet.Text);
                    CldFecCump.StartDate = Convert.ToDateTime(TxtFecDet.Text);
                    ActivarCampRte(true, true, "Ingresar");
                    string vbleUsuGe = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlGenerado.SelectedValue;
                    BindDdlRteCondicional(1, 1, "", "", "", "", "", "", "", vbleUsuGe.Trim(), "", "", "");
                    DdlGenerado.SelectedValue = vbleUsuGe.Trim();
                    DdlBusqRte.SelectedValue = "0";
                    DdlBusqRte.Enabled = false;
                    ViewState["PasoOT"] = "";
                    ViewState["CodPrioridad"] = "";
                    ViewState["BloquearDetalle"] = 0;
                    Result = Idioma.Select("Objeto= 'MensConfIng'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                else
                {
                    ValidarRpte("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    DateTime? FecCump;
                    if (TxtFecCump.Text.Equals(""))
                    { FecCump = null; }
                    else
                    { FecCump = Convert.ToDateTime(TxtFecCump.Text); }

                    List<ClsTypLvDetalleManto> ObjLvDetManto = new List<ClsTypLvDetalleManto>();
                    var TypLvDetManto = new ClsTypLvDetalleManto()
                    {
                        FechaProyectada = Convert.ToDateTime(TxtFecPry.Text.Trim()),
                        FechaCumplimiento = FecCump,
                        FechaReporte = Convert.ToDateTime(TxtFecDet.Text),
                        FechaInicio = null,
                        FechaVerificacion = null,
                        CodIdLvDetManto = 0,
                        CodLibroVuelo = LblNumLVTit.Text.Trim(),
                        CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                        NumCasilla = "0",
                        Reporte = TxtDescRte.Text.Trim(),
                        AccionCorrectiva = txtAccCrr.Text.Trim(),
                        CodTecnico = DdlCumpl.SelectedValue,
                        CodClaseReporteManto = DdlFuente.Text.Trim(),
                        CodClasifReporteManto = DdlClasf.Text.Trim(),
                        CodCategoriaMel = DdlCatgr.Text.Trim(),
                        CodStatus = DdlEstad.Text.Trim(),
                        DocumentoRef = TxtDocRef.Text.Trim(),
                        UbicacionTecnica = DdlAtaRte.Text.Trim(),
                        BanderaOrdenTrabajo = Convert.ToInt32(DdlTipRte.Text),
                        NroVuelo = "",
                        CodBase = DdlBasRte.Text.Trim(),
                        Usu = Session["C77U"].ToString().Trim(),
                        ConsInterno = "",
                        Posicion = DdlPosRte.Text.Trim(),
                        Programado = RdbPgSi.Checked == true ? 1 : 2,
                        FallaConfirmada = RdbFlCSi.Checked == true ? 1 : 2,
                        ReportadoPor = DdlGenerado.Text.Trim(),
                        AccionParcial = TxtAcciParc.Text.Trim(),
                        CodOt = Convert.ToInt32(DdlOtRte.Text),
                        CodUsuarioDiferido = DdlTecDif.Text.Trim(),
                        VerificadoRM = DdlVerif.Text.Trim().Equals("") ? 0 : 1,
                        CodInspectorVerifica = DdlVerif.Text.Trim(),
                        NumLicenciaRM = DdlLicVer.Text.Trim(),
                        TearDown = CkbTearDown.Checked == true ? 1 : 0,
                        RII = CkbRII.Checked == true ? 1 : 0,
                        Notificado = CkbNotif.Checked == true ? 1 : 0,
                        NumLicTecCierre = DdlLicCump.Text.Trim(),
                        TT_A_C = Convert.ToDouble(TxtTtlAKSN.Text),
                        HraProxCump = Convert.ToDouble(TxtHPrxCu.Text),
                        Next_Due = Convert.ToDouble(TxtNexDue.Text),
                        NumLicTecAbre = DdlLicGene.Text.Trim(),
                        IdPosicionTT = null,
                        Accion = "INSERT",
                    };
                    ObjLvDetManto.Add(TypLvDetManto);
                    List<ClsTypLvDetalleManto> ObjMROReporteOTPpal = new List<ClsTypLvDetalleManto>();
                    var TypMROReporteOTPpal = new ClsTypLvDetalleManto()
                    {
                        IDMroRepOT = 0,
                        PasoOT = ViewState["PasoOT"].ToString().Trim(),
                        NumReporte = 0,
                        CodTaller = DdlTall.Text.Trim(),
                        ParteNumero = DdlPnRte.Text.Trim(),
                        SerieNumero = TxtSnRte.Text.Trim(),
                        ConsecutivoROTP = TxtConsTall.Text.Trim(),
                        SubOT = Convert.ToInt32(TxtOtSec.Text),
                    };
                    ObjMROReporteOTPpal.Add(TypMROReporteOTPpal);

                    ClsTypLvDetalleManto ClsLvDetManto = new ClsTypLvDetalleManto();
                    ClsLvDetManto.Alimentar(ObjLvDetManto, ObjMROReporteOTPpal);
                    string Mensj = ClsLvDetManto.GetMensj();
                    if (!Mensj.Equals("OK"))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtnRpt(true, true, true, true, true);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["BtnAccion"] = "";
                    ActivarCampRte(false, false, "Ingresar");
                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(ClsLvDetManto.GetCodIdRte());
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNroRte.Text.Equals("0"))
                { return; }
                if (ViewState["BtnAccion"].ToString() == "")
                {
                    ViewState["BtnAccion"] = "Editar";
                    string VblLicGenAnt, VbLicCumpAnt, VbLicVerif, VbOtAnt, VblTipRte, VblCat;
                    VblLicGenAnt = DdlLicGene.Text;
                    VbLicCumpAnt = DdlLicCump.Text;
                    VbOtAnt = DdlOtRte.Text;
                    VblTipRte = DdlTipRte.Text;
                    VblCat = DdlCatgr.Text;
                    VbLicVerif = DdlLicVer.Text;
                    string VbCodTall = DdlTall.Text;
                    string VbCodClasf = DdlClasf.Text;
                    string VbCodPos = DdlPosRte.Text;
                    string UsuGen = DdlGenerado.Text;
                    string UsuCump = DdlCumpl.Text;
                    string UsuDif = DdlTecDif.Text;
                    string UsuVer = DdlVerif.Text;
                    BindDdlRteCondicional(1, 1, DdlCatgr.Text, VblLicGenAnt, VbLicCumpAnt, VbLicVerif, VbCodTall, VbCodClasf, VbCodPos, UsuGen, UsuCump, UsuDif, UsuVer);
                    DdlLicGene.Text = VblLicGenAnt;
                    DdlLicCump.Text = VbLicCumpAnt;
                    DdlOtRte.Text = VbOtAnt;
                    DdlTipRte.Text = VblTipRte;
                    DdlCatgr.Text = VblCat;
                    DdlLicVer.Text = VbLicVerif;
                    DdlTall.Text = VbCodTall;
                    DdlClasf.Text = VbCodClasf;
                    DdlPosRte.Text = VbCodPos;
                    DdlGenerado.Text = UsuGen;
                    DdlCumpl.Text = UsuCump;
                    DdlTecDif.Text = UsuDif;
                    DdlVerif.Text = UsuVer;
                    ActivarBtnRpt(false, true, false, false, false);
                    DataRow[] Result1 = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result1)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampRte(true, true, "UPDATE");
                    DdlBusqRte.SelectedValue = "0";
                    DdlBusqRte.Enabled = false;
                    CldFecCump.StartDate = Convert.ToDateTime(TxtFecDet.Text);
                    CldFecPry.StartDate = Convert.ToDateTime(TxtFecDet.Text);
                    Result1 = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result1)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la edición?     
                }
                else
                {
                    ValidarRpte("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    DateTime? FecCump;
                    if (TxtFecCump.Text.Equals(""))
                    { FecCump = null; }
                    else
                    { FecCump = Convert.ToDateTime(TxtFecCump.Text); }
                    List<ClsTypLvDetalleManto> ObjLvDetManto = new List<ClsTypLvDetalleManto>();
                    var TypLvDetManto = new ClsTypLvDetalleManto()
                    {
                        FechaProyectada = Convert.ToDateTime(TxtFecPry.Text.Trim()),
                        FechaCumplimiento = FecCump,
                        FechaReporte = Convert.ToDateTime(TxtFecDet.Text),
                        FechaInicio = null,
                        FechaVerificacion = null,
                        CodIdLvDetManto = Convert.ToInt32(TxtNroRte.Text),
                        CodLibroVuelo = LblNumLVTit.Text.Trim(),
                        CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                        NumCasilla = TxtCas.Text.Trim(),
                        Reporte = TxtDescRte.Text.Trim(),
                        AccionCorrectiva = txtAccCrr.Text.Trim(),
                        CodTecnico = DdlCumpl.SelectedValue,
                        CodClaseReporteManto = DdlFuente.Text.Trim(),
                        CodClasifReporteManto = DdlClasf.Text.Trim(),
                        CodCategoriaMel = DdlCatgr.Text.Trim(),
                        CodStatus = DdlEstad.Text.Trim(),
                        DocumentoRef = TxtDocRef.Text.Trim(),
                        UbicacionTecnica = DdlAtaRte.Text.Trim(),
                        BanderaOrdenTrabajo = Convert.ToInt32(DdlTipRte.Text),
                        NroVuelo = "",
                        CodBase = DdlBasRte.Text.Trim(),
                        Usu = Session["C77U"].ToString().Trim(),
                        ConsInterno = "",
                        Posicion = DdlPosRte.Text.Trim(),
                        Programado = RdbPgSi.Checked == true ? 1 : 2,
                        FallaConfirmada = RdbFlCSi.Checked == true ? 1 : 2,
                        ReportadoPor = DdlGenerado.Text.Trim(),
                        AccionParcial = TxtAcciParc.Text.Trim(),
                        CodOt = Convert.ToInt32(DdlOtRte.Text),
                        CodUsuarioDiferido = DdlTecDif.Text.Trim(),
                        VerificadoRM = DdlLicVer.Text.Trim().Equals("") ? 0 : 1,
                        CodInspectorVerifica = DdlVerif.Text.Trim(),
                        NumLicenciaRM = DdlLicVer.Text.Trim(),
                        TearDown = CkbTearDown.Checked == true ? 1 : 0,
                        RII = CkbRII.Checked == true ? 1 : 0,
                        Notificado = CkbNotif.Checked == true ? 1 : 0,
                        NumLicTecCierre = DdlLicCump.Text.Trim(),
                        TT_A_C = Convert.ToDouble(TxtTtlAKSN.Text),
                        HraProxCump = Convert.ToDouble(TxtHPrxCu.Text),
                        Next_Due = Convert.ToDouble(TxtNexDue.Text),
                        NumLicTecAbre = DdlLicGene.Text.Trim(),
                        IdPosicionTT = null,
                        Accion = "UPDATE",
                    };
                    ObjLvDetManto.Add(TypLvDetManto);
                    int borrar = (int)ViewState["IDMroRepOT"];
                    List<ClsTypLvDetalleManto> ObjMROReporteOTPpal = new List<ClsTypLvDetalleManto>();
                    var TypMROReporteOTPpal = new ClsTypLvDetalleManto()
                    {
                        IDMroRepOT = (int)ViewState["IDMroRepOT"],
                        PasoOT = ViewState["PasoOT"].ToString().Trim(),
                        NumReporte = Convert.ToInt32(TxtNroRte.Text),
                        CodTaller = DdlTall.Text.Trim(),
                        ParteNumero = DdlPnRte.Text.Trim(),
                        SerieNumero = TxtSnRte.Text.Trim(),
                        ConsecutivoROTP = TxtConsTall.Text.Trim(),
                        SubOT = Convert.ToInt32(TxtOtSec.Text),
                    };
                    ObjMROReporteOTPpal.Add(TypMROReporteOTPpal);

                    ClsTypLvDetalleManto ClsLvDetManto = new ClsTypLvDetalleManto();
                    ClsLvDetManto.Alimentar(ObjLvDetManto, ObjMROReporteOTPpal);
                    string Mensj = ClsLvDetManto.GetMensj();
                    if (!Mensj.Equals("OK"))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    int CodIdRte = ClsLvDetManto.GetCodIdRte();
                    ActivarBtnRpt(true, true, true, true, true);
                    ViewState["BtnAccion"] = "";
                    DataRow[] Result3 = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result3)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampRte(false, false, "UPDATE");
                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text));
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result4 = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result4)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } 
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_PANTALLA_Reporte_Manto 12,@Usu,'','','',@Rte,@HK,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtNroRte.Text));
                            SC.Parameters.AddWithValue("@HK", Convert.ToInt32(DdlAeroRte.Text));
                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals("S"))
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "');", true);
                                return;
                            }
                            Transac.Commit();
                            LimpiarCamposRte();
                            BindBDdlBusqRte();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Reporte Manto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void DdlAeroRte_TextChanged(object sender, EventArgs e)
        {

        }
        protected void BtnExporRte_Click(object sender, EventArgs e)
        {
            Exportar("ReporteGeneral");
        }
        protected void BtnNotificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            if (DdlEstad.Text.Equals("A"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens27'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//El reporte debe estar cerrado.')", true);
                return;
            }
            if ((int)ViewState["TtlRegDet"] > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens28'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//No es posible notificar un reporte con recurso físico.')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 3,@Usu,'','','','','','','','','','','','','','',@Rte,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", TxtNroRte.Text);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            CkbNotif.Checked = true;
                            BtnNotificar.Enabled = false;
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Notificar Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        //******************************************  Opciones de busqueda *********************************************************
        protected void IbtFind_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["ViewOrigen"] = "LVLO";

            DataRow[] Result = Idioma.Select("Objeto= 'LblTitOpcBusqueda'");
            foreach (DataRow row in Result)
            { LblTitOpcBusqueda.Text = row["Texto"].ToString().Trim(); }
            TblBusqLVlo.Visible = true;
            IbtExpConsulRte.Visible = false;
            GrdBusq.DataSource = null;
            GrdBusq.DataBind();
            MultVieLV.ActiveViewIndex = 3;
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result = Idioma.Select("Objeto= 'LblOpbusRTE'");
            foreach (DataRow row in Result)
            { LblTitOpcBusqueda.Text = row["Texto"].ToString(); }
           // LblTitOpcBusqueda.Text = "Opciones de búsqueda reporte de mantenimiento";
            ViewState["ViewOrigen"] = "RTE";
            TblBusqRte.Visible = true;
            IbtExpConsulRte.Visible = true;
            GrdBusq.DataSource = null;
            GrdBusq.DataBind();
            MultVieLV.ActiveViewIndex = 3;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtConsultarBusq_Click(object sender, ImageClickEventArgs e)
        {
            BIndDataBusq();
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtExpConsulRte_Click(object sender, ImageClickEventArgs e)
        {
            Exportar("Reporte");
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        {
            TblBusqRte.Visible = false;
            TblBusqLVlo.Visible = false;
            if (ViewState["ViewOrigen"].ToString().Equals("RTE"))
            { MultVieLV.ActiveViewIndex = 2; }
            else { MultVieLV.ActiveViewIndex = 0; }
        }
        protected void BIndDataBusq()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                string VbTxtSql = "", VbOpcion = "";

                if (TblBusqRte.Visible == true)
                {
                    //busqueda Reporte
                    CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                    if (RdbBusqRteNum.Checked == true)
                    { VbOpcion = "RteNum"; }
                    if (RdbBusqRteHk.Checked == true)
                    { VbOpcion = "HK"; }
                    if (RdbBusqRteAta.Checked == true)
                    { VbOpcion = "Ata"; }
                    if (RdbBusqRteOT.Checked == true)
                    { VbOpcion = "OT"; }
                    if (RdbBusqRteTecn.Checked == true)
                    { VbOpcion = "Tecn"; }
                    if (RdbBusqRteDescRte.Checked == true)
                    { VbOpcion = "DescRte"; }

                    VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,@CodlV,'','CurBusqRte',@Opc,0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                }
                else
                {                    
                    //Busqueda Libro de vuelo
                    if (RdbBusqLVloNum.Checked == true)
                    { VbOpcion = "NumLV"; }
                    if (RdbBusqLVloFech.Checked == true)
                    { VbOpcion = "Fech"; }
                    if (RdbBusqLVloHK.Checked == true)
                    { VbOpcion = "HK"; }
                    if (RdbBusqLVloNroRte.Checked == true)
                    { VbOpcion = "RteNro"; }
                    CursorIdioma.Alimentar("CurBusqLV", Session["77IDM"].ToString().Trim());
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 23,@Prmtr,'','CurBusqLV',@Opc,0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                }
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodlV", LblNumLVTit.Text.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

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
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text);
            if (ViewState["ViewOrigen"].Equals("RTE"))
            {
                TraerDatosRtes(Convert.ToInt32(vbcod));
                MultVieLV.ActiveViewIndex = 2;
            }
            else
            {
                Traerdatos(vbcod.Trim());
                DdlBusq.SelectedValue = "";
                MultVieLV.ActiveViewIndex = 0;
            }
            PerfilesGrid();
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusq.PageIndex = e.NewPageIndex;
            BIndDataBusq();
        }
        //******************************************  Recurso y Licencia Reporte de mantenimiento *********************************************************
        protected void BtnReserva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (!TxtNroRte.Text.Equals("0"))
            {
                TxtRecurNumRte.Text = TxtNroRte.Text;
                TxtRecurSubOt.Text = TxtOtSec.Text;
                DdlPrioridadOT.Text = ViewState["CodPrioridad"].ToString().Trim();
                if (DdlEstad.Text.Equals("C") || (int)ViewState["BloquearDetalle"] == 1)
                { DdlPrioridadOT.Enabled = false; BtnCargaMaxiva.Enabled = false; }
                else
                {
                    BtnCargaMaxiva.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                    foreach (DataRow row in Result)
                    { BtnCargaMaxiva.ToolTip = row["Texto"].ToString() + " " + ViewState["CarpetaCargaMasiva"].ToString() + "CargaMasiva.xlsx"; }
                }
                BindDRecursoF();
                BindDLicencia();
                PerfilesGrid();
                MultVieLV.ActiveViewIndex = 4;
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BindDRecursoF()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 4,@PN,'','','','',@SubOT,0,0,0,'01-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                    {
                        SC.Parameters.AddWithValue("@PN", TxtConsulPnRecurRte.Text.Trim());
                        SC.Parameters.AddWithValue("@SubOT", TxtRecurSubOt.Text.Trim());
                        SCX2.Open();
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
                            if (DT.Rows.Count > 0)
                            {
                                GrdRecursoF.DataSource = DT;
                                GrdRecursoF.DataBind();
                                ViewState["TtlRegDet"] = DT.Rows.Count;
                            }
                            else
                            {
                                ViewState["TtlRegDet"] = 0;
                                DT.Rows.Add(DT.NewRow());
                                GrdRecursoF.DataSource = DT;
                                GrdRecursoF.DataBind();
                                GrdRecursoF.Rows[0].Cells.Clear();
                                GrdRecursoF.Rows[0].Cells.Add(new TableCell());
                                // GrdRecursoF.Rows[0].Cells[0].ColumnSpan = DT.Columns.Count;
                                DataRow[] Result = Idioma.Select("Objeto= 'RteMens41'");
                                foreach (DataRow row in Result)
                                { GrdRecursoF.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                GrdRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtConsulPnRecurRte_Click(object sender, ImageClickEventArgs e)
        {
            BindDRecursoF();
        }
        protected void IbtExpExcelPnRecurRte_Click(object sender, ImageClickEventArgs e)
        {
            Exportar("Reserva");
        }
        protected void IbtCerrarRec_Click(object sender, ImageClickEventArgs e)
        {
            TxtOtSec.Text = TxtRecurSubOt.Text;
            ViewState["CodPrioridad"] = DdlPrioridadOT.Text.Trim();
            MultVieLV.ActiveViewIndex = 2;
        }
        protected void DdlPNRFPP_TextChanged(object sender, EventArgs e)
        {
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
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string VblString = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'{0}','','','','DescRef',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlPNRFPP.Text);
                SqlCommand SC = new SqlCommand(VblString, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtDesRFPP.Text = SDR["Descripcion"].ToString();
                }
            }
        }
        protected void GrdRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (DdlPrioridadOT.Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens11'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                        return;
                    }
                    string VblPN, VBQuery, VblTxtCant, VbDesc, VbIPC;
                    double VblCant;
                    if ((GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Visible == true)
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).SelectedValue.Trim(); }
                    else
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("TxtPNRFPP") as TextBox).Text.Trim(); }

                    CultureInfo Culture = new CultureInfo("en-US");
                    VblTxtCant = (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim();
                    VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    VbDesc = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox).Text.Trim();
                    VbIPC = (GrdRecursoF.FooterRow.FindControl("TxtIPCRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'','','INSERT',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");

                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@IdDetRsva", 0);
                                    SC.Parameters.AddWithValue("@PN", VblPN);
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@CodPri", DdlPrioridadOT.Text.Trim());
                                    SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                    SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                    SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                    SC.Parameters.AddWithValue("@SubOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                    SC.Parameters.AddWithValue("@Cant", VblCant);
                                    SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlAeroRte.Text));
                                    SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(TxtNroRte.Text));

                                    string Mensj = "OK";
                                    string VbEjecPlano = "N";
                                    int VblSubOt = Convert.ToInt32(TxtRecurSubOt.Text);
                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        VblSubOt = Convert.ToInt32(SDR["SubOT"].ToString().Trim());
                                        VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim());

                                    }
                                    SDR.Close();
                                    Transac.Commit();
                                    if (!Mensj.ToString().Trim().Equals("OK"))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        return;
                                    }
                                    TxtRecurSubOt.Text = VblSubOt.ToString();
                                    if (VbEjecPlano.Trim().Equals("S"))
                                    {
                                        Cnx.SelecBD();
                                        using (SqlConnection SCnxPln = new SqlConnection(Cnx.GetConex()))
                                        {
                                            sqlCon.Open();
                                            VBQuery = string.Format("EXEC SP_IntegradorNEW 6,'',@Usu,'','','',@CodOT,0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                                            using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                                            {
                                                try
                                                {
                                                    sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                                    sqlCmd.Parameters.AddWithValue("@CodOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                                    sqlCmd.ExecuteNonQuery();
                                                }
                                                catch (Exception ex)
                                                {
                                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Generar Nueva Reserva", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                                }
                                            }
                                        }
                                    }
                                    TxtConsulPnRecurRte.Text = "";
                                    BindDRecursoF();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdRecursoF.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex; // Guarda El indice para luego buscar en otro evento com en un TextChanged
            BindDRecursoF();
        }
        protected void GrdRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (DdlPrioridadOT.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una prioridad')", true);
                    return;
                }
                string VblPN, VBQuery, VblTxtCant, VbDesc, VbIPC;
                double VblCant;
                int Idx = (int)ViewState["Index"];
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[Idx].Value.ToString());

                VblPN = (GrdRecursoF.Rows[Idx].FindControl("TxtPNRF") as TextBox).Text.Trim();

                CultureInfo Culture = new CultureInfo("en-US");
                VblTxtCant = (GrdRecursoF.Rows[Idx].FindControl("TxtCantRF") as TextBox).Text.Trim().Equals("") ? "1" : (GrdRecursoF.Rows[Idx].FindControl("TxtCantRF") as TextBox).Text.Trim();
                VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                VbDesc = (GrdRecursoF.Rows[Idx].FindControl("TxtDesRF") as TextBox).Text.Trim();
                VbIPC = (GrdRecursoF.Rows[Idx].FindControl("TxtIPCRF") as TextBox).Text.Trim();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'','','UPDATE',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CodPri", DdlPrioridadOT.Text.Trim());
                                SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                SC.Parameters.AddWithValue("@SubOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlAeroRte.Text));
                                SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(TxtNroRte.Text));
                                string Mensj = "OK";
                                int VblSubOt = Convert.ToInt32(TxtRecurSubOt.Text);
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    VblSubOt = Convert.ToInt32(SDR["SubOT"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals("OK"))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                TxtRecurSubOt.Text = VblSubOt.ToString();
                                GrdRecursoF.EditIndex = -1;
                                BindDRecursoF();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Error en el ingreso')
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdRecursoF.EditIndex = -1;
            BindDRecursoF();
        }
        protected void GrdRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery;
                int Idx = e.RowIndex;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[Idx].Value.ToString());
                string VblPN = (GrdRecursoF.Rows[Idx].FindControl("LblPn") as Label).Text.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                string VblTxtCant = (GrdRecursoF.Rows[Idx].FindControl("LblCantRF") as Label).Text.Trim();
                double VblCant = Convert.ToDouble(VblTxtCant, Culture);
                int VbPosc = Convert.ToInt32((GrdRecursoF.Rows[Idx].FindControl("LblPosc") as Label).Text.Trim());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,'','','','','','','DELETE',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,@Posc,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@SubOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlAeroRte.Text));
                                SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(TxtNroRte.Text));
                                SC.Parameters.AddWithValue("@Posc", VbPosc);

                                string Mensj = "OK";
                                int VblSubOt = Convert.ToInt32(TxtRecurSubOt.Text);
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    VblSubOt = Convert.ToInt32(SDR["SubOT"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals("OK"))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                TxtConsulPnRecurRte.Text = "";
                                BindDRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNRFPP.DataTextField = "PN";
                DdlPNRFPP.DataValueField = "CodPN";
                DdlPNRFPP.DataBind();
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlEstad.Text.Equals("C") || (int)ViewState["BloquearDetalle"] == 1)
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
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
                if (DdlEstad.Text.Equals("C") || (int)ViewState["BloquearDetalle"] == 1)
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { imgE.ToolTip = row["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { imgD.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = true;
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
        protected void GrdRecursoF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdRecursoF.PageIndex = e.NewPageIndex;
            BindDRecursoF();
            PerfilesGrid();
        }
        protected void BindDLicencia()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 5,'','','','','',@NumRTE,0,0,0,'01-01-1','02-01-1','03-01-1'");

                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@NumRTE", TxtRecurNumRte.Text);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
                            if (DT.Rows.Count > 0)
                            {
                                GrdLicen.DataSource = DT;
                                GrdLicen.DataBind();
                            }
                            else
                            {
                                DT.Rows.Add(DT.NewRow());
                                GrdLicen.DataSource = DT;
                                GrdLicen.DataBind();
                                GrdLicen.Rows[0].Cells.Clear();
                                GrdLicen.Rows[0].Cells.Add(new TableCell());
                                DataRow[] Result = Idioma.Select("Objeto= 'RteMens40'");
                                foreach (DataRow row in Result)
                                { GrdLicen.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                GrdLicen.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDRecursoF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlLicenRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            TextBox TxtDesLiRFPP = (GrdLicen.FooterRow.FindControl("TxtDesLiRFPP") as TextBox);
            DropDownList DdlLicenRFPP = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList);
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string VblString = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'','','','','DescLicenRF',@CodLic,0,0,0,'01-01-01','01-01-01','01-01-01'");
                SqlCommand SC = new SqlCommand(VblString, Cnx2);
                SC.Parameters.AddWithValue("@CodLic", DdlLicenRFPP.SelectedValue);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtDesLiRFPP.Text = SDR["Descripcion"].ToString();
                }
            }
        }
        protected void GrdLicen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VBQuery, VblTxtTE, VbCodIdLicencia;
                    double VblTE;
                    if ((GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue.Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens09'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //'Debe ingresar una licencia
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','INSERT',0,@CodIdLic,@TiempEst,0,@NumRte,0,'01-01-1','02-01-1','03-01-1'");
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                                    SC.Parameters.AddWithValue("@TiempEst", VblTE);
                                    SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                                    var Mensj = SC.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDLicencia();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Licencia REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdLicen.EditIndex = e.NewEditIndex;
            BindDLicencia();
        }
        protected void GrdLicen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','UPDATE',0,@CodIdLic,@TiempEst,@IdSvcLic,@NumRte,0,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                                SC.Parameters.AddWithValue("@TiempEst", VblTE);
                                SC.Parameters.AddWithValue("@IdSvcLic", IdSrvLic);
                                SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                GrdLicen.EditIndex = -1;
                                BindDLicencia();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdLicen_RowUpdating Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdLicen.EditIndex = -1;
            BindDLicencia();
        }
        protected void GrdLicen_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string VblTE = "";
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
                    string VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','DELETE',0,@CodIdLic,@TiempEst,@IdSvcLic,@NumRte,0,'01-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                            SC.Parameters.AddWithValue("@TiempEst", Convert.ToDouble(VblTE));
                            SC.Parameters.AddWithValue("@IdSvcLic", IdSrvLic);
                            SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDLicencia();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','LICRF',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlLicenRFPP.DataTextField = "CodLicencia";
                DdlLicenRFPP.DataValueField = "CodIdLicencia";
                DdlLicenRFPP.DataBind();
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlEstad.Text.Equals("C") || (int)ViewState["BloquearDetalle"] == 1)
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
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
                if (DdlEstad.Text.Equals("C") || (int)ViewState["BloquearDetalle"] == 1)
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { imgE.ToolTip = row["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                        foreach (DataRow row in Result)
                        { imgD.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = true;
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
        protected void GrdLicen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdLicen.PageIndex = e.NewPageIndex;
            BindDLicencia();
            PerfilesGrid();
        }
        //******************************************  Subir Recurso maxivamente *********************************************************
        protected void BtnCargaMaxiva_Click(object sender, EventArgs e)
        {
            if ((int)ViewState["TtlRegDet"] > 0)
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }//Para realizar la carga masiva la reserva debe estar vacía')", true);
                return;
            }
            TxtCargaMasiRte.Text = TxtRecurNumRte.Text;
            TxtCargaMasiOT.Text = TxtRecurSubOt.Text;
            IbtGuardarCargaMax.Enabled = false;
            Page.Title = ViewState["PageTit"].ToString().Trim();
            MultVieLV.ActiveViewIndex = 5;
        }
        protected void IbtSubirCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtCargaMasiOT.Text.Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens42'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCargaMasiva, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }//Debe estar generado el número de la reserva
                    return;
                }
                DataTable DT = new DataTable();
                string FileName = "";
                string conexion = "";
                //string conexion1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Asus Pro\Downloads\Reportes.xlsx;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                FileName = "CargaMasiva.xlsx";
                conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ViewState["CarpetaCargaMasiva"].ToString().Trim() + FileName + ";;Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                using (OleDbConnection cnn = new OleDbConnection(conexion))
                {
                    cnn.Open();
                    DataTable dtExcelSchema;
                    dtExcelSchema = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    cnn.Close();

                    cnn.Open();
                    string sql = "SELECT * From [" + SheetName + "]";

                    OleDbCommand command = new OleDbCommand(sql, cnn);
                    OleDbDataAdapter DA = new OleDbDataAdapter(command);

                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        GrdCargaMax.DataSource = DT;
                        GrdCargaMax.DataBind();
                        Session["TablaRsvaResul"] = DT;
                    }
                    cnn.Close();

                    List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
                    foreach (GridViewRow Row in GrdCargaMax.Rows)
                    {
                        TextBox TxtPNRF = Row.FindControl("TxtPNRF") as TextBox;
                        TextBox TxtDesRF = Row.FindControl("TxtDesRF") as TextBox;
                        TextBox TxtCantRF = Row.FindControl("TxtCantRF") as TextBox;
                        TextBox TxtUMRF = Row.FindControl("TxtUMRF") as TextBox;
                        TextBox TxtUMSysRF = Row.FindControl("TxtUMSysRF") as TextBox;
                        TextBox TxtIPCRF = Row.FindControl("TxtIPCRF") as TextBox;
                        string VbTxtCant = TxtCantRF.Text.Trim().Equals("") ? "0" : TxtCantRF.Text.Trim();
                        double VblCant = TxtCantRF.Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCant);

                        var TypSubirRsva = new CsTypSubirReserva()
                        {
                            IdRsva = Convert.ToInt32(TxtCargaMasiOT.Text),
                            Posicion = 0,
                            PN = TxtPNRF.Text.Trim(),
                            Descripcion = TxtDesRF.Text.Trim(),
                            Cantidad = VblCant,
                            UndSolicitada = TxtUMRF.Text.Trim(),
                            UndSistema = TxtUMSysRF.Text.Trim(),
                            IPC = TxtIPCRF.Text.Trim(),
                            Usu = Session["C77U"].ToString(),
                            CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                            Accion = "TEMPORAL",
                        };
                        ObjSubirRsva.Add(TypSubirRsva);
                    }
                    CsTypSubirReserva SubirRsva = new CsTypSubirReserva();

                    SubirRsva.Alimentar(ObjSubirRsva);// 
                    string Mensj = SubirRsva.GetMensj();
                    if (!Mensj.Trim().Equals("OK"))
                    {
                        GrdCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                        GrdCargaMax.DataBind();
                        IbtGuardarCargaMax.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCargaMasiva, UpPnlCargaMasiva.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    GrdCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                    GrdCargaMax.DataBind();
                    IbtGuardarCargaMax.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtGuardarCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
            foreach (GridViewRow Row in GrdCargaMax.Rows)
            {
                TextBox TxtPNRF = Row.FindControl("TxtPNRF") as TextBox;
                TextBox TxtDesRF = Row.FindControl("TxtDesRF") as TextBox;
                TextBox TxtCantRF = Row.FindControl("TxtCantRF") as TextBox;
                TextBox TxtUMRF = Row.FindControl("TxtUMRF") as TextBox;
                TextBox TxtUMSysRF = Row.FindControl("TxtUMSysRF") as TextBox;
                TextBox TxtIPCRF = Row.FindControl("TxtIPCRF") as TextBox;
                string VbTxtCant = TxtCantRF.Text.Trim().Equals("") ? "0" : TxtCantRF.Text.Trim();
                double VblCant = TxtCantRF.Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCant);

                var TypSubirRsva = new CsTypSubirReserva()
                {
                    IdRsva = Convert.ToInt32(TxtCargaMasiOT.Text),
                    Posicion = 0,
                    PN = TxtPNRF.Text.Trim(),
                    Descripcion = TxtDesRF.Text.Trim(),
                    Cantidad = VblCant,
                    UndSolicitada = TxtUMRF.Text.Trim(),
                    UndSistema = TxtUMSysRF.Text.Trim(),
                    IPC = TxtIPCRF.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                    Accion = "INSERT",
                };
                ObjSubirRsva.Add(TypSubirRsva);
            }
            CsTypSubirReserva SubirRsva = new CsTypSubirReserva();

            SubirRsva.Alimentar(ObjSubirRsva);// 
            string Mensj = SubirRsva.GetMensj();
            if (!Mensj.Trim().Equals("OK"))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString(); }
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCargaMasiva, UpPnlCargaMasiva.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                IbtGuardarCargaMax.Enabled = false;
                return;
            }
            IbtGuardarCargaMax.Enabled = false;
            BindDRecursoF();
            MultVieLV.ActiveViewIndex = 4;
        }
        protected void IbtCerrarSubMaxivo_Click(object sender, ImageClickEventArgs e)
        {
            MultVieLV.ActiveViewIndex = 4;
        }
        //******************************************  Impresion Reporte *********************************************************
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            MultVieLV.ActiveViewIndex = 6;
            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[3];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

                string StSql = " EXEC SP_PANTALLA_Reporte_Manto2 8,'','','','','',@RteNum,0,0,0,'01-01-1','02-01-1','03-01-1'";
                using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                {
                    SC.Parameters.AddWithValue("@RteNum", TxtNroRte.Text);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(ds);
                        RvwReporte.LocalReport.EnableExternalImages = true;
                        RvwReporte.LocalReport.ReportPath = "Report/Ing/ReporteV2.rdlc";
                        RvwReporte.LocalReport.DataSources.Clear();
                        RvwReporte.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                        RvwReporte.LocalReport.SetParameters(parameters);
                        RvwReporte.LocalReport.Refresh();
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtCerrarImpresion_Click(object sender, ImageClickEventArgs e)
        {
            MultVieLV.ActiveViewIndex = 2;
        }
        //******************************************  SN On Off  *********************************************************
        protected void BtnSnOnOf_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            TxtSnOnOffNumRte.Text = TxtNroRte.Text;
            BindDSnOnOff();
            BindDHta();
            PerfilesGrid();
            MultVieLV.ActiveViewIndex = 7;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BindDSnOnOff()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 9,'','','','','',@NR,0,0,0,'01-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                    {
                        SC.Parameters.AddWithValue("@NR", TxtSnOnOffNumRte.Text.Trim());
                        SCX2.Open();
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
                            if (DT.Rows.Count > 0)
                            {
                                GrdSnOnOff.DataSource = DT;
                                GrdSnOnOff.DataBind();
                            }
                            else
                            {
                                DT.Rows.Add(DT.NewRow());
                                GrdSnOnOff.DataSource = DT;
                                GrdSnOnOff.DataBind();
                                GrdSnOnOff.Rows[0].Cells.Clear();
                                GrdSnOnOff.Rows[0].Cells.Add(new TableCell());
                                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                                foreach (DataRow row in Result)
                                { GrdSnOnOff.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                GrdSnOnOff.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarSnOnOff_Click(object sender, ImageClickEventArgs e)
        {
            MultVieLV.ActiveViewIndex = 2;
        }
        protected void DdlPNOn_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOn") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescElem = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtDescElem") as TextBox);
            TextBox TxtSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOn") as TextBox);
            string VbPn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("DdlPNOn") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOn.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElem.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOn.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNOn.Visible = true; }
                }
            }
            TxtSNOn.Text = "";
            PerfilesGrid();
        }
        protected void LtbSNOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOn") as TextBox);
            ListBox LtbSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOn") as ListBox);
            string VbSn = LtbSNOn.SelectedValue.Trim();
            TxtSNOn.Text = VbSn;
            LtbSNOn.Visible = false;
            PerfilesGrid();
        }
        protected void DdlPNOff_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOff = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOff") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtSNOff = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOff") as TextBox);
            TextBox TxtDescElem = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtDescElem") as TextBox);
            string VbPn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("DdlPNOff") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOff.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElem.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOff.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNOff.Visible = true; }
                }
            }
            TxtSNOff.Text = "";
            PerfilesGrid();
        }
        protected void LtbSNOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNOff = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOff") as TextBox);
            ListBox LtbSNOff = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOff") as ListBox);
            string VbSn = LtbSNOff.SelectedValue.Trim();
            TxtSNOff.Text = VbSn;
            LtbSNOff.Visible = false;
            PerfilesGrid();
        }
        protected void DdlPNOnPP_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOnPP = (GrdSnOnOff.FooterRow.FindControl("LtbSNOnPP") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescElemPP = (GrdSnOnOff.FooterRow.FindControl("TxtDescElemPP") as TextBox);
            TextBox TxtSNOnPP = (GrdSnOnOff.FooterRow.FindControl("TxtSNOnPP") as TextBox);
            string VbPn = (GrdSnOnOff.FooterRow.FindControl("DdlPNOnPP") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOnPP.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElemPP.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOnPP.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNOnPP.Visible = true; }
                }
            }
            TxtSNOnPP.Text = "";
            PerfilesGrid();
        }
        protected void LtbSNOnPP_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNOnPP = (GrdSnOnOff.FooterRow.FindControl("TxtSNOnPP") as TextBox);
            ListBox LtbSNOnPP = (GrdSnOnOff.FooterRow.FindControl("LtbSNOnPP") as ListBox);
            string VbSn = LtbSNOnPP.SelectedValue.Trim();
            TxtSNOnPP.Text = VbSn;
            LtbSNOnPP.Visible = false;
            PerfilesGrid();
        }
        protected void DdlPNOffPP_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOffPP = (GrdSnOnOff.FooterRow.FindControl("LtbSNOffPP") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescElemPP = (GrdSnOnOff.FooterRow.FindControl("TxtDescElemPP") as TextBox);
            TextBox TxtSNOffPP = (GrdSnOnOff.FooterRow.FindControl("TxtSNOffPP") as TextBox);
            string VbPn = (GrdSnOnOff.FooterRow.FindControl("DdlPNOffPP") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOffPP.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElemPP.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOffPP.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNOffPP.Visible = true; }
                }
            }
            TxtSNOffPP.Text = "";
            PerfilesGrid();
        }
        protected void LtbSNOffPP_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNOffPP = (GrdSnOnOff.FooterRow.FindControl("TxtSNOffPP") as TextBox);
            ListBox LtbSNOffPP = (GrdSnOnOff.FooterRow.FindControl("LtbSNOffPP") as ListBox);
            string VbSn = LtbSNOffPP.SelectedValue.Trim();
            TxtSNOffPP.Text = VbSn;
            LtbSNOffPP.Visible = false;
            PerfilesGrid();
        }
        protected void GrdSnOnOff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
                if ((GrdSnOnOff.FooterRow.FindControl("TxtFecPP") as TextBox).Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha')", true);
                    return;
                }
                DateTime? VbFe = Convert.ToDateTime((GrdSnOnOff.FooterRow.FindControl("TxtFecPP") as TextBox).Text);
                string VbRazR = (GrdSnOnOff.FooterRow.FindControl("DdlRazonRPP") as DropDownList).Text.Trim();
                string VbPos = (GrdSnOnOff.FooterRow.FindControl("DdlPosicPP") as DropDownList).Text.Trim();
                string VbPnOn = (GrdSnOnOff.FooterRow.FindControl("DdlPNOnPP") as DropDownList).Text.Trim();
                string VbSnOn = (GrdSnOnOff.FooterRow.FindControl("TxtSNOnPP") as TextBox).Text.Trim();
                string VbDes = (GrdSnOnOff.FooterRow.FindControl("TxtDescElemPP") as TextBox).Text.Trim();
                string VbPnOff = (GrdSnOnOff.FooterRow.FindControl("DdlPNOffPP") as DropDownList).Text.Trim();
                string VbSnOff = (GrdSnOnOff.FooterRow.FindControl("TxtSNOffPP") as TextBox).Text.Trim();
                int VbCant = Convert.ToInt32((GrdSnOnOff.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim());

                if (!VbSnOn.Equals("") && VbSnOn.Equals(VbSnOff))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Las series son iguales')", true);
                    return;
                }
                if (VbPnOn.Equals("") && VbPnOff.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens30'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar un P/N ON o OFF')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,@TRazR,@Pos,@PnOn,@SnOn,@Des,@PnOff,@SnOff,'','','','','','','INSERT',@CodT,@Rte,@Cant,0,0,0,@Fe,'02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@CodT", 0);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Fe", VbFe);
                                SC.Parameters.AddWithValue("@TRazR", VbRazR);
                                SC.Parameters.AddWithValue("@Pos", VbPos);
                                SC.Parameters.AddWithValue("@PnOn", VbPnOn);
                                SC.Parameters.AddWithValue("@SnOn", VbSnOn);
                                SC.Parameters.AddWithValue("@Des", VbDes);
                                SC.Parameters.AddWithValue("@PnOff", VbPnOff);
                                SC.Parameters.AddWithValue("@SnOff", VbSnOff);
                                SC.Parameters.AddWithValue("@Cant", VbCant);
                                SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BindDSnOnOff();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdSnOnOff.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            BindDSnOnOff();
        }
        protected void GrdSnOnOff_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdSnOnOff.DataKeys[Idx].Value.ToString());
            int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
            if ((GrdSnOnOff.Rows[Idx].FindControl("TxtFec") as TextBox).Text.Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha')", true);
                return;
            }
            DateTime? VbFe = Convert.ToDateTime((GrdSnOnOff.Rows[Idx].FindControl("TxtFec") as TextBox).Text);
            string VbRazR = (GrdSnOnOff.Rows[Idx].FindControl("DdlRazonR") as DropDownList).Text.Trim();
            string VbPos = (GrdSnOnOff.Rows[Idx].FindControl("DdlPosic") as DropDownList).Text.Trim();
            string VbPnOn = (GrdSnOnOff.Rows[Idx].FindControl("DdlPNOn") as DropDownList).Text.Trim();
            string VbSnOn = (GrdSnOnOff.Rows[Idx].FindControl("TxtSNOn") as TextBox).Text.Trim();
            string VbDes = (GrdSnOnOff.Rows[Idx].FindControl("TxtDescElem") as TextBox).Text.Trim();
            string VbPnOff = (GrdSnOnOff.Rows[Idx].FindControl("DdlPNOff") as DropDownList).Text.Trim();
            string VbSnOff = (GrdSnOnOff.Rows[Idx].FindControl("TxtSNOff") as TextBox).Text.Trim();
            int VbCant = Convert.ToInt32((GrdSnOnOff.Rows[Idx].FindControl("TxtCant") as TextBox).Text.Trim());

            if (!VbSnOn.Equals("") && VbSnOn.Equals(VbSnOff))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens29'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Las series son iguales')", true);
                return;
            }
            if (VbPnOn.Equals("") && VbPnOff.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens30'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar un P/N ON o OFF')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,@TRazR,@Pos,@PnOn,@SnOn,@Des,@PnOff,@SnOff,'','','','','','','UPDATE',@CodT,@Rte,@Cant,0,0,0,@Fe,'02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Fe", VbFe);
                            SC.Parameters.AddWithValue("@TRazR", VbRazR);
                            SC.Parameters.AddWithValue("@Pos", VbPos);
                            SC.Parameters.AddWithValue("@PnOn", VbPnOn);
                            SC.Parameters.AddWithValue("@SnOn", VbSnOn);
                            SC.Parameters.AddWithValue("@Des", VbDes);
                            SC.Parameters.AddWithValue("@PnOff", VbPnOff);
                            SC.Parameters.AddWithValue("@SnOff", VbSnOff);
                            SC.Parameters.AddWithValue("@Cant", VbCant);
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdSnOnOff.EditIndex = -1;
                            BindDSnOnOff();
                            PerfilesGrid();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdSnOnOff.EditIndex = -1;
            BindDSnOnOff();
        }
        protected void GrdSnOnOff_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VBQuery;
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdSnOnOff.DataKeys[Idx].Value.ToString());
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,'','','','','','','','','','','','','','DELETE',@CodT,@Rte,0,0,0,0,'02-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDSnOnOff();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string LtxtSql = "";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlRazonRPP = (e.Row.FindControl("DdlRazonRPP") as DropDownList);
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','RAZ',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                DdlRazonRPP.DataSource = Cnx.DSET(LtxtSql);
                DdlRazonRPP.DataTextField = "Descripcion";
                DdlRazonRPP.DataValueField = "CodRemocion";
                DdlRazonRPP.DataBind();

                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','PosR',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                DropDownList DdlPosicPP = (e.Row.FindControl("DdlPosicPP") as DropDownList);
                DdlPosicPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPosicPP.DataTextField = "Descripcion";
                DdlPosicPP.DataValueField = "Codigo";
                DdlPosicPP.DataBind();

                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','PNRTE',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                DropDownList DdlPNOnPP = (e.Row.FindControl("DdlPNOnPP") as DropDownList);
                DdlPNOnPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNOnPP.DataTextField = "PN";
                DdlPNOnPP.DataValueField = "Codigo";
                DdlPNOnPP.DataBind();

                DropDownList DdlPNOffPP = (e.Row.FindControl("DdlPNOffPP") as DropDownList);
                DdlPNOffPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNOffPP.DataTextField = "PN";
                DdlPNOffPP.DataValueField = "Codigo";
                DdlPNOffPP.DataBind();

                TextBox TxtFecPP = (e.Row.FindControl("TxtFecPP") as TextBox);
                TxtFecPP.Text = TxtFecDet.Text;
                CalendarExtender CalFechPP = (e.Row.FindControl("CalFechPP") as CalendarExtender);
                DateTime DiaI = Convert.ToDateTime(TxtFecha.Text);
                CalFechPP.StartDate = Convert.ToDateTime(TxtFecPP.Text);
                CalFechPP.EndDate = DateTime.Now;

                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlEstad.Text.Equals("C"))
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                DropDownList DdlRazonR = (e.Row.FindControl("DdlRazonR") as DropDownList);
                string borrar = dr["CodRazonR"].ToString().Trim();
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','RAZ',0,0,0,0,'01-01-1','02-01-1','03-01-1'", dr["CodRazonR"].ToString().Trim());
                DdlRazonR.DataSource = Cnx.DSET(LtxtSql);
                DdlRazonR.DataTextField = "Descripcion";
                DdlRazonR.DataValueField = "CodRemocion";
                DdlRazonR.DataBind();
                DdlRazonR.SelectedValue = dr["CodRazonR"].ToString().Trim();

                DataRowView DrP = e.Row.DataItem as DataRowView;
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PosR',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DrP["Posicion"].ToString().Trim());
                DropDownList DdlPosic = (e.Row.FindControl("DdlPosic") as DropDownList);
                DdlPosic.DataSource = Cnx.DSET(LtxtSql);
                DdlPosic.DataTextField = "Descripcion";
                DdlPosic.DataValueField = "Codigo";
                DdlPosic.DataBind();
                DdlPosic.SelectedValue = DrP["Posicion"].ToString().Trim();

                DataRowView DrPN = e.Row.DataItem as DataRowView;
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PNRTE',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DrPN["CodPnOn"].ToString().Trim());
                DropDownList DdlPNOn = (e.Row.FindControl("DdlPNOn") as DropDownList);
                DdlPNOn.DataSource = Cnx.DSET(LtxtSql);
                DdlPNOn.DataTextField = "PN";
                DdlPNOn.DataValueField = "Codigo";
                DdlPNOn.DataBind();
                DdlPNOn.SelectedValue = DrPN["CodPnOn"].ToString().Trim();

                DataRowView DrPNOf = e.Row.DataItem as DataRowView;
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PNRTE',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DrPNOf["CodPnOff"].ToString().Trim());
                DropDownList DdlPNOff = (e.Row.FindControl("DdlPNOff") as DropDownList);
                DdlPNOff.DataSource = Cnx.DSET(LtxtSql);
                DdlPNOff.DataTextField = "PN";
                DdlPNOff.DataValueField = "Codigo";
                DdlPNOff.DataBind();
                DdlPNOff.SelectedValue = DrPNOf["CodPnOff"].ToString().Trim();

                CalendarExtender CalFech = (e.Row.FindControl("CalFech") as CalendarExtender);
                DateTime DiaI = Convert.ToDateTime(TxtFecDet.Text);
                CalFech.StartDate = Convert.ToDateTime(TxtFecDet.Text);
                CalFech.EndDate = DateTime.Now;

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
                if (DdlEstad.Text.Equals("C"))
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { imgE.ToolTip = row["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { imgD.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = true;
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
        protected void GrdSnOnOff_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdSnOnOff.PageIndex = e.NewPageIndex;
            BindDSnOnOff();
            PerfilesGrid();
        }
        //******************************************  herramientas *********************************************************
        protected void BindDHta()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto 17,'','','','',@NR,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                    {
                        SC.Parameters.AddWithValue("@NR", TxtSnOnOffNumRte.Text.Trim());
                        SCX2.Open();
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
                            if (DT.Rows.Count > 0)
                            {
                                GrdHta.DataSource = DT;
                                GrdHta.DataBind();
                            }
                            else
                            {
                                DT.Rows.Add(DT.NewRow());
                                GrdHta.DataSource = DT;
                                GrdHta.DataBind();
                                GrdHta.Rows[0].Cells.Clear();
                                GrdHta.Rows[0].Cells.Add(new TableCell());
                                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                                foreach (DataRow row in Result)
                                { GrdHta.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                GrdHta.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDHta", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlPNHtaPP_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNHtaPP = (GrdHta.FooterRow.FindControl("LtbSNHtaPP") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescHtaPP = (GrdHta.FooterRow.FindControl("TxtDescHtaPP") as TextBox);
            TextBox TxtSNHtaPP = (GrdHta.FooterRow.FindControl("TxtSNHtaPP") as TextBox);
            string VbPn = (GrdHta.FooterRow.FindControl("DdlPNHtaPP") as DropDownList).Text.Trim();
            TextBox TxtFechVcePP = (GrdHta.FooterRow.FindControl("TxtFechVcePP") as TextBox);
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','S',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNHtaPP.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescHtaPP.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNHtaPP.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNHtaPP.Visible = true; }
                }
            }
            TxtSNHtaPP.Text = "";
            TxtFechVcePP.Text = "";
            PerfilesGrid();
        }
        protected void DdlPNHta_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("LtbSNHta") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("TxtDescHta") as TextBox);
            TextBox TxtSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("TxtSNHta") as TextBox);
            string VbPn = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("DdlPNHta") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','S',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNHta.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescHta.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNHta.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals(""))
                    { LtbSNHta.Visible = true; }
                }
            }
            TxtSNHta.Text = "";
            PerfilesGrid();
        }
        protected void LtbSNHtaPP_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNHtaPP = (GrdHta.FooterRow.FindControl("TxtSNHtaPP") as TextBox);
            ListBox LtbSNHtaPP = (GrdHta.FooterRow.FindControl("LtbSNHtaPP") as ListBox);
            TextBox TxtFechVcePP = (GrdHta.FooterRow.FindControl("TxtFechVcePP") as TextBox);
            string VblCampo = LtbSNHtaPP.SelectedValue.Trim();
            int position = VblCampo.Trim().IndexOf("|");
            TxtSNHtaPP.Text = VblCampo.Substring(0, position).Trim();
            TxtFechVcePP.Text = VblCampo.Trim().Substring(position + 1);
            LtbSNHtaPP.Visible = false;
            PerfilesGrid();
        }
        protected void LtbSNHta_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("TxtSNHta") as TextBox);
            ListBox LtbSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("LtbSNHta") as ListBox);
            TxtSNHta.Text = LtbSNHta.SelectedValue.Trim();
            LtbSNHta.Visible = false;
            PerfilesGrid();
        }
        protected void GrdHta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {

                int borrar = GrdHta.Rows.Count;
                if (GrdHta.Rows.Count > 2)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens31'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Solo es posible ingresar 3 herramientas')", true);
                    return;
                }
                int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
                if ((GrdHta.FooterRow.FindControl("TxtFechVcePP") as TextBox).Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens32'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//la fecha vencimiento se encuentra vacía')", true);
                    return;
                }
                DateTime? VbFe = Convert.ToDateTime((GrdHta.FooterRow.FindControl("TxtFechVcePP") as TextBox).Text);
                string VbPn = (GrdHta.FooterRow.FindControl("DdlPNHtaPP") as DropDownList).Text.Trim();
                string VbSn = (GrdHta.FooterRow.FindControl("TxtSNHtaPP") as TextBox).Text.Trim();
                string VbDes = (GrdHta.FooterRow.FindControl("TxtDescHtaPP") as TextBox).Text.Trim();
                if (VbPn.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens33'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar un P/N')", true);
                    return;
                }
                if (VbSn.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens34'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//El campo S/N se encuentra vacío')", true);
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,@Pn,@Sn,@Des,'','','','','','','','','','','INSERT',@CodT,@Rte,0,0,0,0,@Fe,'02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@CodT", 0);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Fe", VbFe);
                                SC.Parameters.AddWithValue("@Pn", VbPn);
                                SC.Parameters.AddWithValue("@Sn", VbSn);
                                SC.Parameters.AddWithValue("@Des", VbDes);
                                SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BindDHta();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Herramientas en Reportes", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdHta.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex;
            BindDHta();
        }
        protected void GrdHta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdHta.DataKeys[Idx].Value.ToString());
            int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
            if ((GrdHta.Rows[Idx].FindControl("TxtFecVce") as TextBox).Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens32'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//La fecha se encuetra vacía')", true);
            }
            DateTime? VbFe = Convert.ToDateTime((GrdHta.Rows[Idx].FindControl("TxtFecVce") as TextBox).Text);
            string VbPn = (GrdHta.Rows[Idx].FindControl("DdlPNHta") as DropDownList).Text.Trim();
            string VbSn = (GrdHta.Rows[Idx].FindControl("TxtSNHta") as TextBox).Text.Trim();
            string VbDes = (GrdHta.Rows[Idx].FindControl("TxtDescHta") as TextBox).Text.Trim();
            if (VbSn.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens34'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//El campo S/N se encuentra vacío')", true);
            }
            if (VbPn.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens33'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar un P/N')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,@Pn,@Sn,@Des,'','','','','','','','','','','UPDATE',@CodT,@Rte,0,0,0,0,@Fe,'02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Fe", VbFe);
                            SC.Parameters.AddWithValue("@Pn", VbPn);
                            SC.Parameters.AddWithValue("@Sn", VbSn);
                            SC.Parameters.AddWithValue("@Des", VbDes);
                            SC.Parameters.AddWithValue("@Rte", VbRte);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdHta.EditIndex = -1;
                            BindDHta();
                            PerfilesGrid();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Herramienta Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdHta.EditIndex = -1;
            BindDHta();
        }
        protected void GrdHta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            string VBQuery;
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdHta.DataKeys[Idx].Value.ToString());
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,'','','','','','','','','','','','','','DELETE',@CodT,@Rte,0,0,0,0,'02-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDHta();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Herramienta Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string LtxtSql = "";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','HTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                DropDownList DdlPNHtaPP = (e.Row.FindControl("DdlPNHtaPP") as DropDownList);
                DdlPNHtaPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNHtaPP.DataTextField = "PN";
                DdlPNHtaPP.DataValueField = "Codigo";
                DdlPNHtaPP.DataBind();

                CalendarExtender CalFechVcePP = (e.Row.FindControl("CalFechVcePP") as CalendarExtender);
                CalFechVcePP.StartDate = DateTime.Now;

                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlEstad.Text.Equals("C"))
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView DrPN = e.Row.DataItem as DataRowView;
                LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','HTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'", DrPN["PN"].ToString().Trim());
                DropDownList DdlPNHta = (e.Row.FindControl("DdlPNHta") as DropDownList);
                DdlPNHta.DataSource = Cnx.DSET(LtxtSql);
                DdlPNHta.DataTextField = "PN";
                DdlPNHta.DataValueField = "Codigo";
                DdlPNHta.DataBind();
                DdlPNHta.SelectedValue = DrPN["PN"].ToString().Trim();

                CalendarExtender CalFechVce = (e.Row.FindControl("CalFechVce") as CalendarExtender);
                CalFechVce.StartDate = DateTime.Now;

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
                if (DdlEstad.Text.Equals("C"))
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { imgE.ToolTip = row["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Cumplido'");
                        foreach (DataRow row in Result)
                        { imgD.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                else
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }

                    if (imgD != null)
                    {
                        imgD.Enabled = true;
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
        protected void GrdHta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdHta.EditIndex = e.NewPageIndex;
            BindDHta();
            PerfilesGrid();
        }
        //******************************************  Procedimientos *********************************************************
        protected void Exportar(string Condcion)
        {
            try
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                string StSql, VbNomRpt, VbOpcion = "";

                switch (Condcion)
                {
                    case "Reserva":
                        CursorIdioma.Alimentar("CURRESERVA", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto2 6,'CURRESERVA','','','','',@SubOT,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        VbNomRpt = "Recurso";
                        break;
                    case "ReporteGeneral":
                        CursorIdioma.Alimentar("CurInfomeRte", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto 4,'CurInfomeRte','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        VbNomRpt = "Reports_Maintenance";
                        break;
                    default:
                        if (TblBusqRte.Visible == true)
                        {
                            //busqueda Reporte
                            CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                            if (RdbBusqRteNum.Checked == true)
                            { VbOpcion = "RteNum"; }
                            if (RdbBusqRteHk.Checked == true)
                            { VbOpcion = "HK"; }
                            if (RdbBusqRteAta.Checked == true)
                            { VbOpcion = "Ata"; }
                            if (RdbBusqRteOT.Checked == true)
                            { VbOpcion = "OT"; }
                            if (RdbBusqRteTecn.Checked == true)
                            { VbOpcion = "Tecn"; }
                            if (RdbBusqRteDescRte.Checked == true)
                            { VbOpcion = "DescRte"; }
                        }
                        StSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,@CodlV,'','CurBusqRte',@Opc,0,0,0,0,'01-01-1','02-01-1','03-01-1'");
                        VbNomRpt = "Report";
                        break;
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@SubOT", TxtRecurSubOt.Text.Trim());// solo cuando es para la reserva (recurso)
                        SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim()); // solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());// solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@CodlV", LblNumLVTit.Text.Trim());// solo cuando es para el reporte
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}