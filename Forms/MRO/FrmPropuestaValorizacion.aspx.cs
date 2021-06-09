using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmPropuestaValorizacion : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DtDdlPpal = new DataTable();
        DataTable DtDet = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "";
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
                    Session["77IDM"] = "5"; // 4 español | 5 ingles  */
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                // ModSeguridad();
                BindDdlPpal("UPDATE");
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void BindDdlPpal(string Accion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "SP_PANTALLA_Valorizacion 1,'','','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DtDdlPpal);
                            ViewState["DtDdlPpal"] = DtDdlPpal;
                        }
                    }
                }
            }
            DtDdlPpal = (DataTable)ViewState["DtDdlPpal"];
            DtDdlPpal.Rows.Add(" - ", "", "", "", "", "", "", "", "", "", "01/01/1900", "0");
            DataView DV = DtDdlPpal.DefaultView;
            DV.Sort = "OrdenPpta";
            DtDdlPpal = DV.ToTable();
            DdlNumPpt.DataSource = DtDdlPpal;
            DdlNumPpt.DataTextField = "IdPropuesta";
            DdlNumPpt.DataValueField = "OrdenPpta";
            DdlNumPpt.DataBind();
        }
        protected void BindDetalle(string Accion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC DetalleValorizacion @PT,@ICC";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@PT", DdlNumPpt.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DtDet);
                            ViewState["DtDet"] = DtDet;
                        }
                    }
                }
            }
            DtDet = (DataTable)ViewState["DtDet"];
            if (DtDet.Rows.Count > 0) { GrdDetValrzc.DataSource = DtDet; }
            else { GrdDetValrzc.DataSource = null; }
            GrdDetValrzc.DataBind();
        }
        protected void IbtConsult_Click(object sender, ImageClickEventArgs e)
        {
            DtDdlPpal = (DataTable)ViewState["DtDdlPpal"];
            TxtCliente.Text = "";
            TxtDescTipoPPT.Text = "";
            TxtDesEstado.Text = "";
            TxtDescPptTipoSol.Text = "";
            DataRow[] Result = DtDdlPpal.Select("IdPropuesta='" + DdlNumPpt.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            {

                if (!Row["FechaAprobacion"].ToString().Trim().Equals("")) { BtnValorizar.Visible = false; BtnReValorizar.Visible = false; BtnPlantilla.Visible = false; BtnSolPed.Visible = false; }
                else { BtnValorizar.Visible = true; BtnReValorizar.Visible = true; BtnPlantilla.Visible = true; BtnSolPed.Visible = true; }
                TxtCliente.Text = Row["RazonSocial"].ToString().Trim();
                TxtDescTipoPPT.Text = Row["DescripcionPropuesta"].ToString().Trim();
                TxtDesEstado.Text = Row["DescripcionEstado"].ToString().Trim();
                TxtDescPptTipoSol.Text = Row["Descripcion"].ToString().Trim();
            }
            BindDetalle("UPDATE");
        }

        protected void BtnPNSinValorizar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnSolPed_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCotizacion_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCuadroComprtv_Click(object sender, EventArgs e)
        {

        }

        protected void BtnValorizar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnReValorizar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnPlantilla_Click(object sender, EventArgs e)
        {

        }

        protected void BtnExportar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDetValrzc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }



        protected void TxtVlr_TextChanged(object sender, EventArgs e)
        {
            /*foreach (GridViewRow grvRow in GrdDetValrzc.Rows)
             {
                 TextBox TxtVlr = (TextBox)grvRow.FindControl("TxtVlr");
                 TextBox TxtMnda = (TextBox)grvRow.FindControl("TxtMnda");
                 TxtVlr.Text = TxtVlr.Text.Equals("") ? "0" : TxtVlr.Text.Trim();
                 if (Convert.ToDouble(TxtVlr.Text) > 0 && TxtMnda.Text.Equals("")) { TxtMnda.Text = "COP"; }
                 else { TxtMnda.Text = ""; }
             }

             foreach (GridViewRow dtgItem in this.GrdDetValrzc.Rows)
             {
                 TextBox TxtVlr = (TextBox)GrdDetValrzc.Rows[dtgItem.RowIndex].FindControl("TxtVlr");
                 TextBox TxtMnda = (TextBox)GrdDetValrzc.Rows[dtgItem.RowIndex].FindControl("TxtMnda");
                 TxtMnda.Text = "COP";
             }*/

            var ControlAct = (Control)sender;
            GridViewRow row = (GridViewRow)ControlAct.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtVlr = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtVlr");
            TextBox TxtMnda = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtMnda");
            TxtVlr.Text = TxtVlr.Text.Equals("") ? "0" : TxtVlr.Text.Trim();
            if (Convert.ToDouble(TxtVlr.Text) > 0 && TxtMnda.Text.Equals("")) { TxtMnda.Text = "COP"; }
            else { TxtMnda.Text = ""; }
        }


        protected void DdlNumPpt_TextChanged(object sender, EventArgs e)
        {
            DtDdlPpal = (DataTable)ViewState["DtDdlPpal"];
            TxtCliente.Text = "";
            TxtDescTipoPPT.Text = "";
            TxtDesEstado.Text = "";
            TxtDescPptTipoSol.Text = "";
            DataRow[] Result = DtDdlPpal.Select("IdPropuesta='" + DdlNumPpt.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            {

                if (!Row["FechaAprobacion"].ToString().Trim().Equals("")) { BtnValorizar.Visible = false; BtnReValorizar.Visible = false; BtnPlantilla.Visible = false; BtnSolPed.Visible = false; }
                else { BtnValorizar.Visible = true; BtnReValorizar.Visible = true; BtnPlantilla.Visible = true; BtnSolPed.Visible = true; }
                TxtCliente.Text = Row["RazonSocial"].ToString().Trim();
                TxtDescTipoPPT.Text = Row["DescripcionPropuesta"].ToString().Trim();
                TxtDesEstado.Text = Row["DescripcionEstado"].ToString().Trim();
                TxtDescPptTipoSol.Text = Row["Descripcion"].ToString().Trim();
            }
            BindDetalle("UPDATE");
        }
    }
}