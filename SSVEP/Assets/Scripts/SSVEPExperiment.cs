using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSVEPExperiment : MonoBehaviour
{
    public Color startColor = Color.white;
    public Color endColor = Color.black;
    public Color targetColor = Color.red;

    [Range(1, 40)]
    public int frequencyLeft = 1; // Frequency in Hz

    [Range(1, 40)]
    public int frequencyRight = 1;

    [Range(1, 40)]
    public int frequencyUp = 1;

    [Range(1, 40)]
    public int frequencyDown = 1;

    public Image blinkImgLeft;
    public Image blinkImgRight;
    public Image blinkImgUp;
    public Image blinkImgDown;

    public Text delayText; // Text to display during the delay

    private float elapsedTime;
    private bool isStartColor = true;

    private float intervalLeft;
    private float intervalRight;
    private float intervalUp;
    private float intervalDown;

    public float delayBeforExperiment = 5f;
    public float blinkDuration = 5.0f;
    public float delayBeforeBlink = 1f;


    private bool isBlinkingActive = false; // To check if blinking should start

    private float totalElapsedTimeLeft = 0f; // Total time since blinking started

    public int numberOfTrial = 4;
    private int[] randomNumbers;
    private int currentIndex = 0;
    private float indexTimer = 0f;

    public float delayBeforeNextIndex = 2f;
    private float delayTimer = 0f;
    private bool isDelayActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //// Initialize the array with the desired number of random numbers
        GenerateEqualRandomNumbers();

        intervalLeft = 1f / frequencyLeft; // Calculate the interval based on frequency
        intervalRight = 1f / frequencyRight;
        intervalUp = 1f / frequencyUp;
        intervalDown = 1f / frequencyDown;

        delayText.text = "Starting in 20 seconds...";
    }

    // Update is called once per frame
    void Update()  
    {

        if (!isBlinkingActive)
        {
            // During the delay period
            delayBeforExperiment -= Time.deltaTime; // Decrement the delay timer
            delayText.text = $"Starting in {Mathf.Ceil(delayBeforExperiment)} seconds...";

            if (delayBeforExperiment <= 0f)
            {
                // Start blinking after the delay
                isBlinkingActive = true;
                delayText.enabled = false; // Hide the delay text
            }
        }

        else
        {
            if (currentIndex >= randomNumbers.Length)
            {
                Debug.Log("Completed all random numbers."); // Indicate completion
                return; // Exit if all random numbers are processed
            }

            if (isDelayActive)
            {
                // During delay before moving to next index
                delayTimer += Time.deltaTime;
                if (delayTimer >= delayBeforeNextIndex)
                {
                    // Reset the timer and move to the next index after the delay
                    delayTimer = 0f;
                    isDelayActive = false;
                    currentIndex++;
                }
            }

            else
            {
                // Increment the index timer
                indexTimer += Time.deltaTime;

                if (indexTimer < blinkDuration + delayBeforeBlink)
                {
                    Debug.Log("Current Index: " + currentIndex);

                    int currentRandomNumber = randomNumbers[currentIndex];

                    // Check for the current random number
                    if (currentRandomNumber == 0)
                    {
                        if (indexTimer < delayBeforeBlink)
                        {
                            blinkImgLeft.color = targetColor;
                        }
                        else
                        {
                            HandleBlinking(blinkImgLeft, intervalLeft);
                        }
                    }
                    else if (currentRandomNumber == 1)
                    {
                        if (indexTimer < delayBeforeBlink)
                        {
                            blinkImgRight.color = targetColor;
                        }
                        else
                        {
                            HandleBlinking(blinkImgRight, intervalRight);
                        }
                    }
                    else if (currentRandomNumber == 2)
                    {
                        if (indexTimer < delayBeforeBlink)
                        {
                            blinkImgUp.color = targetColor;
                        }
                        else
                        {
                            HandleBlinking(blinkImgUp, intervalUp);
                        }
                    }
                    else
                    {
                        if (indexTimer < delayBeforeBlink)
                        {
                            blinkImgDown.color = targetColor;
                        }
                        else
                        {
                            HandleBlinking(blinkImgDown, intervalDown);
                        }
                    }
                }

                else
                {
                    // Reset the timer and move to the next index after the duration
                    indexTimer = 0f;
                    blinkImgLeft.color = startColor;
                    blinkImgRight.color = startColor;
                    blinkImgUp.color = startColor;
                    blinkImgDown.color = startColor;
                    isDelayActive = true;
                }
            }
        }

    }

    private void GenerateEqualRandomNumbers()
    {
        randomNumbers = new int[numberOfTrial];

        // Calculate the number of times each number should appear
        int countPerNumber = numberOfTrial / 4;
        int remainder = numberOfTrial % 4;

        // Fill the array with equal numbers of 0, 1, 2, and 3
        int index = 0;
        for (int num = 0; num < 4; num++)
        {
            for (int i = 0; i < countPerNumber; i++)
            {
                randomNumbers[index++] = num;
            }
        }

        // Distribute the remainder randomly
        for (int i = 0; i < remainder; i++)
        {
            randomNumbers[index++] = Random.Range(0, 4);
        }

        // Shuffle the array to randomize the order
        ShuffleArray(randomNumbers);
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void HandleBlinking(Image Img_direct, float interval)
    {
        elapsedTime += Time.deltaTime; // Increment elapsed time

        if (elapsedTime >= interval / 2f) // Check if it's time to switch colors
        {
            Img_direct.color = isStartColor ? endColor : startColor;// Toggle the color
            isStartColor = !isStartColor; // Toggle the color flag
            elapsedTime = 0f; // Reset the elapsed time
        }
    }
}
