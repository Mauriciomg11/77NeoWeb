using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using _77NeoWeb.Prg.PrgIngenieria;
using System.IO;
using ClosedXML.Excel;

namespace _77NeoWeb.Forms.Configuracion.MaestIngPrg
{
    public partial class FrmTaller : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null) { Response.Redirect("~/FrmAcceso.aspx"); } /* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Talleres");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["!dC!@"] = 0;
                Session["77IDM"] = "5"; // 4 español | 5 ingles   */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Talleres";
                ModSeguridad();
                BindData(TxtBusqueda.Text);
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
            IdiomaControles();
        }
        protected void IdiomaControles()
        {
            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
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
                    IbtExpExcel.ToolTip = bO.Equals("IbtExpExcel") ? bT : IbtExpExcel.ToolTip;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdCod") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdNombr") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdCC") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdPrefi") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdAct") ? bT : GrdDatos.Columns[4].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        void BindData(string VbConsultar)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_Pantalla_Parametros 12,'" + VbConsultar + "','','','','',0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"] ); ;
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
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text); }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitExportar'");
            foreach (DataRow row in Result)
            { VbNomRpt = row["Texto"].ToString().Trim(); }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurTaller", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_Pantalla_Parametros 13,@TL,'','','','CurTaller',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'"; 
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    SC.Parameters.AddWithValue("@TL", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SC.Connection = con;
                        sda.SelectCommand = SC;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            ds.Tables[0].TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    wb.Worksheets.Add(dt);
                                }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
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
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
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
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens01Tllr'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el nombre')", true);
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
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens02Tllr'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El prefijo se encuenta asignado')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 12, @Desc, @CC, @Pref, @VbUsu,'','','TblTaller','CodTaller','INSERT', @Act,0,0,0,@ICC,5,'01-01-1','02-01-1','03-01-1'";
                        CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                        int VbActivo = 0;
                        if (chkbox.Checked == true) { VbActivo = 1; }
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);

                        sqlCmd.Parameters.AddWithValue("@Desc", VblCodPpal);
                        sqlCmd.Parameters.AddWithValue("@CC", (GrdDatos.FooterRow.FindControl("DdlCCPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.Parameters.AddWithValue("@Pref", VbPrfj);
                        sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                        sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        sqlCmd.ExecuteNonQuery();
                        BindData(TxtBusqueda.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
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
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VblNombre, VbPrfj, VbPfjAnt, VbQuery;

                VblNombre = (GrdDatos.Rows[e.RowIndex].FindControl("TxtNomT") as TextBox).Text.Trim();
                VbPrfj = (GrdDatos.Rows[e.RowIndex].FindControl("TxtPfj") as TextBox).Text.Trim();
                VbPfjAnt = GrdDatos.DataKeys[e.RowIndex].Values["PfjAnt"].ToString();
                if (VblNombre == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens01Tllr'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el nombre')", true);
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
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens02Tllr'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El prefijo se encuenta asignado')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    CheckBox chkbox = GrdDatos.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox;
                    int VbActivo = 0;
                    if (chkbox.Checked == true) { VbActivo = 1; }
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
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de edición')", true);
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
                Idioma = (DataTable)ViewState["TablaIdioma"];
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
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
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
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
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
            string LtxtSql = "EXEC SP_Pantalla_Parametros 14,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlCCPP = (e.Row.FindControl("DdlCCPP") as DropDownList);
                DdlCCPP.DataSource = Cnx.DSET(LtxtSql);
                DdlCCPP.DataTextField = "Nombre";
                DdlCCPP.DataValueField = "CodCc";
                DdlCCPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
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

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
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