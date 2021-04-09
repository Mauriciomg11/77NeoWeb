<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmCicloDiscosMotor.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.MaestIngPrg.FrmCicloDiscosMotor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>CicloDiscos</title>
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
            if (key < 46 || key > 57) {
                return false;
            }
            else if (key == 47) {
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
    </script>

    <style type="text/css">
        .DivGrid {
            margin: 0 auto;
            text-align: left;
            width: 85%;
            height: 600px;
            top: 15%;
            margin-top: 0px;
        }

        .DivGridAR {
            margin: 0 auto;
            text-align: left;
            width: 98%;
            height: 600px;
            top: 15%;
            margin-top: 0px;
        }

        .TableAla {
            margin: 0 auto;
            text-align: left;
            width: 25%;
            height: 4%;
        }

        .CeldaTabla {
            width: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('[id*=DdlMotorPP],[id*=DdlSubCPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlAF" runat="server">
        <ContentTemplate>
            <div class="CentrarTable">
                <table class="TableAla">
                    <tr>
                        <td class="CeldaTabla">
                            <asp:Button ID="BtnAlaF" runat="server" CssClass=" btn border-primary LblTextoBusq" OnClick="BtnAlaF_Click" Text="Ala Fija" Width="100%" ForeColor="White" /></td>
                        <td class="CeldaTabla">
                            <asp:Button ID="BtnAlaR" runat="server" CssClass=" btn border-primary LblTextoBusq" OnClick="BtnAlaR_Click" Text="Ala Rotatoria" Width="100%" ForeColor="White" /></td>
                    </tr>
                </table>
            </div>
            <div class="CentrarTable">
                <asp:Panel ID="PnlAF" runat="server">
                    <div class="DivGrid DivContendorGrid">
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="ID"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                            OnRowCommand="GrdDatos_RowCommand" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged"
                            OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound" OnPageIndexChanging="GrdDatos_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Motor" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Engine") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlMotorPP" runat="server" Width="100%" Height="28px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subcomponente" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlSubCPP" runat="server" Width="100%" Height="28px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ciclos Equivalentes" HeaderStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CiclosEquivalente") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtCiclos" Text='<%# Eval("CiclosEquivalente") %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCiclosPP"  runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
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
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpPnlAR" runat="server">
        <ContentTemplate>
            <div class="CentrarTable">
                <asp:Panel ID="PnlAR" runat="server" Visible="false">
                    <div class="DivGridAR DivContendorGrid">
                        <asp:GridView ID="GrdAR" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="ID"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                            OnRowCommand="GridAR_RowCommand" OnSelectedIndexChanged="GridAR_SelectedIndexChanged"
                            OnRowDeleting="GridAR_RowDeleting" OnRowDataBound="GridAR_RowDataBound" OnPageIndexChanging="GridAR_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Motor" HeaderStyle-Width="23%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Engine") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlMotorPP" runat="server" Width="100%" Height="28px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subcomponente" HeaderStyle-Width="23%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlSubCPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlSubCPP_TextChanged" AutoPostBack="true" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="23%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Description") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescPP" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cycle Factor Abbr'd" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CFAbbr") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCFAbbrPP" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cycle Factor Ext'd" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CFExt") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCFExtPP"  runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight Count Factor" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("FCFactor") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtFCFactorPP"  runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Life Limit Hours" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("LLHours") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtLLHoursPP"  runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Life Limit Cycles" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("LLCycles") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtLLCyclesPP"  runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                       <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="8"/>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
