using System;
using System.Collections.Generic;

namespace PriorityQueue;

class QueueNode
{
    public string Value { get; }
    public int PriorityValue { get; }

    public QueueNode(string value, int priority)
    {
        Value = value;
        PriorityValue = priority;
    }
}

class PriorityQue
{
    private readonly LinkedList<QueueNode> _priorityQueueNodes = new();

    public void Enqueue(string value, int priority)
    {
        QueueNode newNode = new(value, priority);

        Console.WriteLine($"Queued {value} with priority {priority}");

        if (_priorityQueueNodes.Count == 0)
        {
            _priorityQueueNodes.AddFirst(newNode);
            return;
        }

        LinkedListNode<QueueNode>? current = _priorityQueueNodes.First;

        while (current != null && current.Value.PriorityValue <= priority)
        {
            current = current.Next;
        }

        if (current == null)
        {
            _priorityQueueNodes.AddLast(newNode);
        }
        else
        {
            _priorityQueueNodes.AddBefore(current, newNode);
        }

        
    }

    public void Process()
    {
        if (_priorityQueueNodes.First == null)
        {
            Console.WriteLine("No queue to process");
            return;
        }

        QueueNode nodeToProcess = _priorityQueueNodes.First.Value;

        Console.WriteLine($"Processed {nodeToProcess.Value}");

        _priorityQueueNodes.RemoveFirst();
    }
}