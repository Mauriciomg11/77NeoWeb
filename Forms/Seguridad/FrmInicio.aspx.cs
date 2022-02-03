using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            if (Session["C77U"] == null)
            {
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
                BindMenuControl();
                IdiomaControles();
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
            using (SqlConnection SCNX = new SqlConnection(Cnx.GetConex()))
            {
                SCNX.Open();
                string VblTxtSql = "EXEC SP_ConfiguracionV2_ 1,'', @Us,'','','',0,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                using (SqlCommand SC = new SqlCommand(VblTxtSql, SCNX))
                {
                    SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    DataSet DST = new DataSet();
                    DataTable DT = null;
                    try
                    {
                        SDA.Fill(DST);
                        DT = DST.Tables[0];
                        if (DT != null && DT.Rows.Count > 0)
                        {
                            foreach (DataRow DR in DT.Rows)
                            {
                                if (DR[0].ToString() == DR[2].ToString())
                                {
                                    MenuItem miMenuItem = new MenuItem(Convert.ToString(DR[1]), Convert.ToString(DR[0]), String.Empty, Convert.ToString(DR[7]));
                                    MyMenu.Items.Add(miMenuItem);
                                    AddChildItem(ref miMenuItem, DT);
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
                        SDA.Dispose();
                        DST.Dispose();
                        DT.Dispose();
                    }
                }
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

        protected void LkbPrueba_Click(object sender, EventArgs e)
        {

            string SP = "window.open('/WebPrueba1.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
    }
}