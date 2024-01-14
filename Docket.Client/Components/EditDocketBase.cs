using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Security.Claims;

namespace Docket.Client.Components
{
    public class EditDocketBase : ComponentBase
    {
        [Inject] private IDialogService dialogService { get; set; }
        [CascadingParameter] MudDialogInstance dialog { get; set; }
        [Inject] public IDocketService docketService { get; set; }


        public DTODocket Docket = new DTODocket();
        public bool isLoading = false;

        public async Task SaveOnClick(EditContext context)
        {
            isLoading = true;

            var response = await docketService.Add(new DTODocket
            {
                Title = Docket.Title,
                Body = Docket.Body,
                IsPublic = Docket.IsPublic,
                IsHidden = false,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            });

        }
    }
}
