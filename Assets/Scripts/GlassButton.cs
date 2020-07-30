using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlassButton : MonoBehaviour
{

    public UnityEvent OnClick = new UnityEvent();


    public void Click()
    {
        OnClick.Invoke();
    }
}
