using System;
using System.Collections;
using System.Collections.Generic;

namespace Oneeronaut
{
    public interface IAppController
    {
        public void SubscribeEngineEvents(IAppEngineBridge engineBridge);
    }
    
    public class AppController : IAppController
    {
        private IAppModel model;
        private IAppView view;
        
        public AppController(IAppModel model, IAppView view)
        {
            this.model = model;
            this.view = view;
        }

        public void SubscribeEngineEvents(IAppEngineBridge engineBridge)
        {
            engineBridge.OnCameraPositionChanged += view.HandleUnityCameraPositionChanged;
            engineBridge.OnPlaceObjectGUIActivated += view.HandleUnityPlaceObjectGUIActivated;

            view.OnGUILabelUpdated += engineBridge.HandlePlacedObjectLabelUpdate;
        }

        public void AddPlacedObject()
        {
            throw new NotImplementedException();
        }
        
        
    }
    
}
