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

namespace _77NeoWeb.Forms.Configuracion.ControlPersonal
{
    public partial class FrmPersona : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null) { Response.Redirect("~/FrmAcceso.aspx"); }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /* Session["C77U"] = "00000082";// 00000082|00000133
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["!dC!@"] = 0;
                Session["77IDM"] = "5"; // 4 español | 5 ingles    */
            }
            if (!IsPostBack)
            {
                ModSeguridad();
                BindBDdlBusq();
                BindBDdl();
                ViewState["Accion"] = "";
                MultVw.ActiveViewIndex = 0;
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; GrdLicencias.ShowFooter = false; GrdCursos.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; BtnExportar.Visible = false; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // licencias
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//asignar Cursos
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; BtnAsigUsu.Visible = false; }// boton para asignar la persona al grupo de manto y crar usuario
            if (ClsP.GetCE4() == 0) { }
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
                    LblCodUsu.Text = bO.Equals("LblCodUsu") ? bT : LblCodUsu.Text;
                    LblCedul.Text = bO.Equals("LblCedul") ? bT : LblCedul.Text;
                    LblNombr.Text = bO.Equals("LblNombr") ? bT : LblNombr.Text;
                    LblApell.Text = bO.Equals("LblApell") ? bT : LblApell.Text;
                    LblFechNac.Text = bO.Equals("LblFechNac") ? bT : LblFechNac.Text;
                    LblTelef.Text = bO.Equals("LblTelef") ? bT : LblTelef.Text;
                    LblDirec.Text = bO.Equals("LblDirec") ? bT : LblDirec.Text;
                    LblCorreoP.Text = bO.Equals("LblCorreoP") ? bT : LblCorreoP.Text;
                    LblCorreoEmsa.Text = bO.Equals("LblCorreoEmsa") ? bT : LblCorreoEmsa.Text;
                    LblCelu.Text = bO.Equals("LblCelu") ? bT : LblCelu.Text;
                    LblArea.Text = bO.Equals("LblArea") ? bT : LblArea.Text;
                    LblCargo.Text = bO.Equals("LblCargo") ? bT : LblCargo.Text;
                    CkbActivo.Text = bO.Equals("CkbActivo") ? bT : CkbActivo.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnAsigUsu.Text = bO.Equals("BtnAsigUsu") ? bT : BtnAsigUsu.Text;
                    BtnAsigUsu.ToolTip = bO.Equals("BtnAsigUsuTT") ? bT : BtnAsigUsu.ToolTip;
                    BtnExportar.Text = bO.Equals("BtnExportar") ? bT : BtnExportar.Text;
                    LblBusqPers.Text = bO.Equals("LblBusqPers") ? bT : LblBusqPers.Text;
                    LblTitLicencias.Text = bO.Equals("BtnLicencia") ? bT : LblTitLicencias.Text;
                    GrdLicencias.Columns[0].HeaderText = bO.Equals("CkbActivo") ? bT : GrdLicencias.Columns[0].HeaderText;
                    GrdLicencias.Columns[1].HeaderText = bO.Equals("GrdLicenc") ? bT : GrdLicencias.Columns[1].HeaderText;
                    GrdLicencias.Columns[2].HeaderText = bO.Equals("GrdNum") ? bT : GrdLicencias.Columns[2].HeaderText;
                    GrdLicencias.Columns[3].HeaderText = bO.Equals("GrdFechVen") ? bT : GrdLicencias.Columns[3].HeaderText;
                    GrdLicencias.Columns[4].HeaderText = bO.Equals("GrdMdl") ? bT : GrdLicencias.Columns[4].HeaderText;
                    GrdLicencias.Columns[5].HeaderText = bO.Equals("GrdEspcl") ? bT : GrdLicencias.Columns[5].HeaderText;
                    LblTitCurso.Text = bO.Equals("BtnCursos") ? bT : LblTitCurso.Text;
                    GrdCursos.Columns[0].HeaderText = bO.Equals("CkbActivo") ? bT : GrdCursos.Columns[0].HeaderText;
                    GrdCursos.Columns[1].HeaderText = bO.Equals("GrdNombr") ? bT : GrdCursos.Columns[1].HeaderText;
                    GrdCursos.Columns[2].HeaderText = bO.Equals("GrdFechVen") ? bT : GrdCursos.Columns[2].HeaderText;
                    LblUsuario.Text = bO.Equals("LblUsuario") ? bT : LblUsuario.Text;
                    LblTitCrearusu.Text = bO.Equals("LblTitCrearusu") ? bT : LblTitCrearusu.Text;
                    LblNomUsu.Text = bO.Equals("LblUsuario") ? bT + ":" : LblNomUsu.Text;
                    IbtCerrarCrearUsu.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCrearUsu.ToolTip;
                    BtnAsignarUsu.Text = bO.Equals("BtnAsignarUsu") ? bT : BtnAsignarUsu.Text;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdLicencias.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[6].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[6].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdCursos.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[3].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void BindBDdlBusq()
        {
            string LtxtSql = "EXEC SP_PANTALLA_Persona 8,'','','','BUSC',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlBusqPers.DataSource = Cnx.DSET(LtxtSql);
            DdlBusqPers.DataMember = "Datos";
            DdlBusqPers.DataTextField = "Persona";
            DdlBusqPers.DataValueField = "CodPersona";
            DdlBusqPers.DataBind();
        }
        protected void BindBDdl()
        {
            string LtxtSql = "EXEC SP_PANTALLA_Persona 8,'','','','AREA',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlArea.DataSource = Cnx.DSET(LtxtSql);
            DdlArea.DataMember = "Datos";
            DdlArea.DataTextField = "Descripcion";
            DdlArea.DataValueField = "CodArea";
            DdlArea.DataBind();

            LtxtSql = "EXEC SP_PANTALLA_Persona 8,'','','','CARGO',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlCargo.DataSource = Cnx.DSET(LtxtSql);
            DdlCargo.DataMember = "Datos";
            DdlCargo.DataTextField = "Descripcion";
            DdlCargo.DataValueField = "CodCargo";
            DdlCargo.DataBind();
        }
        protected void LimpiarCampos()
        {
            TxtCodUsu.Text = "";
            TxtCedul.Text = "";
            TxtNombr.Text = "";
            TxtApell.Text = "";
            CkbActivo.Checked = false;
            TxtFechNac.Text = "";
            TxtTelef.Text = "";
            TxtDirec.Text = "";
            TxtCorreoP.Text = "";
            TxtCorreoEmsa.Text = "";
            TxtCelu.Text = "";
            DdlArea.Text = "";
            DdlCargo.Text = "";
            TxtUsuario.Text = "";
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtCedul.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la cedula
                ViewState["Validar"] = "N"; TxtCedul.Focus(); return;
            }
            if (TxtNombr.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un nombre
                ViewState["Validar"] = "N"; TxtNombr.Focus(); return;
            }
            if (TxtApell.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un apellido
                ViewState["Validar"] = "N"; TxtApell.Focus(); return;
            }

            if (TxtFechNac.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la fecha de nacimiento
                ViewState["Validar"] = "N"; TxtFechNac.Focus(); return;
            }
            if (TxtFechNac.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens08'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            DateTime FechaI = Convert.ToDateTime(TxtFechNac.Text);
            DateTime FechaF = Convert.ToDateTime("01/01/1900");
            int Comparar = DateTime.Compare(FechaI, FechaF);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens08'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            if (TxtCorreoEmsa.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el correo de la compañía
                ViewState["Validar"] = "N"; TxtCorreoEmsa.Focus(); return;
            }
            if (TxtCelu.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el número del celular.
                ViewState["Validar"] = "N"; TxtCelu.Focus(); return;
            }
            if (DdlArea.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens07Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el área.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlCargo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens08Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el cargo.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnAsigUsu.Enabled = Otr;
            BtnExportar.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            if (accion.Equals("UPDATE")) { CkbActivo.Enabled = Edi; }
            TxtCedul.Enabled = Ing;
            TxtNombr.Enabled = Edi;
            TxtApell.Enabled = Edi;
            TxtFechNac.Enabled = Edi;
            TxtTelef.Enabled = Edi;
            TxtDirec.Enabled = Edi;
            TxtCorreoP.Enabled = Edi;
            TxtCorreoEmsa.Enabled = Edi;
            TxtCelu.Enabled = Edi;
            DdlArea.Enabled = Edi;
            DdlCargo.Enabled = Edi;
        }
        protected void Traerdatos(string Prmtr)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbFecha;
                    Cnx2.Open();
                    string LtxtSql = "EXEC SP_PANTALLA_Persona 9, @Prmtr,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                    SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        CkbActivo.Checked = Convert.ToBoolean(SDR["Activo"].ToString());
                        TxtCedul.Text = HttpUtility.HtmlDecode(SDR["Cedula"].ToString().Trim());
                        TxtCodUsu.Text = HttpUtility.HtmlDecode(SDR["CodPersona"].ToString().Trim());
                        TxtNombr.Text = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                        TxtApell.Text = HttpUtility.HtmlDecode(SDR["Apellido"].ToString().Trim());
                        VbFecha = HttpUtility.HtmlDecode(SDR["Fechanacimiento"].ToString().Trim());
                        if (!VbFecha.Trim().Equals(""))
                        {
                            DateTime VbFecID = Convert.ToDateTime(VbFecha);
                            TxtFechNac.Text = String.Format("{0:yyyy-MM-dd}", VbFecID);
                        }
                        else { TxtFechNac.Text = ""; }


                        TxtTelef.Text = HttpUtility.HtmlDecode(SDR["Telefono"].ToString().Trim());
                        TxtDirec.Text = HttpUtility.HtmlDecode(SDR["Direccion"].ToString().Trim());
                        TxtCorreoP.Text = HttpUtility.HtmlDecode(SDR["Correo"].ToString().Trim());
                        TxtCorreoEmsa.Text = HttpUtility.HtmlDecode(SDR["CorreoCia"].ToString().Trim());
                        TxtCelu.Text = HttpUtility.HtmlDecode(SDR["Celular"].ToString().Trim());
                        DdlArea.Text = HttpUtility.HtmlDecode(SDR["CodArea"].ToString().Trim());
                        DdlCargo.Text = HttpUtility.HtmlDecode(SDR["CodCargo"].ToString().Trim());
                        TxtUsuario.Text = HttpUtility.HtmlDecode(SDR["Usuario"].ToString().Trim());
                    }
                    SDR.Close();
                    Cnx2.Close();
                    BindDLicen(TxtCodUsu.Text.Trim());
                    BindDCurso(TxtCodUsu.Text.Trim());
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
        protected void DdlBusqPers_TextChanged(object sender, EventArgs e)
        { Traerdatos(DdlBusqPers.Text); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false);

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCampos();
                    CkbActivo.Checked = true;
                    ActivarCampos(true, true, "Ingresar");
                    DdlBusqPers.SelectedValue = "";
                    DdlBusqPers.Enabled = false;
                    BindDLicen("");
                    GrdLicencias.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {

                    ValidarCampos("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    List<CsTypPersona> ObjPersona = new List<CsTypPersona>();
                    var TypPersona = new CsTypPersona()
                    {
                        CodPersona = "",
                        CodEmpresa = "",
                        Nombre = TxtNombr.Text.Trim(),
                        Apellido = TxtApell.Text.Trim(),
                        Registro = "",
                        Cedula = TxtCedul.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Celular = TxtCelu.Text.Trim(),
                        Correo = TxtCorreoP.Text.Trim(),
                        Direccion = TxtDirec.Text.Trim(),
                        Fechanacimiento = Convert.ToDateTime(TxtFechNac.Text.Trim()),
                        FechaIngreso = null,
                        CodArea = DdlArea.Text.Trim(),
                        CodCargo = DdlCargo.Text.Trim(),
                        NivelTecnico = "",
                        NumeroLicencia = "",
                        ValorHoraPer = 0,
                        CodTipoContrPer = "",
                        CodBase = "",
                        CodFS = "",
                        Estado = CkbActivo.Checked == true ? "ACTIVO" : "INACTIVO",
                        Pusuario1 = "",
                        Usu = Session["C77U"].ToString(),
                        CorreoCia = TxtCorreoEmsa.Text.Trim(),
                        HorasTotales = "",
                        Accion = "INSERT"
                    };
                    ObjPersona.Add(TypPersona);
                    CsTypPersona ClsPersona = new CsTypPersona();
                    ClsPersona.Alimentar(ObjPersona);
                    string Mensj = ClsPersona.GetMensj();
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
                    DdlBusqPers.Enabled = true;
                    BindBDdlBusq();
                    DdlBusqPers.Text = ClsPersona.GetCodPersn().ToString().Trim();
                    Traerdatos(ClsPersona.GetCodPersn().ToString().Trim());
                    GrdLicencias.Enabled = true;
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Persona", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtCodUsu.Text.Equals(""))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPDATE");
                    DdlBusqPers.SelectedValue = "";
                    DdlBusqPers.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                }
                else
                {
                    ValidarCampos("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    List<CsTypPersona> ObjPersona = new List<CsTypPersona>();
                    var TypPersona = new CsTypPersona()
                    {
                        CodPersona = TxtCodUsu.Text.Trim(),
                        CodEmpresa = "",
                        Nombre = TxtNombr.Text.Trim(),
                        Apellido = TxtApell.Text.Trim(),
                        Registro = "",
                        Cedula = TxtCedul.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Celular = TxtCelu.Text.Trim(),
                        Correo = TxtCorreoP.Text.Trim(),
                        Direccion = TxtDirec.Text.Trim(),
                        Fechanacimiento = Convert.ToDateTime(TxtFechNac.Text.Trim()),
                        FechaIngreso = null,
                        CodArea = DdlArea.Text.Trim(),
                        CodCargo = DdlCargo.Text.Trim(),
                        NivelTecnico = "",
                        NumeroLicencia = "",
                        ValorHoraPer = 0,
                        CodTipoContrPer = "",
                        CodBase = "",
                        CodFS = "",
                        Estado = CkbActivo.Checked == true ? "ACTIVO" : "INACTIVO",
                        Pusuario1 = "",
                        Usu = Session["C77U"].ToString(),
                        CorreoCia = TxtCorreoEmsa.Text.Trim(),
                        HorasTotales = "",
                        Accion = "UPDATE",
                    };

                    ObjPersona.Add(TypPersona);
                    CsTypPersona ClsPersona = new CsTypPersona();
                    ClsPersona.Alimentar(ObjPersona);
                    string Mensj = ClsPersona.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampos(false, false, "UPDATE");
                    DdlBusqPers.Enabled = true;
                    BindBDdlBusq();
                    DdlBusqPers.Text = TxtCodUsu.Text.Trim();
                    Traerdatos(TxtCodUsu.Text.Trim());
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Persona", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnAsigUsu_Click(object sender, EventArgs e)
        {
            if (TxtUsuario.Text.Trim().Equals("") && !TxtCodUsu.Text.Trim().Equals("")) { MultVw.ActiveViewIndex = 1; }
        }
        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitExporPer'");
            foreach (DataRow row in Result)
            { VbNomRpt = row["Texto"].ToString().Trim(); }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurPersona", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_PANTALLA_Persona 3,'','','','CurPersona',0,0,0,0,'01-01-01','01-01-01','01-01-01'";
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
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
        protected void BindDLicen(string VbConsultar)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SP_PANTALLA_Persona 2,@U,'','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'";
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@U", VbConsultar);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdLicencias.DataSource = dtbl;
                GrdLicencias.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdLicencias.DataSource = dtbl;
                GrdLicencias.DataBind();
                GrdLicencias.Rows[0].Cells.Clear();
                GrdLicencias.Rows[0].Cells.Add(new TableCell());
                GrdLicencias.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdLicencias.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdLicencias.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdLicencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                try
                {
                    string VbLic = "", VbModel = "", VbEspec = "", VBQuery = "";
                    double VbNNum = 0;
                    DateTime VbFechaVenc;
                    VbLic = (GrdLicencias.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).Text.Trim();
                    VbNNum = Convert.ToDouble((GrdLicencias.FooterRow.FindControl("TxtNumPP") as TextBox).Text.Trim());
                    if (!(GrdLicencias.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals(""))
                    {
                        VbFechaVenc = Convert.ToDateTime((GrdLicencias.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim());
                    }
                    else
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens12Persn'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha de vencimiento.
                        return;
                    }
                    VbModel = (GrdLicencias.FooterRow.FindControl("TxtModPP") as TextBox).Text.Trim();
                    VbEspec = (GrdLicencias.FooterRow.FindControl("TxtEspecPP") as TextBox).Text.Trim();

                    if (VbLic.Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens10Persn'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una licencia.
                        return;
                    }
                    if (VbNNum <= 0)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens11Persn'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un numero válido.
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "EXEC SP_TablasGeneral 1,@CP,@Lc,@Md,@Ep,@US,'','','','INSERT',@Ac,@Nm,0,0,0,0,@FV,'02-01-1','03-01-1'";
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                SC.Parameters.AddWithValue("@CP", TxtCodUsu.Text.Trim());
                                SC.Parameters.AddWithValue("@Ac", (GrdLicencias.FooterRow.FindControl("CkbActivoPP") as CheckBox).Checked == false ? 0 : 1);
                                SC.Parameters.AddWithValue("@Lc", VbLic);
                                SC.Parameters.AddWithValue("@Nm", VbNNum);
                                SC.Parameters.AddWithValue("@FV", VbFechaVenc);
                                SC.Parameters.AddWithValue("@Md", VbModel);
                                SC.Parameters.AddWithValue("@Ep", VbEspec);
                                SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                                try
                                {
                                    var Mensj = SC.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString().Trim(); }
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDLicen(TxtCodUsu.Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                }/**/
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    string borr = Ex.ToString().Trim();
                }
            }
        }
        protected void GrdLicencias_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdLicencias.EditIndex = e.NewEditIndex;
            BindDLicen(TxtCodUsu.Text.Trim());
        }
        protected void GrdLicencias_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            DateTime VbFechaVenc;
            int VblId = Convert.ToInt32(GrdLicencias.DataKeys[e.RowIndex].Value.ToString());

            double VbNNum = Convert.ToDouble((GrdLicencias.Rows[e.RowIndex].FindControl("TxtNum") as TextBox).Text.Trim());
            if (VbNNum <= 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens11Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un numero válido.
                return;
            }
            if (!(GrdLicencias.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals(""))
            {
                VbFechaVenc = Convert.ToDateTime((GrdLicencias.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim());
            }
            else
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha de vencimiento.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 1,@CP,'',@Md,@Ep,@US,'','','','UPDATE',@Ac,@Nm,@id,0,0,0,@FV,'02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@CP", TxtCodUsu.Text.Trim());
                        SC.Parameters.AddWithValue("@Ac", (GrdLicencias.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@Nm", VbNNum);
                        SC.Parameters.AddWithValue("@FV", VbFechaVenc);
                        SC.Parameters.AddWithValue("@Md", (GrdLicencias.Rows[e.RowIndex].FindControl("TxtMod") as TextBox).Text.Trim());
                        SC.Parameters.AddWithValue("@Ep", (GrdLicencias.Rows[e.RowIndex].FindControl("TxtEspec") as TextBox).Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@id", VblId);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdLicencias.EditIndex = -1;
                            BindDLicen(TxtCodUsu.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdLicencias_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdLicencias.EditIndex = -1;
            BindDLicen(TxtCodUsu.Text.Trim());
        }
        protected void GrdLicencias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbCod;
            VbCod = GrdLicencias.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 1,'','','','',@US,'','','','DELETE',0,0,@ID,0,0,0,'01-01-01','02-01-1','03-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ID", VbCod);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindDLicen(TxtCodUsu.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdLicencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string LtxtSql = "EXEC SP_PANTALLA_Persona 1,'','','','',2073,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlLicenRFPP.DataTextField = "CodLicencia";
                DdlLicenRFPP.DataValueField = "CodIdLicencia";
                DdlLicenRFPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        protected void GrdLicencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdLicencias.PageIndex = e.NewPageIndex; BindDLicen(TxtCodUsu.Text.Trim()); }
        protected void BindDCurso(string VbConsultar)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SP_TablasGeneral 2,@U,'','','','','','','','SELECT',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	";

            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@U", VbConsultar);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdCursos.DataSource = dtbl;
                GrdCursos.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdCursos.DataSource = dtbl;
                GrdCursos.DataBind();
                GrdCursos.Rows[0].Cells.Clear();
                GrdCursos.Rows[0].Cells.Add(new TableCell());
                GrdCursos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdCursos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdCursos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdCursos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                try
                {
                    string VBQuery = "";
                    DateTime VbFechaVenc;
                    string VbNombre = (GrdCursos.FooterRow.FindControl("DdlNombrePP") as DropDownList).Text.Trim();
                    if (!(GrdCursos.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals(""))
                    { VbFechaVenc = Convert.ToDateTime((GrdCursos.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim()); }
                    else
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens12Persn'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha de vencimiento.
                        BindDCurso(TxtCodUsu.Text.Trim()); PerfilesGrid(); return;
                    }

                    if (VbNombre.Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens14Persn'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un curso.
                        BindDCurso(TxtCodUsu.Text.Trim()); PerfilesGrid(); return;
                    }

                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "EXEC SP_TablasGeneral 2,@CP,@CCrs,@US,'','','','','','INSERT',@Ac,0,0,0,0,0,@FV,'02-01-1','03-01-1'";
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                SC.Parameters.AddWithValue("@CP", TxtCodUsu.Text.Trim());
                                SC.Parameters.AddWithValue("@Ac", (GrdCursos.FooterRow.FindControl("CkbActivoPP") as CheckBox).Checked == false ? 0 : 1);
                                SC.Parameters.AddWithValue("@CCrs", VbNombre);
                                SC.Parameters.AddWithValue("@FV", VbFechaVenc);
                                SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                                try
                                {
                                    var Mensj = SC.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                        foreach (DataRow row in Result)
                                        { Mensj = row["Texto"].ToString().Trim(); }
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDCurso(TxtCodUsu.Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                }/**/
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    string borr = Ex.ToString().Trim();
                }
            }
        }
        protected void GrdCursos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdCursos.EditIndex = e.NewEditIndex;
            BindDCurso(TxtCodUsu.Text.Trim());
        }
        protected void GrdCursos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            DateTime VbFechaVenc;
            int VblId = Convert.ToInt32(GrdCursos.DataKeys[e.RowIndex].Value.ToString());
            if (!(GrdCursos.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals(""))
            {
                VbFechaVenc = Convert.ToDateTime((GrdCursos.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim());
            }
            else
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12Persn'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una fecha de vencimiento.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 2,'','',@US,'','','','','','UPDATE',@Ac,@id,0,0,0,0,@FV,'02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Ac", (GrdCursos.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@FV", VbFechaVenc);
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@id", VblId);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdCursos.EditIndex = -1;
                            BindDCurso(TxtCodUsu.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdCursos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdCursos.EditIndex = -1;
            BindDCurso(TxtCodUsu.Text.Trim());
        }
        protected void GrdCursos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbCod = GrdCursos.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();

            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 2,'','',@US,'','','','','','DELETE',0,@id,0,0,0,0,'01-01-01','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ID", VbCod);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindDCurso(TxtCodUsu.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdCursos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string LtxtSql = "EXEC SP_PANTALLA_Persona 8,'','','','CURSO',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                DropDownList DdlNombrePP = (e.Row.FindControl("DdlNombrePP") as DropDownList);
                DdlNombrePP.DataSource = Cnx.DSET(LtxtSql);
                DdlNombrePP.DataTextField = "Nombre";
                DdlNombrePP.DataValueField = "IdCurso";
                DdlNombrePP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        //***********************ASIGNAR USUARIO A MANTO        
        protected void IbtCerrarCrearusu_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BtnAsignarUsu_Click(object sender, EventArgs e)
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtNomUsu.Text.Trim().Equals(""))
            { return; }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_Persona 10,@CU,@NU,@CD,@US,0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@CU", TxtCodUsu.Text.Trim());
                        SC.Parameters.AddWithValue("@NU", TxtNomUsu.Text.Trim());
                        SC.Parameters.AddWithValue("@CD", TxtCedul.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            Traerdatos(TxtCodUsu.Text.Trim());
                            MultVw.ActiveViewIndex = 0;
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
    }
}