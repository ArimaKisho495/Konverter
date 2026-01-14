using System;
using System.Drawing;
using System.Windows.Forms;
// Это наше главное меню.
namespace Konverter
{
    public class Menu : Form
    {
        private Label title;
        private Button btnStart;
        private Button btnAbout;
        private Button btnExit;

        public Menu()
        {
            Text = "Конвертер величин";
            Width = 500;
            Height = 350;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            BackColor = Color.FromArgb(25, 25, 25); // тёмная тема

            title = new Label()
            {
                Text = "КОНВЕРТЕР ВЕЛИЧИН",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100
            };

            btnStart = CreateBlueButton("Перейти к конвертации");
            btnAbout = CreateBlueButton("О программе");
            btnExit = CreateBlueButton("Выход");

            btnStart.Top = 120;
            btnAbout.Top = 180;
            btnExit.Top = 240;

            btnStart.Click += (s, e) =>
            {
                Hide();
                new Konvertacia().Show();
            };

            btnAbout.Click += (s, e) =>
            {
                MessageBox.Show("Нелинейный модульный конвертер величин.\n" +
                                "Поддерживает логарифмы, интерполяцию и тригонометрию.\n" +
                                "Автор: Жадан М.А ИСС231.\n" +
                                "Преподаватель: Ильющенков Дмитрий Сергеевич.",
                                "О программе",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            };

            btnExit.Click += (s, e) => Application.Exit();

            Controls.Add(title);
            Controls.Add(btnStart);
            Controls.Add(btnAbout);
            Controls.Add(btnExit);
        }

        private Button CreateBlueButton(string text)
        {
            return new Button()
            {
                Text = text,
                Width = 300,
                Height = 45,
                Left = (ClientSize.Width - 300) / 2,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }
    }
}

