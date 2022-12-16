using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointsSc : MonoBehaviour
{
    public ScoreDigitSc[] scoreDigits;

    void Start() {
        if( scoreDigits == null || scoreDigits.Length == 0 ) {
            Debug.Log("ScorePointsSc: La variable digits no ha sido asignada");
        }
    }

    public void Display( int value ) {

        print("ScorePointsSc.Display value: " + value);

        for( int i=0; i < scoreDigits.Length; i++ ) {
            scoreDigits[i].SetDigit(value % 10);
            value = value / 10;
        }
    }
}
