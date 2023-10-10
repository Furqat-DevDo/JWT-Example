using JWT.Enums;
using Microsoft.AspNetCore.Authorization;

namespace JWT.Managers.Helpers;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
    }
}
