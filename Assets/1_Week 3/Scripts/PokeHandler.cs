using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeHandler : MonoBehaviour
{
    [SerializeField] GameObject infoUI;
    public void OnPoke()
    {
        Debug.Log("Object was poked!");
        GetComponent<Renderer>().material.color = Color.red;
        infoUI.SetActive(true);
    }
}