using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class Frm_InfIngenieria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoHCT";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "860064038-4"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles /* */
            }
            if (!IsPostBack)
            {
               
               // ModSeguridad();               
                Page.Title = "Reportes de ingeniería";
                TitForm.Text = "Reportes de ingeniería";
            }
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
    }
}