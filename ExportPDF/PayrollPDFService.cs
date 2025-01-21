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
        public string Concept { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeduction { get; set; }
    }

    public class Payroll
    {
        public int Id { get; set; } = 1;
        public DateTime PayDate { get; set; } = DateTime.Now;
        public string Company { get; set; }
        public string EmployeeName { get; set; }
        public string Category { get; set; }
        public string DNI { get; set; } = "1234";
        public int TotalDays { get; set; } = 30;
        public decimal GrossSalary => PayrollItems.Where(item => !item.IsDeduction).Sum(item => item.Amount);
        public decimal Deductions => PayrollItems.Where(item => item.IsDeduction).Sum(item => item.Amount);
        public decimal NetSalary => GrossSalary - Deductions;
        public List<PayrollItem> PayrollItems { get; set; } = new List<PayrollItem>();
        public string PhoneNumber { get; set; }

        public Payroll(int id, DateTime payDate, string company, string employeeName, string category, string dni, int totalDays, List<PayrollItem> payrollItems, string phoneNumber)
        {
            Id = id;
            PayDate = payDate;
            Company = company;
            EmployeeName = employeeName;
            Category = category;
            DNI = dni;
            TotalDays = totalDays;
            PayrollItems = payrollItems;
            PhoneNumber = phoneNumber;
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
                section.PageSetup = doc.DefaultPageSetup.Clone();
                section.PageSetup.TopMargin = Unit.FromCentimeter(2);
                section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
                section.PageSetup.RightMargin = Unit.FromCentimeter(2);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(2);

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
            headerTable.AddColumn("16cm");

            // Primera fila
            Row row = headerTable.AddRow();
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph("EMPRESA").Format.Font.Bold = true;

            // Segunda fila
            row = headerTable.AddRow();
            row.Cells[0].AddParagraph(payroll.Company);

            SetTableStyle(headerTable);
        }

        private void AddEmployeeTable(Section section, Payroll payroll)
        {
            Table employeeTable = section.AddTable();
            employeeTable.Borders.Width = 0.75;

            employeeTable.AddColumn("6cm");
            employeeTable.AddColumn("3cm");
            employeeTable.AddColumn("3cm");
            employeeTable.AddColumn("4cm");

            Row row = employeeTable.AddRow();
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph("TRABAJADOR/A").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("CATEGORÍA").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("D.N.I.").Format.Font.Bold = true;
            row.Cells[3].AddParagraph("TELÉFONO").Format.Font.Bold = true;

            row = employeeTable.AddRow();
            row.Cells[0].AddParagraph(payroll.EmployeeName);
            row.Cells[1].AddParagraph(payroll.Category);
            row.Cells[2].AddParagraph(payroll.DNI);
            row.Cells[3].AddParagraph(payroll.PhoneNumber);

            SetTableStyle(employeeTable);
        }

        private void AddPayrollItemsTable(Section section, Payroll payroll)
        {
            Table itemsTable = section.AddTable();
            itemsTable.Borders.Width = 0.75;
            itemsTable.Rows.LeftIndent = 0;

            itemsTable.AddColumn("2cm"); // Cuantía
            itemsTable.AddColumn("2cm"); // Precio
            itemsTable.AddColumn("6cm"); // Concepto
            itemsTable.AddColumn("3cm"); // Devengos
            itemsTable.AddColumn("3cm"); // Deducciones

            // Cabecera
            Row headerRow = itemsTable.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Cells[0].AddParagraph("CUANTÍA").Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph("PRECIO").Format.Font.Bold = true;
            headerRow.Cells[2].AddParagraph("CONCEPTO").Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph("DEVENGOS").Format.Font.Bold = true;
            headerRow.Cells[4].AddParagraph("DEDUCCIONES").Format.Font.Bold = true;

            // Conceptos
            foreach (var item in payroll.PayrollItems)
            {
                Row row = itemsTable.AddRow();
                row.Cells[2].AddParagraph(item.Concept);
                if (item.IsDeduction)
                    row.Cells[4].AddParagraph(item.Amount.ToString("N2"));
                else
                    row.Cells[3].AddParagraph(item.Amount.ToString("N2"));
            }

            // Agregar filas en blanco
            for (int i = 0; i < 2; i++)
            {
                Row row = itemsTable.AddRow();
                row.Borders.Left.Width = 0.75;
                row.Borders.Right.Width = 0.75;
                row.Borders.Top.Width = 0;
                row.Borders.Bottom.Width = 0;
            }

            SetTableStyle(itemsTable, false);
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
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph("REM. TOTAL").Format.Font.Bold = true;
            row.Cells[1].AddParagraph("BASE S.S.").Format.Font.Bold = true;
            row.Cells[2].AddParagraph("BASE I.R.P.F.").Format.Font.Bold = true;
            row.Cells[3].AddParagraph("T. A DEDUCIR").Format.Font.Bold = true;

            row = totalsTable.AddRow();
            row.Cells[0].AddParagraph(payroll.GrossSalary.ToString("N2"));
            row.Cells[1].AddParagraph(payroll.GrossSalary.ToString("N2"));
            row.Cells[2].AddParagraph(payroll.GrossSalary.ToString("N2"));
            row.Cells[3].AddParagraph(payroll.Deductions.ToString("N2"));

            SetTableStyle(totalsTable);
        }

        private void AddFooterTable(Section section, Payroll payroll)
        {
            Table footerTable = section.AddTable();
            footerTable.Borders.Width = 0.75;

            footerTable.AddColumn("16cm");

            Row row = footerTable.AddRow();
            row.Shading.Color = Colors.LightGray;
            row.Cells[0].AddParagraph($"FECHA: {payroll.PayDate:dd MMMM yyyy}");

            row = footerTable.AddRow();
            row.Cells[0].AddParagraph($"LÍQUIDO A PERCIBIR: {payroll.NetSalary:N2}").Format.Font.Bold = true;

            SetTableStyle(footerTable);
        }

        private void SetTableStyle(Table table, bool includeInnerBorders = true)
        {
            table.Format.Font.Size = 10;
            table.Format.Font.Name = "Arial";
            table.Format.Alignment = ParagraphAlignment.Left;
            table.Rows.LeftIndent = 0;

            foreach (Row row in table.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Borders.Width = 0.75;
                    if (!includeInnerBorders)
                    {
                        cell.Borders.Top.Width = 0;
                        cell.Borders.Bottom.Width = 0;
                    }
                }
            }
        }
    }
}