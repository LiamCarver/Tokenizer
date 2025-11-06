using System.Text;
using Newtonsoft.Json;

namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var tokens = bytes.Select(b => new byte[] { b }).ToList();

        var vocabulary = new HashSet<byte[]>();
        var targetVocabularyLength = 1;

        var mergeRules = new List<(string Left, string Right)>();

        while (vocabulary.Count < targetVocabularyLength)
        {
            var pairFrequencies = CountPairFrequencies(tokens);
            vocabulary.Add([]);

            //var mostFrequentPair = FindMostFrequentPair(pairFrequencies);

            //var newTokens = MergeMostFrequentPairInSequence(mostFrequentPair, tokens);

            //tokens = newTokens;

            //mergeRules.Add(mostFrequentPair.Key);

            //var newMergedToken = string.Concat(mostFrequentPair.Key.Item1, mostFrequentPair.Key.Item2);
            //vocabulary.Add(newMergedToken);
        }
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

    private static Dictionary<(byte[], byte[]), int> CountPairFrequencies(List<byte[]> tokens)
    {
        Dictionary<(byte[], byte[]), int> pairFrequencies = new(new ByteArrayPairComparer());
        
        for (var index = 0; index < tokens.Count - 1; index++)
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
