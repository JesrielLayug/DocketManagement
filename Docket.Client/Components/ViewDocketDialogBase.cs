using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class ViewDocketDialogBase : ComponentBase
    {
        [Inject] IDialogService DialogService { get; set; }

    }
}
