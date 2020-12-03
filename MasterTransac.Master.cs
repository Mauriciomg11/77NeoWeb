using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb
{
    public partial class MasterTransac : System.Web.UI.MasterPage
    {
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            LblCia.Text = Session["SigCia"].ToString()+" - "+ Session["N77U"].ToString();
            IdiomaControles();
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
                    if (b1.Trim().Equals("IbnRegresarToolTip") || b1.Trim().Equals("IbnRegresarOnClick"))
                    {
                        Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                        IbnRegresar.ToolTip = b2.Trim();
                    }
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbnRegresarOnClick'");
                foreach (DataRow row in Result)
                { IbnRegresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void IbnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
        }
    }
}