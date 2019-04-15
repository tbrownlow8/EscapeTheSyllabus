public class User
{
    public string username;
    public int levelsCompleted;
    public int currentLevel;
    public int deaths;
    public int correctAnswers;
    public int incorrectAnswers;
    public int score;

    public User()
    {
    }

    public User(string username)
    {
        this.username = username;
        this.levelsCompleted = 0;
        this.currentLevel = 1;
        this.deaths = 0;
        this.correctAnswers = 0;
        this.incorrectAnswers = 0;
        this.score = 0;
    }
}
