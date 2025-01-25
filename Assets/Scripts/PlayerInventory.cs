using System;
using InventorySystem.Assets;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    private GameObject _inventoryUI;

    private void Start()
    {
        _inventoryUI = GameObject.FindWithTag("InventoryScreen");
        _inventoryUI.SetActive(false);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (!item) return;
        var _item = new Item(item.item);
        Debug.Log(_item.Id);
        inventory.AddItem(_item, 1);
        Destroy(other.gameObject);
    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
       {
           _inventoryUI.SetActive(!_inventoryUI.activeSelf);
           PauseMenu.SetState(PauseMenu.GetState() == State.Inventory ? State.Play : State.Inventory);
       }     
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[28];
    }
}
