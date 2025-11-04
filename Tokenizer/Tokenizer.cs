using Newtonsoft.Json;

namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var characterArray = input.ToCharArray().Select(character => character.ToString()).ToArray();
        var characterPairFrequencies = GetAdjacentCharacterPairFrequencies(characterArray);
        var sanityCheck = JsonConvert.SerializeObject(characterPairFrequencies, Formatting.Indented);
    }

    private Dictionary<Tuple<string, string>, int> GetAdjacentCharacterPairFrequencies(string[] characters)
    {
        Dictionary<Tuple<string, string>, int> adjacentCharacterFrequencies = new();
        
        for (var index = 0; index < characters.Length - 1; index++)
        {
            var character = characters[index];
            var nextCharacter = characters[index + 1];

            var characterPair = new Tuple<string, string>(character, nextCharacter);

            if (adjacentCharacterFrequencies.TryGetValue(characterPair, out var frequency))
            {
                adjacentCharacterFrequencies[characterPair] = frequency + 1;
            }
            else
            {
                adjacentCharacterFrequencies[characterPair] = 1;
            }
        }

        return adjacentCharacterFrequencies;
    }
}
