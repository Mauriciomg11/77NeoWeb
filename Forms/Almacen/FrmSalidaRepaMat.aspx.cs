using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
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
    public partial class FrmSalidaRepaMat : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSDetalle = new DataSet();
        DataSet DSAsignar = new DataSet();
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
                TitForm.Text = "Salida Reparación";
                Page.Title = TitForm.Text;
                ViewState["PageTit"] = TitForm.Text;
                ViewState["TipoRepa"] = "";
                ViewState["CodOrdenRepa"] = "";
                ViewState["Codigo"] = ""; // es es numero de la SO internacionales o NumRepa Nacionales
                ViewState["Posicion"] = ""; // Posicion de la SO o Repa nal
                ViewState["DT"] = ""; // Dia Tasa cotizac
                ViewState["MT"] = "";// Mes Tasa cotizac
                ViewState["AT"] = "";// Año Tasa cotizac
                ViewState["TRM"] = "";// valor tasa acordado cotiza
                ViewState["CodTipoElem"] = "";
                ViewState["CCosto"] = "";
                ViewState["AfectaInv"] = "";// Si afecta el campo inventario
                ViewState["PPT"] = "";// Si afecta el campo inventario
                ModSeguridad();
                TraerDatos("UPD");
                TipoRepa("N");
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
                    if (bO.Equals("Caption"))
                    { Page.Title = bT; ViewState["PageTit"] = bT; }
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblTitCondManiplc.Text = bO.Equals("LblCondAlma") ? bT : LblTitCondManiplc.Text;
                    BtnCloseMdl.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseMdl.Text;
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;

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
                    string VbTxtSql = "EXEC PNTLL_Reparacion 4,@U,'','','','','',0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";

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
                                DSTDdl.Tables[1].TableName = "RepaNal";
                                DSTDdl.Tables[2].TableName = "RepaInta";
                                /*DSTDdl.Tables[3].TableName = "EjecCodigo";
                                DSTDdl.Tables[4].TableName = "EjecCodComex";*/
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacen.DataSource = DSTDdl.Tables[0];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            }
        }
        protected void TipoRepa(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            ViewState["TipoRepa"] = Tipo;
            if (Tipo.Equals("N"))
            {
                if (DSTDdl.Tables["RepaNal"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaNal"];
                    DdlNumRepa.DataTextField = "CodReparacion";
                }
            }
            else
            {
                if (DSTDdl.Tables["RepaInta"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaInta"];
                    DdlNumRepa.DataTextField = "CodShippingOrder";
                }
            }
            DdlNumRepa.DataValueField = "Codigo";
            DdlNumRepa.DataBind();
            DdlNumRepa.Text = "";
            /*GrdDtlleRepa.DataSource = null;
            GrdDtlleRepa.DataBind();*/
        }
        protected void RdbNacional_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("N"); }
        protected void RdbInter_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("I"); }
        protected void BindDetRepa(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                string S_RepaNAL_INTA = "";
                if (ViewState["TipoRepa"].ToString().Equals("N")) { S_RepaNAL_INTA = "RepaNal"; }
                else { S_RepaNAL_INTA = "RepaInta"; }
                if (DSTDdl.Tables[S_RepaNAL_INTA].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[S_RepaNAL_INTA].Clone();
                    DataRow[] DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo='" + DdlNumRepa.Text.Trim() + "'");
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DT = DR.CopyToDataTable();
                        TxtMoneda.Text = DT.Rows[0]["CodMoneda"].ToString().Trim();
                        DataTable DTEC = new DataTable();
                        DataRow[] DREC; DataRow[] Result;
                        //string S_AplicaComex = "S";
                        /* if (ViewState["TipoRepa"].ToString().Equals("I")) //Si es internacional valida que este liquidada la orden de embarque
                          {
                              DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 5 AND EjecutarCodigo = 'S'"); //Si aplica validacion de la liquidacion
                              if (Cnx.ValidaDataRowVacio(DREC))
                              {
                                  DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("EstadoLiquidacion <> 1");//Esta liquidada
                                  if (Cnx.ValidaDataRowVacio(DR))
                                  {
                                      DT = DR.CopyToDataTable();
                                      Result = Idioma.Select("Objeto= 'Msj05EntC'"); //La orden de embarque no se encuentra liquidada.
                                      foreach (DataRow row in Result)
                                      { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DdlNumRepa.Text.Trim() + "');", true); }
                                      GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                                      return;
                                  }
                              }
                              DREC = DSTDdl.Tables["EjecCodComex"].Select("Caso = 5 AND EjecutarCodigo = 'N'"); //Aplica COMEX
                              if (Cnx.ValidaDataRowVacio(DREC)) { S_AplicaComex = "N"; }
                          } */
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Aprobado = 0");
                        if (Cnx.ValidaDataRowVacio(DR))// Si la Compra esta aprobada
                        {
                            DT = DR.CopyToDataTable();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('La reparación no se encuentra aprobada  | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true);

                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Asentado = 1");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta asentada*/
                        {
                            DT = DR.CopyToDataTable();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('La reparación se encuentra asentada. | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true);

                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        if (Accion.Equals("UPD"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                            {
                                string VbTxtSql = " EXEC PNTLL_Reparacion 5, @CodOC,'','','','',@TipoOC,0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";
                                sqlConB.Open();
                                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                                {
                                    SC.Parameters.AddWithValue("@CodOC", DdlNumRepa.Text.Trim());
                                    SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@TipoOC", ViewState["TipoRepa"]);
                                    // SC.Parameters.AddWithValue("@ApliComex", S_AplicaComex);
                                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                                    {
                                        using (DataSet DSDetalle = new DataSet())
                                        {
                                            SDA.SelectCommand = SC;
                                            SDA.Fill(DSDetalle);
                                            DSDetalle.Tables[0].TableName = "EstadoRepa";
                                            DSDetalle.Tables[1].TableName = "CondManip";
                                            DSDetalle.Tables[2].TableName = "CurTemporal";
                                            DSDetalle.Tables[3].TableName = "CurActualizar";/**/
                                            DSDetalle.Tables[4].TableName = "CurCCosto";/**/
                                            ViewState["DSDetalle"] = DSDetalle;
                                        }
                                    }
                                }
                            }
                        }
                        DSDetalle = (DataSet)ViewState["DSDetalle"];
                        if (DSDetalle.Tables["EstadoRepa"].Rows.Count > 0)
                        { GrdDtlleRepa.DataSource = DSDetalle.Tables["EstadoRepa"]; }
                        GrdDtlleRepa.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void DdlNumRepa_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            //if (!DdlNumRepa.SelectedItem.Value.Equals("")) { BindDetRepa("UPD"); }
            BindDetRepa("UPD");
        }
        protected void BindAsignar(string PN, string SN, string CodTipoElem, string CantRepa, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Reparacion 6, @Alm, @CodRepa, @PN,@SN,@CodTipoE,'', @CantRepa,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Alm", DdlAlmacen.Text.Trim());
                        SC.Parameters.AddWithValue("@CodRepa", ViewState["CodOrdenRepa"]);
                        SC.Parameters.AddWithValue("@PN", PN.Trim());
                        SC.Parameters.AddWithValue("@SN", SN.Trim());
                        SC.Parameters.AddWithValue("@CodTipoE", CodTipoElem.Trim());
                        SC.Parameters.AddWithValue("@CantRepa", CantRepa.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSAsignar = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSAsignar);

                                DSAsignar.Tables[0].TableName = "Asignar";
                                DSAsignar.Tables[1].TableName = "ValorizarInv";

                                ViewState["DSAsignar"] = DSAsignar;
                            }
                        }
                    }
                }
            }
            DSAsignar = (DataSet)ViewState["DSAsignar"];
            //Actualizar la cantidad a despachar en la vista de Detalle asignar para no despachar de la misma ubica si ya no tiene estok
            foreach (DataRow DRCur in DSDetalle.Tables["CurTemporal"].Rows)
            {
                foreach (DataRow DRAsig in DSAsignar.Tables["Asignar"].Rows)
                {
                    string CodUBCur = DRCur["CodUbicaBodega"].ToString().Trim();
                    string CodUBAsig = DRAsig["CodUbicaBodega"].ToString().Trim();                    
                    if (CodUBAsig.Equals(CodUBCur))
                    {
                        string CantCur = DRCur["CantIngr"].ToString().Trim();
                        string CantRepaA = DRAsig["Cantidad"].ToString().Trim();
                        DRAsig["Cantidad"] = Convert.ToInt32(CantRepaA) - Convert.ToInt32(CantCur);
                        DSAsignar.Tables["Asignar"].AcceptChanges();
                    }
                }
            }            
            if (DSAsignar.Tables["Asignar"].Rows.Count > 0)
            { GrdAsignar.DataSource = DSAsignar.Tables["Asignar"]; }
            GrdAsignar.DataBind();
        }
        protected void BindCondicManipulac(string CodRef)
        {
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            if (DSDetalle.Tables["CondManip"].Rows.Count > 0)
            {
                DataRow[] DR = DSDetalle.Tables["CondManip"].Select("CodReferencia='" + CodRef + "'");
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
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["PosicionAnt"] = ViewState["Posicion"];
            ViewState["CodRCant"] = ViewState["CodOrdenRepa"];
            MultVw.ActiveViewIndex = 0;
        }

        protected void GrdDtlleRepa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();

                if (DdlAlmacen.Text.Trim().Equals("0"))
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar el almacén.
                    return;
                }
                if (e.CommandName.Equals("Abrir"))
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    string VblCodRef = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                    string VblPn = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                    string VblSn = ((Label)row.FindControl("LblSn")).Text.ToString().Trim();
                    string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                    string VbCantRepa = ((Label)row.FindControl("LblCantRepa")).Text.ToString().Trim();
                    ViewState["Identf"] = ((Label)row.FindControl("LblIdentfc")).Text.ToString().Trim();
                    ViewState["CodTipoElem"] = ((Label)row.FindControl("LblTipo")).Text.ToString().Trim();
                    ViewState["CodOrdenRepa"] = ((Label)row.FindControl("LblNumRepa")).Text.ToString().Trim();
                    ViewState["CCosto"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["CCostos"].ToString();
                    ViewState["DT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["DiaTasa"].ToString();
                    ViewState["MT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["MesTasa"].ToString();
                    ViewState["AT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["AñoTasa"].ToString();
                    ViewState["TRM"] = ((Label)row.FindControl("Lbltrm")).Text.ToString().Trim();
                    ViewState["Codigo"] = ((Label)row.FindControl("LblNumDoc")).Text.ToString().Trim();
                    ViewState["Posicion"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                    ViewState["PPT"] = GrdDtlleRepa.DataKeys[gvr.RowIndex].Values["PPT"].ToString();
                    LblPNDescripcAsig.Text = ViewState["CodOrdenRepa"] + " | " + VblPn + " | " + VblDescPN + " | ";
                    LblAsigCantSolV.Text = VbCantRepa;
                    BindAsignar(VblPn, VblSn, ViewState["CodTipoElem"].ToString(), VbCantRepa, "UPD");
                    MultVw.ActiveViewIndex = 1;
                    BindCondicManipulac(VblCodRef);
                }
                Idioma = (DataTable)ViewState["TablaIdioma"];
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Seleccionar REpa", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdDtlleRepa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdRepaAbrirTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                DataRowView DRW = e.Row.DataItem as DataRowView;
                double VbDCanEnt = Convert.ToDouble(DRW["CantIngresar"].ToString().Trim());

                if (VbDCanEnt > 0)
                {
                    IbtAbrir.Visible = false; e.Row.BackColor = System.Drawing.Color.PaleGreen;
                }
            }
        }
        protected void GrdAsignar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                Page.Title = ViewState["PageTit"].ToString().Trim();

                if (e.CommandName.Equals("Asignar"))
                {
                    double VbDCantDesp = 0;
                    DataRow[] Result;
                    DSAsignar = (DataSet)ViewState["DSAsignar"];
                    DSDetalle = (DataSet)ViewState["DSDetalle"];

                    string AplicCC = DSDetalle.Tables["CurCCosto"].Rows[0][0].ToString().Trim();
                    if (AplicCC.Equals("S") && ViewState["CCosto"].Equals(""))
                    {
                        Result = Idioma.Select("Objeto= 'Mens01SalRepa'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La solicitud de pedido de la reparación debe tener asignado un centro de costo.
                        return;
                    }
                    // Almacena la vista para realizar el movimeinto de descargue

                    GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    GridViewRow Gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                    int VbDCantStock = Convert.ToInt32(((Label)Row.FindControl("LblStock")).Text.ToString().Trim().Equals("") ? "0" : ((Label)Row.FindControl("LblStock")).Text.ToString().Trim());
                    VbDCantDesp = Convert.ToInt32(((TextBox)Row.FindControl("TxtCantRepa")).Text.ToString().Trim().Equals("") ? "0" : ((TextBox)Row.FindControl("TxtCantRepa")).Text.ToString().Trim());
                    DateTime VbDFech;
                    if (VbDCantStock <= 0 && VbDCantStock< VbDCantDesp)
                    {
                        Result = Idioma.Select("Objeto= 'Mens02SalRepa'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// En esta ubicación ya fue asignada una reparación.
                        return;
                    }
                        string VbSRef = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodReferencia"].ToString();
                        string VbSPN = ((Label)Row.FindControl("LblPn")).Text.ToString().Trim();
                        string VbSSN = ((Label)Row.FindControl("LblSn")).Text.ToString().Trim();
                        if (GrdAsignar.DataKeys[Gvr.RowIndex].Values["FechaShelfLife"].ToString().Trim().Equals("")) { VbDFech = Convert.ToDateTime("01/01/1900"); }
                        else { VbDFech = Convert.ToDateTime(GrdAsignar.DataKeys[Gvr.RowIndex].Values["FechaShelfLife"].ToString().Trim()); }
                        int I_AfectInv = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodTercero"].ToString().Trim().Equals("") ? 1 : 0;
                        I_AfectInv = DSAsignar.Tables["ValorizarInv"].Rows[0][2].ToString().Trim().Equals("2") ? I_AfectInv : 0;
                        double D_VlrRepa = Convert.ToDouble(DSAsignar.Tables["ValorizarInv"].Rows[0][0].ToString().Trim());
                        double D_CostoComex = Convert.ToDouble(DSAsignar.Tables["ValorizarInv"].Rows[0][1].ToString().Trim());
                        string S_CodElem = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodElemento"].ToString();
                        string S_CodUM = ((Label)Row.FindControl("LblUndMed")).Text.ToString().Trim();
                        string S_CodUbica = GrdAsignar.DataKeys[Gvr.RowIndex].Values["CodUbicaBodega"].ToString();

                        DSDetalle.Tables["CurTemporal"].Rows.Add(ViewState["CodOrdenRepa"], VbSRef, VbSPN, VbSSN, VbDCantDesp, VbDFech, ViewState["CodTipoElem"],
                            ViewState["Identf"], ViewState["DT"], ViewState["MT"], ViewState["AT"], ViewState["TRM"], I_AfectInv, D_VlrRepa, D_CostoComex,
                            S_CodElem, "Cod Prove", S_CodUM, S_CodUbica, ViewState["Codigo"], ViewState["Posicion"], ViewState["CCosto"],
                            Convert.ToInt32(ViewState["PPT"]));
                        DSDetalle.Tables["CurTemporal"].AcceptChanges();
                    
                    //Actualizar la cantidad a despachar en la vista de Detalle Reserva          
                    foreach (DataRow row in DSDetalle.Tables["EstadoRepa"].Rows)
                    {
                        if (row["CodReparacion"].ToString().Equals(ViewState["CodOrdenRepa"]))
                        {
                            row["CantIngresar"] = VbDCantDesp;
                        }
                    }
                    DSDetalle.Tables["EstadoRepa"].AcceptChanges();
                    BindDetRepa("");
                    MultVw.ActiveViewIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Asignar OT en WS", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAsignar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAsignr = e.Row.FindControl("IbtAsignr") as ImageButton;
                if (IbtAsignr != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdRepaAsigTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAsignr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            return;
            try
            {
                List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
                foreach (DataRow Row in DSDetalle.Tables["CurTemporal"].Rows)
                {
                    string VbSCodTerc = Row["CodProv"].ToString().Trim();
                    var TypDetalle = new CsInsertElementoAlmacen()
                    {
                        IdIE = Convert.ToInt32(0),
                        CodElemento = Row["CodElem"].ToString().Trim(),
                        CodReferencia = Row["Referencia"].ToString().Trim(),
                        PN = Row["PN"].ToString(),
                        SN = Row["Identif"].ToString().Equals("SN") ? Row["SnLote"].ToString() : "",
                        Lote = Row["Identif"].ToString().Equals("LOTE") ? Row["SnLote"].ToString() : "",
                        CodTipoElem = Row["CodTipoElem"].ToString(),
                        Identificador = Row["Identif"].ToString().Trim(),
                        Descripcion = "",
                        Cantidad = Convert.ToDouble(Row["CantIngr"].ToString().Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(Row["ValorRepa"].ToString()),
                        CodUndMed = Row["CodUM"].ToString(),
                        IdAlmacen = Convert.ToInt32(DdlAlmacen.Text.Trim()),
                        CodBodega = Row["CodUbicaBodega"].ToString().Trim(),
                        CodShippingOrder = Row["CodSO"].ToString().Trim(),
                        Posicion = Row["Pos"].ToString().Trim(),
                        CodAeronave = 0,
                        Matricula = "",
                        CCosto = ViewState["CCosto"].ToString().Trim(),
                        AfectaInventario = Convert.ToInt32(ViewState["AfectaInv"]),
                        CostoImportacion = Convert.ToDouble(Row["CostoComex"].ToString()),
                        CodTercero = VbSCodTerc.Trim(),
                        Consignacion = Convert.ToInt32(0),
                        CodIdUbicacion = Convert.ToInt32(Row["CodIdUbicacion"].ToString().Trim()),
                        FechaVence = Convert.ToDateTime(Row["FechaShelfLife"].ToString().Trim()),
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = "",
                        Proceso = "0102",
                        IdDetPropHk = 0,
                        IdPPt = Convert.ToInt32(Row["PPT"].ToString()),
                        Accion = "SALIDA",
                    };
                    ObjDetalle.Add(TypDetalle);
                }
                CsInsertElementoAlmacen ClaseIEA = new CsInsertElementoAlmacen();
                ClaseIEA.FormOrigen(ViewState["PFileName"].ToString());
                ClaseIEA.Alimentar(ObjDetalle);

                string Mensj = ClaseIEA.GetMensj();
                if (!Mensj.Equals(""))
                {
                    string VblPn = ClaseIEA.GetPn().Trim().Equals("") ? "" : "  [P/N: " + ClaseIEA.GetPn().Trim() + "]  ";
                    string VblSn = ClaseIEA.GetSn().Trim().Equals("") ? "" : " [S/N: " + ClaseIEA.GetSn().Trim() + "] ";
                    string VbLote = ClaseIEA.GetLote().Trim().Equals("") ? "" : " [LT/N: " + ClaseIEA.GetLote().Trim() + "]";
                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VblPn + VblSn + "');", true);
                    return;
                }
                TxtObserv.Text = "";



                TraerDatos("UPD");
                //BindRva();
                //BindDetRsva("UPD");
                MultVw.ActiveViewIndex = 0;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Generar Salida Consumo", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
    }
}