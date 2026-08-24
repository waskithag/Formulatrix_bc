using System;
using FooBar;

FooBarBuzz generator = new();
generator.AddRule(3, "foo");
generator.AddRule(5, "bar");
generator.AddRule(7, "buzz");

Console.WriteLine(generator.GenerateSequence(1, 22));
