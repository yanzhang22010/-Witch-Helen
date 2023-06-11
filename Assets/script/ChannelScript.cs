using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Text;
using System;



public class ChannelScript : MonoBehaviour
{

    
    private SerialPort sp;
    private Thread recvThread;//线程
    private string s = "1,1,0,0";
    public int A, B, C, D;
    private bool isRead = false;


    // Use this for initialization   
    void Start()
    {
        sp = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);
        //串口初始化  
        if (!sp.IsOpen)
        {
            sp.Open();
        }
        recvThread = new Thread(ReceiveData); //该线程用于接收串口数据  
        recvThread.Start();
    }
    void Update()
    {
        if (isRead)
        {
            string[] sArry = Regex.
            Split(s, ",", RegexOptions.IgnoreCase);
            A = int.Parse(sArry[0]);
            B = int.Parse(sArry[1]);
            C = int.Parse(sArry[2]);
            D = int.Parse(sArry[3]);
            s = sArry[0]+","+sArry[1]+",0"+ ",0";
        }
    }

    private void ReceiveData()
    {
        try
        {
             s = "";
            //以行的模式读取串口数据
            while ((s = sp.ReadLine()) != null)
            {
                print(s); //打印读取到的每一行数据
                isRead = true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();//关闭串口
    }
    public void ClosePort()         //关闭串口
    {
        try
        {
            sp.Close();           
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
