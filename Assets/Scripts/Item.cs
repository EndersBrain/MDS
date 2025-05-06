using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public ActionType actionType;
    public Sprite image;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool stackable = true;
    public int itemCost = 0;
    public GameObject prefabToPlace; 
}

public enum ItemType
{
    Building,
    Tool
}

public enum ActionType
{
    Delete,
    SpawnItem,
    Cut,
    ReceiveItem,
    SpawnPoint,

    Rotate
}