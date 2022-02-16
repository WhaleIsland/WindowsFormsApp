using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form : System.Windows.Forms.Form
    {
        private int index, count = 0;
        private ArrayList images = new ArrayList();

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            string command = Environment.CommandLine;
            string[] para = command.Split('\"');
            if (para.Length > 3)
            {
                try
                {
                    PictureBox.Image = Image.FromFile(para[3]);
                    LoadDirectory(para[3]);
                }
                catch (Exception)
                {
                   // nothing...
                }
                
            }

            PictureBox.MouseWheel += new MouseEventHandler(PictureBox1_MouseWheel);
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (PictureBox.Image == null || Width + e.Delta > 1920 || Height + e.Delta > 1080 || Width + e.Delta < 500 || Height + e.Delta < 450)
            {
                return;
            }

            PictureBox.Width += e.Delta;
            PictureBox.Height += e.Delta;
            Width += e.Delta;
            Height += e.Delta;
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); 
            ofd.Multiselect = false;
            ofd.Title = "Select the Image";
            ofd.Filter ="Image Files(*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png";
            DialogResult dialogResult = ofd.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string path = ofd.FileName;
                PictureBox.Image = Image.FromFile(path);
                LoadDirectory(ofd.FileName);
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                // nothing...
            }
            else
            {
                MessageBox.Show("Please select an image file!");

            }

        }

        private void LoadDirectory(string fileName)
        {
            DirectoryInfo rootDir = new DirectoryInfo(fileName.Remove(fileName.LastIndexOf("\\")));

            foreach (var item in rootDir.GetFiles())
            {
                try
                {
                    Image.FromFile(item.FullName);
                }
                catch (Exception)
                {
                    continue;
                }

                images.Add(item.FullName);
            }

            images.Sort(new ImageComparer());
            index = images.IndexOf(fileName);
            count = images.Count;   

        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t Show Photo\t \n\r\n\r\t Version: 1.0.0\t", "About");

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Left && index > 0)
            {
                PictureBox.Image = Image.FromFile(images[index - 1].ToString());
                index--;
            }
            else if (e.KeyData == Keys.Right && index < count - 1)
            {
                PictureBox.Image = Image.FromFile(images[index + 1].ToString());
                index++;

            }
            else
            {
                // nothing...
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right)
            {
                return false;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
        }

        class ImageComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                string sx = x.ToString();
                string sy = y.ToString();
                return sx.CompareTo(sy);
            }
        }

    }
}
