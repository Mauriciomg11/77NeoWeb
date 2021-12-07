<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="TipoContrato.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.ControlPersonal.TipoContrato" %>
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
            width: 60%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -30%;
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
                         <div class="col-sm-6 CentrarBoton ">
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodTipoContrPer"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                            OnRowCommand="GrdDatos_RowCommand"  OnRowEditing="GrdDatos_RowEditing"  OnRowUpdating="GrdDatos_RowUpdating"
                            OnRowCancelingEdit="GrdDatos_RowCancelingEdit"  OnRowDeleting="GrdDatos_RowDeleting"  OnRowDataBound="GrdDatos_RowDataBound"
                            OnPageIndexChanging="GrdDatos_PageIndexChanging" >
                            <Columns>
                                <asp:TemplateField HeaderText="Cód">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodTipoContrPer") %>' runat="server" Width="50px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripc" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="200" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescPP" runat="server" MaxLength="200" Width="100%" />
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
                </div></div></div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
