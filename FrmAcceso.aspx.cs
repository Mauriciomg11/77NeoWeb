using _77NeoWeb.prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace _77NeoWeb
{
    public partial class FrmAcceso : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable IdiomaAll = new DataTable();
        DataTable Idioma = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("Entry");

            Session["Login77"] = null;
            if (!IsPostBack)
            {
                string LtxtSql = "EXEC SP_ACCESO_WEB 1,'','','','','',0, 0,0,0,'01-01-1','01-01-1'";
                Session["D[BX"] = "";
                Session["Nit77Cia"] = "";
                Session["$VR"] = "";
                Session["V$U@"] = "";
                Session["P@$"] = "";
                Session["NomCiaPpal"] = "";
                Session["!dC!@"] = "0";
                DdlNit.DataSource = Cnx.DSET(LtxtSql);
                DdlNit.DataMember = "Datos";
                DdlNit.DataTextField = "RazonSocial";
                DdlNit.DataValueField = "CodNit";
                ViewState["IniSes"] = "CIA";
                ViewState["TablaIdioma"] = Idioma;
                DdlNit.DataBind();
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>myFuncionddlP();</script>", false);
        }
        protected void IdiomaControles()
        {
            string LtxtSql = string.Format("EXEC IdiomaALL {0}", Session["77IDM"].ToString().Trim());
            IdiomaAll = Cnx.DTIdioma(LtxtSql);
            Session["TblIdmGrl"] = IdiomaAll;
            DataRow[] DR = IdiomaAll.Select("CodF='FrmAcceso'");
            if (Cnx.ValidaDataRowVacio(DR))
            { Idioma = DR.CopyToDataTable(); ViewState["TablaIdioma"] = Idioma; }
           // Idioma = (DataTable)ViewState["TablaIdioma"];
            //LblText1.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblText1").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            //LblText2.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblText2").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            //LblText3.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblText3").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            LblInicio.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "LblInicio").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            TbnIngresar.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "TbnIngresar").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            TbnIngresar.Text = Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "TbnIngresar").Select(x => x.Field<string>("Texto")).FirstOrDefault();
            TxtPassEmsa.Attributes.Add("placeholder", Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "placeholder").Select(x => x.Field<string>("Texto")).FirstOrDefault());
            TxtUsuario.Attributes.Add("placeholder", Idioma.AsEnumerable().Where(x => x.Field<string>("Objeto") == "placeholderUsu").Select(x => x.Field<string>("Texto")).FirstOrDefault());
        }
        protected void TbnIngresar_Click(object sender, EventArgs e)
        {
            string LtxtSql, VbUsu, VbPassCia;
            Idioma = (DataTable)ViewState["TablaIdioma"];
            if (ViewState["IniSes"].Equals("USU"))
            {
                // Valida Usuario y contraseña 
                VbUsu = TxtUsuario.Text;
                while (VbUsu.Contains(" "))
                {
                    VbUsu = VbUsu.Replace(" ", "");
                }
                TxtUsuario.Text = VbUsu;               
                using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
                {
                    LtxtSql = "EXEC SP_Configuracion  1,@P1,@P3,'','','',0,0,0,0,'01-01-1','02-01-1','03-01-1'";
                    SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                    SC.Parameters.AddWithValue("@P1", Session["SigCiaPpal"].ToString());
                    SC.Parameters.AddWithValue("@P3", DdlBD.SelectedValue);

                    sqlCon.Open();
                    SqlDataReader tbl = SC.ExecuteReader();
                    if (tbl.Read())
                    {
                        Session["D[BX"] = tbl["NomDB"].ToString();
                        Session["Nit77Cia"] = DdlNit.SelectedValue;
                        Session["$VR"] = tbl["NomSrvdr"].ToString();
                        Session["V$U@"] = tbl["UsuSA"].ToString();
                        Session["P@$"] = tbl["Clve"].ToString();
                        Session["NomCiaPpal"] = tbl["RazonSocial"].ToString();
                        Session["SigCia"] = tbl["SiglaCia"].ToString();
                        Session["LogoPpal"] = tbl["Logo"].ToString();
                        Session["!dC!@"] = DdlBD.Text.ToString(); //cia
                        Session["77IDM"] = tbl["Idioma"].ToString(); //Idiioma
                    }
                    else
                    {
                        Session["D[BX"] = "";
                        Session["!dC!@"] = "0";
                        return;
                    }
                }
                Cnx.SelecBD();
                using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
                {
                    // valida usuario y Pass
                    LtxtSql = " EXEC SP_ConfiguracionV2_ 2,@H77,@H775,'','','',@PI,0,0,@ICC,'01-01-1','02-01-1','03-01-1'";
                    string b2 = Session["77IDM"].ToString();
                    SqlCommand Comando = new SqlCommand(LtxtSql, sqlCon);
                    Comando.Parameters.AddWithValue("@H77", TxtUsuario.Text);
                    Comando.Parameters.AddWithValue("@H775", TxtClave.Text);
                    Comando.Parameters.AddWithValue("@PI", Session["77IDM"].ToString().Trim());
                    Comando.Parameters.AddWithValue("@ICC", DdlBD.Text.ToString().Trim());
                    sqlCon.Open();
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        Session["Login77"] = TxtUsuario.Text;
                        Session["C77U"] = registro["CodUsuario"].ToString();
                        Session["N77U"] = registro["Usuario"].ToString();
                        Session["CodTipoCodigoInicial"] = registro["CodTipoCodigo"].ToString();
                        Response.Redirect("~/Forms/Seguridad/FrmInicio.aspx");
                    }
                    else
                    {
                        DataRow[] Result4 = Idioma.Select("Objeto= 'MensAcc01'");
                        foreach (DataRow row in Result4)
                        { ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + row["Texto"].ToString() + "')", true); } //Datos inválidos o usuario inactivo.
                    }
                }
            }
            else
            {
                // Valida la informacion de la CIA
                VbPassCia = TxtPassEmsa.Text;
                while (VbPassCia.Contains(" "))
                {
                    VbPassCia = VbPassCia.Replace(" ", "");
                }
                TxtPassEmsa.Text = VbPassCia;
                using (SqlConnection sqlCon = new SqlConnection(Cnx.BaseDatosPrmtr()))
                {
                    LtxtSql = "EXEC SP_ACCESO_WEB 2,@E71,@E59,'','','',0, 0,0,0,'01-01-1','01-01-1'";//Valida idcia y Contraseña Ensa | DbConfigWeb
                    SqlCommand SC = new SqlCommand(LtxtSql, sqlCon);
                    SC.Parameters.AddWithValue("@E71", DdlNit.SelectedValue);
                    SC.Parameters.AddWithValue("@E59", TxtPassEmsa.Text);
                    sqlCon.Open();
                    SqlDataReader tbl = SC.ExecuteReader();
                    if (tbl.Read())
                    {
                        string Lmoned = tbl["CodMoneda"].ToString();
                        string S_CiaPpl = tbl["SiglaCiaPpal"].ToString();
                        Session["SigCiaPpal"] = tbl["SiglaCiaPpal"].ToString();
                        Session["77IDM"] = tbl["Idioma"].ToString(); //Idiioma
                        Session["MonLcl"] = tbl["CodMoneda"].ToString(); //Moneda Local
                        Session["FormatFecha"] = tbl["FormatoFecha"].ToString(); // 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
                        DdlBD.Visible = true;
                        LtxtSql = "EXEC SP_Configuracion 1,'" + Session["SigCiaPpal"].ToString() + "','','','','DropDown',0,0,0,0,'01-01-1','02-01-1','03-01-1'";// Muestra las BD que tiene registrada la Empsa  | DbConfigWeb
                        DdlBD.DataSource = Cnx.DSET(LtxtSql);
                        DdlBD.DataMember = "Datos";
                        DdlBD.DataTextField = "RazonSocial";
                        DdlBD.DataValueField = "IdConfiguracion";
                        DdlBD.DataBind();
                        IdiomaControles();
                        DataRow[] Result1 = Idioma.Select("Objeto= 'TbnIngresarUsu'");
                        foreach (DataRow row in Result1)
                        { TbnIngresar.Text = row["Texto"].ToString().Trim(); }
                        ViewState["IniSes"] = "USU";
                        DdlNit.Enabled = false;
                        TxtUsuario.Visible = true;
                        TxtClave.Visible = true;
                        TxtPassEmsa.Visible = false;
                        DdlNit.Visible = false;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No access or invalid password.')", true);
                    }
                    sqlCon.Close();
                }
            }
        }
        protected void DdlNit_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlConx = new SqlConnection(Cnx.BaseDatosPrmtr()))
            {
                Session["Login77"] = null;
                Session["C77U"] = "";
                Session["N77U"] = "";
                Session["!dC!@"] = "0";
                TbnIngresar.Text = "Confirm Company";
                //DdlBD.SelectedValue = "";
                DdlBD.Visible = false;
                TxtClave.Visible = false;
                TxtUsuario.Visible = false;
                // TxtPassEmsa.Text = "";               
                TxtUsuario.Text = "";
                TxtClave.Text = "";
            }
        }
    }
}
