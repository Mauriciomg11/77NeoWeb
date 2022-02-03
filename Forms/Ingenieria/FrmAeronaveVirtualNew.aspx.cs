using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmAeronaveVirtualNew : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            Page.Title = string.Format("Aeronave Virtual");
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
                TitForm.Text = "Aeronave Virtual";
                ViewState["PNSN"] = "PN";
                ViewState["CodElemento"] = "";
                ViewState["EsMotor"] = "N";
                ViewState["Ventana"] = 0;
                ViewState["CodAeronave"] = 0;
                ViewState["CodModelo"] = "";
                ViewState["ValidaFechaSvc"] = "N";
                ViewState["TieneCompensacion"] = "N";
                ViewState["Propiedad"] = 2;// 0= Propiedad Cia| 1=ajeno (Cliente) para la creacion de los componentes 
                MultVw.ActiveViewIndex = 0;
                BindDDdl("UPD");
                BtnInsElem.CssClass = "btn btn-primary";
                ViewState["PageTit"] = "";
                ModSeguridad();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                BtnGuardarInsElem.Visible = false;
                BtnGuardarRemElem.Visible = false;
                BtnGuardarInsMay.Visible = false;
                BtnGuardarRemMay.Visible = false;
                BtnGuardarInsSubC.Visible = false;
                BtnGuardarRemSubC.Visible = false;
                BtnCrearElem.Visible = false;
                DdlAeroInsElem.Visible = false;
                DdlAeroRemElem.Visible = false;
                DdlAeroInsMay.Visible = false;
                DdlAeroRemMay.Visible = false;
                DdlPNInsSubC.Visible = false;
                DdlPNRemSubC.Visible = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
            }
            if (ClsP.GetConsultar() == 0)
            {
            }
            if (ClsP.GetImprimir() == 0)
            {

            }
            if (ClsP.GetEliminar() == 0)
            {
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
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,1,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                SC.Parameters.AddWithValue("@F", "MRO");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("N"))
                    {
                        //Botones de propiedad 
                        BtnPropiedad.Visible = false;
                        BtnCliente.Visible = false;
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
                SC.Parameters.AddWithValue("@F1", "FrmAeronaveVirtualNew");
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
                    if (bO.Equals("CaptionAV"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("CaptionAV") ? bT : TitForm.Text;
                    BtnInsElem.Text = bO.Equals("BtnInsElem") ? bT : BtnInsElem.Text;
                    BtnRemElem.Text = bO.Equals("BtnRemElem") ? bT : BtnRemElem.Text;
                    BtnInsMayor.Text = bO.Equals("BtnInsMayor") ? bT : BtnInsMayor.Text;
                    BtnRemMayor.Text = bO.Equals("BtnRemMayor") ? bT : BtnRemMayor.Text;
                    BtnInsSubC.Text = bO.Equals("BtnInsSubC") ? bT : BtnInsSubC.Text;
                    BtnRemSubC.Text = bO.Equals("BtnRemSubC") ? bT : BtnRemSubC.Text;
                    BtnCrearElem.Text = bO.Equals("BtnCrearElem") ? bT : BtnCrearElem.Text;
                    // ************************************** Instalar Elemento  *******************************************************       
                    LblTitInsEle.Text = bO.Equals("LblTitInsEle") ? bT : LblTitInsEle.Text;
                    LblAeroInsElem.Text = bO.Equals("LblAeroInsElem") ? bT + ":" : LblAeroInsElem.Text;
                    LblBusInsEle.Text = bO.Equals("Busqueda") ? bT : LblBusInsEle.Text;
                    if (bO.Equals("placeholder"))
                    {
                        TxtBusqueda.Attributes.Add("placeholder", bT);
                        TxtRemBusqueda.Attributes.Add("placeholder", bT);
                        TxtBusqInsMay.Attributes.Add("placeholder", bT);
                        TxtRemMayBusqueda.Attributes.Add("placeholder", bT);
                        TxtBusqInsSubC.Attributes.Add("placeholder", bT);
                        TxtBusqRemSubC.Attributes.Add("placeholder", bT);
                    }
                    BtnUltNivInsElem.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivInsElem.Text;
                    BtnUltNivInsElem.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivInsElem.ToolTip;
                    BtnAKVirtualInsElem.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualInsElem.Text;
                    BtnAKVirtualInsElem.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualInsElem.ToolTip;
                    BtnGuardarInsElem.Text = bO.Equals("BtnGuardarInsElem") ? bT : BtnGuardarInsElem.Text;
                    BtnGuardarInsElem.ToolTip = bO.Equals("BtnGuardarInsElemTT") ? bT : BtnGuardarInsElem.ToolTip;
                    LblUbiTecInsElem.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecInsElem.Text;
                    LblPosicInsElem.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicInsElem.Text;
                    LblFechaInsElem.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaInsElem.Text;
                    BtnCompensac.ToolTip = bO.Equals("BtnCompensacion") ? bT : BtnCompensac.ToolTip;
                    LblMotivInsElem.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivInsElem.Text;
                    TxtTitContadores.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitContadores.Text;
                    GrdHisContInsElem.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContInsElem.Columns[0].HeaderText;
                    GrdHisContInsElem.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContInsElem.Columns[1].HeaderText;
                    GrdHisContInsElem.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContInsElem.Columns[2].HeaderText;
                    GrdHisContInsElem.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContInsElem.EmptyDataText;
                    TxtTitServicios.Text = bO.Equals("TxtTitServicios01") ? bT : TxtTitServicios.Text;
                    GrdSvcInsElem.Columns[0].HeaderText = bO.Equals("GrdOt") ? bT : GrdSvcInsElem.Columns[0].HeaderText;
                    GrdSvcInsElem.Columns[2].HeaderText = bO.Equals("GrdUltCump") ? bT : GrdSvcInsElem.Columns[2].HeaderText;
                    GrdSvcInsElem.Columns[4].HeaderText = bO.Equals("GrdRte") ? bT : GrdSvcInsElem.Columns[4].HeaderText;
                    GrdSvcInsElem.Columns[5].HeaderText = bO.Equals("GrdServc") ? bT : GrdSvcInsElem.Columns[5].HeaderText;
                    GrdSvcInsElem.Columns[6].HeaderText = bO.Equals("GrdContador") ? bT : GrdSvcInsElem.Columns[6].HeaderText;
                    GrdSvcInsElem.Columns[7].HeaderText = bO.Equals("GrdFrec") ? bT : GrdSvcInsElem.Columns[7].HeaderText;
                    GrdSvcInsElem.Columns[8].HeaderText = bO.Equals("GrdFrecDia") ? bT : GrdSvcInsElem.Columns[8].HeaderText;
                    GrdSvcInsElem.Columns[9].HeaderText = bO.Equals("GrdVlrUltC") ? bT : GrdSvcInsElem.Columns[9].HeaderText;
                    GrdSvcInsElem.Columns[10].HeaderText = bO.Equals("GrdGeneHis") ? bT : GrdSvcInsElem.Columns[10].HeaderText;
                    GrdSvcInsElem.EmptyDataText = bO.Equals("SinServicioAsig") ? bT : GrdSvcInsElem.EmptyDataText;
                    // ************************************** Visualizar Elemento Instalados  *******************************************************       
                    LblTitAeroVirtual.Text = bO.Equals("LblTitAeroVirtual") ? bT : LblTitAeroVirtual.Text;
                    IbtCerrarAeroVirtual.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAeroVirtual.ToolTip;
                    LblAeroVirtualHK.Text = bO.Equals("LblAeroInsElem") ? bT + ":" : LblAeroVirtualHK.Text;
                    GrdListaAeroVirtual.Columns[0].HeaderText = bO.Equals("GrdUltNivelAV") ? bT : GrdListaAeroVirtual.Columns[0].HeaderText;
                    GrdListaAeroVirtual.Columns[1].HeaderText = bO.Equals("GrdDescrAV") ? bT : GrdListaAeroVirtual.Columns[1].HeaderText;
                    GrdListaAeroVirtual.Columns[4].HeaderText = bO.Equals("GrdDescrElemAV") ? bT : GrdListaAeroVirtual.Columns[4].HeaderText;
                    GrdListaAeroVirtual.Columns[5].HeaderText = bO.Equals("LblPosicion") ? bT : GrdListaAeroVirtual.Columns[5].HeaderText;
                    GrdListaAeroVirtual.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdListaAeroVirtual.EmptyDataText;
                    // ************************************** Remoción Elemento   *******************************************************       
                    LblTitRemEle.Text = bO.Equals("LblTitRemEle") ? bT : LblTitRemEle.Text;
                    LblAeroRemElem.Text = bO.Equals("LblAeroInsElem") ? bT + ":" : LblAeroRemElem.Text;
                    LblBusRemEle.Text = bO.Equals("Busqueda") ? bT : LblBusRemEle.Text;
                    BtnUltNivRemElem.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivRemElem.Text;
                    BtnUltNivRemElem.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivRemElem.ToolTip;
                    BtnAKVirtualRemElem.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualRemElem.Text;
                    BtnAKVirtualRemElem.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualRemElem.ToolTip;
                    BtnAbrirOTCerrar.Text = bO.Equals("BtnAbrirOTCerrar") ? bT : BtnAbrirOTCerrar.Text;
                    BtnGuardarRemElem.Text = bO.Equals("BtnGuardarRemElem") ? bT : BtnGuardarRemElem.Text;
                    BtnGuardarRemElem.ToolTip = bO.Equals("BtnGuardarRemElemTT") ? bT : BtnGuardarRemElem.ToolTip;
                    LblUbiTecRemElem.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecRemElem.Text;
                    LblPosicRemElem.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicRemElem.Text;
                    LblFechaRemElem.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaRemElem.Text;
                    BtnRemCompensac.ToolTip = bO.Equals("BtnCompensacion") ? bT : BtnRemCompensac.ToolTip;
                    LblMotivRemElem.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivRemElem.Text;
                    TxtTitRemContadores.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitRemContadores.Text;
                    GrdHisContRemElem.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContRemElem.Columns[0].HeaderText;
                    GrdHisContRemElem.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContRemElem.Columns[1].HeaderText;
                    GrdHisContRemElem.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContRemElem.Columns[2].HeaderText;
                    GrdHisContRemElem.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContRemElem.EmptyDataText;
                    TxtTitRemServicios.Text = bO.Equals("TxtTitRemServicios") ? bT : TxtTitRemServicios.Text;
                    // ************************************** Ot para cerrar   *******************************************************       
                    TxtTitCierreOT.Text = bO.Equals("TxtTitCierreOT") ? bT : TxtTitCierreOT.Text;
                    IbtCerrarOTcierre.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarOTcierre.ToolTip;
                    GrdOtCerrar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdOtCerrar.EmptyDataText;
                    GrdOtCerrar.Columns[1].HeaderText = bO.Equals("GrdOt") ? bT : GrdOtCerrar.Columns[1].HeaderText;
                    GrdOtCerrar.Columns[2].HeaderText = bO.Equals("LblFecha") ? bT : GrdOtCerrar.Columns[2].HeaderText;
                    GrdOtCerrar.Columns[5].HeaderText = bO.Equals("GrdTrabReqOT") ? bT : GrdOtCerrar.Columns[5].HeaderText;
                    GrdOtCerrar.Columns[6].HeaderText = bO.Equals("GrdMotCierOT") ? bT : GrdOtCerrar.Columns[6].HeaderText;
                    // ************************************** Compensar   *******************************************************       
                    TxtTitCompensacion.Text = bO.Equals("TxtTitCompensacion") ? bT : TxtTitCompensacion.Text;
                    IbtCerrarCompensacion.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCompensacion.ToolTip;
                    BtnCompensReinicio.Text = bO.Equals("BtnCompensReinicio") ? bT : BtnCompensReinicio.Text;
                    BtnCompensReinicio.ToolTip = bO.Equals("BtnCompensReinicioTT") ? bT : BtnCompensReinicio.ToolTip;
                    CkbCompensInicioDia.Text = bO.Equals("CkbCompensInicioDia") ? bT : CkbCompensInicioDia.Text;
                    LblMarcarTry.Text = bO.Equals("LblMarcarTry") ? bT : LblMarcarTry.Text;
                    GrdCompensLv.Columns[2].HeaderText = bO.Equals("GrdLVComp") ? bT : GrdCompensLv.Columns[2].HeaderText;
                    GrdCompensLv.Columns[3].HeaderText = bO.Equals("GrdOrigComp") ? bT : GrdCompensLv.Columns[3].HeaderText;
                    GrdCompensLv.Columns[4].HeaderText = bO.Equals("GrdDestComp") ? bT : GrdCompensLv.Columns[4].HeaderText;
                    GrdCompensLv.Columns[5].HeaderText = bO.Equals("GrdHorComp") ? bT : GrdCompensLv.Columns[5].HeaderText;
                    GrdCompensLv.Columns[6].HeaderText = bO.Equals("GrdHorAcumComp") ? bT : GrdCompensLv.Columns[6].HeaderText;
                    GrdCompensLv.Columns[7].HeaderText = bO.Equals("GrdCcAcum") ? bT : GrdCompensLv.Columns[7].HeaderText;
                    GrdCompensLv.Columns[8].HeaderText = bO.Equals("GrdHraRainComp") ? bT : GrdCompensLv.Columns[8].HeaderText;
                    GrdCompensLv.Columns[9].HeaderText = bO.Equals("GrdCclRmainComp") ? bT : GrdCompensLv.Columns[9].HeaderText;
                    // ************************************** Instalar Mayor   *******************************************************    
                    LblTitInsMay.Text = bO.Equals("LblTitInsMay") ? bT : LblTitInsMay.Text;
                    LblAeroInsMay.Text = bO.Equals("LblAeroInsElem") ? bT + ":" : LblAeroInsMay.Text;
                    LblBusInsMay.Text = bO.Equals("Busqueda") ? bT : LblBusInsMay.Text;
                    BtnUltNivInsMay.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivInsMay.Text;
                    BtnUltNivInsMay.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivInsMay.ToolTip;
                    BtnAKVirtualInsMay.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualInsMay.Text;
                    BtnAKVirtualInsMay.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualInsMay.ToolTip;
                    BtnVisualizarMay.Text = bO.Equals("BtnVisualizarMay") ? bT : BtnVisualizarMay.Text;
                    BtnVisualizarMay.ToolTip = bO.Equals("BtnVisualizarMayTT") ? bT : BtnVisualizarMay.ToolTip;
                    BtnGuardarInsMay.Text = bO.Equals("BtnGuardarInsElem") ? bT : BtnGuardarInsMay.Text;
                    BtnGuardarInsMay.ToolTip = bO.Equals("BtnGuardarInsElemTT") ? bT : BtnGuardarInsMay.ToolTip;
                    LblUbiTecInsMay.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecInsMay.Text;
                    LblPosicInsMay.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicInsMay.Text;
                    LblFechaInsMay.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaInsMay.Text;
                    BtnCompensacInsMay.ToolTip = bO.Equals("BtnCompensacion") ? bT : BtnCompensacInsMay.ToolTip;
                    LblMotivInsMay.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivInsMay.Text;
                    TxtTitHisContInsMay.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitHisContInsMay.Text;
                    GrdHisContInsMay.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContInsMay.Columns[0].HeaderText;
                    GrdHisContInsMay.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContInsMay.Columns[1].HeaderText;
                    GrdHisContInsMay.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContInsMay.Columns[2].HeaderText;
                    GrdHisContInsMay.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContInsMay.EmptyDataText;
                    TxtTitSvcInsMay.Text = bO.Equals("TxtTitSvcInsMay01") ? bT : TxtTitSvcInsMay.Text;
                    GrdSvcInsMay.Columns[0].HeaderText = bO.Equals("GrdOt") ? bT : GrdSvcInsMay.Columns[0].HeaderText;
                    GrdSvcInsMay.Columns[2].HeaderText = bO.Equals("GrdUltCump") ? bT : GrdSvcInsMay.Columns[2].HeaderText;
                    GrdSvcInsMay.Columns[4].HeaderText = bO.Equals("GrdRte") ? bT : GrdSvcInsMay.Columns[4].HeaderText;
                    GrdSvcInsMay.Columns[5].HeaderText = bO.Equals("GrdServc") ? bT : GrdSvcInsMay.Columns[5].HeaderText;
                    GrdSvcInsMay.Columns[6].HeaderText = bO.Equals("GrdContador") ? bT : GrdSvcInsMay.Columns[6].HeaderText;
                    GrdSvcInsMay.Columns[7].HeaderText = bO.Equals("GrdFrec") ? bT : GrdSvcInsMay.Columns[7].HeaderText;
                    GrdSvcInsMay.Columns[8].HeaderText = bO.Equals("GrdFrecDia") ? bT : GrdSvcInsMay.Columns[8].HeaderText;
                    GrdSvcInsMay.Columns[9].HeaderText = bO.Equals("GrdVlrUltC") ? bT : GrdSvcInsMay.Columns[9].HeaderText;
                    GrdSvcInsMay.Columns[10].HeaderText = bO.Equals("GrdGeneHis") ? bT : GrdSvcInsMay.Columns[10].HeaderText;
                    GrdSvcInsMay.EmptyDataText = bO.Equals("SinServicioAsig") ? bT : GrdSvcInsMay.EmptyDataText;
                    // ************************************** Visualizar Mayor   *******************************************************    
                    LblTitVisualizarMay.Text = bO.Equals("LblTitVisualizarMay") ? bT : LblTitVisualizarMay.Text;
                    IbtCerrarVisualMay.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarVisualMay.ToolTip;
                    GrdVisualMay.Columns[0].HeaderText = bO.Equals("BtnUltNivInsElemTT") ? bT : GrdVisualMay.Columns[0].HeaderText;
                    GrdVisualMay.Columns[3].HeaderText = bO.Equals("GrdDescrElemAV") ? bT : GrdVisualMay.Columns[3].HeaderText;
                    GrdVisualMay.Columns[4].HeaderText = bO.Equals("LblPosicion") ? bT : GrdVisualMay.Columns[4].HeaderText;
                    GrdVisualMay.Columns[5].HeaderText = bO.Equals("LblAeroInsElem") ? bT : GrdVisualMay.Columns[5].HeaderText;
                    GrdVisualMay.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdVisualMay.EmptyDataText;
                    // ************************************** Remocion Mayor   *******************************************************   
                    LblTitRemMay.Text = bO.Equals("LblTitRemMay") ? bT : LblTitRemMay.Text;
                    LblAeroRemElMay.Text = bO.Equals("LblAeroInsElem") ? bT + ":" : LblAeroRemElMay.Text;
                    LblBusRemMay.Text = bO.Equals("Busqueda") ? bT : LblBusRemMay.Text;
                    BtnUltNivRemMay.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivRemMay.Text;
                    BtnUltNivRemMay.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivRemMay.ToolTip;
                    BtnAKVirtualRemMay.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualRemMay.Text;
                    BtnAKVirtualRemMay.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualRemMay.ToolTip;
                    BtnVisualizarRemMay.Text = bO.Equals("BtnVisualizarMay") ? bT : BtnVisualizarRemMay.Text;
                    BtnVisualizarRemMay.ToolTip = bO.Equals("BtnVisualizarMayTT") ? bT : BtnVisualizarRemMay.ToolTip;
                    BtnAbrirOTCerrarRemMay.Text = bO.Equals("BtnAbrirOTCerrar") ? bT : BtnAbrirOTCerrarRemMay.Text;
                    BtnGuardarRemMay.Text = bO.Equals("BtnGuardarRemElem") ? bT : BtnGuardarRemMay.Text;
                    BtnGuardarRemMay.ToolTip = bO.Equals("BtnGuardarRemElemTT") ? bT : BtnGuardarRemMay.ToolTip;
                    LblUbiTecRemMay.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecRemMay.Text;
                    LblPosicRemMay.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicRemMay.Text;
                    LblFechaRemMay.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaRemMay.Text;
                    BtnRemMayCompensac.ToolTip = bO.Equals("BtnCompensacion") ? bT : BtnRemMayCompensac.ToolTip;
                    LblMotivRemMay.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivRemMay.Text;
                    TxtTitRemMayContadores.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitRemMayContadores.Text;
                    GrdHisContRemMay.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContRemMay.Columns[0].HeaderText;
                    GrdHisContRemMay.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContRemMay.Columns[1].HeaderText;
                    GrdHisContRemMay.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContRemMay.Columns[2].HeaderText;
                    GrdHisContRemMay.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContRemMay.EmptyDataText;
                    LblTitRemoMayor.Text = bO.Equals("LblTitRemoMayor") ? bT : LblTitRemoMayor.Text;
                    GrdBusqRemMay.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusqRemMay.EmptyDataText;
                    // ************************************** Instalar SubComponente   *******************************************************   
                    LblTitInsSubC.Text = bO.Equals("LblTitInsSubC") ? bT : LblTitInsSubC.Text;
                    LblPNMyInsSubC.Text = bO.Equals("LblPNMyInsSubC") ? bT + ":" : LblPNMyInsSubC.Text;
                    LblSNMyInsSubC.Text = bO.Equals("LblSNMyInsSubC") ? bT + ":" : LblSNMyInsSubC.Text;
                    LblModelInsSubC.Text = bO.Equals("LblModelInsSubC") ? bT + ":" : LblModelInsSubC.Text;
                    LblBusInsSubC.Text = bO.Equals("Busqueda") ? bT : LblBusInsSubC.Text;
                    BtnUltNivInsSubC.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivInsSubC.Text;
                    BtnUltNivInsSubC.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivInsSubC.ToolTip;
                    BtnAKVirtualInsSubC.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualInsSubC.Text;
                    BtnAKVirtualInsSubC.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualInsSubC.ToolTip;
                    BtnVisualizarMayInsSubC.Text = bO.Equals("BtnVisualizarMay") ? bT : BtnVisualizarMayInsSubC.Text;
                    BtnVisualizarMayInsSubC.ToolTip = bO.Equals("BtnVisualizarMayTT") ? bT : BtnVisualizarMayInsSubC.ToolTip;
                    BtnGuardarInsSubC.Text = bO.Equals("BtnGuardarInsElem") ? bT : BtnGuardarInsSubC.Text;
                    BtnGuardarInsSubC.ToolTip = bO.Equals("BtnGuardarInsElemTT") ? bT : BtnGuardarInsSubC.ToolTip;
                    LblUbiTecInsSubC.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecInsSubC.Text;
                    LblPosicInsSubC.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicInsSubC.Text;
                    LblFechaInsSubC.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaInsSubC.Text;
                    LblMotivInsSubC.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivInsSubC.Text;
                    TxtTitContadoresInsSubC.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitContadoresInsSubC.Text;
                    GrdHisContInsSubC.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContInsSubC.Columns[0].HeaderText;
                    GrdHisContInsSubC.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContInsSubC.Columns[1].HeaderText;
                    GrdHisContInsSubC.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContInsSubC.Columns[2].HeaderText;
                    GrdHisContInsSubC.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContInsSubC.EmptyDataText;
                    TxtTitServcInsSubC.Text = bO.Equals("TxtTitServcInsSubC01") ? bT : TxtTitServcInsSubC.Text;
                    GrdSvcInsSubC.Columns[0].HeaderText = bO.Equals("GrdOt") ? bT : GrdSvcInsSubC.Columns[0].HeaderText;
                    GrdSvcInsSubC.Columns[2].HeaderText = bO.Equals("GrdUltCump") ? bT : GrdSvcInsSubC.Columns[2].HeaderText;
                    GrdSvcInsSubC.Columns[4].HeaderText = bO.Equals("GrdRte") ? bT : GrdSvcInsSubC.Columns[4].HeaderText;
                    GrdSvcInsSubC.Columns[5].HeaderText = bO.Equals("GrdServc") ? bT : GrdSvcInsSubC.Columns[5].HeaderText;
                    GrdSvcInsSubC.Columns[6].HeaderText = bO.Equals("GrdContador") ? bT : GrdSvcInsSubC.Columns[6].HeaderText;
                    GrdSvcInsSubC.Columns[7].HeaderText = bO.Equals("GrdFrec") ? bT : GrdSvcInsSubC.Columns[7].HeaderText;
                    GrdSvcInsSubC.Columns[8].HeaderText = bO.Equals("GrdFrecDia") ? bT : GrdSvcInsSubC.Columns[8].HeaderText;
                    GrdSvcInsSubC.Columns[9].HeaderText = bO.Equals("GrdVlrUltC") ? bT : GrdSvcInsSubC.Columns[9].HeaderText;
                    GrdSvcInsSubC.Columns[10].HeaderText = bO.Equals("GrdGeneHis") ? bT : GrdSvcInsSubC.Columns[10].HeaderText;
                    GrdSvcInsSubC.EmptyDataText = bO.Equals("SinServicioAsig") ? bT : GrdSvcInsSubC.EmptyDataText;
                    // ************************************** Remocion SubComponente   *******************************************************   
                    LblTitRemSubC.Text = bO.Equals("LblTitRemSubC") ? bT : LblTitRemSubC.Text;
                    LblPNMyRemSubC.Text = bO.Equals("LblPNMyInsSubC") ? bT + ":" : LblPNMyRemSubC.Text;
                    LblSNMyRemSubC.Text = bO.Equals("LblSNMyInsSubC") ? bT + ":" : LblSNMyRemSubC.Text;
                    LblModelRemSubC.Text = bO.Equals("LblModelInsSubC") ? bT + ":" : LblModelRemSubC.Text;
                    LblBusRemSubC.Text = bO.Equals("Busqueda") ? bT : LblBusRemSubC.Text;
                    BtnUltNivRemSubC.Text = bO.Equals("BtnUltNivInsElem") ? bT : BtnUltNivRemSubC.Text;
                    BtnUltNivRemSubC.ToolTip = bO.Equals("BtnUltNivInsElemTT") ? bT + "." : BtnUltNivRemSubC.ToolTip;
                    BtnAKVirtualRemSubC.Text = bO.Equals("BtnAKVirtualInsElem") ? bT : BtnAKVirtualRemSubC.Text;
                    BtnAKVirtualRemSubC.ToolTip = bO.Equals("BtnAKVirtualInsElemTT") ? bT : BtnAKVirtualRemSubC.ToolTip;
                    BtnVisualizarMayRemSubC.Text = bO.Equals("BtnVisualizarMay") ? bT : BtnVisualizarMayRemSubC.Text;
                    BtnVisualizarMayRemSubC.ToolTip = bO.Equals("BtnVisualizarMayTT") ? bT : BtnVisualizarMayRemSubC.ToolTip;
                    BtnGuardarRemSubC.Text = bO.Equals("BtnGuardarRemElem") ? bT : BtnGuardarRemSubC.Text;
                    BtnGuardarRemSubC.ToolTip = bO.Equals("BtnGuardarRemElemTT") ? bT : BtnGuardarRemSubC.ToolTip;
                    BtnAbrirOTCerrarRemSubC.Text = bO.Equals("BtnAbrirOTCerrar") ? bT : BtnAbrirOTCerrarRemSubC.Text;
                    LblUbiTecRemSubC.Text = bO.Equals("BtnUltNivInsElemTT") ? bT + ":" : LblUbiTecRemSubC.Text;
                    LblPosicRemSubC.Text = bO.Equals("LblPosicion") ? bT + ":" : LblPosicRemSubC.Text;
                    LblFechaRemSubC.Text = bO.Equals("LblFecha") ? bT + ":" : LblFechaRemSubC.Text;
                    LblMotivRemSubC.Text = bO.Equals("LblMotivo") ? bT + ":" : LblMotivRemSubC.Text;
                    TxtTitContadoresRemSub.Text = bO.Equals("TxtTitContadores") ? bT : TxtTitContadoresRemSub.Text;
                    GrdHisContRemSubC.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdHisContRemSubC.Columns[0].HeaderText;
                    GrdHisContRemSubC.Columns[1].HeaderText = bO.Equals("LblFecha") ? bT : GrdHisContRemSubC.Columns[1].HeaderText;
                    GrdHisContRemSubC.Columns[2].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdHisContRemSubC.Columns[2].HeaderText;
                    GrdHisContRemSubC.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdHisContRemSubC.EmptyDataText;
                    TxtTitSrvcRemSubC.Text = bO.Equals("TxtTitSrvcRemSubC") ? bT : TxtTitSrvcRemSubC.Text;
                    GrdBusqRemSubC.EmptyDataText = bO.Equals("SinServicioAsig") ? bT : GrdBusqRemSubC.EmptyDataText;
                    // ************************************** Crear elementos  *******************************************************  
                    LblTitCrearElem.Text = bO.Equals("LblTitCrearElem") ? bT : LblTitCrearElem.Text;
                    IbtCerrarCrearElem.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCrearElem.ToolTip;
                    BtnPropiedad.Text = bO.Equals("BtnPropiedad") ? bT : BtnPropiedad.Text;
                    BtnCliente.Text = bO.Equals("BtnCliente") ? bT : BtnCliente.Text;
                    BtnCrearElemGuardar.Text = bO.Equals("BtnCrearElemGuardar") ? bT : BtnCrearElemGuardar.Text;
                    BtnCrearElemGuardar.ToolTip = bO.Equals("BtnCrearElemGuardarTT") ? bT : BtnCrearElemGuardar.ToolTip;
                    LblTitCrearEDatosE.Text = bO.Equals("LblTitCrearEDatosE") ? bT : LblTitCrearEDatosE.Text;
                    LblCrearElemFechRec.Text = bO.Equals("LblCrearElemFechRec") ? bT : LblCrearElemFechRec.Text;
                    LblCrearElemFechFabr.Text = bO.Equals("LblCrearElemFechFabr") ? bT : LblCrearElemFechFabr.Text;
                    LblCrearEContadores.Text = bO.Equals("LblCrearElemFechFabr") ? bT : LblCrearElemFechFabr.Text;
                    GrdCrearECont.Columns[0].HeaderText = bO.Equals("GrdContador") ? bT : GrdCrearECont.Columns[0].HeaderText;
                    GrdCrearECont.Columns[1].HeaderText = bO.Equals("GrdVlorAc") ? bT : GrdCrearECont.Columns[1].HeaderText;
                    GrdCrearECont.EmptyDataText = bO.Equals("SinHistorico") ? bT : GrdCrearECont.EmptyDataText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnGuardarInsElemOnCl'");
                foreach (DataRow row in Result)
                {
                    BtnGuardarInsElem.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnGuardarInsMay.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnGuardarInsSubC.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                }

                Result = Idioma.Select("Objeto= 'BtnGuardarRemElemOnCl'");
                foreach (DataRow row in Result)
                {
                    BtnGuardarRemElem.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnGuardarRemMay.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnGuardarRemSubC.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                }
                Result = Idioma.Select("Objeto= 'BtnCompensacionOnCl'");
                foreach (DataRow row in Result)
                {
                    BtnCompensac.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnRemCompensac.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnCompensacInsMay.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                    BtnRemMayCompensac.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                }

                Result = Idioma.Select("Objeto= 'BtnCompensReinicioOnCl'");
                foreach (DataRow row in Result)
                { BtnCompensReinicio.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'BtnCrearElemGuardarOnCl'");
                foreach (DataRow row in Result)
                { BtnCrearElemGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_AeronaveVirtual 29,'','','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "HK";
                                DSTDdl.Tables[1].TableName = "Posc";
                                DSTDdl.Tables[2].TableName = "PNMyr";
                                DSTDdl.Tables[3].TableName = "PNCrearElem";
                                DSTDdl.Tables[4].TableName = "PNConsultaCrearElem";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables[0].Rows.Count > 0)
            {
                DdlAeroInsElem.DataSource = DSTDdl.Tables[0];
                DdlAeroInsElem.DataTextField = "Matricula";
                DdlAeroInsElem.DataValueField = "CodAeronave";
                DdlAeroInsElem.DataBind();

                DdlHkConsAeroVirtual.DataSource = DSTDdl.Tables[0];
                DdlHkConsAeroVirtual.DataTextField = "Matricula";
                DdlHkConsAeroVirtual.DataValueField = "CodAeronave";
                DdlHkConsAeroVirtual.DataBind();

                DdlAeroRemElem.DataSource = DSTDdl.Tables[0];
                DdlAeroRemElem.DataTextField = "Matricula";
                DdlAeroRemElem.DataValueField = "CodAeronave";
                DdlAeroRemElem.DataBind();

                DdlAeroInsMay.DataSource = DSTDdl.Tables[0];
                DdlAeroInsMay.DataTextField = "Matricula";
                DdlAeroInsMay.DataValueField = "CodAeronave";
                DdlAeroInsMay.DataBind();

                DdlAeroRemMay.DataSource = DSTDdl.Tables[0];
                DdlAeroRemMay.DataTextField = "Matricula";
                DdlAeroRemMay.DataValueField = "CodAeronave";
                DdlAeroRemMay.DataBind();

                DdlPosicRemElem.DataSource = DSTDdl.Tables[1];
                DdlPosicRemElem.DataTextField = "Descripcion";
                DdlPosicRemElem.DataValueField = "Codigo";
                DdlPosicRemElem.DataBind();

                DdlPosicRemMay.DataSource = DSTDdl.Tables[1];
                DdlPosicRemElem.DataTextField = "Descripcion";
                DdlPosicRemMay.DataTextField = "Descripcion";
                DdlPosicRemMay.DataValueField = "Codigo";
                DdlPosicRemMay.DataBind();

                DdlPosicRemSubC.DataSource = DSTDdl.Tables[1];
                DdlPosicRemElem.DataTextField = "Descripcion";
                DdlPosicRemSubC.DataTextField = "Descripcion";
                DdlPosicRemSubC.DataValueField = "Codigo";
                DdlPosicRemSubC.DataBind();

                DdlPnVisualMay.DataSource = DSTDdl.Tables[2];
                DdlPnVisualMay.DataTextField = "PN";
                DdlPnVisualMay.DataValueField = "Codigo";
                DdlPnVisualMay.DataBind();

                DdlPNInsSubC.DataSource = DSTDdl.Tables[2];
                DdlPNInsSubC.DataTextField = "PN";
                DdlPNInsSubC.DataValueField = "Codigo";
                DdlPNInsSubC.DataBind();

                DdlPNRemSubC.DataSource = DSTDdl.Tables[2];
                DdlPNRemSubC.DataTextField = "PN";
                DdlPNRemSubC.DataValueField = "Codigo";
                DdlPNRemSubC.DataBind();
            }
        }

        protected void BtnInsMayor_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsMay");
            MultVw.ActiveViewIndex = 5;
            BtnInsMayor.CssClass = "btn btn-primary";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnRemMayor_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemMay");
            MultVw.ActiveViewIndex = 7;
            BtnRemMayor.CssClass = "btn btn-primary";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnInsSubC_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsSub");
            BindDDdlSnVisualMay("", "ISC");
            MultVw.ActiveViewIndex = 8;
            BtnInsSubC.CssClass = "btn btn-primary";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnRemSubC_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemSub");
            BindDDdlSnVisualMay("", "ISC");
            MultVw.ActiveViewIndex = 9;
            BtnRemSubC.CssClass = "btn btn-primary";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void AplicarCssClassBtn()
        {
            BtnInsElem.CssClass = "btn btn-outline-primary";
            BtnRemElem.CssClass = "btn btn-outline-primary";
            BtnInsMayor.CssClass = "btn btn-outline-primary";
            BtnRemMayor.CssClass = "btn btn-outline-primary";
            BtnInsSubC.CssClass = "btn btn-outline-primary";
            BtnRemSubC.CssClass = "btn btn-outline-primary";
        }
        protected void LimparCampoHK(string TipoMov)
        {
            AplicarCssClassBtn();
            ViewState["TieneCompensacion"] = "N";
            switch (TipoMov.Trim())
            {
                case "InsEle":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    DdlPNRemSubC.Text = "";
                    DdlSNRemSubC.Text = "";
                    DdlModelRemSubC.Text = "";
                    break;
                case "RemEle":
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    DdlPNRemSubC.Text = "";
                    DdlSNRemSubC.Text = "";
                    DdlModelRemSubC.Text = "";
                    break;
                case "InsMay":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    DdlPNRemSubC.Text = "";
                    DdlSNRemSubC.Text = "";
                    DdlModelRemSubC.Text = "";
                    break;
                case "RemMay":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    DdlPNRemSubC.Text = "";
                    DdlSNRemSubC.Text = "";
                    DdlModelRemSubC.Text = "";
                    break;
                case "InsSub":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNRemSubC.Text = "";
                    DdlSNRemSubC.Text = "";
                    DdlModelRemSubC.Text = "";
                    break;
                case "RemSub":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    break;
                default:
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    DdlAeroRemMay.Text = "0";
                    BtnRemMayCompensac.Visible = false;
                    DdlPNInsSubC.Text = "";
                    DdlSNInsSubC.Text = "";
                    DdlModelInsSubC.Text = "";
                    GrdSvcInsSubC.DataSource = null;
                    GrdSvcInsSubC.DataBind();
                    break;
            }
        }
        //******************************************  INSTALAR COMPONENTE *********************************************************
        protected void BIndDataBusq()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            GrdSvcInsElem.Visible = false;
            GrdBusq.Visible = true;
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitServicios01'");
            foreach (DataRow row in Result)
            { TxtTitServicios.Text = row["Texto"].ToString().Trim(); }// Elementos disponibles"

            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());

                string VbTxtSql = "", VbOpcion = "";
                VbTxtSql = "EXEC SP_TablasIngenieria 11,@SN, @PN,@UN,'','','','','','CurBusInstalar',@CodHK,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroInsElem.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusq.DataSource = DtB;
                            GrdBusq.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusq.DataSource = null;
                            GrdBusq.DataBind();
                        }
                    }
                }
            }
        }//ok lista de elem que se pueden instalar
        protected void BtnInsElem_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsEle");
            BtnInsElem.CssClass = "btn btn-primary";
            MultVw.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BIndDHisElemInsElem(string CodElem)//OK acumulado de los contadores
        {
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            TxtTitContadores.Text = "S/N: " + TxtSnInsElem.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,@ICC, '01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContInsElem.DataSource = DtB;
                            GrdHisContInsElem.DataBind();
                        }
                        else
                        {
                            GrdHisContInsElem.DataSource = null;
                            GrdHisContInsElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDSvcInsElem(string CodElem, string Modelo, string CodHK) //OK servicios
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitServicios02'");
            foreach (DataRow row in Result)
            { TxtTitServicios.Text = row["Texto"].ToString().Trim(); }// Servicios asignados";
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 4,'NORMAL',@CoEl,@Mo,@CHk,2,@ICC,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@CoEl", CodElem.Trim());
                        SC.Parameters.AddWithValue("@Mo", Modelo.Trim());
                        SC.Parameters.AddWithValue("@CHk", CodHK.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);

                            if (DtB.Rows.Count > 0)
                            {
                                GrdSvcInsElem.DataSource = DtB;
                                GrdSvcInsElem.DataBind();
                            }
                            else
                            {
                                GrdSvcInsElem.DataSource = null;
                                GrdSvcInsElem.DataBind();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string borrar = Ex.ToString();
                    }
                }
            }
        }
        protected void LimpiarCamposInsElem(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroInsElem.Text = "0"; }
            TxtPnInsElem.Text = "";
            TxtSnInsElem.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecInsElem.Text = "";
            DdlPosicInsElem.Text = "";
            TxtFechaInsElem.Text = "";
            TxtMotivInsElem.Text = "";
        }
        protected void BtnPNInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDataBusq();
        }
        protected void BtnSNInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDataBusq();
        }
        protected void BtnUltNivInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDataBusq();
        }
        protected void DdlAeroInsElem_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsElem("");
            BIndDataBusq();
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            DataRow[] Result;
            Result = DSTDdl.Tables[0].Select("CodAeronave= " + DdlAeroInsElem.Text.Trim());
            foreach (DataRow row in Result)
            {
                if (row["Activo"].ToString().Trim().Equals("1")) { BtnGuardarInsElem.Enabled = true; }
                else { BtnGuardarInsElem.Enabled = false; }
            }
        }
        protected void TxtFecUltCumpl_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            var Cntrl = (Control)sender;
            GridViewRow row = (GridViewRow)Cntrl.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtFecUltCumpl = (TextBox)GrdSvcInsElem.Rows[rowIndex].FindControl("TxtFecUltCumpl");

            Cnx.ValidarFechas(TxtFecUltCumpl.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow DR in Result)
                { MensjF = DR["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            ViewState["ValidaFechaSvc"] = "N";
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e) //OK
        {
            LimpiarCamposInsElem("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[11].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[7].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[8].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]');", true);
                return;
            }
            TxtPnInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text.Trim());
            TxtSnInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusq.DataKeys[this.GrdBusq.SelectedIndex][0].ToString();
            TxtUbiTecInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[5].Text.Trim());
            string PoscElem = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[10].Text.Trim());
            BIndDHisElemInsElem(ViewState["CodElemento"].ToString().Trim());
            string borr = ViewState["CodModelo"].ToString().Trim();
            string borr1 = Session["!dC!@"].ToString().Trim();

            string LtxtSql = string.Format("EXEC Consultas_General_Ingenieria 2,'{0}','{1}','',0, 0,{2},'01-01-1','01-01-1'", TxtUbiTecInsElem.Text, ViewState["CodModelo"].ToString().Trim(), Session["!dC!@"]);
            DdlPosicInsElem.DataSource = Cnx.DSET(LtxtSql);
            DdlPosicInsElem.DataMember = "Datos";
            DdlPosicInsElem.DataTextField = "Descripcion";
            DdlPosicInsElem.DataValueField = "Codigo";
            DdlPosicInsElem.DataBind();
            if (PoscElem.Equals("S"))
            { DdlPosicInsElem.Enabled = true; }
            else
            { DdlPosicInsElem.Enabled = false; }
            GrdBusq.Visible = false;
            BIndDSvcInsElem(ViewState["CodElemento"].ToString().Trim(), ViewState["CodModelo"].ToString().Trim(), DdlAeroInsElem.Text);
            GrdSvcInsElem.Visible = true;
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdBusq.PageIndex = e.NewPageIndex; BIndDataBusq(); }
        protected void TxtFechaInsElem_TextChanged(object sender, EventArgs e) // OK Valida si tiene Hojas para compensar
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.ValidarFechas(TxtFechaInsElem.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { MensjF = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            if (!DdlAeroInsElem.Text.Equals("0") && !TxtSnInsElem.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,@UBR,'','','',@CodA,0,0,@ICC,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroInsElem.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaInsElem.Text);
                    SC.Parameters.AddWithValue("@UBR", TxtUbiTecInsElem.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnCompensac.Visible = true; }
                        else
                        { BtnCompensac.Visible = false; }
                    }
                }
            }
        }
        protected void BtnGuardarInsElem_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            try
            {
                if (DdlAeroInsElem.Text.Equals("0"))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens01HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar una aeronave')", true);
                    return;
                }
                if (TxtPnInsElem.Text.Equals("") || TxtSnInsElem.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens02HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un elemento')", true);
                    return;
                }
                if (TxtUbiTecInsElem.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicInsElem.Text.Equals("") && DdlPosicInsElem.Enabled == true)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaInsElem.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha
                    return;
                }
                Cnx.ValidarFechas(TxtFechaInsElem.Text.Trim(), "", 1);
                var MensjF = Cnx.GetMensj();
                if (!MensjF.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { MensjF = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
                if (TxtMotivInsElem.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }
                foreach (GridViewRow Row in GrdSvcInsElem.Rows)
                {

                    string VbFechaAnt = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim(); // obtener indice
                    string TxtFecUltCumpl = (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim();
                    string VbReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim();
                    if (!TxtFecUltCumpl.Equals(VbFechaAnt) && VbReporte.Equals("") && ViewState["ValidaFechaSvc"].Equals("N"))
                    {
                        ViewState["ValidaFechaSvc"] = "S";
                        DataRow[] Result1 = Idioma.Select("Objeto= 'Mens07HkVrt'");
                        foreach (DataRow row in Result1)
                        { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//'Existen servicios en los que se modificaron fechas de cumplimiento y no tienen reporte
                        return;
                    }
                }
                List<ClsTypAeronaveVirtual> ObjInsElemento = new List<ClsTypAeronaveVirtual>();
                var TypInsElemento = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "IC",
                    CodAeronave = Convert.ToInt32(DdlAeroInsElem.Text.Trim()),
                    CodModelo = "",
                    NivelElemento = "C",
                    Motor = "N",
                    UltimoNivel = TxtUbiTecInsElem.Text.Trim(),
                    CodMayor = "",
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnInsElem.Text.Trim(),
                    Sn = TxtSnInsElem.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaInsElem.Text),
                    Posicion = DdlPosicInsElem.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivInsElem.Text.Trim(),
                };
                ObjInsElemento.Add(TypInsElemento);

                List<ClsTypAeronaveVirtual> ObjServcManto = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdSvcInsElem.Rows)
                {
                    string StrUC;
                    double VbUC;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrUC = (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim();
                    VbUC = StrUC.Length == 0 ? 0 : Convert.ToDouble(StrUC, Culture);

                    DateTime? VbFechaVence, VbFechaVenceAnt;
                    string VbFecha = (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim().Equals("") ? null : (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim();
                    if (VbFecha == null)
                    { VbFechaVence = null; }
                    else
                    { VbFechaVence = Convert.ToDateTime(VbFecha); }

                    VbFecha = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim().Equals("") ? null : GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    if (VbFecha == null)
                    { VbFechaVenceAnt = null; }
                    else
                    { VbFechaVenceAnt = Convert.ToDateTime(VbFecha); }
                    string borr = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim();
                    int borrar = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    var TypServcManto = new ClsTypAeronaveVirtual()
                    {
                        CodIdContadorElem = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[1].ToString().Trim()),
                        CodElementoSvc = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[3].ToString().Trim(),
                        FechaVence = VbFechaVence,
                        FechaVenceAnt = VbFechaVenceAnt,
                        Resetear = (Row.FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0,
                        CodOT = (Row.FindControl("LblCodOT") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        CodIdContaSrvManto = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        NumReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim(),
                        ValorUltCump = Convert.ToDouble(VbUC),
                        GeneraHist = (Row.FindControl("CkbGenerarHist") as CheckBox).Checked == true ? "S" : "N",
                    };
                    ObjServcManto.Add(TypServcManto);
                }
                List<ClsTypAeronaveVirtual> ObjCompensacion = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensac = new ClsTypAeronaveVirtual()
                    {
                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "N", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicInsElem.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacion.Add(TypCompensac);
                }
                List<ClsTypAeronaveVirtual> ObjOT = new List<ClsTypAeronaveVirtual>();
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjInsElemento, ObjServcManto, ObjCompensacion, ObjOT);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result1)
                    { 
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true);
                    }

                    
                    return;
                }
                BIndDHisElemInsElem(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposInsElem("TODOS");
                BIndDSvcInsElem("", "", "0");
                ViewState["TieneCompensacion"] = "N";
                GrdSvcInsElem.Visible = false;
                BtnCompensac.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();
                Idioma = (DataTable)ViewState["TablaIdioma"];

                Result = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Proceso exitoso

            }
            catch (Exception Ex)
            {
                Result = Idioma.Select("Objeto= 'Mens33HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Instalar Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroInsElem.Text = "0";
            }
        }
        protected void GrdSvcInsElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string VbFecSt;
            DateTime? VbFecDT;
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbContEsReset = dr["Reseteable"].ToString();
                string VbSvcEsReset = dr["Reseteable"].ToString();
                string VbTieneOT = dr["CodOT"].ToString();
                string TieneHisLV = dr["TieneHisLV"].ToString();
                if (VbContEsReset.Equals("N") || VbSvcEsReset.Equals("0") || !VbTieneOT.Equals(""))
                {
                    CheckBox CkbReset = e.Row.FindControl("CkbReset") as CheckBox;
                    CkbReset.Enabled = false;
                }

                Label LblContador = e.Row.FindControl("LblContador") as Label;
                TextBox TxtCumpHist = e.Row.FindControl("TxtCumpHist") as TextBox;
                CheckBox CkbGenerarHist = e.Row.FindControl("CkbGenerarHist") as CheckBox;
                if (LblContador.Text.Trim().Equals("CAL"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens19HkVrt'");
                    foreach (DataRow row in Result)
                    { TxtCumpHist.ToolTip = row["Texto"].ToString().Trim(); }//"El Contador CAL no genera histórico."                    
                }
                if (TieneHisLV.Equals("S"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens20HkVrt'");
                    foreach (DataRow row in Result)
                    { TxtCumpHist.ToolTip = row["Texto"].ToString().Trim(); }//Tiene hojas procesadas en el histórico de contadores.";
                }

                VbFecSt = dr["FechaVencWeb"].ToString().Trim().Equals("") ? "01/01/1900" : dr["FechaVencWeb"].ToString().Trim();
                VbFecDT = Convert.ToDateTime(VbFecSt);

                if (MultVw.ActiveViewIndex == 8) //Instalar SubComp
                {
                    TextBox TxtFecUltCumplInsSubC = e.Row.FindControl("TxtFecUltCumplInsSubC") as TextBox;
                    TxtFecUltCumplInsSubC.Text = VbFecSt.Trim().Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
                }
                else//Instalar Componente
                {
                    TextBox TxtFecUltCumpl = e.Row.FindControl("TxtFecUltCumpl") as TextBox;
                    TxtFecUltCumpl.Text = VbFecSt.Trim().Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
                }
            }
        }
        //******************************************  Aeronave virtual Consultar SN instaladas *********************************************************
        protected void BtnAKVirtualInsElem_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroInsElem.Text;
            BIndDConsAeroVirtual(DdlAeroInsElem.Text);
        }
        protected void BIndDConsAeroVirtual(string CodHK)
        {
            if (CodHK.Equals("0"))
            { return; }
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_Visualizar_Aeronave_Virtual @CodHK,'','',0,'AERONAVE_VIRTUAL', @ICC");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodHK", CodHK);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdListaAeroVirtual.DataSource = DtB;
                            GrdListaAeroVirtual.DataBind();
                        }
                        else
                        {
                            GrdListaAeroVirtual.DataSource = null;
                            GrdListaAeroVirtual.DataBind();
                        }
                    }
                }
            }
        }
        protected void DdlHkConsAeroVirtual_TextChanged(object sender, EventArgs e)
        {
            BIndDConsAeroVirtual(DdlHkConsAeroVirtual.Text);
        }
        protected void IbtCerrarAeroVirtual_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdListaAeroVirtual_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbMayor = dr["Mayor"].ToString();
                switch (VbMayor)
                {
                    case "1":
                        e.Row.BackColor = System.Drawing.Color.White;
                        break;
                    case "2":
                        e.Row.BackColor = System.Drawing.Color.Wheat;
                        break;
                    case "3":
                        e.Row.BackColor = System.Drawing.Color.LightGray;
                        break;
                    default:
                      //  e.Row.BackColor = System.Drawing.Color.Red;
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        break;
                }
            }/**/
        }
        //******************************************  Compensaciones *********************************************************
        protected void BtnCompensac_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroInsElem.Text, TxtFechaInsElem.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BIndDCompesacLV(string CodHK, string Fecha)

        {
            if (CodHK.Equals("0"))
            { return; }
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 25,'','','','',@CodA,0,0,@ICC,@F,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodA", CodHK);
                    SC.Parameters.AddWithValue("@F", Fecha);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdCompensLv.DataSource = DtB;
                            GrdCompensLv.DataBind();
                        }
                        else
                        {
                            GrdCompensLv.DataSource = null;
                            GrdCompensLv.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtCerrarCompensacion_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdCompensLv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                Button BtnSelect = e.Row.FindControl("BtnSelect") as Button;

                DataRow[] Result = Idioma.Select("Objeto='BtnSelect'");
                foreach (DataRow RowIdioma in Result)
                { BtnSelect.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                Result = Idioma.Select("Objeto= 'BtnSelectOnCl'");
                foreach (DataRow row in Result)
                { BtnSelect.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
            }
        }
        protected void GrdCompensLv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                CheckBox CkbOK = row.FindControl("CkbOK") as CheckBox;
                CkbOK.Checked = true;
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
                ViewState["TieneCompensacion"] = "S";
            }
        }
        protected void BtnCompensReinicio_Click(object sender, EventArgs e)
        {
            ViewState["TieneCompensacion"] = "N";

            switch (ViewState["Ventana"])
            {
                case 0: // Instalar Elem
                    BIndDCompesacLV(DdlAeroInsElem.Text, TxtFechaInsElem.Text);
                    break;
                case 3:// Remoción Elem
                    BIndDCompesacLV(DdlAeroRemElem.Text, TxtFechaRemElem.Text);
                    break;
                case 5:// Instalar Mayor
                    BIndDCompesacLV(DdlAeroInsMay.Text, TxtFechaInsMay.Text);
                    break;
                case 7:// Remoción Mayor
                    BIndDCompesacLV(DdlAeroRemMay.Text, TxtFechaRemMay.Text);
                    break;
            }
            GrdCompensLv.Enabled = true;
            CkbCompensInicioDia.Checked = false;
            CkbCompensInicioDia.Enabled = true;
        }
        protected void CkbCompensInicioDia_CheckedChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result = Idioma.Select("Objeto= 'Mens32HkVrt'");
            foreach (DataRow row in Result)
            { ScriptManager.RegisterClientScriptBlock(this.UplCompensacion, UplCompensacion.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Todos los valores del dia seran tomados por el componente instalado')", true);
            GrdCompensLv.Enabled = false;
            CkbCompensInicioDia.Enabled = false;
            ViewState["TieneCompensacion"] = "S";
        }
        //******************************************  REMOCION COMPONENTE *********************************************************
        protected void BIndDRemBusqElem()
        {
            if (DdlAeroRemElem.Text.Equals("0"))
            { return; }
            GrdRemBusqElem.Visible = true;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "", VbOpcion = "";
                //VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 26,@SN,@PN,@UN,'',@CodHK,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                VbTxtSql = "EXEC SP_TablasIngenieria 12, @SN, @PN, @UN,'','','','','','CurBusInstalar',@CodHK,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroRemElem.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdRemBusqElem.DataSource = DtB;
                            GrdRemBusqElem.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdRemBusqElem.DataSource = null;
                            GrdRemBusqElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisContRemElem(string CodElem)
        {
            if (DdlAeroRemElem.Text.Equals("0"))
            { return; }
            TxtTitRemContadores.Text = "S/N: " + TxtSnRemElem.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContRemElem.DataSource = DtB;
                            GrdHisContRemElem.DataBind();
                        }
                        else
                        {
                            GrdHisContRemElem.DataSource = null;
                            GrdHisContRemElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void LimpiarCamposRemElem(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroRemElem.Text = "0"; }
            TxtPnRemElem.Text = "";
            TxtSnRemElem.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecRemElem.Text = "";
            DdlPosicRemElem.Text = "";
            TxtFechaRemElem.Text = "";
            TxtMotivRemElem.Text = "";
        }
        protected void DdlAeroRemElem_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposRemElem("");
            BIndDRemBusqElem();
        }
        protected void BtnRemElem_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemEle");
            MultVw.ActiveViewIndex = 3;
            BtnRemElem.CssClass = "btn btn-primary";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnPNRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDRemBusqElem();
        }
        protected void BtnSNRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDRemBusqElem();
        }
        protected void BtnUltNivRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDRemBusqElem();
        }
        protected void BtnAKVirtualRemElem_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroRemElem.Text;
            BIndDConsAeroVirtual(DdlAeroRemElem.Text);
        }
        protected void BtnGuardarRemElem_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlAeroRemElem.Text.Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens01HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar una aeronave')", true);
                    return;
                }
                if (TxtPnRemElem.Text.Equals("") || TxtSnRemElem.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens02HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar un elemento')", true);
                    return;
                }
                if (TxtUbiTecRemElem.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicRemElem.Text.Equals("") && DdlPosicInsElem.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaRemElem.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha')", true);
                    return;
                }
                Cnx.ValidarFechas(TxtFechaRemElem.Text.Trim(), "", 1);
                var MensjF = Cnx.GetMensj();
                if (!MensjF.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { MensjF = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
                if (TxtMotivRemElem.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }

                List<ClsTypAeronaveVirtual> ObjRemElemento = new List<ClsTypAeronaveVirtual>();
                var TypRemElemento = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "RC",
                    CodAeronave = Convert.ToInt32(DdlAeroRemElem.Text.Trim()),
                    CodModelo = "",
                    NivelElemento = "C",
                    Motor = "N",
                    UltimoNivel = TxtUbiTecRemElem.Text.Trim(),
                    CodMayor = "",
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnRemElem.Text.Trim(),
                    Sn = TxtSnRemElem.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaRemElem.Text),
                    Posicion = DdlPosicRemElem.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivRemElem.Text.Trim(),
                };
                ObjRemElemento.Add(TypRemElemento);

                List<ClsTypAeronaveVirtual> ObjServcManto = new List<ClsTypAeronaveVirtual>();

                List<ClsTypAeronaveVirtual> ObjCompensacion = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensac = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "N", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicRemElem.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacion.Add(TypCompensac);
                }

                List<ClsTypAeronaveVirtual> ObjOT = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdOtCerrar.Rows)
                {
                    DateTime? VbFechaI;
                    string VbCcosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    string VbFIText = GrdOtCerrar.DataKeys[Row.RowIndex].Values[1].ToString().Trim();
                    if (VbFIText.Equals("")) { VbFechaI = Convert.ToDateTime(TxtFechaRemElem.Text); }
                    else { VbFechaI = Convert.ToDateTime(VbFIText); }

                    var TypOT = new ClsTypAeronaveVirtual()
                    {
                        CodNumOrdenTrab = Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        Descripcion = "",
                        CodEstOrdTrab1 = "0002",
                        CodEstOrdTrab2 = "",
                        Aplicabilidad = TxtSnRemElem.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = "",
                        CodBase = "",
                        CodTaller = "",
                        CodPlanManto = "",
                        CentroCosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim(),
                        FechaInicio = VbFechaI,
                        FechaFinal = Convert.ToDateTime(TxtFechaRemElem.Text),
                        FechaReg = Convert.ToDateTime((Row.FindControl("LblFechaReg") as Label).Text.Trim()),
                        IdentificadorCorrPrev = 1,
                        CodPrioridad = "",
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = 1,
                        HorasProyectadas = 0,
                        FechaProyectada = null,
                        FechaVencimiento = null,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = (Row.FindControl("TxtMotivo") as TextBox).Text.Trim(),
                        CodTipoCodigo = "",
                        CodInspectorCierre = "",
                        LicenciaInspCierre = "",
                        PNOT = TxtPnRemElem.Text.Trim(),
                        BloquearDetalle = 0,
                        CodResponsable = Session["C77U"].ToString(),
                        OTSN = 0,
                        OTSO = 0,
                        OTSR = 0,
                        OCSN = 0,
                        OCSO = 0,
                        OCSR = 0,
                        EjecPasos = Convert.ToInt32(GrdOtCerrar.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        CancelOT = 0,
                        WS = "",
                        OKOT = (Row.FindControl("CkbOk") as CheckBox).Checked == true ? 1 : 0,
                        AccionOT = "",
                    };
                    ObjOT.Add(TypOT);
                }
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjRemElemento, ObjServcManto, ObjCompensacion, ObjOT);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDHisContRemElem(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposRemElem("TODOS");
                ViewState["TieneCompensacion"] = "N";
                BtnAbrirOTCerrar.Visible = false;
                BtnRemCompensac.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();
                DataRow[] Result1 = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result1)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                DataRow[] Result1 = Idioma.Select("Objeto= 'Mens25HkVrt'");
                foreach (DataRow row in Result1)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Remover Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroRemElem.Text = "0";
            }
        }
        protected void TxtFechaRemElem_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.ValidarFechas(TxtFechaRemElem.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { MensjF = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            if (!DdlAeroRemElem.Text.Equals("0") && !TxtSnRemElem.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,'','','','',@CodA,0,0,@ICC,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroRemElem.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaRemElem.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnRemCompensac.Visible = true; }
                        else
                        { BtnRemCompensac.Visible = false; }
                    }
                }
            }
        }
        protected void BtnRemCompensac_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroRemElem.Text, TxtFechaRemElem.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdRemBusqElem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            LimpiarCamposRemElem("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[12].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[8].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[9].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens21HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]
                return;
            }
            TxtPnRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[1].Text.Trim());
            TxtSnRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdRemBusqElem.DataKeys[this.GrdRemBusqElem.SelectedIndex][0].ToString();
            TxtUbiTecRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[5].Text.Trim());
            string vbPos = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[7].Text.Trim());
            DdlPosicRemElem.Text = vbPos.Trim();
            BIndDHisContRemElem(ViewState["CodElemento"].ToString().Trim());
            BIndDOCerrarOT(ViewState["CodElemento"].ToString().Trim(), "C");
            GrdRemBusqElem.Visible = false;
        }
        protected void GrdRemBusqElem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdRemBusqElem.PageIndex = e.NewPageIndex;
            BIndDRemBusqElem();
        }
        protected void GrdRemBusqElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }/**/
        }
        //******************************************  OT para cerrar *********************************************************
        protected void BIndDOCerrarOT(string CodElem, string TipoElem)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 21,@CE,'','',@TE, 0,0,0,@ICC,'01-01-01','01-01-01','01-01-01'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CE", CodElem);
                    SC.Parameters.AddWithValue("@TE", TipoElem);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdOtCerrar.DataSource = DtB;
                            GrdOtCerrar.DataBind();
                            switch (TipoElem)
                            {
                                case "C":
                                    BtnAbrirOTCerrar.Visible = true;
                                    Idioma = (DataTable)ViewState["TablaIdioma"];

                                    DataRow[] Result = Idioma.Select("Objeto= 'Mens22HkVrt'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// El componente tiene ordenes de trabajo abierta(s).
                                    break;
                                case "M":
                                    BtnAbrirOTCerrarRemMay.Visible = true;
                                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens23HkVrt'");
                                    foreach (DataRow row in Result1)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// El mayor tiene ordenes de trabajo abierta(s).
                                    break;
                                default:
                                    BtnAbrirOTCerrarRemSubC.Visible = true;
                                    DataRow[] Result2 = Idioma.Select("Objeto= 'Mens24HkVrt'");
                                    foreach (DataRow row in Result2)
                                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// El sub-componente tiene ordenes de trabajo abierta(s).
                                    break;
                            }
                        }
                        else
                        {
                            GrdOtCerrar.DataSource = null;
                            GrdOtCerrar.DataBind();
                            switch (CodElem)
                            {
                                case "C":
                                    BtnAbrirOTCerrar.Visible = false;
                                    break;
                                case "M":
                                    BtnAbrirOTCerrarRemMay.Visible = false;
                                    break;
                                default:
                                    BtnAbrirOTCerrarRemSubC.Visible = false;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        protected void BtnAbrirOTCerrar_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 4;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtCerrarOTcierre_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
            Page.Title = ViewState["PageTit"].ToString();
        }
        //******************************************  INSTALAR MAYOR *********************************************************
        protected void BIndDBusqInsMay()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            GrdSvcInsMay.Visible = false;
            GrdBusqMayDisp.Visible = true;
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitSvcInsMay01'");
            foreach (DataRow row in Result)
            { TxtTitSvcInsMay.Text = row["Texto"].ToString().Trim(); }// Mayores Disponibles
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "", VbOpcion = "";
                VbTxtSql = "EXEC SP_TablasIngenieria 11,@SN, @PN,@UN,'M','','','','','CurBusInstalar',@CodHK,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroInsMay.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusqMayDisp.DataSource = DtB;
                            GrdBusqMayDisp.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusqMayDisp.DataSource = null;
                            GrdBusqMayDisp.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisElemInsMay(string CodElem)
        {
            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            TxtTitHisContInsMay.Text = "S/N: " + TxtSnInsMay.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContInsMay.DataSource = DtB;
                            GrdHisContInsMay.DataBind();
                        }
                        else
                        {
                            GrdHisContInsMay.DataSource = null;
                            GrdHisContInsMay.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDSvcInsMay(string CodElem, string Modelo, string CodHK)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitServicios02'");
            foreach (DataRow row in Result)
            { TxtTitSvcInsMay.Text = row["Texto"].ToString().Trim(); }//Servicios Asignados
            DataTable DTM = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 4,'MAYOR',@CoEl,@Mo,@CHk,2,@ICC,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@CoEl", CodElem.Trim());
                        SC.Parameters.AddWithValue("@Mo", Modelo.Trim());
                        SC.Parameters.AddWithValue("@CHk", CodHK.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DTM);

                            if (DTM.Rows.Count > 0)
                            {
                                GrdSvcInsMay.DataSource = DTM;
                                GrdSvcInsMay.DataBind();
                            }
                            else
                            {
                                GrdSvcInsMay.DataSource = null;
                                GrdSvcInsMay.DataBind();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string borrar = Ex.ToString();
                    }
                }
            }
        }
        protected void LimpiarCamposInsMay(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroInsMay.Text = "0"; }
            TxtPnInsMay.Text = "";
            TxtSnInsMay.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            ViewState["EsMotor"] = "N";
            TxtUbiTecInsMay.Text = "";
            DdlPosicInsMay.Text = "";
            TxtFechaInsMay.Text = "";
            TxtMotivInsMay.Text = "";
        }
        protected void DdlAeroInsMay_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsMay("");
            BIndDBusqInsMay();
        }
        protected void BtnPNInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDBusqInsMay();
        }
        protected void BtnSNInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDBusqInsMay();
        }
        protected void BtnUltNivInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDBusqInsMay();
        }
        protected void BtnAKVirtualInsMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroInsMay.Text;
            BIndDConsAeroVirtual(DdlAeroInsMay.Text);
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnGuardarInsMay_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlAeroInsMay.Text.Equals("0"))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens01HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar una aeronave
                    return;
                }
                if (TxtPnInsMay.Text.Equals("") || TxtSnInsMay.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens02HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un elemento
                    return;
                }
                if (TxtUbiTecInsMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica
                    return;
                }
                if (DdlPosicInsMay.Text.Equals("") && DdlPosicInsMay.Enabled == true)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición
                    return;
                }
                Cnx.ValidarFechas(TxtFechaInsMay.Text.Trim(), "", 1);
                var MensjF = Cnx.GetMensj();
                if (!MensjF.ToString().Trim().Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                    foreach (DataRow row in Result1)
                    { MensjF = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
                if (TxtFechaInsMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha
                    return;
                }
                if (TxtMotivInsMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }
                foreach (GridViewRow Row in GrdSvcInsMay.Rows)
                {

                    string VbFechaAnt = GrdSvcInsMay.DataKeys[Row.RowIndex].Values[0].ToString().Trim(); // obtener indice
                    string TxtFecUltCumplMay = (Row.FindControl("TxtFecUltCumplMay") as TextBox).Text.Trim();
                    string VbReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim();
                    if (!TxtFecUltCumplMay.Equals(VbFechaAnt) && VbReporte.Equals("") && ViewState["ValidaFechaSvc"].Equals("N"))
                    {
                        ViewState["ValidaFechaSvc"] = "S";
                        DataRow[] Result1 = Idioma.Select("Objeto= 'Mens07HkVrt'");
                        foreach (DataRow row in Result1)
                        { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Existen servicios en los que se modificaron fechas de cumplimiento y no tienen reporte
                        return;
                    }
                }

                List<ClsTypAeronaveVirtual> ObjInsElemento = new List<ClsTypAeronaveVirtual>();
                var TypInsElemento = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "IM",
                    CodAeronave = Convert.ToInt32(DdlAeroInsMay.Text.Trim()),
                    CodModelo = "",
                    NivelElemento = "M",
                    Motor = ViewState["EsMotor"].ToString(),
                    UltimoNivel = TxtUbiTecInsMay.Text.Trim(),
                    CodMayor = ViewState["CodElemento"].ToString().Trim(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnInsMay.Text.Trim(),
                    Sn = TxtSnInsMay.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaInsMay.Text),
                    Posicion = DdlPosicInsMay.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivInsMay.Text.Trim(),
                };
                ObjInsElemento.Add(TypInsElemento);

                List<ClsTypAeronaveVirtual> ObjServcManto = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdSvcInsMay.Rows)
                {
                    string StrUC;
                    double VbUC;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrUC = (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim();
                    VbUC = StrUC.Length == 0 ? 0 : Convert.ToDouble(StrUC, Culture);

                    DateTime? VbFechaVence, VbFechaVenceAnt;
                    string VbFecha = (Row.FindControl("TxtFecUltCumplMay") as TextBox).Text.Trim().Equals("") ? null : (Row.FindControl("TxtFecUltCumplMay") as TextBox).Text.Trim();
                    if (VbFecha == null)
                    { VbFechaVence = null; }
                    else
                    { VbFechaVence = Convert.ToDateTime(VbFecha); }

                    VbFecha = GrdSvcInsMay.DataKeys[Row.RowIndex].Values[0].ToString().Trim().Equals("") ? null : GrdSvcInsMay.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    if (VbFecha == null)
                    { VbFechaVenceAnt = null; }
                    else
                    { VbFechaVenceAnt = Convert.ToDateTime(VbFecha); }
                    string borr = GrdSvcInsMay.DataKeys[Row.RowIndex].Values[2].ToString().Trim();
                    int borrar = Convert.ToInt32(GrdSvcInsMay.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    var TypServcManto = new ClsTypAeronaveVirtual()
                    {
                        CodIdContadorElem = Convert.ToInt32(GrdSvcInsMay.DataKeys[Row.RowIndex].Values[1].ToString().Trim()),
                        CodElementoSvc = GrdSvcInsMay.DataKeys[Row.RowIndex].Values[3].ToString().Trim(),
                        FechaVence = VbFechaVence,
                        FechaVenceAnt = VbFechaVenceAnt,
                        Resetear = (Row.FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0,
                        CodOT = (Row.FindControl("LblCodOT") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        CodIdContaSrvManto = Convert.ToInt32(GrdSvcInsMay.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        NumReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim(),
                        ValorUltCump = Convert.ToDouble(VbUC),
                        GeneraHist = (Row.FindControl("CkbGenerarHist") as CheckBox).Checked == true ? "S" : "N",
                    };
                    ObjServcManto.Add(TypServcManto);
                }

                List<ClsTypAeronaveVirtual> ObjCompensacion = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensac = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "M", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicInsMay.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacion.Add(TypCompensac);
                }
                List<ClsTypAeronaveVirtual> ObjOT = new List<ClsTypAeronaveVirtual>();
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjInsElemento, ObjServcManto, ObjCompensacion, ObjOT);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result1)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDHisElemInsMay(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposInsMay("TODOS");
                BIndDSvcInsMay("", "", "0");
                ViewState["TieneCompensacion"] = "N";
                GrdSvcInsMay.Visible = false;
                BtnCompensacInsMay.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();
                DataRow[] Result = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Proceso exitoso
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens33HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Instalar MAYOR", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroInsMay.Text = "0";
            }
        }
        protected void TxtFechaInsMay_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.ValidarFechas(TxtFechaInsMay.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { MensjF = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            if (!DdlAeroInsMay.Text.Equals("0") && !TxtSnInsMay.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,@UBR,'','','',@CodA,0,0,@ICC,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroInsMay.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaInsMay.Text);
                    SC.Parameters.AddWithValue("@UBR", TxtUbiTecInsMay.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnCompensacInsMay.Visible = true; }
                        else
                        { BtnCompensacInsMay.Visible = false; }
                    }
                }
            }
        }
        protected void DdlPosicInsMay_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["EsMotor"].Equals("S"))
            {
                if (DdlPosicInsMay.Text.Trim().Equals("1") || DdlPosicInsMay.Text.Trim().Equals("2") || DdlPosicInsMay.Text.Trim().Equals("3") || DdlPosicInsMay.Text.Trim().Equals("4"))
                { }
                else
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens34HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// La posición de un motor debe ser 1, 2, 3 ó 4
                    DdlPosicInsMay.Text = "";
                }
            }
        }
        protected void BtnCompensacInsMay_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroInsMay.Text, TxtFechaInsMay.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdBusqMayDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsMay("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[11].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[7].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[8].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "alert", "alert('La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]');", true);
                return;
            }
            TxtPnInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[1].Text.Trim());
            TxtSnInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusqMayDisp.DataKeys[this.GrdBusqMayDisp.SelectedIndex][0].ToString();
            ViewState["EsMotor"] = GrdBusqMayDisp.DataKeys[this.GrdBusqMayDisp.SelectedIndex][1].ToString();
            TxtUbiTecInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[5].Text.Trim());
            string PoscElem = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[10].Text.Trim());
            BIndDHisElemInsMay(ViewState["CodElemento"].ToString().Trim());
            string LtxtSql = string.Format("EXEC Consultas_General_Ingenieria 2,'{0}','{1}','',0, 0,{2},'01-01-1','01-01-1'", TxtUbiTecInsMay.Text, ViewState["CodModelo"].ToString().Trim(), Session["!dC!@"]);
            DdlPosicInsMay.DataSource = Cnx.DSET(LtxtSql);
            DdlPosicInsMay.DataMember = "Datos";
            DdlPosicInsMay.DataTextField = "Descripcion";
            DdlPosicInsMay.DataValueField = "Codigo";
            DdlPosicInsMay.DataBind();
            if (PoscElem.Equals("S"))
            { DdlPosicInsMay.Enabled = true; }
            else
            { DdlPosicInsMay.Enabled = false; } /**/
            GrdBusqMayDisp.Visible = false;
            BIndDSvcInsMay(ViewState["CodElemento"].ToString().Trim(), ViewState["CodModelo"].ToString().Trim(), DdlAeroInsMay.Text);
            GrdSvcInsMay.Visible = true;
        }
        protected void GrdBusqMayDisp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusqMayDisp.PageIndex = e.NewPageIndex;
            BIndDBusqInsMay();
        }
        protected void GrdBusqMayDisp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }
        }
        protected void GrdSvcInsMay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string VbFecSt;
            DateTime? VbFecDT;

            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CalendarExtender CalFecUltCumplMay = e.Row.FindControl("CalFecUltCumplMay") as CalendarExtender;
                //CalFecUltCumplMay.EndDate = DateTime.Now;

                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbContEsReset = dr["Reseteable"].ToString();
                string VbSvcEsReset = dr["Reseteable"].ToString();
                string VbTieneOT = dr["CodOT"].ToString();
                string TieneHisLV = dr["TieneHisLV"].ToString();
                if (VbContEsReset.Equals("N") || VbSvcEsReset.Equals("0") || !VbTieneOT.Equals(""))
                {
                    CheckBox CkbReset = e.Row.FindControl("CkbReset") as CheckBox;
                    CkbReset.Enabled = false;
                }

                Label LblContador = e.Row.FindControl("LblContador") as Label;
                TextBox TxtCumpHist = e.Row.FindControl("TxtCumpHist") as TextBox;
                CheckBox CkbGenerarHist = e.Row.FindControl("CkbGenerarHist") as CheckBox;
                if (LblContador.Text.Trim().Equals("CAL"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens19HkVrt'");
                    foreach (DataRow row in Result)
                    { TxtCumpHist.ToolTip = row["Texto"].ToString().Trim(); }//El Contador CAL no genera histórico.
                }
                if (TieneHisLV.Equals("S"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens20HkVrt'");
                    foreach (DataRow row in Result)
                    { TxtCumpHist.ToolTip = row["Texto"].ToString().Trim(); }//Tiene hojas procesadas en el histórico de contadores.
                }

                TextBox TxtFecUltCumplMay = e.Row.FindControl("TxtFecUltCumplMay") as TextBox;
                VbFecSt = dr["FechaVencWeb"].ToString().Trim().Equals("") ? "01/01/1900" : dr["FechaVencWeb"].ToString().Trim();
                VbFecDT = Convert.ToDateTime(VbFecSt);
                TxtFecUltCumplMay.Text = VbFecSt.Trim().Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
            }
        }
        protected void TxtFecUltCumplMay_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            var Cntrl = (Control)sender;
            GridViewRow row = (GridViewRow)Cntrl.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtFecUltCumplMay = (TextBox)GrdSvcInsMay.Rows[rowIndex].FindControl("TxtFecUltCumplMay");

            Cnx.ValidarFechas(TxtFecUltCumplMay.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow DR in Result)
                { MensjF = DR["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            ViewState["ValidaFechaSvc"] = "N";
        }
        //******************************************  VISUALIZAR MAYORES Y SUB-COMPONENTES *********************************************************
        protected void BindDDdlSnVisualMay(string PN, string Origen)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 22,'{0}','','','SNVisMy',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", PN, Session["!dC!@"]);
            if (Origen.Equals("VM"))
            {
                DdlSnVisualMay.DataSource = Cnx.DSET(LtxtSql);
                DdlSnVisualMay.DataMember = "Datos";
                DdlSnVisualMay.DataTextField = "SN";
                DdlSnVisualMay.DataValueField = "Codigo";
                DdlSnVisualMay.DataBind();
            }
            if (Origen.Equals("ISC"))
            {
                DdlSNInsSubC.DataSource = Cnx.DSET(LtxtSql);
                DdlSNInsSubC.DataMember = "Datos";
                DdlSNInsSubC.DataTextField = "SN";
                DdlSNInsSubC.DataValueField = "Codigo";
                DdlSNInsSubC.DataBind();
            }
            if (Origen.Equals("RSC"))
            {
                DdlSNRemSubC.DataSource = Cnx.DSET(LtxtSql);
                DdlSNRemSubC.DataMember = "Datos";
                DdlSNRemSubC.DataTextField = "SN";
                DdlSNRemSubC.DataValueField = "Codigo";
                DdlSNRemSubC.DataBind();
            }
        }
        protected void BIndDVisualMay(string PN, string SN)
        {
            if (PN.Equals("") || SN.Equals(""))
            { return; }
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 27,@P,@S,'','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@P", PN);
                    SC.Parameters.AddWithValue("@S", SN);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdVisualMay.DataSource = DtB;
                            GrdVisualMay.DataBind();
                        }
                        else
                        {
                            GrdVisualMay.DataSource = null;
                            GrdVisualMay.DataBind();
                        }
                    }
                }
            }
        }
        protected void BtnVisualizarMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 6;
            DdlPnVisualMay.Text = TxtPnInsMay.Text.Trim();
            BindDDdlSnVisualMay(TxtPnInsMay.Text.Trim(), "VM");
            DdlSnVisualMay.Text = ViewState["CodElemento"].ToString().Trim();
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtCerrarVisualMay_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void DdlPnVisualMay_TextChanged(object sender, EventArgs e)
        {
            BindDDdlSnVisualMay(DdlPnVisualMay.Text.Trim(), "VM");
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
        }
        protected void DdlSnVisualMay_TextChanged(object sender, EventArgs e)
        {
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
        }
        protected void GrdVisualMay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                /* e.Row.Cells[6].Visible = false;
                 e.Row.Cells[7].Visible = false;
                 e.Row.Cells[8].Visible = false;*/
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                /*e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;*/
                DataRowView dr = e.Row.DataItem as DataRowView;
                int VbMayor = Convert.ToInt32(dr["ComponenteMayor"].ToString());
                switch (VbMayor)
                {
                    case 1:
                        e.Row.BackColor = System.Drawing.Color.Yellow;
                        break;
                    default:
                        e.Row.BackColor = System.Drawing.Color.Silver;
                        break;
                }
            }
        }
        //******************************************  REMOCION MAYOR *********************************************************
        protected void BIndDBusqRemMay()
        {
            if (DdlAeroRemMay.Text.Equals("0"))
            { return; }
            GrdBusqRemMay.Visible = true;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "", VbOpcion = "";
                // VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 26,@SN,@PN,@UN,'',@CodHK,1,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                VbTxtSql = "EXEC SP_TablasIngenieria 12, @SN, @PN, @UN,'','','','','','CurBusInstalar',@CodHK,1,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtRemMayBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtRemMayBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtRemMayBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroRemMay.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusqRemMay.DataSource = DtB;
                            GrdBusqRemMay.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusqRemMay.DataSource = null;
                            GrdBusqRemMay.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisContRemMay(string CodElem)
        {
            if (DdlAeroRemMay.Text.Equals("0"))
            { return; }
            TxtTitRemMayContadores.Text = "S/N: " + TxtSnRemMay.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContRemMay.DataSource = DtB;
                            GrdHisContRemMay.DataBind();
                        }
                        else
                        {
                            GrdHisContRemMay.DataSource = null;
                            GrdHisContRemMay.DataBind();
                        }
                    }
                }
            }
        }
        protected void LimpiarCamposRemMay(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroRemMay.Text = "0"; }
            TxtPnRemMay.Text = "";
            TxtSnRemMay.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecRemMay.Text = "";
            DdlPosicRemMay.Text = "";
            TxtFechaRemMay.Text = "";
            TxtMotivRemMay.Text = "";
        }
        protected void DdlAeroRemMay_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposRemMay("");
            BIndDBusqRemMay();
        }
        protected void BtnPNRemMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDBusqRemMay();
        }
        protected void BtnSNRemMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDBusqRemMay();
        }
        protected void BtnUltNivRemMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDBusqRemMay();
        }
        protected void BtnAKVirtualRemMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroRemMay.Text;
            BIndDConsAeroVirtual(DdlAeroRemMay.Text);
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnVisualizarRemMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 6;
            DdlPnVisualMay.Text = TxtPnRemMay.Text.Trim();
            BindDDdlSnVisualMay(TxtPnRemMay.Text.Trim(), "VM");
            DdlSnVisualMay.Text = ViewState["CodElemento"].ToString().Trim();
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnAbrirOTCerrarRemMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 4;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnGuardarRemMay_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlAeroRemMay.Text.Equals("0"))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens01HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar una aeronave')", true);
                    return;
                }
                if (TxtPnRemMay.Text.Equals("") || TxtSnRemMay.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens35HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un componente mayor')", true);
                    return;
                }
                if (TxtUbiTecRemMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicRemMay.Text.Equals("") && DdlPosicRemMay.Enabled == true)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaRemMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha')", true);
                    return;
                }
                if (TxtMotivRemMay.Text.Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }
                Cnx.ValidarFechas(TxtFechaRemMay.Text.Trim(), "", 1);
                var MensjF = Cnx.GetMensj();
                if (!MensjF.ToString().Trim().Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                    foreach (DataRow row in Result1)
                    { MensjF = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
                List<ClsTypAeronaveVirtual> ObjRemMayor = new List<ClsTypAeronaveVirtual>();
                var TypRemMayor = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "RM",
                    CodAeronave = Convert.ToInt32(DdlAeroRemMay.Text.Trim()),
                    CodModelo = "",
                    NivelElemento = "M",
                    Motor = ViewState["EsMotor"].ToString(),
                    UltimoNivel = TxtUbiTecRemMay.Text.Trim(),
                    CodMayor = ViewState["CodElemento"].ToString().Trim(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnRemMay.Text.Trim(),
                    Sn = TxtSnRemMay.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaRemMay.Text),
                    Posicion = DdlPosicRemMay.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivRemMay.Text.Trim(),
                };
                ObjRemMayor.Add(TypRemMayor);

                List<ClsTypAeronaveVirtual> ObjServcMantoMay = new List<ClsTypAeronaveVirtual>();

                List<ClsTypAeronaveVirtual> ObjCompensacionMay = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensacMay = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "M", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicRemMay.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacionMay.Add(TypCompensacMay);
                }

                List<ClsTypAeronaveVirtual> ObjOTMay = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdOtCerrar.Rows)
                {
                    DateTime? VbFechaI;
                    string VbCcosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    string VbFIText = GrdOtCerrar.DataKeys[Row.RowIndex].Values[1].ToString().Trim();
                    if (VbFIText.Equals("")) { VbFechaI = Convert.ToDateTime(TxtFechaRemMay.Text); }
                    else { VbFechaI = Convert.ToDateTime(VbFIText); }

                    var TypOTMay = new ClsTypAeronaveVirtual()
                    {
                        CodNumOrdenTrab = Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        Descripcion = "",
                        CodEstOrdTrab1 = "0002",
                        CodEstOrdTrab2 = "",
                        Aplicabilidad = TxtSnRemMay.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = "",
                        CodBase = "",
                        CodTaller = "",
                        CodPlanManto = "",
                        CentroCosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim(),
                        FechaInicio = VbFechaI,
                        FechaFinal = Convert.ToDateTime(TxtFechaRemMay.Text),
                        FechaReg = Convert.ToDateTime((Row.FindControl("LblFechaReg") as Label).Text.Trim()),
                        IdentificadorCorrPrev = 1,
                        CodPrioridad = "",
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = 1,
                        HorasProyectadas = 0,
                        FechaProyectada = null,
                        FechaVencimiento = null,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = (Row.FindControl("TxtMotivo") as TextBox).Text.Trim(),
                        CodTipoCodigo = "",
                        CodInspectorCierre = "",
                        LicenciaInspCierre = "",
                        PNOT = TxtPnRemMay.Text.Trim(),
                        BloquearDetalle = 0,
                        CodResponsable = Session["C77U"].ToString(),
                        OTSN = 0,
                        OTSO = 0,
                        OTSR = 0,
                        OCSN = 0,
                        OCSO = 0,
                        OCSR = 0,
                        EjecPasos = Convert.ToInt32(GrdOtCerrar.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        CancelOT = 0,
                        WS = "",
                        OKOT = (Row.FindControl("CkbOk") as CheckBox).Checked == true ? 1 : 0,
                        AccionOT = "",
                    };
                    ObjOTMay.Add(TypOTMay);
                }
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjRemMayor, ObjServcMantoMay, ObjCompensacionMay, ObjOTMay);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result1)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDHisContRemMay(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposRemMay("TODOS");
                ViewState["TieneCompensacion"] = "N";
                BtnAbrirOTCerrarRemMay.Visible = false;
                BtnRemMayCompensac.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();
                DataRow[] Result = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens25HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Inconveniente en la remoción')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Remover Mayor", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroRemMay.Text = "0";
            }
        }
        protected void TxtFechaRemMay_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.ValidarFechas(TxtFechaRemMay.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { MensjF = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            if (!DdlAeroRemMay.Text.Equals("0") && !TxtSnRemMay.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,'','','','',@CodA,0,0,@ICC,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroRemMay.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaRemMay.Text);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnRemMayCompensac.Visible = true; }
                        else
                        { BtnRemMayCompensac.Visible = false; }
                    }
                }
            }
        }
        protected void BtnRemMayCompensac_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroRemMay.Text, TxtFechaRemMay.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdBusqRemMay_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            LimpiarCamposRemMay("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[12].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[8].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[9].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens21HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemMay, UplRemMay.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Ele
                return;
            }
            TxtPnRemMay.Text = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[1].Text.Trim());
            TxtSnRemMay.Text = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusqRemMay.DataKeys[this.GrdBusqRemMay.SelectedIndex][0].ToString();
            TxtUbiTecRemMay.Text = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[5].Text.Trim());
            string vbPos = HttpUtility.HtmlDecode(GrdBusqRemMay.SelectedRow.Cells[7].Text.Trim());
            DdlPosicRemMay.Text = vbPos.Trim();
            BIndDHisContRemMay(ViewState["CodElemento"].ToString().Trim());
            BIndDOCerrarOT(ViewState["CodElemento"].ToString().Trim(), "M");
            GrdBusqRemMay.Visible = false;
        }
        protected void GrdBusqRemMay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusqRemMay.PageIndex = e.NewPageIndex;
            BIndDBusqRemMay();
        }
        protected void GrdBusqRemMay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }
        }
        //******************************************  INSTALAR SUB-COMPONENTE *********************************************************
        protected void BIndDBusqInsSubC()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlModelInsSubC.Text.Equals(""))
            { return; }
            GrdSvcInsSubC.Visible = false;
            GrdBusqInsSubC.Visible = true;
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitServcInsSubC01'");
            foreach (DataRow row in Result)
            { TxtTitServcInsSubC.Text = row["Texto"].ToString().Trim(); }//"Sub-Componentes Disponibles
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "";
                VbTxtSql = "EXEC SP_TablasIngenieria 11,@SN, @PN,@UN,@Md,'','','','','CurBusInstalar',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqInsSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqInsSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UT") ? TxtBusqInsSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Md", "S" + DdlModelInsSubC.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        { GrdBusqInsSubC.DataSource = DtB; ViewState["ValidaFechaSvc"] = "S"; }
                        else
                        { GrdBusqInsSubC.DataSource = null; }
                        GrdBusqInsSubC.DataBind();
                    }
                }
            }
        }
        protected void BIndDHisElemInsSubC(string CodElem)
        {
            if (DdlModelInsSubC.Text.Equals(""))
            { return; }
            TxtTitContadoresInsSubC.Text = "S/N: " + TxtSnInsSubC.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContInsSubC.DataSource = DtB;
                            GrdHisContInsSubC.DataBind();
                        }
                        else
                        {
                            GrdHisContInsSubC.DataSource = null;
                            GrdHisContInsSubC.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDSvcInsSubC(string CodElem, string Modelo, string CodHK)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Modelo.Equals(""))
            { return; }
            DataRow[] Result = Idioma.Select("Objeto= 'TxtTitServicios02'");
            foreach (DataRow row in Result)
            { TxtTitServcInsSubC.Text = row["Texto"].ToString().Trim(); }// Servicios asignados
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 4,'SUB_COMP',@Pn,'',0,@CoEl,@ICC,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@CoEl", CodElem.Trim());
                        SC.Parameters.AddWithValue("@Pn", DdlPNInsSubC.SelectedItem.Text.Trim());
                        SC.Parameters.AddWithValue("@CHk", CodHK.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);

                            if (DtB.Rows.Count > 0)
                            {
                                GrdSvcInsSubC.DataSource = DtB;
                                GrdSvcInsSubC.DataBind();
                            }
                            else
                            {
                                GrdSvcInsSubC.DataSource = null;
                                GrdSvcInsSubC.DataBind();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string borrar = Ex.ToString();
                    }
                }
            }
        }
        protected void LimpiarCamposInsSubC(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlModelInsSubC.Text = ""; }
            TxtPnInsSubC.Text = "";
            TxtSnInsSubC.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            ViewState["EsMotor"] = "N";
            TxtUbiTecInsSubC.Text = "";
            DdlPosicInsSubC.Text = "";
            TxtFechaInsSubC.Text = "";
            TxtMotivInsSubC.Text = "";
        }
        protected void DdlPNInsSub_TextChanged(object sender, EventArgs e)
        {
            BindDDdlSnVisualMay(DdlPNInsSubC.Text.Trim(), "ISC");
            DdlSNInsSubC.Text = "";
            DdlModelInsSubC.Text = "";
            GrdBusqInsSubC.Visible = false;
            LimpiarCamposInsElem("");
        }
        protected void DdlSNInsSub_TextChanged(object sender, EventArgs e)
        {
            DdlModelInsSubC.Text = "";
            string LtxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 22,'{0}','{1}','','MOD',0,0,0,{2},'01-1-2009','01-01-1900','01-01-1900'",
                DdlPNInsSubC.Text.Trim(), DdlSNInsSubC.SelectedItem.Text.Trim(), Session["!dC!@"]);

            DdlModelInsSubC.DataSource = Cnx.DSET(LtxtSql);
            DdlModelInsSubC.DataMember = "Datos";
            DdlModelInsSubC.DataTextField = "Descripcion";
            DdlModelInsSubC.DataValueField = "CodModelo";
            DdlModelInsSubC.DataBind();
        }
        protected void DdlModelInsSub_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsSubC("");
            BIndDBusqInsSubC();
        }
        protected void BtnPNInsSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDBusqInsSubC();
        }
        protected void BtnSNInsSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDBusqInsSubC();
        }
        protected void BtnUltNivInsSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UT";
            BIndDBusqInsSubC();
        }
        protected void BtnAKVirtualInsSubC_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnVisualizarMayInsSubC_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 6;
            DdlPnVisualMay.Text = DdlPNInsSubC.Text.Trim();
            BindDDdlSnVisualMay(DdlPNInsSubC.Text.Trim(), "VM");
            DdlSnVisualMay.Text = DdlSNInsSubC.Text.Trim();
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnGuardarInsSubC_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (DdlPNInsSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens36HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar el P/N del mayor.
                    return;
                }
                if (DdlSNInsSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens37HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar el S/N del mayor')", true);
                    return;
                }
                if (TxtPnInsSubC.Text.Equals("") || TxtSnInsSubC.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens38HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un sub-componente
                    return;
                }
                if (TxtUbiTecInsSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicInsSubC.Text.Equals("") && DdlPosicInsSubC.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaInsSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha')", true);
                    return;
                }
                if (TxtMotivInsSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }

                Cnx.ValidarFechas(TxtFechaInsSubC.Text.Trim(), "", 1);
                var MensjF = Cnx.GetMensj();
                if (!MensjF.ToString().Trim().Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { MensjF = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
                foreach (GridViewRow Row in GrdSvcInsSubC.Rows)
                {

                    string VbFechaAnt = GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[0].ToString().Trim(); // obtener indice
                    string TxtFecUltCumplInsSubC = (Row.FindControl("TxtFecUltCumplInsSubC") as TextBox).Text.Trim();
                    string VbReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim();
                    if (!TxtFecUltCumplInsSubC.Equals(VbFechaAnt) && VbReporte.Equals("") && ViewState["ValidaFechaSvc"].Equals("N"))
                    {
                        ViewState["ValidaFechaSvc"] = "S";
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens07HkVrt'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Existen servicios en los que se modificaron fechas de cumplimiento y no tienen reporte')", true);
                        return;
                    }
                }

                List<ClsTypAeronaveVirtual> ObjInsSubC = new List<ClsTypAeronaveVirtual>();
                var TypInsSubC = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "IS",
                    CodAeronave = 0,
                    CodModelo = DdlModelInsSubC.Text.Trim(),
                    NivelElemento = "S",
                    Motor = "N",
                    UltimoNivel = TxtUbiTecInsSubC.Text.Trim(),
                    CodMayor = DdlSNInsSubC.Text.Trim(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnInsSubC.Text.Trim(),
                    Sn = TxtSnInsSubC.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaInsSubC.Text),
                    Posicion = DdlPosicInsSubC.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivInsSubC.Text.Trim(),
                };
                ObjInsSubC.Add(TypInsSubC);

                List<ClsTypAeronaveVirtual> ObjServcMantoSC = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdSvcInsSubC.Rows)
                {
                    string StrUC;
                    double VbUC;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrUC = (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim();
                    VbUC = StrUC.Length == 0 ? 0 : Convert.ToDouble(StrUC, Culture);

                    DateTime? VbFechaVence, VbFechaVenceAnt;
                    string VbFecha = (Row.FindControl("TxtFecUltCumplInsSubC") as TextBox).Text.Trim().Equals("") ? null : (Row.FindControl("TxtFecUltCumplInsSubC") as TextBox).Text.Trim();
                    if (VbFecha == null)
                    { VbFechaVence = null; }
                    else
                    { VbFechaVence = Convert.ToDateTime(VbFecha); }

                    VbFecha = GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[0].ToString().Trim().Equals("") ? null : GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    if (VbFecha == null)
                    { VbFechaVenceAnt = null; }
                    else
                    { VbFechaVenceAnt = Convert.ToDateTime(VbFecha); }
                    string borr = GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[2].ToString().Trim();
                    int borrar = Convert.ToInt32(GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    var TypServcMantoSC = new ClsTypAeronaveVirtual()
                    {
                        CodIdContadorElem = Convert.ToInt32(GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[1].ToString().Trim()),
                        CodElementoSvc = GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[3].ToString().Trim(),
                        FechaVence = VbFechaVence,
                        FechaVenceAnt = VbFechaVenceAnt,
                        Resetear = (Row.FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0,
                        CodOT = (Row.FindControl("LblCodOT") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        CodIdContaSrvManto = Convert.ToInt32(GrdSvcInsSubC.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        NumReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim(),
                        ValorUltCump = Convert.ToDouble(VbUC),
                        GeneraHist = (Row.FindControl("CkbGenerarHist") as CheckBox).Checked == true ? "S" : "N",
                    };
                    ObjServcMantoSC.Add(TypServcMantoSC);
                }

                List<ClsTypAeronaveVirtual> ObjCompensacionSC = new List<ClsTypAeronaveVirtual>();

                List<ClsTypAeronaveVirtual> ObjOTSC = new List<ClsTypAeronaveVirtual>();
                ClsTypAeronaveVirtual AeronaveVirtualSC = new ClsTypAeronaveVirtual();
                AeronaveVirtualSC.Alimentar(ObjInsSubC, ObjServcMantoSC, ObjCompensacionSC, ObjOTSC);
                string Mensj = AeronaveVirtualSC.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDHisElemInsSubC(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposInsSubC("TODOS");
                BIndDSvcInsSubC("", "", "0");
                ViewState["TieneCompensacion"] = "N";
                GrdSvcInsSubC.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();
                DataRow[] Result1 = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result1)
                { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens33HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Instalar Sub-Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlPNInsSubC.Text = "";
                DdlSNInsSubC.Text = "";
                DdlModelInsSubC.Text = "";
            }
        }
        protected void GrdBusqInsSubC_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            LimpiarCamposInsSubC("");
            ViewState["CodModelo"] = "";
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[7].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[8].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens21HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplInstSubC, UplInstSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]')", true);
                return;
            }
            TxtPnInsSubC.Text = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[1].Text.Trim());
            TxtSnInsSubC.Text = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusqInsSubC.DataKeys[this.GrdBusqInsSubC.SelectedIndex][0].ToString();
            TxtUbiTecInsSubC.Text = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[5].Text.Trim());
            string PoscElem = HttpUtility.HtmlDecode(GrdBusqInsSubC.SelectedRow.Cells[10].Text.Trim());
            BIndDHisElemInsSubC(ViewState["CodElemento"].ToString().Trim());
            string LtxtSql = string.Format("EXEC Consultas_General_Ingenieria 2,'{0}','{1}','',0, 0,{2},'01-01-1','01-01-1'", TxtUbiTecInsSubC.Text, DdlModelInsSubC.Text.Trim(), Session["!dC!@"]);
            DdlPosicInsSubC.DataSource = Cnx.DSET(LtxtSql);
            DdlPosicInsSubC.DataMember = "Datos";
            DdlPosicInsSubC.DataTextField = "Descripcion";
            DdlPosicInsSubC.DataValueField = "Codigo";
            DdlPosicInsSubC.DataBind();
            if (PoscElem.Equals("S"))
            { DdlPosicInsSubC.Enabled = true; }
            else
            { DdlPosicInsSubC.Enabled = false; }
            GrdBusqInsSubC.Visible = false;
            BIndDSvcInsSubC(ViewState["CodElemento"].ToString().Trim(), DdlModelInsSubC.Text.Trim(), "0");
            GrdSvcInsSubC.Visible = true;
        }
        protected void GrdBusqInsSubC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusqInsSubC.PageIndex = e.NewPageIndex;
            BIndDBusqInsSubC();
        }
        protected void GrdBusqInsSubC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[12].Visible = false;
            }
        }
        protected void TxtFecUltCumplInsSubC_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            var Cntrl = (Control)sender;
            GridViewRow row = (GridViewRow)Cntrl.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtFecUltCumplInsSubC = (TextBox)GrdSvcInsSubC.Rows[rowIndex].FindControl("TxtFecUltCumplInsSubC");

            Cnx.ValidarFechas(TxtFecUltCumplInsSubC.Text.Trim(), "", 1);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow DR in Result)
                { MensjF = DR["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }
            ViewState["ValidaFechaSvc"] = "N";
        }
        //******************************************  REMOVER SUB-COMPONENTE *********************************************************
        protected void BIndDBusqRemSubC()
        {
            if (DdlModelRemSubC.Text.Equals(""))
            { return; }
            GrdBusqRemSubC.Visible = true;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurBusInstalar", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "";
                VbTxtSql = "EXEC SP_TablasIngenieria 12, @SN, @PN, @UN,@CM,'','','','','CurBusInstalar',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqRemSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqRemSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtBusqRemSubC.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@CM", DdlSNRemSubC.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusqRemSubC.DataSource = DtB;
                            GrdBusqRemSubC.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusqRemSubC.DataSource = null;
                            GrdBusqRemSubC.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisContRemSubC(string CodElem)
        {
            if (DdlModelRemSubC.Text.Equals(""))
            { return; }
            TxtTitContadoresRemSub.Text = "S/N: " + TxtSnRemSubC.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContRemSubC.DataSource = DtB;
                            GrdHisContRemSubC.DataBind();
                        }
                        else
                        {
                            GrdHisContRemSubC.DataSource = null;
                            GrdHisContRemSubC.DataBind();
                        }
                    }
                }
            }
        }
        protected void LimpiarCamposRemSubC(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlModelRemSubC.Text = ""; }
            TxtPnRemSubC.Text = "";
            TxtSnRemSubC.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodAeronave"] = 0;
            ViewState["CodModelo"] = "";
            ViewState["EsMotor"] = "N";
            TxtUbiTecRemSubC.Text = "";
            DdlPosicRemSubC.Text = "";
            TxtFechaRemSubC.Text = "";
            TxtMotivRemSubC.Text = "";
        }
        protected void DdlPNRemSubC_TextChanged(object sender, EventArgs e)
        {
            BindDDdlSnVisualMay(DdlPNRemSubC.Text.Trim(), "RSC");
            DdlSNRemSubC.Text = "";
            DdlModelRemSubC.Text = "";
            GrdBusqRemSubC.Visible = false;
            LimpiarCamposRemElem("");
        }
        protected void DdlSNRemSubC_TextChanged(object sender, EventArgs e)
        {
            DdlModelRemSubC.Text = "";
            string LtxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 22,'{0}','{1}','','MOD',0,0,0,{2},'01-1-2009','01-01-1900','01-01-1900'",
                DdlPNRemSubC.Text.Trim(), DdlSNRemSubC.SelectedItem.Text.Trim(), Session["!dC!@"]);

            DdlModelRemSubC.DataSource = Cnx.DSET(LtxtSql);
            DdlModelRemSubC.DataMember = "Datos";
            DdlModelRemSubC.DataTextField = "Descripcion";
            DdlModelRemSubC.DataValueField = "CodModelo";
            DdlModelRemSubC.DataBind();
        }
        protected void DdlModelRemSubC_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsSubC("");
            BIndDBusqRemSubC();
        }
        protected void BtnPNRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDBusqRemSubC();
        }
        protected void BtnSNRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDBusqRemSubC();
        }
        protected void BtnUltNivRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDBusqRemSubC();
        }
        protected void BtnAKVirtualRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnVisualizarMayRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 6;
            DdlPnVisualMay.Text = DdlPNRemSubC.Text.Trim();
            BindDDdlSnVisualMay(DdlPNRemSubC.Text.Trim(), "VM");
            DdlSnVisualMay.Text = DdlSNRemSubC.Text.Trim();
            BIndDVisualMay(DdlPnVisualMay.Text.Trim(), DdlSnVisualMay.SelectedItem.Text.Trim());
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnAbrirOTCerrarRemSubC_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 4;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnGuardarRemSubC_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlModelRemSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens08HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un modelo')", true);
                    return;
                }
                if (DdlPNRemSubC.Text.Equals("") || DdlSNRemSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens35HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un componente mayor')", true);
                    return;
                }
                if (TxtPnRemSubC.Text.Equals("") || TxtSnRemSubC.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens38HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un sub-componente')", true);
                    return;
                }
                if (TxtUbiTecRemSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens03HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicRemSubC.Text.Equals("") && DdlPosicRemSubC.Enabled == true)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens04HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaRemSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha')", true);
                    return;
                }
                string VbMnsj = Cnx.ValidarFechas2(TxtFechaRemSubC.Text.Trim(), "", 1);
                if (!VbMnsj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { VbMnsj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechaRemSubC.Focus();
                    return;
                }

                if (TxtMotivRemSubC.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un motivo')", true);
                    return;
                }

                List<ClsTypAeronaveVirtual> ObjRemSubC = new List<ClsTypAeronaveVirtual>();
                var TypRemSubC = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "RS",
                    CodAeronave = Convert.ToInt32(ViewState["CodAeronave"].ToString()),
                    CodModelo = DdlModelRemSubC.Text.Trim(),
                    NivelElemento = "S",
                    Motor = "N",
                    UltimoNivel = TxtUbiTecRemSubC.Text.Trim(),
                    CodMayor = DdlSNRemSubC.Text.Trim(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnRemSubC.Text.Trim(),
                    Sn = TxtSnRemSubC.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaRemSubC.Text),
                    Posicion = DdlPosicRemSubC.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivRemSubC.Text.Trim(),
                };
                ObjRemSubC.Add(TypRemSubC);

                List<ClsTypAeronaveVirtual> ObjServcMantoSubC = new List<ClsTypAeronaveVirtual>();

                List<ClsTypAeronaveVirtual> ObjCompensacionSubC = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensacSubC = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "S", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicRemSubC.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacionSubC.Add(TypCompensacSubC);
                }

                List<ClsTypAeronaveVirtual> ObjOTSubC = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdOtCerrar.Rows)
                {
                    DateTime? VbFechaI;
                    string VbCcosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    string VbFIText = GrdOtCerrar.DataKeys[Row.RowIndex].Values[1].ToString().Trim();
                    if (VbFIText.Equals("")) { VbFechaI = Convert.ToDateTime(TxtFechaRemSubC.Text); }
                    else { VbFechaI = Convert.ToDateTime(VbFIText); }

                    var TypOTSubC = new ClsTypAeronaveVirtual()
                    {
                        CodNumOrdenTrab = Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        Descripcion = "",
                        CodEstOrdTrab1 = "0002",
                        CodEstOrdTrab2 = "",
                        Aplicabilidad = TxtSnRemSubC.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = "",
                        CodBase = "",
                        CodTaller = "",
                        CodPlanManto = "",
                        CentroCosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim(),
                        FechaInicio = VbFechaI,
                        FechaFinal = Convert.ToDateTime(TxtFechaRemSubC.Text),
                        FechaReg = Convert.ToDateTime((Row.FindControl("LblFechaReg") as Label).Text.Trim()),
                        IdentificadorCorrPrev = 1,
                        CodPrioridad = "",
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = 1,
                        HorasProyectadas = 0,
                        FechaProyectada = null,
                        FechaVencimiento = null,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = (Row.FindControl("TxtMotivo") as TextBox).Text.Trim(),
                        CodTipoCodigo = "",
                        CodInspectorCierre = "",
                        LicenciaInspCierre = "",
                        PNOT = TxtPnRemSubC.Text.Trim(),
                        BloquearDetalle = 0,
                        CodResponsable = Session["C77U"].ToString(),
                        OTSN = 0,
                        OTSO = 0,
                        OTSR = 0,
                        OCSN = 0,
                        OCSO = 0,
                        OCSR = 0,
                        EjecPasos = Convert.ToInt32(GrdOtCerrar.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        CancelOT = 0,
                        WS = "",
                        OKOT = (Row.FindControl("CkbOk") as CheckBox).Checked == true ? 1 : 0,
                        AccionOT = "",
                    };
                    ObjOTSubC.Add(TypOTSubC);
                }
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjRemSubC, ObjServcMantoSubC, ObjCompensacionSubC, ObjOTSubC);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDHisContRemSubC(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposRemSubC("TODOS");
                ViewState["TieneCompensacion"] = "N";
                BtnAbrirOTCerrarRemSubC.Visible = false;
                GrdCompensLv.DataSource = null;
                GrdCompensLv.DataBind();

                DataRow[] Result1 = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result1)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Proceso exitoso
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens25HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Inconveniente
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Remover Sub-Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlPNRemSubC.Text = "";
                DdlSNRemSubC.Text = "";
                DdlModelRemSubC.Text = "";
            }
        }
        protected void GrdBusqRemSubC_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            LimpiarCamposRemSubC("");
            ViewState["CodModelo"] = DdlModelRemSubC.Text.Trim();
            ViewState["CodAeronave"] = Convert.ToInt32(GrdBusqRemSubC.DataKeys[this.GrdBusqRemSubC.SelectedIndex][2].ToString());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[8].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[9].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens21HkVrt'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplRemtSubC, UplRemtSubC.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]
                return;
            }
            TxtPnRemSubC.Text = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[1].Text.Trim());
            TxtSnRemSubC.Text = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusqRemSubC.DataKeys[this.GrdBusqRemSubC.SelectedIndex][0].ToString();
            TxtUbiTecRemSubC.Text = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[5].Text.Trim());
            string vbPos = HttpUtility.HtmlDecode(GrdBusqRemSubC.SelectedRow.Cells[7].Text.Trim());
            DdlPosicRemSubC.Text = vbPos.Trim();
            BIndDHisContRemSubC(ViewState["CodElemento"].ToString().Trim());
            BIndDOCerrarOT(ViewState["CodElemento"].ToString().Trim(), "S");
            GrdBusqRemSubC.Visible = false;
        }
        protected void GrdBusqRemSubC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusqRemSubC.PageIndex = e.NewPageIndex;
            BIndDBusqRemSubC();
        }
        protected void GrdBusqRemSubC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {

                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
            }
        }
        //******************************************  CREACION DE ELEMENTOS CONTROLADOS*********************************************************
        protected void BIndDCrearElemContad()
        {
            if (DdlCrearElemPn.Text.Equals(""))
            { return; }
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            DataTable DtB = new DataTable();
            DtB = DSTDdl.Tables[4].Clone();
            DataRow[] Result = DSTDdl.Tables[4].Select("PN='" + DdlCrearElemPn.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            { DtB.ImportRow(Row); }
            if (DtB.Rows.Count > 0)
            {
                GrdCrearECont.DataSource = DtB;
                GrdCrearECont.DataBind();
            }
            else
            {
                GrdCrearECont.DataSource = null;
                GrdCrearECont.DataBind();
            }
            /* Cnx.SelecBD();
              using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
              {
                  string VbTxtSql = string.Format("EXEC SP_PANTALLA_Elemento 6,@P,'','','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'");
                  sqlConB.Open();
                  using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                  {
                      SC.Parameters.AddWithValue("@P", DdlCrearElemPn.Text.Trim());
                      SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                      using (SqlDataAdapter DAB = new SqlDataAdapter())
                      {
                          DAB.SelectCommand = SC;
                          DAB.Fill(DtB);

                          if (DtB.Rows.Count > 0)
                          {
                              GrdCrearECont.DataSource = DtB;
                              GrdCrearECont.DataBind();
                          }
                          else
                          {
                              GrdCrearECont.DataSource = null;
                              GrdCrearECont.DataBind();
                          }
                      }
                  }
              }*/
        }
        protected void LimpiarCamposCrearElem()
        {
            BtnPropiedad.CssClass = "btn btn-outline-primary";
            BtnCliente.CssClass = "btn btn-outline-primary";
            ViewState["Propiedad"] = 2; // 0= Propiedad Cia| 1=ajeno (Cliente)
            DdlCrearElemPn.Text = "";
            TxtCrearElemSn.Text = "";
            TxtCrearElemFechRec.Text = "";
            TxtCrearElemFechFabr.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodAeronave"] = 0;
            ViewState["CodModelo"] = "";
            ViewState["EsMotor"] = "N";
        }
        protected void BtnCrearElem_Click(object sender, EventArgs e)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            DdlCrearElemPn.DataSource = DSTDdl.Tables[3];
            DdlCrearElemPn.DataTextField = "PN";
            DdlCrearElemPn.DataValueField = "Codigo";
            DdlCrearElemPn.DataBind();

            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 10;
            BtnPropiedad.CssClass = "btn btn-outline-primary";
            BtnCliente.CssClass = "btn btn-outline-primary";
            ViewState["Propiedad"] = 2;
            LblTitCrearEDatosE.Text = "Datos del elemento";
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnPropiedad_Click(object sender, EventArgs e)
        {
            BtnPropiedad.CssClass = "btn btn-primary";
            BtnCliente.CssClass = "btn btn-outline-primary";
            ViewState["Propiedad"] = 0; // 0= Propiedad Cia| 1=ajeno (Cliente)
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnCliente_Click(object sender, EventArgs e)
        {
            BtnCliente.CssClass = "btn btn-primary";
            BtnPropiedad.CssClass = "btn btn-outline-primary";
            ViewState["Propiedad"] = 1; // 0= Propiedad Cia| 1=ajeno (Cliente)
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtCerrarCrearElem_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = (int)ViewState["Ventana"]; }
        protected void DdlCrearElemPn_TextChanged(object sender, EventArgs e)
        { BIndDCrearElemContad(); }
        protected void GrdCrearECont_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                DataRow[] Result = Idioma.Select("Objeto= 'LblTitCrearEDatosE'");
                foreach (DataRow row in Result)
                { LblTitCrearEDatosE.Text = row["Texto"].ToString().Trim() + ": " + dr["Descripcion"].ToString(); }// Datos del elemento:
            }
        }
        protected void BtnCrearElemGuardar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlCrearElemPn.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens43HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar un P/N')", true);
                    return;
                }
                if (TxtCrearElemSn.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens42HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una S/N')", true);
                    return;
                }
                if (TxtCrearElemFechRec.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens41HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha de recibo')", true);
                    return;
                }
                if (TxtCrearElemFechFabr.Text.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens40HkVrt'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe seleccionar una fecha de fabricación')", true);
                    return;
                }
                string VbMnsj = Cnx.ValidarFechas2(TxtCrearElemFechRec.Text.Trim(), "", 1);
                if (!VbMnsj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { VbMnsj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtCrearElemFechRec.Focus();
                    return;
                }
                 VbMnsj = Cnx.ValidarFechas2(TxtCrearElemFechFabr.Text.Trim(), TxtCrearElemFechRec.Text.Trim(), 2);
                if (!VbMnsj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { VbMnsj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtCrearElemFechFabr.Focus();
                    return;
                }
                List<ClsTypAeronaveVirtualCrearElem> ObjCrearElem = new List<ClsTypAeronaveVirtualCrearElem>();
                var TypCrearElem = new ClsTypAeronaveVirtualCrearElem()
                {
                    TipoEvento = "",
                    CodAeronave = (int)ViewState["Propiedad"],
                    CodModelo = "",
                    NivelElemento = "",
                    Motor = "",
                    UltimoNivel = "",
                    CodMayor = "",
                    CodElemento = "",
                    Pn = DdlCrearElemPn.Text.Trim(),
                    Sn = TxtCrearElemSn.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtCrearElemFechFabr.Text),
                    Posicion = "",
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = "",
                };
                ObjCrearElem.Add(TypCrearElem);

                List<ClsTypAeronaveVirtualCrearElem> ObjContadores = new List<ClsTypAeronaveVirtualCrearElem>();
                foreach (GridViewRow Row in GrdCrearECont.Rows)
                {
                    string StrUC;
                    double VbUC;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrUC = (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim();
                    VbUC = StrUC.Length == 0 ? 0 : Convert.ToDouble(StrUC, Culture);

                    var TypContadores = new ClsTypAeronaveVirtualCrearElem()
                    {
                        CodIdContadorElem = 0,
                        CodElementoSvc = "",
                        FechaVence = Convert.ToDateTime(TxtCrearElemFechRec.Text),
                        FechaVenceAnt = null,
                        Resetear = 0,
                        CodOT = 0,
                        CodIdContaSrvManto = 0,
                        NumReporte = (Row.FindControl("LblCodContador") as Label).Text.Trim(),
                        ValorUltCump = Convert.ToDouble(VbUC),
                        GeneraHist = "",
                    };
                    ObjContadores.Add(TypContadores);
                }
                ClsTypAeronaveVirtualCrearElem CrearElemento = new ClsTypAeronaveVirtualCrearElem();
                CrearElemento.Alimentar(ObjCrearElem, ObjContadores);
                string Mensj = CrearElemento.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                GrdCrearECont.DataSource = null;
                GrdCrearECont.DataBind();
                LimpiarCamposCrearElem();
                ViewState["Propiedad"] = 2;

                DataRow[] Result1 = Idioma.Select("Objeto= 'Mens18HkVrt'");
                foreach (DataRow row in Result1)
                { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplCrearElem, UplCrearElem.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Crear Elemento", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlCrearElemPn.Text = "";
            }
        }
    }
}