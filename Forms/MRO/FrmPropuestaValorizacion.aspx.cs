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

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmPropuestaValorizacion : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DtDdlPpal = new DataTable();
        DataTable DtDet = new DataTable();
        DataSet DS = new DataSet();
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
                ModSeguridad();
                BindDdlPpal("UPDATE");
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//                          

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

                }


                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
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
                            using (DataSet DS = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DS);

                                DS.Tables[0].TableName = "PPTS";
                                DS.Tables[1].TableName = "MonedaLocal";
                                ViewState["DS"] = DS;
                            }
                        }
                    }
                }
            }
            DS = (DataSet)ViewState["DS"];
            DtDdlPpal = DS.Tables["PPTS"].Clone();
            DtDdlPpal = DS.Tables["PPTS"];
            DtDdlPpal.Rows.Add(" - ", "", "", "", "", "", "", "", "", "", "01/01/1900", "0");
            DataView DV = DtDdlPpal.DefaultView;
            DV.Sort = "OrdenPpta";
            DtDdlPpal = DV.ToTable();
            ViewState["DtDdlPpal"] = DtDdlPpal;

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
        protected void IbtConsult_Click(object sender, ImageClickEventArgs e)
        {

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

        protected void TxtVlr_TextChanged(object sender, EventArgs e)
        {
            DS = (DataSet)ViewState["DS"];
            var ControlAct = (Control)sender;
            GridViewRow row = (GridViewRow)ControlAct.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtVlr = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtVlr");
            TextBox TxtMnda = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtMnda");
            TxtVlr.Text = TxtVlr.Text.Equals("") ? "0" : TxtVlr.Text.Trim();
            if (Convert.ToDouble(TxtVlr.Text) > 0)
            {
                if (TxtMnda.Text.Equals(""))
                {
                    foreach (DataRow Row in DS.Tables[1].Rows)
                    {
                        TxtMnda.Text = Row["CodMoneda"].ToString().Trim();
                        TxtMnda.AutoPostBack = true;
                    }
                }
            }
            else { TxtMnda.Text = ""; }
        }
        protected void CkbGenrSP_CheckedChanged(object sender, EventArgs e)
        {
            var CntrlGrd = (Control)sender;
            GridViewRow row = (GridViewRow)CntrlGrd.NamingContainer;
            int rowIndex = row.RowIndex;
            CheckBox CkbGenrSP = (CheckBox)GrdDetValrzc.Rows[rowIndex].FindControl("CkbGenrSP");
            Label CantPpt = (Label)GrdDetValrzc.Rows[rowIndex].FindControl("CantPpt");
            TextBox TxtCantSP = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtCantSP");
            TxtCantSP.Text = CkbGenrSP.Checked == true ? CantPpt.Text : "0";
        }
        protected void GrdDetValrzc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

    }
}