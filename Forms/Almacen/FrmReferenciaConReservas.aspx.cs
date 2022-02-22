using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmReferenciaConReservas : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTDet = new DataTable();
        DataTable DTConslt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null) if (Session["Login77"] == null)
                {
                    if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
                }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = "XX";
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
                RdbSolPed.Checked = true;
                BindData("UPD");
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0)
            { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { }
            if (ClsP.GetCE1() == 0) { }
            if (ClsP.GetCE2() == 0) { }
            if (ClsP.GetCE3() == 0) { }
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }
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
                    RdbSolPed.Text = bO.Equals("RdbSolPed") ? "&nbsp" + bT : RdbSolPed.Text;
                    RdbPpt.Text = bO.Equals("RdbPpt") ? "&nbsp" + bT : RdbPpt.Text;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtBusqueda.ToolTip = bO.Equals("BtnConsultar") ? bT : IbtBusqueda.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    BtnAprobar.Text = bO.Equals("BtnAprobar") ? bT : BtnAprobar.Text;
                    IbtAprDetAll.ToolTip = bO.Equals("IbtAprDetAll") ? bT : IbtAprDetAll.ToolTip;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;

                    GrdBusq.Columns[1].HeaderText = bO.Equals("RdbSolPed") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("PosMstr") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[3].HeaderText = bO.Equals("FechaMstr") ? bT : GrdBusq.Columns[3].HeaderText;
                    GrdBusq.Columns[4].HeaderText = bO.Equals("PrioridadMstr") ? bT : GrdBusq.Columns[4].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("LblEstadoMst") ? bT : GrdBusq.Columns[5].HeaderText;
                    GrdBusq.Columns[6].HeaderText = bO.Equals("GrdPetic") ? bT : GrdBusq.Columns[6].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("CantMst") ? bT : GrdBusq.Columns[7].HeaderText;
                    GrdBusq.Columns[9].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdBusq.Columns[9].HeaderText;
                    GrdBusq.Columns[10].HeaderText = bO.Equals("GrdEstadoRef") ? bT : GrdBusq.Columns[10].HeaderText;
                    GrdBusq.Columns[11].HeaderText = bO.Equals("Descripcion") ? bT : GrdBusq.Columns[11].HeaderText;/**/
                    GrdBusq.Columns[12].HeaderText = bO.Equals("TipoMstr") ? bT : GrdBusq.Columns[12].HeaderText;/**/
                    GrdBusq.Columns[13].HeaderText = bO.Equals("GrdPpt") ? bT : GrdBusq.Columns[13].HeaderText;/**/
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnAprobarOnCl'");
                foreach (DataRow row in Result) { BtnAprobar.OnClientClick = "return confirm('" + row["Texto"].ToString().Trim() + "');"; }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindData(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC Consultas_General_Logistica2 6,'','','','','',0, 0,@Idm,@ICC,'01-01-1','01-01-1'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            DTDet = (DataTable)ViewState["DTDet"];
            DataRow[] Result;
            DataTable DT = new DataTable();

            DT = DTDet.Clone();
            if (RdbSolPed.Checked == true)
            {
                Result = DTDet.Select("CodPedido LIKE '%" + TxtBusqueda.Text.Trim() + "%'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
            }
            else
            {
                Result = DTDet.Select("PPT LIKE '%" + TxtBusqueda.Text.Trim() + "%'");
                foreach (DataRow Row in Result)
                { DT.ImportRow(Row); }
            }

            if (DT.Rows.Count > 0) { DataView DV = DT.DefaultView; DV.Sort = "CodPedido DESC,Posicion"; DT = DV.ToTable(); GrdBusq.DataSource = DT; }
            else { GrdBusq.DataSource = null; }
            GrdBusq.DataBind();
            ViewState["DTConslt"] = DT;
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BindData("SEL"); }
        protected void IbtAprDetAll_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DTDet = (DataTable)ViewState["DTDet"];
            DTConslt = (DataTable)ViewState["DTConslt"];

            foreach (DataRow Dtll in DTConslt.Rows)
            {
                if (!Dtll["CodReferencia"].ToString().Trim().Equals(""))
                    Dtll["OK"] = "1";
            }
            GrdBusq.DataSource = DTConslt;
            GrdBusq.DataBind();
        }
        protected void BtnAprobar_Click(object sender, EventArgs e)
        {
            DTConslt = (DataTable)ViewState["DTConslt"];
            // DTConslt.AcceptChanges();
            DataTable TblDetPedido = new DataTable();
            TblDetPedido.Columns.Add("IdDetPedido", typeof(int));
            TblDetPedido.Columns.Add("CodReferencia", typeof(string));
            TblDetPedido.Columns.Add("PN", typeof(string));
            TblDetPedido.Columns.Add("CodUndMedida", typeof(string));
            TblDetPedido.Columns.Add("CantidadTotal", typeof(double));
            TblDetPedido.Columns.Add("CantidadAlmacen", typeof(double));
            TblDetPedido.Columns.Add("CantidadReparacion", typeof(double));
            TblDetPedido.Columns.Add("CantidadOrden", typeof(double));
            TblDetPedido.Columns.Add("Posicion", typeof(int));
            TblDetPedido.Columns.Add("AprobacionDetalle", typeof(int));
            TblDetPedido.Columns.Add("CodSeguimiento", typeof(string));
            TblDetPedido.Columns.Add("Descripcion", typeof(string));
            TblDetPedido.Columns.Add("TipoPedido", typeof(int));
            TblDetPedido.Columns.Add("CantidadAjustada", typeof(double));
            TblDetPedido.Columns.Add("Notas", typeof(string));
            TblDetPedido.Columns.Add("PosicionPr", typeof(int));
            TblDetPedido.Columns.Add("IdSrvPr", typeof(int));
            TblDetPedido.Columns.Add("IdReporte", typeof(int));
            TblDetPedido.Columns.Add("IdDetProPSrvSP", typeof(int));
            TblDetPedido.Columns.Add("CodIdDetalleResSP", typeof(int));
            TblDetPedido.Columns.Add("FechaAprob", typeof(DateTime));
            TblDetPedido.Columns.Add("CodAeronaveSP", typeof(int));
            foreach (GridViewRow Row in GrdBusq.Rows)
            {
                CheckBox LblPosP = Row.FindControl("CkbAprobP") as CheckBox;
                if (LblPosP.Checked == true)
                {
                    int IdDetPed = Convert.ToInt32(GrdBusq.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString());
                    string VbRef = (Row.FindControl("LblRef") as Label).Text.Trim();
                    string VbPn = (Row.FindControl("LblPN") as Label).Text.Trim();
                    double VbCntT = Convert.ToDouble((Row.FindControl("LblCant") as Label).Text.Trim());
                    int VbPosc = Convert.ToInt32((Row.FindControl("LblPosc") as Label).Text.Trim());
                    string VbDescr = (Row.FindControl("LblDescr") as Label).Text.Trim();
                    string VbFec = Cnx.ReturnFecha(GrdBusq.DataKeys[Row.RowIndex].Values["FechaDMY"].ToString());

                    TblDetPedido.Rows.Add(IdDetPed, VbRef, VbPn, "UndMed", VbCntT, 1, 0, 1, VbPosc, 1, "SOL", VbDescr, 1, 1, "Notas", 0, 0, 0, 0, 0, Convert.ToDateTime(VbFec), 0);
                }
            }
            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "Aprobar_SolPedido";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@DetSP", TblDetPedido);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@IdConfigCia", Session["!dC!@"].ToString());
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SC.ExecuteNonQuery();
                            transaction.Commit();
                            BindData("UPD");
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                            string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Aprobación Pedido", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);

                        }
                    }
                }
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CkbAprobP = (e.Row.FindControl("CkbAprobP") as CheckBox);
                DataRowView dr = e.Row.DataItem as DataRowView;
                if (dr["CodReferencia"].ToString().Equals("")) { CkbAprobP.Enabled = false; e.Row.BackColor = System.Drawing.Color.DarkOrange; }//
                if (dr["CodEstadoPn"].ToString().Equals("03")) { e.Row.BackColor = System.Drawing.Color.DarkRed; e.Row.ForeColor = System.Drawing.Color.White; }
            }
        }


    }
}