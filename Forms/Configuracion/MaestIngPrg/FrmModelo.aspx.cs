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
using DocumentFormat.OpenXml.Presentation;
using System.Configuration;

namespace _77NeoWeb.Forms.Configuracion.MaestIngPrg
{
    public partial class FrmModelo : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTDet = new DataTable();
        string VbCod, VbNom, VbDes;
        int VbNumMot, VbNumTr, VbPasj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Modelos");

            if (Session["C77U"] == null)
            {
                Session["ValdrMdl"] = "S";
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
                ViewState["Accion"] = "";
                TitForm.Text = "Configuración de Modelos";
                ModSeguridad();
                ActivarCampos(false, false);
                BindData(TxtBusqueda.Text, "UPD");
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmModelo.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                BtnIngresar.Visible = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnModificar.Visible = false;
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
                BtnEliminar.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {

            }
            if (ClsP.GetCE2() == 0)
            {
                BtIFormL.Visible = false;
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

                    TitForm.Text = bO.Equals("TituloMod") ? bT : TitForm.Text;
                    LblBusq.Text = bO.Equals("LblBusq") ? bT + ":" : LblBusq.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("placeholderSinFrm"))
                    { TxtFormL.Attributes.Add("placeholder", bT); }
                    BtIConsultar.ToolTip = bO.Equals("BtIConsultar") ? bT : BtIConsultar.ToolTip;
                    LblCodigo.Text = bO.Equals("LblCodigo") ? bT + ":" : LblCodigo.Text;
                    LblMod.Text = bO.Equals("LblMod") ? bT + ":" : LblMod.Text;
                    LblDesc.Text = bO.Equals("LblDesc") ? bT + ":" : LblDesc.Text;
                    LblNumMot.Text = bO.Equals("LblNumMot") ? bT + ":" : LblNumMot.Text;
                    LblNumTr.Text = bO.Equals("LblNumTr") ? bT + ":" : LblNumTr.Text;
                    LblPasj.Text = bO.Equals("LblPasj") ? bT + ":" : LblPasj.Text;
                    LblFormL.Text = bO.Equals("LblFormL") ? bT + ":" : LblFormL.Text;
                    BtIFormL.ToolTip = bO.Equals("BtIFormL") ? bT : BtIFormL.ToolTip;
                    LblAlaF.Text = bO.Equals("LblAlaF") ? bT : LblAlaF.Text;
                    LblAlaR.Text = bO.Equals("LblAlaR") ? bT : LblAlaR.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    LblFormula.Text = bO.Equals("LblFormula") ? bT : LblFormula.Text;
                    BtnLimp.Text = bO.Equals("BtnLimp") ? bT : BtnLimp.Text;
                    BtnCiclo.ToolTip = bO.Equals("BtnCiclo") ? bT : BtnCiclo.ToolTip;
                    BtnLevant.ToolTip = bO.Equals("BtnLevant") ? bT : BtnLevant.ToolTip;
                    BtiAceptar.ToolTip = bO.Equals("BtiAceptar") ? bT : BtiAceptar.ToolTip;
                    BtiCancelar.ToolTip = bO.Equals("BtiCancelar") ? bT : BtiCancelar.ToolTip;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdCodMod") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("LblMod") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("LblDesc") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("LblNumMot") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("LblNumTr") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("LblPasj") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("LblFormL") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("LblAlaF") ? bT : GrdDatos.Columns[8].HeaderText;
                    GrdDatos.Columns[9].HeaderText = bO.Equals("LblAlaR") ? bT : GrdDatos.Columns[9].HeaderText;
                    GrdDatos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDatos.EmptyDataText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindData(string VbConsultar, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DT = new DataTable();
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_Pantalla_Parametros 3,'','','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            DTDet = (DataTable)ViewState["DTDet"];
            DT = DTDet.Clone();
            Result = DTDet.Select("Modelo LIKE '%" + VbConsultar + "%'");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "CodModelo";
                DT = DV.ToTable();
                GrdDatos.DataSource = DT;
                GrdDatos.DataBind();
                string VblLbl = "";
                Result = Idioma.Select("Objeto= 'GrdActiv'");
                foreach (DataRow row in Result)
                { VblLbl = row["Texto"].ToString().Trim(); }

                foreach (GridViewRow row in GrdDatos.Rows)
                { LinkButton lb = (LinkButton)row.Cells[0].Controls[0]; lb.Text = VblLbl; }
            }
            else
            {
                GrdDatos.DataSource = null;
                GrdDatos.DataBind();
            }
        }
        protected void LimpiarCampos()
        {
            TxtCod.Text = "";
            TxtMod.Text = "";
            TxtDesc.Text = "";
            TxtNumMot.Text = 0.ToString();
            TxtNumTr.Text = 0.ToString();
            TxtPasj.Text = 0.ToString();
            TxtFormL.Text = "";
            RdbAlaF.Checked = true;
            RdbAlaRo.Checked = false;
        }
        protected void ActivarCampos(bool Ing, bool Edi)
        {
            TxtCod.Enabled = Ing;
            TxtMod.Enabled = Edi;
            TxtDesc.Enabled = Edi;
            TxtNumMot.Enabled = Edi;
            TxtNumTr.Enabled = Edi;
            TxtPasj.Enabled = Edi;
            RdbAlaF.Enabled = Edi;
            RdbAlaRo.Enabled = Edi;
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Fml)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            //BtnImprimir.Enabled = Ip;
            BtIFormL.Enabled = Fml;
        }
        protected void AsignarValores()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Session["ValdrMdl"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Mdl'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un código')", true);
                Session["ValdrMdl"] = "N";
                TxtCod.Focus(); return;
            }
            if (TxtMod.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Mdl'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un modelo')", true);
                Session["ValdrMdl"] = "N";
                TxtMod.Focus(); return;
            }
            VbCod = TxtCod.Text.Trim();
            VbNom = TxtMod.Text.Trim();
            VbDes = TxtDesc.Text.Trim();
            VbNumMot = Convert.ToInt32(TxtNumMot.Text);
            VbNumTr = Convert.ToInt32(TxtNumTr.Text);
            VbPasj = Convert.ToInt32(TxtPasj.Text);
        }
        protected void Formula(string Frml, string NewVlr)
        {
            TxtNewFml.Text = Frml + NewVlr;
        }
        protected void BtIFormL_Click1(object sender, ImageClickEventArgs e)
        {
            TxtNewFml.Text = TxtFormL.Text.ToString();
            PnlDatos.Visible = false;
            PnlFrml.Visible = true;
        }
        protected void BtiAceptar_Click(object sender, ImageClickEventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VbQuery = "EXEC SP_Pantalla_Parametros 6,@Cod,@Formula,@UsuC,'','FORMULA',0,0,@ICC,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@Cod", TxtCod.Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Formula", TxtNewFml.Text.ToString());
                            sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindData(TxtBusqueda.Text, "UPD");
                            TxtFormL.Text = TxtNewFml.Text;
                            PnlDatos.Visible = true;
                            PnlFrml.Visible = false;

                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "UPDATE FORMULA", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void BtiCancelar_Click(object sender, ImageClickEventArgs e)
        {
            PnlDatos.Visible = true;
            PnlFrml.Visible = false;
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int VbTipo;
            if (ViewState["Accion"].ToString().Trim().Equals(""))
            {
                ActivarBotones(true, false, false, false, false);
                ViewState["Accion"] = "Aceptar";
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//Aceptar
                ActivarCampos(true, true);
                LimpiarCampos();
                Result = Idioma.Select("Objeto= 'MensConfIng'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?');";
            }
            else
            {
                string Mensj = "";
                AsignarValores();
                VbTipo = RdbAlaF.Checked == true ? 1 : 2;
                if (Session["ValdrMdl"].ToString() == "N") { return; }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasIngenieria 17,@Cod,@Mod,@Desc,@UsuC,'-CodAnt-','','','','INSERT',@NroM,@NroT,@NroP,@Tp,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Cod", VbCod.ToString());
                                sqlCmd.Parameters.AddWithValue("@Mod", VbNom.ToString());
                                sqlCmd.Parameters.AddWithValue("@Desc", VbDes.ToString());
                                sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@NroM", VbNumMot);
                                sqlCmd.Parameters.AddWithValue("@NroT", VbNumTr);
                                sqlCmd.Parameters.AddWithValue("@NroP", VbPasj);
                                sqlCmd.Parameters.AddWithValue("@Tp", VbTipo);
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
                                LimpiarCampos();
                                DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
                                foreach (DataRow row in Result)
                                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                                ViewState["Accion"] = "";
                                ActivarBotones(true, false, false, false, false);
                                ActivarCampos(false, true);
                                BtnIngresar.OnClientClick = "";
                                BindData(TxtBusqueda.Text, "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso de los dato')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            if (TxtCod.Text.Trim().Equals("")) { return; }
            if (ViewState["Accion"].ToString().Trim().Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Accion"] = "Aceptar";
                ActivarBotones(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                ActivarCampos(false, true);
                Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la actualización?');";
            }
            else
            {
                AsignarValores();
                int VbTipo = RdbAlaF.Checked == true ? 1 : 2;
                if (Session["ValdrMdl"].ToString() == "N") { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VbQuery = "EXEC SP_TablasIngenieria 17,@Cod,@Mod,@Desc,@UsuC, @Cod,'','','','UPDATE',@NroM,@NroT,@NroP,@Tp,0,@ICC,'01-01-1','02-01-1','03-01-1'";

                        using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Cod", VbCod.ToString());
                                sqlCmd.Parameters.AddWithValue("@Mod", VbNom.ToString());
                                sqlCmd.Parameters.AddWithValue("@Desc", VbDes.ToString());
                                sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@NroM", VbNumMot);
                                sqlCmd.Parameters.AddWithValue("@NroT", VbNumTr);
                                sqlCmd.Parameters.AddWithValue("@NroP", VbPasj);
                                sqlCmd.Parameters.AddWithValue("@Tp", VbTipo);
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                                foreach (DataRow row in Result)
                                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                                ViewState["Accion"] = "";
                                ActivarBotones(true, false, false, false, false);
                                ActivarCampos(false, false);
                                BtnModificar.OnClientClick = "";
                                LimpiarCampos();
                                BindData(TxtBusqueda.Text, "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la edición de los datos')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtCod.Text.Trim().Equals("")) { return; }
            AsignarValores();
            if (Session["ValdrMdl"].ToString() == "N") { return; }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VbQuery = "EXEC SP_Pantalla_Parametros 6,@id,'','','','DELETE',0,0,@ICC,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@id", TxtCod.Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            LimpiarCampos();
                            ActivarBotones(true, false, false, false, false);
                            BindData(TxtBusqueda.Text, "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación de los datos')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }

        }
        protected void BtIConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BindData(TxtBusqueda.Text, "SEL");
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            TxtCod.Text = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][0].ToString();
            GridViewRow Grow = GrdDatos.Rows[GrdDatos.SelectedIndex];
            TxtMod.Text = ((Label)Grow.FindControl("LblModelo")).Text.Trim();
            TxtDesc.Text = HttpUtility.HtmlDecode(((Label)Grow.FindControl("LblDescripcion")).Text.Trim());
            TxtNumMot.Text = ((Label)Grow.FindControl("LblNroMotor")).Text.Trim();
            TxtNumTr.Text = ((Label)Grow.FindControl("LblNroTri")).Text.Trim();
            TxtPasj.Text = ((Label)Grow.FindControl("LbNroPax")).Text.Trim();
            TxtFormL.Text = ((Label)Grow.FindControl("LblFormLv")).Text.Trim();
            RdbAlaF.Checked = ((Label)Grow.FindControl("LblAlaF")).Text.Trim().Equals("S") ? true : false;
            RdbAlaRo.Checked = ((Label)Grow.FindControl("LblAlaR")).Text.Trim().Equals("S") ? true : false;
            ActivarBotones(true, true, true, true, true);
            DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
            foreach (DataRow row in Result)
            { BtnModificar.Text = row["Texto"].ToString().Trim(); }//"Modificar";

            Result = Idioma.Select("Objeto= 'BotonIng'");
            foreach (DataRow row in Result)
            { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//Ingresar";
            ViewState["Accion"] = "";
            BtnIngresar.OnClientClick = "";
            BtnModificar.OnClientClick = "";
            ActivarCampos(false, false);

        }
        protected void BtnPA_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "(");
        }
        protected void BtnPC_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, ")");
        }
        protected void BtnMas_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "+");
        }
        protected void BtnMenos_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "-");
        }
        protected void BtnPor_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "*");
        }
        protected void BtnDiv_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "/");
        }
        protected void BtnCiclo_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "C");
        }
        protected void BtnLevant_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "L");
        }
        protected void Btn1_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "1");
        }
        protected void Btn2_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "2");
        }
        protected void Btn3_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "3");
        }
        protected void Btn4_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "4");
        }
        protected void Btn5_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "5");
        }
        protected void Btn6_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "6");
        }
        protected void Btn7_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "7");
        }
        protected void Btn8_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "8");
        }
        protected void Btn9_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "9");
        }
        protected void Btn0_Click(object sender, EventArgs e)
        {
            Formula(TxtNewFml.Text, "0");
        }
        protected void BtnLimp_Click(object sender, EventArgs e)
        {
            TxtNewFml.Text = TxtNewFml.Text.Substring(0, TxtNewFml.Text.Length - 1);
        }
    }
}