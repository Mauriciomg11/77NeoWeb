<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmHistoricosContadores.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmHistoricosContadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>HC</title>
    <style type="text/css">
           .CentrarContenedor {
            position: absolute;
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
            top:150px
        }
        .AnchoGrid {
            width: 95%;
        }

        .GridHisC {
            height: 600px;
        }

        .BorderG {
            border: 1px solid black;
        }

        .Scroll {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px;
            margin-left: auto;
            margin-right: auto;
            text-align: right;
        }

        .fechas_I_F {
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            border: 1px solid black;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">       
        function myFuncionddl() {
            $('#<%=DdlHisC1HK.ClientID%>').chosen();
            $('#<%=DdlHisC1PN.ClientID%>').chosen();
            $('#<%=DdlHisC1SN.ClientID%>').chosen();
            $('#<%=DdlHisC1CodCont.ClientID%>').chosen();
            $('#<%=DdlHisC2HK.ClientID%>').chosen();
            $('#<%=DdlHisC2PN.ClientID%>').chosen();
            $('#<%=DdlHisC2SN.ClientID%>').chosen();
            $('#<%=DdlHisC2CodCont.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVw" runat="server">
        <asp:View ID="Vw0Principal" runat="server">
            <asp:UpdatePanel ID="UplPpl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-2 "></div>
                            <div class="col-sm-2 "></div>
                            <div class="col-sm-2 ">
                                <asp:Label ID="LblFechIPpl" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial" />
                                <asp:TextBox ID="TxtFechIPpl" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechFPpl" runat="server" CssClass="LblEtiquet" Text="Fecha Final" />
                                <asp:TextBox ID="TxtFechFPpl" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                        </div>
                        <div class="row GridHisC">
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitHisC1Aplicab" runat="server" Text="" /></h6>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:RadioButton ID="RdbHisC1AplicAK" runat="server" CssClass="LblEtiquet" Text="&nbsp Aeronave" GroupName="HisC1Aplic" OnCheckedChanged="RdbHisC1AplicAK_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:RadioButton ID="RdbHisC1AplicSN" runat="server" CssClass="LblEtiquet" Text="&nbsp Elemento" GroupName="HisC1Aplic" OnCheckedChanged="RdbHisC1AplicSN_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisVlrIndiv1" runat="server" CssClass="LblEtiquet" Text="Individual" />
                                        <asp:TextBox ID="TxtHisVlrIndiv1" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisVlrAcumv1" runat="server" CssClass="LblEtiquet" Text="Acumulado" />
                                        <asp:TextBox ID="TxtHisVlrAcumv1" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblHisC1HK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                        <asp:DropDownList ID="DdlHisC1HK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC1HK_TextChanged" AutoPostBack="true" />
                                        <asp:Label ID="LblHisC1PN" runat="server" CssClass="LblEtiquet" Text="P/N" Visible="false" />
                                        <asp:DropDownList ID="DdlHisC1PN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC1PN_TextChanged" AutoPostBack="true" Visible="false" />
                                    </div>
                                    <div id="DivSN" class="col-sm-4">
                                        <asp:Label ID="LblHisC1SN" runat="server" CssClass="LblEtiquet" Text="S/N" Visible="false" />
                                        <asp:DropDownList ID="DdlHisC1SN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC1SN_TextChanged" AutoPostBack="true" Visible="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisC1CodCont" runat="server" CssClass="LblEtiquet" Text="Contador" />
                                        <asp:DropDownList ID="DdlHisC1CodCont" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC1CodCont_TextChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-0">
                                        <br />
                                        <asp:ImageButton ID="IbtHisC1Find" runat="server" ToolTip="Ejecutar consulta" Height="36px" Width="38px" ImageUrl="~/images/FindV3.png" OnClick="IbtHisC1Find_Click" />
                                    </div>
                                    <div class="col-sm-0">
                                        <br />
                                        <asp:ImageButton ID="IbtHisC1Excel" runat="server" ToolTip="Exportar histórico" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtHisC1Excel_Click" />
                                    </div>
                                    <div class="CentarGridAsig Scroll">
                                        <asp:GridView ID="GrdHisC1" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdHist,CodIdCont,SumIndiv,VlrMax,Descripcion"
                                            CssClass="DiseñoGrid Table table-sm" GridLines="Both"
                                            OnRowDeleting="GrdHisC1_RowDeleting" OnRowCommand="GrdHisC1_RowCommand" OnRowDataBound="GrdHisC1_RowDataBound">
                                            <FooterStyle CssClass="GridFooterStyle" />
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblWS" Text='<%# Eval("FechaHist") %>' runat="server" Width="90%" Enabled="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtHC1FechaPP" runat="server" Width="90%" TextMode="Date" MaxLength="10" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Individual">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblHK" Text='<%# Eval("Horas") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtVlrIndivPP" runat="server" Width="50%" TextMode="Number" step="0.01" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acumulado">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblDesc" Text='<%# Eval("TSN_Actual") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Libro de Vuelo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblFec" Text='<%# Eval("CodlV") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitHisC2Aplicab" runat="server" Text="" /></h6>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:RadioButton ID="RdbHisC2AplicAK" runat="server" CssClass="LblEtiquet" Text="&nbsp Aeronave" GroupName="HisC2Aplic" OnCheckedChanged="RdbHisC2AplicAK_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:RadioButton ID="RdbHisC2AplicSN" runat="server" CssClass="LblEtiquet" Text="&nbsp Elemento" GroupName="HisC2Aplic" OnCheckedChanged="RdbHisC2AplicSN_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <%-- <div class="col-sm-3">
                                    <asp:Button ID="BtnHisC2Consult" Height="30px" runat="server" CssClass="btn btn-success" OnClick="BtnHisC2Consult_Click" Text="Consultar" ToolTip="Ejecutar consulta" />
                                </div>--%>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisVlrIndiv2" runat="server" CssClass="LblEtiquet" Text="Individual" />
                                        <asp:TextBox ID="TxtHisVlrIndiv2" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisVlrAcumv2" runat="server" CssClass="LblEtiquet" Text="Acumulado" />
                                        <asp:TextBox ID="TxtHisVlrAcumv2" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblHisC2HK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                        <asp:DropDownList ID="DdlHisC2HK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC2HK_TextChanged" AutoPostBack="true" />
                                        <asp:Label ID="LblHisC2PN" runat="server" CssClass="LblEtiquet" Text="P/N" Visible="false" />
                                        <asp:DropDownList ID="DdlHisC2PN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC2PN_TextChanged" AutoPostBack="true" Visible="false" />
                                    </div>
                                    <div id="DivSN2" class="col-sm-4">
                                        <asp:Label ID="LblHisC2SN" runat="server" CssClass="LblEtiquet" Text="S/N" Visible="false" />
                                        <asp:DropDownList ID="DdlHisC2SN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC2SN_TextChanged" AutoPostBack="true" Visible="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblHisC2CodCont" runat="server" CssClass="LblEtiquet" Text="Contador" />
                                        <asp:DropDownList ID="DdlHisC2CodCont" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlHisC2CodCont_TextChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-0">
                                        <br />
                                        <asp:ImageButton ID="IbtHisC2Find" runat="server" ToolTip="Ejecutar consulta" Height="36px" Width="38px" ImageUrl="~/images/FindV3.png" OnClick="IbtHisC2Find_Click" />
                                    </div>
                                    <div class="col-sm-0">
                                        <br />
                                        <asp:ImageButton ID="IbtHisC2Excel" runat="server" ToolTip="Exportar histórico" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtHisC2Excel_Click" />
                                    </div>
                                    <div class="CentarGridAsig Scroll">
                                        <asp:GridView ID="GrdHisC2" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdHist,CodIdCont,SumIndiv,VlrMax,Descripcion"
                                            CssClass="DiseñoGrid table-sm" GridLines="Both"
                                            OnRowCommand="GrdHisC2_RowCommand" OnRowDeleting="GrdHisC2_RowDeleting" OnRowDataBound="GrdHisC2_RowDataBound">
                                            <FooterStyle CssClass="GridFooterStyle" />
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblWS2" Text='<%# Eval("FechaHist") %>' runat="server" Width="90%" Enabled="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtHC2FechaPP" runat="server" Width="90%" TextMode="Date" MaxLength="10" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Individual">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblHK2" Text='<%# Eval("Horas") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtVlrIndiv2PP" runat="server" Width="50%" TextMode="Number" step="0.01" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acumulado">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblDesc2" Text='<%# Eval("TSN_Actual") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Libro de Vuelo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblFec2" Text='<%# Eval("CodlV") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtHisC1Excel" />
                    <asp:PostBackTrigger ControlID="IbtHisC2Excel" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1ContNull" runat="server">
            <asp:UpdatePanel ID="UplContNull" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                         <br /> <br />
                        <asp:Label ID="LblTitContNull" runat="server" Text="S/N con valor NULL en el histórico" /></h6>
                    <asp:ImageButton ID="IbtClosContNull" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClosContNull_Click" ImageAlign="Right" />
                    <div class="col-sm-12 CentarGridAsig table-responsive AnchoGrid">
                        <asp:GridView ID="GrdContNull" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" Width="100%">
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <Columns>
                                <asp:TemplateField HeaderText="Matrícula">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contador">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodContador") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtClosContNull" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
