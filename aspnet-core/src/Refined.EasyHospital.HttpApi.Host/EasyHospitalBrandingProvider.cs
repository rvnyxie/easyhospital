using Microsoft.Extensions.Localization;
using Refined.EasyHospital.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Refined.EasyHospital;

[Dependency(ReplaceServices = true)]
public class EasyHospitalBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EasyHospitalResource> _localizer;

    public EasyHospitalBrandingProvider(IStringLocalizer<EasyHospitalResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
