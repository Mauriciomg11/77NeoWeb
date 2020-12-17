using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmWorkSheet : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            /* if (Session["Login77"] == null)
             {
                 Response.Redirect("~/FrmAcceso.aspx");
             }*/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoHCT";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "860064038-4"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles /* */
            }
            if (!IsPostBack)
            {
                MlVw.ActiveViewIndex = 0;
                ModSeguridad();
                BindBDdlAK();
                ViewState["CONSULTA"] = "N";
                Page.Title = "Work Sheet";
                TitForm.Text = "Work Sheet";
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnWSNew.Visible = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; BtnImpWS.Visible = false; BtnImpRecurs.Visible = false; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
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
                SC.Parameters.AddWithValue("@F1", ViewState["PFileName"].ToString().Trim());
                SC.Parameters.AddWithValue("@F2", "");
                SC.Parameters.AddWithValue("@F3", "0");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string b1 = tbl["Objeto"].ToString();
                    string b2 = tbl["Texto"].ToString();
                    Idioma.Rows.Add(tbl["Objeto"].ToString(), tbl["Texto"].ToString());
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdlAK()
        {
            string LtxtSql = "EXEC SP_PANTALLA_Status 11,'','','','HK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlWSHK.DataSource = Cnx.DSET(LtxtSql);
            DdlWSHK.DataMember = "Datos";
            DdlWSHK.DataTextField = "Matricula";
            DdlWSHK.DataValueField = "CodAeronave";
            DdlWSHK.DataBind();
        }
        protected void BtnWSNew_Click(object sender, EventArgs e)
        {
            if (!DdlWSHK.Text.Equals("0")) { MlVw.ActiveViewIndex = 1; }
        }

        protected void BtnImpWS_Click(object sender, EventArgs e)
        {

        }

        protected void BtnImpRecurs_Click(object sender, EventArgs e)
        {

        }




        protected void BtnWSProces_Click(object sender, EventArgs e)
        {

        }
        //*************************************************  WORK SHEET ABIERTAS  *************************************************
        protected void BIndDWSAOpen()
        {

            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurGridWS", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "EXEC SP_PANTALLA_WorkSheet 7,'','','','',@Prmtr,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", DdlWSHK.Text.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdWSAbiertas.DataSource = DtB;
                            GrdWSAbiertas.DataBind();
                        }
                        else
                        {
                            GrdWSAbiertas.DataSource = null;
                            GrdWSAbiertas.DataBind();
                        }
                    }
                }
            }
        }
        protected void DdlWSHK_TextChanged(object sender, EventArgs e)
        {
            if (!DdlWSHK.Text.Equals("0")) { DatosAsignarOT("", "0001"); BIndDWSAOpen(); }
        }
        protected void GrdWSAbiertas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdWSAbiertas.SelectedRow.Cells[1].Text);
            DatosAsignarOT(vbcod, "0001");
            MlVw.ActiveViewIndex = 1;
        }
        protected void GrdWSAbiertas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
        }
        protected void GrdWSAbiertas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdWSAbiertas.PageIndex = e.NewPageIndex;
            BIndDWSAOpen();
        }
        //*************************************************  BUSQUEDA  *************************************************
        protected void BIndDBusqWS()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurGridWS", Session["77IDM"].ToString().Trim());
                string VbTxtSql = "EXEC SP_PANTALLA_WorkSheet 12,@Prmtr,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtWSBusq.Text.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdWSBusq.DataSource = DtB;
                            GrdWSBusq.DataBind();
                        }
                        else
                        {
                            GrdWSBusq.DataSource = null;
                            GrdWSBusq.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtSWConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BIndDBusqWS();
        }
        protected void GrdWSBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            MlVw.ActiveViewIndex = 1;
            string vbcod = HttpUtility.HtmlDecode(GrdWSBusq.SelectedRow.Cells[1].Text);
            string VbIdx = GrdWSBusq.DataKeys[this.GrdWSBusq.SelectedIndex][0].ToString();
            DdlWSHK.Text = GrdWSBusq.DataKeys[this.GrdWSBusq.SelectedIndex][1].ToString();
            BIndDWSAOpen();
            //DatosAsignarOT(vbcod, VbIdx);

        }
        protected void GrdWSBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
            }
        }
        protected void GrdWSBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdWSBusq.PageIndex = e.NewPageIndex;
            BIndDBusqWS();
        }
        //*************************************************  Asignar OT a la WS  *************************************************
        protected void DatosAsignarOT(string WS, string Estado)
        {
            TxtAsigOTHK.Text = DdlWSHK.SelectedItem.Text.Trim();
            TxtAsingOTWS.Text = WS;
            RdbAsigOT.Checked = true;
        }
        protected void IbtCerrarAsigOT_Click(object sender, ImageClickEventArgs e)
        {
            MlVw.ActiveViewIndex = 0;
        }
        protected void IbtAsigOTBusq_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}