<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaSolicitudNuevaRepa.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmAlertaSolicitudNuevaRepa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
      
        .GridDivScroll {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 95%;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .TamanAlert {
            height: 400px;
            width: 95%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
     <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
     <div id="ModalAlerta" class="modal fade " tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitAlrt" runat="server" Text="Alertas" /></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpPlAlert" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12 DivMarco">
                                    <div class="CentrarGrid pre-scrollable">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitAlertaSPRp" runat="server" Text="reservas nuevas" /></h6>
                                        <asp:GridView ID="GrdAlrta" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="pedido">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("CodPedido") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="referencia">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P/N">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Pn") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="S/N">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Sn") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="descripcion">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="cant">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Cant") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="fecha">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("FechaCrea") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="fecha aprobac">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("FechaAprob") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="usuario">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Usu") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="propuesta">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Propuesta") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnExportarModl" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnExportarModl" runat="server" class="btn btn-default" Text="exportar" OnClick="BtnExportarModl_Click" />
                    <asp:Button ID="BtnCerrarAlerta" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lblAlert" runat="server" Text="Estado: Normal" Style="font-weight:bold;"></asp:Label>
    <script type="text/javascript">
    var originalTitle;
    //var notificationTitle = "¡ALERTA! Nuevo Mensaje";
    var intervalId;

    // Iniciar el parpadeo
        function startFlash(TxtMensRecibo) {
        if (!intervalId) {
            originalTitle = document.title;
            // Alternar título cada 1 segundo
            intervalId = setInterval(function () {
                document.title = (document.title === originalTitle) ? TxtMensRecibo : originalTitle;
                
                // Cambiar color de la pestaña es difícil, 
                // pero cambiar el color de un elemento dentro de la página es fácil
                var alertElement = document.getElementById('<%= lblAlert.ClientID %>');
                if (alertElement) {
                    alertElement.style.color = (alertElement.style.color === 'red') ? 'blue' : 'red';
                }
            }, 1000);
        }
    }

    // Detener el parpadeo
    function stopBlinking() {
        if (intervalId) {
            clearInterval(intervalId);
            document.title = originalTitle;
            intervalId = null;
            
            var alertElement = document.getElementById('<%= lblAlert.ClientID %>');
            if (alertElement) {
                alertElement.style.color = 'black'; // Color original
            }
        }
    }

    // Detectar si el usuario volvió a la pestaña para detener la alerta
    window.onfocus = function () {
        stopBlinking();
    };
    </script>
</asp:Content>
