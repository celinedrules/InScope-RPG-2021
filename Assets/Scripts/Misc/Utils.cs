using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Misc
{
    public static class Utils
    {
        private static string ReadToEnd(this TextReader reader, int lineLength)
        {
            return string.Join(System.Environment.NewLine, reader.ReadLines(lineLength));
        }

        private static IEnumerable<string> ReadLines(this TextReader reader, int lineLength)
        {
            var line = new StringBuilder();
            foreach (var word in reader.ReadWords())
                if (line.Length + word.Length < lineLength)
                    line.Append($"{word} ");
                else
                {
                    yield return line.ToString().Trim();
                    line = new StringBuilder($"{word} ");
                }

            if (line.Length > 0)
                yield return line.ToString().Trim();
        }

        private static IEnumerable<string> ReadWords(this TextReader reader)
        {
            while (!reader.IsEof())
            {
                var word = new StringBuilder();
                while (!reader.IsBreak())
                {
                    word.Append(reader.Text());
                    reader.Read();
                }

                reader.Read();
                if (word.Length > 0)
                    yield return word.ToString();
            }
        }

        private static bool IsBreak(this TextReader reader) => reader.IsEof() || reader.IsWhiteSpace();
        private static bool IsWhiteSpace(this TextReader reader) => string.IsNullOrWhiteSpace(reader.Text());
        private static string Text(this TextReader reader) => char.ConvertFromUtf32(reader.Peek());
        private static bool IsEof(this TextReader reader) => reader.Peek() == -1;
    
        public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
        {
            if (null == texture) return;

            string assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (tImporter == null)
                return;
        
            tImporter.textureType = TextureImporterType.Default;

            tImporter.isReadable = isReadable;

            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    
        public static string Wrap(this string text, int lineLength)
        {
            using var reader = new StringReader(text);
            return reader.ReadToEnd(lineLength);
        }
        
        public static string SplitLineToMultiline(string input, int rowLength)
        {        
            StringBuilder result = new StringBuilder();
            StringBuilder line = new StringBuilder();

            Stack<string> stack = new Stack<string>(input.Split(' '));

            while ( stack.Count > 0 )
            {
                var word = stack.Pop();
                if ( word.Length > rowLength )
                {
                    string head = word.Substring(0, rowLength);
                    string tail = word.Substring(rowLength);

                    word = head;
                    stack.Push(tail);
                }

                if ( line.Length + word.Length > rowLength)
                {
                    result.AppendLine(line.ToString());
                    line.Clear();
                }

                line.Append(word + " ");
            }

            result.Append(line);
            return result.ToString();
        }

        public static void MarkAsDirty(Object target)
        {
            EditorUtility.SetDirty(target);
        }
        
        public static string ToTinyUuid(this Guid guid)
        {
            string id = Convert.ToBase64String(guid.ToByteArray());
            return id.Substring(0, id.Length - 2).Replace('+', '-').Replace('/', '_');
        }
    }
}