using System;
using System.Collections.Generic;
using System.IO;

namespace Packets.Extensions
{
    static class Extensions
    {
        private static readonly Dictionary<Type, Action<BinaryWriter, object>> Writers = new Dictionary<Type, Action<BinaryWriter, object>>();
        private static readonly Dictionary<Type, Func<BinaryReader, object>> Readers = new Dictionary<Type, Func<BinaryReader, object>>();

        static Extensions()
        {
            #region Writer Actions
            Writers.Add(typeof(int), (writer, o) => writer.Write((int)o));
            Writers.Add(typeof(long), (writer, o) => writer.Write((long)o));
            Writers.Add(typeof(double), (writer, o) => writer.Write((double)o));
            Writers.Add(typeof(string), (writer, o) => writer.Write((string)o));
            Writers.Add(typeof(float), (writer, o) => writer.Write((float)o));
            Writers.Add(typeof(short), (writer, o) => writer.Write((short)o));
            Writers.Add(typeof(bool), (writer, o) => writer.Write((bool)o));
            Writers.Add(typeof(byte), (writer, o) => writer.Write((byte)o));
            Writers.Add(typeof(Guid), (writer, o) => writer.Write(((Guid)o).ToByteArray()));
            #endregion

            #region Reader Actions
            Readers.Add(typeof(int), reader => reader.ReadInt32());
            Readers.Add(typeof(float), reader => reader.ReadSingle());
            Readers.Add(typeof(Guid), reader => new Guid(reader.ReadBytes(16)));
            Readers.Add(typeof(double), reader => reader.ReadDouble());
            Readers.Add(typeof(short), reader => reader.ReadInt16());
            Readers.Add(typeof(byte), reader => reader.ReadByte());
            Readers.Add(typeof(long), reader => reader.ReadInt64());
            Readers.Add(typeof(bool), reader => reader.ReadBoolean());
            Readers.Add(typeof(string), reader => reader.ReadString());
            #endregion
        }
        internal static void Write(this object val, Type type, BinaryWriter writer)
        {
            Writers[type](writer, val);
        }

        internal static object ReadAny(this BinaryReader reader, Type type)
        {
            return Readers[type](reader);
        }
    }
}
