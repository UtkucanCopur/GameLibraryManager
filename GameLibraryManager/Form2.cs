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

namespace GameLibraryManager
{
    public partial class Form2: Form
    {
        
        private Form1 f1;
        private Form3 f3;
        public int counterForGameList = 0;
        



        public Form2(Form1 form1)
        {
            InitializeComponent();
            
            LoadListBoxData();
            f1 = form1;
            f3 = new Form3(this);
            
        }

        

        

        public void SaveListBoxData()
        {
            
            using (StreamWriter writer = new StreamWriter("listboxdata.txt"))
            {
                foreach (var item in listBox1.Items)
                {
                    writer.WriteLine(item.ToString());  // Her öğeyi dosyaya yaz
                }
            }
        }

        private void LoadListBoxData()
        {
            if (File.Exists("listboxdata.txt"))
            {
                using (StreamReader reader = new StreamReader("listboxdata.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);  // Dosyadaki her satırı ListBox'a ekle
                    }
                }
            }
        }

        
        public void AddGame(List<Games> game)  // Dışarıdan ListBox’a eleman eklemek için metod
        {
            listBox1.Items.Add(game[counterForGameList]);
            counterForGameList++;
        }

        

        public void ViewGamesByName()
        {
            List<string> games = new List<string>();
            foreach (var item in listBox1.Items)
            {
                games.Add(item.ToString());
            }

            games.Sort();

            


            foreach(var item in games)
            {
                listView1.Items.Add(item);
            }

            
        }

        public void ViewGamesByPlayTime()
        {
            List<string> games = new List<string>();
            foreach (var item in listBox1.Items)
            {
                games.Add(item.ToString());
            }
            
            games.Sort((y, x) =>
            {
                double playTimeX = Convert.ToDouble(x.Split(',')[3]);
                double playTimeY = Convert.ToDouble(y.Split(',')[3]);
                return playTimeX.CompareTo(playTimeY);
            });

            



            foreach (var item in games)
            {
                listView1.Items.Add(item);
            }

            

        }

        public void ViewGamesByCompleted()
        {
            List<string> games = new List<string>();
            foreach (var item in listBox1.Items)
            {
                if (item.ToString().Split(',')[2].Equals("Completed"))
                {
                    games.Add(item.ToString());
                }
                
                
            }

            foreach (var item in games)
            {
                listView1.Items.Add(item);
            }


        }

        public void ViewGamesByNotStarted()
        {
            List<string> games = new List<string>();
            foreach (var item in listBox1.Items)
            {
                if (item.ToString().Split(',')[2].Equals("Not Started"))
                {
                    games.Add(item.ToString());
                }


            }

            foreach (var item in games)
            {
                listView1.Items.Add(item);
            }
        }

        public void ViewGamesByPlaying()
        {
            List<string> games = new List<string>();
            foreach (var item in listBox1.Items)
            {
                if (item.ToString().Split(',')[2].Equals("Playing"))
                {
                    games.Add(item.ToString());
                }


            }

            foreach (var item in games)
            {
                listView1.Items.Add(item);
            }
        }

        


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

            SaveListBoxData();
            listView1.Items.Clear();
            e.Cancel = true;
            this.Hide();
            f1.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listView1.Items.Clear();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            } else
            {
                MessageBox.Show("Game not selected");
            }
            



        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                
                if (string.Equals(listBox1.Items[i].ToString().Split(',')[0],textBox1.Text,StringComparison.OrdinalIgnoreCase))
                {
                    listView1.Items.Add(listBox1.Items[i].ToString());
                    
                } 
            }

            

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //EDIT BUTTON
            
            
            if (listBox1.SelectedItem != null)
            {
                f3.EditGames(listBox1.SelectedItem.ToString());
            }
            f3.ShowDialog();
            
        }

        public void RemoveGame()
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            else
            {
                MessageBox.Show("Game not selected");
            }
        }
    }
}
