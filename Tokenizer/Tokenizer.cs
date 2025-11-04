namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var tokens = input.ToCharArray().Select(character => character.ToString()).ToArray();
        var pairFrequencies = CountPairFrequencies(tokens);

        var mostFrequentPair = FindMostFrequentPair(pairFrequencies);

        var newTokens = MergeMostFrequentPairInSequence(mostFrequentPair, tokens);
    }

    private static string[] MergeMostFrequentPairInSequence(KeyValuePair<(string, string), int> tokenWithMaximumCount, string[] currentTokens)
    {
        var firstTokenToMatch = tokenWithMaximumCount.Key.Item1;
        var nextTokenToMatch = tokenWithMaximumCount.Key.Item2;

        var newMergedToken = string.Concat(firstTokenToMatch, nextTokenToMatch);
        var newTokens = new List<string>();

        var index = 0;

        while (index < currentTokens.Length - 1)
        {
            var currentToken = currentTokens[index];
            var nextToken = currentTokens[index + 1];

            if (currentToken == firstTokenToMatch && nextToken == nextTokenToMatch)
            {
                newTokens.Add(newMergedToken);
                index += 2;
            }
            else
            {
                newTokens.Add(currentToken);
                index++;
            }
        }

        if (index == currentTokens.Length - 1)
        {
            newTokens.Add(currentTokens[index]);
        }

        return newTokens.ToArray();
    }


    private static KeyValuePair<(string, string), int> FindMostFrequentPair(Dictionary<(string, string), int> pairFrequencies)
    {
        var maximumFrequencyCount = pairFrequencies.Max(token => token.Value);
        return pairFrequencies.First(pair => pair.Value == maximumFrequencyCount);
    }

    private static Dictionary<(string, string), int> CountPairFrequencies(string[] tokens)
    {
        Dictionary<(string, string), int> pairFrequencies = [];
        
        for (var index = 0; index < tokens.Length - 1; index++)
        {
            var currentToken = tokens[index];
            var nextToken = tokens[index + 1];

            var tokenPair = (currentToken, nextToken);

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
