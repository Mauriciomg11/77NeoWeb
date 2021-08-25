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
        DataSet DSTDdl = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Talleres");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = "00000082"; //00000082|00000133
                    Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = "77NEO01";
                    Session["V$U@"] = "sa";
                    Session["P@$"] = "admindemp";
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Talleres";
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
        protected void BindData(string VbConsultar, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;

            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = " EXEC SP_TablasPlantillaM 12,'','','','','','','','','SELECT',0,0,0,@ICC,0,0,'01-01-1','02-01-1','03-01-1'";
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
                                DSTDdl.Tables[0].TableName = "Taller";
                                DSTDdl.Tables[1].TableName = "CCosto";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];

            DataTable DT = new DataTable();
            DT = DSTDdl.Tables[0].Clone();
            Result = DSTDdl.Tables[0].Select("NomTaller LIKE '%" + VbConsultar + "%'");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "IdTaller DESC";
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
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasPlantillaM 12, @Desc, @CC, @Pref, @VbUsu,'','','TblTaller','CodTaller','INSERT', @Act,0,0,@ICC,0,5,'01-01-1','02-01-1','03-01-1'";
                        CheckBox chkbox = GrdDatos.FooterRow.FindControl("CkbActivoPP") as CheckBox;
                        int VbActivo = 0;
                        if (chkbox.Checked == true) { VbActivo = 1; }
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string Mensj = "";
                                sqlCmd.Parameters.AddWithValue("@Desc", VblCodPpal);
                                sqlCmd.Parameters.AddWithValue("@CC", (GrdDatos.FooterRow.FindControl("DdlCCPP") as DropDownList).SelectedValue.Trim());
                                sqlCmd.Parameters.AddWithValue("@Pref", VbPrfj);
                                sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                                sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SqlDataReader SDR = sqlCmd.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result1)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindData(TxtBusqueda.Text, "UPD");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "INSERT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
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
        { GrdDatos.EditIndex = e.NewEditIndex; BindData(TxtBusqueda.Text, "SEL"); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblNombre, VbPrfj, VbPfjAnt;

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
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    CheckBox chkbox = GrdDatos.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox;
                    int VbActivo = 0;
                    if (chkbox.Checked == true) { VbActivo = 1; }
                    string VbQuery = "EXEC SP_TablasPlantillaM 12, @Desc, @CC, @Pref, @VbUsu, '@Cd',@PfjAnt,'','','UPDATE', @Act,@ID,0,@ICC,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                    {
                        try
                        {
                            string Mensj = "";
                            sqlCmd.Parameters.AddWithValue("@Desc", VblNombre);
                            sqlCmd.Parameters.AddWithValue("@CC", (GrdDatos.Rows[e.RowIndex].FindControl("DdlCC") as DropDownList).SelectedValue.Trim());
                            sqlCmd.Parameters.AddWithValue("@Pref", VbPrfj);
                            sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Cd", GrdDatos.DataKeys[e.RowIndex].Values["CodTaller"].ToString());
                            sqlCmd.Parameters.AddWithValue("@PfjAnt", GrdDatos.DataKeys[e.RowIndex].Values["PfjAnt"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Act", VbActivo);
                            sqlCmd.Parameters.AddWithValue("@ID", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                            }
                            SDR.Close();

                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result1)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdDatos.EditIndex = -1;
                            BindData(TxtBusqueda.Text, "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de edición')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "Update", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
            string VbCod = GrdDatos.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
           
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VbQuery = "EXEC SP_TablasPlantillaM 12, '', '', '', @VbUsu, @Cd,'','','','DELETE', 0,@ID,0,@ICC,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                    {
                        string Mensj = "";
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Cd", GrdDatos.DataKeys[e.RowIndex].Values["CodTaller"].ToString());
                            sqlCmd.Parameters.AddWithValue("@ID", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                            }
                            SDR.Close();

                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result1)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindData(TxtBusqueda.Text, "UPD");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTaller", "DELETE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                     Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable DT = new DataTable();
                DT = DSTDdl.Tables[1].Clone();
                DT.Rows.Add(" - ", "", 1);
                Result = DSTDdl.Tables[1].Select("Activo=1");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
                DropDownList DdlCCPP = (e.Row.FindControl("DdlCCPP") as DropDownList);
                DdlCCPP.DataSource = DT;
                DdlCCPP.DataTextField = "Nombre";
                DdlCCPP.DataValueField = "CodCc";
                DdlCCPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlCC = (DropDownList)e.Row.FindControl("DdlCC");

                DataTable DT = new DataTable();
                DT = DSTDdl.Tables[1].Clone();
              
                GridView gv = (GridView)sender;

                string CodCC = gv.DataKeys[e.Row.RowIndex].Values["CodCc"].ToString();
                string DescCC = gv.DataKeys[e.Row.RowIndex].Values["CentroCosto"].ToString();
                DT.Rows.Add(DescCC, CodCC, 1);
                DT.Rows.Add(" - ", "", 1);

                Result = DSTDdl.Tables[1].Select("Activo=1");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }

                DdlCC.DataSource = DT;
                DdlCC.DataTextField = "Nombre";
                DdlCC.DataValueField = "CodCc";               
                DdlCC.DataBind();

                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlCC.SelectedValue = dr["CCostoTa"].ToString();

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtUpdate'");
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
        { GrdDatos.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, "SEL"); }
    }
}