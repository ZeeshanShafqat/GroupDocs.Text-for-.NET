﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDocs.Text.Extractors.Text;
using GroupDocs.Text.Formatters.Plain;
using GroupDocs.Text.Formatters.Markdown;
using GroupDocs.Text.Formatters.Html;
using System.IO;
using GroupDocs.Text;
using GroupDocs.Text.Detectors.MediaType;
using GroupDocs.Text.Containers;
using GroupDocs.Text.Extractors;
using GroupDocs.Text.Extractors.Metadata;

namespace GroupDocs.Text_for_.NET
{
    public class DocumentTextExtractor
    {

        public class EmailsExtractor
        {
            public static void ExtractEmailAttachments(string fileName)
            {
                //ExStart:ExtractEmailAttachments
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                EmailTextExtractor extractor = new EmailTextExtractor(filePath);
                ExtractorFactory factory = new ExtractorFactory();
                for (int i = 0; i < extractor.AttachmentCount; i++)
                {
                    Console.WriteLine(extractor.GetContentType(i).Name);
                    Stream stream = extractor.GetStream(i);
                    TextExtractor attachmentExtractor = factory.CreateTextExtractor(filePath);
                    try
                    {
                        Console.WriteLine(attachmentExtractor == null ? "Document format is not supported" : attachmentExtractor.ExtractAll());
                    }
                    finally
                    {
                        if (attachmentExtractor != null)
                        {
                            attachmentExtractor.Dispose();
                        }
                    }
                }
                //ExEnd:ExtractEmailAttachments
            }

            /// <summary>
            /// Shows how to extract structured text from emails
            /// Here as a sample usage where we are showing how to extract hyperlinks from an email
            /// Feature is supported by version 17.04 or greater
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractEmailHyperlinks(string fileName)
            {
                //ExStart:ExtractEmailHyperlinks
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                List<string> hyperlinks = new List<string>();
                StringBuilder sb = null;
                string currentLink = null;
                IStructuredExtractor extractor = new EmailTextExtractor(filePath);
                StructuredHandler handler = new StructuredHandler();

                // Handle Hyperlink event to process a starting of a hyperlink
                handler.Hyperlink += (sender, e) =>
                {
                    sb = new StringBuilder();
                    currentLink = e.Properties.Link;
                };

                // Handle ElementClose event to process a closing of a hyperlink
                handler.ElementClosed += (sender, e) =>
                {
                    StructuredHandler h = sender as StructuredHandler;
                    if (h != null && h[0] is HyperlinkProperties) // closing of hyperlink
                    {
                        if (sb != null)
                        {
                            hyperlinks.Add(string.Format("{0} ({1})", sb.ToString(), currentLink));
                        }
                        sb = null;
                        currentLink = null;
                    }
                };

                // Handle ElementText event to process a text
                handler.ElementText += (sender, e) =>
                {
                    if (sb != null) // if hyperlink is open
                    {
                        sb.Append(e.Text);
                    }
                };

                // Extract a text with its structure
                extractor.ExtractStructured(handler);

                foreach (string hl in hyperlinks)
                {
                    Console.WriteLine(hl);
                }
                //ExEnd:ExtractEmailHyperlinks
            }
        }

        public class OneNoteDocument
        {
            /// <summary>
            /// Extract text from onenote file/document
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractOneNoteDocument(string fileName)
            {
                //ExStart:ExtractOneNoteDocument
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                //Set page index
                int pageIndex = 1;
                NoteTextExtractor extractor = new NoteTextExtractor(filePath);
                Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractPage(pageIndex), extractor.PageCount);
                //Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractAll(), extractor.PageCount);
                //ExEnd:ExtractOneNoteDocument
            }

            /// <summary>
            /// Opens password-protected OneNote sections
            /// </summary>
            /// <param name="fileName">Name of the password protected one note file</param>
            public static void OpenPasswordProtectedOneNoteSection(string fileName)
            {
                //ExStart: OpenPasswordProtectedOneNoteSection
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                //set password in the load options
                var loadOptions = new LoadOptions();
                loadOptions.Password = "test";


                //initialize Note text extractor using the load options to open password protected sections
                using (var extractor = new NoteTextExtractor(filePath, loadOptions))
                {
                    //display the extracted text
                    Console.WriteLine(extractor.ExtractAll());
                }
                //ExEnd:OpenPasswordProtectedOneNoteSection
            }
        }

