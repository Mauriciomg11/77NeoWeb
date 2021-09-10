using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgManto;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Manto
{
    public partial class FrmIncoming : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            } /* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "";
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = "00000082";
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
                BindDataDdl();
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                ViewState["AplicaCiaFechVenc"] = "N";
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,1,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                SC.Parameters.AddWithValue("@F", "INCOMING");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("S")) { ViewState["AplicaCiaFechVenc"] = "S"; } //Aplica Fecha Vence
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
                    LblAlmacen.Text = bO.Equals("LblAlmacen") ? bT : LblAlmacen.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT : LblTipo.Text;
                    BtnConsult.Text = bO.Equals("BtnConsultarGral") ? bT : BtnConsult.Text;
                    IbnExcel.ToolTip = bO.Equals("IbnExcel") ? bT : IbnExcel.ToolTip;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    LblTitUbicaFis.Text = bO.Equals("LblTitUbicaFis") ? bT : LblTitUbicaFis.Text;
                    GrdDatos.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDatos.EmptyDataText;
                    GrdDatos.Columns[0].HeaderText = bO.Equals("GrdAsig") ? bT : GrdDatos.Columns[0].HeaderText;
                    GrdDatos.Columns[3].HeaderText = bO.Equals("GrdLot") ? bT : GrdDatos.Columns[3].HeaderText;
                    GrdDatos.Columns[4].HeaderText = bO.Equals("GrdRef") ? bT : GrdDatos.Columns[4].HeaderText;
                    GrdDatos.Columns[5].HeaderText = bO.Equals("GrdCant") ? bT : GrdDatos.Columns[5].HeaderText;
                    GrdDatos.Columns[6].HeaderText = bO.Equals("GrdUM") ? bT : GrdDatos.Columns[6].HeaderText;
                    GrdDatos.Columns[7].HeaderText = bO.Equals("GrdBod") ? bT : GrdDatos.Columns[7].HeaderText;
                    GrdDatos.Columns[8].HeaderText = bO.Equals("GrdF") ? bT : GrdDatos.Columns[8].HeaderText;
                    GrdDatos.Columns[9].HeaderText = bO.Equals("GrdC") ? bT : GrdDatos.Columns[9].HeaderText;
                    //*******************Asignar***************************
                    IbtCerrarCambioBod.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarCambioBod.ToolTip;
                    LblTitCambioBod.Text = bO.Equals("LblTitCambioBod") ? bT : LblTitCambioBod.Text;
                    LblLote.Text = bO.Equals("GrdLot") ? bT : LblLote.Text;
                    LblBodOrig.Text = bO.Equals("LblBodOrig") ? bT : LblBodOrig.Text;
                    LblBodDest.Text = bO.Equals("LblBodDest") ? bT : LblBodDest.Text;
                    LblCantAct.Text = bO.Equals("LblCantAct") ? bT : LblCantAct.Text;
                    LblCantNew.Text = bO.Equals("LblCantNew") ? bT : LblCantNew.Text;
                    LblTitBodDes.Text = bO.Equals("LblBodDest") ? bT : LblTitBodDes.Text;
                    GrdUbicaDes.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdUbicaDes.EmptyDataText;
                    GrdUbicaDes.Columns[0].HeaderText = bO.Equals("GrdTrasf") ? bT : GrdUbicaDes.Columns[0].HeaderText;
                    GrdUbicaDes.Columns[1].HeaderText = bO.Equals("GrdF") ? bT : GrdUbicaDes.Columns[1].HeaderText;
                    GrdUbicaDes.Columns[2].HeaderText = bO.Equals("GrdC") ? bT : GrdUbicaDes.Columns[2].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDataDdl()
        {
            DataTable DtAll = new DataTable();
            DataTable HK = new DataTable();
            DataTable Tipo = new DataTable();
            string VbTxtSql = "EXEC SP_PANTALLA_Incoming 3,@U,'','','ALL',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@U", Session["C77U"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(DtAll);
                        HK = DtAll.Clone();
                        DataRow[] Result = DtAll.Select("Filtro='ALMA'");
                        foreach (DataRow Row in Result)
                        { HK.ImportRow(Row); }
                        DdlAlmacen.DataSource = HK;
                        DdlAlmacen.DataTextField = "Descr";
                        DdlAlmacen.DataValueField = "Cod";
                        DdlAlmacen.DataBind();

                        Tipo = DtAll.Clone();
                        Result = DtAll.Select("Filtro='Tipo'");
                        foreach (DataRow Row in Result)
                        { Tipo.ImportRow(Row); }
                        DdlTipo.DataSource = Tipo;
                        DdlTipo.DataTextField = "Descr";
                        DdlTipo.DataValueField = "Cod";
                        DdlTipo.DataBind();
                    }
                }
            }
        }
        protected void BIndDatos()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "EXEC SP_PANTALLA_Asignacion 4,@Pr,@T,'','',@Al,0,1,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Al", DdlAlmacen.Text.Trim());
                    SC.Parameters.AddWithValue("@Pr", TxtBusqueda.Text.Trim());
                    SC.Parameters.AddWithValue("@T", DdlTipo.Text.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdDatos.DataSource = DtB; GrdDatos.DataBind(); }
                        else { GrdDatos.DataSource = null; GrdDatos.DataBind(); }
                    }
                }
            }
        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        { BIndDatos(); }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Abrir"))
            {
                MultVw.ActiveViewIndex = 1;
                TxtCantNew.Enabled = true; TxtCantNew.Text = "0";
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //int VbIdx = row.RowIndex;
                string VbCabt = ((Label)row.FindControl("LblCant")).Text.ToString().Trim();
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                if (Convert.ToDouble(VbCabt) <= 0) { return; }

                ViewState["CodIdUbicacion"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodIdUbicacion"].ToString();
                ViewState["CodUbicaBodega"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodUbicaBodega"].ToString();
                ViewState["CodElemento"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodElemento"].ToString();
                ViewState["CodTipoElemento"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodTipoElemento"].ToString();
                ViewState["IdentificadorElem"] = GrdDatos.DataKeys[gvr.RowIndex].Values["IdentificadorElem"].ToString();
                ViewState["Activo"] = GrdDatos.DataKeys[gvr.RowIndex].Values["Activo"].ToString();
                ViewState["CodTercero"] = GrdDatos.DataKeys[gvr.RowIndex].Values["CodTercero"].ToString();
                ViewState["FechaVencimientoR"] = GrdDatos.DataKeys[gvr.RowIndex].Values["FechaVencimientoR"].ToString();
                if (ViewState["AplicaCiaFechVenc"].ToString().Equals("S")) // cuando aplica para la cia fecha vence
                {
                    LblFechI.Visible = true; TxtFechI.Visible = true;
                    if (Convert.ToInt32(ViewState["FechaVencimientoR"]) == 0) { LblFechI.Visible = false; TxtFechI.Visible = false; }
                }
                if (ViewState["IdentificadorElem"].ToString().Equals("SN")) { TxtCantNew.Enabled = false; TxtCantNew.Text = "1"; }
                ViewState["CodBodegaOrig"] = ((Label)row.FindControl("LblCodBod")).Text.ToString().Trim();
                TxtPN.Text = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                TxtSN.Text = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                TxtLote.Text = ((Label)row.FindControl("LblLote")).Text.ToString().Trim();
                TxtBodOrig.Text = ((Label)row.FindControl("LblCodBod")).Text.ToString().Trim();
                TxtCantAct.Text = VbCabt.ToString();
                TxtUndM.Text = ((Label)row.FindControl("LblUndM")).Text.ToString().Trim();
                BindDDdlDestino(ViewState["CodTercero"].ToString().Trim());
                Page.Title = ViewState["PageTit"].ToString();
            }
        }
        protected void GrdDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAsig = e.Row.FindControl("IbtAsig") as ImageButton;
                if (IbtAsig != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtAsig'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsig.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void IbnExcel_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                string query = "", VbNomArchivo = "";

                VbNomArchivo = "Incoming";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExpIncoming", Session["77IDM"].ToString().Trim());
                query = "EXEC SP_PANTALLA_Incoming 1,'','','','CurExpIncoming',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandTimeout = 90000000;
                        cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                int VbLog = VbNomArchivo.Length > 30 ? 30 : VbNomArchivo.Length;
                                ds.Tables[0].TableName = VbNomArchivo.Trim().Substring(0, VbLog);
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomArchivo));
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
                Page.Title = ViewState["PageTit"].ToString();
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Incoming", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        //****************************** Ventana de Asignacion  ******************************************
        protected void IbtCerrarCambioBod_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; }
        protected void BindDDdlDestino(string Tercero)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Asignacion 3,'','','WEB','{0}',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", Tercero.ToString().Trim(), Session["!dC!@"]);
            DdlBodDest.DataSource = Cnx.DSET(LtxtSql);
            DdlBodDest.DataMember = "Datos";
            DdlBodDest.DataTextField = "Bodega";
            DdlBodDest.DataValueField = "CodUbicaBodega";
            DdlBodDest.DataBind();
        }
        protected void BIndDFilaColumDest(string CodUbicaOrig, string Codtercer)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {

                string VbTxtSql = "EXEC SP_PANTALLA_Incoming 4,@Bd,@UbOrg,@Trc,'',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Bd", DdlBodDest.SelectedItem.Text.Trim());
                    SC.Parameters.AddWithValue("@UbOrg", CodUbicaOrig.Trim());
                    SC.Parameters.AddWithValue("@Trc", Codtercer.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);
                        if (DtB.Rows.Count > 0) { GrdUbicaDes.DataSource = DtB; GrdUbicaDes.DataBind(); }
                        else { GrdUbicaDes.DataSource = null; GrdUbicaDes.DataBind(); }
                    }
                }
            }
        }
        protected void DdlBodDest_TextChanged(object sender, EventArgs e)
        {
            int VbLenOrg = ViewState["CodBodegaOrig"].ToString().Trim().Length < 4 ? ViewState["CodBodegaOrig"].ToString().Trim().Length : 4;
            int VbLenDest = DdlBodDest.SelectedItem.Text.Trim().Length < 5 ? DdlBodDest.SelectedItem.Text.Trim().Length : 5;

            if (ViewState["CodBodegaOrig"].ToString().Trim().Substring(0, VbLenOrg) == "REPA" && DdlBodDest.SelectedItem.Text.Trim().Substring(0, VbLenDest) == "MANTO")
            { }
            else
            {
                BIndDFilaColumDest(ViewState["CodUbicaBodega"].ToString().Trim(), ViewState["CodTercero"].ToString().Trim());
            }
        }
        protected void GrdUbicaDes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            double VbCantAct = Convert.ToDouble(TxtCantAct.Text);
            TxtCantNew.Text = TxtCantNew.Text.Equals("") ? "0" : TxtCantNew.Text.Trim();
            double VbCantNew = Convert.ToDouble(TxtCantNew.Text);
            if (VbCantNew > VbCantAct)
            {
                Result = Idioma.Select("Objeto= 'Mens01Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//La cantidad a transferir supera la cantidad actual.
                return;
            }
            if (VbCantNew <= 0)
            {
                Result = Idioma.Select("Objeto= 'Mens02Icmg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un cantidad válida.
                return;
            }
            string VbAplicaFV = "N";
            if (TxtFechI.Visible == true)
            {
                VbAplicaFV = "S";
                Cnx.ValidarFechas(TxtFechI.Text.Trim(), "", 1);
                var Mensj = Cnx.GetMensj();
                if (!Mensj.ToString().Trim().Equals(""))
                {
                    Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    Page.Title = ViewState["PageTit"].ToString();
                    return;
                }
            }
            if (e.CommandName.Equals("Asignar"))
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                DateTime? VbFechaV;
                if (VbAplicaFV == "S") { VbFechaV = Convert.ToDateTime(TxtFechI.Text); }
                else { VbFechaV = null; }
                List<ClsTypAsignaciones> ObjAsignaciones = new List<ClsTypAsignaciones>();
                var TypAsignaciones = new ClsTypAsignaciones()
                {
                    CodIdUbicacion = Convert.ToInt32(ViewState["CodIdUbicacion"]),
                    CodUbicaBodegaOrg = ViewState["CodUbicaBodega"].ToString().Trim(),
                    CodUbicaBodegaDst = GrdUbicaDes.DataKeys[gvr.RowIndex].Values["CodUbicaBodega"].ToString(),
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    CodTipoElemento = ViewState["CodTipoElemento"].ToString().Trim(),
                    IdentificadorElem = ViewState["IdentificadorElem"].ToString().Trim(),
                    CodAlmacen = Convert.ToInt32(DdlAlmacen.Text),
                    CodBodegaOrg = TxtBodOrig.Text.Trim(),
                    CodBodegaDst = DdlBodDest.SelectedItem.Text.Trim(),
                    Cantidad = VbCantNew,
                    AplicaFV = VbAplicaFV,
                    FechaVence = VbFechaV,
                    Usu = Session["C77U"].ToString(),
                    SP = "N",
                    Accion = "INCOMING",
                };
                ObjAsignaciones.Add(TypAsignaciones);
                ClsTypAsignaciones Asignaciones = new ClsTypAsignaciones();
                Asignaciones.Alimentar(ObjAsignaciones);//
                string Mensj = Asignaciones.GetMensj();
                if (!Mensj.Equals(""))
                {
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                    return;
                }
                BIndDatos();
                MultVw.ActiveViewIndex = 0;
            }
            Page.Title = ViewState["PageTit"].ToString();
        }
        protected void GrdUbicaDes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAsig = e.Row.FindControl("IbtAsigD") as ImageButton;
                if (IbtAsig != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdTrasf'");
                    foreach (DataRow RowIdioma in Result)
                    {
                        IbtAsig.ToolTip = RowIdioma["Texto"].ToString().Trim();
                        Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                        foreach (DataRow row in Result)
                        { IbtAsig.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                    }
                }
            }
        }
    }
}