using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class PythonToUnity : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port = 1880;
    public bool enable_slider;
    private bool startRecieving = true;
    private string data;

    public Slider slider_up;
    public Slider slider_left;
    public Slider slider_right;
    public Image left_arrow;
    public Image right_arrow;
    public Image up_arrow;
    public Image circle;
    public Image cross_ver;
    public Image cross_hori;

    private float left_value;
    private float right_value;
    private float non_value;
    private float feet_value;
    private float TargetClass;

    // Start is called before the first frame update
    void Start()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        left_arrow.enabled = false;
        right_arrow.enabled = false;
        up_arrow.enabled = false;
        circle.enabled = false;
        cross_hori.enabled = false;
        cross_ver.enabled = false;
        slider_left.gameObject.SetActive(false);
        slider_right.gameObject.SetActive(false);
        slider_up.gameObject.SetActive(false);
    }

    public void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);
                Debug.Log(data);
                string[] stringArray = data.Split(',');
                List<string> stringList = new List<string>(stringArray);
                left_value = float.Parse(stringArray[0]);
                right_value = float.Parse(stringList[1]);
                non_value = float.Parse(stringList[2]);
                feet_value = float.Parse(stringList[3]);
                TargetClass = float.Parse(stringList[4]);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        slider_left.value = left_value;
        slider_right.value = right_value;
        slider_up.value = feet_value;

        float[] values = { left_value, right_value, non_value, feet_value };
        float maxValue = Mathf.Max(values);
        int maxIndex = System.Array.IndexOf(values, maxValue);

        //Debug.Log("The maximum value is: " + maxValue + " at index: " + maxIndex);

        if (TargetClass == 1)
        {
            StartCoroutine(EnableAndDisableAfterDelay(left_arrow, 0.5f));
        }

        else if (TargetClass == 2)
        {
            StartCoroutine(EnableAndDisableAfterDelay(right_arrow, 0.5f));
        }

        else if (TargetClass == 4)
        {
            StartCoroutine(EnableAndDisableAfterDelay(up_arrow, 0.5f));
        }

        else if (TargetClass == 5)
        {
            StartCoroutine(EnableAndDisableAfterDelay(circle, 7f));
            StartCoroutine(EnableAndDisableAfterDelay(cross_ver, 7f));
            StartCoroutine(EnableAndDisableAfterDelay(cross_hori, 7f));

            if (enable_slider)
            {
                StartCoroutine(EnableAndDisableAfterDelay2(slider_left, 7f));
                StartCoroutine(EnableAndDisableAfterDelay2(slider_right, 7f));
                StartCoroutine(EnableAndDisableAfterDelay2(slider_up, 7f));
            }
        }

        IEnumerator EnableAndDisableAfterDelay(Image arrow, float delay)
        {
            arrow.enabled = true;
            yield return new WaitForSeconds(delay);
            arrow.enabled = false;
        }

        IEnumerator EnableAndDisableAfterDelay2(Slider slide, float delay)
        {
            slide.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
            slide.gameObject.SetActive(false);
        }

        
    }
}

