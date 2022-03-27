using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmCuadroComparativoCotiza : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable TblDetalle = new DataTable();
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
                ModSeguridad();
                RdbBusqSP.Checked = true;
                TxtBusqueda.Focus();
            }
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
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; } // 
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }//
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; BtnAprob.Visible = false; IbtAprPNAll.Visible = false; IbtDesAprAll.Visible = false; } // Aprobar           

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
                    RdbBusqSP.Text = bO.Equals("RdbBusqSP") ? "&nbsp" + bT : RdbBusqSP.Text;
                    RdbBusqCot.Text = bO.Equals("RdbBusqCot") ? "&nbsp" + bT : RdbBusqCot.Text;
                    RdbBusqPet.Text = bO.Equals("RdbBusqPet") ? "&nbsp" + bT : RdbBusqPet.Text;
                    RdbBusqPPT.Text = bO.Equals("RdbBusqPPT") ? "&nbsp" + bT : RdbBusqPPT.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    BtnOpenCompra.Text = bO.Equals("BtnOpenCompra") ? bT : BtnOpenCompra.Text;
                    BtnOpenCompra.ToolTip = bO.Equals("BtnOpenCompraTT") ? bT : BtnOpenCompra.ToolTip;
                    BtnOpenRepa.Text = bO.Equals("BtnOpenRepa") ? bT : BtnOpenRepa.Text;
                    BtnOpenRepa.ToolTip = bO.Equals("BtnOpenRepaTT") ? bT : BtnOpenRepa.ToolTip;
                    IbtAprPNAll.ToolTip = bO.Equals("IbtAprPNAll") ? bT : IbtAprPNAll.ToolTip;
                    IbtDesAprAll.ToolTip = bO.Equals("IbtDesAprAll") ? bT : IbtDesAprAll.ToolTip;
                    BtnAprob.Text = bO.Equals("IbtUpdate") ? bT : BtnAprob.Text;
                    BtnExport.Text = bO.Equals("BtnExportMstr") ? bT : BtnExport.Text;
                    // *************************************************Grid detalle *************************************************                  
                    GrdDet.Columns[0].HeaderText = bO.Equals("GrdApr") ? bT : GrdDet.Columns[0].HeaderText;
                    GrdDet.Columns[1].HeaderText = bO.Equals("GrdAprPPT") ? bT : GrdDet.Columns[1].HeaderText;
                    GrdDet.Columns[3].HeaderText = bO.Equals("FechaMstr") ? bT : GrdDet.Columns[3].HeaderText;
                    GrdDet.Columns[4].HeaderText = bO.Equals("RdbBusqCot") ? bT : GrdDet.Columns[4].HeaderText;
                    GrdDet.Columns[5].HeaderText = bO.Equals("GrdProv") ? bT : GrdDet.Columns[5].HeaderText;
                    GrdDet.Columns[7].HeaderText = bO.Equals("Descripcion") ? bT : GrdDet.Columns[7].HeaderText;
                    GrdDet.Columns[8].HeaderText = bO.Equals("GrdPnAlt") ? bT : GrdDet.Columns[8].HeaderText;
                    GrdDet.Columns[9].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDet.Columns[9].HeaderText;
                    GrdDet.Columns[10].HeaderText = bO.Equals("CantMst") ? bT : GrdDet.Columns[10].HeaderText;
                    GrdDet.Columns[11].HeaderText = bO.Equals("GrdCMC") ? bT : GrdDet.Columns[11].HeaderText;
                    GrdDet.Columns[12].HeaderText = bO.Equals("ValorMstr") ? bT : GrdDet.Columns[12].HeaderText;
                    GrdDet.Columns[13].HeaderText = bO.Equals("GrdMond") ? bT : GrdDet.Columns[13].HeaderText;
                    GrdDet.Columns[14].HeaderText = bO.Equals("GrdDescto") ? bT : GrdDet.Columns[14].HeaderText;
                    GrdDet.Columns[15].HeaderText = bO.Equals("GrdStts") ? bT : GrdDet.Columns[15].HeaderText;
                    GrdDet.Columns[16].HeaderText = bO.Equals("GrdTimEnt") ? bT : GrdDet.Columns[16].HeaderText;
                    GrdDet.Columns[17].HeaderText = bO.Equals("GrdTipPag") ? bT : GrdDet.Columns[17].HeaderText;
                    GrdDet.Columns[18].HeaderText = bO.Equals("GrdLugEntr") ? bT : GrdDet.Columns[18].HeaderText;
                    GrdDet.Columns[19].HeaderText = bO.Equals("LblObsMst") ? bT : GrdDet.Columns[19].HeaderText;
                    GrdDet.Columns[20].HeaderText = bO.Equals("GrdTimEntPPT") ? bT : GrdDet.Columns[20].HeaderText;
                    GrdDet.Columns[21].HeaderText = bO.Equals("GrdTipCOt") ? bT : GrdDet.Columns[21].HeaderText;
                    GrdDet.Columns[22].HeaderText = bO.Equals("GrdPPT") ? bT : GrdDet.Columns[22].HeaderText;
                    GrdDet.Columns[23].HeaderText = bO.Equals("GrdGaranti") ? bT : GrdDet.Columns[23].HeaderText;

                }
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdateOnC'");
                foreach (DataRow row in Result) { BtnAprob.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindCuadroComprt(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTipoDoc = "SP", VbSDoc = "", VbPN = "", VbCotiza = "", VbPEPR = "0";
                double NumPR_PT = 0;
                bool VbRslt = double.TryParse(TxtBusqueda.Text.Trim(), out NumPR_PT);
                VbSDoc = NumPR_PT.ToString();
                VbPN = TxtBusqPN.Text.Trim();
                if (RdbBusqCot.Checked == true) { VbTipoDoc = "CT"; VbCotiza = TxtBusqueda.Text.Trim(); }
                if (RdbBusqSP.Checked == true) { VbTipoDoc = "SP"; VbCotiza = TxtBusqueda.Text.Trim(); }
                if (RdbBusqPet.Checked == true) { VbTipoDoc = "PE"; VbPEPR = VbSDoc.Trim(); }
                if (RdbBusqPPT.Checked == true) { VbTipoDoc = "PR"; VbCotiza = TxtBusqueda.Text.Trim(); }
                if (RdbBusqPN.Checked == true) { VbTipoDoc = "PN"; VbPN = TxtBusqPN.Text.Trim(); }

                if (Accion.Equals("UPD"))
                {
                    Cnx.SelecBD();
                    using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                    {
                        string VbTxtSql = "EXEC Consultas_General_Logistica 29, @Pn, @Doc, @TC, @DPEPR,@Idm,@ICC,'01-01-1','01-01-1'";
                        sqlConB.Open();
                        using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                        {
                            TblDetalle.Clear();
                            SC.Parameters.AddWithValue("@Pn", VbPN);
                            SC.Parameters.AddWithValue("@TC", VbTipoDoc);
                            SC.Parameters.AddWithValue("@Doc", VbCotiza.Trim());
                            SC.Parameters.AddWithValue("@DPEPR", VbPEPR.Trim());
                            SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                            SqlDataAdapter SDA = new SqlDataAdapter();
                            SDA.SelectCommand = SC;
                            SDA.Fill(TblDetalle);
                            ViewState["TblDetalle"] = TblDetalle;
                        }
                    }
                }
            }
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int VbNumReg = TblDetalle.Rows.Count;

            if (TblDetalle.Rows.Count > 0) { GrdDet.DataSource = TblDetalle; }
            else
            { GrdDet.DataSource = null; }
            GrdDet.DataBind();
        }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BindCuadroComprt("UPD"); Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void BtnOpenCompra_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/InventariosCompras/FrmOrdenCompra.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
        protected void BtnOpenRepa_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/InventariosCompras/FrmReparacion.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["TblDetalle"] != null)
            {
                Page.Title = ViewState["PageTit"].ToString().Trim();
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                DataTable TbExportar = new DataTable();
                string VbNomCursorIdioma = "CurExportCuadroComparativo";

                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar(VbNomCursorIdioma, Session["77IDM"].ToString().Trim());

                Cnx.SelecBD();
                using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                {
                    SCX.Open();
                    using (SqlCommand SC = new SqlCommand("ExportCuadroComparativo", SCX))
                    {
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurDet", TblDetalle);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"].ToString());
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@NomCursorIdioma", VbNomCursorIdioma);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataAdapter SDA = new SqlDataAdapter();
                            SDA.SelectCommand = SC;
                            SDA.Fill(TbExportar);

                            TbExportar.TableName = "77NeoWeb";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(TbExportar);
                                Response.Clear();
                                Response.Buffer = true;
                                Response.ContentType = "application/ms-excel";
                                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", ViewState["PageTit"].ToString().Trim()));
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
                        catch (Exception Ex)
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Cuadro Comparativo", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                        }
                    }
                }
            }
        }
        protected void BtnAprob_Click(object sender, EventArgs e)
        {
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                DataTable TblDetCotiza = new DataTable();
                TblDetCotiza.Columns.Add("IdDetCotizacion", typeof(int));
                TblDetCotiza.Columns.Add("Aprobacion", typeof(int));
                TblDetCotiza.Columns.Add("TieneDoc", typeof(string));
                string PMensj = "";

                foreach (GridViewRow GrdRow in GrdDet.Rows) // se recorre la grid para actualizar la Datatable detalle
                {
                    int VbIdDetCot = Convert.ToInt32(GrdDet.DataKeys[GrdRow.RowIndex].Values["IdDetCotizacion"].ToString().Trim());
                    string VbTieneDoc = (GrdRow.FindControl("CkbAprob") as CheckBox).Enabled == true ? "N" : "S";
                    int VbAprob = (GrdRow.FindControl("CkbAprob") as CheckBox).Checked == true ? 1 : 0;

                    if (VbTieneDoc.Equals("N"))
                    {
                        DataRow DR = TblDetalle.AsEnumerable().Where(r => ((int)r["IdDetCotizacion"]).Equals(VbIdDetCot)).First();
                        DR["Aprobacion"] = VbAprob;
                    }
                }
                TblDetalle.AcceptChanges();
                foreach (DataRow DR in TblDetalle.Rows)
                { TblDetCotiza.Rows.Add(DR["IdDetCotizacion"].ToString().Trim(), DR["Aprobacion"].ToString().Trim(), DR["TieneDoc"].ToString().Trim()); }
                TblDetCotiza.AcceptChanges();
                Cnx.SelecBD();
                using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                {
                    SCX.Open();
                    using (SqlTransaction transaction = SCX.BeginTransaction())
                    {
                        using (SqlCommand SC = new SqlCommand("AprobCuadroComparativo", SCX, transaction))
                        {
                            try
                            {
                                SC.CommandType = CommandType.StoredProcedure;
                                SqlParameter Prmtrs = SC.Parameters.AddWithValue("@AprobDetCot", TblDetCotiza);
                                SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"].ToString());
                                SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@NIT", Session["Nit77Cia"].ToString());
                                Prmtrs.SqlDbType = SqlDbType.Structured;
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                SDR.Close();
                                transaction.Commit();
                                BindCuadroComprt("UPD"); Page.Title = ViewState["PageTit"].ToString().Trim();
                            }
                            catch (Exception Ex)
                            {
                                transaction.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Aprobar Cotizacion desde Cuadro Comparativo", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            }
                        }
                    }
                }
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
                    foreach (DataRow Dtll in TblDetalle.Rows)
                    {
                        if (Dtll["TieneDoc"].ToString().Equals("N"))
                        { Dtll["Aprobacion"] = "1"; }
                    }
                }
            }
            GrdDet.DataSource = TblDetalle; GrdDet.DataBind();
        }
        protected void IbtDesAprAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (ViewState["TblDetalle"] != null)
            {
                TblDetalle = (DataTable)ViewState["TblDetalle"];
                if (TblDetalle.Rows.Count > 0)
                {
                    foreach (DataRow Dtll in TblDetalle.Rows)
                    {
                        if (Dtll["TieneDoc"].ToString().Equals("N"))
                        { Dtll["Aprobacion"] = "0"; }
                    }
                }
            }
            GrdDet.DataSource = TblDetalle; GrdDet.DataBind();
        }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CkbAprob = (CheckBox)e.Row.FindControl("CkbAprob");
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbTieneDoc = dr["TieneDoc"].ToString().Trim();

                if (dr["TieneDoc"].ToString().Trim().Equals("Y")) { CkbAprob.Enabled = false; }
                else { CkbAprob.Enabled = true; }

                if (CkbAprob.Checked == true) { e.Row.BackColor = System.Drawing.Color.Silver; }
            }
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
    }
}