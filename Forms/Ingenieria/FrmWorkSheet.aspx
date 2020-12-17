<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmWorkSheet.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmWorkSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Work Sheet</title>
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=DdlWSHK.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVw" runat="server">
        <asp:View ID="Vw0WorkSheet" runat="server">
            <asp:UpdatePanel ID="UplWS" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                            <asp:DropDownList ID="DdlWSHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlWSHK_TextChanged" AutoPostBack="true" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnWSNew" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnWSNew_Click" Text="Nuevo" ToolTip="Generar una nueva Work Sheet." Height="23px" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnImpWS" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnImpWS_Click" Text="Work Sheet" ToolTip="Imprimir work sheet." Height="23px" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnImpRecurs" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnImpRecurs_Click" Text="Recurso" ToolTip="Imprimir recurso." Height="23px" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnWSProces" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnWSProces_Click" Text="Procesar" ToolTip="Procesar el estatus nuevamente." Height="23px" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-4 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitWSOpen" runat="server" Text="Work Sheet abiertas" /></h6>
                            <asp:GridView ID="GrdWSAbiertas" runat="server" EmptyDataText="No existen registros ..!"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="15"
                                OnSelectedIndexChanged="GrdWSAbiertas_SelectedIndexChanged" OnPageIndexChanging="GrdWSAbiertas_PageIndexChanging" OnRowDataBound="GrdWSAbiertas_RowDataBound">
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <Columns>
                                    <asp:CommandField HeaderText="Selección" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                            </asp:GridView>
                        </div>
                        <div class="col-sm-6 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitWsBusq" runat="server" Text="Buscar Work Sheet" /></h6>
                            <div class="row">
                                <div class="col-sm-5">
                                    <asp:TextBox ID="TxtWSBusq" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese la Work Sheet a consultar" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="IbtSWConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtSWConsultar_Click" />
                                </div>
                            </div>
                            <asp:GridView ID="GrdWSBusq" runat="server" EmptyDataText="No existen registros ..!" DataKeyNames="Estado,CodHKWS"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="14"
                                OnSelectedIndexChanged="GrdWSBusq_SelectedIndexChanged" OnPageIndexChanging="GrdWSBusq_PageIndexChanging" OnRowDataBound="GrdWSBusq_RowDataBound">
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <Columns>
                                    <asp:CommandField HeaderText="Selección" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnWSNew" />
                    <asp:PostBackTrigger ControlID="BtnImpWS" />
                    <asp:PostBackTrigger ControlID="BtnImpRecurs" />
                    <asp:PostBackTrigger ControlID="BtnWSProces" />
                    <%--  <asp:AsyncPostBackTrigger ControlID="DdlWSHK" EventName="TextChanged" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1AsigOTaWS" runat="server">
            <asp:UpdatePanel ID="UplAsigOT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigOTaWS" runat="server" Text="Asignar orden de trabajo / Reporte a la Work Sheet" /></h6>
                    <asp:ImageButton ID="IbtCerrarAsigOT" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsigOT_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblAsigOTHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                            <asp:TextBox ID="TxtAsigOTHK" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblAsingOTWS" runat="server" CssClass="LblEtiquet" Text="Work Sheet" />
                            <asp:TextBox ID="TxtAsingOTWS" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-5 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitServicios" runat="server" Text="Servicios / Reportes" /></h6>
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:RadioButton ID="RdbAsigOT" runat="server" CssClass="LblEtiquet" Text="&nbsp O.T." GroupName="AsigOT" />
                                    <asp:RadioButton ID="RdbAsigRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte" GroupName="AsigOT" />
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="TxtAsigOT_RTE" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="IbtAsigOTBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtAsigOTBusq_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarAsigOT" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
