﻿using UnityEngine;
using TMPro;
public class VersionText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = $"{Application.version}";
    }
}