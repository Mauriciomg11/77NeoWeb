using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgManto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmUbicacionElemento : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DtAll = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            } /* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "";
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = "00000133"; //00000082|00000133
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
                BindDataDdl();
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["AplicaCiaFechVenc"] = "N";
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//Trasferir entre bodegas 
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }// Bodega Repa
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//Cambio de lotes
          
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
                SC.Parameters.AddWithValue("@F2", "FrmIncoming");
                SC.Parameters.AddWithValue("@F3", "");
                SC.Parameters.AddWithValue("@F4", "");
                sqlCon.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                while (tbl.Read())  //Todos los objetos
                {
                    string bO = tbl["Objeto"].ToString().Trim();
                    string bT = tbl["Texto"].ToString().Trim();
                    Idioma.Rows.Add(bO, bT);
                    if (bO.Equals("CaptionAsig"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("CaptionAsig") ? bT : TitForm.Text;
                    LblAlmacen.Text = bO.Equals("LblAlmacen") ? bT : LblAlmacen.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT : LblTipo.Text;
                    BtnConsult.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsult.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtBusBodeg.Attributes.Add("placeholder", bT); }
                    LblBusBodeg.Text = bO.Equals("GrdBod") ? bT : LblBusBodeg.Text;
                    LblTitUbicaFis.Text = bO.Equals("LblTitUbicaFis") ? bT : LblTitUbicaFis.Text;
                    GrdDatos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDatos.EmptyDataText;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdAsig") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdSP") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdLot") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdRef") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdCant") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdUM") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("GrdBod") ? bT : GrdDatos.Columns[8].HeaderText;
                    GrdDatos.Columns[9].HeaderText = bO.Equals("GrdF") ? bT : GrdDatos.Columns[9].HeaderText;
                    GrdDatos.Columns[10].HeaderText = bO.Equals("GrdC") ? bT : GrdDatos.Columns[10].HeaderText;
                    GrdDatos.Columns[11].HeaderText = bO.Equals("GrdCamLt") ? bT : GrdDatos.Columns[11].HeaderText;
                    //*******************Asignar***************************
                    IbtCerrarCambioBod.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCambioBod.ToolTip;
                    LblTitCambioBod.Text = bO.Equals("LblTitCambioBod") ? bT : LblTitCambioBod.Text;
                    LblLote.Text = bO.Equals("GrdLot") ? bT : LblLote.Text;
                    LblBodOrig.Text = bO.Equals("LblBodOrig") ? bT : LblBodOrig.Text;
                    LblBodDest.Text = bO.Equals("LblBodDest") ? bT : LblBodDest.Text;
                    LblCantAct.Text = bO.Equals("LblCantAct") ? bT : LblCantAct.Text;
                    LblCantNew.Text = bO.Equals("LblCantNew") ? bT : LblCantNew.Text;
                    LblTitBodDes.Text = bO.Equals("LblBodDest") ? bT : LblTitBodDes.Text;
                    GrdUbicaDes.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdUbicaDes.EmptyDataText;
                    GrdUbicaDes.Columns[0].HeaderText = bO.Equals("GrdTrasf") ? bT : GrdUbicaDes.Columns[0].HeaderText;
                    GrdUbicaDes.Columns[1].HeaderText = bO.Equals("GrdF") ? bT : GrdUbicaDes.Columns[1].HeaderText;
                    GrdUbicaDes.Columns[2].HeaderText = bO.Equals("GrdC") ? bT : GrdUbicaDes.Columns[2].HeaderText;
                    //*******************Cambio***************************
                    IbtCerrarCambioLote.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCambioLote.ToolTip;
                    LblTitCambLote.Text = bO.Equals("GrdCamLt") ? bT : LblTitCambLote.Text;
                    LblCLNumLot.Text = bO.Equals("GrdLot") ? bT : LblCLNumLot.Text;
                    LblCLCantOrg.Text = bO.Equals("LblCantAct") ? bT : LblCLCantOrg.Text;
                    LblCLNewLot.Text = bO.Equals("LblCLNewLot") ? bT : LblCLNewLot.Text;
                    LblCLNewCant.Text = bO.Equals("LblCantNew") ? bT : LblCLNewCant.Text;
                    BtnTranLote.Text = bO.Equals("BtnTranLote") ? bT : BtnTranLote.Text;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDataDdl()
        {

            DataTable HK = new DataTable();
            DataTable Tipo = new DataTable();
            string VbTxtSql = "EXEC SP_PANTALLA_Incoming 3,@U,'','','ALL',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@U", Session["C77U"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(DtAll);
                        ViewState["DtAll"] = DtAll;
                        HK = DtAll.Clone();
                        DataRow[] Result = DtAll.Select("Filtro='ALMA'");
                        foreach (DataRow Row in Result)
                        { HK.ImportRow(Row); }
                        DdlAlmacen.DataSource = HK;
                        DdlAlmacen.DataTextField = "Descr";
                        DdlAlmacen.DataValueField = "Cod";
                        DdlAlmacen.DataBind();

                        Tipo = DtAll.Clone();
                        Result = DtAll.Select("Filtro='Tipo'");
                        foreach (DataRow Row in Result)
                        { Tipo.ImportRow(Row); }
                        DdlTipo.DataSource = Tipo;
                        DdlTipo.DataTextField = "Descr";
                        DdlTipo.DataValueField = "Cod";
                        DdlTipo.DataBind();

                    }
                }
            }
        }
        protected void BIndDatos()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "EXEC SP_PANTALLA_Asignacion 2,@Pr,@T,@Pr2,'',@Al,0,1,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Al", DdlAlmacen.Text.Trim());
                    SC.Parameters.AddWithValue("@Pr", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@T", DdlTipo.Text.Trim());
                    SC.Parameters.AddWithValue("@Pr2", TxtBusBodeg.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdDatos.DataSource = DtB; GrdDatos.DataBind(); }
                        else { GrdDatos.DataSource = null; GrdDatos.DataBind(); }
                    }
                }
            }
        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        { BIndDatos(); }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            ViewState["CodIdUbicacion"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodIdUbicacion"].ToString();
            ViewState["CodTercero"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodTercero"].ToString();
            string VbCabt = ((Label)row.FindControl("LblCant")).Text.ToString().Trim();
            string VbPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
            string VbLot = TxtLote.Text = ((Label)row.FindControl("LblLote")).Text.ToString().Trim();
            if (Convert.ToDouble(VbCabt) <= 0) { return; }
            if (e.CommandName.Equals("TrasldBod"))
            {
                MultVw.ActiveViewIndex = 1;
                TxtCantNew.Enabled = true; TxtCantNew.Text = "0";

                ViewState["SPR"] = ((CheckBox)row.FindControl("CkbSPP")).Checked == true ? "Y" : "N";

                ViewState["CodUbicaBodega"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodUbicaBodega"].ToString();
                ViewState["CodElemento"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodElemento"].ToString();
                ViewState["CodTipoElemento"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodTipoElemento"].ToString();
                ViewState["IdentificadorElem"] = GrdDatos.DataKeys[gvr.RowIndex].Values["IdentificadorElem"].ToString();
                ViewState["Activo"] = GrdDatos.DataKeys[gvr.RowIndex].Values["Activo"].ToString();
                ViewState["FechaVencimientoR"] = GrdDatos.DataKeys[gvr.RowIndex].Values["FechaVencimientoR"].ToString();
                ViewState["Reparable"] = GrdDatos.DataKeys[gvr.RowIndex].Values["Reparable"].ToString();
                ViewState["CodEstadoBodega"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodEstadoBodega"].ToString();
                if (ViewState["AplicaCiaFechVenc"].ToString().Equals("S")) // cuando aplica para la cia fecha vence
                {
                    LblFechI.Visible = true; TxtFechI.Visible = true;
                    if (Convert.ToInt32(ViewState["FechaVencimientoR"]) == 0) { LblFechI.Visible = false; TxtFechI.Visible = false; }
                }
                if (ViewState["IdentificadorElem"].ToString().Equals("SN")) { TxtCantNew.Enabled = false; TxtCantNew.Text = "1"; }
                ViewState["CodBodegaOrig"] = ((Label)row.FindControl("LblCodBod")).Text.ToString().Trim();
                TxtPN.Text = VbPn;
                TxtSN.Text = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                TxtLote.Text = VbLot;
                TxtBodOrig.Text = ((Label)row.FindControl("LblCodBod")).Text.ToString().Trim();
                TxtCantAct.Text = VbCabt.ToString();
                TxtUndM.Text = ((Label)row.FindControl("LblUndM")).Text.ToString().Trim();
                if (ViewState["SPR"].ToString().Trim().Equals("Y")) { BindDDdlDestino("SP"); }
                else
                {
                    if (GrdDatos.DataKeys[gvr.RowIndex].Values["BAJA"].ToString().Equals("Y")) { }// BindDDdlDestino("BAJA");
                    if (GrdDatos.DataKeys[gvr.RowIndex].Values["REPA"].ToString().Equals("Y")) { BindDDdlDestino("REPA"); }
                    if (GrdDatos.DataKeys[gvr.RowIndex].Values["MANTO"].ToString().Equals("Y")) { BindDDdlDestino("MANTO"); }
                    if (ViewState["CodEstadoBodega"].Equals("01") || ViewState["CodEstadoBodega"].Equals("06") || ViewState["CodEstadoBodega"].Equals("80")) { BindDDdlDestino("DISPON"); }
                }

                Page.Title = ViewState["PageTit"].ToString();
            }
            if (e.CommandName.Equals("CambLot"))
            {
                MultVw.ActiveViewIndex = 2;
                TxtCLPN.Text = VbPn;
                TxtCLNumLot.Text = VbLot;
                TxtCLCantOrg.Text = VbCabt.ToString();
                TxtCLNewLot.Text = "";
                TxtCLNewCant.Text = "0";
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                ImageButton IbtAsig = e.Row.FindControl("IbtAsig") as ImageButton;
                ImageButton IbtCambLot = e.Row.FindControl("IbtCambLot") as ImageButton;
                if (IbtAsig != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtAsig'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsig.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    if (dr["Activo"].ToString().Equals("0")) { IbtAsig.Visible = false; }
                    if (dr["INTA"].ToString().Equals("Y") || dr["NALS"].ToString().Equals("Y")) { IbtAsig.Visible = false; }
                    if (dr["CodEstadoBodega"].ToString().Equals("04")) { IbtAsig.Visible = false; }
                    if ((int)ViewState["VblCE1"] == 0) { IbtAsig.Visible = false; }
                }
                if (IbtCambLot != null)
                {
                    IbtCambLot.ToolTip = GrdDatos.Columns[11].HeaderText;
                    if (dr["CodEstadoBodega"].ToString().Equals("01") && dr["Activo"].ToString().Equals("1") && dr["IdentificadorElem"].ToString().Equals("LOTE"))
                    { IbtCambLot.Visible = true; }
                    if ((int)ViewState["VblCE3"] == 0) { IbtCambLot.Visible = false; }
                }
                string VbCap = dr["CodTercero"].ToString();
                if (!VbCap.Trim().Equals("")) { e.Row.BackColor = System.Drawing.Color.Orange; }

                CheckBox CkbSPP = e.Row.FindControl("CkbSPP") as CheckBox;
                if (dr["CodEstadoBodega"].ToString().Equals("01") && dr["Reparable"].ToString().Trim().Equals("Y")) { CkbSPP.Enabled = true; }
                if ((int)ViewState["VblCE2"] == 0) { CkbSPP.Visible = false; }
            }
        }
        //********************** Traslado Bodega *************************************
        protected void BindDDdlDestino(string filtro)
        {
            DtAll = (DataTable)ViewState["DtAll"];
            DataTable DTDstn = new DataTable();
            DataRow[] Result;
            DTDstn = DtAll.Clone();
            switch (filtro)
            {
                case "REPA":
                    Result = DtAll.Select("Filtro='ASIGNAC' AND ((Descr='REPA' AND EstadBod='02') OR (Descr='MANTO' AND EstadBod='02') OR (Descr='POSBAJA' AND EstadBod='06') OR Descr=' - ')");
                    foreach (DataRow Row in Result)
                    { DTDstn.ImportRow(Row); }
                    break;
                case "BAJA":

                    break;
                case "SP":
                    Result = DtAll.Select("Filtro='ASIGNAC' AND (Descr='REPA' AND EstadBod='02' OR Descr=' - ')");
                    foreach (DataRow Row in Result)
                    { DTDstn.ImportRow(Row); }
                    break;
                case "MANTO":
                    Result = DtAll.Select("Filtro='ASIGNAC' AND (Descr='REPA' AND EstadBod='02' OR Descr=' - ')");
                    foreach (DataRow Row in Result)
                    { DTDstn.ImportRow(Row); }
                    break;
                default:// Disponible
                    Result = DtAll.Select("Filtro='ASIGNAC' AND (EstadBod='01' OR EstadBod='06' OR EstadBod='80' OR Descr=' - ')");
                    foreach (DataRow Row in Result)
                    { DTDstn.ImportRow(Row); }
                    break;
            }
            DdlBodDest.DataSource = DTDstn;
            DdlBodDest.DataTextField = "Descr";
            DdlBodDest.DataValueField = "Cod";
            DdlBodDest.DataBind();
        }
        protected void BIndDFilaColumDest(string CodUbOr, string CodTerc)
        {
            DtAll = (DataTable)ViewState["DtAll"];
            DataTable DtB = new DataTable();
            DtB = DtAll.Clone();
            GrdUbicaDes.DataSource = null; GrdUbicaDes.DataBind();
            string borrar = "Filtro='UBICA' AND Descr='" + DdlBodDest.Text.Trim() + "' AND Cod<>'" + CodUbOr.Trim() + "' AND CodTercero='" + CodTerc.Trim() + "'";
            DataRow[] Result = DtAll.Select("Filtro='UBICA' AND Descr='" + DdlBodDest.Text.Trim() + "' AND Cod<>'" + CodUbOr.Trim() + "' AND CodTercero='" + CodTerc.Trim() + "'");
            foreach (DataRow Row in Result)
            { DtB.ImportRow(Row); }
            GrdUbicaDes.DataSource = DtB; GrdUbicaDes.DataBind();
        }
        protected void IbtCerrarCambioBod_Click(object sender, ImageClickEventArgs e)
        { GrdUbicaDes.DataSource = null; GrdUbicaDes.DataBind(); MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString(); }
        protected void DdlBodDest_TextChanged(object sender, EventArgs e)
        { BIndDFilaColumDest(ViewState["CodUbicaBodega"].ToString().Trim(), ViewState["CodTercero"].ToString().Trim()); }
        protected void GrdUbicaDes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            double VbCantAct = Convert.ToDouble(TxtCantAct.Text);
            TxtCantNew.Text = TxtCantNew.Text.Equals("") ? "0" : TxtCantNew.Text.Trim();
            double VbCantNew = Convert.ToDouble(TxtCantNew.Text);
            if (VbCantNew > VbCantAct)
            {
                Result = Idioma.Select("Objeto= 'Mens01Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La cantidad a transferir supera la cantidad actual.
                return;
            }
            if (VbCantNew <= 0)
            {
                Result = Idioma.Select("Objeto= 'Mens02Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un cantidad válida.
                return;
            }
            string VbAplicaFV = "N";
            if (TxtFechI.Visible == true)
            {
                VbAplicaFV = "S";
                Cnx.ValidarFechas(TxtFechI.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
            }
            if (e.CommandName.Equals("Asignar"))
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                DateTime? VbFechaV;
                if (VbAplicaFV == "S") { VbFechaV = Convert.ToDateTime(TxtFechI.Text); }
                else { VbFechaV = null; }
                List<ClsTypAsignaciones> ObjAsignaciones = new List<ClsTypAsignaciones>();
                var TypAsignaciones = new ClsTypAsignaciones()
                {
                    CodIdUbicacion = Convert.ToInt32(ViewState["CodIdUbicacion"]),
                    CodUbicaBodegaOrg = ViewState["CodUbicaBodega"].ToString().Trim(),
                    CodUbicaBodegaDst = GrdUbicaDes.DataKeys[gvr.RowIndex].Values["Cod"].ToString(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    CodTipoElemento = ViewState["CodTipoElemento"].ToString().Trim(),
                    IdentificadorElem = ViewState["IdentificadorElem"].ToString().Trim(),
                    CodAlmacen = Convert.ToInt32(DdlAlmacen.Text),
                    CodBodegaOrg = TxtBodOrig.Text.Trim(),
                    CodBodegaDst = DdlBodDest.Text.Trim(),
                    Cantidad = VbCantNew,
                    AplicaFV = VbAplicaFV,
                    FechaVence = VbFechaV,
                    Usu = Session["C77U"].ToString(),
                    SP = ViewState["SPR"].ToString().Trim(),
                    Accion = "Assignation",
                };
                ObjAsignaciones.Add(TypAsignaciones);
                ClsTypAsignaciones Asignaciones = new ClsTypAsignaciones();
                Asignaciones.Alimentar(ObjAsignaciones);//
                string Mensj = Asignaciones.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                var VbNumSP = Asignaciones.GetNumSP();
                if (!VbNumSP.Trim().Equals("")) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbNumSP + "');", true); }
                BIndDatos();
                GrdUbicaDes.DataSource = null; GrdUbicaDes.DataBind();
                MultVw.ActiveViewIndex = 0;
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdUbicaDes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAsig = e.Row.FindControl("IbtAsigD") as ImageButton;
                if (IbtAsig != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdTrasf'");
                    foreach (DataRow RowIdioma in Result)
                    {
                        IbtAsig.ToolTip = RowIdioma["Texto"].ToString().Trim();
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { IbtAsig.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }

                }
            }
        }
        //********************** Cambio lote *************************************
        protected void IbtCerrarCambioLote_Click(object sender, ImageClickEventArgs e)
        { GrdUbicaDes.DataSource = null; GrdUbicaDes.DataBind(); MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString(); }
        protected void BtnTranLote_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            double VbCantAct = Convert.ToDouble(TxtCLCantOrg.Text);
            TxtCLNewCant.Text = TxtCLNewCant.Text.Equals("") ? "0" : TxtCLNewCant.Text.Trim();
            double VbCantNew = Convert.ToDouble(TxtCLNewCant.Text);
            if (VbCantNew > VbCantAct)
            {
                Result = Idioma.Select("Objeto= 'Mens01Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La cantidad a transferir supera la cantidad actual.
                TxtCLNewCant.Focus(); return;
            }
            if (VbCantNew <= 0)
            {
                Result = Idioma.Select("Objeto= 'Mens02Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un cantidad válida.
                TxtCLNewCant.Focus(); return;
            }
            if (TxtCLNewLot.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'Mens04Asg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un cantidad válida.
                TxtCLNewLot.Focus(); return;
            }
            if (TxtCLNewLot.Text.Trim().Equals(TxtCLNumLot.Text.Trim()))
            {
                Result = Idioma.Select("Objeto= 'Mens05Asg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//No es posible realizar el traslado al mismo lote.
                TxtCLNewLot.Focus(); return;
            }
            string VblViene = "", VblPasa = "";
            Result = Idioma.Select("Objeto= 'Mens02Asg'");
            foreach (DataRow row in Result)
            { VblViene = row["Texto"].ToString().Trim(); }//Viene del código:

            Result = Idioma.Select("Objeto= 'Mens03Asg'");
            foreach (DataRow row in Result)
            { VblPasa = row["Texto"].ToString().Trim(); }//Pasa al código:

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasGeneral 14, @NewL,@Terc, @Us, @Vien, @Pas, @Lot,'','','',@IdUcn, @ICC,@CntD, @IdAlm,0,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            string borr1 = ViewState["CodIdUbicacion"].ToString();
                            SC.Parameters.AddWithValue("@NewL", TxtCLNewLot.Text.Trim());
                            SC.Parameters.AddWithValue("@Terc", ViewState["CodTercero"].ToString().Trim());
                            SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString().Trim());
                            SC.Parameters.AddWithValue("@Vien", VblViene.Trim());
                            SC.Parameters.AddWithValue("@Pas", VblPasa.Trim());
                            SC.Parameters.AddWithValue("@Lot", GrdDatos.Columns[4].HeaderText);
                            SC.Parameters.AddWithValue("@IdUcn", ViewState["CodIdUbicacion"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.Parameters.AddWithValue("@CntD", VbCantNew);
                            SC.Parameters.AddWithValue("@IdAlm", DdlAlmacen.Text.Trim());

                            var Mensj1 = SC.ExecuteScalar();
                            if (!Mensj1.ToString().Trim().Equals(""))
                            {
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj1.ToString().Trim() + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Incon en la edicion

                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignación-Cambio-Lote", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                        }
                    }
                }
            }
            BIndDatos();
            MultVw.ActiveViewIndex = 0;
            Page.Title = ViewState["PageTit"].ToString();
        }
    }
}