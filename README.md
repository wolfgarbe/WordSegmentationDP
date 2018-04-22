WordSegmentationDP<br>
[![MIT License](https://img.shields.io/github/license/wolfgarbe/WordSegmentationDP.svg)](https://github.com/wolfgarbe/WordSegmentationDP/blob/master/LICENSE)
========

Word Segmentation using a Dynamic Programming approach.

For a faster Word Segmentation using a Triangular Matrix approach have a look at [WordSegmentationTM](https://github.com/wolfgarbe/WordSegmentationTM).

For a Word Segmentation with Spelling Correction use WordSegmentation and LookupCompound of the [SymSpell library](https://github.com/wolfgarbe/SymSpell).

### Example
`Input: thequickbrownfoxjumpsoverthelazydog`
<br><br>
`Output: the quick brown fox jumps over the lazy dog`

#### Applications

* Word Segmentation for CJK languages for Indexing Spelling correction, Machine translation, Language understanding, Sentiment analysis
* Normalizing English compound nouns for search & indexing (e.g. ice box = ice-box = icebox; pig sty = pig-sty = pigsty) 
* Word segmentation für compounds if both original word and split word parts should be indexed.
* Correction of missing spaces caused by Typing errors.
* Correction of Conversion errors: spaces between word may get lost e.g. when removing line breaks.
* Correction of OCR errors: inferior quality of original documents or handwritten text may prevent that all spaces are recognized.
* Correction of Transmission errors: during the transmission over noisy channels spaces can get lost or spelling errors introduced.
* Keyword extraction from URL addresses, domain names, table column description or programming variables written without spaces.
* For password analysis, the extraction of terms from passwords can be required.
* For Speech recognition, if spaces between words are not properly recognized in spoken language.
* Automatic CamelCasing of programming variables.

#### Performance 
<br>

#### Blog Posts: Algorithm, Benchmarks, Applications
[Fast Word Segmentation for noisy text](https://towardsdatascience.com/fast-word-segmentation-for-noisy-text-2c2c41f9e8da)<br>
<br>

#### Usage of WordSegmentationDP Library
<br>

#### How to use WordSegmentationDP in your project:

WordSegmentationDP targets [.NET Standard v2.0](https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/) and can be used  in:
1. NET Framework (**Windows** Forms, WPF, ASP.NET), 
2. NET Core (UWP, ASP.NET Core, **Windows**, **OS X**, **Linux**),
3. XAMARIN (**iOS**, **OS X**, **Android**) projects.

*The SymSpell, Demo,  DemoCompound and Benchmark projects can be built with the free [Visual Studio Code](https://code.visualstudio.com/), which runs on Windows, MacOS and Linux.*

---

#### Frequency dictionary
Dictionary quality is paramount for correction quality. In order to achieve this two data sources were combined by intersection: Google Books Ngram data which provides representative word frequencies (but contains many entries with spelling errors) and SCOWL — Spell Checker Oriented Word Lists which ensures genuine English vocabulary (but contained no word frequencies required for ranking of suggestions within the same edit distance).

The [frequency_dictionary_en_82_765.txt](https://github.com/wolfgarbe/SymSpell/blob/master/SymSpell/frequency_dictionary_en_82_765.txt) was created by intersecting the two lists mentioned below. By reciprocally filtering only those words which appear in both lists are used. Additional filters were applied and the resulting list truncated to &#8776; 80,000 most frequent words.
* [Google Books Ngram data](http://storage.googleapis.com/books/ngrams/books/datasetsv2.html)   [(License)](https://creativecommons.org/licenses/by/3.0/) : Provides representative word frequencies
* [SCOWL - Spell Checker Oriented Word Lists](http://wordlist.aspell.net/)   [(License)](http://wordlist.aspell.net/scowl-readme/) : Ensures genuine English vocabulary    

#### Dictionary file format
* Plain text file in UTF-8 encoding.
* Word and Word Frequency are separated by space or tab. Per default, the word is expected in the first column and the frequency in the second column. But with the termIndex and countIndex parameters in LoadDictionary() the position and order of the values can be changed and selected from a row with more than two values. This allows to augment the dictionary with additional information or to adapt to existing dictionaries without reformatting.
* Every word-frequency-pair in a separate line. A line is defined as a sequence of characters followed by a line feed ("\n"), a carriage return ("\r"), or a carriage return immediately followed by a line feed ("\r\n").
* Both dictionary terms and input term are expected to be in **lower case**.

You can build your own frequency dictionary for your language or your specialized technical domain.
The SymSpell spelling correction algorithm supports languages with non-latin characters, e.g Cyrillic, Chinese or [Georgian](https://github.com/irakli97/Frequency_Dictionary_GE_363_202).

---

#### Changes


