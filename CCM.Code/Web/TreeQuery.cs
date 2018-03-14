/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CCM.Code
{
    public static class TreeQuery
    {
        public static List<T> TreeWhere<T>(this List<T> entityList, Predicate<T> condition, string keyValue = "F_Id", string parentId = "F_ParentId") where T : class
        {
            List<T> locateList = entityList.FindAll(condition);
            var parameter = Expression.Parameter(typeof(T), "t");
            List<T> treeList = new List<T>();
            foreach (T entity in locateList)
            {
                treeList.Add(entity);
                string pId = entity.GetType().GetProperty(parentId).GetValue(entity, null).ToString();
                while (true)
                {
                    if (string.IsNullOrEmpty(pId) && pId == "0")
                    {
                        break;
                    }
                    Predicate<T> upLambda = (Expression.Equal(parameter.Property(keyValue), Expression.Constant(pId))).ToLambda<Predicate<T>>(parameter).Compile();
                    T upRecord = entityList.Find(upLambda);
                    if (upRecord != null)
                    {
                        treeList.Add(upRecord);
                        pId = upRecord.GetType().GetProperty(parentId).GetValue(upRecord, null).ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return treeList.Distinct().ToList();
        }
    }
}

