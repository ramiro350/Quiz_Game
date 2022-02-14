using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] asnwerButtons;
    int correctAsnwerIndex;
    bool hasAsnweredEarly = true;

    [Header("Buttons Colors")]
    [SerializeField] Sprite defaultAsnwerSprite;
    [SerializeField] Sprite correctAsnwerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scorekeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scorekeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update(){
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion){
            if(progressBar.value == progressBar.maxValue){
                isComplete = true;
                return;
            }
            hasAsnweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAsnweredEarly && !timer.isAsnweringQuestion){
            DisplayAsnwer(-1);
            SetButtonState(false);
        }
    }

    void SetButtonState(bool state){
        for(int i=0;i<asnwerButtons.Length;i++){
            Button button = asnwerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void DisplayQuestion(){
        questionText.text = currentQuestion.getQuestion();
        for(int i = 0; i<asnwerButtons.Length;i++){
        TextMeshProUGUI buttonText = asnwerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = currentQuestion.getAnswer(i);
        }
    }
    
    void GetNextQuestion(){
        if(questions.Count>0){

        SetButtonState(true);
        SetDefaultButtonSprites();
        getRandomQuestion();
        DisplayQuestion();
        progressBar.value++;
        scorekeeper.IncrementQuestionsSeen();
        }
    }

    void getRandomQuestion(){
        int index = Random.Range(0,questions.Count);
        currentQuestion = questions[index];
        if(questions.Contains(currentQuestion)){
        questions.Remove(currentQuestion);
        }
    }

    public void onAsnwerSelected(int index){
        hasAsnweredEarly = true;
        DisplayAsnwer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scorekeeper.CalculateScore() + "%";
    }

    void DisplayAsnwer(int index){
        Image buttonImage;
        if(index == currentQuestion.getCorrectAnswerIndex()){
            questionText.text = "Correct!";
            buttonImage = asnwerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAsnwerSprite;
            scorekeeper.IncrementCorrectAnswers();
        }else{
            correctAsnwerIndex = currentQuestion.getCorrectAnswerIndex();
            string correctAsnwer = currentQuestion.getAnswer(correctAsnwerIndex);
            questionText.text = "Sorry the correct answer was;\n" + correctAsnwer;
            buttonImage = asnwerButtons[correctAsnwerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAsnwerSprite;
        }
    }

    void SetDefaultButtonSprites(){
        for(int i=0;i<asnwerButtons.Length;i++){
            Image buttonImage = asnwerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAsnwerSprite;
        }
    }

}
