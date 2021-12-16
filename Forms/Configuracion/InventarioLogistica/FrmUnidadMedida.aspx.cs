using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Configuracion.InventarioLogistica
{
    public partial class FrmUnidadMedida : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y"))
                {
                    if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
                }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "Configuración Categoría Maestro de articulo";
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
                TitForm.Text = "";
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
            ViewState["VblCE1"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; GrdDatos.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }
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
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtConsultar.ToolTip;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdCod") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdDesc") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdTipo") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdAct") ? bT : GrdDatos.Columns[3].HeaderText;
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
                        Row.Cells[4].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[4].Controls.Remove(imgD);
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
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_TablasGeneral 11, '','','','','','','','','SELECT',0,0,0,0, @Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Datos";
                                DSTDdl.Tables[1].TableName = "Tipo";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            DataTable DT = new DataTable();
            DT = DSTDdl.Tables[0].Clone();
            Result = DSTDdl.Tables[0].Select("Descripcion LIKE '%" + VbConsultar + "%'");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "CodUnidMedida";
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
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                string VbCod = (GrdDatos.FooterRow.FindControl("TxtCodUMPP") as TextBox).Text.Trim();
                if (VbCod == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens09'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el código.
                    return;
                }
                string VbDesc = (GrdDatos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();
                if (VbDesc == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens06'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                    return;
                }
                string VbTipo = (GrdDatos.FooterRow.FindControl("DdlTipoUMPP") as DropDownList).Text.Trim();
                if (VbTipo == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens01UM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo de unidad de medida.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 11,@Cd,@Dsc,@Tp,@US,'','','','','INSERT',@Act,0,0,0,@Idm,@CC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@Cd", VbCod);
                            SC.Parameters.AddWithValue("@Dsc", VbDesc);
                            SC.Parameters.AddWithValue("@Tp", VbTipo);
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Act", (GrdDatos.FooterRow.FindControl("CkbActPP") as CheckBox).Checked == false ? 0 : 1);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
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
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
            string VbCodAnt = GrdDatos.DataKeys[e.RowIndex].Values["CodAnt"].ToString();
            string VbCodTipoUMAnt = GrdDatos.DataKeys[e.RowIndex].Values["CodTipUnMedAnt"].ToString();
            string VbCod = (GrdDatos.Rows[e.RowIndex].FindControl("TxtCodUM") as TextBox).Text.Trim();
            if (VbCod == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens09'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el código.
                return;
            }
            string VbDesc = (GrdDatos.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim();
            if (VbDesc == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens06'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                return;
            }
            string VbTipo = (GrdDatos.Rows[e.RowIndex].FindControl("DdlTipoUM") as DropDownList).Text.Trim();
            if (VbTipo == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01UM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo de unidad de medida.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 11,@Cd,@Dsc,@Tp,@US,@CdAnt,@CdTpUMAnt,'','','UPDATE',@Act,0,0,0, @Idm,@CC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Cd", VbCod);
                        SC.Parameters.AddWithValue("@Dsc", VbDesc);
                        SC.Parameters.AddWithValue("@Tp", VbTipo);
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@CdAnt", VbCodAnt);
                        SC.Parameters.AddWithValue("@CdTpUMAnt", VbCodTipoUMAnt);
                        SC.Parameters.AddWithValue("@Act", (GrdDatos.Rows[e.RowIndex].FindControl("CkbAct") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
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
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();

            string VbCod = GrdDatos.DataKeys[e.RowIndex].Value.ToString();
            string VbCodAnt = GrdDatos.DataKeys[e.RowIndex].Values["CodUnidMedida"].ToString();
            Cnx.SelecBD();

            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 11,@ID,'',@Tp,@US,'','','','','DELETE',0,0,0,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {

                        SC.Parameters.AddWithValue("@ID", VbCod);
                        SC.Parameters.AddWithValue("@Tp", GrdDatos.DataKeys[e.RowIndex].Values["CodTipUnMedAnt"].ToString());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
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
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlTipoUMPP = (e.Row.FindControl("DdlTipoUMPP") as DropDownList);
                DdlTipoUMPP.DataSource = DSTDdl.Tables[1];
                DdlTipoUMPP.DataTextField = "Descripcion";
                DdlTipoUMPP.DataValueField = "CodTipUnMed";
                DdlTipoUMPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlTipoUM = (e.Row.FindControl("DdlTipoUM") as DropDownList);
                DdlTipoUM.DataSource = DSTDdl.Tables[1];
                DdlTipoUM.DataTextField = "Descripcion";
                DdlTipoUM.DataValueField = "CodTipUnMed";
                DdlTipoUM.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlTipoUM.SelectedValue = dr["CodTipUnMed"].ToString();/* */

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }

                if ((int)ViewState["VblCE1"] == 0)
                { TextBox TxtCodUM = (e.Row.FindControl("TxtCodUM") as TextBox); TxtCodUM.Enabled = false; }
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
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, "SEL"); }
    }
}