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

namespace _77NeoWeb.Forms.Seguridad
{
    public partial class FrmSeguPefiles : System.Web.UI.Page
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmSeguPefiles.aspx");

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
        void BindData(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_ConfiguracionV2_ 10,'" + VbConsultar + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                    Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string VblDescipc = (GrdDatos.FooterRow.FindControl("TxtNombreGrupoPP") as TextBox).Text.Trim();
                        if (VblDescipc == String.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un nombre')", true);
                            return;
                        }
                        sqlCon.Open();
                        string query = "INSERT INTO TblUsrGrupo(NombreGrupo) VALUES(@NombreGrupo)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@NombreGrupo", VblDescipc);
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
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmSeguPefiles", "INSERT", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "UPDATE TblUsrGrupo SET NombreGrupo = @NombreGrupo WHERE CodIdGrupo = @ID";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@NombreGrupo", (GrdDatos.Rows[e.RowIndex].FindControl("TxtNombreGrupo") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);

                ClsConexion ClsUE = new ClsConexion();
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmSeguPefiles", "Update", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                    string query = "EXEC SP_ConfiguracionV2_ 11,'','','','','',@id,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                ClsUE.UpdateError(Session["C77U"].ToString(), "FrmSeguPefiles", "DELETE", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((int)ViewState["VblIngMS"] == 0)
            {
                ImageButton imgI = e.Row.FindControl("IbtAddNew") as ImageButton;
                e.Row.Cells[1].Controls.Remove(imgI);
            }
            if ((int)ViewState["VblModMS"] == 0)
            {
                ImageButton img = e.Row.FindControl("IbtEdit") as ImageButton;
                e.Row.Cells[1].Controls.Remove(img);
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                e.Row.Cells[1].Controls.Remove(imgD);
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData(TxtBusqueda.Text);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetFixedHeightForGridIfRowsAreLess(GrdDatos);
        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 22; // height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 22;
                gv.Height = new Unit(height);
            }
        }
    }
}