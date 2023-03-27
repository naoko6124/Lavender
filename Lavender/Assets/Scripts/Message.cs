using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    TextMeshProUGUI message;
    
    void Start()
    {
        message = GetComponent<TextMeshProUGUI>();
    }

    public void ShowMessage(string text)
    {
        StartCoroutine(MessageCoroutine(text));
    }
    
    IEnumerator MessageCoroutine(string text)
    {
        message.text = text;
        yield return new WaitForSeconds(2.0f);
        message.text = "";
    }
}
