using ChessGameWebApp.Client.Pages;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace ChessGameWebApp.Client.Components
{
    public class AuthenticationComponentModel : ComponentBase
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "4 - 20 character")]
        public string Password { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "4 - 20 character")]
        public string Login { get; set; }
    }
}
