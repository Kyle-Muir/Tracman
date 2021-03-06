﻿using System;

namespace Tracman.Core
{
    public class Setting
    {
        public Setting(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");
            Key = key;
            Value = value;
        }

        public Setting()
        {
        }

  
        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public int ToInt()
        {
            int intValue;
            if (int.TryParse(Value, out intValue))
            {
                return intValue;
            }
            string error = "The configuration setting '{0}' with value '{1}' cannot be converted to an int".FormatWith(Key, Value);
            throw new InvalidOperationException(error);
        }

        public decimal ToDecimal()
        {
            decimal decimalValue;
            if (decimal.TryParse(Value, out decimalValue))
            {
                return decimalValue;
            }
            string error = "The configuration setting '{0}' with value '{1}' cannot be converted to a decimal".FormatWith(Key, Value);
            throw new InvalidOperationException(error);
        }

        public T ToEnum<T>() where T : struct
        {
            T enumValue;
            if (Enum.TryParse(Value, out enumValue))
            {
                return enumValue;
            }

            string error = "The configuration setting '{0}' with value '{1}' cannot be converted to an enum of type {2}".FormatWith(Key, Value, typeof(T));
            throw new InvalidOperationException(error);
        }

        public Uri ToUri()
        {
            return new Uri(ToString());
        }

        public bool ToBool()
        {
            bool outValue;
            if (bool.TryParse(Value, out outValue))
            {
                return outValue;
            }

            string error =
                "The configuration setting '{0}' with value '{1}' cannot be converted to boolean value".FormatWith(Key,
                    Value);
            throw new InvalidOperationException(error);
        }

        public Guid ToGuid()
        {
            Guid guidValue;
            if (Guid.TryParse(Value, out guidValue))
            {
                return guidValue;
            }
            string error = "The configuration setting '{0}' with value '{1}' cannot be converted to a Guid".FormatWith(Key, Value);
            throw new InvalidOperationException(error);
        }
    }
}