using Microsoft.AspNetCore.Components;

namespace Docket.Client.Components
{
    public class RateDocketDialogBase : ComponentBase
    {
        [Parameter] public int Rating { get; set; }

        public void Submit()
        {
            throw new NotImplementedException();
        }
    }
}
