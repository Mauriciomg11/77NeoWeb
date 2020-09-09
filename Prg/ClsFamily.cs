using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;

namespace _77NeoWeb.Prg
{
    public class ClsFamily
    {
        ClsConexion Cnx = new ClsConexion();
        public int IDEnc { get; set; }
        public string NomMama { get; set; }
        public string NomPapa { get; set; }
        public string CRUD { get; set; }
        public int IdDet { get; set; }
        public string NomHijos { get; set; }
        public int Edad { get; set; }

        public void Alimentar(IEnumerable<ClsFamily> Tblencabezado, IEnumerable<ClsFamily> TblDetalle)
        {
            DataTable Tblenc = new DataTable();

            Tblenc.Columns.Add("IDEnc", typeof(int));
            Tblenc.Columns.Add("NomMama", typeof(string));
            Tblenc.Columns.Add("NomPapa", typeof(string));
            Tblenc.Columns.Add("CRUD", typeof(string));

            DataTable TblDet = new DataTable();

            TblDet.Columns.Add("IdDet", typeof(int));
            TblDet.Columns.Add("NomHijos", typeof(string));
            TblDet.Columns.Add("Edad", typeof(int));

            foreach (var Campos in Tblencabezado)
            {
                Tblenc.Rows.Add(new object[]
                    {
                        Campos.IDEnc,
                        Campos.NomMama,
                        Campos.NomPapa,
                        Campos.CRUD,
                    });
            }
            foreach (var CamposD in TblDetalle)
            {
                TblDet.Rows.Add(new object[]
                    {
                        CamposD.IdDet,
                         CamposD.NomHijos,
                        CamposD.Edad,
                    });

            }
            Cnx.SelecBD();
            using (SqlConnection sqlCon = new SqlConnection(Cnx.GetConex()))
            {
                sqlCon.Open();
                using (SqlTransaction transaction = sqlCon.BeginTransaction())
                {
                    string VBQuery = "NuevaFamilia";
                    using (SqlCommand sqlCmd = new SqlCommand(VBQuery, sqlCon, transaction))
                    {
                        try
                        {
                            //SqlParameter[] Prmtrs = new SqlParameter[2];
                            //SqlParameter[] param = new SqlParameter[1];
                            //param[0] = new SqlParameter("@estado", SqlDbType.Char);
                            //param[0].Value = "CA";
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter Prmtrs = sqlCmd.Parameters.AddWithValue("@CurEnc", Tblenc);
                            SqlParameter PrmtrsD = sqlCmd.Parameters.AddWithValue("@CurDet", TblDet);
                            //sqlCmd.Parameters.Add(Prmtrs);
                            //sqlCmd.Parameters.Add(PrmtrsD);
                            Prmtrs.SqlDbType = SqlDbType.Structured;
                            PrmtrsD.SqlDbType = SqlDbType.Structured;
                            sqlCmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            string VbUsu, VbPantalla, VbcatVer, VbcatAct;
                            VbUsu = System.Web.HttpContext.Current.Session["C77U"].ToString();
                            VbPantalla = "FrmServicioManto";
                            VbcatVer = System.Web.HttpContext.Current.Session["77Version"].ToString();
                            VbcatAct = System.Web.HttpContext.Current.Session["77Act"].ToString();
                            Cnx.UpdateErrorV2(VbUsu, VbPantalla, "CsTypContaSrvMant", Ex.StackTrace.Substring(Ex.StackTrace.Length - 300, 300), Ex.Message, VbcatVer, VbcatAct);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}