<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LanManager.BLL.Client>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Editar Horários
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Editar Horários</h2>
    

Horários permitidos:<br /><br />
<%
    if (!Model.CanAccessAnyTime)
    {
        foreach (LanManager.BLL.PeriodAllowed periodAllowed in Model.PeriodAllowed.ToList())
        {
            string periodo = periodAllowed.DayOfWeek.HasValue ? System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[periodAllowed.DayOfWeek.Value] : "Todos os dias";
            periodo += " das " + periodAllowed.StartHour + "h até " + periodAllowed.EndHour + "h - Remover";

            Response.Write(Html.ActionLink(periodo, "RemoverHorario", new { id = Model.Id.ToString(), periodAllowedId = periodAllowed.Id.ToString() }) + "<br />");
        }
    
%>


<br /><br />

<%using (Html.BeginForm("AdicionarHorario", "Home", new { id = Model.Id.ToString() }))
  {
      var diasSemana =
          Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(
              x =>
              new SelectListItem
                  {
                      Value = ((int)x).ToString(),
                      Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)x]
                  }).ToList();
      diasSemana.Insert(0, new SelectListItem { Value = "", Text = "Todos", Selected = true });
      %>
 
 <%=Html.DropDownList("dayOfWeek", diasSemana)%>
 das <%=Html.TextBox("startHour", null, new { maxlength = 2 })%> até <%=Html.TextBox("endHour", null, new { maxlength = 2 })%> <%=Html.ValidationMessage("horarios")%>
 <br /><br /><input type="submit" value="Adicionar" /><br />

<%
    }
    }%>
 

<%using (Html.BeginForm())
 {%>
Pode acessar em qualquer horário: <%=Html.CheckBox("canAccessAnyTime", Model.CanAccessAnyTime) %>

<input type="submit" value="Salvar" />

<%
 }%>

    <p>
        <%=Html.ActionLink("Voltar", "Index") %>
    </p>
<script type="text/javascript">
    $('#startHour,#endHour').keypress(function(e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
</script>
</asp:Content>
