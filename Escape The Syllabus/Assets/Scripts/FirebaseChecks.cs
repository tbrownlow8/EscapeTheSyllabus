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
    public GameObject RegisterMenu;
    public GameObject OptionsMenu;
    public GameObject StartMenu;
    private InputField username;
    private InputField password;
    private InputField repassword;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Auth.FirebaseAuth auth;
    private string currentMessage;
    private string lastMessage;

    void Start() {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void Update()
    {
        // Only update feedback if error message changed
        if ((lastMessage == null && currentMessage != null) || (lastMessage != null && currentMessage != null && !lastMessage.Equals(currentMessage)))
        {
            LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = currentMessage;
            lastMessage = currentMessage;
            LoginRegisterFeedback.SetActive(true);

        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
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

                // switch screens
                LoginMenu.SetActive(false);
                MainMenu.SetActive(true);
            }
        }
    }

    public void Login()
    {
        // retrieve username and password
        username = GameObject.Find("UsernameInput").GetComponent<InputField>();
        password = GameObject.Find("PasswordInput").GetComponent<InputField>();
        // attempt to login
        auth.SignInWithEmailAndPasswordAsync(username.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                lastMessage = currentMessage;

                // currently only displays 1 exception
                // might need to append to string instead of replacing
                foreach (System.Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        currentMessage = firebaseEx.Message;
                        AuthStateChanged(this, null);
                    }
                }
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception.ToString());
                lastMessage = currentMessage;

                // currently only displays 1 exception
                // might need to append to string instead of replacing
                foreach (System.Exception exception in task.Exception.Flatten().InnerExceptions) {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        currentMessage = firebaseEx.Message;
                        AuthStateChanged(this, null);
                    }
                }
                return;
            }
            // successful login
            AuthStateChanged(this, null);
            user = task.Result;
            Debug.LogFormat("User signed in successfully.");
        });
    }

    public void Register()
    {
        username = GameObject.Find("UsernameInput").GetComponent<InputField>();
        password = GameObject.Find("PasswordInput").GetComponent<InputField>();
        repassword = GameObject.Find("RePassInput").GetComponent<InputField>();
        if (password.text != repassword.text)
        {
            LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "passwords do no match";
            LoginRegisterFeedback.SetActive(true);
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(username.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                print("error");
                return;
            }
            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            DatabaseUtil database = gameObject.GetComponent<DatabaseUtil>();
            database.writeNewUser(newUser.UserId, newUser.DisplayName);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });
        RegisterMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Logout()
    {
        auth.SignOut();

        OptionsMenu.SetActive(false);
        StartMenu.SetActive(true);
    }
}
