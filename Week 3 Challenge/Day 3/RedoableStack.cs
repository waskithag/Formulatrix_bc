using System;
using System.Collections.Generic;

namespace ModifiedStack;

public class ModedStack<T>
{
    private LinkedList<T> _items = new();
    private LinkedList<T> _redoLog = new();
    private int _itemCount = 0;
    private readonly int _maximum = 3;

    public void Type(T value)
    {
        _redoLog.Clear();

        if (_itemCount == _maximum)
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
            _redoLog.AddLast(_items.Last.Value);
            _items.RemoveLast();
            _itemCount--;
        }
        else
        {
            Console.WriteLine("Stack is empty");
        }
    }

    public void Redo()
    {
        if (_redoLog.Count > 0)
        {
            Console.WriteLine($"Redid {_redoLog.Last.Value}");
            _items.AddLast(_redoLog.Last.Value);
            _redoLog.RemoveLast();
            _itemCount++;
        }
        else
        {
            Console.WriteLine("Redo log is empty");
        }
    }

}

