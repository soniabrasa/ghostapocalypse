using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDigitSc : MonoBehaviour
{
    public Sprite[] digits;

    void Start()
    {
        if ( digits == null || digits.Length == 0 ) {
            Debug.Log("ScoreDigitSc: La variable digits no está correctamente asignada");
        }

    }

    public void SetDigit( int digit ) {
        if ( digit < 0 || digit > digits.Length ) {
            Debug.Log("ScoreDigitSc.SetDigit: Valor de digit ( " + digit + " ) fuera de rango.");

            return;
        }

        GetComponent<SpriteRenderer>().sprite = digits[digit];
    }
}
