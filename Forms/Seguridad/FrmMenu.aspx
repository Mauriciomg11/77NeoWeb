<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmMenu.aspx.cs" Inherits="_77NeoWeb.Forms.FrmMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Menu</title>
    <h1>Menú virtual</h1>
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

        

        
        .AlinearTextoGrid{

            vertical-align:
        }
        .DivGrid {
            width: 98%;
            height: 600px;
            top: 15%;
            margin-top: 0px;
            overflow: scroll;
        }
        .GridDis {
            background-color: white; /*Color del fondo*/
            font-family: Arial;
            font-size: smaller;
            color: midnightblue; /*Color del texto*/
            Width: 100%;
            /* border-color:black; --BorderColor="#999999" */
            /*  border-style:double; -- BorderStyle="Double" */
            /*border-width:3px;-- BorderWidth="1px"*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>

            <div>
                <div class="DivGrid">
                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdFormulario,NomFormWeb"
                        CellPadding="3" CssClass="GridDis" GridLines="Both"
                         OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                        On OnRowDeleting="GrdDatos_RowDeleting" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged" OnRowDataBound="GrdDatos_RowDataBound">
                        <FooterStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#0000ff" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#cae4ff" />
                        <Columns>
                            <asp:CommandField HeaderText="Ir" SelectText="abrir" ShowSelectButton="True" ControlStyle-Width="70px" />
                            <asp:BoundField DataField="NomFormWeb" HeaderText="NomFrmInv" Visible="false" />
                            <asp:TemplateField HeaderText="Posición">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("PosicionVble") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtPos" Text='<%# Eval("PosicionVble") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtPosPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DescSangria") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtIdDescr" Text='<%# Eval("Descripcion") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtIdDescrPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posición Superior">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("PerteneceMenu") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtPosSup" Text='<%# Eval("PerteneceMenu") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtPosSupPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posición Principal">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("PerteneceMenuPpal") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtPosMaster" Text='<%# Eval("PerteneceMenuPpal") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtPosMasterPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nivel">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Sangria") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtNivel" Text='<%# Eval("Sangria") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtNivelPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ruta">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("RutaFormulario") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtRuta" Text='<%# Eval("RutaFormulario") %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtRutaPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="LblNomForm" Text='<%# Eval("NomFormWeb") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtNomForm" Text='<%# Eval("NomFormWeb") %>' runat="server" />
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
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton CssClass="BotonCancelGrid" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
