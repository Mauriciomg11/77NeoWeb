using _77NeoWeb.prg;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmElemento : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DST = new DataSet();
        private DateTime FechaD = DateTime.Today;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "Configuración de Elementos";
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr(); ;
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                Session["VldrElem"] = "S";
                ViewState["CodBodegaE"] = "";
                ViewState["IdentificadorE"] = "";
                ViewState["PondMatSN"] = "N";
                ViewState["PondCompSN"] = "N";
                ViewState["FechaVenceE"] = "";
                ViewState["PNAntEle"] = "";
                ViewState["SNAntEle"] = "";
                ViewState["GrupoEle"] = "";
                ViewState["Accion"] = "";
                ModSeguridad();
                ActivarCampos(false, false, "");
                ActivarBotones(true, false, false, false, true);
                //BindDataDdl("");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblConsMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmElemento.aspx");

            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { ViewState["VblConsMS"] = 0; }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string TxQry = "EXEC SP_ConfiguracionV2_ 19,'PONDERADO','PONDERADO','','','" + Session["Nit77Cia"].ToString() + "',1,2,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                sqlCon.Open();
                SqlDataReader Regs = Comando.ExecuteReader();
                while (Regs.Read())
                {
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    if (VbCaso == 1 && VbAplica.Equals("S"))
                    {
                        // Material Serializado
                        ViewState["PondMatSN"] = "S";
                    }
                    if (VbCaso == 2 && VbAplica.Equals("S"))
                    {
                        // Componenente Serializado
                        ViewState["PondCompSN"] = "S";
                    }
                }
            }
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

                    TitForm.Text = bO.Equals("LblTituloElem") ? bT : TitForm.Text;
                    LblCodigo.Text = bO.Equals("LblCodigo") ? bT + ":" : LblCodigo.Text;
                    LblReferenc.Text = bO.Equals("LblReferenc") ? bT + ":" : LblReferenc.Text;
                    LblReferenc.Text = bO.Equals("LblReferenc") ? bT + ":" : LblReferenc.Text;
                    LblDescr.Text = bO.Equals("LblDescr") ? bT + ":" : LblDescr.Text;
                    LblLote.Text = bO.Equals("LblLote") ? bT + ":" : LblLote.Text;
                    LblFechRec.Text = bO.Equals("LblFechRec") ? bT + ":" : LblFechRec.Text;
                    LblUndMed.Text = bO.Equals("LblUndMed") ? bT + ":" : LblUndMed.Text;
                    LblGrupo.Text = bO.Equals("LblGrupo") ? bT + ":" : LblGrupo.Text;
                    LblAta.Text = bO.Equals("LblAta") ? bT + ":" : LblAta.Text;
                    LblPosic.Text = bO.Equals("LblPosic") ? bT + ":" : LblPosic.Text;
                    LblAerona.Text = bO.Equals("LblAerona") ? bT + ":" : LblAerona.Text;
                    LblMayor.Text = bO.Equals("LblMayor") ? bT + ":" : LblMayor.Text;
                    LblUbicTec.Text = bO.Equals("LblUbicTec") ? bT + ":" : LblUbicTec.Text;
                    LblSheLif.Text = bO.Equals("LblSheLif") ? bT + ":" : LblSheLif.Text;
                    LblEstad.Text = bO.Equals("LblEstad") ? bT + ":" : LblEstad.Text;
                    CkbApu.Text = bO.Equals("CkbApu") ? "&nbsp" + bT : CkbApu.Text;
                    CkbMot.Text = bO.Equals("CkbMot") ? "&nbsp" + bT : CkbMot.Text;
                    CkbConsig.Text = bO.Equals("CkbConsig") ? " " + bT : CkbConsig.Text;
                    RdbActivo.Text = bO.Equals("RdbActivo") ? "&nbsp" + bT + "&nbsp" : RdbActivo.Text;
                    RdbInactivo.Text = bO.Equals("RdbInactivo") ? "&nbsp" + bT : RdbInactivo.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    LblTitContAsig.Text = bO.Equals("LblTitContAsig") ? bT : LblTitContAsig.Text;
                    GrdCont.Columns[0].HeaderText = bO.Equals("GrdNom") ? bT : GrdCont.Columns[0].HeaderText;
                    GrdCont.Columns[1].HeaderText = bO.Equals("GrdContad") ? bT : GrdCont.Columns[1].HeaderText;
                    GrdCont.Columns[2].HeaderText = bO.Equals("GrdValor") ? bT : GrdCont.Columns[2].HeaderText;
                    GrdCont.EmptyDataText = bO.Equals("GtdTextSin") ? bT : GrdCont.EmptyDataText;
                    //**************************************Busq *****************************************************
                    LblTitOpcBusq.Text = bO.Equals("LblTitOpcBusq") ? bT : LblTitOpcBusq.Text;
                    RdbBusqDesc.Text = bO.Equals("LblDescr") ? "&nbsp" + bT : RdbBusqDesc.Text;
                    RdbBusqRef.Text = bO.Equals("LblReferenc") ? "&nbsp" + bT : RdbBusqRef.Text;
                    LblBusqueda.Text = bO.Equals("Busqueda") ? bT : LblBusqueda.Text;
                    if (bO.Equals("placeholderDC"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsultar.ToolTip;
                    IbtCerrar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrar.ToolTip;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("LblReferenc") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[4].HeaderText = bO.Equals("LblLote") ? bT : GrdBusq.Columns[4].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("LblDescr") ? bT : GrdBusq.Columns[5].HeaderText;
                    GrdBusq.Columns[6].HeaderText = bO.Equals("LblFechRec") ? bT : GrdBusq.Columns[6].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("LblUndMed") ? bT : GrdBusq.Columns[7].HeaderText;
                    GrdBusq.Columns[8].HeaderText = bO.Equals("GrdCodGrup") ? bT : GrdBusq.Columns[8].HeaderText;
                    GrdBusq.Columns[9].HeaderText = bO.Equals("LblGrupo") ? bT : GrdBusq.Columns[9].HeaderText;
                    GrdBusq.Columns[11].HeaderText = bO.Equals("LblPosic") ? bT : GrdBusq.Columns[11].HeaderText;
                    GrdBusq.Columns[12].HeaderText = bO.Equals("LblAerona") ? bT : GrdBusq.Columns[12].HeaderText;
                    GrdBusq.Columns[13].HeaderText = bO.Equals("LblMayor") ? bT : GrdBusq.Columns[13].HeaderText;
                    GrdBusq.Columns[14].HeaderText = bO.Equals("LblUbicTec") ? bT : GrdBusq.Columns[14].HeaderText;
                    GrdBusq.Columns[15].HeaderText = bO.Equals("LblSheLif") ? bT : GrdBusq.Columns[15].HeaderText;
                    GrdBusq.Columns[16].HeaderText = bO.Equals("LblEstad") ? bT : GrdBusq.Columns[16].HeaderText;
                    GrdBusq.Columns[17].HeaderText = bO.Equals("GrdFecVen") ? bT : GrdBusq.Columns[17].HeaderText;
                    GrdBusq.Columns[19].HeaderText = bO.Equals("CkbMot") ? bT : GrdBusq.Columns[19].HeaderText;
                    GrdBusq.Columns[20].HeaderText = bO.Equals("CkbConsig") ? bT : GrdBusq.Columns[20].HeaderText;
                    GrdBusq.Columns[21].HeaderText = bO.Equals("RdbActivo") ? bT : GrdBusq.Columns[21].HeaderText;
                    GrdBusq.Columns[22].HeaderText = bO.Equals("LblCodigo") ? bT : GrdBusq.Columns[22].HeaderText;
                    GrdBusq.Columns[23].HeaderText = bO.Equals("GrdIdent") ? bT : GrdBusq.Columns[23].HeaderText;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            if (ViewState["IdentificadorE"].Equals("SN"))
            {
                switch (DdlGrupo.SelectedValue)
                {
                    case "01":
                        if (ViewState["PondMatSN"].Equals("N"))
                        { DdlPN.Enabled = Edi; }
                        TxtSN.Enabled = Edi;
                        break;
                    case "02":
                        if (ViewState["PondCompSN"].Equals("N"))
                        { DdlPN.Enabled = Edi; }
                        TxtSN.Enabled = Edi;
                        break;
                    case "03":
                        DdlPN.Enabled = Edi;
                        TxtSN.Enabled = Edi;
                        RdbActivo.Enabled = Edi;
                        RdbInactivo.Enabled = Edi;
                        break;
                }
            }
            if (ViewState["FechaVenceE"].Equals("S"))
            { IbtFechaI.Enabled = Edi; }
        }
        protected void TraerDatos(string Accion)
        {
            if (TxtCod.Text.ToString() == string.Empty) { return; }
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Elemento 12, @CE,'','','PN',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@CE", TxtCod.Text.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DST = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DST);
                                DST.Tables[0].TableName = "Datos";
                                DST.Tables[1].TableName = "HKConSubPT";
                                DST.Tables[2].TableName = "Contadores";
                                DST.Tables[3].TableName = "PN";

                                ViewState["DST"] = DST;
                            }
                        }
                    }
                }
            }
            DST = (DataSet)ViewState["DST"];

            string VbCodAnt, TxtFecha;

            VbCodAnt = DdlGrupo.Text.Trim();
            DdlGrupo.DataSource = DST.Tables[1];
            DdlGrupo.DataTextField = "Descripcion";
            DdlGrupo.DataValueField = "CodTipoElemento";
            DdlGrupo.DataBind();
            DdlGrupo.Text = VbCodAnt;

            if (DST.Tables[0].Rows.Count > 0)
            {
                TxtRef.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["CodReferencia"].ToString().Trim());
                DdlPN.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["PN"].ToString().Trim());
                ViewState["PN"] = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["PN"].ToString().Trim());
                BindDataDdl("SEL");
                TxtSN.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Sn"].ToString().Trim());
                ViewState["PNAntEle"] = DdlPN.Text.Trim();
                ViewState["SNAntEle"] = TxtSN.Text.Trim();
                TxtLote.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["NumLote"].ToString().Trim());
                TxtDescr.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Descripcion"].ToString().Trim());
                TxtFecha = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["FechaRecibo"].ToString().Trim());
                if (!TxtFecha.Trim().Equals(""))
                {
                    FechaD = Convert.ToDateTime(TxtFecha);
                    TxtFecRec.Text = String.Format("{0:yyyy-MM-dd}", FechaD);
                }
                else { TxtFecRec.Text = ""; }
                TxtUndMed.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["CodUnidadMedida"].ToString().Trim());
                DdlGrupo.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["CodGrupo"].ToString().Trim());
                ViewState["GrupoEle"] = DdlGrupo.Text.Trim();
                TxtAta.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["ATA"].ToString().Trim());
                txtPosic.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["PosicionMotor"].ToString().Trim());
                TxtHK.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Aeronave"].ToString().Trim());
                TxtMayor.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Mayor"].ToString().Trim());
                TxtUbiTec.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["CodUbicacionFisica"].ToString().Trim());
                TxtFecha = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["FechaShelfLife"].ToString().Trim());
                if (!TxtFecha.Trim().Equals(""))
                {
                    FechaD = Convert.ToDateTime(TxtFecha);
                    TxtFecShelfLife.Text = String.Format("{0:dd/MM/yyyy}", FechaD);
                }
                else { TxtFecShelfLife.Text = ""; }
                TxtEstado.Text = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Estado"].ToString().Trim());
                ViewState["FechaVenceE"] = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["FechaVence"].ToString().Trim());
                CkbApu.Checked = DST.Tables[0].Rows[0]["APU"].ToString().Trim().Equals("S") ? true : false;
                CkbMot.Checked = DST.Tables[0].Rows[0]["Motor"].ToString().Trim().Equals("S") ? true : false;
                CkbConsig.Checked = DST.Tables[0].Rows[0]["Consignacion"].ToString().Trim().Equals("S") ? true : false;
                RdbActivo.Checked = DST.Tables[0].Rows[0]["Activo"].ToString().Trim().Equals("S") ? true : false;
                RdbInactivo.Checked = DST.Tables[0].Rows[0]["Activo"].ToString().Trim().Equals("N") ? true : false;
                ViewState["CodBodegaE"] = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["CodBodega"].ToString().Trim());
                ViewState["IdentificadorE"] = HttpUtility.HtmlDecode(DST.Tables[0].Rows[0]["Identificador"].ToString().Trim());
                BIndDataCntdr(TxtCod.Text);
            }
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnModificar.Enabled = Md;
            BtnConsultar.Enabled = Otr;
            /* BtnIngresar.Enabled = In;
             BtnEliminar.Enabled = El;
             BtnInformes.Enabled = Otr;
             GrdMan.Enabled = Otr;
             GrdPN.Enabled = Otr;
             GrdCont.Enabled = Otr;
             BindDataAll(TxtCod.Text, "");*/
        }
        protected void AsignarValores()
        {
            Session["VldrElem"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlPN.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Elem'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar el P/N')", true);
                Session["VldrElem"] = "N";
                return;
            }
            if (ViewState["IdentificadorE"].Equals("SN") && TxtSN.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Elem'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una S/N')", true);
                Session["VldrElem"] = "N";
                return;
            }
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = "EXEC SP_PANTALLA_Elemento 11, @CE,@PN,@SN,'VALIDA',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                SC.Parameters.AddWithValue("@CE", TxtCod.Text.Trim());
                SC.Parameters.AddWithValue("@PN", DdlPN.Text.Trim());
                SC.Parameters.AddWithValue("@SN", TxtSN.Text.Trim());
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    string Mensj = SDR["Mensj"].ToString().Trim();
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    Session["VldrElem"] = "N";
                    return;
                }
            }
        }
        protected void BindDataDdl(string Accion)
        {
            DST = (DataSet)ViewState["DST"];
            string VbPrmtr = "";
            if (!Accion.Equals("")) { VbPrmtr = TxtRef.Text; }
            DdlPN.DataSource = DST.Tables[3];
            DdlPN.DataTextField = "PN";
            DdlPN.DataValueField = "Codigo";
            DdlPN.DataBind();
            DdlPN.Text = ViewState["PN"].ToString().Trim();
        }
        protected void BIndDataBusq(string Prmtr)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VblOpc = "";

                if (RdbBusqPN.Checked == true)
                {
                    VblOpc = "PN";
                }
                if (RdbBusqDesc.Checked == true)
                {
                    VblOpc = "DESC";
                }
                if (RdbBusqRef.Checked == true)
                {
                    VblOpc = "REF";
                }
                if (RdbBusqSN.Checked == true)
                {
                    VblOpc = "SN";
                }

                if (!VblOpc.Equals(""))
                {
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand("EXEC SP_PANTALLA_Elemento 8,@Prmtr,'','',@Op,0,0,@Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'", sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Prmtr", Prmtr.Trim());
                        SC.Parameters.AddWithValue("@Op", VblOpc.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter DAB = new SqlDataAdapter(SC);
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                        else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void BIndDataCntdr(string CodElem)
        {
            DST = (DataSet)ViewState["DST"];
            if (DST.Tables[2].Rows.Count > 0) { GrdCont.DataSource = DST.Tables[2]; GrdCont.DataBind(); }
            else { GrdCont.DataSource = null; GrdCont.DataBind(); }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (RdbInactivo.Checked == true)
            {
                if (DdlGrupo.SelectedValue.Equals("03"))
                {
                    string vlabee = ViewState["CodBodegaE"].ToString();
                    if (ViewState["CodBodegaE"].Equals("") || ViewState["CodBodegaE"].Equals("PREC-") || ViewState["CodBodegaE"].Equals("--") || ViewState["CodBodegaE"].Equals("Limbo") || ViewState["CodBodegaE"].Equals("BAJA"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens01Elem'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// El elemento se encuentra inactivo
                        return;
                    }
                }
                else
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens01Elem'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// El elemento se encuentra inactivo')", true);
                    return;
                }
            }
            if (ViewState["Accion"].ToString().Trim().Equals(""))
            {
                if (!ViewState["FechaVenceE"].Equals("S"))
                {
                    if (!ViewState["IdentificadorE"].Equals("SN"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens02Elem'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Sólo aplica a elementos configurados con fecha de vencimiento')", true);
                        return;
                    }
                }
                TraerDatos("SEL");
                ActivarBotones(false, true, false, false, false);
                ViewState["Accion"] = "Aceptar";
                DataRow[] Result1 = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result1)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//Aceptar
                ActivarCampos(false, true, "Modificar");
                Result1 = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result1)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//Desea realizar la actualización
                BindDataDdl("UPDATE");
                DdlPN.Text = ViewState["PNAntEle"].ToString();
                DdlGrupo.Text = ViewState["GrupoEle"].ToString();
            }
            else
            {
                AsignarValores();
                if (Session["VldrElem"].ToString() == "N") { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasIngenieria 3,'{0}',@PN,@SN,'{1}','{2}','','','','',@Act,0,0,0,0,@ICC,@FecSL,'02-01-1','03-01-1'",
                            TxtCod.Text, Session["C77U"].ToString(), TxtRef.Text);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PN", DdlPN.SelectedValue);
                                SC.Parameters.AddWithValue("@SN", TxtSN.Text.Trim());
                                SC.Parameters.AddWithValue("@Act", RdbActivo.Checked == true ? 1 : 0);
                                SC.Parameters.AddWithValue("@FecSL", TxtFecShelfLife.Text);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                ViewState["Accion"] = "";
                                DataRow[] Result1 = Idioma.Select("Objeto= 'BtnModificar'");
                                foreach (DataRow row in Result1)
                                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//Modificar
                                ActivarBotones(true, true, true, true, true);
                                ActivarCampos(false, false, "");
                                BtnModificar.OnClientClick = "";
                                ViewState["PNAntEle"] = DdlPN.Text.Trim();
                                ViewState["SNAntEle"] = TxtSN.Text.Trim();
                                ViewState["GrupoEle"] = DdlGrupo.Text.Trim();
                                BIndDataCntdr(TxtCod.Text);
                                BindDataDdl("");
                                DdlPN.Text = ViewState["PNAntEle"].ToString();
                                DdlGrupo.Text = ViewState["GrupoEle"].ToString();
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();

                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmElemento", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { BIndDataBusq("77NEO"); PnlCampos.Visible = false; PnlBusq.Visible = true; RdbBusqPN.Checked = true; }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BIndDataBusq(TxtBusqueda.Text); }
        protected void IbtCerrar_Click(object sender, ImageClickEventArgs e)
        { PnlBusq.Visible = false; PnlCampos.Visible = true; }
        protected void IbtFechaI_Click(object sender, ImageClickEventArgs e)
        {
            /* BtnConsultar.Visible = false;
             Session["CalP"] = "I";
             if (TxtFecShelfLife.Text != String.Empty)
             {
                 if (TxtFecShelfLife.Text.Equals("1900-01-01"))
                 {
                     Calendar1.TodaysDate = DateTime.Today;
                 }
                 else { Calendar1.TodaysDate = Convert.ToDateTime(TxtFecShelfLife.Text); }
             }
             else
             {

                 Calendar1.TodaysDate = DateTime.Today;
             }

             if (Calendar1.Visible == false)
             {
                 Calendar1.Visible = true;
             }
             else
             {
                 Calendar1.Visible = false;
                 if ((int)ViewState["VblConsMS"] == 1)
                 {
                     BtnConsultar.Visible = true;
                 }
                 if ((int)ViewState["VblModMS"] == 1)
                 {
                     BtnModificar.Visible = true;
                 }
             }
             Calendar1.Attributes.Add("style", "position:absolute");*/
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            /* DateTime today = Calendar1.SelectedDate;

             string VbVcal = Session["CalP"].ToString();
             if (VbVcal == "I")
             {
                 TxtFecShelfLife.Text = String.Format("{0:yyyy-MM-dd}", today);
             }

             Calendar1.Visible = false;
             if ((int)ViewState["VblConsMS"] == 1)
             {
                 BtnConsultar.Visible = true;
             }
             if ((int)ViewState["VblModMS"] == 1)
             {
                 BtnModificar.Visible = true;
             }*/
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Style.Value = "min-width:100px;";
                e.Row.Cells[2].Style.Value = "min-width:150px;";
                e.Row.Cells[3].Style.Value = "min-width:150px;";
                e.Row.Cells[4].Style.Value = "min-width:150px;";
                e.Row.Cells[5].Style.Value = "min-width:350px;";
                e.Row.Cells[10].Style.Value = "min-width:300px;";
            }
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtCod.Text = GrdBusq.DataKeys[this.GrdBusq.SelectedIndex][0].ToString();
            TraerDatos("UPD");
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
            ActivarBotones(true, true, true, true, true);
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdBusq.PageIndex = e.NewPageIndex; BIndDataBusq(TxtBusqueda.Text); }
    }
}