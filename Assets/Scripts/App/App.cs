namespace Oneeronaut
{
    /**
     * Convenience class for constructing the AppController and AppModel instances.
     */
    public class AppFactory
    {
        public IAppController Controller { get; }

        public AppFactory(IAppView view)
        {
            // Create model
            IAppModel model = new AppModel<PositionObject>();

            // Create controller
            Controller = new AppController(model, view);

            // Connect controller with model
            Controller.SubscribeToModelEvents();
            
            // Connect controller with view
            Controller.SubscribeToViewEvents();
        }
    }
}
