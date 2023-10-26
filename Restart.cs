using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void ReStart( )
    {
        SceneManager.LoadScene("test");
    }
}
