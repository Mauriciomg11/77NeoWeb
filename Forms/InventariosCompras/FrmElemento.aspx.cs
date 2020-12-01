using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.Prg.PrgLogistica;
using System.Reflection.Emit;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmElemento : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        private DateTime FechaD = DateTime.Today;
        protected void Page_Load(object sender, EventArgs e)
        {
           if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }  /**/
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/*
               Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoHCT"; //DbNeoHCT | DbNeoDempV2
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";  */
            }
            if (!IsPostBack)
            {
                Session["VldrElem"] = "S";
                ViewState["CodBodegaE"] = "";
                ViewState["IdentificadorE"] = "";
                ViewState["PondMatSN"] = "N";
                ViewState["PondCompSN"] = "N";
                ViewState["FechaVenceE"] = "";
                ViewState["PNAntEle"] = "";
                ViewState["SNAntEle"] = "";
                ViewState["GrupoEle"] = "";
                ModSeguridad();
                ActivarCampos(false, false, "");
                ActivarBotones(true, false, false, false, true);
                BindDataDdl("");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblConsMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmElemento.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnModificar.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
                ViewState["VblConsMS"] = 0;
            }
            if (ClsP.GetImprimir() == 0)
            {

            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
            }
            if (ClsP.GetCE1() == 0)
            {

            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string TxQry = "EXEC SP_ConfiguracionV2_ 19,'PONDERADO','PONDERADO','','','" + Session["Nit77Cia"].ToString() + "',1,2,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                sqlCon.Open();
                SqlDataReader Regs = Comando.ExecuteReader();
                while (Regs.Read())
                {
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    if (VbCaso == 1 && VbAplica.Equals("S"))
                    {
                        // Material Serializado
                        ViewState["PondMatSN"] = "S";
                    }
                    if (VbCaso == 2 && VbAplica.Equals("S"))
                    {
                        // Componenente Serializado
                        ViewState["PondCompSN"] = "S";
                    }
                }
            }
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            if (ViewState["IdentificadorE"].Equals("SN"))
            {
                switch (DdlGrupo.SelectedValue)
                {
                    case "01":
                        if (ViewState["PondMatSN"].Equals("N"))
                        { DdlPN.Enabled = Edi; }
                        TxtSN.Enabled = Edi;
                        break;
                    case "02":
                        if (ViewState["PondCompSN"].Equals("N"))
                        { DdlPN.Enabled = Edi; }
                        TxtSN.Enabled = Edi;
                        break;
                    case "03":
                        DdlPN.Enabled = Edi;
                        TxtSN.Enabled = Edi;
                        RdbActivo.Enabled = Edi;
                        RdbInactivo.Enabled = Edi;
                        break;
                }

            }
            if (ViewState["FechaVenceE"].Equals("S"))
            {
                //TxtFecShelfLife.Enabled = Edi;
                IbtFechaI.Enabled = Edi;
            }
        }
        protected void TraerDatos()
        {
            if (TxtCod.Text.ToString() != string.Empty)
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    string TxtFecha;
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_Elemento 8,'{0}','','','COD',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtCod.Text);
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader SDR = SqlC.ExecuteReader();
                    if (SDR.Read())
                    {
                        TxtRef.Text = HttpUtility.HtmlDecode(SDR["CodReferencia"].ToString().Trim());
                        DdlPN.Text = HttpUtility.HtmlDecode(SDR["PN"].ToString().Trim());
                        TxtSN.Text = HttpUtility.HtmlDecode(SDR["Sn"].ToString().Trim());
                        ViewState["PNAntEle"] = DdlPN.Text.Trim();
                        ViewState["SNAntEle"] = TxtSN.Text.Trim();
                        TxtLote.Text = HttpUtility.HtmlDecode(SDR["NumLote"].ToString().Trim());
                        TxtDescr.Text = HttpUtility.HtmlDecode(SDR["Descripcion"].ToString().Trim());
                        TxtFecha = HttpUtility.HtmlDecode(SDR["FechaRecibo"].ToString().Trim());
                        if (!TxtFecha.Trim().Equals(""))
                        {
                            FechaD = Convert.ToDateTime(TxtFecha);
                            TxtFecRec.Text = String.Format("{0:yyyy-MM-dd}", FechaD);
                        }
                        else
                        {
                            TxtFecRec.Text = "";
                        }
                        TxtUndMed.Text = HttpUtility.HtmlDecode(SDR["CodUnidadMedida"].ToString().Trim());
                        DdlGrupo.Text = HttpUtility.HtmlDecode(SDR["CodGrupo"].ToString().Trim());
                        ViewState["GrupoEle"] = DdlGrupo.Text.Trim();
                        TxtAta.Text = HttpUtility.HtmlDecode(SDR["ATA"].ToString().Trim());
                        txtPosic.Text = HttpUtility.HtmlDecode(SDR["PosicionMotor"].ToString().Trim());
                        TxtHK.Text = HttpUtility.HtmlDecode(SDR["Aeronave"].ToString().Trim());
                        TxtMayor.Text = HttpUtility.HtmlDecode(SDR["Mayor"].ToString().Trim());
                        TxtUbiTec.Text = HttpUtility.HtmlDecode(SDR["CodUbicacionFisica"].ToString().Trim());
                        TxtFecha = HttpUtility.HtmlDecode(SDR["FechaShelfLife"].ToString().Trim());
                        if (!TxtFecha.Trim().Equals(""))
                        {
                            FechaD = Convert.ToDateTime(TxtFecha);
                            TxtFecShelfLife.Text = String.Format("{0:dd/MM/yyyy}", FechaD);
                        }
                        else
                        {
                            TxtFecShelfLife.Text = "";
                        }
                        TxtEstado.Text = HttpUtility.HtmlDecode(SDR["Estado"].ToString().Trim());
                        ViewState["FechaVenceE"] = HttpUtility.HtmlDecode(SDR["FechaVence"].ToString().Trim());
                        CkbApu.Checked = SDR["APU"].ToString().Trim().Equals("S") ? true : false;
                        CkbMot.Checked = SDR["Motor"].ToString().Trim().Equals("S") ? true : false;
                        CkbConsig.Checked = SDR["Consignacion"].ToString().Trim().Equals("S") ? true : false;
                        RdbActivo.Checked = SDR["Activo"].ToString().Trim().Equals("S") ? true : false;
                        RdbInactivo.Checked = SDR["Activo"].ToString().Trim().Equals("N") ? true : false;
                        ViewState["CodBodegaE"] = HttpUtility.HtmlDecode(SDR["CodBodega"].ToString().Trim());
                        ViewState["IdentificadorE"] = HttpUtility.HtmlDecode(SDR["Identificador"].ToString().Trim());
                        BIndDataCntdr(TxtCod.Text);
                    }
                }
            }
        }
        void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnModificar.Enabled = Md;
            BtnConsultar.Enabled = Otr;
            /* BtnIngresar.Enabled = In;
             BtnEliminar.Enabled = El;
             BtnInformes.Enabled = Otr;
             GrdMan.Enabled = Otr;
             GrdPN.Enabled = Otr;
             GrdCont.Enabled = Otr;
             BindDataAll(TxtCod.Text, "");*/
        }
        void AsignarValores()
        {
            Session["VldrElem"] = "S";

            if (DdlPN.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar el P/N')", true);
                Session["VldrElem"] = "N";
                return;
            }
            if (ViewState["IdentificadorE"].Equals("SN") && TxtSN.Text.Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una S/N')", true);
                Session["VldrElem"] = "N";
                return;
            }
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_Elemento 11,'{0}',@PN,@SN,'VALIDA',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtCod.Text);
                SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                SC.Parameters.AddWithValue("@PN", DdlPN.SelectedValue);
                SC.Parameters.AddWithValue("@SN", TxtSN.Text);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + SDR["Mensj"].ToString() + "')", true);
                    Session["VldrElem"] = "N";
                    return;
                }
            }
        }
        void BindDataDdl(string Accion)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbPrmtr = "";
                if (!Accion.Equals(""))
                {
                    VbPrmtr = TxtRef.Text;
                }
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Elemento 9,'{0}','','','PN',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", VbPrmtr);
                DdlPN.DataSource = Cnx.DSET(LtxtSql);
                DdlPN.DataMember = "Datos";
                DdlPN.DataTextField = "PN";
                DdlPN.DataValueField = "Codigo";
                DdlPN.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','GRU',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlGrupo.DataSource = Cnx.DSET(LtxtSql);
                DdlGrupo.DataMember = "Datos";
                DdlGrupo.DataTextField = "Descripcion";
                DdlGrupo.DataValueField = "CodTipoElemento";
                DdlGrupo.DataBind();
            }
        }
        void BIndDataBusq(string Prmtr)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql, VblOpc = "";
                VbTxtSql = "";
                if (RdbBusqPN.Checked == true)
                {
                    VblOpc = "PN";
                }
                if (RdbBusqDesc.Checked == true)
                {
                    VblOpc = "DESC";
                }
                if (RdbBusqRef.Checked == true)
                {
                    VblOpc = "REF";
                }
                if (RdbBusqSN.Checked == true)
                {
                    VblOpc = "SN";
                }
                VbTxtSql = string.Format("EXEC SP_PANTALLA_Elemento 8,@Prmtr,'','','{0}',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", VblOpc);
                if (!VbTxtSql.Equals("") && !VblOpc.Equals(""))
                {
                    sqlConB.Open();
                    SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB);
                    SC.Parameters.AddWithValue("@Prmtr", Prmtr.Trim());
                    SqlDataAdapter DAB = new SqlDataAdapter(SC);
                    DAB.SelectCommand = SC;
                    DAB.Fill(DtB);

                    if (DtB.Rows.Count > 0)
                    {
                        GrdBusq.DataSource = DtB;
                        GrdBusq.DataBind();
                    }
                    else
                    {
                        GrdBusq.DataSource = null;
                        GrdBusq.DataBind();
                    }
                }
            }
        }
        void BIndDataCntdr(string CodElem)
        {
            DataTable DtC = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = string.Format("EXEC SP_PANTALLA_Elemento 10,'{0}','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", CodElem);

                sqlConB.Open();
                SqlDataAdapter DAC = new SqlDataAdapter(VbTxtSql, sqlConB);
                DAC.Fill(DtC);

                if (DtC.Rows.Count > 0)
                {
                    GrdCont.DataSource = DtC;
                    GrdCont.DataBind();
                }
                else
                {
                    GrdCont.DataSource = null;
                    GrdCont.DataBind();
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            if (RdbInactivo.Checked == true)
            {
                if (DdlGrupo.SelectedValue.Equals("03"))
                {
                    string vlabee = ViewState["CodBodegaE"].ToString();
                    if (ViewState["CodBodegaE"].Equals("") || ViewState["CodBodegaE"].Equals("PREC-") || ViewState["CodBodegaE"].Equals("--") || ViewState["CodBodegaE"].Equals("Limbo") || ViewState["CodBodegaE"].Equals("BAJA"))
                    {

                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('El elemento se encuentra inactivo')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('El elemento se encuentra inactivo')", true);
                    return;
                }
            }
            if (BtnModificar.Text == "Modificar")
            {
                if (!ViewState["FechaVenceE"].Equals("S"))
                {
                    if (!ViewState["IdentificadorE"].Equals("SN"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Sólo aplica a elementos configurados con fecha de vencimiento')", true);
                        return;
                    }
                }
                TraerDatos();
                ActivarBotones(false, true, false, false, false);
                BtnModificar.Text = "Aceptar";
                ActivarCampos(false, true, "Modificar");
                BtnModificar.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
                BindDataDdl("UPDATE");
                DdlPN.Text = ViewState["PNAntEle"].ToString();
                DdlGrupo.Text = ViewState["GrupoEle"].ToString();
            }
            else
            {

                AsignarValores();
                if (Session["VldrElem"].ToString() == "N")
                {
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = string.Format("EXEC SP_TablasIngenieria 3,'{0}',@PN,@SN,'{1}','{2}','','','','',@Act,0,0,0,0,0,@FecSL,'02-01-1','03-01-1'",
                            TxtCod.Text, Session["C77U"].ToString(), TxtRef.Text);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@PN", DdlPN.SelectedValue);
                                SC.Parameters.AddWithValue("@SN", TxtSN.Text.Trim());
                                SC.Parameters.AddWithValue("@Act", RdbActivo.Checked == true ? 1 : 0);
                                SC.Parameters.AddWithValue("@FecSL", TxtFecShelfLife.Text);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
                                BtnModificar.Text = "Modificar";
                                ActivarBotones(true, true, true, true, true);
                                ActivarCampos(false, false, "");
                                BtnModificar.OnClientClick = "";
                                ViewState["PNAntEle"] = DdlPN.Text.Trim();
                                ViewState["SNAntEle"] = TxtSN.Text.Trim();
                                ViewState["GrupoEle"] = DdlGrupo.Text.Trim();
                                BIndDataCntdr(TxtCod.Text);
                                BindDataDdl("");
                                DdlPN.Text = ViewState["PNAntEle"].ToString();
                                DdlGrupo.Text = ViewState["GrupoEle"].ToString();
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmElemento", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                            }
                        }
                    }
                }
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            BIndDataBusq("77NEO");
            PnlCampos.Visible = false;
            PnlBusq.Visible = true;
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void IbtCerrar_Click(object sender, ImageClickEventArgs e)
        {
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
        }
        protected void IbtFechaI_Click(object sender, ImageClickEventArgs e)
        {

            /* BtnConsultar.Visible = false;
             Session["CalP"] = "I";
             if (TxtFecShelfLife.Text != String.Empty)
             {
                 if (TxtFecShelfLife.Text.Equals("1900-01-01"))
                 {
                     Calendar1.TodaysDate = DateTime.Today;
                 }
                 else { Calendar1.TodaysDate = Convert.ToDateTime(TxtFecShelfLife.Text); }
             }
             else
             {

                 Calendar1.TodaysDate = DateTime.Today;
             }

             if (Calendar1.Visible == false)
             {
                 Calendar1.Visible = true;
             }
             else
             {
                 Calendar1.Visible = false;
                 if ((int)ViewState["VblConsMS"] == 1)
                 {
                     BtnConsultar.Visible = true;
                 }
                 if ((int)ViewState["VblModMS"] == 1)
                 {
                     BtnModificar.Visible = true;
                 }
             }
             Calendar1.Attributes.Add("style", "position:absolute");*/
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            /* DateTime today = Calendar1.SelectedDate;

             string VbVcal = Session["CalP"].ToString();
             if (VbVcal == "I")
             {
                 TxtFecShelfLife.Text = String.Format("{0:yyyy-MM-dd}", today);
             }

             Calendar1.Visible = false;
             if ((int)ViewState["VblConsMS"] == 1)
             {
                 BtnConsultar.Visible = true;
             }
             if ((int)ViewState["VblModMS"] == 1)
             {
                 BtnModificar.Visible = true;
             }*/
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)

            {
                e.Row.Cells[1].Style.Value = "min-width:100px;";
                e.Row.Cells[2].Style.Value = "min-width:150px;";
                e.Row.Cells[3].Style.Value = "min-width:150px;";
                e.Row.Cells[4].Style.Value = "min-width:150px;";
                e.Row.Cells[5].Style.Value = "min-width:350px;";
                e.Row.Cells[10].Style.Value = "min-width:300px;";
            }
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtCod.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[22].Text);
                TraerDatos();
                PnlBusq.Visible = false;
                PnlCampos.Visible = true;
                ActivarBotones(true, true, true, true, true);
            }
            catch (Exception ex)
            {
                string VbMEns = ex.ToString();
                Response.Write(VbMEns);

                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + VbMEns + "')", true);
            }

        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusq.PageIndex = e.NewPageIndex;
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void Btborrar_Click(object sender, EventArgs e)
        {
            //TxtFecShelfLife.TextMode="Date";
        }
    }
}