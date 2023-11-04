
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class readFromTextf : MonoBehaviour

  
{   public GameObject other;
    public GameObject panel1;
    public GameObject panel2;
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
            panel1.SetActive(true);
            d = 0;
            ReadFromTheFile();
            Destroy(react);
            StartCoroutine(WaitN());
        }
        if (react.tag == "thought2")
        {
            panel2.SetActive(true);
            d = 1;
            ReadFromTheFile();
            Destroy(react);
            StartCoroutine(WaitN());
        }
    }
    //void OnTriggerExit(Collider react)
    //{
    //    if (react.tag == "thought")
    //    {
    //        panel1.SetActive(false);
    //    }
    //    if (react.tag == "thought2")
    //    {
    //        panel2.SetActive(false);
    //    }
    //}

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
        }
        }else
        {
            other.SetActive(false);
        }

        //
        //foreach (string line in namesArray)
        //  {
        //    print(namesArray[n]);
        //}
        //n++;
    }

      public IEnumerator WaitN()
    { 
        yield return new WaitForSeconds(5);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }
}

