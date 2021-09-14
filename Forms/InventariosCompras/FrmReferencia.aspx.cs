using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmReferencia : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTPN = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSTDdlPrmtr = new DataSet();
        DataSet DSTUndCompra = new DataSet();
        private int VbPos, VbConsu, VbMot, VbMay, VbApu, VbSuC, VbRepa, VbVerif, VbNif;
        float VblStock;
        string VbCod, VbGrup, VbAta, VbUm, VbIdent, VbTip, VbDes, VbModel, VbDescEsp, VbInfAd, PVbCat;
        private string LblCUMCP, LblCEquP;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
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
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            if (!IsPostBack)
            {
                Page.Title = string.Format("Referencia");// Titulo del form
                Session["VlRefer"] = "S";
                ViewState["VbPNSI"] = "";
                ModSeguridad();
                ActivarCampos(false, false, "");
                ActivarBotones(true, false, false, false, true);
                BindDdl("UPD");
                BindDataDdl("", "");
                ViewState["NewRef"] = "";
                ViewState["CRUD"] = "";
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["CE5ActivoNif"] = 1;
            ViewState["CE1Anular"] = 1;
            ViewState["CambioRef"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmReferencia.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                BtnIngresar.Visible = false;
                GrdMan.ShowFooter = false;
                GrdPN.ShowFooter = false;
                GrdCont.ShowFooter = false;
                GrdCamUC.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnModificar.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
                BtnConsultar.Visible = false;
            }
            if (ClsP.GetImprimir() == 0)
            {
                BtnInformes.Visible = false;
            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                BtnEliminar.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {
                ViewState["CE1Anular"] = 0;
            }
            if (ClsP.GetCE2() == 0)
            {

            }
            if (ClsP.GetCE3() == 0)
            {
            }
            if (ClsP.GetCE4() == 0)
            {
                //cambio de referencia
                BtnCambioRef.Visible = false;
            }
            if (ClsP.GetCE5() == 0)
            {
                ViewState["ActivoNif"] = 0;
            }
            if (ClsP.GetCE6() == 0)
            {
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string TxQry = "EXEC SP_ConfiguracionV2_ 19,'FrmReferencianew','FrmReferencianew','','','" + Session["Nit77Cia"].ToString() + "',2,3,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                sqlCon.Open();
                SqlDataReader Regs = Comando.ExecuteReader();
                while (Regs.Read())
                {
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    if (VbCaso == 2 && VbAplica.Equals("S"))
                    {
                        //Manejo de Kit
                    }
                    if (VbCaso == 3 && VbAplica.Equals("S"))
                    {
                        //Nif
                        CkbNiF.Visible = true;
                    }
                }
            }
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
                SC.Parameters.AddWithValue("@F1", "FRMREFERENCIANEW");
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
                    if (bO.Equals("CaptionRef"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("TituloRef") ? bT : TitForm.Text;
                    LblCodigo.Text = bO.Equals("LblCodigo") ? bT + ":" : LblCodigo.Text;
                    LblGrupo.Text = bO.Equals("LblGrupo") ? bT + ":" : LblGrupo.Text;
                    LblModelo.Text = bO.Equals("LblModelo") ? bT + ":" : LblModelo.Text;
                    LblUndDesp.Text = bO.Equals("LblUndDesp") ? bT + ":" : LblUndDesp.Text;
                    LblIdentElem.Text = bO.Equals("LblIdentElem") ? bT + ":" : LblIdentElem.Text;
                    LblDescripc.Text = bO.Equals("LblDescripc") ? bT + ":" : LblDescripc.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT + ":" : LblTipo.Text;
                    LblDescEsp.Text = bO.Equals("LblDescEsp") ? bT + ":" : LblDescEsp.Text;
                    LblInfoAdic.Text = bO.Equals("LblInfoAdic") ? bT + ":" : LblInfoAdic.Text;
                    LblReparab.Text = bO.Equals("LblReparab") ? bT + ":" : LblReparab.Text;
                    LblSi.Text = bO.Equals("LblSi") ? bT : LblSi.Text;
                    CkbPos.Text = bO.Equals("CkbPos") ? "&nbsp " + bT : CkbPos.Text;
                    CkbCons.Text = bO.Equals("CkbCons") ? "&nbsp " + bT : CkbCons.Text;
                    CkbMot.Text = bO.Equals("CkbMot") ? "&nbsp " + bT : CkbMot.Text;
                    CkbMay.Text = bO.Equals("CkbMay") ? "&nbsp " + bT : CkbMay.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    BtnInformes.Text = bO.Equals("BtnInformes") ? bT : BtnInformes.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    BtnUndCompra.Text = bO.Equals("BtnUndCompra") ? bT : BtnUndCompra.Text;
                    BtnCambioRef.Text = bO.Equals("BtnCambioRef") ? bT : BtnCambioRef.Text;
                    GrdPN.Columns[1].HeaderText = bO.Equals("GrdEstad") ? bT : GrdPN.Columns[1].HeaderText;
                    GrdPN.Columns[2].HeaderText = bO.Equals("GrdBloq") ? bT : GrdPN.Columns[2].HeaderText;
                    GrdPN.Columns[4].HeaderText = bO.Equals("BtnUndCompra") ? bT : GrdPN.Columns[4].HeaderText;
                    GrdPN.Columns[5].HeaderText = bO.Equals("GrdEquiv") ? bT : GrdPN.Columns[5].HeaderText;
                    GrdPN.Columns[6].HeaderText = bO.Equals("GrdFecVnc") ? bT : GrdPN.Columns[6].HeaderText;
                    GrdPN.Columns[7].HeaderText = bO.Equals("GrdFabric") ? bT : GrdPN.Columns[7].HeaderText;
                    GrdCont.Columns[0].HeaderText = bO.Equals("GrdContdr") ? bT : GrdCont.Columns[0].HeaderText;
                    LblTitMaesArt.Text = bO.Equals("LblTitMaesArt") ? bT : LblTitMaesArt.Text;
                    LblStokMin.Text = bO.Equals("LblStokMin") ? bT + ":" : LblStokMin.Text;
                    CkbVerif.Text = bO.Equals("CkbVerif") ? "&nbsp " + bT : CkbVerif.Text;
                    LblCateg.Text = bO.Equals("LblCateg") ? bT + ":" : LblCateg.Text;
                    GrdMan.Columns[0].HeaderText = bO.Equals("GrdCondMan") ? bT : GrdMan.Columns[0].HeaderText;
                    // ************************************** busqueda  *******************************************************       
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    RdbBusqR.Text = bO.Equals("RdbBusqR") ? "&nbsp " + bT : RdbBusqR.Text;
                    RdbBusqD.Text = bO.Equals("LblDescripc") ? "&nbsp " + bT : RdbBusqD.Text;
                    LblBusqueda.Text = bO.Equals("Busqueda") ? bT : LblBusqueda.Text;
                    if (bO.Equals("placeholderDC"))
                    {
                        TxtBusqueda.Attributes.Add("placeholder", bT);
                        TxtCambRef.Attributes.Add("placeholder", bT);
                    }
                    IbtConsultar.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsultar.ToolTip;
                    IbtCerrar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrar.ToolTip;
                    GrdDatos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDatos.EmptyDataText;
                    // ************************************** Asignar UND Compra  *******************************************************       
                    LblTitAsigUndMed.Text = bO.Equals("LblTitAsigUndMed") ? bT : LblTitAsigUndMed.Text;
                    IbtCerrarUMC.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarUMC.ToolTip;
                    GrdCamUC.Columns[0].HeaderText = bO.Equals("BtnUndCompra") ? bT : GrdCamUC.Columns[0].HeaderText;
                    GrdCamUC.Columns[1].HeaderText = bO.Equals("GrdEquiv") ? bT : GrdCamUC.Columns[1].HeaderText;
                    GrdCamUC.Columns[2].HeaderText = bO.Equals("LblUndDesp") ? bT : GrdCamUC.Columns[2].HeaderText;
                    // ************************************** Cambio referencia  *******************************************************       
                    LblTitCambRef.Text = bO.Equals("LblTitCambRef") ? bT : LblTitCambRef.Text;
                    RdbRefCRef.Text = bO.Equals("RdbBusqR") ? "&nbsp " + bT : RdbRefCRef.Text;
                    LblBusqCambRef.Text = bO.Equals("Busqueda") ? bT : LblBusqCambRef.Text;
                    IbtConsultarCambRef.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtConsultarCambRef.ToolTip;
                    IbtApliarCambRef.ToolTip = bO.Equals("IbtApliarCambRefTT") ? bT : IbtApliarCambRef.ToolTip;
                    IbtCerrarCambRef.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCambRef.ToolTip;
                    GrdCambioRef.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdCambioRef.EmptyDataText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                Result = Idioma.Select("Objeto= 'IbtApliarCambRefOnCl'");
                foreach (DataRow row in Result)
                { IbtApliarCambRef.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDdl(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_ReferenciaV2 19,@CTC,'','','','',0,0, @Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@CTC", Session["CodTipoCodigoInicial"].ToString());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "GRUP";
                                DSTDdl.Tables[1].TableName = "Ata";
                                DSTDdl.Tables[2].TableName = "Identific";
                                DSTDdl.Tables[3].TableName = "TIPO";
                                DSTDdl.Tables[4].TableName = "Modelo";
                                DSTDdl.Tables[5].TableName = "UndMed";
                                DSTDdl.Tables[6].TableName = "Categoria";
                                DSTDdl.Tables[7].TableName = "EstadoPN";
                                DSTDdl.Tables[8].TableName = "Fabricante";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            string VbCodAnt = "";

            VbCodAnt = DdlGrupo.Text.Trim();
            DdlGrupo.DataSource = DSTDdl.Tables[0];
            DdlGrupo.DataTextField = "Descripcion";
            DdlGrupo.DataValueField = "CodTipoElemento";
            DdlGrupo.DataBind();
            DdlGrupo.Text = VbCodAnt;

            VbCodAnt = DdlAta.Text.Trim();
            DdlAta.DataSource = DSTDdl.Tables[1];
            DdlAta.DataTextField = "Descripcion";
            DdlAta.DataValueField = "CodCapitulo";
            DdlAta.DataBind();
            DdlAta.Text = VbCodAnt;

            VbCodAnt = DdlIdent.Text.Trim();
            DdlIdent.DataSource = DSTDdl.Tables[2];
            DdlIdent.DataTextField = "Identificador";
            DdlIdent.DataValueField = "Codigo";
            DdlIdent.DataBind();
            DdlIdent.Text = VbCodAnt;

            VbCodAnt = DdlTipo.Text.Trim();
            DdlTipo.DataSource = DSTDdl.Tables[3];
            DdlTipo.DataTextField = "Descripcion";
            DdlTipo.DataValueField = "CodTipoCodigo";
            DdlTipo.DataBind();
            DdlTipo.Text = VbCodAnt;

            VbCodAnt = DdlMod.Text.Trim();
            DdlMod.DataSource = DSTDdl.Tables[4];
            DdlMod.DataTextField = "Descripcion";
            DdlMod.DataValueField = "CodModelo";
            DdlMod.DataBind();
            DdlMod.Text = VbCodAnt;
        }
        protected void BindDataDdl(string UM, string Ctgr)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result;
            string VbCodAnt = "";
            DataTable DTUM = new DataTable();
            DataTable DTCTG = new DataTable();

            VbCodAnt = UM.Trim();
            DTUM = DSTDdl.Tables[5].Clone();
            Result = DSTDdl.Tables[5].Select("CodUnidMedida='" + UM.Trim() + "'");// trae el codigo actual por si esta inactivo
            foreach (DataRow Row in Result)
            { DTUM.ImportRow(Row); }

            Result = DSTDdl.Tables[5].Select("ActivoUM=1");
            foreach (DataRow Row in Result)
            { DTUM.ImportRow(Row); }

            DdlUM.DataSource = DTUM;
            DdlUM.DataTextField = "Descripcion";
            DdlUM.DataValueField = "CodUnidMedida";
            DdlUM.DataBind();
            DdlUM.Text = VbCodAnt;

            VbCodAnt = Ctgr.Trim();
            DTCTG = DSTDdl.Tables[6].Clone();
            Result = DSTDdl.Tables[6].Select("CodCategoriaMA='" + Ctgr.Trim() + "'");// trae el codigo actual por si esta inactivo
            foreach (DataRow Row in Result)
            { DTCTG.ImportRow(Row); }

            Result = DSTDdl.Tables[6].Select("Activo=1");
            foreach (DataRow Row in Result)
            { DTCTG.ImportRow(Row); }

            DdlCat.DataSource = DTCTG;
            DdlCat.DataTextField = "Descripcion";
            DdlCat.DataValueField = "CodCategoriaMA";
            DdlCat.DataBind();
            DdlCat.Text = VbCodAnt;
        }
        protected void BindDataAll(string VblRef, string VblPN)
        { BtnUndCompra.Enabled = false; BtnCambioRef.Enabled = false; }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdMan.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[1].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdPN.Rows)
            {

                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[8].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdCont.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[1].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdCamUC.Rows)
            {

                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[3].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdCont.Rows)
            {
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[1].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void GenerarCodigo(string Grup, string Model, string ata)
        {
            string VbCodModelo = Model;
            string VblGrupo = Grup.Trim();
            string VbCapit = ata.Trim();
            if (VbCapit.Equals(""))
            {
                VbCapit = "";
            }
            else
            {
                VbCapit = "-" + ata.Substring(2, 2);
            }
            if (VblGrupo.Equals("01"))
            {
                VblGrupo = "M";
            }
            else if (VblGrupo.Equals("02"))
            {
                VblGrupo = "C";
            }
            else if (VblGrupo.Equals("03"))
            {
                VblGrupo = "H";
            }

            if (Model.Equals(""))
            {
                VbCodModelo = "STD";
            }
            if (TxtCod.Text.Length < 8)
            { TxtCod.Text = VblGrupo + VbCodModelo + VbCapit; }

        }
        protected void BusqNewReg(string Ref)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = String.Format("SELECT TOP 1 CodReferencia FROM TblReferencia WHERE SUBSTRing(RTRIM(CodReferencia),1,7)='{0}' AND IdConfigCia = @ICC ORDER BY IdReferencia desc ", Ref);
                SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlCon.Open();
                SqlDataReader tbl = Comando.ExecuteReader();
                if (tbl.Read())
                { TxtCod.Text = tbl["CodReferencia"].ToString(); }
                else
                { LimpiarCampos(); }
            }
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            ViewState["CodUndMedD"] = "S";
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 8,'{0}','','','','',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
                SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                SqlC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader tbl = SqlC.ExecuteReader();
                if (tbl.Read())
                {
                    if (accion.Equals("Ingresar"))
                    {
                        if (tbl["HMC"].ToString().Equals("N") && tbl["HMM"].ToString().Equals("N"))
                        { CkbApu.Enabled = Edi; }
                        DdlUM.Enabled = Edi;
                    }
                    if (accion.Equals("Modificar"))
                    {
                        if ((int)ViewState["CE1Anular"] == 1)
                        {
                            DdlTipo.Enabled = Edi;
                            if (CkbVerif.Checked == false || CkbVerif.Enabled == true)
                            { CkbVerif.Enabled = Edi; }
                        }
                    }
                    if (tbl["ElE"].ToString().Equals("N") && tbl["SPE"].ToString().Equals("N"))
                    { DdlIdent.Enabled = Edi; DdlGrupo.Enabled = Ing; DdlMod.Enabled = Edi; }
                    DdlAta.Enabled = Edi;
                    TxtDesc.Enabled = Edi;
                    TxtDescEsp.Enabled = Edi;
                    TxtInfAd.Enabled = Edi;
                    RdbNo.Enabled = Edi;
                    RdbSi.Enabled = Edi;
                    CkbPos.Enabled = Edi;
                    CkbCons.Enabled = Edi;
                    CkbMot.Enabled = Edi;
                    CkbMay.Enabled = Edi;
                    CkbSub.Enabled = Edi;
                    DdlCat.Enabled = Edi;

                    DdlCat.Enabled = Edi;
                    if (!DdlIdent.SelectedValue.Equals("SN") && (int)ViewState["CE5ActivoNif"] == 1)
                    { CkbNiF.Enabled = Edi; }
                }
            }
            if (accion.Equals("Modificar"))
            {
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 1,'{0}','{1}','','','',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'", TxtCod.Text, DdlUM.SelectedValue);
                    SqlCommand SqlC2 = new SqlCommand(LtxtSql, Cnx2);
                    SqlC2.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader DR2 = SqlC2.ExecuteReader();
                    if (DR2.Read())
                    {
                        if (DR2["Se_Puede_Mod_UM"].ToString().Equals("S"))
                        { DdlUM.Enabled = Edi; }
                        else { DdlUM.ToolTip = DR2["Motivo"].ToString(); ViewState["CodUndMedD"] = "N"; }
                    }
                }
            }
            using (SqlConnection Cnx3 = new SqlConnection(Cnx.GetConex()))
            {
                ViewState["Apu"] = 'S';
                Cnx3.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 10,'{0}','','','','ACTIVA-APU',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
                SqlCommand SqlC2 = new SqlCommand(LtxtSql, Cnx3);
                SqlC2.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader DR3 = SqlC2.ExecuteReader();
                if (DR3.Read())
                {
                    if (DR3["ActApu"].ToString().Equals("N") && DR3["HisMy"].ToString().Equals("N"))
                    { CkbApu.Enabled = Edi; ViewState["Apu"] = 'N'; }

                }
            }
        }
        protected void LimpiarCampos()
        {
            TxtCod.Text = "";
            DdlGrupo.Text = "";
            DdlAta.Text = "";
            DdlUM.Text = "";
            DdlIdent.Text = "";
            DdlTipo.Text = "";
            TxtDesc.Text = "";
            DdlMod.Text = "";
            TxtDescEsp.Text = "";
            TxtInfAd.Text = "";
            RdbSi.Checked = false;
            RdbNo.Checked = false;
            CkbPos.Checked = false;
            CkbCons.Checked = false;
            CkbMot.Checked = false;
            CkbMay.Checked = false;
            CkbApu.Checked = false;
            CkbSub.Checked = false;
            CkbVerif.Checked = false;
            DdlCat.Text = "";
            BindDataAll("", "");
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            BtnConsultar.Enabled = Otr;
            BtnInformes.Enabled = Otr;
            GrdMan.Enabled = Otr;
            GrdPN.Enabled = Otr;
            GrdCont.Enabled = Otr;
            BindDataAll(TxtCod.Text, "");
        }
        protected void AsignarValores(string Accion)
        {
            Session["VlRefer"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlGrupo.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un grupo
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlAta.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un ata')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlUM.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una unidad de despacho')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlIdent.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un identificador')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlTipo.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar un tipo')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (TxtDesc.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una descripción')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (TxtDescEsp.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //Debe ingresar una descripción')", true);
                Session["VlRefer"] = "N";
                return;
            }
            VbCod = TxtCod.Text;
            VbGrup = DdlGrupo.SelectedValue;
            VbAta = DdlAta.SelectedValue;
            VbModel = DdlMod.SelectedValue;
            VbUm = DdlUM.SelectedValue;
            VbIdent = DdlIdent.SelectedValue;
            VbDes = TxtDesc.Text;
            VbDescEsp = TxtDescEsp.Text;
            VbTip = DdlTipo.SelectedValue;
            VbInfAd = TxtInfAd.Text;
            VblStock = TxtStockM.Text.Trim() == string.Empty ? 0 : (float)Convert.ToDouble(TxtStockM.Text);
            VbVerif = CkbVerif.Checked == true ? 1 : 0;
            PVbCat = DdlCat.SelectedValue;
            VbRepa = 0;
            if (RdbSi.Checked == true)
            {
                VbRepa = 1;
            }
            else if (RdbNo.Checked == true)
            {
                VbRepa = 2;
            }
            VbPos = CkbPos.Checked == true ? 1 : 0;
            VbConsu = CkbCons.Checked == true ? 1 : 0;
            VbMot = CkbMot.Checked == true ? 1 : 0;
            if (VbMot == 1) { VbMay = 1; }
            else { VbMay = CkbMay.Checked == true ? 1 : 0; }
            VbApu = CkbApu.Checked == true ? 1 : 0;
            VbSuC = CkbSub.Checked == true ? 1 : 0;

            if (VbMay == 1)
            {
                VbSuC = 0;
                VbConsu = 0;
            }
            if (VbSuC == 1 || VbApu == 1)
            {
                VbConsu = 0;
            }
            VbNif = CkbNiF.Checked == true ? 1 : 0;
        }
        protected void DdlGrupo_TextChanged(object sender, EventArgs e)
        {
            GenerarCodigo(DdlGrupo.SelectedValue, DdlMod.SelectedValue, DdlAta.SelectedValue);
            switch (DdlGrupo.SelectedValue)
            {
                case "01":
                    if (ViewState["CodUndMedD"].ToString() == "S")
                    { DdlUM.Enabled = true; DdlUM.SelectedValue = ""; }
                    DdlIdent.Enabled = true;
                    DdlIdent.Text = "";
                    break;
                default:
                    DdlUM.Enabled = false;
                    DdlUM.SelectedValue = "EA";
                    DdlIdent.Enabled = false;
                    DdlIdent.Text = "SN";
                    break;
            }
            if (DdlGrupo.SelectedValue.Equals("02"))
            {
                CkbMot.Enabled = true;
                CkbMay.Enabled = true;
                if (ViewState["Apu"].ToString().Equals("N"))
                { CkbApu.Enabled = true; }
                CkbSub.Enabled = true;
            }
            else
            {
                CkbMot.Enabled = false;
                CkbMay.Enabled = false;
                CkbApu.Enabled = false;
                CkbSub.Enabled = false;
                CkbMot.Checked = false;
                CkbMay.Checked = false;
                CkbApu.Checked = false;
                CkbSub.Checked = false;
            }
            BindDataAll("", "");
        }
        protected void DdlAta_TextChanged(object sender, EventArgs e)
        {
            GenerarCodigo(DdlGrupo.SelectedValue, DdlMod.SelectedValue, DdlAta.SelectedValue);
            BindDataAll("", "");
        }
        protected void DdlMod_TextChanged(object sender, EventArgs e)
        {
            GenerarCodigo(DdlGrupo.SelectedValue, DdlMod.SelectedValue, DdlAta.SelectedValue);
            BindDataAll("", "");
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["CRUD"].ToString().Equals(""))
            {
                ActivarBotones(true, false, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                ViewState["CRUD"] = "Aceptar";
                ActivarCampos(true, true, "Ingresar");
                LimpiarCampos();
                DdlTipo.Text = "01";
                GrdPN.DataSource = null; GrdPN.DataBind();
                GrdMan.DataSource = null; GrdMan.DataBind();
                GrdCont.DataSource = null; GrdCont.DataBind();
                Result = Idioma.Select("Objeto= 'MensConfIng'");
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }
            }
            else
            {
                try
                {
                    AsignarValores("Ingresar");
                    if (Session["VlRefer"].ToString() == "N")
                    { BindDataAll(TxtCod.Text, ""); return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        List<CsTypeReferencia> ObjTTRef = new List<CsTypeReferencia>();
                        var detail = new CsTypeReferencia()
                        {
                            CodReferencia = TxtCod.Text,
                            TipoGo = 0,
                            TipoNoGo = 0,
                            Descripcion = VbDes.Trim(),
                            DescripEsp = VbDescEsp.Trim(),
                            Usu = Session["C77U"].ToString(),
                            CodTipoCodigo = VbTip,
                            Reparable = VbRepa,
                            CodTipoElemento = VbGrup,
                            IdCia = 1,
                            StockMin = VblStock,
                            TipoAnt = "",
                            FechaCambioTipo = null,
                            UsuarioModiTipo = "",
                            CodUndMedR = VbUm,
                            CodKitR = "",
                            ConsumoR = VbConsu,
                            MotorR = VbMot,
                            IdentificadorElemR = VbIdent,
                            CodcapituloR = VbAta,
                            SubComponenteR = VbSuC,
                            ComponenteMayorR = VbMay,
                            PosicionPnR = VbPos,
                            APU = VbApu,
                            FechaVencimientoR = 0,
                            Revisado = VbVerif,
                            CodCategoria = PVbCat,
                            Calibracion = 1,
                            ModeloRef = VbInfAd.Trim(),
                            ActivoNIF = VbNif,
                            SP_StockMin = "",
                            CodModeloR = VbModel,
                        };
                        ObjTTRef.Add(detail);
                        CsTypeReferencia TblPN = new CsTypeReferencia();
                        TblPN.Accion("INSERT");
                        TblPN.Insert(ObjTTRef);
                        BindDataAll(TxtCod.Text, "");
                    }
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    BtnIngresar.OnClientClick = "";
                    BindDataAll("", "");
                    BusqNewReg(VbCod);
                    BindDataPN("", "UPD");
                    BindDataCont("");
                    BindDataMan("");

                    ViewState["CRUD"] = "";
                }
                catch (Exception ex)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); } //
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void IbtCerrar_Click(object sender, ImageClickEventArgs e)
        {
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BIndDataBusq(TxtBusqueda.Text); }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["CRUD"].ToString().Equals(""))
            {
                ActivarBotones(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                    ViewState["CRUD"] = "Modif";
                ActivarCampos(false, true, "Modificar");
                Result = Idioma.Select("Objeto= 'IbtUpdateOnC'");
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }// Desea realizar la actualización?');";
            }
            else
            {
                try
                {
                    AsignarValores("Modificar");
                    if (Session["VlRefer"].ToString() == "N")
                    {
                        BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                        return;
                    }
                    List<CsTypeReferencia> ObjTTRef = new List<CsTypeReferencia>();
                    var detail = new CsTypeReferencia()
                    {
                        CodReferencia = TxtCod.Text,
                        TipoGo = 0,
                        TipoNoGo = 0,
                        Descripcion = VbDes.Trim(),
                        DescripEsp = VbDescEsp.Trim(),
                        Usu = Session["C77U"].ToString(),
                        CodTipoCodigo = VbTip,
                        Reparable = VbRepa,
                        CodTipoElemento = VbGrup,
                        IdCia = 1,
                        StockMin = VblStock,
                        TipoAnt = "",
                        FechaCambioTipo = null,
                        UsuarioModiTipo = "",
                        CodUndMedR = VbUm,
                        CodKitR = "",
                        ConsumoR = VbConsu,
                        MotorR = VbMot,
                        IdentificadorElemR = VbIdent,
                        CodcapituloR = VbAta,
                        SubComponenteR = VbSuC,
                        ComponenteMayorR = VbMay,
                        PosicionPnR = VbPos,
                        APU = VbApu,
                        FechaVencimientoR = 0,
                        Revisado = VbVerif,
                        CodCategoria = PVbCat,
                        Calibracion = 1,
                        ModeloRef = VbInfAd.Trim(),
                        ActivoNIF = VbNif,
                        SP_StockMin = "",
                        CodModeloR = VbModel,
                    };
                    ObjTTRef.Add(detail);
                    CsTypeReferencia TblModRef = new CsTypeReferencia();
                    TblModRef.Accion("UPDATE");
                    TblModRef.Insert(ObjTTRef);
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string ProcesarPLano = CkbVerif.Enabled == true ? "S" : "N";
                        sqlCon.Open();
                        string VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 14,'{0}','{1}','{2}','','',{3},0,0,@ICC,'01-01-01','02-01-01','03-01-01'",
                        TxtCod.Text, ProcesarPLano, Session["C77U"].ToString(), VbVerif);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "Planos Referencia", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtEdit'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    BtnModificar.OnClientClick = "";
                    BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                    ViewState["CRUD"] = "";
                }
                catch (Exception ex)
                {
                    DataRow[] Result4 = Idioma.Select("Objeto= 'MensErrMod'");
                    foreach (DataRow row in Result4)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "')", true); }
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            RdbBusqR.Checked = true;
            TxtBusqueda.Text = "";
            BIndDataBusq("77NEO");
            PnlCampos.Visible = false;
            PnlBusq.Visible = true;
            PnlUnidadCompra.Visible = false;
        }
        protected void BtnInformes_Click(object sender, EventArgs e)
        {
            string VbNomRpt = "Reference";
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurValidado", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_PANTALLA_ReferenciaV2 16,'CurValidado','','','','',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
                {
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
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string VBQuery;
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 11,'{0}','','','','VALIDA',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", TxtCod.Text, Session["!dC!@"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        string Mensj = DAE["Mensj"].ToString();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transc = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 11,'{0}','{1}','','','DELETE',0,0,0,{2},'01-01-01','02-01-01','03-01-01'", TxtCod.Text, Session["C77U"].ToString(), Session["!dC!@"]);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transc))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                                Transc.Commit();
                                LimpiarCampos();
                                ActivarBotones(true, false, false, false, true);
                                BindDataAll(TxtCod.Text, "");

                            }
                            catch (Exception ex)
                            {
                                Transc.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE EN GENERAL", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        // *********************************** BUSQUEDAS ***********************************
        protected void BIndDataBusq(string Prmtr)
        {
            try
            {
                BtnUndCompra.Enabled = false;
                BtnCambioRef.Enabled = false;
                DataTable DtB = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                    CursorIdioma.Alimentar("CurBusqRef", Session["77IDM"].ToString().Trim());
                    string VbTxtSql;
                    VbTxtSql = "";
                    if (RdbBusqR.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'{0}','','','','CurBusqRef',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", Prmtr, Session["!dC!@"]);
                    }
                    if (RdbBusqP.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'','{0}','','','CurBusqRef',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", Prmtr, Session["!dC!@"]);
                    }
                    if (RdbBusqD.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'','','{0}','','CurBusqRef',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", Prmtr, Session["!dC!@"]);
                    }
                    if (!VbTxtSql.Equals(""))
                    {
                        sqlConB.Open();
                        SqlDataAdapter DAB = new SqlDataAdapter(VbTxtSql, sqlConB);
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        { GrdDatos.DataSource = DtB; GrdDatos.DataBind(); }
                        else
                        { GrdDatos.DataSource = null; GrdDatos.DataBind(); }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BIndDataBusq", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtCod.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[1].Text);
                DdlGrupo.Text = GrdDatos.SelectedRow.Cells[4].Text;
                DdlAta.Text = GrdDatos.SelectedRow.Cells[6].Text;
                DdlMod.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[8].Text).Trim();
                string VbCat = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[26].Text).Trim(); ;
                string VbCodUM = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[10].Text);
                BindDataDdl(VbCodUM, VbCat);
                DdlIdent.Text = GrdDatos.SelectedRow.Cells[12].Text;
                TxtDesc.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[3].Text);
                TxtDescEsp.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[3].Text);
                DdlTipo.Text = GrdDatos.SelectedRow.Cells[13].Text;
                TxtInfAd.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[15].Text);
                string VblReparable = GrdDatos.SelectedRow.Cells[16].Text;
                if (VblReparable.Equals("N/A"))
                { RdbSi.Checked = false; RdbNo.Checked = false; }
                else if (VblReparable.Equals("S"))
                { RdbSi.Checked = true; RdbNo.Checked = false; }
                else
                { RdbSi.Checked = false; RdbNo.Checked = true; }
                CkbPos.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[17].Text) == 1 ? true : false;
                CkbCons.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[18].Text) == 1 ? true : false;
                CkbMot.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[19].Text) == 1 ? true : false;
                CkbMay.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[20].Text) == 1 ? true : false;
                CkbApu.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[21].Text) == 1 ? true : false;
                CkbSub.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[22].Text) == 1 ? true : false;
                CkbNiF.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[23].Text) == 1 ? true : false;
                TxtStockM.Text = Convert.ToDouble(GrdDatos.SelectedRow.Cells[24].Text).ToString();
                CkbVerif.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[25].Text) == 1 ? true : false;

                BindDataAll(TxtCod.Text, "");
                BindDataPN(TxtCod.Text, "UPD");
                BindDataCont("");
                BindDataMan(TxtCod.Text);

                PerfilesGrid();
                PnlBusq.Visible = false;
                PnlCampos.Visible = true;
                ActivarBotones(true, true, true, true, true);
            }
            catch (Exception ex)
            {
                string VbMEns = ex.ToString();
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + VbMEns + "')", true);
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Style.Value = "min-width:150px;";
                e.Row.Cells[2].Style.Value = "min-width:250px;";
                e.Row.Cells[3].Style.Value = "min-width:250px;";
                e.Row.Cells[4].Style.Value = "min-width:250px;";
                e.Row.Cells[8].Style.Value = "min-width:250px;";
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDatos.PageIndex = e.NewPageIndex; BIndDataBusq(TxtBusqueda.Text); }
        // *********************************** Manipulacion y almacenamiento ***********************************
        protected void BindDataMan(string Ref)
        {
            try
            {
                BtnUndCompra.Enabled = false;
                BtnCambioRef.Enabled = false;
                DSTDdlPrmtr = (DataSet)ViewState["DSTDdlPrmtr"];
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DtMan = new DataTable();
                DataRow[] Result;

                DtMan = DSTDdlPrmtr.Tables[3].Clone();
                Result = DSTDdlPrmtr.Tables[3].Select("CodReferencia='" + Ref.Trim() + "'");
                foreach (DataRow Row in Result) { DtMan.ImportRow(Row); }

                if (DtMan.Rows.Count > 0)
                { GrdMan.DataSource = DtMan; GrdMan.DataBind(); }
                else
                {
                    DtMan.Rows.Add(DtMan.NewRow());
                    GrdMan.DataSource = DtMan;
                    GrdMan.DataBind();
                    GrdMan.Rows[0].Cells.Clear();
                    GrdMan.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdMan.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdMan.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                string Mensje = Ex.Message;
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensje + "')", true);
            }
        }
        protected void GrdMan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            PerfilesGrid();
            if (TxtCod.Text.Equals("")) { BindDataAll(TxtCod.Text, ""); return; }
            if (e.CommandName.Equals("AddNew"))
            {
                string VbDesc, VBQuery;
                VbDesc = (GrdMan.FooterRow.FindControl("DdlManPP") as DropDownList).Text.Trim();
                if (VbDesc == String.Empty)
                {
                    BindDataAll(TxtCod.Text, "");
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens34Ref'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }// Debe seleccionar un item')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 7, @Cod, @Ref, @VbUsu,'','INSERT',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@Cod", VbDesc);
                                sqlCmd.Parameters.AddWithValue("@Ref", TxtCod.Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDataPN(TxtCod.Text.Trim(), "UPD");
                                BindDataMan(TxtCod.Text.Trim());
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdMan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 7,'','','','','DELETE',@id,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@id", GrdMan.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataPN(TxtCod.Text.Trim(), "UPD");
                            BindDataMan(TxtCod.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdMan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','{0}','','','CON',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", TxtCod.Text, Session["!dC!@"]);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlManPP = (e.Row.FindControl("DdlManPP") as DropDownList);
                DdlManPP.DataSource = Cnx.DSET(LtxtSql);
                DdlManPP.DataTextField = "Descripcion";
                DdlManPP.DataValueField = "CodManipulacion";
                DdlManPP.DataBind();
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    imgD.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                { e.Row.Cells[1].Controls.Remove(imgD); }
            }
        }
        protected void GrdMan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdMan.PageIndex = e.NewPageIndex; BindDataMan(TxtCod.Text.Trim()); }
        // *********************************** P/N ***********************************
        protected void BindDataPN(string Ref, string Accion)
        {
            try
            {
                BtnUndCompra.Enabled = false;
                BtnCambioRef.Enabled = false;
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC SP_PANTALLA_ReferenciaV2 20, @Rf,@CUM,'','','',0,0, @Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            SC.Parameters.AddWithValue("@Rf", Ref.Trim());
                            SC.Parameters.AddWithValue("@CUM", DdlUM.Text.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTDdlPrmtr = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTDdlPrmtr);
                                    DSTDdlPrmtr.Tables[0].TableName = "DETPN";
                                    DSTDdlPrmtr.Tables[1].TableName = "CodUMDetPN";
                                    DSTDdlPrmtr.Tables[2].TableName = "DataCntdrAsig";
                                    DSTDdlPrmtr.Tables[3].TableName = "Manipulac";

                                    ViewState["DSTDdlPrmtr"] = DSTDdlPrmtr;
                                }
                            }
                        }
                    }
                }
                DSTDdlPrmtr = (DataSet)ViewState["DSTDdlPrmtr"];

                if (DSTDdlPrmtr.Tables[0].Rows.Count > 0)
                { GrdPN.DataSource = DSTDdlPrmtr.Tables[0]; GrdPN.DataBind(); }
                else
                {
                    DSTDdlPrmtr.Tables[0].Rows.Add(DSTDdlPrmtr.Tables[0].NewRow());
                    GrdPN.DataSource = DSTDdlPrmtr.Tables[0];
                    GrdPN.DataBind();
                    GrdPN.Rows[0].Cells.Clear();
                    GrdPN.Rows[0].Cells.Add(new TableCell());
                    DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdPN.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdPN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "BindDataPN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void ValidarPN(string PN, string PNAnt, string Estado, int Bloqueo)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            Session["VlRefer"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens24Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe seleccionar una referencia')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (Estado == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens25Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe ingresar un estado')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (PN == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens26Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Debe ingresar UN PN')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (Bloqueo == 1 && Estado.Equals("01"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens27Ref'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); } //Un P/N bloqueado no puede ser principal')", true);
                Session["VlRefer"] = "N";
                return;
            }
        }
        protected void DdlUMCom_TextChanged(object sender, EventArgs e)
        {
            string VblUMCD;

            DropDownList DdlUMCom = GrdPN.SelectedRow.FindControl("DdlUMCom") as DropDownList;
            VblUMCD = DdlUMCom.SelectedValue;

            TextBox TxtEqu = GrdPN.SelectedRow.FindControl("TxtEqu") as TextBox;
            Cnx.SelecBD();
            using (SqlConnection sqlConx = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = string.Format("SELECT VlorEquivalencia FROM TblUndXPn WHERE Pn=@PN AND UndCompraPN='{0}' AND IdConfigCIa = @ICC", VblUMCD);
                SqlCommand SC = new SqlCommand(LtxtSql, sqlConx);
                SC.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                sqlConx.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                if (tbl.Read())
                { TxtEqu.Text = tbl["VlorEquivalencia"].ToString(); }
                BindDataMan(TxtCod.Text);
                BindDataCont(ViewState["VbPNSI"].ToString());
            }
        }
        protected void GrdPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblPN, VblEstado, VblUMC, VblTxtEqui;
                    float VblEqui;

                    VblTxtEqui = (GrdPN.FooterRow.FindControl("TxtEquPP") as TextBox).Text.Trim();
                    while (VblTxtEqui.Contains(".")) { VblTxtEqui = VblTxtEqui.Replace(".", ","); }
                    VblPN = (GrdPN.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim();
                    VblEstado = (GrdPN.FooterRow.FindControl("DdlEstPNPP") as DropDownList).SelectedValue.Trim();
                    VblUMC = (GrdPN.FooterRow.FindControl("DdlUMComPP") as DropDownList).SelectedValue.Trim();
                    if ((GrdPN.FooterRow.FindControl("TxtEquPP") as TextBox).Text.Trim().Length == 0)
                    { VblEqui = 1; }
                    else
                    { VblEqui = (float)Convert.ToDouble(VblTxtEqui); }
                    ValidarPN(VblPN, "", VblEstado, 0);
                    if (Session["VlRefer"].Equals("N"))
                    { BindDataAll(TxtCod.Text, VblPN); return; }
                    List<CsTTPN> ObjTTPN = new List<CsTTPN>();
                    var detail = new CsTTPN()
                    {
                        PN = VblPN.Trim(),
                        Descripcion = TxtDesc.Text.Trim(),
                        DescripcionEsp = TxtDescEsp.Text.Trim(),
                        CodReferencia = TxtCod.Text.Trim(),
                        CodFabricante = (GrdPN.FooterRow.FindControl("DdlFabPP") as DropDownList).SelectedValue.Trim(),
                        CodUndMed = DdlUM.SelectedValue,
                        CodEstadoPn = VblEstado,
                        CodClaseElem = "01",
                        CodTipoElem = DdlGrupo.SelectedValue,
                        IdentificadorElem = DdlIdent.SelectedValue,
                        CodKit = "0",
                        SubComponente = CkbSub.Checked == true ? 1 : 0,
                        Consumo = CkbCons.Checked == true ? 1 : 0,
                        Motor = CkbMot.Checked == true ? 1 : 0,
                        ComponenteMayor = CkbMay.Checked == true ? 1 : 0,
                        Codcapitulo = DdlAta.SelectedValue,
                        Usu = Session["C77U"].ToString(),
                        PosicionPn = CkbPos.Checked == true ? 1 : 0,
                        UndCompra = VblUMC,
                        Equivalencia = VblEqui,
                        NSN = (GrdPN.FooterRow.FindControl("TxtNSNPP") as TextBox).Text.Trim(),
                        FechaVencPN = (GrdPN.FooterRow.FindControl("CkbFVPP") as CheckBox).Checked == true ? 1 : 0,
                    };
                    ObjTTPN.Add(detail);
                    CsTTPN TblPN = new CsTTPN();
                    TblPN.Accion("INSERT");
                    TblPN.Insert(ObjTTPN);
                    string Mensj = TblPN.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }

                    string VbPlano = TblPN.GetMensj();
                    if (!VbPlano.Trim().Equals("S"))
                    {
                        Cnx.SelecBD();
                        using (SqlConnection SCnxPln = new SqlConnection(Cnx.GetConex()))
                        {
                            SCnxPln.Open();
                            string VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 15,'{0}','{1}','{2}','','',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'",
                             VblPN.Trim(), Session["C77U"].ToString(), TxtCod.Text.Trim());
                            using (SqlCommand sqlCmd = new SqlCommand(VBQuery, SCnxPln))
                            {
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                try
                                { sqlCmd.ExecuteNonQuery(); }
                                catch (Exception ex)
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); } //Error en el proceso de eliminación')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Nuevo P/N", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                }
                            }
                        }
                    }
                    BindDataAll(TxtCod.Text, VblPN);
                    BindDataPN(TxtCod.Text, "UPD");
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT PN", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["VbPNSI"] = GrdPN.DataKeys[this.GrdPN.SelectedIndex][0].ToString();
            BindDataCont(ViewState["VbPNSI"].ToString());
            BindDataMan(TxtCod.Text);
            if (!ViewState["VbPNSI"].ToString().Equals(""))
            { BtnUndCompra.Enabled = true; BtnCambioRef.Enabled = true; }
            else { BindDataPN(TxtCod.Text, "SEL"); }

            foreach (GridViewRow Row in GrdPN.Rows)
            {
                if (Row.RowIndex == GrdPN.SelectedIndex)
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
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + Row.RowIndex);
                }
            }
        }
        protected void GrdPN_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdPN.EditIndex = e.NewEditIndex; BindDataPN(TxtCod.Text, "SEL"); }
        protected void GrdPN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                string VblPN, VblPNAnt, VblEstado, VblUMC;
                float VblEqui;
                int VblBloqueo;
                VblPNAnt = GrdPN.DataKeys[e.RowIndex].Value.ToString();
                VblPN = (GrdPN.Rows[e.RowIndex].FindControl("TxtPN") as TextBox).Text.Trim();
                VblEstado = (GrdPN.Rows[e.RowIndex].FindControl("DdlEstPN") as DropDownList).SelectedValue.Trim();
                VblBloqueo = (GrdPN.Rows[e.RowIndex].FindControl("CkbBloq") as CheckBox).Checked == true ? 1 : 0;
                VblUMC = (GrdPN.Rows[e.RowIndex].FindControl("DdlUMCom") as DropDownList).SelectedValue.Trim();
                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtEqu") as TextBox).Text.Trim().Length == 0)
                { VblEqui = 1; }
                else
                { VblEqui = (float)Convert.ToDouble((GrdPN.Rows[e.RowIndex].FindControl("TxtEqu") as TextBox).Text.Trim()); }
                ValidarPN(VblPN, VblPNAnt, VblEstado, VblBloqueo);
                if (Session["VlRefer"].Equals("N"))
                { BindDataAll(TxtCod.Text, VblPN); return; }
                List<CsTTPN> ObjTTPN = new List<CsTTPN>();
                var detail = new CsTTPN()
                {
                    PN = VblPN.Trim(),
                    Descripcion = TxtDesc.Text.Trim(),
                    DescripcionEsp = TxtDescEsp.Text.Trim(),
                    CodReferencia = TxtCod.Text,
                    CodFabricante = (GrdPN.Rows[e.RowIndex].FindControl("DdlFab") as DropDownList).SelectedValue.Trim(),
                    CodUndMed = DdlUM.SelectedValue,
                    CodEstadoPn = VblEstado,
                    Bloquear = (GrdPN.Rows[e.RowIndex].FindControl("CkbBloq") as CheckBox).Checked == true ? 1 : 0,
                    CodClaseElem = "01",
                    CodTipoElem = DdlGrupo.SelectedValue,
                    IdentificadorElem = DdlIdent.SelectedValue,
                    CodKit = "0",
                    SubComponente = CkbSub.Checked == true ? 1 : 0,
                    Consumo = CkbCons.Checked == true ? 1 : 0,
                    Motor = CkbMot.Checked == true ? 1 : 0,
                    ComponenteMayor = CkbMay.Checked == true ? 1 : 0,
                    Codcapitulo = DdlAta.SelectedValue,
                    Usu = Session["C77U"].ToString(),
                    PosicionPn = CkbPos.Checked == true ? 1 : 0,
                    UndCompra = VblUMC,
                    Equivalencia = VblEqui,
                    NSN = (GrdPN.Rows[e.RowIndex].FindControl("TxtNSN") as TextBox).Text.Trim(),
                    FechaVencPN = (GrdPN.Rows[e.RowIndex].FindControl("CkbFV") as CheckBox).Checked == true ? 1 : 0,
                };
                ObjTTPN.Add(detail);
                CsTTPN TblPN = new CsTTPN();
                TblPN.Accion("UPDATE");
                TblPN.Insert(ObjTTPN);
                GrdPN.EditIndex = -1;
                BindDataPN(TxtCod.Text, "UPD");
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPosicion", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdPN.EditIndex = -1; BindDataPN(TxtCod.Text, "SEL"); }
        protected void GrdPN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string VBQuery, VbCod, VbCodExt, VblUMCD, Mensj = ""; ;
                float VblEqui;
                int VblDCMy;
                VbCod = GrdPN.DataKeys[e.RowIndex].Value.ToString();
                VbCodExt = GrdPN.DataKeys[e.RowIndex].Values["CodigoExternoPN"].ToString();
                VblEqui = (float)Convert.ToDouble(GrdPN.DataKeys[e.RowIndex].Values["Equivalencia"].ToString());
                VblUMCD = GrdPN.DataKeys[e.RowIndex].Values["UndCompra"].ToString();
                VblDCMy = CkbMay.Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 1,'',@PN,'{0}','','','','','','VALIDA',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'", TxtCod.Text);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@PN", VbCod);
                    Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string borr = registro["Mensaje"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + registro["Mensaje"].ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        PerfilesGrid();
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        try
                        {
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 1,'{7}',@PN,'{1}','{3}','{4}','{6}','{8}','{9}','DELETE',@equi,{5},0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'",
                            VbCod, TxtCod.Text, VblEqui, DdlUM.SelectedValue, VblUMCD, VblDCMy, TxtDesc.Text, VbCodExt, DdlAta.SelectedValue, Session["C77U"].ToString());
                            SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac);
                            sqlCmd.Parameters.AddWithValue("@equi", VblEqui);
                            sqlCmd.Parameters.AddWithValue("@PN", VbCod);
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            ViewState["VbPNSI"] = "";
                            BindDataPN(TxtCod.Text, "UPD");
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE PN", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE PN", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DataRow[] Result;
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlEstPNPP = (e.Row.FindControl("DdlEstPNPP") as DropDownList);
                    DdlEstPNPP.DataSource = DSTDdl.Tables[7];
                    DdlEstPNPP.DataTextField = "Descripcion";
                    DdlEstPNPP.DataValueField = "CodEstadoPn";
                    DdlEstPNPP.DataBind();

                    DSTDdlPrmtr = (DataSet)ViewState["DSTDdlPrmtr"];
                    string VbCodAnt = "";
                    DataTable DTUM = new DataTable();
                    DropDownList DdlUMComPP = (e.Row.FindControl("DdlUMComPP") as DropDownList);

                    VbCodAnt = DdlUM.Text.Trim();
                    DTUM = DSTDdlPrmtr.Tables[1].Clone();
                    DTUM.Rows.Add(DdlUM.Text.Trim(), DdlUM.Text.Trim(), 1);

                    Result = DSTDdlPrmtr.Tables[1].Select("ActivoUM=1");
                    foreach (DataRow Row in Result) { DTUM.ImportRow(Row); }

                    DdlUMComPP.DataSource = DTUM;
                    DdlUMComPP.DataTextField = "Descripcion";
                    DdlUMComPP.DataValueField = "CodUnidMedida";
                    DdlUMComPP.DataBind();
                    DdlUMComPP.Text = VbCodAnt;

                    TextBox TxtEquPP = (e.Row.FindControl("TxtEquPP") as TextBox);

                    if (DdlIdent.SelectedValue.Equals("SN"))
                    {
                        DdlUMComPP.Enabled = false;
                        TxtEquPP.Enabled = false;
                        DdlUMComPP.Text = "EA";
                        TxtEquPP.Text = "1";
                    }

                    DataTable DTFb = new DataTable();
                    DropDownList DdlFabPP = (e.Row.FindControl("DdlFabPP") as DropDownList);

                    DTFb = DSTDdl.Tables[8].Clone();
                    Result = DSTDdl.Tables[8].Select("Activo=1");
                    foreach (DataRow Row in Result)
                    { DTFb.ImportRow(Row); }

                    DdlFabPP.DataSource = DTFb;
                    DdlFabPP.DataTextField = "Nombre";
                    DdlFabPP.DataValueField = "CodFabricante";
                    DdlFabPP.DataBind();
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox TxtEqu = (e.Row.FindControl("TxtEqu") as TextBox);
                    string LtxtSql = "";
                    DropDownList DdlEstPN = (e.Row.FindControl("DdlEstPN") as DropDownList);
                    DdlEstPN.DataSource = DSTDdl.Tables[7];
                    DdlEstPN.DataTextField = "Descripcion";
                    DdlEstPN.DataValueField = "CodEstadoPn";
                    DdlEstPN.DataBind();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DdlEstPN.SelectedValue = dr["CodEstadoPn"].ToString();


                    DropDownList DdlFab = (e.Row.FindControl("DdlFab") as DropDownList);
                    DataTable DTFb = new DataTable();
                    DTFb = DSTDdl.Tables[8].Clone();
                    DTFb.Rows.Add(dr["Fabricante"].ToString().Trim(), dr["CodFabricante"].ToString().Trim(), 1, "");

                    Result = DSTDdl.Tables[8].Select("Activo=1");
                    foreach (DataRow Row in Result)
                    { DTFb.ImportRow(Row); }

                    DdlFab.DataSource = DTFb;
                    DdlFab.DataTextField = "Nombre";
                    DdlFab.DataValueField = "CodFabricante";
                    DdlFab.DataBind();
                    DdlFab.SelectedValue = dr["CodFabricante"].ToString();

                    TextBox TxtPN = (e.Row.FindControl("TxtPN") as TextBox);
                    ViewState["VbPNSI"] = TxtPN.Text;

                    LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','','',@PN,'UMCMOD',0,0,0, @ICC,'01-01-01','02-01-01','03-01-01'");
                    DropDownList DdlUMCom = (e.Row.FindControl("DdlUMCom") as DropDownList);
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlCommand cmd = new SqlCommand(LtxtSql, sqlCon))
                        {
                            cmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                            cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            DdlUMCom.DataSource = cmd.ExecuteReader();
                            DdlUMCom.DataTextField = "UndCompraPN";
                            DdlUMCom.DataValueField = "UndCompraPN";
                            DdlUMCom.DataBind();
                            DdlUMCom.SelectedValue = dr["UndCompra"].ToString();
                        }
                    }
                    if (DdlIdent.SelectedValue.Equals("SN"))
                    {
                        DdlUMCom.Enabled = false;
                        TxtEqu.Enabled = false;
                        DdlUMCom.Text = "EA";
                        TxtEqu.Text = "1";
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtUpdate'");
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
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    imgD.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + e.Row.RowIndex);
            }
        }
        protected void GrdPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdPN.PageIndex = e.NewPageIndex; BindDataPN(TxtCod.Text, "SEL"); }
        // *********************************** Contador ***********************************
        protected void BindDataCont(string PN)
        {
            try
            {
                BtnUndCompra.Enabled = false;
                BtnCambioRef.Enabled = false;
                DSTDdlPrmtr = (DataSet)ViewState["DSTDdlPrmtr"];
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataTable DtCont = new DataTable();
                DataRow[] Result;

                DtCont = DSTDdlPrmtr.Tables[2].Clone();
                Result = DSTDdlPrmtr.Tables[2].Select("PN='" + PN.Trim() + "'");
                foreach (DataRow Row in Result) { DtCont.ImportRow(Row); }

                if (DtCont.Rows.Count > 0)
                { GrdCont.DataSource = DtCont; GrdCont.DataBind(); }
                else
                {
                    DtCont.Rows.Add(DtCont.NewRow());
                    GrdCont.DataSource = DtCont;
                    GrdCont.DataBind();
                    GrdCont.Rows[0].Cells.Clear();
                    GrdCont.Rows[0].Cells.Add(new TableCell());
                    Result = Idioma.Select("Objeto= 'SinRegistros'");
                    foreach (DataRow row in Result)
                    { GrdCont.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                    GrdCont.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (!DdlIdent.Text.Equals("SN"))
                {
                    ImageButton Imge = GrdCont.FooterRow.FindControl("IbtAddNew") as ImageButton;
                    if (Imge != null) { Imge.Enabled = false; }
                }
            }
            catch (Exception Ex)
            {
                string Mensje = Ex.Message;
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensje + "')", true);
            }
        }
        protected void GrdCont_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (TxtCod.Text.Equals(""))
                { BindDataAll(TxtCod.Text, ""); return; }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VbContad;
                    VbContad = (GrdCont.FooterRow.FindControl("DdlContPP") as DropDownList).Text.Trim();
                    if (VbContad == String.Empty)
                    {
                        BindDataAll(TxtCod.Text, "");
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens30Ref'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//  Debe seleccionar un contador')", true);
                        return;
                    }
                    if (ViewState["VbPNSI"].ToString() == string.Empty)
                    {
                        BindDataAll(TxtCod.Text, "");
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens31Ref'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }// Debe seleccionar un P/N')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            string VbQuery = "EXEC SP_PANTALLA_ReferenciaV2 9, @CCtd, @P, @Us,'','INSERT',0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                            using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    sqlCmd.Parameters.AddWithValue("@CCtd", VbContad.Trim());
                                    sqlCmd.Parameters.AddWithValue("@P", ViewState["VbPNSI"].ToString().Trim());
                                    sqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                    sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    sqlCmd.ExecuteNonQuery();
                                    Transac.Commit();
                                    BindDataPN(TxtCod.Text, "UPD");
                                    BindDataCont(ViewState["VbPNSI"].ToString());
                                }
                                catch (Exception)
                                { Transac.Rollback(); }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT CONTADOR", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdCont_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            string VblCodCtd = GrdCont.DataKeys[e.RowIndex].Values["CodContador"].ToString();
            string Mensj = "";
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format(" EXEC SP_PANTALLA_ReferenciaV2 9,'{0}','{1}','','','VALIDA',{2},0,0,{3},'01-01-01','02-01-01','03-01-01'",
                     VblCodCtd, ViewState["VbPNSI"].ToString(), GrdCont.DataKeys[e.RowIndex].Value.ToString(), Session["!dC!@"]);
                SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                SqlDataReader tbl = SqlC.ExecuteReader();
                if (tbl.Read())
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + tbl["Mensj"].ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VbQuery = "EXEC SP_PANTALLA_ReferenciaV2 9,'','','','','DELETE',@id,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    using (SqlCommand sqlCmd = new SqlCommand(VbQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@id", GrdCont.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataPN(TxtCod.Text, "UPD");
                            BindDataCont(ViewState["VbPNSI"].ToString());

                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }// Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdCont_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','','','{0}','CTD',0,0,0,{1},'01-01-01','02-01-01','03-01-01'", ViewState["VbPNSI"].ToString(), Session["!dC!@"]);
                    DropDownList DdlContPP = (e.Row.FindControl("DdlContPP") as DropDownList);
                    DdlContPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlContPP.DataTextField = "CodContador";
                    DdlContPP.DataValueField = "Cod";
                    DdlContPP.DataBind();
                    ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                    if (IbtAddNew != null)
                    {
                        IbtAddNew.Enabled = true;
                        DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                        foreach (DataRow row in Result)
                        { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    imgD.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdCont_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdCont.PageIndex = e.NewPageIndex; BindDataCont(ViewState["VbPNSI"].ToString()); }
        // ************************* Asignar nNidad de Compra *************************
        protected void BindDataCambUMC(string PN, string UndMed, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Referencia 13,@PN, @UM,'','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@PN", PN.Trim());
                        SC.Parameters.AddWithValue("@UM", UndMed.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTUndCompra = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTUndCompra);
                                DSTUndCompra.Tables[0].TableName = "Data";
                                DSTUndCompra.Tables[1].TableName = "UndCompra";

                                ViewState["DSTUndCompra"] = DSTUndCompra;
                            }
                        }
                    }
                }
            }
            DSTUndCompra = (DataSet)ViewState["DSTUndCompra"];

            if (DSTUndCompra.Tables[0].Rows.Count > 0)
            { GrdCamUC.DataSource = DSTUndCompra.Tables[0]; GrdCamUC.DataBind(); }
            else
            {
                DSTUndCompra.Tables[0].Rows.Add(DSTUndCompra.Tables[0].NewRow());
                GrdCamUC.DataSource = DSTUndCompra.Tables[0];
                GrdCamUC.DataBind();
                GrdCamUC.Rows[0].Cells.Clear();
                GrdCamUC.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdCamUC.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdCamUC.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BtnUndCompra_Click(object sender, EventArgs e)
        {
            if (DdlIdent.SelectedValue.Equals("PN") || DdlIdent.SelectedValue.Equals("LOTE"))
            {
                BindDataAll("", "");
                PnlCampos.Visible = false;
                PnlUnidadCompra.Visible = true;
                BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "UPD");
                LblCambioPN.Text = "P/N: " + ViewState["VbPNSI"].ToString().Trim();
            }
        }
        protected void IbtCerrarUMC_Click(object sender, ImageClickEventArgs e)
        { PnlCampos.Visible = true; PnlUnidadCompra.Visible = true; }
        protected void GrdCamUC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblPN, VblUMC, VBQuery, VblTxtEqui;
                    float VblEqui;
                    VblPN = ViewState["VbPNSI"].ToString();
                    VblUMC = (GrdCamUC.FooterRow.FindControl("DdlCUMCPP") as DropDownList).SelectedValue.Trim();
                    VblTxtEqui = (GrdCamUC.FooterRow.FindControl("TxtCEquPP") as TextBox).Text.Trim();
                    VblTxtEqui = VblTxtEqui.Replace(".", ",");
                    if ((GrdCamUC.FooterRow.FindControl("TxtCEquPP") as TextBox).Text.Trim().Length == 0)
                    { VblEqui = 1; }
                    else
                    { VblEqui = (float)Convert.ToDouble(VblTxtEqui); }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{1}','{2}','{3}','INSERT',@Equi,0,0, @ICC,'01-01-01','02-01-01','03-01-01'",
                            "", VblUMC, DdlUM.SelectedValue, Session["C77U"].ToString());
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                                    SC.Parameters.AddWithValue("@Equi", VblEqui);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.ExecuteNonQuery();
                                    Transac.Commit();
                                    //BindDataCambUMC(VblPN);
                                    BindDataCambUMC(VblPN.Trim(), DdlUM.Text.Trim(), "UPD");
                                }
                                catch (Exception ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT PN", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdCamUC_SelectedIndexChanged(object sender, EventArgs e)
        { }
        protected void GrdCamUC_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdCamUC.EditIndex = e.NewEditIndex; BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "UPD"); }
        protected void GrdCamUC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery, VbUCMod, VblTxtEqui;
            float VbequiCUC;

            VblTxtEqui = (GrdCamUC.Rows[e.RowIndex].FindControl("TxtCEqu") as TextBox).Text.Trim();
            VblTxtEqui = VblTxtEqui.Replace(".", ",");

            VbequiCUC = (float)Convert.ToDouble(VblTxtEqui);
            VbUCMod = (GrdCamUC.Rows[e.RowIndex].FindControl("TxtCUMC") as TextBox).Text.Trim();
            if (VbUCMod.Equals(DdlUM.SelectedValue)) { VbequiCUC = 1; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','','VALIDA',@Equi, 0,0,@ICC,'01-01-01','02-01-01','03-01-01'", VbUCMod);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlCd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                SqlCd.Parameters.AddWithValue("@Equi", VbequiCUC);
                SqlCd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader DRCUM = SqlCd.ExecuteReader();
                if (DRCUM.Read())
                {
                    string Mensj = DRCUM["Mensj"].ToString();
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString(); }
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 4,@PN, @UNC,'', @Us,'UPDATE',@Equi, @I,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString().Trim());
                            sqlCmd.Parameters.AddWithValue("@UNC", VbUCMod.Trim());
                            sqlCmd.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Equi", VbequiCUC);
                            sqlCmd.Parameters.AddWithValue("@I", GrdCamUC.DataKeys[e.RowIndex].Value.ToString());
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdCamUC.EditIndex = -1;
                            BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en la actualización')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "UPDATE Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                        }
                    }
                }
            }
        }
        protected void GrdCamUC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdCamUC.EditIndex = -1; BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "SEL"); }
        protected void GrdCamUC_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery, VbUCMod, VblTxtEqui;
            float VbequiCUC;
            foreach (GridViewRow row in GrdCamUC.Rows)
            {
                if (Convert.ToInt32(GrdCamUC.DataKeys[e.RowIndex].Value.ToString()) == Convert.ToInt32(GrdCamUC.DataKeys[row.RowIndex].Value.ToString()))
                {
                    LblCEquP = ((Label)row.FindControl("LblCEquP")).Text;
                    LblCUMCP = ((Label)row.FindControl("LblCUMCP")).Text;
                }
            }
            VbUCMod = LblCUMCP;
            VblTxtEqui = LblCEquP.Replace(".", ",");

            VbequiCUC = (float)Convert.ToDouble(VblTxtEqui);
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','','VALIDA',@Equi, 0,0,@ICC,'01-01-01','02-01-01','03-01-01'", VbUCMod);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlCd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                SqlCd.Parameters.AddWithValue("@Equi", VbequiCUC);
                SqlCd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader DRCUM = SqlCd.ExecuteReader();
                if (DRCUM.Read())
                {
                    string Mensj = DRCUM["Mensj"].ToString();
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','{2}','DELETE',@Equi,{1},0,@ICC,'01-01-01','02-01-01','03-01-01'",
                     VbUCMod, GrdCamUC.DataKeys[e.RowIndex].Value.ToString(), Session["C77U"].ToString());
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Equi", VbequiCUC);
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdCamUC.EditIndex = -1;
                            BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "UPD");/* */
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en la eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                        }
                    }
                }
            }
        }
        protected void GrdCamUC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DSTUndCompra = (DataSet)ViewState["DSTUndCompra"];
                DropDownList DdlCUMCPP = (e.Row.FindControl("DdlCUMCPP") as DropDownList);
                DataTable DT = new DataTable();
                DT = DSTUndCompra.Tables[1].Clone();
                Result = DSTUndCompra.Tables[1].Select("Disponible = 1");
                foreach (DataRow DR in Result)
                { DT.ImportRow(DR); }

                DdlCUMCPP.DataSource = DT;
                DdlCUMCPP.DataTextField = "Descripcion";
                DdlCUMCPP.DataValueField = "CodUnidMedida";
                DdlCUMCPP.DataBind();

                TextBox TxtCUDPP = (e.Row.FindControl("TxtCUDPP") as TextBox);
                TxtCUDPP.Text = DdlUM.SelectedValue;
                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtUpdate'");
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
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    imgD.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdCamUC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdCamUC.PageIndex = e.NewPageIndex; BindDataCambUMC(ViewState["VbPNSI"].ToString().Trim(), DdlUM.Text.Trim(), "SEL"); }
        // ************************* Cambio de Ref *************************
        protected void BindDataCambioRef()
        {
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurCambioRef", Session["77IDM"].ToString().Trim());
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql;
                VbTxtSql = "";
                if (RdbRefCRef.Checked == true)
                {
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 17,'{0}','{1}',{2},'RF','CurCambioRef',0,0,0,{3},'01-01-01','02-01-01','03-01-01'", DdlGrupo.SelectedValue, TxtCod.Text, TxtCambRef.Text, Session["!dC!@"]);
                }
                if (RdbPnCRef.Checked == true)
                {
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 17,'{0}','{1}',@PN,'PN','CurCambioRef',0,0,0,{2},'01-01-01','02-01-01','03-01-01'", DdlGrupo.SelectedValue, TxtCod.Text, Session["!dC!@"]);
                }

                if (!VbTxtSql.Equals(""))
                {
                    sqlConB.Open();
                    SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB);
                    SC.Parameters.AddWithValue("@PN", TxtCambRef.Text);
                    SqlDataAdapter DAB = new SqlDataAdapter(SC);
                    DAB.Fill(DtB);

                    if (DtB.Rows.Count > 0)
                    { GrdCambioRef.DataSource = DtB; GrdCambioRef.DataBind(); }
                    else
                    { GrdCambioRef.DataSource = null; GrdCambioRef.DataBind(); }
                }
                else { GrdCambioRef.DataSource = null; GrdCambioRef.DataBind(); }
            }
        }
        protected void BtnCambioRef_Click(object sender, EventArgs e)
        {
            if (ViewState["VbPNSI"].ToString() != string.Empty)
            {
                BindDataCambioRef();
                LblPNCRef.Text = "P/N: " + ViewState["VbPNSI"].ToString();
                ViewState["NewRef"] = "";
                LblRefCambRef.Text = "";
                RdbRefCRef.Checked = true;
                PnlCampos.Visible = false;
                PnlCambioRef.Visible = true;
            }
        }
        protected void IbtApliarCambRef_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VBQuery = "", Mensj = "";
            if (ViewState["NewRef"].Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una referencia')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 12,'{0}','{1}','','{2}','VALIDA',0,0,0,'{3}','01-01-01','02-01-01','03-01-01'", TxtCod.Text, ViewState["NewRef"], DdlIdent.SelectedValue, Session["!dC!@"]);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlDataReader SDRCR = SqlCd.ExecuteReader();
                if (SDRCR.Read())
                {
                    DataRow[] Result = Idioma.Select("Objeto= '" + SDRCR["Mensj"].ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }

                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 2,@PNAct,'{0}','{1}','','','','','','',0,0,0,0,0,'{2}','01-01-1','02-01-1','03-01-1'",
                        ViewState["NewRef"].ToString(), Session["C77U"].ToString(), Session["!dC!@"]);
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@PNAct", ViewState["VbPNSI"].ToString());
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            ViewState["VbPNSI"] = "";
                            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                            LblRefCambRef.Text = "";
                            ViewState["NewRef"] = "";
                            PnlCambioRef.Visible = false;
                            PnlCampos.Visible = true;
                            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());

                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "Cambio referencia", ex.StackTrace.Substring(ex.StackTrace.Length > 300 ? ex.StackTrace.Length - 300 : 0, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void IbtConsultarCambRef_Click(object sender, ImageClickEventArgs e)
        {
            LblRefCambRef.Text = "";
            ViewState["NewRef"] = "";
            BindDataCambioRef();
        }
        protected void IbtCerrarCambRef_Click(object sender, ImageClickEventArgs e)
        {
            PnlCambioRef.Visible = false;
            PnlCampos.Visible = true;
            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
            LblRefCambRef.Text = "";
            ViewState["NewRef"] = "";
        }
        protected void GrdCambioRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbText = "";
            DataRow[] Result = Idioma.Select("Objeto= 'RdbBusqR'");
            foreach (DataRow row in Result)
            { VbText = row["Texto"].ToString().Trim(); }

            if (RdbRefCRef.Checked == true)
            {
                LblRefCambRef.Text = VbText + ": " + HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
                ViewState["NewRef"] = HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
            }
            if (RdbPnCRef.Checked == true)
            {
                LblRefCambRef.Text = VbText + ": " + HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[2].Text);
                ViewState["NewRef"] = HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
            }
        }
        protected void GrdCambioRef_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdCambioRef.PageIndex = e.NewPageIndex; BindDataCambioRef(); }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetFixedHeightForGridIfRowsAreLess(GrdPN);
            SetFixedHeightForGridIfRowsAreLess(GrdMan);
            SetFixedHeightForGridIfRowsAreLess(GrdDatos);
            SetFixedHeightForGridIfRowsAreLess(GrdCont);
        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 30; //we set header height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                height += 30;
                gv.Height = new Unit(height);
            }
        }
    }
}