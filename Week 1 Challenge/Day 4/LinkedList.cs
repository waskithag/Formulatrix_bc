using System;

namespace LinkedList;

class Node(int value)
{
    public int Value = value;
    public Node? Next = null;
}

class Sequence
{
    private Node? head;
    private Node? tail;

    public void Append(int val)
    {
        Node newNode = new(val);

        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }

        Console.WriteLine($"Appended {val}");
    }

    public void Print()
    {
        Console.Write("Sequence: ");

        Node current = head;
        while (current != null)
        {
            Console.Write(current.Value);
            if (current.Next != null)
                Console.Write(" -> ");
            current = current.Next;
        }

        Console.WriteLine();
    }
}