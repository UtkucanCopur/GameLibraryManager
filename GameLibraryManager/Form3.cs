using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameLibraryManager
{
    public partial class Form3 : Form
    {
        public Form2 f2;
        public List<Games> games = new List<Games>();
        public int control;


        public Form3(Form2 form2)
        {
            InitializeComponent();
            f2 = form2;
        }


        public void EditGames(string game)
        {
            textBox1.Text = game.Split(',')[0];
            dateTimePicker1.Value = game.Split(',')[1] == "" ? DateTime.Now : DateTime.ParseExact(game.Split(',')[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            comboBox2.Text = game.Split(',')[2];
            textBox2.Text = game.Split(',')[3];
            CheckItemsInCheckedListBox(checkedListBox1, game.Split(',')[4].Split('|').ToList());

            
        }

        public void CheckItemsInCheckedListBox(CheckedListBox clb, List<string> itemsToCheck)
        {
            for (int i = 0; i < clb.Items.Count; i++)
            {
                string currentItem = clb.Items[i].ToString();
                if (itemsToCheck.Contains(currentItem))
                {
                    clb.SetItemChecked(i, true);
                }
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //SAVE BUTTON
            
            games.Add(new Games(textBox1.Text, dateTimePicker1.Value.ToString("dd-MM-yyyy"), comboBox2.Text, checkedListBox1, double.Parse(textBox2.Text, CultureInfo.InvariantCulture)));
            f2.AddGame(games);
            
            f2.RemoveGame();
            this.Hide();
            textBox1.Clear();
            textBox2.Clear();
            comboBox2.SelectedItem = null;
            checkedListBox1.ClearSelected();

        }

        
    }
}
