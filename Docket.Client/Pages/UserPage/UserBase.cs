using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Pages.UserPage
{
    public class UserBase : ComponentBase
    {
        [Inject] IUserService UserService { get; set; }
        public IEnumerable<DTOUser> Users;

        protected async override Task OnInitializedAsync()
        {
            Users = await UserService.GetAll();
        }
    }
}
