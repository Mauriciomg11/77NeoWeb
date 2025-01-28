using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.MRO
{
    public partial class FrmCertificadosControlCalidad : System.Web.UI.Page
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
            Page.Title = string.Format("");
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
                ModSeguridad();
                MlVw.ActiveViewIndex = 0;
            }
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx", VbPC);
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0) { ViewState["VblIngMS"] = 0; }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; }
            if (ClsP.GetCE1() == 0)
            {
            }
            if (ClsP.GetCE2() == 0)
            {
            }
            if (ClsP.GetCE3() == 0)
            {
            }
            if (ClsP.GetCE4() == 0)
            {
            }
            if (ClsP.GetCE5() == 0)
            {
            }
            if (ClsP.GetCE6() == 0)
            {
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
                    if (bO.Equals("placeholder"))
                    { TxtBusqueda.Attributes.Add("placeholder", bT); }
                    IbtBusqueda.ToolTip = bO.Equals("IbtConsultar") ? bT : IbtBusqueda.ToolTip;
                    IbtCerrarBusq.ToolTip = bO.Equals("CerrarVentana") ? bT : IbtCerrarBusq.ToolTip;
                    LblBusqueda.Text = bO.Equals("MstrLblBusq") ? bT + ":" : LblBusqueda.Text;
                    LblTitOpcBusq.Text = bO.Equals("LblTitOTOpcBusqueda") ? bT : LblTitOpcBusq.Text;
                    RdbBusqNumOT.Text = bO.Equals("LblOTMstr") ? bT + ":" : RdbBusqNumOT.Text;
                    RdbBusqHK.Text = bO.Equals("LblAeronaveMstr") ? bT + ":" : RdbBusqHK.Text;
                    GrdBusq.EmptyDataText = bO.Equals("SinRegistros") ? bT : GrdBusq.EmptyDataText;
                    GrdBusq.Columns[1].HeaderText = bO.Equals("LblOTMstr") ? bT : GrdBusq.Columns[1].HeaderText;
                    GrdBusq.Columns[2].HeaderText = bO.Equals("GrdAplicab") ? bT : GrdBusq.Columns[2].HeaderText;
                    GrdBusq.Columns[5].HeaderText = bO.Equals("GrdCodHk") ? bT : GrdBusq.Columns[5].HeaderText;
                    GrdBusq.Columns[6].HeaderText = bO.Equals("LblAeronaveMstr") ? bT : GrdBusq.Columns[6].HeaderText;
                    GrdBusq.Columns[7].HeaderText = bO.Equals("GrdFecOT") ? bT : GrdBusq.Columns[7].HeaderText;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindData(string VbConsultar, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            //DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    //string VbTxtSql = " EXEC  SP_PANTALLA_PROPUESTA_V2 17,@OT,'','','','',0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    string VbTxtSql = " EXEC  SP_PANTALLA_PROPUESTA_V2 17,@OT,'','','','',0,0,@Idm,@ICC,'01-01-01','02-01-01','03-01-01'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@OT", VbConsultar);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "HKSinPPT";
                                DSTDdl.Tables[1].TableName = "HKConSubPT";

                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];

        }
        //*************************************** BUSQUEDA ***************************************
        protected void BIndDBusqOT()
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "", VbOpcion = "OT";

                if (RdbBusqNumOT.Checked == true)
                { VbOpcion = "OT"; }
                if (RdbBusqSN.Checked == true)
                { VbOpcion = "SN"; }
                if (RdbBusqPN.Checked == true)
                { VbOpcion = "PN"; }
                if (RdbBusqHK.Checked == true)
                { VbOpcion = "HK"; }
                VbTxtSql = "EXEC SP_PANTALLA_CetificacionesControlCalidad 10,@Prmtr,'','',@Opc,0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";

                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Prmtr", TxtBusqueda.Text.Trim()); ;// VbOpcion.Equals("OT") ? TxtOt.Text : TxtOTBusq.Text.Trim()
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0) { GrdBusq.DataSource = DtB; GrdBusq.DataBind(); }
                        else { GrdBusq.DataSource = null; GrdBusq.DataBind(); }
                    }
                }
            }
        }
        protected void BtnConsultar_Click1(object sender, EventArgs e)
        { MlVw.ActiveViewIndex = 1; Page.Title = ViewState["PageTit"].ToString().Trim(); TxtBusqueda.Text = ""; }
        protected void IbtCerrarBusq_Click(object sender, ImageClickEventArgs e)
        { MlVw.ActiveViewIndex = 0; Page.Title = ViewState["PageTit"].ToString().Trim(); }
        protected void IbtBusqueda_Click(object sender, ImageClickEventArgs e)
        { BIndDBusqOT(); }
        protected void GrdBusq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ir"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int rowIndex = row.RowIndex;
                string vbcod = ((Label)row.FindControl("LblOT")).Text.ToString().Trim();
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                TxtNumOT.Text = vbcod;
                ViewState["CodHK"] = ((Label)row.FindControl("LblCodHk")).Text.ToString().Trim();
                TxtMatr.Text = ((Label)row.FindControl("LblHk")).Text.ToString().Trim();
                TxtSnHK.Text = GrdBusq.DataKeys[gvr.RowIndex].Values["SNHK"].ToString();
                TxtModelo.Text = GrdBusq.DataKeys[gvr.RowIndex].Values["NomModelo"].ToString();
                TxtPnElem.Text = ((Label)row.FindControl("LblPnElem")).Text.ToString().Trim();
                TxtSnElem.Text = ((Label)row.FindControl("LblSnElem")).Text.ToString().Trim();
                TxtDescElem.Text = GrdBusq.DataKeys[gvr.RowIndex].Values["DescrElem"].ToString();

                /*DdlTipo.Text = VbCodTipo;
               BindDataDdlPpal("SELECT", "0");
               DdlPptSuper.Text = "";
               BindDataDdlPptPpal(VbCodCli, "SELECT");
               ActivarGrd(VbCodTipo);
               Traerdatos(vbcod);
               PerfilesGrid();
               IbtAprDet1All.Visible = false; IbtDesAprDet1All.Visible = false;
               if ((int)ViewState["VblIngMS"] == 1 && TxtFechAprob.Text.Equals("") && VbCodTipo.Trim().Equals("00001")) { IbtAprDet1All.Visible = true; IbtDesAprDet1All.Visible = true; }
               ViewState["IdDetPropHk"] = "0";
               ViewState["IdDetPropSrv"] = "0";
               ViewState["RegistroElemHK"] = "";
               ViewState["FilterPnSugerido"] = "N";
               ViewState["FilterElem"] = "N";
               BindServicios("UPDATE", "0");*/
                MlVw.ActiveViewIndex = 0;
                Page.Title = ViewState["PageTit"].ToString().Trim();
            }
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow[] Result = Idioma.Select("Objeto='IbtIrMstr'");
                ImageButton IbtIr = (e.Row.FindControl("IbtIr") as ImageButton);
                if (IbtIr != null)
                {
                    foreach (DataRow RowIdioma in Result)
                    { IbtIr.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }
            }
        }
    }
}