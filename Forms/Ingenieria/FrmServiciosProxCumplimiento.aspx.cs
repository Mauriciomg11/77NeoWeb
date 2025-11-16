using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmServiciosProxCumplimiento : System.Web.UI.Page
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
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                    Session["MonLcl"] = Cnx.GetMonedLcl();// "COP|USD"
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                CkbVisualTodo.Checked = true;
                TxtDiaVisual.Text = "365";
                ModSeguridad();
                BindData("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
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
                    TitForm.Text = bO.Equals("Caption") ? bT : TitForm.Text;
                    LblAeronave.Text = bO.Equals("LblAeronave") ? bT : LblAeronave.Text;
                    LblDiaVisual.Text = bO.Equals("LblDiaVisual") ? bT : LblDiaVisual.Text;
                    LblEtiqDia.Text = bO.Equals("LblEtiqDia") ? bT : LblEtiqDia.Text;
                    CkbVisualTodo.Text = bO.Equals("CkbVisualTodo") ? "&nbsp" + bT : CkbVisualTodo.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    IbnExcel.ToolTip = bO.Equals("IbnExcelTT") ? bT : IbnExcel.ToolTip;
                    BtnSvcRestCero.Text = bO.Equals("BtnSvcRestCero") ? bT : BtnSvcRestCero.Text;
                    BtnSvcRestCero.ToolTip = bO.Equals("BtnSvcRestCeroTT") ? bT : BtnSvcRestCero.ToolTip;
                    BtnUbicaTec.Text = bO.Equals("BtnUbicaTec") ? bT : BtnUbicaTec.Text;
                    BtnUbicaTec.ToolTip = bO.Equals("BtnUbicaTecTT") ? bT : BtnUbicaTec.ToolTip;
                    LblTitServicios.Text = bO.Equals("LblTitServicios") ? bT : LblTitServicios.Text;
                    GrdDatos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDatos.EmptyDataText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("LblAeronave") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdDesc") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdDoc") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdFecUC") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdOT") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[9].HeaderText = bO.Equals("GrdProy") ? bT : GrdDatos.Columns[9].HeaderText;
                    GrdDatos.Columns[10].HeaderText = bO.Equals("GrdFrec") ? bT : GrdDatos.Columns[10].HeaderText;
                    GrdDatos.Columns[11].HeaderText = bO.Equals("GrdUnMed") ? bT : GrdDatos.Columns[11].HeaderText;
                    GrdDatos.Columns[12].HeaderText = bO.Equals("GrdExt") ? bT : GrdDatos.Columns[12].HeaderText;
                    GrdDatos.Columns[13].HeaderText = bO.Equals("GrdRemn") ? bT : GrdDatos.Columns[13].HeaderText;
                    GrdDatos.Columns[14].HeaderText = bO.Equals("GrdFrecD") ? bT : GrdDatos.Columns[14].HeaderText;
                    GrdDatos.Columns[15].HeaderText = bO.Equals("GrdExtD") ? bT : GrdDatos.Columns[15].HeaderText;
                    GrdDatos.Columns[16].HeaderText = bO.Equals("GrdRmnD") ? bT : GrdDatos.Columns[16].HeaderText;
                    GrdDatos.Columns[17].HeaderText = bO.Equals("GrdUltDP") ? bT : GrdDatos.Columns[17].HeaderText;
                    //**********************************Servicios Reseteable  ******************************************
                    IbtCerrarSvcReset.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarSvcReset.ToolTip;
                    LblTitSvcReset.Text = bO.Equals("LblTitServicios") ? bT : LblTitSvcReset.Text;
                    GrdSvcReset.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdSvcReset.EmptyDataText;
                    GrdSvcReset.Columns[0].HeaderText = bO.Equals("GrdSvc") ? bT : GrdSvcReset.Columns[0].HeaderText;
                    GrdSvcReset.Columns[1].HeaderText = bO.Equals("GrdFrec") ? bT : GrdSvcReset.Columns[1].HeaderText;
                    GrdSvcReset.Columns[2].HeaderText = bO.Equals("GrdContdr") ? bT : GrdSvcReset.Columns[2].HeaderText;
                    GrdSvcReset.Columns[3].HeaderText = bO.Equals("GrdFecUC") ? bT : GrdSvcReset.Columns[3].HeaderText;
                    GrdSvcReset.Columns[6].HeaderText = bO.Equals("GrdDesElem") ? bT : GrdSvcReset.Columns[6].HeaderText;
                    GrdSvcReset.Columns[7].HeaderText = bO.Equals("LblAeronave") ? bT : GrdSvcReset.Columns[7].HeaderText;
                    //**********************************Ubicación Técnica ******************************************
                    IbtCerrarUbicTec.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarUbicTec.ToolTip;
                    LblTitUbicTec.Text = bO.Equals("LblTitUbicTec") ? bT : LblTitUbicTec.Text;
                    GrdUbicTec.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdUbicTec.EmptyDataText;
                    GrdUbicTec.Columns[0].HeaderText = bO.Equals("LblAeronave") ? bT : GrdUbicTec.Columns[0].HeaderText;
                    GrdUbicTec.Columns[1].HeaderText = bO.Equals("GrdUltNvl") ? bT : GrdUbicTec.Columns[1].HeaderText;
                    GrdUbicTec.Columns[2].HeaderText = bO.Equals("GrdDesc") ? bT : GrdUbicTec.Columns[2].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindData(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VbTxtSql, VbCodAnt;
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbHK;
                        if (DdlAeronave.Text.Equals("")) { VbHK = ""; }
                        else { VbHK = DdlAeronave.SelectedItem.Text.Trim(); }
                        if (VbHK.Trim().Equals(""))
                        { VbTxtSql = "EXEC Consultas_General 6, '', '', '',@Todo,@Di, @ICC, '06-01-2012', '06-01-2012'"; }
                        else
                        { VbTxtSql = "EXEC Consultas_General 6, @A, '', '',@Todo,@Di, @ICC, '06-01-2012', '06-01-2012'"; }

                        SCnx.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                        {
                            SC.Parameters.AddWithValue("@Todo", CkbVisualTodo.Checked == true ? 1 : 0);
                            SC.Parameters.AddWithValue("@Di", TxtDiaVisual.Text);
                            SC.Parameters.AddWithValue("@A", VbHK);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DST = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DST);
                                    DST.Tables[0].TableName = "Servicios";
                                    DST.Tables[1].TableName = "HK";
                                    DST.Tables[2].TableName = "SvcRst";
                                    DST.Tables[3].TableName = "UbicacionSinInst";

                                    ViewState["DST"] = DST;
                                }
                            }
                        }
                    }
                }
                DST = (DataSet)ViewState["DST"];
                if (DST.Tables[0].Rows.Count > 0) { GrdDatos.DataSource = DST.Tables[0]; GrdDatos.DataBind(); }
                else
                {
                    DST.Tables[0].Rows.Add(DST.Tables[0].NewRow());
                    GrdDatos.DataSource = DST.Tables[0];
                    GrdDatos.DataBind();
                    GrdDatos.Rows[0].Cells.Clear();
                    GrdDatos.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
                VbCodAnt = DdlAeronave.Text.Trim();
                DdlAeronave.DataSource = DST.Tables[1];
                DdlAeronave.DataTextField = "Matricula";
                DdlAeronave.DataValueField = "CodAeronave";
                DdlAeronave.DataBind();
                DdlAeronave.Text = VbCodAnt;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Consulta Proximos cumplimientos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { BindData("UPD"); }
        protected void BtnSvcRestCero_Click(object sender, EventArgs e)
        { BIndDSvcReset(); MultVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString(); }
        protected void BtnUbicaTec_Click(object sender, EventArgs e)
        { BIndDUbicTec(); MultVw.ActiveViewIndex = 2; Page.Title = ViewState["PageTit"].ToString(); }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                Literal litSmfro = (Literal)e.Row.FindControl("litSmfro");
                string VbCap = dr["Bandera"].ToString();
                string S_W_S = dr["W_S"].ToString();
                switch (VbCap)
                {
                    case "3":// Vencidos
                             // e.Row.BackColor = System.Drawing.Color.Tomato;
                             //e.Row.ForeColor = System.Drawing.Color.White;
                        litSmfro.Text = "<i class='bi bi-circle-fill semaforo-alerta' style='color:red; font-size:20px;'></i>";
                        if (!S_W_S.Trim().Equals("")) { litSmfro.Text = "<i class='bi bi-circle-fill' style='color:Darkred; font-size:20px;'></i>"; }// si tiene WS no parpadea
                        break;
                    case "2":// Proximos a vencerse
                             //e.Row.BackColor = System.Drawing.Color.GreenYellow;
                        litSmfro.Text = "<i class='bi bi-circle-fill semaforo-alerta' style='color:Orange; font-size:20px;'></i>";
                        break;
                    default:  // OK                      
                        litSmfro.Text = "<i class='bi bi-circle-fill' style='color:Green; font-size:20px;'></i>";
                        break;
                }/**/
                VbCap = dr["Proyeccion"].ToString();
                if (VbCap.Equals("")) // No tiene configurada la fecha del ult cumplimiento en servicios
                {
                    litSmfro.Text = "<i class='bi bi-circle-fill semaforo-alerta' style='color:Yellow; font-size:20px;'></i>";
                    e.Row.Cells[6].BackColor = System.Drawing.Color.LightSalmon;
                    //e.Row.Cells[6].Text = "<i class='semaforo-alerta' style='color:red; font-size:20px;'></i>";
                }
            }
        }
        protected void IbnExcel_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string VbTxtSql = "", VbNomArchivo = "";
                DataRow[] Result;
                Idioma = (DataTable)ViewState["TablaIdioma"];

                Result = Idioma.Select("Objeto= 'CurExptrAlerProxSvc'");
                foreach (DataRow row in Result) { VbNomArchivo = row["Texto"].ToString().Trim(); }

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExptrAlerProxSvc", Session["77IDM"].ToString().Trim());
                if (DdlAeronave.Text.Trim().Equals("0"))
                { VbTxtSql = "EXEC Consultas_General 3, '', '', 'CurExptrAlerProxSvc',@Todo,@Di, @ICC, '06-01-2012', '06-01-2012'"; }
                else
                { VbTxtSql = "EXEC Consultas_General 3, @A, '', 'CurExptrAlerProxSvc',@Todo,@Di, @ICC, '06-01-2012', '06-01-2012'"; }

                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@Todo", CkbVisualTodo.Checked == true ? 1 : 0);
                        SC.Parameters.AddWithValue("@Di", TxtDiaVisual.Text);
                        SC.Parameters.AddWithValue("@A", DdlAeronave.SelectedItem.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SC.Connection = con;
                            sda.SelectCommand = SC;
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Informe Ingeniería Próximos Cumplimientos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        //*************************************** Servicios Reset  ************************************************
        protected void IbtCerrarSvcReset_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BIndDSvcReset()
        {
            DST = (DataSet)ViewState["DST"];

            if (DST.Tables[2].Rows.Count > 0) { GrdSvcReset.DataSource = DST.Tables[2]; GrdSvcReset.DataBind(); }
            else { GrdSvcReset.DataSource = null; GrdSvcReset.DataBind(); }
        }
        //*************************************** Ubicación Técnica  ************************************************
        protected void IbtCerrarUbicTec_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BIndDUbicTec()
        {
            DST = (DataSet)ViewState["DST"];
            if (DST.Tables[3].Rows.Count > 0) { GrdUbicTec.DataSource = DST.Tables[3]; GrdUbicTec.DataBind(); }
            else { GrdUbicTec.DataSource = null; GrdUbicTec.DataBind(); }
        }
    }
}