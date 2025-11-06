using System.Text;
using Newtonsoft.Json;

namespace Tokenizer;

internal class Tokenizer
{
    public void Tokenize(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var tokens = bytes.Select(b => new byte[] { b }).ToList();

        var vocabulary = new HashSet<byte[]>(tokens);
        var targetVocabularyLength = 1;

        var mergeRules = new List<(string Left, string Right)>();

        while (vocabulary.Count < targetVocabularyLength)
        {
            var pairFrequencies = CountPairFrequencies(tokens);

            var mostFrequentPair = FindMostFrequentPair(pairFrequencies);

            var newTokens = MergeMostFrequentPairInSequence(mostFrequentPair, tokens);

            tokens = newTokens;

            //mergeRules.Add(mostFrequentPair.Key);

            var newMergedToken = mostFrequentPair.Key.Item1.Concat(mostFrequentPair.Key.Item2).ToArray();

            vocabulary.Add(newMergedToken);
        }
    }

    private static List<byte[]> MergeMostFrequentPairInSequence(KeyValuePair<(byte[], byte[]), int> tokenWithMaximumCount, List<byte[]> currentTokens)
    {
        var firstTokenToMatch = tokenWithMaximumCount.Key.Item1;
        var nextTokenToMatch = tokenWithMaximumCount.Key.Item2;

        var newMergedToken = firstTokenToMatch.Concat(nextTokenToMatch).ToArray();
        var newTokens = new List<byte[]>();

        var index = 0;

        var equalityComparer = new ByteArrayComparer();
        equalityComparer.Equals(firstTokenToMatch, nextTokenToMatch);

        while (index < currentTokens.Count - 1)
        {
            var currentToken = currentTokens[index];
            var nextToken = currentTokens[index + 1];

            if (equalityComparer.Equals(currentToken, firstTokenToMatch) && equalityComparer.Equals(nextToken, nextTokenToMatch))
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
