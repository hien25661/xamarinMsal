Microsoft Intune MAM Assembly Remapping MSBuild Task
===============================================

This MSBuild task's purpose is to manipulate a given target set of assemblies' IL to swap out base types of classes as well as change method names in these classes to match the base type counterparts.

This task is used to manipulate `Xamarin.Forms` assemblies to use the base type classes which Microsoft Intune requires to implement its SDK (for example, Intune requires using a `MAMActivity` in place of `Activity` and methods such as `OnMAMCreate` instead of `OnCreate`).

This build task may or may not work with other libraries.

To use the build task in your project, make sure your .csproj includes a reference to the `Microsoft.Intune.MAM.Remapper.targets` file.

Your project also needs to specify a YAML mapping configuration file in the .csproj file like this:
```xml
<ItemGroup>
  <RemappingConfigFile Include="remapping-config.json" />
</ItemGroup>
```

Your remapping-config.json file should follow this JSON format:
```javascript
{
  // List of assembly names to manipulate the IL of
  // You can specify the full/relative path, or just the assembly filename
  "AssembliesToProcess": [
    "Xamarin.Forms.Platform.Android.dll"
  ],
  // List of assemblies that contain the new types/methods to map to
  // for each assembly you need to define the base type/method mappings
  "RemapToAssembly": {
    "TargetAssembly": "Microsoft.Intune.Android.dll",
    "BaseTypeMappings": [
      {
        // Any class with this base type should be remapped
        "From": "Android.App.Activity",
        // This is the new base type the class will be mapped to
        "To": "Microsoft.Intune.Mam.Client.App.MAMActivity",
        "MethodMappings": {
          // Old method name and the new method name to map to
          // This will change `this.name()` calls within the method body too
          "OnCreate": "OnMAMCreate",
          "OnPause": "OnMAMPause",
          "OnResume": "OnMAMResume",
          // ...
        }
      },
      {
        "From": "Android.App.Application",
        "To": "Microsoft.Intune.Mam.Client.App.MAMApplication",
        "MethodMappings": {
          "OnCreate": "OnMAMCreate"
        }
      }
      // ...
    ]
}
```

The remapping task will rewrite the IL of existing references in place, so while future runs will still inspect the IL for changes, it shouldn't find any.

This means that the first time you compile your app, the IL changes will be made, and before those changes are made you may not have intellisense for what the changes will become and you may experience build errors the first time.
