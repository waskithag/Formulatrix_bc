using System;
using System.Collections.Generic;

namespace PriorityQueue;

class QueueNode
{
    public string Value { get; }
    public int PriorityValue { get; }
}

class PriorityQueue
{
    private LinkedList<QueueNode> _priorityQueueNodes = new();

    public Enqueue(string value, int priority)
    {
        if(_priorityQueueNodes.Count == 0)
        {
            _priorityQueueNodes.AddLast(QueueNode(value, priority));
        }
        else
        {
            nodeNow = _priorityQueueNodes.Last;
            while (move == true)
            {
                if (nodeNow.Value.PriorityValue <= )
            }
        }
    }
}