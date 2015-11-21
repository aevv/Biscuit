using System;
using System.Linq;
using System.Reflection;
using DapperExtensions.Mapper;
using DataAccessLayer.Attributes;

namespace DataAccessLayer.Classes
{
    internal sealed class AttributeMapper<T> : ClassMapper<T> where T : class 
    {
        public AttributeMapper()
        {
            Type tableType = typeof (T);
            var table = tableType.GetCustomAttribute<TableAttribute>();
            var schema = tableType.GetCustomAttribute<SchemaAttribute>();

            Table(table.Name);

            if (schema != null)
            {
                Schema(schema.Name);
            }

            AutoMap();
        }

        protected override void AutoMap()
        {
            AutoMap(null);
        }

        protected override void AutoMap(Func<Type, PropertyInfo, bool> canMap)
        {
            Type tableType = typeof (T);
            foreach (PropertyInfo propertyInfo in tableType.GetProperties())
            {
                if (Properties.Any(p => p.Name.Equals(propertyInfo.Name)))
                {
                    continue;
                }

                if (canMap != null && !canMap(tableType, propertyInfo))
                {
                    continue;
                }

                ColumnAttribute columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();

                if (columnAttribute == null)
                {
                    continue;
                }

                PropertyMap propertyMap = new PropertyMap(propertyInfo);
                string column = columnAttribute.Name;
                propertyMap.Column(column);
                PrimaryKeyAttribute keyAttribute = propertyInfo.GetCustomAttribute<PrimaryKeyAttribute>();

                if (keyAttribute != null)
                {
                    propertyMap.Key(keyAttribute.KeyType);
                }

                Properties.Add(propertyMap);
            }
        }
    }
}
