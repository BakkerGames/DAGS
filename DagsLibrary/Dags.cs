﻿using System.Text;
using static DagsLibrary.Constants;

namespace DagsLibrary;

/// <summary>
/// Generate a DAGS script object and assign its dictionary.
/// </summary>
public partial class Dags(IDictionary<string, string> dict)
{
    /// <summary>
    /// Receives metadata from the calling program, such as text input.
    /// </summary>
    public Queue<string> InChannel { get; set; } = new();

    /// <summary>
    /// Sends metadata or commands back to the calling program.
    /// </summary>
    public Queue<string> OutChannel { get; set; } = new();

    /// <summary>
    /// Run one script and return any text in result.
    /// </summary>
    public void RunScript(string script, StringBuilder result)
    {
        if (string.IsNullOrWhiteSpace(script) || script.Equals(NULL_VALUE, OIC))
        {
            return;
        }
        try
        {
            var tokens = SplitTokens(script);
            int index = 0;
            while (index < tokens.Length)
            {
                RunOneCommand(tokens, ref index, result);
            }
        }
        catch (Exception ex)
        {
            throw new SystemException($"{ex.Message}{Environment.NewLine}{script}");
        }
    }

    /// <summary>
    /// Format the script with line breaks and indents.
    /// </summary>
    public static string PrettyScript(string script)
    {
        if (!script.TrimStart().StartsWith('@'))
        {
            return script;
        }

        StringBuilder result = new();
        int indent = 1;
        int parens = 0;
        bool ifLine = false;
        bool forLine = false;
        bool forEachKeyLine = false;
        bool forEachListLine = false;
        var tokens = SplitTokens(script);

        foreach (string s in tokens)
        {
            switch (s)
            {
                case ELSEIF:
                    indent--;
                    break;
                case ELSE:
                    indent--;
                    break;
                case ENDIF:
                    indent--;
                    break;
                case ENDFOR:
                    indent--;
                    break;
                case ENDFOREACHKEY:
                    indent--;
                    break;
                case ENDFOREACHLIST:
                    indent--;
                    break;
            }
            if (parens == 0)
            {
                if (ifLine)
                {
                    result.Append(' ');
                }
                else
                {
                    result.AppendLine();
                    if (indent > 0) result.Append(new string('\t', indent));
                }
            }
            result.Append(s);
            switch (s)
            {
                case IF:
                    ifLine = true;
                    break;
                case ELSEIF:
                    ifLine = true;
                    break;
                case ELSE:
                    indent++;
                    break;
                case THEN:
                    indent++;
                    ifLine = false;
                    break;
                case FOR:
                    forLine = true;
                    break;
                case FOREACHKEY:
                    forEachKeyLine = true;
                    break;
                case FOREACHLIST:
                    forEachListLine = true;
                    break;
            }
            if (s.EndsWith('('))
            {
                parens++;
            }
            else if (s == ")")
            {
                parens--;
                if (forLine && parens == 0)
                {
                    forLine = false;
                    indent++;
                }
                else if (forEachKeyLine && parens == 0)
                {
                    forEachKeyLine = false;
                    indent++;
                }
                else if (forEachListLine && parens == 0)
                {
                    forEachListLine = false;
                    indent++;
                }
            }
        }
        if (indent != 1)
        {
            throw new SystemException($"Indent should be 1 at end of script: {indent}\n{script}");
        }
        if (parens != 0)
        {
            throw new SystemException($"Parenthesis should be 0 at end of script: {parens}\n{script}");
        }
        return result.ToString();
    }
}