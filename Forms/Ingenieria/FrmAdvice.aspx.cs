﻿using _77NeoWeb.prg;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmAdvice : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null) { Response.Redirect("~/FrmAcceso.aspx"); }/**/
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoHCT";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "860064038-4"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles  */
            }
            if (!IsPostBack)
            {
                ModSeguridad();
                BindPn();
                MultVieLV.ActiveViewIndex = 0;
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
            if (ClsP.GetCE1() == 0) { } //
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
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

                    TitForm.Text = bO.Equals("Caption") ? bT : TitForm.Text;
                    LblModel.Text = bO.Equals("LblModel") ? bT + ":" : LblModel.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    LblHK.Text = bO.Equals("LblHK") ? bT + ":" : LblHK.Text;
                    LblDesc.Text = bO.Equals("LblDesc") ? bT + ":" : LblDesc.Text;
                    LblFechaActualiza.Text = bO.Equals("LblFechaActualiza") ? bT + ":" : LblFechaActualiza.Text;
                    GrdAdvice.Columns[1].HeaderText = bO.Equals("GrdNvl") ? bT : GrdAdvice.Columns[1].HeaderText;
                    GrdAdvice.Columns[2].HeaderText = bO.Equals("GrdUbTec") ? bT : GrdAdvice.Columns[2].HeaderText;
                    GrdAdvice.Columns[3].HeaderText = bO.Equals("GrdDesElem") ? bT : GrdAdvice.Columns[3].HeaderText;
                    GrdAdvice.Columns[6].HeaderText = bO.Equals("GrdSvc") ? bT : GrdAdvice.Columns[6].HeaderText;
                    GrdAdvice.Columns[7].HeaderText = bO.Equals("GrdFrec") ? bT : GrdAdvice.Columns[7].HeaderText;
                    GrdAdvice.Columns[8].HeaderText = bO.Equals("GrdDia") ? bT : GrdAdvice.Columns[8].HeaderText;
                    GrdAdvice.Columns[9].HeaderText = bO.Equals("GrdCont") ? bT : GrdAdvice.Columns[9].HeaderText;
                    GrdAdvice.Columns[11].HeaderText = bO.Equals("GrdRem") ? bT : GrdAdvice.Columns[11].HeaderText;
                    GrdAdvice.Columns[12].HeaderText = bO.Equals("GrdRemD") ? bT : GrdAdvice.Columns[12].HeaderText;
                    GrdAdvice.Columns[10].HeaderText = bO.Equals("GrdAcum") ? bT : GrdAdvice.Columns[10].HeaderText;
                    GrdAdvice.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAdvice.EmptyDataText;
                    BtnImprimir.Text = bO.Equals("BtnImprimir") ? bT : BtnImprimir.Text;
                    LblTitImpresion.Text = bO.Equals("BtnImprimir") ? bT : LblTitImpresion.Text;
                    IbtCerrarImpresion.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarImpresion.ToolTip;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindPn()
        {
            string LtxtSql = "EXEC SP_PANTALLA_ADVICE 16,'','','','PN',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            DdlPN.DataSource = Cnx.DSET(LtxtSql);
            DdlPN.DataMember = "Datos";
            DdlPN.DataTextField = "PN";
            DdlPN.DataValueField = "Codigo";
            DdlPN.DataBind();
        }
        protected void DdlPN_TextChanged(object sender, EventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_ADVICE 16,'{0}','','','SN',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlPN.Text);
            DdlSN.DataSource = Cnx.DSET(LtxtSql);
            DdlSN.DataMember = "Datos";
            DdlSN.DataTextField = "SN";
            DdlSN.DataValueField = "Codigo";
            DdlSN.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_ADVICE 16,'{0}','','','MOD',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlPN.Text);
            DdlModel.DataSource = Cnx.DSET(LtxtSql);
            DdlModel.DataMember = "Datos";
            DdlModel.DataTextField = "Descripcion";
            DdlModel.DataValueField = "CodModelo";
            DdlModel.DataBind();
        }
        protected void BindAdvice()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "EXEC SP_Advice_WEB @Usu, '','GRUPOS',@P, @S,@M, @E";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                    SC.Parameters.AddWithValue("@P", DdlPN.Text.Trim());
                    SC.Parameters.AddWithValue("@S", DdlSN.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@M", DdlModel.Text.Trim());
                    SC.Parameters.AddWithValue("@E", DdlSN.Text.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdAdvice.DataSource = DtB;
                            GrdAdvice.DataBind();
                        }
                        else
                        {
                            GrdAdvice.DataSource = null;
                            GrdAdvice.DataBind();
                        }
                    }
                }
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlPN.Text.Trim().Equals("") || DdlSN.Text.Trim().Equals("") || DdlModel.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Advic'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Todos los campos son requeridos.
                return;
            }
            if (!DdlSN.Text.ToString().Trim().Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
                {
                    SCnx.Open();
                    string LtxtSql = "EXEC SP_PANTALLA_ADVICE 17,@Ce,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(LtxtSql, SCnx);
                    SC.Parameters.AddWithValue("@Ce", DdlSN.Text.ToString());
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        TxtHK.Text = SDR["Matricula"].ToString();
                        TxtDesc.Text = HttpUtility.HtmlDecode(SDR["Descripcion"].ToString().Trim());
                        TxtTT.Text = SDR["TSN"].ToString();
                        TxtTSO.Text = SDR["TSO"].ToString();
                        TxtCSN.Text = SDR["CSN"].ToString();
                        TxtCSO.Text = SDR["CSO"].ToString();
                        TxtSSN.Text = SDR["SSN"].ToString();
                        TxtFechaActualiza.Text = SDR["Fecha"].ToString();
                        BindAdvice();
                    }
                }
            }
            BtnImprimir.Enabled = true;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            MultVieLV.ActiveViewIndex = 1;

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SCnx1 = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[24];

                parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                parameters[3] = new ReportParameter("LblMatr", LblHK.Text);
                parameters[4] = new ReportParameter("HK", TxtHK.Text.Trim());
                parameters[5] = new ReportParameter("LblDesc", LblDesc.Text);
                parameters[6] = new ReportParameter("TxtDesc", TxtDesc.Text.Trim());
                parameters[7] = new ReportParameter("DdlPN", DdlPN.Text.Trim());
                parameters[8] = new ReportParameter("DdlSN", DdlSN.SelectedItem.Text.Trim());
                parameters[9] = new ReportParameter("TT", TxtTT.Text.Trim());
                parameters[10] = new ReportParameter("TSO", TxtTSO.Text.Trim());
                parameters[11] = new ReportParameter("CSN", TxtCSN.Text.Trim());
                parameters[12] = new ReportParameter("CSO", TxtCSO.Text.Trim());
                parameters[13] = new ReportParameter("SSN", TxtSSN.Text.Trim());
                parameters[14] = new ReportParameter("LblFechA", LblFechaActualiza.Text.Trim());
                parameters[15] = new ReportParameter("TxtFechA", TxtFechaActualiza.Text.Trim());
                parameters[16] = new ReportParameter("Descr", GrdAdvice.Columns[1].HeaderText+"/"+GrdAdvice.Columns[3].HeaderText);
                parameters[17] = new ReportParameter("Servicio", GrdAdvice.Columns[6].HeaderText);
                parameters[18] = new ReportParameter("Frec", GrdAdvice.Columns[7].HeaderText);
                parameters[19] = new ReportParameter("FrecD", GrdAdvice.Columns[8].HeaderText);
                parameters[20] = new ReportParameter("Cont", GrdAdvice.Columns[9].HeaderText);
                parameters[21] = new ReportParameter("Acum", GrdAdvice.Columns[10].HeaderText);
                parameters[22] = new ReportParameter("Reman", GrdAdvice.Columns[11].HeaderText);
                parameters[23] = new ReportParameter("RemanD", GrdAdvice.Columns[12].HeaderText);

                string StSql = "EXEC SP_Advice_WEB @Usu, '','GRUPOS',@P, @S,@M, @E";
                using (SqlCommand SC = new SqlCommand(StSql, SCnx1))
                {
                    SC.Parameters.AddWithValue("@Usu", Session["C77U"]);
                    SC.Parameters.AddWithValue("@P", DdlPN.Text.Trim());
                    SC.Parameters.AddWithValue("@S", DdlSN.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@M", DdlModel.Text.Trim());
                    SC.Parameters.AddWithValue("@E", DdlSN.Text.Trim());
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(ds);
                        RvwReporte.LocalReport.EnableExternalImages = true;
                        RvwReporte.LocalReport.ReportPath = "Report/Ing/Advice.rdlc";
                        RvwReporte.LocalReport.DataSources.Clear();
                        RvwReporte.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                        RvwReporte.LocalReport.SetParameters(parameters);
                        RvwReporte.LocalReport.Refresh();
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void IbtCerrarImpresion_Click(object sender, ImageClickEventArgs e)
        {
            MultVieLV.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
    }
}