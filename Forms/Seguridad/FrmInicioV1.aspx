﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPpal.Master" AutoEventWireup="true" CodeBehind="FrmInicioV1.aspx.cs" Inherits="_77NeoWeb.Forms.FrmInicioV1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Inicio</title>

    <style type="text/css">
        .Posmenu {
            float: left;
        }

        .menuSalir {
            position: absolute;
            width: 36px;
            height: 35px;
            left: 95%;
            top: 1%;
        }

        .primaryStaticMenu {
            /* Formato del fondo principal*/
            background-color: transparent;
            float: right;
            position: relative;
            margin: 20px auto;
            background: #B0B9AF;
            border-radius: 5px;
            color: aliceblue;
        }

        .primaryStaticMenuItem {
            /* Formato del texto del menu principal*/
            width: 26em;
            /* background-color: #f7f2ea;
            border-width: 1px;
            border-color: #efefef #aaab9c #ccc #efefef;
            border-style: solid;
            color: #777777;
            padding: 0.5em 0 0.5em 1em;*/
            position: relative;
            margin: 10px auto; /*Altura*/
            background: #B0B9AF;
            border-radius: 5px;
        }

        .primaryStaticHover {
            /**/ color: #800000;
            background: #f0e7d7;
            /*formato al posicionarse sobre el texto del menu*/
        }

        .primaryDynamicMenu {
            /*formato del fondo del todo subMenu*/
            /*background-color: #f7f2ea;
            */
            position: relative;
            /*width: 40em;*/
            margin: 20px auto;
            background: #B0B9AF;
            border-radius: 5px;
            border-bottom: solid 1px #ccc;
        }

        .primaryDynamicMenuItem {
            /*Formato solo del submenu que no tiene mas niveles sub menu*/
            /* width: 10em;
            background-color: #f7f2ea;
            color: #777;
            padding: 0.5em 0 0.5em 1em;
            border-width: 1px;
            border-color: #f7f2ea #aaab9c #f7f2ea #efefef;
            border-style: solid;*/
            position: relative;
            width: 30em;
            margin: 3px auto;
            border-radius: 5px;
            background-color: #CFD7CE;
            border-style: solid;
            border-color: #f7f2ea #aaab9c #f7f2ea #efefef;
            height: 2em;
        }

        .primaryDynamicHover {
            color: #800000;
            background: #f0e7d7;
        }

        .Passw {
            text-align: right;
        }

        .Menu {
            text-align: left;
            left: 1%;
        }
    </style>
   <script type="text/javascript">  
       function changeColor(e) {
           e.style.changeColor = "red";
       }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <div class="TextoSuperior">
        <h1>XOM system</h1>
    </div>
    <asp:ImageButton ID="IbnSalir" runat="server" CssClass="menuSalir" ImageUrl="~/images/ExitV1.png" ToolTip="Salir" OnClick="IbnSalir_Click" OnClientClick="return confirm('¿Desea cerrar la sesión?');" />
    <div class="Passw">
       <%--<asp:LinkButton ID="LkbPrueba" runat="server" CssClass="Menu" CausesValidation="False" Text="prueba" OnClick="LkbPrueba_Click" />--%>
        <asp:LinkButton ID="LkbMenu" runat="server" CssClass="Menu" CausesValidation="False" Text="Menú" ForeColor ="#ccffff"  OnClick="LkbMenu_Click" />&nbsp&nbsp&nbsp&nbsp   
        <asp:LinkButton ID="LkbCambPass" runat="server" CssClass="Passw" CausesValidation="False" Text="Cambio contraseña..." ForeColor ="#ccffff" OnClick="LkbCambPass_Click" OnClientClick="return confirm('¿Desea ralizar el cambio de contraseña?');" />

    </div>
    <div class="Posmenu">
        <asp:Menu ID="MyMenu" runat="server" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#069" StaticSubMenuIndent="10px" Orientation="Vertical" BackColor="#F7F6F3"
            MaximumDynamicDisplayLevels="4" StaticEnableDefaultPopOutImage="false" StaticDisplayLevels="1">
            <StaticMenuStyle CssClass="primaryStaticMenu" />
            <StaticMenuItemStyle CssClass="primaryStaticMenuItem" />
            <StaticHoverStyle CssClass="primaryStaticHover" />
            <DynamicMenuStyle CssClass="primaryDynamicMenu" />
            <DynamicMenuItemStyle CssClass="primaryDynamicMenuItem" />
            <DynamicHoverStyle CssClass="primaryDynamicHover" />
        </asp:Menu>
    </div>
</asp:Content>
