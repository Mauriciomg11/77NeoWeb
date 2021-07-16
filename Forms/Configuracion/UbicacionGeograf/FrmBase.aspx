<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmBase.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.UbicacionGeograf.FrmBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .Scroll {
        vertical-align: top;
        overflow: auto;
        width: 90%;
        height: 570px;
        margin-left: auto;
        margin-right: auto;
    }

    .CentarGrid {
        width: 60%;
        margin-left: auto;
        margin-right: auto;
        width: 50%;
    }

    .heightCampo {
        height: 25px;
        width: 95%;
        font-size: 12px;
    }

  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">   
        function solonumeros(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }
            if (key < 48 || key > 57) {
                return false;
            }

            return true;
        }
        function myFuncionddl() {
            $('#<%=DdlUbica.ClientID%>').chosen();          
            $('#<%=DdlBusq.ClientID%>').chosen();          
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Scroll">
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="LblBusq" runat="server" CssClass="LblEtiquet" Text=" Consultar Base" />
                        <asp:DropDownList ID="DdlBusq" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Cod" />
                        <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="LblNombre" runat="server" CssClass="LblEtiquet" Text="Nom" />
                        <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control heightCampo" MaxLength="40" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblFrecR" runat="server" CssClass="LblEtiquet" Text="Frec R" />
                        <asp:TextBox ID="TxtFrecR" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control heightCampo" Enabled="false" MaxLength="20" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblTelef" runat="server" CssClass="LblEtiquet" Text="Tele" />
                        <asp:TextBox ID="TxtTelef" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control heightCampo" Enabled="false" MaxLength="40" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblFax" runat="server" CssClass="LblEtiquet" Text="Fax" />
                        <asp:TextBox ID="TxtFax" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control heightCampo" Enabled="false" MaxLength="40" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-5">
                        <asp:Label ID="LblDir" runat="server" CssClass="LblEtiquet" Text="Direc" />
                        <asp:TextBox ID="TxtDir" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" TextMode="MultiLine" />
                    </div>
                    <div class="col-sm-5">
                        <asp:Label ID="LblUbica" runat="server" CssClass="LblEtiquet" Text="Ubica" />
                        <asp:DropDownList ID="DdlUbica" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:CheckBox ID="CkbActivo" runat="server" CssClass="LblEtiquet" Text="Act" Enabled="false" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="LblDescrip" runat="server" CssClass="LblEtiquet" Text="Desc" />
                        <asp:TextBox ID="TxtDescrip" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" TextMode="MultiLine" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnIngresar_Click" Text="nuevo" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnModificar_Click" Text="modificar" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnEliminar_Click" Text="Elimina" />
                    </div>
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
