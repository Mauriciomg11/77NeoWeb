using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Configuracion.InventarioLogistica
{
    public partial class FrmBodega : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable DTDet = new DataTable();
        DataSet DSTDet = new DataSet();
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
                ModSeguridad();
                Traerdatos("0", "UPDATE");
                BindDBodg("", "UPD");
                ViewState["Accion"] = "";
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
            ClsP.Acceder(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim() + ".aspx");
            if (ClsP.GetAccesoFrm() == 0) { Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx"); }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0; BtnIngresar.Visible = false;
                GrdDetalle.ShowFooter = false;
            }
            if (ClsP.GetModificar() == 0) { ViewState["VblModMS"] = 0; BtnModificar.Visible = false; }
            if (ClsP.GetConsultar() == 0) { }
            if (ClsP.GetImprimir() == 0) { ViewState["VblImpMS"] = 0; }
            if (ClsP.GetEliminar() == 0) { ViewState["VblEliMS"] = 0; BtnEliminar.Visible = false; }
            if (ClsP.GetCE1() == 0) { ViewState["VblCE1"] = 0; } // licencias
            if (ClsP.GetCE2() == 0) { ViewState["VblCE2"] = 0; }//asignar Cursos
            if (ClsP.GetCE3() == 0) { ViewState["VblCE3"] = 0; }// boton para asignar la persona al grupo de manto y crar usuario
            if (ClsP.GetCE4() == 0) { }
            if (ClsP.GetCE5() == 0) { }
            if (ClsP.GetCE6() == 0) { }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbAplica;
                int VbCaso;
                string TxQry = string.Format("EXEC SP_HabilitarCampos @Nit,@F,1,'',0,'',0,'',0,'',0,'',0,'',0,'',0,'',0");
                SqlCommand SC = new SqlCommand(TxQry, sqlCon);
                SC.Parameters.AddWithValue("@Nit", Session["Nit77Cia"].ToString());
                SC.Parameters.AddWithValue("@F", "MRO");
                sqlCon.Open();
                SqlDataReader Regs = SC.ExecuteReader();
                while (Regs.Read())
                {
                    VbCaso = Convert.ToInt32(Regs["CASO"]);
                    VbAplica = Regs["EjecutarCodigo"].ToString();
                    if (VbCaso == 1 && VbAplica.Equals("S"))
                    { LblCliente.Visible = true; DdlCliente.Visible = true; }
                }
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
                    LblCod.Text = bO.Equals("LblCod") ? bT : LblCod.Text;
                    LblBusq.Text = bO.Equals("LblBusq") ? bT : LblBusq.Text;
                    LblNombre.Text = bO.Equals("LblNombre") ? bT : LblNombre.Text;
                    LblCliente.Text = bO.Equals("LblCliente") ? bT : LblCliente.Text;
                    BtnIngresar.Text = bO.Equals("BtnIngresar") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BtnModificar") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                    GrdDetalle.Columns[0].HeaderText = bO.Equals("GrdFila") ? bT : GrdDetalle.Columns[0].HeaderText;
                    GrdDetalle.Columns[1].HeaderText = bO.Equals("GrdColmn") ? bT : GrdDetalle.Columns[1].HeaderText;
                    GrdDetalle.Columns[2].HeaderText = bO.Equals("GrdAct") ? bT : GrdDetalle.Columns[2].HeaderText;

                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void PerfilesGrid()
        {
            foreach (GridViewRow Row in GrdDetalle.Rows)
            {

                if ((int)ViewState["VblModMS"] == 0)
                {
                    ImageButton imgE = Row.FindControl("IbtEdit") as ImageButton;
                    if (imgE != null)
                    {
                        Row.Cells[3].Controls.Remove(imgE);
                    }
                }
                if ((int)ViewState["VblEliMS"] == 0)
                {
                    ImageButton imgD = Row.FindControl("IbtDelete") as ImageButton;
                    if (imgD != null)
                    {
                        Row.Cells[3].Controls.Remove(imgD);
                    }
                }
            }
        }
        protected void LimpiarCampos()
        { TxtCod.Text = ""; TxtNombre.Text = ""; DdlCliente.Text = ""; }
        protected void ValidarCampos(string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["Validar"] = "S";
            if (TxtCod.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Bodg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar la codig
                ViewState["Validar"] = "N"; TxtCod.Focus(); return;
            }
            if (TxtNombre.Text.Trim().Equals(""))
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Bodg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }// Debe ingresar un nombre
                ViewState["Validar"] = "N"; TxtNombre.Focus(); return;
            }
        }
        protected void ActivarBtn(bool In, bool Md, bool El, bool Ip, bool Otr)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
        }
        protected void ActivarCampos(bool Ing, bool Edi, string accion)
        {
            TxtCod.Enabled = Ing; DdlCliente.Enabled = Ing;
            if (!TxtCod.Text.Trim().Equals("REPA")) { TxtNombre.Enabled = Edi; }
        }
        protected void Traerdatos(string Prmtr, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (Accion.Equals("UPDATE"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Bodega 4,'','','','SELECT',@Prmtr,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlConB.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                    {
                        SC.Parameters.AddWithValue("@Prmtr", Prmtr);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        using (SqlDataAdapter SDA = new SqlDataAdapter())
                        {
                            using (DataSet DSTDet = new DataSet())
                            {

                                SDA.SelectCommand = SC;
                                SDA.Fill(DSTDet);
                                DSTDet.Tables[0].TableName = "Bodega";
                                DSTDet.Tables[1].TableName = "Tercero";
                                DSTDet.Tables[2].TableName = "Consult";
                                ViewState["DSTDet"] = DSTDet;
                            }
                        }
                    }
                }
            }
            DSTDet = (DataSet)ViewState["DSTDet"];

            string VbCodAnt = DdlBusq.Text.Trim();
            DdlBusq.DataSource = DSTDet.Tables[0];
            DdlBusq.DataTextField = "CodBodega";
            DdlBusq.DataValueField = "CodidBodega";
            DdlBusq.DataBind();
            DdlBusq.Text = VbCodAnt.Equals("0") ? @Prmtr : VbCodAnt;

            DdlCliente.DataSource = DSTDet.Tables[1];
            DdlCliente.DataTextField = "RazonSocial";
            DdlCliente.DataValueField = "IdTercero";
            DdlCliente.DataBind();


            DataRow[] Result = DSTDet.Tables[2].Select("CodidBodega = " + DdlBusq.Text.Trim());
            foreach (DataRow DR in Result)
            {
                TxtCod.Text = HttpUtility.HtmlDecode(DR["CodBodega"].ToString().Trim());
                TxtNombre.Text = HttpUtility.HtmlDecode(DR["Nombre"].ToString().Trim());
                DdlCliente.Text = HttpUtility.HtmlDecode(DR["CodTercero"].ToString().Trim());
            }
        }
        protected void DdlBusq_TextChanged(object sender, EventArgs e)
        {
            Traerdatos(DdlBusq.Text, "UPDATE"); BindDBodg(DdlBusq.SelectedItem.Text.Trim(), "SEL");
            if (TxtCod.Text.Trim().Equals("REPA")) { BtnEliminar.Enabled = false; }
            else { BtnEliminar.Enabled = true; }
        }
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (ViewState["Accion"].ToString().Equals(""))
            {
                ActivarBtn(true, false, false, false, false);

                ViewState["Accion"] = "Aceptar";
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                LimpiarCampos();
                ActivarCampos(true, true, "Ingresar");
                DdlBusq.SelectedValue = "0";
                DdlBusq.Enabled = false;
                BindDBodg("", "SEL");
                GrdDetalle.Enabled = false;
                Result = Idioma.Select("Objeto= 'MensConfIng'"); // |MensConfMod
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso?
            }
            else
            {
                string Mensj = "", PCod = "";

                ValidarCampos("INSERT");
                if (ViewState["Validar"].Equals("N"))
                { return; }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 8,@Cd,@Nm,@Us,@Cl,'','','','TblBodega','INSERT',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@Cl", DdlCliente.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                {
                                    Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                    PCod = SDR["CodId"].ToString();
                                }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result1)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                ActivarBtn(true, true, true, true, true);
                                ViewState["Accion"] = "";
                                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                                foreach (DataRow row in Result)
                                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                                ActivarCampos(false, false, "Ingresar");
                                DdlBusq.Enabled = true;
                                Traerdatos(PCod, "UPDATE");
                                DdlBusq.Text = PCod;
                                GrdDetalle.Enabled = true;
                                BtnIngresar.OnClientClick = "";
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();

                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Ingresar", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                            }
                        }
                    }
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            PerfilesGrid();
            Idioma = (DataTable)ViewState["TablaIdioma"];

            if (DdlBusq.Text.Equals("0"))
            { return; }

            if (ViewState["Accion"].ToString().Equals(""))
            {
                ActivarBtn(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                ViewState["Accion"] = "Aceptar";
                ActivarCampos(false, true, "UPDATE");
                DdlBusq.Enabled = false;
                Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  
            }
            else
            {
                string Mensj = "";
                if (DdlBusq.Text.Equals("0"))
                { return; }
                ValidarCampos("UPDATE");
                if (ViewState["Validar"].Equals("N"))
                { return; }

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        string VBQuery = "EXEC SP_TablasGeneral 8,@Cd,@Nm,@Us,@Cl,'','','','TblBodega','UPDATE',@ID,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            try
                            {
                                SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                                SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                                SC.Parameters.AddWithValue("@Cl", DdlCliente.Text.Trim());
                                SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                                SqlDataReader SDR = SC.ExecuteReader();
                                if (SDR.Read())
                                { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                                SDR.Close();

                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result1)
                                    { Mensj = row["Texto"].ToString().Trim(); }

                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                ActivarBtn(true, true, true, true, true);
                                DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                                foreach (DataRow row in Result)
                                { BtnModificar.Text = row["Texto"].ToString().Trim(); }
                                ViewState["Accion"] = "";
                                ActivarCampos(false, false, "UPDATE");
                                DdlBusq.Enabled = true;
                                Traerdatos(DdlBusq.Text.Trim(), "UPDATE");
                                BtnModificar.OnClientClick = "";
                            }
                            catch (Exception Ex)
                            {
                                Transac.Rollback();

                                DataRow[] Result1 = Idioma.Select("Objeto= 'MensErrMod'");
                                foreach (DataRow row in Result1)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = ViewState["PFileName"].ToString(), VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Modificar", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);

                            }
                        }
                    }
                }
            }

        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (DdlBusq.Text.Equals("0"))
            { return; }

            PerfilesGrid();
            string VBQuery, Mensj = "";
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 8,@Cd,@Nm,@Us,'','','','','TblBodega','DELETE',@ID,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Cd", TxtCod.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Nm", TxtNombre.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Us", Session["C77U"]);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            { Mensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim()); }
                            SDR.Close();

                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result1 = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result1)
                                { Mensj = row["Texto"].ToString().Trim(); }

                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();

                            DdlBusq.Text = "0";
                            Traerdatos(DdlBusq.Text.Trim(), "UPDATE");
                            LimpiarCampos();
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void BindDBodg(string CodB, string Accion)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            DataRow[] Result;
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Bodega 2, '','','','WEB',0,0,0,@ICC,'01-1-2009','01-01-1900','01-01-1900'";
                    sqlCon.Open();
                    using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                    {
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);

                        SqlDataAdapter SDA = new SqlDataAdapter();
                        SDA.SelectCommand = SC;
                        SDA.Fill(DTDet);
                        ViewState["DTDet"] = DTDet;
                    }
                }
            }
            DTDet = (DataTable)ViewState["DTDet"];
            DataTable DT = new DataTable();
            DT = DTDet.Clone();
            Result = DTDet.Select("CodBodega = '" + CodB.Trim() + "'");
            foreach (DataRow DR in Result)
            {
                DT.ImportRow(DR);
            }
            if (DT.Rows.Count > 0)
            {
                DataView DV = DT.DefaultView;
                DV.Sort = "Columna,Fila";
                DT = DV.ToTable();
                GrdDetalle.DataSource = DT;
                GrdDetalle.DataBind();
            }
            else
            {
                DT.Rows.Add(DT.NewRow());
                GrdDetalle.DataSource = DT;
                GrdDetalle.DataBind();
                GrdDetalle.Rows[0].Cells.Clear();
                GrdDetalle.Rows[0].Cells.Add(new TableCell());
                Result = Idioma.Select("Objeto= 'SinRegistros'");
                foreach (DataRow row in Result)
                { GrdDetalle.Rows[0].Cells[0].Text = row["Texto"].ToString(); }
                GrdDetalle.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void GrdDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (DdlBusq.Text.Equals("0"))
            { return; }
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.CommandName.Equals("AddNew"))
            {
                string VbF, VbC, VBQuery;
                VbF = (GrdDetalle.FooterRow.FindControl("TxtFilaPP") as TextBox).Text.Trim();
                if (VbF == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens05Bodg'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la fila.
                    return;
                }
                VbC = (GrdDetalle.FooterRow.FindControl("TxtColumPP") as TextBox).Text.Trim();
                if (VbC == String.Empty)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'Mens06Bodg'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la columna.
                    return;
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    using (SqlTransaction Transac = sqlCon.BeginTransaction())
                    {
                        VBQuery = "EXEC SP_TablasGeneral 8,@F,@C,@US,@CB,'','','','TblUbicBod','INSERT',@ICC,@Act,@ID,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                        {
                            SC.Parameters.AddWithValue("@F", VbF.ToUpper());
                            SC.Parameters.AddWithValue("@C", VbC.ToUpper());
                            SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                            SC.Parameters.AddWithValue("@CB", TxtCod.Text.Trim());
                            SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                            SC.Parameters.AddWithValue("@Act", (GrdDetalle.FooterRow.FindControl("CkbActPP") as CheckBox).Checked == false ? 0 : 1);
                            SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                            try
                            {
                                var Mensj = SC.ExecuteScalar();
                                if (!Mensj.ToString().Trim().Equals(""))
                                {
                                    DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                    foreach (DataRow row in Result)
                                    { Mensj = row["Texto"].ToString().Trim(); }
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                    Transac.Rollback();
                                    return;
                                }
                                Transac.Commit();
                                BindDBodg(TxtCod.Text, "UPD");
                            }
                            catch (Exception ex)
                            {
                                Transac.Rollback();
                                DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                                foreach (DataRow row in Result)
                                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                                Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                            }
                        }
                    }
                }
            }
        }
        protected void GrdDetalle_RowEditing(object sender, GridViewEditEventArgs e)
        { GrdDetalle.EditIndex = e.NewEditIndex; BindDBodg(TxtCod.Text, "SEL"); }
        protected void GrdDetalle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VblId = GrdDetalle.DataKeys[e.RowIndex].Value.ToString();
            string VbF, VbC, VBQuery;
            VbF = (GrdDetalle.Rows[e.RowIndex].FindControl("TxtFila") as TextBox).Text.Trim();
            if (VbF == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens05Bodg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la fila.
                return;
            }
            VbC = (GrdDetalle.Rows[e.RowIndex].FindControl("TxtColum") as TextBox).Text.Trim();
            if (VbC == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens06Bodg'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar la columna.
                return;
            }

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 8,@F,@C,@US,@CB,@IdDet,'','','TblUbicBod','UPDATE',0,@Act,@ID,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@F", VbF.ToUpper());
                        SC.Parameters.AddWithValue("@C", VbC.ToUpper());
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@CB", TxtCod.Text.Trim());
                        SC.Parameters.AddWithValue("@ID", DdlBusq.Text.Trim().ToUpper());
                        SC.Parameters.AddWithValue("@Act", (GrdDetalle.Rows[e.RowIndex].FindControl("CkbAct") as CheckBox).Checked == false ? 0 : 1);
                        SC.Parameters.AddWithValue("@IdDet", VblId);
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            GrdDetalle.EditIndex = -1;
                            BindDBodg(TxtCod.Text.Trim(), "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDetalle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { GrdDetalle.EditIndex = -1; BindDBodg(TxtCod.Text, "SEL"); }
        protected void GrdDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            string VBQuery;
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();

                using (SqlTransaction Transac = sqlCon.BeginTransaction())
                {
                    VBQuery = "EXEC SP_TablasGeneral 8,'','',@US,@CB,@IdDet,'','','TblUbicBod','DELETE',0,0,0,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon, Transac))
                    {
                        SC.Parameters.AddWithValue("@US", Session["C77U"].ToString());
                        SC.Parameters.AddWithValue("@IdDet", GrdDetalle.DataKeys[e.RowIndex].Value.ToString());
                        SC.Parameters.AddWithValue("@CB", GrdDetalle.DataKeys[e.RowIndex].Values["CodBodega"].ToString());
                        SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                        try
                        {
                            var Mensj = SC.ExecuteScalar();
                            if (!Mensj.ToString().Trim().Equals(""))
                            {
                                DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                                foreach (DataRow row in Result)
                                { Mensj = row["Texto"].ToString().Trim(); }
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Mensj + "');", true);
                                Transac.Rollback();
                                return;
                            }
                            Transac.Commit();
                            BindDBodg(TxtCod.Text.Trim(), "UPD");
                        }
                        catch (Exception ex)
                        {
                            Transac.Rollback();
                            DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso')", true);
                            Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            PerfilesGrid();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton IbtAddNew = (e.Row.FindControl("IbtAddNew") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtAddNew'");
                foreach (DataRow row in Result)
                { IbtAddNew.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ImageButton IbtUpdate = (e.Row.FindControl("IbtUpdate") as ImageButton);
                DataRow[] Result = Idioma.Select("Objeto= 'IbtUpdate'");
                foreach (DataRow row in Result)
                { IbtUpdate.ToolTip = row["Texto"].ToString().Trim(); }
                ImageButton IbtCancel = (e.Row.FindControl("IbtCancel") as ImageButton);
                Result = Idioma.Select("Objeto= 'IbtCancel'");
                foreach (DataRow row in Result)
                { IbtCancel.ToolTip = row["Texto"].ToString().Trim(); }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgE = e.Row.FindControl("IbtEdit") as ImageButton;
                if (imgE != null)
                {
                    imgE.Enabled = true;
                    DataRow[] Result = Idioma.Select("Objeto='IbtEdit'");
                    foreach (DataRow RowIdioma in Result)
                    { imgE.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                }

                ImageButton imgD = e.Row.FindControl("IbtDelete") as ImageButton;
                if (imgD != null)
                {
                    DataRow[] Result = Idioma.Select("Objeto='IbtDelete'");
                    foreach (DataRow RowIdioma in Result)
                    { imgD.ToolTip = RowIdioma["Texto"].ToString().Trim(); }
                    Result = Idioma.Select("Objeto= 'IbtDeleteOnClick'");
                    foreach (DataRow row in Result)
                    { imgD.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }
                }
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        protected void GrdDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { GrdDetalle.PageIndex = e.NewPageIndex; BindDBodg(TxtCod.Text, "SEL"); }
    }
}