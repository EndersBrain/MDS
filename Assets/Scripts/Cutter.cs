using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Cutter : MonoBehaviour // Codul pentru taierea patratelor 3x3 in functie de tipul de cutter (Recycler sau Trasher)
{
    public GameObject squarePrefab;
    public GameObject squarePrefab1;
    public enum CutterType
    {
        Recycler, Trasher
    }
    public CutterType cutterType;
    private bool[,] v;
    private int[] dx = { 0, 0, 1, -1 };
    private int[] dy = { 1, -1, 0, 0 };
    private bool[,,] rasp1 = new bool[10, 3, 3];
    private List<(int, int)> rez = new List<(int, int)>();
    private Queue<(int, int)> q = new Queue<(int, int)>();
    private bool[,] c;
    private bool[,] taieri;
    private bool[,] taierinoi;
    private int cnt = 0;

    void Start()//Inceput, totul este initializat
    {
        v = new bool[3, 3];
        cnt = 0;
        taieri = new bool[3, 3] { { false, false, false }, { false, false, false }, { false, false, false } };
    }

    public void ToggleTaieri(int row, int col)//Taierile care se fac in functie de butoanele apasate in pop-up
    {
        taierinoi = taieri;
        if (row >= 0 && row < 3 && col >= 0 && col < 3)
        {
            taierinoi[row, col] = !taieri[row, col];
            taieri = taierinoi;
        }
    }

    void Update()
    {

    }
    private bool Valid(int x, int y)
    {
        if (x >= 0 && x < 3 && y >= 0 && y < 3)
        {
            return true;
        }
        return false;
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other) //Logica coleziunii dintre cutter si patratul 3x3
    {
        yield return new WaitForSeconds(0.6f);
        BigCubeController bigCube = other.GetComponent<BigCubeController>();
        if (bigCube == null)
            yield break;
        v = new bool[3, 3];
        cnt = 0;
        c = new bool[3, 3] { { false, false, false }, { false, false, false }, { false, false, false } };
        rez.Clear();
        q.Clear();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                v[i, j] = bigCube.visibleGrid[i, j];
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (v[i, j] && !c[i, j] && !taieri[i, j])
                {
                    rez.Add((i, j));
                    c[i, j] = true;
                    q.Enqueue((i, j));
                    bool ok = false;

                    while (q.Count > 0)
                    {
                        var tp = q.Dequeue();
                        for (int h = 0; h < 4; h++)
                        {
                            int x = tp.Item1 + dx[h];
                            int y = tp.Item2 + dy[h];
                            if (Valid(x, y))
                            {
                                if (v[x, y] && !c[x, y] && !taieri[x, y])
                                {
                                    ok = true;
                                    c[x, y] = true;
                                    rez.Add((x, y));
                                    q.Enqueue((x, y));
                                }
                            }
                        }
                    }

                    if (!ok && cutterType == CutterType.Trasher)
                        rez.Clear();
                }

                if (rez.Count > 0)
                {
                    if (cutterType == CutterType.Recycler)
                    {
                        foreach (var it in rez)
                        {
                            rasp1[cnt, it.Item1, it.Item2] = true;
                        }
                        cnt++;
                        rez.Clear();
                    }
                }
            }
        }

        if (cutterType == CutterType.Trasher)
        {
            foreach (var it in rez)
                rasp1[cnt, it.Item1, it.Item2] = true;
            cnt++;
        }

        GameObject g2 = null;
        if (cnt == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bigCube.visibleGrid[i, j] = rasp1[0, i, j];
                }
            }
            bigCube.UpdateVisibility();
        }

        if (cnt == 2)
        {
            g2 = Instantiate(squarePrefab1, transform.position + new Vector3(2, 2, 0), Quaternion.identity);
            g2.transform.SetParent(null);

            BigCubeController g2BigCube = g2.GetComponent<BigCubeController>();
            if (g2BigCube == null)
            {
                Debug.Log("null");
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        bigCube.visibleGrid[i, j] = rasp1[0, i, j];
                        g2BigCube.visibleGrid[i, j] = rasp1[1, i, j];
                    }
                }
                bigCube.UpdateVisibility();
                g2BigCube.UpdateVisibility();
            }
        }


        CameraController camController = Object.FindFirstObjectByType<CameraController>();// Verificarea daca exista un conveyor belt in locul unde ar trebui sa fie taiat patratul
        if (camController == null)
            yield break;

        Vector3 cutterPos = camController.GetSnappedPosition(transform.position);
        Vector3 objectPos = camController.GetSnappedPosition(other.transform.position);

        Vector3 delta = objectPos - cutterPos;
        Vector3 oppositePos = cutterPos - delta;
        Vector3 targetPos = camController.GetSnappedPosition(oppositePos);

        bool foundConveyor = false;
        GameObject[] conveyors = GameObject.FindGameObjectsWithTag("Snappable");
        foreach (GameObject conveyor in conveyors)
        {
            Vector3 conveyorPos = camController.GetSnappedPosition(conveyor.transform.position);
            if (Vector3.Distance(conveyorPos, targetPos) < 0.01f)
            {
                foundConveyor = true;
                break;
            }
        }
        if (!foundConveyor)
            Destroy(other.gameObject);
        rasp1 = new bool[10, 3, 3];
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(ReenableTrigger());
    }
    private IEnumerator ReenableTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider2D>().enabled = true;
    }
}

