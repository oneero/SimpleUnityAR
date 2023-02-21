using System;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    // Used by IAppModel to report updates to placed object distances.
    public class DistanceToUserUpdateEventArgs : EventArgs
    {
        public int Index;
        public float Distance;
    }

    // Used by IAppModel to report object added events.
    public class ObjectAddedEventArgs : EventArgs
    {
        public SVector3 Position;
    }
    
    // Used by IAppView to report object placement.
    public class UserPlacesObjectEventArgs : EventArgs
    {
        public SVector3 PlacementPosition;
    }
    
    // Used by IPositioned as well as IAppView to report position changes.
    public class PositionChangedEventArgs : EventArgs
    {
        public SVector3 NewPosition;
    }
}
