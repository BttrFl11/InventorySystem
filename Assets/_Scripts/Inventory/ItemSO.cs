using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite Icon;
    public SlotTypes.Types SlotType;
    public float Weight;
    public int MaxStack;
}
