using Microsoft.AspNetCore.Components;

namespace ChessGameWebApp.Client.Pages
{
    public class AuthorizationModel : ComponentBase
    {
        [Parameter]
        public string Type { get; set; }

        protected override void OnInitialized()
        {
            Type = Type ?? "auth";
        }
    }
}
