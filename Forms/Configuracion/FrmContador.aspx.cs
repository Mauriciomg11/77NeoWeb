using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Forms.Configuracion
{
    public partial class FrmContador : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        string VbCont, VbDescCn, VbUMCnt, VbIdenCnt;
        int VbResetCnt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Contador");
            if (Session["C77U"] == null)
            {
                Session["VldrCntdr"] = "S";
                Session["C77U"] = "";/* */
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp"; */
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Contadores";
                ModSeguridad();
                ActivarCampos(false, false);
                BindDataDdlCntr();
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
            ClsP.Acceder(Session["C77U"].ToString(), "FrmContador.aspx");

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
        void BindDataDdlCntr()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Tipo_contador 4,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                DdlBuscar.DataSource = Cnx.DSET(LtxtSql);
                DdlBuscar.DataMember = "Datos";
                DdlBuscar.DataTextField = "Descripcion";
                DdlBuscar.DataValueField = "CodContador";
                DdlBuscar.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_Tipo_contador 5,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                DdlUndMed.DataSource = Cnx.DSET(LtxtSql);
                DdlUndMed.DataMember = "Datos";
                DdlUndMed.DataTextField = "Descripcion";
                DdlUndMed.DataValueField = "CodUnidMedida";
                DdlUndMed.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_Tipo_contador 6,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                DdlIdent.DataSource = Cnx.DSET(LtxtSql);
                DdlIdent.DataMember = "Datos";
                DdlIdent.DataTextField = "Descripcion";
                DdlIdent.DataValueField = "IdentificadorC";
                DdlIdent.DataBind();
            }
        }
        void LimpiarCampos()
        {
            TxtCod.Text = "";
            TxtDesc.Text = "";
            DdlUndMed.Text = "";
            DdlIdent.Text = "";
            CkReset.Checked = false;
        }
        void ActivarCampos(bool Ing, bool Edi)
        {
            TxtCod.Enabled = Ing;
            TxtDesc.Enabled = Edi;
            DdlUndMed.Enabled = Edi;
            DdlIdent.Enabled = Edi;
            CkReset.Enabled = Edi;
        }
        void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Fml)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            IbtExpExcel.Enabled = Ip;
        }
        void AsignarValores()
        {
            Session["VldrCntdr"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar un código')", true);
                Session["VldrCntdr"] = "N";
                return;
            }
            if (DdlUndMed.Text == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar una unidad de medida')", true);
                Session["VldrCntdr"] = "N";
                return;
            }
            if (DdlIdent.Text == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar un identificador')", true);
                Session["VldrCntdr"] = "N";
                return;
            }
            VbCont = TxtCod.Text.Trim();
            VbDescCn = TxtDesc.Text.Trim();
            VbUMCnt = DdlUndMed.SelectedValue;
            VbIdenCnt = DdlIdent.SelectedValue;
            VbResetCnt = 0;
            if (CkReset.Checked == true)
            {
                VbResetCnt = 1;
            }
        }
        protected void DdlBuscar_TextChanged(object sender, EventArgs e)
        {
            BusqNewReg(DdlBuscar.SelectedValue);
            ActivarBotones(true, true, true, true, true);
        }
        void BusqNewReg(string VbNewCod)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                while (VbNewCod.Contains(" "))
                {
                    VbNewCod = VbNewCod.Replace(" ", string.Empty);

                }

                string LtxtSql = "EXEC SP_PANTALLA_Tipo_contador 7,'" + VbNewCod + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                sqlCon.Open();
                SqlDataReader tbl = Comando.ExecuteReader();
                if (tbl.Read())
                {
                    TxtCod.Text = tbl["CodContador"].ToString();
                    TxtDesc.Text = tbl["Nombre"].ToString();
                    DdlUndMed.Text = tbl["CodUnidMedida"].ToString();
                    DdlIdent.Text = tbl["identificador"].ToString();
                    CkReset.Checked = false;
                    if (Convert.ToInt32(tbl["Reseteable"].ToString()) == 1)
                    {
                        CkReset.Checked = true;
                    }
                }
            }
        }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            string VbTitul, VbTxtToolT, TxtCad, TxtPantIni, NomArc;
            VbTitul = "Exportar Contadores";
            VbTxtToolT = "Ingrese el contador a colsultar";
            TxtPantIni = "~/Forms/Configuracion/FrmContador.aspx";
           // TxtCad = "EXEC SP_PANTALLA_Informe_Ingenieria 3,'{0}','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            TxtCad = "EXEC SP_PANTALLA_Tipo_contador 8,'{0}','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            NomArc = "Contador";
            Response.Redirect("~/Forms/FrmExportar.aspx?TT=" + VbTitul + "&ToolT=" + VbTxtToolT + "&NomArch=" + NomArc + "&TCDN=" + TxtCad + "&PantI=" + TxtPantIni);
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            string Txtsql;
            //BindData(TxtBusqueda.Text);
            if (BtnIngresar.Text == "Ingresar")
            {
                DdlBuscar.Enabled = false;
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
                    if (Session["VldrCntdr"].ToString() == "N")
                    {
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string TxQry = "EXEC SP_Pantalla_Parametros 1,'" + VbCont + "','','C','CodContador','TblContador',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                        Txtsql = "EXEC SP_TablasPlantillaM 8,@Cod,@Desc, @UM, @Idnt, @UsuC,'','','','',@Rst,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	";
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCont.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDescCn.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@UM", VbUMCnt.ToString());
                        sqlCmd.Parameters.AddWithValue("@Idnt", VbIdenCnt);
                        sqlCmd.Parameters.AddWithValue("@Rst", VbResetCnt);

                        sqlCmd.ExecuteNonQuery();
                        LimpiarCampos();
                        BtnIngresar.Text = "Ingresar";
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, false);
                        DdlBuscar.Enabled = true;
                        BtnIngresar.OnClientClick = "";
                        BindDataDdlCntr();
                    }
                    BusqNewReg(VbCont);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso de los dato')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmContador", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                DdlBuscar.Enabled = false;
                BtnModificar.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
            }
            else
            {
                try
                {
                    AsignarValores();
                    if (Session["VldrCntdr"].ToString() == "N")
                    {
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string Txtsql = "EXEC SP_TablasPlantillaM 9,@Cod,@Desc, @UM, @Idnt, @UsuC,'','','','',@Rst,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCont.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDescCn.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@UM", VbUMCnt.ToString());
                        sqlCmd.Parameters.AddWithValue("@Idnt", VbIdenCnt);
                        sqlCmd.Parameters.AddWithValue("@Rst", VbResetCnt);
                        sqlCmd.ExecuteNonQuery();
                        BtnModificar.Text = "Modificar";
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, false);
                        DdlBuscar.Enabled = true;
                        BtnModificar.OnClientClick = "";
                        LimpiarCampos();
                        BindDataDdlCntr();
                    }
                    BusqNewReg(VbCont);
                }

                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en la edición de los datos')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmContador", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                
                string VBQuery, VbCod;
                VbCod = TxtCod.Text;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_Pantalla_Parametros 11,'" + VbCod + "','','','','VALIDA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "EXEC SP_Pantalla_Parametros 11, " + VbCod + ",'','','','ELIMINA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);                   
                    sqlCmd.ExecuteNonQuery();
                    LimpiarCampos();
                    ActivarBotones(true, false, false, false, false);
                    BindDataDdlCntr();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmTipoAeronave", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
    }
}