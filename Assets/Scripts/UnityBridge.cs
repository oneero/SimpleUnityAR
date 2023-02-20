using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Oneeronaut
{
    public class GUIActionEventArgs : EventArgs
    {
    }

    /**
     * The UnityBridge class implements the IAppEngineBridge interface and serves as the link
     * between the application logic and Unity Engine. It is the only MonoBehaviour in the project.
     * The Awake method also serves as the entry point of the Unity project.
     */
    public class UnityBridge : MonoBehaviour, IAppEngineBridge
    {
        // Events
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<GUIActionEventArgs> OnPlaceObjectGUIActivated;
        
        // Unity references
        private Camera unityCamera;
        private Vector3 unityCameraWorldPosition;
        private Button unityGUIButton;

        // App reference
        private IAppController appController;
        
        void Awake()
        {
            // Find Unity references
            unityCamera = Camera.main;
            if (!unityCamera)
            {
                Debug.LogError("Could not find Unity Camera.");
                Destroy(this);
                return;
            }
            
            

            // Create and initialize app with a factory object.
            // We pass a reference to this object for subscribing to events.
            AppFactory appFactory = new AppFactory(this);
            appController = appFactory.Controller;
        }

        void Update()
        {
            // Check if the Unity Camera has moved and only react if it has
            if (unityCamera.transform.position != unityCameraWorldPosition)
            {
                // Save current world position
                unityCameraWorldPosition = unityCamera.transform.position;
                
                // Build the EventArgs object and fire the event
                PositionChangedEventArgs args = new PositionChangedEventArgs();
                
                // App logic side uses System.Numerics.Vector3 so we need to convert the type here.
                // The conversion method is implemented in TypeExtensions.cs
                args.NewPosition = unityCameraWorldPosition.ToSystem();
                
                OnCameraPositionChanged?.Invoke(this, args);
            }
        }
        
        public void HandlePlacedObjectLabelUpdate(object sender, GUILabelUpdateEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}

