using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OP_17.ViewModels;

namespace OP_17.Services;

public class ExcelExporter
{
    public static string TemplateFile = "resources/template.xlsx";

    public string Export(MainViewModel mainVM, string fileName = "")
    {
        if (fileName == "")
            fileName = GenerateFileName();
        CreateAndOpenFile();
        FillFields(mainVM);
        _sheet.Cells.AutoFitColumns();
        _package.SaveAs(new FileInfo(fileName));
        _package.Dispose();
        return fileName;
    }

    private string GenerateFileName()
    {
        string GetFileName(int i) => "resources/" + i + ".xlsx";
        int i = 1;
        while (File.Exists(GetFileName(i)))
            i++;
        return GetFileName(i);
    }

    private void FillFields(MainViewModel mainVM)
    {
        _fields["companyName"].Value = mainVM.CompanyName;
        _fields["companyOKPO"].Value = mainVM.CompanyOKPO;
        _fields["companyUnit"].Value = mainVM.CompanyUnit;
        _fields["companyOKDP"].Value = mainVM.CompanyOKDP;
        _fields["operation"].Value = mainVM.DocumentOperation;
        _fields["docNumber"].Value = Convert.ToInt32(mainVM.DocumentNumber);
        _fields["docDate"].Value = mainVM.DocumentDateTime.ToString("dd.MM.yyyy");
        _fields["startDate"].Value = mainVM.StartDate?.ToString("dd.MM.yyyy");
        _fields["endDate"].Value = mainVM.EndDate?.ToString("dd.MM.yyyy");

        foreach (var (field, product) in GetMergedCells(_fields["products"]).Zip(mainVM.Products))
            field.Value = product;
        foreach (var (field, saleDate) in GetMergedCells(_fields["salesDates"]).Zip(mainVM.SalesDates))
            field.Value = saleDate;

        int curRow = 26;
        foreach (var dishVM in mainVM.Dishes)
        {
            FillDish(dishVM, curRow);
            curRow++;
        }

        foreach (var (field, sale) in GetMergedCells(_fields["summarySales"]).Zip(mainVM.SummarySales))
            field.Value = sale;
        _fields["summaryAllSales"].Value = mainVM.SummaryAllSales;
        _fields["summaryAllPrice"].Value = mainVM.SummaryAllPrice;
        foreach (var (field, productCount) in GetMergedCells(_fields["summaryAllProductCounts"]).Where((_, i) => i % 2 == 1).Zip(mainVM.SummaryAllProductCounts))
            field.Value = productCount;

        _fields["formerPost"].Value = mainVM.SignatureVM?.FormerPost;
        _fields["former"].Value = mainVM.SignatureVM?.Former;
        _fields["productionHead"].Value = mainVM.SignatureVM?.ProductionHead;
        _fields["companyHeadPost"].Value = mainVM.SignatureVM?.CompanyHeadPost;
        _fields["companyHead"].Value = mainVM.SignatureVM?.CompanyHead;
    }

    private void FillDish(DishViewModel dishVM, int row)
    {
        var rowCells = GetMergedCells(_sheet.Cells[row, 1, row, 81]).ToList();
        rowCells[0].Value = dishVM.Card;
        rowCells[1].Value = dishVM.Name;
        rowCells[2].Value = dishVM.Code;
        for (int i = 0; i < 5; i++)
            rowCells[3 + i].Value = dishVM.Sales[i];
        rowCells[8].Value = dishVM.AllSales;
        rowCells[9].Value = dishVM.Price;
        rowCells[10].Value = dishVM.AllPrice;
        for (int i = 0; i < 5; i++)
        {
            rowCells[11 + 2 * i].Value = dishVM.ProductsCounts[i];
            rowCells[12 + 2 * i].Value = dishVM.AllProductCounts[i];
        }
    }

    private void CreateAndOpenFile()
    {
        var templateFile = new FileInfo(TemplateFile);

        _package = new ExcelPackage(templateFile);
        _workbook = _package.Workbook;
        _sheet = _workbook.Worksheets.First();

        _fields["companyName"] = GetField("A6");
        _fields["companyOKPO"] = GetField("BY6");
        _fields["companyUnit"] = GetField("A8");
        _fields["companyOKDP"] = GetField("BY9");
        _fields["operation"] = GetField("BY10");
        _fields["docNumber"] = GetField("AX13");
        _fields["docDate"] = GetField("BF13");
        _fields["startDate"] = GetField("BN13");
        _fields["endDate"] = GetField("BS13");
        _fields["products"] = GetField("AS18:CF19");
        _fields["salesDates"] = GetField("R18:AD19");
        _fields["salesDates"].Style.Numberformat.Format = "dd.mm";

        _fields["summarySales"] = GetField("R37:AD37");
        _fields["summaryAllSales"] = GetField("AG37");
        _fields["summaryAllPrice"] = GetField("AO37");
        _fields["summaryAllProductCounts"] = GetField("AS37:CC37");

        _fields["formerPost"] = GetField("J38");
        _fields["former"] = GetField("AB38");
        _fields["productionHead"] = GetField("BP38");
        _fields["companyHeadPost"] = GetField("R40");
        _fields["companyHead"] = GetField("AP40");

    }

    private ExcelRange GetField(string fieldAddr, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
    {
        var field = _sheet.Cells[fieldAddr];
        field.Style.HorizontalAlignment = horizontalAlignment;
        return field;
    }

    private IEnumerable<ExcelRange> GetMergedCells(ExcelRange range)
    {
        return _sheet.MergedCells.Select(mc => _sheet.Cells[mc])
             .Where(mc => range.Select(c => c.FullAddress)
                 .Intersect(mc.Select(c => c.FullAddress)).Count() != 0).OrderBy(c => c.Start, new ExcelCellComparer());
    }


    private ExcelPackage _package = null!;
    private ExcelWorkbook _workbook = null!;
    private ExcelWorksheet _sheet = null!;
    private readonly Dictionary<string, ExcelRange> _fields = new();
}

class ExcelCellComparer : IComparer<ExcelCellAddress>
{
    public int Compare(ExcelCellAddress x, ExcelCellAddress y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var rowComparison = x.Row.CompareTo(y.Row);
        if (rowComparison != 0) return rowComparison;
        return x.Column.CompareTo(y.Column);
    }
}