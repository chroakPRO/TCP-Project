namespace Guldkortet
{
    //Creating subclasses to be able to create objects and find information easier.
    class Dunderkatt : Storage
    {
        public Dunderkatt(string serie, string win)
        {
            this.serie = serie;
            this.win = win;
            type = "Dunderkatt";
        }
        public override string ToString()
        {
            return "You have won\n the card Dunderkatt!";
        }
    }
    class Kristallhäst : Storage
    {
        public Kristallhäst(string serie, string win)
        {
            this.serie = serie;
            this.win = win;
            type = "Kristallhäst";
        }
        public override string ToString()
        {
            return "You have won\n the card Kristallhäst!";
        }
    }
    class Överpanda : Storage
    {
        public Överpanda(string serie, string win)
        {
            this.serie = serie;
            this.win = win;
            type = "Överpanda";
        }
        public override string ToString()
        {
            return "You have won\n the card Överpanda!";
        }
    }
    class Eldtomat : Storage
    {
        public Eldtomat(string serie, string win)
        {
            this.serie = serie;
            this.win = win;
            type = "Eldtomat";
            
        }
        public override string ToString()
        {
            return "You have won\n the card Eldtomat!";
        }
    }
}
