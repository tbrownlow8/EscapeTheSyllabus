using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;

public class FirebaseChecks : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LoginMenu;
    public GameObject LoginRegisterFeedback;
    private InputField username;
    private InputField password;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Auth.FirebaseAuth auth;

    void Start() {
        username = GameObject.Find("UsernameInput").GetComponent<InputField>();
        password = GameObject.Find("PasswordInput").GetComponent<InputField>();

        InitializeFirebase();
    }



    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "Signed out";
                LoginRegisterFeedback.SetActive(true);


            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "Succesfully logged in";
                LoginRegisterFeedback.SetActive(true);
            }
        }
    }

    public void login()
    {
        auth.SignInWithEmailAndPasswordAsync(username.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "canceled";
                LoginRegisterFeedback.SetActive(true);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception.ToString());
                LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = task.Exception.ToString();
                 LoginRegisterFeedback.SetActive(true);
                 return;







            }



            AuthStateChanged(this, null);
            user = task.Result;
            Debug.LogFormat("User signed in successfully.");
            LoginMenu.SetActive(false);
            MainMenu.SetActive(true);
        }

  );


    }




}
