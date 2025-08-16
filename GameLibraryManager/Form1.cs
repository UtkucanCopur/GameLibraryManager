using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameLibraryManager
{
    public partial class Form1 : Form
    {
        public List<Games> gameList = new List<Games>();
        public Timer timer;
        public Form2 form2;

        public Form1()
        {
            InitializeComponent();
            form2 = new Form2(this);
            ViewMostPlayedGames();
            timer = timer1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            form2.Show();
            this.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Playing" || comboBox2.Text == "Not Started")
            {
                textBox2.Text = "0";
            }


            if (textBox1.Text == "" || textBox2.Text == "" || comboBox2.SelectedItem == null || checkedListBox1.CheckedItems == null)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            else
            {
                textBox3.Text = dateTimePicker1.Value.ToString("dd-MM-yyyy");

                double playTime = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                gameList.Add(new Games(textBox1.Text, textBox3.Text, comboBox2.SelectedItem.ToString(), checkedListBox1, playTime));
                form2.AddGame(gameList);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                comboBox2.SelectedItem = null;
                foreach (int index in checkedListBox1.CheckedIndices)
                {
                    checkedListBox1.SetItemChecked(index, false);
                }
                checkedListBox1.SelectedItem = null;
                MessageBox.Show("Game Added");

                listBox1.BeginUpdate();
                try
                {
                    form2.SaveListBoxData();
                    ViewMostPlayedGames();
                }
                finally
                {
                    listBox1.EndUpdate();
                }
                timer1.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            form2.ViewGamesByName();
            form2.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form2.ViewGamesByPlayTime();
            form2.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            form2.ViewGamesByCompleted();
            form2.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            form2.ViewGamesByNotStarted();
            form2.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form2.ViewGamesByPlaying();
            form2.Show();
            this.Hide();
        }

        

        

        public void ViewMostPlayedGames()
        {
            List<string> mostPlayedGames = new List<string>();
            if (File.Exists("listboxdata.txt"))
            {
                using (StreamReader reader = new StreamReader("listboxdata.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        mostPlayedGames.Add(line);
                    }
                }
            }

            mostPlayedGames.Sort((y, x) =>
            {
                double playTimeX = double.Parse(x.Split(',')[3], CultureInfo.InvariantCulture);
                double playTimeY = double.Parse(y.Split(',')[3], CultureInfo.InvariantCulture);
                return playTimeX.CompareTo(playTimeY);
            });

            listBox1.Items.Clear();
            if (mostPlayedGames.Count >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    listBox1.Items.Add(mostPlayedGames[i]);
                }
            }
            else
            {
                for (int i = 0; i < mostPlayedGames.Count; i++)
                {
                    listBox1.Items.Add((mostPlayedGames[i]));
                }
            }

            if (mostPlayedGames.Count > 0)
            {
                label6.Text = mostPlayedGames[0];
            }

            double sum = 0;
            for (int i = 0; i < mostPlayedGames.Count; i++)
            {
                sum = sum + double.Parse(mostPlayedGames[i].Split(',')[3].Trim(), CultureInfo.InvariantCulture);
            }

            double avg = mostPlayedGames.Count > 0 ? sum / (double)mostPlayedGames.Count : 0;
            
            label12.Text = avg.ToString("0.00", CultureInfo.InvariantCulture);
            label8.Text = sum.ToString("0.00", CultureInfo.InvariantCulture);


            int sumForComp = 0;
            int sumForNotStarted = 0;
            int sumForPlaying = 0;
            int sumForNotFinished = 0;

            foreach (var item in mostPlayedGames)
            {
                if (item.Split(',')[2] == "Playing")
                {
                    sumForPlaying = sumForPlaying + 1;
                } else if (item.Split(',')[2] == "Completed")
                {
                    sumForComp = sumForComp + 1;
                } else if (item.Split(',')[2] == "Not Started")
                {
                    sumForNotStarted = sumForNotStarted + 1;
                } else if (item.Split(',')[2] == "Not Finished")
                {
                    sumForNotFinished = sumForNotFinished + 1;
                }
            }

            label17.Text = "Not Started: " + sumForNotStarted.ToString();
            label18.Text = "Not Finished: " + sumForNotFinished.ToString();
            label16.Text = "Playing: " + sumForPlaying.ToString();
            label15.Text = "Completed: " + sumForComp.ToString();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            form2.SaveListBoxData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ViewMostPlayedGames();
        }

        

        private void Form1_Activated(object sender, EventArgs e)
        {
            ViewMostPlayedGames();
            timer1.Enabled = true;
            timer1.Enabled = false;
        }
    }

    public class Games
    {
        public string gameName;
        public string releaseDate;
        public string status;
        public double playTime;
        public List<string> gameTag = new List<string>();

        public Games() { }

        public Games(string name, string releaseDate, string status, CheckedListBox tag, double playTime)
        {
            this.gameName = name;
            this.releaseDate = releaseDate;
            this.status = status;
            foreach (var item in tag.CheckedItems)
            {
                gameTag.Add(item.ToString());
            }
            this.playTime = playTime;
        }

        public void SetProps(string name, string releaseDate, string status, CheckedListBox tag, double playTime)
        {
            this.gameName = name;
            this.releaseDate = releaseDate;
            this.status = status;
            foreach (var item in tag.CheckedItems)
            {
                gameTag.Add(item.ToString());
            }
            this.playTime = playTime;
        }

        public override string ToString()
        {
            var culture = CultureInfo.InvariantCulture;
            return $"{gameName},{releaseDate},{status},{playTime.ToString("0.00", culture)},{string.Join("|", gameTag)}";
        }
    }
}


