using System;
using System.ComponentModel;
using System.Globalization;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal class CpuSetTypeConverter : TypeConverter
    {
        private static readonly Type StriType = typeof(string);
        
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == StriType)
            {
                return true;
            }
            
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                return CpuSet.Parse(str);
            }
            
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == StriType)
            {
                return value.ToString();
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}