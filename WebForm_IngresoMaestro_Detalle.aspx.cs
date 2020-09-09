using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;
using _77NeoWeb.Prg;

namespace _77NeoWeb
{
    public partial class WebForm_IngresoMaestro_Detalle : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable DTHj = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbConfigWeb";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";/*   */
            }
            if (!IsPostBack)
            {
                if (ViewState["TablaDet"] == null)
                { CrearStructuraTabla("NEW"); }
                BindDHijo();
            }
        }
        protected void BindDHijo()
        {

            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                DTHj.Rows.Clear();
                string VbTxtSql = string.Format("exec Borrar2 {0}", TxtId.Text.Trim().Equals("") ? "0" : TxtId.Text);
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter(VbTxtSql, sqlCon);
                sqlDa.Fill(DTHj);
            }
            if (DTHj.Rows.Count > 0)
            {
                GrdHijo.DataSource = DTHj;
                GrdHijo.DataBind();
                ViewState["TablaDet"] = DTHj;
            }
            else
            {
                CrearStructuraTabla("Asingar");
            }
        }
        protected void CrearStructuraTabla(string Tipo)
        {
            if (Tipo.ToString().Equals("NEW"))
            {
                DTHj.Columns.Add("IdDet", typeof(int));
                DTHj.Columns.Add("IdEnc", typeof(int));
                DTHj.Columns.Add("NomHijos", typeof(string));
                DTHj.Columns.Add("Edad", typeof(int));
            }
            DTHj.Rows.Add(-1, 0, "Sin Datos...", 0);
            ViewState["TablaDet"] = DTHj;
            GrdHijo.DataSource = DTHj;
            GrdHijo.DataBind();
        }
        protected void RefrescarTabla()
        {
            DTHj = (DataTable)ViewState["TablaDet"];
            DTHj.Rows.Clear();
            try
            {
                foreach (GridViewRow Row in GrdHijo.Rows)
                {
                    int IdDet = Convert.ToInt32(GrdHijo.DataKeys[Row.RowIndex].Values[0].ToString());
                    TextBox TxtNomHijoPP = Row.FindControl("TxtNomHijoP") as TextBox;
                    TextBox TxtEdadPP = Row.FindControl("TxtEdadP") as TextBox;
                    if (IdDet >= 0)
                    {
                        DTHj.Rows.Add(IdDet, TxtId.Text, TxtNomHijoPP.Text, TxtEdadPP.Text);
                    }
                }
                ViewState["TablaDet"] = DTHj;
            }
            catch (Exception Ex)
            {
                string vble = Ex.ToString();
            }

        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        {
            string StrITT;
            double VbITT;
            StrITT = TxtId.Text.Trim().Equals("") ? "0" : TxtId.Text.Trim();
            Cnx.RetirarPuntos(StrITT);
            StrITT = Cnx.ValorDecimal();
            TxtId.Text = StrITT;

            /* Cnx.SelecBD();
             using (SqlConnection Cnx2 = new SqlConnection(Cnx.GetConex()))
             {
                 Cnx2.Open();
                 string LtxtSql = string.Format("SELECT * FROM TblEncabezado WHERE IDEnc={0}", TxtId.Text);
                 SqlCommand SqlC = new SqlCommand(LtxtSql, Cnx2);
                 SqlDataReader SDR = SqlC.ExecuteReader();
                 if (SDR.Read())
                 {
                     TxtMadre.Text = HttpUtility.HtmlDecode(SDR["NomMama"].ToString().Trim());
                     txtPadre.Text = HttpUtility.HtmlDecode(SDR["NomPapa"].ToString().Trim());
                 }
                 BindDHijo();
             }*/
        }
        protected void BtnHabilitar_Click(object sender, EventArgs e)
        {
            TxtMadre.Enabled = true;
            txtPadre.Enabled = true;
            BtnNuevo.Enabled = true;
            BtnEdit.Enabled = true;
            BindDHijo();

        }
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                List<ClsFamily> ObjTypEnc = new List<ClsFamily>();
                var detail = new ClsFamily()
                {
                    IDEnc = 0,
                    NomMama = TxtMadre.Text,
                    NomPapa = txtPadre.Text,
                    CRUD = "INSERT",
                };
                ObjTypEnc.Add(detail);

                List<ClsFamily> ObjTypDet = new List<ClsFamily>();
                var DetailD = new ClsFamily()
                {
                    IdDet = 0,
                    NomHijos = "MAO",
                    Edad = 49,
                };
                ObjTypDet.Add(DetailD);

                ClsFamily TblEnc = new ClsFamily();

                TblEnc.Alimentar(ObjTypEnc, ObjTypDet);
                TxtMadre.Enabled = false;
                txtPadre.Enabled = false;
                BtnNuevo.Enabled = false;
                BtnEdit.Enabled = false;
                BindDHijo();

            }
        }
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            RefrescarTabla();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                List<ClsFamily> ObjTypEnc = new List<ClsFamily>();
                var detail = new ClsFamily()
                {
                    IDEnc = Convert.ToInt32(TxtId.Text),
                    NomMama = TxtMadre.Text,
                    NomPapa = txtPadre.Text,
                    CRUD = "UPDATE",
                };
                ObjTypEnc.Add(detail);

                List<ClsFamily> ObjTypDet = new List<ClsFamily>();
                foreach (GridViewRow Row in GrdHijo.Rows)
                {
                    TextBox TxtNomHijoPP = Row.FindControl("TxtNomHijoP") as TextBox;
                    TextBox TxtEdadPP = Row.FindControl("TxtEdadP") as TextBox;
                    var DetailD = new ClsFamily()
                    {
                        IdDet = Convert.ToInt32(GrdHijo.DataKeys[Row.RowIndex].Values[0].ToString()),
                        NomHijos = TxtNomHijoPP.Text.Trim(),
                        Edad = Convert.ToInt32(TxtEdadPP.Text),
                    };
                    ObjTypDet.Add(DetailD);
                }
                ClsFamily TblEnc = new ClsFamily();

                TblEnc.Alimentar(ObjTypEnc, ObjTypDet);
                TxtMadre.Enabled = false;
                txtPadre.Enabled = false;
                BtnNuevo.Enabled = false;
                BtnEdit.Enabled = false;
                BindDHijo();

            }
        }
        protected void GrdHijo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                RefrescarTabla();
                try
                {

                    TextBox TxtNomHijoPP = GrdHijo.FooterRow.FindControl("TxtNomHijoPP") as TextBox;
                    TextBox TxtEdadPP = GrdHijo.FooterRow.FindControl("TxtEdadPP") as TextBox;


                    DTHj.Rows.Add(0, TxtId.Text, TxtNomHijoPP.Text, TxtEdadPP.Text);
                    GrdHijo.DataSource = DTHj;
                    GrdHijo.DataBind();
                }
                catch (Exception Ex)
                {
                    string Mensj = Ex.ToString();
                    Response.Write(Mensj);
                }
            }
        }
        protected void GrdHijo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }
        protected void GrdHijo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            
            if (e.Row.RowType == DataControlRowType.DataRow)

            {

                TextBox TxtNomHijoP = (e.Row.FindControl("TxtNomHijoP") as TextBox);
                TextBox TxtEdadP = (e.Row.FindControl("TxtEdadP") as TextBox);
                ImageButton imgD = (e.Row.FindControl("IbtDelete") as ImageButton);
                if (TxtMadre.Enabled == true)
                {
                    TxtNomHijoP.Enabled = true;
                    TxtEdadP.Enabled = true;
                    if (imgD != null)
                    {
                        imgD.Enabled = false;
                        imgD.ToolTip = "No esposible eliminar en modo edición";
                    }
                }
                else
                {
                    TxtNomHijoP.Enabled = false;
                    TxtEdadP.Enabled = false;
                    if (imgD != null)
                    {
                        imgD.Enabled = true;
                        imgD.ToolTip = "";
                    }
                }
            }
        }
    }
}