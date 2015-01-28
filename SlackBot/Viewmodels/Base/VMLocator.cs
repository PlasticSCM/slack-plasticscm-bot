using Slack.API.Services;
using SlackBot.Portable.Services;

using Autofac;

namespace SlackBot.Viewmodels.Base
{

    public class VMLocator
    {
        IContainer container;

        public VMLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<PlasticCMDService>().As<IPlasticCMDService>().SingleInstance();
            builder.RegisterType<SlackRTMService>().As<ISlackRTMService>().SingleInstance();
            builder.RegisterType<MainViewModel>();

            this.container = builder.Build();
        }

        public MainViewModel MainVM
        {
            get { return this.container.Resolve<MainViewModel>(); }
        }
    }
}
