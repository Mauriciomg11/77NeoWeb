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
    public partial class FrmEntradaIntercambio : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSDetalle = new DataSet();
        DataSet DSAsignar = new DataSet();
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
                ViewState["TipoInterc"] = "";
                ViewState["CodInterc"] = "";
                ViewState["CodICAnt"] = "";
                ViewState["PosicionAnt"] = "0";
                ViewState["TtlDespacho"] = "0";
                ModSeguridad();
                TraerDatos("UPD");
                TipoInterc("N");
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
                SC.Parameters.AddWithValue("@F2", "frmEntradaCompraMat");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("CaptionEI"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; TitForm.Text = bT; }
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblAlmacen.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacen.Text;
                    RdbNacional.Text = bO.Equals("RdbNal") ? "&nbsp" + bT : RdbNacional.Text;
                    RdbInter.Text = bO.Equals("RdbInter") ? "&nbsp" + bT : RdbInter.Text;
                    LblNumInterc.Text = bO.Equals("DocMstr") ? bT : LblNumInterc.Text;
                    LblMoneda.Text = bO.Equals("LblMonedaMstr") ? bT : LblMoneda.Text;
                    BtnGuardar.Text = bO.Equals("BotonIngOk") ? bT : BtnGuardar.Text;
                    // *********************************************** Detalle Compras ***********************************************
                    GrdDtlleInterc.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleInterc.EmptyDataText;
                    GrdDtlleInterc.Columns[1].HeaderText = bO.Equals("DocMstr") ? bT : GrdDtlleInterc.Columns[1].HeaderText;
                    GrdDtlleInterc.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDtlleInterc.Columns[3].HeaderText;
                    GrdDtlleInterc.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdDtlleInterc.Columns[4].HeaderText;
                    GrdDtlleInterc.Columns[5].HeaderText = bO.Equals("TipoMstr") ? bT : GrdDtlleInterc.Columns[5].HeaderText;
                    GrdDtlleInterc.Columns[6].HeaderText = bO.Equals("LblIdentifMstr") ? bT : GrdDtlleInterc.Columns[6].HeaderText;
                    GrdDtlleInterc.Columns[9].HeaderText = bO.Equals("CantInterc") ? bT : GrdDtlleInterc.Columns[9].HeaderText;
                    GrdDtlleInterc.Columns[10].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtlleInterc.Columns[10].HeaderText;
                    GrdDtlleInterc.Columns[11].HeaderText = bO.Equals("GrdCantRec") ? bT : GrdDtlleInterc.Columns[11].HeaderText;
                    GrdDtlleInterc.Columns[12].HeaderText = bO.Equals("GrdCantIngres") ? bT : GrdDtlleInterc.Columns[12].HeaderText;
                    GrdDtlleInterc.Columns[13].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtlleInterc.Columns[13].HeaderText;
                    GrdDtlleInterc.Columns[14].HeaderText = bO.Equals("LblFactMstr") ? bT : GrdDtlleInterc.Columns[14].HeaderText;
                    GrdDtlleInterc.Columns[15].HeaderText = bO.Equals("LblFechTRMMstr") ? bT : GrdDtlleInterc.Columns[15].HeaderText;
                    // *********************************************** Asignar ***********************************************
                    LblTitAsigFis.Text = bO.Equals("LblTitAsigFis") ? bT : LblTitAsigFis.Text;
                    LblAsigCantSol.Text = GrdDtlleInterc.Columns[9].HeaderText + ":";                  
                    LblFactAsign.Text = " | " + GrdDtlleInterc.Columns[14].HeaderText + ":";
                    GrdAsignar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAsignar.EmptyDataText;
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;
                    GrdAsignar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAsignar.EmptyDataText;
                    GrdAsignar.Columns[3].HeaderText = bO.Equals("GrdCant") ? bT : GrdAsignar.Columns[3].HeaderText;
                    GrdAsignar.Columns[4].HeaderText = bO.Equals("GrdCantDesp") ? bT : GrdAsignar.Columns[4].HeaderText;
                    GrdAsignar.Columns[5].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdAsignar.Columns[5].HeaderText;
                    GrdAsignar.Columns[6].HeaderText = bO.Equals("FechVencMstr") ? bT : GrdAsignar.Columns[6].HeaderText;
                    /**/
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnClMstr'");
                foreach (DataRow row in Result)
                { BtnGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");/**/ }
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
                    string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 9, @U,'','','','',0,0,@Idm, @ICC,'01-01-1','02-01-01','03-01-01'";

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
                                DSTDdl.Tables[1].TableName = "IntercNal";
                                DSTDdl.Tables[2].TableName = "IntercInta";
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
                DdlAlmacen.DataSource = DSTDdl.Tables["Almac"];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            } /**/
        }
        protected void TipoInterc(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            ViewState["TipoInterc"] = Tipo;
            if (Tipo.Equals("N"))
            {
                if (DSTDdl.Tables["IntercNal"].Rows.Count > 0)
                {
                    DdlNumInterc.DataSource = DSTDdl.Tables["IntercNal"];
                    DdlNumInterc.DataTextField = "CodOrdenCompra";
                }
            }
            else
            {
                if (DSTDdl.Tables["IntercInta"].Rows.Count > 0)
                {
                    DdlNumInterc.DataSource = DSTDdl.Tables["IntercInta"];
                    DdlNumInterc.DataTextField = "CodShippingOrder";
                }
            }
            DdlNumInterc.DataValueField = "Codigo";
            DdlNumInterc.DataBind();
            DdlNumInterc.Text = "";
            GrdDtlleInterc.DataSource = null;
            GrdDtlleInterc.DataBind();/**/
        }
        protected void BindDetInterc(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                string S_NAL_INTA = "";
                if (ViewState["TipoInterc"].ToString().Equals("N")) { S_NAL_INTA = "IntercNal"; }
                else { S_NAL_INTA = "IntercInta"; }
                //ViewState["CodInterc"] = DSTDdl.Tables[S_NAL_INTA].AsEnumerable().Where(x => x.Field<string>("Codigo") == DdlNumInterc.Text.Trim()).Select(x => x.Field<string>("CodOrdenCompra")).FirstOrDefault();
                if (DSTDdl.Tables[S_NAL_INTA].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[S_NAL_INTA].Clone();
                    DataRow[] DR = DSTDdl.Tables[S_NAL_INTA].Select("Codigo='" + DdlNumInterc.Text.Trim() + "'");
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DT = DR.CopyToDataTable();
                        TxtMoneda.Text = DT.Rows[0]["CodMoneda"].ToString().Trim();
                        DataTable DTEC = new DataTable();
                        DataRow[] DREC; DataRow[] Result;
                        string S_AplicaComex = "S";
                        if (ViewState["TipoInterc"].ToString().Equals("I"))/*Si es internacional valida que este liquidada la orden de embarque*/
                        {
                            DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 5 AND EjecutarCodigo = 'S'"); /*Si aplica validacion de la liquidacion*/
                            if (Cnx.ValidaDataRowVacio(DREC))
                            {
                                DR = DSTDdl.Tables[S_NAL_INTA].Select("EstadoLiquidacion <> 1"); /*Esta liquidada*/
                                if (Cnx.ValidaDataRowVacio(DR))
                                {
                                    DT = DR.CopyToDataTable();
                                    Result = Idioma.Select("Objeto= 'Msj05EntC'"); /*La orden de embarque no se encuentra liquidada.	*/
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DdlNumInterc.Text.Trim() + "');", true); }
                                    GrdDtlleInterc.DataSource = null; GrdDtlleInterc.DataBind();
                                    return;
                                }
                            }
                            DREC = DSTDdl.Tables["EjecCodComex"].Select("Caso = 1 AND EjecutarCodigo = 'N'"); /*Aplica COMEX*/
                            if (Cnx.ValidaDataRowVacio(DREC)) { S_AplicaComex = "N"; }
                        }
                        DR = DSTDdl.Tables[S_NAL_INTA].Select("Codigo ='" + DdlNumInterc.Text.Trim() + "' AND Aprobado = 0");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta aprobada*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Msj01EntC'"); /*La orden de compra no se encuentra aprobada*/
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodOrdenCompra"].ToString().Trim() + "');", true); }
                            GrdDtlleInterc.DataSource = null; GrdDtlleInterc.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_NAL_INTA].Select("Codigo ='" + DdlNumInterc.Text.Trim() + "' AND Asentado = 1");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta asentada*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Msj02EntC'"); /*La orden de compra se encuentra asentada.*/
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodOrdenCompra"].ToString().Trim() + "');", true); }
                            GrdDtlleInterc.DataSource = null; GrdDtlleInterc.DataBind();
                            return;
                        }
                        if (Accion.Equals("UPD"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                            {
                                string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 10,@CodOC,@TipoOC,@ApliComex,'','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                                sqlConB.Open();
                                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                                {
                                    SC.Parameters.AddWithValue("@CodOC", DdlNumInterc.Text.Trim());
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@TipoOC", ViewState["TipoInterc"]);
                                    SC.Parameters.AddWithValue("@ApliComex", S_AplicaComex);
                                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                                    {
                                        using (DataSet DSDetalle = new DataSet())
                                        {
                                            SDA.SelectCommand = SC;
                                            SDA.Fill(DSDetalle);
                                            DSDetalle.Tables[0].TableName = "EstadoInterc";
                                            DSDetalle.Tables[1].TableName = "CondManip";
                                            DSDetalle.Tables[2].TableName = "CurTemporal";
                                            ViewState["DSDetalle"] = DSDetalle;
                                        }
                                    }
                                }
                            }
                        }
                        DSDetalle = (DataSet)ViewState["DSDetalle"];
                        if (DSDetalle.Tables["EstadoInterc"].Rows.Count > 0)
                        { GrdDtlleInterc.DataSource = DSDetalle.Tables["EstadoInterc"]; }
                        GrdDtlleInterc.DataBind();
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
        protected void RdbNacional_CheckedChanged(object sender, EventArgs e)
        { TipoInterc("N"); }
        protected void RdbInter_CheckedChanged(object sender, EventArgs e)
        { TipoInterc("I"); }
        protected void DdlNumInterc_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (!DdlNumInterc.SelectedItem.Value.Equals("")) { BindDetInterc("UPD"); }
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
        protected void BindAsignar(string PN, string CodRef, string CantInterc, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "SP_PANTALLA_Entrada_Compra 11, @CodInterc, @PN, @CodRef,'','',@CantInterc,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        string borr = ViewState["CodInterc"].ToString();
                        string borr1 = ViewState["Codigo"].ToString();
                        SC.Parameters.AddWithValue("@CodInterc", ViewState["CodInterc"]);
                        SC.Parameters.AddWithValue("@PN", PN.Trim());
                        SC.Parameters.AddWithValue("@CodRef", CodRef.Trim());
                        SC.Parameters.AddWithValue("@CantInterc", CantInterc.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSAsignar = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSAsignar);

                                DSAsignar.Tables[0].TableName = "Asignar";
                                DSAsignar.Tables[1].TableName = "PN_Alter";
                                DSAsignar.Tables[2].TableName = "FechSvr";
                                ViewState["DSAsignar"] = DSAsignar;
                            }
                        }
                    }
                }
            }
            DSAsignar = (DataSet)ViewState["DSAsignar"];

            if (DSAsignar.Tables["Asignar"].Rows.Count > 0)
            { GrdAsignar.DataSource = DSAsignar.Tables["Asignar"]; }
            GrdAsignar.DataBind();
        }
        protected void GrdDtlleInterc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                DataRow[] Result;
                if (RdbNacional.Checked == false && RdbInter.Checked == false) { RdbNacional.Focus(); return; }
                if (DdlAlmacen.Text.Trim().Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el almacén.
                    return;
                }
                if (e.CommandName.Equals("Abrir"))
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    string VblCodRef = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                    ViewState["Pn"] = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                    string VblSn = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                    string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                    string VbCantIC = ((Label)row.FindControl("LblCantInterc")).Text.ToString().Trim();
                    ViewState["CodInterc"] = ((Label)row.FindControl("LblNumIC")).Text.ToString().Trim();
                    ViewState["Identf"] = ((Label)row.FindControl("LblIdentfc")).Text.ToString().Trim();
                    ViewState["CodTipoElem"] = ((Label)row.FindControl("LblTipo")).Text.ToString().Trim();
                    ViewState["CCosto"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["CCostos"].ToString();
                    ViewState["DT"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["DiaTasa"].ToString();
                    ViewState["MT"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["MesTasa"].ToString();
                    ViewState["AT"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["AñoTasa"].ToString();
                    ViewState["TRM"] = ((Label)row.FindControl("Lbltrm")).Text.ToString().Trim();
                    ViewState["Codigo"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["CodShippingOrder"].ToString();
                    ViewState["Posicion"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                    ViewState["PPT"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["PPT"].ToString();
                    ViewState["CodProv"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["CodProveedor"].ToString();
                    ViewState["VlrCotiza"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["Valor_Compra"].ToString();
                    ViewState["VlrComex"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["ValorUnidadP"].ToString();
                    TxtFact.Text = ((Label)row.FindControl("LblFact")).Text.ToString().Trim();
                    ViewState["VlrUnd"] = GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["ValorUnidad"].ToString();
                    LblPNDescripcAsig.Text = ViewState["CodInterc"] + " | " + ViewState["Pn"] + " | " + VblDescPN + " | ";
                    LblAsigCantCompV.Text = VbCantIC;
                    int I_Bloquear = Convert.ToInt32(GrdDtlleInterc.DataKeys[gvr.RowIndex].Values["Bloquear"].ToString());
                    if (I_Bloquear == 1)// Si la Compra esta aprobada
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens15'"); //El P/N se encuentra bloqueado.
                        foreach (DataRow DRM in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DRM["Texto"].ToString() + " | " + ViewState["CodInterc"].ToString().Trim() + "');", true); }
                        return;
                    }
                    if (ViewState["TRM"].Equals("0") && ViewState["TipoInterc"].Equals("I"))
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens36'");
                        foreach (DataRow Row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//La TRM del día no se ha creado.
                        return;
                    }
                    if (Convert.ToDouble(ViewState["VlrCotiza"].ToString()) <= 0)
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens39'");
                        foreach (DataRow Row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//El ítem se encuentra sin un valor ingresado.
                        return;
                    }
                    DataRow[] DREC;
                    DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 3 AND EjecutarCodigo = 'S'"); /*Solicitar en el ingreso de la compra y Repa Verificacion de la Referencia*/
                    if (Cnx.ValidaDataRowVacio(DREC))
                    {
                        DataRow[] DR = DSDetalle.Tables["EstadoInterc"].Select("Revisado = 0 AND CodReferencia = '" + VblCodRef.Trim() + "'"); /*Solicitar en el ingreso de la compra y Repa Verificacion de la Referencia*/
                        if (Cnx.ValidaDataRowVacio(DR))
                        {
                            DataTable DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Msj03EntC'"); /*La referencia no se encuentra verificada*/
                            foreach (DataRow Row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + " | " + ViewState["PN"].ToString().Trim() + "');", true); }
                            return;
                        }
                    }
                    BindAsignar(ViewState["Pn"].ToString(), VblCodRef, VbCantIC, "UPD");
                    MultVw.ActiveViewIndex = 1;
                    BindCondicManipulac(VblCodRef);
                }
                Idioma = (DataTable)ViewState["TablaIdioma"];
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Seleccionar Salida Intercambio", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdDtlleInterc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdAsigICTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                DataRowView DRW = e.Row.DataItem as DataRowView;
                double VbDCanEnt = Convert.ToDouble(DRW["CantIngresar"].ToString().Trim());
                if (VbDCanEnt > 0)
                {
                    IbtAbrir.Visible = false; e.Row.BackColor = System.Drawing.Color.PaleGreen;
                }
            }
        }
        /////////////////////////////////////// ASIGNAR /////////////////////////////////////////////////
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            MultVw.ActiveViewIndex = 0;
        }
        protected void GrdAsignar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                if (e.CommandName.Equals("Asignar"))
                {
                    double VbDCantDesp = 0;
                    DSAsignar = (DataSet)ViewState["DSAsignar"];
                    DSDetalle = (DataSet)ViewState["DSDetalle"];
                    string S_FechaSvr = Cnx.ReturnFecha(DSAsignar.Tables["FechSvr"].Rows[0]["Fecha"].ToString().Trim());
                    // Almacena la vista para realizar el movimeinto de descargue
                    GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow Gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    VbDCantDesp = Convert.ToInt32(((Label)Row.FindControl("LblCantIntC")).Text.ToString().Trim());
                    DateTime VbDFech;
                    string VbSRef = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodReferencia"].ToString();
                    string VbSPN = ((DropDownList)Row.FindControl("DdlPN_Alter")).Text.ToString().Trim();
                    string VbSSN = ((TextBox)Row.FindControl("TxtSN")).Text.ToString().Trim();
                    if (((TextBox)Row.FindControl("TxtFecVen")).Text.ToString().Trim().Equals("")) { VbDFech = Convert.ToDateTime("01/01/1900"); }
                    else { VbDFech = Convert.ToDateTime(((TextBox)Row.FindControl("TxtFecVen")).Text.ToString().Trim()); }
                    double D_VlrInterC = Convert.ToDouble(ViewState["VlrUnd"]) * Convert.ToDouble(ViewState["TRM"]) + Convert.ToDouble(ViewState["VlrComex"].ToString());
                    string S_CodElem = "";
                    string S_CodUM = ((Label)Row.FindControl("LblUndMed")).Text.ToString().Trim();
                    string S_CodUbica = "674";
                    string S_CodUbica_N_I = "";
                    int I_IdUbic = 0;
                    string S_Identfc = GrdAsignar.DataKeys[Gvr.RowIndex].Values["IdentificadorElemR"].ToString().Trim();
                    if (S_Identfc.Equals("SN") && VbSSN.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens27'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// El elemento es serializado, debe ingresar la serie al elemento.
                        return;
                    }
                    if (!((TextBox)Row.FindControl("TxtFecVen")).Text.ToString().Trim().Equals(""))
                    {
                        string VbMnsj = Cnx.ValidarFechas2(S_FechaSvr, ((TextBox)Row.FindControl("TxtFecVen")).Text.ToString().Trim(), 2);
                        if (!VbMnsj.ToString().Trim().Equals(""))
                        {
                            DataRow[] Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                            foreach (DataRow row in Result)
                            { VbMnsj = row["Texto"].ToString().Trim(); }
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                            return;
                        }
                    }
                    // ***<- ACTUALIZA EL CURSOR DETALLE ->***
                    DSDetalle.Tables["CurTemporal"].Rows.Add(ViewState["CodInterc"], VbSRef, VbSPN, VbSSN, VbDCantDesp, VbDFech, ViewState["CodTipoElem"],
                        ViewState["Identf"], ViewState["DT"], ViewState["MT"], ViewState["AT"], ViewState["TRM"], 1, D_VlrInterC, ViewState["VlrComex"],
                        S_CodElem, ViewState["CodProv"], S_CodUM, S_CodUbica, ViewState["Codigo"], ViewState["Posicion"], ViewState["CCosto"],
                        Convert.ToInt32(ViewState["PPT"]), S_CodUbica_N_I, I_IdUbic, ViewState["VlrCotiza"], TxtFact.Text.Trim());
                    DSDetalle.Tables["CurTemporal"].AcceptChanges();
                    //Actualizar la cantidad a despachar en la vista de Detalle Reserva          
                    foreach (DataRow row in DSDetalle.Tables["EstadoInterc"].Rows)
                    {
                        if (row["CodOrdenCompra"].ToString().Equals(ViewState["CodInterc"]))
                        {
                            row["CantIngresar"] = VbDCantDesp;
                            row["NumFacturaOC"] = TxtFact.Text.Trim();
                            row["PN"] = VbSPN;
                            row["SN"] = VbSSN;
                        }
                    }
                    DSDetalle.Tables["EstadoInterc"].AcceptChanges();
                    BindDetInterc("");
                    MultVw.ActiveViewIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar P/N en salida intercambio", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAsignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DSAsignar = (DataSet)ViewState["DSAsignar"];
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRowView DRV = e.Row.DataItem as DataRowView;
                ImageButton IbtAsignr = e.Row.FindControl("IbtAsignr") as ImageButton;
                if (IbtAsignr != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='LblAsigMstr'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsignr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                string S_Identfcdr = DRV["IdentificadorElemR"].ToString().Trim();
                TextBox TxtSN = (e.Row.FindControl("TxtSN") as TextBox);
                if (S_Identfcdr.Equals("PN")) { TxtSN.Enabled = false; }
                int I_SiTieneFechaV = Convert.ToInt32(DRV["FechaVencimientoR"].ToString());
                TextBox TxtFecVen = (e.Row.FindControl("TxtFecVen") as TextBox);
                if (TxtFecVen != null)
                {
                    if (I_SiTieneFechaV == 0)
                    {
                        TxtFecVen.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'TT01EntRep'");
                        foreach (DataRow row in Result)
                        { TxtFecVen.ToolTip = row["Texto"].ToString(); }// Al P/N no aplica fecha vence.                      
                    }
                }
                DropDownList DdlPN_Alter = (e.Row.FindControl("DdlPN_Alter") as DropDownList);
                DdlPN_Alter.DataSource = DSAsignar.Tables["PN_Alter"];
                DdlPN_Alter.DataTextField = "PN";
                DdlPN_Alter.DataValueField = "PN";
                DdlPN_Alter.DataBind();
                DdlPN_Alter.Text = ViewState["Pn"].ToString();
            }
        }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            try
            {
                if (DSDetalle == null) { return; }
                if (DSDetalle.Tables["CurTemporal"].Rows.Count == 0) { return; }

                if (TxtObserv.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens22'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//debe ingresar la observacion
                    TxtObserv.Focus();
                    return;
                }
                List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
                foreach (DataRow Row in DSDetalle.Tables["CurTemporal"].Rows)
                {
                    var TypDetalle = new CsInsertElementoAlmacen()
                    {
                        IdIE = Convert.ToInt32(0),
                        CodElemento = "",
                        CodReferencia = Row["Referencia"].ToString().Trim(),
                        PN = Row["PN"].ToString(),
                        SN = Row["Identif"].ToString().Equals("SN") ? Row["SnLote"].ToString() : "",
                        Lote = Row["Identif"].ToString().Equals("LOTE") ? Row["SnLote"].ToString() : "",
                        CodTipoElem = Row["CodTipoElem"].ToString(),
                        Identificador = Row["Identif"].ToString().Trim(),
                        Descripcion = "",
                        Cantidad = Convert.ToDouble(Row["CantIngr"].ToString().Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(Row["ValorInterc"].ToString()),
                        CodUndMed = Row["CodUM"].ToString(),
                        IdAlmacen = Convert.ToInt32(DdlAlmacen.Text.Trim()),
                        CodBodega = Row["CodUbicaBodega"].ToString().Trim(),
                        CodShippingOrder = Row["CodSO"].ToString().Trim(),
                        Posicion = Row["Pos"].ToString().Trim(),
                        CodAeronave = 0,
                        Matricula = "",
                        DiaTasa = Row["Dia"].ToString().Trim(),
                        MesTasa = Row["Mes"].ToString().Trim(),
                        AnoTasa = Row["ano"].ToString().Trim(),
                        VlorTasaDM = Convert.ToDouble(Row["TRM"].ToString().Trim()),
                        CodTipoMoneda = TxtMoneda.Text.Trim(),
                        DocumentoNro = Row["CodDoc"].ToString().Trim(),
                        PosicionDocumento = 1,
                        Cant_Compra = Convert.ToInt32(Row["CantIngr"].ToString().Trim()),
                        Valor_Compra = Convert.ToDouble(Row["Valor_Compra"].ToString().Trim()),
                        UndMed_Compra = Row["CodUM"].ToString(),
                        FacturaNro = Row["FacturaNro"].ToString().Trim(),
                        NumSolPed = "",
                        CodUbicaDest = "",
                        CCosto = Row["CCosto"].ToString().Trim(),
                        AfectaInventario = Convert.ToInt32(Row["AfectaInventario"]),
                        CostoImportacion = Convert.ToDouble(Row["CostoComex"].ToString()),
                        CodTercero = ViewState["CodProv"].ToString().Trim(),
                        Consignacion = Convert.ToInt32(0),
                        CodIdUbicacion = 0,
                        FechaVence = Convert.ToDateTime(Row["FechaExp"].ToString().Trim()),
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = "",
                        Proceso = "0003",
                        IdDetPropHk = 0,
                        IdPPt = Convert.ToInt32(Row["PPT"].ToString()),
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
                DdlAlmacen.Text = "0";
                RdbNacional.Checked = false;
                RdbInter.Checked = false;
                DdlNumInterc.Text = "";
                TxtMoneda.Text = "";
                GrdDtlleInterc.DataSource = null;
                GrdDtlleInterc.DataBind();
                TraerDatos("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Generar Entrada Intercambio", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}