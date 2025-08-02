using UnityEngine;
using UnityEngine.SceneManagement;
public class ActionResetLevel : ActionBase
{
   
    public override void HandleAction()
    {
        Debug.Log("Reset Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //OnActionFinished?.Invoke(); does not make sense as we reset totally the scene
        //best option would be to reset state without reloading the scene but lack of time ...

    }

}

