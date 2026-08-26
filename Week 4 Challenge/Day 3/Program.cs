using HistoryStack;

var history = new History();

history.AddValidationRule(word => !string.IsNullOrWhiteSpace(word));
history.AddValidationRule(word => word.Length <= 10);

history.Type("hello"); 
history.Type("");
