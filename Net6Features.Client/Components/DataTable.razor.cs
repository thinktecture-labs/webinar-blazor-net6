using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using MudBlazor.Services;
using System.Text.Json;

namespace Net6Features.Client.Components
{
    public partial class DataTable<TDataTableItem> : IAsyncDisposable
        where TDataTableItem : class
    {
        [Inject] public IBreakpointService? BreakpointService { get; set; }
        [Inject] public IDialogService? DialogService { get; set; }
        [Inject] public IServiceProvider? ServiceProvider { get; set; }

        [Parameter, EditorRequired] public List<TDataTableItem> Data { get; set; }
        [Parameter, EditorRequired] public string Row { get; set; } = String.Empty;
        [Parameter, EditorRequired] public bool IsLoading { get; set; } = false;
        [Parameter, EditorRequired] public Func<TDataTableItem, string>? ValueExpression { get; set; }
        [Parameter] public bool UseGrpc { get; set; }
        [Parameter] public EventCallback<bool> UseGrpcChanged { get; set; }
        [Parameter] public string SearchTerm { get; set; } = string.Empty;
        [Parameter] public EventCallback<string> SearchTermChanged { get; set; }
        [Parameter] public string Title { get; set; } = "Data";

        private bool IsPreRendering()
        {
            var httpContextAccessor = ServiceProvider?.GetService<IHttpContextAccessor>();
            return httpContextAccessor != null ? !httpContextAccessor.HttpContext.Response.HasStarted : false;
        }

        private Breakpoint _breakpoint;
        private Guid _subscriptionId;
        private Typo _headerTypo = Typo.h6;
        private Size _iconSize = Size.Medium;

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (BreakpointService is not null)
            {
                _breakpoint = await BreakpointService.GetBreakpoint();

                _headerTypo = _breakpoint <= Breakpoint.Md ? Typo.body1 : Typo.h6;
                _iconSize = _breakpoint <= Breakpoint.Md ? Size.Small : Size.Medium;

                var result = await BreakpointService.Subscribe(breakpoint =>
                {
                    _breakpoint = breakpoint;
                    _headerTypo = _breakpoint <= Breakpoint.Md ? Typo.body1 : Typo.h6;
                });
                _subscriptionId = result.SubscriptionId;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnUseGrpcChanged(bool useGrpc)
        {
            UseGrpc = useGrpc;
            await UseGrpcChanged.InvokeAsync(UseGrpc);
        }

        private async Task OnSearchTermChanged(string searchTerm)
        {
            SearchTerm = searchTerm;
            await SearchTermChanged.InvokeAsync(searchTerm);
        }

        private void ShowQrCode(TDataTableItem item)
        {
            if (DialogService != null)
            {
                var data = JsonSerializer.Serialize(item);
                var parameters = new DialogParameters();
                parameters.Add("Data", data);
                DialogService.Show<QrCode>(String.Empty, parameters, new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true });
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (BreakpointService != null)
            {
                await BreakpointService.Unsubscribe(_subscriptionId);
            }
        }
    }
}