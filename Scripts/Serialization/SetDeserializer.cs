using Godot;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Rusty.ISA;

/// <summary>
/// A class for deserializing zip files into instruction sets.
/// </summary>
public static class SetDeserializer
{
    /* Public methods. */
    /// <summary>
    /// Deserialize a zip file into an instruction set.
    /// </summary>
    public static InstructionSet Deserialize(string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        string folderPath = Path.GetDirectoryName(filePath);
        string fileName = Path.GetFileName(filePath);
        return Deserialize(bytes, folderPath, fileName);
    }

    public static InstructionSet Deserialize(byte[] bytes, string folderPath, string fileName)
    {
        List<InstructionDefinition> definitions = new();
        List<InstructionSet> modules = new();
        GD.Print("Unpacking " + folderPath + "\\" + fileName);

        using MemoryStream zipStream = new(bytes);
        using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);

        // Initialize metadata.
        string name = "";
        string description = "";
        string author = "";
        string version = "";

        // Read entries.
        foreach (var entry in archive.Entries)
        {
            string lowercase = entry.FullName.ToLower();
            string pathToEntry = folderPath + $"\\{fileName}\\" + entry.FullName;

            using Stream entryStream = entry.Open();

            // TXT file.
            if (lowercase.EndsWith(".txt"))
            {
                using StreamReader metadataReader = new(entryStream);
                string metadata = metadataReader.ReadToEnd();

                name = ExtractMetadata(metadata, "Name");
                description = ExtractMetadata(metadata, "Description");
                author = ExtractMetadata(metadata, "Author");
                version = ExtractMetadata(metadata, "Version");
            }

            // XML file.
            else if (lowercase.EndsWith(".xml"))
            {
                using StreamReader reader = new(entryStream);
                string xmlContent = reader.ReadToEnd();
                InstructionDefinition definition = (InstructionDefinition)XmlDeserializer.Deserialize(xmlContent,
                    Path.GetDirectoryName(pathToEntry));
                definitions.Add(definition);
            }

            // Nested ZIP file.
            else if (lowercase.EndsWith(".zip"))
            {
                using (MemoryStream nestedZipStream = new())
                {
                    entryStream.CopyTo(nestedZipStream);
                    byte[] nestedBytes = nestedZipStream.ToArray();

                    InstructionSet module = Deserialize(nestedBytes,
                        Path.GetDirectoryName(pathToEntry),
                        Path.GetFileName(pathToEntry));
                    modules.Add(module);
                }
            }
        }

        // Combine into instruction set.
        return new(name, description, author, version, definitions.ToArray(), modules.ToArray());
    }

    /* Private methods. */
    private static string? ExtractMetadata(string input, string key)
    {
        string pattern = $@"{Regex.Escape(key)}=""(.*?)""";
        Match match = Regex.Match(input, pattern, RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : "";
    }
}