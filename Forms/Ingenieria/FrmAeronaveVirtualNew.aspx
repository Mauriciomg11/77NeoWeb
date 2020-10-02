<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" CodeBehind="FrmAeronaveVirtualNew.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmAeronaveVirtualNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Manto</title>
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .MyCalendar .ajax__calendar_container {
            border: 1px solid #646464;
            background-color: Gray;
            color: Black;
            font-family: Arial;
            font-size: 14px;
            font-weight: bold;
        }

        .DivGrid {
            /*margin: 0 auto;*/
            text-align: left;
            width: 100%;
            /*height: 600px;*/
            /*top: 15%;*/
            margin-top: 0px;
        }

        .DivGridAVirtual {
            height: 450px;
            /*top: 15%;*/
            margin-top: 0px;
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
        function Decimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                var inputValue = $("#inputfield").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function myFuncionddl() {
            $('#<%=DdlAeroInsElem.ClientID%>').chosen();
            $('#<%=DdlPosicInsElem.ClientID%>').chosen();
            $('#<%=DdlHkConsAeroVirtual.ClientID%>').chosen();
            $('#<%=DdlAeroRemElem.ClientID%>').chosen();
            $('#<%=DdlPosicRemElem.ClientID%>').chosen();
            $('#<%=DdlAeroInsMay.ClientID%>').chosen();
            $('#<%=DdlPosicInsMay.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo"></asp:Label></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlBtnPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Table ID="TblBtnPpal" runat="server">
                <asp:TableRow>
                    <asp:TableCell Width="2%">
                        <asp:Button ID="BtnInsElem" CssClass="btn btn-outline-primary" runat="server" Text="Instalar Elemento" OnClick="BtnInsElem_Click" />&nbsp
                        <asp:Button ID="BtnRemElem" CssClass="btn btn-outline-primary" runat="server" Text="Remover Elemento" OnClick="BtnRemElem_Click" />&nbsp
                        <asp:Button ID="BtnInsMayor" CssClass="btn btn-outline-primary" runat="server" Text="Instalar Mayor" OnClick="BtnInsMayor_Click" />&nbsp
                        <asp:Button ID="BtnRemMayor" CssClass="btn btn-outline-primary" runat="server" Text="Remover Mayor" OnClick="BtnRemMayor_Click" />&nbsp
                        <asp:Button ID="BtnInsSubC" CssClass="btn btn-outline-primary" runat="server" Text="Instalar Sub-componente" OnClick="BtnInsSubC_Click" />&nbsp
                        <asp:Button ID="BtnRemSubC" CssClass="btn btn-outline-primary" runat="server" Text="Remover Sub-componente" OnClick="BtnRemSubC_Click" />&nbsp
                        <asp:Button ID="BtnCrearElem" runat="server" CssClass="btn btn-success" Text="Nuevo Elemento" OnClick="BtnCrearElem_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnInsElem" />
            <asp:PostBackTrigger ControlID="BtnRemElem" />
            <asp:PostBackTrigger ControlID="BtnInsMayor" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:MultiView ID="MultVw" runat="server">
        <asp:View ID="Vw0InsElem" runat="server">
            <asp:UpdatePanel ID="UplInstElem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInsElel" runat="server" Text="Instalación de un elemento" /></h6>
                    <asp:Label ID="LblAeroInsElem" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroInsElem" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroInsElem_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusInsEle" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNInsElem_Click" />&nbsp
                    <asp:Button ID="BtnSNInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNInsElem_Click" />&nbsp
                    <asp:Button ID="BtnUltNivInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/N" OnClick="BtnUltNivInsElem_Click" ToolTip="Ultimo Nivel" />&nbsp
                    <asp:Button ID="BtnAKVirtualInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualInsElem_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarInsElem" CssClass="btn btn-success" runat="server" Text="Instalar" OnClick="BtnGuardarInsElem_Click" ToolTip="Realizar la instalación" OnClientClick="return confirm('¿Desea realizar la instalación?');" /><br />
                    <asp:Label ID="LblPnInsElem" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnInsElem" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnInsElem" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnInsElem" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecInsElem" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecInsElem" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicInsElem" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicInsElem" runat="server" CssClass="heightCampo" Width="10%" />
                    <asp:Label ID="LblFechaInsElem" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:ImageButton ID="IbtFechaInsElem" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                    <asp:TextBox ID="TxtFechaInsElem" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" OnTextChanged="TxtFechaInsElem_TextChanged" AutoPostBack="true" />
                    <ajaxToolkit:CalendarExtender ID="CalFechaInsElem" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtFechaInsElem" TargetControlID="TxtFechaInsElem" Format="dd/MM/yyyy" />
                    <asp:Button ID="BtnCompensac" CssClass="btn btn-danger" runat="server" Height="25px" Width="18px" Text="C" Font-Size="9px" ToolTip="Libros de vuelo para la compensación" OnClick="BtnCompensac_Click" OnClientClick="return confirm('¿Desea realizar la compensación?');" Visible="false" />
                    <asp:Label ID="LblMotivInsElem" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivInsElem" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblInsElem" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitContadores" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContInsElem" runat="server" EmptyDataText="Sin histórico..!"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitServicios" runat="server" Text="Elementos Disponibles" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" DataKeyNames="CodElemento"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging" OnRowDataBound="GrdBusq_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Selección" SelectText="Subir" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                    </asp:GridView>
                                </div>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdSvcInsElem" runat="server" AutoGenerateColumns="False" DataKeyNames="FVAnt,CodIdContadorElem,CodIdContaSrvManto,CodElemento"
                                        EmptyDataText="Sin servicios asignados..!" Visible="false" CssClass="DiseñoGrid table-sm" GridLines="Both"
                                        OnRowDataBound="GrdSvcInsElem_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="O.T." HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodOT" Text='<%# Eval("CodOT") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ultimo Cumplim" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtFecUltCumpl" Text='<%# Eval("FechaVencWeb") %>' runat="server" Width="75%" Enabled="false" OnTextChanged="TxtFecUltCumpl_TextChanged" />
                                                    <asp:ImageButton ID="IbtFecUltCumpl" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                                    <ajaxToolkit:CalendarExtender ID="CalFecUltCumpl" runat="server" PopupButtonID="IbtFecUltCumpl" TargetControlID="TxtFecUltCumpl" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reset" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbReset" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reporte" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtReporte" runat="server" MaxLength="150" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Servicio(s)" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblServicio" Text='<%# Eval("Descripcion") %>' runat="server" Font-Size="8px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblContador" Text='<%# Eval("Contador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec." HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFrec" Text='<%# Eval("Frecuencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec. Días" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFrecDia" Text='<%# Eval("NroDias") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Ult. Cumplim" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCumpHist" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Generar Histórico" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbGenerarHist" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnAKVirtualInsElem" />
                    <asp:PostBackTrigger ControlID="BtnCompensac" />
                    <asp:AsyncPostBackTrigger ControlID="TxtFechaInsElem" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1AeroVirtual" runat="server">
            <asp:UpdatePanel ID="UplListaAeroVirtual" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAeroVirtual" runat="server" Text="Ubicaciones con elementos instalados y pendientes por instalar" /></h6>
                    <asp:ImageButton ID="IbtCerrarAeroVirtual" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAeroVirtual_Click" ImageAlign="Right" />
                    <asp:Label ID="Label2" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlHkConsAeroVirtual" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlHkConsAeroVirtual_TextChanged" AutoPostBack="true" />
                    <div class="DivContendorGrid DivGridAVirtual">
                        <asp:GridView ID="GrdListaAeroVirtual" runat="server" EmptyDataText="Sin configurar..!"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both"
                            OnRowDataBound="GrdListaAeroVirtual_RowDataBound">
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarAeroVirtual" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2Compensacion" runat="server">
            <asp:UpdatePanel ID="UplCompensacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="TxtTitCompensacion" runat="server" Text="Compensación" /></h6>
                    <asp:ImageButton ID="IbtCerrarCompensacion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCompensacion_Click" ImageAlign="Right" />
                    <asp:Button ID="BtnCompensReinicio" CssClass="btn btn-success" runat="server" Text="Limpiar" OnClick="BtnCompensReinicio_Click" ToolTip="volver a marcar el trayecto para la compensación" OnClientClick="return confirm('¿Desea limpiar y volver a marcar el trayecto para la compensación?');" /><br />
                    <br />
                    <asp:CheckBox ID="CkbCompensInicioDia" runat="server" CssClass="LblEtiquet" Text="Evento realizado antes del primer vuelo del día" Font-Size="15px" Font-Bold="true" OnCheckedChanged="CkbCompensInicioDia_CheckedChanged" AutoPostBack="true" /><br />
                    <asp:Label ID="Label1" runat="server" CssClass="LblEtiquet" Text="Marcar el trayecto que finaliza antes del evento" />
                    <div class="DivContendorGrid">
                        <asp:GridView ID="GrdCompensLv" runat="server" EmptyDataText="Sin libro de vuelo..!" AutoGenerateColumns="False" AutoGenerateSelectButton="False"
                            CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" DataKeyNames="ID,FechaReporte,HoraDespegue"
                            OnRowDataBound="GrdCompensLv_RowDataBound" OnRowCommand="GrdCompensLv_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnSelect" CssClass="btn btn-success" runat="server" CommandName="Select" ToolTip="Seleccionar" Width="20px" Height="20px" OnClientClick="return confirm('¿Desea seleccionar este registro para la compensación?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbOK" Checked='<%# Eval("OK").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Libro de Vuelo">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodLV" Text='<%# Eval("CodLibroVuelo") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Origen">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodOrigen") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Destino">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodDestino") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Horas">
                                    <ItemTemplate>
                                        <asp:Label ID="Horas" Text='<%# Eval("NumHoraCiclo") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Horas Acumuladas">
                                    <ItemTemplate>
                                        <asp:Label ID="HoraAcum" Text='<%# Eval("HorasAcum") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ciclos Acumulados">
                                    <ItemTemplate>
                                        <asp:Label ID="CicloAcum" Text='<%# Eval("CiclosAcum") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Horas Remanente">
                                    <ItemTemplate>
                                        <asp:Label ID="HoraRemain" Text='<%# Eval("HorasAcumResta") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ciclos Remanente">
                                    <ItemTemplate>
                                        <asp:Label ID="CicloRemain" Text='<%# Eval("CiclosAcumResta") %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarCompensacion" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3RemElem" runat="server">
            <asp:UpdatePanel ID="UplRemElem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRemEle" runat="server" Text="Remoción de un elemento" /></h6>
                    <asp:Label ID="LblAeroRemElem" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroRemElem" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroRemElem_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusRemEle" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtRemBusqueda" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNRemElem_Click" />&nbsp
                    <asp:Button ID="BtnSNRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNRemElem_Click" />&nbsp
                    <asp:Button ID="BtnUltNivRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/N" OnClick="BtnUltNivRemElem_Click" ToolTip="Ultimo Nivel" />&nbsp
                    <asp:Button ID="BtnAKVirtualRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualRemElem_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp
                    <asp:Button ID="BtnAbrirOTCerrar" CssClass="btn btn-danger" runat="server" Height="33px" Text="O.T. Abiertas" OnClick="BtnAbrirOTCerrar_Click" Visible="false" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarRemElem" CssClass="btn btn-success" runat="server" Text="Remover" OnClick="BtnGuardarRemElem_Click" ToolTip="Realizar la remoción" OnClientClick="return confirm('¿Desea realizar la remoción?');" /><br />
                    <asp:Label ID="LblPnRemElem" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnRemElem" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnRemElem" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnRemElem" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecRemElem" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecRemElem" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicRemElem" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicRemElem" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                    <asp:Label ID="LblFechaRemElem" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:ImageButton ID="IbtFechaRemElem" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                    <asp:TextBox ID="TxtFechaRemElem" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" OnTextChanged="TxtFechaRemElem_TextChanged" AutoPostBack="true" />
                    <ajaxToolkit:CalendarExtender ID="CalFechaRemElem" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtFechaRemElem" TargetControlID="TxtFechaRemElem" Format="dd/MM/yyyy" />
                    <asp:Button ID="BtnRemCompensac" CssClass="btn btn-danger" runat="server" Height="25px" Width="18px" Text="C" Font-Size="9px" ToolTip="Libros de vuelo para la compensación" OnClick="BtnRemCompensac_Click" OnClientClick="return confirm('¿Desea realizar la compensación?');" Visible="false" />
                    <asp:Label ID="LblMotivRemElem" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivRemElem" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblRemElem" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitRemContadores" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContRemElem" runat="server" EmptyDataText="Sin histórico..!"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitRemServicios" runat="server" Text="Elementos disponibles" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdRemBusqElem" runat="server" EmptyDataText="No existen elementos con el dato seleccionado ..!" DataKeyNames="CodElemento,CodUbicacionSuperior"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdRemBusqElem_SelectedIndexChanged" OnPageIndexChanging="GrdRemBusqElem_PageIndexChanging" OnRowDataBound="GrdRemBusqElem_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Selección" SelectText="Subir" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnAKVirtualRemElem" />
                    <asp:PostBackTrigger ControlID="BtnRemCompensac" />
                    <asp:PostBackTrigger ControlID="BtnAbrirOTCerrar" />
                    <asp:AsyncPostBackTrigger ControlID="TxtFechaRemElem" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw4CerrarOT" runat="server">
            <asp:UpdatePanel ID="UplCerrarOT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="TxtTitCierreOT" runat="server" Text="Ordenes abiertas (las ordenes abiertas con posibilidad de realizar el cierre sin mover la fecha del servicio)" /></h6>
                    <asp:ImageButton ID="IbtCerrarOTcierre" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarOTcierre_Click" ImageAlign="Right" />
                    <br />
                    <div class="DivContendorGrid">
                        <asp:GridView ID="GrdOtCerrar" runat="server" AutoGenerateColumns="False" DataKeyNames="CentroCosto,FechaInicio,EjecPasos"
                            EmptyDataText="Sin OT para cerrar..!" CssClass="DiseñoGrid table-sm" GridLines="Both">
                            <Columns>
                                <asp:TemplateField HeaderText="Ck" HeaderStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbOk" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="O.T." HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodOT" Text='<%# Eval("CodNumOrdenTrab") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblFechaReg" Text='<%# Eval("FechaRegWeb") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trabajo Requerido" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTrabReq" Text='<%# Eval("Descripcion") %>' runat="server" Font-Size="10px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Motivo cierre" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtMotivo" runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" Font-Size="10px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Work Sheet" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblWS" Text='<%# Eval("WS") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarOTcierre" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw5InsMay" runat="server">
            <asp:UpdatePanel ID="UplInsMay" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInsMay" runat="server" Text="Instalación de un Mayor" /></h6>
                    <asp:Label ID="LblAeroInsMay" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroInsMay" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroInsMay_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusInsMay" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtBusqInsMay" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNInsMay_Click" />&nbsp
                    <asp:Button ID="BtnSNInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNInsMay_Click" />&nbsp
                    <asp:Button ID="BtnUltNivInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/N" OnClick="BtnUltNivInsMay_Click" ToolTip="Ultimo Nivel" />&nbsp
                    <asp:Button ID="BtnAKVirtualInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualInsMay_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarInsMay" CssClass="btn btn-success" runat="server" Text="Instalar" OnClick="BtnGuardarInsMay_Click" ToolTip="Realizar la instalación" OnClientClick="return confirm('¿Desea realizar la instalación?');" /><br />
                    <asp:Label ID="LblPnInsMay" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnInsMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnInsMay" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnInsMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecInsMay" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecInsMay" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicInsMay" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicInsMay" runat="server" CssClass="heightCampo" Width="10%" />
                    <asp:Label ID="LblFechaInsMay" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:ImageButton ID="IbtFechaInsMay" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                    <asp:TextBox ID="TxtFechaInsMay" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" OnTextChanged="TxtFechaInsMay_TextChanged" AutoPostBack="true" />
                    <ajaxToolkit:CalendarExtender ID="CalFechaInsMay" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtFechaInsMay" TargetControlID="TxtFechaInsMay" Format="dd/MM/yyyy" />
                    <asp:Button ID="BtnCompensacInsMay" CssClass="btn btn-danger" runat="server" Height="25px" Width="18px" Text="C" Font-Size="9px" ToolTip="Libros de vuelo para la compensación" OnClick="BtnCompensacInsMay_Click" OnClientClick="return confirm('¿Desea realizar la compensación?');" Visible="false" />
                    <asp:Label ID="LblMotivInsMay" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivInsMay" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblInsMay" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitHisContInsMay" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContInsMay" runat="server" EmptyDataText="Sin histórico..!"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitSvcInsMay" runat="server" Text="Mayores Disponibles" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusqMayDisp" runat="server" EmptyDataText="No existen registros ..!" DataKeyNames="CodElemento"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusqMayDisp_SelectedIndexChanged" OnPageIndexChanging="GrdBusqMayDisp_PageIndexChanging" OnRowDataBound="GrdBusqMayDisp_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Selección" SelectText="Subir" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                    </asp:GridView>
                                </div>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdSvcInsMay" runat="server" AutoGenerateColumns="False" DataKeyNames="FVAnt,CodIdContadorElem,CodIdContaSrvManto,CodElemento"
                                        EmptyDataText="Sin servicios asignados al mayor..!" Visible="false" CssClass="DiseñoGrid table-sm" GridLines="Both"
                                        OnRowDataBound="GrdSvcInsMay_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="O.T." HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodOT" Text='<%# Eval("CodOT") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ultimo Cumplim" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtFecUltCumplMay" Text='<%# Eval("FechaVencWeb") %>' runat="server" Width="75%" Enabled="false" OnTextChanged="TxtFecUltCumplMay_TextChanged" />
                                                    <asp:ImageButton ID="IbtFecUltCumplMay" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                                    <ajaxToolkit:CalendarExtender ID="CalFecUltCumplMay" runat="server" PopupButtonID="IbtFecUltCumplMay" TargetControlID="TxtFecUltCumplMay" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reset" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbReset" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reporte" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtReporte" runat="server" MaxLength="150" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Servicio(s)" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblServicio" Text='<%# Eval("Descripcion") %>' runat="server" Font-Size="8px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblContador" Text='<%# Eval("Contador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec." HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFrec" Text='<%# Eval("Frecuencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec. Días" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFrecDia" Text='<%# Eval("NroDias") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Ult. Cumplim" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCumpHist" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Generar Histórico" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbGenerarHist" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                  <Triggers>
                    <asp:PostBackTrigger ControlID="BtnAKVirtualInsMay" />
                    <asp:PostBackTrigger ControlID="BtnCompensacInsMay" />
                    <asp:AsyncPostBackTrigger ControlID="TxtFechaInsMay" EventName="TextChanged" />
                </Triggers>                
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
