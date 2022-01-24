using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgManto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace _77NeoWeb.Forms.Configuracion.UbicacionGeograf
{
    public partial class FrmUbicaGeograf : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
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
                ModSeguridad();
                BindBDdl("UPD");
                ViewState["Accion"] = "";
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            //ViewState["VblImpMS"] = 1;
            //ViewState["VblCE1"] = 1;
            //ViewState["VblCE2"] = 1;
            //ViewState["VblCE3"] = 1;
            //ViewState["VblCE4"] = 1;
            //ViewState["VblCE5"] = 1;
            //ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            //if (ClsP.GetConsultar() == 0) { }
            //if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            //if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0;}
            //if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            //if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }// 
            //if (ClsP.GetCE4() == 0) { }
            //if (ClsP.GetCE5() == 0) { }
            //if (ClsP.GetCE6() == 0) { }
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
                    LblBusq.Text = bO.Equals("LblBusq") ? bT : LblBusq.Text;
                    LblCod.Text = bO.Equals("LblCod") ? bT : LblCod.Text;
                    LblNombre.Text = bO.Equals("LblNombre") ? bT : LblNombre.Text;
                    LblTipoUbc.Text = bO.Equals("LblTipoUbc") ? bT : LblTipoUbc.Text;
                    LblUbicaSupr.Text = bO.Equals("LblUbicaSupr") ? bT : LblUbicaSupr.Text;
                    LblVlrTasa.Text = bO.Equals("LblVlrTasa") ? bT : LblVlrTasa.Text;
                    CkbActivo.Text = bO.Equals("CkbActivo") ? "&nbsp" + bT : CkbActivo.Text;
                    CkbRutaFrec.Text = bO.Equals("CkbRutaFrec") ? "&nbsp" + bT : CkbRutaFrec.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_TablasGeneral 17,'','','','','','','','','',0,0,0,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Consul";
                                DSTDdl.Tables[1].TableName = "TipoUbGeo";
                                DSTDdl.Tables[2].TableName = "UbicSup";
                                DSTDdl.Tables[3].TableName = "Datos";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            string VbCodAnt = "";

            VbCodAnt = DdlBusq.Text.Trim();
            DdlBusq.DataSource = DSTDdl.Tables[0];
            DdlBusq.DataTextField = "Nombre";
            DdlBusq.DataValueField = "IdUbicaGeogr";
            DdlBusq.DataBind();
            DdlBusq.Text = VbCodAnt;

            VbCodAnt = DdlTipoUbc.Text.Trim();
            DdlTipoUbc.DataSource = DSTDdl.Tables[1];
            DdlTipoUbc.DataTextField = "Descripcion";
            DdlTipoUbc.DataValueField = "TipoUbicaGeogr";
            DdlTipoUbc.DataBind();
            DdlTipoUbc.Text = VbCodAnt;


            VbCodAnt = DdlUbicaSupr.Text.Trim();
            DdlUbicaSupr.DataSource = DSTDdl.Tables[2];
            DdlUbicaSupr.DataTextField = "Nombre";
            DdlUbicaSupr.DataValueField = "CodUbicaGeogr";
            DdlUbicaSupr.DataBind();
            DdlUbicaSupr.Text = VbCodAnt;
        }
        protected void LimpiarCampos(string Accion)
        {
            TxtCod.Text = ""; TxtNombre.Text = ""; DdlTipoUbc.Text = ""; DdlUbicaSupr.Text = ""; TxtVlrTasa.Text = "0"; CkbRutaFrec.Checked = false;
            if (Accion.Trim().Equals("INSERT")) { CkbActivo.Checked = true; }
            else { CkbActivo.Checked = false; }
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                DataRow[] Result;
                string VbDatoRequerido = "";
                Result = Idioma.Select("Objeto= 'MensCampoReq'");
                foreach (DataRow row in Result)
                { VbDatoRequerido = row["Texto"].ToString(); }// Campo Requerdio.

                ViewState["Validar"] = "S";
                if (TxtCod.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbDatoRequerido + "');", true);
                    ViewState["Validar"] = "N"; TxtCod.Focus(); return;
                }
                if (TxtNombre.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbDatoRequerido + "');", true);
                    ViewState["Validar"] = "N"; TxtNombre.Focus(); return;
                }
                if (DdlTipoUbc.Text.Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'Mens01UG'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La ubicación es requerida.
                    ViewState["Validar"] = "N"; return;
                }
                if (DdlUbicaSupr.Text.Trim().Equals("") && !DdlTipoUbc.Text.Trim().Equals("01"))
                {
                    DSTDdl = (DataSet)ViewState["DSTDdl"];

                    Result = DSTDdl.Tables[0].Select("Nombre LIKE '%%'");
                    if (Result.Length != 1)
                    {
                        Result = Idioma.Select("Objeto= 'Mens02UG'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La ubicación superior es requerida.
                        ViewState["Validar"] = "N"; return;
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Validar Campos UbicaGeografica", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            TxtCod.Enabled = Ing; TxtNombre.Enabled = Edi; DdlTipoUbc.Enabled = Edi; DdlUbicaSupr.Enabled = Edi; TxtVlrTasa.Enabled = Edi; CkbRutaFrec.Enabled = Edi;
            if (accion.Trim().Equals("UPDATE")) { CkbActivo.Enabled = Edi; }
        }
        protected void Traerdatos(string Prmtr)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result = DSTDdl.Tables[3].Select("IdUbicaGeogr = " + Prmtr.Trim());
            foreach (DataRow SDR in Result)
            {
                TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodUbicaGeogr"].ToString().Trim());
                TxtNombre.Text = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                DdlTipoUbc.Text = HttpUtility.HtmlDecode(SDR["CodTipoUbicaGeogr"].ToString().Trim());
                DdlUbicaSupr.Text = HttpUtility.HtmlDecode(SDR["CodUbicaGeoSup"].ToString().Trim());
                TxtVlrTasa.Text = HttpUtility.HtmlDecode(SDR["VlorTasa"].ToString().Trim());
                CkbActivo.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["Activo"].ToString().Trim()));
                CkbRutaFrec.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["RutaFrecuente"].ToString().Trim()));
            }
            /* Idioma = (DataTable)ViewState["TablaIdioma"];

             Cnx.SelecBD();
             using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
             {
                 Cnx2.Open();
                 string LtxtSql = "EXEC SP_TablasGeneral 13,@Prmtr,'','','','','','','','ReadUbicGeog',0,0,0,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                 SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                 SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                 SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                 SqlDataReader SDR = SC.ExecuteReader();
                 if (SDR.Read())
                 {
                    
                 }
                 SDR.Close();
                 Cnx2.Close();
             }*/
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        { Traerdatos(DdlBusq.Text.Trim()); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false);

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCampos("INSERT");
                    ActivarCampos(true, true, "Ingresar");

                    DdlBusq.Text = "0";
                    DdlBusq.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    ValidarCampos("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }
                    double VblVlrTasa = 0;
                    string VbIdCia = Session["!dC!@"].ToString();
                    if (TxtVlrTasa.Text.Trim().Equals("")) { VblVlrTasa = Convert.ToDouble(0); }
                    else { VblVlrTasa = Convert.ToDouble(TxtVlrTasa.Text.Trim()); }
                    List<ClsUbicaGeograf> ObjUbGeo = new List<ClsUbicaGeograf>();
                    var TypUbGeo = new ClsUbicaGeograf()
                    {

                        IdUbicaGeogr = 0,
                        CodUbicaGeogr = TxtCod.Text.Trim().ToUpper(),
                        Nombre = TxtNombre.Text.Trim(),
                        CodUbicaGeoSup = DdlUbicaSupr.Text.Trim(),
                        CodTipoUbicaGeogr = DdlTipoUbc.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        VlorTasa = VblVlrTasa,
                        Activa = CkbActivo.Checked == true ? 1 : 0,
                        RutaFrecuente = CkbRutaFrec.Checked == true ? 1 : 0,
                        IdConfigCia = Convert.ToInt32(VbIdCia),//(int)Session["!dC!@"],
                        Accion = "INSERT",
                    };
                    ObjUbGeo.Add(TypUbGeo);
                    ClsUbicaGeograf ClsUbicaGeograf = new ClsUbicaGeograf();
                    ClsUbicaGeograf.Alimentar(ObjUbGeo);
                    string Mensj = ClsUbicaGeograf.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "Ingresar");
                    DdlBusq.Enabled = true;

                    BindBDdl("UPD");
                    DdlBusq.Text = ClsUbicaGeograf.GetId().ToString();
                    Traerdatos(DdlBusq.Text.Trim());
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR UbicaGeografica", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                DataRow[] Result;

                if (TxtCod.Text.Equals("") || DdlBusq.Text.Trim().Equals("0"))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false);
                    Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(true, true, "UPDATE");
                    DdlBusq.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                }
                else
                {
                    ValidarCampos("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    double VblVlrTasa = 0;
                    if (TxtVlrTasa.Text.Trim().Equals("")) { VblVlrTasa = Convert.ToDouble(0); }
                    else { VblVlrTasa = Convert.ToDouble(TxtVlrTasa.Text.Trim()); }
                    string VbIdCia = Session["!dC!@"].ToString();
                    List<ClsUbicaGeograf> ObjUbGeo = new List<ClsUbicaGeograf>();
                    var TypUbGeo = new ClsUbicaGeograf()
                    {
                        IdUbicaGeogr = Convert.ToInt32(DdlBusq.Text.Trim()),
                        CodUbicaGeogr = TxtCod.Text.Trim(),
                        Nombre = TxtNombre.Text.Trim(),
                        CodUbicaGeoSup = DdlUbicaSupr.Text.Trim(),
                        CodTipoUbicaGeogr = DdlTipoUbc.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        VlorTasa = VblVlrTasa,
                        Activa = CkbActivo.Checked == true ? 1 : 0,
                        RutaFrecuente = CkbRutaFrec.Checked == true ? 1 : 0,
                        IdConfigCia = Convert.ToInt32(VbIdCia),
                        Accion = "UPDATE",
                    };
                    ObjUbGeo.Add(TypUbGeo);
                    ClsUbicaGeograf ClsUbicaGeograf = new ClsUbicaGeograf();
                    ClsUbicaGeograf.Alimentar(ObjUbGeo);
                    string Mensj = ClsUbicaGeograf.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampos(false, false, "UPDATE");
                    DdlBusq.Enabled = true;
                    BindBDdl("UPD");
                    Traerdatos(DdlBusq.Text);
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//inconvenientes en la modificacion
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR UbicacionGeografica", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtCod.Text.Equals("") || DdlBusq.Text.Trim().Equals("0"))
                { return; }

                double VblVlrTasa = 0;
                if (TxtVlrTasa.Text.Trim().Equals("")) { VblVlrTasa = Convert.ToDouble(0); }
                else { VblVlrTasa = Convert.ToDouble(TxtVlrTasa.Text.Trim()); }
                string VbIdCia = Session["!dC!@"].ToString();
                List<ClsUbicaGeograf> ObjUbGeo = new List<ClsUbicaGeograf>();
                var TypUbGeo = new ClsUbicaGeograf()
                {
                    IdUbicaGeogr = Convert.ToInt32(DdlBusq.Text.Trim()),
                    CodUbicaGeogr = TxtCod.Text.Trim(),
                    Nombre = TxtNombre.Text.Trim(),
                    CodUbicaGeoSup = DdlUbicaSupr.Text.Trim(),
                    CodTipoUbicaGeogr = DdlTipoUbc.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    VlorTasa = VblVlrTasa,
                    Activa = CkbActivo.Checked == true ? 1 : 0,
                    RutaFrecuente = CkbRutaFrec.Checked == true ? 1 : 0,
                    IdConfigCia = Convert.ToInt32(VbIdCia),
                    Accion = "DELETE",
                };
                ObjUbGeo.Add(TypUbGeo);
                ClsUbicaGeograf ClsUbicaGeograf = new ClsUbicaGeograf();
                ClsUbicaGeograf.Alimentar(ObjUbGeo);
                string Mensj = ClsUbicaGeograf.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                ViewState["Accion"] = "";
                LimpiarCampos("DELETE");
                DdlBusq.Text = "0";
                BindBDdl("UPD");
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en la eliminacion
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Ubicacion Geografica", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}