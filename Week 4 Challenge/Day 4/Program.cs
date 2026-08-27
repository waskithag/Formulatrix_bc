using ModifiedLinkedList;

Sequence sequence = new();

sequence.Append(5);
sequence.Append(15);
sequence.Append(10);
sequence.Append(20);
sequence.AddFilter(x => x == 5);
sequence.Print();
sequence.PrintReverse();