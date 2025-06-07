using Godot;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Rusty.ISA;

/// <summary>
/// A serializer for instruction sets.
/// </summary>
public static class SetSerializer
{
    public static byte[] Serialize(InstructionSet set)
    {
        // Create new ZIP archive.
        MemoryStream stream = new();
        ZipArchive archive = new(stream, ZipArchiveMode.Update, true);

        // Add definitions.
        foreach (InstructionDefinition definition in set.Local)
        {
            // Get category.
            string category = definition.Category;
            if (category == "")
                category = "uncategorized";

            // Get opcode.
            string opcode = definition.Opcode;
            if (opcode == "")
            {
                GD.PrintErr($"Encountered definition without opcode while serializing instruction set {set.ResourceName}! "
                    + "The definition was skipped.");
                continue;
            }

            // Add to archive.
            try
            {
                // Create folder.
                ZipArchiveEntry folder = archive.CreateEntry($"{category}/{opcode}/");

                // Serialize definition.
                InstructionDefinitionDescriptor descriptor = Descriptor.FromObject(definition) as InstructionDefinitionDescriptor;
                if (descriptor.IconPath != "")
                    descriptor.IconPath = "Icon.png";
                string xml = descriptor.GenerateXml("");

                // Create definition.
                ZipArchiveEntry definitionEntry = archive.CreateEntry($"{category}/{opcode}/Def.xml");
                Stream definitionStream = definitionEntry.Open();
                definitionStream.Write(Encoding.UTF8.GetBytes(xml));
                definitionStream.Close();

                // Create icon.
                if (definition.Icon != null)
                {
                    ZipArchiveEntry iconEntry = archive.CreateEntry($"{category}/{opcode}/Icon.png");
                    Stream iconStream = iconEntry.Open();
                    iconStream.Write(definition.Icon.GetImage().SavePngToBuffer());
                    iconStream.Close();
                }
            }
            catch (Exception exception)
            {
                GD.PrintErr($"Could not add instruction {opcode} to serialized instruction set {set.ResourceName} due to "
                    + $"exception: {exception.Message}.");
            }
        }

        // Close and return finished archive as a byte array.
        archive.Dispose();
        stream.Flush();
        byte[] bytes = stream.ToArray();
        stream.Close();
        return bytes;
    }
}