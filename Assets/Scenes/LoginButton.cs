using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    public void handleClick()
    {
        var url = "https://idprep.kuveytturk.com.tr/api/connect/authorize?client_id=3dead4890b724c3dac70a3f6f6c4fea6&response_type=code&redirect_uri=http://vr-vr&scope=cards%20offline_access";
        Application.OpenURL(url);
    }
}
