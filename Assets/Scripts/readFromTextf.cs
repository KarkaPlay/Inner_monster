
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class readFromTextf : MonoBehaviour


{
    [SerializeField] Text txt_f, txt_s;
    string[] namesArray;
    string myFilePath, fileName;
  
    int d,n = 0;
    void Start()
    {
        fileName = "thoughtsFile.txt";
        myFilePath = Application.dataPath + "/" + fileName;
        //myFilePath = fileName;
        //ReadFromTheFile();
    }
    void fixedUpdate()
    {
        
    }
    void OnTriggerEnter(Collider react)
    {
        if (react.tag == "thought")
        {
            d = 0;
            ReadFromTheFile();
            
    }
        if (react.tag == "thought2")
        {
            d = 1;
            ReadFromTheFile();
     
        }
    }

    void DisplayFirst()
    {
 txt_f.text = namesArray[n];
    }


   void DisplaySecond()
    {  
            txt_s.text = namesArray[n];  
    }

    public void ReadFromTheFile()
    {
         
        namesArray = File.ReadAllLines(myFilePath);
        if (n<namesArray.Length) {
        if (d == 0) { 
        DisplayFirst(); n++;
        }
        if (d == 1)
        {
DisplaySecond();
            n++;
        }}
        //
        //foreach (string line in namesArray)
        //  {
        //    print(namesArray[n]);
        //}
        //n++;
    }
}

