using System;
using UnityEngine;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    public interface IAppController
    {
        public void SubscribeToViewEvents();
        public void SubscribeToModelEvents();
    }
    
    public class AppController : IAppController
    {
        private IAppModel model;
        private IAppView view;
        
        public AppController(IAppModel model, IAppView view)
        {
            this.model = model;
            this.view = view;
        }

        /**
         * The controller is responsible for passing events between the view and the model.
         * In this simple example, there is not much else going on. In a more substantial
         * application, this would be the place to implement different checks for the events
         * to determine if they should be passed onwards or if they should be routed differently.
         */
        public void SubscribeToViewEvents()
        {
            view.OnCameraPositionChanged += HandleCameraPositionChanged;
            view.OnRaycastHit += HandleRaycastHit;
        }

        private void HandleCameraPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            model.UpdateUserPosition(eventArgs.NewPosition);
        }

        private void HandleRaycastHit(object sender, RaycastHitEventArgs eventArgs)
        {
            Debug.Log("C: HandleRaycastHit");
            model.PlaceObject(eventArgs.PlacementPosition);
        }

        public void SubscribeToModelEvents()
        {
            model.OnDistanceToUserUpdate += HandleDistanceToUserUpdate;
            model.OnObjectAdded += HandleObjectAdded;
        }
        
        private void HandleDistanceToUserUpdate(object sender, DistanceToUserUpdateEventArgs eventArgs)
        {
            Debug.Log($"C: HandleDistanceToUserUpdate; {eventArgs.Index}, {eventArgs.Distance}");
            view.UpdateDistanceGUI(eventArgs.Index, eventArgs.Distance);
        }
        
        private void HandleObjectAdded(object sender, ObjectAddedEventArgs eventArgs)
        {
            Debug.Log("C: HandleObjectAdded");
            view.PlaceVisObject(eventArgs.Position);
        }
    }
    
}
