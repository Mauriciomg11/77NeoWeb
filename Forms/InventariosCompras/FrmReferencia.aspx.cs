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

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmReferencia : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        private int VbPos, VbConsu, VbMot, VbMay, VbApu, VbSuC, VbRepa, VbVerif, VbNif;
        float VblStock;
        string VbCod, VbGrup, VbAta, VbUm, VbIdent, VbTip, VbDes, VbModel, VbDescEsp, VbInfAd, PVbCat;
        private string LblCUMCP, LblCEquP;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            }/* */
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                /*Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1"; */
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
                BindDataDdl();
                BindDataMan("");
                BindDataPN("");
                ViewState["NewRef"] = "";
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
        }
        protected void BIndDataBusq(string Prmtr)
        {
            try
            {
                DataTable DtB = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {

                    string VbTxtSql;
                    VbTxtSql = "";
                    if (RdbBusqR.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'{0}','','','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", Prmtr);
                    }
                    if (RdbBusqP.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'','{0}','','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", Prmtr);
                    }
                    if (RdbBusqD.Checked == true)
                    {
                        VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 5,'','','{0}','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", Prmtr);
                    }
                    if (!VbTxtSql.Equals(""))
                    {
                        sqlConB.Open();
                        SqlDataAdapter DAB = new SqlDataAdapter(VbTxtSql, sqlConB);
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdDatos.DataSource = DtB;
                            GrdDatos.DataBind();
                        }
                        else
                        {
                            GrdDatos.DataSource = null;
                            GrdDatos.DataBind();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('error')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "BIndDataBusq", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BindDataDdl()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','GRU',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlGrupo.DataSource = Cnx.DSET(LtxtSql);
                DdlGrupo.DataMember = "Datos";
                DdlGrupo.DataTextField = "Descripcion";
                DdlGrupo.DataValueField = "CodTipoElemento";
                DdlGrupo.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','ATA',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlAta.DataSource = Cnx.DSET(LtxtSql);
                DdlAta.DataMember = "Datos";
                DdlAta.DataTextField = "Descripcion";
                DdlAta.DataValueField = "CodCapitulo";
                DdlAta.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','UM',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlUM.DataSource = Cnx.DSET(LtxtSql);
                DdlUM.DataMember = "Datos";
                DdlUM.DataTextField = "Descripcion";
                DdlUM.DataValueField = "CodUnidMedida";
                DdlUM.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','IDE',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlIdent.DataSource = Cnx.DSET(LtxtSql);
                DdlIdent.DataMember = "Datos";
                DdlIdent.DataTextField = "Identificador";
                DdlIdent.DataValueField = "Codigo";
                DdlIdent.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'" + Session["CodTipoCodigoInicial"].ToString() + "','','','','TIP',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlTipo.DataSource = Cnx.DSET(LtxtSql);
                DdlTipo.DataMember = "Datos";
                DdlTipo.DataTextField = "Descripcion";
                DdlTipo.DataValueField = "CodTipoCodigo";
                DdlTipo.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','MOD',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlMod.DataSource = Cnx.DSET(LtxtSql);
                DdlMod.DataMember = "Datos";
                DdlMod.DataTextField = "Descripcion";
                DdlMod.DataValueField = "CodModelo";
                DdlMod.DataBind();

                LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','CAT',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                DdlCat.DataSource = Cnx.DSET(LtxtSql);
                DdlCat.DataMember = "Datos";
                DdlCat.DataTextField = "Descripcion";
                DdlCat.DataValueField = "CodCategoriaMA";
                DdlCat.DataBind();
            }
        }
        protected void BindDataMan(string Ref)
        {
            DataTable DtMan = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 6,'{0}','','','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", Ref);
                sqlCon.Open();
                SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                SqlDA.Fill(DtMan);
                if (DtMan.Rows.Count > 0)
                {
                    GrdMan.DataSource = DtMan;
                    GrdMan.DataBind();
                }
                else
                {
                    DtMan.Rows.Add(DtMan.NewRow());
                    GrdMan.DataSource = DtMan;
                    GrdMan.DataBind();
                    GrdMan.Rows[0].Cells.Clear();
                    GrdMan.Rows[0].Cells.Add(new TableCell());
                    GrdMan.Rows[0].Cells[0].ColumnSpan = DtMan.Columns.Count;
                    GrdMan.Rows[0].Cells[0].Text = "Sin datos..!";
                    GrdMan.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        protected void BindDataPN(string Ref)
        {
            try
            {
                DataTable DtPN = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC Consultas_General_Logistica 31,'{0}','','',0, 0 ,0,'01-01-1','01-01-1'", Ref);
                    sqlCon.Open();
                    SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SqlDA.Fill(DtPN);
                    if (DtPN.Rows.Count > 0)
                    {
                        GrdPN.DataSource = DtPN;
                        GrdPN.DataBind();
                    }
                    else
                    {
                        DtPN.Rows.Add(DtPN.NewRow());
                        GrdPN.DataSource = DtPN;
                        GrdPN.DataBind();
                        GrdPN.Rows[0].Cells.Clear();
                        GrdPN.Rows[0].Cells.Add(new TableCell());
                        GrdPN.Rows[0].Cells[0].ColumnSpan = DtPN.Columns.Count;
                        GrdPN.Rows[0].Cells[0].Text = "Sin datos..!";
                        GrdPN.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            catch (Exception Ex)
            {
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "BindDataPN", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }

        }
        protected void BindDataCont(string PN)
        {
            try
            {
                DataTable DtCont = new DataTable();
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 9,'{0}','','','','SELECT',0,0,0,0,'01-01-01','02-01-01','03-01-01'", PN);
                    sqlCon.Open();
                    SqlDataAdapter SqlDA = new SqlDataAdapter(VbTxtSql, sqlCon);
                    SqlDA.Fill(DtCont);
                    if (DtCont.Rows.Count > 0)
                    {
                        GrdCont.DataSource = DtCont;
                        GrdCont.DataBind();
                    }
                    else
                    {
                        DtCont.Rows.Add(DtCont.NewRow());
                        GrdCont.DataSource = DtCont;
                        GrdCont.DataBind();
                        GrdCont.Rows[0].Cells.Clear();
                        GrdCont.Rows[0].Cells.Add(new TableCell());
                        GrdCont.Rows[0].Cells[0].ColumnSpan = DtCont.Columns.Count;
                        GrdCont.Rows[0].Cells[0].Text = "Sin contador..!";
                        GrdCont.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                if (!DdlIdent.Text.Equals("SN"))
                {
                    ImageButton Imge = GrdCont.FooterRow.FindControl("IbtAddNew") as ImageButton;

                    if (Imge != null)
                    {
                        Imge.Enabled = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                string Mensje = Ex.Message;
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + Mensje + "')", true);
            }
        }
        protected void BindDataCambUMC(string PN)
        {
            DataTable DtCUC = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_Referencia 13,@PN, '','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlCon.Open();
                SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon);
                SC.Parameters.AddWithValue("@PN", PN);

                SqlDataAdapter DACUC = new SqlDataAdapter(SC);
                DACUC.Fill(DtCUC);
                if (DtCUC.Rows.Count > 0)
                {
                    GrdCamUC.DataSource = DtCUC;
                    GrdCamUC.DataBind();
                }
                else
                {
                    DtCUC.Rows.Add(DtCUC.NewRow());
                    GrdCamUC.DataSource = DtCUC;
                    GrdCamUC.DataBind();
                    GrdCamUC.Rows[0].Cells.Clear();
                    GrdCamUC.Rows[0].Cells.Add(new TableCell());
                    GrdCamUC.Rows[0].Cells[0].ColumnSpan = DtCUC.Columns.Count;
                    GrdCamUC.Rows[0].Cells[0].Text = "Sin datos..!";
                    GrdCamUC.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        protected void BindDataCambioRef()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql;
                VbTxtSql = "";
                if (RdbRefCRef.Checked == true)
                {
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_Referencia 4,'{0}','{1}','{2}','RF',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlGrupo.SelectedValue, TxtCod.Text, TxtCambRef.Text);
                }
                if (RdbPnCRef.Checked == true)
                {
                    VbTxtSql = string.Format("EXEC SP_PANTALLA_Referencia 4,'{0}','{1}',@PN,'PN',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlGrupo.SelectedValue, TxtCod.Text);
                }

                if (!VbTxtSql.Equals(""))
                {
                    sqlConB.Open();
                    SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB);
                    SC.Parameters.AddWithValue("@PN", TxtCambRef.Text);
                    SqlDataAdapter DAB = new SqlDataAdapter(SC);
                    DAB.Fill(DtB);

                    if (DtB.Rows.Count > 0)
                    {
                        GrdCambioRef.DataSource = DtB;
                        GrdCambioRef.DataBind();
                    }
                    else
                    {
                        GrdCambioRef.DataSource = null;
                        GrdCambioRef.DataBind();
                    }
                }
                else
                {
                    GrdCambioRef.DataSource = null;
                    GrdCambioRef.DataBind();
                }
            }
        }
        protected void BindDataAll(string VblRef, string VblPN)
        {
            try
            {
                BindDataMan(VblRef);
                BindDataPN(VblRef);
                BindDataCont(VblPN);
                BtnUndCompra.Enabled = false;
                BtnCambioRef.Enabled = false;
            }
            catch(Exception Ex)
            {
                string vd = Ex.ToString();
            }
        }
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
                string LtxtSql = String.Format("SELECT TOP 1 CodReferencia FROM TblReferencia WHERE SUBSTRing(RTRIM(CodReferencia),1,7)='{0}' ORDER BY IdReferencia desc ", Ref);
                SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                sqlCon.Open();
                SqlDataReader tbl = Comando.ExecuteReader();
                if (tbl.Read())
                {
                    TxtCod.Text = tbl["CodReferencia"].ToString();
                }
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
                string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 8,'{0}','','','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
                SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
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
                            {
                                CkbVerif.Enabled = Edi;
                            }
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
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 1,'{0}','{1}','','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text, DdlUM.SelectedValue);
                    SqlCommand SqlC2 = new SqlCommand(LtxtSql, Cnx2);
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
                string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 10,'{0}','','','','ACTIVA-APU',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
                SqlCommand SqlC2 = new SqlCommand(LtxtSql, Cnx3);
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

            if (DdlGrupo.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un grupo')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlAta.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un ata')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlUM.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una unidad de despacho')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlIdent.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un identificador')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (DdlTipo.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un tipo')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (TxtDesc.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una descripción')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (TxtDescEsp.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una descripción')", true);
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
            VbMay = CkbMay.Checked == true ? 1 : 0;
            VbApu = CkbApu.Checked == true ? 1 : 0;
            VbSuC = CkbSub.Checked == true ? 1 : 0;
            VbMay = VbMot == 1 ? 1 : 0; // si motor tiene que ser mayor
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
        protected void ValidarPN(string PN, string PNAnt, string Estado, int Bloqueo)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                string VBQuery = "EXEC SP_Pantalla_Parametros 1,@PN,@PNAnt,'TblPN','PN','TblPN',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                Comando.Parameters.AddWithValue("@PN", PN);
                Comando.Parameters.AddWithValue("@PNAnt", PNAnt);
                SqlDataReader registro = Comando.ExecuteReader();
                if (registro.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('El PN existe')", true);
                    return;
                }
                sqlCon.Close();
            }

            Session["VlRefer"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una referencia')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (Estado == String.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar un estado')", true);
                Session["VlRefer"] = "N";
                return;
            }
            if (PN == String.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar UN PN')", true);
                Session["VlRefer"] = "N";
                return;
            }

            if (Bloqueo == 1 && Estado.Equals("01"))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Un P/N bloqueado no puede ser principal')", true);
                Session["VlRefer"] = "N";
                return;
            }
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
        protected void DdlUMCom_TextChanged(object sender, EventArgs e)
        {
            string VblUMCD;

            DropDownList DdlUMCom = GrdPN.SelectedRow.FindControl("DdlUMCom") as DropDownList;
            VblUMCD = DdlUMCom.SelectedValue;

            TextBox TxtEqu = GrdPN.SelectedRow.FindControl("TxtEqu") as TextBox;
            Cnx.SelecBD();
            using (SqlConnection sqlConx = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = string.Format("SELECT VlorEquivalencia FROM TblUndXPn WHERE Pn=@PN AND UndCompraPN='{0}'", VblUMCD);
                SqlCommand SC = new SqlCommand(LtxtSql, sqlConx);
                SC.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                sqlConx.Open();
                SqlDataReader tbl = SC.ExecuteReader();
                if (tbl.Read())
                {
                    TxtEqu.Text = tbl["VlorEquivalencia"].ToString();
                }
                BindDataMan(TxtCod.Text);
                BindDataCont(ViewState["VbPNSI"].ToString());
            }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (BtnIngresar.Text == "Ingresar")
            {
                ActivarBotones(true, false, false, false, false);
                BtnIngresar.Text = "Aceptar";
                ActivarCampos(true, true, "Ingresar");
                LimpiarCampos();
                DdlTipo.Text = "01";
                BtnIngresar.OnClientClick = "return confirm('¿Desea realizar el ingreso?');";
            }
            else
            {
                try
                {
                    AsignarValores("Ingresar");
                    if (Session["VlRefer"].ToString() == "N")
                    {
                        BindDataAll(TxtCod.Text, "");
                        return;
                    }
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
                    BtnIngresar.Text = "Ingresar";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Ingresar");
                    BtnIngresar.OnClientClick = "";
                    BindDataAll("", "");
                    BusqNewReg(VbCod);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso de los dato')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
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
        {
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            if (BtnModificar.Text == "Modificar")
            {
                ActivarBotones(false, true, false, false, false);
                BtnModificar.Text = "Aceptar";
                ActivarCampos(false, true, "Modificar");
                BtnModificar.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
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
                        string VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 14,'{0}','{1}','{2}','','',{3},0,0,0,'01-01-01','02-01-01','03-01-01'",
                        TxtCod.Text, ProcesarPLano, Session["C77U"].ToString(), VbVerif);
                        using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                        {
                            try
                            {
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "Planos Referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                    BtnModificar.Text = "Modificar";
                    ActivarBotones(true, true, true, true, true);
                    ActivarCampos(false, false, "Modificar");
                    BtnModificar.OnClientClick = "";
                    BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en la edición de los datos')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                BIndDataBusq("77NEO");
                PnlCampos.Visible = false;
                PnlBusq.Visible = true;
                PnlUnidadCompra.Visible = false;
            }
            catch (Exception Ex)
            {
                string mesah = Ex.ToString();
            }
        }
        protected void BtnInformes_Click(object sender, EventArgs e)
        {
            string VbTitul, VbTxtToolT, TxtCad, TxtPantIni, NomArc;
            VbTitul = "Exportar Referencias";
            VbTxtToolT = "Ingrese el P/N a consultar";
            TxtPantIni = "~/Forms/InventariosCompras/FrmReferencia.aspx";
            // TxtCad = "EXEC SP_PANTALLA_Informe_Ingenieria 3,'{0}','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
            TxtCad = "";
            NomArc = "Referencia";
            Response.Redirect("~/Forms/FrmExportar.aspx?TT=" + VbTitul + "&ToolT=" + VbTxtToolT + "&NomArch=" + NomArc + "&TCDN=" + TxtCad + "&PantI=" + TxtPantIni);
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string VBQuery;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 11,'{0}','','','','VALIDA',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
                    SqlCommand SCE = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader DAE = SCE.ExecuteReader();
                    if (DAE.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + DAE["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transc = sqlCon.BeginTransaction())
                    {
                        VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 11,'{0}','{1}','','','DELETE',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text, Session["C77U"].ToString());
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
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE EN GENERAL", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void BtnUndCompra_Click(object sender, EventArgs e)
        {
            if (DdlIdent.SelectedValue.Equals("PN") || DdlIdent.SelectedValue.Equals("LOTE"))
            {
                BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                PnlCampos.Visible = false;
                PnlUnidadCompra.Visible = true;
                BindDataCambUMC(ViewState["VbPNSI"].ToString());
                LblCambioPN.Text = "P/N: " + ViewState["VbPNSI"].ToString();
            }
        }
        protected void IbtCerrarUMC_Click(object sender, ImageClickEventArgs e)
        {
            PnlUnidadCompra.Visible = false;
            PnlCampos.Visible = true;
            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
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
            string VBQuery;
            if (ViewState["NewRef"].Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una referencia')", true);
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 12,'{0}','{1}','','{2}','VALIDA',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text, ViewState["NewRef"], DdlIdent.SelectedValue);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlDataReader SDRCR = SqlCd.ExecuteReader();
                if (SDRCR.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + SDRCR["Mensj"].ToString() + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 2,@PNAct,'{0}','{1}','','','','','','',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                        ViewState["NewRef"].ToString(), Session["C77U"].ToString());
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
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en la asignacion')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "Cambio referencia", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

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
        protected void GrdDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtCod.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[1].Text);
                DdlGrupo.Text = GrdDatos.SelectedRow.Cells[5].Text;
                DdlAta.Text = GrdDatos.SelectedRow.Cells[7].Text;
                DdlMod.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[9].Text).Trim();
                DdlUM.Text = GrdDatos.SelectedRow.Cells[11].Text;
                DdlIdent.Text = GrdDatos.SelectedRow.Cells[13].Text;
                TxtDesc.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[3].Text);
                TxtDescEsp.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[4].Text);
                DdlTipo.Text = GrdDatos.SelectedRow.Cells[14].Text;
                TxtInfAd.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[16].Text);
                string VblReparable = GrdDatos.SelectedRow.Cells[17].Text;
                if (VblReparable.Equals("N/A"))
                {
                    RdbSi.Checked = false;
                    RdbNo.Checked = false;
                }
                else if (VblReparable.Equals("S"))
                {
                    RdbSi.Checked = true;
                    RdbNo.Checked = false;
                }
                else
                {
                    RdbSi.Checked = false;
                    RdbNo.Checked = true;
                }
                CkbPos.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[18].Text) == 1 ? true : false;
                CkbCons.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[19].Text) == 1 ? true : false;
                CkbMot.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[20].Text) == 1 ? true : false;
                CkbMay.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[21].Text) == 1 ? true : false;
                CkbApu.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[22].Text) == 1 ? true : false;
                CkbSub.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[23].Text) == 1 ? true : false;
                CkbNiF.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[24].Text) == 1 ? true : false;
                TxtStockM.Text = Convert.ToDouble(GrdDatos.SelectedRow.Cells[25].Text).ToString();
                CkbVerif.Checked = Convert.ToInt32(GrdDatos.SelectedRow.Cells[26].Text) == 1 ? true : false;
                DdlCat.Text = HttpUtility.HtmlDecode(GrdDatos.SelectedRow.Cells[27].Text).Trim(); ;
                BindDataAll(TxtCod.Text, "");
                PerfilesGrid();
                //--------------------------------
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
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BIndDataBusq(TxtBusqueda.Text);
        }
        protected void GrdMan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();

                if (TxtCod.Text.Equals(""))
                {
                    BindDataAll(TxtCod.Text, "");
                    return;
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VbDesc, VBQuery;
                    VbDesc = (GrdMan.FooterRow.FindControl("DdlManPP") as DropDownList).Text.Trim();
                    if (VbDesc == String.Empty)
                    {
                        BindDataAll(TxtCod.Text, "");
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un item')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 7, @Cod, @Ref, @VbUsu,'','INSERT',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbDesc);
                        sqlCmd.Parameters.AddWithValue("@Ref", TxtCod.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@VbUsu", Session["C77U"].ToString());
                        sqlCmd.ExecuteNonQuery();
                        BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdMan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 7,'','','','','DELETE',@id,0,0,0,'01-01-01','02-01-01','03-01-01'";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdMan.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindDataMan(TxtCod.Text);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdMan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','{0}','','','CON',0,0,0,0,'01-01-01','02-01-01','03-01-01'", TxtCod.Text);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlManPP = (e.Row.FindControl("DdlManPP") as DropDownList);
                DdlManPP.DataSource = Cnx.DSET(LtxtSql);
                DdlManPP.DataTextField = "Descripcion";
                DdlManPP.DataValueField = "CodManipulacion";
                DdlManPP.DataBind();
            }
            if ((int)ViewState["VblEliMS"] == 0)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    e.Row.Cells[1].Controls.Remove(imgD);
                }
            }
        }
        protected void GrdMan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdMan.PageIndex = e.NewPageIndex;
            BindDataAll(TxtCod.Text, "");
        }
        protected void GrdPN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();

                if (e.CommandName.Equals("AddNew"))
                {
                    string VblPN, VblEstado, VblUMC, VblTxtEqui;
                    float VblEqui;

                    VblTxtEqui = (GrdPN.FooterRow.FindControl("TxtEquPP") as TextBox).Text.Trim();
                    while (VblTxtEqui.Contains("."))
                    {
                        VblTxtEqui = VblTxtEqui.Replace(".", ",");

                    }
                    VblPN = (GrdPN.FooterRow.FindControl("TxtPNPP") as TextBox).Text.Trim();
                    VblEstado = (GrdPN.FooterRow.FindControl("DdlEstPNPP") as DropDownList).SelectedValue.Trim();
                    VblUMC = (GrdPN.FooterRow.FindControl("DdlUMComPP") as DropDownList).SelectedValue.Trim();
                    if ((GrdPN.FooterRow.FindControl("TxtEquPP") as TextBox).Text.Trim().Length == 0)
                    {
                        VblEqui = 1;
                    }
                    else
                    {
                        VblEqui = (float)Convert.ToDouble(VblTxtEqui);
                    }
                    ValidarPN(VblPN, "", VblEstado, 0);
                    if (Session["VlRefer"].Equals("N"))
                    {
                        BindDataAll(TxtCod.Text, VblPN);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
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
                        string VbLMensj = TblPN.GetMensj();
                        if (!VbLMensj.Trim().Equals("S"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection SCnxPln = new SqlConnection(Cnx.GetConex()))
                            {
                                sqlCon.Open();
                                string VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 15,'{0}','{1}','{2}','','',0,0,0,0,'01-01-01','02-01-01','03-01-01'",
                                 VblPN.Trim(),Session["C77U"].ToString(), TxtCod.Text.Trim());
                                using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon))
                                {
                                    try
                                    {
                                        sqlCmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                                        Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString(), "PLANOS Nuevo P/N", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                    }
                                }
                            }
                        }
                        BindDataAll(TxtCod.Text, VblPN);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["VbPNSI"] = GrdPN.DataKeys[this.GrdPN.SelectedIndex][0].ToString();
            if (!ViewState["VbPNSI"].ToString().Equals(""))
            {
                BtnUndCompra.Enabled = true;
                BtnCambioRef.Enabled = true;
            }
            else { BindDataAll(TxtCod.Text, ""); }

            BindDataCont(ViewState["VbPNSI"].ToString());
            BindDataMan(TxtCod.Text);
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
            // PerfilesGrid();
        }
        protected void GrdPN_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdPN.EditIndex = e.NewEditIndex;
            BindDataAll(TxtCod.Text, "");
        }
        protected void GrdPN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
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
                {
                    BindDataAll(TxtCod.Text, VblPN);
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
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
                    BindDataAll(TxtCod.Text, VblPN);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmPosicion", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }

        }
        protected void GrdPN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdPN.EditIndex = -1;
            BindDataAll(TxtCod.Text, "");
        }
        protected void GrdPN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VBQuery, VbCod, VbCodExt, VblUMCD;
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
                    VBQuery = string.Format("EXEC SP_TablasIngenieria 1,'',@PN,'{0}','','','','','','VALIDA',0,0,0,0,0,0,'01-01-1','02-01-1','03-01-1'", TxtCod.Text);

                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    Comando.Parameters.AddWithValue("@PN", VbCod);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + registro["Mensaje"].ToString() + "')", true);
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
                            VBQuery = string.Format("EXEC SP_TablasIngenieria 1,'{7}',@PN,'{1}','{3}','{4}','{6}','{8}','{9}','DELETE',@equi,{5},0,0,0,0,'01-01-1','02-01-1','03-01-1'",
                            VbCod, TxtCod.Text, VblEqui, DdlUM.SelectedValue, VblUMCD, VblDCMy, TxtDesc.Text, VbCodExt, DdlAta.SelectedValue, Session["C77U"].ToString());
                            SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac);
                            sqlCmd.Parameters.AddWithValue("@equi", VblEqui);
                            sqlCmd.Parameters.AddWithValue("@PN", VblEqui);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            ViewState["VbPNSI"] = "";
                            BindDataAll(TxtCod.Text, VbCod);
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                            Transac.Rollback();
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    string LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','EST',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                    DropDownList DdlEstPNPP = (e.Row.FindControl("DdlEstPNPP") as DropDownList);
                    DdlEstPNPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlEstPNPP.DataTextField = "Descripcion";
                    DdlEstPNPP.DataValueField = "CodEstadoPn";
                    DdlEstPNPP.DataBind();

                    LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','','{0}','','UMC',0,0,0,0,'01-01-01','02-01-01','03-01-01'", DdlUM.SelectedValue);
                    DropDownList DdlUMComPP = (e.Row.FindControl("DdlUMComPP") as DropDownList);
                    DdlUMComPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlUMComPP.DataTextField = "Descripcion";
                    DdlUMComPP.DataValueField = "CodUnidMedida";
                    DdlUMComPP.DataBind();

                    TextBox TxtEquPP = (e.Row.FindControl("TxtEquPP") as TextBox);

                    if (DdlIdent.SelectedValue.Equals("SN"))
                    {
                        DdlUMComPP.Enabled = false;
                        TxtEquPP.Enabled = false;
                        // DdlUMDespPP.Enabled = false;

                        DdlUMComPP.Text = "EA";
                        TxtEquPP.Text = "1";
                        // DdlUMDespPP.Text = "EA";
                    }

                    LtxtSql = "EXEC SP_PANTALLA_Referencia 12,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    DropDownList DdlFabPP = (e.Row.FindControl("DdlFabPP") as DropDownList);
                    DdlFabPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlFabPP.DataTextField = "Nombre";
                    DdlFabPP.DataValueField = "CodFabricante";
                    DdlFabPP.DataBind();
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox TxtEqu = (e.Row.FindControl("TxtEqu") as TextBox);
                    string LtxtSql = "EXEC SP_PANTALLA_ReferenciaV2 3,'','','','','EST',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                    DropDownList DdlEstPN = (e.Row.FindControl("DdlEstPN") as DropDownList);
                    DdlEstPN.DataSource = Cnx.DSET(LtxtSql);
                    DdlEstPN.DataTextField = "Descripcion";
                    DdlEstPN.DataValueField = "CodEstadoPn";
                    DdlEstPN.DataBind();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DdlEstPN.SelectedValue = dr["CodEstadoPn"].ToString();

                    LtxtSql = "EXEC SP_PANTALLA_Referencia 12,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                    DropDownList DdlFab = (e.Row.FindControl("DdlFab") as DropDownList);
                    DdlFab.DataSource = Cnx.DSET(LtxtSql);
                    DdlFab.DataTextField = "Nombre";
                    DdlFab.DataValueField = "CodFabricante";
                    DdlFab.DataBind();
                    DdlFab.SelectedValue = dr["CodFabricante"].ToString();

                    TextBox TxtPN = (e.Row.FindControl("TxtPN") as TextBox);
                    ViewState["VbPNSI"] = TxtPN.Text;

                    LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','','',@PN,'UMCMOD',0,0,0,0,'01-01-01','02-01-01','03-01-01'");
                    DropDownList DdlUMCom = (e.Row.FindControl("DdlUMCom") as DropDownList);
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlCommand cmd = new SqlCommand(LtxtSql, sqlCon))
                        {
                            cmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GrdPN, "Select$" + e.Row.RowIndex);
            }
        }
        protected void GrdPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdPN.PageIndex = e.NewPageIndex;
            BindDataAll(TxtCod.Text, "");
        }
        protected void GrdCont_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                PerfilesGrid();

                if (TxtCod.Text.Equals(""))
                {
                    BindDataAll(TxtCod.Text, "");
                    return;
                }
                if (e.CommandName.Equals("AddNew"))
                {
                    string VbContad, VBQuery;
                    VbContad = (GrdCont.FooterRow.FindControl("DdlContPP") as DropDownList).Text.Trim();
                    if (VbContad == String.Empty)
                    {
                        BindDataAll(TxtCod.Text, "");
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un contador')", true);
                        return;
                    }
                    if (ViewState["VbPNSI"].ToString() == string.Empty)
                    {
                        BindDataAll(TxtCod.Text, "");
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un P/N')", true);
                        return;
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 9,'{0}','{1}','{2}','','INSERT',0,0,0,0,'01-01-01','02-01-01','03-01-01'",
                            VbContad, ViewState["VbPNSI"].ToString(), Session["C77U"].ToString());
                        SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                        sqlCmd.ExecuteNonQuery();
                        BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT CONTADOR", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdCont_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string VblCodCtd = GrdCont.DataKeys[e.RowIndex].Values["CodContador"].ToString();

                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    Cnx2.Open();
                    string LtxtSql = string.Format(" EXEC SP_PANTALLA_ReferenciaV2 9,'{0}','{1}','','','VALIDA',{2},0,0,0,'01-01-01','02-01-01','03-01-01'",
                         VblCodCtd, ViewState["VbPNSI"].ToString(), GrdCont.DataKeys[e.RowIndex].Value.ToString());
                    SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                    SqlDataReader tbl = SqlC.ExecuteReader();
                    if (tbl.Read())
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('" + tbl["Mensj"].ToString() + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_ReferenciaV2 9,'','','','','DELETE',@id,0,0,0,'01-01-01','02-01-01','03-01-01'";
                    SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", GrdCont.DataKeys[e.RowIndex].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Manipulación", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdCont_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            if (!TxtCod.Text.Equals(""))
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_ReferenciaV2 3,'','','','{0}','CTD',0,0,0,0,'01-01-01','02-01-01','03-01-01'", ViewState["VbPNSI"].ToString());
                    DropDownList DdlContPP = (e.Row.FindControl("DdlContPP") as DropDownList);
                    DdlContPP.DataSource = Cnx.DSET(LtxtSql);
                    DdlContPP.DataTextField = "CodContador";
                    DdlContPP.DataValueField = "Cod";
                    DdlContPP.DataBind();
                }
            }
        }
        protected void GrdCont_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCont.PageIndex = e.NewPageIndex;
            BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
        }
        protected void GrdCamUC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
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
                    {
                        VblEqui = 1;
                    }
                    else
                    {
                        VblEqui = (float)Convert.ToDouble(VblTxtEqui);
                    }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        using (SqlTransaction Transac = sqlCon.BeginTransaction())
                        {
                            VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{1}','{2}','{3}','INSERT',@Equi,0,0,0,'01-01-01','02-01-01','03-01-01'",
                            "", VblUMC, DdlUM.SelectedValue, Session["C77U"].ToString());
                            using (SqlCommand SqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                            {
                                try
                                {
                                    SqlCmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                                    SqlCmd.Parameters.AddWithValue("@Equi", VblEqui);
                                    SqlCmd.ExecuteNonQuery();
                                    Transac.Commit();
                                    BindDataCambUMC(VblPN);
                                    // BindDataAll(TxtCod.Text, VblPN);
                                }
                                catch (Exception ex)
                                {
                                    Transac.Rollback();
                                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en el ingreso')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "INSERT PN", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
        protected void GrdCamUC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GrdCamUC_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdCamUC.EditIndex = e.NewEditIndex;
            BindDataCambUMC(ViewState["VbPNSI"].ToString());
        }
        protected void GrdCamUC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string VBQuery, VbUCMod, VblTxtEqui;
            float VbequiCUC;

            VblTxtEqui = (GrdCamUC.Rows[e.RowIndex].FindControl("TxtCEqu") as TextBox).Text.Trim();
            VblTxtEqui = VblTxtEqui.Replace(".", ",");

            VbequiCUC = (float)Convert.ToDouble(VblTxtEqui);
            VbUCMod = (GrdCamUC.Rows[e.RowIndex].FindControl("TxtCUMC") as TextBox).Text.Trim();
            if (VbUCMod.Equals(DdlUM.SelectedValue))
            {
                VbequiCUC = 1;

            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','','VALIDA',@Equi, 0,0,0,'01-01-01','02-01-01','03-01-01'", VbUCMod);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlCd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                SqlCd.Parameters.AddWithValue("@Equi", VbequiCUC);
                SqlDataReader DRCUM = SqlCd.ExecuteReader();
                if (DRCUM.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + DRCUM["Mensj"].ToString() + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','{2}','UPDATE',@Equi,{1},0,0,'01-01-01','02-01-01','03-01-01'",
                     VbUCMod, GrdCamUC.DataKeys[e.RowIndex].Value.ToString(), Session["C77U"].ToString());
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Equi", VbequiCUC);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdCamUC.EditIndex = -1;
                            BindDataCambUMC(ViewState["VbPNSI"].ToString());/* */
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en la actualización')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "UPDATE Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                        }
                    }
                }
            }
        }
        protected void GrdCamUC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdCamUC.EditIndex = -1;
            BindDataCambUMC(ViewState["VbPNSI"].ToString());
        }
        protected void GrdCamUC_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
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
                VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','','VALIDA',@Equi, 0,0,0,'01-01-01','02-01-01','03-01-01'", VbUCMod);
                SqlCommand SqlCd = new SqlCommand(VBQuery, sqlCon);
                SqlCd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                SqlCd.Parameters.AddWithValue("@Equi", VbequiCUC);
                SqlDataReader DRCUM = SqlCd.ExecuteReader();
                if (DRCUM.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('" + DRCUM["Mensj"].ToString() + "')", true);
                    return;
                }
            }
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = string.Format("EXEC SP_PANTALLA_ReferenciaV2 4,@PN,'{0}','','{2}','DELETE',@Equi,{1},0,0,'01-01-01','02-01-01','03-01-01'",
                     VbUCMod, GrdCamUC.DataKeys[e.RowIndex].Value.ToString(), Session["C77U"].ToString());
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        try
                        {
                            sqlCmd.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Equi", VbequiCUC);
                            sqlCmd.ExecuteNonQuery();
                            Transac.Commit();
                            GrdCamUC.EditIndex = -1;
                            BindDataCambUMC(ViewState["VbPNSI"].ToString());/* */
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Error en la eliminación')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmReferencia", "DELETE Unidad Compra", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());

                        }
                    }
                }
            }
        }
        protected void GrdCamUC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();

            string LtxtSql = string.Format("EXEC SP_PANTALLA_Referencia 30,'{0}',@PN,'','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", DdlUM.SelectedValue);
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DdlCUMCPP = (e.Row.FindControl("DdlCUMCPP") as DropDownList);
                Cnx.SelecBD();
                using (SqlConnection Cx = new SqlConnection(Cnx.GetConex()))
                {
                    Cx.Open();
                    using (SqlCommand SC = new SqlCommand(LtxtSql, Cx))
                    {
                        SC.Parameters.AddWithValue("@PN", ViewState["VbPNSI"].ToString());
                        DdlCUMCPP.DataSource = SC.ExecuteReader();
                        DdlCUMCPP.DataTextField = "CodUnidMedida";
                        DdlCUMCPP.DataValueField = "CodUnidMedida";
                        DdlCUMCPP.DataBind();
                    }
                }
                TextBox TxtCUDPP = (e.Row.FindControl("TxtCUDPP") as TextBox);
                TxtCUDPP.Text = DdlUM.SelectedValue;
            }
        }
        protected void GrdCamUC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCamUC.PageIndex = e.NewPageIndex;
            BindDataCambUMC(ViewState["VbPNSI"].ToString());
            //BindDataAll(TxtCod.Text, ViewState["VbPNSI"].ToString());
        }
        protected void GrdCambioRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RdbRefCRef.Checked == true)
                {
                    LblRefCambRef.Text = "Referencia: " + HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
                    ViewState["NewRef"] = HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
                }

                if (RdbPnCRef.Checked == true)
                {
                    LblRefCambRef.Text = "Referencia: " + HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[2].Text);
                    ViewState["NewRef"] = HttpUtility.HtmlDecode(GrdCambioRef.SelectedRow.Cells[1].Text);
                }
            }
            catch (Exception Ex)
            {
                string Mnsj = Ex.Message.Substring(0, 300);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente; " + Mnsj.ToString() + "')", true);
                ScriptManager.RegisterClientScriptBlock(this.UpPnlUndCompra, UpPnlUndCompra.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente con la consulta')", true);
            }
        }
        protected void GrdCambioRef_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void GrdCambioRef_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCambioRef.PageIndex = e.NewPageIndex;
            BindDataCambioRef();
        }
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
                //adjust footer height based on white space removal between footer and last row
                height += 30;
                gv.Height = new Unit(height);
            }
        }
    }
}