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

namespace _77NeoWeb.Forms.Configuracion.ConfigManto
{
    public partial class FrmPrioridadManto : System.Web.UI.Page
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
            Page.Title = "XX";
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
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                ModSeguridad();
                BindData();
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0)
            { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            { ViewState["VblIngMS"] = 0; GrdDatos.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }

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
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"]);
                SC.Parameters.AddWithValue("@F2", "");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("Caption"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }

                    TitForm.Text = bO.Equals("Titulo") ? bT : TitForm.Text;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdCod") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdDesc") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdCrD") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdDI") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdDF") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdAct") ? bT : GrdDatos.Columns[5].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            /*
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[6].Controls.Remove(imgE);
                    }
                }
            }
            */
        }
        protected void BindData()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SP_TablasGeneral 7, '','','','','','','','PrioSol','SELECT',0,0,0,0,@Idm,0,'01-01-1','02-01-1','03-01-1'";
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0)
            { GrdDatos.DataSource = dtbl; GrdDatos.DataBind(); }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatos.EditIndex = e.NewEditIndex; BindData(); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbDesc, VBQuery;
            int VbCDia = 0, VbDIA = 0, VbDFA = 0;
            PerfilesGrid();
            VbDesc = (GrdDatos.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim();
            if (VbDesc == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens06'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción
                return;
            }
            if ((GrdDatos.Rows[e.RowIndex].FindControl("TxtCritD") as TextBox).Text.Trim().Equals("")) { VbCDia = 0; }
            else { VbCDia = Convert.ToInt32((GrdDatos.Rows[e.RowIndex].FindControl("TxtCritD") as TextBox).Text.Trim()); }
            if (VbCDia < 0) { VbCDia = 0; }

            if ((GrdDatos.Rows[e.RowIndex].FindControl("TxtDI") as TextBox).Text.Trim().Equals("")) { VbDIA = 0; }
            else { VbDIA = Convert.ToInt32((GrdDatos.Rows[e.RowIndex].FindControl("TxtDI") as TextBox).Text.Trim()); }
            if (VbDIA < 0) { VbDIA = 0; }

            if ((GrdDatos.Rows[e.RowIndex].FindControl("TxtDF") as TextBox).Text.Trim().Equals("")) { VbDFA = 0; }
            else { VbDFA = Convert.ToInt32((GrdDatos.Rows[e.RowIndex].FindControl("TxtDF") as TextBox).Text.Trim()); }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 7,@ID,@Desc,@US,'','','','','PrioSol','UPDATE',@CrD,@DIA,@DFA,@Act,0,@CC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@ID", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
                        SC.Parameters.AddWithValue("@Desc", VbDesc);
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@CrD", VbCDia);
                        SC.Parameters.AddWithValue("@DIA", VbDIA);
                        SC.Parameters.AddWithValue("@DFA", VbDFA);
                        SC.Parameters.AddWithValue("@Act", (GrdDatos.Rows[e.RowIndex].FindControl("CkbAct") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdDatos.EditIndex = -1;
                            BindData();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindData(); }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();

            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
    }
}