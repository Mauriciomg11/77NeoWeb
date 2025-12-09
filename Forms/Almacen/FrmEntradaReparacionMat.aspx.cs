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
    public partial class FrmEntradaReparacionMat : System.Web.UI.Page
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
                ViewState["TipoCompra"] = "";
                ViewState["CodOrdenCompra"] = "";
                ViewState["CodOCAnt"] = "";
                ViewState["PosicionAnt"] = "0";
                ViewState["TtlDespacho"] = "0";
                ModSeguridad();
                TraerDatos("UPD");
                TipoRepa("N");
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
                    if (bO.Equals("Caption")) { Page.Title = bT; ViewState["PageTit"] = bT; TitForm.Text = bT; }
                    TitForm.Text = bO.Equals("Titulo") ? bT : TitForm.Text;
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblAlmacen.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacen.Text;
                    RdbNacional.Text = bO.Equals("RdbNal") ? "&nbsp" + bT : RdbNacional.Text;
                    RdbInter.Text = bO.Equals("RdbInter") ? "&nbsp" + bT : RdbInter.Text;
                    LblNumRepa.Text = bO.Equals("LblDoc") ? bT : LblNumRepa.Text;
                    LblMoneda.Text = bO.Equals("LblMonedaMstr") ? bT : LblMoneda.Text;
                    LblTitCondManiplc.Text = bO.Equals("LblCondAlmaMstr") ? bT : LblTitCondManiplc.Text;
                    BtnCloseMdl.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseMdl.Text;
                    BtnGuardar.Text = bO.Equals("BotonIngOk") ? bT : BtnGuardar.Text;

                    // *********************************************** Detalle Repa ***********************************************
                    GrdDtlleRepa.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleRepa.EmptyDataText;
                    GrdDtlleRepa.Columns[1].HeaderText = bO.Equals("LblDoc") ? bT : GrdDtlleRepa.Columns[1].HeaderText;
                    GrdDtlleRepa.Columns[2].HeaderText = bO.Equals("PosMstr") ? bT : GrdDtlleRepa.Columns[2].HeaderText;
                    GrdDtlleRepa.Columns[3].HeaderText = bO.Equals("GrdRepa") ? bT : GrdDtlleRepa.Columns[3].HeaderText;
                    GrdDtlleRepa.Columns[4].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDtlleRepa.Columns[4].HeaderText;
                    GrdDtlleRepa.Columns[5].HeaderText = bO.Equals("Descripcion") ? bT : GrdDtlleRepa.Columns[5].HeaderText;
                    GrdDtlleRepa.Columns[6].HeaderText = bO.Equals("TipoMstr") ? bT : GrdDtlleRepa.Columns[6].HeaderText;
                    GrdDtlleRepa.Columns[7].HeaderText = bO.Equals("LblIdentifMstr") ? bT : GrdDtlleRepa.Columns[7].HeaderText;
                    GrdDtlleRepa.Columns[10].HeaderText = bO.Equals("GrdCantRep") ? bT : GrdDtlleRepa.Columns[10].HeaderText;
                    GrdDtlleRepa.Columns[11].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtlleRepa.Columns[11].HeaderText;
                    GrdDtlleRepa.Columns[12].HeaderText = bO.Equals("GrdCantIngr") ? bT : GrdDtlleRepa.Columns[12].HeaderText;
                    GrdDtlleRepa.Columns[13].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtlleRepa.Columns[13].HeaderText;
                    GrdDtlleRepa.Columns[14].HeaderText = bO.Equals("LblFactMstr") ? bT : GrdDtlleRepa.Columns[14].HeaderText;
                    GrdDtlleRepa.Columns[15].HeaderText = bO.Equals("LblFechTRMMstr") ? bT : GrdDtlleRepa.Columns[15].HeaderText;
                    GrdDtlleRepa.Columns[17].HeaderText = bO.Equals("GrdSalRepGarnt") ? bT : GrdDtlleRepa.Columns[17].HeaderText;
                    // *********************************************** Asignar ***********************************************

                    LblTitAsigFis.Text = bO.Equals("LblTitAsigFis") ? bT : LblTitAsigFis.Text;
                    LblFactAsign.Text = " | " + GrdDtlleRepa.Columns[14].HeaderText + ":";
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;
                    GrdAsignar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleRepa.EmptyDataText;
                    GrdAsignar.Columns[1].HeaderText = bO.Equals("GrdEstadoPN") ? bT : GrdAsignar.Columns[1].HeaderText;
                    GrdAsignar.Columns[4].HeaderText = bO.Equals("LoteMst") ? bT : GrdAsignar.Columns[4].HeaderText;
                    GrdAsignar.Columns[5].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdAsignar.Columns[5].HeaderText;
                    GrdAsignar.Columns[6].HeaderText = bO.Equals("FilaMstr") ? bT : GrdAsignar.Columns[6].HeaderText;
                    GrdAsignar.Columns[7].HeaderText = bO.Equals("ColumMstr") ? bT : GrdAsignar.Columns[7].HeaderText;
                    GrdAsignar.Columns[8].HeaderText = bO.Equals("GrdCantStockMstr") ? bT : GrdAsignar.Columns[8].HeaderText;
                    GrdAsignar.Columns[9].HeaderText = bO.Equals("GrdCantRep") ? bT : GrdAsignar.Columns[9].HeaderText;
                    GrdAsignar.Columns[10].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdAsignar.Columns[10].HeaderText;
                    GrdAsignar.Columns[11].HeaderText = bO.Equals("GrdBodDest") ? bT : GrdAsignar.Columns[11].HeaderText;
                    GrdAsignar.Columns[12].HeaderText = bO.Equals("FechVencMstr") ? bT : GrdAsignar.Columns[12].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnClMstr'");//¿Desea realizar el movimiento?
                foreach (DataRow row in Result)
                { BtnGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/

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
                    string VbTxtSql = "EXEC PNTLL_Reparacion 7,@U,'','','','','',0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";

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
                                DSTDdl.Tables[1].TableName = "RepaNal";
                                DSTDdl.Tables[2].TableName = "RepaInta";
                                DSTDdl.Tables[3].TableName = "AplcComex";
                                /* DSTDdl.Tables[3].TableName = "EjecCodigo";*/
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacen.DataSource = DSTDdl.Tables["Almac"];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            }
        }
        protected void TipoRepa(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            ViewState["TipoRepa"] = Tipo;
            if (Tipo.Equals("N"))
            {
                if (DSTDdl.Tables["RepaNal"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaNal"];
                    DdlNumRepa.DataTextField = "CodReparacion";
                }
            }
            else
            {
                if (DSTDdl.Tables["RepaInta"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaInta"];
                    DdlNumRepa.DataTextField = "CodShippingOrder";
                }
            }
            DdlNumRepa.DataValueField = "Codigo";
            DdlNumRepa.DataBind();
            DdlNumRepa.Text = "";
            /*GrdDtlleRepa.DataSource = null;
            GrdDtlleRepa.DataBind();*/
        }
        protected void RdbNacional_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("N"); }
        protected void RdbInter_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("I"); }
        protected void BindDetRepa(string Accion)
        {
            try
            {
                if (DdlNumRepa.Text.Trim().Equals("")) { return; }
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                string S_RepaNAL_INTA = "";
                if (ViewState["TipoRepa"].ToString().Equals("N")) { S_RepaNAL_INTA = "RepaNal"; }
                else { S_RepaNAL_INTA = "RepaInta"; }
                if (DSTDdl.Tables[S_RepaNAL_INTA].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[S_RepaNAL_INTA].Clone();
                    DataRow[] DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo='" + DdlNumRepa.Text.Trim() + "'");
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DT = DR.CopyToDataTable();
                        TxtMoneda.Text = DT.Rows[0]["CodMoneda"].ToString().Trim();
                        DataTable DTEC = new DataTable();
                        DataRow[] Result;
                        DataRow[] DREC;
                        string S_AplicaComex = "N";
                        if (ViewState["TipoRepa"].ToString().Equals("I")) //Si es internacional valida que este liquidada la orden de embarque
                        {
                            DREC = DSTDdl.Tables["AplcComex"].Select("Caso = 1 AND EjecutarCodigo = 'S'"); //Aplica COMEX
                            if (Cnx.ValidaDataRowVacio(DREC))
                            {
                                S_AplicaComex = "S";
                                DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("EstadoLiquidacion <> 1");//Esta liquidada
                                if (Cnx.ValidaDataRowVacio(DR))
                                {
                                    DT = DR.CopyToDataTable();
                                    Result = Idioma.Select("Objeto= 'Mens07EntRepa'"); //La orden de embarque no se encuentra liquidada.
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DdlNumRepa.Text.Trim() + "');", true); }
                                    GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                                    // return;
                                }
                            }
                        } /**/
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Aprobado = 0");
                        if (Cnx.ValidaDataRowVacio(DR))// Si la reparación esta aprobada
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Mens03EntRepa'"); //La reparación no se encuentra aprobada.
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true); }
                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Asentado = 1");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la reparación esta asentada*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Mens04EntRepa'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true); }// La reparación se encuentra asentada.
                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Reparable = 0");
                        if (Cnx.ValidaDataRowVacio(DR))/* El P/N no se encuentra marcado como reparable*/
                        {
                            DT = DR.CopyToDataTable();
                            Result = Idioma.Select("Objeto= 'Mens05EntRepa'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | P/N: " + DT.Rows[0]["PN"].ToString().Trim() + "');", true); }// La reparación se encuentra asentada.
                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        if (Accion.Equals("UPD"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                            {
                                string VbTxtSql = " EXEC PNTLL_Reparacion 8, @CodRp, @ApliComex,'','','',@TipoRP,0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                                sqlConB.Open();
                                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                                {
                                    SC.Parameters.AddWithValue("@CodRp", DdlNumRepa.Text.Trim());
                                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@TipoRP", ViewState["TipoRepa"]);
                                    SC.Parameters.AddWithValue("@ApliComex", S_AplicaComex);
                                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                                    {
                                        using (DataSet DSDetalle = new DataSet())
                                        {
                                            SDA.SelectCommand = SC;
                                            SDA.Fill(DSDetalle);
                                            DSDetalle.Tables[0].TableName = "DetRepa";
                                            DSDetalle.Tables[1].TableName = "CondManip";
                                            DSDetalle.Tables[2].TableName = "CurTemporal";
                                            DSDetalle.Tables[3].TableName = "CurCCosto";/**/
                                            ViewState["DSDetalle"] = DSDetalle;
                                        }
                                    }
                                }
                            }
                        }
                        DSDetalle = (DataSet)ViewState["DSDetalle"];
                        if (DSDetalle.Tables["DetRepa"].Rows.Count > 0)
                        { GrdDtlleRepa.DataSource = DSDetalle.Tables["DetRepa"]; }
                        GrdDtlleRepa.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Detalle Entrada Repación", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlNumRepa_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            BindDetRepa("UPD");
        }
        protected void BindAsignar(string PN, string SN, string CodTipoElem, string CantRepa, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Reparacion 9, @Alm, @CodRepa, @PN, @SN, @IN, '', @CantRepa,0,0, @Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Alm", DdlAlmacen.Text.Trim());
                        SC.Parameters.AddWithValue("@CodRepa", ViewState["CodOrdenRepa"]);
                        SC.Parameters.AddWithValue("@PN", PN.Trim());
                        SC.Parameters.AddWithValue("@SN", SN.Trim());
                        SC.Parameters.AddWithValue("@IN", ViewState["TipoRepa"].ToString().Equals("I") ? "INTA" : "NALS");
                        SC.Parameters.AddWithValue("@CantRepa", CantRepa.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSAsignar = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSAsignar);

                                DSAsignar.Tables[0].TableName = "Asignar";
                                DSAsignar.Tables[1].TableName = "Bodegas";

                                ViewState["DSAsignar"] = DSAsignar;
                            }
                        }
                    }
                }
            }
            DSAsignar = (DataSet)ViewState["DSAsignar"];
            //Actualizar la cantidad a despachar en la vista de Detalle asignar para no despachar de la misma ubica si ya no tiene estok
            foreach (DataRow DRCur in DSDetalle.Tables["CurTemporal"].Rows)
            {
                foreach (DataRow DRAsig in DSAsignar.Tables["Asignar"].Rows)
                {
                    string CodUBCur = DRCur["CodUbicaBodega"].ToString().Trim();
                    string CodUBAsig = DRAsig["CodUbicaBodega"].ToString().Trim();
                    if (CodUBAsig.Equals(CodUBCur))
                    {
                        string CantCur = DRCur["CantIngr"].ToString().Trim();
                        string CantRepaA = DRAsig["Cantidad"].ToString().Trim();
                        DRAsig["Cantidad"] = Convert.ToInt32(CantRepaA) - Convert.ToInt32(CantCur);
                        DSAsignar.Tables["Asignar"].AcceptChanges();
                    }
                }
            }
            if (DSAsignar.Tables["Asignar"].Rows.Count > 0)
            { GrdAsignar.DataSource = DSAsignar.Tables["Asignar"]; }
            GrdAsignar.DataBind();
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
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["PosicionAnt"] = ViewState["Posicion"];
            ViewState["CodRCant"] = ViewState["CodOrdenRepa"];
            MultVw.ActiveViewIndex = 0;
        }
        protected void GrdDtlleRepa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DataRow[] Result;

                if (DdlAlmacen.Text.Trim().Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el almacén.
                    return;
                }
                LblAsigCantSol.Text = GrdDtlleRepa.Columns[10].HeaderText + ":";//Cant Repa
                if (e.CommandName.Equals("Abrir"))
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    string VblCodRef = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                    string VblPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                    string VblSn = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                    string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                    string VbCantRepa = ((Label)row.FindControl("LblCantRepa")).Text.ToString().Trim();
                    ViewState["S_Garnt"] = ((Label)row.FindControl("LblGrnt")).Text.ToString().Trim();
                    ViewState["Identf"] = ((Label)row.FindControl("LblIdentfc")).Text.ToString().Trim();
                    ViewState["CodTipoElem"] = ((Label)row.FindControl("LblTipo")).Text.ToString().Trim();
                    ViewState["CodOrdenRepa"] = ((Label)row.FindControl("LblNumRepa")).Text.ToString().Trim();
                    ViewState["CCosto"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["CCostos"].ToString();
                    ViewState["DT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["DiaTasa"].ToString();
                    ViewState["MT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["MesTasa"].ToString();
                    ViewState["AT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["AñoTasa"].ToString();
                    ViewState["TRM"] = ((Label)row.FindControl("Lbltrm")).Text.ToString().Trim();
                    ViewState["Codigo"] = ((Label)row.FindControl("LblNumDoc")).Text.ToString().Trim();
                    ViewState["Posicion"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                    ViewState["PPT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["PPT"].ToString();
                    ViewState["CodProv"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["CodProveedor"].ToString();
                    ViewState["VlrCotiza"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["Valor_Compra"].ToString();
                    ViewState["ValorUnidadPExp"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["ValorUnidadPExp"].ToString();
                    ViewState["ValorUnidadP"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["ValorUnidadP"].ToString();
                    TxtFact.Text = ((Label)row.FindControl("LblFact")).Text.ToString().Trim();
                    LblPNDescripcAsig.Text = ViewState["CodOrdenRepa"] + " | " + VblPn + " | " + VblDescPN + " | ";
                    LblAsigCantSolV.Text = VbCantRepa;
                    int I_Bloquear = Convert.ToInt32(GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["Bloquear"].ToString());
                    if (I_Bloquear == 1)// Si la Compra esta aprobada
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens15'"); //El P/N se encuentra bloqueado.
                        foreach (DataRow DRM in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DRM["Texto"].ToString() + " | " + ViewState["CodOrdenRepa"].ToString().Trim() + "');", true); }
                        return;
                    }
                    BindAsignar(VblPn, VblSn, ViewState["CodTipoElem"].ToString(), VbCantRepa, "UPD");
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Seleccionar Ubicacion Ent Repa", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdDtlleRepa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdRepaAbrirTT'");
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
        protected void GrdAsignar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();

                if (e.CommandName.Equals("Asignar"))
                {
                    double VbDCantDesp = 0;
                    DataRow[] Result;
                    DSAsignar = (DataSet)ViewState["DSAsignar"];
                    DSDetalle = (DataSet)ViewState["DSDetalle"];
                    int I_AfectInv = 0;
                    double D_VlrRepa = 0;
                    // Almacena la vista para realizar el movimeinto de descargue

                    GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow Gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    int VbDCantStock = Convert.ToInt32(((Label)Row.FindControl("LblStock")).Text.ToString().Trim().Equals("") ? "0" : ((Label)Row.FindControl("LblStock")).Text.ToString().Trim());
                    VbDCantDesp = Convert.ToInt32(((TextBox)Row.FindControl("TxtCantRepa")).Text.ToString().Trim().Equals("") ? "0" : ((TextBox)Row.FindControl("TxtCantRepa")).Text.ToString().Trim());
                    DateTime VbDFech;
                    if (VbDCantStock <= 0 && VbDCantStock < VbDCantDesp)
                    {
                        Result = Idioma.Select("Objeto= 'Mens02SalRepa'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// En esta ubicación ya fue asignada una reparación.
                        return;
                    }
                    string VbSRef = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodReferencia"].ToString();
                    string VbSPN = ((Label)Row.FindControl("LblPn")).Text.ToString().Trim();
                    string VbSSN = ((Label)Row.FindControl("LblSn")).Text.ToString().Trim();
                    TextBox TxtFecVen = (GrdAsignar.Rows[Gvr.RowIndex].FindControl("TxtFecVen") as TextBox);
                    if (TxtFecVen.Text.Trim().Equals("")) { VbDFech = Convert.ToDateTime("01/01/1900"); }
                    else { VbDFech = Convert.ToDateTime(TxtFecVen.Text.Trim()); }
                    double D_VlrCot = Convert.ToDouble(ViewState["VlrCotiza"]);
                    double D_TRM = Convert.ToDouble(ViewState["TRM"]);
                    double D_CostoComex = Convert.ToDouble(ViewState["ValorUnidadP"]);
                    double D_VlrUndComxExp = Convert.ToDouble(ViewState["ValorUnidadPExp"]);
                    if (ViewState["S_Garnt"].ToString().Trim().Equals("N"))// Es garantia Valor 0 y no afecta inventario
                    {
                        I_AfectInv = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodTercero"].ToString().Trim().Equals("") ? 1 : 0;
                        D_VlrRepa = (D_VlrCot * D_TRM) + D_CostoComex + D_VlrUndComxExp;
                    }

                    string S_CodElem = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodElemento"].ToString();
                    string S_CodUM = ((Label)Row.FindControl("LblUndMed")).Text.ToString().Trim();
                    string S_CodUbica = ((DropDownList)Row.FindControl("DdlBogDest")).Text.ToString().Trim();
                    string S_NomBodDes = ((DropDownList)Row.FindControl("DdlBogDest")).SelectedItem.Text.ToString().Trim();
                    string S_CodUbica_Dest = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodUbicaBodega"].ToString();
                    int I_IdUbic = Convert.ToInt32(GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodIdUbicacion"].ToString());
                    if (ViewState["S_Garnt"].ToString().Trim().Equals("N") && S_NomBodDes.Equals("INSP |  |") && Convert.ToDouble(ViewState["VlrCotiza"]) == 0)// No es garantia y va para INSP
                    {
                        Result = Idioma.Select("Objeto= 'Mens06EntRepa'"); //Debe ingresar el valor a la reparación..
                        foreach (DataRow DRM in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DRM["Texto"].ToString() + " | " + ViewState["CodOrdenRepa"].ToString().Trim() + "');", true); }
                        return;
                    }
                    DSDetalle.Tables["CurTemporal"].Rows.Add(ViewState["CodOrdenRepa"], VbSRef, VbSPN, VbSSN, VbDCantDesp, VbDFech, ViewState["CodTipoElem"],
                        ViewState["Identf"], ViewState["DT"], ViewState["MT"], ViewState["AT"], ViewState["TRM"], I_AfectInv, D_VlrRepa, D_CostoComex,
                        S_CodElem, ViewState["CodProv"], S_CodUM, S_CodUbica, ViewState["Codigo"], ViewState["Posicion"], ViewState["CCosto"],
                        Convert.ToInt32(ViewState["PPT"]), S_CodUbica_Dest, I_IdUbic, D_VlrCot, TxtFact.Text.Trim(), D_VlrUndComxExp);
                    DSDetalle.Tables["CurTemporal"].AcceptChanges();

                    //Actualizar la cantidad a despachar en la vista de Detalle Reserva          
                    foreach (DataRow row in DSDetalle.Tables["DetRepa"].Rows)
                    {
                        if (row["CodReparacion"].ToString().Equals(ViewState["CodOrdenRepa"]))
                        {
                            row["CantIngresar"] = VbDCantDesp;
                            row["NumFactura"] = TxtFact.Text.Trim();
                        }
                    }
                    DSDetalle.Tables["DetRepa"].AcceptChanges();
                    BindDetRepa("");
                    MultVw.ActiveViewIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar P/N en salida Repa", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                    DataRow[] Result = Idioma.Select("Objeto='GrdRepaAsigTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsignr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                TextBox TxtFecVen = (e.Row.FindControl("TxtFecVen") as TextBox);
                DropDownList DdlBogDest = (e.Row.FindControl("DdlBogDest") as DropDownList);
                int I_SiTieneFechaV = Convert.ToInt32(DRV["FechaVencimientoR"].ToString());
                string S_CodTercero = DRV["CodTercero"].ToString().Trim();
                if (TxtFecVen != null)
                {
                    TxtFecVen.Text = Cnx.ReturnFecha(DRV["FechaShelfLife"].ToString().Trim());
                    if (I_SiTieneFechaV == 0)
                    {
                        TxtFecVen.Enabled = false;
                        DataRow[] Result = Idioma.Select("Objeto= 'TT01EntRep'");
                        foreach (DataRow row in Result)
                        { TxtFecVen.ToolTip = row["Texto"].ToString(); }// Al P/N no aplica fecha vence.                      
                    }
                }

                DataTable DT = new DataTable();
                DataRow[] DR = DSAsignar.Tables["Bodegas"].Select("CodTercero ='" + S_CodTercero + "'");
                if (Cnx.ValidaDataRowVacio(DR))
                { DT = DR.CopyToDataTable(); }
                DdlBogDest.DataSource = DT;
                DdlBogDest.DataTextField = "Bodega";
                DdlBogDest.DataValueField = "CodUbicaBodega";
                DdlBogDest.DataBind();
            }
        }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            try
            {
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
                        CodElemento = Row["CodElem"].ToString().Trim(),
                        CodReferencia = Row["Referencia"].ToString().Trim(),
                        PN = Row["PN"].ToString(),
                        SN = Row["Identif"].ToString().Equals("SN") ? Row["SnLote"].ToString() : "",
                        Lote = Row["Identif"].ToString().Equals("LOTE") ? Row["SnLote"].ToString() : "",
                        CodTipoElem = Row["CodTipoElem"].ToString(),
                        Identificador = Row["Identif"].ToString().Trim(),
                        Descripcion = "",
                        Cantidad = Convert.ToDouble(Row["CantIngr"].ToString().Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(Row["ValorRepa"].ToString()),
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
                        CodUbicaDest = Row["CodUbicaBodDest"].ToString(),
                        CCosto = Row["CCosto"].ToString().Trim(),
                        AfectaInventario = Convert.ToInt32(Row["AfectaInventario"]),
                        CostoImportacion = Convert.ToDouble(Row["CostoComex"].ToString()),
                        Costo_Export = Convert.ToDouble(Row["Costo_Export"].ToString()),
                        CodTercero = ViewState["CodProv"].ToString().Trim(),
                        Consignacion = Convert.ToInt32(0),
                        CodIdUbicacion = Convert.ToInt32(Row["CodIdUbicacion"].ToString().Trim()),
                        FechaVence = Convert.ToDateTime(Row["FechaExp"].ToString().Trim()),
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = "",
                        Proceso = "0004",
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
                DdlNumRepa.Text = "";
                TxtMoneda.Text = "";
                GrdDtlleRepa.DataSource = null;
                GrdDtlleRepa.DataBind();
                TraerDatos("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Generar Salida Consumo", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}