using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    void Start()
    {
        
    }

    public void Escenario()
    {
        SceneManager.LoadScene("GameMap");
    }

    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }

    public void MenuInicio()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void NivelVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
    }
}
