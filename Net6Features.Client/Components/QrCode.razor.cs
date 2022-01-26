using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Runtime.InteropServices;

namespace Net6Features.Client.Components
{
    public partial class QrCode
    {
        [Inject] public IJSRuntime JS { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Data { get; set; }


        [DllImport("librustqr")]
        static extern void generate_qr_code(string contents, byte[] buffer, int bufferLength, out int width, out int height);

        private IJSObjectReference _module;
        private ElementReference canvasElem;
        private byte[] buffer = new byte[256 * 256 * 4];
        int width = 100;
        int height = 100;
        private string currentData = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!String.IsNullOrWhiteSpace(Data) && currentData != Data)
                {
                    await InvokeAsync(() =>
                    {
                        generate_qr_code(Data, buffer, buffer.Length, out width, out height);
                        StateHasChanged();
                    });
                    currentData = Data;
                }
                await Task.Delay(250);

                if (_module == null)
                {
                    _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/QrCode.razor.js");
                }

                await _module.InvokeVoidAsync("bufferToCanvas",
                    canvasElem, buffer, width, height);
            }
            
        }
        private void Close() => MudDialog.Close(DialogResult.Ok(true));
    }
}