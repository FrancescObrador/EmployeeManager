using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Windows;

namespace ExportPDF
{
    public class PayrollItem
    {
        public string Code { get; set; } = "1234";
        public string Concept { get; set; } = "Concepto";
        public decimal Amount { get; set; } = 1000;
        public bool IsDeduction { get; set; } = false;
    }

    public class Payroll
    {
        public int Id { get; set; } = 1;
        public DateTime PayDate { get; set; } = DateTime.Now;
        public string Company { get; set; } = "Breach";
        public string CompanyAddress { get; set; } = "Calle Falsa 123";
        public string EmployeeName { get; set; } = "Paco";
        public string Category { get; set; } = "Programador";
        public string DNI { get; set; } = "1234";
        public int TotalDays { get; set; } = 20;
        public decimal TotalEarnings { get; set; } = 20000;
        public decimal TotalDeductions { get; set; } = 2000;
        public decimal NetSalary { get; set; } = 20000;
        public List<PayrollItem> PayrollItems { get; set; } = new List<PayrollItem>();

        public Payroll()
        {
            PayrollItems = new List<PayrollItem>();
            PayrollItems.Add(new PayrollItem());
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

                // Configuración de página
                section.PageSetup.TopMargin = "1cm";
                section.PageSetup.LeftMargin = "1cm";
                section.PageSetup.RightMargin = "1cm";

                // Cabecera
                AddHeaderTable(section, payroll);

                // Información del empleado
                AddEmployeeTable(section, payroll);

                // Conceptos de nómina
                AddPayrollItemsTable(section, payroll);

                // Totales
                AddTotalsTable(section, payroll);

                // Pie de página
                AddFooterTable(section, payroll);

                // Renderización del PDF
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer
                {
                    Document = doc
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

        private void AddHeaderTable(Section section, Payroll payroll)
        {
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0.75;

            // Configurar columnas
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("8cm");

            // Primera fila
            Row row = headerTable.AddRow();
            row.Cells[0].AddParagraph("EMPRESA").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("DOMICILIO").Format.Font.Bold = true;

            // Segunda fila
            row = headerTable.AddRow();
            row.Cells[0].AddParagraph(payroll.Company);
            row.Cells[1].AddParagraph(payroll.CompanyAddress);

            SetTableStyle(headerTable);
        }

        private void AddEmployeeTable(Section section, Payroll payroll)
        {
            Table employeeTable = section.AddTable();
            employeeTable.Borders.Width = 0.75;

            employeeTable.AddColumn("8cm");
            employeeTable.AddColumn("4cm");
            employeeTable.AddColumn("4cm");

            Row row = employeeTable.AddRow();
            row.Cells[0].AddParagraph("TRABAJADOR/A").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("CATEGORÍA").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("D.N.I.").Format.Font.Bold = true;

            row = employeeTable.AddRow();
            row.Cells[0].AddParagraph(payroll.EmployeeName);
            row.Cells[1].AddParagraph(payroll.Category);
            row.Cells[2].AddParagraph(payroll.DNI);

            SetTableStyle(employeeTable);
        }

        private void AddPayrollItemsTable(Section section, Payroll payroll)
        {
            Table itemsTable = section.AddTable();
            itemsTable.Borders.Width = 0.75;

            itemsTable.AddColumn("2cm"); // Cuantía
            itemsTable.AddColumn("2cm"); // Precio
            itemsTable.AddColumn("2cm"); // Código
            itemsTable.AddColumn("4cm"); // Concepto
            itemsTable.AddColumn("3cm"); // Devengos
            itemsTable.AddColumn("3cm"); // Deducciones

            // Cabecera
            Row headerRow = itemsTable.AddRow();
            headerRow.Cells[0].AddParagraph("CUANTÍA").Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph("PRECIO").Format.Font.Bold = true;
            headerRow.Cells[2].AddParagraph("COD").Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph("CONCEPTO").Format.Font.Bold = true;
            headerRow.Cells[4].AddParagraph("DEVENGOS").Format.Font.Bold = true;
            headerRow.Cells[5].AddParagraph("DEDUCCIONES").Format.Font.Bold = true;

            // Conceptos
            foreach (var item in payroll.PayrollItems)
            {
                Row row = itemsTable.AddRow();
                row.Cells[2].AddParagraph(item.Code);
                row.Cells[3].AddParagraph(item.Concept);
                if (item.IsDeduction)
                    row.Cells[5].AddParagraph(item.Amount.ToString("N2"));
                else
                    row.Cells[4].AddParagraph(item.Amount.ToString("N2"));
            }

            SetTableStyle(itemsTable);
        }

        private void AddTotalsTable(Section section, Payroll payroll)
        {
            Table totalsTable = section.AddTable();
            totalsTable.Borders.Width = 0.75;

            totalsTable.AddColumn("4cm");
            totalsTable.AddColumn("4cm");
            totalsTable.AddColumn("4cm");
            totalsTable.AddColumn("4cm");

            Row row = totalsTable.AddRow();
            row.Cells[0].AddParagraph("REM. TOTAL").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("BASE S.S.").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("BASE I.R.P.F.").Format.Font.Bold = true;
            row.Cells[3].AddParagraph("T. A DEDUCIR").Format.Font.Bold = true;

            row = totalsTable.AddRow();
            row.Cells[0].AddParagraph(payroll.TotalEarnings.ToString("N2"));
            row.Cells[1].AddParagraph(payroll.TotalEarnings.ToString("N2"));
            row.Cells[2].AddParagraph(payroll.TotalEarnings.ToString("N2"));
            row.Cells[3].AddParagraph(payroll.TotalDeductions.ToString("N2"));

            SetTableStyle(totalsTable);
        }

        private void AddFooterTable(Section section, Payroll payroll)
        {
            Table footerTable = section.AddTable();
            footerTable.Borders.Width = 0.75;

            footerTable.AddColumn("16cm");

            Row row = footerTable.AddRow();
            row.Cells[0].AddParagraph($"FECHA: {payroll.PayDate:dd MMMM yyyy}");

            row = footerTable.AddRow();
            row.Cells[0].AddParagraph($"LÍQUIDO A PERCIBIR: {payroll.NetSalary:N2}").Format.Font.Bold = true;

            SetTableStyle(footerTable);
        }

        private void SetTableStyle(Table table)
        {
            table.Format.Font.Size = 9;
            table.Format.Alignment = ParagraphAlignment.Left;
            table.Rows.LeftIndent = 0;
        }
    }
}