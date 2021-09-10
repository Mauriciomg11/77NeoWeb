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
using System.Configuration;
using System.Globalization;

namespace _77NeoWeb.Forms.Configuracion.MaestIngPrg
{
    public partial class FrmCicloDiscosMotor : System.Web.UI.Page
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
            Page.Title = string.Format("Configuración_Discos");
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
                     Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración Ciclos de Discos";
                ModSeguridad();
                BtnAlaF.BackColor = Color.SandyBrown;
                BtnAlaF.Font.Bold = true;
                BtnAlaF.Font.Size = 14;
                BtnAlaR.BackColor = Color.LightBlue;
                BindDdl("UPD");
                BindDataAlaF("");
                BindDataAlaR("");

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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmCicloDiscosMotor.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                GrdDatos.ShowFooter = false;
                GrdAR.ShowFooter = false;
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
                //IbtExpExcel.Visible = false;
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
                SC.Parameters.AddWithValue("@F1", "FRMCICLOSESPECIALMOTOR");
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

                    TitForm.Text = bO.Equals("LblTituloCiDis") ? bT : TitForm.Text;
                    BtnAlaF.Text = bO.Equals("BtnAlaF") ? bT : BtnAlaF.Text;
                    BtnAlaR.Text = bO.Equals("BtnAlaR") ? bT : BtnAlaR.Text;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdMtr") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdSubCmpn") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdClEquiv") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdAR.Columns[0].HeaderText = bO.Equals("GrdMtr") ? bT : GrdAR.Columns[0].HeaderText;
                    GrdAR.Columns[1].HeaderText = bO.Equals("GrdSubCmpn") ? bT : GrdAR.Columns[1].HeaderText;
                    GrdAR.Columns[2].HeaderText = bO.Equals("GrdDesc") ? bT : GrdAR.Columns[2].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string LtxtSql = "EXEC SP_Pantalla_Parametros 16,'','','','','DDL',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(LtxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Motor";
                                DSTDdl.Tables[1].TableName = "SubC";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
        }
        protected void BindDataAlaF(string VbConsultar)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 16, @Cns,'','','','ALAFIJA',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Cns", VbConsultar);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0) { GrdDatos.DataSource = dtbl; GrdDatos.DataBind(); }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BindDataAlaR(string VbConsultar)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            DataTable DtAR = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 16, @Cns,'','','','ALAROTA',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Cns", VbConsultar);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DtAR);
                }
            }
            if (DtAR.Rows.Count > 0)
            {
                GrdAR.DataSource = DtAR;
                GrdAR.DataBind();
            }
            else
            {
                DtAR.Rows.Add(DtAR.NewRow());
                GrdAR.DataSource = DtAR;
                GrdAR.DataBind();
                GrdAR.Rows[0].Cells.Clear();
                GrdAR.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdAR.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdAR.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BtnAlaF_Click(object sender, EventArgs e)
        {
            BtnAlaF.BackColor = Color.SandyBrown;
            BtnAlaF.Font.Bold = true;
            BtnAlaF.Font.Size = 14;

            BtnAlaR.BackColor = Color.LightBlue;
            BtnAlaR.Font.Bold = false;
            BtnAlaR.Font.Size = 13;
            PnlAF.Visible = true;
            PnlAR.Visible = false;
            BindDataAlaF("");
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
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
        protected void BtnAlaR_Click(object sender, EventArgs e)
        {
            BtnAlaR.BackColor = Color.SandyBrown;
            BtnAlaR.Font.Bold = true;
            BtnAlaR.Font.Size = 14;

            BtnAlaF.BackColor = Color.LightBlue;
            BtnAlaF.Font.Bold = false;
            BtnAlaF.Font.Size = 13;

            PnlAF.Visible = false;
            PnlAR.Visible = true;
            foreach (GridViewRow RowAR in GrdAR.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = RowAR.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        RowAR.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdAR.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
            if (e.CommandName.Equals("AddNew"))
            {
                string VbMayor, VbSubC, VBQuery;
                VbMayor = (GrdDatos.FooterRow.FindControl("DdlMotorPP") as DropDownList).SelectedValue.Trim();
                VbSubC = (GrdDatos.FooterRow.FindControl("DdlSubCPP") as DropDownList).SelectedValue.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                string VbTxtVlr;
                double VbVlr;
                VbTxtVlr = (GrdDatos.FooterRow.FindControl("TxtCiclosPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDatos.FooterRow.FindControl("TxtCiclosPP") as TextBox).Text.Trim();
                VbVlr = VbTxtVlr.Length == 0 ? 0 : Convert.ToDouble(VbTxtVlr, Culture);

                if (VbMayor == String.Empty || VbSubC == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mensj01CicDisk'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El mayor y el subcomponentes son obligatorios
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasPlantillaM 14, @My, @SbC,'', @Us,'','','','A','INSERT', @Valor,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@My", VbMayor);
                                SC.Parameters.AddWithValue("@SbC", VbSubC);
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@Valor", VbVlr);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDataAlaF("");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT AlA FIJA", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }/**/
                }
            }
        }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasPlantillaM 14,'','','','{0}','','','{1}','A','DELETE',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'",
                     Session["C77U"], GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAlaF("");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE AlA FIJA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }

        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlMotorPP = (e.Row.FindControl("DdlMotorPP") as DropDownList);
                DdlMotorPP.DataSource = DSTDdl.Tables[0];
                DdlMotorPP.DataTextField = "PN";
                DdlMotorPP.DataValueField = "CodPN";
                DdlMotorPP.DataBind();

                DropDownList DdlSubCPP = (e.Row.FindControl("DdlSubCPP") as DropDownList);
                DdlSubCPP.DataSource = DSTDdl.Tables[1];
                DdlSubCPP.DataTextField = "PN";
                DdlSubCPP.DataValueField = "PN";
                DdlSubCPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlMotor = (DropDownList)e.Row.FindControl("DdlMotor");
                DdlMotor.DataTextField = "PN";
                DdlMotor.DataValueField = "CodPN";
                DdlMotor.DataSource = DSTDdl.Tables[0];
                DdlMotor.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlMotor.SelectedValue = dr["Engine"].ToString();

                DropDownList DdlSubC = (DropDownList)e.Row.FindControl("DdlSubC");
                DdlSubC.DataTextField = "PN";
                DdlSubC.DataValueField = "PN";
                DdlSubC.DataSource = DSTDdl.Tables[1];
                DdlSubC.DataBind();
                DataRowView drSc = e.Row.DataItem as DataRowView;
                DdlSubC.SelectedValue = drSc["PN"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
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
            }/* */
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[3].Controls.Remove(imgD);
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
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindDataAlaF("");
        }
        protected void DdlSubCPP_TextChanged(object sender, EventArgs e)
        {
            string VbPn;
            VbPn = (GrdAR.FooterRow.FindControl("DdlSubCPP") as DropDownList).SelectedValue.Trim();
            if (VbPn.ToString() != string.Empty)
            {
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                TextBox TxtDescPP = GrdAR.FooterRow.FindControl("TxtDescPP") as TextBox;
                DataRow[] Result = DSTDdl.Tables[1].Select("PN= '" + VbPn + "'");
                foreach (DataRow row in Result)
                {
                    if (TxtDescPP != null) { TxtDescPP.Text = row["Descripcion"].ToString(); }
                }
            }
            foreach (GridViewRow RowAR in GrdAR.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = RowAR.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        RowAR.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void GridAR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdAR.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
            if (e.CommandName.Equals("AddNew"))
            {
                string VbMayor, VbSubC, VBQuery;
                VbMayor = (GrdAR.FooterRow.FindControl("DdlMotorPP") as DropDownList).SelectedValue.Trim();
                VbSubC = (GrdAR.FooterRow.FindControl("DdlSubCPP") as DropDownList).SelectedValue.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                string VbTxtCFA, VbTxtCFE, VbTxtCFF, VbTxtLLH, VbTxtLLC;
                double VbCFA, VbCFE, VbCFF, VbLLH, VbLLC;
                VbTxtCFA = (GrdAR.FooterRow.FindControl("TxtCFAbbrPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtCFAbbrPP") as TextBox).Text.Trim();
                VbCFA = VbTxtCFA.Length == 0 ? 0 : Convert.ToDouble(VbTxtCFA, Culture);

                VbTxtCFE = (GrdAR.FooterRow.FindControl("TxtCFExtPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtCFExtPP") as TextBox).Text.Trim();
                VbCFE = VbTxtCFE.Length == 0 ? 0 : Convert.ToDouble(VbTxtCFE, Culture);

                VbTxtCFF = (GrdAR.FooterRow.FindControl("TxtFCFactorPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtFCFactorPP") as TextBox).Text.Trim();
                VbCFF = VbTxtCFF.Length == 0 ? 0 : Convert.ToDouble(VbTxtCFF, Culture);

                VbTxtLLH = (GrdAR.FooterRow.FindControl("TxtLLHoursPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtLLHoursPP") as TextBox).Text.Trim();
                VbLLH = VbTxtLLH.Length == 0 ? 0 : Convert.ToDouble(VbTxtLLH, Culture);

                VbTxtLLC = (GrdAR.FooterRow.FindControl("TxtLLCyclesPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtLLCyclesPP") as TextBox).Text.Trim();
                VbLLC = VbTxtLLC.Length == 0 ? 0 : Convert.ToDouble(VbTxtLLC, Culture);

                if (VbMayor == String.Empty || VbSubC == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mensj01CicDisk'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El mayor y el subcomponentes son obligatorios')", true);
                    return;
                }
                Cnx.SelecBD();

                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasPlantillaM 14, @Myr, @SubC, @Desc,@Usu,'','','','H','INSERT', @ICC, @CFA, @CFE, @CFF, @LLH, @LLC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Myr", VbMayor);
                                sqlCmd.Parameters.AddWithValue("@SubC", VbSubC);
                                sqlCmd.Parameters.AddWithValue("@Desc", (GrdAR.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.Parameters.AddWithValue("@CFA", VbCFA);
                                sqlCmd.Parameters.AddWithValue("@CFE", VbCFE);
                                sqlCmd.Parameters.AddWithValue("@CFF", VbCFF);
                                sqlCmd.Parameters.AddWithValue("@LLH", VbLLH);
                                sqlCmd.Parameters.AddWithValue("@LLC", VbLLC);
                                sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"]);
                                var Mensj = sqlCmd.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDataAlaR("");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT ALA ROTATORIA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GridAR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasPlantillaM 14,'','','','{0}','','','{1}','A','DELETE',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'",
                    Session["C77U"], GrdAR.DataKeys[e.RowIndex].Value.ToString());
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAlaR("");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminaci
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE ALA ROTATORIA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GridAR_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow Row in GrdAR.Rows)
            {
                if (Row.RowIndex == GrdAR.SelectedIndex)
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
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAR, "Select$" + Row.RowIndex);
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void GridAR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlMotorPP = (e.Row.FindControl("DdlMotorPP") as DropDownList);
                DdlMotorPP.DataSource = DSTDdl.Tables[0];
                DdlMotorPP.DataTextField = "PN";
                DdlMotorPP.DataValueField = "CodPN";
                DdlMotorPP.DataBind();

                DropDownList DdlSubCPP = (e.Row.FindControl("DdlSubCPP") as DropDownList);
                DdlSubCPP.DataSource = DSTDdl.Tables[1];
                DdlSubCPP.DataTextField = "PN";
                DdlSubCPP.DataValueField = "PN";
                DdlSubCPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlMotor = (DropDownList)e.Row.FindControl("DdlMotor");
                DdlMotor.DataTextField = "PN";
                DdlMotor.DataValueField = "CodPN";
                DdlMotor.DataSource = DSTDdl.Tables[0];
                DdlMotor.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlMotor.SelectedValue = dr["Engine"].ToString();

                DropDownList DdlSubC = (DropDownList)e.Row.FindControl("DdlSubC");
                DdlSubC.DataTextField = "PN";
                DdlSubC.DataValueField = "PN";
                DdlSubC.DataSource = DSTDdl.Tables[1];
                DdlSubC.DataBind();
                DataRowView drSc = e.Row.DataItem as DataRowView;
                DdlSubC.SelectedValue = drSc["PN"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAR, "Select$" + e.Row.RowIndex);
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

            }/* */
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[8].Controls.Remove(imgD);
                }
            }
        }
        protected void GridAR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAR.PageIndex = e.NewPageIndex;
            BindDataAlaR("");
        }
    }
}