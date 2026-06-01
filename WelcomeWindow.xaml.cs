using System;
using System.Media;
using System.Windows;

namespace MzansiShield
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            PlayVoiceGreeting();
        }

        void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("myvoice.wav");
                player.Play();
            }
            catch
            {
                StatusText.Text = "Voice file not found.";
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                StatusText.Text = "Please enter your name first.";
                return;
            }

            ChatWindow chat = new ChatWindow(name);
            chat.Show();
            this.Close();
        }
    }
}