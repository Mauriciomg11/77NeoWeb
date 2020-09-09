using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmPosicion : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Posición");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */
                /* Session["C77U"] = "00000082";
                 Session["D[BX"] = "DbNeoDempV2";
                 Session["$VR"] = "77NEO01";
                 Session["V$U@"] = "sa";
                 Session["P@$"] = "admindemp";*/
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Posiciones";
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmPosicion.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
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
        void BindData(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "SELECT IdPosicion,Codigo,Descripcion, Activo,UsuCrea,UsuMod,FechaCrea, FechaMod, Codigo CodAnt  FROM TblPosicion WHERE Descripcion LIKE '%" + VbConsultar + "%'";
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
                foreach (GridViewRow Row in GrdDatos.Rows)
                {

                    if ((int)ViewState["VblModMS"] == 0)
                    {
                        ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                        if (imgE != null)
                        {
                            Row.Cells[3].Controls.Remove(imgE);
                        }
                    }
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[3].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblCodPpal, VBQuery;
                    VblCodPpal = (GrdDatos.FooterRow.FindControl("TxtCodPosPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_Pantalla_Parametros 1,'" + VblCodPpal + "','0','C','Codigo','TblPosicion',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('La posición existe')", true);
                            return;
                        }
                        sqlCon.Close();
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        if (VblCodPpal == String.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una posición')", true);
                            return;
                        }
                        sqlCon.Open();
                        VBQuery = "EXEC SP_Pantalla_Parametros 7,@Cod,@Desc,@VbC77U,'','INSERT',@Act,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                        int VbActivo = 0;
                        if (chkbox.Checked == true)
                        {
                            VbActivo = 1;
                        }
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VblCodPpal);
                        sqlCmd.Parameters.AddWithValue("@Desc", (GrdDatos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindData(TxtBusqueda.Text);
                    }
                }               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPosicion", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                string VblCodPpal, VblCodAnt, VbQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VblCodPpal = (GrdDatos.Rows[e.RowIndex].FindControl("TxtCodPos") as TextBox).Text.Trim();
                    VblCodAnt = GrdDatos.DataKeys[e.RowIndex].Values["CodAnt"].ToString();
                    VbQuery = "EXEC SP_Pantalla_Parametros 1,'" + VblCodPpal + "','" + VblCodAnt + "','C','Codigo','TblPosicion',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(VbQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('La posición existe')", true);
                        return;
                    }
                    sqlCon.Close();
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    CheckBox chkbox = GrdDatos.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox;
                    int VbActivo = 0;
                    if (chkbox.Checked == true)
                    {
                        VbActivo = 1;
                    }
                    VbQuery = "EXEC SP_Pantalla_Parametros 7, @Cod, @Desc, @VbC77U,'','UPDATE',@Act,@ID,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Cod", VblCodPpal);
                    sqlCmd.Parameters.AddWithValue("@Desc", (GrdDatos.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                    sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData(TxtBusqueda.Text);                   
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPosicion", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                    string query = "EXEC SP_Pantalla_Parametros 2,'','','','','',@id,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPosicion", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((int)ViewState["VblModMS"] == 0)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    e.Row.Cells[3].Controls.Remove(imgE);
                }
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[3].Controls.Remove(imgD);
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