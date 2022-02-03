using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using _77NeoWeb.prg;
using _77NeoWeb.Prg;
using System.IO;
using System.Collections;
using System.Data.OleDb;


namespace _77NeoWeb
{
    public partial class WebForm_IngresoMaestro_Detalle : System.Web.UI.Page
    {
        ClsConexion Cnx = new ClsConexion();
        DataTable DTHj = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["C77U"] == null)
            {
                Session["C77U"] = "";/* */
                Session["C77U"] = "00000082";
                Session["D[BX"] = "DbConfigWeb";
                Session["$VR"] = "77NEO01";
                Session["V$U@"] = "sa";
                Session["P@$"] = "admindemp";
                Session["N77U"] = "UsuPrueba";
                Session["Nit77Cia"] = "811035879-1";/*   */
            }
            // Create connection string variable. Modify the "Data Source"
            // parameter as appropriate for your environment.
            String sConnectionString = Server.MapPath("../ExcelData.xls");

            // Create connection object by using the preceding connection string.
            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            // Open connection with the database.
            objConn.Open();

            // The code to follow uses a SQL SELECT command to display the data from the worksheet.
            // Create new OleDbCommand to return data from worksheet.
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM myRange1", objConn);

            // Create new OleDbDataAdapter that is used to build a DataSet
            // based on the preceding SQL SELECT statement.
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

            // Pass the Select command to the adapter.
            objAdapter1.SelectCommand = objCmdSelect;

            // Create new DataSet to hold information from the worksheet.
            DataSet objDataset1 = new DataSet();

            // Fill the DataSet with the information from the worksheet.
            objAdapter1.Fill(objDataset1, "XLData");

            // Bind data to DataGrid control.
           

            // Clean up objects.
            objConn.Close();
        }

        protected void BindDHijo()
        {

            
        }
        protected void CrearStructuraTabla(string Tipo)
        {
           
        }
        protected void RefrescarTabla()
        {
           

        }
        protected void BtnConsult_Click(object sender, EventArgs e)
        {
            string strFileName;
            string strFilePath;
            string strFolder;
            strFolder = Server.MapPath("./");
            // Retrieve the name of the file that is posted.
            strFileName = oFile.PostedFile.FileName;
            strFileName = Path.GetFileName(strFileName);
            if (oFile.Value != "")
            {
                // Create the folder if it does not exist.
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                // Save the uploaded file to the server.
                strFilePath = strFolder + strFileName;
                if (File.Exists(strFilePath))
                {
                    TxtMadre.Text = strFileName + " already exists on the server!";
                }
                else
                {
                    oFile.PostedFile.SaveAs(strFilePath);
                    TxtMadre.Text = strFileName + " has been successfully uploaded.";
                }
            }
            else
            {
                TxtMadre.Text = "Click 'Browse' to select the file to upload.";
            }
            // Display the result of the upload.
            TxtMadre.Visible = true;
        }
        protected void BtnHabilitar_Click(object sender, EventArgs e)
        {
            

        }
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
          
        }
        protected void BtnEdit_Click(object sender, EventArgs e)
        {
        }
        protected void GrdHijo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }
        protected void GrdHijo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }
        protected void GrdHijo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}