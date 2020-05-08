using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data.SqlClient;
using System.Configuration;

namespace _77NeoWeb.Forms
{
    public partial class FrmAcceso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Login77"] = null;
        }

        protected void TbnIngresar_Click(object sender, EventArgs e)
        {
            ClsConexion cnx = new ClsConexion();
            string LtxtSql, VbUsu;
            VbUsu = TxtUsuario.Text;
            while (VbUsu.Contains(" "))
            {
                VbUsu = VbUsu.Replace(" ", "");
            }
            TxtUsuario.Text = VbUsu;
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString))
            {
                string datoGrid = " EXEC SP_ConfiguracionV2_ 2,'" + TxtUsuario.Text + "','" + TxtClave.Text + "','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";

                SqlCommand Comando = new SqlCommand(datoGrid, sqlCon);
                sqlCon.Open();

                SqlDataReader registro = Comando.ExecuteReader();
                if (registro.Read())
                {
                    Session["Login77"] = TxtUsuario.Text;
                    Session["C77U"] = registro["CodUsuario"].ToString();
                    Session["N77U"] = registro["Usuario"].ToString();
                    Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Datos inválidos o usuario inactivo')", true);
                }
                sqlCon.Close();
                
            }
        }
    }
}
