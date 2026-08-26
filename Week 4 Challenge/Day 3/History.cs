using System;
using System.Collections.Generic;

namespace HistoryStack;

public class History
{
    private LinkedList<string> _items = new();
    private LinkedList<string> _redoLog = new();
    private readonly List<Func<string, bool>> _validationRules = new();
    private int _itemCount = 0;
    private readonly int _maximum = 3;

    public void AddValidationRule(Func<string, bool> rule)
    {
        _validationRules.Add(rule);
    }

    public void Type(string value)
    {
        if (_validationRules.Count > 0)
        {
            foreach (var rule in _validationRules)
            {
                if (!rule(value))
                {
                    Console.WriteLine($"Rejected {value}");
                    return;
                }
            }
        }

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

