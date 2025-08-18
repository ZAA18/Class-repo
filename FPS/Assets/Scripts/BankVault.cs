using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankVault : MonoBehaviour
{
    public Vector3 endPos; // Position the vault door moves to when open
    public float speed = 1.0f; // Opening/closing speed

    private bool moving = false;
    private bool opening = true;
    private Vector3 startPos;
    private float delay = 0f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (moving)
        {
            if (opening)
            {
                MoveVault(endPos);
            }
            else
            {
                MoveVault(startPos);
            }
        }
    }

    void MoveVault(Vector3 goalPos)

    {
        float dist = Vector3.Distance(transform.position, goalPos);

        if (dist > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, goalPos, speed * Time.deltaTime);
        }
        else
        {
            if (opening)
            {
                delay += Time.deltaTime;
                if (delay > 1.5f)
                {
                    opening = false;
                }
            }
            else
            {
                moving = false;
                opening = true;
                delay = 0f;
            }
        }
    }

    public bool Moving
    {
        get { return moving; }
        set { moving = value; }
    }
}
