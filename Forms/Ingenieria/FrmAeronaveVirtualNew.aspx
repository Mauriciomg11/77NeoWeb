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

        /*.DivGrid {*/
            /*margin: 0 auto;*/
            /*text-align: left;
            width: 100%;*/
            /*height: 600px;*/
            /*top: 15%;*/
            /*margin-top: 0px;
        }*/

        .DivGridAVirtual {
            height: 450px;
            /*top: 15%;*/
            margin-top: 0px;
        }

        .TitElem {
            width: 70%;
        }

        .TitElemContad {
            width: 20%;
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
            $('#<%=DdlPnVisualMay.ClientID%>').chosen();
            $('#<%=DdlSnVisualMay.ClientID%>').chosen();
            $('#<%=DdlAeroRemMay.ClientID%>').chosen();
            $('#<%=DdlPosicRemMay.ClientID%>').chosen();
            $('#<%=DdlPNInsSubC.ClientID%>').chosen();
            $('#<%=DdlSNInsSubC.ClientID%>').chosen();
            $('#<%=DdlModelInsSubC.ClientID%>').chosen();
            $('#<%=DdlPosicInsSubC.ClientID%>').chosen();
            $('#<%=DdlPNRemSubC.ClientID%>').chosen();
            $('#<%=DdlSNRemSubC.ClientID%>').chosen();
            $('#<%=DdlModelRemSubC.ClientID%>').chosen();
            $('#<%=DdlPosicRemSubC.ClientID%>').chosen();
            $('#<%=DdlCrearElemPn.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlBtnPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br /><br />
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
            <asp:PostBackTrigger ControlID="BtnRemMayor" />
            <asp:PostBackTrigger ControlID="BtnInsSubC" />
            <asp:PostBackTrigger ControlID="BtnRemSubC" />
            <asp:PostBackTrigger ControlID="BtnCrearElem" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:MultiView ID="MultVw" runat="server">
        <asp:View ID="Vw0InsElem" runat="server">
            <asp:UpdatePanel ID="UplInstElem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInsEle" runat="server" Text="Instalación de un elemento" /></h6>
                    <asp:Label ID="LblAeroInsElem" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroInsElem" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroInsElem_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusInsEle" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNInsElem_Click" />&nbsp
                    <asp:Button ID="BtnSNInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNInsElem_Click" />&nbsp
                    <asp:Button ID="BtnUltNivInsElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivInsElem_Click" ToolTip="Ubicación Técnica" />&nbsp
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
                    <%--<asp:ImageButton ID="IbtFechaInsElem" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />--%>
                    <asp:TextBox ID="TxtFechaInsElem" runat="server" CssClass="form-control-sm heightCampo" onKeyDown="return false" TextMode="Date" Width="11%" OnTextChanged="TxtFechaInsElem_TextChanged" AutoPostBack="true" />
                    <%--<ajaxToolkit:CalendarExtender ID="CalFechaInsElem" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtFechaInsElem" TargetControlID="TxtFechaInsElem" Format="dd/MM/yyyy" />--%>
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
                                    <asp:GridView ID="GrdHisContInsElem" runat="server" AutoGenerateColumns="False" EmptyDataText="Sin histórico..!"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                                            <asp:CommandField HeaderText="Select" SelectText="Install" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
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
                                                    <asp:TextBox ID="TxtFecUltCumpl" Text='<%# Eval("FechaVencWeb") %>' runat="server" Width="100%" onKeyDown="return false" TextMode="Date" OnTextChanged="TxtFecUltCumpl_TextChanged" />
                                                    <%--<asp:ImageButton ID="IbtFecUltCumpl" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                                    <ajaxToolkit:CalendarExtender ID="CalFecUltCumpl" runat="server" PopupButtonID="IbtFecUltCumpl" TargetControlID="TxtFecUltCumpl" Format="dd/MM/yyyy" CssClass="MyCalendar" />--%>
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
                    <asp:PostBackTrigger ControlID="BtnGuardarInsElem" />
                    <asp:PostBackTrigger ControlID="BtnAKVirtualInsElem" />
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
                    <asp:Label ID="LblAeroVirtualHK" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlHkConsAeroVirtual" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlHkConsAeroVirtual_TextChanged" AutoPostBack="true" />
                    <div class="DivContendorGrid DivGridAVirtual">
                        <asp:GridView ID="GrdListaAeroVirtual" runat="server" EmptyDataText="Sin configurar..!" AutoGenerateColumns="False" DataKeyNames="Mayor"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdListaAeroVirtual_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="UltimoNivel" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUlNiv" Text='<%# Eval("UltimoNivel") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Desc_Elem" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesElem" Text='<%# Eval("Desc_Ref") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posición" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" />
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
                    <asp:Label ID="LblMarcarTry" runat="server" CssClass="LblEtiquet" Text="Marcar el trayecto que finaliza antes del evento" />
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
                        <asp:Label ID="LblTitRemEle" runat="server" Text="Remoción de un Elemento" /></h6>
                    <asp:Label ID="LblAeroRemElem" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroRemElem" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroRemElem_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusRemEle" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtRemBusqueda" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNRemElem_Click" />&nbsp
                    <asp:Button ID="BtnSNRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNRemElem_Click" />&nbsp
                    <asp:Button ID="BtnUltNivRemElem" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivRemElem_Click" ToolTip="Ubicación Técnica" />&nbsp
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
                    <asp:TextBox ID="TxtFechaRemElem" runat="server" CssClass="form-control-sm heightCampo" onKeyDown="return false" TextMode="Date" Width="12%" OnTextChanged="TxtFechaRemElem_TextChanged" AutoPostBack="true" />
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
                                    <asp:GridView ID="GrdHisContRemElem" runat="server" EmptyDataText="Sin histórico..!" AutoGenerateColumns="False"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitRemServicios" runat="server" Text="Elementos a Remover" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdRemBusqElem" runat="server" EmptyDataText="No existen elementos con el dato seleccionado ..!" DataKeyNames="CodElemento,CodUbicacionSuperior"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdRemBusqElem_SelectedIndexChanged" OnPageIndexChanging="GrdRemBusqElem_PageIndexChanging" OnRowDataBound="GrdRemBusqElem_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
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
                    <asp:PostBackTrigger ControlID="BtnGuardarRemElem" />
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
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Font-Size="10px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Font-Size="10px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trabajo Requerido" HeaderStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTrabReq" Text='<%# Eval("Descripcion") %>' runat="server" Font-Size="10px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Motivo cierre" HeaderStyle-Width="30%">
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
                    <asp:Button ID="BtnUltNivInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivInsMay_Click" ToolTip="Ubicación Técnica" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAKVirtualInsMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualInsMay_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp
                    <asp:Button ID="BtnVisualizarMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="Mayores" OnClick="BtnVisualizarMay_Click" ToolTip="Visualizar mayores y los subcomponentes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarInsMay" CssClass="btn btn-success" runat="server" Text="Instalar" OnClick="BtnGuardarInsMay_Click" ToolTip="Realizar la instalación" OnClientClick="return confirm('¿Desea realizar la instalación?');" /><br />
                    <asp:Label ID="LblPnInsMay" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnInsMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnInsMay" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnInsMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecInsMay" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecInsMay" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicInsMay" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicInsMay" runat="server" CssClass="heightCampo" Width="10%" OnTextChanged="DdlPosicInsMay_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblFechaInsMay" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                   <asp:TextBox ID="TxtFechaInsMay" runat="server" CssClass="form-control-sm heightCampo"  onKeyDown="return false" TextMode="Date" Width="11%" OnTextChanged="TxtFechaInsMay_TextChanged" AutoPostBack="true" />                  
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
                                    <asp:GridView ID="GrdHisContInsMay" runat="server" AutoGenerateColumns="False" EmptyDataText="Sin histórico..!"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitSvcInsMay" runat="server" Text="Mayores Disponibles" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusqMayDisp" runat="server" EmptyDataText="No existen registros ..!" DataKeyNames="CodElemento,Motor"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusqMayDisp_SelectedIndexChanged" OnPageIndexChanging="GrdBusqMayDisp_PageIndexChanging" OnRowDataBound="GrdBusqMayDisp_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
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
                                                    <asp:TextBox ID="TxtFecUltCumplMay" Text='<%# Eval("FechaVencWeb") %>' runat="server" Width="100%" onKeyDown="return false" TextMode="Date" OnTextChanged="TxtFecUltCumplMay_TextChanged" AutoPostBack="true" />
                                                    <%--<asp:ImageButton ID="IbtFecUltCumplMay" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />--%>
                                                    <%--<ajaxToolkit:CalendarExtender ID="CalFecUltCumplMay" runat="server" PopupButtonID="IbtFecUltCumplMay" TargetControlID="TxtFecUltCumplMay" Format="dd/MM/yyyy" CssClass="MyCalendar" />--%>
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
                    <asp:PostBackTrigger ControlID="BtnVisualizarMay" />
                    <asp:PostBackTrigger ControlID="BtnGuardarInsMay" />
                    <asp:AsyncPostBackTrigger ControlID="TxtFechaInsMay" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlPosicInsMay" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw6VisualizarMay" runat="server">
            <asp:UpdatePanel ID="UplVisualizarMay" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitVisualizarMay" runat="server" Text="Mayores / Sub - Componentes" /></h6>
                    <asp:ImageButton ID="IbtCerrarVisualMay" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarVisualMay_Click" ImageAlign="Right" />
                    <asp:Label ID="LblPNVisualMay" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                    <asp:DropDownList ID="DdlPnVisualMay" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlPnVisualMay_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblSNVisualMay" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                    <asp:DropDownList ID="DdlSnVisualMay" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlSnVisualMay_TextChanged" AutoPostBack="true" />
                    <div class="DivContendorGrid DivGridAVirtual">
                        <asp:GridView ID="GrdVisualMay" runat="server" EmptyDataText="Sin configurar..!" AutoGenerateColumns="False"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both"
                            OnRowDataBound="GrdVisualMay_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Ubicación Técnica" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUT" Text='<%# Eval("Ubica_Tecn") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("Sn") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posición" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPosc" Text='<%# Eval("Posic") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aeronave" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblAK" Text='<%# Eval("Matricula") %>' runat="server" />
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
                    <asp:PostBackTrigger ControlID="IbtCerrarVisualMay" />
                    <asp:AsyncPostBackTrigger ControlID="DdlPnVisualMay" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlSnVisualMay" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw7RemMay" runat="server">
            <asp:UpdatePanel ID="UplRemMay" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRemMay" runat="server" Text="Remoción de un Mayor" /></h6>
                    <asp:Label ID="LblAeroRemElMay" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAeroRemMay" runat="server" CssClass="heightCampo" Width="8%" OnTextChanged="DdlAeroRemMay_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblBusRemMay" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtRemMayBusqueda" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNRemMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNRemMay_Click" />&nbsp
                    <asp:Button ID="BtnSNRemMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNRemMay_Click" />&nbsp
                    <asp:Button ID="BtnUltNivRemMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivRemMay_Click" ToolTip="Ubicación Técnica" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAKVirtualRemMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualRemMay_Click" ToolTip="Visualizar mayores instalados y ubicaciones pendientes" />&nbsp
                    <asp:Button ID="BtnVisualizarRemMay" CssClass="btn btn-primary" runat="server" Height="33px" Text="Mayores" OnClick="BtnVisualizarRemMay_Click" ToolTip="Visualizar mayores y los subcomponentes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAbrirOTCerrarRemMay" CssClass="btn btn-danger" runat="server" Height="33px" Text="O.T. Abiertas" OnClick="BtnAbrirOTCerrarRemMay_Click" Visible="false" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarRemMay" CssClass="btn btn-success" runat="server" Text="Remover" OnClick="BtnGuardarRemMay_Click" ToolTip="Realizar la remoción" OnClientClick="return confirm('¿Desea realizar la remoción?');" /><br />
                    <asp:Label ID="LblPnRemMay" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnRemMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnRemMay" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnRemMay" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecRemMay" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecRemMay" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicRemMay" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicRemMay" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                    <asp:Label ID="LblFechaRemMay" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:TextBox ID="TxtFechaRemMay" runat="server" CssClass="form-control-sm heightCampo" onKeyDown="return false" TextMode="Date" Width="11%" OnTextChanged="TxtFechaRemMay_TextChanged" AutoPostBack="true" />
                    <asp:Button ID="BtnRemMayCompensac" CssClass="btn btn-danger" runat="server" Height="25px" Width="18px" Text="C" Font-Size="9px" ToolTip="Libros de vuelo para la compensación" OnClick="BtnRemMayCompensac_Click" OnClientClick="return confirm('¿Desea realizar la compensación?');" Visible="false" />
                    <asp:Label ID="LblMotivRemMay" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivRemMay" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblRemMay" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitRemMayContadores" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContRemMay" runat="server" EmptyDataText="Sin histórico..!" AutoGenerateColumns="False"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitRemoMayor" runat="server" Text="Mayores a Remover" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusqRemMay" runat="server" EmptyDataText="No existen mayores con el dato seleccionado ..!" DataKeyNames="CodElemento,CodUbicacionSuperior"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusqRemMay_SelectedIndexChanged" OnPageIndexChanging="GrdBusqRemMay_PageIndexChanging" OnRowDataBound="GrdBusqRemMay_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnGuardarRemMay" />
                    <asp:PostBackTrigger ControlID="BtnAKVirtualRemMay" />
                    <asp:PostBackTrigger ControlID="BtnVisualizarRemMay" />
                    <asp:PostBackTrigger ControlID="BtnRemMayCompensac" />
                    <asp:PostBackTrigger ControlID="BtnAbrirOTCerrarRemMay" />
                    <asp:AsyncPostBackTrigger ControlID="TxtFechaRemMay" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw8InsSubC" runat="server">
            <asp:UpdatePanel ID="UplInstSubC" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInsSubC" runat="server" Text="Instalación de un Sub-Componente" /></h6>
                    <asp:Label ID="LblPNMyInsSubC" runat="server" CssClass="LblEtiquet" Text="P/N Mayor:" />
                    <asp:DropDownList ID="DdlPNInsSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlPNInsSub_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblSNMyInsSubC" runat="server" CssClass="LblEtiquet" Text="S/N Mayor:" />
                    <asp:DropDownList ID="DdlSNInsSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlSNInsSub_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblModelInsSubC" runat="server" CssClass="LblEtiquet" Text="Modelo:" />
                    <asp:DropDownList ID="DdlModelInsSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlModelInsSub_TextChanged" AutoPostBack="true" /><br />
                    <br />
                    <asp:Label ID="LblBusInsSubC" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtBusqInsSubC" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNInsSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNInsSubC_Click" />&nbsp
                    <asp:Button ID="BtnSNInsSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNInsSubC_Click" />&nbsp
                    <asp:Button ID="BtnUltNivInsSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivInsSubC_Click" ToolTip="Ubicación Técnica" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAKVirtualInsSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualInsSubC_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp
                     <asp:Button ID="BtnVisualizarMayInsSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="Mayores" OnClick="BtnVisualizarMayInsSubC_Click" ToolTip="Visualizar mayores y los subcomponentes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarInsSubC" CssClass="btn btn-success" runat="server" Text="Instalar" OnClick="BtnGuardarInsSubC_Click" ToolTip="Realizar la instalación" OnClientClick="return confirm('¿Desea realizar la instalación?');" /><br />
                    <asp:Label ID="LblPnInsSubC" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnInsSubC" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnInsSubC" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnInsSubC" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecInsSubC" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecInsSubC" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicInsSubC" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicInsSubC" runat="server" CssClass="heightCampo" Width="10%" />
                    <asp:Label ID="LblFechaInsSubC" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:TextBox ID="TxtFechaInsSubC" runat="server" CssClass="form-control-sm heightCampo" onKeyDown="return false" TextMode="Date" Width="11%" />                
                    <asp:Label ID="LblMotivInsSubC" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivInsSubC" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblInsSubC" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitContadoresInsSubC" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContInsSubC" runat="server" EmptyDataText="Sin histórico..!" AutoGenerateColumns="False"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitServcInsSubC" runat="server" Text="Sub-Componentes Disponibles" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusqInsSubC" runat="server" EmptyDataText="No existen registros ..!" DataKeyNames="CodElemento"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusqInsSubC_SelectedIndexChanged" OnPageIndexChanging="GrdBusqInsSubC_PageIndexChanging" OnRowDataBound="GrdBusqInsSubC_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                    </asp:GridView>
                                </div>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdSvcInsSubC" runat="server" AutoGenerateColumns="False" DataKeyNames="FVAnt,CodIdContadorElem,CodIdContaSrvManto,CodElemento"
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
                                                    <asp:TextBox ID="TxtFecUltCumplInsSubC" Text='<%# Eval("FechaVencWeb") %>' runat="server" Width="100%" onKeyDown="return false" TextMode="Date" OnTextChanged="TxtFecUltCumplInsSubC_TextChanged" />                                               
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
                    <asp:AsyncPostBackTrigger ControlID="DdlPNInsSubC" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlSNInsSubC" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlModelInsSubC" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="BtnGuardarInsSubC" />
                    <asp:PostBackTrigger ControlID="BtnPNInsSubC" />
                    <asp:PostBackTrigger ControlID="BtnSNInsSubC" />
                    <asp:PostBackTrigger ControlID="BtnUltNivInsSubC" />
                    <asp:PostBackTrigger ControlID="BtnAKVirtualInsSubC" />
                    <asp:PostBackTrigger ControlID="BtnVisualizarMayInsSubC" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw9RemSubC" runat="server">
            <asp:UpdatePanel ID="UplRemtSubC" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRemSubC" runat="server" Text="Remoción de un Sub-Componente" /></h6>
                    <asp:Label ID="LblPNMyRemSubC" runat="server" CssClass="LblEtiquet" Text="P/N Mayor:" />
                    <asp:DropDownList ID="DdlPNRemSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlPNRemSubC_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblSNMyRemSubC" runat="server" CssClass="LblEtiquet" Text="S/N Mayor:" />
                    <asp:DropDownList ID="DdlSNRemSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlSNRemSubC_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblModelRemSubC" runat="server" CssClass="LblEtiquet" Text="Modelo:" />
                    <asp:DropDownList ID="DdlModelRemSubC" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlModelRemSubC_TextChanged" AutoPostBack="true" /><br />
                    <br />
                    <asp:Label ID="LblBusRemSubC" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtBusqRemSubC" runat="server" Width="15%" CssClass="form-control-sm heightCampo" placeholder="Ingrese el dato a consultar" />
                    <asp:Button ID="BtnPNRemSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="P/N" OnClick="BtnPNRemSubC_Click" />&nbsp
                    <asp:Button ID="BtnSNRemSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="S/N" OnClick="BtnSNRemSubC_Click" />&nbsp
                    <asp:Button ID="BtnUltNivRemSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="U/T" OnClick="BtnUltNivRemSubC_Click" ToolTip="Ubicación Técnica" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAKVirtualRemSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="Visualizar" OnClick="BtnAKVirtualRemSubC_Click" ToolTip="Visualizar elementos instalados y ubicaciones pendientes" />&nbsp
                    <asp:Button ID="BtnVisualizarMayRemSubC" CssClass="btn btn-primary" runat="server" Height="33px" Text="Mayores" OnClick="BtnVisualizarMayRemSubC_Click" ToolTip="Visualizar mayores y los subcomponentes" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnAbrirOTCerrarRemSubC" CssClass="btn btn-danger" runat="server" Height="33px" Text="O.T. Abiertas" OnClick="BtnAbrirOTCerrarRemSubC_Click" Visible="false" />&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnGuardarRemSubC" CssClass="btn btn-success" runat="server" Text="Remover" OnClick="BtnGuardarRemSubC_Click" ToolTip="Realizar la remoción" OnClientClick="return confirm('¿Desea realizar la remoción?');" /><br />
                    <asp:Label ID="LblPnRemSubC" runat="server" Text="P/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtPnRemSubC" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblSnRemSubC" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtSnRemSubC" runat="server" Width="12%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblUbiTecRemSubC" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" />
                    <asp:TextBox ID="TxtUbiTecRemSubC" runat="server" Width="5%" CssClass="form-control-sm heightCampo" Enabled="false" />
                    <asp:Label ID="LblPosicRemSubC" runat="server" CssClass="LblEtiquet" Text="Posicion:" />
                    <asp:DropDownList ID="DdlPosicRemSubC" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                    <asp:Label ID="LblFechaRemSubC" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                    <asp:TextBox ID="TxtFechaRemSubC" runat="server" CssClass="form-control-sm heightCampo" onKeyDown="return false" TextMode="Date" Width="11%" />
                    <asp:Label ID="LblMotivRemSubC" runat="server" CssClass="LblEtiquet" Text="Motivo:" />
                    <asp:TextBox ID="TxtMotivRemSubC" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="15%" Height="1%" />
                    <br />
                    <br />
                    <asp:Table ID="TblRemSubC" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitContadoresRemSub" runat="server" Text="Contadores" /></h6>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdHisContRemSubC" runat="server" EmptyDataText="Sin histórico..!" AutoGenerateColumns="False"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodC" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblValor" Text='<%# Eval("Valor") %>' runat="server" />
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
                            <asp:TableCell Width="80%" VerticalAlign="Top">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="TxtTitSrvcRemSubC" runat="server" Text="Sub-Componente a Remover" /></h6>
                                <div class="DivGrid DivContendorGrid">
                                    <asp:GridView ID="GrdBusqRemSubC" runat="server" EmptyDataText="No existen Sub-Componentes con el dato seleccionado ..!" DataKeyNames="CodElemento,CodUbicacionSuperior,CodAeronave"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="11"
                                        OnSelectedIndexChanged="GrdBusqRemSubC_SelectedIndexChanged" OnPageIndexChanging="GrdBusqRemSubC_PageIndexChanging" OnRowDataBound="GrdBusqRemSubC_RowDataBound">
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                    </asp:GridView>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DdlPNRemSubC" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlSNRemSubC" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DdlModelRemSubC" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="BtnPNRemSubC" />
                    <asp:PostBackTrigger ControlID="BtnSNRemSubC" />
                    <asp:PostBackTrigger ControlID="BtnUltNivRemSubC" />
                    <asp:PostBackTrigger ControlID="BtnAKVirtualRemSubC" />
                    <asp:PostBackTrigger ControlID="BtnVisualizarMayRemSubC" />
                    <asp:PostBackTrigger ControlID="BtnAbrirOTCerrarRemSubC" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw10CrearElem" runat="server">
            <asp:UpdatePanel ID="UplCrearElem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCrearElem" runat="server" Text="Creación de elementos controlados" /></h6>
                    <asp:Button ID="BtnPropiedad" CssClass="btn btn-outline-primary" runat="server" Text="Propiedad de la compañía" OnClick="BtnPropiedad_Click" Font-Size="10px" />&nbsp
                      <asp:Button ID="BtnCliente" CssClass="btn btn-outline-primary" runat="server" Text="Cliente" OnClick="BtnCliente_Click" Font-Size="10px" />&nbsp&nbsp&nbsp&nbsp
                    <asp:Button ID="BtnCrearElemGuardar" CssClass="btn btn-success" runat="server" Text="Crear" OnClick="BtnCrearElemGuardar_Click" ToolTip="Crear el elemento" OnClientClick="return confirm('¿Desea crear el nuevo elemento?');" />
                    <asp:ImageButton ID="IbtCerrarCrearElem" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCrearElem_Click" ImageAlign="Right" /><br />
                    <br />
                    <div class="TitElem">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitCrearEDatosE" runat="server" Text="Datos del elemento" /></h6>
                    </div>
                    <asp:Label ID="LblCrearElemPn" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                    <asp:DropDownList ID="DdlCrearElemPn" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlCrearElemPn_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblCrearElemSn" runat="server" Text="S/N: " CssClass="LblTextoBusq" />
                    <asp:TextBox ID="TxtCrearElemSn" runat="server" Width="12%" CssClass="form-control-sm heightCampo" />
                    <asp:Label ID="LblCrearElemFechRec" runat="server" CssClass="LblEtiquet" Text="Fecha Recibo:" />                  
                    <asp:TextBox ID="TxtCrearElemFechRec" runat="server" CssClass="form-control-sm heightCampo" TextMode="Date" Width="11%" />                  
                    <asp:Label ID="LblCrearElemFechFabr" runat="server" CssClass="LblEtiquet" Text="Fecha Fabricación:" />
                    <asp:TextBox ID="TxtCrearElemFechFabr" runat="server" CssClass="form-control-sm heightCampo" TextMode="Date" Width="11%" />
                    <br />
                    <br />
                    <asp:Table ID="TblCrearElemCont" runat="server">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="10%">
                                <div class="TitElemContad">
                                    <h6 class="TextoSuperior">
                                        <asp:Label ID="LblCrearEContadores" runat="server" Text="Valor inicial de Contadores" /></h6>
                                </div>
                                <div class="DivContendorGrid">
                                    <asp:GridView ID="GrdCrearECont" runat="server" AutoGenerateColumns="False"
                                        EmptyDataText="Sin contadores asignados..!" CssClass="DiseñoGrid table-sm" GridLines="Both" OnRowDataBound="GrdCrearECont_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contador." HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodContador" Text='<%# Eval("CodContador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCumpHist" Text='<%# Eval("Valor") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
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
                    <asp:PostBackTrigger ControlID="BtnPropiedad" />
                    <asp:PostBackTrigger ControlID="BtnCliente" />
                    <asp:PostBackTrigger ControlID="IbtCerrarCrearElem" />
                    <asp:AsyncPostBackTrigger ControlID="DdlCrearElemPn" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
