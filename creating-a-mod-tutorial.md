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


