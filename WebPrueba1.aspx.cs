using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExcelDataReader;

namespace _77NeoWeb
{
    public partial class WebPrueba1 : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Login77"] == null)
            {
                if (Cnx.GetProduccion().Trim().Equals("Y")) { Response.Redirect("~/FrmAcceso.aspx"); }
            }
            ViewState["PFileName"] = System.IO.Path.GetFileNameWithoutExtension(Request.PhysicalPath); // Nombre del archivo  
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";
                if (Cnx.GetProduccion().Trim().Equals("N"))
                {
                    Session["C77U"] = Cnx.GetUsr(); //00000082|00000133
                    Session["D[BX"] = Cnx.GetBD();//|DbNeoDempV2  |DbNeoAda | DbNeoHCT
                    Session["$VR"] = Cnx.GetSvr();
                    Session["V$U@"] = Cnx.GetUsSvr();
                    Session["P@$"] = Cnx.GetPas();
                    Session["N77U"] = Session["D[BX"];
                    Session["Nit77Cia"] = Cnx.GetNit(); // 811035879-1 TwoGoWo |800019344-4  DbNeoAda | 860064038-4 DbNeoHCT
                    Session["!dC!@"] = Cnx.GetIdCia();
                    Session["77IDM"] = Cnx.GetIdm();
                    Session["MonLcl"] = Cnx.GetMonedLcl();// Moneda Local
                }
            }
            if (!IsPostBack)
            {

            }
        }
        /*private void Import_To_Grid(string FilePath, string Extension, string isHDR)

        {
            string conStr = "";
            switch (Extension)
            {
                case "xls": //Excel 97-03

                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;

                case "xlsx": //Excel 07

                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);

            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable DT = new DataTable();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();
            connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;
            oda.Fill(DT);
            if (DT.Rows.Count > 0)
            {
                Grdprueba.DataSource = DT;
            }
            else { Grdprueba.DataSource = null; }

            Grdprueba.DataBind();
            connExcel.Close();
          
        }*/
        protected void Import(string FilePath, string Extension)
        {
            try
            {
                FileStream stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader ExcelReader;
                /*
                //1. Reading Excel file
                if (Path.GetExtension(FilePath).ToUpper() == ".XLS")
                { ExcelReader = ExcelReaderFactory.CreateBinaryReader(stream); }
                else
                { ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(stream); }// XLSX
*/
                //2. DataSet - The result of each spreadsheet will be created in the result.Tables
               // DataSet result = ExcelReader.AsDataSet();

                ExcelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                //// reader.IsFirstRowAsColumnNames
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    { UseHeaderRow = true }
                };
                //3. DataSet - Create column names from first row
     
                var dataSet = ExcelReader.AsDataSet(conf);
                DataTable DT = dataSet.Tables[0];

                if (DT.Rows.Count > 0)
                {
                    Grdprueba.DataSource = DT;
                }
                else { Grdprueba.DataSource = null; }

                Grdprueba.DataBind();
            }
            catch (Exception ex) { }
        }
        protected void BtnCargaMaxiva_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpCot.Visible == false)
                {
                    FileUpCot.Visible = true;
                    BtnCargaMaxiva.OnClientClick = string.Format("return confirm('debe cargar un archivo');");
                }
                else
                {
                    if (FileUpCot.HasFile == true)
                    {
                        string FolderPath;
                        string FileName = Path.GetFileName(FileUpCot.PostedFile.FileName);
                        string VblExt = Path.GetExtension(FileUpCot.PostedFile.FileName);

                        if (Cnx.GetProduccion().Trim().Equals("Y")) { FolderPath = ConfigurationManager.AppSettings["FolderPath"]; }//Azure
                        else { FolderPath = ConfigurationManager.AppSettings["FoldPathLcl"]; }



                        VblExt = VblExt.Substring(VblExt.LastIndexOf(".") + 1).ToLower();
                        string[] formatos = new string[] { "xls", "xlsx" };
                        if (Array.IndexOf(formatos, VblExt) < 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Archivo inválido');", true);
                            return;
                        }


                        string FilePath = FolderPath + FileName;// @"c:/Unload77NeoWeb/" + FileName; //Server.MapPath(FolderPath + FileName);

                        FileUpCot.SaveAs(FilePath);

                        //Import_To_Grid(FilePath, VblExt, "Yes");
                        Import(FilePath, VblExt);
                        FileUpCot.Visible = false;
                    }
                    //------------------------------------------------------                    
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Debe seleccionar un archivo.');", true);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + Ex.StackTrace.Substring(Ex.StackTrace.Length > 300 ? Ex.StackTrace.Length - 300 : 0, 300) + "');", true);
            }
        }
    }
}