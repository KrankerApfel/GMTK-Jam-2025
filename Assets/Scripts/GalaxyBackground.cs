using UnityEngine;
public class GalaxyBackground : MonoBehaviour
{
    [SerializeField]
    private Material galaxyMaterial;

    void Start()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        galaxyMaterial.SetVector("_Resolution", new Vector4(resolution.x, resolution.y, 0, 0));
    }

}
