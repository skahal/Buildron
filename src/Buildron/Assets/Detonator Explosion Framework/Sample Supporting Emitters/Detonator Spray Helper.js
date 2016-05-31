var startTimeMin : float = 0;
var startTimeMax : float = 0;
var stopTimeMin : float = 10;
var stopTimeMax : float = 10;

var firstMaterial : Material;
var secondMaterial : Material;

private var startTime : float;
private var stopTime : float;

//the time at which this came into existence
private var spawnTime : float;
private var isReallyOn : boolean;
function Start ()
{
	isReallyOn = this.GetComponent.<ParticleEmitter>().emit;
	
	//this kind of emitter should always start off
	this.GetComponent.<ParticleEmitter>().emit = false;
	
	spawnTime = Time.time;
	
	//get a random number between startTimeMin and Max
	startTime = (Random.value * (startTimeMax - startTimeMin)) + startTimeMin + Time.time;
	stopTime = (Random.value * (stopTimeMax - stopTimeMin)) + stopTimeMin + Time.time;
	
	//assign a random material
	if (Random.value > 0.5)
	{
		this.GetComponent.<Renderer>().material = firstMaterial;
	}
	else
	{
		this.GetComponent.<Renderer>().material = secondMaterial;
	}
}

function FixedUpdate () 
{
	//is the start time passed? turn emit on
	if (Time.time > startTime)
	{
		this.GetComponent.<ParticleEmitter>().emit = isReallyOn;
	}
	
	if (Time.time > stopTime)
	{
		this.GetComponent.<ParticleEmitter>().emit = false;
	}
}