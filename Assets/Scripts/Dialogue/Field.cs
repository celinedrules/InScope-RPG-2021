using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Dialogue
{
    [Serializable]
    public class Field
    {
        private string title;
        private string value;
        private FieldType type = FieldType.Text;
        private string typeString = string.Empty;

        public FieldType Type
        {
            get => type;
            set => type = value;
        }

        public string TypeString
        {
            get => typeString;
            set => typeString = value;
        }

        public string Value
        {
            get => value;
            set => this.value = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public Field()
        {
            
        }

        public Field(string title, string value, FieldType type)
        {
            this.Title = title;
            this.Value = value;
            this.type = type;
        }

        public Field(string title, string value, FieldType type, string typeString)
        {
            this.title = title;
            this.value = value;
            this.type = type;
            this.typeString = typeString;
        }
        
        public Field(Field src)
        {
            Title = src.Title;
            Value = src.Value;
            type = src.Type;
        }

        public static List<Field> CopyFields(IEnumerable<Field> src) => src.Select(srcField => new Field(srcField)).ToList();
        public static bool FieldExists(List<Field> fields, string title) => Lookup(fields, title) != null;
        public static Field Lookup(List<Field> fields, string title) => fields?.Find(f => string.Equals(f.Title, title));
        public static string LookupValue(List<Field> fields, string title) => Lookup(fields, title)?.Value;
        public static int LookupInt(List<Field> fields, string title) => Tools.StringToInt(LookupValue(fields, title));
        public static float LookupFloat(List<Field> fields, string title) => Tools.StringToFloat(LookupValue(fields, title));
        public static bool LookupBool(List<Field> fields, string title) => Tools.StringToBool(LookupValue(fields, title));
        public static string FieldValue(Field field) => field?.Value;
        public static bool IsFieldAssigned(List<Field> fields, string title) => (AssignedField(fields, title) != null);
        public static Field AssignedField(List<Field> fields, string title)
        {
            Field field = Lookup(fields, title);
            return ((field != null) && !string.IsNullOrEmpty(field.Value)) ? field : null;
        }
        public static void SetValue(List<Field> fields, string title, string value) => SetValue(fields, title, value, FieldType.Text);
        public static void SetValue(List<Field> fields, string title, float value) => SetValue(fields, title, value.ToString(System.Globalization.CultureInfo.InvariantCulture), FieldType.Number);
        public static void SetValue(List<Field> fields, string title, int value)=> SetValue(fields, title, value.ToString(), FieldType.Number);
        public static void SetValue(List<Field> fields, string title, bool value) =>SetValue(fields, title, value.ToString(), FieldType.Bool);
        
        public static void SetValue(List<Field> fields, string title, string value, FieldType type)
        {
            Field field = Lookup(fields, title);

            if (field != null)
            {
                field.Value = value;
                field.Type = type;
            }
            else
            {
                fields.Add(new Field(title, value, type));
            }
        }
        
        public static string GetTypeString(FieldType type) => type switch
        {
            FieldType.Character => "CustomFieldType_Character",
            FieldType.Bool => "CustomFieldType_Bool",
            FieldType.Item => "CustomFieldType_Item",
            FieldType.Area => "CustomFieldType_Area",
            FieldType.Number => "CustomFieldType_Number",
            _ => string.Empty
        };
    }
}