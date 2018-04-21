// Copyright (C) 2018 Wolf Garbe
// Version: 1.0
// Author: Wolf Garbe wolf.garbe@faroo.com
// Maintainer: Wolf Garbe wolf.garbe@faroo.com
// URL: //https://github.com/wolfgarbe/WordSegmentationDP
// Description: https://towardsdatascience.com/fast-word-segmentation-for-noisy-text-2c2c41f9e8da

// MIT License
// Copyright (c) 2018 Wolf Garbe
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.


//The number of all words in the text corpus from which the frequency dictionary was derived
//Google Books Ngram data: http://storage.googleapis.com/books/ngrams/books/datasetsv2.html
//The probability P of a word = count of the word in the corpus / number of all words in the corpus 
public static long N = 1024908267229L;

//dictionary
public static Dictionary<string, long> dictionary = new Dictionary<string, long>();
public static int maximumDictionaryWordLength = 0;

//Read word, word frequency pairs from dictionary file.
public static bool LoadDictionary(String path)
{
    String line;
    String[] word;

    if (!File.Exists(path)) return false;

    using (StreamReader br = new StreamReader(File.OpenRead(path)))
    {
        while ((line = br.ReadLine()) != null)
        {
            word = line.Split(null);
            if (word[0].Length > maximumDictionaryWordLength) maximumDictionaryWordLength = word[0].Length;
            dictionary[word[0]] = long.Parse(word[1]);
        }
    }
    return true;
}

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

public void test()
{
    if (!LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "../../../frequency_dictionary_en_82_765.txt"))
        Console.WriteLine("file not found");
    else
        Console.WriteLine(WordSegmentationDP("thequickbrownfoxjumpsoverthelazydog", maximumDictionaryWordLength).segmentedString);
}
