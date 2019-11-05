# unity-player-movment-controller

## Usage
This player movment controller script can be applied to your player object to gain movment control in a 2D platform world. 

It has a variable jump dependant on how long you hold down your mapped jump button, and handles throws an `onLand()` event when hitting the ground.

Allows for a gamepad to be connected and swapped over to gamepad controls, and back to keyboard controls when disconnected.

### Features
* 2D Platformer Controller
* Using Unity's new input system to button map jump and horizontal movment.
* Well rounded _feel good_ jump
* Gamepad/Keyboard Hot-swappable input
* Throws `onLand()` event
* Ability to chose what ground is
* Variable jump time, run speed and jump force
* Contains movment smoothing


## Setup

### Input 
This character controller script can be applied to your player object to control movment.

To use this movment controller, you must use Unity's new Input System, which you can follow [these instructions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html) to install.

You then must create a Gameplay node, with mappings for `Jump` and `Horizonatal`.

The `Jump` mapping should handle keyboard and gamepad input, where as the `Horizontal` input just needs to handle gamepad input. They are called in this controller by using `_controls.Gameplay.Jump` and `_controls.Gameplay.Horizontal`.

### Variables
You will need to create a `RigidBody2D` component on the same object. 

Additionally you will need to select which masks are consided "ground" by selecting them on the `ground` serialized variable on the script component in Unity. Additionally you will need to create an empty object on your player object called GroundCheck at the "feet" of your player object. This will need to be passed into the script component under the serialized field `groundCheck` in Unity.

## Dependancies

* [Unity's new input system](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html)
* [LINQ](https://docs.microsoft.com/en-us/dotnet/api/system.linq?view=netframework-4.8)

## Contact
I am still relatively new at game development, but have been doing .NET programming for several years. I would love to hear from you, so feel free to contact me on my [twitter](https://twitter.com/carterjsnowden).