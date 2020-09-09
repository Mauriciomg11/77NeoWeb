using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmTaller : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            } /**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Talleres");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */

                /*Session["C77U"] = "00000132";// 00000132 00000082
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba"; */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Talleres";
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmTaller.aspx");

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
                IbtExpExcel.Visible = false;
            }
        }
        void BindData(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 12,'" + VbConsultar + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            string VbTitul, VbTxtToolT, TxtCad, TxtPantIni, NomArc;
            VbTitul = "Exportar Talleres";
            VbTxtToolT = "Ingrese el nombre del taller a colsultar";
            TxtPantIni = "~/Forms/Configuracion/FrmTaller.aspx";
            TxtCad = "EXEC SP_Pantalla_Parametros 13,'{0}','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            NomArc = "Taller";
            Response.Redirect("~/Forms/FrmExportar.aspx?TT=" + VbTitul + "&ToolT=" + VbTxtToolT + "&NomArch=" + NomArc + "&TCDN=" + TxtCad + "&PantI=" + TxtPantIni);
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
                            Row.Cells[5].Controls.Remove(imgE);
                        }
                    }
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[5].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblCodPpal, VbPrfj, VBQuery;
                    VblCodPpal = (GrdDatos.FooterRow.FindControl("TxtNomTPP") as TextBox).Text.Trim();
                    VbPrfj = (GrdDatos.FooterRow.FindControl("TxtPfjPP") as TextBox).Text.Trim();
                    if (VblCodPpal == String.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar el nombre')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_Pantalla_Parametros 1,'" + VbPrfj + "','','C','Prefijo','TblTaller',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El prefijo se encuenta asignado')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 12, @Desc, @CC, @Pref, @VbUsu,'','','TblTaller','CodTaller','INSERT', @Act,0,0,0,0,5,'01-01-1','02-01-1','03-01-1'";
                        CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                        int VbActivo = 0;
                        if (chkbox.Checked == true)
                        {
                            VbActivo = 1;
                        }
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);

                        sqlCmd.Parameters.AddWithValue("@Desc", VblCodPpal);
                        sqlCmd.Parameters.AddWithValue("@CC", (GrdDatos.FooterRow.FindControl("DdlCCPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.Parameters.AddWithValue("@Pref", VbPrfj);
                        sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                        sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindData(TxtBusqueda.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if (Row.RowIndex == GrdDatos.SelectedIndex)
                {

                    Row.Style["background-color"] = "#D4DAD3";
                    Row.Attributes["onclick"] = "";
                }
                else
                {
                    if (Row.RowIndex % 2 == 0)
                    {
                        Row.Style["background-color"] = "white";
                    }
                    else
                    {
                        Row.Style["background-color"] = "#cae4ff";
                    }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + Row.RowIndex);
                }
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[5].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[5].Controls.Remove(imgD);
                    }
                }
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
                string VblNombre, VbPrfj, VbPfjAnt, VbQuery;

                VblNombre = (GrdDatos.Rows[e.RowIndex].FindControl("TxtNomT") as TextBox).Text.Trim();
                VbPrfj = (GrdDatos.Rows[e.RowIndex].FindControl("TxtPfj") as TextBox).Text.Trim();
                VbPfjAnt = GrdDatos.DataKeys[e.RowIndex].Values["PfjAnt"].ToString();
                if (VblNombre == String.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar el nombre')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VbQuery = "EXEC SP_Pantalla_Parametros 1,'" + VbPrfj + "','" + VbPfjAnt + "','C','Prefijo','TblTaller',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(VbQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El prefijo se encuenta asignado')", true);
                        return;
                    }
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
                    VbQuery = "EXEC SP_TablasPlantillaM 12, @Desc, @CC, @Pref, @VbUsu, @ID,'','','','UPDATE', @Act,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Desc", VblNombre);
                    sqlCmd.Parameters.AddWithValue("@CC", (GrdDatos.Rows[e.RowIndex].FindControl("DdlCC") as DropDownList).SelectedValue.Trim());
                    sqlCmd.Parameters.AddWithValue("@Pref", VbPrfj);
                    sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                    sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                    sqlCmd.Parameters.AddWithValue("@ID", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                string VBQuery, VbCod;
                VbCod = GrdDatos.DataKeys[e.RowIndex].Value.ToString();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_Pantalla_Parametros 15,'" + VbCod + "','','','','VALIDA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_Pantalla_Parametros 15, @id,'','','','ELIMINA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindData(TxtBusqueda.Text);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);                
            }/* */
            string LtxtSql = "EXEC SP_Pantalla_Parametros 14,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlCCPP = (e.Row.FindControl("DdlCCPP") as DropDownList);
                DdlCCPP.DataSource = Cnx.DSET(LtxtSql);
                DdlCCPP.DataTextField = "Nombre";
                DdlCCPP.DataValueField = "CodCc";
                DdlCCPP.DataBind();
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlCC = (DropDownList)e.Row.FindControl("DdlCC");
                DdlCC.DataTextField = "Nombre";
                DdlCC.DataValueField = "CodCc";
                DdlCC.DataSource = Cnx.DSET(LtxtSql);
                DdlCC.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlCC.SelectedValue = dr["CCostoTa"].ToString();
            }
            if ((int)ViewState["VblModMS"] == 0)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    e.Row.Cells[5].Controls.Remove(imgE);
                }
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[5].Controls.Remove(imgD);
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