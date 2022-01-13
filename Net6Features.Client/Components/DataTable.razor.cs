using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;

namespace Net6Features.Client.Components
{
    public partial class DataTable<TItem> : IAsyncDisposable
        where TItem : class
    {
        [Inject] public IBreakpointService? BreakpointService { get; set; }

        [Parameter, EditorRequired] public List<TItem> Data { get; set; } = new List<TItem>();
        [Parameter, EditorRequired] public string Row { get; set; } = String.Empty;
        [Parameter, EditorRequired] public bool IsLoading { get; set; } = false;
        [Parameter, EditorRequired] public Func<TItem, string>? ValueExpression { get; set; }
        [Parameter, EditorRequired] public Func<TItem, string, bool>? SearchExpression { get; set; }
        [Parameter] public EventCallback<bool> UseGrpcChanged { get; set; }
        [Parameter] public string Title { get; set; } = "Data";
        [Parameter] public string SearchTerm { get; set; } = string.Empty;

        private string _searchTerm = string.Empty;
        private bool _useGrpc = false;
        private List<TItem> FilteredData()
        {
            Console.WriteLine("Load data");
            return (String.IsNullOrWhiteSpace(_searchTerm) || SearchExpression == null) ? Data : Data.Where((d) => SearchExpression(d, _searchTerm)).ToList();
        }

        private Breakpoint _breakpoint;
        private Guid _subscriptionId;

        protected override async Task OnInitializedAsync()
        {
            _searchTerm = SearchTerm;
            if (BreakpointService is not null)
            {
                _breakpoint = await BreakpointService.GetBreakpoint();
                var result = await BreakpointService.Subscribe(breakpoint => _breakpoint = breakpoint);
                _subscriptionId = result.SubscriptionId;
            }
            await base.OnInitializedAsync();
        }

        private async Task OnUseGrpcChanged(bool useGrpc)
        {
            _useGrpc = useGrpc;
            await UseGrpcChanged.InvokeAsync(_useGrpc);
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