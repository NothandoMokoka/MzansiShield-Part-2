using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MzansiShield
{
    public partial class ChatWindow : Window
    {
        string userName;
        string favouriteTopic = "";   
        string lastTopic = "";        
        Random random = new Random();

        // random responses for each topic
        Dictionary<string, List<string>> topicResponses = new Dictionary<string, List<string>>()
        {
            { "phishing", new List<string> {
                "Phishing emails often pretend to be from banks or big companies. Never click links in suspicious emails.",
                "If an email asks for your personal info urgently, it's likely phishing. Always verify with the real company.",
                "Check the sender's email address carefully. Phishers use fake addresses that look almost real.",
                "Never enter your password on a site you clicked to from an email. Go directly to the website instead."
            }},
            { "scam", new List<string> {
                "If it sounds too good to be true, it probably is. Never send money to strangers online.",
                "Scammers often create urgency to make you act fast. Take your time and verify first.",
                "Online job scams often ask you to pay upfront. Legitimate jobs don't do that.",
                "If someone contacts you out of nowhere offering prizes or money, it's almost always a scam."
            }},
            { "password", new List<string> {
                "Use a mix of letters, numbers and symbols. Avoid using your name or birthday.",
                "Never reuse the same password across different sites. If one gets hacked, others stay safe.",
                "A password manager can help you create and store strong, unique passwords.",
                "Enable two-factor authentication wherever you can. It adds an extra layer of security."
            }},
            { "privacy", new List<string> {
                "Review the privacy settings on your social media. Limit who can see your personal info.",
                "Avoid sharing too much personal info online. Scammers can use it against you.",
                "Use a VPN when on public Wi-Fi to keep your data private.",
                "Read app permissions before installing. Some apps ask for more access than they need."
            }},
            { "link", new List<string> {
                "Hover over a link before clicking to see where it actually goes.",
                "If a link looks suspicious, don't click it. Go directly to the website instead.",
                "Shortened URLs can hide dangerous sites. Use a URL expander to check first.",
                "Never click links sent by strangers on social media or messaging apps."
            }},
            { "email", new List<string> {
                "Be careful of emails asking you to reset passwords you didn't request.",
                "Legitimate companies will never ask for your password via email.",
                "Check for spelling mistakes in emails — phishers often make small errors.",
                "If an email creates panic or urgency, slow down and verify before acting."
            }}
        };

        // the keywords
        List<string> worriedWords = new List<string> { "worried", "scared", "afraid", "nervous", "anxious", "fear" };
        List<string> frustratedWords = new List<string> { "frustrated", "angry", "annoyed", "confused", "lost", "stuck" };
        List<string> curiousWords = new List<string> { "curious", "interested", "want to know", "tell me more", "explain more", "give me another" };

        public ChatWindow(string name)
        {
            InitializeComponent();
            userName = name;
            HeaderText.Text = $"Welcome, {name}!";
            AddBotMessage($"Hey {name}! I'm MzansiShield, your cybersecurity assistant.");
            AddBotMessage("Ask me about phishing, scams, passwords, privacy, links, or email.");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ProcessInput();
        }

        void ProcessInput()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddUserMessage(input);
            UserInput.Clear();

            string lower = input.ToLower();
            Respond(lower);
        }

        void Respond(string input)
        {
            if (input == "exit" || input == "quit" || input == "bye")
            {
                AddBotMessage($"Goodbye {userName}, stay safe online!");
                return;
            }

            string sentiment = DetectSentiment(input);

            if ((input.Contains("more") || input.Contains("explain") || input.Contains("another")) && lastTopic != "")
            {
                string tip = GetRandomResponse(lastTopic);
                AddBotMessage($"Sure! Here's another tip on {lastTopic}: {tip}");
                return;
            }

            if (input.Contains("favourite") || input.Contains("favorite") || input.Contains("i like") || input.Contains("i love") || input.Contains("interested in"))
            {
                foreach (string topic in topicResponses.Keys)
                {
                    if (input.Contains(topic))
                    {
                        favouriteTopic = topic;
                        AddBotMessage($"Got it! I'll remember that you're interested in {topic}. It's an important cybersecurity topic.");
                        lastTopic = topic;
                        return;
                    }
                }
            }

            if (input.Contains("email"))
            {
                lastTopic = "email";
                AddBotMessage($"{userName}, here's a scenario:");
                AddBotMessage("You receive an email: 'Your bank account is locked. Click here to unlock it.'");
                AddBotMessage("What do you do?\n1. Click the link\n2. Contact your bank directly\n\nType 1 or 2:");
                return;
            }

            if (input == "1" && lastTopic == "email")
            {
                AddBotMessage($"Wrong, {userName}! That is a phishing attempt. Never click links in suspicious emails.", isWarning: true);
                AddBotMessage(GetRandomResponse("email"));
                return;
            }

            if (input == "2" && lastTopic == "email")
            {
                AddBotMessage($"Correct, {userName}! Always verify directly with your bank. Great thinking!");
                AddBotMessage(GetRandomResponse("email"));
                return;
            }

            foreach (string topic in topicResponses.Keys)
            {
                if (input.Contains(topic))
                {
                    lastTopic = topic;

                    // sentiment prefix
                    if (sentiment == "worried")
                        AddBotMessage($"I understand you're concerned, {userName}. It's okay to feel that way. Here's what you should know:");
                    else if (sentiment == "frustrated")
                        AddBotMessage($"I hear you, {userName}. Let me make this simple and clear for you:");
                    else if (sentiment == "curious")
                        AddBotMessage($"Great that you're curious, {userName}! Here's something useful:");

                    AddBotMessage(GetRandomResponse(topic));

                    // reference memory
                    if (favouriteTopic != "" && favouriteTopic != topic)
                        AddBotMessage($"By the way, as someone interested in {favouriteTopic}, you might also want to keep an eye on {topic} threats.");

                    return;
                }
            }

            // greetings
            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
            {
                AddBotMessage($"Hey {userName}! How can I help you stay safe today?");
                return;
            }

            if (input.Contains("how are you") || input.Contains("how r u"))
            {
                AddBotMessage("I'm doing well, thanks for asking! Ready to help you stay cyber safe.");
                return;
            }

            if (input.Contains("thank") || input.Contains("thanks"))
            {
                AddBotMessage($"You're welcome, {userName}! Stay safe out there.");
                return;
            }

            AddBotMessage($"I'm not sure I understand that, {userName}. Can you try rephrasing? Try topics like phishing, scams, passwords, privacy, links, or email.", isWarning: true);
        }

        string GetRandomResponse(string topic)
        {
            if (topicResponses.ContainsKey(topic))
            {
                List<string> responses = topicResponses[topic];
                return responses[random.Next(responses.Count)];
            }
            return "I don't have info on that yet.";
        }

        string DetectSentiment(string input)
        {
            foreach (string word in worriedWords)
                if (input.Contains(word)) return "worried";

            foreach (string word in frustratedWords)
                if (input.Contains(word)) return "frustrated";

            foreach (string word in curiousWords)
                if (input.Contains(word)) return "curious";

            return "neutral";
        }

        void AddUserMessage(string message)
        {
            ChatDisplay.Inlines.Add(new Run($"\nYou: {message}\n") { Foreground = Brushes.White });
            ScrollToBottom();
        }

        void AddBotMessage(string message, bool isWarning = false)
        {
            Color color = isWarning ? Color.FromRgb(255, 80, 80) : Color.FromRgb(0, 255, 65);
            ChatDisplay.Inlines.Add(new Run($"MzansiShield: {message}\n") { Foreground = new SolidColorBrush(color) });
            ScrollToBottom();
        }

        void ScrollToBottom()
        {
            ChatScroll.ScrollToBottom();
        }
    }
}
// MzansiShield Part 2 - Cybersecurity Chatbot
