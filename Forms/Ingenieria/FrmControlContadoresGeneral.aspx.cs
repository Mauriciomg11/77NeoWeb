using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb.Forms.Ingenieria
{
    public partial class FrmControlContadoresGeneral : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (Session["Login77"] == null)
            {
                Response.Redirect("~/FrmAcceso.aspx");
            } */
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                Session["Nit77Cia"] = "811035879-1"; // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["77IDM"] = "5"; // 4 español | 5 ingles  /**/
            }
            if (!IsPostBack)
            {
                Page.Title = "Procesos";
                TitForm.Text = "Procesos de ingenieria";
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddl();</script>", false);
        }
        protected void ColorBtns(string Pos)
        {
            BtnProceLibrV.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusExceso.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusDefect.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusDefectMyr.CssClass = "btn btn-outline-primary BotonesPpal";
            BtnAjusConve.CssClass = "btn btn-outline-primary BotonesPpal";
            switch (Pos)
            {
                case "1":
                    BtnProceLibrV.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "2":
                    BtnAjusExceso.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "3":
                    BtnAjusDefect.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "4":
                    BtnAjusDefectMyr.CssClass = "btn btn-info BotonesPpal";
                    break;
                case "5":
                    BtnAjusConve.CssClass = "btn btn-info BotonesPpal";
                    break;
            }
        }       
        protected void BtnProceLibrV_Click(object sender, EventArgs e)
        {
            ColorBtns("1");
            ListBoXLibrosSinProc();
            MlVPI.ActiveViewIndex = 0;
        }
        protected void BtnAjusExceso_Click(object sender, EventArgs e)
        {
            ColorBtns("2");
        }

        protected void BtnAjusDefect_Click(object sender, EventArgs e)
        {
            ColorBtns("3");
        }

        protected void BtnAjusDefectMyr_Click(object sender, EventArgs e)
        {
            ColorBtns("4");
        }

        protected void BtnAjusConve_Click(object sender, EventArgs e)
        {
            ColorBtns("5");
        }

        protected void ListBoXLibrosSinProc()
        {
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string LtxtSql = "EXEC SP_PANTALLA_Proceso_Ingenieria 3,'','','','',0,0,0,0,'01-1-2009','01-01-1900','01-01-1900'";
                SqlCommand Cm = new SqlCommand(LtxtSql, sqlCon);
                sqlCon.Open();
                SqlDataReader Tbl = Cm.ExecuteReader();
                LbxLibrosSinProc.Items.Clear();
                while (Tbl.Read())
                {
                    LbxLibrosSinProc.Items.Add(Tbl[0].ToString());
                }
            }
        }
        protected void LbxLibrosSinProc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBDdlAK();
        }
        protected void BindBDdlAK()
        {
            string LtxtSql = string.Format("EXEC SP_PANTALLA_Proceso_Ingenieria 4,'','','','',0,0,0,0,'{0}','01-01-1900','01-01-1900'", LbxLibrosSinProc.SelectedValue);
            DdlCorrContHK.DataSource = Cnx.DSET(LtxtSql);
            DdlCorrContHK.DataMember = "Datos";
            DdlCorrContHK.DataTextField = "Matricula";
            DdlCorrContHK.DataValueField = "CodAeronave";
            DdlCorrContHK.DataBind();
        }
        protected void DdlCorrContHK_TextChanged(object sender, EventArgs e)
        {

        }
    }
}