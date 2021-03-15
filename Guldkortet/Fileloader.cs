using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Guldkortet
{
    class Fileloader
    {
        //creating two diffrent list to add information into.
        List<string[]> cardstorage = new List<string[]>();
        List<string[]> userstorage = new List<string[]>();
    //This list will seperate the user and the card number
        // We are using string vektors to add the information
        public void UserLoader(string filename)
        {
            //Creating a filestream to load information from file.
            try
            {
                StreamReader userread = new StreamReader(
                filename, Encoding.Default, true);
                //Goes through all the lines, until end
                while (!userread.EndOfStream)
                {
                    //Splits the string into multiple.
                    string content = userread.ReadLine();
                    string[] vektor = content.Split(new string[] { "###" },
                        StringSplitOptions.None);
                    //Add stringvektor to list.
                    userstorage.Add(vektor);
                }
                //Removeing pointers.
                userread.Dispose();
                //Sending the information onwards towards info reader.
                //So the information can be proccessed.
                InfoHandle.Instance.StoreUserInfo(userstorage);
            }
            //Error handling.
            catch (Exception error)
            {
                MessageBox.Show("The File could not be loaded!" + error.Message);
            }
        }
        //Same thing here we are spliting the string, and then adding the 
        //Diffrent information into two indexs of a string vektor.
        public void CardLoader(string filename)
        {
            try
            {
                StreamReader cardread = new StreamReader(
                  filename, Encoding.Default, true);
                while (!cardread.EndOfStream)
                {
                    string content = cardread.ReadLine();
                    string[] vektor = content.Split(new string[] { "###" },
                        StringSplitOptions.None);
                    cardstorage.Add(vektor);
                }
                //Removeing pointers.
                cardread.Dispose();
                //Sending the information onwards towards info reader.
                //So the information can be proccessed.
                InfoHandle.Instance.StoreCardInfo(cardstorage);
            }
            catch (Exception error)
            {
                MessageBox.Show("The File could not be loaded!" + error.Message);
            }
        }
    }
}
