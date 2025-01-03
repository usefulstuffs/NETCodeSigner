using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETCodeSigner
{
    public partial class Form1 : Form
    {
        // Variables
        private static string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static bool SelectedAppFile = false;
        private static bool SelectedCertFile = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Browse app file",
                Filter = "Application files|*.exe|System files|*.dll|System files|*.sys",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                SelectedAppFile = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Browse certificate",
                Filter = "Certificate file|*.pfx",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.FileName;
                SelectedCertFile = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(exeDir + @"\tools\signtool.exe"))
            {
                MessageBox.Show("SignTool.exe was not found. Please re-download the application.",".NET Code Signer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!SelectedAppFile)
            {
                MessageBox.Show("Please select an application file to sign!", ".NET app signer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!SelectedCertFile)
            {
                MessageBox.Show("Please select a certificate file!", ".NET app signer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox2.Text != "SHA1" && comboBox2.Text != "SHA256")
            {
                MessageBox.Show("Please select an hash algorithm!",".NET app signer",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Certificate password cannot be empty!", ".NET app signer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //MessageBox.Show("signtool.exe " + "sign /f \"" + textBox2.Text + "\" /p \"" + textBox3.Text + "\" /fd " + comboBox2.Text + " /t http://timestamp.digicert.com /d \"" + textBox4.Text + "\" \"" + textBox1.Text + "\"", ".NET app signer - Debugging", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = exeDir + @"\tools\signtool.exe",
                    Arguments = "sign /f \"" + textBox2.Text + "\" /p \"" + textBox3.Text + "\" /fd " + comboBox2.Text + " /t http://timestamp.digicert.com /d \"" + textBox4.Text + "\" \"" + textBox1.Text + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };
                Process.Start(info).WaitForExit();
                MessageBox.Show("Application file should be signed now.",".NET app signer",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
    }
}
