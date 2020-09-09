﻿using System;
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
        string VblConexion, VblDecimal;
        public ClsConexion()
        {
            this.VblConexion = "";
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
                this.VblConexion = string.Format(ConfigurationManager.ConnectionStrings["PConexDB"].ConnectionString, VblNomSrv, VbNomBD, VbUsu, VblPass);
            }
        }
        public string GetConex()
        {
            return this.VblConexion;
        }
        public void RetirarPuntos(string VbCampo)
        {           
            int I = VbCampo.IndexOf(",") == -1 ? 0 : VbCampo.IndexOf(",");
            if (I > 0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, ".").Replace(",", ""); }         
            I = VbCampo.IndexOf(".");
            if (I > 0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, ",").Replace(".", ""); }
            else if (I ==0)
            { VbCampo = VbCampo.Remove(I, 1).Insert(I, "0,").Replace(".", ""); }
            this.VblDecimal = VbCampo;
        }
        public string ValorDecimal()
        {
            return this.VblDecimal;
        }
    }
}