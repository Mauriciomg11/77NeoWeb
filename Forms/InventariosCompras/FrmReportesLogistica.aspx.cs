using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmReportesLogistica : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSRepara = new DataSet();
        DataSet DSEstdComp = new DataSet();
        DataTable DTCncl = new DataTable();
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
                }
            }
            if (!IsPostBack)
            {
                MlVw.ActiveViewIndex = 0;
                TitForm.Text = "XXX";
                ViewState["NS"] = "77NEO-77NEO";
                string VbM = Convert.ToString(DateTime.UtcNow.Month);
                string VbY = Convert.ToString(DateTime.UtcNow.Year);
                string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, "01");
                DateTime VbFecID = Convert.ToDateTime(fecha);
                TxtFechI.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                TxtFechF.Text = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
                TxtFechECI.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                TxtFechECF.Text = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
                RdbRpAll.Checked = true;
                RdbECAll.Checked = true;
                IdiomaControles();
                BindDdl("UPD");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
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
                SC.Parameters.AddWithValue("@F2", "CurExportLogstcRepa");
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
                    BtnReparaciones.Text = bO.Equals("BtnReparaciones") ? bT : BtnReparaciones.Text;
                    BtnReparaciones.ToolTip = bO.Equals("BtnReparacionesTT") ? bT : BtnReparaciones.ToolTip;
                    BtnCompraPend.Text = bO.Equals("BtnCompraPend") ? bT : BtnCompraPend.Text;
                    BtnCompraPend.ToolTip = bO.Equals("CkbECPend") ? bT : BtnCompraPend.ToolTip;
                    IbtCerrarImpr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpr.ToolTip;
                    // ************************************************************ Conciliacion ************************************************************
                    BtnConciliacion.Text = bO.Equals("BtnConciliacion") ? bT : BtnConciliacion.Text;
                    BtnConciliacion.ToolTip = bO.Equals("BtnConciliacionTT") ? bT : BtnConciliacion.ToolTip;
                    LblTitConciliacion.Text = bO.Equals("LblTitConciliacion") ? bT : LblTitConciliacion.Text;
                    LblAlmacenCnc.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacenCnc.Text;
                    LblFechCorteCnc.Text = bO.Equals("LblFechCorte") ? bT : LblFechCorteCnc.Text;
                    IbtEjecutar.ToolTip = bO.Equals("IbtEjecutar") ? bT : IbtEjecutar.ToolTip;
                    GrdConciliacion.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdConciliacion.EmptyDataText;
                    GrdConciliacion.Columns[0].HeaderText = bO.Equals("Asignac") ? bT : GrdConciliacion.Columns[0].HeaderText;
                    GrdConciliacion.Columns[1].HeaderText = bO.Equals("Descripcion") ? bT : GrdConciliacion.Columns[1].HeaderText;
                    GrdConciliacion.Columns[2].HeaderText = bO.Equals("Vlres") ? bT : GrdConciliacion.Columns[2].HeaderText;
                    IbtCerrarConcil.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarConcil.ToolTip;
                    // ************************************************************ Reparaciones ************************************************************
                    LblTitReparaciones.Text = bO.Equals("BtnReparaciones") ? bT : LblTitReparaciones.Text;
                    LblFechI.Text = bO.Equals("LblFechI") ? bT : LblFechI.Text;
                    LblFechF.Text = bO.Equals("LblFechF") ? bT : LblFechF.Text;
                    RdbRpAll.Text = bO.Equals("RdbRpAll") ? "&nbsp" + bT : RdbRpAll.Text;
                    RdbRpCot.Text = bO.Equals("C39") ? "&nbsp" + bT : RdbRpCot.Text;
                    RdbRpCodRepa.Text = bO.Equals("C40") ? "&nbsp" + bT : RdbRpCodRepa.Text;
                    RdbRpProv.Text = bO.Equals("C26") ? "&nbsp" + bT : RdbRpProv.Text;
                    CkbRpPend.Text = bO.Equals("CkbRpPend") ? "&nbsp" + bT : CkbRpPend.Text;
                    CkbRpPend.ToolTip = bO.Equals("CkbRpPendTT") ? bT : CkbRpPend.ToolTip;
                    if (bO.Equals("placeholder"))
                    { TxtRpDocBusq.Attributes.Add("placeholder", bT); TxtECDocBusq.Attributes.Add("placeholder", bT); }
                    IbtRpBusqueda.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtRpBusqueda.ToolTip;
                    IbtExcelHisAlmaRepa.ToolTip = bO.Equals("BtnReparacionesTT") ? bT : IbtExcelHisAlmaRepa.ToolTip;
                    IbtExpRepaPend.ToolTip = bO.Equals("IbtExpRepaPend") ? bT : IbtExpRepaPend.ToolTip;
                    GrdDetRepa.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDetRepa.EmptyDataText;
                    GrdDetRepa.Columns[0].HeaderText = bO.Equals("C34") ? bT : GrdDetRepa.Columns[0].HeaderText;
                    GrdDetRepa.Columns[1].HeaderText = bO.Equals("C38") ? bT : GrdDetRepa.Columns[1].HeaderText;
                    GrdDetRepa.Columns[2].HeaderText = bO.Equals("C39") ? bT : GrdDetRepa.Columns[2].HeaderText;
                    GrdDetRepa.Columns[3].HeaderText = bO.Equals("C40") ? bT : GrdDetRepa.Columns[3].HeaderText;
                    GrdDetRepa.Columns[4].HeaderText = bO.Equals("C35") ? bT : GrdDetRepa.Columns[4].HeaderText;
                    GrdDetRepa.Columns[5].HeaderText = bO.Equals("C07") ? bT : GrdDetRepa.Columns[5].HeaderText;
                    GrdDetRepa.Columns[7].HeaderText = bO.Equals("C41") ? bT : GrdDetRepa.Columns[7].HeaderText;
                    GrdDetRepa.Columns[9].HeaderText = bO.Equals("C13") ? bT : GrdDetRepa.Columns[9].HeaderText;
                    GrdDetRepa.Columns[10].HeaderText = bO.Equals("C43") ? bT : GrdDetRepa.Columns[10].HeaderText;
                    GrdDetRepa.Columns[11].HeaderText = bO.Equals("C44") ? bT : GrdDetRepa.Columns[11].HeaderText;
                    GrdDetRepa.Columns[12].HeaderText = bO.Equals("C09") ? bT : GrdDetRepa.Columns[12].HeaderText;
                    GrdDetRepa.Columns[13].HeaderText = bO.Equals("C45") ? bT : GrdDetRepa.Columns[13].HeaderText;
                    GrdDetRepa.Columns[14].HeaderText = bO.Equals("C46") ? bT : GrdDetRepa.Columns[14].HeaderText;
                    GrdDetRepa.Columns[15].HeaderText = bO.Equals("C47") ? bT : GrdDetRepa.Columns[15].HeaderText;
                    GrdDetRepa.Columns[16].HeaderText = bO.Equals("C26") ? bT : GrdDetRepa.Columns[16].HeaderText;
                    // ************************************************************ Inventario ************************************************************
                    IbtCerrarInvetr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarInvetr.ToolTip;
                    BtnInventario.Text = bO.Equals("BtnInventario") ? bT : BtnInventario.Text;
                    BtnInventario.ToolTip = bO.Equals("BtnInventarioTT") ? bT : BtnInventario.ToolTip;
                    LblTitInventario.Text = bO.Equals("LblTitInventario") ? bT : LblTitInventario.Text;
                    LblAlmacenInv.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacenInv.Text;
                    LblGrupoInv.Text = bO.Equals("LblGrupoInv") ? bT : LblGrupoInv.Text;
                    if (bO.Equals("RdbSrlzdInv")) { ViewState["SNm"] = bT; RdbSrlzdInv.Text = "&nbsp" + bT; }
                    if (bO.Equals("RdbNoSrlzdInv")) { ViewState["NS"] = bT; RdbNoSrlzdInv.Text = "&nbsp" + bT; }
                    LblFechCorte.Text = bO.Equals("LblFechCorte") ? bT : LblFechCorte.Text;
                    IbtExprtrInvtr.ToolTip = bO.Equals("IbtExprtrInvtr") ? bT : IbtExprtrInvtr.ToolTip;
                    // ************************************************************ Estado Compra ************************************************************
                    LblTitCompraPend.Text = bO.Equals("LblTitCompraPend") ? bT : LblTitCompraPend.Text;
                    LblFechECI.Text = bO.Equals("LblFechI") ? bT : LblFechECI.Text;
                    LblFechECF.Text = bO.Equals("LblFechF") ? bT : LblFechECF.Text;
                    RdbECAll.Text = bO.Equals("RdbRpAll") ? "&nbsp" + bT : RdbECAll.Text;
                    RdbECCot.Text = bO.Equals("C39") ? "&nbsp" + bT : RdbECCot.Text;
                    RdbECComp.Text = bO.Equals("RdbECComp") ? "&nbsp" + bT : RdbECComp.Text;
                    RdbECProv.Text = bO.Equals("C26") ? "&nbsp" + bT : RdbECProv.Text;
                    CkbECPend.Text = bO.Equals("CkbRpPend") ? "&nbsp" + bT : CkbECPend.Text;
                    CkbECPend.ToolTip = bO.Equals("CkbECPend") ? bT : CkbECPend.ToolTip;
                    IbtExpECompPend.ToolTip = bO.Equals("IbtExpECompPend") ? bT : IbtExpECompPend.ToolTip;
                    IbtECBusqueda.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtECBusqueda.ToolTip;
                    IbtCerrarCompPend.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCompPend.ToolTip;
                    GrdDetEstdComp.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDetEstdComp.EmptyDataText;
                    GrdDetEstdComp.Columns[0].HeaderText = bO.Equals("C34") ? bT : GrdDetEstdComp.Columns[0].HeaderText;
                    GrdDetEstdComp.Columns[1].HeaderText = bO.Equals("C38") ? bT : GrdDetEstdComp.Columns[1].HeaderText;
                    GrdDetEstdComp.Columns[2].HeaderText = bO.Equals("C39") ? bT : GrdDetEstdComp.Columns[2].HeaderText;
                    GrdDetEstdComp.Columns[3].HeaderText = bO.Equals("RdbECComp") ? bT : GrdDetEstdComp.Columns[3].HeaderText;
                    GrdDetEstdComp.Columns[4].HeaderText = bO.Equals("FechaCompra") ? bT : GrdDetEstdComp.Columns[4].HeaderText;
                    GrdDetEstdComp.Columns[5].HeaderText = bO.Equals("C07") ? bT : GrdDetEstdComp.Columns[5].HeaderText;
                    GrdDetEstdComp.Columns[6].HeaderText = bO.Equals("AprobadoMstr") ? bT : GrdDetEstdComp.Columns[6].HeaderText;
                    GrdDetEstdComp.Columns[9].HeaderText = bO.Equals("LblIdentifMstr") ? bT : GrdDetEstdComp.Columns[9].HeaderText;
                    GrdDetEstdComp.Columns[10].HeaderText = bO.Equals("C13") ? bT : GrdDetEstdComp.Columns[10].HeaderText;
                    GrdDetEstdComp.Columns[11].HeaderText = bO.Equals("C44") ? bT : GrdDetEstdComp.Columns[11].HeaderText;
                    GrdDetEstdComp.Columns[12].HeaderText = bO.Equals("C43") ? bT : GrdDetEstdComp.Columns[12].HeaderText;
                    GrdDetEstdComp.Columns[13].HeaderText = bO.Equals("CantPend") ? bT : GrdDetEstdComp.Columns[13].HeaderText;
                    GrdDetEstdComp.Columns[14].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDetEstdComp.Columns[14].HeaderText;
                    GrdDetEstdComp.Columns[15].HeaderText = bO.Equals("UndDesp") ? bT : GrdDetEstdComp.Columns[15].HeaderText;
                    GrdDetEstdComp.Columns[16].HeaderText = bO.Equals("C09") ? bT : GrdDetEstdComp.Columns[16].HeaderText;
                    GrdDetEstdComp.Columns[17].HeaderText = bO.Equals("C46") ? bT : GrdDetEstdComp.Columns[17].HeaderText;
                    GrdDetEstdComp.Columns[18].HeaderText = bO.Equals("C45") ? bT : GrdDetEstdComp.Columns[18].HeaderText;
                    GrdDetEstdComp.Columns[19].HeaderText = bO.Equals("PPT") ? bT : GrdDetEstdComp.Columns[19].HeaderText;
                    GrdDetEstdComp.Columns[20].HeaderText = bO.Equals("C47") ? bT : GrdDetEstdComp.Columns[20].HeaderText;
                    GrdDetEstdComp.Columns[21].HeaderText = bO.Equals("C26") ? bT : GrdDetEstdComp.Columns[21].HeaderText;
                    // ************************************************************ Entrada y salida ************************************************************
                    LblTitEyS.Text = bO.Equals("BtnEYS") ? bT : LblTitEyS.Text;
                    BtnEYS.Text = bO.Equals("BtnEYS") ? bT : BtnEYS.Text;
                    BtnEYS.ToolTip = bO.Equals("BtnEYSTT") ? bT : BtnEYS.ToolTip;
                    LblAlmacenEyS.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacenEyS.Text;
                    LblFechIEyS.Text = bO.Equals("LblFechI") ? bT : LblFechIEyS.Text;
                    LblFechFEyS.Text = bO.Equals("LblFechF") ? bT : LblFechFEyS.Text;
                    IbtExpEyS.ToolTip = bO.Equals("BtnExportMstr") ? bT : IbtExpEyS.ToolTip;
                    IbtCerrarEyS.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarEyS.ToolTip;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnOnCl1Invt'");
                foreach (DataRow row in Result)
                { IbtExprtrInvtr.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Informe_Logistica 11,'','','','',0,0,@Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
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
                                DSTDdl.Tables[1].TableName = "Tipo";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacenInv.DataSource = DSTDdl.Tables[0];
                DdlAlmacenInv.DataTextField = "NomAlmacen";
                DdlAlmacenInv.DataValueField = "CodIdAlmacen";
                DdlAlmacenInv.DataBind();
                DdlAlmacenCnc.DataSource = DSTDdl.Tables[0];
                DdlAlmacenCnc.DataTextField = "NomAlmacen";
                DdlAlmacenCnc.DataValueField = "CodIdAlmacen";
                DdlAlmacenCnc.DataBind();
                DataTable DT = DSTDdl.Tables[0].Copy();
                DT.Rows.Add(" - ",0);
                DataView DV = DT.DefaultView;
                DV.Sort = "CodIdAlmacen";
                DT = DV.ToTable();
                DdlAlmacenEyS.DataSource = DT;
                DdlAlmacenEyS.DataTextField = "NomAlmacen";
                DdlAlmacenEyS.DataValueField = "CodIdAlmacen";
                DdlAlmacenEyS.DataBind();
            }
            if (DSTDdl.Tables["Tipo"].Rows.Count > 0)
            {
                DdlGrupoInv.DataSource = DSTDdl.Tables[1];
                DdlGrupoInv.DataTextField = "Tipo";
                DdlGrupoInv.DataValueField = "CodTipoElemento";
                DdlGrupoInv.DataBind();
            }
        }
        // ************************************************************ Reparaciones ************************************************************
        protected void BindRepa()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;
                if (TxtFechI.Text.Equals("") || TxtFechF.Text.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MensCampoReq'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                    if (TxtFechF.Text.Equals("")) { TxtFechI.Focus(); }
                    if (TxtFechI.Text.Equals("")) { TxtFechI.Focus(); }
                    return;
                }
                Cnx.ValidarFechas(TxtFechI.Text.Trim(), TxtFechF.Text.Trim(), 2);
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
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTipoDoc = "";
                    string S_Pend = "";

                    if (RdbRpAll.Checked == true) { VbTipoDoc = ""; }
                    if (RdbRpCot.Checked == true) { VbTipoDoc = "COT"; }
                    if (RdbRpCodRepa.Checked == true) { VbTipoDoc = "REP"; }
                    if (RdbRpPN.Checked == true) { VbTipoDoc = "PN"; }
                    if (RdbRpSN.Checked == true) { VbTipoDoc = "SN"; }
                    if (RdbRpProv.Checked == true) { VbTipoDoc = "PROV"; }
                    if (CkbRpPend.Checked == true) { S_Pend = "PEND"; }
                    CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                    CursorIdioma.Alimentar("CurExportLogstcRepa", Session["77IDM"].ToString().Trim());
                    string VbTxtSql = "EXEC PNTLL_Reparacion 10, @TD, @Doc, @Pend, @CID,'','',0,0,0,@Idm, @ICC, @FI, @FF,'03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@TD", VbTipoDoc);
                        SC.Parameters.AddWithValue("@Doc", TxtRpDocBusq.Text.Trim());
                        SC.Parameters.AddWithValue("@Pend", S_Pend);
                        SC.Parameters.AddWithValue("@CID", "CurExportLogstcRepa");
                        SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechI.Text.Trim()));
                        SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechF.Text.Trim()));
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSRepara = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSRepara);
                                DSRepara.Tables[0].TableName = "Consulta";
                                DSRepara.Tables[1].TableName = "Exportar";
                                ViewState["DSRepara"] = DSRepara;
                            }
                        }
                    }
                }
                DSRepara = (DataSet)ViewState["DSRepara"];
                if (DSRepara.Tables["Consulta"].Rows.Count > 0) { GrdDetRepa.DataSource = DSRepara.Tables["Consulta"]; }
                else
                { GrdDetRepa.DataSource = null; }
                GrdDetRepa.DataBind();
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void BtnReparaciones_Click(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 1; TxtFechI.Focus(); }
        protected void IbtRpBusqueda_Click(object sender, ImageClickEventArgs e)
        { BindRepa(); }
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; }
        protected void IbtExcelHisAlmaRepa_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechI.Text.Equals("") || TxtFechF.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MensCampoReq'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                if (TxtFechF.Text.Equals("")) { TxtFechI.Focus(); }
                if (TxtFechI.Text.Equals("")) { TxtFechI.Focus(); }
                return;
            }
            Cnx.ValidarFechas(TxtFechI.Text.Trim(), TxtFechF.Text.Trim(), 2);
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

            string VbNomArchivo = "";
            Result = Idioma.Select("Objeto= 'NomArcRepa'");
            foreach (DataRow row in Result)
            { VbNomArchivo = row["Texto"].ToString().Trim(); }

            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurExportLogstcRepa", Session["77IDM"].ToString().Trim());

            string Query = "EXEC SP_Mvto_Entrada_Repa @FI, @FF, '', @NA, @Idm, @ICC";

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechI.Text.Trim()));
                    cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechF.Text.Trim()));
                    cmd.Parameters.AddWithValue("@NA", "CurExportLogstcRepa");
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
        protected void IbtExpRepaPend_Click(object sender, ImageClickEventArgs e)
        {
            DataRow[] Result;
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSRepara = (DataSet)ViewState["DSRepara"];
                if (DSRepara == null)
                {
                    Result = Idioma.Select("Objeto= 'Mens02RptLog'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                    return;
                }
                /*if (DSRepara.Tables["Exportar"].Rows.Count > 0)
                {
                    Result = Idioma.Select("Objeto= 'Mens02RptLog'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                    return;
                }*/
                string VbNomArchivo = "";
                Result = Idioma.Select("Objeto= 'NomArcRepaEstado'");
                foreach (DataRow row in Result)
                { VbNomArchivo = row["Texto"].ToString().Trim(); }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(DSRepara.Tables["Exportar"]);
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
            catch (Exception)
            {
                Result = Idioma.Select("Objeto= 'Mens02RptLog'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        // ************************************************************ Inventario ************************************************************
        protected void BtnInventario_Click(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 2; TxtFechCorte.Focus(); }
        protected void IbtCerrarInvetr_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; }
        protected void IbtExprtrInvtr_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechCorte.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens01RptLog'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                TxtFechCorte.Focus(); return;
            }
            string VbMnsj = Cnx.ValidarFechas2(TxtFechCorte.Text.Trim(), "", 1);
            if (!VbMnsj.ToString().Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { VbMnsj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFechCorte.Focus();
                return;
            }

            string VbNomArchivo = BtnInventario.Text.Trim() + "_" + DdlAlmacenInv.SelectedItem.Text.Trim() + "_" + DdlGrupoInv.SelectedItem.Text.Trim();
            string Query, VbNomDT;

            switch (DdlGrupoInv.Text.Trim())
            {
                case "01": //Mat
                    if (RdbNoSrlzdInv.Checked == true)
                    {
                        VbNomArchivo = VbNomArchivo + "_" + ViewState["NS"];
                        string borr = ViewState["NS"].ToString().Trim().Substring(0, 6);
                        VbNomDT = DdlAlmacenInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + DdlGrupoInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + ViewState["NS"].ToString().Trim().Substring(0, 6);
                    }
                    else
                    {
                        VbNomArchivo = VbNomArchivo + "_" + ViewState["SNm"];
                        VbNomDT = DdlAlmacenInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + DdlGrupoInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + ViewState["SNm"].ToString().Trim().Substring(0, 6);
                    }

                    break;
                case "02"://Comp
                    VbNomDT = DdlAlmacenInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + DdlGrupoInv.SelectedItem.Text.Trim().Substring(0, 3);
                    break;
                default:// Hta
                    VbNomDT = DdlAlmacenInv.SelectedItem.Text.Trim().Substring(0, 3) + "_" + DdlGrupoInv.SelectedItem.Text.Trim().Substring(0, 3);
                    break;
            }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurExportInventarioNoSrlzd", Session["77IDM"].ToString().Trim());
            if (RdbNoSrlzdInv.Checked == true) { Query = "EXEC Sp_Inventarios @FC,@IdAlm, @Grp,'NO','01', @ICC,'CurExportInventarioNoSrlzd'"; }
            else { Query = "EXEC SP_Inventario_Componentes @FC, @Grp,'NO', @IdAlm,'01', @ICC,'CurExportInventarioNoSrlzd'"; }

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.AddWithValue("@FC", Convert.ToDateTime(TxtFechCorte.Text.Trim()));
                    cmd.Parameters.AddWithValue("@IdAlm", DdlAlmacenInv.Text.Trim());
                    cmd.Parameters.AddWithValue("@Grp", DdlGrupoInv.Text.Trim());
                    cmd.Parameters.AddWithValue("@NA", "CurExportLogstcRepa");
                    cmd.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    cmd.Parameters.AddWithValue("@U", Session["C77U"]);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            ds.Tables[0].TableName = VbNomDT;
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable DT in ds.Tables) { wb.Worksheets.Add(DT); }
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
        protected void DdlGrupoInv_TextChanged(object sender, EventArgs e)
        {
            if (DdlGrupoInv.Text.Trim().Equals("01")) { RdbNoSrlzdInv.Enabled = true; RdbSrlzdInv.Checked = true; RdbSrlzdInv.Enabled = true; }
            else { RdbNoSrlzdInv.Enabled = false; RdbSrlzdInv.Enabled = false; RdbSrlzdInv.Checked = false; RdbNoSrlzdInv.Checked = false; }
        }
        // ************************************************************ Estado de la Compra ************************************************************
        protected void BtnCompraPend_Click(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 3; TxtFechECI.Focus(); }
        protected void IbtCerrarCompPend_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; }
        protected void IbtECBusqueda_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;
                if (TxtFechECI.Text.Equals("") || TxtFechECF.Text.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MensCampoReq'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                    if (TxtFechECF.Text.Equals("")) { TxtFechECI.Focus(); }
                    if (TxtFechECI.Text.Equals("")) { TxtFechECI.Focus(); }
                    return;
                }
                Cnx.ValidarFechas(TxtFechECI.Text.Trim(), TxtFechECF.Text.Trim(), 2);
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
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTipoDoc = "", S_Pend = "";
                    if (RdbECAll.Checked == true) { VbTipoDoc = ""; }
                    if (RdbECCot.Checked == true) { VbTipoDoc = "COT"; }
                    if (RdbECComp.Checked == true) { VbTipoDoc = "COM"; }
                    if (RdbECPN.Checked == true) { VbTipoDoc = "PN"; }
                    if (RdbECProv.Checked == true) { VbTipoDoc = "PROV"; }
                    if (CkbECPend.Checked == true) { S_Pend = "PEND"; }
                    CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                    CursorIdioma.Alimentar("CURINFTODASLASSPOC", Session["77IDM"].ToString().Trim());
                    string VbTxtSql = "EXEC PNTLL_Reparacion 11, @TD, @Doc, @Pend, @CID,'','',0,0,0,@Idm, @ICC, @FI, @FF,'03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@TD", VbTipoDoc);
                        SC.Parameters.AddWithValue("@Doc", TxtECDocBusq.Text.Trim());
                        SC.Parameters.AddWithValue("@Pend", S_Pend);
                        SC.Parameters.AddWithValue("@CID", "CURINFTODASLASSPOC");
                        SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechECI.Text.Trim()));
                        SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechECF.Text.Trim()));
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSEstdComp = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSEstdComp);
                                DSEstdComp.Tables[0].TableName = "Consulta";
                                DSEstdComp.Tables[1].TableName = "Exportar";
                                ViewState["DSEstdComp"] = DSEstdComp;
                            }
                        }
                    }
                }
                DSEstdComp = (DataSet)ViewState["DSEstdComp"];
                if (DSEstdComp.Tables["Consulta"].Rows.Count > 0) { GrdDetEstdComp.DataSource = DSEstdComp.Tables["Consulta"]; }
                else
                { GrdDetEstdComp.DataSource = null; }
                GrdDetEstdComp.DataBind();
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void IbtExpECompPend_Click(object sender, ImageClickEventArgs e)
        {
            DataRow[] Result;
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSEstdComp = (DataSet)ViewState["DSEstdComp"];
                if (DSEstdComp == null)
                {
                    Result = Idioma.Select("Objeto= 'Mens03RptLog'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe realizar primero una consulta del estado de las compras.
                    return;
                }
                string VbNomArchivo = "";
                Result = Idioma.Select("Objeto= 'NomArcEstadoComp'");
                foreach (DataRow row in Result)
                { VbNomArchivo = row["Texto"].ToString().Trim(); }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(DSEstdComp.Tables["Exportar"]);
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
            catch (Exception)
            {
                Result = Idioma.Select("Objeto= 'Mens02RptLog'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        // ************************************************************ Conciliacion ************************************************************
        protected void BtnConciliacion_Click(object sender, EventArgs e)
        {
            MlVw.ActiveViewIndex = 4; TxtFechCorte.Focus();
        }
        protected void IbtCerrarConcil_Click(object sender, ImageClickEventArgs e)
        {
            MlVw.ActiveViewIndex = 0;
        }
        protected void IbtEjecutar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DataRow[] Result;
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtFechCorteCnc.Text.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'Mens01RptLog'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                    TxtFechCorteCnc.Focus(); return;
                }
                string VbMnsj = Cnx.ValidarFechas2(TxtFechCorteCnc.Text.Trim(), "", 1);
                if (!VbMnsj.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { VbMnsj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechCorteCnc.Focus();
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_Conciliacion @FC,@IdAlm,'01',@ICC,@IDM";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@FC", Convert.ToDateTime(TxtFechCorteCnc.Text.Trim()));
                        SC.Parameters.AddWithValue("@IdAlm", DdlAlmacenCnc.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@IDM", Session["77IDM"]);
                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTCncl);
                        ViewState["DTCncl"] = DTCncl;
                    }
                    DTCncl = (DataTable)ViewState["DTCncl"];
                    string S_MesT = DTCncl.Rows[0]["Asignacion"].ToString().Trim();
                    string S_MultLen = "", S_Entr = "", S_Sal = "", S_InvFinal="";
                    Result = Idioma.Select("Objeto= '" + S_MesT + "'");
                    foreach (DataRow row in Result)
                    { S_MultLen = row["Texto"].ToString(); }
                    Result = Idioma.Select("Objeto= 'Entr'");
                    foreach (DataRow row in Result)
                    { S_Entr = row["Texto"].ToString(); }
                    Result = Idioma.Select("Objeto= 'Slda'");
                    foreach (DataRow row in Result)
                    { S_Sal = row["Texto"].ToString(); }
                    Result = Idioma.Select("Objeto= 'InvFinal'");
                    foreach (DataRow row in Result)
                    { S_InvFinal = row["Texto"].ToString(); }
                    foreach (DataRow row in DTCncl.Rows)
                    {
                        if (row["Asignacion"].ToString().Equals(S_MesT))
                        {
                            row["Asignacion"] = S_MultLen;
                        }
                        if (row["Asignacion"].ToString().Equals("ENTRADA"))
                        {
                            row["Asignacion"] = S_Entr;
                        }
                        if (row["Asignacion"].ToString().Equals("SALIDA"))
                        {
                            row["Asignacion"] = S_Sal;
                        }
                        if (row["Asignacion"].ToString().Equals("IF"))
                        {
                            row["Asignacion"] = S_InvFinal;
                        }
                    }
                    GrdConciliacion.DataSource = DTCncl;
                    GrdConciliacion.DataBind();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Conciliacion Ejecutar", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        // ************************************************************ Entradas y Salidas ************************************************************
        protected void BtnEYS_Click(object sender, EventArgs e)
        {
            MlVw.ActiveViewIndex = 5; TxtFechCorte.Focus();
        }
        protected void IbtCerrarEyS_Click(object sender, ImageClickEventArgs e)
        {
            MlVw.ActiveViewIndex = 0;
        }
        protected void IbtExpEyS_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechIEyS.Text.Equals("") || TxtFechFEyS.Text.Equals(""))
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
            }

            string VbNomArchivo = "";
            Result = Idioma.Select("Objeto= 'NomArcEyS'");
            foreach (DataRow row in Result)
            { VbNomArchivo = row["Texto"].ToString().Trim(); }

            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurExptrEyS", Session["77IDM"].ToString().Trim());

            string Query = " EXEC SP_Mvto_Entrada_Salida @FI, @FF,'',@Alm,'01', @ICC,  @Idm, @NA";

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIEyS.Text.Trim()));
                    cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFEyS.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Alm",DdlAlmacenEyS.Text.Trim());
                    cmd.Parameters.AddWithValue("@NA", "CurExptrEyS");
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