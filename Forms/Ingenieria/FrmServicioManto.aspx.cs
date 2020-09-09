using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Runtime.InteropServices.WindowsRuntime;
using _77NeoWeb.Prg.PrgIngenieria;
using System.IO;
using ClosedXML.Excel;
using System.EnterpriseServices;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmServicioManto : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        private string Vbl3Desc, Vbl4Ruta, VBQuery, Vbl6Ext, Vbl8Type;
        private byte[] imagen;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            { Page.Title = string.Format("Servicio_Mantenimiento"); }
            else
            { Page.Title = string.Format("Reparaciones_Mayores"); }
            if (Session["C77U"] == null)
            {
                Session["VldrCntdr"] = "S";
                Session["C77U"] = "";/* */
                /*Session["C77U"] = "00000082";
                 Session["D[BX"] = "DbNeoDempV2";
                 Session["$VR"] = "77NEO01";
                 Session["V$U@"] = "sa";
                 Session["P@$"] = "admindemp";
                 Session["N77U"] = "UsuPrueba";
                 Session["Nit77Cia"] = "811035879-1";   */
            }
            if (!IsPostBack)
            {
                //IbtAdd.CssClass = "BtnImagenAdd";
                ModSeguridad();
                TipoPantalla();
                CorreccionDatos();
                ViewState["UCD"] = 0;
                ViewState["TIPO"] = "A";
                ViewState["IdCodElem"] = -1;
                ViewState["PN"] = "";
                ViewState["SN"] = "";
                ViewState["CodElem"] = "";
                BtnAK.Font.Bold = true;
                BtnAK.Font.Size = 14;
                BindDDdlBusq("");
                BindDDdl();
                BindDAK();
                BindDataAll();
                GrdAeron.Visible = true;
                // ActivarCampos(false, false);
                // BindDataDdlCntr();
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                IbtAdd.Visible = false;
                GrdAeron.ShowFooter = false;
                GrdPN.ShowFooter = false;
                GrdHKAsig.ShowFooter = false;
                GrdAdj.ShowFooter = false;
                GrdRecursoF.ShowFooter = false;
                GrdLicen.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                IbtUpdate.Visible = false;
                IbtGenerOT.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
                IbtFind.Visible = false;
            }
            if (ClsP.GetImprimir() == 0)
            {
                IbtPrint.Visible = false;
            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                IbtDelete.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {// opcion de visualizar status
                ViewState["CE1"] = 0;
                CkbVisuStat.Visible = false;
            }
            if (ClsP.GetCE2() == 0)
            {

            }
            if (ClsP.GetCE3() == 0)
            {// Asignar aeronaves
                ViewState["CE3"] = 0;
            }
            if (ClsP.GetCE4() == 0)
            {// cambiar etapa actual
                ViewState["CE4"] = 0;
            }
            if (ClsP.GetCE5() == 0)
            {
                ViewState["CE5"] = 0;
                IbtRecurso.Visible = false;
            }
            if (ClsP.GetCE6() == 0)
            {
                ViewState["CE6"] = 0;
                CkbBloqRec.Visible = false;
            }
            /*Cnx.SelecBD();
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
            }*/
        }
        protected void PerfilesGrid()
        {
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
                        Row.Cells[11].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[11].Controls.Remove(imgD);
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
                        Row.Cells[6].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[6].Controls.Remove(imgD);
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
            foreach (GridViewRow Row in GrdHKAsig.Rows)
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
            if ((int)ViewState["CE3"] == 0)
            {
                foreach (GridViewRow Row in GrdHKAsig.Rows)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        imgD.ToolTip = "No tiene permiso";
                    }
                }
                GrdHKAsig.ShowFooter = false;
            }
            foreach (GridViewRow Row in GrdRecursoF.Rows)
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
            if (CkbBloqRec.Checked == true)
            {
                foreach (GridViewRow Row in GrdRecursoF.Rows)
                {

                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgE != null)
                    {
                        imgE.Enabled = false;
                        imgE.ToolTip = "El recurso se encuentra bloqueado";
                        imgD.Enabled = false;
                        imgD.ToolTip = "El recurso se encuentra bloqueado";
                    }

                }
                GrdRecursoF.FooterRow.Enabled = false;
                GrdRecursoF.FooterRow.ToolTip = "El recurso se encuentra bloqueado";
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
            }
            TxtHistorico.Enabled = false;
            TxtHistorico.Text = "";
        }
        private decimal LRemanente, LRemanente1, LremanenteDia, LremanenteDia1, LCorridoDias, LCorridoDias1, LCorrido, LCorrido1;
        protected void Cumplimiento(int Id, decimal Ext, decimal ExtDia)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 24,'','','','WEB',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", Id);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
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

                    if (LCorrido > LCorridoDias) // Si el porcentaje de corrido el servicio es mayor el valor que en dias
                    {
                        if (LCorrido > 100)
                        { LblCumplimi.Text = " Cump: 100%"; }
                        else
                        { LblCumplimi.Text = " Cump: " + Convert.ToString(LCorrido) + "%"; }
                    }
                    else
                    {
                        if (LCorridoDias > 100)
                        { LblCumplimi.Text = " Cump: 100%"; }
                        else
                        { LblCumplimi.Text = " Cump: " + Convert.ToString(LCorridoDias) + "%"; }
                    }
                    UpPnlCampos.Update();
                }
            }
        }
        protected void TipoPantalla()
        {
            try
            {
                if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
                { TitForm.Text = "Configuración Servicio de Mantenimiento"; }
                else
                {
                    TitForm.Text = "Configuración Reparaciones Mayores";
                    LblCumplimi.Visible = false;
                    TxtHoriz.Visible = false;
                    TxtMatric.Visible = false;
                    TxtEtapa.Visible = false;
                    TxtActual.Visible = false;
                    CkbAD.Visible = false;
                    CkbSB.Visible = false;
                    CkbAplSub.Visible = false;
                    CkbVisuStat.Visible = false;
                    TxtSubAta.Visible = false;
                    TxtConsAta.Visible = false;
                    DdlTipo.Visible = false;
                    TxtRefOT.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "TipoPantalla", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void CorreccionDatos()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 14,'','','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                {
                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "CorreccionDatos", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                    }
                }
            }
        }
        protected void EstadoOT(int Id)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 25,'','','','WEB',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", Id);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtEstadoOT.Text = SDR["Mensj"].ToString();
                }
            }
        }
        protected void BindDDdlBusq(string Tipo)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", Tipo, Session["PllaSrvManto"].ToString());
            DdlBusq.DataSource = Cnx.DSET(LtxtSql);
            DdlBusq.DataMember = "Datos";
            DdlBusq.DataTextField = "Descripcion";
            DdlBusq.DataValueField = "IdSrvManto";
            DdlBusq.DataBind();
        }
        protected void BindDDdl()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PM',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlGrupo.DataSource = Cnx.DSET(LtxtSql);
            DdlGrupo.DataMember = "Datos";
            DdlGrupo.DataTextField = "Descripcion";
            DdlGrupo.DataValueField = "CodPatronManto";

            DdlGrupo.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','MOD',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlModel.DataSource = Cnx.DSET(LtxtSql);
            DdlModel.DataMember = "Datos";
            DdlModel.DataTextField = "NomModelo";
            DdlModel.DataValueField = "CodModelo";
            DdlModel.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','TAL',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            Ddltaller.DataSource = Cnx.DSET(LtxtSql);
            Ddltaller.DataMember = "Datos";
            Ddltaller.DataTextField = "NomTaller";
            Ddltaller.DataValueField = "CodTaller";
            Ddltaller.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','ATA',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlAta.DataSource = Cnx.DSET(LtxtSql);
            DdlAta.DataMember = "Datos";
            DdlAta.DataTextField = "Descripcion";
            DdlAta.DataValueField = "CodCapitulo";
            DdlAta.DataBind();
            LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','TIP',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            DdlTipo.DataSource = Cnx.DSET(LtxtSql);
            DdlTipo.DataMember = "Datos";
            DdlTipo.DataTextField = "NomTipoSrv";
            DdlTipo.DataValueField = "IdTipoSrv";
            DdlTipo.DataBind();
        }
        protected void BindDTraerdatos(string Prmtr)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 4,'','','','','',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", Prmtr);
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader SDR = SqlC.ExecuteReader();
                    if (SDR.Read())
                    {
                        CkbVisuStat.Checked = HttpUtility.HtmlDecode(SDR["VisualizarStatus"].ToString().Trim()) == "S" ? true : false;
                        TxtId.Text = SDR["IdSrvManto"].ToString();
                        TxtCod.Text = HttpUtility.HtmlDecode(SDR["CodServicioManto"].ToString().Trim());
                        TxtDesc.Text = HttpUtility.HtmlDecode(SDR["Servicio"].ToString().Trim());
                        TxtHoriz.Text = SDR["HorizonteApertura"].ToString();
                        DdlGrupo.SelectedValue = SDR["CodPatronManto"].ToString().Trim();
                        if (DdlGrupo.SelectedValue.Trim().Equals("UCD"))
                        {
                            ViewState["UCD"] = 1;
                        }
                        else
                        {
                            ViewState["UCD"] = 0;
                        }
                        TxtEtapa.Text = SDR["NroEtapas"].ToString();
                        TxtActual.Text = SDR["EtapaActual"].ToString();
                        TxtDoc.Text = HttpUtility.HtmlDecode(SDR["Nrodocumento"].ToString().Trim());
                        TxtRefOT.Text = HttpUtility.HtmlDecode(SDR["Referencia"].ToString().Trim());
                        DdlModel.Text = HttpUtility.HtmlDecode(SDR["CodModeloSM"].ToString().Trim());
                        Ddltaller.Text = HttpUtility.HtmlDecode(SDR["CodTaller"].ToString().Trim());
                        CkbAD.Checked = HttpUtility.HtmlDecode(SDR["AD"].ToString().Trim()) == "S" ? true : false;
                        CkbSB.Checked = HttpUtility.HtmlDecode(SDR["SB"].ToString().Trim()) == "S" ? true : false;
                        CkbAplSub.Checked = HttpUtility.HtmlDecode(SDR["SubComponenteSM"].ToString().Trim()) == "S" ? true : false;
                        DdlAta.Text = HttpUtility.HtmlDecode(SDR["CodCapitulo"].ToString().Trim());
                        TxtSubAta.Text = SDR["SubAta"].ToString();
                        TxtConsAta.Text = SDR["ConsecutivoAta"].ToString();
                        DdlTipo.Text = HttpUtility.HtmlDecode(SDR["IdTipoSrv"].ToString().Trim());
                        CkbBloqRec.Checked = HttpUtility.HtmlDecode(SDR["ValidarRecurso"].ToString().Trim()) == "S" ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {

                string VbMEns = ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + VbMEns + "')", true);
            }
        }
        protected void BindDAK()
        {
            try
            {
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 5,'{0}','','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtCod.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        GrdAeron.DataSource = DT;
                        GrdAeron.DataBind();
                    }
                    else
                    {
                        DT.Rows.Add(DT.NewRow());
                        GrdAeron.DataSource = DT;
                        GrdAeron.DataBind();
                        GrdAeron.Rows[0].Cells.Clear();
                        GrdAeron.Rows[0].Cells.Add(new TableCell());
                        // GrdAeron.Rows[0].Cells[0].ColumnSpan = DT.Columns.Count;
                        GrdAeron.Rows[0].Cells[0].Text = "Sin datos..!";
                        GrdAeron.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDAK", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BindDPN()
        {
            try
            {
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 6,'{0}','','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtCod.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        GrdPN.DataSource = DT;
                        GrdPN.DataBind();

                    }
                    else
                    {
                        DT.Rows.Add(DT.NewRow());
                        GrdPN.DataSource = DT;
                        GrdPN.DataBind();
                        GrdPN.Rows[0].Cells.Clear();
                        GrdPN.Rows[0].Cells.Add(new TableCell());
                        GrdPN.Rows[0].Cells[0].Text = "Sin datos..!";
                        GrdPN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDAK", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BindDSN()
        {
            try
            {
                DataTable DT = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 11,'{0}','','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtCod.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        GrdSN.DataSource = DT;
                        GrdSN.DataBind();
                    }
                    else
                    {
                        //DT.Rows.Add(DT.NewRow());
                        GrdSN.DataSource = DT;
                        GrdSN.DataBind();
                        //GrdSN.Rows[0].Cells.Clear();
                        //GrdSN.Rows[0].Cells.Add(new TableCell());
                        //GrdSN.Rows[0].Cells[0].Text = "Sin series asignadas..!";
                        //GrdSN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDSN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BindDHKAsig()
        {
            try
            {
                DataTable DTHA = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 17,'','','WEB',{0}, 0, 0,'01-01-1900','01-01-1900'", TxtId.Text.Equals("") ? "0" : TxtId.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDAHA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDAHA.Fill(DTHA);
                    if (DTHA.Rows.Count > 0)
                    {
                        GrdHKAsig.DataSource = DTHA;
                        GrdHKAsig.DataBind();
                    }
                    else
                    {
                        DTHA.Rows.Add(DTHA.NewRow());
                        GrdHKAsig.DataSource = DTHA;
                        GrdHKAsig.DataBind();
                        GrdHKAsig.Rows[0].Cells.Clear();
                        GrdHKAsig.Rows[0].Cells.Add(new TableCell());
                        GrdHKAsig.Rows[0].Cells[0].Text = "Empty..!";
                        GrdHKAsig.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDHKAsig", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BindDAdjunto()
        {
            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 28,'DOCINGENIERIA','{0}','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtCod.Text);
                sqlCon.Open();
                SqlDataAdapter SDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SDA.Fill(DT);
                if (DT.Rows.Count > 0)
                {
                    GrdAdj.DataSource = DT;
                    GrdAdj.DataBind();
                }
                else
                {
                    DT.Rows.Add(DT.NewRow());
                    GrdAdj.DataSource = DT;
                    GrdAdj.DataBind();
                    GrdAdj.Rows[0].Cells.Clear();
                    GrdAdj.Rows[0].Cells.Add(new TableCell());
                    GrdAdj.Rows[0].Cells[0].Text = "Sin datos..!";
                    GrdAdj.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            } /**/
        }
        protected void BindDataAll()
        {
            BindDHKAsig();
            BindDAdjunto();
            PerfilesGrid();
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            if (!ViewState["TIPO"].ToString().Equals("S"))
            {
                IbtAdd.Enabled = In;
            }
            IbtUpdate.Enabled = Md;
            IbtDelete.Enabled = El;
            IbtFind.Enabled = Otr;
            IbtPrint.Enabled = Ip;
            IbtRecurso.Enabled = Otr;
            IbtGenerOT.Enabled = Otr;
            BtnAK.Enabled = Otr;
            BtnPN.Enabled = Otr;
            BtnSN.Enabled = Otr;
            GrdAeron.Enabled = Otr;
            GrdPN.Enabled = Otr;
            GrdSN.Enabled = Otr;
            GrdHKAsig.Enabled = Otr;
            GrdAdj.Enabled = Otr;
            // BindDataAll();
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            TxtDesc.Enabled = Edi;
            DdlGrupo.Enabled = Ing;
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            {
                CkbVisuStat.Enabled = Edi;
                TxtHoriz.Enabled = Edi;
                TxtRefOT.Enabled = Edi;
                CkbAD.Enabled = Edi;
                CkbSB.Enabled = Edi;
                if (!ViewState["TIPO"].ToString().Equals("A"))
                { CkbAplSub.Enabled = Edi; }
                else
                {
                    if (DdlGrupo.SelectedValue.Trim().Equals("SVC"))
                    {
                        TxtEtapa.Enabled = Edi;
                        TxtActual.Enabled = (int)ViewState["CE4"] == 0 ? false : Edi;
                    }
                }
                TxtSubAta.Enabled = Edi;
                TxtConsAta.Enabled = Edi;
                DdlTipo.Enabled = Edi;
            }
            TxtDoc.Enabled = Edi;
            DdlModel.Enabled = Edi;
            Ddltaller.Enabled = Edi;
            DdlAta.Enabled = Edi;
            DdlBusq.Enabled = Edi == true ? false : true;
            CkbBloqRec.Enabled = Edi;
        }
        protected void LimpiarCampos()
        {
            TxtId.Text = "";
            TxtCod.Text = "";
            TxtDesc.Text = "";
            TxtHoriz.Text = "";
            DdlGrupo.Text = "";
            TxtEtapa.Text = "";
            TxtActual.Text = "";
            TxtDoc.Text = "";
            DdlAta.Text = "";
            TxtRefOT.Text = "";
            DdlModel.Text = "";
            Ddltaller.Text = "";
            CkbAD.Checked = false;
            CkbSB.Checked = false;
            CkbAplSub.Checked = false;
            CkbVisuStat.Checked = false;
            DdlAta.Text = "";
            TxtSubAta.Text = "";
            TxtConsAta.Text = "";
            DdlTipo.Text = "0";
            TxtEstadoOT.Text = "";
            TxtMatric.Text = "";
        }
        protected void ValidarSvcManto(string Accion)
        {
            try
            {
                ViewState["Validar"] = "S";
                if (TxtDesc.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una descripción')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (DdlGrupo.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un grupo')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Inconvenientes con la validación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "ValidarSvcManto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void ValidarHK(string Accion)
        {
            try
            {
                ViewState["Validar"] = "S";
                string VBQuery;

                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["CodHK"].ToString().Trim().Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una aeronave')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un contador')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (TxtHistorico.Enabled == true && TxtHistorico.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["FrecIni"].ToString().Trim().Equals("0") && ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 7,'{0}','{3}','','','HK',{1},{2},{4},0,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], ViewState["CodHK"], TxtCod.Text, ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + DAE["Mensj"].ToString() + "')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en la validación en el ingreso detalle aeronave')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarHK", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void ValidarDetPN(string Accion)
        {
            try
            {
                ViewState["Validar"] = "S";
                string VBQuery;

                if (Accion.Equals("INSERT"))
                {
                    if (ViewState["PN"].ToString().Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un P/N')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                    if (ViewState["Cntdr"].ToString().Trim().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un contador')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
                if (ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 9,'{0}','{2}','{3}','','VALIDA',{1},{4},0,0,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], ViewState["PN"], TxtCod.Text, ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + DAE["Mensj"].ToString() + "')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en la validación en el ingreso detalle aeronave')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidarPN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void ValidaDetSN()
        {
            try
            {
                ViewState["Validar"] = "S";
                string VBQuery;
                if (TxtHistorico.Enabled == true && ViewState["Historico"].ToString().Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar la descripción del histórico')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                if (ViewState["FrecIni"].ToString().Trim().Equals("0") && ViewState["Frec"].ToString().Trim().Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una frecuencia')", true);
                    ViewState["Validar"] = "N";
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 7,'{0}','{2}','{3}','','SN',{1},{4},0,0,'01-01-01','01-01-01','01-01-01'",
                        ViewState["Cntdr"], ViewState["Reset"], TxtCod.Text, ViewState["CodElem"], ViewState["Frec"]);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + DAE["Mensj"].ToString() + "')", true);
                        ViewState["Validar"] = "N";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en la validación en el ingreso detalle aeronave')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "ValidaDetSN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        {
            BindDTraerdatos(DdlBusq.SelectedValue);
            UpPnlCampos.Update();
            switch (ViewState["TIPO"].ToString())
            {
                case "A":
                    BindDAK();
                    break;
                case "P":
                    BindDPN();
                    break;
                default:
                    BindDSN();
                    break;
            }
            BindDataAll();
            UpPnlPN.Update();
            PerfilesGrid();
        }
        protected void DdlGrupo_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            ViewState["UCM"] = 0;
            if (Session["PllaSrvManto"].ToString().Equals("SERVICIO"))
            {
                if (DdlGrupo.SelectedValue.Trim().Equals("SVC") && GrdAeron.Visible == true)
                {
                    TxtEtapa.Enabled = true;
                    TxtActual.Enabled = true;
                }
                else
                {
                    TxtEtapa.Enabled = false;
                    TxtActual.Enabled = false;
                    TxtEtapa.Text = "0";
                    TxtActual.Text = "0";
                }
            }
        }
        protected void DdlHKPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlHKPP = (GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','','','','CON',{1},0,0,0,'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlHKPP.SelectedValue);
            DropDownList DdlContHKPP = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList);
            DdlContHKPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContHKPP.DataTextField = "CodContador";
            DdlContHKPP.DataValueField = "Cod";
            DdlContHKPP.DataBind();
            return;
        }
        protected void DdlContHKPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlContHKPP = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList);
            TextBox TxtNumDiaPP = (GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox);
            TextBox TxtExtDiaPP = (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox);
            TxtNumDiaPP.Enabled = true;
            TxtExtDiaPP.Enabled = true;

            if (DdlContHKPP.SelectedValue.Trim().Equals("CAL") || DdlContHKPP.SelectedValue.Trim().Equals("CTI"))
            {
                TxtNumDiaPP.Enabled = false;
                TxtNumDiaPP.Text = "0";
                TxtExtDiaPP.Enabled = false;
                TxtExtDiaPP.Text = "0";
            }
        }
        protected void DdlPNPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlPNPP = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList);
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','CONPN',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtCod.Text, DdlPNPP.SelectedValue);
            DropDownList DdlContPNPP = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList);
            DdlContPNPP.DataSource = Cnx.DSET(LtxtSql);
            DdlContPNPP.DataTextField = "CodContador";
            DdlContPNPP.DataValueField = "Cod";
            DdlContPNPP.DataBind();
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 9,'{0}','','','','DescPN',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlPNPP.SelectedValue);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    (GrdPN.FooterRow.FindControl("TxtDescPnPP") as TextBox).Text = SDR["Descripcion"].ToString();
                }
            }
            return;
        }
        protected void DdlContPNPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            DropDownList DdlContPNPP = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList);
            TextBox TxtNumDiaPNPP = (GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox);
            TxtNumDiaPNPP.Enabled = true;

            if (DdlContPNPP.SelectedValue.Trim().Equals("CAL") || DdlContPNPP.SelectedValue.Trim().Equals("CTI"))
            {
                TxtNumDiaPNPP.Enabled = false;
                TxtNumDiaPNPP.Text = "0";
            }
        }
        protected void BtnAK_Click(object sender, EventArgs e)
        {
            BtnAK.Font.Bold = true;
            BtnAK.Font.Size = 15;
            ViewState["TIPO"] = "A";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            BindDDdlBusq("");
            BtnPN.Font.Bold = false;
            BtnPN.Font.Size = 13;
            BtnSN.Font.Bold = false;
            BtnSN.Font.Size = 13;
            GrdAeron.Visible = true;
            GrdPN.Visible = false;
            GrdSN.Visible = false;
            GrdHKAsig.Visible = false;
            IbtAdd.Enabled = true;
            LimpiarCampos();
            BindDAK();
            BindDataAll();
            PerfilesGrid();
        }
        protected void BtnPN_Click(object sender, EventArgs e)
        {
            BtnPN.Font.Bold = true;
            BtnPN.Font.Size = 15;
            ViewState["TIPO"] = "P";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            BindDDdlBusq(ViewState["TIPO"].ToString());
            BtnAK.Font.Bold = false;
            BtnAK.Font.Size = 13;
            BtnSN.Font.Bold = false;
            BtnSN.Font.Size = 13;
            GrdAeron.Visible = false;
            GrdPN.Visible = true;
            GrdSN.Visible = false;
            GrdHKAsig.Visible = true;
            IbtAdd.Enabled = true;
            LimpiarCampos();
            BindDPN();
            BindDataAll();
            PerfilesGrid();
        }
        protected void BtnSN_Click(object sender, EventArgs e)
        {
            BtnSN.Font.Bold = true;
            BtnSN.Font.Size = 15;
            ViewState["TIPO"] = "S";
            ViewState["PN"] = "";
            ViewState["SN"] = "";
            BindDDdlBusq("P");
            BtnPN.Font.Bold = false;
            BtnPN.Font.Size = 13;
            BtnAK.Font.Bold = false;
            BtnAK.Font.Size = 13;
            GrdAeron.Visible = false;
            GrdPN.Visible = false;
            GrdSN.Visible = true;
            GrdHKAsig.Visible = true;
            IbtAdd.Enabled = false;
            LimpiarCampos();
            BindDSN();
            BindDataAll();
            PerfilesGrid();
        }
        protected void IbtAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (IbtAdd.ToolTip == "Ingresar")
            {
                IbtAdd.ImageUrl = "~/images/SaveV2.png";
                ActivarBotones(true, false, false, false, false);
                IbtAdd.ToolTip = "Aceptar";
                ActivarCampos(true, true, "Ingresar");
                LimpiarCampos();
                BindDataAll();
                BindDAK();
                BindDPN();
                BindDSN();
                DdlBusq.SelectedValue = "0";
                IbtAdd.OnClientClick = "return confirm('¿Desea realizar el ingreso?');";
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
                        NroEtapas = TxtEtapa.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtEtapa.Text),
                        EtapaActual = TxtActual.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtActual.Text),
                        SubAta = TxtSubAta.Text.Trim(),
                        ConsecutivoAta = TxtConsAta.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtConsAta.Text),
                        IdTipoSrv = Convert.ToInt32(DdlTipo.SelectedValue),
                        AD = CkbAD.Checked == true ? 1 : 0,
                        SB = CkbSB.Checked == true ? 1 : 0,
                        HorizonteApertura = TxtHoriz.Text.Trim().Equals("") ? 0 : Convert.ToDouble(TxtHoriz.Text),
                        Referencia = TxtRefOT.Text.Trim(),
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = CkbAplSub.Checked == true ? 1 : 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = Session["PllaSrvManto"].ToString(),
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = CkbVisuStat.Checked == true ? 1 : 0,
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
                        string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 16,'{0}','','','','',{1},0,0,0,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), VblIdSvcManto);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Servicio", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                    IbtAdd.ImageUrl = "~/images/AddNew.png";
                    IbtAdd.ToolTip = "Ingresar";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    IbtAdd.OnClientClick = "";
                    BindDTraerdatos(VblIdSvcManto.ToString());
                    switch (ViewState["TIPO"].ToString())
                    {
                        case "A":
                            BindDAK();
                            break;
                        case "P":
                        default:
                            BindDPN();
                            break;
                    }
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtUpdate_Click(object sender, ImageClickEventArgs e)
        {
            if (IbtUpdate.ToolTip == "Modificar")
            {
                if (!TxtCod.Text.Trim().Equals(""))
                {
                    IbtUpdate.ImageUrl = "~/images/SaveV2.png";
                    ActivarBotones(false, true, false, false, false);
                    IbtUpdate.ToolTip = "Aceptar";
                    ActivarCampos(false, true, "Modificar");
                    //LimpiarCampos();
                    IbtUpdate.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
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
                        NroEtapas = TxtEtapa.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtEtapa.Text),
                        EtapaActual = TxtActual.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtActual.Text),
                        SubAta = TxtSubAta.Text.Trim(),
                        ConsecutivoAta = TxtConsAta.Text.Trim().Equals("") ? 0 : Convert.ToInt32(TxtConsAta.Text),
                        IdTipoSrv = Convert.ToInt32(DdlTipo.SelectedValue),
                        AD = CkbAD.Checked == true ? 1 : 0,
                        SB = CkbSB.Checked == true ? 1 : 0,
                        HorizonteApertura = TxtHoriz.Text.Trim().Equals("") ? 0 : Convert.ToDouble(TxtHoriz.Text),
                        Referencia = TxtRefOT.Text.Trim(),
                        CodModeloSM = DdlModel.SelectedValue.Trim(),
                        PnMayor = "",
                        SubComponenteSM = CkbAplSub.Checked == true ? 1 : 0,
                        CodTaller = Ddltaller.SelectedValue.Trim(),
                        CodReferenciaSrv = "",
                        Catalogo = Session["PllaSrvManto"].ToString(),
                        ValidarRecurso = CkbBloqRec.Checked == true ? 1 : 0,
                        VisualizarStatus = CkbVisuStat.Checked == true ? 1 : 0,
                        ServicioMayor = "",
                        Accion = "UPDATE",
                        Aplicabilidad = ViewState["TIPO"].ToString(),
                    };
                    ObjTSM.Add(detail);
                    CsTypeServicioManto TblServicioManto = new CsTypeServicioManto();
                    TblServicioManto.Alimentar(ObjTSM);
                    //IbtUpdate.CssClass = "BtnImagenUpdate";
                    IbtUpdate.ImageUrl = "~/images/Edit.png";
                    IbtUpdate.ToolTip = "Modificar";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    IbtUpdate.OnClientClick = "";
                    BindDataAll();
                }
                catch (Exception Ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
        protected void IbtFind_Click(object sender, ImageClickEventArgs e)
        {
            PnlCampos.Visible = false;
            PnlBusq.Visible = true;

            if (ViewState["TIPO"].ToString().Equals("A"))
            {
                TblBusqHK.Visible = true;
                TblBusqPN.Visible = false;
                TblBusqSN.Visible = false;
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
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void IbtPrint_Click(object sender, ImageClickEventArgs e)
        {
            switch (ViewState["TIPO"].ToString())
            {
                case "A":
                    if (TxtMatric.Text.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un detalle')", true);
                        return;
                    }
                    TitInfSvc.Text = "Informes de servicios " + TxtMatric.Text;
                    break;
                case "P":
                    if (ViewState["PN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un detalle')", true);
                        return;
                    }
                    TitInfSvc.Text = "Informes de servicios " + ViewState["PN"].ToString();
                    break;
                default:
                    if (ViewState["SN"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un detalle')", true);
                        return;
                    }
                    TitInfSvc.Text = "Informes de servicios " + ViewState["PN"].ToString() + " | " + ViewState["SN"].ToString();
                    break;
            }
            PnlCampos.Visible = false;
            PnlInforme.Visible = true;
        }
        protected void IbtDelete_Click(object sender, ImageClickEventArgs e)
        {
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 6,'{0}','{1}','{2}','{3}','{4}','','','','',{5},0,0,0,0,0,'01-01-1','02-01-1','03-01-1'	",
                            TxtCod.Text, TxtDesc.Text.Trim(), ViewState["TIPO"], Session["PllaSrvManto"], Session["C77U"].ToString(), TxtId.Text);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }

                                Transac.Commit();
                                BindDataAll();
                                BIndDataBusq(ViewState["TIPO"].ToString());
                                LimpiarCampos();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR  SRV MANTO", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void IbtRecurso_Click(object sender, ImageClickEventArgs e)
        {
            if (!TxtId.Text.Trim().Equals(""))
            {
                BindDRecursoF();
                BindDLicencia();
                PnlCampos.Visible = false;
                PnlRecursos.Visible = true;
            }
        }
        protected void IbtGenerOT_Click(object sender, ImageClickEventArgs e)
        {
            PerfilesGrid();
            if (TxtId.Text.Trim().Equals(""))
            {
                return;
            }
            if (!ViewState["TIPO"].ToString().Equals("P"))
            {
                if (TxtMatric.Text.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un registro del detalle para obtener la matrícula')", true);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 7,'{0}','{1}','{2}','{3}','{4}','','','','',{5},0,0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                            TxtMatric.Text, DdlGrupo.Text.Trim(), TxtCod.Text, ViewState["TIPO"], Session["PllaSrvManto"], ViewState["IdCodElem"]);
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de generación orden de trabajo')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "GENERAR OT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdAeron_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();

                if (e.CommandName.Equals("AddNew"))
                {
                    while ((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Contains("-"))
                    {
                        (GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text = (GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Replace("-", "");
                    }
                    if ((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Ext"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["Ext"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtExtPP") as TextBox).Text.Trim()) * -1;
                    }

                    while ((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Contains("-"))
                    {
                        (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text = (GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Replace("-", "");
                    }
                    if ((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["ExtDia"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["ExtDia"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtExtDiaPP") as TextBox).Text.Trim()) * -1;
                    }

                    ViewState["CodHK"] = Convert.ToInt32((GrdAeron.FooterRow.FindControl("DdlHKPP") as DropDownList).SelectedValue.Trim());
                    ViewState["Cntdr"] = (GrdAeron.FooterRow.FindControl("DdlContHKPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Reset"] = (GrdAeron.FooterRow.FindControl("CkbResetPP") as CheckBox).Checked == true ? 1 : 0;

                    if ((GrdAeron.FooterRow.FindControl("TxtFrecIniPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["FrecIni"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["FrecIni"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtFrecIniPP") as TextBox).Text.Trim());
                    }

                    if ((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Frec"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["Frec"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtFrecPP") as TextBox).Text.Trim());
                    }

                    if ((GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["NroDia"] = Convert.ToDouble(0);
                    }
                    else
                    {
                        ViewState["NroDia"] = Convert.ToDouble((GrdAeron.FooterRow.FindControl("TxtNumDiaPP") as TextBox).Text.Trim());
                    }

                    if (!(GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["FechaVenc"] = Convert.ToDateTime((GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim());
                        DateTime borrar = (DateTime)ViewState["FechaVenc"];
                    }
                    // validar
                    ValidarHK("INSERT");
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
                            CodIdContaSrvManto = 0,
                            CodAeronave = (int)ViewState["CodHK"],
                            CodElemento = null,
                            CodServicioManto = TxtCod.Text.Trim(),
                            Frecuencia = (double)ViewState["Frec"],
                            Extension = (double)ViewState["Ext"],
                            FechaVencimiento = (GrdAeron.FooterRow.FindControl("TxtFecVenPP") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                            NroDias = (double)ViewState["NroDia"],
                            ExtensionDias = (double)ViewState["ExtDia"],
                            BanOrdenTrabajo = 0,
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            CodOt = null,
                            Compensacion = 0,
                            Resetear = (int)ViewState["Reset"],
                            FrecuenciaInicial = (double)ViewState["FrecIni"],
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
                        BindDAK();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET AERONAVE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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

                        Label ext = Row.FindControl("LblExt") as Label;
                        Label extDia = Row.FindControl("LblExtDia") as Label;
                        if (ext != null)
                        {
                            decimal VbExt = Convert.ToDecimal(ext.Text);
                            decimal VbExtD = Convert.ToDecimal(extDia.Text);
                            int VbID = Convert.ToInt32(GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][0].ToString());
                            TxtMatric.Text = GrdAeron.DataKeys[this.GrdAeron.SelectedIndex][1].ToString();
                            Cumplimiento(VbID, VbExt, VbExtD);
                            EstadoOT(VbID);
                        }
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0)
                        {
                            Row.Style["background-color"] = "white";
                        }
                        else
                        {
                            Row.Style["background-color"] = "#cae4ff";
                        }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + Row.RowIndex);

                    }
                    // PerfilesGrid();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SELECT DET AERONAVE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdAeron.EditIndex = e.NewEditIndex;
            BindDataAll();
            BindDAK();
        }
        protected void GrdAeron_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());
                if ((GrdAeron.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true)
                { TxtHistorico.Enabled = true; }
                else
                { TxtHistorico.Enabled = false; TxtHistorico.Text = ""; }
                while ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text = (GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Ext"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Ext"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim()) * -1;
                }
                while ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text = (GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["ExtDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["ExtDia"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim()) * -1;
                }
                ViewState["CodHK"] = Convert.ToInt32((GrdAeron.Rows[e.RowIndex].FindControl("DdlHK") as DropDownList).SelectedValue.Trim());
                ViewState["Cntdr"] = (GrdAeron.Rows[e.RowIndex].FindControl("DdlCont") as DropDownList).SelectedValue.Trim();
                ViewState["Reset"] = (GrdAeron.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FrecIni"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["FrecIni"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim());
                }

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Frec"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Frec"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim());
                }

                if ((GrdAeron.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["NroDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["NroDia"] = Convert.ToDouble((GrdAeron.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim());
                }

                if (!(GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FechaVenc"] = Convert.ToDateTime((GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim());
                }
                // validar
                ValidarHK("UPDATE");
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
                        CodAeronave = (int)ViewState["CodHK"],
                        CodElemento = null,
                        CodServicioManto = TxtCod.Text.Trim(),
                        Frecuencia = (double)ViewState["Frec"],
                        Extension = (double)ViewState["Ext"],
                        FechaVencimiento = (GrdAeron.Rows[e.RowIndex].FindControl("TxtFecVen") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                        NroDias = (double)ViewState["NroDia"],
                        ExtensionDias = (double)ViewState["ExtDia"],
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = (int)ViewState["Reset"],
                        FrecuenciaInicial = (double)ViewState["FrecIni"],
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = "",
                        PN = "",
                        Accion = "UPDATE",
                        Aplicabilidad = "HK",
                        CrearHistorico = (GrdAeron.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true ? "S" : "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdAeron.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDataAll();
                    BindDAK();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET AERONAVE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdAeron.EditIndex = -1;
            BindDAK(); ;
        }
        protected void GrdAeron_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdAeron.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 8,'','','','','VALIDA',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", IDContaSrvManto);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 8,'','','','{0}','DELETE',{1},0,0,0,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), IDContaSrvManto);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDAK();
                                BindDataAll();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET AERONAVE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdAeron_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                string LtxtSql = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','{1}','','','HK',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlModel.SelectedValue, TxtCod.Text);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlHKPP = (e.Row.FindControl("DdlHKPP") as DropDownList);
                    DdlHKPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlHKPP.DataTextField = "Matricula";
                    DdlHKPP.DataValueField = "CodAeronave";
                    DdlHKPP.DataBind();
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrecI = (e.Row.FindControl("TxtFrecIniPP") as TextBox);
                        TxtFrecI.ReadOnly = true;
                        TxtFrecI.Enabled = false;
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPP") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtExt = (e.Row.FindControl("TxtExtPP") as TextBox);
                        TxtExt.ReadOnly = true;
                        TxtExt.Enabled = false;
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPP") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        TextBox TxtED = (e.Row.FindControl("TxtExtDiaPP") as TextBox);
                        TxtED.ReadOnly = true;
                        TxtED.Enabled = false;
                        ImageButton BtnFech = (e.Row.FindControl("IbtFechaPP") as ImageButton);
                        BtnFech.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbResetPP") as CheckBox);
                        CkRest.Enabled = false;
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','HKMOD',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                    DropDownList DdlHK = (e.Row.FindControl("DdlHK") as DropDownList);
                    DdlHK.DataSource = Cnx.DSET(LtxtSql);
                    DdlHK.DataTextField = "Matricula";
                    DdlHK.DataValueField = "CodAeronave";
                    DdlHK.DataBind();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DdlHK.SelectedValue = dr["CodHK"].ToString();

                    LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','CONMOD',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", dr["CodHK"].ToString());
                    DropDownList DdlCont = (e.Row.FindControl("DdlCont") as DropDownList);
                    DdlCont.DataSource = Cnx.DSET(LtxtSql);
                    DdlCont.DataTextField = "CodContador";
                    DdlCont.DataValueField = "Cod";
                    DdlCont.DataBind();
                    DataRowView DRVC = e.Row.DataItem as DataRowView;
                    DdlCont.SelectedValue = DRVC["CodContador"].ToString();
                    if (DdlCont.SelectedValue.Trim().Equals("CAL") || DdlCont.SelectedValue.Trim().Equals("CTI"))
                    {
                        TextBox TxtNumDia = (e.Row.FindControl("TxtNumDia") as TextBox);
                        TxtNumDia.Enabled = false;
                        TxtNumDia.Text = "0";
                        TextBox TxtExtDia = (e.Row.FindControl("TxtExtDia") as TextBox);
                        TxtExtDia.Enabled = false;
                        TxtExtDia.Text = "0";
                    }
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrecI = (e.Row.FindControl("TxtFrecIni") as TextBox);
                        TxtFrecI.ReadOnly = true;
                        TxtFrecI.Enabled = false;
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrec") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtExt = (e.Row.FindControl("TxtExt") as TextBox);
                        TxtExt.ReadOnly = true;
                        TxtExt.Enabled = false;
                        TextBox TxtND = (e.Row.FindControl("TxtNumDia") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        TextBox TxtED = (e.Row.FindControl("TxtExtDia") as TextBox);
                        TxtED.ReadOnly = true;
                        TxtED.Enabled = false;
                        ImageButton BtnFech = (e.Row.FindControl("IbtFecha") as ImageButton);
                        BtnFech.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbReset") as CheckBox);
                        CkRest.Enabled = false;
                        CheckBox CkbHist = (e.Row.FindControl("CkbHist") as CheckBox);
                        CkbHist.Visible = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdAeron, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = "Seleccione el registro.";
                }
            }
        }
        protected void GrdAeron_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAeron.PageIndex = e.NewPageIndex;
            BindDataAll();
            BindDAK();
        }
        protected void GrdPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (TxtCod.Text.Equals(""))
                {
                    return;
                }
                PerfilesGrid();

                if (e.CommandName.Equals("AddNew"))
                {
                    ViewState["PN"] = (GrdPN.FooterRow.FindControl("DdlPNPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Cntdr"] = (GrdPN.FooterRow.FindControl("DdlContPNPP") as DropDownList).SelectedValue.Trim();
                    ViewState["Reset"] = (GrdPN.FooterRow.FindControl("CkbResetPP") as CheckBox).Checked == true ? 1 : 0;

                    if ((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["Frec"] = Convert.ToInt32(0);
                    }
                    else
                    {
                        ViewState["Frec"] = Convert.ToInt32((GrdPN.FooterRow.FindControl("TxtFrecPNPP") as TextBox).Text.Trim());
                    }

                    if ((GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox).Text.Trim().Equals(""))
                    {
                        ViewState["NroDia"] = Convert.ToInt32(0);
                    }
                    else
                    {
                        ViewState["NroDia"] = Convert.ToInt32((GrdPN.FooterRow.FindControl("TxtNumDiaPNPP") as TextBox).Text.Trim());
                    }

                    // validar
                    ValidarDetPN("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    {
                        BindDataAll();
                        return;
                    }
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
                            NroDias = (int)ViewState["NroDia"],
                            Usu = Session["C77U"].ToString(),
                            banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                            Resetear = (int)ViewState["Reset"],
                            Accion = "INSERT",
                            PN = ViewState["PN"].ToString(),
                            CodContador = ViewState["Cntdr"].ToString(),
                        };
                        ObjContSrvPn.Add(Detail);
                        TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                        ContSrvPn.Alimentar(ObjContSrvPn);
                        BindDataAll();
                        BindDPN();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT DET PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                    if (Row.RowIndex % 2 == 0)
                    {
                        Row.Style["background-color"] = "white";
                    }
                    else
                    {
                        Row.Style["background-color"] = "#cae4ff";
                    }
                    Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + Row.RowIndex);
                }
            }
        }
        protected void GrdPN_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdPN.EditIndex = e.NewEditIndex;
            BindDataAll();
            BindDPN();
        }
        protected void GrdPN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int VblId = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());
                int VbIdContPN = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Values["CodIdContadorPn"].ToString());
                PerfilesGrid();
                ViewState["PN"] = (GrdPN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();
                ViewState["Cntdr"] = (GrdPN.Rows[e.RowIndex].FindControl("LblContPN") as Label).Text.Trim();
                ViewState["Reset"] = (GrdPN.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Frec"] = Convert.ToInt32(0);
                }
                else
                {
                    ViewState["Frec"] = Convert.ToInt32((GrdPN.Rows[e.RowIndex].FindControl("TxtFrecPN") as TextBox).Text.Trim());
                }

                if ((GrdPN.Rows[e.RowIndex].FindControl("TxtNumDiaPN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["NroDia"] = Convert.ToInt32(0);
                }
                else
                {
                    ViewState["NroDia"] = Convert.ToInt32((GrdPN.Rows[e.RowIndex].FindControl("TxtNumDiaPN") as TextBox).Text.Trim());
                }

                // validar
                ValidarDetPN("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                {
                    BindDataAll();
                    return;
                }
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
                        NroDias = (int)ViewState["NroDia"],
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        Resetear = (int)ViewState["Reset"],
                        Accion = "UPDATE",
                        PN = ViewState["PN"].ToString(),
                        CodContador = ViewState["Cntdr"].ToString(),
                    };
                    ObjContSrvPn.Add(Detail);
                    TypeContSrvPn ContSrvPn = new TypeContSrvPn();
                    ContSrvPn.Alimentar(ObjContSrvPn);
                    GrdPN.EditIndex = -1;
                    BindDataAll();
                    BindDPN();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdPN.EditIndex = -1;
            BindDPN();
        }
        protected void GrdPN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery;
                int CodidcodSrvPn = Convert.ToInt32(GrdPN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','','','','VALIDA',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 10,'{1}','{2}','','','DELETE',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", CodidcodSrvPn, TxtCod.Text, Session["C77U"].ToString());
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDPN();
                                BindDataAll();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET P/N", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET AERONAVE SRV MANTO", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PN',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlPNPP = (e.Row.FindControl("DdlPNPP") as DropDownList);
                    DdlPNPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlPNPP.DataTextField = "PN";
                    DdlPNPP.DataValueField = "CodPN";
                    DdlPNPP.DataBind();
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPNPP") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPNPP") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbResetPP") as CheckBox);
                        CkRest.Enabled = false;
                    }
                }
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    if (LblCumplimi.Visible == false)
                    {
                        TextBox TxtFrec = (e.Row.FindControl("TxtFrecPN") as TextBox);
                        TxtFrec.ReadOnly = true;
                        TxtFrec.Enabled = false;
                        TxtFrec.Text = "1";
                        TextBox TxtND = (e.Row.FindControl("TxtNumDiaPN") as TextBox);
                        TxtND.ReadOnly = true;
                        TxtND.Enabled = false;
                        CheckBox CkRest = (e.Row.FindControl("CkbReset") as CheckBox);
                        CkRest.Enabled = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = "Seleccione el registro.";
                }
            }
        }
        protected void GrdPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAeron.PageIndex = e.NewPageIndex;
            BindDataAll();
            BindDPN();
        }
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
                        Label ext = Row.FindControl("LblExt") as Label;
                        Label extDia = Row.FindControl("LblExtDia") as Label;
                        if (ext != null)
                        {
                            decimal VbExt = Convert.ToDecimal(ext.Text);
                            decimal VbExtD = Convert.ToDecimal(extDia.Text);
                            TxtMatric.Text = GrdSN.DataKeys[this.GrdSN.SelectedIndex][2].ToString();
                            int VbID = Convert.ToInt32(GrdSN.DataKeys[this.GrdSN.SelectedIndex][0].ToString());
                            Cumplimiento(VbID, VbExt, VbExtD);
                            EstadoOT(VbID);
                        }
                    }
                    else
                    {
                        if (Row.RowIndex % 2 == 0)
                        {
                            Row.Style["background-color"] = "white";
                        }
                        else
                        {
                            Row.Style["background-color"] = "#cae4ff";
                        }
                        Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + Row.RowIndex);

                    }
                    PerfilesGrid();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "SelectedIndexChanged DET S/N", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdSN.EditIndex = e.NewEditIndex;
            BindDataAll();
            BindDSN();
        }
        protected void GrdSN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                ViewState["Historico"] = TxtHistorico.Text;
                PerfilesGrid();
                int VblId = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());
                ViewState["PN"] = (GrdSN.Rows[e.RowIndex].FindControl("LblPN") as Label).Text.Trim();
                if ((GrdSN.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true)
                { TxtHistorico.Enabled = true; }
                else
                { TxtHistorico.Enabled = false; TxtHistorico.Text = ""; ViewState["Historico"] = ""; }

                while ((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text = (GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Ext"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Ext"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtExt") as TextBox).Text.Trim()) * -1;
                }

                while ((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Contains("-"))
                {
                    (GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text = (GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Replace("-", "");
                }
                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["ExtDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["ExtDia"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtExtDia") as TextBox).Text.Trim()) * -1;
                }

                ViewState["CodElem"] = GrdSN.DataKeys[e.RowIndex].Values["CodElem"].ToString();

                ViewState["Cntdr"] = (GrdSN.Rows[e.RowIndex].FindControl("LblCont") as Label).Text.Trim();
                ViewState["Reset"] = (GrdSN.Rows[e.RowIndex].FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0;

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FrecIni"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["FrecIni"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtFrecIni") as TextBox).Text.Trim());
                }

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["Frec"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["Frec"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtFrec") as TextBox).Text.Trim());
                }

                if ((GrdSN.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["NroDia"] = Convert.ToDouble(0);
                }
                else
                {
                    ViewState["NroDia"] = Convert.ToDouble((GrdSN.Rows[e.RowIndex].FindControl("TxtNumDia") as TextBox).Text.Trim());
                }

                if (!(GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim().Equals(""))
                {
                    ViewState["FechaVenc"] = Convert.ToDateTime((GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim());
                }
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
                        Extension = (double)ViewState["Ext"],
                        FechaVencimiento = (GrdSN.Rows[e.RowIndex].FindControl("TxtFecVenSN") as TextBox).Text.Trim().Equals("") ? null : (DateTime?)ViewState["FechaVenc"],//(DateTime)ViewState["FechaVenc"],
                        NroDias = (double)ViewState["NroDia"],
                        ExtensionDias = (double)ViewState["ExtDia"],
                        BanOrdenTrabajo = 0,
                        Usu = Session["C77U"].ToString(),
                        banUnicoCumplimiento = DdlGrupo.SelectedValue.Trim().Equals("UCD") ? 1 : 0,
                        CodOt = null,
                        Compensacion = 0,
                        Resetear = (int)ViewState["Reset"],
                        FrecuenciaInicial = (double)ViewState["FrecIni"],
                        FrecuenciaInicalEjecutada = 0,
                        CodContador = ViewState["Cntdr"].ToString(),
                        CodElem = ViewState["CodElem"].ToString(),
                        PN = ViewState["PN"].ToString(),
                        Accion = "UPDATE",
                        Aplicabilidad = "SN",
                        CrearHistorico = (GrdSN.Rows[e.RowIndex].FindControl("CkbHist") as CheckBox).Checked == true ? "S" : "N",
                        Historico = ViewState["Historico"].ToString(),
                    };
                    ObjTypContaSM.Add(Detail);
                    CsTypContaSrvMant ContaSrvMant = new CsTypContaSrvMant();
                    ContaSrvMant.Alimentar(ObjTypContaSM);
                    GrdSN.EditIndex = -1;
                    TxtHistorico.Enabled = false;
                    TxtHistorico.Text = "";
                    BindDataAll();
                    BindDSN();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPDATE DET SN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdSN.EditIndex = -1;
            BindDSN();
        }
        protected void GrdSN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                PerfilesGrid();
                string VBQuery;
                int IDContaSrvManto = Convert.ToInt32(GrdSN.DataKeys[e.RowIndex].Value.ToString());

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 12,'','','','','VALIDA',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", IDContaSrvManto);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensj"].ToString() + "')", true);
                        PerfilesGrid();
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 12,'','','','{0}','DELETE',{1},{2},0,0,'01-01-01','01-01-01','01-01-01'",
                        Session["C77U"].ToString(), IDContaSrvManto, TxtId.Text);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                BindDSN();
                                BindDataAll();
                                PerfilesGrid();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Transac.Rollback();
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE DET S/N", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "VALIDACIÓN ELIMINAR DET S/N SRV MANTO", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdSN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdSN, "Select$" + e.Row.RowIndex);
                    e.Row.ToolTip = "Seleccione el registro.";
                }
            }
        }
        protected void GrdSN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdSN.PageIndex = e.NewPageIndex;
            BindDataAll();
            BindDSN();
        }
        protected void GrdHKAsig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                if (TxtCod.Text.Equals(""))
                {
                    BindDataAll();
                    return;
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VBQuery;
                    int VbCodHK = Convert.ToInt32((GrdHKAsig.FooterRow.FindControl("DdlMatAsigPP") as DropDownList).Text.Trim());
                    if ((GrdHKAsig.FooterRow.FindControl("DdlMatAsigPP") as DropDownList).Text.Trim().Equals("0"))
                    {
                        BindDataAll();
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una aeronave')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 4,'{2}','','','','','','','HKASIG','INSERT',{0},{1},0,0,0,0,'01-01-1','02-01-1','03-01-1'"
                            , TxtId.Text, VbCodHK, Session["C77U"].ToString());
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon); ;
                        sqlCmd.ExecuteNonQuery();
                        BindDataAll();
                        UpPnlPN.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Aaeronaves asignadas", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdHKAsig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    int VblId = Convert.ToInt32(GrdHKAsig.DataKeys[e.RowIndex].Values["IdSrvMantoAeronave"].ToString());
                    int VbCodHK = Convert.ToInt32(GrdHKAsig.DataKeys[e.RowIndex].Values["CodAeronave"].ToString());

                    string VBQuery = string.Format("EXEC SP_TablasIngenieria 4,'{2}','','','','','','','HKASIG','DELETE',{0},{1},{3},0,0,0,'01-01-1','02-01-1','03-01-1'"
                           , TxtId.Text, VbCodHK, Session["C77U"].ToString(), VblId);
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAll();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE Aaeronaves asignadas", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdHKAsig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtId.Text.Equals(""))
            {
                string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'{0}','','','','HKAS',{1},0,0,0,'01-01-01','01-01-01','01-01-01'", DdlModel.SelectedValue, TxtId.Text);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList DdlMatPP = (e.Row.FindControl("DdlMatAsigPP") as DropDownList);
                    DdlMatPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlMatPP.DataTextField = "Matricula";
                    DdlMatPP.DataValueField = "CodAeronave";
                    DdlMatPP.DataBind();
                }
            }
        }
        protected void GrdHKAsig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdHKAsig.PageIndex = e.NewPageIndex;
            BindDHKAsig();
        }
        protected void GrdAdj_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (TxtCod.Text.Equals(""))
                {
                    BindDataAll();
                    return;
                }
                if (e.CommandName.Equals("Download"))
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int VblID = int.Parse(GrdAdj.DataKeys[gvr.RowIndex].Value.ToString());
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        string LtxtSql = string.Format(" EXEC SP_PANTALLA__Servicio_Manto2 2,'','','','','',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", VblID);
                        SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
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
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en la descarga')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "Descargar adjuntos", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un archivo')", true);
                            return;
                        }
                    }
                    if (Vbl3Desc.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una descripción')", true);
                        return;
                    }
                    if (Vbl4Ruta.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un archivo')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = string.Format("INSERT INTO TblAdjuntos(IdProceso,CodProceso,Proceso,Descripcion,Ruta,ArchivoAdj,Extension,UsuCrea,UsuMod,FechaCrea,FechaMod,TipoArchivo)  " +
                            "VALUES({0},'{1}','{2}',@Desc,'{4}',@Image,'{6}','{7}','{7}',GETDATE(),GETDATE(),'{8}')",
                            TxtId.Text, TxtCod.Text, "DOCINGENIERIA", "3 N/A", Vbl4Ruta, "Vbl5Adj", Vbl6Ext, Session["C77U"].ToString(), Vbl8Type);
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                                SqlCmd.Parameters.AddWithValue("@Image", imagen);
                                SqlCmd.ExecuteNonQuery();
                                BindDAdjunto();
                                PerfilesGrid();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "INSERT Adjuntos", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT TblAdjuntos", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void TxtSubAta_TextChanged(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'{0}','','','','Consecutivo_ATA',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtSubAta.Text);
                SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    if (TxtConsAta.Text.Trim().Equals("") || TxtConsAta.Text.Trim().Equals("0"))
                    { TxtConsAta.Text = SDR["MAXI"].ToString(); }
                }
            }
        }
        protected void GrdAdj_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdAdj.EditIndex = e.NewEditIndex;
            BindDAdjunto();
        }
        protected void GrdAdj_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
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
                ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una descripción')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VblSiAdjunto = FileUp.HasFile == true ? " Ruta = @Nom,ArchivoAdj = @Image, Extension = @Ext,TipoArchivo = @TipoA," : "";
                sqlCon.Open();
                VBQuery = string.Format("UPDATE TblAdjuntos SET Descripcion =@Desc ," + VblSiAdjunto + "  UsuMod='{1}', FechaMod=GETDATE() " +
                    "WHERE IdAdjuntos = {0}", GrdAdj.DataKeys[e.RowIndex].Value.ToString(), Session["C77U"].ToString());
                using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon))
                {
                    try
                    {
                        SqlCmd.Parameters.AddWithValue("@Desc", Vbl3Desc);
                        if (FileUp.HasFile)
                        {
                            SqlCmd.Parameters.AddWithValue("@Nom", Vbl4Ruta);
                            SqlCmd.Parameters.AddWithValue("@Image", imagen);
                            SqlCmd.Parameters.AddWithValue("@Ext", Vbl6Ext);
                            SqlCmd.Parameters.AddWithValue("@TipoA", Vbl8Type);
                        }
                        SqlCmd.ExecuteNonQuery();
                        GrdAdj.EditIndex = -1;
                        BindDAdjunto();
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlPN, UpPnlPN.GetType(), "IdntificadorBloqueScript", "alert('Error en la actualización')", true);
                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "UPATE Adjunto", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                    }
                }
            }
        }
        protected void GrdAdj_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdAdj.EditIndex = -1;
            BindDAdjunto();
        }
        protected void GrdAdj_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    int VblId = Convert.ToInt32(GrdAdj.DataKeys[e.RowIndex].Values["IdAdjuntos"].ToString());
                    string VblRuta = GrdAdj.DataKeys[e.RowIndex].Values["Ruta"].ToString();

                    string VBQuery = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 13,'{0}','{1}','','','',{2},{3},0,0,'01-01-01','01-01-01','01-01-01'"
                           , Session["C77U"].ToString(), VblRuta, VblId, TxtId.Text);
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDataAll();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "DELETE Adjuntos", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdAdj_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAdj.PageIndex = e.NewPageIndex;
            BindDAdjunto();
        }
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
        protected void BIndDataBusq(string Prmtr)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql, VbOpcion = "";
                VbTxtSql = "";
                if (RdbBusqDes.Checked == true && TblBusqHK.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqDesPN.Checked == true && TblBusqPN.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqPnPN.Checked == true && TblBusqPN.Visible == true)
                {
                    VbOpcion = "P";
                }
                if (RdbBusqDesSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "D";
                }
                if (RdbBusqPnSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "P";
                }
                if (RdbBusqSnSN.Checked == true && TblBusqSN.Visible == true)
                {
                    VbOpcion = "S";
                }
                if (!VbOpcion.Equals(""))
                {
                    VbTxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 15,'{0}','{1}','{2}','{3}','',0,0,0,0,'01-01-01','01-01-01','01-01-01'",
                       Prmtr, ViewState["TIPO"].ToString(), Session["PllaSrvManto"].ToString(), VbOpcion);
                    sqlConB.Open();
                    SqlDataAdapter DAB = new SqlDataAdapter(VbTxtSql, sqlConB);
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
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        {
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
            //BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text);
            BindDTraerdatos(HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text));
            UpPnlCampos.Update();
            switch (ViewState["TIPO"].ToString())
            {
                case "A":
                    BindDAK();
                    break;
                case "P":
                    BindDPN();
                    break;
                default:
                    BindDSN();
                    break;
            }
            BindDataAll();
            UpPnlPN.Update();
            PerfilesGrid();
            PnlBusq.Visible = false;
            PnlCampos.Visible = true;
        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusq.PageIndex = e.NewPageIndex;
            BIndDataBusq(TxtBusqueda.Text);
        }
        // ****************Controles de Recurso fisico ***********************
        protected void BindDRecursoF()
        {
            try
            {
                DataTable DTHA = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 4,'','','','',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtId.Text.Equals("") ? "0" : TxtId.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDAHA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDAHA.Fill(DTHA);
                    if (DTHA.Rows.Count > 0)
                    {
                        GrdRecursoF.DataSource = DTHA;
                        GrdRecursoF.DataBind();
                    }
                    else
                    {
                        DTHA.Rows.Add(DTHA.NewRow());
                        GrdRecursoF.DataSource = DTHA;
                        GrdRecursoF.DataBind();
                        GrdRecursoF.Rows[0].Cells.Clear();
                        GrdRecursoF.Rows[0].Cells.Add(new TableCell());
                        GrdRecursoF.Rows[0].Cells[0].Text = "Empty..!";
                        GrdRecursoF.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDRecursoF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
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
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string VblString = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'{0}','','','','DescRef',0,0,0,0,'01-01-01','01-01-01','01-01-01'", DdlPNRFPP.Text);
                SqlCommand SC = new SqlCommand(VblString, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtDesRFPP.Text = SDR["Descripcion"].ToString();
                }
            }
        }
        protected void IbtCerrarRec_Click(object sender, ImageClickEventArgs e)
        {
            PnlCampos.Visible = true;
            PnlRecursos.Visible = false;
            PerfilesGrid();
        }
        protected void GrdRecursoF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
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
                    VblTxtCant = VblTxtCant.Replace(".", ",");
                    VblCant = (GrdRecursoF.FooterRow.FindControl("TxtCantRFPP") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VblTxtCant);
                    VblCond = (GrdRecursoF.FooterRow.FindControl("CkbCondicPP") as CheckBox).Checked == true ? 1 : 0;
                    VbDesc = (GrdRecursoF.FooterRow.FindControl("TxtDesRFPP") as TextBox).Text.Trim();
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {

                            VBQuery = string.Format("EXEC SP_TablasIngenieria 5,'{0}','{1}','{7}','','','','','','INSERT',{2},{3},{4},{5},{6},0,'01-01-1','02-01-1','03-01-1'",
                            VblPN, Session["C77U"].ToString(), 0, TxtId.Text, VblCant, VblCond, VblFase, VbDesc);

                            using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    //SqlCmd.ExecuteNonQuery();
                                    var Mensj = SqlCmd.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDRecursoF();
                                    // BindDataAll(TxtCod.Text, VblPN);
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdRecursoF.EditIndex = e.NewEditIndex;
            BindDRecursoF();
        }
        protected void GrdRecursoF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                PerfilesGrid();
                string VblPN, VBQuery, VblTxtCant;
                int VblFase, VblCond;
                double VblCant;
                int VblId = Convert.ToInt32(GrdRecursoF.DataKeys[e.RowIndex].Value.ToString());
                VblPN = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtPNRF") as TextBox).Text.Trim();
                VblFase = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim().Equals("") ? 0 : Convert.ToInt32((GrdRecursoF.Rows[e.RowIndex].FindControl("TxtFaseRF") as TextBox).Text.Trim());
                VblTxtCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim().Equals("") ? "0" : (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim();
                Cnx.RetirarPuntos(VblTxtCant);
                VblTxtCant = Cnx.ValorDecimal();
                VblCant = (GrdRecursoF.Rows[e.RowIndex].FindControl("TxtCantRF") as TextBox).Text.Trim().Length == 0 ? 1 : Convert.ToDouble(VblTxtCant);
                VblCond = (GrdRecursoF.Rows[e.RowIndex].FindControl("CkbCondic") as CheckBox).Checked == true ? 1 : 0;
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {

                        VBQuery = string.Format("EXEC SP_TablasIngenieria 5,'{0}','{1}','','','','','','','UPDATE',{2},{3},@Cant,{4},{5},0,'01-01-1','02-01-1','03-01-1'",
                        VblPN, Session["C77U"].ToString(), VblId, TxtId.Text, VblCond, VblFase);

                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Cant", VblCant);
                                var Mensj = SqlCmd.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                GrdRecursoF.EditIndex = -1;
                                BindDRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdRecursoF.EditIndex = -1;
            BindDRecursoF();
        }
        protected void GrdRecursoF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 5,'{0}','{1}','','','','','','','DELETE',{2},{3},@Cant,{4},{5},0,'01-01-1','02-01-1','03-01-1'",
                        VblPN, Session["C77U"].ToString(), VblId, TxtId.Text, VblCond, VblFase);

                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@Cant", VblCant);
                                var Mensj = SqlCmd.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDRecursoF();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdRecursoF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','PNRF',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlPNRFPP = (e.Row.FindControl("DdlPNRFPP") as DropDownList);
                DdlPNRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlPNRFPP.DataTextField = "PN";
                DdlPNRFPP.DataValueField = "CodPN";
                DdlPNRFPP.DataBind();
            }
        }
        protected void GrdRecursoF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdRecursoF.PageIndex = e.NewPageIndex;
            BindDRecursoF();
        }
        protected void BindDLicencia()
        {
            try
            {
                DataTable DTHA = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_Servicio_Manto 1,'','','','',{0},0,0,0,'01-1-2009','01-01-1900','01-01-1900'", TxtId.Text.Equals("") ? "0" : TxtId.Text);
                    sqlCon.Open();
                    SqlDataAdapter SDAHA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SDAHA.Fill(DTHA);
                    if (DTHA.Rows.Count > 0)
                    {
                        GrdLicen.DataSource = DTHA;
                        GrdLicen.DataBind();
                    }
                    else
                    {
                        DTHA.Rows.Add(DTHA.NewRow());
                        GrdLicen.DataSource = DTHA;
                        GrdLicen.DataBind();
                        GrdLicen.Rows[0].Cells.Clear();
                        GrdLicen.Rows[0].Cells.Add(new TableCell());
                        GrdLicen.Rows[0].Cells[0].Text = "Empty..!";
                        GrdLicen.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BindDRecursoF", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void DdlLicenRFPP_TextChanged(object sender, EventArgs e)
        {
            PerfilesGrid();
            TextBox TxtDesLiRFPP = (GrdLicen.FooterRow.FindControl("TxtDesLiRFPP") as TextBox);
            DropDownList DdlLicenRFPP = (GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList);
            Cnx.SelecBD();
            using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
            {
                Cnx2.Open();
                string VblString = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 17,'','','','','DescLicenRF',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", DdlLicenRFPP.SelectedValue);
                SqlCommand SC = new SqlCommand(VblString, Cnx2);
                SqlDataReader SDR = SC.ExecuteReader();
                if (SDR.Read())
                {
                    TxtDesLiRFPP.Text = SDR["Descripcion"].ToString();
                }
            }
        }
        protected void GrdLicen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();
                if (e.CommandName.Equals("AddNew"))
                {
                    string VBQuery, VblTxtTE, VbCodIdLicencia;
                    double VblTE;
                    if ((GrdLicen.FooterRow.FindControl("DdlLicenRFPP") as DropDownList).SelectedValue.Equals("0"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una licencia')", true);
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','INSERT',{2},{3},@TiempEst,0,0,0,'01-01-1','02-01-1','03-01-1'",
                            Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia);
                            using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                                    var Mensj = SqlCmd.ExecuteScalar();
                                    if (!Mensj.ToString().Trim().Equals(""))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj.ToString().Trim() + "')", true);
                                        Transac.Rollback();
                                        return;
                                    }
                                    Transac.Commit();
                                    BindDLicencia();
                                }
                                catch (Exception Ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdLicen.EditIndex = e.NewEditIndex;
            BindDLicencia();
        }
        protected void GrdLicen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
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
                        VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','UPDATE',{2},{3},@TiempEst,{4},0,0,'01-01-1','02-01-1','03-01-1'",
                         Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);
                        using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SqlCmd.Parameters.AddWithValue("@TiempEst", VblTE);
                                SqlCmd.ExecuteNonQuery();
                                Transac.Commit();
                                GrdLicen.EditIndex = -1;
                                BindDLicencia();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "UPDATE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INSERT Recurso", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdLicen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdLicen.EditIndex = -1;
            BindDLicencia();
        }
        private string VblTE;
        protected void GrdLicen_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
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
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 8,'{0}','{1}','','','','','','','DELETE',{2},{3},@TiempEst,{4},0,0,'01-01-1','02-01-1','03-01-1'",
                    Session["C77U"].ToString(), TxtCod.Text, TxtId.Text, VbCodIdLicencia, IdSrvLic);

                    using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            SqlCmd.Parameters.AddWithValue("@TiempEst", Convert.ToDouble(VblTE));
                            SqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            BindDLicencia();
                        }
                        catch (Exception Ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlRF, UpPnlRF.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Licencia", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void GrdLicen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            string LtxtSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 3,'','','','','LICRF',{0},0,0,0,'01-01-01','01-01-01','01-01-01'", TxtId.Text);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlLicenRFPP = (e.Row.FindControl("DdlLicenRFPP") as DropDownList);
                DdlLicenRFPP.DataSource = Cnx.DSET(LtxtSql);
                DdlLicenRFPP.DataTextField = "CodLicencia";
                DdlLicenRFPP.DataValueField = "CodIdLicencia";
                DdlLicenRFPP.DataBind();
            }
        }
        protected void GrdLicen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        // ****************Panel informes  ***********************
        protected void IbtCerrarInf_Click(object sender, ImageClickEventArgs e)
        {
            PnlInforme.Visible = false;
            PnlCampos.Visible = true;
            PerfilesGrid();
        }
        private string StSql;
        protected void BtnSvcAct_Click(object sender, EventArgs e)
        {

            string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
            DataSet ds = new DataSet();
            Cnx.SelecBD();
            using (SqlConnection SC = new SqlConnection(Cnx.GetConex()))
            {
                ReportParameter[] parameters = new ReportParameter[4];
                switch (ViewState["TIPO"].ToString())
                {
                    case "A":
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','A',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtMatric.Text.Trim());
                        parameters[0] = new ReportParameter("PrmrHK", "Matrícula: " + TxtMatric.Text.Trim());
                        break;
                    case "P":
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','P',0,0,0,0,'01-01-01','01-01-01','01-01-01'", ViewState["PN"].ToString().Trim());
                        parameters[0] = new ReportParameter("PrmrHK", "P/N: " + ViewState["PN"].ToString().Trim());
                        break;
                    default:
                        StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 18,'{0}','','','','S',0,0,0,0,'01-01-01','01-01-01','01-01-01'", ViewState["CodElem"].ToString().Trim());
                        string VbMatr = TxtMatric.Text.Equals("") ? "" : "  |  Matrícula: " + TxtMatric.Text;
                        string vvv = "Elemento: P/N  " + ViewState["PN"].ToString().Trim() + "  |  S/N  " + ViewState["SN"].ToString().Trim() + VbMatr;
                        parameters[0] = new ReportParameter("PrmrHK", "Elemento: P/N  " + ViewState["PN"].ToString().Trim() + "  |  S/N  " + ViewState["SN"].ToString().Trim() + VbMatr);
                        break;
                }
                parameters[1] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                parameters[2] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                parameters[3] = new ReportParameter("PrmImg", VbLogo, true);


                SqlDataAdapter da = new SqlDataAdapter(StSql, SC);
                da.Fill(ds);
                RprvSvcActivos.LocalReport.EnableExternalImages = true;
                RprvSvcActivos.LocalReport.ReportPath = "Forms/Ingenieria/Informe/ServiciosActivos.rdlc";
                RprvSvcActivos.LocalReport.DataSources.Clear();
                RprvSvcActivos.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                RprvSvcActivos.LocalReport.SetParameters(parameters);
                RprvSvcActivos.LocalReport.Refresh();
            }
        }
        protected void BtnCumplim_Click(object sender, EventArgs e)
        {
            if (!ViewState["TIPO"].ToString().Equals("P"))
            {
                if (ViewState["TIPO"].ToString().Equals("A"))
                {
                    StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 19,'{0}','{1}','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", TxtMatric.Text.Trim(), TxtCod.Text);
                }
                else
                {
                    StSql = string.Format("EXEC SP_PANTALLA__Servicio_Manto2 20,'{0}','{1}','','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'", ViewState["CodElem"], TxtCod.Text);
                }
                string VbLogo = @"file:///" + Server.MapPath("~/images/" + Session["LogoPpal"].ToString().Trim());
                DataSet ds = new DataSet();
                Cnx.SelecBD();
                using (SqlConnection SC = new SqlConnection(Cnx.GetConex()))
                {
                    ReportParameter[] parameters = new ReportParameter[4];
                    parameters[0] = new ReportParameter("PrmCia", Session["NomCiaPpal"].ToString().Trim());
                    parameters[1] = new ReportParameter("PrmNit", Session["Nit77Cia"].ToString().Trim());
                    parameters[2] = new ReportParameter("PrmImg", VbLogo, true);
                    parameters[3] = new ReportParameter("PrmTipo", ViewState["TIPO"].ToString());
                    SqlDataAdapter da = new SqlDataAdapter(StSql, SC);
                    da.Fill(ds);
                    RprvSvcActivos.LocalReport.EnableExternalImages = true;
                    RprvSvcActivos.LocalReport.ReportPath = "Forms/Ingenieria/Informe/CumplimientoSvc.rdlc";
                    RprvSvcActivos.LocalReport.DataSources.Clear();
                    RprvSvcActivos.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                    RprvSvcActivos.LocalReport.SetParameters(parameters);
                    RprvSvcActivos.LocalReport.Refresh();
                }
            }
        }
        protected void IbtExpExcelSvcAplAK_Click(object sender, ImageClickEventArgs e)
        {
            Exportar("Asignada");
        }
        protected void IbtExpExcelSvcGnrl_Click(object sender, ImageClickEventArgs e)
        {
            Exportar("");
        }
        protected void Exportar(string Condcion)
        {
            try
            {
                string StSql, VbNomRpt;
                if (Condcion.Equals("Asignada"))
                {
                    StSql = "EXEC SP_PANTALLA_Servicio_Manto 27,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    VbNomRpt = "Svc_aeronave_Asignadas";
                }
                else
                {
                    StSql = "EXEC SP_PANTALLA_Servicio_Manto 22,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    VbNomRpt = "Svc_Mantenimiento";
                }
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(StSql, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "REPORTES";
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
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlInforme, UpPnlInforme.GetType(), "IdntificadorBloqueScript", "alert('error')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}
