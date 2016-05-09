using UnityEngine;
using System.Collections;


public class NotDestroy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        DontDestroyOnLoad(gameObject);
    }

}
