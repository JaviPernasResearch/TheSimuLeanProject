using System.IO;
using System.Text;
using UnityEngine;

namespace UnitySimuLean.Utilities
{
    public class FileWriterCSV
    {
        private string filePath;

        public FileWriterCSV(string fileName)
        {
            string projectRoot = Application.dataPath;
            filePath = Path.Combine(projectRoot, fileName +".csv");

            // Create the CSV file if it does not exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        public void AddText(string[] values)
        {
            string line = string.Join(",", values);
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(line);
            }
        }

        public void ClearFile()
        {
            File.WriteAllText(filePath, string.Empty);
        }

        public string GetFilePath()
        {
            return filePath;
        }
    }
}