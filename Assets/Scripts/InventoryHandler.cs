using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InventoryHandler : MonoBehaviour
{
    public string[] inventory;
    public int itemCount = 0;
    private string curObject;
    public GameObject curIcon;
    private int colCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        string[] inventory = new string[8];
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            curObject = collision.gameObject.name;
            Destroy(collision.gameObject);
            colCount = colCount + 1;
            AddToInv(); 
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        colCount = 0;
    }

    void AddToInv()
    {
        if (itemCount < inventory.Length && colCount == 1)
        {
            inventory[itemCount] = curObject;
            Debug.Log("Found new item: " + curObject);
            itemCount = itemCount + 1;
        }
    }
}
