using Docket.Shared;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Components
{
    public class EditableDocketCardBase : ComponentBase
    {
        [Parameter] public IEnumerable<DTODocket> Dockets { get; set; }
    }
}
