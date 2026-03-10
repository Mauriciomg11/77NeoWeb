using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmSalidaBajaMat : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
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
                ViewState["PageTit"] = "";
                ModSeguridad();
                TraerDatos("UPD");
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
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMovimientoActivo.aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
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
                    if (bO.Equals("Caption")) { Page.Title = bT; ViewState["PageTit"] = bT; TitForm.Text = bT; }
                    TitForm.Text = bO.Equals("Titulo") ? bT : TitForm.Text;
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblAlmacen.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacen.Text;                  
                    LblTitCondManiplc.Text = bO.Equals("LblCondAlmaMstr") ? bT : LblTitCondManiplc.Text;
                    BtnCloseMdl.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseMdl.Text;
                    // *********************************************** Detalle Repa ***********************************************
                   /* GrdDtlleRecup.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleRecup.EmptyDataText;
                    GrdDtlleRecup.Columns[1].HeaderText = bO.Equals("GrdUbTec") ? bT : GrdDtlleRecup.Columns[1].HeaderText;
                    GrdDtlleRecup.Columns[2].HeaderText = bO.Equals("PosMstr") ? bT : GrdDtlleRecup.Columns[2].HeaderText;
                    GrdDtlleRecup.Columns[5].HeaderText = bO.Equals("Descripcion") ? bT : GrdDtlleRecup.Columns[5].HeaderText;
                    GrdDtlleRecup.Columns[6].HeaderText = bO.Equals("GrdFecRemc") ? bT : GrdDtlleRecup.Columns[6].HeaderText;
                    GrdDtlleRecup.Columns[7].HeaderText = bO.Equals("GrdRzonREm") ? bT : GrdDtlleRecup.Columns[7].HeaderText;
                    // *********************************************** Asignar ***********************************************
                    LblTitAsigFis.Text = bO.Equals("LblTitAsigFis") ? bT : LblTitAsigFis.Text;
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;
                    GrdAsignar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdAsignar.EmptyDataText;
                    GrdAsignar.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdAsignar.Columns[3].HeaderText;
                    GrdAsignar.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdAsignar.Columns[4].HeaderText;
                    GrdAsignar.Columns[5].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdAsignar.Columns[5].HeaderText;
                    GrdAsignar.Columns[6].HeaderText = bO.Equals("GrdBodDest") ? bT : GrdAsignar.Columns[6].HeaderText;*/
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void TraerDatos(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_SalBaja 1,@U,'','','','','','','','',0,0,0,0,0,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@U", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Almac";
                                DSTDdl.Tables[1].TableName = "ElemDisp";
                                DSTDdl.Tables[2].TableName = "CurTemporal";
                                DSTDdl.Tables[3].TableName = "CondManip";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];           
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacen.DataSource = DSTDdl.Tables["Almac"];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            }
        }
        protected void DdlAlmacen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataTable DT = new DataTable();
                IEnumerable<DataRow> VbQry = from A in DSTDdl.Tables["ElemDisp"].AsEnumerable() where A.Field<int>("CodAlmacen") == Convert.ToInt32(DdlAlmacen.Text.Trim()) || A.Field<int>("CodAlmacen") == 0 select A;
                if (Cnx.ValidaDataRowVacio(VbQry))
                {
                    DT = VbQry.CopyToDataTable();
                   
                    IEnumerable<DataRow> QD = from A in DT.AsEnumerable() where A.Field<int>("CodIdUbicacion") != 0 select A;
                    if (Cnx.ValidaDataRowVacio(QD))
                    {
                        DataTable DTDet = QD.CopyToDataTable();
                        GrdDtlleBaja.DataSource = DTDet;
                    }
                    else { GrdDtlleBaja.DataSource = null; }
                    GrdDtlleBaja.DataBind();
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Almacen S. BAJA", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdDtlleBaja_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                DataRow[] Result;
                if (DdlAlmacen.Text.Trim().Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el almacén.
                    return;
                }
                if (TxtObserv.Text.Trim().Equals(""))
                {
                   Result = Idioma.Select("Objeto= 'MstrMens22'");
                    foreach (DataRow DRB in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DRB["Texto"].ToString() + "');", true); }//debe ingresar la observacion
                    TxtObserv.Focus();
                    return;
                }
                if (e.CommandName.Equals("Abrir"))
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    string S_Ref = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodReferencia"].ToString().Trim();
                    string S_Pn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                    string S_Sn = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                    string S_Lot = ((Label)row.FindControl("LblLote")).Text.ToString().Trim();
                    string S_Desc = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                    DateTime VbDFech =  Convert.ToDateTime(GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["FechV"].ToString().Trim());
                    string S_TipElem = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodTipoElemento"].ToString().Trim();
                    string S_Ident = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["IdentificadorElemR"].ToString().Trim();
                    string S_CodTerc = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodTercero"].ToString().Trim();
                    int I_AfectInv = S_CodTerc.Trim().Equals("") ? 1 : 0;
                    string S_CodElem = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodElemento"].ToString().Trim();
                    string S_CodUM = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodUndMedR"].ToString().Trim();
                    int I_Pos = 0;//((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                    string S_CC = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CCosto"].ToString().Trim();
                    string S_BodOrig = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodUbicaBodega"].ToString().Trim();
                    string S_CodMond = GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodMoneda"].ToString().Trim();
                    int I_IdUbic = Convert.ToInt32(GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["CodIdUbicacion"].ToString());                   
                    int I_Bloquear = Convert.ToInt32(GrdDtlleBaja.DataKeys[gvr.RowIndex].Values["Bloquear"].ToString());
                    if (I_Bloquear == 1)// Si la Compra esta aprobada
                    {
                        Result = Idioma.Select("Objeto= 'MstrMens15'"); //El P/N se encuentra bloqueado.
                        foreach (DataRow DRM in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + DRM["Texto"].ToString() + " | " + ViewState["CodOrdenRepa"].ToString().Trim() + "');", true); }
                        return;
                    }                    
                    DSTDdl.Tables["CurTemporal"].Rows.Add(0, "", S_Ref, S_Pn, S_Sn, S_Lot, S_Desc, 1, VbDFech, S_TipElem, S_Ident, "", "", "", 0, I_AfectInv, 0, 0, S_CodElem, "",
                                                        S_CodUM, "CodBodDest", "CodStado", "", I_Pos, S_CC, 0, S_BodOrig, I_IdUbic, 0, "", 0, S_CodTerc, S_CodMond);
                    DSTDdl.Tables["CurTemporal"].AcceptChanges();
                    if (DSTDdl.Tables["CurTemporal"].Rows.Count > 0)
                    { GrdAsignar.DataSource = DSTDdl.Tables["CurTemporal"]; }
                    GrdAsignar.DataBind();
                    MultVw.ActiveViewIndex = 1;
                    BindCondicManipulac(S_Ref);
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Seleccionar Ubicacion Ent Repa", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }

        protected void GrdDtlleBaja_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BindCondicManipulac(string CodRef)
        {
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["CondManip"].Rows.Count > 0)
            {
                DataRow[] DR = DSTDdl.Tables["CondManip"].Select("CodReferencia='" + CodRef + "'");
                if (Cnx.ValidaDataRowVacio(DR))
                {
                    DataTable DT = DR.CopyToDataTable();
                    GrdMdlCondManplc.DataSource = DT; GrdMdlCondManplc.DataBind();
                    if (DT.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPopup();", true);
                    }
                }
            }
        }

        protected void GrdAsignar_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdAsignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}