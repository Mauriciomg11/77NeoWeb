using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmReserva : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DtRva = new DataTable();
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
                RdbBusqNumOT.Checked = true;
                MlVw.ActiveViewIndex = 0;
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
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//Puede modificar el usuario de la reserva                                     

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
                    BtnConsultar.Text = bO.Equals("IbtConsultar") ? bT : BtnConsultar.Text;
                    LblNumRva.Text = bO.Equals("LblNumRv") ? bT : LblNumRva.Text;
                    LblEstado.Text = bO.Equals("LblEstadoMst") ? bT : LblEstado.Text;
                    LblFechaRv.Text = bO.Equals("GrdFecRva") ? bT : LblFechaRv.Text;
                    LblMatr.Text = bO.Equals("LblAeronaveMstr") ? bT : LblMatr.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    BtnExprt.Text = bO.Equals("BtnExportMstr") ? bT : BtnExprt.Text;
                    BtnAlerta.Text = bO.Equals("BtnAlerta") ? bT : BtnAlerta.Text;
                    BtnAlerta.ToolTip = bO.Equals("BtnAlertaTT") ? bT : BtnAlerta.ToolTip;
                    IbtConsltPn.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsltPn.ToolTip;
                    // *************************************************Detalle Reserva *************************************************
                    LblTitDetRv.Text = bO.Equals("LblTitDetRv") ? bT : LblTitDetRv.Text;
                    GrdReserva.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdReserva.EmptyDataText;
                    GrdReserva.Columns[0].HeaderText = bO.Equals("GrdPos") ? bT : GrdReserva.Columns[0].HeaderText;
                    GrdReserva.Columns[1].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdReserva.Columns[1].HeaderText;
                    GrdReserva.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdReserva.Columns[3].HeaderText;
                    GrdReserva.Columns[4].HeaderText = bO.Equals("GrdCantSol") ? bT : GrdReserva.Columns[4].HeaderText;
                    GrdReserva.Columns[5].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdReserva.Columns[5].HeaderText;
                    GrdReserva.Columns[6].HeaderText = bO.Equals("GrdCantEntr") ? bT : GrdReserva.Columns[6].HeaderText;
                    // *************************************************Recibo usuario la reserva *************************************************
                    LblTitUsuario.Text = bO.Equals("TitRecibRv") ? bT : LblTitUsuario.Text;
                    GrdUsuario.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdUsuario.EmptyDataText;
                    GrdUsuario.Columns[0].HeaderText = bO.Equals("GrdRecibe") ? bT : GrdUsuario.Columns[0].HeaderText;
                    GrdUsuario.Columns[1].HeaderText = bO.Equals("GrdFecDsp") ? bT : GrdUsuario.Columns[1].HeaderText;
                    GrdUsuario.Columns[2].HeaderText = bO.Equals("GrdFecRcb") ? bT : GrdUsuario.Columns[2].HeaderText;
                    // *************************************************Stock *************************************************
                    LblTitStock.Text = bO.Equals("LblTitStock") ? bT : LblTitStock.Text;
                    GrdStok.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdStok.EmptyDataText;
                    GrdStok.Columns[0].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdStok.Columns[0].HeaderText;
                    GrdStok.Columns[3].HeaderText = bO.Equals("LoteMst") ? bT : GrdStok.Columns[3].HeaderText;
                    GrdStok.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdStok.Columns[4].HeaderText;
                    GrdStok.Columns[5].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdStok.Columns[5].HeaderText;
                    // *************************************************opcion de busqueda *************************************************
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtConsltPN.Attributes.Add("placeholder", bT); }
                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    RdbBusqNumOT.Text = bO.Equals("LblOTMstr") ? bT + ":" : RdbBusqNumOT.Text;
                    RdbBusqHK.Text = bO.Equals("LblAeronaveMstr") ? bT + ":" : RdbBusqHK.Text;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("LblOTMstr") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("GrdAplicab") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("GrdCodHk") ? bT : GrdBusq.Columns[5].HeaderText;
                    GrdBusq.Columns[6].HeaderText = bO.Equals("LblAeronaveMstr") ? bT : GrdBusq.Columns[6].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("GrdFecRv") ? bT : GrdBusq.Columns[7].HeaderText;
                    GrdBusq.Columns[8].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdBusq.Columns[8].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BtnExprt_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                if (!TxtNumRva.Text.Trim().Equals(""))
                {
                    string VbNomRpt = "Reserva Nueva";
                    CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                    CursorIdioma.Alimentar("CURRESERVA", Session["77IDM"].ToString().Trim());
                    string StSql = "EXEC SP_PANTALLA_Reserva 1,'','','WEB','CURRESERVA', @Rv,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    Idioma = (DataTable)ViewState["TablaIdioma"];
                    DataRow[] Result = Idioma.Select("Objeto= 'Caption'");
                    foreach (DataRow row in Result)
                    { VbNomRpt = row["Texto"].ToString().Trim(); }// 
                    Cnx.SelecBD();
                    using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                    {
                        using (SqlCommand SC = new SqlCommand(StSql, con))
                        {
                            SC.Parameters.AddWithValue("@Rv", TxtIdRva.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SC.CommandTimeout = 90000000;
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
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Reservas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }

        }
        //*************************************** Alerta ***************************************
        protected void BtnAlerta_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); Response.Redirect("~/Forms/Almacen/FrmAlertaReservaPenRevisar.aspx"); }
        //*************************************** BUSQUEDA ***************************************
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; TxtBusqueda.Focus(); }
        protected void BindDataStok(string Ref)
        {
            DS = (DataSet)ViewState["DS"];
            if (!TxtNumRva.Text.Trim().Equals(""))
            {
                if (DS.Tables["Stock"].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DS.Tables["Stock"].Clone();
                    DataRow[] DR = DS.Tables["Stock"].Select("CodReferencia LIKE '%" + Ref.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    if (DT.Rows.Count > 0) { GrdStok.DataSource = DT; }
                    else { GrdStok.DataSource = null; }
                }
                else { GrdStok.DataSource = null; }
            }
            else { GrdStok.DataSource = null; }
            GrdStok.DataBind();

        }
        protected void BIndDBusqOT()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "", VbOpcion = "OT";

                if (RdbBusqNumOT.Checked == true)
                { VbOpcion = "OT"; }
                if (RdbBusqSN.Checked == true)
                { VbOpcion = "SN"; }
                if (RdbBusqPN.Checked == true)
                { VbOpcion = "PN"; }
                if (RdbBusqHK.Checked == true)
                { VbOpcion = "HK"; }
                VbTxtSql = "EXEC SP_PANTALLA_CetificacionesControlCalidad 10,@Prmtr,'','',@Opc,0,0, @Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim()); ;// VbOpcion.Equals("OT") ? TxtOt.Text : TxtOTBusq.Text.Trim()
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                        else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void IbtConsltPn_Click(object sender, ImageClickEventArgs e)
        { BindDataPpal(TxtNumRva.Text.Equals("") ? "0" : TxtNumRva.Text, "SEL"); }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BIndDBusqOT(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string vbcod = GrdBusq.DataKeys[gvr.RowIndex].Values["CodNumOrdenTrab"].ToString().Trim();
                TxtIdRva.Text = vbcod;
                TxtNumRva.Text = ((Label)row.FindControl("LblOT")).Text.ToString().Trim();
                ViewState["CodHK"] = ((Label)row.FindControl("LblCodHk")).Text.ToString().Trim();
                TxtEstado.Text = ((Label)row.FindControl("LblEstado")).Text.ToString().Trim();
                TxtFechaRv.Text = ((Label)row.FindControl("LblFechOt")).Text.ToString().Trim();
                TxtMatr.Text = ((Label)row.FindControl("LblHk")).Text.ToString().Trim();
                TxtPnElem.Text = ((Label)row.FindControl("LblPnElem")).Text.ToString().Trim();
                TxtSnElem.Text = ((Label)row.FindControl("LblSnEle")).Text.ToString().Trim();

                BindDataPpal(vbcod, "UPD");
                MlVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIr = (e.Row.FindControl("IbtIr") as ImageButton);
                if (IbtIr != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //*************************************** Detalle Reserva ***************************************
        protected void BindDataPpal(string NumRv, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Reserva 6,'','','','',@NR,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@NR", NumRv);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DS = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DS);
                                DS.Tables[0].TableName = "DetRva";
                                DS.Tables[1].TableName = "Stock";
                                DS.Tables[2].TableName = "DetREcibo";
                                DS.Tables[3].TableName = "Usuario";
                                ViewState["DS"] = DS;
                            }
                        }
                    }
                }
            }
            DS = (DataSet)ViewState["DS"];
            DataRow[] DR;
            if (!TxtNumRva.Text.Trim().Equals(""))
            {
                if (DS.Tables["DetRva"].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DS.Tables["DetRva"].Clone();
                    DR = DS.Tables["DetRva"].Select("Pn LIKE '%" + TxtConsltPN.Text.Trim() + "%'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    if (DT.Rows.Count > 0)
                    {
                        DataView DV = DT.DefaultView;
                        DV.Sort = "NumeroPosicion";
                        DT = DV.ToTable();
                        GrdReserva.DataSource = DT;
                    }
                    else { GrdReserva.DataSource = null; }
                }
                else { GrdReserva.DataSource = null; }
            }
            else { GrdReserva.DataSource = null; }
            GrdReserva.DataBind();
        }
        protected void GrdReserva_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["VbRef"] = GrdReserva.DataKeys[this.GrdReserva.SelectedIndex][0].ToString();
            ViewState["VbPos"] = GrdReserva.DataKeys[this.GrdReserva.SelectedIndex][2].ToString();
            BindDataStok(ViewState["VbRef"].ToString());
            BindDataUsu(ViewState["VbPos"].ToString());

            foreach (GridViewRow Row in GrdReserva.Rows)
            {
                if (Row.RowIndex == GrdReserva.SelectedIndex)
                {
                    Row.Style["background-color"] = "#D4DAD3";
                    Row.Attributes["onclick"] = "";
                }
                else
                {
                    if (Row.RowIndex % 2 == 0)
                    { Row.Style["background-color"] = "white"; }
                    else
                    { Row.Style["background-color"] = "#cae4ff"; }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdReserva, "Select$" + Row.RowIndex);
                }
            }
        }
        protected void GrdReserva_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            { e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdReserva, "Select$" + e.Row.RowIndex); }
        }
        //*************************************** Usuarios ***************************************
        protected void BindDataUsu(string Pos)
        {
            DS = (DataSet)ViewState["DS"];
            if (!TxtNumRva.Text.Trim().Equals(""))
            {
                if (DS.Tables["DetREcibo"].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DS.Tables["DetREcibo"].Clone();
                    DataRow[] DR = DS.Tables["DetREcibo"].Select("Posicion ='" + Pos.Trim() + "'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    if (DT.Rows.Count > 0) { GrdUsuario.DataSource = DT; }
                    else { GrdUsuario.DataSource = null; }
                }
                else { GrdUsuario.DataSource = null; }
            }
            else { GrdUsuario.DataSource = null; }
            GrdUsuario.DataBind();
        }
        protected void GrdUsuario_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdUsuario.EditIndex = e.NewEditIndex; BindDataUsu(ViewState["VbPos"].ToString()); }
        protected void GrdUsuario_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbCodusu = (GrdUsuario.Rows[e.RowIndex].FindControl("DdlPersona") as DropDownList).Text.Trim();

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_Reserva 7, @Us,@Cu,'','',@ID,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Us", VbCodusu.Trim());
                        SC.Parameters.AddWithValue("@Cu", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ID", GrdUsuario.DataKeys[e.RowIndex].Value.ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdUsuario.EditIndex = -1;
                            BindDataPpal(TxtIdRva.Text.Trim(), "UPD");
                            BindDataUsu(ViewState["VbPos"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + "Editar Usuario Reserva", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdUsuario_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdUsuario.EditIndex = -1; BindDataUsu(ViewState["VbPos"].ToString()); }
        protected void GrdUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DS = (DataSet)ViewState["DS"];
                DropDownList DdlPersona = (e.Row.FindControl("DdlPersona") as DropDownList);
                DdlPersona.DataSource = DS.Tables[3];
                DdlPersona.DataTextField = "Usuario";
                DdlPersona.DataValueField = "CodUsuario";
                DdlPersona.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlPersona.SelectedValue = dr["CodPersona"].ToString();

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    if ((int)ViewState["VblCE1"] == 0) { imgE.Visible = false; }
                    else
                    {
                        imgE.Visible = true;
                        DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                        foreach (DataRow RowIdioma in Result)
                        { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    }
                }
            }
        }
    }
}