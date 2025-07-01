using Godot;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

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

        using (MemoryStream zipStream = new(bytes))
        using (ZipArchive archive = new(zipStream, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                string lowercase = entry.FullName.ToLower();
                string pathToEntry = folderPath + $"\\{fileName}\\" + entry.FullName;

                using (Stream entryStream = entry.Open())
                {
                    // XML file.
                    if (lowercase.EndsWith(".xml"))
                    {
                        using (StreamReader reader = new(entryStream))
                        {
                            string xmlContent = reader.ReadToEnd();
                            InstructionDefinition definition = (InstructionDefinition)XmlDeserializer.Deserialize(xmlContent,
                                Path.GetDirectoryName(pathToEntry));
                            definitions.Add(definition);
                        }
                    }

                    // Nested ZIP files.
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
            }
        }

        // Combine into instruction set.
        string name = fileName.Replace(".zip", "");
        return new(name, definitions.ToArray(), modules.ToArray());
    }
}