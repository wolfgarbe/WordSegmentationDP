//MIT License: Copyright (c) 2018 Wolf Garbe
//https://github.com/wolfgarbe/WordSegmentationDP
/// <summary>Find best word segmentation for input string.</summary>
/// <param name="input">The string being word segmented.</param>
/// <param name="maxSegmentationWordLength">The maximum word length that should be considered.</param>	
/// <returns>A tuple representing the suggested word segmented text and the sum of logarithmic word occurence probabilities.</returns> 
static (string segmentedString, decimal probabilityLogSum) WordSegmentationDP(string input, int maxSegmentationWordLength = 20, Dictionary<string, (string segmentedString, decimal probabilityLogSum)> cache = null)
{
	//memoization: check wheather input has already calculated, if yes then return from cache
	if (cache == null) cache = new Dictionary<string, (string segmentedString, decimal probabilityLogSum)>();
	if (cache.TryGetValue(input, out (string segmentedString, decimal probabilityLogSum) bestComposition)) return bestComposition; else bestComposition = ("", 0);

	//find best composition within loop (this recursion level)
	for (int i = 1; i <= Math.Min(maxSegmentationWordLength, input.Length); i++)
	{
		string part1 = input.Substring(0, i);

		//logarithmic probability of part1
		decimal ProbabilityLogPart1 = 0;
		if (dictionary.TryGetValue(part1, out long wordCount)) ProbabilityLogPart1 = (decimal)Math.Log10((double)wordCount / (double)N);
		else ProbabilityLogPart1 = (decimal)Math.Log10(10.0 / (N * Math.Pow(10.0, part1.Length)));

		(string segmentedString, decimal probabilityLogSum) rem = ("", 0);
		//recursion for remainder: return values: segmentedString, probabilityLogSum
		if (part1.Length < input.Length) rem = WordSegmentationDP(input.Substring(i), maxSegmentationWordLength, cache);
		//else: no remainder, no further recursion

		//iterativ calculation: we only need to calculate part1 and the sum returned from recursion, 
		//but we don't need to calculate the sum from scratch for all single composition elements returned from the recursion
		if ((i == 1) || (ProbabilityLogPart1 + rem.probabilityLogSum > bestComposition.probabilityLogSum))
		{
			if (part1.Length == input.Length) bestComposition = (part1, ProbabilityLogPart1);
			else
				bestComposition = (part1 + " " + rem.segmentedString, ProbabilityLogPart1 + rem.probabilityLogSum);
		}
	}

	//return value from recursion (only the right part)
	cache.Add(input, bestComposition);
	return bestComposition;
}

Console.WriteLine(WordSegmentationDP("thequickbrownfoxjumpsoverthelazydog", maximumDictionaryWordLength).segmentedString);