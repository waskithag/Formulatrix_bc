using System;

namespace DoubleLinkedList;

class Node(int value)
{
    public int? value = value;
    public Node? next = null;
    public Node? prev = null;
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
            newNode.prev = _tail;
            _tail.next = newNode;
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
            Console.Write(current.value);
            if (current.next != null)
                Console.Write(" -> ");
            current = current.next;
        }

        Console.WriteLine();
    }

    public void PrintReverse()
    {
        Console.Write("Reversed: ");

        Node? current = _tail;
        while (current != null)
        {
            Console.Write(current.value);
            if (current.prev != null)
                Console.Write(" -> ");
            current = current.prev;
        }

        Console.WriteLine();
    }
}