using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmInformePropuesta : System.Web.UI.Page
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
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                ModSeguridad();
                MultVw.ActiveViewIndex = 0;
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//                                      

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
                    BtnSegumiento.Text = bO.Equals("BtnSegumiento") ? bT : BtnSegumiento.Text;
                    BtnExportarVentas.Text = bO.Equals("BtnExportarVentas") ? bT : BtnExportarVentas.Text;
                    BtnExportarRepa.Text = bO.Equals("BtnExportarRepa") ? bT : BtnExportarRepa.Text;
                    BtnSegumiento.ToolTip = bO.Equals("BtnSegumientoTT") ? bT : BtnSegumiento.ToolTip;
                    BtnExportarVentas.ToolTip = bO.Equals("BtnExportarVentasTT") ? bT : BtnExportarVentas.ToolTip;
                    BtnExportarRepa.ToolTip = bO.Equals("BtnExportarRepaTT") ? bT : BtnExportarRepa.ToolTip;
                    //**************************************** Seguimiento //****************************************
                    LblTitSegumient.Text = bO.Equals("LblTitSegumient") ? bT : LblTitSegumient.Text;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("Busqueda") ? bT : LblBusqueda.Text;
                    IbtBusqueda.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtBusqueda.ToolTip;
                    if (bO.Equals("placeholderBusq"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[0].HeaderText = bO.Equals("GrdNumPpt") ? bT : GrdBusq.Columns[0].HeaderText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("GrdEstad") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("GrdUsuario") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("GrdFechNot") ? bT : GrdBusq.Columns[3].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindConsultar()
        {
            TxtBusqueda.Text = TxtBusqueda.Text.Trim().Equals("") ? "1" : TxtBusqueda.Text.Trim();
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Propuesta 20,@Prmtr,@ICC,'','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                        else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void BtnSegumiento_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; }
        protected void BtnExportarVentas_Click(object sender, EventArgs e)
        { { Exportar("VENTA"); } }
        protected void BtnExportarRepa_Click(object sender, EventArgs e)
        { Exportar("REPA"); }
        protected void Exportar(string Opcion)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            string VbTxtSql = "";

            if (Opcion.Equals("VENTA"))
            {
                CursorIdioma.Alimentar("CurExportPPTVenta", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC SP_PANTALLA_Propuesta 28,@ICC,'','CurExportPPTVenta','WEB',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                VbNomRpt = "Sales_Quotation";
            }
            else
            {
                CursorIdioma.Alimentar("CurExportPPTRepa", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC SP_TablasMRO 14, '','','','','','','','CurExportPPTRepa','WEB',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                VbNomRpt = "Repair_Quotation";
            }
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SC.Connection = con;
                        sda.SelectCommand = SC;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            ds.Tables[0].TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
                                Response.Charset = "";
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BindConsultar(); }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                if (dr["Reversion"].ToString().Equals("1")) { e.Row.BackColor = System.Drawing.Color.DarkOrange; }
            }
        }
    }
}