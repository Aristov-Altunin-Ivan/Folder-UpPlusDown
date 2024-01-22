using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string NameSelectedFolder;
        string SelectedFolder;
        List <string> Directories = new List <string> ();

        public void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                SelectedFolder = FBD.SelectedPath;
                OpenFolder(SelectedFolder);
            }
        }
        public void OpenFolder (string SelectedFolder)
        { 
            richTextBox1.Text = "";
            Directories = new List<string>();
            textBox1.Text = SelectedFolder;
            if (Directory.Exists(SelectedFolder))
            {
                if (SelectedFolder.Substring(SelectedFolder.Length-1, 0) == @"\")
                {
                    SelectedFolder = SelectedFolder.Substring(0, SelectedFolder.Length - 1);
                }
                Directories = Directory.GetDirectories(SelectedFolder, "*", SearchOption.TopDirectoryOnly).ToList();
                Directories.ForEach(f => richTextBox1.Text += Path.GetFileName(f) + "\n");
                if (Directories.Count > 0)
                {
                    List<string> list = richTextBox1.Lines.ToList();
                    list.RemoveAt(richTextBox1.Lines.Length - 1);
                    richTextBox1.Lines = list.ToArray();
                    NameSelectedFolder = new DirectoryInfo(System.IO.Path.GetDirectoryName(Directories[0])).Name;
                }
                else
                {
                    MessageBox.Show("Папка пуста!");
                }
            }
            else
            {
                MessageBox.Show("Директория не существует!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directories.Count > 0)
            { 
                foreach (string f in Directories)
                {
                    Directory.Move(f, f + '-' + NameSelectedFolder);
                }
            richTextBox1.Text = "";
            Directories = Directory.GetDirectories(SelectedFolder, "*", SearchOption.TopDirectoryOnly).ToList();
            Directories.ForEach(f => richTextBox1.Text += Path.GetFileName(f) + "\n");
            List<string> list = richTextBox1.Lines.ToList();
            list.RemoveAt(richTextBox1.Lines.Length - 1);
            richTextBox1.Lines = list.ToArray();
            MessageBox.Show("Переименование успешно!");
            }
            else
            {
                MessageBox.Show("Нет выбранных директорий");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text.TrimEnd();
                SelectedFolder = textBox1.Text;
                OpenFolder(SelectedFolder);
            }
            else
            {
                MessageBox.Show("Путь к директории не указан!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> NewName = richTextBox1.Lines.ToList();
            if (Directories.Count > 0 && NewName.Count > 0)
            {
                if (NewName.All(s => !String.IsNullOrWhiteSpace(s)) && NewName.Count == Directories.Count)
                {
                    if (DubleSearch(NewName))
                    {
                        for (int i = 0; i < Directories.Count; i++)
                        {
                            string tmp = SelectedFolder + @"\" + NewName[i];
                            if (Directories[i] != tmp && !Directory.Exists(tmp))
                            {
                                Directory.Move(Directories[i], tmp);
                            }
                        }
                        richTextBox1.Text = "";
                        Directories = Directory.GetDirectories(SelectedFolder, "*", SearchOption.TopDirectoryOnly).ToList();
                        Directories.ForEach(f => richTextBox1.Text += Path.GetFileName(f) + "\n");
                        List<string> list = richTextBox1.Lines.ToList();
                        list.RemoveAt(richTextBox1.Lines.Length - 1);
                        richTextBox1.Lines = list.ToArray();
                        MessageBox.Show("Переименование успешно!");
                    }
                    else
                    {
                        MessageBox.Show("Название дирикторий совпадают!");
                    }
                }
                else
                {
                    MessageBox.Show("Одна из директорий не имеет названия");
                }
            }
            else
            {
                MessageBox.Show("Список директорий пуст!");
            }
        }

        static bool DubleSearch (List<string> NewName)
        {
            int count = 0;
            foreach (string i in NewName)
            {
                count = 0;
                foreach (string j in NewName)
                {
                    if (i == j)
                    {
                        count++;
                    }
                }
                if (count > 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


