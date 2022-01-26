using Microsoft.AspNetCore.Components;

namespace Net6Features.Client.Pages
{
    public partial class Index
    {
        [Inject] public NavigationManager? NavigationManager { get; set; }

        private string _pageTitle = ".NET 6";
        private string _description = "Neue Features coole in .NET 6";
        private string _dataSource = string.Empty;
        private string _searchTerm = string.Empty;
        
        public void NavigateToDataSource()
        {
            if (NavigationManager is not null && !String.IsNullOrWhiteSpace(_dataSource))
            {
                var url = string.IsNullOrWhiteSpace(_searchTerm) 
                    ? $"{_dataSource}" 
                    : $"{_dataSource}?searchTerm={_searchTerm}";
                NavigationManager.NavigateTo(url);
            }
        }

        private void OnSelectedOptionChanged(string option)
        {
            _dataSource = option;
        }
    }
}