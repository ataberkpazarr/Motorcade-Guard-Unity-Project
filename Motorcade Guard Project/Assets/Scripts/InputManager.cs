using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MotorcadeGuard
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;


        private Vector2 startPos, endPos, difference;
        private SwipeType swipetype = SwipeType.NONE;
        private float swipeThreshold = 0.15f; // if our screen is 100 pixel width then we have to make sure that our distance between start and end pos has to be minimum %15 of 100 pixels in other words 15 pixeles
        private float swipeTimeLimit = 0.25f; // time within which we have to complete our swipe

        private float startTime, endTime;

        public Action<SwipeType> swipeCallback;  //for subscribing to callback
        public Action<int> new_endpos;


        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else

            {
                Destroy(gameObject);

            }
        }

       private void Update ()
        {
            if (Input.GetMouseButtonDown(0)) // we we tap on our screen
            {

                startPos = endPos = Input.mousePosition;
                startTime = endTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0)) // if we release our button or click? then we will set our enpos to mouses last position
            {

               endPos = Input.mousePosition;
                endTime = Time.time;

                if (endTime - startTime <= swipeTimeLimit) // so a swipe has just performed 
                {
                    DetectSwipe(); // handle the swipe
                }
            }
        }


        void DetectSwipe() // if it is a swipe then decide which direction it occured 
        {
            swipetype = SwipeType.NONE;
            difference = endPos - startPos;

            if (difference.magnitude > swipeThreshold * Screen.width ) 
            {

                if ( endPos.x > startPos.x)
                {
                    swipetype = SwipeType.RIGHT;
                }

                else if (startPos.x >endPos.x) 
                {
                    swipetype = SwipeType.LEFT;
                }
            }
            if (swipeCallback != null)
            {
                swipeCallback(swipetype);
            }
        }
    }

    

}