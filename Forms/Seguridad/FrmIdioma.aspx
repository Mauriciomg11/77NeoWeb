<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmIdioma.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmIdioma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarBoton {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 90%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -45%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">  
        function myFuncionddl() {
            $('#<%=DdlForm.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
  <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <div class="CentrarContenedor">
                <br />
                <br />
                <table class="">
                    <tr>
                        <td>
                            <asp:RadioButton ID="RdbMens" runat="server" CssClass="LblEtiquet" Text="&nbsp Español" GroupName="BusqRp" Checked="true" />&nbsp&nbsp
                            <asp:RadioButton ID="RdbIngles" runat="server" CssClass="LblEtiquet" Text="&nbsp Ingles" GroupName="BusqRp" />&nbsp&nbsp
                            <asp:RadioButton ID="RdbObj" runat="server" CssClass="LblEtiquet" Text="&nbsp Objeto" GroupName="BusqRp" />&nbsp&nbsp                         
                            <asp:RadioButton ID="RdbDesc" runat="server" CssClass="LblEtiquet" Text="&nbsp Descripción" GroupName="BusqRp" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="500px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:CheckBox ID="CkbSinCorr" runat="server" CssClass="LblEtiquet" Text="&nbsp Sin Corregir" Checked="true" /></td>

                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col-sm-12">
                        <table class="">
                            <tr>
                                <td>
                                    <asp:TextBox ID="TxtIdCia" runat="server" Width="80px" Height="28px" CssClass="form-control" TextMode="Number" Text="0" Visible="false" /></td>
                                <td>
                                    <asp:TextBox ID="TxtPassCia" runat="server" Width="200px" Height="28px" CssClass="form-control" TextMode="Password" Text="" Visible="false" /></td>
                                <td>
                                    <asp:ImageButton ID="IbtCambioPassCia" runat="server" ToolTip="Guardar Clave" Width="30px" Height="30px" ImageUrl="~/images/Check.png" OnClick="IbtCambioPassCia_Click" Visible="false" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 CentrarBoton">
                        <asp:DropDownList ID="DdlForm" runat="server" CssClass="form-control" Width="100%" Height="30px" Font-Size="Smaller" OnTextChanged="DdlForm_TextChanged" AutoPostBack="true" />
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdFomularioUsr, IdFormulario"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdDatos_RowCommand"
                            OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField HeaderText="Cod">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("IdFormulario") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblIdF" Text='<%# Eval("IdFormulario") %>' runat="server" Width="100" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Nombre") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblNom" Text='<%# Eval("Nombre") %>' runat="server" Width="100" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Objeto" HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Objeto") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtObj" Text='<%# Eval("Objeto") %>' runat="server" MaxLength="50" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtObjPP" runat="server" MaxLength="50" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="50" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescPP" runat="server" MaxLength="50" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Español" HeaderStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Espanol") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtEspa" Text='<%# Eval("Espanol") %>' runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtEspaPP" runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ingles" HeaderStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Ingles") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtIngl" Text='<%# Eval("Ingles") %>' runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtInglPP" runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Corregido">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbRevP" Checked='<%# Eval("Aleman").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbRev" Checked='<%# Eval("Aleman").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="5%">
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
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
