using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DFL
{
    public abstract class DflBaseClass
    {
        #region Constructors

        protected DflBaseClass(string path, string encoding)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentException("Null or empty path.", path);
            if (!File.Exists(path)) throw new ArgumentException(String.Format("File {0} does not exist.", path));

            var encodingObj = ValidateEncodingString(encoding);

            _str = new StreamReader(path, encodingObj);
            
        }

        #endregion

        #region Properties

        public string CurrentLine { get; protected set; }

        public IEnumerable<string> CurrentRecord { get; protected set; }

        public IEnumerable<string> HeaderRecord { get; protected set; }

        public int CurrentLineNumber { get; protected set; }

        protected StreamReader _str { get; set; }

        public bool EndOfFile { get; protected set; }

        protected int ExpectedFieldCount { get; set; }

        #endregion

        #region Abstract Methods

        public abstract void GetNextRecord();

        #endregion

        #region Methods

        //Validate encoding string argument and return an Encoding object
        protected static Encoding ValidateEncodingString(string encoding)
        {
            //make the string case insensitive
            var lEncoding = encoding.ToLower(CultureInfo.InvariantCulture);

            EncodingInfo encodingInfo = null;
            try
            {
                encodingInfo = Encoding.GetEncodings().Single(e => e.Name.ToLowerInvariant().Contains(lEncoding));
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(String.Format("{0} is not a valid encoding", encoding), e);
            }

            return Encoding.GetEncoding(encodingInfo.Name);
        }

        protected void GetNextLine()
        {
            var line = _str.ReadLine();
            if (line == null && _str.EndOfStream)
            {
                EndOfFile = true;
            }
            if (string.IsNullOrEmpty(line)) throw new ApplicationException("Empty line.");
            CurrentLine = line;
            CurrentLineNumber++;
        }

        public static IEnumerable<string> ParseMultiValueField(string fieldValue, char multiValueDelimiter, bool omitEmptyValues, bool trimWhitespace)
        {
            if (String.IsNullOrEmpty(fieldValue)) return new List<string>();
            var mvdArray = new[] { multiValueDelimiter };
            var sso = omitEmptyValues ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var splitResult = fieldValue.Split(mvdArray, sso);
            return trimWhitespace ? splitResult.Select(v => v.Trim()).ToList() : splitResult.ToList();
        }

        #endregion
    }
}
