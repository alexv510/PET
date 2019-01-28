using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Gameplay");
    }
}