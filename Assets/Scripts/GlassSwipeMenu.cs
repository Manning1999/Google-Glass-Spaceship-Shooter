using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassSwipeMenu : UnityGlassGestureDetector
{

    [SerializeField]
    private List<GlassButton> buttons = new List<GlassButton>();

    [SerializeField]
    private int buttonMoveSpeed = 3;

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
        if (buttonToFocusOn > buttons.Count || buttonToFocusOn < 0) return;
        Debug.Log("Changing button");


        moveButtons = true;
        currentButton = buttonToFocusOn;
    }



    protected override void Update()
    {
        base.Update();

        if(moveButtons == true)
        {
            MoveButtons();
        }
    }

    void MoveButtons()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        if (Mathf.Abs(170 - buttons[currentButton].transform.GetComponent<RectTransform>().localPosition.x) < 5)
        {
            moveButtons = false;
            return;
        }
        if (buttons[currentButton].transform.GetComponent<RectTransform>().localPosition.x < 170)
        {
            transform.GetComponent<HorizontalLayoutGroup>().padding.left += buttonMoveSpeed;
        }
        else if (buttons[currentButton].transform.GetComponent<RectTransform>().localPosition.x > 170)
        {
            transform.GetComponent<HorizontalLayoutGroup>().padding.left -= buttonMoveSpeed;
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
