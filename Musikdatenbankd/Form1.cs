using Musikdatenbank;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Musikdatenbankd
{
    public partial class Form1 : Form
    {
        public bool isleer = true;
        public bool isaktuel = true;
        public string DateiPfad;
        public string art = "";
        public string typ = "Album";
        public List<Tontrager> TontragerL;
        public string Arbeitsverzeichniss = Directory.GetCurrentDirectory();
        private IniFile LastFile;
        public DialogResult dialogresult;
        public int sortierung = 11;
        public Form1()
        {

            LastFile = new IniFile(Directory.GetCurrentDirectory() + @"\LastFile.ini");
            TontragerL = new List<Tontrager>();

            InitializeComponent();
            label8.Text = "";
            label10.Text = "";
            label12.Text = "";
            label6.Text = "6";
            Autoopen();
        }

        public class IniFile
        {
            private string path;
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section,
                string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section,
                string key, string def, StringBuilder retVal,
                int size, string filePath);

            public IniFile(string INIPath)
            {
                path = INIPath;
            }
            public Section this[string Section]
            {
                get { return new Section(Section, this); }
            }
            public class Section
            {
                private string Name;
                private IniFile File;
                public Section(string name, IniFile file)
                {
                    Name = name;
                    File = file;
                }
                public string this[string Key]
                {
                    get { return File.IniReadValue(Name, Key); }
                    set { if (value != this[Key]) File.IniWriteValue(Name, Key, value); }
                }
            }
            protected void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }
            protected string IniReadValue(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
                return temp.ToString();
            }
        }

        private void Autoopen()
        {
            LastFile = new IniFile(Directory.GetCurrentDirectory() + @"\LastFile.ini");
            DateiPfad = LastFile["Musikdatenbankd"]["LastFile"];
            if (DateiPfad == "")
            {
                isleer = true;
                return;
            }
            else
            {
                BinaryFormatter bin = new BinaryFormatter();
                TontragerL = bin.Deserialize(File.Open(DateiPfad, FileMode.Open)) as List<Tontrager>;
                isleer = false;
                NeuLaden();
                return;
            }
        }

        public void NeuLaden()
        {
            IEnumerable<Tontrager> TontragerL2;
            listBox1.Items.Clear();
            switch (sortierung)
            {
                case 11:
                    TontragerL2 =
                    from tontrager in TontragerL
                    orderby tontrager.name ascending, tontrager.kunstler ascending, tontrager.jahr ascending, tontrager.tontraegertyp ascending
                    select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "";
                    label12.Text = "";
                    label6.Text = "6";
                    break;
                case 12:
                    TontragerL2 =
                    from tontrager in TontragerL
                    orderby tontrager.name descending, tontrager.kunstler ascending,  tontrager.jahr ascending, tontrager.tontraegertyp ascending
                    select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "";
                    label12.Text = "";
                    label6.Text = "5"; break;
                case 21:
                    TontragerL2 =
                    from tontrager in TontragerL
                    orderby tontrager.kunstler ascending, tontrager.name ascending, tontrager.jahr ascending, tontrager.tontraegertyp ascending
                    select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "6";
                    label10.Text = "";
                    label12.Text = "";
                    label6.Text = ""; break;
                case 22:
                    TontragerL2 =
               from tontrager in TontragerL
               orderby tontrager.kunstler descending, tontrager.name ascending, tontrager.jahr ascending, tontrager.tontraegertyp ascending
               select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "5";
                    label10.Text = "";
                    label12.Text = "";
                    label6.Text = ""; break;
                case 31:
                    TontragerL2 =
               from tontrager in TontragerL
               orderby tontrager.jahr ascending, tontrager.name ascending, tontrager.kunstler ascending, tontrager.tontraegertyp ascending
               select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "6";
                    label12.Text = "";
                    label6.Text = ""; break;
                case 32:
                    TontragerL2 =
                from tontrager in TontragerL
                orderby tontrager.jahr descending, tontrager.name ascending, tontrager.kunstler ascending,  tontrager.tontraegertyp ascending
                select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "5";
                    label12.Text = "";
                    label6.Text = ""; break;
                case 41:
                    TontragerL2 =
               from tontrager in TontragerL
               orderby tontrager.tontraegertyp ascending,tontrager.name ascending, tontrager.kunstler ascending, tontrager.jahr ascending
               select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "";
                    label12.Text = "6";
                    label6.Text = ""; break;
                case 42:
                    TontragerL2 =
               from tontrager in TontragerL
               orderby tontrager.tontraegertyp descending, tontrager.name ascending, tontrager.kunstler ascending, tontrager.jahr ascending
               select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "";
                    label12.Text = "5";
                    label6.Text = ""; break;
                default:
                    TontragerL2 =
                from tontrager in TontragerL
                orderby tontrager.name ascending, tontrager.kunstler ascending, tontrager.jahr ascending, tontrager.tontraegertyp ascending
                select tontrager;
                    listBox1.Items.Clear();

                    label8.Text = "";
                    label10.Text = "";
                    label12.Text = "";
                    label6.Text = "6"; break;
            }
            TontragerL = TontragerL2.ToList();
            foreach (Tontrager Tontrager in TontragerL)
            {
                listBox1.Items.Add(Tontrager.name + " - " + Tontrager.kunstler + " - " + Tontrager.jahr + " - " + Tontrager.tontraegertyp + " - " + Tontrager.sampler);
            }

        }

        private void Beenden()
        {
            DialogResult ergebniss;
            if (isaktuel == false)
            {
                DialogResult dialogResult = MessageBox.Show("Beenden ohne zu Speichern?", "Achtung", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    isaktuel = true;
                    LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                    Close();
                }
                if (dialogResult == DialogResult.No)
                {
                    if (listBox1.Items.Count == 0) return;
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.FileName = "Unbenannt";
                    saveFileDialog1.DefaultExt = "mld";
                    saveFileDialog1.Filter = "Musik-Liste-Datei (*.mld)|*.mld";
                    ergebniss = saveFileDialog1.ShowDialog();
                    if (ergebniss == DialogResult.OK)
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(File.Open(saveFileDialog1.FileName, FileMode.Create), TontragerL);
                        DateiPfad = saveFileDialog1.FileName;
                        isaktuel = true;
                        LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                        Close();
                    }
                    if (ergebniss == DialogResult.Cancel)
                    {
                        LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                        return;
                    }
                }
            }
            else
            {
                LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                Close();

            }
        }

        private void Add()
        {
            if (radioButton7.Checked == true)
            {
                art = "CD";
            }
            if (radioButton6.Checked == true)
            {
                art = "Vinyl";
            }
            if (radioButton5.Checked == true)
            {
                art = "MP3";
            }
            if (radioButton8.Checked == true)
            {
                art = "FLAC";
            }
            if (checkBox1.Checked == true)
            {
                typ = "Sampler";
            }
            if (checkBox1.Checked == false)
            {
                typ = " ";
            }
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (radioButton6.Checked == true || radioButton7.Checked == true || radioButton5.Checked == true || radioButton8.Checked == true)
                {
                    TontragerL.Add(new Tontrager(textBox1.Text, textBox2.Text, textBox3.Text, art, typ));
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    checkBox1.Checked = false;
                    radioButton6.Checked = false;
                    radioButton7.Checked = false;
                    radioButton5.Checked = false;
                    radioButton8.Checked = false;
                    isaktuel = false;
                    isleer = false;
                    NeuLaden();
                }
                else
                {
                    MessageBox.Show("Alle Felder müssen ausgefüllt sein, du Apple-User!");
                }
            }
            else
            {
                MessageBox.Show("Alle Felder müssen ausgefüllt sein, du Apple-User!");

            }
            NeuLaden();
        }

        private void Delete()
        {

            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte wäle ein Item aus");
            }
            if (isleer == true) return;
            else
            {
                TontragerL.Remove(TontragerL[listBox1.SelectedIndex]);
                NeuLaden();
                textBox3.Text = "";
                textBox2.Text = "";
                textBox1.Text = "";
                checkBox1.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
                radioButton5.Checked = false;
                radioButton8.Checked = false;
                isaktuel = false;

            }

        }

        private void Change()
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte wäle ein Item aus");
            }
            if (isleer == true) return;
            else
            {
                TontragerL.Remove(TontragerL[listBox1.SelectedIndex]);
                if (radioButton7.Checked == true)
                {
                    art = "CD";
                }
                if (radioButton6.Checked == true)
                {
                    art = "Vinyl";
                }
                if (radioButton5.Checked == true)
                {
                    art = "MP3";
                }
                if (radioButton6.Checked == true)
                {
                    art = "FLAC";
                }
                if (checkBox1.Checked == true)
                {
                    typ = "Sampler";
                }
                if (checkBox1.Checked == false)
                {
                    typ = " ";
                }
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                {
                    if (radioButton6.Checked == true || radioButton7.Checked == true || radioButton5.Checked == true || radioButton8.Checked == true)
                    {
                        TontragerL.Add(new Tontrager(textBox1.Text, textBox2.Text, textBox3.Text, art, typ));
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        checkBox1.Checked = false;
                        radioButton6.Checked = false;
                        radioButton7.Checked = false;
                        radioButton5.Checked = false;
                        radioButton8.Checked = false;
                        isaktuel = false;
                        isleer = false;
                        NeuLaden();
                    }
                    else
                    {
                        MessageBox.Show("Alle Felder müssen ausgefüllt sein, du Apple-User!");
                    }
                }
                else
                {
                    MessageBox.Show("Alle Felder müssen ausgefüllt sein, du Apple-User!");
                }
            }
        }

        private void neuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.ResetText();
            TontragerL.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            NeuLaden();
            isaktuel = false;
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult ergebniisss;
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = "Unbenannt";
            saveFileDialog1.DefaultExt = "mld";
            saveFileDialog1.Filter = "Musik-Liste-Datei (*.mld)|*.mld";
            ergebniisss = saveFileDialog1.ShowDialog();

            if (ergebniisss == DialogResult.OK)
            {
                if (listBox1.Items.Count == 0) return;

                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(File.Open(saveFileDialog1.FileName, FileMode.Create), TontragerL);
                DateiPfad = saveFileDialog1.FileName;
                isaktuel = true;
                return;
            }

            if (ergebniisss == DialogResult.Cancel)
            {
                return;
            }
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult ergebniisso;

            openFileDialog1.FileName = "Unbenannt";
            openFileDialog1.DefaultExt = "mld";
            openFileDialog1.Filter = "Musik-Liste-Datei (*.mld)|*.mld";
            ergebniisso = openFileDialog1.ShowDialog();
            if (ergebniisso == DialogResult.OK)
            {
                BinaryFormatter bin = new BinaryFormatter();
                TontragerL = bin.Deserialize(File.Open(openFileDialog1.FileName, FileMode.Open)) as List<Tontrager>;
                DateiPfad = openFileDialog1.FileName;
                isleer = false;
                NeuLaden();
                return;
            }
            if (ergebniisso == DialogResult.Cancel)
            {
                return;
            }
        }

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Beenden();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (isaktuel == false)
            {
                DialogResult dialogResult = MessageBox.Show("Beenden ohne zu Speichern?", "Achtung", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                    return;
                }
                if (dialogResult == DialogResult.No)
                {
                    DialogResult ergebniisss;
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.FileName = "Unbenannt";
                    saveFileDialog1.DefaultExt = "mld";
                    saveFileDialog1.Filter = "Musik-Liste-Datei (*.mld)|*.mld";
                    ergebniisss = saveFileDialog1.ShowDialog();

                    if (ergebniisss == DialogResult.OK)
                    {
                        if (listBox1.Items.Count == 0) return;

                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(File.Open(saveFileDialog1.FileName, FileMode.Create), TontragerL);
                        DateiPfad = saveFileDialog1.FileName;
                        LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;

                        isaktuel = true;

                        return;
                    }

                    if (ergebniisss == DialogResult.Cancel)
                    {
                        LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
                        return;
                    }
                }
            }
            else
            {
                LastFile["Musikdatenbankd"]["LastFile"] = DateiPfad;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            NeuLaden();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            NeuLaden();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            NeuLaden();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            NeuLaden();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex == -1)
            {
                return;

            }
            else
            {
                textBox1.Text = TontragerL[listBox1.SelectedIndex].kunstler;
                textBox2.Text = TontragerL[listBox1.SelectedIndex].name;
                textBox3.Text = TontragerL[listBox1.SelectedIndex].jahr;
                if (TontragerL[listBox1.SelectedIndex].tontraegertyp == "CD")
                {
                    radioButton7.Checked = true;
                }
                if (TontragerL[listBox1.SelectedIndex].tontraegertyp == "Vinyl")
                {
                    radioButton6.Checked = true;
                }
                if (TontragerL[listBox1.SelectedIndex].tontraegertyp == "MP3")
                {
                    radioButton5.Checked = true;
                }
                if (TontragerL[listBox1.SelectedIndex].tontraegertyp == "FLAC")
                {
                    radioButton8.Checked = true;
                }
                if (TontragerL[listBox1.SelectedIndex].sampler == "Sampler")
                {
                    checkBox1.Checked = true;
                }
                if (TontragerL[listBox1.SelectedIndex].sampler == " ")
                {
                    checkBox1.Checked = false;
                }
            }

        }

        private void programmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Add();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Change();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Delete();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (sortierung == 11) sortierung = 12;
            else sortierung = 11;
            NeuLaden();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (sortierung == 11) sortierung = 12;
            else sortierung = 11;
            NeuLaden();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (sortierung == 21) sortierung = 22;
            else sortierung = 21;
            NeuLaden();
        }

        private void label8_Click(object sender, EventArgs e)
        {

            if (sortierung == 21) sortierung = 22;
            else sortierung = 21;
            NeuLaden();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            if (sortierung == 31) sortierung = 32;
            else sortierung = 31;
            NeuLaden();
        }

        private void label10_Click(object sender, EventArgs e)
        {

            if (sortierung == 31) sortierung = 32;
            else sortierung = 31;
            NeuLaden();
        }

        private void label11_Click(object sender, EventArgs e)
        {

            if (sortierung == 41) sortierung = 42;
            else sortierung = 41;
            NeuLaden();
        }

        private void label12_Click(object sender, EventArgs e)
        {

            if (sortierung == 41) sortierung = 42;
            else sortierung = 41;
            NeuLaden();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    [Serializable()]
    public class Tontrager
    {
        public string kunstler;
        public string name;
        public string jahr;
        public string tontraegertyp;
        public string sampler;

        public Tontrager(string k, string n, string j, string t1, string t2)
        {
            kunstler = k;
            name = n;
            jahr = j;
            tontraegertyp = t1;
            sampler = t2;

        }
    };
}