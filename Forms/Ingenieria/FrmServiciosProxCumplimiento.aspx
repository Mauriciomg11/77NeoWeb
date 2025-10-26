<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmServiciosProxCumplimiento.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmServiciosProxCumplimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .GridDis {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 73%;
        }
         .GridDivUbicTec {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 95%;
        }
        .CentrarSvcReset {
            position: absolute;
            left: 50%;
            width: 80%;
            margin-left: -40%;
            height: 85%;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">       
        function myFuncionddl() {
            $('#<%=DdlAeronave.ClientID%>').chosen();
        }
    </script>
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
                                <asp:Label ID="LblAeronave" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                                <asp:DropDownList ID="DdlAeronave" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblDiaVisual" runat="server" CssClass="LblEtiquet" Text="visualizar próximo" Width="100%" />
                                <asp:TextBox ID="TxtDiaVisual" runat="server" CssClass="form-control-sm heightCampo" Width="50%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" />
                                <asp:Label ID="LblEtiqDia" runat="server" CssClass="LblEtiquet" Text="dia(s)" Width="30%" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:CheckBox ID="CkbVisualTodo" runat="server" CssClass="LblEtiquet" Text="visualizar todo" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-success" Width="100%" OnClick="BtnConsultar_Click" Text="consultar" />
                            </div>
                            <div class="col-sm-0">
                                <asp:ImageButton ID="IbnExcel" runat="server" ToolTip="exportar consulta" CssClass=" BtnExpExcel" Height="38px" Width="38px" ImageUrl="~/images/ExcelV1.png" OnClick="IbnExcel_Click" />
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-4">
                                <asp:Button ID="BtnSvcRestCero" runat="server" CssClass="btn  btn-primary" Width="100%" OnClick="BtnSvcRestCero_Click" Text="Servicios reseteable" />
                            </div>

                            <div class="col-sm-4">
                                <asp:Button ID="BtnUbicaTec" runat="server" CssClass="btn  btn-primary" Width="100%" OnClick="BtnUbicaTec_Click" Text="ubicaciones tecnicas" />
                            </div>
                        </div>
                        <br />
                        <div class="row ">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitServicios" runat="server" Text="servicios próximos a vencerse" /></h6>
                            </div>
                        </div>
                        <div class="GridDis">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both"
                                        OnRowDataBound="GrdDatos_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="aeronave" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripciones">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Documento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Documento") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Sn") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaUltimo cumplim">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("FechaUltimoServicio") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Orden">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("NumOT") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Work Sheet">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("W_S") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proyeccion">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Proyeccion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frecuencia">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="undmed">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("undmed") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extension">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Extension") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remanente">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Remanente") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec_Dias">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Frec_Dias") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ExtensionDias">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ExtensionDias") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reman_Dias">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Reman_Dias") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UltimaFechaProcesada">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UltimaFechaProcesada") %>' runat="server" Width="100%" />
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
                </asp:View>
                <asp:View ID="Vw1SvcReset" runat="server">
                     <br /><br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitSvcReset" runat="server" Text="servicios reseteable sin cero en el histórico" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarSvcReset" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSvcReset_Click" />
                    <div class="CentrarSvcReset DivMarco">
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdSvcReset" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="servicio">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Servicio") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="frecuencia">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("frecuencia") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="contador">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Contador") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaVencimiento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaVencimiento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pn">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Pn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sn">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Sn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion elemento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Matricula">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" />
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
                <asp:View ID="Vw2UbicTec" runat="server">
                     <br /><br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitUbicTec" runat="server" Text="Ubicaciones Técnicas Sin Series Instaladas" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarUbicTec" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarUbicTec_Click" />
                    <div class="CentrarSvcReset DivMarco">
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdUbicTec" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Matricula">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UltimoNivel">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("UltimoNivel") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DescripcionNivel">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("DescripcionNivel") %>' runat="server" />
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
            <asp:PostBackTrigger ControlID="IbnExcel" />
            <asp:PostBackTrigger ControlID="BtnSvcRestCero" />
            <asp:PostBackTrigger ControlID="BtnUbicaTec" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
