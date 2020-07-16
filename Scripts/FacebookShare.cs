using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookShare : MonoBehaviour
{
    //public Text userIdText;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    /*
    public void LogIn()
    {
        FB.LogInWithReadPermissions(callback: OnLogIn);
    }

    private void OnLogIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken token = AccessToken.CurrentAccessToken;
            userIdText.text = token.UserId;
        }
        else
        {
            Debug.Log("Canceled Login");
        }
    }
    */

    public void Share()
    {
        FB.ShareLink(
            new System.Uri("http://virusAttack.altervista.org"),
            callback: OnShare
            );
    }

    private void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else
        {
            Debug.Log("Share succeed");
        }
    }
}
