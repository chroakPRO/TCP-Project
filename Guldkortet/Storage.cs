using System.Collections.Generic;
namespace Guldkortet
{
    /// <summary>
    /// This class is only used to set variables for Storage/Storage Sub-classes
    /// There is also a singleton component that stops the class
    /// from being called more then once. 
    /// The class is created as a storage and list is being used for that type of storage.
    /// </summary>
    public class Storage
    {
        //Using Singleton fundamentals to only start one object.
        private static Storage instance = new Storage();
        public static Storage Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new Storage();
                }
                return instance;
            }
        }
        //declaring and initalising lists and vars.
        public List<Storage> users = new List<Storage>();
        public List<Storage> cardwins = new List<Storage>();
        public string userid { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string serie { get; set; }
        public string win { get; set; }
        public string type { get; set; }
        public virtual string ToString()
        {
            return "Something Went Wrong!";
        }
        //Two methods that are being used to add objects onto a list.
        public string UserAdd(Storage objects)
        {
            users.Add(objects);
            return "Object Added";
        }
        public string CardAdd(Storage objects)
        {
            cardwins.Add(objects);
            return "Object Added!";
        }
    }
}
