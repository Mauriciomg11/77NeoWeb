using _77NeoWeb.prg;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmControlContadoresGeneral : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DetalleElem = new DataTable();
        DataSet DSTProcesar = new DataSet();
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
                ViewState["PageTit"] = "";
                TitForm.Text = "Procesos de ingenieria";
                IdiomaControles();
                if (ViewState["TablaDet"] == null)
                { CrearStructuraTabla("NEW"); }

            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
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
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("TituloProcessIng"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("TituloProcessIng") ? bT : TitForm.Text;
                    BtnProceLibrV.Text = bO.Equals("BtnProceLibrV") ? bT : BtnProceLibrV.Text;
                    BtnProceLibrV.ToolTip = bO.Equals("BtnProceLibrVTT") ? bT : BtnProceLibrV.ToolTip;
                    LblTitProcCont.Text = bO.Equals("BtnProceLibrV") ? bT : LblTitProcCont.Text;
                    BtnAjusExceso.Text = bO.Equals("BtnAjusExceso") ? bT : BtnAjusExceso.Text;
                    BtnAjusExceso.ToolTip = bO.Equals("BtnAjusExcesoTT") ? bT : BtnAjusExceso.ToolTip;
                    BtnAjusDefect.Text = bO.Equals("BtnAjusDefect") ? bT : BtnAjusDefect.Text;
                    BtnAjusDefect.ToolTip = bO.Equals("BtnAjusDefectTT") ? bT : BtnAjusDefect.ToolTip;
                    BtnAjusConve.Text = bO.Equals("BtnAjusConve") ? bT : BtnAjusConve.Text;
                    BtnAjusConve.ToolTip = bO.Equals("BtnAjusConveTT") ? bT : BtnAjusConve.ToolTip;
                    LblSubTitCorreContLVSinProc.Text = bO.Equals("LblSubTitCorreContLVSinProc") ? bT : LblSubTitCorreContLVSinProc.Text;
                    LblSubTitCorrContLV.Text = bO.Equals("LblSubTitCorrContLV") ? bT : LblSubTitCorrContLV.Text;
                    LblSubTitCorrContProcesar.Text = bO.Equals("LblSubTitCorrContProcesar") ? bT : LblSubTitCorrContProcesar.Text;
                    LblSubTitCorreContDatosLV.Text = bO.Equals("LblSubTitCorreContDatosLV") ? bT : LblSubTitCorreContDatosLV.Text;
                    BtnCorrContProcesar.Text = bO.Equals("BtnCorrContProcesar") ? bT : BtnCorrContProcesar.Text;
                    LblCorrContSn1.Text = bO.Equals("LblCorrContSn1") ? bT : LblCorrContSn1.Text;
                    LblCorrContSn2.Text = bO.Equals("LblCorrContSn2") ? bT : LblCorrContSn2.Text;
                    LblCorrContHApu.Text = bO.Equals("LblCorrContHApu") ? bT : LblCorrContHApu.Text;
                    LblCorrContValor.Text = bO.Equals("LblCorrContValor") ? bT : LblCorrContValor.Text;
                    LblCorrContHM.Text = bO.Equals("LblCorrContHM") ? bT : LblCorrContHM.Text;
                    LblCorrContVlos.Text = bO.Equals("LblCorrContVlos") ? bT : LblCorrContVlos.Text;
                    LblCorrContLevant.Text = bO.Equals("LblCorrContLevant") ? bT : LblCorrContLevant.Text;
                    LblCorrContRin.Text = bO.Equals("LblCorrContRin") ? bT : LblCorrContRin.Text;
                    //**************************************Excso**********************************************
                    LblTitExceso.Text = bO.Equals("BtnAjusExceso") ? bT : LblTitExceso.Text;
                    LbExcesFechI.Text = bO.Equals("LbExcesFechI") ? bT : LbExcesFechI.Text;
                    LbExcesHK.Text = bO.Equals("LblSubTitCorrContHK") ? bT : LbExcesHK.Text;
                    LbExcesDescE.Text = bO.Equals("LbExcesDescE") ? bT : LbExcesDescE.Text;
                    BtnExcesProcesar.Text = bO.Equals("BtnCorrContProcesar") ? bT : BtnExcesProcesar.Text;
                    GrdExcesoElem.Columns[0].HeaderText = bO.Equals("GrdMayor") ? bT : GrdExcesoElem.Columns[0].HeaderText;
                    GrdExcesoElem.Columns[3].HeaderText = bO.Equals("LbExcesDescE") ? bT : GrdExcesoElem.Columns[3].HeaderText;
                    LblTitExcesContConHis.Text = bO.Equals("LblTitExcesContConHis") ? bT : LblTitExcesContConHis.Text;
                    GrdExcesoContConHis.Columns[2].HeaderText = bO.Equals("GrdFecha") ? bT : GrdExcesoContConHis.Columns[2].HeaderText;
                    GrdExcesoContConHis.Columns[3].HeaderText = bO.Equals("GrdContador") ? bT : GrdExcesoContConHis.Columns[3].HeaderText;
                    GrdExcesoContConHis.Columns[4].HeaderText = bO.Equals("LblCorrContValor") ? bT : GrdExcesoContConHis.Columns[4].HeaderText;
                    //**************************************DEFECTO**********************************************
                    LblDeftTitulo.Text = bO.Equals("BtnAjusDefect") ? bT : LblDeftTitulo.Text;
                    LblDeftCodHK.Text = bO.Equals("LblSubTitCorrContHK") ? bT : LblDeftCodHK.Text;
                    LblDeftFechI.Text = bO.Equals("LbExcesFechI") ? bT : LblDeftFechI.Text;
                    LblDeftFechF.Text = bO.Equals("LblDeftFechF") ? bT : LblDeftFechF.Text;
                    LblDeftDescr.Text = bO.Equals("LbExcesDescE") ? bT : LblDeftDescr.Text;
                    BtnDeftProcesar.Text = bO.Equals("BtnCorrContProcesar") ? bT : BtnDeftProcesar.Text;
                    GrdDeftElem.Columns[0].HeaderText = bO.Equals("GrdMayor") ? bT : GrdDeftElem.Columns[0].HeaderText;
                    GrdDeftElem.Columns[3].HeaderText = bO.Equals("LbExcesDescE") ? bT : GrdDeftElem.Columns[3].HeaderText;
                    LblTitDeftEleHisManual.Text = bO.Equals("LblTitExcesContConHis") ? bT : LblTitDeftEleHisManual.Text;
                    GrdDeftElemConHis.Columns[2].HeaderText = bO.Equals("GrdFecha") ? bT : GrdDeftElemConHis.Columns[2].HeaderText;
                    GrdDeftElemConHis.Columns[3].HeaderText = bO.Equals("GrdContador") ? bT : GrdDeftElemConHis.Columns[3].HeaderText;
                    GrdDeftElemConHis.Columns[4].HeaderText = bO.Equals("LblCorrContValor") ? bT : GrdDeftElemConHis.Columns[4].HeaderText;
                    //**************************************Conveniencia**********************************************
                    LblConvenTitulo.Text = bO.Equals("BtnAjusConve") ? bT : LblConvenTitulo.Text;
                    LblConvenCodHK.Text = bO.Equals("LblSubTitCorrContHK") ? bT : LblConvenCodHK.Text;
                    LblConvenFechI.Text = bO.Equals("LbExcesFechI") ? bT : LblConvenFechI.Text;
                    LblConvenFechF.Text = bO.Equals("LblDeftFechF") ? bT : LblConvenFechF.Text;
                    BtnConvenProcesar.Text = bO.Equals("BtnCorrContProcesar") ? bT : BtnConvenProcesar.Text;
                    LblTitConvenElemInst.Text = bO.Equals("LblTitConvenElemInst") ? bT : LblTitConvenElemInst.Text;
                    GrdConvenElem.Columns[0].HeaderText = bO.Equals("GrdMayor") ? bT : GrdConvenElem.Columns[0].HeaderText;
                    GrdConvenElem.Columns[1].HeaderText = bO.Equals("GrdMotor") ? bT : GrdConvenElem.Columns[1].HeaderText;
                    GrdConvenElem.Columns[2].HeaderText = bO.Equals("GrdUbica") ? bT : GrdConvenElem.Columns[2].HeaderText;
                    GrdConvenElem.Columns[3].HeaderText = bO.Equals("GrdPosic") ? bT : GrdConvenElem.Columns[3].HeaderText;
                    GrdConvenElem.Columns[6].HeaderText = bO.Equals("LbExcesDescE") ? bT : GrdConvenElem.Columns[6].HeaderText;
                    LblTitConvenEleHisManual.Text = bO.Equals("LblTitExcesContConHis") ? bT : LblTitConvenEleHisManual.Text;
                    GrdConvenElemConHis.Columns[2].HeaderText = bO.Equals("GrdFecha") ? bT : GrdConvenElemConHis.Columns[2].HeaderText;
                    GrdConvenElemConHis.Columns[3].HeaderText = bO.Equals("GrdContador") ? bT : GrdConvenElemConHis.Columns[3].HeaderText;
                    GrdConvenElemConHis.Columns[4].HeaderText = bO.Equals("LblCorrContValor") ? bT : GrdConvenElemConHis.Columns[4].HeaderText;
                }
                sqlCon.Close();
                DataRow[] Result = Idioma.Select("Objeto= 'BtnCorrContProcesarOnCl'");
                foreach (DataRow row in Result)
                { BtnCorrContProcesar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                DataRow[] Result1 = Idioma.Select("Objeto= 'BtnExcesProcesarOnC'");
                foreach (DataRow row in Result1)
                { BtnExcesProcesar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                DataRow[] Result2 = Idioma.Select("Objeto= 'BtnDeftProcesarOnC'");
                foreach (DataRow row in Result2)
                { BtnDeftProcesar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                Result2 = Idioma.Select("Objeto= 'BtnConvenProcesarOnC'");
                foreach (DataRow row in Result2)
                { BtnConvenProcesar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void ColorBtns(string Pos)
        {
            BtnProceLibrV.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusExceso.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusDefect.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusConve.CssClass = "btn btn-outline-primary BotonesPpal";
            switch (Pos)
            {
                case "1":
                    BtnProceLibrV.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "2":
                    BtnAjusExceso.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "3":
                    BtnAjusDefect.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "4":
                    BtnAjusConve.CssClass = "btn btn-info BotonesPpal";
                    break;
            }
        }
        //*******************************************< Procesar LV >*************************************************
        protected void LimpiarCamposProcesarCont()
        {
            TxtCorrContSn1.Text = "";
            TxtCorrContSn2.Text = "";
            TxtCorrContApu.Text = "";
            TxtCorrContHApu.Text = "00:00";
            TxtCorrContValor.Text = "0";
            TxtCorrContHM.Text = "00:00";
            TxtCorrContVlos.Text = "0";
            TxtCorrContLevant.Text = "0";
            TxtCorrContStart.Text = "0";
            TxtCorrContStart2.Text = "0";
            TxtCorrContRin.Text = "0";
            BtnCorrContProcesar.Enabled = false;
        }
        protected void BtnProceLibrV_Click(object sender, EventArgs e)
        {
            ColorBtns("1");
            ListBoXLibrosSinProc();
            BindBDdlAK();
            BindBDdlLVSinProcc();
            LimpiarCamposProcesarCont();
            MlVPI.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void ListBoXLibrosSinProc()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 3,'','','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSTProcesar = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DSTProcesar);
                            DSTProcesar.Tables[0].TableName = "LvSinProcesar";
                            DSTProcesar.Tables[1].TableName = "HK";
                            DSTProcesar.Tables[2].TableName = "LV";
                            LbxLibrosSinProc.Items.Clear();
                            foreach (DataRow row in DSTProcesar.Tables[0].Rows) { LbxLibrosSinProc.Items.Add(row[0].ToString()); }
                            ViewState["DSTProcesar"] = DSTProcesar;
                        }
                    }
                }
            }
        }
        protected void LbxLibrosSinProc_SelectedIndexChanged(object sender, EventArgs e)
        { BindBDdlAK(); LimpiarCamposProcesarCont(); DdlCorrContLVSinProcc.Text = "0"; }
        protected void BindBDdlAK()
        {
            if (!LbxLibrosSinProc.Text.ToString().Equals(""))
            {
                DataTable HK = new DataTable();
                DSTProcesar = (DataSet)ViewState["DSTProcesar"];
                HK = DSTProcesar.Tables[1].Clone();
                DataRow[] Result = DSTProcesar.Tables[1].Select("FechaReporte='" + LbxLibrosSinProc.Text.ToString() + "'");
                foreach (DataRow Row in Result)
                { HK.ImportRow(Row); }
                DdlCorrContHK.DataSource = HK;
                DdlCorrContHK.DataTextField = "Matricula";
                DdlCorrContHK.DataValueField = "CodAeronave";
                DdlCorrContHK.DataBind();
            }
        }
        protected void DdlCorrContHK_TextChanged(object sender, EventArgs e)
        { BindBDdlLVSinProcc(); LimpiarCamposProcesarCont(); }
        protected void BindBDdlLVSinProcc()
        {
            if (!LbxLibrosSinProc.Text.ToString().Equals(""))
            {
                DataTable LV = new DataTable();
                DSTProcesar = (DataSet)ViewState["DSTProcesar"];
                LV = DSTProcesar.Tables[2].Clone();
                DataRow[] Result = DSTProcesar.Tables[2].Select("FechaReporte='" + LbxLibrosSinProc.Text.ToString().Trim() + "' AND CodAeronave = " + DdlCorrContHK.Text.Trim());
                foreach (DataRow Row in Result)
                { LV.ImportRow(Row); }

                DdlCorrContLVSinProcc.DataSource = LV;
                DdlCorrContLVSinProcc.DataTextField = "CodLibroVuelo";
                DdlCorrContLVSinProcc.DataValueField = "IdLibroVuelo";
                DdlCorrContLVSinProcc.DataBind();
            }

        }
        protected void DdlCorrContLVSinProcc_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposProcesarCont();
            TraerdatosLV();
            BtnCorrContProcesar.Enabled = true;
        }
        protected void TraerdatosLV()
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 6,'','','','',@Prmtr,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@Prmtr", DdlCorrContLVSinProcc.Text);
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtCorrContSn1.Text = SDR["Sn1"].ToString().Trim();
                    TxtCorrContStart.Text = SDR["Start"].ToString();
                    TxtCorrContSn2.Text = SDR["Sn2"].ToString().Trim();
                    TxtCorrContStart2.Text = SDR["Start2"].ToString();
                    TxtCorrContApu.Text = HttpUtility.HtmlDecode(SDR["Apu"].ToString().Trim());
                    TxtCorrContHApu.Text = HttpUtility.HtmlDecode(SDR["HoraInicial"].ToString().Trim().Substring(0, 5));
                    TxtCorrContValor.Text = SDR["Horas"].ToString();
                    TxtCorrContHM.Text = HttpUtility.HtmlDecode(SDR["HM"].ToString().Trim());
                    TxtCorrContVlos.Text = SDR["Vuelos"].ToString();
                    TxtCorrContLevant.Text = SDR["Levantes"].ToString();
                    TxtCorrContRin.Text = SDR["Rines"].ToString();
                }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void BtnCorrContProcesar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlCorrContLVSinProcc.Text.Equals("0"))
            { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string PMensj = "", PCodLV = "";
                    string VBQuery = "EXEC SP_PANTALLA_Proceso_Ingenieria 7,@CodLV,@Usu,'','',@HK,0,0,@ICC,@FP,'01-01-1900','01-01-1900'";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@CodLV", DdlCorrContLVSinProcc.SelectedItem.Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@HK", DdlCorrContHK.Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@FP", LbxLibrosSinProc.SelectedValue);
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodLV = HttpUtility.HtmlDecode(SDR["CodLV"].ToString().Trim());
                            }
                            if (!PMensj.Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + PMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { PMensj = row["Texto"].ToString(); }

                                ScriptManager.RegisterClientScriptBlock(this.UplProcesarLV, UplProcesarLV.GetType(), "IdntificadorBloqueScript", "alert('" + PMensj.ToString().Trim() + " " + PCodLV.Trim() + "');", true);
                            }
                            SDR.Close();
                            transaction.Commit();
                            ListBoXLibrosSinProc();
                            BindBDdlAK();
                            BindBDdlLVSinProcc();
                            LimpiarCamposProcesarCont();

                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmControlContadoresGeneral", "Procesar contadores", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                        }
                    }
                }
            }
        }
        //*******************************************< Procesar Por Exceso >*************************************************
        protected void BtnAjusExceso_Click(object sender, EventArgs e)
        {
            ColorBtns("2");
            LimpiarCamposExceso();
            BindBDdlExcesPN();
            BindBDdlExcesSN("");
            DetalleElem = (DataTable)ViewState["TablaDet"];
            DetalleElem.Rows.Clear();
            ViewState["TablaDet"] = DetalleElem;
            MlVPI.ActiveViewIndex = 1;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void LimpiarCamposExceso()
        {
            TxtExcesFechI.Text = "";
            TxtExcesHK.Text = "";
            TxtExcesDescE.Text = "";
            BtnExcesProcesar.Enabled = false;
            GrdExcesoElem.DataSource = null;
            GrdExcesoElem.DataBind();
            DetalleElem.Rows.Clear();
            GrdExcesoContConHis.DataSource = null;
            GrdExcesoContConHis.DataBind();
            GrdExcesoContConHis.Visible = false;
            LblTitExcesContConHis.Visible = false;
        }
        protected void BindBDdlExcesPN()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Proceso_Ingenieria 9,'','','','PNHC',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlExcesPN.DataSource = Cnx.DSET(LtxtSql);
            DdlExcesPN.DataTextField = "PN";
            DdlExcesPN.DataValueField = "Codigo";
            DdlExcesPN.DataBind();
        }
        protected void BindBDdlExcesSN(string PN)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Proceso_Ingenieria 9,'{0}','','','SNHC',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", PN, Session["!dC!@"]);
            DdlExcesSN.DataSource = Cnx.DSET(LtxtSql);
            DdlExcesSN.DataTextField = "SN";
            DdlExcesSN.DataValueField = "Codigo";
            DdlExcesSN.DataBind();
        }
        protected void BIndDExcesElem()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 8,@CE,'','','Exceso',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CE", DdlExcesSN.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdExcesoElem.DataSource = DtB;
                            GrdExcesoElem.DataBind();
                            ViewState["TablaDet"] = DtB;
                        }
                        else
                        {
                            DtB.Rows.Add(DtB.NewRow());
                            GrdExcesoElem.DataSource = DtB;
                            GrdExcesoElem.DataBind();
                            GrdExcesoElem.Rows[0].Cells.Clear();
                            GrdExcesoElem.Rows[0].Cells.Add(new TableCell());
                            GrdExcesoElem.Rows[0].Cells[0].ColumnSpan = DtB.Columns.Count;
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens05PrIng'");
                            foreach (DataRow row in Result)
                            { GrdExcesoElem.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                            GrdExcesoElem.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            ViewState["TablaDet"] = DtB;
                        }
                    }
                }
            }
        }
        protected void CrearStructuraTabla(string Tipo)
        {
            if (Tipo.ToString().Equals("NEW"))
            {
                DetalleElem.Columns.Add("CodElemento", typeof(string));
                DetalleElem.Columns.Add("CodBarras", typeof(string));
                DetalleElem.Columns.Add("ComponenteMayor", typeof(int));
                DetalleElem.Columns.Add("PN", typeof(string));
                DetalleElem.Columns.Add("SN", typeof(string));
                DetalleElem.Columns.Add("CodAeronave", typeof(int));
                DetalleElem.Columns.Add("Descripcion", typeof(string));
                DetalleElem.Columns.Add("HK", typeof(string));
                DetalleElem.Columns.Add("DescElem", typeof(string));
                DetalleElem.Columns.Add("Proceso", typeof(string));
                DetalleElem.Rows.Add("", "", 0, "", "", 0, "", "", "", "");
            }
            ViewState["TablaDet"] = DetalleElem;
        }
        protected void DdlExcesPN_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposExceso();
            BindBDdlExcesSN(DdlExcesPN.Text.Trim());
        }
        protected void DdlExcesSN_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposExceso();
            if (DdlExcesSN.Text.Trim().Equals(""))
            {
                BtnExcesProcesar.Enabled = false;
                return;
            }
            BtnExcesProcesar.Enabled = true;
            BIndDExcesElem();
            foreach (GridViewRow Row in GrdExcesoElem.Rows)
            {
                TxtExcesHK.Text = GrdExcesoElem.DataKeys[Row.RowIndex].Values[1].ToString();
                TxtExcesDescE.Text = GrdExcesoElem.DataKeys[Row.RowIndex].Values[2].ToString();
            }
        }
        protected void GrdExcesoElem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            DetalleElem = (DataTable)ViewState["TablaDet"];
            string VbCodEle = GrdExcesoElem.DataKeys[e.RowIndex].Values["CodElemento"].ToString();

            DataRow[] DRD = DetalleElem.Select("CodElemento='" + VbCodEle + "'");
            foreach (DataRow row in DRD) DetalleElem.Rows.Remove(row);

            ViewState["TablaDet"] = DetalleElem;
            GrdExcesoElem.DataSource = DetalleElem;
            GrdExcesoElem.DataBind();

            GrdExcesoContConHis.DataSource = null;
            GrdExcesoContConHis.DataBind();
            GrdExcesoContConHis.Visible = false;
            LblTitExcesContConHis.Visible = false;
        }
        protected void GrdExcesoElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }/**/
        }
        protected void BtnExcesProcesar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DetalleElem = (DataTable)ViewState["TablaDet"];
            if (TxtExcesFechI.Text.Equals("") || TxtExcesFechI.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06PrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplExceso, UplExceso.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                TxtExcesFechI.Focus();
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string Mensj = "";
                    int VblTieneHis = 0;
                    string VBQuery = "PROC_EXCESO";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurReproceso", DetalleElem);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@FechaI", Convert.ToDateTime(TxtExcesFechI.Text.ToString()));
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@FechaF", "01/01/1900");
                            SqlParameter Prmtrs3 = sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SqlParameter Prmtrs4 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                VblTieneHis = Convert.ToInt32(HttpUtility.HtmlDecode(SDR["TieneHist"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                            if (!Mensj.Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString(); }
                                ScriptManager.RegisterClientScriptBlock(this.UplExceso, UplExceso.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString() + "');", true);
                                BIndDExcesoContConHis();
                                return;
                            }
                            DdlExcesPN.Text = "";
                            BindBDdlExcesSN("");
                            LimpiarCamposExceso();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmControlContadoresGeneral";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "Exceso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BIndDExcesoContConHis()
        {
            DetalleElem = (DataTable)ViewState["TablaDet"];
            GrdExcesoContConHis.Visible = true;
            LblTitExcesContConHis.Visible = true;

            DataTable TblResult = new DataTable();
            TblResult.Columns.Add("PN", typeof(string));
            TblResult.Columns.Add("SN", typeof(string));
            TblResult.Columns.Add("Fecha", typeof(string));
            TblResult.Columns.Add("CodContador", typeof(string));
            TblResult.Columns.Add("ValorTotal", typeof(double));
            TblResult.Columns.Add("CodElemento", typeof(string));

            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "Contadores_Con_Histor";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurElementos", DetalleElem);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@Fecha", TxtExcesFechI.Text.ToString());
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@Proceso", "Exceso");
                            SqlParameter Prmtrs3 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            while (SDR.Read())
                            {
                                TblResult.Rows.Add(HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim()), HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim()),
                                    HttpUtility.HtmlDecode(SDR["Fecha"].ToString().Trim()), HttpUtility.HtmlDecode(SDR["CodContador"].ToString().Trim()),
                                   Convert.ToDouble(HttpUtility.HtmlDecode(SDR["ValorTotal"].ToString().Trim())), HttpUtility.HtmlDecode(SDR["CodElemento"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                            GrdExcesoContConHis.DataSource = TblResult;
                            GrdExcesoContConHis.DataBind();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmControlContadoresGeneral";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "Exceso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }

        }
        //*******************************************< Procesar Por Defecto >*************************************************
        protected void BtnAjusDefect_Click(object sender, EventArgs e)
        {
            ColorBtns("3");
            LimpiarCamposDefecto();
            BindBDdlDeftPN();
            BindBDdlDeftSN("");
            DetalleElem = (DataTable)ViewState["TablaDet"];
            DetalleElem.Rows.Clear();
            ViewState["TablaDet"] = DetalleElem;

            string LtxtSql = string.Format("EXEC SP_PANTALLA_Status 11,'','','','HK',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlDeftCodHK.DataSource = Cnx.DSET(LtxtSql);
            DdlDeftCodHK.DataTextField = "Matricula";
            DdlDeftCodHK.DataValueField = "CodAeronave";
            DdlDeftCodHK.DataBind();

            MlVPI.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void LimpiarCamposDefecto()
        {
            TxtDeftFechI.Text = "";
            TxtDeftFechF.Text = "";
            TxtDeftDescr.Text = "";
            BtnDeftProcesar.Enabled = false;
            GrdDeftElem.DataSource = null;
            GrdDeftElem.DataBind();
            DetalleElem.Rows.Clear();
            GrdDeftElemConHis.DataSource = null;
            GrdDeftElemConHis.DataBind();
            GrdDeftElemConHis.Visible = false;
            LblTitDeftEleHisManual.Visible = false;
            DdlDeftCodHK.Text = "0";
        }
        protected void BindBDdlDeftPN()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Proceso_Ingenieria 9,'','','','PNHC',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlDeftPN.DataSource = Cnx.DSET(LtxtSql);
            DdlDeftPN.DataMember = "Datos";
            DdlDeftPN.DataTextField = "PN";
            DdlDeftPN.DataValueField = "Codigo";
            DdlDeftPN.DataBind();
        }
        protected void BindBDdlDeftSN(string PN)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Proceso_Ingenieria 9,'{0}','','','SNHC',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", PN, Session["!dC!@"]);
            DdlDeftSN.DataSource = Cnx.DSET(LtxtSql);
            DdlDeftSN.DataMember = "Datos";
            DdlDeftSN.DataTextField = "SN";
            DdlDeftSN.DataValueField = "Codigo";
            DdlDeftSN.DataBind();
        }
        protected void BIndDDeftElem()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 8,@CE,'','','Defecto',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CE", DdlDeftSN.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdDeftElem.DataSource = DtB;
                            GrdDeftElem.DataBind();
                            ViewState["TablaDet"] = DtB;
                        }
                        else
                        {
                            DtB.Rows.Add(DtB.NewRow());
                            GrdDeftElem.DataSource = DtB;
                            GrdDeftElem.DataBind();
                            GrdDeftElem.Rows[0].Cells.Clear();
                            GrdDeftElem.Rows[0].Cells.Add(new TableCell());
                            GrdDeftElem.Rows[0].Cells[0].ColumnSpan = DtB.Columns.Count;
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens05PrIng'");
                            foreach (DataRow row in Result)
                            { GrdDeftElem.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                            GrdDeftElem.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            ViewState["TablaDet"] = DtB;
                        }
                    }
                }
            }
        }
        protected void DdlDeftPN_TextChanged(object sender, EventArgs e)
        { LimpiarCamposDefecto(); BindBDdlDeftSN(DdlDeftPN.Text.Trim()); }
        protected void DdlDeftSN_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposDefecto();
            if (DdlDeftSN.Text.Trim().Equals(""))
            {
                BtnDeftProcesar.Enabled = false;
                return;
            }
            BtnDeftProcesar.Enabled = true;
            BIndDDeftElem();
            foreach (GridViewRow Row in GrdDeftElem.Rows)
            {
                TxtDeftDescr.Text = GrdDeftElem.DataKeys[Row.RowIndex].Values[2].ToString();
                DdlDeftCodHK.Text = GrdDeftElem.DataKeys[Row.RowIndex].Values[3].ToString();
            }
        }
        protected void BtnDeftProcesar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DetalleElem = (DataTable)ViewState["TablaDet"];
            if (TxtDeftFechI.Text.Equals("") || TxtDeftFechI.Text.Length > 10 || TxtDeftFechF.Text.Equals("") || TxtDeftFechF.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06PrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplDefecto, UplDefecto.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                TxtDeftFechI.Focus();
                return;
            }
            if (DdlDeftCodHK.Text.Equals("0"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12PrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplDefecto, UplDefecto.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Se requiere la aeronave
                return;
            }//
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string Mensj = "";
                    int VblTieneHis = 0;
                    string VBQuery = "Reproceso_Defecto";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurReproceso", DetalleElem);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@FechaI", Convert.ToDateTime(TxtDeftFechI.Text.ToString()));
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@FechaF", Convert.ToDateTime(TxtDeftFechF.Text.ToString()));
                            SqlParameter Prmtrs3 = sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SqlParameter Prmtrs4 = sqlCmd.Parameters.AddWithValue("@CodElem", DdlDeftSN.Text.Trim());
                            SqlParameter Prmtrs5 = sqlCmd.Parameters.AddWithValue("@CodHK", DdlDeftCodHK.Text.Trim());
                            SqlParameter Prmtrs6 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                VblTieneHis = Convert.ToInt32(HttpUtility.HtmlDecode(SDR["TieneHist"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                            if (!Mensj.Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString(); }
                                ScriptManager.RegisterClientScriptBlock(this.UplDefecto, UplDefecto.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString() + "');", true);
                                BIndDDeftContConHis();
                                return;
                            }
                            DdlDeftPN.Text = "";
                            BindBDdlDeftSN("");
                            LimpiarCamposDefecto();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmControlContadoresGeneral";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "Defecto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdDeftElem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DetalleElem = (DataTable)ViewState["TablaDet"];
            string VbCodEle = GrdDeftElem.DataKeys[e.RowIndex].Values["CodElemento"].ToString();

            DataRow[] DRD = DetalleElem.Select("CodElemento='" + VbCodEle + "'");
            foreach (DataRow row in DRD) DetalleElem.Rows.Remove(row);

            ViewState["TablaDet"] = DetalleElem;
            GrdDeftElem.DataSource = DetalleElem;
            GrdDeftElem.DataBind();
            GrdDeftElemConHis.DataSource = null;
            GrdDeftElemConHis.DataBind();
            GrdDeftElemConHis.Visible = false;
            GrdDeftElemConHis.Visible = false;
        }
        protected void GrdDeftElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (IbtDelete != null)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'IbtDelete'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                    Result1 = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result1)
                    { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }/**/
        }
        protected void BIndDDeftContConHis()
        {
            DetalleElem = (DataTable)ViewState["TablaDet"];
            GrdDeftElemConHis.Visible = true;
            LblTitDeftEleHisManual.Visible = true;

            DataTable TblResult = new DataTable();
            TblResult.Columns.Add("PN", typeof(string));
            TblResult.Columns.Add("SN", typeof(string));
            TblResult.Columns.Add("Fecha", typeof(string));
            TblResult.Columns.Add("CodContador", typeof(string));
            TblResult.Columns.Add("ValorTotal", typeof(double));
            TblResult.Columns.Add("CodElemento", typeof(string));

            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "Contadores_Con_Histor";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurElementos", DetalleElem);
                            SqlParameter Prmtrs1 = sqlCmd.Parameters.AddWithValue("@Fecha", Convert.ToDateTime(TxtDeftFechI.Text.ToString()));
                            SqlParameter Prmtrs2 = sqlCmd.Parameters.AddWithValue("@Proceso", "Defecto");
                            SqlParameter Prmtrs3 = sqlCmd.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"]);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            while (SDR.Read())
                            {
                                TblResult.Rows.Add(HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim()), HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim()),
                                    HttpUtility.HtmlDecode(SDR["Fecha"].ToString().Trim()), HttpUtility.HtmlDecode(SDR["CodContador"].ToString().Trim()),
                                   Convert.ToDouble(HttpUtility.HtmlDecode(SDR["ValorTotal"].ToString().Trim())), HttpUtility.HtmlDecode(SDR["CodElemento"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                            GrdDeftElemConHis.DataSource = TblResult;
                            GrdDeftElemConHis.DataBind();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmControlContadoresGeneral";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "Defecto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        //*******************************************< Procesar Por Conveniencia >*************************************************
        protected void BtnAjusConve_Click(object sender, EventArgs e)
        {
            ColorBtns("4");
            LimpiarCamposConven();
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Status 11,'','','','HK',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlConvenCodHK.DataSource = Cnx.DSET(LtxtSql);
            DdlConvenCodHK.DataTextField = "Matricula";
            DdlConvenCodHK.DataValueField = "CodAeronave";
            DdlConvenCodHK.DataBind();
            MlVPI.ActiveViewIndex = 3;
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void LimpiarCamposConven()
        {
            TxtConvenFechI.Text = "";
            TxtConvenFechF.Text = "";
            BtnConvenProcesar.Enabled = false;
            GrdConvenElem.DataSource = null;
            GrdConvenElem.DataBind();
            LblTitConvenElemInst.Visible = false;
        }
        protected void BIndDConvenElem()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 10,'','','','',@CA,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CA", DdlConvenCodHK.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdConvenElem.DataSource = DtB;
                            GrdConvenElem.DataBind();
                        }
                        else
                        {
                            DtB.Rows.Add(DtB.NewRow());
                            GrdConvenElem.DataSource = DtB;
                            GrdConvenElem.DataBind();
                            GrdConvenElem.Rows[0].Cells.Clear();
                            GrdConvenElem.Rows[0].Cells.Add(new TableCell());
                            GrdConvenElem.Rows[0].Cells[0].ColumnSpan = DtB.Columns.Count;
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens05PrIng'");
                            foreach (DataRow row in Result)
                            { GrdConvenElem.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                            GrdConvenElem.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
        }
        protected void DdlConvenCodHK_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LimpiarCamposConven();

                if (DdlConvenCodHK.Text.Trim().Equals("0"))
                {
                    BtnConvenProcesar.Enabled = false;
                    GrdConvenElem.DataSource = null;
                    GrdConvenElem.DataBind();
                    return;
                }
                BIndDConvenElem();
                LblTitConvenElemInst.Visible = true;
                BtnConvenProcesar.Enabled = true;
            }
            catch (Exception ex)
            { string vblees = ex.ToString(); }
        }
        protected void BIndDConvenContConHis()
        {
            GrdConvenElemConHis.Visible = true;
            LblTitConvenEleHisManual.Visible = true;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 11,'','','','',@CA,0,0,@ICC,@FI,'01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CA", DdlConvenCodHK.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtConvenFechI.Text.Trim()));
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        { GrdConvenElemConHis.DataSource = DtB; }
                        else
                        { GrdConvenElemConHis.DataSource = null; }
                        GrdConvenElemConHis.DataBind();
                    }
                }
            }
        }
        protected void BtnConvenProcesar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtConvenFechI.Text.Equals("") || TxtConvenFechI.Text.Length > 10 || TxtConvenFechF.Text.Equals("") || TxtConvenFechF.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06PrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplConveniencia, UplConveniencia.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                TxtConvenFechI.Focus();
                return;
            }
            if (DdlConvenCodHK.Text.Equals("0"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12PrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplConveniencia, UplConveniencia.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Se requiere la aeronave
                return;
            }//
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string Mensj = "";
                    int VblTieneHis = 0;
                    string VBQuery = "EXEC Reproceso_Conveniencia @FechaI,@FechaF,@Usu,@CodHK, @ICC";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@FechaI", Convert.ToDateTime(TxtConvenFechI.Text.ToString()));
                            sqlCmd.Parameters.AddWithValue("@FechaF", Convert.ToDateTime(TxtConvenFechF.Text.ToString()));
                            sqlCmd.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@CodHK", DdlConvenCodHK.Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                VblTieneHis = Convert.ToInt32(HttpUtility.HtmlDecode(SDR["TieneHist"].ToString().Trim()));
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();
                            if (!Mensj.Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString(); }
                                ScriptManager.RegisterClientScriptBlock(this.UplConveniencia, UplConveniencia.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString() + "');", true);
                                BIndDConvenContConHis();
                                return;
                            }
                            DdlConvenCodHK.Text = "0";
                            BIndDConvenElem();
                            LimpiarCamposConven();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmControlContadoresGeneral";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "Conveniencia", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
    }
}