using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class CsTypeReferencia
    {
        ClsConexion Cnx = new ClsConexion();
        static public string VblAccion;
        public string CodReferencia { get; set; }
        public int TipoGo { get; set; }
        public int TipoNoGo { get; set; }
        public string Descripcion { get; set; }
        public string DescripEsp { get; set; }
        public string Usu { get; set; }
        public string CodTipoCodigo { get; set; }
        public int Reparable { get; set; }
        public string CodTipoElemento { get; set; }
        public int IdCia { get; set; }
        public float StockMin { get; set; }
        public string TipoAnt { get; set; }
        public DateTime? FechaCambioTipo { get; set; }
        public string UsuarioModiTipo { get; set; }
        public string CodUndMedR { get; set; }
        public string CodKitR { get; set; }
        public int ConsumoR { get; set; }
        public int MotorR { get; set; }
        public string IdentificadorElemR { get; set; }
        public string CodcapituloR { get; set; }
        public int SubComponenteR { get; set; }
        public int ComponenteMayorR { get; set; }
        public int PosicionPnR { get; set; }
        public int APU { get; set; }
        public string ManipulacionEmpaque { get; set; }
        public int FechaVencimientoR { get; set; }
        public int Revisado { get; set; }
        public string CodCategoria { get; set; }
        public int Calibracion { get; set; }
        public string ModeloRef { get; set; }
        public int ActivoNIF { get; set; }
        public string SP_StockMin { get; set; }
        public string CodModeloR { get; set; }
        public void Accion(string VbAccion)
        {
            VblAccion = VbAccion;
        }
        public void Insert(IEnumerable<CsTypeReferencia> TblReferencia)
        {
            DataTable table = new DataTable();

            table.Columns.Add("CodReferencia", typeof(string));
            table.Columns.Add("TipoGo", typeof(int));
            table.Columns.Add("TipoNoGo", typeof(int));
            table.Columns.Add("Descripcion", typeof(string));
            table.Columns.Add("DescripEsp", typeof(string));
            table.Columns.Add("Usu", typeof(string));
            table.Columns.Add("CodTipoCodigo", typeof(string));
            table.Columns.Add("Reparable", typeof(int));
            table.Columns.Add("CodTipoElemento", typeof(string));
            table.Columns.Add("IdCia", typeof(int));
            table.Columns.Add("StockMin", typeof(float));
            table.Columns.Add("TipoAnt", typeof(string));
            table.Columns.Add("FechaCambioTipo", typeof(DateTime));
            table.Columns.Add("UsuarioModiTipo", typeof(string));
            table.Columns.Add("CodUndMedR", typeof(string));
            table.Columns.Add("CodKitR", typeof(string));
            table.Columns.Add("ConsumoR", typeof(int));
            table.Columns.Add("MotorR", typeof(int));
            table.Columns.Add("IdentificadorElemR", typeof(string));
            table.Columns.Add("CodcapituloR", typeof(string));
            table.Columns.Add("SubComponenteR", typeof(int));
            table.Columns.Add("ComponenteMayorR", typeof(int));
            table.Columns.Add("PosicionPnR", typeof(int));
            table.Columns.Add("APU", typeof(int));
            table.Columns.Add("FechaVencimientoR", typeof(int));
            table.Columns.Add("Revisado", typeof(int));
            table.Columns.Add("CodCategoria", typeof(string));
            table.Columns.Add("Calibracion", typeof(int));
            table.Columns.Add("ModeloRef", typeof(string));
            table.Columns.Add("ActivoNIF", typeof(int));
            table.Columns.Add("SP_StockMin", typeof(string));
            table.Columns.Add("CodModeloR", typeof(string));

            foreach (var Campos in TblReferencia)
            {
                table.Rows.Add(new object[]
                    {
                        Campos.CodReferencia,
                        Campos.TipoGo,
                        Campos.TipoNoGo,
                        Campos.Descripcion,
                        Campos.DescripEsp,
                        Campos.Usu,
                        Campos.CodTipoCodigo,
                        Campos.Reparable,
                        Campos.CodTipoElemento,
                        Campos.IdCia,
                        Campos.StockMin,
                        Campos.TipoAnt,
                        Campos.FechaCambioTipo,
                        Campos.UsuarioModiTipo,
                        Campos.CodUndMedR,
                        Campos.CodKitR,
                        Campos.ConsumoR,
                        Campos.MotorR,
                        Campos.IdentificadorElemR,
                        Campos.CodcapituloR,
                        Campos.SubComponenteR,
                        Campos.ComponenteMayorR,
                        Campos.PosicionPnR,
                        Campos.APU,
                        Campos.FechaVencimientoR,
                        Campos.Revisado,
                        Campos.CodCategoria,
                        Campos.Calibracion,
                        Campos.ModeloRef,
                        Campos.ActivoNIF,
                        Campos.SP_StockMin,
                        Campos.CodModeloR,
                    });
            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery;
                    if (VblAccion == "INSERT")
                    { VBQuery = "SP_Insert_Referencia"; }
                    else
                    { VBQuery = "SP_Update_Referencia"; }
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurVD", table);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            sqlCmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}