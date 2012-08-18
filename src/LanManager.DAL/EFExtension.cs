using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace LanManager.DAL
{
    public static class EFExtension
    {
        /// <summary>
        /// Sets the modified state property for all properties of the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="context"></param>
        public static void SetAllModified<T>(this T entity, ObjectContext context) where T : IEntityWithKey
        {
            var stateEntry = context.ObjectStateManager.GetObjectStateEntry(entity.EntityKey);
            var propertyNameList = stateEntry.CurrentValues.DataRecordInfo.FieldMetadata.Select(pn => pn.FieldType.Name);
            foreach (var propName in propertyNameList)
            {
                stateEntry.SetModifiedProperty(propName);
            }
        }
    }
}
