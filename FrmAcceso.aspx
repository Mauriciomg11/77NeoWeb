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
            height: 200px;
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
        <h1>Acceso 77NeoWeb</h1>
    </div>
    <div class="centrarCuadroSCV">
        <div class="text">
            <h2>77NeoWeb system</h2>
            <h3>Sistema de gestión aeronautico</h3>
            <h3>Cuidamos sus datos</h3>
            <h3>Volamos con ellos...</h3>
        </div>
    </div>
    <div class="ContenedorLogin">
        <div>
            <div class="btn-info">
                <h2>Inicio</h2>
            </div>
        </div>
        <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <asp:DropDownList ID="DdlNit" runat="server" CssClass="form-control" Height="30px" Font-Size="Smaller" OnTextChanged="DdlNit_TextChanged"></asp:DropDownList>
                    <asp:TextBox ID="TxtPassEmsa" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password compañía" Height="30px"></asp:TextBox><br />
                    <asp:DropDownList ID="DdlBD" runat="server" CssClass="form-control" Height="30px" Font-Size="Smaller" Visible="false"></asp:DropDownList>
                    <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" placeholder="Usuario" Height="30px" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" Height="30px" Visible="false"></asp:TextBox>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group">
            <asp:Button ID="TbnIngresar" runat="server" Text="Validar compañía" CssClass="form-control btn btn-primary active" OnClick="TbnIngresar_Click" />
        </div>

    </div>
</asp:Content>
