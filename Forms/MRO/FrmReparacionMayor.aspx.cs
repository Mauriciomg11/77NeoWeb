using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmReparacionMayor : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPpal = new DataSet();
        DataSet DSDdl = new DataSet();
        DataTable DTAK = new DataTable();
        DataTable DTAdj = new DataTable();
        DataSet DSPNSN = new DataSet();
        private byte[] imagen;
        private string Vbl3Desc, Vbl4Ruta, VBQuery, Vbl6Ext, Vbl8Type;
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
                ViewState["VldrCntdr"] = "S";
                TitForm.Text = "";

                ModSeguridad();
                ViewState["UCD"] = 0;
                ViewState["TIPO"] = "A";
                ViewState["IdCodElem"] = -1;
                ViewState["PN"] = "";
                ViewState["SN"] = "";
                ViewState["CodElem"] = "";
                BtnAK.CssClass = "btn btn-primary";
                BindDAK("UPDATE");
                BindDPN("UPDATE");
                BindDDdl("UPDATE", "SELECT");
                GrdAeron.Visible = true;
                ViewState["TipoAccion"] = "";
                ViewState["CodSvcAnt"] = "";
                ViewState["TieneRegDet"] = "0";
                RdbBusqDes.Checked = true;

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
            ViewState["CE1"] = 1;
            ViewState["CE3"] = 1;
            ViewState["CE4"] = 1;
            ViewState["CE5"] = 1;
            ViewState["CE6"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);

            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false;
                GrdAeron.ShowFooter = false;
                GrdPN.ShowFooter = false;
                GrdAdj.ShowFooter = false;
                GrdRecursoF.ShowFooter = false;
                GrdLicen.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; BtnGenerarOT.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { BtnImprimir.Visible = false; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["CE1"] = 0; }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["CE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["CE4"] = 0; }
            if (ClsP.GetCE5() == 0)
            { ViewState["CE5"] = 0; BtnRecurso.Visible = false; }
            if (ClsP.GetCE6() == 0)
            {
                ViewState["CE6"] = 0;
                CkbBloqRec.Visible = false;
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
                SC.Parameters.AddWithValue("@F1", "FrmServicioManto");
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

                    if (bO.Equals("CaptionRepa"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    TitForm.Text = bO.Equals("LblTituloSMtoRP") ? bT : TitForm.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsultar.Text;
                    BtnImprimir.Text = bO.Equals("BtnImprimirGrl") ? bT : BtnImprimir.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    BtnRecurso.Text = bO.Equals("BtnRecurso") ? bT : BtnRecurso.Text;
                    BtnRecurso.ToolTip = bO.Equals("IbtRecurso") ? bT : BtnRecurso.ToolTip;
                    BtnGenerarOT.Text = bO.Equals("BtnGenerarOT") ? bT : BtnGenerarOT.Text;
                    BtnGenerarOT.ToolTip = bO.Equals("IbtGenerOT") ? bT : BtnGenerarOT.ToolTip;

                    LblCod.Text = bO.Equals("LblCod") ? bT + ":" : LblCod.Text;
                    if (bO.Equals("LblDescrip"))
                    {
                        LblDescrip.Text = bT;
                        GrdPN.Columns[1].HeaderText = bT;
                        GrdAdj.Columns[0].HeaderText = bT;
                        RdbBusqDes.Text = "&nbsp " + bT;
                        RdbBusqDesPN.Text = "&nbsp " + bT;
                        RdbBusqDesSN.Text = "&nbsp " + bT;
                        GrdRecursoF.Columns[2].HeaderText = bT;
                        GrdLicen.Columns[1].HeaderText = bT;
                        ViewState["DesInf"] = bT;
                    }
                    LblGrupo.Text = bO.Equals("LblGrupo") ? bT + ":" : LblGrupo.Text;
                    LblModel.Text = bO.Equals("GrdMod") ? bT + ":" : LblModel.Text;
                    LblDoc.Text = bO.Equals("LblDoc") ? bT + ":" : LblDoc.Text;
                    LblTaller.Text = bO.Equals("LblTaller") ? bT + ":" : LblTaller.Text;
                    LblAta.Text = bO.Equals("LblAta") ? bT + ":" : LblAta.Text;

                    if (bO.Equals("LblTipo")) { GrdRecursoF.Columns[7].HeaderText = bT; ViewState["TypInf"] = bT; }/**/
                    if (bO.Equals("placeholder02"))
                    { TxtHistorico.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("placeholder03"))
                    { TxtEstadoOT.Attributes.Add("placeholder", bT); }
                    if (bO.Equals("BtnAK"))
                    {
                        TxtMatric.Attributes.Add("placeholder", bT);
                        BtnAK.Text = bT;
                    }
                    CkbBloqRec.Text = bO.Equals("CkbBloqRec") ? bT + ":" : CkbBloqRec.Text;
                    CkbBloqRec.ToolTip = bO.Equals("CkbBloqRecTT") ? bT : CkbBloqRec.ToolTip;
                    if (bO.Equals("GrdMatr")) { GrdAeron.Columns[0].HeaderText = bT; ViewState["AkInf"] = bT; }
                    if (bO.Equals("GrdCont")) { GrdAeron.Columns[1].HeaderText = bT; GrdPN.Columns[2].HeaderText = bT; ViewState["ContInf"] = bT; }
                    if (bO.Equals("GrdFrec")) { GrdAeron.Columns[2].HeaderText = bT; GrdPN.Columns[3].HeaderText = bT; GrdSN.Columns[3].HeaderText = bT; }
                    GrdSN.Columns[2].HeaderText = bO.Equals("GrdCont2") ? bT : GrdSN.Columns[2].HeaderText;

                    GrdAdj.Columns[1].HeaderText = bO.Equals("GrdNomArch") ? bT : GrdAdj.Columns[1].HeaderText;
                    // ************************************** Busqueda  *******************************************************       
                    LbltitBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LbltitBusq.Text;
                    LblBusq.Text = bO.Equals("Busqueda") ? bT : LblBusq.Text;
                    if (bO.Equals("placeholder")) { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("BtnConsultarGral") ? bT : IbtConsultar.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[0].HeaderText = bO.Equals("GrdSelect") ? bT : GrdBusq.Columns[0].HeaderText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("GrdId") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("LblCod") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("LblDescrip") ? bT : GrdBusq.Columns[3].HeaderText;
                    GrdBusq.Columns[4].HeaderText = bO.Equals("LblDoc") ? bT : GrdBusq.Columns[4].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("GrdDscPN") ? bT : GrdBusq.Columns[7].HeaderText;
                    // ************************************** Recurso  *******************************************************
                    LblTitRecPartes.Text = bO.Equals("LblTitRecPartes") ? bT : LblTitRecPartes.Text;
                    LblTitRecursoLice.Text = bO.Equals("LblTitRecursoLice") ? bT : LblTitRecursoLice.Text;
                    IbtCloseRecurso.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseRecurso.ToolTip;
                    GrdRecursoF.Columns[0].HeaderText = bO.Equals("GrdPNum") ? bT : GrdRecursoF.Columns[0].HeaderText;
                    GrdRecursoF.Columns[1].HeaderText = bO.Equals("GrdRef") ? bT : GrdRecursoF.Columns[1].HeaderText;
                    GrdRecursoF.Columns[3].HeaderText = bO.Equals("GrdCant") ? bT : GrdRecursoF.Columns[3].HeaderText;
                    GrdRecursoF.Columns[4].HeaderText = bO.Equals("LblEtapa") ? bT : GrdRecursoF.Columns[4].HeaderText;
                    GrdRecursoF.Columns[5].HeaderText = bO.Equals("GrdCondic") ? bT : GrdRecursoF.Columns[5].HeaderText;
                    GrdRecursoF.Columns[6].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdRecursoF.Columns[6].HeaderText;
                    LblTitLicen.Text = bO.Equals("LblTitLicen") ? bT : LblTitLicen.Text;
                    GrdLicen.Columns[0].HeaderText = bO.Equals("GrdLicen") ? bT : GrdLicen.Columns[0].HeaderText;
                    GrdLicen.Columns[2].HeaderText = bO.Equals("GrdTiemEst") ? bT : GrdLicen.Columns[2].HeaderText;
                    // ************************************** Imprimir  *******************************************************  
                    /* IbtCerrarInf.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarInf.ToolTip;
                     BtnSvcAct.Text = bO.Equals("BtnSvcAct") ? bT : BtnSvcAct.Text;
                     BtnCumplim.Text = bO.Equals("BtnCumplim") ? bT : BtnCumplim.Text;
                     IbtExpExcelSvcAplAK.ToolTip = bO.Equals("IbtExpExcelSvcAplAK") ? bT : IbtExpExcelSvcAplAK.ToolTip;
                     IbtExpExcelSvcGnrl.ToolTip = bO.Equals("IbtExpExcelSvcGnrl") ? bT : IbtExpExcelSvcGnrl.ToolTip;
                     ViewState["TitInf"] = bO.Equals("TitInf") ? bT : ViewState["TitInf"];
                     ViewState["DocInf"] = bO.Equals("LblDoc") ? bT : ViewState["DocInf"];
                     ViewState["FrecInf"] = bO.Equals("FrecInf") ? bT : ViewState["FrecInf"];
                     ViewState["fechUCInf"] = bO.Equals("fechUCInf") ? bT : ViewState["fechUCInf"];
                     ViewState["InfOT"] = bO.Equals("InfOT") ? bT : ViewState["InfOT"];
                     ViewState["TitCumpInf"] = bO.Equals("TitCumpInf") ? bT : ViewState["TitCumpInf"];
                     ViewState["DatosEleInf"] = bO.Equals("DatosEleInf") ? bT : ViewState["DatosEleInf"];
                     ViewState["DatosHkInf"] = bO.Equals("DatosHkInf") ? bT : ViewState["DatosHkInf"];
                     ViewState["ServInf"] = bO.Equals("ServInf") ? bT : ViewState["ServInf"];
                     ViewState["GrupInf"] = bO.Equals("LblGrupo") ? bT : ViewState["GrupInf"];
                     ViewState["DiaInf"] = bO.Equals("DiaInf") ? bT : ViewState["DiaInf"];
                     ViewState["OrdenInf"] = bO.Equals("OrdenInf") ? bT : ViewState["OrdenInf"];
                     ViewState["ContInf2"] = bO.Equals("ContInf2") ? bT : ViewState["ContInf2"];
                     ViewState["VlrInf"] = bO.Equals("VlrInf") ? bT : ViewState["VlrInf"];*/
                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbtDeleteOnCl'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            foreach (GridViewRow Row in GrdAdj.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[2].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[2].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdAeron.Rows)
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
            foreach (GridViewRow Row in GrdPN.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[4].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[4].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdSN.Rows)
            {
                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[12].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[12].Controls.Remove(imgD);
                    }
                }
            }
            foreach (GridViewRow Row in GrdRecursoF.Rows)
            {
                ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                if ((int)ViewState["VblModMS"] == 0)
                {
                    if (imgE != null)
                    {
                        Row.Cells[8].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    if (imgD != null)
                    {
                        Row.Cells[8].Controls.Remove(imgD);
                    }
                }
                if (CkbBloqRec.Checked == true)
                {
                    string VblText = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { VblText = row["Texto"].ToString().Trim(); }// El recurso se encuentra bloqueado
                    if (imgE != null)
                    {
                        imgE.Enabled = false; imgE.ToolTip = VblText;
                        imgD.Enabled = false; imgD.ToolTip = VblText;
                    }
                }
            }
            foreach (GridViewRow Row in GrdLicen.Rows)
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
            } /* */
            TxtHistorico.Enabled = false;
            TxtHistorico.Text = "";
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void EstadoOT(int Id)
        {
            DataRow[] Result, Result1;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Result1 = Idioma.Select("Objeto= 'Mens04SM'");
            foreach (DataRow DRI in Result1)
            { TxtEstadoOT.Text = DRI["Texto"].ToString().Trim(); }
            if (ViewState["TIPO"].ToString().Equals("S"))
            {
                DSPNSN = (DataSet)ViewState["DSPNSN"];
                Result = DSPNSN.Tables[1].Select("CodIdContaSrvManto=" + Id);
                foreach (DataRow DR in Result)
                {
                    if (!DR["CodigoOT"].ToString().Trim().Equals(""))
                    {
                        Result1 = Idioma.Select("Objeto= 'Mens01SM'");
                        foreach (DataRow DRI in Result1)
                        { TxtEstadoOT.Text = DRI["Texto"].ToString().Trim() + " " + DR["CodigoOT"].ToString().Trim(); }
                    }
                }
            }
            if (ViewState["TIPO"].ToString().Equals("A"))
            {
                DTAK = (DataTable)ViewState["DTAK"];
                Result = DTAK.Select("CodIdContaSrvManto=" + Id);
                foreach (DataRow DR in Result)
                {
                    if (!DR["CodigoOT"].ToString().Trim().Equals(""))
                    {
                        Result1 = Idioma.Select("Objeto= 'Mens01SM'");
                        foreach (DataRow DRI in Result1)
                        { TxtEstadoOT.Text = DRI["Texto"].ToString().Trim() + " " + DR["CodigoOT"].ToString().Trim(); }
                    }
                }
            }
        }
        protected void BindDDdl(string Accion, string Crud)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','SrvcMyres',0,0, @Idm,@ICC,'01-01-01','01-01-01','01-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSDdl = new DataSet())
                            {

                                SDA.SelectCommand = SC;
                                SDA.Fill(DSDdl);
                                DSDdl.Tables[0].TableName = "PatronManto";
                                DSDdl.Tables[1].TableName = "Modelo";
                                DSDdl.Tables[2].TableName = "Taller";
                                DSDdl.Tables[3].TableName = "ATA";
                                DSDdl.Tables[4].TableName = "DescPN";
                                DSDdl.Tables[5].TableName = "HKGrid";
                                DSDdl.Tables[6].TableName = "HkMod";
                                DSDdl.Tables[7].TableName = "CONMOD";
                                DSDdl.Tables[8].TableName = "PN";
                                DSDdl.Tables[9].TableName = "DescLicenRF";

                                ViewState["DSDdl"] = DSDdl;
                            }
                        }
                    }
                }
            }
            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result;
            string VblCond = "", VbCodAnt = "";

            VbCodAnt = DdlGrupo.Text.Trim();
            DdlGrupo.DataSource = DSDdl.Tables[0];
            DdlGrupo.DataMember = "Datos";
            DdlGrupo.DataTextField = "Descripcion";
            DdlGrupo.DataValueField = "CodPatronManto";
            DdlGrupo.DataBind();
            DdlGrupo.Text = VbCodAnt;

            VbCodAnt = DdlModel.Text.Trim();
            DdlModel.DataSource = DSDdl.Tables[1];
            DdlModel.DataMember = "Datos";
            DdlModel.DataTextField = "NomModelo";
            DdlModel.DataValueField = "CodModelo";
            DdlModel.DataBind();
            DdlModel.Text = VbCodAnt;

            DataTable DTTaller = new DataTable();
            VbCodAnt = Ddltaller.Text.Trim();
            DTTaller = DSDdl.Tables[2].Clone();
            if (Crud.Equals("INSERT"))
            {
                VblCond = "Activo=1";
                Result = DSDdl.Tables[2].Select(VblCond);
                foreach (DataRow Row in Result)
                { DTTaller.ImportRow(Row); Ddltaller.DataSource = DTTaller; }
            }
            if (Crud.Equals("UPDATE"))
            {
                DTTaller.Rows.Add(Ddltaller.SelectedItem.Text.Trim(), Ddltaller.Text.Trim(), 0);
                VblCond = "Activo=1";
                Result = DSDdl.Tables[2].Select(VblCond);
                foreach (DataRow Row in Result)
                { DTTaller.ImportRow(Row); Ddltaller.DataSource = DTTaller; }
            }
            if (Crud.Equals("SELECT")) { Ddltaller.DataSource = DSDdl.Tables[2]; }
            Ddltaller.DataTextField = "NomTaller";
            Ddltaller.DataValueField = "CodTaller";
            Ddltaller.DataBind();
            Ddltaller.Text = VbCodAnt;

            VbCodAnt = DdlAta.Text.Trim();
            DdlAta.DataSource = DSDdl.Tables[3];
            DdlAta.DataMember = "Datos";
            DdlAta.DataTextField = "Descripcion";
            DdlAta.DataValueField = "CodCapitulo";
            DdlAta.DataBind();
            DdlAta.Text = VbCodAnt;
        }
        protected void BindDTraerdatos(string Prmtr, string Accion, string Opcion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 23,'','','','','',@IdSv,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                    using (SqlCommand SC = new SqlCommand(LtxtSql, Cnx2))
                    {
                        SC.Parameters.AddWithValue("@IdSv", Prmtr);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTPpal = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTPpal);
                                DSTPpal.Tables[0].TableName = "Srvc";
                                DSTPpal.Tables[1].TableName = "Recurso";
                                DSTPpal.Tables[2].TableName = "Licencia";

                                ViewState["DSTPpal"] = DSTPpal;
                            }
                        }
                    }
                }
            }
            DSTPpal = (DataSet)ViewState["DSTPpal"];
            if (Opcion.Equals("ALL"))
            {
                foreach (DataRow SDR in DSTPpal.Tables[0].Rows)
                {
                    TxtId.Text = SDR["IdSrvManto"].ToString();
                    TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodServicioManto"].ToString().Trim());
                    TxtDesc.Text = HttpUtility.HtmlDecode(SDR["Servicio"].ToString().Trim());
                    DdlGrupo.SelectedValue = SDR["CodPatronManto"].ToString().Trim();
                    if (DdlGrupo.SelectedValue.Trim().Equals("UCD"))
                    {
                        ViewState["UCD"] = 1;
                    }
                    else
                    {
                        ViewState["UCD"] = 0;
                    }
                    TxtDoc.Text = HttpUtility.HtmlDecode(SDR["Nrodocumento"].ToString().Trim());
                    DdlModel.Text = HttpUtility.HtmlDecode(SDR["CodModeloSM"].ToString().Trim());
                    Ddltaller.Text = HttpUtility.HtmlDecode(SDR["CodTaller"].ToString().Trim());
                    DdlAta.Text = HttpUtility.HtmlDecode(SDR["CodCapitulo"].ToString().Trim());
                    CkbBloqRec.Checked = HttpUtility.HtmlDecode(SDR["ValidarRecurso"].ToString().Trim()) == "S" ? true : false;
                    string borr = HttpUtility.HtmlDecode(SDR["TieneRegDet"].ToString().Trim());
                    ViewState["TieneRegDet"] = HttpUtility.HtmlDecode(SDR["TieneRegDet"].ToString().Trim());

                    switch (ViewState["TIPO"])
                    {
                        case "A":
                            BindDAK("UPDATE");
                            break;
                        default:
                            BindDPN("UPDATE");
                            break;
                    }
                    BindDAdjunto("UPDATE");
                }
            }

            if (DSTPpal.Tables[1].Rows.Count > 0)// Recurso
            { GrdRecursoF.DataSource = DSTPpal.Tables[1]; GrdRecursoF.DataBind(); }
            else
            {
                DSTPpal.Tables[1].Rows.Add(DSTPpal.Tables[1].NewRow());
                GrdRecursoF.DataSource = DSTPpal.Tables[1];
                GrdRecursoF.DataBind();
                GrdRecursoF.Rows[0].Cells.Clear();
                GrdRecursoF.Rows[0].Cells.Add(new TableCell());
                GrdRecursoF.Rows[0].Cells[0].Text = "Empty..!";
                GrdRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

            string VbIdSvc = TxtId.Text.Equals("") ? "0" : TxtId.Text.Trim();
            DataTable DT = DSTPpal.Tables["Licencia"].Clone();
            DataRow[] DR = DSTPpal.Tables[2].Select("IdSrvManto = " + VbIdSvc);
            if (Cnx.ValidaDataRowVacio(DR))
            { DT = DR.CopyToDataTable(); }

            if (DT.Rows.Count > 0) { GrdLicen.DataSource = DT; GrdLicen.DataBind(); }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdLicen.DataSource = DT;
                GrdLicen.DataBind();
                GrdLicen.Rows[0].Cells.Clear();
                GrdLicen.Rows[0].Cells.Add(new TableCell());
                GrdLicen.Rows[0].Cells[0].Text = "Empty..!";
                GrdLicen.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void BindDataAll()
        { PerfilesGrid(); }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            if (!ViewState["TIPO"].ToString().Equals("S")) { BtnIngresar.Enabled = In; }
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            BtnConsultar.Enabled = Otr;
            BtnImprimir.Enabled = Ip;
            BtnRecurso.Enabled = Otr;
            BtnGenerarOT.Enabled = Otr;
            BtnAK.Enabled = Otr;
            BtnPN.Enabled = Otr;
            BtnSN.Enabled = Otr;
            GrdAeron.Enabled = Otr;
            GrdPN.Enabled = Otr;
            GrdSN.Enabled = Otr;
            GrdAdj.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            TxtDesc.Enabled = Edi;
            DdlGrupo.Enabled = Ing;
            TxtDoc.Enabled = Edi;
            if (ViewState["TieneRegDet"].ToString().Equals("0")) { DdlModel.Enabled = Edi; }
            Ddltaller.Enabled = Edi;
            DdlAta.Enabled = Edi;
            CkbBloqRec.Enabled = Edi;
        }
        protected void LimpiarCampos()
        {
            TxtId.Text = "";
            TxtCod.Text = "";
            TxtDesc.Text = "";
            DdlGrupo.Text = "";
            TxtDoc.Text = "";
            DdlAta.Text = "";
            DdlModel.Text = "";
            Ddltaller.Text = "";
            DdlAta.Text = "";
            TxtEstadoOT.Text = "";
            TxtMatric.Text = "";
        }
        protected void ValidarSvcManto(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                if (TxtDesc.Text.Trim().Equals(""))
                {
                    Idioma = (DataTable)ViewState["TablaIdioma"];

                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar una descripción
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlGrupo.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un grupo
                    ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarSvcManto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ValidarHK(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                string VBQuery;
                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["CodHK"].ToString().Trim().Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens07SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una aeronave
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un contador
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (TxtHistorico.Enabled == true && TxtHistorico.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens09SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }/**/
                if (ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una frecuencia
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 7,'{0}','{3}','','','HK',{1},{2},{4}, @ICC,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], ViewState["CodHK"], TxtCod.Text, ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SCE.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        string Mensj = DAE["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarHK", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void ValidarDetPN(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Validar"] = "S";
                string VBQuery;

                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["PN"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens14SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens08SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un contador
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una frecuencia
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 9,'{0}','{2}','{3}','','VALIDA',{1},{4},0, @ICC,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], ViewState["PN"], TxtCod.Text, ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SCE.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        string Mensj = DAE["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarPN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void ValidaDetSN()
        {
            try
            {
                ViewState["Validar"] = "S";
                string VBQuery;
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtHistorico.Enabled == true && ViewState["Historico"].ToString().Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens09SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }/**/
                if (ViewState["FrecIni"].ToString().Trim().Equals("0") && ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens10SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una frecuencia
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 7,'{0}','{2}','{3}','','SN',{1},{4},0,@ICC,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], TxtCod.Text, ViewState["CodElem"], ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SCE.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        string Mensj = DAE["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidaDetSN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void DdlGrupo_TextChanged(object sender, EventArgs e)
        { PerfilesGrid(); ViewState["UCM"] = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void DdlHKPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DropDownList DdlHKPP = (GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','','','','CON',{1},0,{3},{2},'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlHKPP.SelectedValue, Session["!dC!@"], Session["77IDM"]);
            DropDownList DdlContHKPP = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList);
            DdlContHKPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContHKPP.DataTextField = "CodContador";
            DdlContHKPP.DataValueField = "Cod";
            DdlContHKPP.DataBind();
            return;
        }
        protected void DdlPNPP_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            PerfilesGrid();
            DropDownList DdlPNPP = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','CONPN',0,0,{3},{2},'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlPNPP.SelectedValue, Session["!dC!@"], Session["77IDM"]);
            DropDownList DdlContPNPP = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList);
            DdlContPNPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContPNPP.DataTextField = "CodContador";
            DdlContPNPP.DataValueField = "Cod";
            DdlContPNPP.DataBind();

            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result = DSDdl.Tables[4].Select("PN= '" + DdlPNPP.SelectedValue + "'");
            foreach (DataRow Row in Result)
            { (GrdPN.FooterRow.FindControl("TxtDescPnPP") as TextBox).Text = Row["Descripcion"].ToString(); }
        }
        protected void BtnAK_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-primary";
            BtnPN.CssClass = "btn btn-outline-primary";
            BtnSN.CssClass = "btn btn-outline-primary";
            ViewState["TIPO"] = "A";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            GrdAeron.Visible = true;
            GrdPN.Visible = false;
            GrdSN.Visible = false;
            GrdBusq.DataSource = null; GrdBusq.DataBind();
            BtnIngresar.Enabled = true;
            BtnGenerarOT.Enabled = true;
            LimpiarCampos();
            GrdAeron.DataSource = null; GrdAeron.DataBind();
            GrdAdj.DataSource = null; GrdAdj.DataBind();
            RdbBusqDes.Checked = true;
            PerfilesGrid();
        }
        protected void BtnPN_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-outline-primary";
            BtnPN.CssClass = "btn btn-primary";
            BtnSN.CssClass = "btn btn-outline-primary";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            GrdAeron.Visible = false;
            GrdPN.Visible = true;
            GrdSN.Visible = false;
            GrdBusq.DataSource = null;
            GrdBusq.DataBind();
            BtnIngresar.Enabled = true;
            BtnGenerarOT.Enabled = false;
            RdbBusqDesPN.Checked = true;
            if (ViewState["TIPO"].ToString().Equals("A"))
            {
                LimpiarCampos(); GrdPN.DataSource = null; GrdPN.DataBind();
                GrdAdj.DataSource = null; GrdAdj.DataBind();
            }
            ViewState["TIPO"] = "P";
            PerfilesGrid();
        }
        protected void BtnSN_Click(object sender, EventArgs e)
        {
            BtnAK.CssClass = "btn btn-outline-primary";
            BtnPN.CssClass = "btn btn-outline-primary";
            BtnSN.CssClass = "btn btn-primary";

            ViewState["PN"] = "";
            ViewState["SN"] = "";
            GrdAeron.Visible = false;
            GrdPN.Visible = false;
            GrdSN.Visible = true;
            GrdBusq.DataSource = null;
            GrdBusq.DataBind();
            BtnIngresar.Enabled = false;
            BtnGenerarOT.Enabled = true;
            RdbBusqDesSN.Checked = true;
            if (ViewState["TIPO"].ToString().Equals("A"))
            { LimpiarCampos(); }
            if (TxtCod.Text.Trim().Equals(""))
            { GrdSN.DataSource = null; GrdSN.DataBind(); GrdAdj.DataSource = null; GrdAdj.DataBind(); }
            ViewState["TIPO"] = "S";
            PerfilesGrid();
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (ViewState["TipoAccion"].ToString().Equals(""))
            {
                ViewState["TieneRegDet"] = "0";
                ActivarBotones(true, false, false, false, false);
                GrdPN.DataSource = null; GrdPN.DataBind();
                GrdSN.DataSource = null; GrdSN.DataBind();
                GrdAeron.DataSource = null; GrdAeron.DataBind();
                ViewState["TipoAccion"] = "Ingresar";
                Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                ActivarCampos(true, true, "Ingresar");
                LimpiarCampos();
                BindDDdl("SELECT", "INSERT");
                BindDataAll();
                BindDAK("SELECT");
                BindDPN("SELECT");
                Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
            }
            else
            {
                try
                {
                    ValidarSvcManto("Ingresar");
                    if (ViewState["Validar"].ToString() == "N")
                    {
                        BindDataAll();
                        return;
                    }
                    List<CsTypeServicioManto> ObjTSM = new List<CsTypeServicioManto>();
                    var detail = new CsTypeServicioManto()
                    {
                        IdSrvManto = 0,
                        CodServicioManto = "77NEO",
                        CodPatronManto = DdlGrupo.Text,
                        Descripcion = TxtDesc.Text.Trim(),
                        NroDocumento = TxtDoc.Text.Trim(),
                        CodCapitulo = DdlAta.SelectedValue,
                        BadPlan = ViewState["TIPO"].ToString().Equals("A") ? "" : "P",
                        Bandera = ViewState["TIPO"].ToString().Equals("A") ? "A" : "E",
                        BanTipoSrv = 0, //este campo tiene que ver si tiene ot cerradas y el detalle banderaOT sigue con valor 1 0 2
                        Usu = Session["C77U"].ToString(),
                        NroEtapas = 0,
                        EtapaActual = 0,
                        SubAta = "",
                        ConsecutivoAta = 0,
                        IdTipoSrv = 0,
                        AD = 0,
                        SB = 0,
                        HorizonteApertura = 0,
                        Referencia = "",
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = "REPARACION",
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = 0,
                        ServicioMayor = "",
                        Accion = "INSERT",
                        Aplicabilidad = ViewState["TIPO"].ToString(),
                    };
                    ObjTSM.Add(detail);
                    CsTypeServicioManto TblServicioManto = new CsTypeServicioManto();
                    TblServicioManto.Alimentar(ObjTSM);
                    int VblIdSvcManto = TblServicioManto.GetID();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 16,'{0}','','','','',{1},0,0,@CC,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), VblIdSvcManto);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Servicio", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["TipoAccion"] = "";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    BtnIngresar.OnClientClick = "";
                    BindDDdl("SELECT", "SELECT");
                    BindDTraerdatos(VblIdSvcManto.ToString(), "UPDATE", "ALL");
                    switch (ViewState["TIPO"].ToString())
                    {
                        case "A":
                            BindDAK("SELECT");
                            break;
                        case "P":
                        default:
                            BindDPN("SELECT");
                            break;
                    }
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["TipoAccion"].ToString().Equals(""))
            {
                if (!TxtCod.Text.Trim().Equals(""))
                {
                    ActivarBotones(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["TipoAccion"] = "Modificar";
                    ActivarCampos(false, true, "Modificar");
                    BindDDdl("SELECT", "UPDATE");
                    Result = Idioma.Select("Objeto= 'MensConfMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = row["Texto"].ToString().Trim(); };
                }
            }
            else
            {
                try
                {
                    ValidarSvcManto("Modificar");
                    if (ViewState["Validar"].ToString() == "N")
                    {
                        BindDataAll();
                        return;
                    }
                    List<CsTypeServicioManto> ObjTSM = new List<CsTypeServicioManto>();
                    var detail = new CsTypeServicioManto()
                    {
                        IdSrvManto = Convert.ToInt32(TxtId.Text),
                        CodServicioManto = TxtCod.Text.Trim(),
                        CodPatronManto = DdlGrupo.Text.Trim(),
                        Descripcion = TxtDesc.Text.Trim(),
                        NroDocumento = TxtDoc.Text.Trim(),
                        CodCapitulo = DdlAta.SelectedValue,
                        BadPlan = ViewState["TIPO"].ToString().Equals("A") ? "" : "P",
                        Bandera = ViewState["TIPO"].ToString().Equals("A") ? "A" : "E",
                        BanTipoSrv = 0, //este campo tiene que ver si tiene ot cerradas y el detalle banderaOT sigue con valor 1 0 2
                        Usu = Session["C77U"].ToString(),
                        NroEtapas = 0,
                        EtapaActual = 0,
                        SubAta = "",
                        ConsecutivoAta = 0,
                        IdTipoSrv = 0,
                        AD = 0,
                        SB = 0,
                        HorizonteApertura = 0,
                        Referencia = "",
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = "REPARACION",
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = 0,
                        ServicioMayor = "",
                        Accion = "UPDATE",
                        Aplicabilidad = ViewState["TIPO"].ToString(),
                    };
                    ObjTSM.Add(detail);
                    CsTypeServicioManto TblServicioManto = new CsTypeServicioManto();
                    TblServicioManto.Alimentar(ObjTSM);

                    DataRow[] Result = Idioma.Select("Objeto= 'BtnModificar'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["TipoAccion"] = "";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    BindDDdl("SELECT", "UPDATE");
                    BindDTraerdatos(TxtId.Text, "UPDATE", "ALL");
                    BtnModificar.OnClientClick = "";
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrMod'");
                    foreach (DataRow row in Result1)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void BtnImprimir_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string Titulo_InfSvc = "", VbMensj = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitInfSvc'");
            foreach (DataRow row in Result)
            { Titulo_InfSvc = row["Texto"].ToString().Trim() + ": "; }//Informes de servicios

            Result = Idioma.Select("Objeto= 'Mens29SM'");
            foreach (DataRow row in Result)
            { VbMensj = row["Texto"].ToString().Trim(); }//Debe seleccionar un detalle

            switch (ViewState["TIPO"].ToString())
            {
                case "A":
                    if (TxtMatric.Text.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    // TitInfSvc.Text = Titulo_InfSvc + TxtMatric.Text;
                    break;
                case "P":
                    if (ViewState["PN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    // TitInfSvc.Text = Titulo_InfSvc + ViewState["PN"].ToString();
                    break;
                default:
                    if (ViewState["SN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMensj + "');", true);
                        return;
                    }
                    // TitInfSvc.Text = Titulo_InfSvc + ViewState["PN"].ToString() + " | " + ViewState["SN"].ToString();
                    break;
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            ViewState["TieneRegDet"] = "0";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtId.Text.Trim().Equals(""))
                { return; }
                string VBQuery;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 6,'{0}','{1}','{2}','{3}','{4}','','','','',{5},0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'	",
                            TxtCod.Text, TxtDesc.Text.Trim(), ViewState["TIPO"], Session["PllaSrvManto"], Session["C77U"].ToString(), TxtId.Text);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
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

                                Transac.Commit(); BindDataAll(); LimpiarCampos();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Error en el proceso de eliminación'
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR  SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnGenerarOT_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtId.Text.Trim().Equals(""))
            {
                return;
            }
            if (ViewState["TIPO"].ToString().Equals("A") || ViewState["TIPO"].ToString().Equals("S"))//ViewState["SN"]
            {
                if (TxtMatric.Text.Trim().Equals("") && ViewState["TIPO"].ToString().Equals("A"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens19SM'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe seleccionar un registro del detalle para obtener la matrícula 
                    return;
                }
                if (ViewState["SN"].ToString().Trim().Equals("") && ViewState["TIPO"].ToString().Equals("S"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens12'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Seleccione un ítem.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasIngenieria 7,@Mtr, @Grp,@Cd, @Tp,'REPARACION','','','','',@CE,0,0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Mtr", TxtMatric.Text.Trim());
                                SC.Parameters.AddWithValue("@Grp", DdlGrupo.Text.Trim());
                                SC.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim());
                                SC.Parameters.AddWithValue("@Tp", ViewState["TIPO"]);
                                SC.Parameters.AddWithValue("@CE", ViewState["IdCodElem"]);
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                string borr = ViewState["IdCodElem"].ToString();
                                string borr1 = ViewState["TIPO"].ToString();

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
                                if (ViewState["TIPO"].ToString().Equals("S")) { BindDPN("UPDATE"); }
                                else { BindDAK("UPDATE"); }
                            }
                            catch (Exception Ex)
                            {

                                DataRow[] Result = Idioma.Select("Objeto= 'Mens22SM'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Error en el proceso de generación orden de trabajo')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "GENERAR OT", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        // **************** Grid Aeronave ***********************
        private decimal LRemanente, LRemanente1, LremanenteDia, LremanenteDia1, LCorridoDias, LCorridoDias1, LCorrido, LCorrido1;
        protected void Cumplimiento(int Id, decimal Ext, decimal ExtDia)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 24,'','','','WEB',{0},0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'", Id);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    LRemanente = Convert.ToDecimal(SDR["Remanente"].ToString());
                    LRemanente1 = LRemanente + Ext;
                    LremanenteDia = Convert.ToDecimal(SDR["Remanente2"].ToString());
                    LremanenteDia1 = LremanenteDia + ExtDia;
                    LCorridoDias = Convert.ToDecimal(SDR["DiasCorridos"].ToString()); // Calcula de % actual de cumplimiento en dias
                    LCorridoDias1 = 100 - (LremanenteDia / Convert.ToDecimal(SDR["frec2"].ToString())) * 100; // Calcula de % actual de cumplimiento en dias
                    LCorrido = Math.Round(Convert.ToDecimal(SDR["Corrido"].ToString()), 2); // Calcula de % actual de cumplimiento
                    LCorrido1 = 100 - (LRemanente / Convert.ToDecimal(SDR["Frecu"].ToString())) * 100; // Calcula de % actual de cumplimiento
                    LCorrido1 = Math.Round(LCorrido1, 2);
                }
            }
        }
        protected void BindDAK(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 6, @CS,'','','','',0,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@CS", TxtCod.Text.Trim());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        { SDA.SelectCommand = SC; SDA.Fill(DTAK); ViewState["DTAK"] = DTAK; }
                    }
                }
            }
            DTAK = (DataTable)ViewState["DTAK"];
            if (DTAK.Rows.Count > 0)
            { GrdAeron.DataSource = DTAK; GrdAeron.DataBind(); }
            else
            {
                DTAK.Rows.Add(DTAK.NewRow());
                GrdAeron.DataSource = DTAK;
                GrdAeron.DataBind();
                GrdAeron.Rows[0].Cells.Clear();
                GrdAeron.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdAeron.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                GrdAeron.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdAeron_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (e.CommandName.Equals("AddNew"))
                {
                    ViewState["CodHK"] = Convert.ToInt32((GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList).SelectedValue.Trim());
                    ViewState["Cntdr"] = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList).SelectedValue.Trim();

                    if ((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim().Equals(""))
                    { ViewState["Frec"] = Convert.ToDouble(0); }
                    else
                    { ViewState["Frec"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim()); }

                    // validar
                    ValidarHK("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { BindDataAll(); return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                        var Detail = new CsTypContaSrvMant()
                        {
                            CodIdContaSrvManto = 0,
                            CodAeronave = (int)ViewState["CodHK"],
                            CodElemento = null,
                            CodServicioManto = TxtCod.Text.Trim(),
                            Frecuencia = (double)ViewState["Frec"],
                            Extension = 0,
                            FechaVencimiento = null,
                            NroDias = 0,
                            ExtensionDias = 0,
                            BanOrdenTrabajo = 0,
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            CodOt = null,
                            Compensacion = 0,
                            Resetear = 0,
                            FrecuenciaInicial = 0,
                            FrecuenciaInicalEjecutada = 0,
                            CodContador = ViewState["Cntdr"].ToString(),
                            CodElem = "",
                            PN = "",
                            Accion = "INSERT",
                            Aplicabilidad = "HK",
                            CrearHistorico = "N",
                            Historico = "",
                        };
                        ObjTypContaSM.Add(Detail);
                        CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                        ContaSrvMant.Alimentar(ObjTypContaSM);
                        BindDataAll();
                        BindDTraerdatos(TxtId.Text.ToString(), "UPDATE", "ALL");
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["IdCodElem"] = -1;
                foreach (GridViewRow Row in GrdAeron.Rows)
                {
                    if (Row.RowIndex == GrdAeron.SelectedIndex)
                    {
                        Row.Style["background-color"] = "#D4DAD3";
                        Row.Attributes["onclick"] = "";
                        int VbID = Convert.ToInt32(GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][0].ToString());
                        TxtMatric.Text = GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][1].ToString();
                        EstadoOT(VbID);
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0) { Row.Style["background-color"] = "white"; }
                        else { Row.Style["background-color"] = "#cae4ff"; }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + Row.RowIndex);
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SELECT DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdAeron.EditIndex = e.NewEditIndex; BindDataAll(); BindDAK("SELECT"); }
        protected void GrdAeron_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());
                ViewState["CodHK"] = Convert.ToInt32((GrdAeron.Rows[e.RowIndex].FindControl("DdlHK") as DropDownList).SelectedValue.Trim());
                ViewState["Cntdr"] = (GrdAeron.Rows[e.RowIndex].FindControl("DdlCont") as DropDownList).SelectedValue.Trim();
                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                { ViewState["Frec"] = Convert.ToDouble(0); }
                else
                { ViewState["Frec"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim()); }
                // validar
                ValidarHK("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                { BindDataAll(); return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                    var Detail = new CsTypContaSrvMant()
                    {
                        CodIdContaSrvManto = VblId,
                        CodAeronave = (int)ViewState["CodHK"],
                        CodElemento = null,
                        CodServicioManto = TxtCod.Text.Trim(),
                        Frecuencia = (double)ViewState["Frec"],
                        Extension = 0,
                        FechaVencimiento = null,
                        NroDias = 0,
                        ExtensionDias = 0,
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = 0,
                        FrecuenciaInicial = 0,
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = "",
                        PN = "",
                        Accion = "UPDATE",
                        Aplicabilidad = "HK",
                        CrearHistorico = "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdAeron.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDataAll();
                    BindDAK("UPDATE");
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAeron.EditIndex = -1; BindDAK("SELECT"); }
        protected void GrdAeron_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 8,'','','','','VALIDA',{0},0,0,@ICC,'01-01-01','01-01-01','01-01-01'", IDContaSrvManto);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 8,'','','','{0}','DELETE',{1},0,0, @ICC,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), IDContaSrvManto);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDTraerdatos(TxtId.Text.ToString(), "UPDATE", "ALL");
                                BindDataAll();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET AERONAVE", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    string VbCondHK;
                    DropDownList DdlHKPP = (e.Row.FindControl("DdlHKPP") as DropDownList);
                    DataTable DT = new DataTable();
                    if (DdlModel.Text.Trim().Equals("")) { VbCondHK = "CodModelo<> '77NEODemp'"; }
                    else { VbCondHK = "CodModelo= '" + DdlModel.Text.Trim() + "' OR CodModelo = ''"; }
                    DataRow[] DR = DSDdl.Tables[5].Select(VbCondHK);
                    if (IsIENumerableLleno(DR))
                    { DT = DR.CopyToDataTable(); }
                    DdlHKPP.DataSource = DT;
                    DdlHKPP.DataTextField = "Matricula";
                    DdlHKPP.DataValueField = "CodAeronave";
                    DdlHKPP.DataBind();

                    TextBox TxtFrec = (e.Row.FindControl("TxtFrecPP") as TextBox);
                    TxtFrec.ReadOnly = true;
                    TxtFrec.Enabled = false;
                    TxtFrec.Text = "1";
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {

                    DropDownList DdlHK = (e.Row.FindControl("DdlHK") as DropDownList);
                    DdlHK.DataSource = DSDdl.Tables[6]; //HkMod
                    DdlHK.DataTextField = "Matricula";
                    DdlHK.DataValueField = "CodAeronave";
                    DdlHK.DataBind();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DdlHK.SelectedValue = dr["CodHK"].ToString();

                    DataTable DT = new DataTable();
                    DT = DSDdl.Tables[7].Clone();//CONMOD
                    //DT.Rows.Add(" - ", "0", "");
                    Result = DSDdl.Tables[7].Select("CodAeronave =" + dr["CodHK"].ToString());
                    foreach (DataRow Row in Result)
                    { DT.ImportRow(Row); }
                    DropDownList DdlCont = (e.Row.FindControl("DdlCont") as DropDownList);
                    DdlCont.DataSource = DT;
                    DdlCont.DataTextField = "CodContador";
                    DdlCont.DataValueField = "Cod";
                    DdlCont.DataBind();
                    DataRowView DRVC = e.Row.DataItem as DataRowView;
                    DdlCont.SelectedValue = DRVC["CodContador"].ToString();

                    TextBox TxtFrec = (e.Row.FindControl("TxtFrec") as TextBox);
                    TxtFrec.ReadOnly = true;
                    TxtFrec.Enabled = false;
                    TxtFrec.Text = "1";

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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + e.Row.RowIndex);
                    Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// Seleccione el registro.

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
                        Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    } /**/
                }
            }
        }
        protected void GrdAeron_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAeron.PageIndex = e.NewPageIndex; BindDataAll(); BindDAK("SELECT"); }
        // **************** Grid P/N ***********************
        protected void BindDPN(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 22,@Doc,'','','','',0,0,0, @ICC,'01-01-01','01-01-01','01-01-01'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@Doc", TxtCod.Text);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSPS = new DataSet())
                            {
                                DSPNSN.Clear();
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSPNSN);
                                DSPNSN.Tables[0].TableName = "DetPN";
                                DSPNSN.Tables[1].TableName = "DetSN";
                                ViewState["DSPNSN"] = DSPNSN;
                            }
                        }
                    }

                }
            }
            DSPNSN = (DataSet)ViewState["DSPNSN"];
            if (DSPNSN.Tables[1].Rows.Count > 0)
            {
                GrdSN.DataSource = DSPNSN.Tables[1];
                GrdSN.DataBind();
            }
            else { GrdSN.DataSource = null; GrdSN.DataBind(); }
            if (DSPNSN.Tables[0].Rows.Count > 0)
            {
                GrdPN.DataSource = DSPNSN.Tables[0];
                GrdPN.DataBind();
            }
            else
            {
                DSPNSN.Tables["DetPN"].Rows.Add(DSPNSN.Tables["DetPN"].NewRow());
                GrdPN.DataSource = DSPNSN.Tables["DetPN"];
                GrdPN.DataBind();
                GrdPN.Rows[0].Cells.Clear();
                GrdPN.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdPN.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                GrdPN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtCod.Text.Equals("")) { return; }
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    ViewState["PN"] = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Cntdr"] = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList).SelectedValue.Trim();

                    if ((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim().Equals(""))
                    { ViewState["Frec"] = Convert.ToInt32(0); }
                    else
                    { ViewState["Frec"] = Convert.ToInt32((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim()); }
                    // validar
                    ValidarDetPN("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { BindDataAll(); return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        List<TypeContSrvPn> ObjContSrvPn = new List<TypeContSrvPn>();
                        var Detail = new TypeContSrvPn()
                        {
                            CodidcodSrvPn = 0,
                            CodServicioManto = TxtCod.Text,
                            CodIdContadorPn = 0,
                            Frecuencia = (int)ViewState["Frec"],
                            NroDias = 0,
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            Resetear = 0,
                            Accion = "INSERT",
                            PN = ViewState["PN"].ToString(),
                            CodContador = ViewState["Cntdr"].ToString(),
                        };
                        ObjContSrvPn.Add(Detail);
                        TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                        ContSrvPn.Alimentar(ObjContSrvPn);
                        BindDataAll();
                        BindDTraerdatos(TxtId.Text.ToString(), "UPDATE", "ALL");
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET PN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow Row in GrdPN.Rows)
            {
                if (Row.RowIndex == GrdPN.SelectedIndex)
                {
                    Row.Style["background-color"] = "#D4DAD3";
                    Row.Attributes["onclick"] = "";
                    ViewState["PN"] = GrdPN.DataKeys[this.GrdPN.SelectedIndex][2].ToString();
                }
                else
                {
                    if (Row.RowIndex % 2 == 0) { Row.Style["background-color"] = "white"; }
                    else { Row.Style["background-color"] = "#cae4ff"; }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + Row.RowIndex);
                }
            }
        }
        protected void GrdPN_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdPN.EditIndex = e.NewEditIndex; BindDataAll(); BindDPN("SELECT"); }
        protected void GrdPN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                int VblId = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());
                int VbIdContPN = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Values["CodIdContadorPn"].ToString());
                PerfilesGrid();
                ViewState["PN"] = (GrdPN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();
                ViewState["Cntdr"] = (GrdPN.Rows[e.RowIndex].FindControl("LblContPN") as Label).Text.Trim();
                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim().Equals(""))
                { ViewState["Frec"] = Convert.ToInt32(0); }
                else
                { ViewState["Frec"] = Convert.ToInt32((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim()); }
                // validar
                ValidarDetPN("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                { BindDataAll(); return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<TypeContSrvPn> ObjContSrvPn = new List<TypeContSrvPn>();
                    var Detail = new TypeContSrvPn()
                    {
                        CodidcodSrvPn = VblId,
                        CodServicioManto = TxtCod.Text,
                        CodIdContadorPn = VbIdContPN,
                        Frecuencia = (int)ViewState["Frec"],
                        NroDias = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        Resetear = 0,
                        Accion = "UPDATE",
                        PN = ViewState["PN"].ToString(),
                        CodContador = ViewState["Cntdr"].ToString(),
                    };
                    ObjContSrvPn.Add(Detail);
                    TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                    ContSrvPn.Alimentar(ObjContSrvPn);
                    GrdPN.EditIndex = -1;
                    BindDataAll();
                    BindDPN("UPDATE");
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la edicion')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET PN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdPN.EditIndex = -1; BindDPN("SELECT"); }
        protected void GrdPN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VBQuery;
                int CodidcodSrvPn = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','','','','VALIDA',{0},0,0,@ICC,'01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','{2}','','','DELETE',{0},0,0, @ICC, '01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text, Session["C77U"].ToString());
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDTraerdatos(TxtId.Text.ToString(), "UPDATE", "ALL");
                                BindDataAll();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET P/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlPNPP = (e.Row.FindControl("DdlPNPP") as DropDownList);
                    DdlPNPP.DataSource = DSDdl.Tables[8];
                    DdlPNPP.DataTextField = "PN";
                    DdlPNPP.DataValueField = "CodPN";
                    DdlPNPP.DataBind();
                    TextBox TxtFrec = (e.Row.FindControl("TxtFrecPNPP") as TextBox);
                    TxtFrec.ReadOnly = true;
                    TxtFrec.Enabled = false;
                    TxtFrec.Text = "1";
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox TxtFrec = (e.Row.FindControl("TxtFrecPN") as TextBox);
                    TxtFrec.ReadOnly = true;
                    TxtFrec.Enabled = false;
                    TxtFrec.Text = "1";
                    ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                    if (IbtUpdate != null)
                    {
                        Result = Idioma.Select("Objeto= 'IbtUpdate'");
                        foreach (DataRow row in Result)
                        { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                    ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                    if (IbtUpdate != null)
                    {
                        Result = Idioma.Select("Objeto= 'IbtCancel'");
                        foreach (DataRow row in Result)
                        { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + e.Row.RowIndex);
                    Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// 

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
                        Result = Idioma.Select("Objeto='IbtDelete'");
                        foreach (DataRow RowIdioma in Result)
                        { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }/* */
                }
            }
        }
        protected void GrdPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAeron.PageIndex = e.NewPageIndex; BindDataAll(); BindDPN("SELECT"); }
        // **************** Grid S/N ***********************
        protected void GrdSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdCodElem = Convert.ToInt32(GrdSN.DataKeys[this.GrdSN.SelectedIndex][3].ToString()); //Indices 0 = pos1
                ViewState["CodElem"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][1].ToString().Trim();

                ViewState["IdCodElem"] = IdCodElem;
                foreach (GridViewRow Row in GrdSN.Rows)
                {
                    if (Row.RowIndex == GrdSN.SelectedIndex)
                    {
                        Row.Style["background-color"] = "#D4DAD3";
                        Row.Attributes["onclick"] = "";

                        ViewState["PN"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][4].ToString().Trim();
                        ViewState["SN"] = GrdSN.DataKeys[this.GrdSN.SelectedIndex][5].ToString().Trim();

                        TxtMatric.Text = GrdSN.DataKeys[this.GrdSN.SelectedIndex][2].ToString();
                        int VbID = Convert.ToInt32(GrdSN.DataKeys[this.GrdSN.SelectedIndex][0].ToString());
                        EstadoOT(VbID);
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0) { Row.Style["background-color"] = "white"; }
                        else { Row.Style["background-color"] = "#cae4ff"; }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + Row.RowIndex);
                    }
                    PerfilesGrid();
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SelectedIndexChanged DET S/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdSN.EditIndex = e.NewEditIndex; BindDataAll(); BindDPN("SELECT"); }
        protected void GrdSN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());
                ViewState["PN"] = (GrdSN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();

                ViewState["CodElem"] = GrdSN.DataKeys[e.RowIndex].Values["CodElem"].ToString();

                ViewState["Cntdr"] = (GrdSN.Rows[e.RowIndex].FindControl("LblCont") as Label).Text.Trim();

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                { ViewState["Frec"] = Convert.ToDouble(0); }
                else
                { ViewState["Frec"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim()); }
                // validar
                ValidaDetSN();
                if (ViewState["Validar"].Equals("N"))
                {
                    BindDataAll();
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    List<CsTypContaSrvMant> ObjTypContaSM = new List<CsTypContaSrvMant>();
                    var Detail = new CsTypContaSrvMant()
                    {
                        CodIdContaSrvManto = VblId,
                        CodAeronave = 0,
                        CodElemento = 0,
                        CodServicioManto = TxtCod.Text.Trim(),
                        Frecuencia = (double)ViewState["Frec"],
                        Extension = 0,
                        FechaVencimiento = null,
                        NroDias = 0,
                        ExtensionDias = 0,
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = 0,
                        FrecuenciaInicial = 0,
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = ViewState["CodElem"].ToString(),
                        PN = ViewState["PN"].ToString(),
                        Accion = "UPDATE",
                        Aplicabilidad = "SN",
                        CrearHistorico = "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdSN.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDataAll();
                    BindDPN("UPDATE");
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET SN", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdSN.EditIndex = -1; BindDPN("SELECT"); }
        protected void GrdSN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 12,'','','','','VALIDA',{0},0,0, @ICC,'01-01-01','01-01-01','01-01-01'", IDContaSrvManto);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                        PerfilesGrid();
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 12,'','','','{0}','DELETE',{1},{2},0, @ICC,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), IDContaSrvManto, TxtId.Text);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDPN("UPDATE");
                                BindDataAll();
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET S/N", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
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
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + e.Row.RowIndex);
                    DataRow[] Result = Idioma.Select("Objeto= 'GrdSelecReg'");
                    foreach (DataRow row in Result)
                    { e.Row.ToolTip = row["Texto"].ToString().Trim(); }// 

                    ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                    imgE.Enabled = true;
                    Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        protected void GrdSN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdSN.PageIndex = e.NewPageIndex; BindDataAll(); BindDPN("SELECT"); }
        // **************** Grid Adjuntos ***********************
        protected void BindDAdjunto(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 28,'DOCINGENIERIA','{0}','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'", TxtCod.Text);
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        { SDA.SelectCommand = SC; SDA.Fill(DTAdj); ViewState["DTAdj"] = DTAdj; }
                    }
                }
            }
            DTAdj = (DataTable)ViewState["DTAdj"];
            if (DTAdj.Rows.Count > 0)
            { GrdAdj.DataSource = DTAdj; GrdAdj.DataBind(); }
            else
            {
                DTAdj.Rows.Add(DTAdj.NewRow());
                GrdAdj.DataSource = DTAdj;
                GrdAdj.DataBind();
                GrdAdj.Rows[0].Cells.Clear();
                GrdAdj.Rows[0].Cells.Add(new TableCell());
                DataRow[] Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdAdj.Rows[0].Cells[0].Text = row["Texto"].ToString().Trim(); }
                GrdAdj.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdAdj_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (TxtCod.Text.Equals(""))
                { BindDataAll(); return; }
                if (e.CommandName.Equals("Download"))
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int VblID = int.Parse(GrdAdj.DataKeys[gvr.RowIndex].Value.ToString());
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        string LtxtSql = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 2,'','','','','',{0},0,0,@ICC,'01-01-01','01-01-01','01-01-01'", VblID);
                        SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SqlDataReader SDR = SC.ExecuteReader();
                        if (SDR.Read())
                        {
                            Vbl8Type = HttpUtility.HtmlDecode(SDR["TipoArchivo"].ToString().Trim());
                            imagen = (byte[])SDR["ArchivoAdj"];
                            Vbl4Ruta = HttpUtility.HtmlDecode(SDR["Nombre"].ToString().Trim());
                            //Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Vbl4Ruta));
                            Response.ContentType = Vbl8Type;
                            //finalmente escribimos los bytes en la respuesta de la página web
                            Response.BinaryWrite(imagen);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens25SM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la descarga
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "Descargar adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    PerfilesGrid();
                    Vbl3Desc = (GrdAdj.FooterRow.FindControl("TxtDescPP") as TextBox).Text.Trim();
                    FileUpload FileUpPP = (FileUpload)GrdAdj.FooterRow.FindControl("FileUpPP");
                    if (FileUpPP != null)
                    {
                        if (FileUpPP.HasFile)
                        {
                            Vbl4Ruta = FileUpPP.FileName;
                            Vbl6Ext = Path.GetExtension(Vbl4Ruta);
                            Vbl8Type = FileUpPP.PostedFile.ContentType;
                            imagen = new byte[FileUpPP.PostedFile.InputStream.Length];
                            FileUpPP.PostedFile.InputStream.Read(imagen, 0, imagen.Length);
                        }
                        else
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens26SM'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe seleccionar un archivo
                            return;
                        }
                    }
                    if (Vbl3Desc.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                        return;
                    }
                    if (Vbl4Ruta.Equals(""))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'Mens26SM'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un archivo')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = string.Format("INSERT INTO TblAdjuntos(IdProceso,CodProceso,Proceso,Descripcion,Ruta,ArchivoAdj,Extension,UsuCrea,UsuMod,FechaCrea,FechaMod,TipoArchivo, IdConfigCia)  " +
                            "VALUES({0},'{1}','{2}',@Desc,'{4}',@Image,'{6}','{7}','{7}',GETDATE(),GETDATE(),'{8}', @ICC)",
                            TxtId.Text, TxtCod.Text, "DOCINGENIERIA", "3 N/A", Vbl4Ruta, "Vbl5Adj", Vbl6Ext, Session["C77U"].ToString(), Vbl8Type);
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                                SqlCmd.Parameters.AddWithValue("@Image", imagen);
                                SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SqlCmd.ExecuteNonQuery();
                                BindDAdjunto("UPDATE");
                                PerfilesGrid();
                            }
                            catch (Exception Ex)
                            {
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT TblAdjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAdj_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdAdj.EditIndex = e.NewEditIndex; BindDAdjunto("SELECT"); }
        protected void GrdAdj_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            Vbl3Desc = (GrdAdj.Rows[e.RowIndex].FindControl("TxtDesc") as TextBox).Text.Trim();
            FileUpload FileUp = GrdAdj.Rows[GrdAdj.EditIndex].FindControl("FileUp") as FileUpload;
            if (FileUp != null)
            {
                if (FileUp.HasFile)
                {
                    Vbl4Ruta = FileUp.FileName;
                    Vbl6Ext = Path.GetExtension(Vbl4Ruta);
                    Vbl8Type = FileUp.PostedFile.ContentType;
                    imagen = new byte[FileUp.PostedFile.InputStream.Length];
                    FileUp.PostedFile.InputStream.Read(imagen, 0, imagen.Length);
                }
            }
            if (Vbl3Desc.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05SM'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una descripción')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VblSiAdjunto = FileUp.HasFile == true ? " Ruta = @Nom,ArchivoAdj = @Image, Extension = @Ext,TipoArchivo = @TipoA," : "";
                sqlCon.Open();
                VBQuery = string.Format("UPDATE TblAdjuntos SET Descripcion =@Desc ," + VblSiAdjunto + "  UsuMod='{1}', FechaMod=GETDATE() " +
                    "WHERE IdAdjuntos = {0} AND IdConfigCia = @ICC", GrdAdj.DataKeys[e.RowIndex].Value.ToString(), Session["C77U"].ToString());
                using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon))
                {
                    try
                    {
                        SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                        SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        if (FileUp.HasFile)
                        {
                            SqlCmd.Parameters.AddWithValue("@Nom", Vbl4Ruta);
                            SqlCmd.Parameters.AddWithValue("@Image", imagen);
                            SqlCmd.Parameters.AddWithValue("@Ext", Vbl6Ext);
                            SqlCmd.Parameters.AddWithValue("@TipoA", Vbl8Type);
                        }
                        SqlCmd.ExecuteNonQuery();
                        GrdAdj.EditIndex = -1;
                        BindDAdjunto("UPDATE");
                    }
                    catch (Exception Ex)
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la actualización')", true);
                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPATE Adjunto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                    }
                }
            }
        }
        protected void GrdAdj_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdAdj.EditIndex = -1; BindDAdjunto("SELECT"); }
        protected void GrdAdj_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    int VblId = Convert.ToInt32(GrdAdj.DataKeys[e.RowIndex].Values["IdAdjuntos"].ToString());
                    string VblRuta = GrdAdj.DataKeys[e.RowIndex].Values["Ruta"].ToString();

                    string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 13,'{0}','{1}','','','',{2},{3},0,@ICC,'01-01-01','01-01-01','01-01-01'"
                           , Session["C77U"].ToString(), VblRuta, VblId, TxtId.Text);
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDAdjunto("UPDATE");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE Adjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdAdj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {

                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdateAdj") as ImageButton);
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
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgE != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
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
        }
        protected void GrdAdj_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdAdj.PageIndex = e.NewPageIndex; BindDAdjunto("SELECT"); }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetFixedHeightForGridIfRowsAreLess(GrdAeron);
            SetFixedHeightForGridIfRowsAreLess(GrdPN);
            SetFixedHeightForGridIfRowsAreLess(GrdSN);
        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 25; //we set header height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 25;
                gv.Height = new Unit(height);
            }
        }
        // ****************Opciones de busqueda ***********************
        protected void BIndDataBusq()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql, VbOpcion = "";
                VbTxtSql = "";
                if (RdbBusqDes.Checked == true && TblBusqHK.Visible == true) { VbOpcion = "D"; }
                if (RdbBusqDesPN.Checked == true && TblBusqPN.Visible == true) { VbOpcion = "D"; }
                if (RdbBusqPnPN.Checked == true && TblBusqPN.Visible == true) { VbOpcion = "P"; }
                if (RdbBusqDesSN.Checked == true && TblBusqSN.Visible == true) { VbOpcion = "D"; }
                if (RdbBusqPnSN.Checked == true && TblBusqSN.Visible == true) { VbOpcion = "P"; }
                if (RdbBusqSnSN.Checked == true && TblBusqSN.Visible == true) { VbOpcion = "S"; }
                if (!VbOpcion.Equals(""))
                {
                    VbTxtSql = "EXEC SP_PANTALLA__Servicio_Manto2 15,@Txt,@Tp,'REPARACION',@Opc,'',0,0,0,@CC,'01-01-01','01-01-01','01-01-01'";
                    sqlConB.Open();

                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Txt", TxtBusqueda.Text.Trim());
                        SC.Parameters.AddWithValue("@Tp", ViewState["TIPO"]);
                        SC.Parameters.AddWithValue("@Opc", VbOpcion);
                        SC.Parameters.AddWithValue("@CC", Session["!dC!@"]);
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);
                            if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                            else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                        }
                    }
                }
                TxtBusqueda.Focus();
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            if (ViewState["TIPO"].ToString().Equals("A"))
            {
                TblBusqHK.Visible = true;
                TblBusqPN.Visible = false;
                TblBusqSN.Visible = false;
                RdbBusqDes.Checked = true;
            }
            if (ViewState["TIPO"].ToString().Equals("P"))
            {
                TblBusqHK.Visible = false;
                TblBusqPN.Visible = true;
                TblBusqSN.Visible = false;
            }
            if (ViewState["TIPO"].ToString().Equals("S"))
            {
                TblBusqHK.Visible = false;
                TblBusqPN.Visible = false;
                TblBusqSN.Visible = true;
            }
            Page.Title = ViewState["PageTit"].ToString(); TxtBusqueda.Focus();
            MultVw.ActiveViewIndex = 1;
        }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BIndDataBusq(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string vbcod = ((Label)Row.FindControl("LblId")).Text.ToString().Trim();
                GridViewRow GVR = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                BindDDdl("SELECT", "SELECT");
                BindDTraerdatos(vbcod, "UPDATE", "ALL");
                PerfilesGrid();
                TxtEstadoOT.Text = "";
                TxtMatric.Text = "";
                MultVw.ActiveViewIndex = 0;
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtIr = e.Row.FindControl("IbtIr") as ImageButton;
                if (IbtIr != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        // ***************************** Recurso  *****************************
        protected void BtnRecurso_Click(object sender, EventArgs e)
        {
            if (!TxtId.Text.Trim().Equals(""))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                MultVw.ActiveViewIndex = 2;
                if (CkbBloqRec.Checked == true)
                {
                    GrdRecursoF.FooterRow.Enabled = false;
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { GrdRecursoF.FooterRow.ToolTip = row["Texto"].ToString().Trim(); }// "El recurso se encuentra bloqueado";
                }/* */
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void IbtCloseRecurso_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString(); }
        protected void DdlPNRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            TextBox TxtDesRFPP = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox);
            DropDownList DdlPNRFPP = (GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList);
            TextBox TxtPNRFPP = (GrdRecursoF.FooterRow.FindControl("TxtPNRFPP") as TextBox);

            if (DdlPNRFPP.Text.Trim().Equals("- N -"))
            {
                DdlPNRFPP.Visible = false;
                TxtPNRFPP.Visible = true;
                TxtPNRFPP.Enabled = true;
                TxtDesRFPP.Text = "";
                TxtDesRFPP.Enabled = true;
                return;
            }

            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result = DSDdl.Tables[4].Select("PN= '" + DdlPNRFPP.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            { TxtDesRFPP.Text = Row["Descripcion"].ToString(); }
        }
        protected void GrdRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VblPN, VBQuery, VblTxtCant, VbDesc;
                    int VblFase, VblCond;
                    double VblCant;
                    if ((GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).Visible == true)
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("DdlPNRFPP") as DropDownList).SelectedValue.Trim(); }
                    else
                    { VblPN = (GrdRecursoF.FooterRow.FindControl("TxtPNRFPP") as TextBox).Text.Trim(); }
                    VblFase = (GrdRecursoF.FooterRow.FindControl("TxtFaseRFPP") as TextBox).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.FooterRow.FindControl("TxtFaseRFPP") as TextBox).Text.Trim());
                    VblTxtCant = (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim();
                    CultureInfo Culture = new CultureInfo("en-US");
                    VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                    VblCond = (GrdRecursoF.FooterRow.FindControl("CkbCondicPP") as CheckBox).Checked == true ? 1 : 0;
                    VbDesc = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = "EXEC SP_TablasIngenieria 5,@PN,@Us,@Desc,'','','','','','INSERT',@IdPlIns,@IdSvc,@Cnt,@Condc,@Fs,@ICC,'01-01-1','02-01-1','03-01-1'";
                            using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SC.Parameters.AddWithValue("@PN", VblPN);
                                    SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                    SC.Parameters.AddWithValue("@Desc", VbDesc);
                                    SC.Parameters.AddWithValue("@IdPlIns", 0);
                                    SC.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                    SC.Parameters.AddWithValue("@Cnt", VblCant);
                                    SC.Parameters.AddWithValue("@Condc", VblCond);
                                    SC.Parameters.AddWithValue("@Fs", VblFase);
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
                                    //BindDRecursoF();
                                    BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso'
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdRecursoF.EditIndex = e.NewEditIndex; BindDTraerdatos(TxtId.Text.Trim(), "SELECT", "RECURSO"); }   // BindDRecursoF();
        protected void GrdRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VblPN, VBQuery, VblTxtCant;
                int VblFase, VblCond;
                double VblCant;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[e.RowIndex].Value.ToString());
                VblPN = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtPNRF") as TextBox).Text.Trim();
                string VbSDesc = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtDesRF") as TextBox).Text.Trim();
                VblFase = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim());
                VblTxtCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim();
                CultureInfo Culture = new CultureInfo("en-US");
                VblCant = VblTxtCant.Length == 0 ? 0 : Convert.ToDouble(VblTxtCant, Culture);
                VblCond = (GrdRecursoF.Rows[e.RowIndex].FindControl("CkbCondic") as CheckBox).Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasIngenieria 5,@PN,@Us, @Desc,'','','','','','UPDATE',@IdPlIns,@IdSvc,@Cant,@Condc,@Fs,@ICC,'01-01-1','02-01-1','03-01-1'";

                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PN", VblPN);
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@Desc", VbSDesc);
                                SC.Parameters.AddWithValue("@IdPlIns", VblId);
                                SC.Parameters.AddWithValue("@IdSvc", TxtId.Text);
                                SC.Parameters.AddWithValue("@Cant", VblCant);
                                SC.Parameters.AddWithValue("@Condc", VblCond);
                                SC.Parameters.AddWithValue("@Fs", VblFase);
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
                                GrdRecursoF.EditIndex = -1;
                                //BindDRecursoF();
                                BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdRecursoF.EditIndex = -1; BindDTraerdatos(TxtId.Text.Trim(), "SELECT", "RECURSO"); }//BindDRecursoF();
        protected void GrdRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                string VblPN, VBQuery, VblTxtCant;
                int VblFase, VblCond;
                double VblCant;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[e.RowIndex].Value.ToString());
                VblPN = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblPn") as Label).Text.Trim();
                VblFase = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblFaseRF") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.Rows[e.RowIndex].FindControl("LblFaseRF") as Label).Text.Trim());
                VblTxtCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim().Equals("") ? "0" : (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim();
                Cnx.RetirarPuntos(VblTxtCant);
                VblTxtCant = Cnx.ValorDecimal();
                VblCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("LblCantRF") as Label).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VblTxtCant);
                VblCond = (GrdRecursoF.Rows[e.RowIndex].FindControl("CkbCondicP") as CheckBox).Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 5,'{0}','{1}','','','','','','','DELETE',{2},{3},@Cant,{4},{5},@ICC,'01-01-1','02-01-1','03-01-1'",
                        VblPN, Session["C77U"].ToString(), VblId, TxtId.Text, VblCond, VblFase);

                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Cant", VblCant);
                                SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                var Mensj = SqlCmd.ExecuteScalar();
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
                                BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0,{1},{0},'01-01-01','01-01-01','01-01-01'", Session["!dC!@"], Session["77IDM"]);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNRFPP.DataTextField = "PN";
                DdlPNRFPP.DataValueField = "CodPN";
                DdlPNRFPP.DataBind();
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result) { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                if (CkbBloqRec.Checked == true)
                {
                    Result = Idioma.Select("Objeto= 'Mens18SM'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                    IbtAddNew.Enabled = false;
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
                    DataRow[] Result1 = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result1) { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                if (imgD != null)
                {
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result) { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result) { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
        // ***************************** Licencias  *****************************
        protected void DdlLicenRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            TextBox TxtDesLiRFPP = (GrdLicen.FooterRow.FindControl("TxtDesLiRFPP") as TextBox);
            DropDownList DdlLicenRFPP = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList);
            DSDdl = (DataSet)ViewState["DSDdl"];
            DataRow[] Result = DSDdl.Tables[9].Select("CodIdLicencia= " + DdlLicenRFPP.Text.Trim());
            foreach (DataRow Row in Result)
            { TxtDesLiRFPP.Text = Row["Descripcion"].ToString(); }
        }
        protected void GrdLicen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VBQuery, VblTxtTE, VbCodIdLicencia;
                    double VblTE;
                    if ((GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue.Equals("0"))
                    {
                        DataRow[] Result = Idioma.Select("Objeto= 'MstrMens01'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); };// Debe ingrear la licencia
                        return;
                    }
                    VbCodIdLicencia = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue;
                    CultureInfo Culture = new CultureInfo("en-US");
                    VblTxtTE = (GrdLicen.FooterRow.FindControl("TxtTieEstRFPP") as TextBox).Text.Trim().Equals("") ? "0" : (GrdLicen.FooterRow.FindControl("TxtTieEstRFPP") as TextBox).Text.Trim();
                    VblTE = VblTxtTE.Length == 0 ? 0 : Convert.ToDouble(VblTxtTE, Culture);
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','INSERT',{2},{3},@TiempEst,0,0,@ICC,'01-01-1','02-01-1','03-01-1'",
                            Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia);
                            using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                                    SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    var Mensj = SqlCmd.ExecuteScalar();
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
                                    //BindDLicencia();
                                    BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdLicen.EditIndex = e.NewEditIndex; BindDTraerdatos(TxtId.Text.Trim(), "SELECT", "RECURSO"); }//BindDLicencia();
        protected void GrdLicen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                PerfilesGrid();
                string VBQuery, VblTxtTE;
                double VblTE;
                int IdSrvLic = Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString());
                string VbCodIdLicencia = GrdLicen.DataKeys[e.RowIndex].Values["CodIdLicencia"].ToString();
                CultureInfo Culture = new CultureInfo("en-US");
                VblTxtTE = (GrdLicen.Rows[e.RowIndex].FindControl("TxtTieEstRF") as TextBox).Text.Trim().Equals("") ? "0" : (GrdLicen.Rows[e.RowIndex].FindControl("TxtTieEstRF") as TextBox).Text.Trim();
                VblTE = VblTxtTE.Length == 0 ? 0 : Convert.ToDouble(VblTxtTE, Culture);

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','UPDATE',{2},{3},@TiempEst,{4},0, @ICC,'01-01-1','02-01-1','03-01-1'",
                         Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                                SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                GrdLicen.EditIndex = -1;
                                //BindDLicencia();
                                BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdLicen.EditIndex = -1; BindDTraerdatos(TxtId.Text.Trim(), "SELECT", "RECURSO"); }//BindDLicencia();
        private string VblTE;
        protected void GrdLicen_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            int IdSrvLic = Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString());
            string VbCodIdLicencia = GrdLicen.DataKeys[e.RowIndex].Values["CodIdLicencia"].ToString();
            foreach (GridViewRow row in GrdLicen.Rows)
            {
                if (Convert.ToInt32(GrdLicen.DataKeys[e.RowIndex].Value.ToString()) == Convert.ToInt32(GrdLicen.DataKeys[row.RowIndex].Value.ToString()))
                {
                    VblTE = ((Label)row.FindControl("LblTieEstRF")).Text;
                }
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','DELETE',{2},{3},@TiempEst,{4},0, @ICC,'01-01-1','02-01-1','03-01-1'",
                    Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);

                    using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SqlCmd.Parameters.AddWithValue("@TiempEst", Convert.ToDouble(VblTE));
                            SqlCmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            SqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            //BindDLicencia();
                            BindDTraerdatos(TxtId.Text.Trim(), "UPDATE", "RECURSO");
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            DataRow[] Result;
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','LICRF',{0},0,{2},{1},'01-01-01','01-01-01','01-01-01'", TxtId.Text, Session["!dC!@"], Session["77IDM"]);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlLicenRFPP.DataTextField = "CodLicencia";
                DdlLicenRFPP.DataValueField = "CodIdLicencia";
                DdlLicenRFPP.DataBind();

                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                IbtAddNew.Enabled = true;
                Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
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
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }

                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
        }
    }
}