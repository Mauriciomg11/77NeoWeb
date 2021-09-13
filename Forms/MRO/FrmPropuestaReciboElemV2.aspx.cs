using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmPropuestaReciboElemV2 : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPpal = new DataSet();
        DataTable TblDetalle = new DataTable();
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
                BindDDdl("UPDATE");
                ViewState["CodTipoElem"] = "";
                ViewState["TipoElem"] = "";
                ViewState["Identif"] = "";
                TblDetalle.Columns.Add("PN", typeof(string));
                TblDetalle.Columns.Add("CodReferencia", typeof(string));
                TblDetalle.Columns.Add("SN", typeof(string));
                TblDetalle.Columns.Add("Descripcion", typeof(string));
                TblDetalle.Columns.Add("Cantidad", typeof(int));
                TblDetalle.Columns.Add("IdAlmacen", typeof(int));
                TblDetalle.Columns.Add("NomAlmacen", typeof(string));
                TblDetalle.Columns.Add("CodBodega", typeof(string));
                TblDetalle.Columns.Add("NomBodega", typeof(string));
                TblDetalle.Columns.Add("CodTipoElem", typeof(string));
                TblDetalle.Columns.Add("TipoElem", typeof(string));
                TblDetalle.Columns.Add("Identificador", typeof(string));
                ViewState["TblDetalle"] = TblDetalle;
                TblDetalle.Rows.Add(TblDetalle.NewRow());
                BindDDetTmp();
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
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
                    LblIndicaciones.Text = bO.Equals("LblIndicaciones") ? bT : LblIndicaciones.Text;
                    LblObserv.Text = bO.Equals("LblObserv") ? bT : LblObserv.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnIngresar.ToolTip = bO.Equals("BtnIngresarTT") ? bT : BtnIngresar.ToolTip;
                    BtnOpenElem.Text = bO.Equals("BtnOpenElem") ? bT : BtnOpenElem.Text;
                    BtnOpenElem.ToolTip = bO.Equals("BtnOpenElemTT") ? bT : BtnOpenElem.ToolTip;
                    GrdDetalle.Columns[1].HeaderText = bO.Equals("GrdRef") ? bT : GrdDetalle.Columns[1].HeaderText;
                    GrdDetalle.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdDetalle.Columns[2].HeaderText;
                    GrdDetalle.Columns[4].HeaderText = bO.Equals("CantMst") ? bT : GrdDetalle.Columns[4].HeaderText;
                    GrdDetalle.Columns[5].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdDetalle.Columns[5].HeaderText;
                    GrdDetalle.Columns[6].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdDetalle.Columns[6].HeaderText;
                    GrdDetalle.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDetalle.EmptyDataText;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnIngresarOnCl'");
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
                    string VbTxtSql = " EXEC SP_PANTALLA_Propuesta_ReciboElem 13,@Us,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTPpal = new DataSet())
                            {

                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTPpal);
                                DSTPpal.Tables[0].TableName = "PN";
                                DSTPpal.Tables[1].TableName = "SN";
                                DSTPpal.Tables[2].TableName = "Almacen";
                                DSTPpal.Tables[3].TableName = "Bodega";

                                ViewState["DSTPpal"] = DSTPpal;
                            }
                        }
                    }
                }
            }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTPpal = (DataSet)ViewState["DSTPpal"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            DataRow[] Result;
            string VblPn = "", VblSn = "";

            if (TxtObserv.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MstrMens22'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                return;
            }
            List<CsInsertElementoAlmacen> ObjDetalle = new List<CsInsertElementoAlmacen>();
            foreach (DataRow Row in TblDetalle.Rows)
            {
                var TypDetalle = new CsInsertElementoAlmacen()
                {
                    IdIE = Convert.ToInt32(0),
                    CodElemento = "",
                    CodReferencia = Row["CodReferencia"].ToString().Trim(),
                    PN = Row["PN"].ToString(),
                    SN = Row["SN"].ToString(),
                    Lote = "",
                    CodTipoElem = Row["CodTipoElem"].ToString().Trim(),
                    Identificador = Row["Identificador"].ToString().Trim(),
                    Descripcion = Row["Descripcion"].ToString().Trim(),
                    Cantidad = Convert.ToDouble(Row["Cantidad"].ToString().Trim()),
                    CantidadAnt = Convert.ToDouble(0),
                    Valor = Convert.ToDouble(0),
                    CodUndMed = "",
                    IdAlmacen = Convert.ToInt32(Row["IdAlmacen"].ToString().Trim()),
                    CodBodega = Row["CodBodega"].ToString().Trim(),
                    CodShippingOrder = "",
                    Posicion = "0",
                    CodAeronave=0,
                    Matricula = "",
                    CCosto = "",
                    AfectaInventario = Convert.ToInt32(0),
                    CostoImportacion = Convert.ToInt32(0),
                    CodTercero = "",
                    Consignacion = Convert.ToInt32(1),
                    CodIdUbicacion = Convert.ToInt32(0),
                    FechaVence = null,
                    Observacion = TxtObserv.Text.Trim(),
                    Proceso = "RecElemPpt",
                    IdDetPropHk = Convert.ToInt32(0),
                    IdPPt = Convert.ToInt32(0),
                    Accion = "ENTRADA",
                };
                ObjDetalle.Add(TypDetalle);
            }
            CsInsertElementoAlmacen ClaseIEA = new CsInsertElementoAlmacen();
            ClaseIEA.FormOrigen(ViewState["PFileName"].ToString());
            ClaseIEA.Alimentar(ObjDetalle);

            string Mensj = ClaseIEA.GetMensj();
            if (!Mensj.Equals(""))
            {
                VblPn = ClaseIEA.GetPn().Trim().Equals("") ? "" : "  [P/N: " + ClaseIEA.GetPn().Trim() + "]  ";
                VblSn = ClaseIEA.GetSn().Trim().Equals("") ? "" : " [S/N: " + ClaseIEA.GetSn().Trim() + "]";
                Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                foreach (DataRow row in Result)
                { Mensj = row["Texto"].ToString().Trim(); }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + VblPn + VblSn + "');", true);
                return;
            }
            TxtObserv.Text = ""; 
            TblDetalle.Clear();
            BindDDetTmp();
        }
        protected void BtnOpenElem_Click(object sender, EventArgs e)
        { Page.Title = ViewState["PageTit"].ToString().Trim(); Response.Redirect("~/Forms/InventariosCompras/FrmElemento.aspx"); }
        //************************ GridView Detalle ******************************
        protected void BindDDetTmp()
        {
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int VbNumReg = TblDetalle.Rows.Count;
            TblDetalle.AcceptChanges();
            foreach (DataRow row in TblDetalle.Rows)
            {
                object value = row["CodBodega"];
                if (value == DBNull.Value)
                {
                    if (VbNumReg > 1) { row.Delete(); BtnIngresar.Enabled = true; }
                }
            }
            TblDetalle.AcceptChanges();

            if (TblDetalle.Rows.Count > 0) { GrdDetalle.DataSource = TblDetalle; GrdDetalle.DataBind(); }
            else
            {
                TblDetalle.Rows.Add(TblDetalle.NewRow());
                GrdDetalle.DataSource = TblDetalle;
                GrdDetalle.DataBind();
                GrdDetalle.Rows[0].Cells.Clear();
                GrdDetalle.Rows[0].Cells.Add(new TableCell());
                GrdDetalle.Rows[0].Cells[0].Text = "Empty..!";
                GrdDetalle.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                BtnIngresar.Enabled = false;
                TblDetalle.NewRow();
                GrdDetalle.DataSource = TblDetalle;
                GrdDetalle.DataBind();
            }
        }
        protected void DdlPnP_TextChanged(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DSTPpal = (DataSet)ViewState["DSTPpal"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            string VbTextoNewSn = "";
            DropDownList DdlPnP = (GrdDetalle.FooterRow.FindControl("DdlPnP") as DropDownList);
            TextBox TxRefPP = (GrdDetalle.FooterRow.FindControl("TxRefPP") as TextBox);
            TextBox TxtDescPNPP = (GrdDetalle.FooterRow.FindControl("TxtDescPNPP") as TextBox);
            TextBox TxtCant = (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox);

            Result = DSTPpal.Tables[0].Select("PN= '" + DdlPnP.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            {
                TxRefPP.Text = Row["CodReferencia"].ToString().Trim();
                TxtDescPNPP.Text = Row["Descripcion"].ToString().Trim();
                ViewState["CodTipoElem"] = Row["CodTipoElemento"].ToString().Trim();
                ViewState["TipoElem"] = Row["Tipo"].ToString().Trim();
                ViewState["Identif"] = Row["IdentificadorElem"].ToString().Trim();
            }

            Result = Idioma.Select("Objeto= 'DdlPnP'");
            foreach (DataRow row in Result)
            { VbTextoNewSn = "- " + row["Texto"].ToString().Trim() + " -"; }

            DT = DSTPpal.Tables[1].Clone();
            DT.Rows.Add(" - ", "", "", "");
            if (ViewState["Identif"].ToString().Equals("SN"))
            {
                TxtCant.Enabled = false;
                TxtCant.Text = "1";
                DT.Rows.Add(VbTextoNewSn, "NEW", "", "");
                Result = DSTPpal.Tables[1].Select("PN= '" + DdlPnP.Text.Trim() + "'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
            }
            else { TxtCant.Enabled = true; TxtCant.Text = "0"; }

            DropDownList DdlSNPP = (GrdDetalle.FooterRow.FindControl("DdlSNPP") as DropDownList);
            TextBox TxtSNPP = (GrdDetalle.FooterRow.FindControl("TxtSNPP") as TextBox);
            DdlSNPP.Visible = true; TxtSNPP.Visible = false; TxtSNPP.Text = "";
            DdlSNPP.DataSource = DT;
            DdlSNPP.DataTextField = "SN";
            DdlSNPP.DataValueField = "CodSN";
            DdlSNPP.DataBind();
        }
        protected void DdlSNPP_TextChanged(object sender, EventArgs e)
        {
            DropDownList DdlSNPP = (GrdDetalle.FooterRow.FindControl("DdlSNPP") as DropDownList);
            TextBox TxtSNPP = (GrdDetalle.FooterRow.FindControl("TxtSNPP") as TextBox);
            if (DdlSNPP.Text.Trim().Equals("NEW")) { DdlSNPP.Visible = false; TxtSNPP.Visible = true; }
        }
        protected void GrdDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            DataRow[] Result;
            if (e.CommandName.Equals("AddNew"))
            {
                string VbSN = "", VbCant = "0";
                string VbPN = (GrdDetalle.FooterRow.FindControl("DdlPnP") as DropDownList).Text.Trim();
                string VbRef = (GrdDetalle.FooterRow.FindControl("TxRefPP") as TextBox).Text.Trim();
                string VbDesc = (GrdDetalle.FooterRow.FindControl("TxtDescPNPP") as TextBox).Text.Trim();
                if ((GrdDetalle.FooterRow.FindControl("DdlSNPP") as DropDownList).Visible == true)
                { VbSN = (GrdDetalle.FooterRow.FindControl("DdlSNPP") as DropDownList).Text.Trim(); }
                else { VbSN = (GrdDetalle.FooterRow.FindControl("TxtSNPP") as TextBox).Text.Trim(); }
                VbCant = (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim();
                string VbIdAlmac = (GrdDetalle.FooterRow.FindControl("DdlAlmaPP") as DropDownList).Text.Trim();
                string VbNomAlma = (GrdDetalle.FooterRow.FindControl("DdlAlmaPP") as DropDownList).SelectedItem.Text.Trim();
                string VbCodBod = (GrdDetalle.FooterRow.FindControl("DdlBodegPP") as DropDownList).Text.Trim();
                string VbNomBod = (GrdDetalle.FooterRow.FindControl("DdlBodegPP") as DropDownList).SelectedItem.Text.Trim();

                Result = TblDetalle.Select("PN= '" + VbPN + "' AND SN = '" + VbSN + "'");
                foreach (DataRow Row in Result)
                {
                    Result = Idioma.Select("Objeto= 'MensRcElm01'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El elemento ya se encuentra registrado..
                    return;
                }

                if (VbPN.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens16'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N.
                    return;
                }
                if (VbSN.Equals("") && ViewState["Identif"].ToString().Equals("SN"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens17'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una S/N.
                    return;
                }
                if (Convert.ToInt32(VbCant) <= 0)
                {
                    Result = Idioma.Select("Objeto= 'MstrMens18'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la cantidad.
                    return;
                }
                if (VbIdAlmac.Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un P/N.
                    return;
                }
                if (VbCodBod.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens20'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una S/N.
                    return;
                }
                TblDetalle.Rows.Add(VbPN, VbRef, VbSN, VbDesc, Convert.ToInt32(VbCant), Convert.ToInt32(VbIdAlmac), VbNomAlma, VbCodBod, VbNomBod, ViewState["CodTipoElem"], ViewState["TipoElem"], ViewState["Identif"].ToString());
                BindDDetTmp();
            }
        }
        protected void GrdDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            int index = Convert.ToInt32(e.RowIndex);
            TblDetalle.Rows[index].Delete();
            BindDDetTmp();
        }
        protected void GrdDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DSTPpal = (DataSet)ViewState["DSTPpal"];
                DropDownList DdlPnP = (e.Row.FindControl("DdlPnP") as DropDownList);
                DdlPnP.DataSource = DSTPpal.Tables[0];
                DdlPnP.DataTextField = "PN";
                DdlPnP.DataValueField = "CodPN";
                DdlPnP.DataBind();

                DropDownList DdlAlmaPP = (e.Row.FindControl("DdlAlmaPP") as DropDownList);
                DdlAlmaPP.DataSource = DSTPpal.Tables[2];
                DdlAlmaPP.DataTextField = "NomAlmacen";
                DdlAlmaPP.DataValueField = "CodIdAlmacen";
                DdlAlmaPP.DataBind();

                DropDownList DdlBodegPP = (e.Row.FindControl("DdlBodegPP") as DropDownList);
                DdlBodegPP.DataSource = DSTPpal.Tables[3];
                DdlBodegPP.DataTextField = "CodBodeg";
                DdlBodegPP.DataValueField = "CodUbicaBodega";
                DdlBodegPP.DataBind();

                ImageButton IbtAddNew = e.Row.FindControl("IbtAddNew") as ImageButton;

                if (IbtAddNew != null)
                {
                    IbtAddNew.Enabled = true;
                    Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
                }
            }
        }
    }
}