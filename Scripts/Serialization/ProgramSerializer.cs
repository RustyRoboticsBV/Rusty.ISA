namespace Rusty.ISA;

/// <summary>
/// An utility class that can serialize programs.
/// </summary>
public static class ProgramSerializer
{
    /* Public methods. */
    /// <summary>
    /// Convert a program into a CSV string.
    /// </summary>
    public static string Serialize(Program program, InstructionSet instructionSet = null)
    {
        string csv = "";
        for (int i = 0; i < program.Length; i++)
        {
            // Add one line per instruction.
            if (csv.Length > 0)
                csv += '\n';

            // Copy opcode.
            csv += ToCsv(program[i].Opcode);

            // Figure out argument count.
            int argCount = program[i].Arguments.Length;
            if (instructionSet != null)
            {
                InstructionDefinition definition = instructionSet[program[i].Opcode];
                if (definition.Parameters.Length != argCount)
                    argCount = definition.Parameters.Length;
            }

            // Copy arguments.
            for (int j = 0; j < argCount; j++)
            {
                if (j < program[i].Arguments.Length)
                    csv += $",{ToCsv(program[i].Arguments[j])}";
                else
                    csv += ",";
            }
        }
        return csv;
    }

    /* Private methods. */
    /// <summary>
    /// Process a cell to be CSV-safe. We also remove line-breaks and replace them with "\n", forcing us to replace
    /// pre-existing occurances of "\n" with "\\n".
    /// </summary>
    private static string ToCsv(string cell)
    {
        cell = cell.Replace("\\n", "\\\n");
        cell = cell.Replace("\n", "\\n");
        cell = cell.Replace("\"", "\"\"");

        if (cell.Contains(',') || cell.Contains('\"'))
            cell = $"\"{cell}\"";

        return cell;
    }
}