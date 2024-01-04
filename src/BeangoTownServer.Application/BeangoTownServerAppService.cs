using Volo.Abp.Application.Services;

namespace BeangoTownServer;

/* Inherit your application services from this class.
 */
public abstract class BeangoTownServerAppService : ApplicationService
{
    protected BeangoTownServerAppService()
    {
        LocalizationResource = typeof(BeangoTownServerResource);
    }
}