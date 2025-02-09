using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universals : MonoBehaviour
{
    public static Universals Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
