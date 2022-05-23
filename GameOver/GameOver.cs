using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Life_Game
{
    public partial class GameOver : Form
    {
        Form gameForm;
        public GameOver(Form f)
        {
            InitializeComponent();
            gameForm = f;
        }

        private void button1_Click(object sender, EventArgs e) //запуск сохраненной игры
        {
            gameForm.Close();
            Close();
        }
    }
}
