<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmControlContadoresGeneral.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmControlContadoresGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ProcIng</title>
    <style type="text/css">
        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;

        }

        .CentrarContenedor2 {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 80%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .BotonesPpal {
            width: 110%;
            font-size: 12px;
        }

        .LargoDiv {
            height: 60%;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .BorderG {
            border: 1px solid black;
        }

        .GridHisC {
            height: 440px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=LbxLibrosSinProc.ClientID%>').chosen();
            $('#<%=DdlCorrContHK.ClientID%>').chosen();
            $('#<%=DdlCorrContLVSinProcc.ClientID%>').chosen();
            $('#<%=DdlExcesPN.ClientID%>').chosen();
            $('#<%=DdlExcesSN.ClientID%>').chosen();
            $('#<%=DdlDeftPN.ClientID%>').chosen();
            $('#<%=DdlDeftSN.ClientID%>').chosen();
            $('#<%=DdlDeftCodHK.ClientID%>').chosen();
            $('#<%=DdlConvenCodHK.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div class="CentrarContenedor DivMarco">
        <asp:UpdatePanel ID="UplBtnes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                 <br />
                <div class="row">
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="BtnProceLibrV" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnProceLibrV_Click" Text="Procesar Libros de Vuelo" ToolTip="Procesar contadores de cada libro de vuelo por día." />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="BtnAjusExceso" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusExceso_Click" Text="Ajuste Exceso" ToolTip="Eliminar históricos a partir de una fecha." />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="BtnAjusDefect" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusDefect_Click" Text="Ajuste Defecto" ToolTip="Reproceso de contadores de un elemento a partir de una fecha." />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:Button ID="BtnAjusConve" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusConve_Click" Text="Ajuste Conveniencia" ToolTip="Reproceso de contadores de una aeronave y elementos instalados a partir de un rango de fecha." />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnProceLibrV" />
                <asp:PostBackTrigger ControlID="BtnAjusExceso" />
                <asp:PostBackTrigger ControlID="BtnAjusDefect" />
                <asp:PostBackTrigger ControlID="BtnAjusConve" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:MultiView ID="MlVPI" runat="server">
            <asp:View ID="Vw0CorrerContadores" runat="server">
                <asp:UpdatePanel ID="UplProcesarLV" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitProcCont" runat="server" Text="Procesar Contadores" /></h6>
                        <div class="row">
                            <div class="col-sm-2">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblSubTitCorreContLVSinProc" runat="server" Text="Hojas sin procesar" /></h6>
                                <asp:DropDownList ID="LbxLibrosSinProc" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="LbxLibrosSinProc_TextChanged" AutoPostBack="true" />                              
                            </div>
                            <div class="col-sm-2">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblSubTitCorrContHK" runat="server" Text="Hojas sin procesar" /></h6>
                                <asp:DropDownList ID="DdlCorrContHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlCorrContHK_TextChanged" AutoPostBack="true" />
                                <br />
                                <br />
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblSubTitCorrContLV" runat="server" Text="Libros de vuelo sin procesar" /></h6>
                                <asp:DropDownList ID="DdlCorrContLVSinProcc" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlCorrContLVSinProcc_TextChanged" AutoPostBack="true" />
                                <br />
                                <br />
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblSubTitCorrContProcesar" runat="server" Text="Procesar Libro de Vuelo" /></h6>
                                <asp:Button ID="BtnCorrContProcesar" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnCorrContProcesar_Click" Text="Procesar" Enabled="false" OnClientClick="javascript:return confirm('¿Desea procesar el libro de vuelo seleccionado?', 'Mensaje de sistema')" />
                            </div>
                            <div class="col-sm-8">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblSubTitCorreContDatosLV" runat="server" Text="Datos libro de vuelos" /></h6>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Label ID="LblCorrContSn1" runat="server" CssClass="LblEtiquet" Text="Motor 1" />
                                        <asp:TextBox ID="TxtCorrContSn1" runat="server" CssClass="form-control heightCampo" Width="70%" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContStart" runat="server" CssClass="LblEtiquet" Text="Starts" />
                                        <asp:TextBox ID="TxtCorrContStart" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label ID="LblCorrContSn2" runat="server" CssClass="LblEtiquet" Text="Motor 2" />
                                        <asp:TextBox ID="TxtCorrContSn2" runat="server" CssClass="form-control heightCampo" Width="70%" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContStart2" runat="server" CssClass="LblEtiquet" Text="Starts" />
                                        <asp:TextBox ID="TxtCorrContStart2" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Label ID="LblCorrContApu" runat="server" CssClass="LblEtiquet" Text="APU" />
                                        <asp:TextBox ID="TxtCorrContApu" runat="server" CssClass="form-control heightCampo" Width="70%" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContHApu" runat="server" CssClass="LblEtiquet" Text="Horas APU" />
                                        <asp:TextBox ID="TxtCorrContHApu" runat="server" CssClass="form-control heightCampo" Width="70%" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContValor" runat="server" CssClass="LblEtiquet" Text="Valor Contador" />
                                        <asp:TextBox ID="TxtCorrContValor" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContHM" runat="server" CssClass="LblEtiquet" Text="Hora/Minuto" />
                                        <asp:TextBox ID="TxtCorrContHM" runat="server" CssClass="form-control heightCampo" Width="70%" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContVlos" runat="server" CssClass="LblEtiquet" Text="Vuelos" />
                                        <asp:TextBox ID="TxtCorrContVlos" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContLevant" runat="server" CssClass="LblEtiquet" Text="Levantes" />
                                        <asp:TextBox ID="TxtCorrContLevant" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="LblCorrContRin" runat="server" CssClass="LblEtiquet" Text="Rines" />
                                        <asp:TextBox ID="TxtCorrContRin" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:View>
            <asp:View ID="Vw1Exceso" runat="server">
                <div class=".CentrarContenedor2">
                    <asp:UpdatePanel ID="UplExceso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitExceso" runat="server" Text="Procesar por Exceso" /></h6>
                            <div class="row GridHisC">
                                <div class="col-sm-6 table-responsive">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <asp:Label ID="LblExcesPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                            <asp:DropDownList ID="DdlExcesPN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlExcesPN_TextChanged" AutoPostBack="true" />
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="LblExcesSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                            <asp:DropDownList ID="DdlExcesSN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlExcesSN_TextChanged" AutoPostBack="true" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="LbExcesFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Mayor a" />
                                            <asp:TextBox ID="TxtExcesFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label ID="LbExcesHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                            <asp:TextBox ID="TxtExcesHK" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </div>
                                        <div class="col-sm-9">
                                            <asp:Label ID="LbExcesDescE" runat="server" CssClass="LblEtiquet" Text="Descripción" />
                                            <asp:TextBox ID="TxtExcesDescE" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <asp:Button ID="BtnExcesProcesar" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnExcesProcesar_Click" Text="Procesar" Enabled="false" OnClientClick="javascript:return confirm('¿Desea realizar el proceso por exceso?', 'Mensaje de sistema')" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 table-responsive">
                                    <div class="CentarGridAsig table-responsive Scroll">
                                        <asp:GridView ID="GrdExcesoElem" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodElemento,HK,DescElem"
                                            CssClass="DiseñoGrid table-sm" GridLines="Both"
                                            OnRowDeleting="GrdExcesoElem_RowDeleting" OnRowDataBound="GrdExcesoElem_RowDataBound">
                                            <FooterStyle CssClass="GridFooterStyle" />
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Mayor" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CkMyr" Checked='<%# Eval("ComponenteMayor").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P/N">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S/N">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descripción">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="CentarGridAsig table-responsive">
                                            <h6 class="TextoSuperior">
                                                <asp:Label ID="LblTitExcesContConHis" runat="server" Text="Históricos generados manualmente" Visible="false" /></h6>
                                            <asp:GridView ID="GrdExcesoContConHis" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" Visible="false"
                                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                                <FooterStyle CssClass="GridFooterStyle" />
                                                <HeaderStyle CssClass="GridCabecera" />
                                                <RowStyle CssClass="GridRowStyle" />
                                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="P/N">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S/N">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblFecha" Text='<%# Eval("Fecha") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contador">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblContador" Text='<%# Eval("CodContador") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblVlor" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:View>
            <asp:View ID="Vw2Defecto" runat="server">
                <div class="CentrarContenedor2">
                    <asp:UpdatePanel ID="UplDefecto" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblDeftTitulo" runat="server" Text="Procesar por Defecto" /></h6>
                            <div class="row GridHisC">
                                <div class="col-sm-6 table-responsive">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <asp:Label ID="LblDeftPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                            <asp:DropDownList ID="DdlDeftPN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlDeftPN_TextChanged" AutoPostBack="true" />
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="LblDeftSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                            <asp:DropDownList ID="DdlDeftSN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlDeftSN_TextChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label ID="LblDeftCodHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                            <asp:DropDownList ID="DdlDeftCodHK" runat="server" CssClass="heightCampo" Width="100%" />
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label ID="LblDeftFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Mayor a" />
                                            <asp:TextBox ID="TxtDeftFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label ID="LblDeftFechF" runat="server" CssClass="LblEtiquet" Text="Hasta la fecha" />
                                            <asp:TextBox ID="TxtDeftFechF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-9">
                                            <asp:Label ID="LblDeftDescr" runat="server" CssClass="LblEtiquet" Text="Descripción" />
                                            <asp:TextBox ID="TxtDeftDescr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <asp:Button ID="BtnDeftProcesar" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnDeftProcesar_Click" Text="Procesar" Enabled="false" OnClientClick="javascript:return confirm('¿Desea realizar el proceso por defecto?', 'Mensaje de sistema')" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 table-responsive">
                                    <div class="CentarGridAsig table-responsive Scroll">
                                        <asp:GridView ID="GrdDeftElem" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodElemento,HK,DescElem,CodAeronave"
                                            CssClass="DiseñoGrid table-sm" GridLines="Both"
                                            OnRowDeleting="GrdDeftElem_RowDeleting" OnRowDataBound="GrdDeftElem_RowDataBound">
                                            <FooterStyle CssClass="GridFooterStyle" />
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Mayor" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CkMyr" Checked='<%# Eval("ComponenteMayor").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P/N">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S/N">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descripción">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="CentarGridAsig table-responsive">
                                            <h6 class="TextoSuperior">
                                                <asp:Label ID="LblTitDeftEleHisManual" runat="server" Text="Históricos generados manualmente" Visible="false" /></h6>
                                            <asp:GridView ID="GrdDeftElemConHis" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" Visible="false"
                                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                                <FooterStyle CssClass="GridFooterStyle" />
                                                <HeaderStyle CssClass="GridCabecera" />
                                                <RowStyle CssClass="GridRowStyle" />
                                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="P/N">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S/N">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblFecha" Text='<%# Eval("Fecha") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contador">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblContador" Text='<%# Eval("CodContador") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblVlor" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:View>
            <asp:View ID="Vw3Conveniencia" runat="server">
                 <div class="CentrarContenedor2">
                <asp:UpdatePanel ID="UplConveniencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblConvenTitulo" runat="server" Text="Procesar por conveniencia" /></h6>
                        <div class="row GridHisC">
                            <div class="col-sm-6 table-responsive">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Label ID="LblConvenCodHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                        <asp:DropDownList ID="DdlConvenCodHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlConvenCodHK_TextChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblConvenFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Mayor a" />
                                        <asp:TextBox ID="TxtConvenFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblConvenFechF" runat="server" CssClass="LblEtiquet" Text="Hasta la fecha" />
                                        <asp:TextBox ID="TxtConvenFechF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Button ID="BtnConvenProcesar" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnConvenProcesar_Click" Text="Procesar" Enabled="false" OnClientClick="javascript:return confirm('¿Desea realizar el proceso por conveniencia de toda la aeronave?', 'Mensaje de sistema')" />
                                    </div>
                                </div>
                                <br />
                                <div class="CentarGridAsig table-responsive Scroll">
                                    <h6 class="TextoSuperior">
                                        <asp:Label ID="LblTitConvenElemInst" runat="server" Text="Mayores" Visible="false" /></h6>
                                    <asp:GridView ID="GrdConvenElem" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodElemento"
                                        CssClass="DiseñoGrid table-sm" GridLines="Both">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mayor" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkMyr" Checked='<%# Eval("ComponenteMayorR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Motor" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkMtr" Checked='<%# Eval("MotorR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ubicación">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUbica" Text='<%# Eval("CodUbicacionFisica") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Posición">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPosc" Text='<%# Eval("PosicionMotor") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="col-sm-6 table-responsive">
                                <div class="CentarGridAsig table-responsive">
                                    <h6 class="TextoSuperior">
                                        <asp:Label ID="LblTitConvenEleHisManual" runat="server" Text="Históricos generados manualmente" Visible="false" /></h6>
                                    <asp:GridView ID="GrdConvenElemConHis" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" Visible="false"
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFecha" Text='<%# Eval("Fecha") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contador">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblContador" Text='<%# Eval("CodContador") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblVlor" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="DdlConvenCodHK" EventName="TextChanged" />--%>
                    </Triggers>
                </asp:UpdatePanel>
                     </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
