﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class ClsTypAeronaveVirtual
    {
        static public string PMensj;

        public string TipoEvento { get; set; }
        public int CodAeronave { get; set; }
        public string NivelElemento { get; set; }
        public string UltimoNivel { get; set; }
        public string CodMayor { get; set; }
        public string CodElemento { get; set; }
        public string Pn { get; set; }
        public string Sn { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Posicion { get; set; }
        public string Usu { get; set; }
        public string MotivoRemocion { get; set; }

        //-------------  Servicios --------------------
        public int CodIdContadorElem { get; set; }
        public string CodElementoSvc { get; set; }        
        public DateTime? FechaVence { get; set; }
        public DateTime? FechaVenceAnt { get; set; }
        public int Resetear { get; set; }
        public int CodOT { get; set; }
        public int CodIdContaSrvManto { get; set; }
        public string NumReporte { get; set; }
        public double ValorUltCump { get; set; }

        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypAeronaveVirtual> AeronaveVirtual, IEnumerable<ClsTypAeronaveVirtual> ServicioManto)
        {
            DataTable TblAeronaveVirtual = new DataTable();
            TblAeronaveVirtual.Columns.Add("TipoEvento", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodAeronave", typeof(int));
            TblAeronaveVirtual.Columns.Add("NivelElemento", typeof(string));
            TblAeronaveVirtual.Columns.Add("UltimoNivel", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodMayor", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodElemento", typeof(string));
            TblAeronaveVirtual.Columns.Add("Pn", typeof(string));
            TblAeronaveVirtual.Columns.Add("Sn", typeof(string));
            TblAeronaveVirtual.Columns.Add("FechaEvento", typeof(DateTime));
            TblAeronaveVirtual.Columns.Add("Posicion", typeof(string));
            TblAeronaveVirtual.Columns.Add("Usu", typeof(string));
            TblAeronaveVirtual.Columns.Add("MotivoRemocion", typeof(string));


            foreach (var Campo in AeronaveVirtual)
            {
                TblAeronaveVirtual.Rows.Add(new object[]{
                    Campo.TipoEvento,
                    Campo.CodAeronave,
                    Campo.NivelElemento,
                    Campo.UltimoNivel,
                    Campo.CodMayor,
                    Campo.CodElemento,
                    Campo.Pn,
                    Campo.Sn,
                    Campo.FechaEvento,
                    Campo.Posicion,
                    Campo.Usu,
                    Campo.MotivoRemocion,
                });
            }

            DataTable TblServicios = new DataTable();

            TblServicios.Columns.Add("CodIdContadorElem", typeof(int));
            TblServicios.Columns.Add("CodElementoSvc", typeof(string));
            TblServicios.Columns.Add("FechaVence", typeof(DateTime));
            TblServicios.Columns.Add("FechaVenceAnt", typeof(DateTime));
            TblServicios.Columns.Add("Resetear", typeof(int));
            TblServicios.Columns.Add("CodOT", typeof(int));
            TblServicios.Columns.Add("CodIdContaSrvManto", typeof(int));
            TblServicios.Columns.Add("NumReporte", typeof(string));
            TblServicios.Columns.Add("ValorUltCump", typeof(double));



            foreach (var CampoSvc in ServicioManto)
            {
                TblServicios.Rows.Add(new object[] {
                    CampoSvc.CodIdContadorElem,
                    CampoSvc.CodElementoSvc,
                    CampoSvc.FechaVence,
                    CampoSvc.FechaVenceAnt,
                    CampoSvc.Resetear,
                    CampoSvc.CodOT,
                    CampoSvc.CodIdContaSrvManto,
                    CampoSvc.NumReporte,
                    CampoSvc.ValorUltCump,
                });
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_AeroVirtual";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurAeroVirtual", TblAeronaveVirtual);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@CurServManto", TblServicios);
                            Prmtrs.SqlDbType = SqlDbType.Structured;                          
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                string mod= SDR["Modelo"].ToString().Trim();
                                string UN = SDR["UN"].ToString().Trim();
                                string CodRef = SDR["CodRef"].ToString().Trim();                                
                                string NivelSuperior = SDR["NivelSuperior"].ToString().Trim();
                                string Borrar = SDR["Borrar"].ToString().Trim();
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmAeronaveVirtual";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypLvDetalleManto", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }

        }
        public string GetMensj()
        {
            return PMensj;
        }       
    }
}