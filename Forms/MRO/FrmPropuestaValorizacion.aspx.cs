using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.prgMro;
using _77NeoWeb.Prg.PrgMro;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
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
        DataTable DtPnNoValoriz = new DataTable();
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
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                ModSeguridad();
                BindDdlPpal("UPDATE");
                ViewState["NomArchivoPlantilla"] = "CommercialQuotationValorizationWeb.xlsx";
                ViewState["PptAnterior"] = "";
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
                    LblNumPpt.Text = bO.Equals("LblNumPpt") ? bT : LblNumPpt.Text;
                    BtnValorizar.Text = bO.Equals("BtnValorizar") ? bT : BtnValorizar.Text;
                    BtnReValorizar.Text = bO.Equals("BtnReValorizar") ? bT : BtnReValorizar.Text;
                    BtnPlantilla.Text = bO.Equals("BtnPlantilla") ? bT : BtnPlantilla.Text;
                    BtnPlantilla.ToolTip = bO.Equals("BtnPlantillaTT") ? bT : BtnPlantilla.ToolTip;
                    BtnExportar.Text = bO.Equals("BtnExportar") ? bT : BtnExportar.Text;
                    BtnPNSinValorizar.Text = bO.Equals("BtnPNSinValorizar") ? bT : BtnPNSinValorizar.Text;
                    BtnPNSinValorizar.ToolTip = bO.Equals("BtnPNSinValorizarTT") ? bT : BtnPNSinValorizar.ToolTip;
                    BtnSolPed.Text = bO.Equals("BtnSolPed") ? bT : BtnSolPed.Text;
                    BtnSolPed.ToolTip = bO.Equals("BtnSolPedTT") ? bT : BtnSolPed.ToolTip;
                    BtnCotizacion.Text = bO.Equals("BtnCotizacion") ? bT : BtnCotizacion.Text;
                    BtnCotizacion.ToolTip = bO.Equals("BtnCotizacionTT") ? bT : BtnCotizacion.ToolTip;
                    BtnCuadroComprtv.Text = bO.Equals("BtnCuadroComprtv") ? bT : BtnCuadroComprtv.Text;
                    BtnCuadroComprtv.ToolTip = bO.Equals("BtnCuadroComprtvTT") ? bT : BtnCuadroComprtv.ToolTip;
                    LblCliente.Text = bO.Equals("LblCliente") ? bT : LblCliente.Text;
                    LblDescTipoPPT.Text = bO.Equals("LblDescTipoPPT") ? bT : LblDescTipoPPT.Text;
                    LblDesEstado.Text = bO.Equals("LblDesEstado") ? bT : LblDesEstado.Text;
                    LblDescPptTipoSol.Text = bO.Equals("LblDescPptTipoSol") ? bT : LblDescPptTipoSol.Text;
                    IbtAprDet1All.ToolTip = bO.Equals("IbtAprDet1AllTT") ? bT : IbtAprDet1All.ToolTip;
                    IbtGrarSP.ToolTip = bO.Equals("IbtGrarSPTT") ? bT : IbtGrarSP.ToolTip;
                    GrdDetValrzc.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDetValrzc.EmptyDataText;
                    GrdDetValrzc.Columns[1].HeaderText = bO.Equals("GrdAprob") ? bT : GrdDetValrzc.Columns[1].HeaderText;
                    GrdDetValrzc.Columns[2].HeaderText = bO.Equals("GrdSeleSP") ? bT : GrdDetValrzc.Columns[2].HeaderText;
                    GrdDetValrzc.Columns[3].HeaderText = bO.Equals("GrdCantSol") ? bT : GrdDetValrzc.Columns[3].HeaderText;
                    GrdDetValrzc.Columns[4].HeaderText = bO.Equals("GrdNumPed") ? bT : GrdDetValrzc.Columns[4].HeaderText;
                    GrdDetValrzc.Columns[5].HeaderText = bO.Equals("GrdTipoCoti") ? bT : GrdDetValrzc.Columns[5].HeaderText;
                    GrdDetValrzc.Columns[6].HeaderText = bO.Equals("GrdOT") ? bT : GrdDetValrzc.Columns[6].HeaderText;
                    GrdDetValrzc.Columns[7].HeaderText = bO.Equals("GrdNomSrvc") ? bT : GrdDetValrzc.Columns[7].HeaderText;
                    GrdDetValrzc.Columns[8].HeaderText = bO.Equals("GrdPNPT") ? bT : GrdDetValrzc.Columns[8].HeaderText;
                    GrdDetValrzc.Columns[9].HeaderText = bO.Equals("Descripcion") ? bT : GrdDetValrzc.Columns[9].HeaderText;
                    GrdDetValrzc.Columns[10].HeaderText = bO.Equals("GrdCantPt") ? bT : GrdDetValrzc.Columns[10].HeaderText;
                    GrdDetValrzc.Columns[11].HeaderText = bO.Equals("GrdCantReal") ? bT : GrdDetValrzc.Columns[11].HeaderText;
                    GrdDetValrzc.Columns[12].HeaderText = bO.Equals("GrdVlrComp") ? bT : GrdDetValrzc.Columns[12].HeaderText;
                    GrdDetValrzc.Columns[13].HeaderText = bO.Equals("GrdMoned") ? bT : GrdDetValrzc.Columns[13].HeaderText;
                    GrdDetValrzc.Columns[14].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdDetValrzc.Columns[14].HeaderText;
                    GrdDetValrzc.Columns[15].HeaderText = bO.Equals("GrdUndComp") ? bT : GrdDetValrzc.Columns[15].HeaderText;
                    GrdDetValrzc.Columns[16].HeaderText = bO.Equals("GrdUltFecCot") ? bT : GrdDetValrzc.Columns[16].HeaderText;
                    GrdDetValrzc.Columns[17].HeaderText = bO.Equals("GrdTimpEntrCot") ? bT : GrdDetValrzc.Columns[17].HeaderText;
                    GrdDetValrzc.Columns[18].HeaderText = bO.Equals("GrdDocRef") ? bT : GrdDetValrzc.Columns[18].HeaderText;
                    GrdDetValrzc.Columns[19].HeaderText = bO.Equals("GrdStock") ? bT : GrdDetValrzc.Columns[19].HeaderText;
                    GrdDetValrzc.Columns[20].HeaderText = bO.Equals("GrdHK") ? bT : GrdDetValrzc.Columns[20].HeaderText;
                    //******************************************Exportar****************************************************************
                    LblTitOpcExportar.Text = bO.Equals("LblTitOpcExportar") ? bT : LblTitOpcExportar.Text;
                    IbtCerrarExportar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarExportar.ToolTip;
                    BtnDetPpt.Text = bO.Equals("BtnDetPpt") ? bT : BtnDetPpt.Text;
                    BtnExpPlantilla.Text = bO.Equals("BtnExpPlantilla") ? bT : BtnExpPlantilla.Text;
                    //**************************************** Panel PN no encontrados en la valorización ****************************************
                    IbtClosePNoValorizado.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtClosePNoValorizado.ToolTip;
                    LblTitEleNoValorizado.Text = bO.Equals("LblTitEleNoValorizado") ? bT : LblTitEleNoValorizado.Text;
                    GrdPnNoValorizado.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdPnNoValorizado.EmptyDataText;
                    GrdPnNoValorizado.Columns[0].HeaderText = bO.Equals("GrdNrRpt") ? bT : GrdPnNoValorizado.Columns[0].HeaderText;
                    GrdPnNoValorizado.Columns[1].HeaderText = bO.Equals("GrdOT") ? bT : GrdPnNoValorizado.Columns[1].HeaderText;
                    GrdPnNoValorizado.Columns[2].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdPnNoValorizado.Columns[2].HeaderText;
                    GrdPnNoValorizado.Columns[4].HeaderText = bO.Equals("GrdFecRva") ? bT : GrdPnNoValorizado.Columns[4].HeaderText;
                    GrdPnNoValorizado.Columns[5].HeaderText = bO.Equals("GrdCreaPN") ? bT : GrdPnNoValorizado.Columns[5].HeaderText;
                    GrdPnNoValorizado.Columns[6].HeaderText = bO.Equals("GrdFechNotf") ? bT : GrdPnNoValorizado.Columns[6].HeaderText;
                    GrdPnNoValorizado.Columns[7].HeaderText = bO.Equals("GrdFechValoriza") ? bT : GrdPnNoValorizado.Columns[7].HeaderText;
                }
                DataRow[] Result;
                Result = Idioma.Select("Objeto= 'BtnValorizarOnCl'");
                foreach (DataRow row in Result) { BtnValorizar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'BtnReValorizarOnCl'");
                foreach (DataRow row in Result) { BtnReValorizar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'BtnPlantillaOnCl'");
                foreach (DataRow row in Result) { BtnPlantilla.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                Result = Idioma.Select("Objeto= 'IbtGrarSPOnCl'");
                foreach (DataRow row in Result) { IbtGrarSP.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

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
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            ViewState["Valorizada"] = "SIN";
            ViewState["EstatusPPT"] = "SIN";
            ViewState["CarpetaCargaMasiva"] = "SIN";
            BtnValorizar.Enabled = false; BtnReValorizar.Enabled = false; BtnValorizar.ToolTip = "";
            BtnPlantilla.Enabled = false; BtnPlantilla.ToolTip = "";
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC DetalleValorizacion @PT,@ICC,''";
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
                            if (DtDet.Rows.Count > 0)
                            {
                                DataRow row = DtDet.Rows[0];// solo el primer registro
                                ViewState["Valorizada"] = row["Valorizada"].ToString();
                                ViewState["EstatusPPT"] = row["CodEstadoPropuesta"].ToString();
                                ViewState["CarpetaCargaMasiva"] = row["CarpetaCargaMasiva"].ToString();
                                TxtDesEstado.Text = row["DescripcionEstado"].ToString().Trim();
                            }

                        }
                    }
                }
            }
            DtDet = (DataTable)ViewState["DtDet"];
            if (DtDet.Rows.Count > 0) { GrdDetValrzc.DataSource = DtDet; }
            else { GrdDetValrzc.DataSource = null; }
            GrdDetValrzc.DataBind();
            if (ViewState["Valorizada"].ToString().Equals("S"))
            {
                BtnValorizar.Enabled = false; BtnReValorizar.Enabled = true;
                Result = Idioma.Select("Objeto= 'BtnValorizarTT'");
                foreach (DataRow row in Result)
                { BtnValorizar.ToolTip = row["Texto"].ToString(); }//La propuesta se encuentra valorizada.

                BtnPlantilla.Enabled = false; BtnPlantilla.ToolTip = "";
            }
            if (ViewState["Valorizada"].ToString().Equals("N"))
            {
                BtnValorizar.Enabled = true; BtnReValorizar.Enabled = false;
                BtnPlantilla.Enabled = true;
                Result = Idioma.Select("Objeto= 'BtnPlantillaTT'");
                foreach (DataRow row in Result)
                { BtnPlantilla.ToolTip = row["Texto"].ToString() + " " + ViewState["CarpetaCargaMasiva"].ToString() + ViewState["NomArchivoPlantilla"]; }
            }

            string StPpt = ViewState["EstatusPPT"].ToString().Trim();
            if (StPpt.Equals("03") || StPpt.Equals("04") || StPpt.Equals("05") || StPpt.Equals("06") || StPpt.Equals("07") || StPpt.Equals("10") || StPpt.Equals("12") || StPpt.Equals("14"))
            { IbtGrarSP.Enabled = true; }
            else { IbtGrarSP.Enabled = false; }
        }
        protected void DdlNumPpt_TextChanged(object sender, EventArgs e)
        {
            DtDdlPpal = (DataTable)ViewState["DtDdlPpal"];
            ViewState["PptAnterior"] = DdlNumPpt.Text.Trim();
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
            if (DdlNumPpt.Text.Equals("0")) { return; }
            if (ViewState["PptAnterior"].ToString().Trim().Equals(DdlNumPpt.Text.Trim())) { BIndDPnSinValorizar(); }
            else { MultVw.ActiveViewIndex = 2; }
        }
        protected void BtnSolPed_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/Almacen/FrmSolicitudPedido.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
        protected void BtnCotizacion_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/InventariosCompras/FrmCotizacion.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
        protected void BtnCuadroComprtv_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/InventariosCompras/FrmCuadroComparativoCotiza.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
        protected void BtnValorizar_Click(object sender, EventArgs e)
        {
            if (DdlNumPpt.Text.Equals("0")) { return; }

            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            List<CsTypPropuestaValorizada> ObjPropuestaValorizada = new List<CsTypPropuestaValorizada>();
            foreach (GridViewRow Row in GrdDetValrzc.Rows)
            {


                string VbUndMedCompr = (Row.FindControl("LblUndMedCmpra") as Label).Text.Trim().Equals(null) ? "" : (Row.FindControl("LblUndMedCmpra") as Label).Text.Trim();
                double VbEquvl = Convert.ToDouble(GrdDetValrzc.DataKeys[Row.RowIndex].Values["EquivalenciaPV"].ToString().Trim().Equals("") ? "0" : GrdDetValrzc.DataKeys[Row.RowIndex].Values["EquivalenciaPV"].ToString());

                DateTime? VbFechUltComp;
                if ((Row.FindControl("LblFechUlmCmp") as Label).Text.Trim().Equals("")) { VbFechUltComp = null; }
                else { VbFechUltComp = Convert.ToDateTime((Row.FindControl("LblFechUlmCmp") as Label).Text.Trim()); }
                var TypPropuestaValorizada = new CsTypPropuestaValorizada()
                {
                    IdValorizacion = Convert.ToInt32(0),
                    IdPropuesta = Convert.ToInt32(DdlNumPpt.Text.Trim()),
                    IdServicio = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                    NomServicio = (Row.FindControl("LblNomSvc") as Label).Text.Trim(),
                    PnPropuesta = (Row.FindControl("LblPnPpt") as Label).Text.Trim(),
                    CodReferencia = GrdDetValrzc.DataKeys[Row.RowIndex].Values[1].ToString().Trim(),
                    Descripcion = (Row.FindControl("LblDescElem") as Label).Text.Trim(),
                    CantidadPropuesta = Convert.ToDouble((Row.FindControl("CantPpt") as Label).Text.Trim()),
                    ValorCompra = Convert.ToDouble((Row.FindControl("TxtVlr") as TextBox).Text.Trim()),
                    DocReferencia = (Row.FindControl("LblDocRef") as Label).Text.Trim(),
                    FechaUltimaCompra = VbFechUltComp,
                    TiempoEntregaDiasCoti = Convert.ToInt32((Row.FindControl("TxtTiemEntrDiaCot") as TextBox).Text.Trim()),
                    PnStock = GrdDetValrzc.DataKeys[Row.RowIndex].Values[2].ToString().Trim(),
                    CantStock = Convert.ToDouble((Row.FindControl("LblCntStk") as Label).Text.Trim()),
                    CodIdUbicacion = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values[3].ToString().Trim()),
                    Bodega = GrdDetValrzc.DataKeys[Row.RowIndex].Values[4].ToString().Trim(),
                    StockMinimo = Convert.ToDouble(GrdDetValrzc.DataKeys[Row.RowIndex].Values[5].ToString().Trim()),
                    CodTipoCotiza = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values[6].ToString().Trim()),
                    SelectBodeg = 1, //Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values[7].ToString().Trim()),
                    SelectSolicitud = Convert.ToInt32(0),
                    CantidadSolicitud = Convert.ToDouble(GrdDetValrzc.DataKeys[Row.RowIndex].Values[8].ToString().Trim()),
                    ObservacionValorizar = GrdDetValrzc.DataKeys[Row.RowIndex].Values[9].ToString().Trim(),
                    Posicion = Convert.ToInt32((Row.FindControl("LblPos") as Label).Text.Trim()),
                    NomBodega = GrdDetValrzc.DataKeys[Row.RowIndex].Values[10].ToString().Trim(),
                    TiempoEntregaDias = Convert.ToInt32((Row.FindControl("TxtTiemEntrDiaCot") as TextBox).Text.Trim()) + 3,
                    Usu = Session["C77U"].ToString(),
                    Aprobado = (Row.FindControl("CkbAprobP") as CheckBox).Checked == true ? 1 : 0,
                    IdReporte = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values[11].ToString().Trim()),
                    NumPedido = (Row.FindControl("LblNumSP") as Label).Text.Trim(),
                    MonedaProVa = (Row.FindControl("TxtMnda") as TextBox).Text.Trim(),
                    UndMedProVa = (Row.FindControl("LblUndMPt") as Label).Text.Trim(),
                    UnidMinCompra = Convert.ToDouble(GrdDetValrzc.DataKeys[Row.RowIndex].Values[12].ToString().Trim()),
                    CodEstado = GrdDetValrzc.DataKeys[Row.RowIndex].Values[13].ToString().Trim(),
                    PnAlternoPV = GrdDetValrzc.DataKeys[Row.RowIndex].Values[14].ToString().Trim(),
                    TipoCotizacion = (Row.FindControl("LblTipoCot") as Label).Text.Trim(),
                    IdDetPropSrv = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["IdDetPropSrv"].ToString().Trim()),
                    RepaExterna = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["RepaExterna"].ToString().Trim()),
                    CantRealPV = Convert.ToDouble((Row.FindControl("LblCntReal") as Label).Text.Trim()),
                    UndCompraPV = VbUndMedCompr,
                    EquivalenciaPV = VbEquvl,
                    OTVal = Convert.ToInt32((Row.FindControl("LblOtVal") as Label).Text.Trim()),
                    CodAeronaveVal = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["CodAeronaveVal"].ToString().Trim()),
                    MatriculaVal = (Row.FindControl("LblMatric") as Label).Text.Trim(),
                    SNElementoV = GrdDetValrzc.DataKeys[Row.RowIndex].Values["SNElementoV"].ToString().Trim(),
                    IdConfigCia = (int)Session["!dC!@"],
                    Accion = "INSERT",
                };
                ObjPropuestaValorizada.Add(TypPropuestaValorizada);
            }
            CsTypPropuestaValorizada ClsTypPropuestaValorizada = new CsTypPropuestaValorizada();
            ClsTypPropuestaValorizada.Alimentar(ObjPropuestaValorizada);
            string Mensj = ClsTypPropuestaValorizada.GetMensj();
            if (!Mensj.Equals(""))
            {
                DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result2)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                return;
            }
            BindDetalle("UPDATE");
            BIndDPnSinValorizar();
        }
        protected void BtnReValorizar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlNumPpt.Text.Equals("0")) { return; }
            if (!ViewState["Valorizada"].ToString().Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens05VPT'");
                foreach (DataRow row in Result) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }
                return;
            }

            if (ViewState["EstatusPPT"].ToString().Trim().Equals("07") || ViewState["EstatusPPT"].ToString().Trim().Equals("09") || ViewState["EstatusPPT"].ToString().Trim().Equals("11") || ViewState["EstatusPPT"].ToString().Trim().Equals("15") || ViewState["EstatusPPT"].ToString().Trim().Equals("16"))
            {//Cumplida | Cancelada | Devolución | No aprobada
                Result = Idioma.Select("Objeto= 'Mens14PPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La propuesta se encuentra cumplida, cancelada, en estado devolución o marcada como no aprobada.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasMRO 16,@PP,@Us,'','','','','','','REVALORIZAR',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@PP", DdlNumPpt.Text.Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindDetalle("UPDATE");
                        }
                        catch (Exception) { Transac.Rollback(); }
                    }
                }
            }
        }
        protected void BtnPlantilla_Click(object sender, EventArgs e)
        {
            if (DdlNumPpt.Text.Equals("0")) { return; }
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];

                DataTable DT = new DataTable();
                string conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ViewState["CarpetaCargaMasiva"].ToString().Trim() + ViewState["NomArchivoPlantilla"] + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbConnection cnn = new OleDbConnection(conexion))
                {
                    cnn.Open();
                    DataTable dtExcelSchema;
                    dtExcelSchema = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    cnn.Close();

                    cnn.Open();
                    // string sql = "SELECT *FROM [Tabla$]";
                    string sql = "SELECT * FROM [" + SheetName + "]";
                    OleDbCommand command = new OleDbCommand(sql, cnn);
                    OleDbDataAdapter DA = new OleDbDataAdapter(command);

                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DtDet = (DataTable)ViewState["DtDet"];

                        DataRow[] ComprasConVlr = DT.Select("PurchaseValue>0");
                        foreach (DataRow RCCV in ComprasConVlr)
                        {
                            foreach (DataRow R in DtDet.Rows)
                            {
                                if (R["PnPropuesta"].ToString().Equals(RCCV["PN"]))
                                {
                                    R["ValorCompra"] = RCCV["PurchaseValue"];
                                    R["MonedaProVa"] = RCCV["Currency"];
                                    R["TiempoEntregaDiasCoti"] = RCCV["TimeDeliveryDaysQuote"];
                                    R["CodEstado"] = RCCV["CodeStatus"];
                                    R["PnAlternoPV"] = RCCV["AlternatePn"];
                                    R["UnidMinCompra"] = RCCV["UnitMinPurchase"];
                                }
                            } /**/
                        }
                    }
                    cnn.Close();
                    GrdDetValrzc.DataSource = DtDet;
                    GrdDetValrzc.DataBind();
                }
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Valorizar desde Planilla", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (DdlNumPpt.Text.Equals("0")) { return; }
            MultVw.ActiveViewIndex = 1;
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
            Label LblUndMPt = (Label)GrdDetValrzc.Rows[rowIndex].FindControl("LblUndMPt");
            TextBox TxtCantSP = (TextBox)GrdDetValrzc.Rows[rowIndex].FindControl("TxtCantSP");
            TxtCantSP.Text = CkbGenrSP.Checked == true ? CantPpt.Text : "0";
            if (LblUndMPt.Text.Trim().Equals(""))
            {
                TxtCantSP.Text = "0";
                CkbGenrSP.Checked = false;
            }
        }
        protected void GrdDetValrzc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        //******************************************< Exportar >*************************************************
        protected void IbtCerrarExportar_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            MultVw.ActiveViewIndex = 0;
        }
        protected void Exportar(string Opcion)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            string VbNomRpt = "";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            string VbTxtSql = "";

            if (Opcion.Equals("RefSinRev"))
            {
                CursorIdioma.Alimentar("CurExportRefSinRevisar", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC DetalleValorizacion @PT,@ICC,'EXPORTAR'";
                Result = Idioma.Select("Objeto= 'CurRefSinRev'");
                foreach (DataRow row in Result)
                { VbNomRpt = row["Texto"].ToString(); }//ReferenciaSinRevisar
            }
            if (Opcion.Equals("Plantilla"))
            {
                CursorIdioma.Alimentar("CurExportPlantillaValorizacion", Session["77IDM"].ToString().Trim());
                VbTxtSql = "EXEC DetalleValorizacion @PT,@ICC,'PLANTILLA'";
                Result = Idioma.Select("Objeto= 'CurExpForPlntll'");
                foreach (DataRow row in Result)
                { VbNomRpt = row["Texto"].ToString(); }//FormatoPlantilla
            }

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
                    SC.CommandTimeout = 90000000;
                    SC.Parameters.AddWithValue("@PT", DdlNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SC.Connection = con;
                        sda.SelectCommand = SC;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            ds.Tables[0].TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    wb.Worksheets.Add(dt);
                                }
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
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
        protected void BtnDetPpt_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Exportar("RefSinRev");// Exportar el detalle 
        }
        protected void BtnExpPlantilla_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Exportar("Plantilla");// Exportar formato para plantilla
        }
        protected void IbtAprDet1All_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (DdlNumPpt.Text.Equals("0")) { return; }
            DtDet = (DataTable)ViewState["DtDet"];

            foreach (DataRow Dtll in DtDet.Rows)
            {
                if (!Dtll["CodReferencia"].ToString().Trim().Equals(""))
                    Dtll["SelectSolicitud"] = "1"; Dtll["CantidadSolicitud"] = Dtll["CantidadPropuesta"];
            }
            GrdDetValrzc.DataSource = DtDet;
            GrdDetValrzc.DataBind();
        }
        protected void IbtGrarSP_Click(object sender, ImageClickEventArgs e)
        {
            if (DdlNumPpt.Text.Equals("0")) { return; }

            string VbMarcado = "N", VbSPAsig = "N", VbCant0 = "N", VbTipoSP = "";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DtDet = (DataTable)ViewState["DtDet"];
            foreach (GridViewRow Row in GrdDetValrzc.Rows)
            {
                CheckBox CkbGenrSP = Row.FindControl("CkbGenrSP") as CheckBox;
                Label LblNumSP = Row.FindControl("LblNumSP") as Label;
                TextBox TxtCantSP = Row.FindControl("TxtCantSP") as TextBox;
                string VbCan = TxtCantSP.Text.Trim().Equals("") ? "0" : TxtCantSP.Text.Trim();
                if (CkbGenrSP != null)
                {
                    if (CkbGenrSP.Checked == true) { VbMarcado = "S"; VbTipoSP = GrdDetValrzc.DataKeys[Row.RowIndex].Values["RepaExterna"].ToString().Trim().Equals("1") ? "02" : "01"; }
                }
                if (CkbGenrSP.Checked == true && !LblNumSP.Text.Equals("UNK")) { VbSPAsig = "S"; }
                if (CkbGenrSP.Checked == true && Convert.ToDouble(VbCan) <= 0) { VbCant0 = "S"; }
            }
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DataRow[] Result;
            if (VbMarcado.Equals("N"))
            {
                Result = Idioma.Select("Objeto= 'Mens07VPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe seleccionar por lo menos un ítem para generar la solicitud.
                return;
            }

            if (VbSPAsig.Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens08VPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Existen ítems que se encuentran con solicitud de pedido asignada
                return;
            }

            if (VbCant0.Equals("S"))
            {
                Result = Idioma.Select("Objeto= 'Mens11VPT'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Existen registros seleccionados sin cantidad o menores a cero.
                return;
            }

            string VblObs = "";
            Result = Idioma.Select("Objeto= 'Mens10VPT'");
            foreach (DataRow row in Result)
            { VblObs = row["Texto"].ToString().Trim() + " [" + DdlNumPpt.Text.Trim() + "]"; }// Solicitud generada de la propuesta Nro


            List<ClsTypSolicitudPedidoPPT> ObjEncSP = new List<ClsTypSolicitudPedidoPPT>();
            var TypEncSP = new ClsTypSolicitudPedidoPPT()
            {
                IdPedido = 0,
                CodPedido = "",
                Fechapedido = Convert.ToDateTime(DateTime.Now),
                CodPrioridad = "NORMAL",
                CodResponsable = Session["C77U"].ToString(),
                CodReserva = 0,
                CodEstado = "A",
                Obsevacion = VblObs,
                CodtipoSolPedido = VbTipoSP.Trim(),
                Ccostos = "",
                Usu = Session["C77U"].ToString(),
                CodTipoCodigo = Session["CodTipoCodigoInicial"].ToString(),
                FechaRemocionSP = null,
                Aplicabilidad = "",
                Accion = "INSERT",
            };
            ObjEncSP.Add(TypEncSP);

            List<ClsTypSolicitudPedidoPPT> ObjDetSP = new List<ClsTypSolicitudPedidoPPT>();
            foreach (GridViewRow Row in GrdDetValrzc.Rows)
            {
                if ((Row.FindControl("CkbGenrSP") as CheckBox).Checked == true)
                {
                    var TypDetSP = new ClsTypSolicitudPedidoPPT()
                    {
                        IdDetPedido = 0,
                        CodReferencia = GrdDetValrzc.DataKeys[Row.RowIndex].Values["CodReferencia"].ToString().Trim(),//(Row.FindControl("LblUndMedCmpra") as Label).Text.Trim(),
                        PN = (Row.FindControl("LblPnPpt") as Label).Text.Trim(),
                        CodUndMedida = (Row.FindControl("LblUndMPt") as Label).Text.Trim(),
                        CantidadTotal = Convert.ToDouble((Row.FindControl("TxtCantSP") as TextBox).Text.Trim()),
                        CantidadAlmacen = Convert.ToDouble((Row.FindControl("TxtCantSP") as TextBox).Text.Trim()),
                        CantidadReparacion = 0,
                        CantidadOrden = 0,
                        Posicion = 1,
                        AprobacionDetalle = 1,
                        CodSeguimiento = "SOL",
                        Descripcion = VblObs.Trim(),
                        TipoPedido = 0,
                        CantidadAjustada = Convert.ToDouble((Row.FindControl("TxtCantSP") as TextBox).Text.Trim()),
                        Notas = GrdDetValrzc.DataKeys[Row.RowIndex].Values["SNElementoV"].ToString().Trim(),
                        PosicionPr = Convert.ToInt32((Row.FindControl("LblPos") as Label).Text.Trim()),
                        IdSrvPr = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["IdServicio"].ToString().Trim()),
                        IdReporte = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["IdReporte"].ToString().Trim()),
                        IdDetProPSrvSP = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["IdDetPropSrv"].ToString().Trim()),
                        CodIdDetalleResSP = 0,
                        FechaAprob = null,
                        CodAeronaveSP = Convert.ToInt32(GrdDetValrzc.DataKeys[Row.RowIndex].Values["CodAeronaveVal"].ToString().Trim()),
                    };
                    ObjDetSP.Add(TypDetSP);
                }
            }

            ClsTypSolicitudPedidoPPT TypSolicitudPedido = new ClsTypSolicitudPedidoPPT();
            TypSolicitudPedido.NumPPT(Convert.ToInt32(DdlNumPpt.Text));
            TypSolicitudPedido.Alimentar(ObjEncSP, ObjDetSP);
            string Mensj = TypSolicitudPedido.GetMensj();
            if (!Mensj.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                return;
            }
            BindDetalle("UPDATE");
            string VbCodPedido = TypSolicitudPedido.GetCodPedido();
            DataRow[] Result1 = Idioma.Select("Objeto= 'MstrMens03'");
            foreach (DataRow row in Result1)
            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + " [" + VbCodPedido + "]" + "');", true); }// Se generó la solicitud Nro

        }
        //************************** P/N no  valorizados ***********************************************
        protected void BIndDPnSinValorizar()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC Seguimiento_Propuesta_Valorizacion_VS_Reserva_WEB @PP,@CC,'WEB'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@PP", DdlNumPpt.Text.Trim());
                    SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtPnNoValoriz);

                        if (DtPnNoValoriz.Rows.Count > 0)
                        {
                            GrdPnNoValorizado.DataSource = DtPnNoValoriz; GrdPnNoValorizado.DataBind();
                            MultVw.ActiveViewIndex = 2;
                        }
                    }
                }
            }
        }
        protected void IbtClosePNoValorizado_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
       
    }
}