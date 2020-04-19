using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform Player;
    public float speed = 5.0f;
    public float MAX_OFFSET = 0.5f;
    public float SENSETIVITY = 4.0f;
    private bool isMoving = false;
    private bool isAiming = false;
    private int moveCounter = 0;
    private int moveTouch = 0;
    private int moveTouchId = 0;
    private int aimTouch = 0;
    private int aimTouchId = 0;
    private int aimCounter = 0;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 pointC;
    private Vector2 pointD;

    public Transform circle;
    public Transform outerCircle;

    private int FindId(int id)
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            if(id == Input.GetTouch(i).fingerId)
            {
                return i;
            }
        }
        return -1;
    }

    private void IntitializeMoveStick()
    {
        if (Input.touchCount > 0 && !isMoving)
        {
            moveCounter = Input.touchCount;
            for (int i = 0; i < moveCounter; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2 && touch.position.y < Screen.height / 2)
                {
                    isMoving = true;
                    moveTouch = i;
                    moveTouchId = Input.touches[i].fingerId;
                    break;
                }
                else if(i == Input.touchCount - 1)
                {
                    return;
                }
            }
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[moveTouch].position.x, Input.touches[moveTouch].position.y, Camera.main.transform.position.z));

            circle.transform.position = pointA;
            outerCircle.transform.position = pointA;
            circle.GetComponent<SpriteRenderer>().enabled = true;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (isMoving)
        {
            if(moveCounter != Input.touchCount)
            {
                moveTouch = FindId(moveTouchId);
                moveCounter = Input.touchCount;
            }
            if (Input.touches[moveTouch].phase == TouchPhase.Ended || Input.touches[moveTouch].phase == TouchPhase.Canceled || moveTouch < 0)
            {
                isMoving = false;
            }
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[moveTouch].position.x, Input.touches[moveTouch].position.y, Camera.main.transform.position.z));
        }
    }

    void Move()
    {
        Vector2 direction;
        if (isMoving)
        {
            Vector2 offset = pointB - pointA;
            direction = Vector2.ClampMagnitude(offset * SENSETIVITY, 1.0f);

            circle.transform.position = pointA + Vector2.ClampMagnitude(offset, MAX_OFFSET);
        }
        else
        {
            direction = Vector2.zero;
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }
        Player.GetComponent<PlayerController>().ProcessInputs(direction);
    }

    void CreateAimingVector()
    {
        if (Input.touchCount > 0 && !isAiming)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
                {
                    isAiming = true;
                    aimTouch = i;
                    aimTouchId = Input.touches[i].fingerId;
                    aimCounter = Input.touchCount;
                    break;
                }
                else if (i == Input.touchCount - 1)
                {
                    Player.GetComponent<PlayerController>().Aim(Vector2.zero);
                    return;
                }
            }
            pointC = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[aimTouch].position.x, Input.touches[aimTouch].position.y, Camera.main.transform.position.z));
        }
        if (isAiming)
        {
            if (aimCounter != Input.touchCount)
            {
                aimTouch = FindId(aimTouchId);
                aimCounter = Input.touchCount;
            }
            if (Input.touches[aimTouch].phase == TouchPhase.Ended || Input.touches[aimTouch].phase == TouchPhase.Canceled || aimTouch < 0)
            {
                isAiming = false;
                Player.GetComponent<PlayerController>().Aim(Vector2.zero);
                return;
            }
            pointD = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[aimTouch].position.x, Input.touches[aimTouch].position.y, Camera.main.transform.position.z));
            Vector2 aim = pointC - pointD;
            Player.GetComponent<PlayerController>().Aim(aim);
        }

    }

    // Update is called once per frame
    void Update()
    {
        IntitializeMoveStick();
        CreateAimingVector();
    }
    private void FixedUpdate()
    {
        Move();
    }
}