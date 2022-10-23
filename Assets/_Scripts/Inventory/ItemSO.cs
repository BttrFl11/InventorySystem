using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite Icon;
    public float Weight;
    public int StackSize;
}
