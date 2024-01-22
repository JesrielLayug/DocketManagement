using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class DocketCardBase : ComponentBase
    {
        [Parameter] public IEnumerable<DTODocket> Dockets { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        public static char GetFirstLetterOFUser(string name)
        {
            string fiteredName = new string(name
                .Where(char.IsLetter)
                .ToArray());

            return char.ToUpper(fiteredName[0]);
        }

        public void AddRating()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };

            DialogService.Show<RateDocketDialog>("RateDocket", options);
        }
    }
}
