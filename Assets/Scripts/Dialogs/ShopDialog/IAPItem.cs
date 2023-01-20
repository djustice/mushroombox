using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPItem : MonoBehaviour
{
    public void OnPurchaseProduct(Product product)
    {
        Game.counter.coinChange((int)product.definition.payout.quantity);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Animator anim = Game.shopDialog.GetComponent<Animator>();
        anim.SetBool("Idle", false);
        anim.SetBool("Extend", false);
        anim.SetBool("Retract", true);
        anim.SetBool("Idle", true);
    }
}
