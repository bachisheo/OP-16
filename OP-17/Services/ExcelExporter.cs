using System;
using System.Collections.Generic;
using System.Drawing;
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
        fields["companyName"].Value = mainVM.CompanyName;
        fields["companyOKPO"].Value = mainVM.CompanyOKPO;
        fields["companyUnit"].Value = mainVM.CompanyUnit;
        fields["companyOKDP"].Value = mainVM.CompanyOKDP;
        fields["operation"].Value = mainVM.DocumentOperation;
        fields["docNumber"].Value = Convert.ToInt32(mainVM.DocumentNumber);
        fields["docDate"].Value = mainVM.DocumentDateTime.ToString("dd.MM.yyyy");
        fields["startDate"].Value = mainVM.StartDate?.ToString("dd.MM.yyyy");
        fields["endDate"].Value = mainVM.EndDate?.ToString("dd.MM.yyyy");

        foreach (var (field, product) in GetMergedCells(fields["products"]).Zip(mainVM.Products))
            field.Value = product;
        foreach (var (field, product) in GetMergedCells(fields["salesDates"]).Zip(mainVM.SalesDates))
            field.Value = product;

        int curRow = 26;
        foreach (var dishVM in mainVM.Dishes)
        {
            FillDish(dishVM, curRow);
            curRow++;
        }

        fields["formerPost"].Value = mainVM.SignatureVM.FormerPost;
        fields["former"].Value = mainVM.SignatureVM.Former;
        fields["productionHead"].Value = mainVM.SignatureVM.ProductionHead;
        fields["companyHeadPost"].Value = mainVM.SignatureVM.CompanyHeadPost;
        fields["companyHead"].Value = mainVM.SignatureVM.CompanyHead;
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
            rowCells[11 + 2*i].Value = dishVM.ProductsCounts[i];
            rowCells[12 + 2*i].Value = dishVM.ProductsAllCounts[i];
        }
    }

    private void CreateAndOpenFile()
    {
        var templateFile = new FileInfo(TemplateFile);

        _package = new ExcelPackage(templateFile);
        _workbook = _package.Workbook;
        _sheet = _workbook.Worksheets.First();

        fields["companyName"] = GetField("A6");
        fields["companyOKPO"] = GetField("BY6");
        fields["companyUnit"] = GetField("A8");
        fields["companyOKDP"] = GetField("BY9");
        fields["operation"] = GetField("BY10");
        fields["docNumber"] = GetField("AX13");
        fields["docDate"] = GetField("BF13");
        fields["startDate"] = GetField("BN13");
        fields["endDate"] = GetField("BS13");
        fields["products"] = GetField("AS18:CF19");
        fields["salesDates"] = GetField("R18:AD19");
        fields["salesDates"].Style.Numberformat.Format = "dd.mm";
        fields["formerPost"] = GetField("J38");
        fields["former"] = GetField("AB38");
        fields["productionHead"] = GetField("BP38");
        fields["companyHeadPost"] = GetField("R40");
        fields["companyHead"] = GetField("AP40");

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
    private Dictionary<string, ExcelRange> fields = new();
}

class ExcelCellComparer:IComparer<ExcelCellAddress>
{
    public int Compare(ExcelCellAddress? x, ExcelCellAddress? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var rowComparison = x.Row.CompareTo(y.Row);
        if (rowComparison != 0) return rowComparison;
        return x.Column.CompareTo(y.Column);
    }
}