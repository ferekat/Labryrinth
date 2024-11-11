using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Persistence
{
    public class FileReader : IFileReader
    {
        private int size;
        private string[]? fileContent;

        public int Size { get { return size; } }
        public string[] FileContent { get { return fileContent!; } }
        public FileReader(int size) 
        {
            this.size = size;
        }

        public string[] ReadFile(int size)
        {
            try
            {
                switch (size)
                {
                    case 5:
                        fileContent = File.ReadAllLines("easy.txt");
                        break;
                    case 10:
                        fileContent = File.ReadAllLines("medium.txt");
                        break;
                    case 15:
                        fileContent = File.ReadAllLines("hard.txt");
                        break;
                    default:
                        throw new InvalidDataException("Illegal table size!");
                }
                return fileContent;
            }
            catch (Exception ex) { 
                throw new Exception($"Could not open file: {ex.Message}");
            }
        }

    }
}
