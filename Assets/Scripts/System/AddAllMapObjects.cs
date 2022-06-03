using Assets.Scripts.DamagedObject;
using UnityEngine;

public class AddAllMapObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach(var item in FindObjectsOfType<DamagedObject>())
            Resources.AllObjects.Add(item);
    }
}
