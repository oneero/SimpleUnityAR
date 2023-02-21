# SimpleUnityAR
A small example of a simple AR app using Unity.

## Features
This project showcases one way of structuring and implementing the follow features:

1. The user can place AR objects on surfaces anchored to the real world.

2. The placed objects have a GUI element indicating distance to the camera in real-time.

## Dependencies and dev environment
The project uses AR Foundation and AR Kit version 4.2.7 packages.

- Developed on Unity 2021.3.18.
- Built for iOS with XCode 14.2.
- Tested on iPhone 11 Pro Max with iOS 16.2.

### Building and testing
1. Open the project in Unity 2021.3.18.
2. Check File > Build Settings to make sure iOS is selected.
3. Build and Run will then produce and open an XCode project. 

To build and test the app on your iOS device, you will need to configure signing in XCode. You can read more about the process [here](https://learn.unity.com/tutorial/publishing-for-ios#).

Building for Android requires installing and configuring the AR Core package in the Unity package manager. This has not been tested.

## Application structure

The overall structure of the project follows the MVC pattern:
- A model holds the relevant data of the application and provides logic for adding and updating objects.

- A view sends signals to the controller on user inputs and maintains a visual representation of the model.

- A controller subscribes to events from both the model and the view and calls appropriate methods to enable communication between the two.

### Details

The view functions as a bridge between the engine and other parts of the application. It is the only Unity-dependent part as well as the only MonoBehaviour in the project. Creating an alternative implementation with the IAppView interface allows porting the application to other engines.

All parts of the application have corresponding interfaces that can be used to construct differing implementations. The model is also generic, which allows changing the implementation of the data type used for holding position information.

