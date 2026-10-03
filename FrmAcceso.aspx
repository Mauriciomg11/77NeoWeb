<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPpal.Master" AutoEventWireup="true" CodeBehind="FrmAcceso.aspx.cs" Inherits="_77NeoWeb.FrmAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Acceso</title>

    <style type="text/css">
        .form-control {
            justify-content: center;
            width: 100%; /* Asegura que los controles ocupen el 100% del ancho disponible */
            max-width: 400px; /* Limita el tamaño máximo de los controles */
            margin-bottom: 10px;
        }

        .centrarCuadroSCV {
            position: absolute;
            top: 46%;
            left: 5%;
            width: 70%;
            height: 600px;
            padding: 5px;
            text-align: left;
            color: antiquewhite;
        }

        /*.ContenedorLogin {
            position: absolute;
            text-align: center;
            top: 40%;
            left: 50%;
            width: 400px;
            margin-left: -200px;
            height: 325px;
            margin-top: -150px;
            border: 1px solid #808080;
            padding: 5px;
        }

        .contenedor-imagen {
            background-image: url("../images/Logo_Login_Acceso.jpg");
            background-size: cover;*/ /* Cubre todo el div sin distorsionar, cortando partes si es necesario */
        /*background-repeat: no-repeat;
            background-position: center;
            width: 400px;
            height: auto;
        }*/
        .ContenedorLogin {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%); /* centra sin márgenes negativos */
            width: 400px;
            height: 280px;
            text-align: center;
            border: 1px solid #808080;
            padding: 5px;
            box-sizing: border-box;
        }

        /* Solo la marca de agua: sin position, width ni height */
        .contenedor-imagen::before {
            content: "";
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-image: url("../images/Logo_Login_Acceso.jpg");
            background-size: cover;
            background-repeat: no-repeat;
            background-position: center;
            opacity: 0.08;
            z-index: 0;
        }

        .contenedor-imagen > * {
            position: relative;
            z-index: 1;
        }

        .ddl-fondo {
            background-color: black; /* Un color azul de ejemplo */
            color: black; /* Color del texto */
        }

        .placeholder-color::placeholder {
            color: black; /* O el color que prefieras */
            opacity: 1; /* Para asegurar que el color se vea completo */
        }

        .zona-campos {
            height: 100%; /* o una altura fija, ej. 250px, si hay más cosas en el contenedor */
            display: flex;
            align-items: center; /* centrado vertical */
            justify-content: center; /* centrado horizontal */
        }

            /* El div que genera el UpdatePanel */
            .zona-campos > div {
                width: 100%;
            }

            .zona-campos .form-group {
                margin: 0;
                display: flex;
                flex-direction: column;
                align-items: center;
                gap: 10px;
            }

                .zona-campos .form-group .form-control {
                    width: 80%;
                }
    </style>
    <script type="text/javascript">
        function myFuncionddlP() {
            $('#<%=DdlNit.ClientID%>').chosen();
            $('#<%=DdlBD.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="TextoSuperior">
        <h1>
            <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" Text="77NEO Access" /></h1>
    </div>
    <div class="text">
        <div class="centrarCuadroSCV">
            <div style="color: black">
                <h2>77NEO System</h2>
                <h5>
                    <asp:Label ID="LblText1" runat="server" Text="Aeronautical management system" /></h5>
                <h5>
                    <asp:Label ID="LblText2" runat="server" CssClass="CsTitulo" Text="We take care of your data" /></h5>
                <h5>
                    <asp:Label ID="LblText3" runat="server" CssClass="CsTitulo" Text="We fly with them..." /></h5>
            </div>
        </div>
    </div>
    <div class="ContenedorLogin contenedor-imagen">
        <div>
            <div class="btn-info">
                <h2>
                    <asp:Label ID="LblInicio" runat="server" CssClass="CsTitulo" Text="Login" /></h2>
            </div>
        </div>
        <div class="zona-campos">
            <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-group">
                        <asp:DropDownList ID="DdlNit" runat="server" CssClass="form-control ddl-fondo" Height="30px" BackColor="Transparent" Font-Size="Smaller" OnTextChanged="DdlNit_TextChanged" Width="100%" />
                        <asp:TextBox ID="TxtPassEmsa" runat="server" TextMode="Password" CssClass="form-control ddl-fondo placeholder-color" BackColor="Transparent" placeholder="Company password" Height="30px" Width="100%" /><%--<br />--%>
                        <asp:DropDownList ID="DdlBD" runat="server" CssClass="form-control ddl-fondo" Height="30px" BackColor="Transparent" Font-Size="Smaller" Visible="false" Width="100%" />
                        <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control ddl-fondo placeholder-color" placeholder="Usuario" BackColor="Transparent" ForeColor="Black" Height="30px" Visible="false" Width="100%" />
                        <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control ddl-fondo placeholder-color" BackColor="Transparent" ForeColor="Black" placeholder="Password" Height="30px" Visible="false" Width="100%" />
                         <asp:Button ID="TbnIngresar" runat="server" Text="Confirm Company" CssClass="form-control btn btn-primary active" OnClick="TbnIngresar_Click" Width="100%" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>      
    </div>
</asp:Content>
