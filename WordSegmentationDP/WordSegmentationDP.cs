﻿// WordSgmentationDP: Word Segmentation with Dynamic Programming
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

using System;
using System.Collections.Generic;
using System.IO;

public class WordSegmentationDP
{
    //The number of all words in the text corpus from which the frequency dictionary was derived
    //Google Books Ngram data: http://storage.googleapis.com/books/ngrams/books/datasetsv2.html
    //The probability P of a word = count of the word in the corpus / number of all words in the corpus 
    public long N = 1024908267229L;

    //dictionary
    public Dictionary<string, long> dictionary = new Dictionary<string, long>();
    public int maximumDictionaryWordLength = 0;

    /// <summary>Create a new instanc of WordSegmentationTM.</summary>
    public WordSegmentationDP()
    {

    }

    /// <summary>Load multiple dictionary entries from a file of word/frequency count pairs</summary>
    /// <remarks>Merges with any dictionary data already loaded.</remarks>
    /// <param name="corpus">The path+filename of the file.</param>
    /// <param name="termIndex">The column position of the word.</param>
    /// <param name="countIndex">The column position of the frequency count.</param>
    /// <returns>True if file loaded, or false if file not found.</returns>
    public bool LoadDictionary(string corpus, int termIndex=0, int countIndex=1)
    {
        if (!File.Exists(corpus)) return false;

        using (StreamReader sr = new StreamReader(File.OpenRead(corpus)))
        {
            String line;

            //process a single line at a time only for memory efficiency
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineParts = line.Split(null);
                if (lineParts.Length >= 2)
                {
                    string key = lineParts[termIndex];
                    //Int64 count;
                    if (Int64.TryParse(lineParts[countIndex], out Int64 count))
                    {
                        if (key.Length > maximumDictionaryWordLength) maximumDictionaryWordLength = key.Length;
                        dictionary[key] = count;
                    }
                }
            }
        }

        return true;
    }

    
    /// <summary>Find best word segmentation for input string.</summary>
    /// <param name="input">The string being word segmented.</param>
    /// <returns>A tuple representing the suggested word segmented text and the sum of logarithmic word occurence probabilities.</returns> 
    public (string segmentedString, decimal probabilityLogSum) Segment(string input)
    {
        return Segment(input, maximumDictionaryWordLength);
    }

    /// <summary>Find best word segmentation for input string.</summary>
    /// <param name="input">The string being word segmented.</param>
    /// <param name="maxSegmentationWordLength">The maximum word length that should be considered.</param>	
    /// <returns>A tuple representing the suggested word segmented text and the sum of logarithmic word occurence probabilities.</returns> 
    (string segmentedString, decimal probabilityLogSum) Segment(string input, int maxSegmentationWordLength = 20, Dictionary<string, (string segmentedString, decimal probabilityLogSum)> cache = null)
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
            if (part1.Length < input.Length) rem = Segment(input.Substring(i), maxSegmentationWordLength, cache);
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

}

