using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypDetalleLibroVuelo
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj;
        static public string PTTHLV,PHrMn;
        static public int PTtlVuelos;
        public int CodIdDetLibroVuelo { get; set; }
        public string CodLibroVuelo { get; set; }
        public string CodOrigen { get; set; }
        public DateTime HoraSalida { get; set; }
        public string CodDestino { get; set; }
        public DateTime HoraLlegada { get; set; }
        public string CodTipoVuelo { get; set; }
        public int NumPersTransp { get; set; }
        public double NumHoraCiclo { get; set; }
        public int Generado { get; set; }
        public string NroVuelo { get; set; }
        public DateTime HoraDespegue { get; set; }
        public DateTime HoraAterrizaje { get; set; }
        public string TiempoVuelo { get; set; }
        public string Usu { get; set; }
        public string HoraAPU { get; set; }
        public string Accion { get; set; }
        public void Alimentar(IEnumerable<CsTypDetalleLibroVuelo> DetLibroVuelo)//
        {
            DataTable TblDetLibroVuelo = new DataTable();
            TblDetLibroVuelo.Columns.Add("CodIdDetLibroVuelo", typeof(int));
            TblDetLibroVuelo.Columns.Add("CodLibroVuelo", typeof(string));
            TblDetLibroVuelo.Columns.Add("CodOrigen", typeof(string));
            TblDetLibroVuelo.Columns.Add("HoraSalida", typeof(DateTime));
            TblDetLibroVuelo.Columns.Add("CodDestino", typeof(string));
            TblDetLibroVuelo.Columns.Add("HoraLlegada", typeof(DateTime));
            TblDetLibroVuelo.Columns.Add("CodTipoVuelo", typeof(string));
            TblDetLibroVuelo.Columns.Add("NumPersTransp", typeof(int));
            TblDetLibroVuelo.Columns.Add("NumHoraCiclo", typeof(double));
            TblDetLibroVuelo.Columns.Add("Generado", typeof(int));
            TblDetLibroVuelo.Columns.Add("NroVuelo", typeof(string));
            TblDetLibroVuelo.Columns.Add("HoraDespegue", typeof(DateTime));
            TblDetLibroVuelo.Columns.Add("HoraAterrizaje", typeof(DateTime));
            TblDetLibroVuelo.Columns.Add("TiempoVuelo", typeof(string));
            TblDetLibroVuelo.Columns.Add("Usu", typeof(string));
            TblDetLibroVuelo.Columns.Add("HoraAPU", typeof(string));
            TblDetLibroVuelo.Columns.Add("Accion", typeof(string));
            foreach (var Campos in DetLibroVuelo)
            {
                TblDetLibroVuelo.Rows.Add(new object[]
                {
                Campos.CodIdDetLibroVuelo,
                Campos.CodLibroVuelo,
                Campos.CodOrigen,
                Campos.HoraSalida,
                Campos.CodDestino,
                Campos.HoraLlegada,
                Campos.CodTipoVuelo,
                Campos.NumPersTransp,
                Campos.NumHoraCiclo,
                Campos.Generado,
                Campos.NroVuelo,
                Campos.HoraDespegue,
                Campos.HoraAterrizaje,
                Campos.TiempoVuelo,
                Campos.Usu,
                Campos.HoraAPU,
                Campos.Accion,
                });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = "";
                    PTTHLV = "";
                    string VBQuery = "INSERT_UPDATE_DetLibroVuelo";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurDetLV", TblDetLibroVuelo);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            //sqlCmd.ExecuteNonQuery();
                            //PMensj = (string)sqlCmd.ExecuteScalar();

                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PTTHLV = SDR["TTH"].ToString();
                                PHrMn = HttpUtility.HtmlDecode(SDR["Hr_Mn"].ToString().Trim());
                                PTtlVuelos = Convert.ToInt32(SDR["TtlVuelos"].ToString());
                            }
                            SDR.Close();
                            transaction.Commit();
                            sqlCon.Close();

                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmLibroVueloAC";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypDetalleLibroVuelo", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetTtlHorasLV()
        {
            return PTTHLV;
        }
        public string GetTHrMn()
        {
            return PHrMn;
        }
        public int GetTtlVuelos()
        {
            return PTtlVuelos;
        }
        
    }
}