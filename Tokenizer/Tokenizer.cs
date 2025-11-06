using System.Text;
using Newtonsoft.Json;

namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var tokens = bytes.Select(b => new byte[] { b }).ToList();

        var mergeRules = new List<(byte[] Left, byte[] Right)>();
        var targetMergeCount = 50;

        while (mergeRules.Count < targetMergeCount)
        {
            var pairFrequencies = CountPairFrequencies(tokens);
            var mostFrequentPair = FindMostFrequentPair(pairFrequencies);

            tokens = MergeMostFrequentPairInSequence(mostFrequentPair, tokens);

            mergeRules.Add(mostFrequentPair.Key);
        }
    }

    private static List<byte[]> MergeMostFrequentPairInSequence(KeyValuePair<(byte[], byte[]), int> tokenWithMaximumCount, List<byte[]> currentTokens)
    {
        var firstTokenToMatch = tokenWithMaximumCount.Key.Item1;
        var nextTokenToMatch = tokenWithMaximumCount.Key.Item2;

        var newMergedToken = firstTokenToMatch.Concat(nextTokenToMatch).ToArray();
        var newTokens = new List<byte[]>();

        var index = 0;

        while (index < currentTokens.Count - 1)
        {
            var currentToken = currentTokens[index];
            var nextToken = currentTokens[index + 1];

            if (currentToken.SequenceEqual(firstTokenToMatch) && nextToken.SequenceEqual(nextTokenToMatch))
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

        if (index == currentTokens.Count - 1)
        {
            newTokens.Add(currentTokens[index]);
        }

        return newTokens;
    }


    private static KeyValuePair<(byte[], byte[]), int> FindMostFrequentPair(Dictionary<(byte[], byte[]), int> pairFrequencies)
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
