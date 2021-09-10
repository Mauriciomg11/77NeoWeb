using _77NeoWeb.prg;
using _77NeoWeb.Prg.PrgIngenieria;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _77NeoWeb
{
    public partial class FrmImportar : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            BtnExportar.Text = "Exportar ";
            if (Session["C77U"] == null)
            {
                /*Session["C77U"] = ""; */
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbNeoAda"; //DbNeoAda
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = Session["D[BX"];
                 Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                Session["!dC!@"] = 0;
                Session["77IDM"] = "5"; // 4 español | 5 ingles   */
                ViewState["Validar"] = "S";

                /*  Session["C77U"] = "00000082";
                 Session["D[BX"] = "DbNeoDempV2";//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                 Session["$VR"] = "77NEO01";
                 Session["V$U@"] = "sa";
                 Session["P@$"] = "admindemp";
                 Session["N77U"] = Session["D[BX"];
                  Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                 Session["!dC!@"] = 0;
                 Session["77IDM"] = "5"; // 4 español | 5 ingles   */
            }
        }

        protected void BtnImportarV1_Click(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            string FileName = "";
            string conexion = "";
            //string conexion1 = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Asus Pro\Downloads\Reportes.xlsx;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            if (FileUpload1.HasFile)
            {
                FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //conexion = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus Pro\Downloads\{0};Extended Properties='Excel 12.0 Xml;HDR=YES;'", FileName);
            }
            else
            {
                //conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus Pro\Downloads\SubirReserv.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                FileName = "SubirReserv.xlsx";
            }

            // 
            conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Asus Pro\Downloads\" + FileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";

            using (OleDbConnection cnn = new OleDbConnection(conexion))
            {
                cnn.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cnn.Close();

                cnn.Open();
                // string sql = "SELECT *FROM [Tabla$]";
                string sql = "SELECT * From [" + SheetName + "]";

                OleDbCommand command = new OleDbCommand(sql, cnn);
                OleDbDataAdapter DA = new OleDbDataAdapter(command);

                DA.Fill(DT);
                if (DT.Rows.Count > 0)
                {
                    GrdBusq.DataSource = DT;
                    GrdBusq.DataBind();
                }
                cnn.Close();
            }
        }

        protected void GrdBusq_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdBusq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Se debe leer la hoja de Excel cada vez y luego volver a enlazar la tabla de datos con GridView.
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string FileName = GrdBusq.Caption;
            string Extension = Path.GetExtension(FileName);
            string FilePath = Server.MapPath(FolderPath + FileName);
            Import_To_Grid(FilePath, Extension, "YES");

            GrdBusq.PageIndex = e.NewPageIndex;
            GrdBusq.DataBind();
        }

        // ********************************** Segunda opcion  *******************************************/

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {

                /* if (FileUpPP.HasFile)
                 {
                     Vbl4Ruta = FileUpPP.FileName;
                     Vbl6Ext = Path.GetExtension(Vbl4Ruta);
                     Vbl8Type = FileUpPP.PostedFile.ContentType;
                     imagen = new byte[FileUpPP.PostedFile.InputStream.Length];
                     FileUpPP.PostedFile.InputStream.Read(imagen, 0, imagen.Length);
                 }
                 else
                 {
                     ScriptManager.RegisterClientScriptBlock(this.UpPnlCampos, UpPnlCampos.GetType(), "IdntificadorBloqueScript", "alert('Debe seleccionar un archivo')", true);
                     return;
                 }*/


                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                //string FilePath = Server.MapPath(FolderPath + FileName);
                string FilePath = @"" + FolderPath + FileName;
                FileUpload1.SaveAs(FilePath);

                Import_To_Grid(FilePath, Extension, "YES"); // YES es si tiene encabezado o no 
            }

        }
        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;

                    break;

                case ".xlsx": //Excel 2016
                    conStr = ConfigurationManager.ConnectionStrings["Excel2016"].ConnectionString;
                    break;
            }
            //FilePath = @"C:\Users\Asus Pro\Downloads\Reportes.xlsx";
            conStr = string.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            DataTable dt = new DataTable();


            //Obtener el nombre de la primera hoja
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Leer datos de la primera hoja

            connExcel.Open();
            string sql = "SELECT * From [" + SheetName + "]";
            OleDbCommand cmdExcel = new OleDbCommand(sql, connExcel);
            OleDbDataAdapter oda = new OleDbDataAdapter(cmdExcel);
            // oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            //Vincular datos a GridView
            GrdBusq.Caption = Path.GetFileName(FilePath);
            GrdBusq.DataSource = dt;
            GrdBusq.DataBind();
        }

        // ********************************** tercera opcion  *******************************************/
        DataTable dt1 = new DataTable();
        protected void BtnV3_Click(object sender, EventArgs e)
        {

        }

        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                string StSql;
                int VbOpc=0;

                if (RdbAK.Checked == true) { VbOpc = 1; ViewState["NomBtnExp"] = "Vw_Aeronave"; }
                if (RdbRtes.Checked == true) { VbOpc = 5; ViewState["NomBtnExp"] = "Vw_ReporteMantenimiento"; }
                if (RdbPlantMstra.Checked == true) { VbOpc = 6; ViewState["NomBtnExp"] = "Vw_PlantillaMaestra"; }
                if (RdbInvHK.Checked == true) { VbOpc = 3; ViewState["NomBtnExp"] = "Vw_ElementosInstaladosAeronave"; }
                if (RdbHHK.Checked == true) { VbOpc = 2; ViewState["NomBtnExp"] = "Vw_HistoricoContadorAeroanve"; }                
                if (RdbHistSN.Checked == true) { VbOpc = 4; ViewState["NomBtnExp"] = "Vw_HistoricoContadorElemento"; }
                if (RdbSvcMnto.Checked == true) { VbOpc = 7; ViewState["NomBtnExp"] = "Vw_ServicioMantenimiento"; }
                if (RdbRcsoFscoSM.Checked == true) { VbOpc = 8; ViewState["NomBtnExp"] = "Vw_RecursoServicioMantenimiento"; }
                if (RdbLicncSM.Checked == true) { VbOpc = 9; ViewState["NomBtnExp"] = "Vw_LicenciaServicioManto"; }
                if (RdbOT.Checked == true) { VbOpc = 10; ViewState["NomBtnExp"] = "Vw_OrdenTrabajo_OT"; }
                if (RdbWS.Checked == true) { VbOpc = 11; ViewState["NomBtnExp"] = "Vw_WorkSheet_WS"; }
                if (RdbHisSvcCumpl.Checked == true) { VbOpc = 13; ViewState["NomBtnExp"] = "Vw_HistoricoServicioCumplidos"; }
                if (RdbInventr.Checked == true) { VbOpc = 12; ViewState["NomBtnExp"] = "Vw_Inventario"; }               
                if (RdbLV.Checked == true) { VbOpc = 14; ViewState["NomBtnExp"] = "Vw_LibroVuelo"; }
                if (RdbStatusRprt.Checked == true) { VbOpc = 15; ViewState["NomBtnExp"] = "Vw_StatusReport"; }               

                StSql = "EXEC ProyectoUsa @Op";              
                Cnx.SelecBD();
                using (SqlConnection con = new SqlConnection(Cnx.GetConex()))
                {
                    using (SqlCommand SC = new SqlCommand(StSql, con))
                    {
                        SC.CommandTimeout = 90000000;
                        SC.Parameters.AddWithValue("@Op", VbOpc);     
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SC.Connection = con;
                            sda.SelectCommand = SC;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                ds.Tables[0].TableName = ViewState["NomBtnExp"].ToString();
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        wb.Worksheets.Add(dt);
                                    }
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.ContentType = "application/ms-excel";
                                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xlsx", ViewState["NomBtnExp"].ToString()));
                                    Response.Charset = "";
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                string ble = Ex.ToString();
            }
        }

        protected void BtnExportar2_Click(object sender, EventArgs e)
        {
            CsTypExportarIdioma CursorIdioma = new CsTypExportarIdioma();
            CursorIdioma.Alimentar("CURRESERVA","5");

        }
    }
}