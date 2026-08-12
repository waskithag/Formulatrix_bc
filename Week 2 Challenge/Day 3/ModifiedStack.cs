using System;
using System.Collections.Generic;

namespace ModifiedStack;

public class ModedStack<T>
{
    private LinkedList<T> _items = new();
    private int _itemCount = 0;
    private readonly int maximum = 3;

    public void Type(T value)
    {
        if (_itemCount == maximum)
        {
            _items.RemoveFirst();
            _items.AddLast(value);
            Console.WriteLine($"Dropped bottom, Typed {value}");
        }
        else
        {
            _items.AddLast(value);
            _itemCount++;
            Console.WriteLine($"Typed {value}");
        }
    }

    public void Undo()
    {
        if (_itemCount > 0)
        {
            Console.WriteLine($"Undid {_items.Last.Value}");
            _items.RemoveLast();
            _itemCount--;
        }
        else
        {
            Console.WriteLine("Stack is empty");
        }
    }

}

