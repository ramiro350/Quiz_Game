using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float TimeToCompleteQuestion = 30f;
    [SerializeField] float TimeToShowCorrectAsnwer = 10f;

    public bool loadNextQuestion;
    public float fillFraction;

    public bool isAsnweringQuestion;
    float TimerValue;
    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer(){
        TimerValue = 0;
    }

    void UpdateTimer(){
        TimerValue -= Time.deltaTime;

        if(isAsnweringQuestion){
            if(TimerValue>0){
                fillFraction = TimerValue / TimeToCompleteQuestion;
            }else{
                isAsnweringQuestion = false;
                TimerValue = TimeToShowCorrectAsnwer;
            }
        }else{
            if(TimerValue>0){
                fillFraction = TimerValue / TimeToShowCorrectAsnwer;
            }else{
                isAsnweringQuestion = true;
                TimerValue = TimeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }

}
