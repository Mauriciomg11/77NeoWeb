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
                    BtnReparaciones.Text = bO.Equals("BtnReparaciones") ? bT : BtnReparaciones.Text;
                    LblTitReparaciones.Text = bO.Equals("BtnReparaciones") ? bT : LblTitReparaciones.Text;
                    BtnReparaciones.ToolTip = bO.Equals("BtnReparacionesTT") ? bT : BtnReparaciones.ToolTip;
                    IbtCerrarImpr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpr.ToolTip;
                    LblTitReparaciones.Text = bO.Equals("BtnReparaciones") ? bT : LblTitReparaciones.Text;
                    LblFechI.Text = bO.Equals("LblFechI") ? bT : LblFechI.Text;
                    LblFechF.Text = bO.Equals("LblFechF") ? bT : LblFechF.Text;
                    IbtExcelRepa.ToolTip = bO.Equals("BtnExportMstr") ? bT : IbtExcelRepa.ToolTip;
                    // ************************************************************ Inventario ************************************************************
                    IbtCerrarInvetr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarInvetr.ToolTip;
                    BtnInventario.Text = bO.Equals("BtnInventario") ? bT : BtnInventario.Text;
                    BtnInventario.ToolTip = bO.Equals("BtnInventarioTT") ? bT : BtnInventario.ToolTip;
                    LblTitInventario.Text = bO.Equals("BtnReparaciones") ? bT : LblTitInventario.Text;
                    LblAlmacenInv.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacenInv.Text;
                    LblGrupoInv.Text = bO.Equals("LblGrupoInv") ? bT : LblGrupoInv.Text;
                    if (bO.Equals("RdbSrlzdInv")) { ViewState["SNm"] = bT; RdbSrlzdInv.Text = "&nbsp" + bT; }
                    if (bO.Equals("RdbNoSrlzdInv")) { ViewState["NS"] = bT; RdbNoSrlzdInv.Text = "&nbsp" + bT; }
                    LblFechCorte.Text = bO.Equals("LblFechCorte") ? bT : LblFechCorte.Text;
                    IbtExprtrInvtr.ToolTip = bO.Equals("IbtExprtrInvtr") ? bT : IbtExprtrInvtr.ToolTip;

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
        protected void BtnReparaciones_Click(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 1; TxtFechI.Focus(); }
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; }
        protected void IbtExcelRepa_Click(object sender, ImageClickEventArgs e)
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
                            ds.Tables[0].TableName = "Historic";
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
        // ************************************************************ xxx ************************************************************
    }
}