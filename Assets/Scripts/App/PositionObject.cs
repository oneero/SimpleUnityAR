using System;
using SVector3 = System.Numerics.Vector3;

namespace Oneeronaut
{
    /**
     * The IPositioned interface defines requirements for position data types
     * to be used with the AppModel class.
     */
    public interface IPositioned
    {
        event EventHandler<PositionChangedEventArgs> OnPositionChanged;
        SVector3 Position { get; set; }
        float DistanceTo(SVector3 target);
    }
    
    /**
     * A simple class implementing the necessary features for use in AppModel.
     * It holds position data, fires events on position changes and provides
     * a method for calculating distances.
     */
    public class PositionObject : IPositioned
    {
        // Events
        public event EventHandler<PositionChangedEventArgs> OnPositionChanged;

        // Data
        public SVector3 Position
        {
            get => position;
            set
            {
                // Fire events if the position changes
                
                if (position == value) return;
                position = value;
                    
                PositionChangedEventArgs args = new PositionChangedEventArgs();
                args.NewPosition = position;
                OnPositionChanged?.Invoke(this, args);
            }
        }

        private SVector3 position;

        /**
         * The generic AppModel class has the new() type constraint, so we need to
         * provide a constructor with zero arguments.
         */
        public PositionObject()
        {
            Position = SVector3.Zero;
        }

        /**
         * Simple distance calculation implementation.
         */
        public float DistanceTo(SVector3 target)
        {
            return SVector3.Distance(position, target);
        }
    }

}