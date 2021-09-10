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
    public partial class FrmOrdenTrabajoCerrada : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPs = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Tipo_Aeronave");
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = "00000082"; //00000082|00000133
                    Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = "77NEO01";
                    Session["V$U@"] = "sa";
                    Session["P@$"] = "admindemp";
                    Session["N77U"] = Session["D[BX"];
                     Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "";
                ModSeguridad();
                Traerdatos("0", "UPD");
                MultVw.ActiveViewIndex = 0;
                ViewState["EstadoAnt"] = "";
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            // ViewState["VblImpMS"] = 1;
            ViewState["VblCE1"] = 1;//
            ViewState["VblCE2"] = 1;//
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0)
            { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            //if (ClsP.GetIngresar() == 0) { }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            // if (ClsP.GetConsultar() == 0) { }
            // if (ClsP.GetImprimir() == 0) { }
            // if (ClsP.GetEliminar() == 0) {}
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; DdlStatus.Enabled = false; }// Abrir/cerrar
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; GrdDatos.Visible = false; }// abrir paso
            /*if (ClsP.GetCE3() == 0) { }
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }*/
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
                    LblCodOT.Text = bO.Equals("LblCodOT") ? bT : LblCodOT.Text;
                    LblAplica.Text = bO.Equals("LblAplica") ? bT : LblAplica.Text;
                    LblStatus.Text = bO.Equals("LblStatus") ? bT : LblStatus.Text;
                    CkbCancel.Text = bO.Equals("CkbCancel") ? "&nbsp" + bT : CkbCancel.Text;
                    BtnConsult.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsult.Text;
                    LblTitPasos.Text = bO.Equals("LblTitPasos") ? bT : LblTitPasos.Text;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdPaso") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[1].HeaderText = bO.Equals("GrdDes") ? bT : GrdDatos.Columns[1].HeaderText;
                    GrdDatos.Columns[2].HeaderText = bO.Equals("GrdCodEst") ? bT : GrdDatos.Columns[2].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("LblStatus") ? bT : GrdDatos.Columns[3].HeaderText;
                    LblTitOTPendCerr.Text = bO.Equals("LblTitOTPendCerr") ? bT : LblTitOTPendCerr.Text;
                    GrdOtPendCerrar.Columns[1].HeaderText = bO.Equals("GrdCod") ? bT : GrdOtPendCerrar.Columns[1].HeaderText;
                    GrdOtPendCerrar.Columns[2].HeaderText = bO.Equals("LblAplica") ? bT : GrdOtPendCerrar.Columns[2].HeaderText;
                    //*******************Busqueda***************************
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusqueda.Text = bO.Equals("LblTitOpcBusqueda") ? bT : LblTitOpcBusqueda.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtConsultar.ToolTip = bO.Equals("IbtConsultarTTMst") ? bT : IbtConsultar.ToolTip;

                    GrdBusq.Columns[1].HeaderText = bO.Equals("GrdCod") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("LblAplica") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("LblStatus") ? bT : GrdBusq.Columns[3].HeaderText;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDatos.Rows)
            {
                /* if ((int)ViewState["VblModMS"] == 0)
                 {
                     ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                     if (imgE != null) { Row.Cells[4].Controls.Remove(imgE); }
                 }*/
            }
        }
        protected void Traerdatos(string Prmtr, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                    {
                        Cnx2.Open();
                        using (SqlCommand SC = new SqlCommand("EXEC SP_PANTALLA_OT_Cerradas 4,@Prmtr,'','','',0,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'", Cnx2))
                        {
                            SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            using (SqlDataAdapter SDA = new SqlDataAdapter())
                            {
                                using (DataSet DSTPs = new DataSet())
                                {
                                    SDA.SelectCommand = SC;
                                    SDA.Fill(DSTPs);
                                    DSTPs.Tables[0].TableName = "Datos";
                                    DSTPs.Tables[1].TableName = "Estado";
                                    DSTPs.Tables[2].TableName = "OTAbiertas";
                                    DSTPs.Tables[3].TableName = "DescPasos";
                                    ViewState["DSTPs"] = DSTPs;
                                }
                            }
                        }
                    }
                }
                DSTPs = (DataSet)ViewState["DSTPs"];
                if (DSTPs.Tables[0].Rows.Count > 0) //Datos
                {
                    TxtCodOT.Text = HttpUtility.HtmlDecode(DSTPs.Tables[0].Rows[0]["CodOT"].ToString().Trim());
                    TxtAplica.Text = HttpUtility.HtmlDecode(DSTPs.Tables[0].Rows[0]["Aplicabilidad"].ToString().Trim());
                    DdlStatus.Text = HttpUtility.HtmlDecode(DSTPs.Tables[0].Rows[0]["CodEstOrdTrab1"].ToString().Trim());
                    ViewState["EstadoAnt"] = DdlStatus.Text.Trim();
                    CkbCancel.Checked = Convert.ToBoolean(DSTPs.Tables[0].Rows[0]["CancelOT"].ToString());
                    BindDPasos(TxtCodOT.Text.Trim());
                }
                if (CkbCancel.Checked == true) { DdlStatus.Enabled = false; }
                else { DdlStatus.Enabled = true; }

                DdlStatus.DataSource = DSTPs.Tables[1];
                DdlStatus.DataTextField = "Descripcion";
                DdlStatus.DataValueField = "Codigo";
                DdlStatus.DataBind();
                if (DSTPs.Tables[2].Rows.Count > 0) { GrdOtPendCerrar.DataSource = DSTPs.Tables[2]; GrdOtPendCerrar.DataBind(); } //OT abiertas
                else { GrdOtPendCerrar.DataSource = null; GrdOtPendCerrar.DataBind(); }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

            }
        }
        protected void BindDPasos(string VbOT)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataTable dtbl = new DataTable();
            string VbTxtSql = "EXEC SELECT_Pasos_OT_Cerradas @O,@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8, @ICC, @Idm";

            string VP1 = "", VP2 = "", VP3 = "", VP4 = "", VP5 = "", VP6 = "", VP7 = "", VP8 = "";
            DataRow[] Result = Idioma.Select("Objeto= 'BtnMroInsPre'");
            foreach (DataRow row in Result)
            { VP1 = row["Texto"].ToString().Trim(); }//Inspección Preliminar 
            Result = Idioma.Select("Objeto= 'BtnMroPrDes'");
            foreach (DataRow row in Result)
            { VP2 = row["Texto"].ToString().Trim(); }//Pruebas antes de desarme
            Result = Idioma.Select("Objeto= 'BtnMroRteDes'");
            foreach (DataRow row in Result)
            { VP3 = row["Texto"].ToString().Trim(); }//Reporte del desarme
            Result = Idioma.Select("Objeto= 'BtnMroDanOc'");
            foreach (DataRow row in Result)
            { VP4 = row["Texto"].ToString().Trim(); }//Daños Escondidos
            Result = Idioma.Select("Objeto= 'BtnMroAccCorr'");
            foreach (DataRow row in Result)
            { VP5 = row["Texto"].ToString().Trim(); }//Proceso de Inspección 
            Result = Idioma.Select("Objeto= 'BtnMroPrueF'");
            foreach (DataRow row in Result)
            { VP6 = row["Texto"].ToString().Trim(); }//Prueba Final
            Result = Idioma.Select("Objeto= 'BtnMroCumpl'");
            foreach (DataRow row in Result)
            { VP7 = row["Texto"].ToString().Trim(); }//Cumplido / Verificado
            Result = Idioma.Select("Objeto= 'BtnMroTrabEje'");
            foreach (DataRow row in Result)
            { VP8 = row["Texto"].ToString().Trim(); }//Trabajo Ejecutado


            Cnx.SelecBD();
            using (SqlConnection SCnx = new SqlConnection(Cnx.GetConex()))
            {
                SCnx.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, SCnx))
                {
                    SC.Parameters.AddWithValue("@O", VbOT);
                    SC.Parameters.AddWithValue("@P1", VP1);
                    SC.Parameters.AddWithValue("@P2", VP2);
                    SC.Parameters.AddWithValue("@P3", VP3);
                    SC.Parameters.AddWithValue("@P4", VP4);
                    SC.Parameters.AddWithValue("@P5", VP5);
                    SC.Parameters.AddWithValue("@P6", VP6);
                    SC.Parameters.AddWithValue("@P7", VP7);
                    SC.Parameters.AddWithValue("@P8", VP8);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(dtbl);
                }
            }
            if (dtbl.Rows.Count > 0) { GrdDatos.DataSource = dtbl; GrdDatos.DataBind(); }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                DataRow[] Result1 = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result1)
                { GrdDatos.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void DdlStatus_TextChanged(object sender, EventArgs e)
        {
            if (TxtCodOT.Text.Trim().Equals("")) { return; }
            if ((int)ViewState["VblCE1"] == 0) { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (CkbCancel.Checked == true)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01OTC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La O.T. se encuentra cancelada, no es posible realizar la acción.
                return;
            }
            if (!ViewState["EstadoAnt"].ToString().Trim().Equals(DdlStatus.Text.Trim()) && !DdlStatus.Text.Trim().Equals("")) //Cerrda y se Abre
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_PANTALLA_OT_Cerradas 5,@E,@US,'','',@COT,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@E", DdlStatus.Text.Trim());
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@COT", TxtCodOT.Text.Trim());
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                string PMensj = "", PEstado = ""; ;
                                // var Mensj = SC.ExecuteScalar();
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    PEstado = HttpUtility.HtmlDecode(SDR["EstadoAnt"].ToString().Trim());
                                }
                                SDR.Close();


                                if (!PMensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + PMensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { PMensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + PMensj + "');", true);
                                    Transac.Rollback();
                                    GrdDatos.DataSource = null;
                                    GrdDatos.DataBind();
                                    DdlStatus.Text = ViewState["EstadoAnt"].ToString().Trim();
                                    return;
                                }
                                Transac.Commit();
                                sqlCon.Close();
                                GrdDatos.DataSource = null;
                                GrdDatos.DataBind();
                                ViewState["EstadoAnt"] = PEstado.Trim();
                                Traerdatos(TxtCodOT.Text.Trim(), "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; }
        protected void GrdDatos_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDatos.EditIndex = e.NewEditIndex; BindDPasos(TxtCodOT.Text.Trim()); }
        protected void GrdDatos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VbEstadoP = (GrdDatos.Rows[e.RowIndex].FindControl("DdlCodEstadoP") as DropDownList).Text.Trim();

            if (!VbEstadoP.Trim().Equals("02"))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04OTC'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Solo permite ingresar el estado en proceso.
                return;
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    string VBQuery = "EXEC SP_PANTALLA_OT_Cerradas 6,@O,@US,@Ps,'UPDATE',@ID,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@O", TxtCodOT.Text.Trim());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@Ps", (GrdDatos.Rows[e.RowIndex].FindControl("LblPaso") as Label).Text.Trim());
                        SC.Parameters.AddWithValue("@ID", GrdDatos.DataKeys[e.RowIndex].Value.ToString());
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
                            GrdDatos.EditIndex = -1;
                            BindDPasos(TxtCodOT.Text.Trim());
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + "Pasos", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDatos.EditIndex = -1; BindDPasos(TxtCodOT.Text.Trim()); }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DSTPs = (DataSet)ViewState["DSTPs"];
                DropDownList DdlCodEstadoP = (e.Row.FindControl("DdlCodEstadoP") as DropDownList);
                DdlCodEstadoP.DataSource = DSTPs.Tables[3];
                DdlCodEstadoP.DataTextField = "NombreESO";
                DdlCodEstadoP.DataValueField = "CodEstadoSO";
                DdlCodEstadoP.DataBind();
                DataRowView dr = e.Row.DataItem as DataRowView;
                DdlCodEstadoP.SelectedValue = dr["CodEstado"].ToString();

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
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;

                Label LblCodEstadoP = e.Row.FindControl("LblCodEstadoP") as Label;
                if (imgE != null)
                {
                    if (!LblCodEstadoP.Text.Trim().Equals("05")) { imgE.Enabled = false; }
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void GrdOtPendCerrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdOtPendCerrar.DataKeys[this.GrdOtPendCerrar.SelectedIndex][0].ToString().Trim());
            Traerdatos(vbcod, "UPD");
            PerfilesGrid();
        }
        //******************************* Busqueda **********************************
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BIndDBusqOT()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_OT_Cerradas 3,@Prmtr,'','','',0,0,0,@CC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim());
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
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BIndDBusqOT(); }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vbcod = HttpUtility.HtmlDecode(GrdBusq.DataKeys[this.GrdBusq.SelectedIndex][0].ToString().Trim());
            Traerdatos(vbcod, "UPD");
            MultVw.ActiveViewIndex = 0;
            PerfilesGrid();
        }
    }
}