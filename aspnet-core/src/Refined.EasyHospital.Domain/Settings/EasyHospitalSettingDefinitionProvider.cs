using Volo.Abp.Settings;

namespace Refined.EasyHospital.Settings;

public class EasyHospitalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EasyHospitalSettings.MySetting1));
    }
}
