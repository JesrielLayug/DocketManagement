using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class ViewDocketDialogBase : ComponentBase
    {
        [Parameter] public DTODocket Docket { get; set; }
        public static char GetFirstLetterOFUser(string name)
        {
            string fiteredName = new string(name
                .Where(char.IsLetter)
                .ToArray());

            return char.ToUpper(fiteredName[0]);
        }

    }
}
