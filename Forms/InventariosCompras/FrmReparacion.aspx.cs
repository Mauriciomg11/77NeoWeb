using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmReparacion : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable TblDetalle = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSTPpl = new DataSet();
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
                ViewState["Accion"] = "";
                BtnRepaExterna.CssClass = "btn btn-primary";
                ViewState["RepaExtLocal"] = "E";
                ViewState["CodTerceroAnt"] = "";
                ViewState["AutorizadAnt"] = "";
                ViewState["AutorizadPpal"] = "";
                ViewState["CodTransprtAnt"] = "";
                ViewState["LugarEAnt"] = "";
                ViewState["CodPriordAnt"] = "";
                ViewState["CodTallerAnt"] = "";
                ViewState["TtlRegDet"] = 0; // saber si el detalle tiene registro para realizar carga masiva   
                ViewState["Total"] = "0";
                ViewState["CodHK"] = "0";
                ViewState["BloqueoGarantia"] = "N";
                ModSeguridad();
                BindBDdl("UPD");

                RdbMdlBusqRepa.Checked = true;
                RdbMdlOpcBusqPrv.Checked = true;
                BtnSolPedInter.Visible = false;

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
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; BtnOTNew.Visible = false; GrdSolPedInter.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // 
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; BtnAsentar.Visible = false; }// asentar
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//          

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
                    BtnRepaExterna.Text = bO.Equals("BtnRepaExterna") ? bT : BtnRepaExterna.Text;
                    BtnRepaLocal.Text = bO.Equals("BtnRepaLocal") ? bT : BtnRepaLocal.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnOT.Text = bO.Equals("BtnOT") ? bT : BtnOT.Text;
                    BtnOT.ToolTip = bO.Equals("BtnOTTT") ? bT : BtnOT.ToolTip;
                    BtnAsentar.Text = bO.Equals("BtnAsentar") ? bT : BtnAsentar.Text;
                    BtnAsentar.ToolTip = bO.Equals("BtnAsentarTT") ? bT : BtnAsentar.ToolTip;
                    BtnImprimir.Text = bO.Equals("BtnImprimirGrl") ? bT : BtnImprimir.Text;
                    BtnOpenCotiza.Text = bO.Equals("BtnOpenCotiza") ? bT : BtnOpenCotiza.Text;
                    BtnOpenCotiza.ToolTip = bO.Equals("BtnOpenCotizaTT") ? bT : BtnOpenCotiza.ToolTip;
                    BtnSolPedInter.Text = bO.Equals("LblPedido") ? bT : BtnSolPedInter.Text;
                    BtnSolPedInter.ToolTip = bO.Equals("BtnSolPedInterTT") ? bT : BtnSolPedInter.ToolTip;
                    TxtCCosto.ToolTip = bO.Equals("TxtCCostoTT") ? bT : TxtCCosto.ToolTip;
                    LblCotizac.Text = bO.Equals("BtnOpenCotiza") ? bT : LblCotizac.Text;
                    LblPedido.Text = bO.Equals("LblPedido") ? bT : LblPedido.Text;
                    LblNumRepa.Text = bO.Equals("LblNumRepa") ? bT : LblNumRepa.Text;
                    LblFecha.Text = bO.Equals("FechaMstr") ? bT : LblFecha.Text;
                    LblOT.Text = bO.Equals("LblOTMstr") ? bT : LblOT.Text;
                    LblReserva.Text = bO.Equals("LblReserva") ? bT : LblReserva.Text;
                    LblHK.Text = bO.Equals("LblAeronaveMstr") ? bT : LblHK.Text;
                    LblMoned.Text = bO.Equals("LblMoned") ? bT : LblMoned.Text;
                    LblPpt.Text = bO.Equals("LblPpt") ? bT : LblPpt.Text;
                    CkbAprobad.Text = bO.Equals("CkbAprobad") ? bT : CkbAprobad.Text;
                    CkbAsentada.Text = bO.Equals("CkbAsentada") ? bT : CkbAsentada.Text;
                    LblProvee.Text = bO.Equals("LblProvee") ? bT : LblProvee.Text;
                    LblEmplead.Text = bO.Equals("LblEmplead") ? bT : LblEmplead.Text;
                    LblAutoriz.Text = bO.Equals("LblAutoriz") ? bT : LblAutoriz.Text;
                    LblEstd.Text = bO.Equals("LblEstadoMst") ? bT : LblEstd.Text;
                    LblTipo.Text = bO.Equals("TipoMstr") ? bT : LblTipo.Text;
                    LblTransp.Text = bO.Equals("LblTransp") ? bT : LblTransp.Text;
                    LblUbicac.Text = bO.Equals("LblUbicac") ? bT : LblUbicac.Text;
                    LblCant.Text = bO.Equals("CantMst") ? bT : LblCant.Text;
                    LblSubTtal.Text = bO.Equals("LblSubTtal") ? bT : LblSubTtal.Text;
                    LblTtl.Text = bO.Equals("LblTtl") ? bT : LblTtl.Text;
                    LblRazonRemoc.Text = bO.Equals("LblRazonRemoc") ? bT : LblRazonRemoc.Text;
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblBoletines.Text = bO.Equals("LblBoletines") ? bT : LblBoletines.Text;
                    LblTitInstruc.Text = bO.Equals("LblTitInstruc") ? bT : LblTitInstruc.Text;
                    LblTitInstrucGnrl.Text = bO.Equals("LblTitInstrucGnrl") ? bT : LblTitInstrucGnrl.Text;
                    CkbRepair.Text = bO.Equals("CkbRepair") ? bT : CkbRepair.Text;
                    CkbBancoPrueb.Text = bO.Equals("CkbBancoPrueb") ? bT : CkbBancoPrueb.Text;
                    CkbOH.Text = bO.Equals("CkbOH") ? bT : CkbOH.Text;
                    CkbModifc.Text = bO.Equals("CkbModifc") ? bT : CkbModifc.Text;
                    CkbCalibrac.Text = bO.Equals("CkbCalibrac") ? bT : CkbCalibrac.Text;
                    CkbOtros.Text = bO.Equals("CkbOtros") ? bT : CkbOtros.Text;
                    CkbGrtGrntia.Text = bO.Equals("CkbGrtGrntia") ? bT : CkbGrtGrntia.Text;
                    CkbGrtOH.Text = bO.Equals("CkbOH") ? bT : CkbGrtOH.Text;
                    CkbLibera1.Text = bO.Equals("CkbLibera1") ? bT : CkbLibera1.Text;
                    CkbCertifCalib2.Text = bO.Equals("CkbCertifCalib2") ? bT : CkbCertifCalib2.Text;
                    CkbTrabaPedi3.Text = bO.Equals("CkbTrabaPedi3") ? bT : CkbTrabaPedi3.Text;
                    CkbEstandUtili4.Text = bO.Equals("CkbEstandUtili4") ? bT : CkbEstandUtili4.Text;
                    CkbCumplirTodoBolet5.Text = bO.Equals("CkbCumplirTodoBolet5") ? bT : CkbCumplirTodoBolet5.Text;
                    CkbTodoTrabReal6.Text = bO.Equals("CkbTodoTrabReal6") ? bT : CkbTodoTrabReal6.Text;
                    // *************************************************opcion de busqueda Modal *************************************************
                    RdbMdlOpcBusqPrv.Text = bO.Equals("LblProvee") ? "&nbsp" + bT : RdbMdlOpcBusqPrv.Text;
                    RdbMdlOpcBusqCotiz.Text = bO.Equals("BtnOpenCotiza") ? "&nbsp" + bT : RdbMdlOpcBusqCotiz.Text;
                    if (bO.Equals("placeholder"))
                    { TxtModalBusq.Attributes.Add("placeholder", bT); }
                    RdbMdlBusqRepa.Text = bO.Equals("CkbRepair") ? "&nbsp" + bT : RdbMdlBusqRepa.Text;
                    RdbMdlBusqOT.Text = bO.Equals("LblOTMstr") ? "&nbsp" + bT : RdbMdlBusqOT.Text;
                    RdbMdlBusqPrv.Text = bO.Equals("LblProvee") ? "&nbsp" + bT : RdbMdlBusqPrv.Text;
                    RdbMdlBusqPPT.Text = bO.Equals("LblPpt") ? "&nbsp" + bT : RdbMdlBusqPPT.Text;
                    IbtModalBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtModalBusq.ToolTip;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblModalBusq.Text;
                    GrdModalBusqRepa.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqRepa.EmptyDataText;
                    GrdModalBusqRepa.Columns[1].HeaderText = bO.Equals("CkbRepair") ? bT : GrdModalBusqRepa.Columns[1].HeaderText;
                    GrdModalBusqRepa.Columns[2].HeaderText = bO.Equals("LblProvee") ? bT : GrdModalBusqRepa.Columns[2].HeaderText;
                    GrdModalBusqRepa.Columns[3].HeaderText = bO.Equals("FechaMstr") ? bT : GrdModalBusqRepa.Columns[3].HeaderText;
                    GrdModalBusqRepa.Columns[6].HeaderText = bO.Equals("GrdTipElem") ? bT : GrdModalBusqRepa.Columns[6].HeaderText;
                    GrdModalBusqRepa.Columns[7].HeaderText = bO.Equals("BtnOpenCotiza") ? bT : GrdModalBusqRepa.Columns[7].HeaderText;
                    GrdModalBusqRepa.Columns[8].HeaderText = bO.Equals("LblPedido") ? bT : GrdModalBusqRepa.Columns[8].HeaderText;
                    GrdModalBusqRepa.Columns[9].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdModalBusqRepa.Columns[9].HeaderText;
                    GrdModalBusqRepa.Columns[10].HeaderText = bO.Equals("LblMoned") ? bT : GrdModalBusqRepa.Columns[10].HeaderText;
                    GrdModalBusqRepa.Columns[11].HeaderText = bO.Equals("LblOTMstr") ? bT : GrdModalBusqRepa.Columns[11].HeaderText;
                    GrdModalBusqRepa.Columns[12].HeaderText = bO.Equals("LblPpt") ? bT : GrdModalBusqRepa.Columns[12].HeaderText;
                    LblTitModalBusqProv.Text = bO.Equals("LblTitModalBusqProv") ? bT : LblTitModalBusqProv.Text;
                    LblTitModalBusqRepa.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitModalBusqRepa.Text;
                    GrdMdlBusCotiza.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdMdlBusCotiza.EmptyDataText;
                    GrdMdlBusCotiza.Columns[1].HeaderText = bO.Equals("LblRazonRemoc") ? bT : GrdMdlBusCotiza.Columns[1].HeaderText;
                    GrdMdlBusCotiza.Columns[2].HeaderText = bO.Equals("BtnOpenCotiza") ? bT : GrdMdlBusCotiza.Columns[2].HeaderText;
                    GrdMdlBusCotiza.Columns[5].HeaderText = bO.Equals("LblMoned") ? bT : GrdMdlBusCotiza.Columns[5].HeaderText;
                    BtnCloseModalBusqPN.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqPN.Text;
                    //*************************************************Solicitud de pedido Repa Local *************************************************
                    LblTitSolPedInter.Text = bO.Equals("LblTitSolPedInter") ? bT : LblTitSolPedInter.Text;
                    IbtCerrarSolPedInter.ToolTip = bO.Equals("BtnCerrarMst") ? bT : IbtCerrarSolPedInter.ToolTip;
                    GrdSolPedInter.Columns[0].HeaderText = bO.Equals("LblPedido") ? bT : GrdSolPedInter.Columns[0].HeaderText;
                    GrdSolPedInter.Columns[2].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdSolPedInter.Columns[2].HeaderText;
                    GrdSolPedInter.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdSolPedInter.Columns[4].HeaderText;
                    GrdSolPedInter.Columns[5].HeaderText = bO.Equals("CantMst") ? bT : GrdSolPedInter.Columns[5].HeaderText;
                    //************************************************* Generar OT *************************************************
                    LblTitNewOT.Text = bO.Equals("LblTitNewOT") ? bT : LblTitNewOT.Text;
                    BtnOTNew.Text = bO.Equals("BotonIng") ? bT : BtnOTNew.Text;
                    BtnOpenOT.Text = bO.Equals("BtnOT") ? bT : BtnOpenOT.Text;
                    BtnOpenOT.ToolTip = bO.Equals("BtnOpenOTTT") ? bT : BtnOpenOT.ToolTip;
                    BtnOTCerrar.Text = bO.Equals("BtnCerrarMst") ? bT : BtnOTCerrar.Text;
                    LblOtNumOT.Text = bO.Equals("LblOtNumOT") ? bT : LblOtNumOT.Text;
                    LblOtEstado.Text = bO.Equals("LblEstadoMst") ? bT : LblOtEstado.Text;
                    LblOtPrioridad.Text = bO.Equals("LblOtPrioridad") ? bT : LblOtPrioridad.Text;
                    LblOtFechaReg.Text = bO.Equals("FechaMstr") ? bT : LblOtFechaReg.Text;
                    LblOtFechaIni.Text = bO.Equals("LblOtFechaIni") ? bT : LblOtFechaIni.Text;
                    LblOtFechaFin.Text = bO.Equals("LblOtFechaFin") ? bT : LblOtFechaFin.Text;
                    LblOtNumRepa.Text = bO.Equals("LblNumRepa") ? bT : LblOtNumRepa.Text;
                    LblOtTaller.Text = bO.Equals("LblOtTaller") ? bT : LblOtTaller.Text;
                    LblOTTrabajo.Text = bO.Equals("LblOTTrabajo") ? bT : LblOTTrabajo.Text;
                    LblOTAccParc.Text = bO.Equals("LblOTAccParc") ? bT : LblOTAccParc.Text;
                    LblTitImpresion.Text = bO.Equals("LblTitImpresionMstr") ? bT : LblTitImpresion.Text;
                    IbtCerrarImpr.ToolTip = bO.Equals("BtnCerrarMst") ? bT : IbtCerrarImpr.ToolTip;
                    // ************************************************* Aprobar / Asentar *************************************************
                    LblFactura.Text = bO.Equals("LblFactura") ? bT : LblFactura.Text;
                    LblTitAsentar.Text = bO.Equals("LblTitAsentar") ? bT : LblTitAsentar.Text;
                    IbtCloseAsentar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseAsentar.ToolTip;
                    LblTitOpcAprob.Text = bO.Equals("LblTitOpcAprob") ? bT : LblTitOpcAprob.Text;
                    LblTitOpcAsentr.Text = bO.Equals("BtnAsentar") ? bT : LblTitOpcAsentr.Text;
                    IbtAprobar.ToolTip = bO.Equals("IbtAprobar") ? bT : IbtAprobar.ToolTip;
                    IbtDesAprobar.ToolTip = bO.Equals("IbtDesAprobar") ? bT : IbtDesAprobar.ToolTip;
                    IbtAsentar.ToolTip = bO.Equals("BtnAsentar") ? bT : IbtAsentar.ToolTip;
                    IbtDesasentar.ToolTip = bO.Equals("IbtDesasentar") ? bT : IbtDesasentar.ToolTip;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'Mens29Repa'");
                foreach (DataRow row in Result) { IbtAprobar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }//Desea aprobar la Repa?

                Result = Idioma.Select("Objeto= 'Mens30Repa'");
                foreach (DataRow row in Result) { IbtDesAprobar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }// Desea desaprobar la repa?

                Result = Idioma.Select("Objeto= 'Mens31Repa'");
                foreach (DataRow row in Result) { IbtAsentar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }//Desea asentar la repa?

                Result = Idioma.Select("Objeto= 'Mens32Repa'");
                foreach (DataRow row in Result) { IbtDesasentar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }// Desea revertir el asiento la repa?
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            ImageButton ImgIns = GrdSolPedInter.FooterRow.FindControl("IbtAddNew") as ImageButton;
            if (ViewState["Accion"].ToString().Equals("")) { ImgIns.Enabled = false; }
            else { ImgIns.Enabled = true; }

            foreach (GridViewRow Row in GrdSolPedInter.Rows)
            {
                ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    if (ViewState["Accion"].ToString().Equals("")) { imgD.Enabled = false; }

                    if ((int)ViewState["VblEliMS"] == 0) { Row.Cells[5].Controls.Remove(imgD); }
                }
            }
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BindDdlLugarEntrega(string TipoRepa)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["LugarEntrg"].Rows.Count > 0)
            {
                string VbQry = "";
                DataTable DT = new DataTable();
                if (TipoRepa.Equals("ALL")) { VbQry = "Activo=1 OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }
                if (TipoRepa.Equals("N") || TipoRepa.Equals("E")) { VbQry = "Activo=1 OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }//AND TipoUbicacion ='N' 
                if (TipoRepa.Equals("I")) { VbQry = "Activo=1  OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }//AND TipoUbicacion ='I'
                DataRow[] DR = DSTDdl.Tables[6].Select(VbQry);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlUbicac.DataSource = DT;
                DdlUbicac.DataTextField = "Descripcion";
                DdlUbicac.DataValueField = "CodIdTipoUbicaCia";
                DdlUbicac.DataBind();
                DdlUbicac.SelectedValue = ViewState["LugarEAnt"].ToString().Trim();
            }
        }
        protected void BindDdlAutorizado()
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Autorizado"].Rows.Count > 0)
            {
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataRow[] DR = DSTDdl.Tables[2].Select("TipoUsu= 'P' AND Rango = 'Igual_Mayor'");
                foreach (DataRow row in DR)
                { ViewState["AutorizadPpal"] = row["CodUsuario"].ToString().Trim(); }//Usuario autorizacion principal.

                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[2].Select("TipoUsu= 'P' AND Rango = 'Igual_Mayor' OR CodUsuario = '" + ViewState["AutorizadAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DR = DSTDdl.Tables[2].Select("TipoUsu = 'A' AND Rango = 'Igual_Mayor' AND CodUsuario <> '" + ViewState["AutorizadAnt"] + "'");
                foreach (DataRow Row in DR)
                { DT.ImportRow(Row); }
                if (Convert.ToDouble(ViewState["Total"]) == 0)
                {
                    DR = DSTDdl.Tables[2].Select("Rango = 'Menor' AND CodUsuario <> '" + ViewState["AutorizadAnt"] + "'");
                    foreach (DataRow Row in DR)
                    { DT.ImportRow(Row); }
                }
                if (Convert.ToDouble(ViewState["Total"]) > 0 && TxtMoned.Text.Trim().Equals("COP"))
                {

                    DR = DSTDdl.Tables[2].Select("Rango = 'Menor' AND ValorCop > " + ViewState["Total"] + "AND CodUsuario <> '" + ViewState["AutorizadAnt"] + "'");
                    foreach (DataRow Row in DR)
                    { DT.ImportRow(Row); }
                }
                if (Convert.ToDouble(ViewState["Total"]) > 0 && TxtMoned.Text.Trim().Equals("USD"))
                {

                    DR = DSTDdl.Tables[2].Select("Rango = 'Menor' AND ValorCop > " + ViewState["Total"] + "AND CodUsuario <> '" + ViewState["AutorizadAnt"] + "'");
                    foreach (DataRow Row in DR)
                    { DT.ImportRow(Row); }
                }
                DdlAutoriz.DataSource = DT;
                DdlAutoriz.DataTextField = "Usuario";
                DdlAutoriz.DataValueField = "CodUsuario";
                DdlAutoriz.DataBind();
                DdlAutoriz.SelectedValue = ViewState["AutorizadAnt"].ToString().Trim();
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Reparacion 1,'','','','','','DDL',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Tercero";
                                DSTDdl.Tables[1].TableName = "Empleado";
                                DSTDdl.Tables[2].TableName = "Autorizado";
                                DSTDdl.Tables[3].TableName = "Estado";
                                DSTDdl.Tables[4].TableName = "TipoRepa";
                                DSTDdl.Tables[5].TableName = "Transport";
                                DSTDdl.Tables[6].TableName = "LugarEntrg";
                                DSTDdl.Tables[7].TableName = "CotizaSinRepaExterna";
                                DSTDdl.Tables[8].TableName = "EstadOT";
                                DSTDdl.Tables[9].TableName = "PrioriOT";
                                DSTDdl.Tables[10].TableName = "Taller";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            string VbCodAnt;

            if (DSTDdl.Tables["Tercero"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[0].Select("(Activo=1 AND  Clasificacion IN ('P','A')) OR CodTercero= '" + ViewState["CodTerceroAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlProvee.DataSource = DT;
                DdlProvee.DataTextField = "RazonSocial";
                DdlProvee.DataValueField = "CodTercero";
                DdlProvee.DataBind();
                DdlProvee.SelectedValue = ViewState["CodTerceroAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Empleado"].Rows.Count > 0)
            {
                VbCodAnt = DdlEmplead.Text.Trim();
                DdlEmplead.DataSource = DSTDdl.Tables[1];
                DdlEmplead.DataTextField = "Nombre";
                DdlEmplead.DataValueField = "CodPersona";
                DdlEmplead.DataBind();
                DdlEmplead.Text = VbCodAnt;
            }
            BindDdlAutorizado();
            if (DSTDdl.Tables["Estado"].Rows.Count > 0)
            {
                VbCodAnt = DdlEstd.Text.Trim();
                DdlEstd.DataSource = DSTDdl.Tables[3];
                DdlEstd.DataTextField = "Estado";
                DdlEstd.DataValueField = "CodEstadoCompra";
                DdlEstd.DataBind();
                DdlEstd.Text = VbCodAnt.Trim().Equals("") ? "01" : VbCodAnt;
            }
            if (DSTDdl.Tables["TipoRepa"].Rows.Count > 0)
            {
                VbCodAnt = DdlTipo.Text.Trim();
                DdlTipo.DataSource = DSTDdl.Tables[4];
                DdlTipo.DataTextField = "Descripcion";
                DdlTipo.DataValueField = "CodIdTipoOrdenCompra";
                DdlTipo.DataBind();
                DdlTipo.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["Transport"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[5].Select("Activo=1 AND Clasificacion IN ('P') OR CodTercero= '" + ViewState["CodTransprtAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlTransp.DataSource = DT;
                DdlTransp.DataTextField = "RazonSocial";
                DdlTransp.DataValueField = "CodTercero";
                DdlTransp.DataBind();
                DdlTransp.SelectedValue = ViewState["CodTransprtAnt"].ToString().Trim();
            }
            BindDdlLugarEntrega("ALL");
            if (DSTDdl.Tables["EstadOT"].Rows.Count > 0)
            {
                VbCodAnt = DdlOtEstado.Text.Trim();
                DdlOtEstado.DataSource = DSTDdl.Tables[8];
                DdlOtEstado.DataTextField = "Descripcion";
                DdlOtEstado.DataValueField = "Codigo";
                DdlOtEstado.DataBind();
                DdlOtEstado.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["PrioriOT"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[9].Select("Activo=1 OR CodPrioridadSolicitudMat= '" + ViewState["CodPriordAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlOTPrioridad.DataSource = DT;
                DdlOTPrioridad.DataTextField = "Descripcion";
                DdlOTPrioridad.DataValueField = "CodPrioridadSolicitudMat";
                DdlOTPrioridad.DataBind();
                DdlOTPrioridad.SelectedValue = ViewState["CodPriordAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Taller"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[10].Select("Activo=1 OR CodTaller= '" + ViewState["CodTallerAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlOtTaller.DataSource = DT;
                DdlOtTaller.DataTextField = "NomTaller";
                DdlOtTaller.DataValueField = "CodTaller";
                DdlOtTaller.DataBind();
                DdlOtTaller.SelectedValue = ViewState["CodTallerAnt"].ToString().Trim();
            }
        }
        protected void Traerdatos(string CodRepa, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC PNTLL_Reparacion 1,@Cod, @NT,'','','','',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Cod", CodRepa);
                            SC.Parameters.AddWithValue("@NT", Session["Nit77Cia"]);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "Repa";
                                    DSTPpl.Tables[1].TableName = "DetSolPedInter";
                                    DSTPpl.Tables[2].TableName = "SolPedRepaLocal";
                                    DSTPpl.Tables[3].TableName = "Impresion";

                                    ViewState["DSTPpl"] = DSTPpl;
                                    ViewState["TblDetalle"] = DSTPpl.Tables["DetSolPedInter"];
                                }
                            }
                        }
                    }
                }
                TxtModalBusq.Text = "";
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                DataRow[] Result;
                if (DSTPpl.Tables["Repa"].Rows.Count > 0)
                {
                    TxtNumRepa.Text = DSTPpl.Tables[0].Rows[0]["CodReparacion"].ToString().Trim();
                    string VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaRepaDMY"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaRepaDMY"].ToString().Trim();
                    DateTime VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    TxtOT.Text = DSTPpl.Tables[0].Rows[0]["CodNumOrdenTrab"].ToString().Trim();
                    TxtCodigoOT.Text = DSTPpl.Tables[0].Rows[0]["CodigoOT"].ToString().Trim();
                    TxtReserva.Text = DSTPpl.Tables[0].Rows[0]["CodNumReserva"].ToString().Trim();
                    ViewState["CodHK"] = DSTPpl.Tables[0].Rows[0]["CodAeronaveRO"].ToString().Trim();
                    TxtHK.Text = DSTPpl.Tables[0].Rows[0]["Matricula"].ToString().Trim();
                    CkbAprobad.Checked = DSTPpl.Tables[0].Rows[0]["Aprobado"].ToString().Trim().Equals("1") ? true : false;
                    CkbAsentada.Checked = DSTPpl.Tables[0].Rows[0]["Asentado"].ToString().Trim().Equals("1") ? true : false;
                    TxtFactura.Text = DSTPpl.Tables[0].Rows[0]["NumFactura"].ToString().Trim();
                    ViewState["CodTerceroAnt"] = DSTPpl.Tables[0].Rows[0]["CodProveedor"].ToString().Trim();
                    DdlEmplead.Text = DSTPpl.Tables[0].Rows[0]["CodEmpleado"].ToString().Trim();
                    ViewState["AutorizadAnt"] = DSTPpl.Tables[0].Rows[0]["CodAutorizador"].ToString().Trim();
                    ViewState["Total"] = DSTPpl.Tables[0].Rows[0]["ValorTotalCot"].ToString().Trim();
                    BindDdlAutorizado();
                    DdlEstd.Text = DSTPpl.Tables[0].Rows[0]["CodEstadoRepa"].ToString().Trim();
                    DdlTipo.Text = DSTPpl.Tables[0].Rows[0]["CodTipoOrdenRepa"].ToString().Trim();
                    ViewState["CodTransprtAnt"] = DSTPpl.Tables[0].Rows[0]["CodTransportador"].ToString().Trim();
                    ViewState["LugarEAnt"] = DSTPpl.Tables[0].Rows[0]["CodUbicacionCia"].ToString().Trim();
                    TxtlPN.Text = DSTPpl.Tables[0].Rows[0]["PNRepa"].ToString().Trim();
                    TxtlPN.ToolTip = DSTPpl.Tables[0].Rows[0]["DescrElem"].ToString().Trim();
                    TxtSN.Text = DSTPpl.Tables[0].Rows[0]["Sn"].ToString().Trim();
                    TxtSN.ToolTip = DSTPpl.Tables[0].Rows[0]["DescrElem"].ToString().Trim();
                    TxtCant.Text = DSTPpl.Tables[0].Rows[0]["Cantidad"].ToString().Trim();
                    TxtSubTtal.Text = DSTPpl.Tables[0].Rows[0]["Monto"].ToString().Trim();
                    TxtTtl.Text = DSTPpl.Tables[0].Rows[0]["ValorTotalCotTxt"].ToString().Trim();
                    CkbRepair.Checked = DSTPpl.Tables[0].Rows[0]["CkRepa"].ToString().Trim().Equals("1") ? true : false;
                    CkbBancoPrueb.Checked = DSTPpl.Tables[0].Rows[0]["CkBP"].ToString().Trim().Equals("1") ? true : false;
                    CkbOH.Checked = DSTPpl.Tables[0].Rows[0]["CkOH"].ToString().Trim().Equals("1") ? true : false;
                    CkbModifc.Checked = DSTPpl.Tables[0].Rows[0]["CkMdf"].ToString().Trim().Equals("1") ? true : false;
                    CkbCalibrac.Checked = DSTPpl.Tables[0].Rows[0]["CkClb"].ToString().Trim().Equals("1") ? true : false;
                    CkbOtros.Checked = DSTPpl.Tables[0].Rows[0]["CkOtr"].ToString().Trim().Equals("1") ? true : false;
                    TxtOtros.Text = DSTPpl.Tables[0].Rows[0]["Otros"].ToString().Trim();
                    CkbGrtAOG.Checked = DSTPpl.Tables[0].Rows[0]["AOG"].ToString().Trim().Equals("1") ? true : false;
                    CkbGrtGrntia.Checked = DSTPpl.Tables[0].Rows[0]["Garantia"].ToString().Trim().Equals("1") ? true : false;
                    CkbGrtOH.Checked = DSTPpl.Tables[0].Rows[0]["OverHaul"].ToString().Trim().Equals("1") ? true : false;
                    CkbLibera1.Checked = DSTPpl.Tables[0].Rows[0]["CkLA"].ToString().Trim().Equals("1") ? true : false;
                    CkbCertifCalib2.Checked = DSTPpl.Tables[0].Rows[0]["CkCC"].ToString().Trim().Equals("1") ? true : false;
                    CkbTrabaPedi3.Checked = DSTPpl.Tables[0].Rows[0]["CkTPID"].ToString().Trim().Equals("1") ? true : false;
                    CkbEstandUtili4.Checked = DSTPpl.Tables[0].Rows[0]["CkEUFC"].ToString().Trim().Equals("1") ? true : false;
                    CkbCumplirTodoBolet5.Checked = DSTPpl.Tables[0].Rows[0]["CkADSB"].ToString().Trim().Equals("1") ? true : false;
                    CkbTodoTrabReal6.Checked = DSTPpl.Tables[0].Rows[0]["CkTIDSB"].ToString().Trim().Equals("1") ? true : false;
                    TxtCCosto.Text = DSTPpl.Tables[0].Rows[0]["CCosto"].ToString().Trim() + " - " + DSTPpl.Tables[0].Rows[0]["DescCcosto"].ToString().Trim();
                    ViewState["CCostoRepa"] = DSTPpl.Tables[0].Rows[0]["CCosto"].ToString().Trim();
                    TxtRazonRemoc.Text = DSTPpl.Tables[0].Rows[0]["RazonRemocion"].ToString().Trim();
                    TxtObserv.Text = DSTPpl.Tables[0].Rows[0]["Observacion"].ToString().Trim();
                    TxtBoletines.Text = DSTPpl.Tables[0].Rows[0]["Boletines"].ToString().Trim();
                    ViewState["BloqueoGarantia"] = DSTPpl.Tables[0].Rows[0]["BloqueoGarantia"].ToString().Trim();
                    TxtOtNumOT.Text = DSTPpl.Tables[0].Rows[0]["CodNumOrdenTrab"].ToString().Trim();
                    TxtOtCodigoOT.Text = DSTPpl.Tables[0].Rows[0]["CodigoOT"].ToString().Trim();
                    DdlOtEstado.Text = DSTPpl.Tables[0].Rows[0]["CodEstOrdTrab1"].ToString().Trim();
                    ViewState["CodPriordAnt"] = DSTPpl.Tables[0].Rows[0]["CodPrioridad"].ToString().Trim();
                    ViewState["CodTallerAnt"] = DSTPpl.Tables[0].Rows[0]["CodTaller"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaReg"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaReg"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtOtFechaReg.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT).Equals("1900-01-01") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaInicio"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaInicio"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    txtOtFechaIni.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT).Equals("1900-01-01") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaFinal"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaFinal"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtOtFechaFin.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT).Equals("1900-01-01") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);

                    TxtOTTrabajo.Text = DSTPpl.Tables[0].Rows[0]["TrabajReq"].ToString().Trim();
                    TxtOTAccParc.Text = DSTPpl.Tables[0].Rows[0]["AccionParcial"].ToString().Trim();

                    if (ViewState["BloqueoGarantia"].ToString().Equals("S"))
                    {
                        CkbGrtGrntia.ToolTip = "";
                        Result = Idioma.Select("Objeto= 'Mens09Repa'");
                        foreach (DataRow row in Result) { CkbGrtGrntia.ToolTip = row["Texto"].ToString().Trim(); }//Se inactiva el campo porque la reparación ya tiene ingreso.                       
                    }
                    if (TxtOT.Text.Equals("0")) { BtnOTNew.Enabled = true; }
                    else { BtnOTNew.Enabled = false; }
                    if (ViewState["RepaExtLocal"].ToString().Equals("E"))
                    {
                        if ((int)ViewState["VblCE2"] == 1)
                        {
                            BtnAsentar.Visible = true;

                            if (CkbAsentada.Checked == true) { IbtAprobar.Visible = false; IbtDesAprobar.Visible = false; IbtAsentar.Visible = false; IbtDesasentar.Visible = true; }
                            else
                            {
                                if (CkbAprobad.Checked == true) { IbtAprobar.Visible = false; IbtDesAprobar.Visible = true; IbtAsentar.Visible = true; IbtDesasentar.Visible = false; }
                                else { IbtAprobar.Visible = true; IbtDesAprobar.Visible = false; IbtAsentar.Visible = false; IbtDesasentar.Visible = false; }
                            }
                        }
                    }
                    else { BtnAsentar.Visible = false; }

                    BindBDdl("SEL");
                }
            }
            catch (Exception Ex)
            {
                BtnIngresar.Visible = false; BtnModificar.Visible = false;
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
            }
        }
        protected void BotonesTipoRepa(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["TtlRegDet"] = 0; //LimpiarCampos("DEL"); 
            BtnRepaExterna.CssClass = "btn btn-outline-primary";
            BtnRepaLocal.CssClass = "btn btn-outline-primary";
            ViewState["RepaExtLocal"] = Tipo;
            if (Tipo.Equals("E"))
            {
                BtnRepaExterna.CssClass = "btn btn-primary"; BtnSolPedInter.Visible = false; CkbAprobad.Visible = true; CkbAsentada.Visible = true; BtnImprimir.Visible = true; 
                LblFactura.Visible = true; TxtFactura.Visible = true;
                if ((int)ViewState["VblCE2"] == 1)
                { BtnAsentar.Visible = true; }
            }
            else
            {
                BtnRepaLocal.CssClass = "btn btn-primary"; BtnSolPedInter.Visible = true; BtnAsentar.Visible = false;
                CkbAprobad.Visible = false; CkbAsentada.Visible = false; LblFactura.Visible = false; TxtFactura.Visible = false; BtnImprimir.Visible = false;
            }
            GrdModalBusqRepa.DataSource = null; GrdModalBusqRepa.DataBind();
            LimpiarCampos(""); MultVw.ActiveViewIndex = 0;
        }
        protected void BtnRepaExterna_Click(object sender, EventArgs e)
        { BotonesTipoRepa("E"); }
        protected void BtnRepaLocal_Click(object sender, EventArgs e)
        { BotonesTipoRepa("L"); }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (ViewState["Accion"].ToString().Trim().Equals(""))// consulta las reparaciones
            {
                LblTitModalBusqRepa.Visible = true; TblMdlOpcBusRepa.Visible = true; GrdModalBusqRepa.Visible = true;
                LblTitModalBusqProv.Visible = false; TblMdlOpcBusCotiza.Visible = false; GrdMdlBusCotiza.Visible = false;
            }
            else
            {
                LblTitModalBusqRepa.Visible = false; TblMdlOpcBusRepa.Visible = false; GrdModalBusqRepa.Visible = false;
                LblTitModalBusqProv.Visible = true; TblMdlOpcBusCotiza.Visible = true; GrdMdlBusCotiza.Visible = true;
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void ActivarBtn(bool In, bool Md, bool Cnslt, bool El, bool Ip, bool Otr, string Accion)
        {
            BtnRepaExterna.Enabled = Md;
            BtnRepaLocal.Enabled = Md;
            BtnConsultar.Enabled = Cnslt;
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnOT.Enabled = Otr;
            BtnAsentar.Enabled = Otr;
            BtnImprimir.Enabled = Ip;
            BtnOpenCotiza.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            // DdlProvee.Enabled = Edi;
            DdlAutoriz.Enabled = Edi;
            DdlEstd.Enabled = Edi;
            if (ViewState["RepaExtLocal"].ToString().Equals("E")) { DdlTransp.Enabled = Edi; }
            if (ViewState["RepaExtLocal"].ToString().Equals("E")) { TxtFactura.Enabled = Edi; }
            DdlUbicac.Enabled = Edi;
            TxtRazonRemoc.Enabled = Edi;
            TxtObserv.Enabled = Edi;
            TxtBoletines.Enabled = Edi;
            TxtOtros.Enabled = Edi;
            CkbRepair.Enabled = Edi;
            CkbBancoPrueb.Enabled = Edi;
            CkbOH.Enabled = Edi;
            CkbModifc.Enabled = Edi;
            CkbCalibrac.Enabled = Edi;
            CkbOtros.Enabled = Edi;
            CkbGrtAOG.Enabled = Edi;
            if (ViewState["BloqueoGarantia"].ToString().Equals("N"))
            { CkbGrtGrntia.Enabled = Edi; }
            CkbGrtOH.Enabled = Edi;
            CkbLibera1.Enabled = Edi;
            CkbCertifCalib2.Enabled = Edi;
            CkbTrabaPedi3.Enabled = Edi;
            CkbEstandUtili4.Enabled = Edi;
            CkbCumplirTodoBolet5.Enabled = Edi;
            CkbTodoTrabReal6.Enabled = Edi;
        }
        protected void LimpiarCampos(string Accion)
        {
            TxtCCosto.Text = "";
            TxtCotizac.Text = "";
            TxtPedido.Text = "";
            TxtNumRepa.Text = "";
            TxtFecha.Text = "";
            TxtOT.Text = "";
            TxtCodigoOT.Text = "";
            TxtReserva.Text = "";
            TxtHK.Text = "";
            TxtMoned.Text = "";
            TxtPpt.Text = "";
            CkbAprobad.Checked = false;
            CkbAsentada.Checked = false;
            TxtFactura.Text = "";
            DdlProvee.Text = "";
            DdlEmplead.Text = Session["C77U"].ToString().Trim();
            DdlAutoriz.Text = ViewState["AutorizadPpal"].ToString().Trim();
            DdlEstd.Text = "01";
            DdlTipo.Text = "N";
            DdlTransp.Text = "";
            DdlUbicac.Text = "";
            TxtlPN.Text = "";
            TxtSN.Text = "";
            TxtCant.Text = "0";
            TxtSubTtal.Text = "0";
            TxtTtl.Text = "0";
            ViewState["IdCotiza"] = "0";
            TxtRazonRemoc.Text = "";
            TxtObserv.Text = "";
            TxtBoletines.Text = "";
            CkbRepair.Checked = false;
            CkbBancoPrueb.Checked = false;
            CkbOH.Checked = false;
            CkbModifc.Checked = false;
            CkbCalibrac.Checked = false;
            CkbOtros.Checked = false;
            TxtOtros.Text = "";
            CkbGrtAOG.Checked = false;
            CkbGrtGrntia.Checked = false;
            CkbGrtOH.Checked = false;
            CkbLibera1.Checked = false;
            CkbCertifCalib2.Checked = false;
            CkbTrabaPedi3.Checked = false;
            CkbEstandUtili4.Checked = false;
            CkbCumplirTodoBolet5.Checked = false;
            CkbTodoTrabReal6.Checked = false;
            TxtRazonRemoc.Text = "";
            TxtObserv.Text = "";
            TxtBoletines.Text = "";
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (DdlProvee.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el proveedor.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTransp.Text.Trim().Equals("") && ViewState["RepaExtLocal"].ToString().Equals("E"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el transportador.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlUbicac.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la ubicación de entrega.
                ViewState["Validar"] = "N"; return;
            }
            if (CkbOtros.Checked == true && TxtOtros.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la descripción en la opción otros.
                ViewState["Validar"] = "N"; TxtOtros.Focus(); return;
            }
            if (TxtlPN.Text.Trim().Equals("") && TxtHK.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens19Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// El campo P/N es requerido.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    if (ViewState["RepaExtLocal"].ToString().Equals("E")) { ActivarBtn(true, false, true, false, false, false, "INS"); }
                    else { ActivarBtn(false, true, false, false, false, false, "INS"); }

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCampos("INS");
                    ViewState["BloqueoGarantia"] = "N";
                    ActivarCampos(true, true, "INS");
                    string VbD = Convert.ToString(DateTime.UtcNow.Day);
                    string VbM = Convert.ToString(DateTime.UtcNow.Month);
                    string VbY = Convert.ToString(DateTime.UtcNow.Year);
                    string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, VbD);
                    DateTime VbFecID = Convert.ToDateTime(fecha);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                    TxtModalBusq.Text = "";

                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("INS");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    string VbCkRep = CkbRepair.Checked == true ? "1" : "0";
                    string VbCkBPr = CkbBancoPrueb.Checked == true ? "1" : "0";
                    string VbCkOH = CkbOH.Checked == true ? "1" : "0";
                    string VbCkModf = CkbModifc.Checked == true ? "1" : "0";
                    string VbCkClbr = CkbCalibrac.Checked == true ? "1" : "0";
                    string VbCkOtrs = CkbOtros.Checked == true ? "1" : "0";

                    string VbCk1 = CkbLibera1.Checked == true ? "1" : "0";
                    string VbCk2 = CkbCertifCalib2.Checked == true ? "1" : "0";
                    string VbCk3 = CkbTrabaPedi3.Checked == true ? "1" : "0";
                    string VbCk4 = CkbEstandUtili4.Checked == true ? "1" : "0";
                    string VbCk5 = CkbCumplirTodoBolet5.Checked == true ? "1" : "0";
                    string VbCk6 = CkbTodoTrabReal6.Checked == true ? "1" : "0";

                    string VbInstruc = VbCkRep + VbCkBPr + VbCkOH + VbCkModf + VbCkClbr + VbCkOtrs + VbCk1 + VbCk2 + VbCk3 + VbCk4 + VbCk5 + VbCk6;
                    int VbTipoRepa = ViewState["RepaExtLocal"].ToString().Equals("L") ? 2 : 1;
                    List<ClsTypReparacion> ObjRepa = new List<ClsTypReparacion>();
                    var TypRepa = new ClsTypReparacion()
                    {
                        CodReparacion = "",
                        CodCotizacion = TxtCotizac.Text.Trim(),
                        CodPedido = TxtPedido.Text.Trim(),
                        CodProveedor = DdlProvee.Text.Trim(),
                        CodEmpleado = DdlEmplead.Text.Trim(),
                        CodAutorizador = DdlAutoriz.Text.Trim(),
                        CodTipoOrdenRepa = DdlTipo.Text.Trim(),
                        CodTransportador = DdlTransp.Text.Trim(),
                        CodUbicacionCia = DdlUbicac.Text.Trim(),
                        FechaReparacion = Convert.ToDateTime(TxtFecha.Text),
                        CodEstadoRepa = DdlEstd.Text.Trim(),
                        Garantia = CkbGrtGrntia.Checked == true ? 1 : 0,
                        Sn = TxtSN.Text.Trim(),
                        AOG = CkbGrtAOG.Checked == true ? 1 : 0,
                        RazonRemocion = TxtRazonRemoc.Text.Trim(),
                        Instruccion = VbInstruc,
                        Observacion = TxtObserv.Text.Trim(),
                        EngineerBull = TxtBoletines.Text.Trim(),
                        Otros = CkbOtros.Checked == true ? TxtOtros.Text.Trim() : "",
                        Aprobado = CkbAprobad.Checked == true ? 1 : 0,
                        OverHaul = CkbGrtOH.Checked == true ? 1 : 0,
                        CompMayor = Convert.ToInt32(0),
                        SalidaRepa = Convert.ToInt32(0),
                        obstransportador = "",
                        Asentado = CkbAsentada.Checked == true ? 1 : 0,
                        CodAsiento = "",
                        Recibido = Convert.ToInt32(0),
                        CodNumOrdenTrab = Convert.ToInt32(TxtOT.Text.Trim().Equals("") ? "0" : TxtOT.Text.Trim()),
                        TipoReparacion = VbTipoRepa,
                        Cantidad = Convert.ToDouble(TxtCant.Text.Trim().Equals("") ? "0" : TxtCant.Text.Trim()),
                        ReembolsableProv = "",
                        CuentaPuc = "",
                        NumFactura = TxtFactura.Text.Trim(),
                        PNRepa = TxtlPN.Text.Trim(),
                        CodAeronaveRO = Convert.ToInt32(ViewState["CodHK"].ToString()),
                        PPT = Convert.ToInt32(TxtPpt.Text.Trim()),
                    };
                    ObjRepa.Add(TypRepa);

                    List<ClsTypReparacion> ObjDetRepa = new List<ClsTypReparacion>();
                    if (ViewState["RepaExtLocal"].ToString().Equals("L"))
                    {
                        if (TblDetalle.Rows.Count > 0)
                        {
                            foreach (DataRow DR in TblDetalle.Rows)
                            {

                                string IdxDetPed = DR["IdDetPedido"].ToString().Trim();
                                string IdxPed = DR["IdPedido"].ToString().Trim();
                                string VbPos = DR["Posicion"].ToString().Trim();

                                var TypDetRepa = new ClsTypReparacion()
                                {
                                    IDRepaDetSolPed = Convert.ToInt32(0),
                                    IdDetPedido = Convert.ToInt32(IdxDetPed),
                                    IdPedido = Convert.ToInt32(IdxPed),
                                    Posicion = Convert.ToInt32(VbPos),
                                };
                                ObjDetRepa.Add(TypDetRepa);
                            }
                        }
                    }
                    ClsTypReparacion ClsRepa = new ClsTypReparacion();
                    ClsRepa.Accion("INSERT");
                    ClsRepa.Alimentar(ObjRepa, ObjDetRepa);
                    string Mensj = ClsRepa.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        string VbPn = ""; //ClsRepa.GetPN().Trim().Equals("") ? "" : "  P/N: [" + ClsRepa.GetPN().Trim() + "]";
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VbPn + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, true, "INS");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    Traerdatos(ClsRepa.GetCodRepa().ToString().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    if (ViewState["RepaExtLocal"].Equals("L")) { BindSolPedInter(); PerfilesGrid(); }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Reparación", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    if (TxtNumRepa.Text.Equals("")) { return; }
                    if (ViewState["RepaExtLocal"].ToString().Equals("E")) { ActivarBtn(false, true, true, false, false, false, "UPD"); }
                    else { ActivarBtn(false, true, false, false, false, false, "UPD"); }

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//

                    ActivarCampos(true, true, "UPD");

                    TxtModalBusq.Text = "";

                    Result = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    string VbCkRep = CkbRepair.Checked == true ? "1" : "0";
                    string VbCkBPr = CkbBancoPrueb.Checked == true ? "1" : "0";
                    string VbCkOH = CkbOH.Checked == true ? "1" : "0";
                    string VbCkModf = CkbModifc.Checked == true ? "1" : "0";
                    string VbCkClbr = CkbCalibrac.Checked == true ? "1" : "0";
                    string VbCkOtrs = CkbOtros.Checked == true ? "1" : "0";

                    string VbCk1 = CkbLibera1.Checked == true ? "1" : "0";
                    string VbCk2 = CkbCertifCalib2.Checked == true ? "1" : "0";
                    string VbCk3 = CkbTrabaPedi3.Checked == true ? "1" : "0";
                    string VbCk4 = CkbEstandUtili4.Checked == true ? "1" : "0";
                    string VbCk5 = CkbCumplirTodoBolet5.Checked == true ? "1" : "0";
                    string VbCk6 = CkbTodoTrabReal6.Checked == true ? "1" : "0";

                    string VbInstruc = VbCkRep + VbCkBPr + VbCkOH + VbCkModf + VbCkClbr + VbCkOtrs + VbCk1 + VbCk2 + VbCk3 + VbCk4 + VbCk5 + VbCk6;
                    int VbTipoRepa = ViewState["RepaExtLocal"].ToString().Equals("L") ? 2 : 1;
                    List<ClsTypReparacion> ObjRepa = new List<ClsTypReparacion>();
                    var TypRepa = new ClsTypReparacion()
                    {
                        CodReparacion = TxtNumRepa.Text.Trim(),
                        CodCotizacion = TxtCotizac.Text.Trim(),
                        CodPedido = TxtPedido.Text.Trim(),
                        CodProveedor = DdlProvee.Text.Trim(),
                        CodEmpleado = DdlEmplead.Text.Trim(),
                        CodAutorizador = DdlAutoriz.Text.Trim(),
                        CodTipoOrdenRepa = DdlTipo.Text.Trim(),
                        CodTransportador = DdlTransp.Text.Trim(),
                        CodUbicacionCia = DdlUbicac.Text.Trim(),
                        FechaReparacion = Convert.ToDateTime(TxtFecha.Text),
                        CodEstadoRepa = DdlEstd.Text.Trim(),
                        Garantia = CkbGrtGrntia.Checked == true ? 1 : 0,
                        Sn = TxtSN.Text.Trim(),
                        AOG = CkbGrtAOG.Checked == true ? 1 : 0,
                        RazonRemocion = TxtRazonRemoc.Text.Trim(),
                        Instruccion = VbInstruc,
                        Observacion = TxtObserv.Text.Trim(),
                        EngineerBull = TxtBoletines.Text.Trim(),
                        Otros = CkbOtros.Checked == true ? TxtOtros.Text.Trim() : "",
                        Aprobado = CkbAprobad.Checked == true ? 1 : 0,
                        OverHaul = CkbGrtOH.Checked == true ? 1 : 0,
                        CompMayor = Convert.ToInt32(0),
                        SalidaRepa = Convert.ToInt32(0),
                        obstransportador = "",
                        Asentado = CkbAsentada.Checked == true ? 1 : 0,
                        CodAsiento = "",
                        Recibido = Convert.ToInt32(0),
                        CodNumOrdenTrab = Convert.ToInt32(TxtOT.Text.Trim().Equals("") ? "0" : TxtOT.Text.Trim()),
                        TipoReparacion = VbTipoRepa,
                        Cantidad = Convert.ToDouble(TxtCant.Text.Trim().Equals("") ? "0" : TxtCant.Text.Trim()),
                        ReembolsableProv = "",
                        CuentaPuc = "",
                        NumFactura = TxtFactura.Text.Trim(),
                        PNRepa = TxtlPN.Text.Trim(),
                        CodAeronaveRO = Convert.ToInt32(ViewState["CodHK"].ToString()),
                        PPT = Convert.ToInt32(TxtPpt.Text.Trim()),
                    };
                    ObjRepa.Add(TypRepa);

                    List<ClsTypReparacion> ObjDetRepa = new List<ClsTypReparacion>();
                    if (TblDetalle.Rows.Count > 0)
                    {
                        foreach (DataRow DR in TblDetalle.Rows)
                        {

                            string IdxDetPed = DR["IdDetPedido"].ToString().Trim();
                            string IdxPed = DR["IdPedido"].ToString().Trim();
                            string VbPos = DR["Posicion"].ToString().Trim();

                            var TypDetRepa = new ClsTypReparacion()
                            {
                                IDRepaDetSolPed = Convert.ToInt32(DR["IDRepaDetSolPed"].ToString().Trim()),
                                IdDetPedido = Convert.ToInt32(IdxDetPed),
                                IdPedido = Convert.ToInt32(IdxPed),
                                Posicion = Convert.ToInt32(VbPos),
                            };
                            ObjDetRepa.Add(TypDetRepa);
                        }
                    }
                    ClsTypReparacion ClsRepa = new ClsTypReparacion();
                    ClsRepa.Accion("UPDATE");
                    ClsRepa.Alimentar(ObjRepa, ObjDetRepa);
                    string Mensj = ClsRepa.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        string VbPn = ""; //ClsRepa.GetPN().Trim().Equals("") ? "" : "  P/N: [" + ClsRepa.GetPN().Trim() + "]";
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VbPn + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, true, "UPD");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "UPD");
                    Traerdatos(TxtNumRepa.Text.Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    if (ViewState["RepaExtLocal"].Equals("L")) { BindSolPedInter(); PerfilesGrid(); }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Reparación", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnOpenCotiza_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string CT = "window.open('/Forms/InventariosCompras/FrmCotizacion.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), CT, true);
        }
        //****************************** MOdal Busq Repa / Cotizacion **************************************
        protected void BindModalBusqRepa()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbOpc = "SP";
                DataTable DT = new DataTable();
                if (RdbMdlBusqRepa.Checked == true) { VbOpc = "RP"; }
                if (RdbMdlBusqPN.Checked == true) { VbOpc = "PN"; }
                if (RdbMdlBusqSN.Checked == true) { VbOpc = "SN"; }
                if (RdbMdlBusqOT.Checked == true) { VbOpc = "OT"; }
                if (RdbMdlBusqPrv.Checked == true) { VbOpc = "PV"; }
                if (RdbMdlBusqPPT.Checked == true) { VbOpc = "PT"; }
                string VbTxtSql = "EXEC PNTLL_Reparacion 2, @Doc, @Opc,@Typ,'','','',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Doc", TxtModalBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpc);
                    SC.Parameters.AddWithValue("@Typ", ViewState["RepaExtLocal"].ToString());
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0) { GrdModalBusqRepa.DataSource = DT; }
                    else { GrdModalBusqRepa.DataSource = null; }
                    GrdModalBusqRepa.DataBind();
                }
            }
        }
        protected void BindModalBusqCot()
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["CotizaSinRepaExterna"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DataRow[] DR;
                if (RdbMdlOpcBusqPrv.Checked == true)
                {
                    DR = DSTDdl.Tables[7].Select("RazonSocial LIKE '%" + TxtModalBusq.Text.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                if (RdbMdlOpcBusqCotiz.Checked == true)
                {
                    DR = DSTDdl.Tables[7].Select("CodCotizacion LIKE '%" + TxtModalBusq.Text.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                if (DT.Rows.Count > 0) { GrdMdlBusCotiza.DataSource = DT; }
                else { GrdMdlBusCotiza.DataSource = null; }
                GrdMdlBusCotiza.DataBind();
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (LblTitModalBusqRepa.Visible == true) { BindModalBusqRepa(); }
            if (LblTitModalBusqProv.Visible == true) { BindModalBusqCot(); }

            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbCodR = ((Label)row.FindControl("LblCodRepa")).Text.ToString().Trim();
                TxtCotizac.Text = ((Label)row.FindControl("LblCotiza")).Text.ToString().Trim();
                TxtPedido.Text = ((Label)row.FindControl("LblPedido")).Text.ToString().Trim();
                TxtMoned.Text = ((Label)row.FindControl("LblMoneda")).Text.ToString().Trim();
                TxtPpt.Text = ((Label)row.FindControl("LblPT")).Text.ToString().Trim();
                Traerdatos(VbCodR, "UPD");
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
                    foreach (DataRow RowIdioma in Result) { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void GrdMdlBusCotiza_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["Total"] = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["ValorTotalCot"].ToString().Trim();

                ViewState["AutorizadAnt"] = ViewState["AutorizadPpal"];

                TxtPpt.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["IdPropuesta"].ToString().Trim();
                ViewState["CodHK"] = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["CodAeronaveCT"].ToString().Trim();
                TxtHK.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["Matricula"].ToString().Trim();
                DdlProvee.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["CodTercero"].ToString().Trim();
                DdlTipo.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["CodTipoCotizacion"].ToString().Trim();
                BindDdlLugarEntrega(DdlTipo.Text);
                string borr = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["LugarEntrega"].ToString().Trim();
                DdlUbicac.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["LugarEntrega"].ToString().Trim();
                TxtCotizac.Text = ((Label)row.FindControl("LblCodCtzc")).Text.ToString().Trim();
                TxtPedido.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["CodPedido"].ToString().Trim();
                TxtPpt.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["IdPropuesta"].ToString().Trim();
                TxtCant.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["Cantidad"].ToString().Trim();
                TxtlPN.Text = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                TxtMoned.Text = ((Label)row.FindControl("LblMoneda")).Text.ToString().Trim();
                TxtSN.Text = ((Label)row.FindControl("LblSN")).Text.ToString().Trim();
                TxtRazonRemoc.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["DescricionServicio"].ToString().Trim();
                TxtSubTtal.Text = GrdMdlBusCotiza.DataKeys[gvr.RowIndex].Values["Monto"].ToString().Trim();
                TxtTtl.Text = ViewState["Total"].ToString().Trim();
                BindDdlAutorizado();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    DataTable DT = new DataTable();
                    string VbTxtSql = "EXEC SP_PANTALLA_Reparacion 1, @S, @P, @CdRp,'',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@S", TxtSN.Text.Trim());
                        SC.Parameters.AddWithValue("@P", TxtlPN.Text.Trim());
                        SC.Parameters.AddWithValue("@CdRp", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DT);
                        if (DT.Rows.Count > 0)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens08Repa'");
                            foreach (DataRow DR in Result) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DR["Texto"].ToString() + "');", true); }//La serie tiene reparaciones abiertas.
                        }
                    }
                }
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdMdlBusCotiza_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIrCot = (e.Row.FindControl("IbtIrCot") as ImageButton);
                if (IbtIrCot != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtIrCot.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //****************************** Pedido Repa Local **************************************       
        protected void BtnSolPedInter_Click(object sender, EventArgs e)
        {
            if (TxtNumRepa.Text.Equals("")) { return; }
            BindSolPedInter(); PerfilesGrid();
            ViewState["CCostoSP"] = ""; ViewState["PosicionSP"] = "0"; ViewState["IdPedidoRL"] = "0"; ViewState["IdDetPedidoRL"] = "0";
            MultVw.ActiveViewIndex = 1;
        }
        protected void BindSolPedInter()
        {
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int VbNumReg = TblDetalle.Rows.Count;
            TblDetalle.AcceptChanges();
            foreach (DataRow row in TblDetalle.Rows)
            {
                object value = row["PN"];
                if (value == DBNull.Value)
                {
                    if (VbNumReg > 1) { row.Delete(); }
                }
            }
            TblDetalle.AcceptChanges();

            if (TblDetalle.Rows.Count > 0) { GrdSolPedInter.DataSource = TblDetalle; }
            else
            {
                TblDetalle.Rows.Add(TblDetalle.NewRow());
                GrdSolPedInter.DataSource = TblDetalle;
                GrdSolPedInter.Rows[0].Cells.Clear();
                GrdSolPedInter.Rows[0].Cells.Add(new TableCell());
                GrdSolPedInter.Rows[0].Cells[0].Text = "Empty..!";
                GrdSolPedInter.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                TblDetalle.NewRow();
                GrdSolPedInter.DataSource = TblDetalle;
            }
            GrdSolPedInter.DataBind();
        }
        protected void IbtCerrarSolPedInter_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void DdlSolPed_TextChanged(object sender, EventArgs e)
        {
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            if (DSTPpl.Tables[2].Rows.Count > 0)
            {
                DropDownList DdlSolPed = (GrdSolPedInter.FooterRow.FindControl("DdlSolPed") as DropDownList);
                TextBox TxtPn = (GrdSolPedInter.FooterRow.FindControl("TxtPn") as TextBox);
                TextBox TxtRef = (GrdSolPedInter.FooterRow.FindControl("TxtRef") as TextBox);
                TextBox TxtSn = (GrdSolPedInter.FooterRow.FindControl("TxtSn") as TextBox);
                TextBox TxtCantSP = (GrdSolPedInter.FooterRow.FindControl("TxtCant") as TextBox);
                TextBox TxtDescr = (GrdSolPedInter.FooterRow.FindControl("TxtDescr") as TextBox);
                if (!DdlSolPed.Text.Equals(""))
                {
                    DataRow[] Result = DSTPpl.Tables[2].Select("CodPedido= '" + DdlSolPed.Text.Trim() + "'");
                    foreach (DataRow row in Result)
                    {
                        TxtPn.Text = row["PN"].ToString().Trim();
                        TxtRef.Text = row["CodReferencia"].ToString().Trim();
                        TxtSn.Text = row["Notas"].ToString().Trim();
                        TxtCantSP.Text = row["CantidadTotal"].ToString().Trim();
                        ViewState["CCostoSP"] = row["Ccostos"].ToString().Trim();
                        TxtDescr.Text = row["Descripcion"].ToString().Trim();
                        ViewState["PosicionSP"] = row["Posicion"].ToString().Trim();
                        ViewState["IdPedidoRL"] = row["IdPedido"].ToString().Trim();
                        ViewState["IdDetPedidoRL"] = row["IdDetPedido"].ToString().Trim();
                    }
                }
            }
        }
        protected void GrdSolPedInter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            DataRow[] Result;
            if (e.CommandName.Equals("AddNew"))
            {
                string VbSolPed = (GrdSolPedInter.FooterRow.FindControl("DdlSolPed") as DropDownList).Text.Trim();
                if (VbSolPed.ToString().Trim().Equals("-"))
                {
                    Result = Idioma.Select("Objeto= 'Mens16Repa'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe seleccionar un pedido.
                    return;
                }

                Result = TblDetalle.Select("CodPedido= '" + VbSolPed.Trim() + "'");
                foreach (DataRow row in Result)
                {
                    Result = Idioma.Select("Objeto= 'Mens16Repa'");
                    foreach (DataRow DR in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DR["Texto"].ToString() + "');", true); }// Debe seleccionar un pedido.
                    return;
                }

                if (ViewState["CCostoSP"].ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'Mens17Repa'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La Solicitud ya se encuentra registrada.
                    return;
                }

                DataTable DTV = new DataTable();
                Result = TblDetalle.Select("CodPedido <> ''");
                if (IsIENumerableLleno(Result))
                { DTV = Result.CopyToDataTable(); }

                int VbNumReg = DTV.Rows.Count;
                if (VbNumReg > 0)
                {
                    Result = Idioma.Select("Objeto= 'Mens18Repa'");
                    foreach (DataRow DR in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DR["Texto"].ToString() + "');", true); }// Solo se permite un registro.
                    return;
                }

                string IdxDetPed = ViewState["IdDetPedidoRL"].ToString();
                string IdxPed = ViewState["IdPedidoRL"].ToString();
                string VbCodRef = (GrdSolPedInter.FooterRow.FindControl("TxtRef") as TextBox).Text.Trim();
                string VbPn = (GrdSolPedInter.FooterRow.FindControl("TxtPn") as TextBox).Text.Trim();
                string VbCant = (GrdSolPedInter.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim();
                string VbSN = (GrdSolPedInter.FooterRow.FindControl("TxtSn") as TextBox).Text.Trim();
                string VbDesc = (GrdSolPedInter.FooterRow.FindControl("TxtDescr") as TextBox).Text.Trim();
                string VbPos = ViewState["PosicionSP"].ToString().Trim();
                TblDetalle.AcceptChanges();
                TblDetalle.Rows.Add(VbSolPed, VbPos, VbCodRef, VbPn, Convert.ToDouble(VbCant), Convert.ToInt32(IdxDetPed), 0, VbSN, VbDesc, Convert.ToInt32(IdxPed));
                TblDetalle.AcceptChanges();
                ViewState["TblDetalle"] = TblDetalle;
                BindSolPedInter();
                TxtPedido.Text = VbSolPed;
                TxtlPN.Text = VbPn;
                TxtSN.Text = VbSN;
                TxtCant.Text = VbCant;
                ViewState["CCostoSP"] = ""; ViewState["PosicionSP"] = "0"; ViewState["IdPedidoRL"] = "0"; ViewState["IdDetPedidoRL"] = "0";
            }
        }
        protected void GrdSolPedInter_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (!TxtOT.Text.Trim().Equals("0"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens14Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// No es posible eliminar el registro porque la reparación se encuentra con una O.T. asiganada.
                return;
            }

            int index = Convert.ToInt32(e.RowIndex);
            TblDetalle.Rows[index].Delete();
            BindSolPedInter();
            TxtlPN.Text = "";
            TxtSN.Text = "";
            TxtCant.Text = "0";
        }
        protected void GrdSolPedInter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlSolPed = (DropDownList)e.Row.FindControl("DdlSolPed");
                DdlSolPed.DataSource = DSTPpl.Tables[2];
                DdlSolPed.DataTextField = "CodPedido";
                DdlSolPed.DataValueField = "CodPedido";
                DdlSolPed.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto='IbtAddNew'");
                foreach (DataRow RowIdioma in Result)
                { IbtAddNew.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
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
        //****************************** Generar OT **************************************       
        protected void BtnOT_Click(object sender, EventArgs e)
        {
            if (TxtNumRepa.Text.Equals("0")) { return; }
            TxtOtNumRepa.Text = TxtNumRepa.Text.Trim();
            MultVw.ActiveViewIndex = 2;
        }
        protected void ActivarBtnOT(bool In, bool Md, bool Otr, string Accion)
        {
            BtnOpenOT.Enabled = Otr; BtnOTCerrar.Enabled = Otr;
        }
        protected void LimpiarCamposOT(string Accion)
        {
            DdlOtEstado.Text = "0001";
            DdlOTPrioridad.Text = "";
            TxtOtFechaReg.Text = "";
            txtOtFechaIni.Text = "";
            TxtOtFechaFin.Text = "";
            DdlOtTaller.Text = "";
            TxtOTTrabajo.Text = TxtRazonRemoc.Text.Trim();
            TxtOTAccParc.Text = "";
        }
        protected void ValidarCamposOT(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (DdlOTPrioridad.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens22Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la prioridad.
                ViewState["Validar"] = "N"; return;
            }
            if (TxtOTTrabajo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens23Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el transportador.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarCamposOT(bool Ing, bool Edi, string Accion)
        {
            DdlOTPrioridad.Enabled = Edi;
            DdlOtTaller.Enabled = Edi;
            TxtOTTrabajo.Enabled = Edi;
            TxtOTAccParc.Enabled = Edi;
        }
        protected void BtnOTNew_Click(object sender, EventArgs e)
        {
            if (!TxtOtNumOT.Text.Equals("0")) { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtnOT(true, false, false, "INS");
                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnOTNew.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCamposOT("INS");
                    ActivarCamposOT(true, true, "INS");
                    string VbD = Convert.ToString(DateTime.UtcNow.Day);
                    string VbM = Convert.ToString(DateTime.UtcNow.Month);
                    string VbY = Convert.ToString(DateTime.UtcNow.Year);
                    string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, VbD);
                    DateTime VbFecID = Convert.ToDateTime(fecha);
                    TxtOtFechaReg.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);

                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnOTNew.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    ValidarCamposOT("INS");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    DateTime? VbFechaReg = Convert.ToDateTime(TxtOtFechaReg.Text);
                    List<ClsTypOrdenTrabajo> ObjOT = new List<ClsTypOrdenTrabajo>();

                    var TypOT = new ClsTypOrdenTrabajo()
                    {
                        CodNumOrdenTrab = Convert.ToInt32(0),
                        Descripcion = TxtOTTrabajo.Text.Trim(),
                        CodEstOrdTrab1 = DdlOtEstado.Text.Trim(),
                        CodEstOrdTrab2 = "",
                        Aplicabilidad = TxtSN.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = TxtOtNumRepa.Text.Trim(),
                        CodBase = "",
                        CodTaller = DdlOtTaller.Text.Trim(),
                        CodPlanManto = "",
                        CentroCosto = ViewState["CCostoRepa"].ToString().Trim(),
                        FechaInicio = null,
                        FechaFinal = null,
                        FechaReg = VbFechaReg,
                        IdentificadorCorrPrev = 0,
                        CodPrioridad = DdlOTPrioridad.Text.Trim(),
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = 0,
                        HorasProyectadas = 0,
                        FechaProyectada = null,
                        FechaVencimiento = null,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = TxtOTAccParc.Text.Trim(),
                        CodTipoCodigo = "01",
                        CodInspectorCierre = "",
                        LicenciaInspCierre = "",
                        PNOT = TxtlPN.Text.Trim(),
                        BloquearDetalle = 0,
                        CodResponsable = "",
                        OTSN = Convert.ToDouble(0),
                        OTSO = Convert.ToDouble(0),
                        OTSR = Convert.ToDouble(0),
                        OCSN = Convert.ToDouble(0),
                        OCSO = Convert.ToDouble(0),
                        OCSR = Convert.ToDouble(0),
                        EjecPasos = 1,
                        CancelOT = 0,
                        WS = "",
                        OKOT = Convert.ToInt32(ViewState["CodHK"]),
                        AccionOT = "INSERT",
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
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }

                    ActivarBtnOT(false, true, true, "INS");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnOTNew.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCamposOT(false, false, "INS");
                    Traerdatos(TxtNumRepa.Text.ToString().Trim(), "UPD");
                    BtnOTNew.OnClientClick = "";
                    if (ViewState["RepaExtLocal"].Equals("L")) { BindSolPedInter(); PerfilesGrid(); }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR OT de la Reparación", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnOTCerrar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BtnOpenOT_Click(object sender, EventArgs e)
        {
            if (TxtOtNumRepa.Text.Equals("0")) { return; }

            Session["PCodOT"] = TxtOT.Text.ToString();
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string CT = "window.open('/Forms/Ingenieria/FrmOrdenTrabajo.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), CT, true);
        }
        //****************************** Imprimir **************************************       
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
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
            DTMultL.Columns.Add("MltlC32", typeof(string));
            DTMultL.Columns.Add("MltlC33", typeof(string));
            DTMultL.Columns.Add("MltlC34", typeof(string));
            DTMultL.Columns.Add("MltlC35", typeof(string));
            DTMultL.Columns.Add("MltlC36", typeof(string));
            DTMultL.Columns.Add("MltlC37", typeof(string));// Formato fecha y hora
            DTMultL.Columns.Add("MltlC38", typeof(string));
            DTMultL.Columns.Add("MltlC39", typeof(string));
            if (DTMultL.Rows.Count == 0)
            { DTMultL.Rows.Add(0, "01", "", "03", "", "", "06", "", "", "09", "", "11", "12", "13", "", "", "16", "", "", "19", ""); }

            ViewState["DTMultL"] = DTMultL;
        }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (TxtNumRepa.Text.Equals("")) { return; }
            if (CkbAprobad.Checked == false) { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            CampoMultiL();
            DTMultL = (DataTable)ViewState["DTMultL"];
            MultVw.ActiveViewIndex = 3;
            DataRow DR = DTMultL.AsEnumerable().Where(r => ((int)r["ID"]).Equals(0)).First();
            DataRow[] Result = Idioma.Select("Objeto= 'InfDesc1'");
            foreach (DataRow row in Result) { DR["MltlC01"] = row["Texto"].ToString().Trim(); }

            Result = Idioma.Select("Objeto= 'InfDesc2'");
            foreach (DataRow row in Result) { DR["MltlC02"] = row["Texto"].ToString().Trim(); }
            DR["MltlC03"] = LblProvee.Text + ":";
            Result = Idioma.Select("Objeto= 'InfCod'");
            foreach (DataRow row in Result) { DR["MltlC04"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfDir'");
            foreach (DataRow row in Result) { DR["MltlC05"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfCiudad'");
            foreach (DataRow row in Result) { DR["MltlC06"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfPais'");
            foreach (DataRow row in Result) { DR["MltlC07"] = row["Texto"].ToString().Trim() + ":"; }

            DR["MltlC08"] = TitForm.Text; DR["MltlC09"] = LblFecha.Text; DR["MltlC10"] = ""; DR["MltlC11"] = "";
            if (CkbGrtAOG.Checked == true) { DR["MltlC10"] = CkbGrtAOG.Text; }
            if (CkbGrtGrntia.Checked == true) { DR["MltlC11"] = CkbGrtGrntia.Text.ToUpper(); }

            Result = Idioma.Select("Objeto= 'InfTelef'");
            foreach (DataRow row in Result) { DR["MltlC12"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfEnviarA'");
            foreach (DataRow row in Result) { DR["MltlC13"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfEmbarcarA'");
            foreach (DataRow row in Result) { DR["MltlC14"] = row["Texto"].ToString().Trim() + ":"; }

            DR["MltlC15"] = GrdSolPedInter.Columns[4].HeaderText;

            Result = Idioma.Select("Objeto= 'InfFecRemo'");
            foreach (DataRow row in Result) { DR["MltlC16"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfFecInstal'");
            foreach (DataRow row in Result) { DR["MltlC17"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfAeron'");
            foreach (DataRow row in Result) { DR["MltlC18"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfUltRepa'");
            foreach (DataRow row in Result) { DR["MltlC19"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfDatGaran'");
            foreach (DataRow row in Result) { DR["MltlC20"] = row["Texto"].ToString().Trim(); }

            Result = Idioma.Select("Objeto= 'InfMotvRem'");
            foreach (DataRow row in Result) { DR["MltlC21"] = row["Texto"].ToString().Trim() + ":"; }// Motivo Remo
            DR["MltlC22"] = CkbOtros.Text;

            Result = Idioma.Select("Objeto= 'InfDescto'");
            foreach (DataRow row in Result) { DR["MltlC23"] = row["Texto"].ToString().Trim() + ":"; }// descuento

            Result = Idioma.Select("Objeto= 'InfMonto'");
            foreach (DataRow row in Result) { DR["MltlC24"] = row["Texto"].ToString().Trim() + ":"; }// Monto

            Result = Idioma.Select("Objeto= 'InfFecTRM'");
            foreach (DataRow row in Result) { DR["MltlC25"] = row["Texto"].ToString().Trim() + ":"; }// Valor TRM

            DR["MltlC26"] = "";
            if (CkbRepair.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + CkbRepair.Text; }
            if (CkbBancoPrueb.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + " - " + CkbBancoPrueb.Text; }
            if (CkbOH.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + " - " + CkbOH.Text; }
            if (CkbModifc.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + " - " + CkbModifc.Text; }
            if (CkbCalibrac.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + " - " + CkbCalibrac.Text; }
            if (CkbOtros.Checked == true) { DR["MltlC26"] = DR["MltlC26"] + " - " + CkbOtros.Text; }

            DR["MltlC27"] = LblTitInstruc.Text + ":";

            Result = Idioma.Select("Objeto= 'InfBoltIng'");
            foreach (DataRow row in Result) { DR["MltlC28"] = row["Texto"].ToString().Trim() + ":"; }// Beletines Ingeniería

            DR["MltlC29"] = LblObserv.Text + ":";

            Result = Idioma.Select("Objeto= 'InfDevoluc'");
            foreach (DataRow row in Result) { DR["MltlC30"] = row["Texto"].ToString().Trim() + ":"; }// La Unidad Se Debe Devolver Con

            Result = Idioma.Select("Objeto= 'InfNumCot'");
            foreach (DataRow row in Result) { DR["MltlC31"] = row["Texto"].ToString().Trim() + ":"; }// Cotización Local

            Result = Idioma.Select("Objeto= 'InfNumPed'");
            foreach (DataRow row in Result) { DR["MltlC32"] = row["Texto"].ToString().Trim() + ":"; }// Pedido Local

            DR["MltlC33"] = "";
            if (CkbLibera1.Checked == true) { DR["MltlC33"] = CkbLibera1.Text + "." + "\n"; }
            if (CkbCertifCalib2.Checked == true) { DR["MltlC33"] = DR["MltlC33"] + CkbCertifCalib2.Text + "." + "\n"; }
            if (CkbTrabaPedi3.Checked == true) { DR["MltlC33"] = DR["MltlC33"] + CkbTrabaPedi3.Text + "." + "\n"; }
            if (CkbEstandUtili4.Checked == true) { DR["MltlC33"] = DR["MltlC33"] + CkbEstandUtili4.Text + "." + "\n"; }
            if (CkbCumplirTodoBolet5.Checked == true) { DR["MltlC33"] = DR["MltlC33"] + CkbCumplirTodoBolet5.Text + "." + "\n"; }
            if (CkbTodoTrabReal6.Checked == true) { DR["MltlC33"] = DR["MltlC33"] + CkbTodoTrabReal6.Text + "."; }

            Result = Idioma.Select("Objeto= 'InfPrepPor'");
            foreach (DataRow row in Result) { DR["MltlC34"] = row["Texto"].ToString().Trim(); }// Preparado Por

            Result = Idioma.Select("Objeto= 'InfAutorPor'");
            foreach (DataRow row in Result) { DR["MltlC35"] = row["Texto"].ToString().Trim(); }// Firma Autorizada

            DR["MltlC36"] = CkbOtros.Text + ":";

           if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC37"] = "MM/dd/yyyy HH:mm"; }
           else { { DR["MltlC37"] = "dd/MM/yyyy HH:mm"; } }

            DTMultL.AcceptChanges();
            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[3];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

                RpVwAll.LocalReport.EnableExternalImages = true;

                RpVwAll.LocalReport.ReportPath = "Report/Logistica/Inf_ReparacionStdr.rdlc";
                RpVwAll.LocalReport.DataSources.Clear();
                RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DSTPpl.Tables[3]));
                RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DTMultL));
                RpVwAll.LocalReport.SetParameters(parameters);
                RpVwAll.LocalReport.Refresh();

            }
        }
        //****************************** Aprobar / Asentar **************************************       
        protected void BtnAsentar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNumRepa.Text.Equals(""))
            { return; }

            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            DataTable DT = new DataTable();
            if (DSTDdl.Tables["Autorizado"].Rows.Count > 0)
            {
                DR = DSTDdl.Tables[2].Select("Rango = 'Igual_Mayor' AND CodUsuario = '" + Session["C77U"].ToString().Trim() + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                else
                {
                    string VbQuery = "";
                    switch (TxtMoned.Text.Trim())
                    {
                        case "COP":
                            VbQuery = "Rango = 'Menor' AND CodUsuario = '" + Session["C77U"].ToString().Trim() + "' AND ValorCop >" + ViewState["Total"].ToString();
                            break;
                        case "USD":
                            VbQuery = "Rango = 'Menor' AND CodUsuario = '" + Session["C77U"].ToString().Trim() + "' AND ValorUSD >" + ViewState["Total"].ToString();
                            break;
                        default:
                            VbQuery = "Rango = 'Menor' AND CodUsuario = '" + Session["C77U"].ToString().Trim() + "' AND ValorEURO >" + ViewState["Total"].ToString();
                            break;
                    }
                    DR = DSTDdl.Tables[2].Select(VbQuery);
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    else
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens24Repa'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Acceso denegado, verificar configuración aprobacion compras.
                        return;
                    }
                }
            }

            MultVw.ActiveViewIndex = 4;

        }
        protected void IbtCloseAsentar_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void IbtAprobar_Click(object sender, ImageClickEventArgs e)
        {
            string VbEjecPlano = "N";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_Pantalla_Asentar_Repa 9, @PO, @FR, @US,'APROBAR','',0, 0,0, @ICC,'01-01-1','01-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@FR", TxtFactura.Text.Trim());// 
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            //var Mensj = SC.ExecuteScalar();
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                string VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                VbEjecPlano = HttpUtility.HtmlDecode(SDR["EjecPlano"].ToString().Trim());
                                string VbOtrosDatos = HttpUtility.HtmlDecode(SDR["OtrosDatos"].ToString().Trim());
                                if (!VbOtrosDatos.Trim().Equals("")) { VbOtrosDatos = " [" + VbOtrosDatos.Trim() + "]"; }

                                if (!VbMensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { VbMensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + VbOtrosDatos + "');", true);
                                    SDR.Close();
                                    Transac.Rollback();
                                    return;
                                }
                            }
                            SDR.Close();
                            Transac.Commit();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Aprobar Compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }

                }
            }
            Traerdatos(TxtNumRepa.Text.Trim(), "UPD");
            if (VbEjecPlano.Trim().Equals("S"))
            {
                Cnx.SelecBD();
                using (SqlConnection SCXP = new SqlConnection(Cnx.GetConex()))
                {
                    SCXP.Open();
                    string VBQuery = "EXEC SP_Pantalla_Asentar_Repa 10, @PO, '', @US,'','',0, 0,0, @ICC,'01-01-1','01-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCXP))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        { SC.ExecuteNonQuery(); }
                        catch (Exception ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); } //Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Aprobar Repa", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void IbtDesAprobar_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_Pantalla_Asentar_Repa 9, @PO, @FR, @US,'DESAPROBAR','',0, 0,0, @ICC,'01-01-1','01-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@FR", TxtFactura.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                string VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());

                                if (!VbMensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { VbMensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                                    Transac.Rollback();

                                    return;
                                }
                            }
                            SDR.Close();
                            Transac.Commit();
                            Traerdatos(TxtNumRepa.Text.Trim(), "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void IbtAsentar_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (TxtFactura.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens33Repa'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar  la factura del proveedor.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_Pantalla_Asentar_Repa 11, @PO, @FR, @US,'ASENTAR','',0, 0,0,@ICC,'01-01-1','01-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@FR", TxtFactura.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                string VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                string VbOtrosDatos = HttpUtility.HtmlDecode(SDR["OtrosDatos"].ToString().Trim());

                                if (!VbOtrosDatos.Trim().Equals("")) { VbOtrosDatos = " [" + VbOtrosDatos.Trim() + "]"; }

                                if (!VbMensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { VbMensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + VbOtrosDatos + "');", true);
                                    SDR.Close();
                                    Transac.Rollback();
                                    return;
                                }
                            }
                            SDR.Close();
                            Transac.Commit();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Asentar Compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }

                }
            }
            Traerdatos(TxtNumRepa.Text.Trim(), "UPD");
        }
        protected void IbtDesasentar_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNumRepa.Text.Trim().Substring(0, 2).Equals("28"))
            { return; }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_Pantalla_Asentar_Repa 11, @PO, @FR, @US,'DESASENTAR','',0, 0,0,@ICC,'01-01-1','01-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumRepa.Text.Trim());
                        SC.Parameters.AddWithValue("@FR", TxtFactura.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                string VbMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());

                                if (!VbMensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { VbMensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                                    Transac.Rollback();

                                    return;
                                }
                            }
                            SDR.Close();
                            Transac.Commit();
                            Traerdatos(TxtNumRepa.Text.Trim(), "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Desasentar Reparación", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
    }
}