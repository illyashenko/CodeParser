using System.Text.RegularExpressions;

namespace CodeParser;

public static class Parser
{
    static readonly Dictionary<LexemTypes, string> DynamicDefinition = new()
    {
        {LexemTypes.MainClass, "[a-z,A-Z](Main)[:{:]"},
        {LexemTypes.MainMethod, @"[a-z,A-Z](method)[:(:](\S*)[:):][:{:]"},
        {LexemTypes.DeclaringVariable, "^(int|boolean|char|string|float|double)[a-z,A-Z][:;:]"},
        {LexemTypes.Expression, @"[a-z,A-z][:=:](\S*)[:;:]"},
        {LexemTypes.Condition, @"^(if)[:(:](\S*)[:):][:{:]"},
        {LexemTypes.EndModule, "^[:}:]"}
    };
    public static void ParseToAstTree(string inputString, AstTree<Lexems> tree)
    {
        inputString = inputString.Replace(" ", String.Empty);
        inputString = inputString.Replace("\t", String.Empty);
        
        foreach (var lexemDefinition in DynamicDefinition)
        {
            if (new Regex(lexemDefinition.Value).Match(inputString).Length > 0)
            {
                switch (lexemDefinition.Key)
                {
                    case LexemTypes.MainClass:
                        tree.Begin(new LexemsCommon { Type = LexemTypes.MainClass });
                        break;
                    case LexemTypes.MainMethod:
                        tree.Begin(new LexemsCommon { Type = LexemTypes.MainMethod });
                        break;
                    case LexemTypes.DeclaringVariable:
                        tree.Add(new LexemsCommon { Type = LexemTypes.DeclaringVariable });
                        break;
                    case LexemTypes.Expression:
                        inputString = inputString.Replace(";", "");
                        var idx = inputString.IndexOf('=');
                        tree.Add(new LexemsExpression
                        {
                            RightValue = inputString.Substring(0, idx),
                            LeftValue = inputString.Substring(idx + 1),
                            Type = LexemTypes.Expression,
                            Sing = Sings.Equal
                        });
                        break;
                    case LexemTypes.Condition:
                        var idxBegin = inputString.IndexOf('[') + 1;
                        var idxEnd = inputString.IndexOf(']');
                        tree.Begin(new LexemsCondition{Type = LexemTypes.Condition, NameArgument = $"C{inputString.Substring(idxBegin, idxEnd - idxBegin)}"});
                        break;
                    case LexemTypes.EndModule:
                        tree.End();
                        break;
                }
            }
        }
    }
}