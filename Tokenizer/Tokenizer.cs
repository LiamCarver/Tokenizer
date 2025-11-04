namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var originalVocabulary = input.ToCharArray().Select(character => character.ToString()).ToArray();
        var characterPairFrequencies = GetAdjacentCharacterPairFrequencies(originalVocabulary);

        var tokenWithMaximumCount = GetTokenWithMaximumCount(characterPairFrequencies);

        var newVocabulary = GetUpdatedVocabulary(tokenWithMaximumCount, originalVocabulary);
    }

    private string[] GetUpdatedVocabulary(KeyValuePair<Tuple<string, string>, int> tokenWithMaximumCount, string[] currentVocabulary)
    {
        var firstTokenToMatch = tokenWithMaximumCount.Key.Item1;
        var nextTokenToMatch = tokenWithMaximumCount.Key.Item2;

        var newMergedToken = string.Concat(firstTokenToMatch, nextTokenToMatch);
        var newVocabulary = new List<string>();

        var index = 0;

        while (index < currentVocabulary.Length - 2)
        {
            var currentToken = currentVocabulary[index];
            var nextToken = currentVocabulary[index + 1];

            if (currentToken == firstTokenToMatch && nextToken == nextTokenToMatch)
            {
                newVocabulary.Add(newMergedToken);
                index += 2;
            }
            else
            {
                newVocabulary.Add(currentToken);
                index++;
            }
        }

        return newVocabulary.ToArray();
    }

    private KeyValuePair<Tuple<string, string>, int> GetTokenWithMaximumCount(Dictionary<Tuple<string, string>, int> characterPairFrequencies)
    {
        var maximumFrequencyCount = characterPairFrequencies.Max(token => token.Value);
        return characterPairFrequencies.First(pair => pair.Value == maximumFrequencyCount);
    }

    private Dictionary<Tuple<string, string>, int> GetAdjacentCharacterPairFrequencies(string[] tokens)
    {
        Dictionary<Tuple<string, string>, int> adjacentCharacterFrequencies = new();
        
        for (var index = 0; index < tokens.Length - 1; index++)
        {
            var currentToken = tokens[index];
            var nextToken = tokens[index + 1];

            var tokenPair = new Tuple<string, string>(currentToken, nextToken);

            if (adjacentCharacterFrequencies.TryGetValue(tokenPair, out var frequency))
            {
                adjacentCharacterFrequencies[tokenPair] = frequency + 1;
            }
            else
            {
                adjacentCharacterFrequencies[tokenPair] = 1;
            }
        }

        return adjacentCharacterFrequencies;
    }
}
