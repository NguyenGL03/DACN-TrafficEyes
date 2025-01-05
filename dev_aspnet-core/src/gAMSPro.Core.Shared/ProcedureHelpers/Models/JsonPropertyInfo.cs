using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace gAMSPro.ProcedureHelpers.Models
{
    // A custom attribute for demonstration purposes
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class JsonPropertyAttribute : Attribute
    {
        public string Description { get; }

        public JsonPropertyAttribute(string description)
        {
            Description = description;
        }
    }
    public class JsonPropertyInfo : PropertyInfo
    {
        private readonly string _name;
        private readonly JToken _value;
        private readonly List<Attribute> _attributes;


        public JsonPropertyInfo(string name, JToken value)
        {
            _name = name;
            _value = value; 
            _attributes = new List<Attribute>();
        }

        public JsonPropertyInfo(string name, JToken value, List<Attribute> attributes = null)
        {
            _name = name;
            _value = value;
            _attributes = attributes ?? new List<Attribute>();
        }

        public override string Name => _name;

        public JToken Value => _value;

        // Required abstract methods can throw NotImplementedException if not used
        public override Type PropertyType => _value.GetType();

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            return _value.ToObject(PropertyType);
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return _attributes.Where(attr => attr.GetType() == attributeType).ToArray();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return _attributes.ToArray();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return _attributes.Any(attr => attr.GetType() == attributeType);
        } 

        public override MethodInfo[] GetAccessors(bool nonPublic) => throw new NotImplementedException();
        public override MethodInfo GetGetMethod(bool nonPublic) => throw new NotImplementedException();
        public override ParameterInfo[] GetIndexParameters() => throw new NotImplementedException();
        public override MethodInfo GetSetMethod(bool nonPublic) => throw new NotImplementedException();
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override Type DeclaringType => throw new NotImplementedException(); 
        public override Type ReflectedType => throw new NotImplementedException();
        public override PropertyAttributes Attributes => throw new NotImplementedException();
    }
}
