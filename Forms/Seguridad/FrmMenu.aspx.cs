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

namespace _77NeoWeb.Forms
{
    public partial class FrmMenu : System.Web.UI.Page
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
        }
        void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMenu.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("WebMenuInicio.aspx");
            }
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
        }
        void BindData(string VbDesmenu)
        {
            DataTable dtbl = new DataTable();
            Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_ConfiguracionV2_ 3,'" + VbDesmenu + "','" + Session["C77U"].ToString() + "','','',''," + ViewState["VblIngMS"].ToString() + ",0,0,0,'01-01-1','02-01-1','03-01-1'";
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
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
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
                //ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);

                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmMenu", "Update", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                // ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);

                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmMenu", "DELETE", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string VbOpenForm = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][1].ToString();
            if (VbOpenForm != String.Empty && !VbOpenForm.Equals("#"))
            {
                Response.Redirect(VbOpenForm);
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                if (img != null)
                {
                    e.Row.Cells[10].Controls.Remove(img);
                }
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[10].Controls.Remove(imgD);
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int sangr = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Sangria").ToString());

                if (sangr == 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null)
                    {
                        TxtDescP.BackColor = System.Drawing.Color.LightGray;
                    }
                }
                if (sangr == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.DarkOrange;
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null)
                    {
                        TxtDescP.BackColor = System.Drawing.Color.DarkOrange;
                    }
                }
                if (sangr == 2)
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    TextBox TxtDescP = e.Row.FindControl("TxtIdDescrP") as TextBox;
                    if (TxtDescP != null)
                    {
                        TxtDescP.BackColor = System.Drawing.Color.Yellow;
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

            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData(TxtBusqueda.Text);
        }
    }
}