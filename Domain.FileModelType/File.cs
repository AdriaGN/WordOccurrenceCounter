using System.Collections.Generic;

namespace Domain.FileModelType
{
    public class File
    {
        public string Name { get; set; }

        public Dictionary<string, int> WordOccurrences { get; set; }
    }
}
