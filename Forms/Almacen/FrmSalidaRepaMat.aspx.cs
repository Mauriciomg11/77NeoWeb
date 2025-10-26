using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Almacen
{
    public partial class FrmSalidaRepaMat : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        DataSet DSDetalle = new DataSet();
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
                    Session["FormatFecha"] = Cnx.GetFormatFecha();// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Salida Reparación";
                Page.Title = TitForm.Text;
                ViewState["PageTit"] = TitForm.Text;
                ViewState["TipoRepa"] = "";
                ViewState["CodOrdenRepa"] = "";
                ViewState["CodRCant"] = "";
                ViewState["PosicionAnt"] = "0";
                ViewState["TtlDespacho"] = "0";
                ModSeguridad();
                TraerDatos("UPD");
                TipoRepa("N");
                MultVw.ActiveViewIndex = 0;
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
            ViewState["VblCE3"] = 1;
            ViewState["VblCE4"] = 1;
            ViewState["VblCE5"] = 1;
            ViewState["VblCE6"] = 1;
            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmMovimientoActivo.aspx", VbPC);
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
                    LblObserv.Text = bO.Equals("LblObsMst") ? bT : LblObserv.Text;
                }
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void TraerDatos(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC PNTLL_Reparacion 4,@U,'','','','','',0,0,0,@Idm, @ICC,'01-01-01','02-01-01','03-01-01'";

                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@U", Session["C77U"]);
                        SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDdl = new DataSet())
                            {
                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Almac";
                                DSTDdl.Tables[1].TableName = "RepaNal";
                                DSTDdl.Tables[2].TableName = "RepaInta";
                                /*DSTDdl.Tables[3].TableName = "EjecCodigo";
                                DSTDdl.Tables[4].TableName = "EjecCodComex";*/
                                ViewState["DSTDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            if (DSTDdl.Tables["Almac"].Rows.Count > 0)
            {
                DdlAlmacen.DataSource = DSTDdl.Tables[0];
                DdlAlmacen.DataTextField = "NomAlmacen";
                DdlAlmacen.DataValueField = "CodIdAlmacen";
                DdlAlmacen.DataBind();
            }
        }
        protected void TipoRepa(string Tipo)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            DSTDdl = (DataSet)ViewState["DSTDdl"];
            ViewState["TipoRepa"] = Tipo;
            if (Tipo.Equals("N"))
            {
                if (DSTDdl.Tables["RepaNal"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaNal"];
                    DdlNumRepa.DataTextField = "CodReparacion";
                }
            }
            else
            {
                if (DSTDdl.Tables["RepaInta"].Rows.Count > 0)
                {
                    DdlNumRepa.DataSource = DSTDdl.Tables["RepaInta"];
                    DdlNumRepa.DataTextField = "CodShippingOrder";
                }
            }
            DdlNumRepa.DataValueField = "Codigo";
            DdlNumRepa.DataBind();
            DdlNumRepa.Text = "";
            /*GrdDtlleRepa.DataSource = null;
            GrdDtlleRepa.DataBind();*/
        }
        protected void RdbNacional_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("N"); }
        protected void RdbInter_CheckedChanged(object sender, EventArgs e)
        { TipoRepa("I"); }
        protected void BindDetRepa(string Accion)
        {
            try
            {
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DSTDdl = (DataSet)ViewState["DSTDdl"];
                string S_RepaNAL_INTA = "";
                if (ViewState["TipoRepa"].ToString().Equals("N")) { S_RepaNAL_INTA = "RepaNal"; }
                else { S_RepaNAL_INTA = "RepaInta"; }
                if (DSTDdl.Tables[S_RepaNAL_INTA].Rows.Count > 0)
                {
                    DataTable DT = new DataTable();
                    DT = DSTDdl.Tables[S_RepaNAL_INTA].Clone();
                    DataRow[] DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo='" + DdlNumRepa.Text.Trim() + "'");
                    if (Cnx.ValidaDataRowVacio(DR))
                    {
                        DT = DR.CopyToDataTable();
                        TxtMoneda.Text = DT.Rows[0]["CodMoneda"].ToString().Trim();
                        DataTable DTEC = new DataTable();
                        DataRow[] DREC; DataRow[] Result;
                        //string S_AplicaComex = "S";
                      /* if (ViewState["TipoRepa"].ToString().Equals("I")) //Si es internacional valida que este liquidada la orden de embarque
                        {
                            DREC = DSTDdl.Tables["EjecCodigo"].Select("Caso = 5 AND EjecutarCodigo = 'S'"); //Si aplica validacion de la liquidacion
                            if (Cnx.ValidaDataRowVacio(DREC))
                            {
                                DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("EstadoLiquidacion <> 1");//Esta liquidada
                                if (Cnx.ValidaDataRowVacio(DR))
                                {
                                    DT = DR.CopyToDataTable();
                                    Result = Idioma.Select("Objeto= 'Msj05EntC'"); //La orden de embarque no se encuentra liquidada.
                                    foreach (DataRow row in Result)
                                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + " | " + DdlNumRepa.Text.Trim() + "');", true); }
                                    GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                                    return;
                                }
                            }
                            DREC = DSTDdl.Tables["EjecCodComex"].Select("Caso = 5 AND EjecutarCodigo = 'N'"); //Aplica COMEX
                            if (Cnx.ValidaDataRowVacio(DREC)) { S_AplicaComex = "N"; }
                        } */
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Aprobado = 0");
                        if (Cnx.ValidaDataRowVacio(DR))// Si la Compra esta aprobada
                        {
                            DT = DR.CopyToDataTable();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('La reparación no se encuentra aprobada  | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true);

                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        DR = DSTDdl.Tables[S_RepaNAL_INTA].Select("Codigo ='" + DdlNumRepa.Text.Trim() + "' AND Asentado = 1");
                        if (Cnx.ValidaDataRowVacio(DR))/* Si la Compra esta asentada*/
                        {
                            DT = DR.CopyToDataTable();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('La reparación se encuentra asentada. | " + DT.Rows[0]["CodReparacion"].ToString().Trim() + "');", true);

                            GrdDtlleRepa.DataSource = null; GrdDtlleRepa.DataBind();
                            return;
                        }
                        if (Accion.Equals("UPD"))
                        {
                            Cnx.SelecBD();
                            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                            {
                                string VbTxtSql = " EXEC PNTLL_Reparacion 5, @CodOC,'','','','',@TipoOC,0,0,0,4, @ICC,'01-01-01','02-01-01','03-01-01'";
                                sqlConB.Open();
                                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                                {
                                    SC.Parameters.AddWithValue("@CodOC", DdlNumRepa.Text.Trim());
                                    //SC.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                    SC.Parameters.AddWithValue("@TipoOC", ViewState["TipoRepa"]);
                                    //SC.Parameters.AddWithValue("@ApliComex", S_AplicaComex);
                                    using (SqlDataAdapter SDA = new SqlDataAdapter())
                                    {
                                        using (DataSet DSDetalle = new DataSet())
                                        {
                                            SDA.SelectCommand = SC;
                                            SDA.Fill(DSDetalle);
                                            DSDetalle.Tables[0].TableName = "EstadoRepa";
                                            /*DSDetalle.Tables[1].TableName = "CondManip";
                                            DSDetalle.Tables[2].TableName = "CurTemporal";
                                            DSDetalle.Tables[3].TableName = "CurActualizar";*/
                                            ViewState["DSDetalle"] = DSDetalle;
                                        }
                                    }
                                }
                            }
                        }
                        DSDetalle = (DataSet)ViewState["DSDetalle"];
                        if (DSDetalle.Tables["EstadoRepa"].Rows.Count > 0)
                        { GrdDtlleRepa.DataSource = DSDetalle.Tables["EstadoRepa"]; }
                        GrdDtlleRepa.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                String S_Ex = Ex.Message;
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }
            }
        }
        protected void DdlNumRepa_TextChanged(object sender, EventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            if (!DdlNumRepa.SelectedItem.Value.Equals("")) { BindDetRepa("UPD"); }
        }
        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {

        }

        protected void IbtCerrarAsing_Click(object sender, ImageClickEventArgs e)
        {
            Page.Title = ViewState["PageTit"].ToString().Trim();
            ViewState["PosicionAnt"] = ViewState["Posicion"];
            ViewState["CodRCant"] = ViewState["CodOrdenRepa"];
            MultVw.ActiveViewIndex = 0;
        }

        protected void GrdDtlleRepa_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdDtlleRepa_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}