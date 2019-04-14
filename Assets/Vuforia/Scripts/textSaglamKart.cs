using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textSaglamKart : MonoBehaviour
{
	TextMesh textMesh;
	KuveytApi cardInfo = new KuveytApi();

	void Start()
	{
		CardInformation card = cardInfo.getCreditCardInformation("4025916319964789");
		textMesh = GetComponent<TextMesh>();
		//textMesh.text = card.Limit + " TL \n" + card.PointAmount + " TL \n" + card.InstallmentCount + " TL";
        textMesh.text = " fdsfdasfdsa TL \n TLfdsfadsfdsa \n fdsfdsaTL";


	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
