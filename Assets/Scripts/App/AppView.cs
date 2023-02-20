using System;
using System.Collections;
using System.Collections.Generic;

namespace Oneeronaut
{
    public class GUILabelUpdateEventArgs : EventArgs
    {
        
    }
    
    public interface IAppView
    {
        public void HandleUnityCameraPositionChanged(object sender, PositionChangedEventArgs eventArgs);
        public void HandleUnityPlaceObjectGUIActivated(object sender, GUIActionEventArgs eventArgs);
        public event EventHandler<GUILabelUpdateEventArgs> OnGUILabelUpdated;
    }
    
    /**
     * The AppView class contains event handlers for reacting to UnityBridge events and 
     * AppView subscribes to OnCameraPositionChanged and OnPlaceObjectGUIActivated events from UnityBridge.
     * AppView emits the OnLabelUpdated event.
     */
    public class AppView : IAppView
    {
        public event EventHandler<GUILabelUpdateEventArgs> OnGUILabelUpdated; 
        
        public void HandleUnityCameraPositionChanged(object sender, PositionChangedEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }

        public void HandleUnityPlaceObjectGUIActivated(object sender, GUIActionEventArgs eventArgs)
        {
            throw new System.NotImplementedException();
        }
    }
    
}