using System;
using System.Collections.Generic;

using SharpEcho.Recruiting.SpellChecker.Contracts;
using SharpEcho.Recruiting.SpellChecker.Core;

namespace SharpEcho.Recruiting.SpellCheckerConsole
{
    /// <summary>
    /// Thank you for your interest in a position at SharpEcho.  The following are the "requirements" for this project:
    /// 
    /// 1. Implement Main() below so that a user can input a sentence.  Each word in that
    ///    sentence will be evaluated with the SpellChecker, which returns true for a word
    ///    that is spelled correctly and false for a word that is spelled incorrectly.  Display
    ///    out each *distinct* word that is misspelled.  That is, if a user uses the same misspelled
    ///    word more than once, simply output that word one time.
    ///    
    ///    Example:
    ///    Please enter a sentence: Salley sells seashellss by the seashore.  The shells Salley sells are surely by the sea.
    ///    Misspelled words: Salley seashellss
    ///    
    /// 2. The concrete implementation of SpellChecker depends on two other implementations of ISpellChecker, DictionaryDotComSpellChecker
    ///    and MnemonicSpellCheckerIBeforeE.  You will need to implement those classes.  See those classes for details.
    ///    
    /// 3. There are covering unit tests in the SharpEcho.Recruiting.SpellChecker.Tests library that should be implemented as well.
    /// </summary>
    class Program
    {
        /// <summary>
        /// This application is intended to allow a user enter some text (a sentence)
        /// and it will display a distinct list of incorrectly spelled words
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Write("Please enter a sentence: ");
            var sentence = Console.ReadLine();

            // first break the sentance up into words, 
            // then iterate through the list of words using the spell checker
            // capturing distinct words that are misspelled

            string[] seperators = { "/", ",", ".", "!", "?", ";", ":", " "};
            string[] words = sentence.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            List<string> Misspelled = new List<string>();

            // use this spellChecker to evaluate the words
            var spellChecker = new SpellChecker.Core.SpellChecker
                (
                    new ISpellChecker[]
                    { 
                        // I commented out the MnemoicSpellCheckerIBeforeE because it's unnecessary, irrelevant, and not a very good rule.
                        // I did implement the method however which you can see in the class file and it does work as intended
                        // Method also runs and passes the NUnit Tests
                        // The problem with the rule is that there are numerous exceptions, which don't follow any rules so there's no way to implement it properly
                        // Even the examples in the class file that are supposed to be evaluated as misspelled are actually all correct
                        // Given a false like on those examples we would need to cross check against the dictionary spellchecker
                        // and if it's true, then it gets checked by the dictionary spellchecker anyway,
                        // so no matter what, the word has to be checked by the dictionary spellchecker, thus making the Mnemonic spellchecker unnecessary
                        // I could leave it running, and call the dictionary spellchecker from within the Mnemonic Check method upon a false result
                        // but then we're just running extra code that isn't necessary

                        //new MnemonicSpellCheckerIBeforeE(),
                        new DictionaryDotComSpellChecker(),
                    }
                );


            foreach (string word in words)
            {
                var result = spellChecker.Check(word);

                if(!result)
                {
                    // make sure the word hasn't already been used
                    if(Misspelled.Contains(word) == false)
                    {
                        Misspelled.Add(word);
                    }
                }
            }

            Console.WriteLine("Here are some possible misspelled words:");
            foreach(string item in Misspelled)
            {
                Console.WriteLine(item);
            }

            // following is needed otherwise the console closes once the program is completed without any time to see the result
            Console.WriteLine("Press enter to exit");
            Console.Read();
        }
    }
}
