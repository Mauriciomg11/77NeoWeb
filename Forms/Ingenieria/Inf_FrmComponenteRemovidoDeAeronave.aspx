<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Inf_FrmComponenteRemovidoDeAeronave.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.Inf_FrmComponenteRemovidoDeAeronave" %>

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

        .GriHisContadr {
            height: 450px;
        }

        .MyCalendar {
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

        function myFuncionddl() {
            $('#<%=DdlAk.ClientID%>').chosen();
            $('#<%=DdlPN.ClientID%>').chosen();
            $('#<%=DdlSN.ClientID%>').chosen();
            $('#<%=DdlContador.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplHisAkComp" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <asp:Label ID="LblAk" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                    <asp:DropDownList ID="DdlAk" runat="server" CssClass="heightCampo" Width="15%" OnTextChanged="DdlAk_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                    <asp:DropDownList ID="DdlPN" runat="server" CssClass="heightCampo" Width="20%" OnTextChanged="DdlPN_TextChanged" AutoPostBack="true" />
                    <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                    <asp:DropDownList ID="DdlSN" runat="server" CssClass="heightCampo" Width="20%" OnTextChanged="DdlSN_TextChanged" AutoPostBack="true" /><br />
                    <br />
                    <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial"/>
                    <asp:TextBox ID="TxtFechI" runat="server" CssClass="heightCampo" Width="10%" TextMode="Date" MaxLength="10"/>
                    <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="Fecha Final" />
                    <asp:TextBox ID="TxtFechF" runat="server" CssClass="heightCampo" Width="10%" TextMode="Date" MaxLength="10" />
                    &nbsp&nbsp
                     <asp:Button ID="BtnConsultar" CssClass="btn btn-primary" runat="server" Height="33px" Text="Consultar" OnClick="BtnConsultar_Click" />
                    <div class="CentarGridAsig table-responsive Scroll">
                        <asp:GridView ID="GrdHistor" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="CodigoIdHistoricoAeronaveVirtual,CodAeronave"
                            CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" OnRowDeleting="GrdHistor_RowDeleting"
                            OnRowEditing="GrdHistor_RowEditing" OnRowCommand="GrdHistor_RowCommand" OnRowUpdating="GrdHistor_RowUpdating" OnRowCancelingEdit="GrdHistor_RowCancelingEdit"
                            OnRowDataBound="GrdHistor_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Procesar">
                                    <ItemTemplate>                                     
                                        <asp:ImageButton ID="IbtProcesar" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Procesar" ToolTip="Procesar un elemento." />                                      
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Matricula" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblMtrP" Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblPn" Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblSn" Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mayor">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbMayorP" Checked='<%# Eval("ComponenteMayorR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbMayor" Checked='<%# Eval("ComponenteMayorR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SubComp.">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbSubCP" Checked='<%# Eval("SubComponenteR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbSubC" Checked='<%# Eval("SubComponenteR").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha evento" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblFecEvP" Text='<%# Eval("FechaMontaje") %>' runat="server" Width="70%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtFecEv" Text='<%# Eval("FechaMontaje") %>' runat="server" Width="70%" Enabled="false" />
                                        <asp:ImageButton ID="IbnFecEv" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                        <ajaxToolkit:CalendarExtender ID="CalTxtFecEv" runat="server" PopupButtonID="IbnFecEv" TargetControlID="TxtFecEv" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posicion">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Posicion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Posicion") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UbicacionTecnica" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodUbicacionFisicaH") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("CodUbicacionFisicaH") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Evento">
                                    <ItemTemplate>
                                        <asp:Label ID="LblOTRtP" Text='<%# Eval("Evento") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblOtE" Text='<%# Eval("Evento") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Motivo">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("MotivoRemocion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtMotivo" Text='<%# Eval("MotivoRemocion") %>' runat="server" Width="100%" MaxLength="240" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha_Mvto" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Fecha_Mvto") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Fecha_Mvto") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" />
                        </asp:GridView>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Procesar" runat="server">
                    <asp:ImageButton ID="IbtCerrarProces" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarProces_Click" ImageAlign="Right" />
                    <div class="col-sm-7">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitHisC1Aplicab" runat="server" Text="Procesar contadores" /></h6>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblHkProcs" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                <asp:TextBox ID="TxtHkProcs" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblPNProc" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtPNProc" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblSNProc" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSNProc" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblFechMyr" runat="server" CssClass="LblEtiquet" Text="Mayor a" />
                                <asp:TextBox ID="TxtFechMyr" runat="server" CssClass="heightCampo" Width="100%" TextMode="Date" MaxLength="10" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblFechHast" runat="server" CssClass="LblEtiquet" Text="Hasta" />
                                <asp:TextBox ID="TxtFechHast" runat="server" CssClass="heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblContador" runat="server" CssClass="LblEtiquet" Text="Contador" />
                                <asp:DropDownList ID="DdlContador" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <br />
                                <asp:Button ID="BtnConsulProces" CssClass="btn btn-primary" runat="server" Height="33px" Text="Cons" OnClick="BtnConsulProces_Click" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:Button ID="BtnPrcsCont" CssClass="btn btn-primary" runat="server" Height="33px" Text="Procesar" OnClick="BtnPrcsCont_Click" />
                            </div>
                        </div>
                        <div class="table-responsive Scroll-table2">
                            <asp:GridView ID="GrdProcesar" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdHist,CodIdCont,SumIndiv,VlrMax,Descripcion"
                                CssClass="DiseñoGrid" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFecha" Text='<%# Eval("Fecha") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Individual" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblIndivid" Text='<%# Eval("Horas") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acumulado" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblAcumul" Text='<%# Eval("TSN_actual") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Libro Vuelo" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblLibroV" Text='<%# Eval("CodlV") %>' runat="server" Width="100%" Enabled="false" />
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
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers></Triggers>
    </asp:UpdatePanel>
</asp:Content>