        public class PdfDocument
        {
            /// <summary>
            /// Extract text from pdf documents
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractPdfDocument(string fileName)
            {
                //ExStart:ExtractPdfDocument
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                //Set page index
                int pageIndex = 1;
                PdfTextExtractor extractor = new PdfTextExtractor(filePath);
                //set extract mode to standard
                extractor.ExtractMode = ExtractMode.Standard;
                Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractPage(pageIndex), extractor.PageCount);
                //Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractAll(), extractor.PageCount);
                //ExEnd:ExtractPdfDocument
            }
        }

        public class PresentationDocument
        {
            /// <summary>
            /// Extract text from presentatoin documents
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractPresentationDocument(string fileName)
            {
                //ExStart:ExtractPresentationDocument
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                //Set slide index
                int slideIndex = 1;
                SlidesTextExtractor extractor = new SlidesTextExtractor(filePath);
                //set extract mode to standard
                extractor.ExtractMode = ExtractMode.Standard;
                Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractSlide(slideIndex), extractor.SlideCount);
                //Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractAll(), extractor.SlideCount);
                //ExEnd:ExtractPresentationDocument
            }

            /// <summary>
            /// Shows how to extract structured text from presentation documents
            /// Here as a sample usage where we are showing how to extract top-level lists from ppt
            /// Feature is supported by version 17.04 or greater
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractTopLevelLists(string fileName)
            {
                //ExStart:ExtractTopLevelLists
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                StringBuilder sb = new StringBuilder();
                IStructuredExtractor extractor = new SlidesTextExtractor(filePath);
                StructuredHandler handler = new StructuredHandler();

                bool isList = false;

                // Handle Hyperlink event to process a starting of a list
                handler.List += (sender, e) =>
                {
                    e.Properties.SkipElement = e.Properties.Depth > 0; // process only top-level lists
                    if (!e.Properties.SkipElement)
                    {
                        isList = true;
                    }
                };

                // Handle ElementClose event to process a closing of a list
                handler.ElementClosed += (sender, e) =>
                {
                    StructuredHandler h = sender as StructuredHandler;
                    if (h != null && h[0] is ListProperties)
                    {
                        isList = false;
                    }
                };

                // Handle ElementText event to process a text
                handler.ElementText += (sender, e) =>
                {
                    if (!isList)
                    {
                        return;
                    }

                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }

                    sb.Append(e.Text);
                };

                // Extract a text with its structure
                extractor.ExtractStructured(handler);

                Console.WriteLine(sb.ToString());
                //ExEnd:ExtractTopLevelLists
            }
        }

        public class SpreadsheetDocument
        {
            /// <summary>
            /// Extract text from spreadsheet documents
            /// </summary>
            public static void ExtractEntireSheet(string fileName)
            {
                //ExStart:ExtractEntireSheet
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                CellsTextExtractor extractor = new CellsTextExtractor(filePath);
                //set extract mode to standard
                extractor.ExtractMode = ExtractMode.Standard;
                //display all the sheets present in the excel file
                for (int sheetIndex = 0; sheetIndex < extractor.SheetCount; sheetIndex++)
                    Console.WriteLine("{0} Page Count : {1} ", extractor.ExtractSheet(sheetIndex), extractor.SheetCount);
                //ExEnd:ExtractEntireSheet
            }
            /// <summary>
            /// Extracting the sheet by the rows
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractSheetByRows(string fileName)
            {
                //ExStart:ExtractSheetByRows
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                CellsTextExtractor extractor = new CellsTextExtractor(filePath);
                int sheetIndex = 0;
                CellsSheetInfo sheetInfo = extractor.GetSheetInfo(sheetIndex);
                Console.WriteLine(sheetInfo.ExtractSheetHeader());
                for (int rowIndex = 2; rowIndex < sheetInfo.RowCount; rowIndex++)
                {
                    Console.WriteLine(sheetInfo.ExtractRow(rowIndex));
                }
                //ExEnd:ExtractSheetByRows
            }
            /// <summary>
            /// Extracting the selected columns
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractSelectedColumns(string fileName)
            {
                //ExStart:ExtractSelectedColumns
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                CellsTextExtractor extractor = new CellsTextExtractor(filePath);
                int sheetIndex = 0;
                CellsSheetInfo sheetInfo = extractor.GetSheetInfo(sheetIndex);
                Console.WriteLine(sheetInfo.ExtractSheet("B1", "C1"));
                //ExEnd:ExtractSelectedColumns
            }
            /// <summary>
            /// Extracting the selected columns from selected rows
            /// </summary>
            public static void ExtractSelectedColumnsAndRows(string fileName)
            {
                //ExStart:ExtractSelectedColumnsAndRows
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                CellsTextExtractor extractor = new CellsTextExtractor(filePath);
                int sheetIndex = 0;
                CellsSheetInfo sheetInfo = extractor.GetSheetInfo(sheetIndex);
                Console.WriteLine(sheetInfo.ExtractSheetHeader());
                for (int rowIndex = 2; rowIndex < sheetInfo.RowCount; rowIndex++)
                {
                    Console.WriteLine(sheetInfo.ExtractRow(rowIndex, "B1", "C1"));
                }
                //ExEnd:ExtractSelectedColumnsAndRows
            }

            /// <summary>
            /// Create the concrete extractor by hand using filestream
            /// </summary>
            /// <param name="fileName"></param>
            public static void ConcreteExtractor(string fileName)
            {
                //ExStart:ConcreteExtractor
                //get file actual path
                string filePath = Common.getFilePath(fileName);
                using (Stream stream = File.OpenRead(filePath))
                {
                    using (CellsTextExtractor extractor = new CellsTextExtractor(stream))
                    {
                        //set extract mode to standard
                        extractor.ExtractMode = ExtractMode.Standard;
                        Console.WriteLine(extractor.ExtractAll());
                    }
                }
                //ExEnd:ConcreteExtractor
            }

            /// <summary>
            /// Create the concrete extractor by hand
            /// </summary>
            /// <param name="fileName"></param>
            public static void ConcreteExtractorByFile(string fileName)
            {
                //ExStart:ConcreteExtractorByFile
                //get file actual path
                string filePath = Common.getFilePath(fileName);

                using (CellsTextExtractor extractor = new CellsTextExtractor(filePath))
                {
                    Console.WriteLine(extractor.ExtractAll());
                }

                //ExEnd:ConcreteExtractorByFile
            }

            /// <summary>
            /// Shows how to read a structured text from spreadsheets
            /// Feature is supported by version 17.04 or greater
            /// </summary>
            public static void ExtractStructuredText(string fileName)
            {
                //ExStart:ExtractStructuredText
                //get file's complete path 
                string filePath = Common.getFilePath(fileName);
                StringBuilder sb = new StringBuilder();
                IStructuredExtractor extractor = new CellsTextExtractor(filePath);
                StructuredHandler handler = new StructuredHandler();

                // Handle Table event to process a table
                handler.Table += (sender, e) =>
                {
                    e.Properties.SkipElement = e.Properties.Name != "Sheet2"; // process only the sheet which name is Sheet2
                    if (!e.Properties.SkipElement)
                    {
                        if (sb.Length > 0)
                        {
                            sb.AppendLine();
                        }

                        sb.Append(e.Properties.Name);
                    }
                };

                // Handle TableRow event to process a table row
                handler.TableRow += (sender, e) =>
                {
                    sb.AppendLine();
                };

                // Handle TableCell event to process a table cell
                handler.TableCell += (sender, e) =>
                {
                    if (e.Properties.Column > 0)
                    {
                        sb.Append(" ");
                    }
                };

                // Handle ElementText event to process a text
                handler.ElementText += (sender, e) =>
                {
                    sb.Append(e.Text);
                };

                // Extract a text with its structure
                extractor.ExtractStructured(handler);
                Console.WriteLine(sb.ToString());
                //ExEnd:ExtractStructuredText
            }
        }

        public class TextDocument
        {
            /// <summary>
            /// Extract formatted text from word
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractEntireWordPage(string fileName)
            {
                //ExStart:ExtractEntireWordPage
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                int pageIndex = 0;
                WordsFormattedTextExtractor extractor = new WordsFormattedTextExtractor(filePath);
                Console.WriteLine(extractor.ExtractPage(pageIndex));
                //ExEnd:ExtractEntireWordPage
            }
            /// <summary>
            /// Extract text from word by defining a table format
            /// </summary>
            /// <param name="fileName"></param>
            public static void FormattingTable(string fileName)
            {
                //ExStart:FormattingTable
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                WordsFormattedTextExtractor extractor = new WordsFormattedTextExtractor(filePath);
                PlainTableFrame frame = new PlainTableFrame(
                    PlainTableFrameAngle.ASCII,
                    PlainTableFrameEdge.ASCII,
                    PlainTableFrameIntersection.ASCII,
                    new PlainTableFrameConfig(true, true, true, false));
                extractor.DocumentFormatter = new PlainDocumentFormatter(frame);
                Console.WriteLine(extractor.ExtractAll());
                //ExEnd:FormattingTable
            }
            /// <summary>
            /// Extract text with markdown text format
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractingWithMarkdown(string fileName)
            {
                //ExStart:ExtractingWithMarkdown
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                WordsFormattedTextExtractor extractor = new WordsFormattedTextExtractor(filePath);
                extractor.DocumentFormatter = new MarkdownDocumentFormatter();
                Console.WriteLine(extractor.ExtractAll());
                //ExEnd:ExtractingWithMarkdown
            }

            /// <summary>
            /// Extract a text with HTML text formatter
            /// </summary>
            /// <param name="fileName"></param>
            public static void HtmlTextFormating(string fileName)
            {
                //ExStart:HtmlTextFormating
                //get file actual path
                String filePath = Common.getFilePath(fileName);
                WordsFormattedTextExtractor extractor = new WordsFormattedTextExtractor(filePath);
                extractor.DocumentFormatter = new HtmlDocumentFormatter();
                Console.WriteLine(extractor.ExtractAll());
                //ExEnd:HtmlTextFormating
            }

            /// <summary>
            /// Shows how to read structured text from text documents
            /// here we show how to extract header from a document
            /// Feature is supported by version 17.04 or greater
            /// </summary>
            public static void ExtractHeadersFromDocument(string fileName)
            {
                //ExStart:ExtractHeadersFromDocument
                //get file's complete path 
                string filePath = Common.getFilePath(fileName);
                StringBuilder sb = new StringBuilder();
                IStructuredExtractor extractor = new WordsTextExtractor(filePath);
                StructuredHandler handler = new StructuredHandler();

                // Handle List event to prevent processing of lists
                handler.List += (sender, e) => e.Properties.SkipElement = true; // ignore lists
                                                                                // Handle Table event to prevent processing of tables
                handler.Table += (sender, e) => e.Properties.SkipElement = true; // ignore tables
                                                                                 // Handle Paragraph event to process a paragraph
                handler.Paragraph += (sender, e) =>
                {
                    int h1 = (int)ParagraphStyle.Heading1;
                    int h6 = (int)ParagraphStyle.Heading6;

                    int style = (int)e.Properties.Style;
                    if (h1 <= style && style <= h6)
                    {
                        if (sb.Length > 0)
                        {
                            sb.AppendLine();
                        }

                        sb.Append(' ', style - h1); // make an indention for the header (h1 - no indention)
                    }
                    else
                    {
                        e.Properties.SkipElement = e.Properties.Style != ParagraphStyle.Title; // skip paragraph if it's not a header or a title
                    }
                };

                // Handle ElementText event to process a text
                handler.ElementText += (sender, e) => sb.Append(e.Text);

                // Extract a text with its structure
                extractor.ExtractStructured(handler);

                Console.WriteLine(sb.ToString());
                //ExEnd:ExtractHeadersFromDocument
            }

            /// <summary>
            /// Extracts hyperlinks from a document
            /// feature supported in version 17.04 or greater
            /// </summary>
            /// <param name="fileName">Name of the source file</param>
            public static void ExtractHyperlinksFromDocument(string fileName)
            {
                //ExStart:ExtractHyperlinksFromDocument
                //get file path
                string filePath = Common.getFilePath(fileName);
                List<string> hyperlinks = new List<string>();
                StringBuilder sb = null;
                string currentLink = null;
                IStructuredExtractor extractor = new WordsTextExtractor(filePath);
                StructuredHandler handler = new StructuredHandler();

                // Handle Hyperlink event to process a starting of a hyperlink
                handler.Hyperlink += (sender, e) =>
                {
                    sb = new StringBuilder();
                    currentLink = e.Properties.Link;
                };

                // Handle ElementClose event to process a closing of a hyperlink
                handler.ElementClosed += (sender, e) =>
                {
                    StructuredHandler h = sender as StructuredHandler;
                    if (h != null && h[0] is HyperlinkProperties) // closing of hyperlink
                    {
                        if (sb != null)
                        {
                            hyperlinks.Add(string.Format("{0} ({1})", sb.ToString(), currentLink));
                        }
                        sb = null;
                        currentLink = null;
                    }
                };

                // Handle ElementText event to process a text
                handler.ElementText += (sender, e) =>
                {
                    if (sb != null) // if hyperlink is open
                    {
                        sb.Append(e.Text);
                    }
                };

                // Extract a text with its structure
                extractor.ExtractStructured(handler);

                foreach (string hl in hyperlinks)
                {
                    Console.WriteLine(hl);
                }
                //ExEnd:ExtractHyperlinksFromDocument
            }
        }

        public class Epub
        {
            /// <summary>
            /// Extracts a line of characters from a document
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractALine(string fileName)
            {
                //ExStart:ExtractALine
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                using (var extractor = new EpubTextExtractor(filePath))
                {
                    string line = extractor.ExtractLine();
                    while (line != null)
                    {
                        Console.WriteLine(line);
                        line = extractor.ExtractLine();
                    }
                }
                //ExEnd:ExtractALine
            }

            /// <summary>
            /// Extracts all characters from a document
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractAllCharacters(string fileName)
            {
                //ExStart:ExtractAllCharacters
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                using (var extractor = new EpubTextExtractor(filePath))
                {
                    Console.WriteLine(extractor.ExtractAll());
                }
                //ExEnd:ExtractAllCharacters
            }


            /// <summary>
            /// Searches for a text in an epub file using regular expression
            /// </summary>
            /// <param name="fileName"></param>
            public static void SearchTextUsingRegex(string fileName)
            {
                //ExStart:SearchTextInEpubUsingRegex
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                using (EpubTextExtractor extractor = new EpubTextExtractor(filePath))
                {
                    var searchOptions = new RegexSearchOptions();
                    var handler = new ListSearchHandler();
                    extractor.SearchWithRegex("On[a-z]", handler, searchOptions);

                    if (handler.List.Count == 0)
                    {
                        Console.WriteLine("Not found");
                    }
                    else
                    {
                        for (int i = 0; i < handler.List.Count; i++)
                        {
                            Console.Write(handler.List[i].LeftText);
                            Console.Write("_");
                            Console.Write(handler.List[i].FoundText);
                            Console.Write("_");
                            Console.Write(handler.List[i].RightText);
                            Console.WriteLine("---");
                        }
                    }
                }
                //ExEnd:SearchTextInEpubUsingRegex
            }

            /// <summary>
            /// Searches some text in an epub file
            /// </summary>
            /// <param name="fileName"></param>
            public static void SearchText(string fileName)
            {
                //ExStart:SearchTextInEpub
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                using (EpubTextExtractor extractor = new EpubTextExtractor(filePath))
                {
                    var options = new SearchOptions(SearchHighlightOptions.CreateFixedLengthOptions(0));
                    var handler = new ListSearchHandler();
                    var keywords = new string[] { "Name" };
                    extractor.Search(options, handler, keywords);

                    if (handler.List.Count == 0)
                    {
                        Console.WriteLine("Not found");
                    }
                    else
                    {
                        for (int i = 0; i < handler.List.Count; i++)
                        {
                            Console.Write(handler.List[i].LeftText);
                            Console.Write("_");
                            Console.Write(handler.List[i].FoundText);
                            Console.Write("_");
                            Console.Write(handler.List[i].RightText);
                            Console.WriteLine("---");
                        }
                    }
                }
                //ExEnd:SearchTextInEpub
            }

            /// <summary>
            /// Extracts highlighted text in epub file
            /// </summary>
            /// <param name="fileName"></param>
            public static void ExtractHighlight(string fileName)
            {
                //ExStart:ExtractHighlightInEpub
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                using (EpubTextExtractor extractor = new EpubTextExtractor(filePath))
                {
                    IList<string> highlights = extractor.ExtractHighlights(HighlightOptions.CreateFixedLengthOptions(HighlightDirection.Left, 9, 3));
                    for (int i = 0; i < highlights.Count; i++)
                    {
                        Console.WriteLine(highlights[i]);
                    }
                }
                //ExEnd:ExtractHighlightInEpub
            }

            /// <summary>
            /// Detects Epub Media type
            /// </summary>
            /// <param name="fileName"></param>
            public static void DetectEpubMediaType(string fileName)
            {
                //ExStart:DetectEpubMediaType
                //get file's actual path
                String filePath = Common.getFilePath(fileName);
                var detector = new EpubMediaTypeDetector();
                var mediaType = detector.Detect(filePath);

                // APPLICATION/EPUB+ZIP or null if stream is not EPUB document.
                Console.WriteLine(mediaType);
                //ExEnd:DetectEpubMediaType
            }


        }

        public static void PassEncodingToCreatedExtractor(string fileName)
        {
            //ExStart:PassEncodingToCreatedExtractor
            //get file actual path
            string filePath = Common.getFilePath(fileName);
            LoadOptions loadOptions = new LoadOptions("text/plain", Encoding.UTF8);
            ExtractorFactory factory = new ExtractorFactory();
            using (TextExtractor extractor = factory.CreateTextExtractor(filePath, loadOptions))
            {
                Console.WriteLine(extractor != null ? extractor.ExtractAll() : "The document format is not supported");
            }
            //ExEnd:PassEncodingToCreatedExtractor
        }

        /// <summary>
        /// Extract text from a password protected document 
        /// </summary>
        /// <param name="fileName"></param>
        public static void PasswordProtectedDocumentExtractor(string fileName)
        {
            //ExStart:PasswordProtectedDocumentExtractor
            //get file actual path
            string filePath = Common.getFilePath(fileName);
            //To open password-protected document Password property of LoadOptions must be set
            LoadOptions loadOptions = new LoadOptions();
            loadOptions.Password = "12345";

            WordsTextExtractor extractor = null;
            //If password is not set or incorrect InvalidPasswordException is thrown
            try
            {
                Console.WriteLine("Able to open the password protected document");
                extractor = new WordsTextExtractor(filePath, loadOptions);
                Console.WriteLine(extractor.ExtractAll());
            }
            catch (InvalidPasswordException)
            {
                Console.WriteLine("Invalid password.");
            }
            finally
            {
                if (extractor != null)
                {
                    extractor.Dispose();
                }
            }
            //ExEnd:PasswordProtectedDocumentExtractor
        }

        /// <summary>
        /// Shows how a conatiner is created using ExtractFactory
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreatingContainerUsingExtractorFactory(string fileName)
        {
            //ExStart:CreatingContainerUsingExtractorFactory
            //get file actual path
            string filePath = Common.getFilePath(fileName);
            ExtractorFactory factory = new ExtractorFactory(null, new CellsMediaTypeDetector());
            using (Container container = factory.CreateContainer(filePath))
            {
                if (container == null)
                {
                    Console.WriteLine("The document format is not supported");
                }
            }
            //ExEnd:CreatingContainerUsingExtractorFactory
        }

        /// <summary>
        /// Shows how a conatiner is created using ExtractFactory
        /// </summary>
        /// <param name="fileName"></param>
        public static void ExtractorFactoryCreateFormattedExtractor(string fileName)
        {
            //ExStart:ExtractorFactoryCreateFormattedExtractor
            //get file actual path
            string filePath = Common.getFilePath(fileName);
            ExtractorFactory factory = new ExtractorFactory(new MarkdownDocumentFormatter());
            using (TextExtractor extractor = factory.CreateFormattedTextExtractor(fileName))
            {
                Console.WriteLine(extractor != null ? extractor.ExtractAll() : "The document format is not supported");
            }
            //ExEnd:ExtractorFactoryCreateFormattedExtractor
        }

        /// <summary>
        /// Extracts highight from a document
        /// </summary>
        public static void ExtractHighlight(string fileName)
        {
            //ExStart:ExtractHighlight
            //get file path
            string filePath = Common.getFilePath(fileName);
            //initialize words text extractor
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                //extract hightlights from the document
                IList<string> highlights = extractor.ExtractHighlights(
                //set highlight options to get fixed length text from the highlighted portion
                HighlightOptions.CreateFixedLength(HighlightDirection.Left, 15, 10),
                HighlightOptions.CreateFixedLength(HighlightDirection.Right, 20, 10));

                //write the result on console
                for (int i = 0; i < highlights.Count; i++)
                {
                    Console.WriteLine(highlights[i]);
                }
            }
            //ExEnd:ExtractHighlight
        }

        /// <summary>
        /// Shows highlight extraction with defined words from the position
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="wordsCount">count of words from the position from where to extract highlight</param>
        public static void ExtractHighlightWithLimitedWordsCount(string fileName, int wordsCount)
        {
            //ExStart:ExtractHighlightWithLimitedWordsCount
            //get file path
            string filePath = Common.getFilePath(fileName);
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                IList<string> highlights = extractor.ExtractHighlights(
                  HighlightOptions.CreateWordsCountOptions(HighlightDirection.Left, 15, wordsCount),
                  HighlightOptions.CreateWordsCountOptions(HighlightDirection.Right, 20, wordsCount));

                for (int i = 0; i < highlights.Count; i++)
                {
                    Console.WriteLine(highlights[i]);
                }
            }
            //ExEnd:ExtractHighlightWithLimitedWordsCount
        }

        /// <summary>
        /// Extracts highlight to the start or end of line
        /// </summary>
        /// <param name="fileName"></param>
        public static void ExtractHighlightTillStartOrEndOfLine(string fileName)
        {
            //ExStart:ExtractHighlightTillStartOrEndOfLine
            //get file path
            string filePath = Common.getFilePath(fileName);
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                IList<string> highlights = extractor.ExtractHighlights(
                  HighlightOptions.CreateLineOptions(HighlightDirection.Left, 15),
                  HighlightOptions.CreateLineOptions(HighlightDirection.Right, 20));

                for (int i = 0; i < highlights.Count; i++)
                {
                    Console.WriteLine(highlights[i]);
                }
            }
            //ExEnd:ExtractHighlightTillStartOrEndOfLine
        }
        /// <summary>
        /// Searches text in documents.
        /// </summary>
        /// <param name="fileName">the name of the file to searrch text from</param>
        public static void SearchTextInDocuments(string fileName)
        {
            //ExStart:SearchTextInDocuments
            //get file actual path
            string filePath = Common.getFilePath(fileName);
            //initialize words text extractor
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                //initialize search handler
                ListSearchHandler handler = new ListSearchHandler();
                //search for the text
                extractor.Search(new SearchOptions(new SearchHighlightOptions(10)), handler, null, new string[] { "test text", "keyword" });

                //Results count is none
                if (handler.List.Count == 0)
                {
                    Console.WriteLine("Not found");
                }
                else
                {
                    //loop through the list and display the results
                    for (int i = 0; i < handler.List.Count; i++)
                    {
                        Console.Write(handler.List[i].LeftText);
                        Console.Write("_");
                        Console.Write(handler.List[i].FoundText);
                        Console.Write("_");
                        Console.Write(handler.List[i].RightText);
                        Console.WriteLine("---");
                    }
                }
            }
            //ExEnd:SearchTextInDocuments
        }

        /// <summary>
        /// Searches whole word in documents.
        /// </summary>
        /// <param name="fileName"></param>
        public static void SearchWholeWord(string fileName)
        {
            //ExStart:SearchWholeWord
            //get file path
            string filePath = Common.getFilePath(fileName);
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                SearchOptions searchOptions = new SearchOptions(SearchHighlightOptions.CreateFixedLengthOptions(15), true, true);
                ListSearchHandler handler = new ListSearchHandler();
                extractor.Search(searchOptions, handler, null, new string[] { "mark", "down" });

                if (handler.List.Count == 0)
                {
                    Console.WriteLine("Not found");
                }
                else
                {
                    for (int i = 0; i < handler.List.Count; i++)
                    {
                        Console.Write(handler.List[i].LeftText);
                        Console.Write("_");
                        Console.Write(handler.List[i].FoundText);
                        Console.Write("_");
                        Console.Write(handler.List[i].RightText);
                        Console.WriteLine("---");
                    }
                }
            }
            //ExEnd:SearchWholeWord
        }

        /// <summary>
        /// Search text in documents using regular expression
        /// </summary>
        /// <param name="fileName"></param>
        public static void SearchTextWithRegex(string fileName)
        {
            //ExStart:SearchTextWithRegex
            //get file path
            string filePath = Common.getFilePath(fileName);
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                ListSearchHandler handler = new ListSearchHandler();
                extractor.SearchWithRegex("19[0-9]{2}", handler, new RegexSearchOptions(SearchHighlightOptions.CreateFixedLengthOptions(10)));

                if (handler.List.Count == 0)
                {
                    Console.WriteLine("Not found");
                }
                else
                {
                    for (int i = 0; i < handler.List.Count; i++)
                    {
                        Console.Write(handler.List[i].LeftText);
                        Console.Write("_");
                        Console.Write(handler.List[i].FoundText);
                        Console.Write("_");
                        Console.Write(handler.List[i].RightText);
                        Console.WriteLine("---");
                    }
                }
            }
            //ExEnd:SearchTextWithRegex
        }

        /// <summary>
        /// Shows searching a text with highlights limited by line's start/end
        /// </summary>
        /// <param name="fileName"></param>
        public static void UseExtractionModesWithSearch(string fileName)
        {
            //ExStart:UseExtractionModesWithSearch
            //get file path
            string filePath = Common.getFilePath(fileName);
            using (WordsTextExtractor extractor = new WordsTextExtractor(filePath))
            {
                ListSearchHandler handler = new ListSearchHandler();
                SearchHighlightOptions highlightOptions = SearchHighlightOptions.CreateLineOptions(100, 100);
                extractor.Search(new SearchOptions(highlightOptions), handler, null, new string[] { "text", "extraction" });

                if (handler.List.Count == 0)
                {
                    Console.WriteLine("Not found");
                }
                else
                {
                    for (int i = 0; i < handler.List.Count; i++)
                    {
                        Console.Write(handler.List[i].LeftText);
                        Console.Write("_");
                        Console.Write(handler.List[i].FoundText);
                        Console.Write("_");
                        Console.Write(handler.List[i].RightText);
                        Console.WriteLine("---");
                    }
                }
            }
            //ExEnd:UseExtractionModesWithSearch
        }

        /// <summary>
        /// Detects any supported media type using CompositeMediaTypeDetector class
        /// </summary>
        /// <param name="fileName"></param>
        public static void MediaTypeDetection(string fileName)
        {
            //ExStart:MediaTypeDetection
            //get file actual path
            String filePath = Common.getFilePath(fileName);
            var mediaType = CompositeMediaTypeDetector.Default.Detect(filePath);
            Console.WriteLine(mediaType);
            //ExEnd:MediaTypeDetection
        }



    }
}
