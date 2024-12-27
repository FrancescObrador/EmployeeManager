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
    public class Payroll
    {
        public int Id { get; set; }
        public DateTime PayDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Name { get; set; }
        public string Rol { get; set; }

        public Payroll(int _id, DateTime _payDate, decimal _grossSalary, decimal _deductions, decimal netSalary, string name, string rol)
        {
            Id = _id;
            PayDate = _payDate;
            GrossSalary = _grossSalary;
            Deductions = _deductions;
            NetSalary = netSalary;
            Name = name;
            Rol = rol;
        }
    }

    public class PayrollPDFService
    {
        public bool SavePDF(Payroll payroll, string path)
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
                row.Cells[1].AddParagraph(payroll.Name);

                row = employeeTable.AddRow();
                row.Cells[0].AddParagraph("Puesto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.Rol);

                row = employeeTable.AddRow();
                row.Cells[0].AddParagraph("Mes:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.PayDate.ToString());

                section.AddParagraph();

                // Detalles salariales
                Table salaryTable = section.AddTable();
                salaryTable.Borders.Width = 0.75;

                salaryTable.AddColumn("6cm"); // Concepto
                salaryTable.AddColumn("8cm"); // Importe

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Salario Bruto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.GrossSalary.ToString("C"));

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Deducciones:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.Deductions.ToString("C"));

                row = salaryTable.AddRow();
                row.Cells[0].AddParagraph("Salario Neto:").Format.Font.Bold = true;
                row.Cells[1].AddParagraph(payroll.NetSalary.ToString("C"));

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
