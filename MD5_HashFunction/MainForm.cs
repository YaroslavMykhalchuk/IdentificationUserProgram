using System.Security.Cryptography;
using System.Text;

namespace MD5_HashFunction
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        string filePath = "A:\\Development of decentralized applications\\MD5_HashFunction\\MD5_HashFunction\\user.txt";

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
                textBoxPassword.UseSystemPasswordChar = false;
            else
                textBoxPassword.UseSystemPasswordChar = true;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBoxName.Text.Length == 0 && textBoxPassword.Text.Length == 0) || (textBoxName.Text.Length == 0 || textBoxPassword.Text.Length == 0))
                {
                    MessageBox.Show("Заповніть порожні поля", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (textBoxPassword.Text.Length < 8 && textBoxName.Text.Length >= 4)
                {
                    MessageBox.Show("Пароль має бути довжиною більше 7 символів", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (textBoxPassword.Text.Length > 7 && textBoxName.Text.Length < 4)
                {
                    MessageBox.Show("Ім'я має бути довжиною більше 3 символів", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (ValidateUser(filePath, false))
                    {
                        MessageBox.Show("Ви успішно увійшли!", "Congratulation!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ви ввели невірнe ім'я чи пароль!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Registration_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBoxName.Text.Length == 0 && textBoxPassword.Text.Length == 0) || (textBoxName.Text.Length == 0 || textBoxPassword.Text.Length == 0))
                {
                    MessageBox.Show("Заповніть порожні поля", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else if (textBoxPassword.Text.Length < 8 && textBoxName.Text.Length >= 4)
                {
                    MessageBox.Show("Пароль має бути довжиною більше 7 символів", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (textBoxPassword.Text.Length > 7 && textBoxName.Text.Length < 4)
                {
                    MessageBox.Show("Ім'я має бути довжиною більше 3 символів", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!ValidateUser(filePath, true))
                    {
                        AddUser(textBoxName.Text, GetMd5Hash(textBoxPassword.Text), filePath);
                        MessageBox.Show("Ви успішно зареєструвалися!", "Congratulation!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxName.Clear();
                        textBoxPassword.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Користувач з таким ім'ям уже існує!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string GetMd5Hash(string password)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(password);
                var hashBytes = md5.ComputeHash(inputBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public bool ValidateUser(string filePath, bool workmode) //workmode: true if we check only name or false if we check name and password
        {
            FileStream readFileStream = new FileStream(filePath, FileMode.Open);
            StreamReader streamReader = new StreamReader(readFileStream);
            bool validate = false;

            try
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();

                    if (validate == false)
                    {
                        if (line != null)
                        {
                            var parts = line.Split(':');
                            if (!workmode)
                            {
                                if (textBoxName.Text == parts[0] && GetMd5Hash(textBoxPassword.Text) == parts[1])
                                    validate = true;
                            }
                            else
                            {
                                if (textBoxName.Text == parts[0])
                                    validate = true;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                streamReader.Close();
                readFileStream.Close();
            }
            return validate;
        }

        public void AddUser(string userName, string password, string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            try
            {
                streamWriter.WriteLine($"{textBoxName.Text}:{GetMd5Hash(textBoxPassword.Text)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                streamWriter.Close();
                fileStream.Close();
            }
        }

    }
}