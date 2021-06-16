namespace vaYolo.Model.Yolo
{
    public class ObjectClass 
    {
        public ObjectClass(int Class, string description)
        {
            _Class = 0;
            Description = "Undefined";
        }

        public int _Class {get; set;}

        public string Description {get; set;}
    }
}