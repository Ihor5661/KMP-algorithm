using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            richTextBox1.AllowDrop = true;
            richTextBox1.DragDrop += new DragEventHandler(DragDropRichTextBox_DragDrop);
            openFileDialog1.FileName = "";
        }

        private string position;
        private void DragDropRichTextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames != null)
            {
                foreach (string name in fileNames)
                {
                    try
                    {
                        richTextBox1.AppendText(File.ReadAllText(name));
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the program?", "MyNotePad", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                richTextBox1.Text = reader.ReadToEnd();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0 || textBox1.Text.Length == 0)
            {
                MessageBox.Show("Insufficient information for this operation", "MyNotePad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            position = FindSubstring(richTextBox1.Text, textBox1.Text);
            for (int ii = 0; ii < (position.ToArray().Where((n) => n == ' ' || n == '\n').Count()); ii++)
            {
                richTextBox1.SelectionStart = (int.Parse(position.Split(' ')[ii]) - textBox1.Text.Length + 1);
                richTextBox1.SelectionLength = textBox1.Text.Length;

                richTextBox1.SelectionColor = Color.FromArgb(140, 35, 180);
                richTextBox1.SelectionBackColor = Color.FromArgb(50, 170, 95);
            }
        }

        private string FindSubstring(string text, string word)
        {
            int lengthText = text.Length; // O (lengthText + lengthWord)
            int lengthWord = word.Length;
            int[] PI = GetPiArray(word);
            int i = 0, j = 0;

            string colorNum = "";

            while (i < lengthText)
            {
                if (text[i] == word[j])
                {
                    i++;
                    j++;
                    if (j == lengthWord)
                    {
                        colorNum += Convert.ToString((i - 1)) + " ";
                        j--;
                        //j = lengthWord - PI[lengthWord - 1];
                        //i = i - (lengthWord - PI[lengthWord - 1]); // aa aaaa -+
                    }
                }
                else
                {
                    if (j > 0)
                    {
                        j = PI[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            if (colorNum.Length == 0)
            {
                MessageBox.Show("No substring found", "MyNotePad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return colorNum;
        }

        private int[] GetPiArray(string word)
        {
            int length = word.Length; //O(length)
            int[] PI = new int[length];

            int i = 1, j = 0;

            PI[0] = 0;
            while (i < length)
            {
                if (word[i] == word[j])
                {
                    PI[i] = j + 1;
                    i++;
                    j++;
                }
                else
                {
                    if (j == 0)
                    {
                        PI[i] = 0;
                        i++;
                    }
                    else
                    {
                        j = PI[j - 1];
                    }
                }

            }
            return PI;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string answer = "";
            string start = "", stop = "";
            try
            {
                for (int ii = 0; ii < (position.ToArray().Where((n) => n == ' ' || n == '\n').Count()); ii++)
                {
                    start = Convert.ToString(int.Parse(position.Split(' ')[ii]) - textBox1.Text.Length + 1);
                    stop = Convert.ToString(int.Parse(position.Split(' ')[ii]));
                    answer += ($"\n{start} - {stop}");
                }
                MessageBox.Show(answer, "MyNotePad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MyNotePad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void findAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionBackColor = Color.White;
            textBox1.Text = "";
            position = "";
        }

        private void claerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = "";
        }
    }
}
