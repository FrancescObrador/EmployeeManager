using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using DataAccess;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using MigraDoc.Rendering;
using System.IO;
using System.Windows;

namespace ExportPDF
{
    public class PayrollPDFService
    {
        public bool SavePDF(List<Provincia> provincias, string path)
        {
			try
			{
                if (PdfSharp.Capabilities.Build.IsCoreBuild)
                {
                    GlobalFontSettings.FontResolver = new FailsafeFontResolver();
                }

                Document doc = new Document();
                Section section = doc.AddSection();
                Paragraph paragraph = section.AddParagraph();
                paragraph.AddText("Provincias");
                paragraph.Format.Font.Size = 20;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                section.AddParagraph();

                Table table = section.AddTable();
                table.Style = "table";
                table.Borders.Width = 0.75;

                // Col id
                Column column = table.AddColumn("1cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                // Col nombre
                column = table.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Left;
                // Col poblacion
                column = table.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Left;
                // Col superficie
                column = table.AddColumn("3cm");
                column.Format.Alignment = ParagraphAlignment.Left;
                // Col escudo
                column = table.AddColumn("2cm");
                column.Format.Alignment = ParagraphAlignment.Left;

                Row row = table.AddRow();
                row.HeadingFormat = true;

                row.Format.Font.Bold = true;
                row.Shading.Color = Colors.LightGray;
                row.Cells[0].AddParagraph("ID").Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("Provincias");
                row.Cells[2].AddParagraph("Población");
                row.Cells[3].AddParagraph("Superficie");
                row.Cells[4].AddParagraph("Escudo");

                bool isStrip = false;
                foreach (Provincia p in provincias)
                {
                    row = table.AddRow();

                    row.Shading.Color = isStrip ? Colors.LightCoral : Colors.LightBlue;
                    isStrip = !isStrip;

                    row.Cells[0].AddParagraph(p.ID.ToString()).Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].AddParagraph(p.Nombre);
                    row.Cells[2].AddParagraph(p.Poblacion.ToString() + " personas");
                    row.Cells[3].AddParagraph(p.Superficie.ToString() + " km2");
                    
                    string escudoPath = Path.Combine(Environment.CurrentDirectory, "escudos", p.Escudo);
                    
                    MigraDoc.DocumentObjectModel.Shapes.Image img = row.Cells[4].AddImage(escudoPath);
                    img.LockAspectRatio = true;
                    img.Width = Unit.FromPoint(35);
                }

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer
                {
                    Document = doc,
                    PdfDocument = new PdfDocument()
                };

                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(path);
                return true;
            }
            catch (Exception e)
			{
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
			}
        }
    }

}
