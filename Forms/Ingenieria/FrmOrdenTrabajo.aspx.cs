using _77NeoWeb.prg;
using _77NeoWeb.Prg;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            /* if (Session["Login77"] == null)
             {
                 Response.Redirect("~/FrmAcceso.aspx");
             }*/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            Page.Title = string.Format("Orden de Trabajo");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";
                Session["77IDM"] = "5"; // 4 español | 5 ingles
                ViewState["Validar"] = "S";/**/
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Orden de Trabajo";
                MlVwOT.ActiveViewIndex = 0;
                BindBDdlBusqOT();
                BindDdlOTCondicional("", "", "", "");
                DdlLicInsp("", "");
                ViewState["EstadoOT"] = "";
                ViewState["Index"] = 0;
                ViewState["CodPrioridad"] = "NORMAL";
                ViewState["Ventana"] = 0;
                ModSeguridad();
                PerfilesGrid();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }

        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;

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
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnOtModificar.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
            }
            if (ClsP.GetImprimir() == 0)
            {
                //
            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                BtnOTEliminar.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {
                //
            }
            if (ClsP.GetCE2() == 0)
            {
                // 
            }
            if (ClsP.GetCE3() == 0)
            {
                //
            }
            if (ClsP.GetCE4() == 0)
            {
                // 
            }
            if (ClsP.GetCE5() == 0)
            {

            }
            if (ClsP.GetCE6() == 0)
            {
                // 
            }

            IdiomaControles();
        }
        protected void IdiomaControles()
        {
            /*DataRow[] Result = Idioma.Select("Objeto= 'Titulo'");
            foreach (DataRow row in Result)
            {Page.Title = row["Texto"].ToString();}
            Result = Idioma.Select("Objeto= 'Titulo'");
            foreach (DataRow row in Result)
            { TitForm.Text = row["Texto"].ToString(); }*/

            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
            {
                string LtxtSql = "EXEC Idioma @I,@F";
                SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                SC.Parameters.AddWithValue("@I", Session["77IDM"].ToString().Trim());
                SC.Parameters.AddWithValue("@F", ViewState["PFileName"].ToString().Trim());
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())
                {
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                    Page.Title = tbl["Objeto"].ToString().Trim() == "Titulo" ? tbl["Texto"].ToString().Trim() : Page.Title;
                    TitForm.Text = tbl["Objeto"].ToString().Trim().Equals("Caption") ? tbl["Texto"].ToString().Trim() : TitForm.Text;
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
                    BtnOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTDetTec") ? tbl["Texto"].ToString().Trim() : BtnOTDetTec.Text;
                    BtnOTEliminar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTEliminar") ? tbl["Texto"].ToString().Trim() : BtnOTEliminar.Text;
                    BtNOTExportar.Text = tbl["Objeto"].ToString().Trim().Equals("BtNOTExportar") ? tbl["Texto"].ToString().Trim() : BtNOTExportar.Text;
                    BtnOTImprimir.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTImprimir") ? tbl["Texto"].ToString().Trim() : BtnOTImprimir.Text;
                    BtnOtModificar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOtModificar") ? tbl["Texto"].ToString().Trim() : BtnOtModificar.Text;
                    BtnOTReserva.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTReserva") ? tbl["Texto"].ToString().Trim() : BtnOTReserva.Text;
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
                    BtnOTConsultar.Text = tbl["Objeto"].ToString().Trim().Equals("BtnOTConsultar") ? tbl["Texto"].ToString().Trim() : BtnOTConsultar.Text;
                    LblTitOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitOTDetTec") ? tbl["Texto"].ToString().Trim() : LblTitOTDetTec.Text;
                    IbtCerrarOTDetTec.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarOTDetTec.ToolTip;
                    LblBusqOTDetTec.Text = tbl["Objeto"].ToString().Trim().Equals("TxtBusq") ? tbl["Texto"].ToString().Trim() : LblBusqOTDetTec.Text;
                    if (tbl["Objeto"].ToString().Trim().Equals("placeholder"))
                    { TxtConsulOTDetTec.Attributes.Add("placeholder", tbl["Texto"].ToString().Trim()); }
                    IbtConsOTDetTec.ToolTip = tbl["Objeto"].ToString().Trim().Equals("BtnOTConsultar") ? tbl["Texto"].ToString().Trim() : IbtConsOTDetTec.ToolTip;
                    GrdOTDetTec.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Fecha") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[0].HeaderText;
                    GrdOTDetTec.Columns[1].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Técnico") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[1].HeaderText;
                    GrdOTDetTec.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Licencia") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[2].HeaderText;
                    GrdOTDetTec.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("TotalHora") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[3].HeaderText;
                    GrdOTDetTec.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("DatoPasos") ? tbl["Texto"].ToString().Trim() : GrdOTDetTec.Columns[4].HeaderText;
                }

                // ViewState.Add("TablaIdioma", Idioma);
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
        }
        //******************************************  MRO *********************************************************
        protected void BtnMroInsPre_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroPrDes_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroRteDes_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroDanOc_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroAccCorr_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroPrueF_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroCumpl_Click(object sender, EventArgs e)
        {

        }

        protected void BtnMroTrabEje_Click(object sender, EventArgs e)
        {

        }


        //******************************************  O.T. *********************************************************
        protected void ProcesoOTInicial()
        {
            //UPDATE TblORdenTrabajo SET CentroCosto=''  WHERE CentroCosto='0'
            /*UPDATE TblContaSrvManto SET codelemento=NULL WHERE CodElemento =0
             UPDATE TblContaSrvManto SET CodAeronave=NULL WHERE CodAeronave =0*/
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
        protected void TraerDatosBusqOT(int NumOT)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 4,'','','','','',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", NumOT);
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader SDR = SqlC.ExecuteReader();
                    if (SDR.Read())
                    {
                        TxtOt.Text = HttpUtility.HtmlDecode(SDR["CodNumOrdenTrab"].ToString().Trim());
                        TxtOtPpal.Text = HttpUtility.HtmlDecode(SDR["OTMaster"].ToString().Trim());
                        TxtOtReporte.Text = HttpUtility.HtmlDecode(SDR["CodIdLvDetManto"].ToString().Trim());
                        if (Convert.ToInt32(TxtOtReporte.Text) > 0)
                        { BtnOTReserva.Enabled = false; BtnOTReserva.ToolTip = "La reserva se debe realizar desde la pantalla reporte"; }
                        else
                        { BtnOTReserva.Enabled = true; BtnOTReserva.ToolTip = "Recurso físico"; }
                        if (Convert.ToInt32(TxtOtReporte.Text) > 0 || !TxtOtRepacion.Text.Equals(""))
                        { BtnOTReporte.Enabled = false; ; BtnOTReporte.ToolTip = "El reporte solo es posible para las Ordenes de trabajo master"; }
                        else { BtnOTReporte.Enabled = true; ; BtnOTReporte.ToolTip = ""; }
                        TxtOtRepacion.Text = HttpUtility.HtmlDecode(SDR["CodReparacion"].ToString().Trim());
                        TxtlOtPrioridad.Text = HttpUtility.HtmlDecode(SDR["CodPrioridad"].ToString().Trim());
                        TxtOtWS.Text = HttpUtility.HtmlDecode(SDR["WS"].ToString().Trim());
                        UplDatosPpal.Update();
                        TxtMroPpt.Text = HttpUtility.HtmlDecode(SDR["PPT"].ToString().Trim());
                        TxtMroCliente.Text = HttpUtility.HtmlDecode(SDR["ClientePPT"].ToString().Trim());
                        CkbEjePasos.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["EjecPasos"].ToString().Trim()));
                        if (TxtOtPpal.Text.Equals("0") && TxtOtReporte.Text.Equals("0") && TxtOtRepacion.Text.Equals(""))
                        { VisibleBotMRO(true); }
                        else
                        { VisibleBotMRO(false); }
                        string VbCodTall = SDR["CodTaller"].ToString().Trim();
                        string VbCodBase = SDR["CodBase"].ToString().Trim();
                        string VbInsp = SDR["CodInspectorCierre"].ToString().Trim();
                        string VbLInsp = SDR["LicenciaInspCierre"].ToString().Trim();
                        DdlLicInsp(VbInsp, VbLInsp);
                        string VbResp = SDR["CodResponsable"].ToString().Trim();
                        BindDdlOTCondicional(VbCodTall, VbCodBase, VbInsp, VbResp);
                        DdlMroTaller.Text = VbCodTall;
                        DdlOTBase.Text = VbCodBase;
                        DdlOtInsp.Text = VbInsp;
                        DdlOtLicInsp.Text = VbLInsp;
                        DdlOtRespons.Text = VbResp;
                        TxtAplicab.Text = HttpUtility.HtmlDecode(SDR["Aplicabilidad"].ToString().Trim());
                        TxtOtPN.Text = HttpUtility.HtmlDecode(SDR["PNOT"].ToString().Trim());
                        DdlOTAero.Text = HttpUtility.HtmlDecode(SDR["CodAeronave"].ToString().Trim());
                        DdlOtEstado.Text = HttpUtility.HtmlDecode(SDR["CodEstOrdTrab1"].ToString().Trim());
                        ViewState["EstadoOT"] = DdlOtEstado.Text.Trim();
                        DdlOtEstaSec.Text = HttpUtility.HtmlDecode(SDR["CodEstOrdTrab2"].ToString().Trim());
                        if (ViewState["EstadoOT"].Equals("0001"))
                        { BtnOtModificar.Enabled = true; BtnOTEliminar.Enabled = true; }
                        else
                        { BtnOtModificar.Enabled = false; BtnOTEliminar.Enabled = false; }
                        TxtOTFechReg.Text = HttpUtility.HtmlDecode(SDR["FechaReg"].ToString().Trim());
                        TxtOTFechini.Text = HttpUtility.HtmlDecode(SDR["FechaIni"].ToString().Trim());
                        TxtOTFechFin.Text = HttpUtility.HtmlDecode(SDR["FechaFin"].ToString().Trim());
                        TxtOTFechVenc.Text = HttpUtility.HtmlDecode(SDR["FechaVence"].ToString().Trim());
                        CkbCancel.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["CancelOT"].ToString().Trim()));
                        CkbOtBloqDet.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["BloquearDetalle"].ToString().Trim()));
                        ViewState["OTBloquearDetalle"] = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["BloquearDetalle"].ToString().Trim()));
                        TxtOTTrabajo.Text = HttpUtility.HtmlDecode(SDR["TrabajoReq"].ToString().Trim());
                        TxtOTAccParc.Text = HttpUtility.HtmlDecode(SDR["AccionParcial"].ToString().Trim());
                        TxtTSN.Text = HttpUtility.HtmlDecode(SDR["OTSN"].ToString().Trim());
                        TxtTSO.Text = HttpUtility.HtmlDecode(SDR["OTSO"].ToString().Trim());
                        TxtTSR.Text = HttpUtility.HtmlDecode(SDR["OTSR"].ToString().Trim());
                        TxtCSN.Text = HttpUtility.HtmlDecode(SDR["OCSN"].ToString().Trim());
                        TxtCSO.Text = HttpUtility.HtmlDecode(SDR["OCSO"].ToString().Trim());
                        TxtCSR.Text = HttpUtility.HtmlDecode(SDR["OCSR"].ToString().Trim());
                        ViewState["TtlOTRegDet"] = Convert.ToInt32(SDR["TtlRegDet"].ToString());
                        ViewState["CarpetaCargaMasiva"] = SDR["CargaMasiva"].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente con la consulta')", true);
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
        protected void BindBDdlBusqOT()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'','','','','OT',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlBusqOT.DataSource = Cnx.DSET(LtxtSql);
            DdlBusqOT.DataMember = "Datos";
            DdlBusqOT.DataTextField = "Descripcion";
            DdlBusqOT.DataValueField = "Codigo";
            DdlBusqOT.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'','','','','HK',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlOTAero.DataSource = Cnx.DSET(LtxtSql);
            DdlOTAero.DataMember = "Datos";
            DdlOTAero.DataTextField = "Matricula";
            DdlOTAero.DataValueField = "CodAeronave";
            DdlOTAero.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'','','','','ESU',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlOtEstado.DataSource = Cnx.DSET(LtxtSql);
            DdlOtEstado.DataMember = "Datos";
            DdlOtEstado.DataTextField = "Descripcion";
            DdlOtEstado.DataValueField = "Codigo";
            DdlOtEstado.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'','','','','ESD',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlOtEstaSec.DataSource = Cnx.DSET(LtxtSql);
            DdlOtEstaSec.DataMember = "Datos";
            DdlOtEstaSec.DataTextField = "Descripcion";
            DdlOtEstaSec.DataValueField = "Codigo";
            DdlOtEstaSec.DataBind();
        }
        protected void BindDdlOTCondicional(string CT, string CB, string INSP, string RSP)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','TALLE',0,0,0,0,'01-01-01','01-01-01','01-01-01'", CT);
            DdlMroTaller.DataSource = Cnx.DSET(LtxtSql);
            DdlMroTaller.DataMember = "Datos";
            DdlMroTaller.DataTextField = "NomTaller";
            DdlMroTaller.DataValueField = "CodTaller";
            DdlMroTaller.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','BASE',0,0,0,0,'01-01-01','01-01-01','01-01-01'", CB);
            DdlOTBase.DataSource = Cnx.DSET(LtxtSql);
            DdlOTBase.DataMember = "Datos";
            DdlOTBase.DataTextField = "NomBase";
            DdlOTBase.DataValueField = "CodBase";
            DdlOTBase.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','INSP',0,0,0,0,'01-01-01','01-01-01','01-01-01'", INSP);
            DdlOtInsp.DataSource = Cnx.DSET(LtxtSql);
            DdlOtInsp.DataMember = "Datos";
            DdlOtInsp.DataTextField = "Tecnico";
            DdlOtInsp.DataValueField = "CodPersona";
            DdlOtInsp.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','RESP',0,0,0,0,'01-01-01','01-01-01','01-01-01'", RSP);
            DdlOtRespons.DataSource = Cnx.DSET(LtxtSql);
            DdlOtRespons.DataMember = "Datos";
            DdlOtRespons.DataTextField = "Tecnico";
            DdlOtRespons.DataValueField = "CodPersona";
            DdlOtRespons.DataBind();
        }
        protected void VisibleBotMRO(bool Estado)
        {
            CkbEjePasos.Visible = Estado;
            BtnMroInsPre.Visible = Estado;
            BtnMroPrDes.Visible = Estado;
            BtnMroRteDes.Visible = Estado;
            BtnMroDanOc.Visible = Estado;
            BtnMroAccCorr.Visible = Estado;
            BtnMroPrueF.Visible = Estado;
            BtnMroCumpl.Visible = Estado;
            BtnMroTrabEje.Visible = Estado;
            LblMroPpt.Visible = Estado;
            TxtMroPpt.Visible = Estado;
            LblMroCliente.Visible = Estado;
            TxtMroCliente.Visible = Estado;
            LblMroTaller.Visible = Estado;
            DdlMroTaller.Visible = Estado;
        }
        protected void DdlBusqOT_TextChanged(object sender, EventArgs e)
        {
            TraerDatosBusqOT(Convert.ToInt32(DdlBusqOT.Text));
        }

        protected void DdlOtEstado_TextChanged(object sender, EventArgs e)
        {

        }

        protected void DdlOtInsp_TextChanged(object sender, EventArgs e)
        {

        }

        //******************************************  Botones edicion OT *********************************************************       
        protected void BtnOtModificar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnOTReserva_Click(object sender, EventArgs e)
        {
            if (!TxtOt.Text.Equals(""))
            {
                if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
                { BtnOTCargaMasiva.Enabled = false; BtnOTCargaMasiva.ToolTip = "La orden debe estar abierta y no deben existir registros en la reserva"; }
                else
                { BtnOTCargaMasiva.Enabled = true; BtnOTCargaMasiva.ToolTip = "Realizar carga masiva a partir del archivo " + ViewState["CarpetaCargaMasiva"].ToString() + "CargaMasiva.xlsx"; }
                if (CkbOtBloqDet.Checked == true)
                { BtnOTRecurNotif.Enabled = false; }
                else { BtnOTRecurNotif.Enabled = true; }
                BindDOTRecursoF();
                PerfilesGrid();
                MlVwOT.ActiveViewIndex = 2;
            }
        }
        protected void BtnOTConsultar_Click(object sender, EventArgs e)
        {
            LblTitOTOpcBusqueda.Text = "Opciones de búsqueda orden de trabajo";
            TblOTBusq.Visible = true;
            IbtOTExpBusqOT.Visible = true;
            GrdOTBusq.DataSource = null;
            GrdOTBusq.DataBind();
            ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
            MlVwOT.ActiveViewIndex = 4;
        }
        protected void BtnOTImprimir_Click(object sender, EventArgs e)
        {
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
                    StSql = "EXEC Impresion_OT_MRO @OT,'SERVICIO',@Us";
                    VbOpc = "OT_PREV";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('La impresión solo aplica para órdenes de trabajo Master')", true);
                    return;
                }
                ViewState["Ventana"] = MlVwOT.ActiveViewIndex;
                MlVwOT.ActiveViewIndex = 5;
                using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                {
                    SC.Parameters.AddWithValue("@OT", TxtOt.Text);
                    SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(ds);
                        RvwOTPrint.LocalReport.EnableExternalImages = true;
                        switch (VbOpc)
                        {
                            case "OT_PREV":
                                RvwOTPrint.LocalReport.ReportPath = "Forms/Ingenieria/Informe/OrdenTrabajoMRO.rdlc";
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
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC Consultas_General 7,@Us,'','',@O,0,0,'01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@O", Convert.ToInt32(TxtOt.Text));
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
                                ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "')", true);
                                return;
                            }
                            LimpiarCamposOT();
                            BindBDdlBusqOT();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UplOT, UplOT.GetType(), "IdntificadorBloqueScript", "alert('Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnOtReporte_Click(object sender, EventArgs e)
        {

        }

        protected void BtnOtAbiertas8PasCump_Click(object sender, EventArgs e)
        {

        }

        protected void BtNOTExportar_Click(object sender, EventArgs e)
        {

        }

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
                Cnx.SelecBD();
                using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 6,@TC,'','','','',@OT,0,0,0,'01-01-01','01-01-01','01-01-01'");
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                    {
                        SC.Parameters.AddWithValue("@OT", TxtOt.Text.Trim());
                        SC.Parameters.AddWithValue("@TC", TxtConsulOTDetTec.Text.Trim());

                        SCX2.Open();
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
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
                                GrdOTDetTec.Rows[0].Cells[0].Text = "Sin técnicos asignados!";
                                GrdOTDetTec.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDOTDetTec", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarOTDetTec_Click(object sender, ImageClickEventArgs e)
        {
            MlVwOT.ActiveViewIndex = 0;
        }
        protected void IbtConsOTDetTec_Click(object sender, ImageClickEventArgs e)
        {
            BindDOTDetTec();
        }
        protected void DdlOTTecPP_TextChanged(object sender, EventArgs e)
        {
            DropDownList DdlOTTecPP = (GrdOTDetTec.FooterRow.FindControl("DdlOTTecPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'{0}','','','','LINSP',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlOTTecPP.Text.Trim());
            DropDownList DdlOTLicPP = (GrdOTDetTec.FooterRow.FindControl("DdlOTLicPP") as DropDownList);
            DdlOTLicPP.DataSource = Cnx.DSET(LtxtSql);
            DdlOTLicPP.DataTextField = "Licencia";
            DdlOTLicPP.DataValueField = "Codigo";
            DdlOTLicPP.DataBind();
        }
        protected void GrdOTDetTec_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (TxtOt.Text.Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Orden de trabajo')", true);
                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("TxtOTFecTrabPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha')", true);
                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("DdlOTTecPP") as DropDownList).Text.Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un técnico')", true);
                        return;
                    }
                    if ((GrdOTDetTec.FooterRow.FindControl("DdlOTLicPP") as DropDownList).Text.Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una licencia')", true);
                        return;
                    }
                    CultureInfo Culture = new CultureInfo("en-US");
                    string VblTxtCant = (GrdOTDetTec.FooterRow.FindControl("TxtNumHorasPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdOTDetTec.FooterRow.FindControl("TxtNumHorasPP") as TextBox).Text.Trim();
                    double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    if (VblCant <= 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('El tiempo debe ser mayor a cero')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,@TEC,@Lic,@Usu,'','','','','','INSERT',@OT,@T,0,0,0,0,@F,'02-01-1','03-01-1'");

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
                                    BindDOTDetTec();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdOTDetTec.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex; // Guarda El indice para luego buscar en otro evento com en un TextChanged
            BindDOTDetTec();
        }
        protected void GrdOTDetTec_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
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
                    ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha')", true);
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
                        string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,'','',@Usu,'','','','','','UPDATE',@OT,@T,0,0,0,@I,@F,'02-01-1','03-01-1'");

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@I", VblId);
                                SC.Parameters.AddWithValue("@OT", TxtOt.Text);
                                SC.Parameters.AddWithValue("@F", (GrdOTDetTec.Rows[Idx].FindControl("TxtOTFecTrab") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@T", VblCant);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());

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
                                BindDOTDetTec();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en la edición')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, " Validación Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdOTDetTec.EditIndex = -1;
            BindDOTDetTec();
        }
        protected void GrdOTDetTec_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                PerfilesGrid();
                int Idx = e.RowIndex;
                int VblId = Convert.ToInt32(GrdOTDetTec.DataKeys[Idx].Value.ToString());
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format(" EXEC SP_TablasIngenieria 10,'','',@Usu,'','','','','','DELETE',0,0,0,0,0,@I,0,'02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@I", VblId);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());

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
                                BindDOTDetTec();
                            }
                            catch (Exception Ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR Técnicos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTDetTec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 5,'','','','','TEC',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                    DropDownList DdlOTTecPP = (e.Row.FindControl("DdlOTTecPP") as DropDownList);
                    DdlOTTecPP.DataSource = Cnx.DSET(VbTxtSql);
                    DdlOTTecPP.DataTextField = "Tecnico";
                    DdlOTTecPP.DataValueField = "CodPersona";
                    DdlOTTecPP.DataBind();
                    CalendarExtender CalOTFecTrabPP = (e.Row.FindControl("CalOTFecTrabPP") as CalendarExtender);
                    CalOTFecTrabPP.EndDate = DateTime.Now;

                    ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    CalendarExtender CalOTFecTrab = (e.Row.FindControl("CalOTFecTrab") as CalendarExtender);
                    CalOTFecTrab.EndDate = DateTime.Now;
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
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTDetTec, UplOTDetTec.GetType(), "IdntificadorBloqueScript", "alert('Error')", true);
            }
        }
        protected void GrdOTDetTec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdOTDetTec.PageIndex = e.NewPageIndex;
            BindDOTDetTec();
            PerfilesGrid();
        }

        //******************************************  RECURSO FISICO OT*********************************************************
        protected void BindDOTRecursoF()
        {
            try
            {
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection SCX2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 4,@PN,'','','','',@O,0,0,0,'01-01-1','02-01-1','03-01-1'");
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCX2))
                    {
                        SC.Parameters.AddWithValue("@PN", TxtOTRecurConsulPn.Text.Trim());
                        SC.Parameters.AddWithValue("@O", TxtOt.Text.Trim());
                        SCX2.Open();
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DT);
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
                                GrdOTRecursoF.Rows[0].Cells[0].Text = "Sin reserva!";
                                GrdOTRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
        protected void IbtOTCerrarRecur_Click(object sender, ImageClickEventArgs e)
        {
            MlVwOT.ActiveViewIndex = 0;
        }
        protected void IbtOTRecurConsulPn_Click(object sender, ImageClickEventArgs e)
        {
            BindDOTRecursoF();
        }
        protected void IbtOTRecurExpExcelPn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string StSql = "EXEC SP_PANTALLA_Reporte_Manto2 6,'','','','','',@SubOT,0,0,0,'01-01-1','02-01-1','03-01-1'";
                string VbNomRpt = "Recurso";
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@SubOT", TxtOt.Text.Trim());
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
        protected void BtnOTCargaMasiva_Click(object sender, EventArgs e)
        {
            if ((int)ViewState["TtlOTRegDet"] > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Para realizar la carga masiva la reserva debe estar vacía')", true);
                return;
            }
            IbtOTGuardarCargaMax.Enabled = false;
            MlVwOT.ActiveViewIndex = 3;
        }
        protected void BtnOTRecurNotif_Click(object sender, EventArgs e)
        {
            if (CkbOtBloqDet.Checked == true)
            { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 7,@Usu,'','','','',@OT,0,0,0,'01-01-01','01-01-01','01-01-01'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@OT", Convert.ToInt32(TxtOt.Text));
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
                                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                return;
                            }/**/
                            CkbOtBloqDet.Checked = true;
                            BtnOTRecurNotif.Enabled = false;
                            ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Orden de trabajo notificada')", true);
                        }
                        catch (Exception Ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }

                    }
                }
            }
        }
        protected void DdlOTPNRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
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
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string VblString = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'{0}','','','','DescRef',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlOTPNRFPP.Text);
                SqlCommand SC = new SqlCommand(VblString, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtDesRFPP.Text = SDR["Descripcion"].ToString();
                }
            }
        }
        protected void GrdOTRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();

                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.CommandName.Equals("AddNew"))
                {
                    if (TxtlOtPrioridad.Text.Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una prioridad')", true);
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'','','INSERT',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@IdDetRsva", 0);
                                    SC.Parameters.AddWithValue("@PN", VblPN);
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@CodPri", TxtlOtPrioridad.Text.Trim());
                                    SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                    SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                    SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                    SC.Parameters.AddWithValue("@OT", Convert.ToInt32(TxtOt.Text));
                                    SC.Parameters.AddWithValue("@Cant", VblCant);
                                    SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlOTAero.Text));
                                    SC.Parameters.AddWithValue("@IdRte", 0);

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
                                    BindDOTRecursoF();
                                    PerfilesGrid();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdOTRecursoF.EditIndex = e.NewEditIndex;
            ViewState["Index"] = e.NewEditIndex; // Guarda El indice para luego buscar en otro evento com en un TextChanged
            BindDOTRecursoF();
        }
        protected void GrdOTRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                PerfilesGrid();
                if (TxtlOtPrioridad.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una prioridad')", true);
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,@CodPri,@CodTipCod,@IPC,@DescPN,'','','UPDATE',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,0,'01-01-1','02-01-1','03-01-1'");

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CodPri", TxtlOtPrioridad.Text.Trim());
                                SC.Parameters.AddWithValue("@CodTipCod", Session["CodTipoCodigoInicial"].ToString());
                                SC.Parameters.AddWithValue("@IPC", VbIPC.Trim());
                                SC.Parameters.AddWithValue("@DescPN", VbDesc.Trim());
                                SC.Parameters.AddWithValue("@OT", Convert.ToInt32(TxtOt.Text));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlOTAero.Text));
                                SC.Parameters.AddWithValue("@IdRte", 0);
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
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                GrdOTRecursoF.EditIndex = -1;
                                BindDOTRecursoF();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Reserva OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Reserva OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdOTRecursoF.EditIndex = -1;
            BindDOTRecursoF();
        }
        protected void GrdOTRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                PerfilesGrid();
                string VBQuery;
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 9,@PN,@Usu,'','','','','','','DELETE',@IdDetRsva,@OT,@Cant,@CodHK,@IdRte,@Posc,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@IdDetRsva", VblId);
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@OT", Convert.ToInt32(TxtOt.Text));
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@CodHK", Convert.ToInt32(DdlOTAero.Text));
                                SC.Parameters.AddWithValue("@IdRte", 0);
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
                                    ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    return;
                                }
                                TxtOTRecurConsulPn.Text = "";
                                BindDOTRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTRecurso, UplOTRecurso.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR RECURSO OT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdOTRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlOTPNRFPP = (e.Row.FindControl("DdlOTPNRFPP") as DropDownList);
                DdlOTPNRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlOTPNRFPP.DataTextField = "PN";
                DdlOTPNRFPP.DataValueField = "CodPN";
                DdlOTPNRFPP.DataBind();
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = false;
                        IbtAddNew.ToolTip = "Cerrado / Bloqueado";
                    }
                }
                else
                {
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        IbtAddNew.ToolTip = "Nuevo";
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (DdlOtEstado.Text.Equals("0002") || (Boolean)ViewState["OTBloquearDetalle"] == true)
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        imgE.ToolTip = "Cerrado / Bloqueado";
                    }
                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        imgD.ToolTip = "Cerrado / Bloqueado";
                    }
                }
                else
                {
                    if (imgE != null)
                    {
                        imgE.Enabled = true;
                        imgE.ToolTip = "Editar";
                    }
                    if (imgD != null)
                    {
                        imgD.Enabled = true;
                        imgD.ToolTip = "Eliminar";
                    }
                }
            }
        }
        protected void GrdOTRecursoF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdOTRecursoF.PageIndex = e.NewPageIndex;
            BindDOTRecursoF();
            PerfilesGrid();
        }

        //******************************************  Subir Recurso Carga Maxivamente *********************************************************
        protected void IbtOTCerrarCargMaxivo_Click(object sender, ImageClickEventArgs e)
        {
            MlVwOT.ActiveViewIndex = 2;
        }
        protected void IbtOTSubirCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                DataTable DT = new DataTable();
                string FileName = "";
                string conexion = "";
                //string conexion1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Asus Pro\Downloads\Reportes.xlsx;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                FileName = "CargaMasiva.xlsx";
                conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus Pro\Downloads\" + FileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";

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
                ScriptManager.RegisterClientScriptBlock(this.UplOTCargMasiv, UplOTCargMasiv.GetType(), "IdntificadorBloqueScript", "alert('No se realizó la acción, verifica la plantilla')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtOTGuardarCargaMax_Click(object sender, ImageClickEventArgs e)
        {
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
                    Accion = "INSERT",
                };
                ObjSubirRsva.Add(TypSubirRsva);
            }
            CsTypSubirReserva SubirRsva = new CsTypSubirReserva();
            SubirRsva.Alimentar(ObjSubirRsva);// 
            string Mensj = SubirRsva.GetMensj();
            if (!Mensj.Trim().Equals("OK"))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplOTCargMasiv, UplOTCargMasiv.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                IbtOTGuardarCargaMax.Enabled = false;
                return;
            }
            IbtOTGuardarCargaMax.Enabled = false;
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
                string VbTxtSql = "", VbOpcion = "";
                if ((int)ViewState["Ventana"] == 0) // OT
                {
                    if (RdbOTBusqNumOT.Checked == true)
                    { VbOpcion = "OT"; }
                    if (RdbOTBusqSN.Checked == true)
                    { VbOpcion = "SN"; }
                    if (RdbOTBusqPN.Checked == true)
                    { VbOpcion = "PN"; }
                    if (RdbOTBusqHK.Checked == true)
                    { VbOpcion = "HK"; }
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 8,@Prmtr,'','','',@Opc,0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                }
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtOTBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
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
        {
            BIndDBusqOT();
        }
        protected void IbtOTCerrarBusq_Click(object sender, ImageClickEventArgs e)
        {
            TblOTBusq.Visible = false;
            MlVwOT.ActiveViewIndex = (int)ViewState["Ventana"];
        }
        protected void IbtOTExpBusqOT_Click(object sender, ImageClickEventArgs e)
        {
            Exportar("");
        }
        protected void GrdOTBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdOTBusq.SelectedRow.Cells[1].Text);
            if ((int)ViewState["Ventana"] == 0) // OT
            {
                TraerDatosBusqOT(Convert.ToInt32(vbcod));
                MlVwOT.ActiveViewIndex = (int)ViewState["Ventana"];
            }
            PerfilesGrid();
        }
        protected void GrdOTBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdOTBusq.PageIndex = e.NewPageIndex;
            BIndDBusqOT();
        }
        //******************************************  Procedimientos *********************************************************
        protected void Exportar(string Condcion)
        {
            try
            {
                string StSql, VbNomRpt, VbOpcion = "";

                switch (Condcion)
                {
                    case "Reserva":
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto2 6,'','','','','',@SubOT,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        VbNomRpt = "Recurso";
                        break;
                    case "ReporteGeneral":
                        StSql = "EXEC SP_PANTALLA_Reporte_Manto 4,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        VbNomRpt = "Reportes_Mantenimiento";
                        break;
                    default:
                        if ((int)ViewState["Ventana"] == 0) // OT
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
                        }
                        StSql = string.Format("EXEC SP_PANTALLA_OrdenTrabajo2 8, @Prmtr, '', '', '', @Opc, 0, 0, 0, 0, '01-01-01', '01-01-01', '01-01-01'");
                        VbNomRpt = "OT";
                        break;
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@Prmtr", TxtOTBusq.Text.Trim()); // solo cuando es para el reporte
                        SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());// solo cuando es para  la OT o el reporte
                                                                            //SC.Parameters.AddWithValue("@CodlV", TxtNumLv.Text.Trim());// solo cuando es para el reporte
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

        //******************************************  IMPRESION OT *********************************************************
        protected void IbtOTCerrarPrint_Click(object sender, ImageClickEventArgs e)
        {
            MlVwOT.ActiveViewIndex = (int)ViewState["Ventana"];
        }
    }
}