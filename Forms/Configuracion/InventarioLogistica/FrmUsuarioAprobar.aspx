<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmUsuarioAprobar.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.InventarioLogistica.FrmUsuarioAprobar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>TA</title>
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Scroll {
            vertical-align: top;
            overflow: auto;
            width: 20%;
            height: 570px;
            margin-left: auto;
            margin-right: auto;
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
            $('#<%=DdllUsuPpl.ClientID%>').chosen();
            $('#<%=DdlUsuMyrAlt1.ClientID%>').chosen();
            $('#<%=DdlUsuMyrAlt2.ClientID%>').chosen();
            $('#<%=DdlUsuMnrPpl.ClientID%>').chosen();
            $('#<%=DdlUsuMnrAlt1.ClientID%>').chosen();
            $('#<%=DdlUsuTrmPpl.ClientID%>').chosen();
            $('#<%=DdlUsuTrmAlt1.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-8">
                    <div class="col-sm-9">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="lblTitUsuAprMyr" runat="server" Text="Usuarios aprobaciones mayores" /></h6>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuPpl" runat="server" CssClass="LblEtiquet" Text=" Usuario Principal" />
                            <asp:DropDownList ID="DdllUsuPpl" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuMyrAlt1" runat="server" CssClass="LblEtiquet" Text=" Usuario Alterno 1" />
                            <asp:DropDownList ID="DdlUsuMyrAlt1" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuMyrAlt2" runat="server" CssClass="LblEtiquet" Text=" Usuario Alterno 2" />
                            <asp:DropDownList ID="DdlUsuMyrAlt2" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <br />
                    <div class="col-sm-9">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="lblTitUsuAprMnr" runat="server" Text="Usuarios aprobaciones menor valor" /></h6>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuMnrPpl" runat="server" CssClass="LblEtiquet" Text=" Usuario Principal" />
                            <asp:DropDownList ID="DdlUsuMnrPpl" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuMnrAlt1" runat="server" CssClass="LblEtiquet" Text=" Usuario Alterno 1" />
                            <asp:DropDownList ID="DdlUsuMnrAlt1" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <br />
                    <div class="col-sm-9">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="lblTitUsuTRM" runat="server" Text="Usuarios ingreso TRM" /></h6>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuTrmPpl" runat="server" CssClass="LblEtiquet" Text=" Usuario Principal" />
                            <asp:DropDownList ID="DdlUsuTrmPpl" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-9">
                            <asp:Label ID="LblUsuTrmAlt1" runat="server" CssClass="LblEtiquet" Text=" Usuario Alterno 1" />
                            <asp:DropDownList ID="DdlUsuTrmAlt1" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitValores" runat="server" Text="Valores" /></h6>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Label ID="LblMonedaLocal" runat="server" CssClass="LblEtiquet" Text="Moneda Loc" />                           
                            <asp:TextBox ID="TxtMonedaLocal" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0"  Visible="false" />
                            <asp:TextBox ID="MonLocal" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" Text="0" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Label ID="LblDolar" runat="server" CssClass="LblEtiquet" Text="Dolar" />                           
                            <asp:TextBox ID="TxtDolar" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0"  Visible="false"/>
                             <asp:TextBox ID="MonUSD" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" Text="0" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:Label ID="LblEuro" runat="server" CssClass="LblEtiquet" Text=" Euro" />                           
                            <asp:TextBox ID="TxtEuro" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0"  Visible="false"/>
                             <asp:TextBox ID="MonEUR" runat="server" Width="100%" CssClass="form-control heightCampo" Enabled="false" Text="0" />
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="Scroll">
                <div class="col-sm-8">
                    <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnModificar_Click" Text="modificar" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
