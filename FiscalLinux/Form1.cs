using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FP3530;
using System.IO;


namespace FiscalLinux
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public FP3530.CS_BGR_FP2000_KL fp = new CS_BGR_FP2000_KLClass();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.MaximizeBox = false;
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Filter = "*.txt";
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);
            watcher.Path = @"C:/FPLinux/";
            watcher.EnableRaisingEvents = true;
            this.MaximizeBox = false;

            string path = @"C:/FPLinux/Sale.txt";
            if (File.Exists(path))
            {
                System.Threading.Thread.Sleep(1000);
                MakeSell();
            }
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 31;
            numericUpDown2.Minimum = 1;
            numericUpDown2.Maximum = 12;
            numericUpDown3.Minimum = 00;
            numericUpDown3.Maximum = 24;
            numericUpDown3.Value = 14;
            numericUpDown4.Minimum = 00;
            numericUpDown4.Maximum = 24;
            numericUpDown4.Value = 14;
            numericUpDown5.Minimum = 1;
            numericUpDown5.Maximum = 12;
            numericUpDown6.Minimum = 1;
            numericUpDown6.Maximum = 31;
            label2.Text = "OK";
            
        }

         public void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(@"C:/FPLinux/Sale.txt"))
            {
                System.Threading.Thread.Sleep(1000);
                FP3530.CS_BGR_FP2000_KL fp = new CS_BGR_FP2000_KLClass();
                MakeSell();
            }
        }
        private void MakeSell()
        {
            string item = "";
            string itemPrice = "";
            string itemQuantity = "";
            string itemDiscount = "";
            string clientPayIn = "";
            string myPath = @"C:/FPLinux/Sale.txt";
            string[] lines = File.ReadAllLines(myPath);
            var lineCount = lines.Length;
            for (int i = 0; i <= lines.Length; i++)
            {
                try
                {
                    switch (lines[i])
                    {
                        case "Open":
                            fp.INIT_EX2("192.168.123.123", "9100");
                            break;
                    }
                    switch (lines[i])
                    {
                        case "Operator":
                            fp.CMD_48_0_0("1", "0000", "1", "", "");
                            break;
                    }
                    switch (lines[i])
                    {
                        case "OneItemSale":
                            item = lines[i + 1];
                            itemPrice = lines[i + 2];
                            itemQuantity = lines[i + 3];
                            itemDiscount = lines[i + 4];
                            OneItemSale(item, itemPrice, itemQuantity, itemDiscount);
                            break;
                    }
                    switch (lines[i])
                    {
                        case "Total":
                            clientPayIn = lines[i + 1];
                            fp.CMD_53_0_0("", "", "P", clientPayIn, "1", "");
                            break;
                    }
                    switch (lines[i])
                    {
                        case "Close":
                            fp.CMD_56_0_0("", "");
                            break;
                    }
                }
                catch (Exception)
                {

                    break;
                }
                


            }
            string dateItems = DateTime.Now.ToString("dd-MM-yyyy----HH-mm");
            string content = File.ReadAllText(@"C:/FPLinux/Sale.txt");
            string path = @"C:/FPLinux/Archive/" + dateItems + ".txt";
            TextWriter t = new StreamWriter(path, true);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            t.Write(content);
            t.Close();
            File.Delete(@"C:/FPLinux/Sale.txt");
        }
        private void OneItemSale(string item, string itemPrice, string itemQuantity, string itemDiscount)
        {
            fp.CMD_49_0_0(item, "", "Б", itemPrice, itemQuantity, itemDiscount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "Дневен финансов отчет с нулиране...";
            string Closure = "";
            string fmTotal = "";
            string TotA = "";
            string TotB = "";
            string TotC = "";
            string TotD = "";
            string TotE = "";
            string TotF = "";
            string TotG = "";
            string TotH = "";
            fp.INIT_EX2("192.168.123.123", "9100");
            fp.CMD_69_0_0("0", "", ref Closure, ref fmTotal, ref TotA, ref TotB, ref TotC, ref TotD, ref TotE, ref TotF, ref TotG, ref TotH);
            label2.Text = "ОК";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "Дневен финансов отчет без нулиране...";
            string Closure = "";
            string fmTotal = "";
            string TotA = "";
            string TotB = "";
            string TotC = "";
            string TotD = "";
            string TotE = "";
            string TotF = "";
            string TotG = "";
            string TotH = "";
            fp.INIT_EX2("192.168.123.123", "9100");
            fp.CMD_69_0_0("2", "", ref Closure, ref fmTotal, ref TotA, ref TotB, ref TotC, ref TotD, ref TotE, ref TotF, ref TotG, ref TotH);
            label2.Text = "ОК";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "Отчет от дата до дата...";
            string startDate = numericUpDown1.Value.ToString().PadLeft(2, '0') + numericUpDown2.Value.ToString().PadLeft(2, '0') + numericUpDown3.Value.ToString().PadLeft(2, '0');
            string endDate = numericUpDown6.Value.ToString().PadLeft(2, '0') + numericUpDown5.Value.ToString().PadLeft(2, '0') + numericUpDown4.Value.ToString().PadLeft(2, '0');
            fp.INIT_EX2("192.168.123.123", "9100");
            fp.CMD_79_0_0(startDate, endDate);
            label2.Text = "ОК";
        }

     }
}
