using System;
using System.Collections.Generic;
using UnityEngine;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    public class DistanceToUserUpdateEventArgs : EventArgs
    {
        public int Index;
        public float Distance;
    }

    public class ObjectAddedEventArgs : EventArgs
    {
        public SVector3 Position;
    }
    
    public interface IAppModel
    {
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<ObjectAddedEventArgs> OnObjectAdded;
        public void UpdateUserPosition(SVector3 position);
        public void PlaceObject(SVector3 position);
    }
    
    /**
     * The AppModel class servers as a container for the application data.
     * It implements the IAppModel interface and uses generics for the data types.
     * This allows easier alternative implementations for the positional data class.
     */
    public class AppModel<T> : IAppModel
        where T : class, IPositioned, new()
    {
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<ObjectAddedEventArgs> OnObjectAdded;
        private T User { get; set; }
        private List<T> PlacedObjects { get; set; }

        public AppModel()
        {
            User = new T { Position = SVector3.Zero };
            User.OnPositionChanged += HandleUserPositionChanged;
            PlacedObjects = new List<T>();
        }

        public void PlaceObject(SVector3 position)
        {
            Debug.Log($"M: PlaceObject; {PlacedObjects.Count}");
            
            T placedObject = new T { Position = position };
            PlacedObjects.Add(placedObject);
            
            ObjectAddedEventArgs args = new ObjectAddedEventArgs();
            args.Position = position;
            OnObjectAdded?.Invoke(this, args);
        }

        // Handle camera position change events from controller
        public void UpdateUserPosition(SVector3 position)
        {
            // Update the user data
            User.Position = position;
        }
        
        // Handle position change events from user data
        private void HandleUserPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            // todo: iterate placed objects
            for (int i = 0; i < PlacedObjects.Count; i++)
            {
                DistanceToUserUpdateEventArgs args = new DistanceToUserUpdateEventArgs();
                args.Distance = PlacedObjects[i].DistanceTo(eventArgs.NewPosition);
                args.Index = i;
                Debug.Log($"M: User position change; {args.Index}, {args.Distance}");
                OnDistanceToUserUpdate?.Invoke(this, args);
            }
        }
    }
    
}