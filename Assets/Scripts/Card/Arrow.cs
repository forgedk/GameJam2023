using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Arrow : MonoBehaviour
{
    public TMP_Text damageText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void setText(string text)
    {
        damageText.text = text;
    }

    public void setColor(Color color)
    {
        gameObject.GetComponent<UnityEngine.UI.Image>().color = color;
    }
}
