using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

namespace SharpGodotFirebase.Firestore
{
    public static class FirestoreParser
    {
        private static Dictionary<string, int> props = new Dictionary<string, int>()
        {
            { "arrayValue", 1 },
            { "bytesValue", 1 },
            { "booleanValue", 1 },
            { "doubleValue", 1 },
            { "geoPointValue", 1 },
            { "integerValue", 1 },
            { "mapValue", 1 },
            { "nullValue", 1 },
            { "referenceValue", 1 },
            { "stringValue", 1 },
            { "timestampValue", 1 }
        };
        private static string[] propList = new string[11]
        {
            "arrayValue",
            "bytesValue",
            "booleanValue",
            "doubleValue",
            "geoPointValue",
            "integerValue",
            "mapValue",
            "nullValue",
            "referenceValue",
            "stringValue",
            "timestampValue"
        };


        private static object GetFirestoreProp(KeyValuePair<string, object> value)
        {
            if (propList.Contains(value.Key))
            {

            }
            else
            {

            }
            return "";
        }

        public static object Parse(object firestoreJson)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();

            if(firestoreJson is Dictionary<string, object> i)
            {
                foreach(KeyValuePair<string, object> pair in i)
                {
                    
                }
            }
            return false;
        }


    }
}
