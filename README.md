# ADRUnity3DPrimitives
> Additional basic primitive game objects for Unity3D

ADRUnity3DPrimitives provides basic primitive game objects other than box, sphere, cylinder etc. which are already provided by Unity.

It is still under production. Currently only pyramid is available. The cone is finished except the UV mapping.

# How to Use

Import the package to your project. You can create a pyramid from the menu with "Game Object > Create Other > Pyramid".
The number of sides of the base of the pyramid can be adjusted in the inspector with the `Base Sides` parameter.

You can also crate a pyramid programmatically with:

```
ADRPyramid pyramid = ADRPyramid.Create(Vector3 position, Quaternion rotation);
```
