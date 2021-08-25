<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmIngProxCumplimiento.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmIngProxCumplimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContenedor {
            /*vertical-align: top;*/
            /*background: #e0e0e0;*/
            /*margin: 0 0 1rem;*/
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 34%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -17%;
            /*determinamos una altura*/
            height: 50%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            /*border: 1px solid #808080;*/
            padding: 5px;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
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
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">
                <div class="row">
                    <div class="col-sm-5">
                        <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="fecha Inicial" />
                        <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                    </div>
                    <div class="col-sm-5">
                        <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="fecha Final" />
                        <asp:TextBox ID="TxtFechF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                    </div>
                    <div class="col-sm-2">
                        <asp:ImageButton ID="IbnExcel" runat="server" ToolTip="exportar consolidado" CssClass=" BtnExpExcel" Height="35px" Width="35px" ImageUrl="~/images/ExcelV1.png" OnClick="IbnExcel_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbnExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
