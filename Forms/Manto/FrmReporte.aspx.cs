using _77NeoWeb.prg;
using _77NeoWeb.Prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Manto
{
    public partial class FrmReporte : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTGrDtsRpt = new DataSet();
        DataSet DSTOTGrl = new DataSet();
        DataSet DSTRTE = new DataSet();
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
            Page.Title = string.Format("Reporte de mantenimiento");
            if (!IsPostBack)
            {
                TitForm.Text = "Reporte de mantenimiento";
                ViewState["Validar"] = "S";
                ViewState["Accion"] = "";
                ViewState["CodPrioridad"] = "NORMAL";
                MltVRte.ActiveViewIndex = 0;
                ModSeguridad();
                BindBDdlBusqRte();
                TraerDatosRtes(0, "UPD");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMSRte"] = 1;
            //BtnIngresar.Visible = true;
            ViewState["VblModMSRte"] = 1;
            ViewState["VblEliMSRte"] = 1;
            ViewState["VblImpMSRte"] = 1;
            ViewState["VblCE4Rte"] = 1;
            ViewState["VblCE6Rte"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
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
                            if (Convert.ToInt32(ViewState["VblIngMSRte"]) == 1)
                            { BtnIngresar.Visible = true; }
                        }
                        else
                        {
                            BtnIngresar.Visible = false;
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
                    if (bO.Equals("LblTitRteManto"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("LblTitRteManto") ? bT : TitForm.Text;
                    LblAeroRte.Text = bO.Equals("LblAeroRte") ? bT : LblAeroRte.Text;
                    LblOtSec.Text = bO.Equals("LblOtSec") ? bT : LblOtSec.Text;
                    LblNumLv.Text = bO.Equals("LblNumLv") ? bT + ":" : LblNumLv.Text;
                    LblTitRepMant.Text = bO.Equals("LblTitRteManto") ? bT : LblTitRepMant.Text;
                    LblNroRte.Text = bO.Equals("LblNroRte") ? bT : LblNroRte.Text;
                    LblTipRte.Text = bO.Equals("LblTipRte") ? bT : LblTipRte.Text;
                    LblFuente.Text = bO.Equals("LblFuente") ? bT : LblFuente.Text;
                    LblCasi.Text = bO.Equals("LblCasi") ? bT : LblCasi.Text;
                    LblTall.Text = bO.Equals("LblTall") ? bT : LblTall.Text;
                    LblEstad.Text = bO.Equals("LblEstad") ? bT : LblEstad.Text;
                    LblNotif.Text = bO.Equals("LblNotif") ? bT : LblNotif.Text;
                    LblClasf.Text = bO.Equals("LblClasf") ? bT : LblClasf.Text;
                    LblCatgr.Text = bO.Equals("LblCatgr") ? bT : LblCatgr.Text;
                    LblDocRef.Text = bO.Equals("LblDocRef") ? bT : LblDocRef.Text;
                    LblPosRte.Text = bO.Equals("LblPosRte") ? bT : LblPosRte.Text;
                    LblAta.Text = bO.Equals("LblAta") ? bT : LblAta.Text;
                    Generado.Text = bO.Equals("Generado") ? bT : Generado.Text;
                    LblLicGene.Text = bO.Equals("LblLicGene") ? bT : LblLicGene.Text;
                    LblFecDet.Text = bO.Equals("LblFecDet") ? bT : LblFecDet.Text;
                    LblFecProy.Text = bO.Equals("LblFecProy") ? bT : LblFecProy.Text;
                    LblOtRte.Text = bO.Equals("LblOtRte") ? bT : LblOtRte.Text;
                    LblBasRte.Text = bO.Equals("LblBasRte") ? bT : LblBasRte.Text;
                    LblCumpl.Text = bO.Equals("LblCumpl") ? bT : LblCumpl.Text;
                    LblLicCump.Text = bO.Equals("LblLicGene") ? bT : LblLicCump.Text;
                    LblFecCump.Text = bO.Equals("LblFecCump") ? bT : LblFecCump.Text;
                    lblProgr.Text = bO.Equals("lblProgr") ? bT : lblProgr.Text;
                    LblPgSi.Text = bO.Equals("LblPgSi") ? bT : LblPgSi.Text;
                    LblFallC.Text = bO.Equals("LblFallC") ? bT : LblFallC.Text;
                    LblSi.Text = bO.Equals("LblPgSi") ? bT : LblSi.Text;
                    LblTtlAKSN.Text = bO.Equals("LblTtlAKSN") ? bT : LblTtlAKSN.Text;
                    LblHPrxCu.Text = bO.Equals("LblHPrxCu") ? bT : LblHPrxCu.Text;
                    LblDescRte.Text = bO.Equals("LblDescRte") ? bT : LblDescRte.Text;
                    LblAccCorr.Text = bO.Equals("LblAccCorr") ? bT : LblAccCorr.Text;
                    AcciParc.Text = bO.Equals("AcciParc") ? bT : AcciParc.Text;
                    LblTecDif.Text = bO.Equals("LblTecDif") ? bT : LblTecDif.Text;
                    LblTitDatosVer.Text = bO.Equals("LblTitDatosVer") ? bT : LblTitDatosVer.Text;
                    LblVerif.Text = bO.Equals("LblVerif") ? bT : LblVerif.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnReserva.Text = bO.Equals("BtnReserva") ? bT : BtnReserva.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    BtnImprimir.Text = bO.Equals("BtnImprimir") ? bT : BtnImprimir.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    BtnExporRte.Text = bO.Equals("BtnExporRte") ? bT : BtnExporRte.Text;
                    BtnNotificar.Text = bO.Equals("BtnNotificar") ? bT : BtnNotificar.Text;
                    //**************************************Busqueda *****************************************************
                    RdbBusqRteNum.Text = bO.Equals("RdbBusqRteNum") ? bT : RdbBusqRteNum.Text;
                    RdbBusqRteHk.Text = bO.Equals("RdbBusqRteHk") ? bT : RdbBusqRteHk.Text;
                    RdbBusqRteOT.Text = bO.Equals("RdbBusqRteOT") ? bT : RdbBusqRteOT.Text;
                    RdbBusqRteTecn.Text = bO.Equals("RdbBusqRteTecn") ? bT : RdbBusqRteTecn.Text;
                    RdbBusqRteDescRte.Text = bO.Equals("RdbBusqRteDescRte") ? "&nbsp" + bT : RdbBusqRteDescRte.Text;
                    LblBusq.Text = bO.Equals("Busqueda") ? "&nbsp" + bT : LblBusq.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtConsulPnRecurRte.Attributes.Add("placeholder", bT); }
                    IbtConsultarBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsultarBusq.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    IbtExpConsulRte.ToolTip = bO.Equals("IbtExpConsulRte") ? bT : IbtExpConsulRte.ToolTip;
                    GrdBusq.Columns[0].HeaderText = bO.Equals("GrdSelec") ? bT : GrdBusq.Columns[0].HeaderText;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    //************************************** Recurso *****************************************************
                    LblRecsNumRte.Text = bO.Equals("LblNroRte") ? bT : LblRecsNumRte.Text;
                    LblRecsSubOt.Text = bO.Equals("LblOtSec") ? bT : LblRecsSubOt.Text;
                    LblPrioridadOT.Text = bO.Equals("LblPrioridadOT2") ? bT + ":" : LblPrioridadOT.Text;
                    LblTtlRecursoRte.Text = bO.Equals("LblTtlRecursoRte") ? bT : LblTtlRecursoRte.Text;
                    LblRecsBusq.Text = bO.Equals("Busqueda") ? bT : LblRecsBusq.Text;
                    IbtConsulPnRecurRte.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsulPnRecurRte.ToolTip;
                    IbtCerrarRec.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarRec.ToolTip;
                    IbtExpExcelPnRecurRte.ToolTip = bO.Equals("IbtRecurExpExcelPn") ? bT : IbtExpExcelPnRecurRte.ToolTip;
                    LblTitRecursFis.Text = tbl["Objeto"].ToString().Trim().Equals("BtnReserva") ? tbl["Texto"].ToString().Trim() : LblTitRecursFis.Text;
                    GrdRecursoF.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdRecursoF.Columns[2].HeaderText;
                    GrdRecursoF.Columns[3].HeaderText = bO.Equals("Cantidad") ? bT : GrdRecursoF.Columns[3].HeaderText;
                    GrdRecursoF.Columns[4].HeaderText = bO.Equals("UndMed") ? bT : GrdRecursoF.Columns[4].HeaderText;
                    GrdRecursoF.Columns[5].HeaderText = bO.Trim().Equals("CantEntreg") ? bT : GrdRecursoF.Columns[5].HeaderText;
                    LblTitLicencia.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitLicencia") ? tbl["Texto"].ToString().Trim() : LblTitLicencia.Text;
                    GrdLicen.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Licencia") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[0].HeaderText;
                    GrdLicen.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[1].HeaderText;
                    GrdLicen.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("TiempoEstimado") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[2].HeaderText;
                    //************************************** Carga Masiva *****************************************************
                    BtnCargaMaxiva.Text = bO.Equals("BtnCargaMasivaTT1") ? bT + ":" : BtnCargaMaxiva.Text;
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
                DataRow[] Result = Idioma.Select("Objeto= 'GuardarCargaMaxClientClick'");
                foreach (DataRow row in Result)
                { IbtGuardarCargaMax.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  

                Result = Idioma.Select("Objeto= 'BtnNotificar3'");
                foreach (DataRow row in Result)
                { BtnNotificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea notificar el reporte? 

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {

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
        //******************************************  Reporte de mantenimiento *********************************************************//
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BindBDdlBusqRte()
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 12,'','','','','',0,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                using (SqlCommand SC = new SqlCommand(LtxtSql, Cnx2))
                {
                    SC.Parameters.AddWithValue("@U", Session["C77U"]);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSTOTGrl = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DSTOTGrl);
                            DSTOTGrl.Tables[0].TableName = "V00";
                            DSTOTGrl.Tables[1].TableName = "V01";
                            DSTOTGrl.Tables[2].TableName = "HK";
                            DSTOTGrl.Tables[3].TableName = "V03";
                            DSTOTGrl.Tables[4].TableName = "V04";
                            DSTOTGrl.Tables[5].TableName = "Tllr";
                            DSTOTGrl.Tables[6].TableName = "Base";
                            DSTOTGrl.Tables[7].TableName = "V07";
                            DSTOTGrl.Tables[8].TableName = "V08";
                            DSTOTGrl.Tables[9].TableName = "V09";
                            DSTOTGrl.Tables[10].TableName = "Lcia";
                            DSTOTGrl.Tables[11].TableName = "V11";
                            DSTOTGrl.Tables[12].TableName = "PN_Rsva";
                            DSTOTGrl.Tables[13].TableName = "TipRte";
                            DSTOTGrl.Tables[14].TableName = "FteRte";
                            DSTOTGrl.Tables[15].TableName = "EstadoRte";
                            DSTOTGrl.Tables[16].TableName = "ClasifRte";
                            DSTOTGrl.Tables[17].TableName = "Posicion";
                            DSTOTGrl.Tables[18].TableName = "ATA";
                            DSTOTGrl.Tables[19].TableName = "GeneradoPor";
                            ViewState["DSTOTGrl"] = DSTOTGrl;
                        }
                    }
                }
            }
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];

            DdlAeroRte.DataSource = DSTOTGrl.Tables["HK"];
            DdlAeroRte.DataTextField = "Matricula";
            DdlAeroRte.DataValueField = "CodAeronave";
            DdlAeroRte.DataBind();
        }
        protected void BindDdlRteCondicional(string Categ, string LicGen, string LicCump, string LicVer)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result; string VbCodAnt;

            if (DSTOTGrl.Tables[13].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[13].Clone();

                Result = DSTOTGrl.Tables[13].Select("CodReporte=" + ViewState["TipRteAnt"]);// trae el codigo actual por si esta inactivo
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                Result = DSTOTGrl.Tables[13].Select("Activo=1");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlTipRte.DataSource = DT;
                DdlTipRte.DataTextField = "TipoReporte";
                DdlTipRte.DataValueField = "CodReporte";
                DdlTipRte.DataBind();
                DdlTipRte.Text = ViewState["TipRteAnt"].ToString().Trim().Equals("") ? "7777" : ViewState["TipRteAnt"].ToString().Trim();
            }

            VbCodAnt = DdlFuente.Text.Trim();
            DdlFuente.DataSource = DSTOTGrl.Tables[14];
            DdlFuente.DataTextField = "Descripcion";
            DdlFuente.DataValueField = "Codigo";
            DdlFuente.DataBind();
            DdlFuente.Text = VbCodAnt;

            if (DSTOTGrl.Tables[5].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[5].Clone();
                DataRow[] DR = DSTOTGrl.Tables[5].Select("Activo=1 OR CodTaller = '" + ViewState["TllAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }

                DdlTall.DataSource = DT;
                DdlTall.DataTextField = "NomTaller";
                DdlTall.DataValueField = "CodTaller";
                DdlTall.DataBind();
                DdlTall.Text = ViewState["TllAnt"].ToString().Trim();
            }

            VbCodAnt = DdlEstad.Text.Trim().Equals("") ? "A" : DdlEstad.Text.Trim();
            DdlEstad.DataSource = DSTOTGrl.Tables[15];
            DdlEstad.DataTextField = "Descripcion";
            DdlEstad.DataValueField = "CodStatus";
            DdlEstad.DataBind();
            DdlEstad.Text = VbCodAnt;

            if (DSTOTGrl.Tables[16].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[16].Clone();
                DataRow[] DR = DSTOTGrl.Tables[16].Select("Activo=1 OR Codigo= '" + ViewState["ClsfcnAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlClasf.DataSource = DT;
                DdlClasf.DataTextField = "Descripcion";
                DdlClasf.DataValueField = "Codigo";
                DdlClasf.DataBind();
                DdlClasf.Text = ViewState["ClsfcnAnt"].ToString().Trim();
            }

            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{2}',{3},'','CatM',{1},0,0,{4},'01-01-1','02-01-1','03-01-1'",
               DdlClasf.Text, DdlClasf.SelectedValue.Equals("") ? "0" : DdlAeroRte.Text.Trim(), Categ, Session["77IDM"], Session["!dC!@"]);
            DdlCatgr.DataSource = Cnx.DSET(LtxtSql);
            DdlCatgr.DataTextField = "CodCategoriaMel";
            DdlCatgr.DataValueField = "IdCategoria";
            DdlCatgr.DataBind();

            if (DSTOTGrl.Tables[17].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[17].Clone();
                DataRow[] DR = DSTOTGrl.Tables[17].Select("Activo=1 OR Codigo= '" + ViewState["PscnAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlPosRte.DataSource = DT;
                DdlPosRte.DataTextField = "Descripcion";
                DdlPosRte.DataValueField = "Codigo";
                DdlPosRte.DataBind();
                DdlPosRte.Text = ViewState["PscnAnt"].ToString().Trim();
            }

            VbCodAnt = DdlAtaRte.Text.Trim();
            DdlAtaRte.DataSource = DSTOTGrl.Tables[18];
            DdlAtaRte.DataTextField = "Descripcion";
            DdlAtaRte.DataValueField = "CodCapitulo";
            DdlAtaRte.DataBind();
            DdlAtaRte.Text = VbCodAnt;

            if (DSTOTGrl.Tables[19].Rows.Count > 0) // Datos de tecnicos abrir, cierre, difiere y verificado
            {
                DataTable DTGnrd = new DataTable();
                DataTable DTCmpl = new DataTable();
                DataTable DTDfr = new DataTable();
                DataTable DTVrfc = new DataTable();

                DTGnrd = DSTOTGrl.Tables[19].Clone();
                Result = DSTOTGrl.Tables[19].Select("CodPersona= '" + ViewState["GnrdAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTGnrd.ImportRow(Row); }

                DTCmpl = DSTOTGrl.Tables[19].Clone();
                Result = DSTOTGrl.Tables[19].Select("CodPersona= '" + ViewState["CmplAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTCmpl.ImportRow(Row); }

                DTDfr = DSTOTGrl.Tables[19].Clone();
                Result = DSTOTGrl.Tables[19].Select("CodPersona= '" + ViewState["DfrAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTDfr.ImportRow(Row); }

                DTVrfc = DSTOTGrl.Tables[19].Clone();
                Result = DSTOTGrl.Tables[19].Select("CodPersona= '" + ViewState["VrfcAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTVrfc.ImportRow(Row); }

                Result = DSTOTGrl.Tables[19].Select("CrearReporte= 1 AND Estado = 'ACTIVO'");
                foreach (DataRow Row in Result)
                { DTGnrd.ImportRow(Row); DTCmpl.ImportRow(Row); DTDfr.ImportRow(Row); DTVrfc.ImportRow(Row); }

                DdlGenerado.DataSource = DTGnrd;
                DdlGenerado.DataTextField = "Tecnico";
                DdlGenerado.DataValueField = "CodPersona";
                DdlGenerado.DataBind();

                DdlCumpl.DataSource = DTCmpl;
                DdlCumpl.DataTextField = "Tecnico";
                DdlCumpl.DataValueField = "CodPersona";
                DdlCumpl.DataBind();

                DdlTecDif.DataSource = DTDfr;
                DdlTecDif.DataTextField = "Tecnico";
                DdlTecDif.DataValueField = "CodPersona";
                DdlTecDif.DataBind();

                DdlVerif.DataSource = DTVrfc;
                DdlVerif.DataTextField = "Tecnico";
                DdlVerif.DataValueField = "CodPersona";
                DdlVerif.DataBind();
            }

            if (DSTOTGrl.Tables["Lcia"].Rows.Count > 0) //"Licencia"
            {
                DataTable DTG = new DataTable();
                DTG = DSTOTGrl.Tables["Lcia"].Clone();
                DataRow[] DR = DSTOTGrl.Tables["Lcia"].Select("Activo = 1 AND CodPersona = '" + ViewState["GnrdAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTG = DR.CopyToDataTable(); }
                DTG.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables["Lcia"].Select("Licencia= '" + LicGen.Trim() + "' AND CodPersona = '" + ViewState["GnrdAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTG.ImportRow(Row); }
                DdlLicGene.DataSource = DTG;
                DdlLicGene.DataTextField = "Licencia";
                DdlLicGene.DataValueField = "Codigo";
                DdlLicGene.DataBind();

                DataTable DTC = new DataTable();
                DTC = DSTOTGrl.Tables["Lcia"].Clone();
                DR = DSTOTGrl.Tables["Lcia"].Select("Activo = 1 AND CodPersona = '" + ViewState["CmplAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTC = DR.CopyToDataTable(); }
                DTC.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables["Lcia"].Select("Licencia= '" + LicCump.Trim() + "' AND CodPersona = '" + ViewState["CmplAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTC.ImportRow(Row); }
                DdlLicCump.DataSource = DTC;
                DdlLicCump.DataTextField = "Licencia";
                DdlLicCump.DataValueField = "Codigo";
                DdlLicCump.DataBind();

                DataTable DTV = new DataTable();
                DTV = DSTOTGrl.Tables["Lcia"].Clone();
                DR = DSTOTGrl.Tables["Lcia"].Select("Activo = 1 AND CodPersona = '" + ViewState["VrfcAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTV = DR.CopyToDataTable(); }
                DTV.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables["Lcia"].Select("Licencia= '" + LicVer.Trim() + "' AND CodPersona = '" + ViewState["VrfcAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTV.ImportRow(Row); }
                DdlLicVer.DataSource = DTV;
                DdlLicVer.DataTextField = "Licencia";
                DdlLicVer.DataValueField = "Codigo";
                DdlLicVer.DataBind();
            }

            if (DSTOTGrl.Tables[6].Rows.Count > 0) //"Base"
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTOTGrl.Tables[6].Select("Activo=1 OR CodBase= '" + ViewState["BaseAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlBasRte.DataSource = DT;
                DdlBasRte.DataTextField = "NomBase";
                DdlBasRte.DataValueField = "CodBase";
                DdlBasRte.DataBind();
                DdlBasRte.SelectedValue = ViewState["BaseAnt"].ToString().Trim();
            }

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','OTPP',{0},{1},0,{2},'01-01-1','02-01-1','03-01-1'", DdlAeroRte.Text, ViewState["OtAnt"], Session["!dC!@"]);
            DdlOtRte.DataSource = Cnx.DSET(LtxtSql);
            DdlOtRte.DataTextField = "OT";
            DdlOtRte.DataValueField = "CodNumOrdenTrab";
            DdlOtRte.DataBind();
            DdlOtRte.Text = ViewState["OtAnt"].ToString();

            if (DSTOTGrl.Tables[12].Rows.Count > 0) //"P/N"
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTOTGrl.Tables[12].Select("CodTipoElemento <> ''");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DataView DV = DT.DefaultView;
                DV.Sort = "PN";
                DT = DV.ToTable();
                VbCodAnt = DdlPnRte.Text.Trim();
                DdlPnRte.DataSource = DT;
                DdlPnRte.DataTextField = "PN";
                DdlPnRte.DataValueField = "CodPN";
                DdlPnRte.DataBind();
                DdlPnRte.Text = VbCodAnt;
            }

            VbCodAnt = DdlPrioridadOT.Text.Trim();
            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','',{1},'','PRIO',0,0,0,{2},'01-01-1','02-01-1','03-01-1'", ViewState["CodPrioridad"].ToString(), Session["77IDM"], Session["!dC!@"]);
            DdlPrioridadOT.DataSource = Cnx.DSET(LtxtSql);
            DdlPrioridadOT.DataTextField = "Descripcion";
            DdlPrioridadOT.DataValueField = "CodPrioridadSolicitudMat";
            DdlPrioridadOT.DataBind();
            DdlPrioridadOT.Text = VbCodAnt;
        }
        protected void TraerDatosRtes(int NumRte, string Accion)
        {
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        using (SqlCommand SC = new SqlCommand("EXEC SP_PANTALLA_Reporte_Manto2 2,'','','','','',@Rt,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'", Cnx2))
                        {
                            SC.Parameters.AddWithValue("@Rt", NumRte);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTRTE = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTRTE);
                                    DSTRTE.Tables[0].TableName = "DatosRte";
                                    DSTRTE.Tables[1].TableName = "BusqRte";
                                    DSTRTE.Tables[2].TableName = "RFisco";
                                    DSTRTE.Tables[3].TableName = "PNS";
                                    DSTRTE.Tables[4].TableName = "TimeLic";
                                    DSTRTE.Tables[5].TableName = "Licencia";
                                    DSTRTE.Tables[6].TableName = "ImpRte";
                                    DSTRTE.Tables[7].TableName = "SNOnOff";
                                    DSTRTE.Tables[8].TableName = "RazonR";
                                    DSTRTE.Tables[9].TableName = "PosSnOnOff";
                                    DSTRTE.Tables[10].TableName = "Hrrts";/**/
                                    ViewState["DSTRTE"] = DSTRTE;

                                }
                            }
                        }
                    }
                }
                DSTRTE = (DataSet)ViewState["DSTRTE"];
                string VbCodAnt = "";

                VbCodAnt = DdlBusqRte.Text.Trim().Equals("") ? "0" : DdlBusqRte.Text.Trim();
                DdlBusqRte.DataSource = DSTRTE.Tables[1];
                DdlBusqRte.DataTextField = "NumRte";
                DdlBusqRte.DataValueField = "Codigo";
                DdlBusqRte.DataBind();
                DdlBusqRte.Text = VbCodAnt;

                if (DSTRTE.Tables[0].Rows.Count > 0)
                {
                    ViewState["TipRteAnt"] = DSTRTE.Tables[0].Rows[0]["TipoReporte"].ToString();
                    string VbCodCat = DSTRTE.Tables[0].Rows[0]["CodCategoriaMel"].ToString().Trim();
                    string VbLicGen = DSTRTE.Tables[0].Rows[0]["NumLicTecAbre"].ToString().Trim();
                    string VbLicCump = DSTRTE.Tables[0].Rows[0]["NumLicTecCierre"].ToString().Trim();
                    string VbLicVer = DSTRTE.Tables[0].Rows[0]["NumLicenciaRM"].ToString().Trim();
                    ViewState["TllAnt"] = DSTRTE.Tables[0].Rows[0]["CodTaller"].ToString().Trim();
                    ViewState["ClsfcnAnt"] = DSTRTE.Tables[0].Rows[0]["CodClasifReporteManto"].ToString().Trim();
                    ViewState["PscnAnt"] = DSTRTE.Tables[0].Rows[0]["Posicion"].ToString().Trim();
                    ViewState["GnrdAnt"] = DSTRTE.Tables[0].Rows[0]["ReportadoPor"].ToString().Trim();
                    ViewState["CmplAnt"] = DSTRTE.Tables[0].Rows[0]["CodTecnico"].ToString().Trim();
                    ViewState["DfrAnt"] = DSTRTE.Tables[0].Rows[0]["CodUsuarioDiferido"].ToString().Trim();
                    ViewState["VrfcAnt"] = DSTRTE.Tables[0].Rows[0]["CodInspectorVerifica"].ToString().Trim();
                    ViewState["ESTAPPT"] = DSTRTE.Tables[0].Rows[0]["EstaPPT"].ToString().Trim();
                    ViewState["CodPrioridad"] = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["CodPrioridad"].ToString().Trim());
                    ViewState["BaseAnt"] = DSTRTE.Tables[0].Rows[0]["CodBase"].ToString().Trim();
                    ViewState["OtAnt"] = DSTRTE.Tables[0].Rows[0]["OtPrincipal"].ToString().Trim();
                    BindDdlRteCondicional(VbCodCat, VbLicGen, VbLicCump, VbLicVer);

                    DdlAeroRte.Text = DSTRTE.Tables[0].Rows[0]["CodAeronave"].ToString();
                    TxtNroRte.Text = DSTRTE.Tables[0].Rows[0]["NumReporte"].ToString();
                    TxtCodigoRte.Text = DSTRTE.Tables[0].Rows[0]["CodigoRTE"].ToString();
                    TxtConsTall.Text = DSTRTE.Tables[0].Rows[0]["ConsecutivoROTP"].ToString().Trim();
                    DdlFuente.SelectedValue = DSTRTE.Tables[0].Rows[0]["Fuente"].ToString().Trim();
                    TxtCas.Text = DSTRTE.Tables[0].Rows[0]["NumCasilla"].ToString();
                    DdlEstad.SelectedValue = DSTRTE.Tables[0].Rows[0]["Estado"].ToString().Trim();
                    CkbNotif.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["Notificado"].ToString());
                    BtnNotificar.Enabled = CkbNotif.Checked == true ? false : true;
                    DdlCatgr.SelectedValue = VbCodCat;
                    TxtDocRef.Text = DSTRTE.Tables[0].Rows[0]["DocumentoRef"].ToString().Trim();
                    DdlAtaRte.SelectedValue = DSTRTE.Tables[0].Rows[0]["UbicacionTecnica"].ToString().Trim();
                    DdlGenerado.SelectedValue = ViewState["GnrdAnt"].ToString().Trim();
                    DdlLicGene.SelectedValue = VbLicGen;

                    string VbFecSt;
                    DateTime? VbFecDT;

                    VbFecSt = DSTRTE.Tables[0].Rows[0]["FechaReporte"].ToString().Trim().Equals("") ? "01/01/1900" : DSTRTE.Tables[0].Rows[0]["FechaReporte"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecDet.Text = VbFecSt.Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    VbFecSt = DSTRTE.Tables[0].Rows[0]["FechaProyectada"].ToString().Trim().Equals("") ? "01/01/1900" : DSTRTE.Tables[0].Rows[0]["FechaProyectada"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecPry.Text = VbFecSt.Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    VbFecSt = DSTRTE.Tables[0].Rows[0]["FechaCumplimiento"].ToString().Trim().Equals("") ? "01/01/1900" : DSTRTE.Tables[0].Rows[0]["FechaCumplimiento"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecCump.Text = VbFecSt.Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    DdlOtRte.Text = DSTRTE.Tables[0].Rows[0]["OtPrincipal"].ToString().Trim();
                    DdlCumpl.SelectedValue = ViewState["CmplAnt"].ToString().Trim();
                    DdlLicCump.SelectedValue = VbLicCump;
                    RdbPgSi.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["ProgramadoSi"].ToString());
                    RdbPgNo.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["ProgramadoNo"].ToString());
                    RdbFlCSi.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["FallaConfirmadaSi"].ToString());
                    RdbFlCNo.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["FallaConfirmadaNo"].ToString());
                    CkbRII.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["RII"].ToString());
                    DdlPnRte.Text = DSTRTE.Tables[0].Rows[0]["ParteNumero"].ToString().Trim();
                    TxtSnRte.Text = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["SerieNumero"].ToString().Trim());
                    TxtTtlAKSN.Text = DSTRTE.Tables[0].Rows[0]["TT_A_C"].ToString().Trim();
                    TxtHPrxCu.Text = DSTRTE.Tables[0].Rows[0]["HraProxCump"].ToString().Trim();
                    TxtNexDue.Text = DSTRTE.Tables[0].Rows[0]["Next_Due"].ToString().Trim();
                    TxtDescRte.Text = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["Reporte"].ToString().Trim());
                    txtAccCrr.Text = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["AccionCorrectiva"].ToString().Trim());
                    TxtAcciParc.Text = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["AccionParcial"].ToString().Trim());
                    DdlTecDif.SelectedValue = ViewState["DfrAnt"].ToString().Trim();
                    DdlVerif.SelectedValue = ViewState["VrfcAnt"].ToString().Trim();
                    DdlLicVer.SelectedValue = VbLicVer;
                    CkbTearDown.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["TearDown"].ToString());
                    ViewState["PasoOT"] = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["PasoOT"].ToString().Trim());
                    TxtOtSec.Text = DSTRTE.Tables[0].Rows[0]["OtSec"].ToString().Trim();
                    TxtCodigoOtSec.Text = DSTRTE.Tables[0].Rows[0]["CodigoOTSec"].ToString().Trim();
                    ViewState["IDMroRepOT"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["IDMroRepOT"].ToString());
                    ViewState["BloquearDetalle"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["BloquearDetalle"].ToString());
                    ViewState["TtlRegDet"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["TtlRegDet"].ToString());
                    ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["CargaMasiva"].ToString().Trim());
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
        }
        protected void ActivarCampRte(bool Ing, bool Edi, string accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlEstad.SelectedValue.Equals("C") && DdlTipRte.Enabled == false)
            {
                if (Convert.ToInt32(ViewState["VblCE6Rte"]) == 1)
                {
                    DdlEstad.Enabled = Edi;
                    if (DdlVerif.Text.Equals(""))
                    {
                        // DdlVerif.Enabled = Edi;
                        /* DdlVerif.Text = Session["C77U"].ToString().Trim();
                         string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}','','','LICTA',0,0,0,{2},'01-01-1','02-01-1','03-01-1'", DdlVerif.Text, "", Session["!dC!@"]);
                         DdlLicVer.DataSource = Cnx.DSET(LtxtSql);
                         DdlLicVer.DataMember = "Datos";
                         DdlLicVer.DataTextField = "Licencia";
                         DdlLicVer.DataValueField = "Codigo";
                         DdlLicVer.DataBind();*/
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
                TxtFecDet.Enabled = Edi;
                TxtFecPry.Enabled = Edi;
                if (DdlOtRte.Text.Equals("0") || DdlOtRte.Text.Equals(""))
                { DdlOtRte.Enabled = Edi; }
                DdlBasRte.Enabled = Edi;
                DdlCumpl.Enabled = ViewState["UsuDefecto"].Equals("S") ? false : Edi;
                DdlLicCump.Enabled = Edi;
                TxtFecCump.Enabled = Edi;
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
                    {
                        string VbMnsjIdm = "";
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens23'");
                        foreach (DataRow row in Result)
                        { VbMnsjIdm = row["Texto"].ToString().Trim(); }
                        DdlPnRte.ToolTip = VbMnsjIdm; TxtSnRte.ToolTip = VbMnsjIdm;
                    }
                    if (DdlPnRte.Text.Trim().Equals("") && !DdlOtRte.Text.Trim().Equals("0") && TxtNumLv.Text.Trim().Equals("") && ViewState["ESTAPPT"].ToString().Equals("N"))
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
            TxtCodigoOtSec.Text = "0";
            TxtNroRte.Text = "0";
            TxtCodigoRte.Text = "";
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
            DdlAeroRte.Text = "0";
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
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una aeronave o P/N'
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlAeroRte.Text.Equals("0") && DdlAeroRte.Enabled == true && DdlPnRte.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens02'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una aeronave
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlTipRte.Text.Trim().Equals("7777"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens03'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un tipo reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlFuente.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens04'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fuente
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlClasf.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens05'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una clasificación
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlCatgr.Text.Trim().Equals("") && (DdlClasf.Text.Trim().Equals("CARRY OVER") || DdlClasf.Text.Trim().Equals("CARRY FORWARD")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens06'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una categoría
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtDocRef.Text.Trim().Equals("") && DdlClasf.Text.Trim().Equals("CARRY OVER"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens07'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un documento referencia
                    ViewState["Validar"] = "N";
                    TxtDocRef.Focus();
                    return;
                }
                if (DdlAtaRte.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens08'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una ATA
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlGenerado.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens09'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el usuario que genera el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlLicGene.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens10'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la licencia  del usuario que genera el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtFecDet.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha
                    ViewState["Validar"] = "N";
                    TxtFecDet.Focus();
                    return;
                }
                if (TxtFecPry.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens12'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha de proyección
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
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el usuario que cierra el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlLicCump.Text.Trim().Equals("") && DdlEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens15'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la licencia del usuario que cierra el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtFecCump.Text.Trim().Equals("") && DdlEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens16'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha de cumplimiento
                    ViewState["Validar"] = "N";
                    TxtFecCump.Focus();
                    return;
                }
                if (DdlPnRte.Text.Trim().Equals("") && !TxtSnRte.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens17'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un P/N si el campo S/N se encuentra con información
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlEstad.Text.Equals("A") && !txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens18'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe cerrar el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlEstad.Text.Equals("C") && txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la acción correctiva
                    ViewState["Validar"] = "N";
                    txtAccCrr.Focus();
                    return;
                }
                if (DdlEstad.Text.Equals("A") && !TxtFecCump.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens20'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe cerrar el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtDescRte.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens21'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la descripción del reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtAcciParc.Text.Equals("") && (DdlClasf.Text.Trim().Equals("CARRY OVER") || DdlClasf.Text.Trim().Equals("CARRY FORWARD")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens22'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una acción parcial si el reporte está clasificado como diferido
                    ViewState["Validar"] = "N";
                    TxtAcciParc.Focus();
                    return;
                }
                if (!TxtAcciParc.Text.Equals("") && DdlTecDif.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens23'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el técnico que difiere el reporte
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["PermiteFechaIgualDetPry"].Equals("N") && TxtFecDet.Text == TxtFecPry.Text && DdlClasf.Text.Trim().Equals("CARRY FORWARD"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens24'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //La fecha de detección y la fecha de proyección no pueden ser iguales cuando es un reporte C/F.
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

                Cnx.ValidarFechas(TxtFecPry.Text, "", 1);
                string Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecPry.Focus(); ViewState["Validar"] = "N";
                    return;
                }

                Cnx.ValidarFechas(TxtFecCump.Text, "", 1);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals("") && !TxtFecCump.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecCump.Focus(); ViewState["Validar"] = "N";
                    return;
                }

                Cnx.ValidarFechas(TxtFecDet.Text, TxtFecPry.Text, 2);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecPry.Focus(); ViewState["Validar"] = "N";
                    return;
                }

                Cnx.ValidarFechas(TxtFecDet.Text, TxtFecCump.Text, 2);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals("") && !TxtFecCump.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecCump.Focus(); ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarRpte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                    DateTime? VbFecDT;
                    VbFecDT = Convert.ToDateTime(TxtFecDet.Text).AddDays(VbCritDias);
                    TxtFecPry.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
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
            TraerDatosRtes(Convert.ToInt32(DdlBusqRte.SelectedValue), "UPD");
            PerfilesGrid();
        }
        protected void DdlAeroRte_TextChanged(object sender, EventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','OTPP',{0},{1},0,{2},'01-01-1','02-01-1','03-01-1'", DdlAeroRte.Text, "0", Session["!dC!@"]);
            DdlOtRte.DataSource = Cnx.DSET(LtxtSql);
            DdlOtRte.DataMember = "Datos";
            DdlOtRte.DataTextField = "OT";
            DdlOtRte.DataValueField = "CodNumOrdenTrab";
            DdlOtRte.DataBind();
            DdlOtRte.Text = "0";
            DdlClasf.Text = "";
            DdlCatgr.Text = "";
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
                    LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','LICTA',0,0,0,{1},'01-01-1','02-01-1','03-01-1'", DdlCumpl.SelectedValue, Session["!dC!@"]);
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
                    LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','LICTA',0,0,0,{1},'01-01-1','02-01-1','03-01-1'", DdlGenerado.SelectedValue, Session["!dC!@"]);
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
            string VbHk = DdlAeroRte.Text.Trim().Equals("") ? "0" : DdlAeroRte.Text.Trim();

            DataTable DT = new DataTable();
            string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 1,@CL,'','','','CatM', @HK,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(LtxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CL", DdlClasf.Text);
                    SC.Parameters.AddWithValue("@HK", VbHk);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DT);
                        DdlCatgr.DataSource = DT;
                        DdlCatgr.DataTextField = "CodCategoriaMel";
                        DdlCatgr.DataValueField = "IdCategoria";
                        DdlCatgr.DataBind();
                    }
                }
            }
            DdlCatgr.Text = "";
            if (DdlClasf.Text.Equals("CARRY OVER"))
            { TxtFecPry.Enabled = false; }
            else
            { TxtFecPry.Enabled = true; }
        }
        protected void DdlCatgr_TextChanged(object sender, EventArgs e)
        {
            if (!DdlCatgr.Text.Equals("")) { CalcularFechaPry(); }
        }
        protected void TxtFecDet_TextChanged(object sender, EventArgs e)
        {
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
        { CalcularNexDue(TxtTtlAKSN.Text, TxtHPrxCu.Text); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ViewState["Accion"] = "Aceptar";
                    ActivarBtnRpt(true, false, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    LimpiarCamposRte();
                    DdlAeroRte.Text = DdlAeroRte.Text;
                    TxtFecDet.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    TxtFecPry.Text = TxtFecDet.Text;
                    ActivarCampRte(true, true, "Ingresar");
                    string vbleUsuGe = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlGenerado.SelectedValue;
                    DdlGenerado.SelectedValue = vbleUsuGe;

                    ViewState["TipRteAnt"] = "7777";
                    ViewState["TllAnt"] = "";
                    ViewState["ClsfcnAnt"] = "";
                    ViewState["PscnAnt"] = "";
                    ViewState["GnrdAnt"] = vbleUsuGe.Trim();
                    ViewState["CmplAnt"] = "-1";
                    ViewState["DfrAnt"] = "-1";
                    ViewState["VrfcAnt"] = "-1";
                    ViewState["BaseAnt"] = "";
                    ViewState["OtAnt"] = "0";
                    BindDdlRteCondicional("", "", "", "");
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
                    else { FecCump = Convert.ToDateTime(TxtFecCump.Text); }

                    List<ClsTypLvDetalleManto> ObjLvDetManto = new List<ClsTypLvDetalleManto>();
                    var TypLvDetManto = new ClsTypLvDetalleManto()
                    {
                        FechaProyectada = Convert.ToDateTime(TxtFecPry.Text.Trim()),
                        FechaCumplimiento = FecCump,
                        FechaReporte = Convert.ToDateTime(TxtFecDet.Text),
                        FechaInicio = null,
                        FechaVerificacion = null,
                        CodIdLvDetManto = 0,
                        CodLibroVuelo = TxtNumLv.Text.Trim(),
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
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtnRpt(true, true, true, true, true);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampRte(false, false, "Ingresar");
                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(ClsLvDetManto.GetCodIdRte(), "UPD");
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNroRte.Text.Equals("0"))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ViewState["Accion"] = "Aceptar";
                    string VblLicGenAnt, VbLicCumpAnt, VbLicVerif, VbOtAnt, VblCat;
                    VblLicGenAnt = DdlLicGene.Text;
                    VbLicCumpAnt = DdlLicCump.Text;
                    VbOtAnt = DdlOtRte.Text;
                    VblCat = DdlCatgr.Text;
                    VbLicVerif = DdlLicVer.Text;
                    string VbOT = DdlOtRte.Text;

                    ViewState["TipRteAnt"] = DdlTipRte.Text.Trim();
                    ViewState["TllAnt"] = DdlTall.Text.Trim();
                    ViewState["ClsfcnAnt"] = DdlClasf.Text.Trim();
                    ViewState["PscnAnt"] = DdlPosRte.Text.Trim();
                    ViewState["GnrdAnt"] = DdlGenerado.Text.Trim();
                    ViewState["CmplAnt"] = DdlCumpl.Text.Trim();
                    ViewState["DfrAnt"] = DdlTecDif.Text.Trim();
                    ViewState["VrfcAnt"] = DdlVerif.Text.Trim();
                    ViewState["OtAnt"] = DdlOtRte.Text.Trim();
                    BindDdlRteCondicional(DdlCatgr.Text, VblLicGenAnt, VbLicCumpAnt, VbLicVerif);

                    DdlLicGene.Text = VblLicGenAnt;
                    DdlLicCump.Text = VbLicCumpAnt;
                    DdlOtRte.Text = VbOtAnt;

                    DdlCatgr.Text = VblCat;
                    DdlLicVer.Text = VbLicVerif;

                    DdlOtRte.Text = VbOT;
                    ActivarBtnRpt(false, true, false, false, false);
                    DataRow[] Result1 = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result1)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampRte(true, true, "UPDATE");
                    DdlBusqRte.SelectedValue = "0";
                    DdlBusqRte.Enabled = false;
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
                        CodLibroVuelo = TxtNumLv.Text.Trim(),
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
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    int CodIdRte = ClsLvDetManto.GetCodIdRte();
                    ActivarBtnRpt(true, true, true, true, true);
                    DataRow[] Result3 = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result3)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampRte(false, false, "UPDATE");
                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en la actualización')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                    string VBQuery = string.Format("EXEC SP_PANTALLA_Reporte_Manto 12,@Usu,'','','',@Rte,@HK,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtNroRte.Text));
                            SC.Parameters.AddWithValue("@HK", Convert.ToInt32(DdlAeroRte.Text));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            //string VbMensj = (string)SC.ExecuteScalar();
                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals("S"))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { VbMensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "')", true);
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
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Reporte Manto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BtnExporRte_Click(object sender, EventArgs e)
        { Exportar("ReporteGeneral"); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnNotificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            if (DdlEstad.Text.Equals("A"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens27'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El reporte debe estar cerrado.')", true);
                return;
            }
            if ((int)ViewState["TtlRegDet"] > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens28'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//No es posible notificar un reporte con recurso físico.')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 3,@Usu,'','','','','','','','','','','','','','',@Rte,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'	");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", TxtNroRte.Text);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            CkbNotif.Checked = true;
                            BtnNotificar.Enabled = false;
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Notificar Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        //******************************************  Opciones de busqueda *********************************************************
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result1 = Idioma.Select("Objeto= 'LblOpbusRTE'");
            foreach (DataRow row in Result1)
            { LblOpbusRTE.Text = row["Texto"].ToString().Trim(); }//Opciones de búsqueda reporte de mantenimiento
            RdbBusqRteNum.Checked = true;
            TblBusqRte.Visible = true;
            IbtExpConsulRte.Visible = true;
            GrdBusq.DataSource = null;
            GrdBusq.DataBind();
            MltVRte.ActiveViewIndex = 1;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void IbtConsultarBusq_Click(object sender, ImageClickEventArgs e)
        { BIndDataBusq(); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtExpConsulRte_Click(object sender, ImageClickEventArgs e)
        { Exportar("Reporte"); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { TblBusqRte.Visible = false; MltVRte.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BIndDataBusq()
        {
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "", VbOpcion = "";
                if (RdbBusqRteNum.Checked == true) { VbOpcion = "RteNum"; }
                if (RdbBusqRteHk.Checked == true) { VbOpcion = "HK"; }
                if (RdbBusqRteAta.Checked == true) { VbOpcion = "Ata"; }
                if (RdbBusqRteOT.Checked == true) { VbOpcion = "OT"; }
                if (RdbBusqRteTecn.Checked == true) { VbOpcion = "Tecn"; }
                if (RdbBusqRteDescRte.Checked == true) { VbOpcion = "DescRte"; }
                VbTxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,'','','CurBusqRte',@Opc,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            string VblLbl = "";
                            DataRow[] Result1 = Idioma.Select("Objeto= 'GrdEnviar'");
                            foreach (DataRow row in Result1)
                            { VblLbl = row["Texto"].ToString().Trim(); }
                            GrdBusq.DataSource = DtB; GrdBusq.DataBind();

                            foreach (GridViewRow row in GrdBusq.Rows)
                            { LinkButton lb = (LinkButton)row.Cells[0].Controls[0]; lb.Text = VblLbl; }
                        }
                        else
                        { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text);
            TraerDatosRtes(Convert.ToInt32(vbcod), "UPD");
            MltVRte.ActiveViewIndex = 0;
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdBusq.PageIndex = e.NewPageIndex; BIndDataBusq(); }
        //******************************************  Recurso y Licencia Reporte de mantenimiento *********************************************************
        protected void BtnReserva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (!TxtNroRte.Text.Equals("0"))
            {
                TxtRecurNumRte.Text = TxtNroRte.Text;
                TxtRecurCodRte.Text = TxtCodigoRte.Text;
                TxtRecurSubOt.Text = TxtOtSec.Text;
                TxtRecurSubCodigoOt.Text = TxtCodigoOtSec.Text;
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
                MltVRte.ActiveViewIndex = 2;
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BindDRecursoF()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTRTE = (DataSet)ViewState["DSTRTE"];

                DataRow[] Result;
                DataTable DT = new DataTable();

                DT = DSTRTE.Tables[2].Clone();
                Result = DSTRTE.Tables[2].Select("PN LIKE '%" + TxtConsulPnRecurRte.Text.Trim() + "%'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                if (DT.Rows.Count > 0)
                {
                    DataView DV = DT.DefaultView;
                    DV.Sort = "NumeroPosicion";
                    DT = DV.ToTable();
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
                    Result = Idioma.Select("Objeto= 'RteMens41'");
                    foreach (DataRow row in Result)
                    { GrdRecursoF.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtConsulPnRecurRte_Click(object sender, ImageClickEventArgs e)
        { BindDRecursoF(); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtExpExcelPnRecurRte_Click(object sender, ImageClickEventArgs e)
        { Exportar("Reserva"); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtCerrarRec_Click(object sender, ImageClickEventArgs e)
        {
            TxtOtSec.Text = TxtRecurSubOt.Text;
            TxtCodigoOtSec.Text = TxtRecurSubCodigoOt.Text;
            ViewState["CodPrioridad"] = DdlPrioridadOT.Text.Trim();
            MltVRte.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void DdlPNRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
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
            DataRow[] Result = DSTOTGrl.Tables[12].Select("PN= '" + DdlPNRFPP.Text.Trim() + "'");
            foreach (DataRow row in Result)
            { TxtDesRFPP.Text = row["Descripcion"].ToString().Trim(); }
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
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una prioridad')", true);
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'', @ICC,'INSERT',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");

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
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@SubOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                    SC.Parameters.AddWithValue("@Cant", VblCant);
                                    SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlAeroRte.Text));
                                    SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(TxtNroRte.Text));

                                    string Mensj = "OK";
                                    string VbEjecPlano = "N", VbCodigoOT = "";

                                    int VblSubOt = Convert.ToInt32(TxtRecurSubOt.Text);
                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        VblSubOt = Convert.ToInt32(SDR["SubOT"].ToString().Trim());
                                        VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim());
                                        VbCodigoOT = HttpUtility.HtmlDecode(SDR["CodigoOT"].ToString().Trim());
                                    }
                                    SDR.Close();

                                    Transac.Commit();
                                    if (!Mensj.ToString().Trim().Equals("OK"))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString().Trim(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                        return;
                                    }

                                    TxtRecurSubOt.Text = VblSubOt.ToString();
                                    TxtRecurSubCodigoOt.Text = VbCodigoOT.ToString();

                                    if (VbEjecPlano.Trim().Equals("S"))
                                    {
                                        Cnx.SelecBD();
                                        using (SqlConnection SCnxPln = new SqlConnection(Cnx.GetConex()))
                                        {
                                            SCnxPln.Open();
                                            VBQuery = string.Format("EXEC SP_IntegradorNEW 6,'',@Usu,'','','',@CodOT,0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                                            using (SqlCommand sqlCmd = new SqlCommand(VBQuery, SCnxPln))
                                            {
                                                try
                                                {
                                                    sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                                    sqlCmd.Parameters.AddWithValue("@CodOT", Convert.ToInt32(TxtRecurSubOt.Text));
                                                    sqlCmd.ExecuteNonQuery();
                                                }
                                                catch (Exception Ex)
                                                {
                                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                                    foreach (DataRow row in Result)
                                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//EError en el proceso de eliminación')", true);
                                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Generar Nueva Reserva", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                                }
                                            }
                                        }
                                    }
                                    TxtConsulPnRecurRte.Text = "";
                                    TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                    BindDRecursoF();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens07'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una prioridad')", true);
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'',@ICC,'UPDATE',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");

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
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    return;
                                }
                                TxtRecurSubOt.Text = VblSubOt.ToString();
                                GrdRecursoF.EditIndex = -1;
                                TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                BindDRecursoF();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdRecursoF.EditIndex = -1; BindDRecursoF(); }
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,'','','','','',@ICC,'DELETE',@IdDetRsva,@SubOT,@Cant,@CodHK,@IdRte,@Posc,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                                TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                BindDRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0,0,{0},'01-01-01','01-01-01','01-01-01'", Session["!dC!@"]);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = DSTOTGrl.Tables[12]; // PN_Rsva;
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
        { GrdRecursoF.PageIndex = e.NewPageIndex; BindDRecursoF(); PerfilesGrid(); }
        //******************************** Licencias ********************************
        protected void BindDLicencia()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTRTE = (DataSet)ViewState["DSTRTE"];

                if (DSTRTE.Tables[4].Rows.Count > 0) { GrdLicen.DataSource = DSTRTE.Tables[4]; GrdLicen.DataBind(); }
                else
                {
                    DSTRTE.Tables[4].Rows.Add(DSTRTE.Tables[4].NewRow());
                    GrdLicen.DataSource = DSTRTE.Tables[4];
                    GrdLicen.DataBind();
                    GrdLicen.Rows[0].Cells.Clear();
                    GrdLicen.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens40'");
                    foreach (DataRow row in Result)
                    { GrdLicen.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdLicen.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDRecursoF", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlLicenRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DSTRTE = (DataSet)ViewState["DSTRTE"];
            TextBox TxtDesLiRFPP = (GrdLicen.FooterRow.FindControl("TxtDesLiRFPP") as TextBox);
            DropDownList DdlLicenRFPP = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList);

            DataRow[] Result = DSTRTE.Tables[5].Select("CodIdLicencia= " + DdlLicenRFPP.Text.Trim());
            foreach (DataRow row in Result)
            { TxtDesLiRFPP.Text = row["Descripcion"].ToString().Trim(); }
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
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar una licencia
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','INSERT',0,@CodIdLic,@TiempEst,0,@NumRte, @ICC,'01-01-1','02-01-1','03-01-1'");
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                                    SC.Parameters.AddWithValue("@TiempEst", VblTE);
                                    SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    var Mensj = SC.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString().Trim(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                    BindDLicencia();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Licencia REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso REPORTE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdLicen.EditIndex = e.NewEditIndex; BindDLicencia(); }
        protected void GrdLicen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','UPDATE',0,@CodIdLic,@TiempEst,@IdSvcLic,@NumRte, @ICC,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                                SC.Parameters.AddWithValue("@TiempEst", VblTE);
                                SC.Parameters.AddWithValue("@IdSvcLic", IdSrvLic);
                                SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                GrdLicen.EditIndex = -1;
                                TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                BindDLicencia();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Error en el editar')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "GrdLicen_RowUpdating Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdLicen.EditIndex = -1; BindDLicencia(); }
        protected void GrdLicen_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
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
                    string VBQuery = string.Format("EXEC SP_TablasIngenieria 8,@Usu,'','','','','','','','DELETE',0,@CodIdLic,@TiempEst,@IdSvcLic,@NumRte, @ICC,'01-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@CodIdLic", VbCodIdLicencia);
                            SC.Parameters.AddWithValue("@TiempEst", Convert.ToDouble(VblTE));
                            SC.Parameters.AddWithValue("@IdSvcLic", IdSrvLic);
                            SC.Parameters.AddWithValue("@NumRte", Convert.ToInt32(TxtRecurNumRte.Text));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                            BindDLicencia();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlRecursoRte, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTRTE = (DataSet)ViewState["DSTRTE"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = DSTRTE.Tables[5];
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
        { GrdLicen.PageIndex = e.NewPageIndex; BindDLicencia(); PerfilesGrid(); }
        //******************************************  Subir Recurso maxivamente *********************************************************
        protected void BtnCargaMaxiva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((int)ViewState["TtlRegDet"] > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12'");
                foreach (DataRow row in Result)
                { string borr = row["Texto"].ToString().Trim(); ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //'Para realizar la carga masiva la reserva debe estar vacía
                return;
            }
            TxtCargaMasiRte.Text = TxtRecurNumRte.Text;
            TxtCargaMasiCodRte.Text = TxtRecurCodRte.Text;
            TxtCargaMasiOT.Text = TxtRecurSubOt.Text;
            IbtGuardarCargaMax.Enabled = false;
            MltVRte.ActiveViewIndex = 3;
            Page.Title = ViewState["PageTit"].ToString().Trim();
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
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCargaMasiva, UpPnlRecursoRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }//Debe estar generado el número de la reserva')", true);
                    return;
                }
                DataTable DT = new DataTable();
                string FileName = "";
                string conexion = "";
                FileName = "CargaMasiva.xlsx";
                conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ViewState["CarpetaCargaMasiva"].ToString().Trim() + FileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
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
                            ProcesoOrigen = "RESERVA",
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
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                    ProcesoOrigen = "RESERVA",
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
            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
            BindDRecursoF();
            MltVRte.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void IbtCerrarSubMaxivo_Click(object sender, ImageClickEventArgs e)
        { MltVRte.ActiveViewIndex = 2; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        //******************************************  Impresion Reporte *********************************************************
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            MltVRte.ActiveViewIndex = 4;
            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DSTRTE = (DataSet)ViewState["DSTRTE"];
            ReportParameter[] parameters = new ReportParameter[3];

            parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
            parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
            parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

            RvwReporte.LocalReport.EnableExternalImages = true;
            RvwReporte.LocalReport.ReportPath = "Report/Ing/ReporteV2.rdlc";
            RvwReporte.LocalReport.DataSources.Clear();
            RvwReporte.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DSTRTE.Tables[6]));
            RvwReporte.LocalReport.SetParameters(parameters);
            RvwReporte.LocalReport.Refresh();
        }
        protected void IbtCerrarImpresion_Click(object sender, ImageClickEventArgs e)
        { MltVRte.ActiveViewIndex = 0; }
        //******************************************  SN On Off  *********************************************************
        protected void BtnSnOnOf_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            TxtSnOnOffNumRte.Text = TxtNroRte.Text;
            TxtSnOnOffCodRte.Text = TxtCodigoRte.Text;
            BindDSnOnOff();
            BindDHta();
            PerfilesGrid();
            MltVRte.ActiveViewIndex = 5;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BindDSnOnOff()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTRTE = (DataSet)ViewState["DSTRTE"];
                if (DSTRTE.Tables[7].Rows.Count > 0)
                { GrdSnOnOff.DataSource = DSTRTE.Tables[7]; GrdSnOnOff.DataBind(); }
                else
                {
                    DSTRTE.Tables[7].Rows.Add(DSTRTE.Tables[7].NewRow());
                    GrdSnOnOff.DataSource = DSTRTE.Tables[7];
                    GrdSnOnOff.DataBind();
                    GrdSnOnOff.Rows[0].Cells.Clear();
                    GrdSnOnOff.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdSnOnOff.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdSnOnOff.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarSnOnOff_Click(object sender, ImageClickEventArgs e)
        { MltVRte.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void DdlPNOn_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOn") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescElem = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtDescElem") as TextBox);
            TextBox TxtSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOn") as TextBox);
            string VbPn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("DdlPNOn") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOn.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElem.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOn.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals("")) { LtbSNOn.Visible = true; }
                }
            }
            TxtSNOn.Text = "";
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void LtbSNOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOn") as TextBox);
            ListBox LtbSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOn") as ListBox);
            string VbSn = LtbSNOn.SelectedValue.Trim();
            TxtSNOn.Text = VbSn;
            LtbSNOn.Visible = false;
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
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
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOff.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElem.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOff.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals("")) { LtbSNOff.Visible = true; }
                }
            }
            TxtSNOff.Text = "";
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
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
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
            Page.Title = ViewState["PageTit"].ToString().Trim();
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
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LtbSNOffPP.Items.Clear();
                while (Tbl.Read())
                {
                    TxtDescElemPP.Text = HttpUtility.HtmlDecode(Tbl["Descripcion"].ToString().Trim());
                    LtbSNOffPP.Items.Add(Tbl[0].ToString());
                    if (!Tbl["SN"].ToString().Trim().Equals("")) { LtbSNOffPP.Visible = true; }
                }
            }
            TxtSNOffPP.Text = "";
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
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
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
                TextBox TxtFecPP = (GrdSnOnOff.FooterRow.FindControl("TxtFecPP") as TextBox);
                TxtFecPP.Attributes.Add("onfocus", "this.select();");
                if (TxtFecPP.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha')", true);
                    TxtFecPP.Focus(); return;
                }

                Cnx.ValidarFechas(TxtFecPP.Text, "", 1);
                string Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecPP.Focus();
                    return;
                }

                Cnx.ValidarFechas(TxtFecDet.Text, TxtFecPP.Text, 2);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFecPP.Focus();
                    return;
                }

                DateTime? VbFe = Convert.ToDateTime(TxtFecPP.Text);
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
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Las series son iguales')", true);
                    return;
                }
                if (VbPnOn.Equals("") && VbPnOff.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens30'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una P/N ON o OFF')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,@TRazR,@Pos,@PnOn,@SnOn,@Des,@PnOff,@SnOff,'','','','','','','INSERT',@CodT,@Rte,@Cant,0,0,@ICC,@Fe,'02-01-1','03-01-1'");
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
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                                BindDSnOnOff();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdSnOnOff.EditIndex = e.NewEditIndex; ViewState["Index"] = e.NewEditIndex; BindDSnOnOff(); }
        protected void GrdSnOnOff_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdSnOnOff.DataKeys[Idx].Value.ToString());
            int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
            TextBox TxtFec = (GrdSnOnOff.Rows[Idx].FindControl("TxtFec") as TextBox);
            TxtFec.Attributes.Add("onfocus", "this.select();");
            if (TxtFec.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha')", true);               
                TxtFec.Focus(); return;
            }

            Cnx.ValidarFechas(TxtFec.Text, "", 1);
            string Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFec.Focus();
                return;
            }

            Cnx.ValidarFechas(TxtFecDet.Text, TxtFec.Text, 2);
            Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFec.Focus();
                return;
            }

            DateTime? VbFe = Convert.ToDateTime(TxtFec.Text);
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
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Las series son iguales')", true);
                return;
            }
            if (VbPnOn.Equals("") && VbPnOff.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens30'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una P/N ON o OFF')", true);

            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,@TRazR,@Pos,@PnOn,@SnOn,@Des,@PnOff,@SnOff,'','','','','','','UPDATE',@CodT,@Rte,@Cant,0,0,@ICC,@Fe,'02-01-1','03-01-1'");
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
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdSnOnOff.EditIndex = -1;
                            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                            BindDSnOnOff();
                            PerfilesGrid();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdSnOnOff.EditIndex = -1; BindDSnOnOff(); }
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
                    VBQuery = string.Format("EXEC SP_TablasManto 1,@Usu,'','','','','','','','','','','','','','DELETE',@CodT,@Rte,0,0,0,@ICC,'02-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                            BindDSnOnOff();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE SN ON OFF", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdSnOnOff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTRTE = (DataSet)ViewState["DSTRTE"];
            IEnumerable<DataRow> VbQry = from A in DSTRTE.Tables[3].AsEnumerable() where A.Field<string>("CodPn") != "- N -" select A;
            DataTable DT = VbQry.CopyToDataTable();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlRazonRPP = (e.Row.FindControl("DdlRazonRPP") as DropDownList);
                DdlRazonRPP.DataSource = DSTRTE.Tables[8];
                DdlRazonRPP.DataTextField = "Descripcion";
                DdlRazonRPP.DataValueField = "CodRemocion";
                DdlRazonRPP.DataBind();

                DropDownList DdlPosicPP = (e.Row.FindControl("DdlPosicPP") as DropDownList);
                DdlPosicPP.DataSource = DSTRTE.Tables[9];
                DdlPosicPP.DataTextField = "Descripcion";
                DdlPosicPP.DataValueField = "Codigo";
                DdlPosicPP.DataBind();

                DropDownList DdlPNOnPP = (e.Row.FindControl("DdlPNOnPP") as DropDownList);
                DdlPNOnPP.DataSource = DT;
                DdlPNOnPP.DataTextField = "PN";
                DdlPNOnPP.DataValueField = "CodPn";
                DdlPNOnPP.DataBind();

                DropDownList DdlPNOffPP = (e.Row.FindControl("DdlPNOffPP") as DropDownList);
                DdlPNOffPP.DataSource = DT;
                DdlPNOffPP.DataTextField = "PN";
                DdlPNOffPP.DataValueField = "CodPn";
                DdlPNOffPP.DataBind();

                TextBox TxtFecPP = (e.Row.FindControl("TxtFecPP") as TextBox);
                TxtFecPP.Text = TxtFecDet.Text;
                DateTime DiaI = Convert.ToDateTime(TxtFecDet.Text);

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
                string VbFecSt;
                DateTime? VbFecDT;

                DataRowView dr = e.Row.DataItem as DataRowView;
                DropDownList DdlRazonR = (e.Row.FindControl("DdlRazonR") as DropDownList);

                DdlRazonR.DataSource = DSTRTE.Tables[8];
                DdlRazonR.DataTextField = "Descripcion";
                DdlRazonR.DataValueField = "CodRemocion";
                DdlRazonR.DataBind();
                DdlRazonR.SelectedValue = dr["CodRazonR"].ToString().Trim();

                DataRowView DrP = e.Row.DataItem as DataRowView;
                DropDownList DdlPosic = (e.Row.FindControl("DdlPosic") as DropDownList);
                DdlPosic.DataSource = DSTRTE.Tables[9];
                DdlPosic.DataTextField = "Descripcion";
                DdlPosic.DataValueField = "Codigo";
                DdlPosic.DataBind();
                DdlPosic.SelectedValue = DrP["Posicion"].ToString().Trim();

                DataRowView DrPN = e.Row.DataItem as DataRowView;
                DropDownList DdlPNOn = (e.Row.FindControl("DdlPNOn") as DropDownList);
                DdlPNOn.DataSource = DT;
                DdlPNOn.DataTextField = "PN";
                DdlPNOn.DataValueField = "CodPn";
                DdlPNOn.DataBind();
                DdlPNOn.SelectedValue = DrPN["CodPnOn"].ToString().Trim();

                DataRowView DrPNOf = e.Row.DataItem as DataRowView;
                DropDownList DdlPNOff = (e.Row.FindControl("DdlPNOff") as DropDownList);
                DdlPNOff.DataSource = DT;
                DdlPNOff.DataTextField = "PN";
                DdlPNOff.DataValueField = "CodPn";
                DdlPNOff.DataBind();
                DdlPNOff.SelectedValue = DrPNOf["CodPnOff"].ToString().Trim();

                DateTime DiaI = Convert.ToDateTime(TxtFecDet.Text);

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }

                TextBox TxtFec = (e.Row.FindControl("TxtFec") as TextBox);
                VbFecSt = DrP["FechaDMA"].ToString().Trim();
                VbFecDT = Convert.ToDateTime(VbFecSt);
                TxtFec.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
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
        { GrdSnOnOff.PageIndex = e.NewPageIndex; BindDSnOnOff(); PerfilesGrid(); }
        //******************************************  herramientas *********************************************************
        protected void BindDHta()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTRTE = (DataSet)ViewState["DSTRTE"];
                if (DSTRTE.Tables[10].Rows.Count > 0)
                { GrdHta.DataSource = DSTRTE.Tables[10]; GrdHta.DataBind(); }
                else
                {
                    DSTRTE.Tables[10].Rows.Add(DSTRTE.Tables[10].NewRow());
                    GrdHta.DataSource = DSTRTE.Tables[10];
                    GrdHta.DataBind();
                    GrdHta.Rows[0].Cells.Clear();
                    GrdHta.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdHta.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdHta.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDHta", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','S',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','S',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                Cm.Parameters.AddWithValue("@P", VbPn);
                Cm.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                if (GrdHta.Rows.Count > 2)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens31'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Solo es posible ingresar 3 herramientas')", true);
                    return;
                }
                int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);
                TextBox TxtFechVcePP = (GrdHta.FooterRow.FindControl("TxtFechVcePP") as TextBox);
                TxtFechVcePP.Attributes.Add("onfocus", "this.select();");
                if (TxtFechVcePP.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens32'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//la fecha vencimiento se encuentra vacía')", true);
                    TxtFechVcePP.Focus(); return;
                }

                Cnx.ValidarFechas(TxtFechVcePP.Text, "", 1);
                string Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechVcePP.Focus();
                    return;
                }

                Cnx.ValidarFechas(TxtFecDet.Text, TxtFechVcePP.Text, 2);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechVcePP.Focus();
                    return;
                }

                DateTime? VbFe = Convert.ToDateTime(TxtFechVcePP.Text);
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
                    return;
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,@Pn,@Sn,@Des,'','','','','','','','','','','INSERT',@CodT,@Rte,0,0,0,@ICC,@Fe,'02-01-1','03-01-1'");
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
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
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
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Herramientas en Reportes", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdHta.EditIndex = e.NewEditIndex; ViewState["Index"] = e.NewEditIndex; BindDHta(); }
        protected void GrdHta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int Idx = e.RowIndex;
            int VblId = Convert.ToInt32(GrdHta.DataKeys[Idx].Value.ToString());
            int VbRte = Convert.ToInt32(TxtSnOnOffNumRte.Text);

            TextBox TxtFecVce = (GrdHta.Rows[Idx].FindControl("TxtFecVce") as TextBox);
            TxtFecVce.Attributes.Add("onfocus", "this.select();");
            if (TxtFecVce.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens32'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La fecha se encuetra vacía
                TxtFecVce.Focus(); return;
            }

            Cnx.ValidarFechas(TxtFecVce.Text, "", 1);
            string Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFecVce.Focus();
                return;
            }

            Cnx.ValidarFechas(TxtFecDet.Text, TxtFecVce.Text, 2);
            Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFecVce.Focus();
                return;
            }
            DateTime? VbFe = Convert.ToDateTime(TxtFecVce.Text);
            string VbPn = (GrdHta.Rows[Idx].FindControl("DdlPNHta") as DropDownList).Text.Trim();
            string VbSn = (GrdHta.Rows[Idx].FindControl("TxtSNHta") as TextBox).Text.Trim();
            string VbDes = (GrdHta.Rows[Idx].FindControl("TxtDescHta") as TextBox).Text.Trim();
            if (VbSn.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens34'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El campo S/N se encuentra vacío
                return;
            }
            if (VbPn.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens33'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,@Pn,@Sn,@Des,'','','','','','','','','','','UPDATE',@CodT,@Rte,0,0,0,@ICC,@Fe,'02-01-1','03-01-1'");
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
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdHta.EditIndex = -1;
                            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                            BindDHta();
                            PerfilesGrid();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Herramienta Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdHta.EditIndex = -1; BindDHta(); }
        protected void GrdHta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
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
                    VBQuery = string.Format("EXEC SP_TablasManto 2,@Usu,'','','','','','','','','','','','','','DELETE',@CodT,@Rte,0,0,0,@ICC,'02-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CodT", VblId);
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Rte", Convert.ToInt32(TxtSnOnOffNumRte.Text));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                            BindDHta();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Herramienta Rte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdHta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTRTE = (DataSet)ViewState["DSTRTE"];
            IEnumerable<DataRow> VbQry = from A in DSTRTE.Tables[3].AsEnumerable() where A.Field<string>("CodTipoElemento") == "03" select A;
            DataTable DT = VbQry.CopyToDataTable();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNHtaPP = (e.Row.FindControl("DdlPNHtaPP") as DropDownList);
                DdlPNHtaPP.DataSource = DT;
                DdlPNHtaPP.DataTextField = "PN";
                DdlPNHtaPP.DataValueField = "CodPN";
                DdlPNHtaPP.DataBind();

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
                DropDownList DdlPNHta = (e.Row.FindControl("DdlPNHta") as DropDownList);
                DdlPNHta.DataSource = DT;
                DdlPNHta.DataTextField = "PN";
                DdlPNHta.DataValueField = "CodPN";
                DdlPNHta.DataBind();
                DdlPNHta.SelectedValue = DrPN["PN"].ToString().Trim();

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }

                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }

                string VbFecSt;
                DateTime? VbFecDT;
                TextBox TxtFecVce = (e.Row.FindControl("TxtFecVce") as TextBox);
                VbFecSt = DrPN["FechaDMY"].ToString().Trim();
                VbFecDT = Convert.ToDateTime(VbFecSt);
                TxtFecVce.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
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
        { GrdHta.EditIndex = e.NewPageIndex; BindDHta(); PerfilesGrid(); }
        //******************************************  Procedimientos *********************************************************
        protected void Exportar(string Condcion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string StSql, VbNomRpt = "", VbOpcion = "";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                switch (Condcion)
                {
                    case "Reserva":
                        CursorIdioma.Alimentar("CURRESERVA", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto2 6,'CURRESERVA','','','','',@SubOT,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        VbNomRpt = BtnReserva.Text.Trim();
                        break;
                    case "ReporteGeneral":
                        CursorIdioma.Alimentar("CurInfomeRte", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto 4,'CurInfomeRte','','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        DataRow[] Result1 = Idioma.Select("Objeto= 'TitExpNomRte'");
                        foreach (DataRow row in Result1)
                        { VbNomRpt = row["Texto"].ToString().Trim(); }
                        break;
                    default:
                        if (TblBusqRte.Visible == true)
                        {
                            //busqueda Reporte
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
                        CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                        StSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,@CodlV,'','CurBusqRte',@Opc,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'");
                        DataRow[] Result = Idioma.Select("Objeto= 'TitExpBusqRte'");
                        foreach (DataRow row in Result)
                        { VbNomRpt = row["Texto"].ToString().Trim(); }
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
                        SC.Parameters.AddWithValue("@CodlV", TxtNumLv.Text.Trim());// solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@Idm", Session["!dC!@"]);
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}
