using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Rusty.ISA;

/// <summary>
/// A loader utility for instruction icons.
/// </summary>
public static class IconLoader
{
    /// <summary>
    /// Load an image. If "isAlphaTexture" is enabled, then the image will be converted from black-and-white without alpha
    /// into solid white with alpha. Supported file formats: .bmp, .png, .jpg, .tga, .svg, .webp, .ktx.
    /// </summary>
    public static Texture2D Load(string globalPath, bool isAlphaTexture = false)
    {
        byte[] bytes = null;

        // Read file if it exists...
        if (File.Exists(globalPath))
            bytes = File.ReadAllBytes(globalPath);

        // Else, check if it's a zip entry.
        else if (globalPath.Contains(".zip"))
        {
            // Split string into sub-paths for each nested ZIP file.
            string delimitedPath = globalPath.Replace(".zip\\", ".zip>>").Replace(".zip/", ".zip>>");
            string[] parts = delimitedPath.Split(">>");

            // Open top-level ZIP.
            Stream stream = File.OpenRead(parts[0]);

            // Loop through each nested ZIP until we reach the image file.
            for (int i = 1; i < parts.Length; i++)
            {
                // Sanitize path.
                parts[i] = parts[i].Replace("\\", "/");

                // Read archive.
                using ZipArchive archive = new(stream, ZipArchiveMode.Read, true);
                ZipArchiveEntry entry = archive.GetEntry(parts[i]);

                if (entry == null)
                    break;

                // Open memory stream to archive entry.
                using Stream entryStream = entry.Open();
                MemoryStream memoryStream = new MemoryStream();
                entryStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                stream.Dispose();
                stream = memoryStream;

                // If the last part, read bytes.
                if (i == parts.Length - 1)
                {
                    bytes = memoryStream.ToArray();
                    stream.Dispose();
                }
            }
        }

        // Read bytes as image.
        if (bytes != null)
        {
            // Create image.
            string lowercase = globalPath.ToLower();
            Image image = new();
            if (lowercase.EndsWith(".bmp"))
                image.LoadBmpFromBuffer(bytes);
            else if (lowercase.EndsWith(".png"))
                image.LoadPngFromBuffer(bytes);
            else if (lowercase.EndsWith(".jpg") || lowercase.EndsWith(".jpeg"))
                image.LoadJpgFromBuffer(bytes);
            else if (lowercase.EndsWith(".tga"))
                image.LoadTgaFromBuffer(bytes);
            else if (lowercase.EndsWith(".svg"))
                image.LoadSvgFromBuffer(bytes);
            else if (lowercase.EndsWith(".webp"))
                image.LoadWebpFromBuffer(bytes);
            else if (lowercase.EndsWith(".ktx"))
                image.LoadKtxFromBuffer(bytes);
            else
                throw new IOException($"The file at '{globalPath}' had an unknown file extension and could not be loaded.");

            // If the source image is an alpha texture, convert to color.
            if (isAlphaTexture)
                AlphaToColor(image);

            // Create texture.
            ImageTexture texture = new ImageTexture();
            texture.SetImage(image);

            return texture;
        }

        // If the file did not exist, return nothing.
        else
        {
            GD.PrintErr($"Could not find icon '{globalPath}'!");
            return null;
        }
    }

    /// <summary>
    /// Interpret a black-and-white texture as alpha and convert it to a white texture with alpha.
    /// </summary>
    public static Texture2D AlphaToColor(Texture2D texture)
    {
        return AlphaToColor(texture, new Color(1f, 1f, 1f));
    }

    /// <summary>
    /// Interpret a black-and-white texture as alpha and convert it to a mono-colored texture with alpha.
    /// </summary>
    public static Texture2D AlphaToColor(Texture2D texture, Color color)
    {
        Image image = texture.GetImage();
        if (image.DetectAlpha() == Image.AlphaMode.None)
            image.Convert(Image.Format.Rgba8);
        AlphaToColor(image, color);
        ImageTexture result = new();
        result.SetImage(image);
        return result;
    }

    /// <summary>
    /// Interpret a black-and-white image as alpha and convert it to a white image with alpha.
    /// </summary>
    public static void AlphaToColor(Image image)
    {
        AlphaToColor(image, new Color(1f, 1f, 1f));
    }

    /// <summary>
    /// Interpret a black-and-white image as alpha and convert it to a mono-colored image with alpha.
    /// </summary>
    public static void AlphaToColor(Image image, Color color)
    {
        for (int j = 0; j < image.GetHeight(); j++)
        {
            for (int i = 0; i < image.GetWidth(); i++)
            {
                image.SetPixel(i, j, new Color(color.R, color.G, color.B, image.GetPixel(i, j).R));
            }
        }
    }
}