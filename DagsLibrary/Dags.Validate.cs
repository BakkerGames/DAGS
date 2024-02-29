using System.Text;
using static DagsLibrary.Constants;

namespace DagsLibrary;

public partial class Dags
{
    /// <summary>
    /// Validates the information in the dictionary to make sure it will work with DAGS.
    /// </summary>
    public bool ValidateDictionary(StringBuilder result)
    {
        var ok = true;
        foreach (string s in _dict.Keys)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                ok = false;
                result.AppendLine("Empty or whitespace key found");
            }
            else
            {
                var value = _dict[s].TrimStart();
                if (value.StartsWith('@'))
                {
                    ok = ok && ValidateScript(value, result);
                }
            }
        }
        return ok;
    }

    /// <summary>
    /// Validates one script to make sure the syntax is correct.
    /// </summary>
    public bool ValidateScript(string script, StringBuilder result)
    {
        if (string.IsNullOrWhiteSpace(script))
        {
            return true;
        }
        try
        {
            var tokens = SplitTokens(script);
            int index = 0;
            int parenLevel = 0;
            int ifCount = 0;
            int elseifCount = 0;
            int thenCount = 0;
            int endifCount = 0;
            int forLevel = 0;
            int forEachKeyLevel = 0;
            int forEachListLevel = 0;
            string ifLast = ENDIF;
            while (index < tokens.Length)
            {
                if (tokens[index].Equals(IF, OIC))
                {
                    if (ifLast != THEN && ifLast != ELSE && ifLast != ENDIF)
                    {
                        throw new SystemException($"{IF} at {index} is invalid.");
                    }
                    ifCount++;
                    ifLast = IF;
                }
                else if (tokens[index].Equals(THEN, OIC))
                {
                    if (ifLast != IF & ifLast != ELSEIF)
                    {
                        throw new SystemException($"{THEN} at {index} is invalid.");
                    }
                    thenCount++;
                    ifLast = THEN;
                }
                else if (tokens[index].Equals(ELSEIF, OIC))
                {
                    if (ifLast != THEN && ifLast != ENDIF)
                    {
                        throw new SystemException($"{ELSEIF} at {index} is invalid.");
                    }
                    elseifCount++;
                    ifLast = ELSEIF;
                }
                else if (tokens[index].Equals(ELSE, OIC))
                {
                    if (ifLast != THEN && ifLast != ENDIF)
                    {
                        throw new SystemException($"{ELSE} at {index} is invalid.");
                    }
                    ifLast = ELSE;
                }
                else if (tokens[index].Equals(ENDIF, OIC))
                {
                    if (ifLast != THEN && ifLast != ELSE && ifLast != ENDIF)
                    {
                        throw new SystemException($"{ENDIF} at {index} is invalid.");
                    }
                    endifCount++;
                    ifLast = ENDIF;
                }
                else if (tokens[index].Equals(FOR, OIC))
                {
                    forLevel++;
                }
                else if (tokens[index].Equals(ENDFOR, OIC))
                {
                    forLevel--;
                }
                else if (tokens[index].Equals(FOREACHKEY, OIC))
                {
                    forEachKeyLevel++;
                }
                else if (tokens[index].Equals(ENDFOREACHKEY, OIC))
                {
                    forEachKeyLevel--;
                }
                else if (tokens[index].Equals(FOREACHLIST, OIC))
                {
                    forEachListLevel++;
                }
                else if (tokens[index].Equals(ENDFOREACHLIST, OIC))
                {
                    forEachListLevel--;
                }
                if (tokens[index].StartsWith('@'))
                {
                    if (tokens[index].EndsWith('('))
                    {
                        parenLevel++;
                    }
                    if (KEYWORDS.Contains(tokens[index]))
                    {
                        index++;
                        continue;
                    }
                    if (tokens[index].EndsWith('('))
                    {
                        var dict = GetByPrefix(tokens[index]);
                        if (dict.Count == 0)
                        {
                            result.AppendLine($"Token not found: {tokens[index]}");
                            return false;
                        }
                    }
                    else
                    {
                        var value = Get(tokens[index]);
                        if (value == "")
                        {
                            result.AppendLine($"Token not found: {tokens[index]}");
                            return false;
                        }
                    }
                }
                else
                {
                    if (tokens[index] == ")")
                    {
                        parenLevel--;
                    }
                }
                index++;
            }
            if (parenLevel != 0)
            {
                result.AppendLine("Mismatched parenthesis");
                return false;
            }
            if (ifCount != endifCount)
            {
                result.AppendLine($"Mismatched {IF}/{ENDIF} counts");
                return false;
            }
            if (ifCount + elseifCount != thenCount)
            {
                result.AppendLine($"Mismatched {IF}/{ELSEIF} vs {THEN} counts");
                return false;
            }
            if (forLevel != 0)
            {
                result.AppendLine($"Mismatched {FOR.Replace("(", "")}/{ENDFOR}");
                return false;
            }
            if (forEachKeyLevel != 0)
            {
                result.AppendLine($"Mismatched {FOREACHKEY.Replace("(", "")}/{ENDFOREACHKEY}");
                return false;
            }
            if (forEachListLevel != 0)
            {
                result.AppendLine($"Mismatched {FOREACHLIST.Replace("(", "")}/{ENDFOREACHLIST}");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            result.AppendLine(ex.Message);
            return false;
        }
    }

    private readonly List<string> KEYWORDS =
    [
        ABS,
        ADD,
        ADDLIST,
        ADDTO,
        AND,
        CLEARARRAY,
        CLEARLIST,
        COMMENT,
        CONCAT,
        DIV,
        DIVTO,
        ELSE,
        ELSEIF,
        ENDFOR,
        ENDFOREACHKEY,
        ENDFOREACHLIST,
        ENDIF,
        EQ,
        EXEC,
        FALSE,
        FOR,
        FOREACHKEY,
        FOREACHLIST,
        FORMAT,
        GE,
        GET,
        GETARRAY,
        GETINCHANNEL,
        GETLIST,
        GETVALUE,
        GT,
        IF,
        INSERTATLIST,
        ISBOOL,
        ISNULL,
        ISSCRIPT,
        LE,
        LISTLENGTH,
        LOWER,
        LT,
        MOD,
        MODTO,
        MSG,
        MUL,
        MULTO,
        NE,
        NL,
        NOT,
        OR,
        RAND,
        REMOVEATLIST,
        REPLACE,
        RND,
        SCRIPT,
        SET,
        SETARRAY,
        SETLIST,
        SETOUTCHANNEL,
        SUB,
        SUBSTRING,
        SUBTO,
        SWAP,
        THEN,
        TRIM,
        TRUE,
        UPPER,
        WRITE,
    ];
}