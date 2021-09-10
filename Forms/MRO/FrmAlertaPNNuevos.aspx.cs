using _77NeoWeb.prg;
using _77NeoWeb.Prg.prgMro;
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
    public partial class FrmAlertaPNNuevos : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DS = new DataSet();
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
                BindDetalle("UPDATE");
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblImpMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblCE1"] = 1;
            ViewState["VblCE2"] = 1;
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; }//
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }//
            if (ClsP.GetCE4() == 0) { ViewState["VblCE4"] = 0; }//                          

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
                    BtnEditar.Text = bO.Equals("BtnModificar") ? bT : BtnEditar.Text;
                    BtnReferencia.Text = bO.Equals("ReferenciaMst") ? bT : BtnReferencia.Text;
                    BtnReferencia.ToolTip = bO.Equals("BtnReferenciaTT") ? bT : BtnReferencia.ToolTip;
                    GrdDet.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdDet.EmptyDataText;
                    GrdDet.Columns[1].HeaderText = bO.Equals("GrdNewPN") ? bT : GrdDet.Columns[1].HeaderText;
                    GrdDet.Columns[2].HeaderText = bO.Equals("GrdDesc") ? bT : GrdDet.Columns[2].HeaderText;
                    GrdDet.Columns[3].HeaderText = bO.Equals("ReferenciaMst") ? bT : GrdDet.Columns[3].HeaderText;
                    GrdDet.Columns[4].HeaderText = bO.Equals("GrdUndMed") ? bT : GrdDet.Columns[4].HeaderText;
                    GrdDet.Columns[6].HeaderText = bO.Equals("GrdCant") ? bT : GrdDet.Columns[6].HeaderText;
                    GrdDet.Columns[7].HeaderText = bO.Equals("GrdOT") ? bT : GrdDet.Columns[7].HeaderText;
                    GrdDet.Columns[8].HeaderText = bO.Equals("GrdRte") ? bT : GrdDet.Columns[8].HeaderText;
                    GrdDet.Columns[9].HeaderText = bO.Equals("GrdPpt") ? bT : GrdDet.Columns[9].HeaderText;
                    GrdDet.Columns[10].HeaderText = bO.Equals("GrdHK") ? bT : GrdDet.Columns[10].HeaderText;
                    GrdDet.Columns[11].HeaderText = bO.Equals("GrdSolP") ? bT : GrdDet.Columns[11].HeaderText;
                    GrdDet.Columns[12].HeaderText = bO.Equals("GrdSvc") ? bT : GrdDet.Columns[12].HeaderText;
                    GrdDet.Columns[13].HeaderText = bO.Equals("GrdSolPor") ? bT : GrdDet.Columns[13].HeaderText;
                    GrdDet.Columns[14].HeaderText = bO.Equals("GrdFech") ? bT : GrdDet.Columns[14].HeaderText;
                }
                DataRow[] Result;
                Result = Idioma.Select("Objeto= 'MensConfMod'");
                foreach (DataRow row in Result) { BtnEditar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDetalle(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC DetallePNSinCrear @Us, @ICC";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Us", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DS = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DS);

                                DS.Tables[0].TableName = "Detalle";
                                DS.Tables[1].TableName = "PN";
                                ViewState["DS"] = DS;
                            }
                        }
                    }
                }
            }
            DS = (DataSet)ViewState["DS"];
            GrdDet.DataSource = DS.Tables[0]; GrdDet.DataBind();
        }
        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbTienReg = "N";
            List<CsTypPnNoExistente> ObjPnNoExistente = new List<CsTypPnNoExistente>();
            foreach (GridViewRow Row in GrdDet.Rows)
            {
                string TxtPnNew = (Row.FindControl("TxtPnNew") as TextBox).Text.Trim();

                if (!TxtPnNew.Equals(""))
                {
                    VbTienReg = "S";
                    var TypPnNoExistente = new CsTypPnNoExistente()
                    {
                        IdPnNoExistente = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdPnNoExistente"].ToString().Trim()),
                        PnNoExistente = (Row.FindControl("LblPnSol") as Label).Text.Trim(),
                        PnNuevo = (Row.FindControl("TxtPnNew") as TextBox).Text.Trim(),
                        Descripcion = (Row.FindControl("LblDesc") as Label).Text.Trim(),
                        CantSolicitada = Convert.ToDouble((Row.FindControl("LblCantS") as Label).Text.Trim()),
                        CodAeronave = Convert.ToInt32(0),
                        Reporte = Convert.ToInt32((Row.FindControl("LblRte") as Label).Text.Trim()),
                        CodOrdenTrabajo = Convert.ToInt32((Row.FindControl("LblOt") as Label).Text.Trim()),
                        CodIdDetalleRes = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["CodIdDetalleRes"].ToString().Trim()),
                        Matricula = (Row.FindControl("LblHk") as Label).Text.Trim(),
                        Usu = Session["C77U"].ToString(),
                        IdDetPedido = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPedido"].ToString().Trim()),
                        IdPropuesta = Convert.ToInt32((Row.FindControl("LblPpt") as Label).Text.Trim()),
                        IdDetPropuesta = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPropuesta"].ToString().Trim()),
                        IdDetPropHk = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdDetPropHk"].ToString().Trim()),
                        CodIdDetElemPlanInstrumento = Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["CodIdDetElemPlanInstrumento"].ToString().Trim()),
                        IdSrvc= Convert.ToInt32(GrdDet.DataKeys[Row.RowIndex].Values["IdSrvManto"].ToString().Trim()),
                        CodPedido = GrdDet.DataKeys[Row.RowIndex].Values["CodPedido"].ToString().Trim(),
                        IdConfigCia = (int)Session["!dC!@"],
                    };
                    ObjPnNoExistente.Add(TypPnNoExistente);
                }
            }
            if (VbTienReg.Equals("S"))
            {
                CsTypPnNoExistente ClsTypPnNoExistente = new CsTypPnNoExistente();
                ClsTypPnNoExistente.Alimentar(ObjPnNoExistente);
                string Mensj = ClsTypPnNoExistente.GetMensj();
                if (!Mensj.Equals(""))
                {
                    string Pn = ClsTypPnNoExistente.GetPN();
                    DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                    foreach (DataRow row in Result2)
                    { Mensj = row["Texto"].ToString().Trim(); }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + " [" + Pn + "]');", true);
                    return;
                }
                BindDetalle("UPDATE");
            }
        }
        protected void BtnReferencia_Click(object sender, EventArgs e)
        { Response.Redirect("~/Forms/InventariosCompras/FrmReferencia.aspx"); }
        protected void GrdDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}