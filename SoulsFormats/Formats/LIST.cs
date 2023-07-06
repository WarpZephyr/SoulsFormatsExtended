using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// An install list for installing files from game directory to HDD, seen named install.list
    /// </summary>
    public class LIST
    {
        /// <summary>
        /// The total byte size of all files within the list.
        /// </summary>
        public long TotalSize { get; set; } = 0;

        /// <summary>
        /// The name of a properties file, purpose unknown.
        /// </summary>
        public string PropertiesName { get; set; } = string.Empty;

        /// <summary>
        /// Relative paths to all files.
        /// </summary>
        public List<string> Paths { get; set; } = new List<string>();

        /// <summary>
        /// Create a new LIST from file read on the chosen path.
        /// </summary>
        /// <param name="path">The full path to a list file.</param>
        public LIST(string path)
        {
            var lines = new List<string>(File.ReadAllLines(path));
            TotalSize = long.Parse(lines[0]);
            PropertiesName = lines[1];
            lines.RemoveAt(0);
            lines.RemoveAt(1);
            Paths = lines;
        }

        /// <summary>
        /// Write a LIST to a path.
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            File.WriteAllLines(path, Write());
        }

        /// <summary>
        /// Write a LIST to a string array.
        /// </summary>
        /// <returns>A string array.</returns>
        public string[] Write()
        {
            var lines = new List<string>();
            lines.Add(TotalSize.ToString());
            lines.Add(PropertiesName);
            lines.AddRange(Paths);
            return lines.ToArray();
        }

        /// <summary>
        /// Whether or not a file on the chosen path is a LIST.
        /// </summary>
        /// <param name="path">The full path to a list file.</param>
        /// <returns></returns>
        public bool Is(string path)
        {
            return path.EndsWith("install.list") || path.EndsWith(".list");
        }

        /// <summary>
        /// Get all of the files mentioned in the LIST provided the directory they begin in.
        /// </summary>
        /// <param name="dir">The directory the relative paths in the LIST start in.</param>
        /// <param name="verifySize">Verify the combined size of all files is the same as the total size in the LIST, throwing if not.</param>
        /// <returns>A list of all files in the LIST.</returns>
        public List<byte[]> GetFiles(string dir, bool verifySize)
        {
            var files = new List<byte[]>();
            long byteCount = 0;
            foreach (string relPath in Paths)
            {
                string filePath = Path.Combine(dir, relPath);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("A file could not be found on the given path.");
                }
                else
                {
                    var file = File.ReadAllBytes(filePath);
                    byteCount += file.LongLength;
                    files.Add(file);
                }
            }

            if (verifySize && byteCount != TotalSize)
                throw new InvalidDataException($"Combined file size did not match {nameof(TotalSize)} in {nameof(LIST)}");

            return files;
        }

        /// <summary>
        /// Set the detected total size of all the files on the relative paths in the LIST provided the directory they begin in.
        /// </summary>
        /// <param name="dir">The directory the relative paths in the LIST start in.</param>
        public void SetTotalSize(string dir)
        {
            TotalSize = GetTotalSize(dir);
        }

        /// <summary>
        /// Get the total size of all the files mentioned in the LIST provided the directory they begin in.
        /// </summary>
        /// <param name="dir">The directory the relative paths in the LIST start in.</param>
        /// <returns>The total size of all files on relative paths.</returns>
        public long GetTotalSize(string dir)
        {
            long byteCount = 0;
            foreach (string relPath in Paths)
            {
                string filePath = Path.Combine(dir, relPath);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("A file could not be found on the given path.");
                }
                else
                {
                    byteCount += new FileInfo(filePath).Length;
                }
            }
            return byteCount;
        }
    }
}
