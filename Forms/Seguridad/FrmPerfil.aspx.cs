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
    public partial class FrmPerfil : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataSet DSTDdl = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */
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
                ModSeguridad();
                ViewState["IdFormRP"] = "0";
                BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
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
        protected void BindData(string VbDesPefil, string VbDesUsu, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_ConfiguracionV2_ 14,@NomUsu,@DesPrfl,'','','',@IdG,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@NomUsu", VbDesUsu);
                        SC.Parameters.AddWithValue("@DesPrfl", VbDesPefil);
                        SC.Parameters.AddWithValue("@IdG", Session["IdGrupoRP"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Grupos";
                                DSTDdl.Tables[1].TableName = "UsuAsignados";
                                DSTDdl.Tables[2].TableName = "UsuSinAsign";
                                DSTDdl.Tables[3].TableName = "PerfilAsig";
                                DSTDdl.Tables[4].TableName = "PerfilSinAsig";
                                DSTDdl.Tables[5].TableName = "TraerDatosParaAsigPerfil";
                                DSTDdl.Tables[6].TableName = "TraerDatosPrflAsigdos";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            string VbCodAnt = DdlGruposRP.Text.Trim();
            DdlGruposRP.DataSource = DSTDdl.Tables[0];
            DdlGruposRP.DataTextField = "NombreGrupo";
            DdlGruposRP.DataValueField = "CodIdGrupo";
            DdlGruposRP.DataBind();
            DdlGruposRP.Text = VbCodAnt;

            GrdDatos.DataSource = DSTDdl.Tables[1];
            GrdDatos.DataBind();

            GrdDatosUsin.DataSource = DSTDdl.Tables[2];
            GrdDatosUsin.DataBind();

            GrdPerfilAsig.DataSource = DSTDdl.Tables[3];
            GrdPerfilAsig.DataBind();

            GrdSinAsig.DataSource = DSTDdl.Tables[4];
            GrdSinAsig.DataBind();
        }
        protected void ActivarControles()
        {
            CkbIng.Visible = false;
            CkbMod.Visible = false;
            CkbCons.Visible = false;
            CkbImpr.Visible = false;
            CkbElim.Visible = false;
            CkbIng.Checked = false;
            CkbMod.Checked = false;
            CkbCons.Checked = false;
            CkbImpr.Checked = false;
            CkbElim.Checked = false;
            CkbCE1.Visible = false;
            CkbCE2.Visible = false;
            CkbCE3.Visible = false;
            CkbCE4.Visible = false;
            CkbCE5.Visible = false;
            CkbCE6.Visible = false;
            CkbCE1.Checked = false;
            CkbCE2.Checked = false;
            CkbCE3.Checked = false;
            CkbCE4.Checked = false;
            CkbCE5.Checked = false;
            CkbCE6.Checked = false;
            IbtAsignarPerfil.Visible = false;
            LblNombrePantalla.Text = "";
        }
        protected void DdlGruposRP_TextChanged(object sender, EventArgs e)
        {
            Session["IdGrupoRP"] = Convert.ToInt32(DdlGruposRP.SelectedValue);
            BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
            ActivarControles();
        }
        protected void IbtIr_Click(object sender, ImageClickEventArgs e)
        { PnlRol.Visible = false; PnlPerfil.Visible = true; }
        protected void IbtRegresar_Click(object sender, ImageClickEventArgs e)
        { PnlRol.Visible = true; PnlPerfil.Visible = false; }
        protected void IbnBusUsu_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD"); }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    Session["IdUsrGruRP"] = int.Parse(GrdDatos.DataKeys[index].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + ex.Message + "')", true);
            }
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)Session["IdGrupoRP"] != 0)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "DELETE FROM TblUsrAsignacionGrupo WHERE CodIdUsrGrupo = @ID";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@ID", (int)Session["IdUsrGruRP"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
                            }
                            catch (Exception)
                            {
                                Transac.Rollback();
                            }
                        }
                    }
                }
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "SEL"); }
        protected void GrdDatosUsin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                Session["IdUsuRP"] = GrdDatosUsin.DataKeys[index].Value.ToString();
            }
        }
        protected void GrdDatosUsin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbCodUsuPerfil = Session["IdGrupoRP"].ToString();
            if (VbCodUsuPerfil != string.Empty)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_ConfiguracionV2_ 13,@Codusu,'','','','INSERT_ASIG_USU',@CodGru,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Codusu", Session["IdUsuRP"].ToString());
                                SC.Parameters.AddWithValue("@CodGru", (int)Session["IdGrupoRP"]);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
                            }
                            catch (Exception)
                            { Transac.Rollback(); }
                        }
                    }
                }
            }
        }
        protected void GrdDatosUsin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatosUsin.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "SEL"); }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "SEL"); }
        protected void IbtAsignarPerfil_Click(object sender, ImageClickEventArgs e)
        {
            string VbCRUD = "";
            string VbCasosEsp = "";
            if (CkbIng.Checked == true)
            { VbCRUD = VbCRUD + '1'; }
            else
            { VbCRUD = VbCRUD + '0'; }

            if (CkbMod.Checked == true)
            { VbCRUD = VbCRUD + '1'; }
            else
            { VbCRUD = VbCRUD + '0'; }
            if (CkbCons.Checked == true)
            { VbCRUD = VbCRUD + '1'; }
            else
            { VbCRUD = VbCRUD + '0'; }
            if (CkbImpr.Checked == true)
            { VbCRUD = VbCRUD + '1'; }
            else
            { VbCRUD = VbCRUD + '0'; }
            if (CkbElim.Checked == true)
            { VbCRUD = VbCRUD + '1'; }
            else
            { VbCRUD = VbCRUD + '0'; }
            if (CkbCE1.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }
            if (CkbCE2.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }
            if (CkbCE3.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }
            if (CkbCE4.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }
            if (CkbCE5.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }
            if (CkbCE6.Checked == true)
            {
                VbCasosEsp = VbCasosEsp + '1';
            }
            else
            {
                VbCasosEsp = VbCasosEsp + '0';
            }

            if ((int)Session["IdGrupoRP"] != 0)
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_ConfiguracionV2_ 16, @Crud, @CsEsp,'','','', @IdGrp, @IdFrm,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Crud", VbCRUD.Trim());
                                SC.Parameters.AddWithValue("@CsEsp", VbCasosEsp.Trim());
                                SC.Parameters.AddWithValue("@IdGrp", Session["IdGrupoRP"].ToString());
                                SC.Parameters.AddWithValue("@IdFrm", ViewState["IdFormRP"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
                            }
                            catch (Exception)
                            { Transac.Rollback(); }
                        }
                    }
                }
            }
            ActivarControles();
        }
        protected void GrdPerfilAsig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                ViewState["CodidUsrPerfil"] = GrdPerfilAsig.DataKeys[index].Value.ToString();
                string borr = GrdPerfilAsig.DataKeys[index].Value.ToString();
            }
        }
        protected void GrdPerfilAsig_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if ((int)Session["IdGrupoRP"] != 0)
                {
                    DSTDdl = (DataSet)ViewState["DSTDdl"];
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[6].Clone();
                    DataRow[] Result = DSTDdl.Tables[6].Select("CodidUsrPerfil = " + ViewState["CodidUsrPerfil"].ToString().Trim());
                    foreach (DataRow tbl in Result)
                    {
                        LblNombrePantalla.Text = tbl["NomFormWeb"].ToString();
                        string VblNomPant = tbl["NomFormWeb"].ToString();
                        CkbIng.Checked = Convert.ToBoolean(tbl["IngresarF"]);
                        CkbMod.Checked = Convert.ToBoolean(tbl["ModificarF"]);
                        CkbCons.Checked = Convert.ToBoolean(tbl["ConsultarF"]);
                        CkbImpr.Checked = Convert.ToBoolean(tbl["ImprimirF"]);
                        CkbElim.Checked = Convert.ToBoolean(tbl["EliminarF"]);
                        CkbCE1.Checked = Convert.ToBoolean(tbl["CEF1"]);
                        CkbCE2.Checked = Convert.ToBoolean(tbl["CEF2"]);
                        CkbCE3.Checked = Convert.ToBoolean(tbl["CEF3"]);
                        CkbCE4.Checked = Convert.ToBoolean(tbl["CEF4"]);
                        CkbCE5.Checked = Convert.ToBoolean(tbl["CEF5"]);
                        CkbCE6.Checked = Convert.ToBoolean(tbl["CEF6"]);
                        CkbIng.Visible = true;
                        CkbMod.Visible = true;
                        CkbCons.Visible = true;
                        CkbImpr.Visible = true;
                        CkbElim.Visible = true;
                        CkbCE1.Visible = false;
                        CkbCE2.Visible = false;
                        CkbCE3.Visible = false;
                        CkbCE4.Visible = false;
                        CkbCE5.Visible = false;
                        CkbCE6.Visible = false;
                        IbtAsignarPerfil.Visible = false;

                        if (Convert.ToBoolean(tbl["IngresarF"]).Equals(false))
                        {
                            CkbIng.Visible = false;
                        }
                        if (Convert.ToBoolean(tbl["ModificarF"]).Equals(false))
                        {
                            CkbMod.Visible = false;
                        }
                        if (Convert.ToBoolean(tbl["ConsultarF"]).Equals(false))
                        {
                            CkbCons.Visible = false;
                        }
                        if (Convert.ToBoolean(tbl["ImprimirF"]).Equals(false))
                        {
                            CkbImpr.Visible = false;
                        }
                        if (Convert.ToBoolean(tbl["EliminarF"]).Equals(false))
                        {
                            CkbElim.Visible = false;
                        }
                        if (Convert.ToBoolean(tbl["CEF1"]).Equals(true))
                        {
                            CkbCE1.Visible = true;
                            CkbCE1.Text = tbl["CasoEspeciaLF1"].ToString();
                        }
                        if (Convert.ToBoolean(tbl["CEF2"]).Equals(true))
                        {
                            CkbCE2.Visible = true;
                            CkbCE2.Text = tbl["CasoEspeciaLF2"].ToString();
                        }
                        if (Convert.ToBoolean(tbl["CEF3"]).Equals(true))
                        {
                            CkbCE3.Visible = true;
                            CkbCE3.Text = tbl["CasoEspeciaLF3"].ToString();
                        }
                        if (Convert.ToBoolean(tbl["CEF4"]).Equals(true))
                        {
                            CkbCE4.Visible = true;
                            CkbCE4.Text = tbl["CasoEspeciaLF4"].ToString();
                        }
                        if (Convert.ToBoolean(tbl["CEF5"]).Equals(true))
                        {
                            CkbCE5.Visible = true;
                            CkbCE5.Text = tbl["CasoEspeciaLF5"].ToString();
                        }
                        if (Convert.ToBoolean(tbl["CEF6"]).Equals(true))
                        {
                            CkbCE6.Visible = true;
                            CkbCE6.Text = tbl["CasoEspeciaLF6"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + ex.Message + "')", true);
            }
        }
        protected void GrdPerfilAsig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if ((int)Session["IdGrupoRP"] != 0)
            {
                Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM TblUsrPerfiles WHERE CodidUsrPerfil = @ID";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(GrdPerfilAsig.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    ActivarControles();
                    BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "UPD");
                }
            }
        }
        protected void GrdPerfilAsig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string VbNomFormWeb = DataBinder.Eval(e.Row.DataItem, "NomFormWeb").ToString();

                if (VbNomFormWeb == string.Empty) { e.Row.BackColor = System.Drawing.Color.DarkOrange; }
            }
        }
        protected void GrdSinAsig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if ((int)Session["IdGrupoRP"] != 0)
                {
                    if (e.CommandName == "Select")
                    {
                        ActivarControles();
                        int index = int.Parse(e.CommandArgument.ToString());
                        ViewState["IdFormRP"] = int.Parse(GrdSinAsig.DataKeys[index].Value.ToString());

                        DSTDdl = (DataSet)ViewState["DSTDdl"];
                        DataTable DT = new DataTable();
                        DT = DSTDdl.Tables[5].Clone();
                        DataRow[] Result = DSTDdl.Tables[5].Select("CodIdFormulario = " + ViewState["IdFormRP"].ToString());
                        foreach (DataRow tbl in Result)
                        {
                            DT.ImportRow(tbl);
                            LblNombrePantalla.Text = tbl["NomFormWeb"].ToString();
                            string VblNomPant = tbl["NomFormWeb"].ToString();
                            CkbIng.Checked = Convert.ToBoolean(tbl["IngresarF"]);
                            CkbMod.Checked = Convert.ToBoolean(tbl["ModificarF"]);
                            CkbCons.Checked = Convert.ToBoolean(tbl["ConsultarF"]);
                            CkbImpr.Checked = Convert.ToBoolean(tbl["ImprimirF"]);
                            CkbElim.Checked = Convert.ToBoolean(tbl["EliminarF"]);
                            CkbIng.Visible = true;
                            CkbMod.Visible = true;
                            CkbCons.Visible = true;
                            CkbImpr.Visible = true;
                            CkbElim.Visible = true;
                            CkbCE1.Visible = false;
                            CkbCE2.Visible = false;
                            CkbCE3.Visible = false;
                            CkbCE4.Visible = false;
                            CkbCE5.Visible = false;
                            CkbCE6.Visible = false;
                            IbtAsignarPerfil.Visible = false;
                            if (VblNomPant != string.Empty)
                            {
                                IbtAsignarPerfil.Visible = true;
                            }
                            if (Convert.ToBoolean(tbl["IngresarF"]).Equals(false))
                            {
                                CkbIng.Visible = false;
                            }
                            if (Convert.ToBoolean(tbl["ModificarF"]).Equals(false))
                            {
                                CkbMod.Visible = false;
                            }
                            if (Convert.ToBoolean(tbl["ConsultarF"]).Equals(false))
                            {
                                CkbCons.Visible = false;
                            }
                            if (Convert.ToBoolean(tbl["ImprimirF"]).Equals(false))
                            {
                                CkbImpr.Visible = false;
                            }
                            if (Convert.ToBoolean(tbl["EliminarF"]).Equals(false))
                            {
                                CkbElim.Visible = false;
                            }
                            if (Convert.ToBoolean(tbl["CEF1"]).Equals(true))
                            {
                                CkbCE1.Visible = true;
                                CkbCE1.Text = tbl["CasoEspeciaLF1"].ToString();
                            }
                            if (Convert.ToBoolean(tbl["CEF2"]).Equals(true))
                            {
                                CkbCE2.Visible = true;
                                CkbCE2.Text = tbl["CasoEspeciaLF2"].ToString();
                            }
                            if (Convert.ToBoolean(tbl["CEF3"]).Equals(true))
                            {
                                CkbCE3.Visible = true;
                                CkbCE3.Text = tbl["CasoEspeciaLF3"].ToString();
                            }
                            if (Convert.ToBoolean(tbl["CEF4"]).Equals(true))
                            {
                                CkbCE4.Visible = true;
                                CkbCE4.Text = tbl["CasoEspeciaLF4"].ToString();
                            }
                            if (Convert.ToBoolean(tbl["CEF5"]).Equals(true))
                            {
                                CkbCE5.Visible = true;
                                CkbCE5.Text = tbl["CasoEspeciaLF5"].ToString();
                            }
                            if (Convert.ToBoolean(tbl["CEF6"]).Equals(true))
                            {
                                CkbCE6.Visible = true;
                                CkbCE6.Text = tbl["CasoEspeciaLF6"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + ex.Message + "')", true);
            }
        }
        protected void GrdSinAsig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string VbNomFormWeb = DataBinder.Eval(e.Row.DataItem, "NomFormWeb").ToString();

                if (VbNomFormWeb == string.Empty) { e.Row.BackColor = System.Drawing.Color.DarkOrange; }
            }
        }
        protected void GrdSinAsig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdSinAsig.PageIndex = e.NewPageIndex; BindData(TxtBusqueda.Text, TxtBusqUsu.Text, "SEL"); }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetFixedHeightForGridIfRowsAreLess(GrdSinAsig);
            SetFixedHeightForGridIfRowsAreLess(GrdPerfilAsig);
            SetFixedHeightForGridIfRowsAreLess(GrdDatosUsin);
        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 20; // height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 20;
                gv.Height = new Unit(height);
            }
        }

    }
}