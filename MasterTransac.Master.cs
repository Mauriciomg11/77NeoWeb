using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;

namespace _77NeoWeb
{
    public partial class MasterTransac : System.Web.UI.MasterPage
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Response.Cache.SetCacheability(HttpCacheability.NoCache); // Evitar cache
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1)); // Expirar cache inmediatamente
            Response.Cache.SetNoStore(); // No guardar nada en el cache*/
            LblCia.Text = Session["SigCia"].ToString() + " - " + Session["N77U"].ToString();
            IdiomaControles();
            LoadMenu();
            if (Session["77IDM"].ToString() == "4")
            {
                LkbMenu.Text = "Menú";
                LkbCambPass.Text = "Cambio Contraseña...";
            }
            else {
                LkbMenu.Text = "Menu";
                LkbCambPass.Text = "Change Password";
            }
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
                SC.Parameters.AddWithValue("@F1", "0");
                SC.Parameters.AddWithValue("@F2", "");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {

                    string b1 = tbl["Objeto"].ToString();
                    string b2 = tbl["Texto"].ToString();
                    if (b1.Trim().Equals("IbnRegresarToolTip") || b1.Trim().Equals("IbnRegresarOnClick"))
                    {
                        Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                        // IbnRegresar.ToolTip = b2.Trim();
                    }
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbnRegresarOnClick'");
                //foreach (DataRow row in Result)
                //{
                //    IbnRegresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");
                //}

                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void IbnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
        }


        /// <summary>
        /// Metodo para traer el menu desde la base de datos
        ///    /// <param name="Us">codigo del usuario</param>
        /// <param name="ICC">Codigo de la empresa</param>
        /// </summary>
        private void LoadMenu()
        {

            Cnx.SelecBD();
            using (SqlConnection conn = new SqlConnection(Cnx.GetConex()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("EXEC SP_Menu @Us,@ICC,@Id", conn);
                cmd.Parameters.AddWithValue("@Us", Session["C77U"]);
                cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                cmd.Parameters.AddWithValue("@Id", Session["77IDM"]);
               
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable menuTable = new DataTable();
                menuTable.Load(reader);
                StringBuilder menuHtml = new StringBuilder();

                //menuHtml.Append("<div id='menu2' >"); 
                foreach (DataRow row in menuTable.Rows)
                {

                    if (row[0].ToString() == row[2].ToString())
                    {
                        menuHtml.Append("<ul class='list-group'>");
                        menuHtml.Append($"<li class='list-group-item'><a class='dropdown-toggle'  data-toggle='collapse' aria-expanded='false' href='{row["RutaWeb"]}' >{row["Descripcion"]}</a> ");

                        MenuItem miMenuItem = new MenuItem(Convert.ToString(row[1]), Convert.ToString(row[0]), string.Empty, Convert.ToString(row[6]));
                        //MyMenu.Items.Add(miMenuItem);
                        LoadSubMenu(ref miMenuItem, menuTable, menuHtml);
                        menuHtml.Append(" </li></ul>");
                    }

                }

                // menuHtml.Append("</div>");
                menuLiteral.Text = menuHtml.ToString();

            }
        }

        private void LoadSubMenu(ref MenuItem miMenuItem, DataTable menuTable, StringBuilder menuHtml)
        {
            // menuHtml.Append("<ul class='submenu'>");
            foreach (DataRow subItems in menuTable.Rows)
            {

                if (subItems[0].ToString() != subItems[2].ToString() && subItems[2].ToString() == miMenuItem.Value.ToString())
                {
                    menuHtml.Append("<ul class='collapse'>");
                    menuHtml.Append($"<li class='dropdown-toggle'><a class='dropdown-toggle'  href='{subItems[6]}' >{subItems[1]}</a></li>");
                    MenuItem menuChild = new MenuItem(subItems[1].ToString(), subItems[0].ToString(), string.Empty, Convert.ToString(subItems[6]));
                    menuChild.ChildItems.Add(menuChild);
                    LoadSubMenu(ref menuChild, menuTable, menuHtml);
                    menuHtml.Append("</ul>");
                }

            }
            // menuHtml.Append("</ul>");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void IbnSalir_Click(object sender, EventArgs e)
        {
            Session["Login77"] = null;
            Session["D[BX"] = "";
            Session["Nit77Cia"] = "";
            Session["$VR"] = "";
            // Session["V$U@"] = "";
            // Session["P@$"] = "";
            //   Session["SigCia"] = "";
            ///System.Web.Security.FormsAuthentication.SignOut();
            //   Session.Abandon();

            Response.Redirect("~/FrmAcceso.aspx");
        }
        protected void LkbCambPass_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmCambioPass.aspx");
        }
        protected void LkbMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FrmMenu.aspx");
        }

    }
}