using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLevel : MonoBehaviour
{
    public void updateLevel(int level)
    {
        QuestionStats.currentLevel = level;
    }
}
