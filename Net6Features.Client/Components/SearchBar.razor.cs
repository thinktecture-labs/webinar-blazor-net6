using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Net6Features.Client.Components
{
    public partial class SearchBar : IDisposable
    {
        [Inject] public IJSRuntime JS { get; set; }
        [Parameter] public string SearchTerm { get; set; } = string.Empty;
        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public EventCallback<string> SearchTermChanged { get; set; }

        private IJSObjectReference _module;
        private ElementReference _searchBarElement;
        private DotNetObjectReference<SearchBar> _selfReference;
        private int _valueHashCode;

        [JSInvokable]
        public void HandleOnInput(string value)
        {
            Console.WriteLine($"TextChanged {SearchTerm}. JS Value {value}");
            if (SearchTerm != value)
            {
                SearchTermChanged.InvokeAsync(value);
                StateHasChanged();
            }
        }

        protected override bool ShouldRender()
        {
            var lastHashCode = _valueHashCode;
            _valueHashCode = SearchTerm?.GetHashCode() ?? 0;
            return _valueHashCode != lastHashCode;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Console.WriteLine("Register debounce JS Event");
                _selfReference = DotNetObjectReference.Create(this);
                var minInterval = 500; // Only notify every 500 ms
                if (_module == null)
                {
                    _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/SearchBar.js");
                }
                await _module.InvokeVoidAsync("onDebounceInput",
                    _searchBarElement, _selfReference, minInterval);
            }
        }

        public async Task OnSearchTermChanged(string searchTerm)
        {
            await SearchTermChanged.InvokeAsync(searchTerm);
        }

        public void Dispose() => _selfReference?.Dispose();
    }
}