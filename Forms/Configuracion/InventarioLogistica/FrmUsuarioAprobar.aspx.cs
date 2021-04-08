using _77NeoWeb.prg;
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
    public partial class FrmUsuarioAprobar : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null) { Response.Redirect("~/FrmAcceso.aspx"); }/*  */
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
                BindBDdlPersonal();
                ViewState["Accion"] = "";
                Traerdatos();
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
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
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
                    LblUsuPpl.Text = bO.Equals("LblUsuPpl") ? bT : LblUsuPpl.Text;
                    lblTitUsuAprMyr.Text = bO.Equals("lblTitUsuAprMyr") ? bT : lblTitUsuAprMyr.Text;
                    LblUsuMyrAlt1.Text = bO.Equals("LblUsuMyrAlt1") ? bT : LblUsuMyrAlt1.Text;
                    LblUsuMyrAlt2.Text = bO.Equals("LblUsuMyrAlt2") ? bT : LblUsuMyrAlt2.Text;
                    lblTitUsuAprMnr.Text = bO.Equals("lblTitUsuAprMnr") ? bT : lblTitUsuAprMnr.Text;
                    lblTitUsuAprMnr.Text = bO.Equals("lblTitUsuAprMnr") ? bT : lblTitUsuAprMnr.Text;
                    LblUsuMnrPpl.Text = bO.Equals("LblUsuPpl") ? bT : LblUsuMnrPpl.Text;
                    LblUsuMnrAlt1.Text = bO.Equals("LblUsuMyrAlt1") ? bT : LblUsuMnrAlt1.Text;
                    lblTitUsuTRM.Text = bO.Equals("lblTitUsuTRM") ? bT : lblTitUsuTRM.Text;
                    LblUsuTrmPpl.Text = bO.Equals("LblUsuPpl") ? bT : LblUsuTrmPpl.Text;
                    LblUsuTrmAlt1.Text = bO.Equals("LblUsuMyrAlt1") ? bT : LblUsuTrmAlt1.Text;
                    LblTitValores.Text = bO.Equals("LblTitValores") ? bT : LblTitValores.Text;
                    LblMonedaLocal.Text = bO.Equals("LblMonedaLocal") ? bT : LblMonedaLocal.Text;
                    LblDolar.Text = bO.Equals("LblDolar") ? bT : LblDolar.Text;
                    LblEuro.Text = bO.Equals("LblEuro") ? bT : LblEuro.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;

                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdlPersonal()
        {
            string LtxtSql = string.Format(" EXEC SP_TablasGeneral 10,'','','','','','','','','',0,0,0,0,1,{0},'01-01-1','02-01-1','03-01-1'	", Session["!dC!@"]);
            DdllUsuPpl.DataSource = Cnx.DSET(LtxtSql);
            DdllUsuPpl.DataMember = "Datos";
            DdllUsuPpl.DataTextField = "Usuario";
            DdllUsuPpl.DataValueField = "CodUsuario";
            DdllUsuPpl.DataBind();

            DdlUsuMyrAlt1.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuMyrAlt1.DataMember = "Datos";
            DdlUsuMyrAlt1.DataTextField = "Usuario";
            DdlUsuMyrAlt1.DataValueField = "CodUsuario";
            DdlUsuMyrAlt1.DataBind();

            DdlUsuMyrAlt2.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuMyrAlt2.DataMember = "Datos";
            DdlUsuMyrAlt2.DataTextField = "Usuario";
            DdlUsuMyrAlt2.DataValueField = "CodUsuario";
            DdlUsuMyrAlt2.DataBind();

            DdlUsuMnrPpl.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuMnrPpl.DataMember = "Datos";
            DdlUsuMnrPpl.DataTextField = "Usuario";
            DdlUsuMnrPpl.DataValueField = "CodUsuario";
            DdlUsuMnrPpl.DataBind();

            DdlUsuMnrAlt1.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuMnrAlt1.DataMember = "Datos";
            DdlUsuMnrAlt1.DataTextField = "Usuario";
            DdlUsuMnrAlt1.DataValueField = "CodUsuario";
            DdlUsuMnrAlt1.DataBind();

            DdlUsuTrmPpl.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuTrmPpl.DataMember = "Datos";
            DdlUsuTrmPpl.DataTextField = "Usuario";
            DdlUsuTrmPpl.DataValueField = "CodUsuario";
            DdlUsuTrmPpl.DataBind();

            DdlUsuTrmAlt1.DataSource = Cnx.DSET(LtxtSql);
            DdlUsuTrmAlt1.DataMember = "Datos";
            DdlUsuTrmAlt1.DataTextField = "Usuario";
            DdlUsuTrmAlt1.DataValueField = "CodUsuario";
            DdlUsuTrmAlt1.DataBind();
        }
        protected void Traerdatos()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = "EXEC SP_TablasGeneral 10,'','','','','','','','','',0,0,0,0,2,@Prmtr,'01-01-1','02-01-1','03-01-1'";
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@Prmtr", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    DdllUsuPpl.Text = HttpUtility.HtmlDecode(SDR["CodUsuario"].ToString().Trim());
                    DdlUsuMyrAlt1.Text = HttpUtility.HtmlDecode(SDR["CodUsuAlter1"].ToString().Trim());
                    DdlUsuMyrAlt2.Text = HttpUtility.HtmlDecode(SDR["CodUsuAlter2"].ToString().Trim());
                    DdlUsuMnrPpl.Text = HttpUtility.HtmlDecode(SDR["CodUsuarioAprobMenor"].ToString().Trim());
                    DdlUsuMnrAlt1.Text = HttpUtility.HtmlDecode(SDR["CodUsuAprobMenorAlter"].ToString().Trim());
                    DdlUsuTrmPpl.Text = HttpUtility.HtmlDecode(SDR["Usr1CrearTrm"].ToString().Trim());
                    DdlUsuTrmAlt1.Text = HttpUtility.HtmlDecode(SDR["Usr2CrearTrm"].ToString().Trim());
                    MonLocal.Text = HttpUtility.HtmlDecode(SDR["MonLocal"].ToString().Trim());
                    MonUSD.Text = HttpUtility.HtmlDecode(SDR["MonUSD"].ToString().Trim());
                    MonEUR.Text = HttpUtility.HtmlDecode(SDR["MonEUR"].ToString().Trim());
                    TxtMonedaLocal.Text = HttpUtility.HtmlDecode(SDR["ValorCOP"].ToString().Trim());
                    TxtDolar.Text = HttpUtility.HtmlDecode(SDR["ValorUSD"].ToString().Trim());
                    TxtEuro.Text = HttpUtility.HtmlDecode(SDR["ValorEURO"].ToString().Trim());
                    ViewState["IDUsAp"]= HttpUtility.HtmlDecode(SDR["Id"].ToString().Trim());
                }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        { BtnModificar.Enabled = Md; }
        protected void ActivarCampos(bool Ing, bool Edi, bool Vble,bool VbCurrent, string accion)
        {
            DdllUsuPpl.Enabled = Edi; DdlUsuMyrAlt1.Enabled = Edi; DdlUsuMyrAlt2.Enabled = Edi; DdlUsuMnrPpl.Enabled = Edi; DdlUsuMnrAlt1.Enabled = Edi;
            DdlUsuTrmPpl.Enabled = Edi; DdlUsuTrmAlt1.Enabled = Edi;
            TxtMonedaLocal.Enabled = Edi; TxtDolar.Enabled = Edi; TxtEuro.Enabled = Edi;
            TxtMonedaLocal.Visible = Vble; TxtDolar.Visible = Vble; TxtEuro.Visible = Vble;
            MonLocal.Visible = VbCurrent; MonUSD.Visible = VbCurrent; MonEUR.Visible = VbCurrent;
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            string Mensj = "";
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (ViewState["Accion"].ToString().Equals(""))
            {
                ActivarBtn(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                ViewState["Accion"] = "Aceptar";
                ActivarCampos(false, true, true, false, "UPDATE");
                Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
            }
            else
            {               
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 10,@MyP,@My1,@My2,@MnP,@Mn1,@TrP,@Tr1,@Us,'',@ML,@MD,@ME,@Id,3,0,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@MyP", DdllUsuPpl.Text.Trim());
                                SC.Parameters.AddWithValue("@My1", DdlUsuMyrAlt1.Text.Trim());
                                SC.Parameters.AddWithValue("@My2", DdlUsuMyrAlt2.Text.Trim());
                                SC.Parameters.AddWithValue("@MnP", DdlUsuMnrPpl.Text.Trim());
                                SC.Parameters.AddWithValue("@Mn1", DdlUsuMnrAlt1.Text.Trim());
                                SC.Parameters.AddWithValue("@TrP", DdlUsuTrmPpl.Text.Trim());
                                SC.Parameters.AddWithValue("@Tr1", DdlUsuTrmAlt1.Text.Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@ML", TxtMonedaLocal.Text.Trim());
                                SC.Parameters.AddWithValue("@MD", TxtDolar.Text.Trim());
                                SC.Parameters.AddWithValue("@ME", TxtEuro.Text.Trim());
                                SC.Parameters.AddWithValue("@Id", ViewState["IDUsAp"]);
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
                                ActivarCampos(false, false, false, true, "UPDATE");
                                DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                                foreach (DataRow row in Result)
                                { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                                ViewState["Accion"] = "";
                                Traerdatos();
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
    }
}