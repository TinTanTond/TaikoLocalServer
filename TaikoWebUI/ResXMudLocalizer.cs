using TaikoWebUI.Localization;
using Microsoft.Extensions.Localization;

namespace TaikoWebUI;


internal class ResXMudLocalizer(IStringLocalizer<LocalizationResource> localizer) : MudLocalizer
{
    private IStringLocalizer localization = localizer;

    public override LocalizedString this[string key] => localization[key];
}