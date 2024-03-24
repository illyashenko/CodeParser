namespace CodeParser;

public abstract class Lexems
{
    public LexemTypes Type { get; set; }
}

public class LexemsCommon : Lexems { }

public class LexemsExpression : Lexems
{
    public string? RightValue { get; set; }
    public string? LeftValue { get; set; }
    public Sings Sing { get; set; }
}

public class LexemsCondition : Lexems
{ 
    public string? NameArgument { get; set; }
}

public enum LexemTypes
{
    MainClass,
    MainMethod,
    Expression,
    DeclaringVariable,
    Condition,
    EndModule
}

public enum Sings
{
    // '='
    Equal,
    
}