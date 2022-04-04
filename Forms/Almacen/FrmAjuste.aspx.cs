using _77NeoWeb.prg;
using ExcelDataReader;
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
    public partial class FrmAjuste : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSDdl = new DataSet();
        DataSet DSSM = new DataSet();
        DataSet DST = new DataSet();// resultado del la validacion de la carga inicial

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
                    Session["MonLcl"] = Cnx.GetMonedLcl();// Moneda Local
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                ViewState["CodReferencia"] = "";
                ModSeguridad();
                BindBDdl("UPD");
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
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;

            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC.Trim());
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }

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
                    LblMvto.Text = bO.Equals("LblMvto") ? bT : LblMvto.Text;
                    LblAlmac.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmac.Text;
                    LblCcost.Text = bO.Equals("LblCcost") ? bT : LblCcost.Text;
                    LblFech.Text = bO.Equals("LblFech") ? bT : LblFech.Text;
                    LblMotvo.Text = bO.Equals("LblObsMst") ? bT : LblMotvo.Text;
                    BtnCargarInvIni.Text = bO.Equals("LblTitCargMasiv") ? bT : BtnCargarInvIni.Text;
                    BtnSubirInventario.Text = bO.Equals("BtnCargaMaxivaMstr") ? bT : BtnSubirInventario.Text;
                    GrdInconsist.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdInconsist.EmptyDataText;
                    GrdInconsist.Columns[0].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdInconsist.Columns[0].HeaderText;
                    GrdInconsist.Columns[3].HeaderText = bO.Equals("LoteMst") ? bT : GrdInconsist.Columns[3].HeaderText;
                    GrdInconsist.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdInconsist.Columns[4].HeaderText;
                    GrdInconsist.Columns[5].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdInconsist.Columns[5].HeaderText;
                    GrdInconsist.Columns[6].HeaderText = bO.Equals("GrdFila") ? bT : GrdInconsist.Columns[6].HeaderText;
                    GrdInconsist.Columns[7].HeaderText = bO.Equals("GrdColum") ? bT : GrdInconsist.Columns[7].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensAjt21'");// Desea subir el inventario a partir de la plantilla seleccionada?
                foreach (DataRow row in Result) { BtnSubirInventario.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_Pantalla_Ajuste 28, @U,'','','','',0, 0,@Idm,@ICC,'01-01-1','01-01-1'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@U", Session["C77U"]);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSDdl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSDdl);
                                    DSDdl.Tables[0].TableName = "Mvto";
                                    DSDdl.Tables[1].TableName = "Alma";
                                    DSDdl.Tables[2].TableName = "CCsto";
                                    DSDdl.Tables[3].TableName = "UltFech";
                                    DSDdl.Tables[4].TableName = "Obser";
                                    DSDdl.Tables[5].TableName = "Obser1";

                                    ViewState["DSDdl"] = DSDdl;
                                }
                            }
                        }
                    }
                }

                DSDdl = (DataSet)ViewState["DSDdl"];

                if (DSDdl.Tables["Mvto"].Rows.Count > 0)
                {
                    DdlMvto.DataSource = DSDdl.Tables[0];
                    DdlMvto.DataTextField = "Descripcion";
                    DdlMvto.DataValueField = "CodTipoMovimiento";
                    DdlMvto.DataBind();
                }
                if (DSDdl.Tables["Alma"].Rows.Count > 0)
                {
                    DdlAlmac.DataSource = DSDdl.Tables[1];
                    DdlAlmac.DataTextField = "NomAlmacen";
                    DdlAlmac.DataValueField = "CodIdAlmacen";
                    DdlAlmac.DataBind();
                }
                if (DSDdl.Tables["CCsto"].Rows.Count > 0)
                {
                    DdlCcost.DataSource = DSDdl.Tables[2];
                    DdlCcost.DataTextField = "Nombre";
                    DdlCcost.DataValueField = "Codcc";
                    DdlCcost.DataBind();
                }
                if (DSDdl.Tables["UltFech"].Rows.Count > 0)
                {
                    TxtFech.Text = Cnx.ReturnFecha(DSDdl.Tables[3].Rows[0]["fechaAjuste1"].ToString().Trim());
                }
                if (DSDdl.Tables["Obser"].Rows.Count > 0)
                {
                    TxtMotvo.Text = DSDdl.Tables[4].Rows[0]["Observacion"].ToString().Trim();
                }
                else if (DSDdl.Tables["Obser1"].Rows.Count > 0)
                {
                    TxtMotvo.Text = DSDdl.Tables[5].Rows[0]["Observacion"].ToString().Trim();
                }

            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
            }
        }
        protected void BtnCargarInvIni_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            if (DdlMvto.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MensAjt01'");//Debe ingresar un tipo de movimiento.
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                return;
            }
            if (DdlAlmac.Text.Equals("0"))
            {
                Result = Idioma.Select("Objeto= 'MstrMens19'");//Debe ingresar el almacén.
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                return;
            }
            if (DdlCcost.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MensAjt02'");//Debe ingresar un centro de costo.
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                return;
            }
            if (TxtFech.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MstrMens08'");//Fecha inválida.
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                TxtFech.Focus(); return;
            }
            string FechI = Cnx.ReturnFecha(Convert.ToString(DateTime.UtcNow));
            string VbMnsj = Cnx.ValidarFechas2(TxtFech.Text.Trim(), FechI, 2);
            if (!VbMnsj.ToString().Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { VbMnsj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                Page.Title = ViewState["PageTit"].ToString(); TxtFech.Focus();
                return;
            }

            if (TxtMotvo.Text.Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MstrMens22'");//Debe ingresar una observación.
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                TxtMotvo.Focus(); return;
            }
            if (FUpCargaInvIni.Visible == false)
            { FUpCargaInvIni.Visible = true; LblTitInconsist.Text = ""; GrdInconsist.Visible = true; GrdInconsist.DataSource = null; GrdInconsist.DataBind(); }
            else
            {
                try
                {
                    Result = Idioma.Select("Objeto= 'MensAjt23'");
                    foreach (DataRow row in Result) { BtnCargarInvIni.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                    DataTable DT = new DataTable();
                    if (FUpCargaInvIni.Visible == false) { FUpCargaInvIni.Visible = true; }
                    else
                    {
                        if (FUpCargaInvIni.HasFile == true)
                        {
                            //BtnCargarInvIni.CssClass = "btn btn-success";
                            string FolderPath;
                            string FileName = Path.GetFileName(FUpCargaInvIni.PostedFile.FileName);
                            string VblExt = Path.GetExtension(FUpCargaInvIni.PostedFile.FileName);
                            if (Cnx.GetProduccion().Trim().Equals("Y")) { FolderPath = ConfigurationManager.AppSettings["FolderPath"]; }//Azure
                            else { FolderPath = ConfigurationManager.AppSettings["FoldPathLcl"]; }

                            VblExt = VblExt.Substring(VblExt.LastIndexOf(".") + 1).ToLower();
                            string[] formatos = new string[] { "xls", "xlsx" };
                            if (Array.IndexOf(formatos, VblExt) < 0)
                            {
                                BtnCargarInvIni.OnClientClick = "";
                                Result = Idioma.Select("Objeto= 'RteMens40'");//Archivo inválido
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                                return;
                            }
                            string FilePath = FolderPath + FileName;
                            FUpCargaInvIni.SaveAs(FilePath);
                            Import(FilePath, VblExt);
                            FUpCargaInvIni.Visible = false;
                        }
                        else
                        {
                            Result = Idioma.Select("Objeto= 'MstrMens34'");//Debe seleccionar un archivo.
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            return;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    BtnCargarInvIni.OnClientClick = "";
                    Result = Idioma.Select("Objeto= 'MensErrMod'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Cargar Masiva Ajuste", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
                FUpCargaInvIni.Visible = false;
            }
        }
        protected void Import(string FilePath, string Extension)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbIP = Request.ServerVariables["REMOTE_ADDR"]; // IP
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName; //Nombre maquina en internet toma es el nombre del proveedor 
            DataRow[] Result;

            FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader ExcelReader;

            ExcelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

            //// para que tome la primera fila como titulo de campos
            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                { UseHeaderRow = true }
            };
            var dataSet = ExcelReader.AsDataSet(conf);
            DataTable DT = dataSet.Tables[0];
            if (DT.Rows.Count > 0)
            {
                DataRow[] DR = DT.Select("ID_UB = 0");
                if (Cnx.ValidaDataRowVacio(DR))
                {
                    DataTable DTV = DR.CopyToDataTable();
                    if (DTV.Rows.Count != DT.Rows.Count)
                    {
                        Result = Idioma.Select("Objeto= 'MensAjt24'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Todos los registros deben contener valor "0" en el campo "ID_UB"
                    }
                }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "AjusteSP";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                    {
                        try
                        {
                            string PMensj = "";
                            string borr = VbPC.Trim() + "-" + Session["C77U"].ToString().Trim();
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs1 = SC.Parameters.AddWithValue("@Ajuste", DT);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdCia", Session["!dC!@"]);
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@NIT", Session["Nit77Cia"]);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@NomMaquina", VbPC.Trim() + "-" + Session["C77U"].ToString().Trim());
                            SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@FecAjuste", Convert.ToDateTime(TxtFech.Text));
                            SqlParameter Prmtrs7 = SC.Parameters.AddWithValue("@CodMvto", DdlMvto.Text.Trim());
                            SqlParameter Prmtrs8 = SC.Parameters.AddWithValue("@CodAlma", DdlAlmac.SelectedItem.Text.Trim());
                            SqlParameter Prmtrs9 = SC.Parameters.AddWithValue("@CCost", DdlCcost.Text.Trim());
                            SqlParameter Prmtrs10 = SC.Parameters.AddWithValue("@Mtv", TxtMotvo.Text.Trim());
                            Prmtrs1.SqlDbType = SqlDbType.Structured;
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DST = new DataSet())
                                {
                                    SDA.SelectCommand = SC; SDA.Fill(DST); ViewState["DST"] = DST;
                                    PMensj = DST.Tables[1].Rows[0]["Estado"].ToString().Trim();
                                    if (!PMensj.Trim().Equals(""))
                                    {
                                        Result = Idioma.Select("Objeto= '" + DST.Tables[1].Rows[0]["Mensj"].ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { PMensj = row["Texto"].ToString().Trim(); }//LblTitIncosistnc.Text = PMensj; 

                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + PMensj + "');", true);
                                        LblTitInconsist.Text = PMensj;
                                        GrdInconsist.DataSource = DST.Tables[0]; GrdInconsist.DataBind();
                                        BtnCargarInvIni.OnClientClick = "";
                                        return;
                                    }
                                    PMensj = DST.Tables[1].Rows[0]["Mensj"].ToString().Trim();
                                    Result = Idioma.Select("Objeto= '" + PMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { LblTitInconsist.Text = row["Texto"].ToString().Trim(); }//LblTitIncosistnc.Text = PMensj; 
                                    GrdInconsist.DataSource = DST.Tables[0]; GrdInconsist.DataBind();
                                    BtnCargarInvIni.Visible = false; BtnSubirInventario.Visible = true; DdlAlmac.Enabled = false;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            BtnCargarInvIni.OnClientClick = "";
                            Result = Idioma.Select("Objeto= 'MensAjt03'");// Inconvenientes con el archivos, verifique si tiene la estructura requerida.
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Import Detalle Sol Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }

                }
            }

        }
        protected void BtnSubirInventario_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction SlqTr = sqlCon.BeginTransaction())
                {
                    string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;

                    string VBQuery = "EXEC SP_Pantalla_Ajuste 29, @NM, @Us,@CC,'', @Obsv,0, 0,0,@ICC, @FA,'01-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, SlqTr))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@NM", VbPC.Trim() + "-" + Session["C77U"].ToString().Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Obsv", TxtMotvo.Text.Trim());
                            SC.Parameters.AddWithValue("@CC", DdlCcost.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.Parameters.AddWithValue("@FA", Convert.ToDateTime(TxtFech.Text));

                            SC.ExecuteNonQuery();
                            SlqTr.Commit();
                            BtnCargarInvIni.Visible = true; BtnSubirInventario.Visible = false; GrdInconsist.Visible = false;
                            BtnCargarInvIni.OnClientClick = "";
                            Result = Idioma.Select("Objeto= 'MensAjt20'");
                            foreach (DataRow row in Result)
                            { LblTitInconsist.Text = row["Texto"].ToString().Trim(); }//Se realiza la carga del inventario correctamente.
                            DdlAlmac.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            SlqTr.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrIng'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + "Inventario Inicial Carga Masiva", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
    }
}