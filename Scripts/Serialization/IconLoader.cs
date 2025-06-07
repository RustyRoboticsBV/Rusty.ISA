using Godot;
using System.IO;

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
        // If the file exists...
        if (File.Exists(globalPath))
        {
            // Read file.
            byte[] bytes = File.ReadAllBytes(globalPath);

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
            return null;
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