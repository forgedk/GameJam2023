using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadNextScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loadNextScene() {



        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("MainMenu");
    
    }
}
