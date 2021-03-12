using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmHistoricosContadores : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
           {
               Response.Redirect("~/FrmAcceso.aspx");
           } /**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles  */
            }
            if (!IsPostBack)
            {
                ModSeguridad();
                BIndDContNull();
                RdbHisC1AplicAK.Checked = true;
                RdbHisC2AplicAK.Checked = true;
                BindBDdlAK("1"); BindBDdlAK("2");
                ViewState["Validar"] = "S";
                TxtFechIPpl.Text = "2020-01-01";
                TxtFechFPpl.Text = "2020-12-31";
                //Page.Title = "Histórico contadores";
                //TitForm.Text = "Histórico de Contadores";
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
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "Frm_InfIngenieria.aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; GrdHisC1.ShowFooter = false; GrdHisC2.ShowFooter = false; } //eliminar historico
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
            IdiomaControles();
            PerfilesGrid();
        }
        protected void IdiomaControles()
        {
            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
            {
                string Idima = Session["77IDM"].ToString().Trim();
                string NomF = ViewState["PFileName"].ToString().Trim();
                string LtxtSql = "EXEC Idioma @I,@F1,@F2,@F3,@F4";
                SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                SC.Parameters.AddWithValue("@I", Idima);
                SC.Parameters.AddWithValue("@F1", NomF);
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
                    if (bO.Equals("TituloHstC"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("CaptionHst") ? bT : TitForm.Text;
                    //**************************************Contadores con valor Null**********************************************
                    LblTitContNull.Text = bO.Equals("LblTitContNull") ? bT : LblTitContNull.Text;
                    IbtClosContNull.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClosContNull.ToolTip;
                    GrdContNull.Columns[0].HeaderText = bO.Equals("GrdContNullHk") ? bT : GrdContNull.Columns[0].HeaderText;
                    GrdContNull.Columns[3].HeaderText = bO.Equals("GrdContNullCont") ? bT : GrdContNull.Columns[3].HeaderText;
                    GrdContNull.Columns[4].HeaderText = bO.Equals("GrdContNullDesc") ? bT : GrdContNull.Columns[4].HeaderText;
                    LblFechIPpl.Text = bO.Equals("LblFechIPpl") ? bT : LblFechIPpl.Text;
                    LblFechFPpl.Text = bO.Equals("LblFechFPpl") ? bT : LblFechFPpl.Text;
                    RdbHisC1AplicAK.Text = bO.Equals("GrdContNullHk") ? bT : RdbHisC1AplicAK.Text;
                    RdbHisC1AplicSN.Text = bO.Equals("RdbHisC1AplicSN") ? bT : RdbHisC1AplicSN.Text;
                    //BtnHisC1Consult.Text = bO.Equals("BtnHisC1Consult") ? bT : BtnHisC1Consult.Text;
                    IbtHisC1Find.ToolTip = bO.Equals("BtnHisC1ConsultTT") ? bT : IbtHisC1Find.ToolTip;
                    LblHisVlrIndiv1.Text = bO.Equals("LblHisVlrIndiv1") ? bT : LblHisVlrIndiv1.Text;
                    LblHisVlrAcumv1.Text = bO.Equals("LblHisVlrAcumv1") ? bT : LblHisVlrAcumv1.Text;
                    LblHisC1HK.Text = bO.Equals("GrdContNullHk") ? bT : LblHisC1HK.Text;
                    LblHisC1CodCont.Text = bO.Equals("LblHisC1CodCont") ? bT : LblHisC1CodCont.Text;
                    GrdHisC1.Columns[0].HeaderText = bO.Equals("GrdHisC1Fecha") ? bT : GrdHisC1.Columns[0].HeaderText;
                    GrdHisC1.Columns[1].HeaderText = bO.Equals("LblHisVlrIndiv1") ? bT : GrdHisC1.Columns[1].HeaderText;
                    GrdHisC1.Columns[2].HeaderText = bO.Equals("LblHisVlrAcumv1") ? bT : GrdHisC1.Columns[2].HeaderText;
                    GrdHisC1.Columns[3].HeaderText = bO.Equals("GrdHisC1LV") ? bT : GrdHisC1.Columns[3].HeaderText;
                    IbtHisC1Excel.ToolTip = bO.Equals("IbtHisC1Excel") ? bT : IbtHisC1Excel.ToolTip;

                    RdbHisC2AplicAK.Text = bO.Equals("GrdContNullHk") ? bT : RdbHisC2AplicAK.Text;
                    RdbHisC2AplicSN.Text = bO.Equals("RdbHisC1AplicSN") ? bT : RdbHisC2AplicSN.Text;
                    //BtnHisC2Consult.Text = bO.Equals("BtnHisC1Consult") ? bT : BtnHisC2Consult.Text;
                    IbtHisC2Find.ToolTip = bO.Equals("BtnHisC1ConsultTT") ? bT : IbtHisC2Find.ToolTip;
                    LblHisVlrIndiv2.Text = bO.Equals("LblHisVlrIndiv1") ? bT : LblHisVlrIndiv2.Text;
                    LblHisVlrAcumv2.Text = bO.Equals("LblHisVlrAcumv1") ? bT : LblHisVlrAcumv2.Text;
                    LblHisC2HK.Text = bO.Equals("GrdContNullHk") ? bT : LblHisC2HK.Text;
                    LblHisC2CodCont.Text = bO.Equals("LblHisC1CodCont") ? bT : LblHisC2CodCont.Text;
                    GrdHisC2.Columns[0].HeaderText = bO.Equals("GrdHisC1Fecha") ? bT : GrdHisC2.Columns[0].HeaderText;
                    GrdHisC2.Columns[1].HeaderText = bO.Equals("LblHisVlrIndiv1") ? bT : GrdHisC2.Columns[1].HeaderText;
                    GrdHisC2.Columns[2].HeaderText = bO.Equals("LblHisVlrAcumv1") ? bT : GrdHisC2.Columns[2].HeaderText;
                    GrdHisC2.Columns[3].HeaderText = bO.Equals("GrdHisC1LV") ? bT : GrdHisC2.Columns[3].HeaderText;
                    IbtHisC2Excel.ToolTip = bO.Equals("IbtHisC1Excel") ? bT : IbtHisC2Excel.ToolTip;
                    /*if (bO.Equals("placeholderBq"))
                    {
                        TxtWSBusq.Attributes.Add("placeholder", bT);
                    }*/
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdHisC1.Rows)
            {

                if ((int)ViewState["VblCE1"] == 0)
                {
                    ImageButton IbtDelete = Row.FindControl("IbtDelete") as ImageButton;
                    if (IbtDelete != null)
                    { IbtDelete.Visible = false; }
                }
            }
            foreach (GridViewRow Row in GrdHisC2.Rows)
            {

                if ((int)ViewState["VblCE1"] == 0)
                {
                    ImageButton IbtDelete = Row.FindControl("IbtDelete") as ImageButton;
                    if (IbtDelete != null)
                    { IbtDelete.Visible = false; }
                }
            }
        }
        protected void BIndDContNull()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Formulario_Historico 4,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdContNull.DataSource = DtB;
                            GrdContNull.DataBind();
                            MlVw.ActiveViewIndex = 1;
                        }
                        else
                        {
                            GrdContNull.DataSource = null;
                            GrdContNull.DataBind();
                            MlVw.ActiveViewIndex = 0;
                        }
                    }
                }
            }
        }
        protected void IbtClosContNull_Click(object sender, ImageClickEventArgs e)
        {
            MlVw.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        //******************************** Panel historico UNO *****************************************
        protected void BindBDdlAK(string Panel)
        {
            string LtxtSql = "EXEC SP_PANTALLA_Status 11,'','','','HK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";

            if (Panel.Equals("1"))
            {
                DdlHisC1HK.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC1HK.DataMember = "Datos";
                DdlHisC1HK.DataTextField = "Matricula";
                DdlHisC1HK.DataValueField = "CodAeronave";
                DdlHisC1HK.DataBind();
            }
            else
            {
                DdlHisC2HK.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC2HK.DataMember = "Datos";
                DdlHisC2HK.DataTextField = "Matricula";
                DdlHisC2HK.DataValueField = "CodAeronave";
                DdlHisC2HK.DataBind();
            }
        }
        protected void BindBDdlCodCont(string Hk, string PN, string Panel)
        {
            string LtxtSql = "";
            if (!PN.Trim().Equals(""))
            { LtxtSql = string.Format("EXEC SP_PANTALLA_Informe_Ingenieria 16,'{0}','','','ContPN',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", PN); }
            else
            { LtxtSql = string.Format("EXEC SP_PANTALLA__Historico_Contadores 3,'','','','','',{0}, 0,0,0,'01-01-1','01-01-1','01-01-1'", Hk); }
            if (Panel.Equals("1"))
            {
                DdlHisC1CodCont.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC1CodCont.DataMember = "Datos";
                DdlHisC1CodCont.DataTextField = "CodContador";
                DdlHisC1CodCont.DataValueField = "CodContador";
                DdlHisC1CodCont.DataBind();
            }
            else
            {
                DdlHisC2CodCont.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC2CodCont.DataMember = "Datos";
                DdlHisC2CodCont.DataTextField = "CodContador";
                DdlHisC2CodCont.DataValueField = "CodContador";
                DdlHisC2CodCont.DataBind();
            }
        }
        protected void BindBDdlPNHC(string Panel)
        {
            string LtxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 16,'','','','PNHC',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            if (Panel.Equals("1"))
            {
                DdlHisC1PN.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC1PN.DataMember = "Datos";
                DdlHisC1PN.DataTextField = "PN";
                DdlHisC1PN.DataValueField = "Codigo";
                DdlHisC1PN.DataBind();
            }
            else
            {
                DdlHisC2PN.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC2PN.DataMember = "Datos";
                DdlHisC2PN.DataTextField = "PN";
                DdlHisC2PN.DataValueField = "Codigo";
                DdlHisC2PN.DataBind();
            }
        }
        protected void BindBDdlSNHC(string PN, string Panel)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Informe_Ingenieria 16,'{0}','','','SNHC',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", PN);
            if (Panel.Equals("1"))
            {
                DdlHisC1SN.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC1SN.DataMember = "Datos";
                DdlHisC1SN.DataTextField = "SN";
                DdlHisC1SN.DataValueField = "Codigo";
                DdlHisC1SN.DataBind();
            }
            else
            {
                DdlHisC2SN.DataSource = Cnx.DSET(LtxtSql);
                DdlHisC2SN.DataMember = "Datos";
                DdlHisC2SN.DataTextField = "SN";
                DdlHisC2SN.DataValueField = "Codigo";
                DdlHisC2SN.DataBind();
            }
        }
        protected void RdbHisC1AplicAK_CheckedChanged(object sender, EventArgs e)
        {
            BindBDdlAK("1");
            LblHisC1HK.Visible = true;
            DdlHisC1HK.Visible = true;
            LblHisC1PN.Visible = false;
            DdlHisC1PN.Visible = false;
            LblHisC1SN.Visible = false;
            DdlHisC1SN.Visible = false;
            string LtxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 16,'','','','BLANK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlHisC1CodCont.DataSource = Cnx.DSET(LtxtSql);
            DdlHisC1CodCont.DataMember = "Datos";
            DdlHisC1CodCont.DataTextField = "Descripcion";
            DdlHisC1CodCont.DataValueField = "CodContador";
            DdlHisC1CodCont.DataBind();
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
            TxtHisVlrIndiv1.Text = "0";
            TxtHisVlrAcumv1.Text = "0";
            LblTitHisC1Aplicab.Text = "";

        }
        protected void RdbHisC1AplicSN_CheckedChanged(object sender, EventArgs e)
        {
            BindBDdlPNHC("1");
            BindBDdlSNHC(DdlHisC1PN.Text.Trim(), "1");
            LblHisC1HK.Visible = false;
            DdlHisC1HK.Visible = false;
            LblHisC1PN.Visible = true;
            DdlHisC1PN.Visible = true;
            LblHisC1SN.Visible = true;
            DdlHisC1SN.Visible = true;
            string LtxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 16,'','','','BLANK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlHisC1CodCont.DataSource = Cnx.DSET(LtxtSql);
            DdlHisC1CodCont.DataMember = "Datos";
            DdlHisC1CodCont.DataTextField = "Descripcion";
            DdlHisC1CodCont.DataValueField = "CodContador";
            DdlHisC1CodCont.DataBind();
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
            TxtHisVlrIndiv1.Text = "0";
            TxtHisVlrAcumv1.Text = "0";
            LblTitHisC1Aplicab.Text = "";
        }
        protected void DdlHisC1HK_TextChanged(object sender, EventArgs e)
        {
            BindBDdlCodCont(DdlHisC1HK.Text, "", "1");
            DdlHisC1CodCont.DataBind();
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
        }
        protected void DdlHisC1PN_TextChanged(object sender, EventArgs e)
        {
            BindBDdlSNHC(DdlHisC1PN.Text.Trim(), "1");
            BindBDdlCodCont("0", DdlHisC1PN.Text.Trim(), "1");
            DdlHisC1CodCont.DataBind();
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
        }
        protected void DdlHisC1SN_TextChanged(object sender, EventArgs e)
        {
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
        }
        protected void DdlHisC1CodCont_TextChanged(object sender, EventArgs e)
        {
            GrdHisC1.DataSource = null;
            GrdHisC1.DataBind();
        }
        protected void BIndDHistorico()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                try
                {
                    string VbTxtSql = "";
                    if (RdbHisC1AplicAK.Checked == true)
                    { VbTxtSql = "EXEC SP_PANTALLA__Historico_Contadores 1,@Ct,'','','','',@HK, 0,0,0,@FI,@FF,'01-01-1'"; }
                    else { VbTxtSql = "EXEC SP_PANTALLA__Historico_Contadores 5,@Ct,@CE,'','','',0, 0,0,0,@FI,@FF,'01-01-1'"; }
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        DateTime d1 = Convert.ToDateTime(TxtFechIPpl.Text.Trim());
                        SC.Parameters.AddWithValue("@Ct", DdlHisC1CodCont.Text.Trim());
                        SC.Parameters.AddWithValue("@HK", DdlHisC1HK.Text.Trim());
                        SC.Parameters.AddWithValue("@CE", DdlHisC1SN.Text.Trim());
                        SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIPpl.Text.Trim()));
                        SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFPpl.Text.Trim()));
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);
                            if (DtB.Rows.Count > 0)
                            {
                                GrdHisC1.DataSource = DtB;
                                GrdHisC1.DataBind();
                            }
                            else
                            {
                                DtB.Rows.Add(DtB.NewRow());
                                GrdHisC1.DataSource = DtB;
                                GrdHisC1.DataBind();
                                GrdHisC1.Rows[0].Cells.Clear();
                                GrdHisC1.Rows[0].Cells.Add(new TableCell());
                                GrdHisC1.Rows[0].Cells[0].ColumnSpan = DtB.Columns.Count;
                                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC00'");
                                foreach (DataRow row in Result)
                                { GrdHisC1.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                                GrdHisC1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                catch (Exception ex)
                { string b1 = ex.ToString(); }
            }
        }
        protected void ValidarConsulta()
        {
            ViewState["Validar"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechIPpl.Text.Equals("") || TxtFechFPpl.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                ViewState["Validar"] = "N"; return;
            }
            if (TxtFechIPpl.Text.Length > 10 || TxtFechFPpl.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }// 
                ViewState["Validar"] = "N"; return;
            }
            if (RdbHisC1AplicAK.Checked == true && DdlHisC1HK.Text.Equals("0"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC02'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe ingresar una aeronave.');", true);
                ViewState["Validar"] = "N"; return;
            }
            if (RdbHisC1AplicSN.Checked == true && DdlHisC1SN.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC03'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una serie.');", true);
                ViewState["Validar"] = "N"; return;
            }
            if (DdlHisC1CodCont.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC04'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un contador.');", true);
                ViewState["Validar"] = "N"; return;
            }
        }       
        protected void IbtHisC1Find_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ValidarConsulta();
            if (ViewState["Validar"].ToString().Equals("N")) { return; }

            BIndDHistorico();
            foreach (GridViewRow Row in GrdHisC1.Rows)
            {
                TxtHisVlrIndiv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[2].ToString();
                TxtHisVlrAcumv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[3].ToString();
                LblTitHisC1Aplicab.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[4].ToString();
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void GrdHisC1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {

                if ((GrdHisC1.FooterRow.FindControl("TxtHC1FechaPP") as TextBox).Text.Trim().Equals("") || (GrdHisC1.FooterRow.FindControl("TxtHC1FechaPP") as TextBox).Text.Trim().Length > 10)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Fecha inválida
                    return;
                }
                CultureInfo Culture = new CultureInfo("en-US");
                string VblTxtCant = (GrdHisC1.FooterRow.FindControl("TxtVlrIndivPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdHisC1.FooterRow.FindControl("TxtVlrIndivPP") as TextBox).Text.Trim();
                double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 18,@Cnt,@Us,@CEl,'',@CHK,@Vr,0,0,@FI,'01-01-1900','01-01-1900'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string VbHK = "0";
                                if (RdbHisC1AplicAK.Checked == true) { VbHK = DdlHisC1HK.Text; }
                                SC.Parameters.AddWithValue("@Cnt", DdlHisC1CodCont.Text.Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CHK", VbHK);
                                SC.Parameters.AddWithValue("@CEl", DdlHisC1SN.Text.Trim());
                                SC.Parameters.AddWithValue("@FI", Convert.ToDateTime((GrdHisC1.FooterRow.FindControl("TxtHC1FechaPP") as TextBox).Text.Trim()));
                                SC.Parameters.AddWithValue("@Vr", VblCant);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BIndDHistorico();
                                foreach (GridViewRow Row in GrdHisC1.Rows)
                                {
                                    LblTitHisC1Aplicab.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[4].ToString();
                                    TxtHisVlrIndiv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[2].ToString();
                                    TxtHisVlrAcumv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[3].ToString();
                                }
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Historico", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdHisC1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery = "";
            int IdHist = Convert.ToInt32(GrdHisC1.DataKeys[e.RowIndex].Values[0].ToString());
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    if (RdbHisC1AplicAK.Checked == true) { VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 17,@Cnt,@Us,@MAT,'HK',@CHis,@CHK,0,0,'01-1-2009','01-01-1900','01-01-1900'"; }
                    else { VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 17,@Cnt,@Us,@SN,'ELEM',@CHis,0,@CEl,0,'01-1-2009','01-01-1900','01-01-1900'"; }
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Cnt", DdlHisC1CodCont.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            if (RdbHisC1AplicAK.Checked == true) { SC.Parameters.AddWithValue("@MAT", DdlHisC1HK.SelectedItem.Text.Trim()); }
                            if (RdbHisC1AplicSN.Checked == true) { SC.Parameters.AddWithValue("@SN", DdlHisC1SN.SelectedItem.Text.Trim()); }
                            SC.Parameters.AddWithValue("@CHis", IdHist);
                            SC.Parameters.AddWithValue("@CHK", DdlHisC1HK.Text);
                            SC.Parameters.AddWithValue("@CEl", DdlHisC1SN.Text.Trim());
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BIndDHistorico();
                            foreach (GridViewRow Row in GrdHisC1.Rows)
                            {
                                TxtHisVlrIndiv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[2].ToString();
                                TxtHisVlrAcumv1.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[3].ToString();
                                LblTitHisC1Aplicab.Text = GrdHisC1.DataKeys[Row.RowIndex].Values[4].ToString();
                            }
                        }
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }

                    }
                }
            }
        }
        protected void GrdHisC1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
            if (IbtDelete != null)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDelete'");
                foreach (DataRow row in Result)
                { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                foreach (DataRow row in Result)
                { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
            }
        }
        //******************************** EXportar *****************************************
        protected void IbtHisC1Excel_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                ValidarConsulta();
                if (ViewState["Validar"].ToString().Equals("N")) { return; }
                string query = "", VbNomArchivo = "";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportarConsulHistorico", Session["77IDM"].ToString().Trim());
                if (RdbHisC1AplicAK.Checked == true)
                {
                    query = "EXEC SP_PANTALLA__Historico_Contadores 1,@Ct,'','','','CurExportarConsulHistorico',@HK, 0,0,0,@FI,@FF,'01-01-1'";
                    DataRow[] Result = Idioma.Select("Objeto= 'HCNomArcHK'");
                    foreach (DataRow row in Result)
                    { VbNomArchivo = row["Texto"].ToString().Trim() + " " + DdlHisC1HK.SelectedItem.Text.Trim() + " " + DdlHisC1CodCont.Text.Trim(); }
                }
                else
                {
                    query = "EXEC SP_PANTALLA__Historico_Contadores 5,@Ct,@CE,'','','CurExportarConsulHistorico',0, 0,0,0,@FI,@FF,'01-01-1'";
                    DataRow[] Result = Idioma.Select("Objeto= 'HCNomArcElem'");
                    foreach (DataRow row in Result)
                    { VbNomArchivo = row["Texto"].ToString().Trim() + " " + DdlHisC1SN.SelectedItem.Text.Trim() + " " + DdlHisC1CodCont.Text.Trim(); }
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@Ct", DdlHisC1CodCont.Text.Trim());
                        cmd.Parameters.AddWithValue("@HK", DdlHisC1HK.Text.Trim());
                        cmd.Parameters.AddWithValue("@CE", DdlHisC1SN.Text.Trim());
                        cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIPpl.Text.Trim()));
                        cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFPpl.Text.Trim()));
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                ds.Tables[0].TableName = "Historic";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
                                    Response.Charset = "";
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Histórico", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }           
        }
        //******************************** Panel historico dos *****************************************
        protected void RdbHisC2AplicAK_CheckedChanged(object sender, EventArgs e)
        {
            BindBDdlAK("2");
            LblHisC2HK.Visible = true;
            DdlHisC2HK.Visible = true;
            LblHisC2PN.Visible = false;
            DdlHisC2PN.Visible = false;
            LblHisC2SN.Visible = false;
            DdlHisC2SN.Visible = false;
            string LtxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 16,'','','','BLANK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlHisC2CodCont.DataSource = Cnx.DSET(LtxtSql);
            DdlHisC2CodCont.DataMember = "Datos";
            DdlHisC2CodCont.DataTextField = "Descripcion";
            DdlHisC2CodCont.DataValueField = "CodContador";
            DdlHisC2CodCont.DataBind();
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
            TxtHisVlrIndiv2.Text = "0";
            TxtHisVlrAcumv2.Text = "0";
            LblTitHisC2Aplicab.Text = "";
        }
        protected void RdbHisC2AplicSN_CheckedChanged(object sender, EventArgs e)
        {
            BindBDdlPNHC("2");
            BindBDdlSNHC(DdlHisC2PN.Text.Trim(), "2");
            LblHisC2HK.Visible = false;
            DdlHisC2HK.Visible = false;
            LblHisC2PN.Visible = true;
            DdlHisC2PN.Visible = true;
            LblHisC2SN.Visible = true;
            DdlHisC2SN.Visible = true;
            string LtxtSql = "EXEC SP_PANTALLA_Informe_Ingenieria 16,'','','','BLANK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlHisC2CodCont.DataSource = Cnx.DSET(LtxtSql);
            DdlHisC2CodCont.DataMember = "Datos";
            DdlHisC2CodCont.DataTextField = "Descripcion";
            DdlHisC2CodCont.DataValueField = "CodContador";
            DdlHisC2CodCont.DataBind();
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
            TxtHisVlrIndiv2.Text = "0";
            TxtHisVlrAcumv2.Text = "0";
            LblTitHisC2Aplicab.Text = "";
        }
        protected void BIndDHistoricoP2()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "";
                if (RdbHisC2AplicAK.Checked == true)
                { VbTxtSql = "EXEC SP_PANTALLA__Historico_Contadores 1,@Ct,'','','','',@HK, 0,0,0,@FI,@FF,'01-01-1'"; }
                else { VbTxtSql = "EXEC SP_PANTALLA__Historico_Contadores 5,@Ct,@CE,'','','',0, 0,0,0,@FI,@FF,'01-01-1'"; }
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Ct", DdlHisC2CodCont.Text.Trim());
                    SC.Parameters.AddWithValue("@HK", DdlHisC2HK.Text.Trim());
                    SC.Parameters.AddWithValue("@CE", DdlHisC2SN.Text.Trim());
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIPpl.Text.Trim()));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFPpl.Text.Trim()));
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisC2.DataSource = DtB;
                            GrdHisC2.DataBind();
                        }
                        else
                        {
                            DtB.Rows.Add(DtB.NewRow());
                            GrdHisC2.DataSource = DtB;
                            GrdHisC2.DataBind();
                            GrdHisC2.Rows[0].Cells.Clear();
                            GrdHisC2.Rows[0].Cells.Add(new TableCell());
                            GrdHisC2.Rows[0].Cells[0].ColumnSpan = DtB.Columns.Count;
                            DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC00'");
                            foreach (DataRow row in Result)
                            { GrdHisC2.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                            GrdHisC2.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
        }
        protected void ValidarConsultaP2()
        {
            ViewState["Validar"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechIPpl.Text.Equals("") || TxtFechFPpl.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                ViewState["Validar"] = "N"; return;
            }
            if (TxtFechIPpl.Text.Length > 10 || TxtFechFPpl.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }// 
                ViewState["Validar"] = "N"; return;
            }
            if (RdbHisC2AplicAK.Checked == true && DdlHisC2HK.Text.Equals("0"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC02'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe ingresar una aeronave.');", true);
                ViewState["Validar"] = "N"; return;
            }
            if (RdbHisC2AplicSN.Checked == true && DdlHisC2SN.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC03'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una serie.');", true);
                ViewState["Validar"] = "N"; return;
            }
            if (DdlHisC2CodCont.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC04'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un contador.');", true);
                ViewState["Validar"] = "N"; return;
            }
        }       
        protected void IbtHisC2Find_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ValidarConsultaP2();
            if (ViewState["Validar"].ToString().Equals("N")) { return; }

            BIndDHistoricoP2();
            foreach (GridViewRow Row in GrdHisC2.Rows)
            {
                TxtHisVlrIndiv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[2].ToString();
                TxtHisVlrAcumv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[3].ToString();
                LblTitHisC2Aplicab.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[4].ToString();
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void DdlHisC2HK_TextChanged(object sender, EventArgs e)
        {
            BindBDdlCodCont(DdlHisC2HK.Text, "", "2");
            DdlHisC2CodCont.DataBind();
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
        }
        protected void DdlHisC2PN_TextChanged(object sender, EventArgs e)
        {
            BindBDdlSNHC(DdlHisC2PN.Text.Trim(), "2");
            BindBDdlCodCont("0", DdlHisC2PN.Text.Trim(), "2");
            DdlHisC2CodCont.DataBind();
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
        }
        protected void DdlHisC2SN_TextChanged(object sender, EventArgs e)
        {
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
        }
        protected void DdlHisC2CodCont_TextChanged(object sender, EventArgs e)
        {
            GrdHisC2.DataSource = null;
            GrdHisC2.DataBind();
        }
        protected void GrdHisC2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {

                if ((GrdHisC2.FooterRow.FindControl("TxtHC2FechaPP") as TextBox).Text.Trim().Equals("") || (GrdHisC2.FooterRow.FindControl("TxtHC2FechaPP") as TextBox).Text.Trim().Length > 10)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensjHisC01'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Fecha inválida
                    return;
                }
                CultureInfo Culture = new CultureInfo("en-US");
                string VblTxtCant = (GrdHisC2.FooterRow.FindControl("TxtVlrIndiv2PP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdHisC2.FooterRow.FindControl("TxtVlrIndiv2PP") as TextBox).Text.Trim();
                double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 18,@Cnt,@Us,@CEl,'',@CHK,@Vr,0,0,@FI,'01-01-1900','01-01-1900'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                string VbHK = "0";
                                if (RdbHisC2AplicAK.Checked == true) { VbHK = DdlHisC2HK.Text; }
                                SC.Parameters.AddWithValue("@Cnt", DdlHisC2CodCont.Text.Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@CHK", VbHK);
                                SC.Parameters.AddWithValue("@CEl", DdlHisC2SN.Text.Trim());
                                SC.Parameters.AddWithValue("@FI", Convert.ToDateTime((GrdHisC2.FooterRow.FindControl("TxtHC2FechaPP") as TextBox).Text.Trim()));
                                SC.Parameters.AddWithValue("@Vr", VblCant);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BIndDHistoricoP2();
                                foreach (GridViewRow Row in GrdHisC2.Rows)
                                {
                                    TxtHisVlrIndiv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[2].ToString();
                                    TxtHisVlrAcumv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[3].ToString();
                                    LblTitHisC2Aplicab.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[4].ToString();
                                }
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Historico", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdHisC2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery = "";
            int IdHist = Convert.ToInt32(GrdHisC2.DataKeys[e.RowIndex].Values[0].ToString());
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    if (RdbHisC2AplicAK.Checked == true) { VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 17,@Cnt,@Us,@MAT,'HK',@CHis,@CHK,0,0,'01-1-2009','01-01-1900','01-01-1900'"; }
                    else { VBQuery = "EXEC SP_PANTALLA_Informe_Ingenieria 17,@Cnt,@Us,@SN,'ELEM',@CHis,0,@CEl,0,'01-1-2009','01-01-1900','01-01-1900'"; }
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Cnt", DdlHisC2CodCont.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            if (RdbHisC2AplicAK.Checked == true) { SC.Parameters.AddWithValue("@MAT", DdlHisC2HK.SelectedItem.Text.Trim()); }
                            if (RdbHisC2AplicSN.Checked == true) { SC.Parameters.AddWithValue("@SN", DdlHisC2SN.SelectedItem.Text.Trim()); }
                            SC.Parameters.AddWithValue("@CHis", IdHist);
                            SC.Parameters.AddWithValue("@CHK", DdlHisC2HK.Text);
                            SC.Parameters.AddWithValue("@CEl", DdlHisC2SN.Text.Trim());
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BIndDHistoricoP2();
                            foreach (GridViewRow Row in GrdHisC2.Rows)
                            {
                                TxtHisVlrIndiv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[2].ToString();
                                TxtHisVlrAcumv2.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[3].ToString();
                                LblTitHisC2Aplicab.Text = GrdHisC2.DataKeys[Row.RowIndex].Values[4].ToString();
                            }
                        }
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UplPpl, UplPpl.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }

                    }
                }
            }
        }
        protected void GrdHisC2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            ImageButton IbtDelete = (e.Row.FindControl("IbtDelete") as ImageButton);
            if (IbtDelete != null)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDelete'");
                foreach (DataRow row in Result)
                { IbtDelete.ToolTip = row["Texto"].ToString().Trim(); }
                Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                foreach (DataRow row in Result)
                { IbtDelete.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
            }
        }
        //******************************** EXportar panel 2 *****************************************
        protected void IbtHisC2Excel_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                ValidarConsultaP2();
                if (ViewState["Validar"].ToString().Equals("N")) { return; }
                string query = "", VbNomArchivo = "";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportarConsulHistorico", Session["77IDM"].ToString().Trim());
                if (RdbHisC2AplicAK.Checked == true)
                {
                    query = "EXEC SP_PANTALLA__Historico_Contadores 1,@Ct,'','','','CurExportarConsulHistorico',@HK, 0,0,0,@FI,@FF,'01-01-1'";
                    DataRow[] Result = Idioma.Select("Objeto= 'HCNomArcHK'");
                    foreach (DataRow row in Result)
                    { VbNomArchivo = row["Texto"].ToString().Trim() + " " + DdlHisC2HK.SelectedItem.Text.Trim() + " " + DdlHisC2CodCont.Text.Trim(); }
                }
                else
                {
                    query = "EXEC SP_PANTALLA__Historico_Contadores 5,@Ct,@CE,'','','CurExportarConsulHistorico',0, 0,0,0,@FI,@FF,'01-01-1'";
                    DataRow[] Result = Idioma.Select("Objeto= 'HCNomArcElem'");
                    foreach (DataRow row in Result)
                    { VbNomArchivo = row["Texto"].ToString().Trim() + " " + DdlHisC2SN.SelectedItem.Text.Trim() + " " + DdlHisC2CodCont.Text.Trim(); }
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@Ct", DdlHisC2CodCont.Text.Trim());
                        cmd.Parameters.AddWithValue("@HK", DdlHisC2HK.Text.Trim());
                        cmd.Parameters.AddWithValue("@CE", DdlHisC2SN.Text.Trim());
                        cmd.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechIPpl.Text.Trim()));
                        cmd.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechFPpl.Text.Trim()));
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                ds.Tables[0].TableName = "Historic";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
                                    Response.Charset = "";
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Histórico", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
    }
}