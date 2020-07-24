using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetector : MonoBehaviour
{

    public enum Gesture
    {
        TAP,
        SWIPE_FORWARD,
        SWIPE_BACKWARD,
        SWIPE_UP,
        SWIPE_DOWN,
        NULL
    }


  

    public Gesture gesture = Gesture.NULL;

    public bool isTouched = false;

    public interface OnGestureListener
    {
        bool onGesture(Gesture gesture);
    }

    private static int SWIPE_DISTANCE_THRESHOLD_PX = 100;
    private static int SWIPE_VELOCITY_THRESHOLD_PX = 100;
    private static double TAN_60_DEGREES = Mathf.Tan(Mathf.Deg2Rad * 60);

    private GestureDetector gestureDetector;
    private OnGestureListener onGestureListener;

    Vector2 touchStartPosition;

    public GestureDetector()
    {
       
    }

    void Update()
    {

        if (Input.touchCount == 1)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                touchStartPosition = Input.touches[0].position;
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                float velocityX = (Input.touches[0].deltaPosition.x - Input.touches[0].position.x) * Time.deltaTime;
                float velocityY = (Input.touches[0].deltaPosition.y - Input.touches[0].position.y) * Time.deltaTime;


                onFling(touchStartPosition, Input.touches[0].position, Input.touches[0].deltaPosition.x - Input.touches[0].position.x, Input.touches[0].deltaPosition.y - Input.touches[0].position.y);
                
            }
            isTouched = true;
           
            
                                                                                                
        }
        else
        {
            isTouched = false;
            gesture = Gesture.NULL;
        }


    }



    /**
         * Swipe detection depends on the:
         * - movement tan value,
         * - movement distance,
         * - movement velocity.
         *
         * To prevent unintentional SWIPE_DOWN and SWIPE_UP gestures, they are detected if movement
         * angle is only between 60 and 120 degrees.
         * Any other detected swipes, will be considered as SWIPE_FORWARD and SWIPE_BACKWARD, depends
         * on deltaX value sign.
         *
         *           ______________________________________________________________
         *          |                     \        UP         /                    |
         *          |                       \               /                      |
         *          |                         60         120                       |
         *          |                           \       /                          |
         *          |                             \   /                            |
         *          |  BACKWARD  <-------  0  ------------  180  ------>  FORWARD  |
         *          |                             /   \                            |
         *          |                           /       \                          |
         *          |                         60         120                       |
         *          |                       /               \                      |
         *          |                     /       DOWN        \                    |
         *           --------------------------------------------------------------
         */


    public bool onFling(Vector2 startPos, Vector2 endPos, float velocityX, float velocityY)
    {
        float deltaX = endPos.x - startPos.x;
        float deltaY = startPos.y - endPos.y;
        double tan = deltaX != 0 ? Mathf.Abs(deltaY / deltaX) : double.MaxValue;

        if (tan > TAN_60_DEGREES)
        {
            if (Mathf.Abs(deltaY) < SWIPE_DISTANCE_THRESHOLD_PX || Mathf.Abs(velocityY) < SWIPE_VELOCITY_THRESHOLD_PX)
            {
                // Application.Quit(0);
                gesture = Gesture.TAP;
                return false;
            }
            else if (deltaY < 0)
            {
                gesture = Gesture.SWIPE_UP;
                return onGestureListener.onGesture(Gesture.SWIPE_UP);
            }
            else
            {
                gesture = Gesture.SWIPE_DOWN;
                Application.Quit(0);
                return onGestureListener.onGesture(Gesture.SWIPE_DOWN);
            }
        }
        else
        {
            if (Mathf.Abs(deltaX) < SWIPE_DISTANCE_THRESHOLD_PX || Mathf.Abs(velocityX) < SWIPE_VELOCITY_THRESHOLD_PX)
            {
                gesture = Gesture.TAP;
                return false;
            }
            else if (deltaX < 0)
            {
                gesture = Gesture.SWIPE_FORWARD;
                transform.position += transform.forward * 3;
                return onGestureListener.onGesture(Gesture.SWIPE_FORWARD);
            }
            else
            {
                gesture = Gesture.SWIPE_BACKWARD;
                transform.position += transform.forward * -3;
                return onGestureListener.onGesture(Gesture.SWIPE_BACKWARD);
            }
        }
    }


    public void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;
        if (isTouched == true)
        {
            
            GUILayout.Label(gesture.ToString());
            GUILayout.Label("X: " + Input.touches[0].position.x);
            GUILayout.Label("Y: " + Input.touches[0].position.y);
            float deltaX = Input.touches[0].position.x - Input.touches[0].deltaPosition.x;
            float deltaY = Input.touches[0].position.y - Input.touches[0].deltaPosition.y;
            double tan = deltaX != 0 ? Mathf.Abs(deltaY / deltaX) : double.MaxValue;
            GUILayout.Label("delta X: " + deltaX);
            GUILayout.Label("delta Y: " + deltaY);
            GUILayout.Label("Tan 60  deg: " + TAN_60_DEGREES);
            GUILayout.Label("Tan: " + tan);
        }
        else
        {
            GUILayout.Label("Not Being Touched");
        }
    }
}

