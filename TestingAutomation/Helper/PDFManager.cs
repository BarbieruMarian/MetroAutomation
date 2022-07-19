using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace TestFramework.Helper
{
    public class PDFManager
    {
        public static void ParsePDFToFile(string pdfFilePath, string textFilePath)
        {
            PdfReader reader = new PdfReader(pdfFilePath);
            PdfReaderContentParser parser = new PdfReaderContentParser(reader);
            FileStream fs = new FileStream(textFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            SimpleTextExtractionStrategy strategy;

            strategy = parser.ProcessContent(1, new SimpleTextExtractionStrategy());
            sw.WriteLine(strategy.GetResultantText());
            sw.Flush();
            sw.Close();
        }
    }
}
