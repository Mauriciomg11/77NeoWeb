﻿using System;
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
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System.IO;

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmPlantillaMaestra : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
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
                Session["C77U"] = "";
               /* Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles   */
            }
            if (!IsPostBack)
            {
                ViewState["PageTit"] = "";
                TitForm.Text = "Configuración Plantilla Maestra";
                Session["ValPM"] = "S";
                ViewState["ATAPM"] = "";
                ViewState["CodSubAta"] = "";
                ViewState["CodUNPM"] = "";
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
                DdlFlota.Text = "";
                BindData();
                BindDataUN("");
                BindDataPsc("");
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
            IdiomaControles();
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
                        Row.Cells[2].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[2].Controls.Remove(imgD);
                    }
                }
            }
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
                SC.Parameters.AddWithValue("@F1", "FRMCAPITULONEW");
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
                    if (bO.Equals("CaptionPlanMaesra"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("TituloPlanMaestra") ? bT : TitForm.Text;
                    LblFlota.Text = bO.Equals("LblFlota") ? bT : LblFlota.Text;
                    IbtExpExcel.ToolTip = bO.Equals("IbtExpExcelTT") ? bT : IbtExpExcel.ToolTip;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdDescrip") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdUltNvl.Columns[0].HeaderText = bO.Equals("GrdUbicTnc") ? bT : GrdUltNvl.Columns[0].HeaderText;
                    GrdUltNvl.Columns[1].HeaderText = bO.Equals("GrdDescrip") ? bT : GrdUltNvl.Columns[1].HeaderText;
                    GrdUltNvl.Columns[2].HeaderText = bO.Equals("GrdNumElem") ? bT : GrdUltNvl.Columns[2].HeaderText;
                    GrdPosicion.Columns[0].HeaderText = bO.Equals("GrdUbca") ? bT : GrdPosicion.Columns[0].HeaderText;
                    GrdPosicion.Columns[1].HeaderText = bO.Equals("GrdPscn") ? bT : GrdPosicion.Columns[1].HeaderText;
                    GrdPn.Columns[0].HeaderText = bO.Equals("GrdPNPpl") ? bT : GrdPn.Columns[0].HeaderText;
                    GrdPn.Columns[1].HeaderText = bO.Equals("GrdDescrip") ? bT : GrdPn.Columns[1].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        void BindData()
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable dtbl = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Capitulo_PM 7,'" + ViewState["ATAPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
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
                        DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                        foreach (DataRow row in Result)
                        { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }

                        GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception ex)
            { string borr = ex.ToString(); }
        }
        void BindDataUN(string CodUN)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
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
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdUltNvl.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                    GrdUltNvl.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        void BindDataPsc(string CodUN)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
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
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdPosicion.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                    GrdPosicion.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        void BindDataPN(string CodUN)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
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
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdPn.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                    GrdPn.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        protected void LstCapitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["ATAPM"] = LstCapitulo.SelectedValue.Substring(0, 4);
            ViewState["CodSubAta"] = "";
            ViewState["CodUNPM"] = "";
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
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (ViewState["ATAPM"].ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr01'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar un ATA
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr02'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VblCodPpal, VBQuery;
                    VblCodPpal = (GrdDatos.FooterRow.FindControl("TxtCodSubN3PP") as TextBox).Text.Trim();
                    if (VblCodPpal.Length < 2)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr03'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //El codigo debe tener 2 dígitos')", true);
                        return;
                    }
                    VblCodPpal = ViewState["ATAPM"].ToString().Substring(2, 2) + VblCodPpal;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "SELECT * FROM TblSubCapituloN3 WHERE CodSubCapituloN3='" + VblCodPpal + "' AND CodModelo='" + DdlFlota.SelectedValue + "'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr04'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //El código existe')", true);
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
                        sqlCmd.Parameters.AddWithValue("@Ata", ViewState["ATAPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", (GrdDatos.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindData();
                        ViewState["CodSubAta"] = "";
                        BindDataUN(ViewState["CodSubAta"].ToString());
                        BindDataPsc("");
                        BindDataPN("");
                    }
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdDatos.EditIndex = e.NewEditIndex;
            BindData();
            ViewState["CodSubAta"] = "";
            BindDataUN(ViewState["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN(""); /**/
        }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            PerfilesGrid();
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
                    ViewState["CodSubAta"] = "";
                    BindDataUN(ViewState["CodSubAta"].ToString());
                    BindDataPsc("");
                    BindDataPN("");
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdDatos.EditIndex = -1;
            BindData();
            ViewState["CodSubAta"] = "";
            BindDataUN(ViewState["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                string VBQuery;
                BindDataUN(ViewState["CodSubAta"].ToString());
                BindDataPN("");
                BindDataPsc("");
                if (ViewState["CodSubAta"].ToString() == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Sub-Ata')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 19,@CSuAt,@CHK,'','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@CSuAt", ViewState["CodSubAta"].ToString().Trim());
                    Comando.Parameters.AddWithValue("@CHK", DdlFlota.Text);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        ViewState["CodSubAta"] = "";
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
                    ViewState["CodSubAta"] = "";
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }/**/
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (imgE != null)
                {
                    DataRow[] Result3 = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result3)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/
                }
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + e.Row.RowIndex);
                DataRow[] Result = Idioma.Select("Objeto= 'SelecReg'");
                foreach (DataRow row in Result)
                { e.Row.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData();
            ViewState["CodSubAta"] = "";
            BindDataUN(ViewState["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];

            ViewState["CodSubAta"] = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][1].ToString();
            BindDataUN(ViewState["CodSubAta"].ToString());
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
                    DataRow[] Result = Idioma.Select("Objeto= 'SelecReg'");
                    foreach (DataRow row in Result)
                    { Row.ToolTip = row["Texto"].ToString().Trim(); }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdDatos, "Select$" + Row.RowIndex);
                }
            }
            PerfilesGrid();
        }
        protected void GrdUltNvl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (ViewState["CodSubAta"].ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr06'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar una SubATA')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr02'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VblCodPpal, VBQuery;
                    VblCodPpal = (GrdUltNvl.FooterRow.FindControl("TxtCodSubN4PP") as TextBox).Text.Trim();
                    if (VblCodPpal.Length < 2)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr03'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //El codigo debe tener 2 dígitos')", true);
                        return;
                    }
                    VblCodPpal = ViewState["CodSubAta"].ToString() + VblCodPpal;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "SELECT * FROM TblSubCapituloN4 WHERE CodSubCapituloN4='" + VblCodPpal + "' AND CodModelo='" + DdlFlota.SelectedValue + "'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr04'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //El código existe')", true);
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
                        sqlCmd.Parameters.AddWithValue("@CodSubAta", ViewState["CodSubAta"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", (GrdUltNvl.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@NumElem", VbNumelem.ToString());
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindDataUN(ViewState["CodSubAta"].ToString());
                        BindDataPsc("");
                        BindDataPN("");
                    }
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT Ultimo Nivel", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdUltNvl.EditIndex = e.NewEditIndex;
            BindDataUN(ViewState["CodSubAta"].ToString()); ;
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
                    BindDataUN(ViewState["CodSubAta"].ToString());
                    BindDataPsc("");
                    BindDataPN("");
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "Update", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdUltNvl.EditIndex = -1;
            BindDataUN(ViewState["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdUltNvl_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                BindDataPN(ViewState["CodUNPM"].ToString());
                BindDataPsc(ViewState["CodUNPM"].ToString());
                if (ViewState["CodUNPM"].ToString() == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una Ubicación técnica')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 18,'" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        ViewState["CodUNPM"] = "";
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
                    BindDataUN(ViewState["CodSubAta"].ToString());
                    ViewState["CodUNPM"] = "";
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Ultimo Nivel", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdUltNvl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }/**/
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = (e.Row.FindControl("IbtEdit") as ImageButton);
                if (imgE != null)
                {
                    DataRow[] Result3 = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result3)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/
                }
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdUltNvl, "Select$" + e.Row.RowIndex);
                DataRow[] Result = Idioma.Select("Objeto= 'SelecReg'");
                foreach (DataRow row in Result)
                { e.Row.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        protected void GrdUltNvl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdUltNvl.PageIndex = e.NewPageIndex;
            BindDataUN(ViewState["CodSubAta"].ToString());
            BindDataPsc("");
            BindDataPN("");
        }
        protected void GrdUltNvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["CodUNPM"] = GrdUltNvl.DataKeys[this.GrdUltNvl.SelectedIndex][1].ToString();
            Session["NumElement"] = GrdUltNvl.DataKeys[this.GrdUltNvl.SelectedIndex][2].ToString();
            BindDataPsc(ViewState["CodUNPM"].ToString());
            BindDataPN(ViewState["CodUNPM"].ToString());

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
                    DataRow[] Result = Idioma.Select("Objeto= 'SelecReg'");
                    foreach (DataRow row in Result)
                    { Row.ToolTip = row["Texto"].ToString().Trim(); }
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
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (ViewState["CodUNPM"].ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr09'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar una Ubicación técnica')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr02'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VBQuery;
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 13,'" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','',''," + Session["NumElement"].ToString() + ",0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr10'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //La cantidad de posiciones superá el número de elementos')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 12,@CodUN, @Mod,'','',@IdPsc,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@CodUN", ViewState["CodUNPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@IdPsc", (GrdPosicion.FooterRow.FindControl("DdlPscPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.ExecuteNonQuery();
                        BindDataPN(ViewState["CodUNPM"].ToString());
                        BindDataPsc(ViewState["CodUNPM"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPosicion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VbPosicion = GrdPosicion.DataKeys[e.RowIndex].Values["Codigo"].ToString();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 14,'" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','" + VbPosicion + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);

                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr11'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //La posición se encuentra asignada a un elemento')", true);
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
                    BindDataPN(ViewState["CodUNPM"].ToString());
                    BindDataPsc(ViewState["CodUNPM"].ToString());
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPosicion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 11,'" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPscPP = (e.Row.FindControl("DdlPscPP") as DropDownList);
                DdlPscPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPscPP.DataTextField = "Descripcion";
                DdlPscPP.DataValueField = "IdPosicion";
                DdlPscPP.DataBind();
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }/**/
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/
                }
            }
        }
        protected void GrdPn_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    if (ViewState["CodUNPM"].ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr09'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar una Ubicación técnica')", true);
                        return;
                    }
                    if (DdlFlota.SelectedValue.ToString() == string.Empty)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr02'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar un modelo')", true);
                        return;
                    }
                    string VBQuery, VbNivelSuper;
                    VbNivelSuper = DdlFlota.SelectedValue + "-" + ViewState["ATAPM"].ToString() + "-" + ViewState["CodSubAta"].ToString() + "-" + ViewState["CodUNPM"].ToString();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_TablasPlantillaM 5,@NivelSuper, @Ref, @CodUN, @Mod, @VbC77U,'','','','',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@CodUN", ViewState["CodUNPM"].ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", DdlFlota.SelectedValue);
                        sqlCmd.Parameters.AddWithValue("@Ref", (GrdPn.FooterRow.FindControl("DdlPnPP") as DropDownList).SelectedValue.Trim());
                        sqlCmd.Parameters.AddWithValue("@NivelSuper", VbNivelSuper);
                        sqlCmd.Parameters.AddWithValue("@VbC77U", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindDataPN(ViewState["CodUNPM"].ToString());
                        BindDataPsc(ViewState["CodUNPM"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "INSERT posición", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPn_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VbReferenc = GrdPn.DataKeys[e.RowIndex].Values["CodReferencia"].ToString();
                    VBQuery = "EXEC SP_PANTALLA_Capitulo_PM 16,'" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','" + VbReferenc + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensPlaMatr12'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPn2, UpPn2.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //La referencia del parte se encuenta instalada en una aeroanve')", true);
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
                    BindDataPN(ViewState["CodUNPM"].ToString());
                    BindDataPsc(ViewState["CodUNPM"].ToString());
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPlantillaMaestra", "DELETE Referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string LtxtSql = "EXEC SP_PANTALLA_Capitulo_PM 3,'" + ViewState["ATAPM"].ToString() + "','" + ViewState["CodUNPM"].ToString() + "','" + DdlFlota.SelectedValue + "','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPnPP = (e.Row.FindControl("DdlPnPP") as DropDownList);
                DdlPnPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPnPP.DataTextField = "Pn";
                DdlPnPP.DataValueField = "CodReferencia";
                DdlPnPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/
                }
            }
        }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitExporPM'");
            foreach (DataRow row in Result)
            { VbNomRpt = row["Texto"].ToString().Trim(); }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurPLantillaMaestraExportar", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 3,'',@CodMod,'','CurPLantillaMaestraExportar',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    SC.Parameters.AddWithValue("@CodMod", DdlFlota.Text.Trim());
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
    }
}