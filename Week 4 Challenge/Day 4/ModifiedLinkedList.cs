using System;
using System.Transactions;

namespace ModifiedLinkedList;

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
    private readonly List<Func<int, bool>> _filter = new();

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
            bool isFiltered = false;

            foreach (Func<int, bool> rule in _filter)
            {
                if (rule(current.value))
                {
                    isFiltered = true;
                    break;
                }
            }

            Console.Write(isFiltered ? "filtered" : current.value);

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
             bool isFiltered = false;

            foreach (Func<int, bool> rule in _filter)
            {
                if (rule(current.value))
                {
                    isFiltered = true;
                    break;
                }
            }

            Console.Write(isFiltered ? "filtered" : current.value);
            if (current.prev != null)
                Console.Write(" -> ");
            current = current.prev;
        }

        Console.WriteLine();
    }

    public void SetSorting(Func<int, int, int> comparer)
    {
        //??
    }

    public void AddFilter(Func<int, bool> filterRule)
    {
        _filter.Add(filterRule);
    }
}