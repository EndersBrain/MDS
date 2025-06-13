using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Storage : MonoBehaviour
{
    public Text Ramase;
    private bool[,] v;
    private bool[,,] rasp1 = new bool[10, 3, 3];
    private bool[,] c;
    private int cnt = 0;
    private bool trig = true;
    void Start()
    {
        v = new bool[3, 3];
        c = new bool[3, 3];
        cnt = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                System.Random random = new System.Random();
                if (random.Next(2) == 0)
                    c[i, j] = false;
                else
                    c[i, j] = true;
                c[i, j] = true;
            }
        }

    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log(other.tag);
        UnityEngine.Debug.Log("da collide cu storageul");
        if (other.CompareTag("Pick") && trig == true)
        {
            trig = false;
            StartCoroutine(Cooldown());
            //  UnityEngine.Debug.Log("Pick");
            //if (conveyorBelt != null)
            // {
            //    conveyorBelt.RemoveItem(other.transform);
            // }
            v = new bool[3, 3];
            BooleanGridRuntimeRenderer a = other.GetComponent<BooleanGridRuntimeRenderer>();
            bool ok = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    v[i, j] = a.getCell(i, j);
                    if (v[i, j] != c[i, j])
                    {
                        ok = false;
                    }
                }
            }
            if (ok)
            {
                cnt++;
                if (cnt < 10)
                {
                    UnityEngine.Debug.Log(cnt);
                    Ramase.text = (10 - cnt).ToString() + " obiecte ramase";
                }
                if (cnt == 10)
                {
                    Ramase.text = "Felicitari ai terminat toate obiectele";
                }
            }
        }
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        trig = true;
    }
}


