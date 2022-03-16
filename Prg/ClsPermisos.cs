using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Data.SqlClient;

namespace _77NeoWeb.prg
{
    public class ClsPermisos
    {
        private int Ingresar;
        private int Modificar;
        private int Consultar;
        private int Eliminar;
        private int Imprimir;
        private int CE1;
        private int CE2;
        private int CE3;
        private int CE4;
        private int CE5;
        private int CE6;
        private int AccesoFrm;
        public ClsPermisos()
        {
            this.Ingresar = 0;
            this.Modificar = 0;
            this.Consultar = 0;
            this.Eliminar = 0;
            this.Imprimir = 0;
            this.CE1 = 0;
            this.CE2 = 0;
            this.CE3 = 0;
            this.CE4 = 0;
            this.CE5 = 0;
            this.CE6 = 0;
            this.AccesoFrm = 0;
        }
        public void Acceder(string ClsUsu, string ClsNomF, string NomPc)
        {
            ClsConexion Cnx = new ClsConexion();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbVersion, VbAct;
                VbVersion = System.Web.HttpContext.Current.Session["77Version"].ToString();
                VbAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                string VBQuery = "EXEC SP_ConfiguracionV2_ 1, @NomFrm, @Us,@Vrs,@NPC,'',0,@Act, @Idm, @ICC,'01-01-1','02-01-1','03-01-1'";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VBQuery, sqlCon))
                {
                    SC.Parameters.AddWithValue("@NomFrm", ClsNomF);
                    SC.Parameters.AddWithValue("@Us", ClsUsu);
                    SC.Parameters.AddWithValue("@Vrs", VbVersion);
                    SC.Parameters.AddWithValue("@NPC", NomPc);
                    SC.Parameters.AddWithValue("@Act", VbAct);
                    SC.Parameters.AddWithValue("@Idm", System.Web.HttpContext.Current.Session["77IDM"].ToString());
                    SC.Parameters.AddWithValue("@ICC", System.Web.HttpContext.Current.Session["!dC!@"].ToString());
                    SqlDataReader SDR = SC.ExecuteReader();
                    if (SDR.Read())
                    {
                        this.AccesoFrm = 1;
                        this.Ingresar = Convert.ToInt32(SDR["Ingresar"]);
                        this.Modificar = Convert.ToInt32(SDR["Modificar"]);
                        this.Consultar = Convert.ToInt32(SDR["Consultar"]);
                        this.Eliminar = Convert.ToInt32(SDR["Eliminar"]);
                        this.Imprimir = Convert.ToInt32(SDR["imprimir"]);
                        this.CE1 = Convert.ToInt32(SDR["CasoEspecial1"]);
                        this.CE2 = Convert.ToInt32(SDR["CasoEspecial2"]);
                        this.CE3 = Convert.ToInt32(SDR["CasoEspecial3"]);
                        this.CE4 = Convert.ToInt32(SDR["CasoEspecial4"]);
                        this.CE5 = Convert.ToInt32(SDR["CasoEspecial5"]);
                        this.CE6 = Convert.ToInt32(SDR["CasoEspecial6"]);
                    }
                }
            }            
        }
        public int GetAccesoFrm()
        {
            return this.AccesoFrm;
        }
        public int GetIngresar()
        {
            return this.Ingresar;
        }
        public int GetModificar()
        {
            return this.Modificar;
        }
        public int GetConsultar()
        {
            return this.Consultar;
        }
        public int GetEliminar()
        {
            return this.Eliminar;
        }
        public int GetImprimir()
        {
            return this.Imprimir;
        }
        public int GetCE1()
        {
            return this.CE1;
        }
        public int GetCE2()
        {
            return this.CE2;
        }
        public int GetCE3()
        {
            return this.CE3;
        }
        public int GetCE4()
        {
            return this.CE4;
        }
        public int GetCE5()
        {
            return this.CE5;
        }
        public int GetCE6()
        {
            return this.CE6;
        }
    }
}