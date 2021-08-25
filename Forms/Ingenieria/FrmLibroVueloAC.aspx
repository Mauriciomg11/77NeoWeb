<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmLibroVueloAC.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmLibroVueloAC" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>LV</title>
    <style type="text/css">
    .WidthSubTit {
        width: 100%;
    }

    .heightCampo {
        height: 25px;
        width: 95%;
        font-size: 12px;
    }

    .TextMultiLine {
        height: 80px;
        width: 98%;
        font-size: 11px;
    }

    .TablaBotones {
        width: 50%;
        height: 1%;
    }

    .TablaBotonesPrincipal {
        width: 60%;
        height: 1%;
    }

    .MyCalendar .ajax__calendar_container {
        border: 1px solid #646464;
        background-color: Gray;
        color: Black;
        font-family: Arial;
        font-size: 14px;
        font-weight: bold;
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
            $('#<%=DdlBusq.ClientID%>').chosen();
            $('#<%=DdlMatri.ClientID%>').chosen();
            $('#<%=DdlBase.ClientID%>').chosen();
            $('#<%=DdlBusqRte.ClientID%>').chosen();
            $('#<%=DdlTipRte.ClientID%>').chosen();
            $('#<%=DdlFuente.ClientID%>').chosen();
            $('#<%=DdlTall.ClientID%>').chosen();
            $('#<%=DdlEstad.ClientID%>').chosen();
            $('#<%=DdlClasf.ClientID%>').chosen();
            $('#<%=DdlCatgr.ClientID%>').chosen();
            $('#<%=DdlPosRte.ClientID%>').chosen();
            $('#<%=DdlAtaRte.ClientID%>').chosen();
            $('#<%=DdlGenerado.ClientID%>').chosen();
            $('#<%=DdlLicGene.ClientID%>').chosen();
            $('#<%=DdlOtRte.ClientID%>').chosen();
            $('#<%=DdlBasRte.ClientID%>').chosen();
            $('#<%=DdlCumpl.ClientID%>').chosen();
            $('#<%=DdlLicCump.ClientID%>').chosen();
            $('#<%=DdlPnRte.ClientID%>').chosen();
            $('#<%=DdlTecDif.ClientID%>').chosen();
            $('#<%=DdlVerif.ClientID%>').chosen();
            $('#<%=DdlLicVer.ClientID%>').chosen();
            $('#<%=DdlAeroRte.ClientID%>').chosen();
            $('#<%=DdlPrioridadOT.ClientID%>').chosen();
            $('#<%=DdlHkInfLV.ClientID%>').chosen();

            $('[id *=DdlOrig], [id *=DdlPNPP], [id *=DdlOrigPP]').chosen();
            $('[id *=DdlDest], [id *=DdlPNPP], [id *=DdlDestPP]').chosen();
            $('[id *=DdlPNRFPP]').chosen();
            $('[id *=DdlLicenRFPP]').chosen();
            $('[id *=DdlRazonR]').chosen();
            $('[id *=DdlPosic]').chosen();
            $('[id *=DdlPNOn]').chosen();
            $('[id *=DdlPNOff]').chosen();
            $('[id *=DdlPNHta]').chosen();
            //$('[id *=DdlPNOff]').chosen();


        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo"></asp:Label>
    </h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlBtnPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Table ID="TblBtnPpal" runat="server">
                <asp:TableRow>
                    <asp:TableCell Width="2%">
                        <asp:Button ID="BtnDatos" CssClass="btn btn-primary" runat="server" Text="Datos Generales" OnClick="BtnDatos_Click" />
                        <asp:Button ID="BtnVuelos" CssClass="btn btn-primary" runat="server" Text="Trayectos" OnClick="BtnVuelos_Click" />
                        <asp:Button ID="BtnManto" CssClass="btn btn-primary" runat="server" Text="Mantenimiento" OnClick="BtnManto_Click" />
                    </asp:TableCell>
                    <asp:TableCell Width="1%">
                        <asp:CheckBox ID="CkbProcesado" runat="server" Text="Procesado" CssClass="LblEtiquet" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell Width="1%">
                        <asp:Label ID="LblNumLVTit" runat="server" CssClass="LblEtiquet" Font-Size="X-Large" Text="Número"></asp:Label>
                        </td>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnDatos" />
            <asp:PostBackTrigger ControlID="BtnVuelos" />
            <asp:PostBackTrigger ControlID="BtnManto" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:MultiView ID="MultVieLV" runat="server">
        <asp:View ID="Vw0LibroVuelo" runat="server">
            <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Table ID="TblCampos" runat="server" Width="100%">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="11">
                                <table class="TablaBotones">
                                    <tr>
                                        <td width="10%">
                                            <asp:DropDownList ID="DdlBusq" runat="server" CssClass="Campos" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" Width="100%" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtAdd" runat="server" CssClass="BtnImagenAdd" ImageUrl="~/images/AddNew.png" OnClick="IbtAdd_Click" ToolTip="Ingresar" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtUpdate" runat="server" CssClass="BtnImagenUpdate" ImageUrl="~/images/Edit.png" OnClick="IbtUpdate_Click" ToolTip="Modificar" AutoPostBack="false" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtFind" runat="server" CssClass="BtnImagenFind" ImageUrl="~/images/FindV1.png" OnClick="IbtFind_Click" ToolTip="Otras consultas" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtPrint" runat="server" CssClass="BtnImagenPrint" ImageUrl="~/images/PrintV1.png" OnClick="IbtPrint_Click" ToolTip="Imprimir" Visible="false" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtDelete" runat="server" CssClass="BtnImagenDelete" ImageUrl="~/images/deleteV1.png" OnClick="IbtDelete_Click" ToolTip="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" Visible="false" />
                                        </td>
                                        <td width="2%">
                                            <asp:ImageButton ID="IbtAuxiliar" runat="server" CssClass="BtnImagenAuxiliar" ImageUrl="~/images/ReportsV2.png" OnClick="IbtAuxiliar_Click" ToolTip="Informes" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LkbDescargarLV" runat="server" OnClick="LkbDescargarLV_Click" Font-Size="10px" />
                                        </td>
                                        <td></div>                                            
                                            <asp:Button ID="cargarLV" runat="server" Text="Cargar LV" CssClass=" btn btn-success botones BtnEdicion" OnClick="cargarLV_Click" Height="25px" Font-Size="10px" Enabled="false" />
                                            <asp:FileUpload ID="FileUpCLV" runat="server" Font-Size="9px" Enabled="false" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="11">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblDatosGrales" runat="server" Text="Datos Generales" />
                                </h6>
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="10">
                                <br />
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblFecha" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:ImageButton ID="IbtFecha" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" Enabled="false" />
                            </asp:TableCell>
                            <asp:TableCell Width="5%">
                                <asp:TextBox ID="TxtFecha" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" OnTextChanged="TxtFecha_TextChanged" AutoPostBack="true" />
                                <ajaxToolkit:CalendarExtender ID="CalFecha" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtFecha" TargetControlID="TxtFecha" Format="dd/MM/yyyy" />
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblNumLV" runat="server" CssClass="LblEtiquet" Text="Número:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="5%">
                                <asp:TextBox ID="TxtNumLv" runat="server" CssClass="form-control heightCampo" MaxLength="10" Enabled="false" Width="90%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LbLMatri" runat="server" CssClass="LblEtiquet" Text="Matrícula:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="5%">
                                <asp:DropDownList ID="DdlMatri" runat="server" CssClass="heightCampo" Enabled="false" OnTextChanged="DdlMatri_TextChanged" AutoPostBack="true" Width="90%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblBase" runat="server" CssClass="LblEtiquet" Text="Base:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="15%">
                                <asp:DropDownList ID="DdlBase" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observación:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell RowSpan="2" Width="10%">
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="10">
                                <asp:Table ID="Table1" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="7%">
                                            <asp:Label ID="LblHrAPU" runat="server" CssClass="LblEtiquet" Text="Horas APU:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="10%">
                                            <asp:TextBox ID="TxtHrAPU" runat="server" CssClass="form-control heightCampo" Enabled="false" Text="00:00" MaxLength="5" Width="90%"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MEEHrApu" runat="server" AcceptAMPM="true" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true"
                                                InputDirection="RightToLeft" CultureName="es-ES" TargetControlID="TxtHrAPU" MessageValidatorTip="true" />
                                            <ajaxToolkit:MaskedEditValidator ID="MEVHrApu" Font-Size="12px" runat="server"
                                                ControlExtender="MEEHrApu"
                                                ControlToValidate="TxtHrAPU"
                                                TooltipMessage="00:00 - 23:59"
                                                InvalidValueMessage="00:00 - 23:59" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblNumVuelo" runat="server" CssClass="LblEtiquet" Text="Vuelos:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtNumVuelo" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="5%">
                                            <asp:Label ID="LblLevante" runat="server" CssClass="LblEtiquet" Text="Levantes:" Visible="false"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtLevante" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%" Visible="false"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="10%">
                                            <asp:Label ID="LblAterrCorr" runat="server" CssClass="LblEtiquet" Text="Aterrizaje Corrido:" Visible="false"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtAterrCorr" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%" Visible="false"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="12%">
                                            <asp:Label ID="LblEveAutoR" runat="server" CssClass="LblEtiquet" Text="Evento autorrotación:" Visible="false"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtEveAutoR" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%" Visible="false"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="10%">
                                            <asp:Label ID="LblEveSimul" runat="server" CssClass="LblEtiquet" Text="Evento simulación:" Visible="false"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtEveSimul" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%" Visible="false"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <br />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="10">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDaMtr" runat="server" Text="Datos Motor" />
                                </h6>
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="10">
                                <br />
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="10">
                                <asp:Table ID="DatosMotor" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblAlt" runat="server" CssClass="LblEtiquet" Text="ALT:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtAlt" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblKias" runat="server" CssClass="LblEtiquet" Text="KIAS:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtKias" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblOat" runat="server" CssClass="LblEtiquet" Text="OAT:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtOat" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblGW" runat="server" CssClass="LblEtiquet" Text="G.W.:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtGW" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblTat" runat="server" CssClass="LblEtiquet" Text="TAT:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtTat" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell Width="4%">
                                            <asp:Label ID="LblMach" runat="server" CssClass="LblEtiquet" Text="MACH:"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell Width="8%">
                                            <asp:TextBox ID="TxtMach" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="70%"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ID="DetMotor" ColumnSpan="10">
                                <asp:GridView ID="GrdMotor" runat="server" EmptyDataText="Sin motores ..!" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="CodIDDLVM,CodElemMotorLV"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="6" Width="100%"
                                    OnRowDataBound="GrdMotor_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPosP" Text='<%# Eval("PosicionMotor") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblPos" Text='<%# Eval("PosicionMotor") %>' runat="server" Enabled="false" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSNP" Text='<%# Eval("SN") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtStartP" Text='<%# Eval("Start") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TQ" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtTQP" runat="server" Width="100%" Text='<%# Eval("Torque") %>' TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ITT" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtITTP" runat="server" Width="100%" Text='<%# Eval("ITT") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NP (NI)" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtNIP" runat="server" Width="100%" Text='<%# Eval("NI") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NG (NII)" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtNIIP" runat="server" Width="100%" Text='<%# Eval("NII") %>' TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pres. Aceite" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtPresAcP" runat="server" Width="100%" Text='<%# Eval("PresionAceite") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Temp. Aceite" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtTempAcP" runat="server" Width="100%" Text='<%# Eval("TempAceite") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pres. Comb" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtPresCombP" runat="server" Width="100%" Text='<%# Eval("PresComb") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pres. HYD" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtPresHYDP" runat="server" Width="100%" Text='<%# Eval("PresHyd") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel Comb" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtNivCombP" runat="server" Width="100%" Text='<%# Eval("CombVuelo") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OEI" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtOEIP" runat="server" Width="100%" Text='<%# Eval("OEI") %>' TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GAS GEN Cycles (C1)" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtC1P" runat="server" Width="100%" Text='<%# Eval("GasGenCycle") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PWR Turbine Cycles (C2)" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtC2P" runat="server" Width="100%" Text='<%# Eval("PwrTurbineCycle") %>' TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                </asp:GridView>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TxtFecha" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="IbtFind" />
                    <asp:PostBackTrigger ControlID="IbtAuxiliar" />
                    <asp:PostBackTrigger ControlID="cargarLV" />
                    <asp:PostBackTrigger ControlID="LkbDescargarLV" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1Trayectos" runat="server">
            <asp:UpdatePanel ID="UpPnlTray" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTrayectos" runat="server" Text="Trayectos" />
                    </h6>
                    <br />
                    <asp:GridView ID="GrdTray" runat="server" EmptyDataText="Sin trayectos ..!" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdDetLibroVuelo"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="12"
                        OnRowCommand="GrdTray_RowCommand" OnRowEditing="GrdTray_RowEditing"
                        OnRowUpdating="GrdTray_RowUpdating" OnRowCancelingEdit="GrdTray_RowCancelingEdit" OnRowDeleting="GrdTray_RowDeleting"
                        OnRowDataBound="GrdTray_RowDataBound" OnPageIndexChanging="GrdTray_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Origen" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblOrigP" Text='<%# Eval("Origen") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlOrig" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlOrigPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Destino" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblDestP" Text='<%# Eval("Destino") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlDest" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlDestPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Salida" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFecSal" Text='<%# Eval("FechaDespegue") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFecSal" Text='<%# Eval("FechaDespegue") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaS" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechS" runat="server" PopupButtonID="IbtFechaS" TargetControlID="TxtFecSal" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFecSalPP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaSPP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechSPP" runat="server" PopupButtonID="IbtFechaSPP" TargetControlID="TxtFecSalPP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hora:Min" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblHMS" Text='<%# Eval("HD") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtHMS" Text='<%# Eval("HD") %>' runat="server" Width="100%" MaxLength="5" OnTextChanged="TxtHMS_TextChanged" AutoPostBack="true" />
                                    <ajaxToolkit:MaskedEditExtender ID="MEEHMS" runat="server" AcceptAMPM="true" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true"
                                        InputDirection="RightToLeft" CultureName="es-ES" TargetControlID="TxtHMS" MessageValidatorTip="true" />
                                    <ajaxToolkit:MaskedEditValidator ID="MEVHMS"
                                        runat="server"
                                        ToolTip="ERROR FORMATO HORA"
                                        ErrorMessage="*"
                                        ControlExtender="MEEHMS"
                                        ControlToValidate="TxtHMS"
                                        InvalidValueMessage="REGISTRE HORA"
                                        TooltipMessage="Hora 00:00 hasta 23:59" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtHMSPP" runat="server" Width="100%" MaxLength="5" OnTextChanged="TxtHMSPP_TextChanged" AutoPostBack="true" />
                                    <ajaxToolkit:MaskedEditExtender ID="MEEHMSPP" runat="server" AcceptAMPM="true" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true"
                                        InputDirection="RightToLeft" CultureName="es-ES" TargetControlID="TxtHMSPP" MessageValidatorTip="true" />
                                    <ajaxToolkit:MaskedEditValidator ID="MEVHMSPP"
                                        runat="server"
                                        ToolTip="ERROR FORMATO HORA"
                                        ErrorMessage="*"
                                        ControlExtender="MEEHMSPP"
                                        ControlToValidate="TxtHMSPP"
                                        InvalidValueMessage="REGISTRE HORA"
                                        TooltipMessage="Hora 00:00 hasta 23:59" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Llegada" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFecLle" Text='<%# Eval("FechaAterrizaje") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFecLle" Text='<%# Eval("FechaAterrizaje") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaL" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechL" runat="server" PopupButtonID="IbtFechaL" TargetControlID="TxtFecLle" Format="dd/MM/yyyy" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFecLlePP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaLPP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechLPP" runat="server" PopupButtonID="IbtFechaLPP" TargetControlID="TxtFecLlePP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hora:Min" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblHML" Text='<%# Eval("HL") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtHML" Text='<%# Eval("HL") %>' runat="server" Width="100%" MaxLength="5" OnTextChanged="TxtHML_TextChanged" AutoPostBack="true" />
                                    <ajaxToolkit:MaskedEditExtender ID="MEEHML" runat="server" AcceptAMPM="true" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true"
                                        InputDirection="RightToLeft" CultureName="es-ES" TargetControlID="TxtHML" MessageValidatorTip="true" />
                                    <ajaxToolkit:MaskedEditValidator ID="MEVHML"
                                        runat="server"
                                        ToolTip="ERROR FORMATO HORA"
                                        ErrorMessage="*"
                                        ControlExtender="MEEHML"
                                        ControlToValidate="TxtHML"
                                        InvalidValueMessage="REGISTRE HORA"
                                        TooltipMessage="Hora 00:00 hasta 23:59" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtHMLPP" runat="server" Width="100%" MaxLength="5" OnTextChanged="TxtHMLPP_TextChanged" AutoPostBack="true" />
                                    <ajaxToolkit:MaskedEditExtender ID="MEEHMLPP" runat="server" AcceptAMPM="true" MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true"
                                        InputDirection="RightToLeft" CultureName="es-ES" TargetControlID="TxtHMLPP" MessageValidatorTip="true" />
                                    <ajaxToolkit:MaskedEditValidator ID="MEVHMLPP"
                                        runat="server"
                                        ToolTip="ERROR FORMATO HORA"
                                        ErrorMessage="*"
                                        ControlExtender="MEEHMLPP"
                                        ControlToValidate="TxtHMLPP"
                                        InvalidValueMessage="REGISTRE HORA"
                                        TooltipMessage="Hora 00:00 hasta 23:59" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tiempo vuelo" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblTimeV" Text='<%# Eval("TiempoT") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtTimeV" Text='<%# Eval("TiempoT") %>' runat="server" Width="100%" MaxLength="5" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtTimeVPP" runat="server" Width="100%" MaxLength="5" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Num. Pas." HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblNumPas" Text='<%# Eval("NumPersTransp") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtNumPas" Text='<%# Eval("NumPersTransp") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtNumPasPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2Manto" runat="server">
            <asp:UpdatePanel ID="UpPnlRte" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="DdlBusqRte" runat="server" CssClass="Campos" OnTextChanged="DdlBusqRte_TextChanged" AutoPostBack="true" Width="20%"/>
                    <asp:Label ID="LblAeroRte" runat="server" CssClass="LblEtiquet" Text="Aeronave:"/>
                    <asp:DropDownList ID="DdlAeroRte" runat="server" CssClass="Campos" OnTextChanged="DdlAeroRte_TextChanged" AutoPostBack="true" Width="15%" Enabled="false"/>
                    <asp:Label ID="LblOtSec" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:"/>
                    <asp:TextBox ID="TxtOtSec" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />&nbsp&nbsp&nbsp
                    <asp:Label ID="LblNotif" runat="server" CssClass="LblEtiquet" Text="Notif:" Visible="false" />
                     <asp:CheckBox ID="CkbNotif" runat="server" CssClass="LblEtiquet" Font-Size="12px" Text="" Enabled="false" Visible="false" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRteManto" runat="server" Text="Reportes de mantenimiento" />
                    </h6>
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblNroRte" runat="server" CssClass="LblEtiquet" Text="Número:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:TextBox ID="TxtNroRte" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="100%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="4%">
                                <asp:TextBox ID="TxtConsTall" runat="server" CssClass="form-control heightCampo" MaxLength="15" Enabled="false" Width="90%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblTipRte" runat="server" CssClass="LblEtiquet" Text="Tipo Reporte:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="5%">
                                <asp:DropDownList ID="DdlTipRte" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"/>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblFuente" runat="server" CssClass="LblEtiquet" Text="Fuente:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="4%">
                                <asp:DropDownList ID="DdlFuente" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"/>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblCasi" runat="server" CssClass="LblEtiquet" Text="Casilla:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:TextBox ID="TxtCas" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="100%" Font-Size="10px"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblTall" runat="server" CssClass="LblEtiquet" Text="Taller:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="7%">
                                <asp:DropDownList ID="DdlTall" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblEstad" runat="server" CssClass="LblEtiquet" Text="Estado:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="3" Width="6%">
                                <asp:DropDownList ID="DdlEstad" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlEstad_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                           
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblClasf" runat="server" CssClass="LblEtiquet" Text="Clasificación:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlClasf" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlClasf_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblCatgr" runat="server" CssClass="LblEtiquet" Text="Categoria:" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DdlCatgr" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlCatgr_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblDocRef" runat="server" CssClass="LblEtiquet" Text="Docum. Referenc.:" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtDocRef" runat="server" CssClass="form-control heightCampo" MaxLength="20" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblPosRte" runat="server" CssClass="LblEtiquet" Text="Posición:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlPosRte" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="5">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Ata:" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="99%">
                                            <asp:DropDownList ID="DdlAtaRte" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="Generado" runat="server" CssClass="LblEtiquet" Text="Generado:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlGenerado" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblLicGene" runat="server" CssClass="LblEtiquet" Text="Licencia:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DdlLicGene" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="4">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblFecDet" runat="server" CssClass="LblEtiquet" Text="Fecha:" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="IbtFecDet" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecDet" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="11px" OnTextChanged="TxtFecDet_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecDet" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecDet" TargetControlID="TxtFecDet" Format="dd/MM/yyyy" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LblFecProy" runat="server" CssClass="LblEtiquet" Text="Proyec.:" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="IbtFecPry" runat="server" CssClass="BtnImagenCalender DiseñoCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFecPry" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="10.5px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecPry" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecPry" TargetControlID="TxtFecPry" Format="dd/MM/yyyy" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Label ID="LblOtRte" runat="server" CssClass="LblEtiquet" Text="OT Ppal:" />
                                <asp:DropDownList ID="DdlOtRte" runat="server" CssClass="heightCampo" Enabled="false" Width="66%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblBasRte" runat="server" CssClass="LblEtiquet" Text="Base:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="3">
                                <asp:DropDownList ID="DdlBasRte" runat="server" CssClass="heightCampo" Enabled="false" Width="80%"/>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblCumpl" runat="server" CssClass="LblEtiquet" Text="Cumplido:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlCumpl" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblLicCump" runat="server" CssClass="LblEtiquet" Text="Licencia:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DdlLicCump" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFecCump" runat="server" CssClass="LblEtiquet" Text="Fecha Cumplim.:" />
                            </asp:TableCell>
                            <asp:TableCell ID="TbClFecCump">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:ImageButton ID="IbtFecCump" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox ID="TxtFecCump" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="11px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecCump" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecCump" TargetControlID="TxtFecCump" Format="dd/MM/yyyy" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="6">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblProgr" runat="server" CssClass="LblEtiquet" Text="Programado:" />
                                            &nbsp
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblPgSi" runat="server" CssClass="LblEtiquet" Text="Sí" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbPgSi" runat="server" TextAlign="Left" GroupName="Prog" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="Label3" runat="server" CssClass="LblEtiquet" Text="No" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbPgNo" runat="server" TextAlign="Left" GroupName="Prog" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            &nbsp;&nbsp;&nbsp;&nbsp<asp:Label ID="LblFallC" runat="server" CssClass="LblEtiquet" Text="Falla Confirmada:" />&nbsp
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblSi" runat="server" CssClass="LblEtiquet" Text="Sí" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbFlCSi" runat="server" TextAlign="Left" GroupName="FallaC" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblNo" runat="server" CssClass="LblEtiquet" Text="No" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbFlCNo" runat="server" TextAlign="Left" GroupName="FallaC" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            &nbsp;&nbsp<asp:Label ID="LblRII" runat="server" CssClass="LblEtiquet" Text="R.I.I.:" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbRII" runat="server" CssClass="LblEtiquet" Text="" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblPnRte" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlPnRte" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblSnRte" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtSnRte" runat="server" CssClass="form-control heightCampo" MaxLength="20" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblTtlAKSN" runat="server" CssClass="LblEtiquet" Text="TT AK/Comp:" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtTtlAKSN" runat="server" CssClass="form-control heightCampo" Width="85%" step="0.01" TextMode="Number" onkeypress="return Decimal(event);" Enabled="false" Visible="false" OnTextChanged="TxtTtlAKSN_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblHPrxCu" runat="server" CssClass="LblEtiquet" Text="H. Prox. Cumpl.:" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:TextBox ID="TxtHPrxCu" runat="server" CssClass="form-control heightCampo" Width="85%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" OnTextChanged="TxtHPrxCu_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Label ID="LblNexDue" runat="server" CssClass="LblEtiquet" Text="Next Due:" Visible="false" />
                                <asp:TextBox ID="TxtNexDue" runat="server" CssClass="Form-control-sm heightCampo" Width="55%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblDescRte" runat="server" CssClass="LblEtiquet" Text="Descripción Reporte:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="5">
                                <asp:TextBox ID="TxtDescRte" runat="server" CssClass=" form-control-sm TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="1000" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblAccCorr" runat="server" CssClass="LblEtiquet" Text="Acción Correctiva:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="6">
                                <asp:TextBox ID="txtAccCrr" runat="server" CssClass="form-control-sm TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="1000" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="AcciParc" runat="server" CssClass="LblEtiquet" Text="Acción parcial:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="4">
                                <asp:TextBox ID="TxtAcciParc" runat="server" CssClass="form-control TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="254" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblTecDif" runat="server" CssClass="LblEtiquet" Text="Técnico Difiere:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlTecDif" runat="server" CssClass="heightCampo" Enabled="false" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDatosVer" runat="server" Text="Datos de verificación" />
                                </h6>
                                <asp:Table runat="server" Width="100%">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="LblVerif" runat="server" CssClass="LblEtiquet" Text="Verifica:" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="53%">
                                            <asp:DropDownList ID="DdlVerif" runat="server" CssClass="heightCampo" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="30%">
                                            <asp:DropDownList ID="DdlLicVer" runat="server" CssClass="heightCampo" Enabled="false" Font-Size="10px" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="17%">
                                            <asp:CheckBox ID="CkbTearDown" runat="server" CssClass="LblEtiquet" Text="Teardown" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="8">
                                <asp:Table ID="Botnes" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnReserva" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnReserva_Click" Text="Reserva" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnConsultar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnConsultar_Click" Text="Consultar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnImprimir" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnImprimir_Click" Text="Imprimir" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnEliminar_Click" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnSnOnOf" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnSnOnOf_Click" Text="S/N On/Off" ToolTip="Series removidas - instaladas / Herramientas" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnExporRte" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnExporRte_Click" Text="Exportar" ToolTip="Exportar a Excel todos los reportes" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnNotificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnNotificar_Click" Text="Notificar" ToolTip="Notificar el reporte" OnClientClick="return confirm('¿Desea notificar el reporte?');" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnReserva" />
                    <asp:PostBackTrigger ControlID="BtnConsultar" />
                    <asp:PostBackTrigger ControlID="BtnImprimir" />
                    <asp:PostBackTrigger ControlID="BtnSnOnOf" />
                    <asp:PostBackTrigger ControlID="BtnExporRte" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3Busq" runat="server">
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTitOpcBusqueda" runat="server" Text="Opciones de búsqueda " />
            </h6>
            <asp:Table ID="TblBusqRte" runat="server" class="TablaBusqueda" Visible="false" Width="65%">
                <asp:TableRow>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteNum" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteHk" runat="server" CssClass="LblEtiquet" Text="&nbsp Aeronave" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteAta" runat="server" CssClass="LblEtiquet" Text="&nbsp ATA" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="4%">
                        <asp:RadioButton ID="RdbBusqRteOT" runat="server" CssClass="LblEtiquet" Text="&nbsp O.T. Principal" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteTecn" runat="server" CssClass="LblEtiquet" Text="&nbsp Técnico" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="8%">
                        <asp:RadioButton ID="RdbBusqRteDescRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Descripción del reporte" GroupName="BusqRte" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="TblBusqLVlo" runat="server" class="TablaBusqueda" Visible="false" Width="45%">
                <asp:TableRow>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqLVloNum" runat="server" CssClass="LblEtiquet" Text="&nbsp Número" GroupName="BusqVlo" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqLVloFech" runat="server" CssClass="LblEtiquet" Text="&nbsp Fecha" GroupName="BusqVlo" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqLVloHK" runat="server" CssClass="LblEtiquet" Text="&nbsp Matrícula" GroupName="BusqVlo" />
                    </asp:TableCell>
                    <asp:TableCell Width="4%">
                        <asp:RadioButton ID="RdbBusqLVloNroRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte nro" GroupName="BusqVlo" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <table class="TablaBusqueda">
                <tr>
                    <td>
                        <asp:Label ID="LblOpcBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" />
                    </td>
                    <td>
                        <asp:ImageButton ID="IbtConsultarBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultarBusq_Click" />
                    </td>
                    <td>
                        <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    </td>
                    <td>
                        <asp:ImageButton ID="IbtExpConsulRte" runat="server" ToolTip="Exportar Resultado" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpConsulRte_Click" />
                    </td>
                </tr>
            </table>
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!"
                    CssClass="GridControl DiseñoGrid table" GridLines="Both" AllowPaging="true" PageSize="7"
                    OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging">
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
        </asp:View>
        <asp:View ID="Vw4RecursoRte" runat="server">
            <asp:UpdatePanel ID="UpPnlRecursoRte" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblRecFRte" runat="server" CssClass="LblEtiquet" Text="Reporte:" />
                    <asp:TextBox ID="TxtRecurNumRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblRecFSubOt" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:" />
                    <asp:TextBox ID="TxtRecurSubOt" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblPrioridadOT" runat="server" CssClass="LblEtiquet" Text="Prioridad:" />
                    <asp:DropDownList ID="DdlPrioridadOT" runat="server" CssClass="Campos" Width="15%" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTtlRecursoRte" runat="server" Text="Recurso Físico y Licencias" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarRec" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarRec_Click" ImageAlign="Right" />
                    <table class="TablaBusqueda">
                        <tr>
                            <td>
                                <asp:Label ID="LblOtRecurBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtConsulPnRecurRte" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="IbtConsulPnRecurRte" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsulPnRecurRte_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="IbtExpExcelPnRecurRte" runat="server" ToolTip="Exportar reserva" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcelPnRecurRte_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="BtnCargaMaxiva" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnCargaMaxiva_Click" Text="Carga masiva" Width="10%" />
                    <asp:Table ID="TblRecFis" runat="server" Width="98%">
                        <asp:TableRow>
                            <asp:TableCell Width="63%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitRecursFis" runat="server" Text="Reserva"></asp:Label>
                                </h6>
                            </asp:TableCell>
                            <asp:TableCell Width="2%" VerticalAlign="Top">
                            </asp:TableCell>
                            <asp:TableCell Width="35%">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitLicencia" runat="server" Text="Licencias" />
                                </h6>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:GridView ID="GrdRecursoF" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodiddetalleRes"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="6"
                                    OnRowCommand="GrdRecursoF_RowCommand" OnRowEditing="GrdRecursoF_RowEditing" OnRowUpdating="GrdRecursoF_RowUpdating" OnRowCancelingEdit="GrdRecursoF_RowCancelingEdit"
                                    OnRowDeleting="GrdRecursoF_RowDeleting" OnRowDataBound="GrdRecursoF_RowDataBound" OnPageIndexChanging="GrdRecursoF_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPosc" Text='<%# Eval("NumeroPosicion") %>' runat="server" Width="100%" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtPosRF" Text='<%# Eval("NumeroPosicion") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtPosRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlPNRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlPNRFPP_TextChanged" />
                                                <asp:TextBox ID="TxtPNRFPP" runat="server" MaxLength="80" Width="100%" Enabled="false" Visible="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtDesRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCantRF" Text='<%# Eval("CantidadSolicitada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtCantRF" Text='<%# Eval("CantidadSolicitada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtCantRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CodUnidadMed") %>' runat="server" Width="100%" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUMRF" Text='<%# Eval("CodUnidadMed") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cant. Entreg." HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCantEntrRF" Text='<%# Eval("CantidadEntregada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IPC - FIG - ITEM" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("IPC") %>' runat="server" Width="100%" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtIPCRF" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtIPCRFPP" runat="server" MaxLength="240" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />

                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                </asp:GridView>
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Top">
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Top">
                                <asp:GridView ID="GrdLicen" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSrvLic,CodIdLicencia"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="6"
                                    OnRowCommand="GrdLicen_RowCommand" OnRowEditing="GrdLicen_RowEditing" OnRowUpdating="GrdLicen_RowUpdating" OnRowCancelingEdit="GrdLicen_RowCancelingEdit"
                                    OnRowDeleting="GrdLicen_RowDeleting" OnRowDataBound="GrdLicen_RowDataBound" OnPageIndexChanging="GrdLicen_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="18%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlLicenRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlLicenRFPP_TextChanged"></asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDesLiRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtDesLiRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tiempo Estimado" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtTieEstRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                </asp:GridView>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarRec" />
                    <asp:PostBackTrigger ControlID="IbtExpExcelPnRecurRte" />
                    <asp:PostBackTrigger ControlID="BtnCargaMaxiva" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw5CargaMasiva" runat="server">
            <asp:UpdatePanel ID="UpPnlCargaMasiva" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblCargaMasRte" runat="server" CssClass="LblEtiquet" Text="Reporte:" />
                    <asp:TextBox ID="TxtCargaMasiRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblCargaMasOt" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:" />
                    <asp:TextBox ID="TxtCargaMasiOT" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:ImageButton ID="IbtCerrarSubMaxivo" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSubMaxivo_Click" ImageAlign="Right" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTCargMasiv" runat="server" Text="Subir Evaluación" />
                    </h6>
                    <asp:ImageButton ID="IbtSubirCargaMax" runat="server" ToolTip="Cargar archivo..." ImageUrl="~/images/SubirCarga.png" OnClick="IbtSubirCargaMax_Click" Width="30px" Height="30px" />
                    <asp:ImageButton ID="IbtGuardarCargaMax" runat="server" ToolTip="Guardar" ImageUrl="~/images/Descargar.png" OnClick="IbtGuardarCargaMax_Click" Width="30px" Height="30px" Enabled="false" OnClientClick="javascript:return confirm('¿Desea almacenar la información?', 'Mensaje de sistema')" />
                    <asp:GridView ID="GrdCargaMax" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                        <Columns>
                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPosRF" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMRF" Text='<%# Eval("UndDespacho") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Sistema" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMSysRF" Text='<%# Eval("UndSistema") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IPC - FIG - ITEM" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtIPCRF" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarSubMaxivo" />
                    <%--<asp:PostBackTrigger ControlID="IbtSubirCargaMax" />--%>
                    <asp:PostBackTrigger ControlID="IbtGuardarCargaMax" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw6Informe" runat="server">
            <asp:UpdatePanel ID="UpPnlInforme" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión del reporte"></asp:Label>
                    </h6>
                    <asp:ImageButton ID="IbtCerrarImpresion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpresion_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RvwReporte" runat="server" Width="98%"></rsweb:ReportViewer>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarImpresion" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw7SNOnOff" runat="server">
            <asp:UpdatePanel ID="UplSnOnOff" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblSnONOfNumRte" runat="server" CssClass="LblEtiquet" Text="Reporte:"></asp:Label>
                    <asp:TextBox ID="TxtSnOnOffNumRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:ImageButton ID="IbtCerrarSnOnOff" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSnOnOff_Click" ImageAlign="Right" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LlTitSnOnOff" runat="server" Text="Ingreseso de elementos On - Off"></asp:Label>
                    </h6>
                    <asp:GridView ID="GrdSnOnOff" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdDetLvDetManto"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="4"
                        OnRowCommand="GrdSnOnOff_RowCommand" OnRowEditing="GrdSnOnOff_RowEditing" OnRowUpdating="GrdSnOnOff_RowUpdating" OnRowCancelingEdit="GrdSnOnOff_RowCancelingEdit"
                        OnRowDeleting="GrdSnOnOff_RowDeleting" OnRowDataBound="GrdSnOnOff_RowDataBound" OnPageIndexChanging="GrdSnOnOff_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFec" Text='<%# Eval("FechaRemocion") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFec" Text='<%# Eval("FechaRemocion") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFecha" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFech" runat="server" PopupButtonID="IbtFecha" TargetControlID="TxtFec" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFecPP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaPP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechPP" runat="server" PopupButtonID="IbtFechaPP" TargetControlID="TxtFecPP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Razón del evento" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label ID="LblRazonR" Text='<%# Eval("Descripcion") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlRazonR" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlRazonRPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posición" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPosic" Text='<%# Eval("Posicion") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPosic" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPosicPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N ON" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNOn" Text='<%# Eval("CodPnOn") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNOn" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOn_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOn" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOn_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNOnPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOnPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOnPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOnPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N ON" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodElementoOn") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNOn" Text='<%# Eval("CodElementoOn") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNOnPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DesElemento") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDescElem" Text='<%# Eval("DesElemento") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDescElemPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N OFF" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNOff" Text='<%# Eval("CodPnOff") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNOff" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOff_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOff" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOff_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNOffPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOffPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOffPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOffPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N OFF" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodElementoOff") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNOff" Text='<%# Eval("CodElementoOff") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNOffPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCant" Text='<%# Eval("CantDDLV") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCant" Text='<%# Eval("CantDDLV") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCantPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitHta" runat="server" Text="Herramientas"></asp:Label>
                    </h6>
                    <asp:GridView ID="GrdHta" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdHerramientoManto"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="3" Width="80%"
                        OnRowCommand="GrdHta_RowCommand" OnRowEditing="GrdHta_RowEditing" OnRowUpdating="GrdHta_RowUpdating" OnRowCancelingEdit="GrdHta_RowCancelingEdit"
                        OnRowDeleting="GrdHta_RowDeleting" OnRowDataBound="GrdHta_RowDataBound" OnPageIndexChanging="GrdHta_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNHta" Text='<%# Eval("PN") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNHta" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNHta_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNHta" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNHta_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNHtaPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNHtaPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNHtaPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNHtaPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNHta" Text='<%# Eval("SN") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNHtaPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDescHta" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDescHtaPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFecVce" Text='<%# Eval("FechaVence") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFecVce" Text='<%# Eval("FechaVence") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaVce" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechVce" runat="server" PopupButtonID="IbtFechaVce" TargetControlID="TxtFecVce" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFechVcePP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechVcePP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechVcePP" runat="server" PopupButtonID="IbtFechVcePP" TargetControlID="TxtFechVcePP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarSnOnOff" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw8InformesLV" runat="server">
            <asp:UpdatePanel ID="UplInfLV" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInfLV" runat="server" Text="Informes" />
                    </h6>
                    <asp:Label ID="LblAKInfLV" runat="server" CssClass="LblEtiquet" Text="Aeronave:"></asp:Label>
                    <asp:DropDownList ID="DdlHkInfLV" runat="server" CssClass="Campos" Width="8%"></asp:DropDownList>
                    <asp:Label ID="LblFechaIInfLV" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial:" />
                    <asp:ImageButton ID="IbtFIInfLV" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" />
                    <asp:TextBox ID="TxtFIInfLV" runat="server" CssClass="form-Form-control-sm heightCampo" Enabled="false" Width="5%" Font-Size="11px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CleFIInfLV" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFIInfLV" TargetControlID="TxtFIInfLV" Format="dd/MM/yyyy" />
                    <asp:Label ID="LblFechaFInfLV" runat="server" CssClass="LblEtiquet" Text="Fecha Final:" />
                    <asp:ImageButton ID="IbtFFInfLV" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" />
                    <asp:TextBox ID="TxtFFInfLV" runat="server" CssClass="Form-control-sm heightCampo" Enabled="false" Width="5%" Font-Size="11px"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CleFFInfLV" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFFInfLV" TargetControlID="TxtFFInfLV" Format="dd/MM/yyyy" />
                    <asp:ImageButton ID="IbtCerrarInfLV" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarInfLV_Click" ImageAlign="Right" />
                    <asp:Button ID="BtnInfLibroVuelos" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnInfLibroVuelos_Click" Text="Libros de Vuelo" Width="10%" />
                    <asp:Button ID="BtnInfDetLV" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnInfDetLV_Click" Text="Trayectos" Width="10%" />
                    <rsweb:ReportViewer ID="RvwInfLV" runat="server" Width="98%"></rsweb:ReportViewer>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarInfLV" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>

</asp:Content>
