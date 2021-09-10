using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Forms.Seguridad
{
    public partial class FrmConfigPantalla : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo 
            Page.Title = string.Format("Configuración_Pantalla");
            TitForm.Text = "Configuración de Pantallas";
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */
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
                ModSeguridad();
                BindData(TxtBusqueda.Text);
            }
        }
        void ModSeguridad()
        {
            ViewState["VblIngMS"] = 1;
            ViewState["VblModMS"] = 1;
            ViewState["VblEliMS"] = 1;
            ViewState["VblImpMS"] = 1;

            ClsPermisos ClsP = new ClsPermisos();
            ClsP.Acceder(Session["C77U"].ToString(), "FrmConfigPantalla.aspx");

            if (ClsP.GetAccesoFrm() == 0)
            {
                Response.Redirect("WebMenuInicio.aspx");
            }
            if (ClsP.GetIngresar() == 0)
            {
                ViewState["VblIngMS"] = 0;
            }
            if (ClsP.GetModificar() == 0)
            {
                ViewState["VblModMS"] = 0;
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
            }
            if (ClsP.GetCE1() == 0)
            {
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
        }
        void BindData(string VbConsultar)
        {
            DataTable dtbl = new DataTable();
            Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_ConfiguracionV2_ 12,'" + VbConsultar + "','','','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";

                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter(VbTxtSql, sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                GrdDatos.DataSource = dtbl;
                GrdDatos.DataBind();
                GrdDatos.Rows[0].Cells.Clear();
                GrdDatos.Rows[0].Cells.Add(new TableCell());
                GrdDatos.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                GrdDatos.Rows[0].Cells[0].Text = "No existen registros ..!";
                GrdDatos.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        void ActivarCampos(bool B)
        {
            CkbPpl.Enabled = B;
            CkbIng.Enabled = B;
            CkbMod.Enabled = B;
            CkbCons.Enabled = B;
            CkbImpr.Enabled = B;
            CkbElim.Enabled = B;
            TxtCE1.Enabled = B;
            TxtCE2.Enabled = B;
            TxtCE3.Enabled = B;
            TxtCE4.Enabled = B;
            TxtCE5.Enabled = B;
            TxtCE6.Enabled = B;
        }
        void LimparCampos()
        {
            CkbPpl.Checked = false;
            CkbIng.Checked = false;
            CkbMod.Checked = false;
            CkbCons.Checked = false;
            CkbImpr.Checked = false;
            CkbElim.Checked = false;
            TxtCE1.Text = "";
            TxtCE2.Text = "";
            TxtCE3.Text = "";
            TxtCE4.Text = "";
            TxtCE5.Text = "";
            TxtCE6.Text = "";
            TxtDescripcion.Text = "";
        }
        protected void IbtConsultar_Click(object sender, ImageClickEventArgs e)
        { BindData(TxtBusqueda.Text); }
        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            if ((int)Session["IdForm"] > 0)
            {
                if (TxtDescripcion.Text != string.Empty)
                {
                    if (BtnModificar.Text == "Modificar")
                    {
                        BtnModificar.Text = "Aceptar";
                        ActivarCampos(true);
                        BtnModificar.OnClientClick = "return confirm('¿Desea realizar la actualización?');";
                    }
                    else
                    {
                        try
                        {
                            Cnx.BaseDatos(Session["D[BX"].ToString(), Session["$VR"].ToString(), Session["V$U@"].ToString(), Session["P@$"].ToString());
                            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                            {
                                sqlCon.Open();
                                int Ppal = 0, ing = 0, Mod = 0, Cons = 0, Impr = 0, Elim = 0;

                                if (CkbPpl.Checked == true)
                                { Ppal = 1; }
                                if (CkbIng.Checked == true)
                                { ing = 1; }
                                if (CkbMod.Checked == true)
                                { Mod = 1; }
                                if (CkbCons.Checked == true)
                                { Cons = 1; }
                                if (CkbImpr.Checked == true)
                                { Impr = 1; }
                                if (CkbElim.Checked == true)
                                { Elim = 1; }
                                string Txtsql = "UPDATE TblUsrFormulario SET IngresarF=@ing, ModificarF=@Mod, ConsultarF=@Cons, ImprimirF=@Impr, EliminarF=@Elim, CasoEspeciaLF1=@CE1," +
                                    "CasoEspeciaLF2=@CE2,CasoEspeciaLF3 =@CE3, CasoEspeciaLF4=@CE4, CasoEspeciaLF5=@CE5, CasoEspeciaLF6=@CE6, Principal=@Ppal  WHERE CodIdFormulario=@ID";
                                SqlCommand sqlCmd = new SqlCommand(Txtsql, sqlCon);
                                sqlCmd.Parameters.AddWithValue("@Ppal", Ppal);
                                sqlCmd.Parameters.AddWithValue("@ing", ing);
                                sqlCmd.Parameters.AddWithValue("@Mod", Mod);
                                sqlCmd.Parameters.AddWithValue("@Cons", Cons);
                                sqlCmd.Parameters.AddWithValue("@Impr", Impr);
                                sqlCmd.Parameters.AddWithValue("@Elim", Elim);
                                sqlCmd.Parameters.AddWithValue("@CE1", TxtCE1.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@CE2", TxtCE2.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@CE3", TxtCE3.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@CE4", TxtCE4.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@CE5", TxtCE5.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@CE6", TxtCE6.Text.ToString());
                                sqlCmd.Parameters.AddWithValue("@ID", Session["IdForm"].ToString());

                                sqlCmd.ExecuteNonQuery();
                                sqlCon.Close();
                                BtnModificar.Text = "Modificar";
                                ActivarCampos(false);
                                LimparCampos();
                                BtnModificar.OnClientClick = "";
                                BindData(TxtBusqueda.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.UpPanel, UpPanel.GetType(), "IdntificadorBloqueScript", "alert('Error en el proceso de edición')", true);

                            ClsConexion ClsUE = new ClsConexion();
                            ClsUE.UpdateError(Session["C77U"].ToString(), "FrmConfigPantalla", "Update", "0", ex.Message, Session["77Version"].ToString(), Session["77Act"].ToString());
                        }
                    }
                }
            }
        }
        protected void GrdDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                Session["IdForm"] = int.Parse(GrdDatos.DataKeys[index].Value.ToString());
                int vblleee = (int)Session["IdForm"];
                BtnModificar.Enabled = true;
                Cnx.SelecBD();
                using (SqlConnection sqlConx = new SqlConnection(Cnx.GetConex()))
                {
                    string LtxtSql = "SP_ConfiguracionV2_ 12,'','','','',''," + ((int)Session["IdForm"]).ToString() + ",0,0,0,'01-01-01','02-01-01','03-01-01'";
                    SqlCommand Comando = new SqlCommand(LtxtSql, sqlConx);
                    sqlConx.Open();
                    SqlDataReader tbl = Comando.ExecuteReader();
                    if (tbl.Read())
                    {

                        TxtDescripcion.Text = "";
                        if (tbl["NomFormWeb"].ToString() != string.Empty)
                        {
                            TxtDescripcion.Text = tbl["Descripcion"].ToString();
                            CkbPpl.Checked = Convert.ToBoolean(tbl["Principal"]);
                            CkbIng.Checked = Convert.ToBoolean(tbl["IngresarF"]);
                            CkbMod.Checked = Convert.ToBoolean(tbl["ModificarF"]);
                            CkbCons.Checked = Convert.ToBoolean(tbl["ConsultarF"]);
                            CkbImpr.Checked = Convert.ToBoolean(tbl["ImprimirF"]);
                            CkbElim.Checked = Convert.ToBoolean(tbl["EliminarF"]);

                            TxtCE1.Text = tbl["CasoEspeciaLF1"].ToString();
                            TxtCE2.Text = tbl["CasoEspeciaLF2"].ToString();
                            TxtCE3.Text = tbl["CasoEspeciaLF3"].ToString();
                            TxtCE4.Text = tbl["CasoEspeciaLF4"].ToString();
                            TxtCE5.Text = tbl["CasoEspeciaLF5"].ToString();
                            TxtCE6.Text = tbl["CasoEspeciaLF6"].ToString();
                        }
                        BtnModificar.Text = "Modificar";
                    }
                }
            }
        }
        protected void GrdDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDatos.PageIndex = e.NewPageIndex;
            BindData(TxtBusqueda.Text);
        }
        protected override void OnPreRender(EventArgs e)
        {            base.OnPreRender(e);            SetFixedHeightForGridIfRowsAreLess(GrdDatos);        }
        public void SetFixedHeightForGridIfRowsAreLess(GridView gv)
        {
            double headerFooterHeight = gv.HeaderStyle.Height.Value + 20; // height style=35px and there no footer  height so assume footer also same
            double rowHeight = gv.RowStyle.Height.Value;
            int gridRowCount = gv.Rows.Count;
            if (gridRowCount <= gv.PageSize)
            {
                double height = (gridRowCount * rowHeight) + ((gv.PageSize - gridRowCount) * rowHeight) + headerFooterHeight;
                //adjust footer height based on white space removal between footer and last row
                height += 22;
                gv.Height = new Unit(height);
            }
        }
    }
}