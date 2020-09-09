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
    public partial class FrmCicloDiscosMotor : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }  /**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Discos");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/*  */
                /*Session["C77U"] = "00000082";// 00000132 00000082
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1"; */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración Ciclos de Discos";
                ModSeguridad();
                BtnAlaF.BackColor = Color.SandyBrown;
                BtnAlaF.Font.Bold = true;
                BtnAlaF.Font.Size = 14;
                BtnAlaR.BackColor = Color.LightBlue;
                BindDataAlaF("");
                BindDataAlaR("");
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
        }
        void BindDataAlaF(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 16,'" + VbConsultar + "','','','','ALAFIJA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
        void BindDataAlaR(string VbConsultar)
        {
            DataTable DtAR = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 16,'" + VbConsultar + "','','','','ALAROTA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                SqlDataAdapter SqlDAAR = new SqlDataAdapter(VbTxtSql, sqlCon);
                SqlDAAR.Fill(DtAR);
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
                GrdAR.Rows[0].Cells[0].ColumnSpan = DtAR.Columns.Count;
                GrdAR.Rows[0].Cells[0].Text = "No existen registros ..!";
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
                string VbTxtVlr;
                double VbVlr;
                VbTxtVlr = (GrdDatos.FooterRow.FindControl("TxtCiclosPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDatos.FooterRow.FindControl("TxtCiclosPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtVlr);
                VbTxtVlr = Cnx.ValorDecimal();
                VbVlr = (GrdDatos.FooterRow.FindControl("TxtCiclosPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtVlr);

                if (VbMayor == String.Empty || VbSubC == String.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('El mayor y el subcomponentes son obligatorios')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasPlantillaM 14, '{0}', '{1}','','{2}','','','','A','INSERT', @Valor,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                        VbMayor, VbSubC, Session["C77U"]);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Valor", VbVlr);
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDataAlaF("");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT AlA FIJA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }/**/
                }
            }
        }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasPlantillaM 14,'','','','{0}','','','{1}','A','DELETE',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                     Session["C77U"], GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAlaF("");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE AlA FIJA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }

        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string LtxtSql = "EXEC SP_Pantalla_Parametros 16,'','','','','MOTOR',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            string LtxtSqlSC = "EXEC SP_Pantalla_Parametros 16,'','','','','SUBC',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlMotorPP = (e.Row.FindControl("DdlMotorPP") as DropDownList);
                DdlMotorPP.DataSource = Cnx.DSET(LtxtSql);
                DdlMotorPP.DataTextField = "PN";
                DdlMotorPP.DataValueField = "CodPN";
                DdlMotorPP.DataBind();

                DropDownList DdlSubCPP = (e.Row.FindControl("DdlSubCPP") as DropDownList);
                DdlSubCPP.DataSource = Cnx.DSET(LtxtSqlSC);
                DdlSubCPP.DataTextField = "PN";
                DdlSubCPP.DataValueField = "PN";
                DdlSubCPP.DataBind();
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlMotor = (DropDownList)e.Row.FindControl("DdlMotor");
                DdlMotor.DataTextField = "PN";
                DdlMotor.DataValueField = "CodPN";
                DdlMotor.DataSource = Cnx.DSET(LtxtSql);
                DdlMotor.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlMotor.SelectedValue = dr["Engine"].ToString();

                DropDownList DdlSubC = (DropDownList)e.Row.FindControl("DdlSubC");
                DdlSubC.DataTextField = "PN";
                DdlSubC.DataValueField = "PN";
                DdlSubC.DataSource = Cnx.DSET(LtxtSqlSC);
                DdlSubC.DataBind();
                DataRowView drSc = e.Row.DataItem as DataRowView;
                DdlSubC.SelectedValue = drSc["PN"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
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
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = " EXEC SP_Pantalla_Parametros 17,'" + VbPn + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader tbl = SqlC.ExecuteReader();
                    if (tbl.Read())
                    {
                        TextBox TxtDescPP = GrdAR.FooterRow.FindControl("TxtDescPP") as TextBox;
                        if (TxtDescPP != null)
                        {
                            TxtDescPP.Text = tbl["Descripcion"].ToString();
                        }
                    }
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
                string VbTxtCFA, VbTxtCFE, VbTxtCFF, VbTxtLLH, VbTxtLLC;
                double VbCFA, VbCFE, VbCFF, VbLLH, VbLLC;
                VbTxtCFA = (GrdAR.FooterRow.FindControl("TxtCFAbbrPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtCFAbbrPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtCFA);
                VbTxtCFA = Cnx.ValorDecimal();
                VbCFA = (GrdAR.FooterRow.FindControl("TxtCFAbbrPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCFA);

                VbTxtCFE = (GrdAR.FooterRow.FindControl("TxtCFExtPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtCFExtPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtCFE);
                VbTxtCFE = Cnx.ValorDecimal();
                VbCFE = (GrdAR.FooterRow.FindControl("TxtCFExtPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCFE);

                VbTxtCFF = (GrdAR.FooterRow.FindControl("TxtFCFactorPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtFCFactorPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtCFF);
                VbTxtCFF = Cnx.ValorDecimal();
                VbCFF = (GrdAR.FooterRow.FindControl("TxtFCFactorPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCFF);

                VbTxtLLH = (GrdAR.FooterRow.FindControl("TxtLLHoursPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtLLHoursPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtLLH);
                VbTxtLLH = Cnx.ValorDecimal();
                VbLLH = (GrdAR.FooterRow.FindControl("TxtLLHoursPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtLLH);

                VbTxtLLC = (GrdAR.FooterRow.FindControl("TxtLLCyclesPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdAR.FooterRow.FindControl("TxtLLCyclesPP") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VbTxtLLC);
                VbTxtLLC = Cnx.ValorDecimal();
                VbLLC = (GrdAR.FooterRow.FindControl("TxtLLCyclesPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtLLC);

                if (VbMayor == String.Empty || VbSubC == String.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAR, UpPnlAR.GetType(), "IdntificadorBloqueScript", "alert('El mayor y el subcomponentes son obligatorios')", true);
                    return;
                }

                Cnx.SelecBD();

                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasPlantillaM 14, @Myr, @SubC, @Desc,@Usu,'','','','H','INSERT', 0, @CFA, @CFE, @CFF, @LLH, @LLC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Myr", VbMayor);
                                sqlCmd.Parameters.AddWithValue("@SubC", VbSubC);
                                sqlCmd.Parameters.AddWithValue("@Desc", (GrdAR.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@CFA", VbCFA);
                                sqlCmd.Parameters.AddWithValue("@CFE", VbCFE);
                                sqlCmd.Parameters.AddWithValue("@CFF", VbCFF);
                                sqlCmd.Parameters.AddWithValue("@LLH", VbLLH);
                                sqlCmd.Parameters.AddWithValue("@LLC", VbLLC);
                                sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"]);
                                var Mensj = sqlCmd.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlAF, UpPnlAF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDataAlaR("");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlAR, UpPnlAR.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT ALA ROTATORIA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                            }

                        }
                    }
                }/**/
            }
        }
        protected void GridAR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasPlantillaM 14,'','','','{0}','','','{1}','A','DELETE',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                    Session["C77U"], GrdAR.DataKeys[e.RowIndex].Value.ToString());                   
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {                            
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAlaR("");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlAR, UpPnlAR.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
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
            string LtxtSql = "EXEC SP_Pantalla_Parametros 16,'','','','','MOTOR',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            string LtxtSqlSC = "EXEC SP_Pantalla_Parametros 16,'','','','','SUBC',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlMotorPP = (e.Row.FindControl("DdlMotorPP") as DropDownList);
                DdlMotorPP.DataSource = Cnx.DSET(LtxtSql);
                DdlMotorPP.DataTextField = "PN";
                DdlMotorPP.DataValueField = "CodPN";
                DdlMotorPP.DataBind();

                DropDownList DdlSubCPP = (e.Row.FindControl("DdlSubCPP") as DropDownList);
                DdlSubCPP.DataSource = Cnx.DSET(LtxtSqlSC);
                DdlSubCPP.DataTextField = "PN";
                DdlSubCPP.DataValueField = "PN";
                DdlSubCPP.DataBind();
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList DdlMotor = (DropDownList)e.Row.FindControl("DdlMotor");
                DdlMotor.DataTextField = "PN";
                DdlMotor.DataValueField = "CodPN";
                DdlMotor.DataSource = Cnx.DSET(LtxtSql);
                DdlMotor.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlMotor.SelectedValue = dr["Engine"].ToString();

                DropDownList DdlSubC = (DropDownList)e.Row.FindControl("DdlSubC");
                DdlSubC.DataTextField = "PN";
                DdlSubC.DataValueField = "PN";
                DdlSubC.DataSource = Cnx.DSET(LtxtSqlSC);
                DdlSubC.DataBind();
                DataRowView drSc = e.Row.DataItem as DataRowView;
                DdlSubC.SelectedValue = drSc["PN"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAR, "Select$" + e.Row.RowIndex);
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