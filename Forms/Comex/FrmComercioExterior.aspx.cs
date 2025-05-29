using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Comex
{
    public partial class FrmComercioExterior : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPpl = new DataSet();
        DataSet DSTDdl = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                    Session["MonLcl"] = Cnx.GetMonedLcl();// Moneda Local
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                RdbBusqOrden.Checked = true;
                ViewState["Accion"] = "";
                MultVw.ActiveViewIndex = 0;
                ModSeguridad();
                ViewState["CodTerceroAnt"] = "";
                BindBDdl("UPD");
                //AddCamposDataTable("INS");
                //EnablGridDet("Visible", false); /**/

                //BotonesCompr_Intercb("C");

            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
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
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; } // grd.ShowFooter = false;
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // 
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//          

            IdiomaControles();
        }
        protected void IdiomaControles()
        {
            Idioma.Columns.Add("Objeto", typeof(string));
            Idioma.Columns.Add("Texto", typeof(string));
            using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
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
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    //*************************************************Campos *************************************************
                    LblNroEmbq.Text = bO.Equals("LblNroEmbq") ? bT : LblNroEmbq.Text;
                    LblProv.Text = bO.Equals("LblProvee") ? bT : LblProv.Text;

                    // *************************************************Grid detalle *************************************************


                    // *************************************************opcion de busqueda *************************************************
                    RdbBqCompra.Text = bO.Equals("LblNumCotiza") ? "&nbsp" + bT : RdbBqCompra.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }

                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    //GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    //GrdBusq.Columns[1].HeaderText = bO.Equals("BtnOpenSolPed") ? bT : GrdBusq.Columns[1].HeaderText;
                    //GrdBusq.Columns[2].HeaderText = bO.Equals("TipoMstr") ? bT : GrdBusq.Columns[2].HeaderText;
                    //GrdBusq.Columns[3].HeaderText = bO.Equals("LblFechCot") ? bT : GrdBusq.Columns[3].HeaderText;
                    //GrdBusq.Columns[4].HeaderText = bO.Equals("LblProvee") ? bT : GrdBusq.Columns[4].HeaderText;
                    //GrdBusq.Columns[7].HeaderText = bO.Equals("Descripcion") ? bT : GrdBusq.Columns[7].HeaderText;                    

                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result) { BtnEliminar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {

                    string VbTxtSql = "EXEC PNTLL_Comex 2,'','','','','','DDL',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Tercero";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            if (DSTDdl.Tables["Tercero"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[0].Select("(Activo=1 AND  Clasificacion IN ('P','A')) OR CodTercero= '" + ViewState["CodTerceroAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlProv.DataSource = DT;
                DdlProv.DataTextField = "RazonSocial";
                DdlProv.DataValueField = "CodTercero";
                DdlProv.DataBind();
                DdlProv.SelectedValue = ViewState["CodTerceroAnt"].ToString().Trim();
            }
        }
        protected void Traerdatos(string CodSO, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC PNTLL_Comex 2,@CodSO,'','','','','PPAL',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@CodSO", CodSO.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "EncSO";
                                    DSTPpl.Tables[1].TableName = "DetSO";

                                    ViewState["DSTPpl"] = DSTPpl;
                                }
                            }
                        }
                    }
                }
                //TxtBusqPn.Text = "";
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                //DSTDdl = (DataSet)ViewState["DSTDdl"];
                if (DSTPpl.Tables["EncSO"].Rows.Count > 0)
                {
                    string VbFecSt, S_Tipo;
                    DateTime? VbFecDT;
                    TxtNumDoc.Text = DSTPpl.Tables[0].Rows[0]["CodShippingOrder"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["Fecha"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["Fecha"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    S_Tipo = DSTPpl.Tables[0].Rows[0]["Tipo"].ToString().Trim();
                    DdlProv.Text = S_Tipo.Equals("Exportacion")? DSTPpl.Tables[0].Rows[0]["Embarq_A"].ToString().Trim(): DSTPpl.Tables[0].Rows[0]["Embarq_De"].ToString().Trim();
                    RdbImportar.Checked = S_Tipo.Equals("Exportacion") ? false : true;
                    RdbExporar.Checked = S_Tipo.Equals("Exportacion") ? true : false;
                    TxtGuia.Text = DSTPpl.Tables[0].Rows[0]["NroGuia"].ToString().Trim();
                    TxtPeso.Text = DSTPpl.Tables[0].Rows[0]["Peso"].ToString().Trim();
                    TxtNrPaq.Text = DSTPpl.Tables[0].Rows[0]["NroPaquete"].ToString().Trim();
                    TxtObsrv.Text = DSTPpl.Tables[0].Rows[0]["Observaciones"].ToString().Trim();
                    //BindBDdl("SEL");
                }
                /* if (DSTPpl.Tables["DetCotiza"].Rows.Count > 0)
                 {
                     TblDetalle = (DataTable)ViewState["TblDetalle"];
                     DataRow[] DR = DSTPpl.Tables[1].Select("Vista <>''");
                     if (IsIENumerableLleno(DR))
                     { TblDetalle = DR.CopyToDataTable(); TblDetalle.AcceptChanges(); ViewState["TblDetalle"] = TblDetalle; }
                 }
                 else { TblDetalle.Clear(); TblDetalle.AcceptChanges(); AddCamposDataTable("INS"); ; }*/
                //if (TblDetalle.Rows.Count > 0) { }
                //BindDDetTmp();
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; TxtBusqueda.Focus(); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEliminar_Click(object sender, EventArgs e)
        {

        }
        //****************************** Busqueda **************************************
        protected void BIndDBusqSP()
        {
            try
            {
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbSO = "", VbGuia = "", VbDoc = "";

                    if (RdbBusqOrden.Checked == true)
                    { VbSO = TxtBusqueda.Text.Trim(); }
                    if (RdbBusqGuia.Checked == true)
                    { VbGuia = TxtBusqueda.Text.Trim(); }
                    if (RdbBusqDoc.Checked == true)
                    { VbDoc = TxtBusqueda.Text.Trim(); }

                    string VbTxtSql = "EXEC PNTLL_Comex 1,@VbSO,@VbGuia,@VbDoc,'','','',0,0,0,5,1,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@VbSO", VbSO.Trim());
                        SC.Parameters.AddWithValue("@VbGuia", VbGuia.Trim());
                        SC.Parameters.AddWithValue("@VbDoc", VbDoc.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DT);
                            if (DT.Rows.Count > 0) { GrdBusq.DataSource = DT; GrdBusq.DataBind(); }
                            else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                        }
                    }
                }
            }
            catch (Exception Ex) { string Borrar = Ex.Message; }
        }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BIndDBusqSP(); }
        protected void IbtCerrarBusq_Click1(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["CodShippingOrder"] = GrdBusq.DataKeys[gvr.RowIndex].Values["CodShippingOrder"].ToString();
                Traerdatos(ViewState["CodShippingOrder"].ToString().Trim(), "UPD");
                MultVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
                //EnablGridDet("Visible", true);
            }
        }

        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        //****************************** Concpetos **************************************


        protected void IbtCerrarCnptos_Click(object sender, ImageClickEventArgs e)
        {

        }


    }
}