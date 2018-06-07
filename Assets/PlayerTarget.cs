using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTarget : MonoBehaviour {

    public Text CrossHair;
    public float ScaleSpeed = 0.05f;
    public float ScaleMax = 1.5f;
    public float ScaleMin = 1f;
    public float Thickness = 1f;
    public Text UI_Target;

    static GameObject CurrentTarget;
    static GameObject LockedTarget;

    static public GameObject Target {
        get
        {
            if (LockedTarget != null)
            {
                return LockedTarget;
            }
            else
            {
                return null;
            }
        }
    }

    // Use this for initialization
    void Start () {
        CurrentTarget = LockedTarget  = null;
        CrossHair.color = Color.white;
        UI_Target.color = Color.white;
        UI_Target.text = null;
    }
    
    // Update is called once per frame
    void Update () {
        if (PlayerClickHandle.IsLongPress)
        {
            RaycastHit hit;
            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            if (Physics.SphereCast(Camera.main.transform.position, Thickness, Camera.main.transform.forward, out hit))
            {
                    if ((hit.transform.gameObject.tag == "ASTOROID") || ((hit.transform.gameObject.tag == "ENEMY_SPACESHIP")))
                {
                    Debug.DrawRay(Camera.main.transform.position, hit.transform.gameObject.transform.position, Color.red, Time.deltaTime, false);

                    if ((LockedTarget == CurrentTarget) && (CurrentTarget == hit.transform.gameObject))
                    {
                        CrossHair.transform.localScale = Vector3.one * ScaleMax;
                        CrossHair.color = Color.red;
                        UI_Target.color = Color.red;
                        UI_Target.text = (CurrentTarget == null) ? null : CurrentTarget.name.Replace("(Clone)", "").Replace("_", " ");
                    }
                    else if (CurrentTarget != hit.transform.gameObject)
                    {
                        CrossHair.transform.localScale = Vector3.one * ScaleMin;
                        CurrentTarget = hit.transform.gameObject;
                    }
                    else if (CrossHair.transform.localScale.x < ScaleMax)
                    {
                        CrossHair.transform.localScale += Vector3.one * Time.deltaTime * ScaleSpeed;
                        CrossHair.color = Color.red;
                        UI_Target.color = Color.white;
                        UI_Target.text = (CurrentTarget == null) ? null : CurrentTarget.name.Replace("(Clone)", "").Replace("_", " ");
                    }
                    else
                    {
                        CrossHair.transform.localScale = Vector3.one * ScaleMax;
                        LockedTarget = CurrentTarget = hit.transform.gameObject;
                        CrossHair.color = Color.red;
                        UI_Target.color = Color.red;
                        UI_Target.text = (CurrentTarget == null) ? null : CurrentTarget.name.Replace("(Clone)", "").Replace("_", " ");
                    }
                }
                else
                {
                    if (CrossHair.transform.localScale.x > ScaleMin)
                    {
                        CrossHair.transform.localScale -= Vector3.one * Time.deltaTime * ScaleSpeed;
                    }
                    else
                    {
                        CrossHair.transform.localScale = Vector3.one * ScaleMin;
                    }
                    CrossHair.color = Color.white;
                    UI_Target.color = (LockedTarget != null) ? Color.red : Color.white;
                    UI_Target.text = (LockedTarget == null) ? null : LockedTarget.name.Replace("(Clone)", "").Replace("_", " ");
                }
            }
            else
            {
                if (CrossHair.transform.localScale.x > ScaleMin)
                {
                    CrossHair.transform.localScale -= Vector3.one * Time.deltaTime * ScaleSpeed;
                }
                else
                {
                    CrossHair.transform.localScale = Vector3.one * ScaleMin;
                }
                CrossHair.color =  Color.white;
                UI_Target.color = (LockedTarget != null) ? Color.red : Color.white;
                UI_Target.text = (LockedTarget == null) ? null : LockedTarget.name.Replace("(Clone)", "").Replace("_", " ");
            }

        }
        else
        {
            CrossHair.transform.localScale = Vector3.one * ScaleMin;
            CurrentTarget = LockedTarget = null;

            UI_Target.text = null; 
            CrossHair.color = Color.white;
            UI_Target.color = Color.white;
        }


    }
}
