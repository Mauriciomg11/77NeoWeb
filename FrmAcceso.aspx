<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPpal.Master" AutoEventWireup="true" CodeBehind="FrmAcceso.aspx.cs" Inherits="_77NeoWeb.FrmAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Acceso</title>

    <style type="text/css">
        .ContenedorLogin {
            position: absolute;
            text-align: center;
            top: 40%;
            left: 50%;
            width: 400px;
            margin-left: -200px;
            height: 300px;
            margin-top: -150px;
            border: 1px solid #808080;
            padding: 5px;
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
            <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" Text="77NeoWeb Access" /></h1>
    </div>
    <div class="centrarCuadroSCV">
        <div class="text">
            <h2>77NeoWeb system</h2>
            <h5>
                <asp:Label ID="LblText1" runat="server" Text="Aeronautical management system" /></h5>
            <h5>
                <asp:Label ID="LblText2" runat="server" CssClass="CsTitulo" Text="We take care of your data" /></h5>
            <h5>
                <asp:Label ID="LblText3" runat="server" CssClass="CsTitulo" Text="We fly with them..." /></h5>
        </div>
    </div>
    <div class="ContenedorLogin">
        <div>
            <div class="btn-info">
                <h2>
                    <asp:Label ID="LblInicio" runat="server" CssClass="CsTitulo" Text="Login" /></h2>
            </div>
        </div>
        <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <asp:DropDownList ID="DdlNit" runat="server" CssClass="form-control" Height="30px" Font-Size="Smaller" OnTextChanged="DdlNit_TextChanged" />
                    <asp:TextBox ID="TxtPassEmsa" runat="server" TextMode="Password" CssClass="form-control" placeholder="Company password" Height="30px" /><br />
                    <asp:DropDownList ID="DdlBD" runat="server" CssClass="form-control" Height="30px" Font-Size="Smaller" Visible="false" />
                    <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" placeholder="Usuario" Height="30px" Visible="false" />
                    <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" Height="30px" Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group">
            <asp:Button ID="TbnIngresar" runat="server" Text="Confirm Company" CssClass="form-control btn btn-primary active" OnClick="TbnIngresar_Click" />
        </div>

    </div>
</asp:Content>
