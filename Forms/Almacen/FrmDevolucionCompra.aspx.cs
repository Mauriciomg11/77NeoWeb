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
    public partial class FrmDevolucionCompra : System.Web.UI.Page
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
                TitForm.Text = "devolucion compra";
                Page.Title = TitForm.Text;
                ViewState["PageTit"] = TitForm.Text;
                ViewState["CodOrdenCompra"] = "";
                ViewState["Codigo"] = ""; // es es numero de la SO internacionales o NumRepa Nacionales
                ViewState["Posicion"] = ""; // Posicion de la SO o Repa nal
                ViewState["DT"] = ""; // Dia Tasa cotizac
                ViewState["MT"] = "";// Mes Tasa cotizac
                ViewState["AT"] = "";// Año Tasa cotizac
                ViewState["TRM"] = "";// valor tasa acordado cotiza
                ViewState["CodTipoElem"] = "";
                ViewState["CodProv"] = "";// CodProveedor de la Repa
                ViewState["PPT"] = "";// Si afecta el campo inventario
                ViewState["CodMoneda"] = "";// Moneda config a la CIA
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
                    LblAlmacen.Text = bO.Equals("AlmacenMstr") ? bT : LblAlmacen.Text;
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    LblNumCompra.Text = bO.Equals("DocMstr") ? bT : LblNumCompra.Text;
                    Lbltipo.Text = bO.Equals("TipoMstr") ? bT : Lbltipo.Text;
                    LblTitCondManiplc.Text = bO.Equals("LblCondAlmaMstr") ? bT : LblTitCondManiplc.Text;
                    BtnCloseMdl.Text = bO.Equals("BtnCerrarMst") ? bT : BtnCloseMdl.Text;
                    BtnVisualizar.Text = bO.Equals("BtnVisualizar") ? bT : BtnVisualizar.Text;
                    BtnGuardar.Text = bO.Equals("BotonIngOk") ? bT : BtnGuardar.Text;
                    // *********************************************** Detalle Repa ***********************************************
                    GrdDtlleComp.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleComp.EmptyDataText;
                    GrdDtlleComp.Columns[1].HeaderText = bO.Equals("DocMstr") ? bT : GrdDtlleComp.Columns[1].HeaderText;
                    GrdDtlleComp.Columns[2].HeaderText = bO.Equals("PosMstr") ? bT : GrdDtlleComp.Columns[2].HeaderText;
                    GrdDtlleComp.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDtlleComp.Columns[3].HeaderText;
                    GrdDtlleComp.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdDtlleComp.Columns[4].HeaderText;
                    GrdDtlleComp.Columns[5].HeaderText = bO.Equals("TipoMstr") ? bT : GrdDtlleComp.Columns[5].HeaderText;
                    GrdDtlleComp.Columns[6].HeaderText = bO.Equals("LblIdentifMstr") ? bT : GrdDtlleComp.Columns[6].HeaderText;
                    GrdDtlleComp.Columns[8].HeaderText = bO.Equals("GrdCantComp") ? bT : GrdDtlleComp.Columns[8].HeaderText;
                    GrdDtlleComp.Columns[9].HeaderText = bO.Equals("GrdUndComp") ? bT : GrdDtlleComp.Columns[9].HeaderText;
                    GrdDtlleComp.Columns[10].HeaderText = bO.Equals("GrdCantRec") ? bT : GrdDtlleComp.Columns[10].HeaderText;
                    GrdDtlleComp.Columns[11].HeaderText = bO.Equals("GrdCantDev") ? bT : GrdDtlleComp.Columns[11].HeaderText;
                    GrdDtlleComp.Columns[12].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdDtlleComp.Columns[12].HeaderText;
                    GrdDtlleComp.Columns[13].HeaderText = bO.Equals("LblFactMstr") ? bT : GrdDtlleComp.Columns[13].HeaderText;
                    // *********************************************** Asignar Elemento ***********************************************
                    LblTitAsigFis.Text = bO.Equals("LblTitAsigFis") ? bT : LblTitAsigFis.Text;
                    BtnAsignr.Text = bO.Equals("LblAsigMstr") ? bT : BtnAsignr.Text;
                    LblAsigCantSol.Text = bO.Equals("GrdCantComp") ? bT+":" : LblAsigCantSol.Text;
                    LblAsigCantEntrg.Text = bO.Equals("GrdCantRec") ? bT + ":" : LblAsigCantEntrg.Text;
                    IbtCerrarAsing.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarAsing.ToolTip;
                    GrdAsignar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDtlleComp.EmptyDataText;
                    GrdAsignar.Columns[0].HeaderText = bO.Equals("EstdPNMst") ? bT : GrdAsignar.Columns[0].HeaderText;
                    GrdAsignar.Columns[3].HeaderText = bO.Equals("LoteMst") ? bT : GrdAsignar.Columns[3].HeaderText;
                    GrdAsignar.Columns[4].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdAsignar.Columns[4].HeaderText;
                    GrdAsignar.Columns[5].HeaderText = bO.Equals("FilaMstr") ? bT : GrdAsignar.Columns[5].HeaderText;
                    GrdAsignar.Columns[6].HeaderText = bO.Equals("ColumMstr") ? bT : GrdAsignar.Columns[6].HeaderText;
                    GrdAsignar.Columns[7].HeaderText = bO.Equals("GrdCantStockMstr") ? bT : GrdAsignar.Columns[7].HeaderText;
                    GrdAsignar.Columns[8].HeaderText = bO.Equals("GrdCantDespc") ? bT : GrdAsignar.Columns[8].HeaderText;
                    GrdAsignar.Columns[9].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdAsignar.Columns[9].HeaderText;
                    // *********************************************** Visualizar y entrega ***********************************************
                    LblTitVisualizaGuarda.Text = bO.Equals("LblTitVisualizaGuarda") ? bT : LblTitVisualizaGuarda.Text;                   
                    IbtCloseGuardar.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCloseGuardar.ToolTip;
                    LblNumDocGuardar.Text = bO.Equals("DocMstr") ? bT + ":" : LblNumDocGuardar.Text;
                    GrdVisualizar.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdVisualizar.EmptyDataText;
                    GrdVisualizar.Columns[0].HeaderText = bO.Equals("PosMstr") ? bT : GrdVisualizar.Columns[0].HeaderText;
                    GrdVisualizar.Columns[1].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdVisualizar.Columns[1].HeaderText;
                    GrdVisualizar.Columns[4].HeaderText = bO.Equals("LoteMst") ? bT : GrdVisualizar.Columns[4].HeaderText;
                    GrdVisualizar.Columns[5].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdVisualizar.Columns[5].HeaderText;
                    GrdVisualizar.Columns[6].HeaderText = bO.Equals("FilaMstr") ? bT : GrdVisualizar.Columns[6].HeaderText;
                    GrdVisualizar.Columns[7].HeaderText = bO.Equals("ColumMstr") ? bT : GrdVisualizar.Columns[7].HeaderText;
                    GrdVisualizar.Columns[8].HeaderText = bO.Equals("GrdCantStockMstr") ? bT : GrdVisualizar.Columns[8].HeaderText;
                    GrdVisualizar.Columns[9].HeaderText = bO.Equals("CantMst") ? bT : GrdVisualizar.Columns[9].HeaderText;
                    GrdVisualizar.Columns[10].HeaderText = bO.Equals("GrdUndMstr") ? bT : GrdVisualizar.Columns[10].HeaderText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnClMstr'");//¿Desea realizar el movimiento?
               foreach (DataRow row in Result)
              { BtnGuardar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
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
                    string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 3, @U,'','','','',0,0, @Idm, @ICC,'01-01-1','02-01-1','03-01-1'";

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
                                DSTDdl.Tables[1].TableName = "DocCompra";
                                /*DSTDdl.Tables[2].TableName = "RepaInta";*/
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
            if (DSTDdl.Tables["DocCompra"].Rows.Count > 0)
            {
                DdlNumCompra.DataSource = DSTDdl.Tables["DocCompra"];
                DdlNumCompra.DataTextField = "CodOrdenCompra";
                DdlNumCompra.DataValueField = "Codigo";
                DdlNumCompra.DataBind();
                DdlNumCompra.Text = "";
            }
        }
        protected void BindDetCompra(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            try
            {
                if (DSTDdl.Tables["DocCompra"].Rows.Count > 0)
                {
                    if (Accion.Equals("UPD"))
                    {
                        Cnx.SelecBD();
                        using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                        {
                            string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 4, @CodOC,'','','','',0,0,0, @ICC,'01-01-1','02-01-1','03-01-1'";
                            sqlConB.Open();
                            using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                            {
                                SC.Parameters.AddWithValue("@CodOC", DdlNumCompra.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                using (SqlDataAdapter SDA = new SqlDataAdapter())
                                {
                                    using (DataSet DSDetalle = new DataSet())
                                    {
                                        SDA.SelectCommand = SC;
                                        SDA.Fill(DSDetalle);
                                        DSDetalle.Tables[0].TableName = "DetCompra";
                                        DSDetalle.Tables[1].TableName = "CondManip";
                                        DSDetalle.Tables[2].TableName = "CurTemporal";
                                        //DSDetalle.Tables[3].TableName = "CurActualizar";
                                        ViewState["DSDetalle"] = DSDetalle;
                                    }
                                }
                            }
                        }
                    }
                    DSDetalle = (DataSet)ViewState["DSDetalle"];
                    if (DSDetalle.Tables["DetCompra"].Rows.Count > 0)
                    { GrdDtlleComp.DataSource = DSDetalle.Tables["DetCompra"]; }
                    GrdDtlleComp.DataBind();
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void DdlNumCompra_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            TxtTipo.Text = DSTDdl.Tables["DocCompra"].AsEnumerable().Where(x => x.Field<string>("Codigo") == DdlNumCompra.Text.Trim()).Select(x => x.Field<string>("TipoOrdenCompra")).FirstOrDefault();
            // 
            ViewState["CodMoneda"] = DSTDdl.Tables["DocCompra"].AsEnumerable().Where(x => x.Field<string>("Codigo") == DdlNumCompra.Text.Trim()).Select(x => x.Field<string>("CodMoneda")).FirstOrDefault();
            if (!DdlNumCompra.SelectedItem.Value.Equals("")) { BindDetCompra("UPD"); }
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
        protected void BindAsignar(string PN, string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Entrada_Compra 5, @PN, @CodOC,'','','', @Alm,0,@Idm, @ICC,'01-01-1','02-01-1','03-01-1'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@PN", PN.Trim());
                        SC.Parameters.AddWithValue("@CodOC", ViewState["CodOrdenCompra"]);
                        SC.Parameters.AddWithValue("@Alm", DdlAlmacen.Text.Trim());
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSAsignar = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSAsignar);

                                DSAsignar.Tables[0].TableName = "Asignar";
                                ViewState["DSAsignar"] = DSAsignar;
                            }
                        }
                    }
                }
            }
            DSAsignar = (DataSet)ViewState["DSAsignar"];
            //Actualizar la cantidad a despachar en la vista de Detalle asignar para no despachar de la misma ubica si ya no tiene estok
            /* foreach (DataRow DRCur in DSDetalle.Tables["CurTemporal"].Rows)
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
             }*/
            if (DSAsignar.Tables["Asignar"].Rows.Count > 0)
            { GrdAsignar.DataSource = DSAsignar.Tables["Asignar"]; }
            GrdAsignar.DataBind();
        }
        protected void GrdDtlleComp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            DSTDdl = (DataSet)ViewState["DSTDdl"];
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
                ViewState["CodRef"] = ((Label)row.FindControl("LblRef")).Text.ToString().Trim();
                ViewState["PN"] = ((Label)row.FindControl("LblPn")).Text.ToString().Trim();
                ViewState["Identif"] = ((Label)row.FindControl("LblIdentfc")).Text.ToString().Trim();
                ViewState["CodOrdenCompra"] = ((Label)row.FindControl("LblNumOC")).Text.ToString().Trim();
                ViewState["IdDetOrdenCompra"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["IdDetOrdenCompra"].ToString();
                ViewState["Posicion"] = ((Label)row.FindControl("LblPos")).Text.ToString().Trim();
                ViewState["CodProv"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CodProveedor"].ToString();
                ViewState["VlrUnd"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["ValorUnidad"].ToString();
                ViewState["Vr_Compra"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["Valor_Compra"].ToString();
                ViewState["CodUM"] = ((Label)row.FindControl("LblUndMedDev")).Text.ToString().Trim();
                ViewState["CodTipoElem"] = ((Label)row.FindControl("LblTipo")).Text.ToString().Trim();
                ViewState["UndCompra"] = ((Label)row.FindControl("LblUndMedCompra")).Text.ToString().Trim();
                ViewState["PNBloq"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["Bloquear"].ToString();
                ViewState["DT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["DiaTasa"].ToString().Trim();
                ViewState["MT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["MesTasa"].ToString().Trim();
                ViewState["AT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["AñoTasa"].ToString().Trim();
                ViewState["CCto"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CCostos"].ToString().Trim();
                ViewState["PPT"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["PPT"].ToString().Trim();
                ViewState["CodPedido"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["CodPedido"].ToString().Trim();
                ViewState["Equiv"] = GrdDtlleComp.DataKeys[gvr.RowIndex].Values["VlorEquivalencia"].ToString().Trim();
                string VblDescPN = ((Label)row.FindControl("LblDesc")).Text.ToString().Trim();
                ViewState["CantComp"] = ((Label)row.FindControl("LblCantCompra")).Text.ToString().Trim();
                string VbCantRec = ((Label)row.FindControl("LblCantRecb")).Text.ToString().Trim();
                LblPoscn.Text = ViewState["Posicion"].ToString() + " | " + ViewState["PN"].ToString().Trim() + " | " + VblDescPN.Trim() + " | ";
                LblAsigCantCompV.Text = ViewState["CantComp"].ToString(); LblAsigCantRecV.Text = VbCantRec;
                DataRow[] Result;
                if (ViewState["PNBloq"].Equals("1"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens15'");
                    foreach (DataRow Row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }//El P/N se encuentra bloqueado.
                    return;
                }
                BindAsignar(ViewState["PN"].ToString(), "UPD");
                MultVw.ActiveViewIndex = 1;
                BindCondicManipulac(ViewState["CodRef"].ToString());
            }
        }
        protected void GrdDtlleComp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IbtAbrir = e.Row.FindControl("IbtAbrir") as ImageButton;
                if (IbtAbrir != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='GrdAsigTT'");
                    foreach (DataRow RowIdioma in Result)
                    { IbtAbrir.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
                DataRowView DRW = e.Row.DataItem as DataRowView;
                double D_CantDev = Convert.ToDouble(DRW["CantDevolucion"].ToString().Trim());
                if (D_CantDev > 0)
                {
                    IbtAbrir.Visible = false; e.Row.BackColor = System.Drawing.Color.GreenYellow;
                }
            }
        }
        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            // ViewState["PosicionAnt"] = ViewState["Posicion"];
            //ViewState["CodOCAnt"] = ViewState["CodOrdenCompra"];
            MultVw.ActiveViewIndex = 0;
        }
        protected void BtnAsignr_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSDetalle = (DataSet)ViewState["DSDetalle"];
                Page.Title = ViewState["PageTit"].ToString().Trim();
                DataRow[] Result;
                double VbDAsignadas = 0;
                string S_UndComp = "";
                string S_CodUM = "";
                //Valida que las cantidades 
                foreach (GridViewRow GrdRow in GrdAsignar.Rows)
                {
                    TextBox TxtCantDespa = (GrdRow.FindControl("TxtCantDespa") as TextBox);
                    double VbDCantDesp = Convert.ToDouble(TxtCantDespa.Text.Trim().Equals("") ? "0" : TxtCantDespa.Text.Trim());
                     S_UndComp = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["UndMed_Compra"].ToString().Trim();
                     S_CodUM = (GrdRow.FindControl("LblUndMed") as Label).Text.Trim();
                    if (VbDCantDesp > 0)
                    {
                        double VbDCantStokc = Convert.ToDouble((GrdRow.FindControl("LblStock") as Label).Text.Trim());
                        if (VbDCantDesp > VbDCantStokc)
                        {
                            Result = Idioma.Select("Objeto= 'MstrMens30'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La cantidad no debe superar a la que se encuentra en stock.
                            TxtCantDespa.Focus();
                            return;
                        }
                    }
                    VbDAsignadas += VbDCantDesp;
                }
                // Valida que las cantidades a despachar no supere las pendientes por entregar
                double VbDPendiente = Convert.ToDouble(LblAsigCantRecV.Text.Trim());
                if (!S_UndComp.Equals(S_CodUM))
                {
                    VbDPendiente = VbDPendiente * Convert.ToInt32(ViewState["Equiv"]);                  
                }
                if (VbDAsignadas > VbDPendiente)
                {
                    Result = Idioma.Select("Objeto= 'Mens01DevCom'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La cantidad a devolver supera la cantidad recibida.

                    return;
                }
                // Almacena la vista para realizar el movimeinto de descargue

                foreach (GridViewRow GrdRow in GrdAsignar.Rows)
                {
                    TextBox TxtCantDespa = (GrdRow.FindControl("TxtCantDespa") as TextBox);
                    double VbDCantDesp = Convert.ToDouble(TxtCantDespa.Text.Trim().Equals("") ? "0" : TxtCantDespa.Text.Trim());
                    if (VbDCantDesp > 0)
                    {
                         S_UndComp = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["UndMed_Compra"].ToString().Trim();
                         S_CodUM = (GrdRow.FindControl("LblUndMed") as Label).Text.Trim();                      
                        double D_CantCom = VbDCantDesp;
                        //double D_CantCom = Convert.ToDouble(GrdAsignar.DataKeys[GrdRow.RowIndex].Values["Cant_Compra"].ToString().Trim());
                        if (!S_UndComp.Equals(S_CodUM))
                        {
                            D_CantCom = VbDCantDesp / Convert.ToInt32(ViewState["Equiv"]);
                            if ((VbDCantDesp / Convert.ToInt32(ViewState["Equiv"])) % 1 != 0)
                            {
                                Result = Idioma.Select("Objeto= 'Mens02DevCom'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// La cantidad a devolver debe ser el equivalente de la unidad de medida de compra.}
                                return;
                            }
                        }
                        string VbSRef = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["CodReferencia"].ToString();
                        string VbSPN = (GrdRow.FindControl("LblPn") as Label).Text.Trim();
                        string VbSSN = (GrdRow.FindControl("LblSn") as Label).Text.Trim();
                        string S_Lote = (GrdRow.FindControl("LblLot") as Label).Text.Trim();
                        int I_AfectInv = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["CodTercero"].ToString().Trim().Equals("") ? 1 : 0;
                        string S_CodElem = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["CodElemento"].ToString();
                        string S_CodUbica = GrdAsignar.DataKeys[GrdRow.RowIndex].Values["CodUbicaBodega"].ToString();
                        int I_IdUbic = Convert.ToInt32(GrdAsignar.DataKeys[GrdRow.RowIndex].Values["CodIdUbicacion"].ToString());
                        double D_VlrCom = Convert.ToDouble(ViewState["Vr_Compra"]);
                        string S_NomBog = (GrdRow.FindControl("LblBodg") as Label).Text.Trim();
                        string S_F = (GrdRow.FindControl("LblFila") as Label).Text.Trim();
                        string S_C = (GrdRow.FindControl("LblColumn") as Label).Text.Trim();
                        double D_Stck = Convert.ToDouble((GrdRow.FindControl("LblStock") as Label).Text.Trim());
                        DSDetalle.Tables["CurTemporal"].Rows.Add(ViewState["CodOrdenCompra"], VbSRef, VbSPN, VbSSN, S_Lote, VbDCantDesp, ViewState["CodTipoElem"], ViewState["Identif"],
                            ViewState["DT"], ViewState["MT"], ViewState["AT"], I_AfectInv, 0, S_CodElem, ViewState["CodProv"], S_CodUM, S_CodUbica, ViewState["Posicion"],
                            ViewState["CCto"], Convert.ToInt32(ViewState["PPT"]), I_IdUbic, D_VlrCom, D_CantCom, S_UndComp, S_NomBog, S_F, S_C, D_Stck);
                        DSDetalle.Tables["CurTemporal"].AcceptChanges();
                    }
                    //Actualizar la cantidad a despachar en la vista de Detalle Reserva   
                    foreach (DataRow row in DSDetalle.Tables["DetCompra"].Rows)
                    {
                        if (row["Posicion"].ToString().Equals(ViewState["Posicion"]))
                        {
                            row["CantDevolucion"] = VbDAsignadas;
                        }
                    }
                    DSDetalle.Tables["DetCompra"].AcceptChanges();
                    BindDetCompra("");
                    MultVw.ActiveViewIndex = 0;
                    DdlAlmacen.Enabled = false;
                    DdlNumCompra.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            DataRow[] Result;
            try
            {
                if (DSDetalle != null)
                {
                    if (DSDetalle.Tables["CurTemporal"].Rows.Count > 0)
                    {
                        if (TxtObserv.Text.Trim().Equals(""))
                        {
                            Result = Idioma.Select("Objeto= 'MstrMens22'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//debe ingresar la observacion
                            return;
                        }
                        DataTable DT = DSDetalle.Tables["CurTemporal"];

                        DataView DV = DT.DefaultView;
                        DV.Sort = "CodDoc ASC, Pos ASC";
                        DT = DV.ToTable();
                        GrdVisualizar.DataSource = DT; GrdVisualizar.DataBind();
                        LblDocGuardar.Text = DdlNumCompra.Text.Trim();
                        MultVw.ActiveViewIndex = 2;
                    }
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void IbtCloseGuardar_Click(object sender, ImageClickEventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); MultVw.ActiveViewIndex = 0; }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSDetalle = (DataSet)ViewState["DSDetalle"];
            if (DSDetalle.Tables["CurTemporal"].Rows.Count > 0)
            {
                try
                {
                    List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
                    foreach (DataRow Row in DSDetalle.Tables["CurTemporal"].Rows)
                    {

                        var TypDetalle = new CsInsertElementoAlmacen()
                        {
                            IdIE = Convert.ToInt32(0),
                            CodElemento = Row["CodElemento"].ToString().Trim(),
                            CodReferencia = Row["CodReferencia"].ToString().Trim(),
                            PN = Row["PN"].ToString(),
                            SN = Row["SN"].ToString(),
                            Lote = Row["NumLote"].ToString(),
                            CodTipoElem = Row["CodTipoElem"].ToString(),
                            Identificador = Row["IdentificadorElem"].ToString().Trim(),
                            Descripcion = "",
                            Cantidad = Convert.ToDouble(Row["CantIngr"].ToString().Trim()),
                            CantidadAnt = Convert.ToDouble(0),
                            Valor = Convert.ToDouble(Row["VlrUnd"].ToString()),
                            CodUndMed = Row["CodUM"].ToString().Trim(),
                            IdAlmacen = Convert.ToInt32(DdlAlmacen.Text.Trim()),
                            CodBodega = Row["CodUbicaBodega"].ToString().Trim(),
                            CodShippingOrder = DdlNumCompra.Text.Trim(),
                            Posicion = Row["Pos"].ToString().Trim(),
                            CodAeronave = Convert.ToInt32(0),
                            Matricula = "",
                            DiaTasa = ViewState["DT"].ToString().Trim(),
                            MesTasa = ViewState["MT"].ToString().Trim(),
                            AnoTasa = ViewState["AT"].ToString().Trim(),
                            VlorTasaDM = 1,
                            CodTipoMoneda = ViewState["CodMoneda"].ToString().Trim(),
                            DocumentoNro = DdlNumCompra.Text.Trim(),
                            PosicionDocumento = Convert.ToInt32(Row["Pos"].ToString().Trim()),
                            Cant_Compra = Convert.ToDouble(Row["CantCompra"].ToString().Trim()),
                            Valor_Compra = Convert.ToDouble(Row["Valor_Compra"].ToString().Trim()),
                            UndMed_Compra = Row["CodUMCompra"].ToString().Trim(),
                            FacturaNro = "",
                            NumSolPed = "",
                            CCosto = Row["Ccosto"].ToString().Trim(),
                            AfectaInventario = Convert.ToInt32(Row["AfectaInventario"].ToString().Trim()),
                            CostoImportacion = 0,
                            CodTercero = ViewState["CodProv"].ToString().Trim(),
                            Consignacion = Convert.ToInt32(0),
                            CodIdUbicacion = Convert.ToInt32(Row["CodIdUbicacion"].ToString().Trim()),
                            //FechaVence = Convert.ToDateTime(Row["FechaShelfLife"]),
                            Observacion = TxtObserv.Text.Trim(),
                            ValorOT = Convert.ToDouble(0),
                            CodUsuarioReserva = "",
                            Proceso = "0112",
                            IdDetPropHk = Convert.ToInt32(0),
                            IdPPt = Convert.ToInt32(Row["PPT"]),
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
                    DdlNumCompra.Text = "";
                    TraerDatos("UPD");
                    BindDetCompra("UPD");
                    MultVw.ActiveViewIndex = 0;
                    GrdDtlleComp.DataSource = null;
                    GrdDtlleComp.DataBind();
                    GrdAsignar.DataSource = null;
                    GrdAsignar.DataBind();
                    GrdVisualizar.DataSource = null;
                    GrdVisualizar.DataBind();
                    DSDetalle.Tables["CurTemporal"].Clear();
                    DdlAlmacen.Enabled = true;
                    DdlNumCompra.Enabled = true;
                }
                catch (Exception Ex)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
                    string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                    Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, VbcatNArc, Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                }
            }
        }
    }
}