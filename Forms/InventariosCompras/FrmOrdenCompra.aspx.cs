using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmOrdenCompra : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable TblDetalle = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSTPpl = new DataSet();
        DataTable DTBusqCotiza = new DataTable();
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
                ViewState["CodTerceroAnt"] = "";
                ViewState["TipoPagoAnt"] = "";
                ViewState["LugarEAnt"] = "";
                ViewState["LugarFacAnt"] = "";
                ViewState["TtlRegDet"] = 0; // saber si el detalle tiene registro para realizar carga masiva
                ViewState["CarpetaCargaMasiva"] = "";// para mostrar en el boton de carga masiva la ruta por defecto donde se debe guardar el archivo para subir
                ViewState["Monto"] = "0";
                ViewState["ValorIva"] = "0";
                ViewState["DocAprobado"] = "N";
                ViewState["PeriodCerrado"] = "N";
                ViewState["ShipLiquidada"] = "N";
                ViewState["TieneSOMvtoAlma"] = "N";
                ViewState["AutorizadAnt"] = "";
                ViewState["AutorizadPpal"] = "";
                ViewState["CodTransprtAnt"] = "";
                ModSeguridad();
                RdbMdlOpcBusqCotizNum.Checked = true;
                RdbOpcMdlBusqCompra.Checked = true;
                BindBDdl("UPD");
                AddCamposDataTable("INS");
                EnablGridDet("Visible", false); /**/

                BotonesCompr_Intercb("C");

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
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; BtnAsentar.Visible = false; } // asentar
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
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
                    LblFecha.Text = bO.Equals("FechaMstr") ? bT : LblFecha.Text;
                    LblDatosPpt.Text = bO.Equals("LblDatosPpt") ? bT : LblDatosPpt.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnAuxiliares.Text = bO.Equals("BtnAuxiliares") ? bT : BtnAuxiliares.Text;
                    BtnAuxiliares.ToolTip = bO.Equals("BtnAuxiliaresTT") ? bT : BtnAuxiliares.ToolTip;
                    BtnCompra.Text = bO.Equals("LblNumCompraC") ? bT : BtnCompra.Text;
                    BtnInterc.Text = bO.Equals("LblNumCompraI") ? bT : BtnInterc.Text;
                    BtnAsentar.Text = bO.Equals("BtnAsentar") ? bT : BtnAsentar.Text;
                    BtnAsentar.ToolTip = bO.Equals("BtnAsentarTT") ? bT : BtnAsentar.ToolTip;
                    BtnImprimir.Text = bO.Equals("BtnImprimirGrl") ? bT : BtnImprimir.Text;
                    BtnOpenCotiza.Text = bO.Equals("BtnOpenCotiza") ? bT : BtnOpenCotiza.Text;
                    BtnOpenCotiza.ToolTip = bO.Equals("BtnOpenCotizaTT") ? bT : BtnOpenCotiza.ToolTip;
                    LblProvee.Text = bO.Equals("LblProvee") ? bT : LblProvee.Text;
                    LblEmplead.Text = bO.Equals("LblEmplead") ? bT : LblEmplead.Text;
                    LblAutoriz.Text = bO.Equals("LblAutoriz") ? bT : LblAutoriz.Text;
                    LblMoned.Text = bO.Equals("LblMoned") ? bT : LblMoned.Text;
                    LblTipo.Text = bO.Equals("TipoMstr") ? bT : LblTipo.Text;
                    LblTransp.Text = bO.Equals("LblTransp") ? bT : LblTransp.Text;
                    LblTipoPago.Text = bO.Equals("LblTipoPago") ? bT : LblTipoPago.Text;
                    LblEstd.Text = bO.Equals("LblEstadoMst") ? bT : LblEstd.Text;
                    LblUbicac.Text = bO.Equals("LblUbicac") ? bT : LblUbicac.Text;
                    LblEnvioFact.Text = bO.Equals("LblEnvioFact") ? bT : LblEnvioFact.Text;
                    LblFacReferc.Text = bO.Equals("InfCotRef") ? bT : LblFacReferc.Text;
                    LblFactura.Text = bO.Equals("InfFactRef") ? bT : LblFactura.Text;
                    LblObsrv.Text = bO.Equals("LblObsMst") ? bT : LblObsrv.Text;
                    CkbAprobad.Text = bO.Equals("AprobadoMstr") ? bT : CkbAprobad.Text;
                    CkbAsentada.Text = bO.Equals("CkbAsentada") ? bT : CkbAsentada.Text;
                    LblSubTtal.Text = bO.Equals("LblSubTtal") ? bT : LblSubTtal.Text;
                    LblIVA.Text = bO.Equals("LblIVA") ? bT : LblIVA.Text;
                    LblOtrImpt.Text = bO.Equals("LblOtrImpt") ? bT : LblOtrImpt.Text;
                    LblRetencion.Text = bO.Equals("LblRetencion") ? bT : LblRetencion.Text;
                    LblIca.Text = bO.Equals("LblIca") ? bT : LblIca.Text;
                    LblDescto.Text = bO.Equals("LblDescto") ? bT : LblDescto.Text;
                    // *************************************************Modal Busq*************************************************
                    if (bO.Equals("placeholder"))
                    { TxtModalBusq.Attributes.Add("placeholder", bT); }
                    LblTitModalBusqCompra.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitModalBusqCompra.Text;
                    RdbOpcMdlBusqPrv.Text = bO.Equals("LblProvee") ? bT : RdbOpcMdlBusqPrv.Text;
                    RdbOpcMdlBusqPPT.Text = bO.Equals("LblDatosPpt") ? bT : RdbOpcMdlBusqPPT.Text;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblModalBusq.Text;
                    IbtModalBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtModalBusq.ToolTip;
                    BtnCloseModalBusqCompra.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqCompra.Text;
                    GrdModalBusqCompra.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqCompra.EmptyDataText;
                    GrdModalBusqCompra.Columns[1].HeaderText = bO.Equals("LblNumCompraC") ? bT : GrdModalBusqCompra.Columns[1].HeaderText;
                    GrdModalBusqCompra.Columns[2].HeaderText = bO.Equals("LblProvee") ? bT : GrdModalBusqCompra.Columns[2].HeaderText;
                    GrdModalBusqCompra.Columns[3].HeaderText = bO.Equals("FechaMstr") ? bT : GrdModalBusqCompra.Columns[3].HeaderText;
                    GrdModalBusqCompra.Columns[4].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdModalBusqCompra.Columns[4].HeaderText;
                    GrdModalBusqCompra.Columns[5].HeaderText = bO.Equals("LblMoned") ? bT : GrdModalBusqCompra.Columns[5].HeaderText;
                    GrdModalBusqCompra.Columns[6].HeaderText = bO.Equals("LblDatosPpt") ? bT : GrdModalBusqCompra.Columns[6].HeaderText;

                    LblTitModalBusqCotiza.Text = bO.Equals("LblTitModalBusqCotiza") ? bT : LblTitModalBusqCotiza.Text;
                    RdbMdlOpcBusqCotizNum.Text = bO.Equals("BtnOpenCotiza") ? bT : RdbMdlOpcBusqCotizNum.Text;
                    RdbMdlOpcBusqCotizPrv.Text = bO.Equals("LblProvee") ? bT : RdbMdlOpcBusqCotizPrv.Text;
                    IbtAprDetAll.ToolTip = bO.Equals("IbtAprDetAll") ? bT : IbtAprDetAll.ToolTip;
                    GrdModalBusqCot.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqCot.EmptyDataText;
                    GrdModalBusqCot.Columns[1].HeaderText = bO.Equals("LblProvee") ? bT : GrdModalBusqCot.Columns[1].HeaderText;
                    GrdModalBusqCot.Columns[2].HeaderText = bO.Equals("BtnOpenCotiza") ? bT : GrdModalBusqCot.Columns[2].HeaderText;
                    GrdModalBusqCot.Columns[5].HeaderText = bO.Equals("Descripcion") ? bT : GrdModalBusqCot.Columns[5].HeaderText;
                    GrdModalBusqCot.Columns[6].HeaderText = bO.Equals("CantMst") ? bT : GrdModalBusqCot.Columns[6].HeaderText;
                    GrdModalBusqCot.Columns[7].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdModalBusqCot.Columns[7].HeaderText;
                    GrdModalBusqCot.Columns[8].HeaderText = bO.Equals("GrdVlrUnd") ? bT : GrdModalBusqCot.Columns[8].HeaderText;
                    GrdModalBusqCot.Columns[9].HeaderText = bO.Equals("GrdVlrTotal") ? bT : GrdModalBusqCot.Columns[9].HeaderText;
                    BtnAsignarModal.Text = bO.Equals("BtnAsignarModal") ? bT : BtnAsignarModal.Text;
                    // *************************************************Grid detalle *************************************************
                    GrdDet.Columns[1].HeaderText = bO.Equals("BtnOpenCotiza") ? bT : GrdDet.Columns[1].HeaderText;
                    GrdDet.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdDet.Columns[3].HeaderText;
                    GrdDet.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdDet.Columns[4].HeaderText;
                    GrdDet.Columns[5].HeaderText = bO.Equals("GrdCantRec") ? bT : GrdDet.Columns[5].HeaderText;
                    GrdDet.Columns[6].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDet.Columns[6].HeaderText;
                    GrdDet.Columns[7].HeaderText = bO.Equals("GrdVlrUnd") ? bT : GrdDet.Columns[7].HeaderText;
                    // ************************************************* Exportar *************************************************
                    LblTitExport.Text = bO.Equals("BtnExportMstr") ? bT : LblTitExport.Text;
                    IbtCloseExport.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseExport.ToolTip;
                    BtnExportHistorico.Text = bO.Equals("BtnExportHistorico") ? bT : BtnExportHistorico.Text;
                    BtnExportHistorico.ToolTip = bO.Equals("BtnExportHistoricoTT") ? bT : BtnExportHistorico.ToolTip;
                    // ************************************************* Aprobar / Asentar *************************************************
                    LblTitAsentar.Text = bO.Equals("LblTitAsentar") ? bT : LblTitAsentar.Text;
                    IbtCloseAsentar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseAsentar.ToolTip;
                    LblTitOpcAprob.Text = bO.Equals("LblTitOpcAprob") ? bT : LblTitOpcAprob.Text;
                    LblTitOpcAsentr.Text = bO.Equals("BtnAsentar") ? bT : LblTitOpcAsentr.Text;
                    IbtAprobar.ToolTip = bO.Equals("IbtAprobar") ? bT : IbtAprobar.ToolTip;
                    IbtDesAprobar.ToolTip = bO.Equals("IbtDesAprobar") ? bT : IbtDesAprobar.ToolTip;
                    IbtAsentar.ToolTip = bO.Equals("BtnAsentar") ? bT : IbtAsentar.ToolTip;
                    IbtDesasentar.ToolTip = bO.Equals("IbtDesasentar") ? bT : IbtDesasentar.ToolTip;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'Mens18Compra'");
                foreach (DataRow row in Result) { IbtAprobar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }//Desea aprobar la compra?

                Result = Idioma.Select("Objeto= 'Mens19Compra'");
                foreach (DataRow row in Result) { IbtDesAprobar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }// Desea desaprobar la compra?

                Result = Idioma.Select("Objeto= 'Mens20Compra'");
                foreach (DataRow row in Result) { IbtAsentar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }//Desea asentar la compra?

                Result = Idioma.Select("Objeto= 'Mens21Compra'");
                foreach (DataRow row in Result) { IbtDesasentar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }// Desea revertir el asiento la compra?

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDet.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[9].Controls.Remove(imgD);
                    }
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
        protected void BindDdlAutorizado()
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Autorizado"].Rows.Count > 0)
            {
                //DSTDdl = (DataSet)ViewState["DSTDdl"];
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
        protected void BindDdlLugarEntrega(string TipoCompra)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["LugarEntrg"].Rows.Count > 0)
            {
                string VbQry = "";
                DataTable DT = new DataTable();
                if (TipoCompra.Equals("ALL")) { VbQry = "Activo=1 OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }
                if (TipoCompra.Equals("N")) { VbQry = "Activo=1 AND TipoUbicacion ='N' OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }
                if (TipoCompra.Equals("I")) { VbQry = "Activo=1 AND TipoUbicacion ='I' OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'"; }
                DataRow[] DR = DSTDdl.Tables[6].Select(VbQry);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlUbicac.DataSource = DT;
                DdlUbicac.DataTextField = "Descripcion";
                DdlUbicac.DataValueField = "CodIdTipoUbicaCia";
                DdlUbicac.DataBind();
                DdlUbicac.SelectedValue = ViewState["LugarEAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["LugarEntrg"].Rows.Count > 0)
            {
                string VbQry = "";
                DataTable DT = new DataTable();
                if (TipoCompra.Equals("ALL")) { VbQry = "Activo=1 OR CodIdTipoUbicaFac= '" + ViewState["LugarFacAnt"] + "'"; }
                if (TipoCompra.Equals("N")) { VbQry = "Activo=1 AND TipoUbicacionNI ='N' OR CodIdTipoUbicaFac= '" + ViewState["LugarFacAnt"] + "'"; }
                if (TipoCompra.Equals("I")) { VbQry = "Activo=1 AND TipoUbicacionNI ='I' OR CodIdTipoUbicaFac= '" + ViewState["LugarFacAnt"] + "'"; }
                DataRow[] DR = DSTDdl.Tables[7].Select(VbQry);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlEnvioFact.DataSource = DT;
                DdlEnvioFact.DataTextField = "Descripcion";
                DdlEnvioFact.DataValueField = "CodIdTipoUbicaFac";
                DdlEnvioFact.DataBind();
                DdlEnvioFact.SelectedValue = ViewState["LugarFacAnt"].ToString().Trim();
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_OrdenCompra 24,'2500000005','','','DDL',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
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
                                DSTDdl.Tables[7].TableName = "LugarFactura";
                                DSTDdl.Tables[8].TableName = "TipoPago";

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
            if (DSTDdl.Tables["TipoPago"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[8].Select("Activo=1 OR CodTipoPago= '" + ViewState["TipoPagoAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlTipoPago.DataSource = DT;
                DdlTipoPago.DataTextField = "Descripcion";
                DdlTipoPago.DataValueField = "CodTipoPago";
                DdlTipoPago.DataBind();
                DdlTipoPago.SelectedValue = ViewState["TipoPagoAnt"].ToString().Trim();
            }
        }
        protected void AddCamposDataTable(string Accion)
        {
            if (Accion.Equals("INS"))// Nuevo los campos como se llaman en la grid
            {
                TblDetalle.Columns.Add("Posicion", typeof(int));//0
                TblDetalle.Columns.Add("CodCotizacion", typeof(string));//
                TblDetalle.Columns.Add("CodReferencia", typeof(string));//2
                TblDetalle.Columns.Add("PN", typeof(string));//
                TblDetalle.Columns.Add("Descripcion", typeof(string));//4
                TblDetalle.Columns.Add("Cant", typeof(double));//
                TblDetalle.Columns.Add("CantRecibida", typeof(double));//6
                TblDetalle.Columns.Add("Und", typeof(string));//
                TblDetalle.Columns.Add("ValorUnidad", typeof(double));//8
                TblDetalle.Columns.Add("ValorTotal", typeof(double));
                TblDetalle.Columns.Add("CodOrdenCompra", typeof(string));
                TblDetalle.Columns.Add("CODPRIORIDAD", typeof(string));//12
                TblDetalle.Columns.Add("Ccostos", typeof(string));
                TblDetalle.Columns.Add("Nombre", typeof(string));//14
                TblDetalle.Columns.Add("EstadoDES", typeof(string));//               
                TblDetalle.Columns.Add("ShippingOrder", typeof(int));//16
                TblDetalle.Columns.Add("Recibido", typeof(int));//17
                TblDetalle.Columns.Add("IdDetOrdenCompra", typeof(int));//18
                TblDetalle.Columns.Add("IdCotizacion", typeof(int));//
                TblDetalle.Columns.Add("IdDetCotizacion", typeof(int));//20
                TblDetalle.Columns.Add("CodProveedor", typeof(string));//20
                TblDetalle.Columns.Add("TasaIVA", typeof(double));//20
                TblDetalle.Columns.Add("ValorIVA", typeof(double));//20
                TblDetalle.Columns.Add("AccionDet", typeof(string));//20

                ViewState["TblDetalle"] = TblDetalle;
            }
        }
        protected void EnablGridDet(string Propiedad, bool TF)
        {
            if (Propiedad.Equals("Visible"))
            { GrdDet.Visible = TF; }

            if (Propiedad.Equals("Enabled"))
            { GrdDet.Enabled = TF; }
        }
        protected void Traerdatos(string CodComInterc, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_PANTALLA_OrdenCompra 24, @Cod, @NT, @Tip,'',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Cod", CodComInterc);
                            SC.Parameters.AddWithValue("@NT", Session["Nit77Cia"]);
                            SC.Parameters.AddWithValue("@Tip", DdlTipo.Text.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "Compra";
                                    DSTPpl.Tables[1].TableName = "DetCompra";
                                    DSTPpl.Tables[2].TableName = "EncImpresion";

                                    ViewState["DSTPpl"] = DSTPpl;
                                }
                            }
                        }
                    }
                }
                TxtModalBusq.Text = "";
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                if (DSTPpl.Tables["Compra"].Rows.Count > 0)
                {
                    TxtNumCompra.Text = DSTPpl.Tables[0].Rows[0]["CodOrdenCompra"].ToString().Trim();
                    // string VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaOC"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaOC"].ToString().Trim();
                    // DateTime VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecha.Text = Cnx.ReturnFecha(DSTPpl.Tables[0].Rows[0]["FechaOCMDY"].ToString().Trim());

                    TxtTtl.Text = DSTPpl.Tables[0].Rows[0]["ValorTotalM"].ToString().Trim();
                    ViewState["CodTerceroAnt"] = DSTPpl.Tables[0].Rows[0]["CodProveedor"].ToString().Trim();
                    DdlEmplead.Text = DSTPpl.Tables[0].Rows[0]["CodEmpleado"].ToString().Trim();
                    ViewState["AutorizadAnt"] = DSTPpl.Tables[0].Rows[0]["CodAutorizador"].ToString().Trim();
                    ViewState["Total"] = DSTPpl.Tables[0].Rows[0]["ValorTotal"].ToString().Trim();
                    DdlTipo.Text = DSTPpl.Tables[0].Rows[0]["TipoOrdenCompra"].ToString().Trim();
                    ViewState["CodTransprtAnt"] = DSTPpl.Tables[0].Rows[0]["CodTransportador"].ToString().Trim();
                    BindDdlAutorizado();
                    ViewState["TipoPagoAnt"] = DSTPpl.Tables[0].Rows[0]["CodTipoPago"].ToString().Trim();
                    DdlEstd.Text = DSTPpl.Tables[0].Rows[0]["CodEstadoCompra"].ToString().Trim();
                    ViewState["LugarEAnt"] = DSTPpl.Tables[0].Rows[0]["CodUbicaCia"].ToString().Trim();
                    ViewState["LugarFacAnt"] = DSTPpl.Tables[0].Rows[0]["CodIdTipoUbicaFac"].ToString().Trim();
                    TxtFacReferc.Text = DSTPpl.Tables[0].Rows[0]["Referencia"].ToString().Trim();// cotizacion referencia
                    TxtFactura.Text = DSTPpl.Tables[0].Rows[0]["NumFacturaOC"].ToString().Trim();
                    TxtObsrv.Text = DSTPpl.Tables[0].Rows[0]["Observacion"].ToString().Trim();
                    CkbAprobad.Checked = DSTPpl.Tables[0].Rows[0]["Aprobado"].ToString().Trim().Equals("1") ? true : false;
                    CkbAsentada.Checked = DSTPpl.Tables[0].Rows[0]["Asentado"].ToString().Trim().Equals("1") ? true : false;
                    TxtSubTtal.Text = DSTPpl.Tables[0].Rows[0]["MontoM"].ToString().Trim();
                    TxtIVA.Text = DSTPpl.Tables[0].Rows[0]["ValorIVAM"].ToString().Trim();
                    TxtOtrImptM.Text = DSTPpl.Tables[0].Rows[0]["ValorOtrosImpM"].ToString().Trim();
                    TxtOtrImpt.Text = DSTPpl.Tables[0].Rows[0]["ValorOtrosImp"].ToString().Trim();
                    TxtTasaRetefte.Text = DSTPpl.Tables[0].Rows[0]["TasaRetencion"].ToString().Trim();
                    TxtRetefteM.Text = DSTPpl.Tables[0].Rows[0]["ValorRetencionM"].ToString().Trim();
                    TxtRetefte.Text = DSTPpl.Tables[0].Rows[0]["ValorRetencion"].ToString().Trim();
                    TxtTasaICA.Text = DSTPpl.Tables[0].Rows[0]["TasaIca"].ToString().Trim();
                    TxtICAM.Text = DSTPpl.Tables[0].Rows[0]["ValorICAM"].ToString().Trim();
                    TxtICA.Text = DSTPpl.Tables[0].Rows[0]["ValorICA"].ToString().Trim();
                    TxtTasaDescto.Text = DSTPpl.Tables[0].Rows[0]["TasaDescuento"].ToString().Trim();
                    TxtDesctoM.Text = DSTPpl.Tables[0].Rows[0]["ValorDescuentoM"].ToString().Trim();
                    TxtDescto.Text = DSTPpl.Tables[0].Rows[0]["ValorDescuento"].ToString().Trim();
                    ViewState["TtlRegDet"] = DSTPpl.Tables[0].Rows[0]["TtlRegDet"].ToString().Trim();
                    if (CkbAsentada.Checked == true) { IbtAprobar.Visible = false; IbtDesAprobar.Visible = false; IbtAsentar.Visible = false; IbtDesasentar.Visible = true; }
                    else
                    {
                        if (CkbAprobad.Checked == true) { IbtAprobar.Visible = false; IbtDesAprobar.Visible = true; IbtAsentar.Visible = true; IbtDesasentar.Visible = false; }
                        else { IbtAprobar.Visible = true; IbtDesAprobar.Visible = false; IbtAsentar.Visible = false; IbtDesasentar.Visible = false; }
                    }
                    BindBDdl("SEL");
                }
                if (DSTPpl.Tables["DetCompra"].Rows.Count > 0)
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    DataRow[] DR = DSTPpl.Tables[1].Select("CodCotizacion <>''");
                    if (IsIENumerableLleno(DR))
                    { TblDetalle = DR.CopyToDataTable(); TblDetalle.AcceptChanges(); ViewState["TblDetalle"] = TblDetalle; }
                }
                else { TblDetalle.Clear(); TblDetalle.AcceptChanges(); }
                if (TblDetalle.Rows.Count > 0) { }
                BindDDetTmp();
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
        protected void LimpiarCampos(string Accion)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            TxtNumCompra.Text = "";
            TxtTtl.Text = "0";
            TxtFecha.Text = "";
            TxtDatosPpt.Text = "";
            DdlProvee.Text = "";
            DdlEmplead.Text = Session["C77U"].ToString().Trim();
            DdlAutoriz.Text = ViewState["AutorizadPpal"].ToString().Trim();
            TxtMoned.Text = "";
            DdlTipo.Text = "";
            DdlTransp.Text = "";
            DdlTipoPago.Text = "";
            DdlEstd.Text = "01";
            DdlUbicac.Text = "";
            DdlEnvioFact.Text = "";
            TxtFacReferc.Text = "";
            TxtFactura.Text = "";
            TxtObsrv.Text = "";
            CkbAprobad.Checked = false;
            CkbAsentada.Checked = false;
            TxtSubTtal.Text = "0";
            TxtIVA.Text = "0";
            TxtOtrImptM.Text = "0";
            TxtOtrImpt.Text = "0";
            TxtTasaRetefte.Text = "0";
            TxtRetefteM.Text = "0";
            TxtRetefte.Text = "0";
            TxtTasaICA.Text = "0";
            TxtICAM.Text = "0";
            TxtICA.Text = "0";
            TxtTasaDescto.Text = "0";
            TxtDesctoM.Text = "0";
            TxtDescto.Text = "0";
            TblDetalle.Clear();
            TblDetalle.AcceptChanges();
            BindDDetTmp();
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (DdlProvee.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el proveedor.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el tipo.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTransp.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el transportador.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipoPago.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo de pago.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlUbicac.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el lugar de entrega.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlEnvioFact.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el lugar de envío de la factura.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool Cnslt, bool El, bool Ip, bool Otr, string Accion)
        {
            BtnCompra.Enabled = Md;
            BtnInterc.Enabled = Md;
            BtnConsultar.Enabled = Cnslt;
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnAuxiliares.Enabled = Otr;
            BtnAsentar.Enabled = Otr;
            BtnImprimir.Enabled = Ip;
            BtnOpenCotiza.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DdlAutoriz.Enabled = Edi;
            DdlTransp.Enabled = Edi;
            DdlTipoPago.Enabled = Edi;
            if (Accion.Equals("UPD")) { DdlEstd.Enabled = Edi; }
            DdlUbicac.Enabled = Edi;
            DdlEnvioFact.Enabled = Edi;
            // TxtFacReferc.Enabled = Edi;
            TxtFactura.Enabled = Edi;
            TxtObsrv.Enabled = Edi;
            if (CkbAprobad.Checked == false)
            {
                TxtOtrImptM.Visible = Edi == true ? false : true; TxtOtrImpt.Visible = Edi; TxtOtrImpt.Enabled = Edi;
                TxtRetefteM.Visible = Edi == true ? false : true; TxtRetefte.Visible = Edi; TxtTasaRetefte.Enabled = Edi;
                TxtICAM.Visible = Edi == true ? false : true; TxtICA.Visible = Edi; TxtTasaICA.Enabled = Edi;
                TxtDesctoM.Visible = Edi == true ? false : true; TxtDescto.Visible = Edi; TxtTasaDescto.Enabled = Edi;
            }
        }
        protected void BotonesCompr_Intercb(string Tipo)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["TtlRegDet"] = 0; //LimpiarCampos("DEL"); 
            BtnCompra.CssClass = "btn btn-outline-primary";
            BtnInterc.CssClass = "btn btn-outline-primary";
            ViewState["EsCompra_Intercb"] = Tipo;
            if (Tipo.Equals("C"))
            {
                BtnCompra.CssClass = "btn btn-primary";
                DataRow[] Result = Idioma.Select("Objeto= 'LblNumCompraC'");
                foreach (DataRow row in Result) { LblNumCompra.Text = row["Texto"].ToString().Trim() + " No."; RdbOpcMdlBusqCompra.Text = "&nbsp" + row["Texto"].ToString().Trim() + " No."; }
            }
            else
            {
                BtnInterc.CssClass = "btn btn-primary";
                DataRow[] Result = Idioma.Select("Objeto= 'LblNumCompraI'");
                foreach (DataRow row in Result) { LblNumCompra.Text = row["Texto"].ToString().Trim() + " No."; RdbOpcMdlBusqCompra.Text = "&nbsp" + row["Texto"].ToString().Trim() + " No."; }
            }
            GrdModalBusqCompra.DataSource = null; GrdModalBusqCompra.DataBind();
            LimpiarCampos("");
            MultVw.ActiveViewIndex = 0;
        }
        protected void BtnCompra_Click(object sender, EventArgs e)
        { BotonesCompr_Intercb("C"); }
        protected void BtnInterc_Click(object sender, EventArgs e)
        { BotonesCompr_Intercb("I"); }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (ViewState["Accion"].ToString().Trim().Equals(""))// consulta las compras
            {
                LblTitModalBusqCompra.Visible = true; TblMdlOpcBusCompra.Visible = true; GrdModalBusqCompra.Visible = true;
                LblTitModalBusqCotiza.Visible = false; TblMdlOpcBusCotiza.Visible = false; GrdModalBusqCot.Visible = false; IbtAprDetAll.Visible = false; BtnAsignarModal.Visible = false;
            }
            else
            {
                LblTitModalBusqCompra.Visible = false; TblMdlOpcBusCompra.Visible = false; GrdModalBusqCompra.Visible = false;
                LblTitModalBusqCotiza.Visible = true; TblMdlOpcBusCotiza.Visible = true; GrdModalBusqCot.Visible = true; IbtAprDetAll.Visible = true; BtnAsignarModal.Visible = true;
                BindModalBusqCot();
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, true, false, false, false, "INS");
                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(true, true, "INS");
                    LimpiarCampos("INS");

                    string VbD = Convert.ToString(DateTime.UtcNow.Day);
                    string VbM = Convert.ToString(DateTime.UtcNow.Month);
                    string VbY = Convert.ToString(DateTime.UtcNow.Year);
                    string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, VbD);
                    DateTime VbFecID = Convert.ToDateTime(fecha);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);

                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                    EnablGridDet("Enabled", true);
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    TblDetalle.AcceptChanges();
                    foreach (DataRow row in TblDetalle.Rows)
                    {
                        object value = row["PN"];
                        if (value == DBNull.Value)
                        {
                            if (TblDetalle.Rows.Count > 0) { row.Delete(); }
                        }
                    }
                    TblDetalle.AcceptChanges();
                    Valores();
                    List<ClsTypCompra> ObjEncCom = new List<ClsTypCompra>();
                    var TypEncCom = new ClsTypCompra()
                    {
                        CodOrdenCompra = "",
                        CodProveedor = DdlProvee.Text.Trim(),
                        CodEmpleado = DdlEmplead.Text.Trim(),
                        CodAutorizador = DdlAutoriz.Text.Trim(),
                        CodMoneda = TxtMoned.Text.Trim(),
                        TipoOrdenCompra = DdlTipo.Text.Trim(),
                        CodTransportador = DdlTransp.Text.Trim(),
                        CodTipoPago = DdlTipoPago.Text.Trim(),
                        CodUbicaCia = DdlUbicac.Text.Trim(),
                        FechaOC = Convert.ToDateTime(TxtFecha.Text.Trim()),
                        CodEstadoCompra = DdlEstd.Text.Trim(),
                        Monto = Convert.ToDouble(TxtSubTtal.Text.Trim().Equals("") ? "0" : TxtSubTtal.Text.Trim()),
                        TasaIva = Convert.ToDouble(0),
                        ValorIVA = Convert.ToDouble(TxtIVA.Text.Trim().Equals("") ? "0" : TxtIVA.Text.Trim()),
                        TasaRetencion = Convert.ToDouble(TxtTasaRetefte.Text.Trim().Equals("") ? "0" : TxtTasaRetefte.Text.Trim()),
                        ValorRetencion = Convert.ToDouble(TxtRetefte.Text.Trim().Equals("") ? "0" : TxtRetefte.Text.Trim()),
                        TasaIca = Convert.ToDouble(TxtTasaICA.Text.Trim().Equals("") ? "0" : TxtTasaICA.Text.Trim()),
                        ValorICA = Convert.ToDouble(TxtICA.Text.Trim().Equals("") ? "0" : TxtICA.Text.Trim()),
                        ValorOtrosImp = Convert.ToDouble(TxtOtrImpt.Text.Trim().Equals("") ? "0" : TxtOtrImpt.Text.Trim()),
                        TasaDescuento = Convert.ToDouble(TxtTasaDescto.Text.Trim().Equals("") ? "0" : TxtTasaDescto.Text.Trim()),
                        ValorDescuento = Convert.ToDouble(TxtDescto.Text.Trim().Equals("") ? "0" : TxtDescto.Text.Trim()),
                        ValorTotal = Convert.ToDouble(TxtTtl.Text.Trim().Equals("") ? "0" : TxtTtl.Text.Trim()),
                        Referencia = TxtFacReferc.Text.Trim(),
                        Observacion = TxtObsrv.Text.Trim(),
                        Aprobado = Convert.ToInt32(0),
                        Asentado = Convert.ToInt32(0),
                        Recibido = Convert.ToInt32(0),
                        CuentaPuc = "",
                        CodTipoCodigo = "01",
                        CodIdTipoUbicaFac = DdlEnvioFact.Text.Trim(),
                        NumFacturaOC = TxtFactura.Text.Trim(),
                        CompraIntercambio = ViewState["EsCompra_Intercb"].ToString(),
                    };
                    ObjEncCom.Add(TypEncCom);

                    GrdDet.DataSource = TblDetalle;
                    GrdDet.DataBind();

                    List<ClsTypCompra> ObjDetCom = new List<ClsTypCompra>();
                    foreach (DataRow DR in TblDetalle.Rows)
                    {
                        if (!DR["Pn"].ToString().Trim().Equals(""))
                        {
                            var TypDetCom = new ClsTypCompra()
                            {
                                IdDetOrdenCompra = Convert.ToInt32(DR["IdDetOrdenCompra"].ToString().Trim().Equals("") ? "0" : DR["IdDetOrdenCompra"].ToString().Trim()),
                                FechaRecibo = null,
                                Posicion = Convert.ToInt32(DR["Posicion"].ToString().Trim()),
                                PN = DR["PN"].ToString().Trim(),
                                IdDetCotiza = Convert.ToInt32(DR["IdDetCotizacion"].ToString().Trim()),
                                ShippingOrder = Convert.ToInt32(0),
                                ElementoRecibido = Convert.ToInt32(0),
                                FacturaProveedor = Convert.ToInt32(0),
                                Anticipo = Convert.ToInt32(0),
                                Cant = Convert.ToDouble(DR["Cant"].ToString().Trim()),
                                VlrUnd = Convert.ToDouble(DR["ValorUnidad"].ToString().Trim()),
                                TasaIVA = Convert.ToDouble(DR["TasaIVA"].ToString().Trim()),
                                AccionDet = DR["AccionDet"].ToString().Trim(),
                            };
                            ObjDetCom.Add(TypDetCom);
                        }
                    }
                    ClsTypCompra ClsCompra = new ClsTypCompra();
                    ClsCompra.Accion("INSERT");
                    ClsCompra.Alimentar(ObjEncCom, ObjDetCom);
                    string Mensj = ClsCompra.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, true, "INS");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    Traerdatos(ClsCompra.GetCodCompra().ToString().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNumCompra.Text.Equals(""))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, true, false, false, false, "UPD");
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPD");

                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                    EnablGridDet("Enabled", true);
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    TblDetalle.AcceptChanges();
                    foreach (DataRow row in TblDetalle.Rows)
                    {
                        object value = row["PN"];
                        if (value == DBNull.Value)
                        {
                            if (TblDetalle.Rows.Count > 0) { row.Delete(); }
                        }
                    }

                    TblDetalle.AcceptChanges();
                    Valores();
                    List<ClsTypCompra> ObjEncCom = new List<ClsTypCompra>();
                    var TypEncCom = new ClsTypCompra()
                    {
                        CodOrdenCompra = TxtNumCompra.Text.Trim(),
                        CodProveedor = DdlProvee.Text.Trim(),
                        CodEmpleado = DdlEmplead.Text.Trim(),
                        CodAutorizador = DdlAutoriz.Text.Trim(),
                        CodMoneda = TxtMoned.Text.Trim(),
                        TipoOrdenCompra = DdlTipo.Text.Trim(),
                        CodTransportador = DdlTransp.Text.Trim(),
                        CodTipoPago = DdlTipoPago.Text.Trim(),
                        CodUbicaCia = DdlUbicac.Text.Trim(),
                        FechaOC = Convert.ToDateTime(TxtFecha.Text.Trim()),
                        CodEstadoCompra = DdlEstd.Text.Trim(),
                        Monto = Convert.ToDouble(TxtSubTtal.Text.Trim().Equals("") ? "0" : TxtSubTtal.Text.Trim()),
                        TasaIva = Convert.ToDouble(0),
                        ValorIVA = Convert.ToDouble(TxtIVA.Text.Trim().Equals("") ? "0" : TxtIVA.Text.Trim()),
                        TasaRetencion = Convert.ToDouble(TxtTasaRetefte.Text.Trim().Equals("") ? "0" : TxtTasaRetefte.Text.Trim()),
                        ValorRetencion = Convert.ToDouble(TxtRetefte.Text.Trim().Equals("") ? "0" : TxtRetefte.Text.Trim()),
                        TasaIca = Convert.ToDouble(TxtTasaICA.Text.Trim().Equals("") ? "0" : TxtTasaICA.Text.Trim()),
                        ValorICA = Convert.ToDouble(TxtICA.Text.Trim().Equals("") ? "0" : TxtICA.Text.Trim()),
                        ValorOtrosImp = Convert.ToDouble(TxtOtrImpt.Text.Trim().Equals("") ? "0" : TxtOtrImpt.Text.Trim()),
                        TasaDescuento = Convert.ToDouble(TxtTasaDescto.Text.Trim().Equals("") ? "0" : TxtTasaDescto.Text.Trim()),
                        ValorDescuento = Convert.ToDouble(TxtDescto.Text.Trim().Equals("") ? "0" : TxtDescto.Text.Trim()),
                        ValorTotal = Convert.ToDouble(TxtTtl.Text.Trim().Equals("") ? "0" : TxtTtl.Text.Trim()),
                        Referencia = TxtFacReferc.Text.Trim(),
                        Observacion = TxtObsrv.Text.Trim(),
                        Aprobado = Convert.ToInt32(0),
                        Asentado = Convert.ToInt32(0),
                        Recibido = Convert.ToInt32(0),
                        CuentaPuc = "",
                        CodTipoCodigo = "01",
                        CodIdTipoUbicaFac = DdlEnvioFact.Text.Trim(),
                        NumFacturaOC = TxtFactura.Text.Trim(),
                        CompraIntercambio = ViewState["EsCompra_Intercb"].ToString(),
                    };
                    ObjEncCom.Add(TypEncCom);

                    GrdDet.DataSource = TblDetalle;
                    GrdDet.DataBind();

                    List<ClsTypCompra> ObjDetCom = new List<ClsTypCompra>();
                    foreach (DataRow DR in TblDetalle.Rows)
                    {
                        if (!DR["Pn"].ToString().Trim().Equals(""))
                        {
                            var TypDetCom = new ClsTypCompra()
                            {
                                IdDetOrdenCompra = Convert.ToInt32(DR["IdDetOrdenCompra"].ToString().Trim().Equals("") ? "0" : DR["IdDetOrdenCompra"].ToString().Trim()),
                                FechaRecibo = null,
                                Posicion = Convert.ToInt32(DR["Posicion"].ToString().Trim()),
                                PN = DR["PN"].ToString().Trim(),
                                IdDetCotiza = Convert.ToInt32(DR["IdDetCotizacion"].ToString().Trim()),
                                ShippingOrder = Convert.ToInt32(0),
                                ElementoRecibido = Convert.ToInt32(0),
                                FacturaProveedor = Convert.ToInt32(0),
                                Anticipo = Convert.ToInt32(0),
                                Cant = Convert.ToDouble(DR["Cant"].ToString().Trim()),
                                VlrUnd = Convert.ToDouble(DR["ValorUnidad"].ToString().Trim()),
                                TasaIVA = Convert.ToDouble(DR["TasaIVA"].ToString().Trim()),
                                AccionDet = DR["AccionDet"].ToString().Trim(),
                            };
                            ObjDetCom.Add(TypDetCom);
                        }
                    }
                    ClsTypCompra ClsCompra = new ClsTypCompra();
                    ClsCompra.Accion("UPDATE");
                    ClsCompra.Alimentar(ObjEncCom, ObjDetCom);
                    string Mensj = ClsCompra.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }

                    ActivarBtn(true, true, true, true, true, true, "UPD");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampos(false, false, "UPD");
                    Traerdatos(TxtNumCompra.Text.Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnAuxiliares_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 2; }
        protected void BtnOpenCotiza_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string CT = "window.open('/Forms/InventariosCompras/FrmCotizacion.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), CT, true);
        }
        protected void TxtOtrImpt_TextChanged(object sender, EventArgs e)
        {
            Valores();
            TxtTasaRetefte.Attributes.Add("onfocus", "this.select();");
            TxtTasaRetefte.Focus();
        }
        protected void TxtTasaRetefte_TextChanged(object sender, EventArgs e)
        {
            Valores();
            TxtTasaICA.Attributes.Add("onfocus", "this.select();");
            TxtTasaICA.Focus();
        }
        protected void TxtTasaICA_TextChanged(object sender, EventArgs e)
        {
            Valores();
            TxtTasaDescto.Attributes.Add("onfocus", "this.select();");
            TxtTasaDescto.Focus();
        }
        protected void TxtTasaDescto_TextChanged(object sender, EventArgs e)
        { Valores(); }
        //****************************** MOdal Busq Compra / Cotizacion **************************************
        protected void BindModalBusqCompra()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbOpc = "COM";
                DataTable DT = new DataTable();
                if (RdbOpcMdlBusqCompra.Checked == true) { VbOpc = "COM"; }
                if (RdbOpcMdlBusqPrv.Checked == true) { VbOpc = "PV"; }
                if (RdbOpcMdlBusqPPT.Checked == true) { VbOpc = "PT"; }
                string VbTxtSql = "EXEC SP_PANTALLA_OrdenCompra 23, @Doc, @Opc, @Typ,'',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Doc", TxtModalBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpc);
                    SC.Parameters.AddWithValue("@Typ", ViewState["EsCompra_Intercb"].ToString());
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0) { GrdModalBusqCompra.DataSource = DT; }
                    else { GrdModalBusqCompra.DataSource = null; }
                    GrdModalBusqCompra.DataBind();
                }
            }
        }
        protected void BindModalBusqCot()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                if (RdbMdlOpcBusqCotizNum.Checked == true) { VbTxtSql = "EXEC SP_PANTALLA_OrdenCompra 22,'01', @CdPv,@Doc,'', @Tp,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'"; }
                if (RdbMdlOpcBusqCotizPrv.Checked == true) { VbTxtSql = "EXEC SP_PANTALLA_OrdenCompra 22,'01', @CdPv,'', @Doc,@Tp,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'"; }

                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@CdPv", DdlProvee.Text.Trim());
                    SC.Parameters.AddWithValue("@Doc", TxtModalBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Tp", ViewState["EsCompra_Intercb"].Equals("C") ? 1 : 3);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DTBusqCotiza);
                    if (DTBusqCotiza.Rows.Count > 0) { GrdModalBusqCot.DataSource = DTBusqCotiza; }
                    else { GrdModalBusqCot.DataSource = null; }
                    GrdModalBusqCot.DataBind();
                    ViewState["DTBusqCotiza"] = DTBusqCotiza;
                }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (LblTitModalBusqCompra.Visible == true) { BindModalBusqCompra(); }
            if (LblTitModalBusqCotiza.Visible == true) { BindModalBusqCot(); }

            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void GrdModalBusqCompra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbCodCI = ((Label)row.FindControl("LblCodCompra")).Text.ToString().Trim();
                TxtDatosPpt.Text = GrdModalBusqCompra.DataKeys[gvr.RowIndex].Values["DescPPT"].ToString();
                TxtMoned.Text = ((Label)row.FindControl("LblMoneda")).Text.ToString().Trim();
                DdlTipo.Text = GrdModalBusqCompra.DataKeys[gvr.RowIndex].Values["TipoOrdenCompra"].ToString();
                Traerdatos(VbCodCI, "UPD");
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdModalBusqCompra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIrCot = (e.Row.FindControl("IbtIr") as ImageButton);
                if (IbtIrCot != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtIrCot.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void IbtAprDetAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DTBusqCotiza = (DataTable)ViewState["DTBusqCotiza"];
            if (DTBusqCotiza.Rows.Count > 0)
            {
                foreach (DataRow Dtll in DTBusqCotiza.Rows)
                { Dtll["Pasar"] = "1"; }
                GrdModalBusqCot.DataSource = DTBusqCotiza; GrdModalBusqCot.DataBind();
            }
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void BtnAsignarModal_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DTBusqCotiza = (DataTable)ViewState["DTBusqCotiza"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int VbNumReg = TblDetalle.Rows.Count;
            TblDetalle.AcceptChanges();
            foreach (GridViewRow Row in GrdModalBusqCot.Rows)
            {
                if ((Row.FindControl("CkbA") as CheckBox).Checked == true)
                {
                    string VbCodCot = (Row.FindControl("LblCodCot") as Label).Text.Trim();
                    string VbCodRef = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["CodReferencia"].ToString().Trim();
                    string VbCodPrvdr = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["CodProveedor"].ToString().Trim();
                    string VbPN = (Row.FindControl("LblPn") as Label).Text.Trim();
                    string VbDesc = (Row.FindControl("LblDesc") as Label).Text.Trim();
                    double VbCant = Convert.ToDouble((Row.FindControl("LblCant") as Label).Text.Trim());
                    string VbUndMed = (Row.FindControl("LblUndM") as Label).Text.Trim();
                    double VbVlrUnd = Convert.ToDouble((Row.FindControl("LblVlrUnd") as Label).Text.Trim());
                    double VbVlrTtl = Convert.ToDouble((Row.FindControl("LbVlrTtl") as Label).Text.Trim());
                    int VbIdCot = Convert.ToInt32(GrdModalBusqCot.DataKeys[Row.RowIndex].Values["IdCotizacion"].ToString().Trim());
                    int VbIdDetCot = Convert.ToInt32(GrdModalBusqCot.DataKeys[Row.RowIndex].Values["IdDetCotizacion"].ToString().Trim());
                    int VbTasaIva = Convert.ToInt32(GrdModalBusqCot.DataKeys[Row.RowIndex].Values["TasaIVA"].ToString().Trim());
                    int VbVlrIva = Convert.ToInt32(GrdModalBusqCot.DataKeys[Row.RowIndex].Values["ValorIva"].ToString().Trim());

                    string VbTieneReg = "N";
                    foreach (DataRow row in TblDetalle.Rows)
                    {
                        object value = row["CodProveedor"];
                        string borr = row["CodProveedor"].ToString();
                        if (!row["CodProveedor"].ToString().Equals(""))
                        { VbTieneReg = "S"; break; }
                    }

                    DataRow DRPV = TblDetalle.Select("CodProveedor = '" + VbCodPrvdr + "'").FirstOrDefault(); // Verifica si el proveedor es el mismo de la compra
                    if (DRPV != null || VbTieneReg.Equals("N"))
                    {
                        DataRow dr = TblDetalle.Select("Pn = '" + VbPN + "'").FirstOrDefault(); // Busca si existe el PN y no lo agrega
                        if (dr == null)
                        {
                            TblDetalle.Rows.Add(0, VbCodCot, VbCodRef, VbPN, VbDesc, VbCant, 0, VbUndMed, VbVlrUnd, VbVlrTtl, TxtNumCompra.Text.Trim(), "", "", "", "", 0, 0, 0, VbIdCot, VbIdDetCot,
                                                VbCodPrvdr, VbTasaIva, VbVlrIva, "INS");
                            TblDetalle.AcceptChanges();
                            DdlProvee.Text = VbCodPrvdr;
                            TxtMoned.Text = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["CodMoneda"].ToString().Trim();
                            DdlTipo.Text = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["CodTipoCotizacion"].ToString().Trim();
                            if (DdlTipoPago.Text.Trim().Equals("")) { DdlTipoPago.Text = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["CodTipoPago"].ToString().Trim(); }
                        }
                    }
                }
            }

            BindDDetTmp();
            Valores();
        }
        //****************************** DETALLE Compra **************************************
        protected void BindDDetTmp()
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
            EnablGridDet("Visible", true);

            if (TblDetalle.Rows.Count > 0)
            {
                DataView DV = TblDetalle.DefaultView;
                DV.Sort = "Posicion";
                TblDetalle = DV.ToTable();
                GrdDet.DataSource = TblDetalle; GrdDet.DataBind();
            }
            else
            {
                TblDetalle.Rows.Add(TblDetalle.NewRow());
                GrdDet.DataSource = TblDetalle;
                GrdDet.DataBind();
                GrdDet.Rows[0].Cells.Clear();
                GrdDet.Rows[0].Cells.Add(new TableCell());
                GrdDet.Rows[0].Cells[0].Text = "Empty..!";
                GrdDet.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                TblDetalle.NewRow();
                GrdDet.DataSource = TblDetalle;
                GrdDet.DataBind();
            }
        }
        protected void Valores()
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            var VlrMonto = TblDetalle.AsEnumerable().Sum((b) => { return b.Field<double?>("ValorUnidad") * b.Field<double?>("Cant"); }); //*
            TxtSubTtal.Text = VlrMonto.ToString();

            var VlrIva = TblDetalle.AsEnumerable().Sum((b) => { return b.Field<double?>("ValorIVA"); });
            TxtIVA.Text = VlrIva.ToString();

            var VlorTtlCot = TblDetalle.AsEnumerable().Sum((b) => { return b.Field<double?>("ValorTotal"); });

            var VbOtroImp = TxtOtrImpt.Text.Equals("") ? "0" : TxtOtrImpt.Text;

            var VbTasRteF = TxtTasaRetefte.Text.Equals("") ? "0" : TxtTasaRetefte.Text;
            double VbVlrRteF = (Convert.ToDouble(VbTasRteF) * Convert.ToDouble(VlrMonto)) / 100;
            TxtRetefte.Text = VbVlrRteF.ToString();

            var VbTasDesct = TxtTasaDescto.Text.Equals("") ? "0" : TxtTasaDescto.Text;
            double VbVlrDesct = (Convert.ToDouble(VbTasDesct) * Convert.ToDouble(VlrMonto)) / 100;
            TxtDescto.Text = VbVlrDesct.ToString();

            var VbTasICA = TxtTasaICA.Text.Equals("") ? "0" : TxtTasaICA.Text;
            double VbVlrICA = (Convert.ToDouble(VbTasICA) * Convert.ToDouble(VlrMonto)) / 1000;
            TxtICA.Text = VbVlrICA.ToString();

            double VbTtlCompra = Convert.ToDouble(VlrMonto) + Convert.ToDouble(VlrIva) + Convert.ToDouble(VbOtroImp) - (VbVlrRteF + VbVlrDesct + VbVlrICA);
            TxtTtl.Text = VbTtlCompra.ToString();
        }
        protected void GrdDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int index = Convert.ToInt32(e.RowIndex);
            TblDetalle.Rows[index].Delete();
            BindDDetTmp(); Valores();
        }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

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

                if (CkbAprobad.Checked == true)// Si esta aprobado 
                {
                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto='AprobadoMstr'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }
                }
            }
        }
        //****************************** Imprimir **************************************       
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
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            if (CkbAprobad.Checked == false) { return; }

            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            CampoMultiL();
            DTMultL = (DataTable)ViewState["DTMultL"];
            MultVw.ActiveViewIndex = 1;
            DataRow DR = DTMultL.AsEnumerable().Where(r => ((int)r["ID"]).Equals(0)).First();

            DataRow[] Result = Idioma.Select("Objeto= 'InfDesc1OC'");
            foreach (DataRow row in Result) { DR["MltlC01"] = row["Texto"].ToString().Trim(); }

            Result = Idioma.Select("Objeto= 'InfDesc2OC'");
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

            Result = Idioma.Select("Objeto= 'InfTelef'");
            foreach (DataRow row in Result) { DR["MltlC12"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfEntregMatrl'");
            foreach (DataRow row in Result) { DR["MltlC13"] = row["Texto"].ToString().Trim() + ":"; }

            Result = Idioma.Select("Objeto= 'InfEnviarFac'");
            foreach (DataRow row in Result) { DR["MltlC14"] = row["Texto"].ToString().Trim() + ":"; }

            DR["MltlC15"] = LblTransp.Text; DR["MltlC16"] = LblObsrv.Text;

            Result = Idioma.Select("Objeto= 'InfPrepPor'");
            foreach (DataRow row in Result) { DR["MltlC17"] = row["Texto"].ToString().Trim(); }// Preparado Por

            Result = Idioma.Select("Objeto= 'InfAutorPor'");
            foreach (DataRow row in Result) { DR["MltlC18"] = row["Texto"].ToString().Trim(); }// Firma Autorizada

            Result = Idioma.Select("Objeto= 'InfDe'");
            foreach (DataRow row in Result) { DR["MltlC19"] = row["Texto"].ToString().Trim(); }// De

            Result = Idioma.Select("Objeto= 'InfFactRef'");
            foreach (DataRow row in Result) { DR["MltlC20"] = row["Texto"].ToString().Trim(); }// Factura referencia

            Result = Idioma.Select("Objeto= 'InfCotRef'");
            foreach (DataRow row in Result) { DR["MltlC21"] = row["Texto"].ToString().Trim(); }//Cotización referencia

            DR["MltlC22"] = LblSubTtal.Text; DR["MltlC23"] = LblOtrImpt.Text; DR["MltlC24"] = LblRetencion.Text; DR["MltlC25"] = LblIca.Text; DR["MltlC26"] = LblDescto.Text;


            Result = Idioma.Select("Objeto= 'InfFecTrm'");
            foreach (DataRow row in Result) { DR["MltlC27"] = row["Texto"].ToString().Trim(); }//Fecha TRM:

            DR["MltlC28"] = GrdDet.Columns[4].HeaderText;//cant
            DR["MltlC29"] = GrdDet.Columns[3].HeaderText;//desc
            DR["MltlC30"] = GrdDet.Columns[6].HeaderText;//und
            DR["MltlC31"] = GrdDet.Columns[6].HeaderText;//valor und
            if (Session["FormatFecha"].ToString().Equals("101")) { DR["MltlC32"] = "MM/dd/yyyy HH:mm"; }
            else { { DR["MltlC32"] = "dd/MM/yyyy HH:mm"; } }
            //valor und

            DTMultL.AcceptChanges();
            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            Cnx.SelecBD();
            ReportParameter[] parameters = new ReportParameter[3];

            parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
            parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
            parameters[2] = new ReportParameter("PrmImg", VbLogo, true);

            RpVwAll.LocalReport.EnableExternalImages = true;

            RpVwAll.LocalReport.ReportPath = "Report/Logistica/Inf_ImprsOrdenCompra.rdlc";
            RpVwAll.LocalReport.DataSources.Clear();
            RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DSTPpl.Tables[1]));
            RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DTMultL));
            RpVwAll.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", DSTPpl.Tables[2]));
            RpVwAll.LocalReport.SetParameters(parameters);
            RpVwAll.LocalReport.Refresh();
        }
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        //****************************** Exportar **************************************       
        protected void IbtCloseExport_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BtnExportHistorico_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString();
            string StSql, VbNomRpt = "";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                CursorIdioma.Alimentar("CurExportHistorialCompra", Session["77IDM"].ToString().Trim());
                StSql = "EXEC SP_PANTALLA_OrdenCompra 9,'','','','CurExportHistorialCompra',0,0, @Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                using (SqlCommand SC = new SqlCommand(StSql, con))
                {
                    VbNomRpt = BtnExportHistorico.ToolTip;

                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.CommandTimeout = 90000000;
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
        //****************************** Aprobar / Asentar **************************************       
        protected void BtnAsentar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNumCompra.Text.Equals(""))
            { return; }

            if (ViewState["TtlRegDet"].ToString().Trim().Equals("0"))
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
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens17Compra'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Acceso denegado, verificar configuración aprobacion compras.
                        return;
                    }
                }
            }

            MultVw.ActiveViewIndex = 3;

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
                    string VBQuery = "EXEC SP_PANTALLA_Asentar_OrdenCompra 16, @PO, @FR, @US,'APROBAR',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumCompra.Text.Trim());
                        SC.Parameters.AddWithValue("@FR", TxtFactura.Text.Trim());
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
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Aprobar Compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }

                }
            }
            Traerdatos(TxtNumCompra.Text.Trim(), "UPD");
            if (VbEjecPlano.Trim().Equals("S"))
            {
                Cnx.SelecBD();
                using (SqlConnection SCXP = new SqlConnection(Cnx.GetConex()))
                {
                    SCXP.Open();
                    string VBQuery = "EXEC SP_PANTALLA_Asentar_OrdenCompra 17, @PO, '', @US,'APROBAR',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCXP))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumCompra.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        { SC.ExecuteNonQuery(); }
                        catch (Exception ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); } //Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Nuevo P/N", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                    string VBQuery = "EXEC SP_PANTALLA_Asentar_OrdenCompra 16, @PO, @FR, @US,'DESAPROBAR',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumCompra.Text.Trim());
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
                            Traerdatos(TxtNumCompra.Text.Trim(), "UPD");
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
                DataRow[] Result = Idioma.Select("Objeto= 'Mens16Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar  la factura de referencia.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_Asentar_OrdenCompra 18, @PO, @FR, @US,'ASENTAR',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumCompra.Text.Trim());
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
            Traerdatos(TxtNumCompra.Text.Trim(), "UPD");
        }
        protected void IbtDesasentar_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNumCompra.Text.Trim().Substring(0, 2).Equals("33"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens24Compra'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar  la factura de referencia.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction Transac = SCX.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_Asentar_OrdenCompra 18, @PO, @FR, @US,'DESASENTAR',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, Transac))
                    {
                        SC.Parameters.AddWithValue("@PO", TxtNumCompra.Text.Trim());
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
                            Traerdatos(TxtNumCompra.Text.Trim(), "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Desasentar compra", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
    }
}