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
        public decimal GrossSalary => PayrollItems.Where(item => !item.IsDeduction).Sum(item => item.Amount);
        public decimal Deductions => PayrollItems.Where(item => item.IsDeduction).Sum(item => item.Amount);
        public decimal NetSalary => GrossSalary - Deductions;
        public List<PayrollItem> PayrollItems { get; set; } = new List<PayrollItem>();
        public string PhoneNumber { get; set; }

        public Payroll(int id, DateTime payDate, string company, string employeeName, string category, string dni, List<PayrollItem> payrollItems, string phoneNumber)
        {
            Id = id;
            PayDate = payDate;
            Company = company;
            EmployeeName = employeeName;
            Category = category;
            DNI = dni;
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
                section.PageSetup.TopMargin = Unit.FromCentimeter(1.5);
                section.PageSetup.LeftMargin = Unit.FromCentimeter(1.5);
                section.PageSetup.RightMargin = Unit.FromCentimeter(1.5);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(1.5);

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
            headerTable.AddColumn("16cm");

            // Nombre de la empresa
            Row row = headerTable.AddRow();
            var companyCell = row.Cells[0];
            companyCell.AddParagraph(payroll.Company.ToUpper());
            companyCell.Format.Font.Bold = true;
            companyCell.Format.Font.Size = 12;
            companyCell.Format.Alignment = ParagraphAlignment.Center;

            // Tipo de documento
            row = headerTable.AddRow();
            var docTypeCell = row.Cells[0];
            docTypeCell.AddParagraph("RECIBO DE NÓMINA");
            docTypeCell.Format.Font.Size = 11;
            docTypeCell.Format.Alignment = ParagraphAlignment.Center;
            docTypeCell.Format.SpaceAfter = Unit.FromCentimeter(0.5);

            SetTableStyle(headerTable);
        }

        private void AddEmployeeTable(Section section, Payroll payroll)
        {
            Table employeeTable = section.AddTable();
            employeeTable.AddColumn("8cm");
            employeeTable.AddColumn("8cm");

            // Fila 1 - Datos principales
            Row row = employeeTable.AddRow();
            row.Cells[0].AddParagraph($"TRABAJADOR/A: {payroll.EmployeeName}");
            row.Cells[1].AddParagraph($"CATEGORÍA: {payroll.Category}");

            // Fila 2 - Datos secundarios
            row = employeeTable.AddRow();
            row.Cells[0].AddParagraph($"DNI/NIE: {payroll.DNI}");
            row.Cells[1].AddParagraph($"TELÉFONO: {payroll.PhoneNumber}");

            // Estilos
            foreach (Row tableRow in employeeTable.Rows)
            {
                foreach (Cell cell in tableRow.Cells)
                {
                    cell.Format.Font.Size = 10;
                    cell.VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                }
            }

            employeeTable.Borders.Width = 0.25;
            employeeTable.Format.SpaceAfter = Unit.FromCentimeter(0.5);
        }

        private void AddPayrollItemsTable(Section section, Payroll payroll)
        {
            Table itemsTable = section.AddTable();
            itemsTable.AddColumn("10cm"); // Concepto
            itemsTable.AddColumn("3cm");   // Devengos
            itemsTable.AddColumn("3cm");  // Deducciones

            // Cabecera
            Row headerRow = itemsTable.AddRow();
            headerRow.Shading.Color = Colors.LightGray;

            // Configuración de celdas
            var conceptCell = headerRow.Cells[0];
            var devengosCell = headerRow.Cells[1];
            var deduccionesCell = headerRow.Cells[2];

            // Concepto
            var conceptParagraph = conceptCell.AddParagraph("CONCEPTO");
            conceptParagraph.Format.Font.Bold = true;

            // Devengos
            var devengosParagraph = devengosCell.AddParagraph("DEVENGOS");
            devengosParagraph.Format.Font.Bold = true;
            devengosParagraph.Format.Alignment = ParagraphAlignment.Right;

            // Deducciones
            var deduccionesParagraph = deduccionesCell.AddParagraph("DEDUCCIONES");
            deduccionesParagraph.Format.Font.Bold = true;
            deduccionesParagraph.Format.Alignment = ParagraphAlignment.Right;

            // Conceptos
            foreach (var item in payroll.PayrollItems)
            {
                Row row = itemsTable.AddRow();
                row.Cells[0].AddParagraph(item.Concept);

                if (!item.IsDeduction)
                {
                    var amountParagraph = row.Cells[1].AddParagraph(item.Amount.ToString("N2"));
                    amountParagraph.Format.Alignment = ParagraphAlignment.Right;
                }
                else
                {
                    var amountParagraph = row.Cells[2].AddParagraph(item.Amount.ToString("N2"));
                    amountParagraph.Format.Alignment = ParagraphAlignment.Right;
                }
            }

            // Totales parciales
            AddTotalRow(itemsTable, "TOTAL DEVENGOS:", payroll.GrossSalary, 1);
            AddTotalRow(itemsTable, "TOTAL DEDUCCIONES:", payroll.Deductions, 2);

            itemsTable.Borders.Width = 0.25;
            itemsTable.Format.SpaceAfter = Unit.FromCentimeter(0.5);
        }

        private void AddTotalRow(Table table, string label, decimal amount, int columnIndex)
        {
            Row row = table.AddRow();
            row.Cells[0].AddParagraph(label).Format.Font.Bold = true;
            var amountParagraph = row.Cells[columnIndex].AddParagraph(amount.ToString("N2"));
            amountParagraph.Format.Font.Bold = true;
            amountParagraph.Format.Alignment = ParagraphAlignment.Right;
        }

        private void AddTotalsTable(Section section, Payroll payroll)
        {
            Table totalsTable = section.AddTable();
            totalsTable.AddColumn("13cm");
            totalsTable.AddColumn("3cm");

            Row row = totalsTable.AddRow();
            row.Cells[0].AddParagraph("LÍQUIDO A PERCIBIR").Format.Font.Bold = true;
            var netSalaryParagraph = row.Cells[1].AddParagraph(payroll.NetSalary.ToString("N2"));
            netSalaryParagraph.Format.Font.Bold = true;
            netSalaryParagraph.Format.Alignment = ParagraphAlignment.Right;

            totalsTable.Borders.Top.Width = 0.75;
            totalsTable.Borders.Bottom.Width = 0.75;
            totalsTable.Format.Font.Size = 11;
        }

        private void AddFooterTable(Section section, Payroll payroll)
        {
            Table footerTable = section.AddTable();
            footerTable.AddColumn("16cm");

            Row row = footerTable.AddRow();
            var footerCell = row.Cells[0];
            footerCell.AddParagraph($"Fecha de generación: {payroll.PayDate:dd/MM/yyyy}");
            footerCell.Format.Font.Size = 9;
            footerCell.Format.Alignment = ParagraphAlignment.Center;

            footerTable.Borders.Width = 0.25;
            footerTable.Format.SpaceBefore = Unit.FromCentimeter(1);
        }

        private void SetTableStyle(Table table)
        {
            table.Format.Font.Name = "Arial";
            table.Format.Alignment = ParagraphAlignment.Left;
            table.Rows.LeftIndent = 0;
        }
    }

}