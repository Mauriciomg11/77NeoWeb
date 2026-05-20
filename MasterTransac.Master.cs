using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using _77NeoWeb.Prg;
using System.Threading;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using System.IO;

namespace _77NeoWeb
{
    public partial class MasterTransac : System.Web.UI.MasterPage
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        DataTable IdiomaAll = new DataTable();
        DataTable DTAlrtNewRva = new DataTable();
        DataSet DSAlrtOD = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Response.Cache.SetCacheability(HttpCacheability.NoCache); // Evitar cache
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1)); // Expirar cache inmediatamente
            Response.Cache.SetNoStore(); // No guardar nada en el cache*/
            LblCia.Text = Session["SigCia"].ToString() + " - " + Session["N77U"].ToString();
            //IdiomaControles();
            LoadMenu();
            if (Session["77IDM"].ToString() == "4")
            {
                LkbMenu.Text = "Menú";
                LkbCambPass.Text = "Cambio Contraseña...";
            }
            else
            {
                LkbMenu.Text = "Menu";
                LkbCambPass.Text = "Change Password";
            }
        }
        protected void IdiomaControles()
        {
            IdiomaAll = (DataTable)Session["TblIdmGrl"];
            DataRow[] DR = IdiomaAll.Select("CodF='0'");
            if (Cnx.ValidaDataRowVacio(DR))
            { Idioma = DR.CopyToDataTable(); ViewState["TablaIdioma"] = Idioma; }
        }
        protected void IbnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
        }
        /// <summary>
        /// Metodo para traer el menu desde la base de datos
        ///    /// <param name="Us">codigo del usuario</param>
        /// <param name="ICC">Codigo de la empresa</param>
        /// </summary>
        private void LoadMenu()
        {
            Cnx.SelecBD();
            using (SqlConnection conn = new SqlConnection(Cnx.GetConex()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("EXEC SP_Menu @Us,@ICC,@Id", conn);
                cmd.Parameters.AddWithValue("@Us", Session["C77U"]);
                cmd.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                cmd.Parameters.AddWithValue("@Id", Session["77IDM"]);

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable menuTable = new DataTable();
                menuTable.Load(reader);
                StringBuilder menuHtml = new StringBuilder();

                //menuHtml.Append("<div id='menu2' >"); 
                foreach (DataRow row in menuTable.Rows)
                {

                    if (row[0].ToString() == row[2].ToString())
                    {
                        menuHtml.Append("<ul class='list-group'>");
                        menuHtml.Append($"<li class='list-group-item'><a class='dropdown-toggle'  data-toggle='collapse' aria-expanded='false' href='{row["RutaWeb"]}' >{row["Descripcion"]}</a> ");

                        MenuItem miMenuItem = new MenuItem(Convert.ToString(row[1]), Convert.ToString(row[0]), string.Empty, Convert.ToString(row[6]));
                        //MyMenu.Items.Add(miMenuItem);
                        LoadSubMenu(ref miMenuItem, menuTable, menuHtml);
                        menuHtml.Append(" </li></ul>");
                    }

                }

                // menuHtml.Append("</div>");
                menuLiteral.Text = menuHtml.ToString();

            }
        }
        private void LoadSubMenu(ref MenuItem miMenuItem, DataTable menuTable, StringBuilder menuHtml)
        {
            // menuHtml.Append("<ul class='submenu'>");
            foreach (DataRow subItems in menuTable.Rows)
            {

                if (subItems[0].ToString() != subItems[2].ToString() && subItems[2].ToString() == miMenuItem.Value.ToString())
                {
                    menuHtml.Append("<ul class='collapse'>");
                    menuHtml.Append($"<li class='dropdown-toggle'><a class='dropdown-toggle'  href='{subItems[6]}' >{subItems[1]}</a></li>");
                    MenuItem menuChild = new MenuItem(subItems[1].ToString(), subItems[0].ToString(), string.Empty, Convert.ToString(subItems[6]));
                    menuChild.ChildItems.Add(menuChild);
                    LoadSubMenu(ref menuChild, menuTable, menuHtml);
                    menuHtml.Append("</ul>");
                }

            }
            // menuHtml.Append("</ul>");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void IbnSalir_Click(object sender, EventArgs e)
        {
            Session["Login77"] = null;
            Session["D[BX"] = "";
            Session["Nit77Cia"] = "";
            Session["$VR"] = "";
            // Session["V$U@"] = "";
            // Session["P@$"] = "";
            //   Session["SigCia"] = "";
            ///System.Web.Security.FormsAuthentication.SignOut();
            //   Session.Abandon();

            Response.Redirect("~/FrmAcceso.aspx");
        }
        protected void LkbCambPass_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Forms/Seguridad/FrmCambioPass.aspx");
        }
        protected void LkbMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FrmMenu.aspx");
        }
        public void SetObjMensj(string S_Mensj)
        {
            IdiomaAll = (DataTable)Session["TblIdmGrl"];
            LblTitMensjMdlMstr.Text = IdiomaAll.AsEnumerable().Where(x => x.Field<string>("Objeto") == "TitMensjMdlMstr" && x.Field<string>("CodF") == "0")
                                        .Select(x => x.Field<string>("Texto")).FirstOrDefault();
            BtnCloseMdlMstr.Text = IdiomaAll.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnCerrarMst" && x.Field<string>("CodF") == "0")
                                        .Select(x => x.Field<string>("Texto")).FirstOrDefault();
            TxtMensjMstr.Text = S_Mensj;
            UpPnlMdlMensj.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#MensjModal').modal({ backdrop: 'static', keyboard: false }); $('#MensjModal').modal('show');", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#MensjModal').modal('show');", true); //Se cierra con Esc o clic por fuera 
        }
        //************************************ < Metodos de la alerta de nuevas reservas sin revisar> ************************************
        public void DatosNewRva()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlConB = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_PANTALLA_Reserva 8, @Cs,'','','',0,0,0, @ICC,'01-1-2009','01-01-1900','01-01-1900'";
                sqlConB.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlConB))
                {
                    SC.Parameters.AddWithValue("@Cs", Session["C77U"].ToString());
                    SC.Parameters.AddWithValue("@ICC", Session["!dC!@"]);
                    using (SqlDataAdapter DAB = new SqlDataAdapter())
                    {
                        DAB.SelectCommand = SC;
                        DAB.Fill(DTAlrtNewRva);
                        if (DTAlrtNewRva.Rows.Count > 0) { GrdAlrtNewRva.DataSource = DTAlrtNewRva; GrdAlrtNewRva.DataBind(); }
                        else { GrdAlrtNewRva.DataSource = null; GrdAlrtNewRva.DataBind(); }
                        ViewState["DTAlrtNewRva"] = DTAlrtNewRva;
                    }
                }
            }/**/
        }
        public void AlrtNewRva()
        {
            IdiomaAll = (DataTable)Session["TblIdmGrl"];
            DataRow[] DR = IdiomaAll.Select("CodF IN('0','FrmAlertaReservaPenRevisar')");
            if (Cnx.ValidaDataRowVacio(DR))
            { Idioma = DR.CopyToDataTable(); ViewState["TablaIdioma"] = Idioma; }
            LblTitAlrt.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "Titulo").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblTitAlerta.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblTitAlerta").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            IbnNewRvaRevd.ToolTip = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "IbnNewRvaRevd").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.EmptyDataText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "SinRegistros").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[2].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblOTMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[3].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdPos").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[4].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "ReferenciaMst").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[6].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdCantSol").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[7].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdCantEnt").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[8].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdStok").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[9].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdCantInsp").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[10].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdSP").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[11].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblUsrMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            GrdAlrtNewRva.Columns[12].HeaderText = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "GrdDate").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            BtnExpModlAlrNewRva.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnExportMstr").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            BtnCerrarAlerta.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "BtnCerrarMst").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            DatosNewRva();
            IbnNewRvaRevd.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "UniqueKey", "$('#ModalAlertNewRva').modal({ backdrop: 'static', keyboard: false }); $('#ModalAlertNewRva').modal('show');", true);
        }
        protected void BtnExpModlAlrNewRva_Click(object sender, EventArgs e)
        {
            try
            {
                DTAlrtNewRva = (DataTable)ViewState["DTAlrtNewRva"];
                string StSql, VbNomRpt = "Reserva Nueva";
                CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
                CursorIdioma.Alimentar("CurExportRsvNew", Session["77IDM"].ToString().Trim());
                StSql = "EXEC Consultas_General_Logistica 12,'','','CurExportRsvNew',0, 0,@ICC,'01-01-1','01-01-1'";
                Idioma = (DataTable)ViewState["TablaIdioma"];
                DataRow[] Result = Idioma.Select("Objeto= 'Caption'");
                foreach (DataRow row in Result) { VbNomRpt = row["Texto"].ToString().Trim(); }// 
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
                                ds.Tables[0].TableName = "XOM";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables) { wb.Worksheets.Add(dt); }
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
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = "Exportar Alerta Nueva Reserva", VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Alerta Reservas nuevas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void GrdAlrtNewRva_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                Literal litSmfro = (Literal)e.Row.FindControl("litSmfro");
                CheckBox CkbSelec = (CheckBox)e.Row.FindControl("CkbSelec");
                if (dr["Revisado"].ToString().Equals("0")) { litSmfro.Text = "<i class='bi bi-circle-fill semaforo-alerta' style='color:Orange; font-size:20px;'></i>"; }
                else { litSmfro.Text = "<i class='bi bi-circle-fill' style='color:Green; font-size:20px;'></i>"; CkbSelec.Visible = false; }
            }
        }
        protected void IbnNewRvaRevd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DTAlrtNewRva = (DataTable)ViewState["DTAlrtNewRva"];
                DataTable DTRevisada = new DataTable();
                DTRevisada.Columns.Add("V1", typeof(string));
                DTRevisada.Columns.Add("PN", typeof(string));
                DTRevisada.Columns.Add("OT", typeof(string));
                DTRevisada.Columns.Add("Rva", typeof(int));
                DTRevisada.Columns.Add("F2", typeof(int));
                DTRevisada.Columns.Add("F3", typeof(int));
                DTRevisada.Columns.Add("D1", typeof(DateTime));
                DTRevisada.Columns.Add("D2", typeof(DateTime));
                foreach (GridViewRow Row in GrdAlrtNewRva.Rows)
                {
                    CheckBox CkbSelec = Row.FindControl("CkbSelec") as CheckBox;
                    if (CkbSelec.Checked == true)
                    {
                        int IdRva = Convert.ToInt32(GrdAlrtNewRva.DataKeys[Row.RowIndex].Values["CodiddetalleRes"].ToString());
                        int IPos = Convert.ToInt32((Row.FindControl("LblPos") as Label).Text.Trim());
                        string SPn = (Row.FindControl("LblPN") as Label).Text.Trim();
                        string SOT = (Row.FindControl("LblOT") as Label).Text.Trim();
                        DTRevisada.Rows.Add("V1", SPn, SOT, IdRva, IPos, 3, null, null);
                    }
                }
                Cnx.SelecBD();
                using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
                {
                    SCX.Open();
                    using (SqlTransaction transaction = SCX.BeginTransaction())
                    {
                        string VBQuery = "Reserva_Revisada";
                        using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                        {
                            try
                            {
                                SC.CommandType = CommandType.StoredProcedure;
                                SqlParameter Prmtrs = SC.Parameters.AddWithValue("@DTRR", DTRevisada);
                                SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@IdCia", Session["!dC!@"].ToString());
                                SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@Usu", Session["C77U"].ToString());
                                Prmtrs.SqlDbType = SqlDbType.Structured;
                                SC.ExecuteNonQuery();
                                transaction.Commit();
                            }
                            catch (Exception) { transaction.Rollback(); }
                        }
                    }
                }
                DatosNewRva();
                GrdAlrtNewRva.DataSource = null; GrdAlrtNewRva.DataBind();
                IbnNewRvaRevd.Visible = false;
            }
            catch (Exception Ex)
            {
                DataRow[] Result = Idioma.Select("Objeto= 'MensErrMod'");
                foreach (DataRow row in Result)
                { ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "IdntificadorBloqueScript", "alert('" + row["Texto"].ToString() + "');", true); }
                string VbcatUs = Session["C77U"].ToString(), VbcatNArc = "Exportar Alerta Nueva Reserva", VbcatVer = Session["77Version"].ToString(), VbcatAct = Session["77Act"].ToString();
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Alerta Reservas nuevas", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        //************************************ < Metodos de la alerta Orden de Despacho> ************************************
        public void AlertOD()
        {
            string LtxtSql = string.Format("EXEC SP_Pantalla_Orden_Despacho 10,'{0}','','','',0,0,0,{1},'01-1-2009','01-01-1900','01-01-1900'", Session["C77U"], Session["!dC!@"]);

            DSAlrtOD = Cnx.DSET(LtxtSql);
            DSAlrtOD.Tables[0].TableName = "AlertOD";
            if (DSAlrtOD.Tables["AlertOD"].Rows.Count > 0)
            {
                IdiomaAll = (DataTable)Session["TblIdmGrl"];
                DataRow[] DR = IdiomaAll.Select("CodF IN('0','FrmAlertaReservaPenRevisar')");
                if (Cnx.ValidaDataRowVacio(DR))
                { Idioma = DR.CopyToDataTable(); ViewState["TablaIdioma"] = Idioma; }
                LblTitAlertODPpl.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "Titulo").Select(x => x.Field<string>("Texto")).FirstOrDefault();
                GrdAlrtNewOD.DataSource = DSAlrtOD.Tables["AlertOD"]; GrdAlrtNewOD.DataBind();
                IbnNewODRevd.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "UniqueKey", "$('#ModalAlertOD').modal({ backdrop: 'static', keyboard: false }); $('#ModalAlertOD').modal('show');", true);
            }
        }
        protected void IbnNewODRevd_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}