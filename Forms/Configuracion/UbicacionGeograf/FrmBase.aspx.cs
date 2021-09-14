using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgManto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Configuracion.UbicacionGeograf
{
    public partial class FrmBase : System.Web.UI.Page
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
                BindDdl("", "UPD");
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
                    LblBusq.Text = bO.Equals("LblBusq") ? bT : LblBusq.Text;
                    LblCod.Text = bO.Equals("LblCod") ? bT : LblCod.Text;
                    LblNombre.Text = bO.Equals("LblNombre") ? bT : LblNombre.Text;
                    LblFrecR.Text = bO.Equals("LblFrecR") ? bT : LblFrecR.Text;
                    LblTelef.Text = bO.Equals("LblTelef") ? bT : LblTelef.Text;
                    LblFax.Text = bO.Equals("LblFax") ? bT : LblFax.Text;
                    LblDescrip.Text = bO.Equals("LblDescrip") ? bT : LblDescrip.Text;
                    LblUbica.Text = bO.Equals("LblUbicGeog") ? bT : LblUbica.Text;
                    CkbActivo.Text = bO.Equals("CkbActivo") ? "&nbsp" + bT : CkbActivo.Text;
                    LblDir.Text = bO.Equals("LblDir") ? bT : LblDir.Text;
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
        protected void BindDdl(string CodUG, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = " EXEC SP_TablasGeneral 16,'CodUB','','','','','','','','',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@CdUb", CodUG.Trim());
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Busq";
                                DSTDdl.Tables[1].TableName = "Ubica";
                                DSTDdl.Tables[2].TableName = "DatosBase";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result;
            string VbCodAnt = "";

            VbCodAnt = DdlBusq.Text.Trim();
            DdlBusq.DataSource = DSTDdl.Tables[0];
            DdlBusq.DataTextField = "NomBase";
            DdlBusq.DataValueField = "IdBase";
            DdlBusq.DataBind();
            DdlBusq.Text = VbCodAnt;

            DataTable DT = new DataTable();
            VbCodAnt = CodUG.Trim();
            DT = DSTDdl.Tables[1].Clone();

            Result = DSTDdl.Tables[1].Select("CodUbicaGeogr='" + CodUG.Trim() + "'");// trae el codigo actual por si esta inactivo
            foreach (DataRow Row in Result)
            { DT.ImportRow(Row); }

            Result = DSTDdl.Tables[1].Select("Activa=1");
            foreach (DataRow Row in Result)
            { DT.ImportRow(Row); }

            DdlUbica.DataSource = DT;
            DdlUbica.DataTextField = "Nombre";
            DdlUbica.DataValueField = "CodUbicaGeogr";
            DdlUbica.DataBind();
            DdlUbica.Text = VbCodAnt;

        }
        protected void LimpiarCampos(string Accion)
        {
            TxtCod.Text = ""; TxtNombre.Text = ""; TxtFrecR.Text = "0"; TxtTelef.Text = "0"; TxtFax.Text = "0"; TxtDir.Text = ""; TxtDescrip.Text = ""; DdlUbica.Text = "";
            if (Accion.Trim().Equals("INSERT")) { CkbActivo.Checked = true; }
            else { CkbActivo.Checked = false; }
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            string VbDatoRequerido = "";
            DataRow[] Result1 = Idioma.Select("Objeto= 'MensCampoReq'");
            foreach (DataRow row in Result1)
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
            if (TxtTelef.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbDatoRequerido + "');", true);
                ViewState["Validar"] = "N"; TxtTelef.Focus(); return;
            }
            if (TxtDir.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbDatoRequerido + "');", true);
                ViewState["Validar"] = "N"; TxtDir.Focus(); return;
            }
            if (DdlUbica.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Bas'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La ubicación es requerida.
                ViewState["Validar"] = "N"; return;
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
            TxtCod.Enabled = Ing; TxtNombre.Enabled = Edi; TxtFrecR.Enabled = Edi; TxtTelef.Enabled = Edi; TxtFax.Enabled = Edi;
            TxtDir.Enabled = Edi; DdlUbica.Enabled = Edi; TxtDescrip.Enabled = Edi;
            if (accion.Trim().Equals("UPDATE")) { CkbActivo.Enabled = Edi; }

        }
        protected void Traerdatos(string Prmtr)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result = DSTDdl.Tables[2].Select("IdBase = " + Prmtr.Trim());
            foreach (DataRow SDR in Result)
            {
                TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodBase"].ToString().Trim());
                TxtNombre.Text = HttpUtility.HtmlDecode(SDR["NomBase"].ToString().Trim());
                TxtFrecR.Text = HttpUtility.HtmlDecode(SDR["FrecuenciaRadio"].ToString().Trim());
                TxtTelef.Text = HttpUtility.HtmlDecode(SDR["Telefono"].ToString().Trim());
                TxtFax.Text = HttpUtility.HtmlDecode(SDR["Fax"].ToString().Trim());
                TxtDir.Text = HttpUtility.HtmlDecode(SDR["Direccion"].ToString().Trim());
                string VbUbica = HttpUtility.HtmlDecode(SDR["CodUbicaGeogr"].ToString().Trim());
                BindDdl(VbUbica, "SEL");
                TxtDescrip.Text = HttpUtility.HtmlDecode(SDR["Descripcion"].ToString().Trim());
                CkbActivo.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["Activo"].ToString().Trim()));
            }
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

                    List<ClsTypBodega> ObjBase = new List<ClsTypBodega>();
                    var TypBodega = new ClsTypBodega()
                    {
                        IdBase = 0,
                        CodBase = TxtCod.Text.Trim(),
                        NomBase = TxtNombre.Text.Trim(),
                        CodUbicaGeogr = DdlUbica.Text.Trim(),
                        Descripcion = TxtDescrip.Text.Trim(),
                        CodTecnico = "",
                        FrecuenciaRadio = TxtFrecR.Text.Trim(),
                        Fax = TxtFax.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Direccion = TxtDir.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        Activo = CkbActivo.Checked == true ? 1 : 0,
                        IdConfigCia = (int)Session["!dC!@"],
                        Accion = "INSERT",
                    };
                    ObjBase.Add(TypBodega);
                    ClsTypBodega ClsBase = new ClsTypBodega();
                    ClsBase.Alimentar(ObjBase);
                    string Mensj = ClsBase.GetMensj();
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
                    BindDdl(DdlUbica.Text.Trim(), "UPD");
                    DdlBusq.Text = ClsBase.GetIdBase().ToString();
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Base", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtCod.Text.Equals("") || DdlBusq.Text.Trim().Equals("0"))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    string VbCodUb;
                    VbCodUb = DdlUbica.Text.Trim();
                    BindDdl(DdlUbica.Text.Trim(), "SEL");
                    DdlUbica.Text = VbCodUb;
                    ActivarBtn(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
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

                    List<ClsTypBodega> ObjBase = new List<ClsTypBodega>();
                    var TypBase = new ClsTypBodega()
                    {
                        IdBase = Convert.ToInt32(DdlBusq.Text.Trim()),
                        CodBase = TxtCod.Text.Trim(),
                        NomBase = TxtNombre.Text.Trim(),
                        CodUbicaGeogr = DdlUbica.Text.Trim(),
                        Descripcion = TxtDescrip.Text.Trim(),
                        CodTecnico = "",
                        FrecuenciaRadio = TxtFrecR.Text.Trim(),
                        Fax = TxtFax.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Direccion = TxtDir.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        Activo = CkbActivo.Checked == true ? 1 : 0,
                        IdConfigCia = (int)Session["!dC!@"],
                        Accion = "UPDATE",
                    };
                    ObjBase.Add(TypBase);
                    ClsTypBodega ClsBase = new ClsTypBodega();
                    ClsBase.Alimentar(ObjBase);
                    string Mensj = ClsBase.GetMensj();
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
                    DdlBusq.Enabled = true;
                    BindDdl(DdlUbica.Text.Trim(), "UPD");
                    DdlBusq.Text = ClsBase.GetIdBase().ToString().Trim();
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Base", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {

                if (TxtCod.Text.Equals("") || DdlBusq.Text.Trim().Equals("0"))
                { return; }

                List<ClsTypBodega> ObjBase = new List<ClsTypBodega>();
                var TypBase = new ClsTypBodega()
                {
                    IdBase = Convert.ToInt32(DdlBusq.Text.Trim()),
                    CodBase = TxtCod.Text.Trim(),
                    NomBase = TxtNombre.Text.Trim(),
                    CodUbicaGeogr = DdlUbica.Text.Trim(),
                    Descripcion = TxtDescrip.Text.Trim(),
                    CodTecnico = "",
                    FrecuenciaRadio = TxtFrecR.Text.Trim(),
                    Fax = TxtFax.Text.Trim(),
                    Telefono = TxtTelef.Text.Trim(),
                    Direccion = TxtDir.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    Activo = CkbActivo.Checked == true ? 1 : 0,
                    IdConfigCia = (int)Session["!dC!@"],
                    Accion = "DELETE",
                };
                ObjBase.Add(TypBase);
                ClsTypBodega ClsBase = new ClsTypBodega();
                ClsBase.Alimentar(ObjBase);
                string Mensj = ClsBase.GetMensj();
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
                BindDdl("", "UPD");
                DdlBusq.Text = "0";
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en la eliminacion
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Base", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}