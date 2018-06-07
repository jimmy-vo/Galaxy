using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClickHandle : MonoBehaviour
{
    static public float DoubleClickTime = 0.7f;
    static float TimeStickClick;

    public enum ClickState
    {
        None, FirstDown, FirstUp,//shouldn't use
        Click, LongPress, LongPressRelease,
        PostClick, SecondDown,//shouldn't use
        DoubleClick
    }

    public static ClickState ClickEvent
    {
        get { return clickState; }
    }

    public static bool IsLongPress
    {
        get { return (clickState == ClickState.LongPress); }
    }

    public static bool IsClick
    {
        get { return (clickState == ClickState.Click); }
    }
    public static bool IsDoubleClick
    {
        get { return (clickState == ClickState.DoubleClick); }
    }

    static ClickState clickState = ClickState.None;

    private void Start()
    {
        TimeStickClick = DoubleClickTime;
        clickState = ClickState.None;
    }



    private void Update()
    {
        TimeStickClick += Time.deltaTime;
        switch (clickState)
        {
            case ClickState.None:
                if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.FirstDown;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.FirstDown:
                if (TimeStickClick > DoubleClickTime)
                {
                    clickState = ClickState.LongPress;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonUp("Tap"))
                {
                    clickState = ClickState.FirstUp;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.FirstDown;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.FirstUp:
                if (TimeStickClick > DoubleClickTime)
                {
                    clickState = ClickState.Click;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonUp("Tap"))
                {
                    clickState = ClickState.Click;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.SecondDown;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.Click:
                clickState = ClickState.PostClick;
                TimeStickClick = 0;
                break;
            case ClickState.PostClick:
                if (TimeStickClick > DoubleClickTime)
                {
                    clickState = ClickState.None;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonUp("Tap"))
                {
                    clickState = ClickState.None;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.SecondDown;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.SecondDown:
                if (TimeStickClick > DoubleClickTime)
                {
                    clickState = ClickState.LongPress;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonUp("Tap"))
                {
                    clickState = ClickState.DoubleClick;
                    TimeStickClick = 0;
                }
                else if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.None;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.DoubleClick:
                clickState = ClickState.None;
                TimeStickClick = 0;
                break;
            case ClickState.LongPress:
                if (Input.GetButtonUp("Tap"))
                {
                    clickState = ClickState.LongPressRelease;
                    TimeStickClick = 0;
                }
                break;
            case ClickState.LongPressRelease:
                if (Input.GetButtonDown("Tap"))
                {
                    clickState = ClickState.FirstDown;
                }
                else
                {
                    clickState = ClickState.None;
                }
                break;
        }
    }
}
