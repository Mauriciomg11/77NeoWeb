using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class frmEntradaCompraMat : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSDetalle = new DataSet();

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
                ViewState["TipoCompra"] = "";
                ViewState["CodOrdenCompra"] = "";
                ViewState["CodOCAnt"] = "";
                ViewState["PosicionAnt"] = "0";
                ViewState["TtlDespacho"] = "0";
                ModSeguridad();
                TraerDatos("UPD");
                TipoCompra("N");
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
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;

                    // *********************************************** Detalle Reintegro ***********************************************

                }
                // DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnCl1'");
                // foreach (DataRow row in Result)
                //  { BtnGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");/**/ }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void TraerDatos(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 2, @U,'','','','',0,0,@Idm, @ICC,'01-01-1','02-01-01','03-01-01'";

                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@U", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Almac";
                                DSTDdl.Tables[1].TableName = "CompraNal";
                                DSTDdl.Tables[2].TableName = "CompraInta";
                                DSTDdl.Tables[3].TableName = "EjecCodigo";
                                DSTDdl.Tables[4].TableName = "EjecCodComex";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacen.DataSource = DSTDdl.Tables[0];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            }

        }
        protected void TipoCompra(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            ViewState["TipoCompra"] = Tipo;
            if (Tipo.Equals("N"))
            {
                if (DSTDdl.Tables["CompraNal"].Rows.Count > 0)
                {
                    DdlNumCompra.DataSource = DSTDdl.Tables["CompraNal"];
                    DdlNumCompra.DataTextField = "CodOrdenCompra";
                }
            }
            else
            {
                if (DSTDdl.Tables["CompraInta"].Rows.Count > 0)
                {
                    DdlNumCompra.DataSource = DSTDdl.Tables["CompraInta"];
                    DdlNumCompra.DataTextField = "CodShippingOrder";
                }
            }
            DdlNumCompra.DataValueField = "Codigo";
            DdlNumCompra.DataBind();
            DdlNumCompra.Text = "";
            GrdDtlleComp.DataSource = null;
            GrdDtlleComp.DataBind();
        }
        protected void RdbNacional_CheckedChanged(object sender, EventArgs e)
        { TipoCompra("N"); }
        protected void RdbInter_CheckedChanged(object sender, EventArgs e)
        { TipoCompra("I"); }
        protected void BindDetCompra(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                string S_ComNAL_INTA = "";
                if (ViewState["TipoCompra"].ToString().Equals("N")) { S_ComNAL_INTA = "CompraNal"; }
                else { S_ComNAL_INTA = "CompraInta"; }
                if (DSTDdl.Tables[S_ComNAL_INTA].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[S_ComNAL_INTA].Clone();
                    DataRow[] DR = DSTDdl.Tables[S_ComNAL_INTA].Select("Codigo='" + DdlNumCompra.Text.Trim() + "'");
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DT = DR.CopyToDataTable();
                        TxtMoneda.Text = DT.Rows[0]["CodMoneda"].ToString().Trim();
                        DataTable DTEC = new DataTable();
                        DataRow[] DREC; DataRow[] Result;
                        string S_AplicaComex = "S";
                        if (ViewState["TipoCompra"].ToString().Equals("I"))/*Si es internacional valida que este liquidada la orden de embarque*/
                        {
                            DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 5 AND EjecutarCodigo = 'S'"); /*Si aplica validacion de la liquidacion*/
                            if (Cnx.ValidaDataRowVacio(DREC))
                            {
                                DR = DSTDdl.Tables[S_ComNAL_INTA].Select("EstadoLiquidacion <> 1"); /*Esta liquidada*/
                                if (Cnx.ValidaDataRowVacio(DR))
                                {
                                    DT = DR.CopyToDataTable();
                                    Result = Idioma.Select("Objeto= 'Msj05EntC'"); /*La orden de embarque no se encuentra liquidada.	*/
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DdlNumCompra.Text.Trim() + "');", true); }
                                    GrdDtlleComp.DataSource = null; GrdDtlleComp.DataBind();
                                    return;
                                }
                            }
                            DREC = DSTDdl.Tables["EjecCodComex"].Select("Caso = 5 AND EjecutarCodigo = 'N'"); /*Aplica COMEX*/
                            if (Cnx.ValidaDataRowVacio(DREC)) { S_AplicaComex = "N"; }
                        }
                        DR = DSTDdl.Tables[S_ComNAL_INTA].Select("Codigo ='" + DdlNumCompra.Text.Trim() + "' AND Aprobado = 0");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta aprobada*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Msj01EntC'"); /*La orden de compra no se encuentra aprobada*/
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodOrdenCompra"].ToString().Trim() + "');", true); }
                            GrdDtlleComp.DataSource = null; GrdDtlleComp.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_ComNAL_INTA].Select("Codigo ='" + DdlNumCompra.Text.Trim() + "' AND Asentado = 1");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta asentada*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Msj02EntC'"); /*La orden de compra se encuentra asentada.*/
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodOrdenCompra"].ToString().Trim() + "');", true); }
                            GrdDtlleComp.DataSource = null; GrdDtlleComp.DataBind();
                            return;
                        }
                        if (Accion.Equals("UPD"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                            {
                                //string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 3, @CodOC,@TipoOC,'','','',0,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                                string VbTxtSql = "EXEC SP_Estado_Compra @CodOC,@ICC,@TipoOC,@ApliComex";
                                sqlConB.Open();
                                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                                {
                                    SC.Parameters.AddWithValue("@CodOC", DdlNumCompra.Text.Trim());
                                    //SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@TipoOC", ViewState["TipoCompra"]);
                                    SC.Parameters.AddWithValue("@ApliComex", S_AplicaComex);
                                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                                    {
                                        using (DataSet DSDetalle = new DataSet())
                                        {
                                            SDA.SelectCommand = SC;
                                            SDA.Fill(DSDetalle);
                                            DSDetalle.Tables[0].TableName = "EstadoCompra";
                                            DSDetalle.Tables[1].TableName = "CondManip";
                                            DSDetalle.Tables[2].TableName = "CurTemporal";
                                            DSDetalle.Tables[3].TableName = "CurActualizar";
                                            ViewState["DSDetalle"] = DSDetalle;
                                        }
                                    }
                                }
                            }
                        }
                        DSDetalle = (DataSet)ViewState["DSDetalle"];
                        if (DSDetalle.Tables["EstadoCompra"].Rows.Count > 0)
                        { GrdDtlleComp.DataSource = DSDetalle.Tables["EstadoCompra"]; }
                        GrdDtlleComp.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void DdlNumCompra_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (!DdlNumCompra.SelectedItem.Value.Equals("")) { BindDetCompra("UPD"); }
        }
        protected void BindCondicManipulac(string CodRef)
        {
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            if (DSDetalle.Tables["CondManip"].Rows.Count > 0)
            {
                DataRow[] DR = DSDetalle.Tables["CondManip"].Select("CodReferencia='" + CodRef + "'");
                if (Cnx.ValidaDataRowVacio(DR))
                {
                    DataTable DT = DR.CopyToDataTable();
                    GrdMdlCondManplc.DataSource = DT; GrdMdlCondManplc.DataBind();
                    if (DT.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPopup();", true);
                    }
                }
            }
        }
        protected void GrdDtlleComp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (e.CommandName.Equals("Abrir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["CodRef"] = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                ViewState["PN"] = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                ViewState["Identif"] = ((Label)row.FindControl("LblIdentfc")).Text.ToString().Trim();
                ViewState["CodOrdenCompra"] = ((Label)row.FindControl("LblNumOC")).Text.ToString().Trim();
                ViewState["IdDetOrdenCompra"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["IdDetOrdenCompra"].ToString();
                ViewState["FechaVencPN"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["FechaVencPN"].ToString();
                ViewState["Posicion"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                ViewState["PosSO"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["PosSO"].ToString();
                ViewState["Prov"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CodProveedor"].ToString();
                ViewState["VlrUnd"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["ValorUnidad"].ToString();
                ViewState["Vr_Compra"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["Valor_Compra"].ToString();
                ViewState["Trm"] = ((Label)row.FindControl("Lbltrm")).Text.ToString().Trim();
                ViewState["CodUM"] = ((Label)row.FindControl("LblUndMedDesp")).Text.ToString().Trim();
                ViewState["CodTipoElem"] = ((Label)row.FindControl("LblTipo")).Text.ToString().Trim();
                ViewState["UndCompra"] = ((Label)row.FindControl("LblUndMedCompra")).Text.ToString().Trim();
                ViewState["PNBloq"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["Bloquear"].ToString();
                ViewState["DT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["DiaTasa"].ToString().Trim();
                ViewState["MT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["MesTasa"].ToString().Trim();
                ViewState["AT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["AñoTasa"].ToString().Trim();
                ViewState["CCto"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CCostos"].ToString().Trim();
                ViewState["Csto_import"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["ValorUnidadP"].ToString().Trim();
                ViewState["PPT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["PPT"].ToString().Trim();
                ViewState["CodPedido"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CodPedido"].ToString().Trim();
                ViewState["Equiv"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["Equivalencia"].ToString().Trim();
                TxtFact.Text = ((Label)row.FindControl("LblFact")).Text.ToString().Trim();
                string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                ViewState["CantComp"] = ((Label)row.FindControl("LblCantCompra")).Text.ToString().Trim();
                string VbCantRec = ((Label)row.FindControl("LblCantRecb")).Text.ToString().Trim();
                LblPNDescripcAsig.Text = ViewState["Posicion"].ToString() + " | " + ViewState["PN"].ToString().Trim() + " | " + VblDescPN.Trim() + " | ";
                LblAsigCantCompV.Text = ViewState["CantComp"].ToString(); LblAsigCantRecV.Text = VbCantRec;
                DataRow[] Result;
                if (ViewState["PNBloq"].Equals("1"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens15'");
                    foreach (DataRow Row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//El P/N se encuentra bloqueado.
                    return;
                }
                if (ViewState["Trm"].Equals("0") && ViewState["TipoCompra"].Equals("I"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens36'");
                    foreach (DataRow Row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//La TRM del día no se ha creado.
                    return;
                }
                if (!ViewState["PosicionAnt"].Equals(ViewState["Posicion"]) || !ViewState["CodOCAnt"].Equals(ViewState["CodOrdenCompra"]))
                {
                    ViewState["TtlDespacho"] = "0";
                    DSDetalle.Tables["CurTemporal"].Clear();
                    DSDetalle.Tables["CurTemporal"].AcceptChanges();
                }
                DataRow[] DREC;
                DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 3 AND EjecutarCodigo = 'S'"); /*Solicitar en el ingreso de la compra y Repa Verificacion de la Referencia*/
                if (Cnx.ValidaDataRowVacio(DREC))
                {
                    DataRow[] DR = DSDetalle.Tables["EstadoCompra"].Select("Revisado = 0 AND CodReferencia = '" + ViewState["CodRef"].ToString().Trim() + "'"); /*Solicitar en el ingreso de la compra y Repa Verificacion de la Referencia*/
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DataTable DT = DR.CopyToDataTable();
                        Result = Idioma.Select("Objeto= 'Msj03EntC'"); /*La referencia no se encuentra verificada*/
                        foreach (DataRow Row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + " | " + ViewState["PN"].ToString().Trim() + "');", true); }
                        return;
                    }
                }
                BindGridTmp();
                MultVw.ActiveViewIndex = 1;
                BindCondicManipulac(ViewState["CodRef"].ToString());
            }
        }
        protected void GrdDtlleComp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    Result = Idioma.Select("Objeto='GrdAsigTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                DataRowView DRW = e.Row.DataItem as DataRowView;
                double VbDCanComp = Convert.ToDouble(DRW["Cant_Compra"].ToString().Trim());
                double VbDCanRec = Convert.ToDouble(DRW["CantRecibida"].ToString().Trim());
                double VbDCanIngreso = Convert.ToDouble(DRW["CantIngresar"].ToString().Trim());
                if (VbDCanComp <= VbDCanRec)
                {
                    IbtAbrir.Visible = false; e.Row.BackColor = System.Drawing.Color.YellowGreen; //e.Row.BackColor = System.Drawing.Color.PaleGreen; GreenYellow
                }
                if (VbDCanIngreso > 0) { IbtAbrir.Enabled = false; e.Row.BackColor = System.Drawing.Color.Wheat; }
                if ((VbDCanIngreso + VbDCanRec) < VbDCanComp && VbDCanRec > 0) { e.Row.BackColor = System.Drawing.Color.DarkOrange; }
                //else { e.Row.BackColor = System.Drawing.Color.YellowGreen; }

                Result = Idioma.Select("Objeto='GrdAsigdoTT'");
                foreach (DataRow RowIdioma in Result)
                { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

            }
        }
        protected void BtnAsignr_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            if (Convert.ToDouble(ViewState["TtlDespacho"]) <= 0) { return; }// si no tiene registro no ejecuta la asignacion
            ViewState["PosicionAnt"] = ViewState["Posicion"];
            ViewState["CodOCAnt"] = ViewState["CodOrdenCompra"];
            try
            {
                foreach (DataRow row in DSDetalle.Tables["CurTemporal"].Rows)
                {
                    double D_Cant = Convert.ToDouble(row["CantIngr"].ToString());
                    double D_Equiv = Convert.ToDouble(ViewState["Equiv"]);
                    int I_PPT = Convert.ToInt32(ViewState["PPT"]);
                    double D_VlrUnd = Convert.ToDouble(ViewState["VlrUnd"]) * Convert.ToDouble(ViewState["Trm"]) + Convert.ToDouble(ViewState["Csto_import"].ToString());
                    double D_VlrTRM = Convert.ToDouble(ViewState["Trm"]);
                    double D_VlrImpor = Convert.ToDouble(ViewState["Csto_import"].ToString());
                    int I_FV = Convert.ToInt32(ViewState["FechaVencPN"]);
                    string S_Fecha = S_Fecha = Cnx.ReturnFecha(row["FechaExp"].ToString());
                    string S_Sn, S_Lot;
                    switch (ViewState["Identif"])
                    {
                        case "SN":
                            S_Sn = row["SnLote"].ToString(); S_Lot = "";
                            break;
                        case "LOTE":
                            S_Sn = ""; S_Lot = row["SnLote"].ToString();
                            break;
                        default:
                            S_Sn = ""; S_Lot = "";
                            break;
                    }
                    DSDetalle.Tables["CurActualizar"].Rows.Add(ViewState["CodOrdenCompra"], DdlNumCompra.Text.Trim(), ViewState["PosSO"], row["Referencia"].ToString(),
                    row["PN"].ToString(), S_Sn, S_Lot, ViewState["CodTipoElem"], "NSN", "CodBodega", "F", "C", D_Cant, D_VlrUnd, D_VlrTRM, ViewState["CodPedido"],
                    ViewState["CCto"], D_VlrImpor, I_PPT, Convert.ToDateTime(S_Fecha), 0, "CodElemento", "674",
                    ViewState["Identif"], 1, ViewState["Prov"], I_FV, Convert.ToInt32(row["CantIngr"]), ViewState["CodUM"], "", ViewState["DT"], ViewState["MT"],
                    ViewState["AT"], TxtMoneda.Text.Trim(), ViewState["CodOrdenCompra"], Convert.ToInt32(ViewState["Posicion"]), Convert.ToDouble(row["CantIngr"]),
                    Convert.ToDouble(ViewState["Vr_Compra"]), ViewState["UndCompra"], TxtFact.Text.Trim(), D_Equiv, 1);
                }
                DSDetalle.Tables["CurActualizar"].AcceptChanges();
                //Actualizar la cantidad a despachar en la vista de Detalle Reserva          
                int I_IdComp = Convert.ToInt32(ViewState["IdDetOrdenCompra"]);
                foreach (DataRow row in DSDetalle.Tables["EstadoCompra"].Rows)
                {
                    if (Convert.ToInt32(row["IdDetOrdenCompra"].ToString()) == I_IdComp)
                    {
                        row["CantIngresar"] = Convert.ToDouble(ViewState["TtlDespacho"]);
                    }
                    if (!TxtFact.Text.Trim().Equals(""))
                    {
                        if (row["CodOrdenCompra"].ToString().Equals(ViewState["CodOrdenCompra"].ToString().Trim()))
                        {
                            row["NumFacturaOC"] = TxtFact.Text.Trim();
                        }
                    }
                }
                DSDetalle.Tables["EstadoCompra"].AcceptChanges();
                BindDetCompra("");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["PosicionAnt"] = ViewState["Posicion"];
            ViewState["CodOCAnt"] = ViewState["CodOrdenCompra"];
            MultVw.ActiveViewIndex = 0;
        }
        protected void BindGridTmp()
        {
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            if (DSDetalle.Tables["CurTemporal"].Rows.Count > 0) { GrdTemp.DataSource = DSDetalle.Tables["CurTemporal"]; GrdTemp.DataBind(); }
            else
            {
                DSDetalle.Tables["CurTemporal"].Rows.Add(DSDetalle.Tables["CurTemporal"].NewRow());
                GrdTemp.DataSource = DSDetalle.Tables["CurTemporal"];
                GrdTemp.DataBind();
                GrdTemp.Rows[0].Cells.Clear();
                GrdTemp.Rows[0].Cells.Add(new TableCell());
                GrdTemp.Rows[0].Cells[0].Text = "Empty..!";
                GrdTemp.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                DSDetalle.Tables["CurTemporal"].NewRow();
                GrdTemp.DataSource = DSDetalle.Tables["CurTemporal"];
                GrdTemp.DataBind();
            }
        }
        protected void GrdTemp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            if (e.CommandName.Equals("AddNew"))
            {
                try
                {
                    string S_SnLot = (GrdTemp.FooterRow.FindControl("TxtSnLotPP") as TextBox).Text.Trim().ToUpper();
                    string S_Cant = (GrdTemp.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdTemp.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim();
                    string S_Fecha = (GrdTemp.FooterRow.FindControl("TxtFechExp") as TextBox).Text.Trim();
                    if (ViewState["Identif"].ToString().Equals("SN") || ViewState["Identif"].ToString().Equals("LOTE"))
                    {
                        Result = DSDetalle.Tables["CurTemporal"].Select("SnLote = '" + S_SnLot + "'");
                        foreach (DataRow Row in Result)
                        {
                            Result = Idioma.Select("Objeto= 'MensRcElm01'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El elemento ya se encuentra registrado..
                            return;
                        }
                        if (S_SnLot.Equals(""))
                        {
                            Result = Idioma.Select("Objeto= 'MstrMens35'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la serie o el lote según identificador del parte.
                            return;
                        }
                    }
                    if (Convert.ToInt32(S_Cant) <= 0)
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens18'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la cantidad.
                        return;
                    }
                    if (ViewState["FechaVencPN"].ToString().Equals("1"))
                    {
                        string Mensj = Cnx.ValidarFechas2(S_Fecha.Trim(), "", 1);
                        if (!Mensj.ToString().Trim().Equals(""))
                        {
                            Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { Mensj = row["Texto"].ToString().Trim(); }
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                            Page.Title = ViewState["PageTit"].ToString();
                        }
                    }
                    S_Fecha = Cnx.ReturnFecha(S_Fecha);//
                    S_Fecha = S_Fecha.Equals("") ? "01/01/1900" : S_Fecha;
                    double D_TtlFinal = 0;
                    ViewState["TtlDespacho"] = "0";

                    foreach (DataRow row in DSDetalle.Tables["CurTemporal"].Rows)
                    { if (row["CantIngr"] != DBNull.Value) { ViewState["TtlDespacho"] = Convert.ToDouble(ViewState["TtlDespacho"]) + Convert.ToDouble(row["CantIngr"].ToString()); } }
                    if (ViewState["Identif"].ToString().Equals("PN") && Convert.ToDouble(ViewState["TtlDespacho"]) > 0)
                    {
                        Result = Idioma.Select("Objeto= 'Msj04EntC'");
                        foreach (DataRow Row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//No es posible ingresar más de un registro cuando el identificador es por PN.
                        return;
                    }
                    D_TtlFinal = Convert.ToDouble(ViewState["TtlDespacho"]) + Convert.ToDouble(S_Cant);
                    //D_TtlDespacho = DSDetalle.Tables["CurTemporal"].AsEnumerable().Where(x => x.Field<string>("PN") !="").Sum(x => x.Field<double>("CantIngr"));
                    if (Convert.ToDouble(LblAsigCantCompV.Text) < Convert.ToDouble(LblAsigCantRecV.Text) + D_TtlFinal)
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens37'");
                        foreach (DataRow Row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//La cantidad ingresada supera la cantidad registrada en el documento.
                        return;
                    }
                    DSDetalle.Tables["CurTemporal"].Rows.Add(ViewState["IdDetOrdenCompra"], ViewState["CodRef"], ViewState["PN"], S_SnLot, Convert.ToInt32(S_Cant), Convert.ToDateTime(S_Fecha), ViewState["CodTipoElem"], ViewState["Identif"].ToString(), ViewState["DT"], ViewState["MT"], ViewState["AT"], Convert.ToDouble(ViewState["Trm"]));
                    DSDetalle.Tables["CurTemporal"].AcceptChanges();
                    int VbNumReg = DSDetalle.Tables["CurTemporal"].Rows.Count;
                    ViewState["TtlDespacho"] = "0";
                    foreach (DataRow row in DSDetalle.Tables["CurTemporal"].Rows)
                    {
                        if (row["CantIngr"] != DBNull.Value) { ViewState["TtlDespacho"] = Convert.ToDouble(ViewState["TtlDespacho"]) + Convert.ToDouble(row["CantIngr"].ToString()); }
                        object value = row["PN"];
                        if (value == DBNull.Value) { if (VbNumReg > 1) { row.Delete(); } }
                    }
                    DSDetalle.Tables["CurTemporal"].AcceptChanges();
                    BindGridTmp();
                    (GrdTemp.FooterRow.FindControl("TxtSnLotPP") as TextBox).Focus();
                }
                catch (Exception Ex) { string S_Ex = Ex.ToString(); }
            }
        }
        protected void GrdTemp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            int index = Convert.ToInt32(e.RowIndex);
            DSDetalle.Tables["CurTemporal"].Rows[index].Delete();
            DSDetalle.Tables["CurTemporal"].AcceptChanges();
            BindGridTmp();
        }
        protected void GrdTemp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
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
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
                TextBox SnLote = e.Row.FindControl("TxtSnLotPP") as TextBox;
                TextBox TxtCant = e.Row.FindControl("TxtCant") as TextBox;
                SnLote.Enabled = true;
                if (ViewState["Identif"].ToString().Equals("PN")) { SnLote.Enabled = false; }
                else { TxtCant.Enabled = false; TxtCant.Text = "1"; }
                TextBox TxtFechExp = e.Row.FindControl("TxtFechExp") as TextBox;
                TxtFechExp.Enabled = true;
                if (ViewState["FechaVencPN"].ToString().Equals("0")) { TxtFechExp.Enabled = false; }
            }
        }
        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            DataRow[] Result;
            try
            {
                if (DSDetalle.Tables["CurActualizar"].Rows.Count > 0)
                {
                    if (TxtObserv.Text.Trim().Equals(""))
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens22'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//debe ingresar la observacion
                        return;
                    }
                    if (DdlAlmacen.Text.Trim().Equals("0"))
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens19'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el almacén.
                        return;
                    }

                    DataTable DT = DSDetalle.Tables["CurActualizar"];

                    DataView DV = DT.DefaultView;
                    DV.Sort = "CodOrdenCompra ASC, Pos ASC";
                    DT = DV.ToTable();
                    GrdVisualizar.DataSource = DT; GrdVisualizar.DataBind();
                    LblNumDocVlorGuardar.Text = DdlNumCompra.Text.Trim();
                    MultVw.ActiveViewIndex = 2;
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void IbtCloseGuardar_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); MultVw.ActiveViewIndex = 0; }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int VbAfectaInv = 0;
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            try
            {
                List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
                foreach (DataRow Row in DSDetalle.Tables["CurActualizar"].Rows)
                {
                    string VbSCodTerc = Row["CodTercero"].ToString().Trim();

                    if (!VbSCodTerc.Equals("")) { VbAfectaInv = 1; }
                    var TypDetalle = new CsInsertElementoAlmacen()
                    {
                        IdIE = Convert.ToInt32(0),// ----------- ojo--------------
                        CodElemento = Row["CodElemento"].ToString().Trim(),
                        CodReferencia = Row["CodReferencia"].ToString().Trim(),
                        PN = Row["PN"].ToString(),
                        SN = Row["SN"].ToString(),
                        Lote = Row["NumLote"].ToString(),
                        CodTipoElem = Row["CodTipoElem"].ToString(),
                        Identificador = Row["IdentificadorElem"].ToString().Trim(),
                        Descripcion = "",
                        Cantidad = Convert.ToDouble(Row["CantDespchr"].ToString().Trim()) * Convert.ToDouble(Row["Equivalencia"].ToString().Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(Row["VlrUnd"].ToString()),// validar el valor  **********************************
                        CodUndMed = Row["CodUndMedR"].ToString().Trim(),
                        IdAlmacen = Convert.ToInt32(DdlAlmacen.Text.Trim()),
                        CodBodega = Row["CodUbicaBodega"].ToString().Trim(),
                        CodShippingOrder = DdlNumCompra.Text.Trim(),
                        Posicion = Row["Pos"].ToString().Trim(),
                        CodAeronave = Convert.ToInt32(0),
                        Matricula = "",
                        DiaTasa = Row["DiaTasa"].ToString().Trim(),
                        MesTasa = Row["MesTasa"].ToString().Trim(),
                        AnoTasa = Row["AnoTasa"].ToString().Trim(),
                        VlorTasaDM = Convert.ToDouble(Row["VlrTRM"].ToString()),
                        CodTipoMoneda = Row["CodTipoMoneda"].ToString().Trim(),
                        DocumentoNro = Row["DocumentoNro"].ToString().Trim(),
                        PosicionDocumento = Convert.ToInt32(Row["PosicionDocumento"].ToString().Trim()),
                        Cant_Compra = Convert.ToDouble(Row["Cant_Compra"].ToString().Trim()),
                        Valor_Compra = Convert.ToDouble(Row["Valor_Compra"].ToString().Trim()),
                        UndMed_Compra = Row["UndMed_Compra"].ToString().Trim(),
                        FacturaNro = Row["FacturaNro"].ToString().Trim(),
                        NumSolPed = Row["NumSolPed"].ToString().Trim(),
                        CCosto = Row["CCosto"].ToString().Trim(),
                        AfectaInventario = VbAfectaInv,
                        CostoImportacion = Convert.ToDouble(Row["VlrImpor"].ToString()),
                        CodTercero = Row["CodTercero"].ToString().Trim(),
                        Consignacion = Convert.ToInt32(0),
                        CodIdUbicacion = Convert.ToInt32(Row["CodIdUbicacion"].ToString().Trim()),
                        FechaVence = Convert.ToDateTime(Row["FechaShelfLife"]),
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = "",
                        Proceso = "0001",
                        IdDetPropHk = Convert.ToInt32(0),
                        IdPPt = Convert.ToInt32(Row["PPT"]),
                        Accion = "ENTRADA",
                    };
                    ObjDetalle.Add(TypDetalle);
                }
                CsInsertElementoAlmacen ClaseIEA = new CsInsertElementoAlmacen();
                ClaseIEA.FormOrigen(ViewState["PFileName"].ToString());
                ClaseIEA.Alimentar(ObjDetalle);
                string Mensj = ClaseIEA.GetMensj();
                if (!Mensj.Equals(""))
                {
                    string VblPn = ClaseIEA.GetPn().Trim().Equals("") ? "" : "  [P/N: " + ClaseIEA.GetPn().Trim() + "]  ";
                    string VblSn = ClaseIEA.GetSn().Trim().Equals("") ? "" : " [S/N: " + ClaseIEA.GetSn().Trim() + "] ";
                    string VbLote = ClaseIEA.GetLote().Trim().Equals("") ? "" : " [LT/N: " + ClaseIEA.GetLote().Trim() + "]";
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VblPn + VblSn + "');", true);
                    return;
                }
                TxtObserv.Text = "";
                TraerDatos("UPD");
                //BindRva();
                BindDetCompra("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, VbcatNArc, Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}