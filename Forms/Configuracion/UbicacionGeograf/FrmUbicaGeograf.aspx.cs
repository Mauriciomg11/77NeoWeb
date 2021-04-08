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

namespace _77NeoWeb.Forms.Configuracion.UbicacionGeograf
{
    public partial class FrmUbicaGeograf : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["Login77"] == null) { Response.Redirect("~/FrmAcceso.aspx"); }*/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";// 00000082|00000133
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["!dC!@"] = 0;
                Session["77IDM"] = "5"; // 4 español | 5 ingles   /* */
            }
            if (!IsPostBack)
            {
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
                    LblTipoUbc.Text = bO.Equals("LblTipoUbc") ? bT : LblTipoUbc.Text;
                    LblUbicaSupr.Text = bO.Equals("LblUbicaSupr") ? bT : LblUbicaSupr.Text;
                    LblVlrTasa.Text = bO.Equals("LblVlrTasa") ? bT : LblVlrTasa.Text;                  
                    CkbActivo.Text = bO.Equals("CkbActivo") ? "&nbsp" + bT : CkbActivo.Text;
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
        protected void BindBDdlBusq()
        {
            string LtxtSql = string.Format("EXEC SP_TablasGeneral 13,'','','','','','','','Todas','UbicaGeo',0,0,0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"]);
            DdlBusq.DataSource = Cnx.DSET(LtxtSql);
            DdlBusq.DataMember = "Datos";
            DdlBusq.DataTextField = "Nombre";
            DdlBusq.DataValueField = "IdUbicaGeogr";
            DdlBusq.DataBind();
        }
        protected void BindBDdl()
        {
            string LtxtSql = string.Format("EXEC SP_TablasGeneral 13,'','','','','','','','','TipoUbcac',0,0,0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"]);
            DdlTipoUbc.DataSource = Cnx.DSET(LtxtSql);
            DdlTipoUbc.DataMember = "Datos";
            DdlTipoUbc.DataTextField = "Descripcion";
            DdlTipoUbc.DataValueField = "TipoUbicaGeogr";
            DdlTipoUbc.DataBind();

            LtxtSql = string.Format("EXEC SP_TablasGeneral 13,'','','','','','','','','UbicSup',0,0,0,0,0,{0},'01-01-1','02-01-1','03-01-1'", Session["!dC!@"]);
            DdlUbicaSupr.DataSource = Cnx.DSET(LtxtSql);
            DdlUbicaSupr.DataMember = "Datos";
            DdlUbicaSupr.DataTextField = "Nombre";
            DdlUbicaSupr.DataValueField = "CodUbicaGeogr";
            DdlUbicaSupr.DataBind();
        }
        protected void LimpiarCampos(string Accion)
        {
            TxtCod.Text = ""; TxtNombre.Text = "";  DdlTipoUbc.Text = ""; DdlUbicaSupr.Text = ""; TxtVlrTasa.Text = "0";
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
            if (DdlTipoUbc.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01UG'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La ubicación es requerida.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlUbicaSupr.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02UG'");
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
            TxtCod.Enabled = Ing; TxtNombre.Enabled = Edi; DdlTipoUbc.Enabled = Edi; DdlUbicaSupr.Enabled = Edi; TxtVlrTasa.Enabled = Edi;
            if (accion.Trim().Equals("UPDATE")) { CkbActivo.Enabled = Edi; }
        }
        protected void Traerdatos(string Prmtr)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

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
                    TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodUbicaGeogr"].ToString().Trim());
                    TxtNombre.Text = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                    DdlTipoUbc.Text = HttpUtility.HtmlDecode(SDR["CodTipoUbicaGeogr"].ToString().Trim());
                    DdlUbicaSupr.Text = HttpUtility.HtmlDecode(SDR["CodUbicaGeoSup"].ToString().Trim());
                    TxtVlrTasa.Text = HttpUtility.HtmlDecode(SDR["VlorTasa"].ToString().Trim());
                    CkbActivo.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(SDR["Activo"].ToString().Trim()));
                }
                SDR.Close();
                Cnx2.Close();
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        { Traerdatos(DdlBusq.Text.Trim()); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEliminar_Click(object sender, EventArgs e)
        {

        }
    }
}