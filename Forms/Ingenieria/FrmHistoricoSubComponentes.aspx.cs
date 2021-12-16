using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmHistoricoSubComponentes : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTPN = new DataTable();
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
                ModSeguridad();
                BindPn("");
                BindSn("");
                ViewState["Tipo"] = "";
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
                    BtnConsultar.Text = bO.Equals("BtnConsultar") ? bT : BtnConsultar.Text;
                    if (bO.Equals("TxtDescPlHl"))
                    { TxtDesc.Attributes.Add("placeholder", bT); }
                    BtnSubComp.Text = bO.Equals("BtnSubComp") ? bT : BtnSubComp.Text;
                    BtnMayor.Text = bO.Equals("BtnMayor") ? bT : BtnMayor.Text;
                    GrdHistor.Columns[0].HeaderText = bO.Equals("GrdPrcs") ? bT : GrdHistor.Columns[0].HeaderText;
                    GrdHistor.Columns[3].HeaderText = bO.Equals("GrdAcc") ? bT : GrdHistor.Columns[3].HeaderText;
                    GrdHistor.Columns[4].HeaderText = bO.Equals("GrdFec") ? bT : GrdHistor.Columns[4].HeaderText;
                    GrdHistor.Columns[5].HeaderText = bO.Equals("GrdPosc") ? bT : GrdHistor.Columns[5].HeaderText;
                    GrdHistor.Columns[6].HeaderText = bO.Equals("GrdUbTec") ? bT : GrdHistor.Columns[6].HeaderText;
                    GrdHistor.Columns[7].HeaderText = bO.Equals("GrdPnMy") ? bT : GrdHistor.Columns[7].HeaderText;
                    GrdHistor.Columns[8].HeaderText = bO.Equals("GrdSnMy") ? bT : GrdHistor.Columns[8].HeaderText;
                    GrdHistor.Columns[9].HeaderText = bO.Equals("GrdMtvo") ? bT : GrdHistor.Columns[9].HeaderText;
                    GrdHistor.Columns[10].HeaderText = bO.Equals("GrdFecMvt") ? bT : GrdHistor.Columns[10].HeaderText;
                    GrdHistor.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdHistor.EmptyDataText;
                    //**************** Procesar un elemento ****************************
                    IbtCerrarProces.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarProces.ToolTip;
                    LblTitProcsCont.Text = bO.Equals("LblTitProcsCont") ? bT : LblTitProcsCont.Text;
                    LblPrcsSnMy.Text = bO.Equals("LblPrcsSnMy") ? bT : LblPrcsSnMy.Text;
                    LblPrcsFecMyr.Text = bO.Equals("LblPrcsFecMyr") ? bT : LblPrcsFecMyr.Text;
                    LblPrcsFecHast.Text = bO.Equals("LblPrcsFecHast") ? bT : LblPrcsFecHast.Text;
                    LblPrcsCont.Text = bO.Equals("LblPrcsCont") ? bT : LblPrcsCont.Text;
                    BtnPrcsConsult.Text = bO.Equals("BtnPrcsConsult") ? bT : BtnPrcsConsult.Text;
                    BtnPrcsCont.Text = bO.Equals("GrdPrcs") ? bT : BtnPrcsCont.Text;
                    GrdProcesar.Columns[0].HeaderText = bO.Equals("GrdFecPrc") ? bT : GrdProcesar.Columns[0].HeaderText;
                    GrdProcesar.Columns[1].HeaderText = bO.Equals("GrdIndvPrc") ? bT : GrdProcesar.Columns[1].HeaderText;
                    GrdProcesar.Columns[2].HeaderText = bO.Equals("GrdAcumPrc") ? bT : GrdProcesar.Columns[2].HeaderText;
                    GrdProcesar.Columns[3].HeaderText = bO.Equals("GrdLVPrc") ? bT : GrdProcesar.Columns[3].HeaderText;
                    GrdProcesar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdProcesar.EmptyDataText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
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
        protected void BindPn(string Tipo)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_ADVICE 16,'','HIRSC',@Tp,'PN',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(LtxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@Tp", Tipo.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DTPN);
                    ViewState["DTPN"] = DTPN;

                    DdlPN.DataSource = DTPN;
                    DdlPN.DataTextField = "PN";
                    DdlPN.DataValueField = "Codigo";
                    DdlPN.DataBind();
                }
            }
        }
        protected void BindSn(string PN)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_ADVICE 16, @P,'','','SN',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@P", PN.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        using (DataSet DSTDdl = new DataSet())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DSTDdl);
                            DdlSN.DataSource = DSTDdl.Tables[0];
                            DdlSN.DataTextField = "SN";
                            DdlSN.DataValueField = "Codigo";
                            DdlSN.DataBind();
                        }
                    }
                }
            }
        }
        protected void BtnSubComp_Click(object sender, EventArgs e)
        {
            BtnSubComp.CssClass = "btn btn-primary";
            BtnMayor.CssClass = "btn btn-outline-primary";
            ViewState["Tipo"] = "SC";
            BindPn("SC");
            BindSn("");
            TxtDesc.Text = "";
            GrdHistor.DataSource = null;
            GrdHistor.DataBind();
        }
        protected void BtnMayor_Click(object sender, EventArgs e)
        {
            BtnMayor.CssClass = "btn btn-primary";
            BtnSubComp.CssClass = "btn btn-outline-primary";
            ViewState["Tipo"] = "MY";
            BindPn("MY");
            BindSn("");
            TxtDesc.Text = "";
            GrdHistor.DataSource = null;
            GrdHistor.DataBind();
        }
        protected void DdlPN_TextChanged(object sender, EventArgs e)
        {
            BindSn(DdlPN.Text.Trim()); PerfilesGrid();
            DTPN = (DataTable)ViewState["DTPN"];

            DataRow[] Result = DTPN.Select("Codigo= '" + DdlPN.Text.Trim() + "'");
            foreach (DataRow SDR in Result)
            { TxtDesc.Text = SDR["Descripcion"].ToString().Trim(); }
        }
        protected void BindHistorico()
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = " EXEC SP_PANTALLA_Formulario_Historico 10,@E,@T,'','',0,0,0,@ICC,'01-01-01','01-01-01','01-01-01'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@T", ViewState["Tipo"]);
                    SC.Parameters.AddWithValue("@E", DdlSN.Text.Trim());
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
                        catch (Exception Ex)
                        {
                            string borr = Ex.ToString();
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIR'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// 
                        }
                    }
                }
            }
        }
        protected void DdlSN_TextChanged(object sender, EventArgs e)
        { BindHistorico(); }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        { BindHistorico(); }
        protected void GrdHistor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Procesar"))
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                string VbFecF = "";
                if (DdlSN.Text.Trim().Equals(""))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'XX'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe seleccionar una serie.
                    return;
                }
                TxtPrcsPn.Text = DdlPN.Text.Trim();
                TxtPrcsSn.Text = DdlSN.SelectedItem.Text.Trim();
                GridViewRow row2 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ViewState["CEMYR"] = GrdHistor.DataKeys[row2.RowIndex].Values["CodElemMayor"].ToString().Trim();
                TxtPrcsSnMy.Text = ((Label)row2.FindControl("LblSNMyP")).Text.ToString().Trim();
                string VbFecI = ((Label)row2.FindControl("LblFecEvP")).Text.ToString().Trim();
                DateTime VbFecID = Convert.ToDateTime(VbFecI);
                TxtPrcsFecMyr.Text = String.Format("{0:yyyy-MM-dd}", VbFecID);

                Cnx.SelecBD();
                using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
                {
                    SCnx.Open();
                    string LtxtSql = "EXEC SP_PANTALLA_Formulario_Historico 8, @S,'','','SubComp',0,0,0,@ICC,@F,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(LtxtSql, SCnx);
                    SC.Parameters.AddWithValue("@S", DdlSN.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@F", VbFecI);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read()) { VbFecF = SDR["FechaEvento"].ToString(); }
                    if (VbFecF.Equals(""))
                    {
                        //TxtFechHast.Text = VbFecF;
                        DateTime VbFecFD = DateTime.Now;
                        TxtPrcsFecHast.Text = String.Format("{0:yyyy-MM-dd}", VbFecFD);
                    }
                    else
                    {
                        DateTime VbFecFD = Convert.ToDateTime(VbFecF);
                        TxtPrcsFecHast.Text = String.Format("{0:yyyy-MM-dd}", VbFecFD);
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
                        string VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 12, @Mtv,@P,@S,@Us,@Id,0,@ICC,1,@FV,'01-01-1900','01-01-1900'";
                        DateTime? FechaEvento = Convert.ToDateTime("01/01/1900");
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                int VbId = Convert.ToInt32(GrdHistor.DataKeys[e.RowIndex].Values["CodIdHisMayor"].ToString());
                                if (!(GrdHistor.Rows[e.RowIndex].FindControl("TxtFecEv") as TextBox).Text.Equals(""))
                                { FechaEvento = Convert.ToDateTime((GrdHistor.Rows[e.RowIndex].FindControl("TxtFecEv") as TextBox).Text); }

                                SC.Parameters.AddWithValue("@Id", VbId);
                                SC.Parameters.AddWithValue("@Mtv", (GrdHistor.Rows[e.RowIndex].FindControl("TxtMotivo") as TextBox).Text.Trim());
                                SC.Parameters.AddWithValue("@P", (GrdHistor.Rows[e.RowIndex].FindControl("LblPn") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@S", (GrdHistor.Rows[e.RowIndex].FindControl("LblSn") as Label).Text.Trim());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SC.Parameters.AddWithValue("@FV", FechaEvento);
                                SC.ExecuteNonQuery();
                                Transac.Commit();
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
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Editar Historico I-R SubC", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Editar Historico I-R SubC", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 12,'','','',@Us,@Id,0,@ICC,2,'01-1-2009','01-01-1900','01-01-1900'";
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
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "DELETE Historico I/R SubComp", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
                string VbIR = dr["Identificador"].ToString();
                if (VbIR.Equals("I")) { e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow; }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    if (IbtProcesar != null)
                    { IbtProcesar.Visible = false; }
                }
                if (ViewState["Tipo"].ToString().Equals("MY"))
                {
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

                ImageButton IbnFecEv = (e.Row.FindControl("IbnFecEv") as ImageButton);
                if ((int)ViewState["VblCE6"] == 0) { IbnFecEv.Visible = false; }//Editar fecha
            }
        }
        //*********************Panel de procesar ******************************************
        protected void BindCont()
        {
            string LtxtSql = "";
            if (!TxtPrcsPn.Text.Trim().Equals(""))
            {
                LtxtSql = string.Format("EXEC SP_PANTALLA_Informe_Ingenieria 16,'{0}','','','ContPN',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", TxtPrcsPn.Text.Trim(), Session["!dC!@"]);
                DdlPrcsCont.DataSource = Cnx.DSET(LtxtSql);
                DdlPrcsCont.DataTextField = "CodContador";
                DdlPrcsCont.DataValueField = "CodContador";
                DdlPrcsCont.DataBind();
            }
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
                    SC.Parameters.AddWithValue("@Ct", DdlPrcsCont.Text.Trim());
                    SC.Parameters.AddWithValue("@CE", DdlSN.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtPrcsFecMyr.Text.Trim()));
                    SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtPrcsFecHast.Text.Trim()));
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
        protected void IbtCerrarProces_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BtnPrcsConsult_Click(object sender, EventArgs e)
        { BIndProcesar(); BtnPrcsCont.Enabled = true; }
        protected void ValidarConsulta()
        {
            ViewState["Validar"] = "S";
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (TxtPrcsFecMyr.Text.Equals("") || TxtPrcsFecHast.Text.Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIRSC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            if (TxtPrcsFecMyr.Text.Length > 10 || TxtPrcsFecHast.Text.Length > 10)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIRSC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            DateTime FechaI = Convert.ToDateTime(TxtPrcsFecMyr.Text);
            DateTime FechaF = Convert.ToDateTime(TxtPrcsFecHast.Text);
            int Comparar = DateTime.Compare(FechaF, FechaI);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02HIRSC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            FechaI = Convert.ToDateTime(TxtPrcsFecMyr.Text);
            FechaF = Convert.ToDateTime("01/01/1900");
            Comparar = DateTime.Compare(FechaI, FechaF);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIRSC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
            FechaI = Convert.ToDateTime(TxtPrcsFecHast.Text);
            FechaF = Convert.ToDateTime("01/01/1900");
            Comparar = DateTime.Compare(FechaI, FechaF);
            if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01HIRSC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//fecha inválida
                ViewState["Validar"] = "N"; return;
            }
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
                    string VBQuery = "EXEC SP_PANTALLA_Formulario_Historico 13,@CE,@CEM,@CT,@Us,0,0,0,@ICC,@FI,@FF,'01-01-1900'";

                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        string borrar = ViewState["CEMYR"].ToString().Trim();
                        DateTime b2 = Convert.ToDateTime(TxtPrcsFecHast.Text.Trim());
                        SC.Parameters.AddWithValue("@CE", DdlSN.Text.Trim());
                        SC.Parameters.AddWithValue("@CEM", ViewState["CEMYR"].ToString().Trim());
                        SC.Parameters.AddWithValue("@CT", DdlPrcsCont.Text.Trim());
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        SC.Parameters.AddWithValue("@FI", Convert.ToDateTime(TxtPrcsFecMyr.Text.Trim()));
                        SC.Parameters.AddWithValue("@FF", Convert.ToDateTime(TxtPrcsFecHast.Text.Trim()));
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
                            BIndProcesar();
                            BtnPrcsCont.Enabled = false;
                        }
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens03HIRSC'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Inconvenientes en el proceso.

                            Transac.Rollback();
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Procesar contador en el historico Ins-Remoc SubComp", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
    }
}