using System;
using System.Collections;
using System.Collections.Generic;
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
                OnPositionChanged?.Invoke(this, args);
            }
        }

        private SVector3 position;
    }

}