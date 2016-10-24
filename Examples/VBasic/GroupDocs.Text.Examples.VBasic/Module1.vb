﻿Module Module1

    Sub Main()

        'Un-comment to apply license 
        'Utilities.ApplyLicense()

        'Region "TextExtractors"

        'Extracting email attachments
        'EmailsExtractor.ExtractEmailAttachments("The butterfly effect.msg");

        'Extracting onenote text
        'ContentExtractor.OneNoteDocument.ExtractOneNoteDocument("The butterfly effect.one");

        'Extracting pdf text
        'ContentExtractor.PdfDocument.ExtractPdfDocument("The butterfly effect.pdf");

        'Extracting slides text
        'ContentExtractor.PresentationDocument.ExtractPresentationDocument("The butterfly effect.pptx");

        'Extracting the entire sheet
        'ContentExtractor.SpreadsheetDocument.ExtractEntireSheet("The butterfly effect.xlsx");
        'Extracting the sheet by the rows
        'ContentExtractor.SpreadsheetDocument.ExtractSheetByRows("The butterfly effect.xlsx");
        'Extracting the selected columns
        'ContentExtractor.SpreadsheetDocument.ExtractSelectedColumns("The butterfly effect.xlsx");
        'Extracting selected rows and columns
        'ContentExtractor.SpreadsheetDocument.ExtractSelectedColumnsAndRows("The butterfly effect.xlsx"); 

        'Extracting text from word page (formatted)
        'ContentExtractor.TextDocument.ExtractEntireWordPage("The butterfly effect.docx");
        'Extracting formatted/tabled text from word
        'ContentExtractor.TextDocument.FormattingTable("The butterfly effect.docx");
        'Extracting text with markdown text format, at the moment bullets are supported
        'ContentExtractor.TextDocument.ExtractingWithMarkdown("The butterfly effect.docx");
        'Extracting text with html text format
        'ContentExtractor.TextDocument.HtmlTextFormating("The butterfly effect.docx");
        'End Region

        'Region "MetadataExtractors"
        'Extracting metadata from cells
        'MetaDataExtractor.CellsMetadata.ExtractMetadataFromCells("The butterfly effect.xlsx");
        'Extracting metadata from slides
        'MetaDataExtractor.SlidesMetadata.ExtractMetadataFromSlides("The butterfly effect.pptx");
        'Extracting metadata from words
        'MetaDataExtractor.WordsMetaData.ExtractMetadataFromWords("The butterfly effect.docx");
        'Extracting metadata from pdf documents
        'MetaDataExtractor.PdfMetaData.ExtractMetadataFromPdf("The butterfly effect.pdf");
        'Extracting metadata from emails
        'MetaDataExtractor.EmailMetaData.ExtractMetadataFromEmails("The butterfly effect.msg");
        'Extract metadata of any supported file formatted document using extractor factory
        'MetaDataExtractor.UsingExtractorFactory("The butterfly effect.pptx");
        'EndRegion

        'Region "ContainerExtractor"
        'Extracting from OST
        'ContainerExtractor.ExtractFromOstContainer();
        'Enumerating all entities 
        'ContainerExtractor.EnumeratingAllEntities();
        'End Region

        'Region "OtherOperations"
        'Creating a concrete extractor
        'OtherOperations.ConcreteExtractor("The butterfly effect.xlsx");
        'Extract all
        'OtherOperations.ExtractAllFromCells("The butterfly effect.xlsx");
        'Pass media type and encoding to the created extractor
        'OtherOperations.PassEncodingToCreatedExtractor("The butterfly effect.xlsx");
        'End Region

    End Sub

End Module