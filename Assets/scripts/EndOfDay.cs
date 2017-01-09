using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EndOfDay : MonoBehaviour {

    public Text dayCount;
    public Text earnedAmount;
    public Text expensesAmount;
    public Text continueText;
    public Text totalAmount;
    public int secondsBetweenDisplay;

    private bool pressedSpaceAlready = false;
    private IEnumerator display;

    void Start()
    {
        display = displayText();
        StartCoroutine(display);
    }
  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !pressedSpaceAlready){
            StopCoroutine(display);
            dayCount.enabled = earnedAmount.enabled = expensesAmount.enabled = totalAmount.enabled = true;
            StartCoroutine(showContinueText());
        }

        if (Input.GetKeyDown(KeyCode.Space) && pressedSpaceAlready)
        {
            SceneManager.LoadScene(0);
        }
        earnedAmount.text = GameState.Instance.money.ToString();
        dayCount.text = "1";
        expensesAmount.text = "1";
        totalAmount.text = "1";
    }

    private IEnumerator showContinueText()
    {
        yield return new WaitForSeconds(secondsBetweenDisplay);
        pressedSpaceAlready = continueText.enabled = true;
    }

    private IEnumerator displayText()
    {   
        yield return new WaitForSeconds(secondsBetweenDisplay);
        earnedAmount.enabled = true;
        yield return new WaitForSeconds(secondsBetweenDisplay);
        expensesAmount.enabled = true;
        yield return new WaitForSeconds(secondsBetweenDisplay);
        totalAmount.enabled = true;
        yield return new WaitForSeconds(secondsBetweenDisplay);
        pressedSpaceAlready = continueText.enabled = true;
    }
}
