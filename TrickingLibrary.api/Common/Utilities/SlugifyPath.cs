using Common.Utilities;

namespace TrickingLibrary.api.Common.Utilities;

public partial class SlugifyPath : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value == null
            ? null
            : Formatter.FormatePascalToKebab(value.ToString()!).ToLowerInvariant();
    }
}