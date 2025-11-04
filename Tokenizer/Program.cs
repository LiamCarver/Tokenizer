
var text = File.ReadAllText("Corpus.txt");

var tokenizer = new Tokenizer.Tokenizer();
tokenizer.Tokenize(text);