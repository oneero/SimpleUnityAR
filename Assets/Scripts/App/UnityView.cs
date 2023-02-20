using System;
using System.Collections.Generic;
using SVector3 = System.Numerics.Vector3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Oneeronaut
{
    /**
     * IAppView interface can be implemented to drive the application logic in a different engine.
     */
    public interface IAppView
    {
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<RaycastHitEventArgs> OnRaycastHit;
        public void UpdateDistanceGUI(float distance);
        public void PlaceVisObject(SVector3 position);
    }

    /**
     * The UnityView class implements the IAppView interface and serves as a link between the user and the
     * AppController. It is responsible for firing events on user interaction as well as presenting changes
     * coming from the AppModel (via the AppController).
     * 
     * It is the only MonoBehaviour in the project. The Awake method serves as the entry point of the
     * Unity project and thus it is also responsible for handling setting up the app.
     */
    public class UnityView : MonoBehaviour, IAppView
    {
        // Events
        
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<RaycastHitEventArgs> OnRaycastHit;
        
        // Unity references
        
        [SerializeField]                 
        private Button placeObjectButton;

        [SerializeField]
        private Text userDistanceLabel;

        [SerializeField]
        private GameObject visObjectPrefab;
        
        private Camera unityCamera;
        private Vector3 unityCameraWorldPosition;
        private GameObject visObject;
        private ARRaycastManager raycastManager;
        private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

        void Awake()
        {
            // Check Unity references
            
            unityCamera = Camera.main;
            if (!unityCamera)
            {
                AbortWithError("Could not find Camera.");
                return;
            }

            raycastManager = FindFirstObjectByType<ARRaycastManager>();
            if (!raycastManager)
            {
                AbortWithError("Could not find ARRaycastManager");
                return;
            }

            if (!userDistanceLabel)
            {
                AbortWithError("User distance label not assigned.");
                return;
            }

            if (!placeObjectButton)
            {
                AbortWithError("Place object button not assigned");
                return;
            }

            placeObjectButton.onClick.AddListener(HandleAddObjectButtonClick);
            
            // Create and initialize app with a factory object.
            // We pass a reference to this object for subscribing to events.
            AppFactory appFactory = new AppFactory(this);
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
        
        public void UpdateDistanceGUI(float distance)
        {
            userDistanceLabel.text = $"{distance}";
        }

        public void PlaceVisObject(SVector3 position)
        {
            Vector3 pos = position.ToUnity();
            
            if (!visObject)
            {
                visObject = Instantiate(visObjectPrefab, pos, Quaternion.identity);
            }

            visObject.transform.position = pos;
        }
        
        private void HandleAddObjectButtonClick()
        {
            Ray ray = new Ray(unityCameraWorldPosition, unityCamera.transform.forward);
            if (raycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = raycastHits[0].pose;
                RaycastHitEventArgs args = new RaycastHitEventArgs();
                args.PlacementPosition = hitPose.position.ToSystem();
                OnRaycastHit?.Invoke(this, args);
            }
        }

        private void AbortWithError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            Destroy(this);
        }
    }
    
    public class RaycastHitEventArgs : EventArgs
    {
        public SVector3 PlacementPosition;
    }
}

