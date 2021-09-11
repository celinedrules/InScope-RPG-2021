using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class Asset
    {
        protected int id;
        protected List<Field> fields;
        [SerializeField] protected string name;
        [SerializeField] private string description;
        [SerializeField] private FieldType type;


        public Asset()
        {
            // Create theses somewhere else
            // TODO: Look at template.cs
            fields = new List<Field>
            {
                new Field("Name", string.Empty, FieldType.Text),
                new Field("Description", string.Empty, FieldType.Text),
                new Field("Initial Value", string.Empty, FieldType.Text)
            };
        }

        public Asset(Asset src)
        {
            ID = src.ID;
            Fields = src.Fields;
        }

        public List<Field> Fields
        {
            get => fields;
            set => fields = value;
        }

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public FieldType Type
        {
            get => type;
            set => type = value;
        }

        public bool FieldExists(string title) => Field.FieldExists(Fields, title);
        public string LookupValue(string title) => Field.LookupValue(Fields, title);
        public int LookupInt(string title) => Field.LookupInt(Fields, title);
        public float LookupFloat(string title) => Field.LookupFloat(Fields, title);
        public bool LookupBool(string title) => Field.LookupBool(Fields, title);
        public bool IsFieldAssigned(string title) => Field.IsFieldAssigned(Fields, title);
        public Field AssignedField(string title) => Field.AssignedField(Fields, title);
    }
}