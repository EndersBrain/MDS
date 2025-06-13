using UnityEngine;

public class Spawnerscript : MonoBehaviour
{
    public GameObject squarePrefab;
    public Transform spawnPoint;
    public float spawnInterval;
    private float nextSpawnTime;
    public ConveyorBelt conveyorBelt;
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }
    void Update()
    {
        if(Time.time > nextSpawnTime)
        {
            //GameObject spawnObject = Instantiate(squarePrefab, spawnPoint.position, spawnPoint.rotation);
            if(conveyorBelt != null)
            {
                GameObject spawnObject = Instantiate(squarePrefab, spawnPoint.position, spawnPoint.rotation);
                conveyorBelt.AddItem(spawnObject.transform);
            }
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (conveyorBelt == null)
        {
            ConveyorBelt belt = other.GetComponentInParent<ConveyorBelt>();
            if (belt != null)
            {
                conveyorBelt = belt;
                Debug.Log("Assigned ConveyorBelt: " + belt.name);
            }
        }
    }

}