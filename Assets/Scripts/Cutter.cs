using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Cutter : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject squarePrefab1;
    public Transform endPoint;
    public enum CutterType
    {
        Recycler, Trasher
    }
    public CutterType cutterType;
    private bool[,] v;
    private int[] dx = {0, 0, 1, -1};
    private int[] dy = {1, -1, 0, 0};
    private bool[,,] rasp1 = new bool[10, 3, 3];
    private List<(int, int)> rez = new List<(int, int)>();
    private Queue<(int, int)> q = new Queue<(int, int)>();
    private bool[,] c;
    private bool[,] taieri;
    private bool[,] taierinoi;
    private int cnt = 0;

    void Start()
    {
        v = new bool[3, 3];
        cnt = 0;
        taieri = new bool[3, 3] {{false, false, false}, {false, false, false}, {false, false, false}};
        endPoint = GameObject.Find("EndPoint").transform;
    }

    public void ToggleTaieri(int row, int col)
    {
        taierinoi = taieri;
        if (row >= 0 && row < 3 && col >= 0 && col < 3)
        {
            taierinoi[row, col] = !taieri[row, col]; // Inversează valoarea
            taieri = taierinoi;
        }
    }

    void Update()
    {

    }
    private bool Valid(int x, int y)
    {
        if(x >= 0 && x < 3 && y >= 0 && y < 3)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log(other.tag);
        if(other.CompareTag("Pick"))
        {
            UnityEngine.Debug.Log("Pick");
            ConveyorBelt conveyorBelt = Object.FindFirstObjectByType<ConveyorBelt>();
            if (conveyorBelt != null)
            {
                conveyorBelt.RemoveItem(other.transform);
            }
            v = new bool[3, 3];
            cnt = 0;
            c = new bool[3, 3]{{false, false, false}, {false, false, false}, {false, false, false}};
            rez.Clear();
            q.Clear();
            BooleanGridRuntimeRenderer a = other.GetComponent<BooleanGridRuntimeRenderer>();
            for(int i = 0; i < 3; i ++)
            {
                for(int j = 0; j < 3; j ++)
                {
                    v[i, j] = a.getCell(i, j);
                    //v[i, j] = true;
                }
            }
            for(int i = 0; i < 3; i ++)
            {
                for(int j = 0; j < 3; j ++)
                {
                    if(v[i, j] && !c[i, j] && !taieri[i, j])
                    {
                        rez.Add((i, j));
                        c[i, j] = true;
                        q.Enqueue((i, j));
                        bool ok = false;
                        while(q.Count > 0)
                        {
                            var tp = q.Dequeue();
                            for(int h = 0; h < 4; h ++)
                            {
                                int x = tp.Item1 + dx[h];
                                int y = tp.Item2 + dy[h];
                                if(Valid(x, y))
                                {
                                    if(v[x, y] && !c[x, y] && !taieri[x, y])
                                    {
                                        ok = true;
                                        c[x, y] = true;
                                        rez.Add((x, y));
                                        q.Enqueue((x, y));
                                    }
                                }
                            }
                        }
                        if(!ok && cutterType == CutterType.Trasher)
                            rez.Clear();
                    }
                    if(rez.Count > 0)
                    {
                        if(cutterType == CutterType.Recycler)
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
        
            if(cutterType == CutterType.Trasher)
            {
                foreach(var it in rez)
                    rasp1[cnt, it.Item1, it.Item2] = true;
                cnt ++;
            }
            Vector3 origin = transform.position;
            other.transform.position = origin + new Vector3(1, 0, 0);
            GameObject g2 = null;
            if(cnt == 1)
            {
                for(int i = 0; i < 3; i ++)
                    for(int j = 0; j < 3; j ++)
                        other.GetComponent<BooleanGridRuntimeRenderer>().setCell(i, j, rasp1[0, i, j]);
            }
            if(cnt == 2)
            {
                g2 = Instantiate(squarePrefab1, origin + new Vector3(2, 2, 0), Quaternion.identity);
                g2.transform.SetParent(null);
                for(int i = 0; i < 3; i ++)
                {
                    for(int j = 0; j < 3; j ++)
                    {

                        other.GetComponent<BooleanGridRuntimeRenderer>().setCell(i, j, rasp1[0, i, j]);
                        g2.GetComponent<BooleanGridRuntimeRenderer>().setCell(i, j, rasp1[1, i, j]);
                    }
                }
            }
            other.GetComponent<BooleanGridRuntimeRenderer>().UpdateGridRendering();
            if(cnt > 1)
            {
                Wait();
                g2.GetComponent<BooleanGridRuntimeRenderer>().UpdateGridRendering();
            }
            rasp1 = new bool[10, 3, 3];
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(ReenableTrigger());
        }
    }
    private IEnumerator ReenableTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider2D>().enabled = true;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.1f);
    }
}

