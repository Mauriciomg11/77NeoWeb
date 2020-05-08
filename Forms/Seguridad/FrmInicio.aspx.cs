using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace _77NeoWeb.Forms
{
    public partial class FrmInicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {               
                Response.Redirect("~/Forms/Seguridad/FrmAcceso.aspx");
            }
            if (!IsPostBack)
            {
                BindMenuControl();
            }
        }
        protected void BindMenuControl()
        {
            string VblTxtSql = "EXEC SP_ConfiguracionV2_ 1,'','" + Session["C77U"].ToString() + "','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            SqlConnection scSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString.ToString());
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
                            MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
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
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                    miMenuItem.ChildItems.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }
        protected void IbnSalir_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmAcceso.aspx");
        }
    }
}