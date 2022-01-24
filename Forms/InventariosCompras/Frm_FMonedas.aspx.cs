using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class Frm_FMonedas : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTDet = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "XX";
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
                ModSeguridad();
                BindData(TxtBusqueda.Text, "UPD");
                MultVw.ActiveViewIndex = 0;
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0)
            { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            { ViewState["VblIngMS"] = 0; GrdDatos.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnEditarHistrc.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { }
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
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
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblBusquedaH.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusquedaH.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtConsultar.ToolTip;
                    IbtConsultarH.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtConsultarH.ToolTip;
                    BtnEditarHistrc.Text = bO.Equals("BtnEditarHistrc") ? bT : BtnEditarHistrc.Text;
                    BtnEditarHistrc.ToolTip = bO.Equals("BtnEditarHistrcTT") ? bT : BtnEditarHistrc.ToolTip;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("Caption") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("Descripcion") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdSmbl") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdTRMAct") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdUltFecR") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdTrmNw") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("FechaMstr") ? bT : GrdDatos.Columns[5].HeaderText;
                    LblTitHisto.Text = bO.Equals("LblTitHisto") ? bT : LblTitHisto.Text;
                    IbtCloseHist.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseHist.ToolTip;
                    if (bO.Equals("Titulo"))
                    { TxtBusqMon.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("TxtBAno"))
                    { TxtBusqAno.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("TxtBMes"))
                    { TxtBusqMes.Attributes.Add("placeholder", bT); }
                    GrdDatosH.Columns[0].HeaderText = bO.Equals("GrdTRMAct") ? bT : GrdDatosH.Columns[0].HeaderText;
                    GrdDatosH.Columns[1].HeaderText = bO.Equals("FechaMstr") ? bT : GrdDatosH.Columns[1].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[7].Controls.Remove(imgE);
                    }
                }
            }
        }
        protected void BindData(string VbConsultar, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = " EXEC SP_TablasLogistica 4,'','','','','','','','','SELECT',0,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            DTDet = (DataTable)ViewState["DTDet"];
            DataTable DT = new DataTable();
            DT = DTDet.Clone();
            Result = DTDet.Select("Descripcion LIKE '%" + VbConsultar + "%'");
            foreach (DataRow DR in Result)
            { DT.ImportRow(DR); }
            if (DT.Rows.Count > 0)
            { GrdDatos.DataSource = DT; GrdDatos.DataBind(); }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdDatos.DataSource = DT;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                string VbDesc, VBQuery;
                string VbCod = (GrdDatos.FooterRow.FindControl("TxtMondPP") as TextBox).Text.Trim();
                VbDesc = (GrdDatos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();
                if (VbCod.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens09'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el código.
                    return;
                }
                if (VbDesc == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens06'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                    return;
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasLogistica 4,@Desc,@US, @Cd,@Smb,'','','','','INSERT', 0,0,0,0,0,@ICC,NULL,'02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@Desc", VbDesc);
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Cd", VbCod);
                            SC.Parameters.AddWithValue("@Smb", (GrdDatos.FooterRow.FindControl("TxtSimblPP") as TextBox).Text.Trim());
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
                                BindData(TxtBusqueda.Text, "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatos.EditIndex = e.NewEditIndex; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbDesc = (GrdDatos.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim();
            double VbVlrTrm = Convert.ToDouble((GrdDatos.Rows[e.RowIndex].FindControl("TxtTrmNew") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDatos.Rows[e.RowIndex].FindControl("TxtTrmNew") as TextBox).Text.Trim());
            if (VbDesc == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens06'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                return;
            }

            string VbFecha = (GrdDatos.Rows[e.RowIndex].FindControl("TxtFecha") as TextBox).Text.Trim();
            if (!VbFecha.Equals(""))
            {
                Cnx.ValidarFechas(VbFecha.Trim(), "", 1);
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
            }
            if (!VbFecha.Equals("") && VbVlrTrm <= 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Mond'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la TRM nueva.
                return;
            }
            if (VbFecha.Equals("") && VbVlrTrm > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Mond'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la fecha de la TRM nueva.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasLogistica 4,@Desc,@US, @Cd,@Smb,'','','','','UPDATE',0,@Vlr,0,0,0,@ICC, @FNW,'02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Desc", VbDesc);
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Cd", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                        SC.Parameters.AddWithValue("@Smb", (GrdDatos.Rows[e.RowIndex].FindControl("TxtSimbl") as TextBox).Text.Trim());
                        SC.Parameters.AddWithValue("@Vlr", VbVlrTrm < 0 ? 0 : VbVlrTrm);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        if (VbFecha.Equals(""))
                        { SC.Parameters.AddWithValue("@FNW", VbFecha); }
                        else { SC.Parameters.AddWithValue("@FNW", Convert.ToDateTime(VbFecha)); }
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
                            GrdDatos.EditIndex = -1;
                            BindData(TxtBusqueda.Text, "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
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
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, "SEL"); }
        //*****************************************   Historicos  *****************************
        protected void BtnEditarHistrc_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; }
        protected void BIndDataBusq()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                if (!TxtBusqMon.Text.Trim().Equals("") && !TxtBusqAno.Text.Trim().Equals("") && !TxtBusqMes.Text.Trim().Equals(""))
                {
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand("EXEC SP_Pantalla_Moneda 3, @Mnd, @Yr, @Mn,'','',0,0,0,@ICC,'01-01-1','01-01-1'", sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Mnd", TxtBusqMon.Text.Trim());
                        SC.Parameters.AddWithValue("@Yr", TxtBusqAno.Text.Trim());
                        SC.Parameters.AddWithValue("@Mn", TxtBusqMes.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataAdapter DAB = new SqlDataAdapter(SC);
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdDatosH.DataSource = DtB; GrdDatosH.DataBind(); }
                        else
                        {
                            DtB.Rows.Add(DtB.NewRow());
                            GrdDatosH.DataSource = DtB;
                            GrdDatosH.DataBind();
                            GrdDatosH.Rows[0].Cells.Clear();
                            GrdDatosH.Rows[0].Cells.Add(new TableCell());
                            DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                            foreach (DataRow row in Result)
                            { GrdDatosH.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                            GrdDatosH.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
        }
        protected void IbtCloseHist_Click(object sender, ImageClickEventArgs e)
        {
            TxtBusqMon.Text = ""; TxtBusqAno.Text = ""; TxtBusqMes.Text = "";
            GrdDatosH.DataSource = null; GrdDatosH.DataBind(); MultVw.ActiveViewIndex = 0;
        }
        protected void IbtConsultarH_Click(object sender, ImageClickEventArgs e)
        { BIndDataBusq(); }
        protected void GrdDatosH_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatosH.EditIndex = e.NewEditIndex; BIndDataBusq(); }
        protected void GrdDatosH_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            double VbVlrTrm = Convert.ToDouble((GrdDatosH.Rows[e.RowIndex].FindControl("TxtVrT") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDatosH.Rows[e.RowIndex].FindControl("TxtVrT") as TextBox).Text.Trim());

            if (VbVlrTrm <= 0)
            { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = " EXEC SP_Pantalla_Moneda 4, @Cd, @US,'','','', @Id, @Vlr,0, @ICC, @FNW,'01-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Cd", TxtBusqMon.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Id", GrdDatosH.DataKeys[e.RowIndex].Values["CodIdTasa"].ToString());
                        SC.Parameters.AddWithValue("@Vlr", VbVlrTrm);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@FNW", Convert.ToDateTime(GrdDatosH.DataKeys[e.RowIndex].Values["UltFecMod"].ToString()));
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
                            GrdDatosH.EditIndex = -1;
                            BIndDataBusq();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE Histórico TRM", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }

        protected void GrdDatosH_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatosH.EditIndex = -1; BIndDataBusq(); }
        protected void GrdDatosH_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
    }
}