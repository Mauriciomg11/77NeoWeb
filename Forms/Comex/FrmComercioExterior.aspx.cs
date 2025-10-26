using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgLogistica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
        DataTable TblEncShipping = new DataTable();
        DataTable TblDetShipping = new DataTable();
        DataTable TblDetalle = new DataTable();
        DataTable TblAsignar = new DataTable();
        DataSet DSTPpl = new DataSet();
        DataSet DSTDdl = new DataSet();
        public string PMensj, PId, PCodSO, VbAccion;
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
                ViewState["CodSO"] = "";
                MultVw.ActiveViewIndex = 0;
                ModSeguridad();
                ViewState["CodTerceroAnt"] = "";
                ViewState["IdShippingOrder"] = "0";
                BindBDdl("UPD");
                AddCamposTblSO();
                AddCamposDataTable("INS");
                EnablGridDet("Visible", false);
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
        protected void AddCamposTblSO()
        {
            TblEncShipping.Columns.Add("IdShippingOrder", typeof(int));
            TblEncShipping.Columns.Add("CodShippingOrder", typeof(string));
            TblEncShipping.Columns.Add("Fecha", typeof(DateTime));
            TblEncShipping.Columns.Add("ShippedTo", typeof(string));
            TblEncShipping.Columns.Add("IdSucursalTo", typeof(int));
            TblEncShipping.Columns.Add("ShippedFrom", typeof(string));
            TblEncShipping.Columns.Add("IdSucursalFrom", typeof(int));
            TblEncShipping.Columns.Add("CodUndMedida", typeof(string));
            TblEncShipping.Columns.Add("NroGuia", typeof(string));
            TblEncShipping.Columns.Add("ShipVia", typeof(string));
            TblEncShipping.Columns.Add("Peso", typeof(double));
            TblEncShipping.Columns.Add("UsuCrea", typeof(string));
            TblEncShipping.Columns.Add("NroPaquete", typeof(int));
            TblEncShipping.Columns.Add("Observaciones", typeof(string));
            ViewState["TblEncShipping"] = TblEncShipping;

            TblDetShipping.Columns.Add("IdDetShippingOrder", typeof(int));
            TblDetShipping.Columns.Add("CodShippingOrder", typeof(string));
            TblDetShipping.Columns.Add("Posicion", typeof(int));
            TblDetShipping.Columns.Add("IddetOrdenCompra", typeof(double));
            TblDetShipping.Columns.Add("PN", typeof(string));
            TblDetShipping.Columns.Add("SN", typeof(string));
            TblDetShipping.Columns.Add("CodTipoElemento", typeof(string));
            TblDetShipping.Columns.Add("Recibido", typeof(int));
            TblDetShipping.Columns.Add("CantidadSO", typeof(double));
            TblDetShipping.Columns.Add("CompraME", typeof(double));
            TblDetShipping.Columns.Add("CompraML", typeof(double));
            TblDetShipping.Columns.Add("ValorUnitarioML", typeof(double));
            TblDetShipping.Columns.Add("FacturaOE", typeof(string));
            TblDetShipping.Columns.Add("TipoDocDSO", typeof(string));
            TblDetShipping.Columns.Add("DocNumDSO", typeof(string));/**/
            ViewState["TblDetShipping"] = TblDetShipping;
        }
        protected void AddCamposDataTable(string Accion)
        {
            if (Accion.Equals("INS"))// Nuevo los campos como se llaman en la grid
            {
                TblDetalle.Columns.Add("Vista", typeof(string));//0
                TblDetalle.Columns.Add("Posicion", typeof(int));//
                TblDetalle.Columns.Add("Documento", typeof(string));//2
                TblDetalle.Columns.Add("RazonSocial", typeof(string));//
                TblDetalle.Columns.Add("CodReferencia", typeof(string));//4
                TblDetalle.Columns.Add("PN", typeof(string));//
                TblDetalle.Columns.Add("SN", typeof(string));//6
                TblDetalle.Columns.Add("Descripcion", typeof(string));
                TblDetalle.Columns.Add("CodTipoElemento", typeof(string));//8
                TblDetalle.Columns.Add("Cantidad", typeof(double));//
                TblDetalle.Columns.Add("CantidadSO", typeof(double));//10
                TblDetalle.Columns.Add("CodUndMed", typeof(string));
                TblDetalle.Columns.Add("VlrUndDoc", typeof(double));//12
                TblDetalle.Columns.Add("ValorTtlReg", typeof(double));
                TblDetalle.Columns.Add("ValorUndML", typeof(double));//14
                TblDetalle.Columns.Add("FacturaOE", typeof(string));
                TblDetalle.Columns.Add("NumProceso", typeof(string));//16
                TblDetalle.Columns.Add("IdDetOrdenCompra", typeof(double));
                TblDetalle.Columns.Add("EstadoParcial", typeof(string));//18
                TblDetalle.Columns.Add("IdDetCotiza", typeof(int));
                TblDetalle.Columns.Add("IdShippin", typeof(int));//20
                TblDetalle.Columns.Add("CodCotizacion", typeof(string));
                TblDetalle.Columns.Add("CantParcial", typeof(int));//22
                TblDetalle.Columns.Add("TipoDocDSO", typeof(string));
                TblDetalle.Columns.Add("IdDetShippinInactivo", typeof(int));//24
                TblDetalle.Columns.Add("ValorUndMLText", typeof(string));//25
                TblDetalle.Columns.Add("ValorTtlMLText", typeof(string));//26

                ViewState["TblDetalle"] = TblDetalle;
            }
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

                    // *************************************************Grid detalle *************************************************


                    // *************************************************opcion de busqueda *************************************************
                    // RdbBqCompra.Text = bO.Equals("LblNumCotiza") ? "&nbsp" + bT : RdbBqCompra.Text;
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
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDet.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[13].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void EnablGridDet(string Propiedad, bool TF)
        {
            if (Propiedad.Equals("Visible"))
            { GrdDet.Visible = TF; }

            if (Propiedad.Equals("Enabled"))
            { GrdDet.Enabled = TF; }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr, string Accion)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            //BtnExportar.Enabled = Otr;           
            BtnConsultar.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            TxtGuia.Enabled = Edi;
            TxtPeso.Enabled = Edi;
            TxtNrPaq.Enabled = Edi;
            TxtObsrv.Enabled = Edi;
            RdbRepa.Enabled = Edi;
            RdbCompra.Enabled = Edi;
            RdbIntercambio.Enabled = Edi;
            RdbImportar.Enabled = Edi;
            RdbExporar.Enabled = Edi;
            EnablGridDet("Enabled", Edi);
        }
        protected void LimpiarCampos(string Accion)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            TxtNumDoc.Text = "";
            TxtFecha.Text = "";
            TxtGuia.Text = "";
            TxtPeso.Text = "";
            TxtNrPaq.Text = "";
            TxtObsrv.Text = "";
            RdbCompra.Checked = false;
            RdbIntercambio.Checked = false;
            RdbRepa.Checked = false;
            RdbImportar.Checked = false;
            RdbExporar.Checked = false;

            ViewState["IdSO"] = "0";
            TblDetalle.Clear();
            TblDetalle.AcceptChanges();
            BindDDetTmp();

        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtGuia.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe ingresar la Guia');", true);
                ViewState["Validar"] = "N"; TxtGuia.Focus(); return;
            }
            if (RdbCompra.Checked == false && RdbRepa.Checked == false && RdbIntercambio.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe seleccionar si es compra, reparación o intercambio');", true);
                ViewState["Validar"] = "N"; return;
            }
            if (RdbImportar.Checked == false && RdbExporar.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe seleccionar si es una importación o exportación');", true);
                ViewState["Validar"] = "N"; return;
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

                DSTPpl = (DataSet)ViewState["DSTPpl"];
                if (DSTPpl.Tables["EncSO"].Rows.Count > 0)
                {
                    string VbFecSt, S_Tipo, S_SV;
                    DateTime? VbFecDT;
                    ViewState["IdShippingOrder"] = DSTPpl.Tables[0].Rows[0]["IdShippingOrder"].ToString().Trim();
                    TxtNumDoc.Text = DSTPpl.Tables[0].Rows[0]["CodShippingOrder"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["Fecha"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["Fecha"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    S_SV = DSTPpl.Tables[0].Rows[0]["ShipVia"].ToString().Trim();
                    RdbCompra.Checked = S_SV.Equals("Orden de Compra") ? true : false;
                    RdbRepa.Checked = S_SV.Equals("Orden de Reparación") ? true : false;
                    RdbIntercambio.Checked = S_SV.Equals("Intercambio") ? true : false;
                    S_Tipo = DSTPpl.Tables[0].Rows[0]["Tipo"].ToString().Trim();
                    //DdlProv.Text = S_Tipo.Equals("Exportacion") ? DSTPpl.Tables[0].Rows[0]["Embarq_A"].ToString().Trim() : DSTPpl.Tables[0].Rows[0]["Embarq_De"].ToString().Trim();
                    RdbImportar.Checked = S_Tipo.Equals("Exportacion") ? false : true;
                    RdbExporar.Checked = S_Tipo.Equals("Exportacion") ? true : false;
                    TxtGuia.Text = DSTPpl.Tables[0].Rows[0]["NroGuia"].ToString().Trim();
                    TxtPeso.Text = DSTPpl.Tables[0].Rows[0]["Peso"].ToString().Trim();
                    TxtNrPaq.Text = DSTPpl.Tables[0].Rows[0]["NroPaquete"].ToString().Trim();
                    TxtObsrv.Text = DSTPpl.Tables[0].Rows[0]["Observaciones"].ToString().Trim();
                }
                if (DSTPpl.Tables["DetSO"].Rows.Count > 0)
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    DataRow[] DR = DSTPpl.Tables["DetSO"].Select("Vista <>''");
                    if (IsIENumerableLleno(DR))
                    { TblDetalle = DR.CopyToDataTable(); TblDetalle.AcceptChanges(); ViewState["TblDetalle"] = TblDetalle; }
                }
                else { TblDetalle.Clear(); TblDetalle.AcceptChanges(); AddCamposDataTable("UPD"); ; }
                if (TblDetalle.Rows.Count > 0) { }
                BindDDetTmp();
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
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                SqlDataReader SDR;
                TblEncShipping = (DataTable)ViewState["TblEncShipping"];
                TblDetShipping = (DataTable)ViewState["TblDetShipping"];
                DataRow[] Result;
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false, "INS");

                    ViewState["Accion"] = "Aceptar";
                    Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(true, true, "ING");
                    LimpiarCampos("INS");

                    string VbD = Convert.ToString(DateTime.UtcNow.Day);
                    string VbM = Convert.ToString(DateTime.UtcNow.Month);
                    string VbY = Convert.ToString(DateTime.UtcNow.Year);
                    string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, VbD);
                    DateTime VbFecID = Convert.ToDateTime(fecha);
                    TxtFecha.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);

                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }
                    string S_ShipVia = "";
                    if (RdbCompra.Checked == true) { S_ShipVia = "Orden de Compra"; }
                    if (RdbRepa.Checked == true) { S_ShipVia = "Orden de Reparación"; }
                    if (RdbIntercambio.Checked == true) { S_ShipVia = "Intercambio"; }

                    TblDetalle.AcceptChanges();
                    foreach (DataRow row in TblDetalle.Rows)
                    {
                        object value = row["PN"];
                        if (value == DBNull.Value)
                        {
                            if (TblDetalle.Rows.Count > 0) { row.Delete(); }
                        }
                    }
                    TblDetalle.AcceptChanges();
                    int IdShippingOrder = Convert.ToInt32(0);
                    string CodShippingOrder = "";
                    DateTime? Fecha = Convert.ToDateTime(TxtFecha.Text.Trim());
                    string ShippedTo = RdbExporar.Checked == true ? DdlProv.Text.Trim() : "";
                    int IdSucursalTo = Convert.ToInt32(0);
                    string ShippedFrom = RdbImportar.Checked == true ? DdlProv.Text.Trim() : "";
                    int IdSucursalFrom = Convert.ToInt32(0);
                    string CodUndMedida = "KG";
                    string NroGuia = TxtGuia.Text.Trim();
                    string ShipVia = S_ShipVia.Trim();
                    double Peso = Convert.ToDouble(TxtPeso.Text.Trim());
                    string UsuCrea = RdbImportar.Checked == true ? "Importacion" : "Exportacion";
                    int NroPaquete = Convert.ToInt32(TxtNrPaq.Text.Trim());
                    string Observaciones = TxtObsrv.Text.Trim();

                    TblEncShipping.Rows.Add(IdShippingOrder, CodShippingOrder, Fecha, ShippedTo, IdSucursalTo, ShippedFrom, IdSucursalFrom, CodUndMedida,
                        NroGuia, ShipVia, Peso, UsuCrea, NroPaquete, Observaciones);
                    string S_Fac = "";
                    foreach (GridViewRow Row in GrdDet.Rows)
                    {

                        S_Fac = (Row.FindControl("TxtFact") as TextBox).Text.Trim();
                        string S_NuDoc = (Row.FindControl("LblNumDoc") as Label).Text.Trim();
                        if (!S_Fac.Equals(""))
                        {
                            TblDetalle.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Documento"]) == S_NuDoc).ToList().ForEach(x => x.SetField("FacturaOE", S_Fac));
                        }
                    }
                    TblDetalle.AcceptChanges();
                    TblDetShipping.AcceptChanges();
                    int I_Pos = 0;
                    foreach (DataRow DR in TblDetalle.Rows)
                    {

                        int IdDetShippingOrder = 0;
                        I_Pos += 1;
                        double IddetOrdenCompra = Convert.ToDouble(DR["IdDetOrdenCompra"].ToString().Trim());
                        string SN = DR["SN"].ToString().Trim().ToUpper();
                        string PN = DR["PN"].ToString().Trim().ToUpper();
                        string CodTipoElemento = DR["CodTipoElemento"].ToString().Trim().ToUpper();
                        int Recibido = 0;
                        double CantidadSO = Convert.ToDouble(DR["CantidadSO"].ToString().Trim());
                        double CompraME = Convert.ToDouble(DR["VlrUndDoc"].ToString().Trim());
                        double CompraML = Convert.ToDouble(DR["ValorTtlReg"].ToString().Trim());
                        double ValorUnitarioML = Convert.ToDouble(DR["ValorUndML"].ToString().Trim());
                        S_Fac = DR["FacturaOE"].ToString().Trim().ToUpper(); ;
                        string TipoDocDSO = DR["TipoDocDSO"].ToString().Trim().ToUpper();
                        string DocNumDSO = DR["Documento"].ToString().Trim().ToUpper();/**/

                        TblDetShipping.Rows.Add(IdDetShippingOrder, CodShippingOrder, I_Pos, IddetOrdenCompra, PN, SN, CodTipoElemento, Recibido, CantidadSO, CompraME,
                            CompraML, ValorUnitarioML, S_Fac, TipoDocDSO, DocNumDSO);
                        TblDetShipping.AcceptChanges();
                    }
                    VbAccion = "INSERT";
                    Cnx.SelecBD();
                    using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                    {
                        SCX.Open();
                        using (SqlTransaction transaction = SCX.BeginTransaction())
                        {
                            try
                            {
                                string VBQuery = "INS_UPD_Shipping"; //
                                using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                                {
                                    PCodSO = "";
                                    PMensj = "";
                                    PId = "0";
                                    SC.CommandType = CommandType.StoredProcedure;
                                    SqlParameter Prmtrs = SC.Parameters.AddWithValue("@EncSO", TblEncShipping);
                                    SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetSO", TblDetShipping);
                                    SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdCia", HttpContext.Current.Session["!dC!@"].ToString());
                                    SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                                    SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                                    //SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                                    Prmtrs.SqlDbType = SqlDbType.Structured;
                                    SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        PCodSO = HttpUtility.HtmlDecode(SDR["CodSO"].ToString().Trim());
                                        PId = HttpUtility.HtmlDecode(SDR["Id"].ToString().Trim());
                                    }
                                    SDR.Close();
                                    transaction.Commit();
                                }
                            }
                            catch (Exception Ex)
                            {
                                string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                                VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                                VbPantalla = "Generar Shipping";
                                VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                                VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbUsu, VbPantalla, "INSERT/ UPDATE COMEX", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                                transaction.Rollback();
                                PMensj = "Inconveniente con la transacción: Por favor comunicarse con el administrador";
                            }
                            finally
                            {
                                if (SCX.State == ConnectionState.Open)
                                {
                                    SCX.Close();
                                }
                            }
                        }
                    }
                    if (!PMensj.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + PMensj + "');", true);
                        TblEncShipping.Rows.Clear();
                        TblDetShipping.Rows.Clear();
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, "INS");
                    ViewState["Accion"] = "";
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    ViewState["CodSO"] = PCodSO;
                    Traerdatos(ViewState["CodSO"].ToString().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                    TblEncShipping.Rows.Clear();
                    TblDetShipping.Rows.Clear();
                }
            }
            catch (Exception Ex)
            {
                TblEncShipping.Rows.Clear();
                TblDetShipping.Rows.Clear();
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR-MODIFICAR COMEX", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }

        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNumDoc.Text.Equals(""))
                { return; }
                SqlDataReader SDR;
                TblEncShipping = (DataTable)ViewState["TblEncShipping"];
                TblDetShipping = (DataTable)ViewState["TblDetShipping"];
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                int I_Pos = 0, I_TtlReg = 0;
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false, "UPD");
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPD");

                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                    EnablGridDet("Enabled", true);
                    I_Pos = DSTPpl.Tables["DetSO"].AsEnumerable().Max(x => x.Field<int>("Posicion"));

                    IEnumerable<DataRow> VbQry = from A in DSTPpl.Tables["DetSO"].AsEnumerable() select A;

                    DataTable DT = VbQry.CopyToDataTable();
                    if (Cnx.ValidaDataRowVacio(VbQry)) // Busca si tiene registros
                    {
                        DT = VbQry.CopyToDataTable();
                        I_TtlReg = Convert.ToInt32(DT.Rows[0]["TtlRegistro"].ToString().Trim());
                    }
                    if (I_TtlReg > 0)// si tiene registro no puede editasr ni tipo 
                    {
                        RdbRepa.Enabled = false;
                        RdbCompra.Enabled = false;
                        RdbIntercambio.Enabled = false;
                        RdbImportar.Enabled = false;
                        RdbExporar.Enabled = false;
                    }
                }
                else
                {
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }
                    string S_ShipVia = "";
                    if (RdbCompra.Checked == true) { S_ShipVia = "Orden de Compra"; }
                    if (RdbRepa.Checked == true) { S_ShipVia = "Orden de Reparación"; }
                    if (RdbIntercambio.Checked == true) { S_ShipVia = "Intercambio"; }

                    TblDetalle.AcceptChanges();
                    foreach (DataRow row in TblDetalle.Rows)
                    {
                        object value = row["PN"];
                        if (value == DBNull.Value) { if (TblDetalle.Rows.Count > 0) { row.Delete(); } }
                    }

                    TblDetalle.AcceptChanges();
                    int IdShippingOrder = Convert.ToInt32(ViewState["IdShippingOrder"].ToString());
                    string CodShippingOrder = TxtNumDoc.Text.Trim();
                    DateTime? Fecha = Convert.ToDateTime(TxtFecha.Text.Trim());
                    string ShippedTo = "";
                    int IdSucursalTo = Convert.ToInt32(0);
                    string ShippedFrom = "";
                    int IdSucursalFrom = Convert.ToInt32(0);
                    string CodUndMedida = "KG";
                    string NroGuia = TxtGuia.Text.Trim();
                    string ShipVia = S_ShipVia.Trim();
                    double Peso = Convert.ToDouble(TxtPeso.Text.Trim());
                    string UsuCrea = "";
                    int NroPaquete = Convert.ToInt32(TxtNrPaq.Text.Trim());
                    string Observaciones = TxtObsrv.Text.Trim();

                    TblEncShipping.Rows.Add(IdShippingOrder, CodShippingOrder, Fecha, ShippedTo, IdSucursalTo, ShippedFrom, IdSucursalFrom, CodUndMedida,
                        NroGuia, ShipVia, Peso, UsuCrea, NroPaquete, Observaciones);

                    string S_Fac = "", S_CodDocAnt = "";
                    int I_IdDet = 0;
                    foreach (GridViewRow Row in GrdDet.Rows)
                    {
                        S_Fac = (Row.FindControl("TxtFact") as TextBox).Text.Trim();
                        string S_NuDoc = (Row.FindControl("LblNumDoc") as Label).Text.Trim();

                        if (!S_Fac.Equals(""))
                        {
                            if (!S_CodDocAnt.Equals(S_NuDoc))
                            {
                                TblDetalle.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Documento"]) == S_NuDoc).ToList().ForEach(x => x.SetField("FacturaOE", S_Fac));
                            }
                        }
                        if (!S_CodDocAnt.Equals(S_NuDoc)) { S_CodDocAnt = S_NuDoc; }
                    }
                    TblDetalle.AcceptChanges();
                    TblDetShipping.AcceptChanges();
                    foreach (DataRow DR in TblDetalle.Rows)
                    {
                        if (I_Pos == 0)
                        {
                            I_Pos = TblDetalle.AsEnumerable().Max(x => x.Field<int>("Posicion"));
                        }
                        if (Convert.ToInt32(DR["IdShippin"].ToString().Trim()) <= 0) { I_IdDet += -1; }
                        if (Convert.ToInt32(DR["Posicion"].ToString().Trim()) <= 0) { I_Pos += 1; }

                        int IdDetShippingOrder = Convert.ToInt32(DR["IdShippin"].ToString().Trim()) > 0 ? Convert.ToInt32(DR["IdShippin"].ToString().Trim()) : I_IdDet;
                        int Posc = Convert.ToInt32(DR["Posicion"].ToString().Trim()) > 0 ? Convert.ToInt32(DR["Posicion"].ToString().Trim()) : I_Pos;
                        double IddetOrdenCompra = Convert.ToDouble(DR["IdDetOrdenCompra"].ToString().Trim());
                        string PN = DR["PN"].ToString().Trim().ToUpper();
                        string SN = DR["SN"].ToString().Trim().ToUpper();
                        string CodTipoElemento = DR["CodTipoElemento"].ToString().Trim().ToUpper();
                        int Recibido = 0;
                        double CantidadSO = Convert.ToDouble(DR["CantidadSO"].ToString().Trim());
                        double CompraME = Convert.ToDouble(DR["VlrUndDoc"].ToString().Trim());
                        double CompraML = Convert.ToDouble(DR["ValorTtlReg"].ToString().Trim());
                        double ValorUnitarioML = Convert.ToDouble(DR["ValorUndML"].ToString().Trim());
                        S_Fac = DR["FacturaOE"].ToString().Trim().ToUpper();
                        string TipoDocDSO = DR["TipoDocDSO"].ToString().Trim().ToUpper();
                        string DocNumDSO = DR["Documento"].ToString().Trim().ToUpper();/**/

                        TblDetShipping.Rows.Add(IdDetShippingOrder, CodShippingOrder, Posc, IddetOrdenCompra, PN, SN, CodTipoElemento, Recibido, CantidadSO, CompraME,
                            CompraML, ValorUnitarioML, S_Fac, TipoDocDSO, DocNumDSO);
                        TblDetShipping.AcceptChanges();
                    }
                    VbAccion = "UPDATE";
                    Cnx.SelecBD();
                    using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                    {
                        SCX.Open();
                        using (SqlTransaction transaction = SCX.BeginTransaction())
                        {
                            try
                            {
                                string VBQuery = "INS_UPD_Shipping"; //
                                using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                                {
                                    PCodSO = "";
                                    PMensj = "";
                                    PId = "0";
                                    SC.CommandType = CommandType.StoredProcedure;
                                    SqlParameter Prmtrs = SC.Parameters.AddWithValue("@EncSO", TblEncShipping);
                                    SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@DetSO", TblDetShipping);
                                    SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdCia", HttpContext.Current.Session["!dC!@"].ToString());
                                    SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                                    SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                                    //SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                                    Prmtrs.SqlDbType = SqlDbType.Structured;
                                    SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    {
                                        PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                        PCodSO = HttpUtility.HtmlDecode(SDR["CodSO"].ToString().Trim());
                                        PId = HttpUtility.HtmlDecode(SDR["Id"].ToString().Trim());
                                    }
                                    SDR.Close();
                                    transaction.Commit();
                                }
                            }
                            catch (Exception Ex)
                            {
                                TblEncShipping.Rows.Clear();
                                TblDetShipping.Rows.Clear();
                                string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                                VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                                VbPantalla = "Generar Shipping";
                                VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                                VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbUsu, VbPantalla, "INSERT/ UPDATE COMEX", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                                transaction.Rollback();
                                PMensj = "Inconveniente con la transacción: Por favor comunicarse con el administrador";
                            }
                            finally
                            {
                                if (SCX.State == ConnectionState.Open)
                                {
                                    SCX.Close();
                                }
                            }
                        }
                    }
                    if (!PMensj.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + PMensj + "');", true);
                        TblEncShipping.Rows.Clear();
                        TblDetShipping.Rows.Clear();
                        return;
                    }
                    ViewState["Accion"] = "";
                    ActivarBtn(true, true, true, true, true, "UPD");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampos(false, false, "UPD");
                    ViewState["CodSO"] = PCodSO;
                    Traerdatos(ViewState["CodSO"].ToString().Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                    TblEncShipping.Rows.Clear();
                    TblDetShipping.Rows.Clear();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Comex", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
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
                PerfilesGrid();
                //EnablGridDet("Visible", true);
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        //****************************** DETALLE Cotizacion **************************************
        protected void BindDDetTmp()
        {
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int VbNumReg = TblDetalle.Rows.Count;
            TblDetalle.AcceptChanges();
            foreach (DataRow row in TblDetalle.Rows)
            {
                object value = row["PN"];
                if (value == DBNull.Value)
                {
                    if (VbNumReg > 1) { row.Delete(); }
                }
            }
            TblDetalle.AcceptChanges();
            EnablGridDet("Visible", true);
            if (TblDetalle.Rows.Count > 0) { GrdDet.DataSource = TblDetalle; GrdDet.DataBind(); }
            else
            {
                TblDetalle.Rows.Add(TblDetalle.NewRow());
                GrdDet.DataSource = TblDetalle;
                GrdDet.DataBind();
                GrdDet.Rows[0].Cells.Clear();
                GrdDet.Rows[0].Cells.Add(new TableCell());
                GrdDet.Rows[0].Cells[0].Text = "Empty..!";
                GrdDet.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                TblDetalle.NewRow();
                GrdDet.DataSource = TblDetalle;
                GrdDet.DataBind();
            }
        }
        protected void GrdDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                if (RdbCompra.Checked == true && RdbExporar.Checked == true) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('La compra solo permite importar');", true); return; }
                if (RdbCompra.Checked == false && RdbRepa.Checked == false && RdbIntercambio.Checked == false) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe seleccionar si es compra, reparación o intercambio');", true); return; }
                if (RdbImportar.Checked == false && RdbExporar.Checked == false) { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe seleccionar si es una importación o exportación');", true); return; }
                RdbCompra.Enabled = false;
                RdbRepa.Enabled = false;
                RdbIntercambio.Enabled = false;
                RdbImportar.Enabled = false;
                RdbExporar.Enabled = false;
                MultVw.ActiveViewIndex = 3;
            }
        }
        protected void GrdDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (TxtNumDoc.Text.Trim().Equals("")) { return; }

            int index = Convert.ToInt32(e.RowIndex);
            TblDetalle.Rows[index].Delete();
            BindDDetTmp();
        }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        //****************************** Concpetos **************************************
        protected void IbtCerrarCnptos_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        //****************************** View Asignar Documentos **************************************
        protected void IbtAprDetAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            TblAsignar = (DataTable)ViewState["TblAsignar"];
            if (TblAsignar.Rows.Count > 0)
            {
                foreach (DataRow Dtll in TblAsignar.Rows)
                { Dtll["CK"] = "1"; }
                GrdAsignarComRep.DataSource = TblAsignar; GrdAsignarComRep.DataBind();
            }
        }
        protected void DdlProv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string S_TipoDoc = "";
                ViewState["Tipo"] = "";
                if (RdbCompra.Checked == true) { S_TipoDoc = "C"; }
                if (RdbRepa.Checked == true) { S_TipoDoc = "R"; }
                if (RdbIntercambio.Checked == true) { S_TipoDoc = "I"; }
                if (RdbImportar.Checked == true) { ViewState["Tipo"] = "Importacion"; }
                if (RdbExporar.Checked == true) { ViewState["Tipo"] = "Exportacion"; }
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Comex 3,@CodProv,@TipoDoc,@Tipo,'','','',0,0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@CodProv", DdlProv.Text.Trim());
                        SC.Parameters.AddWithValue("@TipoDoc", S_TipoDoc.Trim());
                        SC.Parameters.AddWithValue("@Tipo", ViewState["Tipo"].ToString().Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(TblAsignar);
                            if (TblAsignar.Rows.Count > 0) { GrdAsignarComRep.DataSource = TblAsignar; GrdAsignarComRep.DataBind(); }
                            else { GrdAsignarComRep.DataSource = null; GrdAsignarComRep.DataBind(); }
                        }
                        ViewState["TblAsignar"] = TblAsignar;
                    }
                }
            }
            catch (Exception Ex) { string Borrar = Ex.Message; }
        }
        protected void BtnAsignar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            try
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                TblDetalle.AcceptChanges();
                foreach (GridViewRow Row in GrdAsignarComRep.Rows)
                {
                    if ((Row.FindControl("CkbA") as CheckBox).Checked == true)
                    {
                        string S_Vist = "";
                        int I_Pos = 0;
                        string S_Doc = (Row.FindControl("LblDoc") as Label).Text.Trim();
                        string S_RSc = DdlProv.SelectedItem.Text.Trim();
                        string S_CRf = GrdAsignarComRep.DataKeys[Row.RowIndex].Values["CodReferencia"].ToString().Trim();
                        string S_Pn = (Row.FindControl("LblPn") as Label).Text.Trim();
                        string S_Sn = GrdAsignarComRep.DataKeys[Row.RowIndex].Values["SN"].ToString().Trim();
                        string S_Dsc = (Row.FindControl("LblDesc") as Label).Text.Trim();
                        string S_CTM = GrdAsignarComRep.DataKeys[Row.RowIndex].Values["CodTipoElem"].ToString().Trim();
                        double D_Cnt = Convert.ToDouble((Row.FindControl("LblCantDoc") as Label).Text.Trim());
                        double D_CSO = Convert.ToDouble((Row.FindControl("TxtCantPend") as TextBox).Text.Trim());
                        string S_UM = (Row.FindControl("LblUndM") as Label).Text.Trim();
                        double D_Und = Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["ValorUnidad"].ToString().Trim());
                        double D_Ttl = D_CSO * (D_Und * Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["TRM"].ToString().Trim()));
                        double D_VML = Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["ValorUnidad"].ToString().Trim()) * Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["TRM"].ToString().Trim());
                        string S_Fac = "";
                        string S_NPr = "";
                        double D_IdDC = Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["IdDetOrdencompra"].ToString().Trim());
                        string S_EPc = "";
                        int I_IdCt = Convert.ToInt32(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["IdDetCotiza"].ToString().Trim());
                        string S_Cct = GrdAsignarComRep.DataKeys[Row.RowIndex].Values["CodCotizacion"].ToString().Trim();
                        string S_Tdc = GrdAsignarComRep.DataKeys[Row.RowIndex].Values["TipoDocSO"].ToString().Trim();
                        string S_VML = D_VML.ToString("#,##0.00", CultureInfo.CurrentCulture);
                        string S_VTML = D_Ttl.ToString("#,##0.00", CultureInfo.CurrentCulture);
                        double D_CPF = Convert.ToDouble(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["CantPendF"].ToString().Trim());
                        int I_TtlDoc = Convert.ToInt32(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["TtlDoc"].ToString().Trim());
                        int I_Aprov = Convert.ToInt32(GrdAsignarComRep.DataKeys[Row.RowIndex].Values["Aprobado"].ToString().Trim());

                        if (S_Tdc == "R" && RdbImportar.Checked == true && I_TtlDoc == 0)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('P/N: [" + S_Pn + "] Debe realizar primero la exportación');", true); return; }
                        if (D_CSO <= 0)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('P/N: [" + S_Pn + "] La cantidad debe ser superior a 0');", true); return; }
                        if (D_CSO > D_CPF)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('P/N: [" + S_Pn + "] La cantidad supera la pendiente por registrar');", true); return; }
                        if (I_Aprov == 0)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('P/N: [" + S_Pn + "] El documento no se encuentra aprobado');", true); return; }

                        DataRow dr = TblDetalle.Select("IdDetOrdenCompra = " + Convert.ToString(D_IdDC)).FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                        if (dr == null)
                        {
                            TblDetalle.Rows.Add(S_Vist, I_Pos, S_Doc, S_RSc, S_CRf, S_Pn, S_Sn, S_Dsc, S_CTM, D_Cnt, D_CSO, S_UM, D_Und, D_Ttl, D_VML, S_Fac, S_NPr,
                                                D_IdDC, S_EPc, I_IdCt, 0, S_Cct, D_Cnt, S_Tdc, 0, S_VML, S_VTML);
                        }
                    }
                }
                TblDetalle.AcceptChanges();
                MultVw.ActiveViewIndex = 0;
                BindDDetTmp();
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar COMEX", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtCerrarAsignar_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void GrdAsignarComRep_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}