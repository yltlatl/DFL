using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFL;

namespace FixedWidthFileLibrary
{
    public class FixedWidthFile : DflBaseClass
    {
        #region Constructors
        
        public FixedWidthFile(string path, string encoding)
            : base(path, encoding)
        { }
        
        public FixedWidthFile(string path, string encoding, List<int> startPositions)
            : base(path, encoding)
        {
            for (var i = 0; i < startPositions.Count; i++)
            {
                Fields.Add(new Field(i.ToString(CultureInfo.InvariantCulture), startPositions[i]));
            }
            FieldStartPositionsKnown = true;
        }

        #endregion

        #region Public Properties and Methods

        public bool FieldStartPositionsKnown { get; private set; }

        public void GuessFieldStartPositions()
        {
            GetNextLine();
            var line = CurrentLine;
            for (var i = 0; i < line.Length; i++)
            {
                var character = line[i];
                
            }
        }

        public override void GetNextRecord()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties and Methods

        private List<Field> Fields { get; set; }

        #endregion
    }

    public class Field
    {
        #region Constructors

        public Field(string fieldName, int startPosition)
        {
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentException("Invalid field name.", "fieldName");
            if (startPosition < 0)
                throw new ArgumentException("Field start position cannot be less than zero.", "startPosition");
            FieldName = fieldName;
            StartPosition = startPosition;
        }

        #endregion

        #region Fields

        private int _endPosition;

        #endregion

        #region Properties

        public string FieldName { get; private set; }

        public int StartPosition { get; private set; }

        public int EndPosition
        {
            get { return _endPosition; }
            set
            {
                if (value < 0) throw new Exception("End position cannot be negative.");
                if (value < StartPosition) throw new Exception("End position cannot be less than start position.");
                _endPosition = value;
            }
        }

        #endregion
    }
}
