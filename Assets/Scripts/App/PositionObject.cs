using System;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    /**
     * PositionChangedEventArgs is used when firing events from both Unity and app logic side.
     */
    public class PositionChangedEventArgs : EventArgs
    {
        public SVector3 NewPosition;
    }
    
    public interface IPositioned
    {
        event EventHandler<PositionChangedEventArgs> OnPositionChanged;
        SVector3 Position { get; set; }
        float DistanceTo(SVector3 target);
    }
    
    /**
     * Simple class that automatically fires events when it's position value is changed.
     */
    public class PositionObject : IPositioned
    {
        public event EventHandler<PositionChangedEventArgs> OnPositionChanged;

        public SVector3 Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                    
                PositionChangedEventArgs args = new PositionChangedEventArgs();
                args.NewPosition = position;
                OnPositionChanged?.Invoke(this, args);
            }
        }

        private SVector3 position;

        public PositionObject()
        {
            Position = SVector3.Zero;
        }

        public PositionObject(SVector3 position)
        {
            Position = position;
        }

        public float DistanceTo(SVector3 target)
        {
            return SVector3.Distance(position, target);
        }
    }

}