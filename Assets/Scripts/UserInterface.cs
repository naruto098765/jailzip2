using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{

    public GameObject panel;
    public GameObject start;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(true);
        start.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClick()
    {
        panel.SetActive(false);
        start.SetActive(false);


    }
}
