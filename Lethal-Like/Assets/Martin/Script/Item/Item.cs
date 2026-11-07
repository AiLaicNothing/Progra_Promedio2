using UnityEngine;

public class Item : MonoBehaviour, IPickable
{
    [SerializeField] private string _name;
    [SerializeField] private float value;

    public int Value => Mathf.RoundToInt(value);

    public string GetItemName()
    {
        return _name;
    }

    public int GetItemValue()
    {
        return Value;
    }

    public void Use()
    {
        Function();
    }

    protected virtual void Function()
    {
        Debug.Log("Doesn't do anything");
    }
}
