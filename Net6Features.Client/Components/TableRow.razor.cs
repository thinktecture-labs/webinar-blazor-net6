using Microsoft.AspNetCore.Components;

namespace Net6Features.Client.Components
{
    public partial class TableRow<TItem>
    {
        [Parameter, EditorRequired] public TItem? Item { get; set; }
        [Parameter, EditorRequired] public string SearchTerm { get; set; } = String.Empty;
        [Parameter, EditorRequired] public string Row { get; set; } = String.Empty;
        [Parameter, EditorRequired] public Func<TItem, string>? ValueExpression { get; set; }
    }
}