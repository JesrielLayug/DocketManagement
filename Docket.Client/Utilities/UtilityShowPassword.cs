using MudBlazor;

namespace Docket.Client.Utilities
{
    public class UtilityShowPassword
    {
        public bool IsVisible { get; private set; } = false;
        public InputType InputType { get; private set; } = InputType.Password;
        public string Icon { get; private set; } = Icons.Material.Filled.VisibilityOff;

        public void Toggle()
        {
            IsVisible = !IsVisible;
            InputType = IsVisible ? InputType.Text : InputType.Password;
            Icon = IsVisible ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
        }
    }
}
