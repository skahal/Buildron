function Update () { 
	GetComponent.<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x)); 
}