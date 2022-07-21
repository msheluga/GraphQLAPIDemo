namespace GraphQLAPIDemo
{
    public static class EntityTypes 
    {
        public static bool Initialized = false;
        public static List<Type> Types = new List<Type>();
        public static Dictionary<Type, List<Type>> TypeNavigations = new Dictionary<Type, List<Type>>();
        public static List<ObjectField> ObjectFields = new List<ObjectField>();
    }
}
