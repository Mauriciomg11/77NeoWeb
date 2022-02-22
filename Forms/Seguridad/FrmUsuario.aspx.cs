using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms
{
    public partial class FrmUsuario : System.Web.UI.Page
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
            Page.Title = string.Format("Usuarios");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
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
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), "FrmUsuario.aspx", VbPC);

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("WebMenuInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                GrdDatos.ShowFooter = false;
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
                    LblBusqueda.Text = bO.Equals("Busqueda") ? bT : LblBusqueda.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtConsultar.ToolTip;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdCod") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdIdent") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdNomUs") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdFecUlAc") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdUsu") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdClave") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdAct") ? bT : GrdDatos.Columns[6].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindData(string VbDesUsu, string Accion)
        {
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_ConfiguracionV2_ 6,'','','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Datos";
                                DSTDdl.Tables[1].TableName = "UsuParaAsingar";
                                DSTDdl.Tables[2].TableName = "DatosPersona";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataTable DT = new DataTable();
            DT = DSTDdl.Tables[0].Clone();
            Result = DSTDdl.Tables[0].Select("Nombres LIKE '%" + VbDesUsu.Trim() + "%'");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "CodUsuario DESC";
                DT = DV.ToTable();
                GrdDatos.DataSource = DT;
                GrdDatos.DataBind();
            }
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
        protected void DdlUsuPP_TextChanged(object sender, EventArgs e)
        {
            string VbCodUsu;
            VbCodUsu = (GrdDatos.FooterRow.FindControl("DdlUsuPP") as DropDownList).SelectedValue.Trim();
            if (VbCodUsu.ToString() != string.Empty)
            {
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataRow[] Result;
                Result = DSTDdl.Tables[2].Select("CodPersona ='" + VbCodUsu.Trim() + "'");
                foreach (DataRow tbl in Result)
                {
                    TextBox TxtUsuGv = GrdDatos.FooterRow.FindControl("TxtNombrePP") as TextBox;
                    TextBox TxtIndGv = GrdDatos.FooterRow.FindControl("TxtIdenPP") as TextBox;
                    if (TxtUsuGv != null && TxtIndGv != null)
                    {
                        TxtUsuGv.Text = tbl["Usuario"].ToString();
                        TxtIndGv.Text = tbl["Cedula"].ToString();
                        Session["VbNombFrmUsuario"] = tbl["Nombre"].ToString();
                        Session["VbApellFrmUsuario"] = tbl["Apellido"].ToString();
                    }
                }
            }
        }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.CommandName.Equals("AddNew"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VblCodusu = (GrdDatos.FooterRow.FindControl("DdlUsuPP") as DropDownList).SelectedValue.Trim();
                    if (VblCodusu == String.Empty)
                    {
                        Result = Idioma.Select("Objeto= 'Mens02Usu'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un usuario.
                        return;
                    }
                    CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                    int VbAdmin = 0;
                    if (chkbox.Checked == true)
                    {
                        VbAdmin = 1;
                    }
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 18,@CodUsu, @Nom, @Apell, @Login, @Pass,@VbC77U, @Ident,'','INSERT',0,@Act,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string Mensj = "";
                                sqlCmd.Parameters.AddWithValue("@CodUsu", VblCodusu);
                                sqlCmd.Parameters.AddWithValue("@Ident", (GrdDatos.FooterRow.FindControl("TxtIdenPP") as TextBox).Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Nom", Session["VbNombFrmUsuario"].ToString());
                                sqlCmd.Parameters.AddWithValue("@Apell", Session["VbApellFrmUsuario"].ToString());
                                sqlCmd.Parameters.AddWithValue("@Login", (GrdDatos.FooterRow.FindControl("TxtUsuPP") as TextBox).Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Pass", (GrdDatos.FooterRow.FindControl("TxtPassWPP") as TextBox).Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Act", VbAdmin);
                                sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                                SqlDataReader SDR = sqlCmd.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
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
                                Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//rror en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmUsuario", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatos.EditIndex = e.NewEditIndex; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                CheckBox chkbox = GrdDatos.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox;
                int VbAdmin = 0;
                if (chkbox.Checked == true)
                {
                    VbAdmin = 1;
                }
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 18,@CodUsu, '', '', @Login, @Pass,@VbC77U, '','','UPDATE',0,@Act,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            string Mensj = "";
                            string borrar = GrdDatos.DataKeys[e.RowIndex].Value.ToString();
                            sqlCmd.Parameters.AddWithValue("@Login", (GrdDatos.Rows[e.RowIndex].FindControl("TxtUsu") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Pass", (GrdDatos.Rows[e.RowIndex].FindControl("TxtPassW") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Act", VbAdmin);
                            sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@CodUsu", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                            }
                            SDR.Close();

                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
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
                        catch (Exception)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de edición')", true);
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "EXEC SP_ConfiguracionV2_ 7,@id,'','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"].ToString());
                    sqlCmd.ExecuteNonQuery();
                    sqlCon.Close();
                    BindData(TxtBusqueda.Text, "UPD");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);

                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmUsuario", "DELETE", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlUsuPP = (e.Row.FindControl("DdlUsuPP") as DropDownList);
                DdlUsuPP.DataSource = DSTDdl.Tables[1];
                DdlUsuPP.DataTextField = "Usuario";
                DdlUsuPP.DataValueField = "CodPersona";
                DdlUsuPP.DataBind();

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
            }
            if ((int)ViewState["VblModMS"] == 0)
            {
                ImageButton img = e.Row.FindControl("IbtEdit") as ImageButton;
                e.Row.Cells[7].Controls.Remove(img);
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                e.Row.Cells[7].Controls.Remove(imgD);
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, "SEL"); }
    }
}