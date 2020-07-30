using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassSwipeMenu : GestureDetector
{

    [SerializeField]
    private List<GlassButton> buttons = new List<GlassButton>();

    [SerializeField]
    private float buttonMoveSpeed = 3;

    [SerializeField]
    private int currentButton = 0;

    public bool moveButtons = false;

    int originalPosition;

    [SerializeField]
    int distanceToShift = 0;

    [SerializeField]
    int desiredPadding;



    public void FocusOnButton(int buttonToFocusOn)
    {
        if (buttonToFocusOn > buttons.Count - 1 || buttonToFocusOn < 0) return;

        distanceToShift = (int)Vector3.Distance(buttons[currentButton].gameObject.transform.position, buttons[buttonToFocusOn].gameObject.transform.position);

        if (buttonToFocusOn < currentButton) distanceToShift = -distanceToShift;

        originalPosition = transform.GetComponent<HorizontalLayoutGroup>().padding.left;
        // desiredPadding = originalPosition - distanceToShift;
        // transform.GetComponent<HorizontalLayoutGroup>().padding.left = (int)desiredPadding;
        // transform.GetComponent<RectTransform>().ForceUpdateRectTransforms();
        desiredPadding = originalPosition - (distanceToShift + 50);
        moveButtons = true;
        currentButton = buttonToFocusOn;
    }



    protected override void Update()
    {
        base.Update();

        if(moveButtons == true)
        {
            

    
            try
            {
                Debug.Log("Moving Now");
                GetComponent<HorizontalLayoutGroup>().padding.left = (int)Mathf.Lerp(GetComponent<HorizontalLayoutGroup>().padding.left, desiredPadding, buttonMoveSpeed * Time.deltaTime);
                Debug.Log("Got here");
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

            if(transform.GetComponent<HorizontalLayoutGroup>().padding.left == desiredPadding)
            {
              //  moveButtons = false;
            }
        }
    }



    [ContextMenu("Increase button")]
    public void IncreaseButton()
    {
        FocusOnButton(currentButton + 1);
    }
    [ContextMenu("Decrease button")]
    public void DecreaseButton()
    {
        FocusOnButton(currentButton - 1);
    }




    protected override void Tap()
    {
        base.Tap();
        buttons[currentButton].Click();
    }

    protected override void SwipeForward()
    {
        base.SwipeForward();
        FocusOnButton(currentButton + 1);
    }
    
    protected override void SwipeBack()
    {
        base.SwipeForward();
        FocusOnButton(currentButton - 1);
    }


}
