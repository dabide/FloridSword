using System.Reflection;
using Funq;
using ServiceStack;

namespace FloridSword.SystemService
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("FloridSword System API", typeof(AppHost).GetAssembly())
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}
