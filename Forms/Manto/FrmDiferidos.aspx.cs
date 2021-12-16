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

namespace _77NeoWeb.Forms.Manto
{
    public partial class FrmDiferidos : System.Web.UI.Page
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
                string VbM = Convert.ToString(DateTime.UtcNow.Month);
                string VbY = Convert.ToString(DateTime.UtcNow.Year);
                string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, "01");
                DateTime VbFecID = Convert.ToDateTime(fecha);
                TxtFechI.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                TxtFechF.Text = string.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
                ModSeguridad();
                RdbTodos.Checked = true;
                BindBDdl();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
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
                    RdbTodos.Text = bO.Equals("RdbTodos") ? "&nbsp" + bT : RdbTodos.Text;
                    RdbAbierto.Text = bO.Equals("RdbAbierto") ? "&nbsp" + bT : RdbAbierto.Text;
                    RdbCumpl.Text = bO.Equals("RdbCumpl") ? "&nbsp" + bT : RdbCumpl.Text;
                    LblFechI.Text = bO.Equals("MstrFecI") ? bT : LblFechI.Text;
                    LblFechF.Text = bO.Equals("MstrFecF") ? bT : LblFechF.Text;
                    BtnExportar.Text = bO.Equals("BtnExportar") ? bT : BtnExportar.Text;
                    BtnAlertaCO.Text = bO.Equals("BtnAlertaCO") ? bT : BtnAlertaCO.Text;
                    LblTitReportes.Text = bO.Equals("LblTitReportesD") ? bT : LblTitReportes.Text;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdNumRpte") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("LblAeronave") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdFGen") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdMel") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdCat") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdFvenc") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdFCump") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdStt") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("GrdLV") ? bT : GrdDatos.Columns[8].HeaderText;
                    GrdDatos.Columns[9].HeaderText = bO.Equals("GrdTec") ? bT : GrdDatos.Columns[9].HeaderText;
                    GrdDatos.Columns[10].HeaderText = bO.Equals("GrdDescRte") ? bT : GrdDatos.Columns[10].HeaderText;
                    GrdDatos.Columns[11].HeaderText = bO.Equals("GrdAccC") ? bT : GrdDatos.Columns[11].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl()
        {
            string LtxtSql = string.Format("EXEC SP_TablasIngenieria 16,'','','','','','','','','AKRte',0,0,0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"]);
            DdlAeronave.DataSource = Cnx.DSET(LtxtSql);
            DdlAeronave.DataMember = "Datos";
            DdlAeronave.DataTextField = "Matricula";
            DdlAeronave.DataValueField = "CodAeronave";
            DdlAeronave.DataBind();
        }
        protected void BindData()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbOpc = "";

            if (RdbTodos.Checked == true) { VbOpc = ""; }
            if (RdbAbierto.Checked == true) { VbOpc = "A"; }
            if (RdbCumpl.Checked == true) { VbOpc = "C"; }

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
            string VbTxtSql = "EXEC Consultas_General_Manto 4,@Hk,@Prmt,'','',0,0,0,@ICC,@FI,@FF,'03-10-00'";

            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@Prmt", VbOpc);
                    SC.Parameters.AddWithValue("@Hk", DdlAeronave.Text.Trim() == "" ? "" : DdlAeronave.SelectedItem.Text.Trim());
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
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        { BindData(); }
        protected void BtnAlertaCO_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Manto/FrmAlertaCarryOver.aspx"); }
        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string Query = "", VbNomArchivo = "", VbOpc = "";
                DataRow[] Result;
                if (RdbTodos.Checked == true) { VbOpc = ""; }
                if (RdbAbierto.Checked == true) { VbOpc = "A"; }
                if (RdbCumpl.Checked == true) { VbOpc = "C"; }

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

                Result = Idioma.Select("Objeto= 'CurExptrRteDiferido'");
                foreach (DataRow row in Result) { VbNomArchivo = row["Texto"].ToString().Trim(); }

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExptrRteDiferido", Session["77IDM"].ToString().Trim());
                Query = "EXEC Consultas_General_Manto 4,@Hk,@Prmt,'','CurExptrRteDiferido',0,0,0,@ICC,@FI,@FF,'03-10-00'";
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@Prmt", VbOpc);
                        cmd.Parameters.AddWithValue("@Hk", DdlAeronave.Text.Trim() == "" ? "" : DdlAeronave.SelectedItem.Text.Trim());
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Diferido Reporte Manto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}