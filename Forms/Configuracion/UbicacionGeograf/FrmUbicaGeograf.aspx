<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmUbicaGeograf.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.UbicacionGeograf.FrmUbicaGeograf" %>

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

            $('#<%=DdlBusq.ClientID%>').chosen();
            $('#<%=DdlTipoUbc.ClientID%>').chosen();
            $('#<%=DdlUbicaSupr.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">    
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <br /><br />
            <div class="Scroll">
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="LblBusq" runat="server" CssClass="LblEtiquet" Text=" Consultar UG" />
                        <asp:DropDownList ID="DdlBusq" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Cod" />
                        <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="15" />
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="LblNombre" runat="server" CssClass="LblEtiquet" Text="Nom" />
                        <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control heightCampo" MaxLength="50" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-5">
                        <asp:Label ID="LblTipoUbc" runat="server" CssClass="LblEtiquet" Text="Tipo" />
                        <asp:DropDownList ID="DdlTipoUbc" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                    </div>
                </div>
                <div class="row">

                    <div class="col-sm-6">
                        <asp:Label ID="LblUbicaSupr" runat="server" CssClass="LblEtiquet" Text="Ubica" />
                        <asp:DropDownList ID="DdlUbicaSupr" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblVlrTasa" runat="server" CssClass="LblEtiquet" Text="Valor Tasas" />
                        <asp:TextBox ID="TxtVlrTasa" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control heightCampo" Enabled="false" />
                    </div>
                    <div class="col-sm-1">
                        <br />
                        <asp:CheckBox ID="CkbActivo" runat="server" CssClass="LblEtiquet" Text="Act" Enabled="false" />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:CheckBox ID="CkbRutaFrec" runat="server" CssClass="LblEtiquet" Text="Ruta Frec" Enabled="false" />
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
