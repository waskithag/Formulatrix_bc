using System;
using System.Collections.Generic;

Queue<string> queue = new();

void Enqueue(string val)
{
    queue.Enqueue(val);
    Console.WriteLine($"Queued {val}");
}

void Process()
{
    if (queue.Count > 0)
    {
        string val = queue.Dequeue();
        Console.WriteLine($"Processed {val}");
    }
    else
    {
        Console.WriteLine("Queue is empty");
    }
}

Enqueue("A"); 
Enqueue("B"); 
Process(); 
Process();
Process();