using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmConsultaMovimientos : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPSL = new DataSet();
        DataSet DSSM = new DataSet();
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
                ViewState["CodReferencia"] = "";
                ViewState["TitExprtMvts"] = "Movimientos Almacen";
                ViewState["IdimaPnALter"] = "Partes alternos de la referencia";
                ModSeguridad();
                BindBDdl("", "", "", "UPD");
                RdbMdlBusqPN.Checked = true;
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
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; } // grd.ShowFooter = false;
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // Excel
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//VISUALIZAR HIST
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//EDITAR OBSERVA
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }// Exportar valores        

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
                    LblLote.Text = bO.Equals("LoteMst") ? bT : LblLote.Text;
                    LblTipo.Text = bO.Equals("TipoMstr") ? bT : LblTipo.Text;
                    LblDescrPn.Text = bO.Equals("Descripcion") ? bT : LblDescrPn.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    BtnEjecutar.Text = bO.Equals("BtnEjecutar") ? bT : BtnEjecutar.Text;
                    CkbAlterno.Text = bO.Equals("CkbAlterno") ? "&nbsp" + bT : CkbAlterno.Text;
                    LblStockActual.Text = bO.Equals("GrdCantStockMstr") ? bT : LblStockActual.Text;
                    BtnExport.Text = bO.Equals("BtnExportMstr") ? bT : BtnExport.Text;
                    BtnExport.ToolTip = bO.Equals("BtnExportTT") ? bT : BtnExport.ToolTip;
                    ViewState["IdimaPnALter"] = bO.Equals("LblTitAlterno") ? bT : ViewState["IdimaPnALter"];
                    ViewState["TitExprtMvts"] = bO.Equals("TitExprtMvts") ? bT : ViewState["TitExprtMvts"];
                    LblTitAlterno.Text = bO.Equals("LblTitAlterno") ? bT : LblTitAlterno.Text;
                    GrdAlterno.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAlterno.EmptyDataText;
                    GrdAlterno.Columns[1].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdAlterno.Columns[1].HeaderText;
                    GrdAlterno.Columns[2].HeaderText = bO.Equals("GrdBloq") ? bT : GrdAlterno.Columns[2].HeaderText;
                    LblTitStock.Text = bO.Equals("LblTitStock") ? bT : LblTitStock.Text;
                    GrdStokAlma.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdStokAlma.EmptyDataText;
                    GrdStokAlma.Columns[0].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdStokAlma.Columns[0].HeaderText;
                    GrdStokAlma.Columns[1].HeaderText = bO.Equals("CantMst") ? bT : GrdStokAlma.Columns[1].HeaderText;
                    GrdStokAlma.Columns[2].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdStokAlma.Columns[2].HeaderText;
                    GrdStokAlma.Columns[3].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdStokAlma.Columns[3].HeaderText;
                    GrdStokAlma.Columns[6].HeaderText = bO.Equals("LoteMst") ? bT : GrdStokAlma.Columns[6].HeaderText;
                    GrdStokAlma.Columns[7].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdStokAlma.Columns[7].HeaderText;
                    GrdStokAlma.Columns[8].HeaderText = bO.Equals("GrdFila") ? bT : GrdStokAlma.Columns[8].HeaderText;
                    GrdStokAlma.Columns[9].HeaderText = bO.Equals("GrdColumn") ? bT : GrdStokAlma.Columns[9].HeaderText;
                    LblTitMovimientos.Text = bO.Equals("LblTitMovimientos") ? bT : LblTitMovimientos.Text;
                    // *********************************************** Modal ***********************************************
                    LblTitModalBusqPN.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitModalBusqPN.Text;
                    RdbMdlBusqLote.Text = bO.Equals("LoteMst") ? "&nbsp" + bT : RdbMdlBusqLote.Text;
                    RdbMdlBusqDesc.Text = bO.Equals("Descripcion") ? "&nbsp" + bT : RdbMdlBusqDesc.Text;
                    if (bO.Equals("placeholder"))
                    { TxtModalBusq.Attributes.Add("placeholder", bT); }
                    IbtModalBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtModalBusq.ToolTip;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblModalBusq.Text;
                    BtnCloseModalBusqPN.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqPN.Text;
                    GrdMdlBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdMdlBusq.EmptyDataText;
                    GrdMdlBusq.Columns[3].HeaderText = bO.Equals("LoteMst") ? bT : GrdMdlBusq.Columns[3].HeaderText;
                    GrdMdlBusq.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdMdlBusq.Columns[4].HeaderText;
                    GrdMdlBusq.Columns[5].HeaderText = bO.Equals("TipoMstr") ? bT : GrdMdlBusq.Columns[5].HeaderText;
                    // *********************************************** Movimientos ***********************************************
                    GrdMvtos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdMvtos.EmptyDataText;
                    GrdMvtos.Columns[0].HeaderText = bO.Equals("GrdDoc") ? bT : GrdMvtos.Columns[0].HeaderText;
                    GrdMvtos.Columns[1].HeaderText = bO.Equals("GrdMvto") ? bT : GrdMvtos.Columns[1].HeaderText;
                    GrdMvtos.Columns[2].HeaderText = bO.Equals("FechaMstr") ? bT : GrdMvtos.Columns[2].HeaderText;
                    GrdMvtos.Columns[4].HeaderText = bO.Equals("GrdSnLot") ? bT : GrdMvtos.Columns[4].HeaderText;
                    GrdMvtos.Columns[5].HeaderText = bO.Equals("GrdEntr") ? bT : GrdMvtos.Columns[5].HeaderText;
                    GrdMvtos.Columns[6].HeaderText = bO.Equals("GrdSali") ? bT : GrdMvtos.Columns[6].HeaderText;
                    GrdMvtos.Columns[7].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdMvtos.Columns[7].HeaderText;
                    GrdMvtos.Columns[8].HeaderText = bO.Equals("GrdOtrD") ? bT : GrdMvtos.Columns[8].HeaderText;
                    GrdMvtos.Columns[9].HeaderText = bO.Equals("GrdOT") ? bT : GrdMvtos.Columns[9].HeaderText;
                    GrdMvtos.Columns[10].HeaderText = bO.Equals("PosMstr") ? bT : GrdMvtos.Columns[10].HeaderText;
                    GrdMvtos.Columns[11].HeaderText = bO.Equals("GrdMtvo") ? bT : GrdMvtos.Columns[11].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl(string Pn, string Sn, string Lote, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_PANTALLA_ConsultaMovimiento 13,'','','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPSL = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPSL);
                                    DSTPSL.Tables[0].TableName = "PN";
                                    DSTPSL.Tables[1].TableName = "SN";
                                    DSTPSL.Tables[2].TableName = "LOT";

                                    ViewState["DSTPSL"] = DSTPSL;
                                }
                            }
                        }
                    }
                }

                BindBDdlPN(Pn);
                if (Pn.Equals("") && Lote.Equals(""))
                {
                    BindBDdlSN(Pn, Sn);
                    BindBDdlLOT(Pn, Lote);
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
        protected void BindBDdlPN(string Pn)
        {
            DSTPSL = (DataSet)ViewState["DSTPSL"];
            if (DSTPSL.Tables["PN"].Rows.Count > 0)
            {


                DdlPN.DataSource = DSTPSL.Tables[0];
                DdlPN.DataTextField = "PN";
                DdlPN.DataValueField = "Codigo";
                DdlPN.DataBind();
            }
        }
        protected void BindBDdlSN(string Pn, string Sn)
        {
            DSTPSL = (DataSet)ViewState["DSTPSL"];
            if (DSTPSL.Tables["SN"].Rows.Count > 0)
            {
                DdlSN.DataSource = DSTPSL.Tables[1];
                DdlSN.DataTextField = "SN";
                DdlSN.DataValueField = "Codigo";
                DdlSN.DataBind();
            }
        }
        protected void BindBDdlLOT(string Pn, string Lote)
        {
            DSTPSL = (DataSet)ViewState["DSTPSL"];
            if (DSTPSL.Tables["LOT"].Rows.Count > 0)
            {
                DdlLote.DataSource = DSTPSL.Tables[2];
                DdlLote.DataTextField = "LOTE";
                DdlLote.DataValueField = "Codigo";
                DdlLote.DataBind();
            }
        }
        protected void BindBusPn(string PN)
        {
            DSTPSL = (DataSet)ViewState["DSTPSL"];
            DataRow[] DR; DataTable DT = new DataTable();
            GrdAlterno.DataSource = null; GrdAlterno.DataBind();
            GrdStokAlma.DataSource = null; GrdStokAlma.DataBind();
            GrdMvtos.DataSource = null; GrdMvtos.DataBind();

            DR = DSTPSL.Tables[0].Select("Codigo ='" + PN + "'");
            if (Cnx.ValidaDataRowVacio(DR))
            {
                DT = DR.CopyToDataTable();
                TxtTipo.Text = DT.Rows[0]["DescTipo"].ToString().Trim();
                TxtDescrPn.Text = DT.Rows[0]["Descripcion"].ToString().Trim();
                ViewState["CodReferencia"] = DT.Rows[0]["CodReferencia"].ToString().Trim();
                LblTitAlterno.Text = ViewState["IdimaPnALter"].ToString().Trim() + " [" + ViewState["CodReferencia"] + "]";
                switch (DT.Rows[0]["Identf"].ToString().Trim())
                {
                    case "SN":
                        DR = DSTPSL.Tables[1].Select("PN ='" + PN + "' OR Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlSN.DataSource = DT;
                            DdlSN.DataTextField = "SN";
                            DdlSN.DataValueField = "Codigo";
                        }
                        else { DdlSN.DataSource = null; }
                        DdlSN.DataBind();
                        DR = DSTPSL.Tables[2].Select("Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlLote.DataSource = DT;
                            DdlLote.DataTextField = "LOTE";
                            DdlLote.DataValueField = "Codigo";
                            DdlLote.DataBind();
                        }
                        break;
                    case "LOTE":
                        DR = DSTPSL.Tables[2].Select("PN ='" + PN + "' OR Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlLote.DataSource = DT;
                            DdlLote.DataTextField = "LOTE";
                            DdlLote.DataValueField = "Codigo";
                        }
                        else { DdlLote.DataSource = null; }
                        DdlLote.DataBind();
                        DR = DSTPSL.Tables[1].Select("Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlSN.DataSource = DT;
                            DdlSN.DataTextField = "SN";
                            DdlSN.DataValueField = "Codigo";
                            DdlSN.DataBind();
                        }
                        break;
                    default:
                        DR = DSTPSL.Tables[2].Select("Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlLote.DataSource = DT;
                            DdlLote.DataTextField = "LOTE";
                            DdlLote.DataValueField = "Codigo";
                            DdlLote.DataBind();
                        }
                        DR = DSTPSL.Tables[1].Select("Codigo = ''");
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DT = DR.CopyToDataTable();
                            DdlSN.DataSource = DT;
                            DdlSN.DataTextField = "SN";
                            DdlSN.DataValueField = "Codigo";
                            DdlSN.DataBind();
                        }
                        break;
                }

                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_ConsultaMovimiento 14,@CRf,'','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {

                        SC.Parameters.AddWithValue("@CRf", ViewState["CodReferencia"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataTable DTA = new DataTable())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DTA);
                                GrdAlterno.DataSource = DTA; GrdAlterno.DataBind();
                            }
                        }
                    }
                }
            }
        }
        protected void DdlPN_TextChanged(object sender, EventArgs e)
        { BindBusPn(DdlPN.Text.Trim()); }
        //**************** Modal ***********************************
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true); }
        protected void BindMdlBusq()
        {
            string VbPN = "", VbSn = "", VbLot = "", VbDesc = "";
            if (RdbMdlBusqPN.Checked == true) { VbPN = TxtModalBusq.Text.Trim(); }
            if (RdbMdlBusqSN.Checked == true) { VbSn = TxtModalBusq.Text.Trim(); }
            if (RdbMdlBusqLote.Checked == true) { VbLot = TxtModalBusq.Text.Trim(); }
            if (RdbMdlBusqDesc.Checked == true) { VbDesc = TxtModalBusq.Text.Trim(); }
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_ConsultaMovimiento 15,@Pn, @Sn,@Lt,@Dc,0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Pn", VbPN);
                    SC.Parameters.AddWithValue("@Sn", VbSn);
                    SC.Parameters.AddWithValue("@Lt", VbLot);
                    SC.Parameters.AddWithValue("@Dc", VbDesc);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataTable DTA = new DataTable())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DTA);
                            GrdMdlBusq.DataSource = DTA; GrdMdlBusq.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            BindMdlBusq();
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void GrdMdlBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                if (e.CommandName.Equals("IrPN"))
                {
                    GridViewRow GVR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string VbPN = GrdMdlBusq.DataKeys[GVR.RowIndex].Values["PN"].ToString().Trim();
                    string VbSN = GrdMdlBusq.DataKeys[GVR.RowIndex].Values["SN"].ToString().Trim();
                    string VbLot = GrdMdlBusq.DataKeys[GVR.RowIndex].Values["LOTE"].ToString().Trim();
                    string VbDes = ((Label)GVR.FindControl("LblDesc")).Text.ToString().Trim().Trim();
                    BindBusPn(VbPN);
                    DdlPN.Text = VbPN;
                    DdlSN.Text = VbSN;
                    DdlLote.Text = VbLot;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Consulta movimientos Almacén", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

            }
        }
        protected void GrdMdlBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIrPN = (e.Row.FindControl("IbtIrPN") as ImageButton);
                if (IbtIrPN != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtIrPN.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //**************** Ejecutar ***********************************
        protected void BindEjecutar()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (DdlPN.Text.Trim().Equals("")) { GrdStokAlma.DataSource = null; GrdStokAlma.DataBind(); return; }
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbPn = DdlPN.Text.Trim();
                    string VbSn = DdlSN.Text.Trim();
                    string VbLot = DdlLote.Text.Trim();
                    if (CkbAlterno.Checked == true) { VbPn = ""; VbSn = ""; VbLot = ""; }

                    CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                    CursorIdioma.Alimentar("CURDATOSCONSULTA1", Session["77IDM"].ToString().Trim());
                    string VbTxtSql = "EXEC SP_TablasLogistica 5, @Rf,@Pn,@Sn,@Lt,'','','','','CURDATOSCONSULTA1',0,0,@ExpVlr,1,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Rf", ViewState["CodReferencia"]);
                        SC.Parameters.AddWithValue("@Pn", VbPn);
                        SC.Parameters.AddWithValue("@Sn", VbSn);
                        SC.Parameters.AddWithValue("@Lt", VbLot);
                        SC.Parameters.AddWithValue("@ExpVlr", ViewState["VblCE4"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSSM = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSSM);
                                DSSM.Tables[0].TableName = "Stock";
                                DSSM.Tables[1].TableName = "StockActual";
                                DSSM.Tables[2].TableName = "Mvtos";
                                DSSM.Tables[3].TableName = "Exprtr";
                                ViewState["DSSM"] = DSSM;

                                GrdStokAlma.DataSource = DSSM.Tables[0]; GrdStokAlma.DataBind();
                                TxtStockActual.Text = DSSM.Tables[1].Rows[0]["CantTtl"].ToString().Trim();
                                GrdMvtos.DataSource = DSSM.Tables[2]; GrdMvtos.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE Detalle PPT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnEjecutar_Click(object sender, EventArgs e)
        { BindEjecutar(); }
        protected void GrdStokAlma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbTercero = dr["CodTercero"].ToString().Trim();
                if (VbTercero.Equals("Tercero"))                
                {
                    e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
            }
        }
        //**************** Detalle Movimeintos ***********************************
        protected void GrdMvtos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdMvtos.EditIndex = e.NewEditIndex; DSSM = (DataSet)ViewState["DSSM"]; GrdMvtos.DataSource = DSSM.Tables[2]; GrdMvtos.DataBind(); }
        protected void GrdMvtos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbID = GrdMvtos.DataKeys[e.RowIndex].Values["IdDoc"].ToString().Trim();
            string Observac = (GrdMvtos.Rows[e.RowIndex].FindControl("TxtMotvoE") as TextBox).Text.Trim();

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_ConsultaMovimiento 16, @Us,'','', @Ob,@ID,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Ob", Observac);
                        SC.Parameters.AddWithValue("@ID", VbID);
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
                            GrdMvtos.EditIndex = -1;
                            BindEjecutar();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + "Editar Motivo Mvto Almacen", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdMvtos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdMvtos.EditIndex = -1; DSSM = (DataSet)ViewState["DSSM"]; GrdMvtos.DataSource = DSSM.Tables[2]; GrdMvtos.DataBind(); }
        protected void GrdMvtos_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    if ((int)ViewState["VblCE3"] == 0) { imgE.Visible = false; }
                    else
                    {
                        imgE.Visible = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }
                }
            }
        }
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString();
                if (DdlPN.Text.Trim().Equals("")) { GrdStokAlma.DataSource = null; GrdStokAlma.DataBind(); return; }
                DSSM = (DataSet)ViewState["DSSM"];
                using (XLWorkbook wb = new XLWorkbook())
                {                   
                    wb.Worksheets.Add(DSSM.Tables[3]);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/ms-excel";
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", ViewState["TitExprtMvts"]));
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel Aeronaves", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}