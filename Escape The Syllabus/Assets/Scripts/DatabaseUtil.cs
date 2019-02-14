using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseUtil : MonoBehaviour
{
    DatabaseReference rootReference;
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://escapethesyllabus.firebaseio.com/");

        // Get the root reference location of the database.
        rootReference = FirebaseDatabase.DefaultInstance.RootReference;
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
    public int getCurrentLevel(string userId)
    {
        rootReference.Child("users").Child(userId).Child("currentLevel").GetValueAsync().ContinueWith(task => {
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
                    return 0;
                }
            }
            return 0;
        });
        return 0;
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
