using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgAlmacen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmCargaInicial : System.Web.UI.Page
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
                TblDetalle.Columns.Add("PN", typeof(string));
                TblDetalle.Columns.Add("CodReferencia", typeof(string));
                TblDetalle.Columns.Add("SN", typeof(string));
                TblDetalle.Columns.Add("Lote", typeof(string));
                TblDetalle.Columns.Add("Descripcion", typeof(string));
                TblDetalle.Columns.Add("Valor", typeof(double));
                TblDetalle.Columns.Add("Cantidad", typeof(int));
                TblDetalle.Columns.Add("IdAlmacen", typeof(int));
                TblDetalle.Columns.Add("NomAlmacen", typeof(string));
                TblDetalle.Columns.Add("CodBodega", typeof(string));
                TblDetalle.Columns.Add("NomBodega", typeof(string));
                TblDetalle.Columns.Add("FechaExp", typeof(DateTime));
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

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string borr = Session["Nit77Cia"].ToString();
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,1,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["!dC!@"].ToString());
                SC.Parameters.AddWithValue("@F", "FrmCargaInicial");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("N")) //Consignacion
                    { CkbConsign.Visible = false; }
                }
            }
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
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnIngresar.ToolTip = bO.Equals("BtnIngresarTT") ? bT : BtnIngresar.ToolTip;
                    BtnOpenElem.Text = bO.Equals("BtnOpenElem") ? bT : BtnOpenElem.Text;
                    BtnOpenElem.ToolTip = bO.Equals("BtnOpenElemTT") ? bT : BtnOpenElem.ToolTip;
                    CkbConsign.Text = bO.Equals("CkbConsign") ? "&nbsp" + bT : CkbConsign.Text;
                    CkbConsign.ToolTip = bO.Equals("CkbConsignTT") ? bT : CkbConsign.ToolTip;
                    GrdDetalle.Columns[1].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDetalle.Columns[1].HeaderText;
                    GrdDetalle.Columns[2].HeaderText = bO.Equals("Descripcion") ? bT : GrdDetalle.Columns[2].HeaderText;
                    GrdDetalle.Columns[4].HeaderText = bO.Equals("LoteMst") ? bT : GrdDetalle.Columns[4].HeaderText;
                    GrdDetalle.Columns[5].HeaderText = bO.Equals("ValorMstr") ? bT : GrdDetalle.Columns[5].HeaderText;
                    GrdDetalle.Columns[6].HeaderText = bO.Equals("CantMst") ? bT : GrdDetalle.Columns[6].HeaderText;
                    GrdDetalle.Columns[7].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdDetalle.Columns[7].HeaderText;
                    GrdDetalle.Columns[8].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdDetalle.Columns[8].HeaderText;
                    GrdDetalle.Columns[9].HeaderText = bO.Equals("GrdFechExp") ? bT : GrdDetalle.Columns[9].HeaderText;
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
                    string VbTxtSql = " EXEC SP_PANTALLA_Carga_Inicial 11, @Us,'','','',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
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
            string VblPn, VblSn;
            int VbAfectaInv, VbConsignacion;

            if (TxtObserv.Text.Trim().Equals(""))
            {
                Result = Idioma.Select("Objeto= 'MstrMens22'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                return;
            }
            if (CkbConsign.Checked == true) { VbAfectaInv = 0; VbConsignacion = 1; }// Consignacion
            else { VbAfectaInv = 1; VbConsignacion = 0; }// no es consignacion
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
                    Lote = Row["Lote"].ToString(),
                    CodTipoElem = Row["CodTipoElem"].ToString().Trim(),
                    Identificador = Row["Identificador"].ToString().Trim(),
                    Descripcion = Row["Descripcion"].ToString().Trim(),
                    Cantidad = Convert.ToDouble(Row["Cantidad"].ToString().Trim()),
                    CantidadAnt = Convert.ToDouble(0),
                    Valor = Convert.ToDouble(Row["Valor"].ToString().Trim()),
                    CodUndMed = "",
                    IdAlmacen = Convert.ToInt32(Row["IdAlmacen"].ToString().Trim()),
                    CodBodega = Row["CodBodega"].ToString().Trim(),
                    CodShippingOrder = "0",
                    Posicion = "0",
                    CodAeronave = 0,
                    Matricula = "",
                    CCosto = "",
                    AfectaInventario = VbAfectaInv,
                    CostoImportacion = Convert.ToInt32(0),
                    CodTercero = "",
                    Consignacion = Convert.ToInt32(VbConsignacion),
                    CodIdUbicacion = Convert.ToInt32(0),
                    FechaVence = Convert.ToDateTime(Row["FechaExp"].ToString().Trim()),
                    Observacion = TxtObserv.Text.Trim(),
                    ValorOT = Convert.ToDouble(0),
                    CodUsuarioReserva = "",
                    Proceso = "FrmCargaInicial",
                    IdDetPropHk = Convert.ToInt32(0),
                    IdPPt = CkbConsign.Checked == true ? 1 : 0,
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
                VblSn = ClaseIEA.GetSn().Trim().Equals("") ? "" : " [S/N: " + ClaseIEA.GetSn().Trim() + "] ";
                string VbLote = ClaseIEA.GetLote().Trim().Equals("") ? "" : " [LT/N: " + ClaseIEA.GetLote().Trim() + "]";
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
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
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

            DropDownList DdlPnP = (GrdDetalle.FooterRow.FindControl("DdlPnP") as DropDownList);
            TextBox TxRefPP = (GrdDetalle.FooterRow.FindControl("TxRefPP") as TextBox);
            TextBox TxtDescPNPP = (GrdDetalle.FooterRow.FindControl("TxtDescPNPP") as TextBox);
            TextBox TxtSNPP = (GrdDetalle.FooterRow.FindControl("TxtSNPP") as TextBox);
            TextBox TxtLotPP = (GrdDetalle.FooterRow.FindControl("TxtLotPP") as TextBox);
            TextBox TxtCant = (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox);
            TextBox TxtVlr = (GrdDetalle.FooterRow.FindControl("TxtVlr") as TextBox);
            TextBox TxtFechExp = (GrdDetalle.FooterRow.FindControl("TxtFechExp") as TextBox);

            TxtSNPP.Text = ""; TxtSNPP.Enabled = false;
            TxtLotPP.Text = ""; TxtLotPP.Enabled = false;
            TxtCant.Text = "0"; TxtCant.Enabled = true;
            TxtFechExp.Text = ""; TxtFechExp.Enabled = false;
            TxtVlr.Text = "0"; TxtVlr.Enabled = false;

            if (CkbConsign.Checked == false) { TxtVlr.Enabled = true; }// no es Consignacion

            Result = DSTPpal.Tables[0].Select("PN= '" + DdlPnP.Text.Trim() + "'");
            foreach (DataRow Row in Result)
            {
                TxRefPP.Text = Row["CodReferencia"].ToString().Trim();
                TxtDescPNPP.Text = Row["Descripcion"].ToString().Trim();
                ViewState["CodTipoElem"] = Row["CodTipoElemento"].ToString().Trim();
                ViewState["TipoElem"] = Row["Tipo"].ToString().Trim();
                ViewState["Identif"] = Row["IdentificadorElem"].ToString().Trim();
                ViewState["FechaVencPN"] = Row["FechaVencPN"].ToString().Trim();
            }

            if (ViewState["Identif"].ToString().Equals("SN")) { TxtCant.Enabled = false; TxtCant.Text = "1"; TxtSNPP.Enabled = true; }
            if (ViewState["Identif"].ToString().Equals("LOTE")) { TxtLotPP.Enabled = true; }
            if (ViewState["FechaVencPN"].ToString().Equals("1")) { TxtFechExp.Enabled = true; }
        }
        protected void DdlAlmaPP_TextChanged(object sender, EventArgs e)
        {
            DSTPpal = (DataSet)ViewState["DSTPpal"];
            DataRow[] Result;
            DataTable DT = new DataTable();
            DropDownList DdlAlmaPP = (GrdDetalle.FooterRow.FindControl("DdlAlmaPP") as DropDownList);
            if (CkbConsign.Checked == true)// Consignacion
            {
                Result = DSTPpal.Tables["Bodega"].Select("CodTercero = 'N/A' OR  (CodAlmacen = " + DdlAlmaPP.Text.Trim() + " AND  CodTercero = 'Tercero' AND CodEstadoBodega IN ('01','25'))");
                if (IsIENumerableLleno(Result))
                { DT = Result.CopyToDataTable(); }
            }
            else// no es consignacion
            {
                Result = DSTPpal.Tables["Bodega"].Select("CodTercero = 'N/A' OR  (CodAlmacen = " + DdlAlmaPP.Text.Trim() + " AND  CodTercero = '')");
                if (IsIENumerableLleno(Result))
                { DT = Result.CopyToDataTable(); }
            }
            DropDownList DdlBodegPP = (GrdDetalle.FooterRow.FindControl("DdlBodegPP") as DropDownList);

            DdlBodegPP.DataSource = DT;
            DdlBodegPP.DataTextField = "FC";
            DdlBodegPP.DataValueField = "CodBodega";
            DdlBodegPP.DataBind();
        }
        protected void GrdDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            TblDetalle = (DataTable)ViewState["TblDetalle"];
            DSTPpal = (DataSet)ViewState["DSTPpal"];
            DataRow[] Result;
            if (e.CommandName.Equals("AddNew"))
            {
                string VbCant = "0";
                string VbPN = (GrdDetalle.FooterRow.FindControl("DdlPnP") as DropDownList).Text.Trim();
                string VbRef = (GrdDetalle.FooterRow.FindControl("TxRefPP") as TextBox).Text.Trim();
                string VbDesc = (GrdDetalle.FooterRow.FindControl("TxtDescPNPP") as TextBox).Text.Trim();
                string VbSN = (GrdDetalle.FooterRow.FindControl("TxtSNPP") as TextBox).Text.Trim();
                string VbLot = (GrdDetalle.FooterRow.FindControl("TxtLotPP") as TextBox).Text.Trim();
                VbCant = (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetalle.FooterRow.FindControl("TxtCant") as TextBox).Text.Trim();
                string VbVlr = (GrdDetalle.FooterRow.FindControl("TxtVlr") as TextBox).Text.Trim().Equals("") ? "0" : (GrdDetalle.FooterRow.FindControl("TxtVlr") as TextBox).Text.Trim();
                string VbIdAlmac = (GrdDetalle.FooterRow.FindControl("DdlAlmaPP") as DropDownList).Text.Trim();
                string VbNomAlma = (GrdDetalle.FooterRow.FindControl("DdlAlmaPP") as DropDownList).SelectedItem.Text.Trim();

                if (CkbConsign.Checked == true) { VbVlr = "0"; }// Consignacion
                if (VbIdAlmac.Equals("0"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens19'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el almacén.
                    return;
                }
                string VbCodBod = (GrdDetalle.FooterRow.FindControl("DdlBodegPP") as DropDownList).Text.Trim();
                string VbNomBod = (GrdDetalle.FooterRow.FindControl("DdlBodegPP") as DropDownList).SelectedItem.Text.Trim();
                string VbFecha = (GrdDetalle.FooterRow.FindControl("TxtFechExp") as TextBox).Text.Trim();

                if (ViewState["Identif"].ToString().Equals("SN"))
                {
                    Result = TblDetalle.Select("PN= '" + VbPN + "' AND SN = '" + VbSN + "'");
                    foreach (DataRow Row in Result)
                    {
                        Result = Idioma.Select("Objeto= 'MensRcElm01'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El elemento ya se encuentra registrado..
                        return;
                    }
                }
                else
                {
                    Result = TblDetalle.Select("PN= '" + VbPN + "' AND Lote = '" + VbLot + "'");
                    foreach (DataRow Row in Result)
                    {
                        Result = Idioma.Select("Objeto= 'MensRcElm01'");
                        foreach (DataRow row in Result)
                        { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El elemento ya se encuentra registrado..
                        return;
                    }
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
                if (VbLot.Equals("") && ViewState["Identif"].ToString().Equals("LOTE"))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens24'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el número del lote.
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
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar el almacén.
                    return;
                }
                if (VbCodBod.Equals(""))
                {
                    Result = Idioma.Select("Objeto= 'MstrMens20'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la bodega.
                    return;
                }
                if (ViewState["FechaVencPN"].ToString().Equals("1"))
                {
                    string Mensj = Cnx.ValidarFechas2(VbFecha.Trim(), "", 1);
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

                if (CkbConsign.Checked == true)// Consignacion
                {
                    Result = DSTPpal.Tables["Bodega"].Select("CodBodega = '" + VbCodBod.Trim() + "'");
                    foreach (DataRow row in Result)
                    {
                        if (row["CodTercero"].ToString().Trim().Equals(""))
                        {
                            Result = Idioma.Select("Objeto= 'MensCargIni01'");
                            foreach (DataRow Row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }
                            return;
                        }
                    }

                    Result = Idioma.Select("Objeto= 'BtnIngresarOnCl2'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                else
                {
                    Result = DSTPpal.Tables["Bodega"].Select("CodBodega = '" + VbCodBod.Trim() + "'");
                    foreach (DataRow row in Result)
                    {
                        if (!row["CodTercero"].ToString().Trim().Equals(""))
                        {
                            Result = Idioma.Select("Objeto= 'MensCargIni02'");
                            foreach (DataRow Row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Row["Texto"].ToString() + "');", true); }
                            return;
                        }
                    }

                    Result = Idioma.Select("Objeto= 'BtnIngresarOnCl1'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                VbFecha = Cnx.ReturnFecha(VbFecha);//
                VbFecha = VbFecha.Equals("") ? "01/01/1900" : VbFecha;
                TblDetalle.Rows.Add(VbPN, VbRef, VbSN, VbLot, VbDesc, Convert.ToDouble(VbVlr), Convert.ToInt32(VbCant), Convert.ToInt32(VbIdAlmac), VbNomAlma, VbCodBod, VbNomBod, Convert.ToDateTime(VbFecha), ViewState["CodTipoElem"], ViewState["TipoElem"], ViewState["Identif"].ToString());
                BindDDetTmp();
                CkbConsign.Enabled = false;
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