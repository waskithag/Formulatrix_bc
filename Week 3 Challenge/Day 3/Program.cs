using ModifiedStack;

var modedStack = new ModedStack<string>();

modedStack.Type("A"); 
modedStack.Type("B"); 
modedStack.Type("C"); 
modedStack.Type("D"); 
modedStack.Type("E"); 
modedStack.Type("F"); 
modedStack.Type("G"); 
modedStack.Undo();
modedStack.Undo();
modedStack.Redo();
modedStack.Redo();
modedStack.Redo();
modedStack.Redo();
modedStack.Undo();
modedStack.Undo();

