<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmReferencia.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmReferencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Referencia</title>
    <style type="text/css">
        .DivGrid {
            position: absolute;
            OVERFLOW: auto;
            width: 98%;
            height: 73%;
            top: 24%;
            left: 1%;
            margin-top: 0px;
        }

        .TablaCampos {
            position: absolute;
            text-align: left;
            width: 95%;
        }

        .Campos {
            Height: 30px;
            Width: 100%;
            font-size: 12px;
        }

        .PanelMaestroArt {
            position: absolute;
            width: 21%;
            border: solid;
            border-color: cadetblue;
            height: 97%;
        }

        .TituloMA {
            background-color: cadetblue; /*bg-info text-center*/
            text-align: center;
            color: aliceblue;
            width: 100%;
            /* font-size: 18px;*/
        }

        .DivGridPsc {
            position: absolute;
            width: 21%;
            height: 65%;
            /*top: 66%;
/*            left: 56%;*/
            margin-top: 0px;
        }

        .DivGridPN {
            position: absolute;
            width: 75%;
            height: 300px;
            /* top: 50%;
            left: 1%;*/
            margin-top: 0px;
        }

        .CsGridCambUC {
            width: 100%;
            height: 100%;
        }

        .DivUndCom {
            position: absolute;
            width: 50%;
            height: 60%;
            top: 20%;
            left: 25%
        }
    </style>
    <script type="text/javascript">

        function isNumberKey(evt) {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=DdlGrupo.ClientID%>,#<%=DdlAta.ClientID%>, #<%=DdlUM.ClientID%>,#<%=DdlIdent.ClientID%>, #<%=DdlTipo.ClientID%>, #<%=DdlMod.ClientID%>,#<%=DdlCat.ClientID%>').chosen();
            //$('[id*=DdlGrupo],[id*=DdlAta],[id*=DdlUM],[id*=DdlIdent],[id*=DdlTipo],[id*=DdlMod],[id*=DdlCat]').chosen();
            $('[id *=DdlFabPP], [id *=DdlFab], [id *=DdlContPP], [id *=DdlCUMCPP]').chosen();
            $('[id*=DdlEstPNPP],[id*=DdlEstPN],[id*=DdlManPP],[id*=DdlUMComPP],[id*=DdlUMCom]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo"></asp:Label></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlCampos" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlCampos" runat="server">
                <div class=" CentrarTable ">
                    <table class="TablaCampos table table-sm">
                        <tr>
                            <td>
                                <asp:Label ID="LblCodigo" runat="server" CssClass="LblEtiquet" Text="Código:" />
                            </td>
                            <td width="25%">
                                <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control Campos" Enabled="false" MaxLength="80" />
                            </td>
                            <td>
                                <asp:Label ID="LblGrupo" runat="server" CssClass="LblEtiquet" Text="Grupo:" />
                            </td>
                            <td width="20%">
                                <asp:DropDownList ID="DdlGrupo" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" OnTextChanged="DdlGrupo_TextChanged" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Ata:" />
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="DdlAta" runat="server" CssClass="Campos" Enabled="false" OnTextChanged="DdlAta_TextChanged" AutoPostBack="true" />
                            </td>
                            <td width="30%" rowspan="6">
                                <asp:Panel ID="Panel2" runat="server" CssClass="PanelMaestroArt">
                                    <%--<h6 class="TituloMA">Maestro de artículo</h6>--%>
                                    <h6 class="TextoSuperior">
                                        <asp:Label ID="LblTitMaesArt" runat="server" Text="Maestro de artículo" /></h6>
                                    <table width="100%">
                                        <tr>
                                            <td width="5%">
                                                <asp:Label ID="LblStokMin" runat="server" CssClass="LblEtiquet" Text="Stock Min:" /></td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtStockM" runat="server" CssClass="form-control Campos" Enabled="false" Width="100%" /></td>
                                            <td width="45%">
                                                <asp:CheckBox ID="CkbVerif" runat="server" CssClass="LblEtiquet" Text="Verificado" Enabled="false" />
                                            </td>
                                            <%-- <td width="25%" class="LblEtiquet">Verificado:</td>
                                            <td width="20%">
                                                <asp:CheckBox ID="CkbVerif" runat="server" Enabled="false" /></td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblCateg" runat="server" CssClass="LblEtiquet" Text="Categoría:" /></td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DdlCat" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" Width="100%" /></td>
                                            <td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="GrdMan" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="ID"
                                        CssClass="GridControl DiseñoGrid table table-sm " GridLines="Both" AllowPaging="true" PageSize="2" Width="100%"
                                        OnRowCommand="GrdMan_RowCommand" OnRowDeleting="GrdMan_RowDeleting"
                                        OnRowDataBound="GrdMan_RowDataBound" OnPageIndexChanging="GrdMan_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Condición Manipulación / Almacenamiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Condicion") %>' runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="DdlManPP" runat="server" Width="100%" Height="28px" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="30px">
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
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblModelo" runat="server" CssClass="LblEtiquet" Text="Modelo:" /></td>
                            <td>
                                <asp:DropDownList ID="DdlMod" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" OnTextChanged="DdlMod_TextChanged" AutoPostBack="true"></asp:DropDownList></td>
                            <td>
                                <asp:Label ID="LblUndDesp" runat="server" CssClass="LblEtiquet" Text="U. M. Despacho:" /></td>
                            <td>
                                <asp:DropDownList ID="DdlUM" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" /></td>
                            <td>
                                <asp:Label ID="LblIdentElem" runat="server" CssClass="LblEtiquet" Text="Ident. Elemento:" /></td>
                            <td>
                                <asp:DropDownList ID="DdlIdent" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblDescripc" runat="server" CssClass="LblEtiquet" Text="Descripcion:" /></td>
                            <td colspan="3">
                                <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control Campos" MaxLength="240" Enabled="false" /></td>
                            <td>
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo:" /></td>
                            <td>
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblDescEsp" runat="server" CssClass="LblEtiquet" Text="Descripcion Español:" /></td>
                            <td colspan="3">
                                <asp:TextBox ID="TxtDescEsp" runat="server" CssClass="form-control Campos" TextMode="MultiLine" MaxLength="240" Enabled="false" /></td>
                            <td>
                                <asp:Label ID="LblInfoAdic" runat="server" CssClass="LblEtiquet" Text="Información adicional:" /></td>
                            <td>
                                <asp:TextBox ID="TxtInfAd" runat="server" CssClass="form-control Campos" TextMode="MultiLine" MaxLength="200" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblReparab" runat="server" CssClass="LblEtiquet" Text="Reparable" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblSi" runat="server" CssClass="LblEtiquet" Text="Sí" /></td>
                                        <td>
                                            <asp:RadioButton ID="RdbSi" runat="server" TextAlign="Left" GroupName="Repa" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="LblNo" runat="server" CssClass="LblEtiquet" Text="No" /></td>
                                        <td>
                                            <asp:RadioButton ID="RdbNo" runat="server" TextAlign="Left" GroupName="Repa" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CkbPos" runat="server" CssClass="LblEtiquet" Text="Posición" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbCons" runat="server" CssClass="LblEtiquet" Text="Consumo" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbMot" runat="server" CssClass="LblEtiquet" Text="Motor" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbMay" runat="server" CssClass="LblEtiquet" Text="Mayor" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbApu" runat="server" CssClass="LblEtiquet" Text="&nbsp Apu" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbSub" runat="server" CssClass="LblEtiquet" Text="&nbsp Sub Comp" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbNiF" runat="server" Text="Activo Nif: " ForeColor="#990000" TextAlign="Left" Enabled="false" Visible="false" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" /></td>
                                        <td>
                                            <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" /></td>
                                        <td>
                                            <asp:Button ID="BtnConsultar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnConsultar_Click" Text="Consultar" /></td>
                                        <td>
                                            <asp:Button ID="BtnInformes" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnInformes_Click" Text="Informes" /></td>
                                        <td>
                                            <asp:Button ID="BtnEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnEliminar_Click" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" /></td>
                                        <td>
                                            <asp:Button ID="BtnUndCompra" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnUndCompra_Click" Text="Unidad Compra" /></td>
                                        <td>
                                            <asp:Button ID="BtnCambioRef" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnCambioRef_Click" Text="Cambio Referencia" /></td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr id="Partes_Contadores">
                            <td colspan="6">
                                <asp:GridView ID="GrdPN" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="PN,ParteAnterior,CodigoExternoPN,Equivalencia,UndCompra"
                                    CssClass="GridControl DiseñoGrid table table-sm DivGridPN" GridLines="Both" AllowPaging="true" PageSize="5"
                                    OnRowCommand="GrdPN_RowCommand" OnSelectedIndexChanged="GrdPN_SelectedIndexChanged" OnRowEditing="GrdPN_RowEditing"
                                    OnRowUpdating="GrdPN_RowUpdating" OnRowCancelingEdit="GrdPN_RowCancelingEdit"
                                    OnRowDeleting="GrdPN_RowDeleting" OnRowDataBound="GrdPN_RowDataBound" OnPageIndexChanging="GrdPN_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtPN" Text='<%# Eval("PN") %>' runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtPNPP" runat="server" MaxLength="80" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("EstadoPn") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlEstPN" runat="server" Width="100%" Height="28px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlEstPNPP" runat="server" Width="100%" Height="28px" Font-Size="2em" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bloq.">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbBloqP" Checked='<%# Eval("Bloquear").ToString()=="1" ? true : false %>' runat="server" Enabled="false" Width="50px" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CkbBloq" Checked='<%# Eval("Bloquear").ToString()=="1" ? true : false %>' runat="server" Width="50px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="CkbBloqPP" runat="server" Width="50px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NSN" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("NSN") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtNSN" Text='<%# Eval("NSN") %>' runat="server" MaxLength="80" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNSNPP" runat="server" MaxLength="80" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="U.M. Compra" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("UndCompra") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlUMCom" runat="server" Width="100%" Height="28px" OnTextChanged="DdlUMCom_TextChanged" AutoPostBack="true" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlUMComPP" runat="server" Width="100%" Height="28px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Equival." HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Equivalencia") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtEqu" Text='<%# Eval("Equivalencia") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtEquPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return isNumberKey(event);" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Vencim">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbFVP" Checked='<%# Eval("FechaVencPN").ToString()=="1" ? true : false %>' runat="server" Enabled="false" Width="50px" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CkbFV" Checked='<%# Eval("FechaVencPN").ToString()=="1" ? true : false %>' runat="server" Width="50px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="CkbFVPP" runat="server" Width="50px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fabricante" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Fabricante") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlFab" runat="server" Width="100%" Height="28px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlFabPP" runat="server" Width="100%" Height="28px" Font-Size="2em" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="7%">
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
                            </td>
                            <td>
                                <asp:GridView ID="GrdCont" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdContadorPn,CodContador"
                                    CssClass="GridControl DiseñoGrid table table-sm DivGridPsc" GridLines="Both" AllowPaging="true" PageSize="5"
                                    OnRowCommand="GrdCont_RowCommand" OnRowDeleting="GrdCont_RowDeleting"
                                    OnRowDataBound="GrdCont_RowDataBound" OnPageIndexChanging="GrdCont_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Contador asignado">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CodContador") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlContPP" runat="server" Width="100%" Height="28px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="30px">
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
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="PnlBusq" runat="server" Visible="false">
                <h6 class="TextoSuperior">
                    <asp:Label ID="LblTitOpcBusq" runat="server" Text="Opciones de búsqueda" /></h6>
                <table class="TablaBusqueda">
                    <tr>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqR" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="Referencia" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqP" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="&nbsp P/N" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqD" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="Descripción" /></td>
                    </tr>
                </table>
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        <td>
                            <asp:ImageButton ID="IbtCerrar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrar_Click" /></td>
                    </tr>
                </table>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdDatos" runat="server" EmptyDataText="No existen registros ..!"
                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="7"
                        OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged" OnPageIndexChanging="GrdDatos_PageIndexChanging" OnRowDataBound="GrdDatos_RowDataBound">
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
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DdlGrupo" EventName="TextChanged" />
            <asp:PostBackTrigger ControlID="BtnInformes" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpPnlUndCompra" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlUnidadCompra" runat="server" Visible="false">
                <h6 class="TextoSuperior">
                    <asp:Label ID="LblTitAsigUndMed" runat="server" Text="Asignar unidad de compra" /></h6>
                <asp:ImageButton ID="IbtCerrarUMC" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarUMC_Click" ImageAlign="Right" />
                <div class=" DivUndCom DivContendorGrid">
                    <asp:Label ID="LblCambioPN" runat="server" CssClass="LblEtiquet" Font-Bold="true" Font-Size="16px"></asp:Label>
                    <asp:GridView ID="GrdCamUC" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdundPN"
                        CssClass="GridControl DiseñoGrid table table-sm CsGridCambUC" GridLines="Both" AllowPaging="true" PageSize="5"
                        OnRowCommand="GrdCamUC_RowCommand" OnSelectedIndexChanged="GrdCamUC_SelectedIndexChanged" OnRowEditing="GrdCamUC_RowEditing"
                        OnRowUpdating="GrdCamUC_RowUpdating" OnRowCancelingEdit="GrdCamUC_RowCancelingEdit"
                        OnRowDeleting="GrdCamUC_RowDeleting" OnRowDataBound="GrdCamUC_RowDataBound" OnPageIndexChanging="GrdCamUC_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="U.M. Compra" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCUMCP" Text='<%# Eval("UndCompraPN") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCUMC" Text='<%# Eval("UndCompraPN") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlCUMCPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Equival." HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCEquP" Text='<%# Eval("VlorEquivalencia") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCEqu" Text='<%# Eval("VlorEquivalencia") %>' runat="server" Width="100%" step="0.01" onkeypress="return isNumberKey(event);" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCEquPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return isNumberKey(event);" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="U. M.Despacho" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("UndDespachoPn") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCUD" Text='<%# Eval("UndDespachoPn") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCUDPP" runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="30px">
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
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="PnlCambioRef" runat="server" Visible="false">
                <h6 class="TextoSuperior">
                    <asp:Label ID="LblTitCambRef" runat="server" Text="Cambio de referencia" /></h6>
                <table class="TablaBusqueda">
                    <tr>
                        <td width="10%">
                            <asp:RadioButton ID="RdbRefCRef" runat="server" CssClass="LblEtiquet" GroupName="CambRef" Text="Referencia" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbPnCRef" runat="server" CssClass="LblEtiquet" GroupName="CambRef" Text="&nbsp P/N" /></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblPNCRef" runat="server" CssClass="LblEtiquet" Font-Bold="true" Font-Size="16px" /></td>
                    </tr>
                </table>
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqCambRef" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:TextBox ID="TxtCambRef" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultarCambRef" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultarCambRef_Click" /></td>
                        <td>
                            <asp:ImageButton ID="IbtApliarCambRef" runat="server" ToolTip=" " CssClass="BtnAceptar" ImageUrl="~/images/Save.png" OnClick="IbtApliarCambRef_Click" OnClientClick="javascript:return confirm('Desea asignar el P/N a la nueva referencia?');" /></td>
                        <td>
                            <asp:ImageButton ID="IbtCerrarCambRef" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCambRef_Click" /></td>
                    </tr>
                </table>
                <div class="DivGridCamRef DivContendorGrid">
                    <asp:Label ID="LblRefCambRef" runat="server" CssClass="LblEtiquet" Font-Bold="true" Font-Size="24px"></asp:Label><br />
                    <asp:GridView ID="GrdCambioRef" runat="server" EmptyDataText="No existen registros ..!"
                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="6"
                        OnSelectedIndexChanged="GrdCambioRef_SelectedIndexChanged" OnPageIndexChanging="GrdCambioRef_PageIndexChanging">
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
