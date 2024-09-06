using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] private Button StartHostButton;
    [SerializeField] private Button StartClientButton;

    // Start is called before the first frame update
    void Start()
    {
        StartHostButton.onClick.AddListener(()=>
        {
            Debug.Log("SERVER START");
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        StartClientButton.onClick.AddListener(()=>
        {
            Debug.Log("CLIENT START");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void OnDestroy()
    {
        Debug.Log("GameManager DESTROYED");
        NetworkManager.Singleton.Shutdown();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}