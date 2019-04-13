using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardInfo : MonoBehaviour


{
    private TextMeshPro textmeshPro;

    void Start()    
    {
		textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.SetText("3200 TL \n 1200TL \n 32.45 TL ");

       // KuveytApi cardInfo = new KuveytApi();
       // Debug.Log(cardInfo.getCreditCardInformation("4025916319964789"));
        
    }

	public void changeCardInfo(string limit){
            textmeshPro.SetText(limit);
        }

	// Update is called once per frame
	 void Update()
    {
        
    }
}
