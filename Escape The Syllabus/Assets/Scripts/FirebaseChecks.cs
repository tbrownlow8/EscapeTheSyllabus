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
using UnityEngine.SceneManagement;

public class FirebaseChecks : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LoginMenu;
    public GameObject LoginRegisterFeedback;
    public GameObject RegisterMenu;
    public GameObject OptionsMenu;
    public GameObject StartMenu;
    public GameObject DatabaseUtil;
    private InputField username;
    private InputField password;
    private InputField repassword;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Auth.FirebaseAuth auth;
    private string currentMessage;
    private string lastMessage;
    private bool isNewUser;
    public static FirebaseChecks instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.


    void Start() {
        InitializeFirebase();
    }
    //Awake is always called before any Start functions
    void Awake()
    {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);


      //Sets this to not be destroyed when reloading scene
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

        DatabaseUtil database = DatabaseUtil.GetComponent<DatabaseUtil>();
        if (database != null && user != null && GameManager.instance != null) {
          int level = GameManager.instance.level;
          Debug.Log("current level in game: " + level);
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
                // reads level from database AND sets level in game
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

    public string GetUserId() {
      if (user != null) {
        return user.UserId;
      } else {
        return null;
      }
    }

    public void StartFromCurrentLevel() {
      if (user != null) {
        DatabaseUtil database = DatabaseUtil.GetComponent<DatabaseUtil>();
        database.getCurrentLevel(user.UserId);

      }
    }

    public void StartFromSpecificLevel(int i) {
      if (user != null) {
        DatabaseUtil database = DatabaseUtil.GetComponent<DatabaseUtil>();
        GameManager.instance.level = i;
        database.updateCurrentLevel(user.UserId, i);
      }
    }
}
