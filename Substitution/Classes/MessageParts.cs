using System.Collections.Generic;
using System.Linq;
using System.Text;
using Substitution.Managers;

namespace Substitution.Classes
{
    public class MessageParts : List<MessagePart>
    {
        private readonly List<char> _delims = new List<char> {' ', '?', '!', ',', '.', '\"'};

        public MessageParts(string message)
        {
            var finalSplit = new List<string> {message.Trim()};

            // break message into components based on delimiters, but preserve each
            // delimiter in their own seperate element (for joining later)

            // The math is WAY easier if you work backwards with the removal / insertion 
            // that way the indexes dont move

            foreach (var delim in _delims)
            {
                var itemCount = finalSplit.Count;

                for (var i = itemCount - 1; i >= 0; i--)
                {
                    var split = finalSplit[i].Split(delim).ToList();
                    var splitCount = split.Count - 1;
                    
                    for (var j = splitCount; j > 0; j--)
                    {
                        split.Insert(j, delim.ToString());
                    }

                    finalSplit.RemoveAt(i);
                    finalSplit.InsertRange(i, split);
                }
            }

            // Remove null strings

            finalSplit.RemoveAll(string.IsNullOrEmpty);

            // Add the message parts

            foreach (var value in finalSplit)
            {
                Add(new MessagePart
                {
                    Value = value,
                    IsNoun = NounManager.IsNoun(value), 
                    IsWord = value.Any(char.IsLetterOrDigit)
                });
            }
        }

        public bool HasNoun()
        {
            return this.Any(i => i.IsNoun);
        }

        public void ReplaceWord(int index, string newWord)
        {
            this[index].Value = newWord;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var part in this)
            {
                stringBuilder.Append(part);
            }

            return stringBuilder.ToString();
        }

        public List<int> NounIndexes()
        {
            var currentIndex = 0;
            var returnIndexes = new List<int>();

            foreach (var word in this)
            {
                if (word.IsNoun) returnIndexes.Add(currentIndex);
                currentIndex += 1;
            }

            return returnIndexes;
        }

        public int WordCount()
        {
            return this.Count(i => i.IsWord);
        }
    }
}
