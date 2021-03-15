using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Text;
namespace Guldkortet
{
    public class InfoHandle
    {
        public TcpClient client;
        public TcpListener reciever;
        private static InfoHandle instance = new InfoHandle();
        public static InfoHandle Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InfoHandle();
                }
                return instance;
            }
        }
        //Reading information from vektorstring list and adding it to a new list
        //With included object.
        public void StoreUserInfo(List<string[]> list)
        {
            int i;
            string userid;
            string city;
            string name;
            for (i = 0; i < list.Count; i++)
            {
                userid = list[i][0];
                name = list[i][1];
                city = list[i][2];
                Storage.Instance.UserAdd(new UserStorage(userid, name, city));
            }
        }
        //Same thing here.
        //But here we also add sub-classes so we can use ToString method.
        public void StoreCardInfo(List<string[]> list)
        {
            int i;
            string serie;
            string win;
            for (i = 0; i < list.Count; i++)
            {
                serie = list[i][0];
                win = list[i][1];
                if (win != "Dunderkatt" || win != "Kristallhäst" || 
                    win != "Överpanda" || win != "Eldtomat"  )
                {
                    if (win == "Dunderkatt")
                    {
                        Storage.Instance.CardAdd(new Dunderkatt(serie, win));
                    }
                    if (win == "Kristallhäst")
                    {
                        Storage.Instance.CardAdd(new Kristallhäst(serie, win));
                    }
                    if (win == "Överpanda")
                    {
                        Storage.Instance.CardAdd(new Överpanda(serie, win));
                    }
                    if (win == "Eldtomat")
                    {
                        Storage.Instance.CardAdd(new Eldtomat(serie, win));
                    }
                }
            }
        }
        //This is the heart of the operation is evaluate what type of information has been sent.
        //Then it performs a set of actions depending on the text.  
        private void EvaluateInfo(string p)
        {
            //Splits string to create a simple vektor.
            string[] eva = p.Split(new string[] { "-" },
                        StringSplitOptions.None);
            //Initalizing and declaring a couple of varibles.
            string user = eva[0];
            string card = eva[1];
            bool UserLegit;
            bool CardLegit;
            bool Continue = true;
            int cardmax = Storage.Instance.cardwins.Count - 1;
            int usermax = Storage.Instance.users.Count - 1;
            string city;
            string name;
            //Simple loop to check if the user & cardid is real or not.
            //Changes bool vars to send diffrent types of inputs to EvaluatePrice.
            //Which is used to determine what information will be displayed.
            //We need to use break; so the text dosent spam, very importent.
            for (int i = 0; i < Storage.Instance.users.Count; i++)
            {
                if (Storage.Instance.users[i].userid == user)
                {
                    for (int j = 0; j < Storage.Instance.cardwins.Count; j++)
                    {
                        if (Storage.Instance.cardwins[j].serie == card)
                        {
                            UserLegit = true;
                            CardLegit = true;
                            city = Storage.Instance.users[i].city;
                            name = Storage.Instance.users[i].name;
                            EvaluatePrice(user, name, city, card, CardLegit, UserLegit);
                            Continue = false;
                            break;
                        }
                        else if (j >= cardmax)
                        {
                            UserLegit = true;
                            CardLegit = false;
                            city = Storage.Instance.users[i].city;
                            name = Storage.Instance.users[i].name;
                            EvaluatePrice(user,name, city, card, CardLegit, UserLegit);
                            Continue = false;
                            break;
                        }
                    }
                }
            }
            if (Continue == true)
            {
                //This check for card information, if users fail.
                for (int i = 0; i < Storage.Instance.cardwins.Count; i++)
                {
                    if (Storage.Instance.cardwins[i].serie == card)
                    {
                        UserLegit = false;
                        CardLegit = true;
                        name = "";
                        city = "";
                        EvaluatePrice(user, name, city, card, CardLegit, UserLegit);
                        break;
                    }
                    //If there are no correct users or cards, everything is false;
                    else if (i >= cardmax)
                    {
                        CardLegit = false;
                        UserLegit = false;
                        name = "";
                        city = "";
                        EvaluatePrice(user, name, city, card, CardLegit, UserLegit);
                        break;
                    }
                }
            }
        }
        private void EvaluatePrice(string userid, string name, string city,
            string cardid, bool cardlegit, bool userlegit)
        {
            /*We could have just created sub-classes instead of this scheme.
             * Then we could have just used the ToString method to display information*/
            //Using another form to display popup windows.
            Form2 hello = new Form2();
            hello.label1.Font = new Font("Serif", 14, FontStyle.Bold);
            //Check and gives diffrent outputs depending on if the user/card id is legit or not.
            if (cardlegit == false && userlegit == true)
            {
                //Creating the string that will be used to send information to InfoSender.
                string p = "Hello" + name + " \nwith userid " + userid 
                    + " Is legit\n but cardid is not!\n Please Contact Support!";
                //Sending the information.
                //Setting what type of image that will be displayed in form2.
                hello.pictureBox1.Image = Image.FromFile("error.png");
                //Stretching the image so it fits the PictureBox
                hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                //changeing the label to show text.
                hello.label1.Text = "Hello" + name + " with userid " + userid 
                    + " Is legit\n but cardid is not!\n Please Contact Support!";               
                //Showing Form2 will all the settings above.
                hello.Show();
                MessageBox.Show("Hello" + name + " with userid " + userid 
                    + " Is legit\n but cardid is not!\n Please Contact Support!");
            }
            else if (userlegit == false && cardlegit == true)
            {
                string p = cardid + " Is legit\n but userid is not!\n Please Contact Support!";
                hello.pictureBox1.Image = Image.FromFile("error.png");
                hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                hello.label1.Text = "Hello" + name + " with userid " + userid + " Is not legit!\n but cardid is legit\n Please Contact Support!";
                hello.Show();
            }
            else if (cardlegit == false && userlegit == false)
            {
                string p = "Both your userid and cardid\n is not legit!\n Please Contact Support!";
                SendInfo(p);
                hello.pictureBox1.Image = Image.FromFile("error.png");
                hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                hello.label1.Text = "Both your userid and cardid\n" +
                    "is not legit Please Contact Support!";
                hello.Show();
            }
            else
            {
                //Creating a loop to check what type of win that form2 will display.
                //Using ToSTring method to display label text, polymohprism.
                for (int i = 0; i < Storage.Instance.cardwins.Count; i++)
                {
                    if (cardid == Storage.Instance.cardwins[i].serie)
                    {
                        string p = "User->" + userid + " You have won the price -> " 
                            + Storage.Instance.cardwins[i].win;
                        SendInfo(p);
                        if (Storage.Instance.cardwins[i].win == "Dunderkatt")
                        {
                            hello.pictureBox1.Image = Image.FromFile("dunderkatt.png");
                            hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            hello.label1.Text = "Congratz " + name + "\nfrom " + city + "\n" 
                                + Storage.Instance.cardwins[i].ToString();
                            hello.Show();
                            break;
                        }
                        else if (Storage.Instance.cardwins[i].win == "Kristallhäst")
                        {
                            hello.pictureBox1.Image = Image.FromFile("kristallhäst.png");
                            hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            hello.label1.Text = "Congratz " + name + "\nfrom " + city + "\n" 
                                + Storage.Instance.cardwins[i].ToString();
                            hello.Show();
                            break;
                        }
                        else if (Storage.Instance.cardwins[i].win == "Överpanda")
                        {
                            hello.pictureBox1.Image = Image.FromFile("överpanda.png");
                            hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            hello.label1.Text = "Congratz " + name + "\nfrom " + city + "\n" 
                                + Storage.Instance.cardwins[i].ToString();
                            hello.Show();
                            break;
                        }
                        else if (Storage.Instance.cardwins[i].win == "Eldtomat")
                        {
                            hello.pictureBox1.Image = Image.FromFile("eldtomat.png");
                            hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            hello.label1.Text = "Congratz " + name + "\nfrom " + city + "\n" 
                                + Storage.Instance.cardwins[i].ToString();
                            hello.Show();
                        }
                        else
                        {
                            hello.pictureBox1.Image = Image.FromFile("error.png");
                            hello.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            hello.label1.Text = Storage.Instance.cardwins[i].ToString();
                            hello.Show();
                            break;
                        }
                    }
                }
            }
        }
        /*
         * Instead of having two different classes.
         * Called infosender and inforeciever
         * I had to put all the sending and reciving into one file.
         */
        public void StartEverything()
        {
            try
            {
                //Skapa ett nytt objektet 
                IPAddress adress = IPAddress.Parse("127.0.0.1");
                reciever = new TcpListener(adress, 12345);
                reciever.Start();
            }
            catch (Exception l) { MessageBox.Show(l.Message); return; }
            //Testa om vi redan är anslutna, om inte anslut.
            StartReceiver();
        }
        private void StartReceiver()
        {
            try
            {
                client = reciever.AcceptTcpClient();
            }
            catch (Exception e) { MessageBox.Show(e.Message); return; }
            StartReading(client);
        }
        private async void StartReading(TcpClient x)
        {
            //Creating a byte buffer of 1024, so we can send information.(Memory management)
            byte[] buffer = new byte[1024];
            int n = 0;
            try
            {
                n = await x.GetStream().ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            //Putting the information into a textbox.
            string l = Encoding.Unicode.GetString(buffer, 0, n);         
            //Grabbing that info and sending to InfoReader!
            //I would be able to just send string instant but instead I display it to the backend.
            //To display some sort of info etc.
            EvaluateInfo(l);
            StartReading(x);
        }
        private async void SendInfo(string p)
        {
            byte[] utData = Encoding.Unicode.GetBytes(p);
            try
            { 
                await client.GetStream().WriteAsync(utData, 0, utData.Length);
            }
            catch (Exception pero)
            {
                MessageBox.Show("Can't Send message " + pero.Message);
            }
        }
    }
}
