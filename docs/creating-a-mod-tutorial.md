# Tutorial :: Creating a mod

## Downloads

Download the Buildron for your platform (Linux, Mac or Win) and the ModSdk from our [releases page](https://github.com/skahal/Buildron/releases).
Unzip Buildron in any folder, for example: C:\Buildron.

## Where can I see some Buildron mods in action?
Execute Buildron.

You'll see a button on left-bottom corner with the text "6 mods", click on it and you'll see the list of loaded mods and the preferences of each mod.These mods are the [Buildron Classic Mods](https://github.com/skahal/Buildron-Classic-Mods).

Go back to main screen and hit the "Play" button.

If you don't have a CI server to connect right now, just type TEST_MODE on server IP and type anything in username and password.

About everything you see after hit "Play" button are made by mods.

Close Buildron.

## Create a C# project
Open your IDE (VS/XS) and create a new C# class library project.

The name of your mod is name of this project. The mod name should be unique, so see the mod list already created here: [Mods List](https://github.com/skahal/Buildron#Mods List). 

In the properties/options of the project change the target framework to .NET 3.5

Save the whole solution.

In the same folder where is your .sln file, create a folder called "references" and unzip the Buildron.ModSdk.zip inside it.

### Buildron.Mod.Targets
Open the .csproj file of your C# class library in a text editor and after this line:

```xml  
<Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
```

add the line bellow :

```xml
  <Import Project="..\references\Buildron.Mod.targets" />
```

Save the .csproj file.

Open the Buildron.Mod.targets in a text editor and change the YOUR_BUILDRON_MODS_FOLDER to your Buildron "Mods" folder path. For example, if your Buildron is in folder c:\Buildron, you should replace YOUR_BUILDRON_MODS_FOLDER with c:\Buidlron\Mods.
> Now, every time you compile your C# class library the needed assemblies will be copy to your Unity project assets folder and you will can use them in your assets, like prefabs and whatever.

### References
In the project references, add the reference to:
	
* UnityEngine.dll ([more about reference UnityEngine.dll](https://docs.unity3d.com/Manual/UsingDLL.html))
	*  Mac: /Applications/Unity/Unity.app/Contents/Frameworks/Managed/
	* Win: C:\Program Files\Unity\Editor\Data\Managed
* Buildron.Sdk.dll
* Skahal.Unity.Scripts.dll
* Skahal.Unity.Scripts.Externals.dll

### Mod.cs
In the root folder add a class file called Mod that implements IMod inteface.

The Mod.cs Initialize method is where your mod will receive his ModContext. With ModContext you can register the mod preferences and listen a lot of Buildron's event like CIServerConnected, BuildFound, UserFound and so on. 

## Create the Unity3d project
If your mod has assets, then  you need to create an Unity project too. If your mod has no assets, you can ignore this section. 

Open Unity3d and create a new project with the same name you give to the c# class library project.

### Project settings
In Edit / Project settings / Editor
* Version control mode, select "Visible Meta Files"
* Assert serialization mode, select "Force Text"

### Buildron.ModSdk.Editor.dll
There is only one .dll you should copy manually to inside your folder Assets/Scripts/references, this is Buildron.ModSdk.Editor. This is needed because in this assemblies there tools that you use only on Unity Editor and cannot be reference by your mod.

### Simulator
First of all, you need to create the simulator, access the menu "Buildron / Create simulator". With the simulator you can simulate some Buildron events.

## Developing our ToastyMod sample
> "Setting an example is not the main means of influencing others, it is the only means." (Albert Einstein)

![](https://raw.githubusercontent.com/skahal/Buildron-Mod-Samples/master/src/Unity/ToastyMod/Assets/Sprite.png)

Now that we have the C# class library project created and the Unity3d project created and configured too we'll create a mod that mimics the Mortal Kombat's Toasty easter egg, if ou don't know what I'm talking about, take a look on this video: https://youtu.be/pRcA0AZhuhs?t=1m30s

## Creating the prefab
The first thing we need to do is create a prefab to hold the [Dan Forden](https://pt.wikipedia.org/wiki/Dan_Forden) photo and sound.
In Unity3d editor, create a new game object called ToastyHolderPrefab.

Copy this [image](https://raw.githubusercontent.com/skahal/Buildron-Mod-Samples/master/src/Unity/ToastyMod/Assets/Sprite.png) and this [sound](https://raw.githubusercontent.com/skahal/Buildron-Mod-Samples/master/src/Unity/ToastyMod/Assets/Sound.mp3) to your Assets folder.

Drag the Sprite.png to the ToastyHolderPrefab.

Then add an AudioSource component to the ToastyHolderPrefab and associate the sound.mp3 file to AudioSource component.

Now our prefab is done. Drag it to your "Assets/Resources" folder and delete it from scene.

Select the ToastyHolderPrefab in "Asserts/Resources" folder, in the right bottom corner of Unity editor you can see the assets bundle configuration, create a new one with the full name of our mod, in this case "ToastyMod".

> Remember to mark all your assets with an asset bundle with same name of your mod project. More info at [Working with AssetBundles and the AssetBundle manager](https://unity3d.com/pt/learn/tutorials/topics/scripting/assetbundles-and-assetbundle-manager).


## Programing the mod behavior
Open the C# class library and the Mod.cs file.

### The ToastyController
We need create a MonoBehaviour to mimics the Toasty behavior. Create a file called ToastyController and paste the code below:

```csharp
/// <summary>
/// Controller responsbile to mimics the Mortal Kombat's Toasty! easter egg.
/// </summary>
public class ToastyController : MonoBehaviour {

    #region Fields
    private AudioSource m_audio;
	private Vector2 m_currentTargetPosition;
	private float m_currentSpeed;
    #endregion

    #region Editor properties
    public float WarmupSeconds = 2f;
	public Vector3 MoveSize = new Vector3(85, 0, 0);
	public float SlideInSpeed = 0.5f;
	public float SlideOutSpeed = 1f;
    #endregion

    #region Methods
    void Awake()
	{
		m_audio = GetComponentInChildren<AudioSource> ();
	}

	void Update () {
        // Update the Toasty's sprite in direction of the target position.
		transform.position = Vector2.Lerp (transform.position, m_currentTargetPosition, m_currentSpeed);
	}

	void OnEnable()
	{
        // Everty time that became enabled, starts the slide.
		StartCoroutine (Slide ());
	}

    /// <summary>
    /// Perform the slide of the Toasty's sprite.
    /// </summary>
    /// <returns>The enumerator.</returns>
    private IEnumerator Slide()
	{
        // Put the game object in the initial position and wait the warmup seconds.
		m_currentTargetPosition = transform.position;
		yield return new WaitForSeconds (WarmupSeconds);

        // The sound duration will be used as time of the slide.
		var slideSeconds = m_audio.clip.length;

		// Slide in. Sets the target position (used on Update method) and play sound.
		m_currentTargetPosition = transform.position - MoveSize;
		m_currentSpeed = SlideInSpeed;
		m_audio.Play ();

        // Wait for the sound finish.
		yield return new WaitForSeconds (slideSeconds);

        // Slide out. Sets the target position back to out of the screen.
        m_currentTargetPosition = transform.position + MoveSize;
		m_currentSpeed = SlideOutSpeed;
		yield return new WaitForSeconds (slideSeconds);

        // Deactivate the game object, then it can be used a next time.
		gameObject.SetActive(false);
	}
    #endregion
}
```

### Configuring the prefab
In Unity3d editor select our ToastyHolderPrefab and define the follow properties:

* WarmupSeconds = 2f
* MoveSize = 85, 0, 0
* SlideInSpeed = 0.5
* SlideOutSpeed = 1;

### Instantiating the ToastyHolderPrefab
We need to instanciate our previously created prefab, to do that we need to load our asset:

```csharp
var prefab = context.Assets.Load ("ToastyHolderPrefab");
```

then we need to create a game object with it:

```csharp
var holder = context.GameObjects.Create (prefab);
```

Now our game object that hold our Dan Forden's sprite and sound.

Note: there is an extension method that to this two line of codes in burst:

```csharp
var holder = context.CreateGameObjectFromPrefab("ToastyHolderPrefab");
```

### Listening the build events
The first thing we should decide is when you want to show the "Toasty". Now we'll show the "Toasty" every time a build status is changed to success, running or failed:

```csharp
public void Initialize (IModContext context)
{
	context.BuildStatusChanged += (sender, e) => 
	{
		var status = e.Build.Status;
		var preferences = context.Preferences;
		var active = false;
	
		switch(status)
		{  
			case BuildStatus.Success:
			case BuildStatus.Running:
			case BuildStatus.Failed:
				holder.SetActive(true);
				break;
	
			default:
				holder.SetActive(false);
				break;
		}
	};
}
```

With the above code our mod will be notified everytime a build status changed. If status is one we expected then we activate the holder game object, otherwise we deactivate it.

### Simulating
In the Unity3d editor, go to menu "Buildron / Show simulator". A "Simulator" game object will be created on your current scene. You can save the scene right now, use any name, because this scene is just for simulation, it will never be used by Buildron.

Play the scene and press ctrl+e (win) or cmd+e(mac) to show simulator window. In the "Build status" dropdown select "Success" and click on "BuildStatusChanged" button. The Toasty's sprite should slide and play the sound. Now, you can test the other status too.
 
### Using preferences
We could improve our mod experience allowing the user select when he/she wants to show the Toasty. The best way to do this is through preferences, in the ModContext there is a "Preferences" property that allow each mod register and user it own preferences.

#### Registering preferences
The first thing when we want to use preferences is register them. In our case we want to expose 3 preferences to the users: "Show on success", "Show on running", "Show on failed":

```csharp
context.Preferences.Register (
	new Preference ("ShowOnSuccess", "Show on success", PreferenceKind.Bool, true),
	new Preference ("ShowOnRunning", "Show on running", PreferenceKind.Bool, false),
	new Preference ("ShowOnFailed", "Show on failed", PreferenceKind.Bool, false));
``` 

With the preferences registered, we need to use them. Change our previous BuilsStatusChanged code to this:

```csharp
context.BuildStatusChanged += (sender, e) => 
{
	var status = e.Build.Status;
	var preferences = context.Preferences;
	var active = false;

	switch(status)
	{  
		case BuildStatus.Success:
			active = preferences.GetValue<bool>("ShowOnSuccess");
			break;
			
		case BuildStatus.Running:
			active = preferences.GetValue<bool>("ShowOnRunning");
			break;

		case BuildStatus.Failed:
			active = preferences.GetValue<bool>("ShowOnFailed");
			break;
	}

	holder.SetActive(active);
};
```

### Testing the preferences
In the Unity3d editor play the scene and press ctrl+e (win) or cmd+e(mac) to show simulator window. There is a second tab called "Preferences", change the values of our mod preferences and test its behaviour on simulator.

### Building the mod
Now we need to build the mod to test it diretly on Buildron. Click in the menu "Buildron / Build mod", in the windows select for what plaftorm you want to build it. In the "Mods folder" type the folder "Mods" where is your Buildron installed, in Win should be a folder "Mods" in same place where is your "Buildron.exe" file, in Mac should be a "Mods" folder inside the Buildron.app file.

### Testing on Buildron
Open Buildron, if everything is ok you should see your mod in the mods panel, click in the mods button (left bottom corner). Open the dropdown, your mod should be there, select it, the 3 "Show on" preferences should be shown. Go back to config panel and starts Buildron. Now every a builds status changed match the ToastyMod preferences you should see the Toasty's sprite and sound.

If your mod is not loaded on Buildron, then there is a problem with it, open the Buildron [log file](https://docs.unity3d.com/Manual/LogFiles.html) and look for error messages. If you need any help, [let me know](http://twitter.com/ogiacomelli).

At this time the ToastyMod should looks like this: [https://www.youtube.com/watch?v=Ju9znmp8tOQ](https://www.youtube.com/watch?v=Ju9znmp8tOQ)

Here you can see the full source code of ToastyMod:

* [C# class library](https://github.com/skahal/Buildron-Mod-Samples/tree/master/src/Code/ToastyMod)
* [Unity3d project](https://github.com/skahal/Buildron-Mod-Samples/tree/master/src/Code/ToastyMod)

### Packing the mod
So, your mod is ready to go. Now you need to pack it to allow Buildron users to use it. We recommend you build it to each platform. Open the "Build mod" window and choose "Mac" platform, click and "Build", after build is done, zip the folder of your mod and add ".mac" extension to it, so our ToastyMod will be ToastMod.mac.zip. Repeat the previous steps to generate the package for Linux and Win.

To let others Buildron's know about your mod, fork [Buildron](https://github.com/skahal/buildron/fork) and add a link to your mod repository to our [mods-support#mods-list](Mods list), then [submit a pull-request](https://github.com/skahal/buildron/pull/new/master).

### That's all folks!
Congratulations! Now you know how to build a mod to Buildron.


## FAQ
### Tags
There two special tags used on Buildron and mods: "Build" and "User". If you use them on your mod you should define the both as the first tags on your tag manager. The first should be "Build" and the second should be "User". This is necessary because Unity use the tags index on tag manager to search the game objects.


### Preferences
If you want expose some preferences to let Buildron's user select about your bot, you can use ModContext.Preferences property.
You can register a preferences with the RegisterPreferences methods. It's recommended that you register your mod preferences as first things on Initialize method.

You can read your preferences usen ReadValue method on same ModContext.Preferences property. In most of case you should only read preferences after the CIServerConnected event be raised.

