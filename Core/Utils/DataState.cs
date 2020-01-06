using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SS.GovInteract.Core.Utils
{
    [JsonConverter(typeof(DataStateConverter))]
    public class DataState : IEquatable<DataState>, IComparable<DataState>
    {
        /// <summary>
        /// 新办件
        /// </summary>
        public static readonly DataState New = new DataState(nameof(New));

        /// <summary>
        /// 拒绝受理
        /// </summary>
        public static readonly DataState Denied = new DataState(nameof(Denied));

        /// <summary>
        /// 已受理
        /// </summary>
        public static readonly DataState Accepted = new DataState(nameof(Accepted));

        /// <summary>
        /// 要求返工
        /// </summary>
        public static readonly DataState Redo = new DataState(nameof(Redo));

        /// <summary>
        /// 已办理
        /// </summary>
        public static readonly DataState Replied = new DataState(nameof(Replied));

        /// <summary>
        /// 已审核
        /// </summary>
        public static readonly DataState Checked = new DataState(nameof(Checked));

        public static List<DataState> AllStates => new List<DataState>
        {
            New,
            Denied,
            Accepted,
            Redo,
            Replied,
            Checked
        };

        public static DataState ToDataState(string state)
        {
            if (StringUtils.EqualsIgnoreCase(state, New.Value)) return New;
            if (StringUtils.EqualsIgnoreCase(state, Denied.Value)) return Denied;
            if (StringUtils.EqualsIgnoreCase(state, Accepted.Value)) return Accepted;
            if (StringUtils.EqualsIgnoreCase(state, Redo.Value)) return Redo;
            if (StringUtils.EqualsIgnoreCase(state, Replied.Value)) return Replied;
            if (StringUtils.EqualsIgnoreCase(state, Checked.Value)) return Checked;
            throw new Exception("state not exists");
        }

        internal DataState(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        /// <summary>
        /// 数据类型的值。
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as DataState);
        }

        /// <summary>
        /// 比较两个数据类型是否一致。
        /// </summary>
        /// <param name="a">需要比较的数据类型。</param>
        /// <param name="b">需要比较的数据类型。</param>
        /// <returns>如果一致，则为true；否则为false。</returns>
        public static bool operator ==(DataState a, DataState b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// 比较两个数据类型是否不一致。
        /// </summary>
        /// <param name="a">需要比较的数据类型。</param>
        /// <param name="b">需要比较的数据类型。</param>
        /// <returns>如果不一致，则为true；否则为false。</returns>
        public static bool operator !=(DataState a, DataState b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 比较两个数据类型是否一致。
        /// </summary>
        /// <param name="other">需要比较的数据类型。</param>
        /// <returns>如果一致，则为true；否则为false。</returns>
        public bool Equals(DataState other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 比较两个数据类型是否一致。
        /// </summary>
        /// <param name="other">需要比较的数据类型。</param>
        /// <returns>如果一致，则为0；否则为1。</returns>
        public int CompareTo(DataState other)
        {
            if (other == null)
            {
                return 1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(Value, other.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// 字符串与DataState转换类。
    /// </summary>
    public class DataStateConverter : JsonConverter
    {
        /// <summary>
        /// 确定此实例是否可以转换指定的对象类型。
        /// </summary>
        /// <param name="objectType">对象实例</param>
        /// <returns>
        /// <c>true</c> 如果这个实例可以转换指定的对象类型; 否则, <c>false</c>。
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataState);
        }

        /// <summary>
        /// 编写对象的JSON表示。
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="value">值</param>
        /// <param name="serializer">序列化类</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dataType = value as DataState;
            serializer.Serialize(writer, dataType != null ? dataType.Value : null);
        }

        /// <summary>
        /// 读取对象的JSON表示。
        /// </summary>
        /// <param name="reader">JsonReader</param>
        /// <param name="objectType">对象类型</param>
        /// <param name="existingValue">正在读取的对象的现有值</param>
        /// <param name="serializer">序列化类</param>
        /// <returns>返回对象</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            return string.IsNullOrEmpty(value) ? null : new DataState(value);
        }
    }
}