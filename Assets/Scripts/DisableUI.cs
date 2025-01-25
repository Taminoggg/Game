using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{

    public GameObject gameObject;
    public GameObject gameObject2;

    void Start()
    {
        gameObject.SetActive(false);
        gameObject2.SetActive(false);
    }
}
