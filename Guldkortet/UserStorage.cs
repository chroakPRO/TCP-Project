namespace Guldkortet
{
    //Creating a class to be used to creating simple objects.
    class UserStorage : Storage
    {
        public UserStorage(string uid, string name, string city)
        {
            this.userid = uid;
            this.city = city;
            this.name = name;
        }
        public override string ToString()
        {
            return "0";
        }
    }
}
