using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    /**
     * Simple interface for the Controller.
     */
    public interface IAppController
    {
        public void AttachModel(IAppModel model);
        public void AttachView(IAppView view);
    }
    
    /**
     * The controller is responsible for piping communication between the view and the model.
     * 
     * In this simple example app, there is not much going on here. In a more substantial
     * application, this would be the place to implement different checks for the events
     * to determine if they should be passed onwards or if they should be routed differently.
     *
     * Note that the Controller subscribes to events from the view and model, but directly calls
     * methods in both.
     */
    public class AppController : IAppController
    {
        private IAppModel model;
        private IAppView view;

        public void AttachModel(IAppModel modelToAttach)
        {
            model = modelToAttach;
            SubscribeToModelEvents();
        }

        public void AttachView(IAppView viewToAttach)
        {
            view = viewToAttach;
            SubscribeToViewEvents();
        }

        /**
         * Subscribe to events from the view.
         */
        public void SubscribeToViewEvents()
        {
            view.OnCameraPositionChanged += HandleCameraPositionChanged;
            view.OnUserPlacesObject += HandleUserPlacesObject;
        }

        /**
         * Call the appropriate model methods when receiving events form the view.
         */
        private void HandleCameraPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            model.UpdateUserPosition(eventArgs.NewPosition);
        }

        private void HandleUserPlacesObject(object sender, UserPlacesObjectEventArgs eventArgs)
        {
            model.PlaceObject(eventArgs.PlacementPosition);
        }

        /**
         * Subscribe to model events.
         */
        public void SubscribeToModelEvents()
        {
            model.OnDistanceToUserUpdate += HandleDistanceToUserUpdate;
            model.OnObjectAdded += HandleObjectAdded;
        }
        
        /**
         * Call the appropriate view methods when receiving events form the model.
         */
        private void HandleDistanceToUserUpdate(object sender, DistanceToUserUpdateEventArgs eventArgs)
        {
            view.UpdateDistanceGUI(eventArgs.Index, eventArgs.Distance);
        }
        
        private void HandleObjectAdded(object sender, ObjectAddedEventArgs eventArgs)
        {
            view.CreateEngineObject(eventArgs.Position);
        }
    }
    
}
