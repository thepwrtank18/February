namespace February.Core
{
    public partial class InstallMod : Form
    {
        public string Mod = null!;
        public int Type;
        public string NewName = null!;

        public char[] IllegalChars = {'#', '%', '&', '{', '}', '\\', '<', '>', '*', '?', '/', '$', '!', '\'', '\"', ':', '@', '+', '`', '|', '=', ' '};

        public InstallMod()
        {
            InitializeComponent();
        }

        private void InstallMod_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            switch (Type)
            {
                case 0:
                    File.Copy(Mod, @$".\GameSDK\Precache\ZZZZZ_ModLoad_{textBox1.Text}.pak");
                    break;
                case 1:
                    File.Copy(Mod, @$".\Whiplash\GameSDK\Precache\ZZZZZ_ModLoad_{textBox1.Text}.pak");
                    break;
            }
            
            NewName = textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (IllegalChars.Any(c => textBox1.Text.Contains(c)) || string.IsNullOrWhiteSpace(textBox1.Text))
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
