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
    public class PayrollBase
    {
        public int id { get; set; }
        public DateTime pay_date { get; set; }
        public decimal gross_salary { get; set; }
        public decimal deductions { get; set; }
        public decimal net_salary { get; set; }
    }

    public class PayrollPDFService
    {
        public bool SavePDF(PayrollBase payroll, string path)
        {
            try
            {
                Document doc = new Document();
                Section section = doc.AddSection();

                // Título de la nómina
                Paragraph title = section.AddParagraph("Nómina");
                title.Format.Font.Size = 20;
                title.Format.Font.Bold = true;
                title.Format.Alignment = ParagraphAlignment.Center;

                section.AddParagraph();

                // Información del empleado
                Table employeeTable = section.AddTable();
                employeeTable.Borders.Width = 0.75;

                employeeTable.AddColumn("4cm"); // Etiqueta
                employeeTable.AddColumn("10cm"); // Valor

                Row row = employeeTable.AddRow();
                row.Cells[0].AddParagraph("Empleado:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph("nombre");

                row = employeeTable.AddRow();
                row.Cells[0].AddParagraph("Puesto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph("Puesto");

                row = employeeTable.AddRow();
                row.Cells[0].AddParagraph("Mes:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.pay_date.ToString());

                section.AddParagraph();

                // Detalles salariales
                Table salaryTable = section.AddTable();
                salaryTable.Borders.Width = 0.75;

                salaryTable.AddColumn("6cm"); // Concepto
                salaryTable.AddColumn("8cm"); // Importe

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Salario Bruto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.gross_salary.ToString("C"));

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Deducciones:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.deductions.ToString("C"));

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Salario Neto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.net_salary.ToString("C"));

                // Renderización del PDF
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer
                {
                    Document = doc,
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
