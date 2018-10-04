using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //Load Custom Fonts
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        public Form1()
        {
            byte[] fontData = Properties.Resources.fredoka_one_regular;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.fredoka_one_regular.Length);

            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.fredoka_one_regular.Length, IntPtr.Zero, ref dummy);

            fontData = Properties.Resources.arialbd;
            fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            fonts.AddMemoryFont(fontPtr, Properties.Resources.arialbd.Length);

            fontData = Properties.Resources.ARIBLK;
            fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            fonts.AddMemoryFont(fontPtr, Properties.Resources.ARIBLK.Length);

            fontData = Properties.Resources.IMPACT;
            fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            fonts.AddMemoryFont(fontPtr, Properties.Resources.IMPACT.Length);

            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            InitializeComponent();

            this.rbutton1.Font = new Font(fonts.Families[0], 10);
            this.rbutton2.Font = new Font(fonts.Families[1], 10);
            this.rbutton3.Font = new Font(fonts.Families[2], 10);
            this.button2.Font = new Font(fonts.Families[3], 10);

            this.label1.Text = fonts.Families[0].Name;
            this.label2.Text = fonts.Families[1].Name;
            this.label3.Text = fonts.Families[2].Name;
            this.label4.Text = fonts.Families[3].Name;
        }
    }
}