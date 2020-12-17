using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using _77NeoWeb.prg;

namespace _77NeoWeb.Forms
{
    public partial class FrmInicio : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("Inicio");
             if (Session["Login77"] == null)
             {
                 Response.Redirect("~/FrmAcceso.aspx");
             }/**/
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                 /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";*/
            }
            if (!IsPostBack)
            {
                BindMenuControl();
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
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                    IbnSalir.ToolTip = b1.Trim().Equals("IbnSalir") ? b2.Trim() : IbnSalir.ToolTip;      
                    LkbCambPass.Text = b1.Trim().Equals("LkbCambPass") ? b2.Trim() : LkbCambPass.Text;
                    LkbMenu.Text = b1.Trim().Equals("LkbMenu") ? b2.Trim() : LkbMenu.Text;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbnSalirOnClick'");
                foreach (DataRow row in Result)
                { IbnSalir.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                DataRow[] Result1 = Idioma.Select("Objeto= 'LkbCambPassOnClick'");
                foreach (DataRow row in Result1)
                { LkbCambPass.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/

                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindMenuControl()
        {
            Cnx.SelecBD();
            string VblTxtSql = "EXEC SP_ConfiguracionV2_ 1,'','" + Session["C77U"].ToString() + "','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            SqlConnection scSqlConnection = new SqlConnection(Cnx.GetConex());
            SqlCommand scSqlCommand = new SqlCommand(VblTxtSql, scSqlConnection);
            SqlDataAdapter sdaSqlDataAdapter = new SqlDataAdapter(scSqlCommand);
            DataSet dsDataSet = new DataSet();
            DataTable dtDataTable = null;
            try
            {
                scSqlConnection.Open();
                sdaSqlDataAdapter.Fill(dsDataSet);
                dtDataTable = dsDataSet.Tables[0];
                if (dtDataTable != null && dtDataTable.Rows.Count > 0)
                {
                    foreach (DataRow drDataRow in dtDataTable.Rows)
                    {
                        if (drDataRow[0].ToString() == drDataRow[2].ToString())
                        {
                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[7]));
                            MyMenu.Items.Add(miMenuItem);
                            AddChildItem(ref miMenuItem, dtDataTable);
                        }
                    }
                    MenuItem newMenuItem1 = new MenuItem("");
                    MyMenu.Items.Add(newMenuItem1);

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                scSqlConnection.Close();
                sdaSqlDataAdapter.Dispose();
                dsDataSet.Dispose();
                dtDataTable.Dispose();
            }
        }
        protected void AddChildItem(ref MenuItem miMenuItem, DataTable dtDataTable)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (drDataRow[2].ToString() == miMenuItem.Value.ToString() && drDataRow[0].ToString() != drDataRow[2].ToString())
                {
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[7]));
                    miMenuItem.ChildItems.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }
        protected void IbnSalir_Click(object sender, ImageClickEventArgs e)
        {
            Session["Login77"] = null;
            Session["D[BX"] = "";
            Session["Nit77Cia"] = "";
            Session["$VR"] = "";
            Session["V$U@"] = "";
            Session["P@$"] = "";
            Session["SigCia"] = "";
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("~/FrmAcceso.aspx");
        }
        protected void LkbCambPass_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmCambioPass.aspx");
        }
        protected void LkbMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmMenu.aspx");
        }
    }
}