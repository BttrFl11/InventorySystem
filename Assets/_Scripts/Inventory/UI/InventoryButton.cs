using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private NPC _npc;

    private void Start()
    {
        _npc.Initialize();
    }

    public void OnButton()
    {
        var inventory = InventoryManager.Instance;
        if (inventory.Bag == _npc.BagInventory && inventory.IsOpen)
            return;

        inventory.SetPanelActive(true);
        inventory.Initialize(_npc.BagInventory, _npc.DollInventory);
    }
}
