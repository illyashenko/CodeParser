//*********************
// Console code parser
//*********************
using CodeParser;

Console.Write("укажите путь к файлу: ");
string? path = Console.ReadLine();
var tree = new AstTree<Lexems>();

// read and parse file data
using StreamReader reader = new StreamReader(path??"");
while (reader.ReadLine() is { } lineText)
{
    Parser.ParseToAstTree(lineText, tree);
}
// get NOde main class
var main = tree.Nodes.FirstOrDefault(el => el.Value.Type == LexemTypes.MainClass);

var conditions = CommonExpression.Conditions();
var listResult = new List<int>();
// go around the tree
foreach (var condition in conditions)
{
    if (tree is not null)
    {
        var res = CommonExpression.GetResult(main, condition);
        listResult.Add(res);
    }
}
// Print array result
Console.WriteLine($"[{string.Join(", ", listResult.Distinct().Order().ToList())}]");