using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgLogistica;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Configuracion.InventarioLogistica
{
    public partial class FrmAlmacen : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTDet = new DataTable();
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
                    Session["C77U"] = "00000082"; //00000082|00000133
                    Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = "77NEO01";
                    Session["V$U@"] = "sa";
                    Session["P@$"] = "admindemp";
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = 1;
                    Session["77IDM"] = "4"; // 4 español | 5 ingles  */
                }
            }
            if (!IsPostBack)
            {
                MultVw.ActiveViewIndex = 0;
                ModSeguridad();
                BindBDdlBusq();
                BindBDdl();
                ViewState["Accion"] = "";
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
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false;
                GrdDetalle.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; BtnAsigPers.Visible = false; } // Asignar Personas
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }// 
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
                    LblCod.Text = bO.Equals("LblCod") ? bT : LblCod.Text;
                    LblBusq.Text = bO.Equals("LblBusq") ? bT : LblBusq.Text;
                    LblNombre.Text = bO.Equals("LblNombre") ? bT : LblNombre.Text;
                    LblDescrip.Text = bO.Equals("LblDescrip") ? bT : LblDescrip.Text;
                    LblBase.Text = bO.Equals("LblBase") ? bT : LblBase.Text;
                    LblUbicGeog.Text = bO.Equals("LblUbicGeog") ? bT : LblUbicGeog.Text;
                    CkbActivo.Text = bO.Equals("CkbActivo") ? "&nbsp" + bT : CkbActivo.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    BtnAsigPers.Text = bO.Equals("BtnAsigPers") ? bT : BtnAsigPers.Text;
                    LblTitUbicaAsig.Text = bO.Equals("LblTitUbicaAsig") ? bT : LblTitUbicaAsig.Text;
                    GrdDetalle.Columns[0].HeaderText = bO.Equals("GrdBod") ? bT : GrdDetalle.Columns[0].HeaderText;
                    GrdDetalle.Columns[1].HeaderText = bO.Equals("GrdFil") ? bT : GrdDetalle.Columns[1].HeaderText;
                    GrdDetalle.Columns[2].HeaderText = bO.Equals("GrdColmn") ? bT : GrdDetalle.Columns[2].HeaderText;
                    GrdDetalle.Columns[3].HeaderText = bO.Equals("GrdProp") ? bT : GrdDetalle.Columns[3].HeaderText;
                    //**********************************************Asignar Bodegas **********************************************
                    LblTitAsigarUbica.Text = bO.Equals("LblTitAsigarUbica") ? bT : LblTitAsigarUbica.Text;
                    IbtCerrarAsigUbica.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsigUbica.ToolTip;
                    LblTitUbicaDispo.Text = bO.Equals("LblTitUbicaDispo") ? bT : LblTitUbicaDispo.Text;
                    CkbTodasUbica.Text = bO.Equals("CkbTodasUbica") ? "&nbsp" + bT : CkbTodasUbica.Text;
                    GrdUbicaDispo.Columns[0].HeaderText = bO.Equals("GrdAsig") ? bT : GrdUbicaDispo.Columns[0].HeaderText;
                    GrdUbicaDispo.Columns[1].HeaderText = bO.Equals("GrdBod") ? bT : GrdUbicaDispo.Columns[1].HeaderText;
                    GrdUbicaDispo.Columns[2].HeaderText = bO.Equals("GrdFil") ? bT : GrdUbicaDispo.Columns[2].HeaderText;
                    GrdUbicaDispo.Columns[3].HeaderText = bO.Equals("GrdColmn") ? bT : GrdUbicaDispo.Columns[3].HeaderText;
                    GrdUbicaDispo.Columns[4].HeaderText = bO.Equals("GrdProp") ? bT : GrdUbicaDispo.Columns[4].HeaderText;
                    //**********************************************Asignar Persona **********************************************
                    LblTitAsigUsu.Text = bO.Equals("BtnAsigPers") ? bT : LblTitAsigUsu.Text;
                    IbtCerrarAsigUsu.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsigUsu.ToolTip;
                    GrdAsigUsu.Columns[0].HeaderText = bO.Equals("CkbActivo") ? bT : GrdAsigUsu.Columns[0].HeaderText;
                    GrdAsigUsu.Columns[1].HeaderText = bO.Equals("GrdUser") ? bT : GrdAsigUsu.Columns[1].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDetalle.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[4].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void BindBDdlBusq()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Almacen 6,'','','','Busq',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlBusq.DataSource = Cnx.DSET(LtxtSql);
            DdlBusq.DataMember = "Datos";
            DdlBusq.DataTextField = "Descripcion";
            DdlBusq.DataValueField = "CodIdAlmacen";
            DdlBusq.DataBind();
        }
        protected void BindBDdl()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Almacen 6,'','','','Base',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
            DdlBase.DataSource = Cnx.DSET(LtxtSql);
            DdlBase.DataMember = "Datos";
            DdlBase.DataTextField = "NomBase";
            DdlBase.DataValueField = "CodBase";
            DdlBase.DataBind();
        }
        protected void LimpiarCampos(string Accion)
        {
            TxtCod.Text = ""; TxtNombre.Text = ""; TxtDescrip.Text = ""; DdlBase.Text = ""; TxtUbicGeog.Text = "";
            if (Accion.Trim().Equals("INSERT")) { CkbActivo.Checked = true; }
            else { CkbActivo.Checked = false; }
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtNombre.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Alm'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un nombre.
                ViewState["Validar"] = "N"; TxtNombre.Focus(); return;
            }
            if (TxtDescrip.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Alm'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un nombre
                ViewState["Validar"] = "N"; TxtDescrip.Focus(); return;
            }
            if (DdlBase.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Alm'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un nombre
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            BtnAsigPers.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, bool Vble, string accion)
        {
            TxtDescrip.Enabled = Edi; TxtNombre.Enabled = Ing; DdlBase.Enabled = Edi; CkbActivo.Enabled = Edi;
            LblUbicGeog.Visible = Vble; TxtUbicGeog.Visible = Vble;
        }
        protected void Traerdatos(string Prmtr)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = "EXEC SP_PANTALLA_Almacen 6,'','','','SELECT',@Prmtr,0,0,@CC,'01-1-2009','01-01-1900','01-01-1900'";
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodIdAlmacen"].ToString().Trim());
                    TxtNombre.Text = HttpUtility.HtmlDecode(SDR["NomAlmacen"].ToString().Trim());
                    TxtDescrip.Text = HttpUtility.HtmlDecode(SDR["Descripcion"].ToString().Trim());
                    DdlBase.Text = HttpUtility.HtmlDecode(SDR["CodBase"].ToString().Trim());
                    TxtUbicGeog.Text = HttpUtility.HtmlDecode(SDR["UbicaGeog"].ToString().Trim());
                }
                SDR.Close();
                Cnx2.Close();
                //BindDBodg(TxtCod.Text.Trim(), "UPDATE");
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        { Traerdatos(DdlBusq.Text); BindDBodg(TxtCod.Text.Trim(), "UPDATE"); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (ViewState["Accion"].ToString().Equals(""))
            {
                ActivarBtn(true, false, false, false, false);

                ViewState["Accion"] = "Aceptar";
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                LimpiarCampos("INSERT");
                ActivarCampos(true, true, false, "Ingresar");
                DdlBusq.SelectedValue = "0";
                DdlBusq.Enabled = false;
                BindDBodg("0", "SELECT");
                GrdDetalle.Enabled = false;
                Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
            }
            else
            {
                string Mensj = "", PCod = "";

                ValidarCampos("INSERT");
                if (ViewState["Validar"].Equals("N"))
                { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 9,@Nm,@Dsc,@Us,@Bs,'','','','TblAlmacen','INSERT',0,@IdC,@Act,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Dsc", TxtDescrip.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@Bs", DdlBase.Text.Trim());
                                SC.Parameters.AddWithValue("@IdC", Session["!dC!@"]);
                                SC.Parameters.AddWithValue("@Act", CkbActivo.Checked == true ? 1 : 0);

                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    PCod = SDR["CodId"].ToString();
                                }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result1)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                ActivarBtn(true, true, true, true, true);
                                ViewState["Accion"] = "";
                                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                                foreach (DataRow row in Result)
                                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                                ActivarCampos(false, false, true, "Ingresar");
                                DdlBusq.Enabled = true;
                                BindBDdlBusq();
                                DdlBusq.Text = PCod;
                                Traerdatos(PCod);
                                BindDBodg(TxtCod.Text.Trim(), "SELECT");
                                GrdDetalle.Enabled = true;
                                BtnIngresar.OnClientClick = "";
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();

                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                            }
                        }
                    }
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (DdlBusq.Text.Equals("0"))
            { return; }

            if (ViewState["Accion"].ToString().Equals(""))
            {
                ActivarBtn(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                ViewState["Accion"] = "Aceptar";
                ActivarCampos(false, true, false, "UPDATE");
                DdlBusq.Enabled = false;
                Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
            }
            else
            {
                string Mensj = "";
                if (DdlBusq.Text.Equals("0"))
                { return; }
                ValidarCampos("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                { return; }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 9,@Nm,@Dsc,@Us,@Bs,'','','','TblAlmacen','UPDATE',@ID,0,@Act,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Dsc", TxtDescrip.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@Bs", DdlBase.Text.Trim());
                                SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Act", CkbActivo.Checked == true ? 1 : 0);
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result1)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                ActivarBtn(true, true, true, true, true);
                                DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                                foreach (DataRow row in Result)
                                { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                                ViewState["Accion"] = "";
                                ActivarCampos(false, false, true, "UPDATE");
                                BindBDdlBusq();
                                DdlBusq.Text = TxtCod.Text.Trim();
                                DdlBusq.Enabled = true;
                                Traerdatos(DdlBusq.Text.Trim());
                                BindDBodg(TxtCod.Text.Trim(), "SELECT");
                                BtnModificar.OnClientClick = "";
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();

                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Modificar", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                            }
                        }
                    }
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlBusq.Text.Equals("0"))
            { return; }

            PerfilesGrid();
            string VBQuery, Mensj = "";
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 9,@Nm,@Dsc,@Us,@Bs,'','','','TblAlmacen','DELETE',@ID,0,@Act,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {

                        SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Dsc", TxtDescrip.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Bs", DdlBase.Text.Trim());
                        SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Act", CkbActivo.Checked == true ? 1 : 0);
                        try
                        {
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                            SDR.Close();

                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result1)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindBDdlBusq();
                            DdlBusq.Text = "0";
                            LimpiarCampos("");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void BtnAsigPers_Click(object sender, EventArgs e)
        {
            if (DdlBusq.Text.Equals("0"))
            { return; }
            BindDAsigUsu();
            MultVw.ActiveViewIndex = 2;
        }
        protected void BindDBodg(string CodA, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Almacen 2,'01','','','',@CA,0,0,@CC,'01-1-2009','01-01-1900','01-01-1900'";
                Cnx.SelecBD();
                using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
                {
                    SCnx.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                    {
                        SC.Parameters.AddWithValue("@CA", CodA);
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            DTDet = (DataTable)ViewState["DTDet"];
            if (DTDet.Rows.Count > 0)
            {
                GrdDetalle.DataSource = DTDet;
                GrdDetalle.DataBind();
            }
            else
            {
                DTDet.Rows.Add(DTDet.NewRow());
                GrdDetalle.DataSource = DTDet;
                GrdDetalle.DataBind();
                GrdDetalle.Rows[0].Cells.Clear();
                GrdDetalle.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDetalle.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDetalle.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void GrdDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (DdlBusq.Text.Equals("0"))
            { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            { MultVw.ActiveViewIndex = 1; BindBDdlAsigUB(); }
        }
        protected void GrdDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 9,'','',@US,@CUB,'','','','Detalle','DELETE',@IdA,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@IdA", TxtCod.Text.Trim());
                        SC.Parameters.AddWithValue("@CUB", GrdDetalle.DataKeys[e.RowIndex].Values["CodUbicaBodega"].ToString());
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
                            BindDBodg(TxtCod.Text.Trim(), "UPDATE");
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
        protected void GrdDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        protected void GrdDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDetalle.PageIndex = e.NewPageIndex; BindDBodg(TxtCod.Text, "SELECT"); }
        // *********************** Asignar ubicaciones fisicas *********************
        protected void BindBDdlAsigUB()
        {
            string LtxtSql = string.Format("EXEC SP_TablasGeneral 9,'','','','','','','','Detalle','DdlAsUB',0,0,0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"]);
            ddlUbicaFis.DataSource = Cnx.DSET(LtxtSql);
            ddlUbicaFis.DataMember = "Datos";
            ddlUbicaFis.DataTextField = "CodBodega";
            ddlUbicaFis.DataValueField = "Codigo";
            ddlUbicaFis.DataBind();
        }
        protected void IbtCerrarAsigUbica_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BindDAsigUbica()
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SP_TablasGeneral 9,@BG,'','','','','','','Detalle','SELECT',@Alm,0,0,0,0,@IC,'01-01-1','02-01-1','03-01-1'";
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@BG", ddlUbicaFis.Text.Trim());
                    SC.Parameters.AddWithValue("@Alm", TxtCod.Text.Trim());
                    SC.Parameters.AddWithValue("@IC", Session["!dC!@"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdUbicaDispo.DataSource = dtbl;
                GrdUbicaDispo.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdUbicaDispo.DataSource = dtbl;
                GrdUbicaDispo.DataBind();
                GrdUbicaDispo.Rows[0].Cells.Clear();
                GrdUbicaDispo.Rows[0].Cells.Add(new TableCell());
                GrdUbicaDispo.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdUbicaDispo.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdUbicaDispo.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void ddlUbicaFis_TextChanged(object sender, EventArgs e)
        { BindDAsigUbica(); CkbTodasUbica.Checked = false; }
        protected void CkbTodasUbica_CheckedChanged(object sender, EventArgs e)
        {
            if (CkbTodasUbica.Checked == true)
            {
                foreach (GridViewRow Row in GrdUbicaDispo.Rows)
                {
                    CheckBox CkbAsigna = Row.FindControl("CkbAsigna") as CheckBox;
                    CkbAsigna.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow Row in GrdUbicaDispo.Rows)
                {
                    CheckBox CkbAsigna = Row.FindControl("CkbAsigna") as CheckBox;
                    CkbAsigna.Checked = false;
                }
            }

        }
        protected void GrdUbicaDispo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<ClsTypAsingarBodega> ObjElemBod = new List<ClsTypAsingarBodega>();
            foreach (GridViewRow Row in GrdUbicaDispo.Rows)
            {
                int Ckb = (Row.FindControl("CkbAsigna") as CheckBox).Checked == true ? 1 : 0;
                if (Ckb > 0)
                {
                    var TypUbicBod = new ClsTypAsingarBodega()
                    {
                        CodIdUbicacion = 0,
                        CodElemento = "0",
                        CodAlmacen = Convert.ToInt32(TxtCod.Text),
                        CodBodega = GrdUbicaDispo.DataKeys[Row.RowIndex].Values[0].ToString(),
                        Cantidad = 0,
                        Usu = Session["C77U"].ToString(),
                        Accion = "INSERT",

                    };
                    ObjElemBod.Add(TypUbicBod);
                }
            } /**/
            ClsTypAsingarBodega ElemBod = new ClsTypAsingarBodega();
            ElemBod.Alimentar(ObjElemBod);// 
            GrdUbicaDispo.DataSource = null;
            GrdUbicaDispo.DataBind();
            ddlUbicaFis.Text = "";
            CkbTodasUbica.Checked = false;
            MultVw.ActiveViewIndex = 0;
            BindDBodg(TxtCod.Text.Trim(), "UPDATE");
        }
        protected void GrdUbicaDispo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'GrdAsig'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        // // *********************** Asignar Usuario *********************
        protected void BindDAsigUsu()
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SP_PANTALLA_Almacen 4,'','','','',@Alm,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@Alm", TxtCod.Text.Trim());
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdAsigUsu.DataSource = dtbl;
                GrdAsigUsu.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdAsigUsu.DataSource = dtbl;
                GrdAsigUsu.DataBind();
                GrdAsigUsu.Rows[0].Cells.Clear();
                GrdAsigUsu.Rows[0].Cells.Add(new TableCell());
                GrdAsigUsu.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdAsigUsu.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdAsigUsu.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void IbtCerrarAsigUsu_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void GrdAsigUsu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                try
                {
                    string VBQuery = "";
                    string VbCodUsu = (GrdAsigUsu.FooterRow.FindControl("DdlUsuPP") as DropDownList).Text.Trim();
                    if (VbCodUsu.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08Alm'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un usuario.
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "EXEC SP_TablasGeneral 9,@CP,@US,'','','','','','AsigUsu','INSERT',0,@Ac,@CA,0,0,0,'01-01-1','02-01-1','03-01-1'";
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                SC.Parameters.AddWithValue("@CA", TxtCod.Text.Trim());
                                SC.Parameters.AddWithValue("@Ac", (GrdAsigUsu.FooterRow.FindControl("CkbActivoPP") as CheckBox).Checked == false ? 0 : 1);
                                SC.Parameters.AddWithValue("@CP", VbCodUsu);
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
                                    BindDAsigUsu();
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
        protected void GrdAsigUsu_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdAsigUsu.EditIndex = e.NewEditIndex; BindDAsigUsu(); }
        protected void GrdAsigUsu_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int VblId = Convert.ToInt32(GrdAsigUsu.DataKeys[e.RowIndex].Value.ToString());

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 9,'',@US,'','','','','','AsigUsu','UPDATE',@id,@Ac,0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Ac", (GrdAsigUsu.Rows[e.RowIndex].FindControl("CkbActivo") as CheckBox).Checked == false ? 0 : 1);
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
                            GrdAsigUsu.EditIndex = -1;
                            BindDAsigUsu();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdAsigUsu_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAsigUsu.EditIndex = -1; BindDAsigUsu(); }
        protected void GrdAsigUsu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbCod = GrdAsigUsu.DataKeys[e.RowIndex].Value.ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 9,'',@US,'','','','','','AsigUsu','DELETE',@ID,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'";

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
                            BindDAsigUsu();
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
        protected void GrdAsigUsu_RowDataBound(object sender, GridViewRowEventArgs e)
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
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Almacen 5,'01','','','',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
                DropDownList DdlUsuPP = (e.Row.FindControl("DdlUsuPP") as DropDownList);
                DdlUsuPP.DataSource = Cnx.DSET(LtxtSql);
                DdlUsuPP.DataTextField = "Persona";
                DdlUsuPP.DataValueField = "CodUsuario";
                DdlUsuPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
        protected void GrdAsigUsu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAsigUsu.PageIndex = e.NewPageIndex; BindDAsigUsu(); }
    }
}
