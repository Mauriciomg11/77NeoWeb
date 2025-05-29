<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmIngProxCumplimiento.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmIngProxCumplimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
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
