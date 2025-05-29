<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmServiciosSinCrear.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmServiciosSinCrear" %>

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

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
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
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br /><br />
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAbrirSrvcs" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnAbrirSrvcs_Click" OnClientClick="target ='_blank';" Text="abrir servicios" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAbrirSrvcsMyrs" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnAbrirSrvcsMyrs_Click" OnClientClick="target ='_blank';" Text="servicios mayores"  Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="ScrollDet">
                                    <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        DataKeyNames="CodTipoPropuesta,CodModeloPr,IdDetPropSrv,IdPropuesta"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" EmptyDataText="No existen registros ..!" OnRowCommand="GrdDet_RowCommand"
                                        OnRowDataBound="GrdDet_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Busq">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplBusq" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtBusq" Width="30px" Height="30px" ImageUrl="~/images/FindV3.png" runat="server" CommandName="Busq" ToolTip="Buscar" OnClientClick="target ='';" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtBusq" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="razón social" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRazonS" Text='<%# Eval("RazonSocial") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="propuesta" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPpt" Text='<%# Eval("CodigoPPT") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="contrato" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblContrt" Text='<%# Eval("NumContrato") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="modelo" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblMode" Text='<%# Eval("DescripcionModelo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="matricula" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblHK" Text='<%# Eval("MatricuaPr") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSN" Text='<%# Eval("SnElemento") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="servicio">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtNomSvc" Text='<%# Eval("DescricionServicio") %>' runat="server" Width="100%" Enabled="false" TextMode="MultiLine" />
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
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsiganar" runat="server" Text="servicios para asignar" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <table class="TabOpcBusq">
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                <td>
                                    <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                            </tr>
                        </table>
                    <div class="CentrarBusq DivMarco">                        
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="IdSrvManto, CodContador, CodModeloSM"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdBusq_RowCommand" OnRowDataBound="GrdBusq_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="IbtAsignar" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Asignar" ToolTip="asignar" OnClientClick="target ='';" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="IbtAsignar" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="servicio" HeaderStyle-Width="80%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtNomSvc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" TextMode="MultiLine" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodModelo">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodMod" Text='<%# Eval("NomModelo") %>' runat="server" Width="100%" />
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
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
