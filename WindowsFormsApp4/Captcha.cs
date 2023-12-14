using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Captcha : Form
    {
        static string symbols  = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
        
        static Random r = new Random();
        
        
        public Captcha()
        {
            InitializeComponent();
            Captcha_Load(null, EventArgs.Empty);
            ApplyColorScheme();
        }
        
        private void ApplyColorScheme()
        {
            this.BackColor = Color.White;
            titleLabel.BackColor = Color.FromArgb(118, 227, 131);
            titleLabel.ForeColor = Color.White;
            panel1.BackColor = Color.FromArgb(118, 227, 131);
            inputButton.BackColor = Color.FromArgb(73, 140, 81); 
            inputButton.ForeColor = Color.White;
        }
        
        private char getRandomChar()
        {
            var i = r.Next(symbols.Length);
            return symbols[i];
        }

        private string GenerateRandomString()
        {
            char[] randomChars = new char[5];

            for (int i = 0; i < 5; i++)
            {
                randomChars[i] = getRandomChar();
            }

            return new string(randomChars);
        }
        
        

        private void Captcha_Load(object sender, EventArgs e)
        {
            string randomString = GenerateRandomString();
            capthcaText.Font = new Font(capthcaText.Font, FontStyle.Strikeout);

            capthcaText.Text = randomString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capthcaInput.Text == capthcaText.Text)
            {
                this.Close();
            }
            else
            {
                string randomString = GenerateRandomString();
                capthcaText.Text = randomString;
            }
        }
        
    }
}