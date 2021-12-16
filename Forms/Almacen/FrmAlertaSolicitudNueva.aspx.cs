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

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmAlertaSolicitudNueva : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "";
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
                TitForm.Text = "";
                IdiomaControles();
                BindModal();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalAlerta", "$('#ModalAlerta').modal();", true);
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
                    LblTitAlrt.Text = bO.Equals("Titulo") ? bT : LblTitAlrt.Text;
                    LblTitAlerta.Text = bO.Equals("LblTitAlerta") ? bT : LblTitAlerta.Text;
                    GrdAlrta.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAlrta.EmptyDataText;
                    GrdAlrta.Columns[0].HeaderText = bO.Equals("PedidoMstr") ? bT : GrdAlrta.Columns[0].HeaderText;
                    GrdAlrta.Columns[1].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdAlrta.Columns[1].HeaderText;
                    GrdAlrta.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdAlrta.Columns[3].HeaderText;
                    GrdAlrta.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdAlrta.Columns[4].HeaderText;
                    GrdAlrta.Columns[5].HeaderText = bO.Equals("FechaMstr") ? bT : GrdAlrta.Columns[5].HeaderText;
                    GrdAlrta.Columns[6].HeaderText = bO.Equals("LblUsrMstr") ? bT : GrdAlrta.Columns[6].HeaderText;                  
                    BtnExportarModl.Text = bO.Equals("BtnExportMstr") ? bT : BtnExportarModl.Text;
                    BtnCerrarAlerta.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCerrarAlerta.Text;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindModal()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC Consultas_General_Logistica 11,'','','',1, @ICC,0,'01-01-1','01-01-1'";

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Cs", Session["C77U"].ToString());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DT);
                        if (DT.Rows.Count > 0) { GrdAlrta.DataSource = DT; GrdAlrta.DataBind(); }
                        else { GrdAlrta.DataSource = null; GrdAlrta.DataBind(); }
                        ViewState["DT"] = DT;
                    }
                }
            }
        }
        protected void BtnExportarModl_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DT = (DataTable)ViewState["DT"];
                string StSql, VbNomRpt = "Reserva Nueva";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportAlertSolicitudPedidoNew", Session["77IDM"].ToString().Trim());
                StSql = "EXEC Consultas_General_Logistica 11,'CurExportAlertSolicitudPedidoNew', @Idm,'WEB',0, @ICC,1,'01-01-1','01-01-1'";
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result = Idioma.Select("Objeto= 'Caption'");
                foreach (DataRow row in Result)
                { VbNomRpt = row["Texto"].ToString().Trim(); }// 
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.CommandTimeout = 90000000;
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
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Alerta solicitud nuevas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAlrta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                if (dr["AprobacionDetalle"].ToString().Equals("1")) { e.Row.BackColor = System.Drawing.Color.Yellow; }
            }
        }
    }
}