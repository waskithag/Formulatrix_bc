using System;

namespace CircQueue;

public class CircularQueue<T>(int capacity)
{
    private T[] queue = new T[capacity];
    private int front = 0;
    private int rear = -1;
    private int itemCount = 0;

    public int Capacity { get; private set; }

    private bool IsEmpty() => itemCount == 0;
    private bool IsFull() => itemCount == capacity;

    public void Log(T item)
    {
        if (IsFull())
        {
            Console.WriteLine("Critical: Buffer Full");
            Console.WriteLine($"Overwritten oldest with {item}");
            queue[front] = item;
            front = (front + 1) % capacity;
            rear = (rear + 1) % capacity;
            return;
        }

        if (((float)itemCount / capacity) > 0.66)
        {
            Console.WriteLine($"Warning: Buffer at {(float)itemCount / capacity:P2}");
        }
        rear = (rear + 1) % capacity;
        queue[rear] = item;
        itemCount++;
        Console.WriteLine($"Logged [{item}]");
    }

    public void Read()
    {
        if (IsEmpty())
        {
            Console.WriteLine("No item in buffer");
            return;
        }

        T item = queue[front];
        front = (front + 1) % capacity;
        itemCount--;
        Console.WriteLine($"Read val [{item}]");
    }
}