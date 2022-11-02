using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public void SwitchPanelActivity()
    {
        var inventory = InventoryManager.Instance;

        if(inventory.Bag != null)
        {
            bool active = inventory.IsOpen;
            inventory.SetPanelActive(!active);
        }
    }
}
