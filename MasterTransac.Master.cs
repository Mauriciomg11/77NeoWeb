using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb
{
    public partial class MasterTransac : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblCia.Text = Session["SigCia"].ToString()+" - "+ Session["N77U"].ToString();
        }
        protected void IbnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
        }
    }
}