using Volo.Abp.AspNetCore.Mvc;

namespace BeangoTownServer;

public abstract class BeangoTownServerController : AbpControllerBase
{
    protected BeangoTownServerController()
    {
        LocalizationResource = typeof(BeangoTownServerResource);
    }
}