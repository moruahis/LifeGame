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

namespace Life_Game
{
    public partial class Game : Form//сейчас пишем один большой класс Game
    {
        int n = 7; //переменная размера поля
        int day = 0; //номер дня

        int[,] today; //положение клеток сегодня
        int[,] nextDay; //положение клеток завтра

        string path = Directory.GetCurrentDirectory() + "/data.bin"; //путь файла данных
        
        public Game() //конструктор по умолчанию - вызывается, когда необходима загрузка карты
        {
            InitializeComponent();

            BinaryReader readFile = new BinaryReader(new FileStream(path, FileMode.Open));
            n = readFile.ReadInt32();
            today = new int[n, n];
            nextDay = new int[n, n];

            day = readFile.ReadInt32();
            label1.Text = "День " + day;

            dataGridView1.ColumnCount = n;
            dataGridView1.RowCount = n;

            for (int i = 0; i < n; i++)
            {
                //размеры столбцов и колонок
                dataGridView1.Columns[i].Width = dataGridView1.Width / n; 
                dataGridView1.Rows[i].Height = dataGridView1.Height / n;

                //чтение сохраненной карты
                for (int j = 0; j < n; j++)
                {
                    today[i, j] = readFile.ReadInt32();
                    nextDay[i, j] = 0;
                }
            }

            showToGrid();
            readFile.Close();
        }

        public Game(int size) //конструктор с параметрами - создание поля
        {
            InitializeComponent();

            n = size;
            today = new int[n, n];
            nextDay = new int[n, n];

            dataGridView1.ColumnCount = n;
            dataGridView1.RowCount = n;

            for (int i = 0; i < n; i++)
            {
                dataGridView1.Columns[i].Width = dataGridView1.Width / n;
                dataGridView1.Rows[i].Height = dataGridView1.Height / n;

                //создание пустой карты - заполнение массивов нулями
                for (int j = 0; j < n; j++)
                {
                    today[i, j] = 0;
                    nextDay[i, j] = 0;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //обратчик событий при нажатии на клетку
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor != Color.Black) //если кликнутая ячейка не черная, то сделать ее черной
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Black; 
                today[e.RowIndex, e.ColumnIndex] = 1;
            }
            else //если не белая - белой
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White; 
                today[e.RowIndex, e.ColumnIndex] = 0;
            }
            dataGridView1.ClearSelection();
        }

        void nextDayGeneration() //прогнозирование состояния поля на следующий день (какие клетки умрут, какие нет)
        {

            int left, right, up, down;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (i != 0)
                        left = i - 1;
                    else
                        left = n - 1;

                    if (i != n - 1)
                        right = i + 1;
                    else
                        right = 0;

                    if (j != 0)
                        up = j - 1;
                    else
                        up = n - 1;
                    if (j != n - 1)
                        down = j + 1;
                    else
                        down = 0;
                    int lifeCount = 0;
                    lifeCount += today[left, j];
                    lifeCount += today[left, down];
                    lifeCount += today[left, up];
                    lifeCount += today[right, j];
                    lifeCount += today[right, down];
                    lifeCount += today[right, up];
                    lifeCount += today[i, down];
                    lifeCount += today[i, up];
                    //в lifeCount подсчитывается сколько у клетки живых соседей
                    if (lifeCount == 3 && today[i, j] == 0)//если сегодня клетка мертвая и соседей у нее 3
                        nextDay[i, j] = 1;
                    else if ((lifeCount < 2 || lifeCount > 3) && today[i, j] == 1)
                        nextDay[i, j] = 0;
                    else
                        nextDay[i, j] = today[i, j];
                }
        }
        void copyToDay() //смена дня на завтрашний
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    today[i, j] = nextDay[i, j];
        }
        void showToGrid() //отображение клеток на экран
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (today[i, j] == 1)
                        dataGridView1[j, i].Style.BackColor = Color.Black;
                    else
                        dataGridView1[j, i].Style.BackColor = Color.White;

                }
            dataGridView1.Refresh();

        }

        /***Обработчики событий***/
        private void начатьToolStripMenuItem_Click(object sender, EventArgs e)  //обработка клика на кнопку "Начать"
        {
            timer1.Enabled = true;
        }
        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e) //обработка клика на кнопку "Остановить"
        {
            timer1.Enabled = false;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e) //обработка клика на кнопку "Выход"
        {
            timer1.Enabled = false;
            GameOver newForm = new GameOver(this);
            newForm.Show();
            Enabled = false;
        }

        private void случайноеПолеToolStripMenuItem_Click(object sender, EventArgs e) //обработка клика на кнопку "Случайное поле"
        {
            Random rand = new Random();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    today[i, j] = rand.Next(0, 2);
            showToGrid();
        }

        private void сохранитьПолеToolStripMenuItem_Click(object sender, EventArgs e) //обработка клика на кнопку "Сохранить поле"
        {
            BinaryWriter writeFile = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));

            writeFile.Write(n);
            writeFile.Write(day);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    writeFile.Write(today[i, j]);

            MessageBox.Show("Игра сохранена");
            writeFile.Close();
        }


        private void timer1_Tick(object sender, EventArgs e) //реализация игрового цикла, т.к. таймер - бесконечный цикл с некоторой задержкой
        {
            nextDayGeneration(); //генерируем поле
            copyToDay(); //копируем 
            showToGrid(); //выводим на экран
            label1.Text = "День " + day;
            day++;
        }
    }
}
