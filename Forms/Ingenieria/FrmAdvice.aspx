<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAdvice.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmAdvice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Scroll-table2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">

        function myFuncionddl() {
            $('#<%=DdlPN.ClientID%>').chosen();
            $('#<%=DdlSN.ClientID%>').chosen();
            $('#<%=DdlModel.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplInstSubC" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVieLV" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                     <br /> <br />
                    <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                    <asp:DropDownList ID="DdlPN" runat="server" CssClass="heightCampo" Width="20%" OnTextChanged="DdlPN_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                    <asp:DropDownList ID="DdlSN" runat="server" CssClass="heightCampo" Width="20%" />
                    <asp:Label ID="LblModel" runat="server" CssClass="LblEtiquet" Text="Modelo:" />
                    <asp:DropDownList ID="DdlModel" runat="server" CssClass="heightCampo" Width="15%" />
                    <asp:Button ID="BtnConsultar" CssClass="btn btn-primary" runat="server" Height="33px" Text="Consultar" OnClick="BtnConsultar_Click" />
                    <asp:Button ID="BtnImprimir" CssClass="btn btn-primary" runat="server" Height="33px" Text="Imprimir" OnClick="BtnImprimir_Click" Enabled="false" />
                    &nbsp&nbsp
                    <asp:Label ID="LblHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                    <asp:TextBox ID="TxtHK" runat="server" CssClass="heightCampo" Enabled="false" Width="10%" />
                    <div class="row">
                        <div class="col-sm-4">
                            <asp:Label ID="LblDesc" runat="server" CssClass="LblEtiquet" Text="Descripción" />
                            <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblTT" runat="server" CssClass="LblEtiquet" Text="TT (TSN/HRS):" />
                            <asp:TextBox ID="TxtTT" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblTSO" runat="server" CssClass="LblEtiquet" Text="TSO:" />
                            <asp:TextBox ID="TxtTSO" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblCSN" runat="server" CssClass="LblEtiquet" Text="CSN:" />
                            <asp:TextBox ID="TxtCSN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblCSO" runat="server" CssClass="LblEtiquet" Text="CSO:" />
                            <asp:TextBox ID="TxtCSO" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblSSN" runat="server" CssClass="LblEtiquet" Text="SSN:" />
                            <asp:TextBox ID="TxtSSN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblFechaActualiza" runat="server" CssClass="LblEtiquet" Text="Fecha Actualización" />
                            <asp:TextBox ID="TxtFechaActualiza" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive Scroll-table2">
                        <asp:GridView ID="GrdAdvice" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                            <Columns>
                                <asp:TemplateField HeaderText="Ata" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblAta" Text='<%# Eval("SubAta") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblNivel" Text='<%# Eval("DescSubCapituloN3") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ubicación Técnica" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUbiTec" Text='<%# Eval("CodCapitulo") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción elemento" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDescr" Text='<%# Eval("DescrElem") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Servicio" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblServic" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Frec." HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblFrec" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Días" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDias" Text='<%# Eval("FrecuenciaDia") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContador" Text='<%# Eval("Contador") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acum" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcum" Text='<%# Eval("Acum") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reman." HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReman" Text='<%# Eval("Remanente") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reman. Días" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemanD" Text='<%# Eval("RemanenteDia") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Informe" runat="server">
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión" /></h6>
                    <asp:ImageButton ID="IbtCerrarImpresion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpresion_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RvwReporte" runat="server" Width="98%" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnImprimir" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
