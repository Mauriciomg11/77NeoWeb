<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Frm_InfIngenieria.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.Frm_InfIngenieria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>OT</title>
    <style type="text/css">
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">       
        function myFuncionddl() {
            <%--$('#<%=DdlMroTaller.ClientID%>').chosen();--%>          
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div class="col">
        <div class="col-sm-3">
             <asp:Button ID="BtnAdvice" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnAdvice_Click" Text="Advice" ToolTip="Imprimir valores actuales de los contadores de un elemento." Height="23px" />
        </div>
        <div class="col-sm-3">
             <asp:Button ID="BtnInsRemElem" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnInsRemElem_Click" Text="Instalación/Remoción" ToolTip="Histórico de Istalaciones y remociones / Eliminación de histórico." Height="23px" />
        </div>
         <div class="col-sm-3">
             <asp:Button ID="BtnInsRemSubC" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnInsRemSubC_Click" Text="Histórico Subcomponente" ToolTip="Historico de instalaciones y remociones de Subcomponentes." Height="23px" />
        </div>
    </div>
</asp:Content>
