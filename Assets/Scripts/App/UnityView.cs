using System;
using System.Collections.Generic;
using SVector3 = System.Numerics.Vector3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

namespace Oneeronaut
{
    /**
     * IAppView interface can be implemented to drive the application logic in a different engine.
     */
    public interface IAppView
    {
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<UserPlacesObjectEventArgs> OnUserPlacesObject;
        public void UpdateDistanceGUI(int index, float distance);
        public void CreateEngineObject(SVector3 position);
    }

    /**
     * The UnityView class implements the IAppView interface and serves as a link between the user and the
     * App Controller. It is responsible for firing events on user interaction as well as presenting changes
     * coming from the App Model (via the Controller).
     * 
     * It is the only MonoBehaviour in the project and also serves as a link to the Unity engine.
     */
    public class UnityView : MonoBehaviour, IAppView
    {
        // Events
        
        public event EventHandler<PositionChangedEventArgs> OnCameraPositionChanged;
        public event EventHandler<UserPlacesObjectEventArgs> OnUserPlacesObject;
        
        // Unity references
        
        [SerializeField]                 
        private Button placeObjectButton;
        
        [SerializeField]
        private GameObject visObjectPrefab;
        
        private Camera unityCamera;
        private Vector3 previousUnityCameraPosition;
        
        private ARRaycastManager raycastManager;
        private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

        // Hold references to added objects
        private List<EngineObject> engineObjects = new List<EngineObject>();

        /**
         * The Awake method functions as the entry point of our code.
         * We will setup Unity references as well as initialize the App.
         */
        void Awake()
        {
            // Check and configure Unity references
            
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

            if (!placeObjectButton)
            {
                AbortWithError("Place object button not assigned");
                return;
            }

            placeObjectButton.onClick.AddListener(HandleAddObjectButtonClick);
            
            // Create and initialize app with a factory object.
            // The AppController will receive a reference to this object.
            AppFactory appFactory = new AppFactory(this);
        }

        /**
         * Update is responsible for firing events on camera position changes.
         */
        void Update()
        {
            // Only react if the position changes
            if (unityCamera.transform.position != previousUnityCameraPosition)
            {
                previousUnityCameraPosition = unityCamera.transform.position;
                
                // Build the EventArgs object and fire the event
                // App logic side uses System.Numerics.Vector3 so we need to convert the type here.
                // The conversion method is implemented in TypeExtensions.cs
                PositionChangedEventArgs args = new PositionChangedEventArgs();
                args.NewPosition = previousUnityCameraPosition.ToSystem();
                OnCameraPositionChanged?.Invoke(this, args);
            }
        }

        /**
         * Utility method for testing within the Unity Editor.
         */
        [ContextMenu("FakeRaycastHit")]
        public void FakeRaycastHit()
        {
            Vector3 randomPosition = Random.insideUnitSphere;
            UserPlacesObjectEventArgs args = new UserPlacesObjectEventArgs();
            args.PlacementPosition = randomPosition.ToSystem();
            OnUserPlacesObject?.Invoke(this, args);
        }
        
        /**
         * Listen for GUI Button activation and do a raycast.
         * In case of hits, report nearest to the controller with an event.
         */
        private void HandleAddObjectButtonClick()
        {
            Ray ray = new Ray(previousUnityCameraPosition, unityCamera.transform.forward);
            if (raycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = raycastHits[0].pose;
                UserPlacesObjectEventArgs args = new UserPlacesObjectEventArgs();
                args.PlacementPosition = hitPose.position.ToSystem();
                OnUserPlacesObject?.Invoke(this, args);
            }
        }
        
        /**
         * Instantiate a Unity prefab and save references to the necessary GUI components.
         * Since the objects cannot be moved in this sample app, we do not need to save a
         * reference to the actual gameobject itself.
         *
         * The Controller will call this.
         */
        public void CreateEngineObject(SVector3 position)
        {
            Vector3 pos = position.ToUnity();
            
            // Prefab
            GameObject go = Instantiate(visObjectPrefab, pos, Quaternion.identity);
            
            // GUI
            EngineObject gui = new EngineObject();
            gui.Label = go.GetComponentInChildren<Text>();
            gui.Canvas = go.GetComponentInChildren<Canvas>();
            if (!gui.Label || !gui.Canvas)
            {
                Debug.LogError("Could not find Text or Canvas component on instantiated VisObject.");
                return;
            }
            engineObjects.Add(gui);
        }
        
        /**
         * Update the distance text and rotate the GUI canvas.
         *
         * The Controller will call this.
         */
        public void UpdateDistanceGUI(int index, float distance)
        {
            if (index >= engineObjects.Count)
            {
                Debug.LogError("Index out of range while updating GUI.");
                return;
            }
            
            engineObjects[index].Label.text = $"{distance:F2}";
            engineObjects[index].Canvas.transform.rotation =
                Quaternion.LookRotation(engineObjects[index].Canvas.transform.position - unityCamera.transform.position);
        }

        // Utility; log error and prevent further code from running.
        private void AbortWithError(string errorMessage)
        {
            Debug.LogError(errorMessage);
            Destroy(this);
        }
    }
    
    /**
     * Convenience struct for holding references to Canvas and Text components.
     */
    public struct EngineObject
    {
        public Text Label;
        public Canvas Canvas;
    }
}

