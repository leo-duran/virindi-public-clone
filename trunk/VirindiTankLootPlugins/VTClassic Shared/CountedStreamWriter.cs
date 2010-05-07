using System;
using System.IO;

namespace VTClassic
{
    internal class CountedStreamWriter : StreamWriter
    {
        public CountedStreamWriter(Stream u)
            : base(u)
        {

        }

        public CountedStreamWriter(string u)
            : base(u)
        {

        }

        int iCount = 0;
        public int Count { get { return iCount; } }
        public void ResetCount() { iCount = 0; }

        public override void Write(string value)
        {
            iCount += value.Length;
            base.Write(value);
        }
        public override void Write(char[] buffer)
        {
            iCount += buffer.Length;
            base.Write(buffer);
        }
        public override void Write(char[] buffer, int index, int count)
        {
            iCount += count;
            base.Write(buffer, index, count);
        }
        public override void WriteLine()
        {
            Write(NewLine);
        }
        public override void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(char[] buffer)
        {
            Write(buffer);
            WriteLine();
        }
        public override void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        public override void Write(bool value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(char value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(decimal value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(double value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(float value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(int value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(long value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(object value)
        {
            Write(value.ToString());
        }
        public override void Write(uint value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(ulong value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(string format, params object[] arg)
        {
            Write(String.Format(format, arg));
        }
        public override void Write(string format, object arg0)
        {
            Write(String.Format(format, arg0));
        }
        public override void Write(string format, object arg0, object arg1)
        {
            Write(String.Format(format, arg0, arg1));
        }
        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(String.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(decimal value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0)
        {
            Write(format, arg0);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            Write(format, arg0, arg1);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(format, arg0, arg1, arg2);
            WriteLine();
        }
        public override void WriteLine(string format, params object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }

    }
}