using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Yunchee.Volkswagen.Utility
{
    public class DataTableToObject
    {
        public DataTableToObject() { }

        /// <summary>
        /// Convert an DataRow to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ConvertToObject<T>(DataRow row) where T : new()
        {
            System.Object obj = new T();
            if (row != null)
            {
                DataTable dataTable = row.Table;
                GetObject(dataTable.Columns, row, obj);
            }
            if (obj != null && obj is T)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
        }

        private static void GetObject(DataColumnCollection cols, DataRow dr, System.Object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] pros = type.GetProperties();
            foreach (PropertyInfo pro in pros)
            {
                if (cols.Contains(pro.Name))
                {
                    pro.SetValue(obj, dr[pro.Name] == DBNull.Value ? null : dr[pro.Name], null);
                }
            }
        }

        /// <summary>
        /// Convert a data table to an objct list  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dataTable) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T obj = ConvertToObject<T>(row);
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// Convert object collection to an data table  
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTableFromList(System.Object list)
        {
            DataTable dt = null;
            System.Type listType = list.GetType();

            if (listType.IsGenericType)
            {
                System.Type type = listType.GetGenericArguments()[0];
                dt = new DataTable(type.Name + "List");
                MemberInfo[] mems = type.GetMembers(BindingFlags.Public | BindingFlags.Instance);

                #region 表结构构建
                foreach (MemberInfo mem in mems)
                {
                    //switch(mem.MemberType)
                    //{
                    //    case MemberTypes.Property:
                    //        {
                    //            dt.Columns.Add(((PropertyInfo)mem).Name,typeof(System.String));
                    //            break;
                    //        }
                    //    case MemberTypes.Field:
                    //        {
                    //            dt.Columns.Add(((FieldInfo)mem).Name,typeof(System.String));
                    //            break;
                    //        }
                    //}
                    dt.Columns.Add(mem.Name, mem.ReflectedType);
                }
                #endregion

                #region 表数据填充
                IList iList = list as IList;
                foreach (System.Object record in iList)
                {
                    System.Int32 i = 0;
                    System.Object[] fieldValues = new System.Object[dt.Columns.Count];
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        MemberInfo mem = listType.GetMember(dataColumn.ColumnName)[0];
                        switch (mem.MemberType)
                        {
                            case MemberTypes.Field:
                                {
                                    fieldValues[i] = ((FieldInfo)mem).GetValue(record);
                                    break;
                                }
                            case MemberTypes.Property:
                                {
                                    fieldValues[i] = ((PropertyInfo)mem).GetValue(record, null);
                                    break;
                                }
                        }
                        i++;
                    }
                    dt.Rows.Add(fieldValues);
                }
                #endregion

            }

            return dt;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, null);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    var colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    if (propertyNameList.Count == 0)                    {

                        result.Columns.Add(pi.Name, colType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, colType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}
