using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;

    private Vector2 pointerPosition;

    private Vector2 smoothDirection;

    private bool touched;
    private int pointerID;
    private bool canFire;

    private bool firstTouch = false;

    void Awake()
    {
        direction = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        // set our start point
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            //origin = data.position;

            this.pointerPosition = data.position;

            this.canFire = true;
        }

        firstTouch = true;
    }

    public void OnDrag(PointerEventData data)
    {
        // compare the differnce between our start point and current pointer pos
        if (data.pointerId == pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;

            this.pointerPosition = data.position;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        // reset everything
        if (data.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;

            this.canFire = false;
        }
    }

    public Vector2 GetDirection()
    {
        //smoothDirection = Vector2.MoveTowards(smoothDirection,direction,smoothing);

        return this.direction;
    }

    public Vector2 GetPointerPosition()
    {
        return this.pointerPosition;
    }

    public bool HasGotFirstTouch()
    {
        return this.firstTouch;
    }

    public bool CanFire()
    {
        return canFire;
    }

}
