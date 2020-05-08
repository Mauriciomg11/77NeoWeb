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
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString);
        public void Desconctar()
        {
            cnn.Close();
        }
        public DataSet DSET(string sentencia)
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
        public bool Cosultar(string Ltx1)
        {
            System.Web.HttpContext.Current.Session["ELiminar"] = 0;
            try
            {
                DataTable dtbl = new DataTable();
                SqlDataAdapter sqlDa = new SqlDataAdapter(Ltx1, cnn);
                sqlDa.Fill(dtbl);                
                if(dtbl.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["ELiminar"] = dtbl.Rows.Count;
                }
                return (dtbl.Rows.Count > 0);
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}