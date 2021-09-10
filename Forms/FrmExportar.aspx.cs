using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.EnterpriseServices;
using ClosedXML.Excel;

namespace _77NeoWeb.Forms
{
    public partial class FrmExportar : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        private string VbPrmtrAnt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";  /**/
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
                //string bnllrr = Request.QueryString["ToolT"].ToString();
                TxtBusqueda.ToolTip = Request.QueryString["ToolT"].ToString();
                LblTitulo.Text = Request.QueryString["TT"].ToString();
                ViewState["PFileName"] = "FrmExportar";
                if (Request.QueryString["TT"].ToString().Equals("Exportar Referencias"))
                {
                    Lbl1.Text = "Verificación";
                    Lbl1.Visible = true;
                    Rdb1.Checked = true;
                    Rdb1.Visible = true;
                    Lbl2.Text = "P/N con unidad de compra";
                    Lbl2.Visible = true;
                    Rdb2.Visible = true;
                }
                BindData();
            }
        }
        void BindData()
        {
            DataTable dtbl = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                switch (Request.QueryString["TT"].ToString())
                {
                    case "Exportar Referencias":
                        if (Rdb1.Checked == true)
                        {
                            VbPrmtrAnt = "EXEC SP_PANTALLA_ReferenciaV2 13,@PN,'','','','VERIFICAR',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        }
                        if (Rdb2.Checked == true)
                        {
                            VbPrmtrAnt = "EXEC SP_PANTALLA_ReferenciaV2 13,@PN,'','','','UNDCOMPRA',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        }
                        break;
                    default:
                        VbPrmtrAnt = Request.QueryString["TCDN"].ToString();
                        break;
                }
                string VbTxtSql = string.Format(VbPrmtrAnt, TxtBusqueda.Text);
                sqlCon.Open();
                SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon);
                SC.Parameters.AddWithValue("@PN", TxtBusqueda.Text);
                SqlDataAdapter SDA = new SqlDataAdapter();
                SDA.SelectCommand = SC;
                SDA.Fill(dtbl);

                if (dtbl.Rows.Count > 0)
                {
                    GrdDatos.DataSource = dtbl;
                    GrdDatos.DataBind();
                }
                else
                {
                    GrdDatos.DataSource = null;
                    GrdDatos.DataBind();
                }
            }
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
        }
        protected void IbtExpExcel_Click(object sender, ImageClickEventArgs e)
        {
            /*Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", Request.QueryString["NomArch"].ToString()));
            Response.Charset = "";
            using (StringWriter StWt = new StringWriter())
            {
                HtmlTextWriter HtmlT = new HtmlTextWriter(StWt);
                // GrdDatos.HeaderRow.Style.Add("background-color", "#0000ff");
                GrdDatos.HeaderRow.Style.Add("color", "white");
                foreach (TableCell tableCell in GrdDatos.HeaderRow.Cells)
                {
                    tableCell.Style["background-color"] = "#0000ff";
                }

                foreach (GridViewRow GrdV in GrdDatos.Rows)
                {
                    //gridViewRow.BackColor = System.Drawing.Color.White;
                    foreach (TableCell GrdCell in GrdV.Cells)
                    {
                        if (GrdV.RowIndex % 2 == 0)
                        {
                            GrdCell.Style["background-color"] = "white";
                        }
                        else
                        {
                            GrdCell.Style["background-color"] = "#cae4ff";
                        }

                    }
                }

                GrdDatos.RenderControl(HtmlT);
                Response.Write(StWt.ToString());
                Response.End();
            }*/
            try
            {
                string StSql, VbPrmtrAnt="";
                switch (Request.QueryString["TT"].ToString())
                {
                    case "Exportar Referencias":
                        if (Rdb1.Checked == true)
                        {
                            VbPrmtrAnt = "EXEC SP_PANTALLA_ReferenciaV2 13,@PN,'','','','VERIFICAR',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        }
                        if (Rdb2.Checked == true)
                        {
                            VbPrmtrAnt = "EXEC SP_PANTALLA_ReferenciaV2 13,@PN,'','','','UNDCOMPRA',0,0,0,0,'01-01-01','02-01-01','03-01-01'";
                        }
                        break;
                    default:
                        VbPrmtrAnt = Request.QueryString["TCDN"].ToString();
                        break;
                }
                StSql = string.Format(VbPrmtrAnt, TxtBusqueda.Text);
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@PN", TxtBusqueda.Text);
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SC.Connection = con;
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = "Tabla";
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                   // Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", VbNomRpt));
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", Request.QueryString["NomArch"].ToString()));
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
                Cnx.UpdateErrorV2(VbcatUs, VbcatNArc, "Exportar Excel", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
            }
        }
        protected void IbnCerrar_Click(object sender, ImageClickEventArgs e)
        {
            string VblVolver = Request.QueryString["PantI"].ToString();
            Response.Redirect(VblVolver);
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }


    }
}