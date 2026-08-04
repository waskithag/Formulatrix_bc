using System;
using System.Collections.Generic;

Stack<string> stack = new();

void Type(string word)
{
    stack.Push(word);
    Console.WriteLine($"Typed {word}");
}

void Undo()
{
    if (stack.Count > 0)
    {
        string word = stack.Pop();
        Console.WriteLine($"Undid {word}");
    }
    else
    {
        Console.WriteLine("Stack is empty");
    }
}

Type("foo");
Type("bar");
Undo();
Undo();
Undo();
