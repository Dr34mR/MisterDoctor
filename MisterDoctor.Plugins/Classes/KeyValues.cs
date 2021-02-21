using System.Collections.Generic;

namespace MisterDoctor.Plugins.Classes
{
    public class KeyValues : List<KeyValue>
    {
        public KeyValues()
        {
            
        }

        public KeyValues(IEnumerable<KeyValue> list)
        {
            AddRange(list);
        }
    }
}