using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace Net6Features.Client.Components
{
    public partial class TableRow<TDataTableItem>
    {
        [Inject] public IDialogService? DialogService { get; set; }
        [CascadingParameter] public TDataTableItem Item { get; set; }
        [Parameter] public RenderFragment<TDataTableItem> ChildContent { get; set; }

        private void ShowQrCode()
        {
            if (DialogService != null)
            {
                var data = JsonSerializer.Serialize(Item);
                var parameters = new DialogParameters();
                parameters.Add("Data", data);
                DialogService.Show<QrCode>(String.Empty, parameters, new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true });
            }
        }
    }
}