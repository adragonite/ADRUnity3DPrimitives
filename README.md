# ADRUnity3DPrimitives
> Additional basic primitive game objects for Unity3D

ADRUnity3DPrimitives provides basic primitive game objects other than box, sphere, cylinder etc. which are already provided by Unity.

It is still under production. Currently pyramid and cone are available.

# How to Use

Import the package to your project. You can create a pyramid from the menu with "Game Object > Create Other > Pyramid".
The number of sides of the base of the pyramid can be adjusted in the inspector with the `Base Sides` parameter.

You can also crate a pyramid programmatically with:

```
ADRPyramid pyramid = ADRPyramid.Create(Vector3 position, Quaternion rotation);
```

Creating a cone is very similar, just use "Game Object > Create Other > Cone". You can also create it programmatically with:

```
ADRCone pyramid = ADRCone.Create(Vector3 position, Quaternion rotation);
```
