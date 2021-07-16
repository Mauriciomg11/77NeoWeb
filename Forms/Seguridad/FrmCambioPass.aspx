<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCambioPass.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmCambioPass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ContenedorLogin {
            position: absolute;
            text-align: center;
            top: 60%;
            left: 50%;
            width: 400px;
            margin-left: -200px;
            height: 200px;
            margin-top: -150px;
            border: 1px solid #808080;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="ContenedorLogin">
                <div>
                    <div class="btn-info">
                        <h2>
                            <asp:Label ID="TitConfirmarC" runat="server" Text="Confirmar contraseña" /></h2>
                    </div>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control" placeholder="Usuario" Height="30px" Enabled="false" />
                    <asp:TextBox ID="TxtClave" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" Height="30px" />
                </div>
                <div class="form-group">
                    <asp:Button ID="TbnIngresar" runat="server" Text="Acceder" CssClass="form-control btn btn-primary active" OnClick="TbnIngresar_Click" />
                    <asp:TextBox ID="TxtNuevoPass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Nueva contraseña" Height="30px" Visible="false" />
                    <asp:TextBox ID="TxtConfirmarPass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirmar contraseña" Height="30px" Visible="false" />
                    <asp:Button ID="BtnCambioPass" runat="server" Text="Registrar cambio" CssClass="form-control btn btn-primary active" OnClick="BtnCambioPass_Click" Visible="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
