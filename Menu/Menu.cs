using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Life_Game
{
    public partial class Menu : Form
    {
        int size = 0;

        public Menu() //инициализация элементов управления (выбор пункта меню)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //выбор размера поля
        {
            size = comboBox1.SelectedIndex + 7;
        }

        private void button1_Click(object sender, EventArgs e) //запуск сохраненной игры
        {
            if (File.Exists("data.bin")) //если существует файл с сохранением, то запускаем
            {
                Game newForm = new Game();
                newForm.Show();
            }
            else
                MessageBox.Show("Нет сохранений"); //иначе предупреждаем пользователя
        }

        private void button2_Click(object sender, EventArgs e) //запуск новой игры
        {
            Game newForm = new Game(size);
            newForm.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
