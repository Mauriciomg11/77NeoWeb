using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            Produccion = "Y";//N = para trabajar en el desarrollo | Y  =aplica para Producción            
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
        public DataTable DTIdioma(string TxtSql)
        {
            DataTable DTIdm = new DataTable();
            SelecBD();
            using (SqlConnection SqlCnc = new SqlConnection(BaseDatosPrmtr()))
            {
                using (SqlCommand SC = new SqlCommand(TxtSql, SqlCnc))
                {
                    using (SqlDataAdapter SqlDA = new SqlDataAdapter())
                    {
                        try
                        {
                            SqlDA.SelectCommand = SC;
                            SqlDA.Fill(DTIdm);
                        }
                        catch (SqlException)
                        {
                            return null;
                        }
                        return DTIdm;
                    }
                }
            }
        }
        public void UpdateError(string VbUsu, string VbPantalla, string VbAccion, string VbNumLinea, string VbMensErr, string VbVersion, string VbAct)
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
        public void UpdateErrorV2(string VbUsu, string VbPantalla, string VbAccion, string VbFrmLinea, string VbMensErr, string VbVersion, string VbAct)
        {
            string VbNitErr, VbCiaErr, VblNomBDErr;
            using (SqlConnection sqlCon = new SqlConnection(BaseDatosPrmtr()))
            {
                //VbFrmLinea = VbFrmLinea.Substring(0,300);
                int VblI = 0, VbLargo = 250;
                string VbLinea = VbFrmLinea.ToString().Trim().Substring(VblI, VbLargo);
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
                sqlCmd.Parameters.AddWithValue("@FrmLInea", VbLinea);
                sqlCmd.Parameters.AddWithValue("@Version", VbVersion);
                sqlCmd.Parameters.AddWithValue("@Mensaje", VbMensErr);
                sqlCmd.Parameters.AddWithValue("@ActualizacionErr", VbAct);
                sqlCmd.Parameters.AddWithValue("@NIT", VbNitErr);
                sqlCmd.Parameters.AddWithValue("@NomCia", VbCiaErr);
                sqlCmd.Parameters.AddWithValue("@NomBD", VblNomBDErr);

                sqlCmd.ExecuteNonQuery();
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
                /*return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, @"77NEO01", "DbConfigWeb", "sa", "admindemp");*/
                // string Vb1S = "23.102.100.143";//@"aircraft\SQLEXPRESS";
                return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, "23.102.100.143", "DbConfigWeb", "sa", "Medellin2021**");
            }
            else
            {//Configuracion web en diseño
                 // return this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, @"77NEO01", "DbConfigWeb", "sa", "admindemp");
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
            //return VbFecSt.Equals("01/01/1900") ? "01/01/1900" : string.Format("{0:yyyy-MM-dd}", VbFecDT);
            return string.Format("{0:yyyy-MM-dd}", VbFecDT);
        }
        public string GetIpPubl()
        {
            string S_HN = "77NEO";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());// objeto para guardar la ip
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    S_HN = ip.ToString();// esta es nuestra ip
                }
            }
            return S_HN;
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
        public void AlrtNewRva()
        {  // 1. Obtener el contexto actual
            var handler = HttpContext.Current.Handler as Page;

            if (handler != null && handler.Master is MasterTransac)
            {
                // 2. Castear (convertir) a la Master Page específica
                MasterTransac master = (MasterTransac)handler.Master;

                // 3. Llamar al método público
                master.SetObjMensj("Nueva Reserva");
            }
        }
        /*public bool EnviarCorreo(string S_From, string S_To, string S_Subject, string S_Body)
        {
            MailMessage mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(S_From);
                mail.To.Add(new MailAddress(S_To));
                mail.Subject = S_Subject;
                mail.Body = S_Body;
                mail.IsBodyHtml = false;
                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("soporteneosystem@gmail.com", "hxrzyzcdsojrtguh");
                    client.EnableSsl = true;
                    client.Send(mail);
                }
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }*/
        public bool EnviarCorreo(string S_From, string S_To, string S_Subject, string S_Body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(S_From);

                    // Separar por ; o , y agregar cada correo
                    var destinatarios = S_To.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var correo in destinatarios)
                    {
                        mail.To.Add(new MailAddress(correo.Trim()));
                    }

                    mail.Subject = S_Subject;
                    mail.Body = S_Body;
                    mail.IsBodyHtml = false;

                    using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.Credentials = new NetworkCredential(
                            ConfigurationManager.AppSettings["SmtpUser"],
                            ConfigurationManager.AppSettings["SmtpPass"]);
                        client.EnableSsl = true;
                        client.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //******************* CONEXION TEMPORAL ********
        public string GetUsr() { return "00000157"; }//00000082|00000157 AdmonOMAP | 00000129 | 00000110
        public int GetIdCia() { return 25; }// 1 TwoGoWo |21 Demp |2 HCT PRUEBA| 12 ADA | 20 HCT | 3 Alca
        public string GetMonedLcl() { return "COP"; }//  "COP|USD"
        public int GetFormatFecha() { return 103; }// 103 formato europeo dd/MM/yyyy | 101 formato EEUU MM/dd/yyyyy
        public string GetNit() { return "2525252525-5"; } // 901338233-1 TwoGoWo |811035879-1 Demp |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT |P93000086218 - ALCA || 242424242-7  Opera  || 2525252525-5 OMA
        public string GetBD() { return "DbNeoDempV2"; }// DbNeoDemp2016||DbNeoDempV2 |DbNeoAda | DbNeoHCT ||| BDNeoW  ||
        public string GetSvr() { return @"23.102.100.143"; }// 77NEO01|| 23.102.100.143 ||
        public string GetUsSvr() { return "sa"; }//  "sa"
        public string GetPas() { return "Medellin2021**"; }// admindemp|| Medellin2021**
        public string GetIdm()
        {
            DataSet DSIdm = new DataSet();
            string LtxtSql = string.Format("EXEC SP_ConfiguracionV2_ 21,'','','','','',0,0,0,{0},'01-01-1','02-01-1','03-01-1'", GetIdCia());
            DSIdm = DSET(LtxtSql);
            DSIdm.Tables[0].TableName = "Idioma";
            // Guarda la estructura y datos de la tabla idioma general
            //string boor = System.Web.HttpContext.Current.Session["TblIdmGrl"].ToString().Trim();
            if (System.Web.HttpContext.Current.Session["TblIdmGrl"]==null || System.Web.HttpContext.Current.Session["TblIdmGrl"].ToString().Trim().Equals("") )
            {
                DataTable IdiomaAll = new DataTable();
                LtxtSql = string.Format("EXEC IdiomaALL {0}", DSIdm.Tables["Idioma"].Rows[0]["Idioma"].ToString().Trim());
                IdiomaAll = DTIdioma(LtxtSql);
                System.Web.HttpContext.Current.Session["TblIdmGrl"] = IdiomaAll;
            }           
            //Envia el idioma
            return DSIdm.Tables["Idioma"].Rows[0]["Idioma"].ToString().Trim();            
        }//  4 español | 5 ingles/**/
    }
}