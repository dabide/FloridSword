using Autofac;
using FloridSword.Common.Extensions;
using ServiceStack;

namespace FloridSword.SystemService.Setup
{
    public class SystemServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(SystemServiceModule).GetAssembly())
                .AsDefaultInterface();
        }
    }
}
