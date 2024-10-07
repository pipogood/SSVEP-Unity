using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LSL;

public class Example : MonoBehaviour
{
    public Color startColor = Color.white;
    public Color endColor = Color.black;
    public Color targetColor = Color.red;
    [Range(1, 40)]
    public int frequencyImg = 1; // Frequency in Hz

    //Image Blinking Paramemter 
    public Image blinkImg;
    private float elapsedTime = 0f;
    private bool isStartColor = true;

    //For interval calculations
    private float interval;

    //For cue
    public float blinkDuration = 5.0f;
    public float delayBeforeBlink = 1f;
    public float delayBeforeNextTrial = 2f;
    private float indexTimer = 0f;
    private float delayTimer = 0f;
    private bool isDelayActive = false;

    // For calculate equally class trials
    public int numberOfTrial = 10;
    public int numberOfClass = 2;
    private int[] randomNumbers;
    private int currentIndex = 0;

    //For LSL Outlet
    string StreamName = "UnityEventSSVEP"; // Stream Name
    string StreamType = "Markers";
    private StreamOutlet outlet;
    private string[] sample = { "" };

    void Start()
    {
        interval = 1f / frequencyImg;

        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
            channel_format_t.cf_string, hash.ToString());
        outlet = new StreamOutlet(streamInfo);

        GenerateEqualRandomNumbers();
        for (int i = 0; i < randomNumbers.Length; i++)
        {
            Debug.Log(randomNumbers[i]);
        }


        if (outlet != null)
        {
            sample[0] = "Experiment_Begin"; //Event Name
            outlet.push_sample(sample);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isDelayActive)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayBeforeNextTrial)
            {

                delayTimer = 0f;
                isDelayActive = false;
            }
        }

        else
        {
            indexTimer += Time.deltaTime;

            if (indexTimer < blinkDuration + delayBeforeBlink)
            {
                if (indexTimer < delayBeforeBlink)
                {
                    blinkImg.color = targetColor;
                }
                else
                {
                    elapsedTime += Time.deltaTime; // Increment elapsed time

                    if (elapsedTime >= interval / 2f) // Check if it's time to switch colors
                    {
                        blinkImg.color = isStartColor ? endColor : startColor;// Color changing
                        isStartColor = !isStartColor; // Toggle the color flag
                        elapsedTime = 0f; // Reset the elapsed time
                    }
                }
            }

            else
            {
                indexTimer = 0f;
                blinkImg.color = startColor;
                isDelayActive = true;
            }
        }
    }

    private void GenerateEqualRandomNumbers()
    {
        randomNumbers = new int[numberOfTrial];

        // Calculate the number of times each number should appear
        int countPerNumber = numberOfTrial / numberOfClass;
        int remainder = numberOfTrial % numberOfClass;

        // Fill the array with equal numbers of 0, 1, 2, and 3
        int index = 0;
        for (int num = 0; num < numberOfClass; num++)
        {
            for (int i = 0; i < countPerNumber; i++)
            {
                randomNumbers[index++] = num;
            }
        }

        // Distribute the remainder randomly
        for (int i = 0; i < remainder; i++)
        {
            randomNumbers[index++] = Random.Range(0, numberOfClass);
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
}
