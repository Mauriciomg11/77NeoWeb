<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAeronaveVirtualNew.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmAeronaveVirtualNew" %>

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
                        <asp:Button ID="BtnInsElem" CssClass="btn btn-primary" runat="server" Text="Instalar Elemento" OnClick="BtnInsElem_Click" />&nbsp
                        <asp:Button ID="BtnRemElem" CssClass="btn btn-primary" runat="server" Text="Remover Elemento" OnClick="BtnRemElem_Click" />&nbsp
                        <asp:Button ID="BtnInsMayor" CssClass="btn btn-primary" runat="server" Text="Instalar Mayor" OnClick="BtnInsMayor_Click" />&nbsp
                        <asp:Button ID="BtnRemMayor" CssClass="btn btn-primary" runat="server" Text="Remover Mayor" OnClick="BtnRemElem_Click" />&nbsp
                        <asp:Button ID="BtnInsSubC" CssClass="btn btn-primary" runat="server" Text="Instalar Sub-componente" OnClick="BtnInsSubC_Click" />&nbsp
                        <asp:Button ID="BtnRemSubC" CssClass="btn btn-primary" runat="server" Text="Remover Sub-componente" OnClick="BtnRemSubC_Click" />&nbsp
                        <asp:Button ID="BtnCrearElem" runat="server" CssClass="btn btn-success" Text="Nuevo Elemento" OnClick="BtnCrearElem_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="BtnDatos" />
            <asp:PostBackTrigger ControlID="BtnVuelos" />
            <asp:PostBackTrigger ControlID="BtnManto" />--%>
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
                                    <asp:Label ID="TxtTitServicios" runat="server" Text="Elementos disponibles" /></h6>
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
                    <div class="DivContendorGrid">
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
                       <asp:ImageButton ID="IbtCerrarCompensacion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAeroVirtual_Click" ImageAlign="Right" />
                  </ContentTemplate>
              </asp:UpdatePanel>
         </asp:View>
    </asp:MultiView>
</asp:Content>
