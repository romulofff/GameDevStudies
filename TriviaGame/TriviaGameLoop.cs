using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaGameLoop : MonoBehaviour {

    //Defines question value
    public struct Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;

        public Question(string questionText, string[] answers, int correctAnswerIndex)
        {
            this.questionText = questionText;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
        }
    }

    private Question currentQuestion = new Question("What is your favorite color?", new string[] { "Blue", "Red", "Yellow", "White", "Black" },0 );
    public Button[] answerButtons;
    public Text questionText;

    private Question[] questions = new Question[21];
    private int currentQuestionIndex;
    private int[] questionNumbersChoosen = new int[5];
    private int questionsFinished;

    public GameObject[] TriviaPanels;
    public GameObject finalResultsPanel;
    public Text resultsText;
    private int numOfCorrectAnswers;
    private bool allowSelection = true;

    public GameObject feedbackText;

	// Use this for initialization
	void Start ()
    {
        for (int i=0; i < questionNumbersChoosen.Length; i++)
        {
            questionNumbersChoosen[i] = -1;
        }
        
        questions[0] = new Question("What is the capital of Spain?", new string[] { "Topeka", "Amsterdam", "Madrid", "London", "Toledo" }, 2);
        questions[1] = new Question("Who was the second US president?", new string[] { "Thomas Jefferson", "John Adams", "Bill Clinton", "George Washington", "Abraham Lincoln" }, 1);
        questions[2] = new Question("What is the second planet in our solar system?", new string[] { "Mercury", "Earth", "Saturn", "Venus", "Pluto" }, 3);
        questions[3] = new Question("What is the largest continent?", new string[] { "Africa", "North America", "Asia", "Europe", "Australia" }, 2);
        questions[4] = new Question("What US state has the largest population?", new string[] { "California", "Florida", "Texas", "New York", "North Carolina" }, 0);
        questions[5] = new Question("A platypus is a ________", new string[] { "Bird", "Reptile", "Insect", "Amphibian", "Mammal" }, 4);
        questions[6] = new Question("What is the boiling temperature in fahrenheit?", new string[] { "100 degrees", "190 degrees", "200 degrees", "312 degrees", "212 degrees" }, 4);
        questions[7] = new Question("How many degrees are in a circle?", new string[] { "360", "180", "640", "16", "270" }, 0);
        questions[8] = new Question("What is a name for a group of crows?", new string[] { "A bloat", "A herd", "A pack", "A murder", "A team" }, 3);
        questions[9] = new Question("Who created the painting starry night?", new string[] { "Pablo Picasso", "Vincent Van Gogh", "Andy Warhol", "Leonardo da Vinci", "Frida Kahlo" }, 1);
        questions[10] = new Question("Who played the main character on Forrest Gump?", new string[] { "Nicholas Cage", "Tom Hanks", "Tom Cruise", "Leonardo diCaprio", "Robert Downey Jr." }, 1);
        questions[11] = new Question("What movie started the Marvel Cinematic Universe (MCU)?", new string[] { "The Incredible Hulk", "Batman Begins", "Thor", "Iron Man", "Captain America" }, 3);
        questions[12] = new Question("Who directed the first Star Wars movie?", new string[] { "George Lucas", "Stanley Kubrik", "Steven Spielberg", "Woody Allen", "Martin Scorsese" }, 0);
        questions[13] = new Question("Who is the main singer of the Rolling Stones", new string[] { "Robert Plant", "John Lennon", "Bruce Dickinson", "John Mayer", "Mick Jagger" }, 4);
        questions[14] = new Question("Which band plays \"Smoke on the water\"? ", new string[] { "AC/DC", "Guns N' Roses", "Deep Purple", "Iron Maiden", "KISS" }, 2);
        questions[15] = new Question("How many planets form the solar system?", new string[] { "5", "9", "8", "7", "3" }, 2);
        questions[16] = new Question("What is the name of Disney's main character?", new string[] { "Mickey, The Duck", "Donald Duck", "Goofy", "Mickey Mouse", "Minions" }, 3);
        questions[17] = new Question("How long the Earth takes to make a complete turn around the sun?", new string[] { "24 hours", "364 days", "365 days", "12 hours", "400 days" }, 2);
        questions[18] = new Question("Which countries are part of south america?", new string[] { "Brazil, Russia, India", "Brazil, Chile, Argentina", "USA, Paraguay, Mexico", "Spain, Portugal, Italy", "New Zealand, Congo, Japan" }, 1);
        questions[19] = new Question("Where the Gods from Greek Mythology lived?", new string[] { "The Heaven", "Valhalla", "Olympus", "Mount Everest", "Sky" }, 2);
        questions[20] = new Question("Who directed the movie \"Jurassic Park\"?", new string[] { "Steven Spielberg", "Christopher Nolan", "Michael Crichton", "Colin Trevorrow", "George Lucas" }, 0);

        chooseQuestions();
        assignQuestion(questionNumbersChoosen[0]);

    }

    // Update is called once per frame
    void Update ()
    {
        quitGame();
    }

    //Setting up the interface to show a question
    void assignQuestion(int questionNum)
    {
        currentQuestion = questions[questionNum];
        questionText.text = currentQuestion.questionText;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
        }
    }

    //Checks if the question was answered right or wrong and gives feedback
    //moves to the next question after a pause
    public void checkAnswer(int buttonNum)
    {
        if(allowSelection) { 
            if(buttonNum == currentQuestion.correctAnswerIndex)
            {
                print("Correct!");
                numOfCorrectAnswers++;
                feedbackText.GetComponent<Text>().text = "Correct!";
                feedbackText.GetComponent<Text>().color = Color.green;

            }
            else
            {
                print("Wrong!");
                feedbackText.GetComponent<Text>().text = "Wrong!";
                feedbackText.GetComponent<Text>().color = Color.red;
            }
            StartCoroutine("continueAfterFeedback");
        }
    }

    //Choose the question numbers for the trivia game
    void chooseQuestions()
    {
        for(int i = 0; i < questionNumbersChoosen.Length; i++)
        {
            int questionNum = Random.Range(0, questions.Length);
            if(numNotContained(questionNumbersChoosen, questionNum))
            {
                questionNumbersChoosen[i] = questionNum;
            }
            else
            {
                i--;
            }
        }

        currentQuestionIndex = Random.Range(0, questions.Length);
    }

    //Checking to see if the random number choosen has already been choosen
    bool numNotContained(int[] numbers, int num)
    {
        for(int i=0; i < numbers.Length; i++)
        {
            if (num == numbers[i]) { return false; }
        }
        return true;
    }

    //Assigns new question using the next question number
    public void moveToNextQuestion()
    {
        assignQuestion(questionNumbersChoosen[questionNumbersChoosen.Length -1 - questionsFinished]);
    }

    //Setting the results text to the right text depending on how many question were answered correctly
    void displayResults()
    {
        switch (numOfCorrectAnswers)
        {
        case 5:
            resultsText.text = "5 out of 5 correct. You are all knowing!";
            break;
        case 4:
            resultsText.text = "4 out of 5 correct. You are very smart!";
            break;
        case 3:
            resultsText.text = "3 out of 5 correct. Well done.";
            break;
        case 2:
            resultsText.text = "2 out of 5 correct. Better luck next time.";
            break;
        case 1:
            resultsText.text = "1 out of 5 correct. You can do better than that!";
            break;
        case 0:
            resultsText.text = "0 out of 5 correct. Are you even trying?";
            break;
        default:
            print("Incorrect intelligence level.");
            break;
        }
    }

    //Restarts the level
    public void restartLevel()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    //Coroutine that pauses and then moves onto the next question
    IEnumerator continueAfterFeedback()
    {
        allowSelection = false;
        feedbackText.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        if (questionsFinished < questionNumbersChoosen.Length - 1)
        {
            moveToNextQuestion();
            questionsFinished++;
        }
        else
        {
            foreach (GameObject pan in TriviaPanels)
            {
                pan.SetActive(false);
            }
            finalResultsPanel.SetActive(true);
            displayResults();
        }
        allowSelection = true;
        feedbackText.SetActive(false);
    }

    //Checks for input to quit game (Escape key)    
    void quitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
