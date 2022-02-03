using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class Inf_FrmComponenteRemovidoDeAeronave : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
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
                ModSeguridad();
                string LtxtSql = string.Format("EXEC SP_PANTALLA_Status 11,'','','','HK',0,0,0,{0},'01-1-2009','01-01-1900','01-01-1900'", Session["!dC!@"]);
                DdlAk.DataSource = Cnx.DSET(LtxtSql);
                DdlAk.DataTextField = "Matricula";
                DdlAk.DataValueField = "CodAeronave";
                DdlAk.DataBind();
                BindPn("");
                BindSn("");
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
            ClsP.Acceder(Session["C77U"].ToString(), "Frm_InfIngenieria.aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { } //
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//Editar motivo y procesar contador
            if (ClsP.GetCE3() == 0) { }
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { ViewState["VblCE5"] = 0; }//Eliminar Historico Instalacion Remocion.
            if (ClsP.GetCE6() == 0) { ViewState["VblCE6"] = 0; }//Editar fecha historico instalacion Remocion
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
                    TitForm.Text = bO.Equals("Caption") ? bT : TitForm.Text;
                    LblAk.Text = bO.Equals("LblAk") ? bT + ":" : LblAk.Text;
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    LblFechI.Text = bO.Equals("LblFechI") ? bT + ":" : LblFechI.Text;
                    LblFechF.Text = bO.Equals("LblFechF") ? bT + ":" : LblFechF.Text;
                    GrdHistor.Columns[0].HeaderText = bO.Equals("GrdPrcs") ? bT : GrdHistor.Columns[0].HeaderText;
                    GrdHistor.Columns[1].HeaderText = bO.Equals("LblAk") ? bT : GrdHistor.Columns[1].HeaderText;
                    GrdHistor.Columns[4].HeaderText = bO.Equals("GrdMyr") ? bT : GrdHistor.Columns[4].HeaderText;
                    GrdHistor.Columns[5].HeaderText = bO.Equals("GrdSbC") ? bT : GrdHistor.Columns[5].HeaderText;
                    GrdHistor.Columns[6].HeaderText = bO.Equals("GrdDesc") ? bT : GrdHistor.Columns[6].HeaderText;
                    GrdHistor.Columns[7].HeaderText = bO.Equals("GrdFecEv") ? bT : GrdHistor.Columns[7].HeaderText;
                    GrdHistor.Columns[8].HeaderText = bO.Equals("GrdPosc") ? bT : GrdHistor.Columns[8].HeaderText;
                    GrdHistor.Columns[9].HeaderText = bO.Equals("GrdUbTec") ? bT : GrdHistor.Columns[9].HeaderText;
                    GrdHistor.Columns[10].HeaderText = bO.Equals("GrdEve") ? bT : GrdHistor.Columns[10].HeaderText;
                    GrdHistor.Columns[11].HeaderText = bO.Equals("GrdMtvo") ? bT : GrdHistor.Columns[11].HeaderText;
                    GrdHistor.Columns[12].HeaderText = bO.Equals("GrdFecMvt") ? bT : GrdHistor.Columns[12].HeaderText;
                    GrdHistor.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdHistor.EmptyDataText;
                    //**************** Procesar un elemento ****************************
                    IbtCerrarProces.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarProces.ToolTip;
                    LblTitHisC1Aplicab.Text = bO.Equals("LblTitHisC1Aplicab") ? bT : LblTitHisC1Aplicab.Text;
                    LblHkProcs.Text = bO.Equals("LblAk") ? bT : LblHkProcs.Text;
                    LblFechMyr.Text = bO.Equals("LblFechMyr") ? bT : LblFechMyr.Text;
                    LblFechHast.Text = bO.Equals("LblFechHast") ? bT : LblFechHast.Text;
                    LblContador.Text = bO.Equals("LblContador") ? bT : LblContador.Text;
                    BtnPrcsCont.Text = bO.Equals("GrdPrcs") ? bT : BtnPrcsCont.Text;
                    GrdProcesar.Columns[0].HeaderText = bO.Equals("GrdPrFech") ? bT : GrdProcesar.Columns[0].HeaderText;
                    GrdProcesar.Columns[1].HeaderText = bO.Equals("GrdPrcsIndiv") ? bT : GrdProcesar.Columns[1].HeaderText;
                    GrdProcesar.Columns[2].HeaderText = bO.Equals("GrPrcsAcum") ? bT : GrdProcesar.Columns[2].HeaderText;
                    GrdProcesar.Columns[3].HeaderText = bO.Equals("GrdPrcsLv") ? bT : GrdProcesar.Columns[3].HeaderText;
                    GrdProcesar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdProcesar.EmptyDataText;
                }
                DataRow[] Result1 = Idioma.Select("Objeto= 'BtnPrcsContOnC'");
                foreach (DataRow row in Result1)
                { BtnPrcsCont.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindPn(string CodHk)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_ADVICE 16,'','HIR','{0}','PN',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", CodHk.Trim(), Session["!dC!@"]);
            DdlPN.DataSource = Cnx.DSET(LtxtSql);
            DdlPN.DataTextField = "PN";
            DdlPN.DataValueField = "Codigo";
            DdlPN.DataBind();
        }
        protected void BindSn(string PN)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_ADVICE 16,'{0}','','','SN',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", PN.Trim(), Session["!dC!@"]);
            DdlSN.DataSource = Cnx.DSET(LtxtSql);
            DdlSN.DataTextField = "SN";
            DdlSN.DataValueField = "Codigo";
            DdlSN.DataBind();
        }
        protected void BindHistorico()
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Formulario_Historico 5,@P,@S,'','',@Ak,0,0,@ICC,@FI,@FF,'01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    string VbFI = "", VbFF = "";
                    VbFI = TxtFechI.Text.Trim();
                    VbFF = TxtFechF.Text.Trim();
                    if (VbFI.Equals("")) { VbFI = "01/01/1900"; }
                    if (VbFF.Equals("")) { VbFF = "01/01/1900"; }
                    SC.Parameters.AddWithValue("@Ak", DdlAk.Text.Trim());
                    SC.Parameters.AddWithValue("@P", DdlPN.Text.Trim());
                    SC.Parameters.AddWithValue("@S", DdlSN.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(VbFI));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(VbFF));
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        try
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);
                            if (DtB.Rows.Count > 0)
                            {
                                GrdHistor.DataSource = DtB;
                                GrdHistor.DataBind();
                            }
                            else
                            {
                                GrdHistor.DataSource = null;
                                GrdHistor.DataBind();
                            }
                        }
                        catch
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// 
                        }
                    }
                }
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdHistor.Rows)
            {
                if ((int)ViewState["VblCE2"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[13].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblCE5"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[13].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void DdlAk_TextChanged(object sender, EventArgs e)
        { BindPn(DdlAk.Text.Trim()); BindSn(""); PerfilesGrid(); }
        protected void DdlPN_TextChanged(object sender, EventArgs e)
        { BindSn(DdlPN.Text.Trim()); PerfilesGrid(); }
        protected void DdlSN_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtFechI.Text.Length > 10 || TxtFechF.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// fecha invalida
                return;
            }
            if (!TxtFechI.Text.Trim().Equals("") && TxtFechF.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Campo fecha requerido.
                TxtFechF.Focus(); return;
            }
            if (TxtFechI.Text.Trim().Equals("") && !TxtFechF.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Campo fecha requerido.
                TxtFechI.Focus(); return;
            }
            BindHistorico();
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (TxtFechI.Text.Length > 10 || TxtFechF.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// fecha invalida
                TxtFechI.Focus(); return;
            }
            if (!TxtFechI.Text.Trim().Equals("") && TxtFechF.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Campo fecha requerido.
                TxtFechF.Focus(); return;
            }
            if (TxtFechI.Text.Trim().Equals("") && !TxtFechF.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Campo fecha requerido.
                TxtFechI.Focus(); return;
            }
            BindHistorico();
        }
        //*********************Panel de procesar ******************************************
        protected void BindCont()
        {
            string LtxtSql = "";
            if (!TxtPNProc.Text.Trim().Equals(""))
            {
                LtxtSql = string.Format("EXEC SP_PANTALLA_Informe_Ingenieria 16,'{0}','','','ContPN',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", TxtPNProc.Text.Trim(), Session["!dC!@"]);
                DdlContador.DataSource = Cnx.DSET(LtxtSql);
                DdlContador.DataTextField = "CodContador";
                DdlContador.DataValueField = "CodContador";
                DdlContador.DataBind();
            }
        }
        protected void GrdHistor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Procesar"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VbFecF = "";
                if (DdlSN.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens04HIR'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe seleccionar una serie.
                    return;
                }
                TxtPNProc.Text = DdlPN.Text.Trim();
                TxtSNProc.Text = DdlSN.SelectedItem.Text.Trim();
                GridViewRow row2 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ViewState["CodAeronave"] = GrdHistor.DataKeys[row2.RowIndex].Values["CodAeronave"].ToString().Trim();
                TxtHkProcs.Text = ((Label)row2.FindControl("LblMtrP")).Text.ToString().Trim();
                string VbFecI = ((Label)row2.FindControl("LblFecEvP")).Text.ToString().Trim();
                DateTime VbFecID = Convert.ToDateTime(VbFecI);
                TxtFechMyr.Text = String.Format("{0:yyyy-MM-dd}", VbFecID);

                Cnx.SelecBD();
                using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
                {
                    SCnx.Open();
                    string LtxtSql = "EXEC SP_PANTALLA_Formulario_Historico 8, @S,'','','',0,0,0,@ICC,@F,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(LtxtSql, SCnx);
                    SC.Parameters.AddWithValue("@S", DdlSN.Text.Trim());
                    SC.Parameters.AddWithValue("@F", VbFecI);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read()) { VbFecF = SDR["FechaMontaje"].ToString(); }
                    if (VbFecF.Equals(""))
                    {
                        //TxtFechHast.Text = VbFecF;
                        DateTime VbFecFD = DateTime.Now;
                        TxtFechHast.Text = String.Format("{0:yyyy-MM-dd}", VbFecFD);
                    }
                    else
                    {
                        DateTime VbFecFD = Convert.ToDateTime(VbFecF);
                        TxtFechHast.Text = String.Format("{0:yyyy-MM-dd}", VbFecFD);
                    }
                }
                BindCont();
                BIndProcesar();
                BtnPrcsCont.Enabled = true;
                MultVw.ActiveViewIndex = 1;
            }
        }
        protected void GrdHistor_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdHistor.EditIndex = e.NewEditIndex; BindHistorico(); }
        protected void GrdHistor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            try
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 6, @Mtv,@P,@S,@Us,@Id,@My,0,@ICC,@FV,'01-01-1900','01-01-1900'";
                        DateTime? FechaEvento = Convert.ToDateTime("01/01/1900");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                TextBox TxtFecEv = (GrdHistor.Rows[e.RowIndex].FindControl("TxtFecEv") as TextBox);
                                int VbId = Convert.ToInt32(GrdHistor.DataKeys[e.RowIndex].Values["CodigoIdHistoricoAeronaveVirtual"].ToString());
                                if (!TxtFecEv.Text.Equals(""))
                                { FechaEvento = Convert.ToDateTime(TxtFecEv.Text); }

                                string VbMnsj = Cnx.ValidarFechas2(TxtFecEv.Text.Trim(), "", 1);
                                if (!VbMnsj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + VbMnsj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { VbMnsj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + VbMnsj + "');", true);
                                    Page.Title = ViewState["PageTit"].ToString(); TxtFecEv.Focus();
                                    return;
                                }

                                SC.Parameters.AddWithValue("@Id", VbId);
                                SC.Parameters.AddWithValue("@Mtv", (GrdHistor.Rows[e.RowIndex].FindControl("TxtMotivo") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@P", (GrdHistor.Rows[e.RowIndex].FindControl("LblPn") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@S", (GrdHistor.Rows[e.RowIndex].FindControl("LblSn") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@My", (GrdHistor.Rows[e.RowIndex].FindControl("CkbMayor") as CheckBox).Checked == true ? "1" : "0");
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.Parameters.AddWithValue("@FV", FechaEvento);

                                string Mensj = "";
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { Mensj = ""; }
                                SDR.Close();
                                Transac.Commit();
                                if (!Mensj.ToString().Trim().Equals("")) { }
                                GrdHistor.EditIndex = -1;
                                BindHistorico();
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Editar Motivo Historico I-R", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Editar Motivo Historico I-R", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdHistor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdHistor.EditIndex = -1;
            BindHistorico();
            PerfilesGrid();
        }
        protected void GrdHistor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VBQuery;
            int VbId = Convert.ToInt32(GrdHistor.DataKeys[e.RowIndex].Value.ToString());
            int VbMy = (GrdHistor.Rows[e.RowIndex].FindControl("CkbMayorP") as CheckBox).Checked == true ? 1 : 0;
            int VbSubC = (GrdHistor.Rows[e.RowIndex].FindControl("CkbSubCP") as CheckBox).Checked == true ? 1 : 0;
            if (VbMy + VbSubC > 0)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 7,@Us,'','','',@Id,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@Id", VbId);
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SC.ExecuteNonQuery();
                            Transac.Commit();
                            BindHistorico();
                        }
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Recurso Reporte", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
            PerfilesGrid();
        }
        protected void GrdHistor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
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

                TextBox TxtFecEv = (e.Row.FindControl("TxtFecEv") as TextBox);
                if ((int)ViewState["VblCE6"] == 0) { TxtFecEv.Visible = false; }//Editar fecha

                DataRowView DRV = e.Row.DataItem as DataRowView;
                if (TxtFecEv != null)
                { TxtFecEv.Text = Cnx.ReturnFecha(DRV["FechaMontaje"].ToString().Trim().Equals("") ? "01/01/1900" : DRV["FechaMontaje"].ToString().Trim()); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                ImageButton IbtProcesar = e.Row.FindControl("IbtProcesar") as ImageButton;
                if (IbtProcesar != null)
                {
                    if ((int)ViewState["VblCE2"] == 0) { IbtProcesar.Visible = false; }
                    DataRow[] Result = Idioma.Select("Objeto='IbtProcesar'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtProcesar.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbIR = dr["Evento"].ToString();
                if (VbIR.Equals("I")) { e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow; }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    if (IbtProcesar != null)
                    { IbtProcesar.Visible = false; }
                }

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
                CheckBox CkbSubCP = e.Row.FindControl("CkbSubCP") as CheckBox;
                if (CkbSubCP != null)
                {
                    if (CkbSubCP.Checked == true)
                    {
                        if (imgE != null) { imgE.Visible = false; }
                        if (imgD != null) { imgD.Visible = false; }
                    }
                }
            }
        }
        protected void IbtCerrarProces_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = 0;
        }
        protected void BIndProcesar()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "";
                VbTxtSql = "EXEC SP_PANTALLA__Historico_Contadores 5,@Ct,@CE,'','','',0, 0,0,@ICC,@FI,@FF,'01-01-1'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Ct", DdlContador.Text.Trim());
                    SC.Parameters.AddWithValue("@CE", DdlSN.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechMyr.Text.Trim()));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechHast.Text.Trim()));
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0)
                        {
                            GrdProcesar.DataSource = DtB;
                            GrdProcesar.DataBind();
                        }
                        else
                        {
                            GrdProcesar.DataSource = null;
                            GrdProcesar.DataBind();
                        }
                    }
                }
            }
        }
        protected void ValidarConsulta()
        {
            ViewState["Validar"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtFechMyr.Text.Equals("") || TxtFechHast.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            if (TxtFechMyr.Text.Length > 10 || TxtFechHast.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            DateTime FechaI = Convert.ToDateTime(TxtFechMyr.Text);
            DateTime FechaF = Convert.ToDateTime(TxtFechHast.Text);
            int Comparar = DateTime.Compare(FechaF, FechaI);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            FechaI = Convert.ToDateTime(TxtFechMyr.Text);
            FechaF = Convert.ToDateTime("01/01/1900");
            Comparar = DateTime.Compare(FechaI, FechaF);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            FechaI = Convert.ToDateTime(TxtFechHast.Text);
            FechaF = Convert.ToDateTime("01/01/1900");
            Comparar = DateTime.Compare(FechaI, FechaF);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
        }
        protected void BtnConsulProces_Click(object sender, EventArgs e)
        {
            BIndProcesar();
            BtnPrcsCont.Enabled = true;
        }
        protected void BtnPrcsCont_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ValidarConsulta();
            if (ViewState["Validar"].ToString().Equals("N")) { return; }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 9,@CE,@SN,@CT,@Us,@Hk,0,0,@ICC,@FI,@FF,'01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@CE", DdlSN.Text.Trim());
                        SC.Parameters.AddWithValue("@SN", DdlSN.SelectedItem.Text.Trim());
                        SC.Parameters.AddWithValue("@CT", DdlContador.Text.Trim());
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Hk", ViewState["CodAeronave"]);
                        SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtFechMyr.Text.Trim()));
                        SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtFechHast.Text.Trim()));
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SC.ExecuteNonQuery();
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
                            BIndProcesar();
                            BtnPrcsCont.Enabled = false;
                        }
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens08HIR'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconvenientes en el proceso.

                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Procesar contador en el historico Ins-Remoc", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
    }
}
