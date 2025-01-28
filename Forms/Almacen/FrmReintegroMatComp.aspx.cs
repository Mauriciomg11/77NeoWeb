using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmReintegroMatComp : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSDetRva = new DataSet();
        DataSet DSUbica = new DataSet();
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
                ModSeguridad();
                TraerDatos("UPD");
                MultVw.ActiveViewIndex = 0;
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
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; } // grd.ShowFooter = false;
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // Imprimir INcoming
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//  Puede editar el porcentaje de la recuperacion
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//  Solicitud de pedido     

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
                    LblAeronave.Text = bO.Equals("LblAeronaveMstr") ? bT : LblAeronave.Text;
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    RdbNumRsva.Text = bO.Equals("RdbNumRsva") ? "&nbsp" + bT : RdbNumRsva.Text;
                    RdbRvaOT.Text = bO.Equals("LblOTMstr") ? "&nbsp" + bT : RdbRvaOT.Text;
                    RdbRvaRte.Text = bO.Equals("RdbRvaRte") ? "&nbsp" + bT : RdbRvaRte.Text;
                    LblNumReserva.ToolTip = bO.Equals("RdbNumRsva") ? bT : LblNumReserva.ToolTip;
                    LblNumOrdeTrabajo.ToolTip = bO.Equals("LblOTMstr") ? bT : LblNumOrdeTrabajo.ToolTip;
                    LblNumReporte.ToolTip = bO.Equals("RdbRvaRte") ? bT : LblNumReporte.ToolTip;

                    // *********************************************** Detalle Reintegro ***********************************************
                    GrdDtllRitgr.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtllRitgr.EmptyDataText;
                    GrdDtllRitgr.Columns[1].HeaderText = bO.Equals("PosMstr") ? bT : GrdDtllRitgr.Columns[1].HeaderText;
                    GrdDtllRitgr.Columns[2].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDtllRitgr.Columns[2].HeaderText;
                    GrdDtllRitgr.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdDtllRitgr.Columns[3].HeaderText;
                    GrdDtllRitgr.Columns[4].HeaderText = bO.Equals("TipoMstr") ? bT : GrdDtllRitgr.Columns[4].HeaderText;
                    GrdDtllRitgr.Columns[5].HeaderText = bO.Equals("GrdIdent") ? bT : GrdDtllRitgr.Columns[5].HeaderText;
                    GrdDtllRitgr.Columns[7].HeaderText = bO.Equals("GrdCantSol") ? bT : GrdDtllRitgr.Columns[7].HeaderText;
                    GrdDtllRitgr.Columns[8].HeaderText = bO.Equals("GrdCantEntr") ? bT : GrdDtllRitgr.Columns[8].HeaderText;
                    GrdDtllRitgr.Columns[9].HeaderText = bO.Equals("GrdCantRtgr") ? bT : GrdDtllRitgr.Columns[9].HeaderText;
                    GrdDtllRitgr.Columns[10].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtllRitgr.Columns[10].HeaderText;

                    // *********************************************** Condiciones Almacenamiento / Manipulacion ***********************************************
                    LblTitCondManiplc.Text = bO.Equals("LblTitCondManiplc") ? bT : LblTitCondManiplc.Text;
                    BtnCloseMdl.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseMdl.Text;
                    GrdMdlCondManplc.Columns[0].HeaderText = bO.Equals("Descripcion") ? bT : GrdMdlCondManplc.Columns[0].HeaderText;

                    // *********************************************** Visualizar y entrega ***********************************************
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;
                    LblTitAsigFis.Text = bO.Equals("LblTitAsigFis") ? bT : LblTitAsigFis.Text;
                    LblAsigCantSol.Text = bO.Equals("GrdCantSol") ? " | " + bT + ": " : LblAsigCantSol.Text;
                    LblAsigCantEntrg.Text = bO.Equals("GrdCantEntr") ? " | " + bT + ": " : LblAsigCantEntrg.Text;
                    BtnAsignr.Text = bO.Equals("GrdAsig") ? bT : BtnAsignr.Text;
                    GrdUbicaFisc.EmptyDataText = bO.Equals("GrdEmpty") ? bT : GrdUbicaFisc.EmptyDataText;
                    GrdUbicaFisc.Columns[0].HeaderText = bO.Equals("GrdEstdPn") ? bT : GrdUbicaFisc.Columns[0].HeaderText;
                    GrdUbicaFisc.Columns[3].HeaderText = bO.Equals("LoteMst") ? bT : GrdUbicaFisc.Columns[3].HeaderText;
                    GrdUbicaFisc.Columns[4].HeaderText = bO.Equals("GrdModPN") ? bT : GrdUbicaFisc.Columns[4].HeaderText;
                    GrdUbicaFisc.Columns[5].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdUbicaFisc.Columns[5].HeaderText;
                    GrdUbicaFisc.Columns[6].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdUbicaFisc.Columns[6].HeaderText;
                    GrdUbicaFisc.Columns[7].HeaderText = bO.Equals("GrdFil") ? bT : GrdUbicaFisc.Columns[7].HeaderText;
                    GrdUbicaFisc.Columns[8].HeaderText = bO.Equals("GrdColumn") ? bT : GrdUbicaFisc.Columns[8].HeaderText;
                    GrdUbicaFisc.Columns[9].HeaderText = bO.Equals("GrdCantDesp") ? bT : GrdUbicaFisc.Columns[9].HeaderText;
                    GrdUbicaFisc.Columns[10].HeaderText = bO.Equals("GrdCantRtgr") ? bT : GrdUbicaFisc.Columns[10].HeaderText;
                    GrdUbicaFisc.Columns[11].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdUbicaFisc.Columns[11].HeaderText;
                    GrdUbicaFisc.Columns[12].HeaderText = bO.Equals("GrdNroDoc") ? bT : GrdUbicaFisc.Columns[12].HeaderText;
                    // *********************************************** Visualizar y entrega ***********************************************
                    LblTitVisualizaGuarda.Text = bO.Equals("LblTitVisualizaGuarda") ? bT : LblTitVisualizaGuarda.Text;
                    BtnVisualizar.Text = bO.Equals("BtnVisualizar") ? bT : BtnVisualizar.Text;
                    BtnVisualizar.ToolTip = bO.Equals("BtnVisualizarTT") ? bT : BtnVisualizar.ToolTip;
                    IbtCloseGuardar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseGuardar.ToolTip;
                    BtnGuardar.Text = bO.Equals("IbtGuardarCargaMax") ? bT : BtnGuardar.Text;
                    BtnGuardar.ToolTip = bO.Equals("BtnGuardarTT") ? bT : BtnGuardar.ToolTip;
                    LblNumRvaGuardar.Text = bO.Equals("RdbNumRsva") ? bT + ":" : LblNumRvaGuardar.Text;
                    GrdVisualizar.EmptyDataText = bO.Equals("GrdEmpty") ? bT : GrdVisualizar.EmptyDataText;
                    GrdVisualizar.Columns[0].HeaderText = bO.Equals("PosMstr") ? bT : GrdVisualizar.Columns[0].HeaderText;
                    GrdVisualizar.Columns[1].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdVisualizar.Columns[1].HeaderText;
                    GrdVisualizar.Columns[4].HeaderText = bO.Equals("LoteMst") ? bT : GrdVisualizar.Columns[4].HeaderText;
                    GrdVisualizar.Columns[5].HeaderText = bO.Equals("GrdModPN") ? bT : GrdVisualizar.Columns[5].HeaderText;
                    GrdVisualizar.Columns[6].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdVisualizar.Columns[6].HeaderText;
                    GrdVisualizar.Columns[7].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdVisualizar.Columns[7].HeaderText;
                    GrdVisualizar.Columns[8].HeaderText = bO.Equals("GrdFil") ? bT : GrdVisualizar.Columns[8].HeaderText;
                    GrdVisualizar.Columns[9].HeaderText = bO.Equals("GrdColumn") ? bT : GrdVisualizar.Columns[9].HeaderText;
                    GrdVisualizar.Columns[10].HeaderText = bO.Equals("GrdCantDesp") ? bT : GrdVisualizar.Columns[10].HeaderText;
                    GrdVisualizar.Columns[11].HeaderText = bO.Equals("GrdCantRtgr") ? bT : GrdVisualizar.Columns[11].HeaderText;
                    GrdVisualizar.Columns[12].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdVisualizar.Columns[12].HeaderText;
                    GrdVisualizar.Columns[13].HeaderText = bO.Equals("GrdNroDoc") ? bT : GrdVisualizar.Columns[13].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnCl1'");
                foreach (DataRow row in Result)
                { BtnGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');");/**/ }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void TraerDatos(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Sal_Consumo 6, @U,'','','',0,0, @Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@U", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Almac";
                                DSTDdl.Tables[1].TableName = "Aeronave";
                                DSTDdl.Tables[2].TableName = "Rsva";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];           
            if (DSTDdl.Tables["Aeronave"].Rows.Count > 0)
            {
                DdlAeronave.DataSource = DSTDdl.Tables[1];
                DdlAeronave.DataTextField = "Matricula";
                DdlAeronave.DataValueField = "CodAeronave";
                DdlAeronave.DataBind();
            }
            BindRva();
        }
        protected void BindRva()
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Rsva"].Rows.Count > 0)
            {
                DdlNumRsva.DataSource = DSTDdl.Tables[2];
                if (RdbNumRsva.Checked == true) { DdlNumRsva.DataTextField = "CodNumReserva"; }
                if (RdbRvaOT.Checked == true) { DdlNumRsva.DataTextField = "CodigoOT"; }
                if (RdbRvaRte.Checked == true) { DdlNumRsva.DataTextField = "CodigoRTE"; }
                DdlNumRsva.DataValueField = "CodNumReserva1";
                DdlNumRsva.DataBind();
            }
        }
        protected void RdbNumRsva_CheckedChanged(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BindRva(); }
        protected void RdbRvaOT_CheckedChanged(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BindRva(); }
        protected void RdbRvaRte_CheckedChanged(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BindRva(); }
        protected void BindDetRsva(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Sal_Consumo 10,'','','','',@IdRva,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@IdRva", DdlNumRsva.Text.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSDetRva = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSDetRva);

                                DSDetRva.Tables[0].TableName = "DetRva";
                                DSDetRva.Tables[1].TableName = "CodHk";
                                DSDetRva.Tables[2].TableName = "Asignados";
                                DSDetRva.Tables[3].TableName = "CondManip";
                                ViewState["DSDetRva"] = DSDetRva;
                            }
                        }
                    }
                }
            }
            DSDetRva = (DataSet)ViewState["DSDetRva"];
            if (DSDetRva.Tables["DetRva"].Rows.Count > 0)
            { GrdDtllRitgr.DataSource = DSDetRva.Tables[0]; }
            GrdDtllRitgr.DataBind();

            if (DSDetRva.Tables[1].Rows[0][0].ToString().Trim().Equals("0"))
            { DdlAeronave.Enabled = true; }
            else { DdlAeronave.Text = DSDetRva.Tables[1].Rows[0][0].ToString(); DdlAeronave.Enabled = false; }

            if (DdlNumRsva.Text.Trim().Equals("9999999999")) { LblNumReserva.Text = "" + " - "; }
            else { LblNumReserva.Text = DdlNumRsva.Text.Trim() + " - "; }

            LblNumOrdeTrabajo.Text = DSDetRva.Tables[1].Rows[0][1].ToString(); DdlAeronave.Enabled = false;
            string VbSNumRte = DSDetRva.Tables[1].Rows[0][2].ToString();
            if (VbSNumRte.Equals("")) { LblNumReporte.Text = VbSNumRte; }
            else { LblNumReporte.Text = " - " + VbSNumRte; }
        }
        protected void DdlNumRsva_TextChanged(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BindDetRsva("UPD"); }
        protected void BindCondicManipulac(string CodRef)
        {
            DSDetRva = (DataSet)ViewState["DSDetRva"];
            if (DSDetRva.Tables["CondManip"].Rows.Count > 0)
            {
                DataRow[] DR = DSDetRva.Tables[3].Select("CodReferencia='" + CodRef + "'");
                if (Cnx.ValidaDataRowVacio(DR))
                {
                    DataTable DT = DR.CopyToDataTable();
                    GrdMdlCondManplc.DataSource = DT; GrdMdlCondManplc.DataBind();
                    if (DT.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPopup();", true);
                    }
                }

            }
        }
        protected void BindUbicaciones(string Codref, string PN, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Sal_Consumo 11, @IdRva,'','','', @PosR,0, @Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        string borr = ViewState["PosRva"].ToString().Trim();
                        SC.Parameters.AddWithValue("@IdRva", DdlNumRsva.Text.Trim());
                        SC.Parameters.AddWithValue("@PosR", ViewState["PosRva"].ToString().Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSUbica = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSUbica);

                                DSUbica.Tables[0].TableName = "Ubica";

                                ViewState["DSUbica"] = DSUbica;
                            }
                        }
                    }
                }
            }
            DSUbica = (DataSet)ViewState["DSUbica"];
            if (DSUbica.Tables["Ubica"].Rows.Count > 0)
            { GrdUbicaFisc.DataSource = DSUbica.Tables[0]; }
            GrdUbicaFisc.DataBind();/**/
        }
        protected void GrdDtllRitgr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();

            if (e.CommandName.Equals("Abrir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VblCodRef = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                string VblPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                string VbCantSol = ((Label)row.FindControl("LblCantSol")).Text.ToString().Trim();
                string VbCantEntr = ((Label)row.FindControl("LblCantEntr")).Text.ToString().Trim();
                LblPNDescripcAsig.Text = VblPn + " | " + VblDescPN;
                LblAsigCantSolV.Text = VbCantSol; LblAsigCantEntrgV.Text = VbCantEntr;
                ViewState["CodIdDetalleRes"] = GrdDtllRitgr.DataKeys[gvr.RowIndex].Values["CodIdDetalleRes"].ToString();
                ViewState["PosRva"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim(); ;

                BindUbicaciones(VblCodRef, VblPn, "UPD");
                MultVw.ActiveViewIndex = 1;
                BindCondicManipulac(VblCodRef);/**/
            }
        }
        protected void GrdDtllRitgr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdReitgrTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                DataRowView DRW = e.Row.DataItem as DataRowView;
                double VbDCanSol = Convert.ToDouble(DRW["CantidadSolicitada"].ToString().Trim());
                double VbDCanEnt = Convert.ToDouble(DRW["CantidadEntregada"].ToString().Trim());
                double VbDCanDspch = Convert.ToDouble(DRW["CantDespachar"].ToString().Trim());
                if (VbDCanEnt <= 0)
                {
                    IbtAbrir.Visible = false; e.Row.BackColor = System.Drawing.Color.Wheat;
                }
                if (VbDCanDspch > 0)
                {
                    if ((VbDCanDspch + VbDCanEnt) < VbDCanSol) { e.Row.BackColor = System.Drawing.Color.PaleGreen; }
                    else { e.Row.BackColor = System.Drawing.Color.GreenYellow; }
                    IbtAbrir.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto='GrdReitgrTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void GrdUbicaFisc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbTercero = dr["CodTercero"].ToString().Trim();
                if (VbTercero.Equals("Tercero"))
                {
                    e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }

                TextBox TxtCantDespa = e.Row.FindControl("TxtCantDespa") as TextBox;
                string VbBodegaActiva = dr["Activo"].ToString().Trim();
                if (VbBodegaActiva.Equals("0"))
                {
                    TxtCantDespa.Enabled = false;

                    DataRow[] Result = Idioma.Select("Objeto= 'GrdCantDspcTT'");
                    foreach (DataRow row in Result)
                    { TxtCantDespa.ToolTip = row["Texto"].ToString(); }// Ubicación física inactiva.
                }

                string VbSIsntld = dr["Instalado"].ToString().Trim();
                if (VbSIsntld.Equals("S"))
                {
                    TxtCantDespa.Enabled = false;

                    DataRow[] Result = Idioma.Select("Objeto= 'Mens03Reintgr'");
                    foreach (DataRow row in Result)
                    { TxtCantDespa.ToolTip = row["Texto"].ToString(); }// No es posible realizar el reintegro porque el elemento se encuentra instalado.
                }

            }
        }
        protected void BtnAsignr_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            double VbDAsignadas = 0;
            DSUbica = (DataSet)ViewState["DSUbica"];
            DSDetRva = (DataSet)ViewState["DSDetRva"];

            foreach (GridViewRow GrdRow in GrdUbicaFisc.Rows)
            {
                TextBox TxtCantDespa = (GrdRow.FindControl("TxtCantDespa") as TextBox);
                double VbDCantDesp = Convert.ToDouble(TxtCantDespa.Text.Trim().Equals("") ? "0" : TxtCantDespa.Text.Trim());

                if (VbDCantDesp > 0)
                {
                    double VbDCantEntrgda = Convert.ToDouble((GrdRow.FindControl("LblCantEntregada") as Label).Text.Trim());
                    if (VbDCantDesp > VbDCantEntrgda)
                    {
                        Result = Idioma.Select("Objeto= 'Mens01Reintgr'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La cantidad a despachar supera la cantidad entregada.
                        TxtCantDespa.Focus();
                        return;
                    }
                }
                VbDAsignadas += VbDCantDesp;
            }
            // Valida que las cantidades a reintegrar no supere las pendientes por entregar
            double VbDPendiente = Convert.ToDouble(LblAsigCantEntrgV.Text.Trim());
            if (VbDAsignadas > VbDPendiente)
            {
                Result = Idioma.Select("Objeto= 'Mens02Reintgr'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La cantidad a reintegrar supera la cantidad entregada en su totalidad.

                return;
            }
            // Almacena la vista para realizar el movimeinto de descargue

            foreach (GridViewRow GrdRow in GrdUbicaFisc.Rows)
            {
                TextBox TxtCantDespa = (GrdRow.FindControl("TxtCantDespa") as TextBox);
                double VbDCantDesp = Convert.ToDouble(TxtCantDespa.Text.Trim().Equals("") ? "0" : TxtCantDespa.Text.Trim());
                if (VbDCantDesp > 0)
                {
                    int VbIPos = Convert.ToInt32(ViewState["PosRva"].ToString().Trim());
                    string VbSRef = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodReferencia"].ToString().Trim();
                    string VbSPN = (GrdRow.FindControl("LblPn") as Label).Text.Trim();
                    string VbSSN = (GrdRow.FindControl("LblSn") as Label).Text.Trim();
                    string VbSLt = (GrdRow.FindControl("LblLot") as Label).Text.Trim();
                    string VbSModPN = (GrdRow.FindControl("LblModelPN") as Label).Text.Trim();
                    string VbSBdg = (GrdRow.FindControl("LblBodg") as Label).Text.Trim();
                    string VbSF = (GrdRow.FindControl("LblFila") as Label).Text.Trim();
                    string VbSC = (GrdRow.FindControl("LblColumn") as Label).Text.Trim();
                    double VbDCantEntrgd = Convert.ToDouble((GrdRow.FindControl("LblCantEntregada") as Label).Text.Trim());
                    string VbSUndMed = (GrdRow.FindControl("LblUndMed") as Label).Text.Trim();
                    string VbSCodEle = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodElemento"].ToString().Trim();
                    string VbSCodUbBod = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodUbicaBodega"].ToString().Trim();
                    string VbSIdent = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["IdentificadorElemR"].ToString().Trim();
                    int VbIActivo = Convert.ToInt32(GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["Activo"].ToString().Trim());
                    string VbSCodTerc = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodTercero"].ToString().Trim();
                    string VbSCodEstd = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodEstadoPn"].ToString().Trim();
                    int VbICodALma = Convert.ToInt32(GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodIdAlmacen"].ToString().Trim());
                    string VbSNomAlma = (GrdRow.FindControl("LblNomAlma") as Label).Text.Trim();
                    string VbSCosto = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["Ccosto"].ToString().Trim();
                    int VbIIdPT = Convert.ToInt32(GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["IdPropuesta"].ToString().Trim());
                    int VbIIdDoc = Convert.ToInt32(GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodIdDocumento"].ToString().Trim());
                    string VbSCodUsuD = GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodUsuarioReserva"].ToString().Trim();
                    int VbIIdDetSld = Convert.ToInt32(GrdUbicaFisc.DataKeys[GrdRow.RowIndex].Values["CodIdDetalleSalida"].ToString().Trim());
                    DSDetRva.Tables[2].Rows.Add(VbIPos, VbSRef, VbSPN, VbSSN, VbSLt, VbSModPN, VbSBdg, VbSF, VbSC, VbDCantEntrgd, 0, VbSCodEle, VbSCodUbBod,
                    VbSIdent, VbIActivo, VbSCodTerc, VbDCantDesp, VbSUndMed, VbSCodEstd, VbICodALma, VbSNomAlma, VbSCosto, Convert.ToInt32(ViewState["CodIdDetalleRes"]), VbIIdPT, VbIIdDoc,
                    VbSCodUsuD, VbIIdDetSld);
                    DSDetRva.Tables[2].AcceptChanges();
                }
            }

            //Actualizar la cantidad a despachar en la vista de Detalle Reserva          
            int VblIdDetRes = Convert.ToInt32(ViewState["CodIdDetalleRes"]);
            foreach (DataRow row in DSDetRva.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["CodIdDetalleRes"].ToString()) == VblIdDetRes)
                {
                    row["CantDespachar"] = VbDAsignadas;
                }
            }
            DSDetRva.Tables[0].AcceptChanges();
            BindDetRsva("");
            MultVw.ActiveViewIndex = 0;
        }
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); MultVw.ActiveViewIndex = 0; }
        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSDetRva = (DataSet)ViewState["DSDetRva"];
            if (DSDetRva.Tables["Asignados"].Rows.Count > 0)
            {
                if (TxtObserv.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens22'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//debe ingresar la observacion
                    return;
                }
                DataTable DT = DSDetRva.Tables[2];

                DataView DV = DT.DefaultView;
                DV.Sort = "Pos ASC";
                DT = DV.ToTable();
                GrdVisualizar.DataSource = DT; GrdVisualizar.DataBind();
                LblNumRvaVlorGuardar.Text = DdlNumRsva.Text.Trim();
                MultVw.ActiveViewIndex = 2;
            }
        }
        protected void IbtCloseGuardar_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); MultVw.ActiveViewIndex = 0; }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int VbAfectaInv = 0;
            DSDetRva = (DataSet)ViewState["DSDetRva"];

            try
            {
                List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
                foreach (DataRow Row in DSDetRva.Tables[2].Rows)
                {
                    string VbSCodTerc = Row["CodTercero"].ToString().Trim();

                    if (VbSCodTerc.Equals("")) { VbAfectaInv = 1; }
                    var TypDetalle = new CsInsertElementoAlmacen()
                    {
                        IdIE = Convert.ToInt32(Row["CodIdDetSld"].ToString().Trim()),
                        CodElemento = Row["CodElemento"].ToString().Trim(),
                        CodReferencia = Row["CodReferencia"].ToString().Trim(),
                        PN = Row["PN"].ToString(),
                        SN = Row["SN"].ToString(),
                        Lote = Row["NumLote"].ToString(),
                        CodTipoElem = "",
                        Identificador = Row["IdentificadorElem"].ToString().Trim(),
                        Descripcion = "",
                        Cantidad = Convert.ToDouble(Row["CantDespchr"].ToString().Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(0),// validar el valor  **********************************
                        CodUndMed = Row["CodUndMedR"].ToString().Trim(),
                        IdAlmacen = Convert.ToInt32(Row["IdAlmacen"].ToString().Trim()),
                        CodBodega = Row["CodUbicaBodega"].ToString().Trim(),
                        CodShippingOrder = DdlNumRsva.Text.Trim(),
                        Posicion = Row["Pos"].ToString().Trim(),
                        CodAeronave = Convert.ToInt32(DdlAeronave.Text.Trim()),
                        Matricula = DdlAeronave.SelectedItem.Text.Trim(),
                        CCosto = Row["CCosto"].ToString().Trim(),
                        AfectaInventario = VbAfectaInv,
                        CostoImportacion = Convert.ToInt32(0),
                        CodTercero = "",
                        Consignacion = Convert.ToInt32(0),
                        CodIdUbicacion = Convert.ToInt32(Row["CodIdUbicacion"].ToString().Trim()),
                        FechaVence = null,
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = Row["CodUsuario"].ToString().Trim(),
                        Proceso = "0109",
                        IdDetPropHk = Convert.ToInt32(0),
                        IdPPt = Convert.ToInt32(Row["IdPPT"].ToString().Trim()),
                        Accion = "ENTRADA",
                    };
                    ObjDetalle.Add(TypDetalle);
                }
                CsInsertElementoAlmacen ClaseIEA = new CsInsertElementoAlmacen();
                ClaseIEA.FormOrigen(ViewState["PFileName"].ToString());
                ClaseIEA.Alimentar(ObjDetalle);

                string Mensj = ClaseIEA.GetMensj();
                if (!Mensj.Equals(""))
                {
                    string VblPn = ClaseIEA.GetPn().Trim().Equals("") ? "" : "  [P/N: " + ClaseIEA.GetPn().Trim() + "]  ";
                    string VblSn = ClaseIEA.GetSn().Trim().Equals("") ? "" : " [S/N: " + ClaseIEA.GetSn().Trim() + "] ";
                    string VbLote = ClaseIEA.GetLote().Trim().Equals("") ? "" : " [LT/N: " + ClaseIEA.GetLote().Trim() + "]";
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VblPn + VblSn + "');", true);
                    return;
                }
                TxtObserv.Text = "";
                TraerDatos("UPD");
                BindRva();
                BindDetRsva("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Reintegro", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}