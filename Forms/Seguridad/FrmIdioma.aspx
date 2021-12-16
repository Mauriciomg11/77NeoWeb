<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmIdioma.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmIdioma" %>

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
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
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
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">
             <%--   <div class="CentrarTable">--%>
                    <table class="">
                        <tr>
                            <td>
                                <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" />&nbsp&nbsp&nbsp&nbsp</td>
                             <td>
                                <asp:RadioButton ID="RdbObj" runat="server" CssClass="LblEtiquet" Text="&nbsp Objeto" GroupName="BusqRp" Checked="true" />&nbsp&nbsp                         
                                <asp:RadioButton ID="RdbDesc" runat="server" CssClass="LblEtiquet" Text="&nbsp Descripción" GroupName="BusqRp" /></td>
                            <td>
                                <asp:TextBox ID="TxtBusqueda" runat="server" Width="300px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:CheckBox ID="CkbSinCorr" runat="server" CssClass="LblEtiquet" Text="&nbsp Sin Corregir" Checked="true" /></td>
                           
                            <td>
                                <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        </tr>
                    </table>
                    <div class="row">
                        <div class="col-sm-12 CentrarBoton">
                            <asp:DropDownList ID="DdlForm" runat="server" CssClass="form-control" Width="100%" Height="30px" Font-Size="Smaller" />
                            <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdFomularioUsr, IdFormulario"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="10"
                                OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnPageIndexChanging="GrdDatos_PageIndexChanging">
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
                                    <asp:TemplateField HeaderText="Objeto" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Objeto") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblObj" Text='<%# Eval("Objeto") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Español" HeaderStyle-Width="35%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Espanol") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtEspa" Text='<%# Eval("Espanol") %>' runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ingles" HeaderStyle-Width="35%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Ingles") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtIngl" Text='<%# Eval("Ingles") %>' runat="server" MaxLength="350" TextMode="MultiLine" Width="100%" />
                                        </EditItemTemplate>
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
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
                            </asp:GridView>
                        </div>
                    </div>
               <%-- </div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
