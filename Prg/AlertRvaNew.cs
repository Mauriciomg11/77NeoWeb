using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg
{
    public class AlertRvaNew
    {
        ClsConexion Cnx = new ClsConexion();
        public void NuevaRva()
        {
            DataTable DTDet = new DataTable();
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                string VbTxtSql = "EXEC SP_Ejecutar_Consultas_Activar_Alertas @ICC,@Usu";
                sqlCon.Open();
                using (SqlCommand SC = new SqlCommand(VbTxtSql, sqlCon))
                {
                    SC.Parameters.AddWithValue("@ICC", System.Web.HttpContext.Current.Session["!dC!@"]);
                    SC.Parameters.AddWithValue("@Usu", System.Web.HttpContext.Current.Session["C77U"]);
                    SqlDataAdapter SDA = new SqlDataAdapter();
                    SDA.SelectCommand = SC;
                    SDA.Fill(DTDet);                    
                    if (DTDet.Rows.Count > 0)
                    { System.Web.HttpContext.Current.Session["AlrtNewRva"] = "Y"; }
                }
            }
        }
    }
}