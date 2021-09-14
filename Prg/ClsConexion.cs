using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Sql;

namespace _77NeoWeb.prg
{
    public class ClsConexion
    {
        static public string PMensj;
        static public string Produccion;

        string VblConexion, VblDecimal;

        public ClsConexion()
        {
            this.VblConexion = "";
            Produccion = "N";//N = para trabajar en el desarrollo | Y  =aplica para PRoduccion 
        }
        public void SelecBD()
        {
            string VbNBD, VbSv, VbU, VbCs;
            VbNBD = System.Web.HttpContext.Current.Session["D[BX"].ToString();
            VbSv = System.Web.HttpContext.Current.Session["$VR"].ToString();
            VbU = System.Web.HttpContext.Current.Session["V$U@"].ToString();
            VbCs = System.Web.HttpContext.Current.Session["P@$"].ToString();
            BaseDatos(VbNBD, VbSv, VbU, VbCs);
        }
        public void Desconctar()
        {
            //BaseDatos(System.Web.HttpContext.Current.Session["D[BX"].ToString(), System.Web.HttpContext.Current.Session["$VR"].ToString(), System.Web.HttpContext.Current.Session["V$U@"].ToString(), System.Web.HttpContext.Current.Session["P@$"].ToString());
            SelecBD();
            SqlConnection cnn = new SqlConnection(GetConex());
            cnn.Close();
        }
        public DataSet DSET(string sentencia)
        {
            SelecBD();
            using (SqlConnection cnn = new SqlConnection(GetConex()))
            {
                DataSet ds = new DataSet();
                try
                {
                    SqlDataAdapter SDa = new SqlDataAdapter(sentencia, cnn);
                    SDa.Fill(ds, "Datos");
                }
                catch (SqlException)
                {
                    return null;
                }
                return ds;
            }
        }
        public bool Cosultar(string Ltx1)
        {
            System.Web.HttpContext.Current.Session["ELiminar"] = 0;
            try
            {
                SelecBD();
                using (SqlConnection cnn = new SqlConnection(GetConex()))
                {
                    DataTable dtbl = new DataTable();
                    SqlDataAdapter sqlDa = new SqlDataAdapter(Ltx1, cnn);
                    sqlDa.Fill(dtbl);
                    if (dtbl.Rows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Session["ELiminar"] = dtbl.Rows.Count;
                    }
                    return (dtbl.Rows.Count > 0);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void UpdateError(string VbUsu, string VbPantalla, string VbAccion, string VbNumLinea, string VbMensErr, string VbVersion, string VbAct)
        {
            try
            {
                string VbNitErr, VbCiaErr, VblNomBDErr;
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
                {
                    VbNitErr = System.Web.HttpContext.Current.Session["Nit77Cia"].ToString();
                    VbCiaErr = System.Web.HttpContext.Current.Session["NomCiaPpal"].ToString();
                    VblNomBDErr = System.Web.HttpContext.Current.Session["D[BX"].ToString();

                    sqlCon.Open();
                    string query = "INSERT INTO TblErrores (Usuario, Programa, Codigo, NumeroLinea, Fecha, revisado, Version, Mensaje, ActualizacionErr, NIT, NomCia, NomBD) " +
                       "VALUES(@Usuario, @Programa, @Codigo, @NumeroLinea,GetDate(),0, @Version, @Mensaje, @ActualizacionErr, @NIT, @NomCia, @NomBD)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Usuario", VbUsu);
                    sqlCmd.Parameters.AddWithValue("@Programa", VbPantalla);
                    sqlCmd.Parameters.AddWithValue("@Codigo", VbAccion);
                    sqlCmd.Parameters.AddWithValue("@NumeroLinea", VbNumLinea);
                    sqlCmd.Parameters.AddWithValue("@Version", VbVersion);
                    sqlCmd.Parameters.AddWithValue("@Mensaje", VbMensErr);
                    sqlCmd.Parameters.AddWithValue("@ActualizacionErr", VbAct);
                    sqlCmd.Parameters.AddWithValue("@NIT", VbNitErr);
                    sqlCmd.Parameters.AddWithValue("@NomCia", VbCiaErr);
                    sqlCmd.Parameters.AddWithValue("@NomBD", VblNomBDErr);

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string vble1;
                vble1 = ex.Message;
            }
        }
        public void UpdateErrorV2(string VbUsu, string VbPantalla, string VbAccion, string VbFrmLinea, string VbMensErr, string VbVersion, string VbAct)
        {
            try
            {
                string VbNitErr, VbCiaErr, VblNomBDErr;
                using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString))
                {
                    VbNitErr = System.Web.HttpContext.Current.Session["Nit77Cia"].ToString();
                    VbCiaErr = System.Web.HttpContext.Current.Session["NomCiaPpal"].ToString();
                    VblNomBDErr = System.Web.HttpContext.Current.Session["D[BX"].ToString();

                    sqlCon.Open();
                    string query = "INSERT INTO TblErrores (Usuario, Programa, Codigo, FrmLInea, Fecha, revisado, Version, Mensaje, ActualizacionErr, NIT, NomCia, NomBD) " +
                       "VALUES(@Usuario, @Programa, @Codigo, @FrmLInea,GetDate(),0, @Version, @Mensaje, @ActualizacionErr, @NIT, @NomCia, @NomBD)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Usuario", VbUsu);
                    sqlCmd.Parameters.AddWithValue("@Programa", VbPantalla);
                    sqlCmd.Parameters.AddWithValue("@Codigo", VbAccion);
                    sqlCmd.Parameters.AddWithValue("@FrmLInea", VbFrmLinea);
                    sqlCmd.Parameters.AddWithValue("@Version", VbVersion);
                    sqlCmd.Parameters.AddWithValue("@Mensaje", VbMensErr);
                    sqlCmd.Parameters.AddWithValue("@ActualizacionErr", VbAct);
                    sqlCmd.Parameters.AddWithValue("@NIT", VbNitErr);
                    sqlCmd.Parameters.AddWithValue("@NomCia", VbCiaErr);
                    sqlCmd.Parameters.AddWithValue("@NomBD", VblNomBDErr);

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string vble1;
                vble1 = ex.Message;
            }
        }
        public void BaseDatos(string VbNomBD, string VblNomSrv, string VbUsu, string VblPass)
        {

            if (VbNomBD == string.Empty)
            {
                this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpal"].ConnectionString);
            }
            else
            {
                switch (VbNomBD.Substring(0, 3))
                {
                    case "Web":
                        this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["WebPConexDB"].ConnectionString, "", VbNomBD, "", "");
                        break;
                    default:
                        this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString, VblNomSrv, VbNomBD, VbUsu, VblPass);
                        break;
                }
            }
        }
        public string GetConex() { return this.VblConexion; }
        public void RetirarPuntos(string VbCampo)
        {
            int I = VbCampo.IndexOf(",") == -1 ? 0 : VbCampo.IndexOf(",");
            if (I > 0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, ".").Replace(",", ""); }
            I = VbCampo.IndexOf(".");
            if (I > 0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, ",").Replace(".", ""); }
            else if (I == 0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, "0,").Replace(".", ""); }
            this.VblDecimal = VbCampo;
        }
        public void ValidarFechas(string VbF1, string VbF2, int NumPrmts)
        {
            PMensj = "";
            DateTime FI, FF;
            int Comparar;
            if (NumPrmts == 1)// una fecha
            {
                if (VbF1.Equals(""))
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;

                }
                if (VbF1.Length > 10)
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;
                }

                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime("01/01/1900");
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;
                }
            }
            else// dos fechas
            {
                if (VbF1.Equals("") || VbF2.Equals(""))
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;

                }
                if (VbF1.Length > 10 || VbF2.Length > 10)
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;
                }
                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime(VbF2.Trim());
                Comparar = DateTime.Compare(FF, FI);
                if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
                {
                    PMensj = "MstrMens13";//Rango de Feha invalida
                    return;
                }
                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime("01/01/1900");
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;
                }
                FI = Convert.ToDateTime(VbF2.Trim());
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) //-1 menor; 0 igual; 1 mayor
                {
                    PMensj = "MstrMens08";//Feha invalida
                    return;
                }
            }
        }
        public string ValorDecimal() { return this.VblDecimal; }
        public string GetMensj() { return PMensj; }
        public string GetProduccion() { return Produccion; }
        //******************* cONEXION TEMPORAL ********
        public string GetUsr() { return "00000082"; }//00000082|00000133
        public int GetIdCia() { return 2; }//2 HCT PRUEBA|21 Demp|
        public string GetIdm() { return "4"; }//  4 español | 5 ingles
        public string GetNit() { return "860064038-4"; } // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
        public string GetBD() { return "DbNeoHCT"; }//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
        public string GetSvr() { return "77NEO01"; }//  "77NEO01";
        public string GetUsSvr() { return "sa"; }//  "sa"
        public string GetPas() { return "admindemp"; }//"admindemp";
    }
}