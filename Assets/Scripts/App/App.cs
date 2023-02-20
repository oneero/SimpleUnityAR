using System;
using System.Collections;
using System.Collections.Generic;

namespace Oneeronaut
{
    /**
     * IAppEngineBridge interface can be implemented to run the application logic in a different engine.
     */
    public interface IAppEngineBridge
    {
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<GUIActionEventArgs> OnPlaceObjectGUIActivated;
        public void HandlePlacedObjectLabelUpdate(object sender, GUILabelUpdateEventArgs eventArgs);
    }

    public class AppFactory
    {
        private IAppModel model;
        private IAppView view;
        public IAppController Controller { get; }

        public AppFactory(IAppEngineBridge engineBridge)
        {
            // Create model
            model = new AppModel();

            // Create view
            view = new AppView();
            
            // Create controller
            Controller = new AppController(model, view);
            Controller.SubscribeEngineEvents(engineBridge);

        }
    }
}
