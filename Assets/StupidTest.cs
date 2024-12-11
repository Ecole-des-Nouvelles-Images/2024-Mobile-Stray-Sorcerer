using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidTest : MonoBehaviour
{
    private int x;
    // Start is called before the first frame update
    void Start()
    {
        Stack<int> numbers = new Stack<int>();
        numbers.Push(1);
        numbers.Push(2);
        numbers.Push(3);
        numbers.Push(4);
        numbers.Push(5);
        Debug.Log("taille numbers:"+numbers.Count);
        x= numbers.Pop();
        Debug.Log("taille numbers:"+numbers.Count);
        Debug.Log(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
