using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class CsTypLibroVuelo
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj, PCodLV;
        public int IdLibroVuelo { get; set; }
        public string CodLibroVuelo { get; set; }
        public string CodLvAnt { get; set; }
        public DateTime FechaReporte { get; set; }
        public int CodAeronave { get; set; }
        public string CodBase { get; set; }
        public string Comentario { get; set; }
        public int TotalPasSal { get; set; }
        public string Realizado { get; set; }
        public int PAlt { get; set; }
        public int Kias { get; set; }
        public int Oat { get; set; }
        public int GW { get; set; }
        public int TAT { get; set; }
        public int MACHS { get; set; }
        public string HoraInicial { get; set; }
        public double HoraFinal { get; set; }
        public double Horometro { get; set; }
        public string SnAPU { get; set; }
        public int NumLevante { get; set; }
        public string RevisionManto { get; set; }
        public string IdentificadorH { get; set; }
        public double Horas { get; set; }
        public string identificadorV { get; set; }
        public int Vuelos { get; set; }
        public string identificadorL { get; set; }
        public int Levantes { get; set; }
        public int rines { get; set; }
        public string identificadorR { get; set; }
        public int Acentado { get; set; }
        public string Usu { get; set; }
        public int AterrizajeCorrido { get; set; }
        public double EventoDeAutorrotacion { get; set; }
        public double EventoDeSimulacionFallaMotor { get; set; }
        public string Accion { get; set; }

        // ******** Det motor ***************
        public int CodIDLvDetMotor { get; set; }
        public string SN { get; set; }
        public int NumArranque { get; set; }
        public int NII { get; set; }
        public double ITT { get; set; }
        public double NI { get; set; }
        public double TempAceite { get; set; }
        public double PresionAceite { get; set; }
        public int Torque { get; set; }
        public int Posicion { get; set; }
        public int OEI { get; set; }
        public string NroVuelo { get; set; }
        public int SangradoMotor { get; set; }
        public double AceiteAgreMot { get; set; }
        public string GenOnOff { get; set; }
        public string SnAPUDet { get; set; }
        public double AceiteAgreAPU { get; set; }
        public string SnAYD { get; set; }
        public double AceiteAgreAYD { get; set; }
        public int ART { get; set; }
        public double CombVuelo { get; set; }
        public double PresComb { get; set; }
        public double PresHYD { get; set; }
        public double GasGenCycle { get; set; }
        public double PwrTurbineCycle { get; set; }
        public string CodElemMotorLV { get; set; }
        public void Alimentar(IEnumerable<CsTypLibroVuelo> TblLibroVuelo, IEnumerable<CsTypLibroVuelo> TblDetMotor)//
        {
            DataTable table = new DataTable();
            table.Columns.Add("IdLibroVuelo", typeof(int));
            table.Columns.Add("CodLibroVuelo", typeof(string));
            table.Columns.Add("CodLvAnt", typeof(string));
            table.Columns.Add("FechaReporte", typeof(DateTime));
            table.Columns.Add("CodAeronave", typeof(int));
            table.Columns.Add("CodBase", typeof(string));
            table.Columns.Add("Comentario", typeof(string));
            table.Columns.Add("TotalPasSal", typeof(int));
            table.Columns.Add("Realizado", typeof(string));
            table.Columns.Add("PAlt", typeof(int));
            table.Columns.Add("Kias", typeof(int));
            table.Columns.Add("Oat", typeof(int));
            table.Columns.Add("GW", typeof(int));
            table.Columns.Add("TAT", typeof(int));
            table.Columns.Add("MACHS", typeof(int));
            table.Columns.Add("HoraInicial", typeof(string));
            table.Columns.Add("HoraFinal", typeof(double));
            table.Columns.Add("Horometro", typeof(double));
            table.Columns.Add("SnAPU", typeof(string));
            table.Columns.Add("NumLevante", typeof(int));
            table.Columns.Add("RevisionManto", typeof(string));
            table.Columns.Add("IdentificadorH", typeof(string));
            table.Columns.Add("Horas", typeof(double));
            table.Columns.Add("identificadorV", typeof(string));
            table.Columns.Add("Vuelos", typeof(int));
            table.Columns.Add("identificadorL", typeof(string));
            table.Columns.Add("Levantes", typeof(int));
            table.Columns.Add("rines", typeof(int));
            table.Columns.Add("identificadorR", typeof(string));
            table.Columns.Add("Acentado", typeof(int));
            table.Columns.Add("Usu", typeof(string));
            table.Columns.Add("AterrizajeCorrido", typeof(int));
            table.Columns.Add("EventoDeAutorrotacion", typeof(double));
            table.Columns.Add("EventoDeSimulacionFallaMotor", typeof(double));
            table.Columns.Add("Accion", typeof(string));/**/

            foreach (var Campos in TblLibroVuelo)
            {
                table.Rows.Add(new object[]
                {
                Campos.IdLibroVuelo,
                Campos.CodLibroVuelo,
                Campos.CodLvAnt,
                Campos.FechaReporte,
                Campos.CodAeronave,
                Campos.CodBase,
                Campos.Comentario,
                Campos.TotalPasSal,
                Campos.Realizado,
                Campos.PAlt,
                Campos.Kias,
                Campos.Oat,
                Campos.GW,
                Campos.TAT,
                Campos.MACHS,
                Campos.HoraInicial,
                Campos.HoraFinal,
                Campos.Horometro,
                Campos.SnAPU,
                Campos.NumLevante,
                Campos.RevisionManto,
                Campos.IdentificadorH,
                Campos.Horas,
                Campos.identificadorV,
                Campos.Vuelos,
                Campos.identificadorL,
                Campos.Levantes,
                Campos.rines,
                Campos.identificadorR,
                Campos.Acentado,
                Campos.Usu,
                Campos.AterrizajeCorrido,
                Campos.EventoDeAutorrotacion,
                Campos.EventoDeSimulacionFallaMotor,
                Campos.Accion, /**/
                });
            }
            DataTable DetMotor = new DataTable();
            DetMotor.Columns.Add("CodIDLvDetMotor", typeof(int));
            DetMotor.Columns.Add("SN", typeof(string));
            DetMotor.Columns.Add("NumArranque", typeof(int));
            DetMotor.Columns.Add("NII", typeof(int));
            DetMotor.Columns.Add("ITT", typeof(double));
            DetMotor.Columns.Add("NI", typeof(double));
            DetMotor.Columns.Add("TempAceite", typeof(double));
            DetMotor.Columns.Add("PresionAceite", typeof(double));
            DetMotor.Columns.Add("Torque", typeof(int));
            DetMotor.Columns.Add("Posicion", typeof(int));
            DetMotor.Columns.Add("OEI", typeof(int));
            DetMotor.Columns.Add("NroVuelo", typeof(string));
            DetMotor.Columns.Add("SangradoMotor", typeof(int));
            DetMotor.Columns.Add("AceiteAgreMot", typeof(double));
            DetMotor.Columns.Add("GenOnOff", typeof(string));
            DetMotor.Columns.Add("SnAPUDet", typeof(string));
            DetMotor.Columns.Add("AceiteAgreAPU", typeof(double));
            DetMotor.Columns.Add("SnAYD", typeof(string));
            DetMotor.Columns.Add("AceiteAgreAYD", typeof(double));
            DetMotor.Columns.Add("ART", typeof(int));
            DetMotor.Columns.Add("CombVuelo", typeof(double));
            DetMotor.Columns.Add("PresComb", typeof(double));
            DetMotor.Columns.Add("PresHYD", typeof(double));
            DetMotor.Columns.Add("GasGenCycle", typeof(double));
            DetMotor.Columns.Add("PwrTurbineCycle", typeof(double));
            DetMotor.Columns.Add("CodElemMotorLV", typeof(string)); /**/

            foreach (var CamposD in TblDetMotor)
            {
                DetMotor.Rows.Add(new object[]
                {
                   CamposD.CodIDLvDetMotor, CamposD.SN, CamposD.NumArranque, CamposD.NII,
                   CamposD.ITT, CamposD.NI, CamposD.TempAceite, CamposD.PresionAceite,
                   CamposD.Torque, CamposD.Posicion,
                   CamposD.OEI, CamposD.NroVuelo,
                   CamposD.SangradoMotor, CamposD.AceiteAgreMot,
                   CamposD.GenOnOff, CamposD.SnAPUDet,
                   CamposD.AceiteAgreAPU, CamposD.SnAYD,
                   CamposD.AceiteAgreAYD,
                   CamposD.ART,
                   CamposD.CombVuelo,
                   CamposD.PresComb,
                   CamposD.PresHYD,
                   CamposD.GasGenCycle,
                   CamposD.PwrTurbineCycle,
                   CamposD.CodElemMotorLV,
                });
            }   /**/
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    PMensj = "";
                    PCodLV = "";
                    string VBQuery = "INSERT_UPDATE_LibroVuelo";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurEncLV", table);
                            SqlParameter PrmtrsD = sqlCmd.Parameters.AddWithValue("@CurDetMot", DetMotor);

                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            PrmtrsD.SqlDbType = SqlDbType.Structured;
                            //sqlCmd.ExecuteNonQuery();
                            //PMensj = (string)sqlCmd.ExecuteScalar();
                            SqlDataReader SDR = sqlCmd.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PCodLV = HttpUtility.HtmlDecode(SDR["CodLv"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmLibroVueloAC";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypLibroVuelo", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetNewLv()
        {
            return PCodLV;
        }
    }
}