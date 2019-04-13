using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textChange : MonoBehaviour
{
    TextMesh textMesh;

	void Start()
	{
		textMesh = GetComponent<TextMesh>();
		textMesh.text="3200 TL \n 1200TL \n 32.45 TL ";

		// KuveytApi cardInfo = new KuveytApi();
		// Debug.Log(cardInfo.getCreditCardInformation("4025916319964789"));

	}

    void Update()
    {
        
    }
}
