<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaPNNuevos.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmAlertaPNNuevos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ScrollDet {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 520px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
            font-weight: bold;
            width: 8%;
            height: 27px;
        }

        .Font_btnSelect {
            font-size: 12px;
            font-stretch: condensed;
            width: 14%;
            height: 27px;
        }

        .CentrarContenedor {
            position: absolute;           
            left: 50%;           
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
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
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Button ID="BtnEditar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnEditar_Click" OnClientClick="target ='';" Text="editar" />
                        <asp:Button ID="BtnReferencia" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnReferencia_Click" OnClientClick="target ='_blank';" Text="abrir referencia" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="ScrollDet">
                            <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" 
                                DataKeyNames="IdPnNoExistente, CodIdDetalleRes,IdDetPedido,IdDetPropuesta,IdDetPropHk,CodIdDetElemPlanInstrumento,IdSrvManto,CodPedido"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" EmptyDataText="No existen registros ..!"
                                OnRowDataBound="GrdDet_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnSol" Text='<%# Eval("PnNoExistente") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nuevo parte" HeaderStyle-Width="10%"><%----%>
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtPnNew" Text='<%# Eval("PnNuevo") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descrip" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="referencia" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und med">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMed") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IPC">
                                        <ItemTemplate>
                                            <asp:Label ID="LblIPC" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant solicit" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantS" Text='<%# Eval("CantSolicitada") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ot" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblOt" Text='<%# Eval("CodOrdenTrabajo") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="reprte" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRte" Text='<%# Eval("Reporte") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="propuesta" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPpt" Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="matricula">
                                        <ItemTemplate>
                                            <asp:Label ID="LblHk" Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="solicitud pedido">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSolPed" Text='<%# Eval("CodPedido") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Servicio">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSvc" Text='<%# Eval("Servicio") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Solicitado por">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSolicPor" Text='<%# Eval("Usuario") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFecha" Text='<%# Eval("FechaCrea") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnReferencia" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
