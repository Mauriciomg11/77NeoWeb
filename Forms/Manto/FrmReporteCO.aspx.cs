using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Manto
{
    public partial class FrmReporteCO : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DST = new DataSet();
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
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                    Session["MonLcl"] = Cnx.GetMonedLcl();// "COP|USD"
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                string VbM = Convert.ToString(DateTime.UtcNow.Month);
                string VbY = Convert.ToString(DateTime.UtcNow.Year);
                string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, "01");
                DateTime VbFecID = Convert.ToDateTime(fecha);
                TxtFechI.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                TxtFechF.Text = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
                ModSeguridad();
                BindBDdl();
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            IdiomaControles();
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
                    LblAeronave.Text = bO.Equals("LblAeronave") ? bT : LblAeronave.Text;
                    BtnConsult.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsult.Text;
                    LblStatus.Text = bO.Equals("LblStatus") ? bT : LblStatus.Text;
                    LblOTPpl.Text = bO.Equals("LblOTPpl") ? bT : LblOTPpl.Text;
                    LblRpteNro.Text = bO.Equals("LblRpteNro") ? bT : LblRpteNro.Text;
                    LblFechI.Text = bO.Equals("LblFechI") ? bT : LblFechI.Text;
                    LblFechF.Text = bO.Equals("LblFechF") ? bT : LblFechF.Text;
                    BtnImprimir.Text = bO.Equals("BtnImprimir") ? bT : BtnImprimir.Text;
                    BtnExportar.Text = bO.Equals("BtnExportar") ? bT : BtnExportar.Text;
                    BtnAlertaCO.Text = bO.Equals("BtnAlertaCO") ? bT : BtnAlertaCO.Text;
                    LblTitReportes.Text = bO.Equals("LblTitReportes") ? bT : LblTitReportes.Text;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("LblStatus") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("LblAeronave") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("LblRpteNro") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdPrg") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrRdoPor") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdFGene") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdDescRte") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdFCump") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("GrdAccC") ? bT : GrdDatos.Columns[8].HeaderText;
                    GrdDatos.Columns[10].HeaderText = bO.Equals("GrdLic") ? bT : GrdDatos.Columns[10].HeaderText;
                    GrdDatos.Columns[11].HeaderText = bO.Equals("LblOTPpl") ? bT : GrdDatos.Columns[11].HeaderText;
                    GrdDatos.Columns[12].HeaderText = bO.Equals("GrdLV") ? bT : GrdDatos.Columns[12].HeaderText;
                    GrdDatos.Columns[13].HeaderText = bO.Equals("GrdUbTec") ? bT : GrdDatos.Columns[13].HeaderText;

                    //********************************** Impresion  ******************************************
                    IbtCerrarImpr.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpr.ToolTip;
                    LblTitImpresion.Text = bO.Equals("LblTitImpresion") ? bT : LblTitImpresion.Text;
                    ViewState["PTitulo"] = bO.Equals("PTitulo") ? bT : ViewState["PTitulo"];
                    ViewState["PHK"] = bO.Equals("LblAeronave") ? bT : ViewState["PHK"];
                    ViewState["PCteLbl"] = bO.Equals("PCteLbl") ? bT : ViewState["PCteLbl"];

                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Reporte_Manto2 11,'','','','','',0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DST = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DST);
                            DST.Tables[0].TableName = "HK";
                            DST.Tables[1].TableName = "Estado";
                            DST.Tables[2].TableName = "OT";
                            DST.Tables[3].TableName = "Rte";

                            ViewState["DST"] = DST;
                        }
                    }
                }
            }
            DST = (DataSet)ViewState["DST"];

            DdlAeronave.DataSource = DST.Tables[0];
            DdlAeronave.DataTextField = "Matricula";
            DdlAeronave.DataValueField = "CodAeronave";
            DdlAeronave.DataBind();

            DdlStatus.DataSource = DST.Tables[1];
            DdlStatus.DataTextField = "Descripcion";
            DdlStatus.DataValueField = "CodStatus";
            DdlStatus.DataBind();

            DdlOTPpl.DataSource = DST.Tables[2];
            DdlOTPpl.DataTextField = "Descripcion";
            DdlOTPpl.DataValueField = "CodOt";
            DdlOTPpl.DataBind();

            DdlRpteNro.DataSource = DST.Tables[3];
            DdlRpteNro.DataTextField = "Descripcion";
            DdlRpteNro.DataValueField = "CodIdLvDetManto";
            DdlRpteNro.DataBind();
        }
        protected void BindData()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            DataRow[] Result;
            Cnx.ValidarFechas(TxtFechI.Text.Trim(), TxtFechF.Text.Trim(), 2);
            var Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }

            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC Consultas_General_Manto 25,@St,@Ae,@Sn,@Pn, 0, @Ot,@Rt, @ICC,@FI,@FF,'03-10-00'";
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@St", DdlStatus.Text.Trim());
                    SC.Parameters.AddWithValue("@Ae", DdlAeronave.Text.Trim() == "" ? "" : DdlAeronave.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@Sn", TxtSN.Text.Trim());
                    SC.Parameters.AddWithValue("@Pn", TxtPN.Text.Trim());
                    SC.Parameters.AddWithValue("@Ot", DdlOTPpl.Text.Trim());
                    SC.Parameters.AddWithValue("@Rt", DdlRpteNro.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechI.Text.Trim()));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechF.Text.Trim()));
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0) { GrdDatos.DataSource = dtbl; GrdDatos.DataBind(); }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        { BindData(); }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            MultVw.ActiveViewIndex = 1;
            Idioma = (DataTable)ViewState["TablaIdioma"];

            DataRow[] Result;
            Cnx.ValidarFechas(TxtFechI.Text.Trim(), TxtFechF.Text.Trim(), 2);
            var Mensj = Cnx.GetMensj();
            if (!Mensj.ToString().Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                Page.Title = ViewState["PageTit"].ToString();
                return;
            }

            string FrmtFech= Session["FormatFecha"].ToString();
            if (Session["FormatFecha"].ToString().Equals("101")) { FrmtFech = "MM/dd/yyyy HH:mm"; }
            else { FrmtFech = "dd/MM/yyyy HH:mm"; } 

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[16];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                parameters[3] = new ReportParameter("PTitulo", ViewState["PTitulo"].ToString().Trim());
                parameters[4] = new ReportParameter("PHK", ViewState["PHK"].ToString().Trim());
                parameters[5] = new ReportParameter("PHKTxt", DdlAeronave.Text.Trim() == "" ? "" : DdlAeronave.SelectedItem.Text.Trim());
                parameters[6] = new ReportParameter("PCteLbl", ViewState["PCteLbl"].ToString().Trim());
                parameters[7] = new ReportParameter("PNroRte", LblRpteNro.Text.Trim());
                parameters[8] = new ReportParameter("PEstd", LblStatus.Text.Trim());
                parameters[9] = new ReportParameter("PFGnr", GrdDatos.Columns[5].HeaderText.Trim());
                parameters[10] = new ReportParameter("PLV", GrdDatos.Columns[12].HeaderText.Trim());
                parameters[11] = new ReportParameter("PRdoPor", GrdDatos.Columns[4].HeaderText.Trim());
                parameters[12] = new ReportParameter("PRte", GrdDatos.Columns[6].HeaderText.Trim());
                parameters[13] = new ReportParameter("PAccC", GrdDatos.Columns[8].HeaderText.Trim());
                parameters[14] = new ReportParameter("POT", DdlOTPpl.Text.Trim());
                parameters[15] = new ReportParameter("FrmtFech", FrmtFech);

                string StSql = "EXEC Consultas_General_Manto 25,@St,@Ae,@Sn,@Pn, 0, @Ot,@Rt, @ICC,@FI,@FF,'03-10-00'";
                using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                {
                    SC.Parameters.AddWithValue("@St", DdlStatus.Text.Trim());
                    SC.Parameters.AddWithValue("@Ae", DdlAeronave.Text.Trim() == "" ? "" : DdlAeronave.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@Sn", TxtSN.Text.Trim());
                    SC.Parameters.AddWithValue("@Pn", TxtPN.Text.Trim());
                    SC.Parameters.AddWithValue("@Ot", DdlOTPpl.Text.Trim());
                    SC.Parameters.AddWithValue("@Rt", DdlRpteNro.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechI.Text.Trim()));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechF.Text.Trim()));
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(ds);
                        RpVwReporte.LocalReport.EnableExternalImages = true;
                        RpVwReporte.LocalReport.ReportPath = "Report/Manto/Inf_FrmReporteCO.rdlc";
                        RpVwReporte.LocalReport.DataSources.Clear();
                        RpVwReporte.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                        RpVwReporte.LocalReport.SetParameters(parameters);
                        RpVwReporte.LocalReport.Refresh();
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnAlertaCO_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Manto/FrmAlertaCarryOver.aspx"); }
        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string query = "", VbNomArchivo = "";
                DataRow[] Result;
                Cnx.ValidarFechas(TxtFechI.Text.Trim(), TxtFechF.Text.Trim(), 2);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }

                Result = Idioma.Select("Objeto= 'CurExptrInfRte'");
                foreach (DataRow row in Result) { VbNomArchivo = row["Texto"].ToString().Trim(); }

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExptrInfRte", Session["77IDM"].ToString().Trim());
                query = "EXEC SP_PANTALLA_Informe_MRO 4,'','','','CurExptrInfRte',0,0,0,@ICC,@FI,@FF,'01-01-1900'";
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechI.Text.Trim()));
                        cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechF.Text.Trim()));
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                int VbLog = VbNomArchivo.Length > 30 ? 30 : VbNomArchivo.Length;
                                ds.Tables[0].TableName = VbNomArchivo.Trim().Substring(0, VbLog);
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
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
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Informe Reporte Manto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarImpr_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
    }
}