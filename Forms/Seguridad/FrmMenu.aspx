<%@ Page MaintainScrollPositionOnPostback="true" Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmMenu.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Menu</title>
    <style type="text/css">
        .centrarDivPpal {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            top: 2px;
            left: 30%;
            /*determinamos una anchura*/
            width: 37%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: 2px;
            /*determinamos una altura*/
            height: 100%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            margin-top: 2px;
            border: 1px solid #808080;
            padding: 5px;
            background-color: rgba(0, 0, 0, 0.5);
            color: #000;
        }

        .DivGrid {
            position: absolute;
            width: 98%;
            height: 84%;
            top: 14%;
            left: 1%;
            margin-top: 0px;
        }

        .GridDis {
            Width: 100%;
            height: 60em;
            /* border-color:black; --BorderColor="#999999" */
            /*  border-style:double; -- BorderStyle="Double" */
            border-width: 3px;
            /*BorderWidth="1px"*/
        }

        .ScrollDet {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 480px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <table >
                <tr>
                    <td>
                        <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                    <td>
                        <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                      <td>
                        <asp:ImageButton ID="IbtAbrirIdioma" runat="server" Visible="false" ToolTip="Abrir Idioma" ImageUrl="~/images/IrV2.png" Width="30px" Height="30px" OnClick="IbtAbrirIdioma_Click" /></td>
                </tr>
            </table>
            <div class="ScrollDet">
                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdFormulario,RutaFormulario"
                    CellPadding="3" CssClass="DiseñoGrid table-sm" GridLines="Both"
                    OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                    OnRowDeleting="GrdDatos_RowDeleting" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged" OnRowDataBound="GrdDatos_RowDataBound">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <RowStyle CssClass="GridRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Ir">
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UplAbrir2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="IbtAbrir" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Abrir" OnClick="IbtAbrir_Click" OnClientClick="target ='_blank';" ToolTip="Acceder a la pantalla." />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="IbtAbrir" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NomFormWeb" HeaderText="NomFrmInv" Visible="false" />
                        <asp:TemplateField HeaderText="Posición">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("PosicionVble") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtPos" Text='<%# Eval("PosicionVble") %>' runat="server" Width="100px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtPosPP" runat="server" Width="100px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción" ItemStyle-HorizontalAlign="Left" >
                            <ItemTemplate>
                                <asp:TextBox ID="TxtIdDescrP" Text='<%# Eval("DescSangria") %>' runat="server" Width="300px" ReadOnly="true" TextMode="MultiLine" Height="35px" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtIdDescr" Text='<%# Eval("Descripcion") %>' runat="server" Width="300px" TextMode="MultiLine" Height="35px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtIdDescrPP" runat="server" Width="300px" TextMode="MultiLine" Height="35px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Posición Superior">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("PerteneceMenu") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtPosSup" Text='<%# Eval("PerteneceMenu") %>' runat="server" Width="100px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtPosSupPP" runat="server" Width="100px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Posición Principal">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("PerteneceMenuPpal") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtPosMaster" Text='<%# Eval("PerteneceMenuPpal") %>' runat="server" Width="100px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtPosMasterPP" runat="server" Width="100px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nivel">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("Sangria") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtNivel" Text='<%# Eval("Sangria") %>' runat="server" Width="30px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtNivelPP" runat="server" Width="30px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ruta">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("RutaFormulario") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtRuta" Text='<%# Eval("RutaFormulario") %>' runat="server" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtRutaPP" runat="server" Width="100%" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label ID="LblNomForm" Text='<%# Eval("NomFormWeb") %>' runat="server" Width="100%" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtNomForm" Text='<%# Eval("NomFormWeb") %>' runat="server" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtNomFormPP" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDForm" Visible="false">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("CodIdFormulario") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label Text='<%# Eval("CodIdFormulario") %>' runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
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
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


