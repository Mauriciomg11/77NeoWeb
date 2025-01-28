using _77NeoWeb.prg;
using _77NeoWeb.Prg;
using _77NeoWeb.Prg.PrgLogistica;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmSolicitudPedido : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable TblDetalle = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSTPpl = new DataSet();
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
                }
            }
            if (!IsPostBack)
            {
                ViewState["Accion"] = "";
                ViewState["IdPedido"] = "0";
                ViewState["CodPriordAnt"] = "";
                ViewState["TipoAnt"] = "";
                ViewState["CodCCostoAnt"] = "";
                ViewState["PersonaAnt"] = "";
                ViewState["CodEstadoPn"] = "00";// para guardar el estado cuando es el ingreso por ser una tabla temporal
                ViewState["TtlRegDet"] = 0; // saber si el detalle tiene registro para realizar carga masiva
                ViewState["CarpetaCargaMasiva"] = "";// para mostrar en el boton de carga masiva la ruta por defecto donde se debe guardar el archivo para subir
                ModSeguridad();
                BindBDdl("UPD");
                AddCamposDataTable("INS");
                GrdDetSP.Visible = false;
                MultVw.ActiveViewIndex = 0;
                RdbBusqNumSlPd.Checked = true;
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
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; BtnCargaMaxiva.Visible = false; GrdDetSP.ShowFooter = false; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // Cambio de Tipo
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; LblPpt.Visible = false; DdlPpt.Visible = false; }// Asigar Propueta
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//Modificar Centro de costo
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                ViewState["CCostoDefault"] = "S";//Ccosto Stock ALmacen por defecto
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,2,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["!dC!@"].ToString());
                SC.Parameters.AddWithValue("@F", "CENTROCOSTO");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 2 && VbAplica.Equals("N")) //Ccosto Stock ALmacen por defecto
                    { ViewState["CCostoDefault"] = "N"; }
                }
            }

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
                    // BtnExportar.Text = bO.Equals("BtnExportMstr") ? bT : BtnExportar.Text;
                    BtnAlert.Text = bO.Equals("BtnAlert") ? bT : BtnAlert.Text;
                    BtnAlert.ToolTip = bO.Equals("BtnAlertTT") ? bT : BtnAlert.ToolTip;
                    BtnOpenCotiza.Text = bO.Equals("BtnOpenCotiza") ? bT : BtnOpenCotiza.Text;
                    BtnOpenCotiza.ToolTip = bO.Equals("BtnOpenCotizaTT") ? bT : BtnOpenCotiza.ToolTip;
                    LblCotiza.Text = bO.Equals("BtnOpenCotiza") ? bT : LblCotiza.Text;
                    //**************************************** Modal Asignar PN ****************************************
                    RdbMOdalBusqDesc.Text = bO.Equals("Descripcion") ? bT : RdbMOdalBusqDesc.Text;
                    CkbIngrPNNuevo.Text = bO.Equals("CkbIngrPNNuevo") ? "&nbsp" + bT : CkbIngrPNNuevo.Text;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT : LblModalBusq.Text;
                    IbtModalBusq.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtModalBusq.ToolTip;
                    BtnCloseModalBusqPN.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqPN.Text;
                    GrdModalBusqPN.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqPN.EmptyDataText;
                    GrdModalBusqPN.Columns[0].HeaderText = bO.Equals("GrdSelecc") ? bT : GrdModalBusqPN.Columns[0].HeaderText;
                    GrdModalBusqPN.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdModalBusqPN.Columns[2].HeaderText;
                    GrdModalBusqPN.Columns[3].HeaderText = bO.Equals("GrdStock") ? bT : GrdModalBusqPN.Columns[3].HeaderText;
                    GrdModalBusqPN.Columns[4].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdModalBusqPN.Columns[4].HeaderText;
                    //*************************************************Campos *************************************************
                    LblCodPedd.Text = bO.Equals("RdbBusqNumSlPd") ? bT : LblCodPedd.Text;
                    LblFech.Text = bO.Equals("LblFechMstr") ? bT : LblFech.Text;
                    LblPriord.Text = bO.Equals("LblPriord") ? bT : LblPriord.Text;
                    LblTipo.Text = bO.Equals("TipoMstr") ? bT : LblTipo.Text;
                    LblPpt.Text = bO.Equals("LblPpt") ? bT : LblPpt.Text;
                    LblEstd.Text = bO.Equals("LblEstadoMst") ? bT : LblEstd.Text;
                    LblCcosto.Text = bO.Equals("LblCcosto") ? bT : LblCcosto.Text;
                    LblRespsbl.Text = bO.Equals("LblRespsbl") ? bT : LblRespsbl.Text;
                    LblObsrvcn.Text = bO.Equals("LblObsMst") ? bT : LblObsrvcn.Text;
                    // *************************************************Grid detalle *************************************************
                    GrdDetSP.Columns[0].HeaderText = bO.Equals("GrdPos") ? bT : GrdDetSP.Columns[0].HeaderText;
                    GrdDetSP.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdDetSP.Columns[2].HeaderText;
                    GrdDetSP.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDetSP.Columns[3].HeaderText;
                    GrdDetSP.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdDetSP.Columns[4].HeaderText;
                    GrdDetSP.Columns[5].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDetSP.Columns[5].HeaderText;
                    GrdDetSP.Columns[7].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdDetSP.Columns[7].HeaderText;
                    GrdDetSP.Columns[8].HeaderText = bO.Equals("LblAeronaveMstr") ? bT : GrdDetSP.Columns[8].HeaderText;
                    // *************************************************opcion de busqueda *************************************************
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtModalBusq.Attributes.Add("placeholder", bT); }
                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    RdbBusqNumSlPd.Text = bO.Equals("RdbBusqNumSlPd") ? "&nbsp" + bT : RdbBusqNumSlPd.Text;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("RdbBusqNumSlPd") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[4].HeaderText = bO.Equals("GrdFechP") ? bT : GrdBusq.Columns[4].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdBusq.Columns[5].HeaderText;
                    //************************************** Carga Masiva *****************************************************
                    BtnCargaMaxiva.Text = bO.Equals("BtnCargaMaxivaMstr") ? bT : BtnCargaMaxiva.Text;
                    BtnCargaMaxiva.ToolTip = bO.Equals("BtnCargaMasivaTT1") ? bT : BtnCargaMaxiva.ToolTip;
                    LblTitOTCargMasiv.Text = tbl["Objeto"].ToString().Trim().Equals("LblTitCargMasiv") ? tbl["Texto"].ToString().Trim() : LblTitOTCargMasiv.Text;
                    LblCargaMasvNumPed.Text = tbl["Objeto"].ToString().Trim().Equals("RdbBusqNumSlPd") ? tbl["Texto"].ToString().Trim() : LblCargaMasvNumPed.Text;
                    IbtCerrarSubMaxivo.ToolTip = tbl["Objeto"].ToString().Trim().Equals("CerrarVentana") ? tbl["Texto"].ToString().Trim() : IbtCerrarSubMaxivo.ToolTip;
                    IbtSubirCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtSubirCargaMax") ? tbl["Texto"].ToString().Trim() : IbtSubirCargaMax.ToolTip;
                    IbtGuardarCargaMax.ToolTip = tbl["Objeto"].ToString().Trim().Equals("IbtGuardarCargaMax") ? tbl["Texto"].ToString().Trim() : IbtGuardarCargaMax.ToolTip;
                    GrdCargaMax.Columns[0].HeaderText = tbl["Objeto"].ToString().Trim().Equals("PosMstr") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[0].HeaderText;
                    GrdCargaMax.Columns[2].HeaderText = tbl["Objeto"].ToString().Trim().Equals("Descripcion") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[2].HeaderText;
                    GrdCargaMax.Columns[3].HeaderText = tbl["Objeto"].ToString().Trim().Equals("CantMst") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[3].HeaderText;
                    GrdCargaMax.Columns[4].HeaderText = tbl["Objeto"].ToString().Trim().Equals("GrdUndMstr") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[4].HeaderText;
                    GrdCargaMax.Columns[5].HeaderText = tbl["Objeto"].ToString().Trim().Equals("UndSistemMstr") ? tbl["Texto"].ToString().Trim() : GrdCargaMax.Columns[5].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result) { BtnEliminar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                Result = Idioma.Select("Objeto= 'GuardarCargaMaxClientClick'");
                foreach (DataRow row in Result)
                { IbtGuardarCargaMax.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDetSP.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[9].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[9].Controls.Remove(imgD);
                    }
                }
            }
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void AddCamposDataTable(string Accion)
        {
            if (Accion.Equals("INS"))// Nuevo
            {
                TblDetalle.Columns.Add("Posicion", typeof(int));//0
                TblDetalle.Columns.Add("PN", typeof(string));
                TblDetalle.Columns.Add("Desc_PN", typeof(string));//2
                TblDetalle.Columns.Add("Descripcion", typeof(string));
                TblDetalle.Columns.Add("CodReferencia", typeof(string));//4
                TblDetalle.Columns.Add("CantidadTotal", typeof(double));
                TblDetalle.Columns.Add("CantidadReparacion", typeof(int));//6
                TblDetalle.Columns.Add("CodUndMedida", typeof(string));
                TblDetalle.Columns.Add("Notas", typeof(string));//8
                TblDetalle.Columns.Add("CodSeguimiento", typeof(string));
                TblDetalle.Columns.Add("Matricula", typeof(string));//10
                TblDetalle.Columns.Add("CodEstadoPn", typeof(string));
                TblDetalle.Columns.Add("IdDetPedido", typeof(int));//12
                TblDetalle.Columns.Add("RefPN", typeof(string));
                TblDetalle.Columns.Add("IdPedido", typeof(int));//14

                ViewState["TblDetalle"] = TblDetalle;
                //TblDetalle.Rows.Add(TblDetalle.NewRow());
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_SolicitudPedido 18,'','','','DDL',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
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
                                DSTDdl.Tables[0].TableName = "Priord";
                                DSTDdl.Tables[1].TableName = "Tipo";
                                DSTDdl.Tables[2].TableName = "PPT";
                                DSTDdl.Tables[3].TableName = "Estado";
                                DSTDdl.Tables[4].TableName = "CCosto";
                                DSTDdl.Tables[5].TableName = "Persona";
                                DSTDdl.Tables[6].TableName = "ConsltPN";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            string VbCodAnt = "";

            if (DSTDdl.Tables["Priord"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[0].Select("Activo=1 OR CodPrioridadSolicitudMat= '" + ViewState["CodPriordAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlPriord.DataSource = DT;
                DdlPriord.DataTextField = "Descripcion";
                DdlPriord.DataValueField = "CodPrioridadSolicitudMat";
                DdlPriord.DataBind();
                DdlPriord.SelectedValue = ViewState["CodPriordAnt"].ToString().Trim();
            }

            if (DSTDdl.Tables["Tipo"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                if (ViewState["TipoAnt"].ToString().Trim().Equals("02"))// Repa
                {
                    DR = DSTDdl.Tables[1].Select("CodTipoSolPed IN ('02','03')");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                else
                {
                    DR = DSTDdl.Tables[1].Select("CodTipoSolPed IN ('','01','03')");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                DdlTipo.DataSource = DT;
                DdlTipo.DataTextField = "Descripcion";
                DdlTipo.DataValueField = "CodTipoSolPed";
                DdlTipo.DataBind();
                DdlTipo.SelectedValue = ViewState["TipoAnt"].ToString().Trim();
            }

            if (DSTDdl.Tables["CCosto"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                if (ViewState["CCostoDefault"].ToString().Trim().Equals("S"))// Centro de costo por defecto stock
                {
                    DR = DSTDdl.Tables[4].Select("StockAlmacen = 1  OR CodCc= '" + ViewState["CodCCostoAnt"] + "'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                else
                {
                    DR = DSTDdl.Tables[4].Select("Activo = 1 OR CodCc= '" + ViewState["CodCCostoAnt"] + "'");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                }
                DdlCcosto.DataSource = DT;
                DdlCcosto.DataTextField = "Nombre";
                DdlCcosto.DataValueField = "CodCc";
                DdlCcosto.DataBind();
                DdlCcosto.SelectedValue = ViewState["CodCCostoAnt"].ToString().Trim();
            }

            if (DSTDdl.Tables["Persona"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[5].Select("Estado='ACTIVO' OR CodPersona= '" + ViewState["PersonaAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlRespsbl.DataSource = DT;
                DdlRespsbl.DataTextField = "Persona";
                DdlRespsbl.DataValueField = "CodPersona";
                DdlRespsbl.DataBind();
                DdlRespsbl.SelectedValue = ViewState["PersonaAnt"].ToString().Trim();
            }

            VbCodAnt = DdlPpt.Text.Trim();
            DdlPpt.DataSource = DSTDdl.Tables[2];
            DdlPpt.DataTextField = "CodigoPPT";
            DdlPpt.DataValueField = "IdPropuesta";
            DdlPpt.DataBind();
            DdlPpt.Text = VbCodAnt;

            VbCodAnt = DdlEstd.Text.Trim();
            DdlEstd.DataSource = DSTDdl.Tables[3];
            DdlEstd.DataTextField = "Descripcion";
            DdlEstd.DataValueField = "CodEstPedido";
            DdlEstd.DataBind();
            DdlEstd.Text = VbCodAnt.Trim().Equals("") ? "A" : VbCodAnt;
        }
        protected void Traerdatos(string IdPedido, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_PANTALLA_SolicitudPedido 18,'','','','Ppal',@Id,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Id", IdPedido.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "SolPed";
                                    DSTPpl.Tables[1].TableName = "DetSolPed";

                                    ViewState["DSTPpl"] = DSTPpl;
                                }
                            }
                        }
                    }
                }
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                if (DSTPpl.Tables["SolPed"].Rows.Count > 0)
                {
                    string VbFecSt;
                    DateTime? VbFecDT;
                    TxtCodPedd.Text = DSTPpl.Tables[0].Rows[0]["CodPedido"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["Fechapedido"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["Fechapedido"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFech.Text = String.Format("{0:yyyy-MM-dd}", VbFecDT);
                    ViewState["CodPriordAnt"] = DSTPpl.Tables[0].Rows[0]["CodPrioridad"].ToString().Trim();
                    ViewState["TipoAnt"] = DSTPpl.Tables[0].Rows[0]["CodtipoSolPedido"].ToString().Trim();
                    DdlPpt.Text = DSTPpl.Tables[0].Rows[0]["IdPropuesta"].ToString().Trim();
                    DdlEstd.Text = DSTPpl.Tables[0].Rows[0]["CodEstado"].ToString().Trim();
                    ViewState["CodCCostoAnt"] = DSTPpl.Tables[0].Rows[0]["Ccostos"].ToString().Trim();
                    ViewState["PersonaAnt"] = DSTPpl.Tables[0].Rows[0]["CodResponsable"].ToString().Trim();
                    TxtObsrvcn.Text = DSTPpl.Tables[0].Rows[0]["Obsevacion"].ToString().Trim();
                    ViewState["TieneCotiza"] = DSTPpl.Tables[0].Rows[0]["TieneCotiza"].ToString().Trim();
                    ViewState["TtlRegDet"] = Convert.ToInt32(DSTPpl.Tables[0].Rows[0]["TtlRegDet"].ToString());
                    TxtCotiza.Text = DSTPpl.Tables[0].Rows[0]["CodCotizacion"].ToString().Trim();
                    ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(DSTPpl.Tables[0].Rows[0]["CargaMasiva"].ToString().Trim());
                    if (ViewState["TipoAnt"].ToString().Trim().Equals("01") && (int)ViewState["VblIngMS"] == 1) { BtnCargaMaxiva.Visible = true; }// Solo se puede cargar masivamente compras
                    else { BtnCargaMaxiva.Visible = false; }
                    DataRow[] Result = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                    foreach (DataRow row in Result)
                    { BtnCargaMaxiva.ToolTip = row["Texto"].ToString(); }
                    BindBDdl("SEL");
                }
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
        protected void LimpiarCampos(string Accion)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            TxtCodPedd.Text = "";
            TxtCotiza.Text = "";
            TxtFech.Text = "";
            DdlPriord.Text = "";
            DdlTipo.Text = "";
            DdlPpt.Text = "0";
            DdlEstd.Text = "A";
            TxtObsrvcn.Text = "";
            if (ViewState["CCostoDefault"].ToString().Trim().Equals("N")) { DdlCcosto.Text = ""; }// Ccosto Defecto           
            DdlRespsbl.Text = "";
            TblDetalle.Clear();
            TblDetalle.AcceptChanges();
            BindDDetTmp();
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (DdlPriord.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01SlPd'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la prioridad.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02SlPd'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el tipo.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlEstd.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03SlPd'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un estado.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlCcosto.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04SlPd'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un centro de costo.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlRespsbl.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05SlPd'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un responsable.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            // BtnExportar.Enabled = Otr;
            BtnAlert.Enabled = Otr;
            BtnConsultar.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DdlTipo.ToolTip = "";
            DdlPriord.Enabled = Edi;
            //if (!ViewState["TipoAnt"].ToString().Trim().Equals("02"))// Repa
            //{ DdlTipo.Enabled = Edi; }
            DdlTipo.Enabled = Edi;
            if (Accion.Equals("UPD"))
            {
                if (ViewState["TieneCotiza"].ToString().Equals("S")) { DdlTipo.Enabled = false; }
                DataRow[] Result = Idioma.Select("Objeto= 'DdlTipoTT'");
                foreach (DataRow row in Result)
                { DdlTipo.ToolTip = row["Texto"].ToString().Trim(); }//Tiene cotización.
            }

            if (ViewState["CCostoDefault"].ToString().Trim().Equals("N")) { DdlCcosto.Enabled = Edi; }// Ccosto Defecto
            else
            {
                if (DdlCcosto.Text.Trim().Equals(""))
                {
                    DSTDdl = (DataSet)ViewState["DSTDdl"];
                    DataTable DT = new DataTable();
                    DataRow[] DR = DSTDdl.Tables[4].Select("StockAlmacen = 1");
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    DdlCcosto.DataSource = DT;
                    DdlCcosto.DataTextField = "Nombre";
                    DdlCcosto.DataValueField = "CodCc";
                    DdlCcosto.DataBind();
                } // Ccosto Defecto
            }
            if (ViewState["Accion"].ToString().Equals("")) { DdlPpt.Enabled = false; }
            if (Convert.ToInt32(DdlPpt.Text.Trim()) == 0) { DdlPpt.Enabled = Edi; }
            DdlRespsbl.Enabled = Edi;
            TxtObsrvcn.Enabled = Edi;
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false);

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(true, true, "ING");
                    LimpiarCampos("INS");

                    string VbD = Convert.ToString(DateTime.UtcNow.Day);
                    string VbM = Convert.ToString(DateTime.UtcNow.Month);
                    string VbY = Convert.ToString(DateTime.UtcNow.Year);
                    string fecha = string.Format("{0}-{1}-{2}", VbY, VbM, VbD);
                    DateTime VbFecID = Convert.ToDateTime(fecha);
                    TxtFech.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

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
                    List<ClsTypSolicitudPedido> ObjEncSolPed = new List<ClsTypSolicitudPedido>();
                    var TypEncSolPed = new ClsTypSolicitudPedido()
                    {
                        IdPedido = 0,
                        CodPedido = "",
                        Fechapedido = Convert.ToDateTime(TxtFech.Text),
                        CodPrioridad = DdlPriord.Text.Trim(),
                        CodResponsable = DdlRespsbl.Text.Trim(),
                        CodReserva = Convert.ToInt32(DdlPpt.Text.Trim()),
                        CodEstado = DdlEstd.Text.Trim(),
                        Obsevacion = TxtObsrvcn.Text.Trim(),
                        CodtipoSolPedido = DdlTipo.Text.Trim(),
                        Ccostos = DdlCcosto.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        CodTipoCodigo = Session["CodTipoCodigoInicial"].ToString(),
                        FechaRemocionSP = null,
                        Aplicabilidad = "SolPed",
                        Accion = "INSERT",
                    };
                    ObjEncSolPed.Add(TypEncSolPed);

                    List<ClsTypSolicitudPedido> ObjDetSP = new List<ClsTypSolicitudPedido>();
                    foreach (DataRow Row in TblDetalle.Rows)
                    {
                        var TypDetSP = new ClsTypSolicitudPedido()
                        {
                            IdDetPedido = 0,
                            CodReferencia = Row["CodReferencia"].ToString(),
                            PN = Row["PN"].ToString(),
                            CodUndMedida = Row["CodUndMedida"].ToString(),
                            CantidadTotal = Convert.ToDouble(Row["CantidadTotal"].ToString()),
                            CantidadAlmacen = Convert.ToDouble(Row["CantidadTotal"].ToString()),
                            CantidadReparacion = 0,
                            CantidadOrden = Convert.ToDouble(Row["CantidadTotal"].ToString()),
                            Posicion = 0,
                            AprobacionDetalle = 0,
                            CodSeguimiento = "SOL",
                            Descripcion = Row["Descripcion"].ToString(),
                            TipoPedido = 1,
                            CantidadAjustada = Convert.ToDouble(Row["CantidadTotal"].ToString()),
                            Notas = "",
                            PosicionPr = 0,
                            IdSrvPr = 0,
                            IdReporte = 0,
                            IdDetProPSrvSP = 0,
                            CodIdDetalleResSP = 0,
                            FechaAprob = null,
                            CodAeronaveSP = 0,
                        };
                        ObjDetSP.Add(TypDetSP);
                    }

                    ClsTypSolicitudPedido ClsSolPed = new ClsTypSolicitudPedido();
                    ClsSolPed.Alimentar(ObjEncSolPed, ObjDetSP);
                    string Mensj = ClsSolPed.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    ViewState["IdPedido"] = ClsSolPed.GetIdSolPed();
                    Traerdatos(ViewState["IdPedido"].ToString().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    GrdDetSP.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Solicitud Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtCodPedd.Text.Equals(""))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPD");
                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                    GrdDetSP.Enabled = false;
                }
                else
                {
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    List<ClsTypSolicitudPedido> ObjEncSolPed = new List<ClsTypSolicitudPedido>();
                    var TypEncSolPed = new ClsTypSolicitudPedido()
                    {
                        IdPedido = Convert.ToInt32(ViewState["IdPedido"].ToString()),
                        CodPedido = TxtCodPedd.Text.Trim(),
                        Fechapedido = Convert.ToDateTime(TxtFech.Text),
                        CodPrioridad = DdlPriord.Text.Trim(),
                        CodResponsable = DdlRespsbl.Text.Trim(),
                        CodReserva = Convert.ToInt32(DdlPpt.Text.Trim()),
                        CodEstado = DdlEstd.Text.Trim(),
                        Obsevacion = TxtObsrvcn.Text.Trim(),
                        CodtipoSolPedido = DdlTipo.Text.Trim(),
                        Ccostos = DdlCcosto.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        CodTipoCodigo = Session["CodTipoCodigoInicial"].ToString(),
                        FechaRemocionSP = null,
                        Aplicabilidad = "SolPed",
                        Accion = "UPDATE",
                    };
                    ObjEncSolPed.Add(TypEncSolPed);

                    List<ClsTypSolicitudPedido> ObjDetSP = new List<ClsTypSolicitudPedido>();
                    foreach (GridViewRow Row in GrdDetSP.Rows)
                    {
                        var TypDetSP = new ClsTypSolicitudPedido()
                        {
                            IdDetPedido = Convert.ToInt32(GrdDetSP.DataKeys[Row.RowIndex].Values[0].ToString()),
                            CodReferencia = (Row.FindControl("LblRefP") as Label).Text.Trim(),
                            PN = (Row.FindControl("LblPnP") as Label).Text.Trim(),
                            CodUndMedida = (Row.FindControl("UndMedP") as Label).Text.Trim(),
                            CantidadTotal = Convert.ToDouble((Row.FindControl("LblCantP") as Label).Text.Trim()),
                            CantidadAlmacen = Convert.ToDouble((Row.FindControl("LblCantP") as Label).Text.Trim()),
                            CantidadReparacion = 0,
                            CantidadOrden = Convert.ToDouble((Row.FindControl("LblCantP") as Label).Text.Trim()),
                            Posicion = Convert.ToInt32((Row.FindControl("LblPoscP") as Label).Text.Trim()),
                            AprobacionDetalle = 0,
                            CodSeguimiento = (Row.FindControl("LblCodSegP") as Label).Text.Trim(),
                            Descripcion = (Row.FindControl("LblDescP") as Label).Text.Trim(),
                            TipoPedido = 1,
                            CantidadAjustada = Convert.ToDouble((Row.FindControl("LblCantP") as Label).Text.Trim()),
                            Notas = "",
                            PosicionPr = 0,
                            IdSrvPr = 0,
                            IdReporte = 0,
                            IdDetProPSrvSP = 0,
                            CodIdDetalleResSP = 0,
                            FechaAprob = null,
                            CodAeronaveSP = 0,
                        };
                        ObjDetSP.Add(TypDetSP);
                    }
                    ClsTypSolicitudPedido ClsSolPed = new ClsTypSolicitudPedido();
                    ClsSolPed.Alimentar(ObjEncSolPed, ObjDetSP);
                    string Mensj = ClsSolPed.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampos(false, false, "UPD");
                    Traerdatos(ViewState["IdPedido"].ToString().Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    GrdDetSP.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Persona", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (TxtCodPedd.Text.Equals(""))
            { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC SP_PANTALLA_SolicitudPedido 20,@Usu, @NP,@TSP,'',@IP,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@NP", TxtCodPedd.Text.Trim());
                            SC.Parameters.AddWithValue("@TSP", DdlTipo.Text.Trim());
                            SC.Parameters.AddWithValue("@IP", ViewState["IdPedido"].ToString());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { VbMensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                                return;
                            }
                            Transac.Commit();
                            ViewState["TtlRegDet"] = 0; LimpiarCampos("DEL"); GrdDetSP.Visible = false;
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Solicitud Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnAlert_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            // Response.Redirect("~/Forms/Almacen/FrmAlertaSolicitudNueva.aspx");
            string SPNw = "window.open('/Forms/Almacen/FrmAlertaSolicitudNueva.aspx', '_blank');";
            string SPRp = "window.open('/Forms/Almacen/FrmAlertaSolicitudNuevaRepa.aspx', '_blank');";
            string SPVenc = "window.open('/Forms/Almacen/FrmAlertaVencSP.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SPNw, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SPRp, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SPVenc, true);
            //Response.Redirect("~/Forms/Almacen/FrmAlertaSolicitudNuevaRepa.aspx");
        }
        protected void BtnOpenCotiza_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string CT = "window.open('/Forms/InventariosCompras/FrmCotizacion.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), CT, true);
        }
        //************************** MODAL buscar PN para asignar en la PPT ***********************************************
        protected void BindModalBusqPN()
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            string VbOpcion = "PN  LIKE '%" + TxtModalBusq.Text.Trim() + "%'";
            if (DSTDdl.Tables["ConsltPN"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                if (RdbMOdalBusqPN.Checked == true) { VbOpcion = "PN  LIKE '%" + TxtModalBusq.Text.Trim() + "%'"; }
                if (RdbMOdalBusqDesc.Checked == true) { VbOpcion = "Descripcion  LIKE '%" + TxtModalBusq.Text.Trim() + "%'"; }
                DR = DSTDdl.Tables[6].Select(VbOpcion);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }

                if (DT.Rows.Count > 0) { GrdModalBusqPN.DataSource = DT; GrdModalBusqPN.DataBind(); }
                else { GrdModalBusqPN.DataSource = null; GrdModalBusqPN.DataBind(); }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            BindModalBusqPN();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqPN", "$('#ModalBusqPN').modal();", true);
        }
        protected void GrdModalBusqPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (e.CommandName.Equals("IrPN"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string VbPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                string VbDesc = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                string VblUndM = ((Label)row.FindControl("LblUndMed")).Text.ToString().Trim();
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                string VbRef = GrdModalBusqPN.DataKeys[gvr.RowIndex].Values["CodReferencia"].ToString().Trim();
                ViewState["CodEstadoPn"] = GrdModalBusqPN.DataKeys[gvr.RowIndex].Values["CodEstadoPn"].ToString().Trim();

                (GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Text = VbPn;
                (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Text = VbDesc;
                (GrdDetSP.FooterRow.FindControl("TxtRefPP") as TextBox).Text = VbRef;
                (GrdDetSP.FooterRow.FindControl("TxtUndMPP") as TextBox).Text = VblUndM;
            }
        }
        protected void GrdModalBusqPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result;
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbEstdPN = dr["CodEstadoPn"].ToString();
                if (VbEstdPN.Equals("03")) // PN superado
                { e.Row.BackColor = System.Drawing.Color.DarkRed; e.Row.ForeColor = System.Drawing.Color.White; }
                ImageButton IbtIrPN = (e.Row.FindControl("IbtIrPN") as ImageButton);

                if (IbtIrPN != null)
                {
                    Result = Idioma.Select("Objeto='IbtIrMstr'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtIrPN.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void CkbIngrPNNuevo_CheckedChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            (GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Enabled = true;
            (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Enabled = true;
            CkbIngrPNNuevo.Checked = false;
        }
        //****************************** DETALLE Sol Ped **************************************
        protected void BindDDetTmp()
        {
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            if (TxtCodPedd.Text.Trim().Equals(""))
            {
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
                GrdDetSP.Visible = true;
                if (TblDetalle.Rows.Count > 0) { GrdDetSP.DataSource = TblDetalle; GrdDetSP.DataBind(); }
                else
                {
                    TblDetalle.Rows.Add(TblDetalle.NewRow());
                    GrdDetSP.DataSource = TblDetalle;
                    GrdDetSP.DataBind();
                    GrdDetSP.Rows[0].Cells.Clear();
                    GrdDetSP.Rows[0].Cells.Add(new TableCell());
                    GrdDetSP.Rows[0].Cells[0].Text = "Empty..!";
                    GrdDetSP.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    TblDetalle.NewRow();
                    GrdDetSP.DataSource = TblDetalle;
                    GrdDetSP.DataBind();
                }
            }
            else
            {
                if (DSTPpl.Tables["DetSolPed"].Rows.Count > 0)
                { GrdDetSP.DataSource = DSTPpl.Tables[1]; GrdDetSP.DataBind(); }
                else
                {
                    DSTPpl.Tables[1].Rows.Add(DSTPpl.Tables[1].NewRow());
                    GrdDetSP.DataSource = DSTPpl.Tables[1];
                    GrdDetSP.DataBind();
                    GrdDetSP.Rows[0].Cells.Clear();
                    GrdDetSP.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdDetSP.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdDetSP.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        protected void GrdDetSP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            (GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Enabled = false;
            (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Enabled = false;
            if (e.CommandName.Equals("BusqPN"))// buscar el PN que se va a agregar
            { /*ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqPN", "$('#ModalBusqPN').modal();", true);*/
               ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
            }
            if (TxtCodPedd.Text.Equals("")) // Es desde la DataTable Temporal
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];

                    if ((GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens16'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N.
                        return;
                    }
                    string VbPN = (GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim();
                    string VbDesc = (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();
                    string VbRf = (GrdDetSP.FooterRow.FindControl("TxtRefPP") as TextBox).Text.Trim();
                    string VbUnMed = (GrdDetSP.FooterRow.FindControl("TxtUndMPP") as TextBox).Text.Trim();

                    CultureInfo Culture = new CultureInfo("en-US");
                    string VblTxtCant = (GrdDetSP.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetSP.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim();
                    double VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    if (VblCant <= 0)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens13Aero'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Existen cantidades igual o menor a cero.
                        return;
                    }

                    TblDetalle.Rows.Add(0, VbPN, VbDesc, VbDesc, VbRf, VblCant, 0, VbUnMed, "", "SOL", "", ViewState["CodEstadoPn"].ToString().Trim(), 0, VbRf, 0);
                    BindDDetTmp();
                    ViewState["CodEstadoPn"] = "00";
                }
            }
            else// Insert detalle en una SP existente
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    if ((GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens16'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N.
                        return;
                    }
                    string VBQuery, VblTxtCant, VbDesc, Mensj = "";
                    double VblCant;

                    CultureInfo Culture = new CultureInfo("en-US");
                    VblTxtCant = (GrdDetSP.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetSP.FooterRow.FindControl("TxtCantPP") as TextBox).Text.Trim();
                    VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    VbDesc = (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();

                    if (VblCant <= 0)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens13Aero'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Existen cantidades igual o menor a cero.
                        return;
                    }
                    // VbIPC = (GrdRecursoF.FooterRow.FindControl("TxtIPCRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_TablasLogistica 1, @Rf, @Um, @Dsc, @Usu, @Pn,'','','','INSERT', @Id, @Cnt, @IdEP,0,0, @ICC,'01-01-1','02-01-1','03-01-1'");
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@Rf", (GrdDetSP.FooterRow.FindControl("TxtRefPP") as TextBox).Text.Trim());
                                    SC.Parameters.AddWithValue("@Um", (GrdDetSP.FooterRow.FindControl("TxtUndMPP") as TextBox).Text.Trim());
                                    SC.Parameters.AddWithValue("@Dsc", (GrdDetSP.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim());
                                    SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@PN", (GrdDetSP.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim());
                                    SC.Parameters.AddWithValue("@Id", 0);
                                    SC.Parameters.AddWithValue("@Cnt", VblCant);
                                    SC.Parameters.AddWithValue("@IdEP", ViewState["IdPedido"].ToString());
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                                    SqlDataReader SDR = SC.ExecuteReader();
                                    if (SDR.Read())
                                    { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                    SDR.Close();

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
                                    Traerdatos(ViewState["IdPedido"].ToString(), "UPD");
                                    BindDDetTmp();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT DetPedido Existente", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void GrdDetSP_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDetSP.EditIndex = e.NewEditIndex; BindDDetTmp(); }
        protected void GrdDetSP_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtCodPedd.Text.Equals("")) // Es desde la DataTable Temporal
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                int index = Convert.ToInt32(e.RowIndex);

                CultureInfo Culture = new CultureInfo("en-US");
                string VbTxtCant = (GrdDetSP.Rows[e.RowIndex].FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetSP.Rows[e.RowIndex].FindControl("TxtCant") as TextBox).Text.Trim();
                double VbCant = VbTxtCant.Length == 0 ? 0 : Convert.ToDouble(VbTxtCant, Culture);

                TblDetalle.Rows[index]["CantidadTotal"] = VbCant;
                TblDetalle.AcceptChanges();
                GrdDetSP.EditIndex = -1;
                BindDDetTmp();
            }
            else// Editar una SP existente
            {
                string VBQuery, VbTxtCant;
                double VbCant;
                int Id = Convert.ToInt32(GrdDetSP.DataKeys[e.RowIndex].Values["IdDetPedido"].ToString());
                CultureInfo Culture = new CultureInfo("en-US");
                VbTxtCant = (GrdDetSP.Rows[e.RowIndex].FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetSP.Rows[e.RowIndex].FindControl("TxtCant") as TextBox).Text.Trim();
                VbCant = VbTxtCant.Length == 0 ? 0 : Convert.ToDouble(VbTxtCant, Culture);

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasLogistica 1, @Rf, @Um, @Dsc, @Usu, @Pn,'','','','UPDATE', @Id, @Cnt, @IdEP,0,0, @ICC,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Rf", (GrdDetSP.Rows[e.RowIndex].FindControl("LblRef") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Um", (GrdDetSP.Rows[e.RowIndex].FindControl("UndMed") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Dsc", (GrdDetSP.Rows[e.RowIndex].FindControl("LblDesc") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Pn", (GrdDetSP.Rows[e.RowIndex].FindControl("LblPn") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Id", Convert.ToInt32(GrdDetSP.DataKeys[e.RowIndex].Values["IdDetPedido"].ToString()));
                                SC.Parameters.AddWithValue("@IdEP", ViewState["IdPedido"].ToString());
                                SC.Parameters.AddWithValue("@Cnt", VbCant);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                                GrdDetSP.EditIndex = -1;
                                Traerdatos(ViewState["IdPedido"].ToString(), "UPD");
                                BindDDetTmp();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Error en el editar')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
        }
        protected void GrdDetSP_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDetSP.EditIndex = -1; BindDDetTmp(); }
        protected void GrdDetSP_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtCodPedd.Text.Equals("")) // Es desde la DataTable Temporal
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                int index = Convert.ToInt32(e.RowIndex);
                TblDetalle.Rows[index].Delete();
                BindDDetTmp();
            }
            else
            {
                string VblId = GrdDetSP.DataKeys[e.RowIndex].Values["IdDetPedido"].ToString();

                string VblPN = (GrdDetSP.Rows[e.RowIndex].FindControl("LblPnP") as Label).Text.Trim();

                string VbPosc = (GrdDetSP.Rows[e.RowIndex].FindControl("LblPoscP") as Label).Text.Trim();

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasLogistica 1,@Rf,'', @Dsc, @Usu,@PN, @CdSP,'','','DELETE',@Id,@Cnt,@IdEP,@Posc,0, @ICC,'01-01-1','02-01-1','03-01-1'");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Rf", (GrdDetSP.Rows[e.RowIndex].FindControl("LblRefP") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Dsc", (GrdDetSP.Rows[e.RowIndex].FindControl("LblDescP") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@CdSP", TxtCodPedd.Text.Trim());
                                SC.Parameters.AddWithValue("@Id", VblId);
                                SC.Parameters.AddWithValue("@Cnt", (GrdDetSP.Rows[e.RowIndex].FindControl("LblCantP") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@IdEP", ViewState["IdPedido"].ToString());
                                SC.Parameters.AddWithValue("@Posc", VbPosc);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                                string Mensj = "";

                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj.ToString().Trim() + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                Traerdatos(ViewState["IdPedido"].ToString(), "UPD");
                                BindDDetTmp();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }

                        }
                    }
                }

            }
        }
        protected void GrdDetSP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }

                ImageButton IbtBusqPn = (e.Row.FindControl("IbtBusqPn") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtBusqPnTT'");
                foreach (DataRow row in Result)
                { IbtBusqPn.ToolTip = row["Texto"].ToString().Trim(); }
                if (DdlTipo.Text.Trim().Equals("02") || DdlTipo.Text.Trim().Equals("03"))
                { IbtAddNew.Visible = false; IbtBusqPn.Visible = false; }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
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
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbEstdPN = dr["CodEstadoPn"].ToString();
                switch (VbEstdPN)
                {
                    case "00":// PN que no existen
                        e.Row.BackColor = System.Drawing.Color.Orange;
                        break;
                    case "03":// Superados
                        e.Row.BackColor = System.Drawing.Color.DarkRed; e.Row.ForeColor = System.Drawing.Color.White;
                        break;
                }
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    if (DdlTipo.Text.Trim().Equals("02") || DdlTipo.Text.Trim().Equals("03"))
                    { imgE.Visible = false; }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    if (DdlTipo.Text.Trim().Equals("02") || DdlTipo.Text.Trim().Equals("03"))
                    { imgD.Visible = false; }
                }

            }
        }
        //****************************** Busqueda **************************************
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; TxtBusqueda.Focus(); }
        protected void BIndDBusqSP()
        {
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbOpcion = "SP";

                if (RdbBusqNumSlPd.Checked == true)
                { VbOpcion = "SP"; }
                if (RdbBusqPN.Checked == true)
                { VbOpcion = "PN"; }

                string VbTxtSql = "EXEC SP_PANTALLA_SolicitudPedido 19, @Prmtr,'','',@Opc,0,0, @Idm, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim()); ;
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
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
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); BIndDBusqSP(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["IdPedido"] = GrdBusq.DataKeys[gvr.RowIndex].Values["IdPedido"].ToString();
                Traerdatos(ViewState["IdPedido"].ToString().Trim(), "UPD");
                MultVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DdlTipo.ToolTip = "";
                GrdDetSP.Visible = true;
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
        //******************************************  Subir Recurso maxivamente *********************************************************
        protected void BtnCargaMaxiva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtCodPedd.Text.Equals(""))
            { return; }
            if ((int)ViewState["TtlRegDet"] > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MstrMens32'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Para realizar la carga masiva el detalle debe estar sin registros.
                return;
            }
            TxtCargaMasvNumPed.Text = TxtCodPedd.Text;

            IbtGuardarCargaMax.Enabled = false;
            MultVw.ActiveViewIndex = 2;
            Page.Title = ViewState["PageTit"].ToString().Trim();
        }
        protected void IbtCerrarSubMaxivo_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtSubirCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DataRow[] Result;
                DataTable DT = new DataTable();
                if (FileUpRva.Visible == false) { FileUpRva.Visible = true; }
                else
                {
                    if (FileUpRva.HasFile == true)
                    {
                        string FolderPath;
                        string FileName = Path.GetFileName(FileUpRva.PostedFile.FileName);
                        string VblExt = Path.GetExtension(FileUpRva.PostedFile.FileName);
                        if (Cnx.GetProduccion().Trim().Equals("Y")) { FolderPath = ConfigurationManager.AppSettings["FolderPath"]; }//Azure
                        else { FolderPath = ConfigurationManager.AppSettings["FoldPathLcl"]; }

                        VblExt = VblExt.Substring(VblExt.LastIndexOf(".") + 1).ToLower();
                        string[] formatos = new string[] { "xls", "xlsx" };
                        if (Array.IndexOf(formatos, VblExt) < 0)
                        {
                            Result = Idioma.Select("Objeto= 'RteMens40'");//Archivo inválido
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                            return;
                        }
                        string FilePath = FolderPath + FileName;
                        FileUpRva.SaveAs(FilePath);
                        Import(FilePath, VblExt);
                        FileUpRva.Visible = false;
                    }
                    else
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens34'");//Debe seleccionar un archivo.
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Cargar Masiva Solicitud Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void Import(string FilePath, string Extension)
        {
            try
            {
                FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader ExcelReader;

                ExcelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                //// para que tome la primera fila como titulo de campos
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    { UseHeaderRow = true }
                };
                var dataSet = ExcelReader.AsDataSet(conf);
                DataTable DT = dataSet.Tables[0];

                if (DT.Rows.Count > 0) { GrdCargaMax.DataSource = DT; GrdCargaMax.DataBind(); Session["TablaRsvaResul"] = DT; }               
                List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
                foreach (GridViewRow Row in GrdCargaMax.Rows)
                {
                    TextBox TxtPNRF = Row.FindControl("TxtPNRF") as TextBox;
                    TextBox TxtDesRF = Row.FindControl("TxtDesRF") as TextBox;
                    TextBox TxtCantRF = Row.FindControl("TxtCantRF") as TextBox;
                    TextBox TxtUMRF = Row.FindControl("TxtUMRF") as TextBox;
                    TextBox TxtUMSysRF = Row.FindControl("TxtUMSysRF") as TextBox;
                    TextBox TxtIPCRF = Row.FindControl("TxtIPCRF") as TextBox;
                    string VbTxtCant = TxtCantRF.Text.Trim().Equals("") ? "0" : TxtCantRF.Text.Trim();
                    double VblCant = TxtCantRF.Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCant);

                    var TypSubirRsva = new CsTypSubirReserva()
                    {
                        IdRsva = Convert.ToInt32(ViewState["IdPedido"]),
                        Posicion = 0,
                        PN = TxtPNRF.Text.Trim(),
                        Descripcion = TxtDesRF.Text.Trim(),
                        Cantidad = VblCant,
                        UndSolicitada = TxtUMRF.Text.Trim(),
                        UndSistema = TxtUMSysRF.Text.Trim(),
                        IPC = TxtIPCRF.Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        CodAeronave = 0,
                        ProcesoOrigen = "SOL_PEDIDO",
                        Accion = "TEMPORAL",
                    };
                    ObjSubirRsva.Add(TypSubirRsva);
                }
                CsTypSubirReserva SubirRsva = new CsTypSubirReserva();

                SubirRsva.Alimentar(ObjSubirRsva);// 
                string Mensj = SubirRsva.GetMensj();
                if (!Mensj.Trim().Equals("OK"))
                {
                    GrdCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                    GrdCargaMax.DataBind();
                    IbtGuardarCargaMax.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                GrdCargaMax.DataSource = (DataTable)Session["TablaRsvaResul"];
                GrdCargaMax.DataBind();
                IbtGuardarCargaMax.Enabled = true;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Import Detalle Sol Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbtGuardarCargaMax_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            List<CsTypSubirReserva> ObjSubirRsva = new List<CsTypSubirReserva>();
            foreach (GridViewRow Row in GrdCargaMax.Rows)
            {
                TextBox TxtPNRF = Row.FindControl("TxtPNRF") as TextBox;
                TextBox TxtDesRF = Row.FindControl("TxtDesRF") as TextBox;
                TextBox TxtCantRF = Row.FindControl("TxtCantRF") as TextBox;
                TextBox TxtUMRF = Row.FindControl("TxtUMRF") as TextBox;
                TextBox TxtUMSysRF = Row.FindControl("TxtUMSysRF") as TextBox;
                TextBox TxtIPCRF = Row.FindControl("TxtIPCRF") as TextBox;
                string VbTxtCant = TxtCantRF.Text.Trim().Equals("") ? "0" : TxtCantRF.Text.Trim();
                double VblCant = TxtCantRF.Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VbTxtCant);

                var TypSubirRsva = new CsTypSubirReserva()
                {
                    IdRsva = Convert.ToInt32(ViewState["IdPedido"]),
                    Posicion = 0,
                    PN = TxtPNRF.Text.Trim(),
                    Descripcion = TxtDesRF.Text.Trim(),
                    Cantidad = VblCant,
                    UndSolicitada = TxtUMRF.Text.Trim(),
                    UndSistema = TxtUMSysRF.Text.Trim(),
                    IPC = TxtIPCRF.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    CodAeronave = 0,
                    ProcesoOrigen = "SOL_PEDIDO",
                    Accion = "INSERT",
                };
                ObjSubirRsva.Add(TypSubirRsva);
            }
            CsTypSubirReserva SubirRsva = new CsTypSubirReserva();

            SubirRsva.Alimentar(ObjSubirRsva);// 
            string Mensj = SubirRsva.GetMensj();
            if (!Mensj.Trim().Equals("OK"))
            {
                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                IbtGuardarCargaMax.Enabled = false;
                return;
            }
            IbtGuardarCargaMax.Enabled = false;
            Traerdatos(ViewState["IdPedido"].ToString(), "UPD");
            BindDDetTmp();
            MultVw.ActiveViewIndex = 0;
            GrdCargaMax.DataSource = null; GrdCargaMax.DataBind();

        }
    }
}