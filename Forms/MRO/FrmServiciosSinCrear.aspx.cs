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
    public partial class FrmServiciosSinCrear : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
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
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                ModSeguridad();
                BindSrvcSinCrear("UPDATE");
                MultVw.ActiveViewIndex = 0;
            }
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
                    BtnAbrirSrvcs.Text = bO.Equals("BtnAbrirSrvcs") ? bT : BtnAbrirSrvcs.Text;
                    BtnAbrirSrvcs.ToolTip = bO.Equals("BtnAbrirSrvcsTT") ? bT : BtnAbrirSrvcs.ToolTip;
                    GrdDet.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDet.EmptyDataText;
                    GrdDet.Columns[0].HeaderText = bO.Equals("GrdBusq") ? bT : GrdDet.Columns[0].HeaderText;
                    GrdDet.Columns[1].HeaderText = bO.Equals("GrdRazonS") ? bT : GrdDet.Columns[1].HeaderText;
                    GrdDet.Columns[2].HeaderText = bO.Equals("GrdPpt") ? bT : GrdDet.Columns[2].HeaderText;
                    GrdDet.Columns[3].HeaderText = bO.Equals("GrdContr") ? bT : GrdDet.Columns[3].HeaderText;
                    GrdDet.Columns[4].HeaderText = bO.Equals("GrdModl") ? bT : GrdDet.Columns[4].HeaderText;
                    GrdDet.Columns[5].HeaderText = bO.Equals("GrdHk") ? bT : GrdDet.Columns[5].HeaderText;
                    GrdDet.Columns[8].HeaderText = bO.Equals("GrdSrvc") ? bT : GrdDet.Columns[8].HeaderText;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    IbtBusqueda.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtBusqueda.ToolTip;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[0].HeaderText = bO.Equals("GrdAsignar") ? bT : GrdBusq.Columns[0].HeaderText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("GrdSrvc") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("GrdCodModl") ? bT : GrdBusq.Columns[2].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindSrvcSinCrear(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC DetalleServiciosSinCrear @ICC";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        // SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DS = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DS);

                                DS.Tables[0].TableName = "SvcSinC";
                                DS.Tables[1].TableName = "SvcElem";
                                DS.Tables[2].TableName = "SvcHK";
                                ViewState["DS"] = DS;
                            }
                        }
                    }
                }
            }
            DS = (DataSet)ViewState["DS"];
            GrdDet.DataSource = DS.Tables[0]; GrdDet.DataBind();
        }       
        protected void BtnAbrirSrvcs_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/Ingenieria/FrmServicioManto.aspx"); }
        protected void GrdDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;

            if (e.CommandName.Equals("Busq"))
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                ViewState["PPT"] = ((Label)row.FindControl("LblPpt")).Text.ToString().Trim();
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["CodTipoPropuesta"] = GrdDet.DataKeys[gvr.RowIndex].Values["CodTipoPropuesta"].ToString();
                ViewState["CodModelo"] = GrdDet.DataKeys[gvr.RowIndex].Values["CodModeloPr"].ToString();
                ViewState["IdDetPropSrv"] = GrdDet.DataKeys[gvr.RowIndex].Values["IdDetPropSrv"].ToString();

                if (ViewState["CodTipoPropuesta"].ToString().Trim().Equals("00002")) //aplicabilidad a PPT HK
                {
                    Result = Idioma.Select("Objeto='Mens01SSC'");
                    foreach (DataRow RowIdioma in Result)
                    { LblTitAsiganar.Text = RowIdioma["Texto"].ToString().Trim() + " [" + ((Label)row.FindControl("LblMode")).Text.ToString().Trim() + "]"; }
                }
                else
                {
                    Result = Idioma.Select("Objeto='Mens02SSC'");
                    foreach (DataRow RowIdioma in Result)
                    { LblTitAsiganar.Text = RowIdioma["Texto"].ToString().Trim() + " [" + ((Label)row.FindControl("LblPN")).Text.ToString().Trim() + "]"; }
                }
                MultVw.ActiveViewIndex = 1;
            }
        }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtBusq = (e.Row.FindControl("IbtBusq") as ImageButton);
                if (IbtBusq != null)
                {
                    Result = Idioma.Select("Objeto='IbtBusqTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtBusq.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //****************************************** ASignar Servicio *************************************
        protected void BIndDBusq()
        {
            DS = (DataSet)ViewState["DS"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            if (ViewState["CodTipoPropuesta"].ToString().Trim().Equals("00002")) //aplicabilidad a PPT HK
            {
                DT = DS.Tables[2].Clone();
                Result = DS.Tables[2].Select("Descripcion LIKE '%" + TxtBusqueda.Text.Trim() + "%' AND CodModeloSM LIKE '%" + ViewState["CodModelo"] + "%'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
            }
            else//aplicabilidad a PPT Elemento
            {
                DT = DS.Tables[1].Clone();
                Result = DS.Tables[1].Select("Descripcion LIKE '%" + TxtBusqueda.Text.Trim() + "%' AND CodModeloSM LIKE '%" + ViewState["CodModelo"] + "%'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
            }
            GrdBusq.DataSource = DT; GrdBusq.DataBind();
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BIndDBusq(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Asignar"))
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                GridViewRow RowG = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasMRO 17,@Dsc, @Us,@Ctd,'','','','','','',@ISv,@PP,@IDPS,0,0,@CC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Dsc", ((TextBox)RowG.FindControl("TxtNomSvc")).Text.ToString().Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());                               
                                SC.Parameters.AddWithValue("@Ctd", GrdBusq.DataKeys[gvr.RowIndex].Values["CodContador"].ToString());                               
                                SC.Parameters.AddWithValue("@ISv", GrdBusq.DataKeys[gvr.RowIndex].Values["IdSrvManto"].ToString());
                                SC.Parameters.AddWithValue("@PP", ViewState["PPT"]);
                                SC.Parameters.AddWithValue("@IDPS", ViewState["IdDetPropSrv"]);
                                SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                                using (SqlDataAdapter SDA = new SqlDataAdapter())
                                {
                                    using (DataSet DS = new DataSet())
                                    {
                                        Transac.Commit();
                                        SDA.SelectCommand = SC;
                                        SDA.Fill(DS);

                                        DS.Tables[0].TableName = "SvcSinC";
                                        DS.Tables[1].TableName = "SvcElem";
                                        DS.Tables[2].TableName = "SvcHK";
                                        ViewState["DS"] = DS;
                                    }
                                    DS = (DataSet)ViewState["DS"];
                                    GrdDet.DataSource = DS.Tables[0]; GrdDet.DataBind();
                                    MultVw.ActiveViewIndex = 0;
                                }
                            }
                            catch (Exception) { Transac.Rollback(); }
                        }
                    }
                }
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAsignar = (e.Row.FindControl("IbtAsignar") as ImageButton);
                if (IbtAsignar != null)
                {
                    Result = Idioma.Select("Objeto='IbtAsignarTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsignar.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    Result = Idioma.Select("Objeto= 'IbtAsignarOnCl'");
                    foreach (DataRow row in Result) { IbtAsignar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
    }
}