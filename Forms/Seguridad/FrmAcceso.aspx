<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPpal.Master" AutoEventWireup="true" CodeBehind="FrmAcceso.aspx.cs" Inherits="_77NeoWeb.Forms.FrmAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Acceso</title>
    <h1>Acceso 77NeoWeb</h1>
    <style type="text/css">
        .ContenedorLogin {
            position: absolute;
            text-align: center;
            top: 40%;
            left: 50%;
            width: 400px;
            margin-left: -200px;
            height: 200px;
            margin-top: -150px;
            border: 1px solid #808080;
            padding: 5px;
        }

        .centrarCuadroSCV {
            position: absolute;
            top: 45%;
            left: 5%;
            width: 70%;
            height: 600px;
            padding: 5px;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="centrarCuadroSCV">
        <div class="text">
            <h2>77NeoWeb system</h2>
            <h3>Sistema aeronautico de gestión </h3>
            <h3>Cuidamos su información</h3>
            <h3>Volamos con ellos...</h3>
        </div>
    </div>
    <div class="ContenedorLogin">
        <div>
            <div class="btn-info">
                <h2>Inicio</h2>
            </div>
        </div>
        <div class="form-group">
            <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
            <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="TbnIngresar" runat="server" Text="Iniciar sesión" CssClass="form-control btn btn-primary active" OnClick="TbnIngresar_Click" />
        </div>

    </div>
</asp:Content>
