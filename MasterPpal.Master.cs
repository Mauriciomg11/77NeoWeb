using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb
{
    public partial class MasterPpal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["N77U"]!=null)
            {
            LblCia.Text = Session["SigCia"].ToString() + " - " + Session["N77U"].ToString();
            }
        }
    }
}