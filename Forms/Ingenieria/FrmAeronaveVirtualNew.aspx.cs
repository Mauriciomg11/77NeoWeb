using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmAeronaveVirtualNew : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            /* if (Session["Login77"] == null)
             {
                 Response.Redirect("~/FrmAcceso.aspx");
             } */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo    
            Page.Title = string.Format("Aeronave Virtual");
            if (Session["C77U"] == null)
            {
                /*Session["C77U"] = "";*/
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";
                ViewState["Validar"] = "S";
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Aeronave Virtual";
                ViewState["PNSN"] = "PN";
                ViewState["CodElemento"] = "";
                ViewState["Ventana"] = 0;
                ViewState["CodModelo"] = "";
                ViewState["ValidaFechaSvc"] = "N";
                ViewState["TieneCompensacion"] = "N";
                CalFechaInsElem.EndDate = DateTime.Now;
                CalFechaRemElem.EndDate = DateTime.Now;
                MultVw.ActiveViewIndex = 0;
                BindDDdl();
                BtnInsElem.CssClass = "btn btn-primary";
                /* 
                 CldFecDet.EndDate = DateTime.Now;
                 CldFecCump.EndDate = DateTime.Now;
                 CldFecPry.EndDate = DateTime.Now.AddDays(120);  
                 MultVw.ActiveViewIndex = 0;
                 ModSeguridad();
                 
                 BindDdlRte();
                 BindDMotor("", -1);*/
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void BindDDdl()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 22,'','','','INSHK',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlAeroInsElem.DataSource = Cnx.DSET(LtxtSql);
            DdlAeroInsElem.DataMember = "Datos";
            DdlAeroInsElem.DataTextField = "Matricula";
            DdlAeroInsElem.DataValueField = "CodAeronave";
            DdlAeroInsElem.DataBind();

            DdlHkConsAeroVirtual.DataSource = Cnx.DSET(LtxtSql);
            DdlHkConsAeroVirtual.DataMember = "Datos";
            DdlHkConsAeroVirtual.DataTextField = "Matricula";
            DdlHkConsAeroVirtual.DataValueField = "CodAeronave";
            DdlHkConsAeroVirtual.DataBind();

            DdlAeroRemElem.DataSource = Cnx.DSET(LtxtSql);
            DdlAeroRemElem.DataMember = "Datos";
            DdlAeroRemElem.DataTextField = "Matricula";
            DdlAeroRemElem.DataValueField = "CodAeronave";
            DdlAeroRemElem.DataBind();

            DdlAeroInsMay.DataSource = Cnx.DSET(LtxtSql);
            DdlAeroInsMay.DataMember = "Datos";
            DdlAeroInsMay.DataTextField = "Matricula";
            DdlAeroInsMay.DataValueField = "CodAeronave";
            DdlAeroInsMay.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Reporte_Manto2 1,'','','','','PosR',0,0,0,0,'01-01-1','02-01-1','03-01-1'");
            DdlPosicRemElem.DataSource = Cnx.DSET(LtxtSql);
            DdlPosicRemElem.DataMember = "Datos";
            DdlPosicRemElem.DataTextField = "Descripcion";
            DdlPosicRemElem.DataValueField = "Codigo";
            DdlPosicRemElem.DataBind();

        }
        protected void BtnInsElem_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsEle");
            BtnInsElem.CssClass = "btn btn-primary";
            MultVw.ActiveViewIndex = 0;
        }
        protected void BtnRemElem_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemEle");
            MultVw.ActiveViewIndex = 3;
            BtnRemElem.CssClass = "btn btn-primary";
        }
        protected void BtnInsMayor_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsMay");
            MultVw.ActiveViewIndex = 5;
            BtnInsMayor.CssClass = "btn btn-primary";
        }
        protected void BtnRemMayor_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemMay");
            // MultVw.ActiveViewIndex = 3;
            BtnRemMayor.CssClass = "btn btn-primary";
        }
        protected void BtnInsSubC_Click(object sender, EventArgs e)
        {
            LimparCampoHK("InsSub");
            // MultVw.ActiveViewIndex = 3;
            BtnInsSubC.CssClass = "btn btn-primary";
        }
        protected void BtnRemSubC_Click(object sender, EventArgs e)
        {
            LimparCampoHK("RemSub");
            // MultVw.ActiveViewIndex = 3;
            BtnRemSubC.CssClass = "btn btn-primary";
        }
        protected void BtnCrearElem_Click(object sender, EventArgs e)
        {
            LimparCampoHK("");
        }
        protected void AplicarCssClassBtn()
        {
            BtnInsElem.CssClass = "btn btn-outline-primary";
            BtnRemElem.CssClass = "btn btn-outline-primary";
            BtnInsMayor.CssClass = "btn btn-outline-primary";
            BtnRemMayor.CssClass = "btn btn-outline-primary";
            BtnInsSubC.CssClass = "btn btn-outline-primary";
            BtnRemSubC.CssClass = "btn btn-outline-primary";
        }
        protected void LimparCampoHK(string TipoMov)
        {
            AplicarCssClassBtn();
            ViewState["TieneCompensacion"] = "N";
            switch (TipoMov.Trim())
            {
                case "InsEle":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    break;
                case "RemEle":
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();

                    break;
                case "InsMay":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    break;
                case "RemMay":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    break;
                case "InsSub":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    break;
                case "RemSub":
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    break;
                default:
                    DdlAeroRemElem.Text = "0";
                    BtnRemCompensac.Visible = false;
                    DdlAeroInsElem.Text = "0";
                    BtnCompensac.Visible = false;
                    GrdSvcInsElem.DataSource = null;
                    GrdSvcInsElem.DataBind();
                    DdlAeroInsMay.Text = "0";
                    BtnCompensacInsMay.Visible = false;
                    GrdSvcInsMay.DataSource = null;
                    GrdSvcInsMay.DataBind();
                    break;
            }
        }

        //******************************************  INSTALAR COMPONENTE *********************************************************

        protected void BIndDataBusq()
        {
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            GrdSvcInsElem.Visible = false;
            GrdBusq.Visible = true;
            TxtTitServicios.Text = "Elementos disponibles";
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "", VbOpcion = "";
                VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 23,@SN,@PN,@UN,'',@CodHK,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {

                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroInsElem.Text);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusq.DataSource = DtB;
                            GrdBusq.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusq.DataSource = null;
                            GrdBusq.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisElemInsElem(string CodElem)
        {
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            TxtTitContadores.Text = "S/N: " + TxtSnInsElem.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContInsElem.DataSource = DtB;
                            GrdHisContInsElem.DataBind();
                        }
                        else
                        {
                            GrdHisContInsElem.DataSource = null;
                            GrdHisContInsElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDSvcInsElem(string CodElem, string Modelo, string CodHK)
        {
            if (DdlAeroInsElem.Text.Equals("0"))
            { return; }
            TxtTitServicios.Text = "Servicios asignados";
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 4,'NORMAL',@CoEl,@Mo,@CHk,2,3,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@CoEl", CodElem.Trim());
                        SC.Parameters.AddWithValue("@Mo", Modelo.Trim());
                        SC.Parameters.AddWithValue("@CHk", CodHK.Trim());
                        using (SqlDataAdapter DAB = new SqlDataAdapter())
                        {
                            DAB.SelectCommand = SC;
                            DAB.Fill(DtB);

                            if (DtB.Rows.Count > 0)
                            {
                                GrdSvcInsElem.DataSource = DtB;
                                GrdSvcInsElem.DataBind();
                            }
                            else
                            {
                                GrdSvcInsElem.DataSource = null;
                                GrdSvcInsElem.DataBind();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string borrar = Ex.ToString();
                    }
                }
            }
        }
        protected void LimpiarCamposInsElem(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroInsElem.Text = "0"; }
            TxtPnInsElem.Text = "";
            TxtSnInsElem.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecInsElem.Text = "";
            DdlPosicInsElem.Text = "";
            TxtFechaInsElem.Text = "";
            TxtMotivInsElem.Text = "";
        }
        protected void BtnPNInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDataBusq();
        }
        protected void BtnSNInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDataBusq();
        }
        protected void BtnUltNivInsElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDataBusq();
        }
        protected void DdlAeroInsElem_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsElem("");
            BIndDataBusq();
        }
        protected void TxtFecUltCumpl_TextChanged(object sender, EventArgs e)
        {
            ViewState["ValidaFechaSvc"] = "N";
        }
        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsElem("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[11].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[7].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[8].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]')", true);
                return;
            }
            TxtPnInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[1].Text.Trim());
            TxtSnInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusq.DataKeys[this.GrdBusq.SelectedIndex][0].ToString();
            TxtUbiTecInsElem.Text = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[5].Text.Trim());
            string PoscElem = HttpUtility.HtmlDecode(GrdBusq.SelectedRow.Cells[10].Text.Trim());
            BIndDHisElemInsElem(ViewState["CodElemento"].ToString().Trim());
            if (PoscElem.Equals("S"))
            {
                DdlPosicInsElem.Enabled = true;
                string LtxtSql = string.Format("EXEC Consultas_General_Ingenieria 2,'{0}','{1}','',0, 0,0,'01-01-1','01-01-1'", TxtUbiTecInsElem.Text, ViewState["CodModelo"].ToString().Trim());
                DdlPosicInsElem.DataSource = Cnx.DSET(LtxtSql);
                DdlPosicInsElem.DataMember = "Datos";
                DdlPosicInsElem.DataTextField = "Descripcion";
                DdlPosicInsElem.DataValueField = "Codigo";
                DdlPosicInsElem.DataBind();
            }
            else
            { DdlPosicInsElem.Enabled = false; }
            GrdBusq.Visible = false;
            BIndDSvcInsElem(ViewState["CodElemento"].ToString().Trim(), ViewState["CodModelo"].ToString().Trim(), DdlAeroInsElem.Text);
            GrdSvcInsElem.Visible = true;
        }
        protected void GrdBusq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[12].Visible = false;
            }

        }
        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdBusq.PageIndex = e.NewPageIndex;
            BIndDataBusq();
        }
        protected void TxtFechaInsElem_TextChanged(object sender, EventArgs e)
        {
            if (!DdlAeroInsElem.Text.Equals("0") && !TxtSnInsElem.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,@UBR,'','','',@CodA,0,0,0,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroInsElem.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaInsElem.Text);
                    SC.Parameters.AddWithValue("@UBR", TxtUbiTecInsElem.Text);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnCompensac.Visible = true; }
                        else
                        { BtnCompensac.Visible = false; }
                    }
                }
            }
        }
        protected void BtnGuardarInsElem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlAeroInsElem.Text.Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una aeronave')", true);
                    return;
                }
                if (TxtPnInsElem.Text.Equals("") || TxtSnInsElem.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un elemento')", true);
                    return;
                }
                if (TxtUbiTecInsElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicInsElem.Text.Equals("") && DdlPosicInsElem.Enabled == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaInsElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una fecha')", true);
                    return;
                }
                if (TxtMotivInsElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un motivo')", true);
                    return;
                }
                foreach (GridViewRow Row in GrdSvcInsElem.Rows)
                {

                    string VbFechaAnt = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim(); // obtener indice
                    string TxtFecUltCumpl = (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim();
                    string VbReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim();
                    if (!TxtFecUltCumpl.Equals(VbFechaAnt) && VbReporte.Equals("") && ViewState["ValidaFechaSvc"].Equals("N"))
                    {
                        ViewState["ValidaFechaSvc"] = "S";
                        ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Existen servicios en los que se modificaron fechas de cumplimiento y no tienen reporte')", true);
                        return;
                    }
                }

                List<ClsTypAeronaveVirtual> ObjInsElemento = new List<ClsTypAeronaveVirtual>();
                var TypInsElemento = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "IC",
                    CodAeronave = Convert.ToInt32(DdlAeroInsElem.Text.Trim()),
                    NivelElemento = "C",
                    UltimoNivel = TxtUbiTecInsElem.Text.Trim(),
                    CodMayor = "",
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnInsElem.Text.Trim(),
                    Sn = TxtSnInsElem.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaInsElem.Text),
                    Posicion = DdlPosicInsElem.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivInsElem.Text.Trim(),
                };
                ObjInsElemento.Add(TypInsElemento);

                List<ClsTypAeronaveVirtual> ObjServcManto = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdSvcInsElem.Rows)
                {
                    string StrUC;
                    double VbUC;
                    CultureInfo Culture = new CultureInfo("en-US");
                    StrUC = (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim().Equals("") ? "0" : (Row.FindControl("TxtCumpHist") as TextBox).Text.Trim();
                    VbUC = StrUC.Length == 0 ? 0 : Convert.ToDouble(StrUC, Culture);

                    DateTime? VbFechaVence, VbFechaVenceAnt;
                    string VbFecha = (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim().Equals("") ? null : (Row.FindControl("TxtFecUltCumpl") as TextBox).Text.Trim();
                    if (VbFecha == null)
                    { VbFechaVence = null; }
                    else
                    { VbFechaVence = Convert.ToDateTime(VbFecha); }

                    VbFecha = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim().Equals("") ? null : GrdSvcInsElem.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    if (VbFecha == null)
                    { VbFechaVenceAnt = null; }
                    else
                    { VbFechaVenceAnt = Convert.ToDateTime(VbFecha); }
                    string borr = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim();
                    int borrar = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    var TypServcManto = new ClsTypAeronaveVirtual()
                    {
                        CodIdContadorElem = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[1].ToString().Trim()),
                        CodElementoSvc = GrdSvcInsElem.DataKeys[Row.RowIndex].Values[3].ToString().Trim(),
                        FechaVence = VbFechaVence,
                        FechaVenceAnt = VbFechaVenceAnt,
                        Resetear = (Row.FindControl("CkbReset") as CheckBox).Checked == true ? 1 : 0,
                        CodOT = (Row.FindControl("LblCodOT") as Label).Text.Trim().Equals("") ? 0 : Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        CodIdContaSrvManto = Convert.ToInt32(GrdSvcInsElem.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        NumReporte = (Row.FindControl("TxtReporte") as TextBox).Text.Trim(),
                        ValorUltCump = Convert.ToDouble(VbUC),
                        GeneraHist = (Row.FindControl("CkbGenerarHist") as CheckBox).Checked == true ? "S" : "N",
                    };
                    ObjServcManto.Add(TypServcManto);
                }

                List<ClsTypAeronaveVirtual> ObjCompensacion = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensac = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "N", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicInsElem.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacion.Add(TypCompensac);
                }
                List<ClsTypAeronaveVirtual> ObjOT = new List<ClsTypAeronaveVirtual>();
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjInsElemento, ObjServcManto, ObjCompensacion, ObjOT);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
                BIndDHisElemInsElem(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposInsElem("TODOS");
                BIndDSvcInsElem("", "", "0");
                ViewState["TieneCompensacion"] = "N";
                GrdSvcInsElem.Visible = false;
                BtnCompensac.Visible = false;
                // ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('" + AeronaveVirtual.GetBorrar() + "')", true);
                ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Instalar Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroInsElem.Text = "0";
            }
        }
        protected void GrdSvcInsElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CalendarExtender CalFecUltCumpl = e.Row.FindControl("CalFecUltCumpl") as CalendarExtender;
                CalFecUltCumpl.EndDate = DateTime.Now;

                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbContEsReset = dr["Reseteable"].ToString();
                string VbSvcEsReset = dr["Reseteable"].ToString();
                string VbTieneOT = dr["CodOT"].ToString();
                string TieneHisLV = dr["TieneHisLV"].ToString();
                if (VbContEsReset.Equals("N") || VbSvcEsReset.Equals("0") || !VbTieneOT.Equals(""))
                {
                    CheckBox CkbReset = e.Row.FindControl("CkbReset") as CheckBox;
                    CkbReset.Enabled = false;
                }

                Label LblContador = e.Row.FindControl("LblContador") as Label;
                TextBox TxtCumpHist = e.Row.FindControl("TxtCumpHist") as TextBox;
                CheckBox CkbGenerarHist = e.Row.FindControl("CkbGenerarHist") as CheckBox;
                if (LblContador.Text.Trim().Equals("CAL"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    TxtCumpHist.ToolTip = "El Contador CAL no genera histórico.";
                }
                if (TieneHisLV.Equals("S"))
                {
                    TxtCumpHist.Enabled = false;
                    CkbGenerarHist.Enabled = false;
                    TxtCumpHist.ToolTip = "tiene hojas procesadas en el histórico de contadores.";
                }
            }
        }

        //******************************************  Aeronave virtual Consultar SN instaladas *********************************************************
        protected void BtnAKVirtualInsElem_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroInsElem.Text;
            BIndDConsAeroVirtual(DdlAeroInsElem.Text);
        }
        protected void BIndDConsAeroVirtual(string CodHK)
        {
            if (CodHK.Equals("0"))
            { return; }
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_Visualizar_Aeronave_Virtual @CodHK,'','',0,'AERONAVE_VIRTUAL'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodHK", CodHK);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdListaAeroVirtual.DataSource = DtB;
                            GrdListaAeroVirtual.DataBind();
                        }
                        else
                        {
                            GrdListaAeroVirtual.DataSource = null;
                            GrdListaAeroVirtual.DataBind();
                        }
                    }
                }
            }
        }
        protected void DdlHkConsAeroVirtual_TextChanged(object sender, EventArgs e)
        {
            BIndDConsAeroVirtual(DdlHkConsAeroVirtual.Text);
        }
        protected void IbtCerrarAeroVirtual_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
        }
        protected void GrdListaAeroVirtual_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;

                DataRowView dr = e.Row.DataItem as DataRowView;
                string VbMayor = dr["Mayor"].ToString();
                switch (VbMayor)
                {
                    case "1":
                        e.Row.BackColor = System.Drawing.Color.White;
                        break;
                    case "2":
                        e.Row.BackColor = System.Drawing.Color.Yellow;
                        break;
                    case "3":
                        e.Row.BackColor = System.Drawing.Color.Silver;
                        break;
                    default:
                        e.Row.BackColor = System.Drawing.Color.Red;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        break;
                }
            }
        }

        //******************************************  Compensaciones *********************************************************

        protected void BtnCompensac_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroInsElem.Text, TxtFechaInsElem.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
        }
        protected void BIndDCompesacLV(string CodHK, string Fecha)

        {
            if (CodHK.Equals("0"))
            { return; }
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 25,'','','','',@CodA,0,0,0,@F,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodA", CodHK);
                    SC.Parameters.AddWithValue("@F", Fecha);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdCompensLv.DataSource = DtB;
                            GrdCompensLv.DataBind();
                        }
                        else
                        {
                            GrdCompensLv.DataSource = null;
                            GrdCompensLv.DataBind();
                        }
                    }
                }
            }
        }
        protected void IbtCerrarCompensacion_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
        }
        protected void GrdCompensLv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {


                DataRowView dr = e.Row.DataItem as DataRowView;
                /*string VbMayor = dr["Mayor"].ToString();
                switch (VbMayor)
                {
                    case "1":
                        e.Row.BackColor = System.Drawing.Color.White;
                        break;
                    case "2":
                        e.Row.BackColor = System.Drawing.Color.Yellow;
                        break;
                    case "3":
                        e.Row.BackColor = System.Drawing.Color.Silver;
                        break;
                    default:
                        e.Row.BackColor = System.Drawing.Color.Red;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        break;
                }*/
            }
        }
        protected void GrdCompensLv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                CheckBox CkbOK = row.FindControl("CkbOK") as CheckBox;
                CkbOK.Checked = true;
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
                ViewState["TieneCompensacion"] = "S";
            }
        }
        protected void BtnCompensReinicio_Click(object sender, EventArgs e)
        {
            ViewState["TieneCompensacion"] = "N";
            BIndDCompesacLV(DdlAeroInsElem.Text, TxtFechaInsElem.Text);
            GrdCompensLv.Enabled = true;
            CkbCompensInicioDia.Checked = false;
            CkbCompensInicioDia.Enabled = true;
        }
        protected void CkbCompensInicioDia_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.UplCompensacion, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Todos los valores del dia seran tomados por el componente instalado')", true);
            GrdCompensLv.Enabled = false;
            CkbCompensInicioDia.Enabled = false;
            ViewState["TieneCompensacion"] = "S";
        }

        //******************************************  REMOCION COMPONENTE *********************************************************

        protected void BIndDRemBusqElem()
        {
            if (DdlAeroRemElem.Text.Equals("0"))
            { return; }
            GrdRemBusqElem.Visible = true;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "", VbOpcion = "";
                VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 26,@SN,@PN,@UN,'',@CodHK,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtRemBusqueda.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroRemElem.Text);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdRemBusqElem.DataSource = DtB;
                            GrdRemBusqElem.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdRemBusqElem.DataSource = null;
                            GrdRemBusqElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisContRemElem(string CodElem)
        {
            if (DdlAeroRemElem.Text.Equals("0"))
            { return; }
            TxtTitRemContadores.Text = "S/N: " + TxtSnRemElem.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContRemElem.DataSource = DtB;
                            GrdHisContRemElem.DataBind();
                        }
                        else
                        {
                            GrdHisContRemElem.DataSource = null;
                            GrdHisContRemElem.DataBind();
                        }
                    }
                }
            }
        }
        protected void LimpiarCamposRemElem(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroRemElem.Text = "0"; }
            TxtPnRemElem.Text = "";
            TxtSnRemElem.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecRemElem.Text = "";
            DdlPosicRemElem.Text = "";
            TxtFechaRemElem.Text = "";
            TxtMotivRemElem.Text = "";
        }
        protected void DdlAeroRemElem_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposRemElem("");
            BIndDRemBusqElem();
        }
        protected void BtnPNRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDRemBusqElem();
        }
        protected void BtnSNRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDRemBusqElem();
        }
        protected void BtnUltNivRemElem_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDRemBusqElem();
        }
        protected void BtnAKVirtualRemElem_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroRemElem.Text;
            BIndDConsAeroVirtual(DdlAeroRemElem.Text);
        }
        protected void BtnGuardarRemElem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlAeroRemElem.Text.Equals("0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una aeronave')", true);
                    return;
                }
                if (TxtPnRemElem.Text.Equals("") || TxtSnRemElem.Text.Equals("") || ViewState["CodElemento"].Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un elemento')", true);
                    return;
                }
                if (TxtUbiTecRemElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una ubicación técnica')", true);
                    return;
                }
                if (DdlPosicRemElem.Text.Equals("") && DdlPosicInsElem.Enabled == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una posición')", true);
                    return;
                }
                if (TxtFechaRemElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar una fecha')", true);
                    return;
                }
                if (TxtMotivRemElem.Text.Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un motivo')", true);
                    return;
                }

                List<ClsTypAeronaveVirtual> ObjRemElemento = new List<ClsTypAeronaveVirtual>();
                var TypRemElemento = new ClsTypAeronaveVirtual()
                {
                    TipoEvento = "RC",
                    CodAeronave = Convert.ToInt32(DdlAeroRemElem.Text.Trim()),
                    NivelElemento = "C",
                    UltimoNivel = TxtUbiTecRemElem.Text.Trim(),
                    CodMayor = "",
                    CodElemento = ViewState["CodElemento"].ToString().Trim(),
                    Pn = TxtPnRemElem.Text.Trim(),
                    Sn = TxtSnRemElem.Text.Trim(),
                    FechaEvento = Convert.ToDateTime(TxtFechaRemElem.Text),
                    Posicion = DdlPosicRemElem.Text.Trim(),
                    Usu = Session["C77U"].ToString(),
                    MotivoRemocion = TxtMotivRemElem.Text.Trim(),
                };
                ObjRemElemento.Add(TypRemElemento);

                List<ClsTypAeronaveVirtual> ObjServcManto = new List<ClsTypAeronaveVirtual>();

                List<ClsTypAeronaveVirtual> ObjCompensacion = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdCompensLv.Rows)
                {
                    int VbidC = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim());
                    DateTime VbFechaLV = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[1].ToString().Trim());
                    DateTime VbFechaDespeg = Convert.ToDateTime(GrdCompensLv.DataKeys[Row.RowIndex].Values[2].ToString().Trim());
                    string StrHAcum, StrCAcum, StrHRemain, StrCRemain;
                    double VbHAcum, VbCAcum, VbHRemain, VbCRemain;

                    CultureInfo Culture = new CultureInfo("en-US");
                    StrHAcum = (Row.FindControl("HoraAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraAcum") as Label).Text.Trim();
                    VbHAcum = StrHAcum.Length == 0 ? 0 : Convert.ToDouble(StrHAcum, Culture);

                    StrCAcum = (Row.FindControl("CicloAcum") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloAcum") as Label).Text.Trim();
                    VbCAcum = StrCAcum.Length == 0 ? 0 : Convert.ToDouble(StrCAcum, Culture);
                    Boolean borrarb = (Row.FindControl("CkbOK") as CheckBox).Checked;
                    int borrar = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0;
                    StrHRemain = (Row.FindControl("HoraRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("HoraRemain") as Label).Text.Trim();
                    VbHRemain = StrHRemain.Length == 0 ? 0 : Convert.ToDouble(StrHRemain, Culture);

                    StrCRemain = (Row.FindControl("CicloRemain") as Label).Text.Trim().Equals("") ? "0" : (Row.FindControl("CicloRemain") as Label).Text.Trim();
                    VbCRemain = StrCRemain.Length == 0 ? 0 : Convert.ToDouble(StrCRemain, Culture);

                    var TypCompensac = new ClsTypAeronaveVirtual()
                    {

                        ID = Convert.ToInt32(GrdCompensLv.DataKeys[Row.RowIndex].Values[0].ToString().Trim()),
                        OK = (Row.FindControl("CkbOK") as CheckBox).Checked == true ? 1 : 0,
                        CodlibroVuelo = (Row.FindControl("LblCodLV") as Label).Text.Trim(),
                        FechaLibroVuelo = VbFechaLV,
                        HoraDespegue = VbFechaDespeg,
                        CompensInicioDia = CkbCompensInicioDia.Checked == true ? 1 : 0,
                        HorasAcum = VbHAcum,
                        CiclosAcum = VbCAcum,
                        HorasRemain = VbHRemain,
                        CiclosRemain = VbCRemain,
                        TipoComponente = "N", //M=mayor, N= componenente, S=Subcomp
                        PosicionCE = DdlPosicRemElem.Text,
                        Compensacion = ViewState["TieneCompensacion"].ToString(),
                    };
                    ObjCompensacion.Add(TypCompensac);
                }

                List<ClsTypAeronaveVirtual> ObjOT = new List<ClsTypAeronaveVirtual>();
                foreach (GridViewRow Row in GrdOtCerrar.Rows)
                {
                    DateTime? VbFechaI;
                    string VbCcosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim();
                    string VbFIText = GrdOtCerrar.DataKeys[Row.RowIndex].Values[1].ToString().Trim();
                    if (VbFIText.Equals("")) { VbFechaI = Convert.ToDateTime(TxtFechaRemElem.Text); }
                    else { VbFechaI = Convert.ToDateTime(VbFIText); }

                    var TypOT = new ClsTypAeronaveVirtual()
                    {
                        CodNumOrdenTrab = Convert.ToInt32((Row.FindControl("LblCodOT") as Label).Text.Trim()),
                        Descripcion = "",
                        CodEstOrdTrab1 = "0002",
                        CodEstOrdTrab2 = "",
                        Aplicabilidad = TxtSnRemElem.Text.Trim(),
                        CodCapitulo = "",
                        CodUbicaTecn = "",
                        CodBase = "",
                        CodTaller = "",
                        CodPlanManto = "",
                        CentroCosto = GrdOtCerrar.DataKeys[Row.RowIndex].Values[0].ToString().Trim(),
                        FechaInicio = VbFechaI,
                        FechaFinal = Convert.ToDateTime(TxtFechaRemElem.Text),
                        FechaReg = Convert.ToDateTime((Row.FindControl("LblFechaReg") as Label).Text.Trim()),
                        IdentificadorCorrPrev = 1,
                        CodPrioridad = "",
                        CodIdLvDetManto = 0,
                        CodIdDetSrvManto = 0,
                        BanCerrado = 1,
                        HorasProyectadas = 0,
                        FechaProyectada = null,
                        FechaVencimiento = null,
                        UsuOT = Session["C77U"].ToString(),
                        Referencia = "",
                        AccionParcial = (Row.FindControl("TxtMotivo") as TextBox).Text.Trim(),
                        CodTipoCodigo = "",
                        CodInspectorCierre = "",
                        LicenciaInspCierre = "",
                        PNOT = "",
                        BloquearDetalle = 0,
                        CodResponsable = Session["C77U"].ToString(),
                        OTSN = 0,
                        OTSO = 0,
                        OTSR = 0,
                        OCSN = 0,
                        OCSO = 0,
                        OCSR = 0,
                        EjecPasos = Convert.ToInt32(GrdOtCerrar.DataKeys[Row.RowIndex].Values[2].ToString().Trim()),
                        CancelOT = 0,
                        WS = "",
                        OKOT = (Row.FindControl("CkbOk") as CheckBox).Checked == true ? 1 : 0,
                        AccionOT = "",
                    };
                    ObjOT.Add(TypOT);
                }
                ClsTypAeronaveVirtual AeronaveVirtual = new ClsTypAeronaveVirtual();
                AeronaveVirtual.Alimentar(ObjRemElemento, ObjServcManto, ObjCompensacion, ObjOT);
                string Mensj = AeronaveVirtual.GetMensj();
                if (!Mensj.Trim().Equals(""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                    return;
                }
                BIndDHisContRemElem(ViewState["CodElemento"].ToString().Trim());
                LimpiarCamposRemElem("TODOS");
                ViewState["TieneCompensacion"] = "N";
                BtnAbrirOTCerrar.Visible = false;
                BtnRemCompensac.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('" + AeronaveVirtual.GetBorrar() + "')", true);
                ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('Proceso exitoso')", true);
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInstElem, UplInstElem.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente en la instalación')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Remover Componente", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                DdlAeroRemElem.Text = "0";
            }
        }
        protected void TxtFechaRemElem_TextChanged(object sender, EventArgs e)
        {
            if (!DdlAeroRemElem.Text.Equals("0") && !TxtSnRemElem.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,'','','','',@CodA,0,0,0,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroRemElem.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaRemElem.Text);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnRemCompensac.Visible = true; }
                        else
                        { BtnRemCompensac.Visible = false; }
                    }
                }
            }
        }
        protected void BtnRemCompensac_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroRemElem.Text, TxtFechaRemElem.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
        }
        protected void GrdRemBusqElem_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarCamposRemElem("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[12].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[8].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[9].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]')", true);
                return;
            }
            TxtPnRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[1].Text.Trim());
            TxtSnRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdRemBusqElem.DataKeys[this.GrdRemBusqElem.SelectedIndex][0].ToString();
            TxtUbiTecRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[5].Text.Trim());
            DdlPosicRemElem.Text = HttpUtility.HtmlDecode(GrdRemBusqElem.SelectedRow.Cells[7].Text.Trim());
            BIndDHisContRemElem(ViewState["CodElemento"].ToString().Trim());
            BIndDOCerrarOT(ViewState["CodElemento"].ToString().Trim());
            GrdRemBusqElem.Visible = false;
        }
        protected void GrdRemBusqElem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdRemBusqElem.PageIndex = e.NewPageIndex;
            BIndDRemBusqElem();
        }
        protected void GrdRemBusqElem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) // Cabecera
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)  // registros
            {
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
            }
        }

        //******************************************  OT para cerrar *********************************************************
        protected void BIndDOCerrarOT(string CodElem)
        {
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 21,@CE,'','','',0,0,0,0,'01-01-01','01-01-01','01-01-01'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CE", CodElem);

                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdOtCerrar.DataSource = DtB;
                            GrdOtCerrar.DataBind();
                            BtnAbrirOTCerrar.Visible = true;
                            ScriptManager.RegisterClientScriptBlock(this.UplRemElem, UplRemElem.GetType(), "IdntificadorBloqueScript", "alert('El componente tiene ordenes de trabajo abierta(s)')", true);
                        }
                        else
                        {
                            GrdOtCerrar.DataSource = null;
                            GrdOtCerrar.DataBind();
                            BtnAbrirOTCerrar.Visible = false;
                        }
                    }
                }
            }
        }
        protected void BtnAbrirOTCerrar_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 4;
        }
        protected void IbtCerrarOTcierre_Click(object sender, ImageClickEventArgs e)
        {
            MultVw.ActiveViewIndex = (int)ViewState["Ventana"];
        }

        //******************************************  INSTALAR MAYOR *********************************************************
        protected void BIndDBusqInsMay()
        {
            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            GrdSvcInsMay.Visible = false;
            GrdBusqMayDisp.Visible = true;
            TxtTitSvcInsMay.Text = "Mayores Disponibles";
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "", VbOpcion = "";
                VbTxtSql = string.Format("EXEC SP_PANTALLA_AeronaveVirtual 23,@SN,@PN,@UN,'M',@CodHK,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {

                    SC.Parameters.AddWithValue("@SN", ViewState["PNSN"].Equals("SN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@PN", ViewState["PNSN"].Equals("PN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@UN", ViewState["PNSN"].Equals("UN") ? TxtBusqInsMay.Text.Trim() : "");
                    SC.Parameters.AddWithValue("@Opc", VbOpcion.Trim());
                    SC.Parameters.AddWithValue("@CodHK", DdlAeroInsMay.Text);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdBusqMayDisp.DataSource = DtB;
                            GrdBusqMayDisp.DataBind();
                            ViewState["ValidaFechaSvc"] = "S";
                        }
                        else
                        {
                            GrdBusqMayDisp.DataSource = null;
                            GrdBusqMayDisp.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDHisElemInsMay(string CodElem)
        {
            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            TxtTitHisContInsMay.Text = "S/N: " + TxtSnInsMay.Text;
            DataTable DtB = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format(" EXEC SP_PANTALLA_AeronaveVirtual 1,@CodElem,'','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@CodElem", CodElem.Trim());
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DtB);

                        if (DtB.Rows.Count > 0)
                        {
                            GrdHisContInsMay.DataSource = DtB;
                            GrdHisContInsMay.DataBind();
                        }
                        else
                        {
                            GrdHisContInsMay.DataSource = null;
                            GrdHisContInsMay.DataBind();
                        }
                    }
                }
            }
        }
        protected void BIndDSvcInsMay(string CodElem, string Modelo, string CodHK)
        {
            if (DdlAeroInsMay.Text.Equals("0"))
            { return; }
            TxtTitSvcInsMay.Text = "Servicios Asignados";
            DataTable DTM = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "";
                VbTxtSql = string.Format("EXEC Consultas_General_Ingenieria 4,'MAYOR',@CoEl,@Mo,@CHk,2,3,'01-01-1900','01-01-1900'");
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    try
                    {
                        SC.Parameters.AddWithValue("@CoEl", CodElem.Trim());
                        SC.Parameters.AddWithValue("@Mo", Modelo.Trim());
                        SC.Parameters.AddWithValue("@CHk", CodHK.Trim());
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            SDA.SelectCommand = SC;
                            SDA.Fill(DTM);

                            if (DTM.Rows.Count > 0)
                            {
                                GrdSvcInsMay.DataSource = DTM;
                                GrdSvcInsMay.DataBind();
                            }
                            else
                            {
                               GrdSvcInsMay.DataSource = null;
                               GrdSvcInsMay.DataBind();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string borrar = Ex.ToString();
                    }
                }
            }
        }
        protected void LimpiarCamposInsMay(string Campos)
        {
            if (Campos.Equals("TODOS"))
            { DdlAeroInsMay.Text = "0"; }
            TxtPnInsMay.Text = "";
            TxtSnInsMay.Text = "";
            ViewState["CodElemento"] = "";
            ViewState["CodModelo"] = "";
            TxtUbiTecInsMay.Text = "";
            DdlPosicInsMay.Text = "";
            TxtFechaInsMay.Text = "";
            TxtMotivInsMay.Text = "";
        }
        protected void DdlAeroInsMay_TextChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsMay("");
            BIndDBusqInsMay();
        }
        protected void BtnPNInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "PN";
            BIndDBusqInsMay();
        }
        protected void BtnSNInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "SN";
            BIndDBusqInsMay();
        }
        protected void BtnUltNivInsMay_Click(object sender, EventArgs e)
        {
            ViewState["PNSN"] = "UN";
            BIndDBusqInsMay();
        }
        protected void BtnAKVirtualInsMay_Click(object sender, EventArgs e)
        {
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 1;
            DdlHkConsAeroVirtual.Text = DdlAeroInsMay.Text;
            BIndDConsAeroVirtual(DdlAeroInsMay.Text);
        }
        protected void BtnGuardarInsMay_Click(object sender, EventArgs e)
        {

        }

        protected void TxtFechaInsMay_TextChanged(object sender, EventArgs e)
        {
            if (!DdlAeroInsMay.Text.Equals("0") && !TxtSnInsMay.Text.Equals(""))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string VBQuery = "EXEC SP_PANTALLA_AeronaveVirtual 24,@UBR,'','','',@CodA,0,0,0,@FE,'01-01-1900','01-01-1900'";
                    SqlCommand SC = new SqlCommand(VBQuery, sqlCon);
                    SC.Parameters.AddWithValue("@CodA", DdlAeroInsMay.Text);
                    SC.Parameters.AddWithValue("@FE", TxtFechaInsMay.Text);
                    SC.Parameters.AddWithValue("@UBR", TxtUbiTecInsMay.Text);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        if (Convert.ToInt32(SDR["TieneLV"].ToString()) > 0)
                        { BtnCompensacInsMay.Visible = true; }
                        else
                        { BtnCompensacInsMay.Visible = false; }
                    }
                }
            }
        }
        protected void BtnCompensacInsMay_Click(object sender, EventArgs e)
        {
            if (ViewState["TieneCompensacion"].Equals("N"))
            {
                BIndDCompesacLV(DdlAeroInsMay.Text, TxtFechaInsMay.Text);
                GrdCompensLv.Enabled = true;
                CkbCompensInicioDia.Checked = false;
                CkbCompensInicioDia.Enabled = true;
            }
            else
            {
                GrdCompensLv.Enabled = false;
                CkbCompensInicioDia.Enabled = false;
            }
            ViewState["Ventana"] = MultVw.ActiveViewIndex;
            MultVw.ActiveViewIndex = 2;
        }
        protected void GrdBusqMayDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarCamposInsMay("");
            ViewState["CodModelo"] = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[11].Text.Trim());
            string VbApu_Ref = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[7].Text.Trim());
            string VbApu_Elem = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[8].Text.Trim());
            if (!VbApu_Ref.Equals(VbApu_Elem))
            {
                ScriptManager.RegisterClientScriptBlock(this.UplInsMay, UplInsMay.GetType(), "IdntificadorBloqueScript", "alert('La S/N no se encuentra marcada como APU, debe realizar el cambio en la pantalla [Elemento]')", true);
                return;
            }
            TxtPnInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[1].Text.Trim());
            TxtSnInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[2].Text.Trim());
            ViewState["CodElemento"] = GrdBusqMayDisp.DataKeys[this.GrdBusqMayDisp.SelectedIndex][0].ToString();
            TxtUbiTecInsMay.Text = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[5].Text.Trim());
            string PoscElem = HttpUtility.HtmlDecode(GrdBusqMayDisp.SelectedRow.Cells[10].Text.Trim());
            BIndDHisElemInsMay(ViewState["CodElemento"].ToString().Trim());
            if (PoscElem.Equals("S"))
            {
                DdlPosicInsMay.Enabled = true;
                string LtxtSql = string.Format("EXEC Consultas_General_Ingenieria 2,'{0}','{1}','',0, 0,0,'01-01-1','01-01-1'", TxtUbiTecInsMay.Text, ViewState["CodModelo"].ToString().Trim());
                DdlPosicInsMay.DataSource = Cnx.DSET(LtxtSql);
                DdlPosicInsMay.DataMember = "Datos";
                DdlPosicInsMay.DataTextField = "Descripcion";
                DdlPosicInsMay.DataValueField = "Codigo";
                DdlPosicInsMay.DataBind();
            }
            else
            { DdlPosicInsMay.Enabled = false; }
            GrdBusqMayDisp.Visible = false;
            BIndDSvcInsMay(ViewState["CodElemento"].ToString().Trim(), ViewState["CodModelo"].ToString().Trim(), DdlAeroInsMay.Text);
            GrdSvcInsMay.Visible = true;
        }
        protected void GrdBusqMayDisp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdBusqMayDisp_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GrdSvcInsMay_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TxtFecUltCumplMay_TextChanged(object sender, EventArgs e)
        {

        }
    }

}