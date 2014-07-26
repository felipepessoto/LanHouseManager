<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LanManager.BLL.ClientSession>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ver Sess�o
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Ver Sess�o</h2>
    <p>
        <%
            if (Model.ClientApplicationLog.Count == 0)
            {
                Response.Write("O cliente n�o acessou nenhum aplicativo");
            }
            else
            {
                foreach (LanManager.BLL.ClientApplicationLog appLog in Model.ClientApplicationLog)
                {
                    Response.Write(appLog.Application.Name + " das " + appLog.StartDate + " at� " + appLog.EndDate + "<br />");
                }
            }
        %>
    </p>
</asp:Content>
