using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


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
            Produccion = "Y";//N = para trabajar en el desarrollo | Y  =aplica para PRoduccion            
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
        //public bool Cosultar(string Ltx1)
        //{
        //    System.Web.HttpContext.Current.Session["ELiminar"] = 0;
        //    try
        //    {
        //        SelecBD();
        //        using (SqlConnection cnn = new SqlConnection(GetConex()))
        //        {
        //            DataTable dtbl = new DataTable();
        //            SqlDataAdapter sqlDa = new SqlDataAdapter(Ltx1, cnn);
        //            sqlDa.Fill(dtbl);
        //            if (dtbl.Rows.Count > 0)
        //            {
        //                System.Web.HttpContext.Current.Session["ELiminar"] = dtbl.Rows.Count;
        //            }
        //            return (dtbl.Rows.Count > 0);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        public void UpdateError(string VbUsu, string VbPantalla, string VbAccion, string VbNumLinea, string VbMensErr, string VbVersion, string VbAct)
        {
            try
            {
                string VbNitErr, VbCiaErr, VblNomBDErr;
                using (SqlConnection sqlCon = new SqlConnection(BaseDatosPrmtr()))
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
            catch (Exception)
            {

            }
        }
        public void UpdateErrorV2(string VbUsu, string VbPantalla, string VbAccion, string VbFrmLinea, string VbMensErr, string VbVersion, string VbAct)
        {
            try
            {
                string VbNitErr, VbCiaErr, VblNomBDErr;
                using (SqlConnection sqlCon = new SqlConnection(BaseDatosPrmtr()))
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
            catch (Exception)
            {
            }
        }
        public void BaseDatos(string VbNomBD, string VblNomSrv, string VbUsu, string VblPass)
        {

            if (VbNomBD == string.Empty) { BaseDatosPrmtr(); }
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
        public string BaseDatosPrmtr()
        {
            if (Produccion.Equals("Y"))
            {
                //  return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, "77NEO01", "DbConfigWeb", "sa", "admindemp");
                string Vb1S = "23.102.100.143";//@"aircraft\SQLEXPRESS";
                return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, Vb1S, "DbConfigWeb", "sa", "Medellin2021**");
            }
            else
            {
                 //return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, "77NEO01", "DbConfigWeb", "sa", "admindemp");
               return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, "23.102.100.143", "DbConfigWeb", "sa", "Medellin2021**");
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
        public string ReturnFecha(string StrCampo)
        {
            string VbFecSt;
            DateTime? VbFecDT;
            VbFecSt = StrCampo.Equals("") ? "01/01/1900" : StrCampo;
            VbFecDT = Convert.ToDateTime(VbFecSt);
            return VbFecSt.Equals("01/01/1900") ? "" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
        }
        public string ValidarFechas2(string VbF1, string VbF2, int NumPrmts)
        {
            DateTime FI, FF;
            int Comparar;
            if (NumPrmts == 1)// una fecha
            {
                if (VbF1.Equals("")) { return "MstrMens08"; }//Feha invalida     
                if (VbF1.Length > 10) { return "MstrMens08"; }//Feha invalida

                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime("01/01/1900");
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) { return "MstrMens08"; }// //-1 menor; 0 igual; 1 mayor   -- Feha invalida
            }
            else// dos fechas
            {
                if (VbF1.Equals("") || VbF2.Equals("")) { return "MstrMens08"; }//Feha invalida
                if (VbF1.Length > 10 || VbF2.Length > 10) { return "MstrMens08"; }//Feha invalida
                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime(VbF2.Trim());
                Comparar = DateTime.Compare(FF, FI);
                if (Comparar < 0) { return "MstrMens13"; } //-1 menor; 0 igual; 1 mayor -- Rango de Feha invalida
                FI = Convert.ToDateTime(VbF1.Trim());
                FF = Convert.ToDateTime("01/01/1900");
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) { return "MstrMens08"; }//-1 menor; 0 igual; 1 mayor -- Feha invalida
                FI = Convert.ToDateTime(VbF2.Trim());
                Comparar = DateTime.Compare(FI, FF);
                if (Comparar < 0) { return "MstrMens08"; }////-1 menor; 0 igual; 1 mayor      Feha invalida
            }
            return "";
        }
        public bool ValidaDataRowVacio(IEnumerable<DataRow> ieNumerable)
        {
            bool isFull = false;
            foreach (DataRow item in ieNumerable)
            { isFull = true; break; }
            return isFull;
        }
        public string ValorDecimal() { return this.VblDecimal; }
        public string GetMensj() { return PMensj; }
        public string GetProduccion() { return Produccion; }
        //******************* cONEXION TEMPORAL ********
        public string GetUsr() { return "00000082"; }//00000082|00000133 | 00000129 |
        public int GetIdCia() { return 1; }// 1 TwoGoWo |21 Demp |2 HCT PRUEBA| 12 ADA | 20 HCT | 3 Alca
        public string GetMonedLcl() { return "COP"; }//  "COP|USD"
        public int GetFormatFecha() { return 101; }// 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy
        public string GetNit() { return "901338233-1"; } // 901338233-1 TwoGoWo |811035879-1 Demp |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT |P93000086218 - ALCA
        public string GetBD() { return "DbNeoDempV2"; }//|DbNeoDempV2 |DbNeoAda | DbNeoHCT ||| BDNeoW
        public string GetSvr() { return "77NEO01"; }//  "77NEO01"; ||| 23.102.100.143
        public string GetUsSvr() { return "sa"; }//  "sa"
        public string GetPas() { return "admindemp"; }//"admindemp";|| Medellin2021**
        public string GetIdm() { return "5"; }//  4 español | 5 ingles/**/
    }
}