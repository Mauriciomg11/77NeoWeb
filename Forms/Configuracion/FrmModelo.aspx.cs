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

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmModelo : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        string VbCod, VbNom, VbDes;
        int VbNumMot, VbNumTr, VbPasj;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Modelos");
            if (Session["C77U"] == null)
            {
                Session["ValdrMdl"] = "S";
                Session["C77U"] = "";/**/
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1"; */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Modelos";
                ModSeguridad();
                ActivarCampos(false, false);
                BindData(TxtBusqueda.Text);
                TxtBusqueda.ToolTip = "Modelo";
            }
        }
        void ModSeguridad()
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
        }
        void BindData(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Pantalla_Parametros 3,'" + VbConsultar + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                GrdDatos.DataSource = null;
                GrdDatos.DataBind();               
            }
        }
        void LimpiarCampos()
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
        void ActivarCampos(bool Ing, bool Edi)
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
        void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Fml)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            //BtnImprimir.Enabled = Ip;
            BtIFormL.Enabled = Fml;
        }
        void AsignarValores()
        {
            Session["ValdrMdl"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar un código')", true);
                Session["ValdrMdl"] = "N";
                return;
            }
            if (TxtMod.Text == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar un modelo')", true);
                Session["ValdrMdl"] = "N";
                return;
            }
            VbCod = TxtCod.Text.Trim();
            VbNom = TxtMod.Text.Trim();
            VbDes = TxtDesc.Text.Trim();
            VbNumMot = Convert.ToInt32(TxtNumMot.Text);
            VbNumTr = Convert.ToInt32(TxtNumTr.Text);
            VbPasj = Convert.ToInt32(TxtPasj.Text);
        }
        void Formula(string Frml, string NewVlr)
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
                try
                {
                    sqlCon.Open();
                    string Txtsql = "UPDATE TblModelo SET FormulaLevante=@Formula, UsuMod = @UsuC, FechaMod= GETDATE() WHERE CodModelo = @Cod";
                    SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Cod", TxtCod.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Formula", TxtNewFml.Text.ToString());
                    sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindData(TxtBusqueda.Text);
                    TxtFormL.Text = TxtNewFml.Text;
                    PnlDatos.Visible = true;
                    PnlFrml.Visible = false;

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en la edición de la fórmula')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "UPDATE FORMULA", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
            string Txtsql;
            int VbTipo;
            if (BtnIngresar.Text == "Ingresar")
            {
                ActivarBotones(true, false, false, false, false);
                BtnIngresar.Text = "Aceptar";
                ActivarCampos(true, true);
                LimpiarCampos();
                BtnIngresar.OnClientClick = "return confirm('¿Desea realizar el ingreso?');";
            }
            else
            {
                try
                {
                    AsignarValores();
                    VbTipo = RdbAlaF.Checked== true ? 1 : 2;
                    if (Session["ValdrMdl"].ToString() == "N")
                    {
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string TxQry = "EXEC SP_Pantalla_Parametros 1,'" + VbCod + "','','C','CodModelo','TblModelo',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                        sqlCon.Open();
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El código ya se encuentra asignado')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        Txtsql = string.Format("EXEC SP_Pantalla_Parametros 4,@Cod,@Mod,@Desc,@UsuC,'',@NroM,@NroT,@NroP,{0},'01-01-1','02-01-1','03-01-1'", VbTipo);
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCod.ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", VbNom.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDes.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@NroM", VbNumMot);
                        sqlCmd.Parameters.AddWithValue("@NroT", VbNumTr);
                        sqlCmd.Parameters.AddWithValue("@NroP", VbPasj);
                        sqlCmd.ExecuteNonQuery();
                        LimpiarCampos();
                        BtnIngresar.Text = "Ingresar";
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, true);
                        BtnIngresar.OnClientClick = "";
                        BindData(TxtBusqueda.Text);
                    }
                }
                catch (Exception ex)
                {                  
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso de los dato')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            if (BtnModificar.Text == "Modificar")
            {
                ActivarBotones(false, true, false, false, false);
                BtnModificar.Text = "Aceptar";
                ActivarCampos(false, true);
                BtnModificar.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
            }
            else
            {
                try
                {
                    AsignarValores();
                    int VbTipo = RdbAlaF.Checked == true ? 1 : 2;
                    if (Session["ValdrMdl"].ToString() == "N")
                    {
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string datoGrid = "EXEC SP_Pantalla_Parametros 1,'" + VbCod + "','" + VbCod + "','C','CodModelo','TblModelo',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand Comando = new SqlCommand(datoGrid, sqlCon);
                        sqlCon.Open();
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('El código ya se encuentra asignado')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string Txtsql = string.Format("EXEC SP_Pantalla_Parametros 5,@Cod,@Mod,@Desc,@UsuC,'',@NroM,@NroT,@NroP,{0},'01-01-1','02-01-1','03-01-1'", VbTipo);
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCod.ToString());
                        sqlCmd.Parameters.AddWithValue("@Mod", VbNom.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDes.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@NroM", VbNumMot);
                        sqlCmd.Parameters.AddWithValue("@NroT", VbNumTr);
                        sqlCmd.Parameters.AddWithValue("@NroP", VbPasj);
                        sqlCmd.ExecuteNonQuery();                        
                        BtnModificar.Text = "Modificar";
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, false);
                        BtnModificar.OnClientClick = "";
                        LimpiarCampos();
                        BindData(TxtBusqueda.Text);
                    }
                }

                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en la edición de los datos')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            AsignarValores();
            if (Session["ValdrMdl"].ToString() == "N")
            {
                return;
            }
            string Txtsql = "EXEC SP_Pantalla_Parametros 6,'" + VbCod + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            try
            {
                Cnx.Cosultar(Txtsql);
                LimpiarCampos();
                ActivarBotones(true, false, false, false, false);
                BindData(TxtBusqueda.Text);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en la eliminación de los datos')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmModelo", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtIConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BindData(TxtBusqueda.Text);
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtCod.Text = GrdDatos.DataKeys[this.GrdDatos.SelectedIndex][0].ToString();
            TxtMod.Text = GrdDatos.SelectedRow.Cells[2].Text;
            TxtDesc.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[3].Text);
            TxtNumMot.Text = GrdDatos.SelectedRow.Cells[4].Text;
            TxtNumTr.Text = GrdDatos.SelectedRow.Cells[5].Text;
            TxtPasj.Text = GrdDatos.SelectedRow.Cells[6].Text;
            TxtFormL.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[7].Text);
            RdbAlaF.Checked= HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[8].Text.Trim()).Equals("S")?true:false;
            RdbAlaRo.Checked = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[9].Text.Trim()).Equals("S") ? true : false;
            ActivarBotones(true, true, true, true, true);
            BtnModificar.Text = "Modificar";
            BtnIngresar.Text = "Ingresar";
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