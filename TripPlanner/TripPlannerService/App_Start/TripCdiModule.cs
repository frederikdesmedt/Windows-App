using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;

namespace TripPlannerService.App_Start
{
    public class TripCdiModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<TripContext>().ToSelf().InSingletonScope();
        }
    }
}