
using Firebase.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseNotification : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived   -= OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Tolken: " + token.Token);
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received Registration Tolken: " + e.Message.From);
    }  
}
