using _77NeoWeb.prg;
using _77NeoWeb.Prg;
using _77NeoWeb.Prg.PrgIngenieria;
using AjaxControlToolkit;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmOrdenTrabajo : System.Web.UI.Page
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
            Page.Title = string.Format("Orden de Trabajo");
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
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Orden de Trabajo";
                MlVwOT.ActiveViewIndex = 0;

                DdlLicInsp("", ""); /**/
                ViewState["EstadoOT"] = "";
                ViewState["Index"] = 0;
                ViewState["CodPrioridad"] = "NORMAL";
                ViewState["Ventana"] = 0;
                ViewState["VentanaRva"] = 0;
                ViewState["VentanaBusq"] = 0;
                ViewState["PasoActual"] = 0;
                ViewState["Accion"] = "";
                ViewState["Validar"] = "S";/**/
                ViewState["IdPasos"] = 0;
                ViewState["OrigRte"] = "PA"; // PA=Paso | OT= desde OT              
                ModSeguridad();
                ViewState["TllAnt"] = "";
                ViewState["BaseAnt"] = "";
                ViewState["InsptAnt"] = "";
                ViewState["CCstAnt"] = "";
                ViewState["InsptPsAnt"] = "";
                ViewState["LicInsptPsAnt"] = "";
                ViewState["TecPsAnt"] = "";
                ViewState["LicTecPsAnt"] = "";
                TraerDatosBusqOT(0, "UPD");
                BindDdlOTCondicional("");
                PerfilesGrid();
                CalPasoFechI.EndDate = DateTime.Now;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                GrdOTDetTec.ShowFooter = false;
                GrdOTRecursoF.ShowFooter = false;
                BtnIngresar.Visible = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnOtModificar.Visible = false;
                BtnPasoAceptar.Visible = false;
                BtnModificar.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
            }
            if (ClsP.GetImprimir() == 0)
            {
                //
            }
            if (ClsP.GetEliminar() == 0)
            { ViewState["VblEliMS"] = 0; BtnOTEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0)//
            { }
            if (ClsP.GetCE2() == 0)// CERRAR/CANCEL
            { ViewState["VblCE2"] = 0; }
            if (ClsP.GetCE3() == 0)//MOD DESCRIPCION    
            { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0)// BLQUEO RECURSO
            { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0)// Asignar Aeronave / Tiempos
            { ViewState["VblCE5"] = 0; }
            if (ClsP.GetCE6() == 0) // Activar opcion de ejeuctar pasos
            { ViewState["VblCE6"] = 0; }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                ViewState["Hab8Pasos"] = "N";
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,1,'MRO',1,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                SC.Parameters.AddWithValue("@F", "FRMORDENTRABAJO");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("S") && !Regs["Formulario"].ToString().Trim().Equals("FRMORDENTRABAJO"))
                    {
                        //Habilita los 8 pasos de MRO
                        ViewState["Hab8Pasos"] = "S";
                    }
                    if (VbCaso == 1 && VbAplica.Equals("S") && Regs["Formulario"].ToString().Trim().Equals("MRO"))
                    {
                        //Propiedades de  MRO
                        BtnOTAbiertas8PasCump.Visible = true;
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
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"].ToString().Trim());
                SC.Parameters.AddWithValue("@F2", "FrmReporte");
                SC.Parameters.AddWithValue("@F3", "0");
                SC.Parameters.AddWithValue("@F4", "0");
                sqlCon.Open();//https://localhost:44350/Forms/Ingenieria/FrmOrdenTrabajo.aspx.cs
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                    if (tbl["Objeto"].ToString().Trim().Equals("TituloOT"))
                    { Page.Title = tbl["Texto"].ToString().Trim(); ViewState["PageTit"] = tbl["Texto"].ToString().Trim(); }
                    TitForm.Text = tbl["Objeto"].ToString().Trim().Equals("CaptionOT") ? tbl["Texto"].ToString().Trim() : TitForm.Text;
                    LblTitoTGral.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitoTGral") ? tbl["Texto"].ToString().Trim() : LblTitoTGral.Text;
                    LblOt.Text = tbl["Objeto"].ToString().Trim().Equals("LblOt") ? tbl["Texto"].ToString().Trim() : LblOt.Text;
                    LblOtReporte.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtReporte") ? tbl["Texto"].ToString().Trim() : LblOtReporte.Text;
                    LblOtPrioridad.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtPrioridad") ? tbl["Texto"].ToString().Trim() : LblOtPrioridad.Text;
                    LblMroTaller.Text = tbl["Objeto"].ToString().Trim().Equals("LblMroTaller") ? tbl["Texto"].ToString().Trim() : LblMroTaller.Text;
                    LblMroPpt.Text = tbl["Objeto"].ToString().Trim().Equals("LblMroPpt") ? tbl["Texto"].ToString().Trim() : LblMroPpt.Text;
                    BtnMroInsPre.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroInsPre") ? tbl["Texto"].ToString().Trim() : BtnMroInsPre.Text;
                    BtnMroPrDes.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroPrDes") ? tbl["Texto"].ToString().Trim() : BtnMroPrDes.Text;
                    BtnMroRteDes.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroRteDes") ? tbl["Texto"].ToString().Trim() : BtnMroRteDes.Text;
                    BtnMroDanOc.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroDanOc") ? tbl["Texto"].ToString().Trim() : BtnMroDanOc.Text;
                    BtnMroAccCorr.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroAccCorr") ? tbl["Texto"].ToString().Trim() : BtnMroAccCorr.Text;
                    BtnMroPrueF.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroPrueF") ? tbl["Texto"].ToString().Trim() : BtnMroPrueF.Text;
                    BtnMroCumpl.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroCumpl") ? tbl["Texto"].ToString().Trim() : BtnMroCumpl.Text;
                    BtnMroTrabEje.Text = tbl["Objeto"].ToString().Trim().Equals("BtnMroTrabEje") ? tbl["Texto"].ToString().Trim() : BtnMroTrabEje.Text;
                    LblTitDatosGener.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitDatosGener") ? tbl["Texto"].ToString().Trim() : LblTitDatosGener.Text;
                    LblBusqOT.Text = tbl["Objeto"].ToString().Trim().Equals("LblBusqOT") ? tbl["Texto"].ToString().Trim() : LblBusqOT.Text;
                    LblOtRepacion.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtRepacion") ? tbl["Texto"].ToString().Trim() : LblOtRepacion.Text;
                    LblOtPpal.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtPpal") ? tbl["Texto"].ToString().Trim() : LblOtPpal.Text;
                    LblOtWS.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtWS") ? tbl["Texto"].ToString().Trim() : LblOtWS.Text;
                    LblTitCrearEDatosE.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitCrearEDatosE") ? tbl["Texto"].ToString().Trim() : LblTitCrearEDatosE.Text;
                    CkbEjePasos.Text = tbl["Objeto"].ToString().Trim().Equals("CkbEjePasos") ? tbl["Texto"].ToString().Trim() : CkbEjePasos.Text;
                    LblMroCliente.Text = tbl["Objeto"].ToString().Trim().Equals("LblMroCliente") ? tbl["Texto"].ToString().Trim() : LblMroCliente.Text;
                    BtnOTAbiertas8PasCump.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTAbiertas8PasCump") ? tbl["Texto"].ToString().Trim() : BtnOTAbiertas8PasCump.Text;
                    BtnOTAbiertas8PasCump.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnOTAbiertas8PasCumpToolTip") ? tbl["Texto"].ToString().Trim() : BtnOTAbiertas8PasCump.ToolTip;
                    BtnOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTDetTec") ? tbl["Texto"].ToString().Trim() : BtnOTDetTec.Text;
                    BtnOTEliminar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTEliminar") ? tbl["Texto"].ToString().Trim() : BtnOTEliminar.Text;
                    BtNOTExportar.Text = tbl["Objeto"].ToString().Trim().Equals("BtNOTExportar") ? tbl["Texto"].ToString().Trim() : BtNOTExportar.Text;
                    BtnOTImprimir.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTImprimir") ? tbl["Texto"].ToString().Trim() : BtnOTImprimir.Text;
                    BtnOtModificar.Text = tbl["Objeto"].ToString().Trim().Equals("BotonMod") ? tbl["Texto"].ToString().Trim() : BtnOtModificar.Text;
                    BtnOTReserva.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTReserva") ? tbl["Texto"].ToString().Trim() : BtnOTReserva.Text;
                    BtnOTConsultar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnConsultar") ? tbl["Texto"].ToString().Trim() : BtnOTConsultar.Text;
                    CkbCancel.Text = tbl["Objeto"].ToString().Trim().Equals("CkbCancel") ? tbl["Texto"].ToString().Trim() : CkbCancel.Text;
                    CkbOtBloqDet.Text = tbl["Objeto"].ToString().Trim().Equals("CkbOtBloqDet") ? tbl["Texto"].ToString().Trim() : CkbOtBloqDet.Text;
                    LblAplicab.Text = tbl["Objeto"].ToString().Trim().Equals("LblAplicab") ? tbl["Texto"].ToString().Trim() : LblAplicab.Text;
                    LblOTAccParc.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTAccParc") ? tbl["Texto"].ToString().Trim() : LblOTAccParc.Text;
                    LblOTAero.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTAero") ? tbl["Texto"].ToString().Trim() : LblOTAero.Text;
                    LblOtEstado.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtEstado") ? tbl["Texto"].ToString().Trim() : LblOtEstado.Text;
                    LblOtEstaSec.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtEstaSec") ? tbl["Texto"].ToString().Trim() : LblOtEstaSec.Text;
                    LblOTFechini.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTFechini") ? tbl["Texto"].ToString().Trim() : LblOTFechini.Text;
                    LblOTFechReg.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTFechReg") ? tbl["Texto"].ToString().Trim() : LblOTFechReg.Text;
                    LblOTFechVenc.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTFechVenc") ? tbl["Texto"].ToString().Trim() : LblOTFechVenc.Text;
                    lblOtRespons.Text = tbl["Objeto"].ToString().Trim().Equals("lblOtRespons") ? tbl["Texto"].ToString().Trim() : lblOtRespons.Text;
                    LblOTTrabajo.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTTrabajo") ? tbl["Texto"].ToString().Trim() : LblOTTrabajo.Text;
                    LblTitOtTiempo.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitOtTiempo") ? tbl["Texto"].ToString().Trim() : LblTitOtTiempo.Text;
                    LblOTFechFin.Text = tbl["Objeto"].ToString().Trim().Equals("LblOTFechFin") ? tbl["Texto"].ToString().Trim() : LblOTFechFin.Text;
                    lblOtLicInsp.Text = tbl["Objeto"].ToString().Trim().Equals("lblOtLicInsp") ? tbl["Texto"].ToString().Trim() : lblOtLicInsp.Text;
                    BtnOTReporte.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTReporte") ? tbl["Texto"].ToString().Trim() : BtnOTReporte.Text;
                    LblTitOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitOTDetTec") ? tbl["Texto"].ToString().Trim() : LblTitOTDetTec.Text;
                    IbtCerrarOTDetTec.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarOTDetTec.ToolTip;
                    LblBusqOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("Busqueda") ? tbl["Texto"].ToString().Trim() : LblBusqOTDetTec.Text;
                    if (tbl["Objeto"].ToString().Trim().Equals("placeholder"))
                    {
                        TxtConsulOTDetTec.Attributes.Add("placeholder", tbl["Texto"].ToString().Trim());
                        TxtOTRecurConsulPn.Attributes.Add("placeholder", tbl["Texto"].ToString().Trim());
                        TxtOTBusq.Attributes.Add("placeholder", tbl["Texto"].ToString().Trim());
                    }
                    IbtConsOTDetTec.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnOTConsultar") ? tbl["Texto"].ToString().Trim() : IbtConsOTDetTec.ToolTip;
                    GrdOTDetTec.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Fecha") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[0].HeaderText;
                    GrdOTDetTec.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Técnico") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[1].HeaderText;
                    GrdOTDetTec.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Licencia") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[2].HeaderText;
                    GrdOTDetTec.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("TotalHora") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[3].HeaderText;
                    GrdOTDetTec.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("DatoPasos") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[4].HeaderText;
                    //****************************************  Recuso Fisico --------------------
                    LblRecFRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblRecFRte.Text;
                    LblRecFSubOt.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblRecFSubOt.Text;
                    LblPrioridadOT.Text = tbl["Objeto"].ToString().Trim().Equals("LblPrioridadOT") ? tbl["Texto"].ToString().Trim() : LblPrioridadOT.Text;
                    GrdOTRecursoF.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripción") ? tbl["Texto"].ToString().Trim() : GrdOTRecursoF.Columns[2].HeaderText;
                    GrdOTRecursoF.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Cantidad") ? tbl["Texto"].ToString().Trim() : GrdOTRecursoF.Columns[3].HeaderText;
                    GrdOTRecursoF.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndMed") ? tbl["Texto"].ToString().Trim() : GrdOTRecursoF.Columns[4].HeaderText;
                    GrdOTRecursoF.Columns[5].HeaderText = tbl["Objeto"].ToString().Trim().Equals("CantEntreg") ? tbl["Texto"].ToString().Trim() : GrdOTRecursoF.Columns[5].HeaderText;
                    LblTtlOTRecur.Text = tbl["Objeto"].ToString().Trim().Equals("LblTtlRecursoRte") ? tbl["Texto"].ToString().Trim() : LblTtlOTRecur.Text;
                    IbtOTCerrarRecur.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtOTCerrarRecur.ToolTip;
                    LblOtRecurBusq.Text = tbl["Objeto"].ToString().Trim().Equals("Busqueda") ? tbl["Texto"].ToString().Trim() : LblOtRecurBusq.Text;
                    IbtOTRecurConsulPn.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnConsultar") ? tbl["Texto"].ToString().Trim() : IbtOTRecurConsulPn.ToolTip;
                    IbtOTRecurExpExcelPn.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtRecurExpExcelPn") ? tbl["Texto"].ToString().Trim() : IbtOTRecurExpExcelPn.ToolTip;
                    BtnOTCargaMasiva.Text = tbl["Objeto"].ToString().Trim().Equals("BtnCargaMasivaTT1") ? tbl["Texto"].ToString().Trim() : BtnOTCargaMasiva.Text;
                    BtnOTRecurNotif.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTRecurNotif") ? tbl["Texto"].ToString().Trim() : BtnOTRecurNotif.Text;
                    LblTitOTCargMasiv.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitCargMasiv") ? tbl["Texto"].ToString().Trim() : LblTitOTCargMasiv.Text;
                    LblCargaMasRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblNroRte") ? tbl["Texto"].ToString().Trim() : LblCargaMasRte.Text;
                    LblCargaMasOt.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblCargaMasOt.Text;
                    IbtOTCerrarCargMaxivo.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtOTCerrarCargMaxivo.ToolTip;
                    IbtOTSubirCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtSubirCargaMax") ? tbl["Texto"].ToString().Trim() : IbtOTSubirCargaMax.ToolTip;
                    IbtOTGuardarCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtGuardarCargaMax") ? tbl["Texto"].ToString().Trim() : IbtOTGuardarCargaMax.ToolTip;
                    GrdOTCargaMax.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdOTCargaMax.Columns[2].HeaderText;
                    GrdOTCargaMax.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Cantidad") ? tbl["Texto"].ToString().Trim() : GrdOTCargaMax.Columns[3].HeaderText;
                    GrdOTCargaMax.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndMed") ? tbl["Texto"].ToString().Trim() : GrdOTCargaMax.Columns[4].HeaderText;
                    GrdOTCargaMax.Columns[5].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndSistem") ? tbl["Texto"].ToString().Trim() : GrdOTCargaMax.Columns[5].HeaderText;
                    LblTitLicencia.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitLicencia") ? tbl["Texto"].ToString().Trim() : LblTitLicencia.Text;
                    GrdLicen.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Licencia") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[0].HeaderText;
                    GrdLicen.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[1].HeaderText;
                    GrdLicen.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("TiempoEstimado") ? tbl["Texto"].ToString().Trim() : GrdLicen.Columns[2].HeaderText;
                    //****************************************  Busqueda ****************************************
                    LblTitOTOpcBusqueda.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitOTOpcBusqueda") ? tbl["Texto"].ToString().Trim() : LblTitOTOpcBusqueda.Text;
                    RdbOTBusqNumOT.Text = tbl["Objeto"].ToString().Trim().Equals("RdbOTBusqNumOT") ? tbl["Texto"].ToString().Trim() : RdbOTBusqNumOT.Text;
                    RdbOTBusqHK.Text = tbl["Objeto"].ToString().Trim().Equals("RdbOTBusqHK") ? tbl["Texto"].ToString().Trim() : RdbOTBusqHK.Text;
                    RdbBusqRteNum.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqRteNum") ? tbl["Texto"].ToString().Trim() : RdbBusqRteNum.Text;
                    RdbBusqRteHk.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqRteHk") ? tbl["Texto"].ToString().Trim() : RdbBusqRteHk.Text;
                    RdbBusqRteAta.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqRteAta") ? tbl["Texto"].ToString().Trim() : RdbBusqRteAta.Text;
                    RdbBusqRteTecn.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqRteTecn") ? tbl["Texto"].ToString().Trim() : RdbBusqRteTecn.Text;
                    RdbBusqRteDescRte.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqRteDescRte") ? tbl["Texto"].ToString().Trim() : RdbBusqRteDescRte.Text;
                    IbtOTConsultarBusq.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnConsultar") ? tbl["Texto"].ToString().Trim() : IbtOTConsultarBusq.ToolTip;
                    IbtOTCerrarBusq.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtOTCerrarBusq.ToolTip;
                    IbtOTExpBusqOT.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtOTExpBusqOT") ? tbl["Texto"].ToString().Trim() : IbtOTExpBusqOT.ToolTip;
                    GrdOTBusq.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Selecc") ? tbl["Texto"].ToString().Trim() : GrdOTBusq.Columns[0].HeaderText;
                    GrdOTBusq.EmptyDataText = tbl["Objeto"].ToString().Trim().Equals("SinRegistros") ? tbl["Texto"].ToString().Trim() : GrdOTBusq.EmptyDataText;
                    //****************************************  Pasos ****************************************
                    IbtCerrarPasos.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarPasos.ToolTip;
                    LblPasoEsta.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoEsta") ? tbl["Texto"].ToString().Trim() : LblPasoEsta.Text;
                    LblPasoAplic.Text = tbl["Objeto"].ToString().Trim().Equals("LblAplicab") ? tbl["Texto"].ToString().Trim() : LblPasoAplic.Text;
                    CkbPasoOtro.Text = tbl["Objeto"].ToString().Trim().Equals("RdbPasoOTHER") ? tbl["Texto"].ToString().Trim() : CkbPasoOtro.Text;
                    LblPaosoRealizado.Text = tbl["Objeto"].ToString().Trim().Equals("LblPaosoRealizado") ? tbl["Texto"].ToString().Trim() : LblPaosoRealizado.Text;
                    LblPasoRef.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoRef") ? tbl["Texto"].ToString().Trim() : LblPasoRef.Text;
                    LblPasoDiscrep.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoDiscrep") ? tbl["Texto"].ToString().Trim() : LblPasoDiscrep.Text;
                    LblPasoFecI.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoFecI") ? tbl["Texto"].ToString().Trim() : LblPasoFecI.Text;
                    LblPasoFecF.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoFecF") ? tbl["Texto"].ToString().Trim() : LblPasoFecF.Text;
                    LblPasoTec.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoTec") ? tbl["Texto"].ToString().Trim() : LblPasoTec.Text;
                    LblPasoLicTec.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoLicTec") ? tbl["Texto"].ToString().Trim() : LblPasoLicTec.Text;
                    LblPasoHRealTec.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoHRealTec") ? tbl["Texto"].ToString().Trim() : LblPasoHRealTec.Text;
                    LblPasoInsp.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoInsp") ? tbl["Texto"].ToString().Trim() : LblPasoInsp.Text;
                    LblPasoLicInsp.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoLicTec") ? tbl["Texto"].ToString().Trim() : LblPasoLicInsp.Text;
                    LblPasoHRealInsp.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoHRealTec") ? tbl["Texto"].ToString().Trim() : LblPasoHRealInsp.Text;
                    LblPasoNotas.Text = tbl["Objeto"].ToString().Trim().Equals("LblPasoNotas") ? tbl["Texto"].ToString().Trim() : LblPasoNotas.Text;
                    BtnPasoAceptar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnPasoAceptar") ? tbl["Texto"].ToString().Trim() : BtnPasoAceptar.Text;
                    BtnPasoRepte.Text = tbl["Objeto"].ToString().Trim().Equals("BtnPasoRepte") ? tbl["Texto"].ToString().Trim() : BtnPasoRepte.Text;

                    //****************************************** Pasos cerrados ot abiertas *********************************************************
                    LblTit8PasoOpen.Text = tbl["Objeto"].ToString().Trim().Equals("LblTit8PasoOpen") ? tbl["Texto"].ToString().Trim() : LblTit8PasoOpen.Text;
                    IbtCerrarOT8PasoClose.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarOT8PasoClose.ToolTip;
                    IbtExportarOT8PasoClose.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtOTExpBusqOT") ? tbl["Texto"].ToString().Trim() : IbtExportarOT8PasoClose.ToolTip;
                    Grd8PasoCOTOpen.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Selecc") ? tbl["Texto"].ToString().Trim() : Grd8PasoCOTOpen.Columns[0].HeaderText;
                    Grd8PasoCOTOpen.EmptyDataText = tbl["Objeto"].ToString().Trim().Equals("SinRegistros") ? tbl["Texto"].ToString().Trim() : Grd8PasoCOTOpen.EmptyDataText;
                    //****************************************** Impresión *********************************************************
                    LblTitOTImpresion.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitOTImpresion") ? tbl["Texto"].ToString().Trim() : LblTitOTImpresion.Text;
                    IbtOTCerrarPrint.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtOTCerrarPrint.ToolTip;

                    //****************************************** Reporte *********************************************************
                    IbtCerrarRte.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarRte.ToolTip;
                    LblAeroRte.Text = tbl["Objeto"].ToString().Trim().Equals("LblAeroRte") ? tbl["Texto"].ToString().Trim() : LblAeroRte.Text;
                    LblOtSec.Text = tbl["Objeto"].ToString().Trim().Equals("LblOtSec") ? tbl["Texto"].ToString().Trim() : LblOtSec.Text;
                    LblRteBusq.Text = tbl["Objeto"].ToString().Trim().Equals("LblRteBusq") ? tbl["Texto"].ToString().Trim() : LblRteBusq.Text;
                    LblRteNumPaso.Text = tbl["Objeto"].ToString().Trim().Equals("LblRteNumPaso") ? tbl["Texto"].ToString().Trim() : LblRteNumPaso.Text;
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
                    //****************************************************************** Impresion Reporte ************************************************************
                    LblTitImpresion.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitImpresion") ? tbl["Texto"].ToString().Trim() : LblTitImpresion.Text;
                    IbtCerrarImpresion.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarImpresion.ToolTip;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'GuardarCargaMaxClientClick'");
                foreach (DataRow row in Result)
                { IbtOTGuardarCargaMax.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnOTEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdOTDetTec.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton IbtEdit = Row.FindControl("IbtEdit") as ImageButton;
                    if (IbtEdit != null)
                    { Row.Cells[5].Controls.Remove(IbtEdit); }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton IbtDelete = Row.FindControl("IbtDelete") as ImageButton;
                    if (IbtDelete != null)
                    {
                        Row.Cells[5].Controls.Remove(IbtDelete);
                    }
                }
            }
            foreach (GridViewRow Row in GrdOTRecursoF.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[7].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[7].Controls.Remove(imgD);
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
        }
        //******************************************  MRO *********************************************************        
        protected void BtnMroInsPre_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                ViewState["PasoActual"] = 1;
                DdlEstadoPaso(ViewState["EP1"].ToString().Equals("") ? "01" : ViewState["EP1"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroInsPre.Text.Trim();
                TraerDatosPasos(ViewState["EP1"].ToString().Equals("") ? "01" : ViewState["EP1"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroPrDes_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP1"].ToString().Equals("") ? "01" : ViewState["EP1"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 2;
                DdlEstadoPaso(ViewState["EP2"].ToString().Equals("") ? "01" : ViewState["EP2"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroPrDes.Text.Trim();
                TraerDatosPasos(ViewState["EP2"].ToString().Equals("") ? "01" : ViewState["EP2"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroRteDes_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP2"].ToString().Equals("") ? "01" : ViewState["EP2"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 3;
                DdlEstadoPaso(ViewState["EP3"].ToString().Equals("") ? "01" : ViewState["EP3"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroRteDes.Text.Trim();
                TraerDatosPasos(ViewState["EP3"].ToString().Equals("") ? "01" : ViewState["EP3"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroDanOc_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP3"].ToString().Equals("") ? "01" : ViewState["EP3"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 4;
                DdlEstadoPaso(ViewState["EP4"].ToString().Equals("") ? "01" : ViewState["EP4"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroDanOc.Text.Trim();
                TraerDatosPasos(ViewState["EP4"].ToString().Equals("") ? "01" : ViewState["EP4"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroAccCorr_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP4"].ToString().Equals("") ? "01" : ViewState["EP4"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 5;
                DdlEstadoPaso(ViewState["EP5"].ToString().Equals("") ? "01" : ViewState["EP5"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroAccCorr.Text.Trim();
                TraerDatosPasos(ViewState["EP5"].ToString().Equals("") ? "01" : ViewState["EP5"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroPrueF_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP5"].ToString().Equals("") ? "01" : ViewState["EP5"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 6;
                DdlEstadoPaso(ViewState["EP6"].ToString().Equals("") ? "01" : ViewState["EP6"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroPrueF.Text.Trim();
                TraerDatosPasos(ViewState["EP6"].ToString().Equals("") ? "01" : ViewState["EP6"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroCumpl_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP6"].ToString().Equals("") ? "01" : ViewState["EP5"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 7;
                DdlEstadoPaso(ViewState["EP7"].ToString().Equals("") ? "01" : ViewState["EP7"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroCumpl.Text.Trim();
                TraerDatosPasos(ViewState["EP7"].ToString().Equals("") ? "01" : ViewState["EP7"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        protected void BtnMroTrabEje_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Convert.ToInt32(ViewState["EP7"].ToString().Equals("") ? "01" : ViewState["EP7"].ToString()) < 4)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens29'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
                ViewState["PasoActual"] = 8;
                DdlEstadoPaso(ViewState["EP8"].ToString().Equals("") ? "01" : ViewState["EP8"].ToString());
                LblTitPasos.Text = ViewState["PasoActual"].ToString() + " - " + BtnMroTrabEje.Text.Trim();
                TraerDatosPasos(ViewState["EP8"].ToString().Equals("") ? "01" : ViewState["EP8"].ToString());
                MlVwOT.ActiveViewIndex = 6;
            }
        }
        //******************************************  O.T. *********************************************************
        protected void ActivarBtnOT(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnOtModificar.Enabled = Md;
            BtnOTDetTec.Enabled = Otr;
            BtnOTReserva.Enabled = Otr;
            BtnOTConsultar.Enabled = Otr;
            BtnOTImprimir.Enabled = Ip;
            BtnOTEliminar.Enabled = El;
            BtnOTReporte.Enabled = Otr;
            BtnOTAbiertas8PasCump.Enabled = Otr;
            BtNOTExportar.Enabled = Otr;
            BtnMroInsPre.Enabled = Otr;
            BtnMroAccCorr.Enabled = Otr;
            BtnMroCumpl.Enabled = Otr;
            BtnMroDanOc.Enabled = Otr;
            BtnMroPrDes.Enabled = Otr;
            BtnMroPrueF.Enabled = Otr;
            BtnMroRteDes.Enabled = Otr;
            BtnMroTrabEje.Enabled = Otr;
        }
        protected void ValidarOT(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                if (DdlOtEstado.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens42OT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//El estado es requerido.
                    ViewState["Validar"] = "N"; return;
                }
                if (TxtOTFechFin.Text.Trim().Equals("") && DdlOtEstado.Text.Equals("0002"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens43OT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar al menos un técnico con su respectiva fecha de trabajo.
                    ViewState["Validar"] = "N"; return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarOT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void LimpiarCamposOT()
        {
            DdlBusqOT.Text = "0";
            TxtOt.Text = "";
            TxtOtPpal.Text = "";
            TxtOtReporte.Text = "";
            TxtOtRepacion.Text = "";
            TxtlOtPrioridad.Text = "";
            TxtOtWS.Text = "";
            CkbEjePasos.Checked = false;
            TxtMroPpt.Text = "";
            TxtMroCliente.Text = "";
            DdlMroTaller.Text = "";
            TxtAplicab.Text = "";
            TxtOtPN.Text = "";
            DdlOTBase.Text = "";
            DdlOTAero.Text = "0";
            DdlOtEstado.Text = "";
            DdlOtEstaSec.Text = "";
            TxtOTFechReg.Text = "";
            TxtOTFechini.Text = "";
            TxtOTFechFin.Text = "";
            TxtOTFechVenc.Text = "";
            DdlOtInsp.Text = "";
            DdlOtLicInsp.Text = "";
            DdlOtRespons.Text = "";
            CkbCancel.Checked = false;
            CkbOtBloqDet.Checked = false;
            TxtTSN.Text = "";
            TxtTSO.Text = "";
            TxtTSR.Text = "";
            TxtCSN.Text = "";
            TxtCSO.Text = "";
            TxtCSR.Text = "";
            UplDatosPpal.Update();
        }
        protected void ActivarCampOT(bool Ing, bool Edi, string accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (Edi == true)
            {
                if (DdlOtEstado.SelectedValue.Equals("0002"))
                {

                    if (Convert.ToInt32(ViewState["VblCE4"]) == 1)// BLQUEO RECURSO
                    {
                        if (CkbCancel.Checked == false)
                        {
                            if (DdlOtEstaSec.SelectedValue.Equals("0001"))
                            { CkbOtBloqDet.Enabled = Edi; }
                        }
                    }
                }
                else
                {
                    DdlMroTaller.Enabled = Edi;
                    DdlOtCCosto.Enabled = Edi;
                    if (Convert.ToInt32(ViewState["VblCE5"]) == 1)// Asignar Aeronave / Tiempos
                    { DdlOTAero.Enabled = Edi; TxtTSN.Enabled = Edi; TxtTSO.Enabled = Edi; TxtTSR.Enabled = Edi; TxtCSN.Enabled = Edi; TxtCSO.Enabled = Edi; TxtCSR.Enabled = Edi; }
                    DdlTall.Enabled = Edi;
                    if (Convert.ToInt32(ViewState["VblCE2"]) == 1)/// CERRAR/CANCEL
                    {
                        if (TxtOtReporte.Text.Equals("0"))
                        { DdlOtEstado.Enabled = Edi; CkbCancel.Enabled = Edi; }
                        else
                        {
                            if (Edi == true)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'Mens50OT'");
                                foreach (DataRow row in Result)
                                { DdlOtEstado.ToolTip = row["Texto"].ToString().Trim(); }
                            }
                            else { DdlOtEstado.ToolTip = ""; }
                        }
                    }
                    IbtOTFechVenc.Enabled = Edi;
                    DdlOtInsp.Enabled = Edi;
                    DdlOtLicInsp.Enabled = Edi;
                    DdlOtRespons.Enabled = Edi;
                    DdlOTBase.Enabled = Edi;
                    if (Convert.ToInt32(ViewState["VblCE6"]) == 1)// // Activar opcion de ejeuctar pasos
                    { TxtOTTrabajo.Enabled = Edi; }
                    TxtOTAccParc.Enabled = Edi;
                }
            }
            else
            {
                DdlOtEstado.Enabled = false;
                DdlOtEstado.ToolTip = "";
                CkbOtBloqDet.Enabled = false;
                DdlMroTaller.Enabled = false;
                DdlOtCCosto.Enabled = false;
                DdlOTAero.Enabled = Edi;
                if (Convert.ToInt32(ViewState["VblCE5"]) == 1)// // Asignar Aeronave / Tiempos
                { TxtTSN.Enabled = Edi; TxtTSO.Enabled = Edi; TxtTSR.Enabled = Edi; TxtCSN.Enabled = Edi; TxtCSO.Enabled = Edi; TxtCSR.Enabled = Edi; }
                IbtOTFechVenc.Enabled = Edi;
                DdlOtInsp.Enabled = Edi;
                DdlOtLicInsp.Enabled = Edi;
                DdlOtRespons.Enabled = Edi;
                DdlOTBase.Enabled = Edi;
                TxtOTTrabajo.Enabled = Edi;
                TxtOTAccParc.Enabled = Edi;
            }
        }
        protected void TraerDatosBusqOT(int NumOT, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Accion"] = "";
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        string LtxtSql = "EXEC SP_PANTALLA_OrdenTrabajo2 4,@U,'','','','',@Nr,0,@Idm,@ICC,'01-01-01','01-01-01','01-01-01'";
                        using (SqlCommand SC = new SqlCommand(LtxtSql, Cnx2))
                        {
                            SC.Parameters.AddWithValue("@U", Session["C77U"]);
                            SC.Parameters.AddWithValue("@Nr", NumOT);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTOTGrl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTOTGrl);
                                    DSTOTGrl.Tables[0].TableName = "DatosOT";
                                    DSTOTGrl.Tables[1].TableName = "BusqOT";
                                    DSTOTGrl.Tables[2].TableName = "HK";
                                    DSTOTGrl.Tables[3].TableName = "StdPpl";
                                    DSTOTGrl.Tables[4].TableName = "StdScdr";
                                    DSTOTGrl.Tables[5].TableName = "Tllr";
                                    DSTOTGrl.Tables[6].TableName = "Base";
                                    DSTOTGrl.Tables[7].TableName = "Persona";
                                    DSTOTGrl.Tables[8].TableName = "CCsto";
                                    DSTOTGrl.Tables[9].TableName = "DetTecnico";
                                    DSTOTGrl.Tables[10].TableName = "Licencias";
                                    DSTOTGrl.Tables[11].TableName = "RecursoOT";
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
                }
                DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];

                DdlBusqOT.DataSource = DSTOTGrl.Tables[1];
                DdlBusqOT.DataTextField = "Descripcion";
                DdlBusqOT.DataValueField = "Codigo";
                DdlBusqOT.DataBind();

                DdlOTAero.DataSource = DSTOTGrl.Tables[2];
                DdlOTAero.DataTextField = "Matricula";
                DdlOTAero.DataValueField = "CodAeronave";
                DdlOTAero.DataBind();
                DdlAeroRte.DataSource = DSTOTGrl.Tables[2];
                DdlAeroRte.DataTextField = "Matricula";
                DdlAeroRte.DataValueField = "CodAeronave";
                DdlAeroRte.DataBind();

                DdlOtEstado.DataSource = DSTOTGrl.Tables[3];
                DdlOtEstado.DataTextField = "Descripcion";
                DdlOtEstado.DataValueField = "Codigo";
                DdlOtEstado.DataBind();

                DdlOtEstaSec.DataSource = DSTOTGrl.Tables[4];
                DdlOtEstaSec.DataTextField = "Descripcion";
                DdlOtEstaSec.DataValueField = "Codigo";
                DdlOtEstaSec.DataBind();

                if (DSTOTGrl.Tables[0].Rows.Count > 0)
                {
                    TxtOt.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodNumOrdenTrab"].ToString().Trim());
                    TxtOtPpal.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OTMaster"].ToString().Trim());
                    TxtOtReporte.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodIdLvDetManto"].ToString().Trim());
                    if (Convert.ToInt32(TxtOtReporte.Text) > 0)
                    {
                        BtnOTReserva.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens19'");
                        foreach (DataRow row in Result)
                        { BtnOTReserva.ToolTip = row["Texto"].ToString(); } // La reserva se debe realizar desde la pantalla reporte";                        
                    }
                    else
                    {
                        BtnOTReserva.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'MensOT63'");

                        foreach (DataRow row in Result)
                        {
                            string b1 = row["Texto"].ToString();
                            BtnOTReserva.ToolTip = row["Texto"].ToString();
                        } //
                    }
                    if (Convert.ToInt32(TxtOtReporte.Text) > 0 || !TxtOtRepacion.Text.Equals(""))
                    {
                        BtnOTReporte.Enabled = false; ;
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens20'");
                        foreach (DataRow row in Result)
                        { BtnOTReporte.ToolTip = row["Texto"].ToString(); }  //"El reporte solo es posible para las Ordenes de trabajo master";
                    }
                    else { BtnOTReporte.Enabled = true; ; BtnOTReporte.ToolTip = ""; }
                    TxtOtRepacion.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodReparacion"].ToString().Trim());
                    TxtlOtPrioridad.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodPrioridad"].ToString().Trim());
                    TxtOtWS.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["WS"].ToString().Trim());
                    UplDatosPpal.Update();
                    TxtMroPpt.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["PPT"].ToString().Trim());
                    TxtMroCliente.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["ClientePPT"].ToString().Trim());
                    CkbEjePasos.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["EjecPasos"].ToString().Trim()));
                    if (TxtOtPpal.Text.Equals("0") && TxtOtReporte.Text.Equals("0") && TxtOtRepacion.Text.Equals(""))
                    { VisibleBotMRO(true); }
                    else
                    { VisibleBotMRO(false); }
                    ViewState["TllAnt"] = DSTOTGrl.Tables[0].Rows[0]["CodTaller"].ToString().Trim();
                    ViewState["BaseAnt"] = DSTOTGrl.Tables[0].Rows[0]["CodBase"].ToString().Trim();
                    ViewState["InsptAnt"] = DSTOTGrl.Tables[0].Rows[0]["CodInspectorCierre"].ToString().Trim();
                    string VbLInsp = DSTOTGrl.Tables[0].Rows[0]["LicenciaInspCierre"].ToString().Trim();
                    DdlLicInsp(ViewState["InsptAnt"].ToString().Trim(), VbLInsp);
                    string VbResp = DSTOTGrl.Tables[0].Rows[0]["CodResponsable"].ToString().Trim();
                    ViewState["CCstAnt"] = DSTOTGrl.Tables[0].Rows[0]["CentroCosto"].ToString().Trim();
                    BindDdlOTCondicional(VbResp);

                    DdlOtLicInsp.Text = VbLInsp;
                    DdlOtRespons.Text = VbResp;
                    TxtAplicab.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["Aplicabilidad"].ToString().Trim());
                    TxtOtPN.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["PNOT"].ToString().Trim());
                    DdlOTAero.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodAeronave"].ToString().Trim());
                    DdlOtEstado.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodEstOrdTrab1"].ToString().Trim());
                    ViewState["EstadoOT"] = DdlOtEstado.Text.Trim();
                    DdlOtEstaSec.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CodEstOrdTrab2"].ToString().Trim());
                    if (ViewState["EstadoOT"].Equals("0001"))
                    { BtnOtModificar.Enabled = true; BtnOTEliminar.Enabled = true; }
                    else
                    { BtnOTEliminar.Enabled = false; }
                    TxtOTFechReg.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["FechaReg"].ToString().Trim());
                    CalPasoFechI.StartDate = Convert.ToDateTime(TxtOTFechReg.Text);
                    TxtOTFechini.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["FechaIni"].ToString().Trim());
                    TxtOTFechFin.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["FechaFin"].ToString().Trim());
                    TxtOTFechVenc.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["FechaVence"].ToString().Trim());
                    CkbCancel.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["CancelOT"].ToString().Trim()));
                    CkbOtBloqDet.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["BloquearDetalle"].ToString().Trim()));
                    ViewState["OTBloquearDetalle"] = Convert.ToBoolean(HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["BloquearDetalle"].ToString().Trim()));
                    TxtOTTrabajo.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["TrabajoReq"].ToString().Trim());
                    TxtOTAccParc.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["AccionParcial"].ToString().Trim());
                    TxtTSN.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OTSN"].ToString().Trim());
                    TxtTSO.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OTSO"].ToString().Trim());
                    TxtTSR.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OTSR"].ToString().Trim());
                    TxtCSN.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OCSN"].ToString().Trim());
                    TxtCSO.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OCSO"].ToString().Trim());
                    TxtCSR.Text = HttpUtility.HtmlDecode(DSTOTGrl.Tables[0].Rows[0]["OCSR"].ToString().Trim());
                    ViewState["TtlOTRegDet"] = Convert.ToInt32(DSTOTGrl.Tables[0].Rows[0]["TtlRegDet"].ToString());
                    ViewState["CarpetaCargaMasiva"] = DSTOTGrl.Tables[0].Rows[0]["CargaMasiva"].ToString();
                    ViewState["P1"] = DSTOTGrl.Tables[0].Rows[0]["P1"].ToString(); ViewState["EP1"] = DSTOTGrl.Tables[0].Rows[0]["EP1"].ToString();
                    ViewState["P2"] = DSTOTGrl.Tables[0].Rows[0]["P2"].ToString(); ViewState["EP2"] = DSTOTGrl.Tables[0].Rows[0]["EP2"].ToString();
                    ViewState["P3"] = DSTOTGrl.Tables[0].Rows[0]["P3"].ToString(); ViewState["EP3"] = DSTOTGrl.Tables[0].Rows[0]["EP3"].ToString();
                    ViewState["P4"] = DSTOTGrl.Tables[0].Rows[0]["P4"].ToString(); ViewState["EP4"] = DSTOTGrl.Tables[0].Rows[0]["EP4"].ToString();
                    ViewState["P5"] = DSTOTGrl.Tables[0].Rows[0]["P5"].ToString(); ViewState["EP5"] = DSTOTGrl.Tables[0].Rows[0]["EP5"].ToString();
                    ViewState["P6"] = DSTOTGrl.Tables[0].Rows[0]["P6"].ToString(); ViewState["EP6"] = DSTOTGrl.Tables[0].Rows[0]["EP6"].ToString();
                    ViewState["P7"] = DSTOTGrl.Tables[0].Rows[0]["P7"].ToString(); ViewState["EP7"] = DSTOTGrl.Tables[0].Rows[0]["EP7"].ToString();
                    ViewState["P8"] = DSTOTGrl.Tables[0].Rows[0]["P8"].ToString(); ViewState["EP8"] = DSTOTGrl.Tables[0].Rows[0]["EP8"].ToString();
                    ViewState["CodIdDetSrvManto"] = DSTOTGrl.Tables[0].Rows[0]["CodIdDetSrvManto"].ToString();
                    ViewState["EsInspector"] = DSTOTGrl.Tables[0].Rows[0]["TipoUsu"].ToString();
                    ViewState["UltTec"] = DSTOTGrl.Tables[0].Rows[0]["CodUltTec"].ToString(); ViewState["UltLicTec"] = DSTOTGrl.Tables[0].Rows[0]["CodUltLicTec"].ToString();
                    ViewState["UltInsp"] = DSTOTGrl.Tables[0].Rows[0]["CodUltInsp"].ToString(); ViewState["UltLicInsp"] = DSTOTGrl.Tables[0].Rows[0]["CodUltLicInsp"].ToString();
                    ViewState["IdentificadorCorrPrev"] = DSTOTGrl.Tables[0].Rows[0]["IdentificadorCorrPrev"].ToString();
                    ViewState["BanCerrado"] = Convert.ToInt32(DSTOTGrl.Tables[0].Rows[0]["BanCerrado"].ToString());
                    LblTitCancel.Visible = false;
                    if (CkbCancel.Checked == true)
                    {
                        BtnOtModificar.Enabled = false;
                        LblTitCancel.Visible = true;
                        DataRow[] Result2 = Idioma.Select("Objeto= 'CkbCancel'");
                        foreach (DataRow row in Result2)
                        { LblTitCancel.Text = row["Texto"].ToString().Trim(); }
                    }
                    EstadoPasos();
                }
            }
            catch (Exception)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
            }
        }
        protected void DdlLicInsp(string Insp, string lic)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','{1}','','','LINSP',0,0,0,0,'01-01-01','01-01-01','01-01-01'", Insp, lic);
            DdlOtLicInsp.DataSource = Cnx.DSET(LtxtSql);
            DdlOtLicInsp.DataMember = "Datos";
            DdlOtLicInsp.DataTextField = "Licencia";
            DdlOtLicInsp.DataValueField = "Codigo";
            DdlOtLicInsp.DataBind();
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BindDdlOTCondicional(string RSP)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;
            string LtxtSql = "";

            if (DSTOTGrl.Tables[5].Rows.Count > 0) //"Taller"
            {
                DataTable DT = new DataTable();

                DataRow[] dr = DSTOTGrl.Tables[5].Select("Activo=1");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }

                Result = DSTOTGrl.Tables[5].Select("CodTaller= '" + ViewState["TllAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlMroTaller.DataSource = DT;
                DdlMroTaller.DataTextField = "NomTaller";
                DdlMroTaller.DataValueField = "CodTaller";
                DdlMroTaller.DataBind();
                DdlMroTaller.Text = ViewState["TllAnt"].ToString().Trim();
            }

            if (DSTOTGrl.Tables[6].Rows.Count > 0) //"Base"
            {
                DataTable DT = new DataTable();

                DataRow[] dr = DSTOTGrl.Tables[6].Select("Activo=1");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }
                Result = DSTOTGrl.Tables[6].Select("CodBase= '" + ViewState["BaseAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlOTBase.DataSource = DT;
                DdlOTBase.DataTextField = "NomBase";
                DdlOTBase.DataValueField = "CodBase";
                DdlOTBase.DataBind();
                DdlOTBase.Text = ViewState["BaseAnt"].ToString().Trim();
            }

            if (DSTOTGrl.Tables[7].Rows.Count > 0) //"Inspector"
            {
                DataTable DT = new DataTable();

                DataRow[] dr = DSTOTGrl.Tables[7].Select("Estado = 'ACTIVO' AND Inspector = 1");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }
                Result = DSTOTGrl.Tables[7].Select("CodPersona= '" + ViewState["InsptAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlOtInsp.DataSource = DT;
                DdlOtInsp.DataTextField = "Tecnico";
                DdlOtInsp.DataValueField = "CodPersona";
                DdlOtInsp.DataBind();
                DdlOtInsp.Text = ViewState["InsptAnt"].ToString().Trim();
            }

            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','RESP',0,0,0,0,'01-01-01','01-01-01','01-01-01'", RSP);
            DdlOtRespons.DataSource = Cnx.DSET(LtxtSql);
            DdlOtRespons.DataMember = "Datos";
            DdlOtRespons.DataTextField = "Tecnico";
            DdlOtRespons.DataValueField = "CodPersona";
            DdlOtRespons.DataBind();

            if (DSTOTGrl.Tables[8].Rows.Count > 0) //"CCosto"
            {
                DataTable DT = new DataTable();
                DataRow[] dr = DSTOTGrl.Tables[8].Select("Activo=1");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }
                Result = DSTOTGrl.Tables[8].Select("CodCc= '" + ViewState["CCstAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlOtCCosto.DataSource = DT;
                DdlOtCCosto.DataTextField = "Nombre";
                DdlOtCCosto.DataValueField = "CodCc";
                DdlOtCCosto.DataBind();
                DdlOtCCosto.Text = ViewState["CCstAnt"].ToString().Trim();
            }
        }
        protected void VisibleBotMRO(bool Estado)
        {
            if (ViewState["Hab8Pasos"].Equals("S"))
            {
                if (!TxtOtPpal.Text.Equals("0") || !TxtOtReporte.Text.Equals("0") || !TxtOtRepacion.Text.Equals(""))
                { CkbEjePasos.Visible = false; }
                else { CkbEjePasos.Visible = Estado; }
                if (CkbEjePasos.Checked == true)
                {
                    BtnMroInsPre.Visible = Estado;
                    BtnMroPrDes.Visible = Estado;
                    BtnMroRteDes.Visible = Estado;
                    BtnMroDanOc.Visible = Estado;
                    BtnMroAccCorr.Visible = Estado;
                    BtnMroPrueF.Visible = Estado;
                    BtnMroCumpl.Visible = Estado;
                    BtnMroTrabEje.Visible = Estado;
                    LblMroPpt.Visible = Estado; TxtMroPpt.Visible = Estado;
                    LblMroCliente.Visible = Estado; TxtMroCliente.Visible = Estado;
                    LblMroTaller.Visible = Estado; DdlMroTaller.Visible = Estado;
                }
                else
                {
                    BtnMroInsPre.Visible = false;
                    BtnMroPrDes.Visible = false;
                    BtnMroRteDes.Visible = false;
                    BtnMroDanOc.Visible = false;
                    BtnMroAccCorr.Visible = false;
                    BtnMroPrueF.Visible = false;
                    BtnMroCumpl.Visible = false;
                    BtnMroTrabEje.Visible = false;
                    LblMroPpt.Visible = false; TxtMroPpt.Visible = false;
                    LblMroCliente.Visible = false; TxtMroCliente.Visible = false;
                    LblMroTaller.Visible = false; DdlMroTaller.Visible = false;
                }
            }
        }
        protected void DdlBusqOT_TextChanged(object sender, EventArgs e)
        { TraerDatosBusqOT(Convert.ToInt32(DdlBusqOT.Text), "UPD"); }
        protected void DdlOtEstado_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ViewState["VblCE5"]) == 1 && DdlOtEstado.Text.Trim().Equals("0002"))// Asignar Aeronave / Tiempos
            {
                if (Convert.ToInt32(ViewState["VblCE2"]) == 1)/// CERRAR/CANCEL
                {
                    Idioma = (DataTable)ViewState["TablaIdioma"];

                    if (TxtOTFechFin.Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens43OT'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar al menos un técnico con su respectiva fecha de trabajo.
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        string LtxtSql = "EXEC SP_PANTALLA_OrdenTrabajo2 10,@P,@S,'','','',0,0,0,@ICC,@F,'01-01-01','01-01-01'";
                        SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                        SqlC.Parameters.AddWithValue("@P", TxtOtPN.Text.Trim());
                        SqlC.Parameters.AddWithValue("@S", TxtAplicab.Text.Trim());
                        SqlC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlC.Parameters.AddWithValue("@F", TxtOTFechFin.Text.Trim());
                        SqlDataReader SDR = SqlC.ExecuteReader();
                        if (SDR.Read())
                        {
                            TxtTSN.Text = SDR["TSN"].ToString().Trim(); TxtTSO.Text = SDR["TSO"].ToString().Trim(); TxtTSR.Text = SDR["TSR"].ToString().Trim();
                            TxtCSN.Text = SDR["CSN"].ToString().Trim(); TxtCSO.Text = SDR["CSO"].ToString().Trim(); TxtCSR.Text = SDR["CSR"].ToString().Trim();
                        }
                        if (Convert.ToDouble(TxtTSN.Text) > 0 || Convert.ToDouble(TxtTSO.Text) > 0 || Convert.ToDouble(TxtCSN.Text) > 0 || Convert.ToDouble(TxtCSO.Text) > 0)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensOT56'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplOT, this.UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); } //Por favor verifique los tiempos que el sistema carga en la OT.
                        }
                    }
                }
            }
        }
        protected void DdlOtInsp_TextChanged(object sender, EventArgs e)
        { DdlLicInsp(DdlOtInsp.Text.Trim(), ""); }
        //******************************************  Botones edicion OT *********************************************************       
        protected void BtnOtModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtOt.Text.Equals(""))
                { return; }
                if (ViewState["Accion"].Equals(""))
                {
                    TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "SEL");
                    ViewState["TllAnt"] = DdlMroTaller.Text.Trim();
                    ViewState["BaseAnt"] = DdlOTBase.Text.Trim();
                    ViewState["InsptAnt"] = DdlOtInsp.Text.Trim();
                    string VbLInsp = DdlOtLicInsp.Text.Trim();
                    DdlLicInsp(ViewState["InsptAnt"].ToString().Trim(), VbLInsp);
                    string VbResp = DdlOtRespons.Text.Trim();
                    ViewState["CCstAnt"] = DdlOtCCosto.Text.Trim();
                    BindDdlOTCondicional(VbResp);
                    DdlOtLicInsp.Text = VbLInsp;
                    DdlOtRespons.Text = VbResp;
                    ActivarBtnOT(false, true, false, false, false);
                    ViewState["Accion"] = "UPDATE";
                    DataRow[] Result1 = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result1)
                    { BtnOtModificar.Text = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result1)
                    { BtnOtModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la edición?                
                    ActivarCampOT(true, true, "UPDATE");
                    DdlBusqOT.SelectedValue = "0";
                    DdlBusqOT.Enabled = false;
                    CalOTFechVenc.StartDate = Convert.ToDateTime(TxtOTFechReg.Text);
                }
                else
                {
                    ValidarOT("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    {
                        ActivarBtnOT(true, true, true, true, true);
                        DataRow[] Result4 = Idioma.Select("Objeto= 'BotonMod'");
                        foreach (DataRow row in Result4)
                        { BtnOtModificar.Text = row["Texto"].ToString().Trim(); }
                        ActivarCampOT(false, false, "UPDATE");
                        DdlBusqOT.Enabled = true;
                        TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "SEL");
                        BtnOtModificar.OnClientClick = "";
                        return;
                    }
                    DateTime? FecFin; DateTime? fecVenc;
                    if (TxtOTFechFin.Text.Equals(""))
                    { FecFin = null; }
                    else
                    { FecFin = Convert.ToDateTime(TxtOTFechFin.Text); }
                    if (TxtOTFechVenc.Text.Equals(""))
                    { fecVenc = null; }
                    else
                    { fecVenc = Convert.ToDateTime(TxtOTFechVenc.Text); }
                    DateTime? VbFechaReg = Convert.ToDateTime(TxtOTFechReg.Text);
                    List<ClsTypOrdenTrabajo> ObjOT = new List<ClsTypOrdenTrabajo>();
                    DateTime? VbFechaI;
                    if (TxtOTFechini.Text.Equals("")) { VbFechaI = null; }
                    else { VbFechaI = Convert.ToDateTime(TxtOTFechini.Text); }

                    var TypOT = new ClsTypOrdenTrabajo()
                    {
                        CodNumOrdenTrab = Convert.ToInt32(TxtOt.Text),
                        Descripcion = TxtOTTrabajo.Text.Trim(),
                        CodEstOrdTrab1 = DdlOtEstado.Text.Trim(),
                        CodEstOrdTrab2 = DdlOtEstaSec.Text.Trim(),
                        Aplicabilidad = TxtAplicab.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = "",
                        CodBase = DdlOTBase.Text.Trim(),
                        CodTaller = DdlMroTaller.Text.Trim(),
                        CodPlanManto = "",
                        CentroCosto = DdlOtCCosto.Text.Trim(),
                        FechaInicio = VbFechaI,
                        FechaFinal = FecFin,
                        FechaReg = Convert.ToDateTime(TxtOTFechReg.Text),
                        IdentificadorCorrPrev = Convert.ToInt32(ViewState["IdentificadorCorrPrev"]),
                        CodPrioridad = TxtlOtPrioridad.Text.Trim(),
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = (int)ViewState["BanCerrado"],
                        HorasProyectadas = 0,
                        FechaProyectada = fecVenc,
                        FechaVencimiento = fecVenc,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = TxtOTAccParc.Text.Trim(),
                        CodTipoCodigo = "01",
                        CodInspectorCierre = DdlOtInsp.Text.Trim(),
                        LicenciaInspCierre = DdlOtLicInsp.Text.Trim(),
                        PNOT = TxtOtPN.Text.Trim(),
                        BloquearDetalle = CkbOtBloqDet.Checked == true ? 1 : 0,
                        CodResponsable = DdlOtRespons.Text.Trim(),
                        OTSN = Convert.ToDouble(TxtTSN.Text),
                        OTSO = Convert.ToDouble(TxtTSO.Text),
                        OTSR = Convert.ToDouble(TxtTSR.Text),
                        OCSN = Convert.ToDouble(TxtCSN.Text),
                        OCSO = Convert.ToDouble(TxtCSO.Text),
                        OCSR = Convert.ToDouble(TxtCSR.Text),
                        EjecPasos = CkbEjePasos.Checked == true ? 1 : 0,
                        CancelOT = CkbCancel.Checked == true ? 1 : 0,
                        WS = "",
                        OKOT = Convert.ToInt32(DdlOTAero.Text),
                        AccionOT = "UPDATE",
                    };
                    ObjOT.Add(TypOT);
                    ClsTypOrdenTrabajo ClsOrdenTrabajo = new ClsTypOrdenTrabajo();
                    ClsOrdenTrabajo.Alimentar(ObjOT);
                    string Mensj = ClsOrdenTrabajo.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        UplOT.Update();
                    }
                    ActivarBtnOT(true, true, true, true, true);
                    DataRow[] Result3 = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result3)
                    { BtnOtModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampOT(false, false, "UPDATE");
                    DdlBusqOT.Enabled = true;
                    TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD");
                    BtnOtModificar.OnClientClick = "";
                    ViewState["Accion"] = "";
                    if (!ClsOrdenTrabajo.GetMensjAlterno().Equals(""))
                    {
                        Mensj = ClsOrdenTrabajo.GetMensjAlterno().ToString().Trim();
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result4 = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result4)
                { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Inconveniente en la actualización')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnOTReserva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (!TxtOt.Text.Equals(""))
            {
                if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
                {
                    BtnOTCargaMasiva.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens21'");
                    foreach (DataRow row in Result)
                    { BtnOTCargaMasiva.ToolTip = row["Texto"].ToString(); }// "La orden debe estar abierta y no deben existir registros en la reserva"
                }
                else
                {
                    BtnOTCargaMasiva.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                    foreach (DataRow row in Result)
                    { BtnOTCargaMasiva.ToolTip = row["Texto"].ToString() + " " + ViewState["CarpetaCargaMasiva"].ToString() + "CargaMasiva.xlsx"; }
                }
                if (CkbOtBloqDet.Checked == true)
                { BtnOTRecurNotif.Enabled = false; }
                else { BtnOTRecurNotif.Enabled = true; }
                ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
                ViewState["VentanaRva"] = MlVwOT.ActiveViewIndex;
                BindDOTRecursoF();
                PerfilesGrid();
                LblRecFRte.Visible = false;
                TxtRecurNumRte.Visible = false;
                LblRecFSubOt.Visible = false;
                TxtRecurSubOt.Visible = false;
                LblPrioridadOT.Visible = false;
                DdlPrioridadOT.Visible = false;
                LblTitLicencia.Visible = false;
                GrdLicen.Visible = false;
                MlVwOT.ActiveViewIndex = 2;
            }
        }
        protected void BtnOTConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result = Idioma.Select("Objeto= 'LblTitOTOpcBusqueda'");
            foreach (DataRow row in Result)
            { LblTitOTOpcBusqueda.Text = row["Texto"].ToString(); }
            TblOTBusq.Visible = true;
            IbtOTExpBusqOT.Visible = true;
            GrdOTBusq.DataSource = null;
            GrdOTBusq.DataBind();
            ViewState["VentanaBusq"] = MlVwOT.ActiveViewIndex;
            RdbOTBusqNumOT.Checked = true;
            MlVwOT.ActiveViewIndex = 4;
        }
        protected void BtnOTImprimir_Click(object sender, EventArgs e)
        {
            DataTable Idioma = new DataTable();
            if (TxtOt.Text.Equals(""))
            { return; }
            string StSql = "", VbOpc = "";
            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[3];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

                if (Convert.ToInt32(TxtOtReporte.Text) == 0 && TxtOtRepacion.Text.Trim().Equals("")) // Preventivo
                {
                    StSql = "EXEC Impresion_OT_MRO @OT,'SERVICIO',@ICC";
                    VbOpc = "OT_PREV";
                }
                else
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensOT64'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La impresión solo aplica para órdenes de trabajo Master.
                    return;
                }
                ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
                MlVwOT.ActiveViewIndex = 5;
                using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                {
                    SC.Parameters.AddWithValue("@OT", TxtOt.Text);
                    //SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(ds);
                        RvwOTPrint.LocalReport.EnableExternalImages = true;
                        switch (VbOpc)
                        {
                            case "OT_PREV":
                                RvwOTPrint.LocalReport.ReportPath = Server.MapPath("~/Report/Ing/OrdenTrabajoMRO.rdlc");// "~/Forms/Ingenieria/Informe/OrdenTrabajoMRO.rdlc";
                                break;
                            default:
                                VbOpc = "";
                                break;
                        }
                        RvwOTPrint.LocalReport.DataSources.Clear();
                        RvwOTPrint.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                        RvwOTPrint.LocalReport.SetParameters(parameters);
                        RvwOTPrint.LocalReport.Refresh();
                    }
                }
            }
        }
        protected void BtnOTEliminar_Click(object sender, EventArgs e)
        {
            if (TxtOt.Text.Equals(""))
            { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC Consultas_General 7,@Us,'','',@O,0,@ICC,'01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@O", Convert.ToInt32(TxtOt.Text));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            string VbMensj = "";
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                            }
                            SDR.Close();
                            Transac.Commit();
                            if (!VbMensj.Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { VbMensj = row["Texto"].ToString(); }
                                ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "')", true);
                                return;
                            }
                            LimpiarCamposOT();
                            TraerDatosBusqOT(Convert.ToInt32(DdlBusqOT.Text), "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnOtAbiertas8PasCump_Click(object sender, EventArgs e)
        { BIndDPasoCOTA(); MlVwOT.ActiveViewIndex = 10; }
        protected void BtNOTExportar_Click(object sender, EventArgs e)
        { Exportar("OTGeneral"); }
        protected void BtnOTDetTec_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                BindDOTDetTec();
                PerfilesGrid();
                MlVwOT.ActiveViewIndex = 1;
            }
        }
        //******************************************  Detalle Técnico *********************************************************    
        protected void BindDOTDetTec()
        {
            try
            {
                DataTable DT = new DataTable();
                DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
                DT = DSTOTGrl.Tables[9].Clone();
                DataRow[] DR = DSTOTGrl.Tables[9].Select("Tecnico LIKE '%" + TxtConsulOTDetTec.Text.Trim() + "%'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                if (DT.Rows.Count > 0)
                {
                    GrdOTDetTec.DataSource = DT;
                    GrdOTDetTec.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdOTDetTec.DataSource = DT;
                    GrdOTDetTec.DataBind();
                    GrdOTDetTec.Rows[0].Cells.Clear();
                    GrdOTDetTec.Rows[0].Cells.Add(new TableCell());
                    GrdOTDetTec.Rows[0].Cells[0].Text = "";
                    GrdOTDetTec.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDOTDetTec", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarOTDetTec_Click(object sender, ImageClickEventArgs e)
        { TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "SEL"); MlVwOT.ActiveViewIndex = 0; }
        protected void IbtConsOTDetTec_Click(object sender, ImageClickEventArgs e)
        { BindDOTDetTec(); }
        protected void DdlOTTecPP_TextChanged(object sender, EventArgs e)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];

            if (DSTOTGrl.Tables[10].Rows.Count > 0) //"Inspector"
            {
                DataTable DT = new DataTable();
                DropDownList DdlOTTecPP = (GrdOTDetTec.FooterRow.FindControl("DdlOTTecPP") as DropDownList);
                DropDownList DdlOTLicPP = (GrdOTDetTec.FooterRow.FindControl("DdlOTLicPP") as DropDownList);

                DataRow[] dr = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlOTTecPP.Text.Trim() + "'");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }

                DdlOTLicPP.DataSource = DT;
                DdlOTLicPP.DataTextField = "Licencia";
                DdlOTLicPP.DataValueField = "Codigo";
                DdlOTLicPP.DataBind();
            }
        }
        protected void GrdOTDetTec_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (TxtOt.Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens06'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }// Debe seleccionar una Orden de trabajo

                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("TxtOTFecTrabPP") as TextBox).Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens07'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una fecha
                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("DdlOTTecPP") as DropDownList).Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar un técnico
                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("DdlOTLicPP") as DropDownList).Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens09'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una licencia')", true);
                        return;
                    }
                    CultureInfo Culture = new CultureInfo("en-US");
                    string VblTxtCant = (GrdOTDetTec.FooterRow.FindControl("TxtNumHorasPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdOTDetTec.FooterRow.FindControl("TxtNumHorasPP") as TextBox).Text.Trim();
                    double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    if (VblCant <= 0)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens10'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//El tiempo debe ser mayor a cero')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,@TEC,@Lic,@Usu,'','','','','','INSERT',@OT,@T,0,0,@ICC,0,@F,'02-01-1','03-01-1'");

                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@OT", TxtOt.Text);
                                    SC.Parameters.AddWithValue("@TEC", (GrdOTDetTec.FooterRow.FindControl("DdlOTTecPP") as DropDownList).Text.Trim());
                                    SC.Parameters.AddWithValue("@F", (GrdOTDetTec.FooterRow.FindControl("TxtOTFecTrabPP") as TextBox).Text.Trim());
                                    SC.Parameters.AddWithValue("@T", VblCant);
                                    SC.Parameters.AddWithValue("@Lic", (GrdOTDetTec.FooterRow.FindControl("DdlOTLicPP") as DropDownList).Text.Trim());
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
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
                                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        return;
                                    }
                                    TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD");
                                    BindDOTDetTec();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdOTDetTec.EditIndex = e.NewEditIndex; ViewState["Index"] = e.NewEditIndex; BindDOTDetTec(); }
        protected void GrdOTDetTec_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (TxtOt.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Orden de trabajo')", true);
                    return;
                }
                int Idx = (int)ViewState["Index"];
                int VblId = Convert.ToInt32(GrdOTDetTec.DataKeys[Idx].Value.ToString());

                if ((GrdOTDetTec.Rows[Idx].FindControl("TxtOTFecTrab") as TextBox).Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens07'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una fecha')", true);
                    return;
                }
                CultureInfo Culture = new CultureInfo("en-US");
                string VblTxtCant = (GrdOTDetTec.Rows[Idx].FindControl("TxtNumHoras") as TextBox).Text.Trim().Equals("") ? "1" : (GrdOTDetTec.Rows[Idx].FindControl("TxtNumHoras") as TextBox).Text.Trim();
                double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,'','',@Usu,'','','','','','UPDATE',@OT,@T,0,0, @ICC,@I,@F,'02-01-1','03-01-1'");

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@I", VblId);
                                SC.Parameters.AddWithValue("@OT", TxtOt.Text);
                                SC.Parameters.AddWithValue("@F", (GrdOTDetTec.Rows[Idx].FindControl("TxtOTFecTrab") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@T", VblCant);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
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
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                GrdOTDetTec.EditIndex = -1;
                                TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD");
                                BindDOTDetTec();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//alert('Error en la edición')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, " Validación Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdOTDetTec.EditIndex = -1; BindDOTDetTec(); }
        protected void GrdOTDetTec_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                int Idx = e.RowIndex;
                int VblId = Convert.ToInt32(GrdOTDetTec.DataKeys[Idx].Value.ToString());
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,'','',@Usu,'','','','','','DELETE', 0, 0, 0, 0,@ICC,@I,'02-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@I", VblId);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
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
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD");
                                BindDOTDetTec();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (DSTOTGrl.Tables[7].Rows.Count > 0) //"Inspector"
                    {
                        DataTable DT = new DataTable();
                        DropDownList DdlOTTecPP = (e.Row.FindControl("DdlOTTecPP") as DropDownList);

                        DataRow[] dr = DSTOTGrl.Tables[7].Select("Estado = 'ACTIVO' AND CrearReporte = 1");
                        if (IsIENumerableLleno(dr))
                        { DT = dr.CopyToDataTable(); }

                        DdlOTTecPP.DataSource = DT;
                        DdlOTTecPP.DataTextField = "Tecnico";
                        DdlOTTecPP.DataValueField = "CodPersona";
                        DdlOTTecPP.DataBind();
                        DdlOTTecPP.Text = ViewState["InsptAnt"].ToString().Trim();
                    }
                    CalendarExtender CalOTFecTrabPP = (e.Row.FindControl("CalOTFecTrabPP") as CalendarExtender);
                    CalOTFecTrabPP.EndDate = DateTime.Now;

                    ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    CalendarExtender CalOTFecTrab = (e.Row.FindControl("CalOTFecTrab") as CalendarExtender);
                    CalOTFecTrab.EndDate = DateTime.Now;
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
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    ImageButton IbtEdit = (e.Row.FindControl("IbtEdit") as ImageButton);
                    if (IbtEdit != null)
                    {
                        foreach (DataRow RowIdioma in Result)
                        { IbtEdit.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
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
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error')", true);
            }
        }
        protected void GrdOTDetTec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdOTDetTec.PageIndex = e.NewPageIndex; BindDOTDetTec(); PerfilesGrid(); }
        //******************************************  RECURSO FISICO OT*********************************************************
        protected void BindDOTRecursoF()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
                DSTRTE = (DataSet)ViewState["DSTRTE"];
                DataTable DT = new DataTable();
                DataRow[] DR;

                if ((int)ViewState["VentanaRva"] == 0) //OT
                {
                    DT = DSTOTGrl.Tables[11].Clone();
                    DR = DSTOTGrl.Tables[11].Select("PN LIKE '%" + TxtOTRecurConsulPn.Text.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                else//Rte
                {
                    DT = DSTRTE.Tables[2].Clone();
                    DR = DSTRTE.Tables[2].Select("PN LIKE '%" + TxtOTRecurConsulPn.Text.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                if (DT.Rows.Count > 0)
                {
                    GrdOTRecursoF.DataSource = DT;
                    GrdOTRecursoF.DataBind();
                    ViewState["TtlOTRegDet"] = DT.Rows.Count;
                }
                else
                {
                    ViewState["TtlOTRegDet"] = 0;
                    DT.Rows.Add(DT.NewRow());
                    GrdOTRecursoF.DataSource = DT;
                    GrdOTRecursoF.DataBind();
                    GrdOTRecursoF.Rows[0].Cells.Clear();
                    GrdOTRecursoF.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens41'");
                    foreach (DataRow row in Result)
                    { GrdOTRecursoF.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdOTRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }

            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtOTCerrarRecur_Click(object sender, ImageClickEventArgs e)
        {
            if ((int)ViewState["VentanaRva"] == 7)
            {
                TxtOtSec.Text = TxtRecurSubOt.Text;
                ViewState["CodPrioridad"] = DdlPrioridadOT.Text.Trim();
            }
            MlVwOT.ActiveViewIndex = (int)ViewState["VentanaRva"];
        }
        protected void IbtOTRecurConsulPn_Click(object sender, ImageClickEventArgs e)
        { BindDOTRecursoF(); }
        protected void IbtOTRecurExpExcelPn_Click(object sender, ImageClickEventArgs e)
        { Exportar("Reserva"); }
        protected void BtnOTCargaMasiva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((int)ViewState["TtlOTRegDet"] > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }//Para realizar la carga masiva la reserva debe estar vacía')", true);                
                return;
            }
            if ((int)ViewState["VentanaRva"] == 0)
            {
                LblCargaMasRte.Visible = false;
                TxtCargaMasiRte.Visible = false;
                LblCargaMasOt.Visible = false;
                TxtCargaMasiOT.Visible = false;
            }
            else
            {
                LblCargaMasRte.Visible = true;
                TxtCargaMasiRte.Visible = true;
                LblCargaMasOt.Visible = true;
                TxtCargaMasiOT.Visible = true;
                TxtCargaMasiRte.Text = TxtRecurNumRte.Text;
                TxtCargaMasiOT.Text = TxtRecurSubOt.Text;
            }
            IbtOTGuardarCargaMax.Enabled = false;
            MlVwOT.ActiveViewIndex = 3;
        }
        protected void BtnOTRecurNotif_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbOTRva = "", VbNumRte = "";
            if ((int)ViewState["VentanaRva"] == 0)
            {
                VbOTRva = TxtOt.Text; VbNumRte = "0";
                if (CkbOtBloqDet.Checked == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
            }
            else
            {
                VbOTRva = TxtRecurSubOt.Text; VbNumRte = TxtNroRte.Text;
                if ((int)ViewState["BloquearDetalleRte"] == 1)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'CerrBloq'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    return;
                }
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 7,@Usu,'','','','',@OT,0,0,@ICC,'01-01-01','01-01-01','01-01-01'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@OT", Convert.ToInt32(VbOTRva));
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            string Mensj = "OK";
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
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString(); }
                                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                return;
                            }/**/
                            if ((int)ViewState["VentanaRva"] == 0)
                            { CkbOtBloqDet.Checked = true; }
                            BtnOTRecurNotif.Enabled = false;
                            DataRow[] Result2 = Idioma.Select("Objeto= 'Mens14'");
                            foreach (DataRow row in Result2)
                            { Mensj = row["Texto"].ToString(); }
                            ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);// Orden de trabajo notificada')", true);
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens15'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void DdlOTPNRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;
            TextBox TxtDesRFPP = (GrdOTRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox);
            DropDownList DdlOTPNRFPP = (GrdOTRecursoF.FooterRow.FindControl("DdlOTPNRFPP") as DropDownList);
            TextBox TxtOTPNRFPP = (GrdOTRecursoF.FooterRow.FindControl("TxtOTPNRFPP") as TextBox);
            if (DdlOTPNRFPP.Text.Trim().Equals("- N -"))
            {
                DdlOTPNRFPP.Visible = false;
                TxtOTPNRFPP.Visible = true;
                TxtOTPNRFPP.Enabled = true;
                TxtDesRFPP.Text = "";
                TxtDesRFPP.Enabled = true;
                return;
            }
            Result = DSTOTGrl.Tables[12].Select("PN= '" + DdlOTPNRFPP.Text.Trim() + "'");
            foreach (DataRow row in Result)
            { TxtDesRFPP.Text = row["Descripcion"].ToString().Trim(); }

        }
        protected void GrdOTRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string Vbprioridad = "", VbOTRva = "", VbCodHK = "", VbNumRte = "";
                if ((int)ViewState["VentanaRva"] == 0)
                { Vbprioridad = TxtlOtPrioridad.Text.Trim(); VbOTRva = TxtOt.Text; VbCodHK = DdlOTAero.Text; VbNumRte = "0"; }
                else { Vbprioridad = DdlPrioridadOT.Text.Trim(); VbOTRva = TxtRecurSubOt.Text; VbCodHK = DdlAeroRte.Text; VbNumRte = TxtNroRte.Text; }
                if (e.CommandName.Equals("AddNew"))
                {
                    if (Vbprioridad.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'RteMens36'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una prioridad')", true);
                        return;
                    }
                    string VblPN, VBQuery, VblTxtCant, VbDesc, VbIPC;
                    double VblCant;
                    if ((GrdOTRecursoF.FooterRow.FindControl("DdlOTPNRFPP") as DropDownList).Visible == true)
                    { VblPN = (GrdOTRecursoF.FooterRow.FindControl("DdlOTPNRFPP") as DropDownList).SelectedValue.Trim(); }
                    else
                    { VblPN = (GrdOTRecursoF.FooterRow.FindControl("TxtOTPNRFPP") as TextBox).Text.Trim(); }

                    CultureInfo Culture = new CultureInfo("en-US");
                    VblTxtCant = (GrdOTRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdOTRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim();
                    VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    VbDesc = (GrdOTRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox).Text.Trim();
                    VbIPC = (GrdOTRecursoF.FooterRow.FindControl("TxtIPCRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'', @ICC,'INSERT',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@IdDetRsva", 0);
                                    SC.Parameters.AddWithValue("@PN", VblPN);
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@CodPri", Vbprioridad.Trim());
                                    SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                    SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                    SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@OT", Convert.ToInt32(VbOTRva));
                                    SC.Parameters.AddWithValue("@Cant", VblCant);
                                    SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(VbCodHK));
                                    SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(VbNumRte));

                                    string Mensj = "OK";
                                    string VbEjecPlano = "N";
                                    if ((int)ViewState["VentanaRva"] == 7)
                                    { VbOTRva = TxtRecurSubOt.Text; }

                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim());
                                        if ((int)ViewState["VentanaRva"] == 7)
                                        { VbOTRva = SDR["SubOT"].ToString().Trim(); }
                                    }
                                    SDR.Close();
                                    Transac.Commit();
                                    if (!Mensj.ToString().Trim().Equals("OK"))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString(); }
                                        ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        return;
                                    }
                                    if ((int)ViewState["VentanaRva"] == 7)
                                    {
                                        TxtRecurSubOt.Text = VbOTRva.ToString();
                                        if (VbEjecPlano.Trim().Equals("S"))
                                        {
                                            Cnx.SelecBD();
                                            using (SqlConnection SCnxPln = new SqlConnection(Cnx.GetConex()))
                                            {
                                                //sqlCon.Open();
                                                VBQuery = string.Format("EXEC SP_IntegradorNEW 6,'',@Usu,'','','',@CodOT,0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                                                using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                                                {
                                                    try
                                                    {
                                                        sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                                        sqlCmd.Parameters.AddWithValue("@CodOT", Convert.ToInt32(VbOTRva));
                                                        sqlCmd.ExecuteNonQuery();
                                                    }
                                                    catch (Exception Ex)
                                                    {
                                                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Generar Nueva Reserva OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TxtOTRecurConsulPn.Text = "";
                                    if ((int)ViewState["VentanaRva"] == 0) { TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD"); }//OT
                                    else { TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD"); } //Rte
                                    BindDOTRecursoF();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdOTRecursoF.EditIndex = e.NewEditIndex; ViewState["Index"] = e.NewEditIndex; BindDOTRecursoF(); }
        protected void GrdOTRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string Vbprioridad = "", VbOTRva = "", VbCodHK = "", VbNumRte = "";
                if ((int)ViewState["VentanaRva"] == 0)
                { Vbprioridad = TxtlOtPrioridad.Text.Trim(); VbOTRva = TxtOt.Text; VbCodHK = DdlOTAero.Text; VbNumRte = "0"; }
                else { Vbprioridad = DdlPrioridadOT.Text.Trim(); VbOTRva = TxtRecurSubOt.Text; VbCodHK = DdlAeroRte.Text; VbNumRte = TxtNroRte.Text; }
                if (Vbprioridad.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una prioridad')", true);
                    return;
                }
                string VblPN, VBQuery, VblTxtCant, VbDesc, VbIPC;
                double VblCant;
                int Idx = (int)ViewState["Index"];
                int VblId = Convert.ToInt32(GrdOTRecursoF.DataKeys[Idx].Value.ToString());

                VblPN = (GrdOTRecursoF.Rows[Idx].FindControl("TxtOTPNRF") as TextBox).Text.Trim();

                CultureInfo Culture = new CultureInfo("en-US");
                VblTxtCant = (GrdOTRecursoF.Rows[Idx].FindControl("TxtCantRF") as TextBox).Text.Trim().Equals("") ? "1" : (GrdOTRecursoF.Rows[Idx].FindControl("TxtCantRF") as TextBox).Text.Trim();
                VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                VbDesc = (GrdOTRecursoF.Rows[Idx].FindControl("TxtDesRF") as TextBox).Text.Trim();
                VbIPC = (GrdOTRecursoF.Rows[Idx].FindControl("TxtIPCRF") as TextBox).Text.Trim();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'', @ICC,'UPDATE',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CodPri", Vbprioridad.Trim());
                                SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.Parameters.AddWithValue("@OT", Convert.ToInt32(VbOTRva));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(VbCodHK));
                                SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(VbNumRte));
                                string Mensj = "OK";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals("OK"))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                GrdOTRecursoF.EditIndex = -1;
                                if ((int)ViewState["VentanaRva"] == 0) { TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD"); }//OT
                                else { TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD"); } //Rte
                                BindDOTRecursoF();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Reserva OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Reserva OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdOTRecursoF.EditIndex = -1; BindDOTRecursoF(); }
        protected void GrdOTRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery = "", Vbprioridad = "", VbOTRva = "", VbCodHK = "", VbNumRte = "";
                if ((int)ViewState["VentanaRva"] == 0)
                { Vbprioridad = TxtlOtPrioridad.Text.Trim(); VbOTRva = TxtOt.Text; VbCodHK = DdlOTAero.Text; VbNumRte = "0"; }
                else { Vbprioridad = DdlPrioridadOT.Text.Trim(); VbOTRva = TxtRecurSubOt.Text; VbCodHK = DdlAeroRte.Text; VbNumRte = TxtNroRte.Text; }
                int Idx = e.RowIndex;
                int VblId = Convert.ToInt32(GrdOTRecursoF.DataKeys[Idx].Value.ToString());

                string VblPN = (GrdOTRecursoF.Rows[Idx].FindControl("LblOTPn") as Label).Text.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                string VblTxtCant = (GrdOTRecursoF.Rows[Idx].FindControl("LblCantRF") as Label).Text.Trim();
                double VblCant = Convert.ToDouble(VblTxtCant, Culture);
                int VbPosc = Convert.ToInt32((GrdOTRecursoF.Rows[Idx].FindControl("LblPosc") as Label).Text.Trim());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,'','','','','',@ICC,'DELETE',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,@Posc,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.Parameters.AddWithValue("@OT", Convert.ToInt32(VbOTRva));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(VbCodHK));
                                SC.Parameters.AddWithValue("@IdRte", Convert.ToInt32(VbNumRte));
                                SC.Parameters.AddWithValue("@Posc", VbPosc);

                                string Mensj = "OK";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals("OK"))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                TxtOTRecurConsulPn.Text = "";
                                if ((int)ViewState["VentanaRva"] == 0) { TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD"); }//OT
                                else { TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD"); } //Rte
                                BindDOTRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Error en el proceso de eliminación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR RECURSO OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            PerfilesGrid();

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlOTPNRFPP = (e.Row.FindControl("DdlOTPNRFPP") as DropDownList);
                DdlOTPNRFPP.DataSource = DSTOTGrl.Tables[12]; // PN_Rsva;
                DdlOTPNRFPP.DataTextField = "PN";
                DdlOTPNRFPP.DataValueField = "CodPN";
                DdlOTPNRFPP.DataBind();
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if ((int)ViewState["VentanaRva"] == 0)
                {
                    if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
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
                else
                {
                    if (DdlRteEstad.Text.Equals("C") || (int)ViewState["BloquearDetalleRte"] == 1)
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
                if ((int)ViewState["VentanaRva"] == 0)
                {
                    if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
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
                else
                {
                    if (DdlRteEstad.Text.Equals("C") || (int)ViewState["BloquearDetalleRte"] == 1)
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
        }
        protected void GrdOTRecursoF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdOTRecursoF.PageIndex = e.NewPageIndex; BindDOTRecursoF(); PerfilesGrid(); }
        //******************************************  Subir Recurso Carga Maxivamente *********************************************************
        protected void IbtOTCerrarCargMaxivo_Click(object sender, ImageClickEventArgs e)
        { MlVwOT.ActiveViewIndex = 2; }
        protected void IbtOTSubirCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DT = new DataTable();
                string FileName = "";
                string conexion = "";
                //string conexion1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Asus Pro\Downloads\Reportes.xlsx;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
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
                        GrdOTCargaMax.DataSource = DT;
                        GrdOTCargaMax.DataBind();
                        Session["TablaRsvaResul"] = DT;
                    }
                    cnn.Close();
                    List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
                    foreach (GridViewRow Row in GrdOTCargaMax.Rows)
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
                            IdRsva = Convert.ToInt32(TxtOt.Text),
                            Posicion = 0,
                            PN = TxtPNRF.Text.Trim(),
                            Descripcion = TxtDesRF.Text.Trim(),
                            Cantidad = VblCant,
                            UndSolicitada = TxtUMRF.Text.Trim(),
                            UndSistema = TxtUMSysRF.Text.Trim(),
                            IPC = TxtIPCRF.Text.Trim(),
                            Usu = Session["C77U"].ToString(),
                            CodAeronave = Convert.ToInt32(DdlOTAero.Text),
                            Accion = "TEMPORAL",
                        };
                        ObjSubirRsva.Add(TypSubirRsva);
                    }
                    CsTypSubirReserva SubirRsva = new CsTypSubirReserva();

                    SubirRsva.Alimentar(ObjSubirRsva);// 
                    string Mensj = SubirRsva.GetMensj();
                    if (!Mensj.Trim().Equals("OK"))
                    {
                        GrdOTCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                        GrdOTCargaMax.DataBind();
                        IbtOTGuardarCargaMax.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UplOTCargMasiv, UplOTCargMasiv.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    GrdOTCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                    GrdOTCargaMax.DataBind();
                    IbtOTGuardarCargaMax.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Carga Masiva desde OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtOTGuardarCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbOTRva = "", VbCodHK = "", VbNumRte = "";
            if ((int)ViewState["VentanaRva"] == 0)
            { VbOTRva = TxtOt.Text; VbCodHK = DdlOTAero.Text; VbNumRte = "0"; }
            else { VbOTRva = TxtRecurSubOt.Text; VbCodHK = DdlAeroRte.Text; VbNumRte = TxtNroRte.Text; }

            List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
            foreach (GridViewRow Row in GrdOTCargaMax.Rows)
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
                    IdRsva = Convert.ToInt32(VbOTRva),
                    Posicion = 0,
                    PN = TxtPNRF.Text.Trim(),
                    Descripcion = TxtDesRF.Text.Trim(),
                    Cantidad = VblCant,
                    UndSolicitada = TxtUMRF.Text.Trim(),
                    UndSistema = TxtUMSysRF.Text.Trim(),
                    IPC = TxtIPCRF.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    CodAeronave = Convert.ToInt32(VbCodHK),
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
                ScriptManager.RegisterClientScriptBlock(this.UplOTCargMasiv, UplOTCargMasiv.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                IbtOTGuardarCargaMax.Enabled = false;
                return;
            }
            IbtOTGuardarCargaMax.Enabled = false;
            if ((int)ViewState["VentanaRva"] == 0) { TraerDatosBusqOT(Convert.ToInt32(TxtOt.Text), "UPD"); }//OT
            else { TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD"); } //Rte
            BindDOTRecursoF();
            Session["TablaRsvaResul"] = null;
            MlVwOT.ActiveViewIndex = 2;
        }
        //******************************************  Opciones de busqueda OT *********************************************************
        protected void BIndDBusqOT()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                string VbTxtSql = "", VbOpcion = "OT";
                if ((int)ViewState["VentanaBusq"] == 0) // OT  ViewState["VentanaBusq"]
                {
                    CursorIdioma.Alimentar("CurBusqOT", Session["77IDM"].ToString().Trim());
                    if (RdbOTBusqNumOT.Checked == true)
                    { VbOpcion = "OT"; }
                    if (RdbOTBusqSN.Checked == true)
                    { VbOpcion = "SN"; }
                    if (RdbOTBusqPN.Checked == true)
                    { VbOpcion = "PN"; }
                    if (RdbOTBusqHK.Checked == true)
                    { VbOpcion = "HK"; }
                    VbTxtSql = "EXEC SP_PANTALLA_OrdenTrabajo2 8,@Prmtr,'CurBusqOT','','',@Opc,0,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                }
                else
                {  //busqueda Reporte
                    CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                    if (RdbBusqRteNum.Checked == true)
                    { VbOpcion = "RteNum"; }
                    if (RdbBusqRteHk.Checked == true)
                    { VbOpcion = "HK"; }
                    if (RdbBusqRteAta.Checked == true)
                    { VbOpcion = "Ata"; }
                    if (RdbBusqRteTecn.Checked == true)
                    { VbOpcion = "Tecn"; }
                    if (RdbBusqRteDescRte.Checked == true)
                    { VbOpcion = "DescRte"; }
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,'','','CurBusqRte',@Opc,@OTMst,0,0,@ICC,'01-01-1','02-01-1','03-01-1'");
                }
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtOTBusq.Text.Trim()); ;// VbOpcion.Equals("OT") ? TxtOt.Text : TxtOTBusq.Text.Trim()
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@OTMst", TxtOt.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())

                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdOTBusq.DataSource = DtB;
                            GrdOTBusq.DataBind();
                        }
                        else
                        {
                            GrdOTBusq.DataSource = null;
                            GrdOTBusq.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtOTConsultarBusq_Click(object sender, ImageClickEventArgs e)
        { BIndDBusqOT(); }
        protected void IbtOTCerrarBusq_Click(object sender, ImageClickEventArgs e)
        {
            TblOTBusq.Visible = false;
            TblBusqRte.Visible = false;
            MlVwOT.ActiveViewIndex = (int)ViewState["VentanaBusq"];
        }
        protected void IbtOTExpBusqOT_Click(object sender, ImageClickEventArgs e)
        { Exportar(""); }
        protected void GrdOTBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdOTBusq.SelectedRow.Cells[1].Text);
            if ((int)ViewState["VentanaBusq"] == 0) // OT
            { TraerDatosBusqOT(Convert.ToInt32(vbcod), "UPD"); }
            else
            { TraerDatosRtes(Convert.ToInt32(vbcod), "UPD"); }
            MlVwOT.ActiveViewIndex = (int)ViewState["VentanaBusq"];
            PerfilesGrid();
        }
        protected void GrdOTBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdOTBusq.PageIndex = e.NewPageIndex; BIndDBusqOT(); }
        //******************************************  Procedimientos EXPORTAR*********************************************************
        protected void Exportar(string Condcion)
        {
            try
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                string StSql = "", VbNomRpt = "", VbOpcion = "";
                switch (Condcion)
                {
                    case "Reserva":

                        CursorIdioma.Alimentar("CURRESERVA", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto2 6,'CURRESERVA','','','','',@OT,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        VbNomRpt = "Reserve";

                        break;
                    case "ReporteGeneral":
                        CursorIdioma.Alimentar("CurInfomeRte", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto 4,'CurInfomeRte','','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        VbNomRpt = "Report_Maintenance";
                        break;
                    case "OTGeneral":
                        CursorIdioma.Alimentar("CurInfomeOT", Session["77IDM"].ToString().Trim());
                        StSql = "EXEC SP_PANTALLA_OrdenTrabajo 8,'CurInfomeOT','','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        VbNomRpt = "WO";
                        break;
                    case "PasoCloseOTOpen":
                        CursorIdioma.Alimentar("Cur8cumplido", Session["77IDM"].ToString().Trim());
                        StSql = string.Format(" EXEC SP_PANTALLA_OrdenTrabajo 40,'Cur8cumplido','','','',0,0, @Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                        VbNomRpt = "Steps_Completed_OpenWO";
                        break;
                    default:
                        if ((int)ViewState["VentanaBusq"] == 0) // OT
                        {
                            //busqueda OT
                            if (RdbOTBusqNumOT.Checked == true)
                            { VbOpcion = "OT"; }
                            if (RdbOTBusqSN.Checked == true)
                            { VbOpcion = "SN"; }
                            if (RdbOTBusqPN.Checked == true)
                            { VbOpcion = "PN"; }
                            if (RdbOTBusqHK.Checked == true)
                            { VbOpcion = "HK"; }

                            StSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 8, @Prmtr, 'CurBusqOT', '', '', @Opc, 0, 0, 0, @ICC, '01-01-01', '01-01-01', '01-01-01'");
                            VbNomRpt = "W_Order";
                        }
                        else
                        {  //busqueda Reporte
                            VbOpcion = "OT";
                            CursorIdioma.Alimentar("CurBusqRte", Session["77IDM"].ToString().Trim());
                            if (RdbBusqRteNum.Checked == true)
                            { VbOpcion = "RteNum"; }
                            if (RdbBusqRteHk.Checked == true)
                            { VbOpcion = "HK"; }
                            if (RdbBusqRteAta.Checked == true)
                            { VbOpcion = "Ata"; }
                            if (RdbBusqRteTecn.Checked == true)
                            { VbOpcion = "Tecn"; }
                            if (RdbBusqRteDescRte.Checked == true)
                            { VbOpcion = "DescRte"; }
                            StSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 7,@Prmtr,'','','CurBusqRte',@Opc,@OT,0,0,@ICC,'01-01-1','02-01-1','03-01-1'");
                            VbNomRpt = "Report";
                            if (VbOpcion.Equals("OT")) { TxtOTBusq.Text = TxtOt.Text; }
                        }
                        break;
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@OT", TxtOt.Text); // solo cuando es para el reporte                       
                        SC.Parameters.AddWithValue("@Prmtr", TxtOTBusq.Text.Trim()); // solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim()); // solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]); // ID Cia
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
        //******************************************  IMPRESION OT *********************************************************
        protected void IbtOTCerrarPrint_Click(object sender, ImageClickEventArgs e)
        {
            MlVwOT.ActiveViewIndex = (int)ViewState["Ventana"];
        }
        //******************************************  Pasos *********************************************************
        protected void ActivarCamposPaso(bool Edi, bool boton, string Estado)
        {
            DdlPasoEstado.Enabled = Edi;
            if (DdlPasoEstado.Text.Trim().Equals("01") || DdlPasoEstado.Text.Trim().Equals("05"))
            { DdlPasoEstado.Enabled = false; }
            RdbPasoMaManto.Enabled = Edi;
            RdbPasoMaOH.Enabled = Edi;
            RdbPasoSRM.Enabled = Edi;
            RdbPasoEO.Enabled = Edi;
            RdbPasoOTHER.Enabled = Edi;
            CkbPasoOtro.Enabled = CkbPasoOtro.Visible == true ? Edi : false;
            TxtPasoRef.Enabled = Edi;
            TxtPasoDiscrep.Enabled = Edi;
            IbtPasoFI.Enabled = Edi;
            DdlPasoTec.Enabled = Edi; DdlPasoLicTec.Enabled = Edi; TxtPasoHRealTec.Enabled = Edi;
            DdlPasoInsp.Enabled = Edi; DdlPasoLicInsp.Enabled = Edi; TxtPasoHRealInsp.Enabled = Edi;
            TxtPasoNotas.Enabled = Edi;
            if (Convert.ToInt32(ViewState["PasoActual"]) == 2)
            { BtnPasoRepte.Enabled = boton; }
        }
        protected void ValidarCamposPasos()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbCampoRequerido = "";
            DataRow[] Result = Idioma.Select("Objeto= 'MensCampoReq'");
            foreach (DataRow row in Result)
            { VbCampoRequerido = row["Texto"].ToString().Trim(); }
            ViewState["Validar"] = "S";
            if (RdbPasoMaManto.Checked == false && RdbPasoMaOH.Checked == false && RdbPasoSRM.Checked == false && RdbPasoEO.Checked == false && RdbPasoOTHER.Checked == false && CkbPasoOtro.Checked == false)
            {
                if (Convert.ToInt32(ViewState["PasoActual"]) != 7)
                {
                    ViewState["Validar"] = "N";
                    DataRow[] Result2 = Idioma.Select("Objeto= 'Mens28'");
                    foreach (DataRow row in Result2)
                    { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                    return;
                }
            }
            if (TxtPasoRef.Visible == true && TxtPasoRef.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + VbCampoRequerido + "')", true);
                TxtPasoRef.Focus();
                return;
            }
            if (TxtPasoDiscrep.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + VbCampoRequerido + "')", true);
                TxtPasoDiscrep.Focus();
                return;
            }
            if (TxtPasoFecI.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                DataRow[] Result2 = Idioma.Select("Objeto= 'MensFechaI'");
                foreach (DataRow row in Result2)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }//La fecha inicial es requerida.
                return;
            }
            if (Convert.ToDouble(TxtPasoHRealTec.Text.Trim()) <= 0 && TxtPasoHRealTec.Visible == true)
            {
                ViewState["Validar"] = "N";
                ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + VbCampoRequerido + "')", true);
                TxtPasoHRealTec.Focus();
                return;
            }
            if (Convert.ToDouble(TxtPasoHRealInsp.Text.Trim()) <= 0 && TxtPasoHRealInsp.Visible == true)
            {
                ViewState["Validar"] = "N";
                ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + VbCampoRequerido + "')", true);
                TxtPasoHRealInsp.Focus();
                return;
            }
            if (DdlPasoTec.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                DataRow[] Result2 = Idioma.Select("Objeto= 'Mens22'");
                foreach (DataRow row in Result2)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                return;
            }
            if (DdlPasoLicTec.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                DataRow[] Result2 = Idioma.Select("Objeto= 'Mens23'");
                foreach (DataRow row in Result2)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                return;
            }
            if (DdlPasoInsp.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                DataRow[] Result2 = Idioma.Select("Objeto= 'Mens24'");
                foreach (DataRow row in Result2)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                return;
            }
            if (DdlPasoLicInsp.Text.Trim().Equals(""))
            {
                ViewState["Validar"] = "N";
                DataRow[] Result2 = Idioma.Select("Objeto= 'Mens25'");
                foreach (DataRow row in Result2)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                return;
            }
            if (TxtPasoNotas.Visible == true && TxtPasoNotas.Text.Trim().Equals("") && (DdlPasoEstado.Text.Equals("03") || DdlPasoEstado.Text.Equals("04") || DdlPasoEstado.Text.Equals("05")))
            {
                ViewState["Validar"] = "N";
                ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + VbCampoRequerido + "')", true);
                TxtPasoNotas.Focus();
                return;
            }
        }
        protected void DdlEstadoPaso(string estado)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','ESTPASO',{1},{2},0,0,'01-01-01','01-01-01','01-01-01'", estado, ViewState["VblCE2"], ViewState["PasoActual"]);
            DdlPasoEstado.DataSource = Cnx.DSET(LtxtSql);
            DdlPasoEstado.DataMember = "Datos";
            DdlPasoEstado.DataTextField = "NombreESO";
            DdlPasoEstado.DataValueField = "CodEstadoSO";
            DdlPasoEstado.DataBind();
        }
        protected void DdlPasoPersonal()
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;

            if (DSTOTGrl.Tables[7].Rows.Count > 0) //"Inspector / Tecnico"
            {
                DataTable DTInsp = new DataTable();
                DataTable DTTec = new DataTable();

                DataRow[] DR = DSTOTGrl.Tables[7].Select("Estado = 'ACTIVO' AND Inspector = 1");
                if (IsIENumerableLleno(DR))
                { DTInsp = DR.CopyToDataTable(); }
                DTInsp.Rows.Add("7. Persona", "-", "", "1", "ACTIVO", "1");
                Result = DSTOTGrl.Tables[7].Select("CodPersona= '" + ViewState["InsptPsAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTInsp.ImportRow(Row); }

                DdlPasoInsp.DataSource = DTInsp;
                DdlPasoInsp.DataTextField = "Tecnico";
                DdlPasoInsp.DataValueField = "CodPersona";
                DdlPasoInsp.DataBind();
                DdlPasoInsp.Text = ViewState["InsptPsAnt"].ToString().Trim();

                DR = DSTOTGrl.Tables[7].Select("Estado = 'ACTIVO' AND Inspector = 0");
                if (IsIENumerableLleno(DR))
                { DTTec = DR.CopyToDataTable(); }
                DTTec.Rows.Add("7. Persona", "-", "", "1", "ACTIVO", "1");
                Result = DSTOTGrl.Tables[7].Select("CodPersona= '" + ViewState["TecPsAnt"] + "'");
                foreach (DataRow Row in Result)
                { DTTec.ImportRow(Row); }

                DdlPasoTec.DataSource = DTTec;
                DdlPasoTec.DataTextField = "Tecnico";
                DdlPasoTec.DataValueField = "CodPersona";
                DdlPasoTec.DataBind();
                DdlPasoTec.Text = ViewState["TecPsAnt"].ToString().Trim();
            }
        }
        protected void DdlPasoLicPer()
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;

            if (DSTOTGrl.Tables[10].Rows.Count > 0) //"Licencia"
            {
                DataTable DTLT = new DataTable();
                DTLT = DSTOTGrl.Tables[10].Clone();
                DataTable DTLI = new DataTable();
                DTLI = DSTOTGrl.Tables[10].Clone();
                string borr = "Activo = 1 AND CodPersona = '" + ViewState["TecPsAnt"].ToString().Trim() + "' AND Licencia= '" + ViewState["LicTecPsAnt"].ToString().Trim() + "'";
                DataRow[] DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + ViewState["TecPsAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR))
                { DTLT = DR.CopyToDataTable(); }
                DTLT.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + ViewState["LicTecPsAnt"].ToString().Trim() + "' AND CodPersona = '" + ViewState["TecPsAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result)
                { DTLT.ImportRow(Row); }

                DdlPasoLicTec.DataSource = DTLT;
                DdlPasoLicTec.DataTextField = "Licencia";
                DdlPasoLicTec.DataValueField = "Codigo";
                DdlPasoLicTec.DataBind();
                DdlPasoLicTec.Text = ViewState["LicTecPsAnt"].ToString().Trim();

                DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + ViewState["InsptPsAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR))
                { DTLI = DR.CopyToDataTable(); }
                DTLI.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + ViewState["LicInsptPsAnt"].ToString().Trim() + "' AND CodPersona = '" + ViewState["InsptPsAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result)
                { DTLI.ImportRow(Row); }

                DdlPasoLicInsp.DataSource = DTLI;
                DdlPasoLicInsp.DataTextField = "Licencia";
                DdlPasoLicInsp.DataValueField = "Codigo";
                DdlPasoLicInsp.DataBind();
                DdlPasoLicInsp.Text = ViewState["LicInsptPsAnt"].ToString().Trim();
            }
        }
        protected void EstadoPasos()
        {
            switch (ViewState["EP1"].Equals("") ? "01" : ViewState["EP1"])
            {
                case "01":
                    BtnMroInsPre.CssClass = "btn btn-outline-primary";
                    break;
                case "02":
                    BtnMroInsPre.CssClass = "btn btn-danger";
                    break;
                case "03":
                    BtnMroInsPre.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroInsPre.CssClass = "btn btn-warning";
                    break;
                case "05":
                    BtnMroInsPre.CssClass = "btn btn-info";
                    break;
            }
            switch (ViewState["EP2"].Equals("") ? "01" : ViewState["EP2"])
            {
                case "01":
                    BtnMroPrDes.CssClass = "btn btn-outline-primary";
                    break;
                case "02":
                    BtnMroPrDes.CssClass = "btn btn-danger";
                    break;
                case "03":
                    BtnMroPrDes.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroPrDes.CssClass = "btn btn-warning";
                    break;
                case "05":
                    BtnMroPrDes.CssClass = "btn btn-info";
                    break;
            }
            switch (ViewState["EP3"].Equals("") ? "01" : ViewState["EP3"])
            {
                case "01":
                    BtnMroRteDes.CssClass = "btn btn-outline-primary";
                    break;
                case "02":
                    BtnMroRteDes.CssClass = "btn btn-danger";
                    break;
                case "03":
                    BtnMroRteDes.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroRteDes.CssClass = "btn btn-warning";
                    break;
                case "05":
                    BtnMroRteDes.CssClass = "btn btn-info";
                    break;
            }
            switch (ViewState["EP4"].Equals("") ? "01" : ViewState["EP4"])
            {
                case "01":
                    BtnMroDanOc.CssClass = "btn btn-outline-primary"; break;
                case "02":
                    BtnMroDanOc.CssClass = "btn btn-danger"; break;
                case "03":
                    BtnMroDanOc.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroDanOc.CssClass = "btn btn-warning"; break;
                case "05":
                    BtnMroDanOc.CssClass = "btn btn-info"; break;
            }
            switch (ViewState["EP5"].Equals("") ? "01" : ViewState["EP5"])
            {
                case "01":
                    BtnMroAccCorr.CssClass = "btn btn-outline-primary"; break;
                case "02":
                    BtnMroAccCorr.CssClass = "btn btn-danger"; break;
                case "03":
                    BtnMroAccCorr.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroAccCorr.CssClass = "btn btn-warning"; break;
                case "05":
                    BtnMroAccCorr.CssClass = "btn btn-info"; break;
            }
            switch (ViewState["EP6"].Equals("") ? "01" : ViewState["EP6"])
            {
                case "01":
                    BtnMroPrueF.CssClass = "btn btn-outline-primary"; break;
                case "02":
                    BtnMroPrueF.CssClass = "btn btn-danger"; break;
                case "03":
                    BtnMroPrueF.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroPrueF.CssClass = "btn btn-warning"; break;
                case "05":
                    BtnMroPrueF.CssClass = "btn btn-info"; break;
            }
            switch (ViewState["EP7"].Equals("") ? "01" : ViewState["EP7"])
            {
                case "01":
                    BtnMroCumpl.CssClass = "btn btn-outline-primary"; break;
                case "02":
                    BtnMroCumpl.CssClass = "btn btn-danger"; break;
                case "03":
                    BtnMroCumpl.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroCumpl.CssClass = "btn btn-warning"; break;
                case "05":
                    BtnMroCumpl.CssClass = "btn btn-info"; break;
            }
            switch (ViewState["EP8"].Equals("") ? "01" : ViewState["EP8"])
            {
                case "01":
                    BtnMroTrabEje.CssClass = "btn btn-outline-primary"; break;
                case "02":
                    BtnMroTrabEje.CssClass = "btn btn-danger"; break;
                case "03":
                    BtnMroTrabEje.CssClass = "btn btn-secondary";
                    break;
                case "04":
                    BtnMroTrabEje.CssClass = "btn btn-warning"; break;
                case "05":
                    BtnMroTrabEje.CssClass = "btn btn-info"; break;
            }
        }
        protected void LimpiarCamposPasos()
        {
            TxtPasoAplic.Text = "";
            RdbPasoMaManto.Checked = false;
            RdbPasoMaOH.Checked = false;
            RdbPasoSRM.Checked = false;
            RdbPasoEO.Checked = false;
            RdbPasoOTHER.Checked = false;
            CkbPasoOtro.Checked = false;
            TxtPasoRef.Text = "";
            TxtPasoDiscrep.Text = "";
            TxtPasoFecI.Text = "";
            TxtPasoFecF.Text = "";
            DdlPasoTec.Text = "";
            DdlPasoInsp.Text = "";
            DdlPasoLicTec.Text = "";
            DdlPasoLicInsp.Text = "";
            TxtPasoHRealTec.Text = "0";
            TxtPasoHRealInsp.Text = "0";
            TxtPasoNotas.Text = "";
        }
        protected void TraerDatosPasos(string estado)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Accion"] = "";
                if (CkbCancel.Checked == true)
                { BtnPasoAceptar.Enabled = false; BtnPasoRepte.Enabled = false; BtnPasoRepte.ToolTip = ""; }
                else
                {
                    BtnPasoAceptar.Enabled = true; BtnPasoRepte.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto= 'BtnPasoRepte2'");
                    foreach (DataRow row in Result)
                    { BtnPasoRepte.ToolTip = row["Texto"].ToString(); }//Orden de trabajo Cancelada
                }
                DataRow[] Result2 = Idioma.Select("Objeto= 'LblPasoDiscrep'");
                foreach (DataRow row in Result2)
                { LblPasoDiscrep.Text = row["Texto"].ToString().Trim(); }

                Result2 = Idioma.Select("Objeto= 'RdbPasoMaManto'");
                foreach (DataRow row in Result2)
                { RdbPasoMaManto.Text = row["Texto"].ToString().Trim(); }
                Result2 = Idioma.Select("Objeto= 'RdbPasoMaOH'");
                foreach (DataRow row in Result2)
                { RdbPasoMaOH.Text = row["Texto"].ToString().Trim(); }
                Result2 = Idioma.Select("Objeto= 'RdbPasoSRM'");
                foreach (DataRow row in Result2)
                { RdbPasoSRM.Text = row["Texto"].ToString().Trim().PadLeft(10); }
                Result2 = Idioma.Select("Objeto= 'RdbPasoEO'");
                foreach (DataRow row in Result2)
                { RdbPasoEO.Text = row["Texto"].ToString().Trim(); }
                Result2 = Idioma.Select("Objeto= 'RdbPasoOTHER'");
                foreach (DataRow row in Result2)
                { RdbPasoOTHER.Text = row["Texto"].ToString().Trim(); }
                BtnPasoRepte.Visible = true;
                LblPaosoRealizado.Visible = true;
                RdbPasoMaManto.Visible = true; RdbPasoMaOH.Visible = true; RdbPasoSRM.Visible = true; RdbPasoEO.Visible = true; RdbPasoOTHER.Visible = true;
                LblPasoRef.Visible = true; TxtPasoRef.Visible = true;
                LblPasoHRealTec.Visible = true; TxtPasoHRealTec.Visible = true; LblPasoHRealInsp.Visible = true; TxtPasoHRealInsp.Visible = true;
                LblPasoNotas.Visible = true; TxtPasoNotas.Visible = true;
                CkbPasoOtro.Visible = false;
                switch (ViewState["PasoActual"].ToString())
                {
                    case "1":
                        break;
                    case "2":
                        DataRow[] Result3 = Idioma.Select("Objeto= 'LblPasoDiscrep1'");
                        foreach (DataRow row in Result3)
                        { LblPasoDiscrep.Text = row["Texto"].ToString().Trim(); }

                        break;
                    case "3":

                        break;
                    case "4":

                        break;
                    case "5":
                        BtnPasoRepte.Visible = false;
                        break;
                    case "6":

                        break;
                    case "7":
                        LblPaosoRealizado.Visible = false;
                        RdbPasoMaManto.Visible = false; RdbPasoMaOH.Visible = false; RdbPasoSRM.Visible = false; RdbPasoEO.Visible = false; RdbPasoOTHER.Visible = false;
                        LblPasoRef.Visible = false; TxtPasoRef.Visible = false;
                        DataRow[] Result4 = Idioma.Select("Objeto= 'LblPasoDiscrep2'");
                        foreach (DataRow row in Result4)
                        { LblPasoDiscrep.Text = row["Texto"].ToString().Trim(); }
                        break;
                    case "8":
                        LblPasoRef.Visible = false; TxtPasoRef.Visible = false;
                        LblPasoHRealTec.Visible = false; TxtPasoHRealTec.Visible = false; LblPasoHRealInsp.Visible = false; TxtPasoHRealInsp.Visible = false;
                        LblPasoNotas.Visible = false; TxtPasoNotas.Visible = false;
                        CkbPasoOtro.Visible = true;
                        RdbPasoMaManto.Text = "OH"; RdbPasoMaOH.Text = "REP"; RdbPasoSRM.Text = "TEST"; RdbPasoEO.Text = "CAL"; RdbPasoOTHER.Text = "INSP";
                        DataRow[] Result5 = Idioma.Select("Objeto= 'LblPasoDiscrep3'");
                        foreach (DataRow row in Result5)
                        { LblPasoDiscrep.Text = row["Texto"].ToString().Trim(); }
                        break;
                }
                switch (estado)
                {
                    case "01":
                        DdlPasoEstado.Text = "01";
                        BtnPasoRepte.Visible = false;
                        break;
                    case "05":
                        BtnPasoAceptar.Enabled = false;
                        BtnPasoRepte.Enabled = false;
                        break;
                }
                LimpiarCamposPasos();

                TxtPasoAplic.Text = TxtAplicab.Text.Trim();
                DdlPasoEstado.Text = estado.Trim();
                ViewState["IdPasos"] = 0;
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 9,'','','','','',@O,@P,0,@ICC,'01-01-01','01-01-01','01-01-01'");
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlC.Parameters.AddWithValue("@O", TxtOt.Text);
                    SqlC.Parameters.AddWithValue("@P", ViewState["PasoActual"]);
                    SqlC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SqlC.ExecuteReader();
                    if (SDR.Read())
                    {
                        ViewState["IdPasos"] = Convert.ToInt32(HttpUtility.HtmlDecode(SDR["IDPasos"].ToString().Trim()));
                        DdlPasoEstado.Text = HttpUtility.HtmlDecode(SDR["Estado"].ToString().Trim());
                        switch (ViewState["PasoActual"].ToString())
                        {
                            case "1": ViewState["EP1"] = DdlPasoEstado.Text; break;
                            case "2": ViewState["EP2"] = DdlPasoEstado.Text; break;
                            case "3": ViewState["EP3"] = DdlPasoEstado.Text; break;
                            case "4": ViewState["EP4"] = DdlPasoEstado.Text; break;
                            case "5": ViewState["EP5"] = DdlPasoEstado.Text; break;
                            case "6": ViewState["EP6"] = DdlPasoEstado.Text; break;
                            case "7": ViewState["EP7"] = DdlPasoEstado.Text; break;
                            case "8": ViewState["EP8"] = DdlPasoEstado.Text; break;
                        }
                        if (DdlPasoEstado.Text.Equals("05") || DdlOtEstado.Text.Trim().Equals("0002"))
                        { BtnPasoAceptar.Enabled = false; BtnPasoRepte.Enabled = false; }
                        RdbPasoMaManto.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["RManualM"].ToString().Trim()));
                        RdbPasoMaOH.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["RManualOH"].ToString().Trim()));
                        RdbPasoSRM.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["RSRM"].ToString().Trim()));
                        RdbPasoEO.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["REO"].ToString().Trim()));
                        RdbPasoOTHER.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["RO"].ToString().Trim()));
                        CkbPasoOtro.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["Otro"].ToString().Trim()));
                        TxtPasoRef.Text = HttpUtility.HtmlDecode(SDR["DocReferencia"].ToString().Trim());
                        TxtPasoDiscrep.Text = HttpUtility.HtmlDecode(SDR["Discrepancia"].ToString().Trim());
                        TxtPasoFecI.Text = HttpUtility.HtmlDecode(SDR["FechaI"].ToString().Trim());
                        TxtPasoFecF.Text = HttpUtility.HtmlDecode(SDR["FechaF"].ToString().Trim());
                        ViewState["TecPsAnt"] = SDR["CodTecnico"].ToString().Trim();
                        ViewState["InsptPsAnt"] = SDR["CodInspector"].ToString().Trim();
                        DdlPasoPersonal();
                        ViewState["LicTecPsAnt"] = SDR["LicenciaTec"].ToString().Trim();
                        ViewState["LicInsptPsAnt"] = SDR["LicenciaInsp"].ToString().Trim();
                        DdlPasoLicPer();
                        ViewState["UltTec"] = ViewState["TecPsAnt"].ToString().Trim();
                        ViewState["UltLicTec"] = ViewState["LicTecPsAnt"];
                        ViewState["UltInsp"] = ViewState["InsptPsAnt"].ToString().Trim();
                        ViewState["UltLicInsp"] = ViewState["LicInsptPsAnt"].ToString().Trim();
                        TxtPasoHRealTec.Text = HttpUtility.HtmlDecode(SDR["HHRealTec"].ToString().Trim());
                        TxtPasoHRealInsp.Text = HttpUtility.HtmlDecode(SDR["HHRealInsp"].ToString().Trim());
                        TxtPasoNotas.Text = HttpUtility.HtmlDecode(SDR["Notas"].ToString().Trim());
                    }
                    else
                    {
                        ViewState["TecPsAnt"] = ViewState["UltTec"].ToString().Trim();
                        ViewState["InsptPsAnt"] = ViewState["UltInsp"].ToString().Trim();
                        DdlPasoPersonal();
                        ViewState["LicTecPsAnt"] = ViewState["UltLicTec"].ToString().Trim();
                        ViewState["LicInsptPsAnt"] = ViewState["UltLicInsp"].ToString().Trim();
                        DdlPasoLicPer();
                        if (DdlOtEstado.Text.Trim().Equals("0002"))
                        { BtnPasoAceptar.Enabled = false; }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true);//Inconveniente con la consulta
                }
            }
        }
        protected void IbtCerrarPasos_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Accion"] = "";
            LimpiarCamposPasos();
            ActivarCamposPaso(false, true, DdlPasoEstado.Text.Trim());
            DataRow[] Result = Idioma.Select("Objeto= 'BtnPasoAceptar'");
            foreach (DataRow row in Result)
            { BtnPasoAceptar.Text = row["Texto"].ToString().Trim(); }
            BtnPasoAceptar.OnClientClick = "";
            MlVwOT.ActiveViewIndex = 0;
            EstadoPasos();
        }
        protected void DdlPasoTec_TextChanged(object sender, EventArgs e)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;

            if (DSTOTGrl.Tables[10].Rows.Count > 0) //"Licencia"
            {
                DataTable DT = new DataTable();

                DataRow[] dr = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlPasoTec.Text.Trim() + "'");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }
                DT.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + DdlPasoLicTec.Text.Trim() + "' AND CodPersona = '" + DdlPasoTec.Text.Trim() + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlPasoLicTec.DataSource = DT;
                DdlPasoLicTec.DataTextField = "Licencia";
                DdlPasoLicTec.DataValueField = "Codigo";
                DdlPasoLicTec.DataBind();
            }
        }
        protected void DdlPasoInsp_TextChanged(object sender, EventArgs e)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result;
            if (DSTOTGrl.Tables[10].Rows.Count > 0) //"Licencia"
            {
                DataTable DT = new DataTable();

                DataRow[] dr = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlPasoInsp.Text.Trim() + "'");
                if (IsIENumerableLleno(dr))
                { DT = dr.CopyToDataTable(); }
                DT.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + DdlPasoLicInsp.Text.Trim() + "' AND CodPersona = '" + DdlPasoInsp.Text.Trim() + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlPasoLicInsp.DataSource = DT;
                DdlPasoLicInsp.DataTextField = "Licencia";
                DdlPasoLicInsp.DataValueField = "Codigo";
                DdlPasoLicInsp.DataBind();
            }
        }
        protected void BtnPasoAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtOt.Text.Equals(""))
                { return; }
                if (ViewState["Accion"].Equals(""))
                {
                    ViewState["TecPsAnt"] = DdlPasoTec.Text.Trim();
                    ViewState["InsptPsAnt"] = DdlPasoInsp.Text.Trim();
                    DdlPasoPersonal();
                    ViewState["LicTecPsAnt"] = DdlPasoLicTec.Text.Trim();
                    ViewState["LicInsptPsAnt"] = DdlPasoLicInsp.Text.Trim();
                    DdlPasoLicPer();
                    ViewState["Accion"] = "UPDATE";
                    ActivarCamposPaso(true, false, DdlPasoEstado.Text.Trim());
                    DataRow[] Result = Idioma.Select("Objeto= 'BtnPasoAceptar2'");
                    foreach (DataRow row in Result)
                    { BtnPasoAceptar.Text = row["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'ActualizarOnClick'");
                    foreach (DataRow row in Result)
                    { BtnPasoAceptar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la edición?
                }
                else
                {
                    ValidarCamposPasos();
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    int VbRealizado = 0; string DescrpRealizado = "";
                    if (RdbPasoMaManto.Checked == true)
                    { VbRealizado = 1; DescrpRealizado = RdbPasoMaManto.Text.Trim() + " [" + TxtPasoRef.Text.Trim() + "]"; }
                    if (RdbPasoMaOH.Checked == true)
                    { VbRealizado = 2; DescrpRealizado = RdbPasoMaOH.Text.Trim() + " [" + TxtPasoRef.Text.Trim() + "]"; }
                    if (RdbPasoSRM.Checked == true)
                    { VbRealizado = 3; DescrpRealizado = RdbPasoSRM.Text.Trim() + " [" + TxtPasoRef.Text.Trim() + "]"; }
                    if (RdbPasoEO.Checked == true)
                    { VbRealizado = 4; DescrpRealizado = RdbPasoEO.Text.Trim() + " [" + TxtPasoRef.Text.Trim() + "]"; }
                    if (RdbPasoOTHER.Checked == true)
                    { VbRealizado = 5; DescrpRealizado = RdbPasoOTHER.Text.Trim() + " [" + TxtPasoRef.Text.Trim() + "]"; }
                    DateTime? VbFF = null;
                    if (!TxtPasoFecF.Text.Trim().Equals(""))
                    { VbFF = Convert.ToDateTime(TxtPasoFecF.Text); }

                    string StrHT, StrHI;
                    double VbIHT, VbIHI;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHT = TxtPasoHRealTec.Text.Trim().Equals("") ? "0" : TxtPasoHRealTec.Text.Trim();
                    VbIHT = StrHT.Length == 0 ? 0 : Convert.ToDouble(StrHT, Culture);
                    StrHI = TxtPasoHRealInsp.Text.Trim().Equals("") ? "0" : TxtPasoHRealInsp.Text.Trim();
                    VbIHI = StrHI.Length == 0 ? 0 : Convert.ToDouble(StrHI, Culture);

                    List<CsTypPasosOT> ObjPasos = new List<CsTypPasosOT>();
                    var TypPasos = new CsTypPasosOT()
                    {
                        IDPasos = Convert.ToInt32(ViewState["IdPasos"]),
                        Paso = ViewState["PasoActual"].ToString(),
                        OT = Convert.ToInt32(TxtOt.Text),
                        Realizado = VbRealizado,
                        DescripcionRealizado = DescrpRealizado,
                        DocReferencia = TxtPasoRef.Text.Trim(),
                        Discrepancia = TxtPasoDiscrep.Text.Trim(),
                        FechaI = Convert.ToDateTime(TxtPasoFecI.Text),
                        FechaF = VbFF,
                        Estado = DdlPasoEstado.Text.Trim(),
                        HHEst = 0,
                        HHReal = VbIHT,
                        CodTecnico = DdlPasoTec.Text.Trim(),
                        LicenciaTec = DdlPasoLicTec.Text.Trim(),
                        CodInspector = DdlPasoInsp.Text.Trim(),
                        LicenciaInsp = DdlPasoLicInsp.Text.Trim(),
                        Notas = TxtPasoNotas.Text.Trim(),
                        Otro = CkbPasoOtro.Checked == true ? 1 : 0,
                        Usu = Session["C77U"].ToString(),
                        CodLicenciaTecP = "N/A",
                        CodLicenciaInsP = "N/A",
                        HHRealInsp = VbIHI,
                    };
                    ObjPasos.Add(TypPasos);
                    CsTypPasosOT ClsPaso = new CsTypPasosOT();
                    ClsPaso.Alimentar(ObjPasos);
                    string Mensj = ClsPaso.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }

                    DataRow[] Result2 = Idioma.Select("Objeto= 'BtnPasoAceptar'");
                    foreach (DataRow row in Result2)
                    { BtnPasoAceptar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["IdPasos"] = ClsPaso.GetIdPaso().ToString();
                    TraerDatosPasos(DdlPasoEstado.Text.Trim());
                    ActivarCamposPaso(false, true, DdlPasoEstado.Text.Trim());
                    BtnPasoAceptar.OnClientClick = "";
                    ViewState["Accion"] = "";
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 1)
                    { ViewState["P1"] = 1; ViewState["EP1"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 2)
                    { ViewState["P2"] = 1; ViewState["EP2"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 3)
                    { ViewState["P3"] = 1; ViewState["EP3"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 4)
                    { ViewState["P4"] = 1; ViewState["EP4"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 5)
                    { ViewState["P5"] = 1; ViewState["EP5"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 6)
                    { ViewState["P6"] = 1; ViewState["EP6"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 7)
                    { ViewState["P7"] = 1; ViewState["EP7"] = DdlPasoEstado.Text.Trim(); }
                    if (Convert.ToInt32(ViewState["PasoActual"]) == 8)
                    {
                        ViewState["P8"] = 1; ViewState["EP8"] = DdlPasoEstado.Text.Trim();
                        if (DdlPasoEstado.Text.Trim().Equals("05"))
                        {
                            Result2 = Idioma.Select("Objeto= 'Mens32'"); // recuerde cerrar la orden de trabajo
                            foreach (DataRow row in Result2)
                            { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }
                        }
                    }
                    DdlEstadoPaso(DdlPasoEstado.Text.Trim());
                    EstadoPasos();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPasos, UplPasos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Pasos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnPasoRepte_Click(object sender, EventArgs e)
        {
            ViewState["OrigRte"] = "PA";
            ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
            AbrirPantallaRte();
            if (Convert.ToInt32(ViewState["VblIngMSRte"]) == 1)
            { BtnIngresar.Visible = true; }
            MlVwOT.ActiveViewIndex = 7;
        }
        //******************************************  Reporte Manto *********************************************************
        protected void BtnOtReporte_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                ViewState["OrigRte"] = "OT";
                ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
                AbrirPantallaRte();
                if (Convert.ToInt32(ViewState["P1"]) > 0)
                { BtnIngresar.Visible = false; }
                else if (Convert.ToInt32(ViewState["VblIngMSRte"]) == 1)
                { BtnIngresar.Visible = true; }
                TraerDatosRtes(0, "UPD");
                MlVwOT.ActiveViewIndex = 7;
            }
        }
        protected void AbrirPantallaRte()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["CodPrioridad"] = "NORMAL";
            ViewState["Accion"] = "";
            LimpiarCamposRte();
            ActivarBtnRpt(true, true, true, true, true);
            DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
            foreach (DataRow row in Result)
            { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
            Result = Idioma.Select("Objeto= 'BotonMod'");
            foreach (DataRow row in Result)
            { BtnModificar.Text = row["Texto"].ToString().Trim(); }
            Result = Idioma.Select("Objeto= 'MensConfEli'");
            foreach (DataRow row in Result)
            { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
            Result = Idioma.Select("Objeto= 'BtnNotificar3'");
            foreach (DataRow row in Result)
            { BtnNotificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea notificar el reporte?
            ViewState["UsuDefecto"] = "S";
            ActivarCampRte(false, false, "Ingresar");
            DdlBusqRte.Enabled = true;
            BtnIngresar.OnClientClick = "";
            ViewState["VblIngMSRte"] = 1;
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
                /* GrdRecursoF.ShowFooter = false;
                 GrdLicen.ShowFooter = false;
                 GrdSnOnOff.ShowFooter = false;
                 GrdHta.ShowFooter = false;*/
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
            { }
            if (ClsP.GetCE2() == 0)
            { }
            if (ClsP.GetCE3() == 0)
            { }
            if (ClsP.GetCE4() == 0)
            {
                // Notificar
                ViewState["VblCE4Rte"] = 0;
                BtnNotificar.Visible = false;
            }
            if (ClsP.GetCE5() == 0)
            { }
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
            if (DdlOtEstado.Text.Equals("0002"))
            { BtnIngresar.Enabled = false; BtnModificar.Enabled = false; }
        }
        protected void BindDdlRteCondicional(string Categ, string LicGen, string LicCump, string LicVer)
        {
            DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
            DataRow[] Result; String VbCodAnt;

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
                DataRow[] DR = DSTOTGrl.Tables[5].Select("Activo=1");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DT.Rows.Add("", "-", "", "1");
                Result = DSTOTGrl.Tables[5].Select("CodTaller= '" + ViewState["TllAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlTall.DataSource = DT;
                DdlTall.DataTextField = "NomTaller";
                DdlTall.DataValueField = "CodTaller";
                DdlTall.DataBind();
                DdlTall.Text = ViewState["TllAnt"].ToString().Trim();
            }

            VbCodAnt = DdlRteEstad.Text.Trim().Equals("") ? "A" : DdlRteEstad.Text.Trim();
            DdlRteEstad.DataSource = DSTOTGrl.Tables[15];
            DdlRteEstad.DataTextField = "Descripcion";
            DdlRteEstad.DataValueField = "CodStatus";
            DdlRteEstad.DataBind();
            DdlRteEstad.Text = VbCodAnt;

            if (DSTOTGrl.Tables[16].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[16].Clone();
                DataRow[] DR = DSTOTGrl.Tables[16].Select("Activo=1");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DT.Rows.Add("", "", "-", "1");
                Result = DSTOTGrl.Tables[16].Select("Codigo= '" + ViewState["ClsfcnAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlRteClasf.DataSource = DT;
                DdlRteClasf.DataTextField = "Descripcion";
                DdlRteClasf.DataValueField = "Codigo";
                DdlRteClasf.DataBind();
                DdlRteClasf.SelectedValue = ViewState["ClsfcnAnt"].ToString().Trim();
            }

            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{2}',{3},'','CatM',{1},0,0,{4},'01-01-1','02-01-1','03-01-1'",
               DdlRteClasf.Text, DdlRteClasf.SelectedValue.Equals("") ? "0" : DdlOTAero.Text, Categ, Session["77IDM"], Session["!dC!@"]);
            DdlCatgr.DataSource = Cnx.DSET(LtxtSql);
            DdlCatgr.DataTextField = "CodCategoriaMel";
            DdlCatgr.DataValueField = "IdCategoria";
            DdlCatgr.DataBind();

            if (DSTOTGrl.Tables[17].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DT = DSTOTGrl.Tables[17].Clone();
                DataRow[] DR = DSTOTGrl.Tables[17].Select("Activo=1");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DT.Rows.Add("", "", "-", "1");
                Result = DSTOTGrl.Tables[17].Select("Codigo= '" + ViewState["PscnAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

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

                /* LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}',{2},'','LICTA',0,0,0,{3},'01-01-1','02-01-1','03-01-1'", ViewState["GnrdAnt"].ToString().Trim(), LicGen, Session["77IDM"], Session["!dC!@"]);
                 DdlLicGene.DataSource = Cnx.DSET(LtxtSql);
                 DdlLicGene.DataTextField = "Licencia";
                 DdlLicGene.DataValueField = "Codigo";
                 DdlLicGene.DataBind();

                 LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}',{2},'','LICTA',0,0,0,{3},'01-01-1','02-01-1','03-01-1'", ViewState["CmplAnt"].ToString().Trim(), LicCump, Session["77IDM"], Session["!dC!@"]);
                 DdlLicCump.DataSource = Cnx.DSET(LtxtSql);
                 DdlLicCump.DataTextField = "Licencia";
                 DdlLicCump.DataValueField = "Codigo";
                 DdlLicCump.DataBind();

                 LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','{1}',{2},'','LICTA',0,0,0,{3},'01-01-1','02-01-1','03-01-1'", ViewState["VrfcAnt"].ToString().Trim(), LicVer, Session["77IDM"], Session["!dC!@"]);
                 DdlLicVer.DataSource = Cnx.DSET(LtxtSql);
                 DdlLicVer.DataTextField = "Licencia";
                 DdlLicVer.DataValueField = "Codigo";
                 DdlLicVer.DataBind();*/
            }

            if (DSTOTGrl.Tables[10].Rows.Count > 0) //"Licencia"
            {
                DataTable DTG = new DataTable();
                DTG = DSTOTGrl.Tables[10].Clone();
                DataRow[] DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + ViewState["GnrdAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTG = DR.CopyToDataTable(); }
                DTG.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + LicGen.Trim() + "' AND CodPersona = '" + ViewState["GnrdAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTG.ImportRow(Row); }
                DdlLicGene.DataSource = DTG;
                DdlLicGene.DataTextField = "Licencia";
                DdlLicGene.DataValueField = "Codigo";
                DdlLicGene.DataBind();

                DataTable DTC = new DataTable();
                DTC = DSTOTGrl.Tables[10].Clone();
                DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + ViewState["CmplAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTC = DR.CopyToDataTable(); }
                DTC.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + LicCump.Trim() + "' AND CodPersona = '" + ViewState["CmplAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTC.ImportRow(Row); }
                DdlLicCump.DataSource = DTC;
                DdlLicCump.DataTextField = "Licencia";
                DdlLicCump.DataValueField = "Codigo";
                DdlLicCump.DataBind();

                DataTable DTV = new DataTable();
                DTV = DSTOTGrl.Tables[10].Clone();
                DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + ViewState["VrfcAnt"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR)) { DTV = DR.CopyToDataTable(); }
                DTV.Rows.Add("10. licencias", "-", "", "1", "");
                Result = DSTOTGrl.Tables[10].Select("Licencia= '" + LicVer.Trim() + "' AND CodPersona = '" + ViewState["VrfcAnt"].ToString().Trim() + "'");
                foreach (DataRow Row in Result) { DTV.ImportRow(Row); }
                DdlLicVer.DataSource = DTV;
                DdlLicVer.DataTextField = "Licencia";
                DdlLicVer.DataValueField = "Codigo";
                DdlLicVer.DataBind();
            }

            if (DSTOTGrl.Tables[6].Rows.Count > 0) //"Base"
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTOTGrl.Tables[6].Select("Activo=1");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DT.Rows.Add("", "-", "", "1");
                Result = DSTOTGrl.Tables[6].Select("CodBase= '" + ViewState["BaseAnt"] + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlBasRte.DataSource = DT;
                DdlBasRte.DataTextField = "NomBase";
                DdlBasRte.DataValueField = "CodBase";
                DdlBasRte.DataBind();
                DdlBasRte.SelectedValue = ViewState["BaseAnt"].ToString().Trim();
            }

            VbCodAnt = DdlPnRte.Text.Trim();
            DdlPnRte.DataSource = DSTOTGrl.Tables[12];
            DdlPnRte.DataTextField = "PN";
            DdlPnRte.DataValueField = "CodPN";
            DdlPnRte.DataBind();
            DdlPnRte.Text = VbCodAnt;
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
                        string VblePaso = ViewState["OrigRte"].ToString().Equals("PA") ? ViewState["PasoActual"].ToString() : "";
                        using (SqlCommand SC = new SqlCommand("EXEC SP_PANTALLA_Reporte_Manto2 2,@OT,@Ps,'','OT','',@Rt,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'", Cnx2))
                        {
                            SC.Parameters.AddWithValue("@OT", TxtOt.Text.Trim());
                            SC.Parameters.AddWithValue("@Ps", VblePaso);
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
                                    DSTRTE.Tables[10].TableName = "Hrrts";
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
                    string VbFecha;
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
                    BindDdlRteCondicional(VbCodCat, VbLicGen, VbLicCump, VbLicVer);

                    DdlAeroRte.Text = DSTRTE.Tables[0].Rows[0]["CodAeronave"].ToString();
                    TxtNroRte.Text = DSTRTE.Tables[0].Rows[0]["NumReporte"].ToString();
                    TxtConsTall.Text = DSTRTE.Tables[0].Rows[0]["ConsecutivoROTP"].ToString().Trim();
                    DdlFuente.SelectedValue = DSTRTE.Tables[0].Rows[0]["Fuente"].ToString().Trim();
                    TxtCas.Text = DSTRTE.Tables[0].Rows[0]["NumCasilla"].ToString();
                    DdlRteEstad.SelectedValue = DSTRTE.Tables[0].Rows[0]["Estado"].ToString().Trim();
                    CkbNotif.Checked = Convert.ToBoolean(DSTRTE.Tables[0].Rows[0]["Notificado"].ToString());
                    BtnNotificar.Enabled = CkbNotif.Checked == true ? false : true;
                    DdlCatgr.SelectedValue = VbCodCat;
                    TxtDocRef.Text = DSTRTE.Tables[0].Rows[0]["DocumentoRef"].ToString().Trim();
                    DdlAtaRte.SelectedValue = DSTRTE.Tables[0].Rows[0]["UbicacionTecnica"].ToString().Trim();
                    DdlGenerado.SelectedValue = ViewState["GnrdAnt"].ToString().Trim();
                    DdlLicGene.SelectedValue = VbLicGen;
                    VbFecha = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["FechaReporte"].ToString().Trim());
                    TxtRteFecDet.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
                    VbFecha = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["FechaProyectada"].ToString().Trim());
                    TxtFecPry.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
                    TxtRteOt.Text = DSTRTE.Tables[0].Rows[0]["OtPrincipal"].ToString().Trim();
                    DdlCumpl.SelectedValue = ViewState["CmplAnt"].ToString().Trim();
                    DdlLicCump.SelectedValue = VbLicCump;
                    VbFecha = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["FechaCumplimiento"].ToString().Trim());
                    TxtFecCump.Text = VbFecha.Trim().Equals("") ? "" : String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(VbFecha));
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
                    ViewState["IDMroRepOT"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["IDMroRepOT"].ToString());
                    ViewState["BloquearDetalleRte"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["BloquearDetalle"].ToString());
                    ViewState["TtlRegDet"] = Convert.ToInt32(DSTRTE.Tables[0].Rows[0]["TtlRegDet"].ToString());
                    TxtNumPaso.Text = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["PasoOT"].ToString().Trim());
                    // ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(DSTRTE.Tables[0].Rows[0]["CargaMasiva"].ToString().Trim());                   
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
        protected void IbtCerrarRte_Click(object sender, ImageClickEventArgs e)
        { MlVwOT.ActiveViewIndex = (int)ViewState["Ventana"]; ViewState["Accion"] = ""; }
        protected void ActivarCampRte(bool Ing, bool Edi, string accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlRteEstad.SelectedValue.Equals("C") && DdlTipRte.Enabled == false)
            {
                if (Convert.ToInt32(ViewState["VblCE6Rte"]) == 1)
                {
                    DdlRteEstad.Enabled = Edi;
                    if (DdlVerif.Text.Equals(""))
                    {
                        DdlVerif.Text = Session["C77U"].ToString().Trim();

                        DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
                        DataTable DTV = new DataTable();
                        DTV = DSTOTGrl.Tables[10].Clone();
                        DataRow[] DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlVerif.Text.Trim() + "'");
                        if (IsIENumerableLleno(DR)) { DTV = DR.CopyToDataTable(); }
                        DTV.Rows.Add("10. licencias", "-", "", "1", "");
                        DataRow[] Result = DSTOTGrl.Tables[10].Select("Licencia= '' AND CodPersona = '" + DdlVerif.Text.Trim() + "'");
                        foreach (DataRow Row in Result) { DTV.ImportRow(Row); }
                        DdlLicVer.DataSource = DTV;
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
                DdlRteEstad.Enabled = Edi;
                DdlTipRte.Enabled = Edi;
                DdlFuente.Enabled = Edi;
                DdlTall.Enabled = Edi;
                DdlRteClasf.Enabled = Edi;
                DdlCatgr.Enabled = Edi;
                TxtDocRef.Enabled = Edi;
                DdlPosRte.Enabled = Edi;
                DdlAtaRte.Enabled = Edi;
                DdlGenerado.Enabled = ViewState["UsuDefecto"].Equals("S") ? false : Edi;
                DdlLicGene.Enabled = Edi;
                IbtFecDet.Enabled = Edi;
                IbtFecPry.Enabled = Edi;
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
                    {
                        string VbMnsjIdm = "";
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens23'");
                        foreach (DataRow row in Result)
                        { VbMnsjIdm = row["Texto"].ToString().Trim(); }
                        DdlPnRte.ToolTip = VbMnsjIdm; TxtSnRte.ToolTip = VbMnsjIdm;
                    }
                    if (DdlPnRte.Text.Trim().Equals("") && !TxtRteOt.Text.Trim().Equals("0") && TxtOt.Text.Trim().Equals("") && ViewState["ESTAPPT"].ToString().Equals("N"))
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
            DdlRteEstad.Text = "A";
            CkbNotif.Checked = false;
            DdlRteClasf.Text = "";
            DdlCatgr.Text = "";
            TxtDocRef.Text = "";
            DdlPosRte.Text = "";
            DdlAtaRte.Text = "";
            DdlGenerado.Text = "";
            DdlLicGene.Text = "";
            TxtRteFecDet.Text = "";
            TxtFecPry.Text = "";
            TxtRteOt.Text = "0";
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
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una aeronave o P/N
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
                if (DdlRteClasf.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens05'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una clasificación')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlCatgr.Text.Trim().Equals("") && (DdlRteClasf.Text.Trim().Equals("CARRY OVER") || DdlRteClasf.Text.Trim().Equals("CARRY FORWARD")))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens06'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una categoría')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtDocRef.Text.Trim().Equals("") && DdlRteClasf.Text.Trim().Equals("CARRY OVER"))
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
                if (TxtRteFecDet.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una fecha')", true);
                    ViewState["Validar"] = "N";
                    TxtRteFecDet.Focus();
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
                if (DdlCumpl.Text.Trim().Equals("") && DdlRteEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens14'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar el usuario que cierra el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlLicCump.Text.Trim().Equals("") && DdlRteEstad.SelectedValue.Equals("C"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens15'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la licencia del usuario que cierra el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (TxtFecCump.Text.Trim().Equals("") && DdlRteEstad.SelectedValue.Equals("C"))
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
                if (DdlRteEstad.Text.Equals("A") && !txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens18'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe cerrar el reporte')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlRteEstad.Text.Equals("C") && txtAccCrr.Text.Equals("") && DdlTipRte.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar la acción correctiva')", true);
                    ViewState["Validar"] = "N";
                    txtAccCrr.Focus();
                    return;
                }
                if (DdlRteEstad.Text.Equals("A") && !TxtFecCump.Text.Equals("") && DdlTipRte.Enabled == true)
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
                if (TxtAcciParc.Text.Equals("") && (DdlRteClasf.Text.Trim().Equals("CARRY OVER") || DdlRteClasf.Text.Trim().Equals("CARRY FORWARD")))
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
                if (ViewState["PermiteFechaIgualDetPry"].Equals("N") && TxtRteFecDet.Text == TxtFecPry.Text && DdlRteClasf.Text.Trim().Equals("CARRY FORWARD"))
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
                    DateTime VbProy = Convert.ToDateTime(TxtRteFecDet.Text).AddDays(VbCritDias);
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
        { TraerDatosRtes(Convert.ToInt32(DdlBusqRte.SelectedValue), "UPD"); PerfilesGrid(); }
        protected void DdlRteEstad_TextChanged(object sender, EventArgs e)
        {
            if (DdlTipRte.Enabled == true)
            {
                DSTOTGrl = (DataSet)ViewState["DSTOTGrl"];
                if (DdlRteEstad.SelectedValue.Equals("C"))
                {
                    DdlCumpl.SelectedValue = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlCumpl.Text.Trim();
                    DdlLicCump.Text = "";

                    DataTable DT = new DataTable();
                    DT = DSTOTGrl.Tables[10].Clone();
                    DataRow[] DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlCumpl.Text.Trim() + "'");
                    if (IsIENumerableLleno(DR)) { DT = DR.CopyToDataTable(); }
                    DT.Rows.Add("10. licencias", "-", "", "1", "");
                    DataRow[] Result = DSTOTGrl.Tables[10].Select("Licencia= '' AND CodPersona = '" + DdlCumpl.Text.Trim() + "'");
                    foreach (DataRow Row in Result)
                    { DT.ImportRow(Row); }
                    DdlLicCump.DataSource = DT;
                    DdlLicCump.DataTextField = "Licencia";
                    DdlLicCump.DataValueField = "Codigo";
                    DdlLicCump.DataBind();
                }
                else
                {
                    if (BtnIngresar.Text.Equals("Aceptar"))
                    { DdlGenerado.SelectedValue = ViewState["UsuDefecto"].Equals("S") ? Session["C77U"].ToString() : DdlGenerado.Text.Trim(); DdlLicGene.Text = ""; }

                    DataTable DT = new DataTable();
                    DT = DSTOTGrl.Tables[10].Clone();
                    DataRow[] DR = DSTOTGrl.Tables[10].Select("Activo = 1 AND CodPersona = '" + DdlGenerado.Text.Trim() + "'");
                    if (IsIENumerableLleno(DR)) { DT = DR.CopyToDataTable(); }
                    DT.Rows.Add("10. licencias", "-", "", "1", "");
                    DataRow[] Result = DSTOTGrl.Tables[10].Select("Licencia= '' AND CodPersona = '" + DdlGenerado.Text.Trim() + "'");
                    foreach (DataRow Row in Result) { DT.ImportRow(Row); }
                    DdlLicGene.DataSource = DT;
                    DdlLicGene.DataTextField = "Licencia";
                    DdlLicGene.DataValueField = "Codigo";
                    DdlLicGene.DataBind();
                }
            }
            else
            {
                if (DdlRteEstad.SelectedValue.Equals("A"))
                { DdlVerif.Text = ""; DdlLicVer.Text = ""; }
            }
        }
        protected void DdlRteClasf_TextChanged(object sender, EventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','CatM',{1},0,0,{2},'01-01-1','02-01-1','03-01-1'", DdlRteClasf.Text, DdlOTAero.Text, Session["!dC!@"]);
            DdlCatgr.DataSource = Cnx.DSET(LtxtSql);
            DdlCatgr.DataMember = "Datos";
            DdlCatgr.DataTextField = "CodCategoriaMel";
            DdlCatgr.DataValueField = "IdCategoria";
            DdlCatgr.DataBind();
            DdlCatgr.Text = "";
            if (DdlRteClasf.Text.Equals("CARRY OVER"))
            { IbtFecPry.Enabled = false; }
            else
            { IbtFecPry.Enabled = true; }
        }
        protected void DdlCatgr_TextChanged(object sender, EventArgs e)
        {
            if (!DdlCatgr.Text.Equals(""))
            { CalcularFechaPry(); }
        }
        protected void TxtRteFecDet_TextChanged(object sender, EventArgs e)
        {
            CldFecPry.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
            CldFecCump.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
            if (DdlCatgr.Text.Equals(""))
            { TxtFecPry.Text = TxtRteFecDet.Text; }
            else { CalcularFechaPry(); }
            TxtFecCump.Text = "";
        }
        protected void TxtTtlAKSN_TextChanged(object sender, EventArgs e)
        { CalcularNexDue(TxtTtlAKSN.Text, TxtHPrxCu.Text); TxtHPrxCu.Focus(); }
        protected void TxtHPrxCu_TextChanged(object sender, EventArgs e)
        { CalcularNexDue(TxtTtlAKSN.Text, TxtHPrxCu.Text); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (ViewState["Accion"].Equals(""))
                {
                    ActivarBtnRpt(true, false, false, false, false);
                    ViewState["Accion"] = "INSERT";
                    LimpiarCamposRte();
                    DdlAeroRte.Text = DdlOTAero.Text;
                    TxtRteFecDet.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    TxtFecPry.Text = TxtRteFecDet.Text;
                    CldFecPry.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
                    CldFecCump.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
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
                    BindDdlRteCondicional("", "", "", "");
                    DdlBusqRte.SelectedValue = "0";
                    DdlBusqRte.Enabled = false;
                    ViewState["PasoOT"] = ViewState["OrigRte"].ToString().Equals("PA") ? ViewState["PasoActual"].ToString() : "";
                    ViewState["CodPrioridad"] = "";
                    ViewState["BloquearDetalleRte"] = 0;
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'MensConfIng'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                    TxtRteOt.Text = TxtOt.Text.Trim();
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
                        FechaReporte = Convert.ToDateTime(TxtRteFecDet.Text),
                        FechaInicio = null,
                        FechaVerificacion = null,
                        CodIdLvDetManto = 0,
                        CodLibroVuelo = "",
                        CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                        NumCasilla = "0",
                        Reporte = TxtDescRte.Text.Trim(),
                        AccionCorrectiva = txtAccCrr.Text.Trim(),
                        CodTecnico = DdlCumpl.SelectedValue,
                        CodClaseReporteManto = DdlFuente.Text.Trim(),
                        CodClasifReporteManto = DdlRteClasf.Text.Trim(),
                        CodCategoriaMel = DdlCatgr.Text.Trim(),
                        CodStatus = DdlRteEstad.Text.Trim(),
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
                        CodOt = Convert.ToInt32(TxtOt.Text),
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
                    ActivarCampRte(false, false, "Ingresar");

                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(ClsLvDetManto.GetCodIdRte(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    ViewState["Accion"] = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR REPORTE Desde OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNroRte.Text.Equals("0"))
                { return; }
                if (ViewState["Accion"].Equals(""))
                {
                    string VblLicGenAnt, VbLicCumpAnt, VbLicVerif, VblCat;
                    VblLicGenAnt = DdlLicGene.Text;
                    VbLicCumpAnt = DdlLicCump.Text;
                    VblCat = DdlCatgr.Text;
                    VbLicVerif = DdlLicVer.Text;
                    ViewState["TipRteAnt"] = DdlTipRte.Text.Trim();
                    ViewState["TllAnt"] = DdlTall.Text.Trim();
                    ViewState["ClsfcnAnt"] = DdlRteClasf.Text.Trim();
                    ViewState["PscnAnt"] = DdlPosRte.Text.Trim();
                    ViewState["GnrdAnt"] = DdlGenerado.Text.Trim();
                    ViewState["CmplAnt"] = DdlCumpl.Text.Trim();
                    ViewState["DfrAnt"] = DdlTecDif.Text.Trim();
                    ViewState["VrfcAnt"] = DdlVerif.Text.Trim();
                    BindDdlRteCondicional(DdlCatgr.Text, VblLicGenAnt, VbLicCumpAnt, VbLicVerif);

                    DdlLicGene.Text = VblLicGenAnt;
                    DdlLicCump.Text = VbLicCumpAnt;
                    DdlTipRte.Text = ViewState["TipRteAnt"].ToString().Trim();
                    DdlCatgr.Text = VblCat;
                    DdlLicVer.Text = VbLicVerif;

                    DdlGenerado.Text = ViewState["GnrdAnt"].ToString().Trim();
                    DdlCumpl.Text = ViewState["CmplAnt"].ToString().Trim();
                    DdlTecDif.Text = ViewState["DfrAnt"].ToString().Trim();
                    DdlVerif.Text = ViewState["VrfcAnt"].ToString().Trim();
                    ActivarBtnRpt(false, true, false, false, false);
                    ViewState["Accion"] = "UPDATE";
                    DataRow[] Result1 = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result1)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result1)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la edición?                
                    ActivarCampRte(true, true, "UPDATE");
                    DdlBusqRte.SelectedValue = "0";
                    DdlBusqRte.Enabled = false;
                    CldFecCump.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
                    CldFecPry.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
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
                        FechaReporte = Convert.ToDateTime(TxtRteFecDet.Text),
                        FechaInicio = null,
                        FechaVerificacion = null,
                        CodIdLvDetManto = Convert.ToInt32(TxtNroRte.Text),
                        CodLibroVuelo = "",
                        CodAeronave = Convert.ToInt32(DdlAeroRte.Text),
                        NumCasilla = TxtCas.Text.Trim(),
                        Reporte = TxtDescRte.Text.Trim(),
                        AccionCorrectiva = txtAccCrr.Text.Trim(),
                        CodTecnico = DdlCumpl.SelectedValue,
                        CodClaseReporteManto = DdlFuente.Text.Trim(),
                        CodClasifReporteManto = DdlRteClasf.Text.Trim(),
                        CodCategoriaMel = DdlCatgr.Text.Trim(),
                        CodStatus = DdlRteEstad.Text.Trim(),
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
                        CodOt = Convert.ToInt32(TxtRteOt.Text),
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
                    DataRow[] Result3 = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result3)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampRte(false, false, "UPDATE");
                    DdlBusqRte.Enabled = true;
                    TraerDatosRtes(Convert.ToInt32(TxtNroRte.Text), "UPD");
                    BtnModificar.OnClientClick = "";
                    ViewState["Accion"] = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result4 = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result4)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Inconveniente en la actualización')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR REPORTE OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals("S"))
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "')", true);
                                return;
                            }
                            Transac.Commit();
                            LimpiarCamposRte();
                            DdlBusqRte.Text = "0";
                            TraerDatosRtes(0, "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlRte, UpPnlRte.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Reporte Manto OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnNotificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNroRte.Text.Equals("0"))
                { return; }
                if (DdlRteEstad.Text.Equals("A"))
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
                        string VBQuery = string.Format("EXEC SP_TablasManto 3,@Usu,'','','','','','','','','','','','','','',@Rte,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'");
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
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Notificar Rte OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Notificar Rte OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnExporRte_Click(object sender, EventArgs e)
        { Exportar("ReporteGeneral"); }
        protected void BtnReserva_Click(object sender, EventArgs e)
        {
            if (!TxtNroRte.Text.Equals("0"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'{0}','','','','PRIO',0,0,0,{1},'01-01-1','02-01-1','03-01-1'", ViewState["CodPrioridad"].ToString(), Session["!dC!@"]);
                DdlPrioridadOT.DataSource = Cnx.DSET(LtxtSql);
                DdlPrioridadOT.DataMember = "Datos";
                DdlPrioridadOT.DataTextField = "Descripcion";
                DdlPrioridadOT.DataValueField = "CodPrioridadSolicitudMat";
                DdlPrioridadOT.DataBind();
                LblRecFRte.Visible = true;
                TxtRecurNumRte.Visible = true;
                LblRecFSubOt.Visible = true;
                TxtRecurSubOt.Visible = true;
                LblPrioridadOT.Visible = true;
                DdlPrioridadOT.Visible = true;
                LblTitLicencia.Visible = true;
                GrdLicen.Visible = true;
                TxtRecurNumRte.Text = TxtNroRte.Text;
                TxtRecurSubOt.Text = TxtOtSec.Text;
                ViewState["VentanaRva"] = MlVwOT.ActiveViewIndex;
                DdlPrioridadOT.Text = ViewState["CodPrioridad"].ToString().Trim();
                if (DdlRteEstad.Text.Equals("C") || (int)ViewState["BloquearDetalleRte"] == 1)
                {
                    DdlPrioridadOT.Enabled = false; BtnOTCargaMasiva.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens21'");
                    foreach (DataRow row in Result)
                    { BtnOTCargaMasiva.ToolTip = row["Texto"].ToString(); }// "La orden debe estar abierta y no deben existir registros en la reserva" 
                }
                else
                {
                    BtnOTCargaMasiva.Enabled = true; DdlPrioridadOT.Enabled = true;
                    DataRow[] Result1 = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                    foreach (DataRow row in Result1)
                    { BtnOTCargaMasiva.ToolTip = row["Texto"].ToString() + " " + ViewState["CarpetaCargaMasiva"].ToString() + "CargaMasiva.xlsx"; }
                }
                BindDOTRecursoF();
                BindDLicencia();
                PerfilesGrid();
                MlVwOT.ActiveViewIndex = 2;
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result = Idioma.Select("Objeto= 'LblTitOTOpcBusqueda'");
            foreach (DataRow row in Result)
            { LblTitOTOpcBusqueda.Text = row["Texto"].ToString(); }
            TblBusqRte.Visible = true;
            GrdOTBusq.DataSource = null;
            GrdOTBusq.DataBind();
            ViewState["VentanaBusq"] = MlVwOT.ActiveViewIndex;
            MlVwOT.ActiveViewIndex = 4;
        }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            MlVwOT.ActiveViewIndex = 9;
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
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnSnOnOf_Click(object sender, EventArgs e)
        {
            if (TxtNroRte.Text.Equals("0"))
            { return; }
            TxtSnOnOffNumRte.Text = TxtNroRte.Text;
            BindDSnOnOff();
            BindDHta();
            PerfilesGrid();
            MlVwOT.ActiveViewIndex = 8;
        }
        //******************************************  SN On Off *********************************************************
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarSnOnOff_Click(object sender, ImageClickEventArgs e)
        { MlVwOT.ActiveViewIndex = 7; }
        protected void DdlPNOn_TextChanged(object sender, EventArgs e)
        {
            ListBox LtbSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("LtbSNOn") as ListBox); // El indice se toma en el evento RowEditing
            TextBox TxtDescElem = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtDescElem") as TextBox);
            TextBox TxtSNOn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("TxtSNOn") as TextBox);
            string VbPn = (GrdSnOnOff.Rows[(int)ViewState["Index"]].FindControl("DdlPNOn") as DropDownList).Text.Trim();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 10,@P,'','','','',0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
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
                    DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una fecha
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
                                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
            if ((GrdSnOnOff.Rows[Idx].FindControl("TxtFec") as TextBox).Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens11'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una fecha')", true);
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
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//Debe ingresar una P/N ON o OFF')", true);
                return;
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
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE SN ON OFF OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
                TxtFecPP.Text = TxtRteFecDet.Text;
                CalendarExtender CalFechPP = (e.Row.FindControl("CalFechPP") as CalendarExtender);
                DateTime DiaI = Convert.ToDateTime(TxtOTFechReg.Text);
                CalFechPP.StartDate = Convert.ToDateTime(TxtFecPP.Text);
                CalFechPP.EndDate = DateTime.Now;

                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlRteEstad.Text.Equals("C"))
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

                CalendarExtender CalFech = (e.Row.FindControl("CalFech") as CalendarExtender);
                DateTime DiaI = Convert.ToDateTime(TxtRteFecDet.Text);
                CalFech.StartDate = Convert.ToDateTime(TxtRteFecDet.Text);
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
                if (DdlRteEstad.Text.Equals("C"))
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDHta OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
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
        protected void LtbSNHta_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox TxtSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("TxtSNHta") as TextBox);
            ListBox LtbSNHta = (GrdHta.Rows[(int)ViewState["Index"]].FindControl("LtbSNHta") as ListBox);
            TxtSNHta.Text = LtbSNHta.SelectedValue.Trim();
            LtbSNHta.Visible = false;
            PerfilesGrid();
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
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Herramientas en Reportes OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
            if ((GrdHta.Rows[Idx].FindControl("TxtFecVce") as TextBox).Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'RteMens32'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }//La fecha se encuetra vacía')", true);
                return;
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
                return;
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
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                            { ScriptManager.RegisterClientScriptBlock(this.UplSnOnOff, UplSnOnOff.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Herramienta Rte OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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

                CalendarExtender CalFechVcePP = (e.Row.FindControl("CalFechVcePP") as CalendarExtender);
                CalFechVcePP.StartDate = DateTime.Now;

                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlRteEstad.Text.Equals("C"))
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
                if (DdlRteEstad.Text.Equals("C"))
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
        //******************************************  Licencia de la reserva *********************************************************
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

                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens01'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "')", true); }   //Debe ingresar una licencia')", true);
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
                                        ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
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
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'"); foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                            { ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
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
                if (DdlRteEstad.Text.Equals("C") || (int)ViewState["BloquearDetalleRte"] == 1)
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
                if (DdlRteEstad.Text.Equals("C") || (int)ViewState["BloquearDetalleRte"] == 1)
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
        //******************************************  Pasos Cerrados ot abiertas *********************************************************
        protected void BIndDPasoCOTA()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

                CursorIdioma.Alimentar("Cur8cumplido", Session["77IDM"].ToString().Trim());

                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_OrdenTrabajo 40,'Cur8cumplido','','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'");

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]); // ID Cia
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            Grd8PasoCOTOpen.DataSource = DtB;
                            Grd8PasoCOTOpen.DataBind();
                        }
                        else
                        {
                            Grd8PasoCOTOpen.DataSource = null;
                            Grd8PasoCOTOpen.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtCerrarImpresion_Click(object sender, ImageClickEventArgs e)
        { MlVwOT.ActiveViewIndex = 7; }
        protected void IbtCerrarOT8PasoClose_Click(object sender, ImageClickEventArgs e)
        { MlVwOT.ActiveViewIndex = 0; }
        protected void IbtExportarOT8PasoClose_Click(object sender, ImageClickEventArgs e)
        { Exportar("PasoCloseOTOpen"); }
        protected void Grd8PasoCOTOpen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(Grd8PasoCOTOpen.SelectedRow.Cells[3].Text);
            TraerDatosBusqOT(Convert.ToInt32(vbcod), "UPD");
            MlVwOT.ActiveViewIndex = 0;
        }
        protected void Grd8PasoCOTOpen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { Grd8PasoCOTOpen.PageIndex = e.NewPageIndex; BIndDPasoCOTA(); }
    }
}