using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private float value;

    public void Use()
    {
        Function();
    }

    protected virtual void Function()
    {
        Debug.Log("Doesnt do anything");
    }
}
