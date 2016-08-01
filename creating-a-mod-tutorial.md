# Tutorial :: Creating a mod


## Create a C# project
* Open your IDE (VS/XS) and create a new C# class library project.
* The name of your mod is name of this project. The mod name should be unique, so see the mod list already created here: 
* In the properties/options of the project change the target framework to .NET 3.5
* Opent the .csproj file in a text editor and add the line bellow after this line:
  <Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
  <Import Project="..\msbuilds\Buildron.ClassicMods.targets" />
* Open the Package Manager Console and run: install-package Buildron.ModSdk
* In the project references, add the reference to:
	* UnityEngine.dll (see here where is located your UnityEngine.dll)
	-----* Buildron.Sdk.dll
	-----* Skahal.Unity.Scripts.dll
	-----* Skahal.Unity.Scripts.Externals.dll
* In the root namespace add a class called Mod that implements IMod inteface.


* If your mod has assets, then  you need to create an Unity project too. Open Unity3d and create a new project with the same name you give to the c# class library project.
* In Edit / Project settings / Editor
* * Version control mode, select "Visible Meta Files"
* * Assert serialization mode, select "Force Text"
* Change your msbuild to copy your .dlls to your  Assets/references folder.
* * Now, every time you compile your C# class library the needed assemblies will be copy to your Unity project assets folder and you will can use them in your assets, like prefabs and whatever. So, just compile this first time to seet the assemblies inside your Unity project.
* There is only one .dll you should copy manually to inside your folder Assets/Scripts/references, this is Buildron.ModSdk.Editor. This is needed because in this assemblies there tools that you use only on Unity Editor and can be reference by your mod.
* First of all, you need to create the emulator, access the menu Buildron / Create emulator.
* Remember to mark your assets with an asset bundle with same name of your mod project.



* Tags
* * There two special tags used on Buildron and mods: "Build" ans "User". If you use them on your mod you should define the both as the first tags on your tag manager. The first should be "Build" and the second should be "User". This is necessary because Unity use the tags index on tag manager to search the game objec (ts.