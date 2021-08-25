using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using _77NeoWeb.Prg.PrgLogistica;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        DataTable Idioma = new DataTable();
        DataSet DSTDet = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Datos Aeronave");
            TitForm.Text = "Datos Aeronave";
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
                    Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                ViewState["Validar"] = "S";
                ViewState["Accion"] = "";
                ViewState["AC_Virtual"] = 0;
                ModSeguridad();
                MlVwCampos.ActiveViewIndex = 0;
                Traerdatos("0", "UPD");
                BindBDdlPrmtr("", "");
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
                SC.Parameters.AddWithValue("@F1", "FrmAeronave");
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

                    TitForm.Text = bO.Equals("LblTituloAk") ? bT : TitForm.Text;
                    LblBusqHK.Text = bO.Equals("LblBusqHK") ? bT + ":" : LblBusqHK.Text;
                    LblCodHK.Text = bO.Equals("LblCodHK") ? bT + ":" : LblCodHK.Text;
                    LblTitCampos.Text = bO.Equals("LblTituloAk") ? bT + ":" : LblTitCampos.Text;
                    LblMatr.Text = bO.Equals("LblMatr") ? bT + ":" : LblMatr.Text;
                    LblCCosto.Text = bO.Equals("LblCCosto") ? bT + ":" : LblCCosto.Text;
                    LblFecFabr.Text = bO.Equals("LblFecFabr") ? bT + ":" : LblFecFabr.Text;
                    CkbAdmon.Text = bO.Equals("CkbAdmon") ? "&nbsp" + bT : CkbAdmon.Text;
                    CkbPropiedad.Text = bO.Equals("CkbPropiedad") ? "&nbsp" + bT : CkbPropiedad.Text;
                    LblModelo.Text = bO.Equals("LblModelo") ? bT + ":" : LblModelo.Text;
                    LblTipo.Text = bO.Equals("LblTipo") ? bT + ":" : LblTipo.Text;
                    LblPropie.Text = bO.Equals("LblPropie") ? bT + ":" : LblPropie.Text;
                    LblEstado.Text = bO.Equals("LblEstado") ? bT + ":" : LblEstado.Text;
                    CkbActiva.Text = bO.Equals("ActivaMstr") ? "&nbsp" + bT : CkbActiva.Text;
                    LblTitContadores.Text = bO.Equals("LblTitContadores") ? bT : LblTitContadores.Text;
                    LblFecIngr.Text = bO.Equals("LblFecIngr") ? bT + ":" : LblFecIngr.Text;
                    LblDescri.Text = bO.Equals("LblDescri") ? bT + ":" : LblDescri.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnExpor.Text = bO.Equals("BtnExpor") ? bT : BtnExpor.Text;
                    BtnExpor.ToolTip = bO.Equals("BtnExporTT") ? bT : BtnExpor.ToolTip;
                    BtnSolicitud.Text = bO.Equals("BtnSolicitud") ? bT : BtnSolicitud.Text;
                    BtnSolicitud.ToolTip = bO.Equals("BtnSolicitudTT") ? bT : BtnSolicitud.ToolTip;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'BtnSolicitudOnC'");
                foreach (DataRow row in Result)
                { BtnSolicitud.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }/**/

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindBDdlPrmtr(string CodEstado, string CodCC)
        {
            DSTDet = (DataSet)ViewState["DSTDet"];
            DataRow[] Result;
            string VbCodAnt = "";

            DataTable DTEstd = new DataTable();
            VbCodAnt = CodEstado.Trim();
            DTEstd = DSTDet.Tables[5].Clone();

            Result = DSTDet.Tables[5].Select("CodEstadoAeronave='" + CodEstado.Trim() + "'");// trae el codigo actual por si esta inactivo
            foreach (DataRow Row in Result)
            { DTEstd.ImportRow(Row); }

            Result = DSTDet.Tables[5].Select("Activo=1");
            foreach (DataRow Row in Result)
            { DTEstd.ImportRow(Row); }

            DdlEstado.DataSource = DTEstd;
            DdlEstado.DataTextField = "Descripcion";
            DdlEstado.DataValueField = "CodEstadoAeronave";
            DdlEstado.DataBind();
            DdlEstado.Text = VbCodAnt;

            DataTable DTCCsto = new DataTable();
            VbCodAnt = CodCC.Trim();
            DTCCsto = DSTDet.Tables[6].Clone();

            Result = DSTDet.Tables[6].Select("CodCc='" + CodCC.Trim() + "'");// trae el codigo actual por si esta inactivo
            foreach (DataRow Row in Result)
            { DTCCsto.ImportRow(Row); }

            Result = DSTDet.Tables[6].Select("Activo=1");
            foreach (DataRow Row in Result)
            { DTCCsto.ImportRow(Row); }

            DdlCcosto.DataSource = DTCCsto;
            DdlCcosto.DataTextField = "Nombre";
            DdlCcosto.DataValueField = "CodCc";
            DdlCcosto.DataBind();
            DdlCcosto.Text = VbCodAnt;
        }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtMatr.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una matrícula')", true);
                ViewState["Validar"] = "N";
                return;
            }
            ViewState["Validar"] = "S";
            if (TxtSn.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una serie')", true);
                ViewState["Validar"] = "N";
                return;
            }
            ViewState["Validar"] = "S";
            if (DdlCcosto.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un centro de costo')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtFecFabr.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens04Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar una fecha de fabricación')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlModelo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un modelo'')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (DdlTipo.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un tipo de aeronave')", true);
                ViewState["Validar"] = "N";
                return;
            }

            if (DdlPropie.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens07Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un propietario')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtFecIngr.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens08Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//La fecha de ingreso es requerida')", true);
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtTSN.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens09Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un valor para TSN')", true);
                TxtTSN.Text = "0";
                ViewState["Validar"] = "N";
                return;
            }
            if (TxtCSN.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens10Aero'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + "');", true); }//Debe ingresar un valor para CSN')", true);
                TxtCSN.Text = "0";
                ViewState["Validar"] = "N";
                return;
            }
        }
        protected void Traerdatos(string Prmtr, string Accion)
        {

            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Aeronave 5,'','','','',@Prmtr,0,@Idm,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDet = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDet);
                                DSTDet.Tables[0].TableName = "BusHK";
                                DSTDet.Tables[1].TableName = "Consult";
                                DSTDet.Tables[2].TableName = "Tipo";
                                DSTDet.Tables[3].TableName = "Modelo";
                                DSTDet.Tables[4].TableName = "Propietario";
                                DSTDet.Tables[5].TableName = "Estado";
                                DSTDet.Tables[6].TableName = "CCosto";
                                ViewState["DSTDet"] = DSTDet;
                            }
                        }
                    }
                }
            }
            DSTDet = (DataSet)ViewState["DSTDet"];

            string VbCodAnt = DdlBusqHK.Text.Trim();
            DdlBusqHK.DataSource = DSTDet.Tables[0];
            DdlBusqHK.DataTextField = "Matricula";
            DdlBusqHK.DataValueField = "CodAeronave";
            DdlBusqHK.DataBind();
            DdlBusqHK.Text = VbCodAnt.Equals("0") ? @Prmtr : VbCodAnt;

            DdlTipo.DataSource = DSTDet.Tables[2];
            DdlTipo.DataTextField = "Descripcion";
            DdlTipo.DataValueField = "CodTipoAeronave";
            DdlTipo.DataBind();

            DdlModelo.DataSource = DSTDet.Tables[3];
            DdlModelo.DataTextField = "NomModelo";
            DdlModelo.DataValueField = "CodModelo";
            DdlModelo.DataBind();

            DdlPropie.DataSource = DSTDet.Tables[4];
            DdlPropie.DataTextField = "RazonSocial";
            DdlPropie.DataValueField = "CodTercero";
            DdlPropie.DataBind();

            DataRow[] Result = DSTDet.Tables[1].Select("CodAeronave = " + DdlBusqHK.Text.Trim());
            foreach (DataRow SDR in Result)
            {
                string VbFecha;
                DateTime? FechaD;
                TxtCodHk.Text = SDR["CodAeronave"].ToString();
                string CodEstado = HttpUtility.HtmlDecode(SDR["CodEstadoAeronave"].ToString().Trim());
                string CodCC = HttpUtility.HtmlDecode(SDR["CentroDeCosto"].ToString().Trim());
                BindBDdlPrmtr(CodEstado, CodCC);
                CkbActiva.Checked = Convert.ToBoolean(SDR["Activa"].ToString());
                TxtMatr.Text = HttpUtility.HtmlDecode(SDR["Matricula"].ToString().Trim());
                TxtSn.Text = HttpUtility.HtmlDecode(SDR["SN"].ToString().Trim());
                VbFecha = HttpUtility.HtmlDecode(SDR["FechaFabricante"].ToString().Trim());
                if (!VbFecha.Trim().Equals(""))
                { FechaD = Convert.ToDateTime(VbFecha); TxtFecFabr.Text = String.Format("{0:dd/MM/yyyy}", FechaD); }
                else
                { TxtFecFabr.Text = ""; }
                CkbAdmon.Checked = Convert.ToBoolean(SDR["Administrada"].ToString());
                CkbPropiedad.Checked = Convert.ToBoolean(SDR["Propiedad"].ToString());
                DdlModelo.Text = HttpUtility.HtmlDecode(SDR["CodModelo"].ToString().Trim());
                DdlTipo.Text = HttpUtility.HtmlDecode(SDR["CodTipoAeronave"].ToString().Trim());
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
            { TxtMatr.Enabled = Ing; TxtTSN.Enabled = Ing; TxtCSN.Enabled = Ing; CkbActiva.Checked = true; }//  CkbAdmon.Enabled = Edi;
            else
            {
                TxtMatr.Enabled = (int)ViewState["VblCE1"] == 1 ? Edi : false; CkbActiva.Enabled = Edi;
                //if (CkbAdmon.Checked == true)// is es administrada habilita el check para desactivar y al desactivar ya queda fuera de la cantidad la liciencia por aeronave
                //{ CkbAdmon.Enabled = Edi; }
            }
            CkbAdmon.Enabled = Edi;
            TxtSn.Enabled = Edi;
            DdlCcosto.Enabled = Edi;
            IbtFecFabr.Enabled = Edi;

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
            CkbActiva.Checked = false;
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
        { Traerdatos(DdlBusqHK.Text, "UPD"); }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {
                if (ViewState["Accion"].ToString().Equals(""))
                {
                    ActivarBtn(true, false, false, false, false);

                    ViewState["Accion"] = "Aceptar";
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    LimpiarCampos();
                    TxtCodHk.Text = "0";
                    ActivarCampos(true, true, "Ingresar");
                    DdlBusqHK.SelectedValue = "0";
                    DdlBusqHK.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
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
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    ViewState["Accion"] = "";
                    DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                    foreach (DataRow row in Result)
                    { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                    ActivarCampos(false, false, "Ingresar");
                    DdlBusqHK.Enabled = true;
                    Traerdatos(ClsAeronave.GetCodHK().ToString().Trim(), "UPD");
                    BindBDdlPrmtr(DdlEstado.Text.Trim(), DdlCcosto.Text.Trim());
                    BtnIngresar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//Inconveniente en el ingreso')", true);
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "INGRESAR Aeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                if (TxtCodHk.Text.Equals("0") || TxtCodHk.Text.Equals(""))
                { return; }

                if (ViewState["Accion"].ToString().Equals(""))
                {
                    string VbCodEstado, VblCC;
                    VbCodEstado = DdlEstado.Text.Trim();
                    VblCC = DdlCcosto.Text.Trim();
                    DdlEstado.Text = VbCodEstado;
                    DdlCcosto.Text = VblCC;
                    ActivarBtn(false, true, false, false, false);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                    ViewState["Accion"] = "Aceptar";
                    ActivarCampos(true, true, "UPDATE");
                    DdlBusqHK.SelectedValue = "0";
                    DdlBusqHK.Enabled = false;
                    Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                    foreach (DataRow row in Result)
                    { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
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
                        Activo = CkbActiva.Checked == true ? "1" : "0",
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
                        DataRow[] Result2 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result2)
                        { Mensj = row["Texto"].ToString().Trim(); }
                        ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                    ActivarBtn(true, true, true, true, true);
                    DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                    foreach (DataRow row in Result)
                    { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                    ViewState["Accion"] = "";
                    ActivarCampos(false, false, "UPDATE");
                    DdlBusqHK.Enabled = true;
                    Traerdatos(ClsAeronave.GetCodHK().ToString().Trim(), "UPD");
                    BindBDdlPrmtr(DdlEstado.Text.Trim(), DdlCcosto.Text.Trim());
                    DdlBusqHK.Text = ClsAeronave.GetCodHK().ToString();
                    BtnModificar.OnClientClick = "";
                }
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }//
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "MODIFICAR Aeronave", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnExpor_Click(object sender, EventArgs e)
        {
            try
            {
                string StSql, VbNomRpt = "";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportAeronave", Session["77IDM"].ToString().Trim());
                StSql = "EXEC SP_PANTALLA_Aeronave 4,'','','','CurExportAeronave',0,0,0,@ICC,'01/01/01','01/01/01','01/01/01'";
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result = Idioma.Select("Objeto= 'Caption'");
                foreach (DataRow row in Result)
                { VbNomRpt = row["Texto"].ToString().Trim(); }// Aeronaves
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel Aeronaves", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void BtnSolicitud_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VblObs = "";
            DataRow[] Result = Idioma.Select("Objeto= 'Mens11Aero'");
            foreach (DataRow row in Result)
            { VblObs = row["Texto"].ToString().Trim() + " [" + TxtMatr.Text.Trim() + "]"; }// Generación solicitud de reparación matrícula

            if (TxtCodHk.Text.Equals("0") || TxtCodHk.Text.Equals(""))
            {
                return;
            }
            List<ClsTypSolicitudPedido> ObjEncSP = new List<ClsTypSolicitudPedido>();
            var TypEncSP = new ClsTypSolicitudPedido()
            {
                IdPedido = 0,
                CodPedido = "",
                Fechapedido = Convert.ToDateTime(DateTime.Now),
                CodPrioridad = "NORMAL",
                CodResponsable = Session["C77U"].ToString(),
                CodReserva = 0,
                CodEstado = "A",
                Obsevacion = VblObs,
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
                IdDetPedido = 0,
                CodReferencia = "N/A",
                PN = "N/A",
                CodUndMedida = "EA",
                CantidadTotal = 1,
                CantidadAlmacen = 1,
                CantidadReparacion = 0,
                CantidadOrden = 0,
                Posicion = 1,
                AprobacionDetalle = 0,
                CodSeguimiento = "SOL",
                Descripcion = "Generación solicitud de reparación matrícula [" + TxtMatr.Text.Trim() + "]",
                TipoPedido = 0,
                CantidadAjustada = 1,
                Notas = TxtSn.Text.Trim(),
                PosicionPr = 0,
                IdSrvPr = 0,
                IdReporte = 0,
                IdDetProPSrvSP = 0,
                CodIdDetalleResSP = 0,
                FechaAprob = null,
                CodAeronaveSP = Convert.ToInt32(TxtCodHk.Text),


            };
            ObjDetSP.Add(TypDetSP);

            ClsTypSolicitudPedido TypSolicitudPedido = new ClsTypSolicitudPedido();

            TypSolicitudPedido.Alimentar(ObjEncSP, ObjDetSP);
            string VblPn = TypSolicitudPedido.GetPN();
            string Mensj = TypSolicitudPedido.GetMensj();
            if (!Mensj.Trim().Equals(""))
            {
                ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + "[" + VblPn + "] " + Mensj + "')", true);
                return;
            }
            string VbCodPedido = TypSolicitudPedido.GetCodPedido();
            DataRow[] Result1 = Idioma.Select("Objeto= 'MstrMens03'");
            foreach (DataRow row in Result1)
            { ScriptManager.RegisterClientScriptBlock(this.UpPlHk, UpPlHk.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString().Trim() + " [" + VbCodPedido + "]" + "');", true); }// Se generó la solicitud Nro
        }
    }
}