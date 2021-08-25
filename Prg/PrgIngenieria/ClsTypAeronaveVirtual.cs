using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgIngenieria
{
    public class ClsTypAeronaveVirtual
    {
        static public string PMensj;
        static public string borrar;
        public string TipoEvento { get; set; }
        public int CodAeronave { get; set; }
        public string CodModelo { get; set; }
        public string NivelElemento { get; set; }
        public string Motor { get; set; }
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
        public string GeneraHist { get; set; }

        //-------------  Compensacion --------------------

        public int ID { get; set; }
        public int OK { get; set; }
        public string CodlibroVuelo { get; set; }
        public DateTime FechaLibroVuelo { get; set; }
        public DateTime HoraDespegue { get; set; }
        public int CompensInicioDia { get; set; }
        public double HorasAcum { get; set; }
        public double CiclosAcum { get; set; }
        public double HorasRemain { get; set; }
        public double CiclosRemain { get; set; }
        public string TipoComponente { get; set; }
        public string PosicionCE { get; set; }
        public string Compensacion { get; set; }

        //-------------  Trabajo --------------------
        public int CodNumOrdenTrab { get; set; }
        public string Descripcion { get; set; }
        public string CodEstOrdTrab1 { get; set; }
        public string CodEstOrdTrab2 { get; set; }
        public string Aplicabilidad { get; set; }
        public string CodCapitulo { get; set; }
        public string CodUbicaTecn { get; set; }
        public string CodBase { get; set; }
        public string CodTaller { get; set; }
        public string CodPlanManto { get; set; }
        public string CentroCosto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public DateTime? FechaReg { get; set; }
        public int IdentificadorCorrPrev { get; set; }
        public string CodPrioridad { get; set; }
        public int CodIdLvDetManto { get; set; }
        public int CodIdDetSrvManto { get; set; }
        public int BanCerrado { get; set; }
        public int HorasProyectadas { get; set; }
        public DateTime? FechaProyectada { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string UsuOT { get; set; }
        public string Referencia { get; set; }
        public string AccionParcial { get; set; }
        public string CodTipoCodigo { get; set; }
        public string CodInspectorCierre { get; set; }
        public string LicenciaInspCierre { get; set; }
        public string PNOT { get; set; }
        public int BloquearDetalle { get; set; }
        public string CodResponsable { get; set; }
        public double OTSN { get; set; }
        public double OTSO { get; set; }
        public double OTSR { get; set; }
        public double OCSN { get; set; }
        public double OCSO { get; set; }
        public double OCSR { get; set; }
        public int EjecPasos { get; set; }
        public int CancelOT { get; set; }
        public string WS { get; set; }
        public int OKOT { get; set; }
        public string AccionOT { get; set; }



        ClsConexion Cnx = new ClsConexion();
        public void Alimentar(IEnumerable<ClsTypAeronaveVirtual> AeronaveVirtual, IEnumerable<ClsTypAeronaveVirtual> ServicioManto, IEnumerable<ClsTypAeronaveVirtual> Compensacion, IEnumerable<ClsTypAeronaveVirtual> OrdenTrabajo)
        {
            DataTable TblAeronaveVirtual = new DataTable();
            TblAeronaveVirtual.Columns.Add("TipoEvento", typeof(string));
            TblAeronaveVirtual.Columns.Add("CodAeronave", typeof(int));
            TblAeronaveVirtual.Columns.Add("CodModelo", typeof(string));
            TblAeronaveVirtual.Columns.Add("NivelElemento", typeof(string));
            TblAeronaveVirtual.Columns.Add("Motor", typeof(string));
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
                    Campo.CodModelo,
                    Campo.NivelElemento,
                    Campo.Motor,
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
            TblServicios.Columns.Add("GeneraHist", typeof(string));

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
                    CampoSvc.GeneraHist,
                });
            }

            DataTable TblCompensacion = new DataTable();
            TblCompensacion.Columns.Add("ID", typeof(int));
            TblCompensacion.Columns.Add("OK", typeof(int));
            TblCompensacion.Columns.Add("CodlibroVuelo", typeof(string));
            TblCompensacion.Columns.Add("FechaLibroVuelo", typeof(DateTime));
            TblCompensacion.Columns.Add("HoraDespegue", typeof(DateTime));
            TblCompensacion.Columns.Add("CompensInicioDia", typeof(int));
            TblCompensacion.Columns.Add("HorasAcum", typeof(double));
            TblCompensacion.Columns.Add("CiclosAcum", typeof(double));
            TblCompensacion.Columns.Add("HorasRemain", typeof(double));
            TblCompensacion.Columns.Add("CiclosRemain", typeof(double));
            TblCompensacion.Columns.Add("TipoComponente", typeof(string));
            TblCompensacion.Columns.Add("PosicionCE", typeof(string));
            TblCompensacion.Columns.Add("Compensacion", typeof(string));
            foreach (var CampoCmpsc in Compensacion)
            {
                TblCompensacion.Rows.Add(new object[] {
                   CampoCmpsc.ID,
                   CampoCmpsc.OK,
                   CampoCmpsc.CodlibroVuelo,
                   CampoCmpsc.FechaLibroVuelo,
                   CampoCmpsc.HoraDespegue,
                   CampoCmpsc.CompensInicioDia,
                   CampoCmpsc.HorasAcum,
                   CampoCmpsc.CiclosAcum,
                   CampoCmpsc.HorasRemain,
                   CampoCmpsc.CiclosRemain,
                   CampoCmpsc.TipoComponente,
                   CampoCmpsc.PosicionCE,
                   CampoCmpsc.Compensacion,
                });
            }

            DataTable TblORdenTrabajo = new DataTable();
            TblORdenTrabajo.Columns.Add("CodNumOrdenTrab", typeof(int));
            TblORdenTrabajo.Columns.Add("Descripcion", typeof(string));
            TblORdenTrabajo.Columns.Add("CodEstOrdTrab1", typeof(string));
            TblORdenTrabajo.Columns.Add("CodEstOrdTrab2", typeof(string));
            TblORdenTrabajo.Columns.Add("Aplicabilidad", typeof(string));
            TblORdenTrabajo.Columns.Add("CodCapitulo", typeof(string));
            TblORdenTrabajo.Columns.Add("CodUbicaTecn", typeof(string));
            TblORdenTrabajo.Columns.Add("CodBase", typeof(string));
            TblORdenTrabajo.Columns.Add("CodTaller", typeof(string));
            TblORdenTrabajo.Columns.Add("CodPlanManto", typeof(string));
            TblORdenTrabajo.Columns.Add("CentroCosto", typeof(string));
            TblORdenTrabajo.Columns.Add("FechaInicio", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaFinal", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaReg", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("IdentificadorCorrPrev", typeof(int));
            TblORdenTrabajo.Columns.Add("CodPrioridad", typeof(string));
            TblORdenTrabajo.Columns.Add("CodIdLvDetManto", typeof(int));
            TblORdenTrabajo.Columns.Add("CodIdDetSrvManto", typeof(int));
            TblORdenTrabajo.Columns.Add("BanCerrado", typeof(int));
            TblORdenTrabajo.Columns.Add("HorasProyectadas", typeof(int));
            TblORdenTrabajo.Columns.Add("FechaProyectada", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("FechaVencimiento", typeof(DateTime));
            TblORdenTrabajo.Columns.Add("UsuOT", typeof(string));
            TblORdenTrabajo.Columns.Add("Referencia", typeof(string));
            TblORdenTrabajo.Columns.Add("AccionParcial", typeof(string));
            TblORdenTrabajo.Columns.Add("CodTipoCodigo", typeof(string));
            TblORdenTrabajo.Columns.Add("CodInspectorCierre", typeof(string));
            TblORdenTrabajo.Columns.Add("LicenciaInspCierre", typeof(string));
            TblORdenTrabajo.Columns.Add("PNOT", typeof(string));
            TblORdenTrabajo.Columns.Add("BloquearDetalle", typeof(int));
            TblORdenTrabajo.Columns.Add("CodResponsable", typeof(string));
            TblORdenTrabajo.Columns.Add("OTSN", typeof(double));
            TblORdenTrabajo.Columns.Add("OTSO", typeof(double));
            TblORdenTrabajo.Columns.Add("OTSR", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSN", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSO", typeof(double));
            TblORdenTrabajo.Columns.Add("OCSR", typeof(double));
            TblORdenTrabajo.Columns.Add("EjecPasos", typeof(int));
            TblORdenTrabajo.Columns.Add("CancelOT", typeof(int));
            TblORdenTrabajo.Columns.Add("WS", typeof(string));
            TblORdenTrabajo.Columns.Add("OKOT", typeof(int));
            TblORdenTrabajo.Columns.Add("AccionOT", typeof(string));
            foreach (var CampoOT in OrdenTrabajo)
            {
                TblORdenTrabajo.Rows.Add(new object[] {
                    CampoOT.CodNumOrdenTrab,
                    CampoOT.Descripcion,
                    CampoOT.CodEstOrdTrab1,
                    CampoOT.CodEstOrdTrab2,
                    CampoOT.Aplicabilidad,
                    CampoOT.CodCapitulo,
                    CampoOT.CodUbicaTecn,
                    CampoOT.CodBase,
                    CampoOT.CodTaller,
                    CampoOT.CodPlanManto,
                    CampoOT.CentroCosto,
                    CampoOT.FechaInicio,
                    CampoOT.FechaFinal,
                    CampoOT.FechaReg,
                    CampoOT.IdentificadorCorrPrev,
                    CampoOT.CodPrioridad,
                    CampoOT.CodIdLvDetManto,
                    CampoOT.CodIdDetSrvManto,
                    CampoOT.BanCerrado,
                    CampoOT.HorasProyectadas,
                    CampoOT.FechaProyectada,
                    CampoOT.FechaVencimiento,
                    CampoOT.UsuOT,
                    CampoOT.Referencia,
                    CampoOT.AccionParcial,
                    CampoOT.CodTipoCodigo,
                    CampoOT.CodInspectorCierre,
                    CampoOT.LicenciaInspCierre,
                    CampoOT.PNOT,
                    CampoOT.BloquearDetalle,
                    CampoOT.CodResponsable,
                    CampoOT.OTSN,
                    CampoOT.OTSO,
                    CampoOT.OTSR,
                    CampoOT.OCSN,
                    CampoOT.OCSO,
                    CampoOT.OCSR,
                    CampoOT.EjecPasos,
                    CampoOT.CancelOT,
                    CampoOT.WS,
                    CampoOT.OKOT,
                    CampoOT.AccionOT,
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
                        PMensj = "Inconveniente en el movimiento";
                        try
                        {
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@CurAeroVirtual", TblAeronaveVirtual);
                            SqlParameter Prmtrs2 = SC.Parameters.AddWithValue("@CurServManto", TblServicios);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@CurCompensac", TblCompensacion);
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@CurOT", TblORdenTrabajo);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                string mod = SDR["Modelo"].ToString().Trim();
                                string UN = SDR["UN"].ToString().Trim();
                                string CodRef = SDR["CodRef"].ToString().Trim();
                                string NivelSuperior = SDR["NivelSuperior"].ToString().Trim();
                                string MensjServicios = SDR["MensjServicios"].ToString().Trim();
                                string MensjCompensac = SDR["MensjCompensac"].ToString().Trim();
                                string MensjCorrCont = SDR["MensjCorrCont"].ToString().Trim();
                                string MensjOT = SDR["MensjOT"].ToString().Trim();
                                string MensInsMayor = SDR["MensInsMayor"].ToString().Trim();
                                string MensjCorrerContSubC = SDR["MensjCorrerContSubC"].ToString().Trim();
                                borrar = SDR["MensRemMayor"].ToString().Trim();
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
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypLvDetalleManto", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
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
        public string GetBorrar()
        {
            return borrar;
        }
    }
}