using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Update
    {
        public static void update(object destination, object source)
        {
            var props = source.GetType().GetProperties();
            foreach (var prop in props)
            {
                try
                {
                    if (prop.Name.ToLower() != "id" && prop.GetValue(source) != null)
                    {
                        destination.GetType().GetProperty(prop.Name).SetValue(destination, prop.GetValue(source));
                    }
                }
                catch { }
            }
        }
    }
}
