using System;
using System.IO;

namespace vaYolo
{
    public class VaTxtWriter
    {
        public string Delimiter { get; private set; }

        public string QuoteString
        {
            get
            {
                return quoteString;
            }
            set
            {
                quoteString = value;
                doubleQuoteString = value + value;
            }
        }

        public bool QuoteAllFields { get; set; } = false;

        public bool Trim { get; set; } = false;

        char[] quoteRequiredChars;
        bool checkDelimForQuote = false;
        string quoteString = "\"";
        string doubleQuoteString = "\"\"";
        TextWriter wr;

        public VaTxtWriter(TextWriter wr) : this(wr, ",") { }

        public VaTxtWriter(TextWriter wr, string delimiter)
        {
            this.wr = wr;
            Delimiter = delimiter;
            checkDelimForQuote = delimiter.Length > 1;
            quoteRequiredChars = checkDelimForQuote ? new[] { '\r', '\n' } : new[] { '\r', '\n', delimiter[0] };
        }

        int recordFieldCount = 0;

        public void WriteField(double val) => WriteField(val.ToString());

        public void WriteField(string field)
        {
            var shouldQuote = QuoteAllFields;

            field = field ?? String.Empty;

            if (field.Length > 0 && Trim)
            {
                field = field.Trim();
            }

            if (field.Length > 0)
            {
                if (shouldQuote // Quote all fields
                    || field.Contains(quoteString) // Contains quote
                    || field[0] == ' ' // Starts with a space
                    || field[field.Length - 1] == ' ' // Ends with a space
                    || field.IndexOfAny(quoteRequiredChars) > -1 // Contains chars that require quotes
                    || (checkDelimForQuote && field.Contains(Delimiter)) // Contains delimiter
                ) shouldQuote = true;
            }

            if (shouldQuote && field.Length > 0)
                field = field.Replace(quoteString, doubleQuoteString);

            if (shouldQuote)
                field = quoteString + field + quoteString;

            if (recordFieldCount > 0)
                wr.Write(Delimiter);

            if (field.Length > 0)
                wr.Write(field);

            recordFieldCount++;
        }

        public void NextRecord()
        {
            wr.WriteLine();
            recordFieldCount = 0;
        }
    }

    public class VaTxtReader
    {
		int recordFieldCount = 0;

		string[] CurrentLineToks {get; set;}

        TextReader rr;

		public string Delimiter { get; private set; }

        public VaTxtReader(TextReader rr, string delimiter)
        {
            this.rr = rr;
			Delimiter = delimiter;
        }

		public bool NextRecord()
        {
            var ln = rr.ReadLine();
			if (ln != null) {
				CurrentLineToks = ln.Split(Delimiter);
				recordFieldCount = 0;
				return true;
			}
            			
			return false;
        }

        public T ReadField<T>()
        {
			return (T)Convert.ChangeType(CurrentLineToks[recordFieldCount++], typeof(T));
        }		
    }
}