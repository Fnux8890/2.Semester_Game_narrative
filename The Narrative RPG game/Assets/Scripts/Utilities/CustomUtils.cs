using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class CustomUtils
    {
        public static T InstanceOfType<T>(List<Component> list)
        {
            var result =  list.Find(component => 
                component.GetType() == typeof(T));
            var resultOfType = (T) Convert.ChangeType(result, typeof(T));
            return resultOfType;
        } 
        
        public static T InstanceOfType<T>(List<Component> list, string search)
        {
            var result = list.Find(component => 
                component.GetType() == typeof(T) && component.name == search);
            var resultOfType = (T) Convert.ChangeType(result, typeof(T));
            return resultOfType;
        }
    }
}