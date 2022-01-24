using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace _77NeoWeb.Prg.PrgLogistica
{
    public class ClsTypTercero
    {
        ClsConexion Cnx = new ClsConexion();
        static public string PMensj, PIdTercero, PCodTercero, VbAccion;
        public string CodTercero { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Correo { get; set; }
        public int CodUbicaGeogr { get; set; }
        public int Estado { get; set; }
        public string Comentario { get; set; }
        public string CodClaseServicio { get; set; }
        public string CodTipoPago { get; set; }
        public string CodTipo { get; set; }
        public string CodTipoProveedor { get; set; }
        public string DireccionPago { get; set; }
        public string CodClasiJuridic { get; set; }
        public string CodTipoRegimen { get; set; }
        public string PrecioFacturacion { get; set; }
        public double Cupo { get; set; }
        public string CodMoneda { get; set; }
        public string CodTipoIdent { get; set; }
        public double Descuento { get; set; }
        public int DiasDescuento { get; set; }
        public string PagoA { get; set; }
        public string Clasificacion { get; set; }
        public string CuentaPuc { get; set; }
        public string CuentaPucProveedor { get; set; }
        public string Empleado { get; set; }
        public string Identificacion { get; set; }
        public int Activo { get; set; }
        public string DigVerificacion { get; set; }
        public string CodPostal { get; set; }
        public string Pais { get; set; }
        public string NroDeCta { get; set; }
        public int ClaseCta { get; set; }
        public string BcoBeneficiario { get; set; }
        public string BcoCorresponsal { get; set; }
        public string SwiftCode { get; set; }
        public string ABA { get; set; }
        public string NomContacto { get; set; }
        public string ApeContacto { get; set; }
        public string TipoPagoBanco { get; set; }
        public string CodTipoCodigo { get; set; }
        public string CodClaseProv { get; set; }
        public double IVA { get; set; }
        public int IdTercero { get; set; }

        public void Accion(string Accion)
        { VbAccion = Accion; }
        public void Alimentar(IEnumerable<ClsTypTercero> TypTercero)
        {
            DataTable TblTercero = new DataTable();
            TblTercero.Columns.Add("CodTercero", typeof(string));
            TblTercero.Columns.Add("RazonSocial", typeof(string));
            TblTercero.Columns.Add("Direccion", typeof(string));
            TblTercero.Columns.Add("Telefono", typeof(string));
            TblTercero.Columns.Add("Fax", typeof(string));
            TblTercero.Columns.Add("Correo", typeof(string));
            TblTercero.Columns.Add("CodUbicaGeogr", typeof(int));
            TblTercero.Columns.Add("Estado", typeof(int));
            TblTercero.Columns.Add("Comentario", typeof(string));
            TblTercero.Columns.Add("CodClaseServicio", typeof(string));
            TblTercero.Columns.Add("CodTipoPago", typeof(string));
            TblTercero.Columns.Add("CodTipo", typeof(string));
            TblTercero.Columns.Add("CodTipoProveedor", typeof(string));
            TblTercero.Columns.Add("DireccionPago", typeof(string));
            TblTercero.Columns.Add("CodClasiJuridic", typeof(string));
            TblTercero.Columns.Add("CodTipoRegimen", typeof(string));
            TblTercero.Columns.Add("PrecioFacturacion", typeof(string));
            TblTercero.Columns.Add("Cupo", typeof(double));
            TblTercero.Columns.Add("CodMoneda", typeof(string));
            TblTercero.Columns.Add("CodTipoIdent", typeof(string));
            TblTercero.Columns.Add("Descuento", typeof(double));
            TblTercero.Columns.Add("DiasDescuento", typeof(int));
            TblTercero.Columns.Add("PagoA", typeof(string));
            TblTercero.Columns.Add("Clasificacion", typeof(string));
            TblTercero.Columns.Add("CuentaPuc", typeof(string));
            TblTercero.Columns.Add("CuentaPucProveedor", typeof(string));
            TblTercero.Columns.Add("Empleado", typeof(string));
            TblTercero.Columns.Add("Identificacion", typeof(string));
            TblTercero.Columns.Add("Activo", typeof(int));
            TblTercero.Columns.Add("DigVerificacion", typeof(string));
            TblTercero.Columns.Add("CodPostal", typeof(string));
            TblTercero.Columns.Add("Pais", typeof(string));
            TblTercero.Columns.Add("NroDeCta", typeof(string));
            TblTercero.Columns.Add("ClaseCta", typeof(int));
            TblTercero.Columns.Add("BcoBeneficiario", typeof(string));
            TblTercero.Columns.Add("BcoCorresponsal", typeof(string));
            TblTercero.Columns.Add("SwiftCode", typeof(string));
            TblTercero.Columns.Add("ABA", typeof(string));
            TblTercero.Columns.Add("NomContacto", typeof(string));
            TblTercero.Columns.Add("ApeContacto", typeof(string));
            TblTercero.Columns.Add("TipoPagoBanco", typeof(string));
            TblTercero.Columns.Add("CodTipoCodigo", typeof(string));
            TblTercero.Columns.Add("CodClaseProv", typeof(string));
            TblTercero.Columns.Add("IVA", typeof(double));
            TblTercero.Columns.Add("IdTercero", typeof(int));

            foreach (var Campo in TypTercero)
            {
                TblTercero.Rows.Add(new object[]{
                Campo.CodTercero,
                Campo.RazonSocial,
                Campo.Direccion,
                Campo.Telefono,
                Campo.Fax,
                Campo.Correo,
                Campo.CodUbicaGeogr,
                Campo.Estado,
                Campo.Comentario,
                Campo.CodClaseServicio,
                Campo.CodTipoPago,
                Campo.CodTipo,
                Campo.CodTipoProveedor,
                Campo.DireccionPago,
                Campo.CodClasiJuridic,
                Campo.CodTipoRegimen,
                Campo.PrecioFacturacion,
                Campo.Cupo,
                Campo.CodMoneda,
                Campo.CodTipoIdent,
                Campo.Descuento,
                Campo.DiasDescuento,
                Campo.PagoA,
                Campo.Clasificacion,
                Campo.CuentaPuc,
                Campo.CuentaPucProveedor,
                Campo.Empleado,
                Campo.Identificacion,
                Campo.Activo,
                Campo.DigVerificacion,
                Campo.CodPostal,
                Campo.Pais,
                Campo.NroDeCta,
                Campo.ClaseCta,
                Campo.BcoBeneficiario,
                Campo.BcoCorresponsal,
                Campo.SwiftCode,
                Campo.ABA,
                Campo.NomContacto,
                Campo.ApeContacto,
                Campo.TipoPagoBanco,
                Campo.CodTipoCodigo,
                Campo.CodClaseProv,
                Campo.IVA,
                Campo.IdTercero,
                 });
            }

            Cnx.SelecBD();
            using (SqlConnection SCX = new SqlConnection(Cnx.GetConex()))
            {
                SCX.Open();
                using (SqlTransaction transaction = SCX.BeginTransaction())
                {
                    string VBQuery = "INS_UPD_Tercero";
                    using (SqlCommand SC = new SqlCommand(VBQuery, SCX, transaction))
                    {
                        try
                        {
                            PIdTercero = "";
                            PCodTercero = "";
                            PMensj = "";
                            SC.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = SC.Parameters.AddWithValue("@Tercero", TblTercero);
                            SqlParameter Prmtrs3 = SC.Parameters.AddWithValue("@IdConfigCia", HttpContext.Current.Session["!dC!@"].ToString());
                            SqlParameter Prmtrs4 = SC.Parameters.AddWithValue("@Accion", VbAccion);
                            SqlParameter Prmtrs5 = SC.Parameters.AddWithValue("@Usu", HttpContext.Current.Session["C77U"].ToString());
                            SqlParameter Prmtrs6 = SC.Parameters.AddWithValue("@NIT", HttpContext.Current.Session["Nit77Cia"].ToString());
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            SqlDataReader SDR = SC.ExecuteReader();
                            if (SDR.Read())
                            {
                                PMensj = HttpUtility.HtmlDecode(SDR["Mensj"].ToString().Trim());
                                PIdTercero = HttpUtility.HtmlDecode(SDR["IdTercero"].ToString().Trim());
                                PCodTercero = HttpUtility.HtmlDecode(SDR["CodTercero"].ToString().Trim());
                            }
                            SDR.Close();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "Generar Tercero";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "ClsTypTercero", Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
        public string GetMensj()
        { return PMensj; }

        public string GetPIdTercero()
        { return PIdTercero; }

        public string GetPCodTercero()
        { return PCodTercero; }
    }
}