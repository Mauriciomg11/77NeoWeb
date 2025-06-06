﻿using System;
using System.Web.Security;
using System.Web.UI;

namespace _77NeoWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            string JQueryVer = "1.11.3";

           // BundleConfig.RegisterBundles(BundleTable.Bundles);
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/js/jquery-" + JQueryVer + ".min.js",
                DebugPath = "~/js/jquery-" + JQueryVer + ".js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            /*
            10.2.05.21-4| 28/01/2025| implementacion Menu
            10.2.05.21-5| 04/02/2025| corrección formularios Incoming y Movimientos Almacen, no se visualizaba el detalle
            */
            Session["77Version"] = "10.2.05.21";
            Session["77Act"] = "5";
            Session["$VR"] = "";
            Session["Nit77Cia"] = ""; // Nita cia 811035879-1
            Session["VblCE1MS"] = 1;
            Session["VblCE2MS"] = 1;
            Session["VblCE3MS"] = 1;
            Session["VblCE4MS"] = 1;
            Session["VblCE5MS"] = 1;
            Session["VblCE6MS"] = 1;
            Session["V$U@"] = ""; // V$V@R¡0
            Session["VbNombFrmUsuario"] = "";
            Session["VbApellFrmUsuario"] = "";
            Session["P@$"] = ""; // login
            Session["IdForm"] = 0; //FrmConfigPantalla
            Session["IdGrupoRP"] = 0; // FrmPefil
            Session["IdUsuRP"] = ""; // FrmPefil
            Session["CodidUsrPerfil"] = 0; // FrmPefil
            Session["IdFormRP"] = 0; // FrmPefil
            Session["D[BX"] = ""; //NomBD DbNeoSinDatos
            Session["NomCiaPpal"] = "TwoGoWorking S.A.S."; // FrmPefil
            Session["SigCiaPpal"] = "";
            Session["SigCia"] = "";
            Session["CodTipoCodigoInicial"] = "01";
            Session["PllaSrvManto"] = "SERVICIO"; // "SERVICIO"  "REPARACION"
            Session["LogoPpal"] = "Logo77Neo.png";
            Session["77IDM"] = "ES";
            Session["!dC!@"] = 1; //id Cia
            Session["MonLcl"] = "COP"; // Moneda
            Session["PCodOT"] = "0"; // Para llamar el numero desde la reparación
            Session["FormatFecha"] = "103"; // 103 formato europeo dd/MM/yyyy | 101 formato EEUU M/dd/yyyyy

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
        }
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}