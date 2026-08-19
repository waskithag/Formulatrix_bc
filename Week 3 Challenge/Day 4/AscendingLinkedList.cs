using System;
using System.Transactions;

namespace AscLinkedList;

class Node(int value)
{
    public int value = value;
    public Node? next = null;
    public Node? prev = null;
}

class Sequence
{
    private Node? _head;
    private Node? _tail;

    public void Append(int val)
    {
        Node newNode = new(val);

        if (_head == null)
        {
            _head = _tail = newNode;
        }
        else
        {
            Node current = _head;
            while (current.next != null && current.next.value < val)
            {
                current = current.next;
            }

            if (current.value >= val)
            {
                if (current == _head)
                {
                    current.prev = newNode;
                    newNode.next = current;
                    _head = newNode;
                }
                else
                {
                    current.prev.next = newNode;
                    newNode.prev = current.prev;
                    newNode.next = current;
                    current.prev = newNode;
                }
            }
            else
            {

                if (current == _tail)
                {
                    newNode.prev = current;
                    current.next = newNode;
                    _tail = newNode;
                }
                else
                {
                    current.next.prev = newNode;
                    newNode.prev = current;
                    newNode.next = current.next;
                    current.next = newNode;
                }
            }
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