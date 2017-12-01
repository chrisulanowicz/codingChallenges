using System;
using System.Net;
using System.Threading.Tasks;

using SharpEcho.Recruiting.SpellChecker.Contracts;

namespace SharpEcho.Recruiting.SpellChecker.Core
{
    /// <summary>
    /// This is a dictionary based spell checker that uses dictionary.com to determine if
    /// a word is spelled correctly
    /// 
    /// The URL to do this looks like this: http://dictionary.reference.com/browse/<word>
    /// where <word> is the word to be checked
    /// 
    /// Example: http://dictionary.reference.com/browse/SharpEcho would lookup the word SharpEcho
    /// 
    /// We look for something in the response that gives us a clear indication whether the
    /// word is spelled correctly or not
    /// </summary>
    public class DictionaryDotComSpellChecker : ISpellChecker
    {
        public bool Check(string word)
        {
            // this creates the url we're sending a get request to for the specific word
            string url = $"http://dictionary.reference.com/browse/{word}";

            // try/catch necessary since a misspelled word will send back a 404 error
            try
            {
                // send the Get request and check the response after decompressing which will allow us to access the ResponseUri
                // if no exception raised then the word was found on dictionary.com
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) ;
                
            }
            catch (WebException e)
            {
                // if a word isn't found due to possible misspelling, the response sends back a new uri
                // uri in this case looks like "misspelling?term={word}"
                // Specifically checking for this will prevent false negatives due to actual remote server errors(like 502)
                if (e.Response != null && e.Response.ResponseUri.ToString().Contains("misspelling"))
                {
                    return false;
                }
                else
                {
                    // in case of an error other than misspelling, we'll let it return true, but still alert the user that the word couldn't be checked
                    Console.WriteLine($"Error, unable to check {word}");
                }
            }
            return true;
        }
    }
}
