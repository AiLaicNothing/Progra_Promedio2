using UnityEngine;

public class Sell_Button : MonoBehaviour
{
    [SerializeField] private Seller seller;

    public void Interact()
    {
        if (seller == null) return;

        seller.SellItems();
    }
}
