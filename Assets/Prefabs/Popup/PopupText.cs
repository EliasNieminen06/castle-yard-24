using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.2f);

        transform.rotation = Camera.main.transform.rotation;
    }
}
