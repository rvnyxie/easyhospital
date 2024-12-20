using System;
using System.Collections.Generic;
using System.Text;
using Refined.EasyHospital.Localization;
using Volo.Abp.Application.Services;

namespace Refined.EasyHospital;

/* Inherit your application services from this class.
 */
public abstract class EasyHospitalAppService : ApplicationService
{
    protected EasyHospitalAppService()
    {
        LocalizationResource = typeof(EasyHospitalResource);
    }
}
