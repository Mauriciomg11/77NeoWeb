using System;
using System.Data.SqlClient;
using System.Configuration;

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
        public void Acceder(string ClsUsu,string ClsNomF)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString))
            {
                string datoGrid = "EXEC SP_ValidaMaestras 16,'"+ ClsNomF + "','','','',''," + ClsUsu + ",0,0,0,'01-01-01','02-01-01','03-01-01'";

                try
                {
                    SqlCommand Comando = new SqlCommand(datoGrid, sqlCon);
                sqlCon.Open();
               
                    SqlDataReader registro = Comando.ExecuteReader();
                    if (registro.Read())
                    {
                        this.AccesoFrm = 1;
                        this.Ingresar = Convert.ToInt32(registro["Ingresar"]);
                        this.Modificar = Convert.ToInt32(registro["Modificar"]);
                        this.Consultar = Convert.ToInt32(registro["Consultar"]);
                        this.Eliminar = Convert.ToInt32(registro["Eliminar"]);
                        this.Imprimir = Convert.ToInt32(registro["imprimir"]);
                        this.CE1 = Convert.ToInt32(registro["CasoEspecial1"]);
                        this.CE2 = Convert.ToInt32(registro["CasoEspecial2"]);
                        this.CE3 = Convert.ToInt32(registro["CasoEspecial3"]);
                        this.CE4 = Convert.ToInt32(registro["CasoEspecial4"]);
                        this.CE5 = Convert.ToInt32(registro["CasoEspecial5"]);
                        this.CE6 = Convert.ToInt32(registro["CasoEspecial6"]);
                    }
                    sqlCon.Close();
                }
                catch (Exception ex)
                {                   
                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('"+ ex.Message+"')", true);
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