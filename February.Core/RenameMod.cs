// ReSharper disable StringLiteralTypo
namespace February.Core
{
    public partial class RenameMod : Form
    {
        public string Mod = null!;
        public int Type;
        public string NewName = null!;

        public char[] IllegalChars = {'#', '%', '&', '{', '}', '\\', '<', '>', '*', '?', '/', '$', '!', '\'', '\"', ':', '@', '+', '`', '|', '=', ' '};

        public RenameMod()
        {
            InitializeComponent();
        }

        private void InstallMod_Load(object sender, EventArgs e)
        {
            textBox1.Text = Mod;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            switch (Type)
            {
                case 0:
                    File.Move($@".\GameSDK\Precache\ZZZZZ_ModLoad_{Mod}.pak", @$".\GameSDK\Precache\ZZZZZ_ModLoad_{textBox1.Text}.pak");
                    break;
                case 1:
                    File.Move($@".\Whiplash\GameSDK\Precache\ZZZZZ_ModLoad_{Mod}.pak", @$".\Whiplash\GameSDK\Precache\ZZZZZ_ModLoad_{textBox1.Text}.pak");
                    break;
            }
            
            NewName = textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (IllegalChars.Any(c => textBox1.Text.Contains(c)) || string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == Mod)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }
    }
}
