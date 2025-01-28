using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmPropuestaEntregaElem : System.Web.UI.Page
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
            Page.Title = "";
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
                TitForm.Text = "";
                ModSeguridad();
                BindDDdl("UPDATE");
                ViewState["CodTipoElem"] = "";
                ViewState["TipoElem"] = "";
                ViewState["Identif"] = "";
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
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false;
            }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["CE1"] = 0; }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { ViewState["CE3"] = 0; }
            if (ClsP.GetCE4() == 0) { ViewState["CE4"] = 0; }
            if (ClsP.GetCE5() == 0) { ViewState["CE5"] = 0; }
            if (ClsP.GetCE6() == 0) { ViewState["CE6"] = 0; }
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
                    LblObserv.Text = bO.Equals("LblObserv") ? bT : LblObserv.Text;
                    LblPpt.Text = bO.Equals("LblPpt") ? bT + ":" : LblPpt.Text;
                    LblHk.Text = bO.Equals("LblHk") ? bT + ":" : LblHk.Text;

                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnIngresar.ToolTip = bO.Equals("BtnIngresarTT") ? bT : BtnIngresar.ToolTip;
                    GrdDetalle.Columns[2].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDetalle.Columns[2].HeaderText;
                    GrdDetalle.Columns[3].HeaderText = bO.Equals("Descripcion") ? bT : GrdDetalle.Columns[3].HeaderText;
                    GrdDetalle.Columns[5].HeaderText = bO.Equals("CantMst") ? bT : GrdDetalle.Columns[5].HeaderText;
                    GrdDetalle.Columns[6].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdDetalle.Columns[6].HeaderText;
                    GrdDetalle.Columns[7].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdDetalle.Columns[7].HeaderText;
                    GrdDetalle.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDetalle.EmptyDataText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnCl1'");
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDDdl(string Accion)
        {
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_PropuestaEntregaElem 8,@Us,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {

                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "NumPPT";
                                DSTDdl.Tables[1].TableName = "Almacen";
                                DSTDdl.Tables[2].TableName = "HK";
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            DdlPpt.DataSource = DSTDdl.Tables[0];
            DdlPpt.DataTextField = "NumPpt";
            DdlPpt.DataValueField = "IdPropuesta";
            DdlPpt.DataBind();

            DdlHk.DataSource = DSTDdl.Tables[2];
            DdlHk.DataTextField = "Matricula";
            DdlHk.DataValueField = "CodAeronave";
            DdlHk.DataBind();
        }
        protected void DdlPpt_TextChanged(object sender, EventArgs e)
        {
            if (DdlPpt.Text.Trim().Equals("0")) { BtnIngresar.Enabled = false; LblCliente.Text = ""; }

            DataTable DT = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_PropuestaEntregaElem 4,@Us,'','','',@PT,0,@ICC,0,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@PT", DdlPpt.Text.Trim());
                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                    {
                        SDA.SelectCommand = SC;
                        SDA.Fill(DT);
                        if (DT.Rows.Count > 0)
                        {
                            GrdDetalle.DataSource = DT; GrdDetalle.DataBind(); BtnIngresar.Enabled = true;
                            DSTDdl = (DataSet)ViewState["DSTDdl"];
                            DataRow[] Result = DSTDdl.Tables[0].Select("IdPropuesta=" + DdlPpt.Text.Trim());
                            foreach (DataRow Row in Result)
                            {
                                ViewState["CodTercero"] = Row["CodTercero"].ToString().Trim();
                                LblCliente.Text = "[" + Row["CodTercero"].ToString().Trim() + "] - [" + Row["RazonSocial"].ToString().Trim() + "]";
                            }
                        }
                        else { GrdDetalle.DataSource = null; GrdDetalle.DataBind(); BtnIngresar.Enabled = false; LblCliente.Text = ""; }
                    }
                }
            }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;

            if (TxtObserv.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MstrMens22'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                return;
            }

            List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
            foreach (GridViewRow Row in GrdDetalle.Rows)
            {
                CheckBox CkbSelP = Row.FindControl("CkbSelP") as CheckBox;
                if (CkbSelP.Checked == true)
                {
                    var TypDetalle = new CsInsertElementoAlmacen()
                    {
                        IdIE = Convert.ToInt32(0),
                        CodElemento = GrdDetalle.DataKeys[Row.RowIndex].Values["CodElemento"].ToString().Trim(),
                        CodReferencia = (Row.FindControl("LblRef") as Label).Text.Trim(),
                        PN = (Row.FindControl("LblPn") as Label).Text.Trim(),
                        SN = (Row.FindControl("LblSn") as Label).Text.Trim(),
                        Lote = "",
                        CodTipoElem = GrdDetalle.DataKeys[Row.RowIndex].Values["CodTipoElem"].ToString().Trim(),
                        Identificador = GrdDetalle.DataKeys[Row.RowIndex].Values["IdentificadorElem"].ToString().Trim(),
                        Descripcion = (Row.FindControl("LblDesc") as Label).Text.Trim(),
                        Cantidad = Convert.ToDouble((Row.FindControl("LblCant") as Label).Text.Trim()),
                        CantidadAnt = Convert.ToDouble(0),
                        Valor = Convert.ToDouble(0),
                        CodUndMed = GrdDetalle.DataKeys[Row.RowIndex].Values["CodUndMed"].ToString().Trim(),
                        IdAlmacen = Convert.ToInt32(GrdDetalle.DataKeys[Row.RowIndex].Values["CodIdAlmacen"].ToString().Trim()),
                        CodBodega = GrdDetalle.DataKeys[Row.RowIndex].Values["CodBodega"].ToString().Trim(),
                        CodShippingOrder = "",
                        Posicion = "0",
                        CodAeronave = Convert.ToInt32(DdlHk.Text.Trim()),
                        Matricula = DdlHk.SelectedItem.Text.Trim(),
                        CCosto = "",
                        AfectaInventario = Convert.ToInt32(0),
                        CostoImportacion = Convert.ToInt32(0),
                        CodTercero = ViewState["CodTercero"].ToString(),
                        Consignacion = Convert.ToInt32(1),
                        CodIdUbicacion = Convert.ToInt32(GrdDetalle.DataKeys[Row.RowIndex].Values["CodIdUbicacion"].ToString().Trim()),
                        FechaVence = null,
                        Observacion = TxtObserv.Text.Trim(),
                        ValorOT = Convert.ToDouble(0),
                        CodUsuarioReserva = "",
                        Proceso = "EntregaElemPpt",
                        IdDetPropHk = Convert.ToInt32(GrdDetalle.DataKeys[Row.RowIndex].Values["IdDetPropHk"].ToString().Trim()),
                        IdPPt = Convert.ToInt32(DdlPpt.Text.Trim()),
                        Accion = "SALIDA",
                    };
                    ObjDetalle.Add(TypDetalle);
                }
            }
            CsInsertElementoAlmacen ClaseIEA = new CsInsertElementoAlmacen();
            ClaseIEA.FormOrigen(ViewState["PFileName"].ToString());
            ClaseIEA.Alimentar(ObjDetalle);

            string Mensj = ClaseIEA.GetMensj();
            if (!Mensj.Equals(""))
            {
                string VbPptMaster = ClaseIEA.GetLote().Trim().Equals("") ? "" : "[" + ClaseIEA.GetLote().Trim() + "]";
                string VbPNMen = ClaseIEA.GetPn().Trim().Equals("") ? "" : "[" + ClaseIEA.GetPn().Trim() + "]";
                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "  " + VbPptMaster + "  " + VbPNMen + "');", true);
                return;
            }
            TxtObserv.Text = "";
            LblCliente.Text = ""; TxtObserv.Text = ""; DdlPpt.Text = "0"; DdlHk.Text = "0"; BtnIngresar.Enabled = false;
            GrdDetalle.DataSource = null; GrdDetalle.DataBind();
        }
        protected void GrdDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}