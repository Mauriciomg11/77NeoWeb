using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.prgMro;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmPropuesta : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DtDet1All = new DataTable();
        DataTable DtDdlPptPpal = new DataTable();
        DataTable DtPnNoValoriz = new DataTable();
        DataTable DtElemRepa = new DataTable();
        DataTable DtHKRepa = new DataTable();
        DataTable DtSrvcs = new DataTable();
        DataTable DTPNMat = new DataTable();
        DataSet DS = new DataSet(); DataSet DSDdl = new DataSet(); DataSet DSAlerta = new DataSet(); DataTable DTEncPPT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            } /* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "";
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
                TitForm.Text = "";
                ModSeguridad();
                BindDataDdlPpal("UPDATE", "0");
                BindDataDdlPptPpal("", "UPDATE");
                BindDCondiciones();
                BindDdlPnMat();
                ViewState["Accion"] = "";
                ViewState["CamposNuevos"] = "N";
                ViewState["Notificacion"] = "";
                ViewState["FilterElem"] = "N";
                ViewState["FilterPnSugerido"] = "N";
                ViewState["IdDetPropHk"] = "0";// Importante guarda el id al filtrar un PN o aeroanve para el control al momento de filtrar por PN o por aeroanve GrdElementos
                ViewState["AeroVirtual"] = "";
                ViewState["IdDetPropSrv"] = "0";// Para el control del filtro del trabajo [GrdServicios] para visualizar los Pn sugeridso
                ViewState["VlrAproAllSvc"] = "0";
                ViewState["RegistroElemHK"] = "";// valor del PN por el cual se filtra en en el Det2 GrdElementos
                Alertas();
                MultVw.ActiveViewIndex = 0;
                RdbBusqGnrlPpt.Checked = true;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; BtnImprimir.Visible = false; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//APROB ITEM venta de partes y alerta de ppt aprobadas sin item del det marados
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//GENER OT 
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; BtnNotfLog.Visible = false; }//NOTIF. LOGIST
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; IbtDesaprobar.Visible = false; IbtReturnEstado.Visible = false; }//DESAPROBAR                                        

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
                    if (bO.Equals("Caption"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("Titulo") ? bT : TitForm.Text;
                    BtnNotfPCP.ToolTip = bO.Equals("BtnNotfPCPTT") ? bT : BtnNotfPCP.ToolTip;
                    BtnNotfPCP.Text = bO.Equals("BtnNotfPCP") ? bT : BtnNotfPCP.Text;
                    BtnNotfLog.Text = bO.Equals("BtnNotfLog") ? bT : BtnNotfLog.Text;
                    BtnNotfLog.ToolTip = bO.Equals("BtnNotfLogTT") ? bT : BtnNotfLog.ToolTip;
                    BtnNotfAprob.Text = bO.Equals("BtnNotfAprob") ? bT : BtnNotfAprob.Text;
                    BtnNotfCumpld.Text = bO.Equals("BtnNotfCumpld") ? bT : BtnNotfCumpld.Text;
                    BtnNotfCancel.Text = bO.Equals("BtnNotfCancel") ? bT : BtnNotfCancel.Text;
                    BtnNotfDevolc.Text = bO.Equals("BtnNotfDevolc") ? bT : BtnNotfDevolc.Text;
                    BtnNotfNoAprob.Text = bO.Equals("BtnNotfNoAprob") ? bT : BtnNotfNoAprob.Text;
                    LblNumPpt.Text = bO.Equals("LblNumPpt") ? bT + ":" : LblNumPpt.Text;
                    BtnIngresar.Text = bO.Equals("IbtAddNew") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    BtnEditCondic.Text = bO.Equals("BtnEditCondic") ? bT : BtnEditCondic.Text;
                    BtnEditCondic.ToolTip = bO.Equals("BtnEditCondicTT") ? bT : BtnEditCondic.ToolTip;
                    BtnDetalle.Text = bO.Equals("BtnDetalle") ? bT : BtnDetalle.Text;
                    BtnDetalle.ToolTip = bO.Equals("BtnDetalleTT") ? bT : BtnDetalle.ToolTip;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnImprimir.Text = bO.Equals("BtnImprimirGrl") ? bT : BtnImprimir.Text;
                    BtnImprimir.ToolTip = bO.Equals("BtnImprimirTT") ? bT : BtnImprimir.ToolTip;
                    BtnExportPPT.Text = bO.Equals("BtnExportPPT") ? bT : BtnExportPPT.Text;
                    BtnExportDet.Text = bO.Equals("BtnExportDet") ? bT : BtnExportDet.Text;
                    BtnAux.Text = bO.Equals("BtnAux") ? bT : BtnAux.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT : LblTipo.Text;
                    LblCliente.Text = bO.Equals("LblCliente") ? bT : LblCliente.Text;
                    LblFormPag.Text = bO.Equals("LblFormPag") ? bT : LblFormPag.Text;
                    LblPptSuper.Text = bO.Equals("LblPptSuper") ? bT : LblPptSuper.Text;
                    LbPptComerc.Text = bO.Equals("LbPptComerc") ? bT : LbPptComerc.Text;
                    LblNumContrat.Text = bO.Equals("LblNumContrat") ? bT : LblNumContrat.Text;
                    LblMoned.Text = bO.Equals("LblMoned") ? bT : LblMoned.Text;
                    LblFechTRM.Text = bO.Equals("LblFechTRM") ? bT : LblFechTRM.Text;
                    LblValorTrm.Text = bO.Equals("LblValorTrm") ? bT : LblValorTrm.Text;
                    LblEstado.Text = bO.Equals("LblEstado") ? bT : LblEstado.Text;
                    IbtReturnEstado.ToolTip = bO.Equals("IbtReturnEstado") ? bT : IbtReturnEstado.ToolTip;
                    LblFechAprob.Text = bO.Equals("LblFechAprob") ? bT : LblFechAprob.Text;
                    IbtDesaprobar.ToolTip = bO.Equals("IbtDesaprobar") ? bT : IbtDesaprobar.ToolTip;
                    LblFechEntreg.Text = bO.Equals("LblFechEntreg") ? bT : LblFechEntreg.Text;
                    LblFechValidez.Text = bO.Equals("LblFechValidez") ? bT : LblFechValidez.Text;
                    LblFechEntregTrab.Text = bO.Equals("LblFechEntregTrab") ? bT : LblFechEntregTrab.Text;
                    LblTipoSol.Text = bO.Equals("LblTipoSol") ? bT : LblTipoSol.Text;
                    RdbSinDanOcul.Text = bO.Equals("RdbSinDanOcul") ? bT : RdbSinDanOcul.Text;
                    RdbDanOcul.Text = bO.Equals("RdbDanOcul") ? bT : RdbDanOcul.Text;
                    LblSubTtl.Text = bO.Equals("LblSubTtl") ? bT : LblSubTtl.Text;
                    LblImpuest.Text = bO.Equals("LblImpuest") ? bT : LblImpuest.Text;
                    LblTotal.Text = bO.Equals("LblTotal") ? bT : LblTotal.Text;
                    LblAjusVent.Text = bO.Equals("LblAjusVent") ? bT : LblAjusVent.Text;
                    LblMotvAjust.Text = bO.Equals("LblMotvAjust") ? bT : LblMotvAjust.Text;
                    LblVlrRecurso.Text = bO.Equals("LblVlrRecurso") ? bT : LblVlrRecurso.Text;
                    lblVlrMnObr.Text = bO.Equals("lblVlrMnObr") ? bT : lblVlrMnObr.Text;
                    CkbAplicImpuesto.Text = bO.Equals("CkbAplicImpuesto") ? bT : CkbAplicImpuesto.Text;
                    LblObserv.Text = bO.Equals("LblObserv") ? bT : LblObserv.Text;
                    LblGarant.Text = bO.Equals("LblGarant") ? bT : LblGarant.Text;
                    LblGanacInter.Text = bO.Equals("LblGanacInter") ? bT : LblGanacInter.Text;
                    LblGanacNacional.Text = bO.Equals("LblGanacNacional") ? bT : LblGanacNacional.Text;
                    //******************************* DETALLE DET1*****************************
                    LblTitDetalleGrl.Text = bO.Equals("LblTitDetalleGrl") ? bT : LblTitDetalleGrl.Text;
                    RdbDet1BuqAll.Text = bO.Equals("RdbDet1BuqAll") ? "&nbsp" + bT : RdbDet1BuqAll.Text;
                    IbtAprDet1All.ToolTip = bO.Equals("IbtAprDet1All") ? bT : IbtAprDet1All.ToolTip;
                    IbtDesAprDet1All.ToolTip = bO.Equals("IbtDesAprDet1All") ? bT : IbtDesAprDet1All.ToolTip;
                    RdbDet1BuqOT.Text = bO.Equals("RdbDet1BuqOT") ? "&nbsp" + bT : RdbDet1BuqOT.Text;
                    RdbDet1BuqRte.Text = bO.Equals("RdbDet1BuqRte") ? "&nbsp" + bT : RdbDet1BuqRte.Text;
                    RdbDet1BuqSvc.Text = bO.Equals("RdbDet1BuqSvc") ? "&nbsp" + bT : RdbDet1BuqSvc.Text;
                    CkbAplicOT.Text = bO.Equals("CkbAplicOT") ? "&nbsp" + bT : CkbAplicOT.Text;
                    IbtConsultarDet1.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtConsultarDet1.ToolTip;
                    IbtBusqueda.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtBusqueda.ToolTip;
                    GrdDet1.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDet1.EmptyDataText;
                    GrdDet1.Columns[2].HeaderText = bO.Equals("GrdAprob") ? bT : GrdDet1.Columns[2].HeaderText;
                    GrdDet1.Columns[4].HeaderText = bO.Equals("GrdRef") ? bT : GrdDet1.Columns[4].HeaderText;
                    GrdDet1.Columns[5].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdDet1.Columns[5].HeaderText;
                    GrdDet1.Columns[6].HeaderText = bO.Equals("GrdCantSol") ? bT : GrdDet1.Columns[6].HeaderText;
                    GrdDet1.Columns[7].HeaderText = bO.Equals("GrdCantReal") ? bT : GrdDet1.Columns[7].HeaderText;
                    GrdDet1.Columns[8].HeaderText = bO.Equals("GrdVlrUnd") ? bT : GrdDet1.Columns[8].HeaderText;
                    GrdDet1.Columns[9].HeaderText = bO.Equals("GrdUtili") ? bT : GrdDet1.Columns[9].HeaderText;
                    GrdDet1.Columns[10].HeaderText = bO.Equals("GrdCostV") ? bT : GrdDet1.Columns[10].HeaderText;
                    GrdDet1.Columns[11].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdDet1.Columns[11].HeaderText;
                    GrdDet1.Columns[12].HeaderText = bO.Equals("GrdUndComp") ? bT : GrdDet1.Columns[12].HeaderText;
                    GrdDet1.Columns[13].HeaderText = bO.Equals("GrdMon") ? bT : GrdDet1.Columns[13].HeaderText;
                    GrdDet1.Columns[14].HeaderText = bO.Equals("GrdVlrMon") ? bT : GrdDet1.Columns[14].HeaderText;
                    GrdDet1.Columns[15].HeaderText = bO.Equals("GrdPorcImp") ? bT : GrdDet1.Columns[15].HeaderText;
                    GrdDet1.Columns[16].HeaderText = bO.Equals("GrdVlrConImp") ? bT : GrdDet1.Columns[16].HeaderText;
                    GrdDet1.Columns[18].HeaderText = bO.Equals("GrdTimEntrD") ? bT : GrdDet1.Columns[18].HeaderText;
                    GrdDet1.Columns[19].HeaderText = bO.Equals("RdbDet1BuqRte") ? bT : GrdDet1.Columns[19].HeaderText;
                    GrdDet1.Columns[20].HeaderText = bO.Equals("RdbDet1BuqOT") ? bT : GrdDet1.Columns[20].HeaderText;
                    GrdDet1.Columns[21].HeaderText = bO.Equals("RdbDet1BuqSvc") ? bT : GrdDet1.Columns[21].HeaderText;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtBusqDet1.Attributes.Add("placeholder", bT); TxtModalBusq.Attributes.Add("placeholder", bT); }
                    //**************************************** Busqueda //****************************************
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusqueda.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusqueda.Text;
                    RdbBusqGnrlPpt.Text = bO.Equals("Caption") ? bT + ":" : RdbBusqGnrlPpt.Text;
                    RdbBusqGnrlHk.Text = bO.Equals("RdbBusqGnrlHk") ? bT + ":" : RdbBusqGnrlHk.Text;
                    RdbBusqGnrlOT.Text = bO.Equals("RdbDet1BuqOT") ? bT + ":" : RdbBusqGnrlOT.Text;
                    RdbBusqGnrlRte.Text = bO.Equals("RdbDet1BuqRte") ? bT + ":" : RdbBusqGnrlRte.Text;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("LblNumPpt") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("RdbBusqGnrlHk") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("GrdSnElem") ? bT : GrdBusq.Columns[3].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("LblCliente") ? bT : GrdBusq.Columns[5].HeaderText;
                    GrdBusq.Columns[6].HeaderText = bO.Equals("LblEstado") ? bT : GrdBusq.Columns[6].HeaderText;
                    //**************************************** Parametrizacion Condicion****************************************
                    IbtClseCondic.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClseCondic.ToolTip;
                    LblTitCondiciones.Text = bO.Equals("BtnEditCondicTT") ? bT : LblTitCondiciones.Text;
                    LblCondTiempEntreg.Text = bO.Equals("LblCondTiempEntreg") ? bT : LblCondTiempEntreg.Text;
                    LblCondTiempEntregPpt.Text = bO.Equals("LblCondTiempEntreg") ? bT : LblCondTiempEntregPpt.Text;
                    LblCondFormPago.Text = bO.Equals("LblCondFormPago") ? bT : LblCondFormPago.Text;
                    LblCondFormPagoPpt.Text = bO.Equals("LblCondFormPago") ? bT : LblCondFormPagoPpt.Text;
                    LblCondDanoOcult.Text = bO.Equals("LblCondDanoOcult") ? bT : LblCondDanoOcult.Text;
                    LblCondDanoOcultPpt.Text = bO.Equals("LblCondDanoOcult") ? bT : LblCondDanoOcultPpt.Text;
                    LblCondGarant.Text = bO.Equals("LblCondGarant") ? bT : LblCondGarant.Text;
                    LblCondGarantPpt.Text = bO.Equals("LblCondGarant") ? bT : LblCondGarantPpt.Text;
                    BtnUpdateCond.Text = bO.Equals("BtnUpdateCond") ? bT : BtnUpdateCond.Text;
                    BtnUpdateCondPpt.Text = bO.Equals("BtnUpdateCondPpt") ? bT : BtnUpdateCondPpt.Text;
                    //**************************************** Alerta Modal ****************************************
                    LblTituloModal.Text = bO.Equals("TitMensjMdlMstr") ? bT : LblTituloModal.Text;
                    BtnSiModl.Text = bO.Equals("BtnSiMdlMstr") ? bT : BtnSiModl.Text;
                    //**************************************** Panel PN no encontrados en la valorización ****************************************
                    IbtClosePNoValorizado.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClosePNoValorizado.ToolTip;
                    LblTitEleNoValorizado.Text = bO.Equals("LblTitEleNoValorizado") ? bT : LblTitEleNoValorizado.Text;
                    GrdPnNoValorizado.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdPnNoValorizado.EmptyDataText;
                    GrdPnNoValorizado.Columns[0].HeaderText = bO.Equals("RdbDet1BuqRte") ? bT : GrdPnNoValorizado.Columns[0].HeaderText;
                    GrdPnNoValorizado.Columns[1].HeaderText = bO.Equals("RdbDet1BuqOT") ? bT : GrdPnNoValorizado.Columns[1].HeaderText;
                    GrdPnNoValorizado.Columns[2].HeaderText = bO.Equals("GrdRef") ? bT : GrdPnNoValorizado.Columns[2].HeaderText;
                    GrdPnNoValorizado.Columns[4].HeaderText = bO.Equals("GrdFecRva") ? bT : GrdPnNoValorizado.Columns[4].HeaderText;
                    GrdPnNoValorizado.Columns[5].HeaderText = bO.Equals("GrdCreaPN") ? bT : GrdPnNoValorizado.Columns[5].HeaderText;
                    GrdPnNoValorizado.Columns[6].HeaderText = bO.Equals("GrdFechNotf") ? bT : GrdPnNoValorizado.Columns[6].HeaderText;
                    GrdPnNoValorizado.Columns[7].HeaderText = bO.Equals("GrdFechValoriza") ? bT : GrdPnNoValorizado.Columns[7].HeaderText;
                    //**************************************** Panel PN y Aeronaves a Reparar ****************************************
                    IbtClosDetElemHK.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClosDetElemHK.ToolTip;
                    GrdElementos.Columns[0].HeaderText = bO.Equals("FiltroMst") ? bT : GrdElementos.Columns[0].HeaderText;
                    GrdElementos.Columns[3].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdElementos.Columns[3].HeaderText;
                    GrdAeronave.Columns[1].HeaderText = bO.Equals("GrdAerove") ? bT : GrdAeronave.Columns[1].HeaderText;
                    GrdAeronave.Columns[2].HeaderText = bO.Equals("GrdModel") ? bT : GrdAeronave.Columns[2].HeaderText;
                    GrdAeronave.Columns[3].HeaderText = bO.Equals("GrdDesMod") ? bT : GrdAeronave.Columns[3].HeaderText;
                    //**************************************** Modal Asignar PN ****************************************
                    RdbMOdalBusqDesc.Text = bO.Equals("GrdDescSn") ? bT : RdbMOdalBusqDesc.Text;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT : LblModalBusq.Text;
                    IbtModalBusq.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtModalBusq.ToolTip;
                    BtnCloseModalBusqPN.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqPN.Text;
                    GrdModalBusqPN.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqPN.EmptyDataText;
                    GrdModalBusqPN.Columns[3].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdModalBusqPN.Columns[3].HeaderText;
                    //**************************************** Panel servicios ****************************************
                    LblTitOt.Text = bO.Equals("BtnDetalle") ? bT : LblTitOt.Text;
                    GrdServicios.Columns[0].HeaderText = bO.Equals("FiltroMst") ? bT : GrdServicios.Columns[0].HeaderText;
                    GrdServicios.Columns[2].HeaderText = bO.Equals("BtnDetalle") ? bT : GrdServicios.Columns[2].HeaderText;
                    GrdServicios.Columns[4].HeaderText = bO.Equals("RdbDet1BuqOT") ? bT : GrdServicios.Columns[4].HeaderText;
                    GrdServicios.Columns[5].HeaderText = bO.Equals("RdbDet1BuqRte") ? bT : GrdServicios.Columns[5].HeaderText;
                    GrdServicios.Columns[6].HeaderText = bO.Equals("GrdExtern") ? bT : GrdServicios.Columns[6].HeaderText;
                    //****************************************  Partes sugeridos ****************************************
                    LblTitPnSugerido.Text = bO.Equals("LblTitPnSugerido") ? bT : LblTitPnSugerido.Text;
                    GrdPnSugerd.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdPnSugerd.EmptyDataText;
                    GrdPnSugerd.Columns[1].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdPnSugerd.Columns[1].HeaderText;
                    GrdPnSugerd.Columns[2].HeaderText = bO.Equals("GrdCantSN") ? bT : GrdPnSugerd.Columns[2].HeaderText;
                    LblTitDetalleMH.Text = bO.Equals("lblVlrMnObr") ? bT : LblTitDetalleMH.Text;
                    GrdMO.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdMO.EmptyDataText;
                    GrdMO.Columns[0].HeaderText = bO.Equals("GrdMOLic") ? bT : GrdMO.Columns[0].HeaderText;
                    GrdMO.Columns[1].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdMO.Columns[1].HeaderText;
                    GrdMO.Columns[2].HeaderText = bO.Equals("GrdEstmd") ? bT : GrdMO.Columns[2].HeaderText;
                    GrdMO.Columns[3].HeaderText = bO.Equals("GrdVlor") ? bT : GrdMO.Columns[3].HeaderText;
                    //**************************************** Panel Asignar trabajos masivamente  ****************************************
                    LblTitAsigSvcMasivo.Text = bO.Equals("LblTitAsigSvcMasivo") ? bT : LblTitAsigSvcMasivo.Text;
                    BtnAsigSvcMasivo.Text = bO.Equals("BtnAsigSvcMasivo") ? bT : BtnAsigSvcMasivo.Text;
                    IbtClosAsigSvcMasivo.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClosAsigSvcMasivo.ToolTip;
                    GrdSvcsMasivo.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdSvcsMasivo.EmptyDataText;
                    GrdSvcsMasivo.Columns[1].HeaderText = bO.Equals("RdbDet1BuqRte") ? bT : GrdSvcsMasivo.Columns[1].HeaderText;
                    GrdSvcsMasivo.Columns[2].HeaderText = bO.Equals("RdbDet1BuqOT") ? bT : GrdSvcsMasivo.Columns[2].HeaderText;
                    GrdSvcsMasivo.Columns[3].HeaderText = bO.Equals("RdbDet1BuqSvc") ? bT : GrdSvcsMasivo.Columns[4].HeaderText;
                    GrdSvcsMasivo.Columns[4].HeaderText = bO.Equals("GrdDocNro") ? bT : GrdSvcsMasivo.Columns[5].HeaderText;
                    GrdSvcsMasivo.Columns[5].HeaderText = bO.Equals("GrdCodSvc") ? bT : GrdSvcsMasivo.Columns[4].HeaderText;
                    GrdSvcsMasivo.Columns[6].HeaderText = bO.Equals("GrdSvcPpl") ? bT : GrdSvcsMasivo.Columns[6].HeaderText;
                    //**************************************** Plantilla Masiva  ****************************************
                    LblTitPlntllMasiv.Text = bO.Equals("LblTitPlntllMasiv") ? bT : LblTitPlntllMasiv.Text;
                    IbtGuardarCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtGuardarCargaMax") ? tbl["Texto"].ToString().Trim() : IbtGuardarCargaMax.ToolTip;
                    IbtCerrarSubMaxivo.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarSubMaxivo.ToolTip;
                    GrdCargaMax.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdCargaMax.EmptyDataText;
                    GrdCargaMax.Columns[1].HeaderText = bO.Equals("GrdTipoElem") ? bT : GrdCargaMax.Columns[1].HeaderText;
                    GrdCargaMax.Columns[3].HeaderText = bO.Equals("GrdRef") ? bT : GrdCargaMax.Columns[3].HeaderText;
                    GrdCargaMax.Columns[4].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdCargaMax.Columns[4].HeaderText;
                    GrdCargaMax.Columns[5].HeaderText = bO.Equals("GrdCantSN") ? bT : GrdCargaMax.Columns[5].HeaderText;
                    GrdCargaMax.Columns[6].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdCargaMax.Columns[6].HeaderText;
                    GrdCargaMax.Columns[7].HeaderText = bO.Equals("GrdUndComp") ? bT : GrdCargaMax.Columns[7].HeaderText;
                    GrdCargaMax.Columns[8].HeaderText = bO.Equals("GrdUndSist") ? bT : GrdCargaMax.Columns[8].HeaderText;
                    LblTitPnNoExiste.Text = bO.Equals("LblTitPnNoExiste") ? bT : LblTitPnNoExiste.Text;
                    GrdPnNew.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdPnNew.EmptyDataText;
                    GrdPnNew.Columns[2].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdPnNew.Columns[2].HeaderText;
                    GrdPnNew.Columns[3].HeaderText = bO.Equals("GrdCantSN") ? bT : GrdPnNew.Columns[3].HeaderText;
                    LblTitIncosistnc.Text = bO.Equals("LblTitIncosistnc") ? bT : LblTitIncosistnc.Text;
                    GrdInconsist.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdInconsist.EmptyDataText;
                    GrdInconsist.Columns[1].HeaderText = bO.Equals("GrdRef") ? bT : GrdInconsist.Columns[1].HeaderText;
                    GrdInconsist.Columns[3].HeaderText = bO.Equals("GrdDescSn") ? bT : GrdInconsist.Columns[3].HeaderText;
                    GrdInconsist.Columns[4].HeaderText = bO.Equals("GrdCantSN") ? bT : GrdInconsist.Columns[4].HeaderText;
                    GrdInconsist.Columns[5].HeaderText = bO.Equals("GrdUndComp") ? bT : GrdInconsist.Columns[5].HeaderText;
                    GrdInconsist.Columns[6].HeaderText = bO.Equals("GrdUndSist") ? bT : GrdInconsist.Columns[6].HeaderText;
                    //**************************************** Imprimir  ****************************************
                    LblTitImpresion.Text = bO.Equals("LblTitImpresionMstr") ? bT : LblTitImpresion.Text;
                    IbtCerrarImpr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpr.ToolTip;
                    BtnImprPpal.Text = bO.Equals("BtnImprPpal") ? bT : BtnImprPpal.Text;
                    BtnImprDet.Text = bO.Equals("LblTitDetalleGrl") ? bT : BtnImprDet.Text;
                    //**************************************** ALERTAS  ****************************************
                    LblTitAlrt.Text = bO.Equals("LblTitAlrt") ? bT : LblTitAlrt.Text;
                    LblTitAlertaOTDuplicadas.Text = bO.Equals("LblTitAlertaOTDuplicadas") ? bT : LblTitAlertaOTDuplicadas.Text;
                    GrdAlrtOtDuplicada.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAlrtOtDuplicada.EmptyDataText;
                    GrdAlrtOtDuplicada.Columns[0].HeaderText = bO.Equals("LblNumPpt") ? bT : GrdAlrtOtDuplicada.Columns[0].HeaderText;
                    GrdAlrtOtDuplicada.Columns[1].HeaderText = bO.Equals("RdbDet1BuqOT") ? bT : GrdAlrtOtDuplicada.Columns[1].HeaderText;
                    LblTitAlertaSinDetAprob.Text = bO.Equals("LblTitAlertaSinDetAprob") ? bT : LblTitAlertaSinDetAprob.Text;
                    GrdAlrtDetSinAprb.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAlrtDetSinAprb.EmptyDataText;
                    GrdAlrtDetSinAprb.Columns[1].HeaderText = bO.Equals("LblNumPpt") ? bT : GrdAlrtDetSinAprb.Columns[1].HeaderText;
                    BtnCerrarAlerta.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCerrarAlerta.Text;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'OnClRegresarEstado'");
                foreach (DataRow row in Result) { IbtReturnEstado.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'OnClDesaprobarPPT'");
                foreach (DataRow row in Result) { IbtDesaprobar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'OnClGnrNotifc'");
                foreach (DataRow row in Result)
                {
                    BtnNotfPCP.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfLog.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfAprob.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfCumpld.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfCancel.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfDevolc.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                    BtnNotfNoAprob.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');";
                }

                Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result) { BtnEliminar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                Result = Idioma.Select("Objeto= 'BtnAsigSvcMasivoOnCl'");
                foreach (DataRow row in Result) { BtnAsigSvcMasivo.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                Result = Idioma.Select("Objeto= 'GuardarCargaMaxClientClick'");
                foreach (DataRow row in Result) { IbtGuardarCargaMax.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'IbtAprDet1AllOnCl'");
                foreach (DataRow row in Result) { IbtAprDet1All.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'IbtDesAprDet1AllOnCl'");
                foreach (DataRow row in Result) { IbtDesAprDet1All.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {

            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                foreach (GridViewRow Row in GrdDet1.Rows)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null) { Row.Cells[0].Controls.Remove(imgE); }
                }
                foreach (GridViewRow Row in GrdElementos.Rows)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null) { Row.Cells[5].Controls.Remove(imgD); }
                }
                foreach (GridViewRow Row in GrdAeronave.Rows)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null) { Row.Cells[4].Controls.Remove(imgD); }
                }
                foreach (GridViewRow Row in GrdServicios.Rows)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null) { Row.Cells[7].Controls.Remove(imgE); }
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null) { Row.Cells[7].Controls.Remove(imgD); }
                }
            }
            else
            {
                foreach (GridViewRow Row in GrdDet1.Rows)
                {

                    if ((int)ViewState["VblModMS"] == 0)
                    {
                        ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                        if (imgE != null) { Row.Cells[0].Controls.Remove(imgE); }
                        if ((int)ViewState["VblEliMS"] == 0)
                        {
                            ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                            if (imgD != null) { Row.Cells[0].Controls.Remove(imgD); }
                        }
                    }
                }
                foreach (GridViewRow Row in GrdElementos.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null) { Row.Cells[5].Controls.Remove(imgD); }
                    }
                }
                foreach (GridViewRow Row in GrdAeronave.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null) { Row.Cells[4].Controls.Remove(imgD); }
                    }
                }
                foreach (GridViewRow Row in GrdServicios.Rows)
                {
                    if ((int)ViewState["VblModMS"] == 0)
                    {
                        ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                        if (imgE != null) { Row.Cells[7].Controls.Remove(imgE); }
                    }
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null) { if (imgD != null) { Row.Cells[7].Controls.Remove(imgD); } }
                    }

                }
            }
        }
        protected void ActivarGrd(string Tipo)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            switch (Tipo)
            {
                case "00001":// Venta                       
                    BtnDetalle.Visible = false;
                    GrdDet1.ShowFooter = true;
                    break;
                case "00002":// Repa Aeronave
                    Result = Idioma.Select("Objeto= 'LblTitHK'");
                    foreach (DataRow RowI in Result)
                    { LblTitSNHK.Text = RowI["Texto"].ToString().Trim(); }// Reparación aeronave
                    BindAeronaveRepa("UPDATE");
                    GrdElementos.Visible = false; GrdAeronave.Visible = true; BtnDetalle.Visible = true;
                    LblTitOt.Visible = true; GrdServicios.Visible = true;
                    LblTitPnSugerido.Visible = true; GrdPnSugerd.Visible = true;
                    LblTitDetalleMH.Visible = true; GrdMO.Visible = true;
                    break;
                case "00003":// Repa Elemento
                    Result = Idioma.Select("Objeto= 'LblTitSN'");
                    foreach (DataRow RowI in Result)
                    { LblTitSNHK.Text = RowI["Texto"].ToString().Trim(); }// Reparación Elementos
                    BindElementosRepa("UPDATE");
                    GrdElementos.Visible = true; GrdAeronave.Visible = false; BtnDetalle.Visible = true;
                    LblTitOt.Visible = true; GrdServicios.Visible = true;
                    LblTitPnSugerido.Visible = true; GrdPnSugerd.Visible = true;
                    LblTitDetalleMH.Visible = true; GrdMO.Visible = true;
                    break;
                case "00004":// A todo costo
                    BtnDetalle.Visible = true;
                    Result = Idioma.Select("Objeto= 'LblTitHK'");
                    foreach (DataRow RowI in Result)
                    { LblTitSNHK.Text = RowI["Texto"].ToString().Trim(); }// Reparación aeronave
                    BindAeronaveRepa("UPDATE");
                    GrdElementos.Visible = false; GrdAeronave.Visible = true; BtnDetalle.Visible = true;
                    LblTitOt.Visible = false; GrdServicios.Visible = false;
                    LblTitPnSugerido.Visible = false; GrdPnSugerd.Visible = false;
                    LblTitDetalleMH.Visible = false; GrdMO.Visible = false;
                    break;

            }
        }
        protected void BindDdlPnMat()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0, @Idm,@ICC,'01-01-01','01-01-01','01-01-01'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSDdl = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DTPNMat);
                            ViewState["DTPNMat"] = DTPNMat;
                        }
                    }
                }
            }
        }
        protected void BindDataDdlPpal(string Accion, string Activ)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = " EXEC  SP_PANTALLA_PROPUESTA_V2 17,'','','','','DataPpl',0,0,@idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSDdl);
                                DSDdl.Tables[0].TableName = "HKSinPPT";
                                DSDdl.Tables[1].TableName = "HKConSubPT";
                                DSDdl.Tables[2].TableName = "TipoPT";
                                DSDdl.Tables[3].TableName = "Cliente";
                                DSDdl.Tables[4].TableName = "Moneda";
                                DSDdl.Tables[5].TableName = "TipoSol";
                                DSDdl.Tables[6].TableName = "EstadoPT";
                                DSDdl.Tables[7].TableName = "ALL";
                                ViewState["DSDdl"] = DSDdl;
                            }
                        }
                    }
                }
            }
            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result;
            string VblCond = "", VbCodAnt = "";

            VbCodAnt = DdlTipo.Text.Trim();
            DdlTipo.DataSource = DSDdl.Tables["TipoPT"];
            DdlTipo.DataTextField = "Descripcion";
            DdlTipo.DataValueField = "Codigo";
            DdlTipo.DataBind();
            DdlTipo.Text = VbCodAnt;

            VbCodAnt = DdlCliente.Text.Trim();
            DdlCliente.DataSource = DSDdl.Tables["Cliente"];
            DdlCliente.DataTextField = "Descripcion";
            DdlCliente.DataValueField = "Codigo";
            DdlCliente.DataBind();
            DdlCliente.Text = VbCodAnt;

            DataTable TipoPago = new DataTable();
            VbCodAnt = DdlFormPag.Text.Trim();
            TipoPago = DSDdl.Tables["ALL"].Clone();
            Result = DSDdl.Tables["ALL"].Select("Filtro='TipoPago' AND Codigo=''");
            foreach (DataRow Row in Result)
            { TipoPago.ImportRow(Row); }
            if (Activ.Equals("1"))
            {
                VblCond = "Filtro='TipoPago' AND Activo=1";
                Result = DSDdl.Tables["ALL"].Select("Filtro='TipoPago' AND Codigo='" + VbCodAnt + "'");
                foreach (DataRow Row in Result)
                { TipoPago.ImportRow(Row); }
            }
            else { VblCond = "Filtro='TipoPago'"; }
            Result = DSDdl.Tables["ALL"].Select(VblCond);
            foreach (DataRow Row in Result)
            { TipoPago.ImportRow(Row); }
            DdlFormPag.DataSource = TipoPago;
            DdlFormPag.DataTextField = "Descripcion";
            DdlFormPag.DataValueField = "Codigo";
            DdlFormPag.DataBind();
            DdlFormPag.Text = VbCodAnt;

            VbCodAnt = DdlMoned.Text.Trim();
            DdlMoned.DataSource = DSDdl.Tables["Moneda"];
            DdlMoned.DataTextField = "Descripcion";
            DdlMoned.DataValueField = "Codigo";
            DdlMoned.DataBind();
            DdlMoned.Text = VbCodAnt;

            VbCodAnt = DdlTipoSol.Text.Trim();
            DdlTipoSol.DataSource = DSDdl.Tables["TipoSol"];
            DdlTipoSol.DataTextField = "Descripcion";
            DdlTipoSol.DataValueField = "Codigo";
            DdlTipoSol.DataBind();
            DdlTipoSol.Text = VbCodAnt;
            if (DSDdl.Tables["EstadoPT"].Rows.Count > 0)
            {
                VbCodAnt = DdlEstado.Text.Trim();
                DdlEstado.DataSource = DSDdl.Tables[6];
                DdlEstado.DataTextField = "Descripcion";
                DdlEstado.DataValueField = "Codigo";
                DdlEstado.DataBind();
                DdlEstado.Text = VbCodAnt;
            }
        }
        protected void BindDataDdlPptPpal(string Cod, string Accion)
        {
            string VbCodAnt = "";
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = " EXEC SP_PANTALLA_Propuesta 16,@CdC,'','','WEB',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@CdC", DdlCliente.Text.Trim());
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DtDdlPptPpal);
                            DdlPptSuper.Text = "";
                            ViewState["DtDdlPptPpal"] = DtDdlPptPpal;
                        }
                    }
                }
            }
            DtDdlPptPpal = (DataTable)ViewState["DtDdlPptPpal"];
            DataTable PptPpl = new DataTable();
            VbCodAnt = DdlPptSuper.Text.Trim();
            PptPpl = DtDdlPptPpal.Clone();
            string borr = "IdCliente = " + (Cod.Trim().Equals("") ? "0" : Cod.Trim() + ""); //"IdCliente = "  + 
            PptPpl.Rows.Add(" - ", "", 0, "");
            DataRow[] Result = DtDdlPptPpal.Select("IdCliente = " + (Cod.Trim().Equals("") ? "0" : Cod.Trim()));
            foreach (DataRow Row in Result)
            { PptPpl.ImportRow(Row); }
            DdlPptSuper.DataSource = PptPpl;
            DdlPptSuper.DataTextField = "Descripcion";
            DdlPptSuper.DataValueField = "Codigo";
            DdlPptSuper.DataBind();
            DdlPptSuper.Text = VbCodAnt.Trim();
        }
        protected void Traerdatos(string PPT)
        {
            DdlCliente.ToolTip = "";
            Idioma = (DataTable)ViewState["TablaIdioma"];

            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC  SP_PANTALLA_PROPUESTA_V2 1,'','','','','WEB',@PT,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@PT", PPT);

                        SqlDataReader SDRPpl = SC.ExecuteReader();
                        DTEncPPT.Load(SDRPpl);
                        ViewState["DTEncPPT"] = DTEncPPT;
                        ViewState["CamposNuevos"] = "N";
                        foreach (DataRow SDR in DTEncPPT.Rows)
                        {
                            string VbFecSt;
                            DateTime? VbFecDT;
                            TxtNumPpt.Text = SDR["IdPropuesta"].ToString().Trim();
                            TxtCodigoPpt.Text = SDR["CodigoPPT"].ToString().Trim();
                            TxtFecha.Text = Cnx.ReturnFecha(SDR["FechaPropuesta"].ToString().Trim()); //;
                            DdlTipo.Text = SDR["CodTipoPropuesta"].ToString().Trim();
                            DdlCliente.Text = SDR["IdTercero"].ToString().Trim();
                            DdlFormPag.Text = SDR["CodTipoPago"].ToString().Trim();
                            DdlPptSuper.Text = SDR["DanoOculto"].ToString().Trim();
                            if (DdlPptSuper.Text.Trim().Equals("")) { LblMaster.Visible = true; } else { LblMaster.Visible = false; }
                            TxtPptComerc.Text = SDR["DocReferencia"].ToString().Trim();
                            TxtNumContrat.Text = SDR["NumContrato"].ToString().Trim();
                            DdlMoned.Text = SDR["CodTipoMoneda"].ToString().Trim();
                            VbFecSt = SDR["TRM"].ToString().Trim().Equals("") ? "01/01/1900" : SDR["TRM"].ToString().Trim();
                            VbFecDT = Convert.ToDateTime(VbFecSt);
                            TxtFechTRM.Text = String.Format("{0:yyyy-MM-dd}", VbFecDT);
                            TxtValorTrm.Text = SDR["ValorTRM"].ToString().Trim();
                            DdlEstado.Text = SDR["CodEstadoPropuesta"].ToString().Trim();
                            VbFecSt = SDR["FechaAprobacion"].ToString().Trim().Equals("") ? "01/01/1900" : SDR["FechaAprobacion"].ToString().Trim();
                            VbFecDT = Convert.ToDateTime(VbFecSt);
                            TxtFechAprob.Text = VbFecSt.Equals("01/01/1900") ? "" : String.Format("{0:yyyy-MM-dd}", VbFecDT);
                            VbFecSt = HttpUtility.HtmlDecode(SDR["FechaEntrega"].ToString().Trim().Equals("") ? "01/01/1900" : SDR["FechaEntrega"].ToString().Trim()); //SDR["FechaEntrega"].ToString().Trim()
                            VbFecDT = Convert.ToDateTime(VbFecSt);
                            TxtFechEntreg.Text = String.Format("{0:yyyy-MM-dd}", VbFecDT);
                            VbFecSt = HttpUtility.HtmlDecode(SDR["FechaValidez"].ToString().Trim().Equals("") ? "01/01/1900" : SDR["FechaValidez"].ToString().Trim()); // HttpUtility.HtmlDecode(SDR["FechaValidez"].ToString().Trim());
                            VbFecDT = Convert.ToDateTime(VbFecSt);
                            TxtFechValidez.Text = String.Format("{0:yyyy-MM-dd}", VbFecDT);
                            VbFecSt = HttpUtility.HtmlDecode(SDR["FechaEntregaTrabajo"].ToString().Trim().Equals("") ? "01/01/1900" : SDR["FechaEntregaTrabajo"].ToString().Trim()); //   HttpUtility.HtmlDecode(SDR["FechaEntregaTrabajo"].ToString().Trim());
                            VbFecDT = Convert.ToDateTime(VbFecSt);
                            TxtFechEntregTrab.Text = String.Format("{0:yyyy-MM-dd}", VbFecDT);
                            DdlTipoSol.Text = SDR["IdTipoSolicitudPropuesta"].ToString().Trim();
                            RdbSinDanOcul.Checked = SDR["DanoOc"].ToString().Trim().Equals("1") ? true : false;
                            RdbDanOcul.Checked = SDR["DanoOc"].ToString().Trim().Equals("2") ? true : false;
                            TxtAjusVent.Text = SDR["AjusVta"].ToString().Trim();
                            TxtAjusVentN.Text = SDR["Miscelaneos"].ToString().Trim();
                            TxtVlrRecurso.Text = SDR["VlrRecurso"].ToString().Trim();
                            TxtVlrRecursoN.Text = SDR["VlrRepuestoEP"].ToString().Trim();
                            TxtVlrMnObr.Text = SDR["VlrHH"].ToString().Trim();
                            TxtVlrMnObrN.Text = SDR["VlorTotalHHEP"].ToString().Trim();
                            TxtSubTtl.Text = SDR["SubTtal"].ToString().Trim();
                            TxtSubTtlN.Text = SDR["ValorBruto"].ToString().Trim();
                            TxtImpuest.Text = SDR["ValorImpuesto"].ToString().Trim();
                            TxtImpuestN.Text = SDR["ValorImpuesto"].ToString().Trim();
                            TxtVlrImpuest.Text = SDR["VlrImpt"].ToString().Trim();
                            TxtVlrImpuestN.Text = SDR["Impuesto"].ToString().Trim();
                            TxtTotal.Text = SDR["VlrTotal"].ToString().Trim();
                            TxtTotalN.Text = SDR["ValorNeto"].ToString().Trim();
                            TxtMotvAjust.Text = SDR["NroDeCta"].ToString().Trim();
                            CkbAplicImpuesto.Checked = SDR["AplicaIVA"].ToString().Trim().Equals("1") ? true : false;
                            TxtObserv.Text = SDR["ObservacionRef"].ToString().Trim();
                            TxtGarant.Text = SDR["Garantia"].ToString().Trim();
                            TxtGanacInter.Text = SDR["GananciaInta"].ToString().Trim();
                            TxtGanacNacional.Text = SDR["GananciaNAL"].ToString().Trim();
                            CkbAplicOT.Checked = SDR["EvaluarDesdeOT"].ToString().Trim().Equals("1") ? true : false;
                            TxtCondTiempEntregPpt.Text = HttpUtility.HtmlDecode(SDR["TiempoEntrega"].ToString().Trim());
                            TxtCondFormPagoPpt.Text = HttpUtility.HtmlDecode(SDR["FormaDePagor"].ToString().Trim());
                            TxtCondDanoOcultPpt.Text = HttpUtility.HtmlDecode(SDR["DanoOcultor"].ToString().Trim());
                            TxtCondGarantPpt.Text = HttpUtility.HtmlDecode(SDR["Garantiar"].ToString().Trim());
                            ViewState["ClienteAnt"] = SDR["ClienteAnt"].ToString().Trim();
                            ViewState["FueAprobada"] = SDR["FueAprobada"].ToString().Trim();
                            ViewState["TieneHKAsig"] = SDR["FueAprobada"].ToString().Trim();
                            ViewState["Valorizada"] = SDR["Valorizada"].ToString().Trim();
                            ViewState["Det1"] = SDR["Det1"].ToString().Trim();
                            ViewState["Det2"] = SDR["Det2"].ToString().Trim();
                            ViewState["SinUtilidad"] = SDR["SinUtilidad"].ToString().Trim();
                            ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(SDR["CargaMasiva"].ToString().Trim());
                            BIndDDet1("UPDATE", TxtNumPpt.Text);
                            GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind();
                            GrdMO.DataSource = null; GrdMO.DataBind();
                            GrdDet1.ShowFooter = false;
                            if (DdlTipo.Text.Trim().Equals("00001") && (int)ViewState["VblIngMS"] == 1 && TxtFechAprob.Text.Equals("")) { GrdDet1.ShowFooter = true; }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
            }
        }
        protected void LimpiarCampos()
        {
            TxtNumPpt.Text = "0";
            TxtCodigoPpt.Text = "";
            TxtFecha.Text = "";
            DdlTipo.Text = "";
            DdlCliente.Text = "";
            DdlFormPag.Text = "";
            DdlPptSuper.Text = "";
            TxtPptComerc.Text = "";
            TxtNumContrat.Text = "";
            DdlMoned.Text = "";
            TxtFechTRM.Text = "";
            TxtValorTrm.Text = "0";
            DdlEstado.Text = "";
            TxtFechAprob.Text = "";
            TxtFechEntreg.Text = "";
            TxtFechValidez.Text = "";
            TxtFechEntregTrab.Text = "";
            DdlTipoSol.Text = "";
            RdbSinDanOcul.Checked = true;
            RdbDanOcul.Checked = false;
            TxtAjusVent.Text = "0";
            TxtAjusVentN.Text = "0";
            TxtVlrRecurso.Text = "0";
            TxtVlrRecursoN.Text = "0";
            TxtVlrMnObr.Text = "0";
            TxtVlrMnObrN.Text = "0";
            TxtSubTtl.Text = "0";
            TxtSubTtlN.Text = "0";
            TxtImpuest.Text = "0";
            TxtImpuestN.Text = "0";
            TxtVlrImpuest.Text = "0";
            TxtVlrImpuestN.Text = "0";
            TxtTotal.Text = "0";
            TxtTotalN.Text = "0";
            TxtMotvAjust.Text = "";
            CkbAplicImpuesto.Checked = false;
            TxtObserv.Text = "";
            TxtGarant.Text = "0";
            TxtGanacInter.Text = "0";
            TxtGanacNacional.Text = "0";
            CkbAplicOT.Checked = false;
            GrdDet1.DataSource = null;
            GrdDet1.DataBind();
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Validar"] = "S";
            if (RdbDanOcul.Checked == true && Accion.Equals("INSERT")) { CkbAplicOT.Checked = false; }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens03PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// 
                ViewState["Validar"] = "N"; return;
            }
            if (DdlCliente.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens04PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// 
                ViewState["Validar"] = "N"; return;
            }
            if (DdlFormPag.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens05PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// 
                ViewState["Validar"] = "N"; return;
            }
            if (DdlMoned.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens06PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Moneda
                ViewState["Validar"] = "N"; return;
            }
            if (TxtFechEntreg.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens07PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe ingresar una fecha de entrega de la propuesta.
                ViewState["Validar"] = "N"; TxtFechEntreg.Focus(); return;
            }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechAprob.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    TxtFechAprob.Focus(); ViewState["Validar"] = "N"; return;
                }
            }
            if (!TxtFechEntreg.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechEntreg.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    TxtFechEntreg.Focus(); ViewState["Validar"] = "N"; return;
                }
            }
            if (!TxtFechValidez.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechValidez.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    TxtFechValidez.Focus(); ViewState["Validar"] = "N"; return;
                }
            }
            if (!TxtFechEntregTrab.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechEntregTrab.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    TxtFechEntregTrab.Focus(); ViewState["Validar"] = "N"; return;
                }
            }
            if (DdlTipoSol.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens08PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un tipo de solicitud.
                ViewState["Validar"] = "N"; return;
            }
            if (RdbDanOcul.Checked == true && DdlPptSuper.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens17PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// El daño oculto debe tener una propuesta Principal.
                ViewState["Validar"] = "N"; return;
            }
            TxtAjusVentN.Text = TxtAjusVentN.Text.Equals("") ? "0" : TxtAjusVentN.Text.Trim();
            if (!TxtAjusVentN.Text.Equals("0") && TxtMotvAjust.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens33PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Si tiene ajuste debe ingresar el motivo.
                ViewState["Validar"] = "N"; TxtMotvAjust.Focus(); return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = Otr;
            BtnEditCondic.Enabled = Otr;
            BtnDetalle.Enabled = Otr;
            BtnConsultar.Enabled = Otr;
            BtnImprimir.Enabled = Otr;
            BtnExportPPT.Enabled = Otr;
            BtnExportDet.Enabled = Otr;
            BtnAux.Enabled = Otr;
            BtnNotfPCP.Enabled = Otr;
            BtnNotfLog.Enabled = Otr;
            BtnNotfAprob.Enabled = Otr;
            BtnNotfCancel.Enabled = Otr;
            BtnNotfDevolc.Enabled = Otr;
            BtnNotfNoAprob.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            DdlCliente.ToolTip = "";
            RdbSinDanOcul.ToolTip = "";
            RdbDanOcul.ToolTip = "";
            DdlPptSuper.ToolTip = "";
            DdlTipo.Enabled = Ing;
            DdlCliente.Enabled = Edi;
            DdlFormPag.Enabled = Edi;
            DdlPptSuper.Enabled = Edi;
            TxtPptComerc.Enabled = Edi;
            TxtNumContrat.Enabled = Edi;
            DdlMoned.Enabled = Edi;
            TxtFechTRM.Enabled = Edi;
            TxtValorTrm.Enabled = Edi;
            DdlEstado.Enabled = Ing;
            TxtFechEntreg.Enabled = Edi;
            TxtFechValidez.Enabled = Edi;
            TxtFechEntregTrab.Enabled = Edi;
            DdlTipoSol.Enabled = Edi;
            RdbSinDanOcul.Enabled = Edi;
            RdbDanOcul.Enabled = Edi;
            TxtAjusVent.Visible = Edi == true ? false : true;
            TxtAjusVentN.Visible = Edi == true ? true : false;
            TxtVlrRecurso.Visible = Edi == true ? false : true;
            TxtVlrRecursoN.Visible = Edi == true ? true : false;
            TxtVlrMnObr.Visible = Edi == true ? false : true;
            TxtVlrMnObrN.Visible = Edi == true ? true : false;
            TxtSubTtl.Visible = Edi == true ? false : true;
            TxtSubTtlN.Visible = Edi == true ? true : false;
            TxtImpuest.Visible = Edi == true ? false : true;
            TxtImpuestN.Visible = Edi == true ? true : false;
            TxtVlrImpuest.Visible = Edi == true ? false : true;
            TxtVlrImpuestN.Visible = Edi == true ? true : false;
            TxtTotal.Visible = Edi == true ? false : true;
            TxtTotalN.Visible = Edi == true ? true : false;
            CkbAplicOT.Enabled = Edi;
            if (accion.Equals("UPDATE"))
            {
                if (RdbDanOcul.Checked == true) { CkbAplicOT.Enabled = false; }
                if (DdlTipo.Text.Trim().Equals("00001"))
                { TxtVlrRecursoN.Enabled = true; TxtVlrMnObrN.Enabled = true; TxtSubTtlN.Enabled = true; }
                else { TxtVlrRecursoN.Enabled = false; TxtVlrMnObrN.Enabled = false; TxtSubTtlN.Enabled = false; }
                if (ViewState["Det1"].ToString().Equals("S") || ViewState["Det2"].ToString().Equals("S"))
                {
                    Result = Idioma.Select("Objeto= 'Mens16PPT'");
                    foreach (DataRow row in Result)
                    {
                        DdlCliente.ToolTip = row["Texto"].ToString().Trim(); RdbSinDanOcul.ToolTip = row["Texto"].ToString().Trim();
                        DdlPptSuper.ToolTip = row["Texto"].ToString().Trim();
                        DdlPptSuper.Enabled = false;
                    }// Tiene Det1 o Det2
                }
            }
            TxtMotvAjust.Enabled = Edi;
            CkbAplicImpuesto.Enabled = Edi;
            TxtObserv.Enabled = Edi;
            TxtGarant.Enabled = Edi;
            TxtGanacInter.Enabled = Edi;
            TxtGanacNacional.Enabled = Edi;
            switch (DdlTipo.Text)
            {
                case "00001"://Venta
                    RdbDanOcul.Enabled = false; RdbDanOcul.Checked = false; RdbSinDanOcul.Checked = true;
                    CkbAplicOT.Enabled = false; CkbAplicOT.Checked = false;
                    break;
                case "00004"://A todo costo
                    TxtAjusVentN.Enabled = false; TxtSubTtlN.Enabled = true;
                    DdlPptSuper.Enabled = false; DdlPptSuper.Text = "";
                    RdbDanOcul.Enabled = false; RdbDanOcul.Checked = false; RdbSinDanOcul.Checked = true;
                    CkbAplicOT.Enabled = false; CkbAplicOT.Checked = false;
                    DdlTipoSol.Enabled = false;
                    TxtAjusVentN.Text = "0";
                    break;
            }
        }
        protected void BtnSiModl_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];

            switch (ViewState["Notificacion"])
            {
                case "APROBACION":
                    BIndDPnSinValorizar(TxtNumPpt.Text.Trim());
                    break;
                case "APROBACION2":
                    BindDAprobar();
                    break;
                case "ALLSERVICIOS":
                    AprobAllSvcs();
                    break;
                case "PLANTILLAVENTA":// Cargar plantilla de venta
                    GuardarPlantillaMasiva();
                    break;
                case "EXPORTARPPT":
                    ExportarEnc("", "ENC");// Exportar encabezado todos
                    break;
            }
        }
        protected void BtnNoModl_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            switch (ViewState["Notificacion"])
            {
                case "APROBACION2":
                    DtPnNoValoriz = (DataTable)ViewState["DtPnNoValoriz"];
                    GrdPnNoValorizado.DataSource = DtPnNoValoriz; GrdPnNoValorizado.DataBind();
                    MultVw.ActiveViewIndex = 3;
                    break;
                case "EXPORTARPPT":
                    ExportarEnc(TxtNumPpt.Text.Trim(), "ENC");// Exportar encabezado el de pantalla
                    break;
                default:
                    break;
            }
            CheckBox ChkBoxHeader = (CheckBox)GrdServicios.HeaderRow.FindControl("ChkAll");
            ChkBoxHeader.Checked = ChkBoxHeader.Checked == false ? true : false;
        }
        protected void BtnNotfPCP_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "PCP";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (DdlEstado.Text.Trim().Equals("01"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 4,'',@Us,'','','','','','','PCP',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                Traerdatos(TxtNumPpt.Text.Trim());
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                }
            }
            else
            {
                Result = Idioma.Select("Objeto= 'Mens17PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Ya se encuentra notifica el area.
            }
        }
        protected void BtnNotfLog_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "LOGISTICA";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (DdlEstado.Text.Trim().Equals("02"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 4,'',@Us,'','','','','','','NLOG',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                Traerdatos(TxtNumPpt.Text.Trim());
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                }
            }
            else
            {
                Result = Idioma.Select("Objeto= 'Mens17PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Ya se encuentra notifica el area.
            }
        }
        protected void BindDAprobar()
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string Mensj2 = "";
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 5,'',@Us,'','','','','','','APROB',@PP,0,0,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
                sqlCon.Close();
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                string VBQuery = "EXEC SP_TablasMRO 6,@Tp,@Us,'','','','','','','APROBARPPT',@PP,0,0,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                    SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@Tp", DdlTipo.Text.Trim());
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read()) { Mensj2 = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                    SDR.Close();
                    if (!Mensj2.ToString().Trim().Equals(""))
                    {
                        Result = Idioma.Select("Objeto= '" + Mensj2.ToString().Trim() + "'");
                        if (Result.Length != 0)
                        {
                            foreach (DataRow row in Result)
                            { Mensj2 = row["Texto"].ToString().Trim(); }
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj2 + "');", true);
                        }
                    }
                }
            }
            Traerdatos(TxtNumPpt.Text.Trim());
        }
        protected void BtnNotfAprob_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "APROBACION";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {
                Result = Idioma.Select("Objeto= 'Mens28PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta se encuentra cumplida | cancelada | Devolucion | No aprobada.
                return;
            }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta se encuentra aprobada.
                return;
            }
            if (TxtPptComerc.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens21PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta comercial es requerida.
                return;
            }
            if (ViewState["SinUtilidad"].ToString().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens20PPT'");
                foreach (DataRow row in Result)
                { LblTexMensjModl.Text = row["Texto"].ToString(); } // Existen registros sin valor en el campo utilidad en el detalle. Desea continuar?
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                return;
            }
            else { BIndDPnSinValorizar(TxtNumPpt.Text.Trim()); }
        }
        protected void BtnNotfCumpld_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "CUMPLIDA";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens23PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta debe estar aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 5,'',@Us,'','','','','','','CUMPLID',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void BtnNotfCancel_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "CANCEL";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta se encuentra aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 5,'',@Us,'','','','','','','CANCEL',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void BtnNotfDevolc_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "DEVOLUCION";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta se encuentra aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 5,'',@Us,'','','','','','','DEVOLUCION',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void BtnNotfNoAprob_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Notificacion"] = "NOAPROBADA";
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta se encuentra aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 5,'',@Us,'','','','','','','NOAPROBADA',@PP,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    GrdDet1.DataSource = null; GrdDet1.DataBind();
                    ActivarBtn(true, false, false);
                    BindDataDdlPpal("SELECT", "1");
                    DdlPptSuper.Text = "";
                    BindDataDdlPptPpal("", "SELECT");
                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCampos();
                    ActivarCampos(true, true, "INSERT");
                    DdlEstado.Enabled = false;
                    DdlEstado.Text = "01";
                    DdlTipoSol.Text = "1";
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    ValidarCampos("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }/**/
                    TxtGarant.Text = TxtGarant.Text.Equals("") ? "0" : TxtGarant.Text.Trim();
                    TxtGanacInter.Text = TxtGanacInter.Text.Equals("") ? "0" : TxtGanacInter.Text.Trim();
                    TxtGanacNacional.Text = TxtGanacNacional.Text.Equals("") ? "0" : TxtGanacNacional.Text.Trim();
                    TxtAjusVentN.Text = TxtAjusVentN.Text.Equals("") ? "0" : TxtAjusVentN.Text.Trim();
                    double VbVlrAjus = Convert.ToDouble(TxtAjusVentN.Text);
                    TxtVlrRecursoN.Text = TxtVlrRecursoN.Text.Equals("") ? "0" : TxtVlrRecursoN.Text.Trim();
                    double VbVlrRecF = Convert.ToDouble(TxtVlrRecursoN.Text);
                    TxtVlrMnObrN.Text = TxtVlrMnObrN.Text.Equals("") ? "0" : TxtVlrMnObrN.Text.Trim();
                    double VbVlrHH = Convert.ToDouble(TxtVlrMnObrN.Text);
                    TxtSubTtlN.Text = TxtSubTtlN.Text.Equals("") ? "0" : TxtSubTtlN.Text.Trim();
                    double VbVlrSubTt = Convert.ToDouble(TxtSubTtlN.Text);
                    TxtImpuestN.Text = TxtImpuestN.Text.Equals("") ? "0" : TxtImpuestN.Text.Trim();
                    double VbImpt = Convert.ToDouble(TxtImpuestN.Text);
                    TxtVlrImpuestN.Text = TxtVlrImpuestN.Text.Equals("") ? "0" : TxtVlrImpuestN.Text.Trim();
                    double VbIVlrmpt = Convert.ToDouble(TxtVlrImpuestN.Text);
                    TxtTotalN.Text = TxtTotalN.Text.Equals("") ? "0" : TxtTotalN.Text.Trim();
                    double VbIVlrTtl = Convert.ToDouble(TxtTotalN.Text);
                    TxtValorTrm.Text = TxtValorTrm.Text.Equals("") ? "0" : TxtValorTrm.Text.Trim();
                    double VbIVlrTRM = Convert.ToDouble(TxtValorTrm.Text);

                    DateTime? VbFecApr, VbFecValid, VbFecEntrTrab;
                    if (TxtFechAprob.Text.Trim().Equals("")) { VbFecApr = null; }
                    else { VbFecApr = Convert.ToDateTime(TxtFechAprob.Text); }
                    DateTime? VbFecTrm;
                    if (TxtFechTRM.Text.Trim().Equals("")) { VbFecTrm = null; }
                    else { VbFecTrm = Convert.ToDateTime(TxtFechTRM.Text); }

                    if (TxtFechValidez.Text.Trim().Equals("")) { VbFecValid = null; }
                    else { VbFecValid = Convert.ToDateTime(TxtFechValidez.Text); }

                    if (TxtFechEntregTrab.Text.Trim().Equals("")) { VbFecEntrTrab = null; }
                    else { VbFecEntrTrab = Convert.ToDateTime(TxtFechEntregTrab.Text); }
                    string VbCodcia = Session["!dC!@"].ToString().Trim();

                    List<CsTypEncPropuesta> ObjEncPropuesta = new List<CsTypEncPropuesta>();
                    var TypEncPropuesta = new CsTypEncPropuesta()
                    {
                        IdPropuesta = 0,
                        CodTipoPropuesta = DdlTipo.Text.Trim(),
                        NumContrato = TxtNumContrat.Text.Trim(),
                        DocReferencia = TxtPptComerc.Text.Trim(),
                        ObservacionRef = TxtObserv.Text.Trim(),
                        CodTipoPago = DdlFormPag.Text.Trim(),
                        Garantia = Convert.ToInt32(TxtGarant.Text.Trim()),
                        TiempoEntrega = TxtCondTiempEntreg.Text.Trim(),
                        DanoOculto = DdlPptSuper.Text.Trim(),
                        CodCliente = DdlCliente.Text.Trim(),
                        FechaPropuesta = null,
                        CodTipoMoneda = DdlMoned.Text.Trim(),
                        TRM = VbFecTrm,
                        FechaEntrega = Convert.ToDateTime(TxtFechEntreg.Text),
                        FechaValidez = VbFecValid,
                        CodEstadoPropuesta = DdlEstado.Text.Trim(),
                        IdTipoSolicitudPropuesta = DdlTipoSol.Text.Trim(),
                        ValorBruto = VbVlrSubTt,
                        ValorNeto = VbIVlrTtl,
                        ValorImpuesto = VbImpt,
                        Usu = Session["C77U"].ToString(),
                        Formadepagor = TxtCondFormPago.Text.Trim(),
                        danoocultor = TxtCondDanoOcult.Text.Trim(),
                        garantiar = TxtCondGarant.Text.Trim(),
                        DanoOc = RdbSinDanOcul.Checked == true ? 1 : 2,
                        Impuesto = VbIVlrmpt,
                        VlorTotalHHEP = VbVlrHH,
                        VlrRepuestoEP = VbVlrRecF,
                        FechaEntregaTrabajo = VbFecEntrTrab,
                        ValorTRM = VbIVlrTRM,
                        GananciaNAL = Convert.ToDouble(TxtGanacNacional.Text.Trim()),
                        GananciaInta = Convert.ToDouble(TxtGanacInter.Text.Trim()),
                        AplicaIVA = CkbAplicImpuesto.Checked == true ? 1 : 0,
                        FechaAprobacion = VbFecApr,
                        CodBanco = "",
                        NroDeCta = TxtMotvAjust.Text.Trim(),
                        TipoCuenta = 0,
                        EvaluarDesdeOT = CkbAplicOT.Checked == true ? 1 : 0,
                        IntegradorNeoS = 0,
                        Miscelaneos = VbVlrAjus,
                        AvancePPT = 0,
                        IdConfigCia = Convert.ToInt32(VbCodcia),
                        ClienteAnt = "",
                        Accion = "INSERT",
                    };
                    ObjEncPropuesta.Add(TypEncPropuesta);
                    CsTypEncPropuesta ClsEncPropuesta = new CsTypEncPropuesta();
                    ClsEncPropuesta.Alimentar(ObjEncPropuesta);
                    string Mensj = ClsEncPropuesta.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    TxtNumPpt.Text = ClsEncPropuesta.GetCodPPT().Trim();
                    ActivarBtn(true, true, true);
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//                   
                    ActivarCampos(false, false, "INSERT");
                    BindDataDdlPptPpal(DdlCliente.Text.Trim(), "UPDATE");
                    Traerdatos(TxtNumPpt.Text.Trim());
                    ActivarGrd(DdlTipo.Text.Trim());
                    BindServicios("UPDATE", "0");
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Propuesta", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ActivarCliente()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (!DdlPptSuper.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens10PPT'");
                foreach (DataRow row in Result)
                { DdlCliente.ToolTip = row["Texto"].ToString().Trim(); }//Debe ser una propuesta master
                DdlCliente.Enabled = false;
            }
            if (ViewState["FueAprobada"].ToString().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens11PPT'");
                foreach (DataRow row in Result)
                { DdlCliente.ToolTip = row["Texto"].ToString().Trim(); }//La propuesta fue aprobada en algún momento.
                DdlCliente.Enabled = false;
            }
            if (ViewState["TieneHKAsig"].ToString().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens12PPT'");
                foreach (DataRow row in Result)
                { DdlCliente.ToolTip = row["Texto"].ToString().Trim(); }//La propuesta tiene aeronaves asignadas.
                DdlCliente.Enabled = false;
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;

            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false);
                    BindDataDdlPpal("SELECT", "1");
                    BindDataDdlPptPpal(DdlCliente.Text.Trim(), "SELECT");
                    ViewState["Accion"] = "Aceptar";
                    Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//                  
                    ActivarCampos(false, true, "UPDATE");
                    ActivarCliente();
                    Result = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                }
                else
                {
                    ValidarCampos("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    TxtGarant.Text = TxtGarant.Text.Equals("") ? "0" : TxtGarant.Text.Trim();
                    TxtGanacInter.Text = TxtGanacInter.Text.Equals("") ? "0" : TxtGanacInter.Text.Trim();
                    TxtGanacNacional.Text = TxtGanacNacional.Text.Equals("") ? "0" : TxtGanacNacional.Text.Trim();
                    TxtAjusVentN.Text = TxtAjusVentN.Text.Equals("") ? "0" : TxtAjusVentN.Text.Trim();
                    double VbVlrAjus = Convert.ToDouble(TxtAjusVentN.Text);
                    TxtVlrRecursoN.Text = TxtVlrRecursoN.Text.Equals("") ? "0" : TxtVlrRecursoN.Text.Trim();
                    double VbVlrRecF = Convert.ToDouble(TxtVlrRecursoN.Text);
                    TxtVlrMnObrN.Text = TxtVlrMnObrN.Text.Equals("") ? "0" : TxtVlrMnObrN.Text.Trim();
                    double VbVlrHH = Convert.ToDouble(TxtVlrMnObrN.Text);
                    TxtSubTtlN.Text = TxtSubTtlN.Text.Equals("") ? "0" : TxtSubTtlN.Text.Trim();
                    double VbVlrSubTt = Convert.ToDouble(TxtSubTtlN.Text);
                    TxtImpuestN.Text = TxtImpuestN.Text.Equals("") ? "0" : TxtImpuestN.Text.Trim();
                    double VbImpt = Convert.ToDouble(TxtImpuestN.Text);
                    TxtVlrImpuestN.Text = TxtVlrImpuestN.Text.Equals("") ? "0" : TxtVlrImpuestN.Text.Trim();
                    double VbIVlrmpt = Convert.ToDouble(TxtVlrImpuestN.Text);
                    TxtTotalN.Text = TxtTotalN.Text.Equals("") ? "0" : TxtTotalN.Text.Trim();
                    double VbIVlrTtl = Convert.ToDouble(TxtTotalN.Text);
                    TxtValorTrm.Text = TxtValorTrm.Text.Equals("") ? "0" : TxtValorTrm.Text.Trim();
                    double VbIVlrTRM = Convert.ToDouble(TxtValorTrm.Text);

                    DateTime? VbFecApr;
                    if (TxtFechAprob.Text.Trim().Equals("")) { VbFecApr = null; }
                    else { VbFecApr = Convert.ToDateTime(TxtFechAprob.Text); }
                    DateTime? VbFecTrm;
                    if (TxtFechTRM.Text.Trim().Equals("")) { VbFecTrm = null; }
                    else { VbFecTrm = Convert.ToDateTime(TxtFechTRM.Text); }
                    string VbCodcia = Session["!dC!@"].ToString().Trim();
                    List<CsTypEncPropuesta> ObjEncPropuesta = new List<CsTypEncPropuesta>();
                    var TypEncPropuesta = new CsTypEncPropuesta()
                    {
                        IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.Trim()),
                        CodTipoPropuesta = DdlTipo.Text.Trim(),
                        NumContrato = TxtNumContrat.Text.Trim(),
                        DocReferencia = TxtPptComerc.Text.Trim(),
                        ObservacionRef = TxtObserv.Text.Trim(),
                        CodTipoPago = DdlFormPag.Text.Trim(),
                        Garantia = Convert.ToInt32(TxtGarant.Text.Trim()),
                        TiempoEntrega = TxtCondTiempEntregPpt.Text.Trim(),
                        DanoOculto = DdlPptSuper.Text.Trim(),
                        CodCliente = DdlCliente.Text.Trim(),
                        FechaPropuesta = null,
                        CodTipoMoneda = DdlMoned.Text.Trim(),
                        TRM = VbFecTrm,
                        FechaEntrega = Convert.ToDateTime(TxtFechEntreg.Text),
                        FechaValidez = Convert.ToDateTime(TxtFechValidez.Text),
                        CodEstadoPropuesta = DdlEstado.Text.Trim(),
                        IdTipoSolicitudPropuesta = DdlTipoSol.Text.Trim(),
                        ValorBruto = VbVlrSubTt,
                        ValorNeto = VbIVlrTtl,
                        ValorImpuesto = VbImpt,
                        Usu = Session["C77U"].ToString(),
                        Formadepagor = TxtCondFormPagoPpt.Text.Trim(),
                        danoocultor = TxtCondDanoOcultPpt.Text.Trim(),
                        garantiar = TxtCondGarantPpt.Text.Trim(),
                        DanoOc = RdbSinDanOcul.Checked == true ? 1 : 2,
                        Impuesto = VbIVlrmpt,
                        VlorTotalHHEP = VbVlrHH,
                        VlrRepuestoEP = VbVlrRecF,
                        FechaEntregaTrabajo = Convert.ToDateTime(TxtFechEntregTrab.Text),
                        ValorTRM = VbIVlrTRM,
                        GananciaNAL = Convert.ToDouble(TxtGanacNacional.Text.Trim()),
                        GananciaInta = Convert.ToDouble(TxtGanacInter.Text.Trim()),
                        AplicaIVA = CkbAplicImpuesto.Checked == true ? 1 : 0,
                        FechaAprobacion = VbFecApr,
                        CodBanco = "",
                        NroDeCta = TxtMotvAjust.Text.Trim(),
                        TipoCuenta = 0,
                        EvaluarDesdeOT = CkbAplicOT.Checked == true ? 1 : 0,
                        IntegradorNeoS = 0,
                        Miscelaneos = VbVlrAjus,
                        AvancePPT = 0,
                        IdConfigCia = Convert.ToInt32(VbCodcia),
                        ClienteAnt = ViewState["ClienteAnt"].ToString().Trim(),
                        Accion = "UPDATE",
                    };
                    ObjEncPropuesta.Add(TypEncPropuesta);
                    CsTypEncPropuesta ClsEncPropuesta = new CsTypEncPropuesta();
                    ClsEncPropuesta.Alimentar(ObjEncPropuesta);
                    string Mensj = ClsEncPropuesta.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ViewState["Accion"] = "";
                    Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }/**/
                    Traerdatos(TxtNumPpt.Text.Trim());
                    ActivarBtn(true, true, true);
                    ActivarCampos(false, false, "UPDATE");
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Modificar Propuesta", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (TxtNumPpt.Text.Trim().Equals("") || TxtNumPpt.Text.Trim().Equals("0")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_PROPUESTA_V2 20,@Usu,'','','','',@PP,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { VbMensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                                return;
                            }
                            Transac.Commit();
                            LimpiarCampos();
                            GrdDet1.DataSource = null; GrdDet1.DataBind();
                            GrdElementos.DataSource = null; GrdElementos.DataBind();
                            GrdAeronave.DataSource = null; GrdAeronave.DataBind();
                            GrdServicios.DataSource = null; GrdServicios.DataBind();
                            GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind();
                            GrdMO.DataSource = null; GrdMO.DataBind();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE PROPUESTA", Ex.StackTrace.Substring(0, Ex.StackTrace.Length > 300 ? 300 : Ex.StackTrace.Length), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnEditCondic_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 2; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnDetalle_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            Result = Idioma.Select("Objeto= 'Mens43PPT'");
            foreach (DataRow row in Result)
            { LblTitTrabajos.Text = row["Texto"].ToString().Trim() + " [" + TxtCodigoPpt.Text.Trim() + "]  [" + DdlTipo.SelectedItem.Text.Trim() + "]"; }//Propuesta Nro:

            MultVw.ActiveViewIndex = 4;
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; TxtBusqueda.Focus(); }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (ViewState["Valorizada"].ToString().Equals("N") && !DdlTipo.Text.Trim().Equals("00004")) { return; }
            BtnImprDet.Visible = true;
            if (DdlTipo.Text.Trim().Equals("00004")) { BtnImprDet.Visible = false; }// a todo costo no tiene detalle
            MultVw.ActiveViewIndex = 7;
        }
        protected void BtnExportPPT_Click(object sender, EventArgs e)
        {
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Notificacion"] = "EXPORTARPPT";
            DataRow[] Result = Idioma.Select("Objeto= 'Mens66PPT'");
            foreach (DataRow row in Result)
            { LblTexMensjModl.Text = row["Texto"].ToString(); } //
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
        }
        protected void BtnExportDet_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            ExportarEnc(TxtNumPpt.Text.Trim(), "DET");// Exportar el detalle 
        }
        protected void ExportarEnc(string Opcion, string EncDet)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            string VbTxtSql = "";

            if (EncDet.Equals("ENC"))
            {
                CursorIdioma.Alimentar("CurExportEncPPT", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC SP_PANTALLA_Propuesta 49,'WEB',@PT,ENC,'CurExportEncPPT',0,0, @idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                VbNomRpt = "Commercial_Quotation";
            }
            else
            {
                CursorIdioma.Alimentar("CurExportDetPPT", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC SP_PANTALLA_Propuesta 49,'WEB',@PT,'DET','CurExportDetPPT',0,0, @idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                VbNomRpt = "DetailQuotation";
            }
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    SC.Parameters.AddWithValue("@PT", Opcion);
                    SC.Parameters.AddWithValue("@idm", Session["77IDM"]);
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
        protected void DdlTipo_TextChanged(object sender, EventArgs e)
        {
            if (!DdlTipo.Text.Trim().Equals("00004"))
            {
                TxtVlrRecursoN.Text = "0";
                TxtVlrMnObrN.Text = "0";
                TxtSubTtlN.Enabled = false; TxtSubTtlN.Text = "0";
                DdlPptSuper.Enabled = true; DdlPptSuper.Text = "";
                RdbDanOcul.Enabled = true;
                CkbAplicOT.Enabled = true;
                DdlTipoSol.Enabled = true;
                TxtAjusVentN.Enabled = true;
                if (DdlTipo.Text.Trim().Equals("00001"))
                {
                    RdbDanOcul.Enabled = false; RdbDanOcul.Checked = false; RdbSinDanOcul.Checked = true;
                    CkbAplicOT.Enabled = false; CkbAplicOT.Checked = false;
                }
            }
            else
            {
                TxtSubTtlN.Enabled = true; DdlPptSuper.Enabled = false; DdlPptSuper.Text = "";
                RdbDanOcul.Enabled = false; RdbDanOcul.Checked = false; RdbSinDanOcul.Checked = true;
                CkbAplicOT.Enabled = false; CkbAplicOT.Checked = false;
                DdlTipoSol.Enabled = false;
                TxtAjusVentN.Enabled = false; TxtAjusVentN.Text = "0";
            }
        }
        protected void DdlCliente_TextChanged(object sender, EventArgs e)
        { DdlPptSuper.Text = ""; BindDataDdlPptPpal(DdlCliente.Text.Trim(), "SELECT"); }
        protected void BtnAux_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); Response.Redirect("~/Forms/MRO/FrmInformePropuesta.aspx"); }
        protected void IbtReturnEstado_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;

            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (ViewState["Valorizada"].ToString().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens15PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra valorizada
                return;
            }
            ActivarBtn(false, false, false);

            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_PROPUESTA_V2 4,@Eact,'','','','',@IPP,0,@idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    DataTable DtDdlEstd = new DataTable();
                    SC.Parameters.AddWithValue("@Eact", DdlEstado.Text.Trim());
                    SC.Parameters.AddWithValue("@IPP", TxtNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        ViewState["CodAnt"] = DdlEstado.Text.Trim();
                        ViewState["NomEstdAnt"] = DdlEstado.SelectedItem.Text.Trim();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DtDdlEstd);
                        DtDdlEstd.Rows.Add(ViewState["NomEstdAnt"], ViewState["CodAnt"]);

                        DdlEstado.DataSource = DtDdlEstd;
                        DdlEstado.DataTextField = "DescripcionEstado";
                        DdlEstado.DataValueField = "CodEstadoPro";
                        DdlEstado.DataBind();
                        DdlEstado.Text = ViewState["CodAnt"].ToString().Trim();
                        DdlEstado.Enabled = true;
                        IbtReturnEstado.Visible = false;
                        IbtActualizarEstado.Visible = true;
                    }
                }
            }
        }
        protected void IbtActualizarEstado_Click(object sender, ImageClickEventArgs e)
        {
            if (!ViewState["CodAnt"].ToString().Trim().Equals(DdlEstado.Text.Trim()))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 2,@CEA,@Us,@NEAtr,@NEAct,'','','','','',@PP,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@CEA", DdlEstado.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@NEAtr", ViewState["NomEstdAnt"]);
                            SC.Parameters.AddWithValue("@NEAct", DdlEstado.SelectedItem.Text.Trim());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                Traerdatos(TxtNumPpt.Text.Trim());
                            }
                            catch (Exception)
                            { Transac.Rollback(); }
                        }
                    }
                }
            }
            DdlEstado.Enabled = false;
            IbtReturnEstado.Visible = true;
            IbtActualizarEstado.Visible = false;
            ActivarBtn(true, true, true);
        }
        protected void IbtDesaprobar_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (TxtFechAprob.Text.Trim().Equals("")) { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 3,'',@Us,'','','','','','','',@PP,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception)
                        { Transac.Rollback(); }
                    }
                }
            }
        }
        // *************************************** DETALLE PRINCIPAL ***************************************
        protected void BIndDDet1(string Tipo, string Ppt)
        {
            try
            {
                if (TxtNumPpt.Text.Trim().Equals("")) { return; }
                if (Tipo.Trim().Equals("SELECT"))
                {
                    string Filtro = "ALL";
                    if (RdbDet1BuqAll.Checked == true) { Filtro = "ALL"; }
                    if (RdbDet1BuqPN.Checked == true) { Filtro = "PN"; }
                    if (RdbDet1BuqOT.Checked == true) { Filtro = "OT"; }
                    if (RdbDet1BuqRte.Checked == true) { Filtro = "RTE"; }
                    if (RdbDet1BuqSvc.Checked == true) { Filtro = "SVC"; }

                    DtDet1All = (DataTable)ViewState["DtDet1"];
                    DataTable DtDet1 = new DataTable();
                    DataRow[] Result;
                    DtDet1 = DtDet1All.Clone();
                    switch (Filtro)
                    {
                        case "ALL":
                            GrdDet1.DataSource = DtDet1All;
                            break;
                        case "PN":
                            Result = DtDet1All.Select("PN LIKE'%" + TxtBusqDet1.Text.Trim() + "%'");
                            foreach (DataRow Row in Result)
                            { DtDet1.ImportRow(Row); } /**/
                            GrdDet1.DataSource = DtDet1;
                            break;
                        case "OT":
                            Result = DtDet1All.Select("CodigoOT LIKE '%" + TxtBusqDet1.Text.Trim() + "%'");
                            foreach (DataRow Row in Result)
                            { DtDet1.ImportRow(Row); } /**/
                            GrdDet1.DataSource = DtDet1;
                            break;
                        case "RTE":
                            Result = DtDet1All.Select("CodigoRTE LIKE '%" + TxtBusqDet1.Text.Trim() + "%'");
                            foreach (DataRow Row in Result)
                            { DtDet1.ImportRow(Row); } /**/
                            GrdDet1.DataSource = DtDet1;
                            break;
                        case "SVC":
                            Result = DtDet1All.Select("DescricionServicio LIKE'%" + TxtBusqDet1.Text.Trim() + "%'");
                            foreach (DataRow Row in Result)
                            { DtDet1.ImportRow(Row); } /**/
                            GrdDet1.DataSource = DtDet1;
                            break;
                    }
                    GrdDet1.DataBind();
                }
                else
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {

                        string VbTxtSql = "EXEC SP_PANTALLA_Propuesta 5,'','','','',@IPpt, @ICC, 0, 0, '01-1-2009', '01-01-1900', '01-01-1900'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@IPpt", Ppt);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                            using (SqlDataAdapter DAB = new SqlDataAdapter())
                            {
                                DAB.SelectCommand = SC;
                                DAB.Fill(DtDet1All);
                                if (DtDet1All.Rows.Count > 0) { GrdDet1.DataSource = DtDet1All; GrdDet1.DataBind(); }
                                else
                                {
                                    DtDet1All.Rows.Add(DtDet1All.NewRow());
                                    GrdDet1.DataSource = DtDet1All;
                                    GrdDet1.DataBind();
                                    GrdDet1.Rows[0].Cells.Clear();
                                    GrdDet1.Rows[0].Cells.Add(new TableCell());
                                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                                    foreach (DataRow row in Result)
                                    { GrdDet1.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                    GrdDet1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                }
                                ViewState["DtDet1"] = DtDet1All;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Dato inválido.
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE Detalle PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GuardarPlantillaMasiva()
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            List<CsTypDetallePropuesta> ObjDetallePropuesta = new List<CsTypDetallePropuesta>();
            foreach (GridViewRow Row in GrdCargaMax.Rows)
            {
                string LblDescSvc = (Row.FindControl("TxtPNRF") as TextBox).Text.Trim();
                var TypDetallePropuesta = new CsTypDetallePropuesta()
                {
                    IdDetPropuesta = Convert.ToInt32(0),
                    IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.Trim()),
                    PN = (Row.FindControl("TxtPNRF") as TextBox).Text.Trim(),
                    Descripcion = (Row.FindControl("TxtDesRF") as TextBox).Text.Trim(),
                    CantidadSol = Convert.ToDouble((Row.FindControl("TxtCantRF") as TextBox).Text.Trim()),
                    PorcentajeUtilidad = Convert.ToDouble(0),
                    ValorUnd = Convert.ToDouble(0),
                    CostoVenta = Convert.ToDouble(0),
                    TiempoEntregaDias = Convert.ToInt32(0),
                    TiempoEntregaDiasCoti = Convert.ToInt32(0),
                    IdServicio = Convert.ToInt32(0),
                    SelectComprar = Convert.ToInt32(0),
                    Posicion = Convert.ToInt32((Row.FindControl("TxtPosRF") as TextBox).Text.Trim()),
                    Aprobado = 0,
                    NomServicio = "",
                    Usu = Session["C77U"].ToString(),
                    IdReporte = Convert.ToInt32(0),
                    EstadoPosicion = "",
                    CantidadEntregada = Convert.ToInt32(0),
                    UnidadMedida = (Row.FindControl("TxtUndDespch") as TextBox).Text.Trim(),
                    CodMoneda = DdlMoned.Text.Trim(),
                    ValorMonedaProp = Convert.ToDouble(0),
                    IVA = Convert.ToDouble(TxtImpuest.Text.Trim()),
                    ValorTotal = Convert.ToDouble(0),
                    ValorConImpuesto = Convert.ToDouble(0),
                    UnidMinCompra = Convert.ToDouble(0),
                    CodEstado = "",
                    ObservacionesDP = "",
                    PnAlterno = "",
                    TipoCotizacion = "",
                    IdDetPropSrv = Convert.ToInt32(0),
                    RepaExterna = Convert.ToInt32(0),
                    CantRealDP = Convert.ToDouble(0),
                    UndCompraDPV = (Row.FindControl("TxtUndCompraSys") as TextBox).Text.Trim(),
                    IdConfigCia = Convert.ToInt32(Session["!dC!@"].ToString()),
                    CodTipoPT = DdlTipo.Text.Trim(),
                    Accion = "INSERT",
                };
                ObjDetallePropuesta.Add(TypDetallePropuesta);
            }
            CsTypDetallePropuesta ClsTypDetallePropuesta = new CsTypDetallePropuesta();
            ClsTypDetallePropuesta.GananciaAnterior(Convert.ToDouble(TxtGanacNacional.Text), Convert.ToDouble(TxtGanacInter.Text));
            ClsTypDetallePropuesta.Alimentar(ObjDetallePropuesta);
            string Mensj = ClsTypDetallePropuesta.GetMensj();
            if (!Mensj.Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                return;
            }
            BIndDDet1("UPDATE", TxtNumPpt.Text);

            GrdCargaMax.DataSource = null; GrdCargaMax.DataBind();
            GrdPnNew.DataSource = null; GrdPnNew.DataBind();
            GrdInconsist.DataSource = null; GrdInconsist.DataBind();
            IbtGuardarCargaMax.Visible = false;
            MultVw.ActiveViewIndex = 0;

        }
        protected void UpdateAproAll(int VlrAprob)
        {
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!DdlTipo.Text.Trim().Equals("00001")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 13,@CT,@Us,'','','','','','','APROBAR_ALL',@PP,@GnNl,@GnIt,@VlrA,@ICC,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@CT", DdlTipo.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@GnNl", TxtGanacNacional.Text.Trim());
                            SC.Parameters.AddWithValue("@GnIt", TxtGanacInter.Text.Trim());
                            SC.Parameters.AddWithValue("@VlrA", VlrAprob);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensIncovCons'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE Detalle PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void IbtAprDet1All_Click(object sender, ImageClickEventArgs e)
        { UpdateAproAll(1); }
        protected void IbtDesAprDet1All_Click(object sender, ImageClickEventArgs e)
        { UpdateAproAll(0); }
        protected void IbtConsultarDet1_Click(object sender, ImageClickEventArgs e)
        { BIndDDet1("SELECT", TxtNumPpt.Text); }
        protected void DdlPNRFPP_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            PerfilesGrid();
            DataRow[] Result;
            DTPNMat = (DataTable)ViewState["DTPNMat"];

            DropDownList DdlPNRFPP = (GrdDet1.FooterRow.FindControl("DdlPNRFPP") as DropDownList);
            TextBox TxtPNRFPP = (GrdDet1.FooterRow.FindControl("TxtPNRFPP") as TextBox);
            TextBox TxtDescPNPP = (GrdDet1.FooterRow.FindControl("TxtDescPNPP") as TextBox);
            if (DdlPNRFPP.Text.Trim().Equals("- N -"))
            {
                DdlPNRFPP.Visible = false;
                TxtPNRFPP.Visible = true;
                TxtPNRFPP.Enabled = true;
                TxtDescPNPP.Text = "";
            }
            else
            {
                Result = DTPNMat.Select("CodPN ='" + DdlPNRFPP.Text.Trim() + "'");
                foreach (DataRow Row in Result)
                { TxtDescPNPP.Text = Row["Descripcion"].ToString(); }
            }
        }
        protected void GrdDet1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }
            PerfilesGrid();

            if (e.CommandName.Equals("AddNew"))
            {
                string VbPN = (GrdDet1.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Visible == true ? (GrdDet1.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Text.Trim() : (GrdDet1.FooterRow.FindControl("TxtPNRFPP") as TextBox).Text.Trim();
                string VbCant = (GrdDet1.FooterRow.FindControl("TxtCantSolPP") as TextBox).Text.Equals("") ? "0" : (GrdDet1.FooterRow.FindControl("TxtCantSolPP") as TextBox).Text.Trim();
                string VlrUnd = (GrdDet1.FooterRow.FindControl("TxtVlrUndPP") as TextBox).Text.Equals("") ? "0" : (GrdDet1.FooterRow.FindControl("TxtVlrUndPP") as TextBox).Text.Trim();
                if (VbPN.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'Mens61PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N.
                    return;
                }

                if (Convert.ToInt32(VbCant) <= 0)
                {
                    Result = Idioma.Select("Objeto= 'Mens34PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una cantidad.
                    return;
                }
                List<CsTypDetallePropuesta> ObjDetallePropuesta = new List<CsTypDetallePropuesta>();
                var TypDetallePropuesta = new CsTypDetallePropuesta()
                {
                    IdDetPropuesta = Convert.ToInt32(0),
                    IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.Trim()),
                    PN = VbPN.Trim(),
                    Descripcion = (GrdDet1.FooterRow.FindControl("TxtDescPNPP") as TextBox).Text.Trim(),
                    CantidadSol = Convert.ToDouble(VbCant),
                    PorcentajeUtilidad = Convert.ToDouble(0),
                    ValorUnd = Convert.ToDouble(VlrUnd),
                    CostoVenta = Convert.ToDouble(0),
                    TiempoEntregaDias = Convert.ToInt32(0),
                    TiempoEntregaDiasCoti = Convert.ToInt32(0),
                    IdServicio = Convert.ToInt32(0),
                    SelectComprar = Convert.ToInt32(0),
                    Posicion = Convert.ToInt32(0),
                    Aprobado = (GrdDet1.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Visible == true ? 1 : 0,
                    NomServicio = "",
                    Usu = Session["C77U"].ToString(),
                    IdReporte = Convert.ToInt32(0),
                    EstadoPosicion = "",
                    CantidadEntregada = Convert.ToInt32(0),
                    UnidadMedida = "",
                    CodMoneda = DdlMoned.Text.Trim(),
                    ValorMonedaProp = Convert.ToDouble(0),
                    IVA = Convert.ToDouble(TxtImpuest.Text.Trim()),
                    ValorTotal = Convert.ToDouble(0),
                    ValorConImpuesto = Convert.ToDouble(0),
                    UnidMinCompra = Convert.ToDouble(0),
                    CodEstado = "",
                    ObservacionesDP = "",
                    PnAlterno = "",
                    TipoCotizacion = "",
                    IdDetPropSrv = Convert.ToInt32(0),
                    RepaExterna = Convert.ToInt32(0),
                    CantRealDP = Convert.ToDouble(0),
                    UndCompraDPV = "",
                    IdConfigCia = Convert.ToInt32(Session["!dC!@"].ToString()),
                    CodTipoPT = DdlTipo.Text.Trim(),
                    Accion = "INSERT",
                };
                ObjDetallePropuesta.Add(TypDetallePropuesta);

                CsTypDetallePropuesta ClsTypDetallePropuesta = new CsTypDetallePropuesta();
                ClsTypDetallePropuesta.GananciaAnterior(Convert.ToDouble(TxtGanacNacional.Text), Convert.ToDouble(TxtGanacInter.Text));
                ClsTypDetallePropuesta.Alimentar(ObjDetallePropuesta);
                string Mensj = ClsTypDetallePropuesta.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                Traerdatos(TxtNumPpt.Text.Trim());
            }
            if (e.CommandName.Equals("AddPlantilla"))
            {
                Result = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                foreach (DataRow row in Result)
                { IbtSubirCargaMax.ToolTip = row["Texto"].ToString(); }
                MultVw.ActiveViewIndex = 6;
            }
        }
        protected void GrdDet1_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDet1.EditIndex = e.NewEditIndex; BIndDDet1("SELECT", TxtNumPpt.Text); ActivarBtn(false, false, false); }
        protected void GrdDet1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ViewState["Accion"] = "";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();

            string VbId = GrdDet1.DataKeys[e.RowIndex].Values[0].ToString().Trim();
            string VbIdDetPropSrv = GrdDet1.DataKeys[e.RowIndex].Values[1].ToString().Trim();
            string VbIdSvc = GrdDet1.DataKeys[e.RowIndex].Values[2].ToString().Trim();
            string VbIRpt = GrdDet1.DataKeys[e.RowIndex].Values[3].ToString().Trim();
            string VbAprbd = (GrdDet1.Rows[e.RowIndex].FindControl("CkbAprob") as CheckBox).Checked == true ? "1" : "0";
            string VbCanSol = (GrdDet1.Rows[e.RowIndex].FindControl("TxtCantSol") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtCantSol") as TextBox).Text.Trim();
            string VbPcUt = (GrdDet1.Rows[e.RowIndex].FindControl("TxtPorcUtld") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtPorcUtld") as TextBox).Text.Trim();
            string VbVlrUnd = (GrdDet1.Rows[e.RowIndex].FindControl("TxtVlrUnd") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtVlrUnd") as TextBox).Text.Trim();
            string VbTmpEntD = (GrdDet1.Rows[e.RowIndex].FindControl("TxtTiempEntD") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtTiempEntD") as TextBox).Text.Trim();
            string VbImpt = (GrdDet1.Rows[e.RowIndex].FindControl("TxtTiempEntD") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtTiempEntD") as TextBox).Text.Trim();
            string VbICantR = (GrdDet1.Rows[e.RowIndex].FindControl("TxtCantReal") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDet1.Rows[e.RowIndex].FindControl("TxtCantReal") as TextBox).Text.Trim();

            List<CsTypDetallePropuesta> ObjDetallePropuesta = new List<CsTypDetallePropuesta>();
            var TypDetallePropuesta = new CsTypDetallePropuesta()
            {
                IdDetPropuesta = Convert.ToInt32(VbId),
                IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.Trim()),
                PN = (GrdDet1.Rows[e.RowIndex].FindControl("TxtPN") as TextBox).Text.Trim(),
                Descripcion = "",
                CantidadSol = Convert.ToDouble(VbCanSol),
                PorcentajeUtilidad = Convert.ToDouble(VbPcUt),
                ValorUnd = Convert.ToDouble(VbVlrUnd),
                CostoVenta = Convert.ToDouble((GrdDet1.Rows[e.RowIndex].FindControl("LblCostVnta") as Label).Text.Trim()),
                TiempoEntregaDias = Convert.ToInt32(VbTmpEntD),
                TiempoEntregaDiasCoti = Convert.ToInt32(0),
                IdServicio = Convert.ToInt32(VbIdSvc),
                SelectComprar = Convert.ToInt32(0),
                Posicion = Convert.ToInt32((GrdDet1.Rows[e.RowIndex].FindControl("LblPos") as Label).Text.Trim()),
                Aprobado = Convert.ToInt32(VbAprbd),
                NomServicio = "",
                Usu = Session["C77U"].ToString(),
                IdReporte = Convert.ToInt32(VbIRpt),
                EstadoPosicion = "",
                CantidadEntregada = Convert.ToInt32(0),
                UnidadMedida = "",
                CodMoneda = (GrdDet1.Rows[e.RowIndex].FindControl("LblMnd") as Label).Text.Trim(),
                ValorMonedaProp = Convert.ToDouble((GrdDet1.Rows[e.RowIndex].FindControl("LblVlrMndPpt") as Label).Text.Trim()),
                IVA = Convert.ToDouble(TxtImpuest.Text.Trim()),
                ValorTotal = Convert.ToDouble((GrdDet1.Rows[e.RowIndex].FindControl("LblVlrTtl") as Label).Text.Trim()),
                ValorConImpuesto = Convert.ToDouble((GrdDet1.Rows[e.RowIndex].FindControl("LblVlrConImpt") as Label).Text.Trim()),
                UnidMinCompra = Convert.ToDouble(0),
                CodEstado = "",
                ObservacionesDP = "",
                PnAlterno = "",
                TipoCotizacion = "",
                IdDetPropSrv = Convert.ToInt32(VbIdDetPropSrv),
                RepaExterna = Convert.ToInt32(0),
                CantRealDP = Convert.ToDouble(VbICantR),
                UndCompraDPV = "",
                IdConfigCia = Convert.ToInt32(Session["!dC!@"].ToString()),
                CodTipoPT = DdlTipo.Text.Trim(),
                Accion = "UPDATE",
            };
            ObjDetallePropuesta.Add(TypDetallePropuesta);

            CsTypDetallePropuesta ClsTypDetallePropuesta = new CsTypDetallePropuesta();
            ClsTypDetallePropuesta.GananciaAnterior(Convert.ToDouble(TxtGanacNacional.Text), Convert.ToDouble(TxtGanacInter.Text));
            ClsTypDetallePropuesta.Alimentar(ObjDetallePropuesta);
            string Mensj = ClsTypDetallePropuesta.GetMensj();
            if (!Mensj.Equals(""))
            {
                DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result2)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                return;
            }
            GrdDet1.EditIndex = -1;
            Traerdatos(TxtNumPpt.Text.Trim());
            ActivarBtn(true, true, true);
        }
        protected void GrdDet1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDet1.EditIndex = -1; BIndDDet1("SELECT", TxtNumPpt.Text); ActivarBtn(true, true, true); }
        protected void GrdDet1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            string VbId = GrdDet1.DataKeys[e.RowIndex].Values["IdDetPropuesta"].ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 13,'',@Us,'','','','','','','DELETE',@PP,@Ps,0,0,@ICC,@Id,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Ps", GrdDet1.DataKeys[e.RowIndex].Values[4].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            SC.Parameters.AddWithValue("@Id", VbId);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE Detalle PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDet1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DTPNMat = (DataTable)ViewState["DTPNMat"];
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                ImageButton IbtAddPlntll = (e.Row.FindControl("IbtAddPlntll") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                {
                    IbtAddNew.ToolTip = row["Texto"].ToString().Trim();
                    if (DdlTipo.Text.Trim().Equals("00001"))
                    {
                        if (IbtAddNew != null) { IbtAddNew.Visible = true; }
                    }
                }

                Result = Idioma.Select("Objeto= 'IbtAddPlntll'");
                foreach (DataRow row in Result)
                {
                    IbtAddPlntll.ToolTip = row["Texto"].ToString().Trim();
                    if (DdlTipo.Text.Trim().Equals("00001"))
                    {
                        if (IbtAddPlntll != null) { IbtAddPlntll.Visible = true; }
                    }
                }

                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = DTPNMat;
                DdlPNRFPP.DataTextField = "PN";
                DdlPNRFPP.DataValueField = "CodPN";
                DdlPNRFPP.DataBind();
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

                TextBox TxtVlrUnd = (e.Row.FindControl("TxtVlrUnd") as TextBox);
                if (TxtVlrUnd != null)
                {
                    if (DdlTipo.Text.Trim().Equals("00001")) { TxtVlrUnd.Enabled = true; }
                    else { TxtVlrUnd.Enabled = false; }
                }
                TextBox TxtCantSol = (e.Row.FindControl("TxtCantSol") as TextBox);
                if (TxtCantSol != null)
                {
                    if (DdlTipo.Text.Trim().Equals("00001")) { TxtCantSol.Enabled = true; }
                    else { TxtCantSol.Enabled = false; }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {

                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    CheckBox CkbAprobP = e.Row.FindControl("CkbAprobP") as CheckBox;
                    if (DdlTipo.Text.Trim().Equals("00001")) { }
                    else { }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    if (DdlTipo.Text.Trim().Equals("00001") && (int)ViewState["VblEliMS"] == 1 && TxtFechAprob.Text.Equals(""))
                    {
                        if (imgD != null) { imgD.Visible = true; }
                    }
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        //*************************************** BUSQUEDA ***************************************
        protected void BIndDBusqPPT(string Opc)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Propuesta 32,@Prmtr,'','','',0,@Idm,@CC,@Op,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim().Equals("") ? "0" : TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@Op", Opc);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                        else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        {
            string Filtro = "1";
            if (RdbBusqGnrlPpt.Checked == true) { Filtro = "1"; }
            if (RdbBusqGnrlHk.Checked == true) { Filtro = "2"; }
            if (RdbBusqGnrlSN.Checked == true) { Filtro = "3"; }
            if (RdbBusqGnrlPN.Checked == true) { Filtro = "4"; }
            if (RdbBusqGnrlOT.Checked == true) { Filtro = "5"; }
            if (RdbBusqGnrlRte.Checked == true) { Filtro = "6"; }
            BIndDBusqPPT(Filtro);
        }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string vbcod = GrdBusq.DataKeys[gvr.RowIndex].Values["Codigo"].ToString(); //((Label)row.FindControl("LblPpt")).Text.ToString().Trim();
                string VbCodCli = GrdBusq.DataKeys[gvr.RowIndex].Values["IdTercero"].ToString();
                string VbCodTipo = GrdBusq.DataKeys[gvr.RowIndex].Values["CodTipoPropuesta"].ToString();
                TxtNumPpt.Text = vbcod;
                DdlTipo.Text = VbCodTipo;
                BindDataDdlPpal("SELECT", "0");
                DdlPptSuper.Text = "";
                BindDataDdlPptPpal(VbCodCli, "SELECT");
                ActivarGrd(VbCodTipo);
                Traerdatos(vbcod);
                PerfilesGrid();
                IbtAprDet1All.Visible = false; IbtDesAprDet1All.Visible = false;
                if ((int)ViewState["VblIngMS"] == 1 && TxtFechAprob.Text.Equals("") && VbCodTipo.Trim().Equals("00001")) { IbtAprDet1All.Visible = true; IbtDesAprDet1All.Visible = true; }
                ViewState["IdDetPropHk"] = "0";
                ViewState["IdDetPropSrv"] = "0";
                ViewState["RegistroElemHK"] = "";
                ViewState["FilterPnSugerido"] = "N";
                ViewState["FilterElem"] = "N";
                BindServicios("UPDATE", "0");
                MultVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIr = (e.Row.FindControl("IbtIr") as ImageButton);
                if (IbtIr != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //**************************Condiciones Ppt ***********************************************
        protected void BindDCondiciones()
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = "EXEC SP_PANTALLA_PROPUESTA_V2 17,'','','','','Condiciones',0,0, @idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@idm", Session["77IDM"]);
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtCondTiempEntreg.Text = HttpUtility.HtmlDecode(SDR["CondTiempEntreg"].ToString().Trim());
                    TxtCondFormPago.Text = HttpUtility.HtmlDecode(SDR["CondFormPago"].ToString().Trim());
                    TxtCondDanoOcult.Text = HttpUtility.HtmlDecode(SDR["CondDanoOcult"].ToString().Trim());
                    TxtCondGarant.Text = HttpUtility.HtmlDecode(SDR["CondGarantia"].ToString().Trim());
                }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void IbtClseCondic_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnUpdateCond_Click(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 1,@TE,@FP,@DO,@CG,'','','','','Condicion',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'	";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@TE", TxtCondTiempEntreg.Text.Trim());
                        SC.Parameters.AddWithValue("@FP", TxtCondFormPago.Text.Trim());
                        SC.Parameters.AddWithValue("@DO", TxtCondDanoOcult.Text.Trim());
                        SC.Parameters.AddWithValue("@CG", TxtCondGarant.Text.Trim());
                        SC.ExecuteNonQuery();
                        Transac.Commit();
                        MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim();
                    }
                }
            }
        }
        protected void BtnUpdateCondPpt_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Validar"] = "S";
            if (TxtNumPpt.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens02PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// 
                Page.Title = ViewState["PageTit"].ToString().Trim(); return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 1,@TE,@FP,@DO,@CG,'','','','','PPT',@Pp,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'	";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@TE", TxtCondTiempEntreg.Text.Trim());
                        SC.Parameters.AddWithValue("@FP", TxtCondFormPago.Text.Trim());
                        SC.Parameters.AddWithValue("@DO", TxtCondDanoOcult.Text.Trim());
                        SC.Parameters.AddWithValue("@CG", TxtCondGarant.Text.Trim());
                        SC.Parameters.AddWithValue("@Pp", TxtNumPpt.Text.Trim());
                        SC.ExecuteNonQuery();
                        Transac.Commit();
                        MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim();
                    }
                }
            }
        }
        //************************** P/N no  valorizados ***********************************************
        protected void BIndDPnSinValorizar(string Opc)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC Seguimiento_Propuesta_Valorizacion_VS_Reserva_WEB @PP,@CC,'WEB'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtPnNoValoriz);

                        if (DtPnNoValoriz.Rows.Count > 0)
                        {
                            ViewState["Notificacion"] = "APROBACION2";
                            ViewState["DtPnNoValoriz"] = DtPnNoValoriz;

                            Result = Idioma.Select("Objeto= 'Mens22PPT'");
                            foreach (DataRow row in Result)
                            { LblTexMensjModl.Text = row["Texto"].ToString(); } //
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                        }
                        else { BindDAprobar(); }
                    }
                }
            }
        }
        protected void IbtClosePNoValorizado_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        //************************** Detalle Elementos / trabajos ***********************************************
        protected void IbtClosDetElemHK_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BindElementosRepa(string Accion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC  SP_PANTALLA_PROPUESTA_V2 18,'','','','','SN',@Pp,0,0, @ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Pp", TxtNumPpt.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtElemRepa);
                            ViewState["DtElemRepa"] = DtElemRepa;
                        }
                    }
                }
            }
            DtElemRepa = (DataTable)ViewState["DtElemRepa"];
            if (DtElemRepa.Rows.Count > 0) { GrdElementos.DataSource = DtElemRepa; GrdElementos.DataBind(); }
            else
            {
                DtElemRepa.Rows.Add(DtElemRepa.NewRow());
                GrdElementos.DataSource = DtElemRepa;
                GrdElementos.DataBind();
                GrdElementos.Rows[0].Cells.Clear();
                GrdElementos.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdElementos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdElementos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdElementos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.CommandName.Equals("Filter"))// filtra solo los servicios del PN
            {
                GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind();
                GrdMO.DataSource = null; GrdMO.DataBind();

                GridViewRow RowG = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton IbtFilter = ((ImageButton)RowG.FindControl("IbtFilter")) as ImageButton;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbIdx = GrdElementos.DataKeys[gvr.RowIndex].Values["IdDetPropHk"].ToString().Trim();
                string VlPn = ((Label)RowG.FindControl("LblPn")).Text.ToString().Trim();

                if (ViewState["IdDetPropHk"].ToString().Equals(VbIdx))
                {
                    if (ViewState["FilterElem"].ToString().Equals("N"))
                    {
                        IbtFilter.ImageUrl = "~/images/FilterOut.png";
                        ViewState["FilterElem"] = "S";
                        ViewState["Notificacion"] = "ALLSERVICIOS";
                        Result = Idioma.Select("Objeto= 'Mens40PPT'");
                        foreach (DataRow row in Result)
                        { LblTexMensjModl.Text = row["Texto"].ToString(); } // Desea editar la aprobación de todos los servicios filtrados?
                        BindServicios("", ViewState["IdDetPropHk"].ToString());
                    }
                    else
                    {
                        IbtFilter.ImageUrl = "~/images/FilterIn.png";
                        ViewState["FilterElem"] = "N";
                        ViewState["IdDetPropHk"] = "0";
                        ViewState["RegistroElemHK"] = "";
                        BindServicios("", "0");
                    }
                }
                else
                {
                    foreach (GridViewRow Row in GrdElementos.Rows)
                    {
                        ImageButton ibtFltr = Row.FindControl("IbtFilter") as ImageButton;
                        if (ibtFltr != null) { ibtFltr.ImageUrl = "~/images/FilterIn.png"; }
                        ViewState["FilterElem"] = "S";
                        ViewState["Notificacion"] = "ALLSERVICIOS";
                        Result = Idioma.Select("Objeto= 'Mens40PPT'");
                        foreach (DataRow row in Result)
                        { LblTexMensjModl.Text = row["Texto"].ToString(); } // Desea editar la aprobación de todos los servicios filtrad
                        ViewState["IdDetPropHk"] = VbIdx;
                        ViewState["RegistroElemHK"] = VlPn;
                        IbtFilter.ImageUrl = "~/images/FilterOut.png";
                        BindServicios("", ViewState["IdDetPropHk"].ToString());
                    }
                }
            }
            if (e.CommandName.Equals("FltrPN"))// buscar el PN que se va a agregar
            {
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqPN", "$('#ModalBusqPN').modal();", true);
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
            }
            if (e.CommandName.Equals("AddNew"))
            {
                string VbCant = (GrdElementos.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim();
                VbCant = VbCant.Equals("") ? "0" : VbCant;
                if (Convert.ToInt32(VbCant) < 0)
                {
                    Result = Idioma.Select("Objeto= 'Mens34PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una cantidad.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 7,@Bod,@Us,@Pn,@Desc,@Sn,'','','INSERT','ELEM',@PP,@Cant,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Bod", ViewState["AeroVirtual"]);
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Pn", (GrdElementos.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@Desc", (GrdElementos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@Sn", (GrdElementos.FooterRow.FindControl("TxtSNPP") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                                SC.Parameters.AddWithValue("@Cant", VbCant);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindElementosRepa("UPDATE");
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                }
            }
        }
        protected void GrdElementos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string VbId = GrdElementos.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 7,'',@Us,'','','','','','DELETE','ELEM',@PP,0,@Id,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Id", VbId);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindElementosRepa("UPDATE");
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void GrdElementos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);

                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }

                ImageButton IbtPN = (e.Row.FindControl("IbtPN") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtPNTT'");
                foreach (DataRow row in Result)
                { IbtPN.ToolTip = row["Texto"].ToString().Trim(); }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton IbtFilter = (e.Row.FindControl("IbtFilter") as ImageButton);
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;

                if (IbtFilter != null)
                {
                    Result = Idioma.Select("Objeto='FiltroMst'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtFilter.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
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
        protected void BindAeronaveRepa(string Accion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC  SP_PANTALLA_PROPUESTA_V2 18,'','','','','HK',@Pp,0,0, @ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Pp", TxtNumPpt.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtHKRepa);
                            ViewState["DtHKRepa"] = DtHKRepa;
                        }
                    }
                }
            }
            DtHKRepa = (DataTable)ViewState["DtHKRepa"];
            if (DtHKRepa.Rows.Count > 0) { GrdAeronave.DataSource = DtHKRepa; GrdAeronave.DataBind(); }
            else
            {
                DtHKRepa.Rows.Add(DtHKRepa.NewRow());
                GrdAeronave.DataSource = DtHKRepa;
                GrdAeronave.DataBind();
                GrdAeronave.Rows[0].Cells.Clear();
                GrdAeronave.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdAeronave.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdAeronave.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdAeronave_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.CommandName.Equals("Filter"))// filtra solo los servicios del PN
            {
                GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind();
                GrdMO.DataSource = null; GrdMO.DataBind();

                GridViewRow RowG = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton IbtFilter = ((ImageButton)RowG.FindControl("IbtFilter3")) as ImageButton;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbIdx = GrdAeronave.DataKeys[gvr.RowIndex].Values["IdDetPropHk"].ToString().Trim();
                string VlHk = GrdAeronave.DataKeys[gvr.RowIndex].Values["CodAeronave"].ToString().Trim();

                if (ViewState["IdDetPropHk"].ToString().Equals(VbIdx))
                {
                    if (ViewState["FilterElem"].ToString().Equals("N"))
                    {
                        IbtFilter.ImageUrl = "~/images/FilterOut.png";
                        ViewState["FilterElem"] = "S";
                        ViewState["Notificacion"] = "ALLSERVICIOS";
                        Result = Idioma.Select("Objeto= 'Mens40PPT'");
                        foreach (DataRow row in Result)
                        { LblTexMensjModl.Text = row["Texto"].ToString(); } // Desea editar la aprobación de todos los servicios filtrados?
                        BindServicios("", ViewState["IdDetPropHk"].ToString());
                    }
                    else
                    {
                        IbtFilter.ImageUrl = "~/images/FilterIn.png";
                        ViewState["FilterElem"] = "N";
                        ViewState["IdDetPropHk"] = "0";
                        ViewState["RegistroElemHK"] = "";
                        BindServicios("", "0");
                    }
                }
                else
                {
                    foreach (GridViewRow Row in GrdAeronave.Rows)
                    {
                        ImageButton ibtFltr = Row.FindControl("IbtFilter3") as ImageButton;
                        if (ibtFltr != null) { ibtFltr.ImageUrl = "~/images/FilterIn.png"; }
                        ViewState["FilterElem"] = "S";
                        ViewState["Notificacion"] = "ALLSERVICIOS";
                        Result = Idioma.Select("Objeto= 'Mens40PPT'");
                        foreach (DataRow row in Result)
                        { LblTexMensjModl.Text = row["Texto"].ToString(); } // Desea editar la aprobación de todos los servicios filtrad
                        ViewState["IdDetPropHk"] = VbIdx;
                        ViewState["RegistroElemHK"] = VlHk;
                        IbtFilter.ImageUrl = "~/images/FilterOut.png";
                        BindServicios("", ViewState["IdDetPropHk"].ToString());
                    }
                }
            }
            if (e.CommandName.Equals("AddNew"))
            {
                string VbHK = (GrdAeronave.FooterRow.FindControl("DdlAeronavePP") as DropDownList).Text.Trim();

                if (VbHK.Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'Mens57PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar una aeronave.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 7,'',@Us,'','','','','','INSERT','HK',@PP,@Cant,@HK,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                                SC.Parameters.AddWithValue("@Cant", 1);
                                SC.Parameters.AddWithValue("@HK", VbHK);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindAeronaveRepa("UPDATE");
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                }
            }
        }
        protected void GrdAeronave_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string VbId = GrdAeronave.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 7,'',@Us,'','','','','','DELETE','HK',@PP,0,@Id,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Id", VbId);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindAeronaveRepa("UPDATE");
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void GrdAeronave_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            DSDdl = (DataSet)ViewState["DSDdl"];
            PerfilesGrid();

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);

                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }

                DropDownList DdlAeronavePP = (e.Row.FindControl("DdlAeronavePP") as DropDownList);
                if (DdlPptSuper.Text.Trim().Equals("")) { DdlAeronavePP.DataSource = DSDdl.Tables["HKSinPPT"]; }
                else
                {
                    DataTable DT = new DataTable();
                    DT = DSDdl.Tables["HKConSubPT"].Clone();
                    Result = DSDdl.Tables["HKConSubPT"].Select("IdPropuesta=" + DdlPptSuper.Text.Trim() + "");
                    foreach (DataRow Row in Result)
                    { DT.ImportRow(Row); }

                    DdlAeronavePP.DataSource = DT;
                }
                DdlAeronavePP.DataTextField = "Matricula";
                DdlAeronavePP.DataValueField = "CodAeronave";
                DdlAeronavePP.DataBind();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton IbtFilter = (e.Row.FindControl("IbtFilter3") as ImageButton);
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;

                if (IbtFilter != null)
                {
                    Result = Idioma.Select("Objeto='FiltroMst'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtFilter.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
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
        //************************** MODAL buscar PN para asignar en la PPT ***********************************************
        protected void BindModalBusqPN()
        {
            string VbPn = RdbMOdalBusqPN.Checked == true ? TxtModalBusq.Text.Trim() : "";
            string VbSn = RdbMOdalBusqSN.Checked == true ? TxtModalBusq.Text.Trim() : "";
            string VbDes = RdbMOdalBusqDesc.Checked == true ? TxtModalBusq.Text.Trim() : "";
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Propuesta 46,@P,'',@S,@D,@PP,0,0,@CC,'01-1-2009','01-01-1900','01-01-1900'";

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@P", VbPn);
                    SC.Parameters.AddWithValue("@S", VbSn);
                    SC.Parameters.AddWithValue("@D", VbDes);
                    SC.Parameters.AddWithValue("@PP", DdlPptSuper.Text.Trim().Equals("") ? "0" : DdlPptSuper.Text.Trim());
                    SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdModalBusqPN.DataSource = DtB; GrdModalBusqPN.DataBind(); }
                        else { GrdModalBusqPN.DataSource = null; GrdModalBusqPN.DataBind(); }

                    }
                }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            BindModalBusqPN();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqPN", "$('#ModalBusqPN').modal();", true);
        }
        protected void GrdModalBusqPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (e.CommandName.Equals("IrPN"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string VbPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                string VbSn = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                string VbDesc = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                string VbCant = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();

                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbCntd = GrdModalBusqPN.DataKeys[gvr.RowIndex].Values["Cantidad"].ToString();
                string VbIdentifi = GrdModalBusqPN.DataKeys[gvr.RowIndex].Values["IdentificadorElemR"].ToString();
                ViewState["AeroVirtual"] = GrdModalBusqPN.DataKeys[gvr.RowIndex].Values["Bodega"].ToString();

                (GrdElementos.FooterRow.FindControl("TxtPNPP") as TextBox).Text = VbPn;
                (GrdElementos.FooterRow.FindControl("TxtSNPP") as TextBox).Text = VbSn;
                (GrdElementos.FooterRow.FindControl("TxtDescPP") as TextBox).Text = VbDesc;
                (GrdElementos.FooterRow.FindControl("TxtCantPP") as TextBox).Text = VbCntd;
                if (VbIdentifi.Trim().Equals("SN")) { (GrdElementos.FooterRow.FindControl("TxtCantPP") as TextBox).Enabled = false; }
                else { (GrdElementos.FooterRow.FindControl("TxtCantPP") as TextBox).Enabled = true; }
            }
        }
        protected void GrdModalBusqPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;

                ImageButton IbtIrPN = (e.Row.FindControl("IbtIrPN") as ImageButton);

                if (IbtIrPN != null)
                {
                    Result = Idioma.Select("Objeto='IbtIrMstr'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtIrPN.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //************************** Detalle servicios ***********************************************
        protected void BindServicios(string Accion, string Id)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_PROPUESTA_V2 19,@DO,'','','','',@Pp,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Pp", TxtNumPpt.Text.Trim());
                        SC.Parameters.AddWithValue("@DO", RdbSinDanOcul.Checked == true ? "SIN_DAÑO" : "CON_DAÑO");
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        { DAB.SelectCommand = SC; DAB.Fill(DtSrvcs); ViewState["DtSrvcs"] = DtSrvcs; }
                    }
                }
            }
            ViewState["ChkAll"] = "S";
            DtSrvcs = (DataTable)ViewState["DtSrvcs"];
            DataTable Dt = new DataTable();
            Dt = DtSrvcs.Clone();
            DataRow[] Result;
            if (Id.Equals("0"))
            {
                ViewState["ChkAll"] = "N";
                Result = DtSrvcs.Select("IdPropuesta='" + TxtNumPpt.Text.Trim() + "'");
                foreach (DataRow Row in Result)
                { Dt.ImportRow(Row); }
            }
            else
            {
                Result = DtSrvcs.Select("IdDetPropHk='" + Id + "'"); foreach (DataRow Row in Result)
                {
                    Dt.ImportRow(Row);
                    if (Row["AprobadoDPSM"].ToString().Equals("0")) { ViewState["ChkAll"] = "N"; }// no marca el check de all en true cuando si al menos hay uno en cero
                }
            }
            if (Dt.Rows.Count > 0) { GrdServicios.DataSource = Dt; GrdServicios.DataBind(); }
            else
            {
                Dt.Rows.Add(Dt.NewRow());
                GrdServicios.DataSource = Dt;
                GrdServicios.DataBind();
                GrdServicios.Rows[0].Cells.Clear();
                GrdServicios.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdServicios.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdServicios.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbMensje = DdlTipo.Text.Trim().Equals("00003") ? "Mens37PPT" : "Mens60PPT";

            DataRow[] Result;

            if (e.CommandName.Equals("AddNew"))// ingreso por registro
            {
                if (ViewState["IdDetPropHk"].ToString().Trim().Equals("0"))
                {
                    Result = Idioma.Select("Objeto= '" + VbMensje + "'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe filtrar en el panel.
                    return;
                }

                string VbDescSvc = (GrdServicios.FooterRow.FindControl("TxtDesSvcPP") as TextBox).Text.Trim();
                if (VbDescSvc.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'Mens38PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la descripción del servicio.
                    return;
                }
                List<CsTypDetallePropuestaSvcManto> ObjDetallePropuestaSvcManto = new List<CsTypDetallePropuestaSvcManto>();
                var TypDetallePropuestaSvcManto = new CsTypDetallePropuestaSvcManto()
                {
                    IdDetPropSrv = 0,
                    IdDetPropHk = Convert.ToInt32(ViewState["IdDetPropHk"].ToString().Trim()),
                    IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.ToString().Trim()),
                    AprobadoDPSM = 0, //(GrdServicios.FooterRow.FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0,
                    IdSvcManto = 0,
                    IdReporte = 0,
                    CodOTPrta = 0,
                    Matricula = DdlTipo.Text.Trim().Equals("00003") ? "" : "HK",
                    CodModeloDPSM = "",
                    DescricionServicio = VbDescSvc,
                    Usu = Session["C77U"].ToString(),
                    PN = "",
                    CodReferencia = "",
                    DescripcionPN = "",
                    CodContadorDPSM = "",
                    ReparacionExterna = (GrdServicios.FooterRow.FindControl("CkbRExtPP") as CheckBox).Checked == false ? 0 : 1,
                    Accion = "INSERT",
                };
                ObjDetallePropuestaSvcManto.Add(TypDetallePropuestaSvcManto);
                CsTypDetallePropuestaSvcManto ClsTypDetallePropuestaSvcManto = new CsTypDetallePropuestaSvcManto();
                ClsTypDetallePropuestaSvcManto.Alimentar(ObjDetallePropuestaSvcManto);
                string Mensj = ClsTypDetallePropuestaSvcManto.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
            }
            if (e.CommandName.Equals("FltrSvcMas"))// ingreso Masivo
            {
                if (ViewState["IdDetPropHk"].ToString().Trim().Equals("0"))
                {
                    Result = Idioma.Select("Objeto= '" + VbMensje + "'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe filtrar en el panel.
                    return;
                }
                BindAsigSvcsMasivo();
            }
            if (e.CommandName.Equals("GenOT"))// Generar OT
            {
                string VbOT = "0", VbApliPlano = "N", Mensj = "";
                GridViewRow RowG = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbIdx = GrdServicios.DataKeys[gvr.RowIndex].Values["IdDetPropSrv"].ToString().Trim();
                string VbCodSvcM = GrdServicios.DataKeys[gvr.RowIndex].Values["CodServicioManto"].ToString().Trim();
                string VlRepExt = ((CheckBox)RowG.FindControl("CkbRExtP")).Checked == true ? "1" : "0";
                string VlOt = GrdServicios.DataKeys[gvr.RowIndex].Values["Ot"].ToString().Trim(); //((Label)RowG.FindControl("LblOTP")).Text.ToString().Trim();
                if (!TxtFechAprob.Text.Trim().Equals(""))
                {
                    ((CheckBox)RowG.FindControl("CkbRExtP")).Enabled = false;
                }
                if (CkbAplicOT.Checked == false && TxtFechAprob.Text.Trim().Equals(""))// si no es evaluacion desde ot y no esta aprobada
                {
                    Result = Idioma.Select("Objeto= 'Mens44PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La propuesta debe estar aprobada cuando la evaluación viene de un servicio.
                    ViewState["Validar"] = "N"; return;
                }
                if (!VlOt.Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'Mens47PPT'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// El servicio ya se encuentra con una O.T. asignada.
                    ViewState["Validar"] = "N"; return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC GenerarOT_PPT @Tipo, @PPT, @IdDetPropSrv, @EvalDesdeOT, @RepaExt, @OT, @CodSvcManto, @Usu, @ICC";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Tipo", DdlTipo.Text.Trim());
                                SC.Parameters.AddWithValue("@PPT", TxtNumPpt.Text.Trim());
                                SC.Parameters.AddWithValue("@IdDetPropSrv", VbIdx);
                                SC.Parameters.AddWithValue("@EvalDesdeOT", CkbAplicOT.Checked == true ? 1 : 0);
                                SC.Parameters.AddWithValue("@RepaExt", VlRepExt);
                                SC.Parameters.AddWithValue("@OT", VlOt.Trim());
                                SC.Parameters.AddWithValue("@CodSvcManto", VbCodSvcM.Trim());
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                // SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);

                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    VbOT = HttpUtility.HtmlDecode(SDR["OT"].ToString().Trim());
                                    VbApliPlano = HttpUtility.HtmlDecode(SDR["AplicaPlano"].ToString().Trim());
                                }
                                SDR.Close();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    if (Mensj.ToString().Trim().Equals("Mens47PPT"))// si el mensaje es  debe actualizar porque se asigna la ot que tiene el servicio
                                    { BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString()); }
                                    return;
                                }
                                Transac.Commit();
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                    sqlCon.Close();
                }
                if (VbApliPlano.Equals("S"))
                {
                    string Mensj2 = "";
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string VBQuery = "EXEC SP_TablasMRO 6,'',@Us,'','','','','','','GENERAROT',@OT,0,0,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@OT", VbOT);
                            SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);

                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read()) { Mensj2 = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                            SDR.Close();
                            if (!Mensj2.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj2.ToString().Trim() + "'");
                                if (Result.Length != 0)
                                {
                                    foreach (DataRow row in Result) { Mensj2 = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj2 + "');", true);
                                }
                            }
                        }
                    }
                }
                BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
            }
            if (e.CommandName.Equals("Filter"))// Filtra los Pn Sugeridos
            { //@CRT1=TipoPPT | @CRT2=PN | @NRC1= Evalu a partir OT | @NRC2 = DanoOc | @NRC3 = PPT,| @NRC4 = IdSvcManto | @NRC5 = IdDetPropSrv
                if (!ViewState["Accion"].ToString().Trim().Equals("")) { return; }
                GridViewRow RowG = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton IbtFilter2 = ((ImageButton)RowG.FindControl("IbtFilter2")) as ImageButton;

                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string LblRteP = GrdServicios.DataKeys[gvr.RowIndex].Values["IdReporte"].ToString().Trim();
                string VbIdx = GrdServicios.DataKeys[gvr.RowIndex].Values["IdDetPropSrv"].ToString().Trim();
                string VbIdSvc = GrdServicios.DataKeys[gvr.RowIndex].Values["IdSvcManto"].ToString().Trim();
                string VlPn = GrdServicios.DataKeys[gvr.RowIndex].Values["Pn"].ToString().Trim();

                if (ViewState["IdDetPropSrv"].ToString().Equals(VbIdx))
                {
                    if (ViewState["FilterPnSugerido"].ToString().Equals("N"))
                    {
                        IbtFilter2.ImageUrl = "~/images/FilterOut.png";
                        ViewState["FilterPnSugerido"] = "S";
                        BindPnSugeridos(VlPn, VbIdSvc, VbIdx, LblRteP);
                    }
                    else
                    {
                        IbtFilter2.ImageUrl = "~/images/FilterIn.png";
                        ViewState["FilterPnSugerido"] = "N";
                        ViewState["IdDetPropHk"] = "0";
                        GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind();
                        GrdMO.DataSource = null; GrdMO.DataBind();
                    }
                }
                else
                {

                    foreach (GridViewRow Row in GrdServicios.Rows)
                    {
                        ImageButton ibtFltr = Row.FindControl("IbtFilter2") as ImageButton;
                        if (ibtFltr != null) { ibtFltr.ImageUrl = "~/images/FilterIn.png"; }
                        ViewState["FilterPnSugerido"] = "S";
                        ViewState["IdDetPropSrv"] = VbIdx;
                        IbtFilter2.ImageUrl = "~/images/FilterOut.png";
                    }
                    BindPnSugeridos(VlPn, VbIdSvc, VbIdx, LblRteP);
                }
            }
        }
        protected void GrdServicios_RowEditing(object sender, GridViewEditEventArgs e)
        { ViewState["Accion"] = "Editar"; GrdServicios.EditIndex = e.NewEditIndex; BindServicios("", ViewState["IdDetPropHk"].ToString()); }
        protected void GrdServicios_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ViewState["Accion"] = "";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string OT = GrdServicios.DataKeys[e.RowIndex].Values["Ot"].ToString(); //(GrdServicios.Rows[e.RowIndex].FindControl("LblOT") as Label).Text.Trim();
            int VbRpExt = (GrdServicios.Rows[e.RowIndex].FindControl("CkbRExt") as CheckBox).Checked == true ? 1 : 0;

            if (!OT.Equals("0") && VbRpExt > 0)
            {
                Result = Idioma.Select("Objeto= 'Mens41PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//No es posible marcar como reparación externa un servicio con Orden de Trabajo.
                return;
            }

            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 9,@TP,@Us,'','','','',@RpExt,@ICC,'',@PP,@Apr,@IdPHK,@DO,@Imp,@Id,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            // string borr = ViewState["IdDetPropHk"].ToString();
                            //int borr1 = (GrdServicios.Rows[e.RowIndex].FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0;

                            SC.Parameters.AddWithValue("@TP", DdlTipo.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@RpExt", VbRpExt);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Apr", (GrdServicios.Rows[e.RowIndex].FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0);
                            SC.Parameters.AddWithValue("@IdPHK", ViewState["IdDetPropHk"]);
                            SC.Parameters.AddWithValue("@DO", RdbSinDanOcul.Checked == true ? 1 : 2);
                            SC.Parameters.AddWithValue("@Imp", CkbAplicImpuesto.Checked == true ? 1 : 0);
                            SC.Parameters.AddWithValue("@Id", GrdServicios.DataKeys[e.RowIndex].Value.ToString());
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                            GrdServicios.EditIndex = -1;
                            BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void GrdServicios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { ViewState["Accion"] = ""; GrdServicios.EditIndex = -1; BindServicios("", ViewState["IdDetPropHk"].ToString()); }
        protected void GrdServicios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string OT = GrdServicios.DataKeys[e.RowIndex].Values["Ot"].ToString();

            if (ViewState["Valorizada"].ToString().Trim().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens15PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra valorizada.
                return;
            }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 8,'',@Us,'','','','','',@ICC,'',@PP,0,0,0,0,@Id,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Id", GrdServicios.DataKeys[e.RowIndex].Value.ToString());
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                            BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Eliminar Servicios de PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();

            if (e.Row.RowType == DataControlRowType.Header)
            {

                CheckBox ChkAll = (e.Row.FindControl("ChkAll") as CheckBox);
                ChkAll.Checked = false;
                if ((int)ViewState["VblModMS"] == 1)
                {
                    if (!TxtFechAprob.Text.Trim().Equals("") || DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
                    { ChkAll.Enabled = false; }
                    if (!ViewState["IdDetPropHk"].ToString().Trim().Equals("0"))
                    {
                        ChkAll.Visible = true;
                        if (ViewState["ChkAll"].ToString().Equals("S")) { ChkAll.Checked = true; }
                    }
                    else { ChkAll.Visible = false; }
                }
                else { ChkAll.Visible = false; }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                if (RdbSinDanOcul.Checked == true)
                {
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                else
                {
                    IbtAddNew.Enabled = false;
                    Result = Idioma.Select("Objeto= 'IbtAddNewSvcF'");//No aplica a daño oculto.
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                ImageButton IbtAddMas = (e.Row.FindControl("IbtAddMas") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtAddMasTT'");
                foreach (DataRow row in Result)
                { IbtAddMas.ToolTip = row["Texto"].ToString().Trim(); }

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
                ImageButton IbtGenOT = (e.Row.FindControl("IbtGenOT") as ImageButton);
                ImageButton IbtFilter2 = (e.Row.FindControl("IbtFilter2") as ImageButton);
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                CheckBox CkbRExtP = (e.Row.FindControl("CkbRExtP") as CheckBox);
                if (IbtGenOT != null)
                {
                    Result = Idioma.Select("Objeto= 'IbtGenOTOnCl'");
                    foreach (DataRow row in Result)
                    { IbtGenOT.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    if (CkbRExtP != null)
                    {
                        if (RdbSinDanOcul.Checked == true && CkbRExtP.Checked == false)// no es daño oculto ni es reparacion externa
                        {
                            IbtGenOT.Enabled = true;
                            Result = Idioma.Select("Objeto='IbtGenOTTT'");
                            foreach (DataRow RowIdioma in Result)
                            { IbtGenOT.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        }
                        else
                        {
                            IbtGenOT.Enabled = false;
                            Result = Idioma.Select("Objeto='IbtGenOTTF'");
                            foreach (DataRow RowIdioma in Result)// No aplica por daño oculto o reparación externa.
                            { IbtGenOT.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        }
                    }
                    if ((int)ViewState["VblCE2"] == 0)// Generar ot
                    { IbtGenOT.Visible = false; }
                }
                if (IbtFilter2 != null)
                {
                    Result = Idioma.Select("Objeto='FiltroMst'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtFilter2.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    if (CkbRExtP != null)
                    {
                        IbtFilter2.Visible = CkbRExtP.Checked == true ? false : true;
                    }
                }
                if (imgE != null)
                {
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
                DataRowView DRV = e.Row.DataItem as DataRowView;
                if (DRV["IdSvcManto"].ToString().Equals("0")) { e.Row.BackColor = System.Drawing.Color.DarkOrange; }
            }

        }
        protected void AprobAllSvcs()// aprobar todos los servicios
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 9,@TP,@Us,'','','','','',@ICC,'',@PP,@Apr,@IdPHK,@DO,@Imp,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@TP", DdlTipo.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Apr", ViewState["VlrAproAllSvc"]);
                            SC.Parameters.AddWithValue("@IdPHK", ViewState["IdDetPropHk"]);
                            SC.Parameters.AddWithValue("@DO", RdbSinDanOcul.Checked == true ? 1 : 2);
                            SC.Parameters.AddWithValue("@Imp", CkbAplicImpuesto.Checked == true ? 1 : 0);
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtNumPpt.Text.Trim());
                            BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void ChkAll_CheckedChanged1(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            CheckBox ChkBoxHeader = (CheckBox)GrdServicios.HeaderRow.FindControl("ChkAll");
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                ViewState["VlrAproAllSvc"] = ChkBoxHeader.Checked == true ? "0" : "1";
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                ViewState["VlrAproAllSvc"] = ChkBoxHeader.Checked == true ? "0" : "1";
                return;
            }
            ViewState["VlrAproAllSvc"] = ChkBoxHeader.Checked == true ? "1" : "0";

            if (!ViewState["IdDetPropHk"].ToString().Trim().Equals("0"))
            {
                ViewState["Notificacion"] = "ALLSERVICIOS";
                Result = Idioma.Select("Objeto= 'Mens40PPT'");
                foreach (DataRow row in Result)
                { LblTexMensjModl.Text = row["Texto"].ToString(); } // Desea editar la aprobación de todos los servicios filtrados?
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                return;
            }
            else { }
        }
        //************************** Servicios Masivos ***********************************************
        protected void BindAsigSvcsMasivo()
        {
            DataTable DT = new DataTable();
            string VbTxtSql = "";
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                if (RdbSinDanOcul.Checked == true) { VbTxtSql = "EXEC  SP_PANTALLA_PROPUESTA_V2 12,@P,@TiP,'','','WEB',0,@IdD,0,@ICC,'01-01-01','02-01-01','03-01-01'"; }
                else { VbTxtSql = "EXEC SP_PANTALLA_Propuesta 13,'','','','',@SubP,0,0,0,'01-1-2009','01-01-1900','01-01-1900'"; }

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@P", ViewState["RegistroElemHK"]);
                    SC.Parameters.AddWithValue("@TiP", DdlTipo.Text.Trim());
                    SC.Parameters.AddWithValue("@IdD", ViewState["IdDetPropHk"]);
                    SC.Parameters.AddWithValue("@SubP", DdlPptSuper.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DT);

                        if (DT.Rows.Count > 0) { GrdSvcsMasivo.DataSource = DT; GrdSvcsMasivo.DataBind(); }
                        else { GrdSvcsMasivo.DataSource = null; GrdSvcsMasivo.DataBind(); }
                    }
                    MultVw.ActiveViewIndex = 5;
                }
            }
        }
        protected void IbtClosAsigSvcMasivo_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 4; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnAsigSvcMasivo_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            List<CsTypDetallePropuestaSvcManto> ObjDetallePropuestaSvcManto = new List<CsTypDetallePropuestaSvcManto>();
            foreach (GridViewRow Row in GrdSvcsMasivo.Rows)
            {
                Label LblDescSvc = Row.FindControl("LblDescSvc") as Label;
                string VbIdSvc = GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[0].ToString().Trim(); // obtener indice
                CheckBox CkbCk = Row.FindControl("CkbCk") as CheckBox;
                if (CkbCk.Checked == true)
                {
                    int VbCodOt = 0;
                    if (RdbSinDanOcul.Checked == true) { VbCodOt = Convert.ToInt32(GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[1].ToString().Trim()); }
                    else { VbCodOt = Convert.ToInt32(GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[7].ToString().Trim()); }


                    var TypDetallePropuestaSvcManto = new CsTypDetallePropuestaSvcManto()
                    {
                        IdDetPropSrv = 0,
                        IdDetPropHk = Convert.ToInt32(ViewState["IdDetPropHk"].ToString().Trim()),
                        IdPropuesta = Convert.ToInt32(TxtNumPpt.Text.ToString().Trim()),
                        AprobadoDPSM = 0, //(GrdServicios.FooterRow.FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0,
                        IdSvcManto = Convert.ToInt32(VbIdSvc),
                        IdReporte = Convert.ToInt32(GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        CodOTPrta = VbCodOt,
                        Matricula = GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[3].ToString().Trim(),
                        CodModeloDPSM = GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[4].ToString().Trim(),
                        DescricionServicio = LblDescSvc.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        PN = ViewState["RegistroElemHK"].ToString().Trim(),
                        CodReferencia = GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[5].ToString().Trim(),
                        DescripcionPN = GrdSvcsMasivo.DataKeys[Row.RowIndex].Values[6].ToString().Trim(),
                        CodContadorDPSM = "",
                        ReparacionExterna = 0,
                        Accion = "INSERT",
                    };
                    ObjDetallePropuestaSvcManto.Add(TypDetallePropuestaSvcManto);
                }
            }
            CsTypDetallePropuestaSvcManto ClsTypDetallePropuestaSvcManto = new CsTypDetallePropuestaSvcManto();
            ClsTypDetallePropuestaSvcManto.Alimentar(ObjDetallePropuestaSvcManto);
            string Mensj = ClsTypDetallePropuestaSvcManto.GetMensj();
            if (!Mensj.Equals(""))
            {
                DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result2)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                return;
            }
            BindServicios("UPDATE", ViewState["IdDetPropHk"].ToString());
            MultVw.ActiveViewIndex = 4;
        }
        //************************** Detalles de los PN sugeridos  y valor de mano de  obra configurado ***********************************************
        protected void BindPnSugeridos(string Pn, string IdSvc, string IdDetPropSrv, string IdRte)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_TablasMRO 11,@TP,@P,'','','','','',@ICC,'',@EvOt,@DO,@PP,@ISvc,@Id,@IdRte,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@TP", DdlTipo.Text.Trim());
                    SC.Parameters.AddWithValue("@P", Pn.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@EvOt", CkbAplicOT.Checked == true ? 1 : 0);
                    SC.Parameters.AddWithValue("@DO", RdbSinDanOcul.Checked == true ? 1 : 2);
                    SC.Parameters.AddWithValue("@PP", TxtNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@ISvc", IdSvc.Trim());
                    SC.Parameters.AddWithValue("@Id", IdDetPropSrv.Trim());
                    SC.Parameters.AddWithValue("@IdRte", IdRte.Trim());

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        using (DataSet DS = new DataSet())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DS);

                            if (DS.Tables[0].Rows.Count > 0) { GrdPnSugerd.DataSource = DS.Tables[0]; GrdPnSugerd.DataBind(); }
                            else { GrdPnSugerd.DataSource = null; GrdPnSugerd.DataBind(); }

                            if (DS.Tables[1].Rows.Count > 0) { GrdMO.DataSource = DS.Tables[1]; GrdMO.DataBind(); }
                            else { GrdMO.DataSource = null; GrdMO.DataBind(); }
                        }
                    }
                }
            }
        }
        //************************** Carga a partir de una Plantilla ***********************************************
        protected void IbtCerrarSubMaxivo_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim();
            GrdCargaMax.DataSource = null; GrdCargaMax.DataBind();
            GrdPnNew.DataSource = null; GrdPnNew.DataBind();
            GrdInconsist.DataSource = null; GrdInconsist.DataBind();
            IbtGuardarCargaMax.Visible = false;
        }
        protected void IbtSubirCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            DataRow[] Result;
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                Idioma = (DataTable)ViewState["TablaIdioma"];

                if (TxtNumPpt.Text.Trim().Equals("")) { return; }
                if (FileUpPPT.Visible == false) { FileUpPPT.Visible = true; }
                else
                {
                    if (FileUpPPT.HasFile == true)
                    {
                        string FolderPath;
                        string FileName = Path.GetFileName(FileUpPPT.PostedFile.FileName);
                        string VblExt = Path.GetExtension(FileUpPPT.PostedFile.FileName);
                        if (Cnx.GetProduccion().Trim().Equals("Y")) { FolderPath = ConfigurationManager.AppSettings["FolderPath"]; }//Azure
                        else { FolderPath = ConfigurationManager.AppSettings["FoldPathLcl"]; }

                        VblExt = VblExt.Substring(VblExt.LastIndexOf(".") + 1).ToLower();
                        string[] formatos = new string[] { "xls", "xlsx" };
                        if (Array.IndexOf(formatos, VblExt) < 0)
                        {
                            Result = Idioma.Select("Objeto= 'RteMens40'");//Archivo inválido
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            return;
                        }
                        string FilePath = FolderPath + FileName;
                        FileUpPPT.SaveAs(FilePath);
                        Import(FilePath, VblExt);
                        FileUpPPT.Visible = false;
                    }
                    else
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens34'");//Debe seleccionar un archivo.
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {

                Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Subir Plantilla de PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void Import(string FilePath, string Extension)
        {
            DataRow[] Result;
            try
            {
                FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader ExcelReader;

                ExcelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration// para que tome la primera fila como titulo de campos
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    { UseHeaderRow = true }
                };
                var dataSet = ExcelReader.AsDataSet(conf);
                DataTable TablaPlantilla = dataSet.Tables[0];

                if (TablaPlantilla.Rows.Count > 0) { ViewState["TablaPlantilla"] = TablaPlantilla; }
                else { return; }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VBQuery = "SubirPlantPPTVenta";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        string PMensj = "";
                        Result = Idioma.Select("Objeto= 'LblTitIncosistnc'");
                        foreach (DataRow row in Result)
                        { LblTitIncosistnc.Text = row["Texto"].ToString().Trim(); }
                        SC.CommandType = CommandType.StoredProcedure;
                        SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurPltll", TablaPlantilla);
                        SqlParameter Prmtrs1 = SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                        SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@Accion", "TEMPORAL");
                        SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdCia", Session["!dC!@"]);
                        SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@NIT", Session["Nit77Cia"]);
                        Prmtrs.SqlDbType = SqlDbType.Structured;
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DST = new DataSet())
                            {
                                SDA.SelectCommand = SC; SDA.Fill(DST); ViewState["DST"] = DST;
                                Result = DST.Tables[2].Select("Pos= '1'");
                                foreach (DataRow Row in Result)
                                { PMensj = Row["Mensj"].ToString().Trim(); }
                                if (PMensj.Trim().Equals(""))
                                { IbtGuardarCargaMax.Enabled = true; IbtGuardarCargaMax.Visible = true; }
                                else
                                {
                                    if (PMensj.Trim().Equals("Mens64PPT")) { IbtGuardarCargaMax.Visible = false; }
                                    else { IbtGuardarCargaMax.Visible = false; }
                                    Result = Idioma.Select("Objeto= '" + PMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { PMensj = row["Texto"].ToString().Trim(); LblTitIncosistnc.Text = PMensj; }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + PMensj + "');", true);
                                    GrdCargaMax.DataSource = DST.Tables[0]; GrdCargaMax.DataBind();
                                    GrdPnNew.DataSource = DST.Tables[1]; GrdPnNew.DataBind();
                                    GrdInconsist.DataSource = DST.Tables[2]; GrdInconsist.DataBind();
                                    return;
                                }
                                GrdCargaMax.DataSource = DST.Tables[0]; GrdCargaMax.DataBind();
                                GrdPnNew.DataSource = DST.Tables[1]; GrdPnNew.DataBind();
                                GrdInconsist.DataSource = DST.Tables[2]; GrdInconsist.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Import PPT Venta", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtGuardarCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (TxtNumPpt.Text.Trim().Equals("")) { return; }
            if (!TxtFechAprob.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens13PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra aprobada.
                return;
            }
            if (DdlEstado.Text.Trim().Equals("09") || DdlEstado.Text.Trim().Equals("11") || DdlEstado.Text.Trim().Equals("15") || DdlEstado.Text.Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }
            if (ViewState["Det1"].ToString().Equals("S")) //Tiene  registros
            {
                ViewState["Notificacion"] = "PLANTILLAVENTA";
                Result = Idioma.Select("Objeto= 'Mens65PPT'");
                foreach (DataRow row in Result)
                { LblTexMensjModl.Text = row["Texto"].ToString(); } //Los ítems del detalle serán eliminados y se agregarán los de la plantilla, Desea Continuar?
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
                return;
            }
            else { GuardarPlantillaMasiva(); }
        }
        //************************** Imprimir ***********************************************
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void CampoNewEncVenta()
        {
            DTEncPPT = (DataTable)ViewState["DTEncPPT"];
            if (ViewState["CamposNuevos"].ToString().Equals("N")) // Si la variable es igual a N es porque se consulto uno nuevo o se abre por primera vez la pantalla
            {
                DTEncPPT.Columns.Add("Pago", typeof(string)).SetOrdinal(0); // se agrega un nuevo campo en la primera ubicacion 
                DTEncPPT.Columns.Add("MltlPpt", typeof(string));
                DTEncPPT.Columns.Add("MltlGarantia", typeof(string));
                DTEncPPT.Columns.Add("MltlDias", typeof(string));
                DTEncPPT.Columns.Add("MltlFecha", typeof(string));
                DTEncPPT.Columns.Add("MltlCliente", typeof(string));
                DTEncPPT.Columns.Add("MltlTipo", typeof(string));
                DTEncPPT.Columns.Add("MltlContrato", typeof(string));
                DTEncPPT.Columns.Add("MltlValidez", typeof(string));
                DTEncPPT.Columns.Add("MltlObserv", typeof(string));
                DTEncPPT.Columns.Add("MltlPptComer", typeof(string));
                DTEncPPT.Columns.Add("MltlADescCosto", typeof(string));
                DTEncPPT.Columns.Add("MltlSubTtl", typeof(string));
                DTEncPPT.Columns.Add("MltlImpuesto", typeof(string));
                DTEncPPT.Columns.Add("MltlTotal", typeof(string));
                DTEncPPT.Columns.Add("MltlB", typeof(string));
                DTEncPPT.Columns.Add("MltlC", typeof(string));
                DTEncPPT.Columns.Add("MltlD", typeof(string));
                DTEncPPT.Columns.Add("MltlE", typeof(string));
                DTEncPPT.Columns.Add("MltlF", typeof(string));
                DTEncPPT.Columns.Add("MltlFtextoValidez", typeof(string));
                DTEncPPT.Columns.Add("MltlCordialMente", typeof(string));
                DTEncPPT.Columns.Add("MltlAlterno", typeof(string));
                DTEncPPT.Columns.Add("MltlDescripcionElem", typeof(string));
                DTEncPPT.Columns.Add("MltlCantMin", typeof(string));
                DTEncPPT.Columns.Add("MltlCondc", typeof(string));
                DTEncPPT.Columns.Add("MltlTiempEntr", typeof(string));
                DTEncPPT.Columns.Add("MltlVlrUnd", typeof(string));
                DTEncPPT.Columns.Add("MltlCant", typeof(string));
                DTEncPPT.Columns.Add("MltlAjust", typeof(string));
                DTEncPPT.Columns.Add("MltlRepuest", typeof(string));
                DTEncPPT.Columns.Add("MltlHH", typeof(string));
                DataRow[] Result;
                foreach (DataRow row in DTEncPPT.Rows) //enviar a los campos nuevos de lenguaje el dato a mostrar
                {
                    row["Pago"] = DdlFormPag.SelectedItem.Text.Trim();
                    row["MltlPpt"] = LblNumPpt.Text.Trim();
                    row["MltlGarantia"] = LblGarant.Text.Trim();

                    Result = Idioma.Select("Objeto= 'LblFehaPpt'");
                    foreach (DataRow Row in Result)
                    { row["MltlFecha"] = Row["Texto"].ToString(); }
                    row["MltlCliente"] = LblCliente.Text.Trim();
                    row["MltlTipo"] = LblTipo.Text.Trim();
                    row["MltlContrato"] = LblNumContrat.Text.Trim();
                    row["MltlValidez"] = LblFechValidez.Text.Trim();
                    row["MltlObserv"] = LblObserv.Text.Trim();
                    row["MltlPptComer"] = LbPptComerc.Text.Trim();

                    Result = Idioma.Select("Objeto= 'LblRprViewA'");
                    foreach (DataRow Row in Result)
                    { row["MltlADescCosto"] = Row["Texto"].ToString(); }

                    row["MltlSubTtl"] = LblSubTtl.Text.Trim();
                    row["MltlImpuesto"] = LblImpuest.Text.Trim();
                    row["MltlTotal"] = LblTotal.Text.Trim();
                    row["MltlB"] = "B. " + LblCondTiempEntregPpt.Text.Trim().ToUpper();
                    row["MltlC"] = "C. " + LblCondFormPagoPpt.Text.Trim().ToUpper();
                    row["MltlD"] = "D. " + LblCondDanoOcultPpt.Text.Trim().ToUpper();
                    row["MltlE"] = "E. " + LblCondGarantPpt.Text.Trim().ToUpper();

                    Result = Idioma.Select("Objeto= 'LblRprViewF'");
                    foreach (DataRow Row in Result)
                    { row["MltlF"] = Row["Texto"].ToString(); }

                    Result = Idioma.Select("Objeto= 'LblRprViewFTexto'");
                    foreach (DataRow Row in Result)
                    { row["MltlFtextoValidez"] = Row["Texto"].ToString(); }

                    Result = Idioma.Select("Objeto= 'LblRprViewCordlMent'");
                    foreach (DataRow Row in Result)
                    { row["MltlCordialMente"] = Row["Texto"].ToString(); }
                    row["MltlAjust"] = LblAjusVent.Text.Trim().ToUpper();
                    row["MltlRepuest"] = LblVlrRecurso.Text.Trim().ToUpper();
                    row["MltlHH"] = lblVlrMnObr.Text.Trim().ToUpper();
                }

                ViewState["CamposNuevos"] = "S";
            }
        }
        protected void BtnImprPpal_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DTEncPPT = (DataTable)ViewState["DTEncPPT"];

            CampoNewEncVenta();

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            //Cnx.SelecBD();
            // using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            //  {
            ReportParameter[] parameters = new ReportParameter[3];

            parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
            parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
            parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

            RpVwAll.LocalReport.EnableExternalImages = true;
            if (DdlTipo.Text.Trim().Equals("00001") || DdlTipo.Text.Trim().Equals("00001")) // venta o a todo costo
            { RpVwAll.LocalReport.ReportPath = "Report/RtesMro/Inf_Propuesta.rdlc"; }
            else { RpVwAll.LocalReport.ReportPath = "Report/RtesMro/Inf_propuestaRepa.rdlc"; }// Repa                    
            RpVwAll.LocalReport.DataSources.Clear();
            RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DTEncPPT));
            RpVwAll.LocalReport.SetParameters(parameters);
            RpVwAll.LocalReport.Refresh();
            // }
        }
        protected void BtnImprDet_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DTEncPPT = (DataTable)ViewState["DTEncPPT"];
            DtDet1All = (DataTable)ViewState["DtDet1"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            DT = DtDet1All.Clone();
            Result = DtDet1All.Select("Aprobado= '1'");
            foreach (DataRow Row in Result)
            { DT.ImportRow(Row); }
            CampoNewEncVenta();
            foreach (DataRow row in DTEncPPT.Rows) //enviar a los campos nuevos de lenguaje el dato a mostrar
            {
                row["Pago"] = DdlFormPag.SelectedItem.Text.Trim();
                row["MltlPpt"] = LblNumPpt.Text.Trim();
                row["MltlGarantia"] = LblGarant.Text.Trim();

                Result = Idioma.Select("Objeto= 'LblFehaPpt'");
                foreach (DataRow Row in Result)
                { row["MltlFecha"] = Row["Texto"].ToString(); }
                row["MltlCliente"] = LblCliente.Text.Trim();
                row["MltlTipo"] = LblTipo.Text.Trim();
                row["MltlContrato"] = LblNumContrat.Text.Trim();
                row["MltlValidez"] = LblFechValidez.Text.Trim();
                row["MltlObserv"] = LblObserv.Text.Trim();
                row["MltlPptComer"] = LbPptComerc.Text.Trim();

                Result = Idioma.Select("Objeto= 'LblRprViewA'");
                foreach (DataRow Row in Result)
                { row["MltlADescCosto"] = Row["Texto"].ToString(); }

                row["MltlSubTtl"] = LblSubTtl.Text.Trim();
                row["MltlImpuesto"] = LblImpuest.Text.Trim();
                row["MltlTotal"] = LblTotal.Text.Trim();
                row["MltlB"] = "B. " + LblCondTiempEntregPpt.Text.Trim().ToUpper();
                row["MltlC"] = "C. " + LblCondFormPagoPpt.Text.Trim().ToUpper();
                row["MltlD"] = "D. " + LblCondDanoOcultPpt.Text.Trim().ToUpper();
                row["MltlE"] = "E. " + LblCondGarantPpt.Text.Trim().ToUpper();

                Result = Idioma.Select("Objeto= 'LblRprViewF'");
                foreach (DataRow Row in Result)
                { row["MltlF"] = Row["Texto"].ToString(); }

                Result = Idioma.Select("Objeto= 'LblRprViewFTexto'");
                foreach (DataRow Row in Result)
                { row["MltlFtextoValidez"] = Row["Texto"].ToString(); }

                Result = Idioma.Select("Objeto= 'LblRprViewCordlMent'");
                foreach (DataRow Row in Result)
                { row["MltlCordialMente"] = Row["Texto"].ToString(); }

                Result = Idioma.Select("Objeto= 'RpVwAlterno'");
                foreach (DataRow Row in Result)
                { row["MltlAlterno"] = Row["Texto"].ToString(); }

                row["MltlDescripcionElem"] = GrdDet1.Columns[5].HeaderText;

                Result = Idioma.Select("Objeto= 'RptVwCantMin'");
                foreach (DataRow Row in Result)
                { row["MltlCantMin"] = Row["Texto"].ToString(); }

                Result = Idioma.Select("Objeto= 'RpVwCondic'");
                foreach (DataRow Row in Result)
                { row["MltlCondc"] = Row["Texto"].ToString(); }

                row["MltlTiempEntr"] = GrdDet1.Columns[18].HeaderText;
                row["MltlVlrUnd"] = GrdDet1.Columns[8].HeaderText;
                row["MltlCant"] = GrdPnSugerd.Columns[2].HeaderText;
            }

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[3];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

                RpVwAll.LocalReport.EnableExternalImages = true;
                if (DdlTipo.Text.Trim().Equals("00001") || DdlTipo.Text.Trim().Equals("00001")) // venta o a todo costo
                { RpVwAll.LocalReport.ReportPath = "Report/RtesMro/Inf_propuesta_detalle.rdlc"; }
                else { RpVwAll.LocalReport.ReportPath = "Report/RtesMro/Inf_propuesta_detalleRepa.rdlc"; }
                RpVwAll.LocalReport.DataSources.Clear();
                RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DTEncPPT));
                RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DT));
                RpVwAll.LocalReport.SetParameters(parameters);
                RpVwAll.LocalReport.Refresh();
            }
        }
        //************************** Alertas ***********************************************
        protected void Alertas()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_TablasMRO 15,'','','','','','','','','',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSAlerta = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DSAlerta);
                            DSAlerta.Tables[0].TableName = "SinDetAprob";
                            DSAlerta.Tables[1].TableName = "OTDuplicada";
                            ViewState["DSAlerta"] = DSAlerta;
                        }
                    }
                }
                DSAlerta = (DataSet)ViewState["DSAlerta"];
                if (DSAlerta.Tables["SinDetAprob"].Rows.Count > 0)
                {
                    if ((int)ViewState["VblCE1"] == 1)
                    {
                        GrdAlrtDetSinAprb.Visible = true; LblTitAlertaSinDetAprob.Visible = true;
                        GrdAlrtDetSinAprb.DataSource = DSAlerta.Tables["SinDetAprob"]; GrdAlrtDetSinAprb.DataBind();
                    }
                    GrdAlrtOtDuplicada.DataSource = DSAlerta.Tables["OTDuplicada"];
                    GrdAlrtOtDuplicada.DataBind();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalAlerta", "$('#ModalAlerta').modal();", true);
                }
                else
                {
                    if (DSAlerta.Tables["OTDuplicada"].Rows.Count > 0)
                    {
                        GrdAlrtOtDuplicada.DataSource = DSAlerta.Tables["OTDuplicada"];
                        GrdAlrtOtDuplicada.DataBind();
                        GrdAlrtDetSinAprb.DataSource = null; GrdAlrtDetSinAprb.DataBind();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalAlerta", "$('#ModalAlerta').modal();", true);
                    }
                }
            }
        }
        protected void GrdAlrtDetSinAprb_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DSAlerta = (DataSet)ViewState["DSAlerta"];
            GrdAlrtDetSinAprb.EditIndex = e.NewEditIndex;
            GrdAlrtDetSinAprb.DataSource = DSAlerta.Tables["SinDetAprob"]; GrdAlrtDetSinAprb.DataBind();
        }
        protected void GrdAlrtDetSinAprb_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int VbAprob = (GrdAlrtDetSinAprb.Rows[e.RowIndex].FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0;
            if (VbAprob == 1)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 15,@US,'','','','','','','','UPDATE',@ID,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@ID", GrdAlrtDetSinAprb.DataKeys[e.RowIndex].Values["IdPropuesta"].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                GrdAlrtDetSinAprb.EditIndex = -1;
                                Alertas();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE Alerta PPT aprobada sin detalle aprobado", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                DSAlerta = (DataSet)ViewState["DSAlerta"];
                GrdAlrtDetSinAprb.EditIndex = -1;
                GrdAlrtDetSinAprb.DataSource = DSAlerta.Tables["SinDetAprob"]; GrdAlrtDetSinAprb.DataBind();
            }
        }
        protected void GrdAlrtDetSinAprb_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DSAlerta = (DataSet)ViewState["DSAlerta"];
            GrdAlrtDetSinAprb.EditIndex = -1;
            GrdAlrtDetSinAprb.DataSource = DSAlerta.Tables["SinDetAprob"]; GrdAlrtDetSinAprb.DataBind();
        }
        protected void GrdAlrtDetSinAprb_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();
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
                if (imgE != null)
                {
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
    }
}