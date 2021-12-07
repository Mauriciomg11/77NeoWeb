<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Frm_FCentroCostos.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.Frm_FCentroCostos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style type="text/css">
        .CentrarContenedor {
            /*vertical-align: top;*/
            background: #e0e0e0;
            margin: 0 0 1rem;
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            /*margin-top: -150px;*/
            border: 1px solid #808080;
            padding: 5px;
        }
         .CentrarBoton {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
     <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
        <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">
                <div class="CentrarTable">
                    <table class="TablaBusqueda">
                        <tr>
                            <td>
                                <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtBusqueda" runat="server" Width="500px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        </tr>
                    </table>
                     <div class="row ">
                         <div class="col-sm-10 CentrarBoton ">
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdCCostos, CodCc"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                            OnRowCommand="GrdDatos_RowCommand"  OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating"
                            OnRowCancelingEdit="GrdDatos_RowCancelingEdit"  OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound"
                            OnPageIndexChanging="GrdDatos_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Cód" FooterStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodCc") %>' runat="server" Width="50px" />
                                    </ItemTemplate>
                                     <EditItemTemplate>
                                        <asp:TextBox ID="TxtCod" Text='<%# Eval("CodCc") %>' runat="server" MaxLength="15" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCodPP" runat="server" MaxLength="15" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Nombre") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtNom" Text='<%# Eval("Nombre") %>' runat="server" MaxLength="40" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtNomPP" runat="server" MaxLength="40" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>  
                                  <asp:TemplateField HeaderText="Salida CC">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkSalidaCCP" Checked='<%# Eval("Salida_CC").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkSalidaCC" Checked='<%# Eval("Salida_CC").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkSalidaCCPP" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StockAlma">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkStockAlmaP" Checked='<%# Eval("StockAlma").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkStockAlma" Checked='<%# Eval("StockAlma").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkStockAlmaPP" runat="server"/>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StockRepa">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkStockRepaP" Checked='<%# Eval("StockRepa").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkStockRepa" Checked='<%# Eval("StockRepa").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkStockRepaPP" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="StockHerrta">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbStockHerrtaP" Checked='<%# Eval("StockHerrta").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbStockHerrta" Checked='<%# Eval("StockHerrta").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkbStockHerrtaPP" runat="server"  />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acti">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbActP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbAct" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkbActPP" runat="server" Checked="true" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
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
                </div></div></div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
