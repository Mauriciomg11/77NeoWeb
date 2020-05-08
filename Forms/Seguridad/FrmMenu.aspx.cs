using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Forms
{
    public partial class FrmMenu : System.Web.UI.Page
    {
        string connectionP = ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["Login77"] == null)
            {               
                Response.Redirect("~/Forms/Seguridad/FrmAcceso.aspx");
            }*/
            if (Session["C77U"] == null)
            {
                Session["C77U"] = 0;
            }
            if (!IsPostBack)
            {
               // ModSeguridad();
                BindData();
            }
        }
        void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMenu.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("WebMenuInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
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
            }
            if (ClsP.GetCE1() == 0)
            {
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
        void BindData()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionP))
            {
                string VbTxtSql= "EXEC SP_ConfiguracionV2_ 3,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
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
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                GrdDatos.Rows[0].Cells[0].Text = "No existen registros ..!";
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbnRegresar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void GrdDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}