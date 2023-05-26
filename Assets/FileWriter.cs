using System.IO;
using System.Text;
using UnityEngine;

namespace UnitySimuLean.Utilities
{
    public class FileWriter
    {
        private string filePath;

        public FileWriter(string fileName)
        {
            string projectRoot = Application.dataPath;
            filePath = Path.Combine(projectRoot, fileName+".txt");

            // Create the file if it does not exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                ClearFile();
            }
        }

        public void AddText(string text)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine(text);
            }
        }

        public void ClearFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Flush();
            }
        }
    }
}