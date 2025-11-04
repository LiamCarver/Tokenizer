namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var tokens = input.ToCharArray().Select(character => character.ToString()).ToArray();
        var pairFrequencies = CountPairFrequencies(tokens);

        var tokenWithMaximumCount = FindMostFrequentPair(pairFrequencies);

        var newTokens = MergeMostFrequentPairInSequence(tokenWithMaximumCount, tokens);
    }

    private static string[] MergeMostFrequentPairInSequence(KeyValuePair<Tuple<string, string>, int> tokenWithMaximumCount, string[] currentVocabulary)
    {
        var firstTokenToMatch = tokenWithMaximumCount.Key.Item1;
        var nextTokenToMatch = tokenWithMaximumCount.Key.Item2;

        var newMergedToken = string.Concat(firstTokenToMatch, nextTokenToMatch);
        var newVocabulary = new List<string>();

        var index = 0;

        while (index < currentVocabulary.Length - 1)
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

        // ✅ Add the final token if it was not part of a merge
        if (index == currentVocabulary.Length - 1)
        {
            newVocabulary.Add(currentVocabulary[index]);
        }

        return newVocabulary.ToArray();
    }


    private static KeyValuePair<Tuple<string, string>, int> FindMostFrequentPair(Dictionary<Tuple<string, string>, int> pairFrequencies)
    {
        var maximumFrequencyCount = pairFrequencies.Max(token => token.Value);
        return pairFrequencies.First(pair => pair.Value == maximumFrequencyCount);
    }

    private static Dictionary<Tuple<string, string>, int> CountPairFrequencies(string[] tokens)
    {
        Dictionary<Tuple<string, string>, int> pairFrequencies = [];
        
        for (var index = 0; index < tokens.Length - 1; index++)
        {
            var currentToken = tokens[index];
            var nextToken = tokens[index + 1];

            var tokenPair = new Tuple<string, string>(currentToken, nextToken);

            if (pairFrequencies.TryGetValue(tokenPair, out var frequency))
            {
                pairFrequencies[tokenPair] = frequency + 1;
            }
            else
            {
                pairFrequencies[tokenPair] = 1;
            }
        }

        return pairFrequencies;
    }
}
