using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Rusty.ISA;

/// <summary>
/// An utility class that can serialize programs.
/// </summary>
public static class ProgramDeserializer
{
    /* Public methods. */
    /// <summary>
    /// Convert CSV string into a program.
    /// </summary>
    public static Program Deserialize(string csv, InstructionSet instructionSet)
    {
        List<InstructionInstance> instructions = new();

        // Parse CSV.
        StringReader reader = new StringReader(csv);
        TextFieldParser parser = new(reader);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        while (!parser.EndOfData)
        {
            // Get cells.
            string[] fields = parser.ReadFields();
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = FromCsv(fields[i]);
            }

            // Copy opcode.
            string opcode = fields.Length > 0 ? fields[0] : "";

            // Figure out argument count.
            int argCount = fields.Length - 1;
            if (argCount < 0)
                argCount = 0;

            if (instructionSet != null)
            {
                InstructionDefinition definition = instructionSet[opcode];
                argCount = definition.Parameters.Length;
            }

            // Copy arguments.
            string[] args = new string[argCount];
            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 < fields.Length)
                    args[i] = fields[i + 1];
                else
                    args[i] = "";
            }

            // Create instruction instance.
            instructions.Add(new(fields[0], args));
        }

        // Create program.
        return new(instructions.ToArray());
    }

    /* Private methods. */
    /// <summary>
    /// Interpret "\\n" as "\n" and "\n" as line-breaks.
    /// </summary>
    private static string FromCsv(string cell)
    {
        for (int i = 1; i < cell.Length; i++)
        {
            if (i > 1 && cell[i - 2] == '\\' && cell[i - 1] == '\\' && cell[i] == 'n')
                cell = cell.Substring(0, i - 2) + "\\n" + cell.Substring(i + 1);
            if (cell[i - 1] == '\\' && cell[i] == 'n')
                cell = cell.Substring(0, i - 1) + '\n' + cell.Substring(i + 1);
        }

        return cell;
    }
}