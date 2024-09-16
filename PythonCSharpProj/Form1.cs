namespace PythonCSharpProj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*pictureBox1.MouseMove += pictureBox1.pictureBox_MouseMove;
            pictureBox1.MouseDown += pictureBox1.pictureBox_MouseDown;
            pictureBox1.MouseWheel += pictureBox1.pictureBox_MouseWheel;
            pictureBox1.Paint += pictureBox1.pictureBox_Paint;*/
            Init();
        }

        private void Init()
        {
            string pyDllPath = @"C:\Users\28602\AppData\Local\Programs\Python\Python310\python310.dll";
            string PyPath = @"G:\PythonCSharp\PythonCSharpProj\PythonCSharpProj\PythonCode";
            PyInterface.SetPyEnvironment(pyDllPath, PyPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] imageBytes = PyInterface.CallPythonMethod<byte[]>("testFunc", "TestFunc", new object[] {"111222"}, LogMsg);
            pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
            errorProvider1.SetError(button1, "ERROR!");
        }

        private void LogMsg(object msg)
        {
            richTextBox1.AppendText($"[Debug]{msg.ToString()}\n");
        }
    }
}