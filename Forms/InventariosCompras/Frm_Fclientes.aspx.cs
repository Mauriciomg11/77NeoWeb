using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class Frm_Fclientes : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
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
                ModSeguridad();
                ViewState["IdTercero"] = "0";
                ViewState["CodTerceroAnt"] = "";
                ViewState["CodTipoDocAnt"] = "0";
                ViewState["CodClasePrvdAnt"] = "";
                ViewState["TipoPagoAnt"] = "";
                ViewState["CodTipProvAnt"] = "";
                ViewState["CodBancoAnt"] = "";
                ViewState["CodPaisAnt"] = "";
                ViewState["CiudadAnt"] = "0";
                ViewState["EstadoAnt"] = "0";
                BindBDdl("UPD");
                ViewState["Accion"] = "";
                RdbMdlOpcBusqProv.Checked = true;
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
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false; }//GrdLicencias.ShowFooter = false; 
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // 
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }// 
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
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
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnExport.Text = bO.Equals("BtnExportMstr") ? bT : BtnExport.Text;
                    BtnExport.Text = bO.Equals("BtnExportMstr") ? bT : BtnExport.Text;
                    CkbActivo.Text = bO.Equals("ActivoMstr") ? "&nbsp" + bT : CkbActivo.Text;
                    LblNit.Text = bO.Equals("LblNit") ? bT : LblNit.Text;
                    LblTipoDoc.Text = bO.Equals("LblTipoDoc") ? bT : LblTipoDoc.Text;
                    LblClasfJurd.Text = bO.Equals("LblClasfJurd") ? bT : LblClasfJurd.Text;
                    LblTipoRegmn.Text = bO.Equals("LblTipoRegmn") ? bT : LblTipoRegmn.Text;
                    RdbProvdr.Text = bO.Equals("RdbProvdr") ? "&nbsp" + bT : RdbProvdr.Text;
                    RdbCliente.Text = bO.Equals("RdbCliente") ? "&nbsp" + bT : RdbCliente.Text;
                    RdbAmbos.Text = bO.Equals("RdbAmbos") ? "&nbsp" + bT : RdbAmbos.Text;
                    LblRazonSoc.Text = bO.Equals("LblRazonSoc") ? bT : LblRazonSoc.Text;
                    LblDirecc.Text = bO.Equals("LblDirecc") ? bT : LblDirecc.Text;
                    LblTelef.Text = bO.Equals("LblTelef") ? bT : LblTelef.Text;
                    LblCorreo.Text = bO.Equals("LblCorreo") ? bT : LblCorreo.Text;
                    LblClase.Text = bO.Equals("LblClase") ? bT : LblClase.Text;
                    LblFormaPago.Text = bO.Equals("LblFormaPago") ? bT : LblFormaPago.Text;
                    LblTipTerce.Text = bO.Equals("LblTipTerce") ? bT : LblTipTerce.Text;
                    LblMoned.Text = bO.Equals("LblMoned") ? bT : LblMoned.Text;
                    LblCodPostal.Text = bO.Equals("LblCodPostal") ? bT : LblCodPostal.Text;
                    LblBanco.Text = bO.Equals("LblBanco") ? bT : LblBanco.Text;
                    LblNroCta.Text = bO.Equals("LblNroCta") ? bT : LblNroCta.Text;
                    RdbCtaAhorr.Text = bO.Equals("RdbCtaAhorr") ? "&nbsp" + bT : RdbCtaAhorr.Text;
                    RdbCtaCte.Text = bO.Equals("RdbCtaCte") ? "&nbsp" + bT : RdbCtaCte.Text;
                    RdbCtaNA.Text = bO.Equals("RdbCtaNA") ? "&nbsp" + bT : RdbCtaNA.Text;
                    LblPais.Text = bO.Equals("LblPais") ? bT : LblPais.Text;
                    LblCiudad.Text = bO.Equals("LblCiudad") ? bT : LblCiudad.Text;
                    LblEstado.Text = bO.Equals("LblEstado") ? bT : LblEstado.Text;
                    LblIVA.Text = bO.Equals("LblIVA") ? bT : LblIVA.Text;
                    LblObservac.Text = bO.Equals("LblObsMst") ? bT : LblObservac.Text;
                    LblTitContactoDefecto.Text = bO.Equals("LblTitContactoDefecto") ? bT : LblTitContactoDefecto.Text;
                    TxtNomContactDeft.ToolTip = bO.Equals("TxtNomContactDeft") ? bT : TxtNomContactDeft.ToolTip;
                    TxtApellContactDeft.ToolTip = bO.Equals("TxtApellContactDeft") ? bT : TxtApellContactDeft.ToolTip;
                    LblTitContacto.Text = bO.Equals("LblTitContacto") ? bT : LblTitContacto.Text;
                    TxtDV.ToolTip = "DV";
                    // *************************************************Detalle Contacto*************************************************
                    GrdContacto.Columns[0].HeaderText = bO.Equals("GrdPpl") ? bT : GrdContacto.Columns[0].HeaderText;
                    GrdContacto.Columns[1].HeaderText = bO.Equals("TxtNomContactDeft") ? bT : GrdContacto.Columns[1].HeaderText;
                    GrdContacto.Columns[2].HeaderText = bO.Equals("TxtApellContactDeft") ? bT : GrdContacto.Columns[2].HeaderText;
                    GrdContacto.Columns[3].HeaderText = bO.Equals("LblTelef") ? bT : GrdContacto.Columns[3].HeaderText;
                    GrdContacto.Columns[4].HeaderText = bO.Equals("LblCorreo") ? bT : GrdContacto.Columns[4].HeaderText;
                    // *************************************************Modal Busq*************************************************
                    if (bO.Equals("placeholder"))
                    { TxtModalBusq.Attributes.Add("placeholder", bT); }
                    LblTitModalBusqTerc.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitModalBusqTerc.Text;
                    RdbMdlOpcBusqProv.Text = bO.Equals("LblRazonSoc") ? "&nbsp" + bT : RdbMdlOpcBusqProv.Text;
                    RdbMdlOpcBusqCod.Text = bO.Equals("LblNit") ? "&nbsp" + bT : RdbMdlOpcBusqCod.Text;
                    LblModalBusq.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblModalBusq.Text;
                    IbtModalBusq.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtModalBusq.ToolTip;
                    GrdModalBusqTercero.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqTercero.EmptyDataText;
                    GrdModalBusqTercero.Columns[1].HeaderText = bO.Equals("LblNit") ? bT : GrdModalBusqTercero.Columns[1].HeaderText;
                    GrdModalBusqTercero.Columns[2].HeaderText = bO.Equals("LblRazonSoc") ? bT : GrdModalBusqTercero.Columns[2].HeaderText;
                    GrdModalBusqTercero.Columns[3].HeaderText = bO.Equals("LblMoned") ? bT : GrdModalBusqTercero.Columns[3].HeaderText;
                    BtnCloseModalBusqCompra.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseModalBusqCompra.Text;
                    GrdModalBusqTercero.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdModalBusqTercero.EmptyDataText;
                    GrdModalBusqTercero.Columns[1].HeaderText = bO.Equals("Grd") ? bT : GrdModalBusqTercero.Columns[1].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdContacto.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[5].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[5].Controls.Remove(imgD);
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
        protected void BindCiudad(string CodPais)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Ciudad"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTDdl.Tables[9].Select("Activa = 1 AND CodUbicaGeoSup = '" + CodPais + "' OR CodUbicaGeogr = '' OR IdUbicaGeogr = " + ViewState["CiudadAnt"]);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlCiudad.DataSource = DT;
                DdlCiudad.DataTextField = "Nombre";
                DdlCiudad.DataValueField = "IdUbicaGeogr";
                DdlCiudad.DataBind();
                DdlCiudad.Text = ViewState["CiudadAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Estado"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DataRow[] DR = DSTDdl.Tables[10].Select("Activa = 1 AND CodUbicaGeoSup = '" + CodPais + "' OR CodUbicaGeogr = '' OR IdUbicaGeogr = " + ViewState["EstadoAnt"]);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlEstado.DataSource = DT;
                DdlEstado.DataTextField = "Nombre";
                DdlEstado.DataValueField = "IdUbicaGeogr";
                DdlEstado.DataBind();
                DdlEstado.Text = ViewState["EstadoAnt"].ToString().Trim();
            }
        }
        protected void BindBDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Tercero 6,'','','','DDL',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
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
                                DSTDdl.Tables[0].TableName = "TipoDoc";
                                DSTDdl.Tables[1].TableName = "ClasFJurd";
                                DSTDdl.Tables[2].TableName = "TipoRegmn";
                                DSTDdl.Tables[3].TableName = "ClasProv";
                                DSTDdl.Tables[4].TableName = "TipoPago";
                                DSTDdl.Tables[5].TableName = "TipoProve";
                                DSTDdl.Tables[6].TableName = "Moneda";
                                DSTDdl.Tables[7].TableName = "Banco";
                                DSTDdl.Tables[8].TableName = "Pais";
                                DSTDdl.Tables[9].TableName = "Ciudad";
                                DSTDdl.Tables[10].TableName = "Estado";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }

            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] DR;
            string VbCodAnt;

            if (DSTDdl.Tables["TipoDoc"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[0].Select("Activo=1  OR CodIdDocTerc= " + ViewState["CodTipoDocAnt"]);
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlTipoDoc.DataSource = DT;
                DdlTipoDoc.DataTextField = "Descripcion";
                DdlTipoDoc.DataValueField = "CodIdDocTerc";
                DdlTipoDoc.DataBind();
                DdlTipoDoc.SelectedValue = ViewState["CodTipoDocAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["ClasFJurd"].Rows.Count > 0)
            {
                VbCodAnt = DdlClasfJurd.Text.Trim();
                DdlClasfJurd.DataSource = DSTDdl.Tables[1];
                DdlClasfJurd.DataTextField = "Nombre";
                DdlClasfJurd.DataValueField = "CodClasifJuridic";
                DdlClasfJurd.DataBind();
                DdlClasfJurd.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["TipoRegmn"].Rows.Count > 0)
            {
                VbCodAnt = DdlTipoRegmn.Text.Trim();
                DdlTipoRegmn.DataSource = DSTDdl.Tables[2];
                DdlTipoRegmn.DataTextField = "Nombre";
                DdlTipoRegmn.DataValueField = "CodTipoRegimen";
                DdlTipoRegmn.DataBind();
                DdlTipoRegmn.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["ClasProv"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[3].Select("Activo=1  OR CodClaseProv= '" + ViewState["CodClasePrvdAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlClase.DataSource = DT;
                DdlClase.DataTextField = "DescripcionClaseProv";
                DdlClase.DataValueField = "CodClaseProv";
                DdlClase.DataBind();
                DdlClase.SelectedValue = ViewState["CodClasePrvdAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["TipoPago"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[4].Select("Activo=1 OR CodTipoPago= '" + ViewState["TipoPagoAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlFormaPago.DataSource = DT;
                DdlFormaPago.DataTextField = "Descripcion";
                DdlFormaPago.DataValueField = "CodTipoPago";
                DdlFormaPago.DataBind();
                DdlFormaPago.SelectedValue = ViewState["TipoPagoAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["TipoProve"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[5].Select("Activo = 1 OR CodTipoProveedor = '" + ViewState["CodTipProvAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlTipTerce.DataSource = DT;
                DdlTipTerce.DataTextField = "Descripcion";
                DdlTipTerce.DataValueField = "CodTipoProveedor";
                DdlTipTerce.DataBind();
                DdlTipTerce.SelectedValue = ViewState["CodTipProvAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Moneda"].Rows.Count > 0)
            {
                VbCodAnt = DdllMoned.Text.Trim();
                DdllMoned.DataSource = DSTDdl.Tables[6];
                DdllMoned.DataTextField = "Descripcion";
                DdllMoned.DataValueField = "CodTipoMoneda";
                DdllMoned.DataBind();
                DdllMoned.Text = VbCodAnt;
            }
            if (DSTDdl.Tables["Banco"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[7].Select("Activo=1 OR CodigoBanco = '" + ViewState["CodBancoAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlBanco.DataSource = DT;
                DdlBanco.DataTextField = "NombreBanco";
                DdlBanco.DataValueField = "CodigoBanco";
                DdlBanco.DataBind();
                DdlBanco.SelectedValue = ViewState["CodBancoAnt"].ToString().Trim();
            }
            if (DSTDdl.Tables["Pais"].Rows.Count > 0)
            {
                DataTable DT = new DataTable();
                DR = DSTDdl.Tables[8].Select("Activa = 1 OR CodUbicaGeogr = '" + ViewState["CodPaisAnt"] + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
                DdlPais.DataSource = DT;
                DdlPais.DataTextField = "Nombre";
                DdlPais.DataValueField = "CodUbicaGeogr";
                DdlPais.DataBind();
                DdlPais.SelectedValue = ViewState["CodPaisAnt"].ToString().Trim();
            }
            BindCiudad(ViewState["CodPaisAnt"].ToString().Trim());
        }
        protected void Traerdatos(string CodTerc, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_PANTALLA_Tercero 6, @Cod, @NT, '','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Cod", CodTerc);
                            SC.Parameters.AddWithValue("@NT", Session["Nit77Cia"]);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPpl = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPpl);
                                    DSTPpl.Tables[0].TableName = "Tercero";
                                    DSTPpl.Tables[1].TableName = "DetContacto";

                                    ViewState["DSTPpl"] = DSTPpl;
                                }
                            }
                        }
                    }
                }
                TxtModalBusq.Text = "";
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                if (DSTPpl.Tables["Tercero"].Rows.Count > 0)
                {
                    CkbActivo.Checked = DSTPpl.Tables[0].Rows[0]["Activo"].ToString().Trim().Equals("1") ? true : false;
                    TxtNit.Text = DSTPpl.Tables[0].Rows[0]["CodTercero"].ToString().Trim();
                    ViewState["CodTerceroAnt"] = DSTPpl.Tables[0].Rows[0]["CodTercero"].ToString().Trim();
                    TxtDV.Text = DSTPpl.Tables[0].Rows[0]["DigVerificacion"].ToString().Trim();
                    ViewState["CodTipoDocAnt"] = DSTPpl.Tables[0].Rows[0]["Codtipoident"].ToString().Trim();
                    DdlClasfJurd.Text = DSTPpl.Tables[0].Rows[0]["CodClasiJuridic"].ToString().Trim();
                    DdlTipoRegmn.Text = DSTPpl.Tables[0].Rows[0]["CodTipoRegimen"].ToString().Trim();
                    RdbProvdr.Checked = DSTPpl.Tables[0].Rows[0]["Clasificacion"].ToString().Trim().Equals("P") ? true : false;
                    RdbCliente.Checked = DSTPpl.Tables[0].Rows[0]["Clasificacion"].ToString().Trim().Equals("C") ? true : false;
                    RdbAmbos.Checked = DSTPpl.Tables[0].Rows[0]["Clasificacion"].ToString().Trim().Equals("A") ? true : false;
                    TxtRazonSoc.Text = DSTPpl.Tables[0].Rows[0]["RazonSocial"].ToString().Trim();
                    TxtDirecc.Text = DSTPpl.Tables[0].Rows[0]["Direccion"].ToString().Trim();
                    TxtTelef.Text = DSTPpl.Tables[0].Rows[0]["Telefono"].ToString().Trim();
                    TxtFax.Text = DSTPpl.Tables[0].Rows[0]["Fax"].ToString().Trim();
                    TxtCorreo.Text = DSTPpl.Tables[0].Rows[0]["Correo"].ToString().Trim();
                    ViewState["CodClasePrvdAnt"] = DSTPpl.Tables[0].Rows[0]["CodClaseProv"].ToString().Trim();
                    ViewState["TipoPagoAnt"] = DSTPpl.Tables[0].Rows[0]["CodTipoPago"].ToString().Trim();
                    ViewState["CodTipProvAnt"] = DSTPpl.Tables[0].Rows[0]["CodTipoProveedor"].ToString().Trim();
                    DdllMoned.Text = DSTPpl.Tables[0].Rows[0]["CodMoneda"].ToString().Trim();
                    TxtCodPostal.Text = DSTPpl.Tables[0].Rows[0]["CodPostal"].ToString().Trim();
                    ViewState["CodBancoAnt"] = DSTPpl.Tables[0].Rows[0]["BcoBeneficiario"].ToString().Trim();
                    TxtNroCta.Text = DSTPpl.Tables[0].Rows[0]["NroDeCta"].ToString().Trim();
                    RdbCtaAhorr.Checked = DSTPpl.Tables[0].Rows[0]["ClaseCta"].ToString().Trim().Equals("1") ? true : false;
                    RdbCtaCte.Checked = DSTPpl.Tables[0].Rows[0]["ClaseCta"].ToString().Trim().Equals("2") ? true : false;
                    RdbCtaNA.Checked = DSTPpl.Tables[0].Rows[0]["ClaseCta"].ToString().Trim().Equals("3") ? true : false;
                    ViewState["CodPaisAnt"] = DSTPpl.Tables[0].Rows[0]["Pais"].ToString().Trim();
                    ViewState["CiudadAnt"] = DSTPpl.Tables[0].Rows[0]["Ciudad"].ToString().Trim();
                    ViewState["EstadoAnt"] = DSTPpl.Tables[0].Rows[0]["Estado"].ToString().Trim();
                    TxtSwift.Text = DSTPpl.Tables[0].Rows[0]["SwiftCode"].ToString().Trim();
                    TxtAba.Text = DSTPpl.Tables[0].Rows[0]["ABA"].ToString().Trim();
                    TxtIVA.Text = DSTPpl.Tables[0].Rows[0]["IVA"].ToString().Trim();
                    TxtObservac.Text = DSTPpl.Tables[0].Rows[0]["Comentario"].ToString().Trim();
                    TxtNomContactDeft.Text = DSTPpl.Tables[0].Rows[0]["NomContacto"].ToString().Trim();
                    TxtApellContactDeft.Text = DSTPpl.Tables[0].Rows[0]["ApeContacto"].ToString().Trim();
                    BindBDdl("SEL");
                }
                BindDContacto();

            }
            catch (Exception Ex)
            {
                BtnIngresar.Visible = false; BtnModificar.Visible = false;
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
            }
        }
        protected void LimpiarCampos(string Accion)
        {
            ViewState["IdTercero"] = "0";
            CkbActivo.Checked = false;
            TxtNit.Text = "";
            TxtDV.Text = "";
            DdlTipoDoc.Text = "0";
            DdlClasfJurd.Text = "";
            DdlTipoRegmn.Text = "";
            RdbProvdr.Checked = true;
            RdbCliente.Checked = false;
            RdbAmbos.Checked = false;
            TxtRazonSoc.Text = "";
            TxtDirecc.Text = "";
            TxtTelef.Text = "";
            TxtFax.Text = "";
            TxtCorreo.Text = "";
            DdlClase.Text = "";
            DdlFormaPago.Text = "";
            DdlTipTerce.Text = "";
            DdllMoned.Text = "";
            TxtCodPostal.Text = "";
            DdlBanco.Text = "";
            TxtNroCta.Text = "";
            RdbCtaAhorr.Checked = false;
            RdbCtaCte.Checked = false;
            RdbCtaNA.Checked = false;
            DdlPais.Text = "";
            DdlCiudad.Text = "0";
            DdlEstado.Text = "0";
            TxtSwift.Text = "";
            TxtAba.Text = "";
            TxtObservac.Text = "";
            TxtNomContactDeft.Text = "";
            TxtApellContactDeft.Text = "";
            TxtIVA.Text = "0";
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtNit.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el codigo.
                ViewState["Validar"] = "N"; TxtNit.Focus(); return;
            }
            if (DdlTipoDoc.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el tipo doc.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlClasfJurd.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la clasificación jurídica.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipoRegmn.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo régimen.
                ViewState["Validar"] = "N"; return;
            }
            if (TxtRazonSoc.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la razón social.
                ViewState["Validar"] = "N"; TxtRazonSoc.Focus(); return;
            }
            if (TxtDirecc.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la dirección..
                ViewState["Validar"] = "N"; TxtDirecc.Focus(); return;
            }
            if (TxtTelef.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens07Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el numero telefonico.
                ViewState["Validar"] = "N"; TxtTelef.Focus(); return;
            }
            if (TxtCorreo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens08Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el correo.
                ViewState["Validar"] = "N"; TxtCorreo.Focus(); return;
            }
            if (DdlClase.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens09Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la clase de tercero.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlFormaPago.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens10Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la forma de pago.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlTipTerce.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens11Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el tipo proveedor/ cliente.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlPais.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens12Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el país.
                ViewState["Validar"] = "N"; return;
            }
            if (DdlCiudad.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens13Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la ciudad.
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr, string Accion)
        {
            BtnConsultar.Enabled = Md;
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnExport.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string Accion)
        {
            if (Accion.Equals("UPD")) { CkbActivo.Enabled = Edi; }
            TxtNit.Enabled = Edi;
            TxtDV.Enabled = Edi;
            DdlTipoDoc.Enabled = Edi;
            DdlClasfJurd.Enabled = Edi;
            DdlTipoRegmn.Enabled = Edi;
            RdbProvdr.Enabled = Edi;
            RdbCliente.Enabled = Edi;
            RdbAmbos.Enabled = Edi;
            TxtRazonSoc.Enabled = Edi;
            TxtDirecc.Enabled = Edi;
            TxtTelef.Enabled = Edi;
            TxtFax.Enabled = Edi;
            TxtCorreo.Enabled = Edi;
            DdlClase.Enabled = Edi;
            DdlFormaPago.Enabled = Edi;
            DdlTipTerce.Enabled = Edi;
            DdllMoned.Enabled = Edi;
            TxtCodPostal.Enabled = Edi;
            DdlBanco.Enabled = Edi;
            TxtNroCta.Enabled = Edi;
            RdbCtaAhorr.Enabled = Edi;
            RdbCtaCte.Enabled = Edi;
            RdbCtaNA.Enabled = Edi;
            DdlPais.Enabled = Edi;
            DdlCiudad.Enabled = Edi;
            DdlEstado.Enabled = Edi;
            TxtSwift.Enabled = Edi;
            TxtAba.Enabled = Edi;
            TxtIVA.Enabled = Edi;
            TxtObservac.Enabled = Edi;
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
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
                    ActivarCampos(true, true, "INS");
                    LimpiarCampos("INS");
                    CkbActivo.Checked = true;
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
                    GrdContacto.Enabled = false; GrdContacto.DataSource = null; GrdContacto.DataBind();
                }
                else
                {
                    ValidarCampos("INS");
                    if (ViewState["Validar"].Equals("N")) { return; }
                    string VbClasificacion = "";
                    if (RdbProvdr.Checked == true) { VbClasificacion = "P"; }
                    if (RdbCliente.Checked == true) { VbClasificacion = "C"; }
                    if (RdbAmbos.Checked == true) { VbClasificacion = "A"; }
                    int VbClaseCta = 0;
                    if (RdbCtaAhorr.Checked == true) { VbClaseCta = 1; }
                    if (RdbCtaCte.Checked == true) { VbClaseCta = 2; }
                    if (RdbCtaNA.Checked == true) { VbClaseCta = 3; }
                    List<ClsTypTercero> ObjTercero = new List<ClsTypTercero>();
                    var TypTercero = new ClsTypTercero()
                    {
                        CodTercero = TxtNit.Text.Trim(),
                        RazonSocial = TxtRazonSoc.Text.Trim(),
                        Direccion = TxtDirecc.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Fax = TxtFax.Text.Trim(),
                        Correo = TxtCorreo.Text.Trim(),
                        CodUbicaGeogr = Convert.ToInt32(DdlCiudad.Text.Trim()),
                        Estado = Convert.ToInt32(DdlEstado.Text.Trim()),
                        Comentario = TxtObservac.Text.Trim(),
                        CodClaseServicio = "",
                        CodTipoPago = DdlFormaPago.Text.Trim(),
                        CodTipo = "",
                        CodTipoProveedor = DdlTipTerce.Text.Trim(),
                        DireccionPago = "",
                        CodClasiJuridic = DdlClasfJurd.Text.Trim(),
                        CodTipoRegimen = DdlTipoRegmn.Text.Trim(),
                        PrecioFacturacion = "",
                        Cupo = Convert.ToDouble(0),
                        CodMoneda = DdllMoned.Text.Trim(),
                        CodTipoIdent = DdlTipoDoc.Text.Trim(),
                        Descuento = Convert.ToDouble(0),
                        DiasDescuento = Convert.ToInt32(0),
                        PagoA = "",
                        Clasificacion = VbClasificacion,
                        CuentaPuc = "",
                        CuentaPucProveedor = "",
                        Empleado = "",
                        Identificacion = "",
                        Activo = CkbActivo.Checked == true ? 1 : 0,
                        DigVerificacion = TxtDV.Text.Trim(),
                        CodPostal = TxtCodPostal.Text.Trim(),
                        Pais = DdlPais.Text.Trim(),
                        NroDeCta = TxtNroCta.Text.Trim(),
                        ClaseCta = VbClaseCta,
                        BcoBeneficiario = DdlBanco.Text.Trim(),
                        BcoCorresponsal = "",
                        SwiftCode = TxtSwift.Text.Trim(),
                        ABA = TxtAba.Text.Trim(),
                        NomContacto = "",
                        ApeContacto = "",
                        TipoPagoBanco = "",
                        CodTipoCodigo = "01",
                        CodClaseProv = DdlClase.Text.Trim(),
                        IVA = Convert.ToDouble(TxtIVA.Text.Trim().Equals("") ? "0" : TxtIVA.Text.Trim()),
                        IdTercero = Convert.ToInt32(ViewState["IdTercero"]),
                    };
                    ObjTercero.Add(TypTercero);
                    ClsTypTercero ClsTercero = new ClsTypTercero();
                    ClsTercero.Accion("INSERT");
                    ClsTercero.Alimentar(ObjTercero);
                    string Mensj = ClsTercero.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, "INS");
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "INS");
                    LimpiarCampos("INS");
                    ViewState["IdTercero"] = ClsTercero.GetPIdTercero().Trim();
                    Traerdatos(ClsTercero.GetPCodTercero().Trim(), "UPD");
                    BtnIngresar.OnClientClick = "";
                    GrdContacto.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Tercero", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtNit.Text.Equals(""))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(false, true, false, false, false, "UPD");
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(false, true, "UPD");
                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea editar el registro?  
                    GrdContacto.Enabled = false; ;
                }
                else
                {
                    ValidarCampos("UPD");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    string VbClasificacion = "";
                    if (RdbProvdr.Checked == true) { VbClasificacion = "P"; }
                    if (RdbCliente.Checked == true) { VbClasificacion = "C"; }
                    if (RdbAmbos.Checked == true) { VbClasificacion = "A"; }
                    int VbClaseCta = 0;
                    if (RdbCtaAhorr.Checked == true) { VbClaseCta = 1; }
                    if (RdbCtaCte.Checked == true) { VbClaseCta = 2; }
                    if (RdbCtaNA.Checked == true) { VbClaseCta = 3; }

                    List<ClsTypTercero> ObjTercero = new List<ClsTypTercero>();
                    var TypTercero = new ClsTypTercero()
                    {
                        CodTercero = TxtNit.Text.Trim(),
                        RazonSocial = TxtRazonSoc.Text.Trim(),
                        Direccion = TxtDirecc.Text.Trim(),
                        Telefono = TxtTelef.Text.Trim(),
                        Fax = TxtFax.Text.Trim(),
                        Correo = TxtCorreo.Text.Trim(),
                        CodUbicaGeogr = Convert.ToInt32(DdlCiudad.Text.Trim()),
                        Estado = Convert.ToInt32(DdlEstado.Text.Trim()),
                        Comentario = TxtObservac.Text.Trim(),
                        CodClaseServicio = "",
                        CodTipoPago = DdlFormaPago.Text.Trim(),
                        CodTipo = "",
                        CodTipoProveedor = DdlTipTerce.Text.Trim(),
                        DireccionPago = "",
                        CodClasiJuridic = DdlClasfJurd.Text.Trim(),
                        CodTipoRegimen = DdlTipoRegmn.Text.Trim(),
                        PrecioFacturacion = "",
                        Cupo = Convert.ToDouble(0),
                        CodMoneda = DdllMoned.Text.Trim(),
                        CodTipoIdent = DdlTipoDoc.Text.Trim(),
                        Descuento = Convert.ToDouble(0),
                        DiasDescuento = Convert.ToInt32(0),
                        PagoA = "",
                        Clasificacion = VbClasificacion,
                        CuentaPuc = "",
                        CuentaPucProveedor = "",
                        Empleado = "",
                        Identificacion = "",
                        Activo = CkbActivo.Checked == true ? 1 : 0,
                        DigVerificacion = TxtDV.Text.Trim(),
                        CodPostal = TxtCodPostal.Text.Trim(),
                        Pais = DdlPais.Text.Trim(),
                        NroDeCta = TxtNroCta.Text.Trim(),
                        ClaseCta = VbClaseCta,
                        BcoBeneficiario = DdlBanco.Text.Trim(),
                        BcoCorresponsal = "",
                        SwiftCode = TxtSwift.Text.Trim(),
                        ABA = TxtAba.Text.Trim(),
                        NomContacto = "",
                        ApeContacto = "",
                        TipoPagoBanco = "",
                        CodTipoCodigo = "01",
                        CodClaseProv = DdlClase.Text.Trim(),
                        IVA = Convert.ToDouble(TxtIVA.Text.Trim().Equals("") ? "0" : TxtIVA.Text.Trim()),
                        IdTercero = Convert.ToInt32(ViewState["IdTercero"]),
                    };

                    ObjTercero.Add(TypTercero);
                    ClsTypTercero ClsTercero = new ClsTypTercero();
                    ClsTercero.Accion("UPDATE");
                    ClsTercero.Alimentar(ObjTercero);
                    string Mensj = ClsTercero.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true, "UPD");
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampos(false, false, "UPD");
                    LimpiarCampos("UPD");
                    ViewState["IdTercero"] = ClsTercero.GetPIdTercero().Trim();
                    Traerdatos(ClsTercero.GetPCodTercero().Trim(), "UPD");
                    BtnModificar.OnClientClick = "";
                    GrdContacto.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Tercero", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString();
            string StSql, VbNomRpt = "";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();

            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                string VbProv = "", VbClnt = "", VbAmb = "";
                DataRow[] Result;
                Result = Idioma.Select("Objeto= 'RdbProvdr'");
                foreach (DataRow row in Result)
                { VbProv = row["Texto"].ToString().Trim(); }

                Result = Idioma.Select("Objeto= 'RdbCliente'");
                foreach (DataRow row in Result)
                { VbClnt = row["Texto"].ToString().Trim(); }

                Result = Idioma.Select("Objeto= 'RdbAmbos'");
                foreach (DataRow row in Result)
                { VbAmb = row["Texto"].ToString().Trim(); }

                CursorIdioma.Alimentar("CurExportTercero", Session["77IDM"].ToString().Trim());
                StSql = "EXEC SP_PANTALLA_Tercero 2, @Pr, @Cl, @Amb,'CurExportTercero',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                using (SqlCommand SC = new SqlCommand(StSql, con))
                {
                    Result = Idioma.Select("Objeto= 'NomArchivExp'");
                    foreach (DataRow row in Result)
                    { VbNomRpt = row["Texto"].ToString().Trim(); }

                    SC.Parameters.AddWithValue("@Pr", VbProv);
                    SC.Parameters.AddWithValue("@Cl", VbClnt);
                    SC.Parameters.AddWithValue("@Amb", VbAmb);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
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
        protected void DdlPais_TextChanged(object sender, EventArgs e)
        { ViewState["CiudadAnt"] = "0"; ViewState["EstadoAnt"] = "0"; BindCiudad(DdlPais.Text.Trim()); }
        //****************************** MOdal Busq **************************************
        protected void BindModalBusqTercero()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbOpc = "1";
                DataTable DT = new DataTable();
                if (RdbMdlOpcBusqCod.Checked == true) { VbOpc = "1"; }
                else { VbOpc = "2"; }
                string VbTxtSql = "EXEC SP_PANTALLA_Tercero 1, @Txt,'','','',@Opc,1,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Txt", TxtModalBusq.Text.Trim());
                    SC.Parameters.AddWithValue("@Opc", VbOpc);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0) { GrdModalBusqTercero.DataSource = DT; }
                    else { GrdModalBusqTercero.DataSource = null; }
                    GrdModalBusqTercero.DataBind();
                }
            }
        }
        protected void IbtModalBusq_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            BindModalBusqTercero();
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup();", true);
        }
        protected void GrdModalBusqTercero_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ViewState["IdTercero"] = GrdModalBusqTercero.DataKeys[gvr.RowIndex].Values["IdTercero"].ToString();
                string VbCod = ((Label)row.FindControl("LblCodTrcr")).Text.ToString().Trim();
                Traerdatos(VbCod, "UPD");
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdModalBusqTercero_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIrCot = (e.Row.FindControl("IbtIr") as ImageButton);
                if (IbtIrCot != null)
                {
                    foreach (DataRow RowIdioma in Result) { IbtIrCot.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        //****************************** Detalle Contacto **************************************
        protected void BindDContacto()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            DT = DSTPpl.Tables[1].Clone();
            Result = DSTPpl.Tables[1].Select("Ppal <>3");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "Ppal DESC";
                DT = DV.ToTable();
                GrdContacto.DataSource = DT;
                GrdContacto.DataBind();
            }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdContacto.DataSource = DT;
                GrdContacto.DataBind();
                GrdContacto.Rows[0].Cells.Clear();
                GrdContacto.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdContacto.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdContacto.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdContacto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                if (TxtNit.Text.Equals(""))
                { return; }

                int VbPpl = (GrdContacto.FooterRow.FindControl("CkbPpalPP") as CheckBox).Checked == true ? 1 : 0;
                string VbNom = (GrdContacto.FooterRow.FindControl("TxtnomPP") as TextBox).Text.Trim();
                string VbApell = (GrdContacto.FooterRow.FindControl("TxtApellPP") as TextBox).Text.Trim();
                string VbTel = (GrdContacto.FooterRow.FindControl("TxtTelPP") as TextBox).Text.Trim();
                string VbMail = (GrdContacto.FooterRow.FindControl("TxtMailPP") as TextBox).Text.Trim();

                if (VbNom.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens15Tercro'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el nombre.
                    return;
                }
                if (VbApell.Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens16Tercro'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el apellido.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasLogistica 2, @Cd, @Nm, @Ap, @Tel, @Ml, @US,'','','INSERT',0,@Ppl,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@Cd", TxtNit.Text.Trim());
                            SC.Parameters.AddWithValue("@Nm", VbNom);
                            SC.Parameters.AddWithValue("@Ap", VbApell);
                            SC.Parameters.AddWithValue("@Tel", VbTel);
                            SC.Parameters.AddWithValue("@Ml", VbMail);
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@Ppl", VbPpl);
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
                                Traerdatos(TxtNit.Text.Trim(), "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdContacto_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdContacto.EditIndex = e.NewEditIndex; BindDContacto(); }
        protected void GrdContacto_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (TxtNit.Text.Equals(""))
            { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            int VblId = Convert.ToInt32(GrdContacto.DataKeys[e.RowIndex].Value.ToString());
            int VbPpl = (GrdContacto.Rows[e.RowIndex].FindControl("CkbPpal") as CheckBox).Checked == true ? 1 : 0;
            string VbNom = (GrdContacto.Rows[e.RowIndex].FindControl("Txtnom") as TextBox).Text.Trim();
            string VbApell = (GrdContacto.Rows[e.RowIndex].FindControl("TxtApell") as TextBox).Text.Trim();
            string VbTel = (GrdContacto.Rows[e.RowIndex].FindControl("TxtTel") as TextBox).Text.Trim();
            string VbMail = (GrdContacto.Rows[e.RowIndex].FindControl("TxtMail") as TextBox).Text.Trim();

            if (VbNom.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens15Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el nombre.
                return;
            }
            if (VbApell.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens16Tercro'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el apellido.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasLogistica 2, @Cd, @Nm, @Ap, @Tel, @Ml, @US,'','','UPDATE',@Id,@Ppl,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Cd", TxtNit.Text.Trim());
                        SC.Parameters.AddWithValue("@Nm", VbNom);
                        SC.Parameters.AddWithValue("@Ap", VbApell);
                        SC.Parameters.AddWithValue("@Tel", VbTel);
                        SC.Parameters.AddWithValue("@Ml", VbMail);
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Id", VblId);
                        SC.Parameters.AddWithValue("@Ppl", VbPpl);
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
                            GrdContacto.EditIndex = -1;
                            Traerdatos(TxtNit.Text.Trim(), "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdContacto_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdContacto.EditIndex = -1; BindDContacto(); }
        protected void GrdContacto_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbId = GrdContacto.DataKeys[e.RowIndex].Values["IdContacto"].ToString();
            string VbPpl = GrdContacto.DataKeys[e.RowIndex].Values["Ppal"].ToString();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_TablasLogistica 2, @Cd, '','','','', @US,'','','DELETE',@Id, @Ppl,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Cd", TxtNit.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Id", VbId);
                        SC.Parameters.AddWithValue("@Ppl", VbPpl);
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
                            Traerdatos(TxtNit.Text.Trim(), "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdContacto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
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
                }
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
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
        }
    }
}