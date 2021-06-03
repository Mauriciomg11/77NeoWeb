<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmInformePropuesta.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmInformePropuesta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
        }
        .width{
            width:33%;
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Botones" runat="server">
                    <div class="CentrarBusq  DivMarco">
                        <div class="row">
                            <div class="col-sm-4">
                                <br />
                                <asp:Button ID="BtnSegumiento" runat="server" CssClass="btn btn-primary" OnClick="BtnSegumiento_Click" Text="seguimiento" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <br />
                                <asp:Button ID="BtnExportarVentas" runat="server" CssClass="btn btn-primary" OnClick="BtnExportarVentas_Click" Text="seguimiento" Width="100%"/>
                            </div>
                            <div class="col-sm-4">
                                <br />
                                <asp:Button ID="BtnExportarRepa" runat="server" CssClass="btn btn-primary" OnClick="BtnExportarRepa_Click" Text="seguimiento" Width="100%"/>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitSegumient" runat="server" Text="seguimiento" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <div class="CentrarBusq DivMarco">
                        <table class="TablaBusqueda width ">                            
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                <td >
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="100%" Height="28px" CssClass="form-control" Font-Size="11px" TextMode="Number" step="1" onkeypress="return solonumeros(event);" placeholder="Ingrese el número" /></td>
                                <td>
                                    <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                            </tr>
                        </table>
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" 
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdBusq_RowDataBound">
                                <Columns>                                   
                                    <asp:TemplateField HeaderText="Ppt">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPpt" Text='<%# Eval("Propuesta") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Estado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="usu">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Usuario") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaCrea") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
         <Triggers>
                    <asp:PostBackTrigger ControlID="BtnExportarVentas" />
                    <asp:PostBackTrigger ControlID="BtnExportarRepa" />
             </Triggers>
    </asp:UpdatePanel>
</asp:Content>
