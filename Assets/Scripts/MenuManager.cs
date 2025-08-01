using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Called by the Reset Button
    public void ResetLevel()
    {
        Debug.Log("Reset Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}