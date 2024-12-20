using Refined.EasyHospital.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Refined.EasyHospital.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EasyHospitalController : AbpControllerBase
{
    protected EasyHospitalController()
    {
        LocalizationResource = typeof(EasyHospitalResource);
    }
}
