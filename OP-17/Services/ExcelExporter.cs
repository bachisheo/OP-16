﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using obshepit_form_16.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace obshepit_form_16.Services;

public class ExcelExporter
{
    public static string TemplateFile = "resources/template.xlsx";

    public string Export(MainViewModel mainVM, string fileName = "")
    {
        if (fileName == "")
            fileName = GenerateFileName();
        CreateAndOpenFile();
        FillFields(mainVM);
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
        _fields["Fio"].Value = mainVM.FIO;
        _fields["Post"].Value = mainVM.Post;
        _fields["operation"].Value = mainVM.DocumentOperation;
        _fields["docNumber"].Value = Convert.ToInt32(mainVM.DocumentNumber);
        _fields["docDate"].Value = mainVM.DocumentDateTime?.ToString("dd.MM.yyyy");
        _fields["startDate"].Value = mainVM.StartDate?.ToString("dd.MM.yyyy");
        _fields["endDate"].Value = mainVM.EndDate?.ToString("dd.MM.yyyy");

        for (int i = 0; i < 3; i++)
        {
            _sheet.Cells[20, _fields["firstDateDay"].Start.Column + i * 10].Value = mainVM.SalesDates[i]?.Day;
            _sheet.Cells[20, _fields["firstDateMonth"].Start.Column + i * 10].Value = mainVM.SalesDates[i]?.Month;
            _sheet.Cells[20, _fields["firstDateYear"].Start.Column + i * 10].Value = mainVM.SalesDates[i]?.Year;
        }
        _sheet.Cells["BJ20"].Value = mainVM.SalesDates[3]?.Day;
        _sheet.Cells["BL20"].Value = mainVM.SalesDates[3]?.Month;
        _sheet.Cells["BQ20"].Value = mainVM.SalesDates[3]?.Year;
        _sheet.Cells["BU20"].Value = mainVM.SalesDates[4]?.Day;
        _sheet.Cells["BW20"].Value = mainVM.SalesDates[4]?.Month;
        _sheet.Cells["CA20"].Value = mainVM.SalesDates[4]?.Year;

        int curRow = 25;
        foreach (var prodVM in mainVM.Products)
        {
            FillProd(prodVM, curRow);
            curRow++;
        }

        
        foreach (var (field, productCount) in GetMergedCells(_fields["itogo"]).Where((_, i) => i % 2 == 0).Zip(mainVM.SummaryCountsSums))
            field.Value = productCount;
        foreach (var (field, productCount) in GetMergedCells(_fields["itogo2"]).Where((_, i) => i % 2 == 0).Zip(mainVM.SummaryCountsSums))
            field.Value = productCount;

        //_fields["formerPost"].Value = mainVM.SignatureVM?.FormerPost;
        //_fields["former"].Value = mainVM.SignatureVM?.Former;
        //_fields["productionHead"].Value = mainVM.SignatureVM?.ProductionHead;
        //_fields["companyHeadPost"].Value = mainVM.SignatureVM?.CompanyHeadPost;
        //_fields["companyHead"].Value = mainVM.SignatureVM?.CompanyHead;
    }

    private void FillProd(ProductViewModel productVM, int row)
    {
        var rowCells = GetMergedCells(_sheet.Cells[$"A{row}:BX{row}"]).ToList();
        rowCells[0].Value = productVM.RowNumber;
        rowCells[1].Value = productVM.Name;
        rowCells[2].Value = productVM.Code;
        rowCells[3].Value = productVM.NameEi;
        rowCells[4].Value = productVM.CodeEi;
        rowCells[5].Value = productVM.Price;
     
        for (int i = 0; i < 5; i++)
        {
            rowCells[6 + 2 * i].Value = productVM.Counts[i];
            rowCells[7 + 2 * i].Value = productVM.CountsSums[i];
        }
    }

    private void CreateAndOpenFile()
    {
        var templateFile = new FileInfo(TemplateFile);

        _package = new ExcelPackage(templateFile);
        _workbook = _package.Workbook;
        _sheet = _workbook.Worksheets.First();


        _fields["Fio"] = GetField("AH16");
        _fields["Post"] = GetField("S16");
        _fields["companyName"] = GetField("A6");
        _fields["companyOKPO"] = GetField("BU6");
        _fields["companyUnit"] = GetField("A8");
        _fields["companyOKDP"] = GetField("BU9");
        _fields["operation"] = GetField("BU10");
        _fields["docNumber"] = GetField("AO14");
        _fields["docDate"] = GetField("AX14");
        _fields["startDate"] = GetField("BG14");
        _fields["endDate"] = GetField("BM14");
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
        _fields["firstDateDay"] = GetField("AF20");
        _fields["firstDateMonth"] = GetField("AH20");
        _fields["firstDateYear"] = GetField("AL20");
        _fields["itogo"] = GetField("AI35:BX35");
        _fields["itogo2"] = GetField("AI67:BX67");


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