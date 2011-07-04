using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Braindrops.Reflection;
using Common.Logging;
using Minimod.PrettyTypeSignatures;

namespace Braindrops.Freezable
{
    public static class CloneHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (CloneHelper));

        public static object CreateDeepClone(object obj, Type targetType)
        {
            if (obj == null)
            {
                return null;
            }

            if (!targetType.IsAssignableFrom(obj.GetType()))
            {
                throw new Exception(
                    string.Format(
                                     "Can't clone a object of type {0} to type {1}.",
                                     obj.GetType().GetPrettyName(),
                                     targetType.GetPrettyName()));
            }

            if (obj is ICloneable)
            {
                return ((ICloneable) obj).Clone();
            }

            _log.Warn("DeepCloning is done via BinaryFormatter (performance: slow) for type: " +
                      targetType.GetPrettyName());
            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                memStream.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(memStream);
            }
        }

        public static T CreateDeepClone<T>(T obj)
        {
            if (Equals(obj, default(T)))
            {
                return default(T);
            }

            return (T) CreateDeepClone(obj, typeof (T));
        }
    }
}