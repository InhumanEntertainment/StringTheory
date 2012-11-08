/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Store : MonoBehaviour 
{
    public List<StringTheoryProduct> Products;

#if UNITY_IPHONE
	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
	}

#endif
}

[System.Serializable]
public class StringTheoryProduct
{
    public string Name = "";
    public string ID = "";

    bool _Purchased = false;
    public bool Purchased
    {
        get{ return _Purchased; }
        set
        {
            _Purchased = value;

            if (value)
            {
                if (ID == "com.inhuman.stringtheory.noads")
                {
                    // Disable UI adbox //
                    MobclixBinding.hideBanner(true);

                    // Unlock Some Packs //
                    // Find gameobject in packs screen //
                    // 
                }
            }
        }
    }
}
*/
