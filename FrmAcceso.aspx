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
            clor: antiquewhite;
        }


        /*.ContenedorLogin {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);*/ /* centra sin márgenes negativos */
        /*width: 400px;
            height: 280px;
            text-align: center;
            border: 1px solid #808080;
            padding: 5px;
            box-sizing: border-box;
        }*/

        /* Solo la marca de agua: sin position, width ni height */
        /*.contenedor-imagen::before {
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
        }*/

        .ddl-fondo {
            background-color: black; /* Un color azul de ejemplo */
            color: black; /* Color del texto */
        }

        .placeholder-color::placeholder {
            color: black; /* O el color que prefieras */
            opacity: 1; /* Para asegurar que el color se vea completo */
        }

        .ContenedorLogin {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%); /* centra sin márgenes negativos */
            width: 400px;
            height: 580px; /**/
            text-align: center;
            border: 1px solid #808080;
            padding: 5px;
            box-sizing: border-box;
        }
        /* ===== Fondo gris de la página ===== */
        .login-page {
            min-height: 40vh; /**/
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #e6e8eb; /* fondo gris */
            font-family: "Segoe UI", Roboto, Arial, sans-serif;
        }
        /* ===== Tarjeta de login ===== */
        .login-card {
            width: 100%;
            max-width: 300px;
            padding: 32px 28px 24px;
            background: #ffffff;
            border-radius: 18px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
            box-sizing: border-box;
        }
        /* ===== Logo en la parte superior ===== */
        .login-logo {
            width: 100%;
            height: 100px;
            /*background-image: url("../images/Logo_Login_Acceso.jpg");*/
            background-repeat: no-repeat;
            background-position: center top;
            background-size: contain;
            margin-bottom: 16px;
        }
        /* ===== Texto de encabezado ===== */
        .login-title {
            margin: 0 0 6px;
            text-align: center;
            font-size: 24px;
            font-weight: 700;
            color: #0f2a43;
        }

        .login-subtitle {
            margin: 0 0 24px;
            text-align: center;
            font-size: 14px;
            line-height: 1.4;
            color: #4a5568;
        }

        /* ===== Campos del formulario ===== */
        .login-field {
            margin-bottom: 16px;
        }

            .login-field label {
                display: block;
                margin-bottom: 6px;
                font-size: 13px;
                font-weight: 600;
                color: #1a202c;
            }

            .login-field select,
            .login-field input {
                width: 100%;
                height: 44px;
                padding: 0 12px;
                font-size: 14px;
                color: #2d3748;
                background: #ffffff;
                border: 1px solid #d5dae1;
                border-radius: 8px;
                box-sizing: border-box;
                outline: none;
            }

                .login-field select:focus,
                .login-field input:focus {
                    border-color: #1b8bb0;
                    box-shadow: 0 0 0 3px rgba(27, 139, 176, 0.2);
                }


        /* ===== Botón Ingresar ===== */
        .login-button {
            width: 100%;
            height: 46px;
            margin-top: 8px;
            font-size: 16px;
            font-weight: 600;
            color: #ffffff;
            background: linear-gradient(90deg, #1b4f8a 0%, #1b9bb8 100%);
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: filter 0.2s ease;
        }

            .login-button:hover {
                filter: brightness(1.1);
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
    <%--<div class="text">
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
    </div>--%>
    <div class="login-page login-card login-logo ContenedorLogin">
        <%--ContenedorLogin contenedor-imagen--%>
        <div>
            <div class="btn-info">
            </div>
        </div>
        <div class="login-field">
            <%--zona-campos --%>
            <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-group">
                        <h2>
                            <asp:Label ID="LblInicio" runat="server" CssClass="CsTitulo login-title" Text="Login" /></h2>
                        <asp:DropDownList ID="DdlNit" runat="server" CssClass="form-control ddl-fondo" Height="30px" BackColor="Transparent" Font-Size="Smaller" OnTextChanged="DdlNit_TextChanged" Width="100%" />
                        <asp:TextBox ID="TxtPassEmsa" runat="server" TextMode="Password" CssClass="form-control ddl-fondo placeholder-color" BackColor="Transparent" placeholder="Company password" Height="30px" Width="100%" /><%--<br />--%>
                        <asp:DropDownList ID="DdlBD" runat="server" CssClass="form-control ddl-fondo" Height="30px" BackColor="Transparent" Font-Size="Smaller" Visible="false" Width="100%" />
                        <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control ddl-fondo placeholder-color" placeholder="Usuario" BackColor="Transparent" ForeColor="Black" Height="30px" Visible="false" Width="100%" />
                        <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control ddl-fondo placeholder-color" BackColor="Transparent" ForeColor="Black" placeholder="Password" Height="30px" Visible="false" Width="100%" />
                        <asp:Button ID="TbnIngresar" runat="server" Text="Confirm Company" CssClass="form-control btn btn-primary active login-button" OnClick="TbnIngresar_Click" Width="100%" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
