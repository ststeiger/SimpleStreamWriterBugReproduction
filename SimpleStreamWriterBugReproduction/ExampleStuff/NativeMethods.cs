
namespace SimpleStreamWriterBugReproduction
{
    

    public class NativeMethods 
    {
        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int MessageBox(System.IntPtr hWnd, string text, string caption, int options);

        public static void ShowMessageBox(string title, string message)
        {
            if (title == null)
                title = "null";

            if (message == null)
                message = "null";

            MessageBox(System.IntPtr.Zero, message, title, 0);
        }
    }
}
