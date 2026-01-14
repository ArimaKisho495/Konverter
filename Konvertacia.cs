using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
//Это у нас зона с конвертацией.
namespace Konverter
{
    public class Konvertacia : Form
    {
        private ComboBox cbCategory = new();
        private ComboBox cbFrom = new();
        private ComboBox cbTo = new();
        private TextBox tbValue = new();
        private TextBox tbResult = new();
        private Button btnConvert = new();
        private Label lblFrom = new();
        private Label lblTo = new();
        private Label lblValue = new();
        private KonvertatorBaza engine = new();
        private Button btnExit = new();

        public Konvertacia()
        {
            Text = "Конвертер величин";
            Width = 650;
            Height = 240;

            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(30, 30, 30); // тёмная тема

            InitializeComponents();
            ApplyStyle();
            RegisterModules();
            WireEvents();

            LoadCategories();
        }

        private void InitializeComponents()
        {
            int margin = 15;
            int row1Y = 20;
            int row2Y = 65;

            lblValue.Text = "Значение:";
            lblValue.Left = margin;
            lblValue.Top = row1Y + 5;
            lblValue.Width = 90;

            tbValue.Left = lblValue.Right + 5;
            tbValue.Top = row1Y;
            tbValue.Width = 120;

            lblFrom.Text = "Из:";
            lblFrom.Left = tbValue.Right + 10;
            lblFrom.Top = row1Y + 5;
            lblFrom.Width = 25;

            cbFrom.Left = lblFrom.Right + 5;
            cbFrom.Top = row1Y;
            cbFrom.Width = 150;

            lblTo.Text = "В:";
            lblTo.Left = cbFrom.Right + 10;
            lblTo.Top = row1Y + 5;
            lblTo.Width = 20;

            cbTo.Left = lblTo.Right + 5;
            cbTo.Top = row1Y;
            cbTo.Width = 150;

            // Вторая линия
            cbCategory.Left = margin;
            cbCategory.Top = row2Y;
            cbCategory.Width = 200;

            btnConvert.Text = "Конвертировать";
            btnConvert.Left = cbCategory.Right + 15;
            btnConvert.Top = row2Y - 1;
            btnConvert.Width = 150;
            btnConvert.Height = 30;

            tbResult.Left = btnConvert.Right + 15;
            tbResult.Top = row2Y;
            tbResult.Width = 220;
            tbResult.ReadOnly = true;

            btnExit.Text = "Выход";
            btnExit.Width = 100;
            btnExit.Height = 30;

            // позиция кнопки — правый нижний угол
            btnExit.Left = Width - btnExit.Width - 40;
            btnExit.Top = Height - btnExit.Height - 80;

            btnExit.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

            Controls.Add(btnExit);

            // Единый шрифт
            Font f = new Font("Segoe UI", 10);
            cbCategory.Font = cbFrom.Font = cbTo.Font = tbValue.Font = tbResult.Font = f;
            btnConvert.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Controls.AddRange(new Control[] {
        lblValue, tbValue, lblFrom, cbFrom, lblTo, cbTo,
        cbCategory, btnConvert, tbResult

    });
        }

        private void ApplyStyle()
        {
            BackColor = Color.FromArgb(30, 30, 30);

            foreach (Control c in Controls)
            {
                if (c is Label)
                    c.ForeColor = Color.White;

                if (c is TextBox tb)
                {
                    tb.BackColor = Color.FromArgb(45, 45, 45);
                    tb.ForeColor = Color.White;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }

                if (c is ComboBox cb)
                {
                    cb.BackColor = Color.FromArgb(45, 45, 45);
                    cb.ForeColor = Color.White;
                    cb.FlatStyle = FlatStyle.Flat;
                }
            }

            btnConvert.BackColor = Color.FromArgb(0, 120, 215);
            btnConvert.ForeColor = Color.White;
            btnConvert.FlatStyle = FlatStyle.Flat;
            btnConvert.FlatAppearance.BorderSize = 0;

            btnExit.BackColor = Color.FromArgb(0, 120, 215);
            btnExit.ForeColor = Color.White;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            
        }

        private void RegisterModules()
        {
            engine.RegisterModule(new Logarifm());
            engine.RegisterModule(new Interpolyatia());
            engine.RegisterModule(new Trigonometria());
            engine.RegisterModule(new Discrete());
        }

        private void WireEvents()
        {
            cbCategory.SelectedIndexChanged += (s, e) => OnCategoryChanged();
            btnConvert.Click += (s, e) => DoConvert();
            tbValue.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoConvert(); };
            btnExit.Click += (s, e) =>
            {
                this.Hide();                 
                Menu menu = new Menu();      
                menu.Show();                 
            };
        }

        private void LoadCategories()
        {
            cbCategory.Items.Clear();
            foreach (var c in engine.Categories)
                cbCategory.Items.Add(c);

            if (cbCategory.Items.Count > 0)
                cbCategory.SelectedIndex = 0;
        }

        private void OnCategoryChanged()
        {
            if (cbCategory.SelectedItem == null) return;

            var units = engine.GetUnits(cbCategory.SelectedItem.ToString()).ToArray();

            cbFrom.Items.Clear();
            cbTo.Items.Clear();

            foreach (var u in units)
            {
                cbFrom.Items.Add(new ComboboxItem(u.Id, $"{u.Name} ({u.Symbol})"));
                cbTo.Items.Add(new ComboboxItem(u.Id, $"{u.Name} ({u.Symbol})"));
            }

            if (cbFrom.Items.Count > 0) cbFrom.SelectedIndex = 0;
            if (cbTo.Items.Count > 1) cbTo.SelectedIndex = 1;
            else if (cbTo.Items.Count > 0) cbTo.SelectedIndex = 0;
        }

        private void TryAutoConvert()
        {
            if (!string.IsNullOrWhiteSpace(tbValue.Text))
                DoConvert();
        }

        private void DoConvert()
        {
            try
            {
     
                if (string.IsNullOrWhiteSpace(tbValue.Text))
                {
                    MessageBox.Show("Поле ввода пустое. Введите числовое значение. :(");
                    return;
                }

                string raw = tbValue.Text.Trim().Replace(",", ".");

                
                if (raw.Any(ch => char.IsLetter(ch)))
                {
                    MessageBox.Show("Введены буквенные символы. Разрешены только числа. :(");
                    return;
                }

               
                if (!double.TryParse(raw,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out double val))
                {
                    MessageBox.Show("Введено некорректное значение. Используйте цифры. :(");
                    return;
                }

                
                if (cbCategory.SelectedItem == null ||
                    cbFrom.SelectedItem == null ||
                    cbTo.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите категорию и тип операции. :(");
                    return;
                }

                string category = cbCategory.SelectedItem.ToString();
                string from = ((ComboboxItem)cbFrom.SelectedItem).Id;
                string to = ((ComboboxItem)cbTo.SelectedItem).Id;

                
                if (category == "Логарифмы" && val <= 0)
                {
                    MessageBox.Show("Для логарифмов значение должно быть больше нуля. :(");
                    return;
                }

                
                var result = engine.Convert(category, from, to, val);

                
                tbResult.Text = result.ToString("G12",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка конвертации: " + ex.Message);
            }
        }


        private class ComboboxItem
        {
            public string Id { get; }
            public string Text { get; }

            public ComboboxItem(string id, string text)
            {
                Id = id;
                Text = text;
            }

            public override string ToString() => Text;
        }
    }
}
