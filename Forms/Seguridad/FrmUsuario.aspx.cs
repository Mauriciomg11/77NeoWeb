using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using System.Web.ModelBinding;

namespace _77NeoWeb.Forms
{
    public partial class FrmUsuario : System.Web.UI.Page
    {       
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
               // Session["C77U"] = "00000082";
            }
            if (!IsPostBack)
            {
                ModSeguridad();

                BindData(TxtBusqueda.Text);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmUsuario.aspx");

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
        }
        void BindData(string VbDesUsu)
        {
            Cnx.SelecBD();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_ConfiguracionV2_ 6,'" + VbDesUsu + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter(VbTxtSql, sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                GrdDatos.Rows[0].Cells[0].Text = "No existen registros ..!";
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BindData(TxtBusqueda.Text);
        }
        protected void DdlUsuPP_TextChanged(object sender, EventArgs e)
        {
            string VbCodUsu;
            VbCodUsu = (GrdDatos.FooterRow.FindControl("DdlUsuPP") as DropDownList).SelectedValue.Trim();
            if (VbCodUsu.ToString() != string.Empty)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConx = new SqlConnection(Cnx.GetConex()))
                {

                    string LtxtSql = " EXEC SP_ConfiguracionV2_ 9,'" + VbCodUsu + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(LtxtSql, sqlConx);
                    sqlConx.Open();
                    SqlDataReader tbl = Comando.ExecuteReader();
                    if (tbl.Read())
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
        }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string VblCodusu = (GrdDatos.FooterRow.FindControl("DdlUsuPP") as DropDownList).SelectedValue.Trim();
                        if (VblCodusu == String.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un usuario')", true);
                            return;
                        }

                        CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                        int VbAdmin = 0;
                        if (chkbox.Checked == true)
                        {
                            VbAdmin = 1;
                        }
                        sqlCon.Open();
                        string query = "INSERT INTO TblUsuario(CodUsuario,Identificacion ,Nombre , Apellido, Usuario,PassWeb,Activo,Clave,UsuCrea,UsuMod) " +
                            "VALUES(@CodUsu,@Ident, @Nom,  @Apell, @Login, @Pass, @Act,'',@VbC77U,'')";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@CodUsu", VblCodusu);
                        sqlCmd.Parameters.AddWithValue("@Ident", (GrdDatos.FooterRow.FindControl("TxtIdenPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Nom", Session["VbNombFrmUsuario"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Apell", Session["VbApellFrmUsuario"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Login", (GrdDatos.FooterRow.FindControl("TxtUsuPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Pass", (GrdDatos.FooterRow.FindControl("TxtPassWPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Act", VbAdmin);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindData(TxtBusqueda.Text);
                    }
                }
                if (e.CommandName == "Select")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmUsuario", "INSERT", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdDatos.EditIndex = e.NewEditIndex;
            BindData(TxtBusqueda.Text);
        }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
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
                    string query = "UPDATE TblUsuario SET Usuario = @Login, PassWeb = @Pass, Activo = @Act, UsuMod = @VbC77U, FechaMod=GetDate() WHERE CodUsuario = @CodUsu";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Login", (GrdDatos.Rows[e.RowIndex].FindControl("TxtUsu") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Pass", (GrdDatos.Rows[e.RowIndex].FindControl("TxtPassW") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Act", VbAdmin);
                    sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                    sqlCmd.Parameters.AddWithValue("@CodUsu", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);

                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmUsuario", "Update", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdDatos.EditIndex = -1;
            BindData(TxtBusqueda.Text);
        }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "EXEC SP_ConfiguracionV2_ 7,@id,'','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    sqlCon.Close();
                    BindData(TxtBusqueda.Text);
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
            string LtxtSql = "EXEC SP_ConfiguracionV2_ 8,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";             
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlUsuPP = (e.Row.FindControl("DdlUsuPP") as DropDownList);
                DdlUsuPP.DataSource = Cnx.DSET(LtxtSql);
                DdlUsuPP.DataTextField = "Usuario";
                DdlUsuPP.DataValueField = "CodPersona";
                DdlUsuPP.DataBind();
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
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData(TxtBusqueda.Text);
        }
    }
}