using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class Variable : Asset
    {
        [SerializeField] private string initialValue;
        [SerializeField] private float initialFloatValue;
        [SerializeField] private bool initialBoolValue;
        
        public string InitialValue
        {
            get => initialValue;
            set => initialValue = value;
        }

        public float InitialFloatValue
        {
            get => initialFloatValue;
            set => initialFloatValue = value;
        }

        public bool InitialBoolValue
        {
            get => initialBoolValue;
            set => initialBoolValue = value;
        }

        public Variable()
        {
            
        }

        public Variable(Variable srcVariable) : base(srcVariable)
        {
            
        }

        private FieldType LookupInitialValueType()
        {
            Field initVal = Field.Lookup(Fields, SystemFields.InitialValue);
            return initVal?.Type ?? FieldType.Text;
        }

        private void SetInitialValueType(FieldType type)
        {
            Field initVal = Field.Lookup(Fields, SystemFields.InitialValue);

            if (initVal == null)
                return;
            
            initVal.Type = type;
            initVal.TypeString = Field.GetTypeString(type);
        }
    }
}