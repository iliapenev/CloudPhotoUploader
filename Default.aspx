<%@ Page Title="Dropbox CL493ILPE Homework" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CloudPhotoUpload._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        welcome
    </h2>
    <p>
        Files:</p>
    <asp:ListBox ID="FileList" runat="server" Width="418px"></asp:ListBox>
    <p>
        Type Folder Path</p>
    <asp:TextBox ID="FolderPath" runat="server" AutoPostBack="True" OnTextChanged="FolderPath_TextChanged"></asp:TextBox>
    <p>
        AlbumName</p>
    <asp:TextBox ID="AlbumName" runat="server" Width="241px"></asp:TextBox>
    <asp:Button ID="UploadPhotos" runat="server" OnClick="UploadPhotos_Click" Text="Upload Photos" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please type Album name"
        ControlToValidate="AlbumName"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
