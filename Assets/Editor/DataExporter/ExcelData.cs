﻿using System.Collections.Generic;

//excel内的全部数据  不做过滤
public enum ExcelRule
{
    Error = 0,
    Common,  //客户端服务器通用
    Client,       //客户端使用
    Server,      //服务器使用
    Finish,      //结束标识(行)
    Ignore,     //忽略（行 列）
    Content  //内容
}

public class ExcelRuleUtil
{

    static Dictionary<int, ExcelRule> _colorRuleMap = new Dictionary<int, ExcelRule>()
    {
        { Rgb2Int(0,128,0), ExcelRule.Common},
        { Rgb2Int(255,204,0), ExcelRule.Server},
        { Rgb2Int(0,204,205), ExcelRule.Client},
        { Rgb2Int(128,128,128), ExcelRule.Ignore},
        { Rgb2Int(0,51,102), ExcelRule.Finish},
        { Rgb2Int(0,0,0), ExcelRule.Content},
    };

    static int Rgb2Int(byte r, byte g, byte b)
    {
        return r << 16 | g << 8 | b;
    }

    public static ExcelRule GetExcelRole(byte r, byte g, byte b)
    {
        int color = Rgb2Int(r, g, b);
        if (_colorRuleMap.ContainsKey(color))
            return _colorRuleMap[color];
        return ExcelRule.Error;
    }

}

public class ExcelCell
{
    public object value;
    public string stringValue;
    public int index;
    public ExcelRule rule;

    byte[] _rgb;
    public byte[] rgb
    {
        set
        {
            _rgb = value;
            rule = ExcelRuleUtil.GetExcelRole(_rgb[0], _rgb[1], _rgb[2]);
        }
        get { return _rgb; }
    }



    public bool IsEmpty
    {
        get
        {
            return value == null || string.IsNullOrEmpty(stringValue);
        }
    }
}

public class ExcelRow
{
    public int row;
    public List<ExcelCell> cellList = new List<ExcelCell>();

    public bool IsEmpty
    {
        get { return cellList == null || cellList.Count == 0; }
    }

    public int count { get { return cellList == null ? 0 : cellList.Count; } }

    public ExcelCell GetCell(int index)
    {
        if (index >= count)
            return null;
        return cellList[index];
    }
}

public class ExcelData 
{
    public string fileName;
    public string filePath;
    public List<ExcelRow> excelRows;

    public int count { get { return excelRows == null? 0 : excelRows.Count; } }
    public ExcelRow GetRow(int index)
    {
        if (index >= excelRows.Count)
            return null;
        return excelRows[index];
    }
}
