using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
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

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmCotizacion : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable TblDetalle = new DataTable();
        DataTable DTSolPed = new DataTable();
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
                    Session["MonLcl"] = Cnx.GetMonedLcl();// Moneda Local
                }
            }
            if (!IsPostBack)
            {
                ViewState["Accion"] = "";
                ViewState["IdCotiza"] = "0";
                ViewState["TipoCotiza"] = "01";
                BtnCompra.CssClass = "btn btn-primary";
                ViewState["CodTerceroAnt"] = ""; //CodPriordAnt
                ViewState["ContactoAnt"] = "";// ContactoAnt
                ViewState["TipoPagoAnt"] = "";// TipoAnt
                ViewState["LugarEAnt"] = "";// CodCCostoAnt              
                ViewState["CodTipoCotizacion_ANT"] = "";// Guarda el tipocotiza anterior para saber si es editada
                ViewState["TtlRegDet"] = 0; // saber si el detalle tiene registro para realizar carga masiva
                ViewState["CarpetaCargaMasiva"] = "";// para mostrar en el boton de carga masiva la ruta por defecto donde se debe guardar el archivo para subir
                ViewState["Monto"] = "0";
                ViewState["ValorIva"] = "0";
                ViewState["DocAprobado"] = "N";
                ViewState["PeriodCerrado"] = "N";
                ViewState["ShipLiquidada"] = "N";
                ViewState["TieneSOMvtoAlma"] = "N";
                ViewState["NomArchivoCM"] = "Quote_Cotizacion.xlsx";
                ModSeguridad();
                BindBDdl("UPD");
                RdbBusqNumCot.Checked = true;
                RdbMOdalBusqSP.Checked = true;
                AddCamposDataTable("INS");
                EnablGridDet("Visible", false);
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; } // grd.ShowFooter = false;
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; BtnCargaMaxiva.Visible = false; }
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
                    BtnExportar.Text = bO.Equals("BtnExportMstr") ? bT : BtnExportar.Text;
                    LblNumCotiza.Text = bO.Equals("LblNumCotiza") ? bT : LblNumCotiza.Text;
                    LblNumPetcn.Text = bO.Equals("LblNumPetcn") ? bT : LblNumPetcn.Text;
                    LblNumDocum.Text = bO.Equals("LblNumDocum") ? bT : LblNumDocum.Text;
                    LblDatosPpt.Text = bO.Equals("LblDatosPpt") ? bT : LblDatosPpt.Text;
                    BtnCompra.Text = bO.Equals("BtnCompra") ? bT : BtnCompra.Text;
                    BtnRepa.Text = bO.Equals("BtnRepa") ? bT : BtnRepa.Text;
                    BtnInterc.Text = bO.Equals("BtnInterc") ? bT : BtnInterc.Text;
                    BtnCargaMaxiva.Text = bO.Equals("BtnCargaMaxiva") ? bT : BtnCargaMaxiva.Text;
                    BtnCargaMaxiva.ToolTip = bO.Equals("BtnCargaMaxivaTT") ? bT : BtnCargaMaxiva.ToolTip;
                    BtnOpenSolPed.Text = bO.Equals("BtnOpenSolPed") ? bT : BtnOpenSolPed.Text;
                    BtnOpenSolPed.ToolTip = bO.Equals("BtnOpenSolPedTT") ? bT : BtnOpenSolPed.ToolTip;
                    //*************************************************Campos *************************************************
                    LblProvee.Text = bO.Equals("LblProvee") ? bT : LblProvee.Text;
                    LblContact.Text = bO.Equals("LblContact") ? bT : LblContact.Text;
                    LblMoned.Text = bO.Equals("LblMoned") ? bT : LblMoned.Text;
                    LblTipoCot.Text = bO.Equals("TipoMstr") ? bT : LblTipoCot.Text;
                    LblEstd.Text = bO.Equals("LblEstadoMst") ? bT : LblEstd.Text;
                    LblTipoPago.Text = bO.Equals("LblTipoPago") ? bT : LblTipoPago.Text;
                    LblLugarEntrg.Text = bO.Equals("LblLugarEntrg") ? bT : LblLugarEntrg.Text;
                    LblMedioCot.Text = bO.Equals("LblMedioCot") ? bT : LblMedioCot.Text;
                    LblObsrv.Text = bO.Equals("LblObsMst") ? bT : LblObsrv.Text;
                    LblFechCot.Text = bO.Equals("LblFechCot") ? bT : LblFechCot.Text;
                    LblFechPlazRes.Text = bO.Equals("LblFechPlazRes") ? bT : LblFechPlazRes.Text;
                    LblFechRespt.Text = bO.Equals("LblFechRespt") ? bT : LblFechRespt.Text;
                    LblFechVigc.Text = bO.Equals("LblFechVigc") ? bT : LblFechVigc.Text;
                    LblFechTRM.Text = bO.Equals("LblFechTRM") ? bT : LblFechTRM.Text;
                    LblSubTtal.Text = bO.Equals("LblSubTtal") ? bT : LblSubTtal.Text;
                    LblIVA.Text = bO.Equals("LblIVA") ? bT : LblIVA.Text;
                    LblOtrImpt.Text = bO.Equals("LblOtrImpt") ? bT : LblOtrImpt.Text;
                    LblTtl.Text = bO.Equals("LblTtl") ? bT : LblTtl.Text;
                    LblTRM.Text = bO.Equals("LblTRM") ? bT : LblTRM.Text;
                    // *************************************************Grid detalle *************************************************
                    IbtAprPNAll.ToolTip = bO.Equals("IbtAprPNAll") ? bT : IbtAprPNAll.ToolTip;
                    if (bO.Equals("placeholderBusPN"))
                    { TxtBusqPn.Attributes.Add("placeholder", bT); }
                    IbtBusqPn.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqPn.ToolTip;
                    GrdDet.Columns[1].HeaderText = bO.Equals("BtnOpenSolPed") ? bT : GrdDet.Columns[1].HeaderText;
                    GrdDet.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdDet.Columns[4].HeaderText;
                    GrdDet.Columns[5].HeaderText = bO.Equals("CantMst") ? bT : GrdDet.Columns[5].HeaderText;
                    GrdDet.Columns[6].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDet.Columns[6].HeaderText;
                    GrdDet.Columns[7].HeaderText = bO.Equals("GridVlrUnd") ? bT : GrdDet.Columns[7].HeaderText;
                    GrdDet.Columns[8].HeaderText = bO.Equals("LblIVA") ? bT : GrdDet.Columns[8].HeaderText;
                    GrdDet.Columns[9].HeaderText = bO.Equals("GridVI") ? bT : GrdDet.Columns[9].HeaderText;
                    GrdDet.Columns[10].HeaderText = bO.Equals("GrdTtl") ? bT : GrdDet.Columns[10].HeaderText;
                    GrdDet.Columns[11].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdDet.Columns[11].HeaderText;
                    GrdDet.Columns[12].HeaderText = bO.Equals("GrdTmEnt") ? bT : GrdDet.Columns[12].HeaderText;
                    GrdDet.Columns[13].HeaderText = bO.Equals("GrdUMCmp") ? bT : GrdDet.Columns[13].HeaderText;
                    GrdDet.Columns[14].HeaderText = bO.Equals("GrdPnAlt") ? bT : GrdDet.Columns[14].HeaderText;
                    // *************************************************opcion de busqueda *************************************************
                    RdbBusqNumCot.Text = bO.Equals("LblNumCotiza") ? "&nbsp" + bT : RdbBusqNumCot.Text;
                    RdbBusqProvee.Text = bO.Equals("LblProvee") ? "&nbsp" + bT : RdbBusqProvee.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); TxtModalBusq.Attributes.Add("placeholder", bT); }

                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("BtnOpenSolPed") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("TipoMstr") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("LblFechCot") ? bT : GrdBusq.Columns[3].HeaderText;
                    GrdBusq.Columns[4].HeaderText = bO.Equals("LblProvee") ? bT : GrdBusq.Columns[4].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("Descripcion") ? bT : GrdBusq.Columns[7].HeaderText;
                    //**************************************** Modal Asignar Pedido ****************************************
                    LblTitModalBusqPN.Text = bO.Equals("LblTitModalBusqPN") ? bT : LblTitModalBusqPN.Text;
                    RdbMOdalBusqSP.Text = bO.Equals("BtnOpenSolPed") ? "&nbsp" + bT : RdbMOdalBusqSP.Text;
                    RdbMOdalBusqPPT.Text = bO.Equals("LblDatosPpt") ? "&nbsp" + bT : RdbMOdalBusqPPT.Text;
                    RdbMOdalBusqPet.Text = bO.Equals("LblNumPetcn") ? "&nbsp" + bT : RdbMOdalBusqPet.Text;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT : LblModalBusq.Text;
                    IbtModalBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtModalBusq.ToolTip;
                    IbtAprDetAll.ToolTip = bO.Equals("IbtAprPNAll") ? bT : IbtAprDetAll.ToolTip;
                    GrdModalBusqCot.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqCot.EmptyDataText;
                    GrdModalBusqCot.Columns[1].HeaderText = bO.Equals("BtnOpenSolPed") ? bT : GrdModalBusqCot.Columns[1].HeaderText;
                    GrdModalBusqCot.Columns[4].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdModalBusqCot.Columns[4].HeaderText;
                    GrdModalBusqCot.Columns[5].HeaderText = bO.Equals("CantMst") ? bT : GrdModalBusqCot.Columns[5].HeaderText;
                    GrdModalBusqCot.Columns[6].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdModalBusqCot.Columns[6].HeaderText;
                    GrdModalBusqCot.Columns[7].HeaderText = bO.Equals("GrdMdlSeg") ? bT : GrdModalBusqCot.Columns[7].HeaderText;
                    BtnCloseModalBusqPN.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqPN.Text;
                    BtnAsignarModal.Text = bO.Equals("BtnAsignarModal") ? bT : BtnAsignarModal.Text;

                    //************************************** Exportar Excel *****************************************************
                    LblTitExport.Text = bO.Equals("LblTitExport") ? bT : LblTitExport.Text;
                    IbtCloseExport.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseExport.ToolTip;
                    BtnExportDetCotiza.Text = bO.Equals("BtnExportDetCotiza") ? bT : BtnExportDetCotiza.Text;
                    BtnExportDetCotiza.ToolTip = bO.Equals("BtnExportDetCotizaTT") ? bT : BtnExportDetCotiza.ToolTip;
                    BtnExportDetUnidMed.Text = bO.Equals("BtnExportDetUnidMed") ? bT : BtnExportDetUnidMed.Text;
                    BtnExportDetUnidMed.ToolTip = bO.Equals("BtnExportDetUnidMedTT") ? bT : BtnExportDetUnidMed.ToolTip;

                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result) { BtnEliminar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                Result = Idioma.Select("Objeto= 'BtnCargaMaxivaOnClk'");
                foreach (DataRow row in Result)
                { BtnCargaMaxiva.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

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
                        Row.Cells[15].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void EnablGridDet(string Propiedad, bool TF)
        {
            if (Propiedad.Equals("Visible"))
            { GrdDet.Visible = TF; IbtAprPNAll.Visible = TF; TxtBusqPn.Visible = TF; IbtBusqPn.Visible = TF; }

            if (Propiedad.Equals("Enabled"))
            { GrdDet.Enabled = TF; IbtAprPNAll.Enabled = TF; }
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
            if (Accion.Equals("INS"))// Nuevo los campos como se llaman en la grid
            {
                TblDetalle.Columns.Add("Vista", typeof(string));//0
                TblDetalle.Columns.Add("IdDetCotizacion", typeof(int));//
                TblDetalle.Columns.Add("IdCotizacion", typeof(int));//2
                TblDetalle.Columns.Add("IdDetPedido", typeof(int));//
                TblDetalle.Columns.Add("CodAeronaveCT", typeof(int));//4
                TblDetalle.Columns.Add("Aprobacion", typeof(int));//
                TblDetalle.Columns.Add("CodPedido", typeof(string));//6
                TblDetalle.Columns.Add("Posicion", typeof(int));//
                TblDetalle.Columns.Add("PN", typeof(string));//8
                TblDetalle.Columns.Add("DESPN", typeof(string));
                TblDetalle.Columns.Add("Cantidad", typeof(double));//10
                TblDetalle.Columns.Add("CodUndMed", typeof(string));
                TblDetalle.Columns.Add("ValorUnidad", typeof(double));//12
                TblDetalle.Columns.Add("TasaIva", typeof(double));
                TblDetalle.Columns.Add("ValorIva", typeof(double));//14
                TblDetalle.Columns.Add("ValorTotal", typeof(double));//
                TblDetalle.Columns.Add("CodEstdo", typeof(string));//16
                TblDetalle.Columns.Add("TiempoEntrega", typeof(int));//
                TblDetalle.Columns.Add("UndMinimaCompra", typeof(int));//18
                TblDetalle.Columns.Add("Alterno", typeof(string));//
                TblDetalle.Columns.Add("SN", typeof(string));//20
                TblDetalle.Columns.Add("Ccostos", typeof(string));//
                TblDetalle.Columns.Add("ExisteDoc", typeof(string));//22
                TblDetalle.Columns.Add("AccionDet", typeof(string));//23
                TblDetalle.Columns.Add("ObservacionesDC", typeof(string));//23
                ViewState["TblDetalle"] = TblDetalle;
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {

                    string VbTxtSql = "EXEC PNTLL_Cotizac 1,'','','','','','DDL',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
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
                                DSTDdl.Tables[1].TableName = "Contacto";
                                DSTDdl.Tables[2].TableName = "TipoCot";
                                DSTDdl.Tables[3].TableName = "Moneda";
                                DSTDdl.Tables[4].TableName = "Estado";
                                DSTDdl.Tables[5].TableName = "TipoPago";
                                DSTDdl.Tables[6].TableName = "LugarEntrg";
                                DSTDdl.Tables[7].TableName = "MedioCot";
                                DSTDdl.Tables[8].TableName = "PN";
                                DSTDdl.Tables[9].TableName = "UndMed";
                                DSTDdl.Tables[10].TableName = "EstadoElem";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            string VbCodAnt;

            if (DSTDdl.Tables["Tercero"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[0].Select("(Activo=1 AND  Clasificacion IN ('P','A')) OR CodTercero= '" + ViewState["CodTerceroAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlProvee.DataSource = DT;
                DdlProvee.DataTextField = "RazonSocial";
                DdlProvee.DataValueField = "CodTercero";
                DdlProvee.DataBind();
                DdlProvee.SelectedValue = ViewState["CodTerceroAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Contacto"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[1].Select("CodTercero= '" + ViewState["CodTerceroAnt"] + "' OR CodTercero = ''");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlContact.DataSource = DT;
                DdlContact.DataTextField = "Contacto";
                DdlContact.DataValueField = "Codigo";
                DdlContact.DataBind();
                DdlContact.SelectedValue = ViewState["ContactoAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["TipoCot"].Rows.Count > 0)
            {
                VbCodAnt = DdlTipoCot.Text.Trim();
                DdlTipoCot.DataSource = DSTDdl.Tables[2];
                DdlTipoCot.DataTextField = "Descripcion";
                DdlTipoCot.DataValueField = "CodIdTipoOrdenCompra";
                DdlTipoCot.DataBind();
                DdlTipoCot.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["Moneda"].Rows.Count > 0)
            {
                VbCodAnt = DdlMoned.Text.Trim();
                DdlMoned.DataSource = DSTDdl.Tables[3];
                DdlMoned.DataTextField = "Descripcion";
                DdlMoned.DataValueField = "CodTipoMoneda";
                DdlMoned.DataBind();
                DdlMoned.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["Estado"].Rows.Count > 0)
            {
                VbCodAnt = DdlEstd.Text.Trim();
                DdlEstd.DataSource = DSTDdl.Tables[4];
                DdlEstd.DataTextField = "Descripcion";
                DdlEstd.DataValueField = "CodEstadoCot";
                DdlEstd.DataBind();
                DdlEstd.Text = VbCodAnt.Trim().Equals("") ? "01" : VbCodAnt;
            }
            if (DSTDdl.Tables["TipoPago"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[5].Select("Activo=1 OR CodTipoPago= '" + ViewState["TipoPagoAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlTipoPago.DataSource = DT;
                DdlTipoPago.DataTextField = "Descripcion";
                DdlTipoPago.DataValueField = "CodTipoPago";
                DdlTipoPago.DataBind();
                DdlTipoPago.SelectedValue = ViewState["TipoPagoAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["LugarEntrg"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[6].Select("Activo=1 OR CodIdTipoUbicaCia= '" + ViewState["LugarEAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlLugarEntrg.DataSource = DT;
                DdlLugarEntrg.DataTextField = "Descripcion";
                DdlLugarEntrg.DataValueField = "CodIdTipoUbicaCia";
                DdlLugarEntrg.DataBind();
                DdlLugarEntrg.SelectedValue = ViewState["LugarEAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["MedioCot"].Rows.Count > 0)
            {
                VbCodAnt = DdlMedioCot.Text.Trim();
                DdlMedioCot.DataSource = DSTDdl.Tables[7];
                DdlMedioCot.DataTextField = "Descripcion";
                DdlMedioCot.DataValueField = "CodMedioCotiza";
                DdlMedioCot.DataBind();
                DdlMedioCot.Text = VbCodAnt;
            }
        }
        protected void Traerdatos(string IdCotiza, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC PNTLL_Cotizac 1, @Tpc,'','','','','Ppal',@Id,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Tpc", ViewState["TipoCotiza"]);
                            SC.Parameters.AddWithValue("@Id", IdCotiza.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "Cotiza";
                                    DSTPpl.Tables[1].TableName = "DetCotiza";

                                    ViewState["DSTPpl"] = DSTPpl;
                                }
                            }
                        }
                    }
                }
                TxtBusqPn.Text = "";
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataRow[] Result;
                if (DSTPpl.Tables["Cotiza"].Rows.Count > 0)
                {
                    string VbFecSt;
                    DateTime? VbFecDT;
                    TxtNumCotiza.Text = DSTPpl.Tables[0].Rows[0]["CodCotizacion"].ToString().Trim();
                    TxtNumPetcn.Text = DSTPpl.Tables[0].Rows[0]["PeticionEC"].ToString().Trim();
                    TxtNumDocum.Text = DSTPpl.Tables[0].Rows[0]["Documento"].ToString().Trim();
                    TxtDatosPpt.Text = DSTPpl.Tables[0].Rows[0]["PPT"].ToString().Trim();
                    ViewState["CodTerceroAnt"] = DSTPpl.Tables[0].Rows[0]["CodProveedor"].ToString().Trim();
                    ViewState["ContactoAnt"] = DSTPpl.Tables[0].Rows[0]["Contacto"].ToString().Trim();
                    ViewState["TipoPagoAnt"] = DSTPpl.Tables[0].Rows[0]["CodTipoPago"].ToString().Trim();
                    ViewState["LugarEAnt"] = DSTPpl.Tables[0].Rows[0]["LugarEntrega"].ToString().Trim();
                    DdlTipoCot.Text = DSTPpl.Tables[0].Rows[0]["CodTipoCotizacion"].ToString().Trim();
                    ViewState["CodTipoCotizacion_ANT"] = DSTPpl.Tables[0].Rows[0]["CodTipoCotizacion"].ToString().Trim();
                    DdlMoned.Text = DSTPpl.Tables[0].Rows[0]["CodMoneda"].ToString().Trim();
                    DdlEstd.Text = DSTPpl.Tables[0].Rows[0]["CodEstadoCot"].ToString().Trim();
                    DdlMedioCot.Text = DSTPpl.Tables[0].Rows[0]["CodMedioCotizacion"].ToString().Trim();
                    TxtObsrv.Text = DSTPpl.Tables[0].Rows[0]["Observacion"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaCotiza"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaCotiza"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFechCot.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaPlazo"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaPlazo"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFechPlazRes.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaRespt"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaRespt"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFechRespt.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaVigenc"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaVigenc"].ToString().Trim();
                    ViewState["FechTrmAnt"] = VbFecSt;
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFechVigc.Text = string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    TxtSubTtal.Text = DSTPpl.Tables[0].Rows[0]["MontoT"].ToString().Trim();
                    ViewState["Monto"] = DSTPpl.Tables[0].Rows[0]["Monto"].ToString().Trim();
                    TxtIVA.Text = DSTPpl.Tables[0].Rows[0]["ValorIvaT"].ToString().Trim();
                    ViewState["ValorIva"] = DSTPpl.Tables[0].Rows[0]["ValorIva"].ToString().Trim();
                    TxtOtrImpt.Text = DSTPpl.Tables[0].Rows[0]["ValorOtrosImpuestosT"].ToString().Trim();
                    ViewState["ValorOtrosImp"] = DSTPpl.Tables[0].Rows[0]["ValorOtrosImpuestos"].ToString().Trim();
                    TxtTtl.Text = DSTPpl.Tables[0].Rows[0]["ValorTotalCotT"].ToString().Trim();
                    ViewState["ValorTotal"] = DSTPpl.Tables[0].Rows[0]["ValorTotalCot"].ToString().Trim();
                    VbFecSt = DSTPpl.Tables[0].Rows[0]["FechaTRM"].ToString().Trim().Equals("") ? "01/01/1900" : DSTPpl.Tables[0].Rows[0]["FechaTRM"].ToString().Trim();
                    VbFecDT = Convert.ToDateTime(VbFecSt);
                    TxtFechTRM.Text = VbFecSt.Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
                    TxtTRM.Text = DSTPpl.Tables[0].Rows[0]["TRMT"].ToString().Trim();
                    ViewState["TRM_ANT"] = DSTPpl.Tables[0].Rows[0]["TRM"].ToString().Trim();
                    TxtSnRepa.Text = DSTPpl.Tables[0].Rows[0]["SN"].ToString().Trim();

                    ViewState["TtlRegDet"] = Convert.ToInt32(DSTPpl.Tables[0].Rows[0]["TtlRegDet"].ToString());
                    ViewState["CarpetaCargaMasiva"] = HttpUtility.HtmlDecode(DSTPpl.Tables[0].Rows[0]["CargaMasiva"].ToString().Trim());
                    ViewState["Cancelada"] = DSTPpl.Tables[0].Rows[0]["Cancelada"].ToString().Trim();
                    ViewState["DocAprobado"] = DSTPpl.Tables[0].Rows[0]["DocAprobado"].ToString().Trim();
                    ViewState["ShipLiquidada"] = DSTPpl.Tables[0].Rows[0]["ShipLiquidada"].ToString().Trim();
                    ViewState["PeriodCerrado"] = DSTPpl.Tables[0].Rows[0]["PeriodCerrado"].ToString().Trim();
                    ViewState["TieneSOMvtoAlma"] = DSTPpl.Tables[0].Rows[0]["TieneSOMvtoAlma"].ToString().Trim();

                    ViewState["TxtAprob"] = "";

                    Result = Idioma.Select("Objeto= 'Mens16Cotz'");// El documento se encuentra aprobado.
                    foreach (DataRow row in Result)
                    { ViewState["TxtAprob"] = row["Texto"].ToString().Trim(); }

                    ViewState["TxtPerdCerr"] = "";
                    Result = Idioma.Select("Objeto= 'Mens18Cotz'");// El período contable del ingreso al almacén se encuentra cerrado.
                    foreach (DataRow row in Result)
                    { ViewState["TxtPerdCerr"] = row["Texto"].ToString().Trim(); }

                    ViewState["TxtSOLiqud"] = "";
                    Result = Idioma.Select("Objeto= 'Mens17Cotz'");// Orden Embarque LIquidada.
                    foreach (DataRow row in Result)
                    { ViewState["TxtSOLiqud"] = row["Texto"].ToString().Trim(); }

                    ViewState["TxtTieneSOMvtAlma"] = "";
                    Result = Idioma.Select("Objeto= 'Mens19Cotz'");// Se encuentra con registros asignados a una orden de embarque o tiene movimientos en el almacén.
                    foreach (DataRow row in Result)
                    { ViewState["TxtTieneSOMvtAlma"] = row["Texto"].ToString().Trim(); }

                    if ((ViewState["DocAprobado"].ToString().Trim().Equals("S") || ViewState["ShipLiquidada"].ToString().Trim().Equals("S") || ViewState["PeriodCerrado"].ToString().Trim().Equals("S")) &&
                        !Session["MonLcl"].ToString().Trim().Equals(DdlMoned.Text))
                    {
                        if (ViewState["DocAprobado"].ToString().Trim().Equals("S"))
                        { TxtFechTRM.ToolTip = ViewState["TxtAprob"].ToString(); TxtTRM.ToolTip = ViewState["TxtAprob"].ToString(); }

                        if (ViewState["ShipLiquidada"].ToString().Trim().Equals("S"))
                        { TxtFechTRM.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim(); TxtTRM.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim(); }

                        if (ViewState["PeriodCerrado"].ToString().Trim().Equals("S"))
                        { TxtFechTRM.ToolTip = ViewState["TxtPerdCerr"].ToString(); TxtTRM.ToolTip = ViewState["TxtPerdCerr"].ToString(); }
                    }
                    else { TxtFechTRM.ToolTip = ""; TxtTRM.ToolTip = ""; }

                    if (ViewState["DocAprobado"].ToString().Trim().Equals("S") || ViewState["PeriodCerrado"].ToString().Trim().Equals("S") || ViewState["TieneSOMvtoAlma"].ToString().Trim().Equals("S"))
                    {
                        if (ViewState["TieneSOMvtoAlma"].ToString().Trim().Equals("S"))
                        { DdlProvee.ToolTip = ViewState["TxtTieneSOMvtAlma"].ToString(); DdlMoned.ToolTip = ViewState["TxtTieneSOMvtAlma"].ToString(); }

                        if (ViewState["DocAprobado"].ToString().Trim().Equals("S"))
                        { DdlProvee.ToolTip = ViewState["TxtAprob"].ToString(); DdlMoned.ToolTip = ViewState["TxtAprob"].ToString(); TxtOtrImpt.ToolTip = ViewState["TxtAprob"].ToString(); }

                        if (ViewState["PeriodCerrado"].ToString().Trim().Equals("S"))
                        { DdlProvee.ToolTip = ViewState["TxtPerdCerr"].ToString(); DdlMoned.ToolTip = ViewState["TxtPerdCerr"].ToString(); TxtOtrImpt.ToolTip = ViewState["TxtPerdCerr"].ToString(); }
                    }
                    else { DdlProvee.ToolTip = ""; DdlMoned.ToolTip = ""; TxtOtrImpt.ToolTip = ""; }

                    if (DdlEstd.Text.ToString().Trim().Equals("01") && (int)ViewState["TtlRegDet"] > 0) { BtnCargaMaxiva.Visible = true; }// Solo se puede cargar masivamente 
                    else { BtnCargaMaxiva.Visible = false; }

                    Result = Idioma.Select("Objeto= 'BtnCargaMasivaTT1'");
                    foreach (DataRow row in Result)
                    { BtnCargaMaxiva.ToolTip = row["Texto"].ToString() + " " + ViewState["CarpetaCargaMasiva"].ToString() + ViewState["NomArchivoCM"].ToString(); }

                    BindBDdl("SEL");
                }
                if (DSTPpl.Tables["DetCotiza"].Rows.Count > 0)
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    DataRow[] DR = DSTPpl.Tables[1].Select("Vista <>''");
                    if (IsIENumerableLleno(DR))
                    { TblDetalle = DR.CopyToDataTable(); TblDetalle.AcceptChanges(); ViewState["TblDetalle"] = TblDetalle; }
                }
                else { TblDetalle.Clear(); TblDetalle.AcceptChanges(); AddCamposDataTable("INS"); ; }
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
        protected void LimpiarCampos(string Accion)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            TxtNumCotiza.Text = "";
            TxtNumPetcn.Text = "0";
            TxtNumDocum.Text = "";
            TxtDatosPpt.Text = "";
            DdlProvee.Text = "";
            DdlContact.Text = "";
            DdlMoned.Text = "";
            DdlEstd.Text = "01";
            DdlTipoPago.Text = "";
            DdlLugarEntrg.Text = "";
            DdlMedioCot.Text = "";
            TxtObsrv.Text = "";
            TxtFechCot.Text = "";
            TxtFechPlazRes.Text = "";
            TxtFechRespt.Text = "";
            TxtFechVigc.Text = "";
            TxtFechTRM.Text = "";
            TxtTRM.Text = "0";
            TxtSubTtal.Text = "0";
            TxtIVA.Text = "0";
            TxtOtrImpt.Text = "0";
            TxtTtl.Text = "0";
            ViewState["IdCotiza"] = "0";
            TblDetalle.Clear();
            TblDetalle.AcceptChanges();
            BindDDetTmp();

        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (DdlProvee.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el proveedor.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipoCot.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el tipo.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlMoned.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la moneda.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipoPago.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo de pago.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlLugarEntrg.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el lugar de entrega.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlMedioCot.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el medio de la cotización.
                ViewState["Validar"] = "N"; return;
            }

            if (TxtFechPlazRes.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens07Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una la fecha de plazo de la respuesta.
                ViewState["Validar"] = "N"; TxtFechPlazRes.Focus(); return;
            }
            string Mensj;
            if (!TxtFechPlazRes.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechPlazRes.Text.Trim(), "", 1);

                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechPlazRes.Focus();
                    ViewState["Validar"] = "N"; return;
                }
            }

            if (!TxtFechRespt.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechRespt.Text.Trim(), "", 1);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechRespt.Focus();
                    ViewState["Validar"] = "N"; return;
                }
            }

            if (!TxtFechVigc.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechVigc.Text.Trim(), "", 1);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechVigc.Focus();
                    ViewState["Validar"] = "N"; return;
                }
            }

            if (!TxtFechTRM.Text.Trim().Equals(""))
            {
                Cnx.ValidarFechas(TxtFechTRM.Text.Trim(), "", 1);
                Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString(); TxtFechTRM.Focus();
                    ViewState["Validar"] = "N"; return;
                }
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr, string Accion)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            BtnExportar.Enabled = Otr;
            BtnOpenSolPed.Enabled = Otr;
            BtnConsultar.Enabled = Otr;
            BtnCompra.Enabled = Otr;
            BtnRepa.Enabled = Otr;
            BtnInterc.Enabled = Otr;
            if (Accion.Equals("UPD"))
            {
                if ((int)ViewState["TtlRegDet"] > 0 && ViewState["DocAprobado"].ToString().Trim().Equals("N") && ViewState["ShipLiquidada"].ToString().Trim().Equals("N") && ViewState["PeriodCerrado"].ToString().Trim().Equals("N"))
                { BtnCargaMaxiva.Enabled = Otr == true ? false : true; }
            }
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            DdlContact.Enabled = Edi;
            DdlTipoPago.Enabled = Edi;
            DdlLugarEntrg.Enabled = Edi;
            DdlMedioCot.Enabled = Edi;
            TxtObsrv.Enabled = Edi;
            TxtFechPlazRes.Enabled = Edi;
            TxtFechRespt.Enabled = Edi;
            TxtFechVigc.Enabled = Edi;
            EnablGridDet("Enabled", Edi);
            if (Accion.Equals("UPD"))
            {
                DdlEstd.Enabled = Edi;
                if (!ViewState["DocAprobado"].ToString().Trim().Equals("S") || !ViewState["PeriodCerrado"].ToString().Trim().Equals("S") || !ViewState["TieneSOMvtoAlma"].ToString().Trim().Equals("S"))
                {
                    DdlProvee.Enabled = Edi; DdlMoned.Enabled = Edi; TxtOtrImpt.Enabled = Edi;
                    if (ViewState["TieneSOMvtoAlma"].ToString().Trim().Equals("S")) { DdlProvee.Enabled = false; DdlMoned.Enabled = false; }
                }

                if (!ViewState["DocAprobado"].ToString().Trim().Equals("S") && !ViewState["ShipLiquidada"].ToString().Trim().Equals("S") && !ViewState["PeriodCerrado"].ToString().Trim().Equals("S") &&
                    !Session["MonLcl"].ToString().Trim().Equals(DdlMoned.Text.Trim()))
                { TxtFechTRM.Enabled = Edi; TxtTRM.Enabled = Edi; }
            }
            else { TxtFechTRM.Enabled = Edi; TxtTRM.Enabled = Edi; DdlProvee.Enabled = Edi; DdlMoned.Enabled = Edi; TxtOtrImpt.Enabled = Edi; }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false, "INS");

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
                    TxtFechCot.Text = string.Format("{0:yyyy-MM-dd}", VbFecID);

                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }
                    string VbTipoCotiza;
                    switch (ViewState["TipoCotiza"].ToString().Trim())
                    {
                        case "01": // COMPRA
                            VbTipoCotiza = "C"; break;
                        case "02": // REPA
                            VbTipoCotiza = "P"; break;
                        default: // INTERCAMBIO
                            VbTipoCotiza = "I"; break;
                    }
                    DateTime? VbFecPlzRsp, VbFecRsp, VbFecVig, VbFTRM;
                    string VbDiaT, VbMesT, VbAnoT;
                    if (TxtFechPlazRes.Text.Trim().Equals("")) { VbFecPlzRsp = null; }
                    else { VbFecPlzRsp = Convert.ToDateTime(TxtFechPlazRes.Text); }

                    if (TxtFechRespt.Text.Trim().Equals("")) { VbFecRsp = null; }
                    else { VbFecRsp = Convert.ToDateTime(TxtFechRespt.Text); }

                    if (TxtFechVigc.Text.Trim().Equals("")) { VbFecVig = null; }
                    else { VbFecVig = Convert.ToDateTime(TxtFechVigc.Text); }

                    if (TxtFechTRM.Text.Trim().Equals("")) { VbFTRM = null; VbDiaT = ""; VbMesT = ""; VbAnoT = ""; }
                    else
                    {
                        VbFTRM = Convert.ToDateTime(TxtFechTRM.Text);
                        VbDiaT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Day);
                        VbMesT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Month);
                        VbAnoT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Year);
                    }

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
                    List<ClsTypCotizacion> ObjEncCot = new List<ClsTypCotizacion>();
                    var TypEncCot = new ClsTypCotizacion()
                    {
                        IdCotizacion = Convert.ToInt32(0),
                        CodCotizacion = "",
                        CodTipoCotizacion = DdlTipoCot.Text.Trim(),
                        CodProveedor = DdlProvee.Text.Trim(),
                        FechaSolicitudPet = Convert.ToDateTime(TxtFechCot.Text.Trim()),
                        FechaMaxRespuesta = VbFecPlzRsp,
                        FechaRespuesta = VbFecRsp,
                        FechaVigenciaCot = VbFecVig,
                        CodTipoPeticion = ViewState["TipoCotiza"].ToString().Trim(),
                        ValorTotalCot = Convert.ToDouble(ViewState["ValorTotal"]),
                        CodMoneda = DdlMoned.Text.Trim(),
                        Monto = Convert.ToDouble(ViewState["Monto"]),
                        ValorBruto = Convert.ToDouble(0),
                        DiaTasa = VbDiaT,
                        MesTasa = VbMesT,
                        AñoTasa = VbAnoT,
                        TrmAcordado = Convert.ToDouble(TxtTRM.Text.Trim().Equals("") ? "0" : TxtTRM.Text.Trim()),
                        TrmAcordado_Ant = Convert.ToDouble(TxtTRM.Text.Trim().Equals("") ? "0" : TxtTRM.Text.Trim()),
                        CodTipoPago = DdlTipoPago.Text.Trim(),
                        ValorIva = Convert.ToDouble(ViewState["ValorIva"]),
                        TasaIva = Convert.ToDouble(0),
                        ValorIca = Convert.ToDouble(0),
                        TasaIca = Convert.ToDouble(0),
                        ValorRetencion = Convert.ToDouble(0),
                        TasaRetencion = Convert.ToDouble(0),
                        ValorOtrosImpuestos = Convert.ToDouble(ViewState["ValorOtrosImp"]),
                        CodEstadoCot = DdlEstd.Text.Trim(),
                        Aprobado = Convert.ToInt32(0),
                        ValorDescuento = Convert.ToDouble(0),
                        TasaDescuento = Convert.ToDouble(0),
                        Contacto = DdlContact.Text.Trim(),
                        LugarEntrega = DdlLugarEntrg.Text.Trim(),
                        CodCondicionElem = "",
                        Observacion = TxtObsrv.Text.Trim(),
                        TipoCotiza = VbTipoCotiza.Trim(),
                        CodMedioCotizacion = DdlMedioCot.Text.Trim(),
                        CodTipoCodigo = "01",
                        PeticionEC = Convert.ToInt32(TxtNumPetcn.Text.Trim()),
                        IdConfigCia = Convert.ToInt32(Session["!dC!@"]),
                        FechaTRM = VbFTRM,
                        FechaTRM_Ant = VbFTRM,
                        CodProveedor_ANT = ViewState["CodTerceroAnt"].ToString().Trim(),
                        CodTipoCotizacion_ANT = ViewState["CodTipoCotizacion_ANT"].ToString().Trim(),

                    };
                    ObjEncCot.Add(TypEncCot);

                    Valores();
                    GrdDet.DataSource = TblDetalle;
                    GrdDet.DataBind();

                    List<ClsTypCotizacion> ObjDetCot = new List<ClsTypCotizacion>();
                    foreach (GridViewRow Row in GrdDet.Rows)
                    {
                        string VbSCant = (Row.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCant") as TextBox).Text.Trim();
                        double VbDCant = Convert.ToDouble(VbSCant) < 0 ? 0 : Convert.ToDouble(VbSCant);

                        string VbSIVA = (Row.FindControl("TxtTsIVA") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtTsIVA") as TextBox).Text.Trim();
                        double VbDIVA = Convert.ToDouble(VbSIVA) < 0 ? 0 : Convert.ToDouble(VbSIVA);

                        string VbSTmEnt = (Row.FindControl("TxtTiempEntr") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtTiempEntr") as TextBox).Text.Trim();
                        int VbSImEnt = Convert.ToInt32(VbSTmEnt) < 0 ? 0 : Convert.ToInt32(VbSTmEnt);

                        string VbSMinC = (Row.FindControl("TxtUndMinCompra") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtUndMinCompra") as TextBox).Text.Trim();
                        double VbDMinC = Convert.ToDouble(VbSMinC) < 0 ? 0 : Convert.ToDouble(VbSMinC);

                        if (!(Row.FindControl("DdlPN") as DropDownList).Text.Trim().Equals(""))
                        {
                            var TypDetCot = new ClsTypCotizacion()
                            {
                                IdDetCotizacion = Convert.ToInt32(0),
                                IdCotizacion = Convert.ToInt32(0),
                                IdDetPedido = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim()),
                                PosDC = Convert.ToInt32(0),
                                Pn = (Row.FindControl("DdlPN") as DropDownList).Text.Trim(),
                                Monto = Convert.ToDouble(0),
                                ValorIVA = Convert.ToDouble((Row.FindControl("LblVlrIVA") as Label).Text.Trim()),
                                TasaIVA = VbDIVA,
                                ValorTotal = Convert.ToDouble((Row.FindControl("LblVlrTtl") as Label).Text.Trim()),
                                Cantidad = VbDCant,
                                CodUndMed = (Row.FindControl("DdlUM") as DropDownList).Text.Trim(),
                                ValorUnidad = Convert.ToDouble((Row.FindControl("TxtVlor") as TextBox).Text.Trim()),
                                Aprobacion = Convert.ToInt32(0),
                                CodMedioCotiza = "",
                                CodDetEstadoCotiza = "",
                                TiempoEntrega = VbSImEnt,
                                CodEstdo = (Row.FindControl("DdlEstdElem") as DropDownList).Text.Trim(),
                                UndMinimaCompra = VbDMinC,
                                Alterno = (Row.FindControl("TxtAlterno") as TextBox).Text.Trim(),
                                ObservacionesDC = "",
                                TiempEntregaPropuesta = Convert.ToInt32(0),
                                PorcAlMonto = Convert.ToDouble(0),
                                PorcAlimpuesto = Convert.ToDouble(0),
                                ValorUnidadP = Convert.ToDouble(0),
                                ValorUnidadPExp = Convert.ToDouble(0),
                                GarantiaDC = Convert.ToInt32(0),
                                CodAeronaveCT = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["CodAeronaveCT"].ToString().Trim()),
                                SN = GrdDet.DataKeys[Row.RowIndex].Values["Sn"].ToString().Trim(),
                                IdConfigCia = Convert.ToInt32(Session["!dC!@"]),
                                IdDetPedido_Ant = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim()),
                                Pn_Ant = GrdDet.DataKeys[Row.RowIndex].Values["PN"].ToString().Trim(),
                                Cantidad_Ant = VbDCant,//Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["Cantidad"].ToString().Trim()),
                                CodUndMed_Ant = (Row.FindControl("DdlUM") as DropDownList).Text.Trim(),  //GrdDet.DataKeys[Row.RowIndex].Values["CodUndMed"].ToString().Trim(),
                                ValorUnidad_Ant = Convert.ToDouble((Row.FindControl("TxtVlor") as TextBox).Text.Trim()),//Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["ValorUnidad"].ToString().Trim()),
                                TasaIVA_Ant = VbDIVA,//Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["TasaIva"].ToString().Trim()),
                                AccionDet = "INS",

                            };
                            ObjDetCot.Add(TypDetCot);
                        }
                    }

                    ClsTypCotizacion ClsCotiza = new ClsTypCotizacion();
                    ClsCotiza.Accion("INSERT");
                    ClsCotiza.Alimentar(ObjEncCot, ObjDetCot);
                    string Mensj = ClsCotiza.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        string VbPn = ClsCotiza.GetPN().Trim().Equals("") ? "" : "  P/N: [" + ClsCotiza.GetPN().Trim() + "]";
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VbPn + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, "INS");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    ViewState["IdCotiza"] = ClsCotiza.GetIdCotiza();
                    Traerdatos(ViewState["IdCotiza"].ToString().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                    ViewState["CodTipoCotizacion_ANT"] = DdlTipoCot.Text.Trim();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Cotizacion", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNumCotiza.Text.Equals(""))
                { return; }

                if (ViewState["Cancelada"].ToString().Trim().Equals("03"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens15Cotz'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                    return;
                }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false, "UPD");
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPD");

                    if (DdlMoned.Text.Trim().Equals(Session["MonLcl"].ToString().Trim())) { TxtFechTRM.Enabled = false; TxtFechTRM.Text = ""; TxtTRM.Text = "1"; TxtTRM.Enabled = false; }

                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                    EnablGridDet("Enabled", true);
                }
                else
                {
                    TblDetalle = (DataTable)ViewState["TblDetalle"];
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N")) { return; }

                    string VbTipoCotiza;
                    switch (ViewState["TipoCotiza"].ToString().Trim())
                    {
                        case "01": // COMPRA
                            VbTipoCotiza = "C"; break;
                        case "02": // REPA
                            VbTipoCotiza = "P"; break;
                        default: // INTERCAMBIO
                            VbTipoCotiza = "I"; break;
                    }

                    DateTime? VbFecPlzRsp, VbFecRsp, VbFecVig, VbFTRM, VbFTRM_Ant;
                    string VbDiaT, VbMesT, VbAnoT;
                    if (TxtFechPlazRes.Text.Trim().Equals("")) { VbFecPlzRsp = null; }
                    else { VbFecPlzRsp = Convert.ToDateTime(TxtFechPlazRes.Text); }

                    if (TxtFechRespt.Text.Trim().Equals("")) { VbFecRsp = null; }
                    else { VbFecRsp = Convert.ToDateTime(TxtFechRespt.Text); }

                    if (TxtFechVigc.Text.Trim().Equals("")) { VbFecVig = null; }
                    else { VbFecVig = Convert.ToDateTime(TxtFechVigc.Text); }

                    if (TxtFechTRM.Text.Trim().Equals("")) { VbFTRM = null; VbDiaT = ""; VbMesT = ""; VbAnoT = ""; }
                    else
                    {
                        VbFTRM = Convert.ToDateTime(TxtFechTRM.Text);
                        VbDiaT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Day);
                        VbMesT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Month);
                        VbAnoT = Convert.ToString(Convert.ToDateTime(TxtFechTRM.Text).Year);
                    }

                    if (ViewState["FechTrmAnt"].ToString().Equals("01/01/1900")) { VbFTRM_Ant = null; }
                    else { VbFTRM_Ant = Convert.ToDateTime(ViewState["FechTrmAnt"].ToString()); }

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
                    List<ClsTypCotizacion> ObjEncCot = new List<ClsTypCotizacion>();
                    var TypEncCot = new ClsTypCotizacion()
                    {
                        IdCotizacion = Convert.ToInt32(ViewState["IdCotiza"]),
                        CodCotizacion = TxtNumCotiza.Text.Trim(),
                        CodTipoCotizacion = DdlTipoCot.Text.Trim(),
                        CodProveedor = DdlProvee.Text.Trim(),
                        FechaSolicitudPet = Convert.ToDateTime(TxtFechCot.Text.Trim()),
                        FechaMaxRespuesta = VbFecPlzRsp,
                        FechaRespuesta = VbFecRsp,
                        FechaVigenciaCot = VbFecVig,
                        CodTipoPeticion = ViewState["TipoCotiza"].ToString().Trim(),
                        ValorTotalCot = Convert.ToDouble(ViewState["ValorTotal"]),
                        CodMoneda = DdlMoned.Text.Trim(),
                        Monto = Convert.ToDouble(ViewState["Monto"]),
                        ValorBruto = Convert.ToDouble(0),
                        DiaTasa = VbDiaT,
                        MesTasa = VbMesT,
                        AñoTasa = VbAnoT,
                        TrmAcordado = Convert.ToDouble(TxtTRM.Text.Trim().Equals("") ? "0" : TxtTRM.Text.Trim()),
                        TrmAcordado_Ant = Convert.ToDouble(ViewState["TRM_ANT"]),
                        CodTipoPago = DdlTipoPago.Text.Trim(),
                        ValorIva = Convert.ToDouble(ViewState["ValorIva"]),
                        TasaIva = Convert.ToDouble(0),
                        ValorIca = Convert.ToDouble(0),
                        TasaIca = Convert.ToDouble(0),
                        ValorRetencion = Convert.ToDouble(0),
                        TasaRetencion = Convert.ToDouble(0),
                        ValorOtrosImpuestos = Convert.ToDouble(ViewState["ValorOtrosImp"]),
                        CodEstadoCot = DdlEstd.Text.Trim(),
                        Aprobado = Convert.ToInt32(0),
                        ValorDescuento = Convert.ToDouble(0),
                        TasaDescuento = Convert.ToDouble(0),
                        Contacto = DdlContact.Text.Trim(),
                        LugarEntrega = DdlLugarEntrg.Text.Trim(),
                        CodCondicionElem = "",
                        Observacion = TxtObsrv.Text.Trim(),
                        TipoCotiza = VbTipoCotiza.Trim(),
                        CodMedioCotizacion = DdlMedioCot.Text.Trim(),
                        CodTipoCodigo = "01",
                        PeticionEC = Convert.ToInt32(TxtNumPetcn.Text.Trim()),
                        IdConfigCia = Convert.ToInt32(Session["!dC!@"]),
                        FechaTRM = VbFTRM,
                        FechaTRM_Ant = VbFTRM_Ant,
                        CodProveedor_ANT = ViewState["CodTerceroAnt"].ToString().Trim(),
                        CodTipoCotizacion_ANT = ViewState["CodTipoCotizacion_ANT"].ToString().Trim(),
                    };
                    ObjEncCot.Add(TypEncCot);

                    string VbAccionDet = "SEL";
                    Valores();
                    GrdDet.DataSource = TblDetalle;
                    GrdDet.DataBind();

                    List<ClsTypCotizacion> ObjDetCot = new List<ClsTypCotizacion>();
                    foreach (GridViewRow Row in GrdDet.Rows)
                    {
                        string VbPn = (Row.FindControl("DdlPN") as DropDownList).Text.Trim();
                        string VbPn_Ant = GrdDet.DataKeys[Row.RowIndex].Values["PN"].ToString().Trim();
                        if (!VbPn.Equals(VbPn_Ant)) { VbAccionDet = "UPD"; }

                        string VbSCant = (Row.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCant") as TextBox).Text.Trim();
                        double VbDCant = Convert.ToDouble(VbSCant) < 0 ? 0 : Convert.ToDouble(VbSCant);
                        double VbDCant_Ant = Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["Cantidad"].ToString().Trim());
                        if (!VbDCant.Equals(VbDCant_Ant)) { VbAccionDet = "UPD"; }

                        string VbUndMd = (Row.FindControl("DdlUM") as DropDownList).Text.Trim();
                        string VbUndMd_Ant = GrdDet.DataKeys[Row.RowIndex].Values["CodUndMed"].ToString().Trim();
                        if (!VbUndMd.Equals(VbUndMd_Ant)) { VbAccionDet = "UPD"; }

                        string VbSIVA = (Row.FindControl("TxtTsIVA") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtTsIVA") as TextBox).Text.Trim();
                        double VbDIVA = Convert.ToDouble(VbSIVA) < 0 ? 0 : Convert.ToDouble(VbSIVA);
                        double VbDIVA_Ant = Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["Cantidad"].ToString().Trim());
                        if (!VbDIVA.Equals(VbDIVA_Ant)) { VbAccionDet = "UPD"; }

                        string VbStdo = (Row.FindControl("DdlEstdElem") as DropDownList).Text.Trim();
                        string VbStdo_Ant = GrdDet.DataKeys[Row.RowIndex].Values["CodEstdo"].ToString().Trim();
                        if (!VbStdo.Equals(VbStdo_Ant)) { VbAccionDet = "UPD"; }

                        string VbSTmEnt = (Row.FindControl("TxtTiempEntr") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtTiempEntr") as TextBox).Text.Trim();
                        string VbSTmEnt_Ant = GrdDet.DataKeys[Row.RowIndex].Values["TiempoEntrega"].ToString().Trim();
                        int VbSImEnt = Convert.ToInt32(VbSTmEnt) < 0 ? 0 : Convert.ToInt32(VbSTmEnt);
                        if (!VbSTmEnt.Equals(VbSTmEnt_Ant)) { VbAccionDet = "UPD"; }

                        string VbSMinC = (Row.FindControl("TxtUndMinCompra") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtUndMinCompra") as TextBox).Text.Trim();
                        string VbSMinC_Ant = GrdDet.DataKeys[Row.RowIndex].Values["UndMinimaCompra"].ToString().Trim();
                        double VbDMinC = Convert.ToDouble(VbSMinC) < 0 ? 0 : Convert.ToDouble(VbSMinC);
                        if (!VbSMinC.Equals(VbSMinC_Ant)) { VbAccionDet = "UPD"; }

                        string VbSPnAltrn = (Row.FindControl("TxtAlterno") as TextBox).Text.Trim();
                        string VbSPnAltrn_Ant = GrdDet.DataKeys[Row.RowIndex].Values["Alterno"].ToString().Trim();
                        if (!VbSPnAltrn.Equals(VbSPnAltrn_Ant)) { VbAccionDet = "UPD"; }

                        int VblIdDetCotiza = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetCotizacion"].ToString().Trim());
                        if (VblIdDetCotiza == 0) { VbAccionDet = "INS"; }

                        if (!VbPn.Equals(""))
                        {
                            var TypDetCot = new ClsTypCotizacion()
                            {
                                IdDetCotizacion = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetCotizacion"].ToString().Trim()),
                                IdCotizacion = Convert.ToInt32(ViewState["IdCotiza"]),
                                IdDetPedido = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim()),
                                PosDC = Convert.ToInt32((Row.FindControl("LblPosc") as Label).Text.Trim()),
                                Pn = (Row.FindControl("DdlPN") as DropDownList).Text.Trim(),
                                Monto = Convert.ToDouble(0),
                                ValorIVA = Convert.ToDouble((Row.FindControl("LblVlrIVA") as Label).Text.Trim()),
                                TasaIVA = VbDIVA,
                                ValorTotal = Convert.ToDouble((Row.FindControl("LblVlrTtl") as Label).Text.Trim()),
                                Cantidad = VbDCant,
                                CodUndMed = (Row.FindControl("DdlUM") as DropDownList).Text.Trim(),
                                ValorUnidad = Convert.ToDouble((Row.FindControl("TxtVlor") as TextBox).Text.Trim()),
                                Aprobacion = (Row.FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0,
                                CodMedioCotiza = "",
                                CodDetEstadoCotiza = "",
                                TiempoEntrega = VbSImEnt,
                                CodEstdo = (Row.FindControl("DdlEstdElem") as DropDownList).Text.Trim(),
                                UndMinimaCompra = VbDMinC,
                                Alterno = VbSPnAltrn,
                                ObservacionesDC = GrdDet.DataKeys[Row.RowIndex].Values["ObservacionesDC"].ToString().Trim(),
                                TiempEntregaPropuesta = Convert.ToInt32(0),
                                PorcAlMonto = Convert.ToDouble(0),
                                PorcAlimpuesto = Convert.ToDouble(0),
                                ValorUnidadP = Convert.ToDouble(0),
                                ValorUnidadPExp = Convert.ToDouble(0),
                                GarantiaDC = Convert.ToInt32(0),
                                CodAeronaveCT = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["CodAeronaveCT"].ToString().Trim()),
                                SN = GrdDet.DataKeys[Row.RowIndex].Values["Sn"].ToString().Trim(),
                                IdConfigCia = Convert.ToInt32(Session["!dC!@"]),
                                IdDetPedido_Ant = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim()),
                                Pn_Ant = GrdDet.DataKeys[Row.RowIndex].Values["PN"].ToString().Trim(),
                                Cantidad_Ant = Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["Cantidad"].ToString().Trim()),
                                CodUndMed_Ant = GrdDet.DataKeys[Row.RowIndex].Values["CodUndMed"].ToString().Trim(),
                                ValorUnidad_Ant = Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["ValorUnidad"].ToString().Trim()),
                                TasaIVA_Ant = Convert.ToDouble(GrdDet.DataKeys[Row.RowIndex].Values["TasaIva"].ToString().Trim()),
                                AccionDet = VbAccionDet,
                            };
                            ObjDetCot.Add(TypDetCot);
                        }
                    }

                    ClsTypCotizacion ClsCotiza = new ClsTypCotizacion();
                    ClsCotiza.Accion("UPDATE");
                    ClsCotiza.Alimentar(ObjEncCot, ObjDetCot);
                    string Mensj = ClsCotiza.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        string VbPn = ClsCotiza.GetPN().Trim().Equals("") ? "" : "  P/N: [" + ClsCotiza.GetPN().Trim() + "]";
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VbPn + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, "UPD");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarCampos(false, false, "INS");
                    Traerdatos(ViewState["IdCotiza"].ToString().Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    EnablGridDet("Enabled", false);
                    ViewState["CodTerceroAnt"] = DdlProvee.Text.Trim();
                    ViewState["CodTipoCotizacion_ANT"] = DdlTipoCot.Text.Trim();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Cotizacion", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnCargaMaxiva_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();

            if (TxtNumCotiza.Text.Equals("")) { return; }

            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataTable DT = new DataTable();
                string conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ViewState["CarpetaCargaMasiva"].ToString().Trim() + ViewState["NomArchivoCM"].ToString().Trim() + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbConnection cnn = new OleDbConnection(conexion))
                {
                    cnn.Open();
                    DataTable dtExcelSchema;
                    dtExcelSchema = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    cnn.Close();

                    cnn.Open();
                    string sql = "SELECT * From [" + SheetName + "]";
                    OleDbCommand command = new OleDbCommand(sql, cnn);
                    OleDbDataAdapter DA = new OleDbDataAdapter(command);

                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow DRExcel in DT.Rows)
                        {
                            foreach (DataRow DRDetCot in TblDetalle.Rows)
                            {
                                if (DRDetCot["PN"].ToString().Trim().Equals(DRExcel["PN"].ToString().Trim()))
                                {
                                    DataRow[] DR = DSTDdl.Tables[10].Select("CodCondicionElem='" + DRExcel["Status_Estado"].ToString().Trim() + "'");
                                    if (IsIENumerableLleno(DR))
                                    { DRDetCot["CodEstdo"] = DRExcel["Status_Estado"].ToString().Trim(); }

                                    DRDetCot["ValorUnidad"] = DRExcel["Value_Valor"].ToString().Trim().Equals("") ? "0" : DRExcel["Value_Valor"].ToString().Trim();
                                    DRDetCot["TiempoEntrega"] = DRExcel["DeliveryTimeDays_TiempoEntregaDias"].ToString().Trim().Equals("") ? "0" : DRExcel["DeliveryTimeDays_TiempoEntregaDias"].ToString().Trim();
                                    DRDetCot["UndMinimaCompra"] = DRExcel["Min_Qty_CantMinima"].ToString().Trim().Equals("") ? "0" : DRExcel["Min_Qty_CantMinima"].ToString().Trim();
                                    DRDetCot["Alterno"] = DRExcel["Alternate_PN_Alterno"].ToString().Trim();
                                    DRDetCot["ObservacionesDC"] = DRExcel["Observations_Observaciones"].ToString().Trim();
                                }
                            }
                        }
                    }
                    cnn.Close();
                    TblDetalle.AcceptChanges();
                    GrdDet.DataSource = TblDetalle; GrdDet.DataBind();
                    Valores();
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (TxtNumCotiza.Text.Equals("")) { return; }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = string.Format("EXEC PNTLL_Cotizac 3,@NCT, @TC,'','',@Usu,'',@IdC,0,0,0, @ICC,'01-01-01','02-01-01','03-01-01'");
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SC.Parameters.AddWithValue("@NCT", TxtNumCotiza.Text.Trim());
                            SC.Parameters.AddWithValue("@TC", ViewState["TipoCotiza"].ToString().Trim());
                            SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@IdC", ViewState["IdCotiza"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            var VbMensj = SC.ExecuteScalar();
                            if (!VbMensj.Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + VbMensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { VbMensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + VbMensj + "')", true);
                                return;
                            }
                            Transac.Commit();
                            ViewState["TtlRegDet"] = 0; LimpiarCampos("DEL"); EnablGridDet("Visible", false); GrdBusq.DataSource = null; GrdBusq.DataBind();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la eliminación')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Cotización", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnOpenSolPed_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/Almacen/FrmSolicitudPedido.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
        protected void BotonesTipoCotiza(string Tipo)
        {
            ViewState["TtlRegDet"] = 0; LimpiarCampos("DEL"); EnablGridDet("Visible", false);
            BtnCompra.CssClass = "btn btn-outline-primary";
            BtnRepa.CssClass = "btn btn-outline-primary";
            BtnInterc.CssClass = "btn btn-outline-primary";
            LblSnRepa.Visible = false; TxtSnRepa.Visible = false; GrdBusq.DataSource = null; GrdBusq.DataBind();
            switch (Tipo)
            {
                case "01":
                    ViewState["TipoCotiza"] = "01";
                    BtnCompra.CssClass = "btn btn-primary"; break;
                case "02":
                    ViewState["TipoCotiza"] = "02";
                    BtnRepa.CssClass = "btn btn-primary";
                    LblSnRepa.Visible = true; TxtSnRepa.Visible = true; break;
                default:
                    ViewState["TipoCotiza"] = "03";
                    BtnInterc.CssClass = "btn btn-primary"; break;
            }
        }
        protected void BtnCompra_Click(object sender, EventArgs e)
        { BotonesTipoCotiza("01"); }
        protected void BtnRepa_Click(object sender, EventArgs e)
        { BotonesTipoCotiza("02"); }
        protected void BtnInterc_Click(object sender, EventArgs e)
        { BotonesTipoCotiza("03"); }
        protected void DdlProvee_TextChanged(object sender, EventArgs e)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Contacto"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTDdl.Tables[1].Select("CodTercero= '" + DdlProvee.Text.Trim() + "' OR CodTercero = ''");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlContact.DataSource = DT;
                DdlContact.DataTextField = "Contacto";
                DdlContact.DataValueField = "Codigo";
                DdlContact.DataBind();
            }
            string VbContacto = "";
            if (DSTDdl.Tables["Tercero"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTDdl.Tables[0].Select("CodTercero= '" + DdlProvee.Text.Trim() + "'");
                foreach (DataRow row in DR)
                {
                    if (Convert.ToInt32(row["FPActivo"].ToString()) == 1) { DdlTipoPago.Text = row["CodTipoPago"].ToString().Trim(); }
                    else { DdlTipoPago.Text = ""; }
                    DdlMoned.Text = row["CodMoneda"].ToString().Trim();

                    if (DdlMoned.Text.Trim().Equals(Session["MonLcl"])) { DdlTipoCot.Text = "N"; TxtFechTRM.Enabled = false; TxtFechTRM.Text = ""; TxtTRM.Enabled = false; TxtTRM.Text = "0"; }
                    else { DdlTipoCot.Text = DdlMoned.Text.Trim().Equals("") ? "" : "I"; TxtFechTRM.Enabled = true; TxtFechTRM.Text = ""; TxtTRM.Enabled = true; }

                    VbContacto = row["Contacto"].ToString().Trim();
                }

                DataTable DTC = new DataTable();
                DataRow[] DRC = DSTDdl.Tables[1].Select("Codigo= '" + VbContacto.Trim() + "' AND CodTercero= '" + DdlProvee.Text.Trim() + "'");
                foreach (DataRow RowC in DRC)
                { DdlContact.Text = RowC["Codigo"].ToString().Trim(); }
            }
        }
        protected void DdlMoned_TextChanged(object sender, EventArgs e)
        {
            if (DdlMoned.Text.Trim().Equals(Session["MonLcl"])) { DdlTipoCot.Text = "N"; TxtFechTRM.Enabled = false; TxtFechTRM.Text = ""; TxtTRM.Enabled = false; TxtTRM.Text = "0"; }
            else { DdlTipoCot.Text = DdlMoned.Text.Trim().Equals("") ? "" : "I"; TxtFechTRM.Enabled = true; TxtFechTRM.Text = ""; TxtTRM.Enabled = true; }
        }
        protected void TxtOtrImpt_TextChanged(object sender, EventArgs e)
        { Valores(); }
        //****************************** Busqueda **************************************
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; TxtBusqueda.Focus(); }
        protected void BIndDBusqSP()
        {
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbOpcion = "COT";

                if (RdbBusqNumCot.Checked == true)
                { VbOpcion = "COT"; }
                if (RdbBusqProvee.Checked == true)
                { VbOpcion = "PRV"; }
                if (RdbBusqPN.Checked == true)
                { VbOpcion = "PN"; }
                if (RdbBusqSN.Checked == true)
                { VbOpcion = "SN"; }

                string VbTxtSql = " EXEC PNTLL_Cotizac 2,@Tpc,@Prmtr,'','','',@Opc,0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {                   
                    SC.Parameters.AddWithValue("@Tpc", ViewState["TipoCotiza"]);
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim());
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
                ViewState["IdCotiza"] = GrdBusq.DataKeys[gvr.RowIndex].Values["IdCotizacion"].ToString();
                Traerdatos(ViewState["IdCotiza"].ToString().Trim(), "UPD");
                MultVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
                EnablGridDet("Visible", true);
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
                    foreach (DataRow RowIdioma in Result) { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
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
        protected void Valores()
        {
            double VbDSubTotal = 0, VbDIva = 0, VbDOtroImp = 0, VbDTtl = 0;
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                foreach (GridViewRow GrdRow in GrdDet.Rows) // se recorre la grid para actualizar la Datatable detalle
                {
                    string VbPN = (GrdRow.FindControl("DdlPN") as DropDownList).Text.Trim();

                    if (!VbPN.Equals(""))
                    {
                        string VbIdDetPd = GrdDet.DataKeys[GrdRow.RowIndex].Values["IdDetPedido"].ToString().Trim();
                        int VbIAprob = (GrdRow.FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0;
                        double VbDCant = Convert.ToDouble((GrdRow.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRow.FindControl("TxtCant") as TextBox).Text.Trim());
                        string VbUndMed = (GrdRow.FindControl("DdlUM") as DropDownList).Text.Trim();
                        double VbValorUnd = Convert.ToDouble((GrdRow.FindControl("TxtVlor") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRow.FindControl("TxtVlor") as TextBox).Text.Trim());
                        double VbIva = Convert.ToDouble((GrdRow.FindControl("TxtTsIVA") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRow.FindControl("TxtTsIVA") as TextBox).Text.Trim());
                        //  double VbVlrIva = Convert.ToDouble((GrdRow.FindControl("LblVlrIVA") as Label).Text);
                        double VbVlrTtl = Convert.ToDouble((GrdRow.FindControl("LblVlrTtl") as Label).Text);
                        string VbEstado = (GrdRow.FindControl("DdlEstdElem") as DropDownList).Text.Trim();
                        int VbTiepEntreg = Convert.ToInt32((GrdRow.FindControl("TxtTiempEntr") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRow.FindControl("TxtTiempEntr") as TextBox).Text.Trim());
                        double VbUnMinComp = Convert.ToDouble((GrdRow.FindControl("TxtUndMinCompra") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRow.FindControl("TxtUndMinCompra") as TextBox).Text.Trim());
                        string VbAlter = (GrdRow.FindControl("TxtAlterno") as TextBox).Text.Trim();

                        double VbVlrIva = (VbDCant * VbValorUnd) * VbIva / 100;
                        (GrdRow.FindControl("LblVlrIVA") as Label).Text = VbVlrIva.ToString().Trim();

                        ViewState["Monto"] = (VbDCant * VbValorUnd) + VbVlrIva;
                        VbDSubTotal = VbDSubTotal + Convert.ToDouble(VbDCant) * Convert.ToDouble(VbValorUnd);
                        VbDIva = VbDIva + Convert.ToDouble(VbVlrIva);
                        VbDTtl = VbDTtl + Convert.ToDouble(ViewState["Monto"]);

                        (GrdRow.FindControl("LblVlrTtl") as Label).Text = ViewState["Monto"].ToString();

                        foreach (DataRow row in TblDetalle.Rows)
                        {
                            if (Convert.ToInt32(row["IdDetPedido"].ToString()) == Convert.ToInt32(VbIdDetPd))
                            {
                                row["Aprobacion"] = VbIAprob;
                                row["Cantidad"] = VbDCant;
                                row["CodUndMed"] = VbUndMed;
                                row["ValorUnidad"] = VbValorUnd;
                                row["TasaIva"] = VbIva;
                                row["ValorIva"] = VbVlrIva;
                                row["ValorTotal"] = VbVlrTtl;
                                row["CodEstdo"] = VbEstado;
                                row["TiempoEntrega"] = VbTiepEntreg;
                                row["UndMinimaCompra"] = VbUnMinComp;
                                row["Alterno"] = VbAlter;
                            }
                        }
                    }
                }

                TblDetalle.AcceptChanges();
                TxtSubTtal.Text = VbDSubTotal.ToString();
                ViewState["Monto"] = VbDSubTotal;

                TxtIVA.Text = VbDIva.ToString();
                ViewState["ValorIva"] = TxtIVA.Text.ToString();

                VbDOtroImp = TxtOtrImpt.Text.Trim().Equals("") ? 0 : Convert.ToDouble(TxtOtrImpt.Text.Trim());
                ViewState["ValorOtrosImp"] = VbDOtroImp.ToString();

                VbDTtl = VbDTtl + VbDOtroImp;
                TxtTtl.Text = VbDTtl.ToString();
                ViewState["ValorTotal"] = VbDTtl.ToString();
            }
        }
        protected void IbtBusqPn_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];

                TblDetalle.DefaultView.RowFilter = "PN LIKE '%" + TxtBusqPn.Text.Trim() + "%'";
                Valores();

                GrdDet.DataSource = TblDetalle;
                GrdDet.DataBind();
            }
        }
        protected void IbtAprPNAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                if (TblDetalle.Rows.Count > 0)
                {
                    foreach (DataRow Dtll in TblDetalle.Rows) { Dtll["Aprobacion"] = "1"; }
                    GrdDet.DataSource = TblDetalle; GrdDet.DataBind();
                }
            }
        }
        protected void TxtCant_TextChanged(object sender, EventArgs e)
        {
            Valores();
            var Cntrl = (Control)sender;
            GridViewRow row = (GridViewRow)Cntrl.NamingContainer;
            int rowIndex = row.RowIndex;
            DropDownList DdlUM = (DropDownList)GrdDet.Rows[rowIndex].FindControl("DdlUM");
            DdlUM.Attributes.Add("onfocus", "this.select();");
            DdlUM.Focus();
        }
        protected void TxtVlor_TextChanged(object sender, EventArgs e)
        {
            Valores();
            var ControlAct = (Control)sender;
            GridViewRow row = (GridViewRow)ControlAct.NamingContainer;
            int rowIndex = row.RowIndex;
            TextBox TxtTsIVA = (TextBox)GrdDet.Rows[rowIndex].FindControl("TxtTsIVA");
            TxtTsIVA.Attributes.Add("onfocus", "this.select();");
            TxtTsIVA.Focus();
        }
        protected void TxtTsIVA_TextChanged(object sender, EventArgs e)
        {
            Valores();
            var Cntrl = (Control)sender;
            GridViewRow row = (GridViewRow)Cntrl.NamingContainer;
            int rowIndex = row.RowIndex;
            DropDownList DdlEstdElem = (DropDownList)GrdDet.Rows[rowIndex].FindControl("DdlEstdElem");
            DdlEstdElem.Attributes.Add("onfocus", "this.select();");
            DdlEstdElem.Focus();
        }
        protected void GrdDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            { BindModalBusqCot(); 
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqSP", "$('#ModalBusqSP').modal();", true);
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
            }
        }
        protected void GrdDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (!TxtNumDocum.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens20Cotz'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// No es posible eliminar esta cotización porque se encuentra asignado a un documento.
                return;
            }

            int index = Convert.ToInt32(e.RowIndex);
            TblDetalle.Rows[index].Delete();
            BindDDetTmp();
        }
        protected void GrdDet_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                DataView DV = new DataView(TblDetalle);
                DV.Sort = e.SortExpression;
                GrdDet.DataSource = DV;
                GrdDet.DataBind();
            }
        }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtBusqSP = (e.Row.FindControl("IbtBusqSP") as ImageButton);

                DataRow[] Result = Idioma.Select("Objeto='IbtAsigSolPed'");
                foreach (DataRow RowIdioma in Result)
                { IbtBusqSP.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                if (ViewState["DocAprobado"].ToString().Trim().Equals("S") || ViewState["PeriodCerrado"].ToString().Trim().Equals("S"))// Si esta aprobado o Periodo cerrado bloquea
                {
                    IbtBusqSP.Enabled = false; IbtBusqSP.Enabled = false;

                    if (ViewState["DocAprobado"].ToString().Trim().Equals("S"))
                    { IbtBusqSP.ToolTip = ViewState["TxtAprob"].ToString().Trim(); }

                    if (ViewState["PeriodCerrado"].ToString().Trim().Equals("S"))
                    { IbtBusqSP.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim(); }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DSTDdl = (DataSet)ViewState["DSTDdl"];

                CheckBox CkbAprob = (CheckBox)e.Row.FindControl("CkbAprob");
                DropDownList DdlPN = (DropDownList)e.Row.FindControl("DdlPN");
                DropDownList DdlUM = (DropDownList)e.Row.FindControl("DdlUM");
                DropDownList DdlEstdElem = (DropDownList)e.Row.FindControl("DdlEstdElem");
                TextBox TxtCant = (TextBox)e.Row.FindControl("TxtCant");
                TextBox TxtVlor = (TextBox)e.Row.FindControl("TxtVlor");
                TextBox TxtTsIVA = (TextBox)e.Row.FindControl("TxtTsIVA");
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;

                DdlPN.DataSource = DSTDdl.Tables[8];
                DdlPN.DataTextField = "PN";
                DdlPN.DataValueField = "PN";
                DdlPN.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbPN = dr["Pn"].ToString().Trim();
                DdlPN.SelectedValue = VbPN;

                string VbCodAnt = dr["CodUndMed"].ToString().Trim();
                DataTable DT = new DataTable();
                DataRow[] DR = DSTDdl.Tables[9].Select("ActivoUM=1 AND PN ='" + VbPN + "' OR (UndCompraPN ='" + VbCodAnt + "' AND PN ='" + VbPN + "') OR PN = ''");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlUM.DataSource = DT;
                DdlUM.DataTextField = "Descripcion";
                DdlUM.DataValueField = "UndCompraPN";
                DdlUM.DataBind();
                DdlUM.Text = VbCodAnt;

                DdlEstdElem.DataSource = DSTDdl.Tables[10];
                DdlEstdElem.DataTextField = "Descripcion";
                DdlEstdElem.DataValueField = "CodCondicionElem";
                DdlEstdElem.DataBind();
                DataRowView DREe = e.Row.DataItem as DataRowView;
                DdlEstdElem.Text = DREe["CodEstdo"].ToString().Trim();

                DataRowView DRExistDoc = e.Row.DataItem as DataRowView;
                if (DRExistDoc["ExisteDoc"].ToString().Trim().Equals("S")) { CkbAprob.Enabled = false; }
                else { CkbAprob.Enabled = true; }

                if (!DdlMoned.Text.Trim().Equals(Session["MonLcl"])) { TxtTsIVA.Enabled = false; TxtTsIVA.Text = "0"; }// Si es internal bloquea TasaIVA

                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }

                if (ViewState["DocAprobado"].ToString().Trim().Equals("S") || ViewState["PeriodCerrado"].ToString().Trim().Equals("S") || ViewState["ShipLiquidada"].ToString().Trim().Equals("S"))// Si esta aprobado o Periodo cerrado o liquidad el COMEX
                {
                    TxtCant.Enabled = false; DdlUM.Enabled = false; TxtVlor.Enabled = false; TxtTsIVA.Enabled = false;

                    if (ViewState["ShipLiquidada"].ToString().Trim().Equals("S"))
                    {
                        TxtCant.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim();
                        DdlUM.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim();
                        TxtVlor.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim();
                        TxtTsIVA.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim();
                        if (imgD != null)
                        { imgD.Enabled = false; imgD.ToolTip = ViewState["TxtSOLiqud"].ToString().Trim(); }
                    }

                    if (ViewState["DocAprobado"].ToString().Trim().Equals("S"))
                    {
                        TxtCant.ToolTip = ViewState["TxtAprob"].ToString().Trim();
                        DdlUM.ToolTip = ViewState["TxtAprob"].ToString().Trim();
                        TxtVlor.ToolTip = ViewState["TxtAprob"].ToString().Trim();
                        TxtTsIVA.ToolTip = ViewState["TxtAprob"].ToString().Trim();
                        if (imgD != null) { imgD.Enabled = false; imgD.ToolTip = ViewState["TxtAprob"].ToString().Trim(); }
                    }

                    if (ViewState["PeriodCerrado"].ToString().Trim().Equals("S"))
                    {
                        TxtCant.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim();
                        DdlUM.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim();
                        TxtVlor.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim();
                        TxtTsIVA.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim();
                        if (imgD != null) { imgD.Enabled = false; imgD.ToolTip = ViewState["TxtPerdCerr"].ToString().Trim(); }
                    }
                }

                if (ViewState["TieneSOMvtoAlma"].ToString().Trim().Equals("S"))
                {
                    TxtCant.Enabled = false; DdlUM.Enabled = false;
                    TxtCant.ToolTip = ViewState["TxtTieneSOMvtAlma"].ToString().Trim();
                    DdlUM.ToolTip = ViewState["TxtTieneSOMvtAlma"].ToString().Trim();
                    if (imgD != null) { imgD.Enabled = false; imgD.ToolTip = ViewState["TxtTieneSOMvtAlma"].ToString().Trim(); }
                }

                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        //****************************** MOdal **************************************
        protected void BindModalBusqCot()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTipoDoc = "SP", VbSDoc = "", VbTipoCotiza;
                int NumPR_PT = 0;
                bool VbRslt = int.TryParse(TxtModalBusq.Text.Trim(), out NumPR_PT);
                VbSDoc = NumPR_PT.ToString();
                switch (ViewState["TipoCotiza"].ToString().Trim())
                {
                    case "01": // COMPRA
                        VbTipoCotiza = "C"; break;
                    case "02": // REPA
                        VbTipoCotiza = "P"; break;
                    default: // INTERCAMBIO
                        VbTipoCotiza = "I"; break;
                }
                if (RdbMOdalBusqSP.Checked == true) { VbTipoDoc = "SP"; }
                if (RdbMOdalBusqPPT.Checked == true) { VbTipoDoc = "PR"; }
                if (RdbMOdalBusqPet.Checked == true) { VbTipoDoc = "PT"; }

                string VbTxtSql = " EXEC SP_PANTALLA_Cotizacion 2,@TipSP,@TipCotza,@Doc,@TpDc,0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@TipSP", ViewState["TipoCotiza"]);
                    SC.Parameters.AddWithValue("@TipCotza", VbTipoCotiza);
                    SC.Parameters.AddWithValue("@Doc", VbSDoc);
                    SC.Parameters.AddWithValue("@TpDc", VbTipoDoc);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DTSolPed);
                    if (DTSolPed.Rows.Count > 0) { GrdModalBusqCot.DataSource = DTSolPed; }
                    else { GrdModalBusqCot.DataSource = null; }
                    GrdModalBusqCot.DataBind();
                    ViewState["DTSolPed"] = DTSolPed;
                }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            BindModalBusqCot();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqSP", "$('#ModalBusqSP').modal();", true);
        }
        protected void IbtAprDetAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DTSolPed = (DataTable)ViewState["DTSolPed"];
            if (DTSolPed.Rows.Count > 0)
            {
                foreach (DataRow Dtll in DTSolPed.Rows)
                { Dtll["CHK"] = "1"; }
                GrdModalBusqCot.DataSource = DTSolPed; GrdModalBusqCot.DataBind();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalBusqSP", "$('#ModalBusqSP').modal();", true);
        }
        protected void GrdModalBusqCot_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbEstdPN = dr["CodEstadoPn"].ToString();
                switch (VbEstdPN)
                {
                    case "02":// PN alterno
                        e.Row.BackColor = System.Drawing.Color.Orange; break;
                    case "03":// Superados
                        e.Row.BackColor = System.Drawing.Color.DarkRed; e.Row.ForeColor = System.Drawing.Color.White; break;
                }
            }
        }
        protected void BtnAsignarModal_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DTSolPed = (DataTable)ViewState["DTSolPed"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            Valores();

            TblDetalle.AcceptChanges();
            foreach (GridViewRow Row in GrdModalBusqCot.Rows)
            {
                if ((Row.FindControl("CkbA") as CheckBox).Checked == true)
                {
                    string VbIdDetPd = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim();
                    string VbCodPd = (Row.FindControl("LblCodSped") as Label).Text.Trim();
                    string VbPn = (Row.FindControl("LblPn") as Label).Text.Trim();
                    string VbDescPn = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["DescPn"].ToString().Trim();
                    string VbCant = (Row.FindControl("LblCant") as Label).Text.Trim();
                    string VbUndMed = (Row.FindControl("LblUndM") as Label).Text.Trim();
                    string VbSN = GrdModalBusqCot.DataKeys[Row.RowIndex].Values["SN"].ToString().Trim();

                    DataRow dr = TblDetalle.Select("IdDetPedido = " + VbIdDetPd).FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                    if (dr == null)
                    {
                        TblDetalle.Rows.Add("1 DetCotiza", 0, 0, Convert.ToInt32(VbIdDetPd), 0, 0, VbCodPd.Trim(), 0, VbPn.Trim(), VbDescPn.Trim(),
                             Convert.ToDouble(VbCant), VbUndMed.Trim(), 0, 0, 0, 0, "", 0, 0, "", VbSN.Trim(), "", "N", "INS", "");
                    }
                }
            }
            TblDetalle.AcceptChanges();
            BindDDetTmp();
        }
        //****************************** Exportar **************************************
        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            if (TxtNumCotiza.Text.Equals(""))
            { return; }
            { MultVw.ActiveViewIndex = 2; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        }
        protected void IbtCloseExport_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnExportDetCotiza_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string StSql, VbNomRpt = "";

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    CursorIdioma.Alimentar("CurExportCotizaPetic", Session["77IDM"].ToString().Trim());
                    StSql = "EXEC PNTLL_Cotizac 4, @NumC,'','','','','CurExportCotizaPetic',0,0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'CurExportCotizaPetic'");
                        foreach (DataRow row in Result)
                        { VbNomRpt = row["Texto"].ToString().Trim(); }// Detalle Cotización

                        SC.Parameters.AddWithValue("@NumC", TxtNumCotiza.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.CommandTimeout = 90000000;
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "77NeoWeb";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Detalle Cotización", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnExportDetUnidMed_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string StSql, VbNomRpt = "";

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    CursorIdioma.Alimentar("CurExportCotizaUndMed", Session["77IDM"].ToString().Trim());
                    StSql = "EXEC SP_PANTALLA_Cotizacion 11, @NumC,'','','CurExportCotizaUndMed',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'CurExportCotizaUndMed'");
                        foreach (DataRow row in Result)
                        { VbNomRpt = row["Texto"].ToString().Trim(); }// Detalle Cotización

                        SC.Parameters.AddWithValue("@NumC", TxtNumCotiza.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.CommandTimeout = 90000000;
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "77NeoWeb";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
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
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Cotización Unidad Medida", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}