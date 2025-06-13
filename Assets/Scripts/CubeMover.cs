using System;
using System.Collections;
using UnityEngine;

public class CubeMover : MonoBehaviour // Codul pentru mutarea patratului in functie de directia data
{
    public float speed = 1f;
    public string directie = "right";
    void Start()
    {

    }
    void Update()
    {
        if (directie == "right")
            transform.position += Vector3.right * speed * Time.deltaTime;
        else if (directie == "up")
            transform.position += Vector3.up * speed * Time.deltaTime;
        else if (directie == "left")
            transform.position += Vector3.left * speed * Time.deltaTime;
        else if (directie == "down")
            transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void setDirectie(string val)
    {
        directie = val;
    }

    void OnTriggerEnter2D(Collider2D other)//Detectarea coliziunii cu un conveyor belt
    {
        ConveyorBelt belt = other.GetComponent<ConveyorBelt>();
        if (belt != null)
        {
            StartCoroutine(WaitAndSetDirection(belt.GetDirection(), 0.58f));
        }
    }
    private IEnumerator WaitAndSetDirection(string newDirection, float delay)
    {
        yield return new WaitForSeconds(delay);
        setDirectie(newDirection);
    }
}