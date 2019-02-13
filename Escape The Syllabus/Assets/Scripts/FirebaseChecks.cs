using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using Completed;

public class FirebaseChecks : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LoginMenu;
    public GameObject LoginRegisterFeedback;
    public GameObject RegisterMenu;
    public GameObject OptionsMenu;
    public GameObject StartMenu;
    public GameObject DatabaseUtil;
    public GameObject GameManager;
    private InputField username;
    private InputField password;
    private InputField repassword;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Auth.FirebaseAuth auth;
    private string currentMessage;
    private string lastMessage;
    private bool isNewUser;

    void Start() {
        InitializeFirebase();
        DontDestroyOnLoad(gameObject);
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

        // update level in firebase
        int level = GameManager.GetComponent<GameManager>().GetLevel();
        DatabaseUtil database = DatabaseUtil.GetComponent<DatabaseUtil>();
        if (database != null && user != null) {
          database.updateCurrentLevel(user.UserId, level);
          Debug.Log("updated level " + level);
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
                Invoke("HideFeedback", 3);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "Succesfully logged in";
                LoginRegisterFeedback.SetActive(true);
                Invoke("HideFeedback", 3);

                // forget username and password combo after sign in
                GameObject usernameinput = GameObject.Find("UsernameInput");
                if (usernameinput != null) {
                  usernameinput.GetComponent<InputField>().text= "";
                }
                GameObject passInput = GameObject.Find("PasswordInput");
                if (passInput != null) {
                  passInput.GetComponent<InputField>().text ="";
                }
                DatabaseUtil database = DatabaseUtil.GetComponent<DatabaseUtil>();

                // GameManager.GetComponent<GameManager>().SetLevel(database.getCurrentLevel(user.UserId));


                //this registers the user in the database for a first time login
                if (isNewUser)
                {
                    GameObject repassInput = GameObject.Find("RePassInput");
                    if (repassInput != null) {
                      repassInput.GetComponent<InputField>().text= "";
                    }
                    database.writeNewUser(user.UserId, user.Email);
                    isNewUser = false;
                    Debug.Log("user added to DB");
                }

                // switch screens
                LoginMenu.SetActive(false);
                RegisterMenu.SetActive(false);
                MainMenu.SetActive(true);
            }
        }
    }

    void HideFeedback()
    {
      LoginRegisterFeedback.SetActive(false);
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
        isNewUser = true;
        Debug.Log("enter register method");
        username = GameObject.Find("UsernameInput").GetComponent<InputField>();
        password = GameObject.Find("PasswordInput").GetComponent<InputField>();
        repassword = GameObject.Find("RePassInput").GetComponent<InputField>();
        if (password.text != repassword.text)
        {
            LoginRegisterFeedback.GetComponent<TextMeshProUGUI>().text = "passwords do not match";
            LoginRegisterFeedback.SetActive(true);
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(username.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");
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
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
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
            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = null;
            newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void Logout()
    {
        auth.SignOut();
        OptionsMenu.SetActive(false);
        StartMenu.SetActive(true);
    }
}
