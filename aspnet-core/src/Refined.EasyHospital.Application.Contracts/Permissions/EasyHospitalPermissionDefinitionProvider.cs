using Refined.EasyHospital.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Refined.EasyHospital.Permissions;

public class EasyHospitalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EasyHospitalPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EasyHospitalPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EasyHospitalResource>(name);
    }
}
