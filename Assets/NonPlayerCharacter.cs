using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{

    public float displayTime = 4f;
    public GameObject dialogBox;
    float timerDisplay;
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >=0) // check to see if dialog is being displayed
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0) // if time has run out, disable dialog
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}
