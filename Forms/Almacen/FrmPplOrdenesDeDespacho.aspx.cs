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
    public partial class FrmPplOrdenesDeDespacho : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable IdiomaAll = new DataTable();
        DataTable DTEncbzd = new DataTable("Encabezado");
        DataTable DTDet = new DataTable("Detalle");
        DataSet DsStockO = new DataSet();
        DataSet DSTDdl = new DataSet();
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
                ViewState["PageTit"] = "";
                ModSeguridad();
                TraerDatos();
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMovimientoActivo.aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { BtnGuardar.Visible = false; BtnNewDet.Visible = false; }
            IdiomaControles();
        }
        protected void IdiomaControles()
        {
            IdiomaAll = (DataTable)Session["TblIdmGrl"];
            DataRow[] DR = IdiomaAll.Select("CodF IN('0','FrmMovimientoActivo')");
            if (Cnx.ValidaDataRowVacio(DR))
            { Idioma = DR.CopyToDataTable(); ViewState["TablaIdioma"] = Idioma; }
            ViewState["PageTit"] = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "TituloOD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            Page.Title = ViewState["PageTit"].ToString();
            TitForm.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "CaptionOD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblAlmacenO.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblAlmacenO").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblAlmacenD.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblAlmacenD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblUsuR.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblUsuR").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            BtnGuardar.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BotonIngOk").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblObserv.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblObsMst").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblOD.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblOD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            IbtExportar.ToolTip = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnExportMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            BtnNewDet.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnNewDet").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            // *************************************************Detalle Asignacion *************************************************        
            LblTitAsigFis.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblTitAsigElemMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblBusqueda.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "Busqueda").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            TxtBusqueda.Attributes.Add("placeholder", Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "placeholderBusPN").Select(x => x.Field<string>("Texto")).FirstOrDefault());
            IbtBusqueda.ToolTip = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnConsultar").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            IbtCerrarAsing.ToolTip = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "CerrarVentana").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.EmptyDataText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "SinRegistros").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[3].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdLote").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[4].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblIdentifMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[5].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdCantStockMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[6].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdCantD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[7].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdUndMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[8].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdUbica").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.Columns[9].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdBodD").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdStock.EmptyDataText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "SinRegistros").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            // *************************************************Detalle Reserva *************************************************
            GrdDet.Columns[3].HeaderText = GrdStock.Columns[3].HeaderText;
            GrdDet.Columns[4].HeaderText = GrdStock.Columns[6].HeaderText;
            GrdDet.Columns[5].HeaderText = GrdStock.Columns[7].HeaderText;
            GrdDet.Columns[6].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdBodOrig").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdDet.Columns[7].HeaderText = GrdStock.Columns[9].HeaderText;                     
        }
        protected void TraerDatos()
        {
            string LtxtSql = string.Format("EXEC SP_Pantalla_Orden_Despacho 8, '{0}','','','',0,0, {1}, {2},'01-1-2009','01-01-1900','01-01-1900'", Session["C77U"], Session["77IDM"], Session["!dC!@"]);
            DSTDdl = Cnx.DSET(LtxtSql);
            DSTDdl.Tables[0].TableName = "Almac";
            DSTDdl.Tables[1].TableName = "Users";
            DSTDdl.Tables[2].TableName = "Detalle";
            DSTDdl.Tables[3].TableName = "NumOD";
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacenO.DataSource = DSTDdl.Tables["Almac"];
                DdlAlmacenO.DataTextField = "NomAlmacen";
                DdlAlmacenO.DataValueField = "CodIdAlmacen";
                DdlAlmacenO.DataBind();
            }
            ViewState["DSTDdl"] = DSTDdl;
            TxtOD.Text = DSTDdl.Tables["NumOD"].Rows[0]["Actual"].ToString().Trim();
        }
        protected void BtnNewDet_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (DdlAlmacenO.Text.Equals("0"))
            {
                Result = Idioma.Select("Objeto= 'MstrMens19'");
                foreach (DataRow row in Result)
                { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar el almacén.
                return;
            }
            if (DdlAlmacenD.Text.Equals("0"))
            {
                Result = Idioma.Select("Objeto= 'Mensj01'");
                foreach (DataRow row in Result)
                { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar el almacén destino.
                return;
            }
            if (DdlUsuR.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mensj02'");
                foreach (DataRow row in Result)
                { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar el usuario quien recibe.
                return;
            }
            if (DdlAlmacenD.Enabled == true)
            {
                string LtxtSql = string.Format("EXEC SP_Pantalla_Orden_Despacho 9, '','','','',{0},0,{1},{2},'01-1-2009','01-01-1900','01-01-1900'", DdlAlmacenO.Text, Session["77IDM"], Session["!dC!@"]);
                DsStockO = Cnx.DSET(LtxtSql);
                DsStockO.Tables[0].TableName = "StockO";
                DsStockO.Tables[1].TableName = "BodDest";
                ViewState["DsStockO"] = DsStockO;
            }
            DsStockO = (DataSet)ViewState["DsStockO"];
            MultVw.ActiveViewIndex = 1;
        }
        protected void DdlAlmacenO_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DdlAlmacenO.Text.Equals("0")) { DdlAlmacenD.Items.Clear(); DdlUsuR.Items.Clear(); }
            else
            {
                IEnumerable<DataRow> VbQry = from A in DSTDdl.Tables["Almac"].AsEnumerable() where A.Field<int>("CodidAlmacen") != Convert.ToInt32(DdlAlmacenO.Text) select A;
                DataTable DT = VbQry.CopyToDataTable();
                DdlAlmacenD.DataSource = DT;
                DdlAlmacenD.DataTextField = "NomAlmacen";
                DdlAlmacenD.DataValueField = "CodIdAlmacen";
                DdlAlmacenD.DataBind();
            }
        }
        protected void DdlAlmacenD_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DdlAlmacenD.Text.Equals("0")) { DdlUsuR.Items.Clear(); }
            else
            {
                IEnumerable<DataRow> VbQry = from A in DSTDdl.Tables["Users"].AsEnumerable() where A.Field<int>("CodidAlmacen") == Convert.ToInt32(DdlAlmacenD.Text) || A.Field<int>("CodidAlmacen") == 0 select A;
                DataTable DT = VbQry.CopyToDataTable();
                DdlUsuR.DataSource = DT;
                DdlUsuR.DataTextField = "Usuario";
                DdlUsuR.DataValueField = "CodPersona";
                DdlUsuR.DataBind();
            }
        }
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            MultVw.ActiveViewIndex = 0;
            GrdStock.DataSource = null; GrdStock.DataBind();
        }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            DsStockO = (DataSet)ViewState["DsStockO"];
            string busqueda = TxtBusqueda.Text.Trim();
            IEnumerable<DataRow> VbQry = from A in DsStockO.Tables[0].AsEnumerable() where A.Field<string>("PN").Contains(TxtBusqueda.Text.Trim()) && A.Field<int>("CodIdSelec") == 0 select A;
            if (Cnx.ValidaDataRowVacio(VbQry))
            {
                DataTable DT = VbQry.CopyToDataTable();
                if (DT.Rows.Count > 0) { GrdStock.DataSource = DT; GrdStock.DataBind(); }
                else { GrdStock.DataSource = null; GrdStock.DataBind(); }
            }
            else { GrdStock.DataSource = null; GrdStock.DataBind(); }
        }
        protected void GrdStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DsStockO = (DataSet)ViewState["DsStockO"];

            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView DRV = e.Row.DataItem as DataRowView;
                string S_CodTercero = DRV["CodTercero"].ToString().Trim();
                string S_StdoBod = DRV["CodEstadoBodega"].ToString().Trim();
                string S_Identif = DRV["IdentificadorElemR"].ToString().Trim();
                DropDownList DdlBogDest = (e.Row.FindControl("DdlBogDest") as DropDownList);
                TextBox TxtCantDespa = (e.Row.FindControl("TxtCantDespa") as TextBox);
                if (S_CodTercero.Equals("Tercero"))
                {
                    e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
                if (S_Identif.Equals("SN"))
                {
                    TxtCantDespa.Text = "1";
                    TxtCantDespa.Enabled = false;
                }
                DataTable DT = new DataTable();
                DataRow[] DR = DsStockO.Tables["BodDest"].Select("CodTercero ='" + S_CodTercero + "' AND CodEstadoBodega ='" + S_StdoBod + "' OR CodUbicaBodega = ''");
                if (Cnx.ValidaDataRowVacio(DR))
                { DT = DR.CopyToDataTable(); }
                DdlBogDest.DataSource = DT;
                DdlBogDest.DataTextField = "Bodega";
                DdlBogDest.DataValueField = "CodUbicaBodega";
                DdlBogDest.DataBind();
                ImageButton IbtIr = e.Row.FindControl("IbtIr") as ImageButton;
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                foreach (DataRow RowIdioma in Result)
                { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
            }
        }
        protected void GrdStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DsStockO = (DataSet)ViewState["DsStockO"];
            DataRow[] Result;
            try
            {
                Page.Title = ViewState["PageTit"].ToString();
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                if (e.CommandName.Equals("Asignar"))
                {
                    GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow Gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    string S_Ref = GrdStock.DataKeys[Gvr.RowIndex].Values["CodReferencia"].ToString().Trim();
                    string S_PN = ((Label)Row.FindControl("LblPn")).Text.ToString().Trim();
                    string S_SN = ((Label)Row.FindControl("LblSn")).Text.ToString().Trim();
                    string S_Lot = ((Label)Row.FindControl("LblLote")).Text.ToString().Trim();
                    int I_AD = Convert.ToInt32(DdlAlmacenD.Text);
                    string S_BodD = ((DropDownList)Row.FindControl("DdlBogDest")).Text.ToString().Trim();
                    string S_NomBodD = ((DropDownList)Row.FindControl("DdlBogDest")).SelectedItem.Text.ToString().Trim();
                    int I_AO = Convert.ToInt32(DdlAlmacenO.Text);
                    string S_CUBO = GrdStock.DataKeys[Gvr.RowIndex].Values["CodUbicaBodega"].ToString().Trim();
                    string S_NomBodO = GrdStock.DataKeys[Gvr.RowIndex].Values["Bodega"].ToString().Trim();
                    double D_Cant = Convert.ToDouble(((TextBox)Row.FindControl("TxtCantDespa")).Text.ToString().Trim().Equals("") ? "0" : ((TextBox)Row.FindControl("TxtCantDespa")).Text.ToString().Trim());
                    double D_Stock = Convert.ToDouble(((Label)Row.FindControl("LblCant")).Text.ToString().Trim());
                    string S_UM = ((Label)Row.FindControl("LblUM")).Text.ToString().Trim();
                    int I_IdUbO = Convert.ToInt32(GrdStock.DataKeys[Gvr.RowIndex].Values["CodIdUbicacion"].ToString());
                    string S_EstBod = GrdStock.DataKeys[Gvr.RowIndex].Values["CodEstadoBodega"].ToString().Trim();
                    string S_CodEle = GrdStock.DataKeys[Gvr.RowIndex].Values["CodElemento"].ToString().Trim();
                    string S_TEle = GrdStock.DataKeys[Gvr.RowIndex].Values["CodTipoElemento"].ToString().Trim();
                    string S_CodTerc = GrdStock.DataKeys[Gvr.RowIndex].Values["CodTercero"].ToString().Trim();
                    string S_Identf = GrdStock.DataKeys[Gvr.RowIndex].Values["IdentificadorElemR"].ToString().Trim();
                    if (D_Cant < 1)
                    {
                        Result = Idioma.Select("Objeto= 'Mens13Aero'");
                        foreach (DataRow row in Result)
                        { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Existen cantidades iguales o menores a cero.
                        return;
                    }
                    if (D_Cant > D_Stock)
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens30'");
                        foreach (DataRow row in Result)
                        { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//La cantidad no debe superar a la que se encuentra en stock.
                        return;
                    }
                    if (S_BodD.Equals(""))
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens20'");
                        foreach (DataRow row in Result)
                        { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar la bodega.
                        return;
                    }
                    if (!S_Identf.Equals("SN"))
                    {
                        foreach (DataRow row in DSTDdl.Tables["Detalle"].Rows)
                        {
                            if (row["Pn"].ToString().Equals(S_PN) && row["Lote"].ToString().Equals(S_Lot))
                            {
                                Result = Idioma.Select("Objeto= 'Mensj03'");
                                foreach (DataRow DR in Result)
                                { this.Master.SetObjMensj(DR["Texto"].ToString().Trim()); }//Los elementos no serializados no se pueden despachar en el mismo documento más de una vez.
                                return;
                            }
                        }
                    }
                    int I_Pos = 0;
                    if (DSTDdl.Tables["Detalle"].Rows.Count > 0) { I_Pos = DSTDdl.Tables["Detalle"].AsEnumerable().Max(x => x.Field<int>("Pos")); }
                    I_Pos++;
                    DSTDdl.Tables["Detalle"].Rows.Add(I_Pos, S_Ref, S_PN, S_SN, S_Lot, I_AD, S_BodD, I_AO, S_CUBO, D_Cant, S_UM, "OD", I_IdUbO, S_EstBod, S_CodEle, S_TEle, S_CodTerc,
                                                        S_Identf, S_NomBodO, S_NomBodD);
                    DSTDdl.Tables["Detalle"].AcceptChanges();
                    GrdStock.DataSource = null; GrdStock.DataBind();
                    DdlAlmacenO.Enabled = false;
                    DdlAlmacenD.Enabled = false;
                    DdlUsuR.Enabled = false;
                    foreach (DataRow row in DsStockO.Tables["StockO"].Rows)
                    {
                        if (row["CodIdUbicacion"].ToString().Equals(I_IdUbO.ToString()))
                        {
                            row["CodIdSelec"] = I_IdUbO;
                        }
                    }
                    DsStockO.Tables["StockO"].AcceptChanges();
                    if (DSTDdl.Tables["Detalle"].Rows.Count > 0) { GrdDet.DataSource = DSTDdl.Tables["Detalle"]; GrdDet.DataBind(); }
                    else { GrdDet.DataSource = null; GrdDet.DataBind(); }
                    MultVw.ActiveViewIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar P/N en OD", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }

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
                    DataRowView DRV = e.Row.DataItem as DataRowView;
                    string S_CodTercero = DRV["CodTercero"].ToString().Trim();
                    if (S_CodTercero.Equals("Tercero"))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightSalmon;
                        e.Row.ForeColor = System.Drawing.Color.White;
                    }              
                }
            }
        }
        protected void GrdDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DsStockO = (DataSet)ViewState["DsStockO"];
            int index = Convert.ToInt32(e.RowIndex);
            string S_CodUbic = GrdDet.DataKeys[e.RowIndex].Values["CodIdUbicO"].ToString();
            DSTDdl.Tables["Detalle"].Rows[index].Delete();
            DSTDdl.Tables["Detalle"].AcceptChanges();
            int I_Pos = 1;
            foreach (DataRow row in DSTDdl.Tables["Detalle"].Rows)
            {
                row["Pos"] = I_Pos; // Actualiza el número de fila
                I_Pos++;
            }
            if (DSTDdl.Tables["Detalle"].Rows.Count > 0) { GrdDet.DataSource = DSTDdl.Tables["Detalle"]; GrdDet.DataBind(); }
            else { GrdDet.DataSource = null; GrdDet.DataBind(); }
            foreach (DataRow row in DsStockO.Tables["StockO"].Rows)
            {
                if (row["CodIdUbicacion"].ToString().Equals(S_CodUbic.ToString()))
                {
                    row["CodIdSelec"] = 0;
                }
            }
            DsStockO.Tables["StockO"].AcceptChanges();
        }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result;
            try
            {
                string PMensj = "", PPN = "", PSN = "", PLote = "";
                if (TxtObserv.Text.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens22'");
                    foreach (DataRow row in Result)
                    { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar una observación.
                    return;
                }
                if (DSTDdl.Tables["Detalle"].Rows.Count == 0)
                {
                    Result = Idioma.Select("Objeto= 'Mensj04'");
                    foreach (DataRow row in Result)
                    { this.Master.SetObjMensj(row["Texto"].ToString().Trim()); }//Debe ingresar al menos un parte número en el detalle.
                    return;
                }
                // 1. Crear DataSet con las dos tablas relacionadas
                DataSet DS = new DataSet("Root");
                DTDet = DSTDdl.Tables["Detalle"].Copy();
                DS.Tables.Add(DTDet);
                //DS.Tables.Add(DTEnabezado); -- si tuviera otra tabla se agregaria y se enviaria el DataSet con las dos tablas incorporadas
                // 2. Convertir DataSet a XML String
                string xmlData = DS.GetXml();
                // 3. Enviar a SQL Server
                Cnx.SelecBD();
                using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                {
                    SCX.Open();
                    using (SqlTransaction transaction = SCX.BeginTransaction())
                    {
                        string VBQuery = "InsertOrdenDespacho_SP";
                        using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                        {
                            try
                            {
                                SC.CommandType = CommandType.StoredProcedure;
                                SC.Parameters.Add("@DatosXML", SqlDbType.Xml).Value = xmlData;
                                SC.Parameters.Add("@IdCia", SqlDbType.Int).Value = Session["!dC!@"].ToString();
                                SC.Parameters.Add("@UsuO", SqlDbType.Char).Value = Session["C77U"].ToString();
                                SC.Parameters.Add("@UsuD", SqlDbType.Char).Value = DdlUsuR.Text.Trim();
                                SC.Parameters.Add("@Observ", SqlDbType.Char).Value = TxtObserv.Text.Trim();
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    PPN = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
                                    PSN = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                                    PLote = HttpUtility.HtmlDecode(SDR["Lote"].ToString().Trim());
                                }
                                SDR.Close();
                                transaction.Commit();
                                SCX.Close();
                                if (!PMensj.Equals(""))
                                {
                                    string VblPn = PPN.Equals("") ? "" : "  [P/N: " + PPN + "]  ";
                                    string VblSn = PSN.Equals("") ? "" : " [S/N: " + PSN + "] ";
                                    string VbLote = PLote.Equals("") ? "" : " [LT/N: " + PLote + "]";
                                    Result = Idioma.Select("Objeto= '" + PMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { this.Master.SetObjMensj(row["Texto"].ToString().Trim() + VblPn + VblSn + VbLote); }//Debe ingresar al menos un parte número en el detalle.
                                    return;
                                }
                                DdlAlmacenO.Items.Clear();
                                DdlAlmacenD.Items.Clear();
                                DdlUsuR.Items.Clear();
                                DdlAlmacenO.Enabled = true;
                                DdlAlmacenD.Enabled = true;
                                DdlUsuR.Enabled = true;
                                TxtObserv.Text = "";
                                GrdDet.DataSource = null; GrdDet.DataBind();
                                GrdStock.DataSource = null; GrdStock.DataBind();
                                DSTDdl.Tables["Detalle"].Rows.Clear();
                                TraerDatos();
                            }
                            catch (Exception ex) { transaction.Rollback(); }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = "OrdenDespacho Guardar", VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "OrdenDespacho Guardar", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtExportar_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            /*if (TxtFechIEyS.Text.Equals("") || TxtFechFEyS.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MensCampoReq'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                if (TxtFechFEyS.Text.Equals("")) { TxtFechFEyS.Focus(); }
                if (TxtFechIEyS.Text.Equals("")) { TxtFechIEyS.Focus(); }
                return;
            }
            Cnx.ValidarFechas(TxtFechIEyS.Text.Trim(), TxtFechFEyS.Text.Trim(), 2);
            var MensjF = Cnx.GetMensj();
            if (!MensjF.ToString().Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + MensjF.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { MensjF = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + MensjF + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }*/

            string VbNomArchivo = "OD";
            Result = Idioma.Select("Objeto= 'NomArcEyS'");
            foreach (DataRow row in Result)
            { VbNomArchivo = row["Texto"].ToString().Trim(); }

            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurExptrOD_Asig", Session["77IDM"].ToString().Trim());
            string Query = " EXEC Consultas_General_Almacen 2,'','','',0,@Idm,@ICC,'01/01/2000','01/06/2099'";

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    cmd.CommandTimeout = 90000000;
                    // cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIEyS.Text.Trim()));
                    //   cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFEyS.Text.Trim()));
                    //  cmd.Parameters.AddWithValue("@Alm", DdlAlmacenEyS.Text.Trim());
                    //  cmd.Parameters.AddWithValue("@NA", "CurExptrEyS");
                    cmd.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            ds.Tables[0].TableName = "XOM";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    wb.Worksheets.Add(dt);
                                }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
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
    }
}