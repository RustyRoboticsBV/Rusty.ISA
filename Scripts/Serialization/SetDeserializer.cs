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
    public static InstructionSet Deserialize(string zipFilePath)
    {
        using (FileStream fileStream = File.OpenRead(zipFilePath))
        {
            return Deserialize(fileStream, Path.GetDirectoryName(zipFilePath));
        }
    }

    /* Private methods. */
    private static InstructionSet Deserialize(Stream zipStream, string folderPath)
    {
        List<InstructionDefinition> definitions = new();
        List<InstructionSet> modules = new();

        // Read zip archive.
        using (ZipArchive archive = new(zipStream, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                string lowercase = entry.FullName.ToLower();

                // XML file.
                if (lowercase.EndsWith(".xml"))
                {
                    using (Stream xmlStream = entry.Open())
                    using (StreamReader reader = new(xmlStream))
                    {
                        string xmlContent = reader.ReadToEnd();
                        XmlDeserializer.Deserialize(xmlContent, folderPath);
                    }
                }

                // Nested ZIP files.
                else if (lowercase.EndsWith(".zip"))
                {
                    using (var nestedZipStream = new MemoryStream())
                    {
                        using (var entryStream = entry.Open())
                        {
                            entryStream.CopyTo(nestedZipStream);
                        }
                        nestedZipStream.Seek(0, SeekOrigin.Begin);

                        InstructionSet module = Deserialize(nestedZipStream, folderPath);
                    }
                }
            }

            // Combine into instruction set.
            return new(definitions.ToArray(), modules.ToArray());
        }
    }
}