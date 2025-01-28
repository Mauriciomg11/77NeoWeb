using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.InventariosCompras
{
    public partial class FrmAlertaCalibracionHer : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTPpl = new DataSet();
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
                BtnSinConfigurar.CssClass = "btn btn-primary";
                ModSeguridad();
                BindBDatos("UPD");
                DSTPpl = (DataSet)ViewState["DSTPpl"];
                if (DSTPpl.Tables["Tipo"].Rows.Count > 0)
                {
                    DdlTipo.DataSource = DSTPpl.Tables[0];
                    DdlTipo.DataTextField = "Descripcion";
                    DdlTipo.DataValueField = "CodTipoElemento";
                    DdlTipo.DataBind();
                }

                DdlTipo.Text = "";
                MultVw.ActiveViewIndex = 0;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
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
                    TitForm.Text = bO.Equals("Caption") ? bT : TitForm.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT : LblTipo.Text;
                    BtnSinConfigurar.Text = bO.Equals("BtnSinConfigurar") ? bT : BtnSinConfigurar.Text;
                    BtnSinConfigurar.ToolTip = bO.Equals("BtnSinConfigurarTT") ? bT : BtnSinConfigurar.ToolTip;
                    BtnAbrirElem.Text = bO.Equals("BtnAbrirElem") ? bT : BtnAbrirElem.Text;
                    BtnAbrirElem.ToolTip = bO.Equals("BtnAbrirElemTT") ? bT : BtnAbrirElem.ToolTip;
                    LblTitProxVenc.Text = bO.Equals("LblTitProxVenc") ? bT : LblTitProxVenc.Text;
                    LblTitSinConf.Text = bO.Equals("LblTitSinConf") ? bT : LblTitSinConf.Text;
                    GrdProxVenc.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdProxVenc.EmptyDataText;
                    GrdSinConfg.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdSinConfg.EmptyDataText;
                    GrdProxVenc.Columns[0].HeaderText = bO.Equals("TipoMstr") ? bT : GrdProxVenc.Columns[0].HeaderText;
                    GrdSinConfg.Columns[0].HeaderText = bO.Equals("TipoMstr") ? bT : GrdSinConfg.Columns[0].HeaderText;
                   
                    GrdProxVenc.Columns[2].HeaderText = bO.Equals("GrdSnLote") ? bT : GrdProxVenc.Columns[2].HeaderText;
                    GrdSinConfg.Columns[2].HeaderText = bO.Equals("GrdSnLote") ? bT : GrdSinConfg.Columns[2].HeaderText;
                   
                    GrdSinConfg.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdSinConfg.Columns[3].HeaderText;
                    GrdProxVenc.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdProxVenc.Columns[3].HeaderText;
                   
                    GrdProxVenc.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdProxVenc.Columns[4].HeaderText;
                    GrdSinConfg.Columns[4].HeaderText = bO.Equals("Descripcion") ? bT : GrdSinConfg.Columns[4].HeaderText;
                    
                    GrdSinConfg.Columns[5].HeaderText = bO.Equals("GrdFecVen") ? bT : GrdSinConfg.Columns[5].HeaderText;
                    GrdProxVenc.Columns[5].HeaderText = bO.Equals("GrdFecVen") ? bT : GrdProxVenc.Columns[5].HeaderText;
                   
                    GrdProxVenc.Columns[6].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdProxVenc.Columns[6].HeaderText;
                    GrdSinConfg.Columns[6].HeaderText = bO.Equals("AlmacenMstr") ? bT : GrdSinConfg.Columns[6].HeaderText;
                   
                    GrdSinConfg.Columns[7].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdSinConfg.Columns[7].HeaderText;
                    GrdProxVenc.Columns[7].HeaderText = bO.Equals("BodegaMstr") ? bT : GrdProxVenc.Columns[7].HeaderText;
                   
                    GrdProxVenc.Columns[8].HeaderText = bO.Equals("CantMst") ? bT : GrdProxVenc.Columns[8].HeaderText;
                    GrdSinConfg.Columns[8].HeaderText = bO.Equals("CantMst") ? bT : GrdSinConfg.Columns[8].HeaderText;
                  
                    GrdProxVenc.Columns[9].HeaderText = bO.Equals("GrdRemnt") ? bT : GrdProxVenc.Columns[9].HeaderText;
                    IbtCloseSinConfg.ToolTip = bO.Equals("BtnCerrarMst") ? bT : IbtCloseSinConfg.ToolTip;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        public bool IsIENumerableLleno(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        protected void BindBDatos(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Ingenieria 1,'','','','','','',0,0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTPpl);
                                DSTPpl.Tables[0].TableName = "Tipo";
                                DSTPpl.Tables[1].TableName = "ProxVence";
                                DSTPpl.Tables[2].TableName = "SinConfig";

                                ViewState["DSTPpl"] = DSTPpl;
                            }
                        }
                    }
                }
            }
            DSTPpl = (DataSet)ViewState["DSTPpl"];
            DataRow[] DR;

            DataTable DT = new DataTable();
            DT = DSTPpl.Tables[1].Clone();
            if (DdlTipo.Text.Trim().Equals(""))
            {
                DR = DSTPpl.Tables[1].Select("CodTipoElemento <>'05'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
            }
            else
            {
                DR = DSTPpl.Tables[1].Select("CodTipoElemento ='" + DdlTipo.Text.Trim() + "'");
                if (IsIENumerableLleno(DR))
                { DT = DR.CopyToDataTable(); }
            }

            if (DT.Rows.Count > 0) { GrdProxVenc.DataSource = DT; }
            else { GrdProxVenc.DataSource = null; }
            GrdProxVenc.DataBind();

            DataTable DT2 = new DataTable();
            DT2 = DSTPpl.Tables[2].Clone();
            if (DdlTipo.Text.Trim().Equals(""))
            {
                DR = DSTPpl.Tables[2].Select("CodTipoElemento <>'05'");
                if (IsIENumerableLleno(DR))
                { DT2 = DR.CopyToDataTable(); }
            }
            else
            {
                DR = DSTPpl.Tables[2].Select("CodTipoElemento ='" + DdlTipo.Text.Trim() + "'");
                if (IsIENumerableLleno(DR))
                { DT2 = DR.CopyToDataTable(); }
            }

            if (DT2.Rows.Count > 0) { GrdSinConfg.DataSource = DT2; }
            else { GrdSinConfg.DataSource = null; }
            GrdSinConfg.DataBind();
        }
        protected void BtnSinConfigurar_Click(object sender, EventArgs e)
        { MultVw.ActiveViewIndex = 1; BtnSinConfigurar.Visible = false; }
        protected void DdlTipo_TextChanged(object sender, EventArgs e)
        { BindBDatos("SEL"); }
        protected void GrdProxVenc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbRemain = dr["Remanente"].ToString();
                if (Convert.ToInt32(VbRemain) < 0)
                { e.Row.BackColor = System.Drawing.Color.Red; e.Row.ForeColor = System.Drawing.Color.White; }
            }
        }
        protected void IbtCloseSinConfg_Click(object sender, ImageClickEventArgs e)
        { MultVw.ActiveViewIndex = 0; BtnSinConfigurar.Visible = true; }

        protected void BtnAbrirElem_Click(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            string SP = "window.open('/Forms/InventariosCompras/FrmElemento.aspx', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), SP, true);
        }
    }
}