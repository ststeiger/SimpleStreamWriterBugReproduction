
namespace SimpleStreamWriterBugReproduction
{


    public class AAAA
    : System.IO.StreamWriter
    {
        // https://github.com/microsoft/referencesource/blob/master/mscorlib/system/io/streamwriter.cs
        // https://referencesource.microsoft.com/#mscorlib/system/io/streamwriter.cs,f922405b11eca536


        public delegate System.Exception FetchException_t();

        public FetchException_t ExceptionFetcher;


        protected System.Exception Exception
        {
            get
            {
                if (ExceptionFetcher == null)
                    return null;

                return ExceptionFetcher();
            }
        }

        bool m_autoFlush;


        public override bool AutoFlush
        {
            get
            {
                if (Exception != null)
                    return false;

                return this.m_autoFlush;
            }
            set
            {
                m_autoFlush = value;
            }
        }


        public AAAA(System.IO.Stream stream) : base(stream)
        { }

        public AAAA(string path) : base(path)
        { }

        public AAAA(System.IO.Stream stream, System.Text.Encoding encoding)
            : base(stream, encoding)
        { }

        public AAAA(string path, bool append)
            : base(path, append)
        { }

        public AAAA(System.IO.Stream stream, System.Text.Encoding encoding, int bufferSize)
            : base(stream, encoding, bufferSize)
        { }

        public AAAA(string path, bool append, System.Text.Encoding encoding)
            : base(path, append, encoding)
        { }

        public AAAA(System.IO.Stream stream, System.Text.Encoding encoding, int bufferSize, bool leaveOpen)
            : base(stream, encoding, bufferSize, leaveOpen)
        { }

        public AAAA(string path, bool append, System.Text.Encoding encoding, int bufferSize)
            : base(path, append, encoding, bufferSize)
        { }


        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Exception == null)
                    base.Dispose(disposing);
                else
                {
                    Flush();

                    bool leaveOpen = true;

                    if (base.BaseStream != null)
                    {
                        // Note: flush on the underlying stream can throw (ex., low disk space)
                        if (disposing && !leaveOpen)
                        {
                            base.BaseStream.Dispose();
                        }
                    }
                }
            }
            catch (System.Exception)
            { }
        }



        public override void Flush()
        {
            if (this.Exception == null)
                base.Flush();
        }


        public override System.Threading.Tasks.Task FlushAsync()
        {
            if (this.Exception == null)
                return base.FlushAsync();

            return System.Threading.Tasks.Task.CompletedTask;
        }


    }



    public class WebConsoleWriter
        : System.IO.TextWriter
    {

        System.IO.Stream Response;
        bool autoFlush = true;

        bool LeaveOpen = true;


        public override void Close()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Response != null)
                {
                    // Flush();

                    // Note: flush on the underlying stream can throw (ex., low disk space)
                    if (disposing && !LeaveOpen)
                    {
                        this.Response.Dispose();
                    }
                }
            }
            catch (System.Exception)
            { }
        }

        public WebConsoleWriter(System.IO.Stream strm)
        {
            this.Response = strm;
        }

        public override void Write(char[] buffer)
        {
            byte[] charBuffer = this.Encoding.GetBytes(buffer);
            Response.Write(charBuffer, 0, charBuffer.Length);
            if (autoFlush) this.Response.Flush();
        }


        public override void Write(char[] buffer, int index, int count)
        {
            byte[] charBuffer = this.Encoding.GetBytes(buffer);
            Response.Write(charBuffer, 0, charBuffer.Length);
            if (autoFlush) this.Response.Flush();
        }


        public override System.Threading.Tasks.Task WriteAsync(char[] buffer, int index, int count)
        {
            byte[] charBuffer = this.Encoding.GetBytes(buffer);
            return Response.WriteAsync(charBuffer, 0, charBuffer.Length);
        }



        public override void Write(string value)
        {
            byte[] buffer = this.Encoding.GetBytes(value);
            Response.Write(buffer, 0, buffer.Length);
            if (autoFlush) this.Response.Flush();
        }

        public override System.Threading.Tasks.Task WriteAsync(string value)
        {
            byte[] buffer = this.Encoding.GetBytes(value + "\n");
            return Response.WriteAsync(buffer, 0, buffer.Length);
        }


        public override void WriteLine(string value)
        {
            byte[] buffer = this.Encoding.GetBytes(value + "\n");
            Response.Write(buffer, 0, buffer.Length);
            if (autoFlush) this.Response.Flush();
        }

        public override System.Threading.Tasks.Task WriteLineAsync(string value)
        {
            byte[] buffer = this.Encoding.GetBytes(value + "\n");
            return Response.WriteAsync(buffer, 0, buffer.Length);
        }


        public override void WriteLine()
        {
            byte[] buffer = this.Encoding.GetBytes("\n");
            Response.Write(buffer, 0, buffer.Length);
            if (autoFlush) this.Response.Flush();
        }

        public override System.Threading.Tasks.Task WriteLineAsync()
        {
            byte[] buffer = this.Encoding.GetBytes("\n");
            return Response.WriteAsync(buffer, 0, buffer.Length);
        }


        public override void Flush()
        {
            Response.Flush();
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }


}
