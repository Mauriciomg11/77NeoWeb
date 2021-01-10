using _77NeoWeb.prg;
using System;
using System.Web.UI;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class Frm_InfIngenieria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            } */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles /* */
            }
            if (!IsPostBack)
            {
                MlVw.ActiveViewIndex = 0;
                ModSeguridad();               
                Page.Title = "Reportes de ingeniería";
                TitForm.Text = "Reportes de ingeniería";
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1; // Procesos de ingenieria
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "Frm_InfIngenieria.aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0;  } //
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
            //IdiomaControles();
           // PerfilesGrid();
        }
        protected void BtnAdvice_Click(object sender, EventArgs e)
        {

        }

        protected void BtnInsRemElem_Click(object sender, EventArgs e)
        {

        }

        protected void BtnInsRemSubC_Click(object sender, EventArgs e)
        {

        }

        protected void BtnHistCont_Click(object sender, EventArgs e)
        {
    
            Response.Redirect("~/Forms/Ingenieria/FrmHistoricosContadores.aspx");
        }
        protected void BtnPnPlanti_Click(object sender, EventArgs e)
        {

        }

        protected void BtnProcIngeni_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/Ingenieria/FrmControlContadoresGeneral.aspx");
        }
    }
}