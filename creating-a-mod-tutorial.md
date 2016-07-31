# Tutorial :: Creating a mod


## Create a C# project
* Open your IDE (VS/XS) and create a new C# class library project.
* The name of your mod is name of this project. The mod name should be unique, so see the mod list already created here: 
* In the properties/options of the project change the target framework to .NET 3.5
* Opent the .csproj file in a text editor and add the line bellow after this line:
  <Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
  <Import Project="..\msbuilds\Buildron.ClassicMods.targets" />
* In the project references, add the reference to:
	* UnityEngine.dll
	* Buildron.Sdk.dll
	* Skahal.Unity.Scripts.dll
	* Skahal.Unity.Scripts.Externals.dll
* In the root namespace add a class called Mod that implements IMod inteface.


* If your mod has assets, then  you need to create an Unity project too. Open Unity3d and create a new project with the same name you give to the c# class library project.
* In Edit / Project settings / Editor
* * Version control mode, select "Visible Meta Files"
* * Assert serialization mode, select "Force Text"
* Change your msbuild to copy your .dlls to your  Assets/references folder.
* Remember to mark your assets with an asset bundle with same name of your mod project.



* Tags
* * There two special tags used on Buildron and mods: "Build" ans "User". If you use them on your mod you should define the both as the first tags on your tag manager. The first should be "Build" and the second should be "User". This is necessary because Unity use the tags index on tag manager to search the game objects.