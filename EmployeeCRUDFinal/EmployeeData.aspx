<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeData.aspx.cs" Inherits="EmployeeCRUDFinal.EmployeeData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
        <div>
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">Name</td>
                <td>
                    <asp:TextBox ID="Name" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Gender</td>
                <td>
                    <asp:RadioButtonList ID="Gender" runat="server">
                        <asp:ListItem ID="Female">Female</asp:ListItem>
                        <asp:ListItem ID="Male">Male</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Country</td>
                <td>
                    <asp:DropDownList ID="CountryDropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CountryDropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">State</td>
                <td>
                    <asp:DropDownList ID="StateDropDownList1" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
         
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td>
                    <asp:Button ID="Insert" runat="server" Height="38px" OnClick="Insert_Click" Text="Insert" Width="125px" />
                </td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
     
     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true"  DataKeyNames="EmployeeId"   OnRowCancelingEdit="RowCancelButton_Click" OnRowDeleting="DeleteRowButton_Click" OnRowEditing="EditButton_Click" OnRowUpdating ="RowUpdateButton_CLick" isReadOnly="true"   >  
                    <Columns>  
                   <%-- <asp:BoundField DataField="id" HeaderText="S.No." />  --%>
<%--                        <asp:BoundField DataField="Name" HeaderText="Name" />  
                        <asp:BoundField DataField="Gender" HeaderText="Gender" />  
                        <asp:BoundField DataField="CountryId" HeaderText="CountryId" />  
                        <asp:BoundField DataField="StateId" HeaderText="StateId" /> --%>
                        <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="DeleteRowButton_Click" runat="server" CommandName="Delete" Text="Delete"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="EditButton_Click" runat="server" CommandName="Edit" Text="Edit"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="RowUpdateButton_CLick" runat="server" CommandName="Update" Text="Update" />
                        <asp:Button ID="RowCancelButton_Click" runat="server" CommandName="Cancel" Text="Cancel"/>
                    </EditItemTemplate>
                </asp:TemplateField>

                         </Columns>  
                </asp:GridView> 
    </form>
</body>
</html>
