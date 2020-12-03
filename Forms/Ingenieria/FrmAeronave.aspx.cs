using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmAeronave : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
             if (Session["Login77"] == null)
             {
                 Response.Redirect("~/FrmAcceso.aspx");
             }/* */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Datos Aeronave");
            TitForm.Text = "Datos Aeronave";
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";             
            }
            if (!IsPostBack)
            {
                ViewState["Validar"] = "S";
                ViewState["AC_Virtual"] = 0;
                ModSeguridad();
                MlVwCampos.ActiveViewIndex = 0;
                BindBDdlBusq();
                BindBDdl();               
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

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                BtnIngresar.Visible = false;
                /*GrdRecursoF.ShowFooter = false;*/
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
                BtnModificar.Visible = false;
            }
            if (ClsP.GetConsultar() == 0)
            {
            }
            if (ClsP.GetImprimir() == 0)
            {
                ViewState["VblImpMS"] = 0;
                //BtnImprimir.Visible = false;
            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                //BtnEliminar.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {
                //Modificar matrícula
                ViewState["VblCE1"] = 0;
            }
            if (ClsP.GetCE2() == 0)
            {
                BtnSolicitud.Visible = false;
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

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,2,@F,3,@F,4,@F,6,@F,7,@F,8,@F,12,@F,13,@F,14");
                /*  SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                  SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                  SC.Parameters.AddWithValue("@F", "FrmReporte");
                  sqlCon.Open();
                  SqlDataReader Regs = SC.ExecuteReader();
                  while (Regs.Read())
                  {
                      VbCaso = Convert.ToInt32(Regs["CASO"]);
                      VbAplica = Regs["EjecutarCodigo"].ToString();
                      if (VbCaso == 2 && VbAplica.Equals("S"))
                      {
                          //Asignar por defecto usuario logiado en abrir y cerrar reporte manto
                          ViewState["UsuDefecto"] = "S";
                      }                   
                  }*/
            }
        }
        protected void BindBDdlBusq()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_LibroVuelo 20,'','','','MTR',{0},{1},{2},0,'01-1-2009','01-01-1900','01-01-1900'",
                1, 2, "0");
            DdlBusqHK.DataSource = Cnx.DSET(LtxtSql);
            DdlBusqHK.DataMember = "Datos";
            DdlBusqHK.DataTextField = "Matricula";
            DdlBusqHK.DataValueField = "CodAeronave";
            DdlBusqHK.DataBind();
        }
        protected void BindBDdl()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 1,'','','','TIP',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlTipo.DataSource = Cnx.DSET(LtxtSql);
            DdlTipo.DataMember = "Datos";
            DdlTipo.DataTextField = "Descripcion";
            DdlTipo.DataValueField = "CodTipoAeronave";
            DdlTipo.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 1,'','','','MOD',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlModelo.DataSource = Cnx.DSET(LtxtSql);
            DdlModelo.DataMember = "Datos";
            DdlModelo.DataTextField = "NomModelo";
            DdlModelo.DataValueField = "CodModelo";
            DdlModelo.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 1,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
            DdlPropie.DataSource = Cnx.DSET(LtxtSql);
            DdlPropie.DataMember = "Datos";
            DdlPropie.DataTextField = "RazonSocial";
            DdlPropie.DataValueField = "CodTercero";
            DdlPropie.DataBind();
        }
        protected void BindBDdlRefresh(string CodEstado, string CodCC)
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 1,'{0}','','','EST',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", CodEstado.Trim());
            DdlEstado.DataSource = Cnx.DSET(LtxtSql);
            DdlEstado.DataMember = "Datos";
            DdlEstado.DataTextField = "Descripcion";
            DdlEstado.DataValueField = "CodEstadoAeronave";
            DdlEstado.DataBind();

            LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 2,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'", CodCC.Trim());
            DdlCcosto.DataSource = Cnx.DSET(LtxtSql);
            DdlCcosto.DataMember = "Datos";
            DdlCcosto.DataTextField = "Nombre";
            DdlCcosto.DataValueField = "CodCc";
            DdlCcosto.DataBind();
        }
        protected void ValidarCampos(string Accion)
        {
            ViewState["Validar"] = "S";
            if (TxtMatr.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una matrícula')", true);
                ViewState["Validar"] = "N";
                return;
            }
            ViewState["Validar"] = "S";
            if (TxtSn.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una serie')", true);
                ViewState["Validar"] = "N";
                return;
            }
            ViewState["Validar"] = "S";
            if (DdlCcosto.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un centro de costo')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtFecFabr.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha de fabricación')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlModelo.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un modelo'')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un tipo de aeronave')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un tipo de aeronave')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlPropie.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un propietario')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtFecIngr.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar una fecha de ingreso')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtTSN.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un valor para TSN')", true);
                TxtTSN.Text = "0";
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtCSN.Text.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Debe ingresar un valor para CSN')", true);
                TxtCSN.Text = "0";
                ViewState["Validar"] = "N";
                return;
            }
        }
        protected void Traerdatos(string Prmtr)
        {
            try
            {

                Cnx.SelecBD();
                using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
                {
                    string VbFecha;
                    DateTime? FechaD;
                    Cnx2.Open();
                    string LtxtSql = string.Format("EXEC SP_PANTALLA_Aeronave 5,'','','','',@Prmtr,0,0,0,'01-1-2009','01-01-1900','01-01-1900'");
                    SqlCommand SC = new SqlCommand(LtxtSql, Cnx2);
                    SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        TxtCodHk.Text = SDR["CodAeronave"].ToString();
                        string CodEstado = HttpUtility.HtmlDecode(SDR["CodEstadoAeronave"].ToString().Trim());
                        string CodCC = HttpUtility.HtmlDecode(SDR["CentroDeCosto"].ToString().Trim());
                        BindBDdlRefresh(CodEstado, CodCC);
                        DdlEstado.Text = CodEstado;
                        TxtMatr.Text = HttpUtility.HtmlDecode(SDR["Matricula"].ToString().Trim());
                        TxtSn.Text = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                        DdlCcosto.Text = CodCC;
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaFabricante"].ToString().Trim());
                        if (!VbFecha.Trim().Equals(""))
                        { FechaD = Convert.ToDateTime(VbFecha); TxtFecFabr.Text = String.Format("{0:dd/MM/yyyy}", FechaD); }
                        else
                        { TxtFecFabr.Text = ""; }
                        CkbAdmon.Checked = Convert.ToBoolean(SDR["Administrada"].ToString());
                        CkbPropiedad.Checked = Convert.ToBoolean(SDR["Propiedad"].ToString());
                        DdlModelo.Text = HttpUtility.HtmlDecode(SDR["CodModelo"].ToString().Trim());
                        DdlTipo.Text = HttpUtility.HtmlDecode(SDR["CodTipoAeronave"].ToString().Trim());
                        string borrar = HttpUtility.HtmlDecode(SDR["CodPropietario"].ToString().Trim());
                        DdlPropie.Text = HttpUtility.HtmlDecode(SDR["CodPropietario"].ToString().Trim());
                        VbFecha = HttpUtility.HtmlDecode(SDR["FechaIngreso"].ToString().Trim());
                        if (!VbFecha.Trim().Equals(""))
                        { FechaD = Convert.ToDateTime(VbFecha); TxtFecIngr.Text = String.Format("{0:dd/MM/yyyy}", FechaD); }
                        else
                        { TxtFecIngr.Text = ""; }
                        TxtTSN.Text = SDR["TSN"].ToString().Trim();
                        TxtCSN.Text = SDR["CSN"].ToString();
                        TxtDescri.Text = HttpUtility.HtmlDecode(SDR["Descripcion"].ToString().Trim());
                        ViewState["AC_Virtual"] = Convert.ToInt32(SDR["AC_Virtual"].ToString().Trim());
                        DdlModelo.ToolTip = "";
                    }
                    SDR.Close();
                    Cnx2.Close();
                }
            }
            catch (Exception Ex)
            {
                string VbMEns = Ex.ToString().Trim().Substring(1, 50);
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente con la consulta')", true);
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnExpor.Enabled = Otr;
            BtnSolicitud.Enabled = Otr;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            if (accion.Equals("Ingresar"))
            { TxtMatr.Enabled = Ing; TxtTSN.Enabled = Ing; TxtCSN.Enabled = Ing; }
            else { TxtMatr.Enabled = (int)ViewState["VblCE1"] == 1 ? Edi : false; }

            TxtSn.Enabled = Edi;
            DdlCcosto.Enabled = Edi;
            IbtFecFabr.Enabled = Edi;
            CkbAdmon.Enabled = Edi;
            CkbPropiedad.Enabled = Edi;
            DdlModelo.Enabled = Edi;
            DdlModelo.ToolTip = "";
            if ((int)ViewState["AC_Virtual"] > 0)
            { DdlModelo.Enabled = false; DdlModelo.ToolTip = "Tiene elementos instalados en la aeronave virtual"; }
            DdlTipo.Enabled = Edi;
            DdlPropie.Enabled = Edi;
            DdlEstado.Enabled = Edi;
            IbtFecIngr.Enabled = Edi;
            TxtDescri.Enabled = Edi;
        }
        protected void LimpiarCampos()
        {
            TxtMatr.Text = "";
            TxtSn.Text = "";
            DdlCcosto.Text = "";
            TxtFecFabr.Text = "";
            CkbAdmon.Checked = false;
            CkbPropiedad.Checked = false;
            DdlModelo.Text = "";
            DdlTipo.Text = "";
            DdlPropie.Text = "";
            DdlEstado.Text = "01";
            TxtFecIngr.Text = "";
            TxtTSN.Text = "0";
            TxtCSN.Text = "0";
            TxtDescri.Text = "";
        }
        protected void DdlBusqHK_TextChanged(object sender, EventArgs e)
        {
            Traerdatos(DdlBusqHK.Text);
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                if (BtnIngresar.Text == "Ingresar")
                {
                    ActivarBtn(true, false, false, false, false);
                    BtnIngresar.Text = "Aceptar";
                    LimpiarCampos();
                    TxtCodHk.Text = "0";
                    ActivarCampos(true, true, "Ingresar");
                    BindBDdlRefresh("", "");
                    DdlBusqHK.SelectedValue = "0";
                    DdlBusqHK.Enabled = false;
                    BtnIngresar.OnClientClick = "return confirm('¿Desea realizar el ingreso?');";
                }
                else
                {
                    ValidarCampos("INSERT");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    List<CsTypAeronave> ObjAeronave = new List<CsTypAeronave>();
                    var TypAeronave = new CsTypAeronave()
                    {
                        CodAeronave = Convert.ToInt32(TxtCodHk.Text),
                        SN = TxtSn.Text.Trim(),
                        Matricula = TxtMatr.Text.Trim(),
                        FechaFabricante = Convert.ToDateTime(TxtFecFabr.Text),
                        FechaIngreso = Convert.ToDateTime(TxtFecIngr.Text),
                        CodModelo = DdlModelo.Text.Trim(),
                        CodPropietario = DdlPropie.Text.Trim(),
                        CodTipoAeronave = DdlTipo.Text.Trim(),
                        CodProveedor = "",
                        CodEstadoAeronave = DdlEstado.Text.Trim(),
                        Activo = "1",
                        Descripcion = TxtDescri.Text.Trim(),
                        HoraVoladaIng = Convert.ToDouble(TxtTSN.Text),
                        Usu = Session["C77U"].ToString(),
                        Bloqueada = 0,
                        Propiedad = CkbAdmon.Checked == true ? 1 : 2,
                        CentroDeCosto = DdlCcosto.Text.Trim(),
                        PropiedadCia = CkbPropiedad.Checked == true ? 1 : 0,
                        CSN = Convert.ToInt32(TxtCSN.Text),
                        Accion = "INSERT",
                    };
                    ObjAeronave.Add(TypAeronave);
                    CsTypAeronave ClsAeronave = new CsTypAeronave();
                    ClsAeronave.Alimentar(ObjAeronave);
                    string Mensj = ClsAeronave.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    BtnIngresar.Text = "Ingresar";
                    ActivarCampos(false, false, "Ingresar");
                    DdlBusqHK.Enabled = true;
                    BindBDdlBusq();
                    DdlBusqHK.Text = ClsAeronave.GetCodHK().ToString();
                    Traerdatos(ClsAeronave.GetCodHK().ToString());
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Aeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtCodHk.Text.Equals("0") || TxtCodHk.Text.Equals(""))
                { return; }

                if (BtnModificar.Text == "Modificar")
                {
                    string VbCodEstado, VblCC;
                    VbCodEstado = DdlEstado.Text.Trim();
                    VblCC = DdlCcosto.Text.Trim();
                    BindBDdlRefresh(VbCodEstado, VblCC);
                    DdlEstado.Text = VbCodEstado;
                    DdlCcosto.Text = VblCC;
                    ActivarBtn(false, true, false, false, false);
                    BtnModificar.Text = "Aceptar";
                    ActivarCampos(true, true, "UPDATE");
                    DdlBusqHK.SelectedValue = "0";
                    DdlBusqHK.Enabled = false;
                    BtnModificar.OnClientClick = "return confirm('¿Desea realizar la edición?');";
                }
                else
                {
                    ValidarCampos("UPDATE");
                    if (ViewState["Validar"].Equals("N"))
                    { return; }

                    List<CsTypAeronave> ObjAeronave = new List<CsTypAeronave>();
                    var TypAeronave = new CsTypAeronave()
                    {
                        CodAeronave = Convert.ToInt32(TxtCodHk.Text),
                        SN = TxtSn.Text.Trim(),
                        Matricula = TxtMatr.Text.Trim(),
                        FechaFabricante = Convert.ToDateTime(TxtFecFabr.Text),
                        FechaIngreso = Convert.ToDateTime(TxtFecIngr.Text),
                        CodModelo = DdlModelo.Text.Trim(),
                        CodPropietario = DdlPropie.Text.Trim(),
                        CodTipoAeronave = DdlTipo.Text.Trim(),
                        CodProveedor = "",
                        CodEstadoAeronave = DdlEstado.Text.Trim(),
                        Activo = "1",
                        Descripcion = TxtDescri.Text.Trim(),
                        HoraVoladaIng = Convert.ToDouble(TxtTSN.Text),
                        Usu = Session["C77U"].ToString(),
                        Bloqueada = 0,
                        Propiedad = CkbAdmon.Checked == true ? 1 : 2,
                        CentroDeCosto = DdlCcosto.Text.Trim(),
                        PropiedadCia = CkbPropiedad.Checked == true ? 1 : 0,
                        CSN = Convert.ToInt32(TxtCSN.Text),
                        Accion = "UPDATE",
                    };
                    ObjAeronave.Add(TypAeronave);
                    CsTypAeronave ClsAeronave = new CsTypAeronave();
                    ClsAeronave.Alimentar(ObjAeronave);
                    string Mensj = ClsAeronave.GetMensj();
                    if (!Mensj.Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    BtnModificar.Text = "Modificar";
                    ActivarCampos(false, false, "Ingresar");
                    DdlBusqHK.Enabled = true;
                    BindBDdlBusq();
                    DdlBusqHK.Text = ClsAeronave.GetCodHK().ToString();
                    Traerdatos(ClsAeronave.GetCodHK().ToString());
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('Inconveniente en la actualización')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Aeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {

        }
        protected void BtnExpor_Click(object sender, EventArgs e)
        {
            try
            {
                string StSql, VbNomRpt;
                StSql = "EXEC SP_PANTALLA_Aeronave 4,'','','','',0,0,0,0,'01/01/01','01/01/01','01/01/01'";
                VbNomRpt = "Aeronaves";                
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;                      
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SC.Connection = con;
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "77NeoWeb";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
                                    Response.Charset = "";
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel Aeronaves", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnSolicitud_Click(object sender, EventArgs e)
        {
            if(TxtCodHk.Text.Equals("0") || TxtCodHk.Text.Equals(""))
            {
                return;
            }
            List<ClsTypSolicitudPedido> ObjEncSP = new List<ClsTypSolicitudPedido>();
            var TypEncSP = new ClsTypSolicitudPedido()
            {
                IdPedido = 0,
                CodPedido ="",              
                Fechapedido = Convert.ToDateTime(DateTime.Now),
                CodPrioridad = "NORMAL",
                CodResponsable = Session["C77U"].ToString(),
                CodReserva=0,
                CodEstado = "A",
                Obsevacion = "Generación solicitud de reparación matrícula ["+ TxtMatr.Text.Trim() + "]",
                CodtipoSolPedido = "02",
                Ccostos = DdlCcosto.Text.Trim(),
                Usu = Session["C77U"].ToString(),
                CodTipoCodigo = Session["CodTipoCodigoInicial"].ToString(),
                FechaRemocionSP = null,
                Aplicabilidad = "AERONAVE",
                Accion = "INSERT",
            };
            ObjEncSP.Add(TypEncSP);

            List<ClsTypSolicitudPedido> ObjDetSP = new List<ClsTypSolicitudPedido>();   
                var TypDetSP = new ClsTypSolicitudPedido()
                {
                    IdDetPedido =0,
                    CodReferencia = "N/A",
                    PN = "N/A",
                    CodUndMedida = "EA",
                    CantidadTotal =1,
                    CantidadAlmacen =1,
                    CantidadReparacion =0,
                    CantidadOrden =0,
                    Posicion =1,
                    AprobacionDetalle =0,
                    CodSeguimiento ="SOL",
                    Descripcion = "Generación solicitud de reparación matrícula [" + TxtMatr.Text.Trim() + "]",
                    TipoPedido =0,
                    CantidadAjustada =1,
                    Notas =TxtSn.Text.Trim(),
                    PosicionPr =0,
                    IdSrvPr =0,
                    IdReporte =0,
                    IdDetProPSrvSP =0,
                    CodIdDetalleResSP =0,
                    FechaAprob =null,
                    CodAeronaveSP =Convert.ToInt32(TxtCodHk.Text),


                };
            ObjDetSP.Add(TypDetSP);

            ClsTypSolicitudPedido TypSolicitudPedido = new ClsTypSolicitudPedido();

            TypSolicitudPedido.Alimentar(ObjEncSP, ObjDetSP);
            string Mensj = TypSolicitudPedido.GetMensj();
            if (!Mensj.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                return;
            }
            string VbCodPedido = TypSolicitudPedido.GetCodPedido();
            ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert(' Se generó la solicitud Nro [" + VbCodPedido + "]')", true);
        }
    }
}