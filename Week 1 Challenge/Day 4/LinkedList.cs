using System;

namespace LinkedList;

class Node(int value)
{
    public int? Value = value;
    public Node? Next = null;
}

class Sequence
{
    private Node? _head;
    private Node? _tail;

    public void Append(int val)
    {
        Node? newNode = new(val);

        if (_head == null)
        {
            _head = _tail = newNode;
        }
        else
        {
            _tail.Next = newNode;
            _tail = newNode;
        }

        Console.WriteLine($"Appended {val}");
    }

    public void Print()
    {
        Console.Write("Sequence: ");

        Node? current = _head;
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