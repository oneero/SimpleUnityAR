using System;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    public class DistanceToUserUpdateEventArgs : EventArgs
    {
        public float Distance;
    }
    
    public interface IAppModel
    {
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<PositionChangedEventArgs> OnObjectPositionUpdate;
        public void UpdateUserPosition(SVector3 position);
        public void PlaceObject(SVector3 position);
    }
    
    /**
     * The AppModel class servers as a container for the application data.
     * It implements the IAppModel interface and uses generics for the data types.
     * This allows easier alternative implementations for the positional data class.
     */
    public class AppModel<TP> : IAppModel
        where TP : class, IPositioned, new()
    {
        public event EventHandler<DistanceToUserUpdateEventArgs> OnDistanceToUserUpdate;
        public event EventHandler<PositionChangedEventArgs> OnObjectPositionUpdate;
        private TP User { get; set; }
        private TP PlacedObject { get; set; }

        public AppModel()
        {
            User = new TP { Position = SVector3.Zero };
            User.OnPositionChanged += HandleUserPositionChanged;
            PlacedObject = null;
        }

        public void PlaceObject(SVector3 position)
        {
            PlacedObject = new TP { Position = SVector3.Zero };
            PlacedObject.OnPositionChanged += HandleObjectPositionChanged;
        }

        // Handle position change events from user
        private void HandleUserPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            // We can ignore user position changes if no object has been placed
            if (PlacedObject == null) return;
            
            DistanceToUserUpdateEventArgs args = new DistanceToUserUpdateEventArgs();
            args.Distance = User.DistanceTo(eventArgs.NewPosition);
            OnDistanceToUserUpdate?.Invoke(this, args);
        }
        
        // Handle position change of the placed object
        private void HandleObjectPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            PositionChangedEventArgs args = new PositionChangedEventArgs();
            args.NewPosition = eventArgs.NewPosition;
            OnObjectPositionUpdate?.Invoke(this, args);
        }

        // Handle camera position change events from controller
        public void UpdateUserPosition(SVector3 position)
        {
            User.Position = position;
        }
    }
    
}