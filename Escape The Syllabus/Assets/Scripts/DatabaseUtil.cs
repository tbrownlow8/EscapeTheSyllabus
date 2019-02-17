using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;
using System;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseUtil : MonoBehaviour
{
    DatabaseReference rootReference;
    public static DatabaseUtil instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.

    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://escapethesyllabus.firebaseio.com/");

        // Get the root reference location of the database.
        rootReference = FirebaseDatabase.DefaultInstance.RootReference;
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
    public void writeNewUser(string userId, string username)
    {
        User user = new User(username);
        string json = JsonUtility.ToJson(user);

        rootReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }


    public void updateLevelsCompleted(string userId, int levels)
    {
        rootReference.Child("users").Child(userId).Child("levelsCompleted").SetValueAsync(levels);
    }
    public void updateCurrentLevel(string userId, int level)
    {
        rootReference.Child("users").Child(userId).Child("currentLevel").SetValueAsync(level);
    }
    public void updateDeaths(string userId, int deaths)
    {
        rootReference.Child("users").Child(userId).Child("deaths").SetValueAsync(deaths);
    }
    public void updateCorrectAnswers(string userId, int correct)
    {
        rootReference.Child("users").Child(userId).Child("correctAnswers").SetValueAsync(correct);
    }
    public void updateIncorrectAnswers(string userId, int incorrect)
    {
        rootReference.Child("users").Child(userId).Child("incorrectAnswers").SetValueAsync(incorrect);
    }


    public int getLevelsCompleted(string userId)
    {
        rootReference.Child("users").Child(userId).Child("levelsCompleted").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                return 0;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null)
                {
                    return snapshot.GetValue(false);
                } else
                {
                    return 0;
                }
            }
            return 0;
        });
        return 0;
    }
    public async void getCurrentLevel(string userId)
    {
        await rootReference.Child("users").Child(userId).Child("currentLevel").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null) {
                  Debug.Log("snapshot value = " +snapshot.GetValue(false));
                  if (GameManager.instance != null) {

                  GameManager.instance.level = Convert.ToInt32(snapshot.GetValue(false));
                  }
                  return;
                }
            }
            return;
        });
        if (GameManager.instance != null) {
          Debug.Log(GameManager.instance.level);
        }
        return;
    }
    public int getDeaths(string userId)
    {
        rootReference.Child("users").Child(userId).Child("deaths").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                return 0;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null)
                {
                    return snapshot.GetValue(false);
                }
                else
                {
                    return 1;
                }
            }
            return 1;
        });
        return 1;
    }
    public int getCorrectAnswers(string userId)
    {
        rootReference.Child("users").Child(userId).Child("correctAnswers").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                return 0;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null)
                {
                    return snapshot.GetValue(false);
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        });
        return 0;
    }
    public int getIncorrectAnswers(string userId)
    {
        rootReference.Child("users").Child(userId).Child("incorrectAnswers").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                return 0;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot != null)
                {
                    return snapshot.GetValue(false);
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        });
        return 0;
    }
}
