using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Seguridad
{
    public partial class FrmIdioma : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable DTDet = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Idioma");
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
                ViewState["PageTit"] = "";
                TitForm.Text = "Idioma";

                using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
                {
                    DataTable DT = new DataTable();
                    string VbTxtSql = " EXEC SP_Configuracion 2,'','','','','DDL',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DT);
                        DdlForm.DataSource = DT;
                        DdlForm.DataTextField = "Nombre";
                        DdlForm.DataValueField = "IdFormulario";
                        DdlForm.DataBind();
                        DdlForm.Text = "-1";
                    }
                }
                BindData("UPD");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void BindData(string Accion)
        {
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
                {
                    string VbTxtSql = " EXEC SP_Configuracion 2,'','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            string StrCondic = "";
            string VbForm = "";
            string VbCorreg = "";
            if (!DdlForm.Text.Trim().Equals("-1"))
            { VbForm = " AND IdFormulario = " + DdlForm.Text.Trim(); }

            if (CkbSinCorr.Checked == true)
            { VbCorreg = " AND Aleman = ''"; }

            DTDet = (DataTable)ViewState["DTDet"];
            DataTable DT = new DataTable();
            DT = DTDet.Clone();
            if (RdbObj.Checked == true) { StrCondic = "Objeto LIKE '%" + TxtBusqueda.Text.Trim() + "%'" + VbForm + VbCorreg; }
            if (RdbDesc.Checked == true) { StrCondic = " Descripcion LIKE '%" + TxtBusqueda.Text.Trim() + "%'" + VbForm + VbCorreg; }
            Result = DTDet.Select(StrCondic);
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "IdFormulario,Descripcion";
                DT = DV.ToTable();
                GrdDatos.DataSource = DT;
                GrdDatos.DataBind();
            }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdDatos.DataSource = DT;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                { GrdDatos.Rows[0].Cells[0].Text = "Vacío"; }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData("SEL"); }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatos.EditIndex = e.NewEditIndex; BindData("SEL"); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_Configuracion 3, @Es,@En, @Rv,'','', @Id,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Es", (GrdDatos.Rows[e.RowIndex].FindControl("TxtEspa") as TextBox).Text.Trim());
                        SC.Parameters.AddWithValue("@En", (GrdDatos.Rows[e.RowIndex].FindControl("TxtIngl") as TextBox).Text.Trim());
                        SC.Parameters.AddWithValue("@Rv", (GrdDatos.Rows[e.RowIndex].FindControl("CkbRev") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@Id", GrdDatos.DataKeys[e.RowIndex].Values["CodIdFomularioUsr"].ToString());
                        try
                        {
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            GrdDatos.EditIndex = -1;
                            BindData("UPD");
                        }
                        catch (Exception)
                        { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData("SEL"); }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BindData("SEL"); }       
    }
}