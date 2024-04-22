using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] events;
    [SerializeField] private Sprite[] operationSprites;

    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI firstNumberText;
    [SerializeField] private TextMeshProUGUI secondNumberText;
    [SerializeField] private Image operationImage;
    [SerializeField] private TMP_InputField answerInputField;

    [SerializeField] private float minTimeBetweenEvents = 3.0f;
    [SerializeField] private float maxTimeBetweenEvents = 5.0f;

    private Dictionary<string, Sprite> operationSpritesDict = new ();
    
    private float timeSinceLastEvent = 0.0f;
    private float timeToAnswer = 0.0f;

    private bool eventActive = false;
    private int correctAnswer;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sprite sprite in operationSprites)
            operationSpritesDict.Add(sprite.name, sprite);

        foreach (Event e in events)
            e.operationsAllowed = new HashSet<FourBasicOperations>(e.operationsAllowed).ToArray();

        timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive)
        {
            if (timeToAnswer <= 0.0f)
            {
                Debug.Log("Time's up!");
                eventPanel.SetActive(false);
                eventActive = false;
                timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            }
            else
            {
                timeToAnswer -= Time.deltaTime;
                timeText.text = Mathf.CeilToInt(timeToAnswer).ToString();
            }

            if (Input.GetKeyDown(KeyCode.Return))
                SubmitAnswer();
        }
        else
        {
            if (timeSinceLastEvent <= 0.0f)
            {
                NewEvent();
                eventActive = true;
            }
            else
                timeSinceLastEvent -= Time.deltaTime;
        }
    }

    public void SubmitAnswer()
    {
        if (answerInputField.text == correctAnswer.ToString())
        {
            eventPanel.SetActive(false);
            eventActive = false;
            timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            answerInputField.text = "";
            Debug.Log("Correct!");
        }
        else
            Debug.Log("Incorrect!");
    }

    private void NewEvent()
    {
        Event randomEvent = events[Random.Range(0, events.Length)];

        eventNameText.text = randomEvent.name;
        timeToAnswer = randomEvent.time;

        FourBasicOperations operation = randomEvent.operationsAllowed[Random.Range(0, randomEvent.operationsAllowed.Length)];

        int minFirstNumber;

        if (randomEvent.firstNumberDigits == 1)
            minFirstNumber = 0;
        else
            minFirstNumber = (int)Mathf.Pow(10, randomEvent.firstNumberDigits - 1);
        
        int maxFirstNumber = (int)Mathf.Pow(10, randomEvent.firstNumberDigits);
        int firstNumber = Random.Range(minFirstNumber, maxFirstNumber);

        int minSecondNumber;

        if (randomEvent.secondNumberDigits == 1)
            minSecondNumber = 0;
        else
            minSecondNumber = (int)Mathf.Pow(10, randomEvent.secondNumberDigits - 1);

        int maxSecondNumber = (int)Mathf.Pow(10, randomEvent.secondNumberDigits);
        int secondNumber = Random.Range(minSecondNumber, maxSecondNumber);

        switch (operation)
        {
            case FourBasicOperations.Addition:
                correctAnswer = firstNumber + secondNumber;
                operationImage.sprite = operationSpritesDict["Addition"];
                break;
            case FourBasicOperations.Multiplication:
                correctAnswer = firstNumber * secondNumber;
                operationImage.sprite = operationSpritesDict["Multiplication"];
                break;
            case FourBasicOperations.Subtraction:
                if (!randomEvent.negativesAllowed)
                    secondNumber = Random.Range(minSecondNumber, firstNumber + 1);

                correctAnswer = firstNumber - secondNumber;
                operationImage.sprite = operationSpritesDict["Subtraction"];
                break;
            case FourBasicOperations.Division:
                if (firstNumber != 0)
                {
                    List<int> factors = FindFactors(firstNumber);
                    List<int> factorsWithSecondNumberDigits = new ();

                    foreach (int factor in factors)
                        if (factor.ToString().Length == randomEvent.secondNumberDigits)
                            factorsWithSecondNumberDigits.Add(factor);

                    if (factorsWithSecondNumberDigits.Count > 0)
                        secondNumber = factorsWithSecondNumberDigits[Random.Range(0, factorsWithSecondNumberDigits.Count)];
                    else
                        secondNumber = factors[Random.Range(0, factors.Count)];
                }

                correctAnswer = firstNumber / secondNumber;
                operationImage.sprite = operationSpritesDict["Division"];
                break;
        }

        firstNumberText.text = firstNumber.ToString();
        secondNumberText.text = secondNumber.ToString();

        eventPanel.SetActive(true);
    }

    private List<int> FindFactors(int n)
    {
        List<int> factors = new List<int>();

        int sqrt = (int)Mathf.Sqrt(n);
        for (int i = 1; i <= sqrt; i++)
        {
            if (n % i == 0)
            {
                factors.Add(i);
                int factor = n / i;
                if (factor != i)
                    factors.Add(factor);
            }
        }

        return factors;
    }
}
