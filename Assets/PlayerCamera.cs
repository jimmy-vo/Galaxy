using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Vector3 NormalCameraRelativePosition = new Vector3(0, .93f, -4.5f);
    public Vector3 NormalCameraRelativeRotation = new Vector3(6.5f, 0, 0);
    public float ArmingCameraRelativeDistance = 2f;
    public GameObject Player;
    public GameObject Room;
    
    // Use this for initialization
    void Start () {
        Camera.main.transform.position = Player.transform.position + NormalCameraRelativePosition;
    }
	
	// Update is called once per frame
	void Update () {
        if(PlayerClickHandle.IsLongPress)
        {
            Camera.main.transform.parent.position = (Camera.main.transform.forward.normalized * ArmingCameraRelativeDistance) + transform.position;
            //Debug.DrawRay(Camera.main.transform.parent.position, Camera.main.transform.forward.normalized);
        }
        else
        {
            Camera.main.transform.parent.position = transform.TransformPoint(NormalCameraRelativePosition);
        }
    }
}
