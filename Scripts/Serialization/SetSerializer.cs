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
    /// <summary>
    /// Serialize an instruction set into a byte array that can be written to a file.
    /// </summary>
    public static byte[] Serialize(InstructionSet set)
    {
        // Create new ZIP archive.
        MemoryStream stream = new();
        ZipArchive archive = new(stream, ZipArchiveMode.Update, true);

        // Add metadata file.
        string metadata = $"Name=\"{set.Name}\"\nDesc=\"{set.Description}\"\nAuthor=\"{set.Author}\"\nVersion=\"{set.Version}\"";
        ZipArchiveEntry metadataEntry = archive.CreateEntry("Metadata.txt");
        Stream metadataStream = metadataEntry.Open();
        metadataStream.Write(Encoding.UTF8.GetBytes(metadata));

        // Add definitions.
        foreach (InstructionDefinition definition in set.Local)
        {
            // Get category.
            string category = definition.Category;

            // Get opcode.
            string opcode = definition.Opcode;
            if (opcode == "")
            {
                GD.PrintErr($"Encountered definition without opcode while serializing instruction set '{set.Name}'! "
                    + "The definition was skipped.");
                continue;
            }

            // Add to archive.
            try
            {
                // Create folder.
                string folderPath = (category != "" && set.Name != category) ? $"{category}/{opcode}" : $"{opcode}";
                ZipArchiveEntry folder = archive.CreateEntry(folderPath + "/");

                // Serialize definition.
                string xml = XmlSerializer.Serialize(definition);

                // Create definition file.
                ZipArchiveEntry definitionEntry = archive.CreateEntry($"{folderPath}/Def.xml");
                Stream definitionStream = definitionEntry.Open();
                definitionStream.Write(Encoding.UTF8.GetBytes(xml));
                definitionStream.Close();

                // Create icon file.
                if (definition.Icon != null)
                {
                    ZipArchiveEntry iconEntry = archive.CreateEntry($"{folderPath}/Icon.png");
                    Stream iconStream = iconEntry.Open();
                    iconStream.Write(definition.Icon.GetImage().SavePngToBuffer());
                    iconStream.Close();
                }
            }
            catch (Exception exception)
            {
                GD.PrintErr($"Could not add instruction '{opcode}' to serialized instruction set '{set.Name}' due to "
                    + $"exception: {exception.Message}.");
            }
        }

        // Add modules.
        foreach (InstructionSet module in set.Modules)
        {
            ZipArchiveEntry moduleEntry = archive.CreateEntry(module.Name + ".zip");
            Stream definitionStream = moduleEntry.Open();
            definitionStream.Write(Serialize(module));
            definitionStream.Close();
        }

        // Close and return finished archive as a byte array.
        archive.Dispose();
        stream.Flush();
        byte[] bytes = stream.ToArray();
        stream.Close();
        return bytes;
    }
}