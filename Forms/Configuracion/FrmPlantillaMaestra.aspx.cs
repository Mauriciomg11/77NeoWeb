using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmPlantillaMaestra : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Plantilla");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = ""; /**/
               /* Session["C77U"] = "00000132";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp"; */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración Plantilla Maestra";
                Session["ValPM"] = "S";
                Session["ATAPM"] = "";
                Session["CodSubAta"] = "";
                Session["CodUNPM"] = "";
                Session["NumElement"] = "0";
                ModSeguridad();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 6,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                    sqlCon.Open();
                    SqlDataReader Tbl = Cm.ExecuteReader();
                    LstCapitulo.Items.Clear();
                    while (Tbl.Read())
                    {
                        LstCapitulo.Items.Add(Tbl[0].ToString());
                    }

                    LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 2,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    DdlFlota.DataSource = Cnx.DSET(LtxtSql);
                    DdlFlota.DataMember = "Datos";
                    DdlFlota.DataTextField = "Descripcion";
                    DdlFlota.DataValueField = "CodModelo";
                    DdlFlota.DataBind();
                }
                BindData();
                BindDataUN("");
                BindDataPsc("");
                BindDataPN("");
                BindDataPN("");
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmPlantillaMaestra.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                GrdDatos.ShowFooter = false;
                GrdUltNvl.ShowFooter = false;
                GrdPn.ShowFooter = false;
                GrdPosicion.ShowFooter = false;

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
        void BindData()
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Capitulo_PM 7,'" + Session["ATAPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter(VbTxtSql, sqlCon);
                sqlDa.Fill(dtbl);
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
        }
        void BindDataUN(string CodUN)
        {
            DataTable DtblUN = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Capitulo_PM 8,'" + CodUN + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SqlDA.Fill(DtblUN);
                if (DtblUN.Rows.Count > 0)
                {
                    GrdUltNvl.DataSource = DtblUN;
                    GrdUltNvl.DataBind();
                }
                else
                {
                    DtblUN.Rows.Add(DtblUN.NewRow());
                    GrdUltNvl.DataSource = DtblUN;
                    GrdUltNvl.DataBind();
                    GrdUltNvl.Rows[0].Cells.Clear();
                    GrdUltNvl.Rows[0].Cells.Add(new TableCell());
                    GrdUltNvl.Rows[0].Cells[0].ColumnSpan = DtblUN.Columns.Count;
                    GrdUltNvl.Rows[0].Cells[0].Text = "No existen registros ..!";
                    GrdUltNvl.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        void BindDataPsc(string CodUN)
        {
            DataTable DtPsc = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Capitulo_PM 10,'" + CodUN + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SqlDA.Fill(DtPsc);
                if (DtPsc.Rows.Count > 0)
                {
                    GrdPosicion.DataSource = DtPsc;
                    GrdPosicion.DataBind();
                }
                else
                {
                    DtPsc.Rows.Add(DtPsc.NewRow());
                    GrdPosicion.DataSource = DtPsc;
                    GrdPosicion.DataBind();
                    GrdPosicion.Rows[0].Cells.Clear();
                    GrdPosicion.Rows[0].Cells.Add(new TableCell());
                    GrdPosicion.Rows[0].Cells[0].ColumnSpan = DtPsc.Columns.Count;
                    GrdPosicion.Rows[0].Cells[0].Text = "Sin posiciones asignadas ..!";
                    GrdPosicion.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        void BindDataPN(string CodUN)
        {
            DataTable DtPn = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Capitulo_PM 4,'" + CodUN + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SqlDA.Fill(DtPn);
                if (DtPn.Rows.Count > 0)
                {
                    GrdPn.DataSource = DtPn;
                    GrdPn.DataBind();
                }
                else
                {
                    DtPn.Rows.Add(DtPn.NewRow());
                    GrdPn.DataSource = DtPn;
                    GrdPn.DataBind();
                    GrdPn.Rows[0].Cells.Clear();
                    GrdPn.Rows[0].Cells.Add(new TableCell());
                    GrdPn.Rows[0].Cells[0].ColumnSpan = DtPn.Columns.Count;
                    GrdPn.Rows[0].Cells[0].Text = "Sin partes asignados ..!";
                    GrdPn.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        protected void LstCapitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ATAPM"] = LstCapitulo.SelectedValue.Substring(0, 4);
            Session["CodSubAta"] = "";
            Session["CodUNPM"] = "";
            BindData();
            BindDataUN("");
            BindDataPsc("");
            BindDataPN("");
        }
        protected void DdlFlota_TextChanged(object sender, EventArgs e)
        {
            BindData();
            BindDataUN("");
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdUltNvl.Rows)
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
                foreach (GridViewRow Row in GrdPosicion.Rows)
                {                    
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                foreach (GridViewRow Row in GrdPn.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    if (Session["ATAPM"].ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un ATA')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VblCodPpal, VBQuery;
                    VblCodPpal = (GrdDatos.FooterRow.FindControl("TxtCodSubN3PP") as TextBox).Text.Trim();
                    if (VblCodPpal.Length < 2)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El codigo debe tener 2 dígitos')", true);
                        return;
                    }
                    VblCodPpal = Session["ATAPM"].ToString().Substring(2, 2) + VblCodPpal;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "SELECT * FROM TblSubCapituloN3 WHERE CodSubCapituloN3='" + VblCodPpal + "' AND CodModelo='" + DdlFlota.SelectedValue + "'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El código existe')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        if (VblCodPpal == String.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una posición')", true);
                            return;
                        }
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 1,@Cod,@Ata,@Desc,@VbC77U,@Mod,'','','','',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";

                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VblCodPpal);
                        sqlCmd.Parameters.AddWithValue("@Ata", Session["ATAPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", (GrdDatos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindData();
                        Session["CodSubAta"] = "";
                        BindDataUN(Session["CodSubAta"].ToString());
                        BindDataPsc("");
                        BindDataPN("");
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            BindData();
            Session["CodSubAta"] = "";
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string VbQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VbQuery = "EXEC SP_TablasPlantillaM 2,'','',@Desc,@VbC77U,'','','','','',@ID,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Desc", (GrdDatos.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(GrdDatos.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdDatos.EditIndex = -1;
                    BindData();
                    Session["CodSubAta"] = "";
                    BindDataUN(Session["CodSubAta"].ToString());
                    BindDataPsc("");
                    BindDataPN("");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdDatos.EditIndex = -1;
            BindData();
            Session["CodSubAta"] = "";
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                BindDataUN(Session["CodSubAta"].ToString());
                BindDataPN("");
                BindDataPsc("");
                if (Session["CodSubAta"].ToString() == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Sub-Ata')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 19,'" + Session["CodSubAta"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        Session["CodSubAta"] = "";
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_TablasPlantillaM 7,'','','','','','','','','',@id,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindData();
                    Session["CodSubAta"] = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((int)ViewState["VblModMS"] == 0)
            {
                GrdDatos.Columns[2].Visible = false;               
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                GrdDatos.Columns[3].Visible = false;
            }
            if ((int)ViewState["VblIngMS"] == 0)
            {
                GrdDatos.Columns[4].Visible = false;
            } 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Seleccione el registro.";
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData();
            Session["CodSubAta"] = "";
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CodSubAta"] = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][1].ToString();
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
            UpPn2.Update();
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if (Row.RowIndex == GrdDatos.SelectedIndex)
                {

                    Row.Style["background-color"] = "#D4DAD3";
                    Row.ToolTip = string.Empty;
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
                    Row.ToolTip = "Seleccione el registro.";
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + Row.RowIndex);
                }               
            }
        }
        protected void GrdUltNvl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdUltNvl.Rows)
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
                foreach (GridViewRow Row in GrdPosicion.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                foreach (GridViewRow Row in GrdPn.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    if (Session["CodSubAta"].ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una SubATA')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VblCodPpal, VBQuery;
                    VblCodPpal = (GrdUltNvl.FooterRow.FindControl("TxtCodSubN4PP") as TextBox).Text.Trim();
                    if (VblCodPpal.Length < 2)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('El codigo debe tener 2 dígitos')", true);
                        return;
                    }
                    VblCodPpal = Session["CodSubAta"].ToString() + VblCodPpal;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "SELECT * FROM TblSubCapituloN4 WHERE CodSubCapituloN4='" + VblCodPpal + "' AND CodModelo='" + DdlFlota.SelectedValue + "'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('El código existe')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        if (VblCodPpal == String.Empty)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una ubicación técnica')", true);
                            return;
                        }
                        string VbNumelem = (GrdUltNvl.FooterRow.FindControl("TxtNumEPP") as TextBox).Text.Trim();
                        if (VbNumelem == string.Empty || VbNumelem.Equals("0"))
                        {
                            VbNumelem = "1";
                        }
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 3,@Cod, @CodSubAta,@Desc, @VbC77U, @Mod,'','','','',@NumElem,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VblCodPpal);
                        sqlCmd.Parameters.AddWithValue("@CodSubAta", Session["CodSubAta"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", (GrdUltNvl.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@NumElem", VbNumelem.ToString());
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindDataUN(Session["CodSubAta"].ToString());
                        BindDataPsc("");
                        BindDataPN("");
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT Ultimo Nivel", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdUltNvl.EditIndex = e.NewEditIndex;
            BindDataUN(Session["CodSubAta"].ToString()); ;
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdUltNvl_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string VbQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    int vddd = Convert.ToInt32(GrdUltNvl.DataKeys[e.RowIndex].Value.ToString());
                    string VbNumelem = (GrdUltNvl.Rows[e.RowIndex].FindControl("TxtNumE") as TextBox).Text.Trim();
                    if (VbNumelem == string.Empty || VbNumelem.Equals("0"))
                    {
                        VbNumelem = "1";
                    }

                    sqlCon.Open();
                    VbQuery = "EXEC SP_TablasPlantillaM 4,@Desc, @VbC77U,'','','','','','','',@NueEle, @ID,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Desc", (GrdUltNvl.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                    sqlCmd.Parameters.AddWithValue("@NueEle", VbNumelem.ToString());
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(GrdUltNvl.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    GrdUltNvl.EditIndex = -1;
                    BindDataUN(Session["CodSubAta"].ToString());
                    BindDataPsc("");
                    BindDataPN("");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdUltNvl.EditIndex = -1;
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdUltNvl_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                BindDataPN(Session["CodUNPM"].ToString());
                BindDataPsc(Session["CodUNPM"].ToString());
                if (Session["CodUNPM"].ToString() == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Ubicación técnica')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 18,'" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        Session["CodUNPM"] = "";
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_TablasPlantillaM 6,'','','','','','','','','',@id,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdUltNvl.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindDataUN(Session["CodSubAta"].ToString());
                    Session["CodUNPM"] = "";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Ultimo Nivel", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdUltNvl, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Seleccione el registro.";
            }
        }
        protected void GrdUltNvl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdUltNvl.PageIndex = e.NewPageIndex;
            BindDataUN(Session["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdUltNvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CodUNPM"] = GrdUltNvl.DataKeys[this.GrdUltNvl.SelectedIndex][1].ToString();
            Session["NumElement"] = GrdUltNvl.DataKeys[this.GrdUltNvl.SelectedIndex][2].ToString();
            BindDataPsc(Session["CodUNPM"].ToString());
            BindDataPN(Session["CodUNPM"].ToString());

            foreach (GridViewRow Row in GrdUltNvl.Rows)
            {
                if (Row.RowIndex == GrdUltNvl.SelectedIndex)
                {

                    Row.Style["background-color"] = "#D4DAD3";
                    Row.ToolTip = string.Empty;
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
                    Row.ToolTip = "Seleccione el registro.";
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdUltNvl, "Select$" + Row.RowIndex);
                }
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
        }
        protected void GrdPosicion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdUltNvl.Rows)
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
                foreach (GridViewRow Row in GrdPosicion.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                foreach (GridViewRow Row in GrdPn.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    if (Session["CodUNPM"].ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Ubicación técnica')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VBQuery;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 13,'" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','',''," + Session["NumElement"].ToString() + ",0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('La cantidad de posiciones superá el número de elementos')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 12,@CodUN, @Mod,'','',@IdPsc,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@CodUN", Session["CodUNPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@IdPsc", (GrdPosicion.FooterRow.FindControl("DdlPscPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.ExecuteNonQuery();
                        BindDataPN(Session["CodUNPM"].ToString());
                        BindDataPsc(Session["CodUNPM"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPosicion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VbPosicion = GrdPosicion.DataKeys[e.RowIndex].Values["Codigo"].ToString();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 14,'" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','" + VbPosicion + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('La posición se encuentra asignada a un elemento')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 15,'','','','',@id,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdPosicion.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindDataPN(Session["CodUNPM"].ToString());
                    BindDataPsc(Session["CodUNPM"].ToString());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPosicion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 11,'" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPscPP = (e.Row.FindControl("DdlPscPP") as DropDownList);
                DdlPscPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPscPP.DataTextField = "Descripcion";
                DdlPscPP.DataValueField = "IdPosicion";
                DdlPscPP.DataBind();
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[2].Controls.Remove(imgD);
                }

            }
        }
        protected void GrdPn_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdUltNvl.Rows)
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
                foreach (GridViewRow Row in GrdPosicion.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                foreach (GridViewRow Row in GrdPn.Rows)
                {
                    if ((int)ViewState["VblEliMS"] == 0)
                    {
                        ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                        if (imgD != null)
                        {
                            Row.Cells[2].Controls.Remove(imgD);
                        }
                    }
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    if (Session["CodUNPM"].ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Ubicación técnica')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VBQuery, VbNivelSuper;
                    VbNivelSuper = DdlFlota.SelectedValue + "-" + Session["ATAPM"].ToString() + "-" + Session["CodSubAta"].ToString() + "-" + Session["CodUNPM"].ToString();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 5,@NivelSuper, @Ref, @CodUN, @Mod, @VbC77U,'','','','',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@CodUN", Session["CodUNPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@Ref", (GrdPn.FooterRow.FindControl("DdlPnPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.Parameters.AddWithValue("@NivelSuper", VbNivelSuper);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindDataPN(Session["CodUNPM"].ToString());
                        BindDataPsc(Session["CodUNPM"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPn_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VbReferenc = GrdPn.DataKeys[e.RowIndex].Values["CodReferencia"].ToString();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 16,'" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','" + VbReferenc + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('La referencia del parte se encuenta instalada en una aeroanve')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 17,'','','','',@id,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdPn.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindDataPN(Session["CodUNPM"].ToString());
                    BindDataPsc(Session["CodUNPM"].ToString());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 3,'" + Session["ATAPM"].ToString() + "','" + Session["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPnPP = (e.Row.FindControl("DdlPnPP") as DropDownList);
                DdlPnPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPnPP.DataTextField = "Pn";
                DdlPnPP.DataValueField = "CodReferencia";
                DdlPnPP.DataBind();
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[2].Controls.Remove(imgD);
                }
            }
        }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {

            string VbTitul, VbTxtToolT, TxtCad, TxtPantIni, NomArc;
            VbTitul = "Exportar plantilla maestra";
            VbTxtToolT = "Ingrese la ubicación técnica a colsultar";
            TxtPantIni = "~/Forms/Configuracion/FrmPlantillaMaestra.aspx";
            TxtCad = "EXEC SP_PANTALLA_Informe_Ingenieria 3,'{0}','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            NomArc = "PlantillaMaestra";
            Response.Redirect("~/Forms/FrmExportar.aspx?TT=" + VbTitul + "&ToolT=" + VbTxtToolT + "&NomArch=" + NomArc + "&TCDN=" + TxtCad + "&PantI=" + TxtPantIni);
        }
    }
}