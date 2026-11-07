
using UnityEngine;
using TMPro;

public class Pickup_UI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemValueText;

    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Transform target;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public void ShowPickup(IPickable item, Transform itemTransform)
    {
        target = itemTransform;

        itemNameText.text = item.GetItemName();
        itemValueText.text = "Value: $" + item.GetItemValue();

        gameObject.SetActive(true);
    }

    public void HidePickup()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
