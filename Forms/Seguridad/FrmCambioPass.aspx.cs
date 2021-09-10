using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Seguridad
{
    public partial class FrmCambioPass : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
             if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
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
                Page.Title = "Password";
                TxtUsuario.Text = Session["Login77"].ToString().Trim();
                IdiomaControles();
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
                SC.Parameters.AddWithValue("@F1", "FrmAcceso");
                SC.Parameters.AddWithValue("@F2", "");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string b1 = tbl["Objeto"].ToString();
                    string b2 = tbl["Texto"].ToString();
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                    TbnIngresar.Text = b1.Trim().Equals("BtnAcceder") ? b2.Trim() : TbnIngresar.Text;
                    TitConfirmarC.Text = b1.Trim().Equals("TitConfirmarC") ? b2.Trim() : TitConfirmarC.Text;
                    if (b1.Trim().Equals("placeholderNew"))
                    { TxtNuevoPass.Attributes.Add("placeholder", b2.Trim()); }
                    if (b1.Trim().Equals("placeholderConf"))
                    { TxtConfirmarPass.Attributes.Add("placeholder", b2.Trim()); }
                    BtnCambioPass.Text = b1.Trim().Equals("BtnCambioPass") ? b2.Trim() : BtnCambioPass.Text;
                    TitForm.Text = b1.Trim().Equals("CaptionCambioPas") ? b2.Trim() : TitForm.Text;
                }
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void ValidarCampos()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtNuevoPass.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensAcc02'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                ViewState["Validar"] = "N"; TxtNuevoPass.Focus(); return;
            }
            if (TxtConfirmarPass.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensAcc03'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); } //Debe ingresar la confirmarción');", true);
                ViewState["Validar"] = "N"; TxtConfirmarPass.Focus(); return;
            }
            if (!TxtNuevoPass.Text.Trim().Equals(TxtConfirmarPass.Text))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensAcc04'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); } //Las contraseñas son diferentes');", true);
                ViewState["Validar"] = "N"; TxtNuevoPass.Focus(); return;
            }
        }
        protected void TbnIngresar_Click(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string LtxtSql = " EXEC SP_ConfiguracionV2_ 2,@H77,@H775,'','','',@PI,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                Comando.Parameters.AddWithValue("@H77", TxtUsuario.Text);
                Comando.Parameters.AddWithValue("@H775", TxtClave.Text);
                Comando.Parameters.AddWithValue("@PI", Session["77IDM"].ToString().Trim());
                Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlCon.Open();
                SqlDataReader registro = Comando.ExecuteReader();
                if (registro.Read())
                {
                    TbnIngresar.Visible = false;
                    TxtClave.Visible = false;
                    TxtNuevoPass.Visible = true;
                    TxtConfirmarPass.Visible = true;
                    BtnCambioPass.Visible = true;
                }
                else
                {
                    DataRow[] Result4 = Idioma.Select("Objeto= 'MensAcc01'");
                    foreach (DataRow row in Result4)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                }
            }
        }
        protected void BtnCambioPass_Click(object sender, EventArgs e)
        {
            ValidarCampos();
            if (ViewState["Validar"].Equals("N"))
            { return; }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_ConfiguracionV2_ 20,@H778,@H775,@H77,'','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                Comando.Parameters.AddWithValue("@H77", TxtUsuario.Text);
                Comando.Parameters.AddWithValue("@H775", TxtNuevoPass.Text);
                Comando.Parameters.AddWithValue("@H778", Session["C77U"]);
                sqlCon.Open();
                SqlDataReader registro = Comando.ExecuteReader();
                if (registro.Read())
                { }
                Response.Redirect("~/Forms/Seguridad/Frminicio.aspx");
            }
        }
    }
}