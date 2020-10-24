using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data.SqlClient;
using System.Configuration;

namespace _77NeoWeb
{
    public partial class FrmAcceso : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("Acceso");
           
            Session["Login77"] = null;
            if (!IsPostBack)
            {
                string LtxtSql = "EXEC SP_ACCESO_WEB 1,'','','','','',0, 0,0,0,'01-01-1','01-01-1'";
                Session["D[BX"] = "";
                Session["Nit77Cia"] = "";
                Session["$VR"] = "";
                Session["V$U@"] = "";
                Session["P@$"] = "";
                Session["NomCiaPpal"] = "";
                DdlNit.DataSource = Cnx.DSET(LtxtSql);
                DdlNit.DataMember = "Datos";
                DdlNit.DataTextField = "Nit";
                DdlNit.DataValueField = "CodNit";
                DdlNit.DataBind();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddlP();</script>", false);
        }
        protected void TbnIngresar_Click(object sender, EventArgs e)
        {
            string LtxtSql, VbUsu, VbPassCia;
            if (TbnIngresar.Text.Equals("Iniciar sesión"))
            {
                VbUsu = TxtUsuario.Text;
                while (VbUsu.Contains(" "))
                {
                    VbUsu = VbUsu.Replace(" ", "");
                }
                TxtUsuario.Text = VbUsu;
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
                {
                    LtxtSql = "EXEC SP_Configuracion  1,@P1,@P3,'','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                    SC.Parameters.AddWithValue("@P1", Session["SigCiaPpal"].ToString());
                    SC.Parameters.AddWithValue("@P3", DdlBD.SelectedValue);
                    sqlCon.Open();
                    SqlDataReader tbl = SC.ExecuteReader();
                    if (tbl.Read())
                    {
                        Session["D[BX"] = tbl["NomDB"].ToString();
                        Session["Nit77Cia"] = DdlNit.SelectedValue;
                        Session["$VR"] = tbl["NomSrvdr"].ToString();                       
                        Session["V$U@"] = tbl["UsuSA"].ToString();
                        Session["P@$"] = tbl["Clve"].ToString();
                        Session["NomCiaPpal"] = tbl["RazonSocial"].ToString();
                        Session["SigCia"] = tbl["SiglaCia"].ToString();
                        Session["LogoPpal"] = tbl["Logo"].ToString();
                        Session["77IDM"] = tbl["Logo"].ToString(); //Idiioma
                    }
                    else
                    {
                        Session["D[BX"] = "";
                        return;
                    }
                }              
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    //LtxtSql = " EXEC SP_ConfiguracionV2_ 2,'" + TxtUsuario.Text + "','" + TxtClave.Text + "','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    LtxtSql = " EXEC SP_ConfiguracionV2_ 2,@H77,@H775,'','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";

                    SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                    Comando.Parameters.AddWithValue("@H77", TxtUsuario.Text);
                    Comando.Parameters.AddWithValue("@H775", TxtClave.Text);
                    sqlCon.Open();
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        Session["Login77"] = TxtUsuario.Text;
                        Session["C77U"] = registro["CodUsuario"].ToString();
                        Session["N77U"] = registro["Usuario"].ToString();
                        Session["CodTipoCodigoInicial"]= registro["CodTipoCodigo"].ToString();
                        Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Datos inválidos o usuario inactivo')", true);
                    }
                }
            }
            else
            {
                VbPassCia = TxtPassEmsa.Text;
                while (VbPassCia.Contains(" "))
                {
                    VbPassCia = VbPassCia.Replace(" ", "");
                }
                TxtPassEmsa.Text = VbPassCia;
                //Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
                {
                    LtxtSql = "EXEC SP_ACCESO_WEB 2,@E71,@E59,'','','',0, 0,0,0,'01-01-1','01-01-1'";
                    SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                    SC.Parameters.AddWithValue("@E71", DdlNit.SelectedValue);
                    SC.Parameters.AddWithValue("@E59", TxtPassEmsa.Text);
                    sqlCon.Open();
                    SqlDataReader tbl = SC.ExecuteReader();
                    if (tbl.Read())
                    {
                        Session["SigCiaPpal"] = tbl["SiglaCiaPpal"].ToString();
                        DdlBD.Visible = true;
                        LtxtSql = "EXEC SP_Configuracion  1,'" + Session["SigCiaPpal"].ToString() + "','','','','DropDown',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        DdlBD.DataSource = Cnx.DSET(LtxtSql);
                        DdlBD.DataMember = "Datos";
                        DdlBD.DataTextField = "RazonSocial";
                        DdlBD.DataValueField = "NomDB";
                        DdlBD.DataBind();

                        TbnIngresar.Text = "Iniciar sesión";
                        DdlNit.Enabled = false;
                        TxtUsuario.Visible = true;
                        TxtClave.Visible = true;
                        TxtPassEmsa.Enabled = false;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Clave de la compañía inválida')", true);
                    }
                    sqlCon.Close();
                }
            }
        }
        protected void DdlNit_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlConx = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
            {
                Session["Login77"] = null;
                Session["C77U"] = "";
                Session["N77U"] = "";
                TbnIngresar.Text = "Validar compañía";
                //DdlBD.SelectedValue = "";
                DdlBD.Visible = false;
                TxtClave.Visible = false;
                TxtUsuario.Visible = false;
                // TxtPassEmsa.Text = "";               
                TxtUsuario.Text = "";
                TxtClave.Text = "";
            }
        }    }
}
