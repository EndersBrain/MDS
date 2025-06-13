using System;
using Unity.VisualScripting;
using UnityEngine;

public enum ConveyorType
{
    Horizontal,
    Vertical,
}

public class ConveyorBelt : MonoBehaviour//Codul pentru conveyor belt
{
    public ConveyorType tip = ConveyorType.Vertical;
    public bool esteCurba = false;
    public String directieCurba = "";
    public Sprite curveSprite;
    public Material defaultMaterial;
    private void Start()
    {
        CheckDirectie();
        CautaConveyor();
        CheckSpawner();
    }
    void CheckDirectie()//Verifica directia in care se afla conveyorul
    {
        float rotatie = transform.eulerAngles.z;
        rotatie = Mathf.Round(rotatie) % 360;
        if (rotatie == 0 || rotatie == 180)
            tip = ConveyorType.Vertical;
        else if (rotatie == 90 || rotatie == 270)
            tip = ConveyorType.Horizontal;
    }
    void CautaConveyor()///Verifica casutele de sus, jos stanga si dreapta pentru a verifica daca exista un alt conveyor belt in apropiere
    {
        CheckDirectie();
        float smallBias = 0.01f;
        Vector2 upPos = (Vector2)transform.position + Vector2.up * (1f - smallBias);
        Vector2 downPos = (Vector2)transform.position + Vector2.down * (1f - smallBias);
        Vector2 leftPos = (Vector2)transform.position + Vector2.left * (1f - smallBias);
        Vector2 rightPos = (Vector2)transform.position + Vector2.right * (1f - smallBias);
        float overlapRadius = 0.2f;
        Collider2D colliderUp = Physics2D.OverlapCircle(upPos, overlapRadius);
        Collider2D colliderDown = Physics2D.OverlapCircle(downPos, overlapRadius);
        Collider2D colliderLeft = Physics2D.OverlapCircle(leftPos, overlapRadius);
        Collider2D colliderRight = Physics2D.OverlapCircle(rightPos, overlapRadius);
        String raspuns = GetDirectionFromRotation();
        if (tip == ConveyorType.Vertical)
        {
            if (colliderUp != null)
            {
                ConveyorBelt beltUp = colliderUp.GetComponent<ConveyorBelt>();
                if (beltUp != null && beltUp.esteCurba == false)
                {
                    beltUp.CheckDirectie();
                    if (beltUp.tip == ConveyorType.Horizontal) /// a fost gasit un vecin orizontal pentru un conveyor vertical, asa ca vecinul se transofrma in curba
                        ConvertToCurve(beltUp, Vector2.up, raspuns);
                }
            }
            if (colliderDown != null)
            {
                ConveyorBelt beltDown = colliderDown.GetComponent<ConveyorBelt>();
                if (beltDown != null && beltDown.esteCurba == false)
                {
                    beltDown.CheckDirectie();
                    if (beltDown.tip == ConveyorType.Horizontal) /// a fost gasit un vecin orizontal pentru un conveyor vertical, asa ca vecinul se transofrma in curba
                        ConvertToCurve(beltDown, Vector2.down, raspuns);
                }
            }
        }
        else if (tip == ConveyorType.Horizontal)
        {
            if (colliderLeft != null)
            {
                ConveyorBelt beltLeft = colliderLeft.GetComponent<ConveyorBelt>();
                if (beltLeft != null && beltLeft.esteCurba == false)
                {
                    beltLeft.CheckDirectie();
                    if (beltLeft.tip == ConveyorType.Vertical)/// similar
                        ConvertToCurve(beltLeft, Vector2.left, raspuns);
                }
            }
            if (colliderRight != null)
            {
                ConveyorBelt beltRight = colliderRight.GetComponent<ConveyorBelt>();
                if (beltRight != null && beltRight.esteCurba == false)
                {
                    beltRight.CheckDirectie();
                    if (beltRight.tip == ConveyorType.Vertical) ///similar
                        ConvertToCurve(beltRight, Vector2.right, raspuns);
                }
            }
        }
    }
    void ConvertToCurve(ConveyorBelt neighbor, Vector2 directionToNeighbor, String x)
    {
        neighbor.esteCurba = true;
        SpriteRenderer neighborSR = neighbor.GetComponent<SpriteRenderer>();
        neighborSR.sprite = curveSprite;
        neighborSR.material = defaultMaterial;
        float smallBias = 0.01f;
        float checkDistance = 1.0f;
        float overlapRadius = 0.2f;
        string hasOtherNeighbor = "false";
        if (neighbor.tip == ConveyorType.Vertical)
        {
            Vector2 upPos = (Vector2)neighbor.transform.position + Vector2.up * (checkDistance - smallBias);
            Vector2 downPos = (Vector2)neighbor.transform.position + Vector2.down * (checkDistance - smallBias);
            Collider2D colUp = Physics2D.OverlapCircle(upPos, overlapRadius);
            Collider2D colDown = Physics2D.OverlapCircle(downPos, overlapRadius);
            if (colUp != null && colUp.gameObject != this.gameObject)
                hasOtherNeighbor = "up";
            else if (colDown != null && colDown.gameObject != this.gameObject)
                hasOtherNeighbor = "down";
        }
        else if (neighbor.tip == ConveyorType.Horizontal)
        {
            Vector2 leftPos = (Vector2)neighbor.transform.position + Vector2.left * (checkDistance - smallBias);
            Vector2 rightPos = (Vector2)neighbor.transform.position + Vector2.right * (checkDistance - smallBias);
            Collider2D colLeft = Physics2D.OverlapCircle(leftPos, overlapRadius);
            Collider2D colRight = Physics2D.OverlapCircle(rightPos, overlapRadius);
            if (colLeft != null && colLeft.gameObject != this.gameObject)
                hasOtherNeighbor = "left";
            else if (colRight != null && colRight.gameObject != this.gameObject)
                hasOtherNeighbor = "right";
        }
        Debug.Log(hasOtherNeighbor);
        ///cauta directia in care se afla vecinul, iar curba se va orienta in functie de aceasta
        if (tip == ConveyorType.Horizontal && neighbor.tip == ConveyorType.Vertical)
        {
            if (hasOtherNeighbor == "down")
            {
                if (directionToNeighbor == Vector2.left)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, 90);
                    neighbor.transform.localScale = new Vector3(-1, 1, 1);
                    //Debug.Log("Pica1");
                }
                else if (directionToNeighbor == Vector2.right)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, -90);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("Pica2");
                }
            }
            else
            {
                if (directionToNeighbor == Vector2.left)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, 90);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    // Debug.Log("pica3");
                }
                else if (directionToNeighbor == Vector2.right)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, 180);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("pica4");
                }
            }
        }
        else if (tip == ConveyorType.Vertical && neighbor.tip == ConveyorType.Horizontal)
        {
            if (hasOtherNeighbor == "left")
            {
                if (directionToNeighbor == Vector2.up)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, -90);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("Pica5");
                }
                else if (directionToNeighbor == Vector2.down)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, -90);
                    neighbor.transform.localScale = new Vector3(-1, 1, 1);
                    //Debug.Log("Pica6");
                }
            }
            else
            {
                if (directionToNeighbor == Vector2.up)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, 0);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("Pica7");
                }
                else if (directionToNeighbor == Vector2.down)
                {
                    neighbor.transform.eulerAngles = new Vector3(0, 0, -270);
                    neighbor.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("Pica8");
                }
            }
        }
        neighbor.tip = tip;
        neighbor.directieCurba = x; /// directia in care curba muta obiectele este retinuta din obiectul curent
        string dirNeighbor = neighbor.GetDirectionFromRotation();
        foreach (Transform child in neighbor.transform)
        {
            CubeMover mover = child.GetComponent<CubeMover>();
            if (mover != null)
                mover.setDirectie(dirNeighbor);
        }
    }
    void CheckSpawner()
    { /// verifica daca exista un spawner sus, jos, dreapta sau in stanga, iar daca exista, spawnerul va genera obiecte in ordinea conveyorului
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (var dir in directions)
        {
            Vector2 checkPos = (Vector2)transform.position + dir * 1f;
            Collider2D collider = Physics2D.OverlapPoint(checkPos);
            if (collider != null && collider.CompareTag("Spawner"))
            {
                Spawner spawner = collider.GetComponent<Spawner>();
                spawner.OnObjectSpawned += HandleSpawnedObject;
            }
        }
    }
    void HandleSpawnedObject(GameObject spawnedObj)
    { /// ajusteaza directia obiectului in functie de directia conveyorullui
        if (this == null || gameObject == null)
            return;
        spawnedObj.transform.parent = transform;
        CubeMover mover = spawnedObj.GetComponent<CubeMover>();
        if (mover != null)
        {
            string direction = GetDirectionFromRotation();
            mover.setDirectie(direction);
        }
    }
    public string GetDirectionFromRotation()
    {/// sea gaseste directia in care se afla conveyorul, in functie de rotatia acestuia
        float rotatie = transform.eulerAngles.z;
        rotatie = Mathf.Round(rotatie) % 360;
        bool isFlipped = transform.localScale.x < 0;
        if (esteCurba)
            return directieCurba;
        else
        {
            if (rotatie == 0)
                return isFlipped ? "down" : "up";
            else if (rotatie == 90)
                return isFlipped ? "right" : "left";
            else if (rotatie == 180)
                return isFlipped ? "up" : "down";
            else if (rotatie == 270)
                return isFlipped ? "left" : "right";
            else
                return isFlipped ? "down" : "up";
        }
    }
    public string GetDirection()
    {
        return GetDirectionFromRotation();
    }
}
