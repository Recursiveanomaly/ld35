#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using Excel;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Data;

// note: 
// computer running this requires 64 bit excel installed from https://www.microsoft.com/en-us/download/details.aspx?id=13255 with cmd line 
// unity project requires System.Data.dll and System.EnterpriseServices.dll copied from C:\Program Files\Unity\Editor\Data\Mono\lib\mono\2.0 to the assets folder
public class ExcelReader
{
    public static Dictionary<int, T> LoadStaticData<T>(string fileToRead, string sheetName) where T : StaticDataDef, new()
    {

        FileStream stream = File.Open(fileToRead, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        //Choose one of either 3, 4, or 5
        //3. DataSet - The result of each spreadsheet will be created in the result.Tables
        DataSet result = null;
        try
        {
            result = excelReader.AsDataSet();
        }
        catch (Exception e)
        {
            Debug.LogError("Exception caught in ExcelReader reading excel file");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }

        //4. DataSet - Create column names from first row
        //excelReader.IsFirstRowAsColumnNames = true;
        //DataSet result = excelReader.AsDataSet();

        //5. Data Reader methods
        //while (excelReader.Read())
        //{
        //    //excelReader.GetInt32(0);
        //}

        //6. Free resources (IExcelDataReader is IDisposable)
        excelReader.Close();

        try
        {
            Dictionary<int, T> m_loadedData = new Dictionary<int, T>();
            var table = result.Tables[sheetName];
            Type staticDefType = typeof(T);
            PropertyInfo[] propertyInfos = staticDefType.GetProperties();

            if (table != null)
            {
                // get header names
                Dictionary<int, PropertyInfo> m_columnPropertyMap = null;
                foreach (DataRow row in table.Rows)
                {
                    if(m_columnPropertyMap == null)
                    {
                        //first column has names that link to properties
                        m_columnPropertyMap = new Dictionary<int, PropertyInfo>();

                        for(int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                        {
                            string stringData = row[columnIndex] as string;
                            if (stringData != null)
                            {
                                foreach (PropertyInfo propertyInfo in propertyInfos)
                                {
                                    if (propertyInfo.Name == stringData || propertyInfo.Name == "m_" + stringData)
                                    {
                                        //Debug.Log("Connecting " + propertyInfo.Name + " property to column " + columnIndex + ": " + stringData);
                                        m_columnPropertyMap.Add(columnIndex, propertyInfo);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // create a new def for this row
                        T newDef = new T();
                        foreach (var columnIndexPropertyPair in m_columnPropertyMap)
                        {
                            Type expectedType = columnIndexPropertyPair.Value.PropertyType;

                            object data = row[columnIndexPropertyPair.Key];
                            PropertyInfo propertyInfo = columnIndexPropertyPair.Value;
                            if (data != null && propertyInfo != null)
                            {
                                // parse the data as the expected type
                                if (IsNumericType(expectedType) || expectedType.IsEnum)
                                {
                                    if (data is Double)
                                    {
                                        Double castData = (Double)data;
                                        propertyInfo.SetValue(newDef, (int)castData, null);
                                    }
                                    else Debug.Log("Expected Numberic or Enum, Don't know how to parse type: " + data.GetType() + ", value: " + data);
                                }
                                else if (expectedType.Name.Equals("String"))
                                {
                                    if (data is string)
                                    {
                                        propertyInfo.SetValue(newDef, data, null);
                                    }
                                    else if (IsNumericType(data.GetType()))
                                    {
                                        propertyInfo.SetValue(newDef, data.ToString(), null);
                                    }
                                    else if (data is DBNull)
                                    {
                                        // do nothing
                                    }
                                    else Debug.Log("Expected String, Don't know how to parse type: " + data.GetType() + ", value: " + data);
                                }
                                else if (expectedType.Name.Equals("Boolean"))
                                {
                                    if (data is Boolean)
                                    {
                                        propertyInfo.SetValue(newDef, data, null);
                                    }
                                    else if (data is DBNull)
                                    {
                                        propertyInfo.SetValue(newDef, false, null);
                                    }
                                    else Debug.Log("Expected Boolean, Don't know how to parse type: " + data.GetType() + ", value: " + data);
                                }
                                else if(expectedType.Name.Equals("Single"))
                                {
                                    if(data is Double)
                                    {
                                        string s = data.ToString();
                                        propertyInfo.SetValue(newDef, Single.Parse(s), null);
                                    }
                                    else Debug.Log("Expected Single, Don't know how to parse type: " + data.GetType() + ", value: " + data);
                                }
                                else if(data is DBNull)
                                {
                                    // do nothing
                                }
                                //else
                                //    Debug.Log("Don't know how to parse expected type: " + expectedType + ", value: " + data );
                            }
                        }

                        if(!m_loadedData.ContainsKey(newDef.m_ID))
                        {
                            m_loadedData.Add(newDef.m_ID, newDef);
                        }
                        else
                        {
                            Debug.Log("Error in ExcelReader.LoadStaticData - found duplicate ID in sheet " + sheetName + " id " + newDef.m_ID);
                        }
                    }
                }
                return m_loadedData;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception caught in ExcelReader parsing data");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }

        //string con = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}; Dbq=" + fileToRead + ";";
        //Debug.Log(con);
        //string yourQuery = "SELECT * FROM ["+sheetName+"$]";
        //// our odbc connector 
        //OdbcConnection oCon = new OdbcConnection(con);
        //// our command object 
        //OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);

        //Type staticDefType = typeof(T);
        //PropertyInfo[] propertyInfos = staticDefType.GetProperties();

        //// open the connection 
        //oCon.Open();
        //// lets use a datareader to fill that table! 
        //OdbcDataReader rData = oCmd.ExecuteReader();

        //try
        //{
        //    /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    // read data
        //    /////////////////////////////////////////////////////////////////////////////////////////////////////////

        //    // get header names
        //    Dictionary<string, PropertyInfo> m_columnPropertyMap = new Dictionary<string, PropertyInfo>(); ;
        //    for (int columnIndex = 0; columnIndex < rData.FieldCount; columnIndex++)
        //    {
        //        foreach (PropertyInfo propertyInfo in propertyInfos)
        //        {
        //            string headerName = rData.GetName(columnIndex);
        //            if (headerName != null)
        //            {
        //                Debug.Log("Found column " + headerName);
        //                if (propertyInfo.Name == headerName || propertyInfo.Name == "m_" + headerName)
        //                {
        //                    Debug.Log("Connecting " + propertyInfo.Name + " property to column " + columnIndex + ": " + headerName);
        //                    m_columnPropertyMap.Add(headerName, propertyInfo);
        //                }
        //            }
        //        }
        //    }

        //    // turn excel columns into data objects
        //    //while (rData.Read())
        //    //{
        //    //    //T newDef = new T();
        //    //    foreach(var columnIndexPropertyPair in m_columnPropertyMap)
        //    //    {
        //    //        string key = columnIndexPropertyPair.Key;

        //    //        object value = rData[key];
        //    //        Debug.Log(value);

        //    //        //string data = rData.GetDataTypeName(columnIndexPropertyPair.Key);
        //    //        //Type expectedType = columnIndexPropertyPair.Value.PropertyType;

        //    //        //if(IsNumericType(expectedType) && data.Equals("NUMBER"))
        //    //        //{
        //    //        //    columnIndexPropertyPair.Value.SetValue(newDef, rData.GetInt32(columnIndexPropertyPair.Key), null);
        //    //        //}
        //    //        //else if(expectedType.Name.Equals("String"))
        //    //        //{
        //    //        //    columnIndexPropertyPair.Value.SetValue(newDef, rData.GetString(columnIndexPropertyPair.Key), null);
        //    //        //}
        //    //    }
        //    //}

        //    while (rData.Read())
        //    {
        //        Debug.Log(rData.GetString(1));
        //    }


        //}
        //catch (Exception e)
        //{
        //    Debug.LogError("Exception caught in ExcelReader");
        //    Debug.LogError(e.Message);
        //    Debug.LogError(e.StackTrace);
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        //// close that reader! 
        //rData.Close();
        //// close your connection to the spreadsheet! 
        //oCon.Close();

        return null;
    }

    private static HashSet<Type> NumericTypes = new HashSet<Type>
    {
        typeof(int),
        typeof(uint),
        typeof(double),
        typeof(decimal)
    };

    private static bool IsNumericType(Type type)
    {
        return NumericTypes.Contains(type) ||
               NumericTypes.Contains(Nullable.GetUnderlyingType(type));
    }
}
#endif