namespace Domain.FileModelType
{
    public class MapReduceWord
    {
        public string Word { get; set; }

        public int Occurrences { get; set; }

        public MapReduceWord(string word, int occurrences)
        {
            this.Word = word;
            this.Occurrences = occurrences;
        }
    }
}