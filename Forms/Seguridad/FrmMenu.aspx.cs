using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Seguridad
{
    public partial class FrmMenu : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
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
                ModSeguridad();
                BindData(TxtBusqueda.Text);
                if (Session["C77U"].ToString().Trim().Equals("00000082") || (Session["C77U"].ToString().Trim().Equals("00000083") && Session["!dC!@"].ToString().Trim().Equals("1"))) { IbtAbrirIdioma.Visible = true; }
            }
        }
        void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMenu.aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
            }
            if (ClsP.GetConsultar() == 0)
            {
            }
            if (ClsP.GetImprimir() == 0)
            {

            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
            }
            if (ClsP.GetCE1() == 0)
            {
            }
            if (ClsP.GetCE2() == 0)
            {
            }
            if (ClsP.GetCE3() == 0)
            {
            }
            if (ClsP.GetCE4() == 0)
            {
            }
            if (ClsP.GetCE5() == 0)
            {
            }
            if (ClsP.GetCE6() == 0)
            {
            }
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
                    LblBusqueda.Text = bO.Equals("LblBusqueda") ? bT + ":" : LblBusqueda.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("IbtConsultar") ? bT : IbtConsultar.ToolTip;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdIR") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdPos") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdDesc") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdPosSup") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdPosPpl") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdNvl") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdRuta") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("GrdNom") ? bT : GrdDatos.Columns[8].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        void BindData(string VbDesmenu)
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_ConfiguracionV2_ 3, @Dsc, @Us,'','','', @Ing,0, @Idm, @ICC,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    string borr = ViewState["VblIngMS"].ToString();
                    SC.Parameters.AddWithValue("@Dsc", VbDesmenu);
                    SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                    SC.Parameters.AddWithValue("@Ing", ViewState["VblIngMS"]);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                    if (dtbl.Rows.Count > 0)
                    { GrdDatos.DataSource = dtbl; GrdDatos.DataBind(); }
                    else
                    {
                        dtbl.Rows.Add(dtbl.NewRow());
                        GrdDatos.DataSource = dtbl;
                        GrdDatos.DataBind();
                        GrdDatos.Rows[0].Cells.Clear();
                        GrdDatos.Rows[0].Cells.Add(new TableCell());
                        GrdDatos.Rows[0].Cells[0].Text = "No existen registros ..!";
                        GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null) { Row.Cells[10].Controls.Remove(imgE); }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null) { Row.Cells[10].Controls.Remove(imgD); }
                }
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text); PerfilesGrid(); }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.CommandName.Equals("AddNew"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string VbPosiciones;
                        VbPosiciones = (GrdDatos.FooterRow.FindControl("TxtPosPP") as TextBox).Text.Trim() + (GrdDatos.FooterRow.FindControl("TxtPosSupPP") as TextBox).Text.Trim() +
                             (GrdDatos.FooterRow.FindControl("TxtPosMasterPP") as TextBox).Text.Trim();
                        string query = "EXEC SP_ConfiguracionV2_ 4,@Posiciones,@Descr,@Ruta,@NomForm,'',@Nivel,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Posiciones", VbPosiciones);
                        sqlCmd.Parameters.AddWithValue("@Descr", (GrdDatos.FooterRow.FindControl("TxtIdDescrPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Nivel", (GrdDatos.FooterRow.FindControl("TxtNivelPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Ruta", (GrdDatos.FooterRow.FindControl("TxtRutaPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@NomForm", (GrdDatos.FooterRow.FindControl("TxtNomFormPP") as TextBox).Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        BindData(TxtBusqueda.Text);
                    }
                }
                if (e.CommandName.Equals("Abrir"))
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    string VbIdx = GrdDatos.DataKeys[gvr.RowIndex].Values["RutaFormulario"].ToString();
                    if (VbIdx != String.Empty && !VbIdx.Equals("#")) { Response.Redirect(VbIdx); }
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmMenu", "INSERT", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdDatos.EditIndex = e.NewEditIndex;
            BindData(TxtBusqueda.Text);
            TextBox TxtDesc = GrdDatos.Rows[e.NewEditIndex].FindControl("TxtIdDescr") as TextBox;
        }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string vble;
                vble = (GrdDatos.Rows[e.RowIndex].FindControl("TxtPos") as TextBox).Text.Trim();
                if (vble == String.Empty)
                {
                    //ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una posición')", true);
                    return;
                }
                string VbPosisicones = (GrdDatos.Rows[e.RowIndex].FindControl("TxtPos") as TextBox).Text.Trim() + (GrdDatos.Rows[e.RowIndex].FindControl("TxtPosSup") as TextBox).Text.Trim() +
                    (GrdDatos.Rows[e.RowIndex].FindControl("TxtPosMaster") as TextBox).Text.Trim();

                Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "EXEC SP_ConfiguracionV2_ 5,@Posiciones,@Descr,@Ruta,@NomForm,'',@Nivel,@id,0,0,'01-01-01','02-01-01','03-01-01'";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Posiciones", VbPosisicones);
                    sqlCmd.Parameters.AddWithValue("@Descr", (GrdDatos.Rows[e.RowIndex].FindControl("TxtIdDescr") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Nivel", (GrdDatos.Rows[e.RowIndex].FindControl("TxtNivel") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Ruta", (GrdDatos.Rows[e.RowIndex].FindControl("TxtRuta") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@NomForm", (GrdDatos.Rows[e.RowIndex].FindControl("TxtNomForm") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmMenu", "Update", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData(TxtBusqueda.Text); }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM TblUsrFormulario WHERE CodIdFormulario=@id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    sqlCon.Close();
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmMenu", "DELETE", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string VbOpenForm = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][1].ToString();
            if (VbOpenForm != String.Empty && !VbOpenForm.Equals("#")) { Response.Redirect(VbOpenForm); }
            PerfilesGrid();
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((int)ViewState["VblIngMS"] == 0)
            {
                ImageButton imgI = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (imgI != null)
                {
                    e.Row.Cells[10].Controls.Remove(imgI);
                    GrdDatos.Columns[2].Visible = false;
                    GrdDatos.Columns[4].Visible = false;
                    GrdDatos.Columns[5].Visible = false;
                    GrdDatos.Columns[6].Visible = false;
                    GrdDatos.Columns[7].Visible = false;
                    GrdDatos.Columns[8].Visible = false;
                }

                GrdDatos.ShowFooter = false;
            }
            if ((int)ViewState["VblModMS"] == 0)
            {
                ImageButton img = e.Row.FindControl("IbtEdit") as ImageButton;
                if (img != null) { e.Row.Cells[10].Controls.Remove(img); }
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null) { e.Row.Cells[10].Controls.Remove(imgD); }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                if (IbtAddNew != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                if (IbtUpdate != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                    foreach (DataRow row in Result)
                    { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                if (IbtCancel != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtCancel'");
                    foreach (DataRow row in Result)
                    { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int sangr = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Sangria").ToString());

                if (sangr == 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.LightGray);
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null) { TxtDescP.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.LightGray); }
                }
                if (sangr == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Wheat);
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null) { TxtDescP.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Wheat); }
                }
                if (sangr == 2)
                {
                    e.Row.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.LemonChiffon);
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null)
                    {
                        TxtDescP.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.LemonChiffon);
                    }
                }
                if (sangr == 3)
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
                if (sangr == 4)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null)
                    {
                        TxtDescP.BackColor = System.Drawing.Color.LightBlue;
                    }
                }
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

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

                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtAbrir'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }


                }
                Label LblNomForm = e.Row.FindControl("LblNomForm") as Label;
                if (LblNomForm != null)
                {
                    if (LblNomForm.Text.Trim().Equals("")) { IbtAbrir.Visible = false; }
                }
            }
        }
        protected void IbtAbrir_Click(object sender, ImageClickEventArgs e)
        { }

        protected void IbtAbrirIdioma_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string CT = "window.open('/Forms/Seguridad/FrmIdioma.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), CT, true);
        }
    }
}