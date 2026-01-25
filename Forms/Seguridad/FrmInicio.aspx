<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmInicio.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Inicio</title>
    <style type="text/css">
        .posicionVersion {
            position:absolute;
           top: 144px;
             left: 86%;
            width: 255px;/**/
            /*height: 600px;*/
            padding: 5px;/**/
            text-align: left;/**/
            color: antiquewhite;
        }
    </style>
    <script type="text/jscript">
        function myFuncionddl() { }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">

    <div class="ImagenFondoLoging">
        <br />
        <br />
        <h5>
            <asp:Label ID="LblVersion" runat="server" CssClass="posicionVersion" Text="Version 00.00.00.00" /></h5>
    </div>

</asp:Content>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Inicio</title>

  <script type="text/jscript">
      function myFuncionddl() {  }
  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server" class="ImagenFondoLoging">
   
</asp:Content>--%>