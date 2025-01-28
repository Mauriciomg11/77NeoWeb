using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace _77NeoWeb.Forms.Configuracion.MaestIngPrg
{
    public partial class FrmContador : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataSet DSTDdl = new DataSet();
        string VbCont, VbDescCn, VbUMCnt, VbIdenCnt;
        int VbResetCnt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Contador");
            if (Session["C77U"] == null)
            {
                ViewState["VldrCntdr"] = "S";
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas(); //"admindemp";
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                }
            }
            if (!IsPostBack)
            {
                TitForm.Text = "Configuración de Contadores";
                ViewState["Accion"] = "";
                ModSeguridad();
                ActivarCampos(false, false);
                BindDataDdlCntr("UPD");
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;
            if (!Session["C77U"].ToString().Trim().Equals("00000082")) { BtnIngresar.Visible = false; BtnModificar.Visible = false; BtnEliminar.Visible = false; }

            ClsPermisos ClsP = new ClsPermisos();
            string VbPC = Cnx.GetIpPubl();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmContador.aspx", VbPC);

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
                BtnIngresar.Visible = false;
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

            }
            if (ClsP.GetEliminar() == 0)
            {
                ViewState["VblEliMS"] = 0;
                BtnEliminar.Visible = false;
            }
            if (ClsP.GetCE1() == 0)
            {
                IbtExpExcel.Visible = false;
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
                SC.Parameters.AddWithValue("@F1", "FRMCONTADORELEM");
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

                    TitForm.Text = bO.Equals("TituloConfCont") ? bT : TitForm.Text;
                    LblConsultar.Text = bO.Equals("LblConsultar") ? bT + ":" : LblConsultar.Text;
                    IbtExpExcel.ToolTip = bO.Equals("IbtExpExcel") ? bT : IbtExpExcel.ToolTip;
                    LblCodigo.Text = bO.Equals("lblCodigo") ? bT + ":" : LblCodigo.Text;
                    LblDescrip.Text = bO.Equals("LblDescrip") ? bT + ":" : LblDescrip.Text;
                    LblUndMed.Text = bO.Equals("LblUndMed") ? bT + ":" : LblUndMed.Text;
                    LblIdentif.Text = bO.Equals("LblIdentif") ? bT + ":" : LblIdentif.Text;
                    CkReset.Text = bO.Equals("CkReset") ? "&nbsp" + bT : CkReset.Text;
                    BtnIngresar.Text = bO.Equals("BotonIng") ? bT : BtnIngresar.Text;
                    BtnModificar.Text = bO.Equals("BotonMod") ? bT : BtnModificar.Text;
                    BtnEliminar.Text = bO.Equals("BtnEliminar") ? bT : BtnEliminar.Text;
                }
                DataRow[] Result = Idioma.Select("Objeto= 'MensConfEli'");
                foreach (DataRow row in Result)
                { BtnEliminar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea eliminar el registro?  

                sqlCon.Close();
                ViewState["TablaIdioma"] = Idioma;
            }
        }
        protected void BindDataDdlCntr(string Accion)
        {
            if (Accion.Equals("UPD"))
            {
                Cnx.SelecBD();
                using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
                {
                    string VbTxtSql = "EXEC SP_PANTALLA_Tipo_contador 9,'','','','','',0,0,@Idm,@ICC,'01-01-1','02-01-1','03-01-1'";
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
                                SDA.Fill(DSTDdl);
                                DSTDdl.Tables[0].TableName = "Contador";
                                DSTDdl.Tables[1].TableName = "UndMed";
                                DSTDdl.Tables[2].TableName = "Identificdr";
                                ViewState["DSDdl"] = DSTDdl;
                            }
                        }
                    }
                }
            }
            DSTDdl = (DataSet)ViewState["DSDdl"];

            string VbCodAnt = DdlBuscar.Text.Trim();
            DdlBuscar.DataSource = DSTDdl.Tables[0];
            DdlBuscar.DataTextField = "Descripcion";
            DdlBuscar.DataValueField = "CodContador";
            DdlBuscar.DataBind();
            DdlBuscar.Text = VbCodAnt;

            VbCodAnt = DdlUndMed.Text.Trim();
            DdlUndMed.DataSource = DSTDdl.Tables[1];
            DdlUndMed.DataTextField = "Descripcion";
            DdlUndMed.DataValueField = "CodUnidMedida";
            DdlUndMed.DataBind();
            DdlUndMed.Text = VbCodAnt;

            VbCodAnt = DdlIdent.Text.Trim();
            DdlIdent.DataSource = DSTDdl.Tables[2];
            DdlIdent.DataTextField = "Descripcion";
            DdlIdent.DataValueField = "IdentificadorC";
            DdlIdent.DataBind();
            DdlIdent.Text = VbCodAnt;
        }
        protected void LimpiarCampos()
        {
            TxtCod.Text = "";
            TxtDesc.Text = "";
            DdlUndMed.Text = "";
            DdlIdent.Text = "";
            CkReset.Checked = false;
        }
        protected void ActivarCampos(bool Ing, bool Edi)
        {
            TxtCod.Enabled = Ing;
            TxtDesc.Enabled = Edi;
            DdlUndMed.Enabled = Edi;
            DdlIdent.Enabled = Edi;
            CkReset.Enabled = Edi;
        }
        protected void ActivarBotones(bool In, bool Md, bool El, bool Ip, bool Fml)
        {
            BtnIngresar.Enabled = In;
            BtnModificar.Enabled = Md;
            BtnEliminar.Enabled = El;
            IbtExpExcel.Enabled = Ip;
        }
        protected void AsignarValores()
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            ViewState["VldrCntdr"] = "S";
            if (TxtCod.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens01Cntdr'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//
                ViewState["VldrCntdr"] = "N";
                return;
            }
            if (DdlUndMed.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens02Cntdr'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar una unidad de medida')", true);
                ViewState["VldrCntdr"] = "N";
                return;
            }
            if (DdlIdent.Text == String.Empty)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'Mens03Cntdr'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Debe ingresar un identificador')", true);
                ViewState["VldrCntdr"] = "N";
                return;
            }
            VbCont = TxtCod.Text.Trim();
            VbDescCn = TxtDesc.Text.Trim();
            VbUMCnt = DdlUndMed.SelectedValue;
            VbIdenCnt = DdlIdent.SelectedValue;
            VbResetCnt = 0;
            if (CkReset.Checked == true) { VbResetCnt = 1; }
        }
        protected void DdlBuscar_TextChanged(object sender, EventArgs e)
        {
            BusqNewReg(DdlBuscar.SelectedValue);
            ActivarBotones(true, true, true, true, true);
        }
        protected void BusqNewReg(string VbNewCod)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                while (VbNewCod.Contains(" "))
                {
                    VbNewCod = VbNewCod.Replace(" ", string.Empty);

                }
                try
                {
                    string LtxtSql = "EXEC SP_PANTALLA_Tipo_contador 7,'" + VbNewCod + "','','','','',0,0,@Idm,0,'01-01-1','02-01-1','03-01-1'";
                    using (SqlCommand sqlCmd = new SqlCommand(LtxtSql, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        sqlCon.Open();
                        SqlDataReader tbl = sqlCmd.ExecuteReader();
                        if (tbl.Read())
                        {
                            TxtCod.Text = tbl["CodContador"].ToString().Trim();
                            TxtDesc.Text = tbl["Nombre"].ToString().Trim();
                            DdlUndMed.Text = tbl["CodUnidMedida"].ToString().Trim();
                            DdlIdent.Text = tbl["identificador"].ToString().Trim();
                            CkReset.Checked = false;
                            if (Convert.ToInt32(tbl["Reseteable"].ToString()) == 1)
                            {
                                CkReset.Checked = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensIncovCons'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//

                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), ViewState["PFileName"].ToString().Trim(), "Consultar Contador", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            string VbNomRpt = "";
            DataRow[] Result = Idioma.Select("Objeto= 'TitExportar'");
            foreach (DataRow row in Result)
            { VbNomRpt = row["Texto"].ToString().Trim(); }
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CurExportContador", Session["77IDM"].ToString().Trim());
            string VbTxtSql = "EXEC SP_PANTALLA_Tipo_contador 8,'','','','','CurExportContador',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
            Cnx.SelecBD();
            using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
            {
                using (SqlCommand SC = new SqlCommand(VbTxtSql, con))
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
        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            string Txtsql;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["Accion"].ToString().Trim().Equals(""))
            {
                ViewState["Accion"] = "Aceptar";
                DdlBuscar.Enabled = false;
                ActivarBotones(true, false, false, false, false);

                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//Aceptar
                ActivarCampos(true, true);
                LimpiarCampos();
                Result = Idioma.Select("Objeto= 'MensConfIng'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnIngresar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar el ingreso? 
            }
            else
            {
                try
                {
                    AsignarValores();
                    if (ViewState["VldrCntdr"].ToString() == "N") { return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        string TxQry = "EXEC SP_Pantalla_Parametros 1,'" + VbCont + "','','C','CodContador','TblContador',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand Comando = new SqlCommand(TxQry, sqlCon);
                        sqlCon.Open();
                        SqlDataReader registro = Comando.ExecuteReader();
                        if (registro.Read())
                        {
                            DataRow[] Result = Idioma.Select("Objeto= 'Mens04Cntdr'");
                            foreach (DataRow row in Result)
                            { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//El código ya se encuentra asignado')", true);
                            return;
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        Txtsql = "EXEC SP_TablasPlantillaM 8,@Cod,@Desc, @UM, @Idnt, @UsuC,'','','','',@Rst,0,0,0,@Idm,0,'01-01-1','02-01-1','03-01-1'	";
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCont.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDescCn.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@UM", VbUMCnt.ToString());
                        sqlCmd.Parameters.AddWithValue("@Idnt", VbIdenCnt);
                        sqlCmd.Parameters.AddWithValue("@Rst", VbResetCnt);
                        sqlCmd.Parameters.AddWithValue("@Idm", Session["77IDM"]);

                        sqlCmd.ExecuteNonQuery();
                        LimpiarCampos();
                        DataRow[] Result = Idioma.Select("Objeto= 'BotonIng'");
                        foreach (DataRow row in Result)
                        { BtnIngresar.Text = row["Texto"].ToString().Trim(); }//
                        ViewState["Accion"] = "";
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, false);
                        DdlBuscar.Enabled = true;
                        BtnIngresar.OnClientClick = "";
                        BindDataDdlCntr("UPD");
                    }
                    BusqNewReg(VbCont);
                }
                catch (Exception ex)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrIng'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el ingreso de los dato')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmContador", "INSERT", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["Accion"].ToString().Trim().Equals(""))
            {
                ViewState["Accion"] = "Aceptar";
                ActivarBotones(false, true, false, false, false);
                DataRow[] Result = Idioma.Select("Objeto= 'BotonIngOk'");
                foreach (DataRow row in Result)
                { BtnModificar.Text = row["Texto"].ToString().Trim(); }//Aceptar
                ActivarCampos(false, true);
                DdlBuscar.Enabled = false;
                Result = Idioma.Select("Objeto= 'MensConfMod'"); //MensConfIng |MensConfMod
                foreach (DataRow row in Result)
                { BtnModificar.OnClientClick = string.Format("return confirm('" + row["Texto"].ToString().Trim() + "');"); }//¿Desea realizar la actualización?');";
            }
            else
            {
                try
                {
                    AsignarValores();
                    if (ViewState["VldrCntdr"].ToString() == "N") { return; }
                    Cnx.SelecBD();
                    using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                    {
                        sqlCon.Open();
                        string Txtsql = "EXEC SP_TablasPlantillaM 9,@Cod,@Desc, @UM, @Idnt, @UsuC,'','','','',@Rst,0,0,0,@Idm,0,'01-01-1','02-01-1','03-01-1'";
                        SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Cod", VbCont.ToString());
                        sqlCmd.Parameters.AddWithValue("@Desc", VbDescCn.ToString());
                        sqlCmd.Parameters.AddWithValue("@UsuC", Session["C77U"].ToString());
                        sqlCmd.Parameters.AddWithValue("@UM", VbUMCnt.ToString());
                        sqlCmd.Parameters.AddWithValue("@Idnt", VbIdenCnt);
                        sqlCmd.Parameters.AddWithValue("@Rst", VbResetCnt);
                        sqlCmd.Parameters.AddWithValue("@Idm", Session["77IDM"]);
                        sqlCmd.ExecuteNonQuery();
                        ViewState["Accion"] = "";
                        DataRow[] Result = Idioma.Select("Objeto= 'BotonMod'");
                        foreach (DataRow row in Result)
                        { BtnModificar.Text = row["Texto"].ToString().Trim(); }//
                        ActivarBotones(true, false, false, false, false);
                        ActivarCampos(false, false);
                        DdlBuscar.Enabled = true;
                        BtnModificar.OnClientClick = "";
                        LimpiarCampos();
                        BindDataDdlCntr("UPD");
                    }
                    BusqNewReg(VbCont);
                }

                catch (Exception ex)
                {
                    DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                    foreach (DataRow row in Result)
                    { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en la edición de los datos')", true);
                    Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmContador", "UPDATE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                }
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            Idioma = (DataTable)ViewState["TablaIdioma"];
            try
            {

                string VBQuery, VbCod;
                VbCod = TxtCod.Text;

                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    VBQuery = "EXEC SP_Pantalla_Parametros 11,'" + VbCod + "','','','','VALIDA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand Comando = new SqlCommand(VBQuery, sqlCon);
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        string Mensj = registro["Mensj"].ToString().Trim();
                        DataRow[] Result = Idioma.Select("Objeto= '" + Mensj.ToString().Trim() + "'");
                        foreach (DataRow row in Result)
                        { Mensj = row["Texto"].ToString().Trim(); }

                        ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('" + Mensj + "')", true);
                        return;
                    }
                }
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    sqlCon.Open();
                    string query = "EXEC SP_Pantalla_Parametros 11, " + VbCod + ",'','','','ELIMINA',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.ExecuteNonQuery();
                    LimpiarCampos();
                    ActivarBotones(true, false, false, false, false);
                    BindDataDdlCntr("UPD");
                }
            }
            catch (Exception ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrEli'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + row["Texto"].ToString() + "');", true); }//Error en el proceso de eliminación')", true);
                Cnx.UpdateErrorV2(Session["C77U"].ToString(), "FrmContador", "DELETE", ex.StackTrace.Substring(ex.StackTrace.Length - 300, 300), ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
            }
        }
    }
}