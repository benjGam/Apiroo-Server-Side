namespace ApirooServer.Utils.MemoryBlock
{
    public abstract class MemoryBlock
    {
        protected virtual Dictionary<string, string> _keyPairs { get; } = new Dictionary<string, string>();
        protected abstract string _name { get; }
        public (string key, string value) GetSectionById(int id)
        {
            if (id >= 0 && id <= _keyPairs.Count)
                return (_keyPairs.Keys.ToList()[id], _keyPairs[_keyPairs.Keys.ToList()[id]]);
            return (string.Empty, string.Empty);
        }
        public (string key, string value) GetSectionByKey(string key)
        {
            if (_keyPairs.ContainsKey(key))
                return (key, _keyPairs[key]);
            return (string.Empty, string.Empty);
        }
        public void Read(ref StreamReader Reader)
        {
            List<string> fileContent = new List<string>();
            int BlockSize = int.Parse(Reader.ReadLine()!.Split('&')[1].Replace(" ", string.Empty));
            for(int i = 0; i < BlockSize; i++)
            {
                string CompleteSection = Reader.ReadLine()!;
                _keyPairs[CompleteSection.Split(':')[0].Replace(" ", string.Empty)] = CompleteSection.Split(':')[1].Replace(" ", string.Empty);
            }
        }
        public override string ToString()
        {
            string toReturn = $"::{_name} & {_keyPairs.Keys.Count}\n";
            foreach (string key in _keyPairs.Keys)
                toReturn += $"{key}:{_keyPairs[key]}\n";
            return toReturn;
        }
    }
}
