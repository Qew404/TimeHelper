using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TimeHelper
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private Timer pauseTimer;
        private TimeSpan remainingTime;
        private TimeSpan totalPauseTime;
        private bool isPaused;
        private bool isTimerRunning; // Флаг что таймер вообще запущен

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            // Подписываемся на события блокировки/разблокировки
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        // Обработчик блокировки/разблокировки
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                // Экран заблокирован. Если таймер работает и не на паузе — ставим на паузу
                if (isTimerRunning && !isPaused)
                {
                    timer.Stop();
                    pauseTimer.Start();
                    isPaused = true;

                    // Обновляем кнопки
                    button2.Enabled = false;
                    button3.Enabled = true;
                }
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                // Автоматически продолжаем, если была пауза
                if (isTimerRunning && isPaused)
                {
                    pauseTimer.Stop();
                    timer.Start();
                    isPaused = false;

                    button2.Enabled = true;
                    button3.Enabled = false;
                }
            }
        }

        // Отписываемся при закрытии формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            base.OnFormClosing(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("2 часа");
            comboBox1.Items.Add("4 часа");
            comboBox1.Items.Add("6 часов");
            comboBox1.Items.Add("8 часов");
            comboBox1.Items.Add("10 часов");
            comboBox1.Items.Add("12 часов");

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            pauseTimer = new Timer();
            pauseTimer.Interval = 1000;
            pauseTimer.Tick += PauseTimer_Tick;

            isPaused = false;
            isTimerRunning = false;
            totalPauseTime = TimeSpan.Zero;
            remainingTime = TimeSpan.Zero;

            button2.Enabled = false;
            button3.Enabled = false;

            UpdateTimerDisplay();
            UpdatePauseDisplay();
        }

        // Кнопка "Старт"
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите время!");
                return;
            }

            string selected = comboBox1.SelectedItem.ToString();
            int hours = int.Parse(selected.Split(' ')[0]);

            remainingTime = TimeSpan.FromHours(hours);

            totalPauseTime = TimeSpan.Zero;
            isPaused = false;
            isTimerRunning = true;

            UpdateTimerDisplay();
            UpdatePauseDisplay();
            timer.Start();

            button2.Enabled = true;
            button3.Enabled = false;
        }

        // Кнопка "Пауза"
        private void button2_Click(object sender, EventArgs e)
        {
            if (!isPaused && timer.Enabled)
            {
                timer.Stop();
                pauseTimer.Start();
                isPaused = true;

                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        // Кнопка "Продолжить"
        private void button3_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                pauseTimer.Stop();
                timer.Start();
                isPaused = false;

                button2.Enabled = true;
                button3.Enabled = false;
            }
        }

        // Тик основного таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                timer.Stop();
                isTimerRunning = false;

                MessageBox.Show("Время вышло!\n\nОбщее время паузы: " +
                    totalPauseTime.ToString(@"hh\:mm\:ss"));

                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        // Тик таймера паузы
        private void PauseTimer_Tick(object sender, EventArgs e)
        {
            totalPauseTime = totalPauseTime.Add(TimeSpan.FromSeconds(1));
            UpdatePauseDisplay();
        }

        private void UpdateTimerDisplay()
        {
            listBox2.Items.Clear();
            listBox2.Items.Add(remainingTime.ToString(@"hh\:mm\:ss"));
        }

        private void UpdatePauseDisplay()
        {
            label4.Text = "Общее время пауз: " + totalPauseTime.ToString(@"hh\:mm\:ss");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) 
        {

        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) 
        { 

        }
        private void label1_Click(object sender, EventArgs e) 
        {

        }
        private void label4_Click(object sender, EventArgs e) 
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}