using System;

namespace _77NeoWeb
{
    public partial class MasterReport : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LblCia.Text = Session["SigCia"].ToString();
        }
    }
}