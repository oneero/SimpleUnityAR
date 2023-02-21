namespace Oneeronaut
{
    /**
     * Convenience class for constructing the AppController and AppModel instances.
     */
    public class AppFactory
    {
        private IAppController controller;

        public AppFactory(IAppView view)
        {
            // Create model with PositionObject as the data type
            IAppModel model = new AppModel<PositionObject>();

            // Create controller
            controller = new AppController();

            // Connect controller with model
            controller.AttachModel(model);
            
            // Connect controller with view
            controller.AttachView(view);
        }
    }
}
