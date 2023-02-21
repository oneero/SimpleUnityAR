using System;
using System.Collections.Generic;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    /**
     * Interface for App Models.
     */
    public interface IAppModel
    {
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<ObjectAddedEventArgs> OnObjectAdded;
        public void UpdateUserPosition(SVector3 position);
        public void PlaceObject(SVector3 position);
    }
    
    /**
     * The AppModel class servers as a container for the application data.
     * It implements the IAppModel interface and uses generics for the position data types.
     * The interface and generics enable easier alternative implementations.
     */
    public class AppModel<T> : IAppModel
        where T : class, IPositioned, new()
    {
        // Events
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<ObjectAddedEventArgs> OnObjectAdded;
        
        // Data
        private T User { get; set; }
        private List<T> PlacedObjects { get; set; }

        public AppModel()
        {
            // Create User and subscribe to position change events
            User = new T { Position = SVector3.Zero };
            User.OnPositionChanged += HandleUserPositionChanged;
            
            // Initialize place objects list
            PlacedObjects = new List<T>();
        }

        /**
         * Create new placed object and report back with an event.
         * This is called by the Controller.
         */
        public void PlaceObject(SVector3 position)
        {
            T placedObject = new T { Position = position };
            PlacedObjects.Add(placedObject);
            
            ObjectAddedEventArgs args = new ObjectAddedEventArgs();
            args.Position = position;
            OnObjectAdded?.Invoke(this, args);
        }
        
        /*
         * The Controller will call this to update the User position.
         */
        public void UpdateUserPosition(SVector3 position)
        {
            User.Position = position;
        }
        
        /*
         * The User position data class will report position changes with events and
         * this method will handle those. It will iterate all placed objects and fire
         * events with calculated distances and indices as arguments.
         */
        private void HandleUserPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            for (int i = 0; i < PlacedObjects.Count; i++)
            {
                DistanceToUserUpdateEventArgs args = new DistanceToUserUpdateEventArgs();
                args.Distance = PlacedObjects[i].DistanceTo(eventArgs.NewPosition);
                args.Index = i;
                OnDistanceToUserUpdate?.Invoke(this, args);
            }
        }
    }
    
}